using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.GestaoRede
{
    public class Superintendencia
    {
        public DataTable ListaAtivo()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"  SELECT  SUPERINTENDENCIAID, 
                                                   DESCRICAO
                                            FROM GestaoRede.SUPERINTENDENCIA (NOLOCK)
                                                 WHERE ATIVO = 1
                                            ORDER BY SUPERINTENDENCIAID ";

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
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
                contexto.Dispose();
            }

            return dt;
        }

        public bool PossuiSubsecretariaPor(DataContext contexto, int subsecretariaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM GESTAOREDE.SUPERINTENDENCIA (NOLOCK)
                                    WHERE SUBSECRETARIAID = @SUBSECRETARIAID ";

            contextQuery.Parameters.Add("@SUBSECRETARIAID", SqlDbType.Int, subsecretariaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiSuperintendenciaAtivaPor(DataContext contexto, int subsecretariaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM GESTAOREDE.SUPERINTENDENCIA (NOLOCK)
                                    WHERE SUBSECRETARIAID = @SUBSECRETARIAID
                                          AND ATIVO = 1 ";

            contextQuery.Parameters.Add("@SUBSECRETARIAID", SqlDbType.Int, subsecretariaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public ValidacaoDados Valida(Entidades.Superintendencia superintendencia, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (superintendencia == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (superintendencia.SuperintendenciaId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }
            if (superintendencia.SubsecretariaId <= 0)
            {
                mensagens.Add("Campo SUBSECRETRIA é obrigatório.");
            }
            if (superintendencia.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
            }
            else if (superintendencia.Descricao.Length > 100)
            {
                mensagens.Add("A DESCRIÇÃO deve possuir no máximo 100 caracteres.");
            }

            if (superintendencia.Setor.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE ADMINISTRATIVA é obrigatório.");
            }

            if (superintendencia.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe a Descricao cadastrada 
                    if (this.PossuiOutraDescricaoCadastradaPor(contexto, superintendencia.Descricao, superintendencia.SuperintendenciaId))
                    {
                        mensagens.Add("Esta DESCRIÇÃO já foi utilizada!");
                    }

                    //Verifica se já existe a ua cadastrada 
                    if (this.PossuiOutroSetorCadastradoPor(contexto, superintendencia.Setor, superintendencia.SuperintendenciaId))
                    {
                        mensagens.Add("Esta UNIDADE ADMINISTRATIVA já foi utilizada!");
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

        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int superintendenciaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM GESTAOREDE.SUPERINTENDENCIA
                                WHERE DESCRICAO = @DESCRICAO
	                                AND SUPERINTENDENCIAID <> @SUPERINTENDENCIAID ";

            contextQuery.Parameters.Add("@DESCRICAO", descricao);
            contextQuery.Parameters.Add("@SUPERINTENDENCIAID", superintendenciaId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutroSetorCadastradoPor(DataContext ctx, string setor, int superintendenciaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM GESTAOREDE.SUPERINTENDENCIA
                                WHERE SETOR = @SETOR
	                                AND SUPERINTENDENCIAID <> @SUPERINTENDENCIAID ";

            contextQuery.Parameters.Add("@SETOR", setor);
            contextQuery.Parameters.Add("@SUPERINTENDENCIAID", superintendenciaId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;

        }
        public void Insere(Entidades.Superintendencia Superintendencia)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO gestaorede.SUPERINTENDENCIA
                                                        (SUBSECRETARIAID,
                                                         DESCRICAO,
                                                         SETOR,
                                                         ATIVO,
                                                         USUARIOID,
                                                         DATACADASTRO,
                                                         DATAALTERACAO)
                                            VALUES      (@SUBSECRETARIAID,
                                                         @DESCRICAO,
                                                         @SETOR,
                                                         @ATIVO,
                                                         @USUARIOID,
                                                         @DATACADASTRO,
                                                         @DATAALTERACAO)  ";

                contextQuery.Parameters.Add("@SUBSECRETARIAID", Superintendencia.SubsecretariaId);
                contextQuery.Parameters.Add("@DESCRICAO", Superintendencia.Descricao);
                contextQuery.Parameters.Add("@SETOR", Superintendencia.Setor);
                contextQuery.Parameters.Add("@ATIVO", Superintendencia.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", Superintendencia.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

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

        public void Atualiza(Entidades.Superintendencia Superintendencia)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE GESTAOREDE.SUPERINTENDENCIA 
                                    SET    SUBSECRETARIAID = @SUBSECRETARIAID,
                                           DESCRICAO = @DESCRICAO,
                                           SETOR = @SETOR,
                                           ATIVO = @ATIVO,
                                           USUARIOID = @USUARIOID,
                                           DATAALTERACAO = @DATAALTERACAO
                                    WHERE  SUPERINTENDENCIAID = @SUPERINTENDENCIAID ";

                contextQuery.Parameters.Add("@SUBSECRETARIAID", Superintendencia.SubsecretariaId);
                contextQuery.Parameters.Add("@DESCRICAO", Superintendencia.Descricao);
                contextQuery.Parameters.Add("@SETOR", Superintendencia.Setor);
                contextQuery.Parameters.Add("@ATIVO", Superintendencia.Ativo);
                contextQuery.Parameters.Add("@SuperintendenciaID", Superintendencia.SuperintendenciaId);
                contextQuery.Parameters.Add("@USUARIOID", Superintendencia.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

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

        public ValidacaoDados ValidaRemocao(int superintendenciaId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            PrestacaoContas.PlanoTrabalho rnPlanoTrabalho = new PrestacaoContas.PlanoTrabalho();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (superintendenciaId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (rnPlanoTrabalho.PossuiSuperintendenciaPor(contexto, superintendenciaId))
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

        public void Remove(int SuperintendenciaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE GESTAOREDE.SUPERINTENDENCIA
                            WHERE  SUPERINTENDENCIAID = @SUPERINTENDENCIAID  ";

                contextQuery.Parameters.Add("@SuperintendenciaID", SuperintendenciaId);

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

        public DataTable ListaSuperintendencia()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT SU.*, SE.UA_ATUAL, 
									             s.DESCRICAO as SUBSECRETARIA
                                        FROM GestaoRede.Superintendencia su (NOLOCK) 
									        INNER JOIN GESTAOREDE.SUBSECRETARIA S (NOLOCK) ON SU.SUBSECRETARIAID = S.SUBSECRETARIAID
											LEFT JOIN hades..vw_setor SE ON su.SETOR = SE.SETOR  
                                        ORDER BY DESCRICAO ";

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
    }
}
