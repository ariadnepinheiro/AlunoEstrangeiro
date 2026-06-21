namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;

    public class TceFeriadoNacionalEstadual : IEntity
    {
        public int IdFeriadoNacionalEstadual { get; set; }

        public Date Data { get; set; }

        public string Descricao { get; set; }

        public string TipoEvento { get; set; }

        public string Matricula { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }
    }
}