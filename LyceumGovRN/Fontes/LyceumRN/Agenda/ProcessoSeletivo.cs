using System;
using System.Data;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Techne.Lyceum.RN.Agenda
{
    public class ProcessoSeletivo
    {
        #region PROPRIEDADES

        protected int timeOutConexaoBancoDeDados
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["TimeOutConexaoBancoDeDados"]);
            }
        }

        #endregion

        public DataTable ConsultaPorAgendaId(int agendaId)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.Command = @"SELECT P.[NUMEROEDITAL],
                                                P.[DATANASCIMENTOINICIAL],
                                                P.[DATANASCIMENTOFINAL],
                                                P.[NOMEARQUIVOEDITAL]
                                           FROM [LYCEUM].[Agenda].[PROCESSOSELETIVO] P
                                          INNER JOIN [LYCEUM].[Agenda].[AGENDA] A
                                             ON P.[AGENDAID] = A.[AGENDAID]
                                          WHERE A.[AGENDAID] = @AGENDAID";

                contextQuery.Parameters.Add("@AGENDAID", agendaId);

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }

            return dt;
        }

        public static DataTable ListaEdital()
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable edital = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.Command = @" SELECT P.EDITAL, P.AGENDAID, P.PROCESSOSELETIVOID  
                                            FROM AGENDA.PROCESSOSELETIVO P
                                           INNER JOIN AGENDA.AGENDA A
                                              ON A.AGENDAID = P.AGENDAID
                                           INNER JOIN AGENDA.EVENTO E
                                              ON A.AGENDAID = E.AGENDAID
                                           INNER JOIN AGENDA.TIPOEVENTO T
                                              ON E.TIPOEVENTOID = T.TIPOEVENTOID AND T.TIPOEVENTOID = 4
                                           WHERE CAST(GETDATE() AS DATE) BETWEEN CAST(E.DATAINICIO AS DATE) 
                                             AND CAST(E.DATAFIM AS DATE) ";

                edital = contexto.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }

            return edital;
        }

        public DataTable ListaEditalPorAgenda(int agendaId)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable edital = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.Command = @" SELECT P.EDITAL  
                                            FROM AGENDA.PROCESSOSELETIVO P
                                           WHERE P.AGENDAID = @AGENDAID ";

                contextQuery.Parameters.Add("@AGENDAID ", agendaId);

                edital = contexto.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }

            return edital;
        }

        public static DataTable ProcessoSeletivoAtivo()
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable ProcessoSeletivoAtivo = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.Command = @"SELECT * 
	                                       FROM LYCEUM.AGENDA.PROCESSOSELETIVO PS
	                                      INNER JOIN LYCEUM.AGENDA.AGENDA A
		                                     ON PS.AGENDAID = A.AGENDAID
	                                      INNER JOIN LYCEUM.AGENDA.EVENTO E
		                                     ON A.AGENDAID = E.AGENDAID
	                                      WHERE CAST(GETDATE() AS DATE) BETWEEN CAST(E.DATAINICIO AS DATE) 
                                            AND CAST(E.DATAFIM AS DATE)";

                ProcessoSeletivoAtivo = contexto.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }

            return ProcessoSeletivoAtivo;
        }

        public static bool VerificaDentroIntervaloDataNascimentoProcessoSeletivo(DateTime DataNascimento, Int32 AgendaID)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            bool dentroIntervalo = false;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.Command = @"SELECT 1
                                           FROM [LYCEUM].[Agenda].[PROCESSOSELETIVO] PS
                                          WHERE PS.AGENDAID = @AGENDAID
                                            AND CAST(@DATANASCIMENTO AS DATE) BETWEEN 
                                                    CAST(PS.[DATANASCIMENTOINICIAL] AS DATE) 
                                                    AND CAST(PS.[DATANASCIMENTOFINAL] AS DATE) ";

                contextQuery.Parameters.Add("@DATANASCIMENTO ", DataNascimento);
                contextQuery.Parameters.Add("@AGENDAID ", AgendaID);

                object retorno = contexto.GetReturnValue(contextQuery);

                if (retorno != null)
                    dentroIntervalo = Convert.ToInt32(retorno).Equals(1);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }

            return dentroIntervalo;
        }

        /// <summary>
        /// Gera a pre-matrícula do candidato de acordo com a agenda
        /// </summary>
        /// <param name="agendaId"></param>
        /// <returns></returns>
        public DataTable GeraAlunoPreMatricula(int agendaId, string usuarioId, ref string mensagemErroProcedure)
        {
            DataTable dt = null;

            SqlConnection conexao = null;
            SqlCommand comando = new SqlCommand();
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            DataSet ds = new DataSet();

            try
            {
                conexao = new SqlConnection(RNBase.StringConn);
                conexao.Open();

                comando.CommandType = CommandType.StoredProcedure;
                comando.CommandText = "PROCESSOSELETIVOALUNO.PR_GERA_PRE_MATRICULA_CANDIDATO";
                comando.Parameters.Add(new SqlParameter("@AGENDAID", agendaId));
                comando.Parameters.Add(new SqlParameter("@USUARIOID", usuarioId));
                comando.CommandTimeout = timeOutConexaoBancoDeDados;

                comando.Connection = conexao;

                dataAdapter.SelectCommand = comando;
                dataAdapter.Fill(ds);

                dt = ds.Tables[ds.Tables.Count - 1];
            }
            catch (Exception exception)
            {
                mensagemErroProcedure = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                      "\\n", Convert.ToString(exception.Message));
            }
            finally
            {
                if (conexao.State != ConnectionState.Closed)
                {
                    conexao.Close();
                    conexao.Dispose();
                }
            }

            return dt;
        }

        public DataTable RetornaNumeroChamada(int agendaId)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.Command = @"SELECT MAX(NUMEROCHAMADA) AS NUMEROCHAMADA
                                           FROM [ProcessoSeletivoAluno].[PROCESSOSELETIVO_HISTORICOIMPORTACAO] PH
                                          INNER JOIN [Agenda].[PROCESSOSELETIVO] PS
                                             ON PH.PROCESSOSELETIVOID = PS.PROCESSOSELETIVOID
                                          WHERE PS.AGENDAID = @IDAGENDA";

                contextQuery.Parameters.Add("@IDAGENDA", agendaId);

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }

            return dt;
        }


        #region EXPORTAÇÃO DADOS - INSCRITOS NO PROCESSO SELETIVO

        /// <summary>
        /// Exporta os candidatos isncritos de acordo com a agenda
        /// </summary>
        /// <param name="agendaId"></param>
        /// <returns></returns>
        public DataTable ExportaDadosInscritos(int agendaId)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"PROCESSOSELETIVOALUNO.PR_CONSULTA_CANDIDATOS_INSCRITOS";
                contextQuery.Parameters.Add("@IDAGENDA", agendaId);

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }

            return dt;
        }

        /// <summary>
        /// Exporta os recursos de provas aplicadas aos candidatos inscritos de acordo com a agenda
        /// </summary>
        /// <param name="agendaId"></param>
        /// <returns></returns>
        public DataTable ExportaRecursosAplicacaoProvaInscritos(int agendaId)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"PROCESSOSELETIVOALUNO.PR_CONSULTA_RECURSO_APLICACAO_PROVA_CANDIDATOS_INSCRITOS";
                contextQuery.Parameters.Add("@IDAGENDA", agendaId);

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }

            return dt;
        }

        /// <summary>
        /// Exporta os dados de Unidade de Ensino / Curso / Turno dos candidatos inscritos de acordo com a agenda
        /// </summary>
        /// <param name="agendaId"></param>
        /// <returns></returns>
        public DataTable ExportaUnidadeEnsinoCursoTurnoInscritos(int agendaId)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"PROCESSOSELETIVOALUNO.PR_CONSULTA_UNIDADE_ENSINO_CURSO_TURNO_CANDIDATOS_INSCRITOS";
                contextQuery.Parameters.Add("@IDAGENDA", agendaId);

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }

            return dt;
        }

        #endregion

        #region GERAÇÃO PRÉ-MATRÍCULA CANDIDATOS CLASSIFICADOS

        /// <summary>
        /// Gera a pre-matrícula do candidato de acordo com a agenda
        /// </summary>
        /// <param name="agendaId"></param>
        /// <returns></returns>
        public DataTable GeraPreMatriculaCandidatos(int agendaId)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"PROCESSOSELETIVOALUNO.PR_GERA_PRE_MATRICULA_CANDIDATO";
                contextQuery.Parameters.Add("@IDAGENDA", agendaId);

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }

            return dt;
        }

        #endregion

    }
}
