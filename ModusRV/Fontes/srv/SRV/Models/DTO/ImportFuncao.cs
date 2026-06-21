using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Loader;

namespace SRV.Models.DTO
{
    [ImportFile(Columns = 3)]
    public class ImportFuncao
    {
        public const int COD_FUNCAO_INDEX = 0;
        public const int DES_FUNCAO_INDEX = 1;
        public const int COD_GRUPO_FUNCAO_INDEX = 2;

        [ImportField(
            COD_FUNCAO_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 15,
            Required = true,
            DataType = DataType.String)]
        public string CodFuncao { get; set; }

        [ImportField(
            DES_FUNCAO_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 200,
            Required = true,
            DataType = DataType.String)]
        public string DesFuncao { get; set; }

        [ImportField(
            COD_GRUPO_FUNCAO_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 9,
            Required = false,
            DataType = DataType.Integer)]
        public int? CodGrupoFuncao { get; set; }
    }
}