using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.Entidades
{
    public class LyCandidatoDocContrato
    {
        public string Concurso { get; set; }

        public string Candidato { get; set; }

        public string Status { get; set; }

        public DateTime DtInicioContrato { get; set; }

        public DateTime? DtFimContrato { get; set; }      
    }
}
