using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Entity;

namespace Techne.Lyceum.RN.CartaoEstudante.Query
{
    class LoteRemessaQuery : QueryBase<LoteRemessaQuery>
    {
        private static readonly string TABELA_LOTE_REMESSA = "LYCEUM.CartaoEstudante.LOTEREMESSA";

        LoteRemessaQuery() { }

        public LoteRemessa ObtemLoteRemessaPor(int loteRemessaId)
        {
            string SELECT_LOTE_REMESSA =
                "SELECT * FROM " + TABELA_LOTE_REMESSA + " " +
                "WHERE " +
                "    LOTEREMESSAID = @LOTEREMESSAID";

            LoteRemessa loteRemessa = ObtemPor<LoteRemessa>(
                SELECT_LOTE_REMESSA,
                loteRemessaId
            );

            return loteRemessa;
        }
    }
}
