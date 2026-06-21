using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Logging;
using System.ComponentModel.DataAnnotations;

namespace SRV.Models.Domain
{
	public class FuncaoServidor
	{
		[PrimaryKey]
		[Display(Name = "Código")]
		public int IdFuncaoServidor { get; set; }

		public Servidor Servidor { get; set; }

		[Display(Name = "Função")]
		public Funcao Funcao { get; set; }

		public AnoReferencia AnoReferencia { get; set; }

		[Display(Name = "Data Início da Função")]
		[Required(ErrorMessageResourceName="Required", ErrorMessageResourceType=typeof(Properties.Resources))]
		public DateTime DataInicioFuncao { get; set; }

		[Display(Name = "Data Fim da Função")]
		public DateTime? DataFimFuncao { get; set; }

		public decimal? Proporcionalidade { get; set; }

		[Display(Name = "Unidade Administrativa")]
		public UnidadeAdministrativa UnidadeAdministrativa { get; set; }

		public DateTime? DataFimOriginal { get; set; }

		private string elegivel;
		public string Elegivel
		{
			get { return elegivel == "S" ? "Sim" : "Não"; }
			set { elegivel = value; }
		}

		public decimal? Nota { get; set; }

		[Display(Name = "Carga Horária Alocada")]
		[DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
		public decimal? CargaHorariaAlocada { get; set; }

		[Display(Name = "Carga Horária Livre")]
		[DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
		public decimal? CargaHorariaLivre { get; set; }

		[Display(Name = "Carga Horária Total")]
		[DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
		public decimal? CargaHorariaTotal { get; set; }

		[Display(Name = "Recurso")]
		[StringLength(250, ErrorMessage = "Campo não pode ter mais de {1} caracteres")]
		public string Recurso { get; set; }

		public int TotalDiasAlocado { get; set; }

		public decimal PercentualTotalAlocado { get; set; }
	}
}