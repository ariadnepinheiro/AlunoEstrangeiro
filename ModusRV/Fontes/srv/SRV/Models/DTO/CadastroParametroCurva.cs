using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using SRV.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace SRV.Models.DTO
{
    public class CadastroParametroCurva
    {
        [Display(Name = "Tipo de Unidade Administrativa")]
        public int? IdTipoUnidadeAdm { get; set; }
        public IEnumerable TiposUnidadesAdm { get; set; }

        public IList<GrupoFuncao> Grupos { get; set; }

        public IList<Nota> Notas { get; set; }

        public IList<ParametroCurvaItem> Values { get; set; }

        public ParametroCurvaItem[][] NewValues { get; set; }
    }
}