using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Ocorrencias
{
    public class OcorrenciaInterrupcao
    {
        public DataTable ListaInterrupcaoPor(int ocorrenciaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT OCORRENCIAINTERRUPCAOID, 
								OCORRENCIAID, 
								DATAINTERRUPCAO, 
								USUARIOID, 
								DATACADASTRO, 
								DATAALTERACAO,
								CASE
									WHEN MANHA = 1 THEN 'Sim'
									ELSE 'Não'
								END MANHA,
								CASE
									WHEN TARDE = 1 THEN 'Sim'
									ELSE 'Não'
								END TARDE,
								CASE
									WHEN NOITE = 1 THEN 'Sim'
									ELSE 'Não'
								END NOITE
                                FROM Ocorrencias.OCORRENCIAINTERRUPCAO
                                WHERE OCORRENCIAID = @OCORRENCIAID
                                ORDER BY DATAINTERRUPCAO ";

                contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, ocorrenciaId);

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

        public ValidacaoDados Valida(Entidades.OcorrenciaInterrupcao ocorrenciaInterrupcao, DateTime dataOcorrencia)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (ocorrenciaInterrupcao == null)
            {
                return validacaoDados;
            }

            if (ocorrenciaInterrupcao.OcorrenciaId <= 0)
            {
                mensagens.Add("Campo CÓDIGO OCORRÊNCIA é obrigatório.");
            }

            if (ocorrenciaInterrupcao.DataInterrupcao < dataOcorrencia)
            {
                mensagens.Add("Campo DATA DA INTERRUPÇÃO não pode ser menor que a DATA DA OCORRÊNCIA.");
            }

            if (!ocorrenciaInterrupcao.Manha && !ocorrenciaInterrupcao.Tarde && !ocorrenciaInterrupcao.Noite)
            {
                mensagens.Add("Cada DATA DA INTERRUPÇÃO precisa ter ao menos um turno selecionado.");
            }

            if (ocorrenciaInterrupcao.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se o dia ja foi cadastrado para a ocorrencia
                    if (this.PossuiDataInterrupcaoPor(contexto, ocorrenciaInterrupcao.OcorrenciaId, ocorrenciaInterrupcao.DataInterrupcao))
                    {
                        mensagens.Add("A mesma DATA DA INTERRUPÇÃO não pode ser adicionada mais de uma vez.");
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

        private bool PossuiDataInterrupcaoPor(DataContext contexto, int ocorrenciaId, DateTime dataInterrupcao)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;


            contextQuery.Command = @" SELECT COUNT(1) 
                                        FROM Ocorrencias.OCORRENCIAINTERRUPCAO
                                        WHERE OCORRENCIAID = @OCORRENCIAID 
                                        AND DATAINTERRUPCAO = @DATAINTERRUPCAO ";

            contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, ocorrenciaId);
            contextQuery.Parameters.Add("@DATAINTERRUPCAO", SqlDbType.DateTime, dataInterrupcao);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public void Insere(Entidades.OcorrenciaInterrupcao ocorrenciaInterrupcao)
        {
            RN.Ocorrencias.Ocorrencia rnOcorrencia = new Ocorrencia();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Atualiza Ocorrencia
                rnOcorrencia.AtualizaInterrupcao(contexto, ocorrenciaInterrupcao.OcorrenciaId, ocorrenciaInterrupcao.UsuarioId);

                //Insere Interrupcao
                this.Insere(contexto, ocorrenciaInterrupcao);
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

        public void Insere(DataContext contexto, Ocorrencias.Entidades.OcorrenciaInterrupcao ocorrenciaInterrupcao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Ocorrencias.OCORRENCIAINTERRUPCAO
                                                       (OCORRENCIAID
                                                       ,DATAINTERRUPCAO
                                                       ,MANHA 
                                                       ,TARDE 
                                                       ,NOITE
                                                       ,USUARIOID
                                                       ,DATACADASTRO
                                                       ,DATAALTERACAO)
                                                 VALUES
                                                       (@OCORRENCIAID, 
                                                       @DATAINTERRUPCAO, 
                                                       @MANHA, 
                                                       @TARDE, 
                                                       @NOITE,  
                                                       @USUARIOID, 
                                                       @DATACADASTRO, 
                                                       @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, ocorrenciaInterrupcao.OcorrenciaId);
            contextQuery.Parameters.Add("@DATAINTERRUPCAO", SqlDbType.Date, ocorrenciaInterrupcao.DataInterrupcao);
            contextQuery.Parameters.Add("@MANHA", SqlDbType.Bit, ocorrenciaInterrupcao.Manha);
            contextQuery.Parameters.Add("@TARDE", SqlDbType.Bit, ocorrenciaInterrupcao.Tarde);
            contextQuery.Parameters.Add("@NOITE", SqlDbType.Bit, ocorrenciaInterrupcao.Noite);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, ocorrenciaInterrupcao.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void Remove(int ocorrenciaInterrupcaoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE Ocorrencias.OCORRENCIAINTERRUPCAO
                            WHERE  OCORRENCIAINTERRUPCAOID = @OCORRENCIAINTERRUPCAOID
                                    ";

                contextQuery.Parameters.Add("@OCORRENCIAINTERRUPCAOID", SqlDbType.Int, ocorrenciaInterrupcaoId);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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
        }
    }
}
