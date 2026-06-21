using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Proderj.DOL.Service;

namespace Proderj.DOL.WebApp.Models
{
	public class ListaDisponibilidadeRequestModel
	{
        [Required]
        public int? NUM_FUNC { get; set; }
        
        [Required]
        public int? ANO { get; set; }
	}
}
