using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Seeduc.Infra.Data;
using Seeduc.Infra.Mapper;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using System.Collections;

namespace Techne.Lyceum.RN
{
    public class DocCelebradoMunicipalizacao : RNBase
    {
        public static DataTable Listar(string censo)
        {
            var contextQuery = new ContextQuery(
                @" SELECT  DC.*,
		                M.PROCESSO,
		                M.DT_PUBLICACAO_DO,
		                M.PAGINA_DO
                    FROM    dbo.TCE_DOC_CELEBRADO_MUNICIPALIZACAO dc
                    INNER JOIN dbo.TCE_MUNICIPALIZACAO m ON dc.ID_MUNICIPALIZACAO = m.ID_MUNICIPALIZACAO
                    WHERE   m.CENSO = @CENSO ");
            contextQuery.Parameters.Add("@CENSO", censo);


            return Consultar(contextQuery);
        }

        public ValidacaoDados Validar(TceDocCelebradoMunicipalizacao docCelebradoMunicipalizacao)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (docCelebradoMunicipalizacao == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(docCelebradoMunicipalizacao.Tipo))
            {
                mensagens.Add("O campo TIPO é obrigatório!");
            }

            if (string.IsNullOrEmpty(docCelebradoMunicipalizacao.Numero))
            {
                mensagens.Add("O campo NÚMERO é obrigatório!");
            }
            else
            {
                if (docCelebradoMunicipalizacao.Numero.Length > 50)
                {
                    mensagens.Add("O campo NÚMERO é obrigatório!");
                }
            }

            if (docCelebradoMunicipalizacao.DtCelebracao <= DateTime.MinValue)
            {
                mensagens.Add("O campo DATA DE VALIDADE - INÍCIO é obrigatório!");
            }

            if (docCelebradoMunicipalizacao.DtValidade <= DateTime.MinValue)
            {
                mensagens.Add("O campo DATA DE VALIDADE - FINAL é obrigatório!");
            }

            if (!docCelebradoMunicipalizacao.Observacao.IsNullOrEmptyOrWhiteSpace() && docCelebradoMunicipalizacao.Observacao.Length > 1000)
            {
                mensagens.Add("O campo OBSERVAÇÃO deve ter no máximo 1000 caracteres!");
            }

            if (mensagens.Count == 0)
            {
                if (docCelebradoMunicipalizacao.DtValidade < docCelebradoMunicipalizacao.DtCelebracao)
                {
                    mensagens.Add("O campo DATA DE VALIDADE - FINAL não pode ser antes da DATA DE VALIDADE - INÍCIO!");
                }
            }

            if (mensagens.Count == 0 && docCelebradoMunicipalizacao.IdDocCelebradoMunicipalizacao <= 0)
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    var contextQuery = new ContextQuery(
                        @"SELECT  1
                            FROM  dbo.TCE_DOC_CELEBRADO_MUNICIPALIZACAO
                            WHERE  NUMERO = @NUMERO 
                            AND TIPO = @TIPO ");

                    contextQuery.Parameters.Add("@NUMERO", docCelebradoMunicipalizacao.Numero);
                    contextQuery.Parameters.Add("@TIPO", docCelebradoMunicipalizacao.Tipo);

                    var obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        mensagens.Add("Já existe uma DOCUMENTO CELEBRADO cadastrado com este mesmo número e tipo.");
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

        public void Inserir(TceDocCelebradoMunicipalizacao docCelebradoMunicipalizacao)
        {
            var contextQuery = new ContextQuery(
                @" INSERT INTO TCE_DOC_CELEBRADO_MUNICIPALIZACAO
                                        (ID_MUNICIPALIZACAO,
                                         TIPO,
                                         NUMERO,
                                         DT_CELEBRACAO,
                                         DT_VALIDADE,
                                         MATRICULA,
			                             OBSERVACAO)
                            VALUES      ( @ID_MUNICIPALIZACAO,
                                          @TIPO,
                                          @NUMERO,
                                          @DT_CELEBRACAO,
                                          @DT_VALIDADE,
                                          @MATRICULA,
			                              @OBSERVACAO ) ");

            contextQuery.Parameters.Add("@ID_MUNICIPALIZACAO", docCelebradoMunicipalizacao.IdMunicipalizacao);
            contextQuery.Parameters.Add("@TIPO", docCelebradoMunicipalizacao.Tipo);
            contextQuery.Parameters.Add("@NUMERO", docCelebradoMunicipalizacao.Numero);
            contextQuery.Parameters.Add("@DT_CELEBRACAO", docCelebradoMunicipalizacao.DtCelebracao);
            contextQuery.Parameters.Add("@DT_VALIDADE", docCelebradoMunicipalizacao.DtValidade);
            contextQuery.Parameters.Add("@MATRICULA", docCelebradoMunicipalizacao.Matricula);
            contextQuery.Parameters.Add("@OBSERVACAO", docCelebradoMunicipalizacao.Observacao);

            ExecutarAlteracao(contextQuery);
        }

        public void Alterar(TceDocCelebradoMunicipalizacao docCelebradoMunicipalizacao)
        {
            var contextQuery = new ContextQuery(
            @" UPDATE  TCE_DOC_CELEBRADO_MUNICIPALIZACAO
                SET     TIPO = @TIPO, 
		                NUMERO = @NUMERO, 
		                DT_CELEBRACAO = @DT_CELEBRACAO,
                        DT_VALIDADE = @DT_VALIDADE, 
                        MATRICULA = @MATRICULA,
                        DT_ALTERACAO = GETDATE(),
                        OBSERVACAO = @OBSERVACAO
                WHERE   ID_DOC_CELEBRADO_MUNICIPALIZACAO = @ID_DOC_CELEBRADO_MUNICIPALIZACAO ");

            contextQuery.Parameters.Add("@ID_DOC_CELEBRADO_MUNICIPALIZACAO", docCelebradoMunicipalizacao.IdDocCelebradoMunicipalizacao);
            contextQuery.Parameters.Add("@TIPO", docCelebradoMunicipalizacao.Tipo);
            contextQuery.Parameters.Add("@NUMERO", docCelebradoMunicipalizacao.Numero);
            contextQuery.Parameters.Add("@DT_CELEBRACAO", docCelebradoMunicipalizacao.DtCelebracao);
            contextQuery.Parameters.Add("@DT_VALIDADE", docCelebradoMunicipalizacao.DtValidade);
            contextQuery.Parameters.Add("@MATRICULA", docCelebradoMunicipalizacao.Matricula);
            contextQuery.Parameters.Add("@OBSERVACAO", docCelebradoMunicipalizacao.Observacao);

            ExecutarAlteracao(contextQuery);
        }

        public ValidacaoDados ValidaRemocao(int id)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (id <= 0)
            {
                mensagens.Add("O campo CODIGO é obrigatório!");
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

        public void Remover(int id)
        {
            if (id < 1)
            {
                return;
            }

            var contextQuery = new ContextQuery(
                @"DELETE  TCE_DOC_CELEBRADO_MUNICIPALIZACAO
                    WHERE   ID_DOC_CELEBRADO_MUNICIPALIZACAO = @ID_DOC_CELEBRADO_MUNICIPALIZACAO");

            contextQuery.Parameters.Add("@ID_DOC_CELEBRADO_MUNICIPALIZACAO", id);

            ExecutarAlteracao(contextQuery);
        }

        public static TceDocCelebradoMunicipalizacao Bind(IDictionary chaves, IDictionary valores)
        {
            var docCelebrado = DictionaryMapper.CreateAndMapTo<TceDocCelebradoMunicipalizacao>(chaves);

            return DictionaryMapper.MapTo(valores, docCelebrado);
        }
    }
}
