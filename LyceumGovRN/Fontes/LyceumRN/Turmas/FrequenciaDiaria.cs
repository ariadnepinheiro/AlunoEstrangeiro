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
    public class FrequenciaDiaria
    {
        public DataTable ConsultarMatriculas(String disciplina, String turma, decimal ano, decimal periodo, DateTime dataFrequencia)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT num_chamada,
											T.nome_compl,
											t.pre_nome_social, 
											t.aluno,
											 Case when situacao = 'Matriculado' AND suspenso = 0 then 'Matriculado'				 
                                                  when situacao = 'Matriculado' AND suspenso = 1  then 'Matrícula em Suspensão'				 
												  when situacao = 'Cancelado' AND TURMA_TRANSF <> '' THEN 'Transferido para ' + TURMA_TRANSF
                                                  when situacao = 'Cancelado' AND sit_aluno = 'Suspenso' then 'Suspenso'
												  else situacao
											 end as sit_matricula,
											t.justificativa,
											t.disciplina ,
											t.turma ,
											t.ano ,
											t.semestre ,
											t.faltas,
											t.Tempos,
	                                        CONVERT(VARCHAR, T.FALTAS) + '/' + CONVERT(VARCHAR, T.TEMPOS) AS faltas_tempos,
	                                        case 
		                                        when justificativa is not null then 'N'
		                                        when faltas = 0 then 'S'		
		                                        else 'N'
	                                        end frequencia,
                                            suspenso
                                        FROM (SELECT DISTINCT m.NUM_CHAMADA,
														nome_compl ,
                                                        PE.pre_nome_social,
                                                        m.aluno ,
														m.sit_matricula as situacao,
                                                        isnull(a.suspenso,0) as suspenso,
                                                        a.sit_aluno,
                                                        CASE
					                                        WHEN AD.TIPO IS NOT NULL THEN 'ATENDIMENTO ' + UPPER(AD.TIPO)
					                                        WHEN J.DESCRICAO IS NOT NULL THEN UPPER(J.DESCRICAO)
                                                        END justificativa,
                                                        m.disciplina ,
                                                        m.turma ,
                                                        m.ano ,
                                                        m.semestre ,  
                                                        m.dt_matricula,                                                                   
				                                        (select count(1)
					                                        from Turma.FREQUENCIADIARIA FT
						                                        INNER JOIN Turma.FREQUENCIADIARIA_ALUNOFALTA FA ON FT.FREQUENCIADIARIAID = FA.FREQUENCIADIARIAID
					                                        WHERE FA.ALUNO = A.ALUNO
						                                        AND FT.DATAFREQUENCIA = @DATAFREQUENCIA
						                                        AND FT.ANO = M.ANO
						                                        AND FT.SEMESTRE = M.SEMESTRE
						                                        AND FT.TURMA = M.TURMA
						                                        AND FT.DISCIPLINA = M.DISCIPLINA
						                                        AND FT.DIA_SEMANA = DATEPART(WEEKDAY, @DATAFREQUENCIA)
						                                        AND FA.ATIVO = 1) AS faltas,
				                                        CASE
					                                        WHEN M.SIT_MATRICULA <> 'Matriculado' then 0
				                                        else (SELECT COUNT(DISTINCT H.AULA)
                                                                                FROM   LY_AULA_DOCENTE AD
                                                                                       INNER JOIN LY_HOR_AULA H
                                                                                         ON AD.TURNO = H.TURNO
                                                                                            AND AD.FACULDADE = H.FACULDADE
                                                                                            AND AD.DIA_SEMANA = H.DIA_SEMANA
                                                                                            AND AD.AULA = H.AULA
                                                                                            AND AD.DISCIPLINA = H.DISCIPLINA
                                                                                            AND AD.TURMA = H.TURMA
                                                                                            AND AD.ANO = H.ANO
                                                                                            AND AD.SEMESTRE = H.SEMESTRE 
                                                                                WHERE AD.DISCIPLINA = M.DISCIPLINA
                                                                                        AND AD.TURMA = M.TURMA
                                                                                        AND AD.ANO = M.ANO
                                                                                        AND AD.SEMESTRE = M.SEMESTRE
                                                                                        AND AD.DIA_SEMANA =  DATEPART(WEEKDAY, @DATAFREQUENCIA)
		                                                                                AND AD.DATA_INICIO <> AD.DATA_FIM
                                                                                        AND @DATAFREQUENCIA BETWEEN  AD.DATA_INICIO AND AD.DATA_FIM) 
				                                        end Tempos,
														(select TOP 1 m2.turma 
															 FROM LY_MATRICULA M2 
																INNER JOIN LY_TURMA T2 ON M2.DISCIPLINA = T2.DISCIPLINA
																										 AND M2.TURMA = T2.TURMA
																										 AND M2.ANO = T2.ANO
																										 AND M2.SEMESTRE = T2.SEMESTRE
															 WHERE M2.ANO = @ANO 
																 AND M2.SEMESTRE = @SEMESTRE 
																 AND M2.TURMA <> @TURMA 
																 and M2.ALUNO = M.ALUNO
																 AND M2.SIT_MATRICULA = 'MATRICULADO'
																 AND ( M2.DEPENDENCIA IS NULL OR M2.DEPENDENCIA = 'N')
																 AND ( M2.CONCOMITANTE IS NULL OR M2.CONCOMITANTE = 'N')
																 AND ( M2.EDUC_ESPECIAL IS NULL OR M2.EDUC_ESPECIAL = 'N')
																 AND ( M2.MAIS_EDUCACAO IS NULL OR M2.MAIS_EDUCACAO = 'N')
																 AND T2.OPTATIVAREFORCO = 'N'
																 AND ISNULL(T2.ELETIVA,'N') = 'N' ) AS TURMA_TRANSF
                                                FROM    ly_matricula m WITH ( NOLOCK )
                                                        INNER JOIN ly_aluno a WITH ( NOLOCK ) ON m.aluno = a.aluno
				                                        INNER JOIN LY_TURMA T (NOLOCK) ON M.ANO = T.ANO AND M.SEMESTRE = T.SEMESTRE AND M.TURMA = T.TURMA AND M.DISCIPLINA = T.DISCIPLINA
                                                        INNER JOIN LY_PESSOA PE (NOLOCK)  ON PE.PESSOA = A.PESSOA
				                                        LEFT JOIN LY_ALUNO_LICENCA AL (NOLOCK) ON A.ALUNO = AL.ALUNO
														                                        AND @DATAFREQUENCIA BETWEEN AL.DT_INICIO AND AL.DT_FIM
				                                        LEFT JOIN RecursosHumanos.JUSTIFICATIVAFALTA J (NOLOCK) ON AL.JUSTIFICATIVAFALTAID = J.JUSTIFICATIVAFALTAID
				                                        LEFT JOIN RecursosHumanos.ATENDIMENTOOUTROESPACO AD (NOLOCK) ON AD.ALUNO = A.ALUNO
														                                        AND @DATAFREQUENCIA BETWEEN AD.DATAINICIO AND AD.DATAFIM								
                                                WHERE   m.disciplina = @DISCIPLINA
                                                        AND m.turma = @TURMA
				                                        AND isnull(M.DEPENDENCIA,'N') = 'N'
                                                        AND m.ano = @ANO
                                                        AND m.semestre = @SEMESTRE ) T
                                        ORDER BY isnull(num_chamada,99999), dt_matricula ";

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);
                contextQuery.Parameters.Add("@DATAFREQUENCIA", dataFrequencia.Date);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
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
                ctx.Dispose();
            }

            return dt;
        }

        public ValidacaoDados Valida(RN.DTOs.DadosFrequenciaDiaria dadosFrequenciaDiaria)
        {
            List<string> mensagens = new List<string>();
            RN.SubperiodoLetivo rnSubperiodoLetivo = new SubperiodoLetivo();
            DiaSemAula rnDiaSemAula = new DiaSemAula();
            CompetenciaHabilidadeItem rnCompetenciaHabilidadeItem = new CompetenciaHabilidadeItem();
            DataContext contexto = null;
            HorariosDocente rnHorariosDocentes = new HorariosDocente();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosFrequenciaDiaria == null)
            {
                return validacaoDados;
            }

            if (dadosFrequenciaDiaria.Ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (dadosFrequenciaDiaria.Semestre < 0)
            {
                mensagens.Add("Campo PERIODO é obrigatório.");
            }

            if (dadosFrequenciaDiaria.Turma.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TURMA é obrigatório.");
            }

            if (dadosFrequenciaDiaria.Disciplina.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DISCIPLINA é obrigatório.");
            }

            if (dadosFrequenciaDiaria.Faculdade.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE DE ENSINO é obrigatório.");
            }

            if (dadosFrequenciaDiaria.Turno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TURNO é obrigatório.");
            }

            if (dadosFrequenciaDiaria.DataFrequencia == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DA FREQUENCIA é obrigatório.");
            }
            else
            {
                dadosFrequenciaDiaria.DiaSemana = ((int)dadosFrequenciaDiaria.DataFrequencia.Date.DayOfWeek + 1);

                if (dadosFrequenciaDiaria.DataFrequencia > DateTime.Now.Date)
                {
                    mensagens.Add("Campo DATA DA FREQUENCIA não pode ser maior que a data atual.");
                }
            }

            if (dadosFrequenciaDiaria.PlanoAula.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo REGISTRO DE AULA é obrigatório.");
            }
            else if (dadosFrequenciaDiaria.PlanoAula.Length > 5000)
            {
                mensagens.Add("Campo REGISTRO DE AULA deve ter no máximo 5000 caracteres.");
            }

            if (dadosFrequenciaDiaria.UsuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSAVEL é obrigatório.");
            }

            if (dadosFrequenciaDiaria.CurriculosItemId == null)
            {
                mensagens.Add("Não existe currículos cadastrado para esta disciplina. Favor verificar junto a área responsável.");
            }
            else
            {
                if (dadosFrequenciaDiaria.CurriculosItemId.Count == 0)
                {
                    mensagens.Add("Marque ao menos uma competência/habilidade trabalhada em aula com essa turma");
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Buacar e validar se existe superido Letivo
                    dadosFrequenciaDiaria.Bimestre = rnSubperiodoLetivo.ObtemSubperiodoPor(contexto, dadosFrequenciaDiaria.Ano, dadosFrequenciaDiaria.Semestre, dadosFrequenciaDiaria.DataFrequencia);

                    if (dadosFrequenciaDiaria.Bimestre == 0)
                    {
                        mensagens.Add("Não foi encontrado SUBPERIODO LETIVO cadastrado para esta data.");
                    }

                    //Busca possiveis datas para lançamento de frequencia
                    DataTable datas = rnHorariosDocentes.ListaDataFrequenciaPor(contexto, dadosFrequenciaDiaria.Ano, dadosFrequenciaDiaria.Semestre, dadosFrequenciaDiaria.Turma, dadosFrequenciaDiaria.Disciplina);

                    //Verifica se a data Esta na lista de possiveis
                    DataRow[] data = datas.Select("DATA = '" + dadosFrequenciaDiaria.DataFrequencia.ToString("yyyy-MM-dd") + "'");
                    if (data.Length == 0)
                    {
                        mensagens.Add("Não é possivel lançar frequência para a disciplina / turma nesta DATA.");
                    }
                    else
                    {
                        //Buscar todos os horarios possiveis
                        DataTable horarios = rnHorariosDocentes.ListaHorariosPor(contexto, dadosFrequenciaDiaria.Ano, dadosFrequenciaDiaria.Semestre, dadosFrequenciaDiaria.Turma, dadosFrequenciaDiaria.Disciplina, dadosFrequenciaDiaria.DiaSemana, dadosFrequenciaDiaria.DataFrequencia);

                        //Verifica se foi selecionado algum tempo de aula
                        if (dadosFrequenciaDiaria.Aulas.Count() == 0)
                        {
                            //Caso não tenha colocar todos os horarios
                            dadosFrequenciaDiaria.Aulas = new List<int>();
                            foreach (DataRow horario in horarios.Rows)
                            {
                                dadosFrequenciaDiaria.Aulas.Add((int)horario["AULA"]);
                            }
                        }
                        else if (dadosFrequenciaDiaria.Aulas.Count() != horarios.Rows.Count)
                        {
                            mensagens.Add("A quantidade de HORÁRIO não está correta para esta DATA.");
                        }

                        foreach (int aula in dadosFrequenciaDiaria.Aulas)
                        {
                            //Verifica se o horario / Esta na lista de possiveis para o dia
                            DataRow[] horario = horarios.Select("AULA = '" + aula.ToString() + "'");
                            if (horario.Length == 0)
                            {
                                mensagens.Add("Este HORÁRIO não foi encontrado para esta DATA.");
                            }
                        }

                        //Verifica se algum horario de aluno com falta não esta na lista de aulas
                        foreach (int aula in dadosFrequenciaDiaria.AlunosComFalta.Select(x => x.Aula).Distinct())
                        {
                            if (horarios.Select("AULA = '" + aula.ToString() + "'").Length == 0 || dadosFrequenciaDiaria.Aulas.Where(x => x == aula).Count() == 0)
                            {
                                mensagens.Add("existem HORÁRIOS de Alunos que não foram encontrados para esta DATA.");
                            }
                        }
                    }

                    if (dadosFrequenciaDiaria.CurriculosItemId.Count > 0)
                    {
                        //Caso o usuário selecione itens de uma coluna diferente da anteriormente selecionada, o sistema deve desmarcar todos os itens anteriormente selecionados e manter apenas a seleção dos novos itens da nova coluna desejada
                        //Verifica se existem itens de tipos diferentes
                        if (rnCompetenciaHabilidadeItem.ObtemQuantidadeTipoPor(contexto, dadosFrequenciaDiaria.CurriculosItemId) > 1)
                        {
                            mensagens.Add("As competências/habilidades trabalhadas em aula com essa turma devem ser do mesmo TIPO.");
                        }
                    }

                    //Busca data de reposição - caso seja dia é um dia sem aula
                    DateTime dataReposicao = rnDiaSemAula.ObtemDataReposicaoPor(contexto, dadosFrequenciaDiaria.DataFrequencia, dadosFrequenciaDiaria.Faculdade);
                    dadosFrequenciaDiaria.DataReposicao = null;

                    if (dataReposicao != DateTime.MinValue)
                    {
                        dadosFrequenciaDiaria.DataReposicao = dataReposicao;

                        if (DateTime.Now < dataReposicao)
                        {
                            mensagens.Add(string.Format("O dia Sem Aula ficará indisponível para lançamento. Favor retornar no dia cadastrado para a reposição, {0}.", dataReposicao.ToString("dd/MM/yyyy")));
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

        public void Salva(RN.DTOs.DadosFrequenciaDiaria dadosFrequenciaDiaria)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            TceCompetenciaHabilidadeDocente competenciaHabilidadeDocente = new TceCompetenciaHabilidadeDocente();
            Aluno rnAluno = new Aluno();
            Turmas.HistoricoSuspensao rnHistoricoSuspensao = new HistoricoSuspensao();
            CompetenciaHabilidadeDocente rnCompetenciaHabilidadeDocente = new CompetenciaHabilidadeDocente();
            Entidades.FrequenciaPlanoDeAula frequenciaPlanoDeAula = new Techne.Lyceum.RN.Turmas.Entidades.FrequenciaPlanoDeAula();
            Entidades.FrequenciaDiaria frequenciaDiaria = new Techne.Lyceum.RN.Turmas.Entidades.FrequenciaDiaria();
            FrequenciaPlanoDeAula rnFrequenciaPlanoDeAula = new FrequenciaPlanoDeAula();
            Entidades.FrequenciaDiariaAlunoFalta frequenciaDiariaAlunoFalta = new Techne.Lyceum.RN.Turmas.Entidades.FrequenciaDiariaAlunoFalta();
            FrequenciaDiariaAlunoFalta rnFrequenciaDiariaAlunoFalta = new FrequenciaDiariaAlunoFalta();

            try
            {
                //Busca plano de aula
                frequenciaPlanoDeAula = rnFrequenciaPlanoDeAula.ObtemPor(contexto, dadosFrequenciaDiaria.Ano, dadosFrequenciaDiaria.Semestre, dadosFrequenciaDiaria.Turma, dadosFrequenciaDiaria.Disciplina, dadosFrequenciaDiaria.DataFrequencia);

                //Verifica se plano de aula existe
                if (frequenciaPlanoDeAula.FrequenciaPlanoDeAulaId == 0)
                {
                    //Monta Plano de Aula
                    frequenciaPlanoDeAula = new Techne.Lyceum.RN.Turmas.Entidades.FrequenciaPlanoDeAula();
                    frequenciaPlanoDeAula.Ano = dadosFrequenciaDiaria.Ano;
                    frequenciaPlanoDeAula.Semestre = dadosFrequenciaDiaria.Semestre;
                    frequenciaPlanoDeAula.Turma = dadosFrequenciaDiaria.Turma;
                    frequenciaPlanoDeAula.Disciplina = dadosFrequenciaDiaria.Disciplina;
                    frequenciaPlanoDeAula.DataFrequencia = dadosFrequenciaDiaria.DataFrequencia;
                    frequenciaPlanoDeAula.PlanoAula = dadosFrequenciaDiaria.PlanoAula;
                    frequenciaPlanoDeAula.UsuarioId = dadosFrequenciaDiaria.UsuarioResponsavel;

                    //Insere Plano de Aula
                    rnFrequenciaPlanoDeAula.Insere(contexto, frequenciaPlanoDeAula);
                }
                else if (frequenciaPlanoDeAula.PlanoAula != dadosFrequenciaDiaria.PlanoAula)
                {
                    frequenciaPlanoDeAula.PlanoAula = dadosFrequenciaDiaria.PlanoAula;
                    frequenciaPlanoDeAula.UsuarioId = dadosFrequenciaDiaria.UsuarioResponsavel;

                    //Atualiza Plano de Aula
                    rnFrequenciaPlanoDeAula.Atualiza(contexto, frequenciaPlanoDeAula);
                }

                //Joga todos que estejam salvos na base e tenham sido retirados na tela no Log
                rnCompetenciaHabilidadeDocente.GeraLog(contexto, dadosFrequenciaDiaria.Turma, dadosFrequenciaDiaria.Disciplina, dadosFrequenciaDiaria.DataFrequencia, dadosFrequenciaDiaria.CurriculosItemId);

                //Deleta itens do curriculo minimo que estejam salvos na base e tenham sido retirados na tela
                rnCompetenciaHabilidadeDocente.Remove(contexto, dadosFrequenciaDiaria.Turma, dadosFrequenciaDiaria.Disciplina, dadosFrequenciaDiaria.DataFrequencia, dadosFrequenciaDiaria.CurriculosItemId);

                //Salva Curriculo Minimo
                foreach (var itemId in dadosFrequenciaDiaria.CurriculosItemId)
                {
                    //Verifica se aquele item já foi lançado
                    if (!rnCompetenciaHabilidadeDocente.ExistePor(contexto, itemId, dadosFrequenciaDiaria.Turma, dadosFrequenciaDiaria.Disciplina, dadosFrequenciaDiaria.DataFrequencia))
                    {
                        //Monta curriculo Minimo
                        competenciaHabilidadeDocente = new TceCompetenciaHabilidadeDocente();
                        competenciaHabilidadeDocente.IdCompetenciaHabilidadeItem = itemId;
                        competenciaHabilidadeDocente.Ano = dadosFrequenciaDiaria.Ano;
                        competenciaHabilidadeDocente.Periodo = dadosFrequenciaDiaria.Semestre;
                        competenciaHabilidadeDocente.Subperiodo = dadosFrequenciaDiaria.Bimestre;
                        competenciaHabilidadeDocente.Turma = dadosFrequenciaDiaria.Turma;
                        competenciaHabilidadeDocente.Disciplina = dadosFrequenciaDiaria.Disciplina;
                        competenciaHabilidadeDocente.DataFrequencia = dadosFrequenciaDiaria.DataFrequencia;
                        competenciaHabilidadeDocente.UsuarioId = dadosFrequenciaDiaria.UsuarioResponsavel;

                        //Insere      
                        rnCompetenciaHabilidadeDocente.Insere(contexto, competenciaHabilidadeDocente);
                    }
                }

                //Reativa alunos suspensos que tiveram presença
                foreach (string aluno in dadosFrequenciaDiaria.AlunosSuspensos)
                {
                    //Atualiza dados da tabela de controle
                    rnHistoricoSuspensao.ReativaMatricula(contexto, aluno, dadosFrequenciaDiaria.UsuarioResponsavel);                    

                    //Atualiza aluno
                    rnAluno.AtivaMatriculaEmSuspensao(contexto, aluno);
                }

                foreach (int aula in dadosFrequenciaDiaria.Aulas)
                {
                    //Monta entidade
                    frequenciaDiaria = new Techne.Lyceum.RN.Turmas.Entidades.FrequenciaDiaria();
                    frequenciaDiaria.Ano = dadosFrequenciaDiaria.Ano;
                    frequenciaDiaria.Semestre = dadosFrequenciaDiaria.Semestre;
                    frequenciaDiaria.Turma = dadosFrequenciaDiaria.Turma;
                    frequenciaDiaria.Disciplina = dadosFrequenciaDiaria.Disciplina;
                    frequenciaDiaria.Aula = aula;
                    frequenciaDiaria.Faculdade = dadosFrequenciaDiaria.Faculdade;
                    frequenciaDiaria.DiaSemana = dadosFrequenciaDiaria.DiaSemana;
                    frequenciaDiaria.Turno = dadosFrequenciaDiaria.Turno;
                    frequenciaDiaria.DataFrequencia = dadosFrequenciaDiaria.DataFrequencia;
                    frequenciaDiaria.NumFuncLancamento = null;
                    frequenciaDiaria.UsuarioId = dadosFrequenciaDiaria.UsuarioResponsavel;
                    frequenciaDiaria.DataReposicao = dadosFrequenciaDiaria.DataReposicao;

                    //Verifica se a data / aula já teve lançamento    
                    int id = this.ObtemFrequenciaDiariaIdPor(dadosFrequenciaDiaria.DataFrequencia, dadosFrequenciaDiaria.Turno, dadosFrequenciaDiaria.Faculdade, dadosFrequenciaDiaria.DiaSemana, aula, dadosFrequenciaDiaria.Disciplina, dadosFrequenciaDiaria.Turma, dadosFrequenciaDiaria.Ano, dadosFrequenciaDiaria.Semestre);

                    if (id == 0)
                    {
                        //Insere 
                        this.Insere(contexto, frequenciaDiaria);
                    }
                    else
                    {
                        frequenciaDiaria.FrequenciaDiariaId = id;

                        //Atualiza dados frequencia
                        this.Atualiza(contexto, frequenciaDiaria);

                        List<string> listaAlunos = dadosFrequenciaDiaria.AlunosComFalta.Where(x => x.Aula == aula).Select(y => y.Aluno).Distinct().ToList();

                        //Desativa falta de alunos que não estejam na lista para a aula
                        rnFrequenciaDiariaAlunoFalta.DesativaPor(contexto, frequenciaDiaria.FrequenciaDiariaId, listaAlunos, frequenciaDiaria.UsuarioId);
                    }

                    foreach (DadosFrequenciaDiaria.DadosAlunoFalta aluno in dadosFrequenciaDiaria.AlunosComFalta.Where(x => x.Aula == aula).ToList())
                    {
                        //Monta entidade
                        frequenciaDiariaAlunoFalta = new Techne.Lyceum.RN.Turmas.Entidades.FrequenciaDiariaAlunoFalta();
                        frequenciaDiariaAlunoFalta.FrequenciaDiariaId = frequenciaDiaria.FrequenciaDiariaId;
                        frequenciaDiariaAlunoFalta.Aluno = aluno.Aluno;
                        frequenciaDiariaAlunoFalta.Ativo = true;
                        frequenciaDiariaAlunoFalta.UsuarioId = dadosFrequenciaDiaria.UsuarioResponsavel;

                        //Verifica se o aluno já possui falta na aula, caso nao possua insere
                        if (!rnFrequenciaDiariaAlunoFalta.PossuiPor(contexto, frequenciaDiaria.FrequenciaDiariaId, aluno.Aluno))
                        {
                            //Insere
                            rnFrequenciaDiariaAlunoFalta.Insere(contexto, frequenciaDiariaAlunoFalta);
                        }
                        else
                        {
                            //Ativa a falta caso exista
                            rnFrequenciaDiariaAlunoFalta.AtivaPor(contexto, frequenciaDiariaAlunoFalta);
                        }
                    }
                }
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

        public int ObtemFrequenciaDiariaIdPor(DateTime dataFrequencia, string turno, string faculdade, int diaSemana, int aula, string disciplina, string turma, int ano, int semestre)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT FREQUENCIADIARIAID 
                                    FROM Turma.FREQUENCIADIARIA
                                    WHERE DATAFREQUENCIA = @DATAFREQUENCIA
	                                    AND TURNO = @TURNO
	                                    AND FACULDADE = @FACULDADE
	                                    AND DIA_SEMANA = @DIA_SEMANA
	                                    AND AULA = @AULA
	                                    AND DISCIPLINA = @DISCIPLINA
	                                    AND TURMA = @TURMA
	                                    AND ANO = @ANO
	                                    AND SEMESTRE = @SEMESTRE ";

                contextQuery.Parameters.Add("@DATAFREQUENCIA", SqlDbType.DateTime, dataFrequencia.Date);
                contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, turno);
                contextQuery.Parameters.Add("@FACULDADE", SqlDbType.VarChar, faculdade);
                contextQuery.Parameters.Add("@DIA_SEMANA", SqlDbType.Int, diaSemana);
                contextQuery.Parameters.Add("@AULA", SqlDbType.Int, aula);
                contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, disciplina);
                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, semestre);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["FREQUENCIADIARIAID"]);
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

        private void Insere(DataContext contexto, Entidades.FrequenciaDiaria frequenciaDiaria)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Turma.FREQUENCIADIARIA
                                               (ANO
                                               ,SEMESTRE
                                               ,TURMA
                                               ,DISCIPLINA
                                               ,AULA
                                               ,FACULDADE
                                               ,DIA_SEMANA
                                               ,TURNO
                                               ,DATAFREQUENCIA
                                               ,DATAREPOSICAO
                                               ,NUMFUNCLANCAMENTO
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@ANO, 
                                               @SEMESTRE,
                                               @TURMA, 
                                               @DISCIPLINA, 
                                               @AULA, 
                                               @FACULDADE,
                                               @DIA_SEMANA, 
                                               @TURNO, 
                                               @DATAFREQUENCIA, 
                                               @DATAREPOSICAO,
                                               @NUMFUNCLANCAMENTO, 
                                               @USUARIOID,
                                               @DATACADASTRO, 
                                               @DATAALTERACAO ) 

                                     SELECT IDENT_CURRENT('Turma.FREQUENCIADIARIA') ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Decimal, frequenciaDiaria.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Decimal, frequenciaDiaria.Semestre);
            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, frequenciaDiaria.Turma);
            contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, frequenciaDiaria.Disciplina);
            contextQuery.Parameters.Add("@AULA", SqlDbType.Decimal, frequenciaDiaria.Aula);
            contextQuery.Parameters.Add("@FACULDADE", SqlDbType.VarChar, frequenciaDiaria.Faculdade);
            contextQuery.Parameters.Add("@DIA_SEMANA", SqlDbType.Decimal, frequenciaDiaria.DiaSemana);
            contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, frequenciaDiaria.Turno);
            contextQuery.Parameters.Add("@DATAFREQUENCIA", SqlDbType.DateTime, frequenciaDiaria.DataFrequencia.Date);

            if (frequenciaDiaria.DataReposicao == null || frequenciaDiaria.DataReposicao == DateTime.MinValue)
            {
                contextQuery.Parameters.Add("@DATAREPOSICAO", SqlDbType.DateTime, DBNull.Value);
            }
            else
            {
                contextQuery.Parameters.Add("@DATAREPOSICAO", SqlDbType.DateTime, Convert.ToDateTime(frequenciaDiaria.DataReposicao).Date);
            }

            contextQuery.Parameters.Add("@NUMFUNCLANCAMENTO", SqlDbType.Decimal, DBNull.Value);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, frequenciaDiaria.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            frequenciaDiaria.FrequenciaDiariaId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        private void Atualiza(DataContext contexto, Entidades.FrequenciaDiaria frequenciaDiaria)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Turma.FREQUENCIADIARIA
                                        SET NUMFUNCLANCAMENTO = @NUMFUNCLANCAMENTO,
                                            DATAREPOSICAO = @DATAREPOSICAO,
                                            USUARIOID = @USUARIOID,
                                            DATAALTERACAO = @DATAALTERACAO
                                      WHERE FREQUENCIADIARIAID = @FREQUENCIADIARIAID ";

            contextQuery.Parameters.Add("@FREQUENCIADIARIAID", SqlDbType.Int, frequenciaDiaria.FrequenciaDiariaId);
            contextQuery.Parameters.Add("@NUMFUNCLANCAMENTO", SqlDbType.Decimal, DBNull.Value);

            if (frequenciaDiaria.DataReposicao == null || frequenciaDiaria.DataReposicao == DateTime.MinValue)
            {
                contextQuery.Parameters.Add("@DATAREPOSICAO", SqlDbType.DateTime, frequenciaDiaria.DataFrequencia.Date);
            }
            else
            {
                contextQuery.Parameters.Add("@DATAREPOSICAO", SqlDbType.DateTime, DBNull.Value);
            }

            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, frequenciaDiaria.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaDataReposicao(DataContext contexto, string censo, DateTime dataFrequencia, DateTime? dataReposicao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Turma.FREQUENCIADIARIA
                                        SET DATAREPOSICAO = @DATAREPOSICAO
                                     WHERE FACULDADE = @CENSO
									    AND DATAFREQUENCIA = @DATAFREQUENCIA ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@DATAFREQUENCIA", SqlDbType.DateTime, dataFrequencia.Date);

            if (dataReposicao == null || dataReposicao == DateTime.MinValue)
            {
                contextQuery.Parameters.Add("@DATAREPOSICAO", SqlDbType.DateTime, DBNull.Value);
            }
            else
            {
                contextQuery.Parameters.Add("@DATAREPOSICAO", SqlDbType.DateTime, Convert.ToDateTime(dataReposicao).Date);
            }

            contexto.ApplyModifications(contextQuery);
        }

        public void Remove(DataContext contexto, string censo, DateTime dataFrequencia)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE Turma.FREQUENCIADIARIA
								WHERE FACULDADE = @CENSO
									AND DATAFREQUENCIA = @DATAFREQUENCIA ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@DATAFREQUENCIA", SqlDbType.DateTime, dataFrequencia.Date);

            contexto.ApplyModifications(contextQuery);
        }

        public DataTable ListaMatriculaPor(int ano, int semestre, string turma, string disciplina, DateTime dataFrequencia)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable lista = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"[RecursosHumanos].[SP_ALUNOSFREQUENCIADIARIA]";
                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@DATAFREQUENCIA", dataFrequencia);

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

        public bool PossuiFrequenciaDiaria(int ano, int periodo, string turma)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(1)
                                    FROM Turma.FREQUENCIADIARIA
                                    WHERE TURMA = @TURMA
                                    AND ANO = @ANO
                                    AND SEMESTRE = @PERIODO ";

                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
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
                contexto.Dispose();
            }
        }

        public bool ExisteFrequenciaPor(string faculdade, string turno, string curso, string curriculo, decimal serie)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*)
                                FROM    LY_HOR_OPER HO WITH ( NOLOCK )
                                        INNER JOIN LY_HOR_AULA HA WITH ( NOLOCK ) ON HA.AULA = HO.AULA
                                                                                     AND HA.DIA_SEMANA = HO.DIA_SEMANA
                                                                                     AND HA.FACULDADE = HO.FACULDADE
                                                                                     AND HA.TURNO = HO.TURNO
                                        INNER JOIN [Turma].[FREQUENCIADIARIA] AD WITH ( NOLOCK ) ON HA.AULA = AD.AULA
                                                                                         AND HA.DIA_SEMANA = AD.DIA_SEMANA
                                                                                         AND HA.FACULDADE = AD.FACULDADE
                                                                                         AND HA.TURNO = AD.TURNO
                                                                                         AND HA.DISCIPLINA = AD.DISCIPLINA
                                                                                         AND HA.TURMA = AD.TURMA
                                                                                         AND HA.ANO = AD.ANO
                                                                                         AND HA.SEMESTRE = AD.SEMESTRE
                                        INNER JOIN LY_TURMA T WITH ( NOLOCK ) ON T.ANO = AD.ANO
                                                                                 AND T.SEMESTRE = AD.SEMESTRE
                                                                                 AND T.DISCIPLINA = AD.DISCIPLINA
                                                                                 AND T.TURMA = AD.TURMA
                                                                               
                                WHERE   HO.FACULDADE = @FACULDADE
                                        AND HO.TURNO = @TURNO
                                        AND HO.CURSO = @CURSO
                                        AND HO.CURRICULO = @CURRICULO
                                        AND HO.SERIE = @SERIE  ";

                contextQuery.Parameters.Add("@FACULDADE", faculdade);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@CURRICULO", curriculo);
                contextQuery.Parameters.Add("@SERIE", serie);


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
    }
}
