namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;

    public class TceCursoFormacaoPessoal : IEntity
    {
        public int IdCursoFormacaoPessoal { get; set; }

        public int IdAreaFormacaoPessoal { get; set; }

        public string Curso { get; set; }

        public string Grau { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime DtAlteracao { get; set; }
    }
}