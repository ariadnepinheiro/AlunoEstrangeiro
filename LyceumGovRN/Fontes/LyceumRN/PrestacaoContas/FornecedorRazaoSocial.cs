using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class FornecedorRazaoSocial
    {
        public bool PossuiRazaoSocialVigentePor(DataContext ctx, int fornecedorId, DateTime data)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT COUNT(0)
                                        FROM PrestacaoContas.FORNECEDORRAZAOSOCIAL (nolock)
                                        WHERE FORNECEDORID = @FORNECEDORID
                                            AND (DATAFIM IS NULL or CONVERT(DATE, DATAFIM) >= CONVERT(DATE, @DATA)) ";

                contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorId);
                contextQuery.Parameters.Add("@DATA", SqlDbType.DateTime, data);

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

        public void Insere(DataContext contexto, Entidades.FornecedorRazaoSocial fornecedorRazaoSocial)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO PrestacaoContas.FORNECEDORRAZAOSOCIAL (
                                            FORNECEDORID
                                            ,DESCRICAO
                                            ,DATAINICIO
                                            ,DATAFIM
                                            ,USUARIOID
                                            ,DATACADASTRO
                                            ,DATAALTERACAO
                                        )
                                        VALUES (
                                            @FORNECEDORID
                                            ,@RAZAOSOCIAL
                                            ,@DATACADASTRO
                                            ,null
                                            ,@USUARIOID
                                            ,@DATACADASTRO
                                            ,@DATAALTERACAO
                                        ) ";

            contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorRazaoSocial.FornecedorId);
            contextQuery.Parameters.Add("@RAZAOSOCIAL", SqlDbType.VarChar, fornecedorRazaoSocial.Descricao);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, fornecedorRazaoSocial.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fornecedorId"></param>
        /// <returns></returns>
        public DataTable ListaPor(int fornecedorId)
        {
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT FORNECEDORRAZAOSOCIALID, 
		                                    FORNECEDORID, 
		                                    DESCRICAO, 
		                                    DATAINICIO, 
		                                    DATAFIM, 
		                                    USUARIOID, 
		                                    DATACADASTRO, 
		                                    DATAALTERACAO
                                    FROM [PrestacaoContas].[FORNECEDORRAZAOSOCIAL] (NOLOCK)
                                    WHERE FORNECEDORID = @FORNECEDORID";

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

        /// <summary>
        /// Valida os dados da entidade, para que esteja apta a ser recebida pelo Insere() e Atualiza()
        /// TABELAS: PrestacaoContas.FORNECEDORRAZAOSOCIAL
        /// </summary>
        /// <param name="razaoSocial">entidade que contém os campos necessários para inserir e atualizar a razão social do fornecedor</param>
        /// <param name="cadastro">TRUE se for inserção, FALSE se for atualização</param>
        /// <returns>Objeto ValidacaoDados, que te diz se os dados são válidos ou não. Se não forem, retorna uma coleção de erros</returns>
        public ValidacaoDados Valida(Entidades.FornecedorRazaoSocial razaoSocial, bool cadastro)
        {
            RN.PrestacaoContas.Fornecedor rnFornecedor = new Fornecedor();
            var validacaoDados = new ValidacaoDados();
            var mensagens = new List<string>();

            if (!cadastro && !razaoSocial.FornecedorRazaoSocialId.HasValue)
                mensagens.Add("ID obrigatório para atualização");
            
            if (razaoSocial.Descricao.IsNullOrEmptyOrWhiteSpace())
                mensagens.Add("RAZÃO SOCIAL: Preenchimento obrigatório");

            if (razaoSocial.Descricao.Length > 500)
                mensagens.Add("RAZÃO SOCIAL: Não pode ter mais do que 500 caracteres");

            if (razaoSocial.DataInicio <= SqlDateTime.MinValue.Value)
                mensagens.Add("DATA DE INÍCIO: Não pode ser menor ou igual do que " + SqlDateTime.MinValue.Value.ToString("dd/MM/yyyy"));

            if (razaoSocial.DataInicio > SqlDateTime.MaxValue.Value)
                mensagens.Add("DATA DE INÍCIO: Não pode ser maior do que " + SqlDateTime.MaxValue.Value.ToString("dd/MM/yyyy"));

            if (razaoSocial.DataFim.HasValue) 
            {
                if (razaoSocial.DataFim <= SqlDateTime.MinValue.Value)
                    mensagens.Add("DATA DE FIM: Não pode ser menor ou igual do que " + SqlDateTime.MinValue.Value.ToString("dd/MM/yyyy"));

                if (razaoSocial.DataFim > SqlDateTime.MaxValue.Value)
                    mensagens.Add("DATA DE FIM: Não pode ser maior do que " + SqlDateTime.MaxValue.Value.ToString("dd/MM/yyyy"));
            }

            if (mensagens.Count == 0)
            {
                DataContext contexto = null;

                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (!cadastro && !RazaoSocialExiste(contexto, razaoSocial.FornecedorRazaoSocialId.Value))
                    {
                        mensagens.Add("FORNECEDORRAZAOSOCIALID não existe cadastrado no banco de dados");
                    }

                    if (!rnFornecedor.ExistePor(contexto, razaoSocial.FornecedorId))
                    {
                        mensagens.Add("FORNECEDORID não existe cadastrado no banco de dados");
                    }

                    if (razaoSocial.FornecedorRazaoSocialId.HasValue && TemDataInterseccionada(contexto, razaoSocial.FornecedorRazaoSocialId.Value, razaoSocial.DataInicio))
                        mensagens.Add("DATA DE INÍCIO: não pode estar entre a data de início e data de fim de alguma razão social já existente neste fornecedor");

                    if (razaoSocial.FornecedorRazaoSocialId.HasValue && TemDataInterseccionada(contexto, razaoSocial.FornecedorRazaoSocialId.Value, razaoSocial.DataFim))
                        mensagens.Add("DATA DE FIM: não pode estar entre a data de início e data de fim de alguma razão social já existente neste fornecedor");
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
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fornecedorRazaoSocialId"></param>
        /// <returns></returns>
        public ValidacaoDados ValidaRemocao(int fornecedorRazaoSocialId)
        {
            var validacaoDados = new ValidacaoDados();
            var mensagens = new List<string>();

            //escreva a validação aqui!

            if (mensagens.Count == 0)
            {
                DataContext contexto = null;

                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //escreva a validação de banco de dados aqui!
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
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fornecedorRazaoSocial"></param>
        public void Atualiza(Entidades.FornecedorRazaoSocial fornecedorRazaoSocial)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"
                UPDATE PRESTACAOCONTAS.FORNECEDORRAZAOSOCIAL 
                SET
                    DESCRICAO = @DESCRICAO
                    ,DATAINICIO = @DATAINICIO
                    ,DATAFIM = @DATAFIM
                    ,USUARIOID = @USUARIOID
                    ,DATAALTERACAO = @DATAALTERACAO
                WHERE FORNECEDORRAZAOSOCIALID = @FORNECEDORRAZAOSOCIALID

                declare @FORNECEDORID int
                select @FORNECEDORID = FORNECEDORID 
                from PRESTACAOCONTAS.FORNECEDORRAZAOSOCIAL 
                WHERE FORNECEDORRAZAOSOCIALID = @FORNECEDORRAZAOSOCIALID

                update PrestacaoContas.FORNECEDOR 
                set ENVIADO = 0, 
                    FINALIZADO = NULL 
                where FORNECEDORID = @FORNECEDORID
                ";

                contextQuery.Parameters.Add("@FORNECEDORRAZAOSOCIALID", SqlDbType.Int, fornecedorRazaoSocial.FornecedorRazaoSocialId);
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, fornecedorRazaoSocial.Descricao);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, fornecedorRazaoSocial.DataInicio);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, fornecedorRazaoSocial.DataFim ?? (object)DBNull.Value);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, fornecedorRazaoSocial.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                contexto.ApplyModifications(contextQuery);
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

        public void AtualizaFornecedor(DataContext contexto, Entidades.FornecedorRazaoSocial fornecedorRazaoSocial)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"
                DECLARE @ANTIGAFORNECEDORRAZAOSOCIALID INT
                DECLARE @ANTIGARAZAOSOCIAL VARCHAR(MAX)               
                
                SELECT @ANTIGAFORNECEDORRAZAOSOCIALID = FORNECEDORRAZAOSOCIALID, 
                       @ANTIGARAZAOSOCIAL = DESCRICAO 
                FROM PrestacaoContas.FORNECEDORRAZAOSOCIAL frs
                WHERE FORNECEDORID = @FORNECEDORID 
                    AND (DATAFIM IS NULL OR CONVERT(DATE, DATAFIM) >= CONVERT(DATE, GETDATE()))

                IF (LOWER(TRIM(ISNULL(@RAZAOSOCIAL, ''))) <> LOWER(TRIM(ISNULL(@ANTIGARAZAOSOCIAL, ''))))
                BEGIN
                    UPDATE PRESTACAOCONTAS.FORNECEDORRAZAOSOCIAL 
                    SET
                        DATAFIM = (SELECT DATEADD(D, -1, CAST(GETDATE() AS DATE))),
                        USUARIOID = @USUARIOID,
                        DATAALTERACAO = @DATAALTERACAO
                    WHERE FORNECEDORRAZAOSOCIALID = @ANTIGAFORNECEDORRAZAOSOCIALID

                    INSERT INTO PRESTACAOCONTAS.FORNECEDORRAZAOSOCIAL (
                        FORNECEDORID
                        ,DESCRICAO
                        ,DATAINICIO
                        ,DATAFIM
                        ,USUARIOID
                        ,DATACADASTRO
                        ,DATAALTERACAO
                    )
                    VALUES (
                        @FORNECEDORID
                        ,@RAZAOSOCIAL
                        ,@DATAALTERACAO
                        ,NULL
                        ,@USUARIOID
                        ,@DATAALTERACAO
                        ,@DATAALTERACAO
                    )
                END ";
                                
                contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorRazaoSocial.FornecedorId);
                contextQuery.Parameters.Add("@RAZAOSOCIAL", SqlDbType.VarChar, fornecedorRazaoSocial.Descricao);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, fornecedorRazaoSocial.DataInicio);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, fornecedorRazaoSocial.DataFim ?? (object)DBNull.Value);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, fornecedorRazaoSocial.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                contexto.ApplyModifications(contextQuery);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="fornecedorId"></param>
        /// <returns></returns>
        private bool TemRazaoSocialAtual(DataContext ctx, int fornecedorId)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" 
                                    SELECT COUNT(0) 
                                    FROM PrestacaoContas.FORNECEDORRAZAOSOCIAL (NOLOCK)
                                    WHERE FORNECEDORID = @FORNECEDORID
                                    AND DATAFIM IS NULL
                                    ";

                contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorId);

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="fornecedorId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool TemDataInterseccionada(DataContext ctx, int fornecedorRazaoSocialId, DateTime? data)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" 
                                    select COUNT(0)
                                    from PrestacaoContas.FORNECEDORRAZAOSOCIAL frs1 (nolock)
                                    where frs1.FORNECEDORID = (select FORNECEDORID from PrestacaoContas.FORNECEDORRAZAOSOCIAL where FORNECEDORRAZAOSOCIALID = @FORNECEDORRAZAOSOCIALID)
                                    and frs1.FORNECEDORRAZAOSOCIALID <> @FORNECEDORRAZAOSOCIALID
                                    and (cast(@DATA as date) between cast(DATAINICIO as date) and cast(isnull(DATAFIM, '9999-12-31') as date))
                                    ";

                contextQuery.Parameters.Add("@FORNECEDORRAZAOSOCIALID", SqlDbType.Int, fornecedorRazaoSocialId);
                contextQuery.Parameters.Add("@DATA", SqlDbType.DateTime, data ?? (object)DBNull.Value);

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="fornecedorRazaoSocialId"></param>
        /// <returns></returns>
        private bool RazaoSocialExiste(DataContext ctx, int fornecedorRazaoSocialId)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" 
                                    SELECT COUNT(0) 
                                    FROM PrestacaoContas.FORNECEDORRAZAOSOCIAL (NOLOCK)
                                    WHERE FORNECEDORRAZAOSOCIALID = @FORNECEDORRAZAOSOCIALID
                                    ";

                contextQuery.Parameters.Add("@FORNECEDORRAZAOSOCIALID", SqlDbType.Int, fornecedorRazaoSocialId);

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

        public DateTime? ObtemInicioRazaoSocialAtivaPor(DataContext ctx, int fornecedorId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            DateTime? inicio = null;

            try
            {
                contextQuery.Command = @" SELECT DATAINICIO
                                            FROM   PrestacaoContas.FORNECEDORRAZAOSOCIAL  
											WHERE FORNECEDORID = @FORNECEDORID 
                                            AND DATAFIM IS NULL";

                contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    inicio = (DateTime)reader["DATAINICIO"];
                }

                return inicio;
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }
    }
}
