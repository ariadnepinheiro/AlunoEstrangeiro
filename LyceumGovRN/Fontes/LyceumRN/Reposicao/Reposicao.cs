using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.Reposicao
{
    public class Reposicao
    {
        public DataTable ListaPor(int periodoLancamentoId, DateTime dataGreve, int ano, int semestre, string turma)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            RN.Reposicao.PeriodoLancamento rnPeriodoLancamento = new PeriodoLancamento();
            RN.Reposicao.Entidades.PeriodoLancamento periodoLancamento = new Entidades.PeriodoLancamento();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT R.REPOSICAOID,
                                        R.NUM_FUNC,
		                                CASE
			                                WHEN D.MATRICULA LIKE '%/%' THEN ''
			                                ELSE D.MATRICULA
		                                END MATRICULA,
		                                F.IDVINCULO,
		                                F.NOME_COMPL,
		                                r.TURMA,
		                                r.DISCIPLINA,
		                                DI.NOME AS NOMEDISCIPLINA,
		                                r.DATAREPOSICAO,
		                                r.CHREPOSICAO,										
										r.TIPO_AULA,
										Convert ( bit,CASE 
											WHEN r.CHREPOSICAO = 0 THEN 1
											ELSE 0
										END) RECUSADO,
										CASE 
											WHEN r.CHREPOSICAO = 0 THEN 'Recusado'
											ELSE CONVERT(VARCHAR(20), R.CHREPOSICAO)
										END CHEXIBICAO
                                FROM Reposicao.REPOSICAO r
	                                INNER JOIN LY_DOCENTE D ON R.NUM_FUNC = D.NUM_FUNC
	                                INNER JOIN VW_FUNCIONARIOS F ON D.MATRICULA = F.MATRICULA
	                                INNER JOIN LY_DISCIPLINA DI ON R.DISCIPLINA = DI.DISCIPLINA
                                WHERE R.DATAGREVE = @DATAGREVE
	                                AND R.TURMA = @TURMA
	                                AND R.ANO = @ANO
	                                AND R.SEMESTRE = @SEMESTRE
                                  
                                ORDER BY DI.NOME, F.NOME_COMPL  ";

                contextQuery.Parameters.Add("@DATAGREVE", SqlDbType.DateTime, dataGreve.Date);
                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, semestre);

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

        public DataTable ListaAlocacaoPor(int periodoLancamentoId, DateTime dataGreve, int ano, int semestre, string turma)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            RN.Reposicao.PeriodoLancamento rnPeriodoLancamento = new PeriodoLancamento();
            RN.Reposicao.Entidades.PeriodoLancamento periodoLancamento = new Entidades.PeriodoLancamento();
            DataTable dt = null;

            try
            {
                //Verifica dia da semana
                int diaSemana = (int)dataGreve.Date.DayOfWeek + 1; //Dia da semana no banco começa em 1 (domingo) o DayOfWeek começa em 0 (domingo);

                contextQuery.Command = @" SELECT DISTINCT D.NUM_FUNC,
		                            CASE
			                            WHEN D.MATRICULA LIKE '%/%' THEN ''
			                            ELSE D.MATRICULA
		                            END MATRICULA,
		                            F.IDVINCULO,
		                            F.NOME_COMPL,
		                            DD.TURMA,
		                            DD.DISCIPLINA,
		                            DI.NOME AS NOMEDISCIPLINA,
		                            CONVERT(VARCHAR,@DATAGREVE,103) AS DATAAULA,
		                            COUNT(distinct DD.AULA) AS TEMPOS,
									COUNT(distinct DD.AULA) - ISNULL((SELECT SUM(R.CHREPOSICAO) 
												FROM REPOSICAO.REPOSICAO R WHERE D.NUM_FUNC = R.NUM_FUNC 
																	AND DD.TURMA = R.TURMA 
																	AND DD.ANO = R.ANO 
																	AND DD.SEMESTRE = R.SEMESTRE
																	AND DD.DISCIPLINA = R.DISCIPLINA
																	AND R.TIPO_AULA = ISNULL(REPLACE(T1.TIPO_AULA, 'NGLP', 'GLP'), 'NORMAL')
														AND R.DATAGREVE = @DATAGREVE), 0) AS PENDENTES,
									ISNULL(REPLACE(T1.TIPO_AULA, 'NGLP', 'GLP'), 'NORMAL') AS TIPO_AULA,
									CASE
                                    WHEN (SELECT SUM(R.CHREPOSICAO)
												FROM Reposicao.REPOSICAO R WHERE D.NUM_FUNC = R.NUM_FUNC 
																	AND DD.TURMA = R.TURMA 
																	AND DD.ANO = R.ANO 
																	AND DD.SEMESTRE = R.SEMESTRE
																	AND DD.DISCIPLINA = R.DISCIPLINA
																	AND R.TIPO_AULA = ISNULL(REPLACE(T1.TIPO_AULA, 'NGLP', 'GLP'), 'NORMAL')
														AND R.DATAGREVE = @DATAGREVE) = 0 THEN 'Recusado'
										WHEN ISNULL((SELECT SUM(R.CHREPOSICAO) 
												FROM Reposicao.REPOSICAO R WHERE D.NUM_FUNC = R.NUM_FUNC 
																	AND DD.TURMA = R.TURMA 
																	AND DD.ANO = R.ANO 
																	AND DD.SEMESTRE = R.SEMESTRE
																	AND DD.DISCIPLINA = R.DISCIPLINA
																	AND R.TIPO_AULA = ISNULL(REPLACE(T1.TIPO_AULA, 'NGLP', 'GLP'), 'NORMAL')
														AND R.DATAGREVE = @DATAGREVE), 0) >= COUNT(distinct DD.AULA) THEN 'Cumprida'
										ELSE 'Pendente'
								END SITUACAO
                            FROM LY_DOCENTE D
	                            INNER JOIN VW_FUNCIONARIOS F ON D.MATRICULA = F.MATRICULA
	                            INNER JOIN LY_LICENCA_DOCENTE LD ON D.NUM_FUNC = LD.NUM_FUNC
	                            INNER JOIN Reposicao.LY_AULA_DOCENTE_CONGELADA DD on ld.NUM_FUNC = DD.NUM_FUNC
								LEFT JOIN Reposicao.LY_AULA_DOCENTE_TIPO_CONGELADA T1 (NOLOCK)
                                                on  T1.NUM_FUNC=DD.NUM_FUNC
                                                        AND T1.TURNO=DD.TURNO
                                                        AND T1.FACULDADE = DD.FACULDADE
                                                        AND T1.DIA_SEMANA=DD.DIA_SEMANA
                                                        AND T1.AULA=DD.AULA
                                                        AND T1.DISCIPLINA = DD.DISCIPLINA
                                                        AND T1.TURMA=DD.TURMA
							                            AND T1.ANO = DD.ANO
                                                        AND T1.SEMESTRE = DD.SEMESTRE                                                   
                                                        AND T1.DATA_INICIO = DD.DATA_INICIO
														--AND T1.TIPO_AULA = 'GLP'
                                                        AND @DATAGREVE BETWEEN T1.DATA_INICIO AND T1.DATA_FIM
	                            INNER JOIN LY_DISCIPLINA DI ON DD.DISCIPLINA = DI.DISCIPLINA
                            where ld.MOTIVO = '61' --greve
	                            AND @DATAGREVE BETWEEN LD.DTINI AND LD.DTFIM
                                AND @DATAGREVE BETWEEN DD.DATA_INICIO AND DD.DATA_FIM
	                            AND dd.DIA_SEMANA = @DIA_SEMANA
	                            AND DD.TURMA = @TURMA
	                            AND DD.ANO = @ANO
	                            AND DD.SEMESTRE = @SEMESTRE
	                            AND d.MATRICULA not in('00000000','11111111','22222222','44444444','66666666','88888888','55555551','55555555','99999999')
	                        GROUP BY D.NUM_FUNC, 
		                            D.MATRICULA,
		                            F.IDVINCULO,
		                            F.NOME_COMPL,
		                            DD.TURMA,
		                            DD.DISCIPLINA,
		                            DI.NOME,
									DD.ANO,
									DD.SEMESTRE,
									ISNULL(REPLACE(T1.TIPO_AULA, 'NGLP', 'GLP'), 'NORMAL')
                               ORDER BY DI.NOME, F.NOME_COMPL ";

                contextQuery.Parameters.Add("@DATAGREVE", SqlDbType.DateTime, dataGreve.Date);
                contextQuery.Parameters.Add("@DIA_SEMANA", SqlDbType.Int, diaSemana);
                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, semestre);

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

        public ValidacaoDados Valida(Entidades.Reposicao reposicao, bool recusado, int periodoLancamentoId, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.Reposicao.PeriodoLancamento rnPeriodoLancamento = new PeriodoLancamento();
            RN.Reposicao.Entidades.PeriodoLancamento periodoLancamento = new Entidades.PeriodoLancamento();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (reposicao == null)
            {
                return validacaoDados;
            }

            if (!cadastro)
            {
                if (reposicao.ReposicaoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (periodoLancamentoId <= 0)
            {
                mensagens.Add("Campo PERÍODO LANCAMENTO é obrigatório.");
            }

            if (reposicao.NumFunc <= 0)
            {
                mensagens.Add("Campo NUM_FUNC é obrigatório.");
            }

            if (reposicao.Censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CENSO é obrigatório.");
            }

            if (reposicao.Ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (reposicao.Semestre < 0)
            {
                mensagens.Add("Campo SEMESTRE é obrigatório.");
            }

            if (reposicao.TipoAula.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TIPO AULA é obrigatório.");
            }
            else if (reposicao.TipoAula.ToUpper() != "GLP" && reposicao.TipoAula.ToUpper() != "NORMAL")
            {
                mensagens.Add("Campo TIPO AULA inválido.");
            }

            if (reposicao.Turma.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TURMA é obrigatório.");
            }

            if (reposicao.Disciplina.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DISCIPLINA é obrigatório.");
            }

            if (!recusado)
            {
                if (reposicao.CHReposicao <= 0)
                {
                    mensagens.Add("Campo CH REPOSIÇÃO é obrigatório e deve ser maior que zero.");
                }

                if (reposicao.DataReposicao == DateTime.MinValue)
                {
                    mensagens.Add("Campo DATA REPOSIÇÃO é obrigatório.");
                }
                else if (reposicao.DataReposicao.Date > DateTime.Now.Date)
                {
                    mensagens.Add("A DATA REPOSIÇÃO não pode ser maior que a data atual.");
                }
                else if ((int)reposicao.DataReposicao.DayOfWeek == 0)//Verificar se não é domingo (0)
                {
                    mensagens.Add("A DATA REPOSIÇÃO não pode ser um domingo.");
                }
            }

            if (reposicao.DataGreve == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA GREVE é obrigatório.");
            }
            else if ((int)reposicao.DataGreve.DayOfWeek == 0)//Verificar se não é sabado (6) ou domingo (0)
            {
                mensagens.Add("A DATA GREVE não pode ser domingo.");
            }

            if (reposicao.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Busca dados do periodo
                    periodoLancamento = rnPeriodoLancamento.ObtemPor(contexto, periodoLancamentoId);

                    //Verifica se o periodo esta aberto
                    if (DateTime.Now.Date < Convert.ToDateTime(periodoLancamento.DataInicio).Date || DateTime.Now.Date > Convert.ToDateTime(periodoLancamento.DataFim).Date)
                    {
                        mensagens.Add("O período para o lançamento de reposição está fechado");
                    }

                    //Verifica se a data está no periodo
                    if (reposicao.DataGreve.Date < Convert.ToDateTime(periodoLancamento.DataInicioGreve).Date || reposicao.DataGreve.Date > Convert.ToDateTime(periodoLancamento.DataFimGreve).Date)
                    {
                        mensagens.Add(string.Format("A DATA DA GREVE deve estar no periodo de greve, entre os dias {0} e {1}",
                            periodoLancamento.DataInicioGreve.ToString("dd/MM/yyyy"),
                            Convert.ToDateTime(periodoLancamento.DataFimGreve).ToString("dd/MM/yyyy")));
                    }

                    //a reposição tem q ser após o período da greve 
                    if (reposicao.DataReposicao.Date <= Convert.ToDateTime(periodoLancamento.DataFimGreve).Date)
                    {
                        mensagens.Add("A DATA REPOSIÇÃO deve ser maior que a data fim da greve.");
                    }

                    // 2. A data de reposição deve estar contida no conjunto de dias indicado no filtro 
                    //“Período de Referência”, incluindo sábados;
                    if (reposicao.DataReposicao.Date > Convert.ToDateTime(periodoLancamento.DataFim).Date || reposicao.DataReposicao.Date < Convert.ToDateTime(periodoLancamento.DataInicio).Date)
                    {
                        mensagens.Add("A DATA DA REPOSIÇÃO deve estar no período de lançamento");
                    }

                    //Busca carga horaria maxima a ser cumprida no dia / turma / disciplina / docente
                    int chMaxima = this.ObtemQuantidadeTemposPor(contexto, periodoLancamentoId, reposicao.DataGreve.Date, reposicao.Ano, reposicao.Semestre, reposicao.Turma, reposicao.Disciplina, reposicao.NumFunc, reposicao.TipoAula);

                    //Busca carga horaria já cumprida no dia / turma / disciplina / docente
                    int chReposta = this.ObtemQuantidadeTemposRepostosPor(contexto, reposicao.ReposicaoId, reposicao.DataGreve.Date, reposicao.Ano, reposicao.Semestre, reposicao.Turma, reposicao.Disciplina, reposicao.NumFunc, reposicao.TipoAula);

                    //3. A Carga Horária (CH) de reposição não pode ser maior que o total acumulado para um mesmo 
                    //docente coluna “CH de Aula” do grid “Professores Licenciados – Greve”, podendo ser inferior a 
                    //esse total. Caso a CH seja inferior, o sistema deve permitir novo cadastro com mesmas informações 
                    //até que a CH total seja atingida.
                    if (reposicao.CHReposicao + chReposta > chMaxima)
                    {
                        mensagens.Add("A CARGA HORÁRIA (CH) de reposição não pode ser maior que o total acumulado para um mesmo docente na turma / disciplina");
                    }

                    //Verifica se já foi cadastrado
                    if (this.PossuiOutroCadastroPor(contexto, reposicao))
                    {
                        mensagens.Add("Já foi cadastrada uma reposição para esta data / turma / disciplina / docente.");
                    }

                    //Verifica se ja foi recusado em outro cadastro no dia / turma / disciplina / docente
                    if (this.PossuiOutroCadastroRecusadoPor(contexto, reposicao.ReposicaoId, reposicao.DataGreve.Date, reposicao.Ano, reposicao.Semestre, reposicao.Turma, reposicao.Disciplina, reposicao.NumFunc, reposicao.TipoAula))
                    {
                        mensagens.Add("Esta data / turma / disciplina foi recusada por este docente.");
                    }

                    //Verifica se está sendo recusado e já existe outro cadastro com ch
                    if (recusado && chReposta > 0)
                    {
                        mensagens.Add("Esta reposição não pode ser recusada, pois já existe um cadastro de carga horária reposta para esta data / turma / disciplina / docente.");
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

        private int ObtemQuantidadeTemposPor(DataContext contexto, int periodoLancamentoId, DateTime dataGreve, int ano, int semestre, string turma, string disciplina, int numFunc, string tipoAula)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;

            try
            {
                //Verifica dia da semana
                int diaSemana = (int)dataGreve.Date.DayOfWeek + 1; //Dia da semana no banco começa em 1 (domingo) o DayOfWeek começa em 0 (domingo);

                contextQuery.Command = @" SELECT COUNT(distinct DD.AULA) AS TEMPOS
                            FROM LY_DOCENTE D
	                            INNER JOIN LY_LICENCA_DOCENTE LD ON D.NUM_FUNC = LD.NUM_FUNC
	                            INNER JOIN Reposicao.LY_AULA_DOCENTE_CONGELADA DD on ld.NUM_FUNC = DD.NUM_FUNC
								LEFT JOIN Reposicao.LY_AULA_DOCENTE_TIPO_CONGELADA T1 (NOLOCK)
                                                on  T1.NUM_FUNC=DD.NUM_FUNC
                                                        AND T1.TURNO=DD.TURNO
                                                        AND T1.FACULDADE = DD.FACULDADE
                                                        AND T1.DIA_SEMANA=DD.DIA_SEMANA
                                                        AND T1.AULA=DD.AULA
                                                        AND T1.DISCIPLINA = DD.DISCIPLINA
                                                        AND T1.TURMA=DD.TURMA
							                            AND T1.ANO = DD.ANO
                                                        AND T1.SEMESTRE = DD.SEMESTRE                                                   
                                                        AND T1.DATA_INICIO = DD.DATA_INICIO
														--AND T1.TIPO_AULA = 'GLP'
                                                        AND @DATAGREVE BETWEEN T1.DATA_INICIO AND T1.DATA_FIM
                            where ld.MOTIVO = '61' --greve
	                            AND @DATAGREVE BETWEEN LD.DTINI AND LD.DTFIM
                                AND @DATAGREVE BETWEEN DD.DATA_INICIO AND DD.DATA_FIM
	                            AND DD.DIA_SEMANA = @DIA_SEMANA
	                            AND DD.TURMA = @TURMA
	                            AND DD.DISCIPLINA = @DISCIPLINA
	                            AND DD.ANO = @ANO
	                            AND DD.SEMESTRE = @SEMESTRE
								AND DD.NUM_FUNC = @NUM_FUNC
								AND ISNULL(REPLACE(T1.TIPO_AULA, 'NGLP', 'GLP'), 'NORMAL') = @TIPO_AULA
	                            AND d.MATRICULA not in('00000000','11111111','22222222','44444444','66666666','88888888','55555551','55555555','99999999') ";

                contextQuery.Parameters.Add("@DATAGREVE", SqlDbType.DateTime, dataGreve.Date);
                contextQuery.Parameters.Add("@DIA_SEMANA", SqlDbType.Int, diaSemana);
                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
                contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, disciplina);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, semestre);
                contextQuery.Parameters.Add("@TIPO_AULA", SqlDbType.VarChar, tipoAula);
                contextQuery.Parameters.Add("@NUM_FUNC", SqlDbType.Int, numFunc);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["TEMPOS"]);
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

        private int ObtemQuantidadeTemposRepostosPor(DataContext contexto, int reposicaoId, DateTime dataGreve, int ano, int semestre, string turma, string disciplina, int numFunc, string tipoAula)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT SUM(CHREPOSICAO) AS TEMPOS
                                    FROM Reposicao.REPOSICAO
                                    WHERE DATAGREVE = @DATAGREVE
                                        AND TURMA = @TURMA
                                        AND DISCIPLINA = @DISCIPLINA
                                        AND ANO = @ANO
                                        AND SEMESTRE = @SEMESTRE
                                        AND NUM_FUNC = @NUM_FUNC 
                                        AND TIPO_AULA = @TIPO_AULA
	                                    AND REPOSICAOID <> @REPOSICAOID ";

                contextQuery.Parameters.Add("@REPOSICAOID", SqlDbType.Int, reposicaoId);
                contextQuery.Parameters.Add("@DATAGREVE", SqlDbType.DateTime, dataGreve.Date);
                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
                contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, disciplina);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, semestre);
                contextQuery.Parameters.Add("@TIPO_AULA", SqlDbType.VarChar, tipoAula);
                contextQuery.Parameters.Add("@NUM_FUNC", SqlDbType.Int, numFunc);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = reader["TEMPOS"] == DBNull.Value ? 0 : Convert.ToInt32(reader["TEMPOS"]);
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

        private bool PossuiOutroCadastroRecusadoPor(DataContext ctx, int reposicaoId, DateTime dataGreve, int ano, int semestre, string turma, string disciplina, int numFunc, string tipoAula)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*)
                                    FROM Reposicao.REPOSICAO
                                    WHERE DATAGREVE = @DATAGREVE
                                        AND TURMA = @TURMA
                                        AND DISCIPLINA = @DISCIPLINA
                                        AND ANO = @ANO
                                        AND SEMESTRE = @SEMESTRE
                                        AND NUM_FUNC = @NUM_FUNC 
                                        AND TIPO_AULA = @TIPO_AULA
                                        AND CHREPOSICAO = 0 
	                                    AND REPOSICAOID <> @REPOSICAOID ";

            contextQuery.Parameters.Add("@REPOSICAOID", SqlDbType.Int, reposicaoId);
            contextQuery.Parameters.Add("@DATAGREVE", SqlDbType.DateTime, dataGreve.Date);
            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
            contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, disciplina);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, semestre);
            contextQuery.Parameters.Add("@TIPO_AULA", SqlDbType.VarChar, tipoAula);
            contextQuery.Parameters.Add("@NUM_FUNC", SqlDbType.Int, numFunc);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutroCadastroPor(DataContext ctx, Entidades.Reposicao reposicao)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1)
                                    FROM Reposicao.REPOSICAO
                                    WHERE NUM_FUNC = @NUM_FUNC
	                                    AND	TURMA = @TURMA
	                                    AND	ANO = @ANO
	                                    AND	SEMESTRE = @SEMESTRE
	                                    AND	DISCIPLINA = @DISCIPLINA
	                                    AND	DATAGREVE = @DATAGREVE
	                                    AND	DATAREPOSICAO = @DATAREPOSICAO
	                                    AND	TIPO_AULA = @TIPO_AULA
	                                    AND REPOSICAOID <> @REPOSICAOID ";

            contextQuery.Parameters.Add("@REPOSICAOID", SqlDbType.Int, reposicao.ReposicaoId);
            contextQuery.Parameters.Add("@NUM_FUNC", SqlDbType.Decimal, reposicao.NumFunc);
            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, reposicao.Turma);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Decimal, reposicao.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Decimal, reposicao.Semestre);
            contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, reposicao.Disciplina);
            contextQuery.Parameters.Add("@TIPO_AULA", SqlDbType.VarChar, reposicao.TipoAula);
            contextQuery.Parameters.Add("@DATAGREVE", SqlDbType.DateTime, reposicao.DataGreve.Date);
            contextQuery.Parameters.Add("@DATAREPOSICAO", SqlDbType.DateTime, reposicao.DataReposicao.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.Reposicao reposicao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO Reposicao.REPOSICAO
                                               (NUM_FUNC
                                               ,CENSO
                                               ,DISCIPLINA
                                               ,TURMA
                                               ,ANO
                                               ,SEMESTRE
                                               ,TIPO_AULA
                                               ,DATAREPOSICAO
                                               ,CHREPOSICAO
                                               ,DATAGREVE
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@NUM_FUNC, 
                                               @CENSO, 
                                               @DISCIPLINA, 
                                               @TURMA, 
                                               @ANO, 
                                               @SEMESTRE, 
                                               @TIPO_AULA,
                                               @DATAREPOSICAO, 
                                               @CHREPOSICAO, 
                                               @DATAGREVE, 
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@NUM_FUNC", SqlDbType.Decimal, reposicao.NumFunc);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, reposicao.Censo);
                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, reposicao.Turma);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Decimal, reposicao.Ano);
                contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Decimal, reposicao.Semestre);
                contextQuery.Parameters.Add("@TIPO_AULA", SqlDbType.VarChar, reposicao.TipoAula.ToUpper());
                contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, reposicao.Disciplina);
                contextQuery.Parameters.Add("@DATAREPOSICAO", SqlDbType.DateTime, reposicao.DataReposicao.Date);
                contextQuery.Parameters.Add("@CHREPOSICAO", SqlDbType.Int, reposicao.CHReposicao);
                contextQuery.Parameters.Add("@DATAGREVE", SqlDbType.DateTime, reposicao.DataGreve.Date);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, reposicao.UsuarioId);
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

        public void Atualiza(Entidades.Reposicao reposicao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE Reposicao.REPOSICAO
                                       SET TIPO_AULA = @TIPO_AULA,
                                          DATAREPOSICAO = @DATAREPOSICAO, 
                                          CHREPOSICAO = @CHREPOSICAO, 
                                          USUARIOID = @USUARIOID, 
                                          DATAALTERACAO = @DATAALTERACAO
                                     WHERE REPOSICAOID = @REPOSICAOID ";

                contextQuery.Parameters.Add("@REPOSICAOID", SqlDbType.Int, reposicao.ReposicaoId);
                contextQuery.Parameters.Add("@DATAREPOSICAO", SqlDbType.DateTime, reposicao.DataReposicao.Date);
                contextQuery.Parameters.Add("@TIPO_AULA", SqlDbType.VarChar, reposicao.TipoAula.ToUpper());
                contextQuery.Parameters.Add("@CHREPOSICAO", SqlDbType.Int, reposicao.CHReposicao);
                contextQuery.Parameters.Add("@DATAGREVE", SqlDbType.DateTime, reposicao.DataGreve.Date);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, reposicao.UsuarioId);
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

        public void Remove(int reposicaoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE Reposicao.REPOSICAO
                                        WHERE REPOSICAOID = @REPOSICAOID ";

                contextQuery.Parameters.Add("@REPOSICAOID", SqlDbType.Int, reposicaoId);

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

        public int ObtemQuantidadeTemposPendentesPor(int periodoLancamentoId, DateTime dataGreve, int ano, int semestre, string turma, string disciplina, int numFunc, string tipoAula)
        {
            DataContext contexto = null;
            int chPendende = 0;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                //Busca carga horaria maxima a ser cumprida no dia / turma / disciplina / docente
                int chMaxima = this.ObtemQuantidadeTemposPor(contexto, periodoLancamentoId, dataGreve, ano, semestre, turma, disciplina, numFunc, tipoAula);

                //Busca carga horaria já cumprida no dia / turma / disciplina / docente
                int chReposta = this.ObtemQuantidadeTemposRepostosPor(contexto, periodoLancamentoId, dataGreve, ano, semestre, turma, disciplina, numFunc, tipoAula);

                chPendende = chMaxima - chReposta;

                return chPendende;
            }
            catch (Exception ex)
            {
                if (contexto != null)
                {
                    contexto.Abandon();
                }

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
        }
    }
}
