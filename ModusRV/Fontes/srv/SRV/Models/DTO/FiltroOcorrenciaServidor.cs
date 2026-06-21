using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using SRV.Models.Domain;

namespace SRV.Models.DTO
{
    public class FiltroOcorrenciaServidor
    {
        [Display(Name = "Servidor")]
        public int? IdServidor { get; set; }
        public string NomeServidor { get; set; }

        [Display(Name = "Unidade Administrativa")]
        public int? IdUnidadeAdministrativa { get; set; }
        public IEnumerable UnidadesAdministrativas { get; set; }

        [Display(Name = "Ocorrência")]
        public int? IdOcorrencia { get; set; }
        public IEnumerable Ocorrencias { get; set; }

        public Paging<OcorrenciaServidor> OcorrenciasServidores { get; set; }
    }
}