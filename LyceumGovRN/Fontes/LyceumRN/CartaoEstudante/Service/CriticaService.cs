using System.Collections.Generic;
using Techne.Lyceum.RN.CartaoEstudante.Query;
using Techne.Lyceum.RN.CartaoEstudante.Entity;

namespace Techne.Lyceum.RN.CartaoEstudante.Service
{
    public class CriticaService: SingletonBase<CriticaService>
    {
        private static readonly CriticaQuery criticaQuery = CriticaQuery.Instancia;

        CriticaService() { }

        public void InsereCriticasPadrao()
        {
            criticaQuery.InsereCriticasPadrao();
        }
    }
}
