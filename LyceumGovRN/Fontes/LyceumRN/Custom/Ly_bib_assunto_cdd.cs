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
    class Ly_bib_assunto_cddCustom : Techne.Lyceum.CR.Ly_bib_assunto_cdd.CustomBase
    {
        public override string PreInsert(Ly_bib_assunto_cdd.Row row, TConnectionWritable cn)
        {
            string erro = VerificaClassificacao(row.Codigo_cdd);
            if (!string.IsNullOrEmpty(erro))
                return erro;

            return base.PreInsert(row, cn);
        }

        public override string PreUpdate(Ly_bib_assunto_cdd.Row row, TConnectionWritable cn)
        {
            string erro = VerificaClassificacao_Update(row.Codigo_cdd, row.Id);
            if (!string.IsNullOrEmpty(erro))
                return erro;

            return base.PreUpdate(row, cn);
        }

        private string VerificaClassificacao(string classificacao)
        {
            TConnection connection = Techne.Lyceum.Config.CreateConnection();
            connection.Open();
            QueryTable qt = null;

            try
            {
                qt = new QueryTable("select codigo_cdd from LY_BIB_ASSUNTO_CDD where codigo_cdd=?");
                qt.Query(connection, classificacao);
                if (qt.Rows.Count > 0)
                {
                    return "Já existe uma Classificação CDD com a mesma numeração cadastrada.";
                }
            }
            finally
            {
                connection.Close();
            }
            return "";
        }

        private string VerificaClassificacao_Update(string classificacao, decimal? id)
        {
            TConnection connection = Techne.Lyceum.Config.CreateConnection();
            connection.Open();
            QueryTable qt = null;

            try
            {
                qt = new QueryTable("select codigo_cdd from LY_BIB_ASSUNTO_CDD where codigo_cdd=? and id<>?");
                qt.Query(connection, classificacao, id);
                if (qt.Rows.Count > 0)
                {
                    return "Já existe uma Classificação CDD com a mesma numeração cadastrada.";
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
