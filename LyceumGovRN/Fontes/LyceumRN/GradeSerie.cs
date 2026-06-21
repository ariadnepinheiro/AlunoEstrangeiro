namespace Techne.Lyceum.RN
{
    using System;
    using System.Data;
    using System.Text;
    using Seeduc.Infra.Data;
    using Techne.Data;
    using Techne.Lyceum.CR;

    public struct ParametrosCurso
    {
        public string ano;

        public string semestre;

        public string curso;

        public string turno;

        public string serie;

        public string unidade_responsavel;

        public string nucleo;
    }

    public class GradeSerie : RNBase
    {
        private static QueryTable ExecutarConsulta(StringBuilder consulta, ParametrosCurso paramCurso)
        {
            TConnection connection = Config.CreateConnection();
            QueryTable tabelaDados = new QueryTable(consulta.ToString());

            if (String.IsNullOrEmpty(paramCurso.nucleo))
                tabelaDados.Query(connection, paramCurso.ano, paramCurso.semestre,
                paramCurso.curso, paramCurso.turno, paramCurso.serie, paramCurso.unidade_responsavel);
            else
                tabelaDados.Query(connection, paramCurso.ano, paramCurso.semestre, paramCurso.nucleo);

            return tabelaDados;
        }

        public static QueryTable Consultar(string ano, string semestre, string curso, string turno, string serie, string unidade_responsavel)
        {
            var strConsulta = new StringBuilder();

            strConsulta.Append(" SELECT grade_id, curso, turno, serie, ano, semestre, grade, capacidade, unidade_responsavel, dependencia, curriculo, faculdade ");
            strConsulta.Append(" FROM LY_GRADE_SERIE WHERE ANO = ? AND SEMESTRE = ? AND CURSO = ? AND TURNO =? ");
            strConsulta.Append(" AND SERIE = ? AND UNIDADE_RESPONSAVEL = ? ORDER BY GRADE");

            var oPC = new ParametrosCurso { ano = ano, semestre = semestre, curso = curso, turno = turno, serie = serie, unidade_responsavel = unidade_responsavel };

            return ExecutarConsulta(strConsulta, oPC);
        }

        public static QueryTable ConsultarDadosTurma(string grade_id)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();
            QueryTable qt = null;

            string sql = "select ano, " +
                        "semestre,  " +
                        "turno,  " +
                        "c.NOME as curso,  " +
                        "u.NOME_COMP as unidade,  " +
                        "serie, grade  " +
                        "from LY_GRADE_SERIE g " +
                        "inner join LY_CURSO c on (c.CURSO = g.curso) " +
                        "inner join LY_UNIDADE_ENSINO u on (g.UNIDADE_RESPONSAVEL = u.UNIDADE_ENS) " +
                        "where GRADE_ID = ?";
            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, grade_id);
            }
            finally
            {
                connection.Close();
            }
            return qt;
        }

        public static QueryTable ConsultarDocentesTurma(string grade_id)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();
            QueryTable qt = null;

            string sql = @" SELECT DISTINCT PE.NOME_COMPL AS nome,
                                            DO.NUM_FUNC   AS num_func
                            FROM   LY_GRADE_SERIE GS
                                   JOIN LY_GRADE_TURMA GT
                                     ON GS.GRADE_ID = GT.GRADE_ID
                                   JOIN LY_AULA_DOCENTE A
                                     ON GT.DISCIPLINA = A.DISCIPLINA
                                        AND GT.TURMA = A.TURMA
                                        AND GT.ANO = A.ANO
                                        AND GT.SEMESTRE = A.SEMESTRE
                                   JOIN LY_HOR_AULA H
                                     ON A.TURNO = H.TURNO
                                        AND A.FACULDADE = H.FACULDADE
                                        AND A.DIA_SEMANA = H.DIA_SEMANA
                                        AND A.AULA = H.AULA
                                        AND A.DISCIPLINA = H.DISCIPLINA
                                        AND A.TURMA = H.TURMA
                                        AND A.ANO = H.ANO
                                        AND A.SEMESTRE = H.SEMESTRE
                                   JOIN LY_DISCIPLINA D
                                     ON A.DISCIPLINA = D.DISCIPLINA
                                   JOIN LY_DOCENTE DO
                                     ON A.NUM_FUNC = DO.NUM_FUNC
                                   JOIN LY_PESSOA PE
                                     ON PE.PESSOA = DO.PESSOA
                            WHERE  GS.GRADE_ID = ?
                            ORDER  BY PE.NOME_COMPL  ";
            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, grade_id);
            }
            finally
            {
                connection.Close();
            }
            return qt;
        }

        public static Ly_grade_serie.Row Consultar(decimal grade_id)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                return Ly_grade_serie.QueryFirstRow(connection, "grade_id = ?", grade_id);
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable ConsultarTurma(string ano, string semestre, string nucleo, string municipio, string unidade_responsavel, string turma)
        {
            StringBuilder strConsulta = new StringBuilder();
            strConsulta.Append(@" SELECT DISTINCT 
                    GS.grade_id, 
                    GS.curso, 
                    C.NOME nomeCurso, 
                    GS.turno, 
                    T.DESCRICAO descricaoTurno,
                    GS.serie, 
                    S.DESCRICAO descricaoSerie,
                    GS.ano,
                    GS.semestre,
                    GS.grade,
                    GS.capacidade,
                    GS.unidade_responsavel,
                    UE.NOME_COMP nomeUnidadeResponsavel,
                    GS.dependencia,
                    GS.curriculo,
                    GS.faculdade,
                    (select
		                case TU.em_elaboracao
			            when 'S' then 'Horário incompleto'
			            when 'N' then 'Horário completo'
			            else 'Sem alocação' end) em_elaboracao,
                    substring(gs.GRADE, 0, 1 + LEN(GS.GRADE) - CHARINDEX('-', REVERSE(GS.GRADE))) grade_token,
                    TU.turma_integracao as sufixo,
                    tu.sit_turma
                FROM
                    LY_GRADE_SERIE  GS
                    inner join ly_unidade_ensino UE ON
                        UE.unidade_ens = GS.faculdade
                        and UE.UNIDADE_ENS = GS.UNIDADE_RESPONSAVEL
                    inner join ly_nucleo N ON
                        N.nucleo = UE.nucleo
                    inner join ly_turno T ON
                        T.turno = GS.turno
                    inner join ly_serie S ON 
                        S.SERIE = GS.SERIE AND 
                        S.TURNO = GS.TURNO AND 
                        S.CURRICULO = GS.CURRICULO AND
						S.CURSO = GS.CURSO
                    inner join LY_CURSO C ON 
                        C.CURSO = GS.CURSO
                    inner join LY_TURMA TU ON 
                        TU.TURMA = GS.GRADE AND
                        TU.ANO = GS.ANO AND 
                        TU.SEMESTRE = GS.SEMESTRE AND 
						TU.CURRICULO = GS.CURRICULO AND
						TU.CURSO = GS.CURSO AND
					    TU.TURNO = GS.TURNO AND
						TU.SERIE = GS.SERIE AND
                        TU.SIT_TURMA <> 'Desativada'
                WHERE
                    TU.ESPECIAL <> 'S' 
                    AND GS.ANO = ? ");

            if (!string.IsNullOrEmpty(semestre) && semestre != "-1")
                strConsulta.Append(String.Format(" AND GS.SEMESTRE = {0} ", semestre));

            if (!string.IsNullOrEmpty(nucleo))
                strConsulta.Append(String.Format(" AND UE.NUCLEO = {0} ", nucleo));

            if (!string.IsNullOrEmpty(municipio))
                strConsulta.Append(String.Format(" AND UE.MUNICIPIO = {0}", municipio));

            if (!string.IsNullOrEmpty(unidade_responsavel))
                strConsulta.Append(String.Format(" AND GS.UNIDADE_RESPONSAVEL = {0}", unidade_responsavel));

            if (!string.IsNullOrEmpty(turma))
                strConsulta.Append(String.Format(" AND GS.GRADE_ID = {0}", turma));

            strConsulta.Append(" ORDER BY GS.GRADE ");

            return RNBase.Consultar(strConsulta.ToString(), ano);
        }

        public static QueryTable ConsultarTurmaNotas(string ano, string semestre, string nucleo, string municipio, string unidade_responsavel, string turma)
        {
            StringBuilder strConsulta = new StringBuilder();
            strConsulta.Append(@" SELECT DISTINCT 
                    GS.grade_id, 
                    GS.curso, 
                    C.NOME nomeCurso, 
                    GS.turno, 
                    T.DESCRICAO descricaoTurno,
                    GS.serie, 
                    S.DESCRICAO descricaoSerie,
                    GS.ano,
                    GS.semestre,
                    GS.grade,
                    GS.capacidade,
                    GS.unidade_responsavel,
                    UE.NOME_COMP nomeUnidadeResponsavel,
                    GS.dependencia,
                    GS.curriculo,
                    GS.faculdade,
                    (select
		                case TU.em_elaboracao
			            when 'S' then 'Horário incompleto'
			            when 'N' then 'Horário completo'
			            else 'Sem alocação' end) em_elaboracao,
                    substring(gs.GRADE, 0, 1 + LEN(GS.GRADE) - CHARINDEX('-', REVERSE(GS.GRADE))) grade_token,
                    TU.turma_integracao as sufixo,
                    tu.sit_turma
                FROM
                    LY_GRADE_SERIE  GS
                    inner join ly_unidade_ensino UE ON
                        UE.unidade_ens = GS.faculdade
                        and UE.UNIDADE_ENS = GS.UNIDADE_RESPONSAVEL
                    inner join ly_nucleo N ON
                        N.nucleo = UE.nucleo
                    inner join ly_turno T ON
                        T.turno = GS.turno
                    inner join ly_serie S ON 
                        S.SERIE = GS.SERIE AND 
                        S.TURNO = GS.TURNO AND 
                        S.CURRICULO = GS.CURRICULO AND
						S.CURSO = GS.CURSO
                    inner join LY_CURSO C ON 
                        C.CURSO = GS.CURSO
                    inner join LY_TURMA TU ON 
                        TU.TURMA = GS.GRADE AND
                        TU.ANO = GS.ANO AND 
                        TU.SEMESTRE = GS.SEMESTRE AND 
						TU.CURRICULO = GS.CURRICULO AND
						TU.CURSO = GS.CURSO AND
					    TU.TURNO = GS.TURNO AND
						TU.SERIE = GS.SERIE AND
                        TU.SIT_TURMA <> 'Desativada'
                WHERE
                    TU.ESPECIAL <> 'S' 
                    AND GS.ANO = ? 
                    ");

            //AND (exists  ( SELECT top 1 1
            //                                FROM ly_aula_docente ad (NOLOCK)
            //                                WHERE tu.turma = ad.turma AND 
            //                                      tu.ano = ad.ano AND 
            //                                      tu.semestre = ad.semestre AND 
            //                                      tu.disciplina = ad.disciplina AND 
            //                                      tu.dt_fim = ad.data_fim
            //                                 and ad.NUM_FUNC in (115451,115453,115455,123782,115458,115797,115459,115460)
            //        )
            //         OR exists (SELECT  top 1 1
            //              FROM     ly_prova p1 (NOLOCK)      
            //              WHERE    p1.ano = ?
            //               AND p1.semestre = ?
            //               AND p1.COMPLEMENTO  = 'N'
            //               and p1.SUBPERIODO <= (select MIN(subperiodo) from LY_SUBPERIODO_LETIVO su 
            //               where su.DT_INICIO <= CONVERT(date, GETDATE()) and su.DT_LANCAMENTO >= CONVERT(date, GETDATE()))
            //          and  tu.ANO = p1.ANO 
            //                and tu.DISCIPLINA =p1.DISCIPLINA 
            //                and p1.TURMA =tu.TURMA 
            //                and p1.SEMESTRE = tu.SEMESTRE 
            //                 ))

            if (!string.IsNullOrEmpty(semestre) && semestre != "-1")
                strConsulta.Append(String.Format(" AND GS.SEMESTRE = {0} ", semestre));

            if (!string.IsNullOrEmpty(nucleo))
                strConsulta.Append(String.Format(" AND UE.NUCLEO = {0} ", nucleo));

            if (!string.IsNullOrEmpty(municipio))
                strConsulta.Append(String.Format(" AND UE.MUNICIPIO = {0}", municipio));

            if (!string.IsNullOrEmpty(unidade_responsavel))
                strConsulta.Append(String.Format(" AND GS.UNIDADE_RESPONSAVEL = {0}", unidade_responsavel));

            if (!string.IsNullOrEmpty(turma))
                strConsulta.Append(String.Format(" AND GS.GRADE_ID = {0}", turma));

            strConsulta.Append(" ORDER BY GS.GRADE ");

            return RNBase.Consultar(strConsulta.ToString(), ano);//,ano,semestre);
        }


        public static QueryTable ConsultarTurmaFinalizadaHistorico(string ano, string semestre, string nucleo, string municipio, string unidade_responsavel, string turma)
        {
            StringBuilder strConsulta = new StringBuilder();
            strConsulta.Append(@" SELECT DISTINCT 
                    GS.grade_id, 
                    GS.curso, 
                    C.NOME nomeCurso, 
                    GS.turno, 
                    T.DESCRICAO descricaoTurno,
                    GS.serie, 
                    S.DESCRICAO descricaoSerie,
                    GS.ano,
                    GS.semestre,
                    GS.grade,
                    GS.capacidade,
                    GS.unidade_responsavel,
                    UE.NOME_COMP nomeUnidadeResponsavel,
                    GS.dependencia,
                    GS.curriculo,
                    GS.faculdade,
                    (select
		                case TU.em_elaboracao
			            when 'S' then 'Horário incompleto'
			            when 'N' then 'Horário completo'
			            else 'Sem alocação' end) em_elaboracao,
                    substring(gs.GRADE, 0, 1 + LEN(GS.GRADE) - CHARINDEX('-', REVERSE(GS.GRADE))) grade_token,
                    TU.turma_integracao as sufixo,
                    tu.sit_turma
                FROM
                    LY_GRADE_SERIE  GS
                    inner join ly_unidade_ensino UE ON
                        UE.unidade_ens = GS.faculdade
                        and UE.UNIDADE_ENS = GS.UNIDADE_RESPONSAVEL
                    inner join ly_nucleo N ON
                        N.nucleo = UE.nucleo
                    inner join ly_turno T ON
                        T.turno = GS.turno
                    inner join ly_serie S ON 
                        S.SERIE = GS.SERIE AND 
                        S.TURNO = GS.TURNO AND 
                        S.CURRICULO = GS.CURRICULO AND
						S.CURSO = GS.CURSO
                    inner join LY_CURSO C ON 
                        C.CURSO = GS.CURSO
                    inner join LY_TURMA TU ON 
                        TU.TURMA = GS.GRADE AND
                        TU.ANO = GS.ANO AND 
                        TU.SEMESTRE = GS.SEMESTRE AND 
						TU.CURRICULO = GS.CURRICULO AND
						TU.CURSO = GS.CURSO AND
					    TU.TURNO = GS.TURNO AND
						TU.SERIE = GS.SERIE                        
                WHERE
                    EXISTS (                                                        
						SELECT TOP 1 1 FROM ly_histmatricula hm (NOLOCK)
						WHERE hm.disciplina = TU.DISCIPLINA AND
						hm.TURMA = TU.TURMA and
						hm.ANO = TU.ANO and
						hm.SEMESTRE = TU.SEMESTRE                        
                    ) AND
                    TU.ESPECIAL <> 'S' 
                    AND tu.sit_turma = 'Finalizada'
                    AND GS.ANO = ? ");

            if (!string.IsNullOrEmpty(semestre) && semestre != "-1")
                strConsulta.Append(String.Format(" AND GS.SEMESTRE = {0} ", semestre));

            if (!string.IsNullOrEmpty(nucleo))
                strConsulta.Append(String.Format(" AND US.NUCLEO = {0} ", nucleo));

            if (!string.IsNullOrEmpty(municipio))
                strConsulta.Append(String.Format(" AND US.MUNICIPIO = {0}", municipio));

            if (!string.IsNullOrEmpty(unidade_responsavel))
                strConsulta.Append(String.Format(" AND GS.UNIDADE_RESPONSAVEL = {0}", unidade_responsavel));

            if (!string.IsNullOrEmpty(turma))
                strConsulta.Append(String.Format(" AND GS.GRADE_ID = {0}", turma));

            strConsulta.Append(" ORDER BY GS.GRADE ");

            return RNBase.Consultar(180, strConsulta.ToString(), ano);
        }

        public static QueryTable ConsultarTurma(string ano, string semestre, string nucleo, string municipio, string unidade_responsavel, string turma, string sit_turma, string tipoTurma)
        {
            var strConsulta = new StringBuilder();

            strConsulta.Append(@"
                SELECT DISTINCT
                        GS.grade_id,
                        GS.curso,
                        C.NOME nomeCurso,
                        GS.turno,
                        T.DESCRICAO descricaoTurno,
                        GS.serie,
                        S.DESCRICAO descricaoSerie,
                        GS.ano,
                        GS.semestre,
                        GS.grade,
                        GS.capacidade,
                        DP.NUM_ALUNOS AS capacidadeSala,
                        GS.unidade_responsavel,
                        UE.NOME_COMP nomeUnidadeResponsavel,
                        GS.dependencia,
                        GS.curriculo,
                        GS.faculdade,
                        (
                          SELECT    CASE TU.em_elaboracao
                                      WHEN 'S' THEN 'Horário incompleto'
                                      WHEN 'N' THEN 'Horário completo'
                                      ELSE 'Sem alocação'
                                    END
                        ) em_elaboracao,
                        SUBSTRING(gs.GRADE, 0, 1 + LEN(GS.GRADE) - CHARINDEX('-', REVERSE(GS.GRADE))) grade_token,
                        TU.turma_integracao AS sufixo,
                        tu.tipo_gestao,
                        tu.sit_turma,
                        tu.num_alunos,
	                    CASE 
							WHEN OPTATIVAREFORCO = 'R' and isnull(ELETIVA,'N') = 'N' THEN 'Ensino Religioso'
							WHEN OPTATIVAREFORCO = 'L' and isnull(ELETIVA,'N') = 'N' THEN 'Lingua Estrangeira Optativa'
							when OPTATIVAREFORCO = 'S' and isnull(ELETIVA,'N') = 'N' THEN 'Optativa'
							when ELETIVA = 'S' and tu.TURMAREFERENCIA is not null THEN 'Eletiva' 
						END tipo_alternativa,
						tu.turmareferencia,
						(SELECT  COUNT(DISTINCT aluno)
                        FROM    dbo.LY_MATRICULA m2  ( NOLOCK )
								inner join LY_TURMA t2  ( NOLOCK ) on m2.TURMA = t2.TURMA
													and m2.SEMESTRE = t2.SEMESTRE
													and m2.ANO = t2.ANO 
													and m2.DISCIPLINA = t2.DISCIPLINA
                        WHERE   m2.turma = TU.TURMA
                                AND m2.ano = TU.ANO
                                AND m2.SEMESTRE = TU.SEMESTRE
                                AND m2.SIT_MATRICULA = 'Matriculado'
                                AND ( m2.DEPENDENCIA <> 'S'
                                    OR m2.DEPENDENCIA IS NULL )
                                AND ( t2.ELETIVA <> 'S'
                                    OR t2.ELETIVA IS NULL )) AS matriculadosprincipal,
						(SELECT  COUNT(DISTINCT ALUNO)
                        FROM    DBO.LY_MATRICULA M2 ( NOLOCK )
								INNER JOIN LY_TURMA T2  ( NOLOCK ) ON M2.TURMA = T2.TURMA
													AND M2.SEMESTRE = T2.SEMESTRE
													AND M2.ANO = T2.ANO 
													AND M2.DISCIPLINA = T2.DISCIPLINA
                        WHERE   M2.TURMA = TU.TURMA
                                AND M2.ANO = TU.ANO
                                AND M2.SEMESTRE = TU.SEMESTRE
                                AND M2.SIT_MATRICULA = 'Matriculado'
                                AND t2.ELETIVA = 'S') as matriculadoseletivas
                FROM    LY_GRADE_SERIE GS ( NOLOCK )
                        INNER JOIN ly_unidade_ensino UE ( NOLOCK ) ON UE.unidade_ens = GS.faculdade
                                                                      AND UE.UNIDADE_ENS = GS.UNIDADE_RESPONSAVEL
                        INNER JOIN ly_turno T ( NOLOCK ) ON T.turno = GS.turno
                        
                        INNER JOIN ly_serie S ( NOLOCK ) ON S.SERIE = GS.SERIE
                                                            AND S.TURNO = GS.TURNO
                                                            AND S.CURRICULO = GS.CURRICULO
                                                            AND S.CURSO = GS.CURSO
                        INNER JOIN LY_CURSO C ( NOLOCK ) ON C.CURSO = GS.CURSO
                        INNER JOIN LY_TURMA TU ( NOLOCK ) ON TU.TURMA = GS.GRADE
                                                             AND TU.ANO = GS.ANO
                                                             AND TU.SEMESTRE = GS.SEMESTRE
                                                             AND TU.CURRICULO = GS.CURRICULO
                                                             AND TU.CURSO = GS.CURSO
                                                             AND TU.TURNO = GS.TURNO
                                                             AND TU.SERIE = GS.SERIE
                                                             AND TU.SIT_TURMA = ?
                       INNER JOIN LY_DEPENDENCIA DP  ON DP.FACULDADE = TU.FACULDADE  AND DP.DEPENDENCIA = TU.DEPENDENCIA 
                WHERE   TU.ESPECIAL <> 'S'
                        AND GS.ANO = ?");

            if (!string.IsNullOrEmpty(semestre) && semestre != "-1")
                strConsulta.Append(String.Format(" AND GS.SEMESTRE = {0} ", semestre));

            if (!string.IsNullOrEmpty(nucleo))
                strConsulta.Append(String.Format(" AND UE.NUCLEO = {0} ", nucleo));

            if (!string.IsNullOrEmpty(municipio))
                strConsulta.Append(String.Format(" AND UE.MUNICIPIO = {0}", municipio));

            if (!string.IsNullOrEmpty(unidade_responsavel))
                strConsulta.Append(String.Format(" AND GS.UNIDADE_RESPONSAVEL = {0}", unidade_responsavel));

            if (!string.IsNullOrEmpty(turma))
                strConsulta.Append(String.Format(" AND GS.GRADE_ID = {0}", turma));

            if (tipoTurma == "Principais")
            {
                strConsulta.Append(" AND C.TIPO NOT IN (5,7)");
            }
            else
            {
                strConsulta.Append(" AND C.TIPO IN (5,7)");
            }

            strConsulta.Append(" ORDER BY GS.GRADE ");

            return Consultar(strConsulta.ToString(), sit_turma, ano);
        }

        public static DataTable ConsultarTurmasGreve(string ano, string semestre, string unidade, string datainicio, string datafim)
        {
            var dataTable = new DataTable();

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = @"SET DATEFORMAT DMY
                            SELECT DISTINCT 
                                GS.grade_id, 
                                GS.curso, 
                                C.NOME nomeCurso, 
                                GS.turno, 
                                T.DESCRICAO descricaoTurno,
                                GS.serie, 
                                S.DESCRICAO descricaoSerie,
                                GS.ano,
                                GS.semestre,
                                GS.grade,
                                GS.capacidade,
                                GS.unidade_responsavel,
                                UE.NOME_COMP nomeUnidadeResponsavel,
                                GS.dependencia,
                                GS.curriculo,
                                GS.faculdade,
                                (select
		                            case TU.em_elaboracao
			                        when 'S' then 'Horário incompleto'
			                        when 'N' then 'Horário completo'
			                        else 'Sem alocação' end) em_elaboracao,
                                substring(gs.GRADE, 0, 1 + LEN(GS.GRADE) - CHARINDEX('-', REVERSE(GS.GRADE))) grade_token,
                                TU.turma_integracao as sufixo,
                                tu.tipo_gestao,
                                tu.sit_turma
                            FROM
                                LY_GRADE_SERIE  GS (NOLOCK)
                                inner join ly_unidade_ensino UE (NOLOCK) ON
                                    UE.unidade_ens = GS.faculdade
                                    AND UE.UNIDADE_ENS = GS.UNIDADE_RESPONSAVEL
                                inner join ly_nucleo N (NOLOCK) ON
                                    N.nucleo = UE.nucleo
                                inner join ly_turno T (NOLOCK) ON
                                    T.turno = GS.turno
                                inner join ly_serie S (NOLOCK) ON 
                                    S.SERIE = GS.SERIE AND 
                                    S.TURNO = GS.TURNO AND 
                                    S.CURRICULO = GS.CURRICULO AND
						            S.CURSO = GS.CURSO
                                inner join LY_CURSO C (NOLOCK) ON 
                                    C.CURSO = GS.CURSO
                                inner join LY_TURMA TU (NOLOCK) ON 
                                    TU.TURMA = GS.GRADE AND
                                    TU.ANO = GS.ANO AND 
                                    TU.SEMESTRE = GS.SEMESTRE AND 
						            TU.CURRICULO = GS.CURRICULO AND
						            TU.CURSO = GS.CURSO AND
					                TU.TURNO = GS.TURNO AND
						            TU.SERIE = GS.SERIE --AND
                                   -- TU.SIT_TURMA = 'Aberta'
                                WHERE
                                TU.ESPECIAL <> 'S' AND
                                GS.ANO = @ano 
                                AND GS.SEMESTRE = @semestre
                                AND GS.UNIDADE_RESPONSAVEL = @unidade 
                                AND EXISTS (SELECT DISTINCT TURMA 
                                                FROM LY_AULA_DOCENTE AD
                                                INNER JOIN LY_LICENCA_DOCENTE LD ON AD.NUM_FUNC = LD.NUM_FUNC 
                                                WHERE MOTIVO in ('61','61A')
                                                AND ANO = @ano 
                                                AND SEMESTRE = @semestre
                                                AND FACULDADE = @unidade
                                                and AD.TURMA = TU.TURMA 
                                                and AD.ANO = TU.ANO 
                                                and AD.SEMESTRE = TU.SEMESTRE 
                                                and AD.TURNO = TU.TURNO 
												and AD.FACULDADE = TU.UNIDADE_RESPONSAVEL )
                                                --and ad.DATA_FIM >= GETDATE())
--												and convert(varchar, DTINI, 103) >= @datainicio
--												and convert(varchar, DTINI, 103) <= @datafim 
 
                                ORDER BY GS.GRADE ";

                contextQuery.Parameters.Add("@ano", ano);
                contextQuery.Parameters.Add("@semestre", semestre);
                contextQuery.Parameters.Add("@unidade", unidade);
                contextQuery.Parameters.Add("@datainicio", datainicio);
                contextQuery.Parameters.Add("@datafim", datafim);

                dataTable = ctx.GetDataTable(contextQuery);
            }

            return dataTable;
        }

        public DataTable ObtemConsultaTurmasParaFechamentoMatriculaPor(string ano, string semestre, string curso, string turno, string unidadeResponsavel, string turma)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable turmas = null;
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(@" SELECT DISTINCT
                            GS.grade_id ,
                            GS.curso ,
                            C.NOME nomeCurso ,
                            GS.turno ,
                            T.DESCRICAO descricaoTurno ,
                            GS.serie ,
                            S.DESCRICAO descricaoSerie ,
                            GS.ano ,
                            GS.semestre ,
                            GS.grade ,
                            GS.capacidade ,
                            GS.unidade_responsavel ,
                            UE.NOME_COMP nomeUnidadeResponsavel ,
                            GS.dependencia ,
                            GS.curriculo ,
                            GS.faculdade ,
                            TU.sit_turma ,
                            TU.DT_INICIO,
                            TU.DT_FIM,
                            TU.optativareforco,
                           CASE WHEN (GS.CURSO = '0092.30' AND GS.SERIE = 4) THEN 'S'
								 ELSE  S.ANO_SERIE_CONCLUINTE
							END ANO_SERIE_CONCLUINTE,
                            UE.ID_REGIONAL,
                            CASE
								WHEN ISNULL(TU.ELETIVA,'N') = 'S' AND TU.TURMAREFERENCIA IS NOT NULL THEN 'S'
								ELSE 'N'
                            END ELETIVA
                    FROM    LY_GRADE_SERIE GS
                            INNER JOIN ly_turno T ON T.turno = GS.turno
                            INNER JOIN ly_unidade_ensino UE ON UE.UNIDADE_ENS = GS.UNIDADE_RESPONSAVEL
                                                               AND UE.UNIDADE_ENS = GS.FACULDADE
                            INNER JOIN ly_serie S ON S.SERIE = GS.SERIE
                                                     AND S.TURNO = GS.TURNO
                                                     AND S.CURRICULO = GS.CURRICULO
                                                     AND S.CURSO = GS.CURSO
                            INNER JOIN LY_CURSO C ON C.CURSO = GS.CURSO
                            INNER JOIN LY_TURMA TU ON TU.TURMA = GS.GRADE
                                                      AND TU.ANO = GS.ANO
                                                      AND TU.SEMESTRE = GS.SEMESTRE
                    WHERE   GS.ANO = @ANO
                            AND FECHAMENTO_MANUAL = 'S'
                            AND GS.SEMESTRE = @SEMESTRE
                            AND SIT_TURMA <> 'Desativada' ");

                if (!string.IsNullOrEmpty(curso))
                {
                    sql.Append(" AND c.curso = @CURSO ");
                    contextQuery.Parameters.Add("@CURSO", curso);
                }


                if (!string.IsNullOrEmpty(turno))
                {
                    sql.Append(" AND t.turno = @TURNO ");
                    contextQuery.Parameters.Add("@TURNO", turno);
                }

                if (!string.IsNullOrEmpty(unidadeResponsavel))
                {
                    sql.Append(" AND GS.UNIDADE_RESPONSAVEL = @UNIDADE_RESPONSAVEL ");
                    contextQuery.Parameters.Add("@UNIDADE_RESPONSAVEL", unidadeResponsavel);
                }

                if (!string.IsNullOrEmpty(turma))
                {
                    sql.Append(" AND GS.GRADE = @GRADE ");
                    contextQuery.Parameters.Add("@GRADE", turma);
                }

                sql.Append(" ORDER BY GS.GRADE ");

                contextQuery.Command = sql.ToString();

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);

                turmas = ctx.GetDataTable(contextQuery);
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return turmas;
        }

        public static QueryTable ConsultarGrade(string ano, string semestre, string nucleo, string municipio, string unidade_responsavel)
        {
            System.Text.StringBuilder strConsulta = new System.Text.StringBuilder();
            strConsulta.Append(" SELECT DISTINCT GS.grade_id as grade_id, GS.grade as grade ");
            strConsulta.Append(" FROM LY_GRADE_SERIE  GS ");
            strConsulta.Append(" inner join ly_unidade_ensino US ON US.unidade_ens = GS.faculdade ");
            strConsulta.Append(" inner join ly_nucleo N ON N.nucleo = US.nucleo  ");
            strConsulta.Append(" inner join ly_turno T ON T.turno = GS.turno ");
            strConsulta.Append(" inner join ly_unidade_ensino UE ON UE.UNIDADE_ENS = GS.UNIDADE_RESPONSAVEL ");
            strConsulta.Append(" inner join ly_serie S ON S.SERIE = GS.SERIE AND S.TURNO = GS.TURNO AND S.CURRICULO = GS.CURRICULO ");
            strConsulta.Append(" inner join LY_CURSO C ON C.CURSO = GS.CURSO ");
            strConsulta.Append(" inner join LY_TURMA TU ON TU.TURMA =  GS.GRADE AND TU.ANO = GS.ANO AND TU.SEMESTRE = GS.SEMESTRE ");
            strConsulta.Append(" AND TU.SIT_TURMA <> 'Desativada' ");
            strConsulta.Append(" WHERE ");
            strConsulta.Append(" GS.ANO = ? AND GS.SEMESTRE = ? AND US.NUCLEO = ? ");

            if (!string.IsNullOrEmpty(municipio))
                strConsulta.Append(String.Format(" AND US.MUNICIPIO = {0}", municipio));

            if (!string.IsNullOrEmpty(unidade_responsavel))
                strConsulta.Append(String.Format(" AND GS.UNIDADE_RESPONSAVEL = {0}", unidade_responsavel));

            strConsulta.Append(" ORDER BY GS.GRADE ");

            ParametrosCurso oPC = new ParametrosCurso { ano = ano, semestre = semestre, nucleo = nucleo };

            return ExecutarConsulta(strConsulta, oPC);
        }

        public static QueryTable ConsultarDependencia(string ano, string semestre, string nucleo, string municipio, string unidade_responsavel, string turma)
        {
            System.Text.StringBuilder strConsulta = new System.Text.StringBuilder();
            strConsulta.Append(" SELECT DISTINCT GS.grade_id, GS.curso, C.NOME nomeCurso , GS.turno, T.DESCRICAO descricaoTurno, ");
            strConsulta.Append(" GS.serie, S.DESCRICAO descricaoSerie , GS.ano, GS.semestre, GS.grade, ");
            strConsulta.Append(" GS.capacidade, GS.unidade_responsavel, UE.NOME_COMP nomeUnidadeResponsavel, ");
            strConsulta.Append(" GS.dependencia, GS.curriculo, GS.faculdade, TU.em_elaboracao ");
            strConsulta.Append(" FROM LY_GRADE_SERIE  GS ");
            strConsulta.Append(" inner join ly_unidade_ensino US ON US.unidade_ens = GS.faculdade ");
            strConsulta.Append(" inner join ly_nucleo N ON N.nucleo = US.nucleo  ");
            strConsulta.Append(" inner join ly_turno T ON T.turno = GS.turno ");
            strConsulta.Append(" inner join ly_unidade_ensino UE ON UE.UNIDADE_ENS = GS.UNIDADE_RESPONSAVEL ");
            strConsulta.Append(" inner join ly_serie S ON S.SERIE = GS.SERIE AND S.TURNO = GS.TURNO AND S.CURRICULO = GS.CURRICULO ");
            strConsulta.Append(" inner join LY_CURSO C ON C.CURSO = GS.CURSO ");
            strConsulta.Append(" inner join LY_TURMA TU ON TU.TURMA =  GS.GRADE AND TU.ANO = GS.ANO AND TU.SEMESTRE = GS.SEMESTRE ");
            strConsulta.Append(" AND TU.SIT_TURMA <> 'Desativada' ");
            strConsulta.Append(" WHERE ");
            strConsulta.Append(" GS.ANO = ? AND GS.SEMESTRE = ? AND US.NUCLEO = ? AND TU.ESPECIAL='S'");

            if (!string.IsNullOrEmpty(municipio))
                strConsulta.Append(String.Format(" AND US.MUNICIPIO = {0}", municipio));

            if (!string.IsNullOrEmpty(unidade_responsavel))
                strConsulta.Append(String.Format(" AND GS.UNIDADE_RESPONSAVEL = {0}", unidade_responsavel));

            if (!string.IsNullOrEmpty(turma))
                strConsulta.Append(String.Format(" AND GS.GRADE_ID = {0}", turma));

            strConsulta.Append(" ORDER BY GS.GRADE ");

            ParametrosCurso oPC = new ParametrosCurso { ano = ano, semestre = semestre, nucleo = nucleo };

            return ExecutarConsulta(strConsulta, oPC);
        }

        public QueryTable ConsultarGrade(DbObject curso, string turno, string curriculo, string ano, string periodo, string serie, DbObject unidadeEns)
        {
            return Consultar("select GRADE + '|' + convert(varchar,GRADE_ID) valor, grade turma  from ly_grade_serie where CURSO = ? and TURNO = ? and CURRICULO = ? and convert(varchar,SERIE) = ? and ANO = ? and SEMESTRE = ? and UNIDADE_RESPONSAVEL = ?", curso, turno, curriculo, serie, ano, periodo, unidadeEns);
        }

        public string ObtemTurmaPor(int gradeId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            string turma = string.Empty;

            try
            {

                contextQuery.Command = @" SELECT  GS.GRADE
                                FROM    LY_GRADE_SERIE GS (NOLOCK)
                                WHERE   GRADE_ID = @GRADE_ID ";

                contextQuery.Parameters.Add("@GRADE_ID", gradeId);

                turma = ctx.GetReturnValue<string>(contextQuery);

                return turma;
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

        public static DataTable ConsultarGrade(Decimal grade_id)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"select DISTINCT UE.NOME_COMP,GS.GRADE, GS.ANO, GS.SEMESTRE, C.TITULO, GS.CURRICULO,T.DESCRICAO AS 'NOME_TURNO',GS.SERIE  
                        from LY_GRADE_SERIE GS  
                        INNER JOIN LY_UNIDADE_ENSINO UE ON UE.UNIDADE_ENS = GS.UNIDADE_RESPONSAVEL 
                        INNER JOIN LY_CURSO C ON C.CURSO = GS.CURSO 
                        INNER JOIN LY_TURNO T ON T.TURNO = GS.TURNO 
                        WHERE GRADE_ID = @GRADE_ID ");

                contextQuery.Parameters.Add("@GRADE_ID", grade_id);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static string ObterGradeId(decimal ano, decimal periodo, string curso, string curriculo, decimal serie, string turma)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                return ObterGradeId(ctx, ano, periodo, curso, curriculo, serie, turma);
            }
        }

        public static string ObterGradeId(DataContext ctx, decimal ano, decimal periodo, string curso, string curriculo, decimal serie, string turma)
        {
            return ctx.GetReturnValue<string>(
                new ContextQuery(
                    @"SELECT TOP 1
                             grade_id
                        FROM LY_GRADE_SERIE
                       WHERE ANO = @ANO
                             AND SEMESTRE = @PERIODO
                             AND CURSO = @CURSO
                             --AND CURRICULO = @CURRICULO
                             AND SERIE = @SERIE
                             AND GRADE = @GRADE",
                    new ContextQueryParameter("@ANO", TechneDbType.T_ANO, ano),
                    new ContextQueryParameter("@PERIODO", TechneDbType.T_SEMESTRE2, periodo),
                    new ContextQueryParameter("@CURSO", TechneDbType.T_CODIGO, curso),
                    new ContextQueryParameter("@CURRICULO", TechneDbType.T_CODIGO, curriculo),
                    new ContextQueryParameter("@SERIE", TechneDbType.T_NUMERO_PEQUENO, serie),
                    new ContextQueryParameter("@GRADE", TechneDbType.T_CODIGO, turma)));
        }

        public static QueryTable ConsultarTurmaMatricula(string ano, string semestre, string curso, string turno, string unidade_responsavel, string turma)
        {
            System.Text.StringBuilder strConsulta = new System.Text.StringBuilder();
            strConsulta.Append(@"SELECT DISTINCT 
                            GS.grade_id, 
                            GS.curso, 
                            C.NOME nomeCurso , 
                            GS.turno, 
                            T.DESCRICAO descricaoTurno,
                            GS.serie, 
                            S.DESCRICAO descricaoSerie , 
                            GS.ano, 
                            GS.semestre, 
                            GS.grade,
                            GS.capacidade, 
                            GS.unidade_responsavel, 
                            UE.NOME_COMP nomeUnidadeResponsavel,
                            GS.dependencia, 
                            GS.curriculo, 
                            GS.faculdade,
                            TU.sit_turma,
                            TU.optativareforco,
                            S.ano_serie_concluinte,
                            CASE 
								WHEN ISNULL(TU.ELETIVA,'N') = 'S' AND TU.TURMAREFERENCIA IS NOT NULL THEN 'S' 
								ELSE 'N'
							END eletiva
                            FROM LY_GRADE_SERIE  GS 
                            inner join ly_turno T ON T.turno = GS.turno 
                            inner join ly_unidade_ensino UE ON UE.UNIDADE_ENS = GS.UNIDADE_RESPONSAVEL and UE.UNIDADE_ENS = GS.FACULDADE
                            inner join ly_serie S ON S.SERIE = GS.SERIE AND S.TURNO = GS.TURNO AND S.CURRICULO = GS.CURRICULO AND S.CURSO = GS.CURSO
                            inner join LY_CURSO C ON C.CURSO = GS.CURSO
                            inner join LY_TURMA TU ON TU.TURMA =  GS.GRADE AND TU.ANO = GS.ANO AND TU.SEMESTRE = GS.SEMESTRE 
                            WHERE 
                            GS.ANO = ? 
                            AND FECHAMENTO_MANUAL = 'S'
                            AND GS.SEMESTRE = ? 
                            AND SIT_TURMA <> 'Desativada'");

            if (!string.IsNullOrEmpty(curso))
                strConsulta.Append(String.Format(" AND c.curso = '{0}' ", curso));

            if (!string.IsNullOrEmpty(turno))
                strConsulta.Append(String.Format(" AND t.turno = '{0}'", turno));

            if (!string.IsNullOrEmpty(unidade_responsavel))
                strConsulta.Append(String.Format(" AND GS.UNIDADE_RESPONSAVEL = '{0}'", unidade_responsavel));

            if (!string.IsNullOrEmpty(turma))
                strConsulta.Append(String.Format(" AND GS.GRADE = '{0}'", turma));

            strConsulta.Append(" ORDER BY GS.GRADE ");

            return RNBase.Consultar(strConsulta.ToString(), ano, semestre);
        }

        public string ObtemGradeIdPor(decimal ano, decimal periodo, string curso,decimal serie, string turma)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            string grade_id = string.Empty;

            try
            {

                contextQuery.Command = @" SELECT TOP 1
                             grade_id
                        FROM LY_GRADE_SERIE
                       WHERE ANO = @ANO
                             AND SEMESTRE = @PERIODO
                             AND CURSO = @CURSO      
                             AND SERIE = @SERIE
                             AND GRADE = @GRADE ";

                    contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
                    contextQuery.Parameters.Add("@PERIODO", TechneDbType.T_SEMESTRE2, periodo);
                    contextQuery.Parameters.Add("@CURSO", TechneDbType.T_CODIGO, curso);
                    contextQuery.Parameters.Add("@SERIE", TechneDbType.T_NUMERO_PEQUENO, serie);
                    contextQuery.Parameters.Add("@GRADE", TechneDbType.T_CODIGO, turma);

                    grade_id = ctx.GetReturnValue<string>(contextQuery);

                    return grade_id;
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

        public bool EhSerieConcluinte(decimal ano, decimal periodo, string turma)
        {
            bool ehSerieConcluinte;

            ContextQuery contextQuery = new ContextQuery(
                     @" SELECT   COUNT(*)
                         FROM    LY_GRADE_SERIE GS                           
                            INNER JOIN ly_serie S ON S.SERIE = GS.SERIE
                                                     AND S.TURNO = GS.TURNO
                                                     AND S.CURRICULO = GS.CURRICULO
                                                     AND S.CURSO = GS.CURSO
                        WHERE   GS.ANO = @ANO
                                AND GS.SEMESTRE = @SEMESTRE                               
                                AND GS.GRADE = @TURMA
                                AND (CASE WHEN GS.SERIE = 9 or (GS.CURSO = '0092.30' AND GS.SERIE = 4) THEN 'S'
										 ELSE  S.ANO_SERIE_CONCLUINTE
									END) = 'S'
                                ");


            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", periodo);
            contextQuery.Parameters.Add("@TURMA", turma);

            ehSerieConcluinte = (ExecutarFuncao<int>(contextQuery) > 0);

            return ehSerieConcluinte;
        }
    }
}
