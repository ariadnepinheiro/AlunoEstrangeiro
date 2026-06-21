using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.PrestacaoContas.DTOs
{
    public class DadosReceitasProjetoRecebidas
    {
        public string Censo { get; set; }

        public decimal SaldoAnterior { get; set; }

        public decimal Repasses { get; set; }

        public decimal Despesas { get; set; }

        public decimal Rendimentos { get; set; }

        public decimal Devolucoes { get; set; }

        public int ProgramaTrabalhoId { get; set; }

        public string Programa { get; set; }

        public int PlanoTrabalhoId { get; set; }

        public string PlanoTrabalho { get; set; }

        public string Pt { get; set; }

        public string PtRes { get; set; }

        public decimal CreditosDebitos { get; set; }
    }
}
