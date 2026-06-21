namespace Techne.Lyceum.RN
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Seeduc.Infra.Data;
    using Techne.Lyceum.RN.DTOs;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Lyceum.RN.Util;

    public class AvaliacaoCurriculoMinimoDocente : RNBase
    {
        public static void Alterar(ICollection<TceAvaliacaoCurriculoMinimoDocente> cmdocentes, FiltroAvaliacao filtro)
        {
            if (filtro == null)
            {
                return;
            }

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var matricula = filtro.Matricula;
                    var ano = filtro.Ano;
                    var periodo = filtro.Periodo;
                    var subperiodo = filtro.Subperiodo;

                    // Passo 1: Gravar log das avaliaçoes antigas, caso existam
                    // OBS: Se possível, encapsular de forma organizada e não passar
                    // um monte de argumentos
                    GravarLog(ctx, matricula, ano, periodo, subperiodo);

                    // Passo 2: Limpar todas as entradas das avaliaçoes antigas
                    // OBS: Se possível, encapsular de forma organizada e não passar
                    // um monte de argumentos
                    LimparCompetencias(ctx, matricula, ano, periodo, subperiodo);

                    // Passo 3: Gravar competências novas
                    if (cmdocentes != null)
                    {
                        foreach (var cmdocente in cmdocentes)
                        {
                            Inserir(cmdocente, ctx);
                        }
                    }
                }
                catch (Exception e)
                {
                    ctx.Abandon();

                    throw e;
                }
            }
        }

        public static TceAvaliacaoCurriculoMinimoDocente Carregar(int ano, int periodo, int subperiodo, string matricula)
        {
            try
            {
                var acm = new TceAvaliacaoCurriculoMinimoDocente();

                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery(
                        @"SELECT  *
                        FROM    TCE_AVALIACAO_CM_DOCENTE
                        WHERE   ANO = @ANO
                                AND PERIODO = @PERIODO
                                AND SUBPERIODO = @SUBPERIODO
                                AND MATRICULA = @MATRICULA");

                    contextQuery.Parameters.Add("@ANO", ano);
                    contextQuery.Parameters.Add("@PERIODO", periodo);
                    contextQuery.Parameters.Add("@SUBPERIODO", subperiodo);
                    contextQuery.Parameters.Add("@MATRICULA", matricula);

                    using (var reader = ctx.GetDataReader(contextQuery))
                    {
                        while (reader.Read())
                        {
                            acm.IdAvaliacaoCurriculoMinimo = (int)reader["ID_AVALIACAO_CM"];
                            acm.IdAvaliacaoCurriculoMinimoDocente = (int)reader["ID_AVALIACAO_CM_DOCENTE"];
                            acm.Resposta = (string)reader["RESPOSTA"];
                        }
                    }

                    return acm;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void Inserir(TceAvaliacaoCurriculoMinimoDocente acmDocente, DataContext ctx)
        {
            var contextQuery = new ContextQuery(
                @"INSERT  INTO TCE_AVALIACAO_CM_DOCENTE
                        (
                          ID_AVALIACAO_CM,
                          RESPOSTA,
                          MATRICULA
                        )
                VALUES  (
                          @ID_AVALIACAO_CM,
                          @RESPOSTA,
                          @MATRICULA
                        )");

            contextQuery.Parameters.Add("@ID_AVALIACAO_CM", acmDocente.IdAvaliacaoCurriculoMinimo);
            contextQuery.Parameters.Add("@RESPOSTA", acmDocente.Resposta);
            contextQuery.Parameters.Add("@MATRICULA", acmDocente.Matricula);

            ctx.ApplyModifications(contextQuery);
        }

        public static DataTable Listar(decimal ano, decimal periodo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT *
                    FROM   TCE_AVALIACAO_CM
                    WHERE  ANO = @ANO
                           AND PERIODO = @PERIODO");

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarAvaliacao(int ano, int periodo, int subperiodo, string matricula)
        {
            var contextQuery = new ContextQuery(
                @"SELECT  C.ID_AVALIACAO_CM,
                        ORDEM,
                        AVALIACAO,
                        (
                          SELECT    resposta
                          FROM      TCE_AVALIACAO_CM_DOCENTE d
                          WHERE     D.ID_AVALIACAO_CM = C.ID_AVALIACAO_CM
                                    AND MATRICULA = @MATRICULA
                        ) AS RESPOSTA
                FROM    TCE_AVALIACAO_CM c
                WHERE   ANO = @ANO
                        AND PERIODO = @PERIODO
                        AND SUBPERIODO = @SUBPERIODO
                        AND HABILITADO = 1
                ORDER BY ORDEM");

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@SUBPERIODO", subperiodo);
            contextQuery.Parameters.Add("@MATRICULA", matricula);

            return Consultar(contextQuery);
        }

        public static ValidacaoDados Validar(TceAvaliacaoCurriculoMinimo avaliacaoCurriculoMinimo)
        {
            var validacao = new ValidacaoDados();

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT  1
                    FROM    [TCE_AVALIACAO_CM]
                    WHERE   ANO = @ANO
                            AND PERIODO = @PERIODO
                            AND SUBPERIODO = @SUBPERIODO
                            AND (
                                  AVALIACAO = @AVALIACAO
                                  OR ORDEM = @ORDEM
                                )");

                if (avaliacaoCurriculoMinimo.IdAvaliacaoCurriculoMinimo != 0)
                {
                    contextQuery.Command += " AND ID_AVALIACAO_CM <> @ID_AVALIACAO_CM ";
                }

                contextQuery.Parameters.Add("@ID_AVALIACAO_CM", avaliacaoCurriculoMinimo.IdAvaliacaoCurriculoMinimo);
                contextQuery.Parameters.Add("@AVALIACAO", avaliacaoCurriculoMinimo.Avaliacao);
                contextQuery.Parameters.Add("@ORDEM", avaliacaoCurriculoMinimo.Ordem);
                contextQuery.Parameters.Add("@ANO", avaliacaoCurriculoMinimo.Ano);
                contextQuery.Parameters.Add("@PERIODO", avaliacaoCurriculoMinimo.Periodo);
                contextQuery.Parameters.Add("@SUBPERIODO", avaliacaoCurriculoMinimo.Subperiodo);

                var obj = ctx.GetReturnValue(contextQuery);

                if (obj == null)
                {
                    validacao.Valido = true;
                }
                else
                {
                    validacao.Valido = false;
                    validacao.Mensagem = "Já existe uma Avaliação com este nome/ordem.";
                }
            }

            return validacao;
        }

        private static void GravarLog(DataContext context, string matricula, int ano, int periodo, int subperiodo)
        {
            var contextQuery = new ContextQuery(
                @"INSERT  INTO dbo.TCE_LOG_AVALIACAO_CM_DOCENTE
                        (
                          ID_AVALIACAO_CM_DOCENTE,
                          ID_AVALIACAO_CM,
                          RESPOSTA,
                          MATRICULA,
                          DT_CADASTRO
                                         
                        )
                        SELECT  ID_AVALIACAO_CM_DOCENTE,
                                D.ID_AVALIACAO_CM,
                                RESPOSTA,
                                D.MATRICULA,
                                D.DT_CADASTRO
                        FROM    dbo.TCE_AVALIACAO_CM_DOCENTE D
                                INNER JOIN TCE_AVALIACAO_CM A ON A.ID_AVALIACAO_CM = D.ID_AVALIACAO_CM
                        WHERE   D.MATRICULA = @MATRICULA
                                AND ANO = @ANO
                                AND PERIODO = @PERIODO
                                AND SUBPERIODO = @SUBPERIODO");

            contextQuery.Parameters.Add("@MATRICULA", matricula);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@SUBPERIODO", subperiodo);

            context.ApplyModifications(contextQuery);
        }

        private static void LimparCompetencias(DataContext context, string matricula, int ano, int periodo, int subperiodo)
        {
            var contextQuery = new ContextQuery(
                @"DELETE  FROM TCE_AVALIACAO_CM_DOCENTE
                WHERE   EXISTS ( SELECT 1
                                 FROM   TCE_AVALIACAO_CM
                                 WHERE  ANO = @ANO
                                        AND PERIODO = @PERIODO
                                        AND SUBPERIODO = @SUBPERIODO
                                        AND TCE_AVALIACAO_CM_DOCENTE.ID_AVALIACAO_CM = ID_AVALIACAO_CM )
                        AND MATRICULA = @MATRICULA");

            contextQuery.Parameters.Add("@MATRICULA", matricula);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@SUBPERIODO", subperiodo);

            context.ApplyModifications(contextQuery);
        }
    }
}