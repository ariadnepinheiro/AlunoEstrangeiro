using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.Entidades
{
    public class SerieAbsorvida
    {
        public int SerieAbsorvidaId { get; set; }

        public string UnidadeEnsinoDestinoId { get; set; }

        public string UnidadeEnsinoOrigemId { get; set; }

        public string CursoOrigemId { get; set; }

        public string TurnoOrigemId { get; set; }

        public int SerieOrigemId { get; set; }

        public DateTime DataAbsorcao { get; set; }

        public DateTime DataCadastro { get; set; }

        public string Matricula { get; set; }

        public int NivelAbsorcaoId { get; set; }        
    }
}
