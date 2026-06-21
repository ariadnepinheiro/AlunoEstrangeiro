using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Patrimonio
{
    public class Bem
    {
        public DataTable ListaPatrimonioAtivoPor(string setor, string conta)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT B.BEMID,     
				                        REPLICATE('0',6 - LEN(Mv.NUMERO)) + CONVERT(VARCHAR(6), Mv.NUMERO) AS NUMERO,
                                        B.DESCRICAO, 
                                        B.CLASSIFICACAOID, 
                                        C.CONTA,
                                        C.DESCRICAO AS CLASSIFICACAO, 
                                        I.ESTADOCONSERVACAOID, 
                                        E.CONCEITO  AS ESTADOCONSERVACAO, 
                                        M.MOEDAID, 
                                        M.SIGLA,
                                        M.SIGLA + CONVERT(VARCHAR(100), Patrimonio.FN_CALCULOVALORATUALIZADO(B.BEMID, GETDATE(), I.DATAINICIO, I.VALOR, CV.TAXAVALORRESIDUAL,CV.VIDAUTIL))  AS VALORCOMSIGLA,
                                        CONVERT(VARCHAR,B.DATAAQUISICAO,103) AS DATAAQUISICAO, 
                                        CONVERT(VARCHAR,MV.DATAINICIO,103) as DATAINCORPORACAO
                        FROM   PATRIMONIO.BEM B 
		                        INNER JOIN PATRIMONIO.MOVIMENTACAO Mv (NOLOCK) 
                                                               ON B.BEMID = Mv.BEMID 
                                                                  AND ( Mv.DATAFIM IS NULL 
                                                                         OR DATAFIM >= GETDATE() ) 
                                INNER JOIN PATRIMONIO.BEMVALOR I (NOLOCK) 
                                        ON B.BEMID = I.BEMID 
                                            AND i.DATAINICIO <= GETDATE()
                                            AND ( i.DATAFIM IS NULL 
                                                    OR i.DATAFIM >= GETDATE() ) 
                                INNER JOIN PATRIMONIO.ESTADOCONSERVACAO E (NOLOCK) 
                                        ON I.ESTADOCONSERVACAOID = E.ESTADOCONSERVACAOID 
                                INNER JOIN PATRIMONIO.CLASSIFICACAO C (NOLOCK) 
                                        ON B.CLASSIFICACAOID = C.CLASSIFICACAOID 
                                INNER JOIN Patrimonio.CLASSIFICACAOVIGENCIA CV (NOLOCK) 
                                        ON B.CLASSIFICACAOID = CV.CLASSIFICACAOID
	                                    AND CV.DATAINICIO <= GETDATE()
	                                    AND  ( CV.DATAFIM IS NULL 
                                              OR CV.DATAFIM >= GETDATE() ) 
                                INNER JOIN PATRIMONIO.MOEDA M (NOLOCK) 
                                        ON I.MOEDAID = M.MOEDAID 
                                INNER JOIN HADES..VW_SETOR S ON S.SETOR = MV.SETOR
                        WHERE  S.UA_ATUAL = @SETOR  
                                AND C.CONTA = @CONTA 
                                AND B.BAIXA = 0
                        ORDER  BY B.DESCRICAO ";

                contextQuery.Parameters.Add("@SETOR", SqlDbType.VarChar, setor);
                contextQuery.Parameters.Add("@CONTA", SqlDbType.VarChar, conta);

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

        public DataTable ListaPor(string setor, string classificacao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(@" SELECT DISTINCT B.BEMID,    
				                                    REPLICATE('0',6 - LEN(MV.NUMERO)) + CONVERT(VARCHAR(6), MV.NUMERO) AS NUMERO,
                                                    B.DESCRICAO, 
                                                    B.CLASSIFICACAOID, 
                                                    C.CONTA,
                                                    C.DESCRICAO AS CLASSIFICACAO, 
                                                    I.ESTADOCONSERVACAOID, 
                                                    E.CONCEITO  AS ESTADOCONSERVACAO, 
                                                    M.MOEDAID,
                                                    I.VALOR AS ULTIMOVALORBEM,  
                                                    M.SIGLA,    
				                                    B.BAIXA,
				                                    I.DATAINICIO AS DATAULTIMOVALOR,
				                                    I.VALOR AS ULTIMOVALOR,
				                                    I.VIDAUTIL,
                                                    b.DATABAIXA AS DATADABAIXA,                                    
                                                    CONVERT(VARCHAR, B.DATAAQUISICAO, 103) AS DATAAQUISICAO, 
                                                    CONVERT(VARCHAR, MV.DATAINICIO, 103) AS DATAINCORPORACAO,
                                                    CV.TAXAVALORRESIDUAL,
	                                                CONVERT(BIT,0) AS PRECISAREAVALIAR,
													CONVERT(DECIMAL(10,2) ,NULL) AS VALORATUALIZADO
                                    INTO #BENS
                                    FROM   PATRIMONIO.BEM B 
                                            INNER JOIN PATRIMONIO.BEMVALOR I (NOLOCK) 
                                                    ON B.BEMID = I.BEMID 
                                                        AND I.DATAINICIO  <= ISNULL(B.DATABAIXA, GETDATE())
					                                    AND ( I.DATAFIM IS NULL 
                                                                OR I.DATAFIM >= ISNULL(B.DATABAIXA, GETDATE()) ) 
			                                    INNER JOIN PATRIMONIO.MOVIMENTACAO MV (NOLOCK) 
                                                    ON B.BEMID = MV.BEMID 
                                                        AND MV.DATAINICIO  <=  ISNULL(B.DATABAIXA, GETDATE())
					                                    AND ( MV.DATAFIM IS NULL 
                                                                OR MV.DATAFIM >= ISNULL(B.DATABAIXA, GETDATE()) ) 
                                            INNER JOIN PATRIMONIO.ESTADOCONSERVACAO E (NOLOCK) 
                                                    ON I.ESTADOCONSERVACAOID = E.ESTADOCONSERVACAOID 
                                            INNER JOIN PATRIMONIO.CLASSIFICACAO C (NOLOCK) 
                                                    ON B.CLASSIFICACAOID = C.CLASSIFICACAOID 
                                            INNER JOIN PATRIMONIO.MOEDA M (NOLOCK) 
                                                    ON I.MOEDAID = M.MOEDAID 
                                            INNER JOIN Patrimonio.CLASSIFICACAOVIGENCIA CV (NOLOCK) 
				                                    ON B.CLASSIFICACAOID = CV.CLASSIFICACAOID 
                                                        AND CV.DATAINICIO <= GETDATE() AND  ( CV.DATAFIM IS NULL 
                                                        OR CV.DATAFIM >= GETDATE() )
                                            INNER JOIN HADES..VW_SETOR S ON S.SETOR = MV.SETOR
                                    WHERE  S.UA_ATUAL = @SETOR ");

                if (!classificacao.IsNullOrEmptyOrWhiteSpace())
                {
                    sql.Append(@" AND C.CONTA = @CLASSIFICACAO ");
                }

                sql.Append(@"

                                   UPDATE #BENS SET 
									VALORATUALIZADO = 0
						           WHERE VIDAUTIL = 0

							     UPDATE #BENS SET 
								    VALORATUALIZADO = ULTIMOVALOR
						         WHERE VALORATUALIZADO IS NULL AND DATEDIFF(MONTH,DATAULTIMOVALOR, ISNULL(DATADABAIXA, GETDATE())) = 0
								  
								  
							      UPDATE #BENS SET 
								    VALORATUALIZADO = Patrimonio.FN_CALCULOVALORATUALIZADO(BEMID, ISNULL(DATADABAIXA, GETDATE()), DATAULTIMOVALOR, ULTIMOVALOR, TAXAVALORRESIDUAL,VIDAUTIL) 
						         WHERE VALORATUALIZADO IS NULL
								 
									UPDATE #BENS SET
									PRECISAREAVALIAR = 1
									WHERE BAIXA = 0 AND (YEAR(GETDATE()) - YEAR(DATAULTIMOVALOR)) >= VIDAUTIL

									UPDATE #BENS SET
									PRECISAREAVALIAR = 1
									WHERE BAIXA = 0 AND VALORATUALIZADO <= ((ULTIMOVALORBEM/100) * TAXAVALORRESIDUAL)

                                    SELECT CONVERT(VARCHAR, DATADABAIXA, 103) AS DATABAIXA,* FROM     #BENS  
                                    ORDER BY NUMERO                              

                                    DROP TABLE #BENS ");

                contextQuery.Command = sql.ToString();

                contextQuery.Parameters.Add("@SETOR", SqlDbType.VarChar, setor);


                if (!classificacao.IsNullOrEmptyOrWhiteSpace())
                {
                    contextQuery.Parameters.Add("@CLASSIFICACAO", SqlDbType.VarChar, classificacao);
                }

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

        public DataTable ObtemControleMensalGestorPor(int ano, int mes)
        {
            DataTable dt = null;
            dt = this.ObtemControleMensalGestorInternoPor(ano, mes, 0);
            return dt;
        }

        public DataTable ObtemControleMensalGestorPor(int ano, int mes, int idRegional)
        {
            DataTable dt = null;
            dt = this.ObtemControleMensalGestorInternoPor(ano, mes, idRegional);
            return dt;
        }

        private DataTable ObtemControleMensalGestorInternoPor(int ano, int mes, int idRegional)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            StringBuilder sql = new StringBuilder();
            DataTable dt = null;
            DateTime dataConsulta;

            try
            {
                //Busca Ultimo dia do mes para calculo
                dataConsulta = new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes));

                //Valida data                
                if (dataConsulta.Date >= DateTime.Now.Date)
                {
                    throw new Exception(string.Format("ERRO: A consulta deste ano / mês estará disponivel após {0}.", dataConsulta.ToString("dd/MM/yyyy")));
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

                sql.Append(@"         SELECT DISTINCT MV.SETOR,
				                                        I.MOEDAID,
				                                        B.BEMID,
				                                        B.CLASSIFICACAOID,
				                                        CONVERT(DATE, I.DATAINICIO) AS DATAAQUISICAOREAVALIACAO,
				                                        CONVERT(DECIMAL(10, 2), I.VALOR) AS VALOR,
				                                        CONVERT(DECIMAL(10, 2), CV.TAXAVALORRESIDUAL) AS TAXAVALORRESIDUAL,
                                                        CV.VIDAUTIL
                                        INTO #SETOR
                                        FROM   PATRIMONIO.BEM B	
                                                INNER JOIN PATRIMONIO.MOVIMENTACAO Mv (NOLOCK) 
                                                        ON B.BEMID = Mv.BEMID 
                                                            AND MV.DATAINICIO <= @DATACONSULTA
                                                            AND ( MV.DATAFIM IS NULL 
                 
                                                   OR MV.DATAFIM >= @DATACONSULTA ) ");

                if (idRegional > 0)
                {
                    sql.Append(@"
                                                INNER JOIN #UASREGIONAL UR 
				                                        ON MV.SETOR = UR.SETORID");
                }

                sql.Append(@"
		                                        INNER JOIN PATRIMONIO.BEMVALOR I (NOLOCK) 
                                                        ON B.BEMID = I.BEMID 
                                                            AND I.DATAINICIO <= @DATACONSULTA
                                                            AND ( I.DATAFIM IS NULL 
                                                                    OR i.DATAFIM >= @DATACONSULTA ) 
                                                INNER JOIN Patrimonio.CLASSIFICACAOVIGENCIA CV (NOLOCK) 
		                                            ON B.CLASSIFICACAOID = CV.CLASSIFICACAOID
			                                            AND CV.DATAINICIO <= @DATACONSULTA
			                                            AND  ( CV.DATAFIM IS NULL 
                                                              OR CV.DATAFIM >= @DATACONSULTA )
                                        WHERE  B.DATABAIXA IS NULL 
	                                            OR B.DATABAIXA > @DATACONSULTA -- que estava ativo na data da consulta

                                        SELECT SETOR,
                                               Patrimonio.FN_CALCULOVALORATUALIZADO(BEMID, @DATACONSULTA, DATAAQUISICAOREAVALIACAO, VALOR, TAXAVALORRESIDUAL,VIDAUTIL) AS VALORCALCULADO,
	                                           MOEDAID,
	                                           BEMID,
	                                           CLASSIFICACAOID
                                        INTO #SETORVALOR
                                        from #SETOR

                                        SELECT V.SETOR, 		
		                                        S.NOME AS UNIDADEADMINISTRATIVA,
		                                        LA.MATRICULA,
		                                        LA.NOME_COMPL as AGENTERESPONSAVEL,
		                                        M.SIGLA + CONVERT(VARCHAR(100), SUM(V.VALORCALCULADO)) AS TOTALCALCULADOCOMSIGLA
                                        FROM #SETORVALOR V
	                                        INNER JOIN PATRIMONIO.MOEDA M (NOLOCK) 
                                                        ON V.MOEDAID = M.MOEDAID 
	                                        INNER JOIN HADES.DBO.HD_SETOR S (NOLOCK) 
				                                        ON S.SETOR = V.SETOR
	                                        LEFT JOIN ( SELECT DISTINCT AG.MATRICULA, P.NOME_COMPL, AG.SETOR
			                                        FROM  PATRIMONIO.AGENTERESPONSAVEL AG (NOLOCK)			
				                                        INNER JOIN LY_LOTACAO L (NOLOCK)
							                                        ON L.MATRICULA = AG.MATRICULA
				                                        INNER JOIN LY_PESSOA P (NOLOCK)
							                                        ON L.PESSOA = P.PESSOA
				                                        WHERE 	AG.DATANOMEACAO <= @DATACONSULTA
							                                        AND ( AG.DATADISPENSA IS NULL 
										                                        OR AG.DATADISPENSA >= @DATACONSULTA ) ) LA
							                                        ON  LA.SETOR = V.SETOR
                                        GROUP BY V.SETOR, M.SIGLA, S.NOME, LA.MATRICULA, LA.NOME_COMPL
                                        ORDER BY S.NOME

                                        DROP TABLE #SETORVALOR 
                                        DROP TABLE #SETOR ");

                if (idRegional > 0)
                {
                    sql.Append(@"
                                        DROP TABLE #UASREGIONAL ");
                }

                contexto.CommandTimeout = 600; //10 min
                contextQuery.Command = sql.ToString();

                contextQuery.Parameters.Add("@DATACONSULTA", SqlDbType.Date, dataConsulta.Date);

                if (idRegional > 0)
                {
                    contextQuery.Parameters.Add("@REGIONALID", SqlDbType.Int, idRegional);
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

        public DataTable ObtemInventarioExistenciasFisicasPor(int idRegional, string setor, int? idClassificacao, int ano)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            DateTime dataConsulta;
            StringBuilder sql = new StringBuilder();

            try
            {
                //Monta data da consulta no ultimo dia do ano
                dataConsulta = new DateTime(ano, 12, 31);

                //Valida data
                if (ano >= DateTime.Now.Year)
                {
                    throw new Exception("ERRO: O ano da consulta não pode ser maior ou igual ao ano atual.");
                }

                //Valida data                
                if (dataConsulta.Date >= DateTime.Now.Date)
                {
                    throw new Exception(string.Format("ERRO: A consulta deste ano estará disponivel após {0}.", dataConsulta.ToString("dd/MM/yyyy")));
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

                sql.Append(@" SELECT DISTINCT SE.NOME AS UNIDADEADMINISTRATIVA, 
                                                        C.CONTA, 
                                                        REPLICATE('0', 6 - LEN(MV.NUMERO)) + CONVERT(VARCHAR(6), MV.NUMERO) AS NUMERO, 
                                                        B.DESCRICAO, 
                                                        'Unid' AS UNIDADEMEDIDA, 
                                                        1 AS QUANTIDADE, 
                                                        PATRIMONIO.FN_CALCULOVALORATUALIZADO(B.BEMID, @DATACONSULTA, I.DATAINICIO, I.VALOR, CV.TAXAVALORRESIDUAL,CV.VIDAUTIL) AS VALORCALCULADO, 
				                                        CASE 
					                                        WHEN T.TRANSFERENCIAITEMID IS NOT NULL
						                                        THEN 'ATBM: ' + CONVERT(VARCHAR(20), T.TRANSFERENCIAID) + '/' + CONVERT(VARCHAR(20), YEAR(T.DATAMOVIMENTACAO))
					                                        ELSE  B.DOCUMENTOHABIL
				                                        END OBSERVACAO, 
                                                        MV.SETOR, 
                                                        C.CLASSIFICACAOID, 
                                                        B.BEMID, 
                                                        MV.DATAINICIO, 
                                                        MV.DATAFIM 
                                        FROM   PATRIMONIO.BEM B 
		                                        INNER JOIN Patrimonio.CLASSIFICACAOVIGENCIA CV (NOLOCK) 
		                                               ON B.CLASSIFICACAOID = CV.CLASSIFICACAOID
			                                            AND CV.DATAINICIO <= @DATACONSULTA
			                                            AND  ( CV.DATAFIM IS NULL 
                                                              OR CV.DATAFIM >= @DATACONSULTA ) 
                                                INNER JOIN PATRIMONIO.MOVIMENTACAO MV (NOLOCK) 
                                                        ON B.BEMID = MV.BEMID 
                                                            AND MV.DATAINICIO <= @DATACONSULTA 
                                                            AND ( MV.DATAFIM IS NULL 
                                                                    OR MV.DATAFIM >= @DATACONSULTA ) ");

                if (idRegional > 0)
                {
                    sql.Append(@"
                                                INNER JOIN #UASREGIONAL UR 
				                                        ON MV.SETOR = UR.SETORID ");
                }

                sql.Append(@" 		                            
                            
                                                INNER JOIN HADES.DBO.HD_SETOR SE (NOLOCK) 
                                                        ON SE.SETOR = MV.SETOR 
                                                INNER JOIN PATRIMONIO.BEMVALOR I (NOLOCK) 
                                                        ON B.BEMID = I.BEMID 
                                                            AND I.DATAINICIO <= @DATACONSULTA 
                                                            AND ( I.DATAFIM IS NULL 
                                                                    OR I.DATAFIM >= @DATACONSULTA ) 
                                                INNER JOIN PATRIMONIO.CLASSIFICACAO C (NOLOCK) 
                                                        ON B.CLASSIFICACAOID = C.CLASSIFICACAOID 
		                                        LEFT JOIN (SELECT T.TRANSFERENCIAID, 
                                                                     T.SETORDESTINO, 
                                                                     T.DATAMOVIMENTACAO, 
                                                                     TI.BEMID, 
                                                                     TI.TRANSFERENCIAITEMID 
                                                              FROM   PATRIMONIO.TRANSFERENCIA T (NOLOCK) 
                                                                     INNER JOIN PATRIMONIO.TRANSFERENCIAITEM TI (NOLOCK) 
                                                                             ON T.TRANSFERENCIAID = TI.TRANSFERENCIAID 
                                                              WHERE  SITUACAO = 'ACEITA' ) T 
                                                          ON CONVERT(DATE, T.DATAMOVIMENTACAO) = MV.DATAINICIO 
                                                             AND T.BEMID = B.BEMID 
                                                             AND T.SETORDESTINO = MV.SETOR
                                        WHERE  ( B.DATABAIXA IS NULL 
                                                        OR B.DATABAIXA > @DATACONSULTA ) 
                                        ");

                if (idRegional == 0)
                {
                    sql.Append(@"
		                                  AND MV.SETOR = @SETOR ");
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

                contextQuery.Parameters.Add("@DATACONSULTA", SqlDbType.Date, dataConsulta.Date);

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

        public decimal ObtemValorAtualizadoPor(DataContext contexto, int bemId, DateTime dataPesquisa, DateTime dataAquisicaoReavaliacao, decimal valor, decimal taxaValorResidual, int vidaUtil)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            decimal retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT PATRIMONIO.FN_CALCULOVALORATUALIZADO(@BEMID, @DATAVERIFICACAO, @DATAAQUISICAOREAVALIACAO, @VALOR, @TAXAVALORRESIDUAL, @VIDAUTIL) AS VALORATUALIZADO ";

                contextQuery.Parameters.Add("@BEMID", SqlDbType.Int, bemId);
                contextQuery.Parameters.Add("@DATAVERIFICACAO", SqlDbType.DateTime, dataPesquisa);
                contextQuery.Parameters.Add("@DATAAQUISICAOREAVALIACAO", SqlDbType.DateTime, dataAquisicaoReavaliacao);
                contextQuery.Parameters.Add("@VALOR", valor);
                contextQuery.Parameters.Add("@TAXAVALORRESIDUAL", taxaValorResidual);
                contextQuery.Parameters.Add("@VIDAUTIL", vidaUtil);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToDecimal(reader["VALORATUALIZADO"]);
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

        public DTOs.DadosFichaIndividualBem ObtemFichaIndividualBem(string setor, string numero, string setorComplementar)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            DTOs.DadosFichaIndividualBem dados = new Techne.Lyceum.RN.DTOs.DadosFichaIndividualBem();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;

            try
            {
                contextQuery.Command = @" SELECT TOP 1 B.BEMID, 
                                         B.DESCRICAO, 
                                         M.NUMERO, 
                                         B.DATAAQUISICAO, 
				                         M.DATAINICIO AS DATAINCORPORACAO, 
                                         B.DOCUMENTOHABIL, 
                                         B.HISTORICO, 
                                         C.CONTA, 
                                         O.DESCRICAO AS OPERACAO, 
                                         M.SETOR, 
                                         S.NOME AS UNIDADEADMINISTRATIVA, 
                                         UE.ENDERECO, 
                                         UE.FONE, 
                                         UE.E_MAIL,
			                             V.VALOR,
			                             MO.SIGLA
                            FROM   PATRIMONIO.MOVIMENTACAO M (NOLOCK) 
                                   INNER JOIN PATRIMONIO.BEM B (NOLOCK) 
                                           ON M.BEMID = B.BEMID 
                                   INNER JOIN PATRIMONIO.CLASSIFICACAO C (NOLOCK) 
                                           ON B.CLASSIFICACAOID = C.CLASSIFICACAOID 
                                   INNER JOIN PATRIMONIO.OPERACAO O (NOLOCK) 
                                           ON B.OPERACAOID = O.OPERACAOID 
                                   INNER JOIN HADES.DBO.HD_SETOR S (NOLOCK) 
                                           ON M.SETOR = S.SETOR 
                                   INNER JOIN PATRIMONIO.BEMVALOR V (NOLOCK) 
                                           ON V.BEMID = B.BEMID 
	                               INNER JOIN PATRIMONIO.MOEDA MO (NOLOCK) 
			                               ON V.MOEDAID = MO.MOEDAID
                                   LEFT JOIN LY_UNIDADE_ENSINO UE (NOLOCK) 
                                          ON M.SETOR = UE.SETOR
                            WHERE  M.SETOR = @SETOR OR  M.SETOR = @SETORCOMPLEMENTAR
                                   AND M.NUMERO = @NUMERO 
                            ORDER  BY V.DATAINICIO ASC ";

                contextQuery.Parameters.Add("@SETOR", SqlDbType.VarChar, setor);
                contextQuery.Parameters.Add("@SETORCOMPLEMENTAR", SqlDbType.VarChar, setorComplementar);
                contextQuery.Parameters.Add("@NUMERO", SqlDbType.Int, Convert.ToInt32(numero));

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dados.BemId = Convert.ToInt32(reader["BEMID"]);
                    dados.Descricao = Convert.ToString(reader["DESCRICAO"]);
                    dados.Numero = Convert.ToString(reader["NUMERO"]).PadLeft(6, '0'); ;
                    dados.DataIncorporacao = Convert.ToDateTime(reader["DATAINCORPORACAO"]).ToString("dd/MM/yyyy");
                    dados.DocumentoHabil = Convert.ToString(reader["DOCUMENTOHABIL"]);
                    dados.Historico = Convert.ToString(reader["HISTORICO"]);
                    dados.Conta = Convert.ToString(reader["CONTA"]);
                    dados.Operacao = Convert.ToString(reader["OPERACAO"]);
                    dados.UnidadeAdministrativa = Convert.ToString(reader["UNIDADEADMINISTRATIVA"]);
                    dados.EnderecoUnidadeAdministrativa = Convert.ToString(reader["ENDERECO"]);
                    dados.EmailUnidadeAdministrativa = Convert.ToString(reader["E_MAIL"]);
                    dados.TelefoneUnidadeAdministrativa = Convert.ToString(reader["FONE"]);
                    dados.ValorInicial = Convert.ToDecimal(reader["VALOR"]);
                    dados.Sigla = Convert.ToString(reader["SIGLA"]);
                }

                return dados;
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

        public DTOs.DadosBemPatrimonial ObtemDadosBemPatrimonialPor(int bemId)
        {
            RN.Patrimonio.BemValor rnBemValor = new BemValor();
            RN.Patrimonio.Classificacao rnClassificacao = new Classificacao();
            RN.Patrimonio.Movimentacao rnMovimentacao = new Movimentacao();
            DTOs.DadosBemPatrimonial dados = new DTOs.DadosBemPatrimonial();
            Entidades.Bem bem = new Techne.Lyceum.RN.Patrimonio.Entidades.Bem();
            Entidades.BemValor bemValor = new Techne.Lyceum.RN.Patrimonio.Entidades.BemValor();
            Entidades.Movimentacao movimentacao = new Techne.Lyceum.RN.Patrimonio.Entidades.Movimentacao();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                bem = this.ObtemPor(contexto, bemId);

                //Verifica se o bem ja possui baixa
                if (bem.Baixa)
                {
                    //Busca valor vigente do bem na data da baixa
                    bemValor = rnBemValor.ObtemBemValorVigentePor(contexto, bemId, Convert.ToDateTime(bem.DataBaixa));

                    //Busca taxa valor residual
                    decimal taxaValorResidual = rnClassificacao.RetornaValorResidualVigentePor(contexto, bem.ClassificacaoId, Convert.ToDateTime(bem.DataBaixa));

                    //Busca movimentacao vigente do bem na data da baixa
                    movimentacao = rnMovimentacao.ObtemMovimentacaoVigentePor(contexto, bemId, Convert.ToDateTime(bem.DataBaixa));

                    //Busca valor atualizado na data da baixa
                    dados.ValorAtualizado = this.ObtemValorAtualizadoPor(contexto, bemId, Convert.ToDateTime(bem.DataBaixa), bemValor.DataInicio, bemValor.Valor, taxaValorResidual, bemValor.VidaUtil);
                }
                else
                {
                    //Busca valor vigente do bem
                    bemValor = rnBemValor.ObtemBemValorVigentePor(contexto, bemId, DateTime.Now);

                    //Busca taxa valor residual
                    decimal taxaValorResidual = rnClassificacao.RetornaValorResidualVigentePor(contexto, bem.ClassificacaoId, DateTime.Now);

                    //Busca movimentacao vigente do bem
                    movimentacao = rnMovimentacao.ObtemMovimentacaoVigentePor(contexto, bemId, DateTime.Now);

                    //Busca valor atualizado ate hoje
                    dados.ValorAtualizado = this.ObtemValorAtualizadoPor(contexto, bemId, DateTime.Now, bemValor.DataInicio, bemValor.Valor, taxaValorResidual, bemValor.VidaUtil);
                }

                //Monta dados para tela
                dados.BemId = bemId;
                dados.OperacaoId = bem.OperacaoId;
                dados.ClassificacaoId = bem.ClassificacaoId;
                dados.Descricao = bem.Descricao;
                dados.Setor = movimentacao.Setor;
                dados.DataAquisicao = bem.DataAquisicao;
                dados.DataIncorporacao = movimentacao.DataInicio;
                dados.DocumentoHabil = bem.DocumentoHabil;
                dados.ValorMercado = bem.ValorMercado;
                dados.VidaUtilizada = bem.PeriodoVidaUtilizado;
                dados.Historico = bem.Historico;
                dados.MoedaId = bemValor.MoedaId;
                dados.EstadoconservacaoId = bemValor.EstadoconservacaoId;
                dados.UlimoValor = bemValor.Valor;
                dados.VidaFutura = bemValor.VidaUtil;
                dados.Baixa = bem.Baixa;
                dados.MotivoBaixaId = bem.MotivoBaixaId;
                dados.DataBaixa = bem.DataBaixa;
                dados.ProcessoBaixa = bem.ProcessoBaixa;
                dados.JustificativaBaixa = bem.JustificativaBaixa;
                dados.BoletimOcorrencia = bem.BoletimOcorrencia;
                dados.InstituicaoDestino = bem.InstituicaoDestino;
                dados.CnpjInstituicaoDestino = bem.CnpjInstituicaoDestino;
                dados.Numero = movimentacao.Numero;

                return dados;
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

        public Entidades.Bem ObtemPor(DataContext contexto, int bemId)
        {
            Entidades.Bem bem = new Entidades.Bem();
            ContextQuery contextQuery = new ContextQuery();


            contextQuery.Command = @" SELECT * 
                                        FROM Patrimonio.BEM (NOLOCK) 
                                        WHERE  BEMID = @BEMID ";

            contextQuery.Parameters.Add("@BEMID", SqlDbType.Int, bemId);

            bem = contexto.TryToBindEntity<Entidades.Bem>(contextQuery);

            return bem;
        }

        public string[] ObtemNumeroDescricaoPor(DataContext contexto, int bemId)
        {
            string[] bem = new string[2];
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT NUMERO, DESCRICAO
                                        FROM PATRIMONIO.BEM B (NOLOCK) 
		                                        INNER JOIN PATRIMONIO.MOVIMENTACAO MV (NOLOCK) 
                                        ON B.BEMID = MV.BEMID 
                                            AND ( MV.DATAFIM IS NULL 
                                                    OR DATAFIM >= GETDATE() ) 
                                        WHERE  b.BEMID = @BEMID ";

                contextQuery.Parameters.Add("@BEMID", SqlDbType.Int, bemId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    bem[0] = Convert.ToString(reader["NUMERO"]).PadLeft(6, '0');
                    bem[1] = Convert.ToString(reader["DESCRICAO"]);
                }

                return bem;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public bool PossuiMotivoBaixaPor(DataContext contexto, int motidoBaixaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Patrimonio.BEM (NOLOCK)
                                    WHERE MOTIVOBAIXAID = @MOTIVOBAIXAID ";

            contextQuery.Parameters.Add("@MOTIVOBAIXAID", SqlDbType.Int, motidoBaixaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiOperacaoPor(DataContext contexto, int operacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Patrimonio.BEM (NOLOCK)
                                    WHERE OPERACAOID = @OPERACAOID ";

            contextQuery.Parameters.Add("@OPERACAOID", SqlDbType.Int, operacaoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public ValidacaoDados Valida(DTOs.DadosCadastroBemPatrimonial dadosBem, bool cadastro)
        {
            RN.Patrimonio.PeriodoLancamento rnPeriodoLancamento = new PeriodoLancamento();
            RN.Patrimonio.Movimentacao rnMovimentacao = new Movimentacao();
            RN.Perfil rnPerfil = new Perfil();
            TransferenciaItem rnTransferenciaItem = new TransferenciaItem();
            DataContext contexto = null;
            List<string> mensagens = new List<string>();
            bool itemUsado = false;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosBem == null)
            {
                return validacaoDados;
            }

            if (!cadastro)
            {
                if (dadosBem.BemId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }

                if (dadosBem.Numero.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo NÚMERO DO BEM é obrigatório.");
                }
            }

            //Verfica se é um cadastro de item usado
            if (dadosBem.OperacaoId == (int)RN.Patrimonio.Operacao.EnumOperacao.IncorporacaoSemRegistroAnterior || dadosBem.OperacaoId == (int)RN.Patrimonio.Operacao.EnumOperacao.DoacaoItemUsado)
            {
                itemUsado = true;

                //Limpa dados que são apenas para itens novos
                dadosBem.DataAquisicao = DateTime.MinValue;
                dadosBem.VidaUtil = null;
                dadosBem.Valor = 0;
            }
            else
            {
                itemUsado = false;

                //Limpa dados que são apenas para itens usados
                dadosBem.EstadoconservacaoId = 0;
                dadosBem.VidaUtilizada = null;
                dadosBem.VidaFutura = null;
                dadosBem.ValorMercado = null;
                dadosBem.Historico = null;
            }

            //Verifica quantidade de itens
            if (dadosBem.Quantidade <= 0)
            {
                mensagens.Add("Campo QUANTIDADE é obrigatório.");
            }

            if (dadosBem.Setor.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE ADMINISTRATIVA é obrigatório.");
            }

            if (dadosBem.OperacaoId <= 0)
            {
                mensagens.Add("Campo OPERAÇÃO é obrigatório.");
            }

            if (dadosBem.ClassificacaoId <= 0)
            {
                mensagens.Add("Campo CLASSIFICAÇÃO é obrigatório.");
            }
            else
            {
                if (dadosBem.ClassificacaoId == 37) //“1.2.3.1.1.01.98      BENS OBSOLETOS OU IMPRESTÁVEIS” 
                {
                    mensagens.Add("Esta CLASSIFICAÇÃO não pode ser utilizada para aquisição/incorporação de bens.");
                }
            }

            if (dadosBem.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
            }
            else
            {
                if (dadosBem.Descricao.Length > 1000)
                {
                    mensagens.Add("O campo DESCRIÇÃO deve ter no máximo de 1000 caracteres.");
                }
            }

            //Verifica se é um item usado
            if (itemUsado)
            {
                if (dadosBem.EstadoconservacaoId <= 0)
                {
                    mensagens.Add("Campo ESTADO DE CONSERVAÇÃO é obrigatório.");
                }
            }
            else
            {
                //Se for item novo
                if (dadosBem.DataAquisicao <= DateTime.MinValue)
                {
                    mensagens.Add("Campo DATA DE AQUISIÇÃO é obrigatório.");
                }
                else
                {
                    if (dadosBem.DataAquisicao.Date > DateTime.Now.Date)
                    {
                        mensagens.Add("A DATA DE AQUISIÇÃO não pode ser maior que a data atual.");
                    }
                }
            }

            if (dadosBem.DataIncorporacao <= DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DE INCORPORAÇÃO é obrigatório.");
            }
            else
            {
                //Se for item novo
                if (dadosBem.DataIncorporacao.Date > DateTime.Now.Date)
                {
                    mensagens.Add("A DATA DE INCORPORAÇÃO não pode ser maior que a data atual.");
                }

                //if (dadosBem.DataIncorporacao.Year != DateTime.Now.Year)
                //{
                //    mensagens.Add("A DATA DE INCORPORAÇÃO deve estar no ano corrente.");
                //}

                ////TROCA DA REGRA PARA INCLUSAO DE PATRIMONIOS DE ANOS ANTERIORES - Regra retirada a pedido do chamado 13249
                //if (dadosBem.DataIncorporacao.Year < (DateTime.Now.Year - 2))
                //{
                //    mensagens.Add(string.Format("A DATA DE INCORPORAÇÃO deve ser maior ou igual a {0}.", Convert.ToString(DateTime.Now.Year - 2)));
                //}

                if (dadosBem.DataAquisicao > DateTime.MinValue)
                {
                    if (dadosBem.DataAquisicao.Date > dadosBem.DataIncorporacao.Date)
                    {
                        mensagens.Add("A DATA DE AQUISIÇÃO não pode ser maior que a DATA DE INCORPORAÇÃO.");
                    }
                }
            }

            //Verifica se é um item usado
            if (itemUsado)
            {
                if (dadosBem.VidaUtilizada <= 0)
                {
                    mensagens.Add("Campo PERÍODO DE VIDA ÚTIL JÁ UTILIZADA é obrigatório.");
                }

                if (dadosBem.VidaFutura <= 0)
                {
                    mensagens.Add("Campo PERÍODO DE UTILIZAÇÃO FUTURA é obrigatório.");
                }

                if (dadosBem.ValorMercado <= 0)
                {
                    mensagens.Add("Campo VALOR DE MERCADO é obrigatório.");
                }
            }
            else
            {
                if (dadosBem.VidaUtil <= 0)
                {
                    mensagens.Add("Campo PERÍODO DE VIDA ÚTIL é obrigatório.");
                }

                if (dadosBem.Valor <= 0)
                {
                    mensagens.Add("Campo VALOR é obrigatório, e não pode ser negativo.");
                }
            }

            if (cadastro && dadosBem.MoedaId <= 0)
            {
                mensagens.Add("Campo MOEDA é obrigatório.");
            }

            if (dadosBem.DocumentoHabil.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DOCUMENTO HÁBIL é obrigatório.");
            }
            else
            {
                if (dadosBem.DocumentoHabil.Length > 500)
                {
                    mensagens.Add("O campo DOCUMENTO HÁBIL deve ter no máximo de 500 caracteres.");
                }
            }

            if (!dadosBem.Historico.IsNullOrEmptyOrWhiteSpace())
            {
                if (dadosBem.Historico.Length > 500)
                {
                    mensagens.Add("O campo HISTÓRICO deve ter no máximo de 500 caracteres.");
                }
            }

            if (dadosBem.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO RESPONSÁVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (!rnPerfil.PossuiPerfilLiberacaoPatrimonioFinalizadoPor(contexto, dadosBem.UsuarioId))
                    {
                        if (!rnPeriodoLancamento.PossuiPeriodoLancamentoAbertoPor(contexto, dadosBem.DataIncorporacao.Year, DateTime.Now.Date))
                        {
                            mensagens.Add("A DATA DE INCORPORAÇÃO está fora do intervalo permitido para lançamento.");
                        }
                    }

                    if (!cadastro)
                    {                       
                        //Verifica se possui transferencia pendente ou aceita
                        if (rnTransferenciaItem.PossuiTransferenciaPendenteAbertaPor(contexto, dadosBem.BemId))
                        {
                            mensagens.Add("Este bem não pode ser alterado pois possui transferência pendente ou aceita.");
                        }
                       
                        //Verifica se a unidade é a dona do bem
                        string setorAtualBem = rnMovimentacao.ObtemSetorVigentePor(contexto, dadosBem.BemId, DateTime.Now);
                        if (setorAtualBem != dadosBem.Setor)
                        {
                            mensagens.Add("Este bem não pode ser alterado pois não pertence a esta Unidade.");
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

        public void Insere(DTOs.DadosCadastroBemPatrimonial dadosBem)
        {
            RN.Patrimonio.Movimentacao rnMovimentacao = new Movimentacao();
            RN.Patrimonio.Reavaliacao rnReavaliacao = new Reavaliacao();
            RN.Patrimonio.EstadoConservacao rnEstadoConservacao = new EstadoConservacao();
            RN.Patrimonio.BemValor rnBemValor = new BemValor();
            RN.Patrimonio.Entidades.Bem bem = new Techne.Lyceum.RN.Patrimonio.Entidades.Bem();
            RN.Patrimonio.Entidades.BemValor bemValor = new Techne.Lyceum.RN.Patrimonio.Entidades.BemValor();
            RN.Patrimonio.Entidades.Movimentacao movimentacao = new Techne.Lyceum.RN.Patrimonio.Entidades.Movimentacao();
            int proximoNumero = 0;
            int quantidadeItens = 0;
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Atualiza quantidade de itens que serão criados
                quantidadeItens = dadosBem.Quantidade;

                //Monta dados Especificos para item udaso
                if (dadosBem.OperacaoId == (int)RN.Patrimonio.Operacao.EnumOperacao.IncorporacaoSemRegistroAnterior || dadosBem.OperacaoId == (int)RN.Patrimonio.Operacao.EnumOperacao.DoacaoItemUsado)
                {
                    //Alimenta Campos
                    dadosBem.DataAquisicao = dadosBem.DataIncorporacao;
                    dadosBem.VidaUtil = dadosBem.VidaFutura;
                    dadosBem.Valor = rnReavaliacao.ObtemValorReavaliadoPor(contexto, dadosBem.EstadoconservacaoId, Convert.ToInt32(dadosBem.VidaUtilizada), Convert.ToInt32(dadosBem.VidaFutura), Convert.ToDecimal(dadosBem.ValorMercado), dadosBem.ClassificacaoId);
                }
                else
                {
                    //Busca dados Especificos para item novo
                    dadosBem.EstadoconservacaoId = rnEstadoConservacao.ObtemMelhorEstadoConservacaoAtivoPor(contexto);
                }

                //Monta Entidade Bem
                bem.OperacaoId = dadosBem.OperacaoId;
                bem.ClassificacaoId = dadosBem.ClassificacaoId;
                bem.Descricao = dadosBem.Descricao;
                bem.DataAquisicao = dadosBem.DataAquisicao;
                bem.DocumentoHabil = dadosBem.DocumentoHabil;
                bem.ValorMercado = dadosBem.ValorMercado;
                bem.PeriodoVidaUtilizado = dadosBem.VidaUtilizada;
                bem.Historico = dadosBem.Historico;
                bem.UsuarioId = dadosBem.UsuarioId;
                bem.Baixa = false;

                //Monta Entidade BemValor
                bemValor.MoedaId = dadosBem.MoedaId;
                bemValor.EstadoconservacaoId = dadosBem.EstadoconservacaoId;
                bemValor.Valor = dadosBem.Valor;
                bemValor.VidaUtil = Convert.ToInt32(dadosBem.VidaUtil);
                //Esta data começa com data da aquisicao sem data fim e quando o bem for reavalidado eh criada outra opção
                bemValor.DataInicio = dadosBem.DataAquisicao;
                bemValor.UsuarioId = dadosBem.UsuarioId;

                //Monta Entidade Movimentacao
                movimentacao.Setor = dadosBem.Setor;
                //Esta data começa com data da incorporacao sem data fim e quando o bem for transferido eh criada outra opção
                movimentacao.DataInicio = dadosBem.DataIncorporacao;
                movimentacao.UsuarioId = dadosBem.UsuarioId;

                for (int i = 0; i < quantidadeItens; i++)
                {
                    proximoNumero = 0;
                    //Insere Bem
                    this.Insere(contexto, bem);

                    //Alimenta BemId
                    bemValor.BemId = bem.BemId;

                    //Insere Valor
                    rnBemValor.Insere(contexto, bemValor);

                    //Busca proximo numero para Bem
                    proximoNumero = rnMovimentacao.ObtemProximoNumeroPor(contexto, dadosBem.Setor);

                    //Monta Entidade Movimentacao
                    movimentacao.BemId = bem.BemId;
                    movimentacao.Numero = proximoNumero;

                    //Insere Movimentacao
                    rnMovimentacao.Insere(contexto, movimentacao);

                    //Alimenta com dados gerados
                    dadosBem.Numero = movimentacao.Numero.ToString().PadLeft(6, '0');
                    dadosBem.BemId = bem.BemId;

                    if (i == 0)
                    {
                        dadosBem.NumeroInicial = dadosBem.Numero;
                    }
                }
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
                        mensagem = ex.Message.Replace("ERRO: ", string.Empty);
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
        }

        private void Insere(DataContext contexto, Entidades.Bem bem)
        {
            ContextQuery contextQuery = new ContextQuery();
            bem.DataCadastro = DateTime.Now;

            contextQuery.Command = @" INSERT INTO Patrimonio.BEM
                                    (OPERACAOID, 
                                     CLASSIFICACAOID,                                      
                                     DESCRICAO,                                      
                                     DATAAQUISICAO,                                      
                                     DOCUMENTOHABIL, 
                                     VALORMERCADO, 
                                     PERIODOVIDAUTILIZADO, 
                                     HISTORICO,  
                                     BAIXA,                                   
                                     USUARIOID, 
                                     DATACADASTRO, 
                                     DATAALTERACAO) 
                        VALUES      (@OPERACAOID, 
                                     @CLASSIFICACAOID,                                      
                                     @DESCRICAO,                                      
                                     @DATAAQUISICAO,                                      
                                     @DOCUMENTOHABIL, 
                                     @VALORMERCADO, 
                                     @PERIODOVIDAUTILIZADO, 
                                     @HISTORICO,    
                                     @BAIXA,                                  
                                     @USUARIOID, 
                                     @DATACADASTRO, 
                                     @DATAALTERACAO) 


                         SELECT BEMID
                         FROM Patrimonio.BEM
                         WHERE  OPERACAOID = @OPERACAOID AND 
                                CLASSIFICACAOID = @CLASSIFICACAOID AND                                      
                                DESCRICAO =  @DESCRICAO AND                                  
                                DATAAQUISICAO =  @DATAAQUISICAO AND                                      
                                DOCUMENTOHABIL = @DOCUMENTOHABIL AND
                                ISNULL(VALORMERCADO,0) = ISNULL(@VALORMERCADO,0) AND
                                ISNULL(PERIODOVIDAUTILIZADO,'') = ISNULL(@PERIODOVIDAUTILIZADO,'') AND
                                ISNULL(HISTORICO,'') = ISNULL(@HISTORICO,'') AND 
                                BAIXA = @BAIXA AND                            
                                USUARIOID =  @USUARIOID 
                                AND DATACADASTRO =  @DATACADASTRO
                         ";

            contextQuery.Parameters.Add("@OPERACAOID", SqlDbType.Int, bem.OperacaoId);
            contextQuery.Parameters.Add("@CLASSIFICACAOID", SqlDbType.Int, bem.ClassificacaoId);
            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, bem.Descricao);
            contextQuery.Parameters.Add("@DATAAQUISICAO", SqlDbType.Date, bem.DataAquisicao.Date);
            contextQuery.Parameters.Add("@DOCUMENTOHABIL", SqlDbType.VarChar, bem.DocumentoHabil);
            contextQuery.Parameters.Add("@VALORMERCADO", SqlDbType.Decimal, bem.ValorMercado);
            contextQuery.Parameters.Add("@PERIODOVIDAUTILIZADO", SqlDbType.Int, bem.PeriodoVidaUtilizado);
            contextQuery.Parameters.Add("@HISTORICO", SqlDbType.VarChar, bem.Historico);
            contextQuery.Parameters.Add("@BAIXA", SqlDbType.Bit, false);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, bem.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, bem.DataCadastro);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, bem.DataCadastro);

            bem.BemId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public ValidacaoDados ValidaBaixa(DTOs.DadosBaixaBemPatrimonial dadosBaixa, DateTime dataAquisicao, DateTime dataIncorporacao, string setor)
        {
            RN.Patrimonio.PeriodoLancamento rnPeriodoLancamento = new PeriodoLancamento();           
            RN.Patrimonio.Movimentacao rnMovimentacao = new Movimentacao();
            RN.Patrimonio.TransferenciaItem rnTransferenciaItem = new TransferenciaItem();
            RN.Perfil rnPerfil = new Perfil();
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosBaixa == null)
            {
                return validacaoDados;
            }

            dadosBaixa.Baixa = true;

            if (dadosBaixa.BemId <= 0)
            {
                mensagens.Add("Campo CÓDIGO DO BEM é obrigatório.");
            }

            if (dadosBaixa.MotivoBaixaId <= 0)
            {
                mensagens.Add("Campo MOTIVO BAIXA é obrigatório.");
            }

            if (dataAquisicao <= DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DE AQUISIÇÃO é obrigatório.");
            }

            if (dataIncorporacao <= DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DE AQUISIÇÃO é obrigatório.");
            }


            if (dadosBaixa.ProcessoBaixa.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo PROCESSO é obrigatório.");
            }
            else
            {
                if (dadosBaixa.ProcessoBaixa.Length > 100)
                {
                    mensagens.Add("O campo PROCESSO deve ter no máximo de 100 caracteres.");
                }
            }

            if (dadosBaixa.PrefixoProcesso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo PREFIXO do PROCESSO é obrigatório.");
            }

            if (!dadosBaixa.JustificativaBaixa.IsNullOrEmptyOrWhiteSpace())
            {
                if (dadosBaixa.JustificativaBaixa.Length > 500)
                {
                    mensagens.Add("O campo JUSTIFICATIVA deve ter no máximo de 500 caracteres.");
                }
            }

            //Verifica se o motivo é SUBTRAÍDO
            if (dadosBaixa.MotivoBaixaId == (int)RN.Patrimonio.MotivoBaixa.EnumMotivoBaixa.Subtraido)
            {
                if (dadosBaixa.BoletimOcorrencia.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo BOLETIM DE OCORRÊNCIA é obrigatório.");
                }
                else
                {
                    if (dadosBaixa.BoletimOcorrencia.Length > 100)
                    {
                        mensagens.Add("O campo BOLETIM DE OCORRÊNCIA deve ter no máximo de 100 caracteres.");
                    }
                }
            }
            else
            {
                //Limpa dados do motivo SUBTRAÍDO
                dadosBaixa.BoletimOcorrencia = null;
            }

            if (dadosBaixa.DataBaixa <= DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DA BAIXA é obrigatório.");
            }
            else
            {
                if (dadosBaixa.DataBaixa.Date > DateTime.Now.Date)
                {
                    mensagens.Add("A DATA DA BAIXA não pode ser maior que a data atual.");
                }

                if (dadosBaixa.DataBaixa.Date < dataAquisicao)
                {
                    mensagens.Add("A DATA DA BAIXA não pode ser MENOR que a DATA DE AQUISIÇÃO.");
                }

                if (dadosBaixa.DataBaixa.Date < dataIncorporacao)
                {
                    mensagens.Add("A DATA DA BAIXA não pode ser MENOR que a DATA DE INCORPORAÇÃO.");
                }

                if (!rnPerfil.PossuiPerfilLiberacaoPatrimonioFinalizadoPor(dadosBaixa.UsuarioId))
                {
                    //if (dadosBaixa.DataBaixa.Year != DateTime.Now.Year)
                    //{
                    //    mensagens.Add("O ano BAIXA não pode ser diferente do ano corrente.");
                    //}

                    if (!rnPeriodoLancamento.PossuiPeriodoLancamentoAbertoPor(dadosBaixa.DataBaixa.Year, DateTime.Now.Date))
                    {
                        mensagens.Add("A DATA DA BAIXA está fora do intervalo permitido para lançamento.");
                    }
                }
            }

            //Verifica se o motivo é TRANSFERIDO PARA UMA PREFEITURA OU INSTITUIÇÃO
            if (dadosBaixa.MotivoBaixaId == (int)RN.Patrimonio.MotivoBaixa.EnumMotivoBaixa.TransferidoPrefeituraInstituicao)
            {
                if (dadosBaixa.CnpjInstituicaoDestino.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo CNPJ DA PREFEITURA / INSTITUIÇÃO é obrigatório.");
                }

                if (dadosBaixa.InstituicaoDestino.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo NOME DA PREFEITURA / INSTITUIÇÃO é obrigatório.");
                }
            }
            else
            {
                //Limpa dados do motivo TRANSFERIDO PARA UMA PREFEITURA OU INSTITUIÇÃO
                dadosBaixa.InstituicaoDestino = null;
                dadosBaixa.CnpjInstituicaoDestino = null;
            }

            if (dadosBaixa.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO RESPONSÁVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se o item ja tem baixa     
                    if (this.PossuiBaixaPor(contexto, dadosBaixa.BemId))
                    {
                        mensagens.Add("Já foi cadastrada uma baixa para este Bem.");
                    }

                    //Verifica quem é o dono atual do bem
                    string setorAtual = rnMovimentacao.ObtemSetorVigentePor(contexto, dadosBaixa.BemId, DateTime.Now);
                    if (setorAtual != setor)
                    {
                        mensagens.Add("Este bem não pode ter baixa cadastrada pois possui não pertence a esta unidade.");
                    }
                    else
                    {
                        //Verifica se possui transferencia pendente
                        if (rnTransferenciaItem.PossuiTransferenciaPendentePor(contexto, dadosBaixa.BemId))
                        {
                            mensagens.Add("Este bem não pode ter baixa cadastrada pois possui transferência pendente.");
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

        public bool PossuiBaixaPor(DataContext contexto, int bemId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Patrimonio.BEM (NOLOCK)
                                    WHERE BAIXA = 1
											AND BEMID = @BEMID ";

            contextQuery.Parameters.Add("@BEMID", SqlDbType.Int, bemId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Baixa(DTOs.DadosBaixaBemPatrimonial dadosBaixa)
        {
            RN.Patrimonio.Movimentacao rnMovimentacao = new Movimentacao();
            RN.Patrimonio.BemValor rnBemValor = new BemValor();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Atualiza baixa no bem
                this.AtualizaDadosBaixa(contexto, dadosBaixa);

                //Finaliza Valor Bem Ativo
                rnBemValor.FinalizaBemValorAtivo(contexto, dadosBaixa.BemId, dadosBaixa.DataBaixa, dadosBaixa.UsuarioId);

                //Finaliza Movimentacao Ativa
                rnMovimentacao.FinalizaMovimentacaoAtiva(contexto, dadosBaixa.BemId, dadosBaixa.DataBaixa, dadosBaixa.UsuarioId);
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

        public void AtualizaDadosBaixa(DataContext contexto, DTOs.DadosBaixaBemPatrimonial dadosBaixa)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Patrimonio.BEM
                                SET    BAIXA = @BAIXA, 
                                       MOTIVOBAIXAID = @MOTIVOBAIXAID, 
                                       DATABAIXA = @DATABAIXA, 
                                       PROCESSOBAIXA = @PROCESSOBAIXA,
                                       JUSTIFICATIVABAIXA = @JUSTIFICATIVABAIXA, 
                                       BOLETIMOCORRENCIA = @BOLETIMOCORRENCIA, 
                                       INSTITUICAODESTINO = @INSTITUICAODESTINO, 
                                       CNPJINSTITUICAODESTINO = @CNPJINSTITUICAODESTINO, 
                                       USUARIOID = @USUARIOID, 
                                       DATAALTERACAO = @DATAALTERACAO 
                                WHERE  BEMID = @BEMID ";

            contextQuery.Parameters.Add("@BAIXA", SqlDbType.Bit, dadosBaixa.Baixa);
            contextQuery.Parameters.Add("@MOTIVOBAIXAID", SqlDbType.Int, dadosBaixa.MotivoBaixaId);
            contextQuery.Parameters.Add("@DATABAIXA", SqlDbType.Date, Convert.ToDateTime(dadosBaixa.DataBaixa).Date);
            contextQuery.Parameters.Add("@PROCESSOBAIXA", SqlDbType.VarChar, dadosBaixa.PrefixoProcesso + dadosBaixa.ProcessoBaixa);
            contextQuery.Parameters.Add("@JUSTIFICATIVABAIXA", SqlDbType.VarChar, dadosBaixa.JustificativaBaixa);
            contextQuery.Parameters.Add("@BOLETIMOCORRENCIA", SqlDbType.VarChar, dadosBaixa.BoletimOcorrencia);
            contextQuery.Parameters.Add("@INSTITUICAODESTINO", SqlDbType.VarChar, dadosBaixa.InstituicaoDestino);
            contextQuery.Parameters.Add("@CNPJINSTITUICAODESTINO", SqlDbType.VarChar, dadosBaixa.CnpjInstituicaoDestino);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dadosBaixa.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@BEMID", SqlDbType.Int, dadosBaixa.BemId);

            contexto.ApplyModifications(contextQuery);
        }

        public int RetornaTaxaValorResidualVigentePor(DataContext contexto, int bemId)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int taxaValorResidual = 0;
            try
            {
                contextQuery.Command = @"  SELECT TAXAVALORRESIDUAL
                                       FROM   PATRIMONIO.BEM B
                                               INNER JOIN PATRIMONIO.CLASSIFICACAO C (NOLOCK)
                                                       ON B.CLASSIFICACAOID = C.CLASSIFICACAOID
                                               INNER JOIN PATRIMONIO.CLASSIFICACAOVIGENCIA CV (NOLOCK)
                                                       ON B.CLASSIFICACAOID = CV.CLASSIFICACAOID
                                        WHERE  B.BEMID = @BEMID
                                               AND CV.DATAINICIO <= GETDATE()
                                               AND ( CV.DATAFIM IS NULL
                                                      OR CV.DATAFIM >= GETDATE() )  
										                                         ";

                contextQuery.Parameters.Add("@BEMID", SqlDbType.Int, bemId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    taxaValorResidual = Convert.ToInt32(reader["TAXAVALORRESIDUAL"]);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return taxaValorResidual;
        }


        public void Altera(DTOs.DadosCadastroBemPatrimonial dadosBem)
        {
            RN.Patrimonio.Movimentacao rnMovimentacao = new Movimentacao();
            RN.Patrimonio.Reavaliacao rnReavaliacao = new Reavaliacao();
            RN.Patrimonio.EstadoConservacao rnEstadoConservacao = new EstadoConservacao();
            RN.Patrimonio.BemValor rnBemValor = new BemValor();
            RN.Patrimonio.Entidades.Bem bem = new Techne.Lyceum.RN.Patrimonio.Entidades.Bem();
            RN.Patrimonio.Entidades.BemValor bemValor = new Techne.Lyceum.RN.Patrimonio.Entidades.BemValor();
            RN.Patrimonio.Entidades.Movimentacao movimentacao = new Techne.Lyceum.RN.Patrimonio.Entidades.Movimentacao();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Monta dados Especificos para item udaso
                if (dadosBem.OperacaoId == (int)RN.Patrimonio.Operacao.EnumOperacao.IncorporacaoSemRegistroAnterior || dadosBem.OperacaoId == (int)RN.Patrimonio.Operacao.EnumOperacao.DoacaoItemUsado)
                {
                    //Alimenta Campos
                    dadosBem.DataAquisicao = dadosBem.DataIncorporacao;
                    dadosBem.VidaUtil = dadosBem.VidaFutura;
                    dadosBem.Valor = rnReavaliacao.ObtemValorReavaliadoPor(contexto, dadosBem.EstadoconservacaoId, Convert.ToInt32(dadosBem.VidaUtilizada), Convert.ToInt32(dadosBem.VidaFutura), Convert.ToDecimal(dadosBem.ValorMercado), dadosBem.ClassificacaoId);
                }
                else
                {
                    //Busca dados Especificos para item novo
                    dadosBem.EstadoconservacaoId = rnEstadoConservacao.ObtemMelhorEstadoConservacaoAtivoPor(contexto);
                }

                //Monta Entidade Bem
                bem.BemId = dadosBem.BemId;
                bem.OperacaoId = dadosBem.OperacaoId;
                bem.ClassificacaoId = dadosBem.ClassificacaoId;
                bem.Descricao = dadosBem.Descricao;
                bem.DataAquisicao = dadosBem.DataAquisicao;
                bem.DocumentoHabil = dadosBem.DocumentoHabil;
                bem.ValorMercado = dadosBem.ValorMercado;
                bem.PeriodoVidaUtilizado = dadosBem.VidaUtilizada;
                bem.Historico = dadosBem.Historico;
                bem.UsuarioId = dadosBem.UsuarioId;
                bem.Baixa = false;

                //Altera o Bem
                this.Altera(contexto, bem);

                //Monta Entidade BemValor
                bemValor.BemId = dadosBem.BemId;
                bemValor.EstadoconservacaoId = dadosBem.EstadoconservacaoId;
                bemValor.Valor = dadosBem.Valor;
                bemValor.VidaUtil = Convert.ToInt32(dadosBem.VidaUtil);
                //Esta data começa com data da aquisicao sem data fim e quando o bem for reavalidado eh criada outra opção
                bemValor.DataInicio = dadosBem.DataAquisicao;
                bemValor.UsuarioId = dadosBem.UsuarioId;

                //Altera Valor
                rnBemValor.Altera(contexto, bemValor);

                //Monta Entidade Movimentacao
                movimentacao.BemId = dadosBem.BemId;
                movimentacao.Numero = Convert.ToInt32(dadosBem.Numero);
                movimentacao.Setor = dadosBem.Setor;
                //Esta data começa com data da incorporacao sem data fim e quando o bem for transferido eh criada outra opção
                movimentacao.DataInicio = dadosBem.DataIncorporacao;
                movimentacao.UsuarioId = dadosBem.UsuarioId;

                //Altera Movimentacao
                rnMovimentacao.Altera(contexto, movimentacao);
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
                        mensagem = ex.Message.Replace("ERRO: ", string.Empty);
                    }
                    else
                    {
                        mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine, Convert.ToString(ex.Message));
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
        }

        public void Altera(DataContext contexto, Entidades.Bem bem)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Patrimonio.BEM
                                SET    OPERACAOID = @OPERACAOID, 
                                       CLASSIFICACAOID = @CLASSIFICACAOID, 
                                       DESCRICAO = @DESCRICAO, 
                                       DATAAQUISICAO = @DATAAQUISICAO,
                                       DOCUMENTOHABIL = @DOCUMENTOHABIL, 
                                       VALORMERCADO = @VALORMERCADO, 
                                       PERIODOVIDAUTILIZADO = @PERIODOVIDAUTILIZADO, 
                                       HISTORICO = @HISTORICO, 
                                       USUARIOID = @USUARIOID, 
                                       DATAALTERACAO = @DATAALTERACAO 
                                WHERE  BEMID = @BEMID ";

            contextQuery.Parameters.Add("@BEMID", SqlDbType.Int, bem.BemId);
            contextQuery.Parameters.Add("@OPERACAOID", SqlDbType.Int, bem.OperacaoId);
            contextQuery.Parameters.Add("@CLASSIFICACAOID", SqlDbType.Int, bem.ClassificacaoId);
            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, bem.Descricao);
            contextQuery.Parameters.Add("@DATAAQUISICAO", SqlDbType.Date, bem.DataAquisicao.Date);
            contextQuery.Parameters.Add("@DOCUMENTOHABIL", SqlDbType.VarChar, bem.DocumentoHabil);
            contextQuery.Parameters.Add("@VALORMERCADO", SqlDbType.Decimal, bem.ValorMercado);
            contextQuery.Parameters.Add("@PERIODOVIDAUTILIZADO", SqlDbType.Int, bem.PeriodoVidaUtilizado);
            contextQuery.Parameters.Add("@HISTORICO", SqlDbType.VarChar, bem.Historico);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, bem.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
