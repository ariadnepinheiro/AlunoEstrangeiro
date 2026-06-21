using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosFrequenciaDiaria
    {
        public DadosFrequenciaDiaria()
        {
            Aulas = new List<int>();
            CurriculosItemId = new List<int>();
            AlunosComFalta = new List<DadosAlunoFalta>();
        }

        public int Ano { get; set; }

        public int Semestre { get; set; }

        public string Turma { get; set; }

        public string Disciplina { get; set; }      

        public string Faculdade { get; set; }

        public string Turno { get; set; }

        public DateTime DataFrequencia { get; set; }

        public DateTime? DataReposicao { get; set; }

        public int DiaSemana { get; set; }

        public int Bimestre { get; set; }

        public List<int> Aulas { get; set; }

        public List<int> CurriculosItemId { get; set; }

        public string PlanoAula { get; set; }        

        public string UsuarioResponsavel { get; set; }

        public List<string> AlunosSuspensos { get; set; }

        public List<DadosAlunoFalta> AlunosComFalta { get; set; }

        public class DadosAlunoFalta
        {
            public string Aluno { get; set; }

            public int Aula { get; set; }
        }
    }
}
