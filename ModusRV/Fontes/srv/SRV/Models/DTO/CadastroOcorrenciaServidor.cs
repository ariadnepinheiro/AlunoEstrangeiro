using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace SRV.Models.DTO
{
	public class CadastroOcorrenciaServidor
	{
		public OcorrenciaServidor OcorrenciaServidor { get; set; }

		[Display(Name = "Ocorrências")]
		public IEnumerable Ocorrencias { get; set; }
	}
}