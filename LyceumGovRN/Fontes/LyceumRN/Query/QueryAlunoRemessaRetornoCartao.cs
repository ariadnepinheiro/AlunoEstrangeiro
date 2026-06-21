using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Techne.Web;
using System.Linq;
using Techne.Data;

namespace Techne.Lyceum.RN.Query
{
    public class QueryAlunoRemessaRetornoCartao : LyceumQuery
    {
        public QueryAlunoRemessaRetornoCartao()
        {
            this.GridColumns.Add(new TSearchColumn { FieldName = "aluno", Caption = "Matrícula", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "nome", Caption = "Nome", Width = Unit.Percentage(20) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "idbeneficiario", Caption = "ID Beneficiário", Width = Unit.Percentage(20) });

            this.Messages.KeyNotFound = "Matrícula inválida";
            this.TextField = "aluno";
            this.MaxLength = 15;
            this.DescriptionField = "nome";

            this.GridFilterParameters.Add("nome", "Nome", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("aluno", "Matrícula", TSearchDataType.String, 20);

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
                            r.aluno,
                            r.nome_compl AS nome,
                            ro.idbeneficiario
                    FROM    CartaoEstudante.REMESSA r WITH ( NOLOCK )
                    LEFT JOIN CartaoEstudante.retorno ro WITH ( NOLOCK ) on r.REMESSAID = RO.REMESSAID
                    WHERE   1 = 1",
                    maxRows,
                    RNBase.MudarAspas(System.Threading.Thread.CurrentPrincipal.Identity.Name)));

            if (key != null)
            {
                sql.Append(" AND r.aluno = ? ");
                parValues.Add(key.ToString());
            }
            else
            {
                if (this.HasValue(pars, "aluno"))
                {
                    sql.Append(" AND r.aluno LIKE ? ");
                    parValues.Add(this.LikeExpression(pars["aluno"].ToString()));
                }
            }

            if (this.HasValue(pars, "nome"))
            {
                sql.Append(" AND r.nome_compl LIKE ? ");
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