using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Data;
using Techne.Lyceum.CR;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN
{
    public class GrupoHabilitacao : RNBase
    {
        public static QueryTable ObterGruposDeHabilitacaoDocente(string num_func)
        {
            if (!num_func.IsNullOrEmptyOrWhiteSpace())
            {
                return Consultar(@"SELECT ghdoc.num_func,
                                       ghdoc.agrupamento,
                                       ghdoc.provisorio,
                                       ghdoc.dt_limite,
                                       ghdoc.agrupamento_ingresso,
                                       ghdoc.stamp_atualizacao,
                                       ghdoc.campo_01,
                                       ghdoc.campo_02,
                                       gh.descricao,
                                       gh.disp_glp_dol,
                                       gh.stamp_atualizacao,
                                       gh.tipo,
	                                   ghdoc.datacadastro,
	                                   ghdoc.documentacao
                                FROM   ly_grupo_habilitacao_doc ghdoc (nolock)
                                       INNER JOIN ly_grupo_habilitacao gh
                                               ON ghdoc.agrupamento = gh.agrupamento
                                WHERE  ghdoc.num_func = ?
                                       AND ghdoc.provisorio = 'N'
                                ORDER  BY gh.descricao ", num_func);
            }
            else
                return null;
        }

        public static QueryTable ObterGruposDeHabilitacaoDocenteProvisorios(string num_func)
        {

            if (!num_func.IsNullOrEmptyOrWhiteSpace())
            {
                return Consultar(@"SELECT ghdoc.num_func,ghdoc.agrupamento,ghdoc.provisorio,ghdoc.dt_limite,ghdoc.agrupamento_ingresso,ghdoc.stamp_atualizacao,ghdoc.campo_01,ghdoc.campo_02,
	                gh.DESCRICAO, gh.DISP_GLP_DOL, gh.STAMP_ATUALIZACAO, gh.TIPO	
                    FROM Ly_grupo_habilitacao_doc ghdoc (NOLOCK) INNER JOIN
                    Ly_grupo_habilitacao gh ON ghdoc.agrupamento = gh.agrupamento
                    WHERE ghdoc.num_func = ? AND ghdoc.provisorio = 'S' ORDER BY dt_limite desc", num_func);
            }
            else
                return null;
        }

        public static bool PossuiGrupoCapacitacaoEdEspecial(string pessoa)
        {
            string sql = @"select 1 from LY_GRUPO_HABILITACAO_DOC h INNER JOIN dbo.LY_DOCENTE d ON h.NUM_FUNC=d.NUM_FUNC where AGRUPAMENTO='Q033' AND PESSOA=?";
            int qtd = ExecutarFuncao(sql, pessoa);
            if (qtd == 1)
            {
                return true;
            }
            return false;
        }

        public DataTable ListaGrupoHabilitacaoPor(string disciplina)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable grupos = null;

            try
            {
                contextQuery.Command = @" SELECT  gh.agrupamento ,
                                    gh.descricao
                            FROM    LY_GRUPO_HABILITACAO GH
                                    JOIN LY_GRUPO_HABILITACAO_DISC GD ON GH.AGRUPAMENTO = GD.AGRUPAMENTO
                            WHERE   GD.DISCIPLINA = @DISCIPLINA ";

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);

                grupos = ctx.GetDataTable(contextQuery);
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

            return grupos;
        }

        public bool ExisteGrupoHabilitacaoPor(string disciplina)
        {	
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                            FROM    LY_GRUPO_HABILITACAO_DISC
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

        #region Contrato Temporário
        public static QueryTable ObterDisciplinaIngressoPor(string concurso, string candidato)
        {
            string sql = @"SELECT GH.AGRUPAMENTO, GH.DESCRICAO 
                           FROM LY_GRUPO_HABILITACAO GH
                               INNER JOIN LY_CANDIDATO_DOCENTE CD ON 
                                   GH.AGRUPAMENTO = CD.AGRUPAMENTO_INGRESSO
                           WHERE 
                               CD.CONCURSO = ? 
                               AND CD.CANDIDATO = ?";

            return Consultar(sql, concurso, candidato);
        }
       
        #endregion

        public DataTable ListaGrupoHabilitacaoAtivo()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable grupos = null;

            try
            {
                contextQuery.Command = @" SELECT  GH.AGRUPAMENTO ,
                                                    GH.DESCRICAO
                                            FROM    LY_GRUPO_HABILITACAO GH                                  
                                            WHERE   GH.DISP_GLP_DOL = 'S'
	                                                AND ATIVO = 'S'
                                          order by DESCRICAO ";                

                grupos = ctx.GetDataTable(contextQuery);
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

            return grupos;
        }

        public void RemoveGrupoDisciplina(string disciplina, string agrupamento)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                this.RemoveGrupoDisciplina(ctx, disciplina, agrupamento);
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

        private void RemoveGrupoDisciplina(DataContext ctx, string disciplina, string agrupamento)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE  LY_GRUPO_HABILITACAO_DISC
                        WHERE   DISCIPLINA = @DISCIPLINA 
                                AND AGRUPAMENTO = @AGRUPAMENTO ";

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@AGRUPAMENTO", agrupamento);

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

        public DataTable ListaGrupoHabilitacaoDocAtivo()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable grupos = null;

            try
            {
                contextQuery.Command = @" SELECT  GH.AGRUPAMENTO ,
                                                    GH.DESCRICAO
                                            FROM    LY_GRUPO_HABILITACAO GH                                  
                                            WHERE   ATIVO = 'S'
                                          order by DESCRICAO ";

                grupos = ctx.GetDataTable(contextQuery);
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

            return grupos;
        }
    }
}
