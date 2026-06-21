using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Logging;

namespace SRV.Models.Domain
{
    public class Funcao
    {
        [PrimaryKey]
        public string IdFuncao { get; set; }

        public string DesFuncao { get; set; }

        public decimal? CargaHoraria { get; set; }

        public decimal? CargaHorariaPlan { get; set; }

        public GrupoFuncao GrupoFuncao { get; set; }

        public bool Gratificada { get; set; }

        public override string ToString()
        {
            return IdFuncao.ToString();
        }
    }
}