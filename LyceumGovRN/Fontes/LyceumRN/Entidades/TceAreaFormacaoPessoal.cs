namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;

    public class TceAreaFormacaoPessoal : IEntity
    {
        public int IdAreaFormacaoPessoal { get; set; }

        public string Area { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime DtAlteracao { get; set; }
    }
}