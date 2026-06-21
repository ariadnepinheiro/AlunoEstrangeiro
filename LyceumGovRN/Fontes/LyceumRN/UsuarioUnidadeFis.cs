using Seeduc.Infra.Data;
using System;
using System.Data;

namespace Techne.Lyceum.RN
{
    public class UsuarioUnidadeFis : RNBase
    {
        public static void InserirPrivUnidadeEns(string usuario, string unidadeFisica, DataContext context)
        {
            var contextQuery = new ContextQuery(@" INSERT INTO LY_USUARIO_UNIDADE_FIS(USUARIO,UNIDADE_FIS) VALUES(@USUARIO, @UNIDADE_FIS)");
            contextQuery.Parameters.Add("@USUARIO", usuario);
            contextQuery.Parameters.Add("@UNIDADE_FIS", unidadeFisica);

            context.ApplyModifications(contextQuery);
        }

        public static bool PossuiUmaUnidadeSo(string usuario)
        {
            return ExecutarFuncao("select COUNT(unidade_fis) from LY_USUARIO_UNIDADE_FIS where USUARIO = ?", usuario) == 1;
        }

        public void Insere(DataContext contexto, string unidadeFisica, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO DBO.LY_USUARIO_UNIDADE_FIS 
                                                        (USUARIO, 
                                                         UNIDADE_FIS) 
                                            VALUES      ( @USUARIO, 
                                                          @UNIDADE_FIS )  ";

            contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuario);
            contextQuery.Parameters.Add("@UNIDADE_FIS", TechneDbType.T_CODIGO, unidadeFisica);

            contexto.ApplyModifications(contextQuery);
        }

        public bool PossuiPermissaoPor(string usuario, string unidadeFisica)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                possui = this.PossuiPermissaoPor(ctx, usuario, unidadeFisica);

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

        public bool PossuiPermissaoPor(DataContext ctx, string usuario, string unidadeFisica)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            //Verifica se o uusario é privilegiado
            if (RN.Usuarios.UsuarioPrivilegiado(usuario))
            {
                //Usuario privilegiado possui permissão para todas as unidades
                possui = true;
            }
            else
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                        FROM    LY_USUARIO_UNIDADE_FIS
                        WHERE   USUARIO = @USUARIO
                                AND UNIDADE_FIS = @UNIDADE_FIS ";

                contextQuery.Parameters.Add("@USUARIO", usuario);
                contextQuery.Parameters.Add("@UNIDADE_FIS", unidadeFisica);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }
            }

            return possui;
        }


        public bool PossuiOutrosUsuarios(DataContext ctx, string unidadeFisica, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(1) 
                                        FROM   LY_USUARIO_UNIDADE_FIS 
                                        WHERE  UNIDADE_FIS = @UNIDADE_FIS 
                                               AND USUARIO <> @USUARIO  ";

                contextQuery.Parameters.Add("@UNIDADE_FIS", unidadeFisica);
                contextQuery.Parameters.Add("@USUARIO", usuario);

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
        }

        public void Remove(DataContext ctx, string unidadeFisica)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"DELETE FROM LY_USUARIO_UNIDADE_FIS 
                                        WHERE  UNIDADE_FIS = @UNIDADE_FIS  ";

                contextQuery.Parameters.Add("@UNIDADE_FIS", unidadeFisica);

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
