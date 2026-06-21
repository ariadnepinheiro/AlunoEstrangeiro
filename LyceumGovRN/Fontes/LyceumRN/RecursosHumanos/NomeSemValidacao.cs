using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.RecursosHumanos
{
    public class NomeSemValidacao : RNBase
    {
        public enum Tipo
        {
            [StringValue("Nome")]
            Nome = 1,
            [StringValue("Nome Mãe")]
            Mae = 2,
            [StringValue("Nome Pai")]
            Pai = 3
        }

        public string ObtemNomePor(decimal pessoa, int tipo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT NOME
                                    FROM RecursosHumanos.NOMESEMVALIDACAO
                                    WHERE PESSOA = @PESSOA
	                                    AND TIPO = @TIPO ";

                contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, pessoa);
                contextQuery.Parameters.Add("@TIPO", SqlDbType.Int, tipo); 

                resultado = contexto.GetReturnValue<string>(contextQuery);

                return resultado;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
                contexto.Dispose();
            }
        }
    }
}
