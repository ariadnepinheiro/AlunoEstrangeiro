using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using System;
using System.Data;

namespace Techne.Lyceum.RN
{
    public class SituacaoFinalAluno : RNBase
    {
        public const string Aprovado = "Aprovado";

        public const string AprovadoComDependencia = "Aprovado Com Dep";

        public const string ReprovadoPorFrequencia = "Rep Freq";

        public const string ReprovadoPorNota = "Rep Nota";

        public static TceSituacaoFinalAluno Carregar(string id)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery =
                    new ContextQuery(
                        @" SELECT  *
                            FROM    TCE_SITUACAO_FINAL_ALUNO
                            WHERE   ID_SITUACAO_FINAL_ALUNO = @ID_SITUACAO_FINAL_ALUNO ");
                contextQuery.Parameters.Add("@ID_SITUACAO_FINAL_ALUNO", id);

                return ctx.TryToBindEntity<TceSituacaoFinalAluno>(contextQuery);
            }
        }

        public TceSituacaoFinalAluno ObtemPor(string aluno, int ano, int periodo, string turma)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            TceSituacaoFinalAluno situacaoFinalAluno = new TceSituacaoFinalAluno();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  *
                        FROM    TCE_SITUACAO_FINAL_ALUNO
                        WHERE   ALUNO = @ALUNO
                                AND ANO = @ANO
                                AND PERIODO = @PERIODO
                                AND TURMA = @TURMA ";

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@TURMA", turma);

                situacaoFinalAluno = ctx.TryToBindEntity<TceSituacaoFinalAluno>(contextQuery);

                return situacaoFinalAluno;
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

        public static void Inserir(TceSituacaoFinalAluno situacaoFinalAluno, DataContext context)
        {
            var contextQuery = new ContextQuery(
               @"INSERT  INTO TCE_SITUACAO_FINAL_ALUNO ( ALUNO, ANO,
                                                                    PERIODO, TURMA, FREQUENCIA_GLOBAL, MATRICULA, SITUACAO_FINAL)
                            VALUES  ( @ALUNO, @ANO, @PERIODO, @TURMA,
                                      @FREQUENCIA_GLOBAL, @MATRICULA, @SITUACAO_FINAL) ");

            var freq = situacaoFinalAluno.FrequenciaGlobal.ToString("0.00").Replace(',', '.');


            contextQuery.Parameters.Add("@ALUNO", situacaoFinalAluno.Aluno);
            contextQuery.Parameters.Add("@ANO", situacaoFinalAluno.Ano);
            contextQuery.Parameters.Add("@PERIODO", situacaoFinalAluno.Periodo);
            contextQuery.Parameters.Add("@TURMA", situacaoFinalAluno.Turma);
            contextQuery.Parameters.Add("@FREQUENCIA_GLOBAL", freq);
            contextQuery.Parameters.Add("@MATRICULA", situacaoFinalAluno.Matricula);
            contextQuery.Parameters.Add("@SITUACAO_FINAL", situacaoFinalAluno.SituacaoFinal);

            context.ApplyModifications(contextQuery);
        }

        public static void Remover(TceSituacaoFinalAluno situacaoFinalAluno, DataContext context)
        {
            var contextQuery = new ContextQuery(
               @"DELETE TCE_SITUACAO_FINAL_ALUNO  
                WHERE ALUNO = @ALUNO
                AND ANO = @ANO 
                AND PERIODO = @PERIODO
                AND TURMA = @TURMA   ");

            contextQuery.Parameters.Add("@ALUNO", situacaoFinalAluno.Aluno);
            contextQuery.Parameters.Add("@ANO", situacaoFinalAluno.Ano);
            contextQuery.Parameters.Add("@PERIODO", situacaoFinalAluno.Periodo);
            contextQuery.Parameters.Add("@TURMA", situacaoFinalAluno.Turma);

            context.ApplyModifications(contextQuery);
        }

        public static bool ExisteSituacaoFinalPor(string aluno, decimal ano, decimal periodo, string turma)
        {
            DataContext contexto = null;
            bool retorno = false;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                retorno = ExisteSituacaoFinalPor(contexto, aluno, ano, periodo, turma);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                {
                    contexto.Dispose();
                }
            }
            return retorno;
        }

        public static bool ExisteSituacaoFinalPor(DataContext context, string aluno, decimal ano, decimal periodo, string turma)
        {
            var contextQuery = new ContextQuery(@" SELECT COUNT(ID_SITUACAO_FINAL_ALUNO)
                                                     FROM [LYCEUM].[dbo].[TCE_SITUACAO_FINAL_ALUNO]
                                                    WHERE ALUNO = @ALUNO
                                                      AND ANO = @ANO
                                                      AND PERIODO = @PERIODO
                                                      AND TURMA = @TURMA
                                                ");

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@TURMA", turma);

            return context.GetReturnValue<int>(contextQuery) > 0;
        }

        public bool EhReprovadoPor(DataContext contexto, string aluno, int ano, int periodo, int periodoConfirmacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            string possiveisPeriodosAnteriores = string.Empty;
            int anoAnterior;

            if (periodo == 2)
            {
                anoAnterior = ano;
                possiveisPeriodosAnteriores = "1";
            }
            else
            {
                anoAnterior = ano -1;
                possiveisPeriodosAnteriores = "0, 2";
            }

            contextQuery.Command = string.Format(@" SELECT COUNT(*)
                                        FROM TCE_SITUACAO_FINAL_ALUNO S
	                                        INNER JOIN LY_TURMA T ON S.TURMA = T.TURMA
						                                        AND S.ANO = T.ANO
						                                        AND S.PERIODO = T.SEMESTRE
	                                        INNER JOIN Pedagogico.PERIODOCONFIRMACAOCURSO C ON T.CURSO = C.CURSO
												                                        AND T.SERIE = C.SERIE					
                                        WHERE S.ALUNO = @ALUNO
	                                        AND S.ANO = @ANOANTERIOR
	                                        AND S.PERIODO IN ({0})
	                                        AND PERIODOCONFIRMACAOID = @PERIODOCONFIRMACAOID
	                                        AND (S.SITUACAO_FINAL LIKE 'REP%' OR S.SITUACAO_FINAL = 'Retido')  ", possiveisPeriodosAnteriores);

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@ANOANTERIOR", anoAnterior);
            contextQuery.Parameters.Add("@PERIODOCONFIRMACAOID", periodoConfirmacaoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool EhReprovadoPor(DataContext context, string aluno, decimal ano, decimal periodo, string curso, int serie)
        {
            var contextQuery = new ContextQuery(@" SELECT COUNT(ID_SITUACAO_FINAL_ALUNO)
                                    FROM [LYCEUM].[DBO].[TCE_SITUACAO_FINAL_ALUNO] S
	                                    INNER JOIN [LYCEUM]..LY_TURMA T 
					                                    ON S.TURMA = T.TURMA 
					                                    AND S.ANO = T.ANO 
					                                    AND S.PERIODO = T.SEMESTRE
                                    WHERE ALUNO = @ALUNO
	                                    AND S.ANO = @ANO
	                                    AND SITUACAO_FINAL LIKE 'REP%'
	                                    AND PERIODO = @PERIODO
	                                    AND SERIE = @SERIE
	                                    AND CURSO = @CURSO ");

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@SERIE", serie);
            contextQuery.Parameters.Add("@CURSO", curso);

            return context.GetReturnValue<int>(contextQuery) > 0;
        }

        public bool EhReprovadoItinerarioFormativoTrihaPor(DataContext context, string aluno, decimal ano, decimal periodo, int serie, string modalidade, int tipo)
        {
            var contextQuery = new ContextQuery(@" SELECT COUNT(ID_SITUACAO_FINAL_ALUNO)
                                    FROM [LYCEUM].[DBO].[TCE_SITUACAO_FINAL_ALUNO] S
	                                    INNER JOIN [LYCEUM]..LY_TURMA T 
					                                    ON S.TURMA = T.TURMA 
					                                    AND S.ANO = T.ANO 
					                                    AND S.PERIODO = T.SEMESTRE
										INNER JOIN LY_CURSO C ON C.CURSO = t.CURSO
                                    WHERE ITINERARIOFORMATIVO = 'S'	                                        
	                                        AND TRILHAAPRENDIZAGEMID IS NOT NULL
	                                        AND TRILHAAPRENDIZAGEMID <> 31 --Tipo interno 											
											AND ALUNO = @ALUNO
											AND S.ANO = @ANO
											AND SITUACAO_FINAL LIKE 'REP%'
											AND PERIODO = @PERIODO
											AND SERIE = @SERIE
											and c.MODALIDADE = @MODALIDADE
											and c.TIPO = @TIPO ");

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@SERIE", serie);
            contextQuery.Parameters.Add("@MODALIDADE", modalidade);
            contextQuery.Parameters.Add("@TIPO", tipo);

            return context.GetReturnValue<int>(contextQuery) > 0;
        }

        public bool EhReprovadoPor(string aluno, decimal ano, decimal periodo, string curso, int serie)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.EhReprovadoPor(contexto, aluno, ano, periodo, curso, serie);
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

        public bool EhReprovadoItinerarioFormativoTrihaPor(string aluno, decimal ano, decimal periodo, int serie, string modalidade, int tipo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.EhReprovadoItinerarioFormativoTrihaPor(contexto, aluno, ano, periodo, serie, modalidade, tipo);
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

        internal static void AtualizarSituacaoFinalEFrequenciaGlobal(TceSituacaoFinalAluno situacaoFinalAluno, DataContext context)
        {
            var contextQuery = new ContextQuery(@"UPDATE  [LYCEUM].[DBO].[TCE_SITUACAO_FINAL_ALUNO]
                                                SET     SITUACAO_FINAL = @SITUACAO_FINAL ,
                                                        FREQUENCIA_GLOBAL = @FREQUENCIA_GLOBAL ,
                                                        MATRICULA = @MATRICULA
                                                WHERE   ALUNO = @ALUNO
                                                        AND ANO = @ANO
                                                        AND PERIODO = @PERIODO
                                                        AND TURMA = @TURMA ");

            var freq = situacaoFinalAluno.FrequenciaGlobal.ToString("0.00").Replace(',', '.');

            contextQuery.Parameters.Add("@ALUNO", situacaoFinalAluno.Aluno);
            contextQuery.Parameters.Add("@ANO", situacaoFinalAluno.Ano);
            contextQuery.Parameters.Add("@PERIODO", situacaoFinalAluno.Periodo);
            contextQuery.Parameters.Add("@TURMA", situacaoFinalAluno.Turma);
            contextQuery.Parameters.Add("@FREQUENCIA_GLOBAL", freq);
            contextQuery.Parameters.Add("@MATRICULA", situacaoFinalAluno.Matricula);
            contextQuery.Parameters.Add("@SITUACAO_FINAL", situacaoFinalAluno.SituacaoFinal);

            context.ApplyModifications(contextQuery);
        }

        public DataTable ObtemListaPor(string aluno)
        {	
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT SF.ID_SITUACAO_FINAL_ALUNO,
                                    SF.ALUNO,
                                    R.REGIONAL, 
                                    MU.NOME        AS MUNICIPIO, 
                                    UE.UNIDADE_ENS AS CENSO, 
                                    UE.NOME_COMP   AS ESCOLA, 
                                    T.ANO, 
                                    T.SEMESTRE, 
                                    T.TURMA, 
                                    SF.SITUACAO_FINAL, 
                                    SF.FREQUENCIA_GLOBAL 
                    FROM   TCE_SITUACAO_FINAL_ALUNO SF (NOLOCK) 
                           INNER JOIN LY_TURMA T (NOLOCK) 
                                   ON SF.TURMA = T.TURMA 
                                      AND SF.ANO = T.ANO 
                                      AND SF.PERIODO = T.SEMESTRE 
                           INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK) 
                                   ON T.FACULDADE = UE.UNIDADE_ENS 
                           INNER JOIN TCE_REGIONAL R (NOLOCK) 
                                   ON UE.ID_REGIONAL = R.ID_REGIONAL 
                           INNER JOIN HADES.DBO.HD_MUNICIPIO MU (NOLOCK) 
                                   ON UE.MUNICIPIO = MU.MUNICIPIO 
                    WHERE  ALUNO = @ALUNO
                    ORDER BY T.ANO DESC, T.SEMESTRE DESC ";

                contextQuery.Parameters.Add("@ALUNO", aluno);

                dt = contexto.GetDataTable(contextQuery);
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

            return dt;
        }

        public bool PossuiSituacaoFinal(string aluno, int ano, int periodo, string turma)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT  COUNT(*)
                        FROM    TCE_SITUACAO_FINAL_ALUNO
                        WHERE   ALUNO = @ALUNO
                                AND ANO = @ANO
                                AND PERIODO = @PERIODO
                                AND TURMA = @TURMA"

                };

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@TURMA", turma);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
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

        public void AtualizarSituacaoFinal(TceSituacaoFinalAluno situacaoFinalAluno, DataContext context)
        {
            var contextQuery = new ContextQuery(@"UPDATE  [LYCEUM].[DBO].[TCE_SITUACAO_FINAL_ALUNO]
                                                SET     SITUACAO_FINAL = @SITUACAO_FINAL ,                                                       
                                                        MATRICULA = @MATRICULA
                                                WHERE   ALUNO = @ALUNO
                                                        AND ANO = @ANO
                                                        AND PERIODO = @PERIODO
                                                        AND TURMA = @TURMA ");
                        
            contextQuery.Parameters.Add("@ALUNO", situacaoFinalAluno.Aluno);
            contextQuery.Parameters.Add("@ANO", situacaoFinalAluno.Ano);
            contextQuery.Parameters.Add("@PERIODO", situacaoFinalAluno.Periodo);
            contextQuery.Parameters.Add("@TURMA", situacaoFinalAluno.Turma);            
            contextQuery.Parameters.Add("@MATRICULA", situacaoFinalAluno.Matricula);
            contextQuery.Parameters.Add("@SITUACAO_FINAL", situacaoFinalAluno.SituacaoFinal);

            context.ApplyModifications(contextQuery);
        }
    }
}
