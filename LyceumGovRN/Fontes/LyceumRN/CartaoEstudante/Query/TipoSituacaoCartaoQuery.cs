using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Entity;

namespace Techne.Lyceum.RN.CartaoEstudante.Query
{
    class TipoSituacaoCartaoQuery: QueryBase<TipoSituacaoCartaoQuery>
    {
        private const string TABELA_TIPO_SITUACAO_CARTAO = "Lyceum.CartaoEstudante.TipoSituacaoCartao";

        TipoSituacaoCartaoQuery() { }

        public TipoSituacaoCartao ObtemTipoSituacaoCartaoPor(string codigo)
        {
            string SELECT_TIPO_SITUACAO_CARTAO = "SELECT * FROM " + TABELA_TIPO_SITUACAO_CARTAO + " WHERE CODIGOSITUACAOCARTAO = @CODIGO";

            TipoSituacaoCartao tipoSituacaoCartao = ObtemPor<TipoSituacaoCartao>(
                SELECT_TIPO_SITUACAO_CARTAO,
                codigo
            );

            return tipoSituacaoCartao;
        }
    }
}
