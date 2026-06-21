using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Loader;

namespace SRV.Models.DTO
{
    [ImportFile(Columns = 5)]
    public class ImportIndicadorUnidadeAdministrativa
    {
        public const int COD_INDICADOR = 0;
        public const int COD_CENSO = 1;
        public const int SIGLA_MODALIDADE = 2;
        public const int DES_NIVEL_ENSINO = 3;
        public const int NM_VALOR_REALIZADO = 4;

        [ImportField(
            COD_INDICADOR,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 9,
            Required = true,
            DataType = DataType.Integer)]
        public int CodIndicador { get; set; }

        [ImportField(
            COD_CENSO,
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
            SIGLA_MODALIDADE,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 20,
            Required = true,
            DataType = DataType.String)]
        public string CodModalidade { get; set; }

        [ImportField(
            DES_NIVEL_ENSINO,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 50,
            Required = true,
            DataType = DataType.String)]
        public string DesNivelEnsino { get; set; }

        // Valor opcional
        [ImportField(
            NM_VALOR_REALIZADO,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 6,
            DataType = DataType.Integer,
            NumDecimalPlaces = 2,
            AllowsPoint = false)]
        public decimal? ValorRealizado { get; set; }
    }
}