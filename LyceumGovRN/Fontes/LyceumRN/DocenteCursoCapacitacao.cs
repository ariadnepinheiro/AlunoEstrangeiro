using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using System;
using Techne.Lyceum.RN.Util;
using System.Collections.Generic;
using Techne.Data;

namespace Techne.Lyceum.RN
{
    public class DocenteCursoCapacitacao
    {
        public static DataTable Listar(int idPessoa)
        {
            try
            {
                using (var contexto = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" SELECT C.[PESSOA] AS PESSOAID,
                                            C.[ORDEM] AS CURSOCAPACITACAOID,
	                                        C.[CAPACITACAO] AS NOMECURSO,
	                                        C.[DATA_CONCLUSAO] AS DATACONCLUSAO,
	                                        C.[NOME_INSTITUICAO] AS NOMEINSTITUICAO,
	                                        C.[CARGA_HORARIA] AS CARGAHORARIA,
	                                        0 AS OFERECIDOSEEDUC,
                                            C.[AREACONHECIMENTOID],
                                            C.[TIPOCURSOCAPACITACAOID]
                                       FROM [DBO].[LY_CAPACITACAO] C INNER JOIN [DBO].[TIPOCURSOCAPACITACAO] T
                                         ON C.TIPOCURSOCAPACITACAOID = T.TIPOCURSOCAPACITACAOID
                                      WHERE C.PESSOA = @PESSOAID

                                      UNION ALL

                                     SELECT P.[PESSOAID],
                                            C.[CURSOCAPACITACAOID], 
		                                    C.[NOMECURSO], 
		                                    C.[DATACONCLUSAO], 
		                                    C.[NOMEINSTITUICAO], 
		                                    C.[CARGAHORARIA], 		
		                                    C.[OFERECIDOSEEDUC],
                                            A.[AREACONHECIMENTOID],
                                            T.[TIPOCURSOCAPACITACAOID]
                                       FROM [dbo].[PESSOA_CURSOCAPACITACAO] P INNER JOIN [dbo].CURSOCAPACITACAO C 
                                         ON P.CURSOCAPACITACAOID = C.CURSOCAPACITACAOID
                                      INNER JOIN [dbo].[AREACONHECIMENTO] A ON A.AREACONHECIMENTOID = C.AREACONHECIMENTOID
                                      CROSS APPLY (SELECT TOP 1 TIPOCURSOCAPACITACAOID FROM [dbo].[CURSOCAPACITACAO_TIPOCURSOCAPACITACAO] CTC
                                                       WHERE CTC.CURSOCAPACITACAOID = C.CURSOCAPACITACAOID 
                                                   ) T (TIPOCURSOCAPACITACAOID) 
                                      WHERE P.PESSOAID = @PESSOAID "
                    };

                    contextQuery.Parameters.Add("@PESSOAID", idPessoa);

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
                        Command = @" SELECT C.CURSOCAPACITACAOID, 
                                            C.NOMECURSO, 
                                            C.NOMEINSTITUICAO, 
                                            C.DATACONCLUSAO, 
                                            C.CARGAHORARIA
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

        public static RN.Entidades.DocenteCursoCapacitacao Carregar(int idPessoa, int idCursoCapacitacao)
        {
            try
            {
                RN.Entidades.DocenteCursoCapacitacao entidadeDocenteCursoCapacitacao = new RN.Entidades.DocenteCursoCapacitacao();

                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"SELECT * FROM PESSOA_CURSOCAPACITACAO 
                                        WHERE CURSOCAPACITACAOID = @CURSOCAPACITACAOID AND PESSOAID = @PESSOAID "
                    };

                    contextQuery.Parameters.Add("@PESSOAID", idPessoa);
                    contextQuery.Parameters.Add("@CURSOCAPACITACAOID", idCursoCapacitacao);

                    using (var reader = ctx.GetDataReader(contextQuery))
                    {
                        while (reader.Read())
                        {
                            entidadeDocenteCursoCapacitacao.CursoCapacitacaoId = Convert.ToInt32(reader["CURSOCAPACITACAOID"]);
                            entidadeDocenteCursoCapacitacao.DataAtualizacao = Convert.ToDateTime(reader["DATAATUALIZACAO"]);
                            entidadeDocenteCursoCapacitacao.Matricula = (string)reader["MATRICULA"];
                            entidadeDocenteCursoCapacitacao.PessoaId = Convert.ToInt32(reader["PESSOAID"]);
                        }
                    }

                    return entidadeDocenteCursoCapacitacao;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        public static ValidacaoDados Validar(Entidades.DocenteCursoCapacitacao cursoCapacitacao)
        {
            var validacao = new ValidacaoDados();

            try
            {
                using (var contexto = DataContextBuilder.FromLyceum.UsingNoLock())
                {
                    var contextQuery = new ContextQuery();

                    contextQuery.Command = @"SELECT 1 FROM [PESSOA_CURSOCAPACITACAO] 
                                                WHERE PESSOAID = @PESSOAID
                                                  AND CURSOCAPACITACAOID = @CURSOCAPACITACAOID";

                    contextQuery.Parameters.Add("@PESSOAID", cursoCapacitacao.PessoaId);
                    contextQuery.Parameters.Add("@CURSOCAPACITACAOID", cursoCapacitacao.CursoCapacitacaoId);

                    object obj = contexto.GetReturnValue(contextQuery);

                    if (obj == null)
                    {
                        validacao.Valido = true;
                    }
                    else
                    {
                        validacao.Valido = false;
                        validacao.Mensagem = "Curso de Capacitação já informado para o Docente.";
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

        public static int RemoverCursoCapacitacao(int idCursoCapacitacao, int idPessoa)
        {
            using (var contexto = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" DELETE FROM PESSOA_CURSOCAPACITACAO 
                                        WHERE CURSOCAPACITACAOID = @CURSOCAPACITACAOID AND PESSOAID = @PESSOAID "
                    };

                    contextQuery.Parameters.Add("@CURSOCAPACITACAOID", idCursoCapacitacao);
                    contextQuery.Parameters.Add("@PESSOAID", idPessoa);

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

        public static int InserirCurso(Entidades.DocenteCursoCapacitacao docenteCursoCapacitacao)
        {
            try
            {
                using (var contexto = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" INSERT INTO PESSOA_CURSOCAPACITACAO
                                         (PESSOAID, CURSOCAPACITACAOID, DATAATUALIZACAO, MATRICULA)
                                     VALUES
                                         (@PESSOAID, @CURSOCAPACITACAOID, @DATAATUALIZACAO, @MATRICULA) "
                    };

                    contextQuery.Parameters.Add("@PESSOAID", docenteCursoCapacitacao.PessoaId);
                    contextQuery.Parameters.Add("@CURSOCAPACITACAOID", docenteCursoCapacitacao.CursoCapacitacaoId);
                    contextQuery.Parameters.Add("@DATAATUALIZACAO", docenteCursoCapacitacao.DataAtualizacao);
                    contextQuery.Parameters.Add("@MATRICULA", docenteCursoCapacitacao.Matricula);

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
    }
}
