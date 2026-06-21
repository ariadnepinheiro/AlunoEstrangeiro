using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Entity;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.CartaoEstudante.Query
{
    class RetornoLoginQuery : QueryBase<RetornoLoginQuery>
    {
        private const string TABELA_RETORNO_LOGIN = "Lyceum.CartaoEstudante.RetornoLogin";

        RetornoLoginQuery() {}

        public RetornoLogin ObtemPor(string aluno)
        {
            string SELECT_RETORNO = @"SELECT * FROM " + TABELA_RETORNO_LOGIN + " WHERE ALUNO = @ALUNO";
            RetornoLogin retornoLogin = ObtemPor<RetornoLogin>(SELECT_RETORNO, aluno);

            return retornoLogin;
        }

        public void Insere(RetornoLogin retornoLogin)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO CARTAOESTUDANTE.RETORNOLOGIN 
                                                    (OPERADORAID, 
                                                     ALUNO, 
                                                     IDBENEFICIARIO, 
                                                     DATACONFIRMACAOALUNO, 
                                                     LOGINOPERADORA) 
                                        VALUES      (@OPERADORAID, 
                                                     @ALUNO, 
                                                     @IDBENEFICIARIO,
                                                     @DATACONFIRMACAOALUNO, 
                                                     @LOGINOPERADORA) ";

                contextQuery.Parameters.Add("@OPERADORAID", SqlDbType.Int, retornoLogin.OperadoraId);
                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, retornoLogin.Aluno);
                contextQuery.Parameters.Add("@IDBENEFICIARIO", SqlDbType.Int, retornoLogin.IdBeneficiario);
                contextQuery.Parameters.Add("@DATACONFIRMACAOALUNO", SqlDbType.DateTime, retornoLogin.DataConfirmacaoAluno);
                contextQuery.Parameters.Add("@LOGINOPERADORA", SqlDbType.VarChar, retornoLogin.LoginOperadora);

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
    }
}