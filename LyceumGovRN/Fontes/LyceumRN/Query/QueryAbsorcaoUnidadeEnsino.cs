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
	public class QueryAbsorcaoUnidadeEnsino : LyceumQuery
	{

		public QueryAbsorcaoUnidadeEnsino()
		{
			this.GridColumns.Add(new TSearchColumn() { FieldName = "unidade_ens", Caption = "Código", Width = Unit.Percentage(10) });
			this.GridColumns.Add(new TSearchColumn() { FieldName = "nome_comp", Caption = "Descrição", Width = Unit.Percentage(24) });

			this.Messages.KeyNotFound = "Não existe unidades de destino";
			this.TextField = "unidade_ens";
			this.DescriptionField = "nome_comp";
			this.GridFilterParameters.Add("unidade_ens", "Código", TSearchDataType.String, 20);
			this.GridFilterParameters.Add("nome_comp", "Descrição", TSearchDataType.String, 80);
			this.MaxRows = 100;
			this.MaxLength = 100;
			this.GridWidth = Unit.Pixel(600);

		}

		public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
		{
			ArrayList parValues = new ArrayList();
			StringBuilder sql = new StringBuilder();

			sql.Append(string.Format

					(@"SELECT unidade_ens, nome_comp from VW_UNIDADE_ENSINO_SITUACAO_REGIONAL U (NOLOCK) 
						WHERE NOT EXISTS (	SELECT 1
						FROM SERIEABSORVIDA S (NOLOCK)
						WHERE S.UNIDADEENSINOORIGEMID = U.UNIDADE_ENS
						AND S.NIVELABSORCAOID = 1)",
							 maxRows));

            if (key != null)
            {
                sql.Append(" AND U.unidade_ens  = ? ");
                parValues.Add(key.ToString());
            }
            else
            {

                if (pars.ContainsKey("unidade_ens") && pars["unidade_ens"] != null && pars["unidade_ens"].ToString().Trim().Length > 0)
                {
                    sql.Append(" and U.unidade_ens = ? ");
                    parValues.Add(pars["unidade_ens"].ToString());
                }
            }

			if (pars.ContainsKey("nome_comp") && pars["nome_comp"] != null && pars["nome_comp"].ToString().Trim().Length > 0)
			{
				sql.Append(" and U.nome_comp like ? ");
				parValues.Add(LikeExpression("*" + pars["nome_comp"].ToString()));
			}

			QueryTable qt = new QueryTable(sql.ToString());
			qt.Query(this.CreateConnection(), parValues.ToArray());
			DataView dv = qt.DefaultView;
			dv.Sort = "nome_comp";
			return dv;
		}

	}
}
