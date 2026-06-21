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
    public class QueryTurmaTransferencia : LyceumQuery
    {
        public QueryTurmaTransferencia()
            : base()
        {
            this.GridColumns.Add(new TSearchColumn() { FieldName = "turma", Caption = "Turma", Width = Unit.Percentage(50) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "nome_comp", Caption = "Unidade de ensino", Width = Unit.Percentage(50) });
            //this.GridColumns.Add(new TSearchColumn() { FieldName = "nome_comp", Caption = "Unidade de ensino", Width = Unit.Percentage(50) });

            this.Messages.KeyNotFound = "Turma inválida";
            this.TextField = "turma";
            this.MaxLength = 20;
            //this.DescriptionField = "";
            this.GridFilterParameters.Add("turma", "Turma", TSearchDataType.String, 20);
            this.GridFilterParameters.Add(new TSearchParameter() { Caption = "Unidade Ensino", ParameterName = "uni_ensino", ShowInFilterPanel = true, ParameterType = TSearchDataType.String });
            this.GridFilterParameters.Add(new TSearchParameter() { Caption = "Disciplina", ParameterName = "disciplina", ShowInFilterPanel = false, ParameterType = TSearchDataType.String });
            this.GridFilterParameters.Add(new TSearchParameter() { Caption = "Ano", ParameterName = "ano", ShowInFilterPanel = false, ParameterType = TSearchDataType.String });
            this.GridFilterParameters.Add(new TSearchParameter() { Caption = "Semestre", ParameterName = "semestre", ShowInFilterPanel = false, ParameterType = TSearchDataType.String });
            this.GridFilterParameters.Add(new TSearchParameter() { Caption = "Curso", ParameterName = "curso", ShowInFilterPanel = false, ParameterType = TSearchDataType.String });
            this.MaxRows = 100;


            this.GridWidth = Unit.Pixel(800);
        }

        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            ArrayList parValues = new ArrayList();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT distinct top " + maxRows + " t.turma, uni.nome_comp ");
            sql.Append("FROM  ");
            sql.Append("ly_turma t inner join LY_UNIDADE_ENSINO uni ON t.UNIDADE_RESPONSAVEL = uni.UNIDADE_ENS ");
            sql.Append(" WHERE t.sit_turma = 'Aberta' ");


            if (key != null)
            {
                sql.Append(" and t.turma = ? ");
                parValues.Add(key.ToString());
            }
            else
            {
                if (pars.ContainsKey("turma") && pars["turma"] != null && pars["turma"].ToString().Trim().Length > 0)
                {
                    sql.Append(" and t.turma like ? ");
                    parValues.Add(LikeExpression(pars["turma"].ToString()));
                }
            }

            if (pars.ContainsKey("uni_ensino") && pars["uni_ensino"] != null && pars["uni_ensino"].ToString().Trim().Length > 0)
            {
                sql.Append(" and uni.nome_comp like ? ");
                parValues.Add("%" + LikeExpression(pars["uni_ensino"].ToString()));
            }

            if (pars.ContainsKey("disciplina") && pars["disciplina"] != null && pars["disciplina"].ToString().Trim().Length > 0)
            {
                sql.Append(" and t.disciplina = ? ");
                parValues.Add(pars["disciplina"].ToString());
            }

            if (pars.ContainsKey("ano") && pars["ano"] != null && pars["ano"].ToString().Trim().Length > 0)
            {
                sql.Append(" and t.ano = ? ");
                parValues.Add(pars["ano"].ToString());
            }

            if (pars.ContainsKey("semestre") && pars["semestre"] != null && pars["semestre"].ToString().Trim().Length > 0)
            {
                sql.Append(" and t.semestre = ? ");
                parValues.Add(pars["semestre"].ToString());
            }

            if (pars.ContainsKey("curso") && pars["curso"] != null && pars["curso"].ToString().Trim().Length > 0)
            {
                sql.Append(" and t.curso = ? ");
                parValues.Add(pars["curso"].ToString());
            }

            QueryTable qt = new QueryTable(sql.ToString());
            qt.Query(this.CreateConnection(), parValues.ToArray());
            DataView dv = qt.DefaultView;
            //dv.Sort = "t.faculdade asc";
            return dv;
        }
    }
}
