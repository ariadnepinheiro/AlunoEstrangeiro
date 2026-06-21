using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Loader;

namespace SRV.Models.DTO
{
    [ImportFile(Columns = 4)]
    public class ImportOcorrenciaServidor
    {
        public const int COD_SERVIDOR_INDEX = 0;
        public const int COD_OCORRENCIA_INDEX = 1;
        public const int DT_INICIO_INDEX = 2;
        public const int DT_FIM_INDEX = 3;

        [ImportField(
            COD_SERVIDOR_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 8,
            Required = true,
            DataType = DataType.Integer)]
        public int CodServidor { get; set; }

        [ImportField(
            COD_OCORRENCIA_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 9,
            Required = true,
            DataType = DataType.Integer)]
        public int CodOcorrencia { get; set; }

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
    }
}