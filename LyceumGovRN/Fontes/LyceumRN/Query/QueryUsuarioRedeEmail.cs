using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Techne.Data;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using System.Web;
using Seeduc.Infra.Data;
using Techne.Exceptions;
using System;
using System.Linq;


namespace Techne.Lyceum.RN.Query
{
    public class QueryUsuarioRedeEmail : LyceumQuery
    {
        public QueryUsuarioRedeEmail()
        {
            this.GridColumns.Add(new TSearchColumn() { FieldName = "matricula", Caption = "Matrícula", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "IDVINCULO", Caption = "ID/Vínculo", Width = Unit.Percentage(18) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "nome", Caption = "Nome", Width = Unit.Percentage(24) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "cpf", Caption = "CPF", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "idvinculo_matricula", Caption = "idvinculo_matricula", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "pessoa", Caption = "Pessoa", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "email_interno", Caption = "E-mail Office", Width = Unit.Percentage(24) });

            this.Messages.KeyNotFound = "servidor inválido ou sem permissão de visualização.";
            this.TextField = "idvinculo_matricula";
            this.DescriptionField = "nome";
            this.GridFilterParameters.Add("nome", "Nome", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("matricula", "Matrícula", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("email_interno", "E-mail Office", TSearchDataType.String, 25);
            this.GridFilterParameters.Add("cpf", "CPF", TSearchDataType.String, 19);
            this.GridFilterParameters.Add("idvinculo", "ID/Vínculo", TSearchDataType.String, 15);
            this.MaxRows = 50;
            this.MaxLength = 100;
            this.GridWidth = Unit.Pixel(800);
        }

        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {

            StringBuilder sql = new StringBuilder();
            var contextQuery = new ContextQuery();
            var contextQueryPar = new ContextQuery();
            DataTable dt = null;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            List<string> mensagens = new List<string>();
            Validacao rnValidacao = new Validacao();
            DataView dv = new DataView();

            try
            {
                contextQuery.Command += @"SELECT DISTINCT TOP ( @maxRows )  P.matricula,
                                                                P.NOME_COMPL AS nome,
                                                                P.CPF,
                                                                P.PESSOA,
                                                                P.NOME_MAE   AS mae,
                                                                P.NOME_PAI   AS pai,
                                                                P.IDFUNCIONAL,
                                                                P.E_MAIL as EMAIL_ALTERNATIVO,
                                                                P.E_MAIL_INTERNO AS email_interno,
			                                                    GE.EMAIL AS EMAIL_GOOGLE,
                                                                IDVINCULO,
                                                                IDVINCULO_MATRICULA
                               FROM  VW_FUNCIONARIOS P
		                       LEFT JOIN RECURSOSHUMANOS.GOOGLEEDUCATION GE ON GE.PESSOA = P.PESSOA  
                               WHERE  ( P.DATA_DESATIVACAO IS NULL
                                                 OR CONVERT(DATE, P.DATA_DESATIVACAO) >
                                                    CONVERT(DATE, GETDATE()) )  ";


                if (key != null)
                {
                    contextQuery.Command += " and p.IDVINCULO_MATRICULA = @idvinculo_matricula ";
                    contextQuery.Parameters.Add("@idvinculo_matricula", key.ToString());
                }
                else
                {
                    if (pars.ContainsKey("idvinculo_matricula") && pars["idvinculo_matricula"] != null && pars["idvinculo_matricula"].ToString().Trim().Length > 0)
                    {
                        contextQuery.Command += " AND p.IDVINCULO_MATRICULA LIKE @idvinculo_matricula ";
                        contextQuery.Parameters.Add("@idvinculo_matricula", this.LikeExpression(pars["idvinculo_matricula"].ToString()));
                    }

                    if (pars.ContainsKey("nome") && pars["nome"] != null && pars["nome"].ToString().Trim().Length > 0)
                    {
                        contextQuery.Command += " AND p.nome_compl LIKE @nome ";
                        contextQuery.Parameters.Add("@nome", this.LikeExpression(pars["nome"].ToString()));
                    }

                    if (pars.ContainsKey("email") && pars["email"] != null && pars["email"].ToString().Trim().Length > 0)
                    {
                        contextQuery.Command += " and ( P.E_MAIL like @email  OR P.E_MAIL_INTERNO like @email OR GE.EMAIL like @email ) ";
                        contextQuery.Parameters.Add("@email", this.LikeExpression(pars["email"].ToString()));
                    }

                    if (pars.ContainsKey("cpf") && pars["cpf"] != null && pars["cpf"].ToString().Trim().Length > 0)
                    {
                        contextQuery.Command += " AND p.cpf LIKE @cpf ";
                        string cpf = LikeExpression(pars["cpf"].ToString().RetirarMascaraCPF());
                        contextQuery.Parameters.Add("@cpf", cpf);
                    }

                    if (pars.ContainsKey("matricula") && pars["matricula"] != null && pars["matricula"].ToString().Trim().Length > 0)
                    {
                        contextQuery.Command += " AND p.matricula LIKE @matricula ";
                        contextQuery.Parameters.Add("@matricula", this.LikeExpression(pars["matricula"].ToString()));
                    }
                    if (pars.ContainsKey("email_interno") && pars["email_interno"] != null && pars["email_interno"].ToString().Trim().Length > 0)
                    {
                        contextQuery.Command += " AND P.E_MAIL_INTERNO LIKE @email_interno ";
                        contextQuery.Parameters.Add("@email_interno", this.LikeExpression(pars["email_interno"].ToString()));
                    }

                    if (pars.ContainsKey("idvinculo") && pars["idvinculo"] != null && pars["idvinculo"].ToString().Trim().Length > 0)
                    {
                        contextQuery.Command += " AND p.idvinculo LIKE @idvinculo ";
                        contextQuery.Parameters.Add("@idvinculo", this.LikeExpression(pars["idvinculo"].ToString()));
                    }
                }

                contextQuery.Parameters.Add("@maxRows", MaxRows);

                dt = ctx.GetDataTable(contextQuery);

                dv = dt.DefaultView;

                dv.Sort = " nome ASC";


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
