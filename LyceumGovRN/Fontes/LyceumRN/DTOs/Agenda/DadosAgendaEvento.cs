using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs.Agenda
{
    public class DadosAgendaEvento
    {
        public int AgendaId { get; set; }

        public int EventoId { get; set; }

        public int TipoEventoId { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        public int ParticipaUnidadeId { get; set; }

        public int ParticipaCursoId { get; set; }

        public bool CursoPorUnidade { get; set; }
    }
}
