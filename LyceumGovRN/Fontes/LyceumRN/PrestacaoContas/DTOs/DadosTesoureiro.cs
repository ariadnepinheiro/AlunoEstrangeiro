using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.PrestacaoContas.DTOs
{
   public class DadosTesoureiro
    {
        public bool PossuiIdFuncional { get; set; }

        public decimal? TesoureiroPessoa { get; set; }

        public int? TesoureiroId { get; set; }

        public string Censo { get; set; }

        public string Nome { get; set; }

        public string Rg { get; set; }

        public string Cpf { get; set; }

        public string Endereco { get; set; }

        public string Numero { get; set; }

        public string Complemento { get; set; }

        public string Bairro { get; set; }

        public string Municipio { get; set; }

        public string Email { get; set; }

        public string Telefone { get; set; }

        public string IdFuncional { get; set; }
    }
}
