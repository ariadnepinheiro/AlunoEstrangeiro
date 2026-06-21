using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Proderj.DOL.Service;

namespace Proderj.DOL.WebApp.Models
{
	public class IncluiDisponibilidadeRequestModel
	{
        [Required(ErrorMessage = "Informe a REGIONAL.")]
        public int CodigoRegional { get; set; }

		[Required(ErrorMessage = "Informe o MUNICÍPIO.")]
		public string CodigoMunicipio { get; set; }

        [Required(ErrorMessage = "Informe a UNIDADE ESCOLAR.")]
        public string CodigoUE { get; set; }

        [Required(ErrorMessage = "Informe a DISCIPLINA.")]
        public string CodigoDisciplina { get; set; }

        [Required(ErrorMessage = "Informe a MODALIDADE.")]
        public string[] CodigoModalidade { get; set; }

        [Required(ErrorMessage = "Informe o DIA DA SEMANA.")]
        public DiaDaSemanaEnum[] CodigoDiaSemana { get; set; }

        [Required(ErrorMessage = "Informe o TURNO.")]
        public string[] CodigoTurno { get; set; }
	}
}
