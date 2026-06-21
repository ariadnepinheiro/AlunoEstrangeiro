using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.TIPOPRODUTOSERVICO", Nome = "PrestacaoContas.TIPOPRODUTOSERVICO")]
    public class TipoProdutoServico : IEntity
    {
        [AtributoCampo(Nome = "TIPOPRODUTOSERVICOID")]
        public int TipoProdutoServicoId { get; set; }

        public string Descricao { get; set; }

        public bool Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
