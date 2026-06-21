using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.IMPORTACAOSEI", Nome = "PrestacaoContas.IMPORTACAOSEI")]
    public class ImportacaoSei : IEntity
    {
        [AtributoCampo(Nome = "IMPORTACAOSEIID")]
        public int ImportacaoSeiId { get; set; }

        [AtributoCampo(Nome = "PLANOTRABALHOID")]
        public int PlanoTrabalhoId { get; set; }

        [AtributoCampo(Nome = "PERIODOREFERENCIAID")]
        public int PeriodoReferenciaId { get; set; }

        [AtributoCampo(Nome = "CENSO")]
        public string Censo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATAIMPORTACAO")]
        public DateTime DataImportacao { get; set; }
    }
}