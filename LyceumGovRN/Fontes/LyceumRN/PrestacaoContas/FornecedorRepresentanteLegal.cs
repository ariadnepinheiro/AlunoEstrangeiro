using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class FornecedorRepresentanteLegal
    {
        public bool TemExatamenteUmRepresentantesLegaisVigentes(DataContext ctx, int fornecedorId)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" 
                    select COUNT(0)
                    from PrestacaoContas.FORNECEDORREPRESENTANTELEGAL frl
                    where frl.FORNECEDORID = @FORNECEDORID
                    and frl.DATAFIM is null
                    ";

                contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorId);

                return ctx.GetReturnValue<int>(contextQuery) == 1;
            }
            catch (Exception ex)
            {
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
        }

        public DataTable ListaPor(int fornecedorId)
        {
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT FORNECEDORREPRESENTANTELEGALID, 
			                            FORNECEDORID, 
			                            NOME, 
			                            CPF, 
			                            DATAINICIO, 
			                            DATAFIM, 
			                            USUARIOID, 
			                            DATACADASTRO, 
			                            DATAALTERACAO
                            FROM [PrestacaoContas].[FORNECEDORREPRESENTANTELEGAL] (NOLOCK)
                            WHERE FORNECEDORID = @FORNECEDORID
                            ORDER BY DATAINICIO DESC ";

                contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorId);

                return contexto.GetDataTable(contextQuery);
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
        }

        public bool PossuiRepresentanteLegalAtivoPor(DataContext ctx, int fornecedorId)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();
                
                contextQuery.Command = @" SELECT  COUNT(*)
                                FROM    PrestacaoContas.FORNECEDORREPRESENTANTELEGAL (NOLOCK)
                                WHERE   FORNECEDORID = @FORNECEDORID
										AND CONVERT(DATE, DATAINICIO) <= CONVERT(DATE, GETDATE())
                                        AND (DATAFIM IS NULL
                                            OR CONVERT(DATE, DATAFIM) > CONVERT(DATE, GETDATE())) ";

                contextQuery.Parameters.Add("@FORNECEDORID", fornecedorId);

                return ctx.GetReturnValue<int>(contextQuery) > 0;
            }
            catch (Exception ex)
            {
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
        }

        private bool EhAtivoPor(DataContext ctx, int fornecedorRepresentanteLegalId)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT  COUNT(*)
                                FROM    PrestacaoContas.FORNECEDORREPRESENTANTELEGAL (NOLOCK)
                                WHERE   FORNECEDORREPRESENTANTELEGALID = @FORNECEDORREPRESENTANTELEGALID
                                        AND (DATAFIM IS NULL
                                            OR CONVERT(DATE, DATAFIM) > CONVERT(DATE, GETDATE())) ";

                contextQuery.Parameters.Add("@FORNECEDORREPRESENTANTELEGALID", fornecedorRepresentanteLegalId);

                return ctx.GetReturnValue<int>(contextQuery) > 0;
            }
            catch (Exception ex)
            {
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
        }

        private bool PossuiDataInicioEmOutroIntervaloPor(DataContext ctx, int fornecedorId, DateTime data, int fornecedorRepresentanteLegalId)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT COUNT(*) 
                                FROM  [PrestacaoContas].[FORNECEDORREPRESENTANTELEGAL]  (NOLOCK)
                                WHERE  FORNECEDORID = @FORNECEDORID 
	                                AND FORNECEDORREPRESENTANTELEGALID <> @FORNECEDORREPRESENTANTELEGALID
	                                AND @DATA BETWEEN DATAINICIO AND 
			                                CONVERT(DATE, CONVERT(DATETIME, ISNULL(DATAFIM, GETDATE())) ) ";

                contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorId);
                contextQuery.Parameters.Add("@FORNECEDORREPRESENTANTELEGALID", SqlDbType.Int, fornecedorRepresentanteLegalId);
                contextQuery.Parameters.Add("@DATA", SqlDbType.Date, data.Date);

                return ctx.GetReturnValue<int>(contextQuery) > 0;
            }
            catch (Exception ex)
            {
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
        }

        private bool PossuiOutraIntercaladaPor(DataContext ctx, int fornecedorId, DateTime dataInicio, DateTime dataFim, int fornecedorRepresentanteLegalId)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT COUNT(*) 
                            FROM   [PrestacaoContas].[FORNECEDORREPRESENTANTELEGAL]  (NOLOCK)
                            WHERE FORNECEDORID = @FORNECEDORID 
                                    AND FORNECEDORREPRESENTANTELEGALID <> @FORNECEDORREPRESENTANTELEGALID
                                    AND @DATAINICIO <= CONVERT(DATE, DATAINICIO) 
                                    AND @DATAFIM >= CONVERT(DATE, CONVERT(DATETIME, ISNULL(DATAFIM, GETDATE()))) ";

                contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorId);
                contextQuery.Parameters.Add("@FORNECEDORREPRESENTANTELEGALID", SqlDbType.Int, fornecedorRepresentanteLegalId);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, dataInicio.Date);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, dataFim.Date);

                return ctx.GetReturnValue<int>(contextQuery) > 0;
            }
            catch (Exception ex)
            {
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
        }

        private bool PossuiDataFimEmOutroIntervaloPor(DataContext ctx, int fornecedorId, DateTime data, int fornecedorRepresentanteLegalId)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT COUNT(*) 
                        FROM   [PrestacaoContas].[FORNECEDORREPRESENTANTELEGAL] 
                        WHERE  FORNECEDORID = @FORNECEDORID 
                                AND FORNECEDORREPRESENTANTELEGALID <> @FORNECEDORREPRESENTANTELEGALID
                                AND @DATA BETWEEN 
                                    CONVERT(DATE, CONVERT(DATETIME, DATAINICIO) + 1) AND CONVERT( 
                                    DATE, CONVERT(DATETIME, ISNULL(DATAFIM, GETDATE())))  ";

                contextQuery.Parameters.Add("@FORNECEDORID", fornecedorId);
                contextQuery.Parameters.Add("@FORNECEDORREPRESENTANTELEGALID", fornecedorRepresentanteLegalId);
                contextQuery.Parameters.Add("@DATA", data.Date);

                return ctx.GetReturnValue<int>(contextQuery) > 0;
            }
            catch (Exception ex)
            {
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
        }

        public ValidacaoDados Valida(Entidades.FornecedorRepresentanteLegal fornecedorRepresentanteLegal, bool cadastro)
        {
            var validacaoDados = new ValidacaoDados();
            var mensagens = new List<string>();

            if (fornecedorRepresentanteLegal == null)
                return validacaoDados;

            //Verifica se é alteração
            if (!cadastro && fornecedorRepresentanteLegal.FornecedorRepresentanteLegalId <= 0)
                mensagens.Add("Campo CÓDIGO é obrigatório.");

            if (fornecedorRepresentanteLegal.FornecedorId <= 0)
                mensagens.Add("Campo FORNECEDOR é obrigatório.");

            if (fornecedorRepresentanteLegal.Nome.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME é obrigatório.");
            }
            else
            {
                if (!Validacao.SomenteLetras(fornecedorRepresentanteLegal.Nome))
                {
                    mensagens.Add("Campo NOME não pode conter números.");
                }
            }

            if (fornecedorRepresentanteLegal.Cpf.IsNullOrEmptyOrWhiteSpace())
                mensagens.Add("Campo CPF é obrigatório.");

            else
                if (!Validacao.ValidaCpf(fornecedorRepresentanteLegal.Cpf))
                    mensagens.Add("O CPF informado não é válido!");

            if (fornecedorRepresentanteLegal.DataInicio <= DateTime.MinValue)
                mensagens.Add("Campo DATA INÍCIO é obrigatório.");

            else
                if (fornecedorRepresentanteLegal.DataFim != null && fornecedorRepresentanteLegal.DataFim > DateTime.MinValue)
                    if (fornecedorRepresentanteLegal.DataFim < fornecedorRepresentanteLegal.DataInicio)
                        mensagens.Add("Campo DATA INÍCIO deve ser menor ou igual a DATA FIM.");

            if (fornecedorRepresentanteLegal.UsuarioId.IsNullOrEmptyOrWhiteSpace())
                mensagens.Add("Campo USUÁRIO é obrigatório.");

            if (mensagens.Count == 0)
            {
                DataContext contexto = null;

                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (!cadastro)
                        if (!this.EhAtivoPor(contexto, fornecedorRepresentanteLegal.FornecedorRepresentanteLegalId.Value))
                            mensagens.Add("Apenas representante legal vigente pode ser editado.");

                    if (this.PossuiRepresentanteLegalPor(contexto, fornecedorRepresentanteLegal.Cpf,fornecedorRepresentanteLegal.FornecedorRepresentanteLegalId != null ? fornecedorRepresentanteLegal.FornecedorRepresentanteLegalId.Value : -1))
                    {
                        mensagens.Add("Já existe representante legal cadastrado com este CPF.");
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

        public void Insere(Entidades.FornecedorRepresentanteLegal fornecedorRepresentanteLegal)
        {
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingLock();
                Insere(contexto, fornecedorRepresentanteLegal);
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
        }

        public void Insere(DataContext ctx, Entidades.FornecedorRepresentanteLegal fornecedorRepresentanteLegal)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @"  INSERT INTO PrestacaoContas.FORNECEDORREPRESENTANTELEGAL
                                               (FORNECEDORID
                                               ,NOME
                                               ,CPF
                                               ,DATAINICIO
                                               ,DATAFIM
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@FORNECEDORID
                                               ,@NOME
                                               ,@CPF
                                               ,@DATAINICIO
                                               ,@DATAFIM
                                               ,@USUARIOID
                                               ,@DATACADASTRO
                                               ,@DATAALTERACAO)   

                update PrestacaoContas.FORNECEDOR set FINALIZADO = 0 where FORNECEDORID = @FORNECEDORID
                ";

                contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorRepresentanteLegal.FornecedorId);
                contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, fornecedorRepresentanteLegal.Nome);
                contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, fornecedorRepresentanteLegal.Cpf);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, fornecedorRepresentanteLegal.DataInicio.Date);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, fornecedorRepresentanteLegal.DataFim ?? (object)DBNull.Value);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, fornecedorRepresentanteLegal.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
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
        }

        public void Atualiza(Entidades.FornecedorRepresentanteLegal fornecedorRepresentanteLegal)
        {
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingLock();
                
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @"  UPDATE PRESTACAOCONTAS.FORNECEDORREPRESENTANTELEGAL 
                                            SET    FORNECEDORID = @FORNECEDORID, 
                                                   NOME = @NOME, 
                                                   CPF = @CPF, 
                                                   DATAINICIO = @DATAINICIO, 
                                                   DATAFIM = @DATAFIM, 
                                                   USUARIOID = @USUARIOID, 
                                                   DATAALTERACAO = @DATAALTERACAO 
                                            WHERE  FORNECEDORREPRESENTANTELEGALID = @FORNECEDORREPRESENTANTELEGALID 

                                            update PrestacaoContas.FORNECEDOR set FINALIZADO = 0 where FORNECEDORID = @FORNECEDORID
                ";

                contextQuery.Parameters.Add("@FORNECEDORREPRESENTANTELEGALID", SqlDbType.Int, fornecedorRepresentanteLegal.FornecedorRepresentanteLegalId);
                contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorRepresentanteLegal.FornecedorId);
                contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, fornecedorRepresentanteLegal.Nome);
                contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, fornecedorRepresentanteLegal.Cpf);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, fornecedorRepresentanteLegal.DataInicio.Date);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, fornecedorRepresentanteLegal.DataFim ?? (object)DBNull.Value);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, fornecedorRepresentanteLegal.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                contexto.ApplyModifications(contextQuery);
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
        }

        public ValidacaoDados ValidaRemocao(int fornecedorRepresentanteLegalId)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados();

            if (fornecedorRepresentanteLegalId <= 0)
                mensagens.Add("Campo CÓDIGO é obrigatório.");

            if (mensagens.Count == 0)
            {
                DataContext contexto = null;

                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (!this.EhAtivoPor(contexto, fornecedorRepresentanteLegalId))
                        mensagens.Add("Apenas representante legal vigente pode ser editado.");
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

        public void Remove(int fornecedorRepresentanteLegalId)
        {
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingLock();
                
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @"  
                    declare @FORNECEDORID int
                    select @FORNECEDORID = FORNECEDORID from PRESTACAOCONTAS.FORNECEDORREPRESENTANTELEGAL 
                    WHERE  FORNECEDORREPRESENTANTELEGALID = @FORNECEDORREPRESENTANTELEGALID

                    update PrestacaoContas.FORNECEDOR set FINALIZADO = 0 where FORNECEDORID = @FORNECEDORID

                    DELETE PRESTACAOCONTAS.FORNECEDORREPRESENTANTELEGAL 
                    WHERE  FORNECEDORREPRESENTANTELEGALID = @FORNECEDORREPRESENTANTELEGALID
                ";

                contextQuery.Parameters.Add("@FORNECEDORREPRESENTANTELEGALID", SqlDbType.Int, fornecedorRepresentanteLegalId);

                contexto.ApplyModifications(contextQuery);
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
        }


        public bool PossuiRepresentanteLegalPor(DataContext ctx, string cpf, int fornecedorRepresentanteLegalId)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT  COUNT(*)
                                FROM    PrestacaoContas.FORNECEDORREPRESENTANTELEGAL (NOLOCK)
                                WHERE   CPF = @CPF
										AND FORNECEDORREPRESENTANTELEGALID <> @FORNECEDORREPRESENTANTELEGALID ";

                contextQuery.Parameters.Add("@CPF", cpf);
                contextQuery.Parameters.Add("@FORNECEDORREPRESENTANTELEGALID", SqlDbType.Int, fornecedorRepresentanteLegalId);

                return ctx.GetReturnValue<int>(contextQuery) > 0;
            }
            catch (Exception ex)
            {
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
        }
    }
}