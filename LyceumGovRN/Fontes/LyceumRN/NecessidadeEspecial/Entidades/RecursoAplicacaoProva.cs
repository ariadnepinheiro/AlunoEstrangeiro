using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.NecessidadeEspecial.Entidades
{
    public class RecursoAplicacaoProva : IEntity
    {
        public int RecursoAplicacaoProvaId { get; set; }

        public string Nome { get; set; }

        public bool Exclusivo { get; set; }

        public bool Ativo { get; set; }
    }
}
