using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.CartaoEstudante.Entity
{
    class Cartao: IEntity
    {
        [AtributoCampo(Nome = "CARTAOID")]
        public int CartaoId { get; set; }

        [AtributoCampo(Nome = "OPERADORAID")]
        public int OperadoraId { get; set; }

        [AtributoCampo(Nome = "NUMEROCHIP")]
        public string NumeroChip { get; set; }

        [AtributoCampo(Nome = "NUMEROCARTAO")]
        public string NumeroCartao { get; set; }

        [AtributoCampo(Nome = "NUMEROLOGICO")]
        public string NumeroLogico { get; set; }

        [AtributoCampo(Nome = "DATAINCLUSAO")]
        public DateTime DataInclusao { get; set; }

        [AtributoCampo(Nome = "FLAGBIOMETRIA")]
        public string FlagBiometria { get; set; }
    }
}
