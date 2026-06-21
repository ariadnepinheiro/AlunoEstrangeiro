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
    public class QueryManutencaoVinculo : LyceumQuery
    {
        public QueryManutencaoVinculo()
        {
            this.GridColumns.Add(new TSearchColumn() { FieldName = "idvinculo_matricula", Caption = "IdVinculo / Matricula", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "idvinculo", Caption = "ID/Vínculo", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "idfuncional", Caption = "Id Funcional", Width = Unit.Percentage(0), Visible = false }); 
            this.GridColumns.Add(new TSearchColumn() { FieldName = "matricula", Caption = "Matrícula ou ID/Vínculo", Width = Unit.Percentage(20) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "nome", Caption = "Nome", Width = Unit.Percentage(30) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "cpf", Caption = "CPF", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "rg_num", Caption = "Documento", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "e_mail", Caption = "E-mail", Width = Unit.Percentage(20) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "pessoa", Caption = "Pessoa", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "ordem", Caption = "Ordem", Width = Unit.Percentage(0), Visible = false });

            this.Messages.KeyNotFound = "Id/vínculo inválido.";
            this.TextField = "idvinculo_matricula";
            this.DescriptionField = "nome";
            this.GridFilterParameters.Add("nome", "Nome", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("idvinculo", "ID/Vínculo", TSearchDataType.String, 19);
            this.GridFilterParameters.Add("matricula", "Matrícula", TSearchDataType.String, 19);            
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

            sql.Append("SELECT distinct top " + maxRows.ToString() + @" d.matricula, 
				            p.nome_compl as nome,  
				            p.rg_num, 
				            p.cpf, 
				            p.pessoa,
                            p.idfuncional,
                            p.e_mail,
				            convert(varchar,p.IDFUNCIONAL) + '/' + convert(varchar,d.VINCULO) as idvinculo,
				            ISNULL((convert(varchar,p.IDFUNCIONAL) + '/' + convert(varchar,d.VINCULO)),d.matricula) as idvinculo_matricula,
                            d.ordem
                    FROM  ly_pessoa p (nolock) 
			            inner join ly_vinculo d (nolock) on p.pessoa = d.pessoa 
                        left join ly_lotacao l (nolock) on l.ORDEM = d.ORDEM and l.MATRICULA = d.MATRICULA and l.PESSOA = d.PESSOA
                    where 1 = 1 ");

            if (key != null)
            {
                sql.Append(" and ISNULL((convert(varchar,p.IDFUNCIONAL) + '/' + convert(varchar,d.VINCULO)),d.matricula) = ? ");
                parValues.Add(key);
            }
            else
            {
                if (pars.ContainsKey("nome") && pars["nome"] != null && pars["nome"].ToString().Trim().Length > 0)
                {
                    sql.Append(" and p.nome_compl like ? ");
                    parValues.Add(LikeExpression(pars["nome"].ToString()));
                }

                if (pars.ContainsKey("idvinculo") && pars["idvinculo"] != null && pars["idvinculo"].ToString().Trim().Length > 0)
                {
                    sql.Append(" and convert(varchar,p.IDFUNCIONAL) + '/' + convert(varchar,d.VINCULO) like ? ");
                    parValues.Add(LikeExpression(pars["idvinculo"].ToString()));
                }

                if (pars.ContainsKey("matricula") && pars["matricula"] != null && pars["matricula"].ToString().Trim().Length > 0)
                {
                    sql.Append(" and d.matricula like ? ");
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
            
            }

            QueryTable qt = new QueryTable(sql.ToString());
            qt.Query(this.CreateConnection(), parValues.ToArray());
            DataView dv = qt.DefaultView;
            dv.Sort = "nome asc";
            return dv;

        }

    }
}