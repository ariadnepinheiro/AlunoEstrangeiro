using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SRV.Models.Domain;
using System.Collections;

namespace SRV.Models.DTO
{
    public class FiltroFuncao
    {
        [Display(Name = "Código")]
        public string IdFuncao { get; set; }

        [Display(Name = "Função")]
        public string DesFuncao { get; set; }

        [Display(Name = "Grupo Função")]
        public int? IdGrupoFuncao { get; set; }
        public IEnumerable GruposFuncoes { get; set; }

        public Paging<Funcao> Funcoes { get; set; }
    }
}