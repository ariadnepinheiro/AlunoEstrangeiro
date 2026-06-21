using System;

namespace Techne.Lyceum.RN.Entidades
{
    public class LyHistMatricula
    {
        public string Aluno { get; set; }

        public Decimal Ordem { get; set; }

        public Decimal Ano { get; set; }

        public Decimal Semestre { get; set; }

        public string Disciplina { get; set; }

        public string Turma { get; set; }

        public string TurmaHist { get; set; }

        public string NotaFinal { get; set; }

        public string SituacaoHist { get; set; }

        //public Decimal? PercPresenca { get; set; }

        public Decimal? HorasAula { get; set; }

        public Decimal? AulasDadas { get; set; }

        public Decimal? AulasPrevistas { get; set; }

        public Decimal? Creditos { get; set; }

        public string Observacao { get; set; }

        public string NivelPresenca { get; set; }

        public Decimal? Serie { get; set; }

        public DateTime? DtInicio { get; set; }

        public DateTime? DtFim { get; set; }

        public DateTime? DtUltalt { get; set; }

        public DateTime DtMatricula { get; set; }

        public string Matricula { get; set; }

        public string UnidadeEnsino  { get; set; }

        public string Outras { get; set; }

        public decimal SerieReferencia { get; set; }

        public string DisciplinaReferencia { get; set; }

        public string Dependencia { get; set; }

        public int? FaltaFinal { get; set; }

        public string Concomitante { get; set; }

        public string OptativaReforco { get; set; }

        public string EducEspecial { get; set; }

        public string MaisEducacao { get; set; }
    }
}
