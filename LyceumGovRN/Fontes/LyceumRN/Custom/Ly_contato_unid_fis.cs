using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.CR;
using Techne.Data;

namespace Techne.Lyceum.RN.Custom
{
    public class Ly_contato_unid_fisCustom : Ly_contato_unid_fis.CustomBase
    {
        public override string PreInsert(Ly_contato_unid_fis.Row row, TConnectionWritable cn)
        {
            row.Chave = RN.UnidadeFisica.ConsultarUltimaChaveContato(row.Unidade_fis);

            row.Fone = row.Fone.Replace("(", "").Replace(")", "").Replace("-", "");

            return string.Empty;
        }

    }
}
