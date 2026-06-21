using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using System.Data;
using Techne.Lyceum.RN.DTOs;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.Turmas
{
    public class HistoricoSuspensao
    {
        public void EncerraSuspensaoMatricula(DataContext contexto, string aluno, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE [Turma].[HISTORICOSUSPENSAO]
                                    SET     DATA_CANCELADO = GETDATE(),
                                            USUARIOID = @USUARIOID,
                                            DATAALTERACAO = GETDATE()
                                    WHERE   ALUNO = @ALUNO 
                                            AND DATA_RETORNO IS NULL
	                                        AND DATA_CANCELADO IS NULL ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuario);

            contexto.ApplyModifications(contextQuery);
        }

        public void ReativaMatricula(DataContext contexto, string aluno, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE [Turma].[HISTORICOSUSPENSAO]
                                    SET     DATA_RETORNO = GETDATE(),
                                            USUARIO_RETORNO = @USUARIOID,
                                            USUARIOID = @USUARIOID,
                                            DATAALTERACAO = GETDATE()
                                    WHERE   ALUNO = @ALUNO 
                                            AND DATA_RETORNO IS NULL
	                                        AND DATA_CANCELADO IS NULL ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuario);

            contexto.ApplyModifications(contextQuery);
        }

        public Entidades.HistoricoSuspensao ObtemSuspensaoAtivaPor(string aluno)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            Entidades.HistoricoSuspensao historicoSuspensao = new Entidades.HistoricoSuspensao();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT *
                                        FROM Turma.HISTORICOSUSPENSAO
                                        WHERE ALUNO = @ALUNO
	                                        AND DATA_RETORNO IS NULL
	                                        AND DATA_CANCELADO IS NULL ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);

                historicoSuspensao = contexto.TryToBindEntity<Entidades.HistoricoSuspensao>(contextQuery);

                return historicoSuspensao;
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

        public DataTable ListaAlunoParaSuspenderPor(int ano, int periodo, string censo, int regional)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(@" SELECT  DISTINCT H.HISTORICOSUSPENSAOID,
	                                    R.REGIONAL,
	                                    MU.NOME AS MUNICIPIO,
	                                    UE.NOME_COMP AS ESCOLA,
	                                    UE.UNIDADE_ENS,
		                                M.TURMA,
	                                    A.ALUNO,
	                                    P.NOME_COMPL,
	                                    CONVERT(VARCHAR,H.DATA_EM_SUSPENSAO,103) AS DATA_EM_SUSPENSAO,
	                                    H.DIASFALTASSUSPENSAO,
	                                    CONVERT(VARCHAR,H.INICIOFALTASUSPENSAO,103) AS INICIOFALTASUSPENSAO,
	                                    CONVERT(VARCHAR,H.FIMFALTASUSPENSAO,103) AS FIMFALTASUSPENSAO
                                    FROM [TURMA].[HISTORICOSUSPENSAO] H
	                                    INNER JOIN LY_ALUNO A ON H.ALUNO = A.ALUNO
	                                    INNER JOIN LY_UNIDADE_ENSINO UE ON A.UNIDADE_ENSINO = UE.UNIDADE_ENS
	                                    INNER JOIN TCE_REGIONAL R ON UE.ID_REGIONAL = R.ID_REGIONAL
	                                    INNER JOIN MUNICIPIO MU ON UE.MUNICIPIO = MU.CODIGO
	                                    INNER JOIN LY_PESSOA P ON A.PESSOA = P.PESSOA
		                                INNER JOIN LY_MATRICULA M (NOLOCK) ON  M.ALUNO = A.ALUNO
                                        INNER JOIN DBO.LY_TURMA T (NOLOCK) ON M.DISCIPLINA = T.DISCIPLINA
                                                                                                AND M.TURMA = T.TURMA
                                                                                                AND M.ANO = T.ANO
                                                                                                AND M.SEMESTRE = T.SEMESTRE	
                                    WHERE ISNULL(DIASFALTASSUSPENSAO, 0) > 25 
	                                    AND ATIVOPARASUSPENDER = 1
                                        AND SIT_ALUNO = 'ATIVO'
	                                    AND ISNULL(A.SUSPENSO, 0) = 1
		                                AND M.SIT_MATRICULA = 'Matriculado'
                                        AND T.OPTATIVAREFORCO = 'N'
                                        AND ISNULL(T.ELETIVA,'N') = 'N'
                                        AND ISNULL(M.DEPENDENCIA, 'N') = 'N'
                                        AND ISNULL(M.EDUC_ESPECIAL, 'N') = 'N'
                                        AND ISNULL(M.MAIS_EDUCACAO, 'N') = 'N'
                                        AND ISNULL(M.CONCOMITANTE, 'N') = 'N'
		                                AND M.ANO = @ANO
		                                AND M.SEMESTRE = @PERIODO ");

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);

                if (!censo.IsNullOrEmptyOrWhiteSpace())
                {
                    sql.Append(@" AND A.UNIDADE_ENSINO = @CENSO ");
                }
                else if (regional > 0)
                {
                    sql.Append(@" AND UE.ID_REGIONAL = @REGIONAL ");
                }

                contextQuery.Command = sql.ToString();

                if (!censo.IsNullOrEmptyOrWhiteSpace())
                {
                    contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                }
                else if (regional > 0)
                {
                    contextQuery.Parameters.Add("@REGIONAL", SqlDbType.Int, regional);
                }

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

        public ValidacaoDados ValidaSuspendeAluno(int ano, int periodo, List<int> ListaHistoricoSuspensaoId, string usuarioResponsavel)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (ListaHistoricoSuspensaoId == null)
            {
                return validacaoDados;
            }

            if (ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (periodo < 0)
            {
                mensagens.Add("Campo PERÍODO é obrigatório.");
            }

            if (ListaHistoricoSuspensaoId.Count <= 0)
            {
                mensagens.Add("Nenhum aluno selecionado.");
            }

            if (usuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSAVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    foreach (int historicoSuspensaoId in ListaHistoricoSuspensaoId)
                    {
                        //Verifica se o aluno esta suspenso
                        if (!this.EhAlunoEmSuspensoPor(contexto, historicoSuspensaoId))
                        {
                            string aluno = this.ObtemAlunoPor(contexto, historicoSuspensaoId);

                            mensagens.Add(string.Format("O aluno {0} não está em suspensão.", aluno));
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

        public void SuspendeAluno(int ano, int periodo, List<int> ListaHistoricoSuspensaoId, string usuarioResponsavel)
        {
            HCursosConcl rnHCursosConcl = new HCursosConcl();
            RN.Matricula rnMatricula = new Matricula();
            RN.Matgrade rnMatgrade = new Matgrade();
            RN.Carteirinha rnCarteirinha = new Carteirinha();
            RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            RN.Matriculas.PessoaAluno rnPessoaAluno = new Techne.Lyceum.RN.Matriculas.PessoaAluno();
            RN.RenovacaoMatricula.Renovacao rnRenovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Renovacao();
            Matriculas.OpcaoInscricao rnOpcaoInscricao = new Techne.Lyceum.RN.Matriculas.OpcaoInscricao();
            RN.Aluno rnAluno = new Aluno();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                foreach (int historicoSuspensaoId in ListaHistoricoSuspensaoId)
                {
                    //ATUALIZA AS INFORMAÇÕES DO CANCELAMENTO
                    rnHCursosConcl.InsereSuspensao(ctx, historicoSuspensaoId, ano, periodo);

                    //CANCELA A ENTURMAÇÃO
                    rnMatricula.AtualizaSuspensao(ctx, ano, historicoSuspensaoId, usuarioResponsavel);

                    //CANCELA MATGRADE 
                    rnMatgrade.AtualizaSuspensao(ctx, historicoSuspensaoId);

                    //BLOQUEIA A CARTEIRINHA
                    rnCarteirinha.AtualizaSuspensao(ctx, historicoSuspensaoId);

                    //CANCELAMENTO DA CONFIRMAÇÃO DE MATRICULA 
                    rnConfirmacaoMatricula.AtualizaSuspensao(ctx, ano, historicoSuspensaoId);

                    //FECHA A PESSOA VIGENTE
                    rnPessoaAluno.AtualizaSuspensao(ctx, ano, periodo, historicoSuspensaoId, usuarioResponsavel);

                    //CANCELAMENTO DA RENOVAÇÃO 
                    rnRenovacao.AtualizaSuspensao(ctx, ano, historicoSuspensaoId);

                    //Cancela opções inscrição
                    rnOpcaoInscricao.AtualizaSuspensao(ctx, historicoSuspensaoId);

                    //ATUALIZA DATA DA SUSPENSAO
                    AtualizaSuspensao(ctx, historicoSuspensaoId);

                    //ATUALIZA SITUAÇÃO ALUNO
                   rnAluno.AtualizaSuspensao(ctx, historicoSuspensaoId);
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

        private string ObtemAlunoPor(DataContext contexto, int historicoSuspensaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT ALUNO
                            FROM    Turma.HISTORICOSUSPENSAO
                            WHERE   HISTORICOSUSPENSAOID = @HISTORICOSUSPENSAOID ";


            contextQuery.Parameters.Add("@HISTORICOSUSPENSAOID", SqlDbType.Int, historicoSuspensaoId);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        private bool EhAlunoEmSuspensoPor(DataContext contexto, int historicoSuspensaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*)
                                        FROM LY_ALUNO A 
	                                        INNER JOIN Turma.HISTORICOSUSPENSAO h ON h.ALUNO = A.ALUNO
                                        where HISTORICOSUSPENSAOID = @HISTORICOSUSPENSAOID
	                                        and A.SIT_ALUNO = 'Ativo'
	                                        and ISNULL(A.SUSPENSO, 0) = 1 ";

            contextQuery.Parameters.Add("@HISTORICOSUSPENSAOID", SqlDbType.Int, historicoSuspensaoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void AtualizaSuspensao(DataContext contexto, int historicoSuspensaoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Turma.HISTORICOSUSPENSAO 
                                        SET    DATA_SUSPENSAO = GETDATE()
		                                 where HISTORICOSUSPENSAOID = @HISTORICOSUSPENSAOID ";

            contextQuery.Parameters.Add("@HISTORICOSUSPENSAOID", SqlDbType.Int, historicoSuspensaoId);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
