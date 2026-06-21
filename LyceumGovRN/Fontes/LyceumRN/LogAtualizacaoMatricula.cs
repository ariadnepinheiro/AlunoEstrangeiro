using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN
{
    public class LogAtualizacaoMatricula
    {
        public ICollection<Entidades.LogAtualizacaoMatricula> ObtemListaPor(int numFunc)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ICollection<Entidades.LogAtualizacaoMatricula> lista = new List<Entidades.LogAtualizacaoMatricula>();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  LOGATUALIZACAOMATRICULAID ,
                                        DOCENTEID ,
                                        MATRICULAANTERIOR ,
                                        MATRICULANOVA ,
                                        USUARIO ,
                                        DATACADASTRO,
                                        IDFUNCIONALANTERIOR, 
                                        IDFUNCIONALNOVO, 
                                        VINCULOANTERIOR, 
                                        VINCULONOVO
                                FROM    LOGATUALIZACAOMATRICULA (NOLOCK)
                                WHERE   DOCENTEID = @DOCENTEID ";

                contextQuery.Parameters.Add("@DOCENTEID", numFunc);

                lista = ctx.TryToBindEntities<Entidades.LogAtualizacaoMatricula>(contextQuery);

                return lista;
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

        public void Insere(DataContext ctx, Entidades.LogAtualizacaoMatricula logAtualizacaoMatricula)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT  INTO LOGATUALIZACAOMATRICULA
                            ( DOCENTEID ,
                              MATRICULAANTERIOR ,
                              MATRICULANOVA ,
                              USUARIO,
                              IDFUNCIONALANTERIOR, 
                              IDFUNCIONALNOVO, 
                              VINCULOANTERIOR, 
                              VINCULONOVO
                            )
                    VALUES  ( @DOCENTEID ,
                              @MATRICULAANTERIOR ,
                              @MATRICULANOVA ,
                              @USUARIO,
                              @IDFUNCIONALANTERIOR, 
                              @IDFUNCIONALNOVO, 
                              @VINCULOANTERIOR, 
                              @VINCULONOVO
                            ) ";

                contextQuery.Parameters.Add("@DOCENTEID", logAtualizacaoMatricula.DocenteId);
                contextQuery.Parameters.Add("@MATRICULAANTERIOR", logAtualizacaoMatricula.MatriculaAnterior);
                contextQuery.Parameters.Add("@MATRICULANOVA", logAtualizacaoMatricula.MatriculaNova);
                contextQuery.Parameters.Add("@IDFUNCIONALANTERIOR", logAtualizacaoMatricula.IdFuncionalAnterior == null || logAtualizacaoMatricula.IdFuncionalAnterior <= 0 ? (object)DBNull.Value : logAtualizacaoMatricula.IdFuncionalAnterior);
                contextQuery.Parameters.Add("@IDFUNCIONALNOVO", logAtualizacaoMatricula.IdFuncionalNovo == null || logAtualizacaoMatricula.IdFuncionalNovo <= 0 ? (object)DBNull.Value : logAtualizacaoMatricula.IdFuncionalNovo);
                contextQuery.Parameters.Add("@VINCULOANTERIOR", logAtualizacaoMatricula.VinculoAnterior == null || logAtualizacaoMatricula.VinculoAnterior <= 0 ? (object)DBNull.Value : logAtualizacaoMatricula.VinculoAnterior);
                contextQuery.Parameters.Add("@VINCULONOVO", logAtualizacaoMatricula.VinculoNovo == null || logAtualizacaoMatricula.VinculoNovo <= 0 ? (object)DBNull.Value : logAtualizacaoMatricula.VinculoNovo);
                contextQuery.Parameters.Add("@USUARIO", logAtualizacaoMatricula.UsuarioId);

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
    }
}