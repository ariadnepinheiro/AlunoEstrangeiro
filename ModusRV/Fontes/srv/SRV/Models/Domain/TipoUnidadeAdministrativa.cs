using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SRV.Common.Logging;
using System.ComponentModel;


namespace SRV.Models.Domain
{
    public class TipoUnidadeAdministrativa
    {
        public enum Tipos_Upload
        {
            [DescriptionAttribute("Unidade Regional")]
            REGIONAL = 1,

            [DescriptionAttribute("Unidade Escolar")]
            ESCOLAR = 2,

            [DescriptionAttribute("Demais Unidades")]
            DEMAIS_UNIDADES = 3
        }

        [PrimaryKey]
        [Display(Name = "Código")]
        public int? IdTipoUnidAdm { get; set; }

        [Display(Name = "Tipo Unidade Administrativa")]
        [StringLength(50, ErrorMessage = "Campo não pode ter mais de {1} caracteres")]
        public string DesTipoUnidAdm { get; set; }

        public override string ToString()
        {
            return IdTipoUnidAdm.ToString();
        }
    }
}