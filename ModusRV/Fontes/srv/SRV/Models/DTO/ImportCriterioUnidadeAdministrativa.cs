using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Loader;

namespace SRV.Models.DTO
{
    [ImportFile(Columns = 4)]
    public class ImportCriterioUnidadeAdministrativa
    {
        public const int COD_UNIDADE_ADMINISTRATIVA_INDEX = 0;
        public const int PERC_CURRICULO_MINIMO_INDEX = 1;
        public const int PERC_LANCAMENTO_NOTA_INDEX = 2;
        public const int NOTA_IGE_INDEX = 3;

        [ImportField(
            COD_UNIDADE_ADMINISTRATIVA_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 6,
            Required = true,
            DataType = DataType.Integer)]
        public int CodUnidadeAdministrativa { get; set; }

        [ImportField(
            PERC_CURRICULO_MINIMO_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 6,
            Required = true,
            DataType = DataType.Integer,
            NumDecimalPlaces = 2,
            AllowsPoint = false)]
        public decimal? PercCurriculoMinimo { get; set; }

        [ImportField(
            PERC_LANCAMENTO_NOTA_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 6,
            Required = true,
            DataType = DataType.Integer,
            NumDecimalPlaces = 2,
            AllowsPoint = false)]
        public decimal? PercLancamentoNota { get; set; }

        [ImportField(
            NOTA_IGE_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 6,
            DataType = DataType.Integer,
            NumDecimalPlaces = 2,
            AllowsPoint = false)]
        public decimal? NotaIge { get; set; }
    }
}