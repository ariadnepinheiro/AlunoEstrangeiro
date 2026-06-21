using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Techne.Web;
using Seeduc.Infra.Data;
using System;
using Seeduc.Infra.Validation;
using Techne.Lyceum.RN.Servicos;
using Seeduc.Infra.Extensions;
using System.Linq;
using Techne.Exceptions;

namespace Techne.Lyceum.RN.Query
{
    public class QueryAlunoReabertura : LyceumQuery
    {
        public QueryAlunoReabertura()
        {
            this.GridColumns.Add(new TSearchColumn { FieldName = "aluno", Caption = "Matrícula", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "nome", Caption = "Nome", Width = Unit.Percentage(20) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "rg_num", Caption = "Documento", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "cpf", Caption = "CPF", Width = Unit.Percentage(8) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "mae", Caption = "Nome da Mãe", Width = Unit.Percentage(18) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "pai", Caption = "Nome do Pai", Width = Unit.Percentage(18) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "dt_nascimento", Caption = "Nascimento", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "pessoa", Caption = "pessoa", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn { FieldName = "numinscricao", Caption = "Inscrição Matrícula Fácil", Width = Unit.Percentage(18) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "necessidade_especial", Caption = "Necessidade especial", Width = Unit.Percentage(18), Visible = true });

            this.Messages.KeyNotFound = "Matrícula inválida";
            this.TextField = "aluno";
            this.MaxLength = 20;
            this.DescriptionField = "nome";

            this.GridFilterParameters.Add("nome", "Nome", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("aluno", "Matrícula", TSearchDataType.String, 20);
            this.GridFilterParameters.Add("mae", "Mãe", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("pai", "Pai", TSearchDataType.String, 100);

            this.GridFilterParameters.Add("numinscricao", "Inscrição Matrícula Fácil", TSearchDataType.String, 100);

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
            DataView dv = new DataView();

            try
            {
                var contextQuery = new ContextQuery();
                var contextQueryPar = new ContextQuery();
                var contextQueryJoin = new ContextQuery();
                var contextQueryTemp = new ContextQuery();
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
                        contextQueryParPessoa.Command += " AND p.nome_compl LIKE @NOME_COMPL ";
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
                if (this.HasValue(pars, "ativo"))
                {
                    contextQueryPar.Command += " AND a.SIT_ALUNO <> @SIT_ALUNO ";
                    contextQuery.Parameters.Add("@SIT_ALUNO", pars["ativo"].ToString());
                }

                if (this.HasValue(pars, "numinscricao"))
                {
                    contextQueryPar.Command += " AND a.Numinscricao = @NUMINSCRICAO ";
                    contextQuery.Parameters.Add("@NUMINSCRICAO", pars["numinscricao"].ToString());
                }


                contextQuery.Command = @"          DECLARE @USUARIO          VARCHAR(15) = @USUARIOLOG
                                                  , @PRIVILEGIO       CHAR(1)";

                if (this.HasValue(pars, "fonetizar") && pars["fonetizar"].ToString() == "true")
                {
                    contextQuery.Command += contextQueryTemp.Command;
                }

                contextQuery.Command += @"
                                            SELECT @PRIVILEGIO = U.PRIVIL 
                                              FROM HADES..USUARIO U (NOLOCK) 
                                             WHERE USUARIO = @USUARIO                                         
                                              
                                             
                                            IF (@PRIVILEGIO = 'S') BEGIN

                                               SELECT DISTINCT
                                                      A.ALUNO
                                                    , A.NUMINSCRICAO
                                                    , A.PESSOA
                                                INTO #ALUNO
                                               FROM LY_ALUNO A (NOLOCK)
                                               INNER JOIN LY_H_CURSOS_CONCL H ON H.ALUNO=A.ALUNO AND H.MOTIVO IN ('DUPLICIDADE','DUPLIC_SIS','OBITO') AND H.DT_REABERTURA IS NULL 
                                                                                                       
                                         ";

                if (this.HasValue(pars, "fonetizar") && pars["fonetizar"].ToString() == "true")
                {
                    contextQuery.Command += contextQueryJoin.Command;
                }

                if (contextQueryPar.Command != null)
                {
                    if (contextQuery.Command.Contains("WHERE AND"))
                    {
                        contextQuery.Command = contextQuery.Command.Replace("WHERE AND", " WHERE ");
                    }

                    contextQuery.Command += contextQueryPar.Command;

                }

                contextQuery.Command += @"
                                SELECT DISTINCT TOP (@maxRows)
                                        A.ALUNO ,
                                        A.NUMINSCRICAO ,
                                        P.NOME_PAI AS PAI ,
                                        P.NOME_MAE AS MAE ,
                                        P.PESSOA ,
                                        P.NOME_COMPL AS NOME ,
                                        P.RG_NUM ,
                                        P.CPF ,
                                        CONVERT(VARCHAR(10), P.DT_NASC, 103) AS DT_NASCIMENTO,
                                        NEC.DESCRICAO AS NECESSIDADE_ESPECIAL 
                                FROM    #ALUNO A
                                        INNER JOIN dbo.LY_PESSOA P ON A.PESSOA = P.PESSOA
                                        LEFT JOIN HADES.dbo.NECESSIDADEESPECIAL NEC ON NEC.NECESSIDADEESPECIALID=P.NECESSIDADEESPECIALID";


                if (contextQueryParPessoa.Command != null)
                {
                    contextQuery.Command += @" 
                                        WHERE";

                    if (contextQuery.Command.Contains("WHERE AND"))
                    {
                        contextQuery.Command = contextQuery.Command.Replace("WHERE AND", " WHERE ");
                    }

                    contextQuery.Command += contextQueryParPessoa.Command;
                }

                contextQuery.Command += @" 
                                        DROP TABLE #ALUNO

                                                END ELSE BEGIN
                                              

                                                    SELECT U.USUARIO
                                                         , UUF.UNIDADE_FIS     
                                                      INTO #USUARIO 
                                                      FROM HADES..USUARIO U (NOLOCK)  
                                                      INNER JOIN LY_USUARIO_UNIDADE_FIS UUF (NOLOCK)
                                                        ON U.USUARIO = UUF.USUARIO
                                                      WHERE U.USUARIO = @USUARIO 

                                    ";

                if ((this.HasValue(pars, "fonetizar") && pars["fonetizar"].ToString() == "false") && contextQueryParPessoa.Command != null)
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
                                        WHERE";
                    contextQuery.Command += contextQueryParPessoa.Command;

                }

                contextQuery.Command += @"
                                             
                                                       SELECT DISTINCT TOP (@maxRows)
                                                            a.aluno ,
                                                            a.numinscricao ,                                                          
                                                            a.pessoa                                                                       
                                                         INTO    #MATRICULAS_ESPECIAIS
                                                         FROM LY_ALUNO A (NOLOCK)
                                                         INNER JOIN LY_H_CURSOS_CONCL H ON H.ALUNO=A.ALUNO AND H.MOTIVO IN ('DUPLICIDADE','DUPLIC_SIS','OBITO') AND H.DT_REABERTURA IS NULL 
                                                         JOIN TCE_ALUNO_CONCOMITANTE AC ON AC.ALUNO = A.ALUNO
                                                         JOIN #USUARIO U (NOLOCK)
                                                           ON U.UNIDADE_FIS = AC.CENSO
                                                    ";

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

                contextQuery.Command += @" WHERE ac.STATUS = 'Liberado'
                                        AND ac.DT_CADASTRO = ( SELECT   MAX(acc.DT_CADASTRO)
                                                                FROM     TCE_ALUNO_CONCOMITANTE acc
                                                                WHERE    acc.ALUNO = ac.ALUNO
                                                                )

                                    ";

                contextQuery.Command += contextQueryPar.Command;

                contextQuery.Command += @"        

                                                    INSERT  INTO #MATRICULAS_ESPECIAIS
                                                    ( ALUNO ,
                                                      PESSOA ,
                                                      NUMINSCRICAO
                                                    )
                                                    SELECT DISTINCT TOP ( @maxRows )
                                                            a.aluno ,
                                                            A.pessoa ,
                                                            A.NUMINSCRICAO
                                                    FROM    LY_ALUNO A ( NOLOCK )
                                                            JOIN TCE_ALUNO_EDUC_ESPECIAL esp ON esp.ALUNO = A.ALUNO
                                                            INNER JOIN LY_H_CURSOS_CONCL H ON H.ALUNO=A.ALUNO AND H.MOTIVO IN ('DUPLICIDADE','DUPLIC_SIS','OBITO') AND H.DT_REABERTURA IS NULL
                                                            JOIN #USUARIO U ( NOLOCK ) ON U.UNIDADE_FIS = esp.CENSO 

                                                    ";
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

                contextQuery.Command += @"            
                                                        WHERE   esp.ACEITE = 1
                                                            AND esp.DT_CADASTRO = ( SELECT  MAX(espp.DT_CADASTRO)
                                                                                    FROM    TCE_ALUNO_EDUC_ESPECIAL espp
                                                                                    WHERE   espp.ALUNO = esp.ALUNO
                                                                                  )

                                                            ";

                contextQuery.Command += contextQueryPar.Command;

                contextQuery.Command +=
                    @"             
                                         INSERT  INTO #MATRICULAS_ESPECIAIS
                                        ( ALUNO ,
                                          PESSOA ,
                                          NUMINSCRICAO
                                        )
                                        SELECT DISTINCT TOP ( @maxRows )
                                                a.aluno ,
                                                A.pessoa ,
                                                A.NUMINSCRICAO
                                        FROM    LY_ALUNO A ( NOLOCK )
                                                JOIN #USUARIO U ( NOLOCK ) ON U.UNIDADE_FIS = A.UNIDADE_FISICA
                                                INNER JOIN LY_H_CURSOS_CONCL H ON H.ALUNO=A.ALUNO AND H.MOTIVO IN ('DUPLICIDADE','DUPLIC_SIS','OBITO') 

                ";

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
                contextQuery.Command += @"  
                                            
                                            LEFT JOIN #MATRICULAS_ESPECIAIS ME ON ME.aluno = a.ALUNO
                                            WHERE   ME.aluno IS NULL

                                        ";

                contextQuery.Command += contextQueryPar.Command;

                contextQuery.Command += @"               

                                    SELECT DISTINCT TOP ( @maxRows )
                                        ME.ALUNO ,
                                        ME.NUMINSCRICAO ,
                                        P.NOME_PAI AS PAI ,
                                        P.NOME_MAE AS MAE ,
                                        P.PESSOA ,
                                        P.NOME_COMPL AS NOME ,
                                        P.RG_NUM ,
                                        P.CPF ,
                                        CONVERT(VARCHAR(10), P.DT_NASC, 103) AS DT_NASCIMENTO,
                                        NEC.DESCRICAO AS NECESSIDADE_ESPECIAL 
                                FROM    #MATRICULAS_ESPECIAIS ME
                                        INNER JOIN dbo.LY_PESSOA P ON ME.PESSOA = P.PESSOA
                                        LEFT JOIN HADES.dbo.NECESSIDADEESPECIAL NEC ON NEC.NECESSIDADEESPECIALID=P.NECESSIDADEESPECIALID
        
                                DROP TABLE #MATRICULAS_ESPECIAIS                                                
                                DROP TABLE #USUARIO                    
                            END                      
                                                           
                        ";

                contextQuery.Command += @"  
                                                   ";

                if (this.HasValue(pars, "fonetizar") && pars["fonetizar"].ToString() == "true")
                {
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
                                                    IF EXISTS (SELECT TOP 1 * 
                                                                     FROM sys.objects 
                                                                    WHERE object_id = OBJECT_ID('#PESSOA') 
                                                                      AND type IN ('U'))
                                                    BEGIN
		                                                    DROP TABLE #PESSOA  
                                                    END ";
                }

                contextQuery.Parameters.Add("@maxRows", maxRows);
                contextQuery.Parameters.Add("@USUARIOLOG", System.Threading.Thread.CurrentPrincipal.Identity.Name);

                if (contextQuery.Command.Contains("WHERE AND"))
                {
                    contextQuery.Command = contextQuery.Command.Replace("WHERE AND", " WHERE ");
                }

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