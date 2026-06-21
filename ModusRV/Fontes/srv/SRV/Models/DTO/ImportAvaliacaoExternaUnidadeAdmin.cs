using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Loader;

namespace SRV.Models.DTO
{
    [ImportFile(Columns = 3)]
    public class ImportAvaliacaoExternaUnidadeAdmin
    {
        public const int COD_AVALIACAO_EXTERNA = 0;
        public const int COD_UNIDADE_ADMINISTRATIVA = 1;
        public const int NM_PERC_PARTICIPACAO = 2;

        [ImportField(
            COD_AVALIACAO_EXTERNA,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 9,
            Required = true,
            DataType = DataType.Integer)]
        public int CodAvaliacaoExterna { get; set; }

        [ImportField(
            COD_UNIDADE_ADMINISTRATIVA,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 6,
            Required = true,
            DataType = DataType.Integer)]
        public int CodUnidadeAdministrativa { get; set; }

        [ImportField(
            NM_PERC_PARTICIPACAO,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 6,
            Required = true,
            DataType = DataType.Integer,
            NumDecimalPlaces = 2,
            AllowsPoint = false)]
        public decimal PercParticipacao { get; set; }

    }
}