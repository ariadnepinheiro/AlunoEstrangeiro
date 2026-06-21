using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using System;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN
{
    public class AreaConhecimento
    {
        public static DataTable Listar()
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT * FROM AREACONHECIMENTO "
                };

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarAreaConhecimentoOferecidoSEEDUC(int idTipoCursoCapacitacao)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                string strSQL = @" SELECT * FROM AREACONHECIMENTO WHERE AREACONHECIMENTOID IN 
                                        (SELECT CC.AREACONHECIMENTOID FROM CURSOCAPACITACAO CC INNER JOIN CURSOCAPACITACAO_TIPOCURSOCAPACITACAO TCC
                                            ON CC.CURSOCAPACITACAOID = TCC.CURSOCAPACITACAOID WHERE TCC.TIPOCURSOCAPACITACAOID = @idTipoCursoCapacitacao) ";

                var contextQuery = new ContextQuery
                {
                    Command = strSQL
                };

                contextQuery.Parameters.Add("@idTipoCursoCapacitacao", idTipoCursoCapacitacao);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static ValidacaoDados Validar(AreasConhecimento areasConhecimento)
        {
            var validacao = new ValidacaoDados();
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = @"SELECT 1 FROM [AREACONHECIMENTO] WHERE  
                                    DESCRICAO = @DESCRICAO";

                if (areasConhecimento.AreaConhecimentoId != 0)
                    contextQuery.Command += " AND AREACONHECIMENTOID <> @AREACONHECIMENTOID ";

                contextQuery.Parameters.Add("@AREACONHECIMENTOID", areasConhecimento.AreaConhecimentoId);
                contextQuery.Parameters.Add("@DESCRICAO", areasConhecimento.Descricao);

                object obj = ctx.GetReturnValue(contextQuery);

                if (obj == null)
                {
                    validacao.Valido = true;
                }
                else
                {
                    validacao.Valido = false;
                    validacao.Mensagem = "Já existe uma Área de conhecimento com este nome.";

                }
            }

            return validacao;
        }

        public static int Alterar(AreasConhecimento areasConhecimento)
        {
            //Ver quais dados podem ser alterados
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" UPDATE AREACONHECIMENTO
                        SET DESCRICAO = @DESCRICAO
                        WHERE AREACONHECIMENTOID = @ID "
                    };

                    contextQuery.Parameters.Add("@ID", areasConhecimento.AreaConhecimentoId);
                    contextQuery.Parameters.Add("@DESCRICAO", areasConhecimento.Descricao);

                    return ctx.ApplyModifications(contextQuery);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static int Remover(int idAreaConhecimento)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = "DELETE FROM AREACONHECIMENTO WHERE AREACONHECIMENTOID = @ID "
                    };
                    contextQuery.Parameters.Add("@ID", idAreaConhecimento);

                    return ctx.ApplyModifications(contextQuery);
                }
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public static AreasConhecimento Carregar(int idAreaConhecimento)
        {
            try
            {
                AreasConhecimento AC = new AreasConhecimento();

                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = "SELECT * FROM AREACONHECIMENTO WHERE AREACONHECIMENTOID = @ID "
                    };
                    contextQuery.Parameters.Add("@ID", idAreaConhecimento);

                    using (var reader = ctx.GetDataReader(contextQuery))
                    {
                        while (reader.Read())
                        {
                            AC.Descricao = (string)reader["DESCRICAO"];
                        }
                    }
                    return AC;

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static int Inserir(AreasConhecimento areasConhecimento)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" INSERT INTO AREACONHECIMENTO
                            (DESCRICAO)
                            VALUES
                            (@DESCRICAO) "
                    };
                    contextQuery.Parameters.Add("@DESCRICAO", areasConhecimento.Descricao);

                    return ctx.ApplyModifications(contextQuery);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        public static ValidacaoDados ValidarExclusao(int idAreaConhecimento)
        {
            var validacao = new ValidacaoDados();

            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
                {
                    var contextQuery = new ContextQuery();

                    contextQuery.Command = @" SELECT DISTINCT 1 
                                                FROM [dbo].[CURSOCAPACITACAO] 
                                               WHERE AREACONHECIMENTOID= @idAreaConhecimento 
                                              
                                               UNION ALL

                                              SELECT DISTINCT 1 
                                                FROM [dbo].[LY_CAPACITACAO] 
                                               WHERE AREACONHECIMENTOID= @idAreaConhecimento ";

                    contextQuery.Parameters.Add("@idAreaConhecimento", idAreaConhecimento);

                    object obj = ctx.GetReturnValue(contextQuery);

                    if (obj == null)
                    {
                        validacao.Valido = true;
                    }
                    else
                    {
                        validacao.Valido = false;
                        validacao.Mensagem = "Esta Área de Conhecimento não pode ser excluída devido existir Cursos de Capacitação vinculados.";
                    }
                }
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return validacao;
        }
    }
}
