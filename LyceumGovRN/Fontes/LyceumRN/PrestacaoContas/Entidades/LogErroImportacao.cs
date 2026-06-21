using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.LOGERROIMPORTACAO", Nome = "PrestacaoContas.LOGERROIMPORTACAO")]
    public class LogErroImportacao : IEntity
    {
        [AtributoCampo(Nome = "LOGERROIMPORTACAOID")]
        public int LogErroImportacaoId { get; set; }

        [AtributoCampo(Nome = "IMPORTACAOFGVID")]
        public int ImportacaoFgvId { get; set; }
        
        [AtributoCampo(Nome = "LINHA")]
        public int Linha { get; set; }

        [AtributoCampo(Nome = "DESCRICAOERRO")]
        public string DescricaoErro { get; set; }

        [AtributoCampo(Nome = "USUARIOIMPORTACAO")]
        public string UsuarioImportacao { get; set; }

        [AtributoCampo(Nome = "DATAIMPORTACAO")]
        public DateTime DataImportacao { get; set; }
    }
}
