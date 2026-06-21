using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.DTOs;
using Seeduc.Infra.Data;
using Seeduc.Infra.Extensions;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;

namespace Techne.Lyceum.RN.Turmas
{
    public class Notificacao
    {
        public bool PossuiFormaContatoPor(DataContext contexto, int formaContatoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                      FROM  Turma.NOTIFICACAO (NOLOCK)
                                      WHERE [FORMACONTATOID_1] = @FORMACONTATOID
									    OR [FORMACONTATOID_2] = @FORMACONTATOID
									    OR [FORMACONTATOID_3] = @FORMACONTATOID ";

            contextQuery.Parameters.Add("@FORMACONTATOID", SqlDbType.Int, formaContatoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiMedidaMPRJPor(DataContext contexto, int medidaMPRJId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                      FROM  Turma.NOTIFICACAO (NOLOCK)
                                      WHERE MEDIDAMPRJID = @MEDIDAMPRJID ";

            contextQuery.Parameters.Add("@MEDIDAMPRJID", SqlDbType.Int, medidaMPRJId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiMedidaConselhoTutelarPor(DataContext contexto, int medidaConselhoTutelarId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                      FROM  Turma.NOTIFICACAO (NOLOCK)
                                      WHERE MEDIDACONSELHOTUTELARID = @MEDIDACONSELHOTUTELARID ";

            contextQuery.Parameters.Add("@MEDIDACONSELHOTUTELARID", SqlDbType.Int, medidaConselhoTutelarId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiSituacaoFamiliarPor(DataContext contexto, int situacaoFamiliarId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                      FROM  Turma.NOTIFICACAO (NOLOCK)
                                      WHERE SITUACAOFAMILIARID = @SITUACAOFAMILIARID ";

            contextQuery.Parameters.Add("@SITUACAOFAMILIARID", SqlDbType.Int, situacaoFamiliarId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiTipoEncaminhamentoPor(DataContext contexto, int tipoEncaminhamentoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                      FROM  Turma.NOTIFICACAO (NOLOCK)
                                      WHERE TIPOENCAMINHAMENTOID = @TIPOENCAMINHAMENTOID ";

            contextQuery.Parameters.Add("@TIPOENCAMINHAMENTOID", SqlDbType.Int, tipoEncaminhamentoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable ListaPor(string aluno)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" select NOTIFICACAOID, 
		                                        NUMEROFICAI, 
		                                        NUMEROFAMI, 
		                                        CONVERT(VARCHAR,DATACOMUNICACAO,103) AS DATACOMUNICACAO, 
		                                        QUANTIDADEFALTAS,
		                                        U.USUARIO + ' - ' + U.NOME AS SERVIDOR
                                        from Turma.NOTIFICACAO n
	                                        INNER JOIN HADES..HD_USUARIO U ON n.USUARIOID = U.USUARIO
                                        WHERE ALUNO = @ALUNO ";

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

        public Entidades.Notificacao ObtemPor(int notificacaoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            Entidades.Notificacao notificacao = new Entidades.Notificacao();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT *
                                        FROM Turma.NOTIFICACAO
                                        WHERE NOTIFICACAOID = @NOTIFICACAOID ";

                contextQuery.Parameters.Add("@NOTIFICACAOID", SqlDbType.Int, notificacaoId);

                notificacao = contexto.TryToBindEntity<Entidades.Notificacao>(contextQuery);

                return notificacao;
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

        public int ObtemQuantidadeFaltasPor(string aluno, out DateTime? dataInicio)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            dataInicio = null;
            int faltas = 0;
            List<DateTime> listaGeral = new List<DateTime>();
            List<DateTime> listaFalta = new List<DateTime>();
            List<DTOs.DadosTurma> turmas = new List<DTOs.DadosTurma>();
            List<DTOs.DadosAluno> alunos = new List<DTOs.DadosAluno>();
            List<DTOs.DadosAlunoDia> alunoDia = new List<DTOs.DadosAlunoDia>();

            try
            {
                //Busca maior data de cada turma do aluno
                turmas = this.ListaTurma(contexto, aluno);

                //Busca dias aulas do aluno
                alunos = this.ListaAlunos(contexto, aluno);

                List<string> turmasAluno = alunos.Where(y => y.Aluno == aluno).Select(x => x.Turma).Distinct().ToList();

                if (turmas.Count > 0)
                {
                    var turmasFiltradas = turmas
                                .Where(a => turmasAluno.Contains(a.Turma))
                                .ToList();

                    //Verifica se todas as turmas do aluno tem data de maior lançamento de frequencia
                    if (turmasAluno.Count() == turmasFiltradas.Count())
                    {
                        //Percorre os dias do aluno
                        foreach (DateTime data in alunos.Where(y => y.Aluno == aluno).Select(x => x.Data).Distinct())
                        {
                            DTOs.DadosAlunoDia dia = new DTOs.DadosAlunoDia();
                            dia.Aluno = aluno;
                            dia.Data = data;

                            //Verifica se todas as disciplina daquele dia estão com falta
                            if (alunos.Where(x => x.Aluno == aluno && x.Data == data && x.Falta == false).Count() == 0)
                            {
                                dia.FaltaDia = true;
                            }
                            else
                            {
                                dia.FaltaDia = false;
                            }

                            dia.UltimoDiaLancamento = turmasFiltradas.Max(x => x.MaiorData);
                            alunoDia.Add(dia);
                        }
                    }
                }

                //Lista todos os dias com aula do aluno
                listaGeral = alunoDia.Select(x => x.Data).Distinct().ToList();

                //Lista todos os dias com falta em todas as aulas do aluno
                listaFalta = alunoDia.Where(x => x.FaltaDia == true).Select(x => x.Data).Distinct().ToList();

                //Verifica se faltou no ultimo dia (caso nao tenha faltado nao precisa calcular os outros
                if (listaGeral.Count() != 0 && listaFalta.Count() != 0)
                {
                    if (alunoDia.Max(x => x.UltimoDiaLancamento) == listaFalta.Max())
                    {
                        var resultadoConsecutivos = this.CalculaDiasConsecutivosPor(listaGeral, listaFalta);

                        if (resultadoConsecutivos.QuantidadeDias > 0)
                        {
                            faltas = resultadoConsecutivos.QuantidadeDias;
                            dataInicio = resultadoConsecutivos.Inicio;
                        }
                        else
                        {
                            faltas = 0;
                            dataInicio = null;
                        }
                    }
                }

                return faltas;
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
                if (reader != null)
                {
                    reader.Close();
                }
                if (contexto != null)
                {
                    contexto.Dispose();
                }
            }
        }

        public List<DTOs.DadosTurma> ListaTurma(DataContext contexto, string aluno)
        {
            List<DTOs.DadosTurma> listaDados = new List<DTOs.DadosTurma>();
            DTOs.DadosTurma turma = new DTOs.DadosTurma();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"  SELECT DISTINCT
			                                    M.ANO
			                                    , M.SEMESTRE
			                                    , M.TURMA	
			                                    , FACULDADE as CENSO			
			                                    , MAX(DATAFREQUENCIA) MAIOR_DATA	
	                                    FROM Turma.FREQUENCIADIARIA MD
	                                    INNER JOIN LY_MATRICULA M ON M.ANO = MD.ANO AND M.SEMESTRE  =MD.SEMESTRE AND M.TURMA = MD.TURMA AND M.DISCIPLINA = MD.DISCIPLINA
                                           WHERE M.ANO = YEAR(GETDATE())
	                                            AND SIT_MATRICULA = 'Matriculado'	
	                                            AND ISNULL(M.DEPENDENCIA,'N') = 'N'
												AND M.ALUNO = @ALUNO
										   GROUP BY M.ANO, M.SEMESTRE, M.TURMA, MD.FACULDADE  ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    turma = new DTOs.DadosTurma();

                    turma.Ano = Convert.ToInt32(reader["ANO"]);
                    turma.Semestre = Convert.ToInt32(reader["SEMESTRE"]);
                    turma.Turma = Convert.ToString(reader["TURMA"]);
                    turma.Censo = Convert.ToString(reader["CENSO"]);
                    turma.MaiorData = Convert.ToDateTime(reader["MAIOR_DATA"]);

                    listaDados.Add(turma);
                }

                return listaDados;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public List<DTOs.DadosAluno> ListaAlunos(DataContext contexto, string aluno)
        {
            List<DTOs.DadosAluno> listaDados = new List<DTOs.DadosAluno>();
            DTOs.DadosAluno dadosAluno = new DTOs.DadosAluno();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                //contexto.CommandTimeout = 100; //100 segundos

                contextQuery.Command = @"   SELECT T.*
                                            INTO #LY_TURMA
                                            FROM LY_TURMA T
	                                            INNER JOIN LY_MATRICULA M ON M.ANO = t.ANO AND M.SEMESTRE  =t.SEMESTRE AND M.TURMA = t.TURMA AND M.DISCIPLINA = t.DISCIPLINA
	                                            INNER JOIN LY_DISCIPLINA D  ON ISNULL(t.DISCIPLINA_MULTIPLA, t.DISCIPLINA) = D.DISCIPLINA
                                            where D.TEM_FREQ = 'S'
	                                            and T.ANO =  YEAR(GETDATE())
	                                            AND T.SIT_TURMA = 'ABERTA' 
	                                            AND M.ALUNO = @ALUNO
												AND SIT_MATRICULA = 'Matriculado'

                                            SELECT * 
                                            INTO #DATAS
                                            from #LY_TURMA as m
	                                            CROSS APPLY  Turma.FN_DATAS_AULA_PROCESSO (m.DISCIPLINA, M.TURMA, YEAR(GETDATE()), m.SEMESTRE) p
	                                              

                                            SELECT DISTINCT 
				                                            MD.ANO,
				                                            MD.SEMESTRE,
				                                            MD.FACULDADE AS CENSO,				
				                                            MD.TURMA,	
				                                            MD.DISCIPLINA,	
				                                            M.ALUNO,		
				                                            MD.DATA,
				                                            DAY(MD.DATA) AS DIA,
				                                            CONVERT(INT,0) AS TOTAL_TEMPOS,
				                                            CONVERT(INT,0) AS TOTAL_FALTA,
				                                            CONVERT(BIT,0) AS TEVE_LANCAMENTO,
				                                            CONVERT(BIT,0) AS FALTA					
                                            INTO #GERAL
                                            FROM #DATAS as MD
	                                            INNER JOIN LY_MATRICULA M ON M.ANO = MD.ANO AND M.SEMESTRE  =MD.SEMESTRE AND M.TURMA = MD.TURMA AND M.DISCIPLINA = MD.DISCIPLINA
                                            WHERE 
	                                            MD.ANO = YEAR(GETDATE())	
	                                            AND SIT_MATRICULA = 'Matriculado'	
	                                            AND ISNULL(M.DEPENDENCIA,'N') = 'N'
	                                            AND EXISTS (SELECT TOP 1 1 
					                                            FROM Turma.FREQUENCIADIARIA FD 
     				                                            inner join Turma.FREQUENCIADIARIA_ALUNOFALTA fda on fd.FREQUENCIADIARIAID=fda.FREQUENCIADIARIAID 
					                                            WHERE fda.ALUNO = m.ALUNO
						                                            AND FDA.ATIVO = 1
						                                            AND FD.DATAFREQUENCIA >= DATEADD(MONTH, -2, GETDATE())) --Tenha falta nos ultimos 2 meses
	                                            AND M.ALUNO = @ALUNO
	
                                            CREATE NONCLUSTERED INDEX [FREQ_DIARIA_FK] ON #GERAL
                                            (
	                                            ANO ASC,
	                                            SEMESTRE ASC,
	                                            TURMA ASC,
	                                            DISCIPLINA ASC,
	                                            DATA ASC
                                            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY];

                                            UPDATE #GERAL SET
                                            TOTAL_TEMPOS = (SELECT COUNT(AULA) 
					                                            FROM Turma.FREQUENCIADIARIA FD 
					                                            WHERE FD.ANO=#GERAL.ANO 
							                                            AND FD.SEMESTRE=#GERAL.SEMESTRE 
							                                            AND FD.TURMA=#GERAL.TURMA 
							                                            AND FD.DISCIPLINA=#GERAL.DISCIPLINA 
							                                            AND FD.DATAFREQUENCIA=#GERAL.DATA)

                                            UPDATE #GERAL SET
                                            TEVE_LANCAMENTO = 1
                                            where TOTAL_TEMPOS > 0
 
                                            UPDATE #GERAL SET
                                            TOTAL_FALTA = (SELECT COUNT(AULA) 
					                                            FROM Turma.FREQUENCIADIARIA FD 
     				                                            inner join Turma.FREQUENCIADIARIA_ALUNOFALTA fda on fd.FREQUENCIADIARIAID=fda.FREQUENCIADIARIAID 
					                                            WHERE FD.ANO=#GERAL.ANO 
							                                            AND FD.SEMESTRE=#GERAL.SEMESTRE 
							                                            AND FD.TURMA=#GERAL.TURMA 
							                                            AND FD.DISCIPLINA=#GERAL.DISCIPLINA 
							                                            AND FD.DATAFREQUENCIA=#GERAL.DATA
							                                            AND fda.ALUNO = #GERAL.ALUNO
							                                            AND FDA.ATIVO = 1 )
                                            WHERE TEVE_LANCAMENTO = 1 
 
                                            UPDATE #GERAL SET
                                            FALTA = 1
                                            WHERE 
	                                            TEVE_LANCAMENTO = 1
	                                            AND TOTAL_TEMPOS <> 0
	                                            AND TOTAL_FALTA = TOTAL_TEMPOS
 
                                            SELECT * FROM #GERAL ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosAluno = new DTOs.DadosAluno();

                    dadosAluno.Ano = Convert.ToInt32(reader["ANO"]);
                    dadosAluno.Semestre = Convert.ToInt32(reader["SEMESTRE"]);
                    dadosAluno.Turma = Convert.ToString(reader["TURMA"]);
                    dadosAluno.Censo = Convert.ToString(reader["CENSO"]);
                    dadosAluno.Disciplina = Convert.ToString(reader["DISCIPLINA"]);
                    dadosAluno.Aluno = Convert.ToString(reader["ALUNO"]);
                    dadosAluno.Data = Convert.ToDateTime(reader["DATA"]);
                    dadosAluno.TotalTempos = Convert.ToInt32(reader["TOTAL_TEMPOS"]);
                    dadosAluno.TotalFaltas = Convert.ToInt32(reader["TOTAL_FALTA"]);
                    dadosAluno.TeveLancamento = Convert.ToBoolean(reader["TEVE_LANCAMENTO"]);
                    dadosAluno.Falta = Convert.ToBoolean(reader["FALTA"]);

                    listaDados.Add(dadosAluno);
                }

                return listaDados;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public DTOs.ResultadoConsecutivos CalculaDiasConsecutivosPor(List<DateTime> listaAulas, List<DateTime> listaFaltas)
        {
            DTOs.ResultadoConsecutivos retorno = new DTOs.ResultadoConsecutivos();

            var intersecao = listaAulas.Intersect(listaFaltas).OrderBy(d => d).ToList();

            // Mapeia cada data de ListaA com índice (equivale ao ROW_NUMBER)
            var sequencia = listaAulas.OrderBy(d => d)
                                  .Select((data, index) => new { Data = data.Date, Rn = index })
                                  .ToList();

            // Junta com a interseção para calcular o grupo (grp = Rn - SeqIntersecao)
            var sequenciaIntersecao = sequencia
                .Where(x => intersecao.Contains(x.Data))
                .Select((x, i) => new
                {
                    x.Data,
                    x.Rn,
                    Grupo = x.Rn - i  // mesma ideia do SQL: grp = rn - row_number()
                })
                .ToList();

            if (sequenciaIntersecao.Count == 0)
                return null;

            // Grupo com a MAIOR DATA (mais recente)
            var maiorGrupo = sequenciaIntersecao
                .GroupBy(x => x.Grupo)
                .OrderByDescending(g => g.Max(x => x.Data))
                .First();

            var datasGrupo = maiorGrupo.Select(x => x.Data).OrderBy(d => d).ToList();

            retorno = new DTOs.ResultadoConsecutivos
            {
                Inicio = datasGrupo.First(),
                Fim = datasGrupo.Last(),
                QuantidadeDias = datasGrupo.Count
            };

            return retorno;
        }

        private string ObtemNumeroFicaiPor(DataContext contexto, int ano, out int sequencial)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                sequencial = 1;
                string numero;

                contextQuery.Command = @" select ISNULL(MAX(SEQUENCIAL),0) AS TOTAL
                                        from Turma.NOTIFICACAO (NOLOCK)
                                        WHERE ANO = @ANO
	                                          AND NUMEROFICAI IS NOT NULL ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    sequencial = Convert.ToInt32(reader["TOTAL"]) + 1;
                }

                numero = string.Format("{0}/{1}", Convert.ToString(sequencial).PadLeft(4, '0'), Convert.ToString(ano));
                return numero;
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

        private string ObtemNumeroFamiPor(DataContext contexto, int ano, out int sequencial)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            try
            {
                sequencial = 1;
                string numero;

                contextQuery.Command = @" select ISNULL(MAX(SEQUENCIAL),0) AS TOTAL
                                        from Turma.NOTIFICACAO (NOLOCK)
                                        WHERE ANO = @ANO
	                                          AND NUMEROFAMI IS NOT NULL ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    sequencial = Convert.ToInt32(reader["TOTAL"]) + 1;
                }

                numero = string.Format("{0}/{1}", Convert.ToString(sequencial).PadLeft(4, '0'), Convert.ToString(ano));
                return numero;
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

        public ValidacaoDados Valida(Entidades.Notificacao notificacao, int idade, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (notificacao == null)
            {
                return validacaoDados;
            }

            if (!cadastro)
            {
                if (notificacao.NotificacaoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (notificacao.Aluno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ALUNO é obrigatório.");
            }            

            if (notificacao.QuantidadeFaltas < 3)
            {
                mensagens.Add("Este aluno não possui ao menos 3 faltas consecutivas lançadas no Diário On Line.");
            }

            if (notificacao.DataInicioFaltas == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INÍCIO FALTAS é obrigatório.");
            }

            if (idade < 18)
            {
                //Aba FICAI

                if (notificacao.DataComunicacao == DateTime.MinValue || notificacao.Ano <= 0)
                {
                    mensagens.Add("Campo DATA DA COMUNICAÇÃO é obrigatório.");
                }
                else
                {
                    if (notificacao.DataComunicacao > DateTime.Now.Date)
                    {
                        mensagens.Add("Campo DATA DA COMUNICAÇÃO não pode ser maior que a data atual.");
                    }
                }

                if (!notificacao.Observacao.IsNullOrEmptyOrWhiteSpace() && notificacao.Observacao.Length > 2000)
                {
                    mensagens.Add("Campo OBSERVAÇÕES DO ESTUDANTE deve ter no máximo 2000 caracteres.");
                }

                if (notificacao.FormaContatoId1 == null || notificacao.FormaContatoId1 <= 0)
                {
                    mensagens.Add("Campo 1ª FORMA DE CONTATO é obrigatório.");
                }

                if (notificacao.DataContato1 == null || notificacao.DataContato1 == DateTime.MinValue)
                {
                    mensagens.Add("Campo 1ª DATA DE CONTATO é obrigatório.");
                }
                else
                {
                    if (notificacao.DataContato1 > DateTime.Now.Date)
                    {
                        mensagens.Add("Campo 1ª DATA DE CONTATO não pode ser maior que a data atual.");
                    }
                }

                if (notificacao.FormaContatoId2 != null && notificacao.FormaContatoId2 > 0)
                {
                    if (notificacao.DataContato2 == null || notificacao.DataContato2 == DateTime.MinValue)
                    {
                        mensagens.Add("Campo 2ª DATA DE CONTATO é obrigatório quando informado a 2ª FORMA DE CONTATO.");
                    }
                    else
                    {
                        if (notificacao.DataContato2 > DateTime.Now.Date)
                        {
                            mensagens.Add("Campo 2ª DATA DE CONTATO não pode ser maior que a data atual.");
                        }
                    }
                }
               

                if (notificacao.FormaContatoId3 != null && notificacao.FormaContatoId3 > 0)
                {
                    if (notificacao.DataContato3 == null || notificacao.DataContato3 == DateTime.MinValue)
                    {
                        mensagens.Add("Campo 3ª DATA DE CONTATO é obrigatório quando informado a 3ª FORMA DE CONTATO.");
                    }
                    else
                    {
                        if (notificacao.DataContato3 > DateTime.Now.Date)
                        {
                            mensagens.Add("Campo 3ª DATA DE CONTATO não pode ser maior que a data atual.");
                        }
                    }
                }
                

                if (notificacao.Alegacao.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo ALEGAÇÃO PARA AS FALTAS é obrigatório.");
                }
                else if (notificacao.Alegacao.Length > 2000)
                {
                    mensagens.Add("Campo ALEGAÇÃO PARA AS FALTAS deve ter no máximo 2000 caracteres.");
                }

                if (notificacao.TipoEncaminhamentoId == null || notificacao.TipoEncaminhamentoId <= 0)
                {
                    mensagens.Add("Campo ENCAMINHAMENTOS UE é obrigatório.");
                }


                if (notificacao.DataEncaminhamentoEscola == null || notificacao.DataEncaminhamentoEscola == DateTime.MinValue)
                {
                    mensagens.Add("Campo DATA DO ENCAMINHAMENTO é obrigatório.");
                }
                else
                {
                    if (notificacao.DataEncaminhamentoEscola > DateTime.Now.Date)
                    {
                        mensagens.Add("Campo DATA DO ENCAMINHAMENTO não pode ser maior que a data atual.");
                    }
                }


                //Limpa campos da aba FAMI
                notificacao.EncaminhamentosRealizado = null;
            }
            else
            {
                //Aba FAMI

                if (notificacao.DataComunicacao == DateTime.MinValue || notificacao.Ano <= 0)
                {
                    mensagens.Add("Campo DATA DA COMUNICAÇÃO é obrigatório.");
                }

                if (notificacao.Alegacao.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo ALEGAÇÃO PARA AS FALTAS é obrigatório.");
                }
                else if (notificacao.Alegacao.Length > 2000)
                {
                    mensagens.Add("Campo ALEGAÇÃO PARA AS FALTAS deve ter no máximo 2000 caracteres.");
                }

                if (notificacao.EncaminhamentosRealizado.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo ENCAMINHAMENTOS REALIZADOS é obrigatório.");
                }
                else if (notificacao.EncaminhamentosRealizado.Length > 2000)
                {
                    mensagens.Add("Campo ENCAMINHAMENTOS REALIZADOS deve ter no máximo 2000 caracteres.");
                }

                //Limpa campos da aba FICAI
                notificacao.Observacao = null;
                notificacao.TipoEncaminhamentoId = null;
                notificacao.EquipamentoUsado = null;
                notificacao.DataRetorno = null;
                notificacao.DataEncaminhamentoEscola = null;
                notificacao.ProtocoloConselho = null;
                notificacao.MedidasConselhoTutelarId = null;
                notificacao.DataEncaminhamentoConselho = null;
                notificacao.NomeConselheiro = null;
                notificacao.MedidaMPRJId = null;
                notificacao.DataEncaminhamentoMprj = null;
                notificacao.Promotor = null;
                notificacao.FormaContatoId1 = null;
                notificacao.DataContato1 = null;
                notificacao.FormaContatoId2 = null;
                notificacao.DataContato2 = null;
                notificacao.FormaContatoId3 = null;
                notificacao.DataContato3 = null;
                notificacao.SituacaoFamiliarId = null;
            }

            if (notificacao.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSAVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já foi cadastrada notificacao para aquela data
                    if (this.PossuiOutraNotificacaoPor(contexto, notificacao.Aluno, notificacao.DataComunicacao, notificacao.NotificacaoId))
                    {
                        mensagens.Add("Já foi cadastrada uma notificação para este ALUNO nesta DATA DE COMUNICAÇÃO.");
                    }

                    if (!notificacao.NumeroFicai.IsNullOrEmptyOrWhiteSpace() && this.PossuiOutraNotificacaoFicaiPor(contexto, notificacao.Aluno, notificacao.NumeroFicai, notificacao.NotificacaoId))
                    {
                        mensagens.Add("Já foi cadastrada uma notificação para este ALUNO com este Número FICAI.");
                    }

                    if (!notificacao.NumeroFami.IsNullOrEmptyOrWhiteSpace() && this.PossuiOutraNotificacaoFamiPor(contexto, notificacao.Aluno, notificacao.NumeroFami, notificacao.NotificacaoId))
                    {
                        mensagens.Add("Já foi cadastrada uma notificação para este ALUNO com este Número FAMI.");
                    }

                    if (mensagens.Count == 0 && cadastro)
                    {
                        int sequencial;

                        if (idade < 18)
                        {
                            notificacao.NumeroFicai = this.ObtemNumeroFicaiPor(contexto, notificacao.Ano, out sequencial);
                            notificacao.Sequencial = sequencial;
                        }
                        else
                        {
                            notificacao.NumeroFami = this.ObtemNumeroFamiPor(contexto, notificacao.Ano, out sequencial);
                            notificacao.Sequencial = sequencial;
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

        private bool PossuiOutraNotificacaoPor(DataContext ctx, string aluno, DateTime dataComunicacao, int notificacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Turma.NOTIFICACAO (NOLOCK)
                                    WHERE ALUNO = @ALUNO
										AND DATACOMUNICACAO = @DATACOMUNICACAO
                                        AND NOTIFICACAOID <> @NOTIFICACAOID ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@DATACOMUNICACAO", SqlDbType.Date, dataComunicacao.Date);
            contextQuery.Parameters.Add("@NOTIFICACAOID", SqlDbType.Int, notificacaoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutraNotificacaoFicaiPor(DataContext ctx, string aluno, string numeroFicai, int notificacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Turma.NOTIFICACAO (NOLOCK)
                                    WHERE ALUNO = @ALUNO
										AND NUMEROFICAI = @NUMEROFICAI
                                        AND NOTIFICACAOID <> @NOTIFICACAOID ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@NUMEROFICAI", SqlDbType.VarChar, numeroFicai);
            contextQuery.Parameters.Add("@NOTIFICACAOID", SqlDbType.Int, notificacaoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutraNotificacaoFamiPor(DataContext ctx, string aluno, string numeroFami, int notificacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Turma.NOTIFICACAO (NOLOCK)
                                    WHERE ALUNO = @ALUNO
										AND NUMEROFAMI = @NUMEROFAMI
                                        AND NOTIFICACAOID <> @NOTIFICACAOID ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@NUMEROFICAI", SqlDbType.VarChar, numeroFami);
            contextQuery.Parameters.Add("@NOTIFICACAOID", SqlDbType.Int, notificacaoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.Notificacao notificacao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO Turma.NOTIFICACAO
                                               (ALUNO
                                               ,ANO
                                               ,SEQUENCIAL
                                               ,QUANTIDADEFALTAS
                                               ,DATAINICIOFALTAS
                                               ,NUMEROFICAI
                                               ,NUMEROFAMI
                                               ,DATACOMUNICACAO
                                               ,OBSERVACAO
                                               ,FORMACONTATOID_1
                                               ,DATACONTATO_1
                                               ,FORMACONTATOID_2
                                               ,DATACONTATO_2
                                               ,FORMACONTATOID_3
                                               ,DATACONTATO_3
                                               ,SITUACAOFAMILIARID
                                               ,ALEGACAO
                                               ,TIPOENCAMINHAMENTOID
                                               ,EQUIPAMENTOUSADO
                                               ,DATARETORNO
                                               ,DATAENCAMINHAMENTOESCOLA
                                               ,PROTOCOLOCONSELHO
                                               ,MEDIDACONSELHOTUTELARID
                                               ,DATAENCAMINHAMENTOCONSELHO
                                               ,NOMECONSELHEIRO
                                               ,MEDIDAMPRJID
                                               ,DATAENCAMINHAMENTOMPRJ
                                               ,PROMOTOR
                                               ,ENCAMINHAMENTOSREALIZADO
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@ALUNO,
                                               @ANO,
                                               @SEQUENCIAL,
                                               @QUANTIDADEFALTAS,
                                               @DATAINICIOFALTAS, 
                                               @NUMEROFICAI, 
                                               @NUMEROFAMI,
                                               @DATACOMUNICACAO, 
                                               @OBSERVACAO, 
                                               @FORMACONTATOID_1, 
                                               @DATACONTATO_1, 
                                               @FORMACONTATOID_2, 
                                               @DATACONTATO_2, 
                                               @FORMACONTATOID_3, 
                                               @DATACONTATO_3, 
                                               @SITUACAOFAMILIARID, 
                                               @ALEGACAO, 
                                               @TIPOENCAMINHAMENTOID, 
                                               @EQUIPAMENTOUSADO, 
                                               @DATARETORNO, 
                                               @DATAENCAMINHAMENTOESCOLA, 
                                               @PROTOCOLOCONSELHO, 
                                               @MEDIDACONSELHOTUTELARID,
                                               @DATAENCAMINHAMENTOCONSELHO, 
                                               @NOMECONSELHEIRO,
                                               @MEDIDAMPRJID, 
                                               @DATAENCAMINHAMENTOMPRJ, 
                                               @PROMOTOR, 
                                               @ENCAMINHAMENTOSREALIZADO, 
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO ) 

                                         SELECT IDENT_CURRENT('Turma.NOTIFICACAO')  ";


                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, notificacao.Aluno);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, notificacao.Ano);
                contextQuery.Parameters.Add("@SEQUENCIAL", SqlDbType.Int, notificacao.Sequencial);
                contextQuery.Parameters.Add("@QUANTIDADEFALTAS", SqlDbType.Int, notificacao.QuantidadeFaltas);
                contextQuery.Parameters.Add("@DATAINICIOFALTAS", SqlDbType.DateTime, notificacao.DataInicioFaltas);
                contextQuery.Parameters.Add("@NUMEROFICAI", SqlDbType.VarChar, notificacao.NumeroFicai);
                contextQuery.Parameters.Add("@NUMEROFAMI", SqlDbType.VarChar, notificacao.NumeroFami);
                contextQuery.Parameters.Add("@DATACOMUNICACAO", SqlDbType.DateTime, notificacao.DataComunicacao);
                contextQuery.Parameters.Add("@OBSERVACAO", SqlDbType.VarChar, notificacao.Observacao);

                if (notificacao.FormaContatoId1 != null && notificacao.FormaContatoId1 > 0)
                {
                    contextQuery.Parameters.Add("@FORMACONTATOID_1", SqlDbType.Int, notificacao.FormaContatoId1);
                    contextQuery.Parameters.Add("@DATACONTATO_1", SqlDbType.DateTime, notificacao.DataContato1);
                }
                else
                {
                    contextQuery.Parameters.Add("@FORMACONTATOID_1", SqlDbType.Int, DBNull.Value);
                    contextQuery.Parameters.Add("@DATACONTATO_1", SqlDbType.DateTime, DBNull.Value);
                }

                if (notificacao.FormaContatoId2 != null && notificacao.FormaContatoId2 > 0)
                {
                    contextQuery.Parameters.Add("@FORMACONTATOID_2", SqlDbType.Int, notificacao.FormaContatoId2);
                    contextQuery.Parameters.Add("@DATACONTATO_2", SqlDbType.DateTime, notificacao.DataContato2);
                }
                else
                {
                    contextQuery.Parameters.Add("@FORMACONTATOID_2", SqlDbType.Int, DBNull.Value);
                    contextQuery.Parameters.Add("@DATACONTATO_2", SqlDbType.DateTime, DBNull.Value);
                }

                if (notificacao.FormaContatoId3 != null && notificacao.FormaContatoId3 > 0)
                {
                    contextQuery.Parameters.Add("@FORMACONTATOID_3", SqlDbType.Int, notificacao.FormaContatoId3);
                    contextQuery.Parameters.Add("@DATACONTATO_3", SqlDbType.DateTime, notificacao.DataContato3);
                }
                else
                {
                    contextQuery.Parameters.Add("@FORMACONTATOID_3", SqlDbType.Int, DBNull.Value);
                    contextQuery.Parameters.Add("@DATACONTATO_3", SqlDbType.DateTime, DBNull.Value);
                }

                if (notificacao.SituacaoFamiliarId != null && notificacao.SituacaoFamiliarId > 0)
                {
                    contextQuery.Parameters.Add("@SITUACAOFAMILIARID", SqlDbType.Int, notificacao.SituacaoFamiliarId);
                }
                else
                {
                    contextQuery.Parameters.Add("@SITUACAOFAMILIARID", SqlDbType.Int, DBNull.Value);
                }

                contextQuery.Parameters.Add("@ALEGACAO", SqlDbType.VarChar, notificacao.Alegacao);

                if (notificacao.TipoEncaminhamentoId != null && notificacao.TipoEncaminhamentoId > 0)
                {
                    contextQuery.Parameters.Add("@TIPOENCAMINHAMENTOID", SqlDbType.Int, notificacao.TipoEncaminhamentoId);
                }
                else
                {
                    contextQuery.Parameters.Add("@TIPOENCAMINHAMENTOID", SqlDbType.Int, DBNull.Value);
                }

                contextQuery.Parameters.Add("@EQUIPAMENTOUSADO", SqlDbType.VarChar, notificacao.EquipamentoUsado);

                if (notificacao.DataRetorno != null && notificacao.DataRetorno != DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATARETORNO", SqlDbType.DateTime, notificacao.DataRetorno);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATARETORNO", SqlDbType.DateTime, DBNull.Value);
                }

                if (notificacao.DataEncaminhamentoEscola != null && notificacao.DataEncaminhamentoEscola != DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAENCAMINHAMENTOESCOLA", SqlDbType.DateTime, notificacao.DataEncaminhamentoEscola);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAENCAMINHAMENTOESCOLA", SqlDbType.DateTime, DBNull.Value);
                }

                contextQuery.Parameters.Add("@PROTOCOLOCONSELHO", SqlDbType.VarChar, notificacao.ProtocoloConselho);

                if (notificacao.MedidasConselhoTutelarId != null && notificacao.MedidasConselhoTutelarId > 0)
                {
                    contextQuery.Parameters.Add("@MEDIDACONSELHOTUTELARID", SqlDbType.Int, notificacao.MedidasConselhoTutelarId);
                }
                else
                {
                    contextQuery.Parameters.Add("@MEDIDACONSELHOTUTELARID", SqlDbType.Int, DBNull.Value);
                }

                if (notificacao.DataEncaminhamentoConselho != null && notificacao.DataEncaminhamentoConselho != DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAENCAMINHAMENTOCONSELHO", SqlDbType.DateTime, notificacao.DataEncaminhamentoConselho);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAENCAMINHAMENTOCONSELHO", SqlDbType.DateTime, DBNull.Value);
                }

                contextQuery.Parameters.Add("@NOMECONSELHEIRO", SqlDbType.VarChar, notificacao.NomeConselheiro);

                if (notificacao.MedidaMPRJId != null && notificacao.MedidaMPRJId > 0)
                {
                    contextQuery.Parameters.Add("@MEDIDAMPRJID", SqlDbType.Int, notificacao.MedidaMPRJId);
                }
                else
                {
                    contextQuery.Parameters.Add("@MEDIDAMPRJID", SqlDbType.Int, DBNull.Value);
                }

                if (notificacao.DataEncaminhamentoMprj != null && notificacao.DataEncaminhamentoMprj != DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAENCAMINHAMENTOMPRJ", SqlDbType.DateTime, notificacao.DataEncaminhamentoMprj);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAENCAMINHAMENTOMPRJ", SqlDbType.DateTime, DBNull.Value);
                }

                contextQuery.Parameters.Add("@PROMOTOR", SqlDbType.VarChar, notificacao.Promotor);
                contextQuery.Parameters.Add("@ENCAMINHAMENTOSREALIZADO", SqlDbType.VarChar, notificacao.EncaminhamentosRealizado);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, notificacao.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                notificacao.NotificacaoId = Convert.ToInt32(ctx.GetReturnValue(contextQuery));
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

        public void Atualiza(Entidades.Notificacao notificacao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE Turma.NOTIFICACAO 
                                           SET DATACOMUNICACAO  = @DATACOMUNICACAO, 
                                               OBSERVACAO  = @OBSERVACAO, 
                                               FORMACONTATOID_1  = @FORMACONTATOID_1, 
                                               DATACONTATO_1  = @DATACONTATO_1, 
                                               FORMACONTATOID_2  = @FORMACONTATOID_2, 
                                               DATACONTATO_2  = @DATACONTATO_2, 
                                               FORMACONTATOID_3  = @FORMACONTATOID_3, 
                                               DATACONTATO_3  = @DATACONTATO_3, 
                                               SITUACAOFAMILIARID  = @SITUACAOFAMILIARID, 
                                               ALEGACAO  = @ALEGACAO, 
                                               TIPOENCAMINHAMENTOID  = @TIPOENCAMINHAMENTOID, 
                                               EQUIPAMENTOUSADO  = @EQUIPAMENTOUSADO, 
                                               DATARETORNO  = @DATARETORNO, 
                                               DATAENCAMINHAMENTOESCOLA  = @DATAENCAMINHAMENTOESCOLA, 
                                               PROTOCOLOCONSELHO  = @PROTOCOLOCONSELHO, 
                                               MEDIDACONSELHOTUTELARID  = @MEDIDACONSELHOTUTELARID, 
                                               DATAENCAMINHAMENTOCONSELHO  = @DATAENCAMINHAMENTOCONSELHO, 
                                               NOMECONSELHEIRO  = @NOMECONSELHEIRO, 
                                               MEDIDAMPRJID  = @MEDIDAMPRJID, 
                                               DATAENCAMINHAMENTOMPRJ  = @DATAENCAMINHAMENTOMPRJ, 
                                               ENCAMINHAMENTOSREALIZADO  = @ENCAMINHAMENTOSREALIZADO, 
                                               PROMOTOR  = @PROMOTOR, 
                                               USUARIOID  = @USUARIOID, 
                                               DATAALTERACAO  = @DATAALTERACAO
                                         WHERE NOTIFICACAOID = @NOTIFICACAOID  ";

                contextQuery.Parameters.Add("@NOTIFICACAOID", SqlDbType.Int, notificacao.NotificacaoId);
                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, notificacao.Aluno);
                contextQuery.Parameters.Add("@QUANTIDADEFALTAS", SqlDbType.Int, notificacao.QuantidadeFaltas);
                contextQuery.Parameters.Add("@DATAINICIOFALTAS", SqlDbType.DateTime, notificacao.DataInicioFaltas);
                contextQuery.Parameters.Add("@DATACOMUNICACAO", SqlDbType.DateTime, notificacao.DataComunicacao);
                contextQuery.Parameters.Add("@OBSERVACAO", SqlDbType.VarChar, notificacao.Observacao);

                if (notificacao.FormaContatoId1 != null && notificacao.FormaContatoId1 > 0)
                {
                    contextQuery.Parameters.Add("@FORMACONTATOID_1", SqlDbType.Int, notificacao.FormaContatoId1);
                    contextQuery.Parameters.Add("@DATACONTATO_1", SqlDbType.DateTime, notificacao.DataContato1);
                }
                else
                {
                    contextQuery.Parameters.Add("@FORMACONTATOID_1", SqlDbType.Int, DBNull.Value);
                    contextQuery.Parameters.Add("@DATACONTATO_1", SqlDbType.DateTime, DBNull.Value);
                }

                if (notificacao.FormaContatoId2 != null && notificacao.FormaContatoId2 > 0)
                {
                    contextQuery.Parameters.Add("@FORMACONTATOID_2", SqlDbType.Int, notificacao.FormaContatoId2);
                    contextQuery.Parameters.Add("@DATACONTATO_2", SqlDbType.DateTime, notificacao.DataContato2);
                }
                else
                {
                    contextQuery.Parameters.Add("@FORMACONTATOID_2", SqlDbType.Int, DBNull.Value);
                    contextQuery.Parameters.Add("@DATACONTATO_2", SqlDbType.DateTime, DBNull.Value);
                }

                if (notificacao.FormaContatoId3 != null && notificacao.FormaContatoId3 > 0)
                {
                    contextQuery.Parameters.Add("@FORMACONTATOID_3", SqlDbType.Int, notificacao.FormaContatoId3);
                    contextQuery.Parameters.Add("@DATACONTATO_3", SqlDbType.DateTime, notificacao.DataContato3);
                }
                else
                {
                    contextQuery.Parameters.Add("@FORMACONTATOID_3", SqlDbType.Int, DBNull.Value);
                    contextQuery.Parameters.Add("@DATACONTATO_3", SqlDbType.DateTime, DBNull.Value);
                }

                if (notificacao.SituacaoFamiliarId != null && notificacao.SituacaoFamiliarId > 0)
                {
                    contextQuery.Parameters.Add("@SITUACAOFAMILIARID", SqlDbType.Int, notificacao.SituacaoFamiliarId);
                }
                else
                {
                    contextQuery.Parameters.Add("@SITUACAOFAMILIARID", SqlDbType.Int, DBNull.Value);
                }

                contextQuery.Parameters.Add("@ALEGACAO", SqlDbType.VarChar, notificacao.Alegacao);

                if (notificacao.TipoEncaminhamentoId != null && notificacao.TipoEncaminhamentoId > 0)
                {
                    contextQuery.Parameters.Add("@TIPOENCAMINHAMENTOID", SqlDbType.Int, notificacao.TipoEncaminhamentoId);
                }
                else
                {
                    contextQuery.Parameters.Add("@TIPOENCAMINHAMENTOID", SqlDbType.Int, DBNull.Value);
                }

                contextQuery.Parameters.Add("@EQUIPAMENTOUSADO", SqlDbType.VarChar, notificacao.EquipamentoUsado);

                if (notificacao.DataRetorno != null && notificacao.DataRetorno != DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATARETORNO", SqlDbType.DateTime, notificacao.DataRetorno);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATARETORNO", SqlDbType.DateTime, DBNull.Value);
                }

                if (notificacao.DataEncaminhamentoEscola != null && notificacao.DataEncaminhamentoEscola != DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAENCAMINHAMENTOESCOLA", SqlDbType.DateTime, notificacao.DataEncaminhamentoEscola);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAENCAMINHAMENTOESCOLA", SqlDbType.DateTime, DBNull.Value);
                }

                contextQuery.Parameters.Add("@PROTOCOLOCONSELHO", SqlDbType.VarChar, notificacao.ProtocoloConselho);

                if (notificacao.MedidasConselhoTutelarId != null && notificacao.MedidasConselhoTutelarId > 0)
                {
                    contextQuery.Parameters.Add("@MEDIDACONSELHOTUTELARID", SqlDbType.Int, notificacao.MedidasConselhoTutelarId);
                }
                else
                {
                    contextQuery.Parameters.Add("@MEDIDACONSELHOTUTELARID", SqlDbType.Int, DBNull.Value);
                }

                if (notificacao.DataEncaminhamentoConselho != null && notificacao.DataEncaminhamentoConselho != DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAENCAMINHAMENTOCONSELHO", SqlDbType.DateTime, notificacao.DataEncaminhamentoConselho);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAENCAMINHAMENTOCONSELHO", SqlDbType.DateTime, DBNull.Value);
                }

                contextQuery.Parameters.Add("@NOMECONSELHEIRO", SqlDbType.VarChar, notificacao.NomeConselheiro);

                if (notificacao.MedidaMPRJId != null && notificacao.MedidaMPRJId > 0)
                {
                    contextQuery.Parameters.Add("@MEDIDAMPRJID", SqlDbType.Int, notificacao.MedidaMPRJId);
                }
                else
                {
                    contextQuery.Parameters.Add("@MEDIDAMPRJID", SqlDbType.Int, DBNull.Value);
                }

                if (notificacao.DataEncaminhamentoMprj != null && notificacao.DataEncaminhamentoMprj != DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAENCAMINHAMENTOMPRJ", SqlDbType.DateTime, notificacao.DataEncaminhamentoMprj);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAENCAMINHAMENTOMPRJ", SqlDbType.DateTime, DBNull.Value);
                }

                contextQuery.Parameters.Add("@PROMOTOR", SqlDbType.VarChar, notificacao.Promotor);
                contextQuery.Parameters.Add("@ENCAMINHAMENTOSREALIZADO", SqlDbType.VarChar, notificacao.EncaminhamentosRealizado);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, notificacao.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

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
    }
}
