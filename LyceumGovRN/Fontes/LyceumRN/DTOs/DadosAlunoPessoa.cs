using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosAlunoPessoa
    {
        public string Aluno { get; set; }

        public string Nome { get; set; }

        public DateTime? DataNascimento { get; set; }

        public string NomeMae { get; set; }

        public string UnidadeEnsino { get; set; }

        public string SitAluno { get; set; }

        public string Cpf { get; set; } 
    }
}
