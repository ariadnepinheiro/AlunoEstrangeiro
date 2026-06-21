using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.CartaoEstudante.Entities
{
    public class RetornoRemessa
    {
        public string nomeLote { get; set; }

        public int quantidadeRegistros { get; set; }

        public List<Remessa> remessa { get; set; }
    }
}
