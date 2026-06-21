using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.PrestacaoContas.DTOs
{
    public class DadosDespesa
    {
        public string Evento { get; set; }

        public string FornecedorBeneficiario { get; set; }

        public string DocumentoFiscal { get; set; }

        public string Valor { get; set; }

        public decimal ValorCalculo { get; set; }

        public DateTime DataPagamento { get; set; }
    }
}
