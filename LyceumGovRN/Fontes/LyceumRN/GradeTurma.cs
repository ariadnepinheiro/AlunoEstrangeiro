using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN
{
    public class GradeTurma
    {
        public bool ExisteGradeTurmaPor(string disciplina)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                        FROM    dbo.LY_GRADE_TURMA
                        WHERE   DISCIPLINA = @DISCIPLINA ";

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    existe = true;
                }

                return existe;
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

        public string ObterGradeId(DataContext ctx, decimal ano, decimal periodo, string turma)
        {
            return ctx.GetReturnValue<string>(
                new ContextQuery(
                    @"SELECT DISTINCT GT.GRADE_ID
                        FROM   LY_TURMA T (NOLOCK) 
                               INNER JOIN LY_GRADE_TURMA GT (NOLOCK) 
                                       ON GT.DISCIPLINA = T.DISCIPLINA 
                                          AND GT.TURMA = T.TURMA 
                                          AND GT.ANO = T.ANO 
                                          AND GT.SEMESTRE = T.SEMESTRE 
                        WHERE  T.ANO = @ANO
                               AND T.SEMESTRE = @PERIODO 
                               AND T.TURMA = @TURMA ",
                    new ContextQueryParameter("@ANO", ano),
                    new ContextQueryParameter("@PERIODO", periodo),
                    new ContextQueryParameter("@TURMA", turma)));
        }

        public List<string> ListaDisciplinaMatriculaRegular(decimal ano, decimal periodo, string turma, string gradeId)
        {
            List<string> lista = new List<string>();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            System.Data.SqlClient.SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DISTINCT 
                                                D.DISCIPLINA  AS DISCIPLINA 
              
                                        FROM   LY_GRADE_TURMA GT 
                                               JOIN LY_DISCIPLINA D 
                                                 ON GT.DISCIPLINA = D.DISCIPLINA 
                                        WHERE  GT.TURMA = @TURMA 
                                               AND GT.ANO = @ANO 
                                               AND GT.SEMESTRE = @SEMESTRE 
                                               AND GT.GRADE_ID = @GRADE_ID  ";

                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);
                contextQuery.Parameters.Add("@GRADE_ID", gradeId);


                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    lista.Add(Convert.ToString(reader["DISCIPLINA"]));
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

        public string ObterGradeId(decimal ano, decimal periodo, string turma)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            
            return ObterGradeId(ctx, ano, periodo, turma);
            
        }
    }
}
