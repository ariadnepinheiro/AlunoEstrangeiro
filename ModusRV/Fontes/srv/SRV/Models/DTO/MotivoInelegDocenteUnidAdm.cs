using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;

namespace SRV.Models.DTO
{
    public class MotivoInelegDocenteUnidAdm
    {
        public int IdAnoReferencia { get; set; }
        public UnidadeAdministrativa UnidadeAdministrativa { get; set; }
        public Servidor Servidor { get; set; }

        public IList<MotivoInelegibilidade> Motivos { get; set; }
    }
}