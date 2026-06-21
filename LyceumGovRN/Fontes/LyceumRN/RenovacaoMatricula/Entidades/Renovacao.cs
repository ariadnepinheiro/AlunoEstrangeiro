using System;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.RenovacaoMatricula.Entidades
{
    public enum SituacaoRenovacao
    {
        [Techne.Lyceum.RN.RNBase.StringValue("Renovação de Matrícula Ativa")]
        Ativo = 1,

        [Techne.Lyceum.RN.RNBase.StringValue("Renovação de Matrícula Cancelada")]
        Cancelado = 2,

        [Techne.Lyceum.RN.RNBase.StringValue("Renovação de Matrícula Confirmada")]
        PossuiConfirmacao = 3,
    }

    public class Renovacao
    {
        public int RenovacaoId { get; set; }

        public string AlunoId { get; set; }

        public string UnidadeEnsinoId { get; set; }

        public string CursoId { get; set; }

        public string TurnoId { get; set; }

        public int Ano { get; set; }

        public int Periodo { get; set; }

        public int Serie { get; set; }

        public bool EnsinoReligioso { get; set; }

        public bool LinguaEstrangeira { get; set; }

        public int SituacaoRenovacaoId { get; set; }

        public string Usuario { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }

        public string TipoVaga { get; set; }
    }
}
