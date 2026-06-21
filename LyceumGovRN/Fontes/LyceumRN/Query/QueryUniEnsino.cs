using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Data;
using Techne.Web;
using Techne.Data;
using System.Web.UI.WebControls;

namespace Techne.Lyceum.RN.Query
{
    public class QueryUniEnsino : LyceumQuery
    {
        public QueryUniEnsino()
            : base()
        {
            this.GridColumns.Add(new TSearchColumn() { FieldName = "unidade_ens", Caption = "Censo", Width = Unit.Percentage(50) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "nome_comp", Caption = "Unidade de ensino", Width = Unit.Percentage(50) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "setor", Caption = "setor", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "cgc", Caption = "cgc", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "situacao", Caption = "situacao", Width = Unit.Percentage(0), Visible = false });

            this.Messages.KeyNotFound = "Unidade de ensino inválida";
            this.TextField = "unidade_ens";
            this.MaxLength = 20;
            this.DescriptionField = "nome_comp";
            this.GridFilterParameters.Add("unidade_ens", "Censo", TSearchDataType.String, 20);
            this.GridFilterParameters.Add("nome_comp", "Unidade de ensino", TSearchDataType.String, 20);
            this.MaxRows = 100;

            this.GridWidth = Unit.Pixel(800);
        }

        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            ArrayList parValues = new ArrayList();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT distinct top " + maxRows + " unidade_ens, nome_comp, setor, cgc, situacao ");
            sql.Append("FROM  ");
            sql.Append("LY_UNIDADE_ENSINO ");
            sql.Append(" WHERE unidade_ens is not null ");


            if (key != null)
            {
                sql.Append(" and unidade_ens = ? ");
                parValues.Add(key.ToString());
            }
            else
            {
                if (pars.ContainsKey("unidade_ens") && pars["unidade_ens"] != null && pars["unidade_ens"].ToString().Trim().Length > 0)
                {
                    sql.Append(" and unidade_ens like ? ");
                    parValues.Add("%" + LikeExpression(pars["unidade_ens"].ToString()));
                }
            }

            if (pars.ContainsKey("nome_comp") && pars["nome_comp"] != null && pars["nome_comp"].ToString().Trim().Length > 0)
            {
                sql.Append(" and nome_comp like ? ");
                parValues.Add(LikeExpression(pars["nome_comp"].ToString()));
            }

            QueryTable qt = new QueryTable(sql.ToString());
            qt.Query(this.CreateConnection(), parValues.ToArray());
            DataView dv = qt.DefaultView;
            dv.Sort = "nome_comp asc";
            return dv;
        }
    }
}