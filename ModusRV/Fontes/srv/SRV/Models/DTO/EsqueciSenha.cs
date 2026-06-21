using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace SRV.Models.DTO
{
    public class EsqueciSenha
    {
        [CustomRequired]
        [Display(Name = "Matrícula")]
        public string Login { get; set; }
    }
}