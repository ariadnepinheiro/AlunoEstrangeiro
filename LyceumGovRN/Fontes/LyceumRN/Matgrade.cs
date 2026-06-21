using Seeduc.Infra.Data;
using System.Collections.Generic;
using Techne.Lyceum.RN.Entidades;
using System;
using Techne.Lyceum.RN.DTOs;
using System.Data.SqlClient;
using System.Data;

namespace Techne.Lyceum.RN
{
    public class Matgrade
    {
        public void InsereOuAtualizaMatGrade(DataContext contexto, string aluno, string gradeId)
        {	
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = string.Format(
                            @"DECLARE @aluno T_CODIGO,
                                    @grade_id T_NUMERO_GRANDE,
                                    @sit_matgrade T_SIT_MATGRADE	
                                                                		
                                SET @aluno = '{0}'
                                SET @grade_id = {1}
                                SET @sit_matgrade = 'Matriculado'

                                IF NOT EXISTS ( SELECT  *
                                                FROM    LY_MATGRADE
                                                WHERE   ALUNO = @aluno
                                                        AND GRADE_ID = @grade_id
                                                        AND SIT_MATGRADE = @sit_matgrade ) 
                                    INSERT  INTO LY_MATGRADE
                                            (
                                              ALUNO,
                                              GRADE_ID,
                                              SIT_MATGRADE,
                                              DT_ULTALT
                                            )
                                    VALUES  (
                                              @aluno,
                                              @grade_id,
                                              @sit_matgrade,
                                              GETDATE()
                                            )

                                UPDATE  LY_MATGRADE
                                SET     SIT_MATGRADE = 'Cancelado'
                                WHERE   ALUNO = @aluno
                                        AND GRADE_ID <> @grade_id
                                        AND SIT_MATGRADE = 'Matriculado'",
                            aluno,
                            gradeId);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaSuspensao(DataContext contexto, int historicoSuspensaoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE M 
                                        SET    SIT_MATGRADE = 'Cancelado'
                                    FROM LY_MATGRADE m
                                    INNER JOIN Turma.HISTORICOSUSPENSAO h ON h.ALUNO = M.ALUNO
                                    WHERE  SIT_MATGRADE = 'Matriculado'
	                                    and HISTORICOSUSPENSAOID = @HISTORICOSUSPENSAOID  ";

            contextQuery.Parameters.Add("@HISTORICOSUSPENSAOID", SqlDbType.Int, historicoSuspensaoId);

            contexto.ApplyModifications(contextQuery);
        }

        public static void Remover(DataContext context, string aluno, string gradeId)
        {
            var contextQuery = new ContextQuery(
                @" UPDATE  LY_MATGRADE
                                SET     SIT_MATGRADE = @CANCELADO
                    FROM    LY_MATGRADE MG
                    INNER JOIN LY_GRADE_SERIE GS ( NOLOCK ) ON MG.GRADE_ID = GS.GRADE_ID
                    WHERE   MG.ALUNO = @ALUNO
                            AND MG.GRADE_ID = @GRADE_ID
                            AND MG.SIT_MATGRADE = @MATRICULADO 
                            AND NOT EXISTS ( SELECT TOP 1
                                                    1
                                             FROM   LY_MATRICULA M
                                                    INNER JOIN DBO.LY_GRADE_SERIE GS2 ( NOLOCK ) ON GS2.GRADE = M.TURMA
                                                                                  AND GS2.ANO = M.ANO
                                                                                  AND GS2.SEMESTRE = M.SEMESTRE
                                             WHERE  GS2.GRADE = GS.GRADE
                                                    AND GS2.ANO = M.ANO
                                                    AND GS2.SEMESTRE = M.SEMESTRE
                                                    AND MG.ALUNO = M.ALUNO
                                                    AND SIT_MATRICULA = @MATRICULADO  ) ");

            contextQuery.Parameters.Add("@CANCELADO", Matricula.Cancelado);
            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@GRADE_ID", gradeId);
            contextQuery.Parameters.Add("@MATRICULADO", Matricula.Matriculado);

            context.ApplyModifications(contextQuery);
        }

        public void Insere(string aluno, string gradeId, List<ContextQuery> listaContextQuery)
        {
            ContextQuery contextQuery = Insere(aluno, gradeId);
            listaContextQuery.Add(contextQuery);
        }

        public void Insere(string aluno, string gradeId, DataContext dataContext)
        {
            ContextQuery contextQuery = Insere(aluno, gradeId);
            dataContext.ApplyModifications(contextQuery);
        }

        public void InsereMatgradePrincipal(DataContext ctx, string aluno, decimal gradeId)
        {
            ContextQuery contextQuery = new ContextQuery();
            contextQuery.Command = @"  IF NOT EXISTS (SELECT * 
                                           FROM   ly_matgrade 
                                           WHERE  aluno = @aluno 
                                                  AND grade_id = @grade_id 
                                                  AND sit_matgrade = @sit_matgrade) 
                              INSERT INTO ly_matgrade 
                                          (aluno, 
                                           grade_id, 
                                           sit_matgrade, 
                                           dt_ultalt) 
                              VALUES      (@aluno, 
                                           @grade_id, 
                                           @sit_matgrade, 
                                           Getdate()) 

                            UPDATE ly_matgrade 
                            SET    sit_matgrade = 'Cancelado' 
                            WHERE  aluno = @aluno 
                                   AND grade_id <> @grade_id 
                                   AND sit_matgrade = 'Matriculado'  ";

            contextQuery.Parameters.Add("@aluno",TechneDbType.T_CODIGO, aluno);
            contextQuery.Parameters.Add("@grade_id",TechneDbType.T_NUMERO_GRANDE, gradeId);
            contextQuery.Parameters.Add("@sit_matgrade",TechneDbType.T_SIT_MATGRADE, RN.Matricula.Matriculado);

            ctx.ApplyModifications(contextQuery);
        }

        private ContextQuery Insere(string aluno, string gradeId)
        {
            ContextQuery contextQuery = new ContextQuery(
                @" DECLARE @aluno T_CODIGO,
                           @grade_id T_NUMERO_GRANDE,                                    
                           @sit_matgrade T_SIT_MATGRADE	
                                                                		
                   SET @aluno = @MATRICULAALUNO
                   SET @grade_id = @GRADEID                                
                   SET @sit_matgrade = 'Matriculado'

                   IF NOT EXISTS ( SELECT  TOP 1 ALUNO
                                   FROM    LY_MATGRADE
                                   WHERE   ALUNO = @aluno
                                           AND GRADE_ID = @grade_id
                                           AND SIT_MATGRADE = @sit_matgrade ) 
                       INSERT  INTO LY_MATGRADE
                               (
                                 ALUNO,
                                 GRADE_ID,
                                 SIT_MATGRADE,
                                 DT_ULTALT
                               )
                       VALUES  (
                                 @aluno,
                                 @grade_id,
                                 @sit_matgrade,
                                 GETDATE()
                                )");

            contextQuery.Parameters.Add("@MATRICULAALUNO", aluno);
            contextQuery.Parameters.Add("@GRADEID", gradeId);

            return contextQuery;
        }

        public void Transfere(DataContext dataContext, string aluno, string gradeId, string gradeIdOrigem)
        {
            var contextQuery = new ContextQuery(
                @" DECLARE @aluno T_CODIGO,
                                    @grade_id T_NUMERO_GRANDE,   
                                    @grade_id_origem T_NUMERO_GRANDE,                                 
                                    @sit_matgrade T_SIT_MATGRADE	
                                                                		
                                SET @aluno = @MATRICULAALUNO
                                SET @grade_id = @GRADEID   
                                SET @grade_id_origem = @GRADEIDORIGEM                             
                                SET @sit_matgrade = 'Matriculado'

                                IF NOT EXISTS ( SELECT  *
                                                FROM    LY_MATGRADE
                                                WHERE   ALUNO = @aluno
                                                        AND GRADE_ID = @grade_id
                                                        AND SIT_MATGRADE = @sit_matgrade ) 
                                    INSERT  INTO LY_MATGRADE
                                            (
                                              ALUNO,
                                              GRADE_ID,
                                              SIT_MATGRADE,
                                              DT_ULTALT
                                            )
                                    VALUES  (
                                              @aluno,
                                              @grade_id,
                                              @sit_matgrade,
                                              GETDATE()
                                            )

                                UPDATE  LY_MATGRADE
                                SET     SIT_MATGRADE = 'Cancelado'
                                WHERE   ALUNO = @aluno
                                        AND GRADE_ID = @grade_id_origem
                                        AND SIT_MATGRADE = 'Matriculado'
                               ");

            contextQuery.Parameters.Add("@MATRICULAALUNO", aluno);
            contextQuery.Parameters.Add("@GRADEID", gradeId);
            contextQuery.Parameters.Add("@GRADEIDORIGEM", gradeIdOrigem);

            dataContext.ApplyModifications(contextQuery);
        }

        public void InsereParaFechamento(DataContext ctx, LyMatGrade matgrade)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" INSERT  INTO dbo.LY_MATGRADE
                                    ( ALUNO ,
                                      GRADE_ID ,
                                      NUM_CHAMADA ,
                                      SIT_MATGRADE ,
                                      DT_ULTALT
                                    )
                            VALUES  ( @ALUNO ,
                                      @GRADE_ID ,
                                      @NUM_CHAMADA ,
                                      @SIT_MATGRADE ,
                                      @DT_ULTALT
                                    ) ";

                contextQuery.Parameters.Add("@ALUNO", matgrade.Aluno);
                contextQuery.Parameters.Add("@GRADE_ID", matgrade.GradeId);
                contextQuery.Parameters.Add("@NUM_CHAMADA", matgrade.NumChamada);
                contextQuery.Parameters.Add("@SIT_MATGRADE", matgrade.SitMatgrade);
                contextQuery.Parameters.Add("@DT_ULTALT", matgrade.DtUltalt);

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

        public ICollection<LyMatGrade> ObtemListaPor(string aluno, decimal ano, decimal periodo, string curso, string turno, string curriculo, string serie, string grade)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ICollection<LyMatGrade> matGrades = new List<LyMatGrade>();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT  *
                        FROM    LY_MATGRADE MG
                                INNER JOIN LY_GRADE_SERIE GS ON GS.GRADE_ID = MG.GRADE_ID
                        WHERE   MG.ALUNO = @ALUNO
                                AND GS.ANO = @ANO
                                AND GS.SEMESTRE = @SEMESTRE
                                AND GS.CURSO = @CURSO
                                AND GS.TURNO = @TURNO
                                AND GS.CURRICULO = @CURRICULO
                                AND GS.SERIE = @SERIE
                                AND GS.GRADE = @GRADE 
                                AND MG.SIT_MATGRADE = 'Matriculado'"
                };

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@CURRICULO", curriculo);
                contextQuery.Parameters.Add("@SERIE", serie);
                contextQuery.Parameters.Add("@GRADE", grade);

                matGrades = ctx.TryToBindEntities<LyMatGrade>(contextQuery);

                return matGrades;
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

        public void Remove(DataContext ctx, string aluno, decimal gradeId, DateTime dtUltalt)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" DELETE  LY_MATGRADE
                            WHERE   ALUNO = @ALUNO
                                    AND GRADE_ID = @GRADE_ID
                                    AND DT_ULTALT = @DT_ULTALT ";

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@GRADE_ID", gradeId);
                contextQuery.Parameters.Add("@DT_ULTALT", dtUltalt);

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

        public void RemovePorTurma(DataContext ctx, string aluno, decimal ano, decimal periodo, string turma)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE MG
                            FROM   LY_MATGRADE MG
                                   INNER JOIN LY_GRADE_SERIE GS ON GS.GRADE_ID = MG.GRADE_ID
                            WHERE  MG.ALUNO = @ALUNO
                                   AND GS.ANO = @ANO
                                   AND GS.SEMESTRE = @SEMESTRE
                                   AND GS.GRADE = @TURMA  ";

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);
                contextQuery.Parameters.Add("@TURMA", turma);

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

        public void AtivaMatgrade(DataContext contexto, string aluno, int ano, int periodo, string turma)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE MG 
									SET SIT_MATGRADE = 'Matriculado'
							     FROM   LY_MATGRADE MG
                                   INNER JOIN LY_GRADE_SERIE GS ON GS.GRADE_ID = MG.GRADE_ID
                            WHERE  MG.ALUNO = @ALUNO
                                   AND GS.ANO = @ANO
                                   AND GS.SEMESTRE = @SEMESTRE
                                   AND GS.GRADE = @TURMA 
								   AND SIT_MATGRADE <> 'Matriculado' ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, periodo);

