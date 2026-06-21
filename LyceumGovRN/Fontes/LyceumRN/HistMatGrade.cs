using Seeduc.Infra.Data;
using System;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.RN
{
    public class HistMatGrade
    {
        public static void Inserir(Entidades.LyHistMatGrade histMatGrade)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" INSERT  INTO dbo.LY_HIST_MATGRADE
                                        (  ALUNO ,
                                          GRADE_ID ,
                                          DT_ULTALT ,
                                          NUM_CHAMADA ,
                                          SIT_MATGRADE
                                        )
                                VALUES  (
                                            @ALUNO ,
                                            @GRADE_ID ,
                                            @DT_ULTALT ,
                                            @NUM_CHAMADA ,
                                            @SIT_MATGRADE
                                        ) "
                    };

                    contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO ,histMatGrade.Aluno);
                    contextQuery.Parameters.Add("@GRADE_ID", TechneDbType.T_NUMERO_GRANDE,histMatGrade.Grade_Id);
                    contextQuery.Parameters.Add("@DT_ULTALT", histMatGrade.DtUltalt);
                    contextQuery.Parameters.Add("@NUM_CHAMADA",TechneDbType.T_NUMERO_GRANDE ,histMatGrade.Num_chamada);
                    contextQuery.Parameters.Add("@SIT_MATGRADE", TechneDbType.T_SIT_MATGRADE, histMatGrade.Sit_Matgrade);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception ex)
                {
                    ctx.Abandon();
                    var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                       Environment.NewLine, Convert.ToString(ex.Message));
                    throw new Exception(mensagem);
                }
            }
        }

        public void Insere(DataContext ctx, LyHistMatGrade histMatGrade)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" INSERT  INTO dbo.LY_HIST_MATGRADE
                                        (  ALUNO ,
                                          GRADE_ID ,
                                          DT_ULTALT ,
                                          NUM_CHAMADA ,
                                          SIT_MATGRADE
                                        )
                                VALUES  (
                                            @ALUNO ,
                                            @GRADE_ID ,
                                            @DT_ULTALT ,
                                            @NUM_CHAMADA ,
                                            @SIT_MATGRADE
                                        ) ";

                contextQuery.Parameters.Add("@ALUNO", histMatGrade.Aluno);
                contextQuery.Parameters.Add("@GRADE_ID", histMatGrade.Grade_Id);
                contextQuery.Parameters.Add("@DT_ULTALT", histMatGrade.DtUltalt);
                contextQuery.Parameters.Add("@NUM_CHAMADA", histMatGrade.Num_chamada);
                contextQuery.Parameters.Add("@SIT_MATGRADE", histMatGrade.Sit_Matgrade);

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

        public static void Alterar(Entidades.LyHistMatGrade histMatGrade)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" UPDATE  DBO.LY_HIST_MATGRADE
                                    SET    NUM_CHAMADA=@NUM_CHAMADA,
                                            SIT_MATGRADE=@SIT_MATGRADE
    
                                    WHERE   ALUNO = @ALUNO
                                            AND GRADE_ID= @GRADE_ID
                                            AND DT_ULTALT= @DT_ULTALT"
                    };

                    contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, histMatGrade.Aluno);
                    contextQuery.Parameters.Add("@GRADE_ID", TechneDbType.T_NUMERO_GRANDE, histMatGrade.Grade_Id);
                    contextQuery.Parameters.Add("@DT_ULTALT", histMatGrade.DtUltalt);
                    contextQuery.Parameters.Add("@NUM_CHAMADA", TechneDbType.T_NUMERO_GRANDE, histMatGrade.Num_chamada);
                    contextQuery.Parameters.Add("@SIT_MATGRADE", TechneDbType.T_SIT_MATGRADE, histMatGrade.Sit_Matgrade);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception ex)
                {
                    ctx.Abandon();
                    var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                       Environment.NewLine, Convert.ToString(ex.Message));
                    throw new Exception(mensagem);
                }
            }
        }

        public void Altera(DataContext ctx, LyHistMatGrade histMatGrade)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" UPDATE  DBO.LY_HIST_MATGRADE
                                    SET    NUM_CHAMADA=@NUM_CHAMADA,
                                            SIT_MATGRADE=@SIT_MATGRADE
    
                                    WHERE   ALUNO = @ALUNO
                                            AND GRADE_ID= @GRADE_ID
                                            AND DT_ULTALT= @DT_ULTALT ";

                contextQuery.Parameters.Add("@ALUNO", histMatGrade.Aluno);
                contextQuery.Parameters.Add("@GRADE_ID", histMatGrade.Grade_Id);
                contextQuery.Parameters.Add("@DT_ULTALT", histMatGrade.DtUltalt);
                contextQuery.Parameters.Add("@NUM_CHAMADA", histMatGrade.Num_chamada);
                contextQuery.Parameters.Add("@SIT_MATGRADE", histMatGrade.Sit_Matgrade);

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

        public static void Remover(Entidades.LyHistMatGrade histMatGrade)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"DELETE  dbo.LY_HIST_MATGRADE
                                    WHERE   ALUNO = @ALUNO
                                        AND GRADE_ID=@GRADE_ID
                                        AND DT_ULTALT=@DT_ULTALT"
                    };

                    contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, histMatGrade.Aluno);
                    contextQuery.Parameters.Add("@GRADE_ID", TechneDbType.T_NUMERO_GRANDE, histMatGrade.Grade_Id);
                    contextQuery.Parameters.Add("@DT_ULTALT", TechneDbType.T_DATA, histMatGrade.DtUltalt);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception ex)
                {
                    ctx.Abandon();
                    var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                       Environment.NewLine, Convert.ToString(ex.Message));
                    throw new Exception(mensagem);
                }
            }
        }

        public bool PossuiCadastradoPor(string aluno, decimal gradeId, DateTime dtUltalt)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT  COUNT(*)
                            FROM    LY_HIST_MATGRADE
                            WHERE   ALUNO = @ALUNO
                                    AND GRADE_ID = @GRADE_ID
                                    AND DT_ULTALT = @DT_ULTALT "
                };

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@GRADE_ID", gradeId);
                contextQuery.Parameters.Add("@DT_ULTALT", dtUltalt);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
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
