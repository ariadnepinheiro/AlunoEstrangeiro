using Seeduc.Infra.Data;

namespace ConsoleTests.Cronos
{
    using System;
    using Techne.Lyceum;
    using Techne.Lyceum.CR;

    public static class Attic
    {
        public static void Run()
        {
            var conn = Config.CreateWritableConnection();

            conn.Open(true);

            try
            {
                //Ly_pessoa.Row.Insert(
                //   conn,
                //   888888888m,
                //   "Foo",
                //   "foo",
                //   "foo",
                //   "foo",
                //   "123",
                //   "N",
                //   "N");

                //Ly_pessoa.Row.Update(
                //   conn,
                //   888888888m,
                //   "mae_falecida, pai_falecido",
                //   "N",
                //   "N");

                //Ly_serie.Row.Update(
                //    conn,
                //    "0000.01",
                //    "A",
                //    "0000.01-2006",
                //    3,
                //    "curso_seguinte, serie_seguinte, ano_serie_concluinte, matricula, dt_cadastro",
                //    null,
                //    null,
                //    null,
                //    null,
                //    null);

                //Ly_grade_serie.Row.Update(
                //    conn,
                //    42031,
                //    "fechamento_manual, matricula, dt_cadastro, dt_alteracao",
                //    null,
                //    null,
                //    null,
                //    null);

                //Ly_curriculo.Row.Update(
                //    conn,
                //    "0000.01",
                //    "A",
                //    "0000.01-2007",
                //    "ensino_religioso, lingua_estrangeira",
                //    null,
                //    null);

                //Ly_histmatricula.Row.Update(
                //    conn,
                //    "2000000000000001",
                //    1,
                //    2009,
                //    1,
                //    "100-EMR-1-2",
                //    "unidade_ensino, outras, matricula",
                //    "33333333", "outras", "12121263"
                //    );

                //Ly_matricula.Row.Update(
                //     conn,
                //     "2000000000000001",
                //     "152-EMR-1-4",
                //     "1001-181407",
                //     2009,
                //     1,
                //     "dependencia",
                //     "N"
                //     );

                //Ly_histmatricula.Row.Update(
                //   conn,
                //   "2000000000000001",
                //   1,
                //   2009,
                //   1,
                //   "100-EMR-1-2",
                //   "unidade_ensino, outras, matricula, dependencia",
                //   "33333333", "outras", "12121263", "S"
                //   );

//                using (var context = DataContextBuilder.FromLyceum.UsingLock())
//                {
//                    var contextQuery = new ContextQuery(
//                        @" INSERT  LY_HISTMATRICULA ( ALUNO, 
//                            ORDEM, 
//                            ANO, 
//                            SEMESTRE, 
//                            DISCIPLINA, 
//                            TURMA,
//                            NOTA_FINAL, 
//                            SITUACAO_HIST, 
//                            PERC_PRESENCA,
//                            HORAS_AULA, 
//                            CREDITOS,
//                            OBSERVACAO,
//                            NIVEL_PRESENCA,
//                            SERIE, 
//                            DT_INICIO, 
//                            DT_FIM, 
//                            DT_MATRICULA,
//                            UNIDADE_ENSINO, 
//                            OUTRAS, 
//                            MATRICULA )
//                        VALUES  (
//                            @ALUNO, 
//                            @ORDEM, 
//                            @ANO, 
//                            @SEMESTRE, 
//                            @DISCIPLINA, 
//                            @TURMA,
//                            @NOTA_FINAL, 
//                            @SITUACAO_HIST, 
//                            CONVERT(DECIMAL(10,2),@PERC_PRESENCA),
//                            @HORAS_AULA, 
//                            @CREDITOS,
//                            @OBSERVACAO,
//                            @NIVEL_PRESENCA,
//                            @SERIE, 
//                            @DT_INICIO, 
//                            @DT_FIM, 
//                            GetDate(),
//                            @UNIDADE_ENSINO, 
//                            @OUTRAS, 
//                            @MATRICULA ) ");

//                    contextQuery.Parameters.Add("@ALUNO", "2000000000000001");
//                    contextQuery.Parameters.Add("@ORDEM", TechneDbType.T_NUMERO_PEQUENO, 1);
//                    contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, 2011);
//                    contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, 0);
//                    contextQuery.Parameters.Add("@DISCIPLINA", "88-EMR-1-1");
//                    contextQuery.Parameters.Add("@TURMA", "1001-181407");
//                    contextQuery.Parameters.Add("@NOTA_FINAL", "10");
//                    contextQuery.Parameters.Add("@SITUACAO_HIST", "Aprovado");
//                    contextQuery.Parameters.Add("@PERC_PRESENCA", TechneDbType.T_PERCENTUAL54, 90);
//                    contextQuery.Parameters.Add("@HORAS_AULA", TechneDbType.T_DECIMAL_MEDIO, 1);
//                    contextQuery.Parameters.Add("@CREDITOS", TechneDbType.T_DECIMAL_MEDIO, 1);
//                    contextQuery.Parameters.Add("@OBSERVACAO", null);
//                    contextQuery.Parameters.Add("@NIVEL_PRESENCA", "Presencial");
//                    contextQuery.Parameters.Add("@SERIE", TechneDbType.T_NUMERO_PEQUENO, 1);
//                    contextQuery.Parameters.Add("@DT_INICIO", TechneDbType.T_DATA, DateTime.Now);
//                    contextQuery.Parameters.Add("@DT_FIM", TechneDbType.T_DATA, DateTime.Now);
//                    contextQuery.Parameters.Add("@DT_MATRICULA", TechneDbType.T_DATA, DateTime.Now);
//                    contextQuery.Parameters.Add("@UNIDADE_ENSINO", "33081743");
//                    contextQuery.Parameters.Add("@OUTRAS", null);
//                    contextQuery.Parameters.Add("@MATRICULA", "21212121");

//                    context.ApplyModifications(contextQuery);

                //    var errors = conn.GetErrors();

                //    if (errors.Count > 0)
                //    {
                //        Console.Write("Deu erro!");
                //    }
                //}

                Ly_histmatricula.Row.Update(
                   conn,
                   "2000000000000001",
                   1,
                   2009,
                   1,
                   "100-EMR-1-2",
                   "dependencia",
                   "S"
                   );
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cronos error: {0}", ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}