using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosTrocaMatriculaDocente
    {
        public int NumFunc { get; set; }

        public int Pessoa { get; set; }

        public string Matricula { get; set; }

        public int? IdFuncional { get; set; }

        public int? Vinculo { get; set; }

        public string NomeCompl { get; set; }

        public DateTime DtNasc { get; set; }

        public string Cpf { get; set; }

        public string Sexo { get; set; }

        public decimal? RegimeTrabalho { get; set; }

        public string IdVinculoMatricula { get; set; }
    }
}
