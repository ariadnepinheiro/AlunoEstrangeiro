using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.PrestacaoContas.DTOs
{
    public class DadosSaldoInicial
    {        

        public decimal? ValorInicial { get; set; }

        public int? SaldoInicialID { get; set; }

        public DateTime DataReferenciaVinculo { get; set; }
    }
}
