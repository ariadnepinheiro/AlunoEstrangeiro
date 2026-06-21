using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosLiberacaoConfirmacao
    {
        public string Aluno { get; set; }

        public int IdConfirmacaoMatricula { get; set; }

        public int Ano { get; set; }

        public int Periodo { get; set; }

        public string SituacaoAtual { get; set; }

        public string MatriculaResponsavel { get; set; }
    }
}
