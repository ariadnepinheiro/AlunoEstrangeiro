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
    public class QueryUnidadeEnsinoTurnosEVagas : LyceumQuery
    {

        public QueryUnidadeEnsinoTurnosEVagas()
        {
            this.GridColumns.Add(new TSearchColumn { FieldName = "unidade_ens", Caption = "Censo", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "setor", Caption = "UA", Width = Unit.Percentage(10), Visible = false });
            this.GridColumns.Add(new TSearchColumn { FieldName = "id_regional", Caption = "Regional", Width = Unit.Percentage(10), Visible = false });
            this.GridColumns.Add(new TSearchColumn { FieldName = "municipio", Caption = "Município", Width = Unit.Percentage(10), Visible = false });
            this.GridColumns.Add(new TSearchColumn { FieldName = "nome_comp", Caption = "Nome", Width = Unit.Percentage(20) });

            this.Messages.KeyNotFound = "Unidade Ensino inválida";
            this.GridFilterParameters.Add("unidade_ens", "Censo", TSearchDataType.String, 8);
            this.GridFilterParameters.Add("nome_comp", "Nome", TSearchDataType.String, 200);

            this.TextField = "unidade_ens";
            this.MaxLength = 20;
            this.DescriptionField = "nome_comp";
            this.MaxRows = 200;
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

                if (!this.HasValue(pars, "Ano"))
                {
                    mensagens.Add("O campo Ano é de preenchimento obrigatório.");
                    throw new GetDataException();
                }

                if (this.HasValue(pars, "PeriodoAnalise"))
                {
                    if (pars["PeriodoAnalise"].ToString() == "True")
                    {
                        mensagens = validaFiltroAnalise(pars);

                        if (mensagens.Count != 0)
                        {
                            throw new GetDataException();
                        }

                        contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                        contextQuery.Command = @"TurnosVagas.SP_LISTAESCOLANALISECONFIRMACAOTURNOSVAGAS";

                        contextQuery.Parameters.Add("@PERFILID", this.HasValue(pars, "PeriodoAnalise") ? pars["Perfil"].ToString() : string.Empty);
                        contextQuery.Parameters.Add("@ANALISADATURNO", this.HasValue(pars, "TurnosAnalisados") ? pars["TurnosAnalisados"].ToString() : "Todos");
                        contextQuery.Parameters.Add("@RESULTADOTURNO", this.HasValue(pars, "ResultadoTurnos") ? pars["ResultadoTurnos"].ToString() : string.Empty);
                        contextQuery.Parameters.Add("@ANALISADAVAGA", this.HasValue(pars, "VagasAnalisadas") ? pars["VagasAnalisadas"].ToString() : "Todos");
                        contextQuery.Parameters.Add("@RESULTADOVAGA", this.HasValue(pars, "ResultadoVagas") ? pars["ResultadoVagas"].ToString() : string.Empty);
                        contextQuery.Parameters.Add("@FAIXAVARIACAOINICIO", this.HasValue(pars, "FaixaInicial") ? pars["FaixaInicial"].ToString() : string.Empty);
                        contextQuery.Parameters.Add("@FAIXAVARIACAOFIM", this.HasValue(pars, "FaixaFinal") ? pars["FaixaFinal"].ToString() : string.Empty);
                        contextQuery.Parameters.Add("@TIPOVARIACAO", this.HasValue(pars, "FaixaVariacao") ? pars["FaixaVariacao"].ToString() : "Ambos");
                        contextQuery.Parameters.Add("@CURSO", this.HasValue(pars, "ModSegCurso") && pars["ModSegCurso"].ToString() != "Curso" ? pars["ModSegCurso"].ToString() : string.Empty);
                        contextQuery.Parameters.Add("@SERIE", this.HasValue(pars, "Serie") ? pars["Serie"].ToString() : string.Empty);
                        contextQuery.Parameters.Add("@TURNO", this.HasValue(pars, "Turno") ? pars["Turno"].ToString() : string.Empty);
                        contextQuery.Parameters.Add("@REGIONALID", this.HasValue(pars, "Regional") ? pars["Regional"].ToString() : string.Empty);
                        contextQuery.Parameters.Add("@MUNICIPIO", this.HasValue(pars, "Municipio") ? pars["Municipio"].ToString() : string.Empty);
                        contextQuery.Parameters.Add("@ANO", this.HasValue(pars, "Ano") ? pars["Ano"].ToString() : string.Empty);
                        contextQuery.Parameters.Add("@MAXROWS", Convert.ToString(this.MaxRows + 1));

                        if (key != null)
                        {
                            contextQuery.Parameters.Add("@CENSO", key.ToString());
                            contextQuery.Parameters.Add("@CENSOPARCIAL", string.Empty);
                            contextQuery.Parameters.Add("@NOMEPARCIAL", string.Empty);
                        }
                        else
                        {
                            //Verificar se foi digitado o censo todo
                            if (this.HasValue(pars, "unidade_ens") && pars["unidade_ens"].ToString().Length == 8)
                            {
                                contextQuery.Parameters.Add("@CENSO", pars["unidade_ens"].ToString());
                                contextQuery.Parameters.Add("@CENSOPARCIAL", string.Empty);
                                contextQuery.Parameters.Add("@NOMEPARCIAL", string.Empty);
                            }
                            else
                            {
                                contextQuery.Parameters.Add("@CENSO", string.Empty);

                                if (this.HasValue(pars, "unidade_ens"))
                                {
                                    contextQuery.Parameters.Add("@CENSOPARCIAL", "%" + this.LikeExpression(pars["unidade_ens"].ToString()));
                                }
                                else
                                {
                                    contextQuery.Parameters.Add("@CENSOPARCIAL", string.Empty);
                                }

                                if (this.HasValue(pars, "nome_comp"))
                                {
                                    contextQuery.Parameters.Add("@NOMEPARCIAL", "%" + this.LikeExpression(pars["nome_comp"].ToString()));
                                }
                                else
                                {
                                    contextQuery.Parameters.Add("@NOMEPARCIAL", string.Empty);
                                }
                            }
                        }
                    }
                    else
                    {
                        contextQuery.ContextQueryType = ContextQueryType.Sql;

                        if (!this.HasValue(pars, "PerfilUsuarioLogado"))
                        {
                            mensagens.Add("O Perfil do usuario logado não foi econtrado.");
                            throw new GetDataException();
                        }

                        if (pars["PerfilUsuarioLogado"].ToString() != "privilegiado" 
                            && pars["PerfilUsuarioLogado"].ToString() != "DIRETOR_UE" 
                            && pars["PerfilUsuarioLogado"].ToString() != "REGIONAL" 
                            && pars["PerfilUsuarioLogado"].ToString() != "DIESP"
                            && pars["PerfilUsuarioLogado"].ToString() != "SUPLAN"
                            && pars["PerfilUsuarioLogado"].ToString() != "SUPED")
                        {
                            mensagens.Add("O usuario logado não possui perfil necessário para utilizar a tela.");
                            throw new GetDataException();
                        }

                        //Esta view lista todos as escolas considerando as permissoes usuario
                        contextQuery.Command = @" SELECT distinct top " + Convert.ToString(this.MaxRows + 1) + @" s.unidade_ens, s.nome_comp, s.setor, s.id_regional, s.municipio 
                                FROM VW_UNIDADE_ENSINO_SITUACAO S
                                INNER JOIN DBO.TCE_CTV_CONF_TURNO_INICIAL TI ON S.UNIDADE_ENS=TI.CENSO
                                INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON TI.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                                WHERE SITUACAO = 'ESTADUAL'
                                    AND ANO = @ANO
                                ";

                        contextQuery.Parameters.Add("@ANO", pars["Ano"].ToString());

                        //Verificar permissão de visualizar prisionais (apenas DIESP e privilegiado)
                        if (pars["PerfilUsuarioLogado"].ToString() == "DIESP")
                        {
                            contextQuery.Command += " AND S.ID_REGIONAL = 5 ";
                        }
                        else
                        {
                            if (pars["PerfilUsuarioLogado"].ToString() != "privilegiado" && pars["PerfilUsuarioLogado"].ToString() != "DIRETOR_UE")
                            {
                                contextQuery.Command += " AND S.ID_REGIONAL <> 5 ";
                            }                            
                        }

                        if (key != null)
                        {
                            contextQuery.Command += " AND S.UNIDADE_ENS = @CENSO ";
                            contextQuery.Parameters.Add("@CENSO ", key.ToString());
                        }
                        else
                        {
                            //Verificar se foi digitado o censo todo
                            if (this.HasValue(pars, "unidade_ens") && pars["unidade_ens"].ToString().Length == 8)
                            {
                                contextQuery.Command += " AND S.UNIDADE_ENS = @CENSO ";
                                contextQuery.Parameters.Add("@CENSO ", pars["unidade_ens"].ToString());
                            }
                            else
                            {
                                if (this.HasValue(pars, "unidade_ens"))
                                {
                                    contextQuery.Command += " AND S.UNIDADE_ENS like @UNIDADE_ENS ";
                                    contextQuery.Parameters.Add("@UNIDADE_ENS", "%" + this.LikeExpression(pars["unidade_ens"].ToString()));
                                }

                                if (this.HasValue(pars, "nome_comp"))
                                {
                                    contextQuery.Command += " AND s.nome_comp like @NOME_COMP " ;
                                    contextQuery.Parameters.Add("@NOME_COMP", "%" + this.LikeExpression(pars["nome_comp"].ToString()));
                                }

                                if (this.HasValue(pars, "Municipio"))
                                {
                                    contextQuery.Command += " AND MUNICIPIO = @MUNICIPIO ";
                                    contextQuery.Parameters.Add("@MUNICIPIO ", pars["Municipio"].ToString());
                                }

                                if (pars["PerfilUsuarioLogado"].ToString() != "DIESP" && this.HasValue(pars, "Regional"))
                                {
                                    contextQuery.Command += " AND S.ID_REGIONAL = @ID_REGIONAL ";
                                    contextQuery.Parameters.Add("@ID_REGIONAL ", pars["Regional"].ToString());
                                }
                            }
                        }

                        contextQuery.Command += " ORDER BY S.NOME_COMP ";
                    }
                }

                lista = contexto.GetDataTable(contextQuery);
                dv = lista.DefaultView;
                dv.Sort = "nome_comp ASC";
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

        private List<string> validaFiltroAnalise(IDictionary<string, object> pars)
        {
            List<string> erros = new List<string>();
            if (!this.HasValue(pars, "Perfil"))
            {
                erros.Add("O campo Perfil é de preenchimento obrigatório.");
            }
            if (!this.HasValue(pars, "TurnosAnalisados"))
            {
                erros.Add("O campo Turnos Analisados é de preenchimento obrigatório.");
            }
            else
            {
                if (pars["TurnosAnalisados"].ToString() == "Sim" && !this.HasValue(pars, "ResultadoTurnos"))
                {
                    erros.Add("O campo Resultado é de preenchimento obrigatório quando a opção de Turnos Analisados for igual a 'Sim'.");
                }
            }
            if (!this.HasValue(pars, "VagasAnalisadas"))
            {
                erros.Add("O campo Vagas Analisadas é de preenchimento obrigatório.");
            }
            else
            {
                if (pars["VagasAnalisadas"].ToString() == "Sim" && !this.HasValue(pars, "ResultadoVagas"))
                {
                    erros.Add("O campo Resultado é de preenchimento obrigatório quando a opção de Vagas Analisadas for igual a 'Sim'.");
                }
            }

            if ((!this.HasValue(pars, "FaixaInicial") && this.HasValue(pars, "FaixaFinal")) ||
                (this.HasValue(pars, "FaixaInicial") && !this.HasValue(pars, "FaixaFinal")))
            {
                erros.Add("Para a Faixa de Variação de Vaga todos os campos são obrigatórios.");
            }
            else
            {
                if (this.HasValue(pars, "FaixaInicial") && this.HasValue(pars, "FaixaFinal") && !this.HasValue(pars, "FaixaVariacao"))
                {
                    erros.Add("Para a Faixa de Variação de Vaga todos os campos são obrigatórios.");
                }

                if ((this.HasValue(pars, "FaixaInicial") && !Validacao.ValidaNumerosInteirosPositivos(pars["FaixaInicial"].ToString()))
                || (this.HasValue(pars, "FaixaFinal") && !Validacao.ValidaNumerosInteirosPositivos(pars["FaixaFinal"].ToString())))
                {
                    erros.Add("Os campos da Faixa de Variação de Vaga devem conter só números inteiros.");
                }
            }
            return erros;
        }
    }
}
