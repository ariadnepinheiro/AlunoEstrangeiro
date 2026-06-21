using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN
{
    public class PadacesPerfil : RNBase
    {
        public static DataTable Listar(string padraoAcesso)
        {
            using (var ctx = DataContextBuilder.FromHades.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT  PP.* ,
                            P.DESCRICAO ,
                            PA.NOME
                    FROM    TCE_PADACES_PERFIL PP
                            INNER JOIN DBO.TCE_PERFIL P ON P.ID_PERFIL = PP.ID_PERFIL
                            INNER JOIN DBO.HD_PADACES PA ON PA.PADACES = PP.PADACES
                    WHERE   pp.PADACES = @PADACES "
                };
                contextQuery.Parameters.Add("@PADACES", padraoAcesso);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarPorPadraoAcesso(string padraoAcesso)
        {
            using (var ctx = DataContextBuilder.FromHades.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT  PP.* ,
                            P.DESCRICAO ,
                            PA.NOME
                    FROM    TCE_PADACES_PERFIL PP
                            INNER JOIN DBO.TCE_PERFIL P ON P.ID_PERFIL = PP.ID_PERFIL
                            INNER JOIN DBO.HD_PADACES PA ON PA.PADACES = PP.PADACES
                    WHERE   pp.PADACES = @PADACES 
                    ORDER BY PA.NOME,  P.DESCRICAO  "
                };
                contextQuery.Parameters.Add("@PADACES", padraoAcesso);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarPorPerfil(int idPerfil)
        {
            using (var ctx = DataContextBuilder.FromHades.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT  PP.* ,
                                    P.DESCRICAO ,
                                    PA.NOME
                            FROM    TCE_PADACES_PERFIL PP
                                    INNER JOIN DBO.TCE_PERFIL P ON P.ID_PERFIL = PP.ID_PERFIL
                                    INNER JOIN DBO.HD_PADACES PA ON PA.PADACES = PP.PADACES
                            WHERE   pp.ID_PERFIL = @ID_PERFIL
                            ORDER BY PA.NOME,  P.DESCRICAO "
                };
                contextQuery.Parameters.Add("@ID_PERFIL", idPerfil);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static ValidacaoDados Validar(TcePadacesPerfil padacesPerfil)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (padacesPerfil == null)
            {
                return validacaoDados;
            }

            if (padacesPerfil.IdPerfil <= 0)
            {
                mensagens.Add("O campo PERFIL é obrigatório!");
            }

            if (string.IsNullOrEmpty(padacesPerfil.Padaces)
                || (!string.IsNullOrEmpty(padacesPerfil.Padaces)
                    && padacesPerfil.Padaces.Length > 14))
            {
                mensagens.Add("O campo PADRÃO DE ACESSO é obrigatório com o máximo de 14 caracteres!");
            }

            if (string.IsNullOrEmpty(padacesPerfil.Matricula)
                || (!string.IsNullOrEmpty(padacesPerfil.Matricula)
                    && padacesPerfil.Matricula.Length > 20))
            {
                mensagens.Add("O campo MATRICULA DO RESPONSÁVEL é obrigatório com o máximo de 20 caracteres!");
            }

            if (mensagens.Count == 0)
            {
                using (var ctx = DataContextBuilder.FromHades.ToFastReadingOnly())
                {
                    //Verifica se aquele perfil já existe com aquele padrao de acesso
                    var contextQuery = new ContextQuery(
                        @" SELECT  1
                            FROM    dbo.TCE_PADACES_PERFIL
                            WHERE   ID_PERFIL = @ID_PERFIL
                                    AND PADACES = @PADACES ");

                    contextQuery.Parameters.Add("@ID_PERFIL", padacesPerfil.IdPerfil);
                    contextQuery.Parameters.Add("@PADACES", padacesPerfil.Padaces);

                    var obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        mensagens.Add("Já existe esta combinação de Perfil / Padrao de Acesso cadastrada.");
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

        public static void Inserir(TcePadacesPerfil padacesPerfil)
        {
            using (var ctx = DataContextBuilder.FromHades.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"INSERT INTO dbo.TCE_PADACES_PERFIL
                                        ( ID_PERFIL ,
                                          PADACES ,
                                          MATRICULA 
                                        )
                                VALUES  ( @ID_PERFIL ,
                                          @PADACES ,
                                          @MATRICULA        
                                        ) "
                    };

                    contextQuery.Parameters.Add("@ID_PERFIL", padacesPerfil.IdPerfil);
                    contextQuery.Parameters.Add("@PADACES", padacesPerfil.Padaces);
                    contextQuery.Parameters.Add("@MATRICULA", padacesPerfil.Matricula);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static ValidacaoDados ValidarRemover(int id)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (id <= 0)
            {
                mensagens.Add("O campo ID é obrigatório!");
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

        public static void Remover(int id)
        {
            if (id < 1)
            {
                return;
            }

            using (var ctx = DataContextBuilder.FromHades.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" DELETE  TCE_PADACES_PERFIL
                                    WHERE   ID_PADACES_PERFIL = @ID "
                    };

                    contextQuery.Parameters.Add("@ID", id);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }
    }
}
