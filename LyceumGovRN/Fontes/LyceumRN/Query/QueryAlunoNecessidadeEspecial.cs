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

    public class QueryAlunoNecessidadeEspecial : LyceumQuery
    {
        public QueryAlunoNecessidadeEspecial()
        {
            this.GridColumns.Add(new TSearchColumn { FieldName = "aluno", Caption = "Matrícula", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "nome", Caption = "Nome", Width = Unit.Percentage(20) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "cpf", Caption = "CPF", Width = Unit.Percentage(8) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "mae", Caption = "Nome da Mãe", Width = Unit.Percentage(18) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "pai", Caption = "Nome do Pai", Width = Unit.Percentage(18) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "dt_nascimento", Caption = "Nascimento", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "pessoa", Caption = "pessoa", Width = Unit.Percentage(0), Visible = false });

            this.Messages.KeyNotFound = "Aluno não encontrado";
            this.TextField = "aluno";
            this.MaxLength = 20;
            this.DescriptionField = "nome";

            this.GridFilterParameters.Add("nome", "Nome", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("aluno", "Matrícula", TSearchDataType.String, 20);
            this.GridFilterParameters.Add("mae", "Mãe", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("pai", "Pai", TSearchDataType.String, 100);


            var parametroBooleano = new TSearchParameter("fonetizar", "Fonetizar", TSearchDataType.Boolean) { DefaultValue = "unchecked" };
            this.GridFilterParameters.Add(parametroBooleano);

            this.GridFilterParameters.Add(new TSearchParameter { Caption = "ativo", ParameterName = "ativo", ShowInFilterPanel = false, ParameterType = TSearchDataType.String });
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
                        contextQueryPar.Command += " AND a.aluno LIKE @ALUNO ";
                        contextQuery.Parameters.Add("@ALUNO ", this.LikeExpression(pars["aluno"].ToString()));
                    }
                }
                if (this.HasValue(pars, "nome"))
                {
                    mensagens.AddRange(rnValidacao.ValidaBuscaFoneticaAlunoNovo(pars["nome"].ToString(), "NOME DO ALUNO"));
                }

                if (this.HasValue(pars, "mae"))
                {
                    mensagens.AddRange(rnValidacao.ValidaBuscaFoneticaAlunoNovo(pars["mae"].ToString(), "NOME DA MÃE"));
                }

                if (this.HasValue(pars, "pai"))
                {
                    mensagens.AddRange(rnValidacao.ValidaBuscaFoneticaAlunoNovo(pars["pai"].ToString(), "NOME DO PAI"));
                }

                if (mensagens.Count != 0)
                {
                    throw new GetDataException();
                }

                if (this.HasValue(pars, "fonetizar") && pars["fonetizar"].ToString() == "true")
                {
                    if (this.HasValue(pars, "nome"))
                    {
                        contextQueryPar.Command += " AND A.PESSOA IN (SELECT REGISTROID FROM dbo.FN_BUSCAFONETICA('LY_PESSOA','NOME_COMPL',@NOME_COMPL))  ";
                        contextQuery.Parameters.Add("@NOME_COMPL ", pars["nome"].ToString());
                    }

                    if (this.HasValue(pars, "mae"))
                    {
                        contextQueryPar.Command += " AND A.PESSOA IN (SELECT REGISTROID FROM dbo.FN_BUSCAFONETICA('LY_PESSOA','NOME_MAE',@NOME_MAE))  ";
                        contextQuery.Parameters.Add("@NOME_MAE", pars["mae"].ToString());
                    }

                    if (this.HasValue(pars, "pai"))
                    {
                        contextQueryPar.Command += " AND A.PESSOA IN (SELECT REGISTROID FROM dbo.FN_BUSCAFONETICA('LY_PESSOA','NOME_PAI',@NOME_PAI))  ";
                        contextQuery.Parameters.Add("@NOME_PAI ", pars["pai"].ToString());
                    }

                }
                else
                {
                    if (this.HasValue(pars, "nome"))
                    {
                        contextQueryPar.Command += " AND P.nome_compl LIKE @NOME_COMPL ";
                        contextQuery.Parameters.Add("@NOME_COMPL ", this.LikeExpression(pars["nome"].ToString()));
                    }

                    if (this.HasValue(pars, "mae"))
                    {
                        contextQueryPar.Command += "  AND p.nome_mae LIKE @MAE  ";
                        contextQuery.Parameters.Add("@MAE", this.LikeExpression(pars["mae"].ToString()));
                    }

                    if (this.HasValue(pars, "pai"))
                    {
                        contextQueryPar.Command += " AND p.nome_pai LIKE @PAI";
                        contextQuery.Parameters.Add("@PAI", this.LikeExpression(pars["pai"].ToString()));
                    }

                }
                if (this.HasValue(pars, "ativo"))
                {
                    contextQueryPar.Command += "AND a.SIT_ALUNO  = @SIT_ALUNO ";
                    contextQuery.Parameters.Add("@SIT_ALUNO", pars["ativo"].ToString());
                }

           

                contextQuery.Command = @"DECLARE @USUARIO          VARCHAR(15) = @USUARIOLOG
                                                  , @PRIVILEGIO       CHAR(1)     

                                            SELECT @PRIVILEGIO = U.PRIVIL 
                                              FROM HADES..USUARIO U (NOLOCK) 
                                             WHERE USUARIO = @USUARIO

                                            SELECT U.USUARIO
                                                 , UUF.UNIDADE_FIS     
                                              INTO #USUARIO 
                                              FROM HADES..USUARIO U (NOLOCK)  
                                              LEFT JOIN LY_USUARIO_UNIDADE_FIS UUF (NOLOCK)
                                                ON U.USUARIO = UUF.USUARIO
                                              WHERE U.USUARIO = @USUARIO 
                                              
      
                                               SELECT DISTINCT TOP (@maxRows)
                                                      A.ALUNO
                                                    , A.NUMINSCRICAO
                                                    , P.NOME_PAI                               AS PAI
                                                    , P.NOME_MAE                               AS MAE
                                                    , P.PESSOA
                                                    , P.NOME_COMPL                             AS NOME
                                                    , P.RG_NUM
                                                    , P.CPF
                                                    , CONVERT(VARCHAR(10), P.DT_NASC, 103)     AS DT_NASCIMENTO
          
                                               FROM #USUARIO  U (NOLOCK),
                                                    LY_ALUNO A (NOLOCK)
                                                    INNER JOIN LY_PESSOA                P (NOLOCK) ON P.PESSOA = A.PESSOA
                                         ";



                contextQuery.Command += " WHERE U.USUARIO = @USUARIO AND P.NECESSIDADEESPECIALID <> 30";
                contextQuery.Command += contextQueryPar.Command;

                contextQuery.Parameters.Add("@maxRows", maxRows);
                contextQuery.Parameters.Add("@USUARIOLOG", System.Threading.Thread.CurrentPrincipal.Identity.Name);

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