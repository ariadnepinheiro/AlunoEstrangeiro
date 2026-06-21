using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Loader;

namespace SRV.Models.DTO
{
    [ImportFile(Columns = 6)]
    public class ImportDenunciaAvaliacaoExterna
    {
        public const int CICLO_INDEX = 0;
        public const int CENSO_UNIDADE_INDEX = 1;
        public const int UNIDADE_ADMINISTRATIVA_INDEX = 2;
        public const int MATRICULA_INDEX = 3;
        public const int AVALIACAO_INDEX = 4;
        public const int MOTIVO_INDEX = 5;

        [ImportField(
            CICLO_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 4,
            Required = true,
            DataType = DataType.Integer)]
        public int Ciclo { get; set; }

        [ImportField(
            CENSO_UNIDADE_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 8,
            Required = true)]
        public string CensoUnidade { get; set; }

        [ImportField(
            UNIDADE_ADMINISTRATIVA_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 6,
            Required = true,
            DataType = DataType.Integer)]
        public int UnidadeAdministrativa { get; set; }

        [ImportField(
            MATRICULA_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 8,
            Required = true,
            DataType = DataType.Integer)]
        public int Matricula { get; set; }

        [ImportField(
            AVALIACAO_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 1,
            Required = true,
            DataType = DataType.Integer)]
        public int Avaliacao { get; set; }

        [ImportField(
            MOTIVO_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 1000,
            Required = true)]
        public string Motivo { get; set; }
    }
}