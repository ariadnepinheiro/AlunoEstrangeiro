using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class PeriodoReferenciaExtratoBancario
    {
        public DataTable Lista()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT  PERIODOREFERENCIAEXTRATOBANCARIOID, 
		                                        DIAREFERENCIA, 	                                 
		                                        DATAINICIO, 
		                                        DATAFIM, 
		                                        USUARIOID, 
		                                        DATACADASTRO, 
		                                        DATAALTERACAO
                                        FROM PrestacaoContas.PERIODOREFERENCIAEXTRATOBANCARIO (NOLOCK)
                                        ORDER BY PERIODOREFERENCIAEXTRATOBANCARIOID ";

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

        public ValidacaoDados Valida(Entidades.PeriodoReferenciaExtratoBancario periodoReferenciaExtratoBancario, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (periodoReferenciaExtratoBancario == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (periodoReferenciaExtratoBancario.PeriodoReferenciaExtratoBancarioId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (periodoReferenciaExtratoBancario.DiaReferencia <= 0)
            {
                mensagens.Add("Campo DIA REFERÊNCIA é obrigatório.");
            }

            if (periodoReferenciaExtratoBancario.DataInicio == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INÍCIO é obrigatório.");
            }
            else if (periodoReferenciaExtratoBancario.DataFim != null && periodoReferenciaExtratoBancario.DataFim != DateTime.MinValue)
            {
                if (periodoReferenciaExtratoBancario.DataInicio > periodoReferenciaExtratoBancario.DataFim)
                {
                    mensagens.Add("A DATA INÍCIO não pode ser superior a DATA FIM.");
                }
            }

            if (periodoReferenciaExtratoBancario.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                 try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (PossuiPeriodoReferenciaVigentePor(contexto,periodoReferenciaExtratoBancario.PeriodoReferenciaExtratoBancarioId ))
                    {
                        mensagens.Add("Já existe um período de referência do extrato bancário vigente.");
                    }

                    //Verifica se as datas de inicio e de fim estão intercalada com outro
                    if (this.PossuiOutraIntercaladaPor(contexto, periodoReferenciaExtratoBancario.DiaReferencia, periodoReferenciaExtratoBancario.DataInicio, Convert.ToDateTime(periodoReferenciaExtratoBancario.DataFim)))
                    {
                        mensagens.Add("Já existe outro período de referência para o intervalo de datas informado.");
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

        public void Insere(Entidades.PeriodoReferenciaExtratoBancario periodoReferenciaExtratoBancario)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO PrestacaoContas.PERIODOREFERENCIAEXTRATOBANCARIO
                                                       (DIAREFERENCIA, 
		                                                 DATAINICIO, 
		                                                 DATAFIM, 
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO) 
                                            VALUES      (@DIAREFERENCIA, 
		                                                 @DATAINICIO, 
		                                                 @DATAFIM, 
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO)";

                contextQuery.Parameters.Add("@DIAREFERENCIA", SqlDbType.Int, periodoReferenciaExtratoBancario.DiaReferencia);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, periodoReferenciaExtratoBancario.DataInicio);

                if (periodoReferenciaExtratoBancario.DataFim == null || periodoReferenciaExtratoBancario.DataFim == DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, DBNull.Value);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, periodoReferenciaExtratoBancario.DataFim);
                }
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, periodoReferenciaExtratoBancario.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

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

        public void Atualiza(Entidades.PeriodoReferenciaExtratoBancario periodoReferenciaExtratoBancario)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE PrestacaoContas.PERIODOREFERENCIAEXTRATOBANCARIO
                                        SET    DIAREFERENCIA = @DIAREFERENCIA, 
		                                       DATAINICIO = @DATAINICIO, 
		                                       DATAFIM = @DATAFIM,
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  PERIODOREFERENCIAEXTRATOBANCARIOID = @PERIODOREFERENCIAEXTRATOBANCARIOID ";

                contextQuery.Parameters.Add("@DIAREFERENCIA", SqlDbType.Int, periodoReferenciaExtratoBancario.DiaReferencia);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, periodoReferenciaExtratoBancario.DataInicio);

                if (periodoReferenciaExtratoBancario.DataFim == null || periodoReferenciaExtratoBancario.DataFim == DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, DBNull.Value);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, periodoReferenciaExtratoBancario.DataFim);
                }
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, periodoReferenciaExtratoBancario.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@PERIODOREFERENCIAEXTRATOBANCARIOID", SqlDbType.Int, periodoReferenciaExtratoBancario.PeriodoReferenciaExtratoBancarioId);

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

        public ValidacaoDados ValidaRemocao(int periodoReferenciaExtratoBancarioId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ExtratoBancario rnExtratoBancario = new ExtratoBancario();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (periodoReferenciaExtratoBancarioId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se motivo ja foi utilizado
                    if (rnExtratoBancario.PossuiPeriodoReferenciaExtratoBancarioPor(contexto, periodoReferenciaExtratoBancarioId))
                    {
                        mensagens.Add("Este periodo não pode ser excluído, pois já foi utilizado em um extrato bancário.");
                    }

                    if (PossuiUnicoPeriodoReferenciaVigentePor(contexto))
                    {
                        mensagens.Add("Não é possível excluir este período de referência, pois é o único período de referência vigente..");
                    }
                }
                catch
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw;
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

        public void Remove(int periodoReferenciaExtratoBancarioId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE PrestacaoContas.PERIODOREFERENCIAEXTRATOBANCARIO
                            WHERE  PERIODOREFERENCIAEXTRATOBANCARIOID = @PERIODOREFERENCIAEXTRATOBANCARIOID  ";

                contextQuery.Parameters.Add("@PERIODOREFERENCIAEXTRATOBANCARIOID", SqlDbType.Int, periodoReferenciaExtratoBancarioId);

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

        public bool PossuiPeriodoReferenciaVigentePor(DataContext contexto, int periodoReferenciaExtratoBancarioId, int diaReferencia)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @"SELECT COUNT(*) 
                        FROM   PrestacaoContas.PERIODOREFERENCIAEXTRATOBANCARIO
                        WHERE  (DATAFIM IS NULL OR DATAFIM > GETDATE()  )
                               AND PERIODOREFERENCIAEXTRATOBANCARIOID <> @PERIODOREFERENCIAEXTRATOBANCARIOID 
                               AND DIAREFERENCIA = @DIAREFERENCIA";


            contextQuery.Parameters.Add("@PERIODOREFERENCIAEXTRATOBANCARIOID", SqlDbType.Int, periodoReferenciaExtratoBancarioId);
            contextQuery.Parameters.Add("@DIAREFERENCIA", SqlDbType.Int, diaReferencia);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiPeriodoReferenciaVigentePor(DataContext contexto, int periodoReferenciaExtratoBancarioId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @"SELECT COUNT(*) 
                        FROM   PrestacaoContas.PERIODOREFERENCIAEXTRATOBANCARIO
                        WHERE  (DATAFIM IS NULL OR DATAFIM > GETDATE()  )
                               AND PERIODOREFERENCIAEXTRATOBANCARIOID <> @PERIODOREFERENCIAEXTRATOBANCARIOID 
                               ";

            contextQuery.Parameters.Add("@PERIODOREFERENCIAEXTRATOBANCARIOID", SqlDbType.Int, periodoReferenciaExtratoBancarioId);


            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public int ObtemPeriodoReferenciaIdPor(DataContext contexto, int ano, int mes)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            DateTime primeiroDia = new DateTime(ano, mes, 1);

            try
            {
                contextQuery.Command = @" SELECT PERIODOREFERENCIAEXTRATOBANCARIOID
                                        FROM PRESTACAOCONTAS.PERIODOREFERENCIAEXTRATOBANCARIO
                                        WHERE (DATAFIM IS NULL AND DATAINICIO <= @DATA)
	                                        OR (@DATA BETWEEN DATAINICIO AND DATAFIM) ";

                contextQuery.Parameters.Add("@DATA", SqlDbType.DateTime, primeiroDia);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["PERIODOREFERENCIAEXTRATOBANCARIOID"]);
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

        public bool PossuiUnicoPeriodoReferenciaVigentePor(DataContext contexto)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @"SELECT COUNT(*) 
                        FROM   PrestacaoContas.PERIODOREFERENCIAEXTRATOBANCARIO
                        WHERE  (DATAFIM IS NULL OR DATAFIM > GETDATE() )
                               ";

            if (contexto.GetReturnValue<int>(contextQuery) == 1)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutraIntercaladaPor(DataContext ctx, int disReferencia, DateTime dataInicio, DateTime dataFim)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                            FROM   PrestacaoContas.PERIODOREFERENCIAEXTRATOBANCARIO  (NOLOCK)
                            WHERE                                                                        
                                    -- DIAREFERENCIA <> @DIAREFERENCIA AND
                                    @DATAINICIO <= CONVERT(DATE, DATAINICIO) 
                                    AND @DATAFIM >= CONVERT(DATE, CONVERT(DATETIME, ISNULL(DATAFIM, GETDATE()))) ";

            contextQuery.Parameters.Add("@DIAREFERENCIA", SqlDbType.Int, disReferencia);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, dataInicio.Date);
            contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, dataFim.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }


    }
}
