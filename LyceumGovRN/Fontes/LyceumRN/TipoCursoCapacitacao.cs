using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using System;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN
{
    public class TipoCursoCapacitacao
    {
        public static DataTable Listar()
        {
            return Listar(false);
        }

        public static DataTable Listar(bool cursoOferecidoSEEDUC)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                string strSQL = @" SELECT * FROM TIPOCURSOCAPACITACAO ";

                if (cursoOferecidoSEEDUC)
                {
                    strSQL += @" WHERE TIPOCURSOCAPACITACAOID IN 
                                    (SELECT DISTINCT TIPOCURSOCAPACITACAOID FROM CURSOCAPACITACAO_TIPOCURSOCAPACITACAO) ";
                }

                var contextQuery = new ContextQuery
                {
                    Command = strSQL
                };

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static ValidacaoDados Validar(Entidades.TiposCursosCapacitacao tipoCursoCapacitacao)
        {
            var validacao = new ValidacaoDados();
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = @"SELECT 1 FROM [tipocursocapacitacao] WHERE  
                                    DESCRICAO = @DESCRICAO";

                if (tipoCursoCapacitacao.TipoCursoCapacitacaoId != 0)
                    contextQuery.Command += " AND TIPOCURSOCAPACITACAOID <> @TIPOCURSOCAPACITACAOID ";

                contextQuery.Parameters.Add("@TIPOCURSOCAPACITACAOID", tipoCursoCapacitacao.TipoCursoCapacitacaoId);
                contextQuery.Parameters.Add("@DESCRICAO", tipoCursoCapacitacao.Descricao);

                object obj = ctx.GetReturnValue(contextQuery);

                if (obj == null)
                {
                    validacao.Valido = true;
                }
                else
                {
                    validacao.Valido = false;
                    validacao.Mensagem = "Já existe um Tipo de curso de capacitação com este nome.";

                }
            }

            return validacao;
        }

        public static int Alterar(Entidades.TiposCursosCapacitacao tipoCursoCapacitacao)
        {
            //Ver quais dados podem ser alterados
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" UPDATE tipocursocapacitacao
                        SET DESCRICAO = @DESCRICAO
                        WHERE TIPOCURSOCAPACITACAOID = @ID "
                    };

                    contextQuery.Parameters.Add("@ID", tipoCursoCapacitacao.TipoCursoCapacitacaoId);
                    contextQuery.Parameters.Add("@DESCRICAO", tipoCursoCapacitacao.Descricao);

                    return ctx.ApplyModifications(contextQuery);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static int Remover(int idTipoCursoCapacitacao)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = "DELETE FROM tipocursocapacitacao WHERE TIPOCURSOCAPACITACAOID = @ID "
                    };

                    contextQuery.Parameters.Add("@ID", idTipoCursoCapacitacao);

                    return ctx.ApplyModifications(contextQuery);
                }
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public static Entidades.TiposCursosCapacitacao Carregar(int idTipoCursoCapacitacao)
        {
            try
            {
                Entidades.TiposCursosCapacitacao TCC = new Entidades.TiposCursosCapacitacao();

                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = "SELECT * FROM tipocursocapacitacao WHERE TIPOCURSOCAPACITACAOID = @ID "
                    };
                    contextQuery.Parameters.Add("@ID", idTipoCursoCapacitacao);

                    using (var reader = ctx.GetDataReader(contextQuery))
                    {
                        while (reader.Read())
                        {
                            TCC.Descricao = (string)reader["DESCRICAO"];
                        }
                    }
                    return TCC;

                }
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public static int Inserir(Entidades.TiposCursosCapacitacao tipoCursoCapacitacao)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" INSERT INTO tipocursocapacitacao
                            (DESCRICAO)
                            VALUES
                            (@DESCRICAO) "
                    };
                    contextQuery.Parameters.Add("@DESCRICAO", tipoCursoCapacitacao.Descricao);

                    return ctx.ApplyModifications(contextQuery);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static ValidacaoDados ValidarExclusao(int idTipoCursoCapacitacao)
        {
            var validacao = new ValidacaoDados();

            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
                {
                    var contextQuery = new ContextQuery();

                    contextQuery.Command = "SELECT DISTINCT 1 FROM [dbo].[CURSOCAPACITACAO_TIPOCURSOCAPACITACAO] WHERE TIPOCURSOCAPACITACAOID= @idTipoCursoCapacitacao ";
                    contextQuery.Parameters.Add("@idTipoCursoCapacitacao", idTipoCursoCapacitacao);

                    object obj = ctx.GetReturnValue(contextQuery);

                    if (obj == null)
                    {
                        validacao.Valido = true;
                    }
                    else
                    {
                        validacao.Valido = false;
                        validacao.Mensagem = "Este Tipo de Curso não pode ser excluído devido existir Cursos de Capacitação vinculados.";
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

        public void Atualiza(Entidades.TiposCursosCapacitacao tipoCursoCapacitacao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE tipocursocapacitacao
                                                    SET DESCRICAO = @DESCRICAO
                                                    WHERE TIPOCURSOCAPACITACAOID = @ID ";

                contextQuery.Parameters.Add("@ID", tipoCursoCapacitacao.TipoCursoCapacitacaoId);
                contextQuery.Parameters.Add("@DESCRICAO", tipoCursoCapacitacao.Descricao);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }
    }
}
