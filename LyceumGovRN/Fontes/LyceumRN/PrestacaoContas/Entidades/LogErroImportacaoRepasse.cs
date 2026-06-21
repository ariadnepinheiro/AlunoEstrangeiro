using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.LOGERROIMPORTACAOREPASSE", Nome = "PrestacaoContas.LOGERROIMPORTACAOREPASSE")]
    public class LogErroImportacaoRepasse : IEntity
    {
        [AtributoCampo(Nome = "LOGERROIMPORTACAOREPASSEID")]
        public int LogErroImportacaoRepasseId { get; set; }

        [AtributoCampo(Nome = "IMPORTACAOREPASSEID")]
        public int ImportacaoRepasseId { get; set; }
        
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
