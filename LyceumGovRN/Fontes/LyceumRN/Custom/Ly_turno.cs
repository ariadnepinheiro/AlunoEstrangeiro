using System;
using System.Globalization;

using Techne.Data;

using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN.Custom
{
    public class Ly_turnoCustom : Ly_turno.CustomBase
    {
        public override string PreInsert(Ly_turno.Row row, TConnectionWritable cn)
        {
            if (!string.IsNullOrEmpty(row.Descricao))
                row.Descricao = row.Descricao.Trim();
            if (!string.IsNullOrEmpty(row.Mnemonico))
                row.Mnemonico = row.Mnemonico.Trim();
            if (!string.IsNullOrEmpty(row.Turno))
                row.Turno = row.Turno.Trim();
            if (!string.IsNullOrEmpty(row.Turno))
                row.Turno = row.Turno.Trim();

            return string.Empty;
        }

        public override string PreUpdate(Ly_turno.Row row, TConnectionWritable cn)
        {
            if (!string.IsNullOrEmpty(row.Descricao))
                row.Descricao = row.Descricao.Trim();
            if (!string.IsNullOrEmpty(row.Mnemonico))
                row.Mnemonico = row.Mnemonico.Trim();
            if (!string.IsNullOrEmpty(row.Turno))
                row.Turno = row.Turno.Trim();
            if (!string.IsNullOrEmpty(row.Turno))
                row.Turno = row.Turno.Trim();

            return string.Empty;
        }

        public override string PreDelete(Ly_turno.Row row, TConnectionWritable cn)
        {
            

            return string.Empty;
        }

        //public override string PostInsert(Ly_turno.Row row, TConnectionWritable cn)
        //{
        //    re
        //}

    }
}
