using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosConfirmacaoVagasOferta
    {
        public int Ano { get; set; }

        public int Periodo { get; set; }

        public string Censo { get; set; }

        public string ModalidadeCurso { get; set; }

        public string CursoReferencia { get; set; }

        public int SerieReferencia { get; set; }
       
        public int MatriculadosManha { get; set; }

        public int MatriculadosTarde { get; set; }

        public int MatriculadosNoite { get; set; }

        public int MatriculadosIntegral { get; set; }

        public int QuantidadeMatriculados { get; set; }

        public bool Finalizado { get; set; }

        public string UsuarioId { get; set; }

        public string UsuarioNome { get; set; }

        public DateTime Data { get; set; }

        public List<DadosConfirmacaoVagasOfertaCurso> Ofertas { get; set; }
    }
}
