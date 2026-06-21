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
    public class TransferenciaItem : RNBase
    {
        public const string Aceita = "Aceita";
        public const string Recusada = "Recusada";
        public const string Pendente = "Pendente";

        public DataTable ListaHistoricoTransferenciaPor(int bemId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT E.TRANSFERENCIAITEMID, 
                                    E.TRANSFERENCIAID, 
                                    REPLICATE('0',6 - LEN(e.NUMEROBEMORIGEM)) + CONVERT(VARCHAR(6), e.NUMEROBEMORIGEM) AS NUMERO, 
                                    B.DESCRICAO AS BEM, 
                                    B.CLASSIFICACAOID, 
                                    C.CONTA,
                                    C.DESCRICAO AS CLASSIFICACAO, 
                                    E.SITUACAO, 
                                    T.SETORDESTINO, 
                                    SD.NOME     AS SETORDESTINODESCRICAO, 
                                    T.SETORORIGEM, 
                                    SO.NOME     AS SETORORIGEMDESCRICAO, 
                                    E.JUSTIFICATIVA, 
                                    CONVERT(VARCHAR,T.DATAANDAMENTO,103) AS DATAANDAMENTO,
                                    CONVERT(VARCHAR,T.DATAMOVIMENTACAO,103) AS DATAMOVIMENTACAO,
                                    CONVERT(VARCHAR,T.DATASOLICITACAO,103) AS DATASOLICITACAO,
									ED.CONCEITO AS ESTADOCONSERVACAO,
									E.VALOR,
									E.MOEDAID,
									MO.SIGLA + CONVERT(VARCHAR(100), E.VALOR)  AS VALORCOMSIGLA,
                                    MO.SIGLA
                            FROM   PATRIMONIO.TRANSFERENCIA T (NOLOCK) 
                                    INNER JOIN PATRIMONIO.TRANSFERENCIAITEM E (NOLOCK) 
                                            ON T.TRANSFERENCIAID = E.TRANSFERENCIAID 
									INNER JOIN Patrimonio.MOEDA MO (NOLOCK) 
                                            ON MO.MOEDAID = E.MOEDAID
                                    INNER JOIN HADES.DBO.HD_SETOR SO (NOLOCK) 
                                            ON T.SETORORIGEM = SO.SETOR 
                                    INNER JOIN HADES.DBO.HD_SETOR SD (NOLOCK) 
                                            ON T.SETORDESTINO = SD.SETOR 
                                    INNER JOIN PATRIMONIO.BEM B (NOLOCK) 
                                            ON E.BEMID = B.BEMID 
                                    INNER JOIN PATRIMONIO.BEMVALOR I (NOLOCK) 
                                            ON B.BEMID = I.BEMID 
                                                AND CONVERT(DATE, T.DATAMOVIMENTACAO) BETWEEN I.DATAINICIO AND ISNULL(I.DATAFIM, CONVERT(DATE, GETDATE()))
									INNER JOIN PATRIMONIO.ESTADOCONSERVACAO ED  (NOLOCK)  
											ON ED.ESTADOCONSERVACAOID = I.ESTADOCONSERVACAOID
                                    INNER JOIN PATRIMONIO.CLASSIFICACAO C (NOLOCK) 
                                            ON B.CLASSIFICACAOID = C.CLASSIFICACAOID 
                           WHERE B.BEMID = @BEMID 
									AND DATAANDAMENTO IS NOT NULL
                            ORDER BY T.DATAANDAMENTO DESC  ";

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

        public ValidacaoDados ValidaAndamento(List<Entidades.TransferenciaItem> listaTransferenciaItem, string setorDestino, string usuarioAndamento, DateTime dataMovimentacao)
        {
            List<string> mensagens = new List<string>();
            string[] numeroDescricao = new string[2];
            int quantidadeItens = 0;
            RN.Patrimonio.Bem rnBem = new Bem();
            RN.Patrimonio.Movimentacao rnMovimentacao = new Movimentacao();
            Entidades.Bem bem = new Techne.Lyceum.RN.Patrimonio.Entidades.Bem();
            DateTime ultimaDataInicio;
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (listaTransferenciaItem.Count() == 0)
            {
                mensagens.Add("É obrigatório ACEITAR / RECUSAR ao menos 1 item.");
            }

            if (setorDestino.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo UNIDADE ADMINISTRATIVA DESTINO é obrigatório.");
            }

            if (dataMovimentacao == DateTime.MinValue)
            {
                mensagens.Add("O campo DATA DE TRANSFERÊNCIA é obrigatório.");
            }

            //Valida cada item da lista
            foreach (Entidades.TransferenciaItem item in listaTransferenciaItem)
            {
                if (item.TransferenciaItemId <= 0)
                {
                    mensagens.Add("O campo CODIGO DO ITEM DA TRANSFERÊNCIA é obrigatório.");
                }

                if (item.TransferenciaId <= 0)
                {
                    mensagens.Add("O campo CODIGO DA TRANSFERÊNCIA é obrigatório.");
                }

                if (item.BemId <= 0)
                {
                    mensagens.Add("O campo CODIGO DO ITEM é obrigatório.");
                }

                if (item.Valor == null || item.Valor <= 0)
                {
                    mensagens.Add("O campo VALOR ATUAL DO ITEM é obrigatório.");
                }

                if (item.MoedaId == null || item.MoedaId <= 0)
                {
                    mensagens.Add("O campo MOEDA DO ITEM é obrigatório.");
                }

                if (item.Situacao != Aceita && item.Situacao != Recusada)
                {
                    mensagens.Add("O campo SITUAÇÃO (Aceita / Recusada) é obrigatório para todos os itens.");
                }

                if (item.Situacao == Recusada && item.Justificativa.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Em todos os itens recusados o campo JUSTIFICATIVA é obrigatório.");
                }

                if (usuarioAndamento.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo USUARIO ANDAMENTO é obrigatório.");
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se todos os itens são de uma msm transferencia (lote)
                    if (listaTransferenciaItem.Select(x => x.TransferenciaId).Distinct().Count() > 1)
                    {
                        mensagens.Add("Apenas é possível Aceitar / Recursar 1 lote por vez.");
                    }
                    else
                    {
                        //Busca quantidade de itens da transferencia
                        quantidadeItens = this.ObtemQuantidadeItensPor(contexto, listaTransferenciaItem.Select(x => x.TransferenciaId).First());

                        //Verifica se todos os itens da transferencia estão na lista
                        if (quantidadeItens != listaTransferenciaItem.Count())
                        {
                            mensagens.Add("Todos os itens do lote devem ser Aceitos / Recursados.");
                        }
                    }

                    foreach (Entidades.TransferenciaItem item in listaTransferenciaItem)
                    {
                        //Busca dados do bem
                        bem = rnBem.ObtemPor(contexto, item.BemId);

                        //Busca dados de identificacao do item
                        numeroDescricao = rnBem.ObtemNumeroDescricaoPor(contexto, item.BemId);

                        //Verifica se ainda existe uma transferencia pendente para o item
                        if (!this.ExisteSolicitacaoPendentePor(contexto, item.BemId))
                        {
                            mensagens.Add(string.Format("O item [0} - {1} não está mais pendente.", numeroDescricao[0], numeroDescricao[1]));
                        }

                        if(dataMovimentacao <= bem.DataAquisicao)
                        {
                            mensagens.Add(string.Format("O campo DATA DE TRANSFERÊNCIA deve ser maior que {0} - data de aquisicao do item [1} - {2}.", bem.DataAquisicao.ToString("dd/MM/yyyy"), numeroDescricao[0], numeroDescricao[1]));
                        }
                        else
                        {
                            //Busca data inicio na ua atual
                            ultimaDataInicio = rnMovimentacao.ObtemInicioMovimentacaoAtivaPor(contexto, item.BemId);

                            if (dataMovimentacao <= ultimaDataInicio)
                            {
                                mensagens.Add(string.Format("O campo DATA DE TRANSFERÊNCIA deve ser maior que {0} - data de entrada do item [1} - {2} na unidade administrativa de origem.", ultimaDataInicio.ToString("dd/MM/yyyy"), numeroDescricao[0], numeroDescricao[1]));
                            }
                        }
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

        private int ObtemQuantidadeItensPor(DataContext contexto, int transferenciaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT COUNT(*) AS QUANTIDADE
                                        FROM Patrimonio.TRANSFERENCIAITEM (NOLOCK) 
                                        WHERE  TRANSFERENCIAID = @TRANSFERENCIAID ";

                contextQuery.Parameters.Add("@TRANSFERENCIAID", SqlDbType.Int, transferenciaId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["QUANTIDADE"]);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return retorno;
        }

        public bool PossuiTransferenciaPendenteAbertaPor(DataContext contexto, int bemId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @"   SELECT count(1)
                                        FROM PATRIMONIO.TRANSFERENCIAITEM
                                        WHERE BEMID = @BEMID
	                                          AND SITUACAO IN ('Pendente', 'Aceita') ";

            contextQuery.Parameters.Add("@BEMID", SqlDbType.Int, bemId); 

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;               
            }

            return existe;
        }

        public bool PossuiTransferenciaPendentePor(DataContext contexto, int bemId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @"   SELECT count(1)
                                        FROM PATRIMONIO.TRANSFERENCIAITEM
                                        WHERE BEMID = @BEMID
	                                          AND SITUACAO = 'Pendente' ";

            contextQuery.Parameters.Add("@BEMID", SqlDbType.Int, bemId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable ListaTransferenciaPendenteAbertaPor(int bemId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT SITUACAO, COUNT(1) AS QUANTIDADE
                                        FROM PATRIMONIO.TRANSFERENCIAITEM
                                        WHERE BEMID = @BEMID
	                                          AND SITUACAO IN ('Pendente', 'Aceita')
										GROUP BY SITUACAO ";

                contextQuery.Parameters.Add("@BEMID", SqlDbType.Int, bemId); 

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
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
                contexto.Dispose();
            }

            return dt;
        }

        public bool ExisteSolicitacaoPendentePor(DataContext contexto, int bemId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM   Patrimonio.TRANSFERENCIAITEM (NOLOCK) 
                                    WHERE  BEMID = @BEMID
	                                       AND SITUACAO = @PENDENTE ";

            contextQuery.Parameters.Add("@BEMID", SqlDbType.Int, bemId);
            contextQuery.Parameters.Add("@PENDENTE", SqlDbType.VarChar, TransferenciaItem.Pendente);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(DataContext contexto, Entidades.TransferenciaItem transferenciaItem)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Patrimonio.TRANSFERENCIAITEM
	                                    (TRANSFERENCIAID, 
                                        BEMID, 
	                                    NUMEROBEMORIGEM,
                                        SITUACAO) 
                                    SELECT @TRANSFERENCIAID, 
                                           b.BEMID, 
	                                       mv.NUMERO,
                                           @SITUACAO 
                                    FROM  Patrimonio.BEM b (NOLOCK)
											INNER JOIN PATRIMONIO.MOVIMENTACAO Mv (NOLOCK) 
                                       ON B.BEMID = Mv.BEMID 
                                          AND ( Mv.DATAFIM IS NULL 
                                                 OR DATAFIM >= GETDATE() ) 
                                    WHERE b.BEMID  = @BEMID  ";

            contextQuery.Parameters.Add("@TRANSFERENCIAID", SqlDbType.Int, transferenciaItem.TransferenciaId);
            contextQuery.Parameters.Add("@SITUACAO", SqlDbType.VarChar, Pendente);
            contextQuery.Parameters.Add("@BEMID", SqlDbType.Int, transferenciaItem.BemId);

            contexto.ApplyModifications(contextQuery);
        }

        public void Transfere(List<Entidades.TransferenciaItem> listaTransferenciaItem, string setorDestino, string usuarioAndamento, DateTime dataMovimentacao)
        {
            RN.Patrimonio.Movimentacao rnMovimentacao = new Movimentacao();
            Entidades.Movimentacao movimentacao = new Techne.Lyceum.RN.Patrimonio.Entidades.Movimentacao();
            RN.Patrimonio.Transferencia rnTransferencia = new Transferencia();
            int proximoNumero = 0;
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Insere itens
                foreach (Entidades.TransferenciaItem transferenciaItem in listaTransferenciaItem)
                {
                    //Caso transferencia seja aceita, atualiza dados do item
                    if (transferenciaItem.Situacao == Aceita)
                    {
                        //Busca proximo numero para Bem
                        proximoNumero = rnMovimentacao.ObtemProximoNumeroPor(contexto, setorDestino);

                        //Encerra movimentacao atual do bem com data atual - 1
                        rnMovimentacao.FinalizaMovimentacaoAtiva(contexto, transferenciaItem.BemId, dataMovimentacao.AddDays(-1), usuarioAndamento);

                        //Cria proxima movimentacao do bem começando com data atual
                        movimentacao.BemId = transferenciaItem.BemId;
                        movimentacao.Numero = proximoNumero;
                        movimentacao.Setor = setorDestino;
                        movimentacao.DataInicio = dataMovimentacao;
                        movimentacao.UsuarioId = usuarioAndamento;
                        rnMovimentacao.Insere(contexto, movimentacao);
                    }

                    //Atualiza dados da transferencia
                    rnTransferencia.Atualiza(contexto, transferenciaItem.TransferenciaId, usuarioAndamento, DateTime.Now, dataMovimentacao);

                    //Atualiza TransferenciaItem
                    this.Atualiza(contexto, transferenciaItem);
                }
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
                contexto.Dispose();
            }
        }

        private void Atualiza(DataContext contexto, Entidades.TransferenciaItem transferenciaItem)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Patrimonio.TRANSFERENCIAITEM
                                       SET SITUACAO = @SITUACAO,
                                          JUSTIFICATIVA = @JUSTIFICATIVA,
                                          VALOR = @VALOR,
                                          MOEDAID = @MOEDAID
                                     WHERE TRANSFERENCIAITEMID = @TRANSFERENCIAITEMID  ";

            contextQuery.Parameters.Add("@SITUACAO", SqlDbType.VarChar, transferenciaItem.Situacao);
            contextQuery.Parameters.Add("@JUSTIFICATIVA", SqlDbType.VarChar, transferenciaItem.Justificativa);
            contextQuery.Parameters.Add("@VALOR", SqlDbType.Decimal, transferenciaItem.Valor);
            contextQuery.Parameters.Add("@MOEDAID", SqlDbType.Decimal, transferenciaItem.MoedaId);
            contextQuery.Parameters.Add("@TRANSFERENCIAITEMID", SqlDbType.Int, transferenciaItem.TransferenciaItemId);

            contexto.ApplyModifications(contextQuery);
        }

        public int ObtemQuantidadeSolicitacaoPendentePor(string setor)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            int quantidade = 0;
            try
            {
                contextQuery.Command = @" SELECT COUNT(*) AS QUANTIDADE
                                          FROM   PATRIMONIO.TRANSFERENCIA T (NOLOCK) 
                                          INNER JOIN PATRIMONIO.TRANSFERENCIAITEM E (NOLOCK) 
                                            ON T.TRANSFERENCIAID = E.TRANSFERENCIAID 
                                          WHERE  SETORDESTINO = @SETORDESTINO 
                                             AND SITUACAO = @PENDENTE";

                contextQuery.Parameters.Add("@SETORDESTINO", SqlDbType.VarChar, setor);
                contextQuery.Parameters.Add("@PENDENTE", SqlDbType.VarChar, TransferenciaItem.Pendente);

                quantidade = contexto.GetReturnValue<int>(contextQuery);

                return quantidade;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
                contexto.Dispose();
            }
        }
    }
}