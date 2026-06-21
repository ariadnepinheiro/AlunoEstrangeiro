using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.IMPORTACAOFGV", Nome = "PrestacaoContas.IMPORTACAOFGV")]
    public class ImportacaoFgv : IEntity
    {
        [AtributoCampo(Nome = "IMPORTACAOFGVID")]
        public int ImportacaoFgvId { get; set; }

        [AtributoCampo(Nome = "REGIAOFGVID")]
        public int RegiaoFgvId { get; set; }

        [AtributoCampo(Nome = "DATAREFERENCIA")]
        public DateTime DataReferencia { get; set; }

        [AtributoCampo(Nome = "TOTALITENSTABELA")]
        public int TotalItensTabela { get; set; }

        [AtributoCampo(Nome = "TOTALITENSIMPORTADOS")]
        public int TotalItnesImportados { get; set; }

        [AtributoCampo(Nome = "USUARIOIMPORTACAO")]
        public string UsuarioImportacao { get; set; }

        [AtributoCampo(Nome = "DATAIMPORTACAO")]
        public DateTime DataImportacao { get; set; }
    }
}