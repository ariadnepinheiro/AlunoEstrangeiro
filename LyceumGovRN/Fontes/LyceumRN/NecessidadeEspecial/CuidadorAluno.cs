using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;
using Techne.Lyceum.RN.DTOs;

namespace Techne.Lyceum.RN.NecessidadeEspecial
{
    public class CuidadorAluno
    {
        public DateTime[] RetornaDataMinimaMaximaPor(DataContext contexto, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            DateTime[] datas = null;
            try
            {
                contextQuery.Command = @"SELECT MIN(DATAINICIO) AS DATAMINIMA, MAX(DATAFIM) AS DATAMAXIMA
                                                        FROM   NECESSIDADEESPECIAL.CUIDADORALUNO (NOLOCK) 
                                                        WHERE  ALUNOID = @ALUNOID  ";

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

        public bool EhCuidadorAtivoPor(DataContext contexto, int recursoNecessidadeEspecialId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM   NECESSIDADEESPECIAL.CUIDADORALUNO (NOLOCK) 
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

        public ValidacaoDados Valida(Entidades.CuidadorAluno cuidador, bool cadastro, string cpf)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            LedorAluno rnLedorAluno = new LedorAluno();
            InterpreteTurma rnInterpreteTurma = new InterpreteTurma();
            RecursoNecessidadeEspecial rnRecursoNecessidadeEspecial = new RecursoNecessidadeEspecial();
            Entidades.AvaliacaoNapes avaliacaoNapes = new Entidades.AvaliacaoNapes();
            AvaliacaoNapes rnAvaliacaoNapes = new AvaliacaoNapes();
            DateTime dataInicioNapes = new DateTime();
            DateTime dataFimNapes = new DateTime();
            RN.Matricula rnMatricula = new Matricula();
            NecessidadeEspecial rnNecessidadeEspecial = new NecessidadeEspecial();
            DadosEnturmacaoAluno dadosMatricula = new DadosEnturmacaoAluno();
            List<string[]> turnoAssociacaoAtiva = new List<string[]>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (cuidador == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (cuidador.CuidadorAlunoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (cuidador.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (cpf.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CPF é obrigatório.");
            }

            if (cuidador.RecursoNecessidadeEspecialId <= 0)
            {
                mensagens.Add("Campo CODIGO DO RECURSO é obrigatório.");
            }

            if (cuidador.AlunoId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ALUNO é obrigatório.");
            }

            if (cuidador.DataInicio == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INICIO é obrigatório.");
            }

            if (cuidador.DataFim == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA FIM é obrigatório.");
            }

            if (cuidador.DataInicio != DateTime.MinValue && cuidador.DataFim != DateTime.MinValue && cuidador.DataInicio > cuidador.DataFim)
            {
                mensagens.Add("A DATA INICIO deve ser menor ou igual a DATA FIM.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se recurso está ativo
                    if (!rnRecursoNecessidadeEspecial.EhRecursoAtivoPor(contexto, cuidador.RecursoNecessidadeEspecialId))
                    {
                        mensagens.Add("Este recurso se encontra DESATIVADO.");
                    }

                    //Verifica se o recurso pode atuar como cuidador
                    if (!rnRecursoNecessidadeEspecial.PossuiTipoCadastradoPor(contexto, cuidador.RecursoNecessidadeEspecialId, (int)TipoRecursoNecessidadeEspecial.TipoRecurso.Cuidador))
                    {
                        mensagens.Add("Este recurso não optou pela opção de ser associado como CUIDADOR.");
                    }
                    else
                    {
                        //Busca dados da matricula ativa do aluno 
                        dadosMatricula = rnMatricula.ObtemMatriculaPrincipalAtivaPor(contexto, cuidador.AlunoId);

                        //Verifica se Aluno possui matricula principal ativa
                        if (dadosMatricula.Turma.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add("Este aluno não pode ser associado ao cuidador, pois não possui MATRÍCULA ATIVA.");
                        }

                        //Verifica se aluno possui necessidade especial que necessite de cuidador
                        if (!rnNecessidadeEspecial.NecessitaCuidadorPor(contexto, cuidador.AlunoId))
                        {
                            mensagens.Add("A necessidade especial do aluno não pode ser associada a um cuidador");
                        }

                        //Busca dados da avaliação do NAPES para cuidador do aluno
                        avaliacaoNapes = rnAvaliacaoNapes.ObtemPor(contexto, cuidador.AlunoId, (int)TipoRecursoNecessidadeEspecial.TipoRecurso.Cuidador);

                        //Verifica se necessiade de recurso já foi avaliado pelo NAPES
                        if (avaliacaoNapes.AvaliacaoNapesId <= 0)
                        {
                            mensagens.Add("Este aluno não pode ser associado ao cuidador, pois não possui AVALIAÇÃO DO NAPES.");
                        }

                        if (mensagens.Count == 0)
                        {
                            //Verfica se aluno precisa do recurso
                            if (!Convert.ToBoolean(avaliacaoNapes.NecessitaRecurso))
                            {
                                mensagens.Add("Este aluno não pode ser associado ao cuidador, pois a AVALIAÇÃO DO NAPES foi negativa.");
                            }
                            else
                            {
                                //Verifica se a necessidade é transitoria
                                if (Convert.ToBoolean(avaliacaoNapes.Transitorio))
                                {
                                    //Se for transitoria, verifica datas
                                    dataFimNapes = Convert.ToDateTime(avaliacaoNapes.DataFim);
                                    dataInicioNapes = Convert.ToDateTime(avaliacaoNapes.DataInicio);

                                    if (dataInicioNapes.Date > cuidador.DataInicio.Date)
                                    {
                                        mensagens.Add(string.Format("A DATA INICIO da associação do cuidador deve ser maior ou igual ao início necessidade transitória deste aluno, dia: {0}.", dataFimNapes.Date.ToString("dd/MM/yyyy")));
                                    }

                                    if (dataFimNapes.Date < cuidador.DataFim.Date)
                                    {
                                        mensagens.Add(string.Format("A DATA FIM da associação do cuidador deve ser menor ou igual ao fim necessidade transitória deste aluno, dia: {0}.", dataFimNapes.Date.ToString("dd/MM/yyyy")));
                                    }
                                }
                                else
                                {
                                    //Em caso de ncessidade permanente, se data fim é menor ou igual a data fim da turma do aluno
                                    if (dadosMatricula.DataFimTurma < cuidador.DataFim.Date)
                                    {
                                        mensagens.Add(string.Format("A DATA FIM da associação do cuidador deve ser menor ou igual ao fim da turma, dia: {0}.", dadosMatricula.DataFimTurma.ToString("dd/MM/yyyy")));
                                    }
                                }
                            }

                            //Verifica se aluno já possui outro cuidador ativo
                            if (this.PossuiOutroCuidadorAtivoPor(contexto, cuidador.AlunoId, cuidador.DataInicio, cuidador.DataFim, cuidador.CuidadorAlunoId))
                            {
                                mensagens.Add("Este aluno já possui um cuidador ativo.");
                            }

                            //Verifica se não é a pessoa que representa as empresas
                            if (cpf != "00000000000")
                            {
                                //Busca turnos que o recurso está ativo como cuidador 
                                foreach (string turno in this.ListaDemaisTurnoCuidadorAtivoPor(contexto, cuidador.RecursoNecessidadeEspecialId, cuidador.DataInicio, cuidador.DataFim, cuidador.CuidadorAlunoId))
                                {
                                    string[] turnoTipo = new string[2];
                                    turnoTipo[0] = turno;
                                    turnoTipo[1] = "Cuidador";
                                    turnoAssociacaoAtiva.Add(turnoTipo);
                                }

                                //Busca turnos que o recurso está ativo como ledor 
                                foreach (string turno in rnLedorAluno.ListaTurnoLedorAtivoPor(contexto, cuidador.RecursoNecessidadeEspecialId, cuidador.DataInicio, cuidador.DataFim))
                                {
                                    string[] turnoTipo = new string[2];
                                    turnoTipo[0] = turno;
                                    turnoTipo[1] = "Ledor";
                                    turnoAssociacaoAtiva.Add(turnoTipo);
                                }
                                //Busca turnos que o recurso está ativo como interprete
                                foreach (string turno in rnInterpreteTurma.ListaTurnoInterpreteAtivoPor(contexto, cuidador.RecursoNecessidadeEspecialId, cuidador.DataInicio, cuidador.DataFim))
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
                                        mensagens.Add(string.Format("Este recurso já está alocado como {0} neste PERIODO / TURNO.", turnoTipo[1]));
                                    }
                                    else if (!RN.Turno.VerificarContraTurno(turnoTipo[0], dadosMatricula.Turno))
                                    {
                                        mensagens.Add(string.Format("O aluno escolhido não está em contraturno com as alocações de {0} neste periodo.", turnoTipo[1]));
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

        private bool PossuiOutroCuidadorAtivoPor(DataContext ctx, string alunoId, DateTime dataInicio, DateTime dataFim, int cuidadorId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   [NECESSIDADEESPECIAL].[CUIDADORALUNO] 
                                        WHERE  ALUNOID = @ALUNO 
                                               AND CUIDADORALUNOID <> @CUIDADORALUNOID
                                               AND ( @DATAINICIO BETWEEN DATAINICIO AND DATAFIM 
                                                      OR @DATAFIM BETWEEN DATAINICIO AND DATAFIM ) ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, alunoId);
                contextQuery.Parameters.Add("@CUIDADORALUNOID", SqlDbType.Int, cuidadorId);
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

        private List<string> ListaDemaisTurnoCuidadorAtivoPor(DataContext ctx, int recursoId, DateTime dataInicio, DateTime dataFim, int cuidadorId)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            List<string> retorno = new List<string>();
            try
            {
                contextQuery.Command = @" SELECT DISTINCT TURNO 
                                        FROM   NECESSIDADEESPECIAL.CUIDADORALUNO CA (NOLOCK) 
                                                INNER JOIN LY_MATRICULA M (NOLOCK) 
                                                        ON CA.ALUNOID = M.ALUNO 
                                                INNER JOIN LY_TURMA T (NOLOCK) 
                                                        ON T.TURMA = M.TURMA 
                                                            AND T.ANO = M.ANO 
                                                            AND T.SEMESTRE = M.SEMESTRE 
                                                            AND T.DISCIPLINA = M.DISCIPLINA 
                                        WHERE  M.SIT_MATRICULA = 'Matriculado'
                                            AND CUIDADORALUNOID <> @CUIDADORALUNOID
		                                    AND T.OPTATIVAREFORCO = 'N'
                                            AND ISNULL(T.ELETIVA,'N') = 'N'
                                            AND ISNULL(M.DEPENDENCIA, 'N') = 'N'
                                            AND ISNULL(M.EDUC_ESPECIAL, 'N') = 'N'
                                            AND ISNULL(M.MAIS_EDUCACAO, 'N') = 'N'
                                            AND ISNULL(M.CONCOMITANTE, 'N') = 'N'
                                            AND CA.RECURSONECESSIDADEESPECIALID = @RECURSONECESSIDADEESPECIALID 
                                            AND ( @DATAINICIO BETWEEN DATAINICIO AND DATAFIM 
                                                        OR @DATAFIM BETWEEN DATAINICIO AND DATAFIM ) ";

                contextQuery.Parameters.Add("@RECURSONECESSIDADEESPECIALID", SqlDbType.Int, recursoId);
                contextQuery.Parameters.Add("@CUIDADORALUNOID", SqlDbType.Int, cuidadorId);
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

        public List<string> ListaTurnoCuidadorAtivoPor(DataContext ctx, int recursoId, DateTime dataInicio, DateTime dataFim)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            List<string> retorno = new List<string>();
            try
            {
                contextQuery.Command = @" SELECT DISTINCT TURNO 
                                        FROM   NECESSIDADEESPECIAL.CUIDADORALUNO CA (NOLOCK) 
                                                INNER JOIN LY_MATRICULA M (NOLOCK) 
                                                        ON CA.ALUNOID = M.ALUNO 
                                                INNER JOIN LY_TURMA T (NOLOCK) 
                                                        ON T.TURMA = M.TURMA 
                                                            AND T.ANO = M.ANO 
                                                            AND T.SEMESTRE = M.SEMESTRE 
                                                            AND T.DISCIPLINA = M.DISCIPLINA 
                                        WHERE  M.SIT_MATRICULA = 'Matriculado'
		                                    AND T.OPTATIVAREFORCO = 'N'
                                            AND ISNULL(T.ELETIVA,'N') = 'N'
                                            AND ISNULL(M.DEPENDENCIA, 'N') = 'N'
                                            AND ISNULL(M.EDUC_ESPECIAL, 'N') = 'N'
                                            AND ISNULL(M.MAIS_EDUCACAO, 'N') = 'N'
                                            AND ISNULL(M.CONCOMITANTE, 'N') = 'N'
                                            AND CA.RECURSONECESSIDADEESPECIALID = @RECURSONECESSIDADEESPECIALID 
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

        public DataTable ListaPor(string aluno)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT CUIDADORALUNOID, 
                                       ALUNOID, 
                                       CA.RECURSONECESSIDADEESPECIALID, 
                                       P.CPF,
                                       P.NOME_COMPL, 
                                       CA.DATAINICIO, 
                                       CA.DATAFIM 
                                FROM   NECESSIDADEESPECIAL.CUIDADORALUNO CA (NOLOCK ) 
                                       INNER JOIN NECESSIDADEESPECIAL.RECURSONECESSIDADEESPECIAL R (NOLOCK ) 
                                               ON CA.RECURSONECESSIDADEESPECIALID = 
                                                  R.RECURSONECESSIDADEESPECIALID 
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
                contextQuery.Command = @" SELECT DISTINCT CUIDADORALUNOID, 
                                                CA.ALUNOID, 
                                                PA.NOME_COMPL, 
                                                CA.RECURSONECESSIDADEESPECIALID, 
                                                T.TURNO, 
                                                CA.DATAINICIO, 
                                                CA.DATAFIM 
                                        FROM   NECESSIDADEESPECIAL.CUIDADORALUNO CA (NOLOCK ) 
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
                                        ORDER  BY DATAINICIO DESC ";

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

        public void Insere(Entidades.CuidadorAluno cuidador)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO NECESSIDADEESPECIAL.CUIDADORALUNO
                                                (ALUNOID, 
                                                 RECURSONECESSIDADEESPECIALID, 
                                                 DATAINICIO, 
                                                 DATAFIM, 
                                                 USUARIOID, 
                                                 DATACADASTRO, 
                                                 DATAALTERACAO) 
                                    VALUES      (@ALUNOID, 
                                                 @RECURSONECESSIDADEESPECIALID, 
                                                 @DATAINICIO, 
                                                 @DATAFIM, 
                                                 @USUARIOID, 
                                                 @DATACADASTRO, 
                                                 @DATAALTERACAO)  ";

                contextQuery.Parameters.Add("@ALUNOID", SqlDbType.VarChar, cuidador.AlunoId);
                contextQuery.Parameters.Add("@RECURSONECESSIDADEESPECIALID", SqlDbType.Int, cuidador.RecursoNecessidadeEspecialId);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, cuidador.DataInicio.Date);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, cuidador.DataFim.Date);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, cuidador.UsuarioId);
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

        public void Atualiza(Entidades.CuidadorAluno cuidador)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE NECESSIDADEESPECIAL.CUIDADORALUNO 
                                            SET    DATAINICIO = @DATAINICIO, 
                                                   DATAFIM = @DATAFIM, 
                                                   USUARIOID = @USUARIOID, 
                                                   DATAALTERACAO = @DATAALTERACAO 
                                            WHERE  CUIDADORALUNOID = @CUIDADORALUNOID ";

                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, cuidador.DataInicio.Date);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, cuidador.DataFim.Date);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, cuidador.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@CUIDADORALUNOID", SqlDbType.Int, cuidador.CuidadorAlunoId);

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