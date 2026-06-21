using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using System;
using Techne.Lyceum.RN.Util;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace Techne.Lyceum.RN
{
    public class CtvPropostaSeeduc
    {
        public static bool VerificaPropostaSeeducPorAgenda(int idAgenda, string censo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @" SELECT TOP 1
                                    1
                            FROM    DBO.TCE_CTV_PROPOSTA_SEEDUC PS
                            WHERE   CENSO = @CENSO
                                    AND ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA "
                };

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", idAgenda);

                var obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    return true;
                }
            }
            return false;
        }

        public TceCtvPropostaSeeduc ObtemPor(string censo, int ano, int periodo, string curso, int serie)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            TceCtvPropostaSeeduc proposta = new TceCtvPropostaSeeduc();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT TOP 1 * 
                                        FROM   DBO.TCE_CTV_PROPOSTA_SEEDUC P 
                                               INNER JOIN TCE_CTV_AGENDA_CONF_TURNO_VAGA A 
                                                       ON P.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA 
                                        WHERE  CENSO = @CENSO 
                                               AND ANO = @ANO
                                               AND PERIODO = @PERIODO 
                                               AND CURSO = @CURSO 
                                               AND SERIE = @SERIE  ";

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@SERIE", serie);

                proposta = ctx.TryToBindEntity<TceCtvPropostaSeeduc>(contextQuery);

                return proposta;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public DataTable ListaPor(string censo, int ano)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT A.ANO, 
                                       A.PERIODO, 
	                                   M.DESCRICAO AS NOMEMODALIDADE,
                                       M.MODALIDADE ,
                                       T.DESCRICAO AS NIVEL ,
                                       A.CURSO, 
                                       C.NOME AS NOME, 
                                       A.SERIE, 
                                       P.CENSO, 
                                       P.ID_PROPOSTA_SEEDUC, 
                                       P.VAGAS_CONTINUIDADE, 
                                       P.VAGAS_NOVAS, 
                                       P.TAXAREPROVACAO 
                                FROM   TCE_CTV_PROPOSTA_SEEDUC P (NOLOCK) 
                                       INNER JOIN TCE_CTV_AGENDA_CONF_TURNO_VAGA A (NOLOCK) 
                                               ON P.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA 
                                       INNER JOIN LY_CURSO C (NOLOCK) 
                                               ON A.CURSO = C.CURSO 
                                       INNER JOIN LY_MODALIDADE_CURSO M (NOLOCK) 
	                                           ON C.MODALIDADE = M.MODALIDADE
	                                   INNER JOIN LY_TIPO_CURSO T (NOLOCK) 
	                                           ON C.TIPO = T.TIPO
                                WHERE  P.CENSO = @CENSO 
                                       AND A.ANO = @ANO 
                                ORDER BY ANO DESC, PERIODO DESC, NOMEMODALIDADE, NIVEL, NOME, SERIE ";

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }

            return dt;
        }

        public ValidacaoDados Valida(DTOs.DadosPropostaTurnosVagas dadosProposta, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.CtvAgendaConfTurnoVaga rnCtvAgendaConfTurnoVaga = new CtvAgendaConfTurnoVaga();
            CtvConfTurnoInicial rnCtvConfTurnoInicial = new CtvConfTurnoInicial();
            int idAgenda = 0;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosProposta == null)
            {
                return validacaoDados;
            }

            if (!cadastro)
            {
                if (dadosProposta.IdPropostaSeeduc <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (dadosProposta.Ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (dadosProposta.Periodo < 0)
            {
                mensagens.Add("Campo PERIODO é obrigatório.");
            }

            if (dadosProposta.Curso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CURSO é obrigatório.");
            }

            if (dadosProposta.Serie < 0)
            {
                mensagens.Add("Campo SÉRIE é obrigatório.");
            }

            if (dadosProposta.Censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CENSO é obrigatório.");
            }

            if (dadosProposta.VagasContinuidade < 0)
            {
                mensagens.Add("Campo VAGAS DE CONTINUIDADE é obrigatório.");
            }

            if (dadosProposta.VagasNovas < 0)
            {
                mensagens.Add("Campo VAGAS NOVAS é obrigatório.");
            }

            if (dadosProposta.TaxaReprovacao < 0)
            {
                mensagens.Add("Campo TAXA DE REPROVAÇÃO é obrigatório.");
            }

            if (dadosProposta.Matricula.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO RESPONSÁVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Busca Agenda
                    idAgenda = rnCtvAgendaConfTurnoVaga.ObtemIdAgendaAbertaPor(contexto, dadosProposta.Ano, dadosProposta.Periodo, dadosProposta.Curso, dadosProposta.Serie);
                    if (idAgenda <= 0)
                    {
                        mensagens.Add("A agenda do ano / periodo / curso / serie não existe ou foi encerrada.");
                    }
                    else
                    {
                        dadosProposta.IdAgendaConfTurnoVaga = idAgenda;

                        //Verifica se já existe outra proposta para a agenda
                        if (this.PossuiOutraPropostaPor(contexto, dadosProposta.IdAgendaConfTurnoVaga, dadosProposta.Censo, dadosProposta.IdPropostaSeeduc))
                        {
                            mensagens.Add("Já existe uma proposta cadastrada para o ano / periodo / curso / serie.");
                        }

                        //Verifica se a escola possui modalidade / curso / serie para lançamento
                        if (!rnCtvConfTurnoInicial.PossuiTurnoParaConfirmacaoPor(contexto, dadosProposta.IdAgendaConfTurnoVaga, dadosProposta.Censo))
                        {
                            mensagens.Add("A escola não possui esta modalidade / curso / serie para lançamento, favor utilizar a tela de Inclusão Modalidade Serie.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (contexto != null)
                    {
                        contexto.Dispose();
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        private bool PossuiOutraPropostaPor(DataContext ctx, int idAgendaTurnosVagas, string censo, int idProposta)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                      FROM TCE_CTV_PROPOSTA_SEEDUC (NOLOCK)
                                      WHERE ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA 
                                            AND CENSO = @CENSO
                                            AND ID_PROPOSTA_SEEDUC <> @ID_PROPOSTA_SEEDUC ";

            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", idAgendaTurnosVagas);
            contextQuery.Parameters.Add("@CENSO", censo);
            contextQuery.Parameters.Add("@ID_PROPOSTA_SEEDUC", idProposta);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public void Insere(DTOs.DadosPropostaTurnosVagas dadosProposta)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            TceCtvPropostaSeeduc propostaSeeduc = new TceCtvPropostaSeeduc();

            try
            {
                //Monta Entidade
                propostaSeeduc.IdAgendaConfTurnoVaga = dadosProposta.IdAgendaConfTurnoVaga;
                propostaSeeduc.Censo = dadosProposta.Censo;
                propostaSeeduc.VagasContinuidade = dadosProposta.VagasContinuidade;
                propostaSeeduc.VagasNovas = dadosProposta.VagasNovas;
                propostaSeeduc.TaxaReprovacao = dadosProposta.TaxaReprovacao;
                propostaSeeduc.Matricula = dadosProposta.Matricula;

                //Insere proposta
                this.Insere(contexto, propostaSeeduc);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }
        }

        public void Atualiza(DTOs.DadosPropostaTurnosVagas dadosProposta)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            TceCtvPropostaSeeduc propostaSeeduc = new TceCtvPropostaSeeduc();

            try
            {
                //Monta Entidade
                propostaSeeduc.IdPropostaSeeduc = dadosProposta.IdPropostaSeeduc;
                propostaSeeduc.IdAgendaConfTurnoVaga = dadosProposta.IdAgendaConfTurnoVaga;
                propostaSeeduc.Censo = dadosProposta.Censo;
                propostaSeeduc.VagasContinuidade = dadosProposta.VagasContinuidade;
                propostaSeeduc.VagasNovas = dadosProposta.VagasNovas;
                propostaSeeduc.TaxaReprovacao = dadosProposta.TaxaReprovacao;
                propostaSeeduc.Matricula = dadosProposta.Matricula;

                //Insere proposta
                this.Atualiza(contexto, propostaSeeduc);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }
        }

        public void Insere(DataContext ctx, TceCtvPropostaSeeduc proposta)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO TCE_CTV_PROPOSTA_SEEDUC 
                                                (ID_AGENDA_CONF_TURNO_VAGA, 
                                                 CENSO, 
                                                 VAGAS_CONTINUIDADE, 
                                                 VAGAS_NOVAS, 
                                                 DT_CADASTRO, 
                                                 MATRICULA, 
                                                 TAXAREPROVACAO) 
                                    VALUES     (@ID_AGENDA_CONF_TURNO_VAGA, 
                                                @CENSO, 
                                                @VAGAS_CONTINUIDADE, 
                                                @VAGAS_NOVAS, 
                                                @DT_CADASTRO, 
                                                @MATRICULA, 
                                                @TAXAREPROVACAO)  ";

                contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", proposta.IdAgendaConfTurnoVaga);
                contextQuery.Parameters.Add("@CENSO", proposta.Censo);
                contextQuery.Parameters.Add("@VAGAS_CONTINUIDADE", proposta.VagasContinuidade);
                contextQuery.Parameters.Add("@VAGAS_NOVAS", proposta.VagasNovas);
                contextQuery.Parameters.Add("@DT_CADASTRO", DateTime.Now);
                contextQuery.Parameters.Add("@MATRICULA", proposta.Matricula);
                contextQuery.Parameters.Add("@TAXAREPROVACAO", proposta.TaxaReprovacao);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
        }

        private void Atualiza(DataContext ctx, TceCtvPropostaSeeduc proposta)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE TCE_CTV_PROPOSTA_SEEDUC 
                        SET    VAGAS_CONTINUIDADE = @VAGAS_CONTINUIDADe, 
                               VAGAS_NOVAS = @VAGAS_NOVAS, 
                               TAXAREPROVACAO = @TAXAREPROVACAO, 
                               MATRICULA = @MATRICULA 
                        WHERE  ID_PROPOSTA_SEEDUC = @ID_PROPOSTA_SEEDUC  ";

                contextQuery.Parameters.Add("@ID_PROPOSTA_SEEDUC", proposta.IdPropostaSeeduc);
                contextQuery.Parameters.Add("@VAGAS_CONTINUIDADE", proposta.VagasContinuidade);
                contextQuery.Parameters.Add("@VAGAS_NOVAS", proposta.VagasNovas);
                contextQuery.Parameters.Add("@TAXAREPROVACAO", proposta.TaxaReprovacao);
                contextQuery.Parameters.Add("@MATRICULA", proposta.Matricula);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
        }
    }
}