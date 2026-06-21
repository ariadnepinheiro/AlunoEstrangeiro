using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.CartaoEstudante.DTO.Filter
{
    [Serializable]
    public class MatriculaDuplicadaFilterDTO
    {
        public string Aluno { get; set; }
        public string UnidadeEnsino { get; set; }
        public string Regional { get; set; }

        public MatriculaDuplicadaFilterDTO() { }

        public MatriculaDuplicadaFilterDTO(string aluno, string unidadeEnsino, string regional)
        {
            this.Aluno = aluno;
            this.UnidadeEnsino = unidadeEnsino;
            this.Regional = regional;
        }
    }
}

