using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosNecessidadeEspecialFiltroProcesso
    {
        public int FiltroProcessoId {get; set;}

        public string FiltroProcessoDescricao { get; set; }

        public int NecessidadeEspecialId { get; set; }

        public string NecessidadeEspecialDescricao { get; set; }

        public bool Habilitado { get; set; }
    }
}
