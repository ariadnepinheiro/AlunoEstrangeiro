using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;


namespace Techne.Lyceum.RN.RecursosHumanos
{
    public class DocenteFuncaoGlpTurma
    {
        public DataTable ListaPor(int idDocenteFuncaoGlp)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DT.DOCENTEFUNCAOGLP_TURMAID, 
                                           DT.ID_DOCENTE_FUNCAO_GLP, 
                                           DT.TURMA, 
                                           DT.DISCIPLINA, 
                                           D.NOME AS NOMEDISCIPLINA, 
                                           DT.NUMFUNCCARENCIA, 
                                           DT.CARGAHORARIA, 
                                           DT.ANO, 
                                           DT.PERIODO 
                                    FROM   RECURSOSHUMANOS.DOCENTEFUNCAOGLP_TURMA DT                                          
                                           INNER JOIN LY_DISCIPLINA D (nolock) 
                                                   ON DT.DISCIPLINA = D.DISCIPLINA 
                                    WHERE  ID_DOCENTE_FUNCAO_GLP = @ID_DOCENTE_FUNCAO_GLP  ";

                contextQuery.Parameters.Add("@ID_DOCENTE_FUNCAO_GLP", SqlDbType.Int, idDocenteFuncaoGlp);

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

        public void Insere(DataContext contexto, RecursosHumanos.Entidades.DocenteFuncaoGlpTurma docenteFuncaoGlpTurma)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO RECURSOSHUMANOS.DOCENTEFUNCAOGLP_TURMA 
                                            (ID_DOCENTE_FUNCAO_GLP, 
                                             TURMA, 
                                             DISCIPLINA, 
                                             ANO, 
                                             PERIODO, 
                                             NUMFUNCCARENCIA, 
                                             CARGAHORARIA) 
                                VALUES      (@ID_DOCENTE_FUNCAO_GLP, 
                                             @TURMA, 
                                             @DISCIPLINA, 
                                             @ANO, 
                                             @PERIODO, 
                                             @NUMFUNCCARENCIA, 
                                             @CARGAHORARIA)  ";

            contextQuery.Parameters.Add("@ID_DOCENTE_FUNCAO_GLP", SqlDbType.Decimal, docenteFuncaoGlpTurma.IdDocenteFuncaoGlp);
            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, docenteFuncaoGlpTurma.Turma);
            contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, docenteFuncaoGlpTurma.Disciplina);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Decimal, docenteFuncaoGlpTurma.Ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Decimal, docenteFuncaoGlpTurma.Periodo);
            contextQuery.Parameters.Add("@NUMFUNCCARENCIA", SqlDbType.Decimal, docenteFuncaoGlpTurma.NumFuncCarencia);
            contextQuery.Parameters.Add("@CARGAHORARIA", SqlDbType.Int, docenteFuncaoGlpTurma.CargaHoraria);

            contexto.ApplyModifications(contextQuery);
        }

        public void RemovePorIdDocenteFuncaoGlp(DataContext contexto, int idDocenteFuncaoGlp)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE RECURSOSHUMANOS.DOCENTEFUNCAOGLP_TURMA
                                            WHERE ID_DOCENTE_FUNCAO_GLP = @ID_DOCENTE_FUNCAO_GLP ";

            contextQuery.Parameters.Add("@ID_DOCENTE_FUNCAO_GLP", SqlDbType.Decimal, idDocenteFuncaoGlp);

            contexto.ApplyModifications(contextQuery);
        }


        public bool ExistePor(string turma, decimal ano, decimal periodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command =
                @" 
                    SELECT   count(0) 
                    FROM   [LYCEUM].[RecursosHumanos].[DOCENTEFUNCAOGLP_TURMA] GT
					INNER JOIN LY_DOCENTE_FUNCAO_GLP DF ON DF.ID_DOCENTE_FUNCAO_GLP = GT.ID_DOCENTE_FUNCAO_GLP
                    WHERE  GT.TURMA = @TURMA AND GT.ANO = @ANO AND GT.PERIODO = @PERIODO AND STATUS='Aceita'
                ";

                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                

                return ctx.GetReturnValue<int>(contextQuery) > 0;
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
