using System;
using System.Data;
using System.Text;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using System.Collections.Generic;
using System.Data.SqlClient;
using Techne.Lyceum.RN.DTOs;
using System.Linq;

namespace Techne.Lyceum.RN.RenovacaoMatricula
{
    public class Renovacao : RNBase
    {
        public const string TipoBuscaTurnosEVagas = "TurnosEVagas";
        public const string TipoBuscaControleDeVagas = "ControleDeVagas";
        public const string TipoBuscaPeriodoInvalido = "PeriodoInvalido";

        public DataTable ListaRenovacaoPor(string aluno, int ano, int periodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable renovacao = null;

            try
            {
                contextQuery.Command = string.Format(@"
                SELECT R.renovacaoid                                      AS RENOVACAOID, 
                       R.alunoid                                          AS ALUNO, 
                       R.ano                                              AS ANO, 
                       R.periodo                                          AS PERIODO, 
                       R.unidadeensinoid                                  AS UNIDADE, 
                       U.nome_comp                                        AS UNIDADE_ENSINO, 
                       M.descricao + ' / ' + S.descricao + ' / ' + C.nome AS MOD_SEG_CURSO, 
                       M.descricao                                        AS MODALIDADE, 
                       S.descricao                                        AS SERIE_DESC, 
                       C.nome                                             AS CURSO, 
                       C.curso                                            AS COD_CURSO, 
                       R.serie                                            AS SERIE, 
                       R.turnoid                                          AS TURNO, 
                       R.ensinoreligioso                                  AS ENSINO_RELIGIOSO, 
                       R.linguaestrangeira                                AS LINGUA_ESTRANGEIRA, 
                       CASE 
                        WHEN R.SITUACAORENOVACAOID = 1 THEN 'Ativo' 
                        WHEN R.SITUACAORENOVACAOID = 2 THEN 'Cancelado' 
                        ELSE 'Possui confirmação' 
                       END AS SITUACAO_RENOVACAOID,
                       R.situacaorenovacaoid                              AS COD_SITUACAO_RENOVACAOID, 
                       R.usuario                                          AS USUARIO, 
                       R.datacadastro                                     AS DATA_CADASTRO, 
                       R.dataalteracao                                    AS DATA_ALTERACAO, 
                       CASE 
                        WHEN R.tipovaga = 'VC' THEN 'Vaga de Continuidade' 
                        WHEN R.tipovaga = 'VN' THEN 'Vaga Nova' 
                       END AS TIPO_VAGA 
                FROM   renovacao R 
                       INNER JOIN ly_unidade_ensino U 
                               ON R.unidadeensinoid = U.unidade_ens 
                       INNER JOIN ly_curso C 
                               ON R.cursoid = C.curso 
                       INNER JOIN ly_modalidade_curso M 
                               ON M.modalidade = C.modalidade 
                       INNER JOIN LY_TIPO_CURSO S 
                               ON S.TIPO = C.TIPO  
                WHERE  R.alunoid = '{0}' AND R.ANO = {1} AND R.PERIODO = {2}
                ORDER BY renovacaoid desc; ", aluno, ano, periodo);

                renovacao = ctx.GetDataTable(contextQuery);
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

            return renovacao;
        }

        public ValidacaoDados ValidaCancelaRenovacao(string aluno, int renovacaoId, int ano, int periodo, string usuarioId)
        {
            string tipoVaga = string.Empty;
            List<string> mensagens = new List<string>();
            RN.Matricula rnMatricula = new Matricula();
            RN.PeriodoLetivo rnPeriodoLetivo = new PeriodoLetivo();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            //Verifica campos obrigatórios
            if (renovacaoId <= 0)
            {
                mensagens.Add("O campo Codigo é obrigatório.");
            }

            if (string.IsNullOrEmpty(aluno))
            {
                mensagens.Add("O campo Aluno é obrigatório.");
            }

            if (ano <= 0)
            {
                mensagens.Add("O campo Ano é obrigatório.");
            }

            if (periodo < 0)
            {
                mensagens.Add("O campo Período é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se possui renovaçao ativa para cancelar
                    if (!this.EhRenovacaoAtivaConfirmadaPor(contexto, renovacaoId))
                    {
                        mensagens.Add("Este aluno não possui renovação ativa no ano / periodo.");
                    }

                    //Verifica se o periodo ja iniciou
                    if (!rnPeriodoLetivo.EhAnoPeriodoFuturoPor(contexto, ano, periodo, DateTime.Now))
                    {
                        mensagens.Add("A renovação não pode ser cancelada porque o periodo letivo já iniciou.");
                    }

                    //Verifica se possui matricula ativa no ano / periodo da renovação
                    if (rnMatricula.PossuiMatriculaAtivaPeriodoPor(contexto, aluno, ano, periodo))
                    {
                        mensagens.Add("A renovação não pode ser cancelada porque o aluno já foi enturmado no ano / periodo.");
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
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void CancelaRenovacao(int renovacaoId, string usuarioId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            try
            {
                //Cancela Confirmação gerada pela renovação
                this.CancelaConfirmacaoRenovacao(ctx, renovacaoId, usuarioId);

                //Cancela renovação
                this.CancelaRenovacao(ctx, renovacaoId, usuarioId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ctx.Dispose();
            }
        }

        private void CancelaRenovacao(DataContext contexto, int renovacaoId, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE RENOVACAO
                                    SET     SITUACAORENOVACAOID = 2,
											USUARIO = @USUARIO,
											DATAALTERACAO = @DATAALTERACAO
                                    WHERE   RENOVACAOID = @RENOVACAOID
											AND SITUACAORENOVACAOID IN (1, 3) ";

            contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@RENOVACAOID", SqlDbType.Int, renovacaoId);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaSuspensao(DataContext contexto, int ano, int historicoSuspensaoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE RENOVACAO
                                    SET     SITUACAORENOVACAOID = 2,
		                                    DATAALTERACAO = GETDATE()
                                    FROM RENOVACAO R 
	                                    INNER JOIN Turma.HISTORICOSUSPENSAO h ON h.ALUNO = r.ALUNOID
                                     WHERE R.ANO = @ANO
		                                    AND   SITUACAORENOVACAOID = 1 
		                                    AND HISTORICOSUSPENSAOID = @HISTORICOSUSPENSAOID ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@HISTORICOSUSPENSAOID", SqlDbType.Int, historicoSuspensaoId);

            contexto.ApplyModifications(contextQuery);
        }

        private void CancelaConfirmacaoRenovacao(DataContext contexto, int renovacaoId, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE CM
	                                    SET [STATUS] = 'Não Confirmado',
	                                    MATRICULA = @USUARIO,
	                                    DT_ALTERACAO = @DATAALTERACAO,
	                                    OBSERVACAO = OBSERVACAO + ' - RENOVAÇÃO CANCELADA'
                                    FROM RENOVACAO_CONFIRMACAOMATRICULA RC
	                                    INNER JOIN TCE_CONFIRMACAO_MATRICULA CM ON RC.CONFIRMACAOMATRICULAID = CM.ID_CONFIRMACAO_MATRICULA
                                    WHERE RC.RENOVACAOID = @RENOVACAOID
	                                     ";

            contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@RENOVACAOID", SqlDbType.Int, renovacaoId);

            contexto.ApplyModifications(contextQuery);
        }

        /// <summary>
        /// Retorna todas as Renovações do Aluno
        /// </summary>
        /// <param name="aluno">Matrícula do Aluno</param>
        /// <returns>Renovações</returns>
        public static DataTable ListaRenovacaoPor(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable renovacao = null;

            try
            {
                contextQuery.Command = string.Format(@"
                SELECT R.renovacaoid                                      AS RENOVACAOID, 
                       R.alunoid                                          AS ALUNO, 
                       R.ano                                              AS ANO, 
                       R.periodo                                          AS PERIODO, 
                       R.unidadeensinoid                                  AS UNIDADE, 
                       U.nome_comp                                        AS UNIDADE_ENSINO, 
                       M.descricao + ' / ' + S.descricao + ' / ' + C.nome AS MOD_SEG_CURSO, 
                       M.descricao                                        AS MODALIDADE, 
                       S.descricao                                        AS SERIE_DESC, 
                       C.nome                                             AS CURSO, 
                       C.curso                                            AS COD_CURSO, 
                       R.serie                                            AS SERIE, 
                       R.turnoid                                          AS TURNO, 
                       R.ensinoreligioso                                  AS ENSINO_RELIGIOSO, 
                       R.linguaestrangeira                                AS LINGUA_ESTRANGEIRA, 
                       CASE 
                        WHEN R.SITUACAORENOVACAOID = 1 THEN 'Ativo' 
                        WHEN R.SITUACAORENOVACAOID = 2 THEN 'Cancelado' 
                        ELSE 'Possui confirmação' 
                       END AS SITUACAO_RENOVACAOID,
                       --R.situacaorenovacaoid                              AS SITUACAO_RENOVACAOID, 
                       R.usuario                                          AS USUARIO, 
                       R.datacadastro                                     AS DATA_CADASTRO, 
                       R.dataalteracao                                    AS DATA_ALTERACAO, 
                       CASE 
                        WHEN R.tipovaga = 'VC' THEN 'Vaga de Continuidade' 
                        WHEN R.tipovaga = 'VN' THEN 'Vaga Nova' 
                       END AS TIPO_VAGA 
                FROM   renovacao R 
                       INNER JOIN ly_unidade_ensino U 
                               ON R.unidadeensinoid = U.unidade_ens 
                       INNER JOIN ly_curso C 
                               ON R.cursoid = C.curso 
                       INNER JOIN ly_modalidade_curso M 
                               ON M.modalidade = C.modalidade 
                       INNER JOIN LY_TIPO_CURSO S 
                               ON S.TIPO = C.TIPO  
                WHERE  R.alunoid = '{0}' 
                ORDER BY renovacaoid desc; ", aluno);

                renovacao = ctx.GetDataTable(contextQuery);
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

            return renovacao;
        }

        public void EncerraRenocacoesMatricula(DataContext contexto, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE RENOVACAO
                                    SET     SITUACAORENOVACAOID = 2
                                    WHERE   SITUACAORENOVACAOID = 1
                                            AND ALUNOID = @ALUNOID ";

            contextQuery.Parameters.Add("@ALUNOID", SqlDbType.VarChar, aluno);

            contexto.ApplyModifications(contextQuery);
        }

        public static DataTable ListaUnidadeEnsinoRenovacaoMatricula(string unidadeEnsinoAtual, DataTable unidades_agenda)
        {
            string unidades = string.Empty;

            if (unidades_agenda.Rows.Count > 0)
            {
                DataTable dtDistinct = unidades_agenda.DefaultView.ToTable(true, "UNIDADE_ENS");

                foreach (DataRow row in dtDistinct.Rows)
                {
                    unidades += "'" + row["UNIDADE_ENS"].ToString() + "',";
                }

                unidades = unidades.Substring(0, unidades.Length - 1);
            }

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    string.Format(@"
                SELECT U.unidade_ens, 
                       U.nome_comp 
                FROM   ly_unidade_ensino U 
                WHERE  ( U.unidade_ens = '{0}' --Escola atual 
                         AND ( 
                             --Unidade totalmente absorvida não aparece 
                             NOT EXISTS (SELECT TOP 1 S.unidadeensinoorigemid 
                                         FROM   serieabsorvida S 
                                         WHERE  S.nivelabsorcaoid = 1 --unidade educacional 
                                                AND S.unidadeensinoorigemid = U.unidade_ens) 
                              --Unidade NÃO absorvida aparece 
                              OR NOT EXISTS (SELECT TOP 1 S.unidadeensinoorigemid 
                                             FROM   serieabsorvida S 
                                             WHERE  S.unidadeensinoorigemid = U.unidade_ens) ) 
                        --fechando AND 
                        ) 
                        --Unidades absorvedoras aparecem 
                        OR EXISTS (SELECT TOP 1 S.unidadeensinodestinoid 
                                   FROM   serieabsorvida S 
                                   WHERE  S.unidadeensinodestinoid = U.unidade_ens 
                                          AND S.unidadeensinoorigemid = '{0}' 
                                  --Escola atual 
                                  ) 
                AND UNIDADE_ENS IN ({1})"
                    , unidadeEnsinoAtual
                    , unidades));

                return ctx.GetDataTable(contextQuery);
            }
        }

        public bool PossuiCursoPor(DataContext ctx, string curso)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                FROM  DBO.RENOVACAO
                                WHERE CURSOID = @CURSO ";

            contextQuery.Parameters.Add("@CURSO", curso);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable ListaRenovacaoMatriculaPor(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable renovacoes = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                            R.RENOVACAOID AS RENOVACAOID ,
                            R.ALUNOID AS ALUNO ,
                            PE.NOME_COMPL AS NOMEALUNO ,
                            R.ANO AS ANO ,
                            R.PERIODO AS PERIODO ,
                            R.UNIDADEENSINOID AS CENSO ,
                            E.NOME_COMP AS NOME_COMP ,
                            MD.DESCRICAO + ' / ' + TC.DESCRICAO + ' / ' + C.NOME AS MOD_SEG_CURSO ,
                            T.DESCRICAO AS TURNO ,
                            R.SERIE AS SERIE ,
                            CASE WHEN R.ENSINORELIGIOSO = 0 THEN 'N'
                                 WHEN R.ENSINORELIGIOSO = 1 THEN 'S'
                            END AS ENS_RELIGIOSO ,
                            CASE WHEN R.LINGUAESTRANGEIRA = 0 THEN 'N'
                                 WHEN R.LINGUAESTRANGEIRA = 1 THEN 'S'
                            END AS LINGUA_ESTRANGEIRA ,
                            CASE WHEN R.SITUACAORENOVACAOID = 1 THEN 'ATIVO'
                                 WHEN R.SITUACAORENOVACAOID = 2 THEN 'CANCELADO'
                                 WHEN R.SITUACAORENOVACAOID = 3 THEN 'POSSUI CONFIRMACAO'
                            END AS SITUACAO ,
                            R.USUARIO AS USUARIO ,
                            R.DATACADASTRO AS DATA_HORA ,
                            R.DATAALTERACAO
                    FROM    DBO.RENOVACAO R ( NOLOCK )
                            INNER JOIN DBO.LY_ALUNO A ON R.ALUNOID = A.ALUNO
                            INNER JOIN DBO.LY_UNIDADE_ENSINO E ON R.UNIDADEENSINOID = E.UNIDADE_ENS
                            INNER JOIN DBO.LY_CURSO C ON R.CURSOID = C.CURSO
                            INNER JOIN DBO.LY_MODALIDADE_CURSO MD ON C.MODALIDADE = MD.MODALIDADE
                            INNER JOIN DBO.LY_TIPO_CURSO TC ON C.TIPO = TC.TIPO
                            INNER JOIN DBO.LY_TURNO T ON R.TURNOID = T.TURNO
                            INNER JOIN LY_PESSOA PE (NOLOCK)  ON PE.PESSOA = A.PESSOA
                    WHERE   ALUNO = @ALUNO
                            AND R.SITUACAORENOVACAOID = 1
                    ORDER BY R.RENOVACAOID DESC ,
                            R.DATACADASTRO ";

                contextQuery.Parameters.Add("@ALUNO", aluno);

                renovacoes = ctx.GetDataTable(contextQuery);
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

            return renovacoes;
        }

        public DataTable ListaRenovacoesMatriculasPor(string unidadeEnsino, string curso, int ano, int periodo, string serie, string turno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable renovacoes = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                            R.RENOVACAOID AS RENOVACAOID ,
                            R.ALUNOID AS ALUNO ,
                            PE.NOME_COMPL AS NOMEALUNO ,
                            R.ANO AS ANO ,
                            R.PERIODO AS PERIODO ,
                            R.UNIDADEENSINOID AS CENSO ,
                            E.NOME_COMP AS NOME_COMP ,
                            MD.DESCRICAO + ' / ' + TC.DESCRICAO + ' / ' + C.NOME AS MOD_SEG_CURSO ,
                            T.DESCRICAO AS TURNO ,
                            R.SERIE AS SERIE ,
                            CASE WHEN R.ENSINORELIGIOSO = 0 THEN 'N'
                                 WHEN R.ENSINORELIGIOSO = 1 THEN 'S'
                            END AS ENS_RELIGIOSO ,
                            CASE WHEN R.LINGUAESTRANGEIRA = 0 THEN 'N'
                                 WHEN R.LINGUAESTRANGEIRA = 1 THEN 'S'
                            END AS LINGUA_ESTRANGEIRA ,
                            CASE WHEN R.SITUACAORENOVACAOID = 1 THEN 'ATIVO'
                                 WHEN R.SITUACAORENOVACAOID = 2 THEN 'CANCELADO'
                                 WHEN R.SITUACAORENOVACAOID = 3 THEN 'POSSUI CONFIRMACAO'
                            END AS SITUACAO ,
                            R.USUARIO AS USUARIO ,
                            R.DATACADASTRO AS DATA_HORA ,
                            R.DATAALTERACAO
                    FROM    DBO.RENOVACAO R ( NOLOCK )
                            INNER JOIN DBO.LY_ALUNO A ON R.ALUNOID = A.ALUNO
                            INNER JOIN DBO.LY_UNIDADE_ENSINO E ON R.UNIDADEENSINOID = E.UNIDADE_ENS
                            INNER JOIN DBO.LY_CURSO C ON R.CURSOID = C.CURSO
                            INNER JOIN DBO.LY_MODALIDADE_CURSO MD ON C.MODALIDADE = MD.MODALIDADE
                            INNER JOIN DBO.LY_TIPO_CURSO TC ON C.TIPO = TC.TIPO
                            INNER JOIN DBO.LY_TURNO T ON R.TURNOID = T.TURNO
                            INNER JOIN LY_PESSOA PE (NOLOCK)  ON PE.PESSOA = A.PESSOA
                    WHERE   R.UNIDADEENSINOID = @UNIDADEENSINOID
                            AND R.CURSOID = @CURSOID
                            AND R.ANO = @ANO
                            AND R.PERIODO = @PERIODO
                            AND R.SITUACAORENOVACAOID = @SITUACAORENOVACAOID
                            AND R.SERIE = @SERIE
                            AND R.TURNOID = @TURNOID
                    ORDER BY R.RENOVACAOID DESC ,
                            R.DATACADASTRO ";

                contextQuery.Parameters.Add("@UNIDADEENSINOID", unidadeEnsino);
                contextQuery.Parameters.Add("@CURSOID", curso);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@SERIE", serie);
                contextQuery.Parameters.Add("@TURNOID", turno);
                contextQuery.Parameters.Add("@SITUACAORENOVACAOID", Convert.ToInt32(RN.RenovacaoMatricula.Entidades.SituacaoRenovacao.Ativo));

                renovacoes = ctx.GetDataTable(contextQuery);
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

            return renovacoes;
        }

        public DataTable ListaAnosComRenovacoes()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable anos = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                ANO
                        FROM    DBO.RENOVACAO
                        ORDER BY ANO DESC ";

                anos = ctx.GetDataTable(contextQuery);
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

            return anos;
        }

        public DataTable ListaPeriodosComRenovacoesPor(int ano)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable periodos = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                    PERIODO
                            FROM    DBO.RENOVACAO
                            WHERE   ANO = @ANO
                            ORDER BY PERIODO DESC ";

                contextQuery.Parameters.Add("@ANO", ano);

                periodos = ctx.GetDataTable(contextQuery);
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

            return periodos;
        }

        public DataTable ListaTurnosComRenovacoesPor(int ano, int periodo, string unidadeEnsino, string curso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable turnos = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT TURNO ,
                                    DESCRICAO
                            FROM    DBO.RENOVACAO R
                                    INNER JOIN LY_TURNO T ON R.TURNOID = T.TURNO
                            WHERE   ANO = @ANO
                                    AND PERIODO = @PERIODO
                                    AND UNIDADEENSINOID = @UNIDADEENSINOID
                                    AND CURSOID = @CURSOID
                            ORDER BY DESCRICAO  ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@UNIDADEENSINOID", unidadeEnsino);
                contextQuery.Parameters.Add("@CURSOID", curso);

                turnos = ctx.GetDataTable(contextQuery);
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

            return turnos;
        }

        public DataTable ListaSeriesComRenovacoesPor(int ano, int periodo, string unidadeEnsino, string curso, string turno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable turnos = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                                    R.SERIE AS SERIE
                                            FROM    DBO.RENOVACAO R
                                                    INNER JOIN LY_SERIE S ON R.SERIE = S.SERIE
                                            WHERE   ANO = @ANO
                                                    AND PERIODO = @PERIODO
                                                    AND UNIDADEENSINOID = @UNIDADEENSINOID
                                                    AND CURSOID = @CURSOID
                                                    AND TURNOID = @TURNOID
                                                    AND ( DT_EXTINCAO IS NULL
                                                          OR CONVERT(DATE, DT_EXTINCAO) > CONVERT(DATE, GETDATE())
                                                        )
                                            ORDER BY SERIE ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@UNIDADEENSINOID", unidadeEnsino);
                contextQuery.Parameters.Add("@CURSOID", curso);
                contextQuery.Parameters.Add("@TURNOID", turno);

                turnos = ctx.GetDataTable(contextQuery);
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

            return turnos;
        }

        public ValidacaoDados Valida(RN.RenovacaoMatricula.Entidades.Renovacao renovacao)
        {
            string tipoVaga = string.Empty;
            List<string> mensagens = new List<string>();
            RN.Aluno rnAluno = new Aluno();
            TceControleVaga controleVaga = new TceControleVaga();
            RN.ControleVaga rnControleVaga = new ControleVaga();
            int quantidadeRenovacaoAtiva = 0;
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            //Verifica campos obrigatórios
            if (string.IsNullOrEmpty(renovacao.AlunoId))
            {
                mensagens.Add("O campo Aluno é obrigatório.");
            }

            if (renovacao.Ano <= 0 || renovacao.Periodo < 0)
            {
                mensagens.Add("O campo Ano e Período é obrigatório.");
            }

            if (string.IsNullOrEmpty(renovacao.UnidadeEnsinoId))
            {
                mensagens.Add("O campo Unidade de Ensino é obrigatório.");
            }

            if (string.IsNullOrEmpty(renovacao.CursoId))
            {
                mensagens.Add("O campo Modalidade/Segmento/Curso é obrigatório.");
            }

            if (renovacao.Serie <= 0)
            {
                mensagens.Add("O campo Série/Ano Escolar é obrigatório.");
            }

            if (string.IsNullOrEmpty(renovacao.TurnoId))
            {
                mensagens.Add("O campo Turno é obrigatório.");
            }

            if (string.IsNullOrEmpty(renovacao.Usuario))
            {
                mensagens.Add("O USUÁRIO RESPONSÁVEL é obrigatório.");
            }

            if (string.IsNullOrEmpty(renovacao.TipoVaga))
            {
                mensagens.Add("O TIPO DE VAGA é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se a aluno está ativo
                    if (rnAluno.ObtemSituacaoAlunoPor(contexto, renovacao.AlunoId).ToUpper() != "ATIVO")
                    {
                        mensagens.Add("O Aluno não está Ativo.");
                    }

                    //Verifica se ja existe renovaçao ativa
                    if (PossuiRenovacaoAtivaPor(contexto, renovacao.AlunoId))
                    {
                        mensagens.Add("Aluno com renovação ativa. Necessário cancelar antes.");
                    }

                    //Verifica se já existe controle de vagas criado
                    controleVaga = rnControleVaga.ObtemPor(contexto, renovacao.Ano, renovacao.Periodo, renovacao.UnidadeEnsinoId, renovacao.CursoId, renovacao.TurnoId, renovacao.Serie);

                    if (controleVaga.IdControleVaga > 0)
                    {
                        //Quando já existir controle de vagas, o limite de renovação será o total de vagas por turno, série e curso.

                        //Busca Quantidade de renovaçoes ativas
                        quantidadeRenovacaoAtiva = this.ObtemQuantidadeRenovacaoAtivaPor(contexto, renovacao.Ano, renovacao.Periodo, renovacao.UnidadeEnsinoId, renovacao.CursoId, renovacao.TurnoId, renovacao.Serie);

                        //Verifica se quantidade de renovaçoes ativas atingiu a quantidade de vagas + 30%
                        if (quantidadeRenovacaoAtiva >= (controleVaga.VagasContinuidade) + (controleVaga.VagasContinuidade * 0.3)) //Considerar apenas Vagas de continuidade
                        {
                            mensagens.Add("Não existem mais vagas para o Curso / Serie / Turno escolhidos.");
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
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        private int ObtemQuantidadeRenovacaoAtivaPor(DataContext contexto, int ano, int periodo, string censo, string curso, string turno, int serie)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) AS QUANTIDADE
	                            FROM RENOVACAO (NOLOCK)
	                            WHERE ANO = @ANO
			                            AND PERIODO = @PERIODO
			                            AND UNIDADEENSINOID = @UNIDADEENSINOID
			                            AND CURSOID = @CURSOID
			                            AND TURNOID = @TURNOID
			                            AND SERIE = @SERIE
			                            AND SITUACAORENOVACAOID = 1 --Ativa ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
                contextQuery.Parameters.Add("@UNIDADEENSINOID", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@CURSOID", SqlDbType.VarChar, curso);
                contextQuery.Parameters.Add("@TURNOID", SqlDbType.VarChar, turno);
                contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["QUANTIDADE"]);
                }

                return retorno;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public void InsereRenovacaoMatricula(Entidades.Renovacao renovacao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            try
            {
                this.InsereRenovacaoMatricula(ctx, renovacao);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public void InsereRenovacaoMatricula(DataContext ctx, Entidades.Renovacao renovacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"  INSERT INTO [LYCEUM].[DBO].[RENOVACAO]
                            ( ALUNOID ,
                              UNIDADEENSINOID ,
                              CURSOID ,
                              TURNOID ,
                              ANO ,
                              PERIODO ,
                              SERIE ,
                              SITUACAORENOVACAOID ,
                              USUARIO ,
                              DATAALTERACAO ,
                              TIPOVAGA ,
                              ENSINORELIGIOSO ,
                              LINGUAESTRANGEIRA ,
                              DATACADASTRO
                            )
                     VALUES ( @ALUNOID ,
                              @UNIDADEENSINOID ,
                              @CURSOID ,
                              @TURNOID ,
                              @ANO ,
                              @PERIODO ,
                              @SERIE ,
                              @SITUACAORENOVACAOID ,
                              @USUARIO ,
                              GETDATE() ,
                              @TIPOVAGA ,
                              @ENSINORELIGIOSO ,
                              @LINGUAESTRANGEIRA ,
                              GETDATE()
                            ) ";

                contextQuery.Parameters.Add("@ALUNOID", renovacao.AlunoId);
                contextQuery.Parameters.Add("@UNIDADEENSINOID", renovacao.UnidadeEnsinoId);
                contextQuery.Parameters.Add("@CURSOID", renovacao.CursoId);
                contextQuery.Parameters.Add("@TURNOID", renovacao.TurnoId);
                contextQuery.Parameters.Add("@ANO", renovacao.Ano);
                contextQuery.Parameters.Add("@PERIODO", renovacao.Periodo);
                contextQuery.Parameters.Add("@SERIE", renovacao.Serie);
                contextQuery.Parameters.Add("@SITUACAORENOVACAOID", renovacao.SituacaoRenovacaoId);
                contextQuery.Parameters.Add("@USUARIO", renovacao.Usuario);
                contextQuery.Parameters.Add("@TIPOVAGA", renovacao.TipoVaga);
                contextQuery.Parameters.Add("@ENSINORELIGIOSO", renovacao.EnsinoReligioso);
                contextQuery.Parameters.Add("@LINGUAESTRANGEIRA", renovacao.LinguaEstrangeira);

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

        public void AtualizaRenovacao(DataContext context, RenovacaoMatricula.Entidades.Renovacao renovacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE  dbo.RENOVACAO
                                    SET     ALUNOID = @ALUNOID ,
                                            UNIDADEENSINOID = @UNIDADEENSINOID ,
                                            CURSOID = @CURSOID ,
                                            TURNOID = @TURNOID ,
                                            ANO = @ANO ,
                                            PERIODO = @PERIODO ,
                                            SERIE = @SERIE ,
                                            SITUACAORENOVACAOID = @SITUACAORENOVACAOID ,
                                            USUARIO = @USUARIO ,
                                            DATAALTERACAO = @DATAALTERACAO ,
                                            TIPOVAGA = @TIPOVAGA ,
                                            ENSINORELIGIOSO = @ENSINORELIGIOSO ,
                                            LINGUAESTRANGEIRA = @LINGUAESTRANGEIRA
                                    WHERE   RENOVACAOID = @RENOVACAOID ";

                contextQuery.Parameters.Add("@RENOVACAOID", renovacao.RenovacaoId);
                contextQuery.Parameters.Add("@ALUNOID", renovacao.AlunoId);
                contextQuery.Parameters.Add("@UNIDADEENSINOID", renovacao.UnidadeEnsinoId);
                contextQuery.Parameters.Add("@CURSOID", renovacao.CursoId);
                contextQuery.Parameters.Add("@TURNOID", renovacao.TurnoId);
                contextQuery.Parameters.Add("@ANO", renovacao.Ano);
                contextQuery.Parameters.Add("@PERIODO", renovacao.Periodo);
                contextQuery.Parameters.Add("@SERIE", renovacao.Serie);
                contextQuery.Parameters.Add("@SITUACAORENOVACAOID", renovacao.SituacaoRenovacaoId);
                contextQuery.Parameters.Add("@USUARIO", renovacao.Usuario);
                contextQuery.Parameters.Add("@DATAALTERACAO", renovacao.DataAlteracao);
                contextQuery.Parameters.Add("@TIPOVAGA", renovacao.TipoVaga);
                contextQuery.Parameters.Add("@ENSINORELIGIOSO", renovacao.EnsinoReligioso);
                contextQuery.Parameters.Add("@LINGUAESTRANGEIRA", renovacao.LinguaEstrangeira);

                context.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CancelaRenovacoes(IList<int> idRenovacoes, string usuario)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                foreach (int id in idRenovacoes)
                {
                    this.CancelaRenovacaoMatricula(ctx, id, usuario);
                }
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                throw ex;
            }
            finally
            {
                ctx.Dispose();
            }
        }

        private void CancelaRenovacaoMatricula(DataContext ctx, int idRenovacao, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE  [LYCEUM].[DBO].[RENOVACAO]
                    SET     [SITUACAORENOVACAOID] = 2 ,
                            [USUARIO] = @USUARIO ,
                            [DATAALTERACAO] = GETDATE()
                    WHERE   RENOVACAOID = @RENOVACAOID ";

                contextQuery.Parameters.Add("@USUARIO", usuario);
                contextQuery.Parameters.Add("@RENOVACAOID", idRenovacao);

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

        public DadosEnturmacaoAluno ObtemMatriculaPrincipalAtivaEmAnoPeriodoReferenciaPor(string aluno, int agendaId)
        {
            DadosEnturmacaoAluno matricula = new DadosEnturmacaoAluno();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"  SELECT DISTINCT
                            M.ALUNO AS ALUNO ,
                            M.ANO AS ANO ,
                            M.SEMESTRE AS SEMESTRE ,
                            M.TURMA AS TURMA
                     FROM   DBO.LY_MATRICULA M
                            INNER JOIN LY_TURMA T ON M.DISCIPLINA = T.DISCIPLINA
                                                     AND M.TURMA = T.TURMA
                                                     AND M.ANO = T.ANO
                                                     AND M.SEMESTRE = T.SEMESTRE
                     WHERE  ( M.DEPENDENCIA IS NULL
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
                            AND T.OPTATIVAREFORCO = 'N'
                            AND ISNULL(T.ELETIVA,'N') = 'N'
                            AND M.ALUNO = @ALUNO
                            AND M.SIT_MATRICULA = @SIT_MATRICULA
                            AND EXISTS ( SELECT TOP 1
                                                1
                                         FROM   AGENDA.PERIODOREFERENCIA R
                                                INNER JOIN AGENDA.PERIODOLETIVOAGENDA P ON R.PERIODOLETIVOAGENDAID = P.PERIODOLETIVOAGENDAID
                                         WHERE  P.AGENDAID = @AGENDAID
                                                AND R.ANO = M.ANO
                                                AND R.PERIODO = M.SEMESTRE ) ";

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@SIT_MATRICULA", Matricula.Matriculado);
                contextQuery.Parameters.Add("@AGENDAID", agendaId);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    matricula.Aluno = Convert.ToString(reader["ALUNO"]);
                    matricula.Ano = Convert.ToInt32(reader["ANO"]);
                    matricula.Periodo = Convert.ToInt32(reader["SEMESTRE"]);
                    matricula.Turma = Convert.ToString(reader["TURMA"]);
                }

                return matricula;
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

        public DataTable ListaProximoAnoPeriodoParaRenovacaoPor(string aluno, int agendaId)
        {
            DadosEnturmacaoAluno matriculaAtiva = new DadosEnturmacaoAluno();
            string proximoAno = string.Empty;
            RN.PeriodoLetivo rnPeriodoLetivo = new PeriodoLetivo();
            DataTable dtAnoPeriodo = new DataTable();
            dtAnoPeriodo.Columns.Add("ano");

            matriculaAtiva = this.ObtemMatriculaPrincipalAtivaEmAnoPeriodoReferenciaPor(aluno, agendaId);

            if (!string.IsNullOrEmpty(matriculaAtiva.Aluno))
            {
                //Buscar valor pro proximo ano para o ano / periodo do aluno
                proximoAno = Convert.ToString(rnPeriodoLetivo.ObtemProximoAnoPor(Convert.ToInt32(matriculaAtiva.Ano), Convert.ToInt32(matriculaAtiva.Periodo)));

                if (string.IsNullOrEmpty(proximoAno))
                {
                    return dtAnoPeriodo;
                }

                if (matriculaAtiva.Periodo == 0)
                {
                    //Se o período atual do aluno for ‘0’, pode ir para 0 ou 1
                    dtAnoPeriodo.Rows.Add(string.Format("{0} - 0", proximoAno));
                    dtAnoPeriodo.Rows.Add(string.Format("{0} - 1", proximoAno));
                }
                else if (matriculaAtiva.Periodo == 1)
                {
                    //Se o período atual do aluno for ‘1’ pode ir para 2
                    dtAnoPeriodo.Rows.Add(string.Format("{0} - 2", proximoAno));
                }
                //Se o período atual do aluno for ‘2’ pode ir para 0 ou 1
                else if (matriculaAtiva.Periodo == 2)
                {
                    //Se o período atual do aluno for ‘0’, pode ir para 0 ou 1
                    dtAnoPeriodo.Rows.Add(string.Format("{0} - 0", proximoAno));
                    dtAnoPeriodo.Rows.Add(string.Format("{0} - 1", proximoAno));
                }
            }

            return dtAnoPeriodo;
        }

        public RN.RenovacaoMatricula.Entidades.Renovacao ObtemRenovacaoPor(string aluno, int situacao, int ano, int periodo)
        {
            RN.RenovacaoMatricula.Entidades.Renovacao renovacao = new RN.RenovacaoMatricula.Entidades.Renovacao();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  RENOVACAOID ,
                                                R.ALUNOID ,
                                                UNIDADEENSINOID ,
                                                CURSOID ,
                                                TURNOID ,
                                                ANO ,
                                                PERIODO ,
                                                R.SERIE ,
                                                SITUACAORENOVACAOID ,
                                                USUARIO ,
                                                DATAALTERACAO ,
                                                TIPOVAGA ,
                                                ENSINORELIGIOSO ,
                                                LINGUAESTRANGEIRA ,
                                                DATACADASTRO
                                        FROM    DBO.RENOVACAO R
                                        WHERE   SITUACAORENOVACAOID IN (1)
                                                AND R.ALUNOID = @ALUNO 
                                                AND R.ANO = @ANO 
                                                AND R.PERIODO = @PERIODO ";

                contextQuery.Parameters.Add("@SITUACAORENOVACAOID", situacao);
                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    renovacao.RenovacaoId = Convert.ToInt32(reader["RENOVACAOID"]);
                    renovacao.AlunoId = Convert.ToString(reader["ALUNOID"]);
                    renovacao.UnidadeEnsinoId = Convert.ToString(reader["UNIDADEENSINOID"]);
                    renovacao.CursoId = Convert.ToString(reader["CURSOID"]);
                    renovacao.TurnoId = Convert.ToString(reader["TURNOID"]);
                    renovacao.Ano = Convert.ToInt32(reader["ANO"]);
                    renovacao.Periodo = Convert.ToInt32(reader["PERIODO"]);
                    renovacao.Serie = Convert.ToInt32(reader["SERIE"]);
                    renovacao.SituacaoRenovacaoId = Convert.ToInt32(reader["SITUACAORENOVACAOID"]);
                    renovacao.Usuario = Convert.ToString(reader["USUARIO"]);
                    renovacao.TipoVaga = Convert.ToString(reader["TIPOVAGA"]);
                    renovacao.EnsinoReligioso = Convert.ToBoolean(reader["ENSINORELIGIOSO"]);
                    renovacao.LinguaEstrangeira = Convert.ToBoolean(reader["LINGUAESTRANGEIRA"]);
                    renovacao.DataCadastro = Convert.ToDateTime(reader["DATACADASTRO"]);
                    if (reader["DATAALTERACAO"] != DBNull.Value)
                    {
                        renovacao.DataAlteracao = Convert.ToDateTime(reader["DATAALTERACAO"]);
                    }
                }

                return renovacao;
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

        public IList<DTOs.DadosPossiveisRenovacoes> ListaPossiveisRenovacoesPor(RN.Agenda.Entidades.Agenda agenda, int proximoAno, int proximoPeriodo, string aluno, DateTime dataNascimentoAluno, string tipoBuscaTurnos)
        {
            RN.Aluno rnAluno = new Aluno();
            IList<DTOs.DadosPossiveisRenovacoes> listaRenovacoes = new List<DTOs.DadosPossiveisRenovacoes>();
            DTOs.DadosPossiveisRenovacoes dados = new DTOs.DadosPossiveisRenovacoes();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            RN.Matricula rnMatricula = new Matricula();
            DadosEnturmacaoAluno dadosAtualAluno = new DadosEnturmacaoAluno();
            StringBuilder sql = new StringBuilder();
            int idade = 0;
            string dataCalculoIdade = string.Empty;
            bool necessidadeEspecial = false;

            try
            {
                //Verifica se aluno possui necessidade especial
                necessidadeEspecial = rnAluno.PossuiNecessidadeEspecialPor(aluno);

                //Verifica qual será a data para calculo da idade do aluno
                if (proximoPeriodo == 2)
                {
                    //Para período 2: considerar a data de 31/07/ano letivo em questão. 
                    //Exemplo: renovações para o ano letivo de 2015/2 validar a idade do aluno com referência na data de 31/07/2015. 
                    dataCalculoIdade = "31/07/" + Convert.ToString(proximoAno);
                }
                else
                {
                    //Para o período 0 e 1 do ano letivo: considerar a data de 31/01/ano letivo em questão. 
                    //Exemplo: renovações para o ano letivo de 2015/0 ou 2015/1 validar a idade do aluno com referência na data de 31/01/2015; 
                    dataCalculoIdade = "31/01/" + Convert.ToString(proximoAno);
                }

                idade = Utils.CalcularIdadePorData(dataNascimentoAluno, Convert.ToDateTime(dataCalculoIdade));

                //Busca dados da matricula principal atual do aluno               
                dadosAtualAluno = rnMatricula.ObtemMatriculaPrincipalAtivaPor(aluno);

                //Caso o aluno não tenha matricula principal ativa retorna erro.
                if (string.IsNullOrEmpty(dadosAtualAluno.Aluno))
                {
                    throw new Exception("O aluno não possui matricula principal ativa.");
                }

                sql.Append(@" DECLARE @Absorvidas TABLE
                            (
                              UNIDADEENSINOORIGEMID VARCHAR(20),
			                  UNIDADEENSINODESTINOID VARCHAR(20),
			                  NIVELABSORCAOID INT,
			                  CURSOORIGEMID VARCHAR(20),
			                  TURNOORIGEMID CHAR(1),
			                  SERIEORIGEMID INT,
			                  Unique Clustered (UNIDADEENSINOORIGEMID,UNIDADEENSINODESTINOID, NIVELABSORCAOID,CURSOORIGEMID, TURNOORIGEMID, SERIEORIGEMID)
			                  );

                INSERT INTO @Absorvidas
                SELECT SA.UNIDADEENSINOORIGEMID,SA.UNIDADEENSINODESTINOID,SA.NIVELABSORCAOID,CURSOORIGEMID,TURNOORIGEMID,SERIEORIGEMID
                FROM SERIEABSORVIDA SA (NOLOCK)
                WHERE SA.UNIDADEENSINOORIGEMID = @ESCOLA_ATUAL_ALUNO;

                DECLARE @Serie TABLE
                            (              
			                  CURSO VARCHAR(20),
			                  TURNO CHAR(1),
			                  SERIE INT
			                  --Unique Clustered (CURSO, TURNO, SERIE)
			                  );

                INSERT INTO @Serie
                SELECT DISTINCT CURSO, TURNO, SERIE FROM LY_SERIE (NOLOCK);


                DECLARE @TurnosDisponiveis  TABLE
                            (  
			                  ANO INT,
			                  PERIODO INT,
			                  CENSO  VARCHAR(20),
			                  CURSO VARCHAR(20),
			                  TURNO CHAR(1),
			                  SERIE INT
			                  --Unique Clustered (ANO,PERIODO,CENSO,CURSO, TURNO, SERIE)
			                  );

                INSERT INTO @TurnosDisponiveis 
                ");

                if (tipoBuscaTurnos == TipoBuscaTurnosEVagas)
                {
                    sql.Append(@" SELECT CV.ANO,CV.PERIODO,CT.CENSO,CV.CURSO,CT.TURNO,CV.SERIE 
                FROM TCE_CTV_AGENDA_CONF_TURNO_VAGA CV 
                INNER JOIN TCE_CTV_CONF_TURNO CT ON CT.ID_AGENDA_CONF_TURNO_VAGA = CV.ID_AGENDA_CONF_TURNO_VAGA
                                              AND ((CT.CENSO = @ESCOLA_ATUAL_ALUNO AND CT.CONTINUIDADE = 1) 
													OR (CT.CENSO <> @ESCOLA_ATUAL_ALUNO AND CT.NOVO = 1))
		        WHERE CV.ANO = @PROXIMO_ANO -- Proximo Ano  
                AND CV.PERIODO = @PROXIMO_PERIODO -- Proximo Periodo  
                    AND NOT EXISTS ( SELECT 1 -- Considerar restrição
                                                 FROM   TCE_CTV_RESTRICAO R
                                                 WHERE  R.ID_AGENDA_CONF_TURNO_VAGA = CT.ID_AGENDA_CONF_TURNO_VAGA
                                                        AND R.CENSO = CT.CENSO )
                ");

                }
                else if (tipoBuscaTurnos == TipoBuscaControleDeVagas)
                {
                    sql.Append(@" select CV.ANO,CV.PERIODO,CV.CENSO,CV.CURSO,CV.TURNO,CV.SERIE FROM TCE_CONTROLE_VAGA CV (NOLOCK)
                where  CV.ANO = @PROXIMO_ANO -- Proximo Ano  
                AND CV.PERIODO = @PROXIMO_PERIODO -- Proximo Periodo  
                AND ((CV.CENSO = @ESCOLA_ATUAL_ALUNO AND CV.VAGAS_CONTINUIDADE > 0) 
						OR (CV.CENSO <> @ESCOLA_ATUAL_ALUNO AND cv.VAGAS_NOVAS > 0));	 
                            ");
                }

                sql.Append(@" DECLARE @CurriculosVigentes  TABLE
                            (CURRICULO VARCHAR(20)
			                );

                INSERT INTO @CurriculosVigentes
                SELECT DISTINCT CURRICULO
                FROM    LY_CURRICULO (NOLOCK) 
                WHERE   CURSO = @CURSO_ATUAL_ALUNO --Curso atual  
                        AND TURNO = @TURNO_ATUAL_ALUNO --Turno atual  
		                AND (DT_EXTINCAO IS NULL OR DT_EXTINCAO >= GETDATE());


                DECLARE @TurnosHabilitados  TABLE
                            (  
			                  UNIDADE_ENS  VARCHAR(20),
			                  CURSO VARCHAR(20)
			                  );

                INSERT INTO @TurnosHabilitados
                select DISTINCT uc.UNIDADE_ENS,uc.CURSO
                from LY_UNIDADE_ENSINO_CURSOS uc (nolock)
                where 
                 (Uc.UNIDADE_ENS = @ESCOLA_ATUAL_ALUNO --Escola atual  
                 or
                 Uc.UNIDADE_ENS in (Select sa.UNIDADEENSINODESTINOID from @Absorvidas sa)
                 );

                WITH RenovacoesPossiveis (
	                SERIE_SEGUINTE, TURNO, TURNO_DESCRICAO, UNIDADE_ENS, NOME_COMP, MODALIDADE, MODALIDADE_DESCRICAO, TIPO, TIPO_DESCRICAO,
	                CURSO_SEGUINTE, CURSO_DESCRICAO, MOD_SEG_CURSO ) AS (
	                SELECT DISTINCT
		                S.SERIE AS SERIE_SEGUINTE ,
                        S.TURNO ,
                        T.DESCRICAO TURNO_DESCRICAO,
                        U.UNIDADE_ENS ,
                        U.NOME_COMP ,
                        M.MODALIDADE ,
                        M.DESCRICAO MODALIDADE_DESCRICAO ,
                        TC.TIPO ,
                        TC.DESCRICAO AS TIPO_DESCRICAO ,
                        P.PROXIMOCURSOID CURSO_SEGUINTE ,
                        C.NOME CURSO_DESCRICAO ,
                        M.DESCRICAO + ' / ' + TC.DESCRICAO + ' / ' + 
		                C.NOME AS MOD_SEG_CURSO
	                FROM LY_CURSO C
		                INNER JOIN LY_MODALIDADE_CURSO M (NOLOCK) ON ( C.MODALIDADE = M.MODALIDADE )
                        INNER JOIN LY_TIPO_CURSO TC  (NOLOCK) ON ( C.TIPO = TC.TIPO )
		                INNER JOIN @Serie S ON ( S.CURSO = C.CURSO )
                        INNER JOIN LY_TURNO T (NOLOCK)  ON T.TURNO = S.TURNO
                        INNER JOIN PROGRESSAOSERIE P  (NOLOCK) ON ( P.PROXIMOCURSOID = s.CURSO AND P.PROXIMOSERIEID = S.SERIE )
                        INNER JOIN TCE_RESTRICAO_IDADE_SERIE R  (NOLOCK) ON ( R.CURSO = P.PROXIMOCURSOID AND R.SERIE = P.PROXIMOSERIEID )
                        INNER JOIN @TurnosHabilitados Uc   ON ( Uc.CURSO = P.PROXIMOCURSOID )
                        INNER JOIN LY_UNIDADE_ENSINO U  (NOLOCK) ON ( Uc.UNIDADE_ENS = U.UNIDADE_ENS ) 
		                INNER JOIN @TurnosDisponiveis CV  
			                ON (--CV.ANO = @PROXIMO_ANO -- Proximo Ano  
                            --AND CV.PERIODO = @PROXIMO_PERIODO -- Proximo Periodo  
                            --AND 
			                CV.CURSO = P.PROXIMOCURSOID
                            AND CV.SERIE = P.PROXIMOSERIEID
                            AND CV.CENSO = U.UNIDADE_ENS
                            AND CV.TURNO = S.TURNO
			                )
                            --AND CV.VAGAS_CONTINUIDADE > 0 )
	                WHERE
		                --UC.UNIDADE_ENS = U.UNIDADE_ENS AND
                         P.CURSOID = @CURSO_ATUAL_ALUNO  
                        AND P.SERIEID = @SERIE_ATUAL_ALUNO  
 
                            ");

                if (necessidadeEspecial)
                {
                    //Para alunos com necessidade especial verificar apenas idade minima
                    sql.Append(@"AND @IDADE >= R.IDADE_MINIMA  
                        ");
                }
                else
                {
                    sql.Append(@"AND ( @IDADE BETWEEN R.IDADE_MINIMA AND R.IDADE_MAXIMA ) 
                        ");
                }

                //                if (tipoBuscaTurnos == TipoBuscaTurnosEVagas)
                //                {
                //                    sql.Append(@" AND NOT EXISTS ( SELECT 1 -- Considerar restrição
                //                                         FROM   TCE_CTV_RESTRICAO R
                //                                         WHERE  R.ID_AGENDA_CONF_TURNO_VAGA = CT.ID_AGENDA_CONF_TURNO_VAGA
                //                                                AND R.CENSO = CT.CENSO )
                //                            ");
                //                }

                sql.Append(@" 	AND ( 
				( U.UNIDADE_ENS = @ESCOLA_ATUAL_ALUNO --Escola atual  
				  AND ( 
			--Unidade totalmente absorvida não aparece  
					NOT EXISTS ( SELECT TOP 1 1
								 FROM @Absorvidas SA
								 WHERE SA.NIVELABSORCAOID = 1 --unidade educacional  
									)  --AND SA.UNIDADEENSINOORIGEMID = Uc.UNIDADE_ENS 
					--OR 
					--   NOT EXISTS ( SELECT TOP 1 1
					--	            FROM @Absorvidas SA
					--		        )--WHERE SA.UNIDADEENSINOORIGEMID = Uc.UNIDADE_ENS 
					)
				) 
			--Unidades absorvedoras(destino) aparecem  
				OR (
					EXISTS (--Uma Absorção da escola de origem inteira 
					        SELECT TOP 1 1
                            FROM @Absorvidas SA
                            WHERE SA.NIVELABSORCAOID = 1 AND 
								SA.UNIDADEENSINODESTINOID = Uc.UNIDADE_ENS
                                --AND SA.UNIDADEENSINOORIGEMID = @ESCOLA_ATUAL_ALUNO --Escola atual  
						  )
					OR EXISTS (--Uma Absorção de um curso Inteiro da escola de origem 
					        SELECT TOP 1 1
                            FROM @Absorvidas SA
                            WHERE SA.NIVELABSORCAOID = 2 AND 
								SA.UNIDADEENSINODESTINOID = Uc.UNIDADE_ENS
                                --AND SA.UNIDADEENSINOORIGEMID = @ESCOLA_ATUAL_ALUNO --Escola atual  
								AND SA.CURSOORIGEMID = P.PROXIMOCURSOID
							)
					OR EXISTS (--Uma Absorção de um Turno Inteiro de um Curso da escola de origem 
					         SELECT TOP 1 SA.UNIDADEENSINODESTINOID
                            FROM @Absorvidas SA
                            WHERE SA.NIVELABSORCAOID = 3 AND 
								SA.UNIDADEENSINODESTINOID = Uc.UNIDADE_ENS
                                --AND SA.UNIDADEENSINOORIGEMID = @ESCOLA_ATUAL_ALUNO --Escola atual  
								AND ( SA.CURSOORIGEMID = P.PROXIMOCURSOID OR SA.CURSOORIGEMID IS NULL )
                                AND SA.TURNOORIGEMID = S.TURNO
							)
					OR EXISTS ( --Uma Absorção de uma Série toda ou/de Um Turno de um curso da escola de origem 
					        SELECT TOP 1 1
                            FROM @Absorvidas SA
                            WHERE SA.NIVELABSORCAOID = 4 AND 
								SA.UNIDADEENSINODESTINOID = Uc.UNIDADE_ENS
                                --AND SA.UNIDADEENSINOORIGEMID = @ESCOLA_ATUAL_ALUNO --Escola atual  
								AND SA.CURSOORIGEMID = P.PROXIMOCURSOID
								AND ( SA.TURNOORIGEMID = S.TURNO OR SA.TURNOORIGEMID IS NULL )
                                AND SA.SERIEORIGEMID = P.PROXIMOSERIEID
							)
				)

			)
		AND ( 
			--Cursos completamente absorvidos não aparecem  
                            NOT EXISTS ( SELECT TOP 1
                                                1
                                        FROM     @Absorvidas SA
                                        WHERE    SA.NIVELABSORCAOID = 2
                                                AND SA.UNIDADEENSINOORIGEMID = Uc.UNIDADE_ENS
                                                AND SA.CURSOORIGEMID = P.PROXIMOCURSOID )
                        )
        AND ( 
			--Turno completamente absorvido ou Curso/Turno absorvido não aparece 
                            NOT EXISTS ( SELECT TOP 1
                                                1
                                        FROM     @Absorvidas SA
                                        WHERE    SA.NIVELABSORCAOID = 3 --Nível Absorção: Turno 
                                                AND SA.UNIDADEENSINOORIGEMID = U.UNIDADE_ENS
                                                AND ( SA.CURSOORIGEMID = P.PROXIMOCURSOID
                                                        OR SA.CURSOORIGEMID IS NULL
                                                    )
                                                AND SA.TURNOORIGEMID = S.TURNO )
                        )
        AND ( 
			--Curso/Série absorvido ou Curso/Turno/Série absorvido não aparece 
                            NOT EXISTS ( SELECT TOP 1
                                                1
                                        FROM     @Absorvidas SA
                                        WHERE    SA.NIVELABSORCAOID = 4 --Nível Absorção: Série 
                                                AND SA.UNIDADEENSINOORIGEMID = U.UNIDADE_ENS
                                                AND SA.CURSOORIGEMID = P.PROXIMOCURSOID
                                                AND ( SA.TURNOORIGEMID = S.TURNO
                                                        OR SA.TURNOORIGEMID IS NULL
                                                    )
                                                AND SA.SERIEORIGEMID = P.PROXIMOSERIEID )
                                )
                        UNION
                        SELECT DISTINCT
                                SATU.SERIE_SEGUINTE ,
                                SATU.TURNO TURNO_DESCRICAO,
                                TU.DESCRICAO,
                                U.UNIDADE_ENS ,
                                U.NOME_COMP ,
                                M.MODALIDADE ,
                                M.DESCRICAO MODALIDADE_DESCRICAO ,
                                TC.TIPO ,
                                TC.DESCRICAO TIPO_DESCRICAO ,
                                SATU.CURSO_SEGUINTE ,
                                C.NOME CURSO_DESCRICAO ,
                                M.DESCRICAO + ' / ' + TC.DESCRICAO + ' / ' + 
								C.NOME AS MOD_SEG_CURSO
                        FROM    LY_CURSO C (NOLOCK) 
                                INNER JOIN LY_SERIE SATU (NOLOCK)  -- SERIE ATUAL 
                                ON ( SATU.CURSO_SEGUINTE = C.CURSO )
                                INNER JOIN LY_TURNO TU  (NOLOCK) ON TU.TURNO = SATU.TURNO
                                INNER JOIN LY_MODALIDADE_CURSO M (NOLOCK)  ON ( C.MODALIDADE = M.MODALIDADE )
                                INNER JOIN LY_TIPO_CURSO TC  (NOLOCK) ON ( C.TIPO = TC.TIPO )
                                INNER JOIN @TurnosHabilitados Uc   ON ( Uc.CURSO = C.CURSO )
                                INNER JOIN LY_UNIDADE_ENSINO U (NOLOCK)  ON ( Uc.UNIDADE_ENS = U.UNIDADE_ENS )
	INNER JOIN @TurnosDisponiveis CV 
                                ON -- CV.ANO = @PROXIMO_ANO -- Proximo Ano  
                                --AND CV.PERIODO = @PROXIMO_PERIODO -- Proximo Periodo  
                                --AND
								 CV.CURSO = SATU.CURSO_SEGUINTE
                                AND CV.SERIE = SATU.SERIE_SEGUINTE
                                AND CV.CENSO = Uc.UNIDADE_ENS
                                AND CV.TURNO = SATU.TURNO
                                --AND CV.VAGAS_CONTINUIDADE > 0 
	INNER JOIN TCE_RESTRICAO_IDADE_SERIE R (NOLOCK)  ON ( R.CURSO = SATU.CURSO_SEGUINTE
                                                                        AND R.SERIE = SATU.SERIE_SEGUINTE
                                                                        )
                        WHERE    UC.UNIDADE_ENS = U.UNIDADE_ENS 
                                AND SATU.CURSO = @CURSO_ATUAL_ALUNO --Curso atual  
                                AND SATU.TURNO = @TURNO_ATUAL_ALUNO --Turno atual 
	AND SATU.CURRICULO IN (SELECT CURRICULO FROM @CurriculosVigentes )--Curriculo Atual  
                                AND SATU.SERIE = @SERIE_ATUAL_ALUNO 
                                        ");

                //                if (tipoBuscaTurnos == TipoBuscaTurnosEVagas)
                //                {
                //                    sql.Append(@"AND NOT EXISTS ( SELECT 1 -- Considerar restrição
                //                                                 FROM   TCE_CTV_RESTRICAO R
                //                                                 WHERE  R.ID_AGENDA_CONF_TURNO_VAGA = CT.ID_AGENDA_CONF_TURNO_VAGA
                //                                                        AND R.CENSO = CT.CENSO )
                //                            ");
                //                }


                if (necessidadeEspecial)
                {
                    //Para alunos com necessidade especial verificar apenas idade minima
                    sql.Append(@"AND @IDADE >= R.IDADE_MINIMA  
                        ");
                }
                else
                {
                    sql.Append(@"AND ( @IDADE BETWEEN R.IDADE_MINIMA AND R.IDADE_MAXIMA ) 
                        ");
                }

                sql.Append(@"  	AND ( 
				
			( U.UNIDADE_ENS = @ESCOLA_ATUAL_ALUNO --Escola atual  
            AND ( 
				--Unidade totalmente absorvida não aparece  
                NOT EXISTS ( SELECT TOP 1
                                    1
                            FROM     @Absorvidas SA
                            WHERE    SA.NIVELABSORCAOID = 1 --unidade educacional  
                                    --AND SA.UNIDADEENSINOORIGEMID = Uc.UNIDADE_ENS 
									) 
				--Unidade NÃO absorvida aparece  
					--OR NOT EXISTS ( SELECT TOP 1
     --                   SA.UNIDADEENSINODESTINOID
     --                   FROM    @Absorvidas SA
     --                   --WHERE   SA.UNIDADEENSINOORIGEMID = Uc.UNIDADE_ENS 
					--	)
                    ) 
			--fechando AND  
            ) 
			            --Unidades absorvedoras aparecem  
                          OR (
					EXISTS ( SELECT TOP 1 1
                            FROM @Absorvidas SA
                            WHERE SA.NIVELABSORCAOID = 1 AND 
								SA.UNIDADEENSINODESTINOID = Uc.UNIDADE_ENS  
                                --AND SA.UNIDADEENSINOORIGEMID = @ESCOLA_ATUAL_ALUNO --Escola atual  
						  )
					OR EXISTS ( SELECT TOP 1 1
                            FROM @Absorvidas  SA
                            WHERE SA.NIVELABSORCAOID = 2 AND 
								SA.UNIDADEENSINODESTINOID = Uc.UNIDADE_ENS
                                --AND SA.UNIDADEENSINOORIGEMID = @ESCOLA_ATUAL_ALUNO --Escola atual  
								AND SA.CURSOORIGEMID = SATU.CURSO_SEGUINTE
							)
					OR EXISTS ( SELECT TOP 1 1
                            FROM @Absorvidas SA
                            WHERE SA.NIVELABSORCAOID = 3 AND 
								SA.UNIDADEENSINODESTINOID = Uc.UNIDADE_ENS
                               -- AND SA.UNIDADEENSINOORIGEMID = @ESCOLA_ATUAL_ALUNO --Escola atual  
								AND ( SA.CURSOORIGEMID = SATU.CURSO_SEGUINTE OR SA.CURSOORIGEMID IS NULL )
                                AND SA.TURNOORIGEMID = SATU.TURNO
							)
					OR EXISTS ( SELECT TOP 1 1
                            FROM @Absorvidas SA
                            WHERE SA.NIVELABSORCAOID = 4 AND 
								SA.UNIDADEENSINODESTINOID = Uc.UNIDADE_ENS
                                --AND SA.UNIDADEENSINOORIGEMID = @ESCOLA_ATUAL_ALUNO --Escola atual  
								AND SA.CURSOORIGEMID = SATU.CURSO_SEGUINTE
								AND ( SA.TURNOORIGEMID = SATU.TURNO OR SA.TURNOORIGEMID IS NULL )
                                AND SA.SERIEORIGEMID = SATU.SERIE_SEGUINTE
							)
				)
                                )
                                AND ( 
			            --Cursos completamente absorvidos não aparecem  
                                        NOT EXISTS ( SELECT TOP 1
                                                            1
                                                    FROM     @Absorvidas SA
                                                    WHERE    SA.NIVELABSORCAOID = 2
                                                            AND SA.UNIDADEENSINOORIGEMID = Uc.UNIDADE_ENS
                                                            AND SA.CURSOORIGEMID = SATU.CURSO_SEGUINTE )
                                    )
                                AND ( 
			            --Turno completamente absorvido ou Curso/Turno absorvido não aparece 
                                        NOT EXISTS ( SELECT TOP 1
                                                            1
                                                    FROM     @Absorvidas SA
                                                    WHERE    SA.NIVELABSORCAOID = 3 --Nível Absorção: Turno 
                                                            AND SA.UNIDADEENSINOORIGEMID = U.UNIDADE_ENS
                                                            AND ( SA.CURSOORIGEMID = SATU.CURSO_SEGUINTE
                                                                    OR SA.CURSOORIGEMID IS NULL
                                                                )
                                                            AND SA.TURNOORIGEMID = SATU.TURNO )
                                    )
                                AND ( 
			            --Curso/Série absorvido ou Curso/Turno/Série absorvido não aparece 
                                        NOT EXISTS ( SELECT TOP 1
                                                            1
                                                    FROM     @Absorvidas SA
                                                    WHERE    SA.NIVELABSORCAOID = 4 --Nível Absorção: Série 
                                                            AND SA.UNIDADEENSINOORIGEMID = U.UNIDADE_ENS
                                                            AND SA.CURSOORIGEMID = SATU.CURSO_SEGUINTE
                                                            AND ( SA.TURNOORIGEMID = SATU.TURNO
                                                                    OR SA.TURNOORIGEMID IS NULL
                                                                )
                                                            AND SA.SERIEORIGEMID = SATU.SERIE_SEGUINTE )
                                    )
                                 
						) 
                            ");


                if (agenda.CursoPorUnidade)
                {
                    //Se Agenda possui Curso por unidade
                    sql.Append(@"SELECT  *
                        FROM    RenovacoesPossiveis RP
                        WHERE   EXISTS ( SELECT 1
                                         FROM   Agenda.AGENDA_UNIDADEENSINO AU
                                                INNER JOIN AGENDA.AGENDA_CURSO__AGENDA_UNIDADEENSINO ACU ON ACU.AGENDA_UNIDADEENSINO_ID = AU.UNIDADEENSINOID
                                                INNER JOIN AGENDA.AGENDA_CURSO AC ON AC.AGENDA_CURSO_ID = ACU.AGENDA_CURSO_ID
                                         WHERE  au.AGENDAID = @AGENDAID
                                                AND AU.UNIDADEENSINOID = RP.UNIDADE_ENS
                                                AND AC.CURSOID = RP.CURSO_SEGUINTE
                                                AND AC.SERIE = RP.SERIE_SEGUINTE
                                                AND AC.TURNOID = RP.TURNO ) 
                        ");
                }
                else
                {
                    //Se não possui curso por unidade 
                    sql.Append(@"SELECT  *
                        FROM    RenovacoesPossiveis RP
                        WHERE ");

                    if (agenda.ParticipaUnidadeId == 1)
                    {
                        //Se PARTICIPA_UNIDADE = 1
                        sql.Append(@" EXISTS(SELECT 1
	                        FROM Agenda.AGENDA_UNIDADEENSINO au
	                        WHERE au.AGENDAID = @AGENDAID
	                        AND au.UNIDADEENSINOID = RP.UNIDADE_ENS ) 
                        ");
                    }
                    else if (agenda.ParticipaUnidadeId == 2)
                    {
                        //Se PARTICIPA_UNIDADE = 2
                        sql.Append(@" NOT EXISTS ( SELECT 1
                             FROM   Agenda.AGENDA_UNIDADEENSINO au
                             WHERE  au.AGENDAID = @AGENDAID
                                    AND au.UNIDADEENSINOID = RP.UNIDADE_ENS )
                        ");
                    }
                    else
                    {
                        //Se PARTICIPA_UNIDADE = 0 não precisa de filtro
                        sql.Append(@" 1 = 1
                        ");
                    }

                    if (agenda.ParticipaCursoId == 1)
                    {
                        //SE PARTIPA_CURSO = 1  
                        sql.Append(@" AND   EXISTS(SELECT 1 
                          FROM Agenda.AGENDA_CURSO AC 
                          WHERE AC.AGENDAID = @AGENDAID 
                          AND AC.CURSOID = RP.CURSO_SEGUINTE 
                          AND AC.SERIE = RP.SERIE_SEGUINTE
                          AND AC.TURNOID = RP.TURNO) 
                        ");
                    }
                    else if (agenda.ParticipaCursoId == 1)
                    {
                        sql.Append(@" AND NOT EXISTS ( SELECT 1
                                 FROM   Agenda.AGENDA_CURSO AC
                                 WHERE  AC.AGENDAID = @AGENDAID
                                        AND AC.CURSOID = RP.CURSO_SEGUINTE
                                        AND AC.SERIE = RP.SERIE_SEGUINTE
                                        AND AC.TURNOID = RP.TURNO )
                        ");
                    }
                    //Se PARTICIPA_CURSO = 0 não precisa de filtro                    
                }

                contextQuery.Command = sql.ToString();

                contextQuery.Parameters.Add("@PROXIMO_ANO", proximoAno);
                contextQuery.Parameters.Add("@PROXIMO_PERIODO", proximoPeriodo);
                contextQuery.Parameters.Add("@ESCOLA_ATUAL_ALUNO", dadosAtualAluno.Censo);
                contextQuery.Parameters.Add("@CURSO_ATUAL_ALUNO", dadosAtualAluno.Curso);
                contextQuery.Parameters.Add("@TURNO_ATUAL_ALUNO", dadosAtualAluno.Turno);
                contextQuery.Parameters.Add("@SERIE_ATUAL_ALUNO", dadosAtualAluno.Serie);
                contextQuery.Parameters.Add("@IDADE", idade);
                contextQuery.Parameters.Add("@AGENDAID", agenda.AgendaId);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dados = new Techne.Lyceum.RN.DTOs.DadosPossiveisRenovacoes();

                    dados.SerieSeguinte = Convert.ToInt32(reader["SERIE_SEGUINTE"]);
                    dados.Turno = Convert.ToString(reader["TURNO"]);
                    dados.TurnoNome = Convert.ToString(reader["TURNO_DESCRICAO"]);
                    dados.UnidadeEnsino = Convert.ToString(reader["UNIDADE_ENS"]);
                    dados.UnidadeEnsinoNome = Convert.ToString(reader["NOME_COMP"]);
                    dados.Modalidade = Convert.ToString(reader["MODALIDADE"]);
                    dados.ModalidadeDescricao = Convert.ToString(reader["MODALIDADE_DESCRICAO"]);
                    dados.Tipo = Convert.ToString(reader["TIPO"]);
                    dados.TipoDescricao = Convert.ToString(reader["TIPO_DESCRICAO"]);
                    dados.Curso = Convert.ToString(reader["CURSO_SEGUINTE"]);
                    dados.CursoDescricao = Convert.ToString(reader["CURSO_DESCRICAO"]);
                    dados.ModalidadeSegmentoCurso = Convert.ToString(reader["MOD_SEG_CURSO"]);

                    listaRenovacoes.Add(dados);
                }

                return listaRenovacoes;
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

        public DataTable ObtemListaAlunosComDuplicicadeRenovacaoPor(IList<string> alunos)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable listaAlunosNomes = null;
            int situacao = (int)RN.RenovacaoMatricula.Entidades.SituacaoRenovacao.Ativo;
            string listaMatriculas = string.Empty;

            try
            {
                foreach (string aluno in alunos)
                {
                    if (!string.IsNullOrEmpty(listaMatriculas))
                    {
                        listaMatriculas = listaMatriculas + ",";
                    }

                    listaMatriculas = string.Format(@"{0}'{1}'", listaMatriculas, aluno);
                }

                contextQuery.Command = string.Format(@" SELECT  PE.NOME_COMPL ,
                                R.ALUNOID
                        FROM    DBO.RENOVACAO R
                                INNER JOIN DBO.LY_ALUNO A ON R.ALUNOID = A.ALUNO
                                INNER JOIN LY_PESSOA PE (NOLOCK)  ON PE.PESSOA = A.PESSOA
                        WHERE   SITUACAORENOVACAOID = @SITUACAORENOVACAOID
                                AND R.ALUNOID IN ( {0} )
                        GROUP BY PE.NOME_COMPL ,
                                R.ALUNOID
                        HAVING  COUNT(R.RENOVACAOID) > 1 ", listaMatriculas);

                contextQuery.Parameters.Add("@SITUACAORENOVACAOID", situacao);

                listaAlunosNomes = ctx.GetDataTable(contextQuery);
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

            return listaAlunosNomes;
        }

        public bool PossuiRenovacaoAtivaConfirmadaPor(string aluno, int ano, int periodo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.PossuiRenovacaoAtivaConfirmadaPor(contexto, aluno, ano, periodo);
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
        public bool PossuiRenovacaoAtivaConfirmadaPor(DataContext contexto, string aluno, string censo, int ano, int periodo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;
            string possiveisPeriodos = Utils.RecuperaPossiveisPeriodos(Convert.ToInt32(periodo));

            contextQuery.Command = string.Format(@" SELECT  COUNT(*)
                                        FROM    RENOVACAO (NOLOCK)
                                        WHERE   SITUACAORENOVACAOID IN (1, 3)
                                                AND UNIDADEENSINOID = @CENSO
                                                AND ANO = @ANO
                                                AND PERIODO IN ({0})
                                                AND ALUNOID = @ALUNOID ", possiveisPeriodos);

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@ALUNOID", SqlDbType.VarChar, aluno);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool EhRenovacaoAtivaConfirmadaPor(DataContext contexto, int renovacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT  COUNT(*)
                                        FROM    RENOVACAO (NOLOCK)
                                        WHERE   SITUACAORENOVACAOID IN (1, 3)
                                                AND RENOVACAOID = @RENOVACAOID  ";

            contextQuery.Parameters.Add("@RENOVACAOID", SqlDbType.Int, renovacaoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiRenovacaoAtivaConfirmadaPor(DataContext contexto, string aluno, int ano, int periodo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;
            string possiveisPeriodos = Utils.RecuperaPossiveisPeriodosCompleto(Convert.ToInt32(periodo));

            contextQuery.Command = string.Format(@" SELECT  COUNT(*)
                                        FROM    RENOVACAO (NOLOCK)
                                        WHERE   SITUACAORENOVACAOID IN (1, 3)
                                                AND ANO = @ANO
                                                AND PERIODO IN ( {0} )
                                                AND ALUNOID = @ALUNOID  ", possiveisPeriodos);

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
            contextQuery.Parameters.Add("@ALUNOID", SqlDbType.VarChar, aluno);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiRenovacaoAtivaPor(DataContext ctx, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @"  SELECT COUNT(*)
                                    SITUACAORENOVACAOID
                             FROM   RENOVACAO
                             WHERE  ALUNOID = @ALUNOID
                                    AND SITUACAORENOVACAOID = 1 ";

            contextQuery.Parameters.Add("@ALUNOID", aluno);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public bool PossuiRenovacaoAtivaPor(int ano, int periodo, string censo, string curso, string turno, int serie, string tipoVaga)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @"  SELECT  COUNT(*)
                                        FROM    RENOVACAO
                                        WHERE   SITUACAORENOVACAOID = 1
                                                AND UNIDADEENSINOID = @CENSO
                                                AND ANO = @ANO
                                                AND PERIODO = @PERIODO
                                                AND CURSOID = @CURSO
                                                AND SERIE = @SERIE
                                                AND TURNOID = @TURNO
                                                AND TIPOVAGA = @TIPOVAGA ";

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@SERIE", serie);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@TIPOVAGA", tipoVaga);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
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
        public DadosFichaRenovacao ObtemFichaRenovacaoAlunoPor(int idRenovacao)
        {
            DadosFichaRenovacao dados = new DadosFichaRenovacao();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  A.PESSOA ,
                                P.NOME_COMPL ,
                                P.DT_NASC ,
                                P.SEXO ,
                                P.QT_FILHOS ,
                                P.TIPO_SANGUINEO ,
                                P.ETNIA ,
                                P.EST_CIVIL ,
                                PA.NOME AS PAIS_NASC ,
                                P.NACIONALIDADE ,
                                MNASC.UF AS UF_NASC ,
                                MNASC.NOME AS MUNICIPIO_NASC ,
                                P.CREDO ,
                                P.NECESSIDADEESPECIALID ,
                                NEC.DESCRICAO AS NECESSIDADE_ESPECIAL ,
                                A.ALUNO ,
                                P.NOME_MAE ,
                                P.MAE_FALECIDA ,
                                P.MAE_CPF ,
                                P.NOME_PAI ,
                                P.PAI_FALECIDO ,
                                P.PAI_CPF ,
                                P.RESPONSAVEL ,
                                P.RESP_NOME_COMPL ,
                                P.RESP_CPF ,
                                MAE_TELEFONE ,
                                PAI_TELEFONE ,
                                RESP_FONE ,
                                P.ENDERECO ,
                                P.END_NUM ,
                                P.END_COMPL ,
                                P.BAIRRO ,
                                MEND.NOME AS END_MUNICIPIO ,
                                MEND.UF AS END_UF ,
                                P.CEP ,
                                FLP.FL_FIELD_01 AS LOCALIZACAO ,
                                P.FONE ,
                                P.CELULAR ,
                                A.E_MAIL_INTERNO ,
                                P.CPF ,
                                P.RG_TIPO ,
                                P.RG_NUM ,
                                FLP.FL_FIELD_07 AS COMPLETMENTO_RG ,
                                P.RG_UF ,
                                P.RG_EMISSOR ,
                                P.RG_DTEXP ,
                                P.ID_CENSO ,
                                FLP.FL_FIELD_08 AS NIS ,
                                FLP.FL_FIELD_02 AS TIPO_CERTIDAO ,
                                FLP.FL_FIELD_09 AS CERTIDAO_CIVIL ,
                                C.UF AS UF_CARTORIO ,
                                C.MUNICIPIO AS MUNICIPIO_CARTORIO ,
                                C.NOME_CARTORIO ,
                                P.CERT_NASC_LIVRO ,
                                P.CERT_NASC_FOLHA ,
                                P.CERT_NASC_NUM ,
                                P.CERT_NASC_EMISSAO ,
                                P.CERT_NUMERO_MATRICULA ,
                                FLP.FL_FIELD_04 AS GRATUIDADE ,
                                FLP.FL_FIELD_10 AS PODER_PUBLICO_TRANSPORTE ,
                                FLP.FL_FIELD_05 AS MODAIS ,
                                R.ANO ,
                                R.PERIODO ,
                                R.UNIDADEENSINOID ,
                                UE.NOME_COMP AS ESCOLA_DESCRICAO ,
                                MC.DESCRICAO AS MODALIDADE ,
                                R.CURSOID ,
                                CUR.NOME AS CURSO_DESCRICAO ,
                                R.SERIE ,
                                TU.DESCRICAO AS TURNO ,
                                R.DATACADASTRO AS DT_SUGERIDA ,
                                R.ENSINORELIGIOSO  ,
                                R.LINGUAESTRANGEIRA ,
                                R.SITUACAORENOVACAOID  ,
                                ISNULL(R.DATAALTERACAO, R.DATACADASTRO) AS DT_ALTERACAO ,
                                R.USUARIO AS USUARIO_RESPONSAVEL ,
                                U.NOME AS USUARIO_RESPONSAVEL_NOME
                        FROM    DBO.RENOVACAO R ( NOLOCK )
                                LEFT JOIN HADES.DBO.HD_USUARIO U ( NOLOCK ) ON R.USUARIO = U.USUARIO
                                INNER JOIN DBO.LY_CURSO CUR ( NOLOCK ) ON R.CURSOID = CUR.CURSO
                                INNER JOIN DBO.LY_MODALIDADE_CURSO MC ( NOLOCK ) ON CUR.MODALIDADE = MC.MODALIDADE
                                INNER JOIN DBO.LY_UNIDADE_ENSINO UE ( NOLOCK ) ON R.UNIDADEENSINOID = UE.UNIDADE_ENS
                                INNER JOIN DBO.LY_TURNO TU ( NOLOCK ) ON R.TURNOID = TU.TURNO
                                INNER JOIN LY_ALUNO A ( NOLOCK ) ON A.ALUNO = R.ALUNOID
                                INNER JOIN DBO.LY_PESSOA P ( NOLOCK ) ON A.PESSOA = P.PESSOA
                                LEFT JOIN DBO.LY_FL_PESSOA FLP ( NOLOCK ) ON P.PESSOA = FLP.PESSOA
                                LEFT JOIN HADES.DBO.HD_MUNICIPIO MNASC ( NOLOCK ) ON P.MUNICIPIO_NASC = MNASC.MUNICIPIO
                                LEFT JOIN HADES.DBO.HD_PAIS PA ( NOLOCK ) ON P.PAIS_NASC = PA.PAIS
                                LEFT JOIN HADES.DBO.HD_MUNICIPIO MEND ( NOLOCK ) ON P.END_MUNICIPIO = MEND.MUNICIPIO
                                LEFT JOIN CARTORIO C ( NOLOCK ) ON C.COD_CARTORIO = P.ID_CARTORIO
                                LEFT JOIN HADES.dbo.NECESSIDADEESPECIAL NEC ON NEC.NECESSIDADEESPECIALID=P.NECESSIDADEESPECIALID
                        WHERE   R.RENOVACAOID = @RENOVACAOID ";

                contextQuery.Parameters.Add("@RENOVACAOID", idRenovacao);

                reader = ctx.GetDataReader(contextQuery);

                if (reader.Read())
                {
                    //Preenche dados pessoais
                    dados.Pessoa = Convert.ToInt32(reader["PESSOA"]);
                    dados.AlunoMatricula = Convert.ToString(reader["ALUNO"]);
                    dados.NomeAluno = Convert.ToString(reader["NOME_COMPL"]);
                    dados.DataNascimento = Convert.ToDateTime(reader["DT_NASC"]);
                    dados.Sexo = Convert.ToString(reader["SEXO"]);
                    if (reader["QT_FILHOS"] != DBNull.Value)
                    {
                        dados.QuantidadeFilhos = Convert.ToInt32(reader["QT_FILHOS"]);
                    }

                    dados.TipoSanguineo = Convert.ToString(reader["TIPO_SANGUINEO"]);
                    dados.Etnia = Convert.ToString(reader["ETNIA"]);
                    dados.EstadoCivil = Convert.ToString(reader["EST_CIVIL"]);
                    dados.PaisNascimento = Convert.ToString(reader["PAIS_NASC"]);
                    dados.Nacionalidade = Convert.ToString(reader["NACIONALIDADE"]);
                    dados.UfNascimento = Convert.ToString(reader["UF_NASC"]);
                    dados.Naturalidade = Convert.ToString(reader["MUNICIPIO_NASC"]);
                    dados.Credo = Convert.ToString(reader["CREDO"]);

                    if (reader["NECESSIDADE_ESPECIAL"] != DBNull.Value)
                    {
                        dados.NecessidadeEspecial = Convert.ToString(reader["NECESSIDADE_ESPECIAL"]);
                    }

                    if (reader["DT_ALTERACAO"] != DBNull.Value)
                    {
                        dados.DataSituacao = Convert.ToDateTime(reader["DT_ALTERACAO"]);
                    }

                    //Preenche filiacao
                    dados.NomeMae = Convert.ToString(reader["NOME_MAE"]);
                    dados.FalecidaMae = Convert.ToString(reader["MAE_FALECIDA"]);
                    dados.CPFMae = Convert.ToString(reader["MAE_CPF"]);
                    dados.NomePai = Convert.ToString(reader["NOME_PAI"]);
                    dados.FalecidoPai = Convert.ToString(reader["PAI_FALECIDO"]);
                    dados.CPFPai = Convert.ToString(reader["PAI_CPF"]);
                    dados.ResponsavelLegal = Convert.ToString(reader["RESPONSAVEL"]);
                    if (dados.ResponsavelLegal.ToUpper() == "OUTROS;")
                    {
                        dados.NomeOutros = Convert.ToString(reader["RESP_NOME_COMPL"]);
                        dados.CpfOutros = Convert.ToString(reader["RESP_CPF"]);
                        dados.TelResponsavel = Convert.ToString(reader["RESP_FONE"]);
                    }
                    dados.TelMae = Convert.ToString(reader["MAE_TELEFONE"]);
                    dados.TelPai = Convert.ToString(reader["PAI_TELEFONE"]);

                    //Preenche endereço
                    dados.Endereco = Convert.ToString(reader["ENDERECO"]);
                    dados.NumeroEndereco = Convert.ToString(reader["END_NUM"]);
                    dados.ComplementoEndereco = Convert.ToString(reader["END_COMPL"]);
                    dados.BairroEndereco = Convert.ToString(reader["BAIRRO"]);
                    dados.MunicipioEndereco = Convert.ToString(reader["END_MUNICIPIO"]);
                    dados.EstadoEndereco = Convert.ToString(reader["END_UF"]);
                    dados.CepEndereco = Convert.ToString(reader["CEP"]);
                    dados.LocalizacaoEndereco = Convert.ToString(reader["LOCALIZACAO"]);

                    //Preenche contato
                    dados.Telefone = Convert.ToString(reader["FONE"]);
                    dados.Celular = Convert.ToString(reader["CELULAR"]);
                    dados.Email = Convert.ToString(reader["E_MAIL_INTERNO"]);

                    //Preenche Documentos
                    dados.Cpf = Convert.ToString(reader["CPF"]);
                    dados.TipoDocumento = Convert.ToString(reader["RG_TIPO"]);
                    dados.NumeroDocumento = Convert.ToString(reader["RG_NUM"]);
                    dados.ComplementoIdentidade = Convert.ToString(reader["COMPLETMENTO_RG"]);
                    dados.EstadoDocumento = Convert.ToString(reader["RG_UF"]);
                    dados.OrgaoEmissorDocumento = Convert.ToString(reader["RG_EMISSOR"]);
                    if (reader["RG_DTEXP"] != DBNull.Value)
                    {
                        dados.DataExpedicaoDocumento = Convert.ToDateTime(reader["RG_DTEXP"]);
                    }

                    //Preenche Outras Informacoes
                    dados.Inep = Convert.ToString(reader["ID_CENSO"]);
                    dados.Nis = Convert.ToString(reader["NIS"]);

                    //Preenche Certidao Civil
                    dados.TipoCertidao = Convert.ToString(reader["TIPO_CERTIDAO"]);
                    dados.CertidaoCivil = Convert.ToString(reader["CERTIDAO_CIVIL"]);
                    dados.UfCartorio = Convert.ToString(reader["UF_CARTORIO"]);
                    dados.MunicipioCartorio = Convert.ToString(reader["MUNICIPIO_CARTORIO"]);
                    dados.NomeCartorio = Convert.ToString(reader["NOME_CARTORIO"]);
                    dados.Livro = Convert.ToString(reader["CERT_NASC_LIVRO"]);
                    dados.Folha = Convert.ToString(reader["CERT_NASC_FOLHA"]);
                    dados.NumeroTermo = Convert.ToString(reader["CERT_NASC_NUM"]);
                    if (reader["CERT_NASC_EMISSAO"] != DBNull.Value)
                    {
                        dados.DataEmissaoCertidao = Convert.ToDateTime(reader["CERT_NASC_EMISSAO"]);
                    }
                    dados.MatriculaCertidao = Convert.ToString(reader["CERT_NUMERO_MATRICULA"]);

                    //Preencher Transporte
                    dados.UtilizaTransporte = Convert.ToString(reader["GRATUIDADE"]);
                    dados.PoderResponsavel = Convert.ToString(reader["PODER_PUBLICO_TRANSPORTE"]);
                    dados.Modais = Convert.ToString(reader["MODAIS"]);

                    //Preenche Renovacao Matricula
                    dados.AnoLetivo = Convert.ToInt32(reader["ANO"]);
                    dados.PeriodoLetivo = Convert.ToInt32(reader["PERIODO"]);
                    dados.UnidadeEnsino = Convert.ToString(reader["ESCOLA_DESCRICAO"]);
                    dados.Censo = Convert.ToString(reader["UNIDADEENSINOID"]);
                    dados.Modalidade = Convert.ToString(reader["MODALIDADE"]);
                    dados.Curso = Convert.ToString(reader["CURSOID"]);
                    dados.CursoDescricao = Convert.ToString(reader["CURSO_DESCRICAO"]);
                    dados.Serie = Convert.ToInt32(reader["SERIE"]);
                    dados.Turno = Convert.ToString(reader["TURNO"]);
                    dados.DataSugerida = Convert.ToDateTime(reader["DT_SUGERIDA"]);
                    dados.EnsinoReligioso = Convert.ToString(reader["ENSINORELIGIOSO"]);
                    dados.LinguaEstrangueira = Convert.ToString(reader["LINGUAESTRANGEIRA"]);
                    dados.Situacao = Convert.ToString(reader["SITUACAORENOVACAOID"]);
                    dados.UsuarioResponsavel = Convert.ToString(reader["USUARIO_RESPONSAVEL"]);
                    dados.UsuarioResponsavelNome = Convert.ToString(reader["USUARIO_RESPONSAVEL_NOME"]);
                }

                //Buscar Foto do aluno
                dados.Foto = FotoPessoa.Carregar(Convert.ToInt32(dados.Pessoa));

                //Ajusta descriçoes e formataçoes necessárias
                if (dados.Sexo == "F")
                {
                    dados.Sexo = "Feminino";
                }
                else if (dados.Sexo == "M")
                {
                    dados.Sexo = "Masculino";
                }

                if (dados.NomeMae != "NÃO DECLARADO")
                {
                    if (dados.FalecidaMae == "S")
                    {
                        dados.FalecidaMae = "Sim";
                    }
                    else if (dados.FalecidaMae == "N")
                    {
                        dados.FalecidaMae = "Não";
                    }
                }
                else
                {
                    dados.FalecidaMae = string.Empty;
                }

                if (dados.NomePai != "NÃO DECLARADO")
                {
                    if (dados.FalecidoPai == "S")
                    {
                        dados.FalecidoPai = "Sim";
                    }
                    else if (dados.FalecidoPai == "N")
                    {
                        dados.FalecidoPai = "Não";
                    }
                }
                else
                {
                    dados.FalecidoPai = string.Empty;
                }

                if (dados.Credo == "Naodeclarado")
                {
                    dados.Credo = "Não declarado";
                }

                if (!string.IsNullOrEmpty(dados.ResponsavelLegal))
                {
                    dados.ResponsavelLegal = dados.ResponsavelLegal.Replace(';', ' ');
                }

                dados.CPFMae = dados.CPFMae.AplicarMascaraCPF();
                dados.CPFPai = dados.CPFPai.AplicarMascaraCPF();
                dados.Cpf = dados.Cpf.AplicarMascaraCPF();

                if (dados.EnsinoReligioso == "True")
                {
                    dados.EnsinoReligioso = "Sim";
                }
                else
                {
                    dados.EnsinoReligioso = "Não";
                }

                if (dados.LinguaEstrangueira == "True")
                {
                    dados.LinguaEstrangueira = "Sim";
                }
                else
                {
                    dados.LinguaEstrangueira = "Não";
                }

                if (dados.PeriodoLetivo == 0)
                {
                    dados.PeriodoLetivoDescricao = "Anual";
                }
                else if (dados.PeriodoLetivo == 1)
                {
                    dados.PeriodoLetivoDescricao = "1º Bimestre";
                }
                else
                {
                    dados.PeriodoLetivoDescricao = "2º Bimestre";
                }

                if (dados.UtilizaTransporte == "S")
                {
                    dados.UtilizaTransporte = "Sim";
                }
                else
                {
                    dados.UtilizaTransporte = "Não";
                }

                return dados;
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

        public static DataTable ListaRenovacoesMatriculaAtivaOuPossuiConfirmacaoPor(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable renovacoes = null;

            if (aluno.IsNullOrEmptyOrWhiteSpace())
            {
                return null;
            }

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                            R.RENOVACAOID ,
                            R.ALUNOID AS ALUNO ,
                            R.UNIDADEENSINOID AS CENSO ,
                            R.ANO ,
                            R.PERIODO ,
                            C.NOME AS CURSO ,
                            R.SERIE ,
                            R.TURNOID AS TURNO ,
                            E.NOME_COMP AS ESCOLA ,
                            R.UNIDADEENSINOID + ' - ' + E.NOME_COMP AS UNIDADE_ENSINO ,
                            MD.MODALIDADE ,
                            TC.TIPO AS SEGMENTO ,
                            T.DESCRICAO AS NOME_TURNO ,
                            R.CURSOID AS COD_CURSO ,
                            MD.DESCRICAO + ' / ' + TC.DESCRICAO + ' / ' + C.NOME AS MOD_SEG_CURSO,
                            R.DATACADASTRO
                    FROM    DBO.RENOVACAO R ( NOLOCK )
                            INNER JOIN DBO.LY_UNIDADE_ENSINO E ON R.UNIDADEENSINOID = E.UNIDADE_ENS
                            INNER JOIN DBO.LY_CURSO C ON R.CURSOID = C.CURSO
                            INNER JOIN DBO.LY_MODALIDADE_CURSO MD ON C.MODALIDADE = MD.MODALIDADE
                            INNER JOIN DBO.LY_TIPO_CURSO TC ON C.TIPO = TC.TIPO
                            INNER JOIN DBO.LY_TURNO T ON R.TURNOID = T.TURNO
                    WHERE   R.SITUACAORENOVACAOID IN (1, 3)
                            AND R.ALUNOID = @ALUNO
                    ORDER BY R.ANO DESC ,
                            R.DATACADASTRO ";

                contextQuery.Parameters.Add("@ALUNO", aluno);

                renovacoes = ctx.GetDataTable(contextQuery);
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

            return renovacoes;
        }

        public static DataTable ListaRenovacoesMatriculaAtivaPor(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable renovacoes = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                            R.RENOVACAOID ,
                            R.ALUNOID AS ALUNO ,
                            R.UNIDADEENSINOID AS CENSO ,
                            R.ANO ,
                            R.PERIODO ,
                            C.NOME AS CURSO ,
                            R.SERIE ,
                            R.TURNOID AS TURNO ,
                            E.NOME_COMP AS ESCOLA ,
                            R.UNIDADEENSINOID + ' - ' + E.NOME_COMP AS UNIDADE_ENSINO ,
                            MD.MODALIDADE ,
                            TC.TIPO AS SEGMENTO ,
                            T.DESCRICAO AS NOME_TURNO ,
                            R.CURSOID AS COD_CURSO ,
                            MD.DESCRICAO + ' / ' + TC.DESCRICAO + ' / ' + C.NOME AS MOD_SEG_CURSO,
                            R.DATACADASTRO
                    FROM    DBO.RENOVACAO R ( NOLOCK )
                            INNER JOIN DBO.LY_UNIDADE_ENSINO E ON R.UNIDADEENSINOID = E.UNIDADE_ENS
                            INNER JOIN DBO.LY_CURSO C ON R.CURSOID = C.CURSO
                            INNER JOIN DBO.LY_MODALIDADE_CURSO MD ON C.MODALIDADE = MD.MODALIDADE
                            INNER JOIN DBO.LY_TIPO_CURSO TC ON C.TIPO = TC.TIPO
                            INNER JOIN DBO.LY_TURNO T ON R.TURNOID = T.TURNO
                    WHERE   R.SITUACAORENOVACAOID = 1
                            AND R.ALUNOID = @ALUNO
                    ORDER BY R.ANO DESC ,
                            R.DATACADASTRO ";

                contextQuery.Parameters.Add("@ALUNO", aluno);

                renovacoes = ctx.GetDataTable(contextQuery);
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

            return renovacoes;
        }

        public RenovacaoMatricula.Entidades.Renovacao ObtemRenovacoesMatriculasPor(string aluno, string unidadeEnsino, string curso, int ano, int periodo, string serie, string turno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            RenovacaoMatricula.Entidades.Renovacao renovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Entidades.Renovacao();
            SqlDataReader reader = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                            RENOVACAOID ,
                            R.ALUNOID ,
                            UNIDADEENSINOID ,
                            CURSOID ,
                            TURNOID ,
                            ANO ,
                            PERIODO ,
                            R.SERIE ,
                            SITUACAORENOVACAOID ,
                            USUARIO ,
                            DATAALTERACAO ,
                            TIPOVAGA ,
                            ENSINORELIGIOSO ,
                            LINGUAESTRANGEIRA ,
                            DATACADASTRO
                    FROM    DBO.RENOVACAO R ( NOLOCK )
                    WHERE R.ALUNOID = @ALUNO
                            AND R.UNIDADEENSINOID = @UNIDADEENSINOID
                            AND R.CURSOID = @CURSOID
                            AND R.ANO = @ANO
                            AND R.PERIODO = @PERIODO
                            AND R.SITUACAORENOVACAOID = @SITUACAORENOVACAOID
                            AND R.SERIE = @SERIE
                            AND R.TURNOID = @TURNOID
                    ORDER BY R.RENOVACAOID DESC ,
                            R.DATACADASTRO ";

                contextQuery.Parameters.Add("@UNIDADEENSINOID", unidadeEnsino);
                contextQuery.Parameters.Add("@CURSOID", curso);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@SERIE", serie);
                contextQuery.Parameters.Add("@TURNOID", turno);
                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@SITUACAORENOVACAOID", Convert.ToInt32(RN.RenovacaoMatricula.Entidades.SituacaoRenovacao.Ativo));

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    renovacao.RenovacaoId = Convert.ToInt32(reader["RENOVACAOID"]);
                    renovacao.AlunoId = Convert.ToString(reader["ALUNOID"]);
                    renovacao.UnidadeEnsinoId = Convert.ToString(reader["UNIDADEENSINOID"]);
                    renovacao.CursoId = Convert.ToString(reader["CURSOID"]);
                    renovacao.TurnoId = Convert.ToString(reader["TURNOID"]);
                    renovacao.Ano = Convert.ToInt32(reader["ANO"]);
                    renovacao.Periodo = Convert.ToInt32(reader["PERIODO"]);
                    renovacao.Serie = Convert.ToInt32(reader["SERIE"]);
                    renovacao.SituacaoRenovacaoId = Convert.ToInt32(reader["SITUACAORENOVACAOID"]);
                    renovacao.Usuario = Convert.ToString(reader["USUARIO"]);
                    renovacao.TipoVaga = Convert.ToString(reader["TIPOVAGA"]);
                    renovacao.EnsinoReligioso = Convert.ToBoolean(reader["ENSINORELIGIOSO"]);
                    renovacao.LinguaEstrangeira = Convert.ToBoolean(reader["LINGUAESTRANGEIRA"]);
                    renovacao.DataCadastro = Convert.ToDateTime(reader["DATACADASTRO"]);
                    if (reader["DATAALTERACAO"] != DBNull.Value)
                    {
                        renovacao.DataAlteracao = Convert.ToDateTime(reader["DATAALTERACAO"]);
                    }
                }

                return renovacao;
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
                if (reader != null)
                {
                    reader.Close();
                }
                if (ctx != null)
                    ctx.Dispose();
            }
        }

        public string ObtemTipoBuscaTurnosPor(int ano, int periodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            int tipoEventoTurnoVaga = (int)RN.Agenda.TipoEvento.TipoEventoAgenda.ConfirmacaoTurnosVagas;
            string resultado = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT 
                        CASE   
                          WHEN (DATAINICIO < GETDATE()
                            and e.DATAFIM > GETDATE())  
                            THEN 'TurnosEVagas'
                          WHEN (DATAINICIO < GETDATE()
                            AND e.DATAFIM < GETDATE())  
                            THEN 'ControleDeVagas'
                          ELSE 'PeriodoInvalido'
                        END AS BUSCATURNOS
                        FROM AGENDA.EVENTO E
                        INNER JOIN AGENDA.PERIODOLETIVOAGENDA P ON E.AGENDAID=P.AGENDAID
                        WHERE TIPOEVENTOID = @TIPOEVENTOID
                        AND ANO = @ANO
                        AND PERIODO = @PERIODO ";

                contextQuery.Parameters.Add("@TIPOEVENTOID", tipoEventoTurnoVaga);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                resultado = ctx.GetReturnValue<string>(contextQuery);

                //Caso não seja encontrado evento para o ano / periodo retornar PeriodoInvalido
                if (string.IsNullOrEmpty(resultado))
                {
                    resultado = TipoBuscaPeriodoInvalido;
                }

                return resultado;
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

        public bool PossuiRenovacaoAtivaPor(DataContext ctx, int ano, int periodo, string censo, string curso, string turno, int serie, string tipoVaga)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @"  SELECT  COUNT(*)
                                        FROM    RENOVACAO
                                        WHERE   SITUACAORENOVACAOID IN (1,3)
                                                AND UNIDADEENSINOID = @CENSO
                                                AND ANO = @ANO
                                                AND PERIODO = @PERIODO
                                                AND CURSOID = @CURSO
                                                AND SERIE = @SERIE
                                                AND TURNOID = @TURNO
                                                AND TIPOVAGA = @TIPOVAGA ";

            contextQuery.Parameters.Add("@CENSO", censo);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@CURSO", curso);
            contextQuery.Parameters.Add("@SERIE", serie);
            contextQuery.Parameters.Add("@TURNO", turno);
            contextQuery.Parameters.Add("@TIPOVAGA", tipoVaga);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }



        public void CancelaRenovacaoPor(DataContext ctx, string aluno, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE  [LYCEUM].[DBO].[RENOVACAO]
                    SET     [SITUACAORENOVACAOID] = 2 ,
                            [USUARIO] = @USUARIO ,
                            [DATAALTERACAO] = GETDATE()
                    WHERE   ALUNOID = @ALUNOID 
                            AND SITUACAORENOVACAOID = 1";

                contextQuery.Parameters.Add("@ALUNOID", aluno);
                contextQuery.Parameters.Add("@USUARIO", usuario);

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

        public DataTable ObtemListaRenovacaoAtivaPor(int ano,int periodo,string censo, string alunos)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable listaAlunosNomes = null;
            string possiveisPeriodos = Utils.RecuperaPossiveisFuturosPeriodos(Convert.ToInt32(periodo));

            try
            {
                contextQuery.Command = string.Format(@"  SELECT DISTINCT R.ALUNOID, P.NOME_COMPL
                                        FROM    RENOVACAO R
                                        INNER JOIN LY_ALUNO A ON R.ALUNOID=A.ALUNO
                                        INNER JOIN LY_PESSOA P ON P.PESSOA = A.PESSOA
                                        WHERE   SITUACAORENOVACAOID = 3 
                                                AND R.UNIDADEENSINOID = @CENSO
                                                AND ANO = @ANO
                                                AND PERIODO in ({0})
                                                AND R.ALUNOID IN (" + alunos + ")", possiveisPeriodos);                                             
                                               



                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@CENSO", censo);


                listaAlunosNomes = ctx.GetDataTable(contextQuery);            }

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

            return listaAlunosNomes;
        }


        public DataTable ObtemAlunoRenovacaoPor(int ano,int periodo, string alunos)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable listaAlunosNomes = null;
            string possiveisPeriodos = Utils.RecuperaPossiveisFuturosPeriodos(Convert.ToInt32(periodo));

            try
            {
                contextQuery.Command = string.Format(@"  SELECT DISTINCT A.ALUNO, P.NOME_COMPL , CASE WHEN R.RENOVACAOID IS NULL THEN 'N' ELSE 'S' END RENOVACAO,A.UNIDADE_ENSINO,R.UNIDADEENSINOID
                                           FROM  LY_ALUNO  A
                                        LEFT JOIN RENOVACAO R  ON R.ALUNOID=A.ALUNO AND SITUACAORENOVACAOID = 3 AND ANO = @ANO AND PERIODO IN ({0})
                                        INNER JOIN LY_PESSOA P ON P.PESSOA = A.PESSOA
                                        WHERE A.ALUNO IN (" + alunos + ")" , possiveisPeriodos);



                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                listaAlunosNomes = ctx.GetDataTable(contextQuery);
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

            return listaAlunosNomes;
        }

    }
}
