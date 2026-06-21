using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace SRV.Models.DTO
{
    public class CadastroOcorrencia
    {
        public Ocorrencia Ocorrencia { get; set; }

        [Display(Name = "Tipo de Ocorrência")]
        public IEnumerable TiposOcorrencia { get; set; }

        public bool OperacaoInsert { get; set; }
    }
}