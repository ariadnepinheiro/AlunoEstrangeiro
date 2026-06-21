using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Matriculas
{
    public class MotivoRejeicaoInscricao : RNBase
    {
        public enum Tipo
        {
            [StringValue("Motivo para Confirmação")]
            Confirmacao = 1,
            [StringValue("Motivo para Contato")]
            Contato = 2,
            [StringValue("Motivo Interno do sistema")]
            Interno = 3,
        }

        public DataTable ListaMotivoAtivoParaContato()
        {
            DataTable retorno = null;
            retorno = this.ListaAtivoPor((int)MotivoRejeicaoInscricao.Tipo.Contato);
            return retorno;
        }

        public DataTable ListaMotivoAtivoParaConfirmacao()
        {
            DataTable retorno = null;
            retorno = this.ListaAtivoPor((int)MotivoRejeicaoInscricao.Tipo.Confirmacao);
            return retorno;
        }

        public DataTable ListaMotivoParaContato()
        {
            DataTable retorno = null;
            retorno = this.ListaPor((int)MotivoRejeicaoInscricao.Tipo.Contato);
            return retorno;
        }

        public DataTable ListaMotivoParaConfirmacao()
        {
            DataTable retorno = null;
            retorno = this.ListaPor((int)MotivoRejeicaoInscricao.Tipo.Confirmacao);
            return retorno;
        }

        private DataTable ListaAtivoPor(int tipo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT  MOTIVOREJEICAOINSCRICAOID, 
		                                        DESCRICAO, 
		                                        TIPO, 
		                                        ATIVO, 
		                                        USUARIOID, 
		                                        DATACADASTRO, 
		                                        DATAALTERACAO
                                        FROM MATRICULA.MOTIVOREJEICAOINSCRICAO (NOLOCK)
                                        WHERE TIPO = @TIPO
                                              AND ATIVO = 1
                                        ORDER BY DESCRICAO ";

                contextQuery.Parameters.Add("@TIPO", SqlDbType.Int, tipo);

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

        private DataTable ListaPor(int tipo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT  MOTIVOREJEICAOINSCRICAOID, 
		                                        DESCRICAO, 
		                                        TIPO, 
		                                        ATIVO, 
		                                        USUARIOID, 
		                                        DATACADASTRO, 
		                                        DATAALTERACAO
                                        FROM MATRICULA.MOTIVOREJEICAOINSCRICAO (NOLOCK)
                                        WHERE TIPO = @TIPO
                                        ORDER BY DESCRICAO ";

                contextQuery.Parameters.Add("@TIPO", SqlDbType.Int, tipo);

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

        public ValidacaoDados Valida(Entidades.MotivoRejeicaoInscricao motivoRejeicaoInscricao, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (motivoRejeicaoInscricao == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (motivoRejeicaoInscricao.MotivoRejeicaoInscricaoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
                else if (motivoRejeicaoInscricao.Tipo == 3)
                {
                    mensagens.Add("Este motivo não pode ser alterado pois é usado internamente pelo sistema.");
                }
            }

            if (motivoRejeicaoInscricao.Tipo <= 0)
            {
                mensagens.Add("Campo TIPO é obrigatório.");
            }

            if (motivoRejeicaoInscricao.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
            }

            if (motivoRejeicaoInscricao.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe a descrição cadastrada
                    if (this.PossuiOutraDescricaoCadastradaPor(contexto, motivoRejeicaoInscricao.Descricao, motivoRejeicaoInscricao.Tipo, motivoRejeicaoInscricao.MotivoRejeicaoInscricaoId))
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

        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int tipo, int motivoRejeicaoInscricaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [Matricula].[MOTIVOREJEICAOINSCRICAO] (NOLOCK)
                                WHERE DESCRICAO = @DESCRICAO
                                    AND TIPO = @TIPO
	                                AND MOTIVOREJEICAOINSCRICAOID <> @MOTIVOREJEICAOINSCRICAOID ";

            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);
            contextQuery.Parameters.Add("@TIPO", SqlDbType.Int, tipo);
            contextQuery.Parameters.Add("@MOTIVOREJEICAOINSCRICAOID", SqlDbType.Int, motivoRejeicaoInscricaoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.MotivoRejeicaoInscricao motivoRejeicaoInscricao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO Matricula.MOTIVOREJEICAOINSCRICAO
                                                        (DESCRICAO, 
                                                         ATIVO, 
                                                         TIPO, 
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO) 
                                            VALUES      (@DESCRICAO, 
                                                         @ATIVO, 
                                                         @TIPO, 
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, motivoRejeicaoInscricao.Descricao);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, motivoRejeicaoInscricao.Ativo);
                contextQuery.Parameters.Add("@TIPO", SqlDbType.Int, motivoRejeicaoInscricao.Tipo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, motivoRejeicaoInscricao.UsuarioId);
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

        public void Atualiza(Entidades.MotivoRejeicaoInscricao motivoRejeicaoInscricao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE Matricula.MOTIVOREJEICAOINSCRICAO
                                        SET    DESCRICAO = @DESCRICAO, 
                                               ATIVO = @ATIVO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  MOTIVOREJEICAOINSCRICAOID = @MOTIVOREJEICAOINSCRICAOID ";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, motivoRejeicaoInscricao.Descricao);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, motivoRejeicaoInscricao.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, motivoRejeicaoInscricao.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@MOTIVOREJEICAOINSCRICAOID", SqlDbType.Int, motivoRejeicaoInscricao.MotivoRejeicaoInscricaoId);

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

        public ValidacaoDados ValidaRemocao(int motivoRejeicaoInscricaoId, int tipo)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.Matriculas.ContatoOpcaoInscricao rnContatoOpcaoInscricao = new ContatoOpcaoInscricao();
            RN.Matriculas.ContatoOpcaoInscricaoHist rnContatoOpcaoInscricaoHist = new ContatoOpcaoInscricaoHist();
            RN.Matriculas.OpcaoInscricaoHist rnOpcaoInscricaoHist = new OpcaoInscricaoHist();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (motivoRejeicaoInscricaoId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (tipo <= 0)
            {
                mensagens.Add("Campo TIPO é obrigatório.");
            }
            else if (tipo == 3)
            {
                mensagens.Add("Este motivo não pode ser alterado pois é usado internamente pelo sistema.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se motivo ja foi utilizado em algum contato
                    if (rnContatoOpcaoInscricao.PossuiMotivoPor(contexto, motivoRejeicaoInscricaoId))
                    {
                        mensagens.Add("Este motivo não pode ser excluído, pois já foi utilizado para um contato.");
                    }

                    //Verifica se motivo ja foi utilizado em algum contato
                    if (rnContatoOpcaoInscricaoHist.PossuiMotivoPor(contexto, motivoRejeicaoInscricaoId))
                    {
                        mensagens.Add("Este motivo não pode ser excluído, pois já foi utilizado para um contato.");
                    }

                    //Verifica se motivo ja foi utilizado em alguma opção historico
                    if (rnOpcaoInscricaoHist.PossuiMotivoPor(contexto, motivoRejeicaoInscricaoId))
                    {
                        mensagens.Add("Este motivo não pode ser excluído, pois já foi utilizado para uma confirmação.");
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

        public void Remove(int motivoRejeicaoInscricaoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE Matricula.MOTIVOREJEICAOINSCRICAO
                            WHERE  MOTIVOREJEICAOINSCRICAOID = @MOTIVOREJEICAOINSCRICAOID  ";

                contextQuery.Parameters.Add("@MOTIVOREJEICAOINSCRICAOID", SqlDbType.Int, motivoRejeicaoInscricaoId);

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
