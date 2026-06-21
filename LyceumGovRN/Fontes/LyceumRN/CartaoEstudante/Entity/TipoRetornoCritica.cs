using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.CartaoEstudante.Entity
{
    class TipoRetornoCritica : IEntity
    {
        [AtributoCampo(Nome = "TIPORETORNOCRITICAID")]
        public int TipoRetornoCriticaId { get; set; }
        
        [AtributoCampo(Nome = "CODIGOCRITICA")]
        public string CodigoCritica { get; set; }
        
        [AtributoCampo(Nome = "DESCRICAO")]
        public string Descricao { get; set; }
        
        [AtributoCampo(Nome = "DATAINCLUSAO")]
        public DateTime DataInclusao { get; set; }
    }
}
