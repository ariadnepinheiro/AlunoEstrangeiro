using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceSituacaoFinalAluno : IEntity
    {
        public int IdSituacaoFinalAluno { get; set; }

        public string Aluno { get; set; }

        public decimal Ano { get; set; }

        public decimal Periodo { get; set; }

        public string Turma { get; set; }

        public decimal FrequenciaGlobal { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }

        public string SituacaoFinal { get; set; }
    }
}
