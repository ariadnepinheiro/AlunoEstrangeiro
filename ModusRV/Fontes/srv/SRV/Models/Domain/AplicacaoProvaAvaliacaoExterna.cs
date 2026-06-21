using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Logging;
using System.ComponentModel.DataAnnotations;

namespace SRV.Models.Domain
{
    public class AplicacaoProvaAvaliacaoExterna
    {
        [PrimaryKey]
        [Display(Name = "Código")]
        public int IdAplicacaoProvaAvaliacaoExterna { get; set; }

        [PrimaryKey]
        public AnoReferencia AnoReferencia { get; set; }

        [PrimaryKey]
        public AvaliacaoExterna AvaliacaoExterna { get; set; }

        [PrimaryKey]
        public Servidor Servidor { get; set; }

        [PrimaryKey]
        public UnidadeAdministrativa UnidadeAdministrativa { get; set; }

        public string DesTurma { get; set; }
        public int NmPeriodo { get; set; }
        public string NmPeriodoDesc
        {
            get
            {
                return (NmPeriodo != 0)
                    ? string.Format("{0}º Semestre", NmPeriodo)
                    : "Anual";
            }
        }
        public DateTime DtProva { get; set; }
    }
}