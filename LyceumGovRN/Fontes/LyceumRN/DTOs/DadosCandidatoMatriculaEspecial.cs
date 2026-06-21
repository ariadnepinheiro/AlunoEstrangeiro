using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosCandidatoMatriculaEspecial
    {
        public int MatriculaEspecialDisciplinaId { get; set; }

        public int MatriculaEspecialId { get; set; }

        public string Aluno { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string EmailGoogle { get; set; }

        public string Telefone { get; set; }

        public string Disciplina { get; set; }

        public string NomeDisciplina { get; set; }

        public string Turno { get; set; }

        public string NomeTurno { get; set; }

        public bool Convocado { get; set; }
    }
}
