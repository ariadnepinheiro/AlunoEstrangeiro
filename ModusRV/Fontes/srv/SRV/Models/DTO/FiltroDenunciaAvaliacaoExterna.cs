using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SRV.Models.Domain;

namespace SRV.Models.DTO
{
    public class FiltroDenunciaAvaliacaoExterna
    {
        [Display(Name = "Matrícula")]
        public int? IdServidor { get; set; }

        [Display(Name = "Nome")]
        public string DesNomeServidor { get; set; }

        public Paging<DenunciaAvaliacaoExterna> DenunciasAvaliacoesExternas { get; set; }
    }
}