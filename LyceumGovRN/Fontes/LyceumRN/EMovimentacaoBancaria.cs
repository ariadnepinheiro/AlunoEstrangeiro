using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN
{
    public class EMovimentacaoBancaria
    {
        public decimal? ID { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public decimal IDPrestacaoContas { get; set; }
        public decimal IDContaBanco { get; set; }
        public decimal? IDFonteRecurso { get; set; }
        public decimal IDTipoMovimento { get; set; }
        public decimal? IDFinalidade { get; set; }

        public string SiglaFonteRecurso { get; set; }
        public string SiglaTipoMovimento { get; set; }
        public decimal Banco { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }

        public EMovimentacaoBancaria()
        {
            Data = DateTime.MinValue;
        }
        public void OnValidateFieldValue()
        {

        }
    }
}
