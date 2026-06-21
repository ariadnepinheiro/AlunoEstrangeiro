using System;
using Techne.Lyceum.RN.CartaoEstudante.Entity;
using Seeduc.Infra.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.CartaoEstudante.Query
{
    class RetornoCriticaQuery: QueryBase<RetornoCriticaQuery>
    {
        private const string TABELA_RETORNO_CRITICA = "Lyceum.CartaoEstudante.RetornoCritica";
        public static readonly string INSERT_RETORNO_CRITICA = 
            "INSERT INTO " + TABELA_RETORNO_CRITICA +
            " (RETORNOID, TIPORETORNOCRITICAID, OBSERVACAO) " +
            "VALUES " +
            " (@RETORNOID, @TIPORETORNOCRITICAID, @OBSERVACAO)";

        RetornoCriticaQuery() { }

        public void Insere(RetornoCritica retornoCritica)
        {
            AplicarModificacoes(
                INSERT_RETORNO_CRITICA,
                retornoCritica.RetornoId, retornoCritica.TipoRetornoCriticaId, retornoCritica.Observacao
            );
        }

        internal System.Data.DataTable ObtemPor(int retornoId)
        {
            string SELECT_RETORNO = @"select rc.RETORNOID 
                                            ,rc.RETORNOCRITICAID 
                                            ,rc.DATAINCLUSAO
                                            ,rc.OBSERVACAO
                                            ,trc.CODIGOCRITICA
                                            ,trc.DESCRICAO
                                            from  CartaoEstudante.RETORNOCRITICA rc 
                                            inner join CartaoEstudante.TIPORETORNOCRITICA trc
                                            on trc.TIPORETORNOCRITICAID = rc.TIPORETORNOCRITICAID 
                                            WHERE rc.RETORNOID = @RETORNOID";

            return ObterDataTable(SELECT_RETORNO, retornoId);            
        }
    }
}
