using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Pedagogico
{
    public class ComposicaoItinerarioFormativo : RNBase
    {
        public DataTable Lista(int areaItinerarioId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT COMPOSICAOITINERARIOFORMATIVOID,
                                           AREAITINERARIOFORMATIVOID,
	                                       DESCRICAO,
                                           ATIVO,
                                           USUARIOID,
                                           DATACADASTRO,
                                           DATAALTERACAO
                                    FROM   [Pedagogico].[COMPOSICAOITINERARIOFORMATIVO] (NOLOCK) ";

                contextQuery.Parameters.Add("@AREAITINERARIOFORMATIVOID", SqlDbType.Int, areaItinerarioId);


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

        public DataTable ListaComposicaoItinerarioFormativoAtivo(int areaItinerarioId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT COMPOSICAOITINERARIOFORMATIVOID,
                                           AREAITINERARIOFORMATIVOID,
	                                       DESCRICAO,
                                           ATIVO,
                                           USUARIOID,
                                           DATACADASTRO,
                                           DATAALTERACAO
                                    FROM   [Pedagogico].[COMPOSICAOITINERARIOFORMATIVO] (NOLOCK)
                                            WHERE ATIVO = 1
                                            AND AREAITINERARIOFORMATIVOID = @AREAITINERARIOFORMATIVOID ";

                contextQuery.Parameters.Add("@AREAITINERARIOFORMATIVOID", SqlDbType.Int, areaItinerarioId);

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

        public ValidacaoDados Valida(Entidades.ComposicaoItinerarioFormativo composicaoItinerario, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (composicaoItinerario == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (composicaoItinerario.ComposicaoItinerarioFormativoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (composicaoItinerario.AreaItinerarioFormativoId <= 0)
            {
                mensagens.Add("Campo ÁREA ITINERÁRIO FORMATIVO é obrigatório.");
            }

            if (composicaoItinerario.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
            }

            if (composicaoItinerario.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe a descrição cadastrada
                    if (this.PossuiOutraDescricaoCadastradaPor(contexto, composicaoItinerario.Descricao, composicaoItinerario.ComposicaoItinerarioFormativoId, composicaoItinerario.AreaItinerarioFormativoId))
                    {
                        mensagens.Add("Esta DESCRIÇÃO já foi utilizada para está ÁREA ITINERÁRIO FORMATIVO.");
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

        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int composicaoItinerarioId, int areaItinerarioId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [Pedagogico].[COMPOSICAOITINERARIOFORMATIVO] (NOLOCK)
                                WHERE DESCRICAO = @DESCRICAO
                                    AND AREAITINERARIOFORMATIVOID = @AREAITINERARIOFORMATIVOID
	                                AND COMPOSICAOITINERARIOFORMATIVOID <> @COMPOSICAOITINERARIOFORMATIVOID ";

            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);
            contextQuery.Parameters.Add("@AREAITINERARIOFORMATIVOID", SqlDbType.Int, areaItinerarioId);
            contextQuery.Parameters.Add("@COMPOSICAOITINERARIOFORMATIVOID", SqlDbType.Int, composicaoItinerarioId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.ComposicaoItinerarioFormativo composicaoItinerario)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO [Pedagogico].[COMPOSICAOITINERARIOFORMATIVO]
                                                        (AREAITINERARIOFORMATIVOID,
                                                         DESCRICAO, 
                                                         ATIVO, 
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO) 
                                            VALUES      (@AREAITINERARIOFORMATIVOID,
                                                         @DESCRICAO, 
                                                         @ATIVO, 
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@AREAITINERARIOFORMATIVOID", SqlDbType.Int, composicaoItinerario.AreaItinerarioFormativoId);
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, composicaoItinerario.Descricao);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, composicaoItinerario.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, composicaoItinerario.UsuarioId);
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

        public void Atualiza(Entidades.ComposicaoItinerarioFormativo composicaoItinerario)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE [Pedagogico].[COMPOSICAOITINERARIOFORMATIVO]
                                        SET    AREAITINERARIOFORMATIVOID = @AREAITINERARIOFORMATIVOID,
                                               DESCRICAO = @DESCRICAO, 
                                               ATIVO = @ATIVO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  COMPOSICAOITINERARIOFORMATIVOID = @COMPOSICAOITINERARIOFORMATIVOID ";

                contextQuery.Parameters.Add("@AREAITINERARIOFORMATIVOID", SqlDbType.Int, composicaoItinerario.AreaItinerarioFormativoId);
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, composicaoItinerario.Descricao);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, composicaoItinerario.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, composicaoItinerario.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@COMPOSICAOITINERARIOFORMATIVOID", SqlDbType.Int, composicaoItinerario.ComposicaoItinerarioFormativoId);

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

        public ValidacaoDados ValidaRemocao(int composicaoItinerarioId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.Curso rnCurso = new Curso();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (composicaoItinerarioId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se ja foi utilizado em algum CURSO
                    if (rnCurso.PossuiComposicaoItinerarioFormativoPor(contexto, composicaoItinerarioId))
                    {
                        mensagens.Add("Registro não pode ser excluído pois já foi utilizado.");
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

        public void Remove(int composicaoItinerarioId)
        { 
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE [Pedagogico].[COMPOSICAOITINERARIOFORMATIVO]
                            WHERE  COMPOSICAOITINERARIOFORMATIVOID = @COMPOSICAOITINERARIOFORMATIVOID  ";

                contextQuery.Parameters.Add("@COMPOSICAOITINERARIOFORMATIVOID", SqlDbType.Int, composicaoItinerarioId);

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


