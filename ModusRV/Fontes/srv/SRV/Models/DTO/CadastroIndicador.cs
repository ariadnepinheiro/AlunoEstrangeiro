using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace SRV.Models.DTO
{
    public class CadastroIndicador
    {
        public Indicador Indicador { get; set; }

        public IEnumerable TiposIndicador { get; set; }
    }
}