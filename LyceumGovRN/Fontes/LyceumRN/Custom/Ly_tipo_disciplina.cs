using System;
using System.Globalization;

using Techne.Data;

using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN.Custom
{
    public class Ly_tipo_disciplinaCustom : Ly_tipo_disciplina.CustomBase
    {
        public override string PreInsert(Ly_tipo_disciplina.Row row, TConnectionWritable cn)
        {
            if (!string.IsNullOrEmpty(row.Descricao))
                row.Descricao = row.Descricao.Trim();
            if (!string.IsNullOrEmpty(row.Nivel0))
                row.Nivel0 = row.Nivel0.Trim();
            if (!string.IsNullOrEmpty(row.Nivel1))
                row.Nivel1 = row.Nivel1.Trim();
            if (!string.IsNullOrEmpty(row.Nivel2))
                row.Nivel2 = row.Nivel2.Trim();
            if (!string.IsNullOrEmpty(row.Tipo))
                row.Tipo = row.Tipo.Trim();

            return string.Empty;
        }
        public override string PreUpdate(Ly_tipo_disciplina.Row row, TConnectionWritable cn)
        {
            if (!string.IsNullOrEmpty(row.Descricao))
                row.Descricao = row.Descricao.Trim();
            if (!string.IsNullOrEmpty(row.Nivel0))
                row.Nivel0 = row.Nivel0.Trim();
            if (!string.IsNullOrEmpty(row.Nivel1))
                row.Nivel1 = row.Nivel1.Trim();
            if (!string.IsNullOrEmpty(row.Nivel2))
                row.Nivel2 = row.Nivel2.Trim();
            if (!string.IsNullOrEmpty(row.Tipo))
                row.Tipo = row.Tipo.Trim();

            return string.Empty;
        }
    }
}
