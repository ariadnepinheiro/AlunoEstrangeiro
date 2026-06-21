using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.GestaoRede
{
    public class Subsecretaria
    {
        public DataTable ListaAtivo()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT SUBSECRETARIAID, DESCRICAO
                        FROM GESTAOREDE.SUBSECRETARIA (NOLOCK)
                        WHERE ATIVO = 1
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

        public ValidacaoDados Valida(Entidades.Subsecretaria subsecretaria, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            Superintendencia rnSuperintendencia = new Superintendencia();                
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (subsecretaria == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (subsecretaria.SubsecretariaId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (subsecretaria.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
            }
            else if (subsecretaria.Descricao.Length > 100)
            {
                mensagens.Add("A DESCRIÇÃO deve possuir no máximo 100 caracteres.");
            }

            if (subsecretaria.Setor.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE ADMINISTRATIVA é obrigatório.");
            }

            if (subsecretaria.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe a Descricao cadastrada 
                    if (this.PossuiOutraDescricaoCadastradaPor(contexto, subsecretaria.Descricao, subsecretaria.SubsecretariaId))
                    {
                        mensagens.Add("Esta DESCRIÇÃO já foi utilizada!");
                    }

                    //Verifica se já existe a ua cadastrada 
                    if (this.PossuiOutroSetorCadastradoPor(contexto, subsecretaria.Setor, subsecretaria.SubsecretariaId))
                    {
                        mensagens.Add("Esta UNIDADE ADMINISTRATIVA já foi utilizada!");
                    }

                    //Verifica se é alteração e se a secretaria não esta ativa
                    if (cadastro && !subsecretaria.Ativo)
                    { 
                        //Verifica se possui superintendencia ativa
                        if(rnSuperintendencia.PossuiSuperintendenciaAtivaPor(contexto, subsecretaria.SubsecretariaId))
                        {
                            mensagens.Add("Esta Subsecretaria não pode ser desativada pois possui Superintendencias ativas!");
                        }
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

        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int subsecretariaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM GESTAOREDE.SUBSECRETARIA
                                WHERE DESCRICAO = @DESCRICAO
	                                AND SUBSECRETARIAID <> @SUBSECRETARIAID ";

            contextQuery.Parameters.Add("@DESCRICAO", descricao);
            contextQuery.Parameters.Add("@SubsecretariaID", subsecretariaId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutroSetorCadastradoPor(DataContext ctx, string setor, int subsecretariaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM GESTAOREDE.SUBSECRETARIA
                                WHERE SETOR = @SETOR
	                                AND SUBSECRETARIAID <> @SUBSECRETARIAID ";

            contextQuery.Parameters.Add("@SETOR", setor);
            contextQuery.Parameters.Add("@SUBSECRETARIAID", subsecretariaId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.Subsecretaria Subsecretaria)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO gestaorede.SUBSECRETARIA
                                                        (DESCRICAO,
                                                         SETOR,
                                                         ATIVO,
                                                         USUARIOID,
                                                         DATACADASTRO,
                                                         DATAALTERACAO)
                                            VALUES      (@DESCRICAO,
                                                         @SETOR,
                                                         @ATIVO,
                                                         @USUARIOID,
                                                         @DATACADASTRO,
                                                         @DATAALTERACAO)  ";

                contextQuery.Parameters.Add("@DESCRICAO", Subsecretaria.Descricao);
                contextQuery.Parameters.Add("@SETOR", Subsecretaria.Setor);
                contextQuery.Parameters.Add("@ATIVO", Subsecretaria.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", Subsecretaria.UsuarioId);
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

        public void Atualiza(Entidades.Subsecretaria Subsecretaria)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE GESTAOREDE.SUBSECRETARIA 
                                    SET    DESCRICAO = @DESCRICAO,
                                           SETOR = @SETOR,
                                           ATIVO = @ATIVO,
                                           USUARIOID = @USUARIOID,
                                           DATAALTERACAO = @DATAALTERACAO
                                    WHERE  SUBSECRETARIAID = @SUBSECRETARIAID ";

                contextQuery.Parameters.Add("@DESCRICAO", Subsecretaria.Descricao);
                contextQuery.Parameters.Add("@SETOR", Subsecretaria.Setor);
                contextQuery.Parameters.Add("@ATIVO", Subsecretaria.Ativo);
                contextQuery.Parameters.Add("@SubsecretariaID", Subsecretaria.SubsecretariaId);
                contextQuery.Parameters.Add("@USUARIOID", Subsecretaria.UsuarioId);
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

        public ValidacaoDados ValidaRemocao(int subsecretariaId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            Superintendencia rnSuperintendencia = new Superintendencia();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (subsecretariaId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (rnSuperintendencia.PossuiSubsecretariaPor(contexto, subsecretariaId))
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

        public void Remove(int SubsecretariaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE GESTAOREDE.SUBSECRETARIA
                            WHERE  SUBSECRETARIAID = @SUBSECRETARIAID  ";

                contextQuery.Parameters.Add("@SubsecretariaID", SubsecretariaId);

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

        public DataTable ListaSubsecretaria()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT S.*, SE.UA_ATUAL
                                        FROM GestaoRede.Subsecretaria S (NOLOCK)    
                                            LEFT JOIN hades..vw_setor SE ON S.SETOR = SE.SETOR
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
