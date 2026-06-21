using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SRV.Common.Logging;

namespace SRV.Models.Domain
{
    public class Nota
    {
        [PrimaryKey]
        [Display(Name = "Código")]
        public int? IdNota { get; set; }

        [Display(Name = "Nota")]
        public decimal? DesNota { get; set; }

        public AnoReferencia AnoReferencia { get; set; }

        public override string ToString()
        {
            return IdNota.ToString();
        }
    }
}