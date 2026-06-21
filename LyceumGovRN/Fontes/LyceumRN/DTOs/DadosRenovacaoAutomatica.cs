using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosRenovacaoAutomatica
    {
        public bool PorUnidadeEnsino { get; set; }

        public string UnidadeEnsino { get; set; }

        public bool PorRegional { get; set; }       

        public int Regional { get; set; }

        public List<string> ListaUnidadesEnsino { get; set; }

        public string UsuarioResponsavel { get; set; }
    }
}
