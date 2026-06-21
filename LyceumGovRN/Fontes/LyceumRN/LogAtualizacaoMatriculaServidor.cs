using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN
{
    public class LogAtualizacaoMatriculaServidor
    {
        public ICollection<Entidades.LogAtualizacaoMatriculaServidor> ObtemListaPor(int numFunc)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ICollection<Entidades.LogAtualizacaoMatriculaServidor> lista = new List<Entidades.LogAtualizacaoMatriculaServidor>();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  LOGATUALIZACAOMATRICULASERVIDORID ,
                                        PESSOA ,
                                        ORDEM ,
                                        MATRICULAANTERIOR ,
                                        MATRICULANOVA ,
                                        USUARIO ,
                                        DATACADASTRO,
                                        IDFUNCIONALANTERIOR, 
                                        IDFUNCIONALNOVO, 
                                        VINCULOANTERIOR, 
                                        VINCULONOVO
                                FROM    LOGATUALIZACAOMATRICULASERVIDOR (NOLOCK)
                                WHERE   PESSOA = @PESSOA ";

                contextQuery.Parameters.Add("@PESSOA", numFunc);

                lista = ctx.TryToBindEntities<Entidades.LogAtualizacaoMatriculaServidor>(contextQuery);

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

        public void Insere(DataContext ctx, Entidades.LogAtualizacaoMatriculaServidor logAtualizacaoMatriculaServidor)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT  INTO LOGATUALIZACAOMATRICULASERVIDOR
                            ( PESSOA ,
                              ORDEM ,
                              MATRICULAANTERIOR ,
                              MATRICULANOVA ,
                              USUARIO,
                              IDFUNCIONALANTERIOR, 
                              IDFUNCIONALNOVO, 
                              VINCULOANTERIOR, 
                              VINCULONOVO
                            )
                    VALUES  ( @PESSOA ,
                              @ORDEM ,
                              @MATRICULAANTERIOR ,
                              @MATRICULANOVA ,
                              @USUARIO,
                              @IDFUNCIONALANTERIOR, 
                              @IDFUNCIONALNOVO, 
                              @VINCULOANTERIOR, 
                              @VINCULONOVO
                            ) ";

                contextQuery.Parameters.Add("@PESSOA", logAtualizacaoMatriculaServidor.Pessoa);
                contextQuery.Parameters.Add("@ORDEM", logAtualizacaoMatriculaServidor.Ordem);
                contextQuery.Parameters.Add("@MATRICULAANTERIOR", logAtualizacaoMatriculaServidor.MatriculaAnterior);
                contextQuery.Parameters.Add("@MATRICULANOVA", logAtualizacaoMatriculaServidor.MatriculaNova);
                contextQuery.Parameters.Add("@IDFUNCIONALANTERIOR", logAtualizacaoMatriculaServidor.IdFuncionalAnterior == null || logAtualizacaoMatriculaServidor.IdFuncionalAnterior <= 0 ? (object)DBNull.Value : logAtualizacaoMatriculaServidor.IdFuncionalAnterior);
                contextQuery.Parameters.Add("@IDFUNCIONALNOVO", logAtualizacaoMatriculaServidor.IdFuncionalNovo == null || logAtualizacaoMatriculaServidor.IdFuncionalNovo <= 0 ? (object)DBNull.Value : logAtualizacaoMatriculaServidor.IdFuncionalNovo);
                contextQuery.Parameters.Add("@VINCULOANTERIOR", logAtualizacaoMatriculaServidor.VinculoAnterior == null || logAtualizacaoMatriculaServidor.VinculoAnterior <= 0 ? (object)DBNull.Value : logAtualizacaoMatriculaServidor.VinculoAnterior);
                contextQuery.Parameters.Add("@VINCULONOVO", logAtualizacaoMatriculaServidor.VinculoNovo == null || logAtualizacaoMatriculaServidor.VinculoNovo <= 0 ? (object)DBNull.Value : logAtualizacaoMatriculaServidor.VinculoNovo);
                contextQuery.Parameters.Add("@USUARIO", logAtualizacaoMatriculaServidor.UsuarioId);

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