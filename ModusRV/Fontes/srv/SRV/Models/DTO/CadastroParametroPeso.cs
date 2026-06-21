using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using SRV.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace SRV.Models.DTO
{
    public class CadastroParametroPeso
    {
        [Display(Name = "Modalidade")]
        public int? IdModalidade { get; set; }
        public IEnumerable Modalidades { get; set; }

        [Display(Name = "Tipo de Unidade Administrativa")]
        public int? IdTipoUnidadeAdm { get; set; }
        public IEnumerable TiposUnidadesAdm { get; set; }

        public IList<GrupoFuncao> Grupos { get; set; }

        public IList<ParametroPeso> Values { get; set; }

        public ParametroPeso[][] NewValues { get; set; }

        public bool[] Errors { get; set; }
    }
}