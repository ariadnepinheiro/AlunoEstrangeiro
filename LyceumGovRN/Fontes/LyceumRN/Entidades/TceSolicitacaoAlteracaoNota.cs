namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;

    public class TceSolicitacaoAlteracaoNota : IEntity
    {
        public int Ano { get; set; }

        public string Disciplina { get; set; }

        public DateTime? DtLimite { get; set; }

        public DateTime DtSolicitacao { get; set; }

        public DateTime DtStatus { get; set; }

        public int IdSolicitacaoAlteracaoNota { get; protected set; }

        public string Justificativa { get; set; }

        public string MatriculaAprovador { get; set; }

        public int NumFunc { get; set; }

        public int Semestre { get; set; }

        public string Status { get; set; }

        public int Subperiodo { get; set; }

        public string Turma { get; set; }

        public string UnidadeEns { get; set; }
    }
}