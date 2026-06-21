using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.DTOs;

namespace Techne.Lyceum.RN.NecessidadeEspecial
{
    public class LedorAluno
    {
        public DateTime[] RetornaDataMinimaMaximaPor(DataContext contexto, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            DateTime[] datas = null;
            try
            {
                contextQuery.Command = @" SELECT MIN(DATAINICIO) AS DATAMINIMA, MAX(DATAFIM) AS DATAMAXIMA
                                            FROM   NECESSIDADEESPECIAL.LEDORALUNO (NOLOCK) 
                                            WHERE  ALUNOID = @ALUNOID ";

                contextQuery.Parameters.Add("@ALUNOID", SqlDbType.VarChar, aluno);

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

        public bool EhLedorAtivoPor(DataContext contexto, int recursoNecessidadeEspecialId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM   NECESSIDADEESPECIAL.LEDORALUNO (NOLOCK) 
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
                contextQuery.Command = @" SELECT DISTINCT LEDORALUNOID, 
                                               ALUNOID, 
	                                           L.TURMA,
                                               l.RECURSONECESSIDADEESPECIALID, 
                                               P.NOME_COMPL, 
                                               P.CPF,
                                               l.DATAINICIO, 
                                               l.DATAFIM 
                                        FROM   NECESSIDADEESPECIAL.LEDORALUNO L (NOLOCK ) 
                                               INNER JOIN NECESSIDADEESPECIAL.RECURSONECESSIDADEESPECIAL R (NOLOCK ) 
                                                       ON L.RECURSONECESSIDADEESPECIALID = R.RECURSONECESSIDADEESPECIALID 
                                               INNER JOIN LY_PESSOA P (NOLOCK ) 
                                                       ON R.PESSOAID = P.PESSOA 
                                        WHERE  ALUNOID = @ALUNO  
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
                contextQuery.Command = @" SELECT DISTINCT LEDORALUNOID, 
                                    CA.ALUNOID, 
                                    PA.NOME_COMPL, 
                                    CA.RECURSONECESSIDADEESPECIALID, 
                                    T.TURNO, 
	                                T.TURMA,
                                    CA.DATAINICIO, 
                                    CA.DATAFIM ,
                                    CA.ANO,
                                    CA.SEMESTRE
                            FROM   NECESSIDADEESPECIAL.LEDORALUNO CA (NOLOCK ) 
		                            INNER JOIN NECESSIDADEESPECIAL.RECURSONECESSIDADEESPECIAL R (NOLOCK) ON CA.RECURSONECESSIDADEESPECIALID = R.RECURSONECESSIDADEESPECIALID
		                            INNER JOIN LY_PESSOA P (NOLOCK ) ON R.PESSOAID = P.PESSOA
                                    INNER JOIN LY_ALUNO A (NOLOCK ) ON A.ALUNO = CA.ALUNOID 
		                            INNER JOIN LY_PESSOA PA (NOLOCK ) ON A.PESSOA = PA.PESSOA
                                    INNER JOIN LY_MATRICULA M (NOLOCK) ON CA.ALUNOID = M.ALUNO 
                                    INNER JOIN LY_TURMA T (NOLOCK) ON T.TURMA = M.TURMA 
                                                AND T.ANO = M.ANO 
                                                AND T.SEMESTRE = M.SEMESTRE 
                                                AND T.DISCIPLINA = M.DISCIPLINA 
                            WHERE  M.SIT_MATRICULA = 'MATRICULADO' 
                                    AND T.OPTATIVAREFORCO = 'N' 
                                    AND ISNULL(T.ELETIVA,'N') = 'N'
                                    AND ISNULL(M.DEPENDENCIA, 'N') = 'N' 
                                    AND ISNULL(M.EDUC_ESPECIAL, 'N') = 'N' 
                                    AND ISNULL(M.MAIS_EDUCACAO, 'N') = 'N' 
                                    AND ISNULL(M.CONCOMITANTE, 'N') = 'N' 
                                     AND P.CPF = @CPF  
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

        public ValidacaoDados Valida(Entidades.LedorAluno ledor, bool cadastro, string unidadeEnsino, string curso, string codigoTurno, decimal serie, string cpf)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            CuidadorAluno rnCuidadorAluno = new CuidadorAluno();
            InterpreteTurma rnInterpreteTurma = new InterpreteTurma();
            RecursoNecessidadeEspecial rnRecursoNecessidadeEspecial = new RecursoNecessidadeEspecial();
            Entidades.AvaliacaoNapes avaliacaoNapes = new Entidades.AvaliacaoNapes();
            AvaliacaoNapes rnAvaliacaoNapes = new AvaliacaoNapes();
            DateTime dataInicioNapes = new DateTime();
            DateTime dataFimNapes = new DateTime();
            RN.Matricula rnMatricula = new Matricula();
            RN.NecessidadeEspecial.NecessidadeEspecial rnNecessidadeEspecial = new NecessidadeEspecial();
            DadosEnturmacaoAluno dadosMatricula = new DadosEnturmacaoAluno();
            List<string[]> turnoAssociacaoAtiva = new List<string[]>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (ledor == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (ledor.LedorAlunoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (ledor.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (cpf.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CPF é obrigatório.");
            }

            if (ledor.RecursoNecessidadeEspecialId <= 0)
            {
                mensagens.Add("Campo CODIGO DO RECURSO é obrigatório.");
            }
            if (cadastro)
            {
                if (unidadeEnsino.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo UNIDADE DE ENSINO é obrigatório.");
                }

                if (ledor.Ano <= 0)
                {
                    mensagens.Add("Campo ANO LETIVO é obrigatório.");
                }
                if (ledor.Semestre < 0)
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
                if (ledor.AlunoId.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo ALUNO é obrigatório.");
                }
            }
            if (ledor.DataInicio == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INICIO é obrigatório.");
            }

            if (ledor.DataFim == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA FIM é obrigatório.");
            }

            if (ledor.DataInicio != DateTime.MinValue && ledor.DataFim != DateTime.MinValue && ledor.DataInicio > ledor.DataFim)
            {
                mensagens.Add("A DATA INICIO deve ser menor ou igual a DATA FIM.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se recurso está ativo
                    if (!rnRecursoNecessidadeEspecial.EhRecursoAtivoPor(contexto, ledor.RecursoNecessidadeEspecialId))
                    {
                        mensagens.Add("Este recurso se encontra DESATIVADO.");
                    }

                    //Verifica se o recurso pode atuar como ledor
                    if (!rnRecursoNecessidadeEspecial.PossuiTipoCadastradoPor(contexto, ledor.RecursoNecessidadeEspecialId, (int)TipoRecursoNecessidadeEspecial.TipoRecurso.Ledor))
                    {
                        mensagens.Add("Este recurso não optou pela opção de ser associado como LEDOR.");
                    }
                    else
                    {
                        //Verifica se aluno possui necessidade especial que necessite de ledor
                        if (!rnNecessidadeEspecial.NecessitaLedorPor(contexto, ledor.AlunoId))
                        {
                            mensagens.Add("A necessidade especial do aluno não pode ser associada a um Ledor");
                        }

                        //Busca dados da matricula ativa do aluno 
                        dadosMatricula = rnMatricula.ObtemMatriculaPrincipalAtivaPor(contexto, ledor.AlunoId);

                        //Verifica se Aluno possui matricula principal ativa
                        if (dadosMatricula.Turma.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add("Este aluno não pode ser associado ao ledor, pois não possui MATRÍCULA ATIVA.");
                        }
                        else
                        {
                            ledor.Turma = dadosMatricula.Turma;
                            ledor.Ano = dadosMatricula.Ano;
                            ledor.Semestre = dadosMatricula.Periodo;
                        }

                        //Busca dados da avaliação do NAPES para ledor do aluno
                        avaliacaoNapes = rnAvaliacaoNapes.ObtemPor(contexto, ledor.AlunoId, (int)TipoRecursoNecessidadeEspecial.TipoRecurso.Ledor);

                        //Verifica se necessiade de recurso já foi avaliado pelo NAPES
                        if (avaliacaoNapes.AvaliacaoNapesId <= 0)
                        {
                            mensagens.Add("Este aluno não pode ser associado ao ledor, pois não possui AVALIAÇÃO DO NAPES.");
                        }

                        if (mensagens.Count == 0)
                        {
                            //Verfica se aluno precisa do recurso
                            if (!Convert.ToBoolean(avaliacaoNapes.NecessitaRecurso))
                            {
                                mensagens.Add("Este aluno não pode ser associado ao ledor, pois a AVALIAÇÃO DO NAPES foi negativa.");
                            }
                            else
                            {
                                //Verifica se a necessidade é transitoria
                                if (Convert.ToBoolean(avaliacaoNapes.Transitorio))
                                {
                                    //Se for transitoria, verifica datas
                                    dataFimNapes = Convert.ToDateTime(avaliacaoNapes.DataFim);
                                    dataInicioNapes = Convert.ToDateTime(avaliacaoNapes.DataInicio);

                                    if (dataInicioNapes.Date > ledor.DataInicio.Date)
                                    {
                                        mensagens.Add(string.Format("A DATA INICIO da associação do ledor deve ser maior ou igual ao início necessidade transitória deste aluno, dia: {0}.", Convert.ToString(dataFimNapes.Date)));
                                    }

                                    if (dataFimNapes.Date < ledor.DataFim.Date)
                                    {
                                        mensagens.Add(string.Format("A DATA FIM da associação do ledor deve ser menor ou igual ao fim necessidade transitória deste aluno, dia: {0}.", dataFimNapes.Date.ToString("dd/MM/yyyy")));
                                    }
                                }
                                else
                                {
                                    //Em caso de ncessidade permanente, se data fim é menor ou igual a data fim da turma do aluno
                                    if (dadosMatricula.DataFimTurma < ledor.DataFim.Date)
                                    {
                                        mensagens.Add(string.Format("A DATA FIM da associação do ledor deve ser menor ou igual ao fim da turma, dia: {0}.", dadosMatricula.DataFimTurma.Date.ToString("dd/MM/yyyy")));
                                    }
                                }
                            }

                            //Verifica se aluno já possui outro ledor ativo
                            if (this.PossuiOutroLedorAtivoPor(contexto, ledor.AlunoId, ledor.DataInicio, ledor.DataFim, ledor.LedorAlunoId))
                            {
                                mensagens.Add("Este aluno já possui um ledor ativo.");
                            }

                            //Verifica se não é a pessoa que representa as empresas
                            if (cpf != "00000000000")
                            {
                                //Verifica se ledor possui associacao ativa com a msm turma, e se já possui 2 alunos.
                                if (this.RetornaDemaisQuantidadeAlunoAtivoPor(contexto, dadosMatricula.Turma, ledor.DataInicio, ledor.DataFim, ledor.LedorAlunoId, ledor.RecursoNecessidadeEspecialId) >= 2)
                                {
                                    mensagens.Add("Este ledor já está associado a dois alunos nesta turma.");
                                }

                                //Busca turnos que o recurso está ativo como cuidador 
                                foreach (string turno in rnCuidadorAluno.ListaTurnoCuidadorAtivoPor(contexto, ledor.RecursoNecessidadeEspecialId, ledor.DataInicio, ledor.DataFim))
                                {
                                    string[] turnoTipo = new string[2];
                                    turnoTipo[0] = turno;
                                    turnoTipo[1] = "Cuidador";
                                    turnoAssociacaoAtiva.Add(turnoTipo);
                                }

                                //Busca turnos que o recurso está ativo como ledor 
                                foreach (string turno in this.ListaTurnoLedorAtivoPor(contexto, ledor.RecursoNecessidadeEspecialId, ledor.DataInicio, ledor.DataFim, dadosMatricula.Turma, ledor.Ano, ledor.Semestre))
                                {
                                    string[] turnoTipo = new string[2];
                                    turnoTipo[0] = turno;
                                    turnoTipo[1] = "Ledor";
                                    turnoAssociacaoAtiva.Add(turnoTipo);
                                }

                                //Busca turnos que o recurso está ativo como interprete
                                foreach (string turno in rnInterpreteTurma.ListaTurnoInterpreteAtivoPor(contexto, ledor.RecursoNecessidadeEspecialId, ledor.DataInicio, ledor.DataFim))
                                {
                                    string[] turnoTipo = new string[2];
                                    turnoTipo[0] = turno;
                                    turnoTipo[1] = "Intérprete de Libras";
                                    turnoAssociacaoAtiva.Add(turnoTipo);
                                }

                                foreach (string[] turnoTipo in turnoAssociacaoAtiva)
                                {
                                    //Verifica se o turno do aluno está em contra-turno
                                    if (dadosMatricula.Turno == turnoTipo[0])
                                    {
                                        mensagens.Add(string.Format("Este recurso já está alocado como {0} em outra turma neste PERIODO / TURNO.", turnoTipo[1]));
                                    }
                                    else if (!RN.Turno.VerificarContraTurno(turnoTipo[0], dadosMatricula.Turno))
                                    {
                                        mensagens.Add(string.Format("O aluno escolhido não está em contraturno com as outras alocações de {0} neste periodo.", turnoTipo[1]));
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

        private bool PossuiOutroLedorAtivoPor(DataContext ctx, string alunoId, DateTime dataInicio, DateTime dataFim, int ledorId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   NECESSIDADEESPECIAL.LEDORALUNO (NOLOCK)
                                        WHERE  ALUNOID = @ALUNO 
                                                AND LEDORALUNOID <> @LEDORALUNOID
                                                AND ( @DATAINICIO BETWEEN DATAINICIO AND DATAFIM 
                                                        OR @DATAFIM BETWEEN DATAINICIO AND DATAFIM ) ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, alunoId);
                contextQuery.Parameters.Add("@LEDORALUNOID", SqlDbType.Int, ledorId);
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

        private int RetornaDemaisQuantidadeAlunoAtivoPor(DataContext ctx, string turma, DateTime dataInicio, DateTime dataFim, int ledorId, int recursoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) AS QUANTIDADE
                                            FROM   NECESSIDADEESPECIAL.LEDORALUNO (NOLOCK)
                                            WHERE  TURMA = @TURMA 
                                                    AND RECURSONECESSIDADEESPECIALID = @RECURSONECESSIDADEESPECIALID
                                                    AND LEDORALUNOID <> @LEDORALUNOID
                                                    AND ( @DATAINICIO BETWEEN DATAINICIO AND DATAFIM 
                                                            OR @DATAFIM BETWEEN DATAINICIO AND DATAFIM ) ";

                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
                contextQuery.Parameters.Add("@RECURSONECESSIDADEESPECIALID", SqlDbType.Int, recursoId);
                contextQuery.Parameters.Add("@LEDORALUNOID", SqlDbType.Int, ledorId);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, dataInicio.Date);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, dataFim.Date);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["QUANTIDADE"]);
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

        public List<string> ListaTurnoLedorAtivoPor(DataContext ctx, int recursoId, DateTime dataInicio, DateTime dataFim)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            List<string> retorno = new List<string>();
            try
            {
                contextQuery.Command = @" SELECT DISTINCT TURNO 
                                    FROM   NECESSIDADEESPECIAL.LEDORALUNO LA (NOLOCK) 
                                            INNER JOIN LY_MATRICULA M (NOLOCK) 
                                                    ON LA.ALUNOID = M.ALUNO 
				                                    and LA.ANO = M.ANO
				                                    and LA.SEMESTRE = M.SEMESTRE
				                                    and LA.TURMA = M.TURMA
                                            INNER JOIN LY_TURMA T (NOLOCK) 
                                                    ON T.TURMA = M.TURMA 
                                                        AND T.ANO = M.ANO 
                                                        AND T.SEMESTRE = M.SEMESTRE 
                                                        AND T.DISCIPLINA = M.DISCIPLINA 
                                    WHERE  M.SIT_MATRICULA = 'Matriculado' 
                                            AND LA.RECURSONECESSIDADEESPECIALID = @RECURSONECESSIDADEESPECIALID 
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

        private List<string> ListaTurnoLedorAtivoPor(DataContext ctx, int recursoId, DateTime dataInicio, DateTime dataFim, string turmaExcecao, int ano, int periodo)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            List<string> retorno = new List<string>();
            string possiveisPeriodos = Utils.RecuperaPossiveisPeriodos(periodo);

            try
            {
                contextQuery.Command = string.Format(@" SELECT DISTINCT TURNO 
                                    FROM   NECESSIDADEESPECIAL.LEDORALUNO LA (NOLOCK) 
                                            INNER JOIN LY_MATRICULA M (NOLOCK) 
                                                    ON LA.ALUNOID = M.ALUNO 
				                                    and LA.ANO = M.ANO
				                                    and LA.SEMESTRE = M.SEMESTRE
				                                    and LA.TURMA = M.TURMA
                                            INNER JOIN LY_TURMA T (NOLOCK) 
                                                    ON T.TURMA = M.TURMA 
                                                        AND T.ANO = M.ANO 
                                                        AND T.SEMESTRE = M.SEMESTRE 
                                                        AND T.DISCIPLINA = M.DISCIPLINA 
                                    WHERE  M.SIT_MATRICULA = 'Matriculado' 
                                            AND T.TURMA <> @TURMA
											AND T.ANO = @ANO
											AND T.SEMESTRE IN ( {0} )
                                            AND LA.RECURSONECESSIDADEESPECIALID = @RECURSONECESSIDADEESPECIALID 
                                            AND ( @DATAINICIO BETWEEN DATAINICIO AND DATAFIM 
			                                    OR @DATAFIM BETWEEN DATAINICIO AND DATAFIM ) ", possiveisPeriodos);

                contextQuery.Parameters.Add("@RECURSONECESSIDADEESPECIALID", SqlDbType.Int, recursoId);
                contextQuery.Parameters.Add("@TURMA", turmaExcecao);
                contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
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

        public void Insere(Entidades.LedorAluno ledor)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO NECESSIDADEESPECIAL.LEDORALUNO 
                                                (ALUNOID, 
                                                 RECURSONECESSIDADEESPECIALID, 
                                                 ANO, 
                                                 SEMESTRE, 
                                                 TURMA, 
                                                 DATAINICIO, 
                                                 DATAFIM, 
                                                 USUARIOID, 
                                                 DATACADASTRO, 
                                                 DATAALTERACAO) 
                                    VALUES      (@ALUNOID, 
                                                 @RECURSONECESSIDADEESPECIALID, 
                                                 @ANO, 
                                                 @SEMESTRE, 
                                                 @TURMA, 
                                                 @DATAINICIO, 
                                                 @DATAFIM, 
                                                 @USUARIOID, 
                                                 @DATACADASTRO, 
                                                 @DATAALTERACAO)   ";

                contextQuery.Parameters.Add("@ALUNOID", SqlDbType.VarChar, ledor.AlunoId);
                contextQuery.Parameters.Add("@RECURSONECESSIDADEESPECIALID", SqlDbType.Int, ledor.RecursoNecessidadeEspecialId);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ledor.Ano);
                contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, ledor.Semestre);
                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, ledor.Turma);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, ledor.DataInicio.Date);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, ledor.DataFim.Date);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, ledor.UsuarioId);
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

        public void Atualiza(Entidades.LedorAluno ledor)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE NECESSIDADEESPECIAL.LEDORALUNO 
                                            SET    DATAINICIO = @DATAINICIO, 
                                                   DATAFIM = @DATAFIM, 
                                                   USUARIOID = @USUARIOID, 
                                                   DATAALTERACAO = @DATAALTERACAO 
                                            WHERE  LEDORALUNOID = @LEDORALUNOID ";

                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, ledor.DataInicio.Date);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, ledor.DataFim.Date);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, ledor.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@LEDORALUNOID", SqlDbType.Int, ledor.LedorAlunoId);

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
