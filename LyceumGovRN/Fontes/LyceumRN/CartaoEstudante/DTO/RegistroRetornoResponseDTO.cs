using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.CartaoEstudante.DTO
{
    public class RegistrosRetornoResponseDTO
    {
        public int IdRemessa { get; set; }
        public DateTime? DtProc { get; set; }
        public string StProc { get; set; }
        public int OperadoraId { get; set; }
        public int IdBeneficiario { get; set; }
        public List<CriticaDTO> Criticas { get; set; }

        public RegistrosRetornoResponseDTO()
        {
            Criticas = new List<CriticaDTO>();
        }
    }
}
