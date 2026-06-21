using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.FiscalizacaoLink
{
    public class Operadora
    {
        public DataTable Lista()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT *
                                        FROM   FISCALIZACAOLINK.OPERADORA (NOLOCK) ";

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

        public DataTable ListaOperadoraAtiva()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT *
                        FROM FISCALIZACAOLINK.OPERADORA (NOLOCK)
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

        public ValidacaoDados Valida(Entidades.Operadora operadora, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (operadora == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (operadora.OperadoraId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (operadora.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
            }
            else if (operadora.Descricao.Length > 100)
            {
                mensagens.Add("A DESCRIÇÃO deve possuir no máximo 100 caracteres.");
            }

            if (operadora.CnpjOperadora.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CNPJ DA OPERADORA é obrigatório.");
            }
            else
            {
                string cnpj = operadora.CnpjOperadora.RetirarMascaraCNPJ();
                if (!RN.Validacao.ValidaCnpj(cnpj))
                {
                    mensagens.Add("O CNPJ é inválido.");
                }
            }

            if (operadora.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já existe a Descricao cadastrada 
                    if (this.PossuiOutraDescricaoCadastradaPor(contexto, operadora.Descricao, operadora.OperadoraId))
                    {
                        mensagens.Add("Esta DESCRIÇÃO já foi utilizada!");
                    }

                    //Verifica se já existe o cpnj 
                    if (this.PossuiOutroCnpjCadastradoPor(contexto, operadora.Descricao, operadora.OperadoraId))
                    {
                        mensagens.Add("Este CPNJ já foi utilizada!");
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

        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int OperadoraId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM FISCALIZACAOLINK.OPERADORA
                                WHERE DESCRICAO = @DESCRICAO
	                                AND OPERADORAID <> @OPERADORAID ";

            contextQuery.Parameters.Add("@DESCRICAO", descricao);
            contextQuery.Parameters.Add("@OPERADORAID", OperadoraId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutroCnpjCadastradoPor(DataContext ctx, string cnpj, int OperadoraId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM FISCALIZACAOLINK.OPERADORA
                                WHERE CNPJOPERADORA = @CNPJOPERADORA
	                                AND OPERADORAID <> @OPERADORAID ";

            contextQuery.Parameters.Add("@CNPJOPERADORA", cnpj);
            contextQuery.Parameters.Add("@OPERADORAID", OperadoraId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.Operadora operadora)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO FISCALIZACAOLINK.OPERADORA 
                                                        (DESCRICAO, CNPJOPERADORA, ATIVO, USUARIOID, DATACADASTRO, DATAALTERACAO) 
                                            VALUES      (@DESCRICAO, @CNPJOPERADORA, @ATIVO, @USUARIOID, @DATACADASTRO, @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@DESCRICAO", operadora.Descricao);
                contextQuery.Parameters.Add("@CNPJOPERADORA", operadora.CnpjOperadora);
                contextQuery.Parameters.Add("@ATIVO", operadora.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", operadora.UsuarioId);
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

        public void Atualiza(Entidades.Operadora operadora)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE FISCALIZACAOLINK.OPERADORA 
                                    SET    DESCRICAO = @DESCRICAO,
                                           CNPJOPERADORA = @CNPJOPERADORA,
                                           ATIVO = @ATIVO,
                                           USUARIOID = @USUARIOID,
                                           DATAALTERACAO = @DATAALTERACAO
                                    WHERE  OPERADORAID = @OPERADORAID ";

                contextQuery.Parameters.Add("@DESCRICAO", operadora.Descricao);
                contextQuery.Parameters.Add("@CNPJOPERADORA", operadora.CnpjOperadora);
                contextQuery.Parameters.Add("@ATIVO", operadora.Ativo);
                contextQuery.Parameters.Add("@OPERADORAID", operadora.OperadoraId);
                contextQuery.Parameters.Add("@USUARIOID", operadora.UsuarioId);
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

        public ValidacaoDados ValidaRemocao(int operadoraId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.FiscalizacaoLink.ContratoOperadora rnContratoOperadora = new ContratoOperadora();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (operadoraId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (rnContratoOperadora.PossuiContratoOperadoraPor(contexto, operadoraId))
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

        public void Remove(int operadoraId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE FISCALIZACAOLINK.OPERADORA
                            WHERE  OPERADORAID = @OPERADORAID  ";

                contextQuery.Parameters.Add("@OPERADORAID", operadoraId);

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
