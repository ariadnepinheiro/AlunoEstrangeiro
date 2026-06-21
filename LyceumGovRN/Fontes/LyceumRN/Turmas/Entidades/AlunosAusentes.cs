using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Turmas.Entidades
{
    [AtributoTabela("Turma.ALUNOSAUSENTES", Nome = "Turma.ALUNOSAUSENTES")]
    public class AlunosAusentes : IEntity
    {
        [AtributoCampo(Nome = "ALUNOSAUSENTESID")]
        public int AlunosAusentesId { get; set; }

        [AtributoCampo(Nome = "CENSO")]
        public string Censo { get; set; }

        [AtributoCampo(Nome = "ANO")]
        public int Ano { get; set; }

        [AtributoCampo(Nome = "PERIODO")]
        public int Periodo { get; set; }

        [AtributoCampo(Nome = "TURMA")]
        public string Turma { get; set; }

        [AtributoCampo(Nome = "DATALANCAMENTO")]
        public DateTime DataLancamento { get; set; }

        [AtributoCampo(Nome = "QUANTIDADEMATRICULADOS")]
        public int QuantidadeMatriculados { get; set; }

        [AtributoCampo(Nome = "QUANTIDADEPRESENTES")]
        public int? QuantidadePresentes { get; set; }

        [AtributoCampo(Nome = "QUANTIDADEAMPARADOS")]
        public int? QuantidadeAmparados { get; set; }

        [AtributoCampo(Nome = "QUANTIDADEAFASTAMENTOSCOVID")]
        public int? QuantidadeAfastamentosCovid { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioID { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }

        public string Turno { get; set; }

        public int IdRegional { get; set; }
    }
}
