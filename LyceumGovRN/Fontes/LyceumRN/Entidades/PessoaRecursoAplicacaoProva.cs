using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class PessoaRecursoAplicacaoProva : IEntity
    {
        public int PessoaId { get; set; }
        public int RecursoAplicacaoProvaId { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public string Usuario { get; set; }
    }
}
