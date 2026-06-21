using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.RecursosHumanos
{
    public class RegimeContratacao : RNBase
    {
        public enum Regime
        {
            [StringValue("Concursado Efetivo")]
            Concursado = 1,
            [StringValue("Requisição de Outro Órgão")]
            RequisicaoOutroOrgao = 2,
            [StringValue("Contrato Temporário")]
            ContratoTemporario = 3
        }


        public DataTable ListaRegimeContratacao()
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contexto = DataContextBuilder.FromHades.UsingNoLock();

                contextQuery.Command = @"SELECT *
                                           FROM RECURSOSHUMANOS.REGIMECONTRATACAO
                                         ";

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }

            return dt;
        }

        public DataTable ListaRegimeContratacaoIngresso()
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable regimes = null;

            try
            {
                contexto = DataContextBuilder.FromHades.UsingNoLock();

                contextQuery.Command = @"SELECT *
                                           FROM RECURSOSHUMANOS.REGIMECONTRATACAO
                                           WHERE INGRESSODOCENTE = 1
                                         ";

                regimes = contexto.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }

            return regimes;
        }
    }
}
