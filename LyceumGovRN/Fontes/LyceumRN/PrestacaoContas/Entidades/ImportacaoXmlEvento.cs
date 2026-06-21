using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.IMPORTACAOXMLEVENTO", Nome = "PrestacaoContas.IMPORTACAOXMLEVENTO")]
    public class ImportacaoXmlEvento : IEntity
    {
        [AtributoCampo(Nome = "IMPORTACAOXMLEVENTOID")]
        public int ImportacaoXmlEventoId { get; set; }

        [AtributoCampo(Nome = "EVENTOID")]
        public int EventoId { get; set; }

        [AtributoCampo(Nome = "PRODUTOSERVICOID")]
        public int? ProdutoServicoId { get; set; }

        [AtributoCampo(Nome = "NUMEROITEM")]
        public string NumeroItem { get; set; }

        [AtributoCampo(Nome = "DESCRICAO")]
        public string Descricao { get; set; }

        [AtributoCampo(Nome = "NCM")]
        public string Ncm { get; set; }

        [AtributoCampo(Nome = "QUANTIDADE")]
        public int Quantidade { get; set; }

        [AtributoCampo(Nome = "VALORUNITARIO")]
        public decimal ValorUnitario { get; set; }

        [AtributoCampo(Nome = "DATAIMPORTACAO")]
        public DateTime DataImportacao { get; set; }

        [AtributoCampo(Nome = "DATAANALISE")]
        public DateTime? DataAnalise { get; set; }

        [AtributoCampo(Nome = "USUARIOANALISADOR")]
        public string UsuarioAnalisador { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}

