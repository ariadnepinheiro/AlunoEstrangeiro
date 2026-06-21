
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
    public class FeriadoUnidadeEnsino
    {
        public static DataTable Listar()
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT ID_FERIADO_UE, F.UNIDADE_ENS, E.NOME_COMP, DATA AS DATA_UE, DESCRICAO AS DESCRICAO_UE, 
                        TIPO_EVENTO AS TIPO_EVENTO_UE, MATRICULA, DT_CADASTRO, DT_ALTERACAO
                        FROM TCE_FERIADO_UNIDADE_ENSINO F
                        INNER JOIN LY_UNIDADE_ENSINO E
                        ON F.UNIDADE_ENS = E.UNIDADE_ENS
                        WHERE YEAR(DATA) = YEAR(GETDATE())                        
                        order by DATA "
                };

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable Listar(string censo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT ID_FERIADO_UE, F.UNIDADE_ENS, E.NOME_COMP, DATA AS DATA_UE, DESCRICAO AS DESCRICAO_UE, 
                        TIPO_EVENTO AS TIPO_EVENTO_UE, MATRICULA, DT_CADASTRO, DT_ALTERACAO
                        FROM TCE_FERIADO_UNIDADE_ENSINO F
                        INNER JOIN LY_UNIDADE_ENSINO E
                        ON F.UNIDADE_ENS = E.UNIDADE_ENS
                        WHERE YEAR(DATA) = YEAR(GETDATE())
                        AND F.UNIDADE_ENS = @CENSO
                        order by DATA "
                };
                contextQuery.Parameters.Add("@CENSO", censo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static void Inserir(TceFeriadoUnidadeEnsino feriadoUnidadeEnsino)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" INSERT INTO TCE_FERIADO_UNIDADE_ENSINO
                          (UNIDADE_ENS, DATA, DESCRICAO, TIPO_EVENTO, MATRICULA)
                          VALUES
                          (@UNIDADE_ENS, @DATA, @DESCRICAO, @TIPO_EVENTO, @MATRICULA)"
                    };

                    contextQuery.Parameters.Add("@UNIDADE_ENS", feriadoUnidadeEnsino.UnidadeEnsino);
                    contextQuery.Parameters.Add("@DATA", SqlDbType.Date, feriadoUnidadeEnsino.Data);
                    contextQuery.Parameters.Add("@DESCRICAO", feriadoUnidadeEnsino.Descricao);
                    contextQuery.Parameters.Add("@TIPO_EVENTO", feriadoUnidadeEnsino.TipoEvento);
                    contextQuery.Parameters.Add("@MATRICULA", feriadoUnidadeEnsino.Matricula);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static void Alterar(TceFeriadoUnidadeEnsino feriadoUnidadeEnsino)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"  UPDATE TCE_FERIADO_UNIDADE_ENSINO 
                          SET DATA = @DATA,
                          UNIDADE_ENS = @UNIDADE_ENS,
                          DESCRICAO = @DESCRICAO,
                          TIPO_EVENTO = @TIPO_EVENTO,
                          MATRICULA = @MATRICULA,
                          DT_ALTERACAO = GETDATE()
                          WHERE ID_FERIADO_UE = @ID "
                    };
                    contextQuery.Parameters.Add("@DATA", SqlDbType.Date, feriadoUnidadeEnsino.Data);
                    contextQuery.Parameters.Add("@UNIDADE_ENS", feriadoUnidadeEnsino.UnidadeEnsino);
                    contextQuery.Parameters.Add("@DESCRICAO", feriadoUnidadeEnsino.Descricao);
                    contextQuery.Parameters.Add("@TIPO_EVENTO", feriadoUnidadeEnsino.TipoEvento);
                    contextQuery.Parameters.Add("@MATRICULA", feriadoUnidadeEnsino.Matricula);
                    contextQuery.Parameters.Add("@ID", feriadoUnidadeEnsino.IdFeriadoUnidadeEnsino);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception e)
                {
                    ctx.Abandon();
                    throw e;
                }
            }
        }

        public static void Remover(int idFeriadoUnidadeEnsino)
        {
            if (idFeriadoUnidadeEnsino < 1)
            {
                return;
            }

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" DELETE TCE_FERIADO_UNIDADE_ENSINO
                            WHERE ID_FERIADO_UE = @ID_FERIADO_UE "
                    };
                    contextQuery.Parameters.Add("@ID_FERIADO_UE", idFeriadoUnidadeEnsino);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception e)
                {
                    ctx.Abandon();
                    throw e;
                }
            }
        }

        private static bool VerificarDuplicidade(TceFeriadoUnidadeEnsino feriadoUnidadeEnsino)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                try
                {

                    var contextQuery = new ContextQuery();

                    contextQuery.Command = @" SELECT 1 
                        FROM TCE_FERIADO_UNIDADE_ENSINO
                        WHERE DATA = @DATA
                        AND DESCRICAO = @DESCRICAO
                        AND TIPO_EVENTO = @TIPO_EVENTO
                        AND UNIDADE_ENS = @UNIDADE_ENS 
                        AND ID_FERIADO_UE <> @ID_FERIADO_UE";

                    contextQuery.Parameters.Add("@DATA", SqlDbType.Date, feriadoUnidadeEnsino.Data);
                    contextQuery.Parameters.Add("@DESCRICAO", feriadoUnidadeEnsino.Descricao);
                    contextQuery.Parameters.Add("@TIPO_EVENTO", feriadoUnidadeEnsino.TipoEvento);
                    contextQuery.Parameters.Add("@UNIDADE_ENS", feriadoUnidadeEnsino.UnidadeEnsino);
                    contextQuery.Parameters.Add("@ID_FERIADO_UE", feriadoUnidadeEnsino.IdFeriadoUnidadeEnsino);

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

        public static ValidacaoDados Validar(TceFeriadoUnidadeEnsino feriadoUnidadeEnsino)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (feriadoUnidadeEnsino == null)
            {
                return validacaoDados;
            }

            if (feriadoUnidadeEnsino.Data.Equals(DateTime.MinValue))
            {
                mensagens.Add("O campo Data é obrigatório!");
            }

            if (feriadoUnidadeEnsino.Data < DateTime.Today)
            {
                mensagens.Add("O campo Data não pode ser anterior ao dia atual!");
            }

            if (string.IsNullOrEmpty(feriadoUnidadeEnsino.UnidadeEnsino))
            {
                mensagens.Add("O campo Unidade de Ensino é obrigatório!");
            }

            if (feriadoUnidadeEnsino.UnidadeEnsino.Length != 8)
            {
                mensagens.Add("Unidade de Ensino inválida!");
            }
            if (string.IsNullOrEmpty(feriadoUnidadeEnsino.Descricao)
                || (!string.IsNullOrEmpty(feriadoUnidadeEnsino.Descricao)
                    && feriadoUnidadeEnsino.Descricao.Length > 500))
            {
                mensagens.Add("O campo Descrição é obrigatório com o máximo de 500 caracteres!");
            }

            if (string.IsNullOrEmpty(feriadoUnidadeEnsino.TipoEvento)
                || (!string.IsNullOrEmpty(feriadoUnidadeEnsino.TipoEvento)
                    && feriadoUnidadeEnsino.TipoEvento.Length > 100))
            {
                mensagens.Add("O campo Tipo é obrigatório com o máximo de 100 caracteres!");
            }

            if (VerificarDuplicidade(feriadoUnidadeEnsino))
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

        public static ValidacaoDados ValidarExclusao(TceFeriadoUnidadeEnsino feriadoUnidadeEnsino)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (feriadoUnidadeEnsino == null)
            {
                return validacaoDados;
            }

           
            if (feriadoUnidadeEnsino.Data < DateTime.Today)
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

        public static TceFeriadoUnidadeEnsino Bind(IDictionary chaves, IDictionary valores)
        {
            return new TceFeriadoUnidadeEnsino
            {
                IdFeriadoUnidadeEnsino = chaves == null ? 0 : Convert.ToInt32(chaves["ID_FERIADO_UE"]),
                TipoEvento = Convert.ToString(valores["TIPO_EVENTO_UE"]),
                Descricao = Convert.ToString(valores["DESCRICAO_UE"]),
                Data = Convert.ToDateTime(valores["DATA_UE"] ?? DateTime.MinValue),
                UnidadeEnsino = Convert.ToString(valores["UNIDADE_ENS"])
            };
        }
    }
}
