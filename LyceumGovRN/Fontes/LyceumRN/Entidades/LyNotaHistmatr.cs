using Seeduc.Infra.MapeamentoAtributos;
using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class LyNotaHistmatr : IEntity
    {
        public string Aluno { get; set; }

        public decimal? Ordem { get; set; }

        public decimal? Ano { get; set; }

        public decimal? Semestre { get; set; }

        public string Disciplina { get; set; }

        public string NotaId { get; set; }

        public string Conceito { get; set; }

        public DateTime Data { get; set; }

        public string Observacao { get; set; }

        public string Compareceu { get; set; }

        [AtributoCampo(Nome = "RECUPERACAOPARALELA")]
        public string RecuperacaoParalela { get; set; }

        [AtributoCampo(Nome = "SEMAVALIACAO")]
        public string SemAvaliacao { get; set; }

        public string Turma { get; set; }

        [AtributoCampo(Nome = "NOTAPROVA")]
        public decimal? NotaProva { get; set; }

        [AtributoCampo(Nome = "NOTARECUPERACAO")]
        public decimal? NotaRecuperacao { get; set; }

        [AtributoCampo(Nome = "MOTIVOSEMNOTAID")]
        public int? MotivoSemNotaId { get; set; }
    }
}
