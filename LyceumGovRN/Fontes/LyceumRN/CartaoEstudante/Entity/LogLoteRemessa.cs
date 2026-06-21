using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.CartaoEstudante.Entity
{
    class LogLoteRemessa : IEntity
    {
        [AtributoCampo(Nome = "LOGLOTEREMESSAID")]
        public int LogLoteRemessaId { get; set; }
        [AtributoCampo(Nome = "LOGREMESSAID")]
        public int LogRemessaId { get; set; }
        [AtributoCampo(Nome = "DATAENVIO")]
        public DateTime DataEnvio { get; set; }
    }
}
