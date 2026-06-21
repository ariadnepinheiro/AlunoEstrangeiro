namespace Techne.Lyceum.RN
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Seeduc.Infra.Data;
    using Seeduc.Infra.Mapper;
    using Entidades;
    using Util;

    public class UnidadeEnsinoSituacao : RNBase
    {
        public const string Extincao = "Extincao";

        public const string Paralizacao = "Paralisacao";

        public const string EmProcesso = "EmProcesso";

        public static void Alterar(LyUnidadeEnsinoSituacao unidadeEnsinoSituacao)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery(
                        @"UPDATE  LY_UNIDADE_ENSINO_SITUACAO
                        SET     DT_SITUACAO = @DT_SITUACAO,
                                DT_DOU = @DT_DOU,                                
                                NUMERO_ATO_OFICIAL = @NUMERO_ATO_OFICIAL,
                                OBSERVACAO = @OBSERVACAO,
                                SITUACAO = @SITUACAO,
                                ATO_OFICIAL = @ATO_OFICIAL,
                                MATRICULA = @MATRICULA,
                                DT_ALTERACAO = GETDATE()
                        WHERE   UNIDADE_ENS = @CENSO
                                AND ORDEM = @ORDEM ");

                    contextQuery.Parameters.Add("@CENSO", unidadeEnsinoSituacao.UnidadeEns);
                    contextQuery.Parameters.Add("@ORDEM", unidadeEnsinoSituacao.Ordem);
                    contextQuery.Parameters.Add("@DT_SITUACAO", SqlDbType.DateTime, unidadeEnsinoSituacao.DtSituacao);
                    contextQuery.Parameters.Add("@DT_DOU", SqlDbType.DateTime, unidadeEnsinoSituacao.DtDou);
                    contextQuery.Parameters.Add("@NUMERO_ATO_OFICIAL", unidadeEnsinoSituacao.NumeroAtoOficial);
                    contextQuery.Parameters.Add("@SITUACAO", unidadeEnsinoSituacao.Situacao);
                    contextQuery.Parameters.Add("@ATO_OFICIAL", unidadeEnsinoSituacao.AtoOficial);
                    contextQuery.Parameters.Add("@OBSERVACAO", unidadeEnsinoSituacao.Observacao);
                    contextQuery.Parameters.Add("@MATRICULA", unidadeEnsinoSituacao.Matricula);

                    ctx.ApplyModifications(contextQuery);

                    //var ordemativa = RetornaOrdemAtiva(unidadeEnsinoSituacao.UnidadeEns);

                    //if (ordemativa == unidadeEnsinoSituacao.Ordem
                    //    && (unidadeEnsinoSituacao.AtoOficial.Equals(Paralizacao)
                    //    || unidadeEnsinoSituacao.AtoOficial.Equals(Extincao)))
                    //{
                    //    UnidadeEnsino.AlterarSituacaoFuncionamento(ctx, unidadeEnsinoSituacao.UnidadeEns,
                    //                       unidadeEnsinoSituacao.AtoOficial);
                    //}
                }
                catch (Exception)
                {
                    ctx.Abandon();

                    throw;
                }
            }
        }

        public static LyUnidadeEnsinoSituacao Bind(IDictionary chaves, IDictionary valores)
        {
            var unidadeEnsinoSituacao = DictionaryMapper.CreateAndMapTo<LyUnidadeEnsinoSituacao>(chaves);

            return DictionaryMapper.MapTo(valores, unidadeEnsinoSituacao);
        }

        public static void Inserir(LyUnidadeEnsinoSituacao unidadeEnsinoSituacao)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    unidadeEnsinoSituacao.Ordem = GerarOrdem(ctx, unidadeEnsinoSituacao.UnidadeEns);

                    var contextQuery = new ContextQuery(
                        @"INSERT  INTO LY_UNIDADE_ENSINO_SITUACAO
                                (
                                  UNIDADE_ENS,
                                  ORDEM,
                                  SITUACAO,
                                  DT_SITUACAO,
                                  DT_DOU,
                                  ATO_OFICIAL,
                                  NUMERO_ATO_OFICIAL,
                                  OBSERVACAO,
                                  MATRICULA 
                                )
                        VALUES  (
                                  @UNIDADE_ENS,
                                  @ORDEM,
                                  @SITUACAO,
                                  @DT_SITUACAO,
                                  @DT_DOU,
                                  @ATO_OFICIAL,
                                  @NUMERO_ATO_OFICIAL,
                                  @OBSERVACAO,
                                  @MATRICULA 
                                )");

                    contextQuery.Parameters.Add("@UNIDADE_ENS", unidadeEnsinoSituacao.UnidadeEns);
                    contextQuery.Parameters.Add("@ORDEM", unidadeEnsinoSituacao.Ordem);
                    contextQuery.Parameters.Add("@SITUACAO", unidadeEnsinoSituacao.Situacao);
                    contextQuery.Parameters.Add("@DT_SITUACAO", unidadeEnsinoSituacao.DtSituacao);
                    contextQuery.Parameters.Add("@DT_DOU", unidadeEnsinoSituacao.DtDou);
                    contextQuery.Parameters.Add("@ATO_OFICIAL", unidadeEnsinoSituacao.AtoOficial);
                    contextQuery.Parameters.Add("@NUMERO_ATO_OFICIAL", unidadeEnsinoSituacao.NumeroAtoOficial);
                    contextQuery.Parameters.Add("@OBSERVACAO", unidadeEnsinoSituacao.Observacao);
                    contextQuery.Parameters.Add("@MATRICULA", unidadeEnsinoSituacao.Matricula);

                    ctx.ApplyModifications(contextQuery);

                    var ordemativa = RetornaOrdemAtiva(unidadeEnsinoSituacao.UnidadeEns);
                   
                    if (ordemativa == unidadeEnsinoSituacao.Ordem 
                        && (unidadeEnsinoSituacao.AtoOficial.Equals(Paralizacao)
                        || unidadeEnsinoSituacao.AtoOficial.Equals(Extincao)
                        || unidadeEnsinoSituacao.AtoOficial.Equals(EmProcesso)))
                    {
                        UnidadeEnsino.AlterarSituacaoFuncionamento(ctx, unidadeEnsinoSituacao.UnidadeEns,
                                                                   unidadeEnsinoSituacao.AtoOficial);
                    }
                }
                catch (Exception)
                {
                    ctx.Abandon();

                    throw;
                }
            }
        }

        public static DataTable ListarAtoCriacao(string censo)
        {
            return Consultar(
                new ContextQuery(
                    @" SELECT ES.NUMERO_ATO_OFICIAL, 
                       UE.NOME_COMP 
                FROM   LY_UNIDADE_ENSINO UE 
                       LEFT JOIN LY_UNIDADE_ENSINO_SITUACAO ES 
                              ON UE.UNIDADE_ENS = ES.UNIDADE_ENS 
                WHERE  UE.UNIDADE_ENS = @CENSO 
                       AND ES.ATO_OFICIAL LIKE 'Criacao'",
                    new ContextQueryParameter("@CENSO", censo)));
        }
        public static DataTable ListarAtoCriacaoeRegional(string censo)
        {
            return Consultar(
                new ContextQuery(
                    @" SELECT ES.NUMERO_ATO_OFICIAL, 
                       UE.NOME_COMP ,VW.REGIONAL
                FROM   LY_UNIDADE_ENSINO UE 
                       LEFT JOIN LY_UNIDADE_ENSINO_SITUACAO ES 
                              ON UE.UNIDADE_ENS = ES.UNIDADE_ENS AND ES.ATO_OFICIAL LIKE 'Criacao'
					   LEFT JOIN VW_UNIDADE_ENSINO_SITUACAO_REGIONAL VW
							  ON UE.UNIDADE_ENS=VW.unidade_ens 
                WHERE  UE.UNIDADE_ENS = @CENSO 
                       ",
                    new ContextQueryParameter("@CENSO", censo)));
        }
        public static DataTable Listar(string censo)
        {
            return Consultar(
                new ContextQuery(
                    @"SELECT  *
                    FROM    LY_UNIDADE_ENSINO_SITUACAO
                    WHERE   UNIDADE_ENS = @CENSO
                    ORDER BY DT_SITUACAO DESC",
                    new ContextQueryParameter("@CENSO", censo)));
        }

        public static void Remover(string censo, decimal ordem)
        {
            if (ordem < 1
                || string.IsNullOrEmpty(censo))
            {
                return;
            }
           
            var ordemAtiva = RetornaOrdemAtiva(censo);
           
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery(
                        @"DELETE  LY_UNIDADE_ENSINO_SITUACAO
                        WHERE   UNIDADE_ENS = @CENSO
                                AND ORDEM = @ORDEM ");

                    contextQuery.Parameters.Add("@CENSO", censo);
                    contextQuery.Parameters.Add("@ORDEM", ordem);

                    ctx.ApplyModifications(contextQuery);
                  
                    if (ordemAtiva == ordem)
                    {
                        var atoAtivo = RetornaAtoAtivo(censo);
                        UnidadeEnsino.AlterarSituacaoFuncionamento(ctx, censo, atoAtivo);
                    }
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static ValidacaoDados Validar(LyUnidadeEnsinoSituacao unidadeEnsinoSituacao)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados();

            if (unidadeEnsinoSituacao == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(unidadeEnsinoSituacao.Situacao))
            {
                mensagens.Add("O campo REDE DE ENSINO é obrigatório!");
            }

            if (string.IsNullOrEmpty(unidadeEnsinoSituacao.AtoOficial))
            {
                mensagens.Add("O campo ATO é obrigatório!");
            }

            if (unidadeEnsinoSituacao.DtSituacao == default(DateTime))
            {
                mensagens.Add("O campo DATA DO ATO é obrigatório!");
            }
            else if (unidadeEnsinoSituacao.DtSituacao.Date > DateTime.Now.Date)
            {
                mensagens.Add("O campo DATA DO ATO não pode ser maior que data atual.");
            }

            if (unidadeEnsinoSituacao.DtDou == null)
            {
                mensagens.Add("O campo DATA DO é obrigatório!");
            }

            if (!string.IsNullOrEmpty(unidadeEnsinoSituacao.AtoOficial)
                && unidadeEnsinoSituacao.AtoOficial.ToUpper() != "PARALISACAO")
            {
                if (unidadeEnsinoSituacao.DtDou != null)
                {
                    if (unidadeEnsinoSituacao.DtDou.Value.Date > DateTime.Now.Date)
                    {
                        mensagens.Add("O campo DATA DO não pode ser maior que data atual.");
                    }
                }

                if (string.IsNullOrEmpty(unidadeEnsinoSituacao.NumeroAtoOficial))
                {
                    mensagens.Add("O campo NÚMERO DO ATO OFICIAL é obrigatório!");
                }
            }

            if (mensagens.Count == 0
                && unidadeEnsinoSituacao.Ordem <= 0)
            {
                var contextQuery = new ContextQuery(
                    @" SELECT COUNT(*) FROM LY_UNIDADE_ENSINO_SITUACAO
                               WHERE UNIDADE_ENS = @UNIDADE_ENS
                               AND SITUACAO = @SITUACAO ");

                contextQuery.Parameters.Add("@UNIDADE_ENS", unidadeEnsinoSituacao.UnidadeEns);
                contextQuery.Parameters.Add("@SITUACAO", unidadeEnsinoSituacao.Situacao);

                var quantidade = ExecutarFuncao<int>(contextQuery);

                if (quantidade > 0)
                {
                    mensagens.Add("Já existe uma UNIDADE COMPARTILHADA cadastrada com estes mesmo censo.");
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

        private static decimal GerarOrdem(DataContext ctx, string censo)
        {
            var contextQuery = new ContextQuery(
                @"SELECT ISNULL(MAX(ORDEM), 0) + 1
                FROM   LY_UNIDADE_ENSINO_SITUACAO
                WHERE  UNIDADE_ENS = @CENSO");

            contextQuery.Parameters.Add("@CENSO", censo);

            return ctx.GetReturnValue<decimal>(contextQuery);
        }

        public static bool VerificarSituacaoMunicipalizacao(string unidadeEnsino)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT 1 
                        FROM LY_UNIDADE_ENSINO_SITUACAO
                        WHERE UNIDADE_ENS = @UNIDADE_ENS 
                        AND SITUACAO='MUNICIPALIZADA'");

                contextQuery.Parameters.Add("@UNIDADE_ENS", unidadeEnsino);

                var obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    return true;
                }
            }
            return false;
        }       

        public static int RetornaOrdemAtiva(string unidadeEnsino)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT TOP 1 ORDEM FROM LY_UNIDADE_ENSINO_SITUACAO
                        WHERE UNIDADE_ENS = @UNIDADE_ENS
                        ORDER BY DT_situacao desc");

                contextQuery.Parameters.Add("@UNIDADE_ENS", unidadeEnsino);

                return ctx.GetReturnValue<int>(contextQuery);
            }
        }

        public static string RetornaAtoAtivo(string unidadeEnsino)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT TOP 1
                                ATO_OFICIAL
                        FROM    LY_UNIDADE_ENSINO_SITUACAO
                        WHERE   UNIDADE_ENS = @UNIDADE_ENS
                        ORDER BY DT_SITUACAO DESC ");

                contextQuery.Parameters.Add("@UNIDADE_ENS", unidadeEnsino);

                return ctx.GetReturnValue<string>(contextQuery);
            }
        }
    }
}