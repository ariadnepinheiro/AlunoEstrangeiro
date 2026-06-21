namespace Techne.Lyceum.RN
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using DTOs;
    using Entidades;
    using Seeduc.Infra.Data;
    using Util;
    using System.Text;
    using System.Data.SqlClient;

    public class CompetenciaHabilidadeDocente
    {
        public void GeraLog(DataContext contexto, string turma, string disciplina, DateTime dataFrequencia, List<int> itensExcecaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            StringBuilder sql = new StringBuilder();

            sql.Append(@" INSERT INTO dbo.TCE_LOG_COMPETENCIA_HABILIDADE_DOCENTE
                                               (ID_COMPETENCIA_HABILIDADE_ITEM
                                               ,DISCIPLINA
                                               ,TURMA
                                               ,ANO
                                               ,PERIODO
                                               ,SUBPERIODO
                                               ,MATRICULA
                                               ,DT_CADASTRO
                                               ,DT_LOG
                                               ,DATAFREQUENCIA
                                               ,NUM_FUNC
                                               ,USUARIOID)
                                    select ID_COMPETENCIA_HABILIDADE_ITEM
                                               ,DISCIPLINA
                                               ,TURMA
                                               ,ANO
                                               ,PERIODO
                                               ,SUBPERIODO
                                               ,MATRICULA
                                               ,DT_CADASTRO
                                               ,GETDATE()
                                               ,DATAFREQUENCIA
                                               ,NUM_FUNC
                                               ,USUARIOID
                                    from [dbo].[TCE_COMPETENCIA_HABILIDADE_DOCENTE]
                                    where TURMA = @TURMA
	                                    and DISCIPLINA = @DISCIPLINA
	                                    and DATAFREQUENCIA = @DATAFREQUENCIA ");

            if (itensExcecaoId.Count > 0)
            {
                string ids = string.Empty;

                foreach (int id in itensExcecaoId)
                {
                    if (!ids.IsNullOrEmptyOrWhiteSpace())
                    {
                        ids += ", ";
                    }

                    ids += id.ToString();
                }

                sql.Append(string.Format(@" and ID_COMPETENCIA_HABILIDADE_ITEM not in ({0}) ", ids));
            }

            contextQuery.Command = sql.ToString();

            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
            contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, disciplina);
            contextQuery.Parameters.Add("@DATAFREQUENCIA", SqlDbType.DateTime, dataFrequencia.Date);

            contexto.ApplyModifications(contextQuery);
        }

        public void GeraLog(DataContext contexto, string censo, DateTime dataFrequencia)
        {
            ContextQuery contextQuery = new ContextQuery();
            StringBuilder sql = new StringBuilder();

            sql.Append(@" INSERT INTO dbo.TCE_LOG_COMPETENCIA_HABILIDADE_DOCENTE
                                               (ID_COMPETENCIA_HABILIDADE_ITEM
                                               ,DISCIPLINA
                                               ,TURMA
                                               ,ANO
                                               ,PERIODO
                                               ,SUBPERIODO
                                               ,MATRICULA
                                               ,DT_CADASTRO
                                               ,DT_LOG
                                               ,DATAFREQUENCIA
                                               ,NUM_FUNC
                                               ,USUARIOID)
                                    SELECT DISTINCT CHD.ID_COMPETENCIA_HABILIDADE_ITEM
                                               ,CHD.DISCIPLINA
                                               ,CHD.TURMA
                                               ,CHD.ANO
                                               ,CHD.PERIODO
                                               ,CHD.SUBPERIODO
                                               ,CHD.MATRICULA
                                               ,CHD.DT_CADASTRO
                                               ,GETDATE()
                                               ,CHD.DATAFREQUENCIA
                                               ,CHD.NUM_FUNC
                                               ,CHD.USUARIOID
                                    FROM [DBO].[TCE_COMPETENCIA_HABILIDADE_DOCENTE] CHD
										INNER JOIN LY_TURMA T ON CHD.TURMA = T.TURMA AND CHD.ANO = T.ANO AND CHD.PERIODO = T.SEMESTRE AND CHD.DISCIPLINA = T.DISCIPLINA
									WHERE T.FACULDADE = @CENSO
	                                    AND DATAFREQUENCIA = @DATAFREQUENCIA");
           
            contextQuery.Command = sql.ToString();

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@DATAFREQUENCIA", SqlDbType.DateTime, dataFrequencia.Date);

            contexto.ApplyModifications(contextQuery);
        }

        public void Remove(DataContext contexto, string turma, string disciplina, DateTime dataFrequencia, List<int> itensExcecaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            StringBuilder sql = new StringBuilder();

            sql.Append(@" DELETE [dbo].[TCE_COMPETENCIA_HABILIDADE_DOCENTE]
                                    WHERE TURMA = @TURMA
	                                    AND DISCIPLINA = @DISCIPLINA
	                                    AND DATAFREQUENCIA = @DATAFREQUENCIA 
                                        ");

            if (itensExcecaoId.Count > 0)
            {
                string ids = string.Empty;

                foreach (int id in itensExcecaoId)
                {
                    if (!ids.IsNullOrEmptyOrWhiteSpace())
                    {
                        ids += ", ";
                    }

                    ids += id.ToString();
                }

                sql.Append(string.Format(@" AND ID_COMPETENCIA_HABILIDADE_ITEM NOT IN ({0}) ", ids));
            }

            contextQuery.Command = sql.ToString();

            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
            contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, disciplina);
            contextQuery.Parameters.Add("@DATAFREQUENCIA", SqlDbType.DateTime, dataFrequencia.Date);

            contexto.ApplyModifications(contextQuery);
        }

        public void Remove(DataContext contexto, string censo, DateTime dataFrequencia)
        {
            ContextQuery contextQuery = new ContextQuery();
            StringBuilder sql = new StringBuilder();

            sql.Append(@" DELETE CHD
									FROM [DBO].[TCE_COMPETENCIA_HABILIDADE_DOCENTE] CHD
										INNER JOIN LY_TURMA T ON CHD.TURMA = T.TURMA AND CHD.ANO = T.ANO AND CHD.PERIODO = T.SEMESTRE AND CHD.DISCIPLINA = T.DISCIPLINA
									WHERE T.FACULDADE = @CENSO
	                                    AND DATAFREQUENCIA = @DATAFREQUENCIA 
                                        ");

            contextQuery.Command = sql.ToString();

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@DATAFREQUENCIA", SqlDbType.DateTime, dataFrequencia.Date);

            contexto.ApplyModifications(contextQuery);
        }

        public List<int> ObtemItensPor(string turma, string disciplina, DateTime dataFrequencia)
        {
            List<int> lista = new List<int>();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DISTINCT I.ID_COMPETENCIA_HABILIDADE_ITEM
                                        from TCE_COMPETENCIA_HABILIDADE_DOCENTE D
											INNER JOIN TCE_COMPETENCIA_HABILIDADE_ITEM I ON D.ID_COMPETENCIA_HABILIDADE_ITEM = I.ID_COMPETENCIA_HABILIDADE_ITEM
                                        where TURMA = @TURMA
	                                        and DISCIPLINA = @DISCIPLINA
	                                        and DATAFREQUENCIA = @DATAFREQUENCIA  ";

                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
                contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, disciplina);
                contextQuery.Parameters.Add("@DATAFREQUENCIA", SqlDbType.DateTime, dataFrequencia.Date);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    lista.Add(Convert.ToInt32(reader["ID_COMPETENCIA_HABILIDADE_ITEM"]));
                }

                return lista;
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }

        public bool ExistePor(DataContext contexto, int competenciaHabilidadeItemId, string turma, string disciplina, DateTime dataFrequencia)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;


            contextQuery.Command = @" SELECT COUNT(1) 
                                        from TCE_COMPETENCIA_HABILIDADE_DOCENTE
                                        where ID_COMPETENCIA_HABILIDADE_ITEM = @ID_COMPETENCIA_HABILIDADE_ITEM
	                                        and TURMA = @TURMA
	                                        and DISCIPLINA = @DISCIPLINA
	                                        and DATAFREQUENCIA = @DATAFREQUENCIA ";

            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
            contextQuery.Parameters.Add("@ID_COMPETENCIA_HABILIDADE_ITEM", SqlDbType.Int, competenciaHabilidadeItemId);
            contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, disciplina);
            contextQuery.Parameters.Add("@DATAFREQUENCIA", SqlDbType.DateTime, dataFrequencia.Date);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public void Insere(DataContext contexto, TceCompetenciaHabilidadeDocente competenciaHabilidadeDocente)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO dbo.TCE_COMPETENCIA_HABILIDADE_DOCENTE
                                           (ID_COMPETENCIA_HABILIDADE_ITEM
                                           ,DISCIPLINA
                                           ,TURMA
                                           ,ANO
                                           ,PERIODO
                                           ,SUBPERIODO
                                           ,MATRICULA
                                           ,DT_CADASTRO
                                           ,DATAFREQUENCIA
                                           ,NUM_FUNC
                                           ,USUARIOID)
                                     VALUES
                                           (@ID_COMPETENCIA_HABILIDADE_ITEM, 
                                           @DISCIPLINA, 
                                           @TURMA, 
                                           @ANO,
                                           @PERIODO, 
                                           @SUBPERIODO, 
                                           @MATRICULA, 
                                           @DT_CADASTRO, 
                                           @DATAFREQUENCIA, 
                                           @NUM_FUNC, 
                                           @USUARIOID) ";

            contextQuery.Parameters.Add("@ID_COMPETENCIA_HABILIDADE_ITEM", SqlDbType.Int, competenciaHabilidadeDocente.IdCompetenciaHabilidadeItem);
            contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, competenciaHabilidadeDocente.Disciplina);
            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, competenciaHabilidadeDocente.Turma);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, competenciaHabilidadeDocente.Ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, competenciaHabilidadeDocente.Periodo);
            contextQuery.Parameters.Add("@SUBPERIODO", SqlDbType.Int, competenciaHabilidadeDocente.Subperiodo);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, competenciaHabilidadeDocente.UsuarioId);
            contextQuery.Parameters.Add("@DT_CADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAFREQUENCIA", SqlDbType.DateTime, Convert.ToDateTime(competenciaHabilidadeDocente.DataFrequencia).Date);
            contextQuery.Parameters.Add("@NUM_FUNC", SqlDbType.Int, DBNull.Value);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, competenciaHabilidadeDocente.UsuarioId);

            contexto.ApplyModifications(contextQuery);
        }
    }
}