using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.Entidades
{
    public class DeclaracaoSemNota
    {
        public int NotaId { get; set; }

        public int TipoDeclaracaoSemNotaId { get; set; }

        public string Matricula { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}
