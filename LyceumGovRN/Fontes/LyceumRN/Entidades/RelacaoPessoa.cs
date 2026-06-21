using System;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Entidades
{
    public class RelacaoPessoa : IEntity
    {
        public decimal PessoaId { get; set; }

        public decimal ParenteId { get; set; }

        public int ParentescoId { get; set; }
    }
}
