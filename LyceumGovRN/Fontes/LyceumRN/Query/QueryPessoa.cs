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
    public class QueryPessoa : LyceumQuery
    {
        public QueryPessoa()
        {
            this.GridColumns.Add(new TSearchColumn() { FieldName = "pessoa", Caption = "Código", Width = Unit.Percentage(12) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "nome", Caption = "Nome", Width = Unit.Percentage(28) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "rg_num", Caption = "Documento", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "cpf", Caption = "CPF", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "mae", Caption = "Nome da Mãe", Width = Unit.Percentage(20) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "pai", Caption = "Nome do Pai", Width = Unit.Percentage(20) });

            this.Messages.KeyNotFound = "Código de pessoa inválido";
            this.TextField = "pessoa";
            this.DescriptionField = "nome";
			this.GridFilterParameters.Add("nome", "Nome", TSearchDataType.String, 100);
			this.GridFilterParameters.Add(new TSearchParameter() { ParameterName = "pessoa", Caption = "Código", ParameterType = TSearchDataType.Integer, MaxLength = 10 });
            this.GridFilterParameters.Add("rg_num", "Documento", TSearchDataType.String, 25);
            this.GridFilterParameters.Add("cpf", "CPF", TSearchDataType.String, 19);
            this.MaxRows = 100;
            this.GridWidth = Unit.Pixel(800);
            this.TextFieldType=TSearchDataType.Integer;
            this.MaxLength = 10;
        }

        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            ArrayList parValues = new ArrayList();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT top " + maxRows.ToString() +" "+
                "p.pessoa,p.nome_compl as nome, p.rg_num, p.cpf,  " +
                "p.nome_mae as mae, " +
                "p.nome_pai as pai " +
                "FROM  ly_pessoa p (nolock) where 1=1 ");

            if (key != null)
            {
                decimal pessoa = -1;
                try
                {
                    pessoa = Convert.ToDecimal(key);
                }
                catch { }
                sql.Append(" and p.pessoa = ? ");
                parValues.Add(pessoa);
            }
            else
            {
                if (pars.ContainsKey("pessoa") && pars["pessoa"] != null && pars["pessoa"].ToString().Trim().Length > 0)
                {
                    sql.Append(" and convert(varchar,p.pessoa) like ? ");
                    parValues.Add(LikeExpression(pars["pessoa"].ToString()));
                }
            }

            if (pars.ContainsKey("nome") && pars["nome"] != null && pars["nome"].ToString().Trim().Length > 0)
            {
                sql.Append(" and p.nome_compl like ? ");
                parValues.Add(LikeExpression(pars["nome"].ToString()));
            }

            if (pars.ContainsKey("rg_num") && pars["rg_num"] != null && pars["rg_num"].ToString().Trim().Length > 0)
            {
                sql.Append(" and p.rg_num like ? ");
                string rg = LikeExpression(pars["rg_num"].ToString().RetirarMascaraRG());
                parValues.Add(rg);
            }

            if (pars.ContainsKey("cpf") && pars["cpf"] != null && pars["cpf"].ToString().Trim().Length > 0)
            {
                sql.Append(" and p.cpf like ? ");
                string cpf = LikeExpression(pars["cpf"].ToString().RetirarMascaraCPF());
                parValues.Add(cpf);
            }

            QueryTable qt = new QueryTable(sql.ToString());
            qt.Query(this.CreateConnection(), parValues.ToArray());
            DataView dv = qt.DefaultView;
            dv.Sort = "nome asc";
            return dv;

        }

    }
}
