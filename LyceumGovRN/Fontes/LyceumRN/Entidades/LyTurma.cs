using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class LyTurma : IEntity
    {
        public decimal Ano { get; set; }

        public decimal Semestre { get; set; }

        public string Faculdade { get; set; }

        public string Curso { get; set; }

        public string Curriculo { get; set; }

        public string Turno { get; set; }

        public decimal Serie { get; set; }

        public string Turma { get; set; }

        public string Disciplina { get; set; }
       
        public string Dependencia { get; set; }

        public decimal AulasPrevistas { get; set; }

        public decimal AulasDadas { get; set; }

        public decimal MinAulas { get; set; }

        public decimal NumAlunos { get; set; }

        public DateTime DtUltalt { get; set; }

        public DateTime DtInicio { get; set; }

        public DateTime DtFim { get; set; }

        public string SitTurma { get; set; }

        public string UnidadeResponsavel { get; set; }

        public string Especial { get; set; }

        public string UtilizaIndice { get; set; }

        public string NivelPresenca { get; set; }

        public string ExibeSomenteListaSel { get; set; }

        public string LancamentoHistorico { get; set; }

        public string EmElaboracao { get; set; }

        public DateTime DtCriacao { get; set; }

        public string PermiteChoqueHorario { get; set; }

        public string TipoGestao { get; set; }

        public string Classificacao { get; set; }
    }
}
