using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Techne.Data;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using Seeduc.Infra.Data;
using System;

namespace Techne.Lyceum.RN.Query
{
    public class QueryMatriculaMovimentacao : LyceumQuery
    {
        public QueryMatriculaMovimentacao()
            : base()
        {
            //Colunas da grid da TSearch
            this.GridColumns.Add(new TSearchColumn() { FieldName = "matricula", Caption = "Matrícula", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "pessoa", Caption = "Código", Width = Unit.Percentage(12) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "nome", Caption = "Nome", Width = Unit.Percentage(28) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "rg_num", Caption = "Documento", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "cpf", Caption = "CPF", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "mae", Caption = "Nome da Mãe", Width = Unit.Percentage(20) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "pai", Caption = "Nome do Pai", Width = Unit.Percentage(20) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "fone", Caption = "fone", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "celular", Caption = "cel", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "e_mail_interno", Caption = "email_interno", Width = Unit.Percentage(0), Visible = false });

            this.Messages.KeyNotFound = "Matrícula inválida"; //Mensagem para chave não encontrada
            this.TextField = "matricula"; //chave
            this.DescriptionField = "nome"; //descrição

            //Filtros disponíveis na TSearch
            this.GridFilterParameters.Add("matricula", "Matrícula", TSearchDataType.String, 19);
            this.GridFilterParameters.Add("pessoa", "Código", TSearchDataType.String, 19);
            this.GridFilterParameters.Add("nome", "Nome", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("rg_num", "Documento", TSearchDataType.String, 25);
            this.GridFilterParameters.Add("cpf", "CPF", TSearchDataType.String, 19);

            this.MaxRows = 100; //máximo de linhas
            this.GridWidth = Unit.Pixel(800); //largura da grid
            this.TextFieldType = TSearchDataType.String; //tipo da chave
            this.MaxLength = 10; //tamanho máximo da chave
        }


        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            DataTable dt = null;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            DataView dv = new DataView();
            var contextQuery = new ContextQuery();
            var contextQueryPar = new ContextQuery();
            RN.Usuarios rnUsuarios = new Usuarios();
            RN.Perfil rnPerfil = new Perfil();

            try
            {
                if (key != null)
                {
                    contextQueryPar.Command += " and p.matricula = = @MATRICULA ";
                    contextQuery.Parameters.Add("@MATRICULA ", key.ToString());
                }
                else if (pars.ContainsKey("matricula") && pars["matricula"] != null && pars["matricula"].ToString().Trim().Length > 0)
                {
                    contextQueryPar.Command += " AND convert(varchar, p.matricula ) like @matricula ";
                    contextQuery.Parameters.Add("@matricula ", this.LikeExpression(pars["matricula"].ToString()));
                }

                if (pars.ContainsKey("pessoa") && pars["pessoa"] != null && pars["pessoa"].ToString().Trim().Length > 0)
                {
                    contextQueryPar.Command += " AND convert(varchar, p.pessoa) like @pessoa ";
                    contextQuery.Parameters.Add("@pessoa ", this.LikeExpression(pars["pessoa"].ToString()));
                }

                if (pars.ContainsKey("nome") && pars["nome"] != null && pars["nome"].ToString().Trim().Length > 0)
                {
                    contextQueryPar.Command += " AND convert(varchar,  p.nome_compl) like @nome ";
                    contextQuery.Parameters.Add("@nome ", this.LikeExpression(pars["nome"].ToString()));
                }
                if (pars.ContainsKey("rg_num") && pars["rg_num"] != null && pars["rg_num"].ToString().Trim().Length > 0)
                {
                    contextQueryPar.Command += " AND convert(varchar,  p.rg_num) like @rg_num ";
                    contextQuery.Parameters.Add("@rg_num ", this.LikeExpression(pars["rg_num"].ToString().RetirarMascaraRG()));
                }
                if (pars.ContainsKey("cpf") && pars["cpf"] != null && pars["cpf"].ToString().Trim().Length > 0)
                {
                    contextQueryPar.Command += " AND convert(varchar,  p.cpf) like @cpf ";
                    contextQuery.Parameters.Add("@cpf ", this.LikeExpression(pars["cpf"].ToString().RetirarMascaraCPF()));
                }
                if (rnUsuarios.EhUsuarioRegional(System.Threading.Thread.CurrentPrincipal.Identity.Name))
                {
                    DataTable dtPerfil = rnPerfil.ObtemListaPerfilPor(System.Threading.Thread.CurrentPrincipal.Identity.Name);

                    if (dtPerfil.Select("perfil ='" + RN.RNBase.MudarAspas("REGIONAL") + "'").Length > 0)
                    {
                        contextQueryPar.Command += @" AND EXISTS (SELECT UNIDADE_FIS 
                                           FROM   LY_USUARIO_UNIDADE_FIS USUUNI WITH (NOLOCK) 
                                           WHERE  USUUNI.USUARIO = @USUARIO
                                                  AND USUUNI.UNIDADE_FIS = P.UNIDADE_ENS) ";
                        contextQuery.Parameters.Add("@USUARIO ", System.Threading.Thread.CurrentPrincipal.Identity.Name);
                    }
                }

                contextQuery.Command += @" SELECT DISTINCT TOP ( @maxRows ) P.MATRICULA, 
                                                            P.PESSOA, 
                                                            P.NOME_COMPL AS NOME, 
                                                            P.RG_NUM, 
                                                            P.CPF, 
                                                            P.FONE, 
                                                            P.CELULAR, 
                                                            P.E_MAIL_INTERNO, 
                                                            P.NOME_MAE AS MAE, 
                                                            P.NOME_PAI AS PAI 
                                            FROM            VW_FUNCIONARIOS P 
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

                dv.Sort = "nome ASC";
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




