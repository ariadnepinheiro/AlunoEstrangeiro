using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Loader;

namespace SRV.Models.DTO
{
    [ImportFile(Columns = 9)]
    public class ImportAvaliacaoExternaUnidadeAdminDetalhe
    {
        public const int CICLO = 0;
        public const int CENSO_UNIDADE = 1;
        public const int UNIDADE_ESCOLAR = 2;
		public const int TURMA = 3;
		public const int PERIODO = 4;
		public const int TURNO = 5;
		public const int AVALIACAO = 6;
		public const int PREVISTO = 7;
		public const int REALIZADO = 8;

        [ImportField(
			CICLO,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 4,
            Required = true,
            DataType = DataType.Integer)]
        public int AnoReferencia { get; set; }

        [ImportField(
			CENSO_UNIDADE,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 8,
            Required = true,
            DataType = DataType.Integer)]
        public int CensoUnidadeAdministrativa { get; set; }

        [ImportField(
			UNIDADE_ESCOLAR,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 6,
            Required = true,
            DataType = DataType.Integer)]
		public int IdUnidadeAdministrativa { get; set; }

		[ImportField(
			TURMA,
			EnableTrimming = true,
			EnableValidation = true,
			ValidationMaxLength = 20,
			Required = true,
			DataType = DataType.String)]
		public string Turma { get; set; }

		[ImportField(
			PERIODO,
			EnableTrimming = true,
			EnableValidation = true,
			ValidationMaxLength = 1,
			Required = true,
			DataType = DataType.Integer)]
		public int Periodo { get; set; }

		[ImportField(
			TURNO,
			EnableTrimming = true,
			EnableValidation = true,
			ValidationMaxLength = 1,
			Required = true,
			DataType = DataType.String)]
		public string Turno { get; set; }

		[ImportField(
			AVALIACAO,
			EnableTrimming = true,
			EnableValidation = true,
			ValidationMaxLength = 1,
			Required = true,
			DataType = DataType.Integer)]
		public int AvaliacaoExterna { get; set; }

		[ImportField(
			PREVISTO,
			EnableTrimming = true,
			EnableValidation = true,
			ValidationMaxLength = 4,
			Required = true,
			DataType = DataType.Integer)]
		public int Previsto { get; set; }

		[ImportField(
			REALIZADO,
			EnableTrimming = true,
			EnableValidation = true,
			ValidationMaxLength = 4,
			Required = true,
			DataType = DataType.Integer)]
		public int Realizado { get; set; }

    }
}