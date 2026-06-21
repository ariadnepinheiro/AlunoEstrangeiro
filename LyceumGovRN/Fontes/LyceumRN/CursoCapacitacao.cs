using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using System;
using Techne.Lyceum.RN.Util;
using System.Collections.Generic;
using Techne.Data;

namespace Techne.Lyceum.RN
{
    public class CursoCapacitacao
    {
        public static DataTable Listar()
        {
            try
            {
                using (var contexto = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" SELECT A.[AREACONHECIMENTOID], A.[DESCRICAO] AS AREACONHECIMENTO, C.[CURSOCAPACITACAOID]
                                      , C.[NOMECURSO], C.[NOMEINSTITUICAO], C.[CARGAHORARIA]
                                      , C.[DATAINICIO], C.[DATACONCLUSAO], C.[OFERECIDOSEEDUC],
										SUBSTRING(TIPOSCURSO, 0, LEN(TIPOSCURSO)) AS TIPOSCURSO
                                 FROM CURSOCAPACITACAO C
                                 INNER JOIN AREACONHECIMENTO A 
                                 ON A.[AREACONHECIMENTOID] = C.[AREACONHECIMENTOID]
                                 CROSS APPLY (SELECT DESCRICAO + ', '
			                                  FROM CURSOCAPACITACAO_TIPOCURSOCAPACITACAO CP
			                                  INNER JOIN TIPOCURSOCAPACITACAO  TCP
			                                  ON CP.TIPOCURSOCAPACITACAOID = TCP.TIPOCURSOCAPACITACAOID
			                                  WHERE CP.CURSOCAPACITACAOID = C.CURSOCAPACITACAOID
			                                  ORDER BY DESCRICAO
			                                  FOR XML PATH('')) D (TIPOSCURSO) "
                    };

                    return contexto.GetDataTable(contextQuery);
                }
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
        }

        public static DataTable ListarCursoCapacitacaoOferecidoSEEDUC(int idTipoCursoCapacitacao, int idAreaConhecimentoCapacitacao)
        {
            try
            {
                using (var contexto = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" SELECT C.CURSOCAPACITACAOID, C.NOMECURSO, C.NOMEINSTITUICAO, C.DATACONCLUSAO, C.CARGAHORARIA
                                     FROM CURSOCAPACITACAO C
                                     INNER JOIN CURSOCAPACITACAO_TIPOCURSOCAPACITACAO TCC 
                                        ON C.[CURSOCAPACITACAOID] = TCC.[CURSOCAPACITACAOID]
                                     WHERE TCC.TIPOCURSOCAPACITACAOID = @idTipoCursoCapacitacao
                                       AND C.AREACONHECIMENTOID = @idAreaConhecimentoCapacitacao
			                         ORDER BY C.NOMECURSO "
                    };

                    contextQuery.Parameters.Add("@idTipoCursoCapacitacao", idTipoCursoCapacitacao);
                    contextQuery.Parameters.Add("@idAreaConhecimentoCapacitacao", idAreaConhecimentoCapacitacao);

                    return contexto.GetDataTable(contextQuery);
                }
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
        }

