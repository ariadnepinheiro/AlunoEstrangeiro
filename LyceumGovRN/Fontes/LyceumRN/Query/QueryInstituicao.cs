using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Techne.Data;
using Techne.Lyceum.RN.Util;
using Techne.Web;


namespace Techne.Lyceum.RN.Query
{
    public class QueryInstituicao : LyceumQuery
    {
        public QueryInstituicao()
        {
            this.GridColumns.Add(new TSearchColumn() { FieldName = "outra_faculdade", Caption = "Código", Width = Unit.Percentage(12) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "nome_comp", Caption = "Nome", Width = Unit.Percentage(28) });

            this.Messages.KeyNotFound = "Código inválido.";
            this.TextField = "outra_faculdade";
            this.DescriptionField = "nome_comp";
            this.GridFilterParameters.Add("nome_comp", "Nome", TSearchDataType.String, 100);
            this.GridFilterParameters.Add(new TSearchParameter() { ParameterName = "outra_faculdade", Caption = "Código", ParameterType = TSearchDataType.Integer, MaxLength = 10 });
            this.MaxRows = 100;
            this.GridWidth = Unit.Pixel(800);
            this.TextFieldType = TSearchDataType.Integer;
            this.MaxLength = 10;
        }

        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            ArrayList parValues = new ArrayList();
            StringBuilder sql = new StringBuilder();

            sql.Append(string.Format(
                           @"SELECT TOP {0} outra_faculdade, nome_comp from ly_instituicao where 1=1 ",
                            maxRows));

            if (this.HasValue(pars, "nome_comp"))
            {
                sql.Append(" and nome_comp like ? ");
                parValues.Add( LikeExpression("%" + pars["nome_comp"].ToString()));
            }


            if (key != null)
            {
                string instituicao = string.Empty;
                try
                {
                    instituicao = key.ToString();
                }
                catch { }
                sql.Append(" and outra_faculdade = ? ");
                parValues.Add(instituicao);
            }

            if (this.HasValue(pars, "outra_faculdade"))
            {
                sql.Append(" and outra_faculdade like ? ");
                parValues.Add(LikeExpression(pars["outra_faculdade"].ToString()));
            }


            if (pars.ContainsKey("TIPO_ORIGEM") && pars["TIPO_ORIGEM"] != null && pars["TIPO_ORIGEM"].ToString().Trim() != "Selecione")
            {
                sql.Append(" and TIPO_ORIGEM like ? ");
                parValues.Add(LikeExpression(pars["TIPO_ORIGEM"].ToString()));
            }

            QueryTable qt = new QueryTable(sql.ToString());
            qt.Query(this.CreateConnection(), parValues.ToArray());
            DataView dv = qt.DefaultView;
            dv.Sort = "nome_comp asc";
            return dv;

        }

    }
}
