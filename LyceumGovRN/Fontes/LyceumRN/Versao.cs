namespace Techne.Lyceum.RN
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Seeduc.Infra.Data;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Lyceum.RN.Util;

    public class Versao : RNBase
    {
        public static void Atualizar(TceVersao versao)
        {
            using (var ctx = DataContextBuilder.FromHades.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                        {
                            Command = @"UPDATE  dbo.TCE_VERSAO
                                        SET     VERSAO = @versao,
                                                GESTAO_ONLINE = @GESTAO_ONLINE,
                                                DOCENTE_ONLINE = @DOCENTE_ONLINE,
                                                ALUNO_ONLINE = @ALUNO_ONLINE,
                                                MOTIVO = @motivo,
                                                DESCRICAO = @descricao,
                                                DATA_VERSAO = @data_versao,
                                                usuario = @usuario
                                        WHERE   ID_VERSAO = @id_versao"
                        };

                    contextQuery.Parameters.Add("@versao", versao.Versao);
                    contextQuery.Parameters.Add("@GESTAO_ONLINE", versao.GestaoOnline);
                    contextQuery.Parameters.Add("@DOCENTE_ONLINE", versao.DocenteOnline);
                    contextQuery.Parameters.Add("@ALUNO_ONLINE", versao.AlunoOnline);
                    contextQuery.Parameters.Add("@motivo", versao.Motivo);
                    contextQuery.Parameters.Add("@descricao", versao.Descricao);
                    contextQuery.Parameters.Add("@data_versao", versao.DataVersao);
                    contextQuery.Parameters.Add("@usuario", versao.Usuario);
                    contextQuery.Parameters.Add("@id_versao", versao.IdVersao);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static TceVersao Bind(IDictionary chaves, IDictionary valores)
        {
            return new TceVersao
                {
                    IdVersao = chaves == null ? 0 : Convert.ToInt32(chaves["ID_VERSAO"]),
                    Versao = Convert.ToString(valores["VERSAO"]),
                    GestaoOnline = Convert.ToBoolean(valores["GESTAO_ONLINE"]),
                    DocenteOnline = Convert.ToBoolean(valores["DOCENTE_ONLINE"]),
                    AlunoOnline = Convert.ToBoolean(valores["ALUNO_ONLINE"]),
                    Motivo = Convert.ToString(valores["MOTIVO"]),
                    Descricao = Convert.ToString(valores["DESCRICAO"]),
                    DataVersao = Convert.ToDateTime(valores["DATA_VERSAO"] ?? DateTime.MinValue)
                };
        }

        public static void Inserir(TceVersao versao)
        {
            using (var ctx = DataContextBuilder.FromHades.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                        {
                            Command = @"INSERT INTO dbo.TCE_VERSAO
                                                (
                                                 VERSAO,
                                                 GESTAO_ONLINE,
                                                 DOCENTE_ONLINE,
                                                 ALUNO_ONLINE,
                                                 MOTIVO,
                                                 DESCRICAO,
                                                 DATA_VERSAO,
                                                 USUARIO
                                                )
                                        VALUES  (
                                                 @versao,
                                                 @GESTAO_ONLINE,
                                                 @DOCENTE_ONLINE,
                                                 @ALUNO_ONLINE,
                                                 @motivo,
                                                 @descricao,
                                                 @data_versao,
                                                 @usuario
                                                )"
                        };

                    contextQuery.Parameters.Add("@versao", versao.Versao);
                    contextQuery.Parameters.Add("@GESTAO_ONLINE", versao.GestaoOnline);
                    contextQuery.Parameters.Add("@DOCENTE_ONLINE", versao.DocenteOnline);
                    contextQuery.Parameters.Add("@ALUNO_ONLINE", versao.AlunoOnline);
                    contextQuery.Parameters.Add("@motivo", versao.Motivo);
                    contextQuery.Parameters.Add("@descricao", versao.Descricao);
                    contextQuery.Parameters.Add("@data_versao", versao.DataVersao);
                    contextQuery.Parameters.Add("@usuario", versao.Usuario);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static DataTable Listar()
        {
            using (var ctx = DataContextBuilder.FromHades.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                    {
                        Command = @"SELECT  *
                                    FROM    TCE_VERSAO
                                    ORDER BY DATA_VERSAO DESC, VERSAO DESC"
                    };

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static void Remover(int idVersao)
        {
            if (idVersao < 1)
            {
                return;
            }

            using (var ctx = DataContextBuilder.FromHades.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                        {
                            Command = "DELETE FROM TCE_VERSAO WHERE ID_VERSAO = @id_versao"
                        };

                    contextQuery.Parameters.Add("@id_versao", idVersao);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static ValidacaoDados Validar(TceVersao versao)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
                {
                    Valido = false
                };

            if (versao == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(versao.Versao)
                || (!string.IsNullOrEmpty(versao.Versao)
                    && versao.Versao.Length > 20))
            {
                mensagens.Add("O campo Versão é obrigatório com o máximo de 20 caracteres!");
            }

            if (!(versao.GestaoOnline
                  || versao.DocenteOnline
                  || versao.AlunoOnline))
            {
                mensagens.Add("É obrigatório selecionar ao menos um projeto alterado.");
            }

            if (string.IsNullOrEmpty(versao.Motivo)
                || (!string.IsNullOrEmpty(versao.Motivo)
                    && versao.Motivo.Length > 20))
            {
                mensagens.Add("O campo Motivo é obrigatório com o máximo de 20 caracteres!");
            }

            if (string.IsNullOrEmpty(versao.Descricao)
                || (!string.IsNullOrEmpty(versao.Descricao)
                    && versao.Descricao.Length > 500))
            {
                mensagens.Add("O campo Descrição é obrigatório com o máximo de 500 caracteres!");
            }

            if (versao.DataVersao.Equals(DateTime.MinValue))
            {
                mensagens.Add("O campo Data é obrigatório!");
            }

            if (versao.DataVersao > DateTime.Today)
            {
                mensagens.Add("O campo Data não pode ser após o dia atual!");
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