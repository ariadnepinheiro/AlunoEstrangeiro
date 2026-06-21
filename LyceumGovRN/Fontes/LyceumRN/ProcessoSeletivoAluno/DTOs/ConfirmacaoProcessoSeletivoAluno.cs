using System;

namespace Techne.Lyceum.RN.ProcessoSeletivoAluno.DTOs
{
    public class ConfirmacaoProcessoSeletivoAluno
    {
        public string NumeroEdital { get; set; }
        public string NumeroInscricao { get; set; }
        public string NomeCandidato { get; set; }
        public string DataNascimento { get; set; }
        public string NomeMae { get; set; }
        public string NecessidadeEspecial { get; set; }
        public string RecursosNecessarioProva { get; set; }
        public string UnidadeEnsino { get; set; }
        public string Curso { get; set; }
        public string DataAlteracao { get; set; }
        public string IP { get; set; }
    }
}
