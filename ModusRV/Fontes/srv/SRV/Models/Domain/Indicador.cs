using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using SRV.Common.Logging;

namespace SRV.Models.Domain
{
    public enum TipoIndicador
    {
        [DescriptionAttribute("")]
        Vazio = 0,

        [DescriptionAttribute("ELEGIBILIDADE")]
        Elegibilidade = 1,

        [DescriptionAttribute("PARÂMETRO")]
        Parametro = 2
    }

    public class Indicador
    {
        [PrimaryKey]
        [Display(Name = "Código")]
        public int? IdIndicador { get; set; }

        [Display(Name = "Indicador")]
        [StringLength(50, ErrorMessage = "Campo não pode ter mais de {1} caracteres")]
        public string DesIndicador { get; set; }

        [Display(Name = "Tipo de Indicador")]
        public TipoIndicador TipoIndicador { get; set; }

        public override string ToString()
        {
            return IdIndicador.ToString();
        }
    }
}