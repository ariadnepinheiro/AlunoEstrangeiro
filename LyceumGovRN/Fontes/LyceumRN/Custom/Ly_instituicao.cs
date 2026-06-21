using System;
using System.Globalization;
using Techne.Data;
using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN.Custom
{
    public class Ly_instituicaoCustom : Ly_instituicao.CustomBase
    {
        public override string PreInsert(Ly_instituicao.Row row, TConnectionWritable tconnw)
        {
            row.Outra_faculdade = RN.Instituicao.GeraOutraFaculdade();
            return string.Empty;
        }
    }
}