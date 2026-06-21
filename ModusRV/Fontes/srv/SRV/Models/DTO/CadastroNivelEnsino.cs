using SRV.Models.Domain;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRV.Models.DTO
{
    public class CadastroNivelEnsino
    {
        public NivelEnsino NivelEnsino { get; set; }

        public IEnumerable Modalidades { get; set; }
    }
}