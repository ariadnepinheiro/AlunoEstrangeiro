using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN
{
    public class EPrestacaoContas
    {
        public decimal? ID { get; set; }
        public decimal IDPrestador { get; set; }
        public decimal IDPeriodoRef { get; set; }
        public String NumeroProcesso { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }

        public String Data { get; set; }
        public String SiglaSituacao { get; set; }
        public String TipoSituacao { get; set; }
        public DateTime? InicioAplicacao { get; set; }
        public DateTime? FimAplicacao { get; set; }
        /// <summary>
        /// Coordenadoria ou Unidade de Ensino
        /// </summary>
        public String TipoPrestador { get; set; }
        public String NomePrestador { get; set; }
    }
}
