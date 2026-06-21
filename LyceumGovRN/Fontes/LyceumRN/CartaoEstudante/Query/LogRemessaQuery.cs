using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Entity;

namespace Techne.Lyceum.RN.CartaoEstudante.Query
{
    class LogRemessaQuery : QueryBase<LogRemessaQuery>
    {
        private static readonly string TABELA_LOGREMESSA = "LYCEUM.CartaoEstudante.LOGREMESSA";

        LogRemessaQuery() { }

        internal LogRemessa ObtemUltimoLogPor(int remessaId)
        {
            string SELECT_MAX_LOGREMESSA = "SELECT TOP 1 * FROM " + TABELA_LOGREMESSA + " WHERE REMESSAID = " + remessaId.ToString() + " ORDER BY DATAENVIO DESC";

            LogRemessa log = ObtemPor<LogRemessa>(SELECT_MAX_LOGREMESSA);

            return log;
        }
    }
}
