namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;
    using Seeduc.Infra.MapeamentoAtributos;

    public class TceConfirmacaoMatricula : IEntity
    {
        public string Aluno { get; set; }

        public decimal Ano { get; set; }

        public string Censo { get; set; }

        public string Curriculo { get; set; }

        public string Curso { get; set; }

        public DateTime DtAlteracao { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime DtSugerida { get; set; }

        public bool EnsinoReligioso { get; set; }

        public int IdConfirmacaoMatricula { get; set; }

        public bool LinguaEstrangeiraFacultativa { get; set; }

        public string Matricula { get; set; }

        public decimal Periodo { get; set; }

        public bool ProjetoAutonomia { get; set; }

        public decimal Serie { get; set; }

        public string Status { get; set; }

        [AtributoCampo(Nome = "MATRICULAFACIL")]
        public bool MatriculaFacil { get; set; }

        public string Turno { get; set; }

        [AtributoCampo(Nome = "TIPOVAGAOCUPADA")]
        public string TipoVagaOcupada { get; set; }

        public string Observacao { get; set; }
    }
}