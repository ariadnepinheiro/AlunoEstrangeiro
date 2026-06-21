using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.RN.RenovacaoMatricula
{
    public class RenovacaoConfirmacaoMatricula : RNBase
    {
        public void InsereRenovacaoConfirmacaoMatricula(DataContext context, TceConfirmacaoMatricula confirmacaoMatricula,RenovacaoMatricula.Entidades.Renovacao renovacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO dbo.RENOVACAO_CONFIRMACAOMATRICULA
                                    ( RENOVACAOID ,
                                      CONFIRMACAOMATRICULAID
                                    )
                            VALUES  ( @RENOVACAOID,
                                      @CONFIRMACAOMATRICULAID
                                    ) ";

                contextQuery.Parameters.Add("@RENOVACAOID", renovacao.RenovacaoId);
                contextQuery.Parameters.Add("@CONFIRMACAOMATRICULAID", confirmacaoMatricula.IdConfirmacaoMatricula);

                context.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool PossuiRenovacaoConfirmacaoMatriculaPor(DataContext context, TceConfirmacaoMatricula confirmacaoMatricula, RenovacaoMatricula.Entidades.Renovacao renovacao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                FROM RENOVACAO_CONFIRMACAOMATRICULA (NOLOCK)
                                WHERE RENOVACAOID = @RENOVACAOID
									AND CONFIRMACAOMATRICULAID = @CONFIRMACAOMATRICULAID
	                                 ";

                contextQuery.Parameters.Add("@RENOVACAOID", renovacao.RenovacaoId);
                contextQuery.Parameters.Add("@CONFIRMACAOMATRICULAID", confirmacaoMatricula.IdConfirmacaoMatricula);


                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
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
