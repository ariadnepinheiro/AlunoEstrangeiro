namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;

    public class TceFeriadoMunicipal : IEntity
    {
        public int IdFeriadoMunicipal { get; set; }

        public Date Data { get; set; }

        public string Descricao { get; set; }

        public string TipoEvento { get; set; }

        public string CodMunicipio { get; set; }

        public string Matricula { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }
    }
}