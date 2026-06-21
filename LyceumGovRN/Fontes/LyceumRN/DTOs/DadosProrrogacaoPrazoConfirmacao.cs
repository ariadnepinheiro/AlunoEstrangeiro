using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosProrrogacaoPrazoConfirmacao
    {
        public bool PorUnidadeEnsino { get; set; }

        public string UnidadeEnsino { get; set; }

        public bool PorMunicipio { get; set; }

        public string Municipio { get; set; }

        public bool PorRegional { get; set; }

        public int Regional { get; set; }

        public bool Todos { get; set; }

        public int Dias { get; set; }

        public string UsuarioResponsavel { get; set; }
    }
}
