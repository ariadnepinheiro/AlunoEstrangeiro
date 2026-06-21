using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.Matriculas
{
    public class OpcaoInscricaoHist
    {
        public void Insere(DataContext contexto, int opcaoInscricaoId, bool confirmado, int? motivoRejeicaoInscricaoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Matricula.OPCAOINSCRICAOHIST 
                                                (INSCRICAOALUNOID, 
                                                 OPCAOINSCRICAOID, 
                                                 CONTROLEVAGAID, 
                                                 TIPOFILAID, 
                                                 DATACONVOCACAO, 
                                                 PRAZORESPOSTA,
                                                 CONFIRMADO, 
                                                 DATASITUACAO, 
                                                 TIPOCANDIDATOID,
                                                 MOTIVOREJEICAOINSCRICAOID, 
                                                 FASE, 
                                                 VAGACONCORRENTE, 
                                                 MOTIVORETORNOID,
                                                 DATARETORNO,
                                                 USUARIOID, 
                                                 DATACADASTRO, 
                                                 DATAALTERACAO) 
                                    SELECT INSCRICAOALUNOID, 
                                           OPCAOINSCRICAOID, 
                                           CONTROLEVAGAID, 
                                           TIPOFILAID, 
                                           DATACONVOCACAO, 
                                           PRAZORESPOSTA,
                                           @CONFIRMADO, 
                                           @DATASITUACAO, 
                                           TIPOCANDIDATOID,
                                           @MOTIVOREJEICAOINSCRICAOID, 
                                           FASE,
                                           VAGACONCORRENTE, 
                                           MOTIVORETORNOID,
                                           DATARETORNO,
                                           USUARIOID, 
                                           DATACADASTRO, 
                                           @DATASITUACAO 
                                    FROM   Matricula.OPCAOINSCRICAO 
                                    WHERE  OPCAOINSCRICAOID = @OPCAOINSCRICAOID ";

            contextQuery.Parameters.Add("@OPCAOINSCRICAOID", SqlDbType.Int, opcaoInscricaoId);
            contextQuery.Parameters.Add("@CONFIRMADO", SqlDbType.Bit, confirmado);
            contextQuery.Parameters.Add("@DATASITUACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@MOTIVOREJEICAOINSCRICAOID", SqlDbType.Int, motivoRejeicaoInscricaoId);

            contexto.ApplyModifications(contextQuery);
        }

        public void Remove(DataContext contexto, int opcaoInscricaoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE Matricula.OPCAOINSCRICAOHIST 
                                    WHERE  OPCAOINSCRICAOID = @OPCAOINSCRICAOID ";

            contextQuery.Parameters.Add("@OPCAOINSCRICAOID", SqlDbType.Int, opcaoInscricaoId);

            contexto.ApplyModifications(contextQuery);
        }

        public Matriculas.Entidades.OpcaoInscricaoHist ObtemPor(int opcaoInscricaoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            Matriculas.Entidades.OpcaoInscricaoHist opcaoInscricaoHist = new Techne.Lyceum.RN.Matriculas.Entidades.OpcaoInscricaoHist();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT * 
                                            FROM MATRICULA.OPCAOINSCRICAOHIST (NOLOCK) 
                                            WHERE OPCAOINSCRICAOID = @OPCAOINSCRICAOID ";

                contextQuery.Parameters.Add("@OPCAOINSCRICAOID", SqlDbType.Int, opcaoInscricaoId); 

                opcaoInscricaoHist = contexto.TryToBindEntity<Matriculas.Entidades.OpcaoInscricaoHist>(contextQuery);

                return opcaoInscricaoHist;
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

        public bool PossuiMotivoPor(DataContext contexto, int motivoRejeicaoInscricaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM MATRICULA.OPCAOINSCRICAOHIST (NOLOCK)
                                    WHERE MOTIVOREJEICAOINSCRICAOID = @MOTIVOREJEICAOINSCRICAOID ";

            contextQuery.Parameters.Add("@MOTIVOREJEICAOINSCRICAOID", SqlDbType.Int, motivoRejeicaoInscricaoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiMotivoRetornoPor(DataContext contexto, int motivoRetornoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM MATRICULA.OPCAOINSCRICAOHIST (NOLOCK)
                                    WHERE MOTIVOREJEICAOINSCRICAOID = @MOTIVORETORNOID ";

            contextQuery.Parameters.Add("@MOTIVORETORNOID", SqlDbType.Int, motivoRetornoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable ListaPor(int inscricaoAlunoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            RN.PeriodoLetivo rnPeriodoLetivo = new PeriodoLetivo();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"  SELECT O.OPCAOINSCRICAOID, 
                                   O.DATACADASTRO, 
                                   CV.CENSO, 
                                   UE.NOME_COMP AS ESCOLA, 
                                   CV.CURSO, 
                                   CV.TURNO, 
                                   CV.SERIE, 
                                   C.NOME       AS DESCRICAOCURSO, 
                                   TU.DESCRICAO AS DESCRICAOTURNO, 
                                   T.DESCRICAO  AS DESCRICAOTIPO, 
                                  M.DESCRICAO + ' - ' + C.NOME AS DESCRICAOMODALIDADE, 
                                   CASE 
                                        WHEN CONFIRMADO = 1 THEN 'Confirmado'
                                        ELSE 'Não Confirmado'
                                   END SITUACAO, 
                                   MR.DESCRICAO AS MOTIVO, 
                                   A.NUM_OPCAO AS OPCAOFASE1,
                                   O.DATASITUACAO 
                            FROM   MATRICULA.OPCAOINSCRICAOHIST O (NOLOCK) 
                                   LEFT JOIN MATRICULA.MOTIVOREJEICAOINSCRICAO MR (NOLOCK) 
                                          ON O.MOTIVOREJEICAOINSCRICAOID = MR.MOTIVOREJEICAOINSCRICAOID 
                                   INNER JOIN TCE_CONTROLE_VAGA CV (NOLOCK) 
                                           ON O.CONTROLEVAGAID = CV.ID_CONTROLE_VAGA 
                                   INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK) 
                                           ON CV.CENSO = UE.UNIDADE_ENS 
                                   INNER JOIN LY_CURSO C (NOLOCK) 
                                           ON CV.CURSO = C.CURSO 
                                   INNER JOIN LY_TIPO_CURSO T (NOLOCK) 
                                           ON C.TIPO = T.TIPO 
                                   INNER JOIN LY_TURNO TU (NOLOCK) 
                                           ON CV.TURNO = TU.TURNO 
                                   INNER JOIN LY_MODALIDADE_CURSO M (NOLOCK) 
                                           ON C.MODALIDADE = M.MODALIDADE 
								   LEFT JOIN ALOCACAO.MATR.OPCOES_SEEDUC A (NOLOCK) 
										   ON A.ID_OPCAOESCOLA = O.OPCAOINSCRICAOID 
                            WHERE  INSCRICAOALUNOID = @INSCRICAOALUNOID                   
                            ORDER BY O.DATACADASTRO, O.OPCAOINSCRICAOID ";

                contextQuery.Parameters.Add("@INSCRICAOALUNOID", SqlDbType.Int, inscricaoAlunoId);

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

        public void InsereRetiradaFila(DataContext contexto, int ano, int periodo, string curso, int motivoRetiradaFila)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Matricula.OPCAOINSCRICAOHIST 
                                                (INSCRICAOALUNOID, 
                                                    OPCAOINSCRICAOID, 
                                                    CONTROLEVAGAID, 
                                                    TIPOCANDIDATOID,
                                                    TIPOFILAID, 
                                                    DATACONVOCACAO, 
                                                    PRAZORESPOSTA,
                                                    CONFIRMADO, 
                                                    MOTIVOREJEICAOINSCRICAOID,
                                                    DATASITUACAO, 
                                                    USUARIOID, 			
                                                    DATACADASTRO, 
                                                    DATAALTERACAO, 
				                                    FASE,  
				                                    VAGACONCORRENTE) 
                                    SELECT O.INSCRICAOALUNOID, 
                                            O.OPCAOINSCRICAOID, 
                                            O.CONTROLEVAGAID, 
		                                    O.TIPOCANDIDATOID,
                                            O.TIPOFILAID,         
                                            O.DATACONVOCACAO, 
                                            O.PRAZORESPOSTA,
                                            0, 
                                            @MOTIVOREJEICAOINSCRICAOID,
                                            @DATASITUACAO, 
                                            O.USUARIOID,
                                            O.DATACADASTRO, 
                                            GETDATE(),
		                                    FASE,  
		                                    VAGACONCORRENTE 
                                    FROM   MATRICULA.OPCAOINSCRICAO O
		                                    INNER JOIN MATRICULA.INSCRICAOALUNO I ON O.INSCRICAOALUNOID = I.INSCRICAOALUNOID		
		                                    INNER JOIN TCE_CONTROLE_VAGA CV ON O.CONTROLEVAGAID = ID_CONTROLE_VAGA
                                    WHERE  CV.ANO = @ANO
	                                       AND CV.PERIODO = @PERIODO 
	                                       AND cv.CURSO = @CURSO
	                                       AND O.DATACONVOCACAO IS NULL  ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
            contextQuery.Parameters.Add("@DATASITUACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@MOTIVOREJEICAOINSCRICAOID", SqlDbType.Int, motivoRetiradaFila);

            contexto.ApplyModifications(contextQuery);
        }

        public void InsereRetiradaFila(DataContext contexto, int controleVagaId, int motivoRetiradaFila)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Matricula.OPCAOINSCRICAOHIST 
                                                (INSCRICAOALUNOID, 
                                                    OPCAOINSCRICAOID, 
                                                    CONTROLEVAGAID, 
                                                    TIPOCANDIDATOID,
                                                    TIPOFILAID, 
                                                    DATACONVOCACAO, 
                                                    PRAZORESPOSTA,
                                                    CONFIRMADO, 
                                                    MOTIVOREJEICAOINSCRICAOID,
                                                    DATASITUACAO, 
                                                    USUARIOID, 			
                                                    DATACADASTRO, 
                                                    DATAALTERACAO, 
				                                    FASE,  
				                                    VAGACONCORRENTE) 
                                    SELECT O.INSCRICAOALUNOID, 
                                            O.OPCAOINSCRICAOID, 
                                            O.CONTROLEVAGAID, 
		                                    O.TIPOCANDIDATOID,
                                            O.TIPOFILAID,         
                                            O.DATACONVOCACAO, 
                                            O.PRAZORESPOSTA,
                                            0, 
                                            @MOTIVOREJEICAOINSCRICAOID,
                                            @DATASITUACAO, 
                                            O.USUARIOID,
                                            O.DATACADASTRO, 
                                            GETDATE(),
		                                    FASE,  
		                                    VAGACONCORRENTE 
                                    FROM   MATRICULA.OPCAOINSCRICAO O
		                                    INNER JOIN MATRICULA.INSCRICAOALUNO I ON O.INSCRICAOALUNOID = I.INSCRICAOALUNOID
                                    WHERE  O.CONTROLEVAGAID = @CONTROLEVAGAID
	                                       AND O.DATACONVOCACAO IS NULL ";

            contextQuery.Parameters.Add("@CONTROLEVAGAID", SqlDbType.Int, controleVagaId);
            contextQuery.Parameters.Add("@DATASITUACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@MOTIVOREJEICAOINSCRICAOID", SqlDbType.Int, motivoRetiradaFila);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
