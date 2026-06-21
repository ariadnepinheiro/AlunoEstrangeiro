using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Loader;

namespace SRV.Models.DTO
{
    [ImportFile(Columns = 8)]
    public class ImportFuncaoServidor
    {
        public const int COD_SERVIDOR_INDEX = 0;
        public const int COD_UNIDADE_ADMINISTRATIVA_INDEX = 1;
        public const int COD_FUNCAO_INDEX = 2;
        public const int DT_INICIO_INDEX = 3;
        public const int DT_FIM_INDEX = 4;
        public const int CARGA_HORARIA_ALOCADO_INDEX = 5;
        public const int CARGA_HORARIA_TOTAL_INDEX = 6;
        public const int CCARGA_HORARIA_LIVRE_INDEX = 7;

        [ImportField(
            COD_SERVIDOR_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 8,
            Required = true,
            DataType = DataType.Integer)]
        public int CodServidor { get; set; }

        [ImportField(
            COD_UNIDADE_ADMINISTRATIVA_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 6,
            Required = true,
            DataType = DataType.Integer)]
        public int CodUnidadeAdministrativa { get; set; }

        [ImportField(
            COD_FUNCAO_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 15,
            Required = true,
            DataType = DataType.String)]
        public string CodFuncao { get; set; }

        [ImportField(
            DT_INICIO_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 10,
            Required = true,
            DataType = DataType.DateTime)]
        public DateTime DataInicio { get; set; }

        [ImportField(
            DT_FIM_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 10,
            Required = true,
            DataType = DataType.DateTime)]
        public DateTime DataFim { get; set; }

        [ImportField(
            CARGA_HORARIA_ALOCADO_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 6,
            Required = true,
            DataType = DataType.Integer,
            NumDecimalPlaces = 2,
            AllowsPoint = false)]
        public decimal Alocado { get; set; }

        [ImportField(
            CARGA_HORARIA_TOTAL_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 6,
            Required = true,
            DataType = DataType.Integer,
            NumDecimalPlaces = 2,
            AllowsPoint = false)]
        public decimal Total { get; set; }

        [ImportField(
            CCARGA_HORARIA_LIVRE_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 6,
            Required = true,
            DataType = DataType.Integer,
            NumDecimalPlaces = 2,
            AllowsPoint = false)]
        public decimal Livre { get; set; }
    }
}