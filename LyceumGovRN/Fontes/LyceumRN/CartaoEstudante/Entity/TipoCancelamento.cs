using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.CartaoEstudante.Entity
{
    class TipoCancelamento: IEntity
    {
        [AtributoCampo(Nome = "TIPOCANCELAMENTOID")]
        public int TipoCancelamentoId { get; set; }

        [AtributoCampo(Nome="CODIGOCANCELAMENTO")]
        public int Codigo { get; set; }

        [AtributoCampo(Nome = "DESCRICAO")]
        public string Descricao { get; set; }
    }
}
