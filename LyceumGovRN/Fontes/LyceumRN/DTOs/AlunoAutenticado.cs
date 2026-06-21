namespace Techne.Lyceum.RN.DTOs
{
    using System;

    [Serializable]
    public class AlunoAutenticado
    {
        public DateTime? DataNascimento { get; set; }

        public string Matricula { get; set; }

        public string Nome { get; set; }

        public decimal Pessoa { get; set; }

        public string TelefoneResponsavel { get; set; }
    }
}