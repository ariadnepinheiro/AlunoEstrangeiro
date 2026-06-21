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
	public class QueryAbsorcaoUnidadeEnsinoOrigem : LyceumQuery
	{
		public QueryAbsorcaoUnidadeEnsinoOrigem()
		{
			this.GridColumns.Add(new TSearchColumn() { FieldName = "regional", Caption = "Regional", Width = Unit.Percentage(10) });
			this.GridColumns.Add(new TSearchColumn() { FieldName = "nome", Caption = "Município", Width = Unit.Percentage(10) });
			this.GridColumns.Add(new TSearchColumn() { FieldName = "unidade_ens", Caption = "Código", Width = Unit.Percentage(10) });
			this.GridColumns.Add(new TSearchColumn() { FieldName = "nome_comp", Caption = "Descrição", Width = Unit.Percentage(30) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "ua_antiga", Caption = "U.A. Antiga", Width = Unit.Percentage(8) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "ua_atual", Caption = "U.A.", Width = Unit.Percentage(8) });
			this.GridColumns.Add(new TSearchColumn() { FieldName = "cgc", Caption = "CNPJ", Width = Unit.Percentage(13) });
			this.GridColumns.Add(new TSearchColumn() { FieldName = "situacao", Caption = "Situação", Width = Unit.Percentage(18) });

			this.Messages.KeyNotFound = "Não foram encontradas unidades a serem absorvidas.";

			this.TextField = "unidade_ens";
			this.DescriptionField = "nome_comp";
			this.GridFilterParameters.Add("regional", "Regional", TSearchDataType.String, 100);
			this.GridFilterParameters.Add("nome", "Município", TSearchDataType.String, 100);
			this.GridFilterParameters.Add("unidade_ens", "Código", TSearchDataType.String, 25);
			this.GridFilterParameters.Add("nome_comp", "Descrição", TSearchDataType.String, 19);
			this.GridFilterParameters.Add("setor", "U.A.", TSearchDataType.String, 15);
			this.GridFilterParameters.Add("cgc", "CNPJ", TSearchDataType.String, 15);
			this.MaxRows = 100;
			this.MaxLength = 100;
			this.GridWidth = Unit.Pixel(850);
		}

		public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
		{
			ArrayList parValues = new ArrayList();
			StringBuilder sql = new StringBuilder();
			sql.Append(string.Format
                (@"SELECT unidade_ens, nome_comp, setor, cgc, situacao,municipio,nome, regional ,ua_atual,ua_antiga
					from VW_UNIDADE_ENSINO_SITUACAO_REGIONAL as U (NOLOCK) 
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
            if (pars.ContainsKey("regional") && pars["regional"] != null && pars["regional"].ToString().Trim().Length > 0)
            {
                sql.Append(" and U.regional = ? ");
                parValues.Add(pars["regional"].ToString());
            }

			if (pars.ContainsKey("setor") && pars["setor"] != null && pars["setor"].ToString().Trim().Length > 0)
			{
				sql.Append(" and U.setor = ? ");
				parValues.Add(pars["setor"].ToString());
			}

			if (pars.ContainsKey("nome") && pars["nome"] != null && pars["nome"].ToString().Trim().Length > 0)
			{
				sql.Append(" and U.nome like ? ");
				parValues.Add(LikeExpression(pars["nome"].ToString()));
			}

			if (pars.ContainsKey("nome_comp") && pars["nome_comp"] != null && pars["nome_comp"].ToString().Trim().Length > 0)
			{
				sql.Append(" and U.nome_comp like ? ");
				parValues.Add(LikeExpression("*" + pars["nome_comp"].ToString()));
			}

			if (pars.ContainsKey("cgc") && pars["cgc"] != null && pars["cgc"].ToString().Trim().Length > 0)
			{
				sql.Append(" and U.cgc = ? ");
				parValues.Add(pars["cgc"].ToString());
			}

			QueryTable qt = new QueryTable(sql.ToString());
			qt.Query(this.CreateConnection(), parValues.ToArray());
			DataView dv = qt.DefaultView;
			dv.Sort = "nome_comp";
			return dv;
		}
	}
}
