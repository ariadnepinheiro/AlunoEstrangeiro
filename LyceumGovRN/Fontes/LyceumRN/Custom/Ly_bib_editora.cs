using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.CR;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Custom
{
    class Ly_bib_editoraCustom : Techne.Lyceum.CR.Ly_bib_editora.CustomBase
    {
        public override string PreInsert(Ly_bib_editora.Row row, TConnectionWritable cn)
        {
            string erro = VerificaSigla(row.Sigla_editora);
            if (!string.IsNullOrEmpty(erro))
                return erro;

            return base.PreInsert(row, cn);
        }

        public override string PreUpdate(Ly_bib_editora.Row row, TConnectionWritable cn)
        {
            string erro = VerificaSigla(row.Sigla_editora);
            if (!string.IsNullOrEmpty(erro))
                return erro;

            return base.PreUpdate(row, cn);
        }

        private string VerificaSigla(string sigla)
        {
            TConnection connection = Techne.Lyceum.Config.CreateConnection();
            connection.Open();
            QueryTable qt = null;

            try
            {
                qt = new QueryTable("select sigla_editora from LY_BIB_EDITORA where sigla_editora=?");
                qt.Query(connection, sigla);
                if (qt.Rows.Count > 0)
                {
                    return "Já existe uma Sigla com o mesmo nome cadastrada.";
                }
            }
            finally
            {
                connection.Close();
            }
            return "";
        }
    }
}
