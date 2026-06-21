using System;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN
{
    public class HistFaculdade
    {
        public void Insere(DataContext ctx, LyHistFaculdade histFaculdade)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                LyHistFaculdade historicoFaculdade = new LyHistFaculdade();

                //Verifica se já existe uma linha de historico para aquele aluno / censo
                historicoFaculdade = this.Carrega(histFaculdade.Aluno, Convert.ToDecimal(histFaculdade.Ordem));

                //Caso não exista insere
                if (string.IsNullOrEmpty(historicoFaculdade.Aluno))
                {
                    contextQuery.Command = @" INSERT  INTO DBO.LY_HIST_FACULDADE
                                            ( ALUNO, ORDEM, OUTRA_FACULDADE )
                                    VALUES  ( @ALUNO, @ORDEM, @OUTRA_FACULDADE ) ";

                    contextQuery.Parameters.Add("@ALUNO", histFaculdade.Aluno);
                    contextQuery.Parameters.Add("@ORDEM", histFaculdade.Ordem);
                    contextQuery.Parameters.Add("@OUTRA_FACULDADE", histFaculdade.OutraFaculdade);

                    ctx.ApplyModifications(contextQuery);
                }
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

        public int ObtemMaiorOrdemPor(string aluno)
        {
            int ordem = 0;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = (@" SELECT  ISNULL(MAX(ORDEM), 0) AS ORDEM
                            FROM    LY_HIST_FACULDADE
                            WHERE   ALUNO = @ALUNO ")
                };

                contextQuery.Parameters.Add("@ALUNO", aluno);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    ordem = Convert.ToInt32(reader["ORDEM"]);
                }

                return ordem;
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }

        public LyHistFaculdade Carrega(string aluno, decimal ordem)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            LyHistFaculdade histFaculdade = new LyHistFaculdade();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT DISTINCT
                                    HF.ALUNO ,
                                    ISNULL(HF.ORDEM, 0) AS ORDEM ,
                                    HF.OUTRA_FACULDADE
                            FROM    LY_HIST_FACULDADE HF
                                    INNER JOIN LY_ALUNO A ON HF.ALUNO = A.ALUNO
                            WHERE   A.ALUNO = @ALUNO
                                    AND HF.ORDEM = @ORDEM "
                };

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ORDEM", ordem);

                histFaculdade = ctx.TryToBindEntity<LyHistFaculdade>(contextQuery);

                return histFaculdade;
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
