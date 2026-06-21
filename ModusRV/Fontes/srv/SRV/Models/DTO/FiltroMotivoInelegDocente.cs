using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;

namespace SRV.Models.DTO
{
    public class FiltroMotivoInelegDocente
    {
        public Servidor Servidor { get; set; }
        public MotivoInelegibilidade.TipoMotivo TipoMotivo { get; set; }

        public IList<LancamentoNotaDocente> LancamentosNotasDocentes { get; set; }
        public IList<DenunciaAvaliacaoExterna> DenunciasAvaliacoesExternas { get; set; }
        public IList<AplicacaoProvaAvaliacaoExterna> AplicacoesProvasAvaliacoesExternas { get; set; }
    }
}