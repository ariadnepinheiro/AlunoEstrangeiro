using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Techne.Data;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using System.Web;


namespace Techne.Lyceum.RN.Query
{
    public class QueryDocenteCad : LyceumQuery
    {
        public QueryDocenteCad()
        {
            this.GridColumns.Add(new TSearchColumn() { FieldName = "idvinculo_matricula", Caption = "IdVinculo / Matricula", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "idvinculo", Caption = "Id/Vinculo", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "matricula", Caption = "Matrícula ou ID/Vínculo", Width = Unit.Percentage(20) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "pessoa", Caption = "Código", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "nome", Caption = "Nome", Width = Unit.Percentage(30) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "cpf", Caption = "CPF", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "rg_num", Caption = "Documento", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "num_func", Caption = "Codigo Docente", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "nome_social", Caption = "Nome Social", Width = Unit.Percentage(20) });

            this.Messages.KeyNotFound = "Id/Vínculo inválido ou usuário sem permissão de visualização do Docente (Lotação).";
            this.TextField = "idvinculo_matricula";
            this.DescriptionField = "nome";
            this.GridFilterParameters.Add("nome", "Nome", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("idvinculo", "Id/Vinculo", TSearchDataType.String, 19);            
            this.GridFilterParameters.Add("matricula", "Matrícula", TSearchDataType.String, 19);
            this.GridFilterParameters.Add("cpf", "CPF", TSearchDataType.String, 19);
            this.GridFilterParameters.Add("rg_num", "Documento", TSearchDataType.String, 25);            
            this.MaxRows = 100;
            this.MaxLength = 100;
            this.GridWidth = Unit.Pixel(800);
        }

        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            ArrayList parValues = new ArrayList();
            StringBuilder sql = new StringBuilder();
            bool altdoc_parcial = false;


            if (RN.PadroesDeAcessos.ConsultarPadacesParcial(System.Threading.Thread.CurrentPrincipal.Identity.Name))
            {
                altdoc_parcial = true;
            }
            sql.Append(string.Format(@"SELECT distinct top {0} d.matricula,
					        ISNULL((convert(varchar,p.IDFUNCIONAL) + '/' + convert(varchar,d.VINCULO)),d.matricula) idvinculo_matricula, 
					        (convert(varchar,p.IDFUNCIONAL) + '/' + convert(varchar,d.VINCULO)) idvinculo,
					        d.num_func, 
					        p.nome_compl as nome, 
					        p.PRE_NOME_SOCIAL as nome_social, 
					        p.rg_num, 
					        p.cpf, 
					        p.pessoa, 
                            p.nome_mae as mae, 
					        p.nome_pai as pai,
					        p.idfuncional
                    FROM   USUARIO u WITH (NOLOCK),
                        ly_pessoa p (nolock) inner join ly_docente d 
                            on p.pessoa = d.pessoa ", maxRows));

            if (altdoc_parcial)
            {
                sql.Append(string.Format(@" inner join ly_lotacao lo (NOLOCK) on d.matricula = lo.matricula"));
            }

            sql.Append(string.Format(@"
                    where 1=1 
                    and voluntario='N'
                     and  u.USUARIO = '{0}'",RNBase.MudarAspas(System.Threading.Thread.CurrentPrincipal.Identity.Name)));

            if (altdoc_parcial)
            { 
                sql.Append(@" 
                    AND (LO.DATA_DESATIVACAO IS NULL 
                                OR CONVERT(DATE,LO.DATA_DESATIVACAO) > CONVERT(DATE,GETDATE()))
                    AND (EXISTS ( SELECT TOP 1
                                                      UNIDADE_FIS
                                            FROM      LY_USUARIO_UNIDADE_FIS usuuni WITH (NOLOCK)
                                            WHERE     usuuni.UNIDADE_FIS = lo.UNIDADE_ENS
                                                      AND usuuni.USUARIO = u.USUARIO
                                                      AND u.PRIVIL <> 'S' )
	                                                  OR (U.PRIVIL = 'S')) ") ;
            }

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

                if (pars.ContainsKey("idfuncional") && pars["idfuncional"] != null && pars["idfuncional"].ToString().Trim().Length > 0)
                {
                    sql.Append(" and idfuncional like ? ");
                    parValues.Add(LikeExpression(pars["idfuncional"].ToString()));
                }

                if (pars.ContainsKey("matricula") && pars["matricula"] != null && pars["matricula"].ToString().Trim().Length > 0)
                {
                    sql.Append(" and convert(varchar, d.matricula) like ? ");
                    parValues.Add(LikeExpression(pars["matricula"].ToString()));
                }

                if (pars.ContainsKey("cpf") && pars["cpf"] != null && pars["cpf"].ToString().Trim().Length > 0)
                {
                    sql.Append(" and p.cpf like ? ");
                    string cpf = LikeExpression(pars["cpf"].ToString().RetirarMascaraCPF());
                    parValues.Add(cpf);
                }

                if (pars.ContainsKey("rg_num") && pars["rg_num"] != null && pars["rg_num"].ToString().Trim().Length > 0)
                {
                    sql.Append(" and p.rg_num like ? ");
                    string rg = LikeExpression(pars["rg_num"].ToString().RetirarMascaraRG());
                    parValues.Add(rg);
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
