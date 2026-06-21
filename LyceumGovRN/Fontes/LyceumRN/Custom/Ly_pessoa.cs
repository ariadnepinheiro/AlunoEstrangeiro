using System;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Custom
{
    public class Ly_pessoaCustom : Ly_pessoa.CustomBase
    {
        public override string PreInsert(Ly_pessoa.Row row, TConnectionWritable cn)
        {
            row.Pessoa = RN.Pessoa.GeraPessoa(cn);
            return string.Empty;
        }

        public override string PreUpdate(Ly_pessoa.Row row, TConnectionWritable cn)
        {
            return string.Empty;
        }

        public override string PreDelete(Ly_pessoa.Row row, TConnectionWritable cn)
        {
            return string.Empty;
        }
    }
}