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
    public class QueryCandidatoDocenteMigracao : LyceumQuery
    {
        public QueryCandidatoDocenteMigracao()
        {
            this.GridColumns.Add(new TSearchColumn() { FieldName = "idvinculo_matricula", Caption = "IdVinculo / Matricula", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "idvinculo", Caption = "Id/Vínculo", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "matricula", Caption = "Matrícula ou ID/Vínculo", Width = Unit.Percentage(20) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "idfuncional", Caption = "ID Funcional", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "nome", Caption = "Nome", Width = Unit.Percentage(25) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "cpf", Caption = "CPF", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "rg_num", Caption = "Documento", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "email", Caption = "E-mail", Width = Unit.Percentage(20) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "num_func", Caption = "Nº Docente", Width = Unit.Percentage(5) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "pessoa", Caption = "Pessoa", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "docentecandidatoid", Caption = "docentecandidatoid", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "categoria", Caption = "categoria", Width = Unit.Percentage(0), Visible = false });
            
            this.Messages.KeyNotFound = "Id/vínculo inválido ou usuário sem permissão de visualização do Docente(Lotação).";
            this.TextField = "idvinculo_matricula";
            this.DescriptionField = "nome";
            this.GridFilterParameters.Add("nome", "Nome", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("idvinculo", "Id/Vínculo", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("matricula", "Matrícula", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("rg_num", "Documento", TSearchDataType.String, 25);
            this.GridFilterParameters.Add("cpf", "CPF", TSearchDataType.String, 19);
            this.GridFilterParameters.Add("num_func", "Nº Docente", TSearchDataType.String, 15);

            this.MaxRows = 100;
            this.MaxLength = 100;
            this.GridWidth = Unit.Pixel(800);
        }

        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            ArrayList parValues = new ArrayList();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT top " + maxRows.ToString() + @" d.matricula, 
		                    d.num_func, 
                            d.categoria,
                            p.nome_compl as nome,  
		                    p.rg_num, 
		                    p.cpf, 
		                    p.pessoa, 
                            p.nome_mae as mae, 
                            p.nome_pai as pai, 
                            p.E_MAIL_INTERNO as email, 
		                    p.idfuncional, 
		                    (convert(varchar,p.IDFUNCIONAL) + '/' + convert(varchar,d.VINCULO)) idvinculo,
		                    ISNULL((convert(varchar,p.IDFUNCIONAL) + '/' + convert(varchar,d.VINCULO)),d.matricula) idvinculo_matricula,
                            DC.DOCENTECANDIDATOID                                                      
                        from RecursosHumanos.DOCENTECANDIDATO DC	                    
                        iNNER JOIN LY_DOCENTE D (NOLOCK)  on dc.NUM_FUNC = d.NUM_FUNC
						INNER JOIN ly_pessoa p ON p.pessoa = D.pessoa
	                    where voluntario = 'N' AND SITUACAO = 3");

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
                    sql.Append(" and (convert(varchar,p.IDFUNCIONAL) + '/' + convert(varchar,d.VINCULO)) like ? ");
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
                if (pars.ContainsKey("num_func") && pars["num_func"] != null && pars["num_func"].ToString().Trim().Length > 0)
                {
                    sql.Append(" and convert(varchar,d.num_func) like ? ");
                    parValues.Add(LikeExpression(pars["num_func"].ToString()));
                }

                if (this.HasValue(pars, "Concurso"))
                {
                    sql.Append("  AND convert(varchar,dc.concurso) like ?  ");
                    parValues.Add(LikeExpression(pars["Concurso"].ToString()));
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
