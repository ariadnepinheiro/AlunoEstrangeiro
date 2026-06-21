using System;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.FORNECEDOR__PRODUTOSERVICOGRUPO", Nome = "PrestacaoContas.FORNECEDOR__PRODUTOSERVICOGRUPO")]
    public class FornecedorProdutoServicoGrupo : IEntity
    {
        [AtributoCampo(Nome = "FORNECEDOR__PRODUTOSERVICOGRUPOID")]
        public int? FornecedorProdutoServicoGrupoId { get; set; }

        [AtributoCampo(Nome = "FORNECEDORID")]
        public int FornecedorId { get; set; }

        [AtributoCampo(Nome = "PRODUTOSERVICOGRUPOID")]
        public int ProdutoServicoGrupoId { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}