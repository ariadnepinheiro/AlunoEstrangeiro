using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN
{
    public class TransferenciaTurma : RNBase
    {

        [Serializable]
        public class Horarios
        {
            public Horarios()
            {

            }

            private string _disciplina;
            private string _turma;
            private int _ano;
            private int _semestre;
            private DateTime _dtInicioTurma;
            private DateTime _dtFimTurma;
            private int _diaSemana;
            private int _aula;
            private DateTime _horaInicioAula;
            private DateTime _horaFimAula;
            private DateTime _dtInicioAula;
            private DateTime _dtFimAula;

            public string Disciplina
            {
                get { return _disciplina; }
                set { _disciplina = value; }
            }

            public string Turma
            {
                get { return _turma; }
                set { _turma = value; }
            }

            public int Ano
            {
                get { return _ano; }
                set { _ano = value; }
            }

            public int Semestre
            {
                get { return _semestre; }
                set { _semestre = value; }
            }

            public DateTime DtInicioTurma
            {
                get { return _dtInicioTurma; }
                set { _dtInicioTurma = value; }
            }

            public DateTime DtFimTurma
            {
                get { return _dtFimTurma; }
                set { _dtFimTurma = value; }
            }

            public int Aula
            {
                get { return _aula; }
                set { _aula = value; }
            }

            public int DiaSemana
            {
                get { return _diaSemana; }
                set { _diaSemana = value; }
            }

            public DateTime HoraInicioAula
            {
                get { return _horaInicioAula; }
                set { _horaInicioAula = value; }
            }

            public DateTime HoraFimAula
            {
                get { return _horaFimAula; }
                set { _horaFimAula = value; }
            }

            public DateTime DtInicioAula
            {
                get { return _dtInicioAula; }
                set { _dtInicioAula = value; }
            }

            public DateTime DtFimAula
            {
                get { return _dtFimAula; }
                set { _dtFimAula = value; }
            }
        }

        public static bool ExisteTurmaConcomitante(string aluno, string ano, string periodo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                //buscar turma concomitante anterior
                var contextQuery = new ContextQuery(
                    @"SELECT Count(1) 
                        FROM LY_MATRICULA 
                        WHERE SIT_MATRICULA = 'Matriculado' 
                        AND CONCOMITANTE = 'S' 
                        AND ANO = @ANO 
                        AND SEMESTRE = @SEMESTRE 
                        AND ALUNO = @ALUNO");

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);
                contextQuery.Parameters.Add("@ALUNO", aluno);

                var retorno = ctx.GetReturnValue<int>(contextQuery);

                if (retorno > 0)
                {
                    return true;
                }

                return false;
            }
        }

        public static string ConsultarGradeID(string aluno, string turma)
        {
            string sql = @"SELECT  DISTINCT mg.GRADE_ID
                        FROM    LY_MATGRADE mg
                                INNER JOIN dbo.LY_GRADE_TURMA gt ON mg.GRADE_ID = gt.GRADE_ID
                        WHERE   ALUNO = ?
                                AND SIT_MATGRADE = 'Matriculado'                              
                                AND gt.TURMA = ? ";
            return ConsultarCampo(sql, aluno, turma);
        }

        public DataTable ListaMotivoTransferencia()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT 
                                                   MOTIVO_TRANSF, 
                                                   DESCRICAO 
                                            FROM   DBO.LY_MOTIVO_TRANSF ";
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

        public bool PossuiRegistroTurmaTranfPor(DataContext ctx, string aluno, string turma, string ano, string periodo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*)  
                                    FROM LY_TURMA_TRANSF (NOLOCK)
                                    WHERE ALUNO = @ALUNO
	                                    AND TURMA_DESTINO = @TURMA_DESTINO 
	                                    AND ANO = @ANO
	                                    AND PERIODO = @PERIODO 
	                                    AND DATA = @DATA ";

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@TURMA_DESTINO", turma);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@DATA", DateTime.Today);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }
        //public static string AnalisarChoqueHorario(LyMatricula matriculaOrigem, LyMatricula matriculaDestino)
        //{
        //    return string.Empty;
        //}

        //public static string AnalisarChoqueHorario(LyMatricula matriculaOrigem, LyMatricula matriculaDestino)
        //{
        //    var erro = string.Empty;
        //    RN.Matricula rnMatricula = new Matricula();
        //    DataTable dtDisciplinasDeOutrasTurmas = new DataTable();
        //    DataTable dtDisciplinasTurmaDestino = new DataTable();
        //    RN.AulaDocente rnAulaDocente = new AulaDocente();

        //    ////carregar disciplinas matriculadas do aluno
        //    dtDisciplinasDeOutrasTurmas = rnMatricula.ListaDisciplinaGradePorTurmasDiferenteDe(matriculaDestino.Aluno, matriculaDestino.Ano, matriculaDestino.Semestre, matriculaOrigem.Turma);

        //    //carregar disciplinas da turma concomitante destino
        //    dtDisciplinasTurmaDestino = rnMatricula.ListaDisciplinaGradePor(matriculaDestino.Ano, matriculaDestino.Semestre, matriculaDestino.Turma);

        //    //cria lista de horarios
        //    List<DadosHorarios> listaHorario = new List<DadosHorarios>();

        //    //Adiciona na lista de horarios os horarios das disciplina regulares
        //    foreach (DataRow disciplinaRow in dtDisciplinasDeOutrasTurmas.Rows)
        //    {
        //        List<DadosHorarios> listaHorarioAux = rnAulaDocente.ObtemHorariosPor(disciplinaRow["disciplina"].ToString(), disciplinaRow["TURMA"].ToString(), matriculaDestino.Ano, matriculaDestino.Semestre);

        //        if (listaHorarioAux != null && listaHorarioAux.Count > 0)
        //        {
        //            listaHorario.AddRange(listaHorarioAux);
        //        }
        //    }

        //    //Adiciona na lista de horarios os horarios das disciplina da turma destino
        //    foreach (DataRow disciplinaRow in dtDisciplinasTurmaDestino.Rows)
        //    {
        //        matriculaDestino.Disciplina = disciplinaRow["disciplina"].ToString();
        //        List<DadosHorarios> listaHorarioAux = rnAulaDocente.ObtemHorariosPor(matriculaDestino.Disciplina, matriculaDestino.Turma, matriculaDestino.Ano, matriculaDestino.Semestre);

        //        if (listaHorarioAux != null && listaHorarioAux.Count > 0)
        //        {
        //            listaHorario.AddRange(listaHorarioAux);
        //        }
        //    }

        //    var valorRetorno = VerificarChoqueHorarioPor(matriculaDestino.Aluno, listaHorario);

        //    if (valorRetorno != null)
        //    {
        //        erro = valorRetorno.Aggregate((x, y) => x + "<br/>" + y);

        //        erro = "Choque de horário identificado.<br>" + erro.Replace(",", ",<br>");

        //    }

        //    return erro;
        //}

        public static string AnalisarChoqueHorarioEducacaoEspecial(LyMatricula matriculaOrigem, LyMatricula matriculaDestino, List<Atendimento> disciplinasAtendimento)
        {
            var erro = string.Empty;

            ////carregar disciplinas matriculadas do aluno
            var disciplinasDeOutrasTurmas = Matricula.ConsultaDisciplinaGradePorTurmasDiferenteDe(Convert.ToString(matriculaDestino.Aluno), Convert.ToString(matriculaDestino.Ano), Convert.ToString(matriculaDestino.Semestre), matriculaOrigem.Turma);

            //carregar disciplinas da turma concomitante destino
            var disciplinasTurmaDestino = Matricula.ConsultaDadosTurmaEducacaoEspecial(Convert.ToString(matriculaDestino.Ano), Convert.ToString(matriculaDestino.Semestre), matriculaDestino.Turma, disciplinasAtendimento);

            //cria lista de horarios
            var listaHorario = new List<Horarios>();

            //Adiciona na lista de horarios os horarios das disciplina regulares
            foreach (DataRow disciplinaRow in disciplinasDeOutrasTurmas.Rows)
            {
                List<Horarios> listaHorarioAux = CarregarHorarios(disciplinaRow["disciplina"].ToString(), disciplinaRow["TURMA"].ToString(), Convert.ToString(matriculaDestino.Ano), Convert.ToString(matriculaDestino.Semestre));
                if (listaHorarioAux != null && listaHorarioAux.Count > 0)
                {
                    listaHorario.AddRange(listaHorarioAux);
                }
            }

            //Adiciona na lista de horarios os horarios das disciplina da turma destino
            foreach (DataRow disciplinaRow in disciplinasTurmaDestino.Rows)
            {
                matriculaDestino.Disciplina = disciplinaRow["disciplina"].ToString();
                List<Horarios> listaHorarioAux = CarregarHorarios(matriculaDestino.Disciplina, matriculaDestino.Turma, Convert.ToString(matriculaDestino.Ano), Convert.ToString(matriculaDestino.Semestre));
                if (listaHorarioAux != null && listaHorarioAux.Count > 0)
                {
                    listaHorario.AddRange(listaHorarioAux);
                }
            }

            var valorRetorno = VerificarChoqueHorario(matriculaDestino.Aluno, listaHorario);

            if (valorRetorno != null && !valorRetorno.Ok)
            {
                if (valorRetorno.Errors != null)
                {
                    erro += valorRetorno.Errors.ToString();
                }
                erro = "Choque de horário identificado.<br>" + erro.Replace(",", ",<br>");
            }

            return erro;
        }

        public static RetValue VerificarChoqueHorario(string aluno, List<Horarios> listaHorarios)
        {
            ErrorList listaErro = new ErrorList();
            foreach (Horarios horario1 in listaHorarios)
            {
                //Para cada horário, buscar horários de outras disciplinas da pcolHorarios que possuam aula no mesmo dia (item DiaDaSemana)
                List<Horarios> horarioDiaSemana = listaHorarios.FindAll(delegate(Horarios h) { return h.DiaSemana.Equals(horario1.DiaSemana) && !(h.Disciplina.Equals(horario1.Disciplina) && h.Turma.Equals(horario1.Turma)); });

                if (horarioDiaSemana != null && horarioDiaSemana.Count > 0)
                {
                    foreach (Horarios horario2 in horarioDiaSemana)
                    {
                        if (horario1.HoraInicioAula < horario2.HoraFimAula
                           && horario1.HoraFimAula > horario2.HoraInicioAula
                           && horario1.DtInicioAula <= horario2.DtFimAula
                           && horario1.DtFimAula >= horario2.DtInicioAula)
                        {
                            ErrorList erro = new ErrorList();
                            erro.Add("Coincidência de horário de " + horario2.Disciplina + "/" + horario2.Turma +
                                     " com a disciplina " + horario1.Disciplina + "/" + horario2.Turma);

                            listaErro.Add(erro);
                        }
                    }
                }
            }

            if (listaErro.Count > 0)
            {
                return new RetValue(false, string.Empty, listaErro);
            }

            return null;
        }

        public static List<Horarios> CarregarHorarios(string disciplina, string turma, string ano, string semestre)
        {
            List<Horarios> listaHorarios = new List<Horarios>();

            StringBuilder sql = new StringBuilder();
            sql.Append(" select t.DISCIPLINA, t.TURMA, t.ANO, t.SEMESTRE, ");
            sql.Append(" t.DT_INICIO dtInicioTurma, t.DT_FIM dtFimTurma, ");
            sql.Append(" h.DIA_SEMANA, h.AULA, h.HORAINI_AULA, h.HORAFIM_AULA, ");
            sql.Append(" a.DATA_INICIO dtInicioAula, a.DATA_FIM dtFimAula ");
            sql.Append(" from LY_TURMA t join LY_HOR_AULA h ");
            sql.Append(" on  t.DISCIPLINA = h.DISCIPLINA ");
            sql.Append(" and t.TURMA = h.TURMA ");
            sql.Append(" and t.ANO = h.ANO ");
            sql.Append(" and t.SEMESTRE = h.SEMESTRE ");
            sql.Append(" join LY_AULA_DOCENTE a ");
            sql.Append(" on  h.TURNO = a.TURNO ");
            sql.Append(" and h.FACULDADE = a.FACULDADE ");
            sql.Append(" and h.DIA_SEMANA = a.DIA_SEMANA ");
            sql.Append(" and h.AULA = a.AULA ");
            sql.Append(" and h.DISCIPLINA = a.DISCIPLINA ");
            sql.Append(" and h.TURMA = a.TURMA ");
            sql.Append(" and h.ANO = a.ANO ");
            sql.Append(" and h.SEMESTRE = a.SEMESTRE ");
            sql.Append(" where t.DISCIPLINA = ? ");
            sql.Append(" and t.TURMA = ? ");
            sql.Append(" and t.ANO = ? ");
            sql.Append(" and t.SEMESTRE = ? ");
            sql.Append(" and a.DATA_FIM = t.DT_FIM and t.sit_turma = 'Aberta' ");

            QueryTable qt = Consultar(sql.ToString(), disciplina, turma, ano, semestre);

            if (qt != null && qt.Rows.Count > 0)
            {
                foreach (SimpleRow sr in qt.Rows)
                {
                    Horarios horario = new Horarios();

                    horario.Disciplina = Convert.ToString(sr["DISCIPLINA"]);
                    horario.Turma = Convert.ToString(sr["TURMA"]);
                    horario.Ano = Convert.ToInt32(sr["ANO"]);
                    horario.Semestre = Convert.ToInt32(sr["SEMESTRE"]);
                    horario.DtInicioTurma = Convert.ToDateTime(sr["dtInicioTurma"]);
                    horario.DtFimTurma = Convert.ToDateTime(sr["dtFimTurma"]);
                    horario.DiaSemana = Convert.ToInt32(sr["DIA_SEMANA"]);
                    horario.Aula = Convert.ToInt32(sr["AULA"]);
                    horario.HoraInicioAula = Convert.ToDateTime(sr["HORAINI_AULA"]);
                    horario.HoraFimAula = Convert.ToDateTime(sr["HORAFIM_AULA"]);
                    horario.DtInicioAula = Convert.ToDateTime(sr["dtInicioAula"]);
                    horario.DtFimAula = Convert.ToDateTime(sr["dtFimAula"]);

                    listaHorarios.Add(horario);
                }
            }

            if (listaHorarios.Count > 0)
                return listaHorarios;

            return null;
        }

        //Consultar grade por grade_id 
        public static string ConsultarGrade(string gradeId)
        {
            string sql = "Select grade from ly_grade_serie where GRADE_ID = ?";
            return ConsultarCampo(sql, gradeId);
        }

        public ValidacaoDados ValidaTransferenciaTurmaPrincipal(RN.DTOs.DadosTransferencia dadosTransferencia, out List<string> listaAvisos, bool realizarPermuta, RN.DTOs.DadosTransferencia dadosAlunoPermuta, out List<string> listaAvisosPermuta)
        {
            List<string> mensagens = new List<string>();
            listaAvisos = new List<string>();
            listaAvisosPermuta = new List<string>();
            ValidacaoDados validacaoDadosParcial = new ValidacaoDados();
            ValidacaoDados validacaoDadosFinal = new ValidacaoDados
            {
                Valido = false
            };

            //Valida dados do aluno da transferencia
            validacaoDadosParcial = this.ValidaTransferenciaTurmaPrincipal(dadosTransferencia, out listaAvisos, false);
            if (!validacaoDadosParcial.Valido)
            {
                mensagens.Add(validacaoDadosParcial.Mensagem);
            }

            //Verifica se aluno principal mudou de curso / serie / turno
            if (dadosTransferencia.SemestreAtual != dadosTransferencia.SemestreDestino
                            || dadosTransferencia.TurnoAtual != dadosTransferencia.TurnoDestino
                            || dadosTransferencia.SerieAtual != dadosTransferencia.SerieDestino
                            || dadosTransferencia.CursoAtual != dadosTransferencia.CursoDestino)
            {
                //Verifica se vai haver permuta
                if (realizarPermuta)
                {
                    //Valida dados do aluno da permuta
                    validacaoDadosParcial = new ValidacaoDados();
                    validacaoDadosParcial = this.ValidaTransferenciaTurmaPrincipal(dadosAlunoPermuta, out listaAvisosPermuta, true);
                    if (!validacaoDadosParcial.Valido)
                    {
                        mensagens.Add(validacaoDadosParcial.Mensagem);
                    }
                }
            }
            else if (realizarPermuta) //Verifica se vai haver permuta
            {
                mensagens.Add("Para colocar aluno na vaga que está sendo liberada é necessario que exista troca de turno / serie / curso.");
            }

            if (mensagens.Count > 0)
            {
                validacaoDadosFinal.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDadosFinal.Valido = true;
            }

            return validacaoDadosFinal;
        }

        public ValidacaoDados ValidaTransferenciaTurmaPrincipal(RN.DTOs.DadosTransferencia dadosTransferencia, out List<string> listaAvisos, bool ehAlunoPermuta)
        {
            RN.Curso rnCurso = new Curso();
            List<string> mensagens = new List<string>();
            RN.Aluno rnAluno = new RN.Aluno();
            RN.Perfil rnPerfil = new Perfil();
            RN.Usuarios rnUsuarios = new Usuarios();
            RN.Matricula rnMatricula = new RN.Matricula();
            RN.RestricaoIdadeSerie rnRestricaoIdadeSerie = new RestricaoIdadeSerie();
            RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            RN.Prova rnProva = new Prova();
            RN.Nota rnNota = new Nota();
            RN.Falta rnFalta = new Falta();
            RN.ControleVaga rnControleVaga = new ControleVaga();
            RN.HistMatricula rnHistMatricula = new HistMatricula();
            RN.Turma rnTurma = new Turma();
            List<string> listaAvisosParcial = new List<string>();
            TceRestricaoIdadeSerie restricao = new TceRestricaoIdadeSerie();
            LyMatricula matriculaOrigem = new LyMatricula();
            LyMatricula matriculaDestino = new LyMatricula();
            DataContext contexto = null;
            Int64 resultado;
            string tipoVaga = string.Empty;
            string retornoChoqueHorario = string.Empty;
            string avisoNotaMaximaProva = string.Empty;
            int vagasTurma = 0;
            int idade = 0;
            listaAvisos = new List<string>();

            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (string.IsNullOrEmpty(dadosTransferencia.Aluno))
            {
                mensagens.Add("O campo ALUNO é obrigatório!");
            }

            if (string.IsNullOrEmpty(dadosTransferencia.SituacaoAluno))
            {
                mensagens.Add("A SITUAÇÃO DO ALUNO deve ser informada");
            }
            else if (dadosTransferencia.SituacaoAluno != "Ativo")
            {
                mensagens.Add("Aluno não ativo ou não cadastrado (favor verificar).");
            }

            if (string.IsNullOrEmpty(dadosTransferencia.Ano))
            {
                mensagens.Add("O campo ANO é obrigatório!");
            }

            if (string.IsNullOrEmpty(dadosTransferencia.SemestreAtual))
            {
                mensagens.Add("O campo PERIODO ATUAL é obrigatório!");
            }
            else
            {
                if (!Int64.TryParse(dadosTransferencia.SemestreAtual, out resultado))
                {
                    mensagens.Add("Valor inválido para PERIODO ATUAL de matrícula.");
                }
            }

            if (string.IsNullOrEmpty(dadosTransferencia.SemestreDestino))
            {
                mensagens.Add("O campo PERIODO DESTINO é obrigatório!");
            }
            else
            {
                if (!Int64.TryParse(dadosTransferencia.SemestreDestino, out resultado))
                {
                    mensagens.Add("Valor inválido para PERIODO DESTINO de matrícula.");
                }
            }

            if (string.IsNullOrEmpty(dadosTransferencia.TurmaAtual))
            {
                mensagens.Add("O campo TURMA ATUAL é obrigatório!");
            }

            if (string.IsNullOrEmpty(dadosTransferencia.GradeIdAtual))
            {
                mensagens.Add("O campo GRADE ID ATUAL é obrigatório!");
            }

            if (string.IsNullOrEmpty(dadosTransferencia.SerieAtual))
            {
                mensagens.Add("O campo SERIE ATUAL é obrigatório!");
            }

            if (string.IsNullOrEmpty(dadosTransferencia.TurnoAtual))
            {
                mensagens.Add("O campo TURNO ATUAL é obrigatório!");
            }

            if (string.IsNullOrEmpty(dadosTransferencia.CursoAtual))
            {
                mensagens.Add("O campo CURSO ATUAL é obrigatório!");
            }

            if (string.IsNullOrEmpty(dadosTransferencia.TurmaDestino))
            {
                mensagens.Add("O campo TURMA DESTINO é obrigatório!");
            }
            else if (!string.IsNullOrEmpty(dadosTransferencia.TurmaAtual) && dadosTransferencia.TurmaAtual == dadosTransferencia.TurmaDestino)
            {
                mensagens.Add("A TURMA DE DESTINO não pode ser igual a turma atual do aluno.");
            }

            if (string.IsNullOrEmpty(dadosTransferencia.GradeIdDestino))
            {
                mensagens.Add("O campo GRADE ID DESTINO é obrigatório!");
            }

            if (string.IsNullOrEmpty(dadosTransferencia.UsuarioResponsavel))
            {
                mensagens.Add("O campo USUÁRIO RESPONSÁVEL é obrigatório!");
            }

            if (string.IsNullOrEmpty(dadosTransferencia.SerieDestino))
            {
                mensagens.Add("O campo SERIE DESTINO é obrigatório!");
            }

            if (string.IsNullOrEmpty(dadosTransferencia.TurnoDestino))
            {
                mensagens.Add("O campo TURNO DESTINO é obrigatório!");
            }

            if (string.IsNullOrEmpty(dadosTransferencia.CursoDestino))
            {
                mensagens.Add("O campo CURSO DESTINO é obrigatório!");
            }

            if (string.IsNullOrEmpty(dadosTransferencia.CurriculoDestino))
            {
                mensagens.Add("O campo CURRICULO DESTINO é obrigatório!");
            }

            if (string.IsNullOrEmpty(dadosTransferencia.NecessidadeEspecial))
            {
                mensagens.Add("A NECESSIDADE ESPECIAL do aluno não foi informada, favor ajustar utlizando a tela de Aluno!");
            }

            if (dadosTransferencia.DataNascimento == null || dadosTransferencia.DataNascimento == DateTime.MinValue)
            {
                mensagens.Add("O campo DATA DE NASCIMENTO é obrigatório!");
            }

            if (string.IsNullOrEmpty(dadosTransferencia.UnidadeEnsino))
            {
                mensagens.Add("O campo UNIDADE DE ENSINO é obrigatório!");
            }

            if (dadosTransferencia.ListaDisciplinasTurmaDestino == null || dadosTransferencia.ListaDisciplinasTurmaDestino.Count <= 0)
            {
                mensagens.Add("Não foram encontradas disciplinas para esta turma destino!");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se dados de destino serão os msm de origem
                    if ((dadosTransferencia.SemestreDestino != dadosTransferencia.SemestreAtual)
                        || (dadosTransferencia.CursoDestino != dadosTransferencia.CursoAtual)
                        || (dadosTransferencia.TurnoDestino != dadosTransferencia.TurnoAtual)
                        || (dadosTransferencia.SerieDestino != dadosTransferencia.SerieAtual))
                    {
                        //Verifica se o censo / ano / periodo / curso / serie / turno não participam da fase
                        //if (rnControleVaga.PartipaMatriculaFacilPor(contexto, dadosTransferencia.UnidadeEnsino, Convert.ToInt32(dadosTransferencia.Ano), Convert.ToInt32(dadosTransferencia.SemestreDestino), dadosTransferencia.CursoDestino, Convert.ToInt32(dadosTransferencia.SerieDestino), dadosTransferencia.TurnoDestino))
                        //{
                        //    mensagens.Add("A escola/curso/série/turno destino está participando do Matrícula Fácil, com isso apenas pode ser alterada a TURMA DE DESTINO, os demais dados devem ser mantidos");
                        //}

                        //Verifica se curso permite troca livre de opções
                        if (!rnCurso.PermitePermiteTransferenciaTurmaTotalPor(contexto, dadosTransferencia.CursoDestino))
                        {
                            //Verifica se usuario possui padrao de acesso para alterar campos diferentes da turma
                            if (!rnUsuarios.EhPrivilegiado(contexto, dadosTransferencia.UsuarioResponsavel) && !rnPerfil.PossuiPerfilTransferenciaTurmaTotalPor(contexto, dadosTransferencia.UsuarioResponsavel))
                            {
                                //Caso esteja indo do fundamental para o aprendendo a aprender
                                if ((dadosTransferencia.CursoAtual == "0001.21" || dadosTransferencia.CursoAtual == "2024.19" || dadosTransferencia.CursoAtual == "0001.26" || dadosTransferencia.CursoAtual == "0001.22")
                                     && dadosTransferencia.CursoDestino == "2024.20")
                                {
                                    if (dadosTransferencia.SerieDestino != "1" || (dadosTransferencia.SerieAtual != "6" && dadosTransferencia.SerieAtual != "7"))
                                    {
                                        mensagens.Add("A transferencia do curso fundamental para o aprendendo a aprender apenas é permitido apenas da série 6 ou 7 para a série 1.");
                                    }
                                }
                                else
                                {
                                    //Verifica se o censo / ano / periodo / curso / serie / turno não participam da fase
                                    if (rnControleVaga.PartipaMatriculaFacilPor(contexto, dadosTransferencia.UnidadeEnsino, Convert.ToInt32(dadosTransferencia.Ano), Convert.ToInt32(dadosTransferencia.SemestreDestino), dadosTransferencia.CursoDestino, Convert.ToInt32(dadosTransferencia.SerieDestino), dadosTransferencia.TurnoDestino))
                                    {
                                        mensagens.Add("Este usuário possui permissão para alterar apenas a TURMA DE DESTINO, os demais dados devem ser mantidos para escola/curso/série/turno destino que está participando do Matrícula Fácil.");
                                    }

                                    //mensagens.Add("Este usuário possui permissão para alterar apenas a TURMA DE DESTINO, os demais dados devem ser mantidos.");
                                }
                            }
                        }
                    }

                    //Verifica se está trocando de periodo
                    if (dadosTransferencia.SemestreAtual != dadosTransferencia.SemestreDestino)
                    {
                        if ((dadosTransferencia.SemestreAtual == "1" && dadosTransferencia.SemestreDestino == "2")
                            || (dadosTransferencia.SemestreAtual == "2" && dadosTransferencia.SemestreDestino == "1"))
                        {
                            mensagens.Add("A transferencia não pode ser realizada do período 1 para 2 ou 2 para 1.");
                        }

                        //Caso o aluno esteja trocando de periodo verifica se existe matriculas especiais em aberto.
                        if (rnMatricula.PossuiMatriculaEspecialAtivaPor(contexto, Convert.ToInt32(dadosTransferencia.Ano), dadosTransferencia.Aluno))
                        {
                            mensagens.Add("A transferencia não pode ser realizada com troca de período, pois o aluno possui matriculas especiais em aberto.");
                        }
                    }

                    if (dadosTransferencia.TipoCursoDestino == "Especial")
                    {
                        if (dadosTransferencia.NecessidadeEspecial == "Não possui.")
                        {
                            mensagens.Add("Para escolher um curso de educação especial, informar o tipo de necessidade especial do aluno(a) na aba 'Dados Pessoais'.");
                        }
                    }

                    else if (dadosTransferencia.TipoCursoDestino == "Concomitante/Subsequente")
                    {
                        if (string.IsNullOrEmpty(dadosTransferencia.TipoEnsProfissionalizanteDestino))
                        {
                            mensagens.Add("Para escolher um curso Concomitante/Subsequente, deverá escolher o Tipo de Ensino Profissionalizante.");
                        }
                    }

                    if (rnMatricula.EhMatriculaProgressaoParcial(contexto, dadosTransferencia.Aluno, Convert.ToInt32(dadosTransferencia.Ano), Convert.ToInt32(dadosTransferencia.SemestreDestino), dadosTransferencia.TurmaDestino))
                    {
                        mensagens.Add("A TURMA de destino não pode ser igual a turma de progressão parcial do aluno.");
                    }
                    else
                    {
                        //Verifica a mesma turma já foi finalizada anteriormente
                        if (rnHistMatricula.EhMatriculaHistoricoAtivaPor(contexto, dadosTransferencia.Aluno, Convert.ToInt32(dadosTransferencia.Ano), Convert.ToInt32(dadosTransferencia.SemestreDestino), dadosTransferencia.TurmaDestino))
                        {
                            mensagens.Add("Já existe matricula ativa no histórico para este aluno / ano / periodo / turma.");
                        }
                    }

                    //Verificar se aluno já possui situação final para a turma/ano/periodo
                    if (RN.SituacaoFinalAluno.ExisteSituacaoFinalPor(contexto, dadosTransferencia.Aluno, Convert.ToDecimal(dadosTransferencia.Ano), Convert.ToDecimal(dadosTransferencia.SemestreDestino), dadosTransferencia.TurmaDestino))
                    {
                        mensagens.Add("A transferencia não pode ser realizada pois este aluno já possui situação final para este ano/periodo/turma destino.");
                    }

                    //Verifica idade do aluno e restriução
                    idade = Utils.CalcularIdade(dadosTransferencia.DataNascimento);
                    restricao = rnRestricaoIdadeSerie.CarregaRestricaoPor(contexto, dadosTransferencia.CursoDestino, Convert.ToInt32(dadosTransferencia.SerieDestino));

                    //Verifica se o aluno possui necessidade especial
                    if (dadosTransferencia.NecessidadeEspecial != "<Nenhum>" && dadosTransferencia.NecessidadeEspecial != "Não possui.")
                    {
                        //Para Alunos com necessidades Especiais Verificar restrição de idade minima
                        if (idade < restricao.IdadeMinima)
                        {
                            mensagens.Add(string.Format("Para o curso selecionado não é permitido cadastrar alunos com necessidade especial com menos de {0} anos. Favor verificar a DATA DE NASCIMENTO!",
                               restricao.IdadeMinima));
                        }
                    }
                    else
                    {
                        //Para Alunos sem necessidades Especiais Verificar restrição de idade minima e maxima
                        if (idade < restricao.IdadeMinima || idade > restricao.IdadeMaxima)
                        {
                            mensagens.Add(string.Format("Para o curso selecionado é permitido cadastrar alunos entre {0} e {1} anos. Favor verificar a DATA DE NASCIMENTO!",
                                restricao.IdadeMinima,
                                restricao.IdadeMaxima));
                        }
                    }

                    //Apenas valida vagas para aluno que nao sejma da permuta
                    if (!ehAlunoPermuta)
                    {
                        //Verifica se aluno está trocando de periodo / turno / serie / curso
                        if (dadosTransferencia.SemestreAtual != dadosTransferencia.SemestreDestino
                            || dadosTransferencia.TurnoAtual != dadosTransferencia.TurnoDestino
                            || dadosTransferencia.SerieAtual != dadosTransferencia.SerieDestino
                            || dadosTransferencia.CursoAtual != dadosTransferencia.CursoDestino)
                        {
                            tipoVaga = rnConfirmacaoMatricula.ObtemTipoVagaParaTransferenciaDePossiveisPeriodosPor(contexto, dadosTransferencia.Aluno, Convert.ToInt32(dadosTransferencia.Ano), Convert.ToInt32(dadosTransferencia.SemestreDestino));
                            //Verifica se aluno tem confirmação de matricula para o ano / periodo
                            if (string.IsNullOrEmpty(tipoVaga))
                            {
                                mensagens.Add("Não será possível realizar a transferência, pois não existe confirmação de matrícula!");
                            }

                            //Valida Vaga unica                       
                            int vagasLiberadas = 0;
                            int vagasUtilizadas = 0;

                            //Verificar se tem vaga no curso / serie / turno / ano / semestre
                            vagasLiberadas = rnControleVaga.ObtemVagasLiberadasTotalPor(contexto,
                                dadosTransferencia.UnidadeEnsino,
                                Convert.ToInt32(dadosTransferencia.Ano),
                                Convert.ToInt32(dadosTransferencia.SemestreDestino),
                                Convert.ToInt32(dadosTransferencia.SerieDestino),
                                dadosTransferencia.CursoDestino,
                                dadosTransferencia.TurnoDestino);

                            vagasUtilizadas = rnControleVaga.ObtemVagasUtilizadasTotalPor(contexto,
                                dadosTransferencia.UnidadeEnsino,
                                Convert.ToInt32(dadosTransferencia.Ano),
                                Convert.ToInt32(dadosTransferencia.SemestreDestino),
                                Convert.ToInt32(dadosTransferencia.SerieDestino),
                                dadosTransferencia.CursoDestino,
                                dadosTransferencia.TurnoDestino);

                            if (vagasLiberadas <= vagasUtilizadas)
                            {
                                mensagens.Add("Não será possível realizar a transferência, pois não existem vagas disponíveis para a serie ou modalidade ou turno pretendidos.");
                            }
                        }

                        //Verifica se a turma tem vaga
                        vagasTurma = rnTurma.ObtemVagasPrincipalLiberadasTurmaPor(contexto, Convert.ToInt32(dadosTransferencia.Ano), Convert.ToInt32(dadosTransferencia.SemestreDestino), dadosTransferencia.TurmaDestino);
                        if (vagasTurma <= 0)
                        {
                            mensagens.Add("A capacidade da turma desejada nao comporta mais alunos.");
                        }
                    }

                    //Monta entidades para verificação de choque de horario
                    matriculaOrigem.Ano = Convert.ToInt32(dadosTransferencia.Ano);
                    matriculaOrigem.Semestre = Convert.ToInt32(dadosTransferencia.SemestreAtual);
                    matriculaOrigem.Turma = dadosTransferencia.TurmaAtual;
                    matriculaOrigem.Matricula = dadosTransferencia.UsuarioResponsavel;
                    matriculaOrigem.Aluno = dadosTransferencia.Aluno;

                    matriculaDestino.Ano = Convert.ToInt32(dadosTransferencia.Ano);
                    matriculaDestino.Semestre = Convert.ToInt32(dadosTransferencia.SemestreDestino);
                    matriculaDestino.Turma = dadosTransferencia.TurmaDestino;
                    matriculaDestino.Matricula = dadosTransferencia.UsuarioResponsavel;
                    matriculaDestino.Aluno = dadosTransferencia.Aluno;

                    //Verifica se existe choque de horario
                    retornoChoqueHorario = AnalisaChoqueHorarioPor(contexto, matriculaOrigem, matriculaDestino);
                    if (!string.IsNullOrEmpty(retornoChoqueHorario))
                    {
                        mensagens.Add("Transferência cancelada na verificação do choque de horário.");
                        mensagens.Add(retornoChoqueHorario);
                    }

                    //Verifica ContraTurno com Turma Concomitante
                    if (dadosTransferencia.PossuiTurmaConcomitante && !RN.Turno.VerificarContraTurno(dadosTransferencia.TurnoDestino, dadosTransferencia.TurnoAtualTurmaConcomitante))
                    {
                        mensagens.Add("A Turma Regular deve estar em contraturno com a turma profissional concomitante.");
                    }

                    if (PossuiRegistroTurmaTranfPor(contexto, dadosTransferencia.Aluno, dadosTransferencia.TurmaDestino, dadosTransferencia.Ano, dadosTransferencia.SemestreDestino))
                    {
                        mensagens.Add("Transferência cancelada. O aluno já possui registro de transferência para esta turma na data de hoje.");
                    }

                    //Busca turno Turma Atendimento Especializado - 9999.04 
                    string turnoEducacaoEspecial = rnMatricula.RetornaTurnoMatriculaCursoPor(contexto, Convert.ToDecimal(dadosTransferencia.Ano), Convert.ToDecimal(dadosTransferencia.SemestreDestino), dadosTransferencia.Aluno, "9999.04");

                    //Verifica turma com Turma Atendimento Especializado
                    if (dadosTransferencia.TurnoDestino != "I" &&
                        !turnoEducacaoEspecial.IsNullOrEmptyOrWhiteSpace()
                        && turnoEducacaoEspecial != dadosTransferencia.TurnoDestino)
                    {
                        mensagens.Add("O atendimento de Educacao Especial deve estar no mesmo turno que a turma regular.");
                    }

                    //Caso não existam problemas verificar se existem avisos que precisam ser mostrados para usuario confirmar
                    if (mensagens.Count == 0)
                    {
                        //Para cada disciplina verifica se existe nota ou faltas que não serão migradas para turma destino
                        foreach (string disciplinaDestino in dadosTransferencia.ListaDisciplinasTurmaDestino)
                        {
                            //Verifica se existem notas que não seram migradas
                            listaAvisosParcial = rnNota.ChecaNotasNaoMigradas(contexto, dadosTransferencia.Aluno, dadosTransferencia.TurmaAtual, dadosTransferencia.TurmaDestino, disciplinaDestino);
                            if (listaAvisosParcial.Count > 0)
                            {
                                listaAvisos.AddRange(listaAvisosParcial);
                            }

                            //Verifica se existem faltas que não seram migradas
                            listaAvisosParcial = rnFalta.ChecaFaltasNaoMigradas(contexto, dadosTransferencia.Aluno, dadosTransferencia.TurmaAtual, dadosTransferencia.TurmaDestino, disciplinaDestino);
                            if (listaAvisosParcial.Count > 0)
                            {
                                listaAvisos.AddRange(listaAvisosParcial);
                            }
                        }

                        //Verificar se a nota maxima da turma destino eh diferente da turma origem
                        avisoNotaMaximaProva = rnProva.ChecaNotaMaximaProvaDestino(contexto, dadosTransferencia.TurmaAtual,
                            Convert.ToDecimal(dadosTransferencia.Ano),
                            Convert.ToDecimal(dadosTransferencia.SemestreAtual),
                            dadosTransferencia.TurmaDestino);
                        if (!string.IsNullOrEmpty(avisoNotaMaximaProva))
                        {
                            listaAvisos.Add(avisoNotaMaximaProva);
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

        public ValidacaoDados ValidaTransferenciaTurmaOptativaReforco(LyMatricula matriculaOrigem, string turmaDestino)
        {
            DataContext contexto = null;
            List<string> mensagens = new List<string>();
            string retornoChoqueHorario = string.Empty;
            Turma turma = new Turma();
            LyMatricula matriculaDestino = new LyMatricula
               {
                   Ano = matriculaOrigem.Ano,
                   Semestre = matriculaOrigem.Semestre,
                   Disciplina = matriculaOrigem.Disciplina,
                   Aluno = matriculaOrigem.Aluno,
                   Matricula = matriculaOrigem.Matricula,
                   Turma = turmaDestino
               };

            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (string.IsNullOrEmpty(matriculaOrigem.Aluno))
            {
                mensagens.Add("O campo ALUNO é obrigatório!");
            }

            if (matriculaOrigem.Ano <= 0)
            {
                mensagens.Add("O campo ANO é obrigatório!");
            }

            if (matriculaOrigem.Semestre < 0)
            {
                mensagens.Add("O campo PERIODO é obrigatório!");
            }

            if (string.IsNullOrEmpty(matriculaOrigem.Turma))
            {
                mensagens.Add("O campo TURMA é obrigatório!");
            }

            if (string.IsNullOrEmpty(turmaDestino))
            {
                mensagens.Add("O campo TURMA DESTINO é obrigatório!");
            }
            if (!string.IsNullOrEmpty(matriculaOrigem.Turma) && !string.IsNullOrEmpty(turmaDestino) && matriculaOrigem.Turma == turmaDestino)
            {
                mensagens.Add("Turma de destino não pode ser igual à turma origem.");
            }

            if (string.IsNullOrEmpty(matriculaOrigem.Matricula))
            {
                mensagens.Add("O campo MATRICULA é obrigatório!");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    matriculaOrigem.Disciplina = turma.ObtemDisciplinaOptativaPor(contexto, matriculaOrigem.Ano, matriculaOrigem.Semestre, matriculaOrigem.Turma);

                    if (string.IsNullOrEmpty(matriculaOrigem.Disciplina))
                    {
                        mensagens.Add("Não foi encontrada disciplina para esta turma.");
                    }

                    retornoChoqueHorario = AnalisaChoqueHorarioPor(contexto, matriculaOrigem, matriculaDestino);

                    if (!string.IsNullOrEmpty(retornoChoqueHorario))
                    {
                        mensagens.Add("Encontrado choque de horário para esta turma.");
                        mensagens.Add(retornoChoqueHorario);
                    }

                    if (!turma.ExisteDisciplinaTurmaDestinoPor(contexto, matriculaOrigem.Ano, matriculaOrigem.Semestre, turmaDestino, matriculaOrigem.Disciplina))
                    {
                        mensagens.Add("Turma de destino não possui disciplina compatível.");
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

        public static ValidacaoDados ValidarTransferirProgressaoParcial(LyMatricula progressao, string turmaDestino)
        {
            RN.HistMatricula rnHistMatricula = new HistMatricula();
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (progressao == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(progressao.Aluno))
            {
                mensagens.Add("O campo ALUNO é obrigatório!");
            }

            if (progressao.Ano <= 0)
            {
                mensagens.Add("O campo ANO é obrigatório!");
            }

            if (progressao.Semestre < 0)
            {
                mensagens.Add("O campo PERIODO é obrigatório!");
            }

            if (string.IsNullOrEmpty(progressao.Turma))
            {
                mensagens.Add("O campo TURMA é obrigatório!");
            }

            if (string.IsNullOrEmpty(turmaDestino))
            {
                mensagens.Add("O campo TURMA DESTINO é obrigatório!");
            }
            if (!string.IsNullOrEmpty(progressao.Turma) && !string.IsNullOrEmpty(turmaDestino) && progressao.Turma == turmaDestino)
            {
                mensagens.Add("A TURMA DESTINO não pode ser igual a turma anterior");
            }

            if (string.IsNullOrEmpty(progressao.Disciplina))
            {
                mensagens.Add("O campo DISCIPLINA é obrigatório!");
            }

            if (progressao.SerieReferencia <= 0)
            {
                mensagens.Add("O campo SERIE REFERENCIA é obrigatório!");
            }

            if (string.IsNullOrEmpty(progressao.DisciplinaReferencia))
            {
                mensagens.Add("O campo DISCIPLINA REFERENCIA é obrigatório!");
            }

            if (string.IsNullOrEmpty(progressao.Matricula))
            {
                mensagens.Add("O campo MATRICULA é obrigatório!");
            }

            if (mensagens.Count == 0)
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    //verifica se a aluno já está matriculado naquele ano / semestre / turma /disciplina
                    var contextQuery = new ContextQuery(
                        @" SELECT  COUNT(*)
                                FROM    DBO.LY_MATRICULA M
                                WHERE   M.SIT_MATRICULA = @SIT_MATRICULA
                                        AND M.ANO = @ANO
                                        AND M.SEMESTRE = @SEMESTRE
                                        AND M.ALUNO = @ALUNO
                                        AND M.DISCIPLINA = @DISCIPLINA
                                        AND M.TURMA = @TURMA ");

                    contextQuery.Parameters.Add("@SIT_MATRICULA", Matricula.Matriculado);
                    contextQuery.Parameters.Add("@ANO", progressao.Ano);
                    contextQuery.Parameters.Add("@SEMESTRE", progressao.Semestre);
                    contextQuery.Parameters.Add("@TURMA", turmaDestino);
                    contextQuery.Parameters.Add("@DISCIPLINA", progressao.Disciplina);
                    contextQuery.Parameters.Add("@ALUNO", progressao.Aluno);

                    int matriculaExistente = ctx.GetReturnValue<int>(contextQuery);

                    if (matriculaExistente > 0)
                    {
                        mensagens.Add("Já existe matricula para este aluno nesta turma / disciplina!");
                    }

                    //verifica se a disciplina existe na turma destino
                    contextQuery = new ContextQuery(
                    @" SELECT COUNT(*) FROM dbo.LY_TURMA t
                      INNER JOIN LY_GRADE_SERIE GS ON t.ANO = GS.ANO
                                                                    AND t.SEMESTRE = GS.SEMESTRE
                                                                    AND t.TURMA = GS.GRADE
                      INNER JOIN DBO.LY_GRADE G ON GS.CURRICULO = G.CURRICULO
                                                                 AND GS.CURSO = G.CURSO
                                                                 AND t.DISCIPLINA = G.DISCIPLINA
                                                                AND GS.TURNO = G.TURNO
                                                                AND G.SERIE_IDEAL = GS.SERIE
                           WHERE   t.ANO = @ANO
                                   AND t.SEMESTRE = @SEMESTRE
                                   AND t.TURMA = @TURMA
                                   AND G.DISCIPLINA = @DISCIPLINA ");

                    contextQuery.Parameters.Add("@ANO", progressao.Ano);
                    contextQuery.Parameters.Add("@SEMESTRE", progressao.Semestre);
                    contextQuery.Parameters.Add("@TURMA", turmaDestino);
                    contextQuery.Parameters.Add("@DISCIPLINA", progressao.Disciplina);

                    var disciplinas = ctx.GetReturnValue<int>(contextQuery);

                    if (disciplinas <= 0)
                    {
                        mensagens.Add("A TURMA DESTINO não possui a disciplina para este ano / semestre");
                    }
                }

                //Verifica a mesma turma / disciplina já foi finalizada anteriormente
                if (rnHistMatricula.EhMatriculaHistoricoAtivaPor(progressao.Aluno, Convert.ToInt32(progressao.Ano), Convert.ToInt32(progressao.Semestre), progressao.Turma, progressao.Disciplina))
                {
                    mensagens.Add("Já existe matricula ativa no histórico para este aluno / ano / periodo / turma / disciplina.");
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

        private void InsereRegistroDeTransferenciaTurma(DataContext dataContext, LyMatricula matricula, string turmaDestino)
        {
            var contextQuery = new ContextQuery(
                @" INSERT  INTO dbo.LY_TURMA_TRANSF
                        ( ALUNO ,
                          DISCIPLINA ,
                          TURMA_DESTINO ,
                          ANO ,
                          PERIODO ,
                          TURMA_ANT ,
                          DATA ,
                          USUARIO ,
                          MOTIVO_TRANSF 
                        )
                VALUES  ( @ALUNO ,
                          @DISCIPLINA ,
                          @TURMADESTINO ,
                          @ANO ,
                          @PERIODO ,
                          @TURMA ,
                          GETDATE() ,
                          @MATRICULA ,
                          '01' 
                        )  ");

            contextQuery.Parameters.Add("@ALUNO", matricula.Aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", matricula.Disciplina);
            contextQuery.Parameters.Add("@TURMADESTINO", turmaDestino);
            contextQuery.Parameters.Add("@ANO", matricula.Ano);
            contextQuery.Parameters.Add("@PERIODO", matricula.Semestre);
            contextQuery.Parameters.Add("@TURMA", matricula.Turma);
            contextQuery.Parameters.Add("@MATRICULA", matricula.Matricula);

            dataContext.ApplyModifications(contextQuery);
        }

        private void InsereRegistroDeTransferenciaTurmaPor(DataContext dataContext, LyMatricula matricula, string turmaDestino, string motivo, string numChamada, string unidadeFisica)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT  INTO dbo.LY_TURMA_TRANSF
                        ( ALUNO ,
                          DISCIPLINA ,
                          TURMA_DESTINO ,
                          ANO ,
                          PERIODO ,
                          TURMA_ANT ,
                          DATA ,
                          USUARIO ,
                          MOTIVO_TRANSF,
                          NUM_CHAMADA_ANT,
                          UNIDADE_FISICA
                        )
                VALUES  ( @ALUNO ,
                          @DISCIPLINA ,
                          @TURMADESTINO ,
                          @ANO ,
                          @PERIODO ,
                          @TURMA ,
                          GETDATE() ,
                          @MATRICULA ,
                          @MOTIVO_TRANSF,
                          @NUM_CHAMADA_ANT,

                          @UNIDADE_FISICA
                        )  ";

            contextQuery.Parameters.Add("@ALUNO", matricula.Aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", matricula.Disciplina);
            contextQuery.Parameters.Add("@TURMADESTINO", turmaDestino);
            contextQuery.Parameters.Add("@ANO", matricula.Ano);
            contextQuery.Parameters.Add("@PERIODO", matricula.Semestre);
            contextQuery.Parameters.Add("@TURMA", matricula.Turma);
            contextQuery.Parameters.Add("@MATRICULA", matricula.Matricula);
            contextQuery.Parameters.Add("@MOTIVO_TRANSF", motivo);
            contextQuery.Parameters.Add("@NUM_CHAMADA_ANT", numChamada);
            contextQuery.Parameters.Add("@UNIDADE_FISICA", unidadeFisica);

            dataContext.ApplyModifications(contextQuery);
        }

        public void TransfereTurmaPrincipal(RN.DTOs.DadosTransferencia dadosTransferencia, bool realizarPermuta, RN.DTOs.DadosTransferencia dadosAlunoPermuta, out string aviso)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            aviso = string.Empty;
            RN.Matriculas.OpcaoInscricao rnOpcaoInscricao = new Techne.Lyceum.RN.Matriculas.OpcaoInscricao();
            RN.Matriculas.InscricaoAluno rnInscricaoAluno = new Techne.Lyceum.RN.Matriculas.InscricaoAluno();
            RN.DTOs.DadosEmail dadosEmail = new DadosEmail();
            RN.Turno rnTurno = new Turno();
            DadosConfirmacaoCandidato dadosCandidato = new DadosConfirmacaoCandidato();
            DateTime prazoFinal = DateTime.Now.Date;
            bool emailEnviado = false;
            Matriculas.Entidades.ConvocacaoSemEmail convocacaoSemEmail = new Techne.Lyceum.RN.Matriculas.Entidades.ConvocacaoSemEmail();
            Matriculas.ConvocacaoSemEmail rnConvocacaoSemEmail = new Techne.Lyceum.RN.Matriculas.ConvocacaoSemEmail();
            RN.Matricula rnMatricula = new Matricula();

            try
            {
                //Faz transferencia do aluno
                this.TransfereTurmaPrincipal(contexto, dadosTransferencia);

                //Verifica se será feito permuta com outro aluno da escola
                if (realizarPermuta)
                {
                    //Caso seja faz transferencia do aluno da permuta
                    this.TransfereTurmaPrincipal(contexto, dadosAlunoPermuta);
                }
                else if (dadosTransferencia.SemestreAtual != dadosTransferencia.SemestreDestino
                        || dadosTransferencia.TurnoAtual != dadosTransferencia.TurnoDestino
                        || dadosTransferencia.SerieAtual != dadosTransferencia.SerieDestino
                        || dadosTransferencia.CursoAtual != dadosTransferencia.CursoDestino)
                {
                    //Caso existe troca de turno / serie / curso e não seja permuta, convocar proximo da fila

                    //Busca dados do proximo da fila
                    dadosCandidato = rnOpcaoInscricao.ObtemDadosConfirmacaoProximoFilaPor(contexto, Convert.ToInt32(dadosTransferencia.Ano), Convert.ToInt32(dadosTransferencia.SemestreAtual), dadosTransferencia.UnidadeEnsino, dadosTransferencia.CursoAtual, Convert.ToInt32(dadosTransferencia.SerieAtual), dadosTransferencia.TurnoAtual);

                    //Verifica se exite aluno na fila
                    if (dadosCandidato.OpcaoInscricaoId > 0)
                    {
                        //Convoca proximo aluno da fila

                        //Atualiza dados da convocacao
                        rnOpcaoInscricao.AtualizaConvocacao(contexto, dadosCandidato.OpcaoInscricaoId, DateTime.Now, dadosTransferencia.UsuarioResponsavel, out prazoFinal, dadosCandidato.Municipio);

                        //Atualiza alocação 2ª fase
                        rnInscricaoAluno.AlocaFase2(contexto, dadosCandidato.InscricaoAlunoId);

                        //Busca descricao do turno
                        string descricaoturno = rnTurno.RetornaDescricaoTurno(dadosCandidato.Turno);

                        //Monta email                
                        dadosEmail.Destinatario = dadosCandidato.Email;
                        dadosEmail.Remetente = System.Configuration.ConfigurationManager.AppSettings["EmailMatriculaFase3"].ToString();
                        dadosEmail.Login = System.Configuration.ConfigurationManager.AppSettings["EmailMatriculaFase3_Login"].ToString();
                        dadosEmail.Senha = System.Configuration.ConfigurationManager.AppSettings["EmailMatriculaFase3_Senha"].ToString();
                        dadosEmail.Assunto = "Convocação Matrícula Fácil";
                        dadosEmail.Texto = string.Format(@"<br />{0}
                                            <br />Informamos que sua vaga está reservada na série {1} do Ensino {2} do {3}. Compareça de {4} a {5} na escola onde foi alocado para confirmar a sua matrícula.
                                            <br />{6}
                                           ", dadosCandidato.Nome, dadosCandidato.Serie, dadosCandidato.Segmento, dadosCandidato.Escola, DateTime.Now.ToString("dd/MM/yyyy"), prazoFinal.ToString("dd/MM/yyyy"), rnMatricula.RetornaTextoEmailConvocacaoPor(Convert.ToInt32(dadosTransferencia.Ano)));

                        //tentar Enviar e-mail
                        try
                        {
                            //Envia e-mail
                            RN.Util.Email.Envia(dadosEmail);
                            emailEnviado = true;
                        }
                        catch (Exception)
                        {
                            emailEnviado = false;
                        }

                        //Verifica se não foi possivel enviar o e-mail
                        if (!emailEnviado)
                        {
                            aviso = string.Format("Próximo candidato da fila convocado com falha no envio do email, favor entrar em contado com a candidato: {0} - Nome: {1}, Email: {2}, Telefone {3}",
                            Convert.ToString(dadosCandidato.NumeroInscricao),
                            dadosCandidato.Nome,
                            dadosCandidato.Email,
                            dadosCandidato.Telefone
                            );

                            //Monta entidade de email não enviado
                            convocacaoSemEmail.InscricaoAlunoId = dadosCandidato.InscricaoAlunoId;
                            convocacaoSemEmail.OpcaoInscricaoId = dadosCandidato.OpcaoInscricaoId;
                            convocacaoSemEmail.UsuarioResponsavel = dadosTransferencia.UsuarioResponsavel;
                            convocacaoSemEmail.DataAviso = DateTime.Now;

                            rnConvocacaoSemEmail.Insere(contexto, convocacaoSemEmail);
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

        public void TransfereTurmaPrincipal(DataContext ctx, RN.DTOs.DadosTransferencia dadosTransferencia)
        {
            RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            RN.Matricula rnMatricula = new Matricula();
            RN.Falta rnFalta = new Falta();
            RN.Disciplina rnDisciplina = new Disciplina();
            Matgrade rnMatGrade = new Matgrade();
            string tipoVagaOcupada = string.Empty;
            string disciplina = string.Empty;
            LyMatricula matriculaOrigem = new LyMatricula();
            string num = null;
            bool possuiInconsistenciaFalta = false;

            foreach (string disciplinaDestino in dadosTransferencia.ListaDisciplinasTurmaDestino)
            {
                disciplina = disciplinaDestino;

                //Verifica se a disciplina não é uma eletiva com enturmação separada
                if (!rnDisciplina.EhDisciplinaGradeEletivaPor(ctx, Convert.ToDecimal(dadosTransferencia.Ano), Convert.ToDecimal(dadosTransferencia.SemestreDestino), dadosTransferencia.TurmaDestino, disciplina))
                {
                    //Cancela Turma Atual
                    rnMatricula.CancelaMatriculaPor(ctx, dadosTransferencia.Aluno, dadosTransferencia.TurmaAtual, Convert.ToDecimal(dadosTransferencia.Ano), Convert.ToDecimal(dadosTransferencia.SemestreAtual), dadosTransferencia.UsuarioResponsavel);

                    //Matricula aluno na turma de destino
                    ctx.ApplyModifications(
                        new ContextQuery(
                                @" IF NOT EXISTS(SELECT * 
                                                  FROM   LY_MATRICULA 
                                                  WHERE  ALUNO = @ALUNO 
                                                         AND ANO = @ANO 
                                                         AND SEMESTRE = @SEMESTRE 
                                                         AND DISCIPLINA = @DISCIPLINA 
                                                         AND TURMA = @TURMA) 
                                      BEGIN 
                                          INSERT INTO LY_MATRICULA 
                                                      (ALUNO, 
                                                       DISCIPLINA, 
                                                       TURMA, 
                                                       ANO, 
                                                       SEMESTRE, 
                                                       SIT_MATRICULA, 
                                                       DT_ULTALT, 
                                                       COBRANCA_SEP, 
                                                       DT_INSERCAO, 
                                                       DT_MATRICULA) 
                                          VALUES      (@ALUNO, 
                                                       @DISCIPLINA, 
                                                       @TURMA, 
                                                       @ANO, 
                                                       @SEMESTRE, 
                                                       'Matriculado', 
                                                       GETDATE(), 
                                                       'N', 
                                                       GETDATE(), 
                                                       GETDATE()) 
                                      END 
                                    ELSE 
                                      BEGIN 
                                          UPDATE LY_MATRICULA 
                                          SET    DT_ULTALT = GETDATE(), 
                                                 DT_MATRICULA = GETDATE(),
                                                 CONCOMITANTE = 'N',
                                                 DEPENDENCIA = 'N',
                                                 SERIE_REFERENCIA = NULL, 
                                                 DISCIPLINA_REFERENCIA = NULL, 
                                                 EDUC_ESPECIAL = 'N',
                                                 MAIS_EDUCACAO = 'N', 
                                                 SIT_MATRICULA = 'Matriculado' 
                                          WHERE  ALUNO = @ALUNO 
                                                 AND ANO = @ANO 
                                                 AND SEMESTRE = @SEMESTRE 
                                                 AND DISCIPLINA = @DISCIPLINA 
                                                 AND TURMA = @TURMA 
                                      END  ",
                      new ContextQueryParameter("@ALUNO", dadosTransferencia.Aluno),
                      new ContextQueryParameter("@ANO", dadosTransferencia.Ano),
                      new ContextQueryParameter("@SEMESTRE", dadosTransferencia.SemestreDestino),
                      new ContextQueryParameter("@DISCIPLINA", disciplina),
                      new ContextQueryParameter("@TURMA", dadosTransferencia.TurmaDestino)));

                    //Monta dados de origem para migração de notas e faltas
                    matriculaOrigem.Ano = Convert.ToInt32(dadosTransferencia.Ano);
                    matriculaOrigem.Semestre = Convert.ToInt32(dadosTransferencia.SemestreAtual);
                    matriculaOrigem.Turma = dadosTransferencia.TurmaAtual;
                    matriculaOrigem.Matricula = dadosTransferencia.UsuarioResponsavel;
                    matriculaOrigem.Aluno = dadosTransferencia.Aluno;
                    matriculaOrigem.Disciplina = disciplina;

                    //Verifica se o aluno possui, em qualquer bimestre, numero de faltas maior que de aulas dadas da turma de destino
                    possuiInconsistenciaFalta = rnFalta.PossuiInconsistenciaFaltaAulasDadasPor(ctx, matriculaOrigem, dadosTransferencia.TurmaDestino);

                    //Apenas migrar notas e faltas caso nao exista inconsistencia nas faltas e auldas dadas do destino
                    if (!possuiInconsistenciaFalta)
                    {
                        Nota.MigrarNotas(ctx, matriculaOrigem, dadosTransferencia.TurmaDestino);
                        Falta.MigrarFaltas(ctx, matriculaOrigem, dadosTransferencia.TurmaDestino);
                    }

                    if (!string.IsNullOrEmpty(dadosTransferencia.NumChamadaAtual))
                    {
                        num = dadosTransferencia.NumChamadaAtual;
                    }

                    this.InsereRegistroDeTransferenciaTurmaPor(ctx, matriculaOrigem, dadosTransferencia.TurmaDestino, dadosTransferencia.MotivoTransferencia, num, dadosTransferencia.UnidadeFisica);
                }
            }

            //Atualiza dados do aluno
            ctx.ApplyModifications(
               new ContextQuery(
               @" UPDATE LY_ALUNO 
                        SET    CURSO = @CURSO, 
                               TURNO = @TURNO, 
                               CURRICULO = @CURRICULO, 
                               SERIE = @SERIE, 
                               TIPO_ENSINO_PROFISSIONALIZANTE = @TIPO_ENSINO_PROFISSIONALIZANTE 
                        WHERE  ALUNO = @ALUNO   ",
              new ContextQueryParameter("@ALUNO", dadosTransferencia.Aluno),
              new ContextQueryParameter("@CURSO", dadosTransferencia.CursoDestino),
              new ContextQueryParameter("@TURNO", dadosTransferencia.TurnoDestino),
              new ContextQueryParameter("@CURRICULO", dadosTransferencia.CurriculoDestino),
              new ContextQueryParameter("@SERIE", dadosTransferencia.SerieDestino),
              new ContextQueryParameter("@TIPO_ENSINO_PROFISSIONALIZANTE", dadosTransferencia.TipoEnsProfissionalizanteDestino)));

            //VERIFICA SE DESTINO TERÁ OPTATIVA ENSINO RELIGIOSO
            if (!Convert.ToBoolean(dadosTransferencia.EnsinoReligioso))
            {
                //Caso não possua retirar as optativas ensino religioso existentes para o aluno
                ctx.ApplyModifications(
                  new ContextQuery(
                  @" UPDATE  M
                            SET     SIT_MATRICULA = @SIT_MATRICULA_CANCELADO ,
                                    STAMP_ATUALIZACAO = GETDATE() ,
                                    DT_ULTALT = GETDATE() ,
                                    MATRICULA = @MATRICULA
                            FROM    DBO.LY_MATRICULA M
                                    INNER JOIN DBO.LY_TURMA T ON M.DISCIPLINA = T.DISCIPLINA
                                                                    AND M.TURMA = T.TURMA
                                                                    AND M.ANO = T.ANO
                                                                    AND M.SEMESTRE = T.SEMESTRE
                            WHERE   M.ALUNO = @ALUNO
                                    AND M.ANO = @ANO
                                    AND M.SEMESTRE = @SEMESTRE
                                    AND T.OPTATIVAREFORCO = 'R'
                                    AND M.SIT_MATRICULA = @SIT_MATRICULA_MATRICULADO ",
                 new ContextQueryParameter("@SIT_MATRICULA_CANCELADO", Matricula.Cancelado),
                 new ContextQueryParameter("@MATRICULA", dadosTransferencia.UsuarioResponsavel),
                 new ContextQueryParameter("@ALUNO", dadosTransferencia.Aluno),
                 new ContextQueryParameter("@ANO", dadosTransferencia.Ano),
                 new ContextQueryParameter("@SEMESTRE", dadosTransferencia.SemestreAtual),
                 new ContextQueryParameter("@SIT_MATRICULA_MATRICULADO", Matricula.Matriculado)));
            }

            //VERIFICA SE DESTINO TERÁ OPTATIVA LINGUA ESTRANGEIRA
            if (!Convert.ToBoolean(dadosTransferencia.LinguaEstrangeira))
            {
                //Caso não possua retirar as optativas Lingua estrangeira existentes para o aluno
                ctx.ApplyModifications(
                 new ContextQuery(
                 @" UPDATE  M
                            SET     SIT_MATRICULA = @SIT_MATRICULA_CANCELADO ,
                                    STAMP_ATUALIZACAO = GETDATE() ,
                                    DT_ULTALT = GETDATE() ,
                                    MATRICULA = @MATRICULA
                            FROM    DBO.LY_MATRICULA M
                                    INNER JOIN DBO.LY_TURMA T ON M.DISCIPLINA = T.DISCIPLINA
                                                                    AND M.TURMA = T.TURMA
                                                                    AND M.ANO = T.ANO
                                                                    AND M.SEMESTRE = T.SEMESTRE
                            WHERE   M.ALUNO = @ALUNO
                                    AND M.ANO = @ANO
                                    AND M.SEMESTRE = @SEMESTRE
                                    AND T.OPTATIVAREFORCO = 'L'
                                    AND M.SIT_MATRICULA = @SIT_MATRICULA_MATRICULADO ",
                new ContextQueryParameter("@SIT_MATRICULA_CANCELADO", Matricula.Cancelado),
                new ContextQueryParameter("@MATRICULA", dadosTransferencia.UsuarioResponsavel),
                new ContextQueryParameter("@ALUNO", dadosTransferencia.Aluno),
                new ContextQueryParameter("@ANO", dadosTransferencia.Ano),
                new ContextQueryParameter("@SEMESTRE", dadosTransferencia.SemestreAtual),
                new ContextQueryParameter("@SIT_MATRICULA_MATRICULADO", Matricula.Matriculado)));
            }

            //Cancela as matriculas eletivas existentes para o aluno
            ctx.ApplyModifications(
                 new ContextQuery(
                 @" UPDATE  M
                            SET     SIT_MATRICULA = @SIT_MATRICULA_CANCELADO ,
                                    STAMP_ATUALIZACAO = GETDATE() ,
                                    DT_ULTALT = GETDATE() ,
                                    MATRICULA = @MATRICULA
                            FROM    DBO.LY_MATRICULA M
                                    INNER JOIN DBO.LY_TURMA T ON M.DISCIPLINA = T.DISCIPLINA
                                                                    AND M.TURMA = T.TURMA
                                                                    AND M.ANO = T.ANO
                                                                    AND M.SEMESTRE = T.SEMESTRE
                            WHERE   M.ALUNO = @ALUNO
                                    AND M.ANO = @ANO
                                    AND M.SEMESTRE = @SEMESTRE
                                    AND T.ELETIVA = 'S'
                                    AND M.SIT_MATRICULA = @SIT_MATRICULA_MATRICULADO ",
                new ContextQueryParameter("@SIT_MATRICULA_CANCELADO", Matricula.Cancelado),
                new ContextQueryParameter("@MATRICULA", dadosTransferencia.UsuarioResponsavel),
                new ContextQueryParameter("@ALUNO", dadosTransferencia.Aluno),
                new ContextQueryParameter("@ANO", dadosTransferencia.Ano),
                new ContextQueryParameter("@SEMESTRE", dadosTransferencia.SemestreAtual),
                new ContextQueryParameter("@SIT_MATRICULA_MATRICULADO", Matricula.Matriculado)));

            //Cancela LY_MATGRADE de matriculas canceladas
            if (!Convert.ToBoolean(dadosTransferencia.EnsinoReligioso) || !Convert.ToBoolean(dadosTransferencia.LinguaEstrangeira))
            {
                ctx.ApplyModifications(
               new ContextQuery(
               @" UPDATE  LY_MATGRADE
                            SET     SIT_MATGRADE = @SIT_MATGRADE_CANCELADO
                            FROM    LY_MATGRADE MG
                                    INNER JOIN LY_GRADE_SERIE GS ( NOLOCK ) ON MG.GRADE_ID = GS.GRADE_ID
                            WHERE   MG.ALUNO = @ALUNO
                                    AND MG.SIT_MATGRADE = @SIT_MATRICULADO
                                    AND GS.ANO = @ANO
                                    AND gs.SEMESTRE = @SEMESTRE
                                    AND NOT EXISTS ( SELECT TOP 1
                                                            1
                                                        FROM   LY_MATRICULA M
                                                            INNER JOIN DBO.LY_GRADE_SERIE GS2 ( NOLOCK ) ON GS2.GRADE = M.TURMA
                                                                                            AND GS2.ANO = M.ANO
                                                                                            AND GS2.SEMESTRE = M.SEMESTRE
                                                        WHERE  GS2.GRADE = GS.GRADE
                                                            AND GS2.ANO = M.ANO
                                                            AND GS2.SEMESTRE = M.SEMESTRE
                                                            AND MG.ALUNO = M.ALUNO
                                                            AND SIT_MATRICULA = @SIT_MATRICULADO ) ",
                new ContextQueryParameter("@SIT_MATGRADE_CANCELADO", Matricula.Cancelado),
                new ContextQueryParameter("@ALUNO", dadosTransferencia.Aluno),
                new ContextQueryParameter("@SIT_MATRICULADO", Matricula.Matriculado),
                new ContextQueryParameter("@ANO", dadosTransferencia.Ano),
                new ContextQueryParameter("@SEMESTRE", dadosTransferencia.SemestreAtual)));
            }

            if (dadosTransferencia.SemestreAtual != dadosTransferencia.SemestreDestino)
            {
                //CASO O ALUNO TROQUE DE PERIODO, BUSCA DADOS DA CONFIRMAÇÃO DA TURMA ORIGEM
                var conf = rnConfirmacaoMatricula.ObtemConfirmacaoMatriculaPor(ctx, dadosTransferencia.Aluno, Convert.ToDecimal(dadosTransferencia.Ano),
                           Convert.ToDecimal(dadosTransferencia.SemestreAtual));

                if (conf.Rows.Count > 0)
                {
                    //CANCELA A CONFIRMACAO DA TURMA ATUAL E OBTEM TIPO VAGA
                    tipoVagaOcupada = conf.Rows[0]["TIPOVAGAOCUPADA"].ToString();

                    ctx.ApplyModifications(
                        new ContextQuery(
                        @" UPDATE  TCE_CONFIRMACAO_MATRICULA
                                SET     STATUS = 'Não Confirmado',
                                        MATRICULA = @MATRICULA,                                            
                                        DT_ALTERACAO = GETDATE(),
                                        OBSERVACAO='NÃO CONFIRMAÇÃO REALIZADA POR TRANSFERENCIA DE TURMAS ENTRE PERÍODOS'
                                WHERE   ALUNO = @ALUNO 
	                                AND ANO = @ANO 
	                                AND PERIODO = @PERIODO 
	                                AND ([STATUS] = 'Confirmado' or [STATUS] is null) ",
                       new ContextQueryParameter("@MATRICULA", dadosTransferencia.UsuarioResponsavel),
                       new ContextQueryParameter("@ALUNO", dadosTransferencia.Aluno),
                       new ContextQueryParameter("@ANO", Convert.ToDecimal(dadosTransferencia.Ano)),
                       new ContextQueryParameter("@PERIODO", Convert.ToDecimal(dadosTransferencia.SemestreAtual))));
                }
            }

            //VERIFICA A CONFIRMACAO COM OS DADOS DA TURMA DE DESTINO
            var confDestino = ConfirmacaoMatricula.VerificaConfirmacaoMatriculaDestino(dadosTransferencia);
            int idConfirmacaoCorreta = 0;
            if (confDestino.Rows.Count > 0)
            {
                idConfirmacaoCorreta = Convert.ToInt32(confDestino.Rows[0]["ID_CONFIRMACAO_MATRICULA"]);
            }

            //Cancela possiveis
            rnConfirmacaoMatricula.CancelaOutrasPossiveisConfirmacaoMatriculaPor(ctx, dadosTransferencia.Aluno, Convert.ToInt32(dadosTransferencia.Ano), dadosTransferencia.UsuarioResponsavel, Convert.ToInt32(dadosTransferencia.SemestreDestino), idConfirmacaoCorreta);

            if (idConfirmacaoCorreta == 0)
            {
                ctx.ApplyModifications(
                    new ContextQuery(
                    @"INSERT INTO TCE_CONFIRMACAO_MATRICULA
                                ( ALUNO ,
                                    CENSO ,
                                    ANO ,
                                    PERIODO ,
                                    CURSO ,
                                    SERIE ,
                                    TURNO ,
                                    DT_SUGERIDA ,
                                    CURRICULO ,
                                    ENSINO_RELIGIOSO ,
                                    LINGUA_ESTRANGEIRA_FACULTATIVA ,
                                    PROJETO_AUTONOMIA ,
                                    MATRICULA ,
                                    STATUS ,
                                    DT_ALTERACAO ,
                                    TIPOVAGAOCUPADA
                                )
                            VALUES 
                            (
		                            @ALUNO ,
                                    @CENSO ,
                                    @ANO ,
                                    @PERIODO ,
                                    @CURSO ,
                                    @SERIE ,
                                    @TURNO ,
                                    GETDATE() ,
                                    @CURRICULO ,
                                    @ENSINO_RELIGIOSO ,
                                    @LINGUA_ESTRANGEIRA_FACULTATIVA ,
                                    0 ,
                                    @MATRICULA ,
                                    'Confirmado' ,
                                    GETDATE() ,
                                    @TIPOVAGAOCUPADA
                            ) ",
                   new ContextQueryParameter("@ALUNO", dadosTransferencia.Aluno),
                   new ContextQueryParameter("@CENSO", dadosTransferencia.UnidadeEnsino),
                   new ContextQueryParameter("@ANO", dadosTransferencia.Ano),
                   new ContextQueryParameter("@PERIODO", dadosTransferencia.SemestreDestino),
                   new ContextQueryParameter("@CURSO", dadosTransferencia.CursoDestino),
                   new ContextQueryParameter("@SERIE", dadosTransferencia.SerieDestino),
                   new ContextQueryParameter("@TURNO", dadosTransferencia.TurnoDestino),
                   new ContextQueryParameter("@CURRICULO", dadosTransferencia.CurriculoDestino),
                   new ContextQueryParameter("@ENSINO_RELIGIOSO", dadosTransferencia.EnsinoReligioso),
                   new ContextQueryParameter("@LINGUA_ESTRANGEIRA_FACULTATIVA", dadosTransferencia.LinguaEstrangeira),
                   new ContextQueryParameter("@MATRICULA", dadosTransferencia.UsuarioResponsavel),
                   new ContextQueryParameter("@TIPOVAGAOCUPADA", tipoVagaOcupada)));
            }
            else
            {
                //ATUALIZA A CONFIRMAÇÃO DE MATRICULA COM OS DADOS DA TURMA NOVA
                ctx.ApplyModifications(
                     new ContextQuery(
                     @" UPDATE  TCE_CONFIRMACAO_MATRICULA
                                SET     MATRICULA = @MATRICULA,                                            
                                        STATUS = 'Confirmado',
                                        LINGUA_ESTRANGEIRA_FACULTATIVA = @LINGUA_ESTRANGEIRA_FACULTATIVA,
                                        ENSINO_RELIGIOSO = @ENSINO_RELIGIOSO,
                                        DT_ALTERACAO = GETDATE()
                                WHERE   ID_CONFIRMACAO_MATRICULA = @ID_CONFIRMACAO_MATRICULA ",

                    new ContextQueryParameter("@MATRICULA", dadosTransferencia.UsuarioResponsavel),
                    new ContextQueryParameter("@LINGUA_ESTRANGEIRA_FACULTATIVA", dadosTransferencia.LinguaEstrangeira),
                    new ContextQueryParameter("@ENSINO_RELIGIOSO", dadosTransferencia.EnsinoReligioso),
                    new ContextQueryParameter("@ID_CONFIRMACAO_MATRICULA", idConfirmacaoCorreta)));
            }

            //Cria MATGRADE da turma destino
            rnMatGrade.Transfere(ctx, dadosTransferencia.Aluno, dadosTransferencia.GradeIdDestino, dadosTransferencia.GradeIdAtual);
        }

        public void TransfereTurmaOptativaReforco(LyMatricula matriculaOrigem, string turmaDestino)
        {
            Matricula matriculaRN = new Matricula();
            LyMatricula matriculaDestino = new LyMatricula();
            Matgrade matgradeRN = new Matgrade();
            LyTurma dadosTurmaDestino = new LyTurma();
            LyTurma dadosTurmaOrigem = new LyTurma();
            string gradeIdDestino = string.Empty;
            string gradeIdOrigem = string.Empty;

            using (var dataContext = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    //Monta dados de destino
                    matriculaDestino.Aluno = matriculaOrigem.Aluno;
                    matriculaDestino.Ano = matriculaOrigem.Ano;
                    matriculaDestino.Semestre = matriculaOrigem.Semestre;
                    matriculaDestino.Disciplina = matriculaOrigem.Disciplina;
                    matriculaDestino.Turma = turmaDestino;
                    matriculaDestino.SitMatricula = matriculaOrigem.SitMatricula;
                    matriculaDestino.Matricula = matriculaOrigem.Matricula;


                    matriculaRN.InsereOuAtualizaMatriculaOptativaReforco(dataContext, matriculaDestino);

                    Nota.MigrarNotas(dataContext, matriculaOrigem, turmaDestino);

                    Falta.MigrarFaltas(dataContext, matriculaOrigem, turmaDestino);

                    matriculaRN.RemoveMatriculaOptativaReforco(dataContext, matriculaOrigem);

                    this.InsereRegistroDeTransferenciaTurma(dataContext, matriculaOrigem, turmaDestino);

                    dadosTurmaDestino = RN.Turma.Carregar(Convert.ToInt32(matriculaDestino.Ano),
                                                    Convert.ToInt32(matriculaDestino.Semestre),
                                                    matriculaDestino.Turma);

                    gradeIdDestino = GradeSerie.ObterGradeId(
                        dataContext,
                        matriculaDestino.Ano,
                        matriculaDestino.Semestre,
                        dadosTurmaDestino.Curso,
                        dadosTurmaDestino.Curriculo,
                        dadosTurmaDestino.Serie,
                        matriculaDestino.Turma);

                    dadosTurmaOrigem = RN.Turma.Carregar(Convert.ToInt32(matriculaDestino.Ano),
                                                    Convert.ToInt32(matriculaDestino.Semestre),
                                                   matriculaOrigem.Turma);

                    gradeIdOrigem = GradeSerie.ObterGradeId(
                        dataContext,
                        matriculaDestino.Ano,
                        matriculaDestino.Semestre,
                        dadosTurmaOrigem.Curso,
                        dadosTurmaOrigem.Curriculo,
                        dadosTurmaOrigem.Serie,
                        matriculaOrigem.Turma);

                    matgradeRN.Transfere(dataContext, matriculaDestino.Aluno, gradeIdDestino, gradeIdOrigem);

                }
                catch (Exception exception)
                {
                    string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine, Convert.ToString(exception.Message));
                    throw new Exception(mensagem);
                }
            }
        }

        public static void TransferirProgressaoParcial(LyMatricula matriculaOrigem, string turmaDestino)
        {
            LyMatricula matriculaDestino = new LyMatricula();
            LyTurma lyTurma = new LyTurma();
            LyTurma lyTurmaOrigem = new LyTurma();
            string gradeId = string.Empty;
            string gradeIdOrigem = string.Empty;
            Matgrade rnMatgrade = new Matgrade();

            using (var context = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    //Monta dados de destino
                    matriculaDestino.Aluno = matriculaOrigem.Aluno;
                    matriculaDestino.Ano = matriculaOrigem.Ano;
                    matriculaDestino.Semestre = matriculaOrigem.Semestre;
                    matriculaDestino.Disciplina = matriculaOrigem.Disciplina;
                    matriculaDestino.Turma = turmaDestino;
                    matriculaDestino.SitMatricula = matriculaOrigem.SitMatricula;
                    matriculaDestino.Matricula = matriculaOrigem.Matricula;
                    matriculaDestino.Dependencia = "S";
                    matriculaDestino.SerieReferencia = matriculaOrigem.SerieReferencia;
                    matriculaDestino.DisciplinaReferencia = matriculaOrigem.DisciplinaReferencia;

                    //verificar se ja existe um deles cancelado na ly_matricula
                    var contextQuery = new ContextQuery(
                        @" SELECT  COUNT(*)
                        FROM    LY_MATRICULA
                        WHERE   ALUNO = @ALUNO
                                AND DISCIPLINA = @DISCIPLINA
                                AND TURMA = @TURMA
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE
                                AND SIT_MATRICULA = 'Cancelado' ");

                    contextQuery.Parameters.Add("@ALUNO", matriculaDestino.Aluno);
                    contextQuery.Parameters.Add("@DISCIPLINA", matriculaDestino.Disciplina);
                    contextQuery.Parameters.Add("@TURMA", matriculaDestino.Turma);
                    contextQuery.Parameters.Add("@ANO", matriculaDestino.Ano);
                    contextQuery.Parameters.Add("@SEMESTRE", matriculaDestino.Semestre);

                    var idCancelada = context.GetReturnValue<int>(contextQuery);

                    //se existir atualiza senao insere
                    if (idCancelada > 0)
                    {
                        Matricula.AtualizarProgressaoParcial(context, matriculaDestino);
                    }
                    else
                    {
                        Matricula.InserirProgressaoParcial(context, matriculaDestino);
                    }

                    Nota.MigrarNotas(context, matriculaOrigem, turmaDestino);

                    Falta.MigrarFaltas(context, matriculaOrigem, turmaDestino);

                    Matricula.RemoverProgressaoParcial(context, matriculaOrigem);

                    context.ApplyModifications(
                    new ContextQuery(
                    @" INSERT  INTO dbo.LY_TURMA_TRANSF ( ALUNO, DISCIPLINA, TURMA_DESTINO, ANO,
                                   PERIODO, TURMA_ANT, DATA, USUARIO,
                                   MOTIVO_TRANSF )
                       VALUES  ( @ALUNO, @DISCIPLINA, @TURMADESTINO, @ANO, @PERIODO, @TURMA,
                                  GETDATE(), @MATRICULA, '01' ) ",
                   new ContextQueryParameter("@ALUNO", matriculaOrigem.Aluno),
                   new ContextQueryParameter("@DISCIPLINA", matriculaOrigem.Disciplina),
                   new ContextQueryParameter("@TURMADESTINO", turmaDestino),
                   new ContextQueryParameter("@ANO", matriculaOrigem.Ano),
                   new ContextQueryParameter("@PERIODO", matriculaOrigem.Semestre),
                   new ContextQueryParameter("@TURMA", matriculaOrigem.Turma),
                   new ContextQueryParameter("@MATRICULA", matriculaOrigem.Matricula)));

                    //carregar dados da turma destino
                    lyTurma = RN.Turma.Carregar(Convert.ToInt32(matriculaDestino.Ano),
                                                    Convert.ToInt32(matriculaDestino.Semestre),
                                                    turmaDestino);
                    //obter grade_id destino
                    gradeId = GradeSerie.ObterGradeId(
                        context,
                        matriculaDestino.Ano,
                        matriculaDestino.Semestre,
                        lyTurma.Curso,
                        lyTurma.Curriculo,
                        lyTurma.Serie,
                        matriculaDestino.Turma);

                    //carregar dados da turma origem
                    lyTurmaOrigem = RN.Turma.Carregar(Convert.ToInt32(matriculaOrigem.Ano),
                                                    Convert.ToInt32(matriculaOrigem.Semestre),
                                                    matriculaOrigem.Turma);

                    //obter grade_id origem
                    gradeIdOrigem = GradeSerie.ObterGradeId(
                        context,
                        matriculaOrigem.Ano,
                        matriculaOrigem.Semestre,
                        lyTurmaOrigem.Curso,
                        lyTurmaOrigem.Curriculo,
                        lyTurmaOrigem.Serie,
                        matriculaOrigem.Turma);

                    rnMatgrade.AtualizaProgressaoParcial(context, matriculaDestino.Aluno, Convert.ToDecimal(gradeId), Convert.ToDecimal(gradeIdOrigem));
                }
                catch (Exception)
                {
                    context.Abandon();
                    throw;
                }
            }
        }

        public ValidacaoDados ValidarTransConcomitante(LyMatricula matriculaDestino, LyMatricula matriculaRegular, string turno)
        {
            DataContext contexto = null;
            RN.Matricula rnMatricula = new Matricula();
            LyMatricula matriculaOrigem = new LyMatricula();
            RN.Turma rnTurma = new Turma();
            int vagas = 0;
            string choqueHorario = string.Empty;
            string turmaAnterior = string.Empty;
            string turnoTurmaRegular = string.Empty;
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (matriculaDestino == null)
            {
                return validacaoDados;
            }

            matriculaDestino.Concomitante = "S";
            matriculaDestino.SitMatricula = "Matriculado";

            if (string.IsNullOrEmpty(matriculaDestino.Aluno))
            {
                mensagens.Add("O campo ALUNO é obrigatório!");
            }

            if (string.IsNullOrEmpty(matriculaDestino.Turma))
            {
                mensagens.Add("O campo Turma Destino(Turma Profissionalizante) é obrigatório!");
            }

            if (matriculaDestino.Ano <= 0)
            {
                mensagens.Add("O campo Ano de Escolaridade(Turma Profissionalizante) é obrigatório!");
            }

            if (matriculaDestino.Semestre < 0)
            {
                mensagens.Add("O campo Período de destino(Turma Profissionalizante) é obrigatório!");
            }

            if (string.IsNullOrEmpty(matriculaDestino.Matricula))
            {
                mensagens.Add("O campo MATRICULA é obrigatório!");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    turmaAnterior = rnMatricula.ObtemTurmaMatriculaConcomitantePor(contexto, matriculaDestino.Ano, matriculaDestino.Semestre, matriculaDestino.Aluno);

                    if (string.IsNullOrEmpty(turmaAnterior))
                    {
                        mensagens.Add("Este aluno não possui matrícula em turma de curso concomitante!");
                    }

                    if (matriculaDestino.Turma == turmaAnterior)
                    {
                        mensagens.Add("A turma de destino não pode ser a mesma turma de origem!");
                    }

                    //Verifica se a turma tem vaga
                    vagas = rnTurma.ObtemVagasPrincipalLiberadasTurmaPor(contexto, Convert.ToInt32(matriculaDestino.Ano), Convert.ToInt32(matriculaDestino.Semestre), matriculaDestino.Turma);
                    if (vagas <= 0)
                    {
                        mensagens.Add("A capacidade da turma desejada nao comporta mais alunos.");
                    }

                    matriculaOrigem.Turma = turmaAnterior;

                    //Analisa Choque de Horários
                    choqueHorario = AnalisaChoqueHorarioPor(contexto, matriculaOrigem, matriculaDestino);

                    if (!string.IsNullOrEmpty(choqueHorario))
                    {
                        mensagens.Add("Transferência cancelada na verificação do choque de horário.");
                        mensagens.Add(choqueHorario);
                    }

                    //obter turno turma regular
                    turnoTurmaRegular = rnTurma.ObtemTurnoTurmaPor(contexto, matriculaRegular.Ano, matriculaRegular.Semestre, matriculaRegular.Turma);

                    if (!RN.Turno.VerificarContraTurno(turno, turnoTurmaRegular))
                    {
                        mensagens.Add("A Turma Profissional Concomitante deve estar em contraturno com a turma regular.");
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

        public static void TransferirTurmaConcomitante(LyMatricula matriculaDestino)
        {
            using (var context = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    //carregar dados da turma concomitante anterior.
                    var matriculaAtual = Matricula.CarregarMatriculaConcomitante(matriculaDestino.Aluno, Convert.ToInt32(matriculaDestino.Ano), Convert.ToInt32(matriculaDestino.Semestre));

                    //carregar disciplinas da turma destino
                    var disciplinas = Matricula.ConsultaDisciplinaGrade(Convert.ToString(matriculaDestino.Ano), Convert.ToString(matriculaDestino.Semestre), matriculaDestino.Turma);

                    foreach (DataRow disciplinaRow in disciplinas.Rows)
                    {
                        matriculaDestino.Disciplina = disciplinaRow["disciplina"].ToString();
                        matriculaAtual.Disciplina = disciplinaRow["disciplina"].ToString();
                        matriculaAtual.Matricula = matriculaDestino.Matricula;

                        //verificar se ja existe um deles cancelado na ly_matricula
                        var contextQuery = new ContextQuery(
                            @" SELECT  COUNT(*)
                                FROM    LY_MATRICULA
                                WHERE   ALUNO = @ALUNO
                                        AND DISCIPLINA = @DISCIPLINA
                                        AND TURMA = @TURMA
                                        AND ANO = @ANO
                                        AND SEMESTRE = @SEMESTRE
                                        AND SIT_MATRICULA = 'Cancelado' ");

                        contextQuery.Parameters.Add("@ALUNO", matriculaDestino.Aluno);
                        contextQuery.Parameters.Add("@DISCIPLINA", matriculaDestino.Disciplina);
                        contextQuery.Parameters.Add("@TURMA", matriculaDestino.Turma);
                        contextQuery.Parameters.Add("@ANO", matriculaDestino.Ano);
                        contextQuery.Parameters.Add("@SEMESTRE", matriculaDestino.Semestre);

                        var canceladas = context.GetReturnValue<int>(contextQuery);

                        //se existir atualiza senao insere
                        if (canceladas > 0)
                        {
                            Matricula.AtualizarEnsProfConcomitante(context, matriculaDestino);
                        }
                        else
                        {
                            Matricula.InserirEnsProfConcomitante(context, matriculaDestino);
                        }

                        //Transferir Notas
                        Nota.MigrarNotas(context, matriculaAtual, matriculaDestino.Turma);

                        //Transferir Faltas
                        Falta.MigrarFaltas(context, matriculaAtual, matriculaDestino.Turma);

                        //Atualizar a Situacao na matricula para cancelado da turma concomitante anterir
                        Matricula.RemoverEnsProfConcomitante(context, matriculaAtual);

                        //Insere transferencia na tabela Ly_turma_transf
                        context.ApplyModifications(
                            new ContextQuery(
                                @" INSERT  INTO dbo.LY_TURMA_TRANSF ( ALUNO, DISCIPLINA, TURMA_DESTINO, ANO,
                                               PERIODO, TURMA_ANT, DATA, USUARIO,
                                               MOTIVO_TRANSF )
                                   VALUES  ( @ALUNO, @DISCIPLINA, @TURMADESTINO, @ANO, @PERIODO, @TURMA,
                                              GETDATE(), @MATRICULA, '01' ) ",
                                new ContextQueryParameter("@ALUNO", matriculaAtual.Aluno),
                                new ContextQueryParameter("@DISCIPLINA", matriculaAtual.Disciplina),
                                new ContextQueryParameter("@TURMADESTINO", matriculaDestino.Turma),
                                new ContextQueryParameter("@ANO", matriculaAtual.Ano),
                                new ContextQueryParameter("@PERIODO", matriculaAtual.Semestre),
                                new ContextQueryParameter("@TURMA", matriculaAtual.Turma),
                                new ContextQueryParameter("@MATRICULA", matriculaAtual.Matricula)));
                    }

                    //carregar dados da turma destino
                    var lyTurma = RN.Turma.Carregar(Convert.ToInt32(matriculaDestino.Ano),
                                                    Convert.ToInt32(matriculaDestino.Semestre),
                                                    matriculaDestino.Turma);
                    //obter grade_id destino
                    var gradeId = GradeSerie.ObterGradeId(
                        context,
                        matriculaDestino.Ano,
                        matriculaDestino.Semestre,
                        lyTurma.Curso,
                        lyTurma.Curriculo,
                        lyTurma.Serie,
                        matriculaDestino.Turma);

                    //carregar dados da turma origem
                    var lyTurmaOrigem = RN.Turma.Carregar(Convert.ToInt32(matriculaAtual.Ano),
                                                    Convert.ToInt32(matriculaAtual.Semestre),
                                                    matriculaAtual.Turma);
                    //obter grade_id origem
                    var gradeIdOrigem = GradeSerie.ObterGradeId(
                        context,
                        matriculaAtual.Ano,
                        matriculaAtual.Semestre,
                        lyTurmaOrigem.Curso,
                        lyTurmaOrigem.Curriculo,
                        lyTurmaOrigem.Serie,
                        matriculaAtual.Turma);

                    //Obter dados e atualizar matgrade
                    context.ApplyModifications(
                        new ContextQuery(
                            string.Format(
                                @"DECLARE @aluno T_CODIGO,
                                    @grade_id T_NUMERO_GRANDE,
                                    @grade_id_origem T_NUMERO_GRANDE,
                                    @sit_matgrade T_SIT_MATGRADE	
                                                                		
                                SET @aluno = '{0}'
                                SET @grade_id = {1}
                                SET @grade_id_origem = {2}
                                SET @sit_matgrade = 'Matriculado'

                                IF NOT EXISTS ( SELECT  *
                                                FROM    LY_MATGRADE
                                                WHERE   ALUNO = @aluno
                                                        AND GRADE_ID = @grade_id
                                                        AND SIT_MATGRADE = @sit_matgrade ) 
                                    INSERT  INTO LY_MATGRADE
                                            (
                                              ALUNO,
                                              GRADE_ID,
                                              SIT_MATGRADE,
                                              DT_ULTALT
                                            )
                                    VALUES  (
                                              @aluno,
                                              @grade_id,
                                              @sit_matgrade,
                                              GETDATE()
                                            )

                                UPDATE  LY_MATGRADE
                                SET     SIT_MATGRADE = 'Cancelado'
                                WHERE   ALUNO = @aluno
                                        AND GRADE_ID = @grade_id_origem
                                        AND SIT_MATGRADE = 'Matriculado'",
                                matriculaDestino.Aluno,
                                gradeId,
                                gradeIdOrigem)));
                }
                catch (Exception)
                {
                    context.Abandon();
                    throw;
                }
            }
        }

        public ValidacaoDados ValidarTransMaisEducacao(LyMatricula matriculaOrigem, LyMatricula matriculaDestino)
        {
            DataContext contexto = null;
            RN.Matricula rnMatricula = new Matricula();
            RN.Turma rnTurma = new Turma();
            List<string> mensagens = new List<string>();
            string retornoChoqueHorario = string.Empty;
            string turmaPrincipal = string.Empty;
            string turnoTurmaDestino = string.Empty;
            string turnoTurmaPrincipal = string.Empty;
            int vagas = 0;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (matriculaOrigem == null || matriculaDestino == null)
            {
                return validacaoDados;
            }

            matriculaDestino.MaisEducacao = "S";
            matriculaDestino.SitMatricula = "Matriculado";

            if (string.IsNullOrEmpty(matriculaOrigem.Aluno) || string.IsNullOrEmpty(matriculaDestino.Aluno))
            {
                mensagens.Add("O campo ALUNO é obrigatório!");
            }

            if (string.IsNullOrEmpty(matriculaOrigem.Turma) || string.IsNullOrEmpty(matriculaDestino.Turma))
            {
                mensagens.Add("O campo Turma Mais Educacao é obrigatório!");
            }

            if (matriculaOrigem.Turma == matriculaDestino.Turma)
            {
                mensagens.Add("A turma de destino não pode ser a mesma turma de origem!");
            }

            if (matriculaOrigem.Ano <= 0 || matriculaDestino.Ano <= 0)
            {
                mensagens.Add("O campo Ano é obrigatório!");
            }

            if (matriculaOrigem.Semestre < 0 || matriculaDestino.Semestre < 0)
            {
                mensagens.Add("O campo Período é obrigatório!");
            }

            if (string.IsNullOrEmpty(matriculaOrigem.Matricula) || string.IsNullOrEmpty(matriculaDestino.Matricula))
            {
                mensagens.Add("O campo MATRICULA é obrigatório!");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //verifica se a aluno já está matriculado naquele ano / semestre / turma /disciplina
                    if (rnMatricula.PossuiMatriculaAtivaNaTurmaPorAluno(contexto, matriculaDestino.Aluno, matriculaDestino.Turma, matriculaDestino.Ano, matriculaDestino.Semestre))
                    {
                        mensagens.Add("Já existe matricula para este aluno nesta turma!");
                    }

                    //Verifica se a turma Destino tem vaga
                    vagas = rnTurma.ObtemVagasPrincipalLiberadasTurmaPor(contexto, Convert.ToInt32(matriculaDestino.Ano), Convert.ToInt32(matriculaDestino.Semestre), matriculaDestino.Turma);
                    if (vagas <= 0)
                    {
                        mensagens.Add("Esta TURMA DESTINO não possui vagas para este ano / periodo.");
                    }

                    //Analisa Choque de Horários
                    retornoChoqueHorario = AnalisaChoqueHorarioPor(contexto, matriculaOrigem, matriculaDestino);
                    if (!string.IsNullOrEmpty(retornoChoqueHorario))
                    {
                        mensagens.Add("Transferência cancelada na verificação do choque de horário.");
                        mensagens.Add(retornoChoqueHorario);

                    }

                    //Busca turma principal do aluno
                    turmaPrincipal = rnMatricula.ObtemTurmaPrincipalPor(contexto, matriculaDestino.Aluno, Convert.ToInt32(matriculaDestino.Ano),
                                                                          Convert.ToInt32(matriculaDestino.Semestre));
                    if (matriculaDestino.Turma == turmaPrincipal)
                    {
                        mensagens.Add("A turma de destino não pode ser igual a turma principal do aluno.");
                    }

                    //Busca turno da turma Destino
                    turnoTurmaDestino = rnTurma.ObtemTurnoTurmaPor(contexto, matriculaDestino.Ano, matriculaDestino.Semestre, matriculaDestino.Turma);

                    //Busca turno da turma Regular, OBS: Mais educação não troca de ano / periodo
                    turnoTurmaPrincipal = rnTurma.ObtemTurnoTurmaPor(contexto, matriculaDestino.Ano, matriculaDestino.Semestre, turmaPrincipal);

                    //Verificar se a turma Mais Educacao é contra-turno da Turma Regular
                    if (!Turno.VerificarContraTurno(turnoTurmaDestino, turnoTurmaPrincipal))
                    {
                        mensagens.Add("A turma Mais Educação deve estar em contraturno com a turma principal.");
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

        public static void TransferirTurmaMaisEducacao(LyMatricula matriculaOrigem, LyMatricula matriculaDestino)
        {
            using (var context = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    //carregar disciplinas da turma destino
                    var disciplinas = Matricula.ConsultaDisciplinaGrade(Convert.ToString(matriculaDestino.Ano), Convert.ToString(matriculaDestino.Semestre), matriculaDestino.Turma);

                    foreach (DataRow disciplinaRow in disciplinas.Rows)
                    {
                        matriculaDestino.Disciplina = disciplinaRow["disciplina"].ToString();
                        matriculaOrigem.Disciplina = disciplinaRow["disciplina"].ToString();

                        //verificar se ja existe um deles cancelado na ly_matricula
                        var contextQuery = new ContextQuery(
                            @" SELECT  COUNT(*)
                                                    FROM    LY_MATRICULA
                                                    WHERE   ALUNO = @ALUNO
                                                            AND DISCIPLINA = @DISCIPLINA
                                                            AND TURMA = @TURMA
                                                            AND ANO = @ANO
                                                            AND SEMESTRE = @SEMESTRE
                                                             ");

                        contextQuery.Parameters.Add("@ALUNO", matriculaDestino.Aluno);
                        contextQuery.Parameters.Add("@DISCIPLINA", matriculaDestino.Disciplina);
                        contextQuery.Parameters.Add("@TURMA", matriculaDestino.Turma);
                        contextQuery.Parameters.Add("@ANO", matriculaDestino.Ano);
                        contextQuery.Parameters.Add("@SEMESTRE", matriculaDestino.Semestre);

                        var canceladas = context.GetReturnValue<int>(contextQuery);

                        //se existir atualiza senao insere
                        if (canceladas > 0)
                        {
                            Matricula.AtualizarMaisEducacao(context, matriculaDestino);
                        }
                        else
                        {
                            Matricula.InserirMaisEducacao(context, matriculaDestino);
                        }

                        //Transferir Notas
                        Nota.MigrarNotas(context, matriculaOrigem, matriculaDestino.Turma);

                        //Transferir Faltas
                        Falta.MigrarFaltas(context, matriculaOrigem, matriculaDestino.Turma);

                        //Atualizar a Situacao na matricula para cancelado da turma concomitante anterir
                        Matricula.RemoverMaisEducacao(context, matriculaOrigem);

                        //Insere transferencia na tabela Ly_turma_transf
                        context.ApplyModifications(
                            new ContextQuery(
                                @" INSERT  INTO dbo.LY_TURMA_TRANSF ( ALUNO, DISCIPLINA, TURMA_DESTINO, ANO,
                                                                   PERIODO, TURMA_ANT, DATA, USUARIO,
                                                                   MOTIVO_TRANSF )
                                                       VALUES  ( @ALUNO, @DISCIPLINA, @TURMADESTINO, @ANO, @PERIODO, @TURMA,
                                                                  GETDATE(), @MATRICULA, '01' ) ",
                                new ContextQueryParameter("@ALUNO", matriculaOrigem.Aluno),
                                new ContextQueryParameter("@DISCIPLINA", matriculaOrigem.Disciplina),
                                new ContextQueryParameter("@TURMADESTINO", matriculaDestino.Turma),
                                new ContextQueryParameter("@ANO", matriculaOrigem.Ano),
                                new ContextQueryParameter("@PERIODO", matriculaOrigem.Semestre),
                                new ContextQueryParameter("@TURMA", matriculaOrigem.Turma),
                                new ContextQueryParameter("@MATRICULA", matriculaOrigem.Matricula)));
                    }

                    //carregar dados da turma destino
                    var lyTurma = RN.Turma.Carregar(Convert.ToInt32(matriculaDestino.Ano),
                                                    Convert.ToInt32(matriculaDestino.Semestre),
                                                    matriculaDestino.Turma);
                    //obter grade_id destino
                    var gradeId = GradeSerie.ObterGradeId(
                        context,
                        matriculaDestino.Ano,
                        matriculaDestino.Semestre,
                        lyTurma.Curso,
                        lyTurma.Curriculo,
                        lyTurma.Serie,
                        matriculaDestino.Turma);

                    //carregar dados da turma origem
                    var lyTurmaOrigem = RN.Turma.Carregar(Convert.ToInt32(matriculaOrigem.Ano),
                                                    Convert.ToInt32(matriculaOrigem.Semestre),
                                                    matriculaOrigem.Turma);
                    //obter grade_id origem
                    var gradeIdOrigem = GradeSerie.ObterGradeId(
                        context,
                        matriculaOrigem.Ano,
                        matriculaOrigem.Semestre,
                        lyTurmaOrigem.Curso,
                        lyTurmaOrigem.Curriculo,
                        lyTurmaOrigem.Serie,
                        matriculaOrigem.Turma);

                    //Obter dados e atualizar matgrade
                    context.ApplyModifications(
                        new ContextQuery(
                            string.Format(
                                @"DECLARE @aluno T_CODIGO,
                                    @grade_id T_NUMERO_GRANDE,
                                    @grade_id_origem T_NUMERO_GRANDE,
                                    @sit_matgrade T_SIT_MATGRADE	
                                                                		
                                SET @aluno = '{0}'
                                SET @grade_id = {1}
                                SET @grade_id_origem = {2}
                                SET @sit_matgrade = 'Matriculado'

                                IF NOT EXISTS ( SELECT  *
                                                FROM    LY_MATGRADE
                                                WHERE   ALUNO = @aluno
                                                        AND GRADE_ID = @grade_id
                                                        AND SIT_MATGRADE = @sit_matgrade ) 
                                    INSERT  INTO LY_MATGRADE
                                            (
                                              ALUNO,
                                              GRADE_ID,
                                              SIT_MATGRADE,
                                              DT_ULTALT
                                            )
                                    VALUES  (
                                              @aluno,
                                              @grade_id,
                                              @sit_matgrade,
                                              GETDATE()
                                            )

                                UPDATE  LY_MATGRADE
                                SET     SIT_MATGRADE = 'Cancelado'
                                WHERE   ALUNO = @aluno
                                        AND GRADE_ID = @grade_id_origem
                                        AND SIT_MATGRADE = 'Matriculado'",
                                matriculaDestino.Aluno,
                                gradeId,
                                gradeIdOrigem)));
                }
                catch (Exception)
                {
                    context.Abandon();
                    throw;
                }
            }
        }

        public static ValidacaoDados ValidarTransEducacaoEspecial(LyMatricula matriculaOrigem, LyMatricula matriculaDestino, List<Atendimento> atendimentos)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (matriculaOrigem == null || matriculaDestino == null)
            {
                return validacaoDados;
            }

            matriculaDestino.EducEspecial = "S";
            matriculaDestino.SitMatricula = "Matriculado";

            if (string.IsNullOrEmpty(matriculaOrigem.Aluno) || string.IsNullOrEmpty(matriculaDestino.Aluno))
            {
                mensagens.Add("O campo ALUNO é obrigatório!");
            }

            if (string.IsNullOrEmpty(matriculaOrigem.Turma) || string.IsNullOrEmpty(matriculaDestino.Turma))
            {
                mensagens.Add("O campo Sala de Recurso Educacao Especial é obrigatório!");
            }

            if (matriculaOrigem.Turma == matriculaDestino.Turma)
            {
                mensagens.Add("A Sala de Recurso de destino não pode ser a mesma de origem!");
            }

            if (matriculaOrigem.Ano <= 0 || matriculaDestino.Ano <= 0)
            {
                mensagens.Add("O campo Ano é obrigatório!");
            }

            if (matriculaOrigem.Semestre < 0 || matriculaDestino.Semestre < 0)
            {
                mensagens.Add("O campo Período é obrigatório!");
            }

            if (string.IsNullOrEmpty(matriculaOrigem.Matricula) || string.IsNullOrEmpty(matriculaDestino.Matricula))
            {
                mensagens.Add("O campo MATRICULA é obrigatório!");
            }

            if (atendimentos.Count < 2 || atendimentos.Count > 3)
            {
                mensagens.Add("O Aluno deve ser alocado em no mínimo 2 e no máximo 3 atendimentos !");
            }

            if (mensagens.Count == 0)
            {
                //Verifica se a turma Destino tem vaga
                var vagas = RN.Turma.RetornaVagas(Convert.ToInt32(matriculaDestino.Ano), Convert.ToInt32(matriculaDestino.Semestre), matriculaDestino.Turma);
                if (vagas <= 0)
                {
                    mensagens.Add("Esta SALA DE RECURSO DESTINO não possui vagas para este ano / periodo.");
                }

                //Verifica se o atendimento já tem 5 alunos
                foreach (var atendimento in atendimentos)
                {
                    var matriculados = Turma.RetornaQtdAlunosAtendidos(Convert.ToInt32(matriculaDestino.Ano), Convert.ToInt32(matriculaDestino.Semestre), atendimento.Disciplina, matriculaDestino.Turma);
                    if (matriculados >= 5)
                    {
                        mensagens.Add("O atendimento " + atendimento.Horario + " já possui o máximo de 5 alunos.");
                    }
                }

                //Analisa Choque de Horários
                var retorno = AnalisarChoqueHorarioEducacaoEspecial(matriculaOrigem, matriculaDestino, atendimentos);
                if (!string.IsNullOrEmpty(retorno))
                {
                    mensagens.Add("Transferência cancelada na verificação do choque de horário.");
                }

                //Busca dados da matricula regular
                var matriculaRegular = Matricula.CarregarMatriculaRegular(matriculaDestino.Aluno, Convert.ToInt32(matriculaDestino.Ano),
                                                                      Convert.ToInt32(matriculaDestino.Semestre));
                if (matriculaDestino.Turma == matriculaRegular.Turma)
                {
                    mensagens.Add("A SALA DE RECURSO de destino não pode ser igual a turma regular do aluno.");
                }

                //Busca dados da turma Destino
                var turmaDestino = RN.Turma.Carregar(Convert.ToInt32(matriculaDestino.Ano),
                                                     Convert.ToInt32(matriculaDestino.Semestre), matriculaDestino.Turma);
                //Busca dados da turma Regular
                var turmaRegular = RN.Turma.Carregar(Convert.ToInt32(matriculaRegular.Ano),
                                                     Convert.ToInt32(matriculaRegular.Semestre), matriculaRegular.Turma);

                //Alundo do integral e Ampliado não podem fazer.
                if (turmaRegular.Turno == "A" || turmaRegular.Turno == "I")
                {
                    mensagens.Add("Alunos do turno AMPLIADO ou INTEGRAL não podem ser matriculados em Educacao Especial.");
                }
                else
                {
                    //Verificar se a turma Educacao Especial é contra-turno da Turma Regular
                    if (!Turno.VerificarContraTurno(turmaDestino.Turno, turmaRegular.Turno))
                    {
                        mensagens.Add("A SALA DE RECURSO Mais Educação deve estar em contraturno com a turma regular.");
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

        public static void TransferirTurmaEducacaoEspecial(LyMatricula matriculaOrigem, LyMatricula matriculaDestino, List<Atendimento> atendimentos)
        {
            using (var context = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    foreach (var atendimento in atendimentos)
                    {
                        //verificar se ja existe um deles cancelado na ly_matricula
                        var contextQuery = new ContextQuery(
                            @" SELECT  COUNT(*)
                                                    FROM    LY_MATRICULA
                                                    WHERE   ALUNO = @ALUNO
                                                            AND DISCIPLINA = @DISCIPLINA
                                                            AND TURMA = @TURMA
                                                            AND ANO = @ANO
                                                            AND SEMESTRE = @SEMESTRE
                                                            AND SIT_MATRICULA = 'Cancelado' ");

                        contextQuery.Parameters.Add("@ALUNO", matriculaDestino.Aluno);
                        contextQuery.Parameters.Add("@DISCIPLINA", matriculaDestino.Disciplina);
                        contextQuery.Parameters.Add("@TURMA", matriculaDestino.Turma);
                        contextQuery.Parameters.Add("@ANO", matriculaDestino.Ano);
                        contextQuery.Parameters.Add("@SEMESTRE", matriculaDestino.Semestre);

                        var canceladas = context.GetReturnValue<int>(contextQuery);

                        //se existir atualiza senao insere
                        if (canceladas > 0)
                        {
                            Matricula.AtualizarEducacaoEspecial(context, matriculaDestino);
                        }
                        else
                        {
                            Matricula.InserirEducacaoEspecial(context, matriculaDestino);
                        }

                        matriculaOrigem.Disciplina = atendimento.Disciplina;
                        matriculaDestino.Disciplina = atendimento.Disciplina;

                        //Transferir Notas
                        Nota.MigrarNotas(context, matriculaOrigem, matriculaDestino.Turma);

                        //Transferir Faltas
                        Falta.MigrarFaltas(context, matriculaOrigem, matriculaDestino.Turma);

                        //Atualizar a Situacao na matricula para cancelado da turma concomitante anterir
                        Matricula.RemoverEducacaoEspecial(context, matriculaOrigem);

                        //Insere transferencia na tabela Ly_turma_transf
                        context.ApplyModifications(
                            new ContextQuery(
                                @" INSERT  INTO dbo.LY_TURMA_TRANSF ( ALUNO, DISCIPLINA, TURMA_DESTINO, ANO,
                                                                   PERIODO, TURMA_ANT, DATA, USUARIO,
                                                                   MOTIVO_TRANSF )
                                                       VALUES  ( @ALUNO, @DISCIPLINA, @TURMADESTINO, @ANO, @PERIODO, @TURMA,
                                                                  GETDATE(), @MATRICULA, '01' ) ",
                                new ContextQueryParameter("@ALUNO", matriculaOrigem.Aluno),
                                new ContextQueryParameter("@DISCIPLINA", matriculaOrigem.Disciplina),
                                new ContextQueryParameter("@TURMADESTINO", matriculaDestino.Turma),
                                new ContextQueryParameter("@ANO", matriculaOrigem.Ano),
                                new ContextQueryParameter("@PERIODO", matriculaOrigem.Semestre),
                                new ContextQueryParameter("@TURMA", matriculaOrigem.Turma),
                                new ContextQueryParameter("@MATRICULA", matriculaOrigem.Matricula)));

                        //carregar dados da turma destino
                        var lyTurma = RN.Turma.Carregar(Convert.ToInt32(matriculaDestino.Ano),
                                                        Convert.ToInt32(matriculaDestino.Semestre),
                                                        matriculaDestino.Turma);
                        //obter gradeid destino
                        var gradeId = GradeSerie.ObterGradeId(
                            context,
                            matriculaDestino.Ano,
                            matriculaDestino.Semestre,
                            lyTurma.Curso,
                            lyTurma.Curriculo,
                            lyTurma.Serie,
                            matriculaDestino.Turma);

                        //carregar dados da turma origem
                        var lyTurmaOrigem = RN.Turma.Carregar(Convert.ToInt32(matriculaOrigem.Ano),
                                                        Convert.ToInt32(matriculaOrigem.Semestre),
                                                        matriculaOrigem.Turma);
                        //obter gradeid origem
                        var gradeIdOrigem = GradeSerie.ObterGradeId(
                            context,
                            matriculaOrigem.Ano,
                            matriculaOrigem.Semestre,
                            lyTurmaOrigem.Curso,
                            lyTurmaOrigem.Curriculo,
                            lyTurmaOrigem.Serie,
                            matriculaOrigem.Turma);

                        //Obter dados e atualizar matgrade
                        context.ApplyModifications(
                            new ContextQuery(
                                string.Format(
                                    @"DECLARE @aluno T_CODIGO,
                                    @grade_id T_NUMERO_GRANDE,
                                    @grade_id_origem T_NUMERO_GRANDE,
                                    @sit_matgrade T_SIT_MATGRADE	
                                                                		
                                SET @aluno = '{0}'
                                SET @grade_id = {1}
                                SET @grade_id_origem = {2}
                                SET @sit_matgrade = 'Matriculado'

                                IF NOT EXISTS ( SELECT  *
                                                FROM    LY_MATGRADE
                                                WHERE   ALUNO = @aluno
                                                        AND GRADE_ID = @grade_id
                                                        AND SIT_MATGRADE = @sit_matgrade ) 
                                    INSERT  INTO LY_MATGRADE
                                            (
                                              ALUNO,
                                              GRADE_ID,
                                              SIT_MATGRADE,
                                              DT_ULTALT
                                            )
                                    VALUES  (
                                              @aluno,
                                              @grade_id,
                                              @sit_matgrade,
                                              GETDATE()
                                            )

                                UPDATE  LY_MATGRADE
                                SET     SIT_MATGRADE = 'Cancelado'
                                WHERE   ALUNO = @aluno
                                        AND GRADE_ID = @grade_id_origem
                                        AND SIT_MATGRADE = 'Matriculado'",
                                    matriculaDestino.Aluno,
                                    gradeId,
                                    gradeIdOrigem)));
                    }
                }
                catch (Exception)
                {
                    context.Abandon();
                    throw;
                }
            }
        }
        public DataTable ObtemDadosTurmaAtualPor(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"SELECT TOP 1 M.TURMA, 
                                             M.ANO, 
                                             M.SEMESTRE 
                                FROM   LY_MATRICULA M 
                                       INNER JOIN LY_TURMA T 
                                               ON T.DISCIPLINA = M.DISCIPLINA 
                                                  AND M.TURMA = T.TURMA 
                                                  AND M.ANO = T.ANO 
                                                  AND M.SEMESTRE = T.SEMESTRE 
                                       JOIN LY_PERIODO_LETIVO PL 
                                         ON PL.ANO = M.ANO 
                                            AND PL.PERIODO = M.SEMESTRE 
                                WHERE  M.SIT_MATRICULA = 'Matriculado' 
                                       AND ( M.DEPENDENCIA <> 'S' 
                                              OR M.DEPENDENCIA IS NULL ) 
                                       AND ( M.CONCOMITANTE = 'N' 
                                              OR M.CONCOMITANTE IS NULL ) 
                                       AND ( M.EDUC_ESPECIAL = 'N' 
                                              OR M.EDUC_ESPECIAL IS NULL ) 
                                       AND ( M.MAIS_EDUCACAO = 'N' 
                                              OR M.MAIS_EDUCACAO IS NULL ) 
                                       AND T.OPTATIVAREFORCO = 'N' 
                                       AND ISNULL(T.ELETIVA,'N') = 'N'
                                       AND M.ALUNO = @ALUNO  ";

                contextQuery.Parameters.Add("@ALUNO", aluno);

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

        private static List<string> VerificaChoqueHorarioPor(string aluno, List<DadosHorarios> listaHorarios)
        {
            List<string> listaErro = new List<string>();

            foreach (DadosHorarios horario1 in listaHorarios)
            {
                //Para cada horário, buscar horários de outras disciplinas da pcolHorarios que possuam aula no mesmo dia (item DiaDaSemana)
                List<DadosHorarios> horarioDiaSemana = listaHorarios.FindAll(delegate(DadosHorarios h) { return h.DiaSemana.Equals(horario1.DiaSemana) && !(h.Disciplina.Equals(horario1.Disciplina) && h.Turma.Equals(horario1.Turma)); });

                if (horarioDiaSemana != null && horarioDiaSemana.Count > 0)
                {
                    foreach (DadosHorarios horario2 in horarioDiaSemana)
                    {
                        if (horario1.HoraInicioAula < horario2.HoraFimAula
                           && horario1.HoraFimAula > horario2.HoraInicioAula
                           && horario1.DtInicioAula <= horario2.DtFimAula
                           && horario1.DtFimAula >= horario2.DtInicioAula)
                        {
                            var erro = string.Empty;
                            erro = "Coincidência de horário de " + horario2.Disciplina + "/" + horario2.Turma +
                                     " com a disciplina " + horario1.Disciplina + "/" + horario1.Turma;

                            if (!listaErro.Contains(erro))
                            {
                                listaErro.Add(erro);
                            }
                        }
                    }
                }
            }

            if (listaErro.Count > 0)
            {
                return listaErro;
            }

            return null;
        }

        public string AnalisaChoqueHorarioPor(DataContext contexto, LyMatricula matriculaOrigem, LyMatricula matriculaDestino)
        {
            var erro = string.Empty;
            RN.Matricula rnMatricula = new Matricula();
            DataTable dtDisciplinasDeOutrasTurmas = new DataTable();
            DataTable dtDisciplinasTurmaDestino = new DataTable();
            RN.AulaDocente rnAulaDocente = new AulaDocente();

            //carregar disciplinas matriculadas do aluno
            dtDisciplinasDeOutrasTurmas = rnMatricula.ListaDisciplinaGradePorTurmasDiferenteDe(contexto, matriculaDestino.Aluno, matriculaDestino.Ano, matriculaDestino.Semestre, matriculaOrigem.Turma);

            //carregar disciplinas da turma concomitante destino
            dtDisciplinasTurmaDestino = rnMatricula.ListaDisciplinaGradePor(contexto, matriculaDestino.Ano, matriculaDestino.Semestre, matriculaDestino.Turma);

            //cria lista de horarios
            List<DadosHorarios> listaHorario = new List<DadosHorarios>();

            //Adiciona na lista de horarios os horarios das disciplina regulares
            foreach (DataRow disciplinaRow in dtDisciplinasDeOutrasTurmas.Rows)
            {
                List<DadosHorarios> listaHorarioAux = rnAulaDocente.ObtemHorariosPor(contexto, disciplinaRow["disciplina"].ToString(), disciplinaRow["TURMA"].ToString(), matriculaDestino.Ano, matriculaDestino.Semestre);

                if (listaHorarioAux != null && listaHorarioAux.Count > 0)
                {
                    listaHorario.AddRange(listaHorarioAux);
                }
            }

            //Adiciona na lista de horarios os horarios das disciplina da turma destino
            foreach (DataRow disciplinaRow in dtDisciplinasTurmaDestino.Rows)
            {
                matriculaDestino.Disciplina = disciplinaRow["disciplina"].ToString();
                List<DadosHorarios> listaHorarioAux = rnAulaDocente.ObtemHorariosPor(contexto, matriculaDestino.Disciplina, matriculaDestino.Turma, matriculaDestino.Ano, matriculaDestino.Semestre);

                if (listaHorarioAux != null && listaHorarioAux.Count > 0)
                {
                    listaHorario.AddRange(listaHorarioAux);
                }
            }

            var valorRetorno = VerificaChoqueHorarioPor(matriculaDestino.Aluno, listaHorario);

            if (valorRetorno != null)
            {
                erro = valorRetorno.Aggregate((x, y) => x + "<br/>" + y);
                erro = "Choque de horário identificado:<br>" + erro.Replace(",", ",<br>");
            }

            return erro;
        }
    }
}