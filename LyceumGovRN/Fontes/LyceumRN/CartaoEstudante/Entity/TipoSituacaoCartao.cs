using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.CartaoEstudante.Entity
{
    class TipoSituacaoCartao: IEntity
    {
        [AtributoCampo(Nome = "TIPOSITUACAOCARTAOID")]
        public int TipoSituacaoCartaoId { get; set; }

        [AtributoCampo(Nome = "CODIGOSITUACAOCARTAO")]
        public string CodigoSituacaoCartao { get; set; }

        [AtributoCampo(Nome = "DESCRICAO")]
        public string Descricao { get; set; }
    }
}
