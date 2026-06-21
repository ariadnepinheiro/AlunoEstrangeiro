using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.DTOs
{
    [Serializable]
    public class DisciplinaTurmaTransferenciaAluno : IEntity
    {
        public string Disciplina { get; set; }
        public decimal AulasDadas { get; set; }
        public string Tipo { get; set; }
        public char TemNota { get; set; }
        public string Aluno { get; set; }
    }
}
