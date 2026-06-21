using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Transporte
{
    public class Condutor
    {
        public Entidades.Condutor ObtemPor(int condutorId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();            

            try
            { 
                return this.ObtemPor(contexto, condutorId);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
        }

        public Entidades.Condutor ObtemPor(DataContext contexto, int condutorId)
        {
            Entidades.Condutor condutor = new Entidades.Condutor();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT * 
                                          FROM  [Transporte].[CONDUTOR] (NOLOCK) 
                                          WHERE CONDUTORID = @CONDUTORID ";

            contextQuery.Parameters.Add("@CONDUTORID", SqlDbType.Int, condutorId);

            condutor = contexto.TryToBindEntity<Entidades.Condutor>(contextQuery);

            return condutor;
        }

        public DataTable Lista()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT CONDUTORID, 
                                               CPF, 
                                               NOME, 
                                               NUMEROCNH, 
                                               DATAVALIDADECNH, 
                                               CATEGORIA,
                                               ATIVO, 
                                               USUARIOID, 
                                               DATACADASTRO, 
                                               DATAALTERACAO 
                                        FROM   Transporte.CONDUTOR (NOLOCK) ";

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

        public ValidacaoDados Valida(Entidades.Condutor condutor, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (condutor == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (condutor.CondutorId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (condutor.Cpf.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CPF é obrigatório.");
            }
            else
            {
                condutor.Cpf = Utils.RetirarMascara(condutor.Cpf);

                if (!Validacao.ValidaCpf(condutor.Cpf))
                {
                    mensagens.Add("O CPF informado não é válido!");
                }
            }

            if (condutor.Nome.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME é obrigatório.");
            }
            else if (condutor.Nome.Length > 255)
            {
                mensagens.Add("Campo NOME deve conter no máximo 255 caracteres.");
            }

            if (condutor.NumeroCnh.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NÚMERO DA CNH é obrigatório.");
            }
            else if (condutor.NumeroCnh.Length > 14)
            {
                mensagens.Add("Campo NÚMERO DA CNH deve conter até 14 dígitos.");
            }

            if (condutor.DataValidadeCnh <= DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DE VALIDADE DA CNH é obrigatório.");
            }
            else if (cadastro && condutor.DataValidadeCnh.Date < DateTime.Now.Date)
            {
                mensagens.Add("Campo DATA DE VALIDADE DA CNH deve ser maior ou igual a data atual.");
            }

            if (condutor.Categoria.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CATEGORIA é obrigatório.");
            }

            if (condutor.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe o cpf cadastrado
                    if (this.PossuiOutroCPFCadastradoPor(contexto, condutor.Cpf, condutor.CondutorId))
                    {
                        mensagens.Add("Já existe um condutor com este CPF.");
                    }

                    // Verifica se já existe a cnh cadastrado
                    if (this.PossuiOutraCnhCadastradaPor(contexto, condutor.NumeroCnh, condutor.CondutorId))
                    {
                        mensagens.Add("Já existe um condutor com este NÚMERO DE CNH.");
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

        private bool PossuiOutroCPFCadastradoPor(DataContext ctx, string cpf, int condutorId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [TRANSPORTE].[CONDUTOR] (NOLOCK)
                                WHERE CPF = @CPF
	                                AND CONDUTORID <> @CONDUTORID ";

            contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, cpf);
            contextQuery.Parameters.Add("@CONDUTORID", SqlDbType.Int, condutorId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool EhAtivoPor(DataContext contexto, int condutorId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Transporte.CONDUTOR (NOLOCK)
                                    WHERE CONDUTORID = @CONDUTORID
										  AND ATIVO = 1 ";

            contextQuery.Parameters.Add("@CONDUTORID", SqlDbType.Int, condutorId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutraCnhCadastradaPor(DataContext ctx, string cnh, int condutorId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [TRANSPORTE].[CONDUTOR] (NOLOCK)
                                WHERE NUMEROCNH = @NUMEROCNH
	                                AND CONDUTORID <> @CONDUTORID ";

            contextQuery.Parameters.Add("@NUMEROCNH", SqlDbType.VarChar, cnh);
            contextQuery.Parameters.Add("@CONDUTORID", SqlDbType.Int, condutorId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.Condutor condutor)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO Transporte.CONDUTOR 
                                                (CPF, 
                                                 NOME, 
                                                 NUMEROCNH, 
                                                 DATAVALIDADECNH, 
                                                 CATEGORIA,
                                                 ATIVO, 
                                                 USUARIOID, 
                                                 DATACADASTRO, 
                                                 DATAALTERACAO) 
                                    VALUES      (@CPF, 
                                                 @NOME, 
                                                 @NUMEROCNH, 
                                                 @DATAVALIDADECNH, 
                                                 @CATEGORIA,
                                                 @ATIVO, 
                                                 @USUARIOID, 
                                                 @DATACADASTRO, 
                                                 @DATAALTERACAO) ";


                contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, condutor.Cpf);
                contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, condutor.Nome);
                contextQuery.Parameters.Add("@NUMEROCNH", SqlDbType.VarChar, condutor.NumeroCnh);
                contextQuery.Parameters.Add("@DATAVALIDADECNH", SqlDbType.DateTime, condutor.DataValidadeCnh.Date);
                contextQuery.Parameters.Add("@CATEGORIA", SqlDbType.VarChar, condutor.Categoria);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, condutor.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, condutor.UsuarioId);
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

        public void Atualiza(Entidades.Condutor condutor)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE Transporte.CONDUTOR 
                                        SET    CPF = @CPF, 
                                               NOME = @NOME, 
                                               NUMEROCNH = @NUMEROCNH, 
                                               DATAVALIDADECNH = @DATAVALIDADECNH, 
                                               CATEGORIA = @CATEGORIA,
                                               ATIVO = @ATIVO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  CONDUTORID = @CONDUTORID  ";

                contextQuery.Parameters.Add("@CONDUTORID", SqlDbType.Int, condutor.CondutorId);
                contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, condutor.Cpf);
                contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, condutor.Nome);
                contextQuery.Parameters.Add("@NUMEROCNH", SqlDbType.VarChar, condutor.NumeroCnh);
                contextQuery.Parameters.Add("@CATEGORIA", SqlDbType.VarChar, condutor.Categoria);
                contextQuery.Parameters.Add("@DATAVALIDADECNH", SqlDbType.DateTime, condutor.DataValidadeCnh.Date);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, condutor.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, condutor.UsuarioId);
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

        public ValidacaoDados ValidaRemocao(int condutorId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.Transporte.Prestador rnPrestador = new Prestador();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (condutorId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se ja foi utilizado nas tabelas PRESTADORCONDUTORVEICULO e PRESTADORCONDUTOR
                    if (this.PossuiPrestadorCondutorPorCondutor(contexto, condutorId) || this.PossuiPrestadorCondutorVeiculoPorCondutor(contexto, condutorId))
                    {
                        mensagens.Add("Registro não pode ser excluído pois já foi associado a um prestador e/ou veículo.");
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

        private bool PossuiPrestadorCondutorPorCondutor(DataContext ctx, int condutorId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [Transporte].[PRESTADORCONDUTOR] (NOLOCK)
                                WHERE CONDUTORID = @CONDUTORID ";

            contextQuery.Parameters.Add("@CONDUTORID", SqlDbType.Int, condutorId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiPrestadorCondutorVeiculoPorCondutor(DataContext ctx, int condutorId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [Transporte].[PRESTADORCONDUTORVEICULO] (NOLOCK)
                                WHERE CONDUTORID = @CONDUTORID ";

            contextQuery.Parameters.Add("@CONDUTORID", SqlDbType.Int, condutorId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Remove(int condutorId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE Transporte.CONDUTOR
                            WHERE  CONDUTORID = @CONDUTORID  ";

                contextQuery.Parameters.Add("@CONDUTORID", SqlDbType.Int, condutorId);

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
