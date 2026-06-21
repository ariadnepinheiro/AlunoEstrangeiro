using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SRV.Models.DTO
{
    public class RptExtratoServidor
    {
        public RptExtratoServidor(string downloadToken)
        {
            this.DownloadToken = downloadToken;
            Retorno = false;
        }

        public RptExtratoServidor()
        {
            Retorno = false;
        }
        
        [Display(Name = "Matrícula")]
        public int MatriculaServidor { get; set; }

        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Display(Name = "CPF")]
        public string CPF { get; set; }
        
        public bool ElegivelUnidade { get; set; }
        public bool ElegivelServidor { get; set; }

        [Display(Name = "Resultado total alcançado")]
        public decimal Resultado { get; set; }

        public int IdAnoReferencia { get; set; }

        public bool Retorno { get; set; }

        public int? IdRegional { get; set; }

        public IList<RptRegionalExtratoServidor> Unidades { get; set; }

        public string DownloadToken { get; set; }
    }
}
