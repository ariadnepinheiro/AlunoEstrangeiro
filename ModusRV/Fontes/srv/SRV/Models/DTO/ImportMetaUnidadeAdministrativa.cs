using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Loader;

namespace SRV.Models.DTO
{
    [ImportFile(Columns = 5)]
    public class ImportMetaUnidadeAdministrativa
    {
        public const int COD_INDICADOR_INDEX = 0;
        public const int COD_CENSO_INDEX = 1;
        public const int SIGLA_MODALIDADE_INDEX = 2;
        public const int DES_NIVEL_ENSINO_INDEX = 3;
        public const int VALOR_META_INDEX = 4;

        [ImportField(
            COD_INDICADOR_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 9,
            Required = true,
            DataType = DataType.Integer)]
        public int CodIndicador { get; set; }

        [ImportField(
            COD_CENSO_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 8,
            Required = true,
            DataType = DataType.String)]
        public string CodCenso { get; set; }

        /// <summary>
        /// Sigla da modalidade
        /// </summary>
        [ImportField(
            SIGLA_MODALIDADE_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 20,
            Required = true,
            DataType = DataType.String)]
        public string CodModalidade { get; set; }

        [ImportField(
            DES_NIVEL_ENSINO_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 50,
            Required = true,
            DataType = DataType.String)]
        public string DesNivelEnsino { get; set; }

        // Valor opcional
        [ImportField(
            VALOR_META_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 6,
            DataType = DataType.Integer,
            NumDecimalPlaces = 2,
            AllowsPoint = false)]
        public decimal? ValorMeta { get; set; }
    }
}