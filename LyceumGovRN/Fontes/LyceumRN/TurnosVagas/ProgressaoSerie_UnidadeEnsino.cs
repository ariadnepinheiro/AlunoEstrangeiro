using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using System.Data;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.TurnosVagas
{
    public class ProgressaoSerie_UnidadeEnsino : RNBase
    {
        public static DataTable ListaProgressaoSerie_UnidadeEnsino()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT PROGRESSAOSERIE_UNIDADEENSINO_ID,
                                                 SERIE                           ,
                                                 PREFERENCIAL                    ,
                                                 USUARIOID                       ,
                                                 DATACADASTRO                    ,
                                                 CURSOID                         ,
                                                 PROXIMOCURSOID                  ,
                                                 PROXIMASERIE                    ,
                                                 UNIDADEENSINOID                 
                                            FROM TurnosVagas.PROGRESSAOSERIE_UNIDADEENSINO 
                                           ORDER BY PROGRESSAOSERIE_UNIDADEENSINO_ID";

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return dt;
        }

        public static DataTable ListaProgressaoSerie_UnidadeEnsinoPorCurso(string UnidadeEnsinoId, string CursoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT P.PROGRESSAOSERIE_UNIDADEENSINO_ID                      ,
                                                 C.CURSO                              AS COD_CURSO       , 
                                                 C.NOME                               AS NOME_CURSO      , 
                                                 T.TIPO                               AS COD_NIVEL       , 
                                                 T.DESCRICAO                          AS NOME_NIVEL      ,
                                                 P.SERIE                              AS COD_SERIE       , 
                                                 CAST(P.SERIE AS VARCHAR) + 'ª Série' AS SERIE           , 
                                                 C2.CURSO                             AS PROX_COD_CURSO  , 
                                                 C2.NOME                              AS PROX_NOME_CURSO , 
                                                 T2.TIPO                              AS PROX_COD_NIVEL  , 
                                                 T2.DESCRICAO                         AS PROX_NOME_NIVEL , 
                                                 CAST(P.PROXIMASERIE AS VARCHAR) 
                                                    + 'ª Série'                       AS PROX_SERIE      , 
                                                 PREFERENCIAL                                            , 
                                                 P.DATACADASTRO                                            ,
                                                 P.USUARIOID                                               ,
                                                 P.UNIDADEENSINOID
                                            FROM TurnosVagas.PROGRESSAOSERIE_UNIDADEENSINO P
                                           INNER JOIN LY_CURSO                             C 
                                              ON P.CURSOID                                 = C.CURSO
                                           INNER JOIN LY_TIPO_CURSO                        T 
                                              ON C.TIPO                                    = T.TIPO
                                           INNER JOIN LY_CURSO                            C2 
                                              ON P.PROXIMOCURSOID                          = C2.CURSO
                                           INNER JOIN LY_TIPO_CURSO                       T2 
                                              ON C2.TIPO                                   = T2.TIPO
                                           WHERE P.UNIDADEENSINOID = @UNIDADEENSINOID
                                             AND C.CURSO           = @CURSO
                                           ORDER BY P.UNIDADEENSINOID, 
                                                    C.CURSO, 
                                                    P.SERIE";

                contextQuery.Parameters.Add("@UNIDADEENSINOID", UnidadeEnsinoId.ToString());
                contextQuery.Parameters.Add("@CURSO", CursoId.ToString());

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return dt;
        }

        public Boolean VerificaProgressaoSerie_UnidadeEnsino(Entidades.ProgressaoSerie_UnidadeEnsino ProgressaoSerie_UnidadeEnsino)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT 1
                                            FROM TurnosVagas.PROGRESSAOSERIE_UNIDADEENSINO
                                           WHERE SERIE           = @SERIE         
	                                         AND CURSOID         = @CURSOID       
	                                         AND PROXIMOCURSOID  = @PROXIMOCURSOID
	                                         AND PROXIMASERIE    = @PROXIMASERIE
                                             AND UNIDADEENSINOID = @UNIDADEENSINOID";

                contextQuery.Parameters.Add("@SERIE", ProgressaoSerie_UnidadeEnsino.Serie);
                contextQuery.Parameters.Add("@CURSOID", ProgressaoSerie_UnidadeEnsino.CursoId.Trim());
                contextQuery.Parameters.Add("@PROXIMOCURSOID", ProgressaoSerie_UnidadeEnsino.ProximoCursoId.Trim());
                contextQuery.Parameters.Add("@PROXIMASERIE", ProgressaoSerie_UnidadeEnsino.ProximaSerie);
                contextQuery.Parameters.Add("@UNIDADEENSINOID", ProgressaoSerie_UnidadeEnsino.UnidadeEnsinoId.Trim());

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            if (Convert.ToInt16(dt.Rows.Count.ToString()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean VerificaProgressaoSerie_UnidadeEnsinoPreferencial(Entidades.ProgressaoSerie_UnidadeEnsino ProgressaoSerie_UnidadeEnsino)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT 1
                                            FROM TurnosVagas.PROGRESSAOSERIE_UNIDADEENSINO
                                           WHERE SERIE           = @SERIE         
	                                         AND CURSOID         = @CURSOID       
                                             AND UNIDADEENSINOID = @UNIDADEENSINOID
                                             AND PREFERENCIAL    = 1";

                contextQuery.Parameters.Add("@SERIE", ProgressaoSerie_UnidadeEnsino.Serie);
                contextQuery.Parameters.Add("@CURSOID", ProgressaoSerie_UnidadeEnsino.CursoId.Trim());
                contextQuery.Parameters.Add("@UNIDADEENSINOID", ProgressaoSerie_UnidadeEnsino.UnidadeEnsinoId.Trim());

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            if (Convert.ToInt16(dt.Rows.Count.ToString()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean ExisteOutraProgressaoSerie_UnidadeEnsino(Entidades.ProgressaoSerie_UnidadeEnsino ProgressaoSerie_UnidadeEnsino)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT 1
                                            FROM TurnosVagas.PROGRESSAOSERIE_UNIDADEENSINO
                                           WHERE PROGRESSAOSERIE_UNIDADEENSINO_ID <> @PROGRESSAOSERIE_UNIDADEENSINO_ID
                                             AND SERIE           = @SERIE          
                                             AND CURSOID         = @CURSOID        
                                             AND UNIDADEENSINOID = @UNIDADEENSINOID";

                contextQuery.Parameters.Add("@PROGRESSAOSERIE_UNIDADEENSINO_ID", ProgressaoSerie_UnidadeEnsino.ProgressaoSerie_UnidadeEnsino_Id);
                contextQuery.Parameters.Add("@SERIE", ProgressaoSerie_UnidadeEnsino.Serie);
                contextQuery.Parameters.Add("@CURSOID", ProgressaoSerie_UnidadeEnsino.CursoId);
                contextQuery.Parameters.Add("@UNIDADEENSINOID", ProgressaoSerie_UnidadeEnsino.UnidadeEnsinoId);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            if (Convert.ToInt16(dt.Rows.Count.ToString()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void InsereProgressaoSerie_UnidadeEnsino(Entidades.ProgressaoSerie_UnidadeEnsino ProgressaoSerie_UnidadeEnsino)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @"INSERT INTO TurnosVagas.PROGRESSAOSERIE_UNIDADEENSINO(
                                        SERIE          ,
                                        PREFERENCIAL   ,
                                        USUARIOID      ,
                                        DATACADASTRO   ,
                                        CURSOID        ,
                                        PROXIMOCURSOID ,
                                        PROXIMASERIE   ,
                                        UNIDADEENSINOID 
                                   ) VALUES ( @SERIE          ,
			                                  @PREFERENCIAL   ,
			                                  @USUARIOID      ,
			                                  @DATACADASTRO   ,
			                                  @CURSOID        ,
			                                  @PROXIMOCURSOID ,
			                                  @PROXIMASERIE   ,
                                              @UNIDADEENSINOID   
			                                )"
                };

                contextQuery.Parameters.Add("@SERIE", ProgressaoSerie_UnidadeEnsino.Serie);
                contextQuery.Parameters.Add("@PREFERENCIAL", ProgressaoSerie_UnidadeEnsino.Preferencial);
                contextQuery.Parameters.Add("@USUARIOID", ProgressaoSerie_UnidadeEnsino.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", ProgressaoSerie_UnidadeEnsino.DataCadastro);
                contextQuery.Parameters.Add("@CURSOID", ProgressaoSerie_UnidadeEnsino.CursoId);
                contextQuery.Parameters.Add("@PROXIMOCURSOID", ProgressaoSerie_UnidadeEnsino.ProximoCursoId);
                contextQuery.Parameters.Add("@PROXIMASERIE", ProgressaoSerie_UnidadeEnsino.ProximaSerie);
                contextQuery.Parameters.Add("@UNIDADEENSINOID", ProgressaoSerie_UnidadeEnsino.UnidadeEnsinoId.Trim());

                ctx.ApplyModifications(contextQuery);
            }

            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format(@"Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0} Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public void AlteraProgressaoSerie_UnidadeEnsinoPreferencial(Entidades.ProgressaoSerie_UnidadeEnsino ProgressaoSerie_UnidadeEnsino)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @"UPDATE TurnosVagas.PROGRESSAOSERIE_UNIDADEENSINO
                                   SET PREFERENCIAL = 0
                                 WHERE UNIDADEENSINOID = @UNIDADEENSINOID
                                   AND CURSOID         = @CURSOID
                                   AND SERIE           = @SERIE
                                   AND PREFERENCIAL    = 1"
                };

                contextQuery.Parameters.Add("@SERIE", ProgressaoSerie_UnidadeEnsino.Serie);
                contextQuery.Parameters.Add("@CURSOID", ProgressaoSerie_UnidadeEnsino.CursoId);
                contextQuery.Parameters.Add("@UNIDADEENSINOID", ProgressaoSerie_UnidadeEnsino.UnidadeEnsinoId.Trim());

                ctx.ApplyModifications(contextQuery);
            }

            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format(@"Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0} Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public void AlteraProgressaoSerie_UnidadeEnsinoPreferencial(int ProgressaoSerie_UnidadeEnsino_Id)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @"UPDATE TurnosVagas.PROGRESSAOSERIE_UNIDADEENSINO
                                   SET PREFERENCIAL = 1
                                 WHERE PROGRESSAOSERIE_UNIDADEENSINO_ID = @PROGRESSAOSERIE_UNIDADEENSINO_ID"
                };

                contextQuery.Parameters.Add("@PROGRESSAOSERIE_UNIDADEENSINO_ID", ProgressaoSerie_UnidadeEnsino_Id);

                ctx.ApplyModifications(contextQuery);
            }

            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format(@"Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0} Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public void RemoveProgressaoSerie_UnidadeEnsino(int ProgressaoSerie_UnidadeEnsino_Id)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" DELETE TurnosVagas.PROGRESSAOSERIE_UNIDADEENSINO
                                  WHERE PROGRESSAOSERIE_UNIDADEENSINO_ID = @PROGRESSAOSERIE_UNIDADEENSINO_ID "
                };

                contextQuery.Parameters.Add("@PROGRESSAOSERIE_UNIDADEENSINO_ID", ProgressaoSerie_UnidadeEnsino_Id);

                ctx.ApplyModifications(contextQuery);
            }

            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format(@"Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0} Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

    }
}

