using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Query;
using Techne.Lyceum.RN.CartaoEstudante.Entity;

namespace Techne.Lyceum.RN.CartaoEstudante.Service
{
    public class OperadoraService: SingletonBase<OperadoraService>
    {
        private static readonly OperadoraQuery operadoraQuery = OperadoraQuery.Instancia;

        OperadoraService() { }

        public List<Operadora> ListaTodos()
        {
            return operadoraQuery.ListaTodos<Operadora>();
        }
    }
}
