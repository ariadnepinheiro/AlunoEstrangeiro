using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Loader;

namespace SRV.Models.DTO
{
    [ImportFile(Columns = 8)]
    public class ImportLancamentoNotaDocente
    {
        public const int CICLO_INDEX = 0;
        public const int CENSO_UNIDADE_INDEX = 1;
        public const int UNIDADE_ADMINISTRATIVA_INDEX = 2;
        public const int MATRICULA_INDEX = 3;
        public const int BIMESTRE_INDEX = 4;
        public const int TURMA_INDEX = 5;
        public const int PERIODO_INDEX = 6;
        public const int DISCIPLINA_INDEX = 7;

        [ImportField(
            CICLO_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 4,
            Required = true,
            DataType = DataType.Integer)]
        public int Ciclo { get; set; }

        [ImportField(
            CENSO_UNIDADE_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 8,
            Required = true)]
        public string CensoUnidade { get; set; }

        [ImportField(
            UNIDADE_ADMINISTRATIVA_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 6,
            Required = true,
            DataType = DataType.Integer)]
        public int UnidadeAdministrativa { get; set; }

        [ImportField(
            MATRICULA_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 8,
            Required = true,
            DataType = DataType.Integer)]
        public int Matricula { get; set; }

        [ImportField(
            BIMESTRE_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 1,
            Required = true,
            DataType = DataType.Integer)]
        public int Bimestre { get; set; }

        [ImportField(
            TURMA_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 20,
            Required = true)]
        public string Turma { get; set; }

        [ImportField(
            PERIODO_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 1,
            Required = true,
            DataType = DataType.Integer)]
        public int Periodo { get; set; }

        [ImportField(
            DISCIPLINA_INDEX,
            EnableTrimming = true,
            EnableValidation = true,
            ValidationMaxLength = 20,
            Required = true)]
        public string Disciplina { get; set; }
    }
}