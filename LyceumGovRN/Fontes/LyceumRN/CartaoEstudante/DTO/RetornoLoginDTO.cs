using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Entity;

namespace Techne.Lyceum.RN.CartaoEstudante.DTO
{
    public class RetornoLoginDTO
    {
        public string IdBeneficiario { get; set; }
        public string Matricula { get; set; }
        public List<CriticaDTO> Criticas { get; set; }

        public RetornoLoginDTO()
        {
            Criticas = new List<CriticaDTO>();
        }
    }
}
