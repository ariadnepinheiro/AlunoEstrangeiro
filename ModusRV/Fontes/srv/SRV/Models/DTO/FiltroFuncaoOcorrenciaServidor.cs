using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using SRV.Models.Domain;

namespace SRV.Models.DTO
{
	public class FiltroFuncaoOcorrenciaServidor
	{
		[Display(Name = "Matrícula")]
		public int? IdServidor { get; set; }
		[Display(Name = "Servidor")]
		public string NomeServidor { get; set; }

		public string CoeficienteServidor { get; set; }
		public DateTime DataUltimoProcessamento { get; set; }

		[Display(Name = "Total de Alocação")]
		public int TotalDiasAlocado { get; set; }
		public decimal PercentualTotalAlocado { get; set; }

		[Display(Name = "Total de Afastamento")]
		public int TotalDiasAfastamento { get; set; }
		public decimal PercentualTotalAfastamento { get; set; }	

		public Paging<FuncaoServidor> FuncoesServidores { get; set; }
		public Paging<OcorrenciaServidor> OcorrenciasServidores { get; set; }


	}
}