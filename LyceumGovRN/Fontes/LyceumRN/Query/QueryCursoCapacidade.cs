using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Techne.Data;
using Techne.Web;

namespace Techne.Lyceum.RN.Query
{
    class QueryCursoCapacidade : LyceumQuery
    {
        public QueryCursoCapacidade()
        {
            this.GridColumns.Add(new TSearchColumn() { FieldName = "curso", Caption = "Código", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "nome", Caption = "Curso", Width = Unit.Percentage(90) });

            this.Messages.KeyNotFound = "Curso não cadastrado.";
            this.TextField = "curso";
            this.DescriptionField = "nome";
            this.GridFilterParameters.Add("curso", "Código", TSearchDataType.String, 8);
            this.GridFilterParameters.Add("nome", "Curso", TSearchDataType.String, 100);

            this.MaxRows = 100;
            this.MaxLength = 100;
            this.GridWidth = Unit.Pixel(800);
        }

        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            ArrayList parValues = new ArrayList();
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT distinct top " + maxRows.ToString() + " c.curso as curso, nome,mc.DESCRICAO AS modalidade,tc.DESCRICAO AS segmento ");
            sql.Append(" FROM LY_CURSO C INNER JOIN LY_TIPO_CURSO TC ON C.TIPO=TC.TIPO ");
            sql.Append(" INNER JOIN LY_MODALIDADE_CURSO MC ON C.MODALIDADE = MC.MODALIDADE INNER JOIN LY_TURMA T ON C.CURSO = t.CURSO ");
            sql.Append(" where 1=1 ");
            
            if (key != null)
            {
                sql.Append(" and c.curso= ? ");
                parValues.Add(key);
            }
            else if (pars.ContainsKey("curso") && pars["curso"] != null && pars["curso"].ToString().Trim().Length > 0)
            {
                sql.Append(" and convert(varchar,c.curso) like ? ");
                parValues.Add(LikeExpression(pars["curso"].ToString()));
            }

            if (pars.ContainsKey("nome") && pars["nome"] != null && pars["nome"].ToString().Trim().Length > 0)
            {
                sql.Append(" and nome like ? ");
                parValues.Add(LikeExpression(pars["nome"].ToString()));
            }

            if (pars.ContainsKey("modalidade") && pars["modalidade"] != null && pars["modalidade"].ToString().Trim().Length > 0)
            {
                sql.Append(" and mc.modalidade = ? ");
                parValues.Add(pars["modalidade"]);
            }
            if (pars.ContainsKey("tipo") && pars["tipo"] != null && pars["tipo"].ToString().Trim().Length > 0)
            {
                sql.Append(" and tc.tipo= ? ");
                parValues.Add(pars["tipo"]);
            }


            if ((pars.ContainsKey("ano") && pars["ano"] != null && pars["ano"].ToString().Trim().Length > 0) && (pars.ContainsKey("periodo") && pars["periodo"] != null && pars["periodo"].ToString().Trim().Length > 0))
            {
                sql.Append(" and c.curso not in (select CURSOID from CAPACIDADEALUNOTURMA where ano = ? and periodo = ?)");
                parValues.Add(pars["ano"]);
                parValues.Add(pars["periodo"]);
                
            }
           

            QueryTable qt = new QueryTable(sql.ToString());
            qt.Query(this.CreateConnection(), parValues.ToArray());
            DataView dv = qt.DefaultView;
            dv.Sort = "nome asc";
            return dv;
        }
    }
}
