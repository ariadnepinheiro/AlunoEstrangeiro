using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN.Util;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN
{
    public class Capacitacao : RNBase
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
                                      WHERE C.PESSOA = @PESSOAID "
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

        public static QueryTable ConsultarOrdem(string pessoa)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();
            QueryTable qt = null;

            string sql = "SELECT MAX(ORDEM) AS ORDEM FROM LY_CAPACITACAO WHERE PESSOA=?";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, pessoa);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public static bool PossuiCapacitacaoEdEspecial(string pessoa)
        {
            //Data: 11/04/2013 - Alterado por: Lucas Collina - Solicitado por: Wagner Medeiros
            //A validação por esse tipo de curso (EspecificoEduEspecial) já existia. 
            //Com a criação do campo 'TIPOCURSOCAPACITACAOID' na tabela 'LY_CAPACITACAO', 
            //foi alterado para tratar pelo ID deste tipo de curso == '6'
            //string sql = @"select 1 from LY_CAPACITACAO where TIPO_CURSO='EspecificoEduEspecial' AND PESSOA = ?";

            string sql = @"SELECT 1 FROM LY_CAPACITACAO WHERE TIPOCURSOCAPACITACAOID = 6 AND PESSOA = ?";
            
            int qtd = ExecutarFuncao(sql, pessoa);

            if (qtd == 1)
            {
                return true;
            }

            return false;
        }

        public static ValidacaoDados Validar(Entidades.LyCapacitacao cursoCapacitacao)
        {
            var validacao = new ValidacaoDados();

            try
            {
                using (var contexto = DataContextBuilder.FromLyceum.UsingNoLock())
                {
                    var contextQuery = new ContextQuery();

                    string strSQL = @"SELECT 1 FROM [LY_CAPACITACAO] 
                                       WHERE PESSOA = @PESSOAID
                                         AND CAPACITACAO = @CURSOCAPACITACAO";

                    contextQuery.Parameters.Add("@PESSOAID", cursoCapacitacao.Pessoa);
                    contextQuery.Parameters.Add("@CURSOCAPACITACAO", cursoCapacitacao.Capacitacao);

                    if (cursoCapacitacao.Ordem > 0)
                    {
                        strSQL += " AND ORDEM <> @ORDEM";
                        contextQuery.Parameters.Add("@ORDEM", cursoCapacitacao.Ordem);
                    }

                    contextQuery.Command = strSQL;

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

        public static int InserirCurso(Entidades.LyCapacitacao lyCapacitacao)
        {
            try
            {
                using (var contexto = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" DECLARE @ORDEM INT;
                                     SELECT @ORDEM = ISNULL(MAX(ORDEM), 0) + 1 FROM LY_CAPACITACAO WHERE PESSOA = @PESSOA;
                                     INSERT INTO LY_CAPACITACAO
                                         (ORDEM, PESSOA, CAPACITACAO, DATA_CONCLUSAO, NOME_INSTITUICAO, CARGA_HORARIA, TIPOCURSOCAPACITACAOID, AREACONHECIMENTOID)
                                     VALUES
                                         (@ORDEM, @PESSOA, @CAPACITACAO, @DATA_CONCLUSAO, @NOME_INSTITUICAO, @CARGA_HORARIA, @TIPOCURSOCAPACITACAOID, @AREACONHECIMENTOID); "
                    };

                    contextQuery.Parameters.Add("@PESSOA", lyCapacitacao.Pessoa);
                    contextQuery.Parameters.Add("@CAPACITACAO", lyCapacitacao.Capacitacao);
                    contextQuery.Parameters.Add("@DATA_CONCLUSAO", lyCapacitacao.DataConclusao);
                    contextQuery.Parameters.Add("@NOME_INSTITUICAO", lyCapacitacao.NomeInstituicao);
                    contextQuery.Parameters.Add("@CARGA_HORARIA", lyCapacitacao.CargaHoraria);
                    contextQuery.Parameters.Add("@TIPOCURSOCAPACITACAOID", lyCapacitacao.TipoCursoCapacitacaoId);
                    contextQuery.Parameters.Add("@AREACONHECIMENTOID", lyCapacitacao.AreaConhecimentoId);

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

        public static int AlterarCurso(Entidades.LyCapacitacao lyCapacitacao)
        {
            //Ver quais dados podem ser alterados
            try
            {
                using (var contexto = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" UPDATE [LY_CAPACITACAO]
                                        SET CAPACITACAO = @CAPACITACAO,
                                        DATA_CONCLUSAO = @DATA_CONCLUSAO,
                                        NOME_INSTITUICAO = @NOME_INSTITUICAO,
                                        CARGA_HORARIA = @CARGA_HORARIA,
                                        TIPOCURSOCAPACITACAOID = @TIPOCURSOCAPACITACAOID,
                                        AREACONHECIMENTOID = @AREACONHECIMENTOID
                                    WHERE PESSOA = @PESSOA AND ORDEM = @ORDEM "
                    };

                    contextQuery.Parameters.Add("@PESSOA", Convert.ToInt32(lyCapacitacao.Pessoa));
                    contextQuery.Parameters.Add("@ORDEM", Convert.ToInt32(lyCapacitacao.Ordem));
                    contextQuery.Parameters.Add("@CAPACITACAO", lyCapacitacao.Capacitacao);
                    contextQuery.Parameters.Add("@DATA_CONCLUSAO", lyCapacitacao.DataConclusao);
                    contextQuery.Parameters.Add("@NOME_INSTITUICAO", lyCapacitacao.NomeInstituicao);
                    contextQuery.Parameters.Add("@CARGA_HORARIA", Convert.ToInt32(lyCapacitacao.CargaHoraria));
                    contextQuery.Parameters.Add("@TIPOCURSOCAPACITACAOID", Convert.ToInt32(lyCapacitacao.TipoCursoCapacitacaoId));
                    contextQuery.Parameters.Add("@AREACONHECIMENTOID", Convert.ToInt32(lyCapacitacao.AreaConhecimentoId));

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

        public static int RemoverCursoCapacitacao(int idOrdem, int idPessoa)
        {
            using (var contexto = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" DELETE FROM LY_CAPACITACAO 
                                        WHERE ORDEM = @ORDEM AND PESSOA = @PESSOA "
                    };

                    contextQuery.Parameters.Add("@ORDEM", idOrdem);
                    contextQuery.Parameters.Add("@PESSOA", idPessoa);

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
    }
}
