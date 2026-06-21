using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.Certificacao
{
    public class CertidaoEscolar
    {


       /// <summary>
        /// Lista o Ano Escolar disponível em LY_TURMA
       /// </summary>
        /// <returns> DataTable com Ano Escolar</returns>
        public DataTable ListarAnos()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" 
                                        select distinct T.ano AnoEscolar FROM  LY_TURMA T;

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


        /// <summary>
        /// Lista as turmas de acordo com o Ano, Curso e Unidade de Ensino
        /// </summary>
        /// <param name="ano">Ano</param>
        /// <param name="Curso">Curso</param>
        /// <param name="Faculdade">Unidade de Ensino</param>
        /// <returns>DataTable com Turma e Ano</returns>
        public DataTable ListarTurmas(int ano, string Curso, string Faculdade)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" 
                                       SELECT DISTINCT HM.TURMA,HM.ANO  
                                                
                                FROM   LY_HISTMATRICULA HM 
                                       JOIN LY_TURMA T (NOLOCK) 
                                         ON T.DISCIPLINA = HM.DISCIPLINA 
                                            AND T.TURMA = HM.TURMA 
                                            AND T.ANO = HM.ANO 
                                            AND T.SEMESTRE = HM.SEMESTRE 
                                       JOIN LY_SERIE S 
                                         ON S.CURSO = T.CURSO 
                                            AND S.TURNO = T.TURNO 
                                            AND S.CURRICULO = T.CURRICULO 
                                            AND S.SERIE = T.SERIE 
                                       JOIN LY_CURSO C 
                                         ON C.CURSO = T.CURSO 
                                WHERE  S.EMITE_CERTIFICACAO = 'S' 
                                       AND T.FACULDADE = @FACULDADE 
                                       AND C.CURSO = @CURSO 
                                       AND HM.ANO = @ANO 
                                ORDER  BY HM.TURMA  ";

                contextQuery.Parameters.Add("@ano", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, Curso);
                contextQuery.Parameters.Add("@FACULDADE", SqlDbType.VarChar, Faculdade);



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

        /// <summary>
        /// Lista os alunos autorizados, com tipo de documento solicitado = 2 (Certidão)para o Inspetor gerar
        /// </summary>
        /// <param name="ano">Ano</param>
        /// <param name="Curso">Curso</param>
        /// <param name="Faculdade">Unidade de Ensino</param>
        /// <param name="Turma">Turma</param>
        /// <returns>DataTable com as seguintes informações
        /// ANO,SEMESTRE,SERIE,UA,MODALIDADE,TIPOCONCLUSAOID,TIPO,TIPOCONCLUSAOID,DESCRICAO TIPO_CONCLUSAO,NOME_COMPL,ALUNO,TURMA,TURNO,CURSO,TIPO_CURSO,CURRICULO,UNIDADE_ENS,SITUACAO,ANO_SERIE_CONCLUINTE,EMITE_CERTIFICACAO 
        /// 
        /// </returns>
        public DataTable ListarAlunosCertidaoEscolar(int ano, string Curso, string Faculdade, string Turma)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" 
                                        SELECT DISTINCT LM.ANO, 
                                        LM.SEMESTRE, 
                                        LM.SERIE, 
                                        LUE.SETOR                   AS UA, 
                                        TM.MODALIDADE, 
										doccert.TIPOCONCLUSAOID,
										doccert.DOCUMENTOID,
                                        TM.TIPO, 
                                        TM.TIPOCONCLUSAOID, 
                                        TPCONCLUSAO.DESCRICAO TIPO_CONCLUSAO,
                                        P.NOME_COMPL, 
                                        LM.ALUNO, 
                                        LT.TURMA, 
                                        LT.TURNO, 
                                        LT.CURSO,                                        
                                        LC.TIPO_CURSO, 
                                        LT.CURRICULO, 
                                        LUE.UNIDADE_ENS, 
                                        Upper (TSFA.SITUACAO_FINAL) SITUACAO, 
                                        ANO_SERIE_CONCLUINTE, 
                                        EMITE_CERTIFICACAO 
                         FROM   LY_HISTMATRICULA LM (NOLOCK) 
                               JOIN LY_ALUNO LA (NOLOCK) 
                                 ON LM.ALUNO = LA.ALUNO 
                                    AND LM.SITUACAO_HIST <> 'CANCELADO' 
                                    AND ISNULL(LM.MAIS_EDUCACAO, 'N') = 'N' 
                                    AND ISNULL(LM.EDUC_ESPECIAL, 'N') = 'N' 
                                    AND ISNULL(LM.CONCOMITANTE, 'N') = 'N' 
                                    AND ISNULL(LM.OPTATIVAREFORCO, 'N') = 'N' 
                               INNER JOIN LY_PESSOA P 
                                       ON P.PESSOA = LA.PESSOA 
                               JOIN TCE_SITUACAO_FINAL_ALUNO TSFA (NOLOCK) 
                                 ON TSFA.ALUNO = LM.ALUNO 
                                    AND TSFA.ANO = LM.ANO 
                                    AND TSFA.PERIODO = LM.SEMESTRE 
                                    AND TSFA.TURMA = LM.TURMA 
                                    AND ISNULL(LM.DEPENDENCIA, 'N') = 'N' 
                                    AND TSFA.SITUACAO_FINAL IN ( 'Aprovado', 'Aprovado Com Dep','Promovido' ) 
                               JOIN LY_TURMA LT (NOLOCK) 
                                 ON LM.ANO = LT.ANO 
                                    AND LM.SEMESTRE = LT.SEMESTRE 
                                    AND LM.TURMA = LT.TURMA 
                                    AND LM.DISCIPLINA = LT.DISCIPLINA 
                               JOIN LY_UNIDADE_ENSINO LUE (NOLOCK) 
                                 ON LUE.UNIDADE_ENS = LT.UNIDADE_RESPONSAVEL 
                               JOIN LY_CURSO LC (NOLOCK) 
                                 ON LC.CURSO = LT.CURSO 
                               JOIN CERTIFICACAOESCOLAR.TIPOCONCLUSAO_MODALIDADETIPO TM (NOLOCK) 
                                 ON ( TM.TIPO = LC.TIPO 
                                      AND TM.MODALIDADE = LC.MODALIDADE ) 
							JOIN CERTIFICACAOESCOLAR.DOCUMENTOCERTIFICACAO DOCCERT (NOLOCK)
							ON ( DOCCERT.ALUNO=LM.ALUNO)

                         LEFT  JOIN [CERTIFICACAOESCOLAR].DOCUMENTOGERADO docg (NOLOCK) 
							  on docg.DOCUMENTOCERTID=DOCCERT.DOCUMENTOCERTID

                               JOIN LY_SERIE SE 
                                 ON SE.CURSO = LT.CURSO 
                                    AND SE.TURNO = LT.TURNO 
                                    AND SE.CURRICULO = LT.CURRICULO 
                                    AND SE.SERIE = LT.SERIE 
                        
                           JOIN [CERTIFICACAOESCOLAR].TIPOCONCLUSAO TPCONCLUSAO (NOLOCK) 
                             ON TPCONCLUSAO.TIPOCONCLUSAOID = DOCCERT.TIPOCONCLUSAOID 
                            WHERE  
                         ISNULL(SE.EMITE_CERTIFICACAO, 'N') = 'S' 
                          
                          AND LT.FACULDADE = @FACULDADE 
                          AND LT.CURSO = @CURSO 
                          AND LT.TURMA = @TURMA 
                          AND LT.ANO = @ANO 
						  AND doccert.AUTORIZADO = 1  -- AUTORIZADO
						  AND doccert.DOCUMENTOID = 2 --CERTIDAO
                         AND docg.ARQUIVO IS NULL    -- NÃO POSSUI ARQUIVO GERADO
                        ORDER  BY P.NOME_COMPL,
                                  LM.ANO DESC, 
                                  LM.SEMESTRE DESC, 
                                  LM.SERIE DESC ";

                contextQuery.Parameters.Add("@ano", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, Curso);
                contextQuery.Parameters.Add("@FACULDADE", SqlDbType.VarChar, Faculdade);
                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, Turma);



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
