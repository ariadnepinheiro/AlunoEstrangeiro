using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.Entidades
{
    public class LyHorOper
    {
        public string Turno { get; set; }

        public string Faculdade { get; set; }

        public Decimal DiaSemana { get; set; }

        public Decimal Aula { get; set; }

        public DateTime HorainiAula { get; set; }

        public DateTime HorafimAula { get; set; }

        public string Curso { get; set; }

        public string Curriculo { get; set; }

        public Decimal Serie { get; set; }

        public Decimal Ordem { get; set; }

        public DateTime StampAtualizacao { get; set; }

        public Decimal DuracaoAula { get; set; }
    }
}
