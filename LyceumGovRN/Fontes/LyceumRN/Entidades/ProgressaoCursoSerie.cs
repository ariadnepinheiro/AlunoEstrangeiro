using System;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Entidades
{
    public class ProgressaoCursoSerie : IEntity
    {
        [AtributoCampo(Nome = "PROGRESSAOSERIEID")]
        public int IdProgressaoSerie { get; set; }

        public string CursoId { get; set; }

        public string NomeCurso { get; set; }

        public string Curso { get; set; }

        public string TipoCurso { get; set; }

        public string TipoCursoId { get; set; }

        public string Serie { get; set; }

        public string Modalidade { get; set; }

        public string CodModalidade { get; set; }

        public string ProxCursoId { get; set; }

        public string ProxNomeCurso { get; set; }

        public string ProxCurso { get; set; }

        public string ProxTipoCurso { get; set; }

        public string ProxTipoCursoId { get; set; }

        public string ProxSerie { get; set; }

        public string ProxModalidade { get; set; }

        public string ProxCodModalidade { get; set; }

        [AtributoCampo(Nome = "PARTICIPAFASE1")]
        public bool? ParticipaFase1 { get; set; }

        [AtributoCampo(Nome = "PARTICIPAFASE2")]
        public bool? ParticipaFase2 { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public string DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public string DataAlteracao { get; set; }

        [AtributoCampo(Nome = "MATRICULA")]
        public string Matricula { get; set; }

        [AtributoCampo(Nome = "NOME")]
        public string Nome { get; set; }
    }
}
