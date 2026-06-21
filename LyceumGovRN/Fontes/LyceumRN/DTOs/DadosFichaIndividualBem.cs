using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosFichaIndividualBem
    {
        public int BemId { get; set; }

        public string Descricao { get; set; }

        public string Numero { get; set; }

        public string Conta { get; set; }

        public string DataIncorporacao { get; set; }

        public string Operacao { get; set; }

        public string DocumentoHabil { get; set; }

        public string Historico { get; set; }

        public decimal ValorInicial { get; set; }

        public string Sigla { get; set; }

        public string UnidadeAdministrativa { get; set; }

        public string EnderecoUnidadeAdministrativa { get; set; }

        public string EmailUnidadeAdministrativa { get; set; }

        public string TelefoneUnidadeAdministrativa { get; set; }
    }
}
