using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.Entidades
{
    public class LyLicencaPessoa
    {
        public decimal Pessoa { get; set; }

        public decimal Ordem { get; set; }

        public DateTime Dtini { get; set; }

        public DateTime? Dtfim { get; set; }

        public string Motivo { get; set; }

        public DateTime? DtRetorno { get; set; }

        public DateTime StampAtualizacao { get; set; }
    }
}
