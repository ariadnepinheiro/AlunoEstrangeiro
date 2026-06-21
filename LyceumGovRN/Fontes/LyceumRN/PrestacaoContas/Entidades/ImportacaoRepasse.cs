using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.IMPORTACAOREPASSE", Nome = "PrestacaoContas.IMPORTACAOREPASSE")]
    public class ImportacaoRepasse : IEntity
    {
        [AtributoCampo(Nome = "IMPORTACAOREPASSEID")]
        public int ImportacaoRepasseId { get; set; }

        [AtributoCampo(Nome = "ITEMPLANILHAORCAMENTARIAID")]
        public int ItemPlanilhaOrcamentariaId { get; set; }

        [AtributoCampo(Nome = "VALORTOTALIMPORTADO")]
        public int ValorTotalImportado { get; set; }

        [AtributoCampo(Nome = "TOTALITENSIMPORTADOS")]
        public int TotalItensImportados { get; set; }

        [AtributoCampo(Nome = "USUARIOIMPORTACAO")]
        public string UsuarioImportacao { get; set; }

        [AtributoCampo(Nome = "DATAIMPORTACAO")]
        public DateTime DataImportacao { get; set; }
    }
}