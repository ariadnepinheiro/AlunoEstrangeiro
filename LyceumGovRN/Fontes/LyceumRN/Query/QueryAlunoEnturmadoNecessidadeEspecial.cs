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
    public class QueryAlunoEnturmadoNecessidadeEspecial : LyceumQuery
    {

        public QueryAlunoEnturmadoNecessidadeEspecial()
        {
            this.GridColumns.Add(new TSearchColumn { FieldName = "aluno", Caption = "Matrícula", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "nome_compl", Caption = "Nome", Width = Unit.Percentage(20) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "cpf", Caption = "CPF", Width = Unit.Percentage(8) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "mae", Caption = "Nome da Mãe", Width = Unit.Percentage(18) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "pai", Caption = "Nome do Pai", Width = Unit.Percentage(18) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "dt_nascimento", Caption = "Nascimento", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "pessoa", Caption = "pessoa", Width = Unit.Percentage(0), Visible = false });

            this.Messages.KeyNotFound = "Aluno não encontrado";
            this.TextField = "aluno";
            this.MaxLength = 15;
            this.DescriptionField = "nome_compl";

            this.GridFilterParameters.Add("nome_compl", "Nome", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("aluno", "Matrícula", TSearchDataType.String, 15);
            this.GridFilterParameters.Add("mae", "Mãe", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("pai", "Pai", TSearchDataType.String, 100);

            this.GridFilterParameters.Add(new TSearchParameter { Caption = "ativo", ParameterName = "ativo", ShowInFilterPanel = false, ParameterType = TSearchDataType.String });
            this.MaxRows = 50;
            this.GridWidth = Unit.Pixel(860);
        }

        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable lista = null;
            List<string> mensagens = new List<string>();
            DataView dv = new DataView();

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                if (!this.HasValue(pars, "ano"))
                {
                    mensagens.Add("O campo Ano é de preenchimento obrigatório.");
                    throw new GetDataException();
                }
                if (!this.HasValue(pars, "semestre"))
                {
                    mensagens.Add("O campo Semestre é de preenchimento obrigatório.");
                    throw new GetDataException();
                }
                if (!this.HasValue(pars, "turma"))
                {
                    mensagens.Add("O campo Turma é de preenchimento obrigatório.");
                    throw new GetDataException();
                }
                if (!this.HasValue(pars, "unidade_ens"))
                {
                    mensagens.Add("O campo Unidade de Ensino é de preenchimento obrigatório.");
                    throw new GetDataException();
                }

                contextQuery.ContextQueryType = ContextQueryType.Sql;

                //Esta view lista todos as escolas considerando as permissoes usuario
                contextQuery.Command = @" SELECT distinct top " + Convert.ToString(this.MaxRows + 1) + @"  
                                            A.ALUNO,
                                           P.NOME_COMPL,
                                           M.TURMA,
                                           M.ANO,
                                           M.SEMESTRE,
                                           T.TURNO,
                                           A.NUMINSCRICAO,
                                           A.PESSOA,
                                           P.NOME_PAI                           AS PAI,
                                           P.NOME_MAE                           AS MAE,
                                           P.PESSOA,
                                           P.NOME_COMPL                         AS NOME,
                                           P.RG_NUM,
                                           P.CPF,
                                           CONVERT(VARCHAR(10), P.DT_NASC, 103) AS DT_NASCIMENTO
                                FROM  LY_ALUNO A  
                                    INNER JOIN LY_PESSOA P ON A.PESSOA = P.PESSOA
                                    INNER JOIN DBO.LY_MATRICULA M ON A.ALUNO = M.ALUNO
                                    INNER JOIN LY_TURMA T ON M.DISCIPLINA = T.DISCIPLINA
                                                             AND M.TURMA = T.TURMA
                                                             AND M.ANO = T.ANO
                                                             AND M.SEMESTRE = T.SEMESTRE
                            WHERE   M.ANO = @ANO
                                    AND M.SEMESTRE = @SEMESTRE
                                    AND ( M.DEPENDENCIA IS NULL
                                          OR M.DEPENDENCIA = 'N'
                                        )
                                    AND ( M.CONCOMITANTE IS NULL
                                          OR M.CONCOMITANTE = 'N'
                                        )
                                    AND ( M.EDUC_ESPECIAL IS NULL
                                          OR M.EDUC_ESPECIAL = 'N'
                                        )
                                    AND ( M.MAIS_EDUCACAO IS NULL
                                          OR M.MAIS_EDUCACAO = 'N'
                                        )
                                    AND t.OPTATIVAREFORCO = 'N'
                                    AND ISNULL(T.ELETIVA,'N') = 'N'
                                    AND T.FACULDADE = @UNIDADE
                                    AND T.TURMA = @TURMA
                                    AND M.SIT_MATRICULA = 'Matriculado'
                                    AND P.NECESSIDADEESPECIALID <> 30
                                ";

                contextQuery.Parameters.Add("@ANO", pars["ano"].ToString());
                contextQuery.Parameters.Add("@SEMESTRE", pars["semestre"].ToString());
                contextQuery.Parameters.Add("@TURMA", pars["turma"].ToString());
                contextQuery.Parameters.Add("@UNIDADE", pars["unidade_ens"].ToString());

                if (key != null)
                {
                    contextQuery.Command += @" AND A.ALUNO = @ALUNO ";
                    contextQuery.Parameters.Add("@ALUNO ", key.ToString());
                }
                else
                {
                    if (this.HasValue(pars, "aluno"))
                    {
                        if (pars["aluno"].ToString().Length == 15)
                        {
                            contextQuery.Command += " AND A.ALUNO = @ALUNO ";
                            contextQuery.Parameters.Add("@ALUNO ", pars["aluno"].ToString());
                        }
                        else
                        {
                            contextQuery.Command += " AND A.ALUNO LIKE @ALUNO ";
                            contextQuery.Parameters.Add("@ALUNO ", this.LikeExpression(pars["aluno"].ToString()));
                        }
                    }

                    if (this.HasValue(pars, "nome_compl"))
                    {
                        contextQuery.Command += " AND P.NOME_COMPL LIKE @NOME_COMPL ";
                        contextQuery.Parameters.Add("@NOME_COMPL ", "%" + this.LikeExpression(pars["nome_compl"].ToString()));
                    }

                    if (this.HasValue(pars, "mae"))
                    {
                        contextQuery.Command += " AND P.NOME_MAE LIKE @MAE  ";
                        contextQuery.Parameters.Add("@MAE", "%" + this.LikeExpression(pars["mae"].ToString()));
                    }

                    if (this.HasValue(pars, "pai"))
                    {
                        contextQuery.Command += " AND P.NOME_PAI LIKE @PAI ";
                        contextQuery.Parameters.Add("@PAI", "%" + this.LikeExpression(pars["pai"].ToString()));
                    }
                }               

                lista = contexto.GetDataTable(contextQuery);
                dv = lista.DefaultView;
                dv.Sort = "NOME_COMPL ASC";
            }
            catch (GetDataException)
            {
                contexto.Abandon();
                string mensagem = mensagens.Aggregate((x, y) => x + "<br/>" + y);
                throw new GetDataException(mensagem);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                {
                    contexto.Dispose();
                }
            }

            return lista.DefaultView;
        }        
    }
}

