using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.CartaoEstudante.Entities
{
    public class LoteRemessa
    {
        public DateTime data { get; set; }

        public string nome { get; set; }

        public int codOperadora { get; set; }

        public int quantidadeRegistros { get; set; }
    }
}
