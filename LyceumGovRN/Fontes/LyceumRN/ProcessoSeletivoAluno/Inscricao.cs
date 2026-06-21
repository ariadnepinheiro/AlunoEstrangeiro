using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Data;

namespace Techne.Lyceum.RN.ProcessoSeletivoAluno
{
    public class Inscricao : RNBase
    {
        public enum Situacao
        {
            Inscrito = 0,
            Classificado = 1
        }

        public static int BuscaInscricaoID(Int32 candidatoId, Int32 processoSeletivoId, DataContext contexto)
        {
            ContextQuery contextQuery = new ContextQuery();
            int inscricaoID = 0;

            try
            {
                contextQuery.Command = @"SELECT INSCRICAOID
                                           FROM LYCEUM.ProcessoSeletivoAluno.INSCRICAO
                                          WHERE CANDIDATOID = @CANDIDATOID 
                                            AND PROCESSOSELETIVOID = @PROCESSOSELETIVOID";

                contextQuery.Parameters.Add("@CANDIDATOID", candidatoId);
                contextQuery.Parameters.Add("@PROCESSOSELETIVOID", processoSeletivoId);

                inscricaoID = contexto.GetReturnValue<int>(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return inscricaoID;
        }

        public static Int64 BuscaNumeroInscricaoPorInscricaoId(Int32 inscricaoId, DataContext contexto)
        {
            ContextQuery contextQuery = new ContextQuery();
            Int64 numeroInscricao = 0;

            try
            {
                contextQuery.Command = @"SELECT NUMEROINSCRICAO
                                           FROM LYCEUM.ProcessoSeletivoAluno.INSCRICAO
                                          WHERE INSCRICAOID = @INSCRICAOID ";

                contextQuery.Parameters.Add("@INSCRICAOID", inscricaoId);

                numeroInscricao = contexto.GetReturnValue<Int64>(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return numeroInscricao;
        }

        public static int SalvaInscricao(RN.ProcessoSeletivoAluno.Entidades.Inscricao inscricao, int agendaId, int candidatoId, string unidadeEnsinoId, out Int64 numeroInscricao, DataContext contexto)
        {
            int inscricaoId = 0;

            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @"INSERT INTO LYCEUM.ProcessoSeletivoAluno.INSCRICAO
                                               (NUMEROINSCRICAO
                                               ,SITUACAO
                                               ,DATACADASTRO
                                               ,IP
                                               ,CANDIDATOID
                                               ,PROCESSOSELETIVOID
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@NUMEROINSCRICAO
                                               ,@SITUACAO
                                               ,@DATACADASTRO
                                               ,@IP
                                               ,@CANDIDATOID
                                               ,@PROCESSOSELETIVOID
                                               ,@DATAALTERACAO)";

                inscricao.NumeroInscricao = RetornaNovoNumeroInscricao(agendaId, unidadeEnsinoId, contexto);

                contextQuery.Parameters.Add("@NUMEROINSCRICAO", inscricao.NumeroInscricao);
                contextQuery.Parameters.Add("@SITUACAO", (int)inscricao.Situacao);
                contextQuery.Parameters.Add("@DATACADASTRO", inscricao.DataCadastro);
                contextQuery.Parameters.Add("@IP", inscricao.IP);
                contextQuery.Parameters.Add("@CANDIDATOID", candidatoId);
                contextQuery.Parameters.Add("@PROCESSOSELETIVOID", inscricao.ProcessoSeletivoId);
                contextQuery.Parameters.Add("@DATAALTERACAO", inscricao.DataAlteracao);

                contexto.ApplyModifications(contextQuery);

                inscricaoId = BuscaInscricaoID(candidatoId, inscricao.ProcessoSeletivoId, contexto);
                numeroInscricao = inscricao.NumeroInscricao;
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return inscricaoId;
        }

        public static void AlteraInscricao(Techne.Lyceum.RN.ProcessoSeletivoAluno.Entidades.Inscricao inscricao, int inscricaoId, DataContext contexto)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @"UPDATE LYCEUM.ProcessoSeletivoAluno.INSCRICAO
                                            SET IP = @IP
                                               ,DATAALTERACAO = @DATAALTERACAO
                                          WHERE INSCRICAOID = @INSCRICAOID ";

                contextQuery.Parameters.Add("@IP", inscricao.IP);
                contextQuery.Parameters.Add("@DATAALTERACAO", inscricao.DataAlteracao);
                contextQuery.Parameters.Add("@INSCRICAOID", inscricaoId);

                contexto.ApplyModifications(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
        }

        public static RN.ProcessoSeletivoAluno.DTOs.ConfirmacaoProcessoSeletivoAluno ListaConfirmacao_ProcessoSeletivoAlunoPorInscricao(Int64 numeroInscricao)
        {
            ContextQuery contextQuery = new ContextQuery();
            DataContext contexto = null;
            RN.ProcessoSeletivoAluno.DTOs.ConfirmacaoProcessoSeletivoAluno confirmacaoProcessoSeletivoAluno = new RN.ProcessoSeletivoAluno.DTOs.ConfirmacaoProcessoSeletivoAluno(); ;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.Command = @" SELECT I.[INSCRICAOID],
                                                 I.[NUMEROINSCRICAO],
                                                 C.[NOMECOMPLETO],
                                                 C.[NECESSIDADEESPECIAL],
                                                 C.[NOMEMAE],
                                                 C.[DATANASCIMENTO],
                                                 PC.[NUMEROEDITAL],
                                                 CONVERT(VARCHAR(10), I.[DATAALTERACAO], 103) + ' às ' + CONVERT(VARCHAR(5), I.[DATAALTERACAO], 24) AS DATAALTERACAO,
                                                 Stuff((SELECT Cast(', ' AS VARCHAR(52)) + R.NOME
                                                          FROM [LYCEUM].[NecessidadeEspecial].RECURSOAPLICACAOPROVA R
                                                         INNER JOIN [LYCEUM].[ProcessoSeletivoAluno].RECURSOAPLICACAOPROVA_CANDIDATO CR
                                                            ON R.RECURSOAPLICACAOPROVAID = CR.RECURSOAPLICACAOPROVAID
                                                         WHERE CR.CANDIDATOID = C.CANDIDATOID
                                                         ORDER BY R.NOME
                                                           FOR xml path('')), 1, 1, '') AS RECURSOSPROVA,
                                                 UE.[NOME_COMP] as UNIDADEENSINO,
                                                 CU.[NOME] + ' - TURNO ' + T.[DESCRICAO] AS NOMECURSO,
                                                 I.[IP]
                                            FROM [LYCEUM].[ProcessoSeletivoAluno].[CANDIDATO] C
                                           INNER JOIN [LYCEUM].[ProcessoSeletivoAluno].[INSCRICAO] I
                                              ON I.CANDIDATOID = C.CANDIDATOID
                                           INNER JOIN [LYCEUM].[ProcessoSeletivoAluno].[UNIDADEENSINO_CURSO_TURNO_INSCRICAO] UI
                                              ON I.INSCRICAOID = UI.INSCRICAOID
                                           INNER JOIN [LYCEUM].[dbo].[LY_UNIDADE_ENSINO] UE
                                              ON UI.UNIDADEENSINOID = UE.UNIDADE_ENS
                                           INNER JOIN [LYCEUM].[dbo].[LY_CURSO] CU
                                              ON UI.CURSOID = CU.CURSO
                                           INNER JOIN [LYCEUM].[dbo].[LY_TURNO] T
                                              ON T.TURNO = UI.TURNOID
                                           INNER JOIN [LYCEUM].[Agenda].[PROCESSOSELETIVO] PC
										      ON PC.PROCESSOSELETIVOID = I.PROCESSOSELETIVOID
                                           WHERE I.NUMEROINSCRICAO = @NUMEROINSCRICAO ";

                contextQuery.Parameters.Add("@NUMEROINSCRICAO", numeroInscricao);

                DataTable dadosConfirmacao = contexto.GetDataTable(contextQuery);

                if (dadosConfirmacao.Rows.Count > 0)
                {
                    confirmacaoProcessoSeletivoAluno.Curso = dadosConfirmacao.Rows[0]["NOMECURSO"].ToString();
                    confirmacaoProcessoSeletivoAluno.DataAlteracao = dadosConfirmacao.Rows[0]["DATAALTERACAO"].ToString();
                    confirmacaoProcessoSeletivoAluno.DataNascimento = Convert.ToDateTime(dadosConfirmacao.Rows[0]["DATANASCIMENTO"]).ToString("dd/MM/yyyy");
                    confirmacaoProcessoSeletivoAluno.IP = dadosConfirmacao.Rows[0]["IP"].ToString();
                    confirmacaoProcessoSeletivoAluno.NecessidadeEspecial = dadosConfirmacao.Rows[0]["NECESSIDADEESPECIAL"].ToString();
                    confirmacaoProcessoSeletivoAluno.NomeCandidato = dadosConfirmacao.Rows[0]["NOMECOMPLETO"].ToString();
                    confirmacaoProcessoSeletivoAluno.NomeMae = dadosConfirmacao.Rows[0]["NOMEMAE"].ToString();
                    confirmacaoProcessoSeletivoAluno.NumeroEdital = dadosConfirmacao.Rows[0]["NUMEROEDITAL"].ToString();
                    confirmacaoProcessoSeletivoAluno.NumeroInscricao = dadosConfirmacao.Rows[0]["NUMEROINSCRICAO"].ToString();
                    confirmacaoProcessoSeletivoAluno.RecursosNecessarioProva = dadosConfirmacao.Rows[0]["RECURSOSPROVA"].ToString();
                    confirmacaoProcessoSeletivoAluno.UnidadeEnsino = dadosConfirmacao.Rows[0]["UNIDADEENSINO"].ToString();
                }
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }

            return confirmacaoProcessoSeletivoAluno;
        }

        public static Int64 RetornaNovoNumeroInscricao(int agendaId, string unidadeEnsinoId, DataContext contexto)
        {
            ContextQuery contextQuery = new ContextQuery();
            Int64 numeroInscricao = 0;

            try
            {
                contextQuery.Command = @" SELECT TOP 1
                                                PL.[ANO] ,
                                                ISNULL(CPS.[COUNTINSCRICAO], 0) + 1 AS COUNTINSCRICAO ,
                                                ( SELECT    COUNT(*)
                                                  FROM      AGENDA.PROCESSOSELETIVO PSC
                                                            INNER JOIN AGENDA.PERIODOLETIVOAGENDA PLC ON PSC.AGENDAID = PLC.AGENDAID
                                                  WHERE     PLC.ANO = PL.ANO
                                                            AND PLC.PERIODO = PL.PERIODO
                                                ) AS COUNTPROCESSOSELETIVO
                                        FROM    [AGENDA].[PERIODOLETIVOAGENDA] PL
                                                INNER JOIN [AGENDA].[PROCESSOSELETIVO] PS ON PL.AGENDAID = PS.AGENDAID
                                                LEFT JOIN ( SELECT  COUNT(PROCESSOSELETIVOID) AS COUNTINSCRICAO ,
                                                                    PROCESSOSELETIVOID
                                                            FROM    [PROCESSOSELETIVOALUNO].[INSCRICAO] WITH ( UPDLOCK )
                                                            GROUP BY PROCESSOSELETIVOID
                                                          ) CPS ON CPS.PROCESSOSELETIVOID = PS.PROCESSOSELETIVOID
                                        WHERE   PS.[AGENDAID] = @AGENDAID ";

                contextQuery.Parameters.Add("@AGENDAID", agendaId);

                DataTable dt = contexto.GetDataTable(contextQuery);

                if (dt.Rows.Count > 0)
                {
                    numeroInscricao = Convert.ToInt64(dt.Rows[0]["ANO"].ToString() + String.Format("{0:D1}", dt.Rows[0]["COUNTPROCESSOSELETIVO"]) + unidadeEnsinoId + String.Format("{0:D6}", dt.Rows[0]["COUNTINSCRICAO"]));
                }
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return numeroInscricao;
        }

        /// <summary>
        /// Verifica se há candidatos inscritos de acordo com a agenda
        /// </summary>
        /// <param name="agendaId"></param>
        /// <returns></returns>
        public bool VerificaSeExisteCandidatoInscrito(int agendaId)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            bool existeCandidatoInscrito = false;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.Command = @"SELECT 1
                                           FROM [LYCEUM].[ProcessoSeletivoAluno].[INSCRICAO] I
                                          INNER JOIN [LYCEUM].[Agenda].[PROCESSOSELETIVO] PS
                                             ON I.[PROCESSOSELETIVOID] = PS.[PROCESSOSELETIVOID]
                                          WHERE PS.[AGENDAID] = @AGENDAID";

                contextQuery.Parameters.Add("@AGENDAID", agendaId);

                object retorno = contexto.GetReturnValue(contextQuery);

                if (retorno != null)
                    existeCandidatoInscrito = Convert.ToInt32(retorno).Equals(1);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }

            return existeCandidatoInscrito;
        }

        /// <summary>
        /// REF10. Query para Validação do Número de Inscrição do Arquivo de Importação
        /// </summary>
        /// <param name="idAgenda"></param>
        /// <param name="numeroInscricao"></param>
        /// <param name="contexto"></param>
        /// <returns></returns>
        public bool VerificaNumeroInscricaoValido(int idAgenda, Int64 numeroInscricao, DataContext contexto)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool numeroInscricaoValido = false;

            try
            {
                contextQuery.Command = @"SELECT DISTINCT 1
                                           FROM [LYCEUM].[ProcessoSeletivoAluno].[INSCRICAO] I
                                          INNER JOIN [LYCEUM].[Agenda].[PROCESSOSELETIVO] PS
                                             ON I.[PROCESSOSELETIVOID] = PS.PROCESSOSELETIVOID
                                          WHERE I.[NUMEROINSCRICAO] = @NUMEROINSCRICAO
                                            AND PS.AGENDAID = @AGENDAID";

                contextQuery.Parameters.Add("@NUMEROINSCRICAO", numeroInscricao);
                contextQuery.Parameters.Add("@AGENDAID", idAgenda);

                object retorno = contexto.GetReturnValue(contextQuery);

                if (retorno != null && !(retorno == DBNull.Value) && retorno.ToString() == "1")
                    numeroInscricaoValido = true;
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return numeroInscricaoValido;
        }

        /// <summary>
        /// REF11. Query para Validação do Número de Inscrição já importado para outra chamada
        /// </summary>
        /// <param name="idAgenda"></param>
        /// <param name="numeroInscricao"></param>
        /// <param name="contexto"></param>
        /// <returns></returns>
        public bool VerificaNumeroInscricaoImportadoOutraChamada(int idAgenda, Int64 numeroInscricao, DataContext contexto)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool numeroInscricaoImportadoOutraChamada = false;

            try
            {
                contextQuery.Command = @"SELECT DISTINCT 1
                                          FROM [LYCEUM].[ProcessoSeletivoAluno].[INSCRICAO] I
                                         INNER JOIN [LYCEUM].[Agenda].[PROCESSOSELETIVO] PS
                                            ON I.[PROCESSOSELETIVOID] = PS.PROCESSOSELETIVOID
                                         INNER JOIN [LYCEUM].[ProcessoSeletivoAluno].[CANDIDATOINSCRITOCLASSIFICADO] C
                                            ON I.INSCRICAOID = C.INSCRICAOID
                                         INNER JOIN [LYCEUM].[ProcessoSeletivoAluno].[PROCESSOSELETIVO_HISTORICOIMPORTACAO] PH
                                            ON C.PROCESSOSELETIVO_HISTORICOIMPORTACAOID = PH.PROCESSOSELETIVO_HISTORICOIMPORTACAOID
                                         INNER JOIN [LYCEUM].[ProcessoSeletivoAluno].[HISTORICOGERACAOPREMATRICULA] H
                                            ON H.PROCESSOSELETIVO_HISTORICOIMPORTACAOID = PH.PROCESSOSELETIVO_HISTORICOIMPORTACAOID
                                        WHERE I.[NUMEROINSCRICAO] = @NUMEROINSCRICAO
                                        AND PS.AGENDAID = @AGENDAID";

                contextQuery.Parameters.Add("@NUMEROINSCRICAO", numeroInscricao);
                contextQuery.Parameters.Add("@AGENDAID", idAgenda);

                object retorno = contexto.GetReturnValue(contextQuery);

                if (retorno != null && !(retorno == DBNull.Value) && retorno.ToString() == "1")
                    numeroInscricaoImportadoOutraChamada = true;
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return numeroInscricaoImportadoOutraChamada;
        }

        /// <summary>
        /// REF13. Query para Limpar os Candidatos Classificados Importados para a mesma chamada
        /// </summary>
        /// <param name="idAgenda"></param>
        /// <param name="contexto"></param>
        /// <returns></returns>
        public bool ExcluiCandidatosImportados(int idAgenda, DataContext contexto)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool candidatosExcluidos = false;
            
            try
            {
                contextQuery.Command = @" DELETE [LYCEUM].[ProcessoSeletivoAluno].[CANDIDATOINSCRITOCLASSIFICADO]
                                            FROM [LYCEUM].[ProcessoSeletivoAluno].[CANDIDATOINSCRITOCLASSIFICADO] C
                                           INNER JOIN [LYCEUM].[ProcessoSeletivoAluno].[PROCESSOSELETIVO_HISTORICOIMPORTACAO] PH
                                              ON C.PROCESSOSELETIVO_HISTORICOIMPORTACAOID = PH.PROCESSOSELETIVO_HISTORICOIMPORTACAOID
                                           INNER JOIN [LYCEUM].[Agenda].[PROCESSOSELETIVO] PS
                                              ON PH.PROCESSOSELETIVOID = PS.PROCESSOSELETIVOID
                                            LEFT OUTER JOIN
                                               (
                                                   SELECT MAX(NUMEROCHAMADA) + 1 as NUMEROCHAMADA, PHN.PROCESSOSELETIVOID
                                                     FROM [LYCEUM].[PROCESSOSELETIVOALUNO].[PROCESSOSELETIVO_HISTORICOIMPORTACAO] PHN
                                                    INNER JOIN [LYCEUM].[PROCESSOSELETIVOALUNO].[HISTORICOGERACAOPREMATRICULA] HN
                                                       ON PHN.PROCESSOSELETIVO_HISTORICOIMPORTACAOID = HN.PROCESSOSELETIVO_HISTORICOIMPORTACAOID
                                                    GROUP BY PHN.PROCESSOSELETIVOID
                                               ) MH ON MH.PROCESSOSELETIVOID = PS.PROCESSOSELETIVOID
                                           WHERE PS.AGENDAID = @AGENDAID
                                             AND PH.NUMEROCHAMADA = ISNULL(MH.NUMEROCHAMADA , 1)";

                contextQuery.Parameters.Add("@AGENDAID", idAgenda);
                contexto.ApplyModifications(contextQuery);

                candidatosExcluidos = true;
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return candidatosExcluidos;
        }

        /// <summary>
        /// REF12. Query para Atualizar a situação do Candidato para Classificado
        /// </summary>
        /// <param name="numeroInscricao"></param>
        /// <param name="ordem"></param>
        /// <param name="contexto"></param>
        /// <returns></returns>
        public bool AtualizaSituacaoCandidatoParaClassificado(RN.Entidades.Importacao.ImportaResultadoProcessoSeletivoAluno importaResultadoProcessoSeletivo, int idProcessoSeletivo_HistoricoImportacao, DataContext contexto)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool candidatoAtualizado = false;

            try
            {
                contextQuery.Command = @" 
                                    INSERT INTO [LYCEUM].[ProcessoSeletivoAluno].[CANDIDATOINSCRITOCLASSIFICADO]
                                    (
                                        INSCRICAOID,
                                        PROCESSOSELETIVO_HISTORICOIMPORTACAOID,
                                        ORDEM
                                    )
                                    VALUES
                                    (
                                        (SELECT INSCRICAOID
                                           FROM [LYCEUM].[ProcessoSeletivoAluno].[INSCRICAO] I
                                          WHERE I.NUMEROINSCRICAO = @NUMEROINSCRICAO), 
                                        @PROCESSOSELETIVO_HISTORICOIMPORTACAOID,
                                        @ORDEM
                                    )";

                contextQuery.Parameters.Add("@NUMEROINSCRICAO", importaResultadoProcessoSeletivo.NumeroInscricao);
                contextQuery.Parameters.Add("@PROCESSOSELETIVO_HISTORICOIMPORTACAOID", idProcessoSeletivo_HistoricoImportacao);
                contextQuery.Parameters.Add("@ORDEM", importaResultadoProcessoSeletivo.Ordem);
                contexto.ApplyModifications(contextQuery);

                candidatoAtualizado = true;
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return candidatoAtualizado;
        }
    }
}