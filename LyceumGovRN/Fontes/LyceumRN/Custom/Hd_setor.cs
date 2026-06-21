using System;
using System.Collections.Generic;
using System.Text;
using Techne.HadesLyc.CR;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;

namespace Techne.HadesLyc.RN.Custom
{
    public class Hd_setorCustom : Hd_setor.CustomBase
    {
        public override string PreInsert(Hd_setor.Row row, TConnectionWritable cn)
        {
            RNBase.RetirarEspaco(row);

            if (!string.IsNullOrEmpty(row.Fone)) row.Fone = row.Fone.RetirarMascaraTelefone();
            if (!string.IsNullOrEmpty(row.Fax)) row.Fax = row.Fax.RetirarMascaraTelefone();
            if (!string.IsNullOrEmpty(row.Setor) && !Validacao.Validou(row.Setor, Validacao.Tipo.numerico)) return "Setor inválido.<br>O setor deve ter somente números.";
            
            if (!string.IsNullOrEmpty(row.Cnpj))
            {
                row.Cnpj = row.Cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
                if (!Validacao.ValidaCnpj(row.Cnpj)) return "CNPJ inválido.<br>O CNPJ está com valores incorretos.";
            }

            return string.Empty;
        }

        public override string PreUpdate(Hd_setor.Row row, TConnectionWritable cn)
        {
            RNBase.RetirarEspaco(row);

            if (!string.IsNullOrEmpty(row.Fone)) row.Fone = row.Fone.RetirarMascaraTelefone();
            if (!string.IsNullOrEmpty(row.Fax)) row.Fax = row.Fax.RetirarMascaraTelefone();
            if (!string.IsNullOrEmpty(row.Setor) && !Validacao.Validou(row.Setor, Validacao.Tipo.numerico)) return "Setor inválido.<br>O setor deve ter somente números.";

            if (!string.IsNullOrEmpty(row.Cnpj))
            {
                row.Cnpj = row.Cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
                if (!Validacao.ValidaCnpj(row.Cnpj)) return "CNPJ inválido.<br>O CNPJ está com valores incorretos.";
            }

            return string.Empty;
        }
    }

}
