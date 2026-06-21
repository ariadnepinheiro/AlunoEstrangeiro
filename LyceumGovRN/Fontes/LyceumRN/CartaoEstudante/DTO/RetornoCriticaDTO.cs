using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.CartaoEstudante.DTO
{
    [Serializable]
    public class RetornoCriticaDTO
    {
        public int RetornoCriticaId { get; set; }
        public int RetornoId { get; set; }
        public DateTime DataInclusao { get; set; }
        public string Observacao { get; set; }
        public string CodigoCritica { get; set; }
        public string Descricao { get; set; }        
    }
}
