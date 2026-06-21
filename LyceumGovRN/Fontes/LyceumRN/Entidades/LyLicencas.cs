namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;

    public class LyLicencas : IEntity
    {
        public string Motivo { get; set; }

        public string Descricao { get; set; }

        public string PossuiDtfim { get; set; }

        public Decimal? PeriodoLimite { get; set; }

        public string ParticipaContratoTemporario { get; set; }

        public string ValidaAlocacao { get; set; }
    }
}
