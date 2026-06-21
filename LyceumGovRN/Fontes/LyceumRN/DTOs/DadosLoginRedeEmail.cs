using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosLoginRedeEmail
    {
        public decimal Pessoa { get; set; }

        public int LoginRedeId { get; set; }

        public string Cpf { get; set; }

        public string Nome { get; set; }

        public string IdFuncional { get; set; }

        public string EmailOffice365 { get; set; }

        public string EmailGoogleEducation { get; set; }

        public string EmailAlternativo { get; set; }

        public string LoginRede { get; set; }

        public string UsuarioId { get; set; }
    }
}
