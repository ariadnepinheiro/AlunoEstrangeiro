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
using System.Web;

namespace Techne.Lyceum.RN.Query
{
    public class QuerySetorCedente : LyceumQuery
    {
        public QuerySetorCedente()
            : base()
        {
            //Colunas da grid da TSearch
            this.GridColumns.Add(new TSearchColumn() { FieldName = "ua_antiga", Caption = "U.A. Antiga", Width = Unit.Percentage(30) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "nome", Caption = "Nome", Width = Unit.Percentage(70) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "ua_atual", Caption = "U.A.", Width = Unit.Percentage(70) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "setor", Caption = "", Width = Unit.Percentage(30) });

            this.Messages.KeyNotFound = "Unidade Administrativa inválido"; //Mensagem para chave não encontrada
            this.TextField = "ua_atual"; //chave
            this.DescriptionField = "nome"; //descrição

            //Filtros disponíveis na TSearch
            this.GridFilterParameters.Add("ua_atual", "Unidade Administrativa", TSearchDataType.String, 19);
            this.GridFilterParameters.Add("nome", "Nome", TSearchDataType.String, 50);
    

            this.MaxRows = 100; //máximo de linhas
            this.GridWidth = Unit.Pixel(500); //largura da grid
            this.TextFieldType = TSearchDataType.String; //tipo da chave
            this.MaxLength = 15; //tamanho máximo da chave            
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
                    contextQueryPar.Command += "AND S.UA_ATUAL = @SETOR ";
                    contextQuery.Parameters.Add("@SETOR ", key.ToString());
                }
                else if (pars.ContainsKey("setor") && pars["setor"] != null && pars["setor"].ToString().Trim().Length > 0)
                {
                    contextQueryPar.Command += "AND  S.UA_ATUAL  like @SETOR ";
                    contextQuery.Parameters.Add("@SETOR ", this.LikeExpression(pars["setor"].ToString()));
                }

                if (pars.ContainsKey("nome") && pars["nome"] != null && pars["nome"].ToString().Trim().Length > 0)
                {
                    contextQueryPar.Command += "AND S.NOMESETOR like @NOME ";
                    contextQuery.Parameters.Add("@NOME ", "%" + this.LikeExpression(pars["nome"].ToString()));
                }
               
                contextQuery.Command += @" SELECT DISTINCT TOP ( @maxRows ) S.SETOR, 
                                                           S.NOMESETOR as NOME,
                                                            S.UA_ATUAL,
                                                            S.UA_ANTIGA
                                            FROM            HADES..VW_SETOR S";

                if (!rnPerfil.PossuiPerfilCoordenacaoPatrimonioPor(System.Threading.Thread.CurrentPrincipal.Identity.Name) && !rnUsuarios.EhPrivilegiado(HttpContext.Current.User.Identity.Name))
                {
                    contextQueryPar.Command += @"AND (EXISTS ( SELECT TOP 1
                                                      ar.SETOR
                                            FROM      [LYCEUM].[Patrimonio].[AGENTERESPONSAVEL] AR WITH (NOLOCK)
                                            INNER JOIN VW_FUNCIONARIOS F ON F.MATRICULA = AR.MATRICULA
                                            WHERE     AR.SETOR = S.SETOR
                                                     --AND AR.MATRICULA = @MATRICULA
                                                     AND (F.IDVINCULO_MATRICULA = @MATRICULA or F.MATRICULA=@MATRICULA )
                                                     AND AR.DATANOMEACAO <= GETDATE() 
                                                     AND ( AR.DATADISPENSA IS NULL 
                                                          OR AR.DATADISPENSA >= GETDATE() ) 
                                                      )) ";
                    contextQuery.Parameters.Add("@MATRICULA ", System.Threading.Thread.CurrentPrincipal.Identity.Name);
                }

                if (contextQueryPar.Command != null)
                {
                    contextQuery.Command += @" WHERE ";

                    contextQuery.Command += contextQueryPar.Command;

                    contextQuery.Command = contextQuery.Command.Replace("WHERE AND", " WHERE ");
                }

                contextQuery.Parameters.Add("@maxRows", maxRows);

                dt = ctx.GetDataTable(contextQuery);

                dv = dt.DefaultView;

                dv.Sort = " NOME ASC";
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





