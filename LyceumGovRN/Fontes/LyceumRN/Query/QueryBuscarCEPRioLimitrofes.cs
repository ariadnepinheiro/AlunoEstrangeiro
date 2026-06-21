namespace Techne.Lyceum.RN.Query
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Web.UI.WebControls;
    using Seeduc.Infra.Data;
    using Techne.Web;

    public class QueryBuscarCEPRioLimitrofes : LyceumQuery
    {
        public QueryBuscarCEPRioLimitrofes()
        {
            this.GridColumns.Add(new TSearchColumn { FieldName = "cep", Caption = "CEP", Width = Unit.Percentage(15) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "logradouro", Caption = "Logradouro", Width = Unit.Percentage(50) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "municipio", Caption = "Município", Width = Unit.Percentage(35) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "uf", Caption = "Estado", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "id_logradouro", Caption = "codigoLogradouro", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn { FieldName = "id_municipio", Caption = "codigoMunicipio", Width = Unit.Percentage(0), Visible = false });

            this.Messages.KeyNotFound = "CEP inválido";
            this.TextField = string.Empty;
            this.DescriptionField = string.Empty;
            this.ValueField = string.Empty;
            this.MaxLength = 8;
            this.DescriptionField = string.Empty;
            this.GridFilterParameters.Add("municipio", "Município", TSearchDataType.String, 50);
            this.GridFilterParameters.Add("logradouro", "Logradouro", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("cep", "CEP", TSearchDataType.String, 8);
            this.MaxRows = 100;

            this.GridWidth = Unit.Pixel(800);
        }

        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            var logradouro = string.Empty;
            var municipio = string.Empty;
            var cep = string.Empty;

            if (pars.ContainsKey("logradouro")
                && pars["logradouro"] != null
                && pars["logradouro"].ToString().Trim().Length > 0)
            {
                logradouro = pars["logradouro"].ToString().Replace("'", "''");
            }

            if (pars.ContainsKey("municipio")
                && pars["municipio"] != null
                && pars["municipio"].ToString().Trim().Length > 0)
            {
                municipio = pars["municipio"].ToString().Trim().Replace("'", "''");
            }

            if (pars.ContainsKey("cep")
                && pars["cep"] != null
                && pars["cep"].ToString().Trim().Length > 0)
            {
                cep = pars["cep"].ToString().Trim().Replace("'", "''");
            }

            var sql = new StringBuilder();

            sql.AppendFormat(
                @"SELECT TOP {0}
                        l.CEP,
                        l.NOME AS 'LOGRADOURO',
                        m1.NOME AS 'MUNICIPIO',
                        m1.UF,
                        l.ID_LOGRADOURO,
                        m1.ID_MUNICIPIO,
                        L.BAIRRO
                FROM    dbo.TCE_LOGRADOURO l
                        INNER JOIN dbo.TCE_MUNICIPIO m1 ON l.ID_MUNICIPIO = m1.ID_MUNICIPIO
                WHERE   (
                          m1.UF IN ('RJ','MG')
                          OR EXISTS ( SELECT    1
                                      FROM      LYCEUM..TCE_MUNICIPIO_LIMITROFE ml
                                                INNER JOIN dbo.TCE_MUNICIPIO m2 ON ml.CODIGO_MUNICIPIO = m2.ID_MUNICIPIO
                                      WHERE     m1.ID_MUNICIPIO = m2.ID_MUNICIPIO
                                                AND m2.UF = 'RJ' )
                          OR EXISTS ( SELECT    1
                                      FROM      LYCEUM..TCE_MUNICIPIO_LIMITROFE ml
                                                INNER JOIN dbo.TCE_MUNICIPIO m2 ON ml.CODIGO_MUNICIPIO_LIMITROFE = m2.ID_MUNICIPIO
                                      WHERE     m1.ID_MUNICIPIO = m2.ID_MUNICIPIO
                                                AND m2.UF = 'RJ' )
                        ) ",
                maxRows);

            if (key != null)
            {
                sql.AppendFormat(" AND l.CEP = '{0}'", cep);
            }
            else
            {
                var parameters = new List<string>();

                if (!string.IsNullOrEmpty(logradouro))
                {
                    parameters.Add(string.Format("l.NOME LIKE '%{0}%'", logradouro));
                }

                if (!string.IsNullOrEmpty(municipio))
                {
                    parameters.Add(string.Format("m1.NOME LIKE '%{0}%'", municipio));
                }

                if (!string.IsNullOrEmpty(cep))
                {
                    if (cep.Length == 8)
                    {
                        parameters.Add(string.Format("l.CEP = '{0}'", cep));
                    }
                    else
                    {
                        parameters.Add(string.Format("l.CEP LIKE '%{0}%'", cep));
                    }
                }

                if (parameters.Count > 0)
                {
                    sql.Append(" AND ");
                    sql.Append(parameters.Aggregate((x, y) => string.Format("{0} AND {1}", x, y)));
                }
            }

            sql.Append(" ORDER BY l.CEP, l.NOME, m1.NOME");

            using (var ctx = DataContextBuilder.FromHades.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(sql.ToString());
                var dataTable = ctx.GetDataTable(contextQuery);

                return dataTable.DefaultView;
            }
        }
    }
}
