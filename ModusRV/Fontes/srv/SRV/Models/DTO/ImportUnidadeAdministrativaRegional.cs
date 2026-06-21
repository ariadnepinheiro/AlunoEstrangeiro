using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Loader;

namespace SRV.Models.DTO
{
    [ImportFile(Columns = 3)]
    public class ImportUnidadeAdministrativaRegional
    {
        public const int COD_UNIDADE_ADMINISTRATIVA_INDEX = 0;
        public const int DES_UNIDADE_ADMINISTRATIVA_INDEX = 1;
        public const int COD_CENSO_INDEX = 2;

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
            COD_CENSO_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 8,
            DataType = DataType.String)]
        public string CodCenso { get; set; }
    }
}