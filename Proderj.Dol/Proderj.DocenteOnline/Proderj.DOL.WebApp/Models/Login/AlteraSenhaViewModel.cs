using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Proderj.DOL.WebApp.Models
{
    public class AlteraSenhaViewModel: ViewModelPadrao
    {
        [Required]
        public string Matricula { get; set; }
		
		public string IdFuncional { get; set; }
		 
		public string Vinculo { get; set; }

        [Required]        
        public string SenhaAtual { get; set; }

        [Required]
        public string SenhaNova { get; set; }

        [Required]
        public string SenhaNovaConfirmacao { get; set; }
    }
}