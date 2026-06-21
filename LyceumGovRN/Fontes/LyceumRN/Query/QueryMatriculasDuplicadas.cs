using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Exceptions;
using Techne.Web;
using System.Web.UI.WebControls;

namespace Techne.Lyceum.RN.Query
{
    class QueryMatriculasDuplicadas : LyceumQuery
    {
        public QueryMatriculasDuplicadas()
        {
            this.GridColumns.Add(new TSearchColumn { FieldName = "aluno", Caption = "Matrícula", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "nome", Caption = "Nome", Width = Unit.Percentage(20) });

            this.Messages.KeyNotFound = "Matrícula inválida";
            this.TextField = "aluno";
            this.MaxLength = 20;
            this.DescriptionField = "nome";

            this.GridFilterParameters.Add("nome", "Nome", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("aluno", "Matrícula", TSearchDataType.String, 20);

            this.MaxRows = 50;
            this.GridWidth = Unit.Pixel(860);
        }
        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            DataTable dt = null;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            List<string> mensagens = new List<string>();
            DataView dv = new DataView();

            try
            {
                var contextQuery = new ContextQuery();
                var contextQueryPar = new ContextQuery();

                if (key != null)
                {
                    contextQueryPar.Command += " AND a.aluno = @ALUNO ";
                    contextQuery.Parameters.Add("@ALUNO ", key.ToString());
                }
                else
                {
                    if (this.HasValue(pars, "aluno"))
                    {
                        if (pars["aluno"].ToString().Length == 15)
                        {
                            contextQueryPar.Command += " AND a.aluno = @ALUNO ";
                            contextQuery.Parameters.Add("@ALUNO ", pars["aluno"].ToString());
                        }
                        else
                        {
                            contextQueryPar.Command += " AND a.aluno LIKE @ALUNO ";
                            contextQuery.Parameters.Add("@ALUNO ", this.LikeExpression(pars["aluno"].ToString()));
                        }
                    }
                }

                if (this.HasValue(pars, "nome"))
                {
                    contextQueryPar.Command += " AND PE.nome_compl LIKE @NOME_COMPL ";
                    contextQuery.Parameters.Add("@NOME_COMPL ", this.LikeExpression(pars["nome"].ToString()));
                }
                contextQuery.Command = @"          DECLARE @USUARIO          VARCHAR(15) = @USUARIOLOG
                                                  , @PRIVILEGIO       CHAR(1)

                                            SELECT @PRIVILEGIO = U.PRIVIL 
                                              FROM HADES..USUARIO U (NOLOCK) 
                                             WHERE USUARIO = @USUARIO                                        
                                              
                                             
                                            IF (@PRIVILEGIO = 'S') BEGIN
                                                    
                                                    SELECT DISTINCT TOP ( @maxRows )
                                                        a.aluno ,
                                                        PE.nome_compl AS nome
                                                    FROM    LY_ALUNO A ( NOLOCK )
                                                    INNER JOIN LY_PESSOA PE (NOLOCK)  ON PE.PESSOA = A.PESSOA
                                                    JOIN [LYCEUM].[CartaoEstudante].[DUPLICIDADE] D ON D.ALUNO = A.ALUNO
                                                  
                                        ";
                if (contextQueryPar.Command != null)
                {
                    contextQuery.Command += @" WHERE";

                    contextQuery.Command += contextQueryPar.Command;

                    contextQuery.Command = contextQuery.Command.Replace("WHERE AND", " WHERE ");
                }


            contextQuery.Command += @"

                                            END 
                                            ELSE BEGIN

                                                SELECT U.USUARIO
                                                 , UUF.UNIDADE_FIS     
                                              INTO #USUARIO 
                                              FROM HADES..USUARIO U (NOLOCK)  
                                              INNER JOIN LY_USUARIO_UNIDADE_FIS UUF (NOLOCK)
                                                ON U.USUARIO = UUF.USUARIO
                                              WHERE U.USUARIO = @USUARIO 

                                                    SELECT DISTINCT TOP ( @maxRows )
                                                        a.aluno ,
                                                        PE.nome_compl AS nome
                                                    FROM    LY_ALUNO A ( NOLOCK )
                                                        INNER JOIN LY_PESSOA PE (NOLOCK)  ON PE.PESSOA = A.PESSOA
                                                        JOIN [LYCEUM].[CartaoEstudante].[DUPLICIDADE] D ON D.ALUNO = A.ALUNO
                                                        JOIN #USUARIO U ( NOLOCK ) ON U.UNIDADE_FIS = A.UNIDADE_FISICA
                                                    
                                                    ";


            if (contextQueryPar.Command != null)
            {
                contextQuery.Command += @" WHERE";

                contextQuery.Command += contextQueryPar.Command;

                contextQuery.Command = contextQuery.Command.Replace("WHERE AND", " WHERE ");
            }
                
                contextQuery.Command += @"
                                            DROP TABLE #USUARIO            

                                            END                                           

                                        ";
                contextQuery.Parameters.Add("@maxRows", maxRows);
                contextQuery.Parameters.Add("@USUARIOLOG", System.Threading.Thread.CurrentPrincipal.Identity.Name);

                dt = ctx.GetDataTable(contextQuery);

                dv = dt.DefaultView;

                dv.Sort = "nome ASC";

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
            
            return dv;
        }
    }
}
