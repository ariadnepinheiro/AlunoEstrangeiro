using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Web;
using System.IO;
using Techne.Lyceum.RN.Certificacao.Entidades;
using Techne.Lyceum.RN.Certificacao.DTOs;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.Certificacao
{
    public class HistoricoEscolar
    {
        public HistoricoEscolar(){ }


        /// <summary>
        ///  Lista os tipos da conclusão
        /// </summary>
        /// <returns></returns>
        public static DataTable listarTipoConclusao()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable retorno = null;
            try
            {
                contextQuery.Command = @"SELECT tp.TIPOCONCLUSAOID,
	                                            tp.DESCRICAO,
	                                            tp.ATIVO
                                        FROM CertificacaoEscolar.TIPOCONCLUSAO tp
                                        WHERE tp.ATIVO = 1 ";                
                retorno = contexto.GetDataTable(contextQuery);
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
            return retorno;             
        }

        
        /// <summary>
        ///     retorna a validação de dados do histórico escolar se um aluno possui ou não os tipos de ensino (fundamental, médio e profissionalizante)
        /// </summary>
        /// <param name="matriculaAluno"></param>
        /// <param name="tipoConclusaoID"></param>
        /// <param name="tipoDocumentoID"></param>
        /// <returns></returns>       
        public ValidacaoDados ValidaHistorico(string pessoa, int tipoConclusaoID,  int? tipoDocumentoID, string observacao)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.Aluno rnAluno = new Aluno();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };
                       
            if (string.IsNullOrEmpty(pessoa))
               mensagens.Add("Campo ALUNO é obrigatório");
            
            if (tipoConclusaoID == -1)
               mensagens.Add("Campo NÍVEL é obrigatório.");

            if (tipoDocumentoID != null)
            {
                if (tipoDocumentoID == -1)
                    mensagens.Add("Campo DOCUMENTO é obrigatório.");
            }
                      
                if (observacao.Length > 200)
                    mensagens.Add("Campo OBSERVAÇÃO DO HISTÓRICO ESCOLAR só poderá conter no máximo 200 caracteres");
            
            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                    if (!this.possuiNivelPor(contexto, pessoa, tipoConclusaoID))
                    {
                        mensagens.Add("Este aluno não possui histórico para este nível selecionado ...");
                    }

                    //Verifica se o aluno possui mais de uma aprovação na mesma serie
                    if (this.PossuiDuplicidadeSeriePor(contexto, pessoa, tipoConclusaoID))
                    {
                        string tipoConclusao = string.Empty;
                        if (tipoConclusaoID == 1)
                        {
                            tipoConclusao = "ENSINO FUNDAMENTAL";
                        }
                        else if (tipoConclusaoID == 2)
                        {
                            tipoConclusao = "ENSINO MÉDIO";
                        }
                        else if (tipoConclusaoID == 3)
                        {
                            tipoConclusao = "ENSINO PROFISSIONALIZANTE";
                        }

                        //Busca todas as matriculas da pessoa
                        string matricula = rnAluno.ListaMatriculasPor(contexto, Convert.ToInt32(pessoa)).Aggregate((x, y) => x + ", " + y);

                        mensagens.Add(string.Format("Este aluno possui mais de uma aprovação na mesma série do {0} na(s) sua(s) matrícula(s): {1}, será necessário ajustar o aluno antes de gerar o histórico.", tipoConclusao, matricula));
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
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + "</BR>" + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        private bool PossuiDuplicidadeSeriePor(DataContext contexto, string pessoaID, int tipoConclusaoID)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*)
								FROM (SELECT DISTINCT LT.SERIE
                                    FROM   LY_HISTMATRICULA LM (NOLOCK) 
                                           JOIN LY_ALUNO LA (NOLOCK) 
                                             ON LM.ALUNO = LA.ALUNO 
                                           JOIN TCE_SITUACAO_FINAL_ALUNO TSFA (NOLOCK) 
                                             ON TSFA.ALUNO = LM.ALUNO 
                                                AND TSFA.ANO = LM.ANO 
                                                AND TSFA.PERIODO = LM.SEMESTRE 
                                                AND TSFA.TURMA = LM.TURMA 
                                                AND ISNULL(LM.DEPENDENCIA, 'N') = 'N' 
                                                AND TSFA.SITUACAO_FINAL IN ('Aprovado', 'Aprovado Com Dep','Promovido') 
                                           JOIN LY_TURMA LT (NOLOCK) 
                                             ON LM.ANO = LT.ANO 
                                                AND LM.SEMESTRE = LT.SEMESTRE 
                                                AND LM.TURMA = LT.TURMA 
                                                AND LM.DISCIPLINA = LT.DISCIPLINA 
                                           JOIN LY_CURSO LC (NOLOCK) 
                                             ON LC.CURSO = LT.CURSO 
                                           JOIN CERTIFICACAOESCOLAR.TIPOCONCLUSAO_MODALIDADETIPO TM (NOLOCK) 
                                             ON TM.TIPO = LC.TIPO 
                                                AND TM.MODALIDADE = LC.MODALIDADE
                                    WHERE  LA.PESSOA = @PESSOAID 
                                           AND TM.TIPOCONCLUSAOID = @TIPOCONCLUSAOID 
                                           AND ISNULL(LM.MAIS_EDUCACAO, 'N') = 'N' 
                                           AND ISNULL(LM.EDUC_ESPECIAL, 'N') = 'N' 
                                           AND ISNULL(LM.CONCOMITANTE, 'N') = 'N' 
                                           AND ISNULL(LM.OPTATIVAREFORCO, 'N') = 'N' 
                                           AND LM.SITUACAO_HIST <> 'CANCELADO' 
                                           AND LT.CURSO NOT IN ('0002.37','0001.27') -- RETIRADO ATE DEFINICAO DA SITUAÇÃO DO CES
                                            AND LT.CURSO NOT IN ('0091.29','0091.30','0091.31','0092.39')
                                    GROUP  BY LT.SERIE 
									HAVING COUNT(DISTINCT CONVERT(VARCHAR(4), LM.ANO) + CONVERT(VARCHAR(1), LM.SEMESTRE)) > 1) AS T ";

            contextQuery.Parameters.Add("@PESSOAID", pessoaID);
            contextQuery.Parameters.Add("@TIPOCONCLUSAOID", tipoConclusaoID);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

       /// <summary>
       /// Retorno true, caso o documento do tipo solicitado, possuir autorização.
       /// </summary>
       /// <param name="matriculaAluno"></param>
       /// <param name="tipoConclusaoID"></param>
       /// <param name="tipoDocumentoID"></param>
       /// <returns></returns>
        public bool ValidaAutorizacao(string matriculaAluno, int tipoConclusaoID, int tipoDocumentoID)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @"SELECT COUNT(0) EXISTE 
                                         FROM [CertificacaoEscolar].[DOCUMENTOCERTIFICACAO] 
                                         WHERE 
                                             DOCUMENTOID=@DOCUMENTOID and 
                                             TIPOCONCLUSAOID=@TIPOCONCLUSAOID and
                                             ALUNO=@ALUNO AND
                                             AUTORIZADO=1
                                             ";

            //verificar o aluno,modalidade,nivel
            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, matriculaAluno);
            contextQuery.Parameters.Add("@DOCUMENTOID", SqlDbType.Int, tipoDocumentoID);
            contextQuery.Parameters.Add("@TIPOCONCLUSAOID", SqlDbType.Int, tipoConclusaoID);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;

        }

        public bool possuiNivelPor(string pessoa, int tipoconclusao)
        {
            Seeduc.Infra.Data.DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            bool retorno = false;

            try
            {
                retorno = this.possuiNivelPor(contexto, pessoa,tipoconclusao);
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
                contexto.Dispose();
            }
        }

        public bool possuiNivelPor(DataContext contexto, string pessoa, int tipoconclusao)
        {
            ContextQuery ctxNivel = new ContextQuery();
            bool achouNivel = false;

            #region query
            ctxNivel.Command = @" 
     SELECT count(distinct(q1.aluno))  AS POSSUI FROM (					SELECT  
                    
                    LM.ALUNO
		                    
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
                    
                     
                    JOIN	LY_CURSO LC (NOLOCK) ON LC.CURSO = LT.CURSO 
                    JOIN	CertificacaoEscolar.TIPOCONCLUSAO_MODALIDADETIPO tm (nolock) on (tm.TIPO = LC.TIPO and tm.MODALIDADE = lc.MODALIDADE )
                    
                    WHERE  LA.PESSOA = @PESSOA
                           AND TM.TIPOCONCLUSAOID = @TIPOCONCLUSAOID ) q1 ";
            #endregion query

            ctxNivel.Parameters.Add("@PESSOA", SqlDbType.VarChar, pessoa);
            ctxNivel.Parameters.Add("@TIPOCONCLUSAOID", SqlDbType.Int, tipoconclusao);

            if (contexto.GetReturnValue<int>(ctxNivel) > 0)
                achouNivel = true;
            return achouNivel;
        }


        
        /// <summary>
        ///     retorna um objeto do tipo DocumentoCertificacao verificando se este possui uma observação, caso tenha, está deverá ser preenchida na tela
        /// </summary>
        /// <param name="matriculaAluno"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public RN.Certificacao.Entidades.DocumentoCertificacao retornaObservacaoHistorico(string matriculaAluno, string pessoaID, int tipo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery ctxHistoricoEscolar = new ContextQuery();
            
            RN.Certificacao.Entidades.DocumentoCertificacao doc = new Techne.Lyceum.RN.Certificacao.Entidades.DocumentoCertificacao();

            try
            {
                ctxHistoricoEscolar.Command = @"  SELECT * FROM CertificacaoEscolar.DOCUMENTOCERTIFICACAO DC ( NOLOCK )                                               
                                                  WHERE DC.TIPOCONCLUSAOID = @TIPOCONCLUSAOID
                                                  AND (DC.ALUNO = @ALUNO OR dc.PESSOA = @PESSOA OR dc.PESSOA = null)  
                                                  AND DC.DOCUMENTOID = 1 ";

                ctxHistoricoEscolar.Parameters.Add("@ALUNO", SqlDbType.VarChar, matriculaAluno);
                ctxHistoricoEscolar.Parameters.Add("@TIPOCONCLUSAOID", SqlDbType.Int, tipo);
                ctxHistoricoEscolar.Parameters.Add("@PESSOA", SqlDbType.VarChar, pessoaID);
                
                doc = ctx.TryToBindEntity<RN.Certificacao.Entidades.DocumentoCertificacao>(ctxHistoricoEscolar);
                return doc;              
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
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="matricula"></param>
        /// <param name="tipoConclusaoID"></param>
        /// <returns></returns>
        public List<HistoricoEscolarDTO> listarAnoSemestrePor(string pessoaID, int tipoConclusaoID)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery ctxQuery = new ContextQuery();
            SqlDataReader reader = null;
            List<HistoricoEscolarDTO> listAnoSemestre = new List<HistoricoEscolarDTO>();
            try
            {

                ctxQuery.Command = @" SELECT DISTINCT LT.SERIE, 
                                                    LM.ANO, 
                                                    LM.SEMESTRE 
                                    FROM   LY_HISTMATRICULA LM (NOLOCK) 
                                           JOIN LY_ALUNO LA (NOLOCK) 
                                             ON LM.ALUNO = LA.ALUNO 
                                           JOIN TCE_SITUACAO_FINAL_ALUNO TSFA (NOLOCK) 
                                             ON TSFA.ALUNO = LM.ALUNO 
                                                AND TSFA.ANO = LM.ANO 
                                                AND TSFA.PERIODO = LM.SEMESTRE 
                                                AND TSFA.TURMA = LM.TURMA 
                                                AND ISNULL(LM.DEPENDENCIA, 'N') = 'N' 
                                                AND TSFA.SITUACAO_FINAL IN ('Aprovado', 'Aprovado Com Dep','Promovido') 
                                           JOIN LY_TURMA LT (NOLOCK) 
                                             ON LM.ANO = LT.ANO 
                                                AND LM.SEMESTRE = LT.SEMESTRE 
                                                AND LM.TURMA = LT.TURMA 
                                                AND LM.DISCIPLINA = LT.DISCIPLINA 
                                           JOIN LY_CURSO LC (NOLOCK) 
                                             ON LC.CURSO = LT.CURSO 
                                           JOIN CERTIFICACAOESCOLAR.TIPOCONCLUSAO_MODALIDADETIPO TM (NOLOCK) 
                                             ON TM.TIPO = LC.TIPO 
                                                AND TM.MODALIDADE = LC.MODALIDADE
                                    WHERE  LA.PESSOA = @PESSOAID 
                                           AND TM.TIPOCONCLUSAOID = @TIPOCONCLUSAOID 
                                           AND ISNULL(LM.MAIS_EDUCACAO, 'N') = 'N' 
                                           AND ISNULL(LM.EDUC_ESPECIAL, 'N') = 'N' 
                                           AND ISNULL(LM.CONCOMITANTE, 'N') = 'N' 
                                           AND ISNULL(LM.OPTATIVAREFORCO, 'N') = 'N' 
                                           AND LM.SITUACAO_HIST <> 'CANCELADO' 
                                            AND LT.CURSO NOT IN ('0091.29','0091.30','0091.31','0092.39')
                                           AND LT.CURSO NOT IN ('0002.37','0001.27') -- RETIRADO ATE DEFINICAO DA SITUAÇÃO DO CES
                                    ORDER  BY ANO, 
                                              SERIE, 
                                              SEMESTRE ";

                ctxQuery.Parameters.Add("@PESSOAID", pessoaID);
                ctxQuery.Parameters.Add("@TIPOCONCLUSAOID", tipoConclusaoID);
                reader = ctx.GetDataReader(ctxQuery);

                while (reader.Read())
                {

                    Certificacao.DTOs.HistoricoEscolarDTO heDTO = new HistoricoEscolarDTO();

                    heDTO.Ano = reader["ANO"].ToString().Trim();
                    heDTO.Serie = reader["SERIE"].ToString().Trim();                    
                    heDTO.Semestre = reader["SEMESTRE"].ToString().Trim();

                    listAnoSemestre.Add(heDTO);
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
            return listAnoSemestre;
        }
        


        /// <summary>
        ///     Retorna informações do histórico do aluno com o tipo de conclusao e sua matrícula
        /// </summary>
        /// <param name="matricula"></param>
        /// <param name="tipoConclusaoID"></param>
        /// <returns></returns>
        public List<HistoricoEscolarDTO> listarDisciplinasHistoricoEscolarPor(string matricula, int tipoConclusaoID, string ano, string serie)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery ctxQuery = new ContextQuery();
            SqlDataReader reader = null;
            List<HistoricoEscolarDTO> listAllHistoricoEscolar = new List<HistoricoEscolarDTO>();
            
            #region query
            try 
            {
                ctxQuery.Command = @"
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
                                        JOIN CERTIFICACAOESCOLAR.TIPOCONCLUSAO_MODALIDADETIPO TM (NOLOCK) 
                                          ON ( TM.TIPO = LC.TIPO AND TM.MODALIDADE = LC.MODALIDADE )
                                        JOIN	LY_ALUNO LA (NOLOCK) ON LM.ALUNO = LA.ALUNO 
				                        AND LM.SITUACAO_HIST <> 'CANCELADO' 
				                        and ISNULL(LM.MAIS_EDUCACAO, 'N') = 'N'
				                        and ISNULL(LM.EDUC_ESPECIAL, 'N') = 'N'
				                        and ISNULL( LM.CONCOMITANTE, 'N') = 'N'
				                        and ISNULL( LM.OPTATIVAREFORCO, 'N') = 'N'   
                                 WHERE  PV.SUBPERIODO IS NOT NULL 
                                        AND LA.PESSOA = @PESSOA
                                        AND TM.TIPOCONCLUSAOID = @TIPOCONCLUSAOID 
										and LM.ANO = @ANO 
										and lm.SERIE = @SERIE 
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
                                        and LM.ALUNO = @MATRICULA 
                                        AND TM.TIPOCONCLUSAOID = @TIPOCONCLUSAOID
						                AND LM.ANO = @ANO 
						                AND lm.SERIE = @SERIE
                                 GROUP  BY FR.SUBPERIODO, 
                                           LF.ALUNO, 
                                           LT.ANO, 
                                           LT.SEMESTRE, 
                                           LT.TURMA, 
                                           LT.DISCIPLINA, 
                                           LF.FALTAS, 
                                           FR.AULAS_DADAS) 

select NOME_DISC, NOTA_1B, NOTA_2B, NOTA_3B, NOTA_4B, NOTA_GERAL, CARGA_HORARIA, PERCENTUAL_PRESENCA, SITUACAO, SITUACAO_HIST FROM (
						
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
                    WHERE  LM.ALUNO = @MATRICULA 
                           AND TM.TIPOCONCLUSAOID = @TIPOCONCLUSAOID
						   and LM.ANO = @ANO 
						   and lm.SERIE = @SERIE ) q1
		    group by NOME_DISC, NOTA_1B, NOTA_2B, NOTA_3B, NOTA_4B, NOTA_GERAL, CARGA_HORARIA, PERCENTUAL_PRESENCA, SITUACAO, SITUACAO_HIST ";
            #endregion query

                ctxQuery.Parameters.Add("@MATRICULA",matricula);
                ctxQuery.Parameters.Add("@TIPOCONCLUSAOID",tipoConclusaoID);
                ctxQuery.Parameters.Add("@ANO", ano);
                ctxQuery.Parameters.Add("@SERIE", serie);
                reader = ctx.GetDataReader(ctxQuery);

                while (reader.Read())
                {
                    Certificacao.DTOs.HistoricoEscolarDTO heDTO = new HistoricoEscolarDTO();
                    heDTO.Disciplina = reader["NOME_DISC"].ToString().Trim();

                    heDTO.Nota_1b = (reader["NOTA_1B"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["NOTA_1B"].ToString()));
                    heDTO.Nota_2b = (reader["NOTA_2B"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["NOTA_2B"].ToString()));
                    heDTO.Nota_3b = (reader["NOTA_3B"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["NOTA_3B"].ToString()));
                    heDTO.Nota_4b = (reader["NOTA_4B"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["NOTA_4B"].ToString()));                    
                    heDTO.PercentualPresenca = (reader["PERCENTUAL_PRESENCA"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["PERCENTUAL_PRESENCA"].ToString()));

                    if ((reader["NOTA_1B"] == DBNull.Value)
                    && (reader["NOTA_2B"] == DBNull.Value)
                    && (reader["NOTA_3B"] == DBNull.Value)
                    && (reader["NOTA_4B"] == DBNull.Value))                    
                       heDTO.Situacao_hist = "--";
                    else
                       heDTO.Situacao_hist = reader["SITUACAO_HIST"].ToString();                   
                    
                    heDTO.CargaHoraria = (reader["CARGA_HORARIA"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["CARGA_HORARIA"].ToString()));

                    listAllHistoricoEscolar.Add(heDTO);                    
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }

            return listAllHistoricoEscolar;

        }



        /// <summary>
        ///  Obtém dados de uma determinada escola que o aluno estuda
        /// </summary>
        /// <param name="matricula"></param>
        /// <param name="tipoConclusaoID"></param>
        /// <returns></returns>
        public HistoricoEscolarDTO obterDadosDaEscolaPor(string pessoaID, int tipoConclusaoID)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery ctxQuery = new ContextQuery();
            SqlDataReader reader = null;
            HistoricoEscolarDTO heDto = new HistoricoEscolarDTO();

            #region query
            try
            {
                ctxQuery.Command = @"SELECT DISTINCT TOP 1 TC.DESCRICAO + ' ' + MDC.DESCRICAO AS MODALIDADENIVEL, 
                                                    LUE.SETOR AS UA, 
                                                    LUE.UNIDADE_ENS, 
                                                    LUE.NOME_COMP AS ESCOLA, 
                                                    (LUE.ENDERECO + ' ' + ISNULL(LUE.END_NUM, '') + ' ' + ISNULL(LUE.END_COMPL, '')) AS ENDERECO, 
                                                    (MU.NOME + '/' + MU.UF_SIGLA) AS MUNICIPIO, 
                                                    LUE.CEP, 
                                                    ES.NUMERO_ATO_OFICIAL, 
                                                    ES.DT_DOU, 
                                                    ES.ATO_OFICIAL, 
                                                    ES.ORDEM,
													LM.ANO,
													LM.SEMESTRE 
                                    FROM   LY_HISTMATRICULA LM (NOLOCK) 
                                           JOIN LY_ALUNO LA (NOLOCK) 
                                             ON LM.ALUNO = LA.ALUNO 
	                                       JOIN	TCE_SITUACAO_FINAL_ALUNO TSFA (NOLOCK) 
                                             ON TSFA.ALUNO = LM.ALUNO 
			                                    AND TSFA.ANO = LM.ANO
			                                    AND TSFA.PERIODO = LM.SEMESTRE
			                                    AND	TSFA.TURMA = LM.TURMA
			                                    AND ISNULL(LM.DEPENDENCIA, 'N') = 'N'
			                                    and TSFA.situacao_final in ('Aprovado', 'Aprovado Com Dep','Promovido')
                                           JOIN LY_TURMA LT (NOLOCK) 
                                             ON LM.ANO = LT.ANO 
                                                AND LM.SEMESTRE = LT.SEMESTRE 
                                                AND LM.TURMA = LT.TURMA 
                                                AND LM.DISCIPLINA = LT.DISCIPLINA 
                                           JOIN LY_UNIDADE_ENSINO LUE (NOLOCK) 
                                             ON LUE.UNIDADE_ENS = LT.UNIDADE_RESPONSAVEL 
                                           LEFT JOIN LY_UNIDADE_ENSINO_SITUACAO (NOLOCK) ES 
                                             ON LUE.UNIDADE_ENS = ES.UNIDADE_ENS 
                                             AND ( ES.ATO_OFICIAL = 'Criacao' OR ES.ATO_OFICIAL = 'CRIACAO' ) 
                                           JOIN MUNICIPIO MU (NOLOCK) 
                                             ON LUE.MUNICIPIO = MU.CODIGO 
                                           JOIN LY_CURSO LC (NOLOCK) 
                                             ON LC.CURSO = LT.CURSO 
                                           JOIN CERTIFICACAOESCOLAR.TIPOCONCLUSAO_MODALIDADETIPO TM (NOLOCK) 
                                             ON ( TM.TIPO = LC.TIPO 
                                                  AND TM.MODALIDADE = LC.MODALIDADE ) 
                                           LEFT JOIN LY_MODALIDADE_CURSO MDC(NOLOCK) 
                                                  ON ( MDC.MODALIDADE = LC.MODALIDADE 
                                                       AND MDC.MODALIDADE = TM.MODALIDADE ) 
                                           LEFT JOIN LY_TIPO_CURSO TC 
                                                  ON ( TC.TIPO = LC.TIPO ) 
                                    WHERE  LA.PESSOA = @PESSOAID 
                                           AND TM.TIPOCONCLUSAOID = @TIPOCONCLUSAOID 
                                           AND LM.SITUACAO_HIST <> 'CANCELADO' 
                                           AND ISNULL(LM.MAIS_EDUCACAO, 'N') = 'N' 
                                           AND ISNULL(LM.EDUC_ESPECIAL, 'N') = 'N' 
                                           AND ISNULL(LM.CONCOMITANTE, 'N') = 'N' 
                                           AND ISNULL(LM.OPTATIVAREFORCO, 'N') = 'N'
                                    ORDER BY LM.ANO DESC, LM.SEMESTRE DESC ";

            #endregion query

                ctxQuery.Parameters.Add("@PESSOAID", pessoaID);
                ctxQuery.Parameters.Add("@TIPOCONCLUSAOID", tipoConclusaoID);              
                reader = ctx.GetDataReader(ctxQuery);

                while (reader.Read())
                {          
                    Certificacao.DTOs.HistoricoEscolarDTO he = new HistoricoEscolarDTO();
                    heDto.Ua = reader["UA"].ToString();            
                    heDto.Unidade_ens = reader["UNIDADE_ENS"].ToString();
                    heDto.Escola = reader["ESCOLA"].ToString();
                    heDto.Endereco = reader["ENDERECO"].ToString();
                    heDto.Municipio = reader["MUNICIPIO"].ToString();
                    heDto.Modalidadenivel = reader["MODALIDADENIVEL"].ToString();
                    heDto.Decreto = reader["NUMERO_ATO_OFICIAL"].ToString();
                    heDto.DataDO = Convert.ToDateTime( reader["DT_DOU"] == DBNull.Value ? null : reader["DT_DOU"] );
                    heDto.Cep = reader["CEP"].ToString();
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }

            return heDto;
        
        }


        /// <summary>
        ///  listar as escolas que o aluno estudou
        /// </summary>
        /// <param name="matricula"></param>
        /// <param name="tipoConclusaoID"></param>
        /// <returns></returns>
        public List<HistoricoEscolarDTO> listarVidaPregressaDoAlunoPor(string pessoaID, int tipoConclusaoID)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery ctxQuery = new ContextQuery();
            SqlDataReader reader = null;
            List<HistoricoEscolarDTO> listAllVidaPregressa = new List<HistoricoEscolarDTO>();

            #region query
            try
            {
                ctxQuery.Command = @" SELECT DISTINCT LT.SERIE, 
                                                    LM.ANO, 
                                                    LUE.UNIDADE_ENS, 
                                                    LUE.NOME_COMP AS ESCOLA, 
                                                    LUE.ENDERECO, 
                                                    (MU.NOME + '/' + MU.UF_SIGLA) AS MUNICIPIO 
                                    FROM   LY_HISTMATRICULA LM (NOLOCK) 
                                           JOIN LY_ALUNO LA (NOLOCK) 
                                             ON LM.ALUNO = LA.ALUNO 
                                           JOIN TCE_SITUACAO_FINAL_ALUNO TSFA (NOLOCK) 
                                             ON TSFA.ALUNO = LM.ALUNO 
                                                AND TSFA.ANO = LM.ANO 
                                                AND TSFA.PERIODO = LM.SEMESTRE 
                                                AND TSFA.TURMA = LM.TURMA 
                                                AND ISNULL(LM.DEPENDENCIA, 'N') = 'N' 
                                                AND TSFA.SITUACAO_FINAL IN ('Aprovado', 'Aprovado Com Dep','Promovido') 
                                           JOIN LY_TURMA LT (NOLOCK) 
                                             ON LM.ANO = LT.ANO 
                                                AND LM.SEMESTRE = LT.SEMESTRE 
                                                AND LM.TURMA = LT.TURMA 
                                                AND LM.DISCIPLINA = LT.DISCIPLINA 
                                           JOIN LY_UNIDADE_ENSINO LUE (NOLOCK) 
                                             ON LUE.UNIDADE_ENS = LT.UNIDADE_RESPONSAVEL 
                                           JOIN MUNICIPIO MU (NOLOCK) 
                                             ON LUE.MUNICIPIO = MU.CODIGO 
                                           JOIN LY_CURSO LC (NOLOCK) 
                                             ON LC.CURSO = LT.CURSO 
                                           JOIN CERTIFICACAOESCOLAR.TIPOCONCLUSAO_MODALIDADETIPO TM (NOLOCK) 
                                             ON TM.TIPO = LC.TIPO 
                                                AND TM.MODALIDADE = LC.MODALIDADE 
                                    WHERE  LA.PESSOA = @PESSOAID 
                                           AND TM.TIPOCONCLUSAOID = @TIPOCONCLUSAOID
                                           AND LM.SITUACAO_HIST <> 'CANCELADO' 
                                           AND ISNULL(LM.MAIS_EDUCACAO, 'N') = 'N' 
                                           AND ISNULL(LM.EDUC_ESPECIAL, 'N') = 'N' 
                                           AND ISNULL(LM.CONCOMITANTE, 'N') = 'N' 
                                           AND ISNULL(LM.OPTATIVAREFORCO, 'N') = 'N' 
                                           AND LT.CURSO NOT IN ('0002.37','0001.27') -- RETIRADO ATE DEFINICAO DA SITUAÇÃO DO CES
                                            AND LT.CURSO NOT IN ('0091.29','0091.30','0091.31','0092.39')
                                    ORDER  BY SERIE ASC ";
                
            #endregion query

                ctxQuery.Parameters.Add("@PESSOAID", pessoaID);
                ctxQuery.Parameters.Add("@TIPOCONCLUSAOID", tipoConclusaoID);
                reader = ctx.GetDataReader(ctxQuery);

                while (reader.Read())
                {
                    Certificacao.DTOs.HistoricoEscolarDTO heDTO = new HistoricoEscolarDTO();
                    heDTO.Ano = reader["ANO"].ToString();
                    heDTO.Serie = reader["SERIE"].ToString();
                    heDTO.Unidade_ens = reader["UNIDADE_ENS"].ToString();
                    heDTO.Escola = reader["ESCOLA"].ToString();
                    heDTO.Endereco = reader["ENDERECO"].ToString();
                    heDTO.Municipio = reader["MUNICIPIO"].ToString();
                    listAllVidaPregressa.Add(heDTO);
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }

            return listAllVidaPregressa;        
        
        }


        /// <summary>
        ///  Retornar verdeiro caso tenha sido inserido com sucesso
        /// </summary>
        /// <param name="contexto"></param>
        /// <param name="dadosCampanha"></param>
        /// <returns></returns>
        /// 
        private bool InsereDadosHistoricoEscolarPor(DataContext contexto, Entidades.DocumentoCertificacao histdoc)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            #region Query insert
            contextQuery.Command = @"INSERT INTO [CertificacaoEscolar].[DOCUMENTOCERTIFICACAO]
                                      ( [TIPOCONCLUSAOID],[ALUNO],[DOCUMENTOID],[NUMERO],[FOLHAS],[LIVRO],[OBSERVACAO]
                                      ,[USUARIOID],[DATACADASTRO],[DATAALTERACAO],[EIXO],[AUTORIZADO],[PESSOA] )
                                VALUES (@TIPOCONCLUSAOID,
                                        @ALUNO,
                                        @DOCUMENTOID,
                                        @NUMERO,
                                        @FOLHAS,
                                        @LIVRO,
                                        @OBSERVACAO,                                        
                                        @USUARIOID,
                                        @DATACADASTRO,
                                        @DATAALTERACAO,
                                        @EIXO,
                                        @AUTORIZADO,
                                        @PESSOA)
                                        SELECT IDENT_CURRENT('CertificacaoEscolar.DOCUMENTOCERTIFICACAO')";
            #endregion Query insert

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, histdoc.Aluno);
            contextQuery.Parameters.Add("@EIXO", SqlDbType.VarChar, null);
            contextQuery.Parameters.Add("@TIPOCONCLUSAOID", SqlDbType.Int, histdoc.TipoConclusaoId);
            contextQuery.Parameters.Add("@DOCUMENTOID", SqlDbType.Int, histdoc.DocumentoId);
            contextQuery.Parameters.Add("@NUMERO", SqlDbType.Int, null);
            contextQuery.Parameters.Add("@FOLHAS", SqlDbType.Int, null);
            contextQuery.Parameters.Add("@LIVRO", SqlDbType.Int, null);
            contextQuery.Parameters.Add("@OBSERVACAO", SqlDbType.VarChar, histdoc.Observacao);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, histdoc.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@AUTORIZADO", SqlDbType.Bit, 1);
            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, Convert.ToDecimal(histdoc.Pessoa));

            //retorna o último ID Gerado
            histdoc.DocumentoCertId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));

            if (histdoc.DocumentoCertId != 0)
                retorno = true;

            return retorno;
        }
        

        /// <summary>
        ///     Insere dados Histórico Escolar para tabela DocumentoCertificao
        /// </summary>
        /// <param name="dadosHistoricoEscolar">Objeto do tipo DocumentoCertificao</param>
        /// <returns></returns>
        public bool InsereDadosHistoricoEscolarPor(Entidades.DocumentoCertificacao dadosHistoricoEscolar)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            bool retorno = false;
            try
            {
                retorno = this.InsereDadosHistoricoEscolarPor(contexto, dadosHistoricoEscolar);
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
            return retorno;
        }



        /// <summary>
        ///     Atualiza dados dos hitórico escolar
        /// </summary>
        /// <param name="dadosCampanha"></param>
        /// <returns></returns>
        public bool AtualizaDadosHistoricoEscolar(Entidades.DocumentoCertificacao dadosHistoricoEscolar)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;
            try
            {
                contextQuery.Command = @"Update [CertificacaoEscolar].[DOCUMENTOCERTIFICACAO]
                                         set                                          
                                          [TIPOCONCLUSAOID] = @TIPOCONCLUSAOID
                                          ,[ALUNO] = @ALUNO
                                          ,[NUMERO] = @NUMERO
                                          ,[FOLHAS] = @FOLHAS
                                          ,[LIVRO] = @LIVRO
                                          ,[OBSERVACAO] = @OBSERVACAO
                                          ,[EIXO]=@EIXO
                                          ,[USUARIOID] = @USUARIOID                                         
                                          ,[DATAALTERACAO] = @DATAALTERACAO
                                          ,[PESSOA] = @PESSOA      
                                         where [DOCUMENTOCERTID] = @DOCUMENTOCERTID";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, dadosHistoricoEscolar.Aluno);
                contextQuery.Parameters.Add("@DOCUMENTOCERTID", SqlDbType.Int, dadosHistoricoEscolar.DocumentoCertId);
                contextQuery.Parameters.Add("@TIPOCONCLUSAOID", SqlDbType.Int, dadosHistoricoEscolar.TipoConclusaoId);
                contextQuery.Parameters.Add("@EIXO", SqlDbType.VarChar, dadosHistoricoEscolar.Eixo);
                contextQuery.Parameters.Add("@NUMERO", SqlDbType.Int, dadosHistoricoEscolar.Numero);
                contextQuery.Parameters.Add("@FOLHAS", SqlDbType.Int, dadosHistoricoEscolar.Folhas);
                contextQuery.Parameters.Add("@LIVRO", SqlDbType.Int, dadosHistoricoEscolar.Livro);
                contextQuery.Parameters.Add("@OBSERVACAO", SqlDbType.VarChar, dadosHistoricoEscolar.Observacao);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dadosHistoricoEscolar.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, Convert.ToDecimal(dadosHistoricoEscolar.Pessoa));

                contexto.ApplyModifications(contextQuery);
                retorno = true;
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
            return retorno;
        }



        /// <summary>
        ///     Verifica se histórico escolar foi gerado na tabela documentoGerado
        /// </summary>
        /// <param name="documentoCertID"></param>
        /// <returns></returns>
        public bool verificaSeHistoricoEscolarExisteNoDocGeradoPor(int documentoCertID)
        {
            ContextQuery contextQuery = new ContextQuery();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            int docID = 0;

            bool retorno = false;
            contextQuery.Command = @"SELECT 
                                         count(0) as total                                              
                                     FROM [CertificacaoEscolar].[DOCUMENTOGERADO] (NOLOCK)
                                        WHERE DOCUMENTOGERADO.DOCUMENTOCERTID = @DOCUMENTOCERTID";

            contextQuery.Parameters.Add("@DOCUMENTOCERTID", SqlDbType.Int, documentoCertID);
            
            docID = Convert.ToInt32(contexto.GetReturnValue(contextQuery));

            if (docID != 0)
                retorno = true;

            return retorno;        
        }


        /// <summary>
        ///     Método que retorna a lista de alunos que cumpriram uma determinada dependência de uma ou mais disciplinas
        /// </summary>
        /// <param name="matricula">matrícula do alunos</param>
        /// <param name="ano"> ano </param>
        /// <param name="semestre"> semestre / período </param>
        /// <returns></returns>
        public List<HistoricoEscolarDTO> listarAlunosQueRealizaramDepedenciaPor(string pessoaID, string ano, string semestre)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            List<HistoricoEscolarDTO> listAlunos = new List<HistoricoEscolarDTO>();
            try
            {
                
                #region query
                var contextQuery = new ContextQuery(
                    @"
                         SELECT ly_histmatricula.aluno as aluno,
                                LA.PESSOA AS PESSOA,
								ly_histmatricula.ano as ano,
                                ly_histmatricula.semestre as semestre,                                
                                ly_histmatricula.turma as turma,
                                REPLACE(ly_histmatricula.nota_final,',','.') nota_final,
                                UPPER(ly_histmatricula.situacao_hist) situacao_hist,                
                                ly_histmatricula.serie as serie,
                                ly_disciplina.nome as nomedisciplina,
                                ly_histmatricula.unidade_ensino as censo,                
                                ly_instituicao.nome_comp as unidade_ensino,					
                                CASE WHEN LY_HISTMATRICULA.DEPENDENCIA = 'S'
                                            AND LY_HISTMATRICULA.OPTATIVAREFORCO = 'N'
                                     THEN 'Dependência'            
                                    ELSE NULL
                                END DEPENDENCIA,
                                dr.nome as DISCIPLINA_REFERENCIA, 
                                ly_histmatricula.SERIE_REFERENCIA as serie_referencia
                         FROM   ly_histmatricula
                                        INNER JOIN ly_disciplina ON ly_disciplina.disciplina = ly_histmatricula.disciplina
                                        left JOIN ly_instituicao ON ly_instituicao.outra_faculdade = ly_histmatricula.UNIDADE_ENSINO 
                                        left JOIN ly_disciplina dr ON LY_HISTMATRICULA.DEPENDENCIA = 'S' and dr.disciplina = ly_histmatricula.DISCIPLINA_REFERENCIA
										INNER JOIN LY_ALUNO LA ON LY_HISTMATRICULA.ALUNO = LA.ALUNO 
	                     WHERE  ly_histmatricula.DEPENDENCIA = 'S' AND LY_HISTMATRICULA.OPTATIVAREFORCO = 'N' 
				                        AND LA.PESSOA = @PESSOAID										
                                        AND ANO = @ANO
                                        AND SEMESTRE = @SEMESTRE
                         ");
                #endregion query

                contextQuery.Parameters.Add("@PESSOAID", pessoaID);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                reader = ctx.GetDataReader(contextQuery);

                //Existe aluno que cumpriu a depedencia
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Certificacao.DTOs.HistoricoEscolarDTO heDTO = new HistoricoEscolarDTO();
                        heDTO.MatriculaAluno = reader["aluno"].ToString();
                        heDTO.Ano = reader["ano"].ToString();
                        heDTO.Pessoa = reader["Pessoa"].ToString();
                        heDTO.Unidade_ens = reader["censo"].ToString();
                        heDTO.Escola = reader["unidade_ensino"].ToString();
                        heDTO.Disciplina = reader["nomedisciplina"].ToString();
                        heDTO.Turma = reader["turma"].ToString();                        
                        heDTO.NotaFinal = ( reader["nota_final"] == DBNull.Value ? "null" : reader["nota_final"].ToString() );
                        heDTO.Situacao_hist = reader["situacao_hist"].ToString();
                        heDTO.Serie = reader["serie"].ToString();
                        heDTO.Dependencia = reader["DEPENDENCIA"].ToString();
                        heDTO.DisciplinaReferencia = reader["DISCIPLINA_REFERENCIA"].ToString();
                        heDTO.SerieReferencia = reader["serie_referencia"].ToString();
                        listAlunos.Add(heDTO);
                    }
                 }                
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
            return listAlunos;
        }


        public List<HistoricoEscolarDTO> RetornaAnosDeHistoricoPor(string pessoaID)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            List<HistoricoEscolarDTO> listAnos = new List<HistoricoEscolarDTO>();
            try
            {
                var contextQuery = new ContextQuery(@"SELECT DISTINCT la.pessoa, LM.ANO, lm.ALUNO, lm.SEMESTRE  
                                                        FROM [LYCEUM].[DBO].[LY_HISTMATRICULA] LM
                                                        JOIN	LY_ALUNO LA (NOLOCK) ON LM.ALUNO = LA.ALUNO 
                                                        AND LM.SITUACAO_HIST <> 'CANCELADO' 
                                                        and ISNULL(LM.MAIS_EDUCACAO, 'N') = 'N'
                                                        and ISNULL(LM.EDUC_ESPECIAL, 'N') = 'N'
                                                        and ISNULL( LM.CONCOMITANTE, 'N') = 'N'
                                                        and ISNULL( LM.OPTATIVAREFORCO, 'N') = 'N'
                                                        WHERE la.PESSOA = @PESSOAID AND LM.DEPENDENCIA = 'S' 
                                                        ORDER BY ANO DESC ");
 
                contextQuery.Parameters.Add("@PESSOAID", pessoaID );
                reader = ctx.GetDataReader(contextQuery);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Certificacao.DTOs.HistoricoEscolarDTO heDTO = new HistoricoEscolarDTO();
                        heDTO.Ano = reader["ano"].ToString();
                        heDTO.MatriculaAluno = reader["aluno"].ToString();
                        heDTO.Pessoa = reader["pessoa"].ToString();
                        heDTO.Semestre = reader["semestre"].ToString();
                        listAnos.Add(heDTO);
                    }
                }
                else
                    listAnos = null;
            }
            catch(Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                 if (reader != null)
                {
                    reader.Close();
                }
                    ctx.Dispose();
            }
            return listAnos;           
        }

        
        public List<HistoricoEscolarDTO> listarDisciplinasHistoricoEscolarPor(string pessoa, int tipoConclusaoID )
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery ctxQuery = new ContextQuery();
            SqlDataReader reader = null;
            List<HistoricoEscolarDTO> listAllHistoricoEscolar = new List<HistoricoEscolarDTO>();

            try
            {
                ctxQuery.Command = @" SELECT  DISTINCT
                                        LT.SERIE, 
					                    LM.ANO,  
					                    LM.SEMESTRE,
					                    GD.AGRUPAMENTO,
					                    LA.PESSOA,
					                    ENSINO_RELIGIOSO, 
					                    LINGUA_ESTRANGEIRA
					                    INTO #AGRUPAMENTOS
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
					                    INNER JOIN LY_CURRICULO CUR (NOLOCK) ON LT.CURSO = CUR.CURSO AND LT.TURNO = CUR.TURNO AND LT.CURRICULO = CUR.CURRICULO
                                        JOIN	LY_DISCIPLINA LD (NOLOCK) ON LD.DISCIPLINA = ISNULL(LT.DISCIPLINA_MULTIPLA, LT.DISCIPLINA) 
                                        JOIN	LY_CURSO LC (NOLOCK) ON LC.CURSO = LT.CURSO 
                                        JOIN	CertificacaoEscolar.TIPOCONCLUSAO_MODALIDADETIPO tm (nolock) on (tm.TIPO = LC.TIPO and tm.MODALIDADE = lc.MODALIDADE )
                                        LEFT JOIN LY_GRUPO_HABILITACAO_DISC GD (NOLOCK) ON  LD.DISCIPLINA = GD.DISCIPLINA 
                                        WHERE  LA.PESSOA = @PESSOA
                                           AND TM.TIPOCONCLUSAOID = @TIPOCONCLUSAOID  
                                           AND LT.CURSO NOT IN ('0002.37','0001.27') -- RETIRADO ATE DEFINICAO DA SITUAÇÃO DO CES
                                            AND LT.CURSO NOT IN ('0091.29','0091.30','0091.31','0092.39')
	                                    ORDER BY ANO, SERIE, SEMESTRE, AGRUPAMENTO, PESSOA
                    					
					                    --Adiciona lingua estrangueira para series que possuem
					                     INSERT INTO #AGRUPAMENTOS
					                     SELECT DISTINCT SERIE, ANO, SEMESTRE, 'LEOPT', PESSOA, '0', '1'
					                     FROM #AGRUPAMENTOS 
					                     WHERE LINGUA_ESTRANGEIRA = 'S'

					                     --Adiciona ensino religioso para series que possuem
					                     INSERT INTO #AGRUPAMENTOS
					                     SELECT DISTINCT SERIE, ANO, SEMESTRE, 'REOPT', PESSOA, '1', '0'
					                     FROM #AGRUPAMENTOS 
					                     WHERE ENSINO_RELIGIOSO = 'S'

					                     SELECT DISTINCT A.AGRUPAMENTO, 
												UPPER(G.DESCRICAO) AS GRUPODISCIPLINA
					                     FROM #AGRUPAMENTOS A
												INNER JOIN LY_GRUPO_HABILITACAO G (NOLOCK) ON A.AGRUPAMENTO = G.AGRUPAMENTO
										 ORDER BY GRUPODISCIPLINA ASC

					                     DROP TABLE #AGRUPAMENTOS ";

                ctxQuery.Parameters.Add("@PESSOA", pessoa);
                ctxQuery.Parameters.Add("@TIPOCONCLUSAOID", tipoConclusaoID);               
                reader = ctx.GetDataReader(ctxQuery);

                while (reader.Read())
                {
                    Certificacao.DTOs.HistoricoEscolarDTO heDTO = new HistoricoEscolarDTO();
                    heDTO.Agrupamento = reader["AGRUPAMENTO"].ToString().Trim();
                    heDTO.GrupoDisciplinas = reader["GRUPODISCIPLINA"].ToString().Trim();
                    listAllHistoricoEscolar.Add(heDTO);
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }

            return listAllHistoricoEscolar;

        }

       
        public HistoricoEscolarDTO obtemNotaPor(string pessoa, string ano, string periodo, string serie, string grupoDisciplina, int tipoConclusaoID)
        {
            Certificacao.DTOs.HistoricoEscolarDTO heDTO = new HistoricoEscolarDTO();

            try
            {
                //Verifica se é optativa de lingua estrangeira
                if (grupoDisciplina == "LEOPT")
                {
                    heDTO = this.obtemNotaTurmaOptativaPor(pessoa, ano, periodo, serie, grupoDisciplina, tipoConclusaoID, "L");
                }
                else if (grupoDisciplina == "REOPT") //Verifica se é optativa de ensino religioso
                {
                    heDTO = this.obtemNotaTurmaOptativaPor(pessoa, ano, periodo, serie, grupoDisciplina, tipoConclusaoID, "R");
                }
                else
                {
                    heDTO = this.obtemNotaTurmaPrincipalPor(pessoa, ano, periodo, serie, grupoDisciplina, tipoConclusaoID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return heDTO;
        }

        private HistoricoEscolarDTO obtemNotaTurmaPrincipalPor(string pessoa, string ano, string periodo, string serie, string grupoDisciplina, int tipoConclusaoID)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery ctxQuery = new ContextQuery();
            SqlDataReader reader = null;
            Certificacao.DTOs.HistoricoEscolarDTO heDTO = new HistoricoEscolarDTO();

            try
            {
                #region query
                ctxQuery.Command = @"
             SELECT  DISTINCT PESSOA, 
		LM.SITUACAO_HIST, 
		(ISNULL(LD.HORAS_ATIV, 0) + ISNULL(LD.HORAS_AULA, 0) + ISNULL(LD.HORAS_ESTAGIO, 0) + ISNULL(LD.HORAS_LAB, 0)) AS CARGA_HORARIA,
		(select sum(isnull(CONVERT(DECIMAL(7, 4), ( CASE ISNUMERIC(LN.CONCEITO) 
                                    WHEN 1 THEN Replace(LN.CONCEITO, ',', 
                                                '.') 
                                                WHEN 0 THEN ( CASE LN.CONCEITO 
                                                        WHEN 'SN' THEN '0.0' 
                                                        WHEN NULL THEN '0.0' 
                                                        ELSE '0.0' 
                                                        END ) 
                                        END )) , 0))								 
        FROM   LY_NOTA_HISTMATR LN (NOLOCK) 
            WHERE LN.DISCIPLINA = LM.DISCIPLINA 
                    AND LN.TURMA = LM.TURMA
                    AND LN.ANO = LM.ANO 
                    AND LN.SEMESTRE = LM.SEMESTRE 
                    AND LN.ALUNO = LM.ALUNO ) AS TOTAL_PONTOS 
FROM LY_ALUNO LA 
	INNER JOIN TCE_SITUACAO_FINAL_ALUNO TSFA (NOLOCK) ON TSFA.ALUNO = LA.ALUNO 
	INNER JOIN LY_HISTMATRICULA LM (NOLOCK) ON LM.ALUNO = LA.ALUNO
	INNER JOIN LY_TURMA LT (NOLOCK) 
                                          ON LT.ANO = LM.ANO 
                                             AND LT.TURMA = LM.TURMA 
                                             AND LT.DISCIPLINA = LM.DISCIPLINA 
                                             AND LT.SEMESTRE = LM.SEMESTRE                                              
	INNER JOIN LY_DISCIPLINA LD (NOLOCK) ON LD.DISCIPLINA = ISNULL(LT.DISCIPLINA_MULTIPLA, LT.DISCIPLINA) 
	LEFT JOIN LY_GRUPO_HABILITACAO_DISC GD (NOLOCK) ON  LD.DISCIPLINA = GD.DISCIPLINA 
	LEFT JOIN LY_GRUPO_HABILITACAO G (NOLOCK) ON GD.AGRUPAMENTO = G.AGRUPAMENTO
	INNER JOIN LY_CURSO LC (NOLOCK) ON LT.CURSO = LC.CURSO
    INNER JOIN CERTIFICACAOESCOLAR.TIPOCONCLUSAO_MODALIDADETIPO TM (NOLOCK) 
                                          ON TM.TIPO = LC.TIPO 
                                             AND TM.MODALIDADE = LC.MODALIDADE	 
WHERE PESSOA = @PESSOA
	AND LM.SITUACAO_HIST <> 'CANCELADO' 
	and ISNULL(LM.MAIS_EDUCACAO, 'N') = 'N'
	and ISNULL(LM.EDUC_ESPECIAL, 'N') = 'N'
	and ISNULL( LM.CONCOMITANTE, 'N') = 'N'
	and ISNULL( LM.OPTATIVAREFORCO, 'N') = 'N'
	AND ISNULL(LM.DEPENDENCIA, 'N') = 'N'
	and TSFA.situacao_final in ('Aprovado', 'Aprovado Com Dep','Promovido')
	AND LT.SIT_TURMA <> 'DESATIVADA'
	AND	lm.ano = @ANO
	And lm.SERIE = @SERIE
	AND G.AGRUPAMENTO = @AGRUPAMENTO
	AND TM.TIPOCONCLUSAOID = @TIPOCONCLUSAOID
     AND LT.CURSO NOT IN ('0091.29','0091.30','0091.31','0092.39')
	AND LT.CURSO NOT IN ('0002.37','0001.27') -- RETIRADO ATE DEFINICAO DA SITUAÇÃO DO CES ";
                #endregion query

                ctxQuery.Parameters.Add("@PESSOA", pessoa);
                ctxQuery.Parameters.Add("@ANO", ano);
                ctxQuery.Parameters.Add("@AGRUPAMENTO", grupoDisciplina);
                ctxQuery.Parameters.Add("@SERIE", serie);
                ctxQuery.Parameters.Add("@TIPOCONCLUSAOID", tipoConclusaoID);
                reader = ctx.GetDataReader(ctxQuery);

                while (reader.Read())
                {
                    heDTO.Nota_geral = Convert.ToDecimal(reader["TOTAL_PONTOS"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["TOTAL_PONTOS"].ToString()));
                    heDTO.CargaHoraria = Convert.ToDecimal( reader["CARGA_HORARIA"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["CARGA_HORARIA"].ToString() ));
                    heDTO.Situacao_hist = reader["SITUACAO_HIST"].ToString();
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }

            return heDTO;
        }

        private HistoricoEscolarDTO obtemNotaTurmaOptativaPor(string pessoa, string ano, string periodo, string serie, string grupoDisciplina, int tipoConclusaoID, string tipoOptativa)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery ctxQuery = new ContextQuery();
            SqlDataReader reader = null;
            Certificacao.DTOs.HistoricoEscolarDTO heDTO = new HistoricoEscolarDTO();

            try
            {
                #region query
                ctxQuery.Command = @" WITH MEDIA_TURMA_HIST (BIMESTRE, ALUNO, ANO, SEMESTRE, TURMA, DISCIPLINA, MEDIA) 
                     AS (SELECT PV.SUBPERIODO                   BIMESTRE, 
                                LN.ALUNO, 
                                LT.ANO, 
                                LT.SEMESTRE, 
                                LT.TURMA, 
                                LT.DISCIPLINA, 
                                CONVERT(DECIMAL(7, 4), ( CASE ISNUMERIC(LN.CONCEITO) 
                                                           WHEN 1 THEN REPLACE(LN.CONCEITO, ',', 
                                                                       '.') 
                                                           WHEN 0 THEN ( CASE LN.CONCEITO 
                                                                           WHEN 'SN' THEN '0.0' 
                                                                           WHEN NULL THEN '0.0' 
                                                                           ELSE '0.0' 
                                                                         END ) 
                                                         END )) AS MEDIA 
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
                                     AND LM.ALUNO = LN.ALUNO 
                                     AND LM.SITUACAO_HIST <> 'CANCELADO' 
                                     AND ISNULL(LM.MAIS_EDUCACAO, 'N') = 'N' 
                                     AND ISNULL(LM.EDUC_ESPECIAL, 'N') = 'N' 
                                     AND ISNULL(LM.CONCOMITANTE, 'N') = 'N' 
                                     AND ISNULL(LM.OPTATIVAREFORCO, 'N') = @TIPOOPTATIVA 
                                JOIN LY_TURMA LT (NOLOCK) 
                                  ON LT.ANO = LM.ANO 
                                     AND LT.TURMA = LM.TURMA 
                                     AND LT.DISCIPLINA = LM.DISCIPLINA 
                                     AND LT.SEMESTRE = LM.SEMESTRE 
                                     AND LT.SIT_TURMA <> 'DESATIVADA' 
                                JOIN LY_ALUNO LA (NOLOCK) 
                                  ON LM.ALUNO = LA.ALUNO 
                         WHERE  PV.SUBPERIODO IS NOT NULL 
                                AND LA.PESSOA = @PESSOA 
                                AND LM.ANO = @ANO 
                                AND LM.SERIE = @SERIE 
                                AND LM.OPTATIVAREFORCO = @TIPOOPTATIVA 
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
                                LF.FALTAS     FALTAS, 
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
                                     AND LM.ALUNO = LF.ALUNO 
                                     AND LM.SITUACAO_HIST <> 'CANCELADO' 
                                     AND ISNULL(LM.MAIS_EDUCACAO, 'N') = 'N' 
                                     AND ISNULL(LM.EDUC_ESPECIAL, 'N') = 'N' 
                                     AND ISNULL(LM.CONCOMITANTE, 'N') = 'N' 
                                     AND ISNULL(LM.OPTATIVAREFORCO, 'N') = @TIPOOPTATIVA 
                                JOIN LY_ALUNO LA (NOLOCK) 
                                  ON LM.ALUNO = LA.ALUNO 
                                JOIN LY_TURMA LT (NOLOCK) 
                                  ON LT.ANO = LM.ANO 
                                     AND LT.TURMA = LM.TURMA 
                                     AND LT.DISCIPLINA = LM.DISCIPLINA 
                                     AND LT.SEMESTRE = LM.SEMESTRE 
                                     AND LT.SIT_TURMA <> 'DESATIVADA' 
                         WHERE  FR.SUBPERIODO IS NOT NULL 
                                AND LA.PESSOA = @PESSOA 
                                AND LM.ANO = @ANO 
                                AND LM.SERIE = @SERIE 
                                AND LM.OPTATIVAREFORCO = @TIPOOPTATIVA 
                         GROUP  BY FR.SUBPERIODO, 
                                   LF.ALUNO, 
                                   LT.ANO, 
                                   LT.SEMESTRE, 
                                   LT.TURMA, 
                                   LT.DISCIPLINA, 
                                   LF.FALTAS, 
                                   FR.AULAS_DADAS) 
                SELECT TOTAL_PONTOS, 
                       CARGA_HORARIA, 
                       UPPER(SITUACAO_HIST) AS SITUACAO_HIST 
                FROM   (SELECT LM.ALUNO, 
                               LT.SERIE, 
                               LT.TURMA, 
                               LT.TURNO, 
                               LT.CURSO, 
                               LC.MODALIDADE, 
                               LC.TIPO_CURSO, 
                               LT.CURRICULO, 
                               LM.ANO, 
                               LM.SEMESTRE, 
                               UPPER (LM.SITUACAO_HIST)                      SITUACAO, 
                               LM.SITUACAO_HIST, 
                               ISNULL(LD.PERC_PRESMIN, 0) * 100              AS 
                               PERCENTUAL_PRESENCA_MIN 
                                      , 
                               ( ISNULL(LD.HORAS_ATIV, 0) 
                                 + ISNULL(LD.HORAS_AULA, 0) 
                                 + ISNULL(LD.HORAS_ESTAGIO, 0) 
                                 + ISNULL(LD.HORAS_LAB, 0) )                 AS CARGA_HORARIA, 
                               ISNULL(LT.DISCIPLINA_MULTIPLA, LT.DISCIPLINA) AS DISCIPLINA, 
                               LD.NOME_COMPL                                 AS NOME_DISC, 
                               (SELECT MT.MEDIA 
                                FROM   MEDIA_TURMA_HIST MT 
                                WHERE  MT.ALUNO = LM.ALUNO 
                                       AND MT.ANO = LM.ANO 
                                       AND MT.SEMESTRE = LM.SEMESTRE 
                                       AND MT.DISCIPLINA = LM.DISCIPLINA 
                                       AND MT.TURMA = LM.TURMA 
                                       AND MT.BIMESTRE = 1)                  AS NOTA_1B, 
                               (SELECT MT.MEDIA 
                                FROM   MEDIA_TURMA_HIST MT 
                                WHERE  MT.ALUNO = LM.ALUNO 
                                       AND MT.ANO = LM.ANO 
                                       AND MT.SEMESTRE = LM.SEMESTRE 
                                       AND MT.DISCIPLINA = LM.DISCIPLINA 
                                       AND MT.TURMA = LM.TURMA 
                                       AND MT.BIMESTRE = 2)                  AS NOTA_2B, 
                               (SELECT MT.MEDIA 
                                FROM   MEDIA_TURMA_HIST MT 
                                WHERE  MT.ALUNO = LM.ALUNO 
                                       AND MT.ANO = LM.ANO 
                                       AND MT.SEMESTRE = LM.SEMESTRE 
                                       AND MT.DISCIPLINA = LM.DISCIPLINA 
                                       AND MT.TURMA = LM.TURMA 
                                       AND MT.BIMESTRE = 3)                  AS NOTA_3B, 
                               (SELECT MT.MEDIA 
                                FROM   MEDIA_TURMA_HIST MT 
                                WHERE  MT.ALUNO = LM.ALUNO 
                                       AND MT.ANO = LM.ANO 
                                       AND MT.SEMESTRE = LM.SEMESTRE 
                                       AND MT.DISCIPLINA = LM.DISCIPLINA 
                                       AND MT.TURMA = LM.TURMA 
                                       AND MT.BIMESTRE = 4)                  AS NOTA_4B, 
                               (SELECT SUM(ISNULL(MT.MEDIA, 0)) / COUNT(*) 
                                FROM   MEDIA_TURMA_HIST MT 
                                WHERE  MT.ALUNO = LM.ALUNO 
                                       AND MT.ANO = LM.ANO 
                                       AND MT.SEMESTRE = LM.SEMESTRE 
                                       AND MT.DISCIPLINA = LM.DISCIPLINA 
                                       AND MT.TURMA = LM.TURMA)              AS NOTA_GERAL, 
                               (SELECT SUM(ISNULL(MT.MEDIA, 0)) 
                                FROM   MEDIA_TURMA_HIST MT 
                                WHERE  MT.ALUNO = LM.ALUNO 
                                       AND MT.ANO = LM.ANO 
                                       AND MT.SEMESTRE = LM.SEMESTRE 
                                       AND MT.DISCIPLINA = LM.DISCIPLINA 
                                       AND MT.TURMA = LM.TURMA)              AS TOTAL_PONTOS, 
                               (SELECT SUM(ISNULL(MF.FALTAS, 0)) / COUNT(*) 
                                FROM   MEDIA_FALTA_HIST MF 
                                WHERE  MF.ALUNO = LM.ALUNO 
                                       AND MF.ANO = LM.ANO 
                                       AND MF.SEMESTRE = LM.SEMESTRE 
                                       AND MF.DISCIPLINA = LM.DISCIPLINA 
                                       AND MF.TURMA = LM.TURMA 
                                       AND MF.BIMESTRE = 1)                  AS FALTA_1B, 
                               (SELECT SUM(ISNULL(MF.FALTAS, 0)) / COUNT(*) 
                                FROM   MEDIA_FALTA_HIST MF 
                                WHERE  MF.ALUNO = LM.ALUNO 
                                       AND MF.ANO = LM.ANO 
                                       AND MF.SEMESTRE = LM.SEMESTRE 
                                       AND MF.DISCIPLINA = LM.DISCIPLINA 
                                       AND MF.TURMA = LM.TURMA 
                                       AND MF.BIMESTRE = 2)                  AS FALTA_2B, 
                               (SELECT SUM(ISNULL(MF.FALTAS, 0)) / COUNT(*) 
                                FROM   MEDIA_FALTA_HIST MF 
                                WHERE  MF.ALUNO = LM.ALUNO 
                                       AND MF.ANO = LM.ANO 
                                       AND MF.SEMESTRE = LM.SEMESTRE 
                                       AND MF.DISCIPLINA = LM.DISCIPLINA 
                                       AND MF.TURMA = LM.TURMA 
                                       AND MF.BIMESTRE = 3)                  AS FALTA_3B, 
                               (SELECT SUM(ISNULL(MF.FALTAS, 0)) / COUNT(*) 
                                FROM   MEDIA_FALTA_HIST MF 
                                WHERE  MF.ALUNO = LM.ALUNO 
                                       AND MF.ANO = LM.ANO 
                                       AND MF.SEMESTRE = LM.SEMESTRE 
                                       AND MF.DISCIPLINA = LM.DISCIPLINA 
                                       AND MF.TURMA = LM.TURMA 
                                       AND MF.BIMESTRE = 4)                  AS FALTA_4B, 
                               (SELECT SUM(ISNULL(MF.FALTAS, 0)) 
                                FROM   MEDIA_FALTA_HIST MF 
                                WHERE  MF.ALUNO = LM.ALUNO 
                                       AND MF.ANO = LM.ANO 
                                       AND MF.SEMESTRE = LM.SEMESTRE 
                                       AND MF.DISCIPLINA = LM.DISCIPLINA 
                                       AND MF.TURMA = LM.TURMA)              AS FALTA_GERAL, 
                               (SELECT SUM(ISNULL(MT.AULAS_DADAS, 0)) 
                                FROM   MEDIA_FALTA_HIST MT 
                                WHERE  MT.ALUNO = LM.ALUNO 
                                       AND MT.ANO = LM.ANO 
                                       AND MT.SEMESTRE = LM.SEMESTRE 
                                       AND MT.DISCIPLINA = LM.DISCIPLINA 
                                       AND MT.TURMA = LM.TURMA)              AS AULAS_DADAS, 
                               (SELECT ISNULL(CONVERT(DECIMAL(5, 2), ( 
                                              ( ( 
                                              SUM(ISNULL(AULAS_DADAS, 0)) - SUM( 
                                                             ISNULL(MF.FALTAS, 0)) 
                                                                         ) * 100 
                                                                               ) / 
                                                                                      SUM( 
                                              ISNULL( 
                                              AULAS_DADAS, 0)) 
                                                                     )), 100) 
                                FROM   MEDIA_FALTA_HIST MF 
                                WHERE  MF.ALUNO = LM.ALUNO 
                                       AND MF.ANO = LM.ANO 
                                       AND MF.SEMESTRE = LM.SEMESTRE 
                                       AND MF.DISCIPLINA = LM.DISCIPLINA 
                                       AND MF.TURMA = LM.TURMA)              AS 
                               PERCENTUAL_PRESENCA 
                        FROM   LY_HISTMATRICULA LM (NOLOCK) 
                               JOIN LY_ALUNO LA (NOLOCK) 
                                 ON LM.ALUNO = LA.ALUNO 
                               JOIN LY_TURMA LT (NOLOCK) 
                                 ON LM.ANO = LT.ANO 
                                    AND LM.SEMESTRE = LT.SEMESTRE 
                                    AND LM.TURMA = LT.TURMA 
                                    AND LM.DISCIPLINA = LT.DISCIPLINA 
                               JOIN LY_DISCIPLINA LD (NOLOCK) 
                                 ON LD.DISCIPLINA = ISNULL(LT.DISCIPLINA_MULTIPLA, 
                                                    LT.DISCIPLINA) 
                               JOIN LY_CURSO LC (NOLOCK) 
                                 ON LC.CURSO = LT.CURSO 
                        WHERE  LA.PESSOA = @PESSOA 
                               AND LM.ANO = @ANO 
                               AND LM.SERIE = @SERIE 
                               AND LM.SITUACAO_HIST <> 'CANCELADO' 
                               AND ISNULL(LM.MAIS_EDUCACAO, 'N') = 'N' 
                               AND ISNULL(LM.EDUC_ESPECIAL, 'N') = 'N' 
                               AND ISNULL(LM.CONCOMITANTE, 'N') = 'N' 
                               AND ISNULL(LM.OPTATIVAREFORCO, 'N') = @TIPOOPTATIVA) Q1 ";
                #endregion query

                ctxQuery.Parameters.Add("@PESSOA", pessoa);
                ctxQuery.Parameters.Add("@ANO", ano);
                ctxQuery.Parameters.Add("@AGRUPAMENTO", grupoDisciplina);
                ctxQuery.Parameters.Add("@SERIE", serie);
                ctxQuery.Parameters.Add("@TIPOCONCLUSAOID", tipoConclusaoID);
                ctxQuery.Parameters.Add("@TIPOOPTATIVA", tipoOptativa);

                reader = ctx.GetDataReader(ctxQuery);

                while (reader.Read())
                {
                    heDTO.Nota_geral = Convert.ToDecimal(reader["TOTAL_PONTOS"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["TOTAL_PONTOS"].ToString()));
                    heDTO.CargaHoraria = Convert.ToDecimal(reader["CARGA_HORARIA"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["CARGA_HORARIA"].ToString()));
                    heDTO.Situacao_hist = reader["SITUACAO_HIST"].ToString();
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }

            return heDTO;
        }
        
        public List<HistoricoEscolarDTO> listarTodasAsDisciplinasPor(string pessoa, int tipoConclusaoID)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery ctxQuery = new ContextQuery();
            SqlDataReader reader = null;
            List<HistoricoEscolarDTO> listAllHistoricoEscolar = new List<HistoricoEscolarDTO>();

            try
            {
                #region query
                ctxQuery.Command = @" SELECT DISTINCT
                                        LT.SERIE, 
					                    LM.ANO,  
					                    LM.SEMESTRE,
					                    GD.AGRUPAMENTO,
					                    LA.PESSOA,
					                    ENSINO_RELIGIOSO, 
					                    LINGUA_ESTRANGEIRA
					                    INTO #AGRUPAMENTOS
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
					                    INNER JOIN LY_CURRICULO CUR (NOLOCK) ON LT.CURSO = CUR.CURSO AND LT.TURNO = CUR.TURNO AND LT.CURRICULO = CUR.CURRICULO
                                        JOIN	LY_DISCIPLINA LD (NOLOCK) ON LD.DISCIPLINA = ISNULL(LT.DISCIPLINA_MULTIPLA, LT.DISCIPLINA) 
                                        JOIN	LY_CURSO LC (NOLOCK) ON LC.CURSO = LT.CURSO 
                                        JOIN	CertificacaoEscolar.TIPOCONCLUSAO_MODALIDADETIPO tm (nolock) on (tm.TIPO = LC.TIPO and tm.MODALIDADE = lc.MODALIDADE )
                                        LEFT JOIN LY_GRUPO_HABILITACAO_DISC GD (NOLOCK) ON  LD.DISCIPLINA = GD.DISCIPLINA 
                                        WHERE  LA.PESSOA = @PESSOA
                                           AND TM.TIPOCONCLUSAOID = @TIPOCONCLUSAOID  
                                           AND LT.CURSO NOT IN ('0002.37','0001.27') -- RETIRADO ATE DEFINICAO DA SITUAÇÃO DO CES
                                            AND LT.CURSO NOT IN ('0091.29','0091.30','0091.31','0092.39')
	                                    ORDER BY ANO, SERIE, SEMESTRE, AGRUPAMENTO, PESSOA
                    					
					                    --Adiciona lingua estrangueira para series que possuem
					                     INSERT INTO #AGRUPAMENTOS
					                     SELECT DISTINCT SERIE, ANO, SEMESTRE, 'LEOPT', PESSOA, '0', '1'
					                     FROM #AGRUPAMENTOS 
					                     WHERE LINGUA_ESTRANGEIRA = 'S'

					                     --Adiciona ensino religioso para series que possuem
					                     INSERT INTO #AGRUPAMENTOS
					                     SELECT DISTINCT SERIE, ANO, SEMESTRE, 'REOPT', PESSOA, '1', '0'
					                     FROM #AGRUPAMENTOS 
					                     WHERE ENSINO_RELIGIOSO = 'S'

					                     SELECT DISTINCT SERIE, 
                                                ANO, 
                                                SEMESTRE, 
                                                AGRUPAMENTO, 
                                                PESSOA 
					                     FROM #AGRUPAMENTOS A
					                     ORDER BY ANO, SERIE, SEMESTRE, AGRUPAMENTO, PESSOA

					                     DROP TABLE #AGRUPAMENTOS ";
                #endregion query

                ctxQuery.Parameters.Add("@PESSOA", pessoa);
                ctxQuery.Parameters.Add("@TIPOCONCLUSAOID", tipoConclusaoID);
                reader = ctx.GetDataReader(ctxQuery);
            
                while (reader.Read())
                {
                    Certificacao.DTOs.HistoricoEscolarDTO heDTO = new HistoricoEscolarDTO();
                    heDTO.Serie = reader["SERIE"].ToString().Trim();
                    heDTO.Agrupamento = reader["AGRUPAMENTO"].ToString().Trim();
                    heDTO.Semestre = reader["SEMESTRE"].ToString().Trim();
                    heDTO.Ano = reader["ANO"].ToString().Trim();
                    heDTO.Pessoa = reader["PESSOA"].ToString().Trim();
                    listAllHistoricoEscolar.Add(heDTO);
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
            return listAllHistoricoEscolar;
        }

        public HistoricoEscolarDTO situacaofinalDoAlunoPor(string pessoa, string serie, string ano, int tipoConclusaoID)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery ctxQuery = new ContextQuery();
            SqlDataReader reader = null;
            Certificacao.DTOs.HistoricoEscolarDTO heDTO = new HistoricoEscolarDTO();

            try
            {
                #region query
                ctxQuery.Command = @"                     
                                               
                    SELECT  TSFA.ALUNO AS ALUNO, 
                            TSFA.ANO AS ANO, 
                            TSFA.PERIODO AS PERIODO, 
                            upper(TSFA.SITUACAO_FINAL) AS SITUACAO_FINAL, 
                            TSFA.FREQUENCIA_GLOBAL AS FREQUENCIA_GLOBAL
                    FROM lY_HISTMATRICULA LM (NOLOCK) 
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
                    LEFT JOIN LY_TIPO_CURSO  TC ON ( TC.TIPO=LC.TIPO ) 
                    WHERE LA.PESSOA = @PESSOA
							and lm.ano = @ANO
							and lm.SERIE = @SERIE							
							AND TM.TIPOCONCLUSAOID = @TIPOCONCLUSAOID
                             AND LT.CURSO NOT IN ('0091.29','0091.30','0091.31','0092.39')
		        group by TSFA.ALUNO, TSFA.ANO, TSFA.PERIODO, TSFA.SITUACAO_FINAL, TSFA.FREQUENCIA_GLOBAL ";
                #endregion query

                ctxQuery.Parameters.Add("@PESSOA", pessoa);
                ctxQuery.Parameters.Add("@SERIE", serie);
                ctxQuery.Parameters.Add("@ANO", ano);                
                ctxQuery.Parameters.Add("@TIPOCONCLUSAOID", tipoConclusaoID);
                reader = ctx.GetDataReader(ctxQuery);
                
                while (reader.Read())
                {
                    heDTO.MatriculaAluno = reader["ALUNO"].ToString();
                    heDTO.Ano = reader["ANO"].ToString().Trim();
                    heDTO.Semestre = reader["PERIODO"].ToString().Trim();

                    if ((reader["SITUACAO_FINAL"].ToString().Trim() == "APROVADO"))
                        heDTO.SituacaoFinal = "APROV";
                    else if ((reader["SITUACAO_FINAL"].ToString().Trim() == "REP NOTA"))
                        heDTO.SituacaoFinal = "REP NOTA";
                    else if ((reader["SITUACAO_FINAL"].ToString().Trim() == "APROVADO COM DEP"))
                        heDTO.SituacaoFinal = "APROV C/ DEP";
                    else if ((reader["SITUACAO_FINAL"].ToString().Trim() == "REP FREQ"))
                        heDTO.SituacaoFinal = "REP FREQ";
                    else if ((reader["SITUACAO_FINAL"].ToString().Trim() == "PROMOVIDO"))
                        heDTO.SituacaoFinal = "PROMOV";
                    else if ((reader["SITUACAO_FINAL"].ToString().Trim() == "RETIDO"))
                        heDTO.SituacaoFinal = "RETIDO";

                    heDTO.Freqtotal = Convert.ToDecimal(reader["FREQUENCIA_GLOBAL"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["FREQUENCIA_GLOBAL"].ToString() ));
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }

            return heDTO;

        }
        
        public bool OfereceOptativaHabilitadaPor(string pessoa, string ano, string periodo, string serie, int tipoConclusaoID, string tipoOptativa)
        {
            bool retorno = false;
            string ensinoReligioso = string.Empty;
            string linguaEstrangeira = string.Empty;
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
										CUR.CURRICULO,
					                    ENSINO_RELIGIOSO, 
					                    LINGUA_ESTRANGEIRA					                   
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
					                    INNER JOIN LY_CURRICULO CUR (NOLOCK) ON LT.CURSO = CUR.CURSO AND LT.TURNO = CUR.TURNO AND LT.CURRICULO = CUR.CURRICULO
                                        JOIN	LY_CURSO LC (NOLOCK) ON LC.CURSO = LT.CURSO 
                                        JOIN	CertificacaoEscolar.TIPOCONCLUSAO_MODALIDADETIPO tm (nolock) on (tm.TIPO = LC.TIPO and tm.MODALIDADE = lc.MODALIDADE )
                                        WHERE  LA.PESSOA = @PESSOA
											   AND TM.TIPOCONCLUSAOID = @TIPOCONCLUSAOID  
											   AND LM.ANO = @ANO
											   AND LM.SEMESTRE = @SEMESTRE
											   AND LT.SERIE = @SERIE ";

                contextQuery.Parameters.Add("@PESSOA", pessoa);
                contextQuery.Parameters.Add("@TIPOCONCLUSAOID", tipoConclusaoID);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);
                contextQuery.Parameters.Add("@SERIE", serie);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    ensinoReligioso = Convert.ToString(reader["ENSINO_RELIGIOSO"]);
                    linguaEstrangeira = Convert.ToString(reader["LINGUA_ESTRANGEIRA"]);
                }

                if (tipoOptativa == "REOPT" && ensinoReligioso == "S")
                {
                    retorno = true;
                }
                else if (tipoOptativa == "LEOPT" && linguaEstrangeira == "S")
                {
                    retorno = true;
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
   }
}
