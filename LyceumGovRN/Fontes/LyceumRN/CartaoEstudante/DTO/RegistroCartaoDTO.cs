using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.CartaoEstudante.DTO
{
    public class RegistroCartaoDTO
    {
        public long NumeroRegistro { get; set; }
        public string IdBeneficiario { get; set; }
        public DateTime? DtAtualizacao { get; set; }
        public MatriculaResponseDTO Matricula { get; set; }
        public CartaoResponseDTO Cartao { get; set; }
    }
}
