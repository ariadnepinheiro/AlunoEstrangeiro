using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.CartaoEstudante.Entity
{
    class RetornoCritica: IEntity
    {
        [AtributoCampo(Nome = "RETORNOCRITICAID")]
        public int RetornoCriticaId { get; set; }

        [AtributoCampo(Nome = "RETORNOID")]
        public int RetornoId { get; set; }

        [AtributoCampo(Nome = "TIPORETORNOCRITICAID")]
        public int TipoRetornoCriticaId { get; set; }

        [AtributoCampo(Nome = "DATAINCLUSAO")]
        public DateTime DataInclusao { get; set; }

        [AtributoCampo(Nome = "OBSERVACAO")]
        public string Observacao { get; set; }
    }
}
