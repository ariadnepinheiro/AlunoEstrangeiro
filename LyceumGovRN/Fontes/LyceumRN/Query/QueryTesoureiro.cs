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
    class QueryTesoureiro : LyceumQuery
    {
        public QueryTesoureiro()
        {
            this.GridColumns.Add(new TSearchColumn() { FieldName = "CPF", Caption = "CPF", Width = Unit.Percentage(10) });
			this.GridColumns.Add(new TSearchColumn() { FieldName = "NOME", Caption = "Nome", Width = Unit.Percentage(24) });


            this.Messages.KeyNotFound = "CPF inválido.";
            this.TextField = "CPF";
            this.DescriptionField = "NOME";
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
                    contextQueryPar.Command += " AND cpf = @CPF ";
                    contextQuery.Parameters.Add("@CPF ", key.ToString());
                }
                else
                {
                    if (this.HasValue(pars, "cpf"))
                    {
                        if (pars["cpf"].ToString().Length == 11)
                        {
                            contextQueryPar.Command += " AND cpf = @cpf ";
                            contextQuery.Parameters.Add("@cpf ", pars["cpf"].ToString());
                        }
                        else
                        {
                            contextQueryPar.Command += " AND cpf LIKE @cpf ";
                            contextQuery.Parameters.Add("@cpf ", this.LikeExpression(pars["cpf"].ToString()));
                        }
                    }
                }
                if (pars.ContainsKey("nome") && pars["nome"] != null && pars["nome"].ToString().Trim().Length > 0)
                {
                    contextQueryPar.Command += " AND convert(varchar,  nome) like @nome ";
                    contextQuery.Parameters.Add("@nome ", this.LikeExpression(pars["nome"].ToString()));
                }
                contextQuery.Command = @" SELECT DISTINCT TOP ( @maxRows ) 
                                                                                  [NOME]
                                                                                  ,[CPF]
                                                                                  ,[RG]
                                                                                  ,[ENDERECO]
                                                                                  ,[NUMERO]
                                                                                  ,[COMPLEMENTO]
                                                                                  ,[BAIRRO]
                                                                                  ,[MUNICIPIOID]
                                                                                  ,[CEP]
                                                                                  ,[EMAIL]
                                                                                  ,[TELEFONE]
                                                                                  ,CONVERT(VARCHAR, TESOUREIROID) as CODIGO
                                                         
                                               FROM   [PrestacaoContas].[TESOUREIRO] A (NOLOCK)
                                                       ";
                if (contextQueryPar.Command != null)
                {
                    contextQuery.Command += @" WHERE";

                    contextQuery.Command += contextQueryPar.Command;

                    contextQuery.Command = contextQuery.Command.Replace("WHERE AND", " WHERE ");
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

