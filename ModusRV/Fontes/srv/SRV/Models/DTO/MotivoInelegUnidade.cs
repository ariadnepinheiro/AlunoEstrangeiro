using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;

namespace SRV.Models.DTO
{
    public class MotivoInelegUnidade
    {
        public UnidadeAdministrativa Unidade { get; set; }

        public IList<string> Motivos { get; set; }
    }
}