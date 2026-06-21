namespace Techne.Lyceum.RN
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Seeduc.Infra.Data;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Lyceum.RN.Util;

    public class RestricaoIdadeSerie : RNBase
    {
        public static void Alterar(TceRestricaoIdadeSerie restricaoIdadeSerie)
        {
            var contextQuery = new ContextQuery(
                @" UPDATE  TCE_RESTRICAO_IDADE_SERIE
                SET     IDADE_MINIMA = @IDADE_MINIMA, 
                        IDADE_MAXIMA = @IDADE_MAXIMA,
                        MATRICULA = @MATRICULA,
                        DT_ALTERACAO = GetDate()
                WHERE   ID_RESTRICAO_IDADE_SERIE = @ID_RESTRICAO_IDADE_SERIE ");

            contextQuery.Parameters.Add("@ID_RESTRICAO_IDADE_SERIE", restricaoIdadeSerie.IdRestricaoIdadeSerie);
            contextQuery.Parameters.Add("@IDADE_MINIMA", restricaoIdadeSerie.IdadeMinima);
            contextQuery.Parameters.Add("@IDADE_MAXIMA", restricaoIdadeSerie.IdadeMaxima);
            contextQuery.Parameters.Add("@MATRICULA", restricaoIdadeSerie.Matricula);

            ExecutarAlteracao(contextQuery);
        }

        public static void Inserir(TceRestricaoIdadeSerie restricaoIdadeSerie)
        {
            var contextQuery = new ContextQuery(
                @"INSERT  INTO TCE_RESTRICAO_IDADE_SERIE ( CURSO, SERIE, IDADE_MINIMA, IDADE_MAXIMA, MATRICULA )
                VALUES  ( @CURSO, @SERIE, @IDADE_MINIMA, @IDADE_MAXIMA, @MATRICULA )");

            contextQuery.Parameters.Add("@CURSO", restricaoIdadeSerie.Curso);
            contextQuery.Parameters.Add("@SERIE", restricaoIdadeSerie.Serie);
            contextQuery.Parameters.Add("@IDADE_MINIMA", restricaoIdadeSerie.IdadeMinima);
            contextQuery.Parameters.Add("@IDADE_MAXIMA", restricaoIdadeSerie.IdadeMaxima);
            contextQuery.Parameters.Add("@MATRICULA", restricaoIdadeSerie.Matricula);

            ExecutarAlteracao(contextQuery);
        }

        public static DataTable Listar()
        {
            var contextQuery = new ContextQuery(
                @" SELECT  ID_RESTRICAO_IDADE_SERIE, R.CURSO AS CODIGO_CURSO, SERIE, IDADE_MINIMA, IDADE_MAXIMA,
                        DT_CADASTRO, DT_ALTERACAO, MATRICULA, M.DESCRICAO AS MODALIDADE,
                        T.DESCRICAO AS SEGMENTO, C.NOME AS NOME_CURSO
                FROM    TCE_RESTRICAO_IDADE_SERIE r
                        INNER JOIN dbo.LY_CURSO c ON r.CURSO = c.CURSO
                        INNER JOIN dbo.LY_MODALIDADE_CURSO m ON c.MODALIDADE = m.MODALIDADE
                        INNER JOIN dbo.LY_TIPO_CURSO T ON C.TIPO = T.TIPO ");

            return Consultar(contextQuery);
        }

        public static void Remover(int id)
        {
            if (id < 1)
            {
                return;
            }

            var contextQuery = new ContextQuery(
                @"DELETE  FROM dbo.TCE_RESTRICAO_IDADE_SERIE
                WHERE   ID_RESTRICAO_IDADE_SERIE = @ID_RESTRICAO_IDADE_SERIE");

            contextQuery.Parameters.Add("@ID_RESTRICAO_IDADE_SERIE", id);

            ExecutarAlteracao(contextQuery);
        }

        public static ValidacaoDados Validar(TceRestricaoIdadeSerie restricaoIdadeSerie)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
                                 {
                                     Valido = false
                                 };

            if (restricaoIdadeSerie == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(restricaoIdadeSerie.Curso))
            {
                mensagens.Add("O campo ESCOLARIDADE é obrigatório!");
            }

            if (restricaoIdadeSerie.Serie <= 0)
            {
                mensagens.Add("O campo SÉRIE é obrigatório!");
            }

            if (restricaoIdadeSerie.IdadeMinima <= 0)
            {
                mensagens.Add("O campo IDADE MÍNIMA é obrigatório e deve ser maior que zero!");
            }

            if (restricaoIdadeSerie.IdadeMaxima <= 0
                || restricaoIdadeSerie.IdadeMaxima <= restricaoIdadeSerie.IdadeMinima)
            {
                mensagens.Add("O campo IDADE MÁXIMA é obrigatório e deve ser maior que a idade mínima!");
            }

            if (mensagens.Count == 0)
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
                {
                    var contextQuery = new ContextQuery(
                        @"SELECT  1
                        FROM    dbo.TCE_RESTRICAO_IDADE_SERIE
                        WHERE   CURSO = @CURSO
                                AND SERIE = @SERIE");

                    contextQuery.Parameters.Add("@CURSO", restricaoIdadeSerie.Curso);
                    contextQuery.Parameters.Add("@SERIE", restricaoIdadeSerie.Serie);

                    var obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        mensagens.Add("Já existe uma RESTRIÇÃO DE IDADE SÉRIE cadastrada com estes mesmos dados.");
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

        public TceRestricaoIdadeSerie CarregaRestricaoPor(string curso, int serie)
        {
            TceRestricaoIdadeSerie restricaoIdadeSerie = new TceRestricaoIdadeSerie();
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                restricaoIdadeSerie = CarregaRestricaoPor(contexto, curso, serie);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                {
                    contexto.Dispose();
                }
            }
            return restricaoIdadeSerie;
        }

        public TceRestricaoIdadeSerie CarregaRestricaoPor(DataContext ctx, string curso, int serie)
        {
            ContextQuery contextQuery = new ContextQuery();
            TceRestricaoIdadeSerie restricaoIdadeSerie = new TceRestricaoIdadeSerie();

            contextQuery.Command = @" SELECT  *
                                FROM    TCE_RESTRICAO_IDADE_SERIE
                                WHERE   CURSO = @CURSO
                                        AND SERIE = @SERIE ";

            contextQuery.Parameters.Add("@CURSO", curso);
            contextQuery.Parameters.Add("@SERIE", serie);

            restricaoIdadeSerie = ctx.TryToBindEntity<TceRestricaoIdadeSerie>(contextQuery);

            return restricaoIdadeSerie;
        }
    }
}