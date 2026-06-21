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
    public class QueryArmazemLivro : LyceumQuery
    {
        public QueryArmazemLivro()
        {
            this.GridColumns.Add(new TSearchColumn() { FieldName = "nome_completo", Caption = "Nome", Width = Unit.Percentage(24) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "rg_num", Caption = "Documento", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "cpf", Caption = "CPF", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "idfuncional", Caption = "ID Funcional", Width = Unit.Percentage(18) });

            this.Messages.KeyNotFound = "CPF inválido ou sem permissão de visualização.";
            this.TextField = "cpf";
            this.DescriptionField = "nome_completo";
            this.GridFilterParameters.Add("nome_completo", "Nome", TSearchDataType.String, 100);            
            this.GridFilterParameters.Add("rg_num", "Documento", TSearchDataType.String, 25);
            this.GridFilterParameters.Add("cpf", "CPF", TSearchDataType.String, 19);
            this.GridFilterParameters.Add("idfuncional", "ID Funcional", TSearchDataType.String, 8);
            this.MaxRows = 100;
            this.MaxLength = 100;
            this.GridWidth = Unit.Pixel(800);
        }

        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            ArrayList parValues = new ArrayList();
            StringBuilder sql = new StringBuilder();

            sql.Append(string.Format(@"SELECT distinct top {0} a.cpf, nome_completo, escola,rg_num,idfuncional
                    FROM   armazem_livro_2019 A WITH (NOLOCK)
                     inner join ly_pessoa p (nolock) 
                    on p.pessoa = A.pessoa 
                      INNER JOIN USUARIO u ON u.USUARIO = '{1}'", maxRows,RNBase.MudarAspas(System.Threading.Thread.CurrentPrincipal.Identity.Name)));
                       
                sql.Append(@" 
                     where ( EXISTS ( SELECT TOP 1
                                                UNIDADE_FIS
                                       FROM     LY_USUARIO_UNIDADE_FIS usuuni
                                                WITH ( NOLOCK )
                                       WHERE    usuuni.UNIDADE_FIS = A.CENSO
                                                AND usuuni.USUARIO = u.USUARIO
                                                AND u.PRIVIL <> 'S' )
                              OR ( U.PRIVIL = 'S' )
                            ) ");


                if (key != null)
                {
                    sql.Append(" and A.CPF = ? ");
                    parValues.Add(key);
                }
                else
                {
                    if (pars.ContainsKey("cpf") && pars["cpf"] != null && pars["cpf"].ToString().Trim().Length > 0)
                    {
                        sql.Append(" and convert(varchar,A.CPF) like ? ");
                        parValues.Add(LikeExpression(pars["cpf"].ToString()));
                    }

                    if (pars.ContainsKey("nome_completo") && pars["nome_completo"] != null && pars["nome_completo"].ToString().Trim().Length > 0)
                    {
                        sql.Append(" and nome_completo like ? ");
                        parValues.Add(LikeExpression(pars["nome_completo"].ToString()));
                    }

                    if (pars.ContainsKey("rg_num") && pars["rg_num"] != null && pars["rg_num"].ToString().Trim().Length > 0)
                    {
                        sql.Append(" and p.rg_num like ? ");
                        string rg = LikeExpression(pars["rg_num"].ToString().RetirarMascaraRG());
                        parValues.Add(rg);
                    }

                    if (pars.ContainsKey("idfuncional") && pars["idfuncional"] != null && pars["idfuncional"].ToString().Trim().Length > 0)
                    {
                        sql.Append(" and convert(varchar,p.idfuncional) like ? ");
                        parValues.Add(LikeExpression(pars["idfuncional"].ToString()));
                    }
                }

            QueryTable qt = new QueryTable(sql.ToString());
            qt.Query(this.CreateConnection(), parValues.ToArray());
            DataView dv = qt.DefaultView;
            dv.Sort = "nome_completo asc";
            return dv;

        }
    }
}
