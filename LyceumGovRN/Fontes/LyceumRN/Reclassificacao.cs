using System;
using Techne.Data;
using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN
{
    public class Reclassificacao : RNBase
    {
        public static QueryTable ConsultarReclassificacao(string aluno)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql = @"SELECT id_aluno_reclassificacao, aluno, r.curso, r.turno, r.curriculo, s.DESCRICAO as serie, dt_reclassificacao, 
                                observacao, curso_original, turno_original, curriculo_original, serie_original 
                                from LY_ALUNO_RECLASSIFICACAO r
                                inner join ly_serie s on r.serie = s.serie and r.CURSO = s.CURSO and r.TURNO = s.TURNO and r.CURRICULO = s.CURRICULO
                                where ALUNO = ?";


                QueryTable qt;
                qt = new QueryTable(sql);
                qt.Query(connection, aluno);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable ConsultarSeries(string curso, string turno, string curriculo, string serie)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                //string curso, string turno, string curriculo
                String sql = @"select SERIE, DESCRICAO from LY_SERIE where CURSO = ? and TURNO = ? and CURRICULO = ? and SERIE > ?";


                QueryTable qt;
                qt = new QueryTable(sql);
                qt.Query(connection, curso, turno, curriculo, serie);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable ConsultarSeries()
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                //string curso, string turno, string curriculo
                String sql = @"select SERIE, DESCRICAO from LY_SERIE";


                QueryTable qt;
                qt = new QueryTable(sql);
                qt.Query(connection);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }




        #region Métodos vazios para utilização pelo ObjectDataSource
        [Obsolete("Não utilizar: método vazio para utilização pelo ObjectDataSource")]
        public static void InsertMethodODS(object serie, object curso, object turno, object dt_reclassificacao, object observacao)
        {
            return;
        }
        [Obsolete("Não utilizar: método vazio para utilização pelo ObjectDataSource")]
        public static void DeleteMethodODS(object id_aluno_reclassificicao)
        {
            return;
        }
        [Obsolete("Não utilizar: método vazio para utilização pelo ObjectDataSource")]
        public static void UpdateMethodODS(object id_aluno_reclassificacao, object serie, object curso, object turno, object dt_reclassificacao, object observacao)
        {
            return;
        }
        #endregion


        public static bool VerificaMatricula(string aluno)
        {

            string sql = @"select 1 from ly_matgrade mg 
                    join ly_grade_turma gt on mg.grade_id = gt.grade_id 
					join ly_grade_serie gs on gs.GRADE_ID = mg.GRADE_ID and gs.GRADE = gt.TURMA and gs.GRADE = gt.TURMA 
					join ly_matricula m on m.ALUNO = mg.ALUNO and gt.disciplina = m.disciplina and m.turma = gt.turma and m.ANO = gt.ANO and m.SEMESTRE = gt.SEMESTRE 
					where mg.sit_matgrade = 'Matriculado'
					and mg.aluno = ?";
                //"select 1 from LY_MATRICULA WHERE ALUNO = ? ";

            int retorno = ExecutarFuncao(sql, aluno);

            if (retorno >= 1)
                return true;
            else
                return false;
        }

        public static RetValue Inserir(Ly_aluno_reclassificacao.Row row)
        {
            //Inserção
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                Ly_aluno.Row.Update(connection, row.Aluno, "serie, curso, turno, curriculo", row.Serie, row.Curso, row.Turno, row.Curriculo);
                Ly_aluno_reclassificacao.Row.Insert(connection, row.Aluno, row.Curso, row.Turno, row.Curriculo, row.Serie,
                    row.Curso_original, row.Turno_original, row.Curriculo_original, row.Serie_original, "dt_reclassificacao, observacao",
                    row.Dt_reclassificacao, row.Observacao);

                //if (VerificaMatricula(row.Aluno))
                //{
                //    QueryTable qt = ConsultaMatricula(row.Aluno);
                //    if (qt.Rows.Count > 1)
                //    {
                //        for (int i = 0; i < qt.Rows.Count; i++)
                //        {
                //            Ly_matricula.Row.Delete(connection, row.Aluno, qt.Rows[i]["disciplina"].ToString(), qt.Rows[i]["turma"].ToString(), (decimal?)qt.Rows[i]["ano"], (decimal?)qt.Rows[i]["periodo"]);
                //        }
                //    }
                //}


                RetValue ret = VerificarErro(connection.GetErrors());
                if (ret != null && !ret.Ok)
                    connection.Rollback();
                return ret;
            }
            catch
            {
                connection.Rollback();
                return VerificarErro(connection.GetErrors());
            }
            finally
            {
                connection.Close();
            }
        }

        public static RetValue Atualizar(Ly_aluno_reclassificacao.Row row)
        {
            //Atualização
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                Ly_aluno_reclassificacao.Row.Update(connection,row.Id_aluno_reclassificacao, "dt_reclassificacao, observacao",
                    row.Dt_reclassificacao, row.Observacao);


                RetValue ret = VerificarErro(connection.GetErrors());
                if (ret != null && !ret.Ok)
                    connection.Rollback();
                return ret;
            }
            catch
            {
                connection.Rollback();
                return VerificarErro(connection.GetErrors());
            }
            finally
            {
                connection.Close();
            }
        }

        public static RetValue Excluir(decimal id_recl)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                decimal serie;
                string curso;
                string turno;
                string curriculo;
                string aluno;
                QueryTable qt = ConsultarReclassificacao(id_recl);
                if (qt != null)
                {
                    aluno = qt.Rows[0]["aluno"].ToString();
                    serie = Convert.ToDecimal(qt.Rows[0]["serie_original"]);
                    curso = qt.Rows[0]["curso_original"].ToString();
                    turno = qt.Rows[0]["turno_original"].ToString();
                    curriculo = qt.Rows[0]["curriculo_original"].ToString();

                    Ly_aluno.Row.Update(connection, aluno, "serie, curso, turno, curriculo", serie, curso, turno, curriculo);
                    Ly_aluno_reclassificacao.Row.Delete(connection, id_recl);
                }
                
                RetValue ret = VerificarErro(connection.GetErrors());
                if (ret != null && !ret.Ok)
                    connection.Rollback();
                return ret;
            }
            catch
            {
                connection.Rollback();
                return VerificarErro(connection.GetErrors());
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable ConsultarReclassificacao(decimal id_recl)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql = @"SELECT aluno, curso, turno, curriculo, serie, dt_reclassificacao, 
                               observacao, curso_original, turno_original, curriculo_original, serie_original 
                               from LY_ALUNO_RECLASSIFICACAO where id_aluno_reclassificacao = ?";


                QueryTable qt;
                qt = new QueryTable(sql);
                qt.Query(connection, id_recl);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable ConsultaMatricula(string aluno)
        {
            TConnectionWritable cn = Config.CreateWritableConnection();

            QueryTable qt = new QueryTable("Select top 1 gt.ano as ano, gt.semestre as periodo, gt.turma as turma, gt.disciplina as disciplina from ly_matgrade mg " +
                    "join ly_grade_turma gt on mg.grade_id = gt.grade_id " +
                    "join ly_grade_serie gs on gs.GRADE_ID = mg.GRADE_ID and gs.GRADE = gt.TURMA and gs.GRADE = gt.TURMA " +
                    "join ly_matricula m on m.ALUNO = mg.ALUNO and gt.disciplina = m.disciplina and m.turma = gt.turma and m.ANO = gt.ANO and m.SEMESTRE = gt.SEMESTRE " +
                    "where mg.sit_matgrade = 'Matriculado' " +
                    "and mg.aluno = ?");

            qt.Query(cn, aluno);

            return qt;
        }


        /*Criar os métodos de ConsultarCursoSerie(serie), ConsultarTurnoSerie(serie, curso), ConsultarCurriculoSerie(serie, curso, turno) 
         * para preencher as combos da grid*/

        public static QueryTable ConsultarCursoReclassificacao(string curso)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                //string curso
                String sql = @"SELECT NOME, CURSO FROM LY_CURSO WHERE TEM_RECLASSIFICACAO = 'S'  and CURSO > = ? ORDER BY NOME"; ;


                QueryTable qt;
                qt = new QueryTable(sql);
                qt.Query(connection, curso);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static bool ExisteRegistro(string aluno, string curso, string turno, string curriculo, decimal serie)
        {

            string sql = @"select 1 from ly_aluno_reclassificacao 
                           where ALUNO = ? and CURSO = ? and TURNO = ? AND CURRICULO = ? AND SERIE = ?";

            int retorno = ExecutarFuncao(sql, aluno, curso, turno, curriculo, serie);

            if (retorno >= 1)
                return true;
            else
                return false;
        }

        public static int VerificaId(string aluno, string curso, string turno, decimal serie)
        {
            //string curriculo,
            string sql = @"select id_aluno_reclassificacao from ly_aluno_reclassificacao 
                           where ALUNO = ? and CURSO = ? and TURNO = ? AND SERIE = ?";

            int retorno = ExecutarFuncao(sql, aluno, curso, turno, serie);

            return retorno;
        }


        public static string ConsultarCurso(string p)
        {
            string sql = @"select curso from ly_aluno_reclassificacao 
                           where ID_ALUNO_RECLASSIFICACAO = ?";

            string retorno = ConsultarCampo(sql, p);

            return retorno;
        }

        public static string ConsultarTurno(string p)
        {
            string sql = @"select turno from ly_aluno_reclassificacao 
                           where ID_ALUNO_RECLASSIFICACAO = ?";

            string retorno = ConsultarCampo(sql, p);

            return retorno;
        }

    }
}
