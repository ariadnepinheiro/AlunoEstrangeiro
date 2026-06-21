using System;

namespace Techne.Lyceum.RN.DTOs
{
    [Serializable]
    public class DadosEnturmacaoAluno
    {
        public string Aluno { get; set; }

        public int Ano { get; set; }

        public int Periodo { get; set; }

        public string Turma { get; set; }

        public string GradeId { get; set; } 

        public string Curso { get; set; }

        public string TipoCurso { get; set; } 

        public string TipoEnsinoProfissionalizante { get; set; }

        public int Serie { get; set; }

        public string Turno { get; set; }

        public string Censo { get; set; }

        public string Curriculo { get; set; }

        public DateTime DataFimTurma { get; set; }

        public int IdControleVaga { get; set; }

        public string MunicipioEscola { get; set; }

        public DateTime DataMatricula { get; set; }

        public string Tipo { get; set; } 
    }
}
