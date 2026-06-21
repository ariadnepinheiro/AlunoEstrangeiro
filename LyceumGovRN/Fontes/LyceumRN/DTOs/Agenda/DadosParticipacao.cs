using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs.Agenda
{
    public class DadosParticipacao
    {
        public bool ParticipaTotal { get; set; }

        public bool ParticipaUnidade { get; set; }

        public bool ParticipaCurso { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        public string UnidadeEnsino { get; set; }

        public string Curso { get; set; }

        public string Turno { get; set; }

        public int Serie { get; set; }
    }
}