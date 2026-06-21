using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace SRV.Models.DTO
{
	public class CadastroFuncaoServidor
	{
		public FuncaoServidor FuncaoServidor { get; set; }

		public UnidadeAdministrativa UnidadeAdministrativa { get; set; }

		[Display(Name = "Função")]
		public IEnumerable Funcoes { get; set; }

		[Display(Name = "Unidade Administrativa")]
		public IEnumerable UnidadesAdministrativas { get; set; }
	}
}