using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN
{
    public class AulaDocenteTipo
    {
        public bool PossuiGLP(DataContext ctx, decimal nunFunc)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT  COUNT(*)
                                FROM    LY_AULA_DOCENTE_TIPO
                                WHERE   NUM_FUNC = @NUM_FUNC
                                        AND DATA_FIM > GETDATE()
                                        AND TIPO_AULA = 'GLP' ";

            contextQuery.Parameters.Add("@NUM_FUNC", nunFunc);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiGLP(decimal numFunc)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                possui = this.PossuiGLP(ctx, numFunc);
                return possui;
            }

            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public void DesalocaAulasTipo(DataContext ctx, decimal numFunc, DateTime dataFimAula)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE LY_AULA_DOCENTE_TIPO 
                                        SET     STAMP_ATUALIZACAO = @STAMP_ATUALIZACAO,
                                                DATA_FIM = case when @DATAFIMAULA > DATA_INICIO then  @DATAFIMAULA else DATA_INICIO end, 
                                                TIPO_AULA = 'NGLP' 
                                        WHERE  DATA_FIM >= CONVERT(DATE, @DATAFIMAULA) 
                                               AND DATA_FIM <> DATA_INICIO
                                               AND NUM_FUNC = @NUM_FUNC  ";

                contextQuery.Parameters.Add("@NUM_FUNC", numFunc);
                contextQuery.Parameters.Add("@DATAFIMAULA", dataFimAula.Date);
                contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", DateTime.Now);

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
        }

        public void DesalocaAulasTipo(DataContext ctx, decimal numFunc, string turno, string faculdade, int diaSemana, int aula, string disciplina, string turma, decimal ano, decimal semestre, DateTime dataInicioAula, DateTime dataFimAula)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE LY_AULA_DOCENTE_TIPO 
                                            SET STAMP_ATUALIZACAO = @STAMP_ATUALIZACAO,
                                                DATA_FIM = case when @DATAFIMAULA > DATA_INICIO then  @DATAFIMAULA else DATA_INICIO end, 
                                                TIPO_AULA = 'NGLP' 
                                        WHERE  DATA_FIM >= CONVERT(DATE, @DATAFIMAULA) 
                                               AND DATA_FIM <> DATA_INICIO
                                               AND NUM_FUNC = @NUM_FUNC
											   AND TURNO = @TURNO
											   AND FACULDADE = @FACULDADE
                                               AND AULA = @AULA
											   AND DIA_SEMANA = @DIA_SEMANA
											   AND DISCIPLINA = @DISCIPLINA
											   AND TURMA = @TURMA
											   AND ANO = @ANO
											   AND SEMESTRE = @SEMESTRE
											   AND DATA_INICIO = @DATA_INICIO  ";

                contextQuery.Parameters.Add("@NUM_FUNC", numFunc);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@FACULDADE", faculdade);
                contextQuery.Parameters.Add("@DIA_SEMANA", diaSemana);
                contextQuery.Parameters.Add("@AULA", aula);
                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@DATA_INICIO", dataInicioAula);
                contextQuery.Parameters.Add("@DATAFIMAULA", dataFimAula.Date);
                contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", DateTime.Now);

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
        }

        public int ObtemQuantidadeGlpsAtivasPor(decimal pessoa, decimal ano)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*) as TOTAL
                                FROM    LY_AULA_DOCENTE_TIPO AD (NOLOCK)
										INNER JOIN LY_DOCENTE D (NOLOCK) ON AD.NUM_FUNC = D.NUM_FUNC
                                WHERE   D.PESSOA = @PESSOA
                                        AND AD.DATA_INICIO <= GETDATE() 
										AND AD.DATA_FIM >= GETDATE()
										AND AD.ANO = @ANO
                                        AND TIPO_AULA = 'GLP' ";

                contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_CODIGO, pessoa);
                contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["TOTAL"]);
                }

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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }

        public int ObtemQuantidadeGlpsPor(decimal numFunc, DateTime dataFimAula)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*) as TOTAL
                                FROM    LY_AULA_DOCENTE_TIPO AD (NOLOCK)									
                               WHERE  DATA_FIM >= CONVERT(DATE, @DATAFIMAULA) 
                                               AND DATA_FIM <> DATA_INICIO
                                               AND NUM_FUNC = @NUM_FUNC 
                                        AND TIPO_AULA = 'GLP' ";

                contextQuery.Parameters.Add("@NUM_FUNC", numFunc);
                contextQuery.Parameters.Add("@DATAFIMAULA", dataFimAula);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["TOTAL"]);
                }

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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }


        public bool PossuiGLPAtiva(decimal numFunc)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                possui = this.PossuiGLPAtiva(ctx, numFunc);
                return possui;
            }

            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        } 
        
        public bool PossuiGLPAtiva(DataContext ctx, decimal nunFunc)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @"SELECT COUNT(DD.AULA)TOL_AULA_NORMAL
                                    FROM   LY_AULA_DOCENTE DD (NOLOCK)                                    
                                    WHERE  NUM_FUNC=@NUM_FUNC
			                            AND ANO= YEAR(GETDATE())
                                        AND DATA_FIM <> DATA_INICIO
			                            AND EXISTS (SELECT 1
                                                   FROM   LY_TURMA TT (NOLOCK)
                                                   WHERE  TT.ANO = DD.ANO
                                                          AND TT.SEMESTRE = DD.SEMESTRE
                                                          AND TT.DISCIPLINA = DD.DISCIPLINA
                                                          AND TT.TURMA = DD.TURMA
                                                          AND TT.DT_FIM = DD.DATA_FIM
                                                          AND DD.DATA_FIM>=CONVERT(DATE,CURRENT_TIMESTAMP)
                                                          )
                                           AND  EXISTS (   SELECT 1
                                                           FROM   LY_AULA_DOCENTE_TIPO T1 (NOLOCK)
                                                           WHERE  T1.NUM_FUNC=DD.NUM_FUNC
                                                                  AND T1.TURNO=DD.TURNO
                                                                  AND T1.FACULDADE = DD.FACULDADE
                                                                  AND T1.DIA_SEMANA=DD.DIA_SEMANA
                                                                  AND T1.AULA=DD.AULA
                                                                  AND T1.DISCIPLINA = DD.DISCIPLINA
                                                                  AND T1.TURMA=DD.TURMA
									                              AND T1.ANO = DD.ANO
                                                                  AND T1.SEMESTRE = DD.SEMESTRE
                                                                  AND T1.TIPO_AULA = 'GLP'
                                                                  AND T1.DATA_INICIO = DD.DATA_INICIO
                                                                  ) ";

            contextQuery.Parameters.Add("@NUM_FUNC", nunFunc);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

    }
}
