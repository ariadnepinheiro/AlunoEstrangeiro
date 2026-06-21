using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.Certificacao
{
   public class TipoConclusao
    {
       public DataTable Listar()
       {
           DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
           ContextQuery contextQuery = new ContextQuery();
           DataTable dt = null;

           try
           {
               contextQuery.Command = @" 
                                        SELECT [TIPOCONCLUSAOID]
                                              ,[DESCRICAO]
                                              
                                          FROM [CertificacaoEscolar].[TIPOCONCLUSAO]
                                         WHERE [ATIVO]=1

                                       ";

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


       public DataTable Listar(string aluno)
       {
           DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
           ContextQuery contextQuery = new ContextQuery();
           DataTable dt = null;

           try
           {
               contextQuery.Command = @" 
                                       WITH MEDIA_TURMA_HIST (BIMESTRE, ALUNO, ANO, SEMESTRE, TURMA, DISCIPLINA, MEDIA) 
                      AS (SELECT PV.SUBPERIODO                   BIMESTRE, 
                         LN.ALUNO, 
                          LT.ANO, 
                          LT.SEMESTRE, 
                          LT.TURMA, 
                          LT.DISCIPLINA 
                         -- INCLUIDA PRA TRATAR CONVERSAO DE VARCHAR PARA NUMERIC: ROBSON 
                                       , 
                                        CONVERT(DECIMAL(7, 4), ( CASE ISNUMERIC(LN.CONCEITO) 
                                                             WHEN 1 THEN Replace(LN.CONCEITO, ',', 
                                                                            '.') 
                                                                           WHEN 0 THEN ( CASE LN.CONCEITO 
                                                                                   WHEN 'SN' THEN '0.0' 
                                                                                   WHEN NULL THEN '0.0' 
                                                                                   ELSE '0.0' 
                                                                                 END ) 
                                                                 END )) AS MEDIA 
                                 --CONVERT(DECIMAL(7,4), REPLACE(ISNULL(REPLACE(LN.CONCEITO, 'SN', 0), 0), ',', '.')) AS MEDIA 
								   
                                 FROM   LY_NOTA_HISTMATR LN (NOLOCK) 
                                        JOIN LY_PROVA PV (NOLOCK) 
                                          ON LN.DISCIPLINA = PV.DISCIPLINA 
                                             AND LN.TURMA = PV.TURMA 
                                             AND LN.ANO = PV.ANO 
                                             AND LN.SEMESTRE = PV.SEMESTRE 
                                             AND LN.NOTA_ID = PV.PROVA 
                                        JOIN LY_HISTMATRICULA LM (NOLOCK) 
                                          ON LM.ANO = LN.ANO 
                                             AND LM.TURMA = LN.TURMA 
                                             AND LM.DISCIPLINA = LN.DISCIPLINA 
                                             AND LM.SEMESTRE = LN.SEMESTRE 
                                             AND LM.SITUACAO_HIST <> 'CANCELADO' 
                                             AND ISNULL(LM.MAIS_EDUCACAO, 'N') = 'N' 
                                             AND ISNULL(LM.EDUC_ESPECIAL, 'N') = 'N' 
                                             AND ISNULL(LM.CONCOMITANTE, 'N') = 'N' 
                                             AND ISNULL(LM.OPTATIVAREFORCO, 'N') = 'N' 
                                        --AND LM.ANO = @p_ano  
                                        --AND LM.SEMESTRE = @p_periodo 
                                        JOIN LY_TURMA LT (NOLOCK) 
                                          ON LT.ANO = LM.ANO 
                                             AND LT.TURMA = LM.TURMA 
                                             AND LT.DISCIPLINA = LM.DISCIPLINA 
                                             AND LT.SEMESTRE = LM.SEMESTRE 
                                             AND LT.SIT_TURMA <> 'DESATIVADA' 
                                        JOIN LY_CURSO LC (NOLOCK) 
                                          ON( LT.CURSO = LC.CURSO ) 
                                        JOIN CERTIFICACAOESCOLAR.TIPOCONCLUSAO_MODALIDADETIPO TM (NOLOCK 
                                             ) 
                                          ON ( TM.TIPO = LC.TIPO 
                                               AND TM.MODALIDADE = LC.MODALIDADE ) 
                                 WHERE  PV.SUBPERIODO IS NOT NULL 
                                        AND LN.ALUNO = @aluno                                        
                                 GROUP  BY PV.SUBPERIODO, 
                                           LN.ALUNO, 
                                           LT.ANO, 
                                           LT.SEMESTRE, 
                                           LT.TURMA, 
                                           LT.DISCIPLINA, 
                                           LN.CONCEITO), 
                             MEDIA_FALTA_HIST (BIMESTRE, ALUNO, ANO, SEMESTRE, TURMA, DISCIPLINA, FALTAS 
                             , AULAS_DADAS) 
                             AS (SELECT FR.SUBPERIODO BIMESTRE,  
		                        LF.ALUNO,  
		                        LT.ANO,  
		                        LT.SEMESTRE,  
		                        LT.TURMA,  
		                        LT.DISCIPLINA,  
		                        LF.FALTAS FALTAS,  
		                        FR.AULAS_DADAS  
                                 FROM   LY_FALTA_HISTMATR LF (NOLOCK) 
                                        JOIN LY_FREQ FR (NOLOCK) 
                                          ON LF.DISCIPLINA = FR.DISCIPLINA 
                                             AND LF.TURMA = FR.TURMA 
                                             AND LF.ANO = FR.ANO 
                                             AND LF.FREQ_ID = FR.FREQ 
                                             AND LF.SEMESTRE = FR.PERIODO 
                                        JOIN LY_HISTMATRICULA LM (NOLOCK) 
                                          ON LM.ANO = LF.ANO 
                                             AND LM.TURMA = LF.TURMA 
                                             AND LM.DISCIPLINA = LF.DISCIPLINA 
                                             AND LM.SEMESTRE = LF.SEMESTRE 
                                             AND LM.SITUACAO_HIST <> 'CANCELADO' 
                                             AND ISNULL(LM.MAIS_EDUCACAO, 'N') = 'N' 
                                             AND ISNULL(LM.EDUC_ESPECIAL, 'N') = 'N' 
                                             AND ISNULL(LM.CONCOMITANTE, 'N') = 'N' 
                                             AND ISNULL(LM.OPTATIVAREFORCO, 'N') = 'N' 
                                        --AND LM.ANO = @p_ano  
                                        --AND LM.SEMESTRE = @p_periodo 
                                        JOIN LY_TURMA LT (NOLOCK) 
                                          ON LT.ANO = LM.ANO 
                                             AND LT.TURMA = LM.TURMA 
                                             AND LT.DISCIPLINA = LM.DISCIPLINA 
                                             AND LT.SEMESTRE = LM.SEMESTRE 
                                             AND LT.SIT_TURMA <> 'DESATIVADA' 
                                        JOIN LY_CURSO LC (NOLOCK) 
                                          ON( LT.CURSO = LC.CURSO ) 
                                        JOIN CERTIFICACAOESCOLAR.TIPOCONCLUSAO_MODALIDADETIPO TM (NOLOCK 
                                             ) 
                                          ON ( TM.TIPO = LC.TIPO 
                                               AND TM.MODALIDADE = LC.MODALIDADE ) 
                                 WHERE  FR.SUBPERIODO IS NOT NULL 
                                        and LM.ALUNO = @aluno                                         						               
                                 GROUP  BY FR.SUBPERIODO, 
                                           LF.ALUNO, 
                                           LT.ANO, 
                                           LT.SEMESTRE, 
                                           LT.TURMA, 
                                           LT.DISCIPLINA, 
                                           LF.FALTAS, 
                                           FR.AULAS_DADAS) 

select distinct TIPOCONCLUSAOID,
	   DESCRICAO FROM (						
					SELECT  
                    TC.DESCRICAO +' '+  MDC.DESCRICAO  AS MODALIDADENIVEL,lue.SETOR as UA,
                    LM.ALUNO,  
		                    LT.SERIE,  
		                    LT.TURMA,   
		                    LT.TURNO, 
		                    LT.CURSO,LC.MODALIDADE,LC.TIPO_CURSO,
		                    LT.CURRICULO, 
		                    LM.ANO,  
		                    LM.SEMESTRE,  
		                    LUE.UNIDADE_ENS,  
		                    LUE.NOME_COMP AS ESCOLA,  
		                    LUE.ENDERECO,
		                    MU.NOME AS MUNICIPIO,
		                    G.DESCRICAO AS GRUPODISCIPLINA,
		                    G.AGRUPAMENTO,
		                    UPPER (TSFA.SITUACAO_FINAL) SITUACAO,
                            LM.SITUACAO_HIST,
							tp.TIPOCONCLUSAOID,
							tp.DESCRICAO,
		                    ISNULL(LD.PERC_PRESMIN, 0) * 100 AS PERCENTUAL_PRESENCA_MIN,
		                    (ISNULL(LD.HORAS_ATIV, 0) + ISNULL(LD.HORAS_AULA, 0) + ISNULL(LD.HORAS_ESTAGIO, 0) + ISNULL(LD.HORAS_LAB, 0)) AS CARGA_HORARIA,  
		                    ISNULL(LT.DISCIPLINA_MULTIPLA, LT.DISCIPLINA) AS DISCIPLINA, LD.NOME_COMPL AS NOME_DISC,
		                    (SELECT MT.MEDIA FROM MEDIA_TURMA_HIST MT WHERE MT.ALUNO = LM.ALUNO AND MT.ANO = LM.ANO AND MT.SEMESTRE = LM.SEMESTRE AND MT.DISCIPLINA = LM.DISCIPLINA AND MT.TURMA = LM.TURMA AND MT.BIMESTRE = 1) AS NOTA_1B,
		                    (SELECT MT.MEDIA FROM MEDIA_TURMA_HIST MT WHERE MT.ALUNO = LM.ALUNO AND MT.ANO = LM.ANO AND MT.SEMESTRE = LM.SEMESTRE AND MT.DISCIPLINA = LM.DISCIPLINA AND MT.TURMA = LM.TURMA AND MT.BIMESTRE = 2) AS NOTA_2B,  
		                    (SELECT MT.MEDIA FROM MEDIA_TURMA_HIST MT WHERE MT.ALUNO = LM.ALUNO AND MT.ANO = LM.ANO AND MT.SEMESTRE = LM.SEMESTRE AND MT.DISCIPLINA = LM.DISCIPLINA AND MT.TURMA = LM.TURMA AND MT.BIMESTRE = 3) AS NOTA_3B,
		                    (SELECT MT.MEDIA FROM MEDIA_TURMA_HIST MT WHERE MT.ALUNO = LM.ALUNO AND MT.ANO = LM.ANO AND MT.SEMESTRE = LM.SEMESTRE AND MT.DISCIPLINA = LM.DISCIPLINA AND MT.TURMA = LM.TURMA AND MT.BIMESTRE = 4) AS NOTA_4B,
		                    (SELECT SUM(ISNULL(MT.MEDIA, 0)) / COUNT(*)
                        FROM MEDIA_TURMA_HIST MT WHERE MT.ALUNO = LM.ALUNO AND MT.ANO = LM.ANO AND MT.SEMESTRE = LM.SEMESTRE AND MT.DISCIPLINA = LM.DISCIPLINA AND MT.TURMA = LM.TURMA) AS NOTA_GERAL,  
		                    (SELECT SUM(ISNULL(MT.MEDIA, 0)) FROM MEDIA_TURMA_HIST MT WHERE MT.ALUNO = LM.ALUNO AND MT.ANO = LM.ANO AND MT.SEMESTRE = LM.SEMESTRE AND MT.DISCIPLINA = LM.DISCIPLINA AND MT.TURMA = LM.TURMA) AS TOTAL_PONTOS,
		                    (SELECT SUM(ISNULL(MF.FALTAS , 0))/ COUNT(*)
                        FROM MEDIA_FALTA_HIST MF WHERE MF.ALUNO = LM.ALUNO AND MF.ANO = LM.ANO AND MF.SEMESTRE = LM.SEMESTRE AND MF.DISCIPLINA = LM.DISCIPLINA AND MF.TURMA = LM.TURMA AND MF.BIMESTRE = 1) AS FALTA_1B,
		                    (SELECT SUM(ISNULL(MF.FALTAS , 0))/ COUNT(*)
                        FROM MEDIA_FALTA_HIST MF WHERE MF.ALUNO = LM.ALUNO AND MF.ANO = LM.ANO AND MF.SEMESTRE = LM.SEMESTRE AND MF.DISCIPLINA = LM.DISCIPLINA AND MF.TURMA = LM.TURMA AND MF.BIMESTRE = 2) AS FALTA_2B,  
		                    (SELECT SUM(ISNULL(MF.FALTAS , 0))/ COUNT(*)
                        FROM MEDIA_FALTA_HIST MF WHERE MF.ALUNO = LM.ALUNO AND MF.ANO = LM.ANO AND MF.SEMESTRE = LM.SEMESTRE AND MF.DISCIPLINA = LM.DISCIPLINA AND MF.TURMA = LM.TURMA AND MF.BIMESTRE = 3) AS FALTA_3B,  
		                    (SELECT SUM(ISNULL(MF.FALTAS , 0))/ COUNT(*)
                        FROM MEDIA_FALTA_HIST MF WHERE MF.ALUNO = LM.ALUNO AND MF.ANO = LM.ANO AND MF.SEMESTRE = LM.SEMESTRE AND MF.DISCIPLINA = LM.DISCIPLINA AND MF.TURMA = LM.TURMA AND MF.BIMESTRE = 4) AS FALTA_4B,  
		                    (SELECT SUM(ISNULL(MF.FALTAS, 0))  FROM MEDIA_FALTA_HIST MF WHERE MF.ALUNO = LM.ALUNO AND MF.ANO = LM.ANO AND MF.SEMESTRE = LM.SEMESTRE AND MF.DISCIPLINA = LM.DISCIPLINA AND MF.TURMA = LM.TURMA) AS FALTA_GERAL,
		                    (SELECT SUM(ISNULL(MT.AULAS_DADAS, 0)) FROM MEDIA_FALTA_HIST MT WHERE MT.ALUNO = LM.ALUNO AND MT.ANO = LM.ANO AND MT.SEMESTRE = LM.SEMESTRE AND MT.DISCIPLINA = LM.DISCIPLINA AND MT.TURMA = LM.TURMA) AS AULAS_DADAS,
		                    (SELECT ISNULL(CONVERT(DECIMAL(5,2), (((SUM(ISNULL(AULAS_DADAS, 0)) - SUM(ISNULL(MF.FALTAS, 0))) * 100) / SUM(ISNULL(AULAS_DADAS, 0)))), 100) FROM MEDIA_FALTA_HIST MF WHERE MF.ALUNO = LM.ALUNO AND MF.ANO = LM.ANO AND MF.SEMESTRE = LM.SEMESTRE AND MF.DISCIPLINA = LM.DISCIPLINA AND MF.TURMA = LM.TURMA) AS PERCENTUAL_PRESENCA  
                    FROM	LY_HISTMATRICULA LM (NOLOCK) 
                    JOIN	LY_ALUNO LA (NOLOCK) ON LM.ALUNO = LA.ALUNO 
				                    AND LM.SITUACAO_HIST <> 'CANCELADO' 
				                    and ISNULL(LM.MAIS_EDUCACAO, 'N') = 'N'
				                    and ISNULL(LM.EDUC_ESPECIAL, 'N') = 'N'
				                    and ISNULL( LM.CONCOMITANTE, 'N') = 'N'
				                    and ISNULL( LM.OPTATIVAREFORCO, 'N') = 'N'
                    JOIN	TCE_SITUACAO_FINAL_ALUNO TSFA (NOLOCK) ON TSFA.ALUNO = LM.ALUNO 
				                    AND TSFA.ANO = LM.ANO
				                    AND TSFA.PERIODO = LM.SEMESTRE
				                    AND	TSFA.TURMA = LM.TURMA
				                    AND ISNULL(LM.DEPENDENCIA, 'N') = 'N'
				                    and TSFA.situacao_final in ('Aprovado', 'Aprovado Com Dep','Promovido')
                    JOIN	LY_TURMA LT (NOLOCK) ON LM.ANO = LT.ANO 
				                    AND LM.SEMESTRE = LT.SEMESTRE 
				                    AND LM.TURMA = LT.TURMA 
				                    AND LM.DISCIPLINA = LT.DISCIPLINA 
                    JOIN	LY_UNIDADE_ENSINO LUE (NOLOCK) ON LUE.UNIDADE_ENS = LT.UNIDADE_RESPONSAVEL 
                    JOIN    MUNICIPIO MU (NOLOCK) ON LUE.MUNICIPIO = MU.CODIGO
                    JOIN	LY_DISCIPLINA LD (NOLOCK) ON LD.DISCIPLINA = ISNULL(LT.DISCIPLINA_MULTIPLA, LT.DISCIPLINA) 
                    JOIN	LY_CURSO LC (NOLOCK) ON LC.CURSO = LT.CURSO 
                    JOIN	CertificacaoEscolar.TIPOCONCLUSAO_MODALIDADETIPO tm (nolock) on (tm.TIPO = LC.TIPO and tm.MODALIDADE = lc.MODALIDADE )
                    LEFT JOIN LY_GRUPO_HABILITACAO_DISC GD (NOLOCK) ON  LD.DISCIPLINA = GD.DISCIPLINA 
                    LEFT JOIN LY_GRUPO_HABILITACAO G (NOLOCK) ON GD.AGRUPAMENTO = G.AGRUPAMENTO
                    LEFT JOIN LY_MODALIDADE_CURSO MDC(NOLOCK) ON (MDC.MODALIDADE=LC.MODALIDADE AND MDC.MODALIDADE=TM.MODALIDADE)
                    LEFT JOIN LY_TIPO_CURSO  TC ON ( TC.TIPO=LC.TIPO)
					join CertificacaoEscolar.TIPOCONCLUSAO tp on (tp.TIPOCONCLUSAOID = tm.TIPOCONCLUSAOID)  
                    WHERE  LM.ALUNO = @aluno                            
			 ) q1

                                       ";

               contextQuery.Parameters.Add("@aluno", SqlDbType.VarChar, aluno);

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

    }
}
