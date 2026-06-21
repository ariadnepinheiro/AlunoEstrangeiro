using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SRV.Common.Logging;

namespace SRV.Models.Domain
{
    public class GrupoFuncao
    {
        [PrimaryKey]
        [Display(Name = "Código")]
        public int? IdGrupoFuncao { get; set; }

        [Display(Name = "Grupo de Função")]
        [StringLength(50, ErrorMessage = "Campo não pode ter mais de {1} caracteres")]
        public string DesGrupoFuncao { get; set; }

        public override string ToString()
        {
            return IdGrupoFuncao.ToString();
        }
    }
}