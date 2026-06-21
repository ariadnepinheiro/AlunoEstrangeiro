namespace ConsoleTests.Context
{
    using System;
    using ConsoleTests.Domain;
    using Seeduc.Infra.Data;
    using Seeduc.Infra.Mapper;

    public static class Attic
    {
        public static void BulkRun()
        {
            using (var ctx = DataContextBuilder.FromHades.UsingLock())
            {
                try
                {
                    ctx.BeginBulkModifications();

                    for (var i = 0; i < 200; i++)
                    {
                        ctx.ApplyModifications(
                            new ContextQuery(
                                @"UPDATE  HADES..HD_USUARIO
                                    SET     EMAIL = @EMAIL
                                    WHERE   USUARIO = 'walbert'",
                                new ContextQueryParameter("@EMAIL", i.ToString())));
                    }

                    ctx.EndBulkModifications();

                    ctx.ApplyModifications(
                        new ContextQuery(
                            @"UPDATE  HADES..HD_USUARIO
                            SET     EMAIL = '300'
                            WHERE   USUARIO = 'walbert'"));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);

                    throw;
                }
            }
        }

        public static void Run()
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    var contador = ctx.GetReturnValue<int>(
                                new ContextQuery(
                                    @"SELECT  NULL
                                    FROM    ly_aluno"));

                    // var contextQuery = new ContextQuery("SELECT TOP 1000 * FROM LY_DOCENTE ORDER BY MATRICULA");
                    var contextQuery = new ContextQuery
                    {
                        Command = @"select DISTINCT t.DISCIPLINA, t.TURMA,
                        d.nome AS nome_disciplina,
                        CONVERT (decimal(10,2), ROUND(CAST(REPLACE(n.conceito, ',', '.') AS real), 2)) AS nota,                        
                        CONVERT (decimal(10,2), ROUND(CAST((100 -((fa.FALTAS/fr.AULAS_DADAS)*100)) AS real), 2)) AS frequencia,  
                        CONVERT (decimal(10,2), (SELECT  ROUND(AVG(CAST(REPLACE(n.conceito, ',', '.') AS real)), 2) AS media
                        FROM    ly_nota n
                                INNER JOIN ly_prova p ON n.DISCIPLINA = p.DISCIPLINA
                                                         AND n.TURMA = p.TURMA
                                                         AND n.ANO = p.ANO
                                                         AND n.SEMESTRE = p.SEMESTRE
                                                         AND n.PROVA = p.PROVA
                        WHERE p.DISCIPLINA = mat.DISCIPLINA
                        AND p.TURMA = mat.TURMA
                        AND p.ano = mat.ANO
                        AND p.SEMESTRE = mat.SEMESTRE
                        AND p.SUBPERIODO = fr.SUBPERIODO
                        AND n.SEM_AVALIACAO = 'N'
                        GROUP BY n.DISCIPLINA, n.TURMA, n.ANO, n.SEMESTRE, n.PROVA)) AS media        
                        FROM ly_matricula mat
                        inner join LY_TURMA t 
	                        on mat.DISCIPLINA = t.DISCIPLINA 
	                        and mat.TURMA = t.TURMA 
	                        and mat.ANO = t.ANO 
	                        and mat.SEMESTRE = t.SEMESTRE
                        join ly_disciplina d 
	                        on  d.disciplina = isnull(t.DISCIPLINA_MULTIPLA, t.DISCIPLINA)
                        inner JOIN LY_FREQ fr
	                        on t.DISCIPLINA = fr.DISCIPLINA
	                        and t.TURMA = fr.TURMA
	                        and t.ANO = fr.ANO
	                        and fr.PERIODO = t.semestre 	
                        INNER JOIN dbo.LY_PROVA pv 
	                        ON pv.SUBPERIODO = fr.SUBPERIODO
                            and pv.DISCIPLINA = t.DISCIPLINA  
                            and pv.TURMA = t.TURMA
                            and pv.ANO = t.ANO 
                            and pv.SEMESTRE = t.SEMESTRE    
                        left JOIN ly_nota n
	                        on  n.aluno = mat.aluno 
	                        AND n.disciplina = t.DISCIPLINA 
	                        AND n.ano = t.ano 
	                        AND n.semestre = t.semestre 
	                        AND n.turma = t.turma	
	                        AND n.prova = pv.PROVA
	                        AND n.SEM_AVALIACAO = 'N'	
                        LEFT JOIN LY_falta fa
	                        ON fa.TURMA = t.TURMA
	                        and fa.ANO = t.ANO
	                        and fa.ALUNO = mat.ALUNO
	                        and fa.DISCIPLINA = t.DISCIPLINA
	                        and fa.PERIODO = t.SEMESTRE
	                        and fa.FREQ = fr.FREQ
                        WHERE  mat.sit_matricula <> 'Cancelado'
                        and isnull(t.ESPECIAL,'N') = 'N'
                        AND mat.aluno = @aluno 
                        AND t.ano = @ano
                        AND t.semestre = @periodo
                        AND fr.SUBPERIODO = @subperido"
                    };
                    contextQuery.Parameters.Add("@aluno", "201100050230955");
                    contextQuery.Parameters.Add("@ano", 2011);
                    contextQuery.Parameters.Add("@periodo", 0);
                    contextQuery.Parameters.Add("@subperido", 2);

                    var dataTable = ctx.GetDataTable(contextQuery);

                    var docentes = DataTableMapper.CreateAndMapTo<LyDocente>(dataTable);

                    foreach (var docente in docentes)
                    {
                        Console.WriteLine("foo");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Seeduc.Infra error: {0}", ex.Message);
            }
        }
    }
}
