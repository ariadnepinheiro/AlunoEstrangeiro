using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace SRV.Models.DTO
{
    public class FiltroRptConsolidado
    {
        [Display(Name = "Ano Referência")]
        public int? IdAnoReferencia { get; set; }
        public IEnumerable Referencias { get; set; }

        [Display(Name = "Regional")]
        public int? IdRegional { get; set; }
        public IEnumerable Regionais { get; set; }

        [Display(Name = "Unidade Administrativa")]
        public int? IdUnidadeAdministrativa { get; set; }
        public IEnumerable UnidadesAdministrativas { get; set; }

        [Display(Name = "Matrícula Servidor")]
        public int? MatriculaServidor { get; set; }

        [Display(Name = "Nome Servidor")]
        public string NoServidor { get; set; }

        public Dictionary<int, ConsolidadoUnidadeAdm> Resultado { get; set; }

        public string DownloadToken { get; set; }
    }
}