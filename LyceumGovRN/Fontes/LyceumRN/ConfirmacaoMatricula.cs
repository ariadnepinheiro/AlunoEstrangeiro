using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.DTOs;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN
{
    public class ConfirmacaoMatricula : RNBase
    {
        public const string Confirmado = "Confirmado";
        public const string Pendente = null;
        public const string NaoConfirmado = "Não Confirmado";

        #region Propriedades e Enum
        public enum SituacaoRenovacao
        {
            [StringValue("Ativo")]
            Ativo = 1,
            [StringValue("Cancelado")]
            Cancelado = 2,
            [StringValue("Possui confirmação")]
            PossuiConfirmacao = 3
        }
        #endregion

        public class Observacao
        {
            public const string CanceladaPorTransferenciaDeAluno = "CANCELADA POR TRANSFERENCIA DO ALUNO";
            public const string CanceladaPorReprovacaoDeAluno = "CANCELADA POR REPROVAÇÃO DO ALUNO";
        }

        public static DataTable Listar(string user, string matricula)
        {
            var contextQuery = new ContextQuery(string.Format(@"DECLARE @USUARIO VARCHAR(15) = @USER, @PRIVILEGIO CHAR(1) 

                                                                SELECT @PRIVILEGIO = U.privil 
                                                                FROM   hades..USUARIO U (nolock) 
                                                                WHERE  USUARIO = @USUARIO 

                                                                SELECT U.usuario, UUF.unidade_fis 
                                                                INTO   #usuario 
                                                                FROM   hades..USUARIO U (nolock) 
                                                                       LEFT JOIN LY_USUARIO_UNIDADE_FIS UUF (nolock) ON U.usuario = UUF.usuario 
                                                                WHERE  U.usuario = @USUARIO 

                                                        IF ( @PRIVILEGIO = 'S' ) 
                                                          
                                                            BEGIN 
                                                              SELECT DISTINCT CM.ID_CONFIRMACAO_MATRICULA, 
                                                                              CM.ALUNO, 
                                                                              CM.CENSO, 
                                                                              CM.ANO, 
                                                                              CM.PERIODO, 
                                                                              C.NOME AS CURSO, 
                                                                              CM.SERIE, 
                                                                              CM.TURNO, 
                                                                              CM.DT_SUGERIDA, 
                                                                              CM.ENSINO_RELIGIOSO, 
                                                                              CM.LINGUA_ESTRANGEIRA_FACULTATIVA, 
                                                                              CM.PROJETO_AUTONOMIA, 
                                                                              CM.STATUS, 
                                                                              CM.MATRICULA, 
                                                                              CM.DT_CADASTRO, 
                                                                              CM.DT_ALTERACAO, 
                                                                              E.NOME_COMP AS 
                                                                              ESCOLA, 
                                                                              CM.CENSO + ' - ' + E.NOME_COMP AS UNIDADE_ENSINO, 
                                                                              MD.MODALIDADE, 
                                                                              CM.CURRICULO, 
                                                                              TC.TIPO AS SEGMENTO, 
                                                                              T.DESCRICAO 
                                                                              AS NOME_TURNO, 
                                                                              CM.CURSO AS COD_CURSO, 
                                                                              MD.DESCRICAO + ' / ' + TC.DESCRICAO + ' / ' + C.NOME AS MOD_SEG_CURSO, 
                                                                              TIPOVAGAOCUPADA, 
                                                                              ISNULL(CR.ENSINO_RELIGIOSO, 'N') AS PODE_ENSINO_RELIGIOSO, 
                                                                              ISNULL(CR.LINGUA_ESTRANGEIRA, 'N') AS PODE_LINGUA_ESTRANGEIRA, 
                                                                              CASE WHEN CM.STATUS IS NULL THEN 'N'
                                                                                   ELSE 'S'
                                                                              END CADASTROU
                                                              FROM   dbo.TCE_CONFIRMACAO_MATRICULA CM (nolock) 
                                                                     INNER JOIN dbo.LY_UNIDADE_ENSINO E ON CM.censo = E.UNIDADE_ENS 
                                                                     INNER JOIN dbo.LY_CURSO C ON CM.curso = C.CURSO 
                                                                     INNER JOIN dbo.LY_MODALIDADE_CURSO MD ON C.MODALIDADE = MD.MODALIDADE 
                                                                     INNER JOIN dbo.LY_TIPO_CURSO TC ON C.TIPO = TC.TIPO 
                                                                     INNER JOIN dbo.LY_TURNO t ON CM.turno = t.TURNO 
                                                                     LEFT JOIN dbo.LY_CURRICULO cr ON CM.curso = cr.CURSO AND CM.turno = cr.TURNO AND CM.curriculo = cr.CURRICULO 
                                                              WHERE  ALUNO = @MATRICULA
                                                              ORDER  BY CM.ano DESC, CM.dt_cadastro 
                                                          END 
                                                        
                                                        ELSE 
                                                          
                                                            BEGIN 
                                                              SELECT DISTINCT CM.ID_CONFIRMACAO_MATRICULA, 
                                                                              CM.ALUNO, 
                                                                              CM.CENSO, 
                                                                              CM.ANO, 
                                                                              CM.PERIODO, 
                                                                              C.NOME AS CURSO, 
                                                                              CM.SERIE, 
                                                                              CM.TURNO, 
                                                                              CM.DT_SUGERIDA, 
                                                                              CM.ENSINO_RELIGIOSO, 
                                                                              CM.LINGUA_ESTRANGEIRA_FACULTATIVA, 
                                                                              CM.PROJETO_AUTONOMIA, 
                                                                              CM.STATUS, 
                                                                              CM.MATRICULA, 
                                                                              CM.DT_CADASTRO, 
                                                                              CM.DT_ALTERACAO, 
                                                                              E.NOME_COMP AS ESCOLA, 
                                                                              CM.CENSO + ' - ' + E.NOME_COMP AS UNIDADE_ENSINO, 
                                                                              MD.MODALIDADE, 
                                                                              CM.CURRICULO, 
                                                                              TC.TIPO AS SEGMENTO, 
                                                                              T.DESCRICAO AS NOME_TURNO, 
                                                                              CM.CURSO AS COD_CURSO, 
                                                                              MD.DESCRICAO + ' / ' + TC.DESCRICAO + ' / ' + C.NOME AS MOD_SEG_CURSO, 
                                                                              TIPOVAGAOCUPADA, 
                                                                              ISNULL(CR.ENSINO_RELIGIOSO, 'N') AS PODE_ENSINO_RELIGIOSO, 
                                                                              ISNULL(CR.LINGUA_ESTRANGEIRA, 'N') AS PODE_LINGUA_ESTRANGEIRA, 
                                                                              CASE WHEN CM.STATUS IS NULL THEN 'N'
                                                                                   ELSE 'S'
                                                                              END CADASTROU
                                                              FROM   dbo.TCE_CONFIRMACAO_MATRICULA CM (nolock) 
                                                                     INNER JOIN dbo.LY_UNIDADE_ENSINO E ON CM.censo = E.UNIDADE_ENS 
                                                                     INNER JOIN dbo.LY_CURSO C ON CM.curso = C.CURSO 
                                                                     INNER JOIN dbo.LY_MODALIDADE_CURSO MD ON C.MODALIDADE = MD.MODALIDADE 
                                                                     INNER JOIN dbo.LY_TIPO_CURSO TC ON C.TIPO = TC.TIPO 
                                                                     INNER JOIN dbo.LY_TURNO t ON CM.turno = t.TURNO 
                                                                     JOIN #usuario U (nolock) ON U.unidade_fis = E.UNIDADE_ENS 
                                                                     LEFT JOIN dbo.LY_CURRICULO cr ON CM.curso = cr.CURSO AND CM.turno = cr.TURNO AND CM.curriculo = cr.CURRICULO 
                                                              WHERE  ALUNO = @MATRICULA
                                                              ORDER  BY CM.ano DESC, CM.dt_cadastro 
                                                          END 

                                                        DROP TABLE #usuario"));

            contextQuery.Parameters.Add("@MATRICULA", matricula);
            contextQuery.Parameters.Add("@USER", user);

            return Consultar(contextQuery);
        }

        public void AtualizaSuspensao(DataContext contexto, int ano, int historicoSuspensaoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE CM
                                        SET STATUS = 'Não Confirmado',
                                        OBSERVACAO = 'ALUNO ENCERRADO EM ' + CONVERT(VARCHAR,GETDATE(),103) + ' ÀS ' +  CONVERT(VARCHAR,GETDATE(),108) + ' PELO MOTIVO SUSPENSÃO',
	                                    DT_ALTERACAO = GETDATE()
                                    from TCE_CONFIRMACAO_MATRICULA CM
	                                    INNER JOIN Turma.HISTORICOSUSPENSAO h ON h.ALUNO = Cm.ALUNO
                                     WHERE CM.ANO = @ANO
                                        AND (STATUS = 'CONFIRMADO' OR STATUS IS NULL) 
	                                    and HISTORICOSUSPENSAOID = @HISTORICOSUSPENSAOID  ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@HISTORICOSUSPENSAOID", SqlDbType.Int, historicoSuspensaoId);

            contexto.ApplyModifications(contextQuery);
        }

        public DataTable ListaConfirmacoesParaLiberacaoPor(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable confirmacoes = null;

            try
            {
                //Lista confirmaçoes de periodos com data fim maior que data atual
                contextQuery.Command = @" SELECT DISTINCT
                            CM.ID_CONFIRMACAO_MATRICULA ,
                            CM.ANO ,
                            CM.PERIODO ,
                            CM.CENSO + ' - ' + E.NOME_COMP AS UNIDADE_ENSINO ,
                            MD.DESCRICAO + ' / ' + TC.DESCRICAO + ' / ' + C.NOME AS MOD_SEG_CURSO ,
                            CM.SERIE ,
                            CM.TURNO ,
                            CM.DT_SUGERIDA ,
                            CM.ENSINO_RELIGIOSO ,
                            CM.LINGUA_ESTRANGEIRA_FACULTATIVA ,
                            CM.PROJETO_AUTONOMIA ,
                            CASE WHEN CM.STATUS IS NULL THEN 'Pendente'
                                 ELSE CM.STATUS
                            END STATUS ,
                            CM.DT_ALTERACAO
                    FROM    dbo.TCE_CONFIRMACAO_MATRICULA CM
                            INNER JOIN dbo.LY_PERIODO_LETIVO sl ON CM.ANO = sl.ANO
                                                                      AND CM.PERIODO = sl.PERIODO
                            INNER JOIN dbo.LY_UNIDADE_ENSINO E ON CM.CENSO = E.UNIDADE_ENS
                            INNER JOIN dbo.LY_CURSO C ON CM.CURSO = C.CURSO
                            INNER JOIN dbo.LY_MODALIDADE_CURSO MD ON C.MODALIDADE = MD.MODALIDADE
                            INNER JOIN dbo.LY_TIPO_CURSO TC ON C.TIPO = TC.TIPO
                    WHERE   ALUNO = @ALUNO
                            AND SL.DT_FIM >= GETDATE()
                    ORDER BY ID_CONFIRMACAO_MATRICULA DESC ";

                contextQuery.Parameters.Add("@ALUNO", aluno);

                confirmacoes = ctx.GetDataTable(contextQuery);
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

            return confirmacoes;
        }

        public bool ExisteConfirmacaoPendenteConfirmadoSemPermissaoAcesso(string aluno, string usuarioResponsavel)
        {
            RN.UsuarioUnidadeFis rnUsuarioUnidadeFis = new UsuarioUnidadeFis();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            bool retorno = false;
            string censo = string.Empty;

            try
            {
                //Verifica se existe confirmação pendente 
                if (this.ExisteConfirmacaoPendentePor(contexto, aluno))
                {
                    retorno = true;
                }
                else
                {
                    //Busca censo da ultima confirmacao confirmada do aluno do ano atual ou proximos
                    censo = this.ObtemUltimoCensoConfirmadoPor(contexto, aluno, DateTime.Now.Year);

                    //Verifica se usuario tem permissão naquela escola
                    if (!rnUsuarioUnidadeFis.PossuiPermissaoPor(contexto, usuarioResponsavel, censo) && !censo.IsNullOrEmptyOrWhiteSpace())
                    {
                        retorno = true;
                    }
                }

                return retorno;
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

        public bool ExisteConfirmacaoConfirmadaPor(DataContext contexto, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM TCE_CONFIRMACAO_MATRICULA (NOLOCK)
                                WHERE STATUS = @STATUS
	                                AND ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@STATUS", SqlDbType.VarChar, Confirmado);
            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public void Remove(DataContext ctx, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE FROM TCE_CONFIRMACAO_MATRICULA 
                                        WHERE  ALUNO = @ALUNO  ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);

            ctx.ApplyModifications(contextQuery);
        }

        private bool ExisteConfirmacaoPendentePor(DataContext contexto, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM TCE_CONFIRMACAO_MATRICULA (NOLOCK)
                                WHERE STATUS IS NULL
	                                AND ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        private string ObtemUltimoCensoConfirmadoPor(DataContext contexto, string aluno, int ano)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT TOP 1 CENSO
                            FROM TCE_CONFIRMACAO_MATRICULA (NOLOCK)
                            WHERE STATUS = @STATUS
	                            AND ALUNO = @ALUNO
                                AND ANO >= @ANO
                            ORDER BY DT_ALTERACAO DESC ";

            contextQuery.Parameters.Add("@STATUS", SqlDbType.VarChar, Confirmado);
            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@ano", SqlDbType.Int, ano);
            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        private static void Alterar(TceConfirmacaoMatricula confirmacaoMatricula, DataContext context)
        {
            var contextQuery = new ContextQuery(
                @"UPDATE  TCE_CONFIRMACAO_MATRICULA
                    SET    ENSINO_RELIGIOSO = @ENSINO_RELIGIOSO,
                            LINGUA_ESTRANGEIRA_FACULTATIVA = @LINGUA_ESTRANGEIRA_FACULTATIVA,
                            PROJETO_AUTONOMIA = @PROJETO_AUTONOMIA, 
                            STATUS = @STATUS,
                            MATRICULA = @MATRICULA, 
                            DT_ALTERACAO = GETDATE()
                    WHERE   ID_CONFIRMACAO_MATRICULA = @ID_CONFIRMACAO_MATRICULA ");

            contextQuery.Parameters.Add("@ID_CONFIRMACAO_MATRICULA", confirmacaoMatricula.IdConfirmacaoMatricula);
            contextQuery.Parameters.Add("@ENSINO_RELIGIOSO", confirmacaoMatricula.EnsinoReligioso);
            contextQuery.Parameters.Add("@LINGUA_ESTRANGEIRA_FACULTATIVA", confirmacaoMatricula.LinguaEstrangeiraFacultativa);
            contextQuery.Parameters.Add("@PROJETO_AUTONOMIA", confirmacaoMatricula.ProjetoAutonomia);
            contextQuery.Parameters.Add("@STATUS", confirmacaoMatricula.Status);
            contextQuery.Parameters.Add("@MATRICULA", confirmacaoMatricula.Matricula);

            context.ApplyModifications(contextQuery);
        }

        public static void Inserir(TceConfirmacaoMatricula confirmacaoMatricula)
        {
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingLock();
                Inserir(confirmacaoMatricula, contexto);
            }
            catch (Exception ex)
            {
                if (contexto != null)
                    contexto.Abandon();

                throw ex;
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }
        }

        public static void Inserir(TceConfirmacaoMatricula confirmacaoMatricula, DataContext context)
        {
            ContextQuery contextQuery = InsereContextQuery(confirmacaoMatricula);
            context.ApplyModifications(contextQuery);
        }

        public void Inserir(TceConfirmacaoMatricula confirmacaoMatricula, List<ContextQuery> listaContextQuery)
        {
            ContextQuery contextQuery = InsereContextQuery(confirmacaoMatricula);
            listaContextQuery.Add(contextQuery);
        }

        private static ContextQuery InsereContextQuery(TceConfirmacaoMatricula confirmacaoMatricula)
        {
            var contextQuery = new ContextQuery(
                @" INSERT INTO TCE_CONFIRMACAO_MATRICULA ( 
                       ALUNO, 
                       CENSO, 
                       ANO, 
                       PERIODO, 
                       CURSO,
                       SERIE, 
                       TURNO, 
                       DT_CADASTRO,
                       DT_SUGERIDA, 
                       DT_ALTERACAO,
                       STATUS,
                       CURRICULO,
                       ENSINO_RELIGIOSO, 
                       LINGUA_ESTRANGEIRA_FACULTATIVA, 
                       PROJETO_AUTONOMIA, 
                       MATRICULA, 
                       TIPOVAGAOCUPADA
                   ) VALUES (
                       @ALUNO, 
                       @CENSO, 
                       @ANO, 
                       @PERIODO, 
                       @CURSO, 
                       @SERIE, 
                       @TURNO, 
                       GetDate(),
                       GetDate(), 
                       GetDate(),
                       @STATUS,
                       @CURRICULO,
                       @ENSINO_RELIGIOSO, 
                       @LINGUA_ESTRANGEIRA_FACULTATIVA, 
                       @PROJETO_AUTONOMIA, 
                       @MATRICULA, 
                       @TIPOVAGAOCUPADA
                   ) ");

            contextQuery.Parameters.Add("@ALUNO", confirmacaoMatricula.Aluno);
            contextQuery.Parameters.Add("@CENSO", confirmacaoMatricula.Censo);
            contextQuery.Parameters.Add("@ANO", confirmacaoMatricula.Ano);
            contextQuery.Parameters.Add("@PERIODO", confirmacaoMatricula.Periodo);
            contextQuery.Parameters.Add("@CURSO", confirmacaoMatricula.Curso);
            contextQuery.Parameters.Add("@SERIE", confirmacaoMatricula.Serie);
            contextQuery.Parameters.Add("@TURNO", confirmacaoMatricula.Turno);
            contextQuery.Parameters.Add("@STATUS", confirmacaoMatricula.Status);
            contextQuery.Parameters.Add("@ENSINO_RELIGIOSO", confirmacaoMatricula.EnsinoReligioso);
            contextQuery.Parameters.Add("@LINGUA_ESTRANGEIRA_FACULTATIVA", confirmacaoMatricula.LinguaEstrangeiraFacultativa);
            contextQuery.Parameters.Add("@PROJETO_AUTONOMIA", confirmacaoMatricula.ProjetoAutonomia);
            contextQuery.Parameters.Add("@MATRICULA", confirmacaoMatricula.Matricula);
            contextQuery.Parameters.Add("@CURRICULO", confirmacaoMatricula.Curriculo);
            contextQuery.Parameters.Add("@TIPOVAGAOCUPADA", confirmacaoMatricula.TipoVagaOcupada);

            return contextQuery;
        }

        public ValidacaoDados ValidaConfirmacaoTelaAluno(TceConfirmacaoMatricula confirmacaoMatricula)
        {
            DataContext contexto = null;
            RN.Aluno rnAluno = new Aluno();
            RN.ControleVaga rnControleVaga = new ControleVaga();
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (confirmacaoMatricula == null)
            {
                return validacaoDados;
            }

            if (confirmacaoMatricula.IdConfirmacaoMatricula <= 0)
            {
                mensagens.Add("O campo CODIGO é obrigatório!");
            }

            if (string.IsNullOrEmpty(confirmacaoMatricula.Aluno))
            {
                mensagens.Add("O campo ALUNO é obrigatório!");
            }

            if (string.IsNullOrEmpty(confirmacaoMatricula.Censo))
            {
                mensagens.Add("O campo CENSO é obrigatório!");
            }

            if (confirmacaoMatricula.Ano <= 0)
            {
                mensagens.Add("O campo ANO é obrigatório!");
            }

            if (string.IsNullOrEmpty(confirmacaoMatricula.Curso))
            {
                mensagens.Add("O campo CURSO é obrigatório!");
            }

            if (confirmacaoMatricula.Serie <= 0)
            {
                mensagens.Add("O campo SERIE é obrigatório!");
            }

            if (string.IsNullOrEmpty(confirmacaoMatricula.Turno))
            {
                mensagens.Add("O campo TURNO é obrigatório!");
            }

            if (string.IsNullOrEmpty(confirmacaoMatricula.Status))
            {
                mensagens.Add("O campo STATUS é obrigatório!");
            }
            else if (confirmacaoMatricula.Status != Confirmado && confirmacaoMatricula.Status != NaoConfirmado)
            {
                mensagens.Add("STATUS inválido!");
            }

            if (string.IsNullOrEmpty(confirmacaoMatricula.Matricula))
            {
                mensagens.Add("O campo MATRICULA é obrigatório!");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (!rnAluno.EhAlunoAtivoPor(contexto, confirmacaoMatricula.Aluno))
                    {
                        mensagens.Add("A confirmação não foi possível, pois o aluno não está ativo.");
                    }

                    //Verifica se o status é confirmado, para nao confirmados não é necessario validar vagas
                    if (confirmacaoMatricula.Status == Confirmado)
                    {
                        //Valida Vaga unica                       
                        int vagasLiberadas = 0;
                        int vagasUtilizadas = 0;

                        //Verificar se tem vaga no curso / serie / turno / ano / semestre
                        vagasLiberadas = rnControleVaga.ObtemVagasLiberadasTotalPor(contexto,
                            confirmacaoMatricula.Censo,
                            Convert.ToInt32(confirmacaoMatricula.Ano),
                            Convert.ToInt32(confirmacaoMatricula.Periodo),
                            Convert.ToInt32(confirmacaoMatricula.Serie),
                            confirmacaoMatricula.Curso,
                            confirmacaoMatricula.Turno);

                        vagasUtilizadas = rnControleVaga.ObtemVagasUtilizadasTotalPor(contexto,
                            confirmacaoMatricula.Censo,
                            Convert.ToInt32(confirmacaoMatricula.Ano),
                            Convert.ToInt32(confirmacaoMatricula.Periodo),
                            Convert.ToInt32(confirmacaoMatricula.Serie),
                            confirmacaoMatricula.Curso,
                            confirmacaoMatricula.Turno);

                        if (vagasLiberadas <= vagasUtilizadas)
                        {
                            mensagens.Add("Não será possível realizar a confirmação, pois não existem vagas disponíveis para o ANO / PERIODO / CURSO / SÉRIE / TURNO informados!");
                        }

                        //Caso a confirmação nao seja vindo do matricula facil (tem prioridade)
                        if (!this.EhConfirmacaoMatriculaFacilPor(contexto, confirmacaoMatricula.IdConfirmacaoMatricula))
                        {
                            //Verifica se já existe outra confirmada
                            if (this.PossuiConfirmacaoMatriculaConfirmadaEmPossiveisPeriodosPor(contexto, confirmacaoMatricula.Aluno, Convert.ToInt32(confirmacaoMatricula.Ano), Convert.ToInt32(confirmacaoMatricula.Periodo)))
                            {
                                mensagens.Add("Já existe um registro confirmado para este aluno/ano.");
                            }

                            //Valida se existe uma confiramão de matricula facil pendente
                            if (this.PossuiConfirmacaoMatriculaFacilPendentePor(confirmacaoMatricula.Aluno, confirmacaoMatricula.Ano, confirmacaoMatricula.Periodo))
                            {
                                mensagens.Add("Não será possível realizar a confirmação, pois existe outro registro de confirmação prioritário pendente.");
                            }
                        }
                    }

                    if (!this.EhConfirmacaoPendentePor(contexto, confirmacaoMatricula.IdConfirmacaoMatricula))
                    {
                        mensagens.Add("Não será possível realizar a operação, pois esta confirmação não se encontra mais pendente!");
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

        public ValidacaoDados ValidaDisciplinasOptativas(int idConfirmacaoMatricula, bool ensinoReligioso, bool linguaEstrangeiraFacultativa, string usuarioResponsavel)
        {
            List<string> mensagens = new List<string>();
            RN.Curriculo rnCurriculo = new Curriculo();
            DataContext contexto = null;
            RN.PeriodoLetivo rnPeriodoLetivo = new PeriodoLetivo();
            RN.Matricula rnMatricula = new Matricula();
            TceConfirmacaoMatricula confirmacaoMatricula = new TceConfirmacaoMatricula();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (idConfirmacaoMatricula <= 0)
            {
                mensagens.Add("Favor informar o CODIGO.");
            }

            if (usuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Favor informar o USUÁRIO.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Busca dados da confirmacao
                    confirmacaoMatricula = this.ObtemConfirmacaoMatriculaPor(contexto, idConfirmacaoMatricula);

                    //Verifica se esta confirmada
                    if (confirmacaoMatricula.Status != Confirmado)
                    {
                        mensagens.Add("Apenas podem sera alteradas confirmações CONFIRMADAS.");
                    }
                    else
                    {
                        //Verifica se é de um ano / periodo ativo
                        if (!rnPeriodoLetivo.EhAnoPeriodoAtivoPor(contexto, Convert.ToInt32(confirmacaoMatricula.Ano), Convert.ToInt32(confirmacaoMatricula.Periodo), DateTime.Now))
                        {
                            mensagens.Add("Apenas podem sera alteradas confirmações de anos / periodos abertos.");
                        }
                    }

                    //Verifica se possui enturmacao em uma opção que esta sendo desmarcada
                    if (!ensinoReligioso)
                    {
                        if (rnMatricula.PossuiMatriculaOptativaEnsinoReligioso(contexto, confirmacaoMatricula.Aluno))
                        {
                            mensagens.Add("A opção ENSINO RELIGIOSO não pode ser desmarcada pois o aluno encontra-se enturmado com esta oção.");
                        }
                    }

                    if (!linguaEstrangeiraFacultativa)
                    {
                        if (rnMatricula.PossuiMatriculaOptativaLinguaEstrangeira(contexto, confirmacaoMatricula.Aluno))
                        {
                            mensagens.Add("A opção LINGUA ESTRANGEIRA FACULTATIVA não pode ser desmarcada pois o aluno encontra-se enturmado com esta oção.");
                        }
                    }

                    //Busca dados do curriculo
                    DataTable dtCurriculo = rnCurriculo.ObtemCurriculoPor(contexto, idConfirmacaoMatricula);

                    if (dtCurriculo.Rows.Count > 0)
                    {
                        //Verifica se curriculo permite
                        if (dtCurriculo.Rows[0]["PODE_LINGUA_ESTRANGEIRA"].ToString() != "S" && linguaEstrangeiraFacultativa)
                        {
                            mensagens.Add("O curriculo desta confirmação não permite a opção LINGUA ESTRANGEIRA FACULTATIVA.");
                        }

                        //Verifica se curriculo permite
                        if (dtCurriculo.Rows[0]["PODE_ENSINO_RELIGIOSO"].ToString() != "S" && ensinoReligioso)
                        {
                            mensagens.Add("O curriculo desta confirmação não permite a opção ENSINO RELIGIOSO.");
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

        public void AtualizaDisciplinasOptativas(int idConfirmacaoMatricula, bool ensinoReligioso, bool linguaEstrangeiraFacultativa, string usuarioResponsavel)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE  TCE_CONFIRMACAO_MATRICULA
                    SET     MATRICULA = @MATRICULA , 
                            ENSINO_RELIGIOSO = @ENSINO_RELIGIOSO ,
                            LINGUA_ESTRANGEIRA_FACULTATIVA = @LINGUA_ESTRANGEIRA_FACULTATIVA ,                            
                            DT_ALTERACAO = GETDATE()
                    WHERE   ID_CONFIRMACAO_MATRICULA = @ID_CONFIRMACAO_MATRICULA ";

                contextQuery.Parameters.Add("@ID_CONFIRMACAO_MATRICULA", SqlDbType.Int, idConfirmacaoMatricula);
                contextQuery.Parameters.Add("@ENSINO_RELIGIOSO", SqlDbType.Bit, ensinoReligioso);
                contextQuery.Parameters.Add("@LINGUA_ESTRANGEIRA_FACULTATIVA", SqlDbType.Bit, linguaEstrangeiraFacultativa);
                contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, usuarioResponsavel);

                contexto.ApplyModifications(contextQuery);
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

        public ValidacaoDados Valida(TceConfirmacaoMatricula confirmacaoMatricula)
        {
            List<string> mensagens = new List<string>();
            RN.Curriculo rnCurriculo = new Curriculo();
            RN.Matricula rnMatricula = new Matricula();
            RN.Aluno rnAluno = new Aluno();
            RN.PadroesDeAcessos rnPadroesDeAcesso = new PadroesDeAcessos();
            Pedagogico.Entidades.PeriodoConfirmacao periodoConfirmacao = new Pedagogico.Entidades.PeriodoConfirmacao();
            Pedagogico.PeriodoConfirmacao rnPeriodoConfirmacao = new Pedagogico.PeriodoConfirmacao();
            RN.RenovacaoMatricula.Renovacao rnRenovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Renovacao();
            RN.SituacaoFinalAluno rnSituacaoFinalAluno = new SituacaoFinalAluno();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (confirmacaoMatricula == null)
            {
                return validacaoDados;
            }

            if (confirmacaoMatricula.Aluno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Favor informar o ALUNO.");
            }

            if (confirmacaoMatricula.Ano <= 0)
            {
                mensagens.Add("Favor informar o ANO.");
            }

            if (confirmacaoMatricula.Periodo < 0)
            {
                mensagens.Add("Favor informar o PERIODO.");
            }

            if (confirmacaoMatricula.Censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Favor informar o CENSO.");
            }

            if (confirmacaoMatricula.Curso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Favor informar o CURSO.");
            }

            if (confirmacaoMatricula.Curriculo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Não existe MATRIZ CURRICULAR para as informações selecionadas.");
            }

            if (confirmacaoMatricula.Matricula.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Favor informar o USUARIO.");
            }

            if (confirmacaoMatricula.Turno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Favor informar o TURNO.");
            }

            if (confirmacaoMatricula.Serie < 0)
            {
                mensagens.Add("Favor informar o SERIE.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Monta Demais informaçoes
                    confirmacaoMatricula.ProjetoAutonomia = false;
                    confirmacaoMatricula.Status = null;
                    confirmacaoMatricula.Observacao = null;
                    confirmacaoMatricula.TipoVagaOcupada = "VC";

                    //Verifica se o aluno está ativo
                    if (!rnAluno.EhAlunoAtivoPor(contexto, confirmacaoMatricula.Aluno))
                    {
                        mensagens.Add("Este aluno não está ativo.");
                    }

                    //Verifica se ja existe confirmacao igual
                    if (this.ExisteConfirmacaoPor(contexto, confirmacaoMatricula.Aluno, Convert.ToInt32(confirmacaoMatricula.Ano), Convert.ToInt32(confirmacaoMatricula.Periodo), confirmacaoMatricula.Censo, confirmacaoMatricula.Curso, Convert.ToInt32(confirmacaoMatricula.Serie), confirmacaoMatricula.Turno))
                    {
                        mensagens.Add("Já existe uma confirmação de matricula cadastrada com estes dados.");
                    }

                    //Verifica se aluno possui renovação (Ativa ou com confrimação) para o censo, ano e possiveis periodos
                    if (!rnRenovacao.PossuiRenovacaoAtivaConfirmadaPor(contexto, confirmacaoMatricula.Aluno, confirmacaoMatricula.Censo, Convert.ToInt32(confirmacaoMatricula.Ano), Convert.ToInt32(confirmacaoMatricula.Periodo)))
                    {
                        //Verifica se não eh aluno concluinte (para concluintes pode gerar sem necessidade de renovação)
                        if (!rnAluno.EhalunoRegularConcluinte(confirmacaoMatricula.Aluno))
                        {
                            mensagens.Add("Este aluno não possui renovação para a escola atual no ano / periodo informado.");
                        }
                    }

                    //Verifica se tem matricula ativa
                    if (rnMatricula.PossuiMatriculaAtivaPossiveisPeriodosPor(contexto, confirmacaoMatricula.Aluno, confirmacaoMatricula.Ano, confirmacaoMatricula.Periodo))
                    {
                        mensagens.Add("Este aluno possui matricula ativa para outra confirmação de matricula. Favor realizar antes a liberação do registro de confirmação.");
                    }

                    //Verifica se é diretor
                    if (rnPadroesDeAcesso.PossuiPadraoDiretorPor(contexto, confirmacaoMatricula.Matricula))
                    {
                        //Caso seja diretor precisa respeitar o periodo de confirmacao                        

                        //Busca periodo de confirmacao 
                        periodoConfirmacao = rnPeriodoConfirmacao.ObtemPor(contexto, Convert.ToInt32(confirmacaoMatricula.Ano), Convert.ToInt32(confirmacaoMatricula.Periodo));

                        if (periodoConfirmacao != null && periodoConfirmacao.PeriodoConfirmacaoId > 0)
                        {
                            //Verifica se, para o ano / periodo, a data atual esta entre o inicio e fim permitido 
                            if (DateTime.Now.Date >= periodoConfirmacao.DataInicio.Date && DateTime.Now.Date <= periodoConfirmacao.DataFim.Date)
                            { 
                                //Verifica se o aluno foi reprovado ou retido no ano/periodo anterior em um dos cursos / series permitidos
                                if (!rnSituacaoFinalAluno.EhReprovadoPor(contexto, confirmacaoMatricula.Aluno, Convert.ToInt32(confirmacaoMatricula.Ano), Convert.ToInt32(confirmacaoMatricula.Periodo), periodoConfirmacao.PeriodoConfirmacaoId))
                                {
                                    mensagens.Add("Este aluno não foi REPROVADO em curso / série permitido para criação de registro de confirmação de Matricula.");
                                }

                                //Verifica se o aluno já possui confirmacao de matricula vindo do matricula facil, caso possua não será possível criar 
                                //a linha da nova confirmação. O sistema deverá mostrar mensagem crítica: Existe confirmação pendente para o aluno em Unidade Escolar
                                if (this.PossuiConfirmacaoMatriculaFacilPendenteConfirmadaPor(contexto, confirmacaoMatricula.Aluno, Convert.ToInt32(confirmacaoMatricula.Ano), Convert.ToInt32(confirmacaoMatricula.Periodo)))
                                {
                                    mensagens.Add("Existe confirmação pendente para o aluno em Unidade Escolar.");
                                }
                            }
                            else
                            {
                                mensagens.Add(string.Format("O DIRETOR apenas pode criar registro de confirmação de Matricula para o ano {0} periodo {1} entre os dias {2} à {3}.",
                                    confirmacaoMatricula.Ano.ToString(),
                                    confirmacaoMatricula.Periodo.ToString(),
                                    periodoConfirmacao.DataInicio.ToString("dd/MM/yyyy"),
                                    periodoConfirmacao.DataFim.ToString("dd/MM/yyyy")));
                            }
                        }
                        else
                        {
                            mensagens.Add(string.Format("Não existe periodo liberado para o DIRETOR criar registro de confirmação de Matricula para o ano {0} periodo {1}.", confirmacaoMatricula.Ano.ToString(), confirmacaoMatricula.Periodo.ToString()));
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

        public void Insere(TceConfirmacaoMatricula confirmacaoMatricula)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            try
            {
                //Cancela outras nos possiveis periodos
                this.CancelaPossiveisConfirmacoesPor(contexto, confirmacaoMatricula.Aluno, Convert.ToInt32(confirmacaoMatricula.Ano), Convert.ToInt32(confirmacaoMatricula.Periodo), confirmacaoMatricula.Matricula);

                //Insere nova
                this.Insere(contexto, confirmacaoMatricula);
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

        private void Insere(DataContext contexto, TceConfirmacaoMatricula confirmacaoMatricula)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO TCE_CONFIRMACAO_MATRICULA ( 
                       ALUNO, 
                       CENSO, 
                       ANO, 
                       PERIODO, 
                       CURSO,
                       SERIE, 
                       TURNO, 
                       DT_CADASTRO,
                       DT_SUGERIDA, 
                       DT_ALTERACAO,
                       STATUS,
                       CURRICULO,
                       ENSINO_RELIGIOSO, 
                       LINGUA_ESTRANGEIRA_FACULTATIVA, 
                       PROJETO_AUTONOMIA, 
                       MATRICULA, 
                       TIPOVAGAOCUPADA 
                   ) VALUES (
                       @ALUNO, 
                       @CENSO, 
                       @ANO, 
                       @PERIODO, 
                       @CURSO, 
                       @SERIE, 
                       @TURNO, 
                       GetDate(),
                       GetDate(), 
                       GetDate(),
                       @STATUS,
                       @CURRICULO,
                       @ENSINO_RELIGIOSO, 
                       @LINGUA_ESTRANGEIRA_FACULTATIVA, 
                       @PROJETO_AUTONOMIA, 
                       @MATRICULA, 
                       @TIPOVAGAOCUPADA 
                   ) 

                    SELECT IDENT_CURRENT('dbo.TCE_CONFIRMACAO_MATRICULA') ";

            contextQuery.Parameters.Add("@ALUNO", confirmacaoMatricula.Aluno);
            contextQuery.Parameters.Add("@CENSO", confirmacaoMatricula.Censo);
            contextQuery.Parameters.Add("@ANO", confirmacaoMatricula.Ano);
            contextQuery.Parameters.Add("@PERIODO", confirmacaoMatricula.Periodo);
            contextQuery.Parameters.Add("@CURSO", confirmacaoMatricula.Curso);
            contextQuery.Parameters.Add("@SERIE", confirmacaoMatricula.Serie);
            contextQuery.Parameters.Add("@TURNO", confirmacaoMatricula.Turno);
            contextQuery.Parameters.Add("@STATUS", confirmacaoMatricula.Status);
            contextQuery.Parameters.Add("@ENSINO_RELIGIOSO", confirmacaoMatricula.EnsinoReligioso);
            contextQuery.Parameters.Add("@LINGUA_ESTRANGEIRA_FACULTATIVA", confirmacaoMatricula.LinguaEstrangeiraFacultativa);
            contextQuery.Parameters.Add("@PROJETO_AUTONOMIA", confirmacaoMatricula.ProjetoAutonomia);
            contextQuery.Parameters.Add("@MATRICULA", confirmacaoMatricula.Matricula);
            contextQuery.Parameters.Add("@CURRICULO", confirmacaoMatricula.Curriculo);
            contextQuery.Parameters.Add("@TIPOVAGAOCUPADA", confirmacaoMatricula.TipoVagaOcupada);

            confirmacaoMatricula.IdConfirmacaoMatricula = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        private void CancelaPossiveisConfirmacoesPor(DataContext contexto, string aluno, int ano, int periodo, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();

            string possiveisPeriodos = Utils.RecuperaPossiveisPeriodosCompleto(periodo);

            contextQuery.Command = string.Format(@" UPDATE  TCE_CONFIRMACAO_MATRICULA
                                                   SET     STATUS = @STATUS ,
                                                           MATRICULA = @MATRICULA ,
                                                           DT_ALTERACAO = GETDATE() 
                                                   WHERE   ALUNO = @ALUNO 
                                                           AND ANO = @ANO
                                                           AND (STATUS IS NULL OR STATUS <> @STATUS)
                                                           AND PERIODO IN ( {0} ) ", possiveisPeriodos);

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@STATUS", NaoConfirmado);
            contextQuery.Parameters.Add("@MATRICULA", usuario);

            contexto.ApplyModifications(contextQuery);
        }

        public bool EhConfirmacaoPendentePor(int idConfirmacaoMatricula)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                FROM TCE_CONFIRMACAO_MATRICULA (NOLOCK)
                                WHERE STATUS IS NULL
	                                AND ID_CONFIRMACAO_MATRICULA = @ID_CONFIRMACAO_MATRICULA  ";

                contextQuery.Parameters.Add("@ID_CONFIRMACAO_MATRICULA", SqlDbType.Int, idConfirmacaoMatricula);

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
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

        public bool EhConfirmacaoPendentePor(DataContext contexto, int idConfirmacaoMatricula)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM TCE_CONFIRMACAO_MATRICULA (NOLOCK)
                                WHERE STATUS IS NULL
	                                AND ID_CONFIRMACAO_MATRICULA = @ID_CONFIRMACAO_MATRICULA  ";

            contextQuery.Parameters.Add("@ID_CONFIRMACAO_MATRICULA", SqlDbType.Int, idConfirmacaoMatricula);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool EhConfirmacaoMatriculaFacilPor(DataContext contexto, int idConfirmacaoMatricula)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   TCE_CONFIRMACAO_MATRICULA (NOLOCK) 
                                        WHERE  ID_CONFIRMACAO_MATRICULA = @ID_CONFIRMACAO_MATRICULA
                                               AND ISNULL(MATRICULAFACIL, 0) = 1 ";

            contextQuery.Parameters.Add("@ID_CONFIRMACAO_MATRICULA", SqlDbType.Int, idConfirmacaoMatricula);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiConfirmacaoMatriculaFacilPendentePor(string aluno, decimal ano, decimal periodo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;
            string possiveisPeriodos = Utils.RecuperaPossiveisPeriodosCompleto(Convert.ToInt32(periodo));

            try
            {
                contextQuery.Command = string.Format(@" SELECT COUNT(*) 
                                FROM TCE_CONFIRMACAO_MATRICULA (NOLOCK)
                                WHERE STATUS IS NULL
									AND ISNULL(MATRICULAFACIL, 0) = 1
	                                AND ALUNO = @ALUNO
                                    AND ANO = @ANO
                                    AND PERIODO IN ( {0} ) ", possiveisPeriodos);

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
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

        public bool PossuiConfirmacaoMatriculaFacilPendenteConfirmadaPor(DataContext contexto, string aluno, decimal ano, decimal periodo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;
            string possiveisPeriodos = Utils.RecuperaPossiveisPeriodosCompleto(Convert.ToInt32(periodo));

            contextQuery.Command = string.Format(@" SELECT COUNT(*) 
                                FROM TCE_CONFIRMACAO_MATRICULA (NOLOCK)
                                WHERE (STATUS IS NULL OR STATUS = @CONFIRMADO )
									AND ISNULL(MATRICULAFACIL, 0) = 1
	                                AND ALUNO = @ALUNO
                                    AND ANO = @ANO
                                    AND PERIODO IN ( {0} ) ", possiveisPeriodos);

            contextQuery.Parameters.Add("@CONFIRMADO", SqlDbType.VarChar, Confirmado);
            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;

        }

        public static void Alterar(ICollection<TceConfirmacaoMatricula> confirmacaoMatriculas)
        {
            if (confirmacaoMatriculas == null)
            {
                return;
            }

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    foreach (var confirmacaoMatricula in confirmacaoMatriculas)
                    {
                        //Verifica se a confirmação é de vaga nova com prioridade
                        if (confirmacaoMatricula.Status == Confirmado && confirmacaoMatricula.TipoVagaOcupada == "VN")
                        {
                            //Cancela as confirmações confirmadas e matriculas
                            CancelaConfirmacaoMatriculaAtual(ctx, confirmacaoMatricula);
                        }

                        Alterar(confirmacaoMatricula, ctx);
                    }
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        private static void CancelaConfirmacaoMatriculaAtual(DataContext contexto, TceConfirmacaoMatricula confirmacaoMatricula)
        {
            string periodosPossiveis;
            RN.Matricula rnMatricula = new Matricula();
            RN.Matgrade rnMatgrade = new Matgrade();
            RN.DTOs.DadosLiberacaoConfirmacao liberacao = new DadosLiberacaoConfirmacao();
            RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            string observacao = "Confirmacao retirada por prioriade de vaga nova";

            //Monta periodos possiveis
            if (confirmacaoMatricula.Periodo == 0)
            {
                periodosPossiveis = "0, 1, 2";
            }
            else if (confirmacaoMatricula.Periodo == 1)
            {
                periodosPossiveis = "0, 1";
            }
            else
            {
                periodosPossiveis = "0, 2";
            }

            //Monta dados para liberação
            liberacao.Aluno = confirmacaoMatricula.Aluno;
            liberacao.Ano = Convert.ToInt32(confirmacaoMatricula.Ano);
            liberacao.MatriculaResponsavel = confirmacaoMatricula.Matricula;

            //Cancela matriculas
            rnMatricula.LiberaMatriculasEmPeridosPossiveisPor(contexto, liberacao, periodosPossiveis);

            //Cancela matgrade
            rnMatgrade.LiberaMatgradeEmPeridosPossiveisPor(contexto, liberacao, periodosPossiveis);

            //Cancela outra confirmacao confirmada atual
            rnConfirmacaoMatricula.CancelaPossiveisConfirmacaoMatriculaConfirmadaPor(contexto, confirmacaoMatricula.Aluno, Convert.ToInt32(confirmacaoMatricula.Ano), Convert.ToInt32(confirmacaoMatricula.Periodo), confirmacaoMatricula.Matricula, Convert.ToInt32(confirmacaoMatricula.Periodo), observacao, periodosPossiveis);
        }

        public static void InserirConfirmacao(TceConfirmacaoMatricula confirmacaoMatricula, string censoAtual)
        {
            //Se a escola foi absorvida valor de TipoVagaOcupada = VN, senao VC
            //REGRA RETIRADA - NAO IMPORTA A UNIDADE SERÁ SEMPRE VC
            //if (confirmacaoMatricula.Censo == censoAtual)
            //{
            //    confirmacaoMatricula.TipoVagaOcupada = "VC";
            //}
            //else
            //{
            //    confirmacaoMatricula.TipoVagaOcupada = "VN";
            //}

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                try
                {
                    var contextQuery = new ContextQuery(
                        @" INSERT  INTO TCE_CONFIRMACAO_MATRICULA ( ALUNO, CENSO, ANO, PERIODO, CURSO,
                                         SERIE, TURNO, DT_SUGERIDA, CURRICULO,
                                         ENSINO_RELIGIOSO, LINGUA_ESTRANGEIRA_FACULTATIVA,PROJETO_AUTONOMIA, MATRICULA, TIPOVAGAOCUPADA, STATUS, DT_CADASTRO,DT_ALTERACAO )
                                    VALUES  
                                        (@ALUNO, @CENSO, @ANO, @PERIODO, @CURSO, 
                                        @SERIE, @TURNO, GetDate(), @CURRICULO,
                                        
                                        @ENSINO_RELIGIOSO, @LINGUA_ESTRANGEIRA_FACULTATIVA,0, @MATRICULA, @TIPOVAGAOCUPADA, @STATUS, GetDate(),GetDate() ) ;
                            
                            SELECT    ID_CONFIRMACAO_MATRICULA 
                            FROM  TCE_CONFIRMACAO_MATRICULA
                            WHERE ALUNO=@ALUNO
                            AND CENSO=@CENSO
                            AND ANO=@ANO
                            AND PERIODO=@PERIODO
                            AND CURSO=@CURSO
                            AND SERIE=@SERIE
                            AND TURNO=@TURNO
                            AND CURRICULO=@CURRICULO
                            AND ENSINO_RELIGIOSO=@ENSINO_RELIGIOSO
                            AND LINGUA_ESTRANGEIRA_FACULTATIVA=@LINGUA_ESTRANGEIRA_FACULTATIVA
                            AND TIPOVAGAOCUPADA=@TIPOVAGAOCUPADA
                            AND STATUS=@STATUS
                            "
                        );

                    contextQuery.Parameters.Add("@ALUNO", confirmacaoMatricula.Aluno);
                    contextQuery.Parameters.Add("@CENSO", confirmacaoMatricula.Censo);
                    contextQuery.Parameters.Add("@ANO", confirmacaoMatricula.Ano);
                    contextQuery.Parameters.Add("@PERIODO", confirmacaoMatricula.Periodo);
                    contextQuery.Parameters.Add("@CURSO", confirmacaoMatricula.Curso);
                    contextQuery.Parameters.Add("@SERIE", confirmacaoMatricula.Serie);
                    contextQuery.Parameters.Add("@TURNO", confirmacaoMatricula.Turno);
                    contextQuery.Parameters.Add("@ENSINO_RELIGIOSO", confirmacaoMatricula.EnsinoReligioso);
                    contextQuery.Parameters.Add("@LINGUA_ESTRANGEIRA_FACULTATIVA", confirmacaoMatricula.LinguaEstrangeiraFacultativa);
                    contextQuery.Parameters.Add("@MATRICULA", confirmacaoMatricula.Matricula);
                    contextQuery.Parameters.Add("@CURRICULO", confirmacaoMatricula.Curriculo);
                    contextQuery.Parameters.Add("@TIPOVAGAOCUPADA", confirmacaoMatricula.TipoVagaOcupada);
                    contextQuery.Parameters.Add("@STATUS", confirmacaoMatricula.Status);

                    var codigo = ctx.GetReturnValue(contextQuery);

                    confirmacaoMatricula.IdConfirmacaoMatricula = Convert.ToInt32(codigo);
                }
                catch (Exception ex)
                {
                    ctx.Abandon();
                    var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                       Environment.NewLine, Convert.ToString(ex.Message));
                    throw new Exception(mensagem);
                }
            }
        }

        private static void Alterar(TceConfirmacaoMatricula confirmacaoMatricula)
        {
            var contextQuery = new ContextQuery(
                @" UPDATE  TCE_CONFIRMACAO_MATRICULA
                    SET     STATUS = @STATUS ,
                            MATRICULA = @MATRICULA , 
                            ENSINO_RELIGIOSO = @ENSINO_RELIGIOSO ,
                            LINGUA_ESTRANGEIRA_FACULTATIVA = @LINGUA_ESTRANGEIRA_FACULTATIVA ,
                            PROJETO_AUTONOMIA = @PROJETO_AUTONOMIA ,
                            CURRICULO = @CURRICULO ,
                            TIPOVAGAOCUPADA = @TIPOVAGAOCUPADA,
                            DT_ALTERACAO = GETDATE()
                    WHERE   ID_CONFIRMACAO_MATRICULA = @ID_CONFIRMACAO_MATRICULA ");

            contextQuery.Parameters.Add("@ID_CONFIRMACAO_MATRICULA", confirmacaoMatricula.IdConfirmacaoMatricula);

            contextQuery.Parameters.Add("@ENSINO_RELIGIOSO", confirmacaoMatricula.EnsinoReligioso);
            contextQuery.Parameters.Add("@LINGUA_ESTRANGEIRA_FACULTATIVA", confirmacaoMatricula.LinguaEstrangeiraFacultativa);
            contextQuery.Parameters.Add("@PROJETO_AUTONOMIA", confirmacaoMatricula.ProjetoAutonomia);
            contextQuery.Parameters.Add("@MATRICULA", confirmacaoMatricula.Matricula);
            contextQuery.Parameters.Add("@CURRICULO", confirmacaoMatricula.Curriculo);
            contextQuery.Parameters.Add("@TIPOVAGAOCUPADA", confirmacaoMatricula.TipoVagaOcupada);
            contextQuery.Parameters.Add("@STATUS", confirmacaoMatricula.Status);

            ExecutarAlteracao(contextQuery);
        }

        public void Atualiza(DataContext contexto, TceConfirmacaoMatricula confirmacaoMatricula)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE  TCE_CONFIRMACAO_MATRICULA
                    SET     STATUS = @STATUS ,
                            MATRICULA = @MATRICULA , 
                            ENSINO_RELIGIOSO = @ENSINO_RELIGIOSO ,
                            LINGUA_ESTRANGEIRA_FACULTATIVA = @LINGUA_ESTRANGEIRA_FACULTATIVA ,
                            PROJETO_AUTONOMIA = @PROJETO_AUTONOMIA ,
                            CURRICULO = @CURRICULO ,
                            TIPOVAGAOCUPADA = @TIPOVAGAOCUPADA,
                            DT_ALTERACAO = GETDATE()
                    WHERE   ID_CONFIRMACAO_MATRICULA = @ID_CONFIRMACAO_MATRICULA ";

            contextQuery.Parameters.Add("@ID_CONFIRMACAO_MATRICULA", confirmacaoMatricula.IdConfirmacaoMatricula);
            contextQuery.Parameters.Add("@ENSINO_RELIGIOSO", confirmacaoMatricula.EnsinoReligioso);
            contextQuery.Parameters.Add("@LINGUA_ESTRANGEIRA_FACULTATIVA", confirmacaoMatricula.LinguaEstrangeiraFacultativa);
            contextQuery.Parameters.Add("@PROJETO_AUTONOMIA", confirmacaoMatricula.ProjetoAutonomia);
            contextQuery.Parameters.Add("@MATRICULA", confirmacaoMatricula.Matricula);
            contextQuery.Parameters.Add("@CURRICULO", confirmacaoMatricula.Curriculo);
            contextQuery.Parameters.Add("@TIPOVAGAOCUPADA", confirmacaoMatricula.TipoVagaOcupada);
            contextQuery.Parameters.Add("@STATUS", confirmacaoMatricula.Status);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizarStatusNaoConfirmado(TceConfirmacaoMatricula confirmacaoMatricula)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE  TCE_CONFIRMACAO_MATRICULA
                                                SET     STATUS = 'Não Confirmado' ,
                                                        DT_ALTERACAO = GETDATE()
                                                WHERE   STATUS IS NULL
                                                        AND ALUNO = @ALUNO
                                                        AND ANO = @ANO
                                                        AND ( ( ( @PERIODO = 1
                                                                  OR @PERIODO = 0
                                                                )
                                                                AND ( PERIODO = 1
                                                                      OR PERIODO = 0
                                                                    )
                                                              )
                                                              OR ( @PERIODO = 2
                                                                   AND PERIODO = 2
                                                                 )
                                                            ) ";

                contextQuery.Parameters.Add("@ALUNO", confirmacaoMatricula.Aluno);
                contextQuery.Parameters.Add("@ANO", confirmacaoMatricula.Ano);
                contextQuery.Parameters.Add("@PERIODO", confirmacaoMatricula.Periodo);

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

        public void AtualizaStatusNaoConfirmadoPor(string aluno, int ano, int periodo, string responsavel)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                this.AtualizaStatusNaoConfirmadoPor(ctx, aluno, ano, periodo, responsavel);
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

        public void AtualizaStatusNaoConfirmadoPor(DataContext ctx, string aluno, int ano, int periodo, string responsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE  TCE_CONFIRMACAO_MATRICULA
                                                SET     STATUS = @STATUSNAOCONFIRMADO ,
                                                        DT_ALTERACAO = GETDATE() ,
                                                        MATRICULA = @MATRICULA
                                                WHERE   ALUNO = @ALUNO
                                                        AND (STATUS IS NULL OR STATUS = @STATUSCONFIRMADO)
                                                        AND ANO = @ANO
                                                        AND ( ( ( @PERIODO = 1
                                                                  OR @PERIODO = 0
                                                                )
                                                                AND ( PERIODO = 1
                                                                      OR PERIODO = 0
                                                                    )
                                                              )
                                                              OR ( @PERIODO = 2
                                                                   AND PERIODO = 2
                                                                 )
                                                            ) ";

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@STATUSNAOCONFIRMADO", NaoConfirmado);
            contextQuery.Parameters.Add("@STATUSCONFIRMADO", Confirmado);
            contextQuery.Parameters.Add("@MATRICULA", responsavel);

            ctx.ApplyModifications(contextQuery);
        }

        public void InserirOuAtualizar(TceConfirmacaoMatricula confirmacaoMatricula)
        {
            //Atualiza todas as possiveis renovações ativas ou pendentes do aluno para não confirmado
            AtualizaStatusNaoConfirmadoPor(confirmacaoMatricula.Aluno, Convert.ToInt32(confirmacaoMatricula.Ano), Convert.ToInt32(confirmacaoMatricula.Periodo), confirmacaoMatricula.Matricula);

            int idConfirmacao = ObtemIdConfirmacaoMatriculaPor(confirmacaoMatricula.Aluno, confirmacaoMatricula.Ano, confirmacaoMatricula.Periodo, confirmacaoMatricula.Censo, confirmacaoMatricula.Curso, confirmacaoMatricula.Serie, confirmacaoMatricula.Turno);

            if (idConfirmacao > 0)
            {
                confirmacaoMatricula.IdConfirmacaoMatricula = idConfirmacao;
                Alterar(confirmacaoMatricula);
            }
            else
            {
                Inserir(confirmacaoMatricula);
            }
        }

        public TceConfirmacaoMatricula ObtemConfirmacaoPendenteOuAtivaPor(string aluno, decimal ano, decimal periodo, int serie, string turno, string curso, string unidade)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            TceConfirmacaoMatricula confirmacao = new TceConfirmacaoMatricula();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT TOP 1
                                                *
                                        FROM    TCE_CONFIRMACAO_MATRICULA (NOLOCK)
                                        WHERE   ALUNO = @ALUNO
                                                AND ANO = @ANO
                                                AND PERIODO = @PERIODO
                                                AND SERIE = @SERIE
                                                AND TURNO = @TURNO
                                                AND CURSO = @CURSO
                                                AND CENSO = @UNIDADE
                                                AND ( [STATUS] = 'Confirmado'
                                                      OR [STATUS] IS NULL
                                                    )
                                        ORDER BY DT_CADASTRO DESC ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@SERIE", serie);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@UNIDADE", unidade);

                confirmacao = ctx.TryToBindEntity<TceConfirmacaoMatricula>(contextQuery);

                return confirmacao;
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

        public int ObtemConfirmacoesPendentesPor(DataContext contexto, string censo, int ano, int periodo, int serie, string curso, string turno)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @"  SELECT  COUNT(*) AS TOTAL
                                            FROM    TCE_CONFIRMACAO_MATRICULA CM (NOLOCK) 
                                            INNER JOIN dbo.LY_ALUNO A (NOLOCK)  ON A.ALUNO =CM.ALUNO
                                            WHERE   CM.CENSO = @CENSO
                                                    AND CM.ANO = @ANO
                                                    AND CM.PERIODO = @PERIODO
                                                    AND CM.CURSO = @CURSO
                                                    AND CM.SERIE = @SERIE
                                                    AND CM.TURNO = @TURNO
                                                    AND [STATUS] IS NULL 
                                                    AND SIT_ALUNO = 'Ativo' ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
                contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);
                contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, turno);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["TOTAL"]);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return retorno;
        }

        public TceConfirmacaoMatricula ObtemConfirmacaoAtivaPor(string aluno, int ano, int periodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            TceConfirmacaoMatricula confirmacao = new TceConfirmacaoMatricula();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT TOP 1
                                                *
                                        FROM    TCE_CONFIRMACAO_MATRICULA (NOLOCK)
                                        WHERE   ALUNO = @ALUNO
                                                AND ANO = @ANO
                                                AND PERIODO = @PERIODO
                                                AND [STATUS] = 'Confirmado'
                                        ORDER BY DT_CADASTRO DESC ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@ALUNO", aluno);

                confirmacao = ctx.TryToBindEntity<TceConfirmacaoMatricula>(contextQuery);

                return confirmacao;
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

        public TceConfirmacaoMatricula ObtemConfirmacaoAtivaPor(string aluno, decimal ano, decimal periodo, int serie, string turno, string curso, string unidade)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            TceConfirmacaoMatricula confirmacao = new TceConfirmacaoMatricula();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT TOP 1
                                                *
                                        FROM    TCE_CONFIRMACAO_MATRICULA (NOLOCK)
                                        WHERE   ALUNO = @ALUNO
                                                AND ANO = @ANO
                                                AND PERIODO = @PERIODO
                                                AND SERIE = @SERIE
                                                AND TURNO = @TURNO
                                                AND CURSO = @CURSO
                                                AND CENSO = @UNIDADE
                                                AND [STATUS] = 'Confirmado'
                                        ORDER BY DT_CADASTRO DESC ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@SERIE", serie);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@UNIDADE", unidade);

                confirmacao = ctx.TryToBindEntity<TceConfirmacaoMatricula>(contextQuery);

                return confirmacao;
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

        public TceConfirmacaoMatricula ObtemConfirmacaoAtivaPor(string aluno, decimal ano, decimal periodo, int serie, string turno, string curso, string unidade, string tipoVaga)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            TceConfirmacaoMatricula confirmacao = new TceConfirmacaoMatricula();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT TOP 1
                                                *
                                        FROM    TCE_CONFIRMACAO_MATRICULA (NOLOCK)
                                        WHERE   ALUNO = @ALUNO
                                                AND ANO = @ANO
                                                AND PERIODO = @PERIODO
                                                AND SERIE = @SERIE
                                                AND TURNO = @TURNO
                                                AND CURSO = @CURSO
                                                AND CENSO = @UNIDADE
                                                AND [STATUS] = 'Confirmado'
                                                AND TIPOVAGAOCUPADA = @TIPOVAGAOCUPADA
                                        ORDER BY DT_CADASTRO DESC ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@SERIE", serie);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@UNIDADE", unidade);
                contextQuery.Parameters.Add("@TIPOVAGAOCUPADA", tipoVaga);

                confirmacao = ctx.TryToBindEntity<TceConfirmacaoMatricula>(contextQuery);

                return confirmacao;
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

        public static DataTable VerificaConfirmacaoMatriculaAluno(string aluno, decimal ano, decimal periodo, int serie, string turno, string curso, string unidade)
        {
            var dataTable = new DataTable();

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = @"SELECT * FROM TCE_CONFIRMACAO_MATRICULA (NOLOCK) 
                                        WHERE   ALUNO = @ALUNO 
                                        AND ANO = @ANO 
                                        AND PERIODO = @PERIODO 
                                        AND SERIE = @SERIE 
                                        AND TURNO = @TURNO
                                        AND CURSO = @CURSO
                                        AND CENSO = @UNIDADE
                                        AND [STATUS] = 'Confirmado'";
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@SERIE", serie);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@UNIDADE", unidade);

                dataTable = ctx.GetDataTable(contextQuery);
            }

            return dataTable;
        }

        public DataTable ListaPossiveisConfirmacaoMatriculaPor(string aluno, int ano, int periodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable possiveisConfirmacoes = null;

            string possiveisPeriodos = Utils.RecuperaPossiveisFuturosPeriodos(periodo);

            try
            {
                contextQuery.Command = string.Format(@" SELECT  *
                            FROM    DBO.TCE_CONFIRMACAO_MATRICULA
                            WHERE   ALUNO = @ALUNO
                                    AND ([STATUS] = 'CONFIRMADO' OR [STATUS] IS NULL)
                                    AND ANO = @ANO
                                    AND PERIODO IN ( {0} ) ", possiveisPeriodos);

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);

                possiveisConfirmacoes = ctx.GetDataTable(contextQuery);
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

            return possiveisConfirmacoes;
        }

        public DataTable ListaPossiveisConfirmacaoMatriculaPor(string aluno, int ano)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable possiveisConfirmacoes = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT CM.ID_CONFIRMACAO_MATRICULA, 
                                            CM.ALUNO, 
                                            CM.CENSO, 
                                            CM.ANO, 
                                            CM.PERIODO, 
                                            C.NOME AS CURSO, 
                                            CM.SERIE, 
                                            CM.TURNO, 
                                            CM.DT_SUGERIDA, 
                                            CM.ENSINO_RELIGIOSO, 
                                            CM.LINGUA_ESTRANGEIRA_FACULTATIVA, 
                                            CM.PROJETO_AUTONOMIA,                                             
                                            CM.MATRICULA, 
                                            CM.DT_CADASTRO, 
                                            CM.DT_ALTERACAO, 
                                            CONVERT(VARCHAR(50), CM.DT_SUGERIDA, 103) AS DT_SUGERIDA_FORMATADA,
											CONVERT(VARCHAR(50), CM.DT_ALTERACAO, 103) AS DT_ALTERACAO_FORMATADA,
                                            E.NOME_COMP AS 
                                            ESCOLA, 
                                            CM.CENSO + ' - ' + E.NOME_COMP AS UNIDADE_ENSINO, 
                                            MD.MODALIDADE, 
                                            CM.CURRICULO, 
                                            TC.TIPO AS SEGMENTO, 
                                            T.DESCRICAO 
                                            AS NOME_TURNO, 
                                            CM.CURSO AS COD_CURSO, 
                                            MD.DESCRICAO + ' / ' + TC.DESCRICAO + ' / ' + C.NOME AS MOD_SEG_CURSO, 
                                            TIPOVAGAOCUPADA, 
                                            ISNULL(CR.ENSINO_RELIGIOSO, 'N') AS PODE_ENSINO_RELIGIOSO, 
                                            ISNULL(CR.LINGUA_ESTRANGEIRA, 'N') AS PODE_LINGUA_ESTRANGEIRA, 
                                            CASE 
												WHEN CM.STATUS IS NULL THEN 'Pendente'
                                                ELSE CM.STATUS
                                            END STATUS
                            FROM   dbo.TCE_CONFIRMACAO_MATRICULA CM (nolock) 
                                    INNER JOIN dbo.LY_UNIDADE_ENSINO E ON CM.censo = E.UNIDADE_ENS 
                                    INNER JOIN dbo.LY_CURSO C ON CM.curso = C.CURSO 
                                    INNER JOIN dbo.LY_MODALIDADE_CURSO MD ON C.MODALIDADE = MD.MODALIDADE 
                                    INNER JOIN dbo.LY_TIPO_CURSO TC ON C.TIPO = TC.TIPO 
                                    INNER JOIN dbo.LY_TURNO t ON CM.turno = t.TURNO 
                                    LEFT JOIN dbo.LY_CURRICULO cr ON CM.curso = cr.CURSO AND CM.turno = cr.TURNO AND CM.curriculo = cr.CURRICULO 
                            WHERE  CM.ALUNO = @ALUNO    
								   AND CM.ANO = @ANO
                            ORDER  BY CM.ANO DESC, CM.DT_CADASTRO ";

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);

                possiveisConfirmacoes = ctx.GetDataTable(contextQuery);
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

            return possiveisConfirmacoes;
        }

        public DataTable ObtemConfirmacaoMatriculaPor(string aluno, decimal ano, decimal periodo)
        {
            DataContext contexto = null;
            DataTable dataTable = new DataTable();

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                dataTable = ObtemConfirmacaoMatriculaPor(contexto, aluno, ano, periodo);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;

                if (contexto != null)
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
                if (contexto != null)
                    contexto.Dispose();
            }

            return dataTable;
        }

        public DataTable ObtemConfirmacaoMatriculaPor(DataContext contexto, string aluno, decimal ano, decimal periodo)
        {
            DataTable dataTable = new DataTable();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT *
                                        FROM TCE_CONFIRMACAO_MATRICULA (NOLOCK)
                                       WHERE ALUNO = @ALUNO
                                             AND ANO = @ANO
                                             AND PERIODO = @PERIODO
                                             AND ( [STATUS] = 'Confirmado'
                                                   OR [STATUS] IS NULL)
                                       ORDER BY STATUS DESC ";

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@ALUNO", aluno);

            dataTable = contexto.GetDataTable(contextQuery);

            return dataTable;
        }

        private TceConfirmacaoMatricula ObtemConfirmacaoMatriculaPor(DataContext contexto, int idConfirmacaoMatricula)
        {
            TceConfirmacaoMatricula confirmacao = new TceConfirmacaoMatricula();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT *
                                        FROM    TCE_CONFIRMACAO_MATRICULA (NOLOCK)
                                        WHERE   ID_CONFIRMACAO_MATRICULA = @ID_CONFIRMACAO_MATRICULA ";

            contextQuery.Parameters.Add("@ID_CONFIRMACAO_MATRICULA", idConfirmacaoMatricula);

            confirmacao = contexto.TryToBindEntity<TceConfirmacaoMatricula>(contextQuery);

            return confirmacao;
        }

        public int ObtemIdConfirmacaoMatriculaPendenteOuAtivaPor(DataContext contexto, string aluno, decimal ano, decimal periodo)
        {
            return ObtemIdConfirmacaoMatriculaPendenteOuAtivaPor(contexto, aluno, ano, periodo, null, null, null, null);
        }

        public void EncerraConfirmacoesMatricula(DataContext contexto, string aluno, int ano, int periodo)
        {
            ContextQuery contextQuery = new ContextQuery();

            string observacao = string.Format("ALUNO ENCERRADO EM {0} ÀS {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm"));

            contextQuery.Command = @" UPDATE TCE_CONFIRMACAO_MATRICULA
                            SET STATUS = @STATUS,
                            OBSERVACAO = @OBSERVACAO
                            WHERE   ALUNO = @ALUNO
                                    AND ANO = @ANO
                                    AND PERIODO = @PERIODO
                                    AND (STATUS = 'CONFIRMADO' OR STATUS IS NULL) ";

            contextQuery.Parameters.Add("@STATUS", SqlDbType.VarChar, ConfirmacaoMatricula.NaoConfirmado);
            contextQuery.Parameters.Add("@OBSERVACAO", SqlDbType.VarChar, observacao);
            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);

            contexto.ApplyModifications(contextQuery);
        }

        public int ObtemIdConfirmacaoMatriculaPendenteOuAtivaPor(DataContext contexto, string aluno, decimal ano, decimal periodo, string censo, string curso, decimal? serie, string turno)
        {
            ContextQuery contextQuery = new ContextQuery();
            int idConfirmacaoMatricula = int.MinValue;

            contextQuery.Command = @" SELECT TOP 1 ID_CONFIRMACAO_MATRICULA
                                      FROM   TCE_CONFIRMACAO_MATRICULA (NOLOCK)
                                      WHERE  ALUNO = @ALUNO
                                             AND ANO = @ANO
                                             AND PERIODO = @PERIODO
                                             AND ( [STATUS] = 'CONFIRMADO'
                                                   OR [STATUS] IS NULL )";

            if (serie != null)
            {
                contextQuery.Command += " AND SERIE = @SERIE";
                contextQuery.Parameters.Add("@SERIE", serie);
            }

            if (turno != null)
            {
                contextQuery.Command += " AND TURNO = @TURNO";
                contextQuery.Parameters.Add("@TURNO", turno);
            }

            if (curso != null)
            {
                contextQuery.Command += " AND CURSO = @CURSO";
                contextQuery.Parameters.Add("@CURSO", curso);
            }

            if (censo != null)
            {
                contextQuery.Command += " AND CENSO = @CENSO";
                contextQuery.Parameters.Add("@CENSO", censo);
            }

            contextQuery.Command += " ORDER BY STATUS DESC ";

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);

            object retorno = contexto.GetReturnValue(contextQuery);

            if (retorno != null)
                idConfirmacaoMatricula = Convert.ToInt32(retorno);

            return idConfirmacaoMatricula;
        }

        public int ObtemIdConfirmacaoMatriculaPor(string aluno, decimal ano, decimal periodo, string censo, string curso, decimal serie, string turno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            int idConfirmacaoMatricula = 0;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT TOP 1 ID_CONFIRMACAO_MATRICULA
                            FROM    TCE_CONFIRMACAO_MATRICULA (NOLOCK)
                            WHERE   ALUNO = @ALUNO
                                    AND ANO = @ANO
                                    AND PERIODO = @PERIODO
                                    AND SERIE = @SERIE
                                    AND TURNO = @TURNO
                                    AND CURSO = @CURSO
                                    AND CENSO = @CENSO
                            ORDER BY DT_CADASTRO DESC "
                };

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@SERIE", serie);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@CURSO", curso);

                idConfirmacaoMatricula = ctx.GetReturnValue(contextQuery) == null ? 0 : ctx.GetReturnValue<int>(contextQuery);

                return idConfirmacaoMatricula;
            }

            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public void AlteraDados(DataContext ctx, TceConfirmacaoMatricula confirmacaoMatricula)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE  TCE_CONFIRMACAO_MATRICULA
                                    SET     CURRICULO = @CURRICULO ,                                           
                                            MATRICULA = @MATRICULA ,
                                            [STATUS] = @STATUS ,
                                            TIPOVAGAOCUPADA = @TIPOVAGAOCUPADA ,
                                            PROJETO_AUTONOMIA = @PROJETO_AUTONOMIA ,
                                            OBSERVACAO = @OBSERVACAO ,
                                            DT_ALTERACAO = GETDATE()
                                    WHERE   ID_CONFIRMACAO_MATRICULA = @ID_CONFIRMACAO_MATRICULA ";

                contextQuery.Parameters.Add("@ID_CONFIRMACAO_MATRICULA", confirmacaoMatricula.IdConfirmacaoMatricula);
                contextQuery.Parameters.Add("@CURRICULO", confirmacaoMatricula.Curriculo);
                contextQuery.Parameters.Add("@MATRICULA", confirmacaoMatricula.Matricula);
                contextQuery.Parameters.Add("@STATUS", confirmacaoMatricula.Status);
                contextQuery.Parameters.Add("@TIPOVAGAOCUPADA", confirmacaoMatricula.TipoVagaOcupada);
                contextQuery.Parameters.Add("@PROJETO_AUTONOMIA", confirmacaoMatricula.ProjetoAutonomia);
                contextQuery.Parameters.Add("@OBSERVACAO", confirmacaoMatricula.Observacao);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AtualizaDadosTransferencia(TceConfirmacaoMatricula confirmacaoMatricula, List<ContextQuery> listaContextQuery)
        {
            ContextQuery contextQuery = new ContextQuery(
                @" UPDATE  TCE_CONFIRMACAO_MATRICULA
                   SET     CURRICULO = @CURRICULO ,
                           PERIODO = @PERIODO ,
                           STATUS = @STATUS ,
                           TIPOVAGAOCUPADA = @TIPOVAGAOCUPADA ,
                           ENSINO_RELIGIOSO = @ENSINO_RELIGIOSO,
                           LINGUA_ESTRANGEIRA_FACULTATIVA = @LINGUA_ESTRANGEIRA_FACULTATIVA,
                           MATRICULA = @MATRICULA ,
                           DT_ALTERACAO = GETDATE()
                   WHERE   ID_CONFIRMACAO_MATRICULA = @ID_CONFIRMACAO_MATRICULA ",
                new ContextQueryParameter("@CURRICULO", confirmacaoMatricula.Curriculo),
                new ContextQueryParameter("@PERIODO", confirmacaoMatricula.Periodo),
                new ContextQueryParameter("@STATUS", confirmacaoMatricula.Status),
                new ContextQueryParameter("@MATRICULA", confirmacaoMatricula.Matricula),
                new ContextQueryParameter("@ID_CONFIRMACAO_MATRICULA", confirmacaoMatricula.IdConfirmacaoMatricula),
                new ContextQueryParameter("@ENSINO_RELIGIOSO", confirmacaoMatricula.EnsinoReligioso),
                new ContextQueryParameter("@LINGUA_ESTRANGEIRA_FACULTATIVA", confirmacaoMatricula.LinguaEstrangeiraFacultativa),
                new ContextQueryParameter("@TIPOVAGAOCUPADA", "VN")
            );

            listaContextQuery.Add(contextQuery);
        }

        public void CancelaOutrasConfirmacoesPossiveisPor(string aluno, int ano, string observacao, string matriculaAndamento, string possiveisPeriodos, int idConfirmacaoMatricula, List<ContextQuery> listaContextQuery)
        {
            ContextQuery contextQuery = new ContextQuery(
                string.Format(@" UPDATE  TCE_CONFIRMACAO_MATRICULA
                                 SET     [STATUS] = @STATUS ,
                                         MATRICULA = @MATRICULA ,
                                         DT_ALTERACAO = @DT_ALTERACAO ,
                                         OBSERVACAO = ISNULL(OBSERVACAO, '') + '  ' + @OBSERVACAO
                                 WHERE   ALUNO = @ALUNO
                                         AND ( [STATUS] = 'CONFIRMADO'
                                               OR [STATUS] IS NULL )
                                         AND ANO = @ANO
                                         AND ID_CONFIRMACAO_MATRICULA <> @ID_CONFIRMACAO_MATRICULA
                                         AND PERIODO IN ( {0} ) ", possiveisPeriodos),
                new ContextQueryParameter("@ALUNO", aluno),
                new ContextQueryParameter("@ANO", ano),
                new ContextQueryParameter("@STATUS", ConfirmacaoMatricula.NaoConfirmado),
                new ContextQueryParameter("@DT_ALTERACAO", DateTime.Now),
                new ContextQueryParameter("@OBSERVACAO", observacao),
                new ContextQueryParameter("@MATRICULA", matriculaAndamento),
                new ContextQueryParameter("@ID_CONFIRMACAO_MATRICULA", idConfirmacaoMatricula)
            );

            listaContextQuery.Add(contextQuery);
        }

        public static string RetornaProximoAnoPeriodoConfirmacaoMatricula(string aluno)
        {
            var matriculaAtiva = RN.Matricula.CarregarMatriculaAtiva(aluno);

            string proximoAno = string.Empty;
            string proximoPeriodo = string.Empty;
            string AnoPeriodo = string.Empty;

            if (string.IsNullOrEmpty(matriculaAtiva.Turma))
            {
                AnoPeriodo = string.Empty;
                return AnoPeriodo;
            }

            //Se o período atual do aluno for ‘0’ então o valor inicial é Ano Atual +1 + ‘- 0’.
            if (matriculaAtiva.Semestre == 0)
            {
                proximoAno = Convert.ToString(matriculaAtiva.Ano + 1);
                proximoPeriodo = "0";
            }

            //Se o período atual do aluno for ‘1’ então o valor inicial é Ano Atual  + ‘- 2’.
            if (matriculaAtiva.Semestre == 1)
            {
                proximoAno = Convert.ToString(matriculaAtiva.Ano);
                proximoPeriodo = "2";
            }

            //Se o período atual do aluno for ‘2’ então o valor inicial é Ano Atual +1 + ‘- 1’.
            if (matriculaAtiva.Semestre == 2)
            {
                proximoAno = Convert.ToString(matriculaAtiva.Ano + 1);
                proximoPeriodo = "1";
            }

            AnoPeriodo = string.Format("{0} - {1}", proximoAno, proximoPeriodo);
            return AnoPeriodo;
        }

        public static DataTable ListaUnidadeEnsinoConfirmacaoMatricula(string unidadeEnsinoAtual)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT U.UNIDADE_ENS, UNIDADE_ENS + ' - ' + U.NOME_COMP AS NOME_COMP
                        FROM LY_UNIDADE_ENSINO U 
                        WHERE 
                          (
                          U.UNIDADE_ENS = @UNIDADE_ENS --Escola atual
                          AND (

                            --Unidade totalmente absorvida não aparece
                            NOT EXISTS (
                            SELECT TOP 1 S.UNIDADEENSINOORIGEMID
                            FROM SERIEABSORVIDA S
                            WHERE S.NIVELABSORCAOID = 1 --unidade educãcional
                              AND S.UNIDADEENSINOORIGEMID = U.UNIDADE_ENS)

                            --Unidade NÃO absorvida aparece
                            OR NOT EXISTS (
                            SELECT TOP 1 S.UNIDADEENSINOORIGEMID
                            FROM SERIEABSORVIDA S
                            WHERE S.UNIDADEENSINOORIGEMID = U.UNIDADE_ENS)
                            ) --fechando AND
                          )

                          --Unidades absorvedoras aparecem
                          OR EXISTS (
                            SELECT TOP 1 S.UNIDADEENSINODESTINOID
                            FROM SERIEABSORVIDA S
                            WHERE S.UNIDADEENSINODESTINOID = U.UNIDADE_ENS
                              AND S.UNIDADEENSINOORIGEMID = @UNIDADE_ENS --Escola atual
                            ) ");

                contextQuery.Parameters.Add("@UNIDADE_ENS", unidadeEnsinoAtual);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListaProximoTurnoConfirmacaoMatricula(int proximoAno, int proximoPeriodo, string proximoCenso, string proximoCurso)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"  SELECT DISTINCT C.TURNO ,T.DESCRICAO
                        FROM LY_GRADE G
                               INNER JOIN LY_CURRICULO C 
                                 ON G.CURRICULO = C.CURRICULO
                                    AND G.CURSO = C.CURSO
                                    AND G.TURNO = C.TURNO
                               INNER JOIN LY_TURNO T 
                                 ON T.TURNO = C.TURNO
                          INNER JOIN LY_UNIDADE_ENSINO_CURSOS U 
                          ON (U.CURSO = C.CURSO)
                          INNER JOIN DBO.TCE_CONTROLE_VAGA CV ON CV.ANO = ANO_INI 
                                               AND CV.PERIODO = SEM_INI
                                               AND CV.CENSO = U.UNIDADE_ENS
                                               AND CV.CURSO = C.CURSO
                                               AND CV.TURNO = C.TURNO
                        WHERE ANO_INI = @PROXIMO_ANO --Ano
                          AND SEM_INI = @PROXIMO_PERIODO --Período
                          AND U.UNIDADE_ENS = @PROXIMO_CENSO --Escola selecionada           
                          AND ( DT_EXTINCAO IS NULL
                                OR CONVERT(DATE, DT_EXTINCAO) > CONVERT(DATE, GETDATE()))
                          AND C.CURSO = @PROXIMO_CURSO --Curso Selecionado 
                          AND
                          (
                          --Curso/Turno absorvido ou Turno totalmente absorvido não aparece
                          NOT EXISTS (
                            SELECT TOP 1 S.UNIDADEENSINOORIGEMID
                            FROM SERIEABSORVIDA S
                            WHERE S.NIVELABSORCAOID = 3 --turno
                              AND S.UNIDADEENSINOORIGEMID = U.UNIDADE_ENS
                              AND (S.CURSOORIGEMID = C.CURSO OR S.CURSOORIGEMID IS NULL)
                              AND S.TURNOORIGEMID = C.TURNO)
                          )
                        ORDER BY C.TURNO
                 ");

                contextQuery.Parameters.Add("@PROXIMO_ANO", proximoAno);
                contextQuery.Parameters.Add("@PROXIMO_PERIODO", proximoPeriodo);
                contextQuery.Parameters.Add("@PROXIMO_CENSO", proximoCenso);
                contextQuery.Parameters.Add("@PROXIMO_CURSO", proximoCurso);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListaProximoCurriculoConfirmacaoMatricula(int proximoAno, int proximoPeriodo, string proximoCenso, string proximoCurso, string proximoTurno)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"  SELECT DISTINCT C.CURRICULO 
                        FROM LY_GRADE G
                               INNER JOIN LY_CURRICULO C 
                                 ON G.CURRICULO = C.CURRICULO
                                    AND G.CURSO = C.CURSO
                                    AND G.TURNO = C.TURNO
                          INNER JOIN LY_UNIDADE_ENSINO_CURSOS U 
                                  ON (U.CURSO = C.CURSO)
                        WHERE C.ANO_INI = @PROXIMO_ANO --Ano
                          AND C.SEM_INI = @PROXIMO_PERIODO --Período
                          AND U.UNIDADE_ENS = @PROXIMO_CENSO --Escola selecionada           
                          AND ( DT_EXTINCAO IS NULL
                                OR CONVERT(DATE, DT_EXTINCAO) > CONVERT(DATE, GETDATE()))
                          AND C.CURSO = @PROXIMO_CURSO --Curso Selecionado 
                          AND C.TURNO = @PROXIMO_TURNO --Turno Selecionado 
                        ORDER BY C.CURRICULO
                         ");

                contextQuery.Parameters.Add("@PROXIMO_ANO", proximoAno);
                contextQuery.Parameters.Add("@PROXIMO_PERIODO", proximoPeriodo);
                contextQuery.Parameters.Add("@PROXIMO_CENSO", proximoCenso);
                contextQuery.Parameters.Add("@PROXIMO_CURSO", proximoCurso);
                contextQuery.Parameters.Add("@PROXIMO_TURNO", proximoTurno);


                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListaConfirmacaoMatriculaConfirmadaPor(string aluno)
        {
            if (aluno.IsNullOrEmptyOrWhiteSpace())
            {
                return null;
            }

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"  SELECT DISTINCT CM.ID_CONFIRMACAO_MATRICULA, CM.ALUNO, CM.CENSO, CM.ANO, CM.PERIODO,
                                    C.NOME AS CURSO, CM.SERIE, CM.TURNO, CM.DT_SUGERIDA, CM.ENSINO_RELIGIOSO,
                                    CM.LINGUA_ESTRANGEIRA_FACULTATIVA, CM.PROJETO_AUTONOMIA, CM.STATUS,
                                    CM.MATRICULA, CM.DT_CADASTRO, CM.DT_ALTERACAO, E.NOME_COMP AS ESCOLA,
                                    CM.CENSO + ' - ' + E.NOME_COMP AS UNIDADE_ENSINO, MD.MODALIDADE,CM.CURRICULO,
                                    TC.TIPO AS SEGMENTO, T.DESCRICAO AS NOME_TURNO, CM.CURSO AS COD_CURSO,
                                    MD.DESCRICAO + ' / ' + TC.DESCRICAO + ' / ' + C.NOME AS MOD_SEG_CURSO,
                                    ISNULL(CR.ENSINO_RELIGIOSO, 'N') AS PODE_ENSINO_RELIGIOSO, ISNULL(CR.LINGUA_ESTRANGEIRA, 'N') AS PODE_LINGUA_ESTRANGEIRA,
                                     CASE WHEN CM.DT_ALTERACAO IS NOT NULL
                                              AND CM.STATUS IS NULL THEN 'N'
                                         WHEN CM.DT_ALTERACAO IS NULL THEN 'N'
                                         ELSE 'S'
                                    END CADASTROU
                        FROM DBO.TCE_CONFIRMACAO_MATRICULA CM (NOLOCK)
                        INNER JOIN DBO.LY_UNIDADE_ENSINO E (NOLOCK) ON  CM.CENSO = E.UNIDADE_ENS
                        INNER JOIN DBO.LY_CURSO C (NOLOCK) ON  CM.CURSO = C.CURSO
                        INNER JOIN DBO.LY_MODALIDADE_CURSO MD (NOLOCK) ON C.MODALIDADE = MD.MODALIDADE
                        INNER JOIN DBO.LY_TIPO_CURSO TC (NOLOCK)ON C.TIPO = TC.TIPO
                        INNER JOIN DBO.LY_TURNO T (NOLOCK) ON CM.TURNO = T.TURNO
                        LEFT JOIN DBO.LY_CURRICULO CR (NOLOCK) ON CM.CURSO = CR.CURSO AND CM.TURNO = CR.TURNO AND CM.CURRICULO = CR.CURRICULO 
                        WHERE CM.STATUS = @STATUS
                        AND CM.ALUNO = @ALUNO
                        ORDER BY CM.ANO DESC, DT_CADASTRO ");

                contextQuery.Parameters.Add("@STATUS", Confirmado);
                contextQuery.Parameters.Add("@ALUNO", aluno);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static bool PossuiConfirmacaoEmAberto(string aluno)
        {
            string sql = "select 1 from TCE_CONFIRMACAO_MATRICULA (NOLOCK) where ALUNO = ? AND STATUS IS NULL ";

            int retorno = ExecutarFuncao(sql, aluno);

            if (retorno == 1)
                return true;
            else
                return false;
        }

        public static DataTable RetornaListaProximoAnoPeriodoConfirmacaoMatricula(string aluno)
        {
            DataTable dtAno = new DataTable();

            dtAno.Columns.Add("ano");

            var matriculaAtiva = RN.Matricula.CarregarMatriculaAtiva(aluno);


            if (!PeriodoLetivo.PossuiProximoAno(matriculaAtiva.Ano))
                return dtAno;

            string proximoAno = string.Empty;
            string proximoPeriodo = string.Empty;
            string AnoPeriodo = string.Empty;

            if (string.IsNullOrEmpty(matriculaAtiva.Turma))
            {
                AnoPeriodo = string.Empty;
                dtAno.Rows.Add(AnoPeriodo);
                return dtAno;
            }

            //Se o período atual do aluno for ‘0’ então o valor inicial é Ano Atual +1 + ‘- 0’.
            if (matriculaAtiva.Semestre == 0)
            {
                proximoAno = Convert.ToString(matriculaAtiva.Ano + 1);
                proximoPeriodo = "0";

                AnoPeriodo = string.Format("{0} - {1}", proximoAno, proximoPeriodo);
                dtAno.Rows.Add(AnoPeriodo);

                proximoAno = Convert.ToString(matriculaAtiva.Ano + 1);
                proximoPeriodo = "1";

                dtAno.Rows.Add(string.Format("{0} - {1}", proximoAno, proximoPeriodo));
                return dtAno;

            }

            //Se o período atual do aluno for ‘1’ então o valor inicial é Ano Atual  + ‘- 2’.
            if (matriculaAtiva.Semestre == 1)
            {
                proximoAno = Convert.ToString(matriculaAtiva.Ano);
                proximoPeriodo = "2";
            }

            //Se o período atual do aluno for ‘2’ então o valor inicial é Ano Atual +1 + ‘- 1’.
            if (matriculaAtiva.Semestre == 2)
            {
                proximoAno = Convert.ToString(matriculaAtiva.Ano + 1);
                proximoPeriodo = "0";

                AnoPeriodo = string.Format("{0} - {1}", proximoAno, proximoPeriodo);
                dtAno.Rows.Add(AnoPeriodo);

                proximoAno = Convert.ToString(matriculaAtiva.Ano + 1);
                proximoPeriodo = "1";

                AnoPeriodo = string.Format("{0} - {1}", proximoAno, proximoPeriodo);
                dtAno.Rows.Add(AnoPeriodo);
                return dtAno;
            }

            AnoPeriodo = string.Format("{0} - {1}", proximoAno, proximoPeriodo);
            dtAno.Rows.Add(AnoPeriodo);
            return dtAno;
        }

        public bool PossuiConfirmacaoMatriculaConfirmadaPor(DataContext ctx, string aluno, string curso, decimal serie, string turno, int ano, int periodo, string censo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM   TCE_CONFIRMACAO_MATRICULA (NOLOCK) 
                                WHERE  ALUNO = @ALUNO 
                                       AND CENSO = @CENSO 
                                       AND CURSO = @CURSO 
                                       AND SERIE = @SERIE 
                                       AND TURNO = @TURNO 
                                       AND ANO = @ANO 
                                       AND PERIODO = @PERIODO 
                                       AND STATUS = @STATUS  ";

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@CENSO", censo);
            contextQuery.Parameters.Add("@CURSO", curso);
            contextQuery.Parameters.Add("@SERIE", serie);
            contextQuery.Parameters.Add("@TURNO", turno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@STATUS", Confirmado);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiConfirmacaoMatriculaConfirmadaPor(DataContext ctx, List<string> alunos, string curso, decimal serie, string turno, int ano, int periodo, string censo)
        {
            string matriculas = alunos.Aggregate((x, y) => x + ", " + y);

            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = string.Format(@" SELECT COUNT(*) 
                                FROM   TCE_CONFIRMACAO_MATRICULA (NOLOCK) 
                                WHERE  ALUNO IN ({0})
                                       AND CENSO = @CENSO 
                                       AND CURSO = @CURSO 
                                       AND SERIE = @SERIE 
                                       AND TURNO = @TURNO 
                                       AND ANO = @ANO 
                                       AND PERIODO = @PERIODO 
                                       AND STATUS = @STATUS  ", matriculas);

            contextQuery.Parameters.Add("@CENSO", censo);
            contextQuery.Parameters.Add("@CURSO", curso);
            contextQuery.Parameters.Add("@SERIE", serie);
            contextQuery.Parameters.Add("@TURNO", turno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@STATUS", Confirmado);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiConfirmacaoMatriculaConfirmadaEmPossiveisPeriodosPor(DataContext ctx, string aluno, int ano, int periodo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;
            string possiveisPeriodos = Utils.RecuperaPossiveisPeriodosCompleto(periodo);

            contextQuery.Command = string.Format(@"SELECT count(*)
                            FROM    TCE_CONFIRMACAO_MATRICULA (NOLOCK)
                            WHERE  ALUNO = @ALUNO
                            AND STATUS = @STATUS
                            AND ANO = @ANO
                            AND PERIODO IN ( {0} )", possiveisPeriodos);

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@STATUS", Confirmado);
            contextQuery.Parameters.Add("@ANO", ano);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiConfirmacaoMatriculaConfirmadaEmProximosPeriodosPor(DataContext ctx, List<string> alunos, int ano, int periodo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;
            string possiveisPeriodos;
            int proximoAno;

            if (periodo == 1)
            {
                //1 so pode considerar como futuro 2 do mesmo ano 
                possiveisPeriodos = "2";
                proximoAno = ano;
            }
            else
            {
                //0 pode considerar como futuro 0 ou 1 do proximo ano
                //2 considerar como futuro 0 ou 1 do proximo ano
                possiveisPeriodos = "0 , 1";
                proximoAno = ano + 1;
            }

            string matriculas = alunos.Aggregate((x, y) => x + ", " + y);

            contextQuery.Command = string.Format(@"SELECT count(*)
                            FROM    TCE_CONFIRMACAO_MATRICULA (NOLOCK)
                            WHERE  ALUNO IN ({0})
                            AND STATUS = @STATUS
                            AND ANO = @ANO
                            AND PERIODO IN ( {1} )", matriculas, possiveisPeriodos);

            contextQuery.Parameters.Add("@STATUS", Confirmado);
            contextQuery.Parameters.Add("@ANO", proximoAno);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool ExisteOutraConfirmacaoConfirmadaPor(string aluno, int ano, string periodosPossiveis, int idConfirmacaoMatricula)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool duplicidade = false;

            try
            {
                contextQuery.Command = string.Format(@" SELECT  COUNT(*)
                        FROM    DBO.TCE_CONFIRMACAO_MATRICULA
                        WHERE   ANO = @ANO
                                AND PERIODO IN ( {0} )
                                AND ALUNO = @ALUNO
                                AND STATUS = @STATUS
                                AND ID_CONFIRMACAO_MATRICULA <> @ID_CONFIRMACAO_MATRICULA ", periodosPossiveis);

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@STATUS", Confirmado);
                contextQuery.Parameters.Add("@ID_CONFIRMACAO_MATRICULA", idConfirmacaoMatricula);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    duplicidade = true;
                }

                return duplicidade;
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

        public ValidacaoDados ValidaLiberacaoPor(DadosLiberacaoConfirmacao liberacao)
        {
            List<string> mensagens = new List<string>();
            RN.Nota rnNota = new Nota();
            RN.Falta rnFalta = new Falta();
            bool possuiOutraConfirmacada = false;
            string periodosPossiveis = string.Empty;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (liberacao == null)
            {
                return validacaoDados;
            }

            if (liberacao.IdConfirmacaoMatricula <= 0)
            {
                mensagens.Add("Favor informar o Id da Confirmação.");
            }

            if (liberacao.Ano <= 0)
            {
                mensagens.Add("Favor informar o ano da Confirmação.");
            }

            if (liberacao.Periodo < 0)
            {
                mensagens.Add("Favor informar o periodo da Confirmação.");
            }

            if (string.IsNullOrEmpty(liberacao.Aluno))
            {
                mensagens.Add("Favor informar o aluno.");
            }

            if (string.IsNullOrEmpty(liberacao.SituacaoAtual))
            {
                mensagens.Add("Favor informar a Matrícula do responsável pela liberação.");
            }

            //Verificar status atual da confirmação
            if (string.IsNullOrEmpty(liberacao.SituacaoAtual) || liberacao.SituacaoAtual == "Pendente")
            {
                //Não liberar confirmações que já estejam pendentes
                mensagens.Add("A confirmação não pode ser liberada, pois já está com situação Pendente");
            }

            if (mensagens.Count == 0)
            {
                //Monta periodos possiveis
                if (liberacao.Periodo == 0)
                {
                    periodosPossiveis = "0, 1, 2";
                }
                else if (liberacao.Periodo == 1)
                {
                    periodosPossiveis = "0, 1";
                }
                else
                {
                    periodosPossiveis = "0, 2";
                }

                //Verifica se aluno possui outra confirmacao confirmada
                if (this.ExisteOutraConfirmacaoConfirmadaPor(liberacao.Aluno, liberacao.Ano, periodosPossiveis, liberacao.IdConfirmacaoMatricula))
                {
                    possuiOutraConfirmacada = true;
                }

                if (liberacao.SituacaoAtual == Confirmado)
                {
                    //Caso a confirmação esteja Confirmada                    

                    //Verifica duplicidade de confirmacao
                    if (possuiOutraConfirmacada)
                    {
                        mensagens.Add("A confirmação não pode ser liberada porque o aluno possui duas confirmações com status de confirmado, favor entrar em contato com a SEEDUC para resolver a situação deste aluno.");
                    }
                    //Verifica se possui notas ou faltas em matricula equivalente aquela confirmacao.
                    if (rnNota.PossuiNotaEmPeridosPossiveisPor(liberacao.Aluno, liberacao.Ano, periodosPossiveis) || rnFalta.PossuiFaltaEmPeridosPossiveisPor(liberacao.Aluno, liberacao.Ano, periodosPossiveis))
                    {
                        mensagens.Add("A confirmação não pode ser liberada porque o aluno já possui notas e/ou faltas lançadas, será necessário efetuar sua transferência.");
                    }
                }
                else if (liberacao.SituacaoAtual == NaoConfirmado)
                {
                    //Caso a confirmação esteja Não Confirmada

                    //Verifica se aluno possui outra confirmação de matricula Confirmada
                    if (possuiOutraConfirmacada)
                    {
                        mensagens.Add("É necessário primeiro liberar o registro de confirmação com status “Confirmado” que existe para o mesmo ano e período.");
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

        public void LiberaPor(DadosLiberacaoConfirmacao liberacao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            RN.Matricula rnMatricula = new Matricula();
            RN.Matgrade rnMatgrade = new Matgrade();
            string periodosPossiveis = string.Empty;

            try
            {
                //Verifica se a confirmação que será liberada esta confirmada
                if (liberacao.SituacaoAtual == Confirmado)
                {
                    //Monta periodos possiveis
                    if (liberacao.Periodo == 0)
                    {
                        periodosPossiveis = "0, 1, 2";
                    }
                    else if (liberacao.Periodo == 1)
                    {
                        periodosPossiveis = "0, 1";
                    }
                    else
                    {
                        periodosPossiveis = "0, 2";
                    }

                    //Cancela matriculas
                    rnMatricula.LiberaMatriculasEmPeridosPossiveisPor(ctx, liberacao, periodosPossiveis);

                    //Cancela matgrade
                    rnMatgrade.LiberaMatgradeEmPeridosPossiveisPor(ctx, liberacao, periodosPossiveis);
                }

                //Libera confirmacao matricula
                this.LiberaConfirmacaoMatricula(ctx, liberacao.IdConfirmacaoMatricula, liberacao.MatriculaResponsavel);
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

        public void LiberaConfirmacaoMatricula(DataContext ctx, int idConfirmacaoMatricula, string matriculaResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE  TCE_CONFIRMACAO_MATRICULA
                            SET     STATUS = NULL ,
                                    MATRICULA = @MATRICULA ,
                                    DT_ALTERACAO = NULL 
                            WHERE   ID_CONFIRMACAO_MATRICULA = @ID_CONFIRMACAO_MATRICULA ";

                contextQuery.Parameters.Add("@MATRICULA", matriculaResponsavel);
                contextQuery.Parameters.Add("@ID_CONFIRMACAO_MATRICULA", idConfirmacaoMatricula);

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

        public TceConfirmacaoMatricula ObtemUltimaConfirmacaoPor(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            TceConfirmacaoMatricula ultimaConfirmacao = new TceConfirmacaoMatricula();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT TOP 1
                                        *
                                FROM    DBO.TCE_CONFIRMACAO_MATRICULA
                                WHERE   ALUNO = @ALUNO
                                ORDER BY ID_CONFIRMACAO_MATRICULA DESC "
                };

                contextQuery.Parameters.Add("@ALUNO", aluno);

                ultimaConfirmacao = ctx.TryToBindEntity<TceConfirmacaoMatricula>(contextQuery);

                return ultimaConfirmacao;
            }

            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public static DataTable VerificaConfirmacaoMatriculaDestino(RN.DTOs.DadosTransferencia dadosTransferencia)
        {
            var dataTable = new DataTable();

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT * 
                                         FROM TCE_CONFIRMACAO_MATRICULA (NOLOCK)
                                         WHERE  ALUNO = @ALUNO 
	                                          AND CENSO = @CENSO
	                                          AND CURSO = @CURSO
	                                          AND SERIE = @SERIE
	                                          AND TURNO = @TURNO
	                                          AND ANO = @ANO 
	                                          AND PERIODO = @PERIODO ";

                contextQuery.Parameters.Add("@ANO", dadosTransferencia.Ano);
                contextQuery.Parameters.Add("@PERIODO", dadosTransferencia.SemestreDestino);
                contextQuery.Parameters.Add("@ALUNO", dadosTransferencia.Aluno);
                contextQuery.Parameters.Add("@CENSO", dadosTransferencia.UnidadeEnsino);
                contextQuery.Parameters.Add("@CURSO", dadosTransferencia.CursoDestino);
                contextQuery.Parameters.Add("@SERIE", dadosTransferencia.SerieDestino);
                contextQuery.Parameters.Add("@TURNO", dadosTransferencia.TurnoDestino);

                dataTable = ctx.GetDataTable(contextQuery);
            }

            return dataTable;
        }

        public bool ExisteConfirmacaoPor(string aluno, int ano, int periodo, string situacao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            string possiveisPeriodos = Utils.RecuperaPossiveisPeriodos(periodo);

            try
            {
                contextQuery.Command = string.Format(@" SELECT  COUNT(*)
                            FROM    DBO.TCE_CONFIRMACAO_MATRICULA
                            WHERE   ALUNO = @ALUNO
                                    AND STATUS = @STATUS
                                    AND ANO = @ANO
                                    AND PERIODO IN ( {0} ) ", possiveisPeriodos);

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@STATUS", situacao);
                contextQuery.Parameters.Add("@ANO", ano);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    existe = true;
                }

                return existe;
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

        public bool ExisteConfirmacaoPor(DataContext contexto, string aluno, int ano, int periodo, string censo, string curso, int serie, string turno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @"  SELECT  COUNT(*)
                            FROM    DBO.TCE_CONFIRMACAO_MATRICULA
                            WHERE   ALUNO = @ALUNO
                                    AND ANO = @ANO
                                    AND PERIODO = @PERIODO
                                    AND CENSO = @CENSO 
                                    AND CURSO = @CURSO 
                                    AND SERIE = @SERIE 
                                    AND TURNO = @TURNO ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
            contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);
            contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, turno);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public string ObtemTipoVagaPor(DataContext ctx, string aluno, int ano, int periodo, string censo)
        {
            ContextQuery contextQuery = new ContextQuery();
            string tipoVaga = string.Empty;

            contextQuery.Command = @" SELECT TOP 1
                                    TIPOVAGAOCUPADA
                            FROM    DBO.TCE_CONFIRMACAO_MATRICULA
                            WHERE   ALUNO = @ALUNO
                                    AND ANO = @ANO
                                    AND PERIODO = @PERIODO
                                    AND STATUS = @STATUS
                                    AND CENSO = @CENSO
                            ORDER BY DT_CADASTRO DESC ";

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@STATUS", Confirmado);
            contextQuery.Parameters.Add("@CENSO", censo);

            tipoVaga = ctx.GetReturnValue<string>(contextQuery);

            return tipoVaga;
        }

        public string ObtemTipoVagaParaTransferenciaDePossiveisPeriodosPor(DataContext ctx, string aluno, int ano, int periodo)
        {
            string tipoVaga = string.Empty;
            string possiveisPeriodos = string.Empty;

            //Verificar possiveis periodos com relação:         
            if (periodo == 2)
            {
                //2 pode transferir para 0 ou 2  
                possiveisPeriodos = "0, 2";
            }
            else if (periodo == 1)
            {
                //1 pode transferir para 0 ou 1
                possiveisPeriodos = "0, 1";
            }
            else
            {
                //0 pode transferir para 0 ou 1 ou 2
                possiveisPeriodos = "0, 1, 2";
            }

            ContextQuery contextQuery = new ContextQuery
            {
                Command = string.Format(@" SELECT TOP 1
                                    TIPOVAGAOCUPADA
                            FROM    DBO.TCE_CONFIRMACAO_MATRICULA
                            WHERE   ALUNO = @ALUNO
                                    AND ANO = @ANO
                                    AND PERIODO IN ( {0} )
                                    AND STATUS = @STATUS
                            ORDER BY DT_CADASTRO DESC ", possiveisPeriodos)
            };

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@STATUS", Confirmado);

            tipoVaga = ctx.GetReturnValue<string>(contextQuery);

            return tipoVaga;
        }

        public void GeraConfirmacaoMatriculaDeRenovacaoPor(DataContext context, Turma.DadosTurma dadosTurmaOrigem, string situacaoFinalAluno, string aluno, decimal proximoAno, decimal proximoPeriodo, string matriculaUsuario)
        {
            RN.RenovacaoMatricula.Renovacao rnRenovacao = new RN.RenovacaoMatricula.Renovacao();
            RN.ControleVaga rnControleVaga = new ControleVaga();
            RN.RenovacaoMatricula.Entidades.Renovacao renovacao = new RN.RenovacaoMatricula.Entidades.Renovacao();
            TceConfirmacaoMatricula confirmacaoMatricula = new TceConfirmacaoMatricula();
            int situacaoAtiva = (int)RN.RenovacaoMatricula.Entidades.SituacaoRenovacao.Ativo;
            int situacaoPossuiConfirmacao = (int)RN.RenovacaoMatricula.Entidades.SituacaoRenovacao.PossuiConfirmacao;
            string curriculo = string.Empty;
            int vagasLiberadas = 0;
            int vagasUtilizadas = 0;
            RN.RenovacaoMatricula.RenovacaoConfirmacaoMatricula rnRenovacaoConfirmacaoMatricula = new RN.RenovacaoMatricula.RenovacaoConfirmacaoMatricula();
            Matricula rnMatricula = new Matricula();
            DataTable confirmacaoOrigem = null;
            Aluno rnAluno = new Aluno();
            RN.Aluno.DadosAluno dadosAluno = new Aluno.DadosAluno();
            RN.Curso rnCurso = new Curso();

            try
            {
                //Busca dados do aluno
                dadosAluno = rnAluno.ObtemDadosAluno(aluno);

                //Carregar renovação do aluno
                renovacao = rnRenovacao.ObtemRenovacaoPor(aluno, situacaoAtiva, Convert.ToInt32(proximoAno), Convert.ToInt32(proximoPeriodo));

                //Caso aquele aluno possua renovação gerar confirmação
                if (renovacao.RenovacaoId > 0)
                {
                    //Busca curriculo para os dados escolhidos pelo aluno (por regra usar top 1)
                    curriculo = RN.Curriculo.RetornaCurriculo(renovacao.CursoId, renovacao.TurnoId, renovacao.Ano, renovacao.Periodo);

                    //Caso não exista curriculo para os dados escolhidos pela aluno não gera renovação
                    if (string.IsNullOrEmpty(curriculo))
                    {
                        throw new Exception(String.Format("ERRO_VALIDACAO:Para o aluno: {0} - {1} não foi possivel gerar confirmação pois para o ano: {2}, periodo: {3}, curso: {4}, turno: {5} escolhido na renovação não existe curriculo relacionado.",
                            dadosAluno.Aluno,
                            dadosAluno.Nome_compl,
                            renovacao.Ano,
                            renovacao.Periodo,
                            renovacao.CursoId,
                            renovacao.TurnoId));
                    }

                    //Criar entidade de confirmação
                    confirmacaoMatricula.Aluno = renovacao.AlunoId;
                    confirmacaoMatricula.Censo = renovacao.UnidadeEnsinoId;
                    confirmacaoMatricula.Ano = renovacao.Ano;
                    confirmacaoMatricula.Periodo = renovacao.Periodo;
                    confirmacaoMatricula.Curso = renovacao.CursoId;
                    confirmacaoMatricula.Serie = renovacao.Serie;
                    confirmacaoMatricula.Turno = renovacao.TurnoId;
                    confirmacaoMatricula.Curriculo = curriculo;
                    confirmacaoMatricula.DtSugerida = DateTime.Now;
                    confirmacaoMatricula.EnsinoReligioso = renovacao.EnsinoReligioso;
                    confirmacaoMatricula.LinguaEstrangeiraFacultativa = renovacao.LinguaEstrangeira;
                    confirmacaoMatricula.ProjetoAutonomia = false; //Como regra criar false
                    confirmacaoMatricula.Matricula = matriculaUsuario;
                    confirmacaoMatricula.DtCadastro = renovacao.DataCadastro; //Como regra a data de cadastro será a data de cadastro da renovação
                    confirmacaoMatricula.DtAlteracao = DateTime.Now;
                    confirmacaoMatricula.TipoVagaOcupada = renovacao.TipoVaga;
                    confirmacaoMatricula.Observacao = "MIGRAÇÃO RENOVAÇÃO";

                    //Verifica se o aluno possui outra confirmação confirmada
                    if (ExisteConfirmacaoPor(aluno, renovacao.Ano, renovacao.Periodo, RN.ConfirmacaoMatricula.Confirmado))
                    {
                        //Caso possui a confirmação gerada será como Não confirmada
                        confirmacaoMatricula.Status = RN.ConfirmacaoMatricula.NaoConfirmado;
                    }
                    else
                    {
                        //Verifica se o aluno possui outra confirmação pendente
                        if (ExisteConfirmacaoPor(aluno, renovacao.Ano, renovacao.Periodo, RN.ConfirmacaoMatricula.Pendente))
                        {
                            //Caso possui a confirmação gerada será como pendente
                            confirmacaoMatricula.Status = RN.ConfirmacaoMatricula.Pendente;
                        }
                        else
                        {
                            //Caso contrario será gerado como confirmado
                            confirmacaoMatricula.Status = RN.ConfirmacaoMatricula.Confirmado;
                        }
                    }

                    //Verificar vaga apenas para confirmações com situação confirmada
                    if (confirmacaoMatricula.Status == RN.ConfirmacaoMatricula.Confirmado)
                    {
                        //Verificar se tem vaga no curso / serie / turno / ano / semestre
                        vagasLiberadas = rnControleVaga.ObtemVagasLiberadasTotalPor(context,
                            renovacao.UnidadeEnsinoId,
                            renovacao.Ano,
                            renovacao.Periodo,
                            renovacao.Serie,
                            renovacao.CursoId,
                            renovacao.TurnoId);

                        vagasUtilizadas = rnControleVaga.ObtemVagasUtilizadasTotalPor(context,
                            renovacao.UnidadeEnsinoId,
                            renovacao.Ano,
                            renovacao.Periodo,
                            renovacao.Serie,
                            renovacao.CursoId,
                            renovacao.TurnoId);

                        if (vagasLiberadas <= vagasUtilizadas)
                        {
                            //Caso não existe deixa confirmação como pendente
                            confirmacaoMatricula.Status = RN.ConfirmacaoMatricula.Pendente;
                            confirmacaoMatricula.Observacao = "NÃO EXISTE VAGAS PARA GERAR CONFIRMAÇAO DE RENOVAÇÃO";
                        }
                    }

                    //Inserir confirmação
                    this.InsereConfirmacaoMatricula(context, confirmacaoMatricula);

                    if (!rnRenovacao.PossuiRenovacaoAtivaPor(context, renovacao.Ano, renovacao.Periodo, renovacao.UnidadeEnsinoId, renovacao.CursoId, renovacao.TurnoId, renovacao.Serie, renovacao.TipoVaga))
                    {
                        throw new Exception(String.Format("ERRO_VALIDACAO:Para o aluno: {0} - {1} não foi possivel gerar o relacionamento de renovação com confirmação pois para o ano.",
                                    dadosAluno.Aluno,
                                    dadosAluno.Nome_compl
                                   ));
                    }

                    if (!rnRenovacaoConfirmacaoMatricula.PossuiRenovacaoConfirmacaoMatriculaPor(context, confirmacaoMatricula, renovacao))
                    {
                        //Inserir tabela de relacionamento
                        rnRenovacaoConfirmacaoMatricula.InsereRenovacaoConfirmacaoMatricula(context, confirmacaoMatricula, renovacao);
                    }

                    //Atualizar situação da renovação
                    renovacao.SituacaoRenovacaoId = situacaoPossuiConfirmacao;
                    renovacao.DataAlteracao = DateTime.Now;
                    renovacao.Usuario = matriculaUsuario;
                    rnRenovacao.AtualizaRenovacao(context, renovacao);
                }
                else
                {
                    if (situacaoFinalAluno != FechamentoMatricula.Aprovado && situacaoFinalAluno != FechamentoMatricula.Promovido)
                    {
                        //verifica se o aluno é de turma concluinte do ensino medio, caso seja gerar confirmação
                        if (rnMatricula.EhMatriculaRegularConcluinte(aluno, Convert.ToInt32(dadosTurmaOrigem.Ano), Convert.ToInt32(dadosTurmaOrigem.Periodo), dadosTurmaOrigem.Grade))
                        {
                            if (!((((Convert.ToDecimal(dadosTurmaOrigem.Ano) == 2022 || Convert.ToDecimal(dadosTurmaOrigem.Ano) == 2023) && Convert.ToDecimal(dadosTurmaOrigem.Periodo) == 0 && dadosTurmaOrigem.Curso == "0002.31" && (dadosTurmaOrigem.Serie == "2" || dadosTurmaOrigem.Serie == "3")) ||
                            ((Convert.ToDecimal(dadosTurmaOrigem.Ano) == 2022 || Convert.ToDecimal(dadosTurmaOrigem.Ano) == 2023) && Convert.ToDecimal(dadosTurmaOrigem.Periodo) == 2 && dadosTurmaOrigem.Curso == "0002.83" && (dadosTurmaOrigem.Serie == "3" )) ||
                             (rnCurso.EhItinerarioFormativoTrihaComMatrizPor(dadosTurmaOrigem.Curso, Convert.ToDecimal(dadosTurmaOrigem.Ano), Convert.ToDecimal(dadosTurmaOrigem.Periodo))
                             && ((Convert.ToDecimal(dadosTurmaOrigem.Periodo) == 0 && dadosTurmaOrigem.Serie == "2") ||
                                 (Convert.ToDecimal(dadosTurmaOrigem.Periodo) == 2 && (dadosTurmaOrigem.Serie == "3" || dadosTurmaOrigem.Serie == "4")))
                           )
                             )
                            ))
                            {

                                confirmacaoOrigem = ObtemConfirmacaoMatriculaPor(aluno, Convert.ToInt32(dadosTurmaOrigem.Ano), Convert.ToInt32(dadosTurmaOrigem.Periodo));

                                curriculo = RN.Curriculo.RetornaCurriculo(dadosTurmaOrigem.Curso, dadosTurmaOrigem.Turno, Convert.ToInt32(proximoAno), Convert.ToInt32(proximoPeriodo));

                                if (string.IsNullOrEmpty(curriculo))
                                {
                                    throw new Exception(String.Format("ERRO_VALIDACAO:Para o aluno: {0} - {1} não foi possivel gerar confirmação pois não existe curriculo relacionado.",
                                        dadosAluno.Aluno,
                                        dadosAluno.Nome_compl
                                       ));
                                }

                                //Criar entidade de confirmação                    
                                confirmacaoMatricula.Aluno = aluno;
                                confirmacaoMatricula.Censo = dadosTurmaOrigem.Faculdade;
                                confirmacaoMatricula.Ano = proximoAno;
                                confirmacaoMatricula.Periodo = proximoPeriodo;
                                confirmacaoMatricula.Curso = dadosTurmaOrigem.Curso;
                                confirmacaoMatricula.Serie = Convert.ToDecimal(dadosTurmaOrigem.Serie);
                                confirmacaoMatricula.Turno = dadosTurmaOrigem.Turno;
                                confirmacaoMatricula.Curriculo = curriculo;
                                confirmacaoMatricula.DtSugerida = DateTime.Now;
                                confirmacaoMatricula.EnsinoReligioso = Convert.ToBoolean(confirmacaoOrigem.Rows[0]["ENSINO_RELIGIOSO"]);
                                confirmacaoMatricula.LinguaEstrangeiraFacultativa = Convert.ToBoolean(confirmacaoOrigem.Rows[0]["LINGUA_ESTRANGEIRA_FACULTATIVA"]);
                                confirmacaoMatricula.ProjetoAutonomia = false;
                                confirmacaoMatricula.Matricula = matriculaUsuario;
                                confirmacaoMatricula.DtCadastro = DateTime.Now;
                                confirmacaoMatricula.TipoVagaOcupada = "VC";
                                confirmacaoMatricula.Observacao = "GERADA POR REPROVAÇÃO DO ALUNO CONCLUINTE";
                                confirmacaoMatricula.Status = ConfirmacaoMatricula.Pendente;

                                //criar confirmação
                                InsereConfirmacaoMatricula(context, confirmacaoMatricula);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsereConfirmacaoMatricula(DataContext context, TceConfirmacaoMatricula confirmacaoMatricula)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT  INTO dbo.TCE_CONFIRMACAO_MATRICULA
                                    ( ALUNO ,
                                      CENSO ,
                                      ANO ,
                                      PERIODO ,
                                      CURSO ,
                                      SERIE ,
                                      TURNO ,
                                      CURRICULO ,
                                      DT_SUGERIDA ,
                                      ENSINO_RELIGIOSO ,
                                      LINGUA_ESTRANGEIRA_FACULTATIVA ,
                                      PROJETO_AUTONOMIA ,
                                      STATUS ,
                                      MATRICULA ,
                                      DT_CADASTRO ,
                                      DT_ALTERACAO ,
                                      TIPOVAGAOCUPADA ,
                                      OBSERVACAO
                                    )
                            VALUES  ( @ALUNO ,
                                      @CENSO ,
                                      @ANO ,
                                      @PERIODO ,
                                      @CURSO ,
                                      @SERIE ,
                                      @TURNO ,
                                      @CURRICULO ,
                                      @DT_SUGERIDA ,
                                      @ENSINO_RELIGIOSO ,
                                      @LINGUA_ESTRANGEIRA_FACULTATIVA ,
                                      @PROJETO_AUTONOMIA ,
                                      @STATUS ,
                                      @MATRICULA ,
                                      @DT_CADASTRO ,
                                      @DT_ALTERACAO ,
                                      @TIPOVAGAOCUPADA ,
                                      @OBSERVACAO
                                    ) ";

                contextQuery.Parameters.Add("@ALUNO", confirmacaoMatricula.Aluno);
                contextQuery.Parameters.Add("@CENSO", confirmacaoMatricula.Censo);
                contextQuery.Parameters.Add("@ANO", confirmacaoMatricula.Ano);
                contextQuery.Parameters.Add("@PERIODO", confirmacaoMatricula.Periodo);
                contextQuery.Parameters.Add("@CURSO", confirmacaoMatricula.Curso);
                contextQuery.Parameters.Add("@SERIE", confirmacaoMatricula.Serie);
                contextQuery.Parameters.Add("@TURNO", confirmacaoMatricula.Turno);
                contextQuery.Parameters.Add("@CURRICULO", confirmacaoMatricula.Curriculo);
                contextQuery.Parameters.Add("@DT_SUGERIDA", confirmacaoMatricula.DtSugerida);
                contextQuery.Parameters.Add("@ENSINO_RELIGIOSO", confirmacaoMatricula.EnsinoReligioso);
                contextQuery.Parameters.Add("@LINGUA_ESTRANGEIRA_FACULTATIVA", confirmacaoMatricula.LinguaEstrangeiraFacultativa);
                contextQuery.Parameters.Add("@PROJETO_AUTONOMIA", confirmacaoMatricula.ProjetoAutonomia);
                contextQuery.Parameters.Add("@STATUS", confirmacaoMatricula.Status);
                contextQuery.Parameters.Add("@MATRICULA", confirmacaoMatricula.Matricula);
                contextQuery.Parameters.Add("@DT_CADASTRO", confirmacaoMatricula.DtCadastro);
                if (confirmacaoMatricula.Status != ConfirmacaoMatricula.Pendente)
                {
                    contextQuery.Parameters.Add("@DT_ALTERACAO", confirmacaoMatricula.DtAlteracao);
                }
                else
                {
                    contextQuery.Parameters.Add("@DT_ALTERACAO", null);
                }
                contextQuery.Parameters.Add("@TIPOVAGAOCUPADA", confirmacaoMatricula.TipoVagaOcupada);
                contextQuery.Parameters.Add("@OBSERVACAO", confirmacaoMatricula.Observacao);

                context.ApplyModifications(contextQuery);

                contextQuery = new ContextQuery(string.Format(@" 
                    SELECT  ID_CONFIRMACAO_MATRICULA
                    FROM    dbo.TCE_CONFIRMACAO_MATRICULA (NOLOCK)
                    WHERE   CENSO = @CENSO
                            AND ANO = @ANO
                            AND PERIODO = @PERIODO
                            AND CURSO = @CURSO
                            AND SERIE = @SERIE
                            AND TURNO = @TURNO
                            AND CURRICULO = @CURRICULO
                            AND ALUNO = @ALUNO
                            AND STATUS {0}                                           
                              ", string.IsNullOrEmpty(confirmacaoMatricula.Status) ? " is null" : " =@STATUS"));

                contextQuery.Parameters.Add("@ALUNO", confirmacaoMatricula.Aluno);
                contextQuery.Parameters.Add("@CENSO", confirmacaoMatricula.Censo);
                contextQuery.Parameters.Add("@ANO", confirmacaoMatricula.Ano);
                contextQuery.Parameters.Add("@PERIODO", confirmacaoMatricula.Periodo);
                contextQuery.Parameters.Add("@CURSO", confirmacaoMatricula.Curso);
                contextQuery.Parameters.Add("@SERIE", confirmacaoMatricula.Serie);
                contextQuery.Parameters.Add("@TURNO", confirmacaoMatricula.Turno);
                contextQuery.Parameters.Add("@CURRICULO", confirmacaoMatricula.Curriculo);

                if (!string.IsNullOrEmpty(confirmacaoMatricula.Status))
                {
                    contextQuery.Parameters.Add("@STATUS", confirmacaoMatricula.Status);
                }

                confirmacaoMatricula.IdConfirmacaoMatricula = Convert.ToInt32(context.GetReturnValue(contextQuery));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsereAlunoPreCadastro(DataContext contexto, int controleVagaId, string aluno, string curriculo, string tipoVaga, bool ensinoReligioso, bool linguaEstrangeiraFacultativa, string usuarioResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO DBO.TCE_CONFIRMACAO_MATRICULA 
                                                    (ALUNO, 
                                                     CENSO, 
                                                     ANO, 
                                                     PERIODO, 
                                                     CURSO, 
                                                     SERIE, 
                                                     TURNO, 
                                                     CURRICULO, 
                                                     DT_SUGERIDA, 
                                                     ENSINO_RELIGIOSO, 
                                                     LINGUA_ESTRANGEIRA_FACULTATIVA, 
                                                     PROJETO_AUTONOMIA, 
                                                     STATUS, 
                                                     MATRICULA, 
                                                     DT_CADASTRO, 
                                                     DT_ALTERACAO, 
                                                     TIPOVAGAOCUPADA,
													 MATRICULAFACIL) 
                                        SELECT @ALUNO, 
                                               CENSO, 
                                               ANO, 
                                               PERIODO, 
                                               CURSO, 
                                               SERIE, 
                                               TURNO, 
                                               @CURRICULO, 
                                               @DATATUAL, 
                                               @ENSINO_RELIGIOSO, 
                                               @LINGUA_ESTRANGEIRA_FACULTATIVA,  
                                               0, 
                                               @STATUS, 
                                               @USUARIORESPONSAVEL, 
                                               @DATATUAL, 
                                               @DATATUAL, 
                                               @TIPOVAGAOCUPADA,
                                               1 --Marca como vindo da matricula facil
                                        FROM   TCE_CONTROLE_VAGA CV (NOLOCK) 
                                        WHERE  ID_CONTROLE_VAGA = @ID_CONTROLE_VAGA   ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@CURRICULO", SqlDbType.VarChar, curriculo);
            contextQuery.Parameters.Add("@ENSINO_RELIGIOSO", SqlDbType.Bit, ensinoReligioso);
            contextQuery.Parameters.Add("@LINGUA_ESTRANGEIRA_FACULTATIVA", SqlDbType.Bit, linguaEstrangeiraFacultativa);
            contextQuery.Parameters.Add("@STATUS", SqlDbType.VarChar, Confirmado);
            contextQuery.Parameters.Add("@USUARIORESPONSAVEL", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@TIPOVAGAOCUPADA", SqlDbType.VarChar, tipoVaga);
            contextQuery.Parameters.Add("@DATATUAL", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@ID_CONTROLE_VAGA", SqlDbType.Int, controleVagaId);

            contexto.ApplyModifications(contextQuery);
        }

        public void InsereDuplicidade(DataContext contexto, DadosDuplicidadeAluno dadosDuplicidadeAluno)
        {
            int idConfirmacao = 0;
            ContextQuery contextQuery = new ContextQuery();
            TceConfirmacaoMatricula confirmacaoMatricula = new TceConfirmacaoMatricula();

            //Monta Entidade
            confirmacaoMatricula.Aluno = dadosDuplicidadeAluno.MatriculaCorreta;
            confirmacaoMatricula.Censo = dadosDuplicidadeAluno.Censo;
            confirmacaoMatricula.Ano = dadosDuplicidadeAluno.Ano;
            confirmacaoMatricula.Periodo = dadosDuplicidadeAluno.Periodo;
            confirmacaoMatricula.Curso = dadosDuplicidadeAluno.Curso;
            confirmacaoMatricula.Serie = dadosDuplicidadeAluno.Serie;
            confirmacaoMatricula.Turno = dadosDuplicidadeAluno.Turno;
            confirmacaoMatricula.Status = Confirmado;
            confirmacaoMatricula.EnsinoReligioso = dadosDuplicidadeAluno.EnsinoReligioso;
            confirmacaoMatricula.LinguaEstrangeiraFacultativa = dadosDuplicidadeAluno.LinguaEstrangeiraFacultativa;
            confirmacaoMatricula.ProjetoAutonomia = false;
            confirmacaoMatricula.Matricula = dadosDuplicidadeAluno.UsuarioId;
            confirmacaoMatricula.Curriculo = dadosDuplicidadeAluno.Curriculo;
            confirmacaoMatricula.TipoVagaOcupada = dadosDuplicidadeAluno.TipoVaga;

            idConfirmacao = ObtemIdConfirmacaoMatriculaPor(confirmacaoMatricula.Aluno, confirmacaoMatricula.Ano, confirmacaoMatricula.Periodo, confirmacaoMatricula.Censo, confirmacaoMatricula.Curso, confirmacaoMatricula.Serie, confirmacaoMatricula.Turno);

            if (idConfirmacao > 0)
            {
                confirmacaoMatricula.IdConfirmacaoMatricula = idConfirmacao;
                this.AlteraDados(contexto, confirmacaoMatricula);
            }
            else
            {
                this.Insere(contexto, confirmacaoMatricula);
                idConfirmacao = confirmacaoMatricula.IdConfirmacaoMatricula;
            }
        }

        public void InsereEncaminhamentoEspecial(DataContext contexto, DadosEncaminhamentoEspecial dados, string tipoVaga, out int confirmacaoMatriculaId)
        {
            confirmacaoMatriculaId = 0;
            ContextQuery contextQuery = new ContextQuery();
            TceConfirmacaoMatricula confirmacaoMatricula = new TceConfirmacaoMatricula();

            //Monta Entidade
            confirmacaoMatricula.Aluno = dados.PessoaAluno.Aluno;
            confirmacaoMatricula.Censo = dados.Censo;
            confirmacaoMatricula.Ano = dados.Ano;
            confirmacaoMatricula.Periodo = dados.Periodo;
            confirmacaoMatricula.Curso = dados.Curso;
            confirmacaoMatricula.Serie = dados.Serie;
            confirmacaoMatricula.Turno = dados.Turno;
            confirmacaoMatricula.Status = Confirmado;
            confirmacaoMatricula.EnsinoReligioso = false;
            confirmacaoMatricula.LinguaEstrangeiraFacultativa = false;
            confirmacaoMatricula.ProjetoAutonomia = false;
            confirmacaoMatricula.Matricula = dados.UsuarioResponsavel;
            confirmacaoMatricula.Curriculo = dados.Curriculo;
            confirmacaoMatricula.TipoVagaOcupada = tipoVaga;

            confirmacaoMatriculaId = ObtemIdConfirmacaoMatriculaPor(confirmacaoMatricula.Aluno, confirmacaoMatricula.Ano, confirmacaoMatricula.Periodo, confirmacaoMatricula.Censo, confirmacaoMatricula.Curso, confirmacaoMatricula.Serie, confirmacaoMatricula.Turno);

            if (confirmacaoMatriculaId > 0)
            {
                confirmacaoMatricula.IdConfirmacaoMatricula = confirmacaoMatriculaId;
                this.AlteraDados(contexto, confirmacaoMatricula);
            }
            else
            {
                this.Insere(contexto, confirmacaoMatricula);
                confirmacaoMatriculaId = confirmacaoMatricula.IdConfirmacaoMatricula;
            }
        }

        public void CancelaPossiveisConfirmacaoMatriculaPor(DataContext contexto, string aluno, int ano, int periodo, string matriculaResponsavel, int periodoOrigem, string observacao)
        {
            try
            {
                contexto.ApplyModifications(
                    CancelaPossiveisConfirmacaoMatriculaPor(
                        aluno,
                        ano,
                        periodo,
                        matriculaResponsavel,
                        periodoOrigem,
                        observacao
                    )
                );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CancelaPossiveisConfirmacaoMatriculaPor(string aluno, int ano, int periodo, string matriculaResponsavel, string observacao, List<ContextQuery> listaContextQuery)
        {
            ContextQuery contextQuery = CancelaPossiveisConfirmacaoMatriculaPor(
                aluno,
                ano,
                periodo,
                matriculaResponsavel,
                periodo,
                observacao
            );

            listaContextQuery.Add(contextQuery);
        }

        public ContextQuery CancelaPossiveisConfirmacaoMatriculaPor(string aluno, int ano, int periodo, string matriculaResponsavel, int periodoOrigem, string observacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            string possiveisPeriodos = Utils.RecuperaPossiveisPeriodosCompleto(periodoOrigem);

            contextQuery.Command = string.Format(
                @" UPDATE  TCE_CONFIRMACAO_MATRICULA
                   SET     [STATUS] = @STATUS ,
                           MATRICULA = @MATRICULA ,
                           DT_ALTERACAO = GETDATE(),
                           OBSERVACAO = (CASE LEN(ISNULL(OBSERVACAO,'')) 
					                WHEN 0 THEN @OBSERVACAO
					                ELSE 
						                RTRIM(OBSERVACAO) + ' / ' + @OBSERVACAO 
					                END)
                   WHERE   ALUNO = @ALUNO
                           AND ( [STATUS] = 'CONFIRMADO'
                                 OR [STATUS] IS NULL )
                           AND ANO = @ANO
                           AND PERIODO IN ( {0} ) ", possiveisPeriodos);

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@STATUS", NaoConfirmado);
            contextQuery.Parameters.Add("@MATRICULA", matriculaResponsavel);
            contextQuery.Parameters.Add("@OBSERVACAO", observacao);

            return contextQuery;
        }

        public void CancelaOutrasPossiveisConfirmacaoMatriculaPor(DataContext ctx, string aluno, int ano, string matriculaResponsavel, int periodo, int idConfirmacaoCorreta)
        {
            ContextQuery contextQuery = new ContextQuery();

            string possiveisPeriodos = Utils.RecuperaPossiveisPeriodosCompleto(periodo);

            contextQuery.Command = string.Format(
                @" UPDATE  TCE_CONFIRMACAO_MATRICULA
                   SET     [STATUS] = @STATUS ,
                           MATRICULA = @MATRICULA ,
                           DT_ALTERACAO = GETDATE()
                   WHERE   ALUNO = @ALUNO
                           AND ID_CONFIRMACAO_MATRICULA <> @ID_CONFIRMACAO_MATRICULA
                           AND ( [STATUS] = 'CONFIRMADO'
                                 OR [STATUS] IS NULL )
                           AND ANO = @ANO
                           AND PERIODO IN ( {0} ) ", possiveisPeriodos);

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@STATUS", NaoConfirmado);
            contextQuery.Parameters.Add("@MATRICULA", matriculaResponsavel);
            contextQuery.Parameters.Add("@ID_CONFIRMACAO_MATRICULA", idConfirmacaoCorreta);

            ctx.ApplyModifications(contextQuery);
        }

        public void GeraConfirmacaoMatriculaDeRenovacaoComEnturmacaoPor(DataContext context, Turma.DadosTurma dadosTurmaOrigem, string situacaoFinalAluno, string aluno, decimal proximoAno, decimal proximoPeriodo, string proximoCurso, string proximoTurno, string proximoCurriculo, decimal proximaSerie, string proximaUnidadeEnsino, string matriculaUsuario)
        {
            RN.ControleVaga rnControleVaga = new ControleVaga();
            RN.RenovacaoMatricula.Renovacao rnRenovacao = new RN.RenovacaoMatricula.Renovacao();
            RN.RenovacaoMatricula.Entidades.Renovacao renovacao = new RN.RenovacaoMatricula.Entidades.Renovacao();
            TceConfirmacaoMatricula confirmacaoMatricula = new TceConfirmacaoMatricula();
            int situacaoAtiva = (int)RN.RenovacaoMatricula.Entidades.SituacaoRenovacao.Ativo;
            int situacaoPossuiConfirmacao = (int)RN.RenovacaoMatricula.Entidades.SituacaoRenovacao.PossuiConfirmacao;
            string curriculo = string.Empty;
            int vagasLiberadas = 0;
            int vagasUtilizadas = 0;
            RN.RenovacaoMatricula.RenovacaoConfirmacaoMatricula rnRenovacaoConfirmacaoMatricula = new RN.RenovacaoMatricula.RenovacaoConfirmacaoMatricula();
            Matricula rnMatricula = new Matricula();
            DataTable podeERLE = null;
            Curriculo rnCurriculo = new Curriculo();
            int possuiRenovacaoAutomatica = 0;
            DataTable unidadesRenovacaoAutomatica = new DataTable();
            string tipoVaga = string.Empty;
            Aluno rnAluno = new Aluno();
            RN.Aluno.DadosAluno dadosAluno = new Aluno.DadosAluno();

            try
            {
                //Busca dados do aluno
                dadosAluno = rnAluno.ObtemDadosAluno(aluno);

                //Verifica tipo Vaga para gerar confirmação / renovação
                if (proximaUnidadeEnsino != dadosTurmaOrigem.Faculdade)
                {
                    tipoVaga = "VN";
                }
                else
                {
                    tipoVaga = "VC";
                }

                //Carregar renovação do aluno
                renovacao = rnRenovacao.ObtemRenovacaoPor(aluno, situacaoAtiva, Convert.ToInt32(proximoAno), Convert.ToInt32(proximoPeriodo));

                //Carrega em cache a lista com escolas com renovação automativa
                unidadesRenovacaoAutomatica = (DataTable)RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.UnidadesEnsinoRenovacaoAutomatica, RN.RenovacaoMatricula.UnidadeEnsinoRenovacaoAutomatica.QueryListaUnidadesEnsinoRenovacaoAutomatica);

                //Verificar se aluno está sendo enturmado em uma escola com renovação automatica
                possuiRenovacaoAutomatica = unidadesRenovacaoAutomatica.Select("UNIDADEENSINOID = " + proximaUnidadeEnsino).Count();

                //Verifica opções facultativas do proximo curriculo
                podeERLE = rnCurriculo.ObtemPodeEnsinoReligiosoLinguaEstrangPor(proximoCurriculo, proximoCurso, proximoTurno, Convert.ToInt32(proximoAno), Convert.ToInt32(proximoPeriodo));


                if (renovacao.RenovacaoId == 0)
                {
                    //Para alunos de escolas com renovação automatica de matricula, gerar com dados de destino
                    renovacao.AlunoId = aluno;
                    renovacao.Ano = Convert.ToInt32(proximoAno);
                    renovacao.CursoId = proximoCurso;
                    renovacao.UnidadeEnsinoId = proximaUnidadeEnsino;
                    renovacao.TurnoId = proximoTurno;
                    renovacao.Periodo = Convert.ToInt32(proximoPeriodo);
                    renovacao.Serie = Convert.ToInt32(proximaSerie);
                    renovacao.SituacaoRenovacaoId = situacaoAtiva;
                    renovacao.Usuario = matriculaUsuario;
                    renovacao.TipoVaga = tipoVaga;
                    renovacao.EnsinoReligioso = Convert.ToBoolean(podeERLE.Rows[0]["PODE_ENSINO_RELIGIOSO"]);
                    renovacao.LinguaEstrangeira = Convert.ToBoolean(podeERLE.Rows[0]["PODE_LINGUA_ESTRANGEIRA"]);
                    renovacao.DataCadastro = DateTime.Now;


                }

                //Caso aquele aluno possua renovação gerar confirmação
                if (renovacao.RenovacaoId > 0 || possuiRenovacaoAutomatica > 0)
                {
                    //Busca curriculo para os dados escolhidos pelo aluno (por regra usar top 1)
                    curriculo = RN.Curriculo.RetornaCurriculo(renovacao.CursoId, renovacao.TurnoId, renovacao.Ano, renovacao.Periodo);

                    //Caso não exista curriculo para os dados escolhidos pela aluno não gera renovação
                    if (string.IsNullOrEmpty(curriculo))
                    {
                        throw new Exception(String.Format("ERRO_VALIDACAO:Para o aluno: {0} - {1} não foi possivel gerar confirmação pois para o ano: {2}, periodo: {3}, curso: {4}, turno: {5} escolhido na renovação não existe curriculo relacionado.",
                            dadosAluno.Aluno,
                            dadosAluno.Nome_compl,
                            renovacao.Ano,
                            renovacao.Periodo,
                            renovacao.CursoId,
                            renovacao.TurnoId));
                    }

                    //Criar entidade de confirmação
                    confirmacaoMatricula.Aluno = renovacao.AlunoId;
                    confirmacaoMatricula.Censo = renovacao.UnidadeEnsinoId;
                    confirmacaoMatricula.Ano = renovacao.Ano;
                    confirmacaoMatricula.Periodo = renovacao.Periodo;
                    confirmacaoMatricula.Curso = renovacao.CursoId;
                    confirmacaoMatricula.Serie = renovacao.Serie;
                    confirmacaoMatricula.Turno = renovacao.TurnoId;
                    confirmacaoMatricula.Curriculo = curriculo;
                    confirmacaoMatricula.DtSugerida = DateTime.Now;
                    confirmacaoMatricula.EnsinoReligioso = renovacao.EnsinoReligioso;
                    confirmacaoMatricula.LinguaEstrangeiraFacultativa = renovacao.LinguaEstrangeira;
                    confirmacaoMatricula.ProjetoAutonomia = false; //Como regra criar false
                    confirmacaoMatricula.Matricula = matriculaUsuario;
                    confirmacaoMatricula.DtCadastro = renovacao.DataCadastro; //Como regra a data de cadastro será a data de cadastro da renovação
                    confirmacaoMatricula.DtAlteracao = DateTime.Now;
                    confirmacaoMatricula.TipoVagaOcupada = renovacao.TipoVaga;
                    confirmacaoMatricula.Observacao = "MIGRAÇÃO RENOVAÇÃO";

                    //Verifica se o aluno possui outra confirmação confirmada
                    if (ExisteConfirmacaoPor(aluno, renovacao.Ano, renovacao.Periodo, RN.ConfirmacaoMatricula.Confirmado))
                    {
                        //Caso possui a confirmação gerada será como Não confirmada
                        confirmacaoMatricula.Status = RN.ConfirmacaoMatricula.NaoConfirmado;
                    }
                    else
                    {
                        //Verifica se o aluno possui outra confirmação pendente
                        if (ExisteConfirmacaoPor(aluno, renovacao.Ano, renovacao.Periodo, RN.ConfirmacaoMatricula.Pendente))
                        {
                            //Caso possui a confirmação gerada será como pendente
                            confirmacaoMatricula.Status = RN.ConfirmacaoMatricula.Pendente;
                        }
                        else
                        {
                            //Caso contrario será gerado como confirmado
                            confirmacaoMatricula.Status = RN.ConfirmacaoMatricula.Confirmado;
                        }
                    }

                    //Verificar vaga apenas para confirmações com situação confirmada
                    if (confirmacaoMatricula.Status == RN.ConfirmacaoMatricula.Confirmado)
                    {
                        //Verificar se tem vaga no curso / serie / turno / ano / semestre
                        vagasLiberadas = rnControleVaga.ObtemVagasLiberadasTotalPor(context,
                            renovacao.UnidadeEnsinoId,
                            renovacao.Ano,
                            renovacao.Periodo,
                            renovacao.Serie,
                            renovacao.CursoId,
                            renovacao.TurnoId);

                        vagasUtilizadas = rnControleVaga.ObtemVagasUtilizadasTotalPor(context,
                            renovacao.UnidadeEnsinoId,
                            renovacao.Ano,
                            renovacao.Periodo,
                            renovacao.Serie,
                            renovacao.CursoId,
                            renovacao.TurnoId);

                        if (vagasLiberadas <= vagasUtilizadas)
                        {
                            //Caso não existe deixa confirmação como pendente
                            confirmacaoMatricula.Status = RN.ConfirmacaoMatricula.Pendente;
                            confirmacaoMatricula.Observacao = "NÃO EXISTE VAGAS PARA GERAR CONFIRMAÇAO DE RENOVAÇÃO";
                        }
                    }

                    //Inserir confirmação
                    this.InsereConfirmacaoMatricula(context, confirmacaoMatricula);

                    if (possuiRenovacaoAutomatica == 0)
                    {
                        if (!rnRenovacao.PossuiRenovacaoAtivaPor(context, renovacao.Ano, renovacao.Periodo, renovacao.UnidadeEnsinoId, renovacao.CursoId, renovacao.TurnoId, renovacao.Serie, renovacao.TipoVaga))
                        {
                            throw new Exception(String.Format("ERRO_VALIDACAO:Para o aluno: {0} - {1} não foi possivel gerar o relacionamento de renovação com confirmação pois para o ano.",
                                        dadosAluno.Aluno,
                                        dadosAluno.Nome_compl
                                       ));
                        }

                        if (!rnRenovacaoConfirmacaoMatricula.PossuiRenovacaoConfirmacaoMatriculaPor(context, confirmacaoMatricula, renovacao))
                        {   //Inserir tabela de relacionamento
                            rnRenovacaoConfirmacaoMatricula.InsereRenovacaoConfirmacaoMatricula(context, confirmacaoMatricula, renovacao);
                        }

                        //Atualizar situação da renovação
                        renovacao.SituacaoRenovacaoId = situacaoPossuiConfirmacao;
                        renovacao.DataAlteracao = DateTime.Now;
                        renovacao.Usuario = matriculaUsuario;
                        rnRenovacao.AtualizaRenovacao(context, renovacao);
                    }
                }
                else
                {
                    if (situacaoFinalAluno != FechamentoMatricula.Aprovado && situacaoFinalAluno != FechamentoMatricula.Promovido)
                    {
                        //verifica se o aluno é de turma concluinte do ensino medio, caso seja gerar confirmação
                        //Ou 9º Ano
                        if (rnMatricula.EhMatriculaRegularConcluinte(aluno, Convert.ToInt32(dadosTurmaOrigem.Ano), Convert.ToInt32(dadosTurmaOrigem.Periodo), dadosTurmaOrigem.Grade))
                        {
                            //Criar entidade de confirmação                    
                            confirmacaoMatricula.Aluno = aluno;
                            confirmacaoMatricula.Censo = proximaUnidadeEnsino;
                            confirmacaoMatricula.Ano = proximoAno;
                            confirmacaoMatricula.Periodo = proximoPeriodo;
                            confirmacaoMatricula.Curso = proximoCurso;
                            confirmacaoMatricula.Serie = proximaSerie;
                            confirmacaoMatricula.Turno = proximoTurno;
                            confirmacaoMatricula.Curriculo = proximoCurriculo;
                            confirmacaoMatricula.DtSugerida = DateTime.Now;
                            confirmacaoMatricula.EnsinoReligioso = Convert.ToBoolean(podeERLE.Rows[0]["PODE_ENSINO_RELIGIOSO"]);
                            confirmacaoMatricula.LinguaEstrangeiraFacultativa = Convert.ToBoolean(podeERLE.Rows[0]["PODE_LINGUA_ESTRANGEIRA"]);
                            confirmacaoMatricula.ProjetoAutonomia = false;
                            confirmacaoMatricula.Matricula = matriculaUsuario;
                            confirmacaoMatricula.DtCadastro = DateTime.Now;
                            confirmacaoMatricula.DtAlteracao = DateTime.Now;
                            confirmacaoMatricula.TipoVagaOcupada = tipoVaga;
                            confirmacaoMatricula.Observacao = "GERADA POR REPROVAÇÃO DO ALUNO CONCLUINTE";
                            confirmacaoMatricula.Status = ConfirmacaoMatricula.Pendente;

                            //criar confirmação
                            InsereConfirmacaoMatricula(context, confirmacaoMatricula);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AtualizaCurriculoConfirmacaoMatriculaPor(DataContext context, TceConfirmacaoMatricula confirmacaoMatricula)
        {
            ContextQuery contextQuery = new ContextQuery();
            try
            {
                contextQuery.Command = @" UPDATE  TCE_CONFIRMACAO_MATRICULA
                                    SET     CURRICULO = @CURRICULO ,
                                            MATRICULA = @MATRICULA ,
                                            DT_ALTERACAO = @DT_ALTERACAO 
                                    WHERE   ALUNO = @ALUNO 
                                        AND ANO = @ANO 
                                        AND PERIODO = @PERIODO 
                                        AND SERIE = @SERIE 
                                        AND TURNO = @TURNO
                                        AND CURSO = @CURSO
                                        AND CENSO = @UNIDADE
                                        AND [STATUS] = 'Confirmado'";

                contextQuery.Parameters.Add("@ANO", confirmacaoMatricula.Ano);
                contextQuery.Parameters.Add("@PERIODO", confirmacaoMatricula.Periodo);
                contextQuery.Parameters.Add("@ALUNO", confirmacaoMatricula.Aluno);
                contextQuery.Parameters.Add("@SERIE", confirmacaoMatricula.Serie);
                contextQuery.Parameters.Add("@TURNO", confirmacaoMatricula.Turno);
                contextQuery.Parameters.Add("@CURSO", confirmacaoMatricula.Curso);
                contextQuery.Parameters.Add("@UNIDADE", confirmacaoMatricula.Censo);
                contextQuery.Parameters.Add("@CURRICULO", confirmacaoMatricula.Curriculo);
                contextQuery.Parameters.Add("@MATRICULA", confirmacaoMatricula.Matricula);
                contextQuery.Parameters.Add("@DT_ALTERACAO", confirmacaoMatricula.DtAlteracao);

                context.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DadosFichaConfirmacao ObtemFichaConfirmacaoAlunoPor(int idConfirmacaoMatricula)
        {
            DadosFichaConfirmacao dados = new DadosFichaConfirmacao();
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
                            CM.ANO ,
                            CM.PERIODO ,
                            CM.CENSO ,
                            UE.NOME_COMP AS ESCOLA_DESCRICAO ,
                            MC.DESCRICAO AS MODALIDADE ,
                            CM.CURSO ,
                            CUR.NOME AS CURSO_DESCRICAO ,
                            CM.SERIE ,
                            TU.DESCRICAO AS TURNO ,
                            CM.DT_SUGERIDA ,
                            CM.ENSINO_RELIGIOSO ,
                            CM.LINGUA_ESTRANGEIRA_FACULTATIVA ,
                            CM.STATUS ,
                            CM.DT_ALTERACAO ,
                            CM.MATRICULA AS USUARIO_RESPONSAVEL,
                            U.NOME AS USUARIO_RESPONSAVEL_NOME
                    FROM    DBO.TCE_CONFIRMACAO_MATRICULA CM ( NOLOCK )
                            LEFT JOIN HADES.DBO.HD_USUARIO U ( NOLOCK ) ON CM.MATRICULA = U.USUARIO
                            INNER JOIN DBO.LY_CURSO CUR ( NOLOCK ) ON CM.CURSO = CUR.CURSO
                            INNER JOIN DBO.LY_MODALIDADE_CURSO MC ( NOLOCK ) ON CUR.MODALIDADE = MC.MODALIDADE
                            INNER JOIN DBO.LY_UNIDADE_ENSINO UE ( NOLOCK ) ON CM.CENSO = UE.UNIDADE_ENS
                            INNER JOIN DBO.LY_TURNO TU ( NOLOCK ) ON CM.TURNO = TU.TURNO
                            INNER JOIN LY_ALUNO A ( NOLOCK ) ON A.ALUNO = CM.ALUNO
                            INNER JOIN DBO.LY_PESSOA P ( NOLOCK ) ON A.PESSOA = P.PESSOA
                            LEFT JOIN DBO.LY_FL_PESSOA FLP ( NOLOCK ) ON P.PESSOA = FLP.PESSOA
                            LEFT JOIN HADES.DBO.HD_MUNICIPIO MNASC ( NOLOCK ) ON P.MUNICIPIO_NASC = MNASC.MUNICIPIO
                            LEFT JOIN HADES.DBO.HD_PAIS PA ( NOLOCK ) ON P.PAIS_NASC = PA.PAIS
                            LEFT JOIN HADES.DBO.HD_MUNICIPIO MEND ( NOLOCK ) ON P.END_MUNICIPIO = MEND.MUNICIPIO
                            LEFT JOIN CARTORIO C ( NOLOCK ) ON C.COD_CARTORIO = P.ID_CARTORIO
                            LEFT JOIN HADES.dbo.NECESSIDADEESPECIAL NEC ON NEC.NECESSIDADEESPECIALID=P.NECESSIDADEESPECIALID
                    WHERE   CM.ID_CONFIRMACAO_MATRICULA = @ID_CONFIRMACAO_MATRICULA ";

                contextQuery.Parameters.Add("@ID_CONFIRMACAO_MATRICULA", idConfirmacaoMatricula);

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
                    dados.NecessidadeEspecial = Convert.ToString(reader["NECESSIDADE_ESPECIAL"]);
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

                    //Preenche Confirmacao Matricula
                    dados.AnoLetivo = Convert.ToInt32(reader["ANO"]);
                    dados.PeriodoLetivo = Convert.ToInt32(reader["PERIODO"]);
                    dados.UnidadeEnsino = Convert.ToString(reader["ESCOLA_DESCRICAO"]);
                    dados.Censo = Convert.ToString(reader["CENSO"]);
                    dados.Modalidade = Convert.ToString(reader["MODALIDADE"]);
                    dados.Curso = Convert.ToString(reader["CURSO"]);
                    dados.CursoDescricao = Convert.ToString(reader["CURSO_DESCRICAO"]);
                    dados.Serie = Convert.ToInt32(reader["SERIE"]);
                    dados.Turno = Convert.ToString(reader["TURNO"]);
                    dados.DataSugerida = Convert.ToDateTime(reader["DT_SUGERIDA"]);
                    dados.EnsinoReligioso = Convert.ToString(reader["ENSINO_RELIGIOSO"]);
                    dados.LinguaEstrangueira = Convert.ToString(reader["LINGUA_ESTRANGEIRA_FACULTATIVA"]);
                    dados.Situacao = Convert.ToString(reader["STATUS"]);
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

        public void CancelaPorReprovacaoPossiveisConfirmacaoMatriculaPor(DataContext context, string aluno, int ano, int periodo, string matriculaResponsavel, int periodoOrigem)
        {
            ContextQuery contextQuery = new ContextQuery();
            string possiveisPeriodos;

            try
            {
                //Verificar possiveis periodos com relação ao periodo de origem
                possiveisPeriodos = Utils.RecuperaPossiveisFuturosPeriodos(periodoOrigem);

                if (ano == 2021 && periodo == 1)
                {
                    possiveisPeriodos = "1";
                }

                contextQuery.Command = string.Format(@" UPDATE  TCE_CONFIRMACAO_MATRICULA
                                    SET     [STATUS] = @STATUS ,
                                            MATRICULA = @MATRICULA ,
                                            DT_ALTERACAO = @DT_ALTERACAO ,
                                            OBSERVACAO = (CASE LEN(ISNULL(OBSERVACAO,'')) 
					                                WHEN 0 THEN @OBSERVACAO
					                                ELSE 
						                                RTRIM(OBSERVACAO) + ' / ' + @OBSERVACAO 
					                                END)
                                    WHERE   ALUNO = @ALUNO
                                            AND ( [STATUS] = 'CONFIRMADO'
                                                  OR [STATUS] IS NULL
                                                )
                                            AND ANO = @ANO
                                    AND PERIODO IN ( {0} ) ", possiveisPeriodos);

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@STATUS", NaoConfirmado);
                contextQuery.Parameters.Add("@MATRICULA", matriculaResponsavel);
                contextQuery.Parameters.Add("@DT_ALTERACAO", DateTime.Now);
                contextQuery.Parameters.Add("@OBSERVACAO", Observacao.CanceladaPorReprovacaoDeAluno);

                context.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //        public void CancelaPossiveisConfirmacaoMatriculaPor(DataContext context, string aluno, int ano, int periodo, string matriculaResponsavel)
        //        {
        //            ContextQuery contextQuery = new ContextQuery();
        //            string possiveisPeriodos;
        //            string observacao = "CANCELADA POR TRANSFERENCIA DO ALUNO";

        //            //Verificar possiveis periodos com relação:         
        //            if (periodo == 2)
        //            {
        //                //2 pode transferir para 0 ou 2  
        //                possiveisPeriodos = "0, 2";
        //            }
        //            else if (periodo == 1)
        //            {
        //                //1 pode transferir para 0 ou 1
        //                possiveisPeriodos = "0, 1";
        //            }
        //            else
        //            {
        //                //0 pode transferir para 0 ou 1 ou 2
        //                possiveisPeriodos = "0, 1, 2";
        //            }

        //            try
        //            {
        //                contextQuery.Command = string.Format(@" UPDATE  TCE_CONFIRMACAO_MATRICULA
        //                                    SET     [STATUS] = @STATUS ,
        //                                            MATRICULA = @MATRICULA ,
        //                                            DT_ALTERACAO = @DT_ALTERACAO ,
        //                                            OBSERVACAO = (CASE LEN(ISNULL(OBSERVACAO,'')) 
        //WHEN 0 THEN @OBSERVACAO
        //ELSE 
        //    RTRIM(OBSERVACAO) + ' / ' + @OBSERVACAO 
        //END)
        //                                    WHERE   ALUNO = @ALUNO
        //                                            AND ( [STATUS] = 'CONFIRMADO'
        //                                                  OR [STATUS] IS NULL
        //                                                )
        //                                            AND ANO = @ANO
        //                                    AND PERIODO IN ( {0} ) ", possiveisPeriodos);

        //                contextQuery.Parameters.Add("@ALUNO", aluno);
        //                contextQuery.Parameters.Add("@ANO", ano);
        //                contextQuery.Parameters.Add("@STATUS", NaoConfirmado);
        //                contextQuery.Parameters.Add("@MATRICULA", matriculaResponsavel);
        //                contextQuery.Parameters.Add("@DT_ALTERACAO", DateTime.Now);
        //                contextQuery.Parameters.Add("@OBSERVACAO", observacao);

        //                context.ApplyModifications(contextQuery);
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }
        
        public DataTable ObtemPossiveisConfirmacaoMatriculaPor(string aluno, decimal ano, decimal periodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            DataTable dataTable = new DataTable();
            ContextQuery contextQuery = new ContextQuery();

            string possiveisPeriodos = Utils.RecuperaPossiveisPeriodosCompleto(Convert.ToInt32(periodo));


            contextQuery.Command = string.Format(@" SELECT *
                                        FROM TCE_CONFIRMACAO_MATRICULA (NOLOCK)
                                        WHERE ALUNO = @ALUNO
                                             AND ANO = @ANO
                                             AND PERIODO IN ( {0} )
                                             AND ( [STATUS] = 'Confirmado'
                                                   OR [STATUS] IS NULL)
                                        ORDER BY STATUS DESC ", possiveisPeriodos); ;

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@ALUNO", aluno);

            dataTable = ctx.GetDataTable(contextQuery);

            return dataTable;
        }

        public void CancelaPossiveisConfirmacaoMatriculaConfirmadaPor(DataContext ctx, string aluno, int ano, int periodo, string matriculaResponsavel, int periodoOrigem, string observacao, string periodosPossiveis)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = string.Format(
                @" UPDATE  TCE_CONFIRMACAO_MATRICULA
                   SET     [STATUS] = @STATUS ,
                           MATRICULA = @MATRICULA ,
                           DT_ALTERACAO = GETDATE(),
                           OBSERVACAO = (CASE LEN(ISNULL(OBSERVACAO,'')) 
					                WHEN 0 THEN @OBSERVACAO
					                ELSE 
						                RTRIM(OBSERVACAO) + ' / ' + @OBSERVACAO 
					                END)
                   WHERE   ALUNO = @ALUNO
                           AND [STATUS] = @STATUSCONFIRMADO
                           AND ANO = @ANO
                           AND PERIODO IN ( {0} ) ", periodosPossiveis);

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@STATUS", NaoConfirmado);
                contextQuery.Parameters.Add("@STATUSCONFIRMADO", ConfirmacaoMatricula.Confirmado);
                contextQuery.Parameters.Add("@MATRICULA", matriculaResponsavel);
                contextQuery.Parameters.Add("@OBSERVACAO", observacao);

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

        public void CancelaPossiveisConfirmacaoMatriculaPor(DataContext ctx, string aluno, decimal ano, decimal periodo, string matriculaResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            string possiveisPeriodos = Utils.RecuperaPossiveisPeriodosCompleto(Convert.ToInt32(periodo));

            try
            {
                string observacao = string.Format("RETIRADO EM {0} ÀS {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm"));

                contextQuery.Command = string.Format(@" UPDATE TCE_CONFIRMACAO_MATRICULA
                            SET STATUS = @STATUS,
                                MATRICULA = @MATRICULA ,
                                DT_ALTERACAO = GETDATE(),
                                OBSERVACAO = (CASE LEN(ISNULL(OBSERVACAO,'')) 
					                    WHEN 0 THEN @OBSERVACAO
					                    ELSE 
						                    RTRIM(OBSERVACAO) + ' / ' + @OBSERVACAO 
					                    END)
                            WHERE   ALUNO = @ALUNO
                                    AND ANO = @ANO
                                    AND (STATUS = 'CONFIRMADO' OR STATUS IS NULL)
                                    AND PERIODO IN ( {0} ) ", possiveisPeriodos);

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@STATUS", NaoConfirmado);
                contextQuery.Parameters.Add("@MATRICULA", matriculaResponsavel);
                contextQuery.Parameters.Add("@OBSERVACAO", observacao);

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

        public DataTable ListaConfirmacaoMatriculaPor(string matriculas)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            DataTable dataTable = new DataTable();
            ContextQuery contextQuery = new ContextQuery();

            if (matriculas.IsNullOrEmptyOrWhiteSpace()) return null;

            contextQuery.Command = string.Format(@"SELECT DISTINCT CM.ID_CONFIRMACAO_MATRICULA, CM.ALUNO, CM.CENSO, CM.ANO, CM.PERIODO,
                                    C.NOME AS CURSO, CM.SERIE, CM.TURNO, CM.DT_SUGERIDA, CM.ENSINO_RELIGIOSO,
                                    CM.LINGUA_ESTRANGEIRA_FACULTATIVA, CM.PROJETO_AUTONOMIA, CM.STATUS,
                                    CM.MATRICULA, CM.DT_CADASTRO, CM.DT_ALTERACAO, E.NOME_COMP AS ESCOLA,
                                    CM.CENSO + ' - ' + E.NOME_COMP AS UNIDADE_ENSINO, MD.MODALIDADE,CM.CURRICULO,
                                    TC.TIPO AS SEGMENTO, T.DESCRICAO AS NOME_TURNO, CM.CURSO AS COD_CURSO,
                                    MD.DESCRICAO + ' / ' + TC.DESCRICAO + ' / ' + C.NOME AS MOD_SEG_CURSO,
                                    ISNULL(CR.ENSINO_RELIGIOSO, 'N') AS PODE_ENSINO_RELIGIOSO, ISNULL(CR.LINGUA_ESTRANGEIRA, 'N') AS PODE_LINGUA_ESTRANGEIRA,
                                     CASE WHEN CM.DT_ALTERACAO IS NOT NULL
                                              AND CM.STATUS IS NULL THEN 'N'
                                         WHEN CM.DT_ALTERACAO IS NULL THEN 'N'
                                         ELSE 'S'
                                    END CADASTROU,TIPOVAGAOCUPADA
                        FROM DBO.TCE_CONFIRMACAO_MATRICULA CM (NOLOCK)
                        INNER JOIN DBO.LY_UNIDADE_ENSINO E (NOLOCK) ON  CM.CENSO = E.UNIDADE_ENS
                        INNER JOIN DBO.LY_CURSO C (NOLOCK) ON  CM.CURSO = C.CURSO
                        INNER JOIN DBO.LY_MODALIDADE_CURSO MD (NOLOCK) ON C.MODALIDADE = MD.MODALIDADE
                        INNER JOIN DBO.LY_TIPO_CURSO TC (NOLOCK)ON C.TIPO = TC.TIPO
                        INNER JOIN DBO.LY_TURNO T (NOLOCK) ON CM.TURNO = T.TURNO
                        LEFT JOIN DBO.LY_CURRICULO CR (NOLOCK) ON CM.CURSO = CR.CURSO AND CM.TURNO = CR.TURNO AND CM.CURRICULO = CR.CURRICULO 
                        INNER JOIN dbo.LY_PERIODO_LETIVO sl ON CM.ANO = sl.ANO
                                                               AND CM.PERIODO = sl.PERIODO
                        WHERE CM.STATUS = @STATUS
                        AND CM.ALUNO IN ('{0}')  
                        AND SL.DT_FIM >= GETDATE()
                        ORDER BY CM.ANO DESC, DT_CADASTRO", matriculas);

            contextQuery.Parameters.Add("@STATUS", Confirmado);

            dataTable = ctx.GetDataTable(contextQuery);

            return dataTable;
        }

        public TceConfirmacaoMatricula ObtemConfirmacaoPor(int idConfirmacaoMatricula)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            TceConfirmacaoMatricula confirmacao = new TceConfirmacaoMatricula();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT TOP 1
                                                *
                                        FROM    TCE_CONFIRMACAO_MATRICULA (NOLOCK)
                                        WHERE   ID_CONFIRMACAO_MATRICULA = @ID_CONFIRMACAO_MATRICULA   ";

                contextQuery.Parameters.Add("@ID_CONFIRMACAO_MATRICULA", idConfirmacaoMatricula);

                confirmacao = ctx.TryToBindEntity<TceConfirmacaoMatricula>(contextQuery);

                return confirmacao;
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

        public bool PossuiConfirmacaoConfirmadaPor(string aluno, decimal ano, decimal periodo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                FROM TCE_CONFIRMACAO_MATRICULA (NOLOCK)
                                WHERE STATUS = @STATUS								
	                                AND ALUNO = @ALUNO
                                    AND ANO = @ANO
                                    AND PERIODO = @PERIODO ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@STATUS", Confirmado);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
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

        public int ObtemControleVagaIdConfirmadoPor(DataContext contexto, string aluno, int ano, int periodo, out string municipio)
        {
            int controleVagaId = 0;
            municipio = string.Empty;
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DISTINCT 
                                            CM.ALUNO ,
                                            CM.ANO ,
                                            CM.PERIODO ,
							                CV.ID_CONTROLE_VAGA,
							                UE.MUNICIPIO
							  FROM TCE_CONFIRMACAO_MATRICULA cm (NOLOCK)
								INNER JOIN TCE_CONTROLE_VAGA CV (NOLOCK) ON CV.ANO = CM.ANO
							                              AND CV.PERIODO = CM.PERIODO
														  AND CV.CURSO = CM.CURSO
														  AND CV.SERIE = CM.SERIE
														  AND CV.CENSO = CM.CENSO
														  AND CV.TURNO = CM.TURNO
								INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK) ON CV.CENSO = UE.UNIDADE_ENS
                                WHERE STATUS = @STATUS							
	                                AND ALUNO = @ALUNO
                                    AND cm.ANO = @ANO
                                    AND cm.PERIODO = @PERIODO ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@STATUS", Confirmado);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    municipio = Convert.ToString(reader["MUNICIPIO"]);
                    controleVagaId = Convert.ToInt32(reader["ID_CONTROLE_VAGA"]);
                }

                return controleVagaId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public DataTable ObtemListaConfirmacaConfirmadaPor(int ano, int periodo, string censo, string alunos)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable listaAlunosNomes = null;
            string possiveisPeriodos = Utils.RecuperaPossiveisFuturosPeriodos(Convert.ToInt32(periodo));

            try
            {
                contextQuery.Command = string.Format(@"  SELECT DISTINCT CM.ALUNO, P.NOME_COMPL
                                        FROM    TCE_CONFIRMACAO_MATRICULA CM
                                        INNER JOIN LY_ALUNO A ON CM.ALUNO = A.ALUNO
                                        INNER JOIN LY_PESSOA P ON P.PESSOA = A.PESSOA
                                        WHERE    [STATUS] = 'Confirmado'
                                                AND CM.CENSO = @CENSO
                                                AND ANO = @ANO
                                                AND PERIODO in ({0})
                                                AND CM.ALUNO IN (" + alunos + ")", possiveisPeriodos);




                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@CENSO", censo);


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

        public DataTable ObtemAlunoConfirmacaoPor(int ano, int periodo, string alunos)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable listaAlunosNomes = null;
            string possiveisPeriodos = Utils.RecuperaPossiveisFuturosPeriodos(Convert.ToInt32(periodo));

            try
            {
                contextQuery.Command = string.Format(@"  SELECT DISTINCT A.ALUNO, P.NOME_COMPL , CASE WHEN R.ID_CONFIRMACAO_MATRICULA IS NULL THEN 'N' ELSE 'S' END RENOVACAO,A.UNIDADE_ENSINO,R.CENSO UNIDADEENSINOID
                                           FROM  LY_ALUNO  A
                                        LEFT JOIN TCE_CONFIRMACAO_MATRICULA R  ON R.ALUNO=A.ALUNO AND [STATUS] = 'Confirmado' AND ANO = @ANO AND PERIODO IN ({0})
                                        INNER JOIN LY_PESSOA P ON P.PESSOA = A.PESSOA
                                        WHERE A.ALUNO IN (" + alunos + ")", possiveisPeriodos);



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

        public TceConfirmacaoMatricula ObtemConfirmacaoAtivaPossiveisPeriodoPor(string aluno, int ano, int periodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            TceConfirmacaoMatricula confirmacao = new TceConfirmacaoMatricula();
            ContextQuery contextQuery = new ContextQuery();
            string possiveisPeriodos = Utils.RecuperaPossiveisPeriodosCompleto(periodo);


            try
            {

                contextQuery.Command = string.Format(@" SELECT TOP 1
                                                *
                                        FROM    TCE_CONFIRMACAO_MATRICULA (NOLOCK)
                                        WHERE   ALUNO = @ALUNO
                                                AND ANO = @ANO
                                                AND PERIODO IN ( {0} )
                                        ORDER BY DT_CADASTRO DESC ", possiveisPeriodos);

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@ALUNO", aluno);

                confirmacao = ctx.TryToBindEntity<TceConfirmacaoMatricula>(contextQuery);

                return confirmacao;
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
    }
}