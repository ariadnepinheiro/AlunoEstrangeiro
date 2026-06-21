using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.CartaoEstudante.DTO
{
    public class RegistroDuplicidadeMatriculaDTO
    {
        public long NumeroRegistro { get; set; }
        public string IdBeneficiario { get; set; }
        public DateTime DtAtualizacao { get; set; }
        public List<MatriculaResponseDTO> Matriculas { get; set; }

        public RegistroDuplicidadeMatriculaDTO()
        {
            Matriculas = new List<MatriculaResponseDTO>();    
        }
    }
}
