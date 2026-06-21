namespace Techne.Lyceum.RN.Query
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web.UI.WebControls;
    using Data;
    using Techne.Web;
    using Seeduc.Infra.Data;
    using System;
    using Seeduc.Infra.Validation;
    using Techne.Lyceum.RN.Servicos;
    using Seeduc.Infra.Extensions;
    using System.Linq;
    using Techne.Exceptions;
    using System.Web;

    public class QueryAlunoCertificacaoNaoCadastrado : LyceumQuery
    {
        public QueryAlunoCertificacaoNaoCadastrado()
        {           

            this.GridColumns.Add(new TSearchColumn { FieldName = "nome", Caption = "Nome", Width = Unit.Percentage(35) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "cpf", Caption = "CPF", Width = Unit.Percentage(15) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "mae", Caption = "Nome da Mãe", Width = Unit.Percentage(30) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "pai", Caption = "Nome do Pai", Width = Unit.Percentage(30) });

            this.Messages.KeyNotFound = "CPF inválido";
            this.TextField = "cpf";
            this.MaxLength = 11;
            this.DescriptionField = "nome";

            this.GridFilterParameters.Add("cpf", "CPF", TSearchDataType.String, 20);
            this.GridFilterParameters.Add("nome", "Nome", TSearchDataType.String, 100);           
            this.GridFilterParameters.Add("mae", "Mãe", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("pai", "Pai", TSearchDataType.String, 100);

            this.MaxRows = 50;
            this.GridWidth = Unit.Pixel(860);
        }

        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            DataTable dt = null;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            List<string> mensagens = new List<string>();
            Validacao rnValidacao = new Validacao();

            try
            {
                var contextQuery = new ContextQuery();


                contextQuery.Command = @" SELECT DISTINCT TOP (@maxRows)
                                                      NOME      AS NOME,
	                                                  NOMEPAI   AS PAI,
                                                      NOMEMAE   AS MAE,
                                                      CPF
                                                FROM CertificacaoEscolar.ALUNOCERTIFICACAO
                                                WHERE 1=1
                                        ";


                contextQuery.Parameters.Add("@maxRows", maxRows);

                if (key != null)
                {
                    contextQuery.Command += " AND CPF = @CPF ";
                    contextQuery.Parameters.Add("@CPF", key.ToString());
                }
                else
                {
                    if (this.HasValue(pars, "cpf"))
                    {
                        contextQuery.Command += " AND CPF LIKE @CPF ";
                        contextQuery.Parameters.Add("@CPF ", this.LikeExpression(pars["cpf"].ToString()));
                    }

                    if (this.HasValue(pars, "nome"))
                    {
                        contextQuery.Command += " AND NOME LIKE @NOME ";
                        contextQuery.Parameters.Add("@NOME", this.LikeExpression(pars["nome"].ToString()));
                    }

                    if (this.HasValue(pars, "mae"))
                    {
                        contextQuery.Command += " AND NOMEMAE LIKE @MAE  ";
                        contextQuery.Parameters.Add("@MAE", this.LikeExpression(pars["mae"].ToString()));
                    }

                    if (this.HasValue(pars, "pai"))
                    {
                        contextQuery.Command += " AND NOMEPAI LIKE @PAI";
                        contextQuery.Parameters.Add("@PAI", this.LikeExpression(pars["pai"].ToString()));
                    }
                }

                dt = ctx.GetDataTable(contextQuery);
            }

            catch (GetDataException)
            {
                ctx.Abandon();
                string mensagem = mensagens.Aggregate((x, y) => x + "<br/>" + y);
                throw new GetDataException(mensagem);
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