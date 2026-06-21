using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.CartaoEstudante.DTO
{
    [Serializable]
    public class AcompanhamentoRemessaDTO
    {
        public string NomeOperadora { get; set; }
        public int RemessaId { get; set; }
        public string Aluno { get; set; }
        public string Nome_Compl { get; set; }
        public string Unidade_Ens { get; set; }
        public string SituacaoProcessamento { get; set; }
        public string CodSolicitacao { get; set; }
        public DateTime? DataInclusaoRemessa { get; set; }
        public DateTime? DataEnvio { get; set; }
        public DateTime? DataInclusaoRetorno { get; set; }
    }
}
