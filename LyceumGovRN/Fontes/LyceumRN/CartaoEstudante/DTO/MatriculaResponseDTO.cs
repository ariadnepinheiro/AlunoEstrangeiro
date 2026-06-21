using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.CartaoEstudante.DTO
{
    public class MatriculaResponseDTO
    {
        public string Matricula { get; set; }
        public string FlagMatriculaPrincipal { get; set; }
        public DateTime DtFlagMatriculaPrincipal { get; set; }
    }
}
