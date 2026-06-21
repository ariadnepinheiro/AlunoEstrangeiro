using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Pedagogico
{
    public class TrilhaAprendizagem
    {
        public bool PossuiItinerarioFormativoPor(DataContext ctx, int itinerarioFormativoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [Pedagogico].[TRILHAAPRENDIZAGEM]
                                WHERE ITINERARIOFORMATIVOID = @ITINERARIOFORMATIVOID ";

            contextQuery.Parameters.Add("@ITINERARIOFORMATIVOID", SqlDbType.Int, itinerarioFormativoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable Lista()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT T.TRILHAAPRENDIZAGEMID,
                                           T.ITINERARIOFORMATIVOID,
                                           I.DESCRICAO AS ITINERARIO,
	                                       T.DESCRICAO,
	                                       T.OBJETIVO,
	                                       T.OFERTA,
	                                       T.TIPO,
                                           T.ATIVO,
                                           T.USUARIOID,
                                           T.DATACADASTRO,
                                           T.DATAALTERACAO
                                    FROM   [Pedagogico].[TRILHAAPRENDIZAGEM] (NOLOCK) T
                                    INNER JOIN  [Pedagogico].[ITINERARIOFORMATIVO] (NOLOCK) I ON T.ITINERARIOFORMATIVOID = I.ITINERARIOFORMATIVOID";

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

        public DataTable Lista(int itinerarioFormativoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT T.TRILHAAPRENDIZAGEMID,
                                           T.ITINERARIOFORMATIVOID,
                                           I.DESCRICAO AS ITINERARIO,
	                                       T.DESCRICAO,
	                                       T.TIPO,
	                                       T.OBJETIVO,
	                                       T.OFERTA,
                                           T.ATIVO,
                                           T.USUARIOID,
                                           T.DATACADASTRO,
                                           T.DATAALTERACAO
                                    FROM   [Pedagogico].[TRILHAAPRENDIZAGEM] (NOLOCK) T
                                    INNER JOIN  [Pedagogico].[ITINERARIOFORMATIVO] (NOLOCK) I ON T.ITINERARIOFORMATIVOID = I.ITINERARIOFORMATIVOID
                                            WHERE T.ITINERARIOFORMATIVOID = @ITINERARIOFORMATIVOID ";

                contextQuery.Parameters.Add("@ITINERARIOFORMATIVOID", SqlDbType.Int, itinerarioFormativoId);

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

        public DataTable ListaTrilhaAprendizagemAtivoPor(int itinerarioFormativoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT TRILHAAPRENDIZAGEMID,
                                           ITINERARIOFORMATIVOID,
	                                       DESCRICAO,
	                                       TIPO,
	                                       OBJETIVO,
	                                       OFERTA,
                                           ATIVO,
                                           USUARIOID,
                                           DATACADASTRO,
                                           DATAALTERACAO
                                    FROM   [Pedagogico].[TRILHAAPRENDIZAGEM] (NOLOCK)
                                            WHERE ATIVO = 1
                                            AND ITINERARIOFORMATIVOID = @ITINERARIOFORMATIVOID ";

                contextQuery.Parameters.Add("@ITINERARIOFORMATIVOID", SqlDbType.Int, itinerarioFormativoId);

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

        public string ObtemTipoTrilhaPor(DataContext contexto, string curso)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT TR.TIPO
                            FROM    LY_CURSO C  
									INNER JOIN Pedagogico.TRILHAAPRENDIZAGEM TR ON C.TRILHAAPRENDIZAGEMID = TR.TRILHAAPRENDIZAGEMID
                            WHERE   CURSO = @CURSO ";

            contextQuery.Parameters.Add("@CURSO", TechneDbType.T_CODIGO, curso); 

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public ValidacaoDados Valida(Entidades.TrilhaAprendizagem trilhaAprendizagem, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (trilhaAprendizagem == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (trilhaAprendizagem.TrilhaAprendizagemId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (trilhaAprendizagem.ItinerarioFormativoId <= 0)
            {
                mensagens.Add("Campo ITINERÁRIO FORMATIVO é obrigatório.");
            }

            if (trilhaAprendizagem.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
            }

            if (trilhaAprendizagem.Tipo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TIPO é obrigatório.");
            }
            else
            {
                if (trilhaAprendizagem.Tipo != "APROFUNDAMENTO" && trilhaAprendizagem.Tipo != "PROFISSIONALIZANTE")
                {
                    mensagens.Add("Campo TIPO apenas aceita as opções APROFUNDAMENTO e PROFISSIONALIZANTE.");
                }
            }

            if (trilhaAprendizagem.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe a descrição cadastrada
                    if (this.PossuiOutraDescricaoCadastradaPor(contexto, trilhaAprendizagem.Descricao, trilhaAprendizagem.TrilhaAprendizagemId))
                    {
                        mensagens.Add("Esta DESCRIÇÃO já foi utilizada para outra TRILHA DE APRENDIZAGEM.");
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

        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int trilhaAprendizagemId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [Pedagogico].[TRILHAAPRENDIZAGEM] (NOLOCK)
                                WHERE DESCRICAO = @DESCRICAO
                                    AND TRILHAAPRENDIZAGEMID <> @TRILHAAPRENDIZAGEMID ";

            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);
            contextQuery.Parameters.Add("@TRILHAAPRENDIZAGEMID", SqlDbType.Int, trilhaAprendizagemId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.TrilhaAprendizagem trilhaAprendizagem)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO [Pedagogico].[TRILHAAPRENDIZAGEM]
                                                        (ITINERARIOFORMATIVOID,
                                                         DESCRICAO,
	                                                     TIPO,
	                                                     OBJETIVO, 
	                                                     OFERTA,
                                                         ATIVO, 
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO) 
                                            VALUES      (@ITINERARIOFORMATIVOID,
                                                         @DESCRICAO,
	                                                     @TIPO,
	                                                     @OBJETIVO, 
	                                                     @OFERTA, 
                                                         @ATIVO, 
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@ITINERARIOFORMATIVOID", SqlDbType.Int, trilhaAprendizagem.ItinerarioFormativoId);
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, trilhaAprendizagem.Descricao);
                contextQuery.Parameters.Add("@TIPO", SqlDbType.VarChar, trilhaAprendizagem.Tipo);
                contextQuery.Parameters.Add("@OBJETIVO", SqlDbType.VarChar, trilhaAprendizagem.Objetivo);
                contextQuery.Parameters.Add("@OFERTA", SqlDbType.Bit, trilhaAprendizagem.Oferta);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, trilhaAprendizagem.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, trilhaAprendizagem.UsuarioId);
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

        public void Atualiza(Entidades.TrilhaAprendizagem trilhaAprendizagem)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE [Pedagogico].[TRILHAAPRENDIZAGEM]
                                        SET    ITINERARIOFORMATIVOID = @ITINERARIOFORMATIVOID,
                                               DESCRICAO = @DESCRICAO,
                                               TIPO = @TIPO,
                                               OBJETIVO = @OBJETIVO, 
                                               OFERTA = @OFERTA,
                                               ATIVO = @ATIVO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  TRILHAAPRENDIZAGEMID = @TRILHAAPRENDIZAGEMID ";

                contextQuery.Parameters.Add("@ITINERARIOFORMATIVOID", SqlDbType.Int, trilhaAprendizagem.ItinerarioFormativoId);
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, trilhaAprendizagem.Descricao);
                contextQuery.Parameters.Add("@TIPO", SqlDbType.VarChar, trilhaAprendizagem.Tipo);
                contextQuery.Parameters.Add("@OBJETIVO", SqlDbType.VarChar, trilhaAprendizagem.Objetivo);
                contextQuery.Parameters.Add("@OFERTA", SqlDbType.Bit, trilhaAprendizagem.Oferta);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, trilhaAprendizagem.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, trilhaAprendizagem.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@TRILHAAPRENDIZAGEMID", SqlDbType.Int, trilhaAprendizagem.TrilhaAprendizagemId);

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

        public ValidacaoDados ValidaRemocao(int trilhaAprendizagemId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.Curso rnCurso = new Curso();
            TrilhaAprendizagemEscola rnTrilhaAprendizagemEscola = new TrilhaAprendizagemEscola();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (trilhaAprendizagemId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se ja foi utilizado em algum CURSO
                    if (rnCurso.PossuiTrilhaAprendizagemPor(contexto, trilhaAprendizagemId))
                    {
                        mensagens.Add("Registro não pode ser excluído pois já foi utilizado em uma escolaridade.");
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

        public void Remove(int trilhaAprendizagemId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE [Pedagogico].[TRILHAAPRENDIZAGEM]
                            WHERE  TRILHAAPRENDIZAGEMID = @TRILHAAPRENDIZAGEMID  ";

                contextQuery.Parameters.Add("@TRILHAAPRENDIZAGEMID", SqlDbType.Int, trilhaAprendizagemId);

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

        public bool EhTrilhaAtivaPor(int trilhaAprendizagemId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool ativo = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(1)
                                        FROM   [Pedagogico].[TRILHAAPRENDIZAGEM]
                            WHERE  TRILHAAPRENDIZAGEMID = @TRILHAAPRENDIZAGEMID 
                                    AND ATIVO = 1 ";

                contextQuery.Parameters.Add("@TRILHAAPRENDIZAGEMID", SqlDbType.Int, trilhaAprendizagemId);

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
    }
}
