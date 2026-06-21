using System;

namespace Techne.Lyceum.RN.DTOs
{
    [Serializable]
    public class DadosFaltasAulas
    {
        public int FaltasFinal { get; set; }

        public decimal AulasDadas { get; set; }

        public decimal AulasPrevistas { get; set; }
    }
}
