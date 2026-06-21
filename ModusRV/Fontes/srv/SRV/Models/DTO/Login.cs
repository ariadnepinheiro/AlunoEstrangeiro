using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SRV.Common.Validation;
using System.Collections;

namespace SRV.Models.DTO
{
    public class Login
    {
        [CustomRequired]
        [Display(Name = "Matrícula")]
        public string Usuario { get; set; }

        [CustomRequired]
        [Display(Name = "Senha")]
        public string Senha { get; set; }

        [CustomRequired]
        [Display(Name = "Ciclo")]
        public int Ciclo { get; set; }

        public IEnumerable Ciclos { get; set; }
    }
}