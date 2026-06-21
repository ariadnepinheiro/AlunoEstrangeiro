using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN
{
    public class TransporteUnid : RNBase
    {
        public static ValidacaoDados Validar(LyTransporteUnid transporteUnid)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (transporteUnid == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(transporteUnid.UnidadeFis))
            {
                mensagens.Add("O campo UNIDADE FÍSICA é obrigatório!");
            }

            if (string.IsNullOrEmpty(transporteUnid.TransporteTipo))
            {
                mensagens.Add("O campo TIPO DE TRANSPORTE é obrigatório!");
            }

            if (mensagens.Count == 0)
            {
                var contextQuery = new ContextQuery(
                        @" SELECT  COUNT(*)
                        FROM    LY_TRANSPORTE_UNID
                        WHERE   UNIDADE_FIS = @UNIDADE_FIS
                                AND TRANSPORTE_TIPO = @TRANSPORTE_TIPO ");
                contextQuery.Parameters.Add("@UNIDADE_FIS", transporteUnid.UnidadeFis);
                contextQuery.Parameters.Add("@TRANSPORTE_TIPO", transporteUnid.TransporteTipo);

                var quantidade = ExecutarFuncao<int>(contextQuery);

                if (quantidade > 0)
                {
                    mensagens.Add("Este transporte já foi cadastrado para esta unidade física.");
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

        public static void Inserir(LyTransporteUnid transporteUnid)
        {
            var contextQuery = new ContextQuery(
                @" INSERT  INTO dbo.LY_TRANSPORTE_UNID
                            ( TRANSPORTE_TIPO ,
                              UNIDADE_FIS ,
                              MATRICULA 
                            )
                    VALUES  ( @TRANSPORTE_TIPO ,
                              @UNIDADE_FIS ,
                              @MATRICULA 
                            ) ");

            contextQuery.Parameters.Add("@TRANSPORTE_TIPO", transporteUnid.TransporteTipo);
            contextQuery.Parameters.Add("@UNIDADE_FIS", transporteUnid.UnidadeFis);
            contextQuery.Parameters.Add("@MATRICULA", transporteUnid.Matricula);

            ExecutarAlteracao(contextQuery);
        }

        public static void Remover(string censo, string transporteTipo)
        {
            if (string.IsNullOrEmpty(censo)
                || string.IsNullOrEmpty(transporteTipo))
            {
                return;
            }

            var contextQuery = new ContextQuery(
                @"DELETE  LY_TRANSPORTE_UNID
                     WHERE   UNIDADE_FIS = @UNIDADE_FIS
                            AND TRANSPORTE_TIPO = @TRANSPORTE_TIPO ");

            contextQuery.Parameters.Add("@TRANSPORTE_TIPO", transporteTipo);
            contextQuery.Parameters.Add("@UNIDADE_FIS", censo);

            ExecutarAlteracao(contextQuery);
        }

        public static LyTransporteUnid Carregar(string censo, string transporteTipo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery =
                    new ContextQuery(
                        @"SELECT *
                                    FROM    LY_TRANSPORTE_UNID
                                    WHERE   UNIDADE_FIS = @UNIDADE_FIS
                            AND TRANSPORTE_TIPO = @TRANSPORTE_TIPO ");

                contextQuery.Parameters.Add("@TRANSPORTE_TIPO", transporteTipo);
                contextQuery.Parameters.Add("@UNIDADE_FIS", censo);

                return ctx.TryToBindEntity<LyTransporteUnid>(contextQuery);
            }
        }

        public static DataTable Listar(string unidadeFis)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT  *
                        FROM    dbo.LY_TRANSPORTE_UNID
                        where UNIDADE_FIS = @UNIDADE_FIS
                        order by TRANSPORTE_TIPO "

                };
                contextQuery.Parameters.Add("@UNIDADE_FIS", unidadeFis);

                return ctx.GetDataTable(contextQuery);
            }
        }

    }
}
