namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;
    using Seeduc.Infra.MapeamentoAtributos;

    public class LyNota : IEntity
    {
        public string Aluno { get; set; }
        public decimal? Ano { get; set; }
        public string Compareceu { get; set; }
        public string Conceito { get; set; }
        public DateTime Data { get; set; }
        public string Disciplina { get; set; }
        public decimal? Formulario { get; set; }
        public string Justificativa { get; set; }
        public decimal? Ordem { get; set; }
        public string Prova { get; set; }
        public string RecuperacaoParalela { get; set; }
        public string SemAvaliacao { get; set; }
        public decimal? Semestre { get; set; }
        public string Turma { get; set; }
        [AtributoCampo(Nome = "NOTAID")]
        public int NotaId { get; set; }
        [AtributoCampo(Nome = "NOTAPROVA")]
        public decimal? NotaProva { get; set; }
        [AtributoCampo(Nome = "NOTARECUPERACAO")]
        public decimal? NotaRecuperacao { get; set; }
        [AtributoCampo(Nome = "MOTIVOSEMNOTAID")]
        public int? MotivoSemNotaId { get; set; }
    }
}