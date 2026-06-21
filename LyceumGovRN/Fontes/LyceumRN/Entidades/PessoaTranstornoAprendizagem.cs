using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    [AtributoTabela("dbo.PESSOATRANSTORNOAPRENDIZAGEM", Nome = "dbo.PESSOATRANSTORNOAPRENDIZAGEM")]
    public class PessoaTranstornoAprendizagem : IEntity
    {
        [AtributoCampo(Nome = "PESSOATRANSTORNOAPRENDIZAGEMID")]
        public int PessoaTranstornoAprendizagemId { get; set; }

        [AtributoCampo(Nome = "PESSOA")]
        public decimal Pessoa { get; set; }

        [AtributoCampo(Nome = "TRANSTORNOAPRENDIZAGEMID")]
        public int TranstornoAprendizagemId { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
