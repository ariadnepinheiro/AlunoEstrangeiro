using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Patrimonio
{
    public class Movimentacao
    {
        public int ObtemPrimeiraMovimentacaoPor(int bemId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT TOP 1 MOVIMENTACAOID
                                            FROM Patrimonio.MOVIMENTACAO (nolock)
                                            WHERE BEMID = @BEMID
                                            ORDER BY DATAINICIO ";

                contextQuery.Parameters.Add("@BEMID", SqlDbType.Int, bemId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["MOVIMENTACAOID"]);
                }

                return retorno;
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

        public int ObtemProximoNumeroPor(DataContext contexto, string setor)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 1;
            try
            {
                contextQuery.Command = @"  SELECT ISNULL(MAX(NUMERO),0) + 1 AS PROXIMONUMERO
                                         FROM PATRIMONIO.MOVIMENTACAO
                                         WHERE SETOR = @SETOR ";

                contextQuery.Parameters.Add("@SETOR", SqlDbType.VarChar, setor);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["PROXIMONUMERO"]);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return retorno;
        }

        public string ObtemSetorVigentePor(int bemId, DateTime dataPesquisa)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.ObtemSetorVigentePor(contexto, bemId, dataPesquisa);
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

        public string ObtemSetorVigentePor(DataContext contexto, int bemId, DateTime dataPesquisa)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT TOP 1 SETOR 
                                    FROM PATRIMONIO.MOVIMENTACAO (NOLOCK)
                                    WHERE  BEMID = @BEMID
	                                    AND DATAINICIO  <= CONVERT(DATE,@DATA)
	                                    AND (DATAFIM IS NULL OR DATAFIM >= CONVERT(DATE,@DATA)) ";

            contextQuery.Parameters.Add("@BEMID", SqlDbType.Int, bemId);
            contextQuery.Parameters.Add("@DATA", SqlDbType.DateTime, dataPesquisa);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public Entidades.Movimentacao ObtemMovimentacaoVigentePor(DataContext contexto, int bemId, DateTime dataPesquisa)
        {
            SqlDataReader reader = null;
            Entidades.Movimentacao movimentacao = new Entidades.Movimentacao();
            ContextQuery contextQuery = new ContextQuery();

            try
            {

                contextQuery.Command = @" SELECT * 
                                    FROM PATRIMONIO.MOVIMENTACAO (NOLOCK)
                                    WHERE  BEMID = @BEMID
	                                    AND DATAINICIO  <= CONVERT(DATE,@DATA)
	                                    AND (DATAFIM IS NULL OR DATAFIM >= CONVERT(DATE,@DATA)) ";

                contextQuery.Parameters.Add("@BEMID", SqlDbType.Int, bemId);
                contextQuery.Parameters.Add("@DATA", SqlDbType.DateTime, dataPesquisa);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    movimentacao.BemId = Convert.ToInt32(reader["BEMID"]);
                    movimentacao.DataInicio = Convert.ToDateTime(reader["DATAINICIO"]);
                    movimentacao.Setor = Convert.ToString(reader["SETOR"]);
                    movimentacao.Numero = Convert.ToInt32(reader["NUMERO"]);

                    if (reader["DATAFIM"] != DBNull.Value)
                    {
                        movimentacao.DataFim = Convert.ToDateTime(reader["DATAFIM"]);
                    }
                }
                return movimentacao;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public void Insere(DataContext contexto, Entidades.Movimentacao movimentacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Patrimonio.MOVIMENTACAO
                                       (BEMID
                                       ,NUMERO
                                       ,SETOR
                                       ,DATAINICIO
                                       ,USUARIOID
                                       ,DATACADASTRO
                                       ,DATAALTERACAO)
                                 VALUES
                                       (@BEMID
                                       ,@NUMERO
                                       ,@SETOR
                                       ,@DATAINICIO
                                       ,@USUARIOID
                                       ,@DATACADASTRO
                                       ,@DATAALTERACAO)  ";

            contextQuery.Parameters.Add("@BEMID", SqlDbType.Int, movimentacao.BemId);
            contextQuery.Parameters.Add("@NUMERO", SqlDbType.Int, movimentacao.Numero);
            contextQuery.Parameters.Add("@SETOR", SqlDbType.VarChar, movimentacao.Setor);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, movimentacao.DataInicio.Date);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, movimentacao.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void Altera(DataContext contexto, Entidades.Movimentacao movimentacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"   UPDATE Patrimonio.MOVIMENTACAO
                                        SET DATAINICIO = @DATAINICIO,    
                                            USUARIOID = @USUARIOID, 
                                            DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  BEMID = @BEMID ";

            contextQuery.Parameters.Add("@BEMID", SqlDbType.Int, movimentacao.BemId);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, movimentacao.DataInicio.Date);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, movimentacao.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void FinalizaMovimentacaoAtiva(DataContext contexto, int bemId, DateTime dataFim, string usuarioResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE PATRIMONIO.MOVIMENTACAO
                                       SET DATAFIM = @DATAFIM,
                                          USUARIOID = @USUARIOID,
                                          DATAALTERACAO = @DATAALTERACAO
                                     WHERE BEMID = @BEMID
	                                    AND (DATAFIM IS NULL OR DATAFIM >= GETDATE())
                                     ";

            contextQuery.Parameters.Add("@BEMID", SqlDbType.Int, bemId);
            contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, dataFim.Date);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public DateTime ObtemInicioMovimentacaoAtivaPor(DataContext contexto, int bemId)
        {
            DateTime data = DateTime.MinValue;
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT MV.DATAINICIO
                                            FROM PATRIMONIO.MOVIMENTACAO MV (NOLOCK) 
                                            WHERE  BEMID = @BEMID 
	                                            AND MV.DATAFIM IS NULL ";

                contextQuery.Parameters.Add("@BEMID", SqlDbType.Int, bemId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    data = Convert.ToDateTime(reader["DATAINICIO"]);
                }

                return data;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public DataTable ObtemDemonstrativoMovimentacaoPor(int idRegional, string setor, DateTime dataInicio, DateTime dataFim, out DateTime dataSaldoAnterior, out DateTime dataSaldoFinal)
        {
            DataTable lista = null;
            lista = this.ObtemDemonstrativoMovimentacaoInternoPor(idRegional, setor, dataInicio, dataFim, out dataSaldoAnterior, out dataSaldoFinal);
            return lista;
        }

        public DataTable ObtemDemonstrativoMovimentacaoPor(string setor, DateTime dataInicio, DateTime dataFim, out DateTime dataSaldoAnterior, out DateTime dataSaldoFinal)
        {
            DataTable lista = null;
            lista = this.ObtemDemonstrativoMovimentacaoInternoPor(0, setor, dataInicio, dataFim, out dataSaldoAnterior, out dataSaldoFinal);
            return lista;
        }

        public DataTable ObtemDemonstrativoMovimentacaoPor(DateTime dataInicio, DateTime dataFim, out DateTime dataSaldoAnterior, out DateTime dataSaldoFinal)
        {
            DataTable lista = null;
            lista = this.ObtemDemonstrativoMovimentacaoInternoPor(0, null, dataInicio, dataFim, out dataSaldoAnterior, out dataSaldoFinal);
            return lista;
        }

        private DataTable ObtemDemonstrativoMovimentacaoInternoPor(int idRegional, string setor, DateTime dataInicio, DateTime dataFim, out DateTime dataSaldoAnterior, out DateTime dataSaldoFinal)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable lista = null;
            dataSaldoAnterior = DateTime.MinValue;

            try
            {
                dataSaldoAnterior = dataInicio.AddDays(-1);
                dataSaldoFinal = dataFim;

                //Valida datas               
                if (dataInicio.Date > DateTime.Now.Date)
                {
                    throw new Exception("ERRO: A data início não pode ser maior que a data atual.");
                }

                if (dataFim.Date > DateTime.Now.Date)
                {
                    throw new Exception("ERRO: A data fim não pode ser maior que a data atual.");
                }

                if (dataFim.Date <= dataInicio.Date)
                {
                    throw new Exception("ERRO: A data fim não pode ser menor ou igual a data início.");
                }

                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                contexto.CommandTimeout = 600; //10 min

                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;

                //Verifica se será por regional ou unidade
                if (!setor.IsNullOrEmptyOrWhiteSpace() || idRegional > 0)
                {
                    //Caso o relatorio seja por unidade limpa informação de regional
                    if (!setor.IsNullOrEmptyOrWhiteSpace())
                    {
                        idRegional = 0;
                    }

                    //Caso o relatorio seja por regional limpa informação de unidade
                    if (idRegional > 0)
                    {
                        setor = null;
                    }

                    contextQuery.Command = @"Patrimonio.SP_DEMONSTRATIVOMOVIMENTACAO";
                }
                else
                {
                    contextQuery.Command = @"Patrimonio.SP_DEMONSTRATIVOMOVIMENTACAOTOTAL";
                }

                contextQuery.Parameters.Add("@DATAINICIO", dataInicio.Date);
                contextQuery.Parameters.Add("@DATAFIM", dataFim.Date);
                contextQuery.Parameters.Add("@DATASALDOANTERIOR", dataSaldoAnterior.Date);
                contextQuery.Parameters.Add("@DATASALDOFINAL", dataSaldoFinal.Date);

                if (!setor.IsNullOrEmptyOrWhiteSpace() || idRegional > 0)
                {
                    contextQuery.Parameters.Add("@SETOR", SqlDbType.VarChar, setor);
                    contextQuery.Parameters.Add("@REGIONALID", SqlDbType.VarChar, idRegional);
                }

                lista = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                if (contexto != null)
                {
                    contexto.Abandon();
                }
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    if (Convert.ToString(ex.Message).Contains("ERRO:"))
                    {
                        mensagem = ex.Message;
                    }
                    else
                    {
                        mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine,
                            Convert.ToString(ex.Message));
                    }
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                {
                    contexto.Dispose();
                }
            }

            return lista;
        }

        public DataTable ObtemEntradaBensMoveisPor(int idRegional, string setor, int? idClassificacao, int ano, int mesInicio, int mesFim)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            DateTime dataFim;
            DateTime dataInicio;
            StringBuilder sql = new StringBuilder();

            try
            {
                //Monta datas para verificação:
                dataInicio = new DateTime(ano, mesInicio, 1);
                dataFim = new DateTime(ano, mesFim, DateTime.DaysInMonth(ano, mesFim));

                //Valida data
                if (mesInicio > mesFim)
                {
                    throw new Exception("ERRO: O mês inicio deve ser menor ou igual ao mês final.");
                }
                else if (dataFim.Date >= DateTime.Now.Date)
                {
                    throw new Exception(string.Format("ERRO: A consulta deste ano / mês estará disponivel após {0}.", dataFim.ToString("dd/MM/yyyy")));
                }

                //Caso o relatorio seja por unidade limpa informação de regional
                if (!setor.IsNullOrEmptyOrWhiteSpace())
                {
                    idRegional = 0;
                }

                if (idRegional > 0)
                {
                    sql.Append(@"  --Busca de uas da regional
                                        SELECT distinct SETORID
                                        INTO #UASREGIONAL
                                        FROM HADES.DBO.REGIONAL__SETOR (NOLOCK) 
                                        WHERE REGIONALID = @REGIONALID

                                        INSERT INTO #UASREGIONAL
                                        SELECT distinct SETOR AS SETORID
                                        FROM LY_UNIDADE_ENSINO (NOLOCK) 
                                        WHERE ID_REGIONAL = @REGIONALID 
                                        AND SETOR IS NOT NULL
                                ");
                }

                sql.Append(@" SELECT DISTINCT S.NOME AS UNIDADEADMINISTRATIVA,
				                            C.CONTA,
				                            REPLICATE('0',6 - LEN(M.NUMERO)) + CONVERT(VARCHAR(6), M.NUMERO) AS NUMERO,
				                            B.DESCRICAO,
				                            'Unid' as UNIDADEMEDIDA,
				                            1 as QUANTIDADE,
				                            PATRIMONIO.FN_CALCULOVALORATUALIZADO(B.BEMID, M.DATAINICIO, I.DATAINICIO, I.VALOR, CV.TAXAVALORRESIDUAL,CV.VIDAUTIL) AS 	VALORCALCULADO, 
				                            CASE 
					                            WHEN T.TRANSFERENCIAITEMID IS NOT NULL
						                            THEN 'ATBM: ' + CONVERT(VARCHAR(20), T.TRANSFERENCIAID) + '/' + CONVERT(VARCHAR(20), YEAR(T.DATAMOVIMENTACAO))
					                            ELSE  B.DOCUMENTOHABIL
				                            END OBSERVACAO,
				                            M.SETOR, 
				                            C.CLASSIFICACAOID, 
				                            B.BEMID,
				                            M.DATAINICIO, 
				                            M.DATAFIM,
				                            M.MOVIMENTACAOID,
					                        I.VALOR as VALORINICIAL
                            FROM   PATRIMONIO.MOVIMENTACAO M (NOLOCK) 
		                            INNER JOIN PATRIMONIO.BEM B (NOLOCK) 
				                            ON M.BEMID = B.BEMID 
                                    INNER JOIN PATRIMONIO.BEMVALOR I (NOLOCK) 
	                                        ON B.BEMID = I.BEMID 
		                                        AND I.DATAINICIO <= M.DATAINICIO
		                                        AND ( I.DATAFIM IS NULL 
				                                        OR i.DATAFIM >= M.DATAINICIO ) 
		                            INNER JOIN Patrimonio.CLASSIFICACAO C (NOLOCK)
				                            ON C.CLASSIFICACAOID = B.CLASSIFICACAOID
                                    INNER JOIN Patrimonio.CLASSIFICACAOVIGENCIA CV (NOLOCK) 
                                            ON B.CLASSIFICACAOID = CV.CLASSIFICACAOID
	                                        AND CV.DATAINICIO <= M.DATAINICIO
	                                        AND  ( CV.DATAFIM IS NULL 
                                                  OR CV.DATAFIM >= M.DATAINICIO ) ");

                if (idRegional > 0)
                {
                    sql.Append(@"
                                                INNER JOIN #UASREGIONAL UR 
				                                        ON M.SETOR = UR.SETORID ");
                }

                sql.Append(@" 
		                            INNER JOIN HADES.DBO.HD_SETOR S (NOLOCK) 
				                            ON M.SETOR = S.SETOR
		                            LEFT JOIN (SELECT T.TRANSFERENCIAID, 
                                                        T.SETORDESTINO, 
                                                        T.DATAMOVIMENTACAO, 
                                                        TI.BEMID, 
                                                        TI.TRANSFERENCIAITEMID 
                                                FROM   PATRIMONIO.TRANSFERENCIA T (NOLOCK) 
                                                        INNER JOIN PATRIMONIO.TRANSFERENCIAITEM TI (NOLOCK) 
                                                                ON T.TRANSFERENCIAID = TI.TRANSFERENCIAID 
                                                WHERE  SITUACAO = 'ACEITA' ) T 
                                            ON CONVERT(DATE, T.DATAMOVIMENTACAO) = M.DATAINICIO 
                                                AND T.BEMID = B.BEMID
                                                AND T.SETORDESTINO = M.SETOR
                            WHERE M.DATAINICIO >= @DATAINICIO
	                              AND M.DATAINICIO <= @DATAFIM	");

                if (idRegional == 0)
                {
                    sql.Append(@"
		                                  AND M.SETOR = @SETOR ");
                }

                if (idClassificacao != null && idClassificacao > 0)
                {
                    sql.Append(@"
		                                  AND C.CLASSIFICACAOID = @CLASSIFICACAO ");
                }

                sql.Append(@" 		                            
                            ORDER  BY UNIDADEADMINISTRATIVA, C.CONTA, NUMERO ");

                if (idRegional > 0)
                {
                    sql.Append(@"
                                        DROP TABLE #UASREGIONAL ");
                }

                contextQuery.Command = sql.ToString();

                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, dataInicio.Date);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, dataFim.Date);

                if (idClassificacao != null && idClassificacao > 0)
                {
                    contextQuery.Parameters.Add("@CLASSIFICACAO", SqlDbType.Int, Convert.ToInt32(idClassificacao));
                }

                if (idRegional > 0)
                {
                    contextQuery.Parameters.Add("@REGIONALID", SqlDbType.Int, idRegional);
                }
                else
                {
                    contextQuery.Parameters.Add("@SETOR", SqlDbType.VarChar, setor);
                }

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                if (contexto != null)
                {
                    contexto.Abandon();
                }
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    if (Convert.ToString(ex.Message).Contains("ERRO:"))
                    {
                        mensagem = ex.Message;
                    }
                    else
                    {
                        mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine,
                            Convert.ToString(ex.Message));
                    }
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

        public DataTable ObtemSaidaBensMoveisPor(int idRegional, string setor, int? idClassificacao, int ano, int mesInicio, int mesFim)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            DateTime dataInicio;
            DateTime dataFim;
            StringBuilder sql = new StringBuilder();

            try
            {
                //Monta datas para verificação:
                dataInicio = new DateTime(ano, mesInicio, 1);
                dataFim = new DateTime(ano, mesFim, DateTime.DaysInMonth(ano, mesFim));

                //Valida data
                if (mesInicio > mesFim)
                {
                    throw new Exception("ERRO: O mês inicio deve ser menor ou igual ao mês final.");
                }
                else if (dataFim.Date >= DateTime.Now.Date)
                {
                    throw new Exception(string.Format("ERRO: A consulta deste ano / mês estará disponivel após {0}.", dataFim.ToString("dd/MM/yyyy")));
                }

                //Caso o relatorio seja por unidade limpa informação de regional
                if (!setor.IsNullOrEmptyOrWhiteSpace())
                {
                    idRegional = 0;
                }

                if (idRegional > 0)
                {
                    sql.Append(@"  --Busca de uas da regional
                                        SELECT distinct SETORID
                                        INTO #UASREGIONAL
                                        FROM HADES.DBO.REGIONAL__SETOR (NOLOCK) 
                                        WHERE REGIONALID = @REGIONALID

                                        INSERT INTO #UASREGIONAL
                                        SELECT distinct SETOR AS SETORID
                                        FROM LY_UNIDADE_ENSINO (NOLOCK) 
                                        WHERE ID_REGIONAL = @REGIONALID 
                                        AND SETOR IS NOT NULL
                                ");
                }

                sql.Append(@" SELECT DISTINCT S.NOME AS UNIDADEADMINISTRATIVA, 
				                                    C.CONTA,
				                                    REPLICATE('0',6 - LEN(M.NUMERO)) + CONVERT(VARCHAR(6), M.NUMERO) AS NUMERO,
				                                    B.DESCRICAO,
				                                    'Unid' as UNIDADEMEDIDA,
				                                    1 as QUANTIDADE,
				                                    PATRIMONIO.FN_CALCULOVALORATUALIZADO(B.BEMID, M.DATAFIM, I.DATAINICIO, I.VALOR, CV.TAXAVALORRESIDUAL,CV.VIDAUTIL) AS 	VALORCALCULADO, 
				                                    CASE 
					                                    WHEN B.BAIXA = 0 --Transferencia para outra unidade 
							                                    THEN 'Transferido para ' + (SELECT TOP 1 S2.NOME + ' - ' + S2.SETOR 
									                                    FROM   PATRIMONIO.TRANSFERENCIAITEM I2 (NOLOCK) 
											                                    INNER JOIN PATRIMONIO.TRANSFERENCIA T2 (NOLOCK) 
													                                    ON I2.TRANSFERENCIAID = T2.TRANSFERENCIAID 
											                                    INNER JOIN HADES.DBO.HD_SETOR S2 (NOLOCK) 
													                                    ON S2.SETOR = T2.SETORDESTINO 
									                                    WHERE  I2.BEMID = B.BEMID 
											                                    AND I2.NUMEROBEMORIGEM = M.NUMERO 
											                                    AND T2.SETORORIGEM = M.SETOR )
					                                    WHEN B.BAIXA = 1 and b.MOTIVOBAIXAID = 2 --Subtraido
							                                    THEN 'Processo de Baixa nº: ' + ISNULL(B.PROCESSOBAIXA, 'NÃO INFORMADO') + ' nº do BO:' + b.BOLETIMOCORRENCIA
					                                    WHEN B.BAIXA = 1 and b.MOTIVOBAIXAID = 3 --Transferido para prefeitura ou instituição
							                                    THEN 'Processo de Baixa nº: ' + ISNULL(B.PROCESSOBAIXA, 'NÃO INFORMADO') + ' - ' + b.INSTITUICAODESTINO + ' CNPJ: ' + B.CNPJINSTITUICAODESTINO
					                                    ELSE 'Processo de Baixa nº: ' + ISNULL(B.PROCESSOBAIXA, 'NÃO INFORMADO') --Inservivel
				                                    END OBSERVACAO,
				                                    b.MOTIVOBAIXAID,
				                                    b.CLASSIFICACAOID, 
				                                    M.DATAINICIO, 
				                                    M.DATAFIM, 
				                                    s.SETOR, 					
				                                    B.BEMID 
                                    FROM   PATRIMONIO.MOVIMENTACAO M (NOLOCK) 
		                                    INNER JOIN PATRIMONIO.BEM B (NOLOCK) 
				                                    ON M.BEMID = B.BEMID 
		                                    INNER JOIN PATRIMONIO.BEMVALOR I (NOLOCK) 
			                                    ON B.BEMID = I.BEMID 
				                                    AND I.DATAINICIO <= M.DATAFIM
				                                    AND ( I.DATAFIM IS NULL 
						                                    OR i.DATAFIM >= M.DATAFIM ) 
		                                    INNER JOIN Patrimonio.CLASSIFICACAO C (NOLOCK)
				                                    ON C.CLASSIFICACAOID = B.CLASSIFICACAOID
		                                    INNER JOIN Patrimonio.CLASSIFICACAOVIGENCIA CV (NOLOCK) 
		                                       ON B.CLASSIFICACAOID = CV.CLASSIFICACAOID
			                                    AND CV.DATAINICIO <= M.DATAFIM
			                                    AND  ( CV.DATAFIM IS NULL 
                                                      OR CV.DATAFIM >= M.DATAFIM ) ");

                if (idRegional > 0)
                {
                    sql.Append(@"
                                                INNER JOIN #UASREGIONAL UR 
				                                        ON M.SETOR = UR.SETORID ");
                }

                sql.Append(@"
		                                    INNER JOIN HADES.DBO.HD_SETOR S (NOLOCK) 
				                                    ON M.SETOR = S.SETOR		   
                                    WHERE M.DATAFIM IS NOT NULL
		                                    AND M.DATAFIM <= @DATAFIM
		                                    AND M.DATAFIM >= @DATAINICIO ");

                if (idRegional == 0)
                {
                    sql.Append(@"
		                                  AND M.SETOR = @SETOR ");
                }

                if (idClassificacao != null && idClassificacao > 0)
                {
                    sql.Append(@"
		                                  AND b.CLASSIFICACAOID = @CLASSIFICACAO ");
                }

                sql.Append(@"
		                                    ORDER  BY UNIDADEADMINISTRATIVA, M.DATAFIM  ");

                if (idRegional > 0)
                {
                    sql.Append(@"
                                        DROP TABLE #UASREGIONAL ");
                }

                contextQuery.Command = sql.ToString();

                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, dataFim.Date);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, dataInicio.Date);

                if (idClassificacao != null && idClassificacao > 0)
                {
                    contextQuery.Parameters.Add("@CLASSIFICACAO", SqlDbType.Int, Convert.ToInt32(idClassificacao));
                }

                if (idRegional > 0)
                {
                    contextQuery.Parameters.Add("@REGIONALID", SqlDbType.Int, idRegional);
                }
                else
                {
                    contextQuery.Parameters.Add("@SETOR", SqlDbType.VarChar, setor);
                }

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                if (contexto != null)
                {
                    contexto.Abandon();
                }
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    if (Convert.ToString(ex.Message).Contains("ERRO:"))
                    {
                        mensagem = ex.Message;
                    }
                    else
                    {
                        mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine,
                            Convert.ToString(ex.Message));
                    }
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
    }
}