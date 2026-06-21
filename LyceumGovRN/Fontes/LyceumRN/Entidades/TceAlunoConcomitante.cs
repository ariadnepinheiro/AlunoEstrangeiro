using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceAlunoConcomitante : IEntity
    {
        public int IdAlunoConcomitante { get; set; }

        public string Aluno { get; set; }

        public string Censo { get; set; }

        public int Ano { get; set; }

        public int Periodo { get; set; }

        public string Status { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }
    }
}
