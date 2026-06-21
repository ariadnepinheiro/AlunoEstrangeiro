using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace SRV.Models.DTO
{
	public class FiltroAplicacaoProvaAvaliacaoExterna
	{
		[Display(Name = "Matrícula")]
		public int? IdServidor { get; set; }
		
		[Display(Name = "Nome")]
		public string DesNomeServidor { get; set; }
		
		public Paging<AplicacaoProvaAvaliacaoExterna> AplicaoesProvas { get; set; }
	}
}