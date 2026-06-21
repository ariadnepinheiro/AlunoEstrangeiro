using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.CartaoEstudante.Entity
{
    class CartaoSituacao: IEntity
    {
        [AtributoCampo(Nome = "CARTAOSITUACAOID")]
        public int CartaoSituacaoId { get; set; }

        [AtributoCampo(Nome = "TIPOCANCELAMENTOID")]
        public int TipoCancelamentoId { get; set; }

        [AtributoCampo(Nome = "RETORNOCARTAOID")]
        public int RetornoCartaoId { get; set; }

        [AtributoCampo(Nome = "TIPOSITUACAOCARTAOID")]
        public int TipoSituacaoCartaoId { get; set; }

        [AtributoCampo(Nome = "CARTAOID")]
        public int CartaoId { get; set; }

        [AtributoCampo(Nome = "SITUACAOCARTAO")]
        public string SituacaoCartao { get; set; }

        [AtributoCampo(Nome = "DATASITUACAO")]
        public DateTime DataSituacao { get; set; }

        [AtributoCampo(Nome = "DATAINCLUSAO")]
        public DateTime DataInclusao { get; set; }
    }
}
