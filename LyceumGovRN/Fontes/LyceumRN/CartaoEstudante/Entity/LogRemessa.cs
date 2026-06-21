using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.CartaoEstudante.Entity
{
    class LogRemessa : IEntity
    {
        [AtributoCampo(Nome = "LOGREMESSAID")]
        public int LogRemessaId { get; set; }
        [AtributoCampo(Nome = "REMESSAID")]
        public int RemessaId { get; set; }
        [AtributoCampo(Nome = "DATAENVIO")]
        public DateTime DataEnvio { get; set; }
    }
}
