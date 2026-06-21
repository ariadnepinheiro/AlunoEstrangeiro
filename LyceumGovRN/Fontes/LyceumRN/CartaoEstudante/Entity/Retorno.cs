using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.CartaoEstudante.Entity
{
    class Retorno: IEntity
    {
        [AtributoCampo(Nome = "RETORNOID")]
        public int RetornoId { get; set; }

        [AtributoCampo(Nome = "OPERADORAID")]
        public int OperadoraId { get; set; }

        [AtributoCampo(Nome = "REMESSAID")]
        public int RemessaId { get; set; }

        [AtributoCampo(Nome = "IDBENEFICIARIO")]
        public int IdBeneficiario { get; set; }

        [AtributoCampo(Nome = "DATAPROCESSAMENTO")]
        public DateTime? DataProcessamento { get; set; }

        [AtributoCampo(Nome = "SITUACAOPROCESSAMENTO")]
        public string SituacaoProcessamento { get; set; }

        [AtributoCampo(Nome = "DATAINCLUSAO")]
        public DateTime DataInclusao { get; set; }                
    }
}
