using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Techne.Data;
using Techne.Web;

namespace Techne.Lyceum.RN.Query
{
    public class QueryAlunoSolicitacaoManual : LyceumQuery
    {
        public QueryAlunoSolicitacaoManual()
        {
            this.GridColumns.Add(new TSearchColumn { FieldName = "aluno", Caption = "Matrícula", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "nome", Caption = "Nome", Width = Unit.Percentage(20) });

            this.Messages.KeyNotFound = "Matrícula inválida";
            this.TextField = "aluno";
            this.MaxLength = 20;
            this.DescriptionField = "nome";

            this.GridFilterParameters.Add("aluno", "Matrícula", TSearchDataType.String, 20);
            this.GridFilterParameters.Add("nome", "Nome", TSearchDataType.String, 100);

            this.MaxRows = 50;
            this.GridWidth = Unit.Pixel(860);
        }

        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            var parValues = new ArrayList();
            var sql = new StringBuilder();

            sql.Append(
                string.Format(
                    @"SELECT DISTINCT TOP {0} 
                            a.aluno,
                            PE.nome_compl AS nome
                    FROM    LY_ALUNO a WITH ( NOLOCK )
                    INNER JOIN LY_PESSOA PE (NOLOCK)  ON PE.PESSOA = A.PESSOA
                    WHERE   1 = 1 ",
                    maxRows,
                    RNBase.MudarAspas(System.Threading.Thread.CurrentPrincipal.Identity.Name)));

            if (key != null)
            {
                sql.Append(" AND a.aluno = ? ");
                parValues.Add(key.ToString());
            }
            else
            {
                if (this.HasValue(pars, "aluno"))
                {
                    sql.Append(" AND a.aluno LIKE ? ");
                    parValues.Add(this.LikeExpression(pars["aluno"].ToString()));
                }
            }

            if (this.HasValue(pars, "nome"))
            {
                sql.Append(" AND PE.nome_compl LIKE ? ");
                parValues.Add(this.LikeExpression(pars["nome"].ToString()));
            }

            var qt = new QueryTable(sql.ToString());

            qt.Query(this.CreateConnection(), parValues.ToArray());

            var dv = qt.DefaultView;

            dv.Sort = "nome ASC";

            return dv;
        }
    }
}