using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.DTOs;
using System.Data.SqlClient;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.RN.Patrimonio
{
    public class AgenteResponsavel
    {
        public DataTable ListaPor(string setor)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT A.AGENTERESPONSAVELID, 
                                                A.SETOR, 
                                                S.UA_ATUAL,
                                                S.NOMESETOR AS SETORDESCRICAO, 
                                                A.MATRICULA, 
                                                P.NOME_COMPL, 
				                                F.FUNCAO,
				                                F.DESCRICAO AS FUNCAODESCRICAO,
                                                A.DATANOMEACAO, 
                                                A.DATADISPENSA, 
                                                A.DATAPUBLICACAONOMEACAO, 
                                                A.DATAPUBLICACAODISPENSA ,
                                                A.USUARIOID,
				                                A.DATACADASTRO
                                FROM   [PATRIMONIO].[AGENTERESPONSAVEL] A (NOLOCK) 
                                        LEFT JOIN LY_LOTACAO L (NOLOCK) 
                                                ON A.MATRICULA = L.MATRICULA   AND (L.DATA_DESATIVACAO IS NULL
			                                OR CONVERT(DATE, L.DATA_DESATIVACAO) > CONVERT(DATE, GETDATE()))
                                            AND CONVERT(DATE,L.DATA_NOMEACAO) <  CONVERT(DATE, GETDATE())
                                        JOIN ( SELECT   PESSOA ,
														MATRICULA 
											   FROM     LY_VINCULO VV WITH ( NOLOCK )														
											   UNION
											   SELECT   PESSOA ,
														MATRICULA 
											   FROM     LY_DOCENTE WITH ( NOLOCK )
											 ) SR ON SR.MATRICULA = A.MATRICULA
                                        LEFT JOIN LY_PESSOA P (NOLOCK) 
                                                ON P.PESSOA = SR.PESSOA 
		                                LEFT JOIN LY_FUNCAO F (NOLOCK)
				                                ON F.FUNCAO = L.FUNCAO
                                        INNER JOIN HADES.DBO.VW_SETOR S (NOLOCK) 
                                                ON A.SETOR = S.SETOR 
                                WHERE A.SETOR = @SETOR 
	                                
                                ORDER BY A.DATANOMEACAO DESC ";

                contextQuery.Parameters.Add("@SETOR", SqlDbType.VarChar, setor);

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

        public DataTable ListaPorMatricula(string matricula)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT A.AGENTERESPONSAVELID, 
                                                A.SETOR, 
                                                S.UA_ATUAL,
                                                S.NOMESETOR AS SETORDESCRICAO, 
                                                A.MATRICULA, 
                                                P.NOME_COMPL, 
				                                F.FUNCAO,
				                                F.DESCRICAO AS FUNCAODESCRICAO,
                                                A.DATANOMEACAO, 
                                                A.DATADISPENSA, 
                                                A.DATAPUBLICACAONOMEACAO, 
                                                A.DATAPUBLICACAODISPENSA ,
                                                A.USUARIOID,
				                                A.DATACADASTRO
                                FROM   [PATRIMONIO].[AGENTERESPONSAVEL] A (NOLOCK) 
                                        LEFT JOIN LY_LOTACAO L (NOLOCK) 
                                                ON A.MATRICULA = L.MATRICULA   AND (L.DATA_DESATIVACAO IS NULL
			                                OR CONVERT(DATE, L.DATA_DESATIVACAO) > CONVERT(DATE, GETDATE()))
                                            AND CONVERT(DATE,L.DATA_NOMEACAO) <  CONVERT(DATE, GETDATE())
                                        JOIN ( SELECT   PESSOA ,
														MATRICULA 
											   FROM     LY_VINCULO VV WITH ( NOLOCK )														
											   UNION
											   SELECT   PESSOA ,
														MATRICULA 
											   FROM     LY_DOCENTE WITH ( NOLOCK )
											 ) SR ON SR.MATRICULA = A.MATRICULA
                                        LEFT JOIN LY_PESSOA P (NOLOCK) 
                                                ON P.PESSOA = SR.PESSOA 
		                                LEFT JOIN LY_FUNCAO F (NOLOCK)
				                                ON F.FUNCAO = L.FUNCAO
                                        INNER JOIN HADES.DBO.VW_SETOR S (NOLOCK) 
                                                ON A.SETOR = S.SETOR 
                                WHERE A.MATRICULA = @MATRICULA
	                                
                                ORDER BY A.DATANOMEACAO DESC ";

                contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, matricula);

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

        public bool CHTC(string matricula)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;
       
            try
            {
                contextQuery.Command = @" SELECT distinct 1 as ret
                                            FROM  LY_DOCENTE LD (NOLOCK)
		                                            INNER JOIN	LY_LOTACAO LL (NOLOCK) ON LL.MATRICULA = LD.MATRICULA		
		                                            INNER JOIN	LY_FUNCAO LF ON LF.FUNCAO = LL.FUNCAO
		                                            INNER JOIN	LY_CATEGORIA_DOCENTE CD ON CD.CATEGORIA = LD.CATEGORIA
		                                    where LD.USUARIO = @MATRICULA and
                                                 (LF.FUNCAO = 10091 
                                                or LF.FUNCAO = 10092 
                                                or LF.FUNCAO = 13 
                                                or LF.FUNCAO = 14
                                                or LF.FUNCAO = 10090 )";

                contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, matricula);

               
                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    existe = true;
                }

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

            return existe;
        }


        public String[] VerificaMatricula(string matricula)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            String[] vet = new string[3];
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"SELECT DISTINCT LO.SETOR, UE.NOME_COMP
                                        FROM   LY_LOTACAO LO
											   INNER JOIN	LY_UNIDADE_ENSINO UE (NOLOCK) ON UE.UNIDADE_ENS = LO.UNIDADE_ENS
                                        WHERE  LO.MATRICULA = @MATRICULA
                                                and (FUNCAO = 10091 
                                                or FUNCAO = 10092 
                                                or FUNCAO = 13 
                                                or FUNCAO = 14 )
                                               AND ( LO.DATA_DESATIVACAO IS NULL
                                                      OR CONVERT(DATE, LO.DATA_DESATIVACAO) > CONVERT(DATE, Getdate()) ) ";

                contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, matricula);

               
                 dt = ctx.GetDataTable(contextQuery);
         
                 foreach (DataRow row in dt.Rows)
                 {
                     vet[0] = Convert.ToString(row["SETOR"]);
                     vet[1] = Convert.ToString(row["NOME_COMP"]);
                 }
                 
                

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

            return vet;
        }

        public DataTable VerificaAlocacao(string setor)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"SELECT 
                                               T.TURMA,
                                               CASE T.EM_ELABORACAO  
			                                          WHEN 'S' THEN 'INCOMPLETA'  
			                                          WHEN 'N' THEN 'COMPLETA'  
			                                          ELSE 'SEM ALOCACAO'  
                                                END EM_ELABORACAO, 
                                               (	SELECT COUNT(DISTINCT ALUNO) FROM LY_MATRICULA M (NOLOCK)
			                                        WHERE 1 = 1 
			                                        AND T.ANO = M.ANO
			                                        AND T.SEMESTRE = M.SEMESTRE
			                                        AND T.TURMA = M.TURMA
			                                        AND M.SIT_MATRICULA <> 'CANCELADO'
		                                        ) ALUNOS_MATRICULADOS
                                        FROM   LY_UNIDADE_ENSINO UE (NOLOCK) 
                                               JOIN TCE_REGIONAL R (NOLOCK) ON R.ID_REGIONAL = UE.ID_REGIONAL
                                               JOIN LY_TURMA T (NOLOCK) ON T.UNIDADE_RESPONSAVEL = UE.UNIDADE_ENS AND T.ANO = YEAR(GETDATE())
                                               JOIN MUNICIPIO MM (NOLOCK) ON MM.CODIGO = UE.MUNICIPIO
                                               JOIN LY_CURSO CU ON CU.CURSO = T.CURSO						
                                               JOIN    HADES..VW_SETOR VWS on VWS.SETOR = UE.SETOR

                                        WHERE		EXISTS (
					                                        SELECT	1
					                                        FROM	LY_UNIDADE_ENSINO_SITUACAO US (NOLOCK)
					                                        WHERE	US.UNIDADE_ENS = UE.UNIDADE_ENS
					                                        AND		US.SITUACAO = 'ESTADUAL'
					                                        AND		US.UNIDADE_ENS+CONVERT(VARCHAR(20),US.DT_SITUACAO) IN 
																					                                        (
																					                                        SELECT	UNIDADE_ENS+CONVERT(VARCHAR(20),MAX(DT_SITUACAO)) 
																					                                        FROM LY_UNIDADE_ENSINO_SITUACAO (NOLOCK) GROUP BY UNIDADE_ENS
																					                                        )
					                                        )
		                                        AND	(UE.SIT_FUNCIONAMENTO IS NULL OR UE.SIT_FUNCIONAMENTO = 'EMATIVIDADE')
		                                        AND  UE.SETOR = @SETOR
		                                        AND (T.EM_ELABORACAO <>'N' OR T.EM_ELABORACAO IS NULL)
		                                        AND T.SIT_TURMA = 'ABERTA'
		                                        AND T.CURSO NOT IN ('9999.92')
                                        GROUP  BY R.REGIONAL, UE.SETOR, UE.NOME_COMP, T.TURMA, T.EM_ELABORACAO, T.ANO, T.SEMESTRE, MM.NOME,T.FACULDADE,T.CURSO,T.CURRICULO,T.SERIE,T.TURNO, VWS.UA_ATUAL, VWS.UA_ANTIGA,
		                                        T.SERIE,
		                                        T.CURSO,
		                                        CU.NOME
                                        ORDER  BY R.REGIONAL, UE.NOME_COMP, T.TURMA";

                contextQuery.Parameters.Add("@SETOR", SqlDbType.VarChar, setor);

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

        public DataTable VerificaPendenciaCadastro(string setor)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT  DISTINCT
			                                        VWS.UA_ANTIGA SETOR ,
			                                        LUE.UNIDADE_ENS ,
			                                        LUE.NOME_COMP ESCOLA ,
			                                        LUE.ID_REGIONAL ,
			                                        LUE.MUNICIPIO,
			                                        LD.MATRICULA ,
			                                        LD.PESSOA,
			                                        ISNULL(LD.VINCULO, '') VINCULO,
			                                        LD.CATEGORIA,
			                                        LL.FUNCAO,
			                                        CONVERT(VARCHAR(MAX), NULL) AS PENDENCIA,
			                                        VWS.UA_ATUAL
	                                        INTO #DOCENTES
	                                        FROM  LY_DOCENTE LD (NOLOCK)
		                                        INNER JOIN	LY_LOTACAO LL (NOLOCK) ON LL.MATRICULA = LD.MATRICULA					
		                                        INNER JOIN	LY_UNIDADE_ENSINO LUE (NOLOCK) ON LUE.SETOR = LL.SETOR
		                                        INNER JOIN    HADES..VW_SETOR VWS ON VWS.SETOR = LUE.SETOR
	                                        WHERE NOT EXISTS ( 
					                                        SELECT	1
					                                        FROM	LY_LICENCA_DOCENTE LLD (NOLOCK)
					                                        WHERE	LLD.NUM_FUNC = LD.NUM_FUNC
					                                        AND		LLD.DTFIM IS NULL 
					                                        )
	                                        AND LL.FUNCAO NOT IN ('10311','10127')  --ESTAGIÁRIO (ECO), REGENTE MAIS EDUCAÇÃO
	                                        AND EXISTS (
			                                        SELECT	1
			                                        FROM	LY_UNIDADE_ENSINO_SITUACAO US (NOLOCK)
			                                        WHERE	US.UNIDADE_ENS = LUE.UNIDADE_ENS
			                                        AND		US.SITUACAO = 'ESTADUAL'
			                                        AND		US.UNIDADE_ENS+CONVERT(VARCHAR(20),US.DT_SITUACAO) IN 
						                                        (
						                                        SELECT	UNIDADE_ENS+CONVERT(VARCHAR(20),MAX(DT_SITUACAO)) 
						                                        FROM LY_UNIDADE_ENSINO_SITUACAO (NOLOCK) GROUP BY UNIDADE_ENS
						                                        )
			                                        )
	                                        AND	(LUE.SIT_FUNCIONAMENTO IS NULL OR LUE.SIT_FUNCIONAMENTO = 'EMATIVIDADE')
	                                        AND LL.DATA_NOMEACAO <= CONVERT(DATE, GETDATE())
	                                        AND (LL.DATA_DESATIVACAO IS NULL OR LL.DATA_DESATIVACAO >= CONVERT(DATE, GETDATE()))
	                                        --AND	(LUE.ID_REGIONAL = ISNULL(@regional, LUE.ID_REGIONAL) OR LUE.ID_REGIONAL IS NULL)     
	                                        --AND (LUE.MUNICIPIO = ISNULL(@municipio, LUE.MUNICIPIO) OR LUE.MUNICIPIO IS NULL)     
	                                        AND (LUE.SETOR = ISNULL(@SETOR, LUE.SETOR) OR LUE.SETOR IS NULL)


	                                        SELECT DISTINCT SETOR ,
			                                        UNIDADE_ENS ,
			                                        ESCOLA ,
			                                        ID_REGIONAL ,
			                                        MUNICIPIO,
			                                        MATRICULA ,
			                                        LD.PESSOA,
			                                        VINCULO,
			                                        CATEGORIA,
			                                        FUNCAO,
			                                        PENDENCIA,
			                                        UA_ATUAL,
			                                        LP.NOME_COMPL AS NOME_DOCENTE ,
			                                        LP.NOME_MAE,
			                                        LP.NOME_PAI,
		                                            LP.IDFUNCIONAL AS ID_FUNCIONAL,
			                                        LP.CPF ,
			                                        LP.DT_NASC,
			                                        LP.NOME_COMPL,
			                                        LP.SEXO,
			                                        LP.NECESSIDADEESPECIALID,
			                                        LP.ETNIA,
			                                        LP.EST_CIVIL,
			                                        LP.PAIS_NASC,
			                                        LP.NACIONALIDADE,
			                                        LP.MUNICIPIO_NASC,
			                                        LP.ENDERECO,
			                                        LP.END_MUNICIPIO,
			                                        LP.CEP,
			                                        FL.FL_FIELD_01
	                                        INTO #PENDENTES
	                                        FROM #DOCENTES LD
		                                        INNER JOIN LY_PESSOA LP (NOLOCK) ON LP.PESSOA = LD.PESSOA
		                                        INNER JOIN LY_FL_PESSOA FL (NOLOCK) ON FL.PESSOA = LP.PESSOA
	                                        WHERE LP.DT_NASC < '1900-01-01'
		                                        OR LP.DT_NASC IS NULL
		                                        OR LP.NOME_COMPL IS NULL
		                                        OR LP.SEXO IS NULL
		                                        OR LP.NECESSIDADEESPECIALID IS NULL
		                                        OR LP.ETNIA IS NULL
		                                        OR LP.EST_CIVIL IS NULL
		                                        OR LP.PAIS_NASC IS NULL
		                                        OR LP.NACIONALIDADE IS NULL
		                                        OR LP.MUNICIPIO_NASC IS NULL
		                                        OR LP.ENDERECO IS NULL
		                                        OR LP.END_MUNICIPIO IS NULL
		                                        OR LP.CEP IS NULL
		                                        OR FL.FL_FIELD_01 IS NULL
		                                        OR LP.CPF IS NULL 
		                                        OR (NOT EXISTS (SELECT    1
						                                        FROM TCE_FORMACAO_PESSOAL FP
						                                        WHERE FP.PESSOA = LP.PESSOA )
			                                        OR NOT EXISTS (SELECT    1
						                                           FROM TCE_FORMACAO_PESSOAL FP (NOLOCK)
						                                           WHERE FP.PESSOA = LP.PESSOA and ESCOLARIDADE like 'Ensino Médio%')
			                                        )


                                        SELECT  
                                                SETOR ,
                                                UNIDADE_ENS ,
                                                ESCOLA ,
                                                REGIONAL ,
                                                LD.ID_REGIONAL ,
                                                MUNICIPIO.CODIGO ,
                                                MUNICIPIO.NOME NOME_MUNIC ,
                                                LD.MATRICULA ,
                                                UPPER (NOME_COMPL) NOME_DOCENTE ,
		                                        ISNULL(ID_FUNCIONAL, '') ID_FUNCIONAL,
		                                        ISNULL(LD.VINCULO, '') VINCULO,
                                                CPF , 
		                                        CD.NOME CARGO,
		                                        LF.DESCRICAO FUNCAO,
		                                        CASE
			                                        WHEN ID_FUNCIONAL IS NULL THEN 'ID FUNCIONAL NÃO CADASTRADO' + '. '
			                                        ELSE ''
		                                        END +
		                                        CASE
			                                        WHEN LD.VINCULO IS NULL THEN 'VÍNCULO NÃO CADASTRADO' + '. '
			                                        ELSE ''
		                                        END +
                                                CASE 
			                                        WHEN DT_NASC IS NULL THEN 'NASCIMENTO NÃO INFORMADO' + '. '
			                                        ELSE ''
		                                        END +
                                                CASE 
			                                        WHEN NOME_COMPL IS NULL THEN 'NOME NÃO INFORMADO' + '. '
                                                    ELSE ''
                                                END + 
                                                CASE 
			                                        WHEN SEXO IS NULL THEN 'SEXO NÃO INFORMADO' + '. '
			                                        ELSE ''
		                                        END +
                                                CASE 
			                                        WHEN NECESSIDADEESPECIALID IS NULL THEN 'NECESSIDADE ESPECIAL NÃO INFORMADA' + '. '
			                                        ELSE ''
		                                        END +
                                                CASE 
			                                        WHEN ETNIA IS NULL THEN 'COR/RACA  NÃO INFORMADA' + '. '
			                                        ELSE ''
		                                        END +
		                                        CASE 
			                                        WHEN EST_CIVIL IS NULL THEN 'ESTADO CIVIL NÃO INFORMADO' + '. '
			                                        ELSE ''
		                                        END +
                                                CASE 
			                                        WHEN PAIS_NASC IS NULL THEN 'PAIS NASCIMENTO NÃO INFORMADO' + '. '
			                                        ELSE ''
		                                        END +
                                                CASE 
			                                        WHEN NACIONALIDADE IS NULL THEN 'NACIONALIDADE NÃO INFORMADA' + '. '
			                                        ELSE ''
		                                        END +
                                                CASE 
			                                        WHEN MUNICIPIO_NASC IS NULL THEN 'MUNICIPIO NASCIMENTO NÃO INFORMADO' + '. '
			                                        ELSE ''
		                                        END +
                                                CASE 
			                                        WHEN ENDERECO IS NULL THEN 'ENDEREÇO NÃO INFORMADO' + '. '
			                                        ELSE ''
		                                        END +
		                                        CASE 
			                                        WHEN END_MUNICIPIO IS NULL THEN 'ENDEREÇO/MUNICÍPIO  NÃO INFORMADO' + '. '
			                                        ELSE ''
		                                        END +
		                                        CASE 
			                                        WHEN LD.CEP IS NULL THEN 'CEP NÃO INFORMADO' + '. '
			                                        ELSE ''
		                                        END +
		                                        CASE 
			                                        WHEN FL_FIELD_01 IS NULL THEN 'ZONA RESIDENCIAL NÃO INFORMADA' + '. '
			                                        ELSE ''
		                                        END +
		                                        CASE 
			                                        WHEN CPF IS NULL THEN 'CPF NÃO INFORMADO' 
			                                        ELSE ''
		                                        END +        
		                                        CASE 
			                                        WHEN LD.NOME_MAE IS NULL THEN 'NOME DA MÃE NÃO INFORMADO' + '. '
			                                        ELSE ''
		                                        END +        
		                                        CASE 
			                                        WHEN LD.NOME_PAI IS NULL THEN 'NOME DO PAI NÃO INFORMADO' + '. ' 
			                                        ELSE ''
		                                        END +

		                                        CASE 
			                                        WHEN NOT EXISTS ( SELECT    1
							                                          FROM      TCE_FORMACAO_PESSOAL FP (NOLOCK)
							                                          WHERE     FP.PESSOA = LD.PESSOA and ESCOLARIDADE like 'Ensino Médio%') THEN 'ENSINO MÉDIO NÃO CADASTRADO' + '. '
			                                        ELSE ''
		                                        END +			 
		                                        CASE 
			                                        WHEN NOT EXISTS ( SELECT    1
							                                          FROM      TCE_FORMACAO_PESSOAL FP (NOLOCK)
							                                          WHERE     FP.PESSOA = LD.PESSOA and ESCOLARIDADE like 'Superior%') THEN 'ENSINO SUPERIOR NÃO CADASTRADO' + '. '
			                                        ELSE ''       
		                                        END +  
		                                        CASE 
			                                        WHEN (EXISTS ( SELECT    1
							                                          FROM      TCE_FORMACAO_PESSOAL FP (NOLOCK)
							                                          WHERE     FP.PESSOA = LD.PESSOA and ESCOLARIDADE like 'Superior%')
				                                          AND
				                                          NOT EXISTS ( SELECT    1
							                                          FROM      TCE_FORMACAO_PESSOAL FP (NOLOCK)
							                                          WHERE     FP.PESSOA = LD.PESSOA and ESCOLARIDADE like 'Superior %')) THEN 'ENSINO SUPERIOR SEM ESPECIFICAÇÃO' + '. '
			                                        ELSE ''
		                                        END +
		                                        CASE
			                                        WHEN NOT EXISTS ( SELECT    1
                                                                      FROM      TCE_FORMACAO_PESSOAL FP (NOLOCK)
                                                                      WHERE     FP.PESSOA = LD.PESSOA ) THEN 'FORMAÇAO PESSOAL NÃO INFORMADA' + '. '
				                                                ELSE ''
		                                        END
		                                        AS PENDENCIA,
		                                        UA_ATUAL
                                        FROM   #PENDENTES LD
		                                        INNER JOIN	TCE_REGIONAL TR (NOLOCK) ON TR.ID_REGIONAL = LD.ID_REGIONAL
		                                        INNER JOIN	MUNICIPIO (NOLOCK) ON MUNICIPIO.CODIGO = LD.MUNICIPIO
		                                        INNER JOIN	LY_FUNCAO LF ON LF.FUNCAO = LD.FUNCAO
		                                        INNER JOIN	LY_CATEGORIA_DOCENTE CD ON CD.CATEGORIA = LD.CATEGORIA
                                        GROUP	BY 
		                                        LD.SETOR,
                                                LD.UNIDADE_ENS ,
                                                LD.ESCOLA ,
                                                TR.REGIONAL ,
                                                LD.ID_REGIONAL ,
                                                MUNICIPIO.CODIGO ,
                                                MUNICIPIO.NOME ,
                                                LD.SETOR ,
                                                LD.MATRICULA ,
                                                LD.CPF ,
                                                LD.NOME_COMPL ,
                                                LD.CEP ,
                                                LD.FL_FIELD_01 ,
                                                LD.CPF ,
                                                LD.ETNIA ,
                                                LD.EST_CIVIL ,
                                                LD.PAIS_NASC ,
                                                LD.NACIONALIDADE ,
                                                LD.MUNICIPIO_NASC ,
                                                LD.ENDERECO ,
                                                LD.END_MUNICIPIO ,
                                                LD.DT_NASC ,
                                                LD.NOME_COMPL ,
                                                LD.SEXO ,
                                                LD.NECESSIDADEESPECIALID ,
                                                LD.PESSOA,
                                                LD.ID_FUNCIONAL,
		                                        LD.VINCULO,
		                                        CD.NOME,
		                                        LF.DESCRICAO,
		                                        LD.NOME_MAE,
		                                        LD.NOME_PAI,
		                                        UA_ATUAL
                                        ORDER BY TR.REGIONAL ,
		                                        MUNICIPIO.NOME ,
		                                        ESCOLA

                                        DROP TABLE #PENDENTES
                                        DROP TABLE #DOCENTES";

                contextQuery.Parameters.Add("@SETOR", SqlDbType.VarChar, setor);


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

        public DataTable VerificaCargahoraria(string setor)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" DECLARE @MUNICIPIO INT;
                                            DECLARE @escola INT;
                                            DECLARE @coord INT;

                                            SET @escola = null;
                                            SET @coord = null;

                                            SET @MUNICIPIO = NULL;

                                            SELECT   
                                            MATRICULA,      
                                            IDFUNCIONAL,                        
                                            VINCULO,
                                            NOME_COMPL,  
                                            DESCRICAO,
                                            NUM_FUNC,  
                                                     CONVERT(INT,CH_SEMANAL_EFETIVA)CH_SEMANAL_EFETIVA                       
                                            FROM (                
                                            SELECT DISTINCT       
                                                  
                                            --CAMPOS:      
                                            DD.MATRICULA,      
                                            PE.IDFUNCIONAL,                        
                                            DD.VINCULO,
                                            PE.NOME_COMPL,  
                                            INGR.DESCRICAO,
                                            DD.NUM_FUNC,   
                                            ISNULL(CH.CARGAHORARIAREGENCIA,0)CH_SEMANAL_EFETIVA

                                            FROM  LY_DOCENTE DD (NOLOCK)
                                            JOIN LY_PESSOA PE ON PE.PESSOA = DD.PESSOA                          
                                            JOIN LY_LOTACAO LT (NOLOCK) ON DD.MATRICULA = LT.MATRICULA
							                                            AND (LT.DATA_NOMEACAO <= CONVERT(DATE, CURRENT_TIMESTAMP) 
							                                            AND (LT.DATA_DESATIVACAO > CONVERT(DATE,CURRENT_TIMESTAMP) OR LT.DATA_DESATIVACAO IS NULL))    
                                            JOIN HADES..VW_SETOR VS ON VS.SETOR = LT.SETOR 
                                            LEFT JOIN (
			                                            SELECT	NUM_FUNC, MAX(HH.DESCRICAO) DESCRICAO, MAX(HH.AGRUPAMENTO) AGRUPAMENTO
			                                            FROM	LY_GRUPO_HABILITACAO HH (NOLOCK)  
			                                            JOIN	LY_GRUPO_HABILITACAO_DOC DF (NOLOCK) 
			                                            ON		DF.AGRUPAMENTO = HH.AGRUPAMENTO        
			                                            WHERE	DF.AGRUPAMENTO_INGRESSO = 'S'
			                                            GROUP BY NUM_FUNC
			                                            ) AS INGR  ON		INGR.NUM_FUNC = DD.NUM_FUNC 			
                                            JOIN LY_FUNCAO FU (NOLOCK)			ON	 FU.FUNCAO = LT.FUNCAO 
									                                            AND	 FU.CAMPO_01 = 'S'
                                            JOIN LY_UNIDADE_ENSINO EU  (NOLOCK) ON	 LT.SETOR = EU.SETOR 
									                                            AND LT.UNIDADE_ENS = EU.UNIDADE_ENS
                                            JOIN TCE_REGIONAL R (NOLOCK)		ON	 R.ID_REGIONAL = EU.ID_REGIONAL
                                            JOIN MUNICIPIO MM (NOLOCK)			ON	 MM.CODIGO = EU.MUNICIPIO
                                            LEFT JOIN LY_CATEGORIA_DOCENTE CO ( NOLOCK ) ON CO.CATEGORIA = DD.CATEGORIA  
                                            LEFT JOIN RecursosHumanos.CH_AGRUPAMENTOCARGO AS CH ( NOLOCK ) ON CH.AGRUPAMENTOCARGOSID = CO.AGRUPAMENTOCARGOSID
                                                                                                          AND CH.FUNCAO = LT.FUNCAO          
                                            LEFT JOIN APOLLO..REL_CARENCIA_UNID (NOLOCK) CARUNID	ON	CARUNID.UNIDADE_ENS = EU.UNIDADE_ENS
											                                            AND	CARUNID.AGRUPAMENTO = INGR.AGRUPAMENTO
                                            LEFT JOIN (
			                                            SELECT	REGIONAL, MUNICIPIO, AGRUPAMENTO, SUM(CARENCIA_REAL) CARENCIA_REAL, SUM(GLP_REAL) GLP_REAL, SUM(CONTRATO_TEMP) CONTRATO_TEMP
			                                            FROM	(
						                                            SELECT	REGIONAL, UE1.MUNICIPIO, AGRUPAMENTO, C.UNIDADE_ENS, CARENCIA_REAL, GLP_REAL, CONTRATO_TEMP
						                                            FROM	APOLLO..REL_CARENCIA_UNID C (NOLOCK)
						                                            JOIN	LY_UNIDADE_ENSINO UE1
						                                            ON		UE1.UNIDADE_ENS = C.UNIDADE_ENS
					                                            ) AS MUN
			                                            GROUP BY MUNICIPIO, AGRUPAMENTO, REGIONAL
			                                            ) CARMUN	ON		CARMUN.AGRUPAMENTO = INGR.AGRUPAMENTO
								                                            AND		EU.MUNICIPIO = CARMUN.MUNICIPIO			
								                                            AND		EU.ID_REGIONAL = CARMUN.REGIONAL
                                            LEFT JOIN (
			                                            SELECT	NUM_FUNC, ANO, CENSO, TURNO, DIAS
			                                            FROM	APOLLO..REL_AULAS_DOCENTE (NOLOCK)
			                                            WHERE	ANO = year(getdate())
			                                            ) AL	ON		AL.NUM_FUNC = DD.NUM_FUNC
                                            LEFT JOIN -- aulas do docente no ano corrente em turma aberta sem contar as GLPs      
                                            (	SELECT	NUM_FUNC, ATUACAO, ANO 
	                                            FROM	APOLLO..REL_CH_LIVRE (NOLOCK)
	                                            WHERE	ANO = year(getdate())
                                             ) AS SS   ON SS.NUM_FUNC = DD.NUM_FUNC           
                                            LEFT JOIN (
			                                            SELECT NUM_FUNC, MOTIVO
			                                            FROM LY_LICENCA_DOCENTE WITH(NOLOCK) 
			                                            WHERE MOTIVO = '43' 
			                                            AND DTINI <= CONVERT(DATE, CURRENT_TIMESTAMP) 
			                                            AND DTFIM >= CONVERT(DATE, CURRENT_TIMESTAMP) 
			                                            ) MT	ON MT.NUM_FUNC = DD.NUM_FUNC
                                            LEFT JOIN (
			                                            SELECT	NUM_FUNC, DTFIM, LB1.DESCRICAO MOTIVO, DTINI
			                                            FROM	LY_LICENCA_DOCENTE MB (NOLOCK) 
			                                            JOIN	LY_LICENCAS LB1 (NOLOCK)
			                                            ON		MB.MOTIVO = LB1.MOTIVO
			                                            WHERE	MB.MOTIVO <> '13'
			                                            AND		DTFIM IS NOT NULL
			                                            AND		STR(MB.NUM_FUNC) + CONVERT(VARCHAR(20),(CONVERT(DATE, DTINI))) IN (
										                                            SELECT	STR(NUM_FUNC) + CONVERT(VARCHAR(20), MAX(CONVERT(DATE, DTINI)))
										                                            FROM	LY_LICENCA_DOCENTE (NOLOCK) 
										                                            GROUP BY NUM_FUNC)
			                                            ) ULC	ON ULC.NUM_FUNC = DD.NUM_FUNC
                                            WHERE         
                                            --LOTAÇÃO EM FUNÇÃO DE REGÊNCIA              
	                                            DD.CONCURSO IS NULL
	                                            AND DD.CANDIDATO IS NULL
                                            	      
                                            --TEM CARGA LIVRE SOBRANDO                        
                                            AND CASE WHEN MT.MOTIVO = '43'  THEN (CH.CARGAHORARIAREGENCIA/2 - isnull(SS.ATUACAO,0))
	                                            ELSE (CH.CARGAHORARIAREGENCIA - isnull(SS.ATUACAO,0))  END  > 0
                                                  
                                            -- NÃO TEM LICENÇA DE CARGA HORÁRIA REDUZIDA                    
                                            AND		EXISTS (
				                                            SELECT	1
				                                            FROM	LY_UNIDADE_ENSINO_SITUACAO US (NOLOCK)
				                                            WHERE	US.UNIDADE_ENS = EU.UNIDADE_ENS
				                                            AND		US.SITUACAO = 'ESTADUAL'
				                                            AND		US.UNIDADE_ENS+CONVERT(VARCHAR(20),US.DT_SITUACAO) IN 
																				                                            (
																				                                            SELECT	UNIDADE_ENS+CONVERT(VARCHAR(20),MAX(DT_SITUACAO)) 
																				                                            FROM LY_UNIDADE_ENSINO_SITUACAO (NOLOCK) GROUP BY UNIDADE_ENS
																				                                            )
				                                            )
                                            AND		(EU.SIT_FUNCIONAMENTO IS NULL OR EU.SIT_FUNCIONAMENTO = 'EMATIVIDADE')      
                                            AND		ISNULL(DD.VOLUNTARIO,'N') = 'N'
                                            AND (EU.ID_REGIONAL = @coord or @coord is null)      
                                            AND (EU.MUNICIPIO = @MUNICIPIO or @MUNICIPIO is null)        
                                            AND (EU.UNIDADE_ENS = @escola or @escola is null)    
                                            AND EU.SETOR = @SETOR  
                                            ) AS S           
                                                  
                                            --NÃO EXISTE LICENÇA DIFERENTE DA CH REDUZIDA (43)      
                                            where   NOT EXISTS (
					                                            SELECT	1  
					                                            FROM	LY_LICENCA_DOCENTE (NOLOCK) 
					                                            WHERE	S.NUM_FUNC = NUM_FUNC 
					                                            AND		MOTIVO <> '43' 
					                                            AND		(DTFIM IS NULL OR DTFIM >= CONVERT(DATE, CURRENT_TIMESTAMP))
					                                            )
					                                            ";

                contextQuery.Parameters.Add("@SETOR", SqlDbType.VarChar, setor);


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

        public ValidacaoDados Valida(Entidades.AgenteResponsavel agenteResponsavel, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            RN.Lotacao rnLotacao = new Lotacao();
            LyLotacao lotacao = new LyLotacao();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (agenteResponsavel == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (agenteResponsavel.AgenteResponsavelId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (agenteResponsavel.Setor.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE ADMINISTRATIVA é obrigatório.");
            }

            if (agenteResponsavel.Matricula.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo MATRICULA é obrigatório.");
            }

            if (agenteResponsavel.DataNomeacao <= DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DE DESIGNAÇÃO é obrigatório.");
            }
            else
            {

                if (agenteResponsavel.DataNomeacao.Date > DateTime.Now.Date)
                {
                    mensagens.Add("A DATA DE DESIGNAÇÃO não pode ser maior que a data atual.");
                }

                if (agenteResponsavel.DataPublicacaoNomeacao != null && agenteResponsavel.DataPublicacaoNomeacao > DateTime.MinValue)
                {
                    if (Convert.ToDateTime(agenteResponsavel.DataPublicacaoNomeacao).Date < agenteResponsavel.DataNomeacao.Date)
                    {
                        mensagens.Add("A DATA DE PUBLICAÇÃO DA DESIGNAÇÃO não pode ser menor que a DATA DE DESIGNAÇÃO.");
                    }
                }

                if (agenteResponsavel.DataDispensa != null && agenteResponsavel.DataDispensa > DateTime.MinValue)
                {
                    if (Convert.ToDateTime(agenteResponsavel.DataDispensa).Date < agenteResponsavel.DataNomeacao.Date)
                    {
                        mensagens.Add("A DATA DE DISPENSA não pode ser menor que a DATA DE DESIGNAÇÃO.");
                    }
                }
            }

            if (agenteResponsavel.DataPublicacaoNomeacao != null && agenteResponsavel.DataPublicacaoNomeacao > DateTime.MinValue)
            {
                if (Convert.ToDateTime(agenteResponsavel.DataPublicacaoNomeacao).Date > DateTime.Now.Date)
                {
                    mensagens.Add("A DATA DE PUBLICAÇÃO DA DESIGNAÇÃO não pode ser maior que a data atual.");
                }
            }

            if (agenteResponsavel.DataDispensa != null && agenteResponsavel.DataDispensa > DateTime.MinValue)
            {
                if (Convert.ToDateTime(agenteResponsavel.DataDispensa).Date > DateTime.Now.Date)
                {
                    mensagens.Add("A DATA DE DISPENSA não pode ser maior que a data atual.");
                }

                if (agenteResponsavel.DataPublicacaoDispensa != null && agenteResponsavel.DataPublicacaoDispensa > DateTime.MinValue)
                {
                    if (agenteResponsavel.DataPublicacaoDispensa < agenteResponsavel.DataDispensa)
                    {
                        mensagens.Add("A DATA DE PUBLICAÇÃO DA DISPENSA não pode ser menor que a DATA DE DISPENSA.");
                    }
                }
            }

            if (agenteResponsavel.DataPublicacaoDispensa != null && agenteResponsavel.DataPublicacaoDispensa > DateTime.MinValue)
            {
                if (Convert.ToDateTime(agenteResponsavel.DataPublicacaoDispensa).Date > DateTime.Now.Date)
                {
                    mensagens.Add("A DATA DE PUBLICAÇÃO DA DISPENSA não pode ser maior que a data atual.");
                }
            }

            if (agenteResponsavel.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (cadastro)
                    {
                        lotacao = rnLotacao.ObtemLotacaoAtivaPor(agenteResponsavel.Matricula);

                        if (lotacao.Matricula == null)
                        {
                            mensagens.Add("Servidor sem lotação ativa");
                        }
                    }


                    //Verifica se já existe um agente responsavel vigente para o setor
                    if ((agenteResponsavel.DataDispensa == null || agenteResponsavel.DataDispensa <= DateTime.MinValue) && this.PossuiOutroAgenteVigentePor(contexto, agenteResponsavel.Setor, agenteResponsavel.AgenteResponsavelId))
                    {
                        mensagens.Add("Esse funcionário não pode ser designado Responsável, pois não consta DATA DA DISPENSA cadastrada para o responsável anterior");
                    }
                    else
                    {
                        //Verifica se a data de nomeação está intercalada com outro agente
                        //if (this.PossuiDataNomeacaoEmOutroIntervaloPor(contexto, agenteResponsavel.Setor, agenteResponsavel.DataNomeacao, agenteResponsavel.AgenteResponsavelId))
                        //{
                        //    mensagens.Add("DATA DE DESIGNAÇÃO não pode estar dentro do intervalo de outro agente responsável.");
                        //}

                        //Verifica se não possui data de dispensa
                        if (agenteResponsavel.DataDispensa != null && agenteResponsavel.DataDispensa > DateTime.MinValue)
                        {
                            //Verifica se a data de dispensa está intercalada com outro agente
                            //if (this.PossuiDataDispensaEmOutroIntervaloPor(contexto, agenteResponsavel.Setor, Convert.ToDateTime(agenteResponsavel.DataDispensa), agenteResponsavel.AgenteResponsavelId))
                            //{
                            //    mensagens.Add("DATA DE DISPENSA não pode estar dentro do intervalo de outro agente responsável.");
                            //}

                            //Verifica se as datas de nomeação e de dispensa estão intercalada com outro agente
                            //if (this.PossuiOutraIntercaladaPor(contexto, agenteResponsavel.Setor, agenteResponsavel.DataNomeacao, Convert.ToDateTime(agenteResponsavel.DataDispensa), agenteResponsavel.AgenteResponsavelId))
                            //{
                            //    mensagens.Add("DATA DE DESIGNAÇÃO E DISPENSA da lotacao anterior não podem intercalar com outro agente responsável.");
                            //}
                        }
                        else {
                            //Verifica se as datas de nomeação anteriores são menores da enviada
                            //if (this.PossuiDataNomeacaoEmOutroMenor(contexto, agenteResponsavel.Setor, agenteResponsavel.DataNomeacao))
                            //{
                            //    mensagens.Add("DATA DE DESIGNAÇÃO não pode ser inferior a uma já cadastrada por um outro agente responsável.");
                            //}

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

        private bool PossuiOutroAgenteVigentePor(DataContext ctx, string setor, int agenteResponsavelId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM [Patrimonio].[AGENTERESPONSAVEL] (NOLOCK)
                                    WHERE  SETOR = @SETOR 
                                           AND AGENTERESPONSAVELID <> @AGENTERESPONSAVELID
                                           AND DATANOMEACAO <= GETDATE() 
                                           AND ( DATADISPENSA IS NULL 
                                                  OR DATADISPENSA >= GETDATE() )  ";

            contextQuery.Parameters.Add("@SETOR", SqlDbType.VarChar, setor);
            contextQuery.Parameters.Add("@AGENTERESPONSAVELID", SqlDbType.Int, agenteResponsavelId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiDataNomeacaoEmOutroIntervaloPor(DataContext ctx, string setor, DateTime data, int agenteResponsavelId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM  [Patrimonio].[AGENTERESPONSAVEL]  (NOLOCK)
                                WHERE  SETOR = @SETOR 
	                                AND AGENTERESPONSAVELID <> @AGENTERESPONSAVELID
	                                AND DATANOMEACAO <=@DATA
                                    AND DATADISPENSA > @DATA ";

//             AND @DATA BETWEEN DATANOMEACAO AND 
//			                                CONVERT(DATE, CONVERT(DATETIME,DATADISPENSA)) ";


            contextQuery.Parameters.Add("@SETOR", SqlDbType.VarChar, setor);
            contextQuery.Parameters.Add("@AGENTERESPONSAVELID", SqlDbType.Int, agenteResponsavelId);
            contextQuery.Parameters.Add("@DATA", SqlDbType.Date, data.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiDataNomeacaoEmOutroMenor(DataContext ctx, string setor, DateTime data)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                        FROM   [Patrimonio].[AGENTERESPONSAVEL] 
                        WHERE  SETOR = @SETOR 
                                 and DATANOMEACAO > @DATA ";

            contextQuery.Parameters.Add("@SETOR", setor);
            contextQuery.Parameters.Add("@DATA", data.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiDataDispensaEmOutroIntervaloPor(DataContext ctx, string setor, DateTime data, int agenteResponsavelId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                        FROM   [Patrimonio].[AGENTERESPONSAVEL] 
                        WHERE  SETOR = @SETOR 
                                AND AGENTERESPONSAVELID <> @AGENTERESPONSAVELID
                                AND @DATA BETWEEN 
                                    CONVERT(DATE, CONVERT(DATETIME,DATANOMEACAO)) AND CONVERT( 
                                    DATE, CONVERT(DATE,isnull(DATADISPENSA,getdate())))  ";

            contextQuery.Parameters.Add("@SETOR", setor);
            contextQuery.Parameters.Add("@AGENTERESPONSAVELID", agenteResponsavelId);
            contextQuery.Parameters.Add("@DATA", data.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiOutraIntercaladaPor(DataContext ctx, string setor, DateTime dataNomeacao, DateTime dataDispensa, int agenteResponsavelId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                            FROM   [Patrimonio].[AGENTERESPONSAVEL]  (NOLOCK)
                            WHERE SETOR = @SETOR 
                                    AND AGENTERESPONSAVELID <> @AGENTERESPONSAVELID
                                    AND @DATANOMEACAO <= CONVERT(DATE, DATANOMEACAO) 
                                    AND @DATADISPENSA >= CONVERT(DATE, CONVERT(DATETIME,DATADISPENSA)) ";

            contextQuery.Parameters.Add("@SETOR", setor);
            contextQuery.Parameters.Add("@AGENTERESPONSAVELID", agenteResponsavelId);
            contextQuery.Parameters.Add("@DATANOMEACAO", dataNomeacao.Date);
            contextQuery.Parameters.Add("@DATADISPENSA", dataDispensa.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public void Insere(Entidades.AgenteResponsavel agenteResponsavel)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO PATRIMONIO.AGENTERESPONSAVEL 
                                                (SETOR, 
                                                 MATRICULA, 
                                                 DATANOMEACAO, 
                                                 DATADISPENSA, 
                                                 DATAPUBLICACAONOMEACAO, 
                                                 DATAPUBLICACAODISPENSA, 
                                                 USUARIOID, 
                                                 DATACADASTRO, 
                                                 DATAALTERACAO) 
                                    VALUES      (@SETOR, 
                                                 @MATRICULA, 
                                                 @DATANOMEACAO, 
                                                 @DATADISPENSA, 
                                                 @DATAPUBLICACAONOMEACAO, 
                                                 @DATAPUBLICACAODISPENSA, 
                                                 @USUARIOID, 
                                                 @DATACADASTRO, 
                                                 @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@SETOR", SqlDbType.VarChar, agenteResponsavel.Setor);
                contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, agenteResponsavel.Matricula);
                contextQuery.Parameters.Add("@DATANOMEACAO", SqlDbType.Date, agenteResponsavel.DataNomeacao.Date);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, agenteResponsavel.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                if (agenteResponsavel.DataDispensa != null && agenteResponsavel.DataDispensa > DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATADISPENSA", SqlDbType.Date, Convert.ToDateTime(agenteResponsavel.DataDispensa).Date);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATADISPENSA", SqlDbType.Date, null);
                }

                if (agenteResponsavel.DataPublicacaoNomeacao != null && agenteResponsavel.DataPublicacaoNomeacao > DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAPUBLICACAONOMEACAO", SqlDbType.Date, Convert.ToDateTime(agenteResponsavel.DataPublicacaoNomeacao).Date);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAPUBLICACAONOMEACAO", SqlDbType.Date, null);
                }

                if (agenteResponsavel.DataPublicacaoDispensa != null && agenteResponsavel.DataPublicacaoDispensa > DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAPUBLICACAODISPENSA", SqlDbType.Date, Convert.ToDateTime(agenteResponsavel.DataPublicacaoDispensa).Date);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAPUBLICACAODISPENSA", SqlDbType.Date, null);
                }

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

        public void Atualiza(Entidades.AgenteResponsavel agenteResponsavel)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE Patrimonio.AGENTERESPONSAVEL
                                    SET    DATADISPENSA = @DATADISPENSA, 
		                                   DATAPUBLICACAODISPENSA = @DATAPUBLICACAODISPENSA,
                                           DATANOMEACAO = @DATANOMEACAO,
                                           DATAPUBLICACAONOMEACAO = @DATAPUBLICACAONOMEACAO,
		                                   USUARIOID = @USUARIOID, 
		                                   DATAALTERACAO = @DATAALTERACAO 
                                    WHERE  AGENTERESPONSAVELID = @AGENTERESPONSAVELID ";

                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, agenteResponsavel.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@AGENTERESPONSAVELID", SqlDbType.Int, agenteResponsavel.AgenteResponsavelId);
                contextQuery.Parameters.Add("@DATANOMEACAO", SqlDbType.Date, agenteResponsavel.DataNomeacao.Date);

                if (agenteResponsavel.DataDispensa != null && agenteResponsavel.DataDispensa > DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATADISPENSA", SqlDbType.Date, Convert.ToDateTime(agenteResponsavel.DataDispensa).Date);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATADISPENSA", SqlDbType.Date, null);
                }

                if (agenteResponsavel.DataPublicacaoDispensa != null && agenteResponsavel.DataPublicacaoDispensa > DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAPUBLICACAODISPENSA", SqlDbType.Date, Convert.ToDateTime(agenteResponsavel.DataPublicacaoDispensa).Date);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAPUBLICACAODISPENSA", SqlDbType.Date, null);
                }
                if (agenteResponsavel.DataPublicacaoNomeacao != null && agenteResponsavel.DataPublicacaoNomeacao > DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAPUBLICACAONOMEACAO", SqlDbType.Date, Convert.ToDateTime(agenteResponsavel.DataPublicacaoNomeacao).Date);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAPUBLICACAONOMEACAO", SqlDbType.Date, null);
                }


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

        public void Remove(int agenteResponsavelId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE Patrimonio.AGENTERESPONSAVEL
                            WHERE  AGENTERESPONSAVELID = @AGENTERESPONSAVELID  ";

                contextQuery.Parameters.Add("@AGENTERESPONSAVELID", SqlDbType.Int, agenteResponsavelId);

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
