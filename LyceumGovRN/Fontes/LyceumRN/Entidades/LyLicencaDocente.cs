namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;

    public class LyLicencaDocente : IEntity
    {
        public Decimal NumFunc { get; set; }

        public DateTime Dtini { get; set; }

        public DateTime? Dtfim { get; set; }

        public string Motivo { get; set; }

        public DateTime? DtRetorno { get; set; }

        public DateTime? StampAtualizacao { get; set; }
    }
}
