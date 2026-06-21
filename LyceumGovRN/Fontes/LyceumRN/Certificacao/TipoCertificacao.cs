using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Certificacao
{   
    public class TipoCertificacao 
    {
        public DataTable ListaAtivoPor()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"  SELECT '' AS TIPOCERTIFICACAOID,'Selecione' as DESCRICAO   
                                union all  SELECT  TIPOCERTIFICACAOID, 
                                        DESCRICAO
                                FROM CertificacaoEscolar.TipoCertificacao (NOLOCK)
                                     WHERE ATIVO = 1
                                ORDER BY TIPOCERTIFICACAOID ";

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

        public DataTable Lista()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT  TIPOCERTIFICACAOID, 
		                                        DESCRICAO, 
		                                        PERMITEPOLO, 
		                                        PERMITECEJA,
                                                PERMITETRANSPARENCIA,
                                                ETAPAENSINO,		                                     
		                                        ATIVO, 
		                                        USUARIOID, 
		                                        DATACADASTRO, 
		                                        DATAALTERACAO
                                        FROM CertificacaoEscolar.TipoCertificacao (NOLOCK)
                                        ORDER BY DESCRICAO ";

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

        public ValidacaoDados Valida(Entidades.TipoCertificacao tipoCertificacao, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (tipoCertificacao == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (tipoCertificacao.TipoCertificacaoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (tipoCertificacao.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
            }

            if (tipoCertificacao.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe a descrição cadastrada
                    if (this.PossuiOutraDescricaoCadastradaPor(contexto, tipoCertificacao.Descricao, tipoCertificacao.TipoCertificacaoId))
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

        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int tipoCertificacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM CertificacaoEscolar.TipoCertificacao (NOLOCK)
                                WHERE DESCRICAO = @DESCRICAO
	                                AND TIPOCERTIFICACAOID <> @TIPOCERTIFICACAOID ";

            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);
            contextQuery.Parameters.Add("@TIPOCERTIFICACAOID", SqlDbType.Int, tipoCertificacaoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.TipoCertificacao tipoCertificacao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO CERTIFICACAOESCOLAR.TIPOCERTIFICACAO
                                                        (DESCRICAO, 
                                                         PERMITEPOLO,
                                                         PERMITECEJA,
                                                         PERMITETRANSPARENCIA,
                                                         ETAPAENSINO,
                                                         ATIVO, 
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO) 
                                            VALUES      (@DESCRICAO, 
                                                         @PERMITEPOLO,
                                                         @PERMITECEJA,
                                                         @PERMITETRANSPARENCIA,
                                                         @ETAPAENSINO,
                                                         @ATIVO, 
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, tipoCertificacao.Descricao);
                contextQuery.Parameters.Add("@PERMITEPOLO", SqlDbType.Bit, tipoCertificacao.PermitePolo);
                contextQuery.Parameters.Add("@PERMITECEJA", SqlDbType.Bit, tipoCertificacao.PermiteCeja);
                contextQuery.Parameters.Add("@PERMITETRANSPARENCIA", SqlDbType.Bit, tipoCertificacao.PermiteTransparencia);
                contextQuery.Parameters.Add("@ETAPAENSINO", SqlDbType.VarChar, tipoCertificacao.EtapaEnsino);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, tipoCertificacao.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, tipoCertificacao.UsuarioId);
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

        public void Atualiza(Entidades.TipoCertificacao tipoCertificacao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE CertificacaoEscolar.TipoCertificacao
                                        SET    DESCRICAO = @DESCRICAO, 
                                               PERMITEPOLO = @PERMITEPOLO,
                                               PERMITECEJA = @PERMITECEJA,
                                               PERMITETRANSPARENCIA = @PERMITETRANSPARENCIA,
                                               ETAPAENSINO = @ETAPAENSINO,
                                               ATIVO = @ATIVO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  TipoCertificacaoID = @TipoCertificacaoID ";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, tipoCertificacao.Descricao);
                contextQuery.Parameters.Add("@PERMITEPOLO", SqlDbType.Bit, tipoCertificacao.PermitePolo);
                contextQuery.Parameters.Add("@PERMITECEJA", SqlDbType.Bit, tipoCertificacao.PermiteCeja);
                contextQuery.Parameters.Add("@PERMITETRANSPARENCIA", SqlDbType.Bit, tipoCertificacao.PermiteTransparencia);
                contextQuery.Parameters.Add("@ETAPAENSINO", SqlDbType.VarChar, tipoCertificacao.EtapaEnsino);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, tipoCertificacao.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, tipoCertificacao.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@TipoCertificacaoID", SqlDbType.Int, tipoCertificacao.TipoCertificacaoId);

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

        public ValidacaoDados ValidaRemocao(int tipoCertificacaoId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            EnccejaRequerimento rnEnccejaRequerimento = new EnccejaRequerimento();
            TipoCertificacao rnTipoCertificacao = new TipoCertificacao();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (tipoCertificacaoId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();                   

                    //Verifica se motivo ja foi utilizado em algum requerimento
                    if (rnEnccejaRequerimento.PossuiTipoCertificacaoPor(contexto, tipoCertificacaoId))
                    {
                        mensagens.Add("Este tipo não pode ser excluído, pois já foi utilizado para um requerimento.");
                    }

                    //Verifica se tipo de certificação já foi associado a alguma unidade certificadora
                    if (rnTipoCertificacao.EstaAssociadoAUnidade(contexto, tipoCertificacaoId))
                    {
                        mensagens.Add("Este tipo não pode ser excluído, pois está associado a uma unidade certificadora.");
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

        private bool EstaAssociadoAUnidade(DataContext ctx, int tipoCertificacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM CertificacaoEscolar.TipoCertificacao__UnidadeCertificadora (NOLOCK)
                                WHERE TIPOCERTIFICACAOID = @TIPOCERTIFICACAOID ";

            contextQuery.Parameters.Add("@TIPOCERTIFICACAOID", SqlDbType.Int, tipoCertificacaoId);

            return ctx.GetReturnValue<int>(contextQuery) > 0;
        }

        public void Remove(int tipoCertificacaoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE CERTIFICACAOESCOLAR.TIPOCERTIFICACAO
                            WHERE  TIPOCERTIFICACAOID = @TIPOCERTIFICACAOID  ";

                contextQuery.Parameters.Add("@TIPOCERTIFICACAOID", SqlDbType.Int, tipoCertificacaoId);

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