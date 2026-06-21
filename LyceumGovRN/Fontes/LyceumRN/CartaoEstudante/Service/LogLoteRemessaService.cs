using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Entity;
using Techne.Lyceum.RN.CartaoEstudante.Query;

namespace Techne.Lyceum.RN.CartaoEstudante.Service
{
    public class LogLoteRemessaService : SingletonBase<LogLoteRemessaService>
    {
        private static readonly LogLoteRemessaQuery logLoteRemessaQuery = LogLoteRemessaQuery.Instancia;

        LogLoteRemessaService() { }

        public DateTime? ObtemUltimaDataEnvioPor(int loteRemessaId)
        {
            LogLoteRemessa logLoteRemessa = logLoteRemessaQuery.ObtemUltimoLogPor(loteRemessaId);

            return logLoteRemessa != null ? logLoteRemessa.DataEnvio : default(DateTime?);
        }

        public DateTime? ObtemPrimeiraDataEnvioPor(int loteRemessaId)
        {
            LogLoteRemessa logLoteRemessa = logLoteRemessaQuery.ObtemPrimeiroLogPor(loteRemessaId);

            return logLoteRemessa != null ? logLoteRemessa.DataEnvio : default(DateTime?);
        }
    }
}
