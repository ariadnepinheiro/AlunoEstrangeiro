using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Custom
{
    public class Ly_unidade_fisicaCustom : Ly_unidade_fisica.CustomBase
    {
        public override string PreInsert(Ly_unidade_fisica.Row row, TConnectionWritable cn)
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
            if (!string.IsNullOrEmpty(row.E_mail) && !Validacao.Validou(row.E_mail, Validacao.Tipo.email)) return "E-mail inválido.<br>O e-mail está em um formato incorreto.";

            return string.Empty;
        }

        public override string PreUpdate(Ly_unidade_fisica.Row row, TConnectionWritable cn)
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
            if (!string.IsNullOrEmpty(row.E_mail) && !Validacao.Validou(row.E_mail, Validacao.Tipo.email)) return "E-mail inválido.<br>O e-mail está em um formato incorreto.";

            return string.Empty;
        }

        public override string PreDelete(Ly_unidade_fisica.Row row, TConnectionWritable cn)
        {
            return string.Empty;
        }
    }
}
