using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Web;
using System.Web.UI.WebControls;
using System.Collections;
using Techne.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Query
{
    public class QueryPessoaLotacao : LyceumQuery
    {
        public QueryPessoaLotacao()
        {
            this.GridColumns.Add(new TSearchColumn() { FieldName = "pessoa", Caption = "Pessoa", Width = Unit.Percentage(0), Visible = false });            
            this.GridColumns.Add(new TSearchColumn() { FieldName = "nome", Caption = "Nome", Width = Unit.Percentage(24) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "rg_num", Caption = "Documento", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "cpf", Caption = "CPF", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "mae", Caption = "Nome da Mãe", Width = Unit.Percentage(18) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "pai", Caption = "Nome do Pai", Width = Unit.Percentage(18) });            

            this.Messages.KeyNotFound = "Pessoa inválida.";
            this.TextField = "pessoa";
            this.DescriptionField = "nome";
            
            this.GridFilterParameters.Add("nome", "Nome", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("pessoa", "Pessoa", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("rg_num", "Documento", TSearchDataType.String, 25);
            this.GridFilterParameters.Add("cpf", "CPF", TSearchDataType.String, 19);
            this.MaxRows = 100;
            this.MaxLength = 100;
            this.GridWidth = Unit.Pixel(800);
        }

        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            ArrayList parValues = new ArrayList();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT top " + maxRows.ToString() +
               @"p.nome_compl as nome,  p.rg_num, p.cpf, p.pessoa, a.aluno,	
	            p.nome_mae as mae,
	            p.nome_pai as pai
                FROM
                ly_pessoa p (nolock) inner join LY_ALUNO a (nolock)
                on p.pessoa = a.pessoa where 1=1");

            if (key != null)
            {
                sql.Append(" and p.pessoa = ? ");
                parValues.Add(key);
            }
            else if (pars.ContainsKey("pessoa") && pars["pessoa"] != null && pars["pessoa"].ToString().Trim().Length > 0)
            {
                sql.Append(" and convert(varchar,p.pessoa) like ? ");
                parValues.Add(LikeExpression(pars["pessoa"].ToString()));
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
