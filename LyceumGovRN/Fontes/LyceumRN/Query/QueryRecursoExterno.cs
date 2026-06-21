using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Web;
using System.Web.UI.WebControls;
using System.Collections;
using Techne.Data;
using System.Data;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.Query
{
    class QueryRecursoExterno : LyceumQuery
    {
        public QueryRecursoExterno()
        {
            this.GridColumns.Add(new TSearchColumn() { FieldName = "cpf", Caption = "CPF", Width = Unit.Percentage(10) });
			this.GridColumns.Add(new TSearchColumn() { FieldName = "nome", Caption = "Nome", Width = Unit.Percentage(24) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "codigo", Caption = "Código", Width = Unit.Percentage(24) });  
            this.GridColumns.Add(new TSearchColumn() { FieldName = "pessoa", Caption = "Pessoa", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "USUARIOEXTERNOID", Caption = "USUARIOEXTERNOID", Width = Unit.Percentage(0), Visible = false });


            this.Messages.KeyNotFound = "CPF inválido.";
            this.TextField = "cpf";
            this.DescriptionField = "nome";
            this.GridFilterParameters.Add("cpf", "CPF", TSearchDataType.String, 19);
            this.GridFilterParameters.Add("nome", "Nome", TSearchDataType.String, 100);
            this.MaxRows = 100;
            this.MaxLength = 100;
            this.GridWidth = Unit.Pixel(800);
        }

        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            DataTable dt = null;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            DataView dv = new DataView();
            var contextQueryPar = new ContextQuery();

            try
            {
                var contextQuery = new ContextQuery();


                if (key != null)
                {
                    contextQueryPar.Command += " AND p.cpf = @CPF ";
                    contextQuery.Parameters.Add("@CPF ", key.ToString());
                }
                else
                {
                    if (this.HasValue(pars, "cpf"))
                    {
                        if (pars["cpf"].ToString().Length == 11)
                        {
                            contextQueryPar.Command += " AND p.cpf = @cpf ";
                            contextQuery.Parameters.Add("@cpf ", pars["cpf"].ToString());
                        }
                        else
                        {
                            contextQueryPar.Command += " AND p.cpf LIKE @cpf ";
                            contextQuery.Parameters.Add("@cpf ", this.LikeExpression(pars["cpf"].ToString()));
                        }
                    }
                }
                if (pars.ContainsKey("nome") && pars["nome"] != null && pars["nome"].ToString().Trim().Length > 0)
                {
                    contextQueryPar.Command += " AND convert(varchar,  p.nome_compl) like @nome ";
                    contextQuery.Parameters.Add("@nome ", this.LikeExpression(pars["nome"].ToString()));
                }
                contextQuery.Command = @" SELECT DISTINCT TOP ( @maxRows ) P.NOME_COMPL AS NOME,
                                                              P.CPF,
                                                              P.PESSOA,
                                                              CONVERT(VARCHAR,UE.USUARIOEXTERNOID) as CODIGO,
                                                              ue.USUARIOEXTERNOID
                                               FROM   LY_PESSOA P (NOLOCK)
											   INNER JOIN RecursosHumanos.USUARIOEXTERNO UE ON UE.PESSOAID = P.PESSOA
											   WHERE P.CPF IS NOT NULL
                                             ";
                if (contextQueryPar.Command != null)
                {
                    contextQuery.Command += contextQueryPar.Command;
                }

                contextQuery.Parameters.Add("@maxRows", maxRows);

                dt = ctx.GetDataTable(contextQuery);

                dv = dt.DefaultView;

                dv.Sort = "NOME ASC";

            }           
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }

            return dt.DefaultView;
        }
    }
}

