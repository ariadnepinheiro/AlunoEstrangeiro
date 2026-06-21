namespace Techne.Lyceum.RN
{
    using System.Data;
    using Seeduc.Infra.Data;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Lyceum.RN.Util;

    public class AvaliacaoCurriculoMinimoJustificativa : RNBase
    {
        public static TceAvaliacaoCurriculoMinimoJustificativa Carregar(int ano, int periodo, int subperiodo, string matricula)
        {
            var acmj = new TceAvaliacaoCurriculoMinimoJustificativa();

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT  *
                    FROM    TCE_AVALIACAO_CM_JUSTIFICATIVA
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
                        acmj.IdAvaliacaoCurriculoMinimoJustificativa = (int)reader["ID_AVALIACAO_CM_JUSTIFICATIVA"];
                        acmj.Justificativa = (string)reader["JUSTIFICATIVA"];
                    }
                }

                return acmj;
            }
        }

        public static void Inserir(TceAvaliacaoCurriculoMinimoJustificativa acmJustificatica)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                // Passo 1: Gravar log das competências antigas, caso existam
                // OBS: Se possível, encapsular de forma organizada e não passar
                // um monte de argumentos
                GravarLog(ctx, acmJustificatica.Ano, acmJustificatica.Periodo, acmJustificatica.Subperiodo, acmJustificatica.Matricula);

                // Passo 2: Limpar todas as entradas das competências antigas
                // OBS: Se possível, encapsular de forma organizada e não passar
                // um monte de argumentos
                LimparJustificativa(ctx, acmJustificatica.Ano, acmJustificatica.Periodo, acmJustificatica.Subperiodo, acmJustificatica.Matricula);

                var contextQuery = new ContextQuery(
                    @"INSERT  INTO TCE_AVALIACAO_CM_JUSTIFICATIVA
                            (
                              ANO,
                              PERIODO,
                              SUBPERIODO,
                              JUSTIFICATIVA,
                              MATRICULA
                            )
                    VALUES  (
                              @ANO,
                              @PERIODO,
                              @SUBPERIODO,
                              @JUSTIFICATIVA,
                              @MATRICULA
                            )");

                contextQuery.Parameters.Add("@ANO", acmJustificatica.Ano);
                contextQuery.Parameters.Add("@PERIODO", acmJustificatica.Periodo);
                contextQuery.Parameters.Add("@SUBPERIODO", acmJustificatica.Subperiodo);
                contextQuery.Parameters.Add("@JUSTIFICATIVA", acmJustificatica.Justificativa);
                contextQuery.Parameters.Add("@MATRICULA", acmJustificatica.Matricula);

                ctx.ApplyModifications(contextQuery);
            }
        }

        public static DataTable Listar(decimal ano, decimal periodo)
        {
            var contextQuery = new ContextQuery(
                @"SELECT  *
                FROM    ID_AVALIACAO_CM_JUSTIFICATIVA
                WHERE   ANO = @ANO
                        AND PERIODO = @PERIODO");

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);

            return Consultar(contextQuery);
        }

        public static ValidacaoDados Validar(TceAvaliacaoCurriculoMinimo avaliacaoCurriculoMinimo)
        {
            var validacao = new ValidacaoDados();

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT  1
                    FROM    TCE_AVALIACAO_CM
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

        public static ValidacaoDados ValidarExclusao(TceAvaliacaoCurriculoMinimo avaliacaoCurriculoMinimo)
        {
            var validacao = new ValidacaoDados();

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT DISTINCT
                            1
                    FROM    [dbo].[TCE_AVALIACAO_CM_DOCENTE]
                    WHERE   ID_AVALIACAO_CM = @ID");

                contextQuery.Parameters.Add("@ID", avaliacaoCurriculoMinimo.IdAvaliacaoCurriculoMinimo);

                var obj = ctx.GetReturnValue(contextQuery);

                if (obj == null)
                {
                    validacao.Valido = true;
                }
                else
                {
                    validacao.Valido = false;
                    validacao.Mensagem = "Esta avaliação não pode ser excluída devido existir currículo mínimo vinculado.";
                }
            }

            return validacao;
        }

        private static void GravarLog(DataContext context, int ano, int periodo, int subperiodo, string matricula)
        {
            var contextQuery = new ContextQuery(
                @"INSERT  INTO dbo.TCE_LOG_AVALIACAO_CM_JUSTIFICATIVA
                            (
                              ID_AVALIACAO_CM_JUSTIFICATIVA,
                              ANO,
                              PERIODO,
                              SUBPERIODO,
                              JUSTIFICATIVA,
                              MATRICULA,
                              DT_CADASTRO
                                                
                            )
                            SELECT  ID_AVALIACAO_CM_JUSTIFICATIVA,
                                    ANO,
                                    PERIODO,
                                    SUBPERIODO,
                                    JUSTIFICATIVA,
                                    MATRICULA,
                                    DT_CADASTRO
                            FROM    dbo.TCE_AVALIACAO_CM_JUSTIFICATIVA
                            WHERE   MATRICULA = @MATRICULA
                                    AND ANO = @ANO
                                    AND PERIODO = @PERIODO
                                    AND SUBPERIODO = @SUBPERIODO");

            contextQuery.Parameters.Add("@MATRICULA", matricula);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@SUBPERIODO", subperiodo);

            context.ApplyModifications(contextQuery);
        }

        private static void LimparJustificativa(DataContext context, int ano, int periodo, int subperiodo, string matricula)
        {
            var contextQuery = new ContextQuery(
                @"DELETE  FROM dbo.TCE_AVALIACAO_CM_JUSTIFICATIVA
                WHERE   MATRICULA = @MATRICULA
                        AND ANO = @ANO
                        AND PERIODO = @PERIODO
                        AND SUBPERIODO = @SUBPERIODO");

            contextQuery.Parameters.Add("@MATRICULA", matricula);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@SUBPERIODO", subperiodo);

            context.ApplyModifications(contextQuery);
        }
    }
}