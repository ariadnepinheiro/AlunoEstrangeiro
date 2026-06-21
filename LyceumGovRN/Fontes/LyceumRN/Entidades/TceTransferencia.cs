namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;

    public class TceTransferencia : IEntity
    {
        public string Aluno { get; set; }

        public DateTime DtAlteracao { get; set; }

        public DateTime DtCadastro { get; set; }

        public int IdTransferencia { get; set; }

        public string Justificativa { get; set; }

        public string MatriculaAndamento { get; set; }

        public string MatriculaSolicitante { get; set; }

        public string Motivo { get; set; }

        public string Observacao { get; set; }

        public string Status { get; set; }
    }
}