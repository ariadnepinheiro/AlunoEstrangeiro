namespace Techne.Lyceum.RN
{
    using System;
    using System.Data;
    using System.Text;
    using Seeduc.Infra.Data;
    using Techne.Data;
    using Techne.Lyceum.RN.DTOs;
    using Techne.Lyceum.RN.Util;
    using System.Data.SqlClient;

    public class Boletim : RNBase
    {
        public DateTime ObtemDataUltimaAtualizacaoBancoPor()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DateTime dataUltimaAtualizacao = DateTime.MinValue;
            SqlDataReader reader = null;

            try
            {
                contextQuery.Command = @" SELECT  MAX(RESTORE_DATE) RESTORE_DATE
                        FROM    [msdb].[dbo].[restorehistory]
                        WHERE   DESTINATION_DATABASE_NAME = 'LYCEUM' ";


                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    if (reader["RESTORE_DATE"] != DBNull.Value)
                    {
                        dataUltimaAtualizacao = Convert.ToDateTime(reader["RESTORE_DATE"]);
                    }
                }

                return dataUltimaAtualizacao;
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }

        public bool ExisteProvaSemNotaPor(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                        FROM    LY_MATRICULA M ( NOLOCK )
                                INNER JOIN LY_PROVA P ( NOLOCK ) ON P.ANO = M.ANO
                                                         AND P.SEMESTRE = M.SEMESTRE
                                                         AND P.DISCIPLINA = M.DISCIPLINA
                                                         AND P.TURMA = M.TURMA
                        WHERE   ALUNO = @ALUNO
                                AND SIT_MATRICULA = @SIT_MATRICULA
                                AND M.ANO <> YEAR(GETDATE())
                                AND ( NOT EXISTS ( SELECT TOP 1
                                                            1
                                                   FROM     LY_NOTA N ( NOLOCK )
                                                   WHERE    N.ANO = P.ANO
                                                            AND N.SEMESTRE = P.SEMESTRE
                                                            AND N.DISCIPLINA = P.DISCIPLINA
                                                            AND N.TURMA = P.TURMA
                                                            AND N.PROVA = P.PROVA
                                                            AND N.ALUNO = M.ALUNO )
                                      OR EXISTS ( SELECT TOP 1
                                                            1
                                                  FROM      LY_NOTA N ( NOLOCK ) 
                                                  WHERE     N.ANO = P.ANO
                                                            AND N.SEMESTRE = P.SEMESTRE
                                                            AND N.DISCIPLINA = P.DISCIPLINA
                                                            AND N.TURMA = P.TURMA
                                                            AND N.PROVA = P.PROVA
                                                            AND N.ALUNO = M.ALUNO
                                                            AND N.CONCEITO = NULL )
                                    ) ";

                contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
                contextQuery.Parameters.Add("@SIT_MATRICULA", TechneDbType.T_SIT_MATRICULA, Matricula.Matriculado);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    existe = true;
                }

                return existe;
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

        public bool ExisteFrequenciaSemFaltaPor(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                    FROM    LY_MATRICULA M ( NOLOCK )
                                            INNER JOIN LY_FREQ P ( NOLOCK ) ON P.ANO = M.ANO
                                                                    AND P.PERIODO = M.SEMESTRE
                                                                    AND P.DISCIPLINA = M.DISCIPLINA
                                                                    AND P.TURMA = M.TURMA
                                    WHERE   ALUNO = @ALUNO
                                            AND SIT_MATRICULA = @SIT_MATRICULA
                                            AND M.ANO <> YEAR(GETDATE())
                                            AND NOT EXISTS ( SELECT TOP 1
                                                                    1
                                                             FROM   LY_FALTA N ( NOLOCK )
                                                             WHERE  N.ANO = P.ANO
                                                                    AND N.PERIODO = P.PERIODO
                                                                    AND N.DISCIPLINA = P.DISCIPLINA
                                                                    AND N.TURMA = P.TURMA
                                                                    AND N.FREQ = P.FREQ
                                                                    AND N.ALUNO = M.ALUNO ) ";

                contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
                contextQuery.Parameters.Add("@SIT_MATRICULA", TechneDbType.T_SIT_MATRICULA, Matricula.Matriculado);


                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    existe = true;
                }

                return existe;
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

        public DataTable ListaBoletimConsolidadoBimestralPor(int ano, int periodo, string aluno)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable lista = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"SP_BOLETIM_DO_ALUNO";
                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                lista = contexto.GetDataTable(contextQuery);
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
            finally
            {
                if (contexto != null)
                {
                    contexto.Dispose();
                }
            }

            return lista;
        }

        public DataTable ListaBoletimConsolidadoBimestralHistoricoPor(int ano, int periodo, string aluno)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable lista = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"SP_BOLETIM_DO_ALUNO_HISTORICO";
                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                lista = contexto.GetDataTable(contextQuery);
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
            finally
            {
                if (contexto != null)
                {
                    contexto.Dispose();
                }
            }

            return lista;
        }

        public String ObtemSituacaoMatricula(string aluno, int ano, int semestre)
        {
         DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dadosTurma = null;
            string retorno = "";
            var dateTime = DateTime.Now.ToString("dd/MM/yyyy");
            var time = DateTime.Now.ToString("HH:mm");
            try
            {
                //Verifica se o Alun já concluiu o curso
                 contextQuery.Command = "";
                 contextQuery.Command = string.Format(@" Select  SIT_ALUNO, DATAALTERACAO
                                                         FROM LY_ALUNO
                                                         WHERE ALUNO = @ALUNO 
                                                         order by DATAALTERACAO desc");

                 contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
                 contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
                 contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_ANO, semestre);

                dadosTurma = ctx.GetDataTable(contextQuery);
                int conta = 0;
                string status = "Ativo";
                
                foreach (DataRow item in dadosTurma.Rows)
                {
                    if (item["SIT_ALUNO"].ToString() ==status )
                        retorno = "Matrícula Ativa às " + time + " do dia " + dateTime + ".";                    
                    conta = conta + 1;
                }
                

                if (retorno == "") {//senao achar nenhum 
                    //Verifica se o Aluno ainda esta estudando
                    contextQuery.Command = "";
                    contextQuery.Command = string.Format(@"Select top(1) DT_ENCERRAMENTO, b.DESCRICAO 
                                                            from   LY_H_CURSOS_CONCL a
														    inner join  LY_MOTIVOSAIDA b on a.MOTIVO = b.MOTIVOSAIDA
                                                            where ALUNO = @MATALUNO
                                                            order by 1 desc");

                    contextQuery.Parameters.Add("@MATALUNO", TechneDbType.T_CODIGO, aluno);
                    contextQuery.Parameters.Add("@MATANO", TechneDbType.T_ANO, ano);

                    dadosTurma = ctx.GetDataTable(contextQuery);
        
                    foreach (DataRow item in dadosTurma.Rows)
                    {
                        retorno = "Matrícula Inativa no dia " + item["DT_ENCERRAMENTO"].ToString().Substring(0, 10) + ". Motivo: " + item["DESCRICAO"].ToString();
                        
                  
                    }     
            
                }

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
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return retorno;

            }

        public DataTable ObtemDadosTurmaPrincipalPor(string aluno, int ano, int periodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dadosTurma = null;
            string possiveisPeriodos = Utils.RecuperaPossiveisPeriodosParaTurmaPrincipal(periodo);

            try
            {
                contextQuery.Command = string.Format(@" SELECT TOP 1
                                    T.TURMA ,
                                    T.SERIE ,
                                    T.ANO ,
                                    T.SEMESTRE ,
                                    C.NOME AS CURSO ,
                                    ( UE.UNIDADE_ENS + ' - ' + UE.NOME_COMP ) AS UNIDADE ,
                                    TU.DESCRICAO AS TURNO
                            FROM    LY_MATRICULA M ( NOLOCK )
                                    INNER JOIN DBO.LY_TURMA T ( NOLOCK ) ON M.DISCIPLINA = T.DISCIPLINA
                                                                            AND M.TURMA = T.TURMA
                                                                            AND M.ANO = T.ANO
                                                                            AND M.SEMESTRE = T.SEMESTRE
                                    INNER JOIN LY_CURSO C ( NOLOCK ) ON T.CURSO = C.CURSO
                                    INNER JOIN LY_UNIDADE_ENSINO UE ( NOLOCK ) ON UE.UNIDADE_ENS = T.FACULDADE
                                    INNER JOIN LY_TURNO TU ( NOLOCK ) ON TU.TURNO = T.TURNO
                            WHERE   M.ALUNO = @ALUNO
                                    AND M.SIT_MATRICULA = @SIT_MATRICULA
                                    AND M.ANO = @ANO
                                    AND M.SEMESTRE IN ( {0} )  
                                    AND T.OPTATIVAREFORCO = 'N'
                                    AND ISNULL(T.ELETIVA,'N') = 'N'
                                    AND ISNULL(M.DEPENDENCIA, 'N') = 'N'
                                    AND ISNULL(M.EDUC_ESPECIAL, 'N') = 'N'
                                    AND ISNULL(M.MAIS_EDUCACAO, 'N') = 'N'
                                    AND ISNULL(M.CONCOMITANTE, 'N') = 'N' ", possiveisPeriodos);

                contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
                contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
                contextQuery.Parameters.Add("@SIT_MATRICULA", TechneDbType.T_SIT_MATRICULA, Matricula.Matriculado);

                dadosTurma = ctx.GetDataTable(contextQuery);
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
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return dadosTurma;
        }

        public DataTable ObtemDadosAlunoTurmaPrincipalAtivaPor(string aluno, int ano, int periodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dadosTurma = null;
            string possiveisPeriodos = Utils.RecuperaPossiveisPeriodosParaTurmaPrincipal(periodo);


            try
            {
                contextQuery.Command =  string.Format(@" SELECT TOP 1 T.TURMA, 
                                                 T.SERIE, 
                                                 T.ANO, 
                                                 T.SEMESTRE, 
                                                 C.NOME         AS CURSO, 
                                                 UE.UNIDADE_ENS AS CENSO, 
                                                 UE.NOME_COMP   AS UNIDADE, 
                                                 TU.DESCRICAO   AS TURNO, 
                                                 RE.REGIONAL, 
                                                 MU.NOME        AS MUNICIPIO, 
                                                 A.ALUNO, 
                                                 P.NOME_COMPL   AS NOMEALUNO, 
                                                 P.NOME_MAE     AS NOMEMAE, 
                                                 P.DT_NASC, 
                                                 A.SIT_ALUNO 
                                    FROM   LY_ALUNO A ( NOLOCK ) 
                                           INNER JOIN LY_PESSOA P ( NOLOCK ) 
                                                   ON A.PESSOA = P.PESSOA 
                                           INNER JOIN LY_UNIDADE_ENSINO UE ( NOLOCK ) 
                                                   ON UE.UNIDADE_ENS = A.UNIDADE_ENSINO 
                                           INNER JOIN HADES.DBO.HD_MUNICIPIO MU ( NOLOCK ) 
                                                   ON UE.MUNICIPIO = MU.MUNICIPIO 
                                           INNER JOIN TCE_REGIONAL RE ( NOLOCK ) 
                                                   ON UE.ID_REGIONAL = RE.ID_REGIONAL 
                                           LEFT JOIN LY_MATRICULA M ( NOLOCK ) 
                                                  ON A.ALUNO = M.ALUNO 
                                                     AND M.ANO = @ANO
                                                     AND M.SEMESTRE IN ( {0} ) 
                                                     AND M.SIT_MATRICULA = @SIT_MATRICULA 
                                                     AND ISNULL(M.DEPENDENCIA, 'N') = 'N' 
                                                     AND ISNULL(M.EDUC_ESPECIAL, 'N') = 'N' 
                                                     AND ISNULL(M.MAIS_EDUCACAO, 'N') = 'N' 
                                                     AND ISNULL(M.CONCOMITANTE, 'N') = 'N' 
                                           LEFT JOIN DBO.LY_TURMA T ( NOLOCK ) 
                                                  ON M.DISCIPLINA = T.DISCIPLINA 
                                                     AND M.TURMA = T.TURMA 
                                                     AND M.ANO = T.ANO 
                                                     AND M.SEMESTRE = T.SEMESTRE 
                                                     AND T.OPTATIVAREFORCO = 'N' 
                                                     AND ISNULL(T.ELETIVA,'N') = 'N'
                                           LEFT JOIN LY_CURSO C ( NOLOCK ) 
                                                  ON T.CURSO = C.CURSO 
                                           LEFT JOIN LY_TURNO TU ( NOLOCK ) 
                                                  ON TU.TURNO = T.TURNO 
                                    WHERE  A.ALUNO = @ALUNO  ", possiveisPeriodos);

                contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
                contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
                contextQuery.Parameters.Add("@SIT_MATRICULA", TechneDbType.T_SIT_MATRICULA, Matricula.Matriculado);

                dadosTurma = ctx.GetDataTable(contextQuery);
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
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return dadosTurma;
        }


        public DataTable ObtemDadosTurmaPrincipalHistoricoPor(string aluno, int ano, int periodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dadosTurma = null;
            string possiveisPeriodos = Utils.RecuperaPossiveisPeriodosParaTurmaPrincipal(periodo);

            try
            {
                contextQuery.Command = string.Format(@" SELECT TOP 1
                            T.TURMA ,
                            T.SERIE ,
                            T.ANO ,
                            T.SEMESTRE ,
                            C.NOME AS CURSO ,
                            ( UE.UNIDADE_ENS + ' - ' + UE.NOME_COMP ) AS UNIDADE ,
                            TU.DESCRICAO AS TURNO
                    FROM    DBO.LY_HISTMATRICULA M ( NOLOCK )
                            INNER JOIN DBO.LY_TURMA T ( NOLOCK ) ON M.DISCIPLINA = T.DISCIPLINA
                                                                    AND M.TURMA = T.TURMA
                                                                    AND M.ANO = T.ANO
                                                                    AND M.SEMESTRE = T.SEMESTRE
                            INNER JOIN LY_CURSO C ( NOLOCK ) ON T.CURSO = C.CURSO
                            INNER JOIN LY_UNIDADE_ENSINO UE ( NOLOCK ) ON UE.UNIDADE_ENS = T.FACULDADE
                            INNER JOIN LY_TURNO TU ( NOLOCK ) ON TU.TURNO = T.TURNO
                    WHERE   M.ALUNO = @ALUNO
                            AND M.ANO = @ANO
                            AND M.SEMESTRE IN ( {0} )
                            AND M.SITUACAO_HIST <> 'Cancelado'
                            AND M.SITUACAO_HIST <> 'Dispensado'
                            AND M.SITUACAO_HIST <> 'Inconcluido'
                            AND ISNULL(M.DEPENDENCIA, 'N') = 'N'
                            AND ISNULL(M.MAIS_EDUCACAO, 'N') = 'N'
                            AND ISNULL(M.EDUC_ESPECIAL, 'N') = 'N'
                            AND ISNULL(M.CONCOMITANTE, 'N') = 'N'
                            AND ISNULL(M.OPTATIVAREFORCO, 'N') = 'N' ", possiveisPeriodos);

                contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
                contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);

                dadosTurma = ctx.GetDataTable(contextQuery);
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
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return dadosTurma;
        }

        public void SalvaTelefoneResponsavel(decimal pessoa, string telefone)
        {
            //Monta string de conexão para outro banco
            string stringConexao = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["StringConexao"].ConnectionString;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLockWithConnectionString(stringConexao);
            ContextQuery contextQuery = new ContextQuery();
            bool existeFieldPessoa = false;

            try
            {
                //Verifica se existe fild para a pessoa
                existeFieldPessoa = ExisteFieldPessoaPor(stringConexao, pessoa);

                if (existeFieldPessoa)
                {
                    contextQuery.Command = @" UPDATE  LY_FL_PESSOA
                                SET     FL_FIELD_06 = @TELEFONE
                                WHERE   PESSOA = @PESSOA ";
                }
                else
                {
                    contextQuery.Command = @" INSERT  INTO LY_FL_PESSOA
                                        ( PESSOA, FL_FIELD_06 )
                                VALUES  ( @PESSOA, @TELEFONE ) ";
                }

                contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);
                contextQuery.Parameters.Add("@TELEFONE", TechneDbType.T_ALFAEXTRALARGE, telefone);

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

        public bool ExisteFieldPessoaPor(string stringConexao, decimal pessoa)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLockWithConnectionString(stringConexao);
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                            FROM    LY_FL_PESSOA
                            WHERE   PESSOA = @PESSOA ";

                contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    existe = true;
                }

                return existe;
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

        public DataTable ObtemSituacaoFinalPor(int ano, int periodo, string aluno, string turma)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT  CASE SITUACAO_FINAL
                                      WHEN 'APROVADO COM DEP' THEN 'APROVADO COM DEPENDÊNCIA'
                                      WHEN 'REP FALTA' THEN 'REPROVADO POR FALTA'
                                      WHEN 'REP FREQ' THEN 'REPROVADO POR FALTA'
                                      WHEN 'REP NOTA' THEN 'REPROVADO POR NOTA'
                                      WHEN 'PROMOVIDO' THEN 'PROMOVIDO COM CONTINUIDADE CURRICULAR'
                                      ELSE SITUACAO_FINAL
                                    END SITUACAO_FINAL
                                    ,FREQUENCIA_GLOBAL
                        FROM    DBO.TCE_SITUACAO_FINAL_ALUNO (NOLOCK)
                        WHERE   ALUNO = @ALUNO
                                AND ANO = @ANO
                                AND PERIODO = @PERIODO
                                AND TURMA = @TURMA ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Decimal, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Decimal, periodo);
                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);

                dt = ctx.GetDataTable(contextQuery);

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
            return dt;
        }

        public decimal ObtemFrequenciaGlobalAtualPor(int ano, int periodo, string aluno, string turma)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            decimal frequenciaGlobal = 0;
            SqlDataReader reader = null;

            try
            {
                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"SP_OBTEMFREQUENCIAGLOBALATUAL";
                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@TURMA", turma);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    frequenciaGlobal = Convert.ToDecimal(reader["PERCENTUAL_FREQUENCIA"]);
                }

                return frequenciaGlobal;
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();

            }
        }
    }
}