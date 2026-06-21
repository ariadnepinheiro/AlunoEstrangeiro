using System;

namespace Techne.Lyceum.RN.Matriculas.Entidades
{
    public class RestricaoIdadeSerie
    {
        public int IdRestricaoIdadeSerie { get; set; }

        public string Curso { get; set; }

        public int Serie { get; set; }

        public int IdadeMinima { get; set; }

        public int IdadeMaxima { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime? DtAlteracao { get; set; }

        public string Matricula { get; set; }
    }
}