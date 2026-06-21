using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Loader;

namespace SRV.Models.DTO
{
	[ImportFile(Columns = 5)]
	public class ImportServidor
	{
		public const int MATRICULA_INDEX = 0;
		public const int NOME_INDEX = 1;
		public const int CPF_INDEX = 2;
		public const int ID_FUNCIONAL_INDEX = 3;
		public const int VINCULO_INDEX = 4;

		[ImportField(
			MATRICULA_INDEX,
			EnableTrimming = true,
			EnableValidation = true,
			ValidationMaxLength = 8,
			Required = true,
			DataType = DataType.Integer)]
		public int Matricula { get; set; }

		[ImportField(
			NOME_INDEX,
			EnableTrimming = true,
			EnableValidation = true,
			ValidationMaxLength = 70,
			Required = true)]
		public string Nome { get; set; }

		[ImportField(
			CPF_INDEX,
			EnableTrimming = true,
			EnableValidation = true,
			ValidationMaxLength = 11,
			Required = true,
			DataType = DataType.Integer)]
		public long Cpf { get; set; }

		[ImportField(
			ID_FUNCIONAL_INDEX,
			EnableTrimming = true,
			EnableValidation = true,
			ValidationMaxLength = 50,
			Required = true)]
		public string IdFuncional { get; set; }

		[ImportField(
			VINCULO_INDEX,
			EnableTrimming = true,
			EnableValidation = true,
			ValidationMaxLength = 3,
			Required = true,
			DataType = DataType.Integer)]
		public int? Vinculo { get; set; }

	}
}