using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Entity;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.CartaoEstudante.Query
{
    class TipoCancelamentoQuery: QueryBase<TipoCancelamentoQuery>
    {
        private const string TABELA_TIPO_CANCELAMENTO = "Lyceum.CartaoEstudante.TipoCancelamento";

        TipoCancelamentoQuery() { }

        #region MÉTODOS
        public TipoCancelamento ObtemTipoCancelamentoPor(int codigo)
        {
            string SELECT_TIPO_CANCELAMENTO = "SELECT * FROM " + TABELA_TIPO_CANCELAMENTO + " WHERE CODIGOCANCELAMENTO = @CODIGO";
            
            TipoCancelamento tipoCancelamento = ObtemPor<TipoCancelamento>(
                SELECT_TIPO_CANCELAMENTO,
                codigo
            );

            return tipoCancelamento;
        }

        #endregion
    }
}
