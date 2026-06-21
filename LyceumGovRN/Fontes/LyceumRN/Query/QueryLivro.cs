using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Data;

using Techne.Web;
using Techne.Data;
using System.Web.UI.WebControls;

namespace Techne.Lyceum.RN.Query
{
    public class QueryLivro : LyceumQuery
    {
        public QueryLivro()
            : base()
        {
            this.GridColumns.Add(new TSearchColumn() { FieldName = "aluno", Caption = "Matrícula", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "nome", Caption = "Nome", Width = Unit.Percentage(23) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "rg_num", Caption = "Documento", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "cpf", Caption = "CPF", Width = Unit.Percentage(8) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "mae", Caption = "Nome da Mãe", Width = Unit.Percentage(19) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "pai", Caption = "Nome do Pai", Width = Unit.Percentage(19) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "dt_nascimento", Caption = "Nascimento", Width = Unit.Percentage(9) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "pessoa", Caption = "pessoa", Width = Unit.Percentage(0), Visible = false });

            this.Messages.KeyNotFound = "Livro não encontrado.";
            this.TextField = "aluno";
            this.MaxLength = 20;
            this.DescriptionField = "nome";
            this.GridFilterParameters.Add("nome", "Nome", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("aluno", "Matrícula", TSearchDataType.String, 20);
            this.GridFilterParameters.Add("mae", "Mãe", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("pai", "Pai", TSearchDataType.String, 100);
            //this.GridFilterParameters.Add("ativo", "ativo", TSearchDataType.String, 100);
            this.GridFilterParameters.Add(new TSearchParameter() { Caption = "ativo", ParameterName = "ativo", ShowInFilterPanel = false, ParameterType = TSearchDataType.String });


            this.MaxRows = 100;

            this.GridWidth = Unit.Pixel(860);
        }

        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            try
            {
                ArrayList parValues = new ArrayList();
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT distinct top " + maxRows + " a.aluno, ");
                sql.Append("p.nome_mae as mae, ");
                sql.Append("p.nome_pai as pai, ");
                sql.Append("p.pessoa,p.nome_compl as nome,p.rg_num,p.cpf, Convert(varchar(10), p.DT_NASC, 103) as dt_nascimento ");
                sql.Append("FROM  ");
                sql.Append("ly_pessoa p (nolock), ( ");
                sql.Append("select ALU.pessoa, ALU.aluno, ALU.SIT_ALUNO FROM LY_ALUNO ALU, USUARIO U where U.USUARIO = '" + RN.RNBase.MudarAspas(System.Threading.Thread.CurrentPrincipal.Identity.Name) + "' and U.PRIVIL = 'S' union all ");
                sql.Append("select ALU.pessoa, ALU.aluno, ALU.SIT_ALUNO FROM USUARIO U, LY_USUARIO_UNIDADE_FIS USUUNI JOIN LY_ALUNO ALU ");
                sql.Append("ON USUUNI.UNIDADE_FIS = ALU.UNIDADE_FISICA where U.USUARIO = '" + RN.RNBase.MudarAspas(System.Threading.Thread.CurrentPrincipal.Identity.Name) + "'  AND USUUNI.USUARIO = U.USUARIO and U.PRIVIL <> 'S') a ");
                sql.Append("where p.pessoa = a.pessoa ");
                // sql.Append("ly_pessoa p inner join " + VW_ZZCRO_ALUNO + " a ");
                // sql.Append("on p.pessoa = a.pessoa ");
                //sql.Append("where 1=1 ");


                if (key != null)
                {
                    sql.Append(" and a.aluno = ? ");
                    parValues.Add(key.ToString());
                }
                else
                {
                    if (pars.ContainsKey("aluno") && pars["aluno"] != null && pars["aluno"].ToString().Trim().Length > 0)
                    {
                        sql.Append(" and a.aluno like ? ");
                        parValues.Add(LikeExpression(pars["aluno"].ToString()));
                    }
                }

                if (pars.ContainsKey("nome") && pars["nome"] != null && pars["nome"].ToString().Trim().Length > 0)
                {
                    sql.Append(" and p.nome_compl like ? ");
                    parValues.Add(LikeExpression(pars["nome"].ToString()));
                }

                if (pars.ContainsKey("mae") && pars["mae"] != null && pars["mae"].ToString().Trim().Length > 0)
                {
                    sql.Append(" and p.nome_mae like ? ");
                    parValues.Add(LikeExpression(pars["mae"].ToString()));
                }

                if (pars.ContainsKey("pai") && pars["pai"] != null && pars["pai"].ToString().Trim().Length > 0)
                {
                    sql.Append(" and p.nome_pai like ? ");
                    parValues.Add(LikeExpression(pars["pai"].ToString()));
                }

                if (pars.ContainsKey("ativo") && pars["ativo"] != null && pars["ativo"].ToString().Trim().Length > 0)
                {
                    sql.Append(" and a.SIT_ALUNO = ? ");
                    parValues.Add(pars["ativo"].ToString());
                }

                QueryTable qt = new QueryTable(sql.ToString());
                qt.Query(this.CreateConnection(), parValues.ToArray());
                DataView dv = qt.DefaultView;
                dv.Sort = "nome asc";
                return dv;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
