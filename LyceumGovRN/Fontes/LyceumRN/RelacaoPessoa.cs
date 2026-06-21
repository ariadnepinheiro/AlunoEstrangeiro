using System;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN
{
    /// <summary>
    /// Summary description for Class1
    /// </summary>
    public class RelacaoPessoa : RNBase
    {
        public static void Inserir(Entidades.RelacaoPessoa relacaoPessoa)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"INSERT INTO RELACAOPESSOA
                                        (PESSOAID,
                                         PARENTEID,
                                         PARENTESCOID
                                        ) 
                                VALUES  (@PESSOAID,
                                         @PARENTEID,
                                         @PARENTESCOID
                                        )"
                    };
                    contextQuery.Parameters.Add("@PESSOAID", relacaoPessoa.PessoaId);
                    contextQuery.Parameters.Add("@PARENTEID", relacaoPessoa.ParenteId);
                    contextQuery.Parameters.Add("@PARENTESCOID", relacaoPessoa.ParentescoId);


                    ctx.ApplyModifications(contextQuery);
                }

                catch (Exception ex)
                {
                    ctx.Abandon();
                    var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                       Environment.NewLine, Convert.ToString(ex.Message));
                    throw new Exception(mensagem);
                }
            }
        }

        public void Insere(DataContext contexto, decimal pessoaId, decimal parenteId, int parentescoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO RELACAOPESSOA
                                        (PESSOAID,
                                         PARENTEID,
                                         PARENTESCOID
                                        ) 
                                VALUES  (@PESSOAID,
                                         @PARENTEID,
                                         @PARENTESCOID
                                        ) ";

            contextQuery.Parameters.Add("@PESSOAID", pessoaId);
            contextQuery.Parameters.Add("@PARENTEID", parenteId);
            contextQuery.Parameters.Add("@PARENTESCOID", parentescoId);

            contexto.ApplyModifications(contextQuery);
        }

        public static void Excluir(decimal Parente)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"DELETE FROM RELACAOPESSOA WHERE PARENTEID = @PARENTEID"
                    };
                    contextQuery.Parameters.Add("@PARENTEID", Parente);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception ex)
                {
                    ctx.Abandon();
                    var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                       Environment.NewLine, Convert.ToString(ex.Message));
                    throw new Exception(mensagem);
                }

            }
           
        }


        public static DataTable RetornaParentesco(int pessoa)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                        (@"SELECT PARENTESCOID,PARENTEID FROM RELACAOPESSOA
                                    WHERE PESSOAID = @PESSOAID"); 
                    {    
                    contextQuery.Parameters.Add("@PESSOAID", pessoa);
                    }

                    return Consultar(contextQuery);
                }
                catch (Exception ex)
                {
                    ctx.Abandon();
                    var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                       Environment.NewLine, Convert.ToString(ex.Message));
                    throw new Exception(mensagem);
                }
            } 
        }
        public static bool ConsultaIrmao(Entidades.RelacaoPessoa pessoa)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery

                    {
						Command = @"SELECT PESSOAID FROM RELACAOPESSOA 
                                    WHERE 
                                    PESSOAID = @PESSOAID
                                    AND PARENTEID = @PARENTEID"
                    };
                    contextQuery.Parameters.Add("@PESSOAID", pessoa.PessoaId);
                    contextQuery.Parameters.Add("@PARENTEID", pessoa.ParenteId);

					object obj = ctx.GetReturnValue(contextQuery);

					if (obj == null)
					{
						return false;
					}
					else
					{
						return true;
					}
                }
                catch (Exception ex)
                {
                    ctx.Abandon();
                    var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                       Environment.NewLine, Convert.ToString(ex.Message));
                    throw new Exception(mensagem);
                }
            }

        }


        public void RemovePessoaDuplicadaPor(DataContext ctx, decimal pessoaId)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE RELACAOPESSOA 
                                        WHERE  (PESSOAID = @PESSOAID OR PARENTEID = @PESSOAID)  ";

                contextQuery.Parameters.Add("@PESSOAID", pessoaId);

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
