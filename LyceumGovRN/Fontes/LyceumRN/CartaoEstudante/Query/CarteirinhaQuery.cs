using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.CartaoEstudante.Query
{
    class CarteirinhaQuery : QueryBase<CarteirinhaQuery>
    {


        internal System.Data.DataTable ListarCarteirinhas(string aluno)
        {
            var sql = new StringBuilder();

            sql.Append(string.Format(@"SELECT	CASE WHEN RC.RETORNOCARTAOID IS NULL THEN C.ALUNO ELSE RC.ALUNO END AS ALUNO
		                        ,CASE WHEN RC.RETORNOCARTAOID IS NULL THEN C.COD_BARRAS_CARTEIRINHA ELSE RC.NUMEROCHIP END AS COD_BARRAS_CARTEIRINHA
		                        ,CASE WHEN RC.RETORNOCARTAOID IS NULL THEN NULL ELSE RC.NUMEROCARTAO END AS NUMEROCARTAO
		                        ,CASE WHEN RC.RETORNOCARTAOID IS NULL THEN C.SIT_CARTEIRINHA ELSE TSC.DESCRICAO END AS SIT_CARTEIRINHA
		                        ,CASE WHEN RC.RETORNOCARTAOID IS NULL THEN C.MOTIVO ELSE TC.DESCRICAO END AS MOTIVO
		                        ,CASE WHEN RC.RETORNOCARTAOID IS NULL THEN C.DT_SOLICITACAO ELSE NULL END AS DT_SOLICITACAO
		                        ,CASE WHEN RC.RETORNOCARTAOID IS NULL THEN NULL ELSE RC.NUMEROLOTE END AS NUMEROLOTE
		                        ,CASE WHEN RC.RETORNOCARTAOID IS NULL THEN NULL ELSE RC.DATAUTILIZACAO END AS DATAUTILIZACAO
		                        ,CASE WHEN RC.LOCALIMPRESSAO = '1' THEN 'Bureau' WHEN RC.LOCALIMPRESSAO = '2' THEN 'Biometria' WHEN RC.LOCALIMPRESSAO IS NULL THEN 'Não Definido' ELSE 'Não Identificado' END AS LOCALIMPRESSAO
		                        ,CASE WHEN RC.RETORNOCARTAOID IS NULL THEN NULL ELSE RC.DATAENTREGALOTE END AS DATAENTREGALOTE
		                        ,CASE WHEN RC.RETORNOCARTAOID IS NULL THEN NULL ELSE RC.DATACONFIRMACAOENTREGA END AS DATACONFIRMACAOENTREGA
		                        ,CASE WHEN RC.RETORNOCARTAOID IS NULL THEN C.STAMP_ATUALIZACAO ELSE RC.DATAINCLUSAO END AS DATA_ALT_SITUACAO
		                        ,CASE WHEN RC.RETORNOCARTAOID IS NULL THEN C.DT_IMPRESSAO ELSE RC.DATAIMPRESSAO END AS DT_IMPRESSAO
                        FROM (
			                        SELECT T3.COD_BARRAS_CARTEIRINHA, MAX(T3.DATA_ALT_SITUACAO) AS DATA_ALT_SITUACAO
			                        FROM (  SELECT
						                        C.COD_BARRAS_CARTEIRINHA
						                        , CASE WHEN C.DATA_ALT_SITUACAO IS NULL THEN C.STAMP_ATUALIZACAO ELSE C.DATA_ALT_SITUACAO END DATA_ALT_SITUACAO
						                        FROM (SELECT COD_BARRAS_CARTEIRINHA, MAX(STAMP_ATUALIZACAO) AS STAMP_ATUALIZACAO
						                        FROM DBO.LY_CARTEIRINHA (NOLOCK)
						                        GROUP BY COD_BARRAS_CARTEIRINHA
						                        ) AS T1
						                        JOIN LY_CARTEIRINHA C (NOLOCK) ON T1.COD_BARRAS_CARTEIRINHA = C.COD_BARRAS_CARTEIRINHA
						                        AND T1.STAMP_ATUALIZACAO = C.STAMP_ATUALIZACAO
						                        WHERE ALUNO = '{0}' 
					                        UNION 
						                        SELECT
						                        RC.NUMEROCHIP
						                        , RC.DATAINCLUSAO
						                        FROM ( SELECT
								                        NUMEROCHIP
								                        , MAX(DATAINCLUSAO) AS STAMP_ATUALIZACAO
								                        FROM CartaoEstudante.RETORNOCARTAO (NOLOCK)
								                        GROUP BY NUMEROCHIP
							                         ) AS T2
							                        JOIN CartaoEstudante.RETORNOCARTAO RC (NOLOCK) ON T2.NUMEROCHIP = RC.NUMEROCHIP
							                        AND T2.STAMP_ATUALIZACAO = RC.DATAINCLUSAO
							                        JOIN CartaoEstudante.TIPOSITUACAOCARTAO TSC (NOLOCK) ON TSC.TIPOSITUACAOCARTAOID = RC.TIPOSITUACAOCARTAOID
							                        JOIN CartaoEstudante.TIPOCANCELAMENTO TC (NOLOCK) ON TC.TIPOCANCELAMENTOID = RC.TIPOCANCELAMENTOID
							                        WHERE RC.ALUNO = '{0}' 
					                        ) AS T3
			                        GROUP BY T3.COD_BARRAS_CARTEIRINHA
	                        ) AS T4
	                        LEFT JOIN CartaoEstudante.RETORNOCARTAO RC (NOLOCK) 
		                        ON RC.NUMEROCHIP = T4.COD_BARRAS_CARTEIRINHA AND RC.DATAINCLUSAO = T4.DATA_ALT_SITUACAO
	                        LEFT JOIN dbo.LY_CARTEIRINHA C (NOLOCK) 
		                        ON C.COD_BARRAS_CARTEIRINHA = T4.COD_BARRAS_CARTEIRINHA	AND T4.DATA_ALT_SITUACAO = ISNULL(C.DATA_ALT_SITUACAO, C.STAMP_ATUALIZACAO)
	                        LEFT JOIN CartaoEstudante.TIPOSITUACAOCARTAO TSC (NOLOCK) ON TSC.TIPOSITUACAOCARTAOID = RC.TIPOSITUACAOCARTAOID
	                        LEFT JOIN CartaoEstudante.TIPOCANCELAMENTO TC (NOLOCK) ON TC.TIPOCANCELAMENTOID = RC.TIPOCANCELAMENTOID "
                            ,aluno));

            return base.ObterDataTable(sql.ToString());
        }
    }
}