            contexto.ApplyModifications(contextQuery);
        }

        public bool PossuiMatGrade(LyMatGrade matgrade)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT  COUNT(*)
                        FROM    LY_MATGRADE
                        WHERE   ALUNO = @ALUNO
                                AND GRADE_ID = @GRADE_ID
                                AND SIT_MATGRADE = @SIT_MATGRADE"

                };

                contextQuery.Parameters.Add("@ALUNO", matgrade.Aluno);
                contextQuery.Parameters.Add("@GRADE_ID", matgrade.GradeId);
                contextQuery.Parameters.Add("@SIT_MATGRADE", matgrade.SitMatgrade);

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

        public int RetornaMatGradePrincipalAtivaPor(DataContext contexto, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;
            SqlDataReader reader = null;

            try
            {
                contextQuery.Command = @" SELECT COUNT(1) AS QUANTIDADE
                                            FROM   LY_MATGRADE MG
											    INNER JOIN LY_GRADE_SERIE GS ON MG.GRADE_ID=GS.GRADE_ID
											    INNER JOIN LY_TURMA T ON GS.ANO = T.ANO
												     AND GS.SEMESTRE = T.SEMESTRE
												     AND GS.GRADE = T.TURMA
                                            WHERE  ALUNO = @ALUNO 
                                                   AND SIT_MATGRADE = 'Matriculado'
											       AND ISNULL(T.ELETIVA, 'N') = 'S'
											       AND ISNULL(T.OPTATIVAREFORCO, 'N') = 'S' ";

                contextQuery.Parameters.Add("@ALUNO", aluno); 
                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["QUANTIDADE"]);
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

        public void AtualizaProgressaoParcial(DataContext ctx, string aluno, decimal gradeIdDestino, decimal gradeIdOrigem)
        {
            ContextQuery contextQuery = new ContextQuery();
            try
            {
                contextQuery.Command = @" IF NOT EXISTS ( SELECT  *
                                                FROM    LY_MATGRADE
                                                WHERE   ALUNO = @aluno
                                                        AND GRADE_ID = @grade_id
                                                        AND SIT_MATGRADE = @sit_matgrade ) 
                                    INSERT  INTO LY_MATGRADE
                                            (
                                              ALUNO,
                                              GRADE_ID,
                                              SIT_MATGRADE,
                                              DT_ULTALT
                                            )
                                    VALUES  (
                                              @aluno,
                                              @grade_id,
                                              @sit_matgrade,
                                              GETDATE()
                                            )

                                UPDATE  MG
                                SET     MG.SIT_MATGRADE = 'Cancelado'
                                FROM    LY_MATGRADE MG
                                        INNER JOIN LY_GRADE_SERIE GS ON GS.GRADE_ID = MG.GRADE_ID
                                WHERE   MG.ALUNO = @aluno
                                        AND MG.GRADE_ID = @grade_id_origem
                                        AND MG.SIT_MATGRADE = 'Matriculado'
                                        AND NOT EXISTS ( SELECT TOP 1
                                                                1
                                                         FROM   DBO.LY_MATRICULA M
                                                         WHERE  MG.ALUNO = M.ALUNO
                                                                AND GS.ANO = M.ANO
                                                                AND GS.SEMESTRE = M.SEMESTRE
                                                                AND M.TURMA = GS.GRADE
                                                                AND M.SIT_MATRICULA = 'Matriculado' ) ";

                contextQuery.Parameters.Add("@aluno", aluno);
                contextQuery.Parameters.Add("@grade_id", gradeIdDestino);
                contextQuery.Parameters.Add("@grade_id_origem", gradeIdOrigem);
                contextQuery.Parameters.Add("@sit_matgrade", "Matriculado");

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

        public void LiberaMatgradeEmPeridosPossiveisPor(DataContext ctx, DadosLiberacaoConfirmacao liberacao, string periodosPossiveis)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = string.Format(@"  UPDATE  MG
                            SET     SIT_MATGRADE = @SIT_CANCELADO
                            FROM    DBO.LY_MATGRADE MG
                                    INNER JOIN DBO.LY_GRADE_TURMA GT ON MG.GRADE_ID = GT.GRADE_ID
                            WHERE   SIT_MATGRADE = @SIT_MATGRADE
                                    AND ALUNO = @ALUNO
                                    AND ANO = @ANO
                                    AND SEMESTRE IN ( {0} ) ", periodosPossiveis);

                contextQuery.Parameters.Add("@SIT_CANCELADO", Matricula.Cancelado);
                contextQuery.Parameters.Add("@SIT_MATGRADE", Matricula.Matriculado);
                contextQuery.Parameters.Add("@ALUNO", liberacao.Aluno);
                contextQuery.Parameters.Add("@ANO", liberacao.Ano);

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

        public void AtualizaSituacaoParaCanceladoPor(string aluno, List<ContextQuery> listaContextQueries)
        {
            ContextQuery contextQuery =
                new ContextQuery(
                    @"UPDATE  LY_MATGRADE
                      SET     SIT_MATGRADE = 'Cancelado'
                      WHERE   ALUNO = @ALUNO
                              AND ( SIT_MATGRADE = 'Matriculado'
                                    OR SIT_MATGRADE = 'Trancado' )",
                new ContextQueryParameter("@ALUNO", aluno));

            listaContextQueries.Add(contextQuery);
        }

        public void InsereMatgrade(DataContext contexto, string aluno, int ano, int periodo, string turma)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT  INTO LY_MATGRADE
                                       (
                                         ALUNO,
                                         GRADE_ID,
                                         SIT_MATGRADE,
                                         DT_ULTALT
                                       )
		                            SELECT DISTINCT @ALUNO,
			                            GS.GRADE_ID,
			                            'Matriculado',
			                            GETDATE()
		                            FROM LY_GRADE_SERIE GS 
		                            WHERE GS.ANO = @ANO
                                            AND GS.SEMESTRE = @SEMESTRE
                                            AND GS.GRADE = @TURMA ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, periodo);

            contexto.ApplyModifications(contextQuery);
        }

        public void CancelaMatgradePor(DataContext ctx, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE LY_MATGRADE 
                        SET    SIT_MATGRADE = 'Cancelado'
                        WHERE  ALUNO = @ALUNO 
                               AND (SIT_MATGRADE = 'Matriculado') ";

                contextQuery.Parameters.Add("@ALUNO", aluno);                

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

        public void CancelaMatgradeEletivaPor(DataContext ctx, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE LY_MATGRADE 
							SET    SIT_MATGRADE = 'Cancelado'
						FROM LY_MATGRADE MG
                            INNER JOIN LY_GRADE_SERIE GS ON MG.GRADE_ID=GS.GRADE_ID
                            INNER JOIN LY_TURMA T ON GS.ANO = T.ANO
                                 AND GS.SEMESTRE = T.SEMESTRE
                                 AND GS.GRADE = T.TURMA
                        WHERE  ALUNO = @ALUNO 
                               AND SIT_MATGRADE = 'Matriculado'
							   AND T.ELETIVA = 'S' ";

                contextQuery.Parameters.Add("@ALUNO", aluno);

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

        public bool PossuiMatgradeCanceladaPor(string aluno, string turma, decimal ano, decimal semestre)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT COUNT(1)
                        FROM LY_MATGRADE MG
                            INNER JOIN LY_GRADE_SERIE GS ON MG.GRADE_ID=GS.GRADE_ID
                            INNER JOIN LY_TURMA T ON GS.ANO = T.ANO
                                 AND GS.SEMESTRE = T.SEMESTRE
                                 AND GS.GRADE = T.TURMA
                        WHERE  ALUNO = @ALUNO 
                               AND SIT_MATGRADE <> 'Matriculado'
							   AND t.TURMA = @TURMA
                               AND t.ANO = @ANO
                               AND t.SEMESTRE = @SEMESTRE  "
                };

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);

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

        public bool PossuiMatgradeAtivaPor(string aluno, string turma, decimal ano, decimal semestre)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT COUNT(1)
                        FROM LY_MATGRADE MG
                            INNER JOIN LY_GRADE_SERIE GS ON MG.GRADE_ID=GS.GRADE_ID
                            INNER JOIN LY_TURMA T ON GS.ANO = T.ANO
                                 AND GS.SEMESTRE = T.SEMESTRE
                                 AND GS.GRADE = T.TURMA
                        WHERE  ALUNO = @ALUNO 
                               AND SIT_MATGRADE = 'Matriculado'
							   AND t.TURMA = @TURMA
                               AND t.ANO = @ANO
                               AND t.SEMESTRE = @SEMESTRE  "
                };

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);

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
    }
}

