using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Certificacao
{
    public class UsuarioUnidadeCertificadora
    {
        public bool PossuiUnidadeCertificadoraPor(DataContext contexto, int unidadeCertificadoraId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM CertificacaoEscolar.USUARIOUNIDADECERTIFICADORA (NOLOCK)
                                    WHERE UNIDADECERTIFICADORAID = @UNIDADECERTIFICADORAID ";

            contextQuery.Parameters.Add("@UNIDADECERTIFICADORAID", SqlDbType.Int, unidadeCertificadoraId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable Lista()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT U.USUARIOUNIDADECERTIFICADORAID, 
		                                        U.UNIDADECERTIFICADORAID, 	
												UC.DESCRICAO AS UNIDADECERTIFICADORA,                                     
		                                        U.USUARIO,
												HU.NOME
										FROM CERTIFICACAOESCOLAR.USUARIOUNIDADECERTIFICADORA U (NOLOCK)
											 INNER JOIN CERTIFICACAOESCOLAR.UNIDADECERTIFICADORA UC (NOLOCK) 
														ON UC.UNIDADECERTIFICADORAID = U.UNIDADECERTIFICADORAID
											 INNER JOIN HADES.DBO.HD_USUARIO HU ON U.USUARIO = HU.USUARIO  ";

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

        public ValidacaoDados Valida(Entidades.UsuarioUnidadeCertificadora usuarioUnidadeCertificadora)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (usuarioUnidadeCertificadora == null)
            {
                return validacaoDados;
            }

            if (usuarioUnidadeCertificadora.UnidadeCertificadoraId <= 0)
            {
                mensagens.Add("Campo UNIDADE CERTIFICADORA é obrigatório.");
            }

            if (usuarioUnidadeCertificadora.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO é obrigatório.");
            }

            if (usuarioUnidadeCertificadora.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSÁVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe a unidade para o usuario
                    if (this.PossuiOutraCadastradaPor(contexto, usuarioUnidadeCertificadora.Usuario, usuarioUnidadeCertificadora.UnidadeCertificadoraId))
                    {
                        mensagens.Add("Esta UNIDADE CERTIFICADORA já foi adicionada para este USUÁRIO.");
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

        private bool PossuiOutraCadastradaPor(DataContext ctx, string usuario, int unidadeCertificadoraId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM CertificacaoEscolar.USUARIOUNIDADECERTIFICADORA (NOLOCK)
                                WHERE UNIDADECERTIFICADORAID = @UNIDADECERTIFICADORAID
	                                AND USUARIO = @USUARIO ";

            contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuario);
            contextQuery.Parameters.Add("@UNIDADECERTIFICADORAID", SqlDbType.Int, unidadeCertificadoraId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.UsuarioUnidadeCertificadora usuarioUnidadeCertificadora)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO CertificacaoEscolar.USUARIOUNIDADECERTIFICADORA
                                                        (UNIDADECERTIFICADORAID, 
                                                         USUARIO, 
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO) 
                                            VALUES      (@UNIDADECERTIFICADORAID, 
                                                         @USUARIO, 
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@UNIDADECERTIFICADORAID", SqlDbType.Int, usuarioUnidadeCertificadora.UnidadeCertificadoraId);
                contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuarioUnidadeCertificadora.Usuario);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioUnidadeCertificadora.UsuarioId);
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

        public ValidacaoDados ValidaRemocao(int usuarioUnidadeCertificadoraId)
        {
            List<string> mensagens = new List<string>();
            UnidadeCertificadora rnUnidadeCertificadora = new UnidadeCertificadora();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (usuarioUnidadeCertificadoraId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
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

        public void Remove(int usuarioUnidadeCertificadoraId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE CertificacaoEscolar.USUARIOUNIDADECERTIFICADORA
                            WHERE  USUARIOUNIDADECERTIFICADORAID = @USUARIOUNIDADECERTIFICADORAID  ";

                contextQuery.Parameters.Add("@USUARIOUNIDADECERTIFICADORAID", SqlDbType.Int, usuarioUnidadeCertificadoraId);

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
