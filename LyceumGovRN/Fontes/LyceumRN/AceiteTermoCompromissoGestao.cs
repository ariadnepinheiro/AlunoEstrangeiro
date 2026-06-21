using System;
using System.Collections.Generic;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN
{
    public class AceiteTermoCompromissoGestao : RNBase
    {
        public static TceTermoCompromissoGestao RetornaMenorTermoSemAceite(string matricula)
        {
            var termo = new TceTermoCompromissoGestao();

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT TOP 1
                                        T.ID_TERMO_GESTAO, T.ANO, T.PADRAO_ACESSO, DT_INICIO, T.DT_FIM,
                                        T.ARQUIVO
                                FROM    DBO.TCE_TERMO_COMPROMISSO_GESTAO T
                                WHERE   T.DT_FIM >= CONVERT(DATE, GETDATE())
                                        AND EXISTS ( SELECT 1
                                                     FROM   HADES.DBO.HD_USUARIO U
                                                            INNER JOIN HADES.DBO.HD_PADUSUARIO PU ON U.USUARIO = PU.USUARIO
                                                     WHERE  U.USUARIO = @MATRICULA
                                                            AND PU.PADACES = T.PADRAO_ACESSO )
                                        AND NOT EXISTS ( SELECT 1
                                                         FROM   DBO.TCE_ACEITE_TERMO_COMPROMISSO_GESTAO A
                                                                INNER JOIN TCE_TERMO_COMPROMISSO_GESTAO TG ON A.ID_TERMO_GESTAO = TG.ID_TERMO_GESTAO
                                                         WHERE  A.MATRICULA = @MATRICULA
                                                                AND T.ANO = TG.ANO
                                                                AND T.ARQUIVO = TG.ARQUIVO )
                                ORDER BY T.ANO  "
                };
                contextQuery.Parameters.Add("@MATRICULA", matricula);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    if (reader.Read())
                    {
                        termo.IdTermoGestao = Convert.ToInt32(reader["ID_TERMO_GESTAO"]);
                        termo.Ano = Convert.ToInt32(reader["ANO"]);
                        termo.PadraoAcesso = Convert.ToString(reader["PADRAO_ACESSO"]);
                        termo.DtInicio = Convert.ToDateTime(reader["DT_INICIO"]);
                        termo.DtFim = Convert.ToDateTime(reader["DT_FIM"]);
                        termo.Arquivo = Convert.ToString(reader["ARQUIVO"]);
                    }
                    else
                    {
                        return null;
                    }
                }

                return termo;
            }
        }

        public static void Inserir(TceAceiteTermoCompromissoGestao aceiteTermoCompromissoGestao)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" INSERT  INTO dbo.TCE_ACEITE_TERMO_COMPROMISSO_GESTAO ( MATRICULA, IP, ANO,ID_TERMO_GESTAO )
                                    VALUES  ( @MATRICULA, @IP, @ANO,@ID_TERMO_GESTAO ) "
                    };

                    contextQuery.Parameters.Add("@MATRICULA", aceiteTermoCompromissoGestao.Matricula);
                    contextQuery.Parameters.Add("@IP", aceiteTermoCompromissoGestao.Ip);
                    contextQuery.Parameters.Add("@ANO", aceiteTermoCompromissoGestao.Ano);
                    contextQuery.Parameters.Add("@ID_TERMO_GESTAO", aceiteTermoCompromissoGestao.IdTermoGestao);
 
                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static ValidacaoDados Validar(TceAceiteTermoCompromissoGestao aceiteTermoCompromissoGestao)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (aceiteTermoCompromissoGestao == null)
            {
                return validacaoDados;
            }

            if (aceiteTermoCompromissoGestao.Ano <= 0)
            {
                mensagens.Add("O campo ANO é obrigatório!");
            }

            if (string.IsNullOrEmpty(aceiteTermoCompromissoGestao.Matricula) || aceiteTermoCompromissoGestao.Matricula.Length > 8)
            {
                mensagens.Add("O USUARIO é obrigatório e seu login deve ter no máximo 8 caracteres!");
            }

            if (string.IsNullOrEmpty(aceiteTermoCompromissoGestao.Ip) || aceiteTermoCompromissoGestao.Ip.Length > 20)
            {
                mensagens.Add("O campo ARQUIVO é obrigatório e deve  ter no máximo 20 caracteres!");
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
    }
}