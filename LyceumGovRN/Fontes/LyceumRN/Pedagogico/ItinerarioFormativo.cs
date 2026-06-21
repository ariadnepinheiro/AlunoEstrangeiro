using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using System.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.Pedagogico
{
    public class ItinerarioFormativo
    {
        public DataTable Lista()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT ITINERARIOFORMATIVOID,
                                           C.DESCRICAO AS CATEGORIA,
	                                       I.DESCRICAO,
	                                       I.OBJETIVO,
                                           I.OFERTA,
                                           I.ATIVO,
                                           I.USUARIOID,
                                           I.DATACADASTRO,
                                           I.DATAALTERACAO,
                                           C.CATEGORIAITINERARIOFORMATIVOID
                                    FROM   [Pedagogico].[ITINERARIOFORMATIVO] I(NOLOCK)
                                    INNER JOIN [Pedagogico].[CATEGORIAITINERARIOFORMATIVO] C ON C.CATEGORIAITINERARIOFORMATIVOID = I.CATEGORIAITINERARIOFORMATIVOID ";

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

        public DataTable ListaItinerarioFormativoAtivo()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT ITINERARIOFORMATIVOID,
                                           C.DESCRICAO AS CATEGORIA,
	                                       I.DESCRICAO,
	                                       I.OBJETIVO,
                                           I.OFERTA,
                                           I.ATIVO,
                                           I.USUARIOID,
                                           I.DATACADASTRO,
                                           I.DATAALTERACAO,
                                           C.CATEGORIAITINERARIOFORMATIVOID
                                    FROM   [Pedagogico].[ITINERARIOFORMATIVO] I (NOLOCK)
                                    INNER JOIN [Pedagogico].[CATEGORIAITINERARIOFORMATIVO] C ON C.CATEGORIAITINERARIOFORMATIVOID = I.CATEGORIAITINERARIOFORMATIVOID 
                                            WHERE I.ATIVO = 1";

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

        public ValidacaoDados Valida(Entidades.ItinerarioFormativo itinerarioFormativo, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (itinerarioFormativo == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (itinerarioFormativo.ItinerarioFormativoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (itinerarioFormativo.CategoriaItinerarioFormativoId <= 0)
            {
                mensagens.Add("Campo CATEGORIA é obrigatório.");
            }

            if (itinerarioFormativo.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
            }

            if (itinerarioFormativo.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe a descrição cadastrada
                    if (this.PossuiOutraDescricaoCadastradaPor(contexto, itinerarioFormativo.Descricao, itinerarioFormativo.ItinerarioFormativoId))
                    {
                        mensagens.Add("Esta DESCRIÇÃO já foi utilizada.");
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

        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int itinerarioFormativoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [Pedagogico].[ITINERARIOFORMATIVO] (NOLOCK)
                                WHERE DESCRICAO = @DESCRICAO
	                                AND ITINERARIOFORMATIVOID <> @ITINERARIOFORMATIVOID ";

            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);
            contextQuery.Parameters.Add("@ITINERARIOFORMATIVOID", SqlDbType.Int, itinerarioFormativoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.ItinerarioFormativo itinerarioFormativo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO [Pedagogico].[ITINERARIOFORMATIVO]
                                                        (CATEGORIAITINERARIOFORMATIVOID,
                                                         DESCRICAO,
	                                                     OBJETIVO,
                                                         OFERTA, 
                                                         ATIVO, 
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO) 
                                            VALUES      (@CATEGORIAITINERARIOFORMATIVOID,
                                                         @DESCRICAO,
	                                                     @OBJETIVO,
                                                         @OFERTA, 
                                                         @ATIVO, 
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@CATEGORIAITINERARIOFORMATIVOID", SqlDbType.Int, itinerarioFormativo.CategoriaItinerarioFormativoId);
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, itinerarioFormativo.Descricao);
                contextQuery.Parameters.Add("@OBJETIVO", SqlDbType.VarChar, itinerarioFormativo.Objetivo);
                contextQuery.Parameters.Add("@OFERTA", SqlDbType.Bit, itinerarioFormativo.Oferta);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, itinerarioFormativo.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, itinerarioFormativo.UsuarioId);
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

        public void Atualiza(Entidades.ItinerarioFormativo itinerarioFormativo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE [Pedagogico].[ITINERARIOFORMATIVO]
                                        SET    CATEGORIAITINERARIOFORMATIVOID = @CATEGORIAITINERARIOFORMATIVOID,
                                               DESCRICAO = @DESCRICAO,
	                                           OBJETIVO = @OBJETIVO, 
                                               OFERTA = @OFERTA,
                                               ATIVO = @ATIVO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  ITINERARIOFORMATIVOID = @ITINERARIOFORMATIVOID ";

                contextQuery.Parameters.Add("@CATEGORIAITINERARIOFORMATIVOID", SqlDbType.Int, itinerarioFormativo.CategoriaItinerarioFormativoId);
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, itinerarioFormativo.Descricao);
                contextQuery.Parameters.Add("@OBJETIVO", SqlDbType.VarChar, itinerarioFormativo.Objetivo);
                contextQuery.Parameters.Add("@OFERTA", SqlDbType.Bit, itinerarioFormativo.Oferta);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, itinerarioFormativo.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, itinerarioFormativo.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@ITINERARIOFORMATIVOID", SqlDbType.Int, itinerarioFormativo.ItinerarioFormativoId);

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

        public ValidacaoDados ValidaRemocao(int itinerarioFormativoId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            TrilhaAprendizagem rnTrilhaAprendizagem = new TrilhaAprendizagem();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (itinerarioFormativoId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se ja foi utilizado em algum CURSO
                    if (rnTrilhaAprendizagem.PossuiItinerarioFormativoPor(contexto, itinerarioFormativoId))
                    {
                        mensagens.Add("Registro não pode ser excluído pois possui trilhas cadastradas.");
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

        public void Remove(int itinerarioFormativoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE [Pedagogico].[ITINERARIOFORMATIVO]
                            WHERE  ITINERARIOFORMATIVOID = @ITINERARIOFORMATIVOID  ";

                contextQuery.Parameters.Add("@ITINERARIOFORMATIVOID", SqlDbType.Int, itinerarioFormativoId);

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


        public bool EhItinerarioAtivoPor(int itinerarioFormativoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool ativo = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(1)
                                        FROM   [Pedagogico].[ITINERARIOFORMATIVO]
                            WHERE  ITINERARIOFORMATIVOID = @ITINERARIOFORMATIVOID   ";

                contextQuery.Parameters.Add("@ITINERARIOFORMATIVOID", SqlDbType.Int, itinerarioFormativoId);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    ativo = true;
                }

                return ativo;
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

        public bool PossuiCategoriaPor(DataContext ctx, int categoriaItinerarioFormativoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [Pedagogico].[ITINERARIOFORMATIVO]
                                WHERE CATEGORIAITINERARIOFORMATIVOID = @CATEGORIAITINERARIOFORMATIVOID ";

            contextQuery.Parameters.Add("@CATEGORIAITINERARIOFORMATIVOID", SqlDbType.Int, categoriaItinerarioFormativoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }
    }
}
