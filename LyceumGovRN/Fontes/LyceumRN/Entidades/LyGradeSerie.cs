using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class LyGradeSerie : IEntity
    {
        public decimal GradeId { get; set; }

        public string Curso { get; set; }

        public string Turno { get; set; }

        public string Curriculo { get; set; }

        public decimal Serie { get; set; }

        public decimal Ano { get; set; }

        public decimal Semestre { get; set; }

        public string Grade { get; set; }

        public decimal? Capacidade { get; set; }

        public decimal? NumFunc { get; set; }

        public string Faculdade { get; set; }

        public string Dependencia { get; set; }

        public DateTime? DtInicio { get; set; }

        public DateTime? DtFim { get; set; }

        public string FormulaMf1 { get; set; }

        public string FormulaMf3 { get; set; }

        public string FormulaMf2 { get; set; }

        public string FormulaCa1 { get; set; }

        public string FormulaCa2 { get; set; }

        public string FormulaCa3 { get; set; }

        public string ConceitoMin1 { get; set; }

        public string ConceitoMin2 { get; set; }

        public string ConceitoMin3 { get; set; }

        public string ConceitoMinEx { get; set; }

        public string ConceitoMinEx2 { get; set; }

        public decimal? UltNumChamada { get; set; }

        public string UnidadeResponsavel { get; set; }

        public DateTime? DataNumeracao { get; set; }

        public string FlField01 { get; set; }

        public string ObsFormulaMf1 { get; set; }

        public string ObsFormulaMf2 { get; set; }

        public string ObsFormulaMf3 { get; set; }

        public string FechamentoManual { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime DtAlteracao { get; set; }
    }
}
