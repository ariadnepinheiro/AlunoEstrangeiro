using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Enum;

namespace Techne.Lyceum.RN.CartaoEstudante.DTO
{
    [Serializable]
    public class SolicitacaoDTO
    {
        public string Aluno { get; set; }
        public DateTime? DataSolicitacao { get; set; }
        public DateTime? DataUltimoRetorno { get; set; }
        public string NomeOperadora { get; set; }
        public string Observacao { get; set; }
        public string SituacaoRetorno { get; set; }
        public string SituacaoSolicitacao { get; set; }
        public string Usuario { get; set; }
        public string TipoSolicitacao { get; set; }
    }
}
