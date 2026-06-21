using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proderj.DOL.WebApp.Models
{
	public class CadastroGLPIncluiDisponibilidadeRequestModel
	{
        [Required(ErrorMessage = "Informe a regional")]
        [DisplayName("Regional")]
        public int CodigoRegional { get; set; }

		[Required(ErrorMessage = "Informe o município")]
		[DisplayName("Município")]
		public string CodigoMunicipio { get; set; }

		[Required(ErrorMessage = "Informe a disciplina")]
		[DisplayName("Disciplina")]
		public string CodigoDisciplina { get; set; }

		[Required(ErrorMessage = "Informe o dia da semana")]
		[DisplayName("Dia da semana")]
		public short CodigoDiaSemana { get; set; }

		[Required(ErrorMessage = "Informe a hora de inicio da dispinibilidade")]
		[DisplayName("Hora disponível de")]
		public DateTime HoraInicio { get; set; }

		[Required(ErrorMessage = "Informe a hora de fim da dispinibilidade")]
		[DisplayName("Hora disponível até")]
		public DateTime HoraFinal { get; set; }

      
	}
}
