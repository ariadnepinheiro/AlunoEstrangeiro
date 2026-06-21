using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.RecursosHumanos
{
    public class JustificativaFalta
    {
        public DataTable ListaAtivoPor()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"  SELECT  JUSTIFICATIVAFALTAID, 
                                                    DESCRICAO
                                            FROM RecursosHumanos.JUSTIFICATIVAFALTA (NOLOCK)
                                                 WHERE ATIVO = 1
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

        public DataTable Lista()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"SELECT JUSTIFICATIVAFALTAID, 
                                                DESCRICAO,  
                                                LEIAMPARO,  
                                                CASOESPECIFICO,  
                                                ATIVO,  
                                                USUARIOID,  
                                                DATACADASTRO,  
                                                DATAALTERACAO
                                        FROM RecursosHumanos.JUSTIFICATIVAFALTA (NOLOCK)
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

        public ValidacaoDados Valida(Entidades.JustificativaFalta justificativaFalta, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.AlunoLicenca rnAlunoLicenca = new AlunoLicenca();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (justificativaFalta == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (justificativaFalta.JustificativaFaltaId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (justificativaFalta.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
            }
            else
            {
                if (justificativaFalta.Descricao.Length > 500)
                {
                    mensagens.Add("Campo DESCRIÇÃO deve conter no máximo 500 caracteres!");
                }
            }

            if (justificativaFalta.LeiAmparo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo LEI AMPARO é obrigatório.");
            }
            else
            {
                if (justificativaFalta.LeiAmparo.Length > 100)
                {
                    mensagens.Add("Campo LEI AMPARO deve conter no máximo 100 caracteres!");
                }
            }

            if (justificativaFalta.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já existe a descrição cadastrada
                    if (this.PossuiOutraDescricaoCadastradaPor(contexto, justificativaFalta.Descricao, justificativaFalta.JustificativaFaltaId))
                    {
                        mensagens.Add("Esta DESCRIÇÃO já foi utilizada.");
                    }

                    //O sistema deverá permitir que o Tipo Motivo Ausência informado seja editado na base de dados conforme os perfis autorizados SOMENTE se ele não estiver sendo usado.
                    if (!cadastro)
                    {
                        //Verifica se foi utilizado
                        if (rnAlunoLicenca.PossuiJustificativaFaltaPor(contexto, justificativaFalta.JustificativaFaltaId))
                        {
                            mensagens.Add("Motivo em uso. Não pode ser alterado.");
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

        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int justificativaFaltaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM RecursosHumanos.JUSTIFICATIVAFALTA (NOLOCK)
                                WHERE DESCRICAO = @DESCRICAO
	                                AND JUSTIFICATIVAFALTAID <> @JUSTIFICATIVAFALTAID ";

            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);
            contextQuery.Parameters.Add("@JUSTIFICATIVAFALTAID", SqlDbType.Int, justificativaFaltaId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.JustificativaFalta justificativaFalta)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO RecursosHumanos.JUSTIFICATIVAFALTA
                                                        (DESCRICAO,
                                                         LEIAMPARO,
                                                         CASOESPECIFICO, 
                                                         ATIVO, 
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO) 
                                            VALUES      (@DESCRICAO, 
                                                         @LEIAMPARO,
                                                         @CASOESPECIFICO, 
                                                         @ATIVO, 
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, justificativaFalta.Descricao);
                contextQuery.Parameters.Add("@LEIAMPARO", SqlDbType.VarChar, justificativaFalta.LeiAmparo);
                contextQuery.Parameters.Add("@CASOESPECIFICO", SqlDbType.Bit, justificativaFalta.CasoEspecifico);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, justificativaFalta.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, justificativaFalta.UsuarioId);
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

        public void Atualiza(Entidades.JustificativaFalta justificativaFalta)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE RecursosHumanos.JUSTIFICATIVAFALTA
                                        SET    DESCRICAO = @DESCRICAO, 
                                               LEIAMPARO = @LEIAMPARO,
                                               CASOESPECIFICO = @CASOESPECIFICO,
                                               ATIVO = @ATIVO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  JUSTIFICATIVAFALTAID = @JUSTIFICATIVAFALTAID ";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, justificativaFalta.Descricao);
                contextQuery.Parameters.Add("@LEIAMPARO", SqlDbType.VarChar, justificativaFalta.LeiAmparo);
                contextQuery.Parameters.Add("@CASOESPECIFICO", SqlDbType.Bit, justificativaFalta.CasoEspecifico);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, justificativaFalta.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, justificativaFalta.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@JUSTIFICATIVAFALTAID", SqlDbType.Int, justificativaFalta.JustificativaFaltaId);

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

        public ValidacaoDados ValidaRemocao(int justificativaFaltaId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.AlunoLicenca rnAlunoLicenca = new AlunoLicenca();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (justificativaFaltaId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se foi utilizado
                    if (rnAlunoLicenca.PossuiJustificativaFaltaPor(contexto, justificativaFaltaId))
                    {
                        mensagens.Add("Motivo em uso. Não pode ser excluído.");
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

        public void Remove(int justificativaFaltaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE RecursosHumanos.JUSTIFICATIVAFALTA
                            WHERE  JUSTIFICATIVAFALTAID = @JUSTIFICATIVAFALTAID  ";

                contextQuery.Parameters.Add("@JUSTIFICATIVAFALTAID", SqlDbType.Int, justificativaFaltaId);

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
