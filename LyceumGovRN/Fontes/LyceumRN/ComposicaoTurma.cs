using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Data;
using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN
{
    public class ComposicaoTurma : RNBase
    {
        public class DadosComposicao
        {
            public string usuario { get; set; }
            public string unidadeEns { get; set; }
            public string curso { get; set; }
            public string turno { get; set; }
            public string curriculo { get; set; }
            public decimal ano { get; set; }
            public decimal periodo { get; set; }
            public decimal serie { get; set; }
            public string turmaDestino { get; set; }
            public string gradeID { get; set; }
            public List<KeyValuePair<string, string>> turmas { get; set; }

            public DadosComposicao()
            { }

            public DadosComposicao(string usuario, string unidadeEns, string curso, string turno, string curriculo, decimal ano, decimal periodo, decimal serie, string turmaDestino, string gradeID, List<KeyValuePair<string, string>> turmas)
            {
                this.unidadeEns = unidadeEns;
                this.curso = curso;
                this.turno = turno;
                this.curriculo = curriculo;
                this.ano = ano;
                this.periodo = periodo;
                this.serie = serie;
                this.turmaDestino = turmaDestino;
                this.gradeID = gradeID;
                this.turmas = turmas;
                this.usuario = usuario;
            }
        }

        public static RetValue ComporTurmas(DadosComposicao dados)
        {
            RetValue retorno = null;
            TConnectionWritable connection = Config.CreateWritableConnection();

            try
            {
                connection.Open(true);

                for (int i = 0; i < dados.turmas.Count; i++)
                {
                    QueryTable qtDiscNota = new QueryTable(@"select distinct gt.DISCIPLINA, gt.TURMA, gt.ANO, gt.SEMESTRE
                                                    from LY_GRADE_TURMA gt inner join LY_NOTA n
                                                    on  gt.DISCIPLINA = n.DISCIPLINA
                                                    and gt.TURMA = n.TURMA
                                                    and gt.ANO = n.ANO
                                                    and gt.SEMESTRE = n.SEMESTRE
                                                    where gt.GRADE_ID = ?
                                                    order by gt.DISCIPLINA");
                    qtDiscNota.Query(connection, dados.turmas[i].Value);

                    if (qtDiscNota.Rows.Count > 0)
                    {
                        return new RetValue(false, "", new Techne.Library.ErrorList("A turma " + qtDiscNota.Rows[0]["TURMA"].ToString() + " possui nota cadastrada na disciplina " + qtDiscNota.Rows[0]["DISCIPLINA"].ToString()));
                    }

                    QueryTable qtDiscImagem = new QueryTable(@"select distinct gt.DISCIPLINA, gt.TURMA, gt.ANO, gt.SEMESTRE
                                                            from LY_GRADE_TURMA gt inner join LY_IMAGEM i
                                                            on  gt.DISCIPLINA = i.DISCIPLINA
                                                            and gt.TURMA = i.TURMA
                                                            and gt.ANO = i.ANO
                                                            and gt.SEMESTRE = i.SEMESTRE
                                                            where gt.GRADE_ID = ?
                                                            order by gt.DISCIPLINA");
                    qtDiscImagem.Query(connection, dados.turmas[i].Value);

                    if (qtDiscImagem.Rows.Count > 0)
                    {
                        return new RetValue(false, "", new Techne.Library.ErrorList("A turma " + qtDiscImagem.Rows[0]["TURMA"].ToString() + " possui imagem de frequência cadastrada na disciplina " + qtDiscImagem.Rows[0]["DISCIPLINA"].ToString()));
                    }

                    QueryTable qtDiscFalta = new QueryTable(@"select distinct gt.DISCIPLINA, gt.TURMA, gt.ANO, gt.SEMESTRE
                                                            from LY_GRADE_TURMA gt inner join LY_FALTA f
                                                            on  gt.DISCIPLINA = f.DISCIPLINA
                                                            and gt.TURMA = f.TURMA
                                                            and gt.ANO = f.ANO
                                                            and gt.SEMESTRE = f.PERIODO
                                                            where gt.GRADE_ID = ?
                                                            order by gt.DISCIPLINA");
                    qtDiscFalta.Query(connection, dados.turmas[i].Value);

                    if (qtDiscFalta.Rows.Count > 0)
                    {
                        return new RetValue(false, "", new Techne.Library.ErrorList("A turma " + qtDiscFalta.Rows[0]["TURMA"].ToString() + " possui falta cadastrada na disciplina " + qtDiscFalta.Rows[0]["DISCIPLINA"].ToString()));
                    }

                    QueryTable qtDiscFreq = new QueryTable(@"select distinct gt.DISCIPLINA, gt.TURMA, gt.ANO, gt.SEMESTRE
                                                            from LY_GRADE_TURMA gt inner join LY_LISTA_FREQ lf
                                                            on  gt.DISCIPLINA = lf.DISCIPLINA
                                                            and gt.TURMA = lf.TURMA
                                                            and gt.ANO = lf.ANO
                                                            and gt.SEMESTRE = lf.SEMESTRE
                                                            where gt.GRADE_ID = ?
                                                            order by gt.DISCIPLINA");
                    qtDiscFreq.Query(connection, dados.turmas[i].Value);

                    if (qtDiscFreq.Rows.Count > 0)
                    {
                        return new RetValue(false, "", new Techne.Library.ErrorList("A turma " + qtDiscFreq.Rows[0]["TURMA"].ToString() + " possui lista de frequência cadastrada na disciplina " + qtDiscFreq.Rows[0]["DISCIPLINA"].ToString()));
                    }
                }

                for (int i = 0; i < dados.turmas.Count; i++)
                {
                    QueryTable qtAlunos = new QueryTable("select distinct aluno from LY_MATGRADE where GRADE_ID = ? ");

                    qtAlunos.Query(connection, dados.turmas[i].Value);

                    for (int j = 0; j < qtAlunos.Rows.Count; j++)
                    {
                        QueryTable qtDisciplinasAluno = new QueryTable(@"select m.aluno, m.disciplina, turma, m.ano, m.semestre from LY_GRADE_TURMA gt join LY_MATRICULA m
                                                                        on  gt.DISCIPLINA = m.DISCIPLINA
                                                                        and gt.TURMA = m.TURMA
                                                                        and gt.ANO = m.ANO
                                                                        and gt.SEMESTRE = m.SEMESTRE
                                                                        where gt.GRADE_ID = ?
                                                                        and m.ALUNO = ?");
                        qtDisciplinasAluno.Query(connection, dados.turmas[i].Value, qtAlunos.Rows[j]["aluno"].ToString());

                        for (int y = 0; y < qtDisciplinasAluno.Rows.Count; y++)
                        {
                            Ly_turma_transf.Row.Insert(connection, qtAlunos.Rows[j]["aluno"].ToString(), qtDisciplinasAluno.Rows[y]["disciplina"].ToString(), dados.turmaDestino, dados.ano, dados.periodo, qtDisciplinasAluno.Rows[y]["turma"].ToString(), DateTime.Now, dados.usuario);

                            retorno = VerificarErro(connection.GetErrors());
                            if (retorno != null)
                            {
                                if (!retorno.Ok)
                                    return retorno;
                            }

                            Ly_matricula.Row.Update(connection, qtAlunos.Rows[j]["aluno"].ToString(), qtDisciplinasAluno.Rows[y]["disciplina"].ToString(), qtDisciplinasAluno.Rows[y]["turma"].ToString(), Convert.ToDecimal(qtDisciplinasAluno.Rows[y]["ano"]), Convert.ToDecimal(qtDisciplinasAluno.Rows[y]["semestre"]), "turma,dt_ultalt,sit_matricula", dados.turmaDestino, DateTime.Now, "Matriculado");

                            retorno = VerificarErro(connection.GetErrors());
                            if (retorno != null)
                            {
                                if (!retorno.Ok)
                                    return retorno;
                            }

                            QueryTable qtMatGrade = new QueryTable("Select aluno from ly_matgrade where aluno = ? and grade_id = ?");
                            qtMatGrade.Query(connection, qtAlunos.Rows[j]["aluno"].ToString(), dados.turmas[i].Value);

                            if (qtMatGrade.Rows.Count > 0)
                            {
                                TCommand comandMatGrade = new TCommand("Update ly_matgrade set sit_matgrade = 'Transf.Internamente', dt_ultalt = getdate() where aluno = @aluno and grade_id = @grade_id", connection);
                                comandMatGrade.Parameters.Add("@aluno", qtAlunos.Rows[j]["aluno"].ToString());
                                comandMatGrade.Parameters.Add("@grade_id", dados.turmas[i].Value);
                                comandMatGrade.ExecuteNonQuery();
                            }

                            retorno = VerificarErro(connection.GetErrors());
                            if (retorno != null)
                            {
                                if (!retorno.Ok)
                                    return retorno;
                            }
                        }
                    }
                }
                if (retorno != null)
                {
                    if (!retorno.Ok)
                        return retorno;
                }
                else
                    return new RetValue(true, "Composição de turmas realizada com sucesso.", new Techne.Library.ErrorList(""));

            }
            catch (Exception ex)
            {
                connection.Rollback();
                return new RetValue(false, "", new Techne.Library.ErrorList(ex.Message));
            }
            finally
            {
                connection.Close();
            }

            return retorno;
        }
    }
}
