namespace Techne.Lyceum.RN.Query
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web.UI.WebControls;
    using Data;
    using Techne.Web;
    using System;
    using Seeduc.Infra.Data;
    using Techne.Exceptions;
    using System.Linq;
    using Techne.Lyceum.RN.Util;

    public class QueryAlunoTransf : LyceumQuery
    {
        public QueryAlunoTransf()
        {
            this.GridColumns.Add(new TSearchColumn { FieldName = "aluno", Caption = "Matrícula", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "nome", Caption = "Nome", Width = Unit.Percentage(20) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "cpf", Caption = "CPF", Width = Unit.Percentage(8) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "mae", Caption = "Nome da Mãe", Width = Unit.Percentage(18) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "pai", Caption = "Nome do Pai", Width = Unit.Percentage(18) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "dt_nascimento", Caption = "Nascimento", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "pessoa", Caption = "pessoa", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn { FieldName = "unidade_ensino", Caption = "Censo", Width = Unit.Percentage(8) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "escola", Caption = "Escola", Width = Unit.Percentage(18) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "sit_aluno", Caption = "Situação", Width = Unit.Percentage(15) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "municipio", Caption = "municipio", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn { FieldName = "necessidade_especial", Caption = "necessidade_especial", Width = Unit.Percentage(0), Visible = false });

            this.Messages.KeyNotFound = "Matrícula inválida";
            this.TextField = "aluno";
            this.MaxLength = 20;
            this.DescriptionField = "nome";

            this.GridFilterParameters.Add("nome", "Nome", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("aluno", "Matrícula", TSearchDataType.String, 20);
            this.GridFilterParameters.Add("mae", "Mãe", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("pai", "Pai", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("unidade_ensino", "Censo", TSearchDataType.Integer, 8);
            this.GridFilterParameters.Add("sit_aluno", "Situação", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("cpf", "CPF", TSearchDataType.String, 20);
            var parametroBooleano = new TSearchParameter("fonetizar", "Fonetizar", TSearchDataType.Boolean) { DefaultValue = "unchecked" };
            this.GridFilterParameters.Add(parametroBooleano);

            this.MaxRows = 50;
            this.GridWidth = Unit.Pixel(960);
        }

        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            DataTable dt = null;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            List<string> mensagens = new List<string>();
            Validacao rnValidacao = new Validacao();
            DataView dv = new DataView();

            try
            {
                var contextQuery = new ContextQuery();
                var contextQueryPar = new ContextQuery();
                var contextQueryTemp = new ContextQuery();
                var contextQueryJoin = new ContextQuery();
                var contextQueryParPessoa = new ContextQuery();

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
                        contextQueryTemp.Command += @" 
                                                        SELECT  REGISTROID 
                                                        INTO    #NOME_ALUNO 
                                                        FROM    dbo.FN_BUSCAFONETICA('LY_PESSOA', 'NOME_COMPL', @NOME_COMPL)  ";
                        contextQuery.Parameters.Add("@NOME_COMPL ", pars["nome"].ToString());
                        contextQueryJoin.Command += @"  
                                                       INNER JOIN #NOME_ALUNO TA ON TA.REGISTROID = A.PESSOA";
                    }

                    if (this.HasValue(pars, "mae"))
                    {
                        contextQueryTemp.Command += @"  
                                                       SELECT  REGISTROID 
                                                        INTO    #NOME_MAE 
                                                       FROM    dbo.FN_BUSCAFONETICA('LY_PESSOA', 'NOME_MAE', @NOME_MAE)  ";
                        contextQuery.Parameters.Add("@NOME_MAE", pars["mae"].ToString());
                        contextQueryJoin.Command += @"  
                                                        INNER JOIN #NOME_MAE TM ON TM.REGISTROID = A.PESSOA";
                    }

                    if (this.HasValue(pars, "pai"))
                    {
                        contextQueryTemp.Command += @"  
                                                        SELECT  REGISTROID 
                                                         INTO    #NOME_PAI 
                                                        FROM    dbo.FN_BUSCAFONETICA('LY_PESSOA', 'NOME_PAI', @NOME_PAI)  ";
                        contextQuery.Parameters.Add("@NOME_PAI ", pars["pai"].ToString());
                        contextQueryJoin.Command += @"  
                                                        INNER JOIN #NOME_PAI TP ON TP.REGISTROID = A.PESSOA";

                    }

                }
                else
                {
                    if (this.HasValue(pars, "nome"))
                    {
                        contextQueryParPessoa.Command += " AND P.nome_compl LIKE @NOME_COMPL ";
                        contextQuery.Parameters.Add("@NOME_COMPL ", this.LikeExpression(pars["nome"].ToString()));
                    }

                    if (this.HasValue(pars, "mae"))
                    {
                        contextQueryParPessoa.Command += " AND p.nome_mae LIKE @MAE  ";
                        contextQuery.Parameters.Add("@MAE", this.LikeExpression(pars["mae"].ToString()));
                    }

                    if (this.HasValue(pars, "pai"))
                    {
                        contextQueryParPessoa.Command += " AND p.nome_pai LIKE @PAI";
                        contextQuery.Parameters.Add("@PAI", this.LikeExpression(pars["pai"].ToString()));
                    }

                }
                if (this.HasValue(pars, "sit_aluno"))
                {
                    contextQueryPar.Command += " AND a.SIT_ALUNO  = @SIT_ALUNO ";
                    contextQuery.Parameters.Add("@SIT_ALUNO", pars["sit_aluno"].ToString());
                }

                if (this.HasValue(pars, "cpf"))
                {
                    string cpf = pars["cpf"].ToString();
                    contextQueryParPessoa.Command += " AND p.cpf  = @CPF ";
                    contextQuery.Parameters.Add("@CPF", cpf.RetirarMascaraCPF());
                }

                if (this.HasValue(pars, "unidade_ensino"))
                {
                    contextQueryPar.Command += " AND a.unidade_ensino = @unidade_ensino ";
                    contextQuery.Parameters.Add("@UNIDADE_ENSINO", pars["unidade_ensino"].ToString());
                }

                if (key == null)
                {
                    if (this.HasValue(pars, "fonetizar") && pars["fonetizar"].ToString() == "true")
                    {
                        contextQuery.Command += contextQueryTemp.Command;
                    }
                    else
                    {
                        if (contextQueryParPessoa.Command != null)
                        {
                            contextQuery.Command += @"
                                            SELECT DISTINCT 
                                                p.nome_pai AS pai ,
                                                p.nome_mae AS mae ,
                                                p.pessoa ,
                                                p.nome_compl AS nome ,
                                                p.rg_num ,
                                                p.cpf ,
                                                CONVERT(VARCHAR(10), p.DT_NASC, 103) AS dt_nascimento ,
                                                NEC.DESCRICAO AS necessidade_especial
                                        INTO    #PESSOA
                                        FROM    dbo.LY_PESSOA P                                      
                                        LEFT JOIN HADES.dbo.NECESSIDADEESPECIAL NEC ON NEC.NECESSIDADEESPECIALID=P.NECESSIDADEESPECIALID

                                        ";

                            contextQuery.Command += @" WHERE";

                            contextQuery.Command += contextQueryParPessoa.Command;

                            contextQuery.Command = contextQuery.Command.Replace("WHERE AND", " WHERE ");
                        }

                    }
                }
                contextQuery.Command += @"
                                            SELECT DISTINCT TOP ( @maxRows )
                                                    A.PESSOA ,
                                                    a.aluno ,
                                                    a.numinscricao ,
                                                    a.sit_aluno ,
                                                    a.unidade_ensino ,
                                                    UE.NOME_COMP AS escola ,
                                                    UE.MUNICIPIO ,
                                                    M.NOME AS NOME_MUNICIPIO";

                if ((this.HasValue(pars, "fonetizar") && pars["fonetizar"].ToString() == "false") && contextQueryParPessoa.Command != null)
                {
                    contextQuery.Command += @" ,P.* ";
                }
                else
                {
                    contextQuery.Command += @" 
                                            INTO #ALUNO ";
                }

                contextQuery.Command += @"                                                    
                                            FROM    LY_ALUNO a WITH ( NOLOCK )		                                          
                                                    INNER JOIN LY_UNIDADE_ENSINO UE ON UE.UNIDADE_ENS = A.UNIDADE_ENSINO
                                                    INNER JOIN dbo.MUNICIPIO M ON M.CODIGO = UE.MUNICIPIO
                                        ";

                if (key == null)
                {
                    if (this.HasValue(pars, "fonetizar") && pars["fonetizar"].ToString() == "true")
                    {
                        contextQuery.Command += contextQueryJoin.Command;
                    }
                    else
                    {
                        if (contextQueryParPessoa.Command != null)
                        {
                            contextQuery.Command += "  INNER JOIN #PESSOA P ON P.PESSOA=A.PESSOA ";
                        }
                    }
                }
                contextQuery.Command += @"
                                            LEFT JOIN LY_H_CURSOS_CONCL HC ON hc.ALUNO = a.ALUNO
                                                                              AND DT_REABERTURA IS NULL
                                                                              AND MOTIVO = 'OBITO' 
                                            WHERE   HC.ALUNO IS NULL
                                        ";


                contextQuery.Command += contextQueryPar.Command;

                if ((this.HasValue(pars, "fonetizar") && pars["fonetizar"].ToString() == "true") || key != null || contextQueryParPessoa.Command == null)
                {
                    contextQuery.Command += @"
                                            SELECT DISTINCT TOP ( @maxRows )
                                                    a.aluno ,
                                                    a.numinscricao ,
                                                    a.sit_aluno ,
                                                    a.unidade_ensino ,
                                                    a.escola ,
                                                    a.MUNICIPIO ,
                                                    a.NOME_MUNICIPIO ,
                                                    p.nome_pai AS pai ,
                                                    p.nome_mae AS mae ,
                                                    p.pessoa ,
                                                    p.nome_compl AS nome ,
                                                    p.rg_num ,
                                                    p.cpf ,
                                                    CONVERT(VARCHAR(10), p.DT_NASC, 103) AS dt_nascimento ,
                                                    NEC.DESCRICAO AS necessidade_especial
                                            FROM    #ALUNO A
                                                    INNER JOIN LY_PESSOA p WITH ( NOLOCK ) ON p.PESSOA = a.PESSOA 
                                                    LEFT JOIN HADES.dbo.NECESSIDADEESPECIAL NEC ON NEC.NECESSIDADEESPECIALID=P.NECESSIDADEESPECIALID

                                          DROP TABLE #ALUNO  

                    ";

                    if (this.HasValue(pars, "nome"))
                    {
                        contextQuery.Command += @" 
                                                     DROP TABLE #NOME_ALUNO   ";
                    }
                    if (this.HasValue(pars, "mae"))
                    {
                        contextQuery.Command += @" 
                                                    DROP TABLE #NOME_MAE   ";
                    }
                    if (this.HasValue(pars, "pai"))
                    {
                        contextQuery.Command += @" 
                                                    DROP TABLE #NOME_PAI  ";
                    }
                }
                else
                {
                    contextQuery.Command += @" 

                                        DROP TABLE #PESSOA ";
                }

                contextQuery.Parameters.Add("@maxRows", maxRows);

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

            return dt.DefaultView;
        }
    }
}