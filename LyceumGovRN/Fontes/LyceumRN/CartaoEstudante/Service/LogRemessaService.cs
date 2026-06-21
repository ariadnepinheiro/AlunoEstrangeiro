using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Query;
using Techne.Lyceum.RN.CartaoEstudante.Entity;

namespace Techne.Lyceum.RN.CartaoEstudante.Service
{
    public class LogRemessaService : SingletonBase<LogRemessaService>
    {
        private static readonly LogRemessaQuery logRemessaQuery = LogRemessaQuery.Instancia;

        LogRemessaService() { }

        public DateTime? ObtemUltimaDataEnvioPor(int remessaId)
        {
            LogRemessa logRemessa = logRemessaQuery.ObtemUltimoLogPor(remessaId);

            return logRemessa != null ? logRemessa.DataEnvio : default(DateTime?);
        }
    }
}
