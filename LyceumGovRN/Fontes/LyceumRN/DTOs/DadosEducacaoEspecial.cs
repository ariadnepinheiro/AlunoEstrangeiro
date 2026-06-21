using System.Collections.Generic;

namespace Techne.Lyceum.RN.DTOs
{
   public class DadosEducacaoEspecial
    {
        public string Aluno { get; set; }

        public bool ExibirEducacaoEspecial { get; set; }

        public bool ExibirEnturmacao { get; set; }

        public bool Enturmado { get; set; }

        public bool Aceite { get; set; }

        public int Ano { get; set; }

        public string Censo { get; set; }

        public string NomeUnidadeEnsino { get; set; }

        public int Periodo { get; set; }

        public string Curso { get; set; }

        public string NomeCurso { get; set; }

        public int Serie { get; set; }

        public string Turno { get; set; }

        public string NomeTurno { get; set; }

        public string Turma { get; set; }

        public List<Atendimento> Atendimentos { get; set; }
    }
}
