using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.FiscalizacaoLink
{
    public class Interrupcao
    {
        public DataTable ListaPor(int avaliacaoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT INTERRUPCAOID, 
                                                I.AVALIACAOID, 
                                                A.CIRCUITOSETORID, 
												C.DESCRICAO AS DESCRICAOCONTRATO,
												cs.DESIGNACAO as DESIGNACAOCIRCUITO,
                                                CHAMADO, 
                                                DATAINTERRUPCAO, 
		                                        CONVERT(VARCHAR(20), DATAINTERRUPCAO, 108) AS HORAINTERRUPCAO,
                                                DATAREESTABELECIMENTO, 
		                                        CASE 
			                                        WHEN DATAREESTABELECIMENTO IS NOT NULL THEN CONVERT(VARCHAR(20), DATAREESTABELECIMENTO, 108)
			                                        ELSE NULL
		                                        END HORAREESTABELECIMENTO,
                                                TIPOPROBLEMA, 
												I.MOTIVOINTERRUPCAOID,
												M.DESCRICAO AS MOTIVO,
												I.MOTIVOCOMPLEMENTO,
                                                I.USUARIOID, 
                                                I.DATACADASTRO, 
                                                I.DATAALTERACAO,
                                                A.ENVIOFATURAMENTO 
                                        FROM   FISCALIZACAOLINK.INTERRUPCAO I (NOLOCK)
										       INNER JOIN FISCALIZACAOLINK.AVALIACAO A (NOLOCK) 
																		ON I.AVALIACAOID = A.AVALIACAOID
											   INNER JOIN FISCALIZACAOLINK.CIRCUITOSETOR CS (NOLOCK) 
																		ON A.CIRCUITOSETORID = CS.CIRCUITOSETORID
											   INNER JOIN FISCALIZACAOLINK.CONTRATOSETOR CO (NOLOCK) 
																		ON CS.CONTRATOSETORID = CO.CONTRATOSETORID
											   INNER JOIN FISCALIZACAOLINK.CONTRATO C (NOLOCK) 
																		ON CO.CONTRATOID = C.CONTRATOID
											   LEFT JOIN FISCALIZACAOLINK.MOTIVOINTERRUPCAO M (NOLOCK)
																		ON I.MOTIVOINTERRUPCAOID = M.MOTIVOINTERRUPCAOID
                                        WHERE  I.AVALIACAOID = @AVALIACAOID ";

                contextQuery.Parameters.Add("@AVALIACAOID", SqlDbType.Int, avaliacaoId);

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

        public bool ExisteInterrupcaoPor(DataContext contexto, int avaliacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM  FiscalizacaoLink.INTERRUPCAO (NOLOCK)
                                    WHERE  AVALIACAOID = @AVALIACAOID ";

            contextQuery.Parameters.Add("@AVALIACAOID", SqlDbType.Int, avaliacaoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public ValidacaoDados Valida(RN.FiscalizacaoLink.Entidades.Interrupcao interrupcao, int ano, int mes, bool cadastro, int circuitoSetorId)
        {
            List<string> mensagens = new List<string>();
            RN.FiscalizacaoLink.Avaliacao rnAvaliacao = new Avaliacao();
            List<string> validacaoCamposGeraisInterrupcao = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (interrupcao == null)
            {
                return validacaoDados;
            }

            if (!cadastro)
            {
                if (interrupcao.InterrupcaoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }

                if (interrupcao.AvaliacaoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO DA AVALIAÇÃO é obrigatório.");
                }
            }

            if (interrupcao.Chamado.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NÚMERO DO CHAMADO é obrigatório.");
            }

            if (interrupcao.MotivoInterrupcaoId <= 0)
            {
                mensagens.Add("Campo MOTIVO é obrigatório.");
            }

            //Verifica se tem motivo
            if (!interrupcao.MotivoComplemento.IsNullOrEmptyOrWhiteSpace())
            {
                if (interrupcao.MotivoComplemento.Length > 5000)
                {
                    mensagens.Add("Campo COMPLEMENTO DO MOTIVO deve conter no máximo 500 caracteres.");
                }
            }

            //Valida campos obrigatorios gerais da Interrupcao
            validacaoCamposGeraisInterrupcao = this.ValidaCamposGerais(interrupcao, ano, mes, circuitoSetorId);
            if (validacaoCamposGeraisInterrupcao.Count > 0)
            {
                mensagens.AddRange(validacaoCamposGeraisInterrupcao);
            }
            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se avaliação pode ter interrupcao
                    if (!rnAvaliacao.PossuiOpcaoInterrupcaoPor(contexto, interrupcao.AvaliacaoId))
                    {
                        mensagens.Add("Para cadastar uma INTERRUPÇÃO é necessário marcar SIM no campo 'houve interrupção'.");
                    }

                    //Valida se já foi enviado para faturamento
                    if (rnAvaliacao.PossuiEnvioFaturamentoPor(contexto, interrupcao.AvaliacaoId))
                    {
                        mensagens.Add("Esta avaliação não pode ser editada pois já foi enviada para faturamento.");
                    }

                    //Verifica se existe interrupcao com msm chamado
                    if (cadastro && this.PossuiInterrupcaoPor(contexto, interrupcao.Chamado))
                    {
                        mensagens.Add("Já existe uma interrupção cadastrada com este número de chamado.");
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
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public List<string> ValidaCamposGerais(RN.FiscalizacaoLink.Entidades.Interrupcao interrupcao, int ano, int mes, int circuitoSetorId)
        {
            List<string> mensagens = new List<string>();
            RN.FiscalizacaoLink.CircuitoSetor rnCircuitoSetor = new CircuitoSetor();
            RN.FiscalizacaoLink.Entidades.CircuitoSetor circuitoSetor = new Entidades.CircuitoSetor();

            if (ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (mes <= 0)
            {
                mensagens.Add("Campo MÊS é obrigatório.");
            }

            if (circuitoSetorId <= 0)
            {
                mensagens.Add("Campo CIRCUITO / CONTRATO é obrigatório.");
            }
            else
            {
                //Busca dados do circuito
                circuitoSetor = rnCircuitoSetor.ObtemPor(circuitoSetorId);

                if (circuitoSetor.CircuitoSetorId <= 0)
                {
                    mensagens.Add("Campo CIRCUITO / CONTRATO é inválido.");
                }
            }

            if (interrupcao.DataInterrupcao == null || interrupcao.DataInterrupcao <= DateTime.MinValue)
            {
                mensagens.Add("Campo DATA/HORA INTERRUPÇÃO é obrigatório.");
            }
            else
            {
                if (interrupcao.DataInterrupcao > DateTime.Now)
                {
                    mensagens.Add("A DATA/HORA DE INTERRUPÇÃO não pode ser maior que a data atual.");
                }

                //Verifica se a data esta no ano / mes
                if (interrupcao.DataInterrupcao.Month != mes || interrupcao.DataInterrupcao.Year != ano)
                {
                    mensagens.Add("A DATA/HORA DE INTERRUPÇÃO deve estar no ano e mês selecionados.");
                }

                if (circuitoSetor.CircuitoSetorId > 0)
                {
                    //Valida se a data escolhida em "Data/Hora Interrupção" está dentro do período de vigência do circuito escolhido, 
                    if (interrupcao.DataInterrupcao.Date < circuitoSetor.Inicio || 
                        (circuitoSetor.Fim != null && circuitoSetor.Fim != DateTime.MinValue && interrupcao.DataInterrupcao.Date > Convert.ToDateTime(circuitoSetor.Fim).Date))
                    {
                        mensagens.Add("A DATA DE INTERRUPÇÃO não está no período em que o link/circuito indicado estava ativo. Por favor, verifique as informações e ajuste, caso necessário.");
                    }
                }


                if (interrupcao.DataReestabelecimento != null && interrupcao.DataReestabelecimento > DateTime.MinValue)
                {
                    if (Convert.ToDateTime(interrupcao.DataReestabelecimento) > DateTime.Now)
                    {
                        mensagens.Add("A DATA/HORA DE REESTABELECIMENTO não pode ser maior que a data atual.");
                    }

                    if (Convert.ToDateTime(interrupcao.DataReestabelecimento) <= interrupcao.DataInterrupcao)
                    {
                        mensagens.Add("A DATA/HORA DE REESTABELECIMENTO deve ser maior que a DATA/HORA INTERRUPÇÃO.");
                    }

                    if (circuitoSetor.CircuitoSetorId > 0)
                    {
                        //Valida se a data escolhida em "Data/Hora Restabelecimento" está dentro do período de vigência do circuito escolhido, 
                        if (Convert.ToDateTime(interrupcao.DataReestabelecimento).Date < circuitoSetor.Inicio ||
                            (circuitoSetor.Fim != null && circuitoSetor.Fim != DateTime.MinValue && Convert.ToDateTime(interrupcao.DataReestabelecimento).Date > Convert.ToDateTime(circuitoSetor.Fim).Date))
                        {
                            mensagens.Add("A DATA DE RESTABELECIMENTO não está no período em que o link/circuito indicado estava ativo. Por favor, verifique as informações e ajuste, caso necessário.");
                        }
                    }
                }
            }

            if (interrupcao.TipoProblema.IsNullOrEmptyOrWhiteSpace()
                || (interrupcao.TipoProblema != "Interno" && interrupcao.TipoProblema != "Externo"))
            {
                mensagens.Add("Campo TIPO DE PROBLEMA é obrigatório.");
            }

            if (interrupcao.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO RESPONSAVEL é obrigatório.");
            }

            return mensagens;
        }

        public bool PossuiMotivoInterrupcaoPor(DataContext contexto, int motivoInterrupcaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM FISCALIZACAOLINK.INTERRUPCAO 
                                    where MOTIVOINTERRUPCAOID = @MOTIVOINTERRUPCAOID ";

            contextQuery.Parameters.Add("@MOTIVOINTERRUPCAOID", motivoInterrupcaoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }       

        public bool PossuiInterrupcaoPor(DataContext contexto, string chamado)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM FISCALIZACAOLINK.INTERRUPCAO (NOLOCK)
                                        WHERE CHAMADO = @CHAMADO";

            contextQuery.Parameters.Add("@CHAMADO", SqlDbType.VarChar, chamado);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(RN.FiscalizacaoLink.Entidades.Interrupcao interrupcao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                this.Insere(contexto, interrupcao);
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

        public void Insere(DataContext contexto, RN.FiscalizacaoLink.Entidades.Interrupcao interrupcao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO FiscalizacaoLink.INTERRUPCAO
                                               (AVALIACAOID                                               
                                               ,CHAMADO
                                               ,DATAINTERRUPCAO
                                               ,DATAREESTABELECIMENTO
                                               ,TIPOPROBLEMA
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO
                                               ,MOTIVOINTERRUPCAOID
                                               ,MOTIVOCOMPLEMENTO)
                                         VALUES
                                               (@AVALIACAOID, 
                                               @CHAMADO, 
                                               @DATAINTERRUPCAO, 
                                               @DATAREESTABELECIMENTO, 
                                               @TIPOPROBLEMA, 
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO, 
                                               @MOTIVOINTERRUPCAOID, 
                                               @MOTIVOCOMPLEMENTO) ";

            contextQuery.Parameters.Add("@AVALIACAOID", SqlDbType.Int, interrupcao.AvaliacaoId);            
            contextQuery.Parameters.Add("@CHAMADO", SqlDbType.VarChar, interrupcao.Chamado);
            contextQuery.Parameters.Add("@DATAINTERRUPCAO", SqlDbType.DateTime, interrupcao.DataInterrupcao);
            contextQuery.Parameters.Add("@TIPOPROBLEMA", SqlDbType.VarChar, interrupcao.TipoProblema);
            contextQuery.Parameters.Add("@MOTIVOINTERRUPCAOID", SqlDbType.Int, interrupcao.MotivoInterrupcaoId);
            contextQuery.Parameters.Add("@MOTIVOCOMPLEMENTO", SqlDbType.VarChar, interrupcao.MotivoComplemento);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, interrupcao.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            if (interrupcao.DataReestabelecimento != null && interrupcao.DataReestabelecimento > DateTime.MinValue)
            {
                contextQuery.Parameters.Add("@DATAREESTABELECIMENTO", SqlDbType.DateTime, interrupcao.DataReestabelecimento);
            }
            else
            {
                contextQuery.Parameters.Add("@DATAREESTABELECIMENTO", SqlDbType.DateTime, null);
            }

            contexto.ApplyModifications(contextQuery);
        }

        public void Atualiza(RN.FiscalizacaoLink.Entidades.Interrupcao interrupcao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE FISCALIZACAOLINK.INTERRUPCAO 
                                        SET    DATAINTERRUPCAO = @DATAINTERRUPCAO, 
                                               DATAREESTABELECIMENTO = @DATAREESTABELECIMENTO, 
                                               TIPOPROBLEMA = @TIPOPROBLEMA,
                                               MOTIVOINTERRUPCAOID = @MOTIVOINTERRUPCAOID,
                                               MOTIVOCOMPLEMENTO = @MOTIVOCOMPLEMENTO,
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  INTERRUPCAOID = @INTERRUPCAOID ";

                contextQuery.Parameters.Add("@INTERRUPCAOID", SqlDbType.Int, interrupcao.InterrupcaoId);
                contextQuery.Parameters.Add("@DATAINTERRUPCAO", SqlDbType.DateTime, interrupcao.DataInterrupcao);
                contextQuery.Parameters.Add("@MOTIVOINTERRUPCAOID", SqlDbType.Int, interrupcao.MotivoInterrupcaoId);
                contextQuery.Parameters.Add("@MOTIVOCOMPLEMENTO", SqlDbType.VarChar, interrupcao.MotivoComplemento);
                contextQuery.Parameters.Add("@TIPOPROBLEMA", SqlDbType.VarChar, interrupcao.TipoProblema);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, interrupcao.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                if (interrupcao.DataReestabelecimento != null && interrupcao.DataReestabelecimento > DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAREESTABELECIMENTO", SqlDbType.DateTime, interrupcao.DataReestabelecimento);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAREESTABELECIMENTO", SqlDbType.DateTime, null);
                }

                contexto.ApplyModifications(contextQuery);
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

        public ValidacaoDados ValidaRemocao(int interrupcaoId, int avalizacaoId, string usuarioid)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (interrupcaoId <= 0)
            {
                mensagens.Add("Campo CÓDIGO DA INTERRUPÇÃO é obrigatório.");
            }

            if (avalizacaoId <= 0)
            {
                mensagens.Add("Campo CÓDIGO DA AVALIAÇÃO é obrigatório.");
            }

            if (usuarioid.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO RESPONSAVEL é obrigatório.");
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public int ObtemQuantidadeInterrupcoesPor(int avaliacaoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            int retorno = 0;

            try
            {
                retorno = this.ObtemQuantidadeInterrupcoesPor(contexto, avaliacaoId);
                return retorno;
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

        private int ObtemQuantidadeInterrupcoesPor(DataContext contexto, int avaliacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT COUNT(*) AS QTDE
                                    FROM FISCALIZACAOLINK.INTERRUPCAO (NOLOCK)
                                    WHERE AVALIACAOID = @AVALIACAOID ";

                contextQuery.Parameters.Add("@AVALIACAOID", SqlDbType.Int, avaliacaoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["QTDE"]);
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

        public void Remove(int interrupcaoId, int avalizacaoId, string usuarioid)
        {
            RN.FiscalizacaoLink.Avaliacao rnAvaliacao = new Avaliacao();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Remover interrupcao
                this.Remove(contexto, interrupcaoId);

                //Verifica se ainda existe alguma interrupcao para a avaliação
                if (this.ObtemQuantidadeInterrupcoesPor(contexto, avalizacaoId) <= 0)
                {
                    //Caso não exista altera resposta para Não possui interrupcao
                    rnAvaliacao.RetiraOpcaoInterrupcao(contexto, avalizacaoId, usuarioid);
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

        private void Remove(DataContext contexto, int interrupcaoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE FISCALIZACAOLINK.INTERRUPCAO 
                                        WHERE  INTERRUPCAOID = @INTERRUPCAOID ";

            contextQuery.Parameters.Add("@INTERRUPCAOID", SqlDbType.Int, interrupcaoId);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
