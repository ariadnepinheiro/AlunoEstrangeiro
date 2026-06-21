using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosRelatorioDependencia
    {
        public string UnidadeEnsino { get; set; }

        public int CampanhaId { get; set; }

        public string Dependencia { get; set; }

        public string Edificacao { get; set; }

        public string Pavimento { get; set; }

        public int? CampanhaEscolaId { get; set; }

        public int? RespostaDependenciaId { get; set; }

        public string Tipo_Banheiro { get; set; }

    }
}
