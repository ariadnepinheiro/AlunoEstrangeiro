using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.CartaoEstudante.Entities
{
    public class RetornoLoteRemessa
    {
        public List<LoteRemessa> listaLoteRemessa { get; set; }

        public int quantidadeLotes { get; set; }
    }
}
