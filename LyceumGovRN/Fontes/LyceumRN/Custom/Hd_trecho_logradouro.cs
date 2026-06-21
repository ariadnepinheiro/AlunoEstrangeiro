using Techne.Data;
using Techne.HadesLyc.CR;
using Techne.Lyceum.RN.Util;

namespace Techne.HadesLyc.RN.Custom
{
    public class Hd_trecho_logradouroCustom : Hd_trecho_logradouro.CustomBase
    {
        public override string PreInsert(Hd_trecho_logradouro.Row row, TConnectionWritable cn)
        {
            
            if (!string.IsNullOrEmpty(row.Cep)) row.Cep = row.Cep.RetirarCaracteres();
            return string.Empty;
        }

        public override string PreUpdate(Hd_trecho_logradouro.Row row, TConnectionWritable cn)
        {
            if (!string.IsNullOrEmpty(row.Cep)) row.Cep = row.Cep.RetirarCaracteres();
            return string.Empty;
        }
    }

}