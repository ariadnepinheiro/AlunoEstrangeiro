using System;
using System.Globalization;

using Techne.Data;

using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN.Custom
{
    public class Ly_grupo_conceitoCustom : Ly_grupo_conceito.CustomBase
    {
        public override string PreInsert(Ly_grupo_conceito.Row row, TConnectionWritable cn)
        {
            if (!string.IsNullOrEmpty(row.Descricao))
                row.Descricao = row.Descricao.Trim();
            if (!string.IsNullOrEmpty(row.Grupo))
                row.Grupo = row.Grupo.Trim();

            return string.Empty;
        }
        public override string PreUpdate(Ly_grupo_conceito.Row row, TConnectionWritable cn)
        {
            if (!string.IsNullOrEmpty(row.Descricao))
                row.Descricao = row.Descricao.Trim();
            if (!string.IsNullOrEmpty(row.Grupo))
                row.Grupo = row.Grupo.Trim();

            return string.Empty;
        }
    }
}
