using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.CartaoEstudante.DTO;
using Techne.Lyceum.RN.CartaoEstudante.Entity;

namespace Techne.Lyceum.RN.CartaoEstudante.Query
{
    class RegistrosRetornoQuery : QueryBase<RegistrosRetornoQuery>
    {
        private static readonly string TABELA_RETORNO = "LYCEUM.CartaoEstudante.RETORNO";
        private static readonly string TABELA_REMESSA = "LYCEUM.CartaoEstudante.REMESSA";

        RegistrosRetornoQuery() {
            NomeTabela = TABELA_RETORNO;
        }

        #region MÉTODOS

        public long[] ListaSolicitacoes(int operadoraId)
        {
            string SELECT_SOLICITACOES =
                "SELECT R.REMESSAID " +
                "FROM " + TABELA_REMESSA + " R " +
                "WHERE " +
                "  REMESSAID NOT IN (" +
                "    SELECT DISTINCT REMESSAID " +
                "    FROM " + TABELA_RETORNO + " RE " +
                "    WHERE " +
                "      RE.SITUACAOPROCESSAMENTO IN (1,2,10) " + 
                "      AND RE.REPROCESSAR = 0 " + 
                "  ) " +
                "  AND R.OPERADORAID = @OPERADORAID " +
                "  AND R.DATAINCLUSAO < GETDATE() - 2 " +
                "ORDER BY " +
                "  R.REMESSAID";

            List<long> solicitacoes = ListaPorDe<long>(
                SELECT_SOLICITACOES,
                operadoraId
            );

            return solicitacoes.ToArray<long>();
        }

        public void Insere(Retorno retorno, IList<TipoRetornoCritica> criticas)
		{
            ContextQuery retornoCtx = new ContextQuery(RetornoQuery.INSERT_RETORNO);
            DataContext dataContext = DataContextBuilder.FromLyceum.UsingLock();
            RetornoQuery retornoQuery = RetornoQuery.Instancia;  
            CriticaQuery criticaQuery = CriticaQuery.Instancia;
            RetornoCriticaQuery retornoCriticaQuery = RetornoCriticaQuery.Instancia;

            try
            {
                VerificaEAtribuiParametros(
                    retornoCtx.Command,
                    new object[] { 
                        retorno.RemessaId, retorno.OperadoraId, retorno.IdBeneficiario, retorno.DataProcessamento, retorno.SituacaoProcessamento
                    },
                    retornoCtx
                );

                dataContext.ApplyModifications(retornoCtx);

                if (criticas.Count > 0)
                {
                    int ultimoRetorno = retornoQuery.ObtemUltimoRetornoID();
                    Retorno retornoInserido = retornoQuery.ObtemPorId(ultimoRetorno);
                    
                    TipoRetornoCritica tipoRetornoCritica;
                    
                    foreach (TipoRetornoCritica critica in criticas)
                    {
                        tipoRetornoCritica = criticaQuery.ObtemCriticaPor(critica.CodigoCritica);

                        if (tipoRetornoCritica.CodigoCritica == null)
                            tipoRetornoCritica = criticaQuery.ObtemCriticaPadrao();

                        ContextQuery retornoCriticaCtx = new ContextQuery(RetornoCriticaQuery.INSERT_RETORNO_CRITICA);
                        VerificaEAtribuiParametros(
                            retornoCriticaCtx.Command,
                            new object[] { 
                                retornoInserido.RetornoId, tipoRetornoCritica.TipoRetornoCriticaId, null 
                            },
                            retornoCriticaCtx
                        );

                        dataContext.ApplyModifications(retornoCriticaCtx);
                    }
                }
            }
            catch (Exception ex)
            {
                dataContext.Abandon();
                throw ex;
            }
            finally
            {
                dataContext.Dispose();
            }
		}

        #endregion
    }
}
