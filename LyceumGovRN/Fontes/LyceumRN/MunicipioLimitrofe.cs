namespace Techne.Lyceum.RN
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Seeduc.Infra.Data;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Lyceum.RN.Util;

    public class MunicipioLimitrofe : RNBase
    {
        public static void Inserir(TceMunicipioLimitrofe municipioLimitrofe)
        {
            var contextQuery = new ContextQuery(
                @"INSERT  INTO TCE_MUNICIPIO_LIMITROFE (CODIGO_MUNICIPIO_LIMITROFE, UF, CODIGO_MUNICIPIO, MATRICULA )
                VALUES  ( @CODIGO_MUNICIPIO_LIMITROFE, @UF, @CODIGO_MUNICIPIO, @MATRICULA )");

            contextQuery.Parameters.Add("@CODIGO_MUNICIPIO_LIMITROFE", municipioLimitrofe.CodigoMunicipioLimitrofe);
            contextQuery.Parameters.Add("@UF", municipioLimitrofe.Uf);
            contextQuery.Parameters.Add("@CODIGO_MUNICIPIO", municipioLimitrofe.CodigoMunicipio);
            contextQuery.Parameters.Add("@MATRICULA", municipioLimitrofe.Matricula);

            ExecutarAlteracao(contextQuery);
        }

        public static DataTable Listar(string codMunicipioOrigem)
        {
            var contextQuery = new ContextQuery(
                @" SELECT  MLI.ID_MUNICIPIO_LIMITROFE,
                       MLI.CODIGO_MUNICIPIO AS COD_ORIGEM,
                       M.NOME AS MUNICIPIO_ORIGEM,
                       M.UF AS UF_ORIGEM,
                       MLI.CODIGO_MUNICIPIO_LIMITROFE ,
                       MLI.UF,
                       ML.NOME AS MUNICIPIO_LIMITROFE
                FROM    dbo.TCE_MUNICIPIO_LIMITROFE MLI
                       INNER JOIN hades.dbo.TCE_MUNICIPIO M ON MLI.CODIGO_MUNICIPIO = M.ID_MUNICIPIO
                       INNER JOIN dbo.MUNICIPIO ML ON ML.CODIGO = MLI.CODIGO_MUNICIPIO_LIMITROFE
                WHERE   MLI.CODIGO_MUNICIPIO = @CODIGO_MUNICIPIO ");

            contextQuery.Parameters.Add("@CODIGO_MUNICIPIO", codMunicipioOrigem);

            return Consultar(contextQuery);
        }

        public static ValidacaoDados Validar(TceMunicipioLimitrofe municipioLimitrofe)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
                                 {
                                     Valido = false
                                 };

            if (municipioLimitrofe == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(municipioLimitrofe.CodigoMunicipio))
            {
                mensagens.Add("O campo MUNICÍPIO ORIGEM é obrigatório!");
            }

            if (string.IsNullOrEmpty(municipioLimitrofe.Uf))
            {
                mensagens.Add("O campo UF é obrigatório!");
            }

            if (string.IsNullOrEmpty(municipioLimitrofe.CodigoMunicipioLimitrofe))
            {
                mensagens.Add("O campo MUNICÍPIO LIMÍTROFE é obrigatório!");
            }

            if (!string.IsNullOrEmpty(municipioLimitrofe.CodigoMunicipioLimitrofe)
                && !string.IsNullOrEmpty(municipioLimitrofe.CodigoMunicipio)
                && municipioLimitrofe.CodigoMunicipioLimitrofe == municipioLimitrofe.CodigoMunicipio)
            {
                mensagens.Add("O campo MUNICÍPIO LIMÍTROFE não pode ser igual ao MUNICÍPIO ORIGEM!");
            }

            if (mensagens.Count == 0)
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
                {
                    var contextQuery = new ContextQuery(
                        @"SELECT  1
                        FROM    dbo.TCE_MUNICIPIO_LIMITROFE
                        WHERE   CODIGO_MUNICIPIO_LIMITROFE = @CODIGO_MUNICIPIO_LIMITROFE
                                AND CODIGO_MUNICIPIO = @CODIGO_MUNICIPIO");

                    contextQuery.Parameters.Add("@CODIGO_MUNICIPIO_LIMITROFE", municipioLimitrofe.CodigoMunicipioLimitrofe);
                    contextQuery.Parameters.Add("@CODIGO_MUNICIPIO", municipioLimitrofe.CodigoMunicipio);

                    var obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        mensagens.Add("Já existe um MUNICIPIO LIMÍTROFE cadastrado com estes mesmos dados.");
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + System.Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public static void Remover(int id)
        {
            if (id < 1)
            {
                return;
            }

            var contextQuery = new ContextQuery(
                @"DELETE  TCE_MUNICIPIO_LIMITROFE
                WHERE   ID_MUNICIPIO_LIMITROFE = @ID_MUNICIPIO_LIMITROFE");

            contextQuery.Parameters.Add("@ID_MUNICIPIO_LIMITROFE", id);

            ExecutarAlteracao(contextQuery);
        }
    }
}