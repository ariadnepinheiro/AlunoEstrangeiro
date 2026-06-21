using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.DTOs
{
    [Serializable]
    public class NotaTurmaAtualTransferenciaAluno : IEntity
    {
        public string Disciplina { get; set; }
        public string Prova { get; set; }
        public string Conceito { get; set; }
        public decimal? Ordem { get; set; }
        public decimal? Formulario { get; set; }
        public string Compareceu { get; set; }
        public DateTime Data { get; set; }
        public string RecuperacaoParalela { get; set; }
        public string SemAvaliacao { get; set; }
        public string Justificativa { get; set; }
        public int NotaId { get; set; }
        public decimal? NotaProva { get; set; }
        public decimal? NotaRecuperacao { get; set; }
        public int? MotivoSemNotaId { get; set; }
        public bool AtualizaNota { get; set; }
    }
}
