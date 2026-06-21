namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;

    public class LyUnidadeEnsinoSituacao : IEntity
    {
        public string AtoOficial { get; set; }

        public DateTime? DtAlteracao { get; set; }

        public DateTime? DtCadastro { get; set; }

        public DateTime? DtDou { get; set; }

        public DateTime DtSituacao { get; set; }

        public string Matricula { get; set; }

        public string NumeroAtoOficial { get; set; }

        public string Observacao { get; set; }

        public decimal Ordem { get; set; }

        public string Situacao { get; set; }

        public string UnidadeEns { get; set; }
    }
}