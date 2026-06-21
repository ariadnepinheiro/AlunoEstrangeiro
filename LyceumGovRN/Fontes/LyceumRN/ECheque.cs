using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN
{
    public class ECheque
    {
        public decimal? ID { get; set; }
        public decimal? IDContaBanco { get; set; }
        public decimal? Numero { get; set; }
        public decimal? Valor { get; set; }
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataApresentacao { get; set; }
        public decimal? IDCompra { get; set; }

        public decimal? IDPrestacaoContas { get; set; }
        public decimal? IDFonteRecurso { get; set; }
        public string SiglaFonteRecurso { get; set; }
        public decimal Banco { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }

    }
}
