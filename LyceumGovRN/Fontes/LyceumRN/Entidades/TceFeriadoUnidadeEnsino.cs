namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;

    public class TceFeriadoUnidadeEnsino : IEntity
    {
        public int IdFeriadoUnidadeEnsino { get; set; }

        public Date Data { get; set; }

        public string Descricao { get; set; }

        public string TipoEvento { get; set; }

        public string UnidadeEnsino { get; set; }

        public string Matricula { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }
    }
}