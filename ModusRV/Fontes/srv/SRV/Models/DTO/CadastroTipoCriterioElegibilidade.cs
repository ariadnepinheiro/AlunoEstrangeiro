using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using System.Collections;

namespace SRV.Models.DTO
{
    public class CadastroTipoCriterioElegibilidade
    {
        public TipoCriterioElegibilidade TipoCriterioElegibilidade { get; set; }

        public IEnumerable AnosReferencia { get; set; }

        public IEnumerable TiposUnidadeAdministrativa { get; set; }
    }
}