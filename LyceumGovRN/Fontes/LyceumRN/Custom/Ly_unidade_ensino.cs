using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Custom
{
    public class Ly_unidade_ensinoCustom : Ly_unidade_ensino.CustomBase
    {
        public override string PreInsert(Ly_unidade_ensino.Row row, TConnectionWritable cn)
        {
            if (!string.IsNullOrEmpty(row.Cep))
            {
                row.Cep = row.Cep.RetirarCaracteres();
                if (row.Cep.ToString().Length < 8) return "CEP inválido. <br>O CEP deve ter 8 números.";
            }
            if (!string.IsNullOrEmpty(row.Cgc))
            {
                row.Cgc = row.Cgc.Replace(".", "").Replace("-", "").Replace("/", "");
                if (!Validacao.ValidaCnpj(row.Cgc)) return "CNPJ inválido.<br>O CNPJ está com valores incorretos.";
            }

            return string.Empty;
        }

        public override string PreUpdate(Ly_unidade_ensino.Row row, TConnectionWritable cn)
        {
            if (!string.IsNullOrEmpty(row.Cep))
            {
                row.Cep = row.Cep.RetirarCaracteres();
                if (row.Cep.ToString().Length < 8) return "CEP inválido. <br>O CEP deve ter 8 números.";
            }
            if (!string.IsNullOrEmpty(row.Cgc))
            {
                row.Cgc = row.Cgc.Replace(".", "").Replace("-", "").Replace("/", "");
                if (!Validacao.ValidaCnpj(row.Cgc)) return "CNPJ inválido.<br>O CNPJ está com valores incorretos.";
            }
            return string.Empty;
        }
    }
}
