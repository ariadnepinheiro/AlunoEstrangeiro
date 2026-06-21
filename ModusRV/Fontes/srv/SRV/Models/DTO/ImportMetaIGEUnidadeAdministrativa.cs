using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Loader;

namespace SRV.Models.DTO
{
    [ImportFile(Columns = 2)]
    public class ImportMetaIGEUnidadeAdministrativa
    {
        public const int COD_UNIDADE_ADMINISTRATIVA_INDEX = 0;
        public const int NM_META_IGE_INDEX = 1;

        [ImportField(
            COD_UNIDADE_ADMINISTRATIVA_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 6,
            Required = true,
            DataType = DataType.Integer)]
        public int CodUnidadeAdministrativa { get; set; }

        [ImportField(
            NM_META_IGE_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 6,
            Required = true,
            DataType = DataType.Integer,
            NumDecimalPlaces = 2,
            AllowsPoint = false)]
        public decimal MetaIge { get; set; }
    }
}