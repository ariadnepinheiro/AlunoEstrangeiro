using System;
using System.Collections.Generic;
using System.Data;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;
using System.Text;

namespace Techne.Lyceum.RN
{
    public class TurmaProjeto : RNBase
    {

        public static RetValue Incluir(Ly_turma dtTurma)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);
            RetValue valorRetorno = null;

            try
            {
                if (!VerificaAutorizacaoUnidadeEnsino(connection, dtTurma.Rows[0].Unidade_responsavel, dtTurma.Rows[0].Curso, dtTurma.Rows[0].Turno))
                {
                    connection.Rollback();
                    return new RetValue(false, null, new ErrorList("Curso não autorizado para unidade de ensino."));
                }

                if (RN.Turma.VerificarConflitoDependencia(connection, dtTurma.Rows[0].Faculdade, dtTurma.Rows[0].Dependencia, dtTurma.Rows[0].Dt_inicio.Value, dtTurma.Rows[0].Dt_fim.Value, dtTurma.Rows[0].Turma, dtTurma.Rows[0].Turno))
                {
                    connection.Rollback();
                    return new RetValue(false, null, new ErrorList("Conflito de dependencia."));
                }

                if (!RN.Serie.VerificarSerieExtinta(connection, dtTurma.Rows[0].Curso, dtTurma.Rows[0].Turno, dtTurma.Rows[0].Curriculo, dtTurma.Rows[0].Serie.Value))
                {
                    connection.Rollback();
                    return new RetValue(false, null, new ErrorList("Série extinta."));
                }

                if (dtTurma.Rows[0].Dt_inicio.Value.Year != dtTurma.Rows[0].Ano)
                {
                    connection.Rollback();
                    return new RetValue(false, null, new ErrorList("Início das aulas deve ser do ano " + dtTurma.Rows[0].Ano + "."));
                }

                if (dtTurma.Rows[0].Dt_fim.Value.Year != dtTurma.Rows[0].Ano)
                {
                    connection.Rollback();
                    return new RetValue(false, null, new ErrorList("Término das aulas deve ser do ano " + dtTurma.Rows[0].Ano + "."));
                }

                //Inclusão de turma
                valorRetorno = RN.Turma.Incluir(connection, dtTurma,string.Empty);

                if (valorRetorno != null)
                {
                    connection.Rollback();
                    return valorRetorno;
                }

            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Rollback();

                return new RetValue(false, null, new ErrorList(ex.Message));
            }
            finally
            {
                connection.Close();
            }
            return new RetValue(true, "Registro incluído com sucesso.", null);
        }

        private static bool VerificaAutorizacaoUnidadeEnsino(TConnection connection, string unidade_ensino, string curso, string turno)
        {
            string sql = " select 1 " +
                         " from LY_UNIDADE_ENSINO_CURSOS " +
                         " where " +
                         " UNIDADE_ENS = ? " +
                         " and CURSO = ? " +
                         " and TURNO = ? ";

            DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, unidade_ensino, curso, turno);
            if (!valorConsulta.IsNull)
                return true;
            else
                return false;
        }

        public static RetValue Alterar(Techne.Lyceum.RN.Turma.DadosTurma turmaOld, String nomeTurma, Ly_turma dtTurma)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            RetValue valorRetorno = null;
            try
            {
                #region Validações da Turma

                if (turmaOld.Turno != dtTurma.Rows[0].Turno)
                {
                    List<String> erros = RN.Turma.PermiteAlterarTurnoDeTurma(turmaOld, nomeTurma, turmaOld.Turno);
                    if (erros.Count > 0)
                    {
                        connection.Rollback();
                        return new RetValue(false, null, new ErrorList(erros[0]));
                    }
                }

                if (dtTurma.Rows[0].Dt_fim.Value.Date < DateTime.Now.Date)
                {
                    connection.Rollback();
                    return new RetValue(false, null, new ErrorList("Término das Aulas não deve ser menor que data atual."));
                }

                if (dtTurma.Rows[0].Dt_inicio.Value.Year != dtTurma.Rows[0].Ano)
                {
                    connection.Rollback();
                    return new RetValue(false, "", new ErrorList("Início das Aulas deve ser do ano " + dtTurma.Rows[0].Ano + "."));
                }

                if (dtTurma.Rows[0].Dt_fim.Value.Year != dtTurma.Rows[0].Ano)
                {
                    connection.Rollback();
                    return new RetValue(false, null, new ErrorList("Término das Aulas deve ser do ano " + dtTurma.Rows[0].Ano + "."));
                }

                if (RN.Turma.VerificarConflitoDependencia(connection, dtTurma.Rows[0].Faculdade, dtTurma.Rows[0].Dependencia, dtTurma.Rows[0].Dt_inicio.Value, dtTurma.Rows[0].Dt_fim.Value, dtTurma.Rows[0].Turma, dtTurma.Rows[0].Turno))
                {
                    connection.Rollback();
                    return new RetValue(false, null, new ErrorList("Conflito de dependencia."));
                }

                if (!VerificaAutorizacaoUnidadeEnsino(connection, dtTurma.Rows[0].Unidade_responsavel, dtTurma.Rows[0].Curso, dtTurma.Rows[0].Turno))
                {
                    connection.Rollback();
                    return new RetValue(false, "", new ErrorList("Curso não autorizado para unidade de ensino."));
                }

                if (!RN.Serie.VerificarSerieExtinta(connection, dtTurma.Rows[0].Curso, dtTurma.Rows[0].Turno, dtTurma.Rows[0].Curriculo, dtTurma.Rows[0].Serie.Value))
                {
                    connection.Rollback();
                    return new RetValue(false, "", new ErrorList("Série extinta."));
                }

                //altera os dados da turma
                valorRetorno = RN.Turma.Alterar(connection, dtTurma, turmaOld.Turno);
                if (valorRetorno != null)
                {
                    connection.Rollback();
                    return valorRetorno;
                }

                #endregion
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Rollback();
                return new RetValue(false, null, new ErrorList(ex.Message));
            }
            finally
            {
                connection.Close();
            }

            return new RetValue(true, "Registro alterado com sucesso.", null);
        }       

        public static Ly_turma.Row ConsultarDetalhes(string turma, string disciplina)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                Ly_turma.Row row = CR.Ly_turma.QueryFirstRow(connection,
                        @" TURMA = ? 
                          AND DISCIPLINA = ? ", turma, disciplina);
                return row;
            }
            finally
            {
                connection.Close();
            }
        }

        public static RetValue AtualizarDocentes(Ly_turma.Row dadosDocente)
        {
            RetValue retorno = null;

            TConnectionWritable connection = Config.CreateWritableConnection();
            try
            {
                connection.Open(true);

                if (dadosDocente != null)
                {

                    Ly_turma.Row.Update(connection, dadosDocente.Disciplina, dadosDocente.Turma, dadosDocente.Ano, dadosDocente.Semestre, "num_func", dadosDocente.Num_func);

                    retorno = VerificarErro(connection.GetErrors());

                    if (retorno != null && !retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }
                    return new RetValue(true, "Registro alterado com sucesso.", null);
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        public static decimal ConsultaMatricula(decimal? num_func)
        {
            string sql = @"select matricula from ly_docente where num_func = ?";

            return ExecutarFuncaoDec(sql, num_func);
        }

        public static QueryTable ConsultarTurma(string ano, string unidade_responsavel, string sit_turma)
        {
            StringBuilder strConsulta = new StringBuilder();
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
                    TU.tipo_gestao,
                    tu.sit_turma
                FROM
                    LY_GRADE_SERIE  GS (NOLOCK)
                    inner join ly_unidade_ensino US (NOLOCK) ON
                        US.unidade_ens = GS.faculdade
                    inner join ly_nucleo N (NOLOCK) ON
                        N.nucleo = US.nucleo
                    inner join ly_turno T (NOLOCK) ON
                        T.turno = GS.turno
                    inner join ly_unidade_ensino UE (NOLOCK) ON
                        UE.UNIDADE_ENS = GS.UNIDADE_RESPONSAVEL
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
						TU.SERIE = GS.SERIE AND
                        TU.SIT_TURMA = ?
                    WHERE
                    TU.CLASSIFICACAO = 'PROJ' AND
                    GS.ANO = ? ");

            if (!string.IsNullOrEmpty(unidade_responsavel))
                strConsulta.Append(String.Format(" AND GS.UNIDADE_RESPONSAVEL = {0}", unidade_responsavel));

            strConsulta.Append(" ORDER BY GS.GRADE ");

            return RNBase.Consultar(strConsulta.ToString(), sit_turma, ano);
        }


        #region Métodos vazios para utilização pelo ObjectDataSource

        [Obsolete("Não utilizar: método vazio para utilização pelo ObjectDataSource")]
        public static void UpdateMethodODS(object disciplina, object nome, object matricula)
        {
            return;
        }
        #endregion
    }
}