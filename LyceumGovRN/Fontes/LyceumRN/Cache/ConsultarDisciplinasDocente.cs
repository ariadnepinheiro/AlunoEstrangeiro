namespace Techne.Lyceum.RN.Cache
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Web;
    using Seeduc.Infra.Data;

    public static class ConsultarDisciplinasDocente
    {
        private const string Chave = "__CACHE_CONSULTAR_DISCIPLINAS_DOCENTE__";

        public static void Limpar()
        {
            var current = HttpContext.Current;

            current.Session.Remove(Chave);
        }

        public static DataTable Consultar(string matricula)
        {
            if (string.IsNullOrEmpty(matricula))
            {
                return null;
            }

            var usarCache = Convert.ToBoolean(ConfigurationManager.AppSettings["UsarCache"] ?? "false");
            var current = HttpContext.Current;
            var result = current.Session[Chave] as DataTable;

            if (result == null
                || !usarCache)
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    var contextQuery = new ContextQuery(
@"DECLARE @num_func T_NUMFUNC 
SET @num_func = (
                  SELECT TOP 1
                            num_func
                  FROM      LY_DOCENTE
                  WHERE     MATRICULA = @MATRICULA
                )                 

SELECT  ue.unidade_ens,
        ue.nome_comp,
        t.disciplina AS disciplina,
        t.disciplina + '|' + t.turma + '|' + CONVERT(VARCHAR, t.ano) + '|' + CONVERT(VARCHAR, t.semestre) chave,
        t.turma,
        CONVERT(VARCHAR, t.ano) + '/' + CONVERT(VARCHAR, t.semestre) anosemestre,
        t.ano,
        t.semestre,
        d.nome_compl,
        CASE WHEN t.curso IN ( '0001.11', '0001.16', '0001.17', '0001.51' ) THEN 'N'
             ELSE 'S'
        END validoParaLancamento,
        CASE WHEN (
                    SELECT  COUNT(*)
                    FROM    ly_prova p1 ( NOLOCK )
                    WHERE   p1.ano = t.ano
                            AND p1.semestre = t.semestre
                            AND p1.disciplina = t.disciplina
                            AND p1.turma = t.turma
                            AND p1.subperiodo <= MAX(spl.subperiodo)
                            AND ISNULL(p1.COMPLEMENTO, 'N') = 'N'
                  ) = 0 THEN 'N'
             ELSE 'S'
        END possuinotaspendentes,
        [dbo].[FCE_STATUS_TURMA_BIMESTRES](t.ano, t.semestre, t.turma) AS status_lancamento,
        t.CURSO AS curso,
        cu.modalidade,
        cu.tipo,
        t.SERIE AS serie
FROM    ly_disciplina d ( NOLOCK )
        INNER JOIN ly_turma t ( NOLOCK ) ON ISNULL(t.disciplina_multipla, t.disciplina) = d.disciplina
        INNER JOIN LY_CURSO CU ON CU.CURSO = T.CURSO
        INNER JOIN ly_aula_docente ad ( NOLOCK ) ON t.turma = ad.turma
                                                    AND t.disciplina = ad.disciplina
                                                    AND t.ano = ad.ano
                                                    AND t.semestre = ad.semestre
                                                    AND ad.data_fim = t.dt_fim
        INNER JOIN ly_unidade_ensino ue ( NOLOCK ) ON t.unidade_responsavel = ue.unidade_ens
        INNER JOIN ly_subperiodo_letivo spl ON spl.ano = t.ano
                                               AND spl.periodo = t.semestre
                                               AND CONVERT(DATE, GETDATE()) >= spl.dt_inicio
                                               AND CONVERT(DATE, GETDATE()) <= spl.DT_LANCAMENTO
WHERE   ad.NUM_FUNC = @num_func
        AND t.sit_turma = 'Aberta'
        AND t.ANO = YEAR(GETDATE())
GROUP BY ue.unidade_ens,
        ue.nome_comp,
        t.disciplina,
        t.turma,
        t.ano,
        t.semestre,
        d.nome_compl,
        t.CURSO,
        cu.modalidade,
        cu.tipo,
        t.SERIE
UNION
SELECT DISTINCT
        ue.unidade_ens,
        ue.nome_comp,
        t.disciplina AS disciplina,
        t.disciplina + '|' + t.turma + '|' + CONVERT(VARCHAR, t.ano) + '|' + CONVERT(VARCHAR, t.semestre) chave,
        t.turma,
        CONVERT(VARCHAR, t.ano) + '/' + CONVERT(VARCHAR, t.semestre) anosemestre,
        t.ano,
        t.semestre,
        d.nome_compl,
        CASE WHEN t.curso IN ( '0001.11', '0001.16', '0001.17', '0001.51' ) THEN 'N'
             ELSE 'S'
        END validoParaLancamento,
        CASE WHEN (
                    SELECT  COUNT(*)
                    FROM    ly_prova p1 ( NOLOCK )
                    WHERE   p1.ano = t.ano
                            AND p1.semestre = t.semestre
                            AND p1.disciplina = t.disciplina
                            AND p1.turma = t.turma
                            AND p1.subperiodo <= MAX(spl.subperiodo)
                            AND ISNULL(p1.COMPLEMENTO, 'N') = 'N'
                  ) = 0 THEN 'N'
             ELSE 'S'
        END possuinotaspendentes,
        [dbo].[FCE_STATUS_TURMA_BIMESTRES](t.ano, t.semestre, t.turma) AS status_lancamento,
        t.CURSO AS curso,
        cu.modalidade,
        cu.tipo,
        t.SERIE AS serie
FROM    ly_disciplina d ( NOLOCK )
        INNER JOIN ly_turma t ( NOLOCK ) ON ISNULL(t.disciplina_multipla, t.disciplina) = d.disciplina
        INNER JOIN LY_CURSO CU ON CU.CURSO = T.CURSO
        INNER JOIN ly_unidade_ensino ue ( NOLOCK ) ON t.unidade_responsavel = ue.unidade_ens
        LEFT JOIN ly_subperiodo_letivo spl ON spl.ano = t.ano
                                              AND spl.periodo = t.semestre
                                              AND CONVERT(DATE, GETDATE()) >= spl.dt_inicio
                                              AND CONVERT(DATE, GETDATE()) <= spl.DT_LANCAMENTO
WHERE   t.NUM_FUNC = @num_func
        AND t.sit_turma = 'Aberta'
        AND t.CLASSIFICACAO = 'PROJ'
        AND t.ANO = YEAR(GETDATE())
GROUP BY ue.unidade_ens,
        ue.nome_comp,
        t.disciplina,
        t.turma,
        t.ano,
        t.semestre,
        d.nome_compl,
        t.CURSO,
        cu.modalidade,
        cu.tipo,
        t.SERIE
ORDER BY unidade_ens,
        ano,
        semestre,
        turma,
        disciplina");

                    contextQuery.Parameters.Add("@MATRICULA", matricula);

                    // Não é recomendado, mas está sendo utilizado para minimizar o acesso ao banco de dados
                    current.Session[Chave] = result = ctx.GetDataTable(contextQuery);
                }
            }

            return result;
        }
    }
}
