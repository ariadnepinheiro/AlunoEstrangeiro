using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SRV.Models.Domain;
using System.Collections;

namespace SRV.Models.DTO
{
    public class FiltroIndicadorUnidadeAdministrativa
    {
        [Display(Name = "Nível de Ensino")]
        public int? IdNivelEnsino { get; set; }
        public IEnumerable NiveisEnsino { get; set; }

        [Display(Name = "Unidade Administrativa")]
        public int? IdUnidadeAdministrativa { get; set; }
        public IEnumerable UnidadesAdministrativas { get; set; }

        [Display(Name = "Modalidade")]
        public int? IdModalidade { get; set; }
        public IEnumerable Modalidades { get; set; }

        [Display(Name = "Indicador")]
        public int? IdIndicador { get; set; }
        public IEnumerable Indicadores { get; set; }

        public Paging<IndicadorUnidadeAdministrativa> IndicadoresUnidades { get; set; }
    }
}