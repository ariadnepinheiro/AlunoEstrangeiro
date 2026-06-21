using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Loader;

namespace SRV.Models.DTO
{
    [ImportFile(Columns = 5)]
    public class ImportUnidadeAdministrativaDemaisUnidades
    {    

        public const int COD_UNIDADE_ADMINISTRATIVA_INDEX = 0;
        public const int DES_UNIDADE_ADMINISTRATIVA_INDEX = 1;
        public const int COD_UNIDADE_REGIONAL_INDEX = 2;
        public const int COD_CENSO_INDEX = 3;
        public const int COD_TIPO_UNIDADE_INDEX = 4;

        [ImportField(
            COD_UNIDADE_ADMINISTRATIVA_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 6,
            Required = true,
            DataType = DataType.Integer)]
        public int CodUnidadeAdministrativa { get; set; }

        [ImportField(
            DES_UNIDADE_ADMINISTRATIVA_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 200,
            Required = true,
            DataType = DataType.String)]
        public string DesUnidadeAdministrativa { get; set; }

        [ImportField(
            COD_UNIDADE_REGIONAL_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 6,
            Required = true,
            DataType = DataType.Integer)]
        public int CodUnidadeRegional { get; set; }

        [ImportField(
            COD_CENSO_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 8,
            DataType = DataType.String)]
        public string CodCenso { get; set; }

        [ImportField(
            COD_TIPO_UNIDADE_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 9,
            Required = true,
            DataType = DataType.Integer)]
        public int CodTipoUnidadeAdministrativa { get; set; }
    }
}