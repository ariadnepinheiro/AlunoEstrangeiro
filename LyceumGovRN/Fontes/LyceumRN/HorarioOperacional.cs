using Techne.Data;
using Techne.Lyceum.RN.Util;
using System.Collections.Generic;
using Techne.Lyceum.RN.Entidades;
using System.Data;
using Techne.Lyceum.CR;
using System;
using Techne.Library;
using Seeduc.Infra.Data;
using System.Linq;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN
{
    public class HorarioOperacional : RNBase
    {
        public List<int> ListaDiasSemana()
        {
            List<int> diasSemana = new List<int>();
            diasSemana.Add(1); //Domingo
            diasSemana.Add(2); //Segunda
            diasSemana.Add(3); //Terça
            diasSemana.Add(4); //Quarta
            diasSemana.Add(5); //Quinta
            diasSemana.Add(6); //Sexta
            diasSemana.Add(7); //Sabado

            return diasSemana;
        }

        public bool PossuiCursoPor(DataContext ctx, string curso)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                FROM LY_HOR_OPER
                                WHERE CURSO = @CURSO ";

            contextQuery.Parameters.Add("@CURSO", curso);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public static QueryTable Consultar(string unidade_fisica, string turno, string curso, string curriculo, decimal serie)
        {
            QueryTable qt = null;

            if (!string.IsNullOrEmpty(unidade_fisica)
                && !string.IsNullOrEmpty(turno)
                && !string.IsNullOrEmpty(curso)
                && !string.IsNullOrEmpty(curriculo)
                && serie > 0)
            {
                var connection = Config.CreateConnection();

                connection.Open();

                string sql = " SELECT DISTINCT AULA, HORAINI_AULA, HORAFIM_AULA, ORDEM " +
                             " FROM LY_HOR_OPER " +
                             " WHERE FACULDADE = ? " +
                             " AND TURNO = ? " +
                             " AND CURSO = ? " +
                             " AND CURRICULO = ? " +
                             " AND SERIE = ? " +
                             " ORDER BY ORDEM, HORAINI_AULA ASC";
                try
                {
                    qt = new QueryTable(sql);

                    qt.Query(connection, unidade_fisica, turno, curso, curriculo, serie);
                }
                finally
                {
                    connection.Close();
                }
            }

            return qt;
        }

        /// Verifica se existe horário operacional
        public static QueryTable VerificarHorarioOperacional(TConnection connection, string turno, string faculdade, decimal diaSemana, decimal aula, string curso, string curriculo, decimal serie)
        {
            QueryTable qt = null;

            if (!string.IsNullOrEmpty(turno)
                && !string.IsNullOrEmpty(faculdade)
                && diaSemana > 0
                && aula > 0
                && !string.IsNullOrEmpty(curso)
                && !string.IsNullOrEmpty(curriculo)
                && serie > 0)
            {
                string sql = " SELECT horaini_aula, horafim_aula FROM LY_HOR_OPER " +
                             " Where turno = ? " +
                             " AND faculdade = ? " +
                             " AND DIA_SEMANA = ? " +
                             " AND AULA = ?" +
                             " AND CURSO = ?" +
                             " AND CURRICULO = ?" +
                             " AND SERIE = ?";

                qt = new QueryTable(sql);


                qt.Query(connection, turno, faculdade, diaSemana, aula, curso, curriculo, serie);
            }

            return qt;
        }
       
        public static RetValue SalvarTurno(Ly_turno dtTurno)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);
            RetValue retorno = null;

            try
            {
                if (dtTurno != null)
                {
                    if (dtTurno.Rows != null)
                    {
                        for (int j = 0; j < dtTurno.Rows.Count; j++)
                        {



                            ColunasTable colunas = MontarParametros(dtTurno.Columns, dtTurno.Rows[j]);

                            // Ly_turno.Row.Insert(connection,dtTurno.Rows[j].Turno,dtTurno.Rows[j].Mnemonico,"",
                            //                       "09:09", dtTurno.Rows[j].Horafim , null,
                            //                        colunas.Colunas, colunas.ValorColuna);

                            Ly_turno.Row.Update(connection, dtTurno.Rows[j].Turno, colunas.Colunas, colunas.ValorColuna);

                            retorno = VerificarErro(connection.GetErrors());

                            if (retorno != null && !retorno.Ok)
                            {
                                connection.Rollback();
                                return retorno;
                            }


                        }

                        retorno = new RetValue(true, "Registro atualizado com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }

            return retorno;
        }

        public static string ObterGradeID(string _curso, string _turno, string _curriculo, string _unidadeEns, string _unidadeFis, decimal serie)
        {
            QueryTable qt = null;

            string sql = "Select grade_id from ly_grade_serie where curso = ? and turno = ? and curriculo = ? and serie = ? and faculdade = ? and unidade_responsavel = ? ";

            qt = new QueryTable(sql);

            qt.Query(Config.CreateConnection(), _curso, _turno, _curriculo, serie, _unidadeFis, _unidadeEns);

            if (qt.Rows != null)
            {
                if (qt.Rows.Count > 0)
                    return qt.Rows[0]["grade_id"].ToString();
                else
                    return string.Empty;
            }
            else
                return string.Empty;
        }

        public static bool ExisteHorarioOper(string curso, string turno, string curriculo, string serie)
        {
            QueryTable qt = Consultar("select 1 From ly_hor_oper where CURSO = ? and TURNO = ? and CURRICULO = ? and SERIE = ?", curso, turno, curriculo, serie);
            return qt.Rows.Count > 0;
        }

        public DataTable ListaPor(string unidadeFisica, string curso, string turno, string curriculo, string serie)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable horarios = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"A_DADOSHORARIOOPER";
                contextQuery.Parameters.Add("@UNIDADE_FIS", unidadeFisica);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@CURRICULO", curriculo);
                contextQuery.Parameters.Add("@SERIE", serie);

                horarios = contexto.GetDataTable(contextQuery);
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
            return horarios;
        }

        public ValidacaoDados Valida(List<LyHorOper> listaHorarios, int anoCurriculo)
        {
            List<string> mensagens = new List<string>();
            string erro = string.Empty;
            DateTime inicioTurno = DateTime.MinValue;
            DateTime finalTurno = DateTime.MinValue;
            string[] horainicioFimTurno = new string[2];
            Turno rnTurno = new Turno();
            CursoDuracao rnCursoDuracao = new CursoDuracao();
            List<int> diasSemana = new List<int>();
            TimeSpan duracao = new TimeSpan();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            //Verifica campos obrigatorios gerais
            if (listaHorarios.Where(x => string.IsNullOrEmpty(x.Turno)).Count() > 0)
            {
                mensagens.Add("O campo TURNO deve ser selecionado.");
            }
            else
            {
                //Buscar horarios de inicio e fim do turno
                horainicioFimTurno = rnTurno.ObtemHoraInicioFimPor(listaHorarios[0].Turno);

                if (horainicioFimTurno[0] != string.Empty && horainicioFimTurno[1] != string.Empty)
                {
                    inicioTurno = new DateTime(1899, 12, 30, Convert.ToInt32(horainicioFimTurno[0].Split(':').First()), Convert.ToInt32(horainicioFimTurno[0].Split(':').Last()), 0);
                    finalTurno = new DateTime(1899, 12, 30, Convert.ToInt32(horainicioFimTurno[1].Split(':').First()), Convert.ToInt32(horainicioFimTurno[1].Split(':').Last()), 0);
                }
                else
                {
                    mensagens.Add("Não foi encontrado horario de inicio e fim para o TURNO.");
                }
            }

            if (listaHorarios.Where(x => string.IsNullOrEmpty(x.Faculdade)).Count() > 0)
            {
                mensagens.Add("O campo FACULDADE deve ser selecionado.");
            }

            if (listaHorarios.Where(x => string.IsNullOrEmpty(x.Curso)).Count() > 0)
            {
                mensagens.Add("O campo CURSO deve ser selecionado.");
            }

            if (listaHorarios.Where(x => string.IsNullOrEmpty(x.Curriculo)).Count() > 0)
            {
                mensagens.Add("O campo CURRICULO deve ser selecionado.");
            }

            if (listaHorarios.Where(x => x.Serie < 0).Count() > 0)
            {
                mensagens.Add("O campo SERIE deve ser selecionado.");
            }

            if (listaHorarios.Where(x => x.DuracaoAula < 0).Count() > 0)
            {
                mensagens.Add("O campo DURAÇÃO deve ser selecionado.");
            }

            if (listaHorarios.Where(x => x.DiaSemana < 0).Count() > 0)
            {
                mensagens.Add("O campo DIA SEMANA não foi encontrado.");
            }

            if (listaHorarios.Where(x => x.Ordem < 0).Count() > 0)
            {
                mensagens.Add("O campo ORDEM é obrigatório em todos os horários.");
            }

            //Verifica se a duração é uma duração cadastrada
            if (!rnCursoDuracao.EhDuracaoCadastradaPor(anoCurriculo, listaHorarios.Select(x => x.Curso).First(), listaHorarios.Select(x => x.Turno).First(), Convert.ToInt32(listaHorarios.Select(x => x.DuracaoAula).First())))
            {
                mensagens.Add("A DURAÇÃO escolhida não está cadastrada para este ano / curso / turno.");
            }

            if (listaHorarios.Where(x => x.HorainiAula == DateTime.MinValue).Count() > 0)
            {
                mensagens.Add("O campo HORA INÍCIO é obrigatório e deve ser um horário válido (hh:mm) em todos os horários.");
            }
            else
            {
                if (inicioTurno != DateTime.MinValue)
                {
                    if (listaHorarios.Where(x => x.HorainiAula < inicioTurno).Count() > 0)
                    {
                        mensagens.Add(string.Format("Não é permitido incluir HORÁRIOS INÍCIO menor do que o início do turno {0}.", inicioTurno.ToString("HH:mm")));
                    }
                }
            }

            if (listaHorarios.Where(x => x.HorafimAula == DateTime.MinValue).Count() > 0)
            {
                mensagens.Add("O campo HORA FIM é obrigatório e deve ser um horário válido (hh:mm) em todos os horários.");
            }
            else
            {
                if (finalTurno != DateTime.MinValue)
                {
                    if (listaHorarios.Where(x => x.HorafimAula > finalTurno).Count() > 0)
                    {
                        mensagens.Add(string.Format("Não é permitido incluir HORÁRIOS FINAL maior do que o final do turno {0}.", finalTurno.ToString("HH:mm")));
                    }
                }
            }

            //Verifica se existe hora inicio e hora fim validos
            if (listaHorarios.Where(x => x.HorafimAula == DateTime.MinValue || x.HorainiAula == DateTime.MinValue).Count() == 0)
            {
                if (listaHorarios.Where(x => x.HorafimAula <= x.HorainiAula).Count() > 0)
                {
                    mensagens.Add("A HORA INÍCIO deve ser menor que a HORA FIM em todos os horários.");
                }
            }

            foreach (LyHorOper horario in listaHorarios)
            {
                duracao = horario.HorafimAula.Subtract(horario.HorainiAula);

                if (duracao.Hours == 1 != (horario.DuracaoAula == 60))
                {
                    erro = "Intervalo de Horário deve ser igual a duração ou deve ser preenchido.";
                    if (!mensagens.Contains(erro))
                    {
                        mensagens.Add(erro);
                    }
                }
                if (duracao.Hours != 1 && duracao.Minutes != horario.DuracaoAula)
                {
                    erro = "Intervalo de Horário deve ser igual a duração ou deve ser preenchido.";
                    if (!mensagens.Contains(erro))
                    {
                        mensagens.Add(erro);
                    }
                }

                if (listaHorarios.Where(x => x.HorainiAula < horario.HorainiAula && x.Ordem > horario.Ordem).Count() > 0)
                {
                    erro = "Os horários estão fora de sequência. Favor verificar.";
                    if (!mensagens.Contains(erro))
                    {
                        mensagens.Add(erro);
                    }
                }

                if (listaHorarios.Where(x => x.HorainiAula == horario.HorainiAula && x.Ordem != horario.Ordem).Count() > 0)
                {
                    erro = "Existe HORÁRIO INICIAL repetido. Favor verificar.";
                    if (!mensagens.Contains(erro))
                    {
                        mensagens.Add(erro);
                    }
                }

                if (listaHorarios.Where(x => horario.HorainiAula > x.HorainiAula && horario.HorainiAula < x.HorafimAula && x.Ordem != horario.Ordem).Count() > 0)
                {
                    erro = "Não é permitido um HORÁRIO DE INÍCIO dentro de um intervalo de horário já cadastrado.";
                    if (!mensagens.Contains(erro))
                    {
                        mensagens.Add(erro);
                    }
                }

                if (listaHorarios.Where(x => horario.HorafimAula > x.HorainiAula && horario.HorafimAula < x.HorafimAula && x.Ordem != horario.Ordem).Count() > 0)
                {
                    erro = "Não é permitido um HORÁRIO DE FIM dentro de um intervalo de horário já cadastrado.";
                    if (!mensagens.Contains(erro))
                    {
                        mensagens.Add(erro);
                    }
                }

                //Verifica se existe erros de preenchimento para iniciar validações de banco
                if (mensagens.Count == 0)
                {
                    //Verifica se aula já existe para fazer validações especificas da atualização
                    if (horario.Aula > 0)
                    {
                        if (ExisteHoraAulaPor(horario.Faculdade, horario.Turno, horario.Curso, horario.Curriculo, horario.Serie))
                        {
                            erro = "Horarios não podem ser alterados porque existem turmas utilizando este horário operacional.";
                            if (!mensagens.Contains(erro))
                            {
                                mensagens.Add(erro);
                            }
                        }
                    }
                    else
                    {
                        //Carrega dias da semana
                        diasSemana = ListaDiasSemana();

                        //Percorre todos os dias da semana
                        foreach (int dia in diasSemana)
                        {
                            horario.DiaSemana = dia;

                            //Verifica se ja exista no banco outro cadastro para a faculdade / curso /turno / serie / dia semana / ordem
                            if (EhHorarioCadastradoPor(horario.Faculdade, horario.Turno, horario.Curso, horario.Curriculo, horario.Serie, horario.DiaSemana, horario.Ordem))
                            {
                                erro = "Horarios já cadastrado. Favor verificar.";
                                if (!mensagens.Contains(erro))
                                {
                                    mensagens.Add(erro);
                                }
                            }
                        }
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

        public ValidacaoDados ValidaRemocao(string faculdade, string turno, string curso, string curriculo, decimal serie)
        {
            RN.AulaDocente rnAulaDocente = new AulaDocente();
            RN.Turmas.FrequenciaDiaria rnFrequenciaDiaria = new Techne.Lyceum.RN.Turmas.FrequenciaDiaria();
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (string.IsNullOrEmpty(faculdade))
            {
                mensagens.Add("O campo UNIDADE ENSINO não foi encontrado.");
            }

            if (string.IsNullOrEmpty(turno))
            {
                mensagens.Add("O campo TURNO não foi encontrado.");
            }

            if (string.IsNullOrEmpty(curso))
            {
                mensagens.Add("O campo CURSO não foi encontrado.");
            }

            if (string.IsNullOrEmpty(curriculo))
            {
                mensagens.Add("O campo CURRICULO não foi encontrado.");
            }
            if (serie <= 0)
            {
                mensagens.Add("O campo SERIE não foi encontrado.");
            }

            if (mensagens.Count == 0)
            {
                if (!EhHorarioCadastradoPor(faculdade, turno, curso, curriculo, serie))
                {
                    mensagens.Add("Não foi encontrado nenhum horario operacional cadastrado para a Unidade Ensino / Curso / Turno / Curriculo / Serie.");
                }

                if (ExisteHoraAulaTurmaDocentePor(faculdade, turno, curso, curriculo, serie))
                {
                    mensagens.Add("Não foi possível remover os horários pois existem horário de aulas ativos para esse horário operacional.");
                }

                if (rnAulaDocente.ExisteAulaVigentePor(faculdade, turno, curso, curriculo, serie))
                {
                    mensagens.Add("Não foi possível remover os horários pois existem aulas ativa para esse horário operacional.");
                }

                if (rnFrequenciaDiaria.ExisteFrequenciaPor(faculdade, turno, curso, curriculo, serie))
                {
                    mensagens.Add("Não foi possível remover os horários pois existem frequência diária para esse horário operacional.");
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

        public void Salva(List<LyHorOper> listaHorarios)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            List<int> diasSemana = new List<int>();
            bool insere = false;

            try
            {
                diasSemana = ListaDiasSemana();

                foreach (LyHorOper horario in listaHorarios)
                {
                    //Verifica se já existe a aula e alimenta aula na entidade (caso não exista valor será 0)
                    horario.Aula = ObtemAulaPor(horario.Faculdade, horario.Turno, horario.Curso, horario.Curriculo, horario.Serie, horario.Ordem);

                    if (horario.Aula <= 0)
                    {                        
                        //Busca proxima aula
                        horario.Aula = ObtemProximaAulaPor(horario.Faculdade, horario.Turno);
                        insere = true;
                    }

                    //Percorre todos os dias da semana
                    foreach (int dia in diasSemana)
                    {
                        horario.DiaSemana = dia;

                        if (insere)
                        {                            
                            this.Insere(ctx, horario);
                        }
                        else
                        {
                            this.Altera(ctx, horario);                            
                        }
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

        public void Remove(string faculdade, string turno, string curso, string curriculo, decimal serie)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Apaga glps de docentes em aula
                RemoveAulaDocenteTipo(ctx, faculdade, turno, curso, curriculo, serie);

                //Apaga docente em aula
                RemoveAulaDocente(ctx, faculdade, turno, curso, curriculo, serie);

                //Apaga aulas
                RemoveHoraAula(ctx, faculdade, turno, curso, curriculo, serie);

                //Apaga horario operacional
                Remove(ctx, faculdade, turno, curso, curriculo, serie);
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
                } throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        private bool ExisteHoraAulaTurmaDocentePor(string faculdade, string turno, string curso, string curriculo, decimal serie)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                FROM    LY_HOR_OPER HO WITH ( NOLOCK )
                                        INNER JOIN LY_HOR_AULA HA WITH ( NOLOCK ) ON HA.AULA = HO.AULA
                                                                                     AND HA.DIA_SEMANA = HO.DIA_SEMANA
                                                                                     AND HA.FACULDADE = HO.FACULDADE
                                                                                     AND HA.TURNO = HO.TURNO
                                        INNER JOIN LY_AULA_DOCENTE AD WITH ( NOLOCK ) ON HA.AULA = AD.AULA
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
                                                                                 AND T.DT_FIM = AD.DATA_FIM
                                WHERE   T.SIT_TURMA = 'ABERTA'
                                        AND HO.FACULDADE = @FACULDADE
                                        AND HO.TURNO = @TURNO
                                        AND HO.CURSO = @CURSO
                                        AND HO.CURRICULO = @CURRICULO
                                        AND HO.SERIE = @SERIE ";

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

        public bool ExisteHoraAulaPor(string faculdade, string turno, string curso, string curriculo, decimal serie)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                FROM    LY_HOR_OPER HO ( NOLOCK )
                                        INNER JOIN LY_HOR_AULA HA ( NOLOCK ) ON HA.AULA = HO.AULA
                                                                                AND HA.DIA_SEMANA = HO.DIA_SEMANA
                                                                                AND HA.FACULDADE = HO.FACULDADE
                                                                                AND HA.TURNO = HO.TURNO
                                WHERE   HO.FACULDADE = @FACULDADE
                                        AND HO.TURNO = @TURNO
                                        AND HO.CURSO = @CURSO
                                        AND HO.CURRICULO = @CURRICULO
                                        AND HO.SERIE = @SERIE ";

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

        private bool EhHorarioCadastradoPor(string faculdade, string turno, string curso, string curriculo, decimal serie, decimal diaSemana, decimal ordem)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool casdatrado = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                        FROM    dbo.LY_HOR_OPER
                                        WHERE   FACULDADE = @FACULDADE
                                                AND TURNO = @TURNO
                                                AND CURSO = @CURSO
                                                AND CURRICULO = @CURRICULO
                                                AND SERIE = @SERIE
                                                AND DIA_SEMANA = @DIA_SEMANA
                                                AND ORDEM = @ORDEM ";

                contextQuery.Parameters.Add("@FACULDADE", faculdade);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@CURRICULO", curriculo);
                contextQuery.Parameters.Add("@SERIE", serie);
                contextQuery.Parameters.Add("@DIA_SEMANA", diaSemana);
                contextQuery.Parameters.Add("@ORDEM", ordem);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    casdatrado = true;
                }

                return casdatrado;
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

        private bool EhHorarioCadastradoPor(string faculdade, string turno, string curso, string curriculo, decimal serie)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool casdatrado = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                        FROM    dbo.LY_HOR_OPER
                                        WHERE   FACULDADE = @FACULDADE
                                                AND TURNO = @TURNO
                                                AND CURSO = @CURSO
                                                AND CURRICULO = @CURRICULO
                                                AND SERIE = @SERIE ";

                contextQuery.Parameters.Add("@FACULDADE", faculdade);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@CURRICULO", curriculo);
                contextQuery.Parameters.Add("@SERIE", serie);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    casdatrado = true;
                }

                return casdatrado;
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

        private int ObtemAulaPor(string faculdade, string turno, string curso, string curriculo, decimal serie, decimal ordem)
        {
            int aula = 0;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DISTINCT TOP 1
                                        AULA
                                FROM    LY_HOR_OPER
                                WHERE   FACULDADE = @FACULDADE
                                        AND TURNO = @TURNO
                                        AND CURSO = @CURSO
                                        AND CURRICULO = @CURRICULO
                                        AND SERIE = @SERIE
                                        AND ORDEM = @ORDEM  ";

                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@FACULDADE", faculdade);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@CURRICULO", curriculo);
                contextQuery.Parameters.Add("@SERIE", serie);
                contextQuery.Parameters.Add("@ORDEM", ordem);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    aula = Convert.ToInt32(reader["AULA"]);
                }

                return aula;
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

        private int ObtemProximaAulaPor(string faculdade, string turno)
        {
            int proximoAula = 0;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  ISNULL(MAX(AULA), 0) + 1 AULA
                                        FROM    LY_HOR_OPER
                                        WHERE   TURNO = @TURNO
                                                AND FACULDADE = @FACULDADE  ";

                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@FACULDADE", faculdade);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    proximoAula = Convert.ToInt32(reader["AULA"]);
                }

                return proximoAula;
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

        private void Insere(DataContext ctx, LyHorOper horarioOperacional)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT  INTO dbo.LY_HOR_OPER
                                    ( TURNO ,
                                      FACULDADE ,
                                      DIA_SEMANA ,
                                      AULA ,
                                      HORAINI_AULA ,
                                      HORAFIM_AULA ,
                                      CURSO ,
                                      CURRICULO ,
                                      SERIE ,
                                      ORDEM ,
                                      STAMP_ATUALIZACAO ,
                                      DURACAO_AULA
                                    )
                            VALUES  ( @TURNO ,
                                      @FACULDADE ,
                                      @DIA_SEMANA ,
                                      @AULA ,
                                      @HORAINI_AULA ,
                                      @HORAFIM_AULA ,
                                      @CURSO ,
                                      @CURRICULO ,
                                      @SERIE ,
                                      @ORDEM ,
                                      @STAMP_ATUALIZACAO ,
                                      @DURACAO_AULA
                                    ) ";

                contextQuery.Parameters.Add("@TURNO", horarioOperacional.Turno);
                contextQuery.Parameters.Add("@FACULDADE", horarioOperacional.Faculdade);
                contextQuery.Parameters.Add("@DIA_SEMANA", horarioOperacional.DiaSemana);
                contextQuery.Parameters.Add("@AULA", horarioOperacional.Aula);
                contextQuery.Parameters.Add("@HORAINI_AULA", horarioOperacional.HorainiAula);
                contextQuery.Parameters.Add("@HORAFIM_AULA", horarioOperacional.HorafimAula);
                contextQuery.Parameters.Add("@CURSO", horarioOperacional.Curso);
                contextQuery.Parameters.Add("@CURRICULO", horarioOperacional.Curriculo);
                contextQuery.Parameters.Add("@SERIE", horarioOperacional.Serie);
                contextQuery.Parameters.Add("@ORDEM", horarioOperacional.Ordem);
                contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", DateTime.Now);
                contextQuery.Parameters.Add("@DURACAO_AULA", horarioOperacional.DuracaoAula);

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
        }

        private void Altera(DataContext ctx, LyHorOper horarioOperacional)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE  LY_HOR_OPER
                            SET     HORAINI_AULA = @HORAINI_AULA ,
                                    HORAFIM_AULA = @HORAFIM_AULA ,
                                    CURSO = @CURSO ,
                                    CURRICULO = @CURRICULO ,
                                    SERIE = @SERIE ,
                                    ORDEM = @ORDEM ,
                                    STAMP_ATUALIZACAO = @STAMP_ATUALIZACAO ,
                                    DURACAO_AULA = @DURACAO_AULA
                            WHERE   AULA = @AULA
                                    AND TURNO = @TURNO
                                    AND FACULDADE = @FACULDADE
                                    AND DIA_SEMANA = @DIA_SEMANA ";

                contextQuery.Parameters.Add("@TURNO", horarioOperacional.Turno);
                contextQuery.Parameters.Add("@FACULDADE", horarioOperacional.Faculdade);
                contextQuery.Parameters.Add("@DIA_SEMANA", horarioOperacional.DiaSemana);
                contextQuery.Parameters.Add("@AULA", horarioOperacional.Aula);
                contextQuery.Parameters.Add("@HORAINI_AULA", horarioOperacional.HorainiAula);
                contextQuery.Parameters.Add("@HORAFIM_AULA", horarioOperacional.HorafimAula);
                contextQuery.Parameters.Add("@CURSO", horarioOperacional.Curso);
                contextQuery.Parameters.Add("@CURRICULO", horarioOperacional.Curriculo);
                contextQuery.Parameters.Add("@SERIE", horarioOperacional.Serie);
                contextQuery.Parameters.Add("@ORDEM", horarioOperacional.Ordem);
                contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", DateTime.Now);
                contextQuery.Parameters.Add("@DURACAO_AULA", horarioOperacional.DuracaoAula);

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
        }
        
        public void RemoveAulaDocenteTipo(DataContext ctx, string faculdade, string turno, string curso, string curriculo, decimal serie)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE AD
                        FROM    LY_HOR_OPER HO
                                JOIN LY_HOR_AULA HA ON HA.AULA = HO.AULA
                                                       AND HA.DIA_SEMANA = HO.DIA_SEMANA
                                                       AND HA.FACULDADE = HO.FACULDADE
                                                       AND HA.TURNO = HO.TURNO
                                JOIN LY_AULA_DOCENTE_TIPO AD ON HO.AULA = AD.AULA
                                                                AND HO.DIA_SEMANA = AD.DIA_SEMANA
                                                                AND HO.FACULDADE = AD.FACULDADE
                                                                AND HO.TURNO = AD.TURNO
                                                                AND AD.DISCIPLINA = HA.DISCIPLINA
                                                                AND AD.TURMA = HA.TURMA
                                                                AND AD.ANO = HA.ANO
                                                                AND HA.SEMESTRE = AD.SEMESTRE
                        WHERE   HO.FACULDADE = @FACULDADE
                                AND HO.TURNO = @TURNO
                                AND HO.CURSO = @CURSO
                                AND HO.CURRICULO = @CURRICULO
                                AND HO.SERIE = @SERIE ";

                contextQuery.Parameters.Add("@FACULDADE", faculdade);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@CURRICULO", curriculo);
                contextQuery.Parameters.Add("@SERIE", serie);

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
        }

        public void RemoveAulaDocente(DataContext ctx, string faculdade, string turno, string curso, string curriculo, decimal serie)
        {	
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE AD
                            FROM    LY_HOR_OPER HO
                                    JOIN LY_HOR_AULA HA ON HA.AULA = HO.AULA
                                                           AND HA.DIA_SEMANA = HO.DIA_SEMANA
                                                           AND HA.FACULDADE = HO.FACULDADE
                                                           AND HA.TURNO = HO.TURNO
                                    JOIN LY_AULA_DOCENTE AD ON HA.AULA = AD.AULA
                                                               AND HA.DIA_SEMANA = AD.DIA_SEMANA
                                                               AND HA.FACULDADE = AD.FACULDADE
                                                               AND HA.TURNO = AD.TURNO
                                                               AND AD.DISCIPLINA = HA.DISCIPLINA
                                                               AND AD.TURMA = HA.TURMA
                                                               AND AD.ANO = HA.ANO
                                                               AND HA.SEMESTRE = AD.SEMESTRE
                            WHERE   HO.FACULDADE = @FACULDADE
                                    AND HO.TURNO = @TURNO
                                    AND HO.CURSO = @CURSO
                                    AND HO.CURRICULO = @CURRICULO
                                    AND HO.SERIE = @SERIE ";

                contextQuery.Parameters.Add("@FACULDADE", faculdade);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@CURRICULO", curriculo);
                contextQuery.Parameters.Add("@SERIE", serie);

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
        }

        public void RemoveHoraAula(DataContext ctx, string faculdade, string turno, string curso, string curriculo, decimal serie)
        {	
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE HA
                                FROM    LY_HOR_OPER HO
                                        JOIN LY_HOR_AULA HA ON HA.AULA = HO.AULA
                                                               AND HA.DIA_SEMANA = HO.DIA_SEMANA
                                                               AND HA.FACULDADE = HO.FACULDADE
                                                               AND HA.TURNO = HO.TURNO      
                                WHERE   HO.FACULDADE = @FACULDADE
                                        AND HO.TURNO = @TURNO
                                        AND HO.CURSO = @CURSO
                                        AND HO.CURRICULO = @CURRICULO
                                        AND HO.SERIE = @SERIE ";

                contextQuery.Parameters.Add("@FACULDADE", faculdade);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@CURRICULO", curriculo);
                contextQuery.Parameters.Add("@SERIE", serie);

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
        }

        public void Remove(DataContext ctx, string faculdade, string turno, string curso, string curriculo, decimal serie)
        {	
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE  LY_HOR_OPER 
                                    WHERE   FACULDADE = @FACULDADE
                                            AND TURNO = @TURNO
                                            AND CURSO = @CURSO
                                            AND CURRICULO = @CURRICULO
                                            AND SERIE = @SERIE ";

                contextQuery.Parameters.Add("@FACULDADE", faculdade);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@CURRICULO", curriculo);
                contextQuery.Parameters.Add("@SERIE", serie);

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
        }
    }
}