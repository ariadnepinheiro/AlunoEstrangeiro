using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Enum;

namespace Techne.Lyceum.RN.CartaoEstudante.DTO.Filter
{
    [Serializable]
    public class AcompanhamentoRemessaFilterDTO
    {
        public string Aluno { get; set; }
        public string UnidadeEnsino { get; set; }
        public string Municipio { get; set; }
        public int IdRegional { get; set; }
        public DateTime? DataEnvioInicio { get; set; }
        public DateTime? DataEnvioFim { get; set; }
        public TipoSituacaoProcessamentoEnum TipoSituacaoProcessamento { get; set; }
    }
}