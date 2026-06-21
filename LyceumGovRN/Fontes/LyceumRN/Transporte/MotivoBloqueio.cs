using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Transporte
{
    public class MotivoBloqueio : RNBase
    {
        public enum TipoMotivoBloqueio
        {
            [StringValue("Prestador")]
            Prestador = 1,
            [StringValue("Veiculo")]
            Veiculo = 2,
            [StringValue("Condutor")]
            Condutor = 3
        }

        public DataTable Lista()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT MOTIVOBLOQUEIOID,
	                                       DESCRICAO,
                                           TIPO,
                                           CASE 
												WHEN TIPO = 1 THEN 'PRESTADOR'
												WHEN TIPO = 2 THEN 'VEICULO'
												WHEN TIPO = 3 THEN 'CONDUTOR'
												ELSE ''
										   END TIPODESCRICAO,
                                           ATIVO,
                                           USUARIOID,
                                           DATACADASTRO,
                                           DATAALTERACAO
                                    FROM   [Transporte].[MOTIVOBLOQUEIO] (NOLOCK) ";

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Empty;
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

            return dt;
        }

        public DataTable ListaAtivoPor(int tipo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT MOTIVOBLOQUEIOID,
	                                       DESCRICAO,
                                           TIPO,
                                           CASE 
												WHEN TIPO = 1 THEN 'PRESTADOR'
												WHEN TIPO = 2 THEN 'VEICULO'
												WHEN TIPO = 3 THEN 'CONDUTOR'
												ELSE ''
										   END TIPODESCRICAO,
                                           ATIVO,
                                           USUARIOID,
                                           DATACADASTRO,
                                           DATAALTERACAO
                                    FROM   [Transporte].[MOTIVOBLOQUEIO] (NOLOCK)
                                            WHERE ATIVO = 1
                                                AND TIPO = @TIPO ";

                contextQuery.Parameters.Add("@TIPO", SqlDbType.Int, tipo);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Empty;
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

            return dt;
        }

        public ValidacaoDados Valida(Entidades.MotivoBloqueio motivoBloqueio, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (motivoBloqueio == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (motivoBloqueio.MotivoBloqueioId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (motivoBloqueio.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
            }
            else if (motivoBloqueio.Descricao.Length > 500)
            {
                mensagens.Add("Campo DESCRIÇÃO deve conter no máximo 500 caracteres.");
            }

            if (motivoBloqueio.Tipo <= 0)
            {
                mensagens.Add("Campo TIPO é obrigatório.");
            }
            else if (motivoBloqueio.Tipo != 1 && motivoBloqueio.Tipo != 2 && motivoBloqueio.Tipo != 3)
            {
                mensagens.Add("Campo TIPO inválido.");
            }

            if (motivoBloqueio.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe a descrição cadastrada
                    if (this.PossuiOutraDescricaoCadastradaPor(contexto, motivoBloqueio.Descricao, motivoBloqueio.MotivoBloqueioId, motivoBloqueio.Tipo))
                    {
                        mensagens.Add("Esta DESCRIÇÃO já foi utilizada para este tipo.");
                    }
                }
                catch (Exception ex)
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (contexto != null)
                    {
                        contexto.Dispose();
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

        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int motivoBloqueioId, int tipo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [Transporte].[MOTIVOBLOQUEIO] (NOLOCK)
                                WHERE DESCRICAO = @DESCRICAO
	                                AND TIPO = @TIPO
	                                AND MOTIVOBLOQUEIOID <> @MOTIVOBLOQUEIOID ";

            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);
            contextQuery.Parameters.Add("@MOTIVOBLOQUEIOID", SqlDbType.Int, motivoBloqueioId);
            contextQuery.Parameters.Add("@TIPO", SqlDbType.Int, tipo);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.MotivoBloqueio motivoBloqueio)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO Transporte.MOTIVOBLOQUEIO
                                                        (DESCRICAO, 
                                                         TIPO, 
                                                         ATIVO, 
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO) 
                                            VALUES      (@DESCRICAO, 
                                                         @TIPO, 
                                                         @ATIVO, 
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, motivoBloqueio.Descricao);
                contextQuery.Parameters.Add("@TIPO", SqlDbType.Int, motivoBloqueio.Tipo);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, motivoBloqueio.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, motivoBloqueio.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

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

        public void Atualiza(Entidades.MotivoBloqueio motivoBloqueio)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE Transporte.MOTIVOBLOQUEIO
                                        SET    DESCRICAO = @DESCRICAO, 
                                               TIPO = @TIPO, 
                                               ATIVO = @ATIVO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  MOTIVOBLOQUEIOID = @MOTIVOBLOQUEIOID ";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, motivoBloqueio.Descricao);
                contextQuery.Parameters.Add("@TIPO", SqlDbType.Int, motivoBloqueio.Tipo);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, motivoBloqueio.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, motivoBloqueio.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@MOTIVOBLOQUEIOID", SqlDbType.Int, motivoBloqueio.MotivoBloqueioId);

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

        public ValidacaoDados ValidaRemocao(int motivoBloqueioId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.Transporte.VeiculoBloqueio rnVeiculoBloqueio = new VeiculoBloqueio();
            RN.Transporte.PrestadorBloqueio rnPrestadorBloqueio = new PrestadorBloqueio();
            RN.Transporte.CondutorBloqueio rnCondutorBloqueio = new CondutorBloqueio();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (motivoBloqueioId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se motivo ja foi utilizado 
                    if (rnVeiculoBloqueio.PossuiBloqueioPor(contexto, motivoBloqueioId))
                    {
                        mensagens.Add("Registro não pode ser excluído, pois existem bloqueios associados a esse Motivo de Bloqueio.");
                    }
                    else if (rnCondutorBloqueio.PossuiBloqueioPor(contexto, motivoBloqueioId))
                    {
                        mensagens.Add("Registro não pode ser excluído, pois existem bloqueios associados a esse Motivo de Bloqueio.");
                    }
                    else if (rnPrestadorBloqueio.PossuiBloqueioPor(contexto, motivoBloqueioId))
                    {
                        mensagens.Add("Registro não pode ser excluído, pois existem bloqueios associados a esse Motivo de Bloqueio.");
                    }
                }
                catch
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw;
                }
                finally
                {
                    if (contexto != null)
                    {
                        contexto.Dispose();
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

        public void Remove(int motivoBloqueioId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE Transporte.MOTIVOBLOQUEIO
                            WHERE  MOTIVOBLOQUEIOID = @MOTIVOBLOQUEIOID  ";

                contextQuery.Parameters.Add("@MOTIVOBLOQUEIOID", SqlDbType.Int, motivoBloqueioId);

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
