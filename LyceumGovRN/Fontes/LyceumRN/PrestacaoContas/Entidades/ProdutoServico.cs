using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.PRODUTOSERVICO", Nome = "PrestacaoContas.PRODUTOSERVICO")]
    public class ProdutoServico : IEntity
    {
        [AtributoCampo(Nome = "PRODUTOSERVICOID")]
        public int ProdutoServicoId { get; set; }

        [AtributoCampo(Nome = "TIPOPRODUTOSERVICOID")]
        public int TipoProdutoServicoId { get; set; }

        [AtributoCampo(Nome = "FINALIDADEID")]
        public int FinalidadeId { get; set; }

        [AtributoCampo(Nome = "PRODUTOSERVICOGRUPOID")]
        public int ProdutoServicoGrupoId { get; set; }

        [AtributoCampo(Nome = "UNIDADEMEDIDAID")]
        public int UnidadeMedidaId { get; set; }

        [AtributoCampo(Nome = "NOME")]
        public string Nome { get; set; }

        [AtributoCampo(Nome = "DETALHE")]
        public string Detalhe { get; set; }

        [AtributoCampo(Nome = "INVENTARIAVEL")]
        public bool Inventariavel { get; set; }

        [AtributoCampo(Nome = "PEQUENASDESPESAS")]
        public bool PequenasDespesas { get; set; }

        [AtributoCampo(Nome = "NECESSITAORCAMENTO")]
        public bool NecessitaOrcamento { get; set; }

        [AtributoCampo(Nome = "NCM")]
        public string Ncm { get; set; }

        [AtributoCampo(Nome = "FLAGNCM")]
        public bool FlagNcm { get; set; }

        [AtributoCampo(Nome = "CODIGOFGV")]
        public int? CodigoFgv { get; set; }

        [AtributoCampo(Nome = "ATIVO")]
        public bool Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
