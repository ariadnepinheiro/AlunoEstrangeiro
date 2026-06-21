using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.AAGE.Entidades
{
    public class DocenteAAGEUnidadeEnsino
    {
        public int DocenteAAGEUnidadeEnsinoId { get; set; }

        public decimal DocenteId { get; set; }

        public string UnidadeEnsinoId { get; set; }

        public DateTime DataInicioVinculo { get; set; }

        public DateTime DataFimVinculo { get; set; }

        public string UsuarioId { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }
    }
}
