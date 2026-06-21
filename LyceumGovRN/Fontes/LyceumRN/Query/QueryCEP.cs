using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Techne.Web;
using Techne.Data;
using System.Web.UI.WebControls;

namespace Techne.Lyceum.RN.Query
{
    public class QueryCEP : LyceumQuery
    {
        public QueryCEP()
            : base()
        {
            this.GridColumns.Add(new TSearchColumn() { FieldName = "cep", Caption = "CEP", Width = Unit.Percentage(15) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "nomeLogradouro", Caption = "Logradouro", Width = Unit.Percentage(30) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "nomeMunicipio", Caption = "Município", Width = Unit.Percentage(30) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "nomeBairro", Caption = "Bairro", Width = Unit.Percentage(15) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "uf", Caption = "Estado", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "codigoLogradouro", Caption = "codigoLogradouro", Width = Unit.Percentage(0), Visible=false });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "codigoMunicipio", Caption = "codigoMunicipio", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "codigoBairro", Caption = "codigoBairro", Width = Unit.Percentage(0), Visible = false });

            this.Messages.KeyNotFound = "CEP inválida";
            this.TextField = "";
            this.DescriptionField = "";
            this.ValueField = "";
            this.MaxLength = 8;
            this.DescriptionField = "";
            this.GridFilterParameters.Add("municipio", "Município", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("logradouro", "Logradouro", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("cep", "CEP", TSearchDataType.String, 8);
            this.MaxRows = 100;

            this.GridWidth = Unit.Pixel(800);
        }

        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            ArrayList arPars = new ArrayList();
            QueryTable qt = null;
            
            string logradouro = "";
            string municipio = "";
            string cep = "";

            if (pars.ContainsKey("logradouro") && pars["logradouro"] != null && pars["logradouro"].ToString().Trim().Length > 0)
                logradouro = pars["logradouro"].ToString().Trim();
            if (pars.ContainsKey("municipio") && pars["municipio"] != null && pars["municipio"].ToString().Trim().Length > 0)
                municipio = pars["municipio"].ToString().Trim();
            if (pars.ContainsKey("cep") && pars["cep"] != null && pars["cep"].ToString().Trim().Length > 0)
                cep = pars["cep"].ToString().Trim();
            
            System.Text.StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT top " + maxRows + " right(replicate('0',8)+ convert(varchar(8),TL.CEP),8)  cep, L.Codigo codigoLogradouro, L.Nome nomeLogradouro, M.codigo codigoMunicipio, ");
            sql.Append(" M.Nome nomeMunicipio, MB.codigo codigoBairro, MB.nome nomeBairro, M.uf_sigla as uf ");
            sql.Append(" from municipio M ");
            sql.Append(" inner join logradouro L ON L.municipio_codigo = M.codigo ");
            sql.Append(" inner join trecho_logradouro TL ON TL.logradouro_codigo = L.codigo ");
            sql.Append(" AND M.codigo = TL.logr_municipio_codigo ");
            sql.Append(" inner join mBairro MB ON MB.codigo = TL.MBairro_codigo ");
            sql.Append(" AND MB.MUNICIPIO_CODIGO = TL.LOGR_MUNICIPIO_CODIGO  ");
            sql.Append(" WHERE TL.CEP IS NOT NULL ");

            if (key != null)
            {
                sql.Append(" AND TL.CEP = ?");
                arPars.Add(key.ToString());
            }
            else
            {
                if (!string.IsNullOrEmpty(logradouro))
                {
                    sql.Append(" AND L.NOME LIKE ?");
                    arPars.Add(LikeExpression(logradouro));
                }
                if (!string.IsNullOrEmpty(municipio))
                {
                    sql.Append(" AND M.NOME LIKE ?");
                    arPars.Add(LikeExpression(municipio));
                }
                if (!string.IsNullOrEmpty(cep))
                {
                    sql.Append(" AND TL.CEP like ?");
                    arPars.Add(LikeExpression(cep.TrimStart('0',' ')));
                }
            }
            sql.Append(" ORDER BY TL.CEP, L.Codigo ");

            qt = new QueryTable(sql.ToString());
            qt.Query(this.CreateConnection(), arPars.ToArray());

            DataView dv = qt.DefaultView;
            return dv;

        }

    }
}
