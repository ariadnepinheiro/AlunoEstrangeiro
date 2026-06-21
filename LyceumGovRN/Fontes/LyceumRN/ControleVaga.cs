namespace Techne.Lyceum.RN
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Seeduc.Infra.Data;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Lyceum.RN.Util;
    using System.Data.SqlClient;
    using Techne.Lyceum.RN.DTOs;

    public class ControleVaga : RNBase
    {
        public bool PossuiCursoPor(DataContext ctx, string curso)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                FROM TCE_CONTROLE_VAGA
                                WHERE CURSO = @CURSO ";

            contextQuery.Parameters.Add("@CURSO", curso);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public static void Alterar(TceControleVaga controleVaga)
        {
            var contextQuery = new ContextQuery(
                @"UPDATE  TCE_CONTROLE_VAGA
                SET     VAGAS_LIBERADAS = @VAGAS_LIBERADAS,
                        VAGAS_NOVAS = @VAGAS_NOVAS,
                        VAGAS_CONTINUIDADE = @VAGAS_CONTINUIDADE,
                        PARTICIPAMATRICULAFACIL = @PARTICIPAMATRICULAFACIL,
                        VISUALIZAVAGA = @VISUALIZAVAGA,
                        OFERECEVAGAFASE1 = @OFERECEVAGAFASE1,
                        MATRICULA = @MATRICULA,
                        DT_ALTERACAO = GETDATE()
                WHERE   ID_CONTROLE_VAGA = @ID_CONTROLE_VAGA");

            contextQuery.Parameters.Add("@ID_CONTROLE_VAGA", controleVaga.IdControleVaga);
            contextQuery.Parameters.Add("@VAGAS_LIBERADAS", (controleVaga.VagasNovas + controleVaga.VagasContinuidade));
            contextQuery.Parameters.Add("@VAGAS_CONTINUIDADE", controleVaga.VagasContinuidade);
            contextQuery.Parameters.Add("@VAGAS_NOVAS", controleVaga.VagasNovas);
            contextQuery.Parameters.Add("@PARTICIPAMATRICULAFACIL", controleVaga.ParticipaMatriculaFacil);
            contextQuery.Parameters.Add("@VISUALIZAVAGA", controleVaga.VisualizaVaga);
            contextQuery.Parameters.Add("@OFERECEVAGAFASE1", controleVaga.OfereceVagaFase1);
            contextQuery.Parameters.Add("@Matricula", controleVaga.Matricula);

            ExecutarAlteracao(contextQuery);
        }

        public static int CalculaVagasUtilizadasEspera(string censo, int ano, int periodo, int serie, string curso, string turno)
        {
            var contextQuery = new ContextQuery(
                @"SELECT  ( SELECT  COUNT(*)
									FROM    MATRICULA.OPCAOINSCRICAO OP (NOLOCK) 
											INNER JOIN TCE_CONTROLE_VAGA CV (NOLOCK) ON OP.CONTROLEVAGAID = CV.ID_CONTROLE_VAGA
									WHERE   CV.CENSO = @CENSO
											AND CV.ANO = @ANO
											AND CV.PERIODO = @PERIODO
											AND CV.CURSO = @CURSO
											AND CV.SERIE = @SERIE
											AND CV.TURNO = @TURNO
											AND OP.DATACONVOCACAO IS NOT NULL 
                                ) AS VAGAS_UTILIZADAS");

            contextQuery.Parameters.Add("@CENSO", censo);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@CURSO", curso);
            contextQuery.Parameters.Add("@SERIE", serie);
            contextQuery.Parameters.Add("@TURNO", turno);
            contextQuery.Parameters.Add("@STATUS_TRANSFERENCIA", Transferencia.Pendente);
            contextQuery.Parameters.Add("@STATUS_CONFIRMACAO", ConfirmacaoMatricula.Confirmado);

            return ExecutarFuncao(contextQuery);
        }

        public static void Inserir(TceControleVaga controleVaga)
        {
            var contextQuery = new ContextQuery(
                @" INSERT INTO TCE_CONTROLE_VAGA
                                (CENSO,
                                 ANO,
                                 PERIODO,
                                 CURSO,
                                 SERIE,
                                 TURNO,
                                 VAGAS_LIBERADAS,
                                 VAGAS_CONTINUIDADE,
                                 VAGAS_NOVAS,
                                 MATRICULA,
                                 PARTICIPAMATRICULAFACIL,
                                 VISUALIZAVAGA,
                                 VAGAPLANEJADA,
                                 OFERECEVAGAFASE1,
                                 PARALISAMATRICULAFACIL)
                    VALUES      ( @CENSO,
                                  @ANO,
                                  @PERIODO,
                                  @CURSO,
                                  @SERIE,
                                  @TURNO,
                                  @VAGAS_LIBERADAS,
                                  @VAGAS_CONTINUIDADE,
                                  @VAGAS_NOVAS,
                                  @MATRICULA,
                                  @PARTICIPAMATRICULAFACIL,
                                  @VISUALIZAVAGA,
                                  @VAGAPLANEJADA,
                                  @OFERECEVAGAFASE1,
                                  @PARALISAMATRICULAFACIL )  ");

            contextQuery.Parameters.Add("@Censo", controleVaga.Censo);
            contextQuery.Parameters.Add("@Ano", controleVaga.Ano);
            contextQuery.Parameters.Add("@Periodo", controleVaga.Periodo);
            contextQuery.Parameters.Add("@Curso", controleVaga.Curso);
            contextQuery.Parameters.Add("@Serie", controleVaga.Serie);
            contextQuery.Parameters.Add("@Turno", controleVaga.Turno);
            contextQuery.Parameters.Add("@VAGAS_LIBERADAS", (controleVaga.VagasNovas + controleVaga.VagasContinuidade));
            contextQuery.Parameters.Add("@VAGAS_CONTINUIDADE", controleVaga.VagasContinuidade);
            contextQuery.Parameters.Add("@VAGAS_NOVAS", controleVaga.VagasNovas);
            contextQuery.Parameters.Add("@Matricula", controleVaga.Matricula);
            contextQuery.Parameters.Add("@PARTICIPAMATRICULAFACIL", controleVaga.ParticipaMatriculaFacil);
            contextQuery.Parameters.Add("@VISUALIZAVAGA", controleVaga.VisualizaVaga);
            contextQuery.Parameters.Add("@OFERECEVAGAFASE1", controleVaga.OfereceVagaFase1);
            contextQuery.Parameters.Add("@PARALISAMATRICULAFACIL", 0);
            contextQuery.Parameters.Add("@VAGAPLANEJADA", (controleVaga.VagasNovas));

            ExecutarAlteracao(contextQuery);
        }

        public static string Inserir(DataContext context, IList<int> idAgendas, string matricula)
        {
            try
            {
                foreach (var agenda in idAgendas)
                {
                    var contextQuery = new ContextQuery(
                        @" INSERT  INTO DBO.TCE_CONTROLE_VAGA
                                    ( CENSO ,
                                      ANO ,
                                      PERIODO ,
                                      CURSO ,
                                      SERIE ,
                                      TURNO ,
                                      VAGAS_LIBERADAS ,
                                      VAGAS_CONTINUIDADE ,
                                      VAGAS_NOVAS ,
                                      MATRICULA,
                                      PARTICIPAMATRICULAFACIL,
                                      VISUALIZAVAGA,
                                      OFERECEVAGAFASE1,
                                      PARALISAMATRICULAFACIL
                                    )
                                    SELECT  V.CENSO ,
                                            A.ANO ,
                                            A.PERIODO ,
                                            A.CURSO ,
                                            A.SERIE ,
                                            V.TURNO ,
                                            SUM(VAGAS_CONTINUIDADE) + SUM(VAGAS_NOVAS) AS VAGAS_LIBERADAS ,
                                            SUM(VAGAS_CONTINUIDADE) AS VAGAS_CONTINUIDADE ,
                                            SUM(VAGAS_NOVAS) AS VAGAS_NOVAS ,
                                            @MATRICULA AS MATRICULA,
                                            0,
                                            0,
                                            0,
                                            0
                                    FROM    DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                                            INNER JOIN DBO.TCE_CTV_CONF_VAGA V ON A.ID_AGENDA_CONF_TURNO_VAGA = V.ID_AGENDA_CONF_TURNO_VAGA
                                            LEFT JOIN TCE_CTV_RESTRICAO R ON A.ID_AGENDA_CONF_TURNO_VAGA = R.ID_AGENDA_CONF_TURNO_VAGA and v.CENSO = r.CENSO
                                    WHERE   A.ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                                            AND R.ID_RESTRICAO IS NULL
                                            AND NOT EXISTS (SELECT * 
                                                                   FROM   TCE_CONTROLE_VAGA CV 
                                                                   WHERE  CV.ANO = A.ANO 
                                                                          AND CV.PERIODO = A.PERIODO 
                                                                          AND CV.CENSO = V.CENSO 
                                                                          AND CV.CURSO = A.CURSO 
                                                                          AND CV.SERIE = A.SERIE 
                                                                          AND CV.TURNO = V.TURNO) 
                                    GROUP BY V.CENSO ,
                                            A.ANO ,
                                            A.PERIODO ,
                                            A.CURSO ,
                                            A.SERIE ,
                                            V.TURNO ");

                    contextQuery.Parameters.Add("@MATRICULA", matricula);
                    contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", agenda);

                    context.ApplyModifications(contextQuery);
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                context.Abandon();
                return ex.Message;
            }
        }

        public void IncrementaVagaNova(DataContext contexto, int controleVagaId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE TCE_CONTROLE_VAGA
									SET VAGAS_LIBERADAS = VAGAS_CONTINUIDADE + VAGAS_NOVAS + 1,
                                        VAGAS_NOVAS = VAGAS_NOVAS + 1 
									 WHERE ID_CONTROLE_VAGA = @ID_CONTROLE_VAGA ";

            contextQuery.Parameters.Add("@ID_CONTROLE_VAGA", SqlDbType.Int, controleVagaId);


            contexto.ApplyModifications(contextQuery);
        }

        public DataTable ListaParticipaMatriculaFacil(int ano, int periodo, string curso)
        {
            var contextQuery = new ContextQuery(
               @" SELECT DISTINCT
                                    ID_CONTROLE_VAGA ,
                                    V.CENSO ,
                                    V.ANO ,
                                    V.PERIODO ,
                                    V.CURSO ,
                                    V.SERIE ,
                                    V.TURNO ,
                                    VAGAS_LIBERADAS ,
                                    VAGAS_CONTINUIDADE ,
                                    VAGAS_NOVAS ,
                                    V.MATRICULA ,
                                    CASE
										WHEN PARTICIPAMATRICULAFACIL = 1 THEN 'Sim'
										ELSE 'Não'
                                    END PARTICIPAMATRICULAFACIL,
                                    CASE
										WHEN VISUALIZAVAGA = 1 THEN 'Sim'
										ELSE 'Não'
                                    END VISUALIZAVAGA,
                                   PARALISAMATRICULAFACIL,
                                   CASE
										WHEN OFERECEVAGAFASE1 = 1 THEN 'Sim'
										ELSE 'Não'
                                    END  OFERECEVAGAFASE1,
                                    CONVERT(VARCHAR(10), V.DT_CADASTRO, 103) AS DT_CADASTRO ,
                                    CONVERT(VARCHAR(10), V.DT_ALTERACAO, 103) AS DT_ALTERACAO ,
                                    UE.NOME_COMP AS ESCOLA ,
                                    M.NOME AS MUNICIPIO_NOME,
                                    MC.DESCRICAO AS MODALIDADE ,
                                    TC.DESCRICAO AS SEGMENTO ,
                                    C.NOME AS NOME_CURSO ,
                                    T.DESCRICAO AS NOME_TURNO ,  
		                            CASE
			                            WHEN V.CURSO IN ('9999.81','9999.82','9999.83','9999.84',
							                            '9999.85','9999.86','9999.87')
							                            THEN (SELECT COUNT(1)
							                            FROM MATRICULA.MATRICULAESPECIALDISCIPLINA d
								                            INNER JOIN MATRICULA.MATRICULAESPECIAL M 
										                            ON D.MATRICULAESPECIALID = M.MATRICULAESPECIALID
							                            WHERE d.DISCIPLINA = V.CURSO
								                            AND d.TURNO = V.TURNO
								                            AND M.ANO = V.ANO
								                            AND d.DATACONVOCACAO IS NOT NULL)
			                            ELSE ( ( SELECT  COUNT(*)
                                        FROM    dbo.TCE_TRANSFERENCIA_DESTINO TD
                                                INNER JOIN dbo.TCE_TRANSFERENCIA T ON TD.ID_TRANSFERENCIA = T.ID_TRANSFERENCIA
                                        WHERE   TD.CENSO = V.CENSO
                                                AND TD.ANO = V.ANO
                                                AND TD.PERIODO = V.PERIODO
                                                AND TD.CURSO = V.CURSO
                                                AND TD.SERIE = V.SERIE
                                                AND TD.TURNO = V.TURNO
                                                AND STATUS = @STATUS_TRANSFERENCIA
                                                AND NOT EXISTS ( SELECT *
                                                                     FROM   DBO.TCE_CONFIRMACAO_MATRICULA C
                                                                            INNER JOIN DBO.LY_ALUNO A ON A.ALUNO = C.ALUNO
                                                                     WHERE  C.ALUNO = T.ALUNO
                                                                            AND C.ANO = TD.ANO
                                                                            AND C.PERIODO = TD.PERIODO
                                                                            AND C.CENSO = TD.CENSO
                                                                            AND C.CURSO = TD.CURSO
                                                                            AND C.TURNO = TD.TURNO
                                                                            AND C.SERIE = TD.SERIE
                                                                            AND [STATUS] = @STATUS_CONFIRMACAO
                                                                            AND SIT_ALUNO = 'Ativo' )     
                                      ) + ( SELECT  COUNT(*)
                                            FROM    TCE_CONFIRMACAO_MATRICULA CM
                                                    INNER JOIN dbo.LY_ALUNO A ON A.ALUNO = CM.ALUNO
                                            WHERE   CM.CENSO = V.CENSO
                                                    AND CM.ANO = V.ANO
                                                    AND CM.PERIODO = V.PERIODO
                                                    AND CM.CURSO = V.CURSO
                                                    AND CM.SERIE = V.SERIE
                                                    AND CM.TURNO = V.TURNO
                                                    AND [STATUS] = @STATUS_CONFIRMACAO
                                                    AND SIT_ALUNO = 'Ativo'
                                          ) + ( SELECT  COUNT(*)
                                            FROM    MATRICULA.OPCAOINSCRICAO OP (NOLOCK) 
						                            INNER JOIN TCE_CONTROLE_VAGA CV (NOLOCK) ON OP.CONTROLEVAGAID = CV.ID_CONTROLE_VAGA
                                            WHERE   CV.CENSO = V.CENSO
                                                    AND CV.ANO = V.ANO
                                                    AND CV.PERIODO = V.PERIODO
                                                    AND CV.CURSO = V.CURSO
                                                    AND CV.SERIE = V.SERIE
                                                    AND CV.TURNO = V.TURNO
                                                    AND OP.DATACONVOCACAO IS NOT NULL 
                                          ) ) 
			                            END VAGAS_UTILIZADAS,
                                        VAGAPLANEJADA,
			                            ( SELECT  COUNT(*)
                                            FROM    MATRICULA.OPCAOINSCRICAO OP (NOLOCK) 
						                            INNER JOIN TCE_CONTROLE_VAGA CV (NOLOCK) ON OP.CONTROLEVAGAID = CV.ID_CONTROLE_VAGA
                                            WHERE   CV.CENSO = V.CENSO
                                                    AND CV.ANO = V.ANO
                                                    AND CV.PERIODO = V.PERIODO
                                                    AND CV.CURSO = V.CURSO
                                                    AND CV.SERIE = V.SERIE
                                                    AND CV.TURNO = V.TURNO
                                                    AND OP.DATACONVOCACAO IS NULL
                                                    
                                          )  AS FILAESPERA        
                            FROM    TCE_CONTROLE_VAGA V
                                    INNER JOIN ly_unidade_ensino UE ON V.CENSO = UE.UNIDADE_ENS
		                            LEFT JOIN MUNICIPIO M ON UE.MUNICIPIO = M.CODIGO
                                    INNER JOIN LY_CURSO C ON C.CURSO = V.CURSO
                                    INNER JOIN LY_MODALIDADE_CURSO MC ON C.MODALIDADE = MC.MODALIDADE
                                    INNER JOIN LY_TIPO_CURSO TC ON C.TIPO = TC.TIPO
                                    INNER JOIN dbo.LY_SERIE S ON S.SERIE = V.SERIE
                                                                 AND C.CURSO = S.CURSO
                                                                 AND S.TURNO = V.TURNO
                                    INNER JOIN dbo.LY_TURNO T ON T.TURNO = V.TURNO
                            WHERE   ANO = @ANO
                                    AND PERIODO = @PERIODO 
									AND V.CURSO = @CURSO
									AND PARTICIPAMATRICULAFACIL = 1 ");

            contextQuery.Parameters.Add("@CURSO", curso);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@STATUS_TRANSFERENCIA", Transferencia.Pendente);
            contextQuery.Parameters.Add("@STATUS_CONFIRMACAO", ConfirmacaoMatricula.Confirmado);

            return Consultar(contextQuery);
        }

        public DataTable QuantitativosParticipaMatriculaFacil(int ano, int periodo, string curso)
        {
            var contextQuery = new ContextQuery(
               @"  SELECT 
                    (SELECT COUNT(1)    
                    FROM    TCE_CONTROLE_VAGA V
                    WHERE   ANO = @ANO
                           AND PERIODO = @PERIODO 
	                       AND V.CURSO = @CURSO) as QUANTIDADE,  
                    (SELECT COUNT(1)     
                    FROM    TCE_CONTROLE_VAGA V
                    WHERE   ANO = @ANO
                           AND PERIODO = @PERIODO 
	                       AND V.CURSO = @CURSO
	                       AND PARTICIPAMATRICULAFACIL = 1) as PARTICIPAMATRICULAFACIL, 
                    (SELECT COUNT(1)     
                    FROM    TCE_CONTROLE_VAGA V
                    WHERE   ANO = @ANO
                           AND PERIODO = @PERIODO 
	                       AND V.CURSO = @CURSO
	                       AND PARTICIPAMATRICULAFACIL = 1
	                       AND PARALISAMATRICULAFACIL = 1) as PARALISADA, 
                    (SELECT COUNT(1)    
                    FROM    TCE_CONTROLE_VAGA V
                    WHERE   ANO = @ANO
                           AND PERIODO = @PERIODO 
	                       AND V.CURSO = @CURSO
	                       AND PARTICIPAMATRICULAFACIL = 1
	                       AND PARALISAMATRICULAFACIL = 0) as NAOPARALISADA, 
                    (SELECT COUNT(1)
					FROM   MATRICULA.OPCAOINSCRICAO O
							INNER JOIN TCE_CONTROLE_VAGA CV ON O.CONTROLEVAGAID = ID_CONTROLE_VAGA
					WHERE  CV.ANO = @ANO
						   AND CV.PERIODO = @PERIODO 
						   AND CV.CURSO = @CURSO
						   AND O.DATACONVOCACAO IS NULL ) as FILA ");

            contextQuery.Parameters.Add("@CURSO", curso);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);

            return Consultar(contextQuery);
        }

        public static DataTable Listar(string censo, int ano, int periodo)
        {
            var contextQuery = new ContextQuery(
                @" SELECT DISTINCT
                                    ID_CONTROLE_VAGA ,
                                    V.CENSO ,
                                    V.ANO ,
                                    V.PERIODO ,
                                    V.CURSO ,
                                    V.SERIE ,
                                    V.TURNO ,
                                    VAGAS_LIBERADAS ,
                                    VAGAS_CONTINUIDADE ,
                                    VAGAS_NOVAS ,
                                    V.MATRICULA ,
                                    PARTICIPAMATRICULAFACIL,
                                    VISUALIZAVAGA,
                                    PARALISAMATRICULAFACIL,
                                    OFERECEVAGAFASE1,
                                    CONVERT(VARCHAR(10), V.DT_CADASTRO, 103) AS DT_CADASTRO ,
                                    CONVERT(VARCHAR(10), V.DT_ALTERACAO, 103) AS DT_ALTERACAO ,
                                    UE.NOME_COMP AS ESCOLA ,
                                    MC.DESCRICAO AS MODALIDADE ,
                                    TC.DESCRICAO AS SEGMENTO ,
                                    C.NOME AS NOME_CURSO ,
                                    T.DESCRICAO AS NOME_TURNO ,  
		                            CASE
			                            WHEN V.CURSO IN ('9999.81','9999.82','9999.83','9999.84',
							                            '9999.85','9999.86','9999.87')
							                            THEN (SELECT COUNT(1)
							                            FROM MATRICULA.MATRICULAESPECIALDISCIPLINA d
								                            INNER JOIN MATRICULA.MATRICULAESPECIAL M 
										                            ON D.MATRICULAESPECIALID = M.MATRICULAESPECIALID
							                            WHERE d.DISCIPLINA = V.CURSO
								                            AND d.TURNO = V.TURNO
								                            AND M.ANO = V.ANO
								                            AND d.DATACONVOCACAO IS NOT NULL)
			                            ELSE ( ( SELECT  COUNT(*)
                                        FROM    dbo.TCE_TRANSFERENCIA_DESTINO TD
                                                INNER JOIN dbo.TCE_TRANSFERENCIA T ON TD.ID_TRANSFERENCIA = T.ID_TRANSFERENCIA
                                        WHERE   TD.CENSO = V.CENSO
                                                AND TD.ANO = V.ANO
                                                AND TD.PERIODO = V.PERIODO
                                                AND TD.CURSO = V.CURSO
                                                AND TD.SERIE = V.SERIE
                                                AND TD.TURNO = V.TURNO
                                                AND STATUS = @STATUS_TRANSFERENCIA
                                                AND NOT EXISTS ( SELECT *
                                                                     FROM   DBO.TCE_CONFIRMACAO_MATRICULA C
                                                                            INNER JOIN DBO.LY_ALUNO A ON A.ALUNO = C.ALUNO
                                                                     WHERE  C.ALUNO = T.ALUNO
                                                                            AND C.ANO = TD.ANO
                                                                            AND C.PERIODO = TD.PERIODO
                                                                            AND C.CENSO = TD.CENSO
                                                                            AND C.CURSO = TD.CURSO
                                                                            AND C.TURNO = TD.TURNO
                                                                            AND C.SERIE = TD.SERIE
                                                                            AND [STATUS] = @STATUS_CONFIRMACAO
                                                                            AND SIT_ALUNO = 'Ativo' )     
                                      ) + ( SELECT  COUNT(*)
                                            FROM    TCE_CONFIRMACAO_MATRICULA CM
                                                    INNER JOIN dbo.LY_ALUNO A ON A.ALUNO = CM.ALUNO
                                            WHERE   CM.CENSO = V.CENSO
                                                    AND CM.ANO = V.ANO
                                                    AND CM.PERIODO = V.PERIODO
                                                    AND CM.CURSO = V.CURSO
                                                    AND CM.SERIE = V.SERIE
                                                    AND CM.TURNO = V.TURNO
                                                    AND [STATUS] = @STATUS_CONFIRMACAO
                                                    AND SIT_ALUNO = 'Ativo'
                                          ) + ( SELECT  COUNT(*)
                                            FROM    MATRICULA.OPCAOINSCRICAO OP (NOLOCK) 
						                            INNER JOIN TCE_CONTROLE_VAGA CV (NOLOCK) ON OP.CONTROLEVAGAID = CV.ID_CONTROLE_VAGA
                                            WHERE   CV.CENSO = V.CENSO
                                                    AND CV.ANO = V.ANO
                                                    AND CV.PERIODO = V.PERIODO
                                                    AND CV.CURSO = V.CURSO
                                                    AND CV.SERIE = V.SERIE
                                                    AND CV.TURNO = V.TURNO
                                                    AND OP.DATACONVOCACAO IS NOT NULL 
                                          ) ) 
			                            END VAGAS_UTILIZADAS,
                                        VAGAPLANEJADA  ,
									( SELECT  COUNT(*)
                                            FROM    MATRICULA.OPCAOINSCRICAO OP (NOLOCK) 
						                            INNER JOIN TCE_CONTROLE_VAGA CV (NOLOCK) ON OP.CONTROLEVAGAID = CV.ID_CONTROLE_VAGA
                                            WHERE   CV.CENSO = V.CENSO
                                                    AND CV.ANO = V.ANO
                                                    AND CV.PERIODO = V.PERIODO
                                                    AND CV.CURSO = V.CURSO
                                                    AND CV.SERIE = V.SERIE
                                                    AND CV.TURNO = V.TURNO
                                                    
                                          )  as TOTALALUNO,
									( SELECT  COUNT(*)
                                            FROM    MATRICULA.OPCAOINSCRICAO OP (NOLOCK) 
						                            INNER JOIN TCE_CONTROLE_VAGA CV (NOLOCK) ON OP.CONTROLEVAGAID = CV.ID_CONTROLE_VAGA
                                            WHERE   CV.CENSO = V.CENSO
                                                    AND CV.ANO = V.ANO
                                                    AND CV.PERIODO = V.PERIODO
                                                    AND CV.CURSO = V.CURSO
                                                    AND CV.SERIE = V.SERIE
                                                    AND CV.TURNO = V.TURNO
                                                    AND OP.DATACONVOCACAO IS NULL
                                                    
                                          )  AS FILAESPERA      
                            FROM    TCE_CONTROLE_VAGA V
                                    INNER JOIN ly_unidade_ensino UE ON V.CENSO = UE.UNIDADE_ENS
                                    INNER JOIN LY_CURSO C ON C.CURSO = V.CURSO
                                    INNER JOIN LY_MODALIDADE_CURSO MC ON C.MODALIDADE = MC.MODALIDADE
                                    INNER JOIN LY_TIPO_CURSO TC ON C.TIPO = TC.TIPO
                                    INNER JOIN dbo.LY_SERIE S ON S.SERIE = V.SERIE
                                                                 AND C.CURSO = S.CURSO
                                                                 AND S.TURNO = V.TURNO
                                    INNER JOIN dbo.LY_TURNO T ON T.TURNO = V.TURNO
                            WHERE   CENSO = @CENSO
                                    AND ANO = @ANO
                                    AND PERIODO = @PERIODO ");

            contextQuery.Parameters.Add("@CENSO", censo);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@STATUS_TRANSFERENCIA", Transferencia.Pendente);
            contextQuery.Parameters.Add("@STATUS_CONFIRMACAO", ConfirmacaoMatricula.Confirmado);

            return Consultar(contextQuery);
        }

        public static int RetornaPendentesPorTurma(string censo, int ano, int periodo, int serie, string curso, string turno, string turma)
        {
            var contextQuery = new ContextQuery(
                @"SELECT COUNT(*) AS VAGAS_PENDENTES
                FROM   dbo.TCE_TRANSFERENCIA_DESTINO TD
                       INNER JOIN dbo.TCE_TRANSFERENCIA T ON TD.ID_TRANSFERENCIA = T.ID_TRANSFERENCIA
                WHERE  TD.CENSO = @CENSO
                       AND TD.ANO = @ANO
                       AND TD.PERIODO = @PERIODO
                       AND TD.CURSO = @CURSO
                       AND TD.SERIE = @SERIE
                       AND TD.TURNO = @TURNO
                       AND STATUS = @STATUS
                       AND TURMA = @TURMA");

            contextQuery.Parameters.Add("@CENSO", censo);
            contextQuery.Parameters.Add("@CURSO", curso);
            contextQuery.Parameters.Add("@SERIE", serie);
            contextQuery.Parameters.Add("@TURNO", turno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@STATUS", Transferencia.Pendente);
            contextQuery.Parameters.Add("@TURMA", turma);

            return ExecutarFuncao(contextQuery);
        }

        public int ObtemVagasLiberadasTotalPor(string censo, int ano, int periodo, int serie, string curso, string turno)
        {
            DataContext contexto = null;
            int retorno = 0;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                retorno = this.ObtemVagasLiberadasTotalPor(contexto, censo, ano, periodo, serie, curso, turno);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                {
                    contexto.Dispose();
                }
            }
            return retorno;
        }

        public int ObtemVagasLiberadasTotalPor(DataContext contexto, string censo, int ano, int periodo, int serie, string curso, string turno)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT VAGAS_LIBERADAS
                FROM    dbo.TCE_CONTROLE_VAGA
                WHERE   CENSO = @CENSO
                        AND CURSO = @CURSO
                        AND SERIE = @SERIE
                        AND TURNO = @TURNO
                        AND ANO = @ANO
                        AND PERIODO = @PERIODO ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
                contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);
                contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, turno);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["VAGAS_LIBERADAS"]);
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

        public string ObtemMunicipioPor(DataContext contexto, int idControleVaga)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT UE.MUNICIPIO
                                    FROM TCE_CONTROLE_VAGA CV (NOLOCK)
	                                    INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK) ON CV.CENSO = UE.UNIDADE_ENS
                                    WHERE CV.ID_CONTROLE_VAGA = @ID_CONTROLE_VAGA ";

            contextQuery.Parameters.Add("@ID_CONTROLE_VAGA", SqlDbType.Int, idControleVaga);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public static ValidacaoDados Validar(TceControleVaga controleVaga, out int vagasUtilizadas)
        {
            RN.ControleVaga rnControleVaga = new ControleVaga();
            RN.RenovacaoMatricula.Renovacao rnRenovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Renovacao();
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
                                 {
                                     Valido = false
                                 };
            vagasUtilizadas = 0;

            if (controleVaga.IdControleVaga == 0)
            {
                if (string.IsNullOrEmpty(controleVaga.Censo))
                {
                    mensagens.Add("O campo CENSO é obrigatório!");
                }

                if (controleVaga.Ano <= 0)
                {
                    mensagens.Add("O campo ANO LETIVO é obrigatório!");
                }

                if (string.IsNullOrEmpty(controleVaga.Curso))
                {
                    mensagens.Add("O campo CURSO é obrigatório!");
                }
                else
                {
                    //Validações para cursos de disciplinas da matricula especial
                    //9999.81	REGRESSEEDUC - GEOGRAFIA                   
                    //9999.82	REGRESSEEDUC - HISTÓRIA   
                    //9999.83	REGRESSEEDUC - BIOLOGIA                    
                    //9999.84	REGRESSEEDUC - FÍSICA                   
                    //9999.85	REGRESSEEDUC - QUÍMICA
                    //9999.86	REGRESSEEDUC - MATEMÁTICA
                    //9999.87	REGRESSEEDUC - LÍNGUA PORTUGUESA
                    if (controleVaga.Curso == "9999.81" || controleVaga.Curso == "9999.82" || controleVaga.Curso == "9999.83" || controleVaga.Curso == "9999.84"
                        || controleVaga.Curso == "9999.85" || controleVaga.Curso == "9999.86" || controleVaga.Curso == "9999.87")
                    {
                        //Caso sejá um dos cursos de matricula especial, validar se estão de acordo com regra para lançamento
                        if (controleVaga.Periodo != 0)
                        {
                            mensagens.Add("Os CURSO que representam as Disciplinas de turma Reforço apenas podem ser cadastrados no periodo 0!");
                        }

                        if (controleVaga.Censo != "33183554") //Escola onde serão lançadas as disciplinas
                        {
                            mensagens.Add("Os CURSO que representam as Disciplinas de turma Reforço apenas podem ser cadastrados na escola 33183554 - CE CARLOS WALTER MARINHO CAMPOS!");
                        }

                        if (controleVaga.Serie != 3) //Serie onde serão lançadas as disciplinas
                        {
                            mensagens.Add("Os CURSO que representam as Disciplinas de turma Reforço apenas podem ser cadastrados na serie 3!");
                        }
                    }
                }

                if (controleVaga.Serie <= 0)
                {
                    mensagens.Add("O campo SÉRIE é obrigatório!");
                }

                if (string.IsNullOrEmpty(controleVaga.Turno))
                {
                    mensagens.Add("O campo TURNO é obrigatório!");
                }
            }

            if (controleVaga.VagasLiberadas < 0)
            {
                mensagens.Add("O campo VAGAS LIBERADAS é obrigatório!");
            }

            if (controleVaga.VagasNovas < 0)
            {
                mensagens.Add("O campo VAGAS NOVAS é obrigatório!");
            }

            if (controleVaga.VagasContinuidade < 0)
            {
                mensagens.Add("O campo VAGAS CONTINUIDADE é obrigatório!");
            }

            if (controleVaga.VagasLiberadas != (controleVaga.VagasContinuidade + controleVaga.VagasNovas))
            {
                mensagens.Add("As VAGAS LIBERADAS devem ser igual as vagas de continuidade + vagas novas!");
            }
            else
            {
                vagasUtilizadas = rnControleVaga.ObtemVagasUtilizadasTotalPor(
                        controleVaga.Censo,
                        controleVaga.Ano,
                        controleVaga.Periodo,
                       controleVaga.Serie,
                       controleVaga.Curso,
                       controleVaga.Turno);

                //Verifica se liberadas é maior que utilizadas
                if (controleVaga.VagasLiberadas < vagasUtilizadas)
                {
                    mensagens.Add("As VAGAS LIBERADAS devem ser maior ou igual as utilizadas!");
                }
            }

            if (!controleVaga.ParticipaMatriculaFacil && controleVaga.OfereceVagaFase1)
            {
                mensagens.Add("A opção Oferece vagas na 1ª Fase, apenas pode ser marcada para cursos que participam do matricula facil.");
            }

            if (mensagens.Count == 0)
            {
                if (controleVaga.IdControleVaga == 0)
                {
                    using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
                    {
                        var contextQuery = new ContextQuery(
                        @"SELECT  1
                    FROM    dbo.TCE_CONTROLE_VAGA
                    WHERE   CENSO = @CENSO
                            AND ANO = @ANO
                            AND PERIODO = @PERIODO
                            AND CURSO = @CURSO
                            AND SERIE = @SERIE
                            AND TURNO = @TURNO");

                        contextQuery.Parameters.Add("@CENSO", controleVaga.Censo);
                        contextQuery.Parameters.Add("@ANO", controleVaga.Ano);
                        contextQuery.Parameters.Add("@PERIODO", controleVaga.Periodo);
                        contextQuery.Parameters.Add("@CURSO", controleVaga.Curso);
                        contextQuery.Parameters.Add("@SERIE", controleVaga.Serie);
                        contextQuery.Parameters.Add("@TURNO", controleVaga.Turno);

                        var obj = ctx.GetReturnValue(contextQuery);

                        if (obj != null)
                        {
                            mensagens.Add("Já existe um quadro de vagas cadastrado com estes mesmos dados.");
                        }

                        contextQuery = new ContextQuery(
                            @" SELECT TOP 1 1 FROM DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA
                            WHERE ANO = @ANO
                            AND PERIODO = @PERIODO
                            AND CURSO = @CURSO
                            AND SERIE = @SERIE
                            AND ENCERRADO = 0 ");

                        contextQuery.Parameters.Add("@ANO", controleVaga.Ano);
                        contextQuery.Parameters.Add("@PERIODO", controleVaga.Periodo);
                        contextQuery.Parameters.Add("@CURSO", controleVaga.Curso);
                        contextQuery.Parameters.Add("@SERIE", controleVaga.Serie);

                        obj = ctx.GetReturnValue(contextQuery);

                        if (obj != null)
                        {
                            mensagens.Add("Não é possivel cadastrar esse quadro de vagas, pois o lançamento de vagas para este ANO / PERIODO / CURSO / SERIE ainda não foi encerrado.");
                        }
                    }
                }

                //Verifica se existe vagas de continuidade
                if (controleVaga.VagasContinuidade <= 0)
                {
                    //Caso não exista verifica se existe renovaçao ativa
                    if (rnRenovacao.PossuiRenovacaoAtivaPor(controleVaga.Ano, controleVaga.Periodo, controleVaga.Censo, controleVaga.Curso, controleVaga.Turno, controleVaga.Serie, "VC"))
                    {
                        mensagens.Add("Não é permitido zerar as vagas de continuidade pois já existem renovações. Será necessário cancelar as renovações antes.");
                    }
                }

                //Verifica se existe vagas novas
                if (controleVaga.VagasNovas <= 0)
                {
                    //Caso não exista verifica se existe renovaçao ativa
                    if (rnRenovacao.PossuiRenovacaoAtivaPor(controleVaga.Ano, controleVaga.Periodo, controleVaga.Censo, controleVaga.Curso, controleVaga.Turno, controleVaga.Serie, "VN"))
                    {
                        mensagens.Add("Não é permitido zerar as vagas novas pois já existem renovações. Será necessário cancelar as renovações antes.");
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

        public static DataTable RetornaVagasContinuidadeNova(string censo, int ano, int periodo, int serie, string curso, string turno)
        {
            var contextQuery = new ContextQuery(
                @"SELECT  DISTINCT
                        ID_CONTROLE_VAGA,
                        V.CENSO,
                        V.ANO,
                        V.PERIODO,
                        V.CURSO,
                        V.SERIE,
                        V.TURNO,
                        PARTICIPAMATRICULAFACIL,
                        PARALISAMATRICULAFACIL,
                        OFERECEVAGAFASE1,
                        VISUALIZAVAGA,
                        VAGAS_CONTINUIDADE ,
                        VAGAS_NOVAS ,
                        VAGAPLANEJADA
                FROM    TCE_CONTROLE_VAGA V                       
                WHERE    CENSO = @CENSO
                        AND CURSO = @CURSO
                        AND SERIE = @SERIE
                        AND TURNO = @TURNO
                        AND ANO = @ANO
                        AND PERIODO = @PERIODO");

            contextQuery.Parameters.Add("@CENSO", censo);
            contextQuery.Parameters.Add("@CURSO", curso);
            contextQuery.Parameters.Add("@SERIE", serie);
            contextQuery.Parameters.Add("@TURNO", turno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);

            return Consultar(contextQuery);
        }

        public int CalculaVagasUtilizadasContinuidade(string censo, int ano, int periodo, int serie, string curso, string turno)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*) AS VAGAS
                                FROM    TCE_CONFIRMACAO_MATRICULA CM (nolock)
                                INNER JOIN dbo.LY_ALUNO A (nolock) ON A.ALUNO = CM.ALUNO
                                WHERE   CM.CENSO = @CENSO
                                        AND CM.ANO = @ANO
                                        AND CM.PERIODO = @PERIODO
                                        AND CM.CURSO = @CURSO
                                        AND CM.SERIE = @SERIE
                                        AND CM.TURNO = @TURNO
                                        AND CM.TIPOVAGAOCUPADA = 'VC'
                                        AND [STATUS] = @STATUS_CONFIRMACAO
                                        AND SIT_ALUNO = 'Ativo'  ";

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@SERIE", serie);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@STATUS_CONFIRMACAO", ConfirmacaoMatricula.Confirmado);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["VAGAS"]);
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }

        public int CalculaVagasUtilizadasNovas(string censo, int ano, int periodo, int serie, string curso, string turno)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT ( ( SELECT  COUNT(*)
                            FROM    dbo.TCE_TRANSFERENCIA_DESTINO TD
                                    INNER JOIN dbo.TCE_TRANSFERENCIA T ON TD.ID_TRANSFERENCIA = T.ID_TRANSFERENCIA
                            WHERE   TD.CENSO = @CENSO
                                    AND TD.ANO = @ANO
                                    AND TD.PERIODO = @PERIODO
                                    AND TD.CURSO = @CURSO
                                    AND TD.SERIE = @SERIE
                                    AND TD.TURNO = @TURNO
                                    AND STATUS = @STATUS_TRANSFERENCIA
                                    AND NOT EXISTS ( SELECT *
                                                     FROM   DBO.TCE_CONFIRMACAO_MATRICULA C
                                                            INNER JOIN DBO.LY_ALUNO A ON A.ALUNO = C.ALUNO
                                                     WHERE  C.ALUNO = T.ALUNO
                                                            AND C.ANO = TD.ANO
                                                            AND C.PERIODO = TD.PERIODO
                                                            AND C.CENSO = TD.CENSO
                                                            AND C.CURSO = TD.CURSO
                                                            AND C.TURNO = TD.TURNO
                                                            AND C.SERIE = TD.SERIE
                                                            AND C.TIPOVAGAOCUPADA = 'VN'
                                                            AND [STATUS] = @STATUS_CONFIRMACAO
                                                            AND SIT_ALUNO = 'Ativo' )     
                          ) + ( SELECT  COUNT(*)
                                FROM    TCE_CONFIRMACAO_MATRICULA CM
                                INNER JOIN dbo.LY_ALUNO A ON A.ALUNO =CM.ALUNO
                                WHERE   CM.CENSO = @CENSO
                                        AND CM.ANO = @ANO
                                        AND CM.PERIODO = @PERIODO
                                        AND CM.CURSO = @CURSO
                                        AND CM.SERIE = @SERIE
                                        AND CM.TURNO = @TURNO
                                        AND [STATUS] = @STATUS_CONFIRMACAO
                                        AND CM.TIPOVAGAOCUPADA = 'VN'
                                        AND SIT_ALUNO = 'Ativo' 
                              ) ) AS VAGAS_UTILIZADAS";

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@SERIE", serie);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@STATUS_TRANSFERENCIA", Transferencia.Pendente);
                contextQuery.Parameters.Add("@STATUS_CONFIRMACAO", ConfirmacaoMatricula.Confirmado);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["VAGAS_UTILIZADAS"]);
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }

        public int ObtemVagasUtilizadasTotalPor(string censo, int ano, int periodo, int serie, string curso, string turno)
        {
            DataContext contexto = null;
            int retorno = 0;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                retorno = this.ObtemVagasUtilizadasTotalPor(contexto, censo, ano, periodo, serie, curso, turno);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                {
                    contexto.Dispose();
                }
            }
            return retorno;
        }

        public int ObtemVagasUtilizadasTotalPor(DataContext ctx, string censo, int ano, int periodo, int serie, string curso, string turno)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;

            //Contam vagas: Transferencia pendentes + confirmaçoes confirmadas + opçoes convocadas
            contextQuery.Command = @" SELECT ( ( SELECT  COUNT(*)
                            FROM    dbo.TCE_TRANSFERENCIA_DESTINO TD
                                    INNER JOIN dbo.TCE_TRANSFERENCIA T ON TD.ID_TRANSFERENCIA = T.ID_TRANSFERENCIA
                            WHERE   TD.CENSO = @CENSO
                                    AND TD.ANO = @ANO
                                    AND TD.PERIODO = @PERIODO
                                    AND TD.CURSO = @CURSO
                                    AND TD.SERIE = @SERIE
                                    AND TD.TURNO = @TURNO
                                    AND STATUS = @STATUS_TRANSFERENCIA
                                    AND NOT EXISTS ( SELECT *
                                                     FROM   DBO.TCE_CONFIRMACAO_MATRICULA C
                                                            INNER JOIN DBO.LY_ALUNO A ON A.ALUNO = C.ALUNO
                                                     WHERE  C.ALUNO = T.ALUNO
                                                            AND C.ANO = TD.ANO
                                                            AND C.PERIODO = TD.PERIODO
                                                            AND C.CENSO = TD.CENSO
                                                            AND C.CURSO = TD.CURSO
                                                            AND C.TURNO = TD.TURNO
                                                            AND C.SERIE = TD.SERIE
                                                            AND [STATUS] = @STATUS_CONFIRMACAO
                                                            AND SIT_ALUNO = 'Ativo' )     
                          ) + ( SELECT  COUNT(*)
                                FROM    TCE_CONFIRMACAO_MATRICULA CM
                                INNER JOIN dbo.LY_ALUNO A ON A.ALUNO =CM.ALUNO
                                WHERE   CM.CENSO = @CENSO
                                        AND CM.ANO = @ANO
                                        AND CM.PERIODO = @PERIODO
                                        AND CM.CURSO = @CURSO
                                        AND CM.SERIE = @SERIE
                                        AND CM.TURNO = @TURNO
                                        AND [STATUS] = @STATUS_CONFIRMACAO
                                        AND SIT_ALUNO = 'Ativo' 
                              ) + ( SELECT  COUNT(*)
									FROM    MATRICULA.OPCAOINSCRICAO OP (NOLOCK) 
											INNER JOIN TCE_CONTROLE_VAGA CV (NOLOCK) ON OP.CONTROLEVAGAID = CV.ID_CONTROLE_VAGA
									WHERE   CV.CENSO = @CENSO
											AND CV.ANO = @ANO
											AND CV.PERIODO = @PERIODO
											AND CV.CURSO = @CURSO
											AND CV.SERIE = @SERIE
											AND CV.TURNO = @TURNO
											AND OP.DATACONVOCACAO IS NOT NULL 
                                ) ) AS VAGAS_UTILIZADAS ";

            contextQuery.Parameters.Add("@CENSO", censo);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@CURSO", curso);
            contextQuery.Parameters.Add("@SERIE", serie);
            contextQuery.Parameters.Add("@TURNO", turno);
            contextQuery.Parameters.Add("@STATUS_TRANSFERENCIA", Transferencia.Pendente);
            contextQuery.Parameters.Add("@STATUS_CONFIRMACAO", ConfirmacaoMatricula.Confirmado);

            reader = ctx.GetDataReader(contextQuery);

            while (reader.Read())
            {
                retorno = Convert.ToInt32(reader["VAGAS_UTILIZADAS"]);
            }

            if (reader != null)
            {
                reader.Close();
            }

            return retorno;
        }

        public TceControleVaga Carregar(int idControleVaga)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            TceControleVaga controle = new TceControleVaga();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT 
                                                *
                                        FROM    TCE_CONTROLE_VAGA (NOLOCK)
                                        WHERE  ID_CONTROLE_VAGA=@ID_CONTROLE_VAGA
                                                   ";

                contextQuery.Parameters.Add("@ID_CONTROLE_VAGA", idControleVaga);

                controle = ctx.TryToBindEntity<TceControleVaga>(contextQuery);

                return controle;
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

        public TceControleVaga ObtemControleVagaOpcaoPor(DataContext contexto, int opcaoInscricaoId)
        {
            TceControleVaga controleVaga = new TceControleVaga();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"   SELECT CV.*
                                        FROM TCE_CONTROLE_VAGA CV (NOLOCK)
										    INNER JOIN MATRICULA.OPCAOINSCRICAO O (NOLOCK) 
													    ON CV.ID_CONTROLE_VAGA = O.CONTROLEVAGAID
                                        WHERE O.OPCAOINSCRICAOID = @OPCAOINSCRICAOID ";

            contextQuery.Parameters.Add("@OPCAOINSCRICAOID", SqlDbType.Int, opcaoInscricaoId);

            controleVaga = contexto.TryToBindEntity<TceControleVaga>(contextQuery);

            return controleVaga;
        }

        public TceControleVaga ObtemPor(DataContext contexto, int idControleVaga)
        {
            TceControleVaga controleVaga = new TceControleVaga();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"   SELECT *
                                        FROM TCE_CONTROLE_VAGA CV (NOLOCK)
                                        WHERE ID_CONTROLE_VAGA = @ID_CONTROLE_VAGA ";

            contextQuery.Parameters.Add("@ID_CONTROLE_VAGA", SqlDbType.Int, idControleVaga);

            controleVaga = contexto.TryToBindEntity<TceControleVaga>(contextQuery);

            return controleVaga;
        }

        public TceControleVaga ObtemPor(DataContext contexto, int ano, int periodo, string censo, string curso, string turno, int serie)
        {
            TceControleVaga controleVaga = new TceControleVaga();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT * 
                                    FROM TCE_CONTROLE_VAGA (NOLOCK)
                                    WHERE ANO = @ANO
	                                    AND PERIODO = @PERIODO
	                                    AND CENSO = @CENSO
	                                    AND CURSO = @CURSO
	                                    AND TURNO = @TURNO
	                                    AND SERIE = @SERIE ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
            contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, turno);
            contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);

            controleVaga = contexto.TryToBindEntity<TceControleVaga>(contextQuery);

            return controleVaga;
        }

        public bool PartipaMatriculaFacilPor(string unidade, int ano, int periodo, string curso, int serie, string turno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                possui = this.PartipaMatriculaFacilPor(ctx, unidade, ano, periodo, curso, serie, turno);
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

        public bool PartipaMatriculaFacilPor(DataContext ctx, string unidade, int ano, int periodo, string curso, int serie, string turno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                            FROM   TCE_CONTROLE_VAGA
                            WHERE CENSO = @CENSO
	                            AND ANO = @ANO 
	                            AND PERIODO = @PERIODO
                                AND CURSO = @CURSO 
                                AND SERIE = @SERIE
                                AND TURNO = @TURNO
                                AND PARTICIPAMATRICULAFACIL = 1";

            contextQuery.Parameters.Add("@CENSO", unidade);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@CURSO", curso);
            contextQuery.Parameters.Add("@SERIE", serie);
            contextQuery.Parameters.Add("@TURNO", turno);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DadosControleVaga ObtemDadosControleVagaPor(int idControleVaga)
        {
            DadosControleVaga dados = new DadosControleVaga();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                dados = this.ObtemDadosControleVagaPor(contexto, idControleVaga);
                return dados;
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

        private DadosControleVaga ObtemDadosControleVagaPor(DataContext contexto, int idControleVaga)
        {
            DadosControleVaga dados = new DadosControleVaga();
            SqlDataReader dataReader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT CV.ID_CONTROLE_VAGA,
							   CV.CENSO,
							   CV.ANO,
							   CV.PERIODO,
							   CU.MODALIDADE ,
							   CU.TIPO ,
							   CV.CURSO,
							   CV.SERIE,
							   CV.TURNO							   

                        FROM   TCE_CONTROLE_VAGA CV (NOLOCK)									
							   INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK)
									ON CV.CENSO = UE. UNIDADE_ENS
							   INNER JOIN LY_CURSO CU (NOLOCK)
									ON CV.CURSO = CU.CURSO
							   INNER JOIN LY_MODALIDADE_CURSO MO (NOLOCK)
									ON CU.MODALIDADE = MO.MODALIDADE 
							   INNER JOIN LY_TIPO_CURSO TC (NOLOCK)
									ON TC.TIPO = CU.TIPO	
                        WHERE CV.ID_CONTROLE_VAGA = @CONTROLEVAGAID ";

                contextQuery.Parameters.Add("@CONTROLEVAGAID", SqlDbType.Int, idControleVaga);

                dataReader = contexto.GetDataReader(contextQuery);

                while (dataReader.Read())
                {
                    dados.Censo = Convert.ToString(dataReader["CENSO"]);
                    dados.Ano = Convert.ToInt32(dataReader["ANO"]);
                    dados.Periodo = Convert.ToInt32(dataReader["PERIODO"]);
                    dados.Modalidade = Convert.ToString(dataReader["MODALIDADE"]);
                    dados.Segmento = Convert.ToString(dataReader["TIPO"]);
                    dados.Curso = Convert.ToString(dataReader["CURSO"]);
                    dados.Serie = Convert.ToInt32(dataReader["SERIE"]);
                    dados.Turno = Convert.ToString(dataReader["TURNO"]);
                    dados.ControleVagaId = Convert.ToInt32(dataReader["ID_CONTROLE_VAGA"]);
                }

                return dados;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
            }
        }

        public DataTable ListaVagaMatriculaFacilPor(string censo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            RN.PeriodoLetivo rnPeriodoLetivo = new PeriodoLetivo();
            string periodos = string.Empty;
            int ano = 0;

            try
            {
                ano = rnPeriodoLetivo.ObtemAnoAberto();

                if (ano > 0)
                {
                    periodos = rnPeriodoLetivo.ObtemPeriodoAbertoPor(ano);

                    contextQuery.Command = string.Format(@" SELECT DISTINCT ID_CONTROLE_VAGA, 
                                                V.CENSO, 
                                                V.ANO, 
                                                V.PERIODO, 
                                                V.CURSO, 
                                                C.MODALIDADE, 
                                                V.SERIE, 
                                                V.TURNO, 
                                                VAGAS_LIBERADAS, 
                                                VAGAS_CONTINUIDADE, 
                                                VAGAS_NOVAS, 
                                                ( (SELECT COUNT(*) 
                                                   FROM   DBO.TCE_TRANSFERENCIA_DESTINO TD (NOLOCK) 
                                                          INNER JOIN DBO.TCE_TRANSFERENCIA T (NOLOCK) 
                                                                  ON TD.ID_TRANSFERENCIA = T.ID_TRANSFERENCIA 
                                                   WHERE  TD.CENSO = V.CENSO 
                                                          AND TD.ANO = V.ANO 
                                                          AND TD.PERIODO = V.PERIODO 
                                                          AND TD.CURSO = V.CURSO 
                                                          AND TD.SERIE = V.SERIE 
                                                          AND TD.TURNO = V.TURNO 
                                                          AND STATUS = 'PENDENTE' 
                                                          AND NOT EXISTS (SELECT * 
                                                                          FROM   DBO.TCE_CONFIRMACAO_MATRICULA C 
                                                                                 (NOLOCK) 
                                                                                 INNER JOIN DBO.LY_ALUNO A ( 
                                                                                            NOLOCK) 
                                                                                         ON A.ALUNO = C.ALUNO 
                                                                          WHERE  C.ALUNO = T.ALUNO 
                                                                                 AND C.ANO = TD.ANO 
                                                                                 AND C.PERIODO = TD.PERIODO 
                                                                                 AND C.CENSO = TD.CENSO 
                                                                                 AND C.CURSO = TD.CURSO 
                                                                                 AND C.TURNO = TD.TURNO 
                                                                                 AND C.SERIE = TD.SERIE 
                                                                                 AND [STATUS] = 'CONFIRMADO' 
                                                                                 AND SIT_ALUNO = 'ATIVO')) 
                                                  + (SELECT COUNT(*) 
                                                     FROM   TCE_CONFIRMACAO_MATRICULA CM (NOLOCK) 
                                                            INNER JOIN DBO.LY_ALUNO A (NOLOCK) 
                                                                    ON A.ALUNO = CM.ALUNO 
                                                     WHERE  CM.CENSO = V.CENSO 
                                                            AND CM.ANO = V.ANO 
                                                            AND CM.PERIODO = V.PERIODO 
                                                            AND CM.CURSO = V.CURSO 
                                                            AND CM.SERIE = V.SERIE 
                                                            AND CM.TURNO = V.TURNO 
                                                            AND [STATUS] = 'CONFIRMADO' 
                                                            AND SIT_ALUNO = 'ATIVO') 
                                                  + (SELECT COUNT(*) 
                                                     FROM   MATRICULA.OPCAOINSCRICAO OP (NOLOCK) 
                                                            INNER JOIN TCE_CONTROLE_VAGA CV (NOLOCK) 
                                                                    ON OP.CONTROLEVAGAID = CV.ID_CONTROLE_VAGA 
                                                     WHERE  CV.CENSO = V.CENSO 
                                                            AND CV.ANO = V.ANO 
                                                            AND CV.PERIODO = V.PERIODO 
                                                            AND CV.CURSO = V.CURSO 
                                                            AND CV.SERIE = V.SERIE 
                                                            AND CV.TURNO = V.TURNO 
                                                            AND OP.DATACONVOCACAO IS NOT NULL) ) AS 
                                                VAGAS_UTILIZADA,
												(SELECT COUNT(1) 
														FROM MATRICULA.VAGARESERVADA VR (NOLOCK) 
														WHERE VR.CONTROLEVAGAID = V.ID_CONTROLE_VAGA) AS RESERVADAS
                                INTO   #VAGAS 
                                FROM   TCE_CONTROLE_VAGA V (NOLOCK) 
                                       INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK) 
                                               ON V.CENSO = UE.UNIDADE_ENS 
                                       INNER JOIN LY_CURSO C (NOLOCK) 
                                               ON C.CURSO = V.CURSO 
                                WHERE  V.CENSO = @CENSO  
                                       AND PARTICIPAMATRICULAFACIL = 1 
                                       AND PARALISAMATRICULAFACIL = 0
                                       AND V.ANO = @ANO 
	                                   AND V.PERIODO IN  ( {0} )

                                SELECT ID_CONTROLE_VAGA, 
                                       V.CENSO, 
                                       C.CURSO, 
                                       C.NOME                                AS DESCRICAOCURSO, 
                                       M.DESCRICAO                           AS MODALIDADE, 
                                       T.DESCRICAO                           AS TIPOCURSO, 
                                       V.TURNO, 
                                       TU.DESCRICAO                          AS DESCRICAOTURNO, 
                                       V.SERIE,
									   ( VAGAS_LIBERADAS - VAGAS_UTILIZADA ) - RESERVADAS as VAGAS,
									    RESERVADAS,
                                       ( VAGAS_LIBERADAS - VAGAS_UTILIZADA ) AS TOTAL
                                FROM   #VAGAS V 
                                       INNER JOIN LY_CURSO C (NOLOCK) 
                                               ON V.CURSO = C.CURSO 
                                       INNER JOIN LY_TIPO_CURSO T (NOLOCK) 
                                               ON C.TIPO = T.TIPO 
                                       INNER JOIN LY_TURNO TU (NOLOCK) 
                                               ON V.TURNO = TU.TURNO 
                                       INNER JOIN LY_MODALIDADE_CURSO M (NOLOCK) 
                                               ON V.MODALIDADE = M.MODALIDADE 
                                WHERE  ( VAGAS_LIBERADAS - VAGAS_UTILIZADA ) > 0 
                                ORDER  BY C.NOME, 
                                          SERIE, 
                                          TURNO 

                                DROP TABLE #VAGAS  ", periodos);

                    contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                    contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);

                    dt = contexto.GetDataTable(contextQuery);
                }
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

        public DataTable ListaTurnoMatriculaRegresPor(int ano, string curso)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            try
            {


                contextQuery.Command = @" SELECT DISTINCT                                                
                                                V.TURNO, T.DESCRICAO
                                FROM   TCE_CONTROLE_VAGA V (NOLOCK)                                       
                                       INNER JOIN LY_TURNO T (NOLOCK) 
                                               ON T.TURNO = v.TURNO 
                                WHERE V.ANO = @ANO  
                                       AND V.PERIODO = 0                                        
                                       AND V.CENSO = '33183554'
                                       AND V.CURSO = @CURSO
	                                   AND V.SERIE = 3
                                    ";


                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);

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

        public ValidacaoDados ValidaParalisacao(int ano, int periodo, string curso, int motivoRetiradaFila, string usuarioId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (periodo < 0)
            {
                mensagens.Add("Campo PERIODO é obrigatório.");
            }

            if (curso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CURSO é obrigatório.");
            }

            if (motivoRetiradaFila <= 0)
            {
                mensagens.Add("Campo ALTERA SITUAÇÃO é obrigatório.");
            }

            if (usuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se o curso tem alguma linha marcada como Participamatricula Facil
                    if (!this.ExisteMatriculaFacilPor(contexto, ano, periodo, curso))
                    {
                        mensagens.Add("Não existe nenhuma opção marcada como participa matricula fácil para este curso.");
                    }
                    else
                    {
                        //Verifica se todos já estao NÃO paralisados
                        if (!this.ExistePor(contexto, ano, periodo, curso, false))
                        {
                            mensagens.Add("Não existe nenhuma opção marcada como não paralisada para este curso.");
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

        private bool ExisteMatriculaFacilPor(DataContext contexto, int ano, int periodo, string curso)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(1)     
                                    FROM    TCE_CONTROLE_VAGA V
                                    WHERE   ANO = @ANO
                                           AND PERIODO = @PERIODO 
	                                       AND V.CURSO = @CURSO
	                                       AND PARTICIPAMATRICULAFACIL = 1 ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool ExistePor(DataContext contexto, int ano, int periodo, string curso, bool paralisado)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(1)     
                                    FROM    TCE_CONTROLE_VAGA V
                                    WHERE   ANO = @ANO
                                           AND PERIODO = @PERIODO 
	                                       AND V.CURSO = @CURSO
	                                       AND PARALISAMATRICULAFACIL = @PARALISAMATRICULAFACIL ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
            contextQuery.Parameters.Add("@PARALISAMATRICULAFACIL", SqlDbType.Bit, paralisado);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public int Paralisa(int ano, int periodo, string curso, int motivoRetiradaFila, string usuarioId)
        {
            int retorno = 0;
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.Matriculas.OpcaoInscricao rnOpcaoInscricao = new Techne.Lyceum.RN.Matriculas.OpcaoInscricao();
            RN.Matriculas.OpcaoInscricaoHist rnOpcaoInscricaoHist = new Techne.Lyceum.RN.Matriculas.OpcaoInscricaoHist();

            try
            {
                //Joga fila para historico
                rnOpcaoInscricaoHist.InsereRetiradaFila(contexto, ano, periodo, curso, motivoRetiradaFila);

                //Deleta fila
                rnOpcaoInscricao.RemoveRetiradaFila(contexto, ano, periodo, curso);

                //Paralisa curso
                retorno = this.Paralisa(contexto, ano, periodo, curso, usuarioId);
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

            return retorno;
        }

        private int Paralisa(DataContext contexto, int ano, int periodo, string curso, string usuarioId)
        {
            int retorno = 0;
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE TCE_CONTROLE_VAGA
	                                        SET PARALISAMATRICULAFACIL = 1,
		                                        MATRICULA = @MATRICULA,
		                                        DT_ALTERACAO = @DT_ALTERACAO
                                        WHERE ANO = @ANO
										   AND PERIODO = @PERIODO 
										   AND CURSO = @CURSO
										   AND PARTICIPAMATRICULAFACIL = 1 ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DT_ALTERACAO", SqlDbType.DateTime, DateTime.Now);

            retorno = contexto.ApplyModifications(contextQuery);

            return retorno;
        }

        public ValidacaoDados ValidaRetiraParalisacao(int ano, int periodo, string curso, string usuarioId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (periodo < 0)
            {
                mensagens.Add("Campo PERIODO é obrigatório.");
            }

            if (curso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CURSO é obrigatório.");
            }

            if (usuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se o curso tem alguma linha marcada como Participamatricula Facil
                    if (!this.ExisteMatriculaFacilPor(contexto, ano, periodo, curso))
                    {
                        mensagens.Add("Não existe nenhuma opção marcada como participa matricula fácil para este curso.");
                    }
                    else
                    {
                        //Verifica se todos já estao NÃO paralisados
                        if (!this.ExistePor(contexto, ano, periodo, curso, true))
                        {
                            mensagens.Add("Não existe nenhuma opção marcada como paralisada para este curso.");
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

        public int RetiraParalisa(int ano, int periodo, string curso, string usuarioId)
        {
            int retorno = 0;

            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE TCE_CONTROLE_VAGA
	                                        SET PARALISAMATRICULAFACIL = 0,
		                                        MATRICULA = @MATRICULA,
		                                        DT_ALTERACAO = @DT_ALTERACAO
                                        WHERE ANO = @ANO
										   AND PERIODO = @PERIODO 
										   AND CURSO = @CURSO";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
                contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, usuarioId);
                contextQuery.Parameters.Add("@DT_ALTERACAO", SqlDbType.DateTime, DateTime.Now);

                retorno = contexto.ApplyModifications(contextQuery);
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

            return retorno;
        }

        public int Paralisa(int controleVagaId, bool paralisa, int motivoRetiradaFila, string usuarioId)
        {
            int retorno = 0;
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.Matriculas.OpcaoInscricao rnOpcaoInscricao = new Techne.Lyceum.RN.Matriculas.OpcaoInscricao();
            RN.Matriculas.OpcaoInscricaoHist rnOpcaoInscricaoHist = new Techne.Lyceum.RN.Matriculas.OpcaoInscricaoHist();

            try
            {
                if (paralisa)
                {
                    //Joga fila para historico
                    rnOpcaoInscricaoHist.InsereRetiradaFila(contexto, controleVagaId, motivoRetiradaFila);

                    //Deleta fila
                    rnOpcaoInscricao.RemoveRetiradaFila(contexto, controleVagaId);
                }

                //Paralisa curso
                this.AtualizaParalisa(contexto, controleVagaId, paralisa, motivoRetiradaFila, usuarioId);
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

            return retorno;
        }

        public void AtualizaParalisa(DataContext contexto, int controleVagaId, bool paralisa, int motivoRetiradaFila, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE TCE_CONTROLE_VAGA
	                                        SET PARALISAMATRICULAFACIL = @PARALISAMATRICULAFACIL,
		                                        MATRICULA = @MATRICULA,
		                                        DT_ALTERACAO = @DT_ALTERACAO
                                        WHERE ID_CONTROLE_VAGA = @ID_CONTROLE_VAGA ";

            contextQuery.Parameters.Add("@ID_CONTROLE_VAGA", SqlDbType.Int, controleVagaId);
            contextQuery.Parameters.Add("@PARALISAMATRICULAFACIL", SqlDbType.Bit, paralisa);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DT_ALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }
    }
}