using System;
using System.Collections.Generic;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN
{
    public class ValidacaoTipoDependencia: RNBase
    {
        public const string Validado = "Validado";

        public const string NaoConfirmado = "Não validado";

        public const string Reaberto = "Reaberto";

        public static TceValidacaoTipoDependencia RetornaUltimaValidacao(string unidadeFisica)
        {
            var validacaoTipoDependencia = new TceValidacaoTipoDependencia();

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT TOP 1
                                            ID_VALIDACAO, UNIDADE_FISICA, MATRICULA, STATUS, DT_CADASTRO
                                    FROM    TCE_VALIDACAO_TIPO_DEPENDENCIA
                                    WHERE   UNIDADE_FISICA = @UNIDADE_FISICA
                                    ORDER BY DT_CADASTRO DESC "
                };
                contextQuery.Parameters.Add("@UNIDADE_FISICA", unidadeFisica);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    if (reader.Read())
                    {
                        validacaoTipoDependencia.IdValidacao = Convert.ToInt32(reader["ID_VALIDACAO"]);
                        validacaoTipoDependencia.UnidadeFisica = unidadeFisica;
                        validacaoTipoDependencia.Matricula = Convert.ToString(reader["MATRICULA"]);
                        validacaoTipoDependencia.Status = Convert.ToString(reader["STATUS"]);
                        validacaoTipoDependencia.DtCadastro = Convert.ToDateTime(reader["DT_CADASTRO"]);
                    }
                    else
                    {
                        return null;
                    }
                }

                return validacaoTipoDependencia;
            }
        }

        public static void Inserir(TceValidacaoTipoDependencia validacaoTipoDependencia)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" INSERT  INTO TCE_VALIDACAO_TIPO_DEPENDENCIA ( UNIDADE_FISICA, MATRICULA,
                                              STATUS )
                                     VALUES  ( @UNIDADE_FISICA, @MATRICULA, @STATUS ) "
                    };
                    contextQuery.Parameters.Add("@UNIDADE_FISICA", validacaoTipoDependencia.UnidadeFisica);
                    contextQuery.Parameters.Add("@MATRICULA", validacaoTipoDependencia.Matricula);
                    contextQuery.Parameters.Add("@STATUS", validacaoTipoDependencia.Status);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static ValidacaoDados Validar(TceValidacaoTipoDependencia validacaoTipoDependencia)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (validacaoTipoDependencia == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(validacaoTipoDependencia.UnidadeFisica))
            {
                mensagens.Add("O campo UNIDADE FÍSICA é obrigatório!");
            }

            if (string.IsNullOrEmpty(validacaoTipoDependencia.Matricula) || validacaoTipoDependencia.Matricula.Length > 20)
            {
                mensagens.Add("A MATRÍCULA DO RESPONSÁVEL é obrigatória e deve ter 20 digitos!");
            }

            if (string.IsNullOrEmpty(validacaoTipoDependencia.Status))
            {
                mensagens.Add("O campo STATUS é obrigatório!");
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
