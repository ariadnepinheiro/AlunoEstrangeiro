using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosMovimentacaoServidor
    {
        public int Pessoa { get; set; }

        public string Matricula { get; set; }

        public int Ordem { get; set; }

        public string SetorDestino { get; set; }

        public string UsuarioResponsavel { get; set; }

        public DateTime DataMovimentacao { get; set; }
    }
}
