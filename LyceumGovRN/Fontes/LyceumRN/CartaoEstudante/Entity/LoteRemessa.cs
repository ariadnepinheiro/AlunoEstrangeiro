using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.CartaoEstudante.Entity
{
    class LoteRemessa : IEntity
    {
        [AtributoCampo(Nome = "LOTEREMESSAID")]
        public int LoteRemessaId { get; set; }
        [AtributoCampo(Nome = "NOMELOTEREMESSA")]
        public string Nome { get; set; }
        [AtributoCampo(Nome = "QUANTIDADEREGISTROS")]
        public int QtdRegistros { get; set; }
        [AtributoCampo(Nome = "OPERADORAID")]
        public int OperadoraId { get; set; }
        [AtributoCampo(Nome = "DATAGERACAO")]
        public DateTime DataGeracao { get; set; }
        [AtributoCampo(Nome = "SITUACAOPROCESSAMENTO")]
        public string SituacaoProcessamento { get; set; }
    }
}
