using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.CartaoEstudante.Entity
{
    public class Operadora : IEntity
    {
        [AtributoCampo(Nome = "OPERADORAID")]
        public int OperadoraId { get; set; }

        [AtributoCampo(Nome = "NOMEOPERADORA")]
        public string NomeOperadora { get; set; }
    }
}