using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Logging;
using System.ComponentModel.DataAnnotations;

namespace SRV.Models.Domain
{
    public class MotivoInelegibilidade
    {
        public enum TipoMotivo
        {
            LancamentoNotaDocente = 15,
            DenunciaAvaliacaoExterna = 16,
            AplicacaoProvaAvaliacaoExterna = 17
        }

        [PrimaryKey]
        public int IdMotivoInelegibilidade { get; set; }

        public string DesMotivoInelegibilidade { get; set; }
    }
}