using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN
{
    public class CampoEnvioMensagem
    {
        /// <summary>
        /// Lista os campos a serem substituídos na mensagem 
        /// </summary>
        /// <param name="IDENTIFICADORCAMPO"></param>
        /// <returns></returns>
        public DataTable ListaCampoSubstituido(string IDENTIFICADORCAMPO)
        {
            DataContext ctx = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                ctx = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.Command = @"SELECT C.IDENTIFICADORCAMPO,
                                                C.NOMECAMPO
                                           FROM [LYCEUM].[dbo].[CAMPOENVIOMENSAGEM] C
                                          WHERE C.IDENTIFICADORCAMPO IN (" + IDENTIFICADORCAMPO + ")";

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
    }
}
