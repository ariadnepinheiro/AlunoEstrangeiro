using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class DocenteCursoCapacitacao : IEntity
    {
        public int PessoaId { get; set; }
        public int CursoCapacitacaoId { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public string Matricula { get; set; }
    }
}