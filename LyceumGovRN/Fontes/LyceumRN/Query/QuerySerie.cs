using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Web;
using System.Web.UI.WebControls;
using System.Collections;
using Techne.Data;
using System.Data;

namespace Techne.Lyceum.RN.Query
{
    public class QuerySerie : LyceumQuery
    {
        public QuerySerie() : base()
        {
            this.GridColumns.Add(new TSearchColumn() { FieldName = "curso", Caption = "Curso", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "turno", Caption = "Turno", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "serie", Caption = "Ano de Escolaridade", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "cursoDescricao", Caption = "Escolaridade", Width = Unit.Percentage(25) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "turnoDescricao", Caption = "Turno", Width = Unit.Percentage(25) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "curriculo", Caption = "Matriz Curricular", Width = Unit.Percentage(25)});
            this.GridColumns.Add(new TSearchColumn() { FieldName = "serieDescricao", Caption = "Ano de Escolaridade", Width = Unit.Percentage(25)});

            this.Messages.KeyNotFound = "Ano escolar inválido.";
            this.TextField = "serie";
            this.DescriptionField = "serieDescricao";

            this.GridFilterParameters.Add(new TSearchParameter() { Caption = "Ano", ParameterName = "ano", ShowInFilterPanel = false, ParameterType = TSearchDataType.String });
            this.GridFilterParameters.Add(new TSearchParameter() { Caption = "Periodo", ParameterName = "periodo", ShowInFilterPanel = false, ParameterType = TSearchDataType.String });
            this.GridFilterParameters.Add(new TSearchParameter() { Caption = "Escolaridade", ParameterName = "cursoDescricao", ShowInFilterPanel = false, ParameterType = TSearchDataType.String });
            this.GridFilterParameters.Add(new TSearchParameter() { Caption = "Turno", ParameterName = "turnoDescricao", ShowInFilterPanel = false, ParameterType = TSearchDataType.String });
            this.GridFilterParameters.Add(new TSearchParameter() { Caption = "Curso", ParameterName = "curso", ShowInFilterPanel = false, ParameterType = TSearchDataType.String});
            this.GridFilterParameters.Add(new TSearchParameter() { Caption = "Turno", ParameterName = "turno", ShowInFilterPanel = false, ParameterType = TSearchDataType.String });
            
            this.MaxRows = 100;
            this.MaxLength = 100;
            this.GridWidth = Unit.Pixel(800);
        }


        /// <summary>
        /// Método de query para a TSearch. Estou obrigando informar curso e turno.
        /// </summary>
        /// <param name="pars"></param>
        /// <param name="key"></param>
        /// <param name="maxRows"></param>
        /// <returns></returns>
        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            ArrayList parValues = new ArrayList();
            StringBuilder sql = new StringBuilder();
            sql.Append("select distinct top " + maxRows + " c.curso, c.NOME cursoDescricao, t.turno, t.DESCRICAO turnoDescricao, s.serie, s.DESCRICAO serieDescricao, s.curriculo ");
            sql.Append("from LY_SERIE s ");
            sql.Append("inner join LY_CURSO c on c.CURSO = s.CURSO ");
            sql.Append("inner join LY_TURNO t on t.TURNO = s.TURNO ");
            sql.Append("inner join LY_CURRICULO cur on cur.CURRICULO = s.CURRICULO and cur.CURSO = s.CURSO and cur.TURNO = s.TURNO ");
            sql.Append(" WHERE 1 = 1 ");


            if (key != null)
            {
                sql.Append(" and s.serie = ? ");
                parValues.Add(key.ToString());
            }
            else
            {
                if (pars.ContainsKey("serie") && pars["serie"] != null && pars["serie"].ToString().Trim().Length > 0)
                {
                    sql.Append(" and s.serie like ? ");
                    parValues.Add(LikeExpression(pars["serie"].ToString()));
                }
            }

            if (pars.ContainsKey("ano") && pars["ano"] != null && pars["ano"].ToString().Trim().Length > 0)
            {
                sql.Append(" and cur.ANO_INI = ? ");
                parValues.Add(pars["ano"].ToString());
            }

            if (pars.ContainsKey("periodo") && pars["periodo"] != null && pars["periodo"].ToString().Trim().Length > 0)
            {
                sql.Append(" and cur.SEM_INI = ? ");
                parValues.Add(pars["periodo"].ToString());
            }

            if (pars.ContainsKey("curso") && pars["curso"] != null && pars["curso"].ToString().Trim().Length > 0)
            {
                sql.Append(" and s.curso = ? ");
                parValues.Add(pars["curso"].ToString());
            }
            else
            {
                sql.Append(" and s.curso = null ");
            }

            if (pars.ContainsKey("cursoDescricao") && pars["cursoDescricao"] != null && pars["cursoDescricao"].ToString().Trim().Length > 0)
            {
                sql.Append(" and c.NOME like ? ");
                parValues.Add(LikeExpression(pars["cursoDescricao"].ToString()));
            }

            if (pars.ContainsKey("serieDesc") && pars["serieDesc"] != null && pars["serieDesc"].ToString().Trim().Length > 0)
            {
                sql.Append(" and s.DESCRICAO like ? ");
                parValues.Add(LikeExpression(pars["serieDesc"].ToString()));
            }

            if (pars.ContainsKey("turno") && pars["turno"] != null && pars["turno"].ToString().Trim().Length > 0)
            {
                sql.Append(" and s.turno = ? ");
                parValues.Add(pars["turno"].ToString());
            }
            else
            {
                sql.Append(" and s.turno = null ");
            }

            QueryTable qt = new QueryTable(sql.ToString());
            qt.Query(this.CreateConnection(), parValues.ToArray());
            DataView dv = qt.DefaultView;
            return dv;
        }

    }
}
