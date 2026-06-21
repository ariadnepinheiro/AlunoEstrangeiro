using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN
{
    public class Prova : RNBase
    {
        public DataTable ListaBimestresComProvaPor(int ano)
        {	
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable bimestres = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                        SUBPERIODO,
										SEMESTRE
                                FROM    LY_PROVA
                                WHERE   ANO = @ANO ";

                contextQuery.Parameters.Add("@ANO", ano);

                bimestres = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
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
                if (ctx != null)
                    ctx.Dispose();
            }


            return bimestres;
        }

        public void SpGeraProvas(DataContext ctx, int ano, int semetre, int bimestre, string disciplina)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {             
                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"SP_GERA_PROVAS";
                contextQuery.Parameters.Add("@P_ANO", ano);
                contextQuery.Parameters.Add("@P_SEMESTRE", semetre);
                contextQuery.Parameters.Add("@P_BIMESTRE", bimestre);
                contextQuery.Parameters.Add("@P_DISCIPLINA", disciplina);

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

        public void RemoveProvaPor(DataContext ctx, string disciplina)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE  FROM LY_PROVA
                        WHERE   DISCIPLINA = @DISCIPLINA ";

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);

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

        public bool ExisteProvaPor(string disciplina)
        {	
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                        FROM    dbo.LY_PROVA
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

        public string ChecaNotaMaximaProvaDestino(DataContext ctx, string turmaAtual, decimal ano, decimal semestre, string turmaNova)
        {
            ContextQuery contextQuery = new ContextQuery();
            string aviso = string.Empty;
            DataTable provas = null;

            //Busca nota maxima das disciplinas na turma atual
            provas = ObtemPor(ctx, turmaAtual, ano, semestre);

            foreach (DataRow dr in provas.Rows)
            {
                contextQuery = new ContextQuery();
                //Para cada disciplina verifica nota maxima diferente na turma destino
                contextQuery.Command = @" SELECT COUNT(*) AS QUANTIDADE 
                                    FROM   LY_PROVA PV (NOLOCK) 
                                    WHERE  PV.TURMA = @TURMA 
                                           AND PV.ANO = @ANO 
                                           AND PV.SEMESTRE = @SEMESTRE 
                                           AND PV.DISCIPLINA = @DISCIPLINA 
                                           AND PV.PROVA = @PROVA 
                                           AND PV.NOTA_MAX <> @NOTA_MAX  ";

                contextQuery.Parameters.Add("@TURMA", turmaNova);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@DISCIPLINA", Convert.ToString(dr["disciplina"]));
                contextQuery.Parameters.Add("@PROVA", Convert.ToString(dr["prova"]));
                contextQuery.Parameters.Add("@NOTA_MAX", Convert.ToString(dr["nota_max"]));

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    aviso += "<br />A nota máxima da prova " + Convert.ToString(dr["nome"]) +
                        " é diferente entre as turmas " + turmaAtual + " e " + turmaNova +
                        ", para a disciplina " + Convert.ToString(dr["disciplina"]) + ".<br />";
                }
            }

            return aviso;
        }

        public DataTable ObtemPor(DataContext ctx, string turma, decimal ano, decimal semestre)
        {
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            contextQuery.Command = @" SELECT PV.DISCIPLINA, 
                               PV.PROVA, 
                               PV.NOTA_MAX, 
                               PV.NOME 
                        FROM   LY_PROVA PV (NOLOCK)
                        WHERE  PV.TURMA = @TURMA 
                               AND PV.ANO = @ANO 
                               AND PV.SEMESTRE = @SEMESTRE ";

            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", semestre);

            dt = ctx.GetDataTable(contextQuery);

            return dt;
        }
    }
}