        public static RN.Entidades.CursoCapacitacao Carregar(int idCursoCapacitacao)
        {
            try
            {
                RN.Entidades.CursoCapacitacao entidadeCursoCapacitacao = new RN.Entidades.CursoCapacitacao();

                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = "SELECT * FROM CURSOCAPACITACAO WHERE CURSOCAPACITACAOID = @ID "
                    };

                    contextQuery.Parameters.Add("@ID", idCursoCapacitacao);

                    using (var reader = ctx.GetDataReader(contextQuery))
                    {
                        while (reader.Read())
                        {
                            entidadeCursoCapacitacao.AreaConhecimentoId = Convert.ToInt32(reader["AREACONHECIMENTOID"]);
                            entidadeCursoCapacitacao.CargaHoraria = Convert.ToInt32(reader["CARGAHORARIA"]);
                            entidadeCursoCapacitacao.CursoCapacitacaoId = Convert.ToInt32(reader["CURSOCAPACITACAOID"]);
                            entidadeCursoCapacitacao.DataConclusao = Convert.ToDateTime(reader["DATACONCLUSAO"]);
                            entidadeCursoCapacitacao.DataInicio = Convert.ToDateTime(reader["DATAINICIO"]);
                            entidadeCursoCapacitacao.NomeCurso = (string)reader["NOMECURSO"];
                            entidadeCursoCapacitacao.NomeInstituicao = (string)reader["NOMEINSTITUICAO"];
                            entidadeCursoCapacitacao.OferecidoSeeduc = (bool)reader["OFERECIDOSEEDUC"];
                        }
                    }
                    return entidadeCursoCapacitacao;

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        public static ValidacaoDados Validar(Entidades.CursoCapacitacao cursoCapacitacao)
        {
            var validacao = new ValidacaoDados();

            try
            {
                using (var contexto = DataContextBuilder.FromLyceum.UsingNoLock())
                {
                    var contextQuery = new ContextQuery();

                    contextQuery.Command = @"SELECT 1 FROM [CURSOCAPACITACAO] 
                                                WHERE NOMECURSO = @NOMECURSO";

                    if (cursoCapacitacao.CursoCapacitacaoId != 0)
                    {
                        contextQuery.Command += " AND CURSOCAPACITACAOID <> @CURSOCAPACITACAOID ";
                        contextQuery.Parameters.Add("@CURSOCAPACITACAOID", cursoCapacitacao.CursoCapacitacaoId);
                    }

                    contextQuery.Parameters.Add("@NOMECURSO", cursoCapacitacao.NomeCurso);

                    object obj = contexto.GetReturnValue(contextQuery);

                    if (obj == null)
                    {
                        validacao.Valido = true;
                    }
                    else
                    {
                        validacao.Valido = false;
                        validacao.Mensagem = "Já existe um Curso de Capacitação com este nome.";
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

        public static int AlterarCurso(Entidades.CursoCapacitacao cursoCapacitacao)
        {
            //Ver quais dados podem ser alterados
            try
            {
                using (var contexto = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" UPDATE [CURSOCAPACITACAO]
                                        SET AREACONHECIMENTOID = @AREACONHECIMENTOID,
                                        NOMECURSO = @NOMECURSO,
                                        NOMEINSTITUICAO = @NOMEINSTITUICAO,
                                        CARGAHORARIA = @CARGAHORARIA,
                                        DATAINICIO = @DATAINICIO,
                                        DATACONCLUSAO = @DATACONCLUSAO,
                                        OFERECIDOSEEDUC = @OFERECIDOSEEDUC
                                    WHERE CURSOCAPACITACAOID = @ID "
                    };

                    contextQuery.Parameters.Add("@ID", cursoCapacitacao.CursoCapacitacaoId);
                    contextQuery.Parameters.Add("@AREACONHECIMENTOID", cursoCapacitacao.AreaConhecimentoId);
                    contextQuery.Parameters.Add("@NOMECURSO", cursoCapacitacao.NomeCurso);
                    contextQuery.Parameters.Add("@NOMEINSTITUICAO", cursoCapacitacao.NomeInstituicao);
                    contextQuery.Parameters.Add("@CARGAHORARIA", cursoCapacitacao.CargaHoraria);
                    contextQuery.Parameters.Add("@DATAINICIO", cursoCapacitacao.DataInicio);
                    contextQuery.Parameters.Add("@DATACONCLUSAO", cursoCapacitacao.DataConclusao);
                    contextQuery.Parameters.Add("@OFERECIDOSEEDUC", Convert.ToInt32(cursoCapacitacao.OferecidoSeeduc));

                    return contexto.ApplyModifications(contextQuery);
                }
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
        }

        public static int RemoverCurso(int idCursoCapacitacao)
        {
            using (var contexto = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    RemoverTiposCurso(contexto, idCursoCapacitacao);

                    var contextQuery = new ContextQuery
                    {
                        Command = " DELETE FROM CURSOCAPACITACAO WHERE CURSOCAPACITACAOID = @ID "
                    };

                    contextQuery.Parameters.Add("@ID", idCursoCapacitacao);

                    return contexto.ApplyModifications(contextQuery);
                }
                catch (Exception exception)
                {
                    contexto.Abandon();
                    string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine, Convert.ToString(exception.Message));
                    throw new Exception(mensagem);
                }
            }
        }

        public static int InserirCurso(Entidades.CursoCapacitacao cursoCapacitacao)
        {
            try
            {
                using (var contexto = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" INSERT INTO CURSOCAPACITACAO
                                         (AREACONHECIMENTOID,NOMECURSO,NOMEINSTITUICAO,CARGAHORARIA,DATAINICIO,DATACONCLUSAO,OFERECIDOSEEDUC)
                                     VALUES
                                         (@AREACONHECIMENTOID,@NOMECURSO,@NOMEINSTITUICAO,@CARGAHORARIA,@DATAINICIO,@DATACONCLUSAO,@OFERECIDOSEEDUC) "
                    };

                    contextQuery.Parameters.Add("@AREACONHECIMENTOID", cursoCapacitacao.AreaConhecimentoId);
                    contextQuery.Parameters.Add("@NOMECURSO", cursoCapacitacao.NomeCurso);
                    contextQuery.Parameters.Add("@NOMEINSTITUICAO", cursoCapacitacao.NomeInstituicao);
                    contextQuery.Parameters.Add("@CARGAHORARIA", cursoCapacitacao.CargaHoraria);
                    contextQuery.Parameters.Add("@DATAINICIO", cursoCapacitacao.DataInicio);
                    contextQuery.Parameters.Add("@DATACONCLUSAO", cursoCapacitacao.DataConclusao);
                    contextQuery.Parameters.Add("@OFERECIDOSEEDUC", Convert.ToInt32(cursoCapacitacao.OferecidoSeeduc));

                    return contexto.ApplyModifications(contextQuery);
                }
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
        }

        public static int InserirTiposCurso(ClasseGenerica classeGenerica)
        {
            using (DataContext contexto = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    RemoverTiposCurso(contexto, classeGenerica.Pai);

                    var contextQuery = new ContextQuery
                    {
                        Command = @"INSERT INTO [CURSOCAPACITACAO_TIPOCURSOCAPACITACAO]
                                        (CURSOCAPACITACAOID, TIPOCURSOCAPACITACAOID)
                                    VALUES
                                        (@CURSOCAPACITACAOID, @TIPOCURSOCAPACITACAOID) "
                    };

                    int retornoContexto = 0;

                    foreach (int filho in classeGenerica.Filhos)
                    {
                        contextQuery.Parameters.Clear();
                        contextQuery.Parameters.Add("@CURSOCAPACITACAOID", classeGenerica.Pai);
                        contextQuery.Parameters.Add("@TIPOCURSOCAPACITACAOID", filho);

                        retornoContexto = contexto.ApplyModifications(contextQuery);
                        if (retornoContexto < 0)
                        {
                            throw new Exception("");
                        }
                    }

                    return retornoContexto;
                }
                catch (Exception exception)
                {
                    contexto.Abandon();
                    contexto.Dispose();
                    string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine, Convert.ToString(exception.Message));
                    throw new Exception(mensagem);
                }
            }
        }

        public static void RemoverTiposCurso(DataContext context, int idCursoCapacitacao)
        {
            var contextQuery = new ContextQuery
            {
                Command = @" DELETE FROM [CURSOCAPACITACAO_TIPOCURSOCAPACITACAO]
                                WHERE CURSOCAPACITACAOID = @CURSOCAPACITACAOID "
            };

            contextQuery.Parameters.Add("@CURSOCAPACITACAOID", idCursoCapacitacao);
            context.ApplyModifications(contextQuery);
        }

        public static DataTable ListarTiposCurso(string idCursoCapacitacao)
        {
            try
            {
                using (var contexto = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"SELECT TIPOCURSOCAPACITACAOID
                                      FROM CURSOCAPACITACAO_TIPOCURSOCAPACITACAO
                                     WHERE CURSOCAPACITACAOID = @CURSOCAPACITACAOID"
                    };

                    contextQuery.Parameters.Add("@CURSOCAPACITACAOID", idCursoCapacitacao);

                    return contexto.GetDataTable(contextQuery);
                }
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
        }

        public static ValidacaoDados ValidarExclusao(int idCursoCapacitacao)
        {
            var validacao = new ValidacaoDados();

            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
                {
                    var contextQuery = new ContextQuery();

                    contextQuery.Command = "SELECT DISTINCT 1 FROM [dbo].[PESSOA_CURSOCAPACITACAO] WHERE CURSOCAPACITACAOID= @idCursoCapacitacao ";
                    contextQuery.Parameters.Add("@idCursoCapacitacao", idCursoCapacitacao);

                    object obj = ctx.GetReturnValue(contextQuery);

                    if (obj == null)
                    {
                        validacao.Valido = true;
                    }
                    else
                    {
                        validacao.Valido = false;
                        validacao.Mensagem = "Este Curso de Capacitação não pode ser excluído devido existir docentes vinculados.";
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

        public static object ObtemIdentityCursoCapacitacao()
        {
            try
            {
                TConnection connection = Config.CreateConnection();
                connection.Open();
                int ID;
                DbObject dbID;

                try
                {
                    dbID = TCommand.ExecuteScalar(connection, "SELECT MAX(CURSOCAPACITACAOID) FROM CURSOCAPACITACAO ", 1);
                }
                finally
                {
                    connection.Close();
                }

                if (!dbID.IsNull)
                {
                    ID = (int)dbID;
                    return ID;
                }
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return 1;
        }
    }
}
