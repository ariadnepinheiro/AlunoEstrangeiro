using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using Techne.Data;
using System.Text;

namespace Techne.Lyceum.RN.AvaliacaoExterna
{
    public class ReaberturaTurma
    {
        public bool PossuiEtapaPor(DataContext contexto, int etapaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(1) 
                                        FROM  AvaliacaoExterna.REABERTURATURMA (NOLOCK) 
                                        WHERE  ETAPAID = @ETAPAID ";

            contextQuery.Parameters.Add("@ETAPAID", SqlDbType.Int, etapaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable Consultar(int periodo, string unidadeEnsino, int provaId, int etapaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT A.AVALIACAOID, 
                                                    E.ETAPAID, 
                                                    LYT.TURMA, 
                                                    LYT.ANO, 
                                                    LYT.SEMESTRE, 
                                                    LYC.NOME, 
                                                    LYN.DESCRICAO, 
                                                    LYT.SERIE, 
                                                    UE.NOME_COMP, 
                                                    E.INICIOTRANSCRICAO, 
                                                    E.FIMTRANSCRICAO, 
                                                    RT.REABERTURATURMAID, 
                                                    CASE 
                                                      WHEN RT.REABERTURATURMAID IS NULL THEN NULL 
                                                      WHEN RT.REABERTURATURMAID IS NOT NULL 
                                                           AND APROVADORID IS NULL THEN 'PENDENTE' 
                                                      WHEN RT.REABERTURATURMAID IS NOT NULL 
                                                           AND APROVADORID IS NOT NULL 
                                                           AND ( DATALIBERACAO IS NULL 
                                                                  OR DATAFECHAMENTO IS NULL ) THEN 'REPROVADO' 
                                                      WHEN RT.REABERTURATURMAID IS NOT NULL 
                                                           AND APROVADORID IS NOT NULL 
                                                           AND ( DATALIBERACAO IS NOT NULL 
                                                                 AND DATAFECHAMENTO IS NOT NULL ) THEN 'APROVADO' 
                                                    END AS STATUS, 
                                                    CASE 
                                                      WHEN TR.DATAFINALIZACAO IS NOT NULL THEN  'FINALIZADA' 
                                                      WHEN TR.DATAINICIO IS NOT NULL THEN 'INICIADA' 
                                                      ELSE 'SEM TRANSCRIÇÃO' 
                                                    END AS STATUSTRANSCRICAO, 
                                                    RT.JUSTIFICATIVA, 
                                                    RT.DATAFECHAMENTO, 
                                                    TR.TRANSCRICAOTURMAID 
                                    FROM   avaliacaoexterna.avaliacao a 
                                           INNER JOIN avaliacaoexterna.prova p 
                                                   ON a.avaliacaoid = p.avaliacaoid 
                                           INNER JOIN avaliacaoexterna.etapa e 
                                                   ON p.provaid = e.provaid 
                                           INNER JOIN ly_turma lyt 
                                                   ON a.ano = lyt.ano 
                                                      AND e.curso = lyt.curso 
                                                      AND e.serie = lyt.serie 
                                           INNER JOIN ly_unidade_ensino ue 
                                                   ON ue.unidade_ens = lyt.faculdade 
                                           INNER JOIN ly_curso lyc 
                                                   ON lyc.curso = lyt.curso 
                                           INNER JOIN ly_turno lyn 
                                                   ON lyn.turno = lyt.turno 
		                                    LEFT JOIN [AVALIACAOEXTERNA].[TRANSCRICAOTURMA] TR 
				                                    ON TR.ETAPAID = E.ETAPAID 
                                                     AND TR.TURMA = LYT.TURMA
                                                     AND TR.ANO = LYT.ANO
                                                     AND TR.SEMESTRE = LYT.SEMESTRE 
                                           LEFT JOIN AVALIACAOEXTERNA.REABERTURATURMA RT 
                                                  ON RT.ETAPAID = E.ETAPAID 
                                                     AND RT.TURMA = LYT.TURMA 
                                                     AND RT.ANO = LYT.ANO 
                                                     AND RT.SEMESTRE = LYT.SEMESTRE 
                                    WHERE  lyt.sit_turma IN ( 'ABERTA', 'Finalizada' ) 
	                                       AND (RT.REABERTURATURMAID IS NULL OR REABERTURATURMAID = (SELECT MAX(RT2.REABERTURATURMAID) 
																	                                    FROM AVALIACAOEXTERNA.REABERTURATURMA RT2 
																	                                    WHERE RT.ETAPAID = RT2.ETAPAID 
																		                                      AND RT.TURMA = RT2.TURMA 
																		                                      AND RT.ANO = RT2.ANO 
																		                                      AND RT.SEMESTRE = RT2.SEMESTRE ))
                                           AND P.PROVAID = @PROVAID 
										   AND E.ETAPAID = @ETAPAID
                                           AND LYT.SEMESTRE = @PERIODO 
	                                       AND LYT.FACULDADE = @UNIDADEENSINOID ";

                contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, provaId);
                contextQuery.Parameters.Add("@ETAPAID", SqlDbType.Int, etapaId);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
                contextQuery.Parameters.Add("@UNIDADEENSINOID", SqlDbType.VarChar, unidadeEnsino);

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

        public DataTable ConsultarSolicitacoes(int avaliacaoId, int periodo, string regional, string municipio, string unidadeEnsino, string usuario)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(@" SELECT DISTINCT A.AVALIACAOID, 
                                                E.ETAPAID, 
                                                LYT.TURMA, 
                                                LYT.ANO, 
                                                LYT.SEMESTRE, 
                                                LYC.NOME, 
                                                LYN.DESCRICAO, 
                                                LYT.SERIE, 
                                                UE.NOME_COMP, 
                                                RT.REABERTURATURMAID, 
                                                CASE 
                                                  WHEN RT.REABERTURATURMAID IS NULL THEN NULL 
                                                  WHEN RT.REABERTURATURMAID IS NOT NULL 
                                                       AND APROVADORID IS NULL THEN 'PENDENTE' 
                                                  WHEN RT.REABERTURATURMAID IS NOT NULL 
                                                       AND APROVADORID IS NOT NULL 
                                                       AND ( DATALIBERACAO IS NULL 
                                                              OR DATAFECHAMENTO IS NULL ) THEN 'REPROVADO' 
                                                  WHEN RT.REABERTURATURMAID IS NOT NULL 
                                                       AND APROVADORID IS NOT NULL 
                                                       AND ( DATALIBERACAO IS NOT NULL 
                                                             AND DATAFECHAMENTO IS NOT NULL ) THEN 'APROVADO' 
                                                END AS STATUS, 
                                                RT.JUSTIFICATIVA, 
                                                RT.DATAFECHAMENTO,
				                                CONVERT(VARCHAR(20), DATALIBERACAO, 103) AS DATALIBERACAO,
												CASE
													WHEN APROVADORID IS NULL THEN ''
													WHEN DATALIBERACAO IS NOT NULL THEN CONVERT(VARCHAR(20), DATALIBERACAO, 103)
													ELSE CONVERT(VARCHAR(20), RT.DATAALTERACAO, 103)
											    END DATAANALISE
                                FROM   LY_TURMA LYT (NOLOCK) 
                                       INNER JOIN AVALIACAOEXTERNA.ETAPA E 
                                               ON E.CURSO = LYT.CURSO 
                                                  AND E.SERIE = LYT.SERIE 
									   INNER JOIN AVALIACAOEXTERNA.PROVA P 
											   ON P.PROVAID = E.PROVAID
                                       INNER JOIN AVALIACAOEXTERNA.AVALIACAO A 
                                               ON A.ANO = LYT.ANO 
											    AND A.AVALIACAOID = p.AVALIACAOID
                                       INNER JOIN LY_UNIDADE_ENSINO UE 
                                               ON UE.UNIDADE_ENS = LYT.FACULDADE 
                                       INNER JOIN LY_CURSO LYC 
                                               ON LYC.CURSO = LYT.CURSO 
                                       INNER JOIN LY_TURNO LYN 
                                               ON LYN.TURNO = LYT.TURNO 
                                       INNER JOIN AVALIACAOEXTERNA.REABERTURATURMA RT 
                                               ON RT.ETAPAID = E.ETAPAID 
                                                  AND RT.TURMA = LYT.TURMA 
												  and RT.ANO = LYT.ANO
												  and RT.SEMESTRE = LYT.SEMESTRE
                                       INNER JOIN USUARIO u ON u.USUARIO = @USUARIO
                                                                AND ( EXISTS ( SELECT TOP 1
                                                                                        UNIDADE_FIS
                                                                               FROM     LY_USUARIO_UNIDADE_FIS usuuni
                                                                                        WITH ( NOLOCK )
                                                                               WHERE    usuuni.UNIDADE_FIS = UE.UNIDADE_ENS
                                                                                        AND usuuni.USUARIO = u.USUARIO
                                                                                        AND u.PRIVIL <> 'S' )
                                                                      OR ( U.PRIVIL = 'S' )
                                                                    )
                                WHERE  LYT.SIT_TURMA in ('ABERTA', 'Finalizada') 
                                       AND a.AVALIACAOID = @AVALIACAOID 
                                       AND lyt.SEMESTRE = @PERIODO 
                                        ");

                if (unidadeEnsino.IsNullOrEmptyOrWhiteSpace())
                {
                    if (!regional.IsNullOrEmptyOrWhiteSpace())
                    {
                        sql.Append(@" AND UE.ID_REGIONAL = @REGIONAL 
                                  ");

                        contextQuery.Parameters.Add("@REGIONAL", SqlDbType.Int, Convert.ToInt32(regional));
                    }

                    if (!municipio.IsNullOrEmptyOrWhiteSpace())
                    {
                        sql.Append(@" AND UE.MUNICIPIO = @MUNICIPIO 
                                  ");

                        contextQuery.Parameters.Add("@MUNICIPIO", SqlDbType.VarChar, municipio);
                    }
                }
                else
                {
                    sql.Append(@" AND lyt.FACULDADE = @UNIDADEENSINOID 
                                  ");

                    contextQuery.Parameters.Add("@UNIDADEENSINOID", SqlDbType.VarChar, unidadeEnsino);
                }

                sql.Append(" ORDER BY RT.REABERTURATURMAID DESC ");

                contextQuery.Command = sql.ToString();

                contextQuery.Parameters.Add("@AVALIACAOID", SqlDbType.Int, avaliacaoId);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
                contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuario);

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

        public void Solicita(Entidades.ReaberturaTurma reaberturaTurma)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"INSERT INTO AvaliacaoExterna.REABERTURATURMA
                                                        (ETAPAID,
                                                         ANO, 
                                                         SEMESTRE, 
                                                         TURMA,
                                                         SOLICITANTEID,
                                                         DATASOLICITACAO,
                                                         JUSTIFICATIVA,
                                                         USUARIOID,
                                                         DATACADASTRO,
                                                         DATAALTERACAO)
                                            VALUES      (@ETAPAID,
                                                         @ANO, 
                                                         @SEMESTRE, 
                                                         @TURMA,
                                                         @SOLICITANTEID,
                                                         @DATASOLICITACAO,
                                                         @JUSTIFICATIVA,
                                                         @USUARIOID,
                                                         @DATACADASTRO,
                                                         @DATAALTERACAO)

                                            SELECT IDENT_CURRENT('AvaliacaoExterna.REABERTURATURMA') ";

                contextQuery.Parameters.Add("@ETAPAID", SqlDbType.Int, reaberturaTurma.EtapaId);
                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, reaberturaTurma.Turma);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, reaberturaTurma.Ano);
                contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, reaberturaTurma.Semestre);
                contextQuery.Parameters.Add("@SOLICITANTEID", SqlDbType.VarChar, reaberturaTurma.SolicitanteId);
                contextQuery.Parameters.Add("@DATASOLICITACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@JUSTIFICATIVA", SqlDbType.VarChar, reaberturaTurma.Justificativa);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, reaberturaTurma.UsuarioID);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                reaberturaTurma.ReaberturaTurmaId = Convert.ToInt32(ctx.GetReturnValue(contextQuery));
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

        public Entidades.ReaberturaTurma ObtemPor(DataContext contexto, int reaberturaTurmaId)
        {
            Entidades.ReaberturaTurma reaberturaTurma = new Techne.Lyceum.RN.AvaliacaoExterna.Entidades.ReaberturaTurma();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" select *
                            from AvaliacaoExterna.REABERTURATURMA
                            where REABERTURATURMAID = @REABERTURATURMAID ";

            contextQuery.Parameters.Add("@REABERTURATURMAID", SqlDbType.Int, reaberturaTurmaId);

            reaberturaTurma = contexto.TryToBindEntity<Entidades.ReaberturaTurma>(contextQuery);

            return reaberturaTurma;
        }

        public void AprovaTodas(List<int> listaReaberturaTurmaId, DateTime dataFechamento, string usuarioId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.AvaliacaoExterna.Entidades.ReaberturaTurma reaberturaTurma = new Techne.Lyceum.RN.AvaliacaoExterna.Entidades.ReaberturaTurma();
            TranscricaoTurma rnTranscricaoTurma = new TranscricaoTurma();

            try
            {
                foreach (int reaberturaTurmaId in listaReaberturaTurmaId)
                {
                    //Atualiza com os dados da aprovacao
                    this.Aprova(contexto, reaberturaTurmaId, usuarioId, dataFechamento, usuarioId);

                    //Busca todos os dados da aprovacao                
                    reaberturaTurma = this.ObtemPor(contexto, reaberturaTurmaId);

                    //Verifica existe transcricacao finalizada para a turma
                    if (rnTranscricaoTurma.PossuiTranscricaoFinalizadaPor(contexto, reaberturaTurma.EtapaId, reaberturaTurma.Turma, reaberturaTurma.Ano, reaberturaTurma.Semestre))
                    {
                        rnTranscricaoTurma.ReabreTranscriacao(contexto, reaberturaTurma.EtapaId, reaberturaTurma.Turma, reaberturaTurma.Ano, reaberturaTurma.Semestre);
                    }
                }
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

        public void Aprova(int reaberturaTurmaId, string aprovadorId, DateTime dataFechamento, string usuarioId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.AvaliacaoExterna.Entidades.ReaberturaTurma reaberturaTurma = new Techne.Lyceum.RN.AvaliacaoExterna.Entidades.ReaberturaTurma();
            TranscricaoTurma rnTranscricaoTurma = new TranscricaoTurma();

            try
            {
                //Atualiza com os dados da aprovacao
                this.Aprova(contexto, reaberturaTurmaId, aprovadorId, dataFechamento, usuarioId);

                //Busca todos os dados da aprovacao                
                reaberturaTurma = this.ObtemPor(contexto, reaberturaTurmaId);

                //Verifica existe transcricacao finalizada para a turma
                if (rnTranscricaoTurma.PossuiTranscricaoFinalizadaPor(contexto, reaberturaTurma.EtapaId, reaberturaTurma.Turma, reaberturaTurma.Ano, reaberturaTurma.Semestre))
                {
                    rnTranscricaoTurma.ReabreTranscriacao(contexto, reaberturaTurma.EtapaId, reaberturaTurma.Turma, reaberturaTurma.Ano, reaberturaTurma.Semestre);
                }
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

        private void Aprova(DataContext ctx, int reaberturaTurmaId, string aprovadorId, DateTime dataFechamento, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE AvaliacaoExterna.REABERTURATURMA SET
                                                         STATUSID = 1,
                                                         APROVADORID = @APROVADORID,
                                                         DATALIBERACAO = @DATALIBERACAO,
                                                         DATAFECHAMENTO = @DATAFECHAMENTO,
                                                         USUARIOID = @USUARIOID,
                                                         DATAALTERACAO = @DATAALTERACAO
                                          WHERE REABERTURATURMAID = @REABERTURATURMAID ";

            contextQuery.Parameters.Add("@REABERTURATURMAID", SqlDbType.Int, reaberturaTurmaId);
            contextQuery.Parameters.Add("@APROVADORID", SqlDbType.VarChar, aprovadorId);
            contextQuery.Parameters.Add("@DATALIBERACAO", SqlDbType.Date, DateTime.Now);
            contextQuery.Parameters.Add("@DATAFECHAMENTO", SqlDbType.Date, dataFechamento);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            ctx.ApplyModifications(contextQuery);
        }

        public void ReprovaTodos(List<int> listaReaberturaTurmaId, string usuarioId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                foreach (int reaberturaTurmaId in listaReaberturaTurmaId)
                {
                    this.Reprova(ctx, reaberturaTurmaId, usuarioId, usuarioId);
                }
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

        public void Reprova(int reaberturaTurmaId, string aprovadorId, string usuarioId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                this.Reprova(ctx, reaberturaTurmaId, aprovadorId, usuarioId);
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

        public void Reprova(DataContext ctx, int reaberturaTurmaId, string aprovadorId, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE AvaliacaoExterna.REABERTURATURMA SET
                                                         STATUSID = 1,
                                                         APROVADORID = @APROVADORID,
                                                         DATALIBERACAO = null,
                                                         DATAFECHAMENTO = null,
                                                         USUARIOID = @USUARIOID,
                                                         DATAALTERACAO = @DATAALTERACAO
                                          WHERE REABERTURATURMAID = @REABERTURATURMAID ";

            contextQuery.Parameters.Add("@REABERTURATURMAID", SqlDbType.Int, reaberturaTurmaId);
            contextQuery.Parameters.Add("@APROVADORID", SqlDbType.VarChar, aprovadorId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            ctx.ApplyModifications(contextQuery);
        }

        public ValidacaoDados ValidaSolicitacao(Entidades.ReaberturaTurma reaberturaTurma)
        {
            List<string> mensagens = new List<string>();
            RN.AvaliacaoExterna.Etapa rnEtapa = new Etapa();
            RN.Turma rnTurma = new Turma();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (reaberturaTurma == null)
            {
                return validacaoDados;
            }

            if (reaberturaTurma.EtapaId <= 0)
            {
                mensagens.Add("Campo CODIGO ETAPA é obrigatório.");
            }

            if (reaberturaTurma.Turma.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TURMA é obrigatório.");
            }

            if (reaberturaTurma.Ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (reaberturaTurma.Semestre < 0)
            {
                mensagens.Add("Campo SEMESTRE é obrigatório.");
            }

            if (reaberturaTurma.SolicitanteId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo SOLICITANTEID é obrigatório.");
            }

            if (reaberturaTurma.Justificativa.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo JUSTIFICATIVA é obrigatório.");
            }

            if (reaberturaTurma.UsuarioID.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo Usuario é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (!rnEtapa.PossuiEtapaPor(contexto, reaberturaTurma.EtapaId))
                    {
                        mensagens.Add("A ETAPA informada não existe.");
                    }

                    if (this.PossuiReaberturaPendentePor(contexto, reaberturaTurma.EtapaId, reaberturaTurma.Turma, reaberturaTurma.Ano, reaberturaTurma.Semestre))
                    {
                        mensagens.Add("Já existe uma solicitação de reabertura pendente para esta turma.");
                    }

                    if (!rnTurma.VerificaSeTurmaExiste(contexto, reaberturaTurma.Turma, reaberturaTurma.Ano, reaberturaTurma.Semestre))
                    {
                        mensagens.Add("A TURMA informada não existe.");
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

        private bool PossuiReaberturaPendentePor(DataContext contexto, int etapaId, string turmaId, int ano, int semestre)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"SELECT COUNT(0) 
                                    FROM AVALIACAOEXTERNA.REABERTURATURMA RT 
                                    WHERE RT.ETAPAID = @ETAPAID 
                                            AND RT.TURMA = @TURMAID 
                                            AND RT.ANO = @ANO
                                            AND RT.SEMESTRE = @SEMESTRE
                                            AND APROVADORID IS NULL";

            contextQuery.Parameters.Add("@ETAPAID", SqlDbType.Int, etapaId);
            contextQuery.Parameters.Add("@TURMAID", SqlDbType.VarChar, turmaId);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, semestre);

            return (contexto.GetReturnValue<int>(contextQuery) > 0);
        }

        public ValidacaoDados ValidaLiberacao(int reaberturaTurmaId, string aprovadorId, DateTime dataFechamento, string usuarioId)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados();

            if (reaberturaTurmaId <= 0)
            {
                mensagens.Add("Campo CÓDIGO é obrigatório.");
            }

            if (aprovadorId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo APROVADORID é obrigatório.");
            }

            if (dataFechamento.Date == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DE FECHAMENTO é obrigatório.");
            }
            else if (dataFechamento.Date < DateTime.Now.Date)
            {
                mensagens.Add("Campo DATA DE FECHAMENTO tem que ser maior ou igual ao dia de hoje.");
            }

            if (usuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatorio.");
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

        public ValidacaoDados ValidaAprovaTodos(List<int> listaReaberturaTurmaId, DateTime dataFechamento, string usuarioId)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados();

            if (listaReaberturaTurmaId.Count <= 0)
            {
                mensagens.Add("Não existem Solicitações para a consulta.");
            }            

            if (dataFechamento.Date == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DE FECHAMENTO é obrigatório.");
            }
            else if (dataFechamento.Date < DateTime.Now.Date)
            {
                mensagens.Add("Campo DATA DE FECHAMENTO tem que ser maior ou igual ao dia de hoje.");
            }

            if (usuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
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
    }
}