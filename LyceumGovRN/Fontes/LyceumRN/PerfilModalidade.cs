using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN
{
    public class PerfilModalidade : RNBase
    {
        public static DataTable Listar(int idPerfil)
        {
            try
            {
               var contextQuery = new ContextQuery(
                               @" SELECT  M.MODALIDADE ,
                                        M.DESCRICAO ,
                                        M.MODALIDADE + ' - ' + M.DESCRICAO AS MODALIDADE_DESCRICAO ,
                                        P.ID_PERFIL ,
                                        P.DESCRICAO AS DESCRICAO_PERFIL,
                                        pm.PERFILMODALIDADEID
                                FROM    DBO.PERFILMODALIDADE PM
                                        INNER JOIN DBO.LY_MODALIDADE_CURSO M ON PM.MODALIDADEID = M.MODALIDADE
                                        INNER JOIN HADES.DBO.TCE_PERFIL P ON PM.PERFILID = P.ID_PERFIL
                                WHERE   PM.PERFILID = @ID_PERFIL  ");

                contextQuery.Parameters.Add("@ID_PERFIL", idPerfil);

                return Consultar(contextQuery);
            }
            catch (Exception ex)
            {
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                    Environment.NewLine,
                    Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
        }

        public static ValidacaoDados Validar(Entidades.PerfilModalidade perfilModalidade)
        {
            try
            {
                var mensagens = new List<string>();
                var validacaoDados = new ValidacaoDados
                {
                    Valido = false
                };

                if (perfilModalidade == null)
                {
                    return validacaoDados;
                }

                if (perfilModalidade.PerfilId <= 0)
                {
                    mensagens.Add("Perfil de Acesso não encontrado.");
                }

                if (string.IsNullOrEmpty(perfilModalidade.ModalidadeId))
                {
                    mensagens.Add("Selecione a Modalidade.");
                }

                if (string.IsNullOrEmpty(perfilModalidade.Matricula)
                        || (!string.IsNullOrEmpty(perfilModalidade.Matricula)
                            && perfilModalidade.Matricula.Length > 12))
                {
                    mensagens.Add("O campo MATRICULA é obrigatório com o máximo de 12 caracteres!");
                }

                if (mensagens.Count == 0)
                {
                    using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                    {
                        //Verifica já existe outra inserção com o mesmo perfil / modalidade
                        var contextQuery = new ContextQuery
                        {
                            Command =
                                @" SELECT TOP 1
                                    1
                            FROM    DBO.PERFILMODALIDADE
                            WHERE   MODALIDADEID = @MODALIDADEID
                                    AND PERFILMODALIDADEID <> @PERFILMODALIDADEID "
                        };

                        contextQuery.Parameters.Add("@MODALIDADEID", perfilModalidade.ModalidadeId);
                        contextQuery.Parameters.Add("@PERFILMODALIDADEID", perfilModalidade.PerfilModalidadeId);

                        var obj = ctx.GetReturnValue(contextQuery);

                        if (obj != null)
                        {
                            mensagens.Add("Esta Modalidade já foi cadastrado anteriormente.");
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
            catch (Exception ex)
            {
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                    Environment.NewLine,
                    Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
        }

        public static void Inserir(Entidades.PerfilModalidade perfilModalidade)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" INSERT  INTO DBO.PERFILMODALIDADE
                                            ( MODALIDADEID ,
                                              PERFILID ,
                                              MATRICULA 
                                            )
                                    VALUES  ( @MODALIDADEID ,
                                              @PERFILID ,
                                              @MATRICULA                 
                                            ) "
                    };

                    contextQuery.Parameters.Add("@MODALIDADEID", perfilModalidade.ModalidadeId);
                    contextQuery.Parameters.Add("@PERFILID", perfilModalidade.PerfilId);
                    contextQuery.Parameters.Add("@MATRICULA", perfilModalidade.Matricula);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception ex)
                {
                    var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                    throw new Exception(mensagem);
                }
            }
        }

        public static void Remover(int id)
        {
            if (id < 1)
            {
                return;
            }

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" DELETE  DBO.PERFILMODALIDADE
                                    WHERE   PERFILMODALIDADEID = @PERFILMODALIDADEID "
                    };

                    contextQuery.Parameters.Add("@PERFILMODALIDADEID", id);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception ex)
                {
                    var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                    throw new Exception(mensagem);
                }
            }
        }

        public static DataTable ListarModalidadesSemPerfil()
        {
            try
            {
                var contextQuery = new ContextQuery(
                   @" SELECT  M.MODALIDADE ,
                                M.DESCRICAO ,
                                M.MODALIDADE + ' - ' + M.DESCRICAO AS MODALIDADE_DESCRICAO
                        FROM    DBO.LY_MODALIDADE_CURSO M
                        WHERE   NOT EXISTS ( SELECT 1
                                             FROM   DBO.PERFILMODALIDADE PM
                                             WHERE  PM.MODALIDADEID = M.MODALIDADE ) ");

                return Consultar(contextQuery);
            }
            catch (Exception ex)
            {
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                    Environment.NewLine,
                    Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
        }

        public static string RetornaPerfilResponsavel(string modalidade)
        {
            var perfil = string.Empty;

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @" SELECT DISTINCT
                                    PE.DESCRICAO AS PERFIL_RESPONSAVEL
                            FROM    DBO.PERFILMODALIDADE PM
                                    INNER JOIN HADES.DBO.TCE_PERFIL PE ON PM.PERFILID = PE.ID_PERFIL
                            WHERE   PM.MODALIDADEID = @MODALIDADEID "
                };

                contextQuery.Parameters.Add("@MODALIDADEID", modalidade);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    while (reader.Read())
                    {
                        perfil = Convert.ToString(reader["PERFIL_RESPONSAVEL"]);
                    }
                }
            }
            return perfil;
        }
    }
}