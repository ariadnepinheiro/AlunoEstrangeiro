using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.PRODUTOSERVICOVALORMAXIMO", Nome = "PrestacaoContas.PRODUTOSERVICOVALORMAXIMO")]
    public class ProdutoServicoValorMaximo : IEntity
    {
        [AtributoCampo(Nome = "PRODUTOSERVICOVALORMAXIMOID")]
        public int ProdutoServicoValorMaximoId { get; set; }

        [AtributoCampo(Nome = "PRODUTOSERVICOID")]
        public int ProdutoServicoId { get; set; }

        [AtributoCampo(Nome = "REGIAOFGVID")]
        public int RegiaoFgvId { get; set; }

        [AtributoCampo(Nome = "VALORMAXIMO")]
        public decimal ValorMaximo { get; set; }

        [AtributoCampo(Nome = "DATAINICIO")]
        public DateTime DataInicio { get; set; }

        [AtributoCampo(Nome = "DATAFIM")]
        public DateTime? DataFim { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
