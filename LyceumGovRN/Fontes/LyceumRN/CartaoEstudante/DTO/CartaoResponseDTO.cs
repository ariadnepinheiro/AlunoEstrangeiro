using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.CartaoEstudante.DTO
{
    public class CartaoResponseDTO
    {
        public string NrRiocard { get; set; }
        public string StatusCartao { get; set; }
        public string NrChip { get; set; }
        public DateTime? DtImpr { get; set; }
        public string LocalImpressao { get; set; }
        public long? NrLoteEntrega { get; set; }
        public DateTime? DtEntregaLote { get; set; }
        public int CodCancel { get; set; }
        public DateTime? DataConfirmacaoAluno { get; set; }
    }
}
