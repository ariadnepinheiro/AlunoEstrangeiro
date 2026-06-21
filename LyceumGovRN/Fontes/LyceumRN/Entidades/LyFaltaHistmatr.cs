using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class LyFaltaHistmatr : IEntity
    {
        public string Aluno { get; set; }

        public decimal? Ordem { get; set; }

        public decimal? Ano { get; set; }

        public decimal? Semestre { get; set; }

        public string Disciplina { get; set; }

        public string FreqId { get; set; }

        public decimal? Faltas { get; set; }

        public string Turma { get; set; }
    }
}
