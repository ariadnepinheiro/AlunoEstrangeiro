namespace Techne.Lyceum.RN
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Seeduc.Infra.Data;
    using Entidades;
    using Util;

    public class Documento : RNBase
    {
        public static void Alterar(TceDocumento documento)
        {
            var contextQuery = new ContextQuery(
                @"UPDATE  TCE_DOCUMENTO
                SET     DESCRICAO = @DESCRICAO, 
		                PRAZO = @PRAZO, 
		                OBRIGATORIO = @OBRIGATORIO,
                        DT_ALTERACAO = GETDATE(), 
                        MATRICULA = @MATRICULA
                WHERE   ID_DOCUMENTO = @ID_DOCUMENTO ");

            contextQuery.Parameters.Add("@ID_DOCUMENTO", documento.IdDocumento);
            contextQuery.Parameters.Add("@DESCRICAO", documento.Descricao);
            contextQuery.Parameters.Add("@PRAZO", documento.Prazo);
            contextQuery.Parameters.Add("@OBRIGATORIO", documento.Obrigatorio);
            contextQuery.Parameters.Add("@MATRICULA", documento.Matricula);

            ExecutarAlteracao(contextQuery);
        }

        public static void Inserir(TceDocumento documento)
        {
            var contextQuery = new ContextQuery(
                @"INSERT  INTO TCE_DOCUMENTO ( ANO, PERIODO, DESCRICAO, PRAZO, OBRIGATORIO, MATRICULA )
                VALUES  ( @ANO, @PERIODO, @DESCRICAO, @PRAZO, @OBRIGATORIO, @MATRICULA )");

            contextQuery.Parameters.Add("@ANO", documento.Ano);
            contextQuery.Parameters.Add("@PERIODO", documento.Periodo);
            contextQuery.Parameters.Add("@DESCRICAO", documento.Descricao);
            contextQuery.Parameters.Add("@PRAZO", documento.Prazo);
            contextQuery.Parameters.Add("@OBRIGATORIO", documento.Obrigatorio);
            contextQuery.Parameters.Add("@MATRICULA", documento.Matricula);

            ExecutarAlteracao(contextQuery);
        }

        public static DataTable Listar(int ano, int periodo)
        {
            var contextQuery = new ContextQuery(
                @" SELECT  ID_DOCUMENTO, ANO, PERIODO, DESCRICAO, PRAZO, OBRIGATORIO, DT_CADASTRO,
                        DT_ALTERACAO, MATRICULA
                FROM    dbo.TCE_DOCUMENTO
                WHERE   ANO = @ANO
                        AND PERIODO = @PERIODO ");

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);

            return Consultar(contextQuery);
        }

        public static void Remover(int id)
        {
            if (id < 1)
            {
                return;
            }

            var contextQuery = new ContextQuery(
                @"DELETE FROM TCE_DOCUMENTO
                WHERE ID_DOCUMENTO = @ID_DOCUMENTO");

            contextQuery.Parameters.Add("@ID_DOCUMENTO", id);

            ExecutarAlteracao(contextQuery);
        }

        public static ValidacaoDados Validar(TceDocumento documento)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
                                 {
                                     Valido = false
                                 };

            if (documento == null)
            {
                return validacaoDados;
            }

            if (documento.Ano <= 0)
            {
                mensagens.Add("O campo ANO LETIVO é obrigatório!");
            }

            if (documento.Periodo < 0)
            {
                mensagens.Add("O campo PERÍODO é obrigatório!");
            }

            if (string.IsNullOrEmpty(documento.Descricao)
                || documento.Descricao.Length > 100)
            {
                mensagens.Add("O campo DOCUMENTO é obrigatório e deve ter no máximo 100 caracteres!");
            }

            if (documento.Prazo <= 0)
            {
                mensagens.Add("O campo PRAZO PARA ENTREGA é obrigatório e deve ser maior que zero!");
            }

            if (mensagens.Count == 0)
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    var contextQuery = new ContextQuery(
                        @"SELECT  1
                        FROM    dbo.TCE_DOCUMENTO
                        WHERE   ANO = @ANO
                                AND PERIODO = @PERIODO
                                AND DESCRICAO = @DESCRICAO");

                    contextQuery.Parameters.Add("@ANO", documento.Ano);
                    contextQuery.Parameters.Add("@PERIODO", documento.Periodo);
                    contextQuery.Parameters.Add("@DESCRICAO", documento.Descricao);

                    var obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        mensagens.Add("Já existe um DOCUMENTO cadastrado com estes mesmos dados.");
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

        public static ValidacaoDados ValidarRemover(int id)
        {
            var validacaoDados = new ValidacaoDados
                                 {
                                     Valido = false
                                 };

            if (id == 0)
            {
                return validacaoDados;
            }

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT  1
                    FROM    dbo.TCE_DOCUMENTO_ALUNO
                    WHERE   ID_DOCUMENTO = @ID_DOCUMENTO");

                contextQuery.Parameters.Add("@ID_DOCUMENTO", id);

                var obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    validacaoDados.Mensagem = "Não é permitido realizar a exclusão deste documento, pois ele está sendo utilizado.";
                }
            }

            if (string.IsNullOrEmpty(validacaoDados.Mensagem))
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }
    }
}