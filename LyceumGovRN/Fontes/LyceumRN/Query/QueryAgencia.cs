using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using Techne.Web;
using Techne.Data;
using System.Web.UI.WebControls;

namespace Techne.Lyceum.RN.Query
{
    public class QueryAgencia: LyceumQuery
    {
        public QueryAgencia(): base()
        {
            this.GridColumns.Add(new TSearchColumn() { FieldName = "agencia", Caption = "Agência", Width = Unit.Percentage(15) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "banco", Caption = "Banco", Width = Unit.Percentage(15) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "nome", Caption = "Nome", Width = Unit.Percentage(50) });

            this.Messages.KeyNotFound = "Agencia não encontrada!";
            this.TextField = "agencia";
            this.DescriptionField = "nome";
            this.ValueField = "";
            this.MaxLength = 8;
            this.GridFilterParameters.Add("agencia", "Agência", TSearchDataType.String, 20);
            this.GridFilterParameters.Add(new TSearchParameter() { ShowInFilterPanel = false, ParameterName = "banco", ParameterType = TSearchDataType.Integer });
            this.GridFilterParameters.Add("nome", "Nome da Agência", TSearchDataType.String, 100);
            this.MaxRows = 100;
            this.GridWidth = Unit.Pixel(800);
        }

        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            ArrayList arPars = new ArrayList();
            QueryTable qt = null;

            int? banco = null;
            string nome = "";
            string agencia = "";

            if (pars.ContainsKey("banco") && pars["banco"] != null && pars["banco"].ToString().Trim().Length > 0)
                banco = Convert.ToInt32(pars["banco"].ToString());

            if (pars.ContainsKey("nome") && pars["nome"] != null && pars["nome"].ToString().Trim().Length > 0)
                nome = pars["nome"].ToString().Trim();

            if (pars.ContainsKey("agencia") && pars["agencia"] != null && pars["agencia"].ToString().Trim().Length > 0)
                agencia = pars["agencia"].ToString().Trim();


            System.Text.StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT top " + maxRows + " AGENCIA, BANCO, NOME from AGENCIAS WHERE 1=1");

            if (key != null)
            {
                sql.Append(" AND AGENCIA = ?");
                arPars.Add(key.ToString());

                if (banco.HasValue)
                {
                    sql.Append(" AND BANCO = ?");
                    arPars.Add(banco);
                }
            }
            else
            {
                if (banco.HasValue)
                {
                    sql.Append(" AND BANCO = ?");
                    arPars.Add(banco);
                }
                if (!string.IsNullOrEmpty(nome))
                {
                    sql.Append(" AND NOME LIKE ?");
                    arPars.Add(LikeExpression(nome));
                }
                if (!string.IsNullOrEmpty(agencia))
                {
                    sql.Append(" AND AGENCIA LIKE ?");
                    arPars.Add(LikeExpression(agencia));
                }
            }

            sql.Append(" ORDER BY NOME ");

            qt = new QueryTable(sql.ToString());

            try
            {
                qt.Query(this.CreateConnection(), arPars.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }


            DataView dv = qt.DefaultView;
            return dv;
        }
    }
}
