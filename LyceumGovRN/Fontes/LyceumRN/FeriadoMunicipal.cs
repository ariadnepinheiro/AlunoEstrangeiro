using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using System.Collections;

namespace Techne.Lyceum.RN
{
    public class FeriadoMunicipal
    {
        public static DataTable Listar()
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT ID_FERIADO_MUNIC, F.MUNICIPIO, M.NOME, DATA AS DATA_MUNIC, DESCRICAO AS DESCRICAO_MUNIC, 
                        TIPO_EVENTO AS TIPO_EVENTO_MUNIC, MATRICULA, DT_CADASTRO, DT_ALTERACAO
                        FROM TCE_FERIADO_MUNICIPAL F
                        INNER JOIN MUNICIPIO M
                        ON F.MUNICIPIO = M.CODIGO
                        WHERE YEAR(DATA) = YEAR(GETDATE())						
                        ORDER BY DATA  "
                };                

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable Listar(string codMunicipio)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT ID_FERIADO_MUNIC, F.MUNICIPIO, M.NOME, DATA AS DATA_MUNIC, DESCRICAO AS DESCRICAO_MUNIC, 
                        TIPO_EVENTO AS TIPO_EVENTO_MUNIC, MATRICULA, DT_CADASTRO, DT_ALTERACAO
                        FROM TCE_FERIADO_MUNICIPAL F
                        INNER JOIN MUNICIPIO M
                        ON F.MUNICIPIO = M.CODIGO
                        WHERE YEAR(DATA) = YEAR(GETDATE())
						AND F.MUNICIPIO = @COD_MUNICIPIO
                        ORDER BY DATA  "
                };
                contextQuery.Parameters.Add("@COD_MUNICIPIO", codMunicipio);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static void Inserir(TceFeriadoMunicipal feriadoMunicipal)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" INSERT INTO TCE_FERIADO_MUNICIPAL
                          (MUNICIPIO, DATA, DESCRICAO, TIPO_EVENTO, MATRICULA)
                          VALUES
                          (@MUNICIPIO, @DATA, @DESCRICAO, @TIPO_EVENTO, @MATRICULA)"
                    };

                    contextQuery.Parameters.Add("@MUNICIPIO", feriadoMunicipal.CodMunicipio);
                    contextQuery.Parameters.Add("@DATA", SqlDbType.Date, feriadoMunicipal.Data);
                    contextQuery.Parameters.Add("@DESCRICAO", feriadoMunicipal.Descricao);
                    contextQuery.Parameters.Add("@TIPO_EVENTO", feriadoMunicipal.TipoEvento);
                    contextQuery.Parameters.Add("@MATRICULA", feriadoMunicipal.Matricula);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static void Alterar(TceFeriadoMunicipal feriadoMunicipal)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"  UPDATE TCE_FERIADO_MUNICIPAL 
                          SET DATA = @DATA,
                          MUNICIPIO = @MUNICIPIO,
                          DESCRICAO = @DESCRICAO,
                          TIPO_EVENTO = @TIPO_EVENTO,
                          MATRICULA = @MATRICULA,
                          DT_ALTERACAO = GETDATE()
                          WHERE ID_FERIADO_MUNIC = @ID "
                    };
                    contextQuery.Parameters.Add("@DATA", SqlDbType.Date, feriadoMunicipal.Data);
                    contextQuery.Parameters.Add("@MUNICIPIO", feriadoMunicipal.CodMunicipio);
                    contextQuery.Parameters.Add("@DESCRICAO", feriadoMunicipal.Descricao);
                    contextQuery.Parameters.Add("@TIPO_EVENTO", feriadoMunicipal.TipoEvento);
                    contextQuery.Parameters.Add("@MATRICULA", feriadoMunicipal.Matricula);
                    contextQuery.Parameters.Add("@ID", feriadoMunicipal.IdFeriadoMunicipal);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception e)
                {
                    ctx.Abandon();
                    throw e;
                }
            }
        }

        public static void Remover(int idFeriadoMunicipal)
        {
            if (idFeriadoMunicipal < 1)
            {
                return;
            }            

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" DELETE TCE_FERIADO_MUNICIPAL
                            WHERE ID_FERIADO_MUNIC = @ID_FERIADO_MUNIC "
                    };
                    contextQuery.Parameters.Add("@ID_FERIADO_MUNIC", idFeriadoMunicipal);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception e)
                {
                    ctx.Abandon();
                    throw e;
                }
            }
        }

        private static bool VerificarDuplicidade(TceFeriadoMunicipal feriadoMunicipal)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                try
                {

                    var contextQuery = new ContextQuery();

                    contextQuery.Command = @" SELECT 1 
                        FROM TCE_FERIADO_MUNICIPAL
                        WHERE DATA = @DATA
                        AND DESCRICAO = @DESCRICAO
                        AND TIPO_EVENTO = @TIPO_EVENTO
                        AND MUNICIPIO = @MUNICIPIO 
                        AND ID_FERIADO_MUNIC <> @ID_FERIADO_MUNIC";

                    contextQuery.Parameters.Add("@DATA", SqlDbType.Date, feriadoMunicipal.Data);
                    contextQuery.Parameters.Add("@DESCRICAO", feriadoMunicipal.Descricao);
                    contextQuery.Parameters.Add("@TIPO_EVENTO", feriadoMunicipal.TipoEvento);
                    contextQuery.Parameters.Add("@MUNICIPIO", feriadoMunicipal.CodMunicipio);
                    contextQuery.Parameters.Add("@ID_FERIADO_MUNIC", feriadoMunicipal.IdFeriadoMunicipal);

                    object obj = ctx.GetReturnValue(contextQuery);

                    if (obj == null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    ctx.Abandon();
                    throw e;
                }
            }
        }

        public static ValidacaoDados Validar(TceFeriadoMunicipal feriadoMunicipal)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (feriadoMunicipal == null)
            {
                return validacaoDados;
            }

            if (feriadoMunicipal.Data.Equals(DateTime.MinValue))
            {
                mensagens.Add("O campo Data é obrigatório!");
            }

            if (feriadoMunicipal.Data < DateTime.Today)
            {
                mensagens.Add("O campo Data não pode ser anterior ao dia atual!");
            }

            if (string.IsNullOrEmpty(feriadoMunicipal.CodMunicipio))
            {
                mensagens.Add("O campo Município é obrigatório!");
            }

            if (feriadoMunicipal.CodMunicipio.Length != 8)
            {
                mensagens.Add("Município inválido!");
            }

            if (string.IsNullOrEmpty(feriadoMunicipal.Descricao)
                || (!string.IsNullOrEmpty(feriadoMunicipal.Descricao)
                    && feriadoMunicipal.Descricao.Length > 500))
            {
                mensagens.Add("O campo Descrição é obrigatório com o máximo de 500 caracteres!");
            }

            if (string.IsNullOrEmpty(feriadoMunicipal.TipoEvento)
                || (!string.IsNullOrEmpty(feriadoMunicipal.TipoEvento)
                    && feriadoMunicipal.TipoEvento.Length > 100))
            {
                mensagens.Add("O campo Tipo é obrigatório com o máximo de 100 caracteres!");
            }

            if (VerificarDuplicidade(feriadoMunicipal))
            {
                mensagens.Add("Já existe um feriado cadastrado com estes dados!");
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

        public static ValidacaoDados ValidarExclusao(TceFeriadoMunicipal feriadoMunicipal)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (feriadoMunicipal == null)
            {
                return validacaoDados;
            }


            if (feriadoMunicipal.Data < DateTime.Today)
            {
                mensagens.Add("O campo Data não pode ser anterior ao dia atual!");
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

        public static TceFeriadoMunicipal Bind(IDictionary chaves, IDictionary valores)
        {
            return new TceFeriadoMunicipal
            {
                IdFeriadoMunicipal = chaves == null ? 0 : Convert.ToInt32(chaves["ID_FERIADO_MUNIC"]),
                TipoEvento = Convert.ToString(valores["TIPO_EVENTO_MUNIC"]),
                Descricao = Convert.ToString(valores["DESCRICAO_MUNIC"]),
                Data = Convert.ToDateTime(valores["DATA_MUNIC"] ?? DateTime.MinValue),
                CodMunicipio = Convert.ToString(valores["MUNICIPIO"])
            };
        }
    }
}
