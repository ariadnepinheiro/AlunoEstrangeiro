using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using SRV.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace SRV.Models.DTO
{
    public class CadastroParametroNota
    {
        [Display(Name = "Modalidade")]
        public int? IdModalidade { get; set; }
        public IEnumerable Modalidades { get; set; }

        public IList<Indicador> Indicadores { get; set; }

        public IList<Nota> Notas { get; set; }

        public IList<ParametroNotaItem> Values { get; set; }

        public ParametroNotaItem[][] NewValues { get; set; }
    }
}