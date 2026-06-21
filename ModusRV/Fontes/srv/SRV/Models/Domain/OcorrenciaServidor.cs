using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Logging;
using System.ComponentModel.DataAnnotations;

namespace SRV.Models.Domain
{
    public class OcorrenciaServidor
    {
        [PrimaryKey]
		[Display(Name = "Código")]
        public int IdOcorrenciaServidor { get; set; }

        public Servidor Servidor { get; set; }

		[Display(Name = "Ocorrência")]
        public Ocorrencia Ocorrencia { get; set; }

		[Display(Name = "Data Início Ocorrência")]
        public DateTime DataInicioOcorrencia { get; set; }

		[Display(Name = "Data Fim Ocorrência")]
        public DateTime? DataFimOcorrencia { get; set; }

        public UnidadeAdministrativa UnidadeAdministrativa { get; set; }

        public DateTime? DataFimOriginal { get; set; }

		[Display(Name = "Recurso")]
		[StringLength(250, ErrorMessage = "Campo não pode ter mais de {1} caracteres")]
		public string Recurso { get; set; }

		public int TotalDiasAlocado { get; set; }

		public decimal PercentualTotalAlocado { get; set; }
    }
}