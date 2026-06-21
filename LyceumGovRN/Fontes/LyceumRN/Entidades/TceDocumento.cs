namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;

    public class TceDocumento : IEntity
    {
        public int Ano { get; set; }

        public string Descricao { get; set; }

        public DateTime DtAlteracao { get; set; }

        public DateTime DtCadastro { get; set; }

        public int IdDocumento { get; set; }

        public string Matricula { get; set; }

        public bool Obrigatorio { get; set; }

        public int Periodo { get; set; }

        public int Prazo { get; set; }
    }
}