using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Logging;

namespace SRV.Models.Domain
{
    public class MotivoInelegDocente
    {
        [PrimaryKey]
        public int MotivoInelegDocenteId { get; set; }

        [PrimaryKey]
        public AnoReferencia AnoReferencia { get; set; }

        [PrimaryKey]
        public Servidor Servidor { get; set; }

        [PrimaryKey]
        public MotivoInelegibilidade MotivoInelegibilidade { get; set; }
    }
}