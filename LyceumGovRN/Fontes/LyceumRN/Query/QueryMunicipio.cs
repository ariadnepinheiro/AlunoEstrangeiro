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
    public class QueryMunicipio : LyceumQuery
    {
        public QueryMunicipio()
        {
            this.GridColumns.Add(new TSearchColumn() { FieldName = "codigo", Caption = "Código", Width = Unit.Percentage(12) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "nome", Caption = "Município", Width = Unit.Percentage(28) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "uf_sigla", Caption = "Estado", Width = Unit.Percentage(28) });

            this.Messages.KeyNotFound = "Código inválido.";
            this.TextField = "codigo";
            this.DescriptionField = "nome";
            this.GridFilterParameters.Add("nome", "nome", TSearchDataType.String, 100);
            this.GridFilterParameters.Add(new TSearchParameter() { ParameterName = "codigo", Caption = "Código", ParameterType = TSearchDataType.Integer, MaxLength = 8 });
            this.MaxRows = 100;
            this.GridWidth = Unit.Pixel(500);
            this.TextFieldType = TSearchDataType.Integer;
            this.MaxLength = 8;
        }

        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            ArrayList parValues = new ArrayList();
            StringBuilder sql = new StringBuilder();

            sql.Append(string.Format(
                           @"SELECT TOP {0} codigo, nome, uf_sigla from municipio where 1=1 ",
                            maxRows));

            if (this.HasValue(pars, "nome"))
            {
                sql.Append(" and nome like ? ");
                parValues.Add(LikeExpression("%" + pars["nome"].ToString()));
            }


            if (key != null)
            {
                string codigo = string.Empty;
                try
                {
                    codigo = key.ToString();
                }
                catch { }
                sql.Append(" and codigo = ? ");
                parValues.Add(codigo);
            }

            if (this.HasValue(pars, "codigo"))
            {
                sql.Append(" and codigo like ? ");
                parValues.Add(LikeExpression(pars["codigo"].ToString()));
            }

            QueryTable qt = new QueryTable(sql.ToString());
            qt.Query(this.CreateConnection(), parValues.ToArray());
            DataView dv = qt.DefaultView;
            dv.Sort = "nome asc";
            return dv;

        }

    }
}
