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
    public class QueryDocenteLotacaoAtiva : LyceumQuery
    {
        public QueryDocenteLotacaoAtiva()
        {
			this.GridColumns.Add(new TSearchColumn() { FieldName = "matricula", Caption = "Matrícula", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "idvinculo", Caption = "Id/Vínculo", Width = Unit.Percentage(10) });
			this.GridColumns.Add(new TSearchColumn() { FieldName = "nome", Caption = "Nome", Width = Unit.Percentage(24) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "rg_num", Caption = "Documento", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "cpf", Caption = "CPF", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "num_func", Caption = "Nº Docente", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "pessoa", Caption = "Pessoa", Width = Unit.Percentage(0), Visible = false });

            this.Messages.KeyNotFound = "Matrícula ou Id/Vínculo inválida(o) ou sem lotação ativa na UA selecionada.";
            this.TextField = "idvinculo";
            this.DescriptionField = "nome";
            this.GridFilterParameters.Add("nome", "Nome", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("matricula", "Matrícula", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("rg_num", "Documento", TSearchDataType.String, 25);
            this.GridFilterParameters.Add("cpf", "CPF", TSearchDataType.String, 19);
            this.GridFilterParameters.Add("num_func", "Nº Docente", TSearchDataType.String, 15);
            this.GridFilterParameters.Add("idvinculo", "ID/Vínculo", TSearchDataType.String, 15);

            this.MaxRows = 100;
            this.MaxLength = 100;
            this.GridWidth = Unit.Pixel(800);
        }

        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            ArrayList parValues = new ArrayList();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT distinct top " + maxRows.ToString() + " d.matricula, d.num_func, p.nome_compl as nome,  p.rg_num, p.cpf, p.pessoa, lo.setor, p.idfuncional, " +
                " ISNULL((CONVERT(VARCHAR, P.IDFUNCIONAL) + '/' + CONVERT(VARCHAR ,D.VINCULO)), D.MATRICULA) idvinculo " +
                "FROM  " +
                "ly_pessoa p (nolock) inner join ly_docente d " +
                "on p.pessoa = d.pessoa " +
                " inner join ly_lotacao lo (NOLOCK) on d.matricula = lo.matricula " +
                " inner join hades..vw_setor s on s.setor = lo.setor " +
                " where 1=1 and voluntario='N' " +
                " and (lo.data_desativacao is null OR convert(date,lo.data_desativacao) > convert(date,GetDate()))");

            if (key != null)
            {
                sql.Append(" and ISNULL((CONVERT(VARCHAR, P.IDFUNCIONAL) + '/' + CONVERT(VARCHAR ,D.VINCULO)), D.MATRICULA) = ? ");
                parValues.Add(key);
            }
            else
            {
                if (pars.ContainsKey("nome") && pars["nome"] != null && pars["nome"].ToString().Trim().Length > 0)
                {
                    sql.Append(" and p.nome_compl like ? ");
                    parValues.Add(LikeExpression(pars["nome"].ToString()));
                }

                if (pars.ContainsKey("matricula") && pars["matricula"] != null && pars["matricula"].ToString().Trim().Length > 0)
                {
                    sql.Append(" and D.MATRICULA like ? ");
                    parValues.Add(LikeExpression(pars["matricula"].ToString()));
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

                if (pars.ContainsKey("idvinculo") && pars["idvinculo"] != null && pars["idvinculo"].ToString().Trim().Length > 0)
                {
                    sql.Append(" and ISNULL((CONVERT(VARCHAR, P.IDFUNCIONAL) + '/' + CONVERT(VARCHAR ,D.VINCULO)), D.MATRICULA) like ? ");
                    parValues.Add(LikeExpression(pars["idvinculo"].ToString()));
                }
               
                if (pars.ContainsKey("num_func") && pars["num_func"] != null && pars["num_func"].ToString().Trim().Length > 0)
                {
                    sql.Append(" and convert(varchar,d.num_func) like ? ");
                    parValues.Add(LikeExpression(pars["num_func"].ToString()));
                }                
            }

            QueryTable qt = new QueryTable(sql.ToString());
            qt.Query(this.CreateConnection(), parValues.ToArray());
            DataView dv = qt.DefaultView;
            dv.Sort = "nome asc";
            return dv;

        }

    }
}
