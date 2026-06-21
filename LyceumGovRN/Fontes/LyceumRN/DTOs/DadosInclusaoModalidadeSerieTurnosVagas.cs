using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosInclusaoModalidadeSerieTurnosVagas
    {
        public DadosInclusaoModalidadeSerieTurnosVagas()
        {
            ListaTurnos = new List<DadosTurnoInclusaoModalidadeSerie>();
        }

        public int Ano { get; set; }

        public int Periodo { get; set; }

        public string Censo { get; set; }

        public string Curso { get; set; }

        public int Serie { get; set; }
       
        public int IdAgendaConfTurnoVaga { get; set; }

        public string UsuarioResponsavel { get; set; }

        public int PropostaVagaNova { get; set; }

        public int PropostaVagaContinuidade { get; set; }

        public List<DadosTurnoInclusaoModalidadeSerie> ListaTurnos { get; set; }
    }
}
