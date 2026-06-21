namespace Techne.Lyceum.RN
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Seeduc.Infra.Data;
    using Techne.Data;
    using Techne.Library;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Lyceum.RN.Util;
    using System.Data.SqlClient;
    using Techne.Lyceum.RN.DTOs;

    public class Dependencia : RNBase
    {
        public static DataTable BuscaTipoSala()
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT distinct CTD.LYTIPODEPENDENCIAID, TD.NOME
		                             FROM  LYCURSO_LYTIPODEPENDENCIA CTD
                                INNER JOIN LY_TIPO_DEPENDENCIA TD ON CTD.LYTIPODEPENDENCIAID = TD.TIPO_DEPEND"

                };
                return ctx.GetDataTable(contextQuery);
            }
        }

        public DataTable ConsultarAtiva(string faculdade, string turno, string turma, string tipo_depend, string ano, string semestre, string curso, string turmaReferencia, string sala)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT D.DEPENDENCIA, 
                                    (D.DEPENDENCIA + ISNULL(' - ' + D.DESCRICAO, '')) DESCRICAO, 
                                    D.TIPO_DEPEND 
                            INTO #DEPENDENCIA
                            FROM   LY_DEPENDENCIA D 
                            WHERE  D.FACULDADE = @FACULDADE 
                                    AND D.ATIVA = 'S' 
                                    AND NOT EXISTS (SELECT 1 
                                                    FROM   LY_TURMA TU 
                                                    WHERE  TU.FACULDADE = @FACULDADE
                                                            AND TU.OPTATIVAREFORCO = 'N' 
                                                            AND TU.TURNO = @TURNO 
                                                            AND TU.SIT_TURMA = 'Aberta'
                                                            AND TU.DEPENDENCIA = D.DEPENDENCIA 
								                            AND TU.ANO = @ANO
								                            and TU.SEMESTRE = @SEMESTRE )
                                    AND TIPO_DEPEND = @TIPO_DEPEND 

                            INSERT INTO #DEPENDENCIA
                            SELECT DISTINCT D.DEPENDENCIA, 
                                            (D.DEPENDENCIA + ISNULL(' - ' + D.DESCRICAO, '')) DESCRICAO, 
                                            D.TIPO_DEPEND 
                            FROM   LY_DEPENDENCIA D 
                            where DEPENDENCIA = @SALA
                                 AND D.FACULDADE = @FACULDADE 

                            INSERT INTO #DEPENDENCIA
                            SELECT DISTINCT D.DEPENDENCIA, 
                                            (D.DEPENDENCIA + ISNULL(' - ' + D.DESCRICAO, '')) DESCRICAO, 
                                            D.TIPO_DEPEND 
                            FROM   LY_TURMA TU 
                                    INNER JOIN LY_DEPENDENCIA D 
                                            ON TU.DEPENDENCIA = D.DEPENDENCIA 
                                                AND D.FACULDADE = TU.FACULDADE 
                            WHERE  @TURMAREFERENCIA IS NOT NULL
		                            AND TU.TURMA = @TURMAREFERENCIA
                                    AND TU.ANO = @ANO
                                    AND TU.SEMESTRE = @SEMESTRE 

                            INSERT INTO #DEPENDENCIA
                            SELECT DISTINCT D.DEPENDENCIA, 
                                            (D.DEPENDENCIA + ISNULL(' - ' + D.DESCRICAO, '')) DESCRICAO, 
                                            D.TIPO_DEPEND 
                            FROM   LY_DEPENDENCIA D 
                                    INNER JOIN LYCURSO_LYTIPODEPENDENCIA T 
                                            ON T.LYTIPODEPENDENCIAID = D.TIPO_DEPEND 
                            WHERE  T.LYCURSOID = @CURSO 
                            AND  D.FACULDADE = @FACULDADE  
                            AND D.ATIVA = 'S'                           

                            SELECT DISTINCT *
                            FROM #DEPENDENCIA
                            ORDER  BY DESCRICAO  ";

                contextQuery.Parameters.Add("@FACULDADE", faculdade);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@TIPO_DEPEND", tipo_depend);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@TURMAREFERENCIA", turmaReferencia);
                contextQuery.Parameters.Add("@SALA", sala);

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

        public DataTable ConsultaDependenciaAtivaPor(string faculdade, string turno, string turma, string ano, string semestre, string tipo_depend, string curso, string sala)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT D.DEPENDENCIA, 
                                            (D.DEPENDENCIA + ISNULL(' - ' + D.DESCRICAO, '')) DESCRICAO, 
                                            D.TIPO_DEPEND 
                                    INTO #DEPENDENCIA
                                    FROM   LY_DEPENDENCIA D 
                                    WHERE  D.FACULDADE = @FACULDADE 
                                            AND D.ATIVA = 'S' 
                                            AND TIPO_DEPEND = @TIPO_DEPEND 

                                    INSERT INTO #DEPENDENCIA
                                    SELECT DISTINCT D.DEPENDENCIA, 
                                                    (D.DEPENDENCIA + ISNULL(' - ' + D.DESCRICAO, '')) DESCRICAO, 
                                                    D.TIPO_DEPEND 
                                    FROM   LY_DEPENDENCIA D 
                                    where DEPENDENCIA = @SALA
                                         AND D.FACULDADE = @FACULDADE 
                                    	  
                                    INSERT INTO #DEPENDENCIA
                                    SELECT DISTINCT D.DEPENDENCIA, 
                                                    (D.DEPENDENCIA + ISNULL(' - ' + D.DESCRICAO, '')) DESCRICAO, 
                                                    D.TIPO_DEPEND 
                                    FROM   LY_DEPENDENCIA D 
                                            INNER JOIN LYCURSO_LYTIPODEPENDENCIA T 
                                                    ON T.LYTIPODEPENDENCIAID = D.TIPO_DEPEND 
                                    WHERE  T.LYCURSOID = @CURSO -- PEGAR O ID DO CURSO DA TELA TURMA
                                            AND D.ATIVA = 'S' 
                                            AND D.FACULDADE = @FACULDADE  

                                    SELECT DISTINCT *
                                    FROM #DEPENDENCIA
                                    ORDER  BY DESCRICAO   ";

                contextQuery.Parameters.Add("@FACULDADE", faculdade);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@TIPO_DEPEND", tipo_depend);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@SALA", sala);

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

        public static ErrorList VerificarDisponibilidadeDependencia(TConnection connection, decimal diaSemana, string turno, decimal ano, decimal semestre, DateTime horaIni, DateTime horaFim, DateTime dtIni, DateTime dtFim, string faculdade, string dependencia, string turma, decimal aula, decimal num_func, string disciplina)
        {
            //Para determinacao de sala ocupada, para a aula inserida
            RN.Docentes rnDocentes = new Docentes();

            if (dependencia != Constantes.DEPENDENCIA_INDETERMINADA)
            {
                string sql = @"select * from LY_AULA_DOCENTE ad
                        inner join LY_TURMA t on 
                            ad.DISCIPLINA = ad.DISCIPLINA and ad.TURMA = t.TURMA and 
                            ad.ANO = t.ANO and ad.SEMESTRE = t.SEMESTRE and ad.DATA_FIM = t.DT_FIM
                        inner join LY_HOR_AULA ha on 
                            ha.TURNO = ad.TURNO and ha.FACULDADE = ad.FACULDADE and ha.DIA_SEMANA = ad.DIA_SEMANA and 
                            ha.AULA = ad.AULA and ha.DISCIPLINA = ad.DISCIPLINA and ha.TURMA = ad.TURMA and     
                            ha.ANO = ad.ANO and ha.SEMESTRE = ad.SEMESTRE
                        where 
                            ad.TURMA <> ? and 
                            ad.ANO = ? and 
                            ad.SEMESTRE = ? and 
                            ad.TURNO = ? and 
                            ad.DIA_SEMANA = ? and 
                            ha.HORAFIM_AULA > ? and 
                            ha.HORAINI_AULA < ? and
                            ad.FACULDADE = ? and 
                            t.DEPENDENCIA = ? and 
                            t.SIT_TURMA <> 'Desativada' AND 
                            T.DT_INICIO <= ? AND
                            T.DT_FIM >= ?";

                QueryTable qt = null;
                qt = new QueryTable(sql);

                qt.Query(connection,
                    turma, ano, semestre, turno, diaSemana,
                    new DateTime(1899, 12, 30, horaIni.Hour, horaIni.Minute, horaIni.Second),
                    new DateTime(1899, 12, 30, horaFim.Hour, horaFim.Minute, horaFim.Second),
                    faculdade, dependencia,
                    new DateTime(dtFim.Year, dtFim.Month, dtFim.Day, 0, 0, 0),
                    new DateTime(dtIni.Year, dtIni.Month, dtIni.Day, 0, 0, 0));

                if (qt != null)
                {
                    if (qt.Rows.Count > 0)
                    {
                        string matricula = rnDocentes.ObtemMatriculaPor(num_func);
                        string nomeDocente = rnDocentes.ObtemNomeDocentePorNumFunc(num_func);
                        string nomeDisciplina = RN.Disciplina.ObterNomeDisciplina(connection, disciplina);

                        string mensagem = aula + "|" + diaSemana + "|A Sala desta aula está previamente alocada neste horário para: " +
                                          " Disciplina : " + Convert.ToString(qt.Rows[0]["DISCIPLINA"]) +
                                          " Turma : " + Convert.ToString(qt.Rows[0]["TURMA"]) +
                                          " Ano: " + Convert.ToString(qt.Rows[0]["ANO"]) +
                                          " Semestre: " + Convert.ToString(qt.Rows[0]["SEMESTRE"]) +
                                          "|" + String.Format("{0:HH:mm}", horaIni) + "|" + String.Format("{0:HH:mm}", horaFim) + "|" + matricula + " - " + nomeDocente + "|" + nomeDisciplina;

                        ErrorList erro = new ErrorList();
                        erro.Add(mensagem, "ERRO_VALIDACAO");
                        return erro;
                    }
                }
            }

            return null;
        }

        public static DataTable ListarSalaDeAula(string censo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" Select FACULDADE, DEPENDENCIA, TIPO_DEPEND, 
                        ATIVA, SALA_ANEXA, NUM_ALUNOS, AREA, CAD_SALA_AULA 
                        from LY_DEPENDENCIA
                        where CAD_SALA_AULA = 'S'
                        AND ATIVA = 'S'
                        and FACULDADE = @CENSO
                        order by DEPENDENCIA "

                };
                contextQuery.Parameters.Add("@CENSO", censo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarSalaDeAula_InspEsc(string censo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"   SELECT DEPENDENCIA, 
                                   EDIFICACAO, 
                                   PAVIMENTO, 
                                   NULL SIM, 
                                   NULL NAO 
                            FROM   LY_DEPENDENCIA 
                            WHERE  CAD_SALA_AULA = 'S' 
                                   AND ATIVA = 'S' 
                                   AND TIPO_DEPEND = 'SALA' 
                                   AND FACULDADE = @CENSO 
                            ORDER  BY DEPENDENCIA "

                };
                contextQuery.Parameters.Add("@CENSO", censo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarBanheiro_InspEsc(string censo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"   SELECT DEPENDENCIA, 
                                   EDIFICACAO, 
                                   PAVIMENTO, 
                                   NULL SIM, 
                                   NULL NAO 
                            FROM   LY_DEPENDENCIA 
                            WHERE  CAD_SALA_AULA = 'S' 
                                   AND ATIVA = 'S' 
                                   AND TIPO_DEPEND LIKE 'BANHEIRO%'
                                   AND FACULDADE = @CENSO 
                            ORDER  BY DEPENDENCIA "

                };
                contextQuery.Parameters.Add("@CENSO", censo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarPorUnidadeFisica(string unidadeFisica, string ativa)
        {
            return Consultar(
                new ContextQuery(
                    @"SELECT  d.faculdade,
                            d.dependencia,
                            d.tipo_depend,
                            d.ativa,
                            d.sala_anexa,
                            d.num_alunos,
                            d.descricao,
                            d.obs,
                            d.edificacao,
                            d.pavimento,
                            d.area,
                            d.cad_sala_aula,
                            d.matricula,
                            d.dt_cadastro,
                            d.dt_alteracao,
                            ufe.nome_edificacao,
                            ufe.nome_pavimento
                    FROM    LY_DEPENDENCIA d
                            INNER JOIN dbo.LY_UNIDADE_FISICA uf ON d.FACULDADE = uf.UNIDADE_FIS
                            LEFT JOIN dbo.LY_UNIDADE_FISICA_EDIFICACAO ufe ON uf.UNIDADE_FIS = ufe.UNIDADE_FIS
                                                                              AND d.PAVIMENTO = ufe.PAVIMENTO
                                                                              AND d.EDIFICACAO = ufe.EDIFICACAO
                    WHERE   ATIVA = @ATIVA
                            AND uf.UNIDADE_FIS = @UNIDADE_FIS
                            AND d.TIPO_DEPEND = 'SALA'
                    ORDER BY DEPENDENCIA",
                    new ContextQueryParameter("@ATIVA", ativa == "True" ? "S" : "N"),
                    new ContextQueryParameter("@UNIDADE_FIS", unidadeFisica)));
        }

        public static DataTable ListarSalaRecursoPorUnidadeFisica(string unidadeFisica)
        {
            return Consultar(
                new ContextQuery(
                    @"SELECT  d.faculdade,
                            d.dependencia,
                            d.tipo_depend,
                            d.ativa,
                            d.sala_anexa,
                            d.num_alunos,
                            d.descricao,
                            d.obs,
                            d.edificacao,
                            d.pavimento,
                            d.area,
                            d.cad_sala_aula,
                            d.matricula,
                            d.dt_cadastro,
                            d.dt_alteracao,
                            ufe.nome_edificacao,
                            ufe.nome_pavimento
                    FROM    LY_DEPENDENCIA d
                            INNER JOIN dbo.LY_UNIDADE_FISICA uf ON d.FACULDADE = uf.UNIDADE_FIS
                            LEFT JOIN dbo.LY_UNIDADE_FISICA_EDIFICACAO ufe ON uf.UNIDADE_FIS = ufe.UNIDADE_FIS
                                                                              AND d.PAVIMENTO = ufe.PAVIMENTO
                                                                              AND d.EDIFICACAO = ufe.EDIFICACAO
                    WHERE   ATIVA = 'S'
                            AND uf.UNIDADE_FIS = @UNIDADE_FIS
                            AND d.TIPO_DEPEND = 'SALAAEE'
                    ORDER BY DEPENDENCIA",

                    new ContextQueryParameter("@UNIDADE_FIS", unidadeFisica)));
        }

        //        SELECT TIPO_DEPEND, NOME
        //  FROM dbo.LY_TIPO_DEPENDENCIA
        //WHERE TIPO_DEPEND LIKE 'BANHEIRO%';

        public static DataTable ListarBanheiroPorUnidadeFisica(string unidadeFisica)
        {

            return Consultar(
                new ContextQuery(
                   @"SELECT d.faculdade,
                            d.dependencia,
                            d.tipo_depend,
                            d.ativa,
                            d.descricao,                            
                            d.edificacao,
                            d.pavimento,                            
                            d.cad_sala_aula,
                            d.matricula,
                            d.dt_cadastro,
                            d.dt_alteracao,
                            ufe.nome_edificacao,
                            ufe.nome_pavimento
                    FROM    LY_DEPENDENCIA d
                            INNER JOIN dbo.LY_UNIDADE_FISICA uf ON d.FACULDADE = uf.UNIDADE_FIS
                            LEFT JOIN dbo.LY_UNIDADE_FISICA_EDIFICACAO ufe ON uf.UNIDADE_FIS = ufe.UNIDADE_FIS
                                                                              AND d.PAVIMENTO = ufe.PAVIMENTO
                                                                              AND d.EDIFICACAO = ufe.EDIFICACAO
                    WHERE  uf.UNIDADE_FIS = @UNIDADE_FIS
                           AND d.TIPO_DEPEND LIKE 'BANHEIRO%'
                    ORDER BY DEPENDENCIA",

                    new ContextQueryParameter("@UNIDADE_FIS", unidadeFisica)));

        }

        public string ObtemSalaAlternativaVaziaPor(DataContext contexto, string censo, string turno, int ano, int periodo)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;
            string possiveisPeriodos = Utils.RecuperaPossiveisPeriodosCompleto(periodo);

            contextQuery.Command = string.Format(@" SELECT TOP 1 DEPENDENCIA
		                            FROM LY_DEPENDENCIA D
                                    WHERE D.FACULDADE = @CENSO
                                    AND TIPO_DEPEND <> 'SALA'
		                            AND TIPO_DEPEND <> 'SALAAEE'
                                    AND TIPO_DEPEND NOT LIKE 'BANHEIRO%'
									AND D.DEPENDENCIA NOT IN ( SELECT DISTINCT DEPENDENCIA
                                                                FROM    DBO.LY_TURMA T
                                                                WHERE   T.TURNO = @TURNO
                                                                        AND T.ANO = @ANO
                                                                        AND T.SEMESTRE IN ( {0} )
                                                                        AND T.FACULDADE = D.FACULDADE )
								ORDER BY D.DEPENDENCIA ", possiveisPeriodos);

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, turno);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);        

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public static DataTable ListarSalasAlternativasPor(string faculdade)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT  FACULDADE,DEPENDENCIA, DESCRICAO, TIPO_DEPEND, NUM_ALUNOS, ATIVA,  SALA_ANEXA
		                            FROM LY_DEPENDENCIA 
                                    WHERE FACULDADE = @FACULDADE
                                    AND TIPO_DEPEND <> 'SALA'
		                            AND TIPO_DEPEND <> 'SALAAEE'
                                    AND TIPO_DEPEND NOT LIKE 'BANHEIRO%'"

                };
                contextQuery.Parameters.Add("@FACULDADE", faculdade);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static void InserirSalaDeAula(LyDependencia dependencia)
        {
            dependencia.Dependencia = GerarSalaAulaRec(dependencia.Faculdade);
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"  INSERT  INTO LY_DEPENDENCIA ( FACULDADE, DEPENDENCIA, TIPO_DEPEND, ATIVA,
                                                                  NUM_ALUNOS, AREA, CAD_SALA_AULA, DESCRICAO, OBS,
                                                                  EDIFICACAO, PAVIMENTO, SALA_ANEXA, 
                                                                  MATRICULA, DT_CADASTRO, DT_ALTERACAO )
                                    VALUES  ( @CENSO, @DEPENDENCIA, 'SALA', 'S', @NUM_ALUNOS, @AREA, 'S',
                                              @DESCRICAO, @OBS, @EDIFICACAO, @PAVIMENTO, @SALA_ANEXA,
                                              @MATRICULA, @DT_CADASTRO, @DT_ALTERACAO ) "
                    };

                    contextQuery.Parameters.Add("@CENSO", TechneDbType.T_CODIGO, dependencia.Faculdade);
                    contextQuery.Parameters.Add("@DEPENDENCIA", TechneDbType.T_CODIGO, dependencia.Dependencia);
                    contextQuery.Parameters.Add("@NUM_ALUNOS", TechneDbType.T_NUMERO_PEQUENO, dependencia.NumAlunos);
                    contextQuery.Parameters.Add("@AREA", TechneDbType.T_DECIMAL_MEDIO, dependencia.Area);
                    contextQuery.Parameters.Add("@DESCRICAO", dependencia.Descricao);
                    contextQuery.Parameters.Add("@OBS", dependencia.Obs);
                    contextQuery.Parameters.Add("@EDIFICACAO", dependencia.Edificacao);
                    contextQuery.Parameters.Add("@PAVIMENTO", dependencia.Pavimento);
                    contextQuery.Parameters.Add("@SALA_ANEXA", dependencia.SalaAnexa);
                    contextQuery.Parameters.Add("@MATRICULA", dependencia.Matricula);
                    contextQuery.Parameters.Add("@DT_CADASTRO", DateTime.Now);
                    contextQuery.Parameters.Add("@DT_ALTERACAO", DateTime.Now);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception e)
                {
                    ctx.Abandon();
                    throw e;
                }
            }
        }

        public static void InserirSalaAlternativa(DadosSalaAlternativa SalasAlter)
        {
            var ctx = DataContextBuilder.FromLyceum.UsingLock();
            try
            {
                var contextQuery = new ContextQuery(
                    @" INSERT  INTO dbo.LY_DEPENDENCIA
                            ( FACULDADE, DEPENDENCIA, DESCRICAO, TIPO_DEPEND, NUM_ALUNOS, ATIVA,  
                              SALA_ANEXA, MATRICULA, DT_CADASTRO, DT_ALTERACAO)
                    VALUES  ( @FACULDADE, @DEPENDENCIA, @DESCRICAO, @TIPO_DEPEND, @NUM_ALUNOS, @ATIVA,
                              @SALA_ANEXA, @MATRICULA, @DT_CADASTRO, @DT_ALTERACAO) ");

                contextQuery.Parameters.Add("@FACULDADE", SalasAlter.FACULDADE);
                contextQuery.Parameters.Add("@DEPENDENCIA", SalasAlter.DEPENDENCIA);
                contextQuery.Parameters.Add("@DESCRICAO", SalasAlter.DESCRICAO);
                contextQuery.Parameters.Add("@TIPO_DEPEND", SalasAlter.TIPO_DEPEND);
                contextQuery.Parameters.Add("@NUM_ALUNOS", SalasAlter.NUM_ALUNOS);
                contextQuery.Parameters.Add("@ATIVA", SalasAlter.ATIVA);
                contextQuery.Parameters.Add("@SALA_ANEXA", SalasAlter.SALA_ANEXA);
                contextQuery.Parameters.Add("@MATRICULA", SalasAlter.MATRICULA);
                contextQuery.Parameters.Add("@DT_CADASTRO", DateTime.Now);
                contextQuery.Parameters.Add("@DT_ALTERACAO", DateTime.Now);

                ExecutarAlteracao(contextQuery);
            }
            catch (Exception)
            {
                ctx.Abandon();
            }
        }

        public static void AlterarSalaAlternativa(DadosSalaAlternativa SalasAlter)
        {
            var ctx = DataContextBuilder.FromLyceum.UsingLock();
            try
            {
                var contextQuery = new ContextQuery(
                    @"UPDATE  dbo.LY_DEPENDENCIA
                    SET     DEPENDENCIA = @DEPENDENCIA,
                            DESCRICAO = @DESCRICAO,
                            TIPO_DEPEND = @TIPO_DEPEND,
                            NUM_ALUNOS = @NUM_ALUNOS,
                            ATIVA = @ATIVA,
                            SALA_ANEXA = @SALA_ANEXA,
                            MATRICULA = @MATRICULA,
                            DT_ALTERACAO = @DT_ALTERACAO
                    WHERE   FACULDADE = @FACULDADE
                            AND DEPENDENCIA = @DEPENDENCIA");

                contextQuery.Parameters.Add("@FACULDADE", SalasAlter.FACULDADE);
                contextQuery.Parameters.Add("@DEPENDENCIA", SalasAlter.DEPENDENCIA);
                contextQuery.Parameters.Add("@DESCRICAO", SalasAlter.DESCRICAO);
                contextQuery.Parameters.Add("@TIPO_DEPEND", SalasAlter.TIPO_DEPEND);
                contextQuery.Parameters.Add("@NUM_ALUNOS", SalasAlter.NUM_ALUNOS);
                contextQuery.Parameters.Add("@ATIVA", SalasAlter.ATIVA);
                contextQuery.Parameters.Add("@MATRICULA", SalasAlter.MATRICULA);
                contextQuery.Parameters.Add("@SALA_ANEXA", SalasAlter.SALA_ANEXA);
                contextQuery.Parameters.Add("@DT_ALTERACAO", DateTime.Now);

                ExecutarAlteracao(contextQuery);
            }
            catch (Exception)
            {
                ctx.Abandon();
            }
        }

        public static void InserirSalaRecurso(LyDependencia dependencia)
        {
            dependencia.Dependencia = GerarSalaRecurso(dependencia.Faculdade);
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"  INSERT  INTO LY_DEPENDENCIA ( FACULDADE, DEPENDENCIA, TIPO_DEPEND, ATIVA,
                                                                  NUM_ALUNOS, AREA, CAD_SALA_AULA, DESCRICAO, OBS,
                                                                  EDIFICACAO, PAVIMENTO, SALA_ANEXA, 
                                                                  MATRICULA, DT_CADASTRO, DT_ALTERACAO )
                                    VALUES  ( @CENSO, @DEPENDENCIA, 'SALAAEE', 'S', @NUM_ALUNOS, @AREA, 'S',
                                              @DESCRICAO, @OBS, @EDIFICACAO, @PAVIMENTO, @SALA_ANEXA, 
                                              @MATRICULA, @DT_CADASTRO, @DT_ALTERACAO ) "
                    };

                    contextQuery.Parameters.Add("@CENSO", TechneDbType.T_CODIGO, dependencia.Faculdade);
                    contextQuery.Parameters.Add("@DEPENDENCIA", TechneDbType.T_CODIGO, dependencia.Dependencia);
                    contextQuery.Parameters.Add("@NUM_ALUNOS", TechneDbType.T_NUMERO_PEQUENO, dependencia.NumAlunos);
                    contextQuery.Parameters.Add("@AREA", TechneDbType.T_DECIMAL_MEDIO, dependencia.Area);
                    contextQuery.Parameters.Add("@DESCRICAO", dependencia.Descricao);
                    contextQuery.Parameters.Add("@OBS", dependencia.Obs);
                    contextQuery.Parameters.Add("@EDIFICACAO", dependencia.Edificacao);
                    contextQuery.Parameters.Add("@PAVIMENTO", dependencia.Pavimento);
                    contextQuery.Parameters.Add("@SALA_ANEXA", dependencia.SalaAnexa);
                    contextQuery.Parameters.Add("@MATRICULA", dependencia.Matricula);
                    contextQuery.Parameters.Add("@DT_CADASTRO", DateTime.Now);
                    contextQuery.Parameters.Add("@DT_ALTERACAO", DateTime.Now);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception e)
                {
                    ctx.Abandon();
                    throw e;
                }
            }
        }

        public static void InserirBanheiroeVestiario(LyDependencia dependencia)
        {
            dependencia.Dependencia = GerarBanheiro(dependencia.Faculdade);


            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"  INSERT  INTO LY_DEPENDENCIA ( FACULDADE, DEPENDENCIA, TIPO_DEPEND,
                                      ATIVA, DESCRICAO,EDIFICACAO, PAVIMENTO,MATRICULA,DT_CADASTRO, DT_ALTERACAO)

                                        VALUES  (@CENSO, @DEPENDENCIA, @TIPODEPENDENCIA,
                                                @ATIVA,@DESCRICAO, 
                                                @EDIFICACAO, @PAVIMENTO,  
                                                @MATRICULA, @DT_CADASTRO,@DT_ALTERACAO ) "
                    };

                    contextQuery.Parameters.Add("@TIPODEPENDENCIA", TechneDbType.T_CODIGO, dependencia.TipoDepend);
                    contextQuery.Parameters.Add("@CENSO", TechneDbType.T_CODIGO, dependencia.Faculdade);
                    contextQuery.Parameters.Add("@DEPENDENCIA", TechneDbType.T_CODIGO, dependencia.Dependencia);
                    contextQuery.Parameters.Add("@ATIVA", TechneDbType.T_CODIGO, dependencia.Ativa);
                    contextQuery.Parameters.Add("@DESCRICAO", dependencia.Descricao);
                    contextQuery.Parameters.Add("@EDIFICACAO", dependencia.Edificacao);
                    contextQuery.Parameters.Add("@PAVIMENTO", dependencia.Pavimento);
                    contextQuery.Parameters.Add("@MATRICULA", dependencia.Matricula);
                    contextQuery.Parameters.Add("@DT_CADASTRO", DateTime.Now);
                    contextQuery.Parameters.Add("@DT_ALTERACAO", DateTime.Now);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception e)
                {
                    ctx.Abandon();
                    throw e;
                }
            }
        }

        public static void AlterarSalaDeAula(LyDependencia dependencia)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" UPDATE LY_DEPENDENCIA
                            SET NUM_ALUNOS = @NUM_ALUNOS,
                            AREA = @AREA,
                            SALA_ANEXA = @SALA_ANEXA,
                            MATRICULA = @MATRICULA, 
                            DT_ALTERACAO = @DT_ALTERACAO
                            WHERE FACULDADE = @CENSO
                            AND DEPENDENCIA = @DEPENDENCIA "
                    };
                    contextQuery.Parameters.Add("@CENSO", TechneDbType.T_CODIGO, dependencia.Faculdade);
                    contextQuery.Parameters.Add("@DEPENDENCIA", TechneDbType.T_CODIGO, dependencia.Dependencia);
                    contextQuery.Parameters.Add("@NUM_ALUNOS", TechneDbType.T_NUMERO_PEQUENO, dependencia.NumAlunos);
                    contextQuery.Parameters.Add("@AREA", TechneDbType.T_NUMERO_PEQUENO, dependencia.Area);
                    contextQuery.Parameters.Add("@SALA_ANEXA", dependencia.SalaAnexa);
                    contextQuery.Parameters.Add("@MATRICULA", dependencia.Matricula);
                    contextQuery.Parameters.Add("@DT_ALTERACAO", DateTime.Now);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception e)
                {
                    ctx.Abandon();
                    throw e;
                }
            }
        }

        public static ValidacaoDados ValidarDesabilitarSalaDeAula(LyDependencia dependencia)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dependencia == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(dependencia.Faculdade))
            {
                mensagens.Add("A UNIDADE DE ENSINO é obrigatória.");
            }

            if (string.IsNullOrEmpty(dependencia.Dependencia))
            {
                mensagens.Add("A DEPENDENCIA é obrigatória.");
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

        public static void DesabilitarSalaDeAula(LyDependencia dependencia)
        {
            if (dependencia == null)
            {
                return;
            }

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"UPDATE LY_DEPENDENCIA
                            SET ATIVA = 'N',
                            MATRICULA = @MATRICULA
                            WHERE FACULDADE = @CENSO
                            AND DEPENDENCIA = @DEPENDENCIA "
                    };
                    contextQuery.Parameters.Add("@CENSO", TechneDbType.T_CODIGO, dependencia.Faculdade);
                    contextQuery.Parameters.Add("@DEPENDENCIA", TechneDbType.T_CODIGO, dependencia.Dependencia);
                    contextQuery.Parameters.Add("@MATRICULA", dependencia.Matricula);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static string GerarSalaAulaRec(string censo)
        {
            if (string.IsNullOrEmpty(censo))
            {
                return null;
            }

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    SqlDataReader reader = default(SqlDataReader);
                    string valor = string.Empty;
                    int aux = 0;

                    do
                    {
                        aux = aux + 1;
                        if (reader != null)
                            reader.Close();
                        var contextQuery = new ContextQuery
                        {
                            Command = @"SELECT  COUNT(*) as total
                                    FROM    LY_DEPENDENCIA
                                    WHERE   FACULDADE = @CENSO
                                            AND CAD_SALA_AULA = 'S'
                                            AND TIPO_DEPEND = 'SALA' "
                        };
                        contextQuery.Parameters.Add("@CENSO", TechneDbType.T_CODIGO, censo);

                        reader = ctx.GetDataReader(contextQuery);
                        reader.Read();

                        var total = Convert.ToInt32(reader["total"]) + aux;
                        valor = "SL-" + total.ToString().PadLeft(2, '0');

                    } while (VerificaSala(censo, valor));

                    reader.Close();

                    return valor;
                }
                catch (Exception e)
                {
                    ctx.Abandon();
                    throw e;
                }
            }
        }

        private static bool VerificaSala(string censo, string sala)
        {
            try
            {
                return (!String.IsNullOrEmpty(RNBase.ConsultarCampo("SELECT top(1) * FROM dbo.LY_DEPENDENCIA WHERE FACULDADE = ? AND CAD_SALA_AULA = 'S' AND TIPO_DEPEND = 'SALA' AND DEPENDENCIA=? ", censo, sala)));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GerarBanheiro(string censo)
        {
            bool podeUsar = false;
            string valor = string.Empty;

            if (string.IsNullOrEmpty(censo))
            {
                return null;
            }

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"SELECT  COUNT(*)
                                    FROM    LY_DEPENDENCIA
                                    WHERE   FACULDADE = @CENSO
                                            AND TIPO_DEPEND LIKE 'BANHEIRO%'; "
                    };
                    contextQuery.Parameters.Add("@CENSO", TechneDbType.T_CODIGO, censo);


                    var total = Convert.ToInt32(ctx.GetReturnValue(contextQuery)) + 1;

                    while (!podeUsar)
                    {
                        valor = "BAN-" + total.ToString().PadLeft(2, '0');

                        var contextQuery2 = new ContextQuery(
                                      @"SELECT  1
                                        FROM    LY_DEPENDENCIA
                                        WHERE   FACULDADE = @FACULDADE
                                        AND DEPENDENCIA = @DEPENDENCIA");

                        contextQuery2.Parameters.Add("@FACULDADE", censo);
                        contextQuery2.Parameters.Add("@DEPENDENCIA", valor);

                        var obj = ctx.GetReturnValue(contextQuery2);

                        if (obj == null)
                        {
                            podeUsar = true;
                        }

                        total++;
                    }

                    return valor;
                }
                catch (Exception e)
                {
                    ctx.Abandon();
                    throw e;
                }
            }
        }

        public static string GerarSalaRecurso(string censo)
        {
            if (string.IsNullOrEmpty(censo))
            {
                return null;
            }

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"SELECT  COUNT(*)
                                    FROM    LY_DEPENDENCIA
                                    WHERE   FACULDADE = @CENSO
                                            AND CAD_SALA_AULA = 'S'
                                            AND TIPO_DEPEND = 'SALAAEE' "
                    };
                    contextQuery.Parameters.Add("@CENSO", TechneDbType.T_CODIGO, censo);

                    var total = Convert.ToInt32(ctx.GetReturnValue(contextQuery)) + 1;

                    var valor = "SR-" + total.ToString().PadLeft(2, '0');

                    return valor;
                }
                catch (Exception e)
                {
                    ctx.Abandon();
                    throw e;
                }
            }
        }

        public static decimal ObterNumeroAluno(string faculdade, string dependencia)
        {
            return ExecutarFuncaoDec(@"select TOP 1 NUM_ALUNOS FROM ly_dependencia where faculdade = ? AND dependencia = ?", faculdade, dependencia);
        }

        public static ValidacaoDados ValidarSalaDeAula(LyDependencia dependencia)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dependencia == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(dependencia.Faculdade))
            {
                mensagens.Add("O campo Unidade de Ensino é obrigatório!");
            }
            if (dependencia.NumAlunos == 0)
            {
                mensagens.Add("O campo Número de Alunos é obrigatório!");
            }
            var minimo = System.Configuration.ConfigurationSettings.AppSettings["MinAlunoPorSala"];
            var maximo = System.Configuration.ConfigurationSettings.AppSettings["MaxAlunoPorSala"];

            if (dependencia.NumAlunos > int.Parse(maximo))
            {
                mensagens.Add(
                   string.Format("Não é permitido cadastrar capacidade superior à {0} alunos.",
                   maximo));
            }

            if (dependencia.NumAlunos < int.Parse(minimo))
            {
                mensagens.Add(
                   string.Format("Não é permitido cadastrar capacidade inferior à {0} alunos.",
                   minimo));
            }

            if (dependencia.NumAlunos < int.Parse(minimo)
                || dependencia.NumAlunos > int.Parse(maximo))
            {
                mensagens.Add(
                    string.Format("O número de alunos deve estar entre {0} e {1}!",
                    minimo,
                    maximo));
            }

            if (dependencia.Area != null)
            {
                if (dependencia.Area < 0
                    || dependencia.Area > 999)
                {
                    mensagens.Add("O campo Área deve estar entre 1 e 999!");
                }
            }

            //RETIRADA  DE ACORDO COM A DEMANDA 3192
            //var dt = VerificaAlunoMatriculadoPorSala(dependencia.Faculdade, dependencia.Dependencia);

            //if (dt.Rows.Count > 0)
            //{
            //    if (dependencia.NumAlunos < int.Parse(dt.Rows[0]["TOTAL_ALUNO"].ToString()))
            //    {
            //        mensagens.Add(string.Format("A sala está vinculada à turma {0} e a quantidade de alunos matriculados é superior ao valor informado como capacidade.", dt.Rows[0]["TURMA"].ToString()));
            //    }
            //}
            //SO PODE ALTERAR O NUMERO DE ALUNOS SE NAO TIVER TURMA VINCULADA OU SE A TURMA VINCULADA TIVER UM NUMERO DE ALUNOS MENOR
            //ver como vai colocar esta validação

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

        public static ValidacaoDados ValidarRemover(LyDependencia dependencia)
        {
            var mensagens = new List<string>();

            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dependencia == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(dependencia.Faculdade))
            {
                mensagens.Add("A UNIDADE DE ENSINO é obrigatória.");
            }

            if (string.IsNullOrEmpty(dependencia.Dependencia))
            {
                mensagens.Add("A DEPENDENCIA é obrigatória.");
            }

            if (mensagens.Count == 0)
            {
                if (ExisteDependenciaPor(dependencia.Faculdade, dependencia.Dependencia))
                {
                    mensagens.Add("A sala está vinculada e não pode ser excluída.");
                }

                if (ExisteRespostaDependenciaPor(dependencia.Faculdade, dependencia.Dependencia))
                {
                    mensagens.Add("A sala está vinculado ao RT e não pode ser excluída.");
                }

                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    var contextQuery = new ContextQuery(
                        @"SELECT  1
                            FROM    LY_TURMA
                            WHERE   FACULDADE = @FACULDADE
                                    AND DEPENDENCIA = @DEPENDENCIA");

                    contextQuery.Parameters.Add("@FACULDADE", dependencia.Faculdade);
                    contextQuery.Parameters.Add("@DEPENDENCIA", dependencia.Dependencia);

                    var obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        mensagens.Add("Não é possível excluir esta dependência pois existe uma turma vinculada.");
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
        public static ValidacaoDados ValidarRemoverBanheiroeVestiario(LyDependencia dependencia)
        {
            var mensagens = new List<string>();

            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dependencia == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(dependencia.Faculdade))
            {
                mensagens.Add("A UNIDADE DE ENSINO é obrigatória.");
            }

            if (string.IsNullOrEmpty(dependencia.Dependencia))
            {
                mensagens.Add("A DEPENDENCIA é obrigatória.");
            }

            if (mensagens.Count == 0)
            {
                if (ExisteRespostaDependenciaPor(dependencia.Faculdade, dependencia.Dependencia))
                {
                    mensagens.Add("O banheiro está vinculado ao RT e não pode ser excluído.");
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

        internal static bool ExisteRespostaDependenciaPor(string unidadeEnsino, string dependencia)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(0)
                    FROM    INSPECAOESCOLAR.RESPOSTADEPENDENCIA
                    WHERE   FACULDADE = @FACULDADE
                            AND DEPENDENCIA = @DEPENDENCIA
                            ";

                contextQuery.Parameters.Add("@FACULDADE", unidadeEnsino);
                contextQuery.Parameters.Add("@DEPENDENCIA", dependencia);

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
        internal static bool ExisteDependenciaPor(string unidadeEnsino, string dependencia)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                    FROM    DBO.LY_HOR_AULA
                    WHERE   FACULDADE = @FACULDADE
                            AND DEPENDENCIA = @DEPENDENCIA
                            ";

                contextQuery.Parameters.Add("@FACULDADE", unidadeEnsino);
                contextQuery.Parameters.Add("@DEPENDENCIA", dependencia);

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

        public static void Remover(LyDependencia dependencia)
        {
            if (dependencia == null)
            {
                return;
            }

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"DELETE LY_DEPENDENCIA                            
                            WHERE FACULDADE = @CENSO
                            AND DEPENDENCIA = @DEPENDENCIA "
                    };
                    contextQuery.Parameters.Add("@CENSO", TechneDbType.T_CODIGO, dependencia.Faculdade);
                    contextQuery.Parameters.Add("@DEPENDENCIA", TechneDbType.T_CODIGO, dependencia.Dependencia);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static LyDependencia Bind(IDictionary chaves, IDictionary valores)
        {
            var total = Convert.ToInt32(Math.Round(Convert.ToDouble(valores["AREA"]) * 0.8));

            return new LyDependencia
            {
                Faculdade = chaves == null ? string.Empty : chaves.Contains("CompositeKey") ? Convert.ToString(chaves["CompositeKey"]).Split(';')[0] : string.Empty,
                Dependencia = chaves == null ? string.Empty : chaves.Contains("CompositeKey") ? Convert.ToString(chaves["CompositeKey"]).Split(';')[1] : string.Empty,
                Area = Convert.ToDouble(valores["AREA"]),
                SalaAnexa = (valores["SALA_ANEXA"] == null || valores["SALA_ANEXA"] == "") ? "N" : Convert.ToString(valores["SALA_ANEXA"]),
                NumAlunos = total,
            };
        }

        public static DataTable BuscaMaiorDependencia(string FACULDADE)
        {

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT ISNULL('SA-' + CONVERT(VARCHAR(19),MAX(CONVERT(INT,SUBSTRING(DEPENDENCIA, 4, len(DEPENDENCIA)))) + 1), 'SA-1') as DEPENDENCIA
		                            FROM LY_DEPENDENCIA 
                                        WHERE FACULDADE = @FACULDADE
                                         AND DEPENDENCIA like '%SA-%'"
                };
                contextQuery.Parameters.Add("@FACULDADE", FACULDADE);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarQuantidadeTipoSala(string unidadeFisica)
        {
            if (string.IsNullOrEmpty(unidadeFisica))
            {
                return null;
            }

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT DISTINCT CTD.LYTIPODEPENDENCIAID AS TIPO_DEPEND, TP.NOME, ISNULL(TCET.QUANTIDADE,0) AS QUANTIDADE, TCET.QUANTIDADE AS QTDREAL    
                                     FROM LYCURSO_LYTIPODEPENDENCIA CTD
                                        INNER JOIN LY_TIPO_DEPENDENCIA TP ON CTD.LYTIPODEPENDENCIAID = TP.TIPO_DEPEND
                                        LEFT JOIN TCE_TIPO_DEPENDENCIA_UNIDADE_FISICA TCET ON CTD.LYTIPODEPENDENCIAID = TCET.TIPO_DEPENDENCIA
                                        AND  TCET.UNIDADE_FISICA = @UNIDADE_FISICA
                                     ORDER BY TP.NOME
                              "
                };
                contextQuery.Parameters.Add("@UNIDADE_FISICA", unidadeFisica);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable VerificaExistenciaTipoDepen(string FACULDADE, string TIPODEPENDEN)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT distinct ID_TIPO_DEPENDENCIA_UNIDADE_FISICA 
		                                FROM TCE_TIPO_DEPENDENCIA_UNIDADE_FISICA  
                                        WHERE UNIDADE_FISICA = @UNIDADE_FISICA
                                        AND TIPO_DEPENDENCIA = @TIPO_DEPENDENCIA"
                };
                contextQuery.Parameters.Add("@UNIDADE_FISICA", FACULDADE);
                contextQuery.Parameters.Add("@TIPO_DEPENDENCIA", TIPODEPENDEN);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable VerificaAlunoMatriculadoPorSala(string censo, string dependencia)
        {
            if (string.IsNullOrEmpty(censo))
            {
                return null;
            }

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"SELECT T.TURMA,COUNT(DISTINCT ALUNO) AS TOTAL_ALUNO
                                    FROM LY_MATRICULA M
                                    INNER  JOIN LY_TURMA T ON M.TURMA = T.TURMA AND M.DISCIPLINA = T.DISCIPLINA AND M.ANO = T.ANO AND M.SEMESTRE = T.SEMESTRE 
                                    WHERE SIT_MATRICULA <> 'Cancelado'
                                    AND T.FACULDADE = @CENSO
                                    AND T.DEPENDENCIA = @DEPENDENCIA
                                    GROUP BY T.TURMA
                                    ORDER BY COUNT(DISTINCT ALUNO)
                                    "
                    };
                    contextQuery.Parameters.Add("@CENSO", TechneDbType.T_CODIGO, censo);
                    contextQuery.Parameters.Add("@DEPENDENCIA", TechneDbType.T_CODIGO, dependencia);

                    return ctx.GetDataTable(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static void Alterar(LyDependencia dependencia)
        {
            var contextQuery = new ContextQuery(
                @"UPDATE  DBO.LY_DEPENDENCIA
                SET     TIPO_DEPEND = @TIPO_DEPEND,
                        DESCRICAO = @DESCRICAO,
                        ATIVA = @ATIVA,
                        SALA_ANEXA = @SALA_ANEXA,
                        OBS = @OBS,
                        EDIFICACAO = @EDIFICACAO,
                        PAVIMENTO = @PAVIMENTO,
                        AREA = @AREA,
                        NUM_ALUNOS = @NUM_ALUNOS,
                        MATRICULA = @MATRICULA,
                        DT_ALTERACAO = GETDATE()
                WHERE   FACULDADE = @FACULDADE
                        AND DEPENDENCIA = @DEPENDENCIA");

            contextQuery.Parameters.Add("@FACULDADE", dependencia.Faculdade);
            contextQuery.Parameters.Add("@DEPENDENCIA", dependencia.Dependencia);
            contextQuery.Parameters.Add("@TIPO_DEPEND", dependencia.TipoDepend);
            contextQuery.Parameters.Add("@DESCRICAO", dependencia.Descricao);
            contextQuery.Parameters.Add("@ATIVA", dependencia.Ativa);
            contextQuery.Parameters.Add("@SALA_ANEXA", dependencia.SalaAnexa);
            contextQuery.Parameters.Add("@OBS", dependencia.Obs);
            contextQuery.Parameters.Add("@EDIFICACAO", dependencia.Edificacao);
            contextQuery.Parameters.Add("@PAVIMENTO", dependencia.Pavimento);
            contextQuery.Parameters.Add("@AREA", dependencia.Area);
            contextQuery.Parameters.Add("@MATRICULA", dependencia.Matricula);
            contextQuery.Parameters.Add("@NUM_ALUNOS", dependencia.NumAlunos);

            ExecutarAlteracao(contextQuery);
        }
        public static void AlterarBanheiroeVestiario(LyDependencia dependencia)
        {
            var contextQuery = new ContextQuery(
                @"UPDATE  DBO.LY_DEPENDENCIA
                SET     TIPO_DEPEND = @TIPO_DEPEND,
                        DESCRICAO = @DESCRICAO,
                        ATIVA = @ATIVA,
                        EDIFICACAO = @EDIFICACAO,
                        PAVIMENTO = @PAVIMENTO,
                        MATRICULA = @MATRICULA,
                        DT_ALTERACAO = GETDATE()
                WHERE   FACULDADE = @FACULDADE
                        AND DEPENDENCIA = @DEPENDENCIA");

            contextQuery.Parameters.Add("@FACULDADE", dependencia.Faculdade);
            contextQuery.Parameters.Add("@DEPENDENCIA", dependencia.Dependencia);
            contextQuery.Parameters.Add("@TIPO_DEPEND", dependencia.TipoDepend);
            contextQuery.Parameters.Add("@DESCRICAO", dependencia.Descricao);
            contextQuery.Parameters.Add("@ATIVA", dependencia.Ativa);
            contextQuery.Parameters.Add("@EDIFICACAO", dependencia.Edificacao);
            contextQuery.Parameters.Add("@PAVIMENTO", dependencia.Pavimento);
            contextQuery.Parameters.Add("@MATRICULA", dependencia.Matricula);

            ExecutarAlteracao(contextQuery);
        }

        public static ValidacaoDados ValidarInserir(LyDependencia dependencia)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dependencia == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(dependencia.Faculdade))
            {
                mensagens.Add("O campo UNIDADE DE ENSINO é obrigatório!");
            }

            if (string.IsNullOrEmpty(dependencia.TipoDepend))
            {
                mensagens.Add("O campo TIPO DE DEPENDENCIA é obrigatório!");
            }

            if (string.IsNullOrEmpty(dependencia.Edificacao))
            {
                mensagens.Add("O campo EDIFICAÇÃO é obrigatório!");
            }

            if (string.IsNullOrEmpty(dependencia.Pavimento))
            {
                mensagens.Add("O campo PAVIMENTO é obrigatório!");
            }

            if (string.IsNullOrEmpty(dependencia.Dependencia))
            {
                mensagens.Add("O campo DEPENDÊNCIA é obrigatório!");
            }
            else
            {
                if (dependencia.TipoDepend == "SALA" && dependencia.Dependencia != "SL-")
                {
                    mensagens.Add("O campo DEPENDÊNCIA deve conter a sigla SL");
                }
            }

            if (dependencia.TipoDepend == "SALA" || dependencia.TipoDepend == "SALAAEE")
            {
                if (dependencia.NumAlunos == 0)
                {
                    mensagens.Add("O campo Número de Alunos é obrigatório!");
                }
                var minimo = System.Configuration.ConfigurationSettings.AppSettings["MinAlunoPorSala"];
                var maximo = System.Configuration.ConfigurationSettings.AppSettings["MaxAlunoPorSala"];

                if (dependencia.NumAlunos > int.Parse(maximo))
                {
                    mensagens.Add(
                       string.Format("Não é permitido cadastrar capacidade superior à {0} alunos.",
                       maximo));
                }

                if (dependencia.NumAlunos < int.Parse(minimo))
                {
                    mensagens.Add(
                       string.Format("Não é permitido cadastrar capacidade inferior à {0} alunos.",
                       minimo));
                }

                if (dependencia.NumAlunos < int.Parse(minimo)
                    || dependencia.NumAlunos > int.Parse(maximo))
                {
                    mensagens.Add(
                        string.Format("O número de alunos deve estar entre {0} e {1}!",
                        minimo,
                        maximo));
                }

                if (dependencia.Area != null)
                {
                    if (dependencia.Area < 0
                        || dependencia.Area > 999)
                    {
                        mensagens.Add("O campo Área deve estar entre 1 e 999!");
                    }
                }

                var dt = VerificaAlunoMatriculadoPorSala(dependencia.Faculdade, dependencia.Dependencia);

                if (dt.Rows.Count > 0)
                {
                    if (dependencia.NumAlunos < int.Parse(dt.Rows[0]["TOTAL_ALUNO"].ToString()))
                    {
                        mensagens.Add(string.Format("A sala está vinculada à turma {0} e a quantidade de alunos matriculados é superior ao valor informado como capacidade.", dt.Rows[0]["TURMA"].ToString()));
                    }
                }
                //SO PODE ALTERAR O NUMERO DE ALUNOS SE NAO TIVER TURMA VINCULADA OU SE A TURMA VINCULADA TIVER UM NUMERO DE ALUNOS MENOR
                //ver como vai colocar esta validação
            }

            if (mensagens.Count == 0)
            {

                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    var contextQuery = new ContextQuery(
                        @"SELECT  1
                        FROM    LY_DEPENDENCIA
                        WHERE   FACULDADE = @FACULDADE
                        AND DEPENDENCIA = @DEPENDENCIA");

                    contextQuery.Parameters.Add("@FACULDADE", dependencia.Faculdade);
                    contextQuery.Parameters.Add("@DEPENDENCIA", dependencia.Dependencia);

                    var obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        mensagens.Add("Já existe uma DEPENDENCIA cadastrada com estes mesmos dados.");
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

        public static ValidacaoDados ValidarInserirBanheiroeVestiario(LyDependencia dependencia)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dependencia == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(dependencia.Faculdade))
            {
                mensagens.Add("O campo UNIDADE DE ENSINO é obrigatório!");
            }

            if (string.IsNullOrEmpty(dependencia.TipoDepend))
            {
                mensagens.Add("O campo TIPO DE DEPENDENCIA é obrigatório!");
            }

            if (string.IsNullOrEmpty(dependencia.Edificacao))
            {
                mensagens.Add("O campo EDIFICAÇÃO é obrigatório!");
            }

            if (string.IsNullOrEmpty(dependencia.Pavimento))
            {
                mensagens.Add("O campo PAVIMENTO é obrigatório!");
            }

            //if (dependencia.TipoDepend == "SALA" || dependencia.TipoDepend == "SALAAEE")
            //{
            //    if (dependencia.NumAlunos == 0)
            //    {
            //        mensagens.Add("O campo Número de Alunos é obrigatório!");
            //    }
            //    var minimo = System.Configuration.ConfigurationSettings.AppSettings["MinAlunoPorSala"];
            //    var maximo = System.Configuration.ConfigurationSettings.AppSettings["MaxAlunoPorSala"];

            //    if (dependencia.NumAlunos > int.Parse(maximo))
            //    {
            //        mensagens.Add(
            //           string.Format("Não é permitido cadastrar capacidade superior à {0} alunos.",
            //           maximo));
            //    }

            //    if (dependencia.NumAlunos < int.Parse(minimo))
            //    {
            //        mensagens.Add(
            //           string.Format("Não é permitido cadastrar capacidade inferior à {0} alunos.",
            //           minimo));
            //    }

            //    if (dependencia.NumAlunos < int.Parse(minimo)
            //        || dependencia.NumAlunos > int.Parse(maximo))
            //    {
            //        mensagens.Add(
            //            string.Format("O número de alunos deve estar entre {0} e {1}!",
            //            minimo,
            //            maximo));
            //    }

            //    if (dependencia.Area != null)
            //    {
            //        if (dependencia.Area < 0
            //            || dependencia.Area > 999)
            //        {
            //            mensagens.Add("O campo Área deve estar entre 1 e 999!");
            //        }
            //    }

            //    var dt = VerificaAlunoMatriculadoPorSala(dependencia.Faculdade, dependencia.Dependencia);

            //    if (dt.Rows.Count > 0)
            //    {
            //        if (dependencia.NumAlunos < int.Parse(dt.Rows[0]["TOTAL_ALUNO"].ToString()))
            //        {
            //            mensagens.Add(string.Format("A sala está vinculada à turma {0} e a quantidade de alunos matriculados é superior ao valor informado como capacidade.", dt.Rows[0]["TURMA"].ToString()));
            //        }
            //    }
            //    //SO PODE ALTERAR O NUMERO DE ALUNOS SE NAO TIVER TURMA VINCULADA OU SE A TURMA VINCULADA TIVER UM NUMERO DE ALUNOS MENOR
            //    //ver como vai colocar esta validação
            //}

            if (mensagens.Count == 0)
            {

                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    var contextQuery = new ContextQuery(
                        @"SELECT  1
                        FROM    LY_DEPENDENCIA
                        WHERE   FACULDADE = @FACULDADE
                        AND DEPENDENCIA = @DEPENDENCIA");

                    contextQuery.Parameters.Add("@FACULDADE", dependencia.Faculdade);
                    contextQuery.Parameters.Add("@DEPENDENCIA", dependencia.Dependencia);

                    var obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        mensagens.Add("Já existe uma DEPENDENCIA cadastrada com estes mesmos dados.");
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

        public static ValidacaoDados Validar(DadosSalaAlternativa SalaAlternativa)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (SalaAlternativa == null)
            {
                return validacaoDados;
            }

            if (Convert.ToInt32(SalaAlternativa.NUM_ALUNOS) <= 0)
            {
                mensagens.Add("Capacidade máxima de alunos não pode ser menor ou igual a zero.");
            }

            if (string.IsNullOrEmpty(SalaAlternativa.DEPENDENCIA))
            {
                mensagens.Add("O campo DEPENDENCIA é obrigatório!");
            }

            if (Convert.ToInt32(VerificaQuantidadeDependenciaParaSalvar(SalaAlternativa.FACULDADE, SalaAlternativa.TIPO_DEPEND).Rows[0][0]) <= Convert.ToInt32(VerificaQuantidadeDependenciaSalva(SalaAlternativa.FACULDADE, SalaAlternativa.TIPO_DEPEND).Rows[0][0]))
            {
                if (Convert.ToString(VerificaQuantidadeDependenciaParaSalvar(SalaAlternativa.FACULDADE, SalaAlternativa.TIPO_DEPEND).Rows[0][0]) == "0")
                {
                    mensagens.Add("Quantidade de dependência menor que a quantidade de registros de salas alternativas!");
                }
                else
                {
                    mensagens.Add("Quantidade de dependência menor que a quantidade de registros de salas alternativas!");
                }
            }


            if (string.IsNullOrEmpty(SalaAlternativa.TIPO_DEPEND))
            {
                mensagens.Add("O campo TIPO é obrigatório!");
            }

            if (string.IsNullOrEmpty(SalaAlternativa.NUM_ALUNOS))
            {
                mensagens.Add("O campo CAPACIDADE MÁXIMA DE ALUNOS é obrigatório!");
            }
            if (Convert.ToInt32(SalaAlternativa.NUM_ALUNOS) > 45)
            {
                mensagens.Add("Capacidade máxima de alunos não pode exceder a 45!");
            }
            if (string.IsNullOrEmpty(SalaAlternativa.ATIVA))
            {
                mensagens.Add("O campo ATIVA é obrigatório!");
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

        public static ValidacaoDados Validaralterar(DadosSalaAlternativa SalaAlternativa)
        {
            List<string> listaTipoCurso = new List<string>();
            List<string> ListaCursoTipoDependencia = new List<string>();
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (SalaAlternativa == null)
            {
                return validacaoDados;
            }
            if (SalaAlternativa.ATIVA != "S")
            {
                if (Convert.ToUInt32(VerificaSituacaoSala(Convert.ToString(SalaAlternativa.FACULDADE), SalaAlternativa.DEPENDENCIA).Rows.Count) != 0)
                {
                    if (Convert.ToString(VerificaSituacaoSala(Convert.ToString(SalaAlternativa.FACULDADE), SalaAlternativa.DEPENDENCIA).Rows[0][0]) == "Aberta")
                    {
                        mensagens.Add("A sala alternativa não pode ser desativada, pois possui vínculo com turmas ativas/vigentes!");
                    }

                }
            }

            if (Convert.ToInt32(SalaAlternativa.NUM_ALUNOS) <= 0)
            {
                mensagens.Add("Capacidade máxima de alunos não pode ser menor ou igual a zero.");
            }

            if (SalaAlternativa.TIPO_DEPEND_ANTERIOR != SalaAlternativa.TIPO_DEPEND || SalaAlternativa.ATIVA_ANTERIOR != SalaAlternativa.ATIVA)
            {
                if (Convert.ToInt32(VerificaQuantidadeDependenciaParaSalvar(SalaAlternativa.FACULDADE, SalaAlternativa.TIPO_DEPEND).Rows[0][0]) <= Convert.ToInt32(VerificaQuantidadeDependenciaSalva(SalaAlternativa.FACULDADE, SalaAlternativa.TIPO_DEPEND).Rows[0][0]))
                {
                    if (SalaAlternativa.ATIVA == "S")
                    {
                        mensagens.Add("Quantidade de dependência menor que a quantidade de registros de salas alternativas!");
                    }
                }
            }

            if (string.IsNullOrEmpty(SalaAlternativa.DEPENDENCIA))
            {
                mensagens.Add("O campo DEPENDENCIA é obrigatório!");
            }


            if (string.IsNullOrEmpty(SalaAlternativa.TIPO_DEPEND))
            {
                mensagens.Add("O campo TIPO é obrigatório!");
            }

            if (string.IsNullOrEmpty(SalaAlternativa.NUM_ALUNOS))
            {
                mensagens.Add("O campo CAPACIDADE MÁXIMA DE ALUNOS é obrigatório!");
            }
            if (Convert.ToInt32(SalaAlternativa.NUM_ALUNOS) > 45)
            {
                mensagens.Add("Capacidade máxima de alunos não pode exceder a 45!");
            }
            if (string.IsNullOrEmpty(SalaAlternativa.ATIVA))
            {
                mensagens.Add("O campo ATIVA é obrigatório!");
            }
            if (SalaAlternativa.TIPO_DEPEND_ANTERIOR != SalaAlternativa.TIPO_DEPEND)
            {

                foreach (DataRow row in CursosTurmasAbertas(SalaAlternativa.FACULDADE, SalaAlternativa.DEPENDENCIA).Rows)
                {
                    string CursoTurma = string.Empty;

                    CursoTurma = row["CURSO"].ToString();

                    listaTipoCurso.Add(CursoTurma);
                }


                foreach (DataRow row in CursoTipoDependencia(SalaAlternativa.TIPO_DEPEND).Rows)
                {
                    string CursoDependencia = string.Empty;


                    CursoDependencia = row["LYCURSOID"].ToString();

                    ListaCursoTipoDependencia.Add(CursoDependencia);

                }

                List<string> result = listaTipoCurso.Except(ListaCursoTipoDependencia).ToList();

                if (result.Count == 1)
                {
                    mensagens.Add("Tipo depêndencia selecionado não parametrizado para cursos de turmas ativas já associadas a esta sala alternativa!");
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

        public static DataTable VerificaQuantidadeDependenciaSalva(string FACULDADE, string TIPODEPENDEN)
        {

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT COUNT(TIPO_DEPEND) AS QUANTIDADE
		                                FROM LY_DEPENDENCIA  
                                        WHERE FACULDADE = @UNIDADE_FISICA
                                        AND TIPO_DEPEND = @TIPO_DEPENDENCIA
                                        AND ATIVA <> 'N'"
                };
                contextQuery.Parameters.Add("@UNIDADE_FISICA", FACULDADE);
                contextQuery.Parameters.Add("@TIPO_DEPENDENCIA", TIPODEPENDEN);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable VerificaQuantidadeDependenciaParaSalvar(string FACULDADE, string TIPODEPENDEN)
        {

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT ISNULL(TCET.QUANTIDADE,0) AS QUANTIDADE    
                                     FROM LYCURSO_LYTIPODEPENDENCIA CTD
                                        INNER JOIN LY_TIPO_DEPENDENCIA TP ON CTD.LYTIPODEPENDENCIAID = TP.TIPO_DEPEND
                                        LEFT JOIN TCE_TIPO_DEPENDENCIA_UNIDADE_FISICA TCET ON CTD.LYTIPODEPENDENCIAID = TCET.TIPO_DEPENDENCIA
                                        AND  TCET.UNIDADE_FISICA = @UNIDADE_FISICA
                                        WHERE CTD.LYTIPODEPENDENCIAID = @TIPO_DEPENDENCIA
                                     ORDER BY TCET.QUANTIDADE DESC ,TP.NOME "
                };
                contextQuery.Parameters.Add("@UNIDADE_FISICA", FACULDADE);
                contextQuery.Parameters.Add("@TIPO_DEPENDENCIA", TIPODEPENDEN);

                return ctx.GetDataTable(contextQuery);
            }

        }

        public static ValidacaoDados ValidarAlterar(LyDependencia dependencia)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dependencia == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(dependencia.Faculdade))
            {
                mensagens.Add("O campo UNIDADE DE ENSINO é obrigatório!");
            }

            if (string.IsNullOrEmpty(dependencia.Dependencia))
            {
                mensagens.Add("O campo DEPENDENCIA é obrigatório!");
            }

            if (string.IsNullOrEmpty(dependencia.TipoDepend))
            {
                mensagens.Add("O campo TIPO DE DEPENDENCIA é obrigatório!");
            }

            if (string.IsNullOrEmpty(dependencia.Edificacao))
            {
                mensagens.Add("O campo EDIFICAÇÃO é obrigatório!");
            }

            if (string.IsNullOrEmpty(dependencia.Pavimento))
            {
                mensagens.Add("O campo PAVIMENTO é obrigatório!");
            }

            if (dependencia.TipoDepend == "SALA" || dependencia.TipoDepend == "SALAAEE")
            {
                if (dependencia.NumAlunos == 0)
                {
                    mensagens.Add("O campo Número de Alunos é obrigatório!");
                }
                var minimo = System.Configuration.ConfigurationSettings.AppSettings["MinAlunoPorSala"];
                var maximo = System.Configuration.ConfigurationSettings.AppSettings["MaxAlunoPorSala"];

                if (dependencia.NumAlunos > int.Parse(maximo))
                {
                    mensagens.Add(
                       string.Format("Não é permitido cadastrar capacidade superior à {0} alunos.",
                       maximo));
                }

                if (dependencia.NumAlunos < int.Parse(minimo))
                {
                    mensagens.Add(
                       string.Format("Não é permitido cadastrar capacidade inferior à {0} alunos.",
                       minimo));
                }

                if (dependencia.NumAlunos < int.Parse(minimo)
                    || dependencia.NumAlunos > int.Parse(maximo))
                {
                    mensagens.Add(
                        string.Format("O número de alunos deve estar entre {0} e {1}!",
                        minimo,
                        maximo));
                }

                if (dependencia.Area != null)
                {
                    if (dependencia.Area < 0
                        || dependencia.Area > 999)
                    {
                        mensagens.Add("O campo Área deve estar entre 1 e 999!");
                    }
                }

                if (EhDependenciaAtiva(dependencia.Faculdade, dependencia.Dependencia) && dependencia.Ativa == "N")
                {
                    var dt = Dependencia.VerificaAlunoMatriculadoPorSala(dependencia.Faculdade, dependencia.Dependencia);

                    if (dt.Rows.Count > 0)
                    {
                        mensagens.Add("Esta sala de aula não pode ser desativada pois possui " + dt.Rows.Count.ToString() + " turma(s) vinculada(s).");
                    }
                }

                //RETIRADA  DE ACORDO COM A DEMANDA 3192
                //var dt = VerificaAlunoMatriculadoPorSala(dependencia.Faculdade, dependencia.Dependencia);

                //if (dt.Rows.Count > 0)
                //{
                //    if (dependencia.NumAlunos < int.Parse(dt.Rows[0]["TOTAL_ALUNO"].ToString()))
                //    {
                //        mensagens.Add(string.Format("A sala está vinculada à turma {0} e a quantidade de alunos matriculados é superior ao valor informado como capacidade.", dt.Rows[0]["TURMA"].ToString()));
                //    }
                //}
                //SO PODE ALTERAR O NUMERO DE ALUNOS SE NAO TIVER TURMA VINCULADA OU SE A TURMA VINCULADA TIVER UM NUMERO DE ALUNOS MENOR
                //ver como vai colocar esta validação
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
        public static ValidacaoDados ValidarAlterarBanheiroeVestiario(LyDependencia dependencia)
        {
            
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dependencia == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(dependencia.Faculdade))
            {
                mensagens.Add("O campo UNIDADE DE ENSINO é obrigatório!");
            }

            if (string.IsNullOrEmpty(dependencia.Dependencia))
            {
                mensagens.Add("O campo DEPENDENCIA é obrigatório!");
            }

            if (string.IsNullOrEmpty(dependencia.TipoDepend))
            {
                mensagens.Add("O campo TIPO DE DEPENDENCIA é obrigatório!");
            }

            if (string.IsNullOrEmpty(dependencia.Edificacao))
            {
                mensagens.Add("O campo EDIFICAÇÃO é obrigatório!");
            }

            if (string.IsNullOrEmpty(dependencia.Pavimento))
            {
                mensagens.Add("O campo PAVIMENTO é obrigatório!");
            }

            if (mensagens.Count == 0)
            {
                if (dependencia.Ativa == "N")
                {
                    if (ExisteRespostaDependenciaAnoVigentePor(dependencia.Faculdade, dependencia.Dependencia))
                    {
                        mensagens.Add("O banheiro está vinculado ao RT e não pode ser desativado.");
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

        public static ValidacaoDados ValidaQuadroSalaAlternativa(DadosSalaAlternativa SalaAlternativa)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (Convert.ToUInt32(VerificaAtiva(SalaAlternativa.FACULDADE, SalaAlternativa.TIPO_DEPEND).Rows[0][0]) > SalaAlternativa.quatidade)
            {
                mensagens.Add("A quantidade de dependência é inferior a quantidade de registros ativos para este tipo de dependência na lista de salas alternativas.");
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

        public static DataTable VerificaAtiva(string FACULDADE, string TIPODEPENDEN)
        {

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT COUNT(TIPO_DEPEND) AS QUANTIDADE
		                                FROM LY_DEPENDENCIA  
                                        WHERE FACULDADE = @UNIDADE_FISICA
                                        AND TIPO_DEPEND = @TIPO_DEPENDENCIA
                                        AND ATIVA = 'S'"
                };
                contextQuery.Parameters.Add("@UNIDADE_FISICA", FACULDADE);
                contextQuery.Parameters.Add("@TIPO_DEPENDENCIA", TIPODEPENDEN);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static void DeleteDependencia(string id)
        {
            var ctx = DataContextBuilder.FromLyceum.UsingLock();
            try
            {
                var contextQuery = new ContextQuery(
                    @"DELETE FROM TCE_TIPO_DEPENDENCIA_UNIDADE_FISICA
                    WHERE ID_TIPO_DEPENDENCIA_UNIDADE_FISICA = @ID_TIPO_DEPENDENCIA_UNIDADE_FISICA");

                contextQuery.Parameters.Add("@ID_TIPO_DEPENDENCIA_UNIDADE_FISICA", id);

                ExecutarAlteracao(contextQuery);
            }
            catch (Exception)
            {
                ctx.Abandon();
            }
        }

        public static DataTable VerificaSituacaoSala(string FACULDADE, string DEPENDENCIA)
        {

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT distinct TOP 1 SIT_TURMA  
		                                FROM LY_TURMA  
                                        WHERE FACULDADE = @UNIDADE_FISICA
                                        AND ANO = @ANO
                                        AND DEPENDENCIA = @DEPENDENCIA
                                        AND SIT_TURMA = 'Aberta'"
                };
                contextQuery.Parameters.Add("@UNIDADE_FISICA", FACULDADE);
                contextQuery.Parameters.Add("@ANO", DateTime.Now.Year);
                contextQuery.Parameters.Add("@DEPENDENCIA", DEPENDENCIA);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable CursosTurmasAbertas(string FACULDADE, string DEPENDENCIA)
        {

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT DISTINCT T.CURSO
                                        FROM LY_TURMA T
                                              INNER JOIN LY_DEPENDENCIA D
                                                    ON (T.FACULDADE = D.FACULDADE
                                                    AND T.DEPENDENCIA = D.DEPENDENCIA)
                                        WHERE T.SIT_TURMA = 'ABERTA'
                                              AND T.DEPENDENCIA = @DEPENDENCIA 
                                              AND T.FACULDADE = @UNIDADE_FISICA "
                };
                contextQuery.Parameters.Add("@DEPENDENCIA", DEPENDENCIA);
                contextQuery.Parameters.Add("@UNIDADE_FISICA", FACULDADE);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable CursoTipoDependencia(string TIPO_DEPEND)
        {

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT LYCURSOID
                                    FROM LYCURSO_LYTIPODEPENDENCIA
                                        WHERE LYTIPODEPENDENCIAID = @TIPO_DEPEND"
                };
                contextQuery.Parameters.Add("@TIPO_DEPEND", TIPO_DEPEND);

                return ctx.GetDataTable(contextQuery);
            }
        }


        private static bool ExisteRespostaDependenciaAnoVigentePor(string unidadeEnsino, string dependencia)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(0)
                                    FROM    INSPECAOESCOLAR.RESPOSTADEPENDENCIA RP 
                                    INNER JOIN INSPECAOESCOLAR.CAMPANHAESCOLA CE ON CE.CAMPANHAESCOLAID = RP.CAMPANHAESCOLAID
                                    INNER JOIN INSPECAOESCOLAR.CAMPANHA C ON C.CAMPANHAID = CE.CAMPANHAID
                                    WHERE   FACULDADE = @FACULDADE
                                            AND DEPENDENCIA = @DEPENDENCIA
                                            AND C.ANO = @ANO
                                                                ";

                contextQuery.Parameters.Add("@FACULDADE", unidadeEnsino);
                contextQuery.Parameters.Add("@DEPENDENCIA", dependencia);
                contextQuery.Parameters.Add("@ANO", DateTime.Now.Year);

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

        private static bool EhDependenciaAtiva(string censo, string dependencia )
        {
            bool alunoAtivo = false;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            object obj = new Object();
            bool ativa = false;


            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT 
                            COUNT(*)
                    FROM    LY_DEPENDENCIA                            
                    WHERE   FACULDADE = @FACULDADE
                            AND DEPENDENCIA = @DEPENDENCIA 
                            AND ATIVA = 'S'                           
                    "
                };

                contextQuery.Parameters.Add("@FACULDADE", censo);
                contextQuery.Parameters.Add("@DEPENDENCIA", dependencia);

                obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    if (Convert.ToInt32(obj) > 0)
                    {
                        ativa = true;
                    }                    
                }

                return ativa;
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
