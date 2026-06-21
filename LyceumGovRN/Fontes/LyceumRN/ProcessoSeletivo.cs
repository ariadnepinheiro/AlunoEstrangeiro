using System;
using Seeduc.Infra.Data;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN.Util;
using Techne.Library;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.RN
{
    public class ProcessoSeletivo : RNBase
    {
        #region Propriedades e Enum
        public const string DOCI_TEXT = "Professor para atuar nos anos finais do ensino fundamental e/ou ensino médio";
        public const string DOCI_VALUE = "DOC I";
        public const string DOCII_TEXT = "Professor para atuar nos anos iniciais do ensino fundamental";
        public const string DOCII_VALUE = "DOC II";
        public const string CDRH_NAO_AVALIADO = "Não Avaliado";
        public const string CDRH_AUTORIZADO = "Autorizado";
        public const string CDRH_AUTORIZADO_OBS = "Autorizado pela COSEP";
        public const string CDRH_NAO_AUTORIZADO = "Não Autorizado";
        public const string CDRH_NAO_AUTORIZADO_OBS = "Não autorizado pela COSEP";
        public const string CDRH_EM_EXIGENCIA = "Em exigência";
        public const string CDRH_EM_EXIGENCIA_OBS = "Em exigência pela COSEP";
        public const string CQHI_NAO_AVALIADO = "Não Avaliado";
        public const string CQHI_NECESSARIA = "Necessária";
        public const string CQHI_NECESSARIA_OBS = "Considerada necessária pela CDGP";
        public const string CQHI_NAO_NECESSARIA = "Não Necessária";
        public const string CQHI_NAO_NECESSARIA_OBS = "Considerada não necessária pela CDGP";
        public const string APROVADA_OBS = "Proposta aprovada pela COSEP e pela CDGP";
        public const string REMESSA_SIM = "Sim";
        public const string REMESSA_NAO = "Não";
        public const string RESCISAO_APROVADA = "Rescisão aprovada";
        public const string RESCISAO_NAO_APROVADA = "Rescisão não aprovada";
        public const string AMPLIACAO_APROVADA = "Ampliação de carga horária aprovada";
        public const string AMPLIACAO_NAO_APROVADA = "Ampliação de carga horária não aprovada";
        public const string REDUCAO_APROVADA = "Redução de carga horária aprovada";
        public const string REDUCAO_NAO_APROVADA = "Redução de carga horária não aprovada";

        // Status do candidato inscrito em um processo seletivo para Contrato Temporário
        public enum Status
        {
            [StringValue("Situação Inexistente")]
            Erro = 0,
            [StringValue("Aguardando")]
            Aguardando = 1,
            [StringValue("Convocado")]
            Convocado = 2,
            [StringValue("Aguardando Avaliação de Proposta de Contrato Temporário")]
            PropostaContratoTemporario = 3,
            [StringValue("Aguardando Remessa de Aprovação")]
            AguardandoRemessaAprovacao = 4,
            [StringValue("Desistente")]
            Desistente = 5,
            [StringValue("Reprovado")]
            Reprovado = 6,
            [StringValue("Aprovado")]
            Aprovado = 7,
            [StringValue("Cancelado")]
            Cancelado = 8,
            [StringValue("Desativado")]
            Desativado = 9,
            [StringValue("Aguardando Avaliação de Rescisão de Contrato Temporário")]
            AguardandoAvaliacaoRescisao = 10,
            [StringValue("Contrato Rescindido")]
            ContratoRescindido = 11,
            [StringValue("Reprovado pela COSEP")]
            ReprovadoRH = 12,
            [StringValue("Reprovado pela CDGP")]
            ReprovadoQHI = 13,
            [StringValue("Aguardando Avaliação de Proposta de Ampliação de Carga Horária de Contrato Temporário")]
            AguardandoAvaliacaoAmpliacaoCargaHoraria = 14,
            [StringValue("Aguardando Avaliação de Redução de Carga Horária de Contrato Temporário")]
            AguardandoAvaliacaoReducaoCargaHoraria = 15,
            [StringValue("Aguardando Remessa de Rescisão de Contrato Temporário")]
            AguardandoRemessaRescisao = 16,
            [StringValue("Aguardando Remessa de Redução de Carga Horária de Contrato Temporário")]
            AguardandoRemessaAmpliacaoCargaHoraria = 17,
            [StringValue("Aguardando Remessa de Redução de Carga Horária de Contrato Temporário")]
            AguardandoRemessaReducaoCargaHoraria = 18,
            [StringValue("Contrato Temporário com Carga Horária Ampliada")]
            ContratoCargaHorariaAmpliada = 19,
            [StringValue("Contrato Temporário com Carga Horária Reduzida")]
            ContratoCargaHorariaReduzida = 20,
            [StringValue("Faltoso")]
            Faltoso = 21,
            [StringValue("Desclassificado")]
            Inabilitado = 22,
            [StringValue("Aguardando avaliação CGP")]
            AguardandoCGP = 23,
            [StringValue("Admitido")]
            Admitido = 24,
            [StringValue("Demitido")]
            Demitido = 25,
            [StringValue("Inscrito")]
            Inscrito = 26
        }

        // Status de um contrato temporário
        public enum StatusContrato
        {
            [StringValue("Situação Inexistente")]
            Erro = 0,
            [StringValue("Aprovado")]
            Aprovado = 1,
            [StringValue("Carga Horária Ampliada")]
            CargaHorariaAmpliada = 2,
            [StringValue("Carga Horária Reduzida")]
            CargaHorariaReduzida = 3,
            [StringValue("Rescindido")]
            Rescindido = 4
        }

        // Status do candidato inscrito em um processo seletivo para Contrato Temporário para consulta pública
        public enum StatusPublico
        {
            [StringValue("Aguardando")]
            Aguardando = 1,
            [StringValue("Aprovado")]
            Aprovado = 2,
            [StringValue("Reprovado")]
            Reprovado = 3
        }

        // Tipo das solicitações realizadas em Contrato Temporário
        public enum TipoSolicitacao
        {
            [StringValue("Proposta de Ampliação de Carga Horária em Avaliação")]
            AmpliacaoCargaHorariaSolicitacao = 1,
            [StringValue("Proposta de Ampliação de Carga Horária Aprovada")]
            AmpliacaoCargaHorariaAprovada = 2,
            [StringValue("Proposta de Ampliação de Carga Horária Reprovada")]
            AmpliacaoCargaHorariaReprovada = 3,
            [StringValue("Solicitação de Redução de Carga Horária em Avaliação")]
            ReducaoCargaHorariaSolicitacao = 4,
            [StringValue("Solicitação de Redução de Carga Horária Aprovada")]
            ReducaoCargaHorariaAprovada = 5,
            [StringValue("Solicitação de Redução de Carga Horária Reprovada")]
            ReducaoCargaHorariaReprovada = 6,
            [StringValue("Solicitação de Rescisão de Contrato Temporário em Avaliação")]
            RescisaoSolicitacao = 7,
            [StringValue("Solicitação de Rescisão de Contrato Temporário Aprovada")]
            RescisaoAprovada = 8,
            [StringValue("Solicitação de Rescisão de Contrato Temporário Reprovada")]
            RescisaoReprovada = 9
        }

        // Tipo dos documentos remetidos em Contrato Temporário
        public enum TipoDocumento
        {
            [StringValue("Tipo de Documento Inexistente")]
            Erro = 0,
            [StringValue("Aprovação")]
            Aprovacao = 1,
            [StringValue("Ampliação de Carga Horária")]
            AmpliacaoCargaHoraria = 2,
            [StringValue("Redução de Carga Horária")]
            ReducaoCargaHoraria = 3,
            [StringValue("Rescisão de Contrato Temporário")]
            Rescisao = 4
        }
        #endregion

        /// <summary>
        /// Consulta o número de inscrições disponíveis para um processo seletivo, dadas coordenadoria,
        /// função e disciplina.
        /// DEFINIÇÃO: Número de inscrições disponíveis é a quantidade de candidatos inscritos que não
        /// foram convocados.
        /// </summary>
        /// <param name="processoSeletivo">Processo Seletivo</param>
        /// <param name="coordenadoria">Coordenadoria</param>
        /// <param name="funcao">Função</param>
        /// <param name="disciplina">Disciplina de Ingresso</param>
        public static int ConsultarInscricoesDisponiveis(string processoSeletivo, string coordenadoria, string municipio)
        {
            string sql = @"SELECT COUNT(candidato) from LY_CANDIDATO_DOCENTE WHERE STATUS in (1, 2, 23, 24) and CONCURSO = ? and NUCLEO = ? ";

            if (!string.IsNullOrEmpty(municipio))
            {
                sql += " and MUNICIPIO_PROC = ?";
                return ExecutarFuncao(sql, processoSeletivo, coordenadoria, municipio);
            }

            return ExecutarFuncao(sql, processoSeletivo, coordenadoria);
        }

        /// <summary>
        /// Consulta o número de inscrições disponíveis para um Concurso, dadas coordenadoria, disciplina, cota da inscrição e municipio
        /// </summary>
        /// <param name="strConcurso">Concurso</param>
        /// <param name="strCoordenadoria">Coordenadoria</param>
        /// <param name="strMunicipio">Municipio</param>
        /// <param name="strDisciplina">Disciplina de Ingresso</param>
        /// <param name="intCotaIdInscricao">Cota da Inscrição</param>
        /// <returns></returns>
        public static int ConsultarInscricoesDisponiveisPor(string strDisciplina, string strConcurso, string strMunicipio, string strNucleo)
        {
            string sql = @"SELECT COUNT(CD.CANDIDATO) from LY_CANDIDATO_DOCENTE  CD
						INNER JOIN ContratoTemporario.CANDIDATODOCENTE_GRUPOHABILITACAO CG 
						ON CD.CANDIDATO = CG.CANDIDATO AND CD.CONCURSO = CG.CONCURSO
						WHERE STATUS = 26 AND CG.HABILITADO = 1 AND CG.AGRUPAMENTO = ? AND CD.CONCURSO = ? 
						AND CD.MUNICIPIO_PROC = ? AND CD.NUCLEO = ? ";

            return ExecutarFuncao(sql, strDisciplina, strConcurso, strMunicipio, strNucleo);
        }

        public static int ConsultarInscricoesDisponiveisPor(string strDisciplina, string strConcurso, string strMunicipio, string strNucleo, string strCotaId)
        {
            string sql = @"SELECT COUNT(CD.CANDIDATO) from LY_CANDIDATO_DOCENTE  CD
							INNER JOIN ContratoTemporario.CANDIDATODOCENTE_GRUPOHABILITACAO CG 
							ON CD.CANDIDATO = CG.CANDIDATO AND CD.CONCURSO = CG.CONCURSO
							WHERE STATUS = 1 AND CG.AGRUPAMENTO = ? AND CD.CONCURSO = ?
							AND CD.MUNICIPIO_PROC = ? AND CD.NUCLEO = ? AND CD.COTAIDINSCRICAO = ? ";

            return ExecutarFuncao(sql, strDisciplina, strConcurso, strMunicipio, strNucleo, strCotaId);
        }

        public static RetValue ConvocarCandidatos(TConnectionWritable connection, string qtd, string concurso, string disciplina, DateTime data, DateTime hora, string municipio, string nucleo, int intRegional, int intCota, ref QueryTable qt)
        {
            RetValue retorno = null;
            //QueryTable qt = null;
            CR.Ly_candidato_docente dt = new CR.Ly_candidato_docente();
            qt = RN.ProcessoSeletivo.SelecionarConvocados(qtd, concurso, disciplina, municipio, nucleo, intCota.ToString());

            if (qt != null)
            {
                //RN.CandidatoDocente.AtualizarCandidatos();
                PopularLyCandidatoDocente(qt, dt, concurso, data, hora, intRegional, intCota, disciplina);
                retorno = AtualizarCandidatos(connection, dt);
            }

            return retorno;
        }

        public static RetValue ExecutaConvocaReprova(string qtd, string concurso, string disciplina, DateTime data, DateTime hora, string municipio, string nucleo, int intRegional, int intCota, ref QueryTable qt)
        {
            RetValue retorno;
            retorno = null;
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            retorno = ConvocarCandidatos(connection, qtd, concurso, disciplina, data, data, municipio, nucleo, intRegional, intCota, ref qt);

            if (retorno != null)
            {
                if (!retorno.Ok)
                {
                    connection.Rollback();
                    connection.Close();
                    return retorno;
                }
            }

            connection.Close();
            return new RetValue(true, "Candidatos convocados com sucesso.", null); ;
        }

        public static void PopularLyCandidatoDocente(QueryTable qt, CR.Ly_candidato_docente dt, string concurso, DateTime data, DateTime hora, int regional, int intCota, string strDisciplina)
        {
            if (qt != null)
            {
                foreach (SimpleRow linhaDados in qt.Rows)
                {
                    CR.Ly_candidato_docente.Row linhaDestino = dt.NewRow();
                    PopularLinhaCandidatoDocente(linhaDados, linhaDestino, concurso, data, hora, regional, intCota, strDisciplina);
                    dt.Rows.Add(linhaDestino);
                }
            }
        }

        private static void PopularLinhaCandidatoDocente(SimpleRow linhaDados, CR.Ly_candidato_docente.Row linhaDestino, string concurso, DateTime data, DateTime hora, int intRegional, int intCota, string strDisciplina)
        {
            linhaDestino.Candidato = Convert.ToString(linhaDados["Candidato"]);
            linhaDestino.Concurso = concurso;
            linhaDestino.Dt_apresentacao = data;
            linhaDestino.Hora_apresentacao = hora;
            linhaDestino.RegionalId = intRegional;
            linhaDestino.CotaIdConvocacao = intCota;
            linhaDestino.Agrupamento_ingresso = strDisciplina;
        }

        public static RetValue AtualizarCandidatos(TConnectionWritable connection, Ly_candidato_docente dt)
        {
            RetValue retorno = null;
            if (dt != null)
            {
                if (dt.Rows != null)
                {

                    foreach (Ly_candidato_docente.Row linha in dt.Rows)
                    {
                        Ly_candidato_docente.Row.Update(connection, linha.Concurso, linha.Candidato, "Agrupamento_Ingresso, Status, Dt_apresentacao, Dt_convocacao, Hora_apresentacao, REGIONALID, COTAIDCONVOCACAO", linha.Agrupamento_ingresso, "2", linha.Dt_apresentacao, DateTime.Now, linha.Hora_apresentacao, linha.RegionalId, linha.CotaIdConvocacao);

                        retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null)
                            return retorno;
                    }
                }
            }
            return new RetValue(true, "Candidatos convocados com sucesso.", null); ;
        }

        /// <summary>
        /// Altera a situação do candidato para o status escolhido
        /// </summary>
        /// <param name="strConcurso"></param>
        /// <param name="strCandidato"></param>
        /// <param name="strStatus"></param>
        public static void AlterarSituacaoCandidato(string strConcurso, string strCandidato, string strStatus)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE LY_CANDIDATO_DOCENTE SET STATUS= @STATUS
										where CONCURSO= @CONCURSO AND CANDIDATO= @CANDIDATO";

                contextQuery.Parameters.Add("@STATUS", strStatus);
                contextQuery.Parameters.Add("@CONCURSO", strConcurso);
                contextQuery.Parameters.Add("@CANDIDATO", strCandidato);

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
            finally
            {
                ctx.Dispose();
            }
        }

        /// <summary>
        /// Atualiza a situação do candidato para aguardando e seta nulo para os dados da convocação.
        /// </summary>
        /// <param name="strConcurso"></param>
        /// <param name="strCandidato"></param>
        public static void AlterarSituacaoEDadosConvocacao(string strConcurso, string strCandidato)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE LY_CANDIDATO_DOCENTE
										SET STATUS = '1', DT_APRESENTACAO = NULL, HORA_APRESENTACAO = NULL,
										AGRUPAMENTO_INGRESSO = NULL, COTAIDCONVOCACAO = NULL, DT_CONVOCACAO = NULL
										WHERE CONCURSO = @CONCURSO AND CANDIDATO = @CANDIDATO";

                contextQuery.Parameters.Add("@CONCURSO", strConcurso);
                contextQuery.Parameters.Add("@CANDIDATO", strCandidato);

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
            finally
            {
                ctx.Dispose();
            }
        }

        /// <summary>
        /// Seleciona os candidatos convocados em um Processo Seletivo.
        /// DEFINIÇÃO: Não será mais realizado o critério de reprovação por Pontuação para o Processo Seletivo.
        /// </summary>
        /// <param name="qtd"></param>
        /// <param name="concurso"></param>
        /// <param name="nucleo"></param>
        /// <param name="categoria"></param>
        /// <param name="disciplina"></param>
        /// <returns></returns>
        public static QueryTable SelecionarConvocados(string qtd, string concurso, string disciplina, string municipio, string nucleo, string strCota)
        {
            string sql = string.Format(@"SELECT TOP {0}
                            cd.candidato AS candidato ,
                            nome ,
                            DT_NASC AS datanasc ,
                            status ,
                            ( SELECT    SUM(ctpont.PONTUACAO)
                              FROM      ( (SELECT   pontuacao
                                           FROM     LY_CONCURSO_DOC_EXPERIENCIA code
                                                    INNER JOIN LY_CANDIDATO_DOC_EXPERIENCIAS cade ON code.CONCURSO = cade.CONCURSO
                                                                                  AND code.EXPERIENCIA = cade.EXPERIENCIA
                                           WHERE    cade.CANDIDATO = cd.candidato
                                                    AND cade.CONCURSO = cd.CONCURSO )
                                          UNION ALL
                                          ( SELECT  pontuacao
                                            FROM    LY_CONCURSO_DOC_TITULACOES codt
                                                    JOIN LY_CANDIDATO_DOC_TITULACOES cadt ON codt.CONCURSO = cadt.CONCURSO
                                                                                  AND codt.TITULACAO = cadt.TITULACAO
                                            WHERE   cadt.CANDIDATO = cd.candidato
                                                    AND cadt.CONCURSO = cd.CONCURSO
                                          )
                                        ) ctpont
                            ) AS pontuacao
                    FROM    LY_CANDIDATO_DOCENTE cd
                            INNER JOIN contratotemporario.CANDIDATODOCENTE_GRUPOHABILITACAO cg ON cg.CONCURSO = cd.CONCURSO
                                                                                  AND cg.CANDIDATO = cd.CANDIDATO
                    WHERE   cd.CONCURSO = ?
                            AND cg.AGRUPAMENTO = ?
                            AND cd.STATUS = 26
                            AND CG.HABILITADO = 1
                            AND cd.MUNICIPIO_PROC = ?
                            AND cd.NUCLEO = ? ", qtd);

            if (Convert.ToInt32(strCota) != 3)
            {
                sql += @" and cd.COTAIDINSCRICAO = ? 
                            order by pontuacao desc, DT_NASC asc ";
                return Consultar(sql, concurso, disciplina, municipio, nucleo, Convert.ToInt32(strCota));
            }

            sql += " order by pontuacao desc, DT_NASC asc ";
            return Consultar(sql, concurso, disciplina, municipio, nucleo);
        }

        /// <summary>
        /// Este método executa uma query que retorna uma List<string> com emails dos candidatos para envio de mala-direta.
        /// </summary>
        /// <param name="qtd">string</param>
        /// <param name="concurso">string</param>
        /// <param name="nucleo">string</param>
        /// <param name="disciplina">string</param>
        /// <param name="municipio">string</param>
        /// <returns>List<string> endereços de email</returns>
        public string MontarMalaDireta(string candidato, string concurso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            string email = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT  cd.e_mail ,
                        cd.candidato AS candidato ,
                        nome ,
                        DT_NASC AS datanasc ,
                        status ,
                        ( SELECT    SUM(ctpont.PONTUACAO)
                          FROM      ( (SELECT   pontuacao
                                       FROM     LY_CONCURSO_DOC_EXPERIENCIA code
                                                JOIN LY_CANDIDATO_DOC_EXPERIENCIAS cade ON code.CONCURSO = cade.CONCURSO
                                                                              AND code.EXPERIENCIA = cade.EXPERIENCIA
                                       WHERE    cade.CANDIDATO = cd.candidato
                                                AND cade.CONCURSO = cd.CONCURSO )
                                      UNION ALL
                                      ( SELECT  pontuacao
                                        FROM    LY_CONCURSO_DOC_TITULACOES codt
                                                JOIN LY_CANDIDATO_DOC_TITULACOES cadt ON codt.CONCURSO = cadt.CONCURSO
                                                                              AND codt.TITULACAO = cadt.TITULACAO
                                        WHERE   cadt.CANDIDATO = cd.candidato
                                                AND cadt.CONCURSO = cd.CONCURSO
                                      )
                                    ) ctpont
                        ) AS pontuacao
                FROM    LY_CANDIDATO_DOCENTE cd
                WHERE   CANDIDATO = @CANDIDATO
                        AND cd.CONCURSO = @CONCURSO ";

                contextQuery.Parameters.Add("@CANDIDATO", candidato);
                contextQuery.Parameters.Add("@CONCURSO", concurso);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    if (reader["e_mail"] != DBNull.Value)
                    {
                        email = reader["e_mail"].ToString();
                    }
                }

                return email;
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
                if (reader != null)
                {
                    reader.Close();
                }

                if (ctx != null)
                {
                    ctx.Dispose();
                }
            }
        }

        public DataTable RetornaEnderecoCoordenadoria(string idregional)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = new DataTable();

            try
            {
                contextQuery.Command = @" SELECT isnull(RE.LOGRADOURO,'') + ',' +  isnull(RE.NUMERO,'') + ' ' +  isnull(RE.COMPLEMENTO,'') + '-' +  isnull(RE.BAIRRO,'') + '-' 
                +  isnull(MU.NOME,'') + '-' +  isnull(RE.REGIONAL,'') + '-' +  isnull(RE.CEP,'') as ENDERECO 
                FROM TCE_REGIONAL RE INNER JOIN MUNICIPIO MU ON MU.CODIGO = RE.MUNICIPIO 
                WHERE ID_REGIONAL = @IDREGIONAL ";

                contextQuery.Parameters.Add("@IDREGIONAL", idregional);


                dt = ctx.GetDataTable(contextQuery);

            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return dt;

        }

        public DataTable CarregaSituacaoPropostaContrato()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = new DataTable();

            try
            {
                contextQuery.Command = "SELECT STATUSID, DESCRICAO FROM LY_CANDIDATO_DOCENTE_STATUS WHERE STATUSID IN(5,22,21,24) order by DESCRICAO";


                dt = ctx.GetDataTable(contextQuery);

            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return dt;

        }

        public static QueryTable SelecionarAprovados(string concurso, string disciplina, DateTime data, DateTime hora, string municipio, string nucleo, string strCota)
        {
            string sql = @" SELECT  cd.candidato AS candidato ,
                        nome ,
                        DT_NASC AS datanasc ,
                        status ,
                        DT_CONVOCACAO ,
                        ( SELECT    SUM(ctpont.PONTUACAO)
                          FROM      ( (SELECT   pontuacao
                                       FROM     LY_CONCURSO_DOC_EXPERIENCIA code
                                                JOIN LY_CANDIDATO_DOC_EXPERIENCIAS cade ON code.CONCURSO = cade.CONCURSO
                                                                              AND code.EXPERIENCIA = cade.EXPERIENCIA
                                       WHERE    cade.CANDIDATO = cd.candidato
                                                AND cade.CONCURSO = CD.CONCURSO )
                                      UNION ALL
                                      ( SELECT  pontuacao
                                        FROM    LY_CONCURSO_DOC_TITULACOES codt
                                                JOIN LY_CANDIDATO_DOC_TITULACOES cadt ON codt.CONCURSO = cadt.CONCURSO
                                                                              AND codt.TITULACAO = cadt.TITULACAO
                                        WHERE   cadt.CANDIDATO = cd.candidato
                                                AND cadt.CONCURSO = cd.CONCURSO
                                      )
                                    ) ctpont
                        ) AS pontuacao
                FROM    LY_CANDIDATO_DOCENTE cd
                WHERE   CD.CONCURSO = ?
                        AND CD.AGRUPAMENTO_INGRESSO = ?
                        AND STATUS = 2
                        AND CD.DT_APRESENTACAO = ?
                        AND CD.HORA_APRESENTACAO = ?
                        AND CD.MUNICIPIO_PROC = ?
                        AND CD.NUCLEO = ?  ";

            if (Convert.ToInt32(strCota) != 3)
            {
                sql += @" and cd.COTAIDCONVOCACAO = ? 
                        order by pontuacao desc, DT_NASC asc ";
                return Consultar(sql, concurso, disciplina, data, hora, municipio, nucleo, strCota);
            }
            sql += "order by pontuacao desc, DT_NASC asc ";

            return Consultar(sql, concurso, disciplina, data, hora, municipio, nucleo);
        }

        public static DataTable SelecionarConvocadosEAprovados(string concurso, string nucleo, string municipio)
        {
            DataTable dt = null;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  cd.candidato AS candidato ,
                                    nome ,
                                    dt_nasc AS datanasc ,
                                    dhr_cadastro AS dtinscricao ,
                                    status ,
                                    ( SELECT    SUM(ctpont.pontuacao)
                                      FROM      ( (SELECT   pontuacao
                                                   FROM     ly_concurso_doc_experiencia code
                                                            INNER JOIN ly_candidato_doc_experiencias cade ON code.concurso = cade.concurso
                                                                                          AND code.experiencia = cade.experiencia
                                                   WHERE    cade.candidato = cd.candidato
                                                            AND cade.CONCURSO = cd.CONCURSO
		                                                                    )
                                                  UNION ALL
                                                  ( SELECT  pontuacao
                                                    FROM    ly_concurso_doc_titulacoes codt
                                                            INNER JOIN ly_candidato_doc_titulacoes cadt ON codt.concurso = cadt.concurso
                                                                                          AND codt.titulacao = cadt.titulacao
                                                    WHERE   cadt.candidato = cd.candidato
                                                            AND cadt.CONCURSO = cd.CONCURSO
                                                  )
                                                ) ctpont
                                    ) AS pontuacao ,
                                    ct.sigla AS cota ,
                                    gh.descricao AS disciplina
                            FROM    ly_candidato_docente cd
                                    INNER JOIN ly_grupo_habilitacao gh ON gh.agrupamento = cd.agrupamento_ingresso
                                    INNER JOIN contratotemporario.cota ct ON ct.cotaid = cd.cotaidconvocacao
                            WHERE   1 = 1
                                    AND cd.concurso = @concurso
                                    AND cd.nucleo = @nucleo
                                    AND cd.municipio_proc = @municipio
                                    AND status IN ( 2, 24 )
                            UNION
                            SELECT  cd.candidato AS candidato ,
                                    nome ,
                                    dt_nasc AS datanasc ,
                                    dhr_cadastro AS dtinscricao ,
                                    status ,
                                    ( SELECT    SUM(ctpont.pontuacao)
                                      FROM      ( (SELECT   pontuacao
                                                   FROM     ly_concurso_doc_experiencia code
                                                            INNER JOIN ly_candidato_doc_experiencias cade ON code.concurso = cade.concurso
                                                                                          AND code.experiencia = cade.experiencia
                                                   WHERE    cade.candidato = cd.candidato
                                                            AND cade.CONCURSO = cd.CONCURSO
		                                                                    )
                                                  UNION ALL
                                                  ( SELECT  pontuacao
                                                    FROM    ly_concurso_doc_titulacoes codt
                                                            INNER JOIN ly_candidato_doc_titulacoes cadt ON codt.concurso = cadt.concurso
                                                                                          AND codt.titulacao = cadt.titulacao
                                                    WHERE   cadt.candidato = cd.candidato
                                                            AND cadt.CONCURSO = cd.CONCURSO
                                                  )
                                                ) ctpont
                                    ) AS pontuacao ,
                                    ct.sigla AS cota ,
                                    gh.descricao AS disciplina
                            FROM    ly_candidato_docente cd
                                    INNER JOIN contratotemporario.candidatodocente_grupohabilitacao cg ON cd.concurso = cg.concurso
                                                                                          AND cd.candidato = cg.candidato
                                    INNER JOIN ly_grupo_habilitacao gh ON gh.agrupamento = cg.agrupamento
                                    INNER JOIN contratotemporario.cota ct ON ct.cotaid = cd.cotaidinscricao
                            WHERE   1 = 1
                                    AND cd.concurso = @concurso
                                    AND cd.nucleo = @nucleo
                                    AND cd.municipio_proc = @municipio
                                    AND cg.habilitado = 1
                                    AND status IN ( 1, 23, 26 )
                            ORDER BY pontuacao DESC ,
                                    dt_nasc ASC ";

                contextQuery.Parameters.Add("@concurso", concurso);
                contextQuery.Parameters.Add("@nucleo", nucleo);
                contextQuery.Parameters.Add("@municipio", municipio);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}", Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return dt;
        }

        /// <summary>
        /// Consulta o status de um candidato a partir de seu processo seletivo e número de inscrição .
        /// </summary>
        /// <param name="processoSeletivo">Processo Seletivo do Candidato</param>
        /// /// <param name="numeroInscricao">Número de Inscrição do Candidato</param>
        public static Status ConsultarStatusCandidato(string processoSeletivo, string numeroInscricao)
        {
            string sql = @"SELECT status FROM ly_candidato_docente WHERE concurso = ? AND candidato = ?";
            return (Status)Enum.Parse(typeof(Status), Convert.ToString(RNBase.ExecutarFuncao(sql, processoSeletivo, numeroInscricao)));
        }

        public static QueryTable ObterCoordenadoria()
        {
            string sql = @"select distinct E.nucleo, N.descricao from VW_UNIDADE_ENSINO_SITUACAO (NOLOCK) S join LY_UNIDADE_ENSINO (NOLOCK) E on S.UNIDADE_ENS = E.UNIDADE_ENS join LY_NUCLEO (NOLOCK) N on N.NUCLEO = E.NUCLEO";
            return Consultar(sql);
        }

        public static QueryTable ObterRegional()
        {
            string sql = @"select distinct E.ID_REGIONAL, N.REGIONAL from VW_UNIDADE_ENSINO_SITUACAO (NOLOCK) S join LY_UNIDADE_ENSINO (NOLOCK) E on S.UNIDADE_ENS = E.UNIDADE_ENS join TCE_REGIONAL (NOLOCK) N on N.ID_REGIONAL = E.ID_REGIONAL";
            return Consultar(sql);
        }

        public static int ConsultarAulasAlocadasRH(string concurso, string candidato)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int ordem = 0;

            try
            {
                contextQuery.Command = @"select count(*) as qtd
									from LY_CANDIDATO_DOCENTE cd join LY_DOCENTE d
									on  cd.CONCURSO = d.CONCURSO
									and cd.CANDIDATO = d.CANDIDATO
									join LY_AULA_DOCENTE a
									on d.NUM_FUNC = a.NUM_FUNC
									where cd.CONCURSO = @CONCURSO
									and cd.CANDIDATO = @CANDIDATO
									AND  A.DATA_INICIO < GETDATE()
                                    AND A.DATA_FIM >=GETDATE() ";

                contextQuery.Parameters.Add("@CONCURSO", concurso);
                contextQuery.Parameters.Add("@CANDIDATO", candidato);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    ordem = Convert.ToInt32(reader["qtd"]);
                }

                return ordem;
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }

        public static void AprovarRescisao(LyLotacao dadoslotacao, LyDocente dadosDocente, string strNumFunc, string strObservacao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            RN.Lotacao lotacao = new Techne.Lyceum.RN.Lotacao();
            RN.Docentes docente = new Techne.Lyceum.RN.Docentes();
            RN.CandidatoDocente candidatoDocente = new CandidatoDocente();

            try
            {
                lotacao.DesativaLotacao(ctx, Convert.ToDateTime(dadoslotacao.DataDesativacao), dadoslotacao.Usuario, dadoslotacao.Matricula);
                docente.AtualizaDemissaoDocente(ctx, Convert.ToDateTime(dadoslotacao.DataDesativacao), strNumFunc);
                candidatoDocente.AtualizaCandidatoDocenteContrato(ctx, dadosDocente.Concurso, dadosDocente.Candidato, Convert.ToDateTime(dadoslotacao.DataDesativacao), RN.ProcessoSeletivo.StatusContrato.Rescindido.GetStringValue());
                candidatoDocente.AtualizaCandidatoDocenteStatus(ctx, strObservacao, dadosDocente.Candidato, dadosDocente.Concurso, Convert.ToInt32(RN.ProcessoSeletivo.Status.ContratoRescindido));
                candidatoDocente.IncluirCandidatoDocenteSolicitacoes(ctx, dadosDocente.Concurso, dadosDocente.Candidato, Convert.ToDateTime(dadoslotacao.DataDesativacao));
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
                } throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public static void AprovarAlteracaoCargaHoraria(string strConcurso, string strCandidato, string strCargaHoraria, string strCargaNova, string strTipo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            RN.CandidatoDocente candidatoDocente = new CandidatoDocente();
            RN.Docentes rnDocentes = new Docentes();

            try
            {
                candidatoDocente.AtualizaCargaHoraria(ctx, strConcurso, strCandidato, strCargaNova);
                rnDocentes.AtualizaCargaHorariaPor(ctx, strConcurso, strCandidato, strCargaNova);
                candidatoDocente.IncluirCandidatoDocenteSolicitacoes(ctx, strCandidato, strConcurso, strCargaHoraria, strCargaNova, strTipo);
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
                } throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public static QueryTable ConsultarListaCandidatos(DbObject concurso, DbObject nucleo, string categoria, DbObject agrupamento, DbObject candidato)
        {
            //Usado pela tela ListaCandidatos
            string sql = @" SELECT  cd.CANDIDATO AS candidato ,
                            cd.NOME AS nome ,
                            ISNULL(( SELECT SUM(ctpont.PONTUACAO)
                                     FROM   ( (SELECT   pontuacao
                                               FROM     LY_CONCURSO_DOC_EXPERIENCIA code
                                                        JOIN LY_CANDIDATO_DOC_EXPERIENCIAS cade ON code.CONCURSO = cade.CONCURSO
                                                                                  AND code.EXPERIENCIA = cade.EXPERIENCIA
                                               WHERE    cade.CANDIDATO = cd.candidato
                                                        AND cade.CONCURSO = cd.CONCURSO )
                                              UNION ALL
                                              ( SELECT  pontuacao
                                                FROM    LY_CONCURSO_DOC_TITULACOES codt
                                                        JOIN LY_CANDIDATO_DOC_TITULACOES cadt ON codt.CONCURSO = cadt.CONCURSO
                                                                                  AND codt.TITULACAO = cadt.TITULACAO
                                                WHERE   cadt.CANDIDATO = cd.candidato
                                                        AND cadt.concurso = cd.CONCURSO
                                              )
                                            ) ctpont
                                   ), 0) AS pontuacao ,
                            cd.STATUS AS situacao
                    FROM    LY_CANDIDATO_DOCENTE cd
                    WHERE   cd.CONCURSO = ?
                            AND cd.NUCLEO = ?
                            AND cd.CATEGORIA = ?
                            AND cd.AGRUPAMENTO_INGRESSO = ?  ";
            if (!candidato.IsNull)
            {
                sql += @" and cd.CANDIDATO = ? 
        					order by pontuacao desc, cd.DT_NASC asc";
                return Consultar(sql, concurso, nucleo, categoria, agrupamento, candidato);
            }
            else
                sql += @" order by pontuacao desc, cd.DT_NASC asc";
            return Consultar(sql, concurso, nucleo, categoria, agrupamento);
        }

        /// <summary>
        /// Verifica se existe a data de apresentação é válida ou não.
        /// DEFINIÇÃO: A data de apresentação não pode ser menor que 48 horas após a data de convocação
        /// dos candidatos selecionados no processo seletivo, desconsiderados sábados e domingos.
        /// </summary>
        /// <param name="idProcessoSeletivo">Identificador do Processo Seletivo</param>
        /// <param name="dataApresentacao">Data de Apresentação do Processo Seletivo</param>
        public static bool ValidaDataApresentacao(string idProcessoSeletivo, DateTime dataApresentacao)
        {
            if (String.IsNullOrEmpty(idProcessoSeletivo))
                return false;
            TConnection connection = Config.CreateConnection();
            DateTime dataConvocacao = DateTime.Now.Date;
            DateTime dataApresentacaoMinima = new DateTime();
            if (dataConvocacao.DayOfWeek.Equals(DayOfWeek.Thursday))
                dataApresentacaoMinima = dataConvocacao.AddDays(4D);
            else if (dataConvocacao.DayOfWeek.Equals(DayOfWeek.Friday))
                dataApresentacaoMinima = dataConvocacao.AddDays(3D);
            else
                dataApresentacaoMinima = dataConvocacao.AddDays(2D);
            return dataApresentacao >= dataApresentacaoMinima;
        }

        /// <summary>
        /// Converte o status do candidato de consulta pública para o status usado em Contrato Temporário.
        /// </summary>
        /// <param name="status">Status utilizado para Contrato Temporário</param>
        /// <returns></returns>
        public static StatusPublico ConverteStatusCandidatoParaStatusCandidatoPublico(Status status)
        {
            if (status.Equals(RN.ProcessoSeletivo.Status.Aguardando))
                return StatusPublico.Aguardando;
            if (status.Equals(RN.ProcessoSeletivo.Status.Convocado) ||
                status.Equals(RN.ProcessoSeletivo.Status.PropostaContratoTemporario) ||
                status.Equals(RN.ProcessoSeletivo.Status.AguardandoRemessaAprovacao) ||
                status.Equals(RN.ProcessoSeletivo.Status.Aprovado) ||
                status.Equals(RN.ProcessoSeletivo.Status.AguardandoAvaliacaoAmpliacaoCargaHoraria) ||
                status.Equals(RN.ProcessoSeletivo.Status.AguardandoRemessaAmpliacaoCargaHoraria) ||
                status.Equals(RN.ProcessoSeletivo.Status.AguardandoAvaliacaoReducaoCargaHoraria) ||
                status.Equals(RN.ProcessoSeletivo.Status.AguardandoRemessaReducaoCargaHoraria) ||
                status.Equals(RN.ProcessoSeletivo.Status.AguardandoAvaliacaoRescisao) ||
                status.Equals(RN.ProcessoSeletivo.Status.AguardandoRemessaRescisao) ||
                status.Equals(RN.ProcessoSeletivo.Status.ContratoCargaHorariaAmpliada) ||
                status.Equals(RN.ProcessoSeletivo.Status.ContratoCargaHorariaReduzida))
                return StatusPublico.Aprovado;
            if (status.Equals(RN.ProcessoSeletivo.Status.Reprovado) ||
                status.Equals(RN.ProcessoSeletivo.Status.ReprovadoRH) ||
                status.Equals(RN.ProcessoSeletivo.Status.Desativado) ||
                status.Equals(RN.ProcessoSeletivo.Status.Desistente) ||
                status.Equals(RN.ProcessoSeletivo.Status.Cancelado) ||
                status.Equals(RN.ProcessoSeletivo.Status.ContratoRescindido))
                return StatusPublico.Reprovado;
            if (status.Equals(RN.ProcessoSeletivo.Status.ReprovadoQHI) ||
                status.Equals(RN.ProcessoSeletivo.Status.Erro))
                return StatusPublico.Reprovado;
            return StatusPublico.Reprovado;
        }

        public static void Remover(string concurso, string candidato, string nome, string cpf, string usuario)
        {
            RN.LiberacaoCandidatoDocente rnLiberacaoCandidatoDocente = new Techne.Lyceum.RN.LiberacaoCandidatoDocente();
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    CandidatoDocExperiencias.RemoverExperiencia(ctx, concurso, candidato);
                    CandidatoDocTitulacoes.RemoverTitulacao(ctx, concurso, candidato);
                    RemoverGrupoHabilitacao(ctx, concurso, candidato);
                    RemoverCandidato(ctx, concurso, candidato);
                    rnLiberacaoCandidatoDocente.InserirLiberacaoCandidato(ctx, concurso, candidato, nome, cpf, usuario);

                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        private static void RemoverCandidato(DataContext dataContext, string concurso, string candidato)
        {
            var contextQuery = new ContextQuery(
                @"DELETE dbo.LY_CANDIDATO_DOCENTE
                                        WHERE CONCURSO = @CONCURSO AND CANDIDATO=@CANDIDATO");

            contextQuery.Parameters.Add("@CONCURSO", concurso);
            contextQuery.Parameters.Add("@CANDIDATO", candidato);

            dataContext.ApplyModifications(contextQuery);
        }

        private static void RemoverGrupoHabilitacao(DataContext dataContext, string concurso, string candidato)
        {
            var contextQuery = new ContextQuery(@"DELETE from contratotemporario.CANDIDATODOCENTE_GRUPOHABILITACAO
												WHERE CONCURSO = @CONCURSO AND CANDIDATO=@CANDIDATO");

            contextQuery.Parameters.Add("@CONCURSO", concurso);
            contextQuery.Parameters.Add("@CANDIDATO", candidato);

            dataContext.ApplyModifications(contextQuery);
        }

        public static QueryTable ConsultarCargaHoraria(string strConcurso)
        {
            string sql = @" SELECT MAX(CH.CARGAHORARIAREGENCIA) AS CH_SEMANAL_EFETIVA 
                            FROM   LY_CONCURSO_DOCENTE CD WITH (NOLOCK) 
                                   INNER JOIN CONTRATOTEMPORARIO.CONCURSO_DOCENTE__CATEGORIA_DOCENTE CDCD WITH (NOLOCK) 
                                           ON CD.CONCURSO = CDCD.CONCURSOID 
                                   INNER JOIN LY_CATEGORIA_DOCENTE CATD WITH (NOLOCK) 
                                           ON CATD.CATEGORIA = CDCD.CATEGORIAID 
                                   INNER JOIN RECURSOSHUMANOS.CH_AGRUPAMENTOCARGO CH (NOLOCK) 
                                           ON CATD.FUNCAO = CH.FUNCAO 
                                              AND CATD.AGRUPAMENTOCARGOSID = CH.AGRUPAMENTOCARGOSID 
                            WHERE  CD.CONCURSO = ?  ";
            return Consultar(sql, strConcurso);
        }

        public DataTable ListaCoordenadoriaPor(string municipio, string processo)
        {
            DataTable coordenadoria = null;
            ContextQuery contextQuery = new ContextQuery();

            if (!string.IsNullOrEmpty(municipio) && !string.IsNullOrEmpty(processo))
            {
                try
                {
                    contextQuery.Command = @"   SELECT DISTINCT
                                                n.nucleo AS nucleo ,
                                                n.descricao AS descricao
                                      FROM      dbo.MUNICIPIO_NUCLEO mn
                                                INNER JOIN dbo.ly_nucleo n ON mn.nucleoid = n.nucleo
                                                INNER JOIN dbo.LY_CONCURSO_DOC_HABILITACAO ldh ON n.nucleo = ldh.nucleo
                                      WHERE     ldh.Concurso = @PROCESSO
                                                AND ldh.MUNICIPIO_PROC = @MUNICIPIO ";

                    contextQuery.Parameters.Add("@MUNICIPIO", municipio);
                    contextQuery.Parameters.Add("@PROCESSO", processo);

                    coordenadoria = Consultar(contextQuery);
                }
                catch (Exception exception)
                {
                    string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine, Convert.ToString(exception.Message));
                    throw new Exception(mensagem);
                }
            }

            return coordenadoria;
        }

        public DataTable ListaMunicipioPor(string strCoordenadoria, string strMunicipio)
        {
            DataTable municipio = null;
            ContextQuery contextQuery = new ContextQuery();

            if (!string.IsNullOrEmpty(strCoordenadoria) && !string.IsNullOrEmpty(strMunicipio))
            {
                try
                {
                    contextQuery.Command = @"   SELECT * from MUNICIPIO_NUCLEO
                                                WHERE NUCLEOID = @NUCLEO 
												AND MUNICIPIOID = @MUNICIPIO ";

                    contextQuery.Parameters.Add("@NUCLEO", strCoordenadoria);
                    contextQuery.Parameters.Add("@MUNICIPIO", strMunicipio);

                    municipio = Consultar(contextQuery);
                }
                catch (Exception exception)
                {
                    string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine, Convert.ToString(exception.Message));
                    throw new Exception(mensagem);
                }
            }

            return municipio;
        }

        public DataTable RetornaExistenciaHabilitacaoPor(string strConcurso, string strMunicipioProc, string strAgrupamento, int intNucleo)
        {
            DataTable habilitacao = null;
            ContextQuery contextQuery = new ContextQuery();

            if (!string.IsNullOrEmpty(strConcurso) && !string.IsNullOrEmpty(strMunicipioProc) && !string.IsNullOrEmpty(strAgrupamento) && intNucleo != 0)
            {
                try
                {
                    contextQuery.Command = @"   SELECT * from LY_CANDIDATO_DOCENTE
                                                WHERE CONCURSO = @CONCURSO 
												AND   MUNICIPIO_PROC = @MUNICIPIO 
												AND	AGRUPAMENTO_INGRESSO = @AGRUPAMENTO
												AND	NUCLEO = @NUCLEO 
												";

                    contextQuery.Parameters.Add("@CONCURSO", strConcurso);
                    contextQuery.Parameters.Add("@MUNICIPIO", strMunicipioProc);
                    contextQuery.Parameters.Add("@AGRUPAMENTO", strAgrupamento);
                    contextQuery.Parameters.Add("@NUCLEO", intNucleo);

                    habilitacao = Consultar(contextQuery);
                }
                catch (Exception exception)
                {
                    string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine, Convert.ToString(exception.Message));
                    throw new Exception(mensagem);
                }
            }

            return habilitacao;
        }

        public static DataTable RetornaSituacaoPropostaContrato()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable situacao = null;

            try
            {
                contextQuery.Command = @"SELECT STATUSID,DESCRICAO
										FROM LY_CANDIDATO_DOCENTE_STATUS WITH (NOLOCK)
										WHERE STATUSID IN(2,24,25)
										ORDER BY DESCRICAO ";

                situacao = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return situacao;
        }

        public string ObtemCategoriaPor(string strConcurso, decimal decCargaHoraria)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            string strCategoria = string.Empty;

            try
            {
                contextQuery.Command = @"SELECT CATD.CATEGORIA 
                                    FROM   LY_CONCURSO_DOCENTE CD WITH (NOLOCK) 
                                           INNER JOIN CONTRATOTEMPORARIO.CONCURSO_DOCENTE__CATEGORIA_DOCENTE CDCD WITH (NOLOCK) 
                                                   ON CD.CONCURSO = CDCD.CONCURSOID 
                                           INNER JOIN LY_CATEGORIA_DOCENTE CATD WITH (NOLOCK) 
                                                   ON CATD.CATEGORIA = CDCD.CATEGORIAID 
                                           INNER JOIN RECURSOSHUMANOS.CH_AGRUPAMENTOCARGO CHC (NOLOCK) 
                                                   ON CATD.FUNCAO = CHC.FUNCAO 
                                                      AND CATD.AGRUPAMENTOCARGOSID = CHC.AGRUPAMENTOCARGOSID 
                                    WHERE  ( CD.CONCURSO = @CONCURSO ) 
                                           AND CHC.CARGAHORARIAREGENCIA = (SELECT MIN(CH2.CARGAHORARIAREGENCIA) 
                                                           FROM LY_CONCURSO_DOCENTE CD2 WITH (NOLOCK) 
					                                       INNER JOIN  CONTRATOTEMPORARIO.CONCURSO_DOCENTE__CATEGORIA_DOCENTE CDCD2 WITH (NOLOCK) 
							                                       ON CD2.CONCURSO = CDCD2.CONCURSOID 
					                                       INNER JOIN LY_CATEGORIA_DOCENTE CATD2 WITH (NOLOCK) 
							                                       ON CATD2.CATEGORIA = CDCD2.CATEGORIAID 
					                                       INNER JOIN RECURSOSHUMANOS.CH_AGRUPAMENTOCARGO CH2 (NOLOCK) 
							                                       ON CATD2.FUNCAO = CH2.FUNCAO 
								                                      AND CATD2.AGRUPAMENTOCARGOSID = CH2.AGRUPAMENTOCARGOSID 
						                                    WHERE  ( @CH_SEMANAL_EFETIVA BETWEEN 1 AND CH2.CARGAHORARIAREGENCIA ) 
							                                       AND ( CD2.CONCURSO = @CONCURSO  ))  ";

                contextQuery.Parameters.Add("@CH_SEMANAL_EFETIVA", decCargaHoraria);
                contextQuery.Parameters.Add("@CONCURSO", strConcurso);

                strCategoria = ctx.GetReturnValue<string>(contextQuery);

                return strCategoria;
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

        public static DataTable ConsultarCargaHorariaPor(string strConcurso, string strCategoria)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dtCargaHoraria = null;

            try
            {

                contextQuery.Command = @"SELECT CONVERT(INT, CHE.CARGAHORARIAREGENCIA) AS CH_SEMANAL_EFETIVA, 
                                       (SELECT CONVERT(INT, COALESCE(MAX(CH.CARGAHORARIAREGENCIA), 0) + 1) 
                                        FROM   LY_CONCURSO_DOCENTE CD (NOLOCK) 
                                               INNER JOIN CONTRATOTEMPORARIO.CONCURSO_DOCENTE__CATEGORIA_DOCENTE CDCD  WITH (NOLOCK) 
                                                       ON ( CD.CONCURSO = CDCD.CONCURSOID ) 
                                               INNER JOIN LY_CATEGORIA_DOCENTE CAT (NOLOCK) 
                                                       ON CAT.CATEGORIA = CDCD.CATEGORIAID 
                                               INNER JOIN RECURSOSHUMANOS.CH_AGRUPAMENTOCARGO CH (NOLOCK) 
                                                       ON CAT.FUNCAO = CH.FUNCAO 
                                                          AND CAT.AGRUPAMENTOCARGOSID = CH.AGRUPAMENTOCARGOSID 
                                        WHERE  CD.CONCURSO = CDOC.CONCURSO 
                                               AND CH.CARGAHORARIAREGENCIA < CHE.CARGAHORARIAREGENCIA) AS MENORVALOR 
                                FROM   LY_CONCURSO_DOCENTE CDOC WITH (NOLOCK) 
                                       INNER JOIN CONTRATOTEMPORARIO.CONCURSO_DOCENTE__CATEGORIA_DOCENTE CDC WITH (NOLOCK) 
                                               ON CDOC.CONCURSO = CDC.CONCURSOID 
                                       INNER JOIN LY_CATEGORIA_DOCENTE CATD WITH (NOLOCK) 
                                               ON CATD.CATEGORIA = CDC.CATEGORIAID 
                                       INNER JOIN RECURSOSHUMANOS.CH_AGRUPAMENTOCARGO CHE (NOLOCK) 
                                               ON CATD.FUNCAO = CHE.FUNCAO 
                                                  AND CATD.AGRUPAMENTOCARGOSID = CHE.AGRUPAMENTOCARGOSID 
                                WHERE  CDOC.CONCURSO = @CONCURSO 
                                       AND CATD.CATEGORIA = @CATEGORIA ";

                contextQuery.Parameters.Add("@CONCURSO", strConcurso);
                contextQuery.Parameters.Add("@CATEGORIA", strCategoria);

                dtCargaHoraria = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                    mensagem = Convert.ToString(ex.Message);

                throw new Exception(mensagem);
            }
            return dtCargaHoraria;
        }

        public DataTable ObtemDadosFolhaPagamentoPor(string concurso, string candidato)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT UE.UNIDADE_ENS AS UNIDADE_ENS,
                                               UE.NOME_COMP   AS NOME_COMP,
                                               PDB.BANCO      AS BANCO,
                                               PDB.AGENCIA    AS AGENCIA,
                                               PDB.CONTABANCO AS CONTA,
                                               DO.DT_ADMISSAO AS DT_ADMISSAO
                                        FROM   LY_DOCENTE DO
                                               JOIN LY_LOTACAO L
                                                 ON L.MATRICULA = DO.MATRICULA
                                               LEFT JOIN LY_UNIDADE_ENSINO UE
                                                      ON UE.UNIDADE_ENS = L.UNIDADE_ENS
                                               JOIN LY_PESSOA PE
                                                 ON PE.PESSOA = DO.PESSOA
                                               LEFT JOIN [RECURSOSHUMANOS].[PESSOADADOSBANCARIOS] PDB
                                                      ON PDB.PESSOAID = PE.PESSOA
                                                      AND PDB.ATIVO = 1
                                        WHERE  DO.CONCURSO = @CONCURSO
                                               AND DO.CANDIDATO = @CANDIDATO ";

                contextQuery.Parameters.Add("@CONCURSO", concurso);
                contextQuery.Parameters.Add("@CANDIDATO", candidato);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return dt;
        }

        public DataTable ObtemCandidatoAvaliacaoRescisaoPor(string concurso, string candidato)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"  SELECT CD.NOME,
                                               CD.IDFUNCIONAL,
                                               CD.CPF,
                                               CT.SIGLA                AS COTA,
                                               CD.DT_PROPOSTA          AS DATAADMISSAO,
                                               CDS.DESCRICAO           AS SITUACAO,
                                               UE.UNIDADE_ENS,
                                               CD.CATEGORIA            AS CARGO,
                                               CD.CARGA_HORARIA,
                                               CDC.DT_FIM_CONTRATO     AS DATAULTIMOEXERCICIO,
                                               CD.STATUS_OBS           AS JUSTIFICATIVA,
                                               CD.MUNICIPIO_PROC,
                                               CD.REGIONALID,
                                               CD.NUCLEO               AS COORDENADORIAID,
                                               CD.AGRUPAMENTO_INGRESSO AS DISCIPLINA,
                                               DOC.PESSOA,
                                               DOC.MATRICULA,
                                               DOC.NUM_FUNC,
                                                ISNULL((CONVERT(VARCHAR, PE.IDFUNCIONAL) + '/' + CONVERT(VARCHAR ,DOC.VINCULO)),DOC.MATRICULA) IDVINCULO
                                        FROM   LY_CANDIDATO_DOCENTE CD
                                               INNER JOIN LY_CANDIDATO_DOCENTE_STATUS CDS
                                                       ON CDS.STATUSID = CD.STATUS
                                               INNER JOIN LY_DOCENTE DOC
                                                       ON CD.CONCURSO = DOC.CONCURSO
                                                          AND CD.CANDIDATO = DOC.CANDIDATO
                                               INNER JOIN LY_LOTACAO LT
                                                       ON DOC.MATRICULA = LT.MATRICULA
                                                          AND DOC.PESSOA = LT.PESSOA
                                               INNER JOIN LY_PESSOA PE ON PE.PESSOA = DOC.PESSOA
                                               INNER JOIN LY_UNIDADE_ENSINO UE
                                                       ON UE.UNIDADE_ENS = LT.UNIDADE_ENS
                                               INNER JOIN LY_GRUPO_HABILITACAO GH
                                                       ON GH.AGRUPAMENTO = CD.AGRUPAMENTO_INGRESSO
                                               INNER JOIN LY_CANDIDATO_DOC_CONTRATO CDC
                                                       ON CD.CANDIDATO = CDC.CANDIDATO
                                                          AND CD.CONCURSO = CDC.CONCURSO
                                               LEFT JOIN CONTRATOTEMPORARIO.COTA CT
                                                      ON CT.COTAID = CD.COTAIDCONVOCACAO
                                        WHERE  CD.CONCURSO = @CONCURSO
                                               AND CD.CANDIDATO = @CANDIDATO
                                               ";

                contextQuery.Parameters.Add("@CONCURSO", concurso);
                contextQuery.Parameters.Add("@CANDIDATO", candidato);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return dt;
        }

        public DataTable ObtemCandidatoAvaliacaoRHPor(string concurso, string candidato)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"   SELECT CD.NOME,
                                                   CD.IDFUNCIONAL,
                                                   CD.CPF,
                                                   CT.SIGLA                AS COTA,
                                                   CD.DT_PROPOSTA          AS DATAADMISSAO,
                                                   CDS.DESCRICAO           AS SITUACAO,
                                                   UE.UNIDADE_ENS,
                                                   CD.CATEGORIA            AS CARGO,
                                                   CD.CARGA_HORARIA,
                                                   CD.MUNICIPIO_PROC,
                                                   CD.REGIONALID,
                                                   CD.NUCLEO               AS COORDENADORIAID,
                                                   CD.AGRUPAMENTO_INGRESSO AS DISCIPLINA,
                                                   DOC.PESSOA,
                                                   DOC.MATRICULA,
                                                   DOC.NUM_FUNC,
                                                ISNULL((CONVERT(VARCHAR, PE.IDFUNCIONAL) + '/' + CONVERT(VARCHAR ,DOC.VINCULO)),DOC.MATRICULA) IDVINCULO

                                            FROM   LY_CANDIDATO_DOCENTE CD
                                                   INNER JOIN LY_CANDIDATO_DOCENTE_STATUS CDS
                                                           ON CDS.STATUSID = CD.STATUS
                                                   INNER JOIN LY_DOCENTE DOC
                                                           ON CD.CONCURSO = DOC.CONCURSO
                                                              AND CD.CANDIDATO = DOC.CANDIDATO
                                                   INNER JOIN LY_LOTACAO LT
                                                           ON DOC.MATRICULA = LT.MATRICULA
                                                              AND DOC.PESSOA = LT.PESSOA
                                                    INNER JOIN LY_PESSOA PE ON PE.PESSOA = DOC.PESSOA
                                                   INNER JOIN LY_UNIDADE_ENSINO UE
                                                           ON UE.UNIDADE_ENS = LT.UNIDADE_ENS
                                                   INNER JOIN LY_GRUPO_HABILITACAO GH
                                                           ON GH.AGRUPAMENTO = CD.AGRUPAMENTO_INGRESSO
                                                   LEFT JOIN CONTRATOTEMPORARIO.COTA CT
                                                          ON CT.COTAID = CD.COTAIDCONVOCACAO
                                        WHERE  CD.CONCURSO = @CONCURSO
                                               AND CD.CANDIDATO = @CANDIDATO
                                               ";

                contextQuery.Parameters.Add("@CONCURSO", concurso);
                contextQuery.Parameters.Add("@CANDIDATO", candidato);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return dt;
        }

        public static int ObtemTotalInscricoesDisponiveis(string processoSeletivo, int regional, string municipio)
        {
            string sql = @"SELECT COUNT(candidato) from LY_CANDIDATO_DOCENTE WHERE STATUS in (1, 2, 23, 24,26) and CONCURSO = ? and REGIONALID = ? ";

            if (!string.IsNullOrEmpty(municipio))
            {
                sql += " and MUNICIPIO_PROC = ?";
                return ExecutarFuncao(sql, processoSeletivo, regional, municipio);
            }

            return ExecutarFuncao(sql, processoSeletivo, regional);
        }

        public DataTable ObtemConvocadosEAprovadosPor(string concurso, string regional, string municipio)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"SELECT  cd.candidato AS candidato ,
                                    nome ,
                                    dt_nasc AS datanasc ,
                                    dhr_cadastro AS dtinscricao ,
                                    CONVERT(VARCHAR,status) AS STATUS ,
                                    ( SELECT    SUM(ctpont.pontuacao)
                                      FROM      ( (SELECT   pontuacao
                                                   FROM     ly_concurso_doc_experiencia code
                                                            INNER JOIN ly_candidato_doc_experiencias cade ON code.concurso = cade.concurso
                                                                                          AND code.experiencia = cade.experiencia
                                                   WHERE    cade.candidato = cd.candidato
                                                            AND cade.CONCURSO = cd.CONCURSO
		                                                                    )
                                                  UNION ALL
                                                  ( SELECT  pontuacao
                                                    FROM    ly_concurso_doc_titulacoes codt
                                                            INNER JOIN ly_candidato_doc_titulacoes cadt ON codt.concurso = cadt.concurso
                                                                                          AND codt.titulacao = cadt.titulacao
                                                    WHERE   cadt.candidato = cd.candidato
                                                            AND cadt.CONCURSO = cd.CONCURSO
                                                  )
                                                ) ctpont
                                    ) AS pontuacao ,
                                    ct.sigla AS cota ,
                                    gh.descricao AS disciplina,
                                    DS.DESCRICAO AS NOME_STATUS
                            FROM    ly_candidato_docente cd
                                    INNER JOIN ly_grupo_habilitacao gh ON gh.agrupamento = cd.agrupamento_ingresso
                                    INNER JOIN contratotemporario.cota ct ON ct.cotaid = cd.cotaidconvocacao
                                    INNER JOIN LY_CANDIDATO_DOCENTE_STATUS DS ON DS.STATUSID = CD.STATUS
                            WHERE   1 = 1
                                    AND cd.concurso = @concurso
                                    AND cd.regionalid = @regionalid
                                    AND cd.municipio_proc = @municipio
                                    AND status IN ( 2, 24 )
                            UNION
                            SELECT  cd.candidato AS candidato ,
                                    nome ,
                                    dt_nasc AS datanasc ,
                                    dhr_cadastro AS dtinscricao ,
                                    CONVERT(VARCHAR,status) AS STATUS ,
                                    ( SELECT    SUM(ctpont.pontuacao)
                                      FROM      ( (SELECT   pontuacao
                                                   FROM     ly_concurso_doc_experiencia code
                                                            INNER JOIN ly_candidato_doc_experiencias cade ON code.concurso = cade.concurso
                                                                                          AND code.experiencia = cade.experiencia
                                                   WHERE    cade.candidato = cd.candidato
                                                            AND cade.CONCURSO = cd.CONCURSO
		                                                                    )
                                                  UNION ALL
                                                  ( SELECT  pontuacao
                                                    FROM    ly_concurso_doc_titulacoes codt
                                                            INNER JOIN ly_candidato_doc_titulacoes cadt ON codt.concurso = cadt.concurso
                                                                                          AND codt.titulacao = cadt.titulacao
                                                    WHERE   cadt.candidato = cd.candidato
                                                            AND cadt.CONCURSO = cd.CONCURSO
                                                  )
                                                ) ctpont
                                    ) AS pontuacao ,
                                    ct.sigla AS cota ,
                                    gh.descricao AS disciplina,
                                    DS.DESCRICAO AS NOME_STATUS
                            FROM    ly_candidato_docente cd
                                    INNER JOIN contratotemporario.candidatodocente_grupohabilitacao cg ON cd.concurso = cg.concurso
                                                                                          AND cd.candidato = cg.candidato
                                    INNER JOIN ly_grupo_habilitacao gh ON gh.agrupamento = cg.agrupamento
                                    INNER JOIN contratotemporario.cota ct ON ct.cotaid = cd.cotaidinscricao
                                    INNER JOIN LY_CANDIDATO_DOCENTE_STATUS DS ON DS.STATUSID = CD.STATUS
                            WHERE   1 = 1
                                    AND cd.concurso = @concurso
                                    AND cd.regionalid = @regionalid
                                    AND cd.municipio_proc = @municipio
                                    AND cg.habilitado = 1
                                    AND status IN ( 1, 23, 26, 5,21,22 )
                            ORDER BY pontuacao DESC ,
                                    dt_nasc ASC  ";

                contextQuery.Parameters.Add("@REGIONALID", regional);
                contextQuery.Parameters.Add("@concurso", concurso);
                contextQuery.Parameters.Add("@municipio", municipio);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
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
                ctx.Dispose();
            }

            return dt;
        }

        public int ObtemInscricoesDisponiveisPor(string disciplina, string concurso, string municipio, string regional)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            int total;

            try
            {
                contextQuery.Command = @"SELECT Count(CD.CANDIDATO)
                                                FROM   LY_CANDIDATO_DOCENTE CD
                                                       INNER JOIN CONTRATOTEMPORARIO.CANDIDATODOCENTE_GRUPOHABILITACAO CG
                                                               ON CD.CANDIDATO = CG.CANDIDATO
                                                                  AND CD.CONCURSO = CG.CONCURSO
                                                WHERE  STATUS = 1
                                                       AND CG.HABILITADO = 1
                                                       AND CG.AGRUPAMENTO = @AGRUPAMENTO
                                                       AND CD.CONCURSO = @CONCURSO
                                                       AND CD.MUNICIPIO_PROC = @MUNICIPIO_PROC
                                                       AND CD.REGIONALID = @REGIONALID  ";


                contextQuery.Parameters.Add("@AGRUPAMENTO", disciplina);
                contextQuery.Parameters.Add("@CONCURSO", concurso);
                contextQuery.Parameters.Add("@MUNICIPIO_PROC", municipio);
                contextQuery.Parameters.Add("@REGIONALID", regional);


                total = ctx.GetReturnValue<int>(contextQuery);
            }
            catch (Exception ex)
            {
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
                ctx.Dispose();
            }

            return total;
        }

        public int ObtemInscricoesDisponiveisPor(string disciplina, string concurso, string municipio, string regional, string cota)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            int total;

            try
            {
                contextQuery.Command = @"SELECT Count(CD.CANDIDATO)
                                                FROM   LY_CANDIDATO_DOCENTE CD
                                                       INNER JOIN CONTRATOTEMPORARIO.CANDIDATODOCENTE_GRUPOHABILITACAO CG
                                                               ON CD.CANDIDATO = CG.CANDIDATO
                                                                  AND CD.CONCURSO = CG.CONCURSO
                                                WHERE  STATUS = 1
                                                       AND CG.HABILITADO = 1                                                      
                                                       AND CG.AGRUPAMENTO = @AGRUPAMENTO
                                                       AND CD.CONCURSO = @CONCURSO
                                                       AND CD.MUNICIPIO_PROC = @MUNICIPIO_PROC
                                                       AND CD.REGIONALID = @REGIONALID 
                                                       AND CD.COTAIDINSCRICAO = @COTAIDINSCRICAO
                                            ";


                contextQuery.Parameters.Add("@AGRUPAMENTO", disciplina);
                contextQuery.Parameters.Add("@CONCURSO", concurso);
                contextQuery.Parameters.Add("@MUNICIPIO_PROC", municipio);
                contextQuery.Parameters.Add("@REGIONALID", regional);
                contextQuery.Parameters.Add("@COTAIDINSCRICAO", cota);


                total = ctx.GetReturnValue<int>(contextQuery);
            }
            catch (Exception ex)
            {
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
                ctx.Dispose();
            }

            return total;
        }


        public static QueryTable SelecionarConvocadosPor(string qtd, string concurso, string disciplina, string municipio, string regional, string strCota)
        {
            string sql = string.Format(@"SELECT TOP {0}
                            cd.candidato AS candidato ,
                            nome ,
                            DT_NASC AS datanasc ,
                            status ,
                            ( SELECT    SUM(ctpont.PONTUACAO)
                              FROM      ( (SELECT   pontuacao
                                           FROM     LY_CONCURSO_DOC_EXPERIENCIA code
                                                    INNER JOIN LY_CANDIDATO_DOC_EXPERIENCIAS cade ON code.CONCURSO = cade.CONCURSO
                                                                                  AND code.EXPERIENCIA = cade.EXPERIENCIA
                                           WHERE    cade.CANDIDATO = cd.candidato
                                                    AND cade.CONCURSO = cd.CONCURSO )
                                          UNION ALL
                                          ( SELECT  pontuacao
                                            FROM    LY_CONCURSO_DOC_TITULACOES codt
                                                    JOIN LY_CANDIDATO_DOC_TITULACOES cadt ON codt.CONCURSO = cadt.CONCURSO
                                                                                  AND codt.TITULACAO = cadt.TITULACAO
                                            WHERE   cadt.CANDIDATO = cd.candidato
                                                    AND cadt.CONCURSO = cd.CONCURSO
                                          )
                                        ) ctpont
                            ) AS pontuacao
                    FROM    LY_CANDIDATO_DOCENTE cd
                            INNER JOIN contratotemporario.CANDIDATODOCENTE_GRUPOHABILITACAO cg ON cg.CONCURSO = cd.CONCURSO
                                                                                  AND cg.CANDIDATO = cd.CANDIDATO
                    WHERE   cd.CONCURSO = ?
                            AND cg.AGRUPAMENTO = ?
                            AND cd.STATUS = 1
                            AND CG.HABILITADO = 1
                            AND cd.MUNICIPIO_PROC = ?
                            AND cd.REGIONALID = ? ", qtd);

            if (Convert.ToInt32(strCota) != 3)
            {
                sql += @" and cd.COTAIDINSCRICAO = ? 
                            order by pontuacao desc, DT_NASC asc ";
                return Consultar(sql, concurso, disciplina, municipio, regional, Convert.ToInt32(strCota));
            }

            sql += " order by pontuacao desc, DT_NASC asc ";
            return Consultar(sql, concurso, disciplina, municipio, regional);
        }

        public static RetValue ExecutaConvocaReprovaPor(string qtd, string concurso, string disciplina, DateTime data, DateTime hora, string municipio, int regional, int intCota, ref QueryTable qt)
        {
            RetValue retorno;
            retorno = null;
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            retorno = ConvocarCandidatos(connection, qtd, concurso, disciplina, data, data, municipio, regional, intCota, ref qt);

            if (retorno != null)
            {
                if (!retorno.Ok)
                {
                    connection.Rollback();
                    connection.Close();
                    return retorno;
                }
            }

            connection.Close();
            return new RetValue(true, "Candidatos convocados com sucesso.", null); ;
        }
        public static RetValue ConvocarCandidatos(TConnectionWritable connection, string qtd, string concurso, string disciplina, DateTime data, DateTime hora, string municipio, int regional, int intCota, ref QueryTable qt)
        {
            RetValue retorno = null;
            
            CR.Ly_candidato_docente dt = new CR.Ly_candidato_docente();
            qt = RN.ProcessoSeletivo.SelecionarConvocadosPor(qtd, concurso, disciplina, municipio, regional.ToString(), intCota.ToString());

            if (qt != null)
            {
                
                PopularLyCandidatoDocente(qt, dt, concurso, data, hora, regional, intCota, disciplina);
                retorno = AtualizarCandidatos(connection, dt);
            }

            return retorno;
        }

        public static QueryTable SelecionarAprovadosPor(string qtd, string concurso, string disciplina, DateTime data, DateTime hora, string municipio, string regional, string strCota)
        {
            string sql = string.Format(@" SELECT TOP {0} cd.candidato AS candidato ,
                        nome ,
                        DT_NASC AS datanasc ,
                        status ,
                        DT_CONVOCACAO ,
                        ( SELECT    SUM(ctpont.PONTUACAO)
                          FROM      ( (SELECT   pontuacao
                                       FROM     LY_CONCURSO_DOC_EXPERIENCIA code
                                                JOIN LY_CANDIDATO_DOC_EXPERIENCIAS cade ON code.CONCURSO = cade.CONCURSO
                                                                              AND code.EXPERIENCIA = cade.EXPERIENCIA
                                       WHERE    cade.CANDIDATO = cd.candidato
                                                AND cade.CONCURSO = CD.CONCURSO )
                                      UNION ALL
                                      ( SELECT  pontuacao
                                        FROM    LY_CONCURSO_DOC_TITULACOES codt
                                                JOIN LY_CANDIDATO_DOC_TITULACOES cadt ON codt.CONCURSO = cadt.CONCURSO
                                                                              AND codt.TITULACAO = cadt.TITULACAO
                                        WHERE   cadt.CANDIDATO = cd.candidato
                                                AND cadt.CONCURSO = cd.CONCURSO
                                      )
                                    ) ctpont
                        ) AS pontuacao
                FROM    LY_CANDIDATO_DOCENTE cd
                WHERE   CD.CONCURSO = ?
                        AND CD.AGRUPAMENTO_INGRESSO = ?
                        AND STATUS = 2
                        AND CD.DT_APRESENTACAO = ?
                        AND CD.HORA_APRESENTACAO = ?
                        AND CD.MUNICIPIO_PROC = ?
                        AND CD.REGIONALID = ?  ", qtd);

            if (Convert.ToInt32(strCota) != 3)
            {
                sql += @" and cd.COTAIDCONVOCACAO = ? 
                        order by pontuacao desc, DT_NASC asc ";
                return Consultar(sql, concurso, disciplina, data, hora, municipio, regional, strCota);
            }
            sql += "order by DT_CONVOCACAO DESC, pontuacao desc, DT_NASC asc  ";

            return Consultar(sql, concurso, disciplina, data, hora, municipio, regional);
        }

     

    }
}
