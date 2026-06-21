using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlTypes;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class DocumentosFornecedor
    {
        public bool PossuiDocumentosObrigatoriosPendentes(DataContext ctx, int fornecedorId, string tipo)
        {
            bool possui = false;

            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT COUNT(*)
                                        FROM PRESTACAOCONTAS.DOCUMENTOSNECESSARIOSFORNECEDOR DN (nolock)
	                                        LEFT JOIN PRESTACAOCONTAS.DOCUMENTOSFORNECEDOR DF (nolock) 
				                                        ON DN.DOCUMENTOSNECESSARIOSFORNECEDORID = DF.DOCUMENTOSNECESSARIOSFORNECEDORID 
				                                        AND DF.FORNECEDORID = @FORNECEDORID
	                                        LEFT JOIN PRESTACAOCONTAS.FORNECEDORDOCUMENTOARQUIVO DFA (nolock) 
				                                        ON DF.DOCUMENTOSFORNECEDORID = DFA.DOCUMENTOSFORNECEDORID
                                        WHERE DN.TIPO = @TIPO
	                                        AND DFA.NOMEARQUIVO IS NULL 
	                                        AND DN.ATIVO = 1
	                                        AND GETDATE() BETWEEN DN.DATAINICIO AND ISNULL(DN.DATAFIM, GETDATE() + 1)  ";

                contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorId);
                contextQuery.Parameters.Add("@TIPO", SqlDbType.VarChar, tipo);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
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

        public bool PossuiDocumentosForaDataAtual(DataContext ctx, int fornecedorId)
        {
            bool possui = false;

            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT COUNT(*)
                                        FROM PRESTACAOCONTAS.DOCUMENTOSFORNECEDOR DF (nolock) 
	                                        LEFT JOIN PRESTACAOCONTAS.FORNECEDORDOCUMENTOARQUIVO DFA (nolock) 
				                                        ON DF.DOCUMENTOSFORNECEDORID = DFA.DOCUMENTOSFORNECEDORID
                                        WHERE DF.FORNECEDORID = @FORNECEDORID
	                                        AND ((DF.DATAFIM IS NULL AND DF.DATAINICIO > GETDATE())
		                                        OR (DF.DATAFIM IS NOT NULL AND GETDATE() NOT BETWEEN  DF.DATAINICIO AND DF.DATAFIM)) ";

                contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorId);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
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

        public DataTable ListaDocumentosPrestesAExpirar(int fornecedorId, DateTime dataLimite)
        {
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" 
                SELECT 
                DF.DOCUMENTOSFORNECEDORID
                ,DF.FORNECEDORID
                ,DN.DOCUMENTOSNECESSARIOSFORNECEDORID
                ,DN.DESCRICAO
                ,DN.PERIODICIDADE as PERIODICIDADE_MESES
                ,CASE
                    WHEN DN.PERIODICIDADE = 0 THEN ''
                    WHEN DN.PERIODICIDADE = 1 THEN 'Mensal'
                    WHEN DN.PERIODICIDADE = 2 THEN 'Bimestral'
                    WHEN DN.PERIODICIDADE = 3 THEN 'Trimestral'
                    WHEN DN.PERIODICIDADE = 6 THEN 'Semestral'                       
                    WHEN DN.PERIODICIDADE = 12 THEN 'Anual'
                    ELSE 'Sim'
                END PERIODICIDADE
                ,CASE WHEN DF.DATAINICIO = '1753-01-01 00:00:00.000' THEN NULL 
                    ELSE DF.DATAINICIO END AS DATAINICIO
                ,DF.DATAFIM
                ,CASE
                    WHEN FORNECEDORDOCUMENTOARQUIVOID IS NULL THEN 'Não'
                    ELSE 'Sim'
                END ENVIADO
                ,case
	                WHEN DF.DATAFIM < getdate() then 'Sim'
	                else 'Não'
                end EXPIRADO

                FROM  
                PRESTACAOCONTAS.DOCUMENTOSNECESSARIOSFORNECEDOR DN (nolock)
                LEFT JOIN PRESTACAOCONTAS.DOCUMENTOSFORNECEDOR DF (nolock) ON DN.DOCUMENTOSNECESSARIOSFORNECEDORID = DF.DOCUMENTOSNECESSARIOSFORNECEDORID --AND DF.FORNECEDORID = @FORNECEDORID
                LEFT JOIN PRESTACAOCONTAS.FORNECEDORDOCUMENTOARQUIVO DFA (nolock) ON DF.DOCUMENTOSFORNECEDORID = DFA.DOCUMENTOSFORNECEDORID
                left join PrestacaoContas.FORNECEDOR f (nolock) on f.FORNECEDORID = DF.FORNECEDORID

                where 
                DF.DATAFIM <= @DATALIMITE
                and DF.FORNECEDORID = @FORNECEDORID

                order by DF.DATAFIM desc
                ";

                contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorId);
                contextQuery.Parameters.Add("@DATALIMITE", SqlDbType.DateTime, dataLimite);

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

        public DataTable ListaPor(int fornecedorId, string tipo)
        {
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT DF.DOCUMENTOSFORNECEDORID
                                                ,DN.DOCUMENTOSNECESSARIOSFORNECEDORID
                                                ,DN.DESCRICAO
                                                ,DN.PERIODICIDADE as PERIODICIDADE_MESES
                                                ,CASE
                                                    WHEN DN.PERIODICIDADE = 0 THEN ''
                                                    WHEN DN.PERIODICIDADE = 1 THEN 'Mensal'
                                                    WHEN DN.PERIODICIDADE = 2 THEN 'Bimestral'
                                                    WHEN DN.PERIODICIDADE = 3 THEN 'Trimestral'
                                                    WHEN DN.PERIODICIDADE = 6 THEN 'Semestral'                       
                                                    WHEN DN.PERIODICIDADE = 12 THEN 'Anual'
                                                    ELSE 'Sim'
                                                END PERIODICIDADE
                                                  ,CASE WHEN DF.DATAINICIO = '1753-01-01 00:00:00.000' THEN NULL 
                                                      ELSE DF.DATAINICIO END AS DATAINICIO

                                                ,DF.DATAFIM
                                                ,DN.DATAINICIO as DATAINICIOMINIMA
                                                ,DN.DATAFIM as DATAFIMMAXIMA
                                                ,CASE
                                                    WHEN FORNECEDORDOCUMENTOARQUIVOID IS NULL THEN 'Não'
                                                    ELSE 'Sim'
                                                END ENVIADO
                                                ,DFA.FORNECEDORDOCUMENTOARQUIVOID
                                                ,DFA.CHAVEARQUIVO
                                                ,DFA.ARQUIVO
                                                ,DFA.TIPOARQUIVO
                                                ,DFA.NOMEARQUIVO
                                                FROM  PRESTACAOCONTAS.DOCUMENTOSNECESSARIOSFORNECEDOR DN (nolock)
					                                LEFT JOIN PRESTACAOCONTAS.DOCUMENTOSFORNECEDOR DF (nolock) 
							                                ON DN.DOCUMENTOSNECESSARIOSFORNECEDORID = DF.DOCUMENTOSNECESSARIOSFORNECEDORID 
							                                AND DF.FORNECEDORID = @FORNECEDORID
					                                LEFT JOIN PRESTACAOCONTAS.FORNECEDORDOCUMENTOARQUIVO DFA (nolock) 
							                                ON DF.DOCUMENTOSFORNECEDORID = DFA.DOCUMENTOSFORNECEDORID
                                                WHERE TIPO = @TIPO
					                                AND DN.DATAINICIO <= GETDATE()
					                                AND (DN.DATAFIM IS NULL OR DN.DATAFIM >= GETDATE()) ";

                contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorId);
                contextQuery.Parameters.Add("@TIPO", SqlDbType.VarChar, tipo);

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

        public bool PossuiDocumentosNecessariosFornecedorPor(DataContext contexto, int documentosNecessariosFornecedorId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.DOCUMENTOSFORNECEDOR (NOLOCK)
                                    WHERE DOCUMENTOSNECESSARIOSFORNECEDORID = @DOCUMENTOSNECESSARIOSFORNECEDORID ";

            contextQuery.Parameters.Add("@DOCUMENTOSNECESSARIOSFORNECEDORID", SqlDbType.Int, documentosNecessariosFornecedorId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public ValidacaoDados Valida(Entidades.DocumentosFornecedor documentosFornecedor)
        {
            var validacaoDados = new ValidacaoDados();
            var mensagens = new List<string>();

            if (documentosFornecedor.DataInicio.Value <= SqlDateTime.MinValue.Value)
            {
                mensagens.Add("DATA DE INICIO: não pode ser menor ou igual do que " + SqlDateTime.MinValue.Value.ToString("dd/MM/yyyy"));
            }

            if (documentosFornecedor.DataInicio.Value > DateTime.Now.Date)
            {
                mensagens.Add("DATA DE INICIO: não pode ser maior do que a data de hoje.");
            }

            if (documentosFornecedor.DataFim.HasValue && documentosFornecedor.DataFim.Value > SqlDateTime.MaxValue.Value)
            {
                mensagens.Add("DATA DE FIM: não pode ser maior do que " + SqlDateTime.MaxValue.Value.ToString("dd/MM/yyyy"));
            }

            if (documentosFornecedor.DataFim.HasValue && documentosFornecedor.DataFim.Value < documentosFornecedor.DataInicio.Value)
            {
                mensagens.Add("DATA DE FIM: não pode ser menor do que a DATA DE INÍCIO.");
            }

            if (documentosFornecedor.DataFim.HasValue && documentosFornecedor.DataFim.Value < DateTime.Now.Date)
            {
                mensagens.Add("DATA DE FIM: não pode ser menor do que a data de hoje.");
            }

            if (mensagens.Count == 0)
            {
                DataContext contexto = null;

                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    DateTime dataInicioMin, dataFimMax;
                    int periodicidade;
                    ObterRangeDeDataValidoPor(contexto, documentosFornecedor.DocumentosNecessariosFornecedorId, out periodicidade, out dataInicioMin, out dataFimMax);

                    if (documentosFornecedor.DataInicio.Value.Date < dataInicioMin.Date)
                    {
                        mensagens.Add("DATA DE INÍCIO: não pode ser menor do que " + dataInicioMin.ToString("dd/MM/yyyy") + " para este documento");
                    }

                    if (documentosFornecedor.DataFim.HasValue && documentosFornecedor.DataFim.Value.Date > dataFimMax.Date)
                    {
                        mensagens.Add("DATA DE FIM: não pode ser maior do que " + dataFimMax.ToString("dd/MM/yyyy") + " para este documento");
                    }

                    if (!mensagens.Any() && periodicidade > 0 && (!documentosFornecedor.DataFim.HasValue
                        || (documentosFornecedor.DataFim.HasValue
                        && documentosFornecedor.DataFim.Value != documentosFornecedor.DataInicio.Value.AddMonths(periodicidade).AddDays(-1))))
                    {
                        mensagens.Add("DATA DE FIM: conforme a periodicidade, esta data deve ser obrigatoriamente " + documentosFornecedor.DataInicio.Value.AddMonths(periodicidade).AddDays(-1).ToString("dd/MM/yyyy"));
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
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        private void ObterRangeDeDataValidoPor(DataContext contexto, int documentosNecessariosFornecedorId, out int periodicidade, out DateTime dataInicioMin, out DateTime dataFimMax)
        {
            ContextQuery contextQuery = new ContextQuery();
            periodicidade = 0;
            dataInicioMin = SqlDateTime.MinValue.Value;
            dataFimMax = SqlDateTime.MaxValue.Value;

            contextQuery.Command = @"
                    select PERIODICIDADE, DATAINICIO ,DATAFIM
                    from PrestacaoContas.DOCUMENTOSNECESSARIOSFORNECEDOR dnf (nolock)
                    where DOCUMENTOSNECESSARIOSFORNECEDORID = @DOCUMENTOSNECESSARIOSFORNECEDORID
                    ";

            contextQuery.Parameters.Add("@DOCUMENTOSNECESSARIOSFORNECEDORID", SqlDbType.Int, documentosNecessariosFornecedorId);

            using (var dr = contexto.GetDataReader(contextQuery))
                while (dr.Read())
                {
                    periodicidade = dr["PERIODICIDADE"] != DBNull.Value ? Convert.ToInt32(dr["PERIODICIDADE"]) : 0;
                    dataInicioMin = dr["DATAINICIO"] != DBNull.Value ? Convert.ToDateTime(dr["DATAINICIO"]) : SqlDateTime.MinValue.Value;
                    dataFimMax = dr["DATAFIM"] != DBNull.Value ? Convert.ToDateTime(dr["DATAFIM"]) : SqlDateTime.MaxValue.Value;
                }
        }

        public void Atualiza(Entidades.DocumentosFornecedor documentosFornecedor)
        {
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingLock();

                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @"
                update PrestacaoContas.DOCUMENTOSFORNECEDOR set
                DATAINICIO = @DATAINICIO
                ,DATAFIM = @DATAFIM
                ,USUARIOID = @USUARIOID
                ,DATAALTERACAO = @DATAALTERACAO
                where DOCUMENTOSFORNECEDORID = @DOCUMENTOSFORNECEDORID

                declare @FORNECEDORID int
                select @FORNECEDORID = FORNECEDORID from PrestacaoContas.DOCUMENTOSFORNECEDOR df (nolock) where DOCUMENTOSFORNECEDORID = @DOCUMENTOSFORNECEDORID

                update PrestacaoContas.FORNECEDOR set ENVIADO = 0, FINALIZADO = NULL where FORNECEDORID = @FORNECEDORID
                ";

                contextQuery.Parameters.Add("@DOCUMENTOSFORNECEDORID", SqlDbType.Int, documentosFornecedor.DocumentosFornecedorId);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, documentosFornecedor.DataInicio.HasValue ? documentosFornecedor.DataInicio.Value.Date : (object)DBNull.Value);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, documentosFornecedor.DataFim.HasValue ? documentosFornecedor.DataFim.Value.Date : (object)DBNull.Value);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, documentosFornecedor.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, documentosFornecedor.DataAlteracao);

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

        public void Insere(Entidades.DocumentosFornecedor documentosFornecedor)
        {
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingLock();

                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @"
                insert into PrestacaoContas.DOCUMENTOSFORNECEDOR (
                    FORNECEDORID
                    ,DOCUMENTOSNECESSARIOSFORNECEDORID
                    ,DATAINICIO
                    ,DATAFIM
                    ,USUARIOID
                    ,DATACADASTRO
                    ,DATAALTERACAO
                ) values (
                    @FORNECEDORID
                    ,@DOCUMENTOSNECESSARIOSFORNECEDORID
                    ,@DATAINICIO
                    ,@DATAFIM
                    ,@USUARIOID
                    ,@DATACADASTRO
                    ,@DATACADASTRO
                )

                update PrestacaoContas.FORNECEDOR set ENVIADO = 0, FINALIZADO = NULL where FORNECEDORID = @FORNECEDORID
                ";

                contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, documentosFornecedor.FornecedorId);
                contextQuery.Parameters.Add("@DOCUMENTOSNECESSARIOSFORNECEDORID", SqlDbType.Int, documentosFornecedor.DocumentosNecessariosFornecedorId);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, documentosFornecedor.DataInicio.HasValue ? documentosFornecedor.DataInicio.Value.Date : (object)DBNull.Value);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, documentosFornecedor.DataFim.HasValue ? documentosFornecedor.DataFim.Value.Date : (object)DBNull.Value);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, documentosFornecedor.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);

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
    }
}
