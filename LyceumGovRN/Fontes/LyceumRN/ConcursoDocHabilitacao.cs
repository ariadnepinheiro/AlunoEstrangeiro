using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN
{
    public class ConcursoDocHabilitacao
    {
        public void Insere(RN.Entidades.LyConcursoDocHabilitacao dadosConcurso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"INSERT INTO LY_CONCURSO_DOC_HABILITACAO 
                                                                       (CONCURSO
                                                                       ,REGIONALID
                                                                       ,AGRUPAMENTO
                                                                       ,MUNICIPIO_PROC)
                                                                 VALUES
                                                                       (@CONCURSO,
                                                                        @REGIONALID,  
                                                                        @AGRUPAMENTO, 
                                                                        @MUNICIPIO_PROC) ";

                contextQuery.Parameters.Add("@CONCURSO", dadosConcurso.Concurso);
                contextQuery.Parameters.Add("@REGIONALID", dadosConcurso.RegionalId);
                contextQuery.Parameters.Add("@AGRUPAMENTO", dadosConcurso.Agrupamento);
                contextQuery.Parameters.Add("@MUNICIPIO_PROC", dadosConcurso.Municipio_proc);

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

        public int Remove(RN.Entidades.LyConcursoDocHabilitacao dadosConcurso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            int ret = int.MinValue;

            try
            {
                contextQuery.Command = @"DELETE FROM LY_CONCURSO_DOC_HABILITACAO
                                                                     WHERE CONCURSO = @CONCURSO 
                                                                     AND   MUNICIPIO_PROC = @MUNICIPIO_PROC  
                                                                     AND	AGRUPAMENTO = @AGRUPAMENTO 
                                                                     AND	REGIONALID = @REGIONALID";

                contextQuery.Parameters.Add("@CONCURSO", dadosConcurso.Concurso);
                contextQuery.Parameters.Add("@REGIONALID", dadosConcurso.RegionalId);
                contextQuery.Parameters.Add("@AGRUPAMENTO", dadosConcurso.Agrupamento);
                contextQuery.Parameters.Add("@MUNICIPIO_PROC", dadosConcurso.Municipio_proc);

                ret = ctx.ApplyModifications(contextQuery);
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

            return ret;
        }

        public DataTable ValidaHabilitacoesExistentes(RN.Entidades.LyConcursoDocHabilitacao dadosConcurso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"SELECT CONCURSO
                                              ,REGIONALID
                                              ,AGRUPAMENTO
                                              ,MUNICIPIO_PROC
                                          FROM LYCEUM.dbo.LY_CONCURSO_DOC_HABILITACAO
                                          WHERE CONCURSO=@CONCURSO and		
                                                REGIONALID=@REGIONALID and
                                                AGRUPAMENTO=@AGRUPAMENTO and	
                                                MUNICIPIO_PROC=@MUNICIPIO_PROC ";

                contextQuery.Parameters.Add("@CONCURSO", dadosConcurso.Concurso);
                contextQuery.Parameters.Add("@REGIONALID", dadosConcurso.RegionalId);
                contextQuery.Parameters.Add("@AGRUPAMENTO", dadosConcurso.Agrupamento);
                contextQuery.Parameters.Add("@MUNICIPIO_PROC", dadosConcurso.Municipio_proc);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
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
                ctx.Dispose();
            }

            return dt;
        }

        public DataTable ConsultaHabilitacoesConcurso(string concurso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT ISNULL(N.DESCRICAO,RE.REGIONAL) AS NUCLEO
                                                    ,ISNULL(N.NUCLEO,RE.ID_REGIONAL) AS NUCLEO_ID
                                                    ,M.NOME AS MUNICIPIO
                                                    ,M.CODIGO AS MUNICIPIO_ID
                                                    ,GR.DESCRICAO AS DISCIPLINA
                                                    ,GR.AGRUPAMENTO AS DISCIPLINA_ID
                                                     FROM LY_CONCURSO_DOC_HABILITACAO HAB
                                                     LEFT join LY_NUCLEO N ON N.NUCLEO = HAB.NUCLEO
                                                     LEFT JOIN TCE_REGIONAL RE ON RE.ID_REGIONAL = HAB.REGIONALID
                                                     INNER JOIN ly_grupo_habilitacao gr on gr.agrupamento = HAB.AGRUPAMENTO
                                                     inner join MUNICIPIO M ON M.CODIGO = HAB.MUNICIPIO_PROC
                                                     WHERE CONCURSO = @CONCURSO
                                                     ORDER BY NUCLEO, MUNICIPIO";

                contextQuery.Parameters.Add("@CONCURSO", concurso);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
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
                ctx.Dispose();
            }

            return dt;
        }
    }
}
