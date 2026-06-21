using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Logging;
using System.ComponentModel.DataAnnotations;

namespace SRV.Models.Domain
{
    public class LancamentoNotaDocente
    {
        [PrimaryKey]
        [Display(Name = "Código")]
        public int IdLancamentoNotaDocente { get; set; }

        public int NmBimestre { get; set; }
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
        public string DesDisciplina { get; set; }

        public AnoReferencia AnoReferencia { get; set; }

        public Servidor Servidor { get; set; }

        public UnidadeAdministrativa UnidadeAdministrativa { get; set; }
    }
}