using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.PrestacaoContas.DTOs
{
    public class DadosFormulario2
    {
        public DadosFormulario2()
        {
            Despesas = new List<DadosDespesa>();
        }

        public string PeriodoPrestacao { get; set; }

        public string SaldoAnterior { get; set; }

        public string RepassesRecebidos { get; set; }

        public int ProgramaTrabalhoId { get; set; }

        public string Programa { get; set; }

        public int PlanoTrabalhoId { get; set; }

        public string PlanoTrabalho { get; set; }

        public string Pt { get; set; }

        public string PtRes { get; set; }

        public string SaldoInicial { get; set; }

        public List<DadosDespesa> Despesas { get; set; }

        public string TotalPequenasDespesas { get; set; }

        public string TotalDespesas { get; set; }

        public string SaldoFinal { get; set; }

        public string SaldoFinalComRendimento { get; set; }

        public string CreditosDebitos { get; set; }
    }
}
