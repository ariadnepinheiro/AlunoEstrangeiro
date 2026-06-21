using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using Techne.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.AvaliacaoExterna
{
    public class AlunoParticipante
    {
        public bool PossuiStatusParticipacaoPor(DataContext contexto, int situacaoParticipanteId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM AvaliacaoExterna.ALUNOPARTICIPANTE (NOLOCK)
                                        WHERE SITUACAOPARTICIPANTEID = @SITUACAOPARTICIPANTEID ";

            contextQuery.Parameters.Add("@SITUACAOPARTICIPANTEID", SqlDbType.Int, situacaoParticipanteId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiComponentePor(DataContext ctx, int componenteId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT COUNT(0) 
                                FROM [AvaliacaoExterna].[ALUNOPARTICIPANTE] (NOLOCK)
                                WHERE COMPONENTEID = @COMPONENTEID ";

            contextQuery.Parameters.Add("@COMPONENTEID", SqlDbType.Int, componenteId);

            return ctx.GetReturnValue<int>(contextQuery) > 0;
        }

        public DataTable ListaAlunoPor(int provaId, string turma, int semestre, int ano)
        {
            DataTable dt = null;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Verifica se eh o ano atual
                if (ano == DateTime.Now.Year)
                {
                    //Para ano atual busca na matricula
                    dt = this.ListaAlunoMatriculaPor(ctx, provaId, turma, semestre, ano);
                }
                else
                {
                    //Para anos anteriores busca na historico
                    dt = this.ListaAlunoHistoricoPor(ctx, provaId, turma, semestre, ano);
                }
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

        public DataTable ListaAlunoMatriculaPor(DataContext contexto, int provaId, string turma, int semestre, int ano)
        {
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            contextQuery.Command = @" SELECT  DISTINCT T.TURMA,
                                                  T.SERIE,
	                                              PR.PROVAID,
	                                              T.ANO,
	                                              T.SEMESTRE AS PERIODO,
	                                              m.ALUNO,
	                                              P.NOME_COMPL,
	                                              p.DT_NASC,
	                                              p.SEXO
                                            FROM LY_TURMA T (NOLOCK)
	                                            INNER JOIN DBO.LY_MATRICULA M (NOLOCK)
				                                            ON T.TURMA = M.TURMA 
					                                            AND T.SEMESTRE = M.SEMESTRE 
					                                            AND T.ANO = M.ANO
	                                            INNER JOIN dbo.LY_ALUNO A (NOLOCK)
				                                            ON a.ALUNO = m.ALUNO
	                                            INNER JOIN LY_PESSOA P  (NOLOCK)
				                                            ON P.PESSOA = A.PESSOA
	                                            INNER JOIN AVALIACAOEXTERNA.AVALIACAO AV (NOLOCK)
				                                            ON T.ANO = AV.ANO
	                                            INNER JOIN AVALIACAOEXTERNA.PROVA PR (NOLOCK)
				                                            ON AV.AVALIACAOID = PR.AVALIACAOID
                                            WHERE T.TURMA = @TURMA
	                                            AND T.ANO = @ANO
	                                            AND T.SEMESTRE = @SEMESTRE
	                                            AND PR.PROVAID = @PROVAID
	                                            AND M.SIT_MATRICULA = 'Matriculado'
	                                            AND ISNULL(M.DEPENDENCIA, 'N') = 'N'
                                            ORDER BY P.NOME_COMPL ASC ";

            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, semestre);
            contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, provaId);

            dt = contexto.GetDataTable(contextQuery);

            return dt;
        }

        public DataTable ListaAlunoHistoricoPor(DataContext contexto, int provaId, string turma, int semestre, int ano)
        {
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            contextQuery.Command = @" SELECT  DISTINCT T.TURMA,
                                                  T.SERIE,
	                                              PR.PROVAID,
	                                              T.ANO,
	                                              T.SEMESTRE AS PERIODO,
	                                              m.ALUNO,
	                                              P.NOME_COMPL,
	                                              p.DT_NASC,
	                                              p.SEXO
                                            FROM LY_TURMA T (NOLOCK)
	                                            INNER JOIN DBO.LY_HISTMATRICULA M 
				                                            ON T.TURMA = M.TURMA 
					                                            AND T.SEMESTRE = M.SEMESTRE 
					                                            AND T.ANO = M.ANO
	                                            INNER JOIN dbo.LY_ALUNO A 
				                                            ON a.ALUNO = m.ALUNO
	                                            INNER JOIN LY_PESSOA P 
				                                            ON P.PESSOA = A.PESSOA
	                                            INNER JOIN AVALIACAOEXTERNA.AVALIACAO AV
				                                            ON T.ANO = AV.ANO
	                                            INNER JOIN AVALIACAOEXTERNA.PROVA PR
				                                            ON AV.AVALIACAOID = PR.AVALIACAOID
                                            WHERE T.TURMA = @TURMA
	                                            AND T.ANO = @ANO
	                                            AND T.SEMESTRE = @SEMESTRE
	                                            AND PR.PROVAID = @PROVAID
	                                            and m.SITUACAO_HIST <> 'Cancelado'
	                                            AND ISNULL(M.DEPENDENCIA, 'N') = 'N'
                                            ORDER BY P.NOME_COMPL ASC ";

            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, semestre);
            contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, provaId);

            dt = contexto.GetDataTable(contextQuery);

            return dt;
        }

        public bool EstaConsistente(IList<string> alunosGrid, int provaId, string turma, int semestre, int ano)
        {
            List<string> alunosBd = ListaAlunoPor(provaId, turma, semestre, ano)
                    .ToList<Entidades.AlunoParticipante>()
                    .Select(s => s.Aluno)
                    .ToList();

            //a contagem de alunos do grid e do bd tem que ser iguais
            if (alunosGrid.Count != alunosBd.Count)
                return false;

            //e todos os alunos do grid tem que estar contidos na lista de alunos que veio do banco de dados
            if (!alunosGrid.All(q => alunosBd.Contains(q)))
                return false;

            //e todos os alunos que vieram do bd tem que estar contidos na lista de alunos que está no grid
            if (!alunosBd.All(q => alunosGrid.Contains(q)))
                return false;

            //se passou por todas as validações, retornar true (consistente)
            return true;
        }

        public void Carrega(int provaId, string turma, int ano, int semestre, out IList<Entidades.AlunoParticipante> alunos, out IList<Entidades.Questao> questoes, out List<Entidades.Resposta> respostas)
        {
            RN.AvaliacaoExterna.Resposta rnResposta = new Resposta();
            RN.AvaliacaoExterna.Questao rnQuestao = new Questao();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            respostas = new List<Entidades.Resposta>();

            try
            {
                //Verifica se eh o ano atual
                if (ano == DateTime.Now.Year)
                {
                    //Para ano atual busca na matricula
                    alunos = this.CarregaAlunosMatriculadosParticipantes(ctx, provaId, turma, ano, semestre);
                }
                else
                {
                    //Para anos anteriores busca na historico
                    alunos = this.CarregaAlunosHistoricoParticipantes(ctx, provaId, turma, ano, semestre);
                }

                questoes = rnQuestao.CarregaQuestoes(ctx, provaId);

                if (alunos != null)
                {
                    //Busca todos as repostas para cada aluno da turma
                    foreach (string aluno in alunos.Select(x => x.Aluno).Distinct().ToList())
                    {
                        var respostasDestaIteracao = rnResposta.CarregaRespostas(ctx, provaId, aluno);
                        if (respostasDestaIteracao != null && respostasDestaIteracao.Any())
                            respostas.AddRange(respostasDestaIteracao.ToList());
                    }
                }
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

        public void Carrega(int provaId, string turma, int ano, int semestre,
                    out IList<Entidades.AlunoParticipante> alunos,
                    out IList<Entidades.Questao> questoes,
                    out List<Entidades.Resposta> respostas,
                    out IList<Entidades.SituacaoParticipante> situacoes
)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            RN.AvaliacaoExterna.SituacaoParticipante rnSituacaoParticipante = new SituacaoParticipante();
            RN.AvaliacaoExterna.Questao rnQuestao = new Questao();
            RN.AvaliacaoExterna.Resposta rnResposta = new Resposta();
            respostas = new List<Entidades.Resposta>();

            try
            {
                //Verifica se eh o ano atual
                if (ano == DateTime.Now.Year)
                {
                    //Para ano atual busca na matricula
                    alunos = this.CarregaAlunosMatriculadosParticipantes(ctx, provaId, turma, ano, semestre);
                }
                else
                {
                    //Para anos anteriores busca na historico
                    alunos = this.CarregaAlunosHistoricoParticipantes(ctx, provaId, turma, ano, semestre);
                }

                questoes = rnQuestao.CarregaQuestoes(ctx, provaId);

                //Busca todos as repostas para cada aluno da turma
                foreach (string aluno in alunos.Select(x => x.Aluno).Distinct().ToList())
                {
                    var respostasDestaIteracao = rnResposta.CarregaRespostas(ctx, provaId, aluno);
                    if (respostasDestaIteracao != null && respostasDestaIteracao.Any())
                        respostas.AddRange(respostasDestaIteracao.ToList());
                }

                situacoes = rnSituacaoParticipante.ListaPor(ctx);
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


        public IList<Entidades.AlunoParticipante> CarregaAlunosMatriculadosParticipantes(DataContext ctx, int provaId, string turma, int ano, int semestre)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT DISTINCT a.* 
                                    FROM AVALIACAOEXTERNA.ALUNOPARTICIPANTE A
										 INNER JOIN DBO.LY_MATRICULA M ON A.ALUNO = M.ALUNO
															AND M.ANO = @ANO
															AND M.SEMESTRE = @SEMESTRE
															AND M.TURMA = @TURMA
                                    WHERE PROVAID = @PROVAID
											AND M.SIT_MATRICULA = 'Matriculado'
	                                        AND ISNULL(M.DEPENDENCIA, 'N') = 'N' ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, semestre);
            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
            contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, provaId);

            return ctx.GetDataTable(contextQuery).ToList<Entidades.AlunoParticipante>();
        }

        public IList<Entidades.AlunoParticipante> CarregaAlunosHistoricoParticipantes(DataContext ctx, int provaId, string turma, int ano, int semestre)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT DISTINCT a.* 
                                    FROM AVALIACAOEXTERNA.ALUNOPARTICIPANTE A
										 INNER JOIN DBO.LY_HISTMATRICULA M ON A.ALUNO = M.ALUNO
															AND M.ANO = @ANO
															AND M.SEMESTRE = @SEMESTRE
															AND M.TURMA = @TURMA
                                    WHERE PROVAID = @PROVAID
											AND M.SITUACAO_HIST <> 'Cancelado'
	                                        AND ISNULL(M.DEPENDENCIA, 'N') = 'N' ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, semestre);
            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
            contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, provaId);

            return ctx.GetDataTable(contextQuery).ToList<Entidades.AlunoParticipante>();
        }

        public int ObtemIdPor(DataContext contexto, string aluno, int provaId, int componeneteId)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {


                contextQuery.Command = @" SELECT ALUNOPARTICIPANTEID
                                        FROM [AVALIACAOEXTERNA].[ALUNOPARTICIPANTE] (NOLOCK)
                                        WHERE PROVAID = @PROVAID
	                                        AND ALUNO = @ALUNO
	                                        AND COMPONENTEID = @COMPONENTEID ";

                contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, provaId);
                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
                contextQuery.Parameters.Add("@COMPONENTEID", SqlDbType.Int, componeneteId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["ALUNOPARTICIPANTEID"]);
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

        public void SalvaAlunoParticipante(DataContext ctx, Entidades.AlunoParticipante aluno)
        {
            this.Insere(ctx, aluno);
        }

        private void Insere(DataContext contexto, Entidades.AlunoParticipante aluno)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO AvaliacaoExterna.ALUNOPARTICIPANTE
                                           (PROVAID
                                           ,COMPONENTEID
                                           ,ALUNO
                                           ,SITUACAOPARTICIPANTEID
                                           ,DATAPARTICIPACAO
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
                                     VALUES
                                           (@PROVAID, 
                                           @COMPONENTEID, 
                                           @ALUNO, 
                                           @SITUACAOPARTICIPANTEID, 
                                           @DATAPARTICIPACAO, 
                                           @USUARIOID, 
                                           @DATACADASTRO, 
                                           @DATAALTERACAO)
                                    
                                    SELECT IDENT_CURRENT('AvaliacaoExterna.ALUNOPARTICIPANTE') ";

            contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, aluno.ProvaId);
            contextQuery.Parameters.Add("@COMPONENTEID", SqlDbType.Int, aluno.ComponenteId);
            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno.Aluno);
            contextQuery.Parameters.Add("@SITUACAOPARTICIPANTEID", SqlDbType.VarChar, aluno.SituacaoParticipanteId);
            contextQuery.Parameters.Add("@DATAPARTICIPACAO", SqlDbType.DateTime, aluno.DataParticipacao);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, aluno.UsuarioID);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            aluno.AlunoParticipanteId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        private void Atualiza(DataContext contexto, Entidades.AlunoParticipante aluno)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE AvaliacaoExterna.ALUNOPARTICIPANTE
                                       SET SITUACAOPARTICIPANTEID = @SITUACAOPARTICIPANTEID, 
                                           DATAPARTICIPACAO = @DATAPARTICIPACAO, 
                                           USUARIOID = @USUARIOID, 
                                           DATAALTERACAO = @DATAALTERACAO
                                     WHERE ALUNOPARTICIPANTEID = @ALUNOPARTICIPANTEID ";

            contextQuery.Parameters.Add("@SITUACAOPARTICIPANTEID", SqlDbType.Int, aluno.SituacaoParticipanteId);
            contextQuery.Parameters.Add("@DATAPARTICIPACAO", SqlDbType.DateTime, aluno.DataParticipacao);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, aluno.UsuarioID);
            contextQuery.Parameters.Add("@ALUNOPARTICIPANTEID", SqlDbType.Int, aluno.AlunoParticipanteId);

            contexto.ApplyModifications(contextQuery);
        }

        private void Remove(DataContext contexto, Entidades.AlunoParticipante aluno)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE AvaliacaoExterna.ALUNOPARTICIPANTE                                       
                                     WHERE ALUNO = @ALUNO
                                    AND PROVAID = @PROVAID ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno.Aluno);
            contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, aluno.ProvaId);

            contexto.ApplyModifications(contextQuery);
        }
    }
}