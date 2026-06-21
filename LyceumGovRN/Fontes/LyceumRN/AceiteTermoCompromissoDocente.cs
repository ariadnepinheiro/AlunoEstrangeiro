using System;
using System.Collections.Generic;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN
{
    public class AceiteTermoCompromissoDocente : RNBase
    {
        public static TceTermoCompromissoDocente RetornaMenorTermoSemAceite(string matricula)
        {
            var termo = new TceTermoCompromissoDocente();

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT TOP 1
                                        T.ID_TERMO_DOCENTE, T.ANO, DT_INICIO, T.DT_FIM, T.ARQUIVO
                                FROM    DBO.TCE_TERMO_COMPROMISSO_DOCENTE T
                                WHERE   T.DT_FIM >= CONVERT(DATE, GETDATE())
                                        AND NOT EXISTS ( SELECT 1
                                                         FROM   DBO.TCE_ACEITE_TERMO_COMPROMISSO_DOCENTE A
                                                         WHERE  A.ID_TERMO_DOCENTE = T.ID_TERMO_DOCENTE
                                                                AND A.MATRICULA = @MATRICULA )
                                ORDER BY T.ANO " 
                };
                contextQuery.Parameters.Add("@MATRICULA", matricula);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    if (reader.Read())
                    {
                        termo.IdTermoDocente = Convert.ToInt32(reader["ID_TERMO_DOCENTE"]);
                        termo.Ano = Convert.ToInt32(reader["ANO"]);
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

        public static void Inserir(TceAceiteTermoCompromissoDocente aceiteTermoCompromissoDocente)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" INSERT  INTO dbo.TCE_ACEITE_TERMO_COMPROMISSO_DOCENTE ( MATRICULA, IP, ANO,
                                                        ID_TERMO_DOCENTE )
                                    VALUES  ( @MATRICULA, @IP, @ANO, @ID_TERMO_DOCENTE ) "
                    };

                    contextQuery.Parameters.Add("@MATRICULA", aceiteTermoCompromissoDocente.Matricula);
                    contextQuery.Parameters.Add("@IP", aceiteTermoCompromissoDocente.Ip);
                    contextQuery.Parameters.Add("@ANO", aceiteTermoCompromissoDocente.Ano);
                    contextQuery.Parameters.Add("@ID_TERMO_DOCENTE", aceiteTermoCompromissoDocente.IdTermoDocente);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static ValidacaoDados Validar(TceAceiteTermoCompromissoDocente aceiteTermoCompromissoDocente)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (aceiteTermoCompromissoDocente == null)
            {
                return validacaoDados;
            }

            if (aceiteTermoCompromissoDocente.Ano <= 0)
            {
                mensagens.Add("O campo ANO é obrigatório!");
            }

            if (string.IsNullOrEmpty(aceiteTermoCompromissoDocente.Matricula) || aceiteTermoCompromissoDocente.Matricula.Length > 8)
            {
                mensagens.Add("O USUARIO é obrigatório e seu login deve ter no máximo 8 caracteres!");
            }

            if (string.IsNullOrEmpty(aceiteTermoCompromissoDocente.Ip) || aceiteTermoCompromissoDocente.Ip.Length > 20)
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
