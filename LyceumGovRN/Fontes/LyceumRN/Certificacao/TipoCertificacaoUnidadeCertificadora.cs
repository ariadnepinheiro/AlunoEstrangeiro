using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Certificacao
{   
    public class TipoCertificacaoUnidadeCertificadora
    {
        public DataTable Lista()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" 
select 
tu.TIPOCERTIFICACAOID
,tc.DESCRICAO
,tu.UNIDADECERTIFICADORAID
,uc.DESCRICAO
,tu.USUARIOID
,tu.DATACADASTRO
,tu.DATAALTERACAO

from 
CertificacaoEscolar.TIPOCERTIFICACAO__UNIDADECERTIFICADORA tu (nolock)
inner join CertificacaoEscolar.TIPOCERTIFICACAO tc (nolock) on tc.TIPOCERTIFICACAOID = tu.TIPOCERTIFICACAOID
inner join CertificacaoEscolar.UNIDADECERTIFICADORA uc (nolock) on uc.UNIDADECERTIFICADORAID = tu.UNIDADECERTIFICADORAID

order by
tu.DATACADASTRO desc
                ";

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

        public ValidacaoDados Valida(Entidades.TipoCertificacaoUnidadeCertificadora tcuc)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (tcuc == null)
            {
                return validacaoDados;
            }

            if (tcuc.TipoCertificacaoId <= 0)
            {
                mensagens.Add("Campo TIPO CERTIFICAÇÃO é obrigatório.");
            }

            if (tcuc.UnidadeCertificadoraId <= 0)
            {
                mensagens.Add("Campo UNIDADE CERTIFICADORA é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe o relacionamento informado
                    if (this.JaExiste(contexto, tcuc.TipoCertificacaoId, tcuc.UnidadeCertificadoraId))
                    {
                        mensagens.Add("Este TIPO CERTIFICAÇÃO já existe associado a esta UNIDADE CERTIFICADORA.");
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

        private bool JaExiste(DataContext ctx, int tipoCertificacaoId, int unidadeCertificadoraId)
        {
            ContextQuery contextQuery = new ContextQuery();
            
            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM CertificacaoEscolar.TIPOCERTIFICACAO__UNIDADECERTIFICADORA (NOLOCK)
                                WHERE TIPOCERTIFICACAOID = @TIPOCERTIFICACAOID
                                AND UNIDADECERTIFICADORAID = @UNIDADECERTIFICADORAID ";

            contextQuery.Parameters.Add("@TIPOCERTIFICACAOID", SqlDbType.Int, tipoCertificacaoId);
            contextQuery.Parameters.Add("@UNIDADECERTIFICADORAID", SqlDbType.Int, unidadeCertificadoraId);

            return ctx.GetReturnValue<int>(contextQuery) > 0;
        }

        public void Insere(Entidades.TipoCertificacaoUnidadeCertificadora tcuc)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO CERTIFICACAOESCOLAR.TIPOCERTIFICACAO__UNIDADECERTIFICADORA
                                                        (TIPOCERTIFICACAOID,
                                                         UNIDADECERTIFICADORAID, 
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO) 
                                            VALUES      (@TIPOCERTIFICACAOID,
                                                         @UNIDADECERTIFICADORAID, 
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@TIPOCERTIFICACAOID", SqlDbType.Int, tcuc.TipoCertificacaoId);
                contextQuery.Parameters.Add("@UNIDADECERTIFICADORAID", SqlDbType.Int, tcuc.UnidadeCertificadoraId);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, tcuc.UsuarioId);
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

        public ValidacaoDados ValidaRemocao(int tipoCertificacaoId, int unidadeCertificadoraId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            EnccejaRequerimento rnEnccejaRequerimento = new EnccejaRequerimento();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (tipoCertificacaoId <= 0)
            {
                mensagens.Add("Campo TIPOCERTIFICACAOID é obrigatório.");
            }

            if (unidadeCertificadoraId <= 0)
                mensagens.Add("Campo UNIDADECERTIFICADORAID é obrigatório.");

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();                   

                    //caso seja necessário vallidar alguma coisa no banco de dados, o código vai aqui...
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

        public void Remove(int tipoCertificacaoId, int unidadeCertificadoraId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE CERTIFICACAOESCOLAR.TIPOCERTIFICACAO__UNIDADECERTIFICADORA
                            WHERE  TIPOCERTIFICACAOID = @TIPOCERTIFICACAOID and UNIDADECERTIFICADORAID = @UNIDADECERTIFICADORAID ";

                contextQuery.Parameters.Add("@TIPOCERTIFICACAOID", SqlDbType.Int, tipoCertificacaoId);
                contextQuery.Parameters.Add("@UNIDADECERTIFICADORAID", SqlDbType.Int, unidadeCertificadoraId);

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