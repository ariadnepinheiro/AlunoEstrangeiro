using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosFinalizacaoOferta
    {
        public bool Finalizado { get; set; }

        public string UsuarioId { get; set; }

        public string UsuarioNome { get; set; }

        public DateTime Data { get; set; }

        public int MatriculadosManha { get; set; }

        public int MatriculadosTarde { get; set; }

        public int MatriculadosNoite { get; set; }

        public int MatriculadosIntegral { get; set; }
    }
}
