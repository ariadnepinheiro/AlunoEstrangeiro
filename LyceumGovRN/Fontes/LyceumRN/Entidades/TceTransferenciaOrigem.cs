namespace Techne.Lyceum.RN.Entidades
{
    using Seeduc.Infra.Entities;

    public class TceTransferenciaOrigem : IEntity
    {
        public int? Ano { get; set; }

        public string Censo { get; set; }

        public string Curriculo { get; set; }

        public string Curso { get; set; }

        public int IdTransferencia { get; set; }

        public int IdTransferenciaOrigem { get; set; }

        public int? Periodo { get; set; }

        public int Serie { get; set; }

        public string Turma { get; set; }

        public string Turno { get; set; }

        public string UnidadeFisica { get; set; }
    }
}