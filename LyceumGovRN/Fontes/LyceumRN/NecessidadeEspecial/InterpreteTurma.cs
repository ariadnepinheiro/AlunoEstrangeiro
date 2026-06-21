using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.NecessidadeEspecial
{
    public class InterpreteTurma
    {
        public DateTime[] RetornaDataMinimaMaximaPor(DataContext contexto, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            DateTime[] datas = null;
            try
            {
                contextQuery.Command = @" SELECT MIN(DATAINICIO) AS DATAMINIMA, 
                                                   MAX(DATAFIM)    AS DATAMAXIMA 
                                            FROM   NECESSIDADEESPECIAL.INTERPRETETURMA T (NOLOCK) 
                                                   INNER JOIN LY_MATRICULA M (NOLOCK ) 
                                                           ON M.TURMA = T.TURMA 
                                                              AND M.ANO = T.ANO 
                                                              AND M.SEMESTRE = T.SEMESTRE 
                                            WHERE  M.ALUNO = @ALUNOID  ";

                contextQuery.Parameters.Add("@ALUNOID", TechneDbType.T_CODIGO, aluno);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    if (reader["DATAMINIMA"] != DBNull.Value || reader["DATAMAXIMA"] != DBNull.Value)
                    {
                        datas = new DateTime[2];

                        if (reader["DATAMINIMA"] != DBNull.Value)
                        {
                            datas[0] = Convert.ToDateTime(reader["DATAMINIMA"]);
                        }
                        if (reader["DATAMAXIMA"] != DBNull.Value)
                        {
                            datas[1] = Convert.ToDateTime(reader["DATAMAXIMA"]);
                        }
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return datas;
        }

        public bool EhInterpreteAtivoPor(DataContext contexto, int recursoNecessidadeEspecialId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM   NECESSIDADEESPECIAL.INTERPRETETURMA (NOLOCK) 
                                    WHERE  RECURSONECESSIDADEESPECIALID = @RECURSONECESSIDADEESPECIALID 
                                            AND DATAFIM >= CONVERT(DATE, GETDATE()) ";

                contextQuery.Parameters.Add("@RECURSONECESSIDADEESPECIALID", SqlDbType.Int, recursoNecessidadeEspecialId);

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
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
        }

        public DataTable ListaPor(string aluno)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT INTERPRETETURMAID, 
	                               M.ALUNO, 
	                               T.TURMA,
                                   T.RECURSONECESSIDADEESPECIALID, 
                                   P.NOME_COMPL, 
                                   P.CPF,
                                   T.DATAINICIO, 
                                   T.DATAFIM                                    
                            FROM   NECESSIDADEESPECIAL.INTERPRETETURMA T (NOLOCK ) 
                                   INNER JOIN NECESSIDADEESPECIAL.RECURSONECESSIDADEESPECIAL R (NOLOCK ) 
                                           ON T.RECURSONECESSIDADEESPECIALID = R.RECURSONECESSIDADEESPECIALID 
                                   INNER JOIN LY_PESSOA P (NOLOCK ) 
                                           ON R.PESSOAID = P.PESSOA 
	                               INNER JOIN LY_MATRICULA M (NOLOCK ) 
			                               ON M.TURMA = T.TURMA AND  M.ANO = T.ANO AND M.SEMESTRE = T.SEMESTRE
                            WHERE  M.ALUNO = @ALUNO 
							ORDER BY DATAINICIO DESC";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);

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

        public DataTable ListaPorCpf(string cpf)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT INTERPRETETURMAID, 
                                               LA.RECURSONECESSIDADEESPECIALID, 
                                               T.TURNO, 
	                                           T.TURMA,
                                               LA.DATAINICIO, 
                                               LA.DATAFIM,
                                               T.ANO,
                                               T.SEMESTRE
                                        FROM   NECESSIDADEESPECIAL.INTERPRETETURMA LA (NOLOCK) 
												INNER JOIN NECESSIDADEESPECIAL.RECURSONECESSIDADEESPECIAL R (NOLOCK) ON LA.RECURSONECESSIDADEESPECIALID = R.RECURSONECESSIDADEESPECIALID
												INNER JOIN LY_PESSOA P (NOLOCK ) ON R.PESSOAID = P.PESSOA
                                                INNER JOIN LY_TURMA T (NOLOCK) 
                                                        ON T.TURMA = LA.TURMA 
                                                            AND T.ANO = LA.ANO 
                                                            AND T.SEMESTRE = LA.SEMESTRE 
                                        WHERE P.CPF = @CPF 
                                        ORDER  BY DATAINICIO DESC  ";

                contextQuery.Parameters.Add("@CPF", TechneDbType.T_CPF, cpf);

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

        public List<string> ListaTurnoInterpreteAtivoPor(DataContext ctx, int recursoId, DateTime dataInicio, DateTime dataFim)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            List<string> retorno = new List<string>();
            try
            {
                contextQuery.Command = @" SELECT DISTINCT TURNO 
                                        FROM   NECESSIDADEESPECIAL.INTERPRETETURMA LA (NOLOCK) 
                                                INNER JOIN LY_TURMA T (NOLOCK) 
                                                        ON T.TURMA = LA.TURMA 
                                                            AND T.ANO = LA.ANO 
                                                            AND T.SEMESTRE = LA.SEMESTRE 
                                        WHERE LA.RECURSONECESSIDADEESPECIALID = @RECURSONECESSIDADEESPECIALID 
                                                AND ( @DATAINICIO BETWEEN DATAINICIO AND DATAFIM 
			                                        OR @DATAFIM BETWEEN DATAINICIO AND DATAFIM ) ";

                contextQuery.Parameters.Add("@RECURSONECESSIDADEESPECIALID", SqlDbType.Int, recursoId);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, dataInicio.Date);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, dataFim.Date);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno.Add(Convert.ToString(reader["TURNO"]));
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

        public List<string> ListaTurnoInterpreteAtivoPor(DataContext ctx, int recursoId, DateTime dataInicio, DateTime dataFim, string turmaExcecao)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            List<string> retorno = new List<string>();
            try
            {
                contextQuery.Command = @" SELECT DISTINCT TURNO 
                                            FROM   NECESSIDADEESPECIAL.INTERPRETETURMA LA (NOLOCK) 
                                                    INNER JOIN LY_TURMA T (NOLOCK) 
                                                            ON T.TURMA = LA.TURMA 
                                                                AND T.ANO = LA.ANO 
                                                                AND T.SEMESTRE = LA.SEMESTRE 
                                            WHERE LA.RECURSONECESSIDADEESPECIALID = @RECURSONECESSIDADEESPECIALID 
		                                            AND T.TURMA <> @TURMA
                                                    AND ( @DATAINICIO BETWEEN DATAINICIO AND DATAFIM 
			                                            OR @DATAFIM BETWEEN DATAINICIO AND DATAFIM ) ";

                contextQuery.Parameters.Add("@RECURSONECESSIDADEESPECIALID", SqlDbType.Int, recursoId);
                contextQuery.Parameters.Add("@TURMA", turmaExcecao);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, dataInicio.Date);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, dataFim.Date);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno.Add(Convert.ToString(reader["TURNO"]));
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

        public ValidacaoDados Valida(Entidades.InterpreteTurma interprete, bool cadastro, string unidadeEnsino, string curso, string codigoTurno, decimal serie, string cpf)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.NecessidadeEspecial.NecessidadeEspecial rnNecessidadeEspecial = new NecessidadeEspecial();
            AvaliacaoNapes rnAvaliacaoNapes = new AvaliacaoNapes();
            CuidadorAluno rnCuidadorAluno = new CuidadorAluno();
            RN.Turma rnTurma = new Turma();
            LedorAluno rnLedorAluno = new LedorAluno();
            RecursoNecessidadeEspecial rnRecursoNecessidadeEspecial = new RecursoNecessidadeEspecial();
            DadosTurma dadosTurma = new DadosTurma();
            List<string[]> turnoAssociacaoAtiva = new List<string[]>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (interprete == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (interprete.InterpreteTurmaId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (interprete.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (cpf.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CPF é obrigatório.");
            }

            if (interprete.RecursoNecessidadeEspecialId <= 0)
            {
                mensagens.Add("Campo CODIGO DO RECURSO é obrigatório.");
            }
            if (cadastro)
            {
                if (unidadeEnsino.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo UNIDADE DE ENSINO é obrigatório.");
                }
                if (interprete.Ano <= 0)
                {
                    mensagens.Add("Campo ANO LETIVO é obrigatório.");
                }
                if (interprete.Semestre < 0)
                {
                    mensagens.Add("Campo PERÍODO LETIVO é obrigatório.");
                }
                if (curso.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo CURSO é obrigatório.");
                }
                if (codigoTurno.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo TURNO é obrigatório.");
                }
                if (serie <= 0)
                {
                    mensagens.Add("Campo SÉRIE é obrigatório.");
                }
                if (interprete.Turma.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo TURMA é obrigatório.");
                }
            }
            if (interprete.DataInicio == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INICIO é obrigatório.");
            }

            if (interprete.DataFim == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA FIM é obrigatório.");
            }

            if (interprete.DataInicio != DateTime.MinValue && interprete.DataFim != DateTime.MinValue && interprete.DataInicio > interprete.DataFim)
            {
                mensagens.Add("A DATA INICIO deve ser menor ou igual a DATA FIM.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se recurso está ativo
                    if (!rnRecursoNecessidadeEspecial.EhRecursoAtivoPor(contexto, interprete.RecursoNecessidadeEspecialId))
                    {
                        mensagens.Add("Este recurso se encontra DESATIVADO.");
                    }

                    //Verifica se o recurso pode atuar como interprete
                    if (!rnRecursoNecessidadeEspecial.PossuiTipoCadastradoPor(contexto, interprete.RecursoNecessidadeEspecialId, (int)TipoRecursoNecessidadeEspecial.TipoRecurso.Interprete))
                    {
                        mensagens.Add("Este recurso não optou pela opção de ser associado como INTÉRPRETE DE LIBRAS.");
                    }
                    else
                    {
                        //Busca dados da turma (data fim da turma e turno)
                        dadosTurma = rnTurma.ObtemDadosTurmaAbertaPor(contexto, interprete.Turma, interprete.Ano, interprete.Semestre);
                        if (dadosTurma.Turma.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add("A turma escolhida não está aberta.");
                        }
                        else
                        {
                            //Verifica se turma escolhida tem aluno com avaliação napes positiva
                            if (!rnAvaliacaoNapes.PossuiAvaliacaoPor(contexto, interprete.Turma, interprete.Ano, interprete.Semestre, (int)TipoRecursoNecessidadeEspecial.TipoRecurso.Interprete))
                            {
                                mensagens.Add("A turma escolhida não possui alunos com AVALIAÇÃO DO NAPES positiva.");
                            }

                            //Verifica se turma escolhida tem aluno necessidade especial que necessite de interprete
                            if (!rnNecessidadeEspecial.NecessitaInterpretePor(contexto, interprete.Turma, interprete.Ano, interprete.Semestre))
                            {
                                mensagens.Add("A turma escolhida não possui alunos com necessidade especial que pode ser associada a um Ledor");
                            }

                            //Verifica se data fim é menor ou igual a data fim da turma do aluno
                            if (dadosTurma.DataFimTurma < interprete.DataFim.Date)
                            {
                                mensagens.Add(string.Format("A DATA FIM da associação do INTÉRPRETE DE LIBRAS deve ser menor ou igual ao fim da turma, Data Fim: {0}.", dadosTurma.DataFimTurma.ToString("dd/MM/yyyy")));
                            }

                            //Verifica se a turma já possui outro interprete ativo
                            if (this.PossuiOutroInterpreteAtivoPor(contexto, interprete.Turma, interprete.DataInicio, interprete.DataFim, interprete.InterpreteTurmaId, interprete.Ano, interprete.Semestre))
                            {
                                mensagens.Add("Esta turma já possui um INTÉRPRETE DE LIBRAS ativo.");
                            }

                            //Verifica se não é a pessoa que representa as empresas
                            if (cpf != "00000000000")
                            {
                                //Busca turnos que o recurso está ativo como cuidador 
                                foreach (string turno in rnCuidadorAluno.ListaTurnoCuidadorAtivoPor(contexto, interprete.RecursoNecessidadeEspecialId, interprete.DataInicio, interprete.DataFim))
                                {
                                    string[] turnoTipo = new string[2];
                                    turnoTipo[0] = turno;
                                    turnoTipo[1] = "Cuidador";
                                    turnoAssociacaoAtiva.Add(turnoTipo);
                                }

                                //Busca turnos que o recurso está ativo como ledor 
                                foreach (string turno in rnLedorAluno.ListaTurnoLedorAtivoPor(contexto, interprete.RecursoNecessidadeEspecialId, interprete.DataInicio, interprete.DataFim))
                                {
                                    string[] turnoTipo = new string[2];
                                    turnoTipo[0] = turno;
                                    turnoTipo[1] = "Ledor";
                                    turnoAssociacaoAtiva.Add(turnoTipo);
                                }

                                //Busca turnos que o recurso está ativo como interprete
                                foreach (string turno in this.ListaTurnoInterpreteAtivoPor(contexto, interprete.RecursoNecessidadeEspecialId, interprete.DataInicio, interprete.DataFim, interprete.Turma))
                                {
                                    string[] turnoTipo = new string[2];
                                    turnoTipo[0] = turno;
                                    turnoTipo[1] = "Intérprete de Libras";
                                    turnoAssociacaoAtiva.Add(turnoTipo);
                                }

                                foreach (string[] turnoTipo in turnoAssociacaoAtiva)
                                {
                                    //Verifica se o turno da turma está em contra-turno
                                    if (dadosTurma.Turno == turnoTipo[0])
                                    {
                                        mensagens.Add(string.Format("Este recurso já está alocado como {0} em outra turma neste PERIODO / TURNO.", turnoTipo[1]));
                                    }
                                    else if (!RN.Turno.VerificarContraTurno(turnoTipo[0], dadosTurma.Turno))
                                    {
                                        mensagens.Add(string.Format("A turma escolhida deve estar em contraturno com as outras alocações de {0} neste periodo.", turnoTipo[1]));
                                    }
                                }
                            }
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
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        private bool PossuiOutroInterpreteAtivoPor(DataContext ctx, string turma, DateTime dataInicio, DateTime dataFim, int interpreteId, int ano, int semestre)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                            FROM   NECESSIDADEESPECIAL.INTERPRETETURMA (nolock)
                                            WHERE  TURMA = @TURMA 
		                                            AND ANO = @ANO
		                                            AND SEMESTRE = @SEMESTRE
                                                    AND INTERPRETETURMAID <> @INTERPRETETURMAID
                                                    AND ( @DATAINICIO BETWEEN DATAINICIO AND DATAFIM 
                                                            OR @DATAFIM BETWEEN DATAINICIO AND DATAFIM ) ";

                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, semestre);
                contextQuery.Parameters.Add("@INTERPRETETURMAID", SqlDbType.Int, interpreteId);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, dataInicio.Date);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, dataFim.Date);

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
        }

        public void Insere(Entidades.InterpreteTurma interprete)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO NECESSIDADEESPECIAL.INTERPRETETURMA 
                                                (RECURSONECESSIDADEESPECIALID, 
                                                 ANO, 
                                                 SEMESTRE, 
                                                 TURMA, 
                                                 DATAINICIO, 
                                                 DATAFIM, 
                                                 USUARIOID, 
                                                 DATACADASTRO, 
                                                 DATAALTERACAO) 
                                    VALUES      (@RECURSONECESSIDADEESPECIALID, 
                                                 @ANO, 
                                                 @SEMESTRE, 
                                                 @TURMA, 
                                                 @DATAINICIO, 
                                                 @DATAFIM, 
                                                 @USUARIOID, 
                                                 @DATACADASTRO, 
                                                 @DATAALTERACAO)  ";

                contextQuery.Parameters.Add("@RECURSONECESSIDADEESPECIALID", SqlDbType.Int, interprete.RecursoNecessidadeEspecialId);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, interprete.Ano);
                contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, interprete.Semestre);
                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, interprete.Turma);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, interprete.DataInicio.Date);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, interprete.DataFim.Date);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, interprete.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                contexto.ApplyModifications(contextQuery);
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

        public void Atualiza(Entidades.InterpreteTurma interprete)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE NECESSIDADEESPECIAL.INTERPRETETURMA 
                                                SET    DATAINICIO = @DATAINICIO, 
                                                        DATAFIM = @DATAFIM, 
                                                        USUARIOID = @USUARIOID, 
                                                        DATAALTERACAO = @DATAALTERACAO 
                                                WHERE  INTERPRETETURMAID = @INTERPRETETURMAID  ";

                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, interprete.DataInicio.Date);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, interprete.DataFim.Date);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, interprete.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@INTERPRETETURMAID", SqlDbType.Int, interprete.InterpreteTurmaId);

                contexto.ApplyModifications(contextQuery);
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
    }
}