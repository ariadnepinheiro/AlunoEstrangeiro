using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.PrestacaoContas.DTOs
{
    public class DadosContaCorrente
    {
        public int ContaCorrenteId { get; set; }

        public int IdRegional { get; set; }

        public string Censo { get; set; }

        public string Banco { get; set; }

        public string BancoNome { get; set; }

        public string Agencia { get; set; }

        public string AgenciaNome { get; set; }

        public string Conta { get; set; }
    }
}
