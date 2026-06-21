using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using System.Data;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN
{
    public class EstudoAdicional : RNBase
    {
        public DataTable ListaEstudoAdicional()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable estudosAdicionais = null;

            try
            {
                contextQuery.Command = @" SELECT  ESTUDOADICIONALID ,
                                                NOME
                                        FROM    ESTUDOADICIONAL
                                        ORDER BY NOME ";

                estudosAdicionais = ctx.GetDataTable(contextQuery);
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

            return estudosAdicionais;
        }

        public ValidacaoDados Valida(Entidades.EstudoAdicional estudoAdicional)
        {
            List<string> mensagens = new List<string>();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            object obj = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (string.IsNullOrEmpty(estudoAdicional.Nome) || estudoAdicional.Nome.Trim().Length > 50)
            {
                mensagens.Add("O campo NOME é obrigatório, com no máximo 50 caracteres.");
            }

            if (mensagens.Count == 0)
            {
                //Verifica se o Nome já existe.
                contextQuery.Command = @"SELECT TOP 1
                                                1
                                        FROM    ESTUDOADICIONAL
                                        WHERE   NOME = @NOME
                                                AND ESTUDOADICIONALID <> @ESTUDOADICIONALID ";
                contextQuery.Parameters.Add("@NOME", estudoAdicional.Nome.Trim());
                contextQuery.Parameters.Add("@ESTUDOADICIONALID", estudoAdicional.EstudoAdicionalId);

                obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    mensagens.Add("Já existe um estudo adicional cadastrado com este nome.");
                }

                if (estudoAdicional.EstudoAdicionalId > 0)
                {
                    contextQuery = new ContextQuery();
                    obj = null;                   

                    //Verifica se o estudo adicional já foi utilizado
                    contextQuery.Command = @"SELECT TOP 1
                                        1
                                FROM    dbo.FORMACAOPESSOAL_ESTUDOADICIONAL
                                WHERE   ESTUDOADICIONALID = @ESTUDOADICIONALID ";
                    contextQuery.Parameters.Add("@ESTUDOADICIONALID", estudoAdicional.EstudoAdicionalId);

                    obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        mensagens.Add("Este estudo adicional não pode ser alterado pois já foi utilizado.");
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public ValidacaoDados ValidaRemocao(int estudoAdicionalId)
        {
            List<string> mensagens = new List<string>();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            object obj = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (estudoAdicionalId <= 0)
            {
                return validacaoDados;
            }

            //Verifica se o estudo adicional já foi utilizado
            contextQuery.Command = @"SELECT TOP 1
                                        1
                                FROM    dbo.FORMACAOPESSOAL_ESTUDOADICIONAL
                                WHERE   ESTUDOADICIONALID = @ESTUDOADICIONALID ";
            contextQuery.Parameters.Add("@ESTUDOADICIONALID", estudoAdicionalId);

            obj = ctx.GetReturnValue(contextQuery);

            if (obj != null)
            {
                mensagens.Add("Este estudo adicional não pode ser removido pois já foi utilizado.");
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void Insere(Entidades.EstudoAdicional estudoAdicional)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @"INSERT  INTO dbo.ESTUDOADICIONAL
                                            ( NOME )
                                    VALUES  ( @NOME )"
                };
                contextQuery.Parameters.Add("@NOME", estudoAdicional.Nome.Trim().ToUpper());

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

        public void Altera(Entidades.EstudoAdicional estudoAdicional)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @"UPDATE  ESTUDOADICIONAL
                                SET     NOME = @NOME
                                WHERE   ESTUDOADICIONALID = @ESTUDOADICIONALID "
                };
                contextQuery.Parameters.Add("@NOME", estudoAdicional.Nome.Trim().ToUpper());
                contextQuery.Parameters.Add("@ESTUDOADICIONALID", estudoAdicional.EstudoAdicionalId);

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

        public void Remove(int estudoAdicionalId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" DELETE  ESTUDOADICIONAL
                                WHERE   ESTUDOADICIONALID = @ESTUDOADICIONALID "
                };
                contextQuery.Parameters.Add("@ESTUDOADICIONALID", estudoAdicionalId);

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