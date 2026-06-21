namespace Techne.Lyceum.RN
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Seeduc.Infra.Data;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Lyceum.RN.Util;

    public class UnidadeFisicaEdificacao : RNBase
    {
        public static ValidacaoDados Validar(LyUnidadeFisicaEdificacao unidadeFisicaEdificacao, bool verificarDuplicados)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (unidadeFisicaEdificacao == null)
                return validacaoDados;

            ValidaCampos(unidadeFisicaEdificacao, mensagens);

            if (verificarDuplicados)
                ValidaDuplicado(unidadeFisicaEdificacao, mensagens);

            if (mensagens.Count > 0)
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            else
                validacaoDados.Valido = true;

            return validacaoDados;
        }

        private static void ValidaDuplicado(LyUnidadeFisicaEdificacao unidadeFisicaEdificacao, List<string> mensagens)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT  1 
                      FROM LY_UNIDADE_FISICA_EDIFICACAO
                      WHERE 
                        UNIDADE_FIS = @UNIDADE_FIS
                        AND EDIFICACAO = @EDIFICACAO
                        AND PAVIMENTO = @PAVIMENTO ");

                contextQuery.Parameters.Add("@UNIDADE_FIS", unidadeFisicaEdificacao.UnidadeFis);
                contextQuery.Parameters.Add("@EDIFICACAO", unidadeFisicaEdificacao.Edificacao);
                contextQuery.Parameters.Add("@PAVIMENTO", unidadeFisicaEdificacao.Pavimento);                

                var obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                    mensagens.Add("Edificação e pavimento já cadastrados para esta unidade física.");
            }
        }

        private static void ValidaCampos(LyUnidadeFisicaEdificacao unidadeFisicaEdificacao, List<string> mensagens)
        {
            if (string.IsNullOrEmpty(unidadeFisicaEdificacao.UnidadeFis))
            {
                mensagens.Add("O campo UNIDADE FÍSICA é obrigatório!");
            }

            if (string.IsNullOrEmpty(unidadeFisicaEdificacao.Edificacao))
            {
                mensagens.Add("O campo EDIFICAÇÃO é obrigatório!");
            }

            if (string.IsNullOrEmpty(unidadeFisicaEdificacao.NomeEdificacao))
            {
                mensagens.Add("O campo NOME DA EDIFICAÇÃO é obrigatório!");
            }

            if (string.IsNullOrEmpty(unidadeFisicaEdificacao.Pavimento))
            {
                mensagens.Add("O campo PAVIMENTO é obrigatório!");
            }

            if (string.IsNullOrEmpty(unidadeFisicaEdificacao.NomePavimento))
            {
                mensagens.Add("O campo NOME DO PAVIMENTO é obrigatório!");
            }
        }

        public static void Inserir(LyUnidadeFisicaEdificacao unidadeFisicaEdificacao)
        {
            var contextQuery = new ContextQuery(
                @" INSERT  INTO dbo.LY_UNIDADE_FISICA_EDIFICACAO
                            ( UNIDADE_FIS ,
                              EDIFICACAO ,
                              NOME_EDIFICACAO ,
                              PAVIMENTO ,
                              NOME_PAVIMENTO ,
                              MATRICULA 
                            )
                    VALUES  ( @UNIDADE_FIS ,
                              @EDIFICACAO ,
                              @NOME_EDIFICACAO ,
                              @PAVIMENTO ,
                              @NOME_PAVIMENTO ,
                              @MATRICULA
                            ) ");

            contextQuery.Parameters.Add("@UNIDADE_FIS", unidadeFisicaEdificacao.UnidadeFis);
            contextQuery.Parameters.Add("@EDIFICACAO", unidadeFisicaEdificacao.Edificacao);
            contextQuery.Parameters.Add("@NOME_EDIFICACAO", unidadeFisicaEdificacao.NomeEdificacao);
            contextQuery.Parameters.Add("@PAVIMENTO", unidadeFisicaEdificacao.Pavimento);
            contextQuery.Parameters.Add("@NOME_PAVIMENTO", unidadeFisicaEdificacao.NomePavimento);
            contextQuery.Parameters.Add("@MATRICULA", unidadeFisicaEdificacao.Matricula);

            ExecutarAlteracao(contextQuery);
        }

        public static void Alterar(LyUnidadeFisicaEdificacao unidadeFisicaEdificacao)
        {
            var contextQuery = new ContextQuery(
                @"UPDATE  dbo.LY_UNIDADE_FISICA_EDIFICACAO
                    SET     NOME_EDIFICACAO = @NOME_EDIFICACAO ,
                            NOME_PAVIMENTO = @NOME_PAVIMENTO ,
                            MATRICULA = @MATRICULA ,
                            DT_ALTERACAO = GETDATE()
                    WHERE   UNIDADE_FIS = @UNIDADE_FIS
                            AND EDIFICACAO = @EDIFICACAO
                            AND PAVIMENTO = @PAVIMENTO ");

            contextQuery.Parameters.Add("@UNIDADE_FIS", unidadeFisicaEdificacao.UnidadeFis);
            contextQuery.Parameters.Add("@EDIFICACAO", unidadeFisicaEdificacao.Edificacao);
            contextQuery.Parameters.Add("@NOME_EDIFICACAO", unidadeFisicaEdificacao.NomeEdificacao);
            contextQuery.Parameters.Add("@PAVIMENTO", unidadeFisicaEdificacao.Pavimento);
            contextQuery.Parameters.Add("@NOME_PAVIMENTO", unidadeFisicaEdificacao.NomePavimento);
            contextQuery.Parameters.Add("@MATRICULA", unidadeFisicaEdificacao.Matricula);

            ExecutarAlteracao(contextQuery);
        }

        public static ValidacaoDados ValidarRemover(string faculdade, string edificacao, string pavimento)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (string.IsNullOrEmpty(faculdade))
            {
                mensagens.Add("A UNIDADE FÍSICA é obrigatória.");
            }

            if (string.IsNullOrEmpty(edificacao))
            {
                mensagens.Add("A EDIFICAÇÃO é obrigatória.");
            }

            if (string.IsNullOrEmpty(pavimento))
            {
                mensagens.Add("A PAVIMENTO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    var contextQuery = new ContextQuery(
                        @"SELECT  1
                            FROM    LY_DEPENDENCIA
                            WHERE   EDIFICACAO = @EDIFICACAO
                                    AND PAVIMENTO = @PAVIMENTO
                                    AND FACULDADE = @FACULDADE ");

                    contextQuery.Parameters.Add("@EDIFICACAO", edificacao);
                    contextQuery.Parameters.Add("@PAVIMENTO", pavimento);
                    contextQuery.Parameters.Add("@FACULDADE", faculdade);

                    var obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        mensagens.Add("Não é possível excluir esta edificação pois existe uma dependência vinculada.");
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public static void Remover(string censo, string edificacao, string pavimento)
        {
            if (string.IsNullOrEmpty(censo)
                || string.IsNullOrEmpty(edificacao)
                || string.IsNullOrEmpty(pavimento))
            {
                return;
            }

            var contextQuery = new ContextQuery(
                @"DELETE  LY_UNIDADE_FISICA_EDIFICACAO
                     WHERE   UNIDADE_FIS = @UNIDADE_FIS
                            AND EDIFICACAO = @EDIFICACAO
                            AND PAVIMENTO = @PAVIMENTO ");

            contextQuery.Parameters.Add("@UNIDADE_FIS", censo);
            contextQuery.Parameters.Add("@EDIFICACAO", edificacao);
            contextQuery.Parameters.Add("@PAVIMENTO", pavimento);

            ExecutarAlteracao(contextQuery);
        }

        public static LyUnidadeFisicaEdificacao Carregar(string censo, string edificacao, string pavimento)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery =
                    new ContextQuery(
                        @"SELECT *
                                    FROM    LY_UNIDADE_FISICA_EDIFICACAO
                                    WHERE   UNIDADE_FIS = @UNIDADE_FIS
                            AND EDIFICACAO = @EDIFICACAO
                            AND PAVIMENTO = @PAVIMENTO ");

                contextQuery.Parameters.Add("@UNIDADE_FIS", censo);
                contextQuery.Parameters.Add("@EDIFICACAO", edificacao);
                contextQuery.Parameters.Add("@PAVIMENTO", pavimento);

                return ctx.TryToBindEntity<LyUnidadeFisicaEdificacao>(contextQuery);
            }
        }

        public static DataTable ConsultarEdificaoPavimento(string unidade)
        {

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(@"SELECT EDIFICACAO + '/' + PAVIMENTO AS codigo,
	                    NOME_EDIFICACAO + '/' + NOME_PAVIMENTO AS descricao
                    FROM    ly_unidade_fisica_edificacao FE
                    INNER JOIN dbo.LY_UNIDADES_ASSOCIADAS UA ON FE.UNIDADE_FIS=UA.UNIDADE_FIS
                    WHERE UNIDADE_ENS = @UNIDADE_ENS");

                contextQuery.Parameters.Add("@UNIDADE_ENS", unidade);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ConsultarEdificacoes(object unidade)
        {
            if (unidade == null
                || string.IsNullOrEmpty((string)unidade))
            {
                return null;
            }

            return Consultar(
                new ContextQuery(
                    @"SELECT  ufe.*
                    FROM    LY_UNIDADE_FISICA_EDIFICACAO ufe
                    WHERE   UNIDADE_FIS = @UNIDADE",
                    new ContextQueryParameter("@UNIDADE", unidade.ToString())));
        }

        public static DataTable ConsultarPavimentos(string unidade, string edificacao)
        {
            return Consultar(
                new ContextQuery(
                    @"SELECT  ufe.PAVIMENTO,
                            ufe.NOME_PAVIMENTO
                    FROM    LY_UNIDADE_FISICA_EDIFICACAO ufe
                         
                    WHERE   UNIDADE_FIS = @UNIDADE
                            AND ufe.EDIFICACAO = @EDIFICACAO ",
                    new ContextQueryParameter("@UNIDADE", unidade),
                    new ContextQueryParameter("@EDIFICACAO", edificacao)));
        }

        public static DataTable ListarPorUnidFisica(string unidadeFisica)
        {

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT *
                                    FROM    LY_UNIDADE_FISICA_EDIFICACAO 
                                    WHERE   UNIDADE_FIS = @UNIDADE_FIS "

                };
                contextQuery.Parameters.Add("@UNIDADE_FIS", unidadeFisica);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarPorUnidEnsino(string censo)
        {

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT *
                                    FROM    LY_UNIDADE_FISICA_EDIFICACAO e
                                    INNER JOIN dbo.LY_UNIDADES_ASSOCIADAS ua
                                    ON e.UNIDADE_FIS = ua.UNIDADE_FIS
                                    WHERE   ua.UNIDADE_ENS = @CENSO "

                };
                contextQuery.Parameters.Add("@CENSO", censo);

                return ctx.GetDataTable(contextQuery);
            }
        }
    }
}
