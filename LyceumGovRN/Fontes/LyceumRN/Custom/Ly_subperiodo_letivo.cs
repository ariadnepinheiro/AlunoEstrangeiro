using System;
using System.Globalization;

using Techne.Data;

using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN.Custom
{
    public class Ly_subperiodo_letivoCustom : Ly_subperiodo_letivo.CustomBase
    {
        public override string PreInsert(Ly_subperiodo_letivo.Row row, TConnectionWritable cn)
        {
            if (!string.IsNullOrEmpty(row.Ano.ToString())) row.Ano = Convert.ToDecimal(row.Ano.ToString().Trim());
            if (!string.IsNullOrEmpty(row.Periodo.ToString())) row.Periodo = Convert.ToDecimal(row.Periodo.ToString().Trim());
            if (!string.IsNullOrEmpty(row.Subperiodo.ToString())) row.Subperiodo = Convert.ToDecimal(row.Subperiodo.ToString().Trim());
            if (!string.IsNullOrEmpty(row.Descricao.ToString())) row.Descricao = Convert.ToString(row.Descricao.ToString().Trim());
            if (!string.IsNullOrEmpty(row.Dt_inicio.ToString())) row.Dt_inicio = Convert.ToDateTime(row.Dt_inicio.ToString().Trim());
            if (!string.IsNullOrEmpty(row.Dt_fim.ToString())) row.Dt_fim = Convert.ToDateTime(row.Dt_fim.ToString().Trim());
            if(!string.IsNullOrEmpty(row.Dias_letivos.ToString())) row.Dias_letivos = Convert.ToDecimal(row.Dias_letivos.ToString().Trim());

            return string.Empty;
        }

        public override string PreUpdate(Ly_subperiodo_letivo.Row row, TConnectionWritable cn)
        {
            if (!string.IsNullOrEmpty(row.Ano.ToString())) row.Ano = Convert.ToDecimal(row.Ano.ToString().Trim());
            if (!string.IsNullOrEmpty(row.Periodo.ToString())) row.Periodo = Convert.ToDecimal(row.Periodo.ToString().Trim());
            if (!string.IsNullOrEmpty(row.Subperiodo.ToString())) row.Subperiodo = Convert.ToDecimal(row.Subperiodo.ToString().Trim());
            if (!string.IsNullOrEmpty(row.Descricao.ToString())) row.Descricao = Convert.ToString(row.Descricao.ToString().Trim());
            if (!string.IsNullOrEmpty(row.Dt_inicio.ToString())) row.Dt_inicio = Convert.ToDateTime(row.Dt_inicio.ToString().Trim());
            if (!string.IsNullOrEmpty(row.Dt_fim.ToString())) row.Dt_fim = Convert.ToDateTime(row.Dt_fim.ToString().Trim());
            if (!string.IsNullOrEmpty(row.Dias_letivos.ToString())) row.Dias_letivos = Convert.ToDecimal(row.Dias_letivos.ToString().Trim());

            return string.Empty;
        }

        public override string PreDelete(Ly_subperiodo_letivo.Row row, TConnectionWritable cn)
        {

            return string.Empty;
        }
    }
}
