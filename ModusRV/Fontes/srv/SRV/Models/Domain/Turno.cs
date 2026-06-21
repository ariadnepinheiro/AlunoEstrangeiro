using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using SRV.Common.Logging;
using System.ComponentModel.DataAnnotations;

namespace SRV.Models.Domain
{
	public enum CodigoTurno
	{
		[DescriptionAttribute("Ampliado")]
		A = 0,
		[DescriptionAttribute("Integral")]
		I = 1,
		[DescriptionAttribute("Manhã")]
		M = 2,
		[DescriptionAttribute("Noite")]
		N = 3,
		[DescriptionAttribute("Tarde")]
		T = 4
	}

	public enum GrupoTurno
	{
		[DescriptionAttribute("Diurno")]
		D = 0,
		[DescriptionAttribute("Noturno")]
		N = 1
	}
	
	public class Turno
	{
		[PrimaryKey]
		public int IdTurno { get; set; }

		public CodigoTurno Codigo { get; set; }

		[StringLength(20, ErrorMessage = "Campo não pode ter mais de {1} caracteres")]
		public string Nome { get; set; }

		public GrupoTurno Grupo { get; set; }
	}
}