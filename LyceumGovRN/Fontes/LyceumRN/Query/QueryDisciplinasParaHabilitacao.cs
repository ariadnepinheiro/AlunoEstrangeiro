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
    public class QueryDisciplinasParaHabilitacao : LyceumQuery
    {
        public QueryDisciplinasParaHabilitacao()
            : base()
        {
            this.GridColumns.Add(new TSearchColumn() { FieldName = "agrupamento", Caption = "Agrupamento", Width = Unit.Percentage(25) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "descricao", Caption = "Descrição", Width = Unit.Percentage(75) });
            
            
            this.Messages.KeyNotFound = "Grupo de disciplinas inválido";
            this.TextField = "agrupamento";           
            this.DescriptionField = "descricao";

            this.GridFilterParameters.Add("agrupamento", "Agrupamento", TSearchDataType.String, 25);
            this.GridFilterParameters.Add("descricao", "Descrição", TSearchDataType.String, 75);
            
            this.MaxRows = 100;
            this.MaxLength = 20;
            this.GridWidth = Unit.Pixel(800);
        }

        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            ArrayList parValues = new ArrayList();
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT distinct top " + maxRows + " gh.AGRUPAMENTO, gh.DESCRICAO ");
            sql.Append(" from LY_GRUPO_HABILITACAO gh ");
            sql.Append("WHERE gh.DESCRICAO IS NOT NULL AND ATIVO='S' ");
            if (key != null)
            {
                sql.Append(" AND gh.AGRUPAMENTO = ? ");
                parValues.Add(key.ToString());
            }
            else
            {
                if (pars.ContainsKey("matricula") && pars["matricula"] != null && pars["matricula"].ToString().Trim().Length > 0)
                {
                    sql.Append(" and AGRUPAMENTO not in (");
                    sql.Append(" select AGRUPAMENTO from ly_grupo_habilitacao_doc g ");
                    sql.Append(" inner join LY_DOCENTE d ");
                    sql.Append(" on g.NUM_FUNC = d.NUM_FUNC ");
                    sql.Append(" where MATRICULA = ? ) ");

                    parValues.Add(pars["matricula"].ToString());
                }
                if (pars.ContainsKey("agrupamento") && pars["agrupamento"] != null && pars["agrupamento"].ToString().Trim().Length > 0)
                {
                    sql.Append(" AND gh.AGRUPAMENTO like ? ");
                    parValues.Add(LikeExpression(pars["agrupamento"].ToString()));
                }
                if (pars.ContainsKey("descricao") && pars["descricao"] != null && pars["descricao"].ToString().Trim().Length > 0)
                {
                    sql.Append(" AND gh.DESCRICAO like ? ");
                    parValues.Add(LikeExpression(pars["descricao"].ToString()));
                }               
            }
            
            QueryTable qt = new QueryTable(sql.ToString());
            qt.Query(this.CreateConnection(), parValues.ToArray());
            DataView dv = qt.DefaultView;
            dv.Sort = "DESCRICAO asc";
            return dv;
        }
    }
}
