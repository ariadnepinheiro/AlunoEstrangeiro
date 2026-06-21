using System;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.RN
{
    using Seeduc.Infra.Data;
    using Techne.Data;
    using System.Collections.Generic;
    using System.Data;

    public class Falta : RNBase
    {
        internal static ContextQuery Atualizar(int faltas, string aluno, string disciplina, string turma, int ano, int periodo, string frequencia)
        {
            var contextQuery = new ContextQuery
                               {
                                   Command = @"UPDATE  LY_FALTA
                                               SET     FALTAS = @FALTAS
                                               WHERE   ALUNO = @ALUNO
                                                       AND DISCIPLINA = @DISCIPLINA
                                                       AND TURMA = @TURMA
                                                       AND ANO = ANO
                                                       AND PERIODO = @PERIODO
                                                       AND FREQ = @FREQ"
                               };

            contextQuery.Parameters.Add("@FALTAS", TechneDbType.T_DECIMAL_MEDIO_PRECISO, faltas);
            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", TechneDbType.T_CODIGO, disciplina);
            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
            contextQuery.Parameters.Add("@PERIODO", TechneDbType.T_SEMESTRE2, periodo);
            contextQuery.Parameters.Add("@FREQ", TechneDbType.T_FALTA, frequencia);

            return contextQuery;
        }

        internal static ContextQuery Inserir(int faltas, string aluno, string disciplina, string turma, int ano, int periodo, string frequencia)
        {
            var contextQuery = new ContextQuery
                               {
                                   Command = @"
                                                    SELECT @FALTAATIVA = Count(*) 
                                                    FROM   LY_FALTA  
                                                    WHERE  ALUNO = @ALUNO 
                                                    AND    ANO = @ANO 
                                                    AND    PERIODO = @PERIODO 
                                                    AND    DISCIPLINA = @DISCIPLINA 
                                                    AND    TURMA = @TURMA 
                                                    AND    FREQ = @FREQ
                                                    
                                                    IF ( @FALTAATIVA = 0 ) 
                                                    BEGIN 
                                                      INSERT INTO LY_FALTA 
                                                                  ( 
                                                                              ALUNO, 
                                                                              DISCIPLINA, 
                                                                              TURMA, 
                                                                              ANO, 
                                                                              PERIODO, 
                                                                              FREQ, 
                                                                              FALTAS 
                                                                  ) 
                                                                  VALUES 
                                                                  ( 
                                                                              @ALUNO, 
                                                                              @DISCIPLINA, 
                                                                              @TURMA, 
                                                                              @ANO, 
                                                                              @PERIODO, 
                                                                              @FREQ, 
                                                                              @FALTAS 
                                                                  ) 
                                                    END 
                                                    ELSE 
                                                    BEGIN 
                                                      UPDATE LY_FALTA 
                                                      SET    FALTAS = @FALTAS                                                              
                                                      WHERE  ALUNO = @ALUNO 
                                                      AND    DISCIPLINA = @DISCIPLINA 
                                                      AND    TURMA = @TURMA 
                                                      AND    ANO = @ANO 
                                                      AND    PERIODO = @PERIODO 
                                                      AND    FREQ = @FREQ 
                                                    END"
                               };

            contextQuery.Parameters.Add("@FALTAATIVA", SqlDbType.Int, 0);
            contextQuery.Parameters.Add("@FALTAS", TechneDbType.T_DECIMAL_MEDIO_PRECISO, faltas);
            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", TechneDbType.T_CODIGO, disciplina);
            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
            contextQuery.Parameters.Add("@PERIODO", TechneDbType.T_SEMESTRE2, periodo);
            contextQuery.Parameters.Add("@FREQ", TechneDbType.T_FALTA, frequencia);

            return contextQuery;
        }

        internal void Insere(LyFalta lyFalta, List<ContextQuery> listaContextQuery)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"

                            SELECT @FALTAATIVA = COUNT(*) 
                            FROM   LY_FALTA M 
                            WHERE  ALUNO = @ALUNO 
                                    AND ANO = @ANO 
                                    AND PERIODO = @PERIODO 
                                    AND DISCIPLINA = @DISCIPLINA 
                                    AND TURMA = @TURMA 
		                            AND FREQ = @FREQ

                            IF ( @FALTAATIVA = 0 ) 
                                BEGIN 
                                    INSERT INTO LY_FALTA 
                                     ( 
                                                ALUNO,
                                                DISCIPLINA,
                                                TURMA,
                                                ANO,
                                                PERIODO,
                                                FREQ, 
                                                FALTAS, 
                                                FALTAS_COMPENSADAS
                                     ) VALUES ( 
                                                @ALUNO, 
                                                @DISCIPLINA, 
                                                @TURMA, 
                                                @ANO, 
                                                @PERIODO, 
                                                @FREQ, 
                                                @FALTAS, 
                                                @FALTAS_COMP
                                     )
                                END 
                            ELSE 
                                BEGIN 
                                  UPDATE LY_FALTA 
	                                SET    FALTAS = @FALTAS, 
                                    FALTAS_COMPENSADAS = @FALTAS_COMP 
                                WHERE  ALUNO = @ALUNO 
                                    AND DISCIPLINA = @DISCIPLINA 
                                    AND TURMA = @TURMA 
                                    AND ANO = @ANO 
                                    AND PERIODO = @PERIODO 
                                    AND FREQ = @FREQ
                                END ";

            contextQuery.Parameters.Add("@FALTAATIVA", SqlDbType.Int, 0);
            contextQuery.Parameters.Add("@ALUNO", lyFalta.Aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", lyFalta.Disciplina);
            contextQuery.Parameters.Add("@TURMA", lyFalta.Turma);
            contextQuery.Parameters.Add("@ANO", lyFalta.Ano);
            contextQuery.Parameters.Add("@PERIODO", lyFalta.Periodo);
            contextQuery.Parameters.Add("@FREQ", lyFalta.Freq);
            contextQuery.Parameters.Add("@FALTAS", lyFalta.Faltas);
            contextQuery.Parameters.Add("@FALTAS_COMP", lyFalta.FaltasCompensadas);

            listaContextQuery.Add(contextQuery);
        }
        
        internal static bool ExisteFaltas(TConnectionWritable connection, string aluno, string disciplinaDestino, string turmaDestinoNota, decimal ano, decimal semestre, string freq)
        {
            QueryTable qt = Consultar(connection, "SELECT  1 FROM dbo.LY_FALTA WHERE aluno = ? AND disciplina = ? AND turma = ? AND ano = ? AND periodo = ? AND FREQ = ?",
                aluno, disciplinaDestino, turmaDestinoNota, ano, semestre, freq);
            return qt.Rows.Count > 0;
        }

        public bool PossuiInconsistenciaFaltaAulasDadasPor(DataContext contexto, LyMatricula matriculaOrigem, string turmaDestino)
        {
            //Verifica se o aluno possui em qualquer semestre numero de falta maior que o numero de aulas dadas da turma de destino
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM   LY_FALTA F 
                                           INNER JOIN LY_FREQ FR 
                                                   ON F.DISCIPLINA = FR.DISCIPLINA 
                                                      AND FR.TURMA = @TURMADESTINO 
                                                      AND F.ANO = FR.ANO 
                                                      AND F.PERIODO = FR.PERIODO 
                                                      AND F.FREQ = FR.FREQ 
                                    WHERE  ALUNO = @ALUNO 
                                           AND F.DISCIPLINA = @DISCIPLINA 
                                           AND F.TURMA = @TURMAORIGEM 
                                           AND F.ANO = @ANO 
                                           AND F.PERIODO = @PERIODO 
                                           AND F.FALTAS > FR.AULAS_DADAS  ";

            contextQuery.Parameters.Add("@TURMADESTINO", SqlDbType.VarChar, turmaDestino);
            contextQuery.Parameters.Add("@ALUNO ", SqlDbType.VarChar, matriculaOrigem.Aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, matriculaOrigem.Disciplina);
            contextQuery.Parameters.Add("@TURMAORIGEM", SqlDbType.VarChar, matriculaOrigem.Turma);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, matriculaOrigem.Ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, matriculaOrigem.Semestre);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public static void MigrarFaltas(DataContext ctx, LyMatricula matricula, string turmaDestino)
        {
            //verificar se a disciplina tinha faltas
            var contextQuery = new ContextQuery(
                        @" SELECT  COUNT(*) AS TOTAL
                            FROM    LY_FALTA F
									INNER JOIN LY_TURMA t ON F.ANO = t.ANO
															AND F.PERIODO = t.SEMESTRE
															AND F.TURMA = t.TURMA
															AND F.DISCIPLINA = t.DISCIPLINA
                            WHERE   F.ALUNO = @ALUNO
                                    AND F.DISCIPLINA = @DISCIPLINA
                                    AND F.ANO = @ANO
                                    AND F.PERIODO = @SEMESTRE
									AND ISNULL(T.ELETIVA, 'N') = 'N'
                                    AND EXISTS ( SELECT 1
                                                 FROM   LY_FREQ FR
                                                 WHERE  F.DISCIPLINA = FR.DISCIPLINA
                                                        AND FR.TURMA = @TURMA
                                                        AND F.ANO = FR.ANO
                                                        AND F.PERIODO = FR.PERIODO
                                                        AND F.FREQ = FR.FREQ) ");

            contextQuery.Parameters.Add("@ALUNO", matricula.Aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", matricula.Disciplina);
            contextQuery.Parameters.Add("@TURMA", matricula.Turma);
            contextQuery.Parameters.Add("@ANO", matricula.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", matricula.Semestre);

            var faltas = ctx.GetReturnValue<int>(contextQuery);

            if (faltas > 0)
            {
                //se existir cadastra as novas para a turma nova
                ctx.ApplyModifications(
                new ContextQuery(
                            @" INSERT  INTO dbo.LY_FALTA
                        ( ALUNO ,
                          DISCIPLINA ,
                          TURMA ,
                          ANO ,
                          PERIODO ,
                          FREQ ,
                          FALTAS ,
                          FALTAS_COMPENSADAS 
                        )
                        ( SELECT    f.ALUNO ,
                                    F.DISCIPLINA ,
                                    @TURMADESTINO AS TURMA ,
                                    F.ANO ,
                                    F.PERIODO ,
                                    F.FREQ ,
                                    F.FALTAS ,
                                    F.FALTAS_COMPENSADAS
                          FROM      LY_FALTA F
									INNER JOIN LY_TURMA t ON F.ANO = t.ANO
															AND F.PERIODO = t.SEMESTRE
															AND F.TURMA = t.TURMA
															AND F.DISCIPLINA = t.DISCIPLINA
                          WHERE     F.ALUNO = @ALUNO
                                    AND F.DISCIPLINA = @DISCIPLINA
                                    AND F.ANO = @ANO
                                    AND F.PERIODO = @SEMESTRE
                                    AND f.TURMA = @TURMA
									AND ISNULL(T.ELETIVA, 'N') = 'N'
                                    AND EXISTS ( SELECT 1
                                                 FROM   LY_FREQ FR
                                                 WHERE  F.DISCIPLINA = FR.DISCIPLINA
                                                        AND FR.TURMA = @TURMADESTINO
                                                        AND F.ANO = FR.ANO
                                                        AND F.PERIODO = FR.PERIODO
                                                        AND F.FREQ = FR.FREQ )
                                    AND NOT EXISTS ( SELECT 1
                                                 FROM   LY_FALTA F2
                                                 WHERE  F.DISCIPLINA = F2.DISCIPLINA
                                                        AND F2.TURMA = @TURMADESTINO
                                                        AND F.ANO = F2.ANO
                                                        AND F.PERIODO = F2.PERIODO
                                                        AND F.FREQ = F2.FREQ 
                                                        AND F.ALUNO = F2.ALUNO )
                                                ) ",
               new ContextQueryParameter("@ALUNO", matricula.Aluno),
               new ContextQueryParameter("@DISCIPLINA", matricula.Disciplina),
               new ContextQueryParameter("@TURMA", matricula.Turma),
               new ContextQueryParameter("@TURMADESTINO", turmaDestino),
               new ContextQueryParameter("@ANO", matricula.Ano),
               new ContextQueryParameter("@SEMESTRE", matricula.Semestre)));

                //deleta faltas da turma antiga
                ctx.ApplyModifications(
                new ContextQuery(
                @" DELETE  LY_FALTA
                    WHERE   LY_FALTA.ALUNO = @ALUNO
                            AND LY_FALTA.DISCIPLINA = @DISCIPLINA
                            AND LY_FALTA.ANO = @ANO
                            AND LY_FALTA.PERIODO = @SEMESTRE
                            AND dbo.LY_FALTA.TURMA = @TURMA
                            --AND EXISTS ( SELECT 1
                            --             FROM   LY_FREQ FR
                            --             WHERE  LY_FALTA.DISCIPLINA = FR.DISCIPLINA
                            --                    AND FR.TURMA = @TURMADESTINO
                            --                    AND LY_FALTA.ANO = FR.ANO
                            --                    AND LY_FALTA.PERIODO = FR.PERIODO
                            --                    AND LY_FALTA.FREQ = FR.FREQ )  ",
               new ContextQueryParameter("@ALUNO", matricula.Aluno),
               new ContextQueryParameter("@DISCIPLINA", matricula.Disciplina),
               new ContextQueryParameter("@TURMA", matricula.Turma),
               new ContextQueryParameter("@ANO", matricula.Ano),
               new ContextQueryParameter("@TURMADESTINO", turmaDestino),
               new ContextQueryParameter("@SEMESTRE", matricula.Semestre)));
            }
        }

        public static int RetornaBimestesLançados(string aluno, decimal ano, decimal semestre, string turma, string disciplina)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {

                var contextQuery = new ContextQuery(
                    @" SELECT  COUNT(FA.FREQ) AS BIMESTRES
                        FROM    LY_FALTA FA ( NOLOCK )
                        INNER JOIN LY_FREQ FR ( NOLOCK ) ON FA.ANO = FR.ANO
                                                            AND FA.DISCIPLINA = FR.DISCIPLINA
                                                            AND FA.PERIODO = FR.PERIODO
                                                            AND FA.TURMA = FR.TURMA
                                                            AND FA.FREQ = FR.FREQ
                        WHERE   FA.ALUNO = @ALUNO
                                AND FA.DISCIPLINA = @DISCIPLINA
                                AND FA.TURMA = @TURMA
                                AND FA.ANO = @ANO
                                AND FA.PERIODO = @SEMESTRE
                                AND FA.FALTAS IS NOT NULL
                                AND FR.AULAS_DADAS IS NOT NULL
                                AND FR.AULAS_PREVISTAS IS NOT NULL ");

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);

                return ctx.GetReturnValue<int>(contextQuery);
            }
        }

        public static DadosFaltasAulas CalculaFaltasAulas(string aluno, decimal ano, decimal semestre, string turma, string disciplina)
        {
            var faltasAulas = new DadosFaltasAulas();
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {

                var contextQuery = new ContextQuery(
                    @" SELECT  SUM(FALTAS) AS FALTAS, SUM(AULAS_DADAS) AS AULAS_DADAS,
                                SUM(AULAS_PREVISTAS) AS AULAS_PREVISTAS
                        FROM    LY_FALTA FA ( NOLOCK )
                                INNER JOIN LY_FREQ FR ( NOLOCK ) ON FA.ANO = FR.ANO
                                                                    AND FA.DISCIPLINA = FR.DISCIPLINA
                                                                    AND FA.PERIODO = FR.PERIODO
                                                                    AND FA.TURMA = FR.TURMA
                                                                    AND FA.FREQ = FR.FREQ
                        WHERE   FA.ALUNO = @ALUNO
                                AND FA.DISCIPLINA = @DISCIPLINA
                                AND FA.TURMA = @TURMA
                                AND FA.ANO = @ANO
                                AND FA.PERIODO = @SEMESTRE
                                AND FA.FALTAS IS NOT NULL
                                AND FR.AULAS_DADAS IS NOT NULL
                                AND FR.AULAS_PREVISTAS IS NOT NULL ");

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    while (reader.Read())
                    {
                        faltasAulas.FaltasFinal = Convert.ToInt32(reader["FALTAS"]);
                        faltasAulas.AulasDadas = Convert.ToDecimal(reader["AULAS_DADAS"]);
                        faltasAulas.AulasPrevistas = Convert.ToDecimal(reader["AULAS_PREVISTAS"]);
                    }
                }
            }
            return faltasAulas;
        }

        public void RemovePorMatriculaParaFechamento(DataContext ctx, string aluno, string disciplina, string turma, decimal ano, decimal semestre)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" DELETE  LY_FALTA
                    WHERE   ALUNO = @ALUNO
                            AND DISCIPLINA = @DISCIPLINA
                            AND TURMA = @TURMA
                            AND ANO = @ANO
                            AND PERIODO = @PERIODO ";

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", semestre);

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

        public bool PossuiFaltaPor(string disciplina)
        {	
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                            FROM    LY_FALTA NOLOCK
                            WHERE   DISCIPLINA = @DISCIPLINA ";

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);

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
       
        public bool PossuiFaltaNaoZeradaPor(DataContext ctx, string aluno, decimal ano, decimal periodo, string turma)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM   LY_FALTA (nolock)
                                WHERE  ALUNO = @ALUNO 
                                       AND TURMA = @TURMA 
                                       AND ANO = @ANO 
                                       AND PERIODO = @SEMESTRE 
                                       AND FALTAS <> 0  ";

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", periodo);
            contextQuery.Parameters.Add("@TURMA", turma);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void RemoveFaltaZerada(DataContext ctx, string aluno, decimal ano, decimal periodo, string turma)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE LY_FALTA 
                                WHERE ALUNO = @ALUNO
	                                AND TURMA = @TURMA
	                                AND ANO =  @ANO
	                                AND PERIODO = @PERIODO
	                                AND FALTAS = 0 ";

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
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

        public bool PossuiFaltaEmPeridosPossiveisPor(string aluno, int ano, string periodosPossiveis)
        {	
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;           

            try
            {
                contextQuery.Command = string.Format(@" SELECT  COUNT(*)
                        FROM    DBO.LY_MATRICULA M
                                INNER JOIN DBO.LY_FALTA f ON M.ALUNO = f.ALUNO
                                                             AND M.DISCIPLINA = f.DISCIPLINA
                                                             AND M.TURMA = f.TURMA
                                                             AND M.ANO = f.ANO
                                                             AND M.SEMESTRE = f.PERIODO
                        WHERE   M.SIT_MATRICULA = @SIT_MATRICULA
                                AND M.ALUNO = @ALUNO
                                AND M.ANO = @ANO
                                AND M.SEMESTRE IN ( {0} ) ", periodosPossiveis);

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@SIT_MATRICULA", Matricula.Matriculado);

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
    
        internal List<FrequenciaAlunoTurma> ObtemFrequenciaAlunoTurmaAtualTransferencia(DataContext contexto, string aluno, TceTransferenciaOrigem transferenciaOrigem, TceTransferenciaDestino transferenciaDestino)
        {
            DataTable dataTable = new DataTable();
            List<FrequenciaAlunoTurma> listaFrequenciaAlunoTurma = new List<FrequenciaAlunoTurma>();

            dataTable = contexto.GetDataTable(
                new ContextQuery(
                    @"SELECT  F.DISCIPLINA,
                              F.FREQ,
                              CAST(ISNULL(F.FALTAS, 0) AS INT) AS FALTAS,
                              CAST(ISNULL(F.FALTAS_COMPENSADAS, 0) AS INT) AS FALTASCOMPENSADAS,
                              CASE WHEN FD.ALUNO IS NULL THEN 0 ELSE 1 END AS ATUALIZAFALTA
                      FROM    LY_FALTA F
                              LEFT JOIN LY_FALTA FD ON FD.ALUNO = F.ALUNO 
                                                   AND FD.DISCIPLINA = F.DISCIPLINA 
                                                   AND FD.TURMA = @TURMA_DESTINO 
                                                   AND FD.ANO = @ANO_DESTINO 
                                                   AND FD.PERIODO = @PERIODO_DESTINO 
                                                   AND FD.FREQ = F.FREQ

                              INNER JOIN LY_FREQ FR ON FR.DISCIPLINA = F.DISCIPLINA
                                                   AND FR.FREQ       = F.FREQ
                                                   AND FR.TURMA      = @TURMA_DESTINO
                                                   AND FR.ANO        = @ANO_DESTINO
                                                   AND FR.PERIODO    = @PERIODO_DESTINO

                              INNER JOIN ( SELECT  DISTINCT G.DISCIPLINA
                                           FROM    LY_GRADE G
                                           WHERE   G.CURSO           = @CURSO_DESTINO
                                                   AND G.TURNO       = @TURNO_DESTINO
                                                   AND G.CURRICULO   = @CURRICULO_DESTINO
                                                   AND G.SERIE_IDEAL = @SERIE_DESTINO
                              ) G ON G.DISCIPLINA  = F.DISCIPLINA

                      WHERE   F.ALUNO = @ALUNO
                              AND F.TURMA = @TURMA_ORIGEM
                              AND F.ANO = @ANO_ORIGEM
                              AND F.PERIODO = @PERIODO_ORIGEM
                              AND CAST(ISNULL(F.FALTAS, 0) AS INT) <= CAST(ISNULL(FR.AULAS_DADAS, 0) AS INT)

                      ORDER BY F.DISCIPLINA,
                               F.FREQ",
                new ContextQueryParameter("@ALUNO", aluno),
                new ContextQueryParameter("@TURMA_ORIGEM", transferenciaOrigem.Turma),
                new ContextQueryParameter("@ANO_ORIGEM", transferenciaOrigem.Ano),
                new ContextQueryParameter("@PERIODO_ORIGEM", transferenciaOrigem.Periodo),
                new ContextQueryParameter("@TURMA_DESTINO", transferenciaDestino.Turma),
                new ContextQueryParameter("@ANO_DESTINO", transferenciaDestino.Ano),
                new ContextQueryParameter("@PERIODO_DESTINO", transferenciaDestino.Periodo),
                new ContextQueryParameter("@CURSO_DESTINO", transferenciaDestino.Curso),
                new ContextQueryParameter("@TURNO_DESTINO", transferenciaDestino.Turno),
                new ContextQueryParameter("@CURRICULO_DESTINO", transferenciaDestino.Curriculo),
                new ContextQueryParameter("@SERIE_DESTINO", transferenciaDestino.Serie)

            ));

            foreach (DataRow dataRow in dataTable.Rows)
            {
                RN.DTOs.FrequenciaAlunoTurma frequenciaAlunoTurma = new RN.DTOs.FrequenciaAlunoTurma();
                frequenciaAlunoTurma.Disciplina = dataRow["DISCIPLINA"].ToString(); ;
                frequenciaAlunoTurma.Freq = dataRow["FREQ"].ToString();
                frequenciaAlunoTurma.Faltas = Convert.ToInt32(dataRow["FALTAS"]);
                frequenciaAlunoTurma.FaltasCompensadas = Convert.ToInt32(dataRow["FALTASCOMPENSADAS"]);
                frequenciaAlunoTurma.AtualizaFalta = Convert.ToBoolean(dataRow["ATUALIZAFALTA"]);

                listaFrequenciaAlunoTurma.Add(frequenciaAlunoTurma);
            }

            return listaFrequenciaAlunoTurma;
        }

        internal void AtualizaQuantidadeDeFaltas(LyFalta lyFalta, List<ContextQuery> listaContextQuery)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"UPDATE LY_FALTA 
                                     SET    FALTAS = @FALTAS, 
                                            FALTAS_COMPENSADAS = @FALTAS_COMP 
                                     WHERE  ALUNO = @ALUNO 
                                            AND DISCIPLINA = @DISCIPLINA 
                                            AND TURMA = @TURMA 
                                            AND ANO = @ANO 
                                            AND PERIODO = @PERIODO 
                                            AND FREQ = @FREQ";

            contextQuery.Parameters.Add("@FALTAS", lyFalta.Faltas);
            contextQuery.Parameters.Add("@FALTAS_COMP", lyFalta.FaltasCompensadas);
            contextQuery.Parameters.Add("@ALUNO", lyFalta.Aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", lyFalta.Disciplina);
            contextQuery.Parameters.Add("@TURMA", lyFalta.Turma);
            contextQuery.Parameters.Add("@ANO", lyFalta.Ano);
            contextQuery.Parameters.Add("@PERIODO", lyFalta.Periodo);
            contextQuery.Parameters.Add("@FREQ", lyFalta.Freq);

            listaContextQuery.Add(contextQuery);
        }

        internal void RemovePor(string aluno, string disciplina, string turma, int? ano, int? periodo, string freq, List<ContextQuery> listaContextQuery)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"DELETE LY_FALTA 
                                     WHERE  ALUNO = @ALUNO 
                                            AND DISCIPLINA = @DISCIPLINA 
                                            AND TURMA = @TURMA 
                                            AND ANO = @ANO 
                                            AND PERIODO = @PERIODO 
                                            AND FREQ = @FREQ";

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@FREQ", freq);

            listaContextQuery.Add(contextQuery);
        }

        public List<string> ChecaFaltasNaoMigradas(DataContext ctx, string aluno, string turmaAtual, string turmaNova, string disciplina)
        {
            //Verifica se alguma falta não será migrada para turma destino por não existir freq gerada
            List<string> listaAvisos = new List<string>();
            DataTable faltaSemFreqDestino = ObtemFaltaSemPFreqDestinoPor(ctx, aluno, turmaAtual, turmaNova, disciplina);

            foreach (DataRow dr in faltaSemFreqDestino.Rows)
            {
                if (listaAvisos.Count == 0)
                {
                    if (dr["faltas_compensadas"] != DBNull.Value)
                    {
                        listaAvisos.Add("<br>Faltas não migradas: Disciplina: " + Convert.ToString(dr["disciplina"]) + " Turma: " + Convert.ToString(dr["turma"]) + " Ano: " + Convert.ToString(dr["ano"]) + " Semestre: " + Convert.ToString(dr["periodo"]) + " - " + Convert.ToString(dr["freq"]) + " = " + (Convert.ToInt32(dr["faltas"]) - Convert.ToInt32(dr["faltas_compensadas"])));
                    }
                    else
                    {
                        listaAvisos.Add("<br>Faltas não migradas: Disciplina: " + Convert.ToString(dr["disciplina"]) + " Turma: " + Convert.ToString(dr["turma"]) + " Ano: " + Convert.ToString(dr["ano"]) + " Semestre: " + Convert.ToString(dr["periodo"]) + " - " + Convert.ToString(dr["freq"]) + " = " + (Convert.ToInt32(dr["faltas"])));
                    }
                }
                else
                {
                    if (dr["faltas_compensadas"] != DBNull.Value)
                    {
                        listaAvisos.Add("<br>Disciplina: " + Convert.ToString(dr["disciplina"]) + " Turma: " + Convert.ToString(dr["turma"]) + " Ano: " + Convert.ToString(dr["ano"]) + " Semestre: " + Convert.ToString(dr["periodo"]) + " - " + Convert.ToString(dr["freq"]) + " = " + (Convert.ToInt32(dr["faltas"]) - Convert.ToInt32(dr["faltas_compensadas"])));
                    }
                    else
                    {
                        listaAvisos.Add("<br>Disciplina: " + Convert.ToString(dr["disciplina"]) + " Turma: " + Convert.ToString(dr["turma"]) + " Ano: " + Convert.ToString(dr["ano"]) + " Semestre: " + Convert.ToString(dr["periodo"]) + " - " + Convert.ToString(dr["freq"]) + " = " + (Convert.ToInt32(dr["faltas"])));
                    }
                }
            }


            return listaAvisos;
        }

        public DataTable ObtemFaltaSemPFreqDestinoPor(DataContext ctx, string aluno, string turmaAtual, string turmaNova, string disciplina)
        {
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            contextQuery.Command = @" SELECT F.DISCIPLINA, 
                                           F.TURMA, 
                                           F.ANO, 
                                           F.PERIODO, 
                                           FREQ, 
                                           FALTAS, 
                                           FALTAS_COMPENSADAS 
                                    FROM   LY_FALTA F 
                                    WHERE  F.ALUNO = @ALUNO 
                                           AND F.TURMA = @TURMA_ATUAL 
                                           AND F.DISCIPLINA = @DISCIPLINA 
                                           AND NOT EXISTS (SELECT 1 
                                                           FROM   LY_FREQ FR2 
                                                           WHERE  FR2.DISCIPLINA = F.DISCIPLINA 
                                                                  AND FR2.TURMA = @TURMA_NOVA 
                                                                  AND FR2.ANO = F.ANO 
                                                                  AND FR2.PERIODO = F.PERIODO 
                                                                  AND FR2.FREQ = F.FREQ)  ";

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@TURMA_ATUAL", turmaAtual);
            contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
            contextQuery.Parameters.Add("@TURMA_NOVA", turmaNova);

            dt = ctx.GetDataTable(contextQuery);

            return dt;
        }

    }
}