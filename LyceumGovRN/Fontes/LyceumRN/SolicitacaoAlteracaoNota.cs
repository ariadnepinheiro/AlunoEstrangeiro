namespace Techne.Lyceum.RN
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Data;
    using Library;
    using Seeduc.Infra.Data;
    using Techne.Lyceum.RN.DTOs;
    using Techne.Lyceum.RN.Entidades;

    public class SolicitacaoAlteracaoNota : RNBase
    {
        private const string Aprovacao = "Aprovado";

        private const string Aguardando = "Aguardando";

        private const string Reprovacao = "Reprovado";

        public static RetValue Aprovar(IList<StatusSolicitacaoAlteracaoNota> statusSolicitacaoAlteracaoNotas)
        {
            RetValue retorno;
            var connection = Config.CreateWritableConnection();

            foreach (var statusSolicitacaoAlteracaoNota in statusSolicitacaoAlteracaoNotas)
            {
                statusSolicitacaoAlteracaoNota.Status = Aprovacao;
            }

            try
            {
                connection.Open(true);

                // Altera o status da solicitação
                retorno = AlterarStatus(connection, statusSolicitacaoAlteracaoNotas);

                if (retorno != null
                    && !retorno.Ok)
                {
                    connection.Rollback();

                    return retorno;
                }
            }
            catch (Exception e)
            {
                connection.Rollback();

                retorno = new RetValue(false, null, new ErrorList(e.Message));
            }
            finally
            {
                connection.Close();
            }

            return retorno;
        }

        public static RetValue Incluir(TceSolicitacaoAlteracaoNota solicitacaoAlteracaoNota)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var data = DateTime.Now;
                    var contextQuery = new ContextQuery(
                        @"INSERT  INTO [dbo].[TCE_SOLICITACAO_ALTERACAO_NOTA]
                                (
                                  NUM_FUNC,
                                  UNIDADE_ENS,
                                  TURMA,
                                  DISCIPLINA,
                                  ANO,
                                  SEMESTRE,
                                  SUBPERIODO,
                                  STATUS,
                                  DT_STATUS,
                                  DT_SOLICITACAO,
                                  JUSTIFICATIVA
                                )
                        VALUES  (
                                  @NUM_FUNC,
                                  @UNIDADE_ENS,
                                  @TURMA,
                                  @DISCIPLINA,
                                  @ANO,
                                  @SEMESTRE,
                                  @SUBPERIODO,
                                  @STATUS,
                                  @DT_STATUS,
                                  @DT_SOLICITACAO,
                                  @JUSTIFICATIVA
                                )");

                    contextQuery.Parameters.Add("@NUM_FUNC", solicitacaoAlteracaoNota.NumFunc);
                    contextQuery.Parameters.Add("@UNIDADE_ENS", solicitacaoAlteracaoNota.UnidadeEns);
                    contextQuery.Parameters.Add("@TURMA", solicitacaoAlteracaoNota.Turma);
                    contextQuery.Parameters.Add("@DISCIPLINA", solicitacaoAlteracaoNota.Disciplina);
                    contextQuery.Parameters.Add("@ANO", solicitacaoAlteracaoNota.Ano);
                    contextQuery.Parameters.Add("@SEMESTRE", solicitacaoAlteracaoNota.Semestre);
                    contextQuery.Parameters.Add("@SUBPERIODO", solicitacaoAlteracaoNota.Subperiodo);
                    contextQuery.Parameters.Add("@STATUS", Aguardando);
                    contextQuery.Parameters.Add("@DT_STATUS", SqlDbType.DateTime, data);
                    contextQuery.Parameters.Add("@DT_SOLICITACAO", SqlDbType.DateTime, data);
                    contextQuery.Parameters.Add("@JUSTIFICATIVA", solicitacaoAlteracaoNota.Justificativa);

                    ctx.ApplyModifications(contextQuery);

                    return new RetValue(true, "Solicitação enviada com sucesso. Acompanhe o status da liberação do lançamento através do campo “Status para Lançamento” na tela de seleção de turmas. Após a liberação do diretor, você terá 7 dias consecutivos para efetuar o lançamento.", null);
                }
                catch (Exception e)
                {
                    ctx.Abandon();

                    return new RetValue(false, null, new ErrorList(e.Message));
                }
            }
        }

        public static QueryTable ListarPorUA(string setor, string status, string ano, string periodo)
        {
            if (string.IsNullOrEmpty(setor))
            {
                return null;
            }

            var sql = @"SELECT  
                                n.DESCRICAO AS 'COORDENADORIA',
                                ue.NOME_COMP AS 'ESCOLA',
                                d.MATRICULA AS 'MATRICULA',
                                PE.NOME_COMPL AS 'NOME',
                                san.TURMA,
                                san.DISCIPLINA,
                                san.SUBPERIODO,
                                san.DT_SOLICITACAO,
                                san.JUSTIFICATIVA,
                                san.STATUS,
                                san.DT_STATUS,
                                san.ID_SOLICITACAO_ALTERACAO_NOTA,
                                san.MATRICULA_APROVADOR,
                                san.ANO,
                                san.DT_LIMITE,
                                san.SEMESTRE ,
                                disc.NOME  as 'DISC_NOME'

                        FROM    dbo.TCE_SOLICITACAO_ALTERACAO_NOTA san WITH ( NOLOCK )
                                INNER JOIN dbo.LY_UNIDADE_ENSINO ue WITH ( NOLOCK )
                                                                         ON san.UNIDADE_ENS = ue.UNIDADE_ENS
                                INNER JOIN dbo.LY_NUCLEO n WITH ( NOLOCK )
                                                                ON ue.NUCLEO = n.NUCLEO
                                INNER JOIN dbo.LY_DOCENTE d WITH ( NOLOCK )
                                                                ON san.NUM_FUNC = d.NUM_FUNC
                                INNER JOIN dbo.LY_PESSOA PE WITH ( NOLOCK )
                                                                ON PE.PESSOA = D.PESSOA
                                LEFT JOIN dbo.LY_VINCULO V WITH ( NOLOCK )
                                                                  ON  V.MATRICULA = SAN.MATRICULA_APROVADOR
                                inner join ly_disciplina disc on disc.DISCIPLINA = san.DISCIPLINA    
                        WHERE   ue.SETOR = ? 
                        and san.ANO = ? 
                        and san.semestre = ? ";

            if (!string.IsNullOrEmpty(status))
            {
                sql += "AND [STATUS] = ?";
                sql += " ORDER BY n.DESCRICAO,ue.NOME_COMP,PE.NOME_COMPL,DT_SOLICITACAO";

                return Consultar(sql, setor, ano, periodo, status);
            }

            sql += " ORDER BY n.DESCRICAO,ue.NOME_COMP,PE.NOME_COMPL,DT_SOLICITACAO";

            return Consultar(sql, setor, ano, periodo);
        }

        public static QueryTable ListarPorNucleo(string nucleo, string status, string ano, string periodo)
        {
            if (string.IsNullOrEmpty(nucleo))
            {
                return null;
            }

            var sql = @" SELECT  
                                re.REGIONAL,
                                ue.NOME_COMP AS 'ESCOLA',
                                d.MATRICULA AS 'MATRICULA',
                                PE.NOME_COMPL AS 'NOME',
                                san.TURMA,
                                san.DISCIPLINA,
                                san.SUBPERIODO,
                                san.DT_SOLICITACAO,
                                san.JUSTIFICATIVA,
                                san.STATUS,
                                san.DT_STATUS,
                                san.ID_SOLICITACAO_ALTERACAO_NOTA,
                                san.MATRICULA_APROVADOR,
                                san.ANO,
                                san.DT_LIMITE,
                                san.SEMESTRE ,
                                disc.NOME  as 'DISC_NOME'

                        FROM    dbo.TCE_SOLICITACAO_ALTERACAO_NOTA san WITH ( NOLOCK )
                                INNER JOIN dbo.LY_UNIDADE_ENSINO ue WITH ( NOLOCK )
                                                                         ON san.UNIDADE_ENS = ue.UNIDADE_ENS
                                INNER JOIN TCE_REGIONAL re WITH ( NOLOCK )
                                                                ON ue.ID_REGIONAL = re.ID_REGIONAL
                                INNER JOIN dbo.LY_DOCENTE d WITH ( NOLOCK )
                                                                ON san.NUM_FUNC = d.NUM_FUNC
                                INNER JOIN dbo.LY_PESSOA PE WITH ( NOLOCK )
                                                                ON PE.PESSOA = D.PESSOA
                                LEFT JOIN dbo.LY_VINCULO V WITH ( NOLOCK )
                                                                  ON  V.MATRICULA = SAN.MATRICULA_APROVADOR
                                inner join ly_disciplina disc on disc.DISCIPLINA = san.DISCIPLINA    
                        WHERE   re.ID_REGIONAL = ? 
                        and san.ANO = ? 
                        and san.SEMESTRE = ? ";

            if (!string.IsNullOrEmpty(status))
            {
                sql += "AND [STATUS] = ?";
                sql += " ORDER BY RE.REGIONAL,ue.NOME_COMP,PE.NOME_COMPL,DT_SOLICITACAO";

                return Consultar(sql, nucleo, ano, periodo, status);
            }

            sql += " ORDER BY RE.REGIONAL,ue.NOME_COMP,PE.NOME_COMPL,DT_SOLICITACAO";

            return Consultar(sql, nucleo, ano, periodo);
        }

        public static QueryTable ListarPorCenso(string censo, string status, string ano, string periodo)
        {
            if (string.IsNullOrEmpty(censo))
            {
                return null;
            }

            var sql = @" SELECT  
                                RE.REGIONAL,
                                ue.NOME_COMP AS 'ESCOLA',
                                d.MATRICULA AS 'MATRICULA',
                                PE.NOME_COMPL AS 'NOME',
                                san.TURMA,
                                san.DISCIPLINA,
                                san.SUBPERIODO,
                                san.DT_SOLICITACAO,
                                san.JUSTIFICATIVA,
                                san.STATUS,
                                san.DT_STATUS,
                                san.ID_SOLICITACAO_ALTERACAO_NOTA,
                                san.MATRICULA_APROVADOR,
                                san.ANO,
                                san.DT_LIMITE,
                                san.SEMESTRE ,
                                disc.NOME as 'DISC_NOME'

                        FROM    dbo.TCE_SOLICITACAO_ALTERACAO_NOTA san WITH ( NOLOCK )
                                INNER JOIN dbo.LY_UNIDADE_ENSINO ue WITH ( NOLOCK )
                                                                         ON san.UNIDADE_ENS = ue.UNIDADE_ENS
                                INNER JOIN TCE_REGIONAL RE (NOLOCK)
																ON UE.ID_REGIONAL = RE.ID_REGIONAL
                                INNER JOIN dbo.LY_DOCENTE d WITH ( NOLOCK )
                                                                ON san.NUM_FUNC = d.NUM_FUNC
                                INNER JOIN dbo.LY_PESSOA PE WITH ( NOLOCK )
                                                                ON PE.PESSOA = D.PESSOA
                                LEFT JOIN dbo.LY_VINCULO V WITH ( NOLOCK )
                                                                  ON  V.MATRICULA = SAN.MATRICULA_APROVADOR
                                inner join ly_disciplina disc on disc.DISCIPLINA = san.DISCIPLINA    
                        WHERE   ue.UNIDADE_ENS = ? 
                        and san.ANO = ? 
                        and san.semestre = ?  ";

            if (!string.IsNullOrEmpty(status))
            {
                sql += "AND [STATUS] = ?";
                sql += " ORDER BY RE.REGIONAL,ue.NOME_COMP,PE.NOME_COMPL,DT_SOLICITACAO";

                return Consultar(sql, censo, ano, periodo, status);
            }

            sql += " ORDER BY RE.REGIONAL,ue.NOME_COMP,PE.NOME_COMPL,DT_SOLICITACAO";

            return Consultar(sql, censo, ano, periodo);
        }

        public static RetValue Remover(int idSolicitacaoAlteracaoNota)
        {
            RetValue retorno;
            var connection = Config.CreateWritableConnection();
            var sql = string.Format(
                                    @"DELETE
                                      FROM    [dbo].[TCE_SOLICITACAO_ALTERACAO_NOTA]
                                      WHERE   [ID_SOLICITACAO_ALTERACAO_NOTA] = ?
                                              AND [STATUS] = '{0}'",
                                                                   Aguardando);

            try
            {
                connection.Open(true);

                TCommand.ExecuteNonQuery(connection, sql, idSolicitacaoAlteracaoNota);

                retorno = VerificarErro(connection.GetErrors());

                if (retorno != null
                    && !retorno.Ok)
                {
                    connection.Rollback();
                }
            }
            catch (Exception e)
            {
                connection.Rollback();

                retorno = new RetValue(false, null, new ErrorList(e.Message));
            }
            finally
            {
                connection.Close();
            }

            return retorno;
        }

        public static RetValue Reprovar(IList<StatusSolicitacaoAlteracaoNota> statusSolicitacaoAlteracaoNotas)
        {
            RetValue retorno;
            var connection = Config.CreateWritableConnection();

            foreach (var statusSolicitacaoAlteracaoNota in statusSolicitacaoAlteracaoNotas)
            {
                statusSolicitacaoAlteracaoNota.Status = Reprovacao;
            }

            try
            {
                connection.Open(true);

                retorno = AlterarStatus(connection, statusSolicitacaoAlteracaoNotas);

                if (retorno != null
                    && !retorno.Ok)
                {
                    connection.Rollback();
                }
            }
            catch (Exception e)
            {
                connection.Rollback();

                retorno = new RetValue(false, null, new ErrorList(e.Message));
            }
            finally
            {
                connection.Close();
            }

            return retorno;
        }

        public static bool PeriodoAlteracaoNotaValido(TceSolicitacaoAlteracaoNota solicitacaoAlteracaoNotas)
        {
            var sql = @"SELECT DISTINCT 1
                        FROM   [dbo].[TCE_SOLICITACAO_ALTERACAO_NOTA] (NOLOCK)
                        WHERE  [NUM_FUNC] = ?
                               AND [STATUS] = ?
                               AND [TURMA] = ?
                               AND [DISCIPLINA] = ?
                               AND [ANO] = ?
                               AND [SUBPERIODO] = ?
                               AND [UNIDADE_ENS] = ?
                               AND [DT_STATUS] <= GETDATE()
                               AND [DT_LIMITE] >= GETDATE()";

            var retorno = ExecutarFuncao(sql, solicitacaoAlteracaoNotas.NumFunc, Aprovacao, solicitacaoAlteracaoNotas.Turma, solicitacaoAlteracaoNotas.Disciplina, solicitacaoAlteracaoNotas.Ano, solicitacaoAlteracaoNotas.Subperiodo, solicitacaoAlteracaoNotas.UnidadeEns);

            return retorno == 1;
        }

        public static bool ExisteSolicitacaoAlteracaoNota(TceSolicitacaoAlteracaoNota solicitacaoAlteracaoNotas)
        {
            var sql = @"SELECT 1
                        FROM   [dbo].[TCE_SOLICITACAO_ALTERACAO_NOTA] (NOLOCK)
                        WHERE  [NUM_FUNC] = ?
                               AND [STATUS] = ?
                               AND [TURMA] = ?
                               AND [DISCIPLINA] = ?
                               AND [ANO] = ?
                               AND [SUBPERIODO] = ?
                               AND [UNIDADE_ENS] = ?";

            var retorno = ExecutarFuncao(sql, solicitacaoAlteracaoNotas.NumFunc, Aguardando, solicitacaoAlteracaoNotas.Turma, solicitacaoAlteracaoNotas.Disciplina, solicitacaoAlteracaoNotas.Ano, solicitacaoAlteracaoNotas.Subperiodo, solicitacaoAlteracaoNotas.UnidadeEns);

            return retorno == 1;
        }

        public static DateTime? DataSolicitacaoAlteracaoNota(TceSolicitacaoAlteracaoNota solicitacaoAlteracaoNota)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT DT_SOLICITACAO
                        FROM   [dbo].[TCE_SOLICITACAO_ALTERACAO_NOTA] (NOLOCK)
                        WHERE  [NUM_FUNC] = @NUM_FUNC
                               AND [STATUS] = @STATUS
                               AND [TURMA] = @TURMA
                               AND [DISCIPLINA] = @DISCIPLINA
                               AND [ANO] = @ANO
                               AND [SUBPERIODO] = @SUBPERIODO
                               AND [UNIDADE_ENS] = @UNIDADE_ENS");

                contextQuery.Parameters.Add("@NUM_FUNC", solicitacaoAlteracaoNota.NumFunc);
                contextQuery.Parameters.Add("@STATUS", Aguardando);
                contextQuery.Parameters.Add("@TURMA", solicitacaoAlteracaoNota.Turma);
                contextQuery.Parameters.Add("@DISCIPLINA", solicitacaoAlteracaoNota.Disciplina);
                contextQuery.Parameters.Add("@ANO", solicitacaoAlteracaoNota.Ano);
                contextQuery.Parameters.Add("@SUBPERIODO", solicitacaoAlteracaoNota.Subperiodo);
                contextQuery.Parameters.Add("@UNIDADE_ENS", solicitacaoAlteracaoNota.UnidadeEns);

                var result = ctx.GetReturnValue(contextQuery);

                if (result != null
                    && result != DBNull.Value)
                {
                    return (DateTime)result;
                }
            }

            return null;
        }

        private static RetValue AlterarStatus(TConnectionWritable connection, IEnumerable<StatusSolicitacaoAlteracaoNota> statusSolicitacaoAlteracaoNotas)
        {
            RetValue retorno = null;
            var sql = @"UPDATE  [dbo].[TCE_SOLICITACAO_ALTERACAO_NOTA]
                        SET     [STATUS] = ?,
                                [DT_STATUS] = ?,
                                [DT_LIMITE] = ?,
                                [MATRICULA_APROVADOR] = ?                                
                        WHERE   [ID_SOLICITACAO_ALTERACAO_NOTA] = ?";

            try
            {
                foreach (var statusSolicitacaoAlteracaoNota in statusSolicitacaoAlteracaoNotas)
                {
                    if (statusSolicitacaoAlteracaoNota.Status == Aprovacao)
                    {
                        TCommand.ExecuteNonQuery(
                                    connection,
                                    sql,
                                    statusSolicitacaoAlteracaoNota.Status,
                                    DateTime.Now,
                                    DateTime.Today.AddDays(7),
                                    statusSolicitacaoAlteracaoNota.MatriculaAprovador,
                                    statusSolicitacaoAlteracaoNota.IdSolicitacaoAlteracaoNota);

                        retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null
                            && !retorno.Ok)
                        {
                            return retorno;
                        }
                    }

                    if (statusSolicitacaoAlteracaoNota.Status == Reprovacao)
                    {
                        TCommand.ExecuteNonQuery(
                                    connection,
                                    sql,
                                    statusSolicitacaoAlteracaoNota.Status,
                                    DateTime.Now,
                                    DBNull.Value,
                                    statusSolicitacaoAlteracaoNota.MatriculaAprovador,
                                    statusSolicitacaoAlteracaoNota.IdSolicitacaoAlteracaoNota);

                        retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null
                            && !retorno.Ok)
                        {
                            connection.Rollback();

                            return retorno;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                retorno = new RetValue(false, null, new ErrorList(e.Message));
            }

            return retorno;
        }
    }
}