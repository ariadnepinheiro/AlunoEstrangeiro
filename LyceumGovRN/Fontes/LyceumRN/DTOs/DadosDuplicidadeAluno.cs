using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosDuplicidadeAluno
    {
        public decimal PessoaCorreta { get; set; }

        public string MatriculaCorreta { get; set; }

        public string Nome { get; set; }

        public string NomeMae { get; set; }

        public DateTime DataNascimento { get; set; }

        public string SituacaoMatricula { get; set; }

        public int Ano { get; set; }

        public int Periodo { get; set; }

        public string Censo { get; set; }

        public string Curso { get; set; }

        public int Serie { get; set; }

        public string Turno { get; set; }

        public string TipoVaga { get; set; }

        public bool EnsinoReligioso { get; set; }

        public bool LinguaEstrangeiraFacultativa { get; set; }

        public string Curriculo { get; set; }
        
        public string UsuarioId { get; set; }

        public List<decimal> PessoasParaRemover { get; set; }

        public List<string> MatriculasParaCancelar { get; set; }
    }
}
