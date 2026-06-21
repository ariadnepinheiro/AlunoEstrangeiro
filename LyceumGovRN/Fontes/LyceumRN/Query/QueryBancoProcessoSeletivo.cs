using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using Techne.Web;
using Techne.Data;
using System.Web.UI.WebControls;

namespace Techne.Lyceum.RN.Query
{
    public class QueryBancoProcessoSeletivo : LyceumQuery
    {
		public QueryBancoProcessoSeletivo()
            : base()
        {
            this.GridColumns.Add(new TSearchColumn() { FieldName = "banco", Caption = "Código", Width = Unit.Percentage(15) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "nome", Caption = "Nome", Width = Unit.Percentage(50) });

            this.Messages.KeyNotFound = "Banco não encontrado!";
            this.TextField = "banco";
            this.DescriptionField = "nome";
            this.ValueField = "";
            this.MaxLength = 3;
            this.GridFilterParameters.Add("banco", "Banco", TSearchDataType.Integer, 3);
            this.GridFilterParameters.Add("nome", "Nome", TSearchDataType.String, 100);
            this.MaxRows = 100;
            this.GridWidth = Unit.Pixel(800);
            this.TextFieldType = TSearchDataType.Integer;
        }

		/// <summary>
		/// DEFINIÇÃO: O banco usado em Processo Seletivo é o Banco Itaú S.A.
		/// </summary>
        public override System.Data.DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            ArrayList arPars = new ArrayList();
            QueryTable qt = null;

            int? banco = null;
            string nome = "";

            if (pars.ContainsKey("banco") && pars["banco"] != null && pars["banco"].ToString().Trim().Length > 0)
                banco = Convert.ToInt32(pars["banco"].ToString());

            if (pars.ContainsKey("nome") && pars["nome"] != null && pars["nome"].ToString().Trim().Length > 0)
                nome = pars["nome"].ToString().Trim();

            
            System.Text.StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT top " + maxRows + " BANCO, NOME from BANCOS where BANCO IN ('341','237')");

            if (key != null)
            {
                sql.Append(" AND BANCO = ?");
                arPars.Add(Convert.ToInt32(key.ToString()));
            }
            else
            {
                if (banco.HasValue)
                {
                    sql.Append(" AND BANCO = ?");
                    arPars.Add(banco);
                }
                if (!string.IsNullOrEmpty(nome))
                {
                    sql.Append(" AND NOME LIKE ?");
                    arPars.Add(LikeExpression(nome));
                }
            }

            sql.Append(" ORDER BY NOME ");

            qt = new QueryTable(sql.ToString());

            try
            {
                qt.Query(this.CreateConnection(), arPars.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            

            DataView dv = qt.DefaultView;
            return dv;
        }
    }
}
