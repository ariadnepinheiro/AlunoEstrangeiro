using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Query;
using Techne.Lyceum.RN.CartaoEstudante.Entity;

namespace Techne.Lyceum.RN.CartaoEstudante.Service
{
    public class LoteRemessaService : SingletonBase<LoteRemessaService>
    {
        private static readonly LoteRemessaQuery loteRemessaQuery = LoteRemessaQuery.Instancia;
        private static readonly LoteRemessaService logRemessaService = LoteRemessaService.Instancia;

        public string ObtemNomeLotePor(int loteRemessaId)
        {
            LoteRemessa loteRemessa = loteRemessaQuery.ObtemLoteRemessaPor(loteRemessaId);

            return loteRemessa == null ? "" : loteRemessa.Nome;
        }
    }
}
