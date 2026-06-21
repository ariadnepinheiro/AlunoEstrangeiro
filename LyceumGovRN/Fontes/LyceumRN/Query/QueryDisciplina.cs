using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Techne.Web;
using System.Web.UI.WebControls;
using System.Collections;
using Techne.Data;

namespace Techne.Lyceum.RN.Query
{
    public class QueryDisciplina : LyceumQuery
    {
        public QueryDisciplina()
            : base()
        {
            this.GridColumns.Add(new TSearchColumn() { FieldName = "disciplina", Caption = "Disciplina", Width = Unit.Percentage(25) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "nome", Caption = "Nome", Width = Unit.Percentage(75) });
            
            this.Messages.KeyNotFound = "Disciplina inválida";
            this.TextField = "disciplina";
            this.MaxLength = 20;
            this.DescriptionField = "nome";
            this.GridFilterParameters.Add("disciplina", "Disciplina", TSearchDataType.String, 25);
            this.GridFilterParameters.Add("nome", "Nome", TSearchDataType.String, 75);
            this.MaxRows = 100;

            this.GridWidth = Unit.Pixel(800);
        }

        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            ArrayList parValues = new ArrayList();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT distinct top " + maxRows + " d.disciplina, d.nome ");
            sql.Append("FROM ly_disciplina d ");
            sql.Append("WHERE d.disciplina IS NOT NULL ");

            if (key != null)
            {
                sql.Append(" AND d.disciplina = ? ");
                parValues.Add(key.ToString());
            }
            else
            {
                if (pars.ContainsKey("disciplina") && pars["disciplina"] != null && pars["disciplina"].ToString().Trim().Length > 0)
                {
                    sql.Append(" AND d.disciplina like ? ");
                    parValues.Add(LikeExpression(pars["disciplina"].ToString()));
                }

                if (pars.ContainsKey("nome") && pars["nome"] != null && pars["nome"].ToString().Trim().Length > 0)
                {
                    sql.Append(" AND d.nome like ? ");
                    parValues.Add(LikeExpression(pars["nome"].ToString()));
                }
            }
            
            QueryTable qt = new QueryTable(sql.ToString());
            qt.Query(this.CreateConnection(), parValues.ToArray());
            DataView dv = qt.DefaultView;
            dv.Sort = "disciplina asc, nome asc";
            return dv;
        }
    }
}
