namespace Techne.Lyceum.RN.Entidades
{
    using Seeduc.Infra.Entities;
using System;

    public class LyMatricula : IEntity
    {
        public string Aluno { get; set; }

        public decimal Ano { get; set; }

        public string Disciplina { get; set; }

        public decimal Semestre { get; set; }

        public string Turma { get; set; }

        public string SitMatricula { get; set; }

        public string Dependencia { get; set; }

        public string Matricula { get; set; }

        public decimal SerieReferencia { get; set; }

        public string DisciplinaReferencia { get; set; }

        public string Concomitante { get; set; }

        public string EducEspecial { get; set; }

        public string MaisEducacao { get; set; }

        public string CobrancaSep { get; set; }

        public DateTime? DtInsercao { get; set; }

        public DateTime? DtMatricula { get; set; }

        public DateTime? DtUltalt { get; set; }
    }
}