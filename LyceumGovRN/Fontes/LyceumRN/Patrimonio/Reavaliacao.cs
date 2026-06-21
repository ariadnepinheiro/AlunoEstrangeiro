using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.Patrimonio
{
    public class Reavaliacao
    {
        public bool PossuiMoedaPor(DataContext ctx, int moedaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PATRIMONIO.REAVALIACAO (NOLOCK)
                                    WHERE MOEDAID = @MOEDAID ";

            contextQuery.Parameters.Add("@MOEDAID", SqlDbType.Int, moedaId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable ListaPor(int bemId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT R.REAVALIACAOID, 
                                           v.BEMID, 
                                           R.DATAREAVALIACAO, 
                                           R.VIDAADICIONAL, 
                                           R.VALORMERCADO, 
                                           M.SIGLA + CONVERT(VARCHAR(100), R.VALORMERCADO)  AS VALORMERCADOCOMSIGLA,
                                           V.VALOR    AS VALORGERADO, 
	                                       M.SIGLA + CONVERT(VARCHAR(100), V.VALOR)  AS VALORGERADOCOMSIGLA,
                                           V.ESTADOCONSERVACAOID, 
                                           CASE R.INSERVIVEL WHEN 1 THEN 'Sim' ELSE 'Não' END INSERVIVEL,
                                           E.CONCEITO AS ESTADOCONSERVACAO, 
                                           V.VIDAUTIL AS VIDAUTIL 
                                    FROM   PATRIMONIO.REAVALIACAO R (NOLOCK) 
                                           LEFT JOIN PATRIMONIO.MOEDA M (NOLOCK) 
                                                   ON R.MOEDAID = M.MOEDAID 
                                           INNER JOIN PATRIMONIO.BEMVALOR V (NOLOCK) 
                                                   ON R.BEMVALORID = V.BEMVALORID
                                           INNER JOIN PATRIMONIO.ESTADOCONSERVACAO E (NOLOCK) 
                                                   ON E.ESTADOCONSERVACAOID = V.ESTADOCONSERVACAOID 
                                    WHERE  V.BEMID = @BEMID 
                                    ORDER  BY R.DATAREAVALIACAO DESC  ";

                contextQuery.Parameters.Add("@BEMID", SqlDbType.Int, bemId);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }

            return dt;
        }

        public ValidacaoDados ValidaCadastro(DTOs.DadosBemReavaliacao reavaliacao, string setor)
        {
            RN.Patrimonio.PeriodoLancamento rnPeriodoLancamento = new PeriodoLancamento();
            RN.Patrimonio.Movimentacao rnMovimentacao = new Movimentacao();
            RN.Patrimonio.TransferenciaItem rnTransferenciaItem = new TransferenciaItem();
            DataContext contexto = null;
            RN.Perfil rnPerfil = new Perfil();
            List<string> mensagens = new List<string>();
            RN.Patrimonio.Bem rnBem = new Bem();
            RN.Patrimonio.BemValor rnBemValor = new BemValor();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (reavaliacao == null)
            {
                return validacaoDados;
            }

            reavaliacao.DataReavaliacao = DateTime.Now.Date;

            if (reavaliacao.BemId <= 0)
            {
                mensagens.Add("Campo CÓDIGO é obrigatório.");
            }

            if (reavaliacao.ClassificacaoId <= 0)
            {
                mensagens.Add("Campo CLASSIFICACAO é obrigatório.");
            }

            if (reavaliacao.Inservivel == null)
            {
                mensagens.Add("Campo INSERVÍVEL é obrigatório.");
            }
            else
            {
                if (!Convert.ToBoolean(reavaliacao.Inservivel))
                {
                    if (reavaliacao.EstadoconservacaoId == null || reavaliacao.EstadoconservacaoId <= 0)
                    {
                        mensagens.Add("Campo ESTADO DE CONSERVAÇÃO é obrigatório.");
                    }

                    if (reavaliacao.VidaAdicional == null || reavaliacao.VidaAdicional <= 0)
                    {
                        mensagens.Add("Campo VIDA ADICIONAL é obrigatório.");
                    }

                    if (reavaliacao.ValorMercado == null || reavaliacao.ValorMercado <= 0)
                    {
                        mensagens.Add("Campo VALOR DE MERCADO é obrigatório.");
                    }

                    if (reavaliacao.DataAquisicao <= DateTime.MinValue)
                    {
                        mensagens.Add("Campo DATA DA AQUISIÇÃO é obrigatório.");
                    }

                    if (reavaliacao.VidaAdicional == null || reavaliacao.MoedaId <= 0)
                    {
                        mensagens.Add("Campo MOEDA é obrigatório.");
                    }
                }
                else
                {
                    if (reavaliacao.Processo.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo PROCESSO é obrigatório.");
                    }
                    else
                    {
                        if (reavaliacao.Processo.Length > 100)
                        {
                            mensagens.Add("O campo PROCESSO deve ter no máximo de 100 caracteres.");
                        }
                    }

                    reavaliacao.EstadoconservacaoId = null;
                    reavaliacao.VidaAdicional = null;
                    reavaliacao.ValorMercado = null;
                    reavaliacao.MoedaId = null;
                }
            }

            if (reavaliacao.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO RESPONSÁVEL é obrigatório.");
            }

            if (!rnPerfil.PossuiPerfilLiberacaoPatrimonioFinalizadoPor(reavaliacao.UsuarioId))
            {
                if (!rnPeriodoLancamento.PossuiPeriodoLancamentoAbertoPor(reavaliacao.DataReavaliacao.Year, DateTime.Now.Date))
                {
                    mensagens.Add("A DATA DA REAVALIAÇÃO está fora do intervalo permitido para lançamento.");
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se o item ja tem baixa     
                    if (rnBem.PossuiBaixaPor(contexto, reavaliacao.BemId))
                    {
                        mensagens.Add("Não é possível reavaliar este item pois ele já tem uma baixa cadastrada.");
                    }

                    //Verifica se existe outra reavaliação na mesma data
                    if (this.PossuiReavaliacaoPor(contexto, reavaliacao.DataReavaliacao, reavaliacao.BemId))
                    {
                        mensagens.Add("Já existe uma reavaliação cadastrada nesta data.");
                    }
                    else
                    {
                        //Verifica se o bem ao menos ficou 1 dia na ua
                        DateTime ultimaDataInicio = rnBemValor.ObtemInicioValorAtivoPor(contexto, reavaliacao.BemId);
                        if (ultimaDataInicio.Date >= reavaliacao.DataReavaliacao.Date)
                        {
                            mensagens.Add("Não é possível reavaliar este item pois a data da reavaliação deve ser maior que a data de aquisição / última reavaliação.");
                        }
                    }

                    if (reavaliacao.UltimoValorAtualizado >= 0)
                    {
                        var taxa = rnBem.RetornaTaxaValorResidualVigentePor(contexto, reavaliacao.BemId);

                        if (reavaliacao.ValorMercado < ((reavaliacao.UltimoValorAtualizado / 100) * taxa))
                        {
                            mensagens.Add("Não é possível reavaliar este item pois o valor de mercado está abaixo do valor permitido.");
                        }
                    }                   

                    //Verifica quem é o dono atual do bem
                    string setorAtual = rnMovimentacao.ObtemSetorVigentePor(contexto, reavaliacao.BemId, DateTime.Now);
                    if (setorAtual != setor)
                    {
                        mensagens.Add("Este bem não pode ser reavaliado pois possui não pertence a esta unidade.");
                    }

                    //Verifica se possui transferencia pendente
                    if (rnTransferenciaItem.PossuiTransferenciaPendentePor(contexto, reavaliacao.BemId))
                    {
                        mensagens.Add("Este bem não pode ser reavaliado pois possui transferência pendente.");
                    }
                }

                catch (Exception ex)
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (contexto != null)
                    {
                        contexto.Dispose();
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        private bool PossuiReavaliacaoPor(DataContext contexto, DateTime data, int bemId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PATRIMONIO.REAVALIACAO R (NOLOCK)
									INNER JOIN  PATRIMONIO.BEMVALOR V (NOLOCK) 
													ON R.BEMVALORID = V.BEMVALORID
                                    WHERE R.DATAREAVALIACAO = @DATA
										  AND V.BEMID = @BEMID ";

            contextQuery.Parameters.Add("@DATA", SqlDbType.Date, data.Date);
            contextQuery.Parameters.Add("@BEMID", SqlDbType.Int, bemId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(DTOs.DadosBemReavaliacao dadosReavaliacao)
        {
            RN.Patrimonio.BemValor rnBemValor = new BemValor();
            RN.Patrimonio.Movimentacao rnMovimentacao = new Movimentacao();
            RN.Patrimonio.Bem rnBem = new Bem();
            DTOs.DadosBaixaBemPatrimonial dadosBaixa = new Techne.Lyceum.RN.DTOs.DadosBaixaBemPatrimonial();
            RN.Patrimonio.Entidades.BemValor bemValor = new Techne.Lyceum.RN.Patrimonio.Entidades.BemValor();
            RN.Patrimonio.Entidades.Reavaliacao reavaliacao = new Techne.Lyceum.RN.Patrimonio.Entidades.Reavaliacao();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            int vidaUtilizada = 0;

            try
            {
                //Monta entidade reavaliação
                reavaliacao.Inservivel = Convert.ToBoolean(dadosReavaliacao.Inservivel);
                reavaliacao.MoedaId = dadosReavaliacao.MoedaId;
                reavaliacao.VidaAdicional = dadosReavaliacao.VidaAdicional;
                reavaliacao.ValorMercado = dadosReavaliacao.ValorMercado;
                reavaliacao.DataReavaliacao = dadosReavaliacao.DataReavaliacao;
                reavaliacao.UsuarioId = dadosReavaliacao.UsuarioId;

                //Verifica se bem foi considerado Inservivel
                if (Convert.ToBoolean(dadosReavaliacao.Inservivel))
                {
                    //Monta dados baixa
                    dadosBaixa.Baixa = true;
                    dadosBaixa.BemId = dadosReavaliacao.BemId;
                    dadosBaixa.DataBaixa = dadosReavaliacao.DataReavaliacao;
                    dadosBaixa.MotivoBaixaId = (int)RN.Patrimonio.MotivoBaixa.EnumMotivoBaixa.Inservivel;
                    dadosBaixa.ProcessoBaixa = dadosReavaliacao.Processo;
                    dadosBaixa.JustificativaBaixa = "Reavaliado como inservível";
                    dadosBaixa.BoletimOcorrencia = null;
                    dadosBaixa.CnpjInstituicaoDestino = null;
                    dadosBaixa.InstituicaoDestino = null;
                    dadosBaixa.UsuarioId = dadosReavaliacao.UsuarioId;

                    //Efetua baixa do bem
                    rnBem.AtualizaDadosBaixa(contexto, dadosBaixa);

                    //Alimenta bemValorid
                    reavaliacao.BemValorId = rnBemValor.ObtemBemValorAtivoPor(contexto, dadosBaixa.BemId);

                    //Finaliza Valor Bem Ativo
                    rnBemValor.FinalizaBemValorAtivo(contexto, dadosBaixa.BemId, dadosBaixa.DataBaixa, dadosBaixa.UsuarioId);

                    //Finaliza Movimentacao Ativa
                    rnMovimentacao.FinalizaMovimentacaoAtiva(contexto, dadosBaixa.BemId, dadosBaixa.DataBaixa, dadosBaixa.UsuarioId);
                }
                else
                {
                    //Encerra Valor Bem atual do bem com data atual - 1
                    rnBemValor.FinalizaBemValorAtivo(contexto, dadosReavaliacao.BemId, dadosReavaliacao.DataReavaliacao.AddDays(-1), dadosReavaliacao.UsuarioId);

                    //Cria proximo Valor Bem do bem começando com data atual              
                    bemValor.BemId = dadosReavaliacao.BemId;
                    bemValor.MoedaId = Convert.ToInt32(dadosReavaliacao.MoedaId);
                    bemValor.EstadoconservacaoId = Convert.ToInt32(dadosReavaliacao.EstadoconservacaoId);
                    bemValor.VidaUtil = Convert.ToInt32(reavaliacao.VidaAdicional);
                    bemValor.DataInicio = dadosReavaliacao.DataReavaliacao;
                    bemValor.UsuarioId = dadosReavaliacao.UsuarioId;

                    //Calcula vida já utilizada
                    vidaUtilizada = DateTime.Now.Year - dadosReavaliacao.DataAquisicao.Year;

                    //Calcula Valor
                    bemValor.Valor = this.ObtemValorReavaliadoPor(contexto, bemValor.EstadoconservacaoId, vidaUtilizada, bemValor.VidaUtil, Convert.ToDecimal(dadosReavaliacao.ValorMercado), dadosReavaliacao.ClassificacaoId);

                    //Insere Valor
                    rnBemValor.Insere(contexto, bemValor);

                    //Alimenta bemValorid
                    reavaliacao.BemValorId = bemValor.BemValorId;
                }

                //Insere reavaliação
                this.Insere(contexto, reavaliacao);
            }
            catch (Exception ex)
            {
                if (contexto != null)
                {
                    contexto.Abandon();
                }
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    if (Convert.ToString(ex.Message).Contains("ERRO:"))
                    {
                        mensagem = ex.Message.Replace("ERRO: ", string.Empty);
                    }
                    else
                    {
                        mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine,
                            Convert.ToString(ex.Message));
                    }
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }
        }

        private void Insere(DataContext contexto, Entidades.Reavaliacao reavaliacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Patrimonio.REAVALIACAO
                                                (BEMVALORID, 
                                                 INSERVIVEL,                                                 
                                                 MOEDAID, 
                                                 VIDAADICIONAL, 
                                                 VALORMERCADO, 
                                                 DATAREAVALIACAO, 
                                                 USUARIOID, 
                                                 DATACADASTRO, 
                                                 DATAALTERACAO) 
                                    VALUES      (@BEMVALORID,
                                                 @INSERVIVEL,                                                 
                                                 @MOEDAID, 
                                                 @VIDAADICIONAL, 
                                                 @VALORMERCADO, 
                                                 @DATAREAVALIACAO, 
                                                 @USUARIOID, 
                                                 @DATACADASTRO, 
                                                 @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@BEMVALORID", SqlDbType.Int, reavaliacao.BemValorId);
            contextQuery.Parameters.Add("@INSERVIVEL", SqlDbType.Bit, reavaliacao.Inservivel);
            contextQuery.Parameters.Add("@MOEDAID", SqlDbType.Int, reavaliacao.MoedaId);
            contextQuery.Parameters.Add("@VIDAADICIONAL", SqlDbType.Int, reavaliacao.VidaAdicional);
            contextQuery.Parameters.Add("@VALORMERCADO", SqlDbType.Decimal, reavaliacao.ValorMercado);
            contextQuery.Parameters.Add("@DATAREAVALIACAO", SqlDbType.Date, reavaliacao.DataReavaliacao.Date);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, reavaliacao.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public decimal ObtemValorReavaliadoPor(DataContext contexto, int estadoConservacaoId, int anosVidaUtilizado, int anosVidaFutura, decimal valorMercado, int classificacaoId)
        {
            RN.Patrimonio.EstadoConservacao rnEstadoConservacao = new EstadoConservacao();
            RN.Patrimonio.PeriodoVidaUtilizado rnPeriodoVidaUtilizado = new PeriodoVidaUtilizado();
            RN.Patrimonio.PeriodoVidaFutura rnPeriodoVidaFutura = new PeriodoVidaFutura();
            RN.Patrimonio.Classificacao rnClassificacao = new Classificacao();
            int EC = 0;
            int PVU = 0;
            int PUB = 0;
            decimal fatorReavaliacao = 0;
            decimal valorAtualizado = 0;
            decimal taxaResidual = 0;
            decimal valorResidual = 0;

            try
            {
                //Ajusta valores minimos e máximos para busca de pontuação
                if (anosVidaUtilizado < 1)
                {
                    anosVidaUtilizado = 1;
                }
                else if (anosVidaUtilizado > 10)
                {
                    anosVidaUtilizado = 10;
                }

                //Busca Pontuação do estado de conservação
                EC = rnEstadoConservacao.ObtemPontuacaoPor(contexto, estadoConservacaoId);

                //Busca pontuação da vida utilizada
                PVU = rnPeriodoVidaUtilizado.ObtemPontuacaoPor(contexto, anosVidaUtilizado);

                //Busca pontuação da vida futura
                PUB = rnPeriodoVidaFutura.ObtemPontuacaoPor(contexto, anosVidaFutura);

                // Fator de reavaliação (%) = 4 x EC + 6 x PVU – 3 x PUB
                fatorReavaliacao = 4 * EC + 6 * PVU - 3 * PUB;

                //Valor Atualizado = Valor de Mercado x Fator de reavaliação (%)
                valorAtualizado = valorMercado * (fatorReavaliacao / 100);

                //Busca taxa residual
                taxaResidual = rnClassificacao.RetornaValorResidualVigentePor(contexto, classificacaoId);

                //Calcula residual do bem
                valorResidual = valorMercado * (taxaResidual / 100);

                //O Valor Atualizado não poderá ser igual ou menor que o valor residual do bem 
                if (valorAtualizado <= valorResidual)
                {
                    throw new Exception("ERRO: Valor calculado é igual ou menor que o valor residual do bem. Refaça a operação!");
                }

                return valorAtualizado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}