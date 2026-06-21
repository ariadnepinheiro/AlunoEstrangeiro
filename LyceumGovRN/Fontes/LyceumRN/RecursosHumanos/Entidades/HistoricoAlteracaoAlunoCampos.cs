using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.RecursosHumanos.Entidades
{
    [AtributoTabela("RecursosHumanos.HISTORICOALTERACAOALUNO_CAMPOS", Nome = "RecursosHumanos.HISTORICOALTERACAOALUNO_CAMPOS")]
    public class HistoricoAlteracaoAlunoCampos : IEntity
    {
        [AtributoCampo(Nome = "HISTORICOALTERACAOALUNO_CAMPOSID")]
        public int HistoricoAlteracaoAlunoCamposId { get; set; }

        [AtributoCampo(Nome = "HISTORICOALTERACAOALUNOID")]
        public int HistoricoAlteracaoAlunoId { get; set; }

        [AtributoCampo(Nome = "CAMPO")]
        public string Campo { get; set; }

        [AtributoCampo(Nome = "VALORANTERIOR")]
        public string ValorAnterior { get; set; }

        [AtributoCampo(Nome = "VALORATUAL")]
        public string ValorAtual { get; set; }
    }
}
