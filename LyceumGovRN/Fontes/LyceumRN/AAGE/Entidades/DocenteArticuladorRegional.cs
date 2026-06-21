using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.AAGE.Entidades
{
    public class DocenteArticuladorRegional
    {
        public int DocenteArticuladorRegionalId { get; set; }

        public decimal DocenteId { get; set; }

        public int RegionalId { get; set; }

        public DateTime DataInicioVinculo { get; set; }

        public DateTime DataFimVinculo { get; set; }

        public string UsuarioId { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }
    }
}
