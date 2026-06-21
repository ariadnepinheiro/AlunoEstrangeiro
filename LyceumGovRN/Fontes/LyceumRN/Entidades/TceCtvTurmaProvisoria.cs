using System;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceCtvTurmaProvisoria
    {
        public int IdTurmaProvisoria { get; set; }

        public int Ano { get; set; }

        public int Periodo { get; set; }

        public string Censo { get; set; }

        public string Turma { get; set; }

        public string Curso { get; set; }

        public int Serie { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }
    }
}
