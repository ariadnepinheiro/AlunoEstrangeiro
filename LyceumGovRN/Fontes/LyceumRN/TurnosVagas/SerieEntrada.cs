using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using System.Data;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.TurnosVagas
{
    public class SerieEntrada : RNBase
    {
        public static DataTable ListaSerieEntrada()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT SERIEENTRADAID,
                                                 ENTRADA       ,
                                                 USUARIOID     ,
                                                 DATACADASTRO  ,
                                                 CURSOID       ,
                                                 SERIE          
                                            FROM TurnosVagas.SERIEENTRADA
                                        ORDER BY SERIEENTRADAID ";

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

        public static DataTable ListaSerieEntradaPorCurso(string CursoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT  
                                                 S.CURSO,
                                                 S.SERIE, 
                                                 Cast(ISNULL(S.SERIE, 0) AS VARCHAR) + 'ª Série' AS DESCRICAO,
                                                 IsNull(SE.ENTRADA, 0) AS ENTRADA
                                            FROM TurnosVagas.SERIEENTRADA SE 
                                           RIGHT JOIN LY_SERIE            S
                                              ON SE.CURSOID               = S.CURSO
                                             AND SE.SERIE                 = S.SERIE
                                           WHERE S.CURSO = @CURSO
                                           ORDER BY S.SERIE";

                contextQuery.Parameters.Add("@CURSO", CursoId.Trim());

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

        public static DataTable ListaSerieEntradaPorCursoSerie(string CursoId, int SerieId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT  
                                                 S.CURSO,
                                                 S.SERIE, 
                                                 Cast(ISNULL(S.SERIE, 0) AS VARCHAR) + 'ª Série' AS DESCRICAO,
                                                 IsNull(SE.ENTRADA, 0) AS ENTRADA
                                            FROM TurnosVagas.SERIEENTRADA SE 
                                           RIGHT JOIN LY_SERIE            S
                                              ON SE.CURSOID               = S.CURSO
                                             AND SE.SERIE                 = S.SERIE
                                           WHERE S.CURSO = @CURSO
                                             AND S.SERIE = @SERIE
                                           ORDER BY S.SERIE";

                contextQuery.Parameters.Add("@CURSO", CursoId.Trim());
                contextQuery.Parameters.Add("@SERIE", SerieId);

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

        public static DataTable ListaSerie()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT SERIE, CAST(SERIE AS VARCHAR) + 'ª Série' AS DESCRICAO
                                            FROM LY_SERIE 
                                           ORDER BY SERIE";

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

        public static DataTable ListaSeriePorCurso(string Curso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT SERIE, CAST(SERIE AS VARCHAR) + 'ª Série' AS DESCRICAO
                                            FROM LY_SERIE 
                                           WHERE CURSO = @CURSO  
                                           ORDER BY SERIE";

                contextQuery.Parameters.Add("@CURSO", Curso.Trim());

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

        public static Boolean VerificaSerieEntrada(string CursoId, int Serie)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT 1
                                            FROM TurnosVagas.SERIEENTRADA
                                           WHERE CURSOID = @CURSOID
                                             AND SERIE   = @SERIE ";

                contextQuery.Parameters.Add("@CURSOID", CursoId);
                contextQuery.Parameters.Add("@SERIE", Serie);

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

        public static Boolean VerificaExistenciaSerieEntrada(string CursoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT 1
                                            FROM TurnosVagas.SERIEENTRADA
                                           WHERE CURSOID = @CURSOID ";

                contextQuery.Parameters.Add("@CURSOID", CursoId);

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

        public void InsereSerieEntrada(Entidades.SerieEntrada SerieEntrada)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @"INSERT INTO TurnosVagas.SERIEENTRADA( 
                                        ENTRADA       ,
                                        USUARIOID     ,
                                        DATACADASTRO  ,
                                        CURSOID       ,
                                        SERIE          
                                    ) VALUES ( @ENTRADA       ,
                                               @USUARIOID     ,
                                               @DATACADASTRO  ,
                                               @CURSOID       ,
                                               @SERIE          
                                             )"
                };

                contextQuery.Parameters.Add("@ENTRADA", SerieEntrada.Entrada);
                contextQuery.Parameters.Add("@USUARIOID", SerieEntrada.UsuarioId.Trim());
                contextQuery.Parameters.Add("@DATACADASTRO", SerieEntrada.DataCadastro);
                contextQuery.Parameters.Add("@CURSOID", SerieEntrada.CursoId.Trim());
                contextQuery.Parameters.Add("@SERIE", SerieEntrada.Serie);

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

        public void RemoveSerieEntrada(string CursoId, int Serie)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" DELETE TurnosVagas.SERIEENTRADA
                                  WHERE CURSOID = @CURSOID
                                    AND SERIE   = @SERIE "
                };

                contextQuery.Parameters.Add("@CURSOID", CursoId);
                contextQuery.Parameters.Add("@SERIE", Serie);

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

