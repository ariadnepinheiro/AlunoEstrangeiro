using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.CartaoEstudante.DTO
{
    [Serializable]
    public class RetornoDTO
    {
        public int RetornoId { get; set; }
        public long RemessaId { get; set; }
        public int OperadoraId { get; set; }
        public long IdBeneficiario { get; set; }
        public DateTime? DataProcessamento { get; set; }
        public string SituacaoProcessamento { get; set; }
        public DateTime DataInclusao { get; set; }
        public List<RetornoCriticaDTO> Criticas { get; set; }
    }
}
