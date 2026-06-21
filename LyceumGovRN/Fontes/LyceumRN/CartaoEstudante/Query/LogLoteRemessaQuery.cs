using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Entity;

namespace Techne.Lyceum.RN.CartaoEstudante.Query
{
    class LogLoteRemessaQuery : QueryBase<LogLoteRemessaQuery>
    {
        private static readonly string TABELA_LOGLOTEREMESSA = "LYCEUM.CartaoEstudante.LOGLOTEREMESSA";

        LogLoteRemessaQuery() { }

        internal LogLoteRemessa ObtemUltimoLogPor(int loteRemessaId)
        {
            string SELECT_MAX_LOGLOTEREMESSA = "SELECT TOP 1 * FROM " + TABELA_LOGLOTEREMESSA + " WHERE LOTEREMESSAID = " + loteRemessaId.ToString() + " ORDER BY DATAENVIO DESC";

            LogLoteRemessa log = ObtemPor<LogLoteRemessa>(SELECT_MAX_LOGLOTEREMESSA);

            return log;
        }

        internal LogLoteRemessa ObtemPrimeiroLogPor(int loteRemessaId)
        {
            string SELECT_MIN_LOGLOTEREMESSA = "SELECT TOP 1 * FROM " + TABELA_LOGLOTEREMESSA + " WHERE LOTEREMESSAID = " + loteRemessaId.ToString() + " ORDER BY DATAENVIO";

            LogLoteRemessa log = ObtemPor<LogLoteRemessa>(SELECT_MIN_LOGLOTEREMESSA);

            return log;
        }
    }
}
