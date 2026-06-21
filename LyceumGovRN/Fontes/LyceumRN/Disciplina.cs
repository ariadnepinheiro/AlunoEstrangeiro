namespace Techne.Lyceum.RN
{
    using System;
    using System.Data;
    using Seeduc.Infra.Data;
    using Seeduc.Infra.Entities;
    using Techne.Data;
    using Techne.Library;
    using Techne.Lyceum.CR;
    using Techne.Lyceum.RN.DTOs;
    using System.Data.SqlClient;
    using System.Collections.Generic;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Lyceum.RN.Util;
    using System.Linq;
    using System.Collections;

    public class Disciplina : RNBase
    {
        public class NotasDisciplina : IEntity
        {
            public string GrupoNota { get; set; }
            public string NotaMax { get; set; }
            public int CasasDecimais { get; set; }
        }

        public static QueryTable Consultar(string unidade_responsavel)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = "select DISTINCT DISCIPLINA,NOME from ly_disciplina WHERE FACULDADE = ?";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, unidade_responsavel);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public DataTable ListaDisciplinaParaMultiplaPor(string disciplina)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable disciplinas = null;

            try
            {
                contextQuery.Command = @" SELECT  d.disciplina ,
                                d.nome_compl
                        FROM    LY_DISCIPLINA d
                        WHERE   NOT EXISTS ( SELECT TOP 1
                                                    1
                                             FROM   LY_DISCIPLINA_MULTIPLA dm
                                             WHERE  dm.DISCIPLINA = @DISCIPLINA
                                                    AND dm.disciplina = d.disciplina ) ";

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);

                disciplinas = ctx.GetDataTable(contextQuery);
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return disciplinas;
        }

        public static NotasDisciplina ConsultarDisciplinaConceitos(string disciplina)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery("select isnull(nota_max,'') nota_max, isnull(n_casas_dec,0) casas_decimais, isnull(grupo_nota,'') grupo_nota from ly_disciplina WITH(NOLOCK) where DISCIPLINA = @DISCIPLINA");

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);

                var notasDisciplina = ctx.TryToBindEntity<NotasDisciplina>(contextQuery);

                if (string.IsNullOrEmpty(notasDisciplina.NotaMax))
                {
                    notasDisciplina.NotaMax = "0";
                }

                return notasDisciplina;
            }
        }

        public static QueryTable ConsultarPorTurma(string turma, string ano, string semestre, string grade)
        {
            var cn = Config.CreateConnection();

            var qt = new QueryTable("select d.DISCIPLINA, d.NOME from ly_grade_turma gt inner join ly_disciplina d on d.disciplina = gt.disciplina " +
                                "inner join ly_grade_serie gs on gs.grade_id = gt.grade_id and gs.ano = gt.ano and gs.semestre = gt.semestre " +
                                "WHERE gt.ano = ? " +
                                "AND gt.semestre = ? " +
                                "AND gt.turma = ? " +
                                "AND gs.grade = ? ");

            qt.Query(cn, ano, semestre, turma, grade);

            return qt;
        }

        public static QueryTable ConsultarPorAnoSemestre(decimal ano, decimal semestre, string pcurso, string pturno, string curriculo, string aluno)
        {
            string curso = pcurso != null ? pcurso.Split('-')[0].Trim() : null;
            string turno = pturno != null ? pturno.Split('-')[0].Trim() : null;

            TConnection cn = Config.CreateConnection();

            QueryTable qt = new QueryTable(
                  @"Select distinct d.disciplina, (d.disciplina + ' - ' + d.nome) nome from LY_turma t inner join LY_GRADE_TURMA g on g.TURMA = t.TURMA and g.DISCIPLINA = t.DISCIPLINA 
                    inner join ly_disciplina d on t.disciplina = d.disciplina 
                    where t.ano = isnull(?,0) and t.semestre = isnull(?,0) and t.curso = isnull(?,'') and t.turno = isnull(?,'') and t.curriculo = isnull(?,'') 
                    and not exists 
                        (select 1 from ly_matgrade mg 
                            join ly_grade_turma gt 
                            on mg.grade_id = gt.grade_id 
                            join ly_grade_serie gs on gs.GRADE_ID = mg.GRADE_ID and gs.GRADE = gt.TURMA 
                            join ly_matricula m on m.ALUNO = mg.ALUNO and gt.disciplina = m.disciplina and gt.turma = m.turma and gt.ano = m.ano and gt.semestre = m.semestre 
                            join ly_disciplina d2 on m.disciplina = d2.disciplina 
                            where mg.sit_matgrade = 'Matriculado' 
                            and mg.aluno = ? and m.disciplina = d.disciplina)");

            qt.Query(cn, ano, semestre, curso, turno, curriculo, aluno);

            return qt;
        }

        public static QueryTable Consultar(string curriculo, string curso, string turno)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = " select DISTINCT D.DISCIPLINA, (D.DISCIPLINA + ' - ' + D.NOME) NOME from ly_disciplina D " +
                         " INNER JOIN LY_GRADE G ON G.DISCIPLINA = D.DISCIPLINA " +
                         " AND G.curriculo = ? " +
                         " AND G.turno = ? " +
                         " AND G.curso = ? ";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, curriculo, turno, curso);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public static QueryTable Consultar(string curso, string turno, string curriculo, decimal serie)
        {
            QueryTable qt = null;

            if (!string.IsNullOrEmpty(curso)
                && !string.IsNullOrEmpty(turno)
                && !string.IsNullOrEmpty(curriculo)
                && serie != 0)
            {
                TConnection connection = Config.CreateConnection();

                connection.Open();

                string sql = " select distinct D.DISCIPLINA, D.NOME " +
                             " from ly_disciplina D " +
                             " inner join ly_grade G ON G.disciplina = D.disciplina " +
                             " where " +
                             " G.curso = ? and G.turno = ? and G.curriculo = ? and G.serie_ideal = ? " +
                             " ORDER BY D.NOME ";

                try
                {
                    qt = new QueryTable(sql);

                    qt.Query(connection, curso, turno, curriculo, serie);
                }
                finally
                {
                    connection.Close();
                }
            }

            return qt;
        }

        public static QueryTable ConsultarComMultipla(string turma, string ano, string semestre)
        {
            QueryTable qt = null;

            if (!string.IsNullOrEmpty(turma)
                && !string.IsNullOrEmpty(ano)
                && !string.IsNullOrEmpty(semestre))
            {
                TConnection connection = Config.CreateConnection();
                connection.Open();

                string sql = @"
                    declare @turma varchar(255)
                    declare @ano decimal
                    declare @semestre decimal
                    set @turma = ?
                    set @ano = ?
                    set @semestre = ?    
    
                    select * from (
        
                    select t.disciplina, d.NOME from LY_TURMA t
                    inner join LY_DISCIPLINA d on d.DISCIPLINA = t.DISCIPLINA
					INNER JOIN LY_SERIE S on t.CURRICULO = S.CURRICULO
														AND T.TURNO = S.TURNO
														AND T.CURSO = S.CURSO
														AND T.SERIE = S.SERIE
                    where TURMA like @turma
                    and t.ANO = @ano and t.SEMESTRE = @semestre
                    and t.DISCIPLINA_MULTIPLA <> null					
					and (S.OFERTAELETIVA = 'N' or (S.OFERTAELETIVA = 'S' AND ISNULL(D.ELETIVA, 'N') = 'N'))
                    union

                    select d.disciplina, d.NOME 
					from LY_TURMA turma
                    inner join LY_DISCIPLINA d on d.DISCIPLINA = turma.DISCIPLINA
					INNER JOIN LY_SERIE S on turma.CURRICULO = S.CURRICULO
														AND turma.TURNO = S.TURNO
														AND turma.CURSO = S.CURSO
														AND TURMA.SERIE = S.SERIE
                    where TURMA like @turma
                    and turma.ANO = @ano and turma.SEMESTRE = @semestre
                    and turma.DISCIPLINA_MULTIPLA is null
                    and d.MULTIPLA = 'N'
					and (S.OFERTAELETIVA = 'N' or (S.OFERTAELETIVA = 'S' AND ISNULL(D.ELETIVA, 'N') = 'N'))

                    union

                    select d1.disciplina + '|' + d.disciplina disciplina, d1.nome + ' (' + d.NOME + ')' nome 
					from LY_DISCIPLINA_MULTIPLA dm
                    inner join LY_DISCIPLINA d on d.DISCIPLINA = dm.DISCIPLINA_MULTIPLA
                    inner join ly_disciplina d1
                    on d1.disciplina = dm.disciplina
                    where EXISTS (
	                    select 1 from LY_TURMA turma
	                    inner join LY_DISCIPLINA d on d.DISCIPLINA = turma.DISCIPLINA
						INNER JOIN LY_SERIE S on turma.CURRICULO = S.CURRICULO
														AND turma.TURNO = S.TURNO
														AND turma.CURSO = S.CURSO
														AND TURMA.SERIE = S.SERIE
	                    where TURMA like @turma
	                    and turma.ANO = @ano and turma.SEMESTRE = @semestre
	                    and turma.DISCIPLINA_MULTIPLA is null
	                    and d.MULTIPLA = 'S'
                        and d.disciplina = dm.DISCIPLINA
						and (S.OFERTAELETIVA = 'N' or (S.OFERTAELETIVA = 'S' AND ISNULL(D.ELETIVA, 'N') = 'N')))

                    union

                    select d1.disciplina + '|' + d.disciplina disciplina, d1.nome + ' (' + d.NOME + ')' nome 
					from LY_TURMA turma
                    inner join LY_DISCIPLINA d on d.DISCIPLINA = turma.DISCIPLINA_MULTIPLA
                    inner join LY_DISCIPLINA d1 on d1.DISCIPLINA = turma.DISCIPLINA
					INNER JOIN LY_SERIE S on turma.CURRICULO = S.CURRICULO
														AND turma.TURNO = S.TURNO
														AND turma.CURSO = S.CURSO
														AND TURMA.SERIE = S.SERIE
                    where TURMA like @turma
                    and turma.ANO = @ano and turma.SEMESTRE = @semestre
                    and turma.DISCIPLINA_MULTIPLA is not null
                    and (S.OFERTAELETIVA = 'N' or (S.OFERTAELETIVA = 'S' AND ISNULL(D.ELETIVA, 'N') = 'N'))
					
					union

					SELECT D1.DISCIPLINA + '|' + D.DISCIPLINA DISCIPLINA, D1.NOME + ' (' + D.NOME + ')' NOME 
                    FROM LY_TURMA TURMA
                    INNER JOIN LY_DISCIPLINA D   ON D.DISCIPLINA = TURMA.DISCIPLINA_MULTIPLA
                    INNER JOIN LY_DISCIPLINA D1  ON D1.DISCIPLINA = TURMA.DISCIPLINA
					INNER JOIN LY_SERIE S on turma.CURRICULO = S.CURRICULO
														AND turma.TURNO = S.TURNO
														AND turma.CURSO = S.CURSO
														AND TURMA.SERIE = S.SERIE
                    WHERE TURMA LIKE @turma
                        AND TURMA.ANO = @ano 
                        AND TURMA.SEMESTRE = @semestre
                        AND TURMA.DISCIPLINA_MULTIPLA IS NOT NULL
						AND ISNULL(D.ELETIVA, 'N') = 'S'
						AND S.OFERTAELETIVA = 'S'

                    ) as tabela                     

                    order by tabela.nome ";
                try
                {
                    qt = new QueryTable(sql);

                    qt.Query(connection, turma, ano, semestre);
                }
                finally
                {
                    connection.Close();
                }
            }

            return qt;
        }

        public static Boolean ConsultarIsMultipla(TConnection connection, string turma, string ano, string semestre, string curso, string turno, string curriculo, string serie, string disciplina)
        {
            QueryTable qt = null;

            if (!string.IsNullOrEmpty(turma)
                && !string.IsNullOrEmpty(ano)
                && !string.IsNullOrEmpty(semestre))
            {
                string sql = @"
                    declare @turma varchar(255)
                    declare @ano decimal
                    declare @semestre decimal
                    declare @curso varchar(255)
                    declare @serie decimal
                    declare @turno varchar(255)
                    declare @curriculo varchar(255)

                    set @turma = ?
                    set @ano = ?
                    set @semestre = ?
                    set @curso = ?
                    set @turno = ?
                    set @serie = ?
                    set @curriculo = ?
                    
                    select * from (

                    select t.disciplina, d.NOME from LY_TURMA t
                    inner join LY_DISCIPLINA d
                    on d.DISCIPLINA = t.DISCIPLINA
                    where TURMA like @turma
                    and t.ANO = @ano and t.SEMESTRE = @semestre
                    and t.DISCIPLINA_MULTIPLA <> null

                    union

                    select d.disciplina, d.NOME from LY_TURMA turma
                    inner join LY_DISCIPLINA d
                    on d.DISCIPLINA = turma.DISCIPLINA
                    where TURMA like @turma
                    and turma.ANO = @ano and turma.SEMESTRE = @semestre
                    and turma.DISCIPLINA_MULTIPLA is null
                    and d.MULTIPLA = 'N'

                    union

                    select dm.DISCIPLINA_MULTIPLA, d.NOME from LY_DISCIPLINA_MULTIPLA dm
                    inner join LY_DISCIPLINA d
                    on d.DISCIPLINA = dm.DISCIPLINA_MULTIPLA
                    where EXISTS (
                        select 1 from LY_TURMA turma
	                    inner join LY_DISCIPLINA d
	                    on d.DISCIPLINA = turma.DISCIPLINA
	                    where TURMA like @turma
	                    and turma.ANO = @ano and turma.SEMESTRE = @semestre
	                    and turma.DISCIPLINA_MULTIPLA is null
	                    and d.MULTIPLA = 'S'
                        and d.disciplina = dm.disciplina)

                    union

                    select d.disciplina, d.NOME from LY_TURMA turma
                    inner join LY_DISCIPLINA d
                    on d.DISCIPLINA = turma.DISCIPLINA_MULTIPLA
                    where TURMA like @turma
                    and turma.ANO = @ano and turma.SEMESTRE = @semestre
                    and turma.DISCIPLINA_MULTIPLA is not null

                    except

                    select g.DISCIPLINA, d.NOME from ly_grade g
                    inner join LY_DISCIPLINA d
                    on d.DISCIPLINA = g.DISCIPLINA
                    where curso = @curso and turno = @turno
                    and SERIE_IDEAL = @serie and CURRICULO = @curriculo) tabela

                    order by tabela.nome";

                qt = new QueryTable(sql);
                qt.Query(connection, turma, ano, semestre, curso, turno, serie, curriculo);

                foreach (SimpleRow row in qt.Rows)
                    if (row["disciplina"] == disciplina)
                        return true;
            }
            return false;
        }

        public static string ObterNomeDisciplina(TConnection connection, string disciplina)
        {
            DbObject valorConsulta = TCommand.ExecuteScalar(connection, "select NOME from ly_disciplina where disciplina = ?", disciplina);

            if (!valorConsulta.IsNull)
                return (string)valorConsulta;

            return string.Empty;
        }

        public static bool EhDisciplinaEletiva(TConnection connection, string disciplina)
        {
            DbObject valorConsulta = TCommand.ExecuteScalar(connection, "select NOME from ly_disciplina where isnull(eletiva, 'n') = 'S' and disciplina = ?", disciplina);

            if (!valorConsulta.IsNull)
                return true;

            return false;
        }

        public bool EhDisciplinaGradeEletivaPor(decimal ano, decimal periodo, string turma, string disciplina)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.EhDisciplinaGradeEletivaPor(contexto, ano, periodo, turma, disciplina);
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

        public bool EhDisciplinaGradeEletivaPor(DataContext contexto, decimal ano, decimal periodo, string turma, string disciplina)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*)
                                    FROM    LYCEUM.DBO.LY_TURMA T ( NOLOCK )
                                            INNER JOIN LY_CURSO C ( NOLOCK ) ON C.CURSO = T.CURSO
											INNER JOIN LY_SERIE S ON T.CURRICULO = S.CURRICULO
														AND T.TURNO = S.TURNO
														AND T.CURSO = S.CURSO  
														AND T.SERIE = S.SERIE                                        
                                    WHERE   T.ANO = @ANO
                                            AND T.SEMESTRE = @PERIODO
                                            AND T.TURMA = @TURMA
											AND T.DISCIPLINA = @DISCIPLINA
                                            AND ISNULL(T.ELETIVA,'N') = 'S'
                                            AND C.OFERTAELETIVA = 'S'
											AND S.OFERTAELETIVA = 'S' ";

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@DISCIPLINA", disciplina); 

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        /// <summary>
        /// Obtém dados das disciplinas da turma.
        /// </summary>
        /// <param name="conn">Conexão.</param>
        /// <param name="grade_id">Grade ID da turma.</param>
        /// <returns>QueryTable contendo horas_aula (horas_aula + horas_ativ + horas_estagio + horas_lab), creditos.</returns>
        public static QueryTable ObterDadosDisciplina(TConnection conn, string grade_id)
        {
            return Consultar(conn, @"
                    SELECT	d.disciplina,
                            isnull(d.HORAS_AULA,0) + isnull(d.HORAS_LAB,0) + isnull(d.HORAS_ATIV,0) + isnull(d.HORAS_ESTAGIO,0) horas_aula, 
                            isnull(d.CREDITOS,0) creditos
                    FROM	ly_grade_turma gt INNER JOIN 
		                    ly_disciplina d ON d.disciplina = gt.disciplina 
                    where GRADE_ID = ?", grade_id);
        }

        public static QueryTable ConsultarPorDocente(string docente, string curso, string turno, string curriculo, decimal serie)
        {
            QueryTable qt = null;

            if (!string.IsNullOrEmpty(docente)
                && !string.IsNullOrEmpty(curso)
                && !string.IsNullOrEmpty(turno)
                && !string.IsNullOrEmpty(curriculo)
                && serie != 0)
            {

                TConnection connection = Config.CreateConnection();

                connection.Open();

                string sql = " select D.DISCIPLINA, D.NOME " +
                             " from ly_grupo_habilitacao_doc doc inner join ly_grupo_habilitacao_disc disc " +
                             " on doc.agrupamento = disc.agrupamento " +
                             " inner join ly_disciplina D ON D.disciplina = disc.disciplina " +
                             " inner join ly_grade G ON G.disciplina = D.disciplina " +
                             " where num_func = ? " +
                             " and G.curso = ? and G.turno = ? and G.curriculo = ? and G.serie_ideal = ? " +
                             " and ((provisorio = 'N') or (provisorio = 'S' and dt_limite >= convert(date,getdate()))) ";
                try
                {
                    qt = new QueryTable(sql);

                    qt.Query(connection, docente, curso, turno, curriculo, serie);
                }
                finally
                {
                    connection.Close();
                }
            }

            return qt;
        }

        public static QueryTable ConsultarPorAluno(string aluno, string ano, string semestre)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = "SELECT DISTINCT m.disciplina, d.nome FROM ly_matricula m inner join ly_disciplina d on d.disciplina = m.disciplina WHERE m.aluno = ? AND m.ano = ? AND m.semestre = ?";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, aluno, ano, semestre);
            }
            finally
            {
                connection.Close();
            }


            return qt;
        }

        public static Ly_disciplina ConsultarDadosDisciplina(string disciplina)
        {
            var connection = Config.CreateConnection();

            connection.Open();

            return Ly_disciplina.Query(connection, "disciplina= ?", disciplina);
        }

        public LyDisciplina ObtemDisciplinaPor(string disciplina)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            LyDisciplina dadosDisciplina = new LyDisciplina();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  *
                            FROM    LY_DISCIPLINA
                            WHERE   DISCIPLINA = @DISCIPLINA ";

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);

                dadosDisciplina = ctx.TryToBindEntity<LyDisciplina>(contextQuery);

                return dadosDisciplina;
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

        public bool EhDisciplinaCadastradaPor(string disciplina)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                FROM    LY_DISCIPLINA
                                WHERE   DISCIPLINA = @DISCIPLINA ";

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);

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

        public ValidacaoDados ValidaInsercao(LyDisciplina disciplina)
        {
            List<string> mensagens = new List<string>();
            RN.Conceito rnConceito = new Conceito();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (disciplina == null)
            {
                return validacaoDados;
            }

            //Verifica campos obrigatorios
            if (string.IsNullOrEmpty(disciplina.Disciplina))
            {
                mensagens.Add("O CÓDIGO do Componente Curricular é de preenchimento obrigatório.");
            }

            if (string.IsNullOrEmpty(disciplina.Faculdade))
            {
                mensagens.Add("A FACULDADE do Componente Curricular é de preenchimento obrigatório.");
            }

            if (string.IsNullOrEmpty(disciplina.NotaMaxMedia))
            {
                mensagens.Add("A NOTAMAXMEDIA do Componente Curricular é de preenchimento obrigatório.");
            }

            if (string.IsNullOrEmpty(disciplina.Depto))
            {
                mensagens.Add("O DEPTO do Componente Curricular é de preenchimento obrigatório.");
            }

            if (string.IsNullOrEmpty(disciplina.TruncaMedia))
            {
                mensagens.Add("O TRUNCAMEDIA do Componente Curricular é de preenchimento obrigatório.");
            }

            if (string.IsNullOrEmpty(disciplina.Nome) || string.IsNullOrEmpty(disciplina.NomeCompl))
            {
                mensagens.Add("A COMPONENTE CURRICULAR é de preenchimento obrigatório.");
            }

            if (string.IsNullOrEmpty(disciplina.TemNota) || (disciplina.TemNota != "S" && disciplina.TemNota != "N"))
            {
                mensagens.Add("A opção TEM NOTA é de preenchimento obrigatório, com valores 'S' ou 'N'.");
            }

            if (string.IsNullOrEmpty(disciplina.TemFreq) || (disciplina.TemFreq != "S" && disciplina.TemFreq != "N"))
            {
                mensagens.Add("A opção TEM FREQUENCIA é de preenchimento obrigatório, com valores 'S' ou 'N'.");
            }

            if (string.IsNullOrEmpty(disciplina.VerificaHorario) || (disciplina.VerificaHorario != "S" && disciplina.VerificaHorario != "N"))
            {
                mensagens.Add("A opção VERIFICA HORARIO é de preenchimento obrigatório, com valores 'S' ou 'N'.");
            }

            if (string.IsNullOrEmpty(disciplina.Ativa) || (disciplina.Ativa != "S" && disciplina.Ativa != "N"))
            {
                mensagens.Add("A opção ATIVA é de preenchimento obrigatório, com valores 'S' ou 'N'.");
            }

            if (string.IsNullOrEmpty(disciplina.Estagio) || (disciplina.Estagio != "S" && disciplina.Estagio != "N"))
            {
                mensagens.Add("A opção ESTAGIO é de preenchimento obrigatório, com valores 'S' ou 'N'.");
            }

            if (string.IsNullOrEmpty(disciplina.Componente))
            {
                mensagens.Add("O COMPONENTE é de preenchimento obrigatório.");
            }

            if (string.IsNullOrEmpty(disciplina.NomeFantasia))
            {
                mensagens.Add("O CÓDIGO SARE é de preenchimento obrigatório.");
            }

            if (disciplina.HorasAula == null)
            {
                mensagens.Add("A HORA DE AULA TOTAL é de preenchimento obrigatório.");
            }
            else
            {
                if (disciplina.HorasAula > 9999)
                {
                    mensagens.Add("A HORA DE AULA TOTAL deve ser menor que 9999.");
                }
            }

            if (disciplina.HorasEstagio == null)
            {
                mensagens.Add("A HORA DE ESTÁGIO TOTAL é de preenchimento obrigatório.");
            }
            else
            {
                if (disciplina.HorasEstagio > 9999)
                {
                    mensagens.Add("A HORA DE ESTÁGIO TOTAL deve ser menor que 9999.");
                }
            }

            if (disciplina.HorasAtiv == null)
            {
                mensagens.Add("A HORA DE ATIVIDADE TOTAL é de preenchimento obrigatório.");
            }
            else
            {
                if (disciplina.HorasAtiv > 9999)
                {
                    mensagens.Add("A HORA DE ATIVIDADE TOTAL deve ser menor que 9999.");
                }
            }

            if (disciplina.AulasSemanais == null)
            {
                mensagens.Add("O TOTAL DE AULAS SEMANAIS é de preenchimento obrigatório.");
            }
            else
            {
                if (disciplina.AulasSemanais <= 0)
                {
                    mensagens.Add("O TOTAL DE AULAS SEMANAIS deve ser maior que zero");
                }

                if (disciplina.AulasSemanais > 9999)
                {
                    mensagens.Add("O TOTAL DE AULAS SEMANAIS deve ser menor que 9999.");
                }
            }

            if (disciplina.NotaMax == null)
            {
                mensagens.Add("A NOTA MÁXIMA é de preenchimento obrigatório.");
            }
            else
            {
                if (Convert.ToDecimal(disciplina.NotaMax) == 0)
                {
                    mensagens.Add("A NOTA MÁXIMA deve ser maior que 0(zero).");
                }
            }

            if (disciplina.Eletiva == "S" && disciplina.Grupo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Para Componente Curricular marcado como ELETIVA é necessário escolher um GRUPO.");
            }

            if (mensagens.Count == 0)
            {
                //Verificar se um dos campos de horas estão preenchidos
                if (disciplina.HorasAtiv <= 0 && disciplina.HorasAula <= 0 && disciplina.HorasEstagio <= 0)
                {
                    validacaoDados.Mensagem = "Um dos campos Hora de Aula Total, Hora de Estágio Total ou Hora de Atividade Total deve ser maior que zero.";
                    return validacaoDados;
                }

                //Verificar se existe outra disciplina com mesmo codigo
                if (EhDisciplinaCadastradaPor(disciplina.Disciplina))
                {
                    mensagens.Add("Já existe um Componente Curricular cadastrado com este código.");
                }

                if (!string.IsNullOrEmpty(disciplina.GrupoNota))
                {
                    //Caso a disciplina possuia grupo Nota, verifica se o grupo ainda existe
                    if (!rnConceito.EhConceitoCadastradoPor(disciplina.GrupoNota))
                    {
                        mensagens.Add("Grupo nota foi excluido. Favor escolher outro.");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(disciplina.NotaMax))
                        {
                            if (!rnConceito.EhConceitoCadastradoPor(disciplina.GrupoNota, disciplina.NotaMax))
                            {
                                mensagens.Add("Nota máxima foi excluida do grupo de conceito. Favor escolher outra.");
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

        public ValidacaoDados ValidaAlteracao(LyDisciplina disciplina, string temNotaAtual, string temFreqAtual)
        {
            List<string> mensagens = new List<string>();
            RN.Conceito rnConceito = new Conceito();
            RN.Nota rnNota = new Nota();
            RN.Falta rnFalta = new Falta();
            RN.DisciplinaMultipla rnDisciplinaMultipla = new DisciplinaMultipla();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (disciplina == null)
            {
                return validacaoDados;
            }

            //Verifica campos obrigatorios
            if (string.IsNullOrEmpty(disciplina.Disciplina))
            {
                mensagens.Add("O CÓDIGO do Componente Curricular é de preenchimento obrigatório.");
            }

            if (string.IsNullOrEmpty(disciplina.Nome))
            {
                mensagens.Add("O COMPONENTE CURRICULAR é de preenchimento obrigatório.");
            }

            if (string.IsNullOrEmpty(disciplina.TemNota) || (disciplina.TemNota != "S" && disciplina.TemNota != "N"))
            {
                mensagens.Add("A opção TEM NOTA é de preenchimento obrigatório, com valores 'S' ou 'N'.");
            }

            if (string.IsNullOrEmpty(disciplina.TemFreq) || (disciplina.TemFreq != "S" && disciplina.TemFreq != "N"))
            {
                mensagens.Add("A opção TEM FREQUENCIA é de preenchimento obrigatório, com valores 'S' ou 'N'.");
            }

            if (string.IsNullOrEmpty(disciplina.VerificaHorario) || (disciplina.VerificaHorario != "S" && disciplina.VerificaHorario != "N"))
            {
                mensagens.Add("A opção VERIFICA HORARIO é de preenchimento obrigatório, com valores 'S' ou 'N'.");
            }

            if (string.IsNullOrEmpty(disciplina.Ativa) || (disciplina.Ativa != "S" && disciplina.Ativa != "N"))
            {
                mensagens.Add("A opção ATIVA é de preenchimento obrigatório, com valores 'S' ou 'N'.");
            }

            if (string.IsNullOrEmpty(disciplina.Estagio) || (disciplina.Estagio != "S" && disciplina.Estagio != "N"))
            {
                mensagens.Add("A opção ESTAGIO é de preenchimento obrigatório, com valores 'S' ou 'N'.");
            }

            if (string.IsNullOrEmpty(disciplina.Componente))
            {
                mensagens.Add("O COMPONENTE é de preenchimento obrigatório.");
            }

            if (string.IsNullOrEmpty(disciplina.NomeFantasia))
            {
                mensagens.Add("O CÓDIGO SARE é de preenchimento obrigatório.");
            }

            if (disciplina.HorasAula == null)
            {
                mensagens.Add("A HORA DE AULA TOTAL é de preenchimento obrigatório.");
            }
            else
            {
                if (disciplina.HorasAula > 9999)
                {
                    mensagens.Add("A HORA DE AULA TOTAL deve ser menor que 9999.");
                }
            }

            if (disciplina.HorasEstagio == null)
            {
                mensagens.Add("A HORA DE ESTÁGIO TOTAL é de preenchimento obrigatório.");
            }
            else
            {
                if (disciplina.HorasEstagio > 9999)
                {
                    mensagens.Add("A HORA DE ESTÁGIO TOTAL deve ser menor que 9999.");
                }
            }

            if (disciplina.HorasAtiv == null)
            {
                mensagens.Add("A HORA DE ATIVIDADE TOTAL é de preenchimento obrigatório.");
            }
            else
            {
                if (disciplina.HorasAtiv > 9999)
                {
                    mensagens.Add("A HORA DE ATIVIDADE TOTAL deve ser menor que 9999.");
                }
            }

            if (disciplina.AulasSemanais == null)
            {
                mensagens.Add("O TOTAL DE AULAS SEMANAIS é de preenchimento obrigatório.");
            }
            else
            {
                if (disciplina.AulasSemanais <= 0)
                {
                    mensagens.Add("O TOTAL DE AULAS SEMANAIS deve ser maior que zero");
                }

                if (disciplina.AulasSemanais > 9999)
                {
                    mensagens.Add("O TOTAL DE AULAS SEMANAIS deve ser menor que 9999.");
                }
            }

            if (disciplina.NotaMax == null)
            {
                mensagens.Add("A NOTA MÁXIMA é de preenchimento obrigatório.");
            }
            else
            {
                if (Convert.ToDecimal(disciplina.NotaMax) == 0)
                {
                    mensagens.Add("A NOTA MÁXIMA deve ser maior que 0(zero).");
                }
            }

            if (disciplina.Eletiva == "S" && disciplina.Grupo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Para Componente Curricular marcado como ELETIVA é necessário escolher um GRUPO.");
            }

            if (mensagens.Count == 0)
            {
                //Verificar se um dos campos de horas estão preenchidos
                if (disciplina.HorasAtiv <= 0 && disciplina.HorasAula <= 0 && disciplina.HorasEstagio <= 0)
                {
                    validacaoDados.Mensagem = "Um dos campos Hora de Aula Total, Hora de Estágio Total ou Hora de Atividade Total deve ser maior que zero.";
                    return validacaoDados;
                }

                if (!string.IsNullOrEmpty(disciplina.GrupoNota))
                {
                    //Caso a disciplina possuia grupo Nota, verifica se o grupo ainda existe
                    if (!rnConceito.EhConceitoCadastradoPor(disciplina.GrupoNota))
                    {
                        mensagens.Add("Grupo nota foi excluido. Favor escolher outro.");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(disciplina.NotaMax))
                        {
                            if (!rnConceito.EhConceitoCadastradoPor(disciplina.GrupoNota, disciplina.NotaMax))
                            {
                                mensagens.Add("Nota máxima foi excluida do grupo de conceito. Favor escolher outra.");
                            }
                        }
                    }
                }

                //Verifica se disciplina já pertence a grupo de tipo diferente
                if (PossuiOutroTipoDisciplinaGrupoPor(disciplina.Disciplina, disciplina.Campo01))
                {
                    mensagens.Add(string.Format("Atividade Complementar: Disciplina não pode ser do tipo {0} pois está em um grupo de tipo diferente.", disciplina.Campo01));
                }

                if (disciplina.TemNota == "N")
                {
                    //Caso a marcação de TEM NOTA esteja N, verificar se já existia notas cadastradas para a disciplina
                    if (rnNota.PossuiNotaPor(disciplina.Disciplina))
                    {
                        mensagens.Add("Não é possível remover a opção Pontuação pois existem notas lançadas para estw Componente Curricular. Um novo Componente Curricular deverá ser criada.");
                    }
                }

                if (disciplina.TemFreq == "N")
                {
                    //Caso a marcação de TEM FREQ esteja N, verificar se já existia faltas cadastradas para a disciplina
                    if (rnFalta.PossuiFaltaPor(disciplina.Disciplina))
                    {
                        mensagens.Add("Não é possível remover a opção Frequência pois existem faltas lançadas para esta disciplina. Um novo Componente Curricular deverá ser criada.");
                    }
                }

                if (Convert.ToString(disciplina.Multipla) != "S")
                {
                    //Caso não possui a marcação de DISCIPLINA MULTIPLA, verificar se já existia multiplas cadastradas para a disciplina
                    if (rnDisciplinaMultipla.PossuiMultiplaPor(disciplina.Disciplina))
                    {
                        mensagens.Add("Não é possível remover a opção DISCIPLINA MÚLTIPLA pois já existem múltiplas cadastradas para este Componente Curricular.");
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

        public bool PossuiPresencaSemCartaoPor(string disciplina)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                            FROM    dbo.LY_PRESENCA_SEM_CARTAO
                            WHERE   DISCIPLINA = @DISCIPLINA ";

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);

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

        public ValidacaoDados ValidaRemocao(string disciplina)
        {
            List<string> mensagens = new List<string>();
            RN.Turma rnTurma = new Turma();
            RN.HistMatricula rnHistMatricula = new HistMatricula();
            RN.GrupoHabilitacao rnGrupoHabilitacao = new GrupoHabilitacao();
            RN.DisciplinaMultipla rnDisciplinaMultipla = new DisciplinaMultipla();
            RN.Grade rnGrade = new Grade();
            RN.Prova rnProva = new Prova();
            RN.Freq rnFreq = new Freq();
            RN.GradeTurma rnGradeTurma = new GradeTurma();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (string.IsNullOrEmpty(disciplina))
            {
                return validacaoDados;
            }

            //Verificar se existe a disciplina
            if (!EhDisciplinaCadastradaPor(disciplina))
            {
                mensagens.Add("Não foi encontrada um Componente Curricular cadastrada com este código.");
            }

            if (mensagens.Count == 0)
            {
                if (rnGrupoHabilitacao.ExisteGrupoHabilitacaoPor(disciplina))
                {
                    mensagens.Add("Não é possível remover o Componente Curricular pois existe Componente Curricular Habilitado por Grupo para este componente.");
                }

                if (rnDisciplinaMultipla.ExisteDisciplinaMultiplaPor(disciplina))
                {
                    mensagens.Add("Não é possível remover o Componente Curricular pois existe Disciplina múltipla cadastrada para este componente.");
                }

                if (rnTurma.ExisteTurmaPor(disciplina))
                {
                    mensagens.Add("Não é possível remover o Componente Curricular pois existe Turma cadastrada com este componente.");
                }

                if (rnHistMatricula.ExisteHistoricoPor(disciplina))
                {
                    mensagens.Add("Não é possível remover o Componente Curricular pois existe Histórico de matrícula cadastrado para este Componente.");
                }

                if (rnGrade.ExisteGradePor(disciplina))
                {
                    mensagens.Add("Não é possível remover o Componente Curricular pois existe Grade cadastrada para este componente.");
                }

                if (rnGradeTurma.ExisteGradeTurmaPor(disciplina))
                {
                    mensagens.Add("Não é possível remover o Componente Curricular pois existe Grade Turma cadastrada para este componente.");
                }

                if (rnProva.ExisteProvaPor(disciplina))
                {
                    mensagens.Add("Não é possível remover o Componente Curricular pois existe prova cadastrada para este componente.");
                }

                if (rnFreq.ExisteFreqPor(disciplina))
                {
                    mensagens.Add("Não é possível remover o Componente Curricular pois existe frequencia cadastrada para este componente.");
                }

                if (this.PossuiPresencaSemCartaoPor(disciplina))
                {
                    mensagens.Add("Não é possível remover o Componente Curricular pois existe Presença Sem Cartão cadastrada para este componente.");
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

        public void InsereDisciplina(LyDisciplina disciplina)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Insere disciplina
                this.InsereDisciplina(ctx, disciplina);
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

        public string ObtemComponentePor(DataContext contexto, string disciplina)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT COMPONENTE
                                      FROM LY_DISCIPLINA
                                      WHERE DISCIPLINA = @DISCIPLINA ";

            contextQuery.Parameters.Add("@DISCIPLINA", TechneDbType.T_CODIGO, disciplina); 
 
            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        private void InsereDisciplina(DataContext ctx, LyDisciplina disciplina)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT  INTO dbo.LY_DISCIPLINA
                                ( DISCIPLINA ,
                                  FACULDADE ,
                                  DEPTO ,
                                  NOME ,
                                  NOME_COMPL ,
                                  CREDITOS ,
                                  TEM_NOTA ,
                                  TIPO_NOTA ,
                                  TEM_FREQ ,
                                  PRIORIZA_FREQ ,
                                  VERIFICA_HORARIO ,
                                  ATIVA ,
                                  FORMULA_MF1 ,
                                  FORMULA_MF2 ,
                                  FORMULA_MF3 ,
                                  FORMULA_CA1 ,
                                  FORMULA_CA2 ,
                                  FORMULA_CA3 ,
                                  CONCEITO_MIN_1 ,
                                  CONCEITO_MIN_2 ,
                                  CONCEITO_MIN_3 ,
                                  CONCEITO_MIN_EX ,
                                  CONCEITO_MIN_EX_2 ,
                                  PERC_PRESMIN ,
                                  SERVICO ,
                                  ESTAGIO ,
                                  HORAS_AULA ,
                                  HORAS_LAB ,
                                  HORAS_ATIV ,
                                  HORAS_ESTAGIO ,
                                  PRAZO_REVISAO ,
                                  FORMULA_PREREQ ,
                                  FORMULA_EQUIV ,
                                  TIPO ,
                                  GRUPO_NOTA ,
                                  GRUPO_MEDIA ,
                                  N_CASAS_DEC ,
                                  NOTA_MAX ,
                                  N_CASAS_DEC_MEDIA ,
                                  NOTA_MAX_MEDIA ,
                                  AULAS_SEMANAIS ,
                                  AULAS_SEM_AULA ,
                                  AULAS_SEM_LAB ,
                                  AULAS_SEM_ATIV ,
                                  TRUNCA_MEDIA ,
                                  FALTA_DIARIA ,
                                  PRAZO_DIVULGACAO ,
                                  PIM ,
                                  COPIA_NOTA_SUBTURMA ,
                                  NOME_FANTASIA ,
                                  AVAL_COMPETENCIA ,
                                  STAMP_ATUALIZACAO ,
                                  TEM_AVAL_DESCRITIVA ,
                                  COMPONENTE ,
                                  AREA_CONHECIMENTO ,
                                  PERMITE_MANTER_HORARIO ,
                                  MULTIPLA ,
                                  CAMPO_01 ,
                                  CATEGORIA_ENTURMACAO ,
                                  OBS_FORMULA_MF1 ,
                                  OBS_FORMULA_MF2 ,
                                  OBS_FORMULA_MF3,
                                  ELETIVA,
                                  GRUPO
                                )
                        VALUES  ( @DISCIPLINA ,
                                  @FACULDADE ,
                                  @DEPTO ,
                                  @NOME ,
                                  @NOME_COMPL ,
                                  @CREDITOS ,
                                  @TEM_NOTA ,
                                  @TIPO_NOTA ,
                                  @TEM_FREQ ,
                                  @PRIORIZA_FREQ ,
                                  @VERIFICA_HORARIO ,
                                  @ATIVA ,
                                  @FORMULA_MF1 ,
                                  @FORMULA_MF2 ,
                                  @FORMULA_MF3 ,
                                  @FORMULA_CA1 ,
                                  @FORMULA_CA2 ,
                                  @FORMULA_CA3 ,
                                  @CONCEITO_MIN_1 ,
                                  @CONCEITO_MIN_2 ,
                                  @CONCEITO_MIN_3 ,
                                  @CONCEITO_MIN_EX ,
                                  @CONCEITO_MIN_EX_2 ,
                                  @PERC_PRESMIN ,
                                  @SERVICO ,
                                  @ESTAGIO ,
                                  @HORAS_AULA ,
                                  @HORAS_LAB ,
                                  @HORAS_ATIV ,
                                  @HORAS_ESTAGIO ,
                                  @PRAZO_REVISAO ,
                                  @FORMULA_PREREQ ,
                                  @FORMULA_EQUIV ,
                                  @TIPO ,
                                  @GRUPO_NOTA ,
                                  @GRUPO_MEDIA ,
                                  @N_CASAS_DEC ,
                                  @NOTA_MAX ,
                                  @N_CASAS_DEC_MEDIA ,
                                  @NOTA_MAX_MEDIA ,
                                  @AULAS_SEMANAIS ,
                                  @AULAS_SEM_AULA ,
                                  @AULAS_SEM_LAB ,
                                  @AULAS_SEM_ATIV ,
                                  @TRUNCA_MEDIA ,
                                  @FALTA_DIARIA ,
                                  @PRAZO_DIVULGACAO ,
                                  @PIM ,
                                  @COPIA_NOTA_SUBTURMA ,
                                  @NOME_FANTASIA ,
                                  @AVAL_COMPETENCIA ,
                                  @STAMP_ATUALIZACAO ,
                                  @TEM_AVAL_DESCRITIVA ,
                                  @COMPONENTE ,
                                  @AREA_CONHECIMENTO ,
                                  @PERMITE_MANTER_HORARIO ,
                                  @MULTIPLA ,
                                  @CAMPO_01 ,
                                  @CATEGORIA_ENTURMACAO ,
                                  @OBS_FORMULA_MF1 ,
                                  @OBS_FORMULA_MF2 ,
                                  @OBS_FORMULA_MF3 ,
                                  @ELETIVA,
                                  @GRUPO
                                ) ";

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina.Disciplina);
                contextQuery.Parameters.Add("@FACULDADE", disciplina.Faculdade);
                contextQuery.Parameters.Add("@DEPTO", disciplina.Depto);
                contextQuery.Parameters.Add("@NOME", disciplina.Nome);
                contextQuery.Parameters.Add("@NOME_COMPL", disciplina.NomeCompl);
                contextQuery.Parameters.Add("@CREDITOS", disciplina.Creditos);
                contextQuery.Parameters.Add("@TEM_NOTA", disciplina.TemNota);
                contextQuery.Parameters.Add("@TIPO_NOTA", disciplina.TipoNota);
                contextQuery.Parameters.Add("@TEM_FREQ", disciplina.TemFreq);
                contextQuery.Parameters.Add("@PRIORIZA_FREQ", disciplina.PriorizaFreq);
                contextQuery.Parameters.Add("@VERIFICA_HORARIO", disciplina.VerificaHorario);
                contextQuery.Parameters.Add("@ATIVA", disciplina.Ativa);
                contextQuery.Parameters.Add("@FORMULA_MF1", disciplina.FormulaMf1);
                contextQuery.Parameters.Add("@FORMULA_MF2", disciplina.FormulaMf2);
                contextQuery.Parameters.Add("@FORMULA_MF3", disciplina.FormulaMf3);
                contextQuery.Parameters.Add("@FORMULA_CA1", disciplina.FormulaCa1);
                contextQuery.Parameters.Add("@FORMULA_CA2", disciplina.FormulaCa2);
                contextQuery.Parameters.Add("@FORMULA_CA3", disciplina.FormulaCa3);
                contextQuery.Parameters.Add("@CONCEITO_MIN_1", disciplina.ConceitoMin1);
                contextQuery.Parameters.Add("@CONCEITO_MIN_2", disciplina.ConceitoMin2);
                contextQuery.Parameters.Add("@CONCEITO_MIN_3", disciplina.ConceitoMin3);
                contextQuery.Parameters.Add("@CONCEITO_MIN_EX", disciplina.ConceitoMinEx);
                contextQuery.Parameters.Add("@CONCEITO_MIN_EX_2", disciplina.ConceitoMinEx2);
                contextQuery.Parameters.Add("@PERC_PRESMIN", disciplina.PercPresmin);
                contextQuery.Parameters.Add("@SERVICO", disciplina.Servico);
                contextQuery.Parameters.Add("@ESTAGIO", disciplina.Estagio);
                contextQuery.Parameters.Add("@HORAS_AULA", disciplina.HorasAula);
                contextQuery.Parameters.Add("@HORAS_LAB", disciplina.HorasLab);
                contextQuery.Parameters.Add("@HORAS_ATIV", disciplina.HorasAtiv);
                contextQuery.Parameters.Add("@HORAS_ESTAGIO", disciplina.HorasEstagio);
                contextQuery.Parameters.Add("@PRAZO_REVISAO", disciplina.PrazoRevisao);
                contextQuery.Parameters.Add("@FORMULA_PREREQ", disciplina.FormulaPrereq);
                contextQuery.Parameters.Add("@FORMULA_EQUIV", disciplina.FormulaEquiv);
                contextQuery.Parameters.Add("@TIPO", disciplina.Tipo);
                contextQuery.Parameters.Add("@GRUPO_NOTA", disciplina.GrupoNota);
                contextQuery.Parameters.Add("@GRUPO_MEDIA", disciplina.GrupoMedia);
                contextQuery.Parameters.Add("@N_CASAS_DEC", disciplina.NCasasDec);
                contextQuery.Parameters.Add("@NOTA_MAX", disciplina.NotaMax);
                contextQuery.Parameters.Add("@N_CASAS_DEC_MEDIA", disciplina.NCasasDecMedia);
                contextQuery.Parameters.Add("@NOTA_MAX_MEDIA", disciplina.NotaMaxMedia);
                contextQuery.Parameters.Add("@AULAS_SEMANAIS", disciplina.AulasSemanais);
                contextQuery.Parameters.Add("@AULAS_SEM_AULA", disciplina.AulasSemAula);
                contextQuery.Parameters.Add("@AULAS_SEM_LAB", disciplina.AulasSemLab);
                contextQuery.Parameters.Add("@AULAS_SEM_ATIV", disciplina.AulasSemAtiv);
                contextQuery.Parameters.Add("@TRUNCA_MEDIA", disciplina.TruncaMedia);
                contextQuery.Parameters.Add("@FALTA_DIARIA", disciplina.FaltaDiaria);
                contextQuery.Parameters.Add("@PRAZO_DIVULGACAO", disciplina.PrazoDivulgacao);
                contextQuery.Parameters.Add("@PIM", disciplina.Pim);
                contextQuery.Parameters.Add("@COPIA_NOTA_SUBTURMA", disciplina.CopiaNotaSubturma);
                contextQuery.Parameters.Add("@NOME_FANTASIA", disciplina.NomeFantasia);
                contextQuery.Parameters.Add("@AVAL_COMPETENCIA", disciplina.AvalCompetencia);
                contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", DateTime.Now);
                contextQuery.Parameters.Add("@TEM_AVAL_DESCRITIVA", disciplina.TemAvalDescritiva);
                contextQuery.Parameters.Add("@COMPONENTE", disciplina.Componente);
                contextQuery.Parameters.Add("@AREA_CONHECIMENTO", disciplina.AreaConhecimento);
                contextQuery.Parameters.Add("@PERMITE_MANTER_HORARIO", disciplina.PermiteManterHorario);
                contextQuery.Parameters.Add("@MULTIPLA", disciplina.Multipla);
                contextQuery.Parameters.Add("@CAMPO_01", disciplina.Campo01);
                contextQuery.Parameters.Add("@CATEGORIA_ENTURMACAO", disciplina.CategoriaEnturmacao);
                contextQuery.Parameters.Add("@OBS_FORMULA_MF1", disciplina.ObsFormulaMf1);
                contextQuery.Parameters.Add("@OBS_FORMULA_MF2", disciplina.ObsFormulaMf2);
                contextQuery.Parameters.Add("@OBS_FORMULA_MF3", disciplina.ObsFormulaMf3);
                contextQuery.Parameters.Add("@ELETIVA", disciplina.Eletiva);
                contextQuery.Parameters.Add("@GRUPO", disciplina.Grupo);

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

        public void AlteraDisciplina(LyDisciplina disciplina, string temNotaAtual, string temFreqAtual)
        {
            RN.Prova rnProva = new Prova();
            RN.Freq rnFreq = new Freq();
            DataTable listaBimestres = new DataTable();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            try
            {
                //Altera disciplina
                this.AlteraDisciplina(ctx, disciplina);

                //Verifica se a opção TEM NOTA está sendo alterada
                if (disciplina.TemNota != temNotaAtual)
                {
                    if (disciplina.TemNota == "S")
                    {
                        //Caso a opção TEM NOTA da disciplina esteja sendo alterada para S

                        //Busca bimestre do ano atual com provas ja geradas
                        listaBimestres = rnProva.ListaBimestresComProvaPor(DateTime.Now.Year);

                        //Gera prova para bimestres do ano atual
                        if (listaBimestres.Rows.Count > 0)
                        {
                            foreach (DataRow dr in listaBimestres.Rows)
                            {
                                int bimestre =  Convert.ToInt32(dr["SUBPERIODO"]);
                                int periodo =  Convert.ToInt32(dr["SEMESTRE"]);

                                rnProva.SpGeraProvas(ctx, DateTime.Now.Year, periodo, bimestre, disciplina.Disciplina);
                            }
                        }
                    }
                    else
                    {
                        //Caso a opção TEM NOTA da disciplina esteja sendo alterada para N, excluir provas já geradas
                        rnProva.RemoveProvaPor(ctx, disciplina.Disciplina);
                    }

                    if (disciplina.TemFreq == "S")
                    {
                        //Busca bimestre do ano atual com freqs ja geradas
                        listaBimestres = rnFreq.ListaBimestresComFreqPor(DateTime.Now.Year);

                        //Gera freq para bimestres do ano atual
                        if (listaBimestres.Rows.Count > 0)
                        {
                            foreach (DataRow dr in listaBimestres.Rows)
                            {
                                int bimestre = Convert.ToInt32(dr["SUBPERIODO"]);
                                int periodo = Convert.ToInt32(dr["PERIODO"]);

                                rnFreq.SpGeraFrequencia(ctx, DateTime.Now.Year, periodo, bimestre, disciplina.Disciplina);
                            }
                        }
                    }
                    else
                    {
                        //Caso a opção TEM FREQ da disciplina esteja sendo alterada para N, excluir freqs já geradas
                        rnFreq.RemoveFreqPor(ctx, disciplina.Disciplina);
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

        private void AlteraDisciplina(DataContext ctx, LyDisciplina disciplina)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE  LY_DISCIPLINA
                        SET     NOME = @NOME ,
                                NOME_COMPL = @NOME_COMPL ,
                                TEM_NOTA = @TEM_NOTA ,
                                TIPO_NOTA = @TIPO_NOTA ,
                                TEM_FREQ = @TEM_FREQ ,
                                PRIORIZA_FREQ = @PRIORIZA_FREQ ,
                                VERIFICA_HORARIO = @VERIFICA_HORARIO ,
                                ATIVA = @ATIVA ,
                                FORMULA_MF1 = @FORMULA_MF1 ,
                                FORMULA_MF2 = @FORMULA_MF2 ,
                                FORMULA_MF3 = @FORMULA_MF3 ,
                                FORMULA_CA1 = @FORMULA_CA1 ,
                                FORMULA_CA2 = @FORMULA_CA2 ,
                                FORMULA_CA3 = @FORMULA_CA3 ,
                                CONCEITO_MIN_1 = @CONCEITO_MIN_1 ,
                                CONCEITO_MIN_2 = @CONCEITO_MIN_2 ,
                                CONCEITO_MIN_3 = @CONCEITO_MIN_3 ,
                                CONCEITO_MIN_EX = @CONCEITO_MIN_EX ,
                                CONCEITO_MIN_EX_2 = @CONCEITO_MIN_EX_2 ,
                                PERC_PRESMIN = @PERC_PRESMIN ,
                                SERVICO = @SERVICO ,
                                ESTAGIO = @ESTAGIO ,
                                HORAS_AULA = @HORAS_AULA ,
                                HORAS_LAB = @HORAS_LAB ,
                                HORAS_ATIV = @HORAS_ATIV ,
                                HORAS_ESTAGIO = @HORAS_ESTAGIO ,
                                PRAZO_REVISAO = @PRAZO_REVISAO ,
                                FORMULA_PREREQ = @FORMULA_PREREQ ,
                                FORMULA_EQUIV = @FORMULA_EQUIV ,
                                TIPO = @TIPO ,
                                GRUPO_NOTA = @GRUPO_NOTA ,
                                GRUPO_MEDIA = @GRUPO_MEDIA ,
                                N_CASAS_DEC = @N_CASAS_DEC ,
                                NOTA_MAX = @NOTA_MAX ,
                                AULAS_SEMANAIS = @AULAS_SEMANAIS ,
                                AULAS_SEM_AULA = @AULAS_SEM_AULA ,
                                AULAS_SEM_LAB = @AULAS_SEM_LAB ,
                                AULAS_SEM_ATIV = @AULAS_SEM_ATIV ,
                                TRUNCA_MEDIA = @TRUNCA_MEDIA ,
                                FALTA_DIARIA = @FALTA_DIARIA ,
                                PRAZO_DIVULGACAO = @PRAZO_DIVULGACAO ,
                                PIM = @PIM ,
                                COPIA_NOTA_SUBTURMA = @COPIA_NOTA_SUBTURMA ,
                                NOME_FANTASIA = @NOME_FANTASIA ,
                                AVAL_COMPETENCIA = @AVAL_COMPETENCIA ,
                                STAMP_ATUALIZACAO = @STAMP_ATUALIZACAO ,
                                TEM_AVAL_DESCRITIVA = @TEM_AVAL_DESCRITIVA ,
                                COMPONENTE = @COMPONENTE ,
                                AREA_CONHECIMENTO = @AREA_CONHECIMENTO ,
                                PERMITE_MANTER_HORARIO = @PERMITE_MANTER_HORARIO ,
                                MULTIPLA = @MULTIPLA ,
                                CAMPO_01 = @CAMPO_01 ,
                                CATEGORIA_ENTURMACAO = @CATEGORIA_ENTURMACAO ,
                                OBS_FORMULA_MF1 = @OBS_FORMULA_MF1 ,
                                OBS_FORMULA_MF2 = @OBS_FORMULA_MF2 ,
                                OBS_FORMULA_MF3 = @OBS_FORMULA_MF3 ,
                                ELETIVA = @ELETIVA,
                                GRUPO = @GRUPO
                        WHERE   DISCIPLINA = @DISCIPLINA ";

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina.Disciplina);
                contextQuery.Parameters.Add("@NOME", disciplina.Nome);
                contextQuery.Parameters.Add("@NOME_COMPL", disciplina.NomeCompl);
                contextQuery.Parameters.Add("@TEM_NOTA", disciplina.TemNota);
                contextQuery.Parameters.Add("@TIPO_NOTA", disciplina.TipoNota);
                contextQuery.Parameters.Add("@TEM_FREQ", disciplina.TemFreq);
                contextQuery.Parameters.Add("@PRIORIZA_FREQ", disciplina.PriorizaFreq);
                contextQuery.Parameters.Add("@VERIFICA_HORARIO", disciplina.VerificaHorario);
                contextQuery.Parameters.Add("@ATIVA", disciplina.Ativa);
                contextQuery.Parameters.Add("@FORMULA_MF1", disciplina.FormulaMf1);
                contextQuery.Parameters.Add("@FORMULA_MF2", disciplina.FormulaMf2);
                contextQuery.Parameters.Add("@FORMULA_MF3", disciplina.FormulaMf3);
                contextQuery.Parameters.Add("@FORMULA_CA1", disciplina.FormulaCa1);
                contextQuery.Parameters.Add("@FORMULA_CA2", disciplina.FormulaCa2);
                contextQuery.Parameters.Add("@FORMULA_CA3", disciplina.FormulaCa3);
                contextQuery.Parameters.Add("@CONCEITO_MIN_1", disciplina.ConceitoMin1);
                contextQuery.Parameters.Add("@CONCEITO_MIN_2", disciplina.ConceitoMin2);
                contextQuery.Parameters.Add("@CONCEITO_MIN_3", disciplina.ConceitoMin3);
                contextQuery.Parameters.Add("@CONCEITO_MIN_EX", disciplina.ConceitoMinEx);
                contextQuery.Parameters.Add("@CONCEITO_MIN_EX_2", disciplina.ConceitoMinEx2);
                contextQuery.Parameters.Add("@PERC_PRESMIN", disciplina.PercPresmin);
                contextQuery.Parameters.Add("@SERVICO", disciplina.Servico);
                contextQuery.Parameters.Add("@ESTAGIO", disciplina.Estagio);
                contextQuery.Parameters.Add("@HORAS_AULA", disciplina.HorasAula);
                contextQuery.Parameters.Add("@HORAS_LAB", disciplina.HorasLab);
                contextQuery.Parameters.Add("@HORAS_ATIV", disciplina.HorasAtiv);
                contextQuery.Parameters.Add("@HORAS_ESTAGIO", disciplina.HorasEstagio);
                contextQuery.Parameters.Add("@PRAZO_REVISAO", disciplina.PrazoRevisao);
                contextQuery.Parameters.Add("@FORMULA_PREREQ", disciplina.FormulaPrereq);
                contextQuery.Parameters.Add("@FORMULA_EQUIV", disciplina.FormulaEquiv);
                contextQuery.Parameters.Add("@TIPO", disciplina.Tipo);
                contextQuery.Parameters.Add("@GRUPO_NOTA", disciplina.GrupoNota);
                contextQuery.Parameters.Add("@GRUPO_MEDIA", disciplina.GrupoMedia);
                contextQuery.Parameters.Add("@N_CASAS_DEC", disciplina.NCasasDec);
                contextQuery.Parameters.Add("@NOTA_MAX", disciplina.NotaMax);
                contextQuery.Parameters.Add("@AULAS_SEMANAIS", TechneDbType.T_DECIMAL_PRECISO, disciplina.AulasSemanais);
                contextQuery.Parameters.Add("@AULAS_SEM_AULA", TechneDbType.T_DECIMAL_PRECISO, disciplina.AulasSemAula);
                contextQuery.Parameters.Add("@AULAS_SEM_LAB", disciplina.AulasSemLab);
                contextQuery.Parameters.Add("@AULAS_SEM_ATIV", disciplina.AulasSemAtiv);
                contextQuery.Parameters.Add("@TRUNCA_MEDIA", disciplina.TruncaMedia);
                contextQuery.Parameters.Add("@FALTA_DIARIA", disciplina.FaltaDiaria);
                contextQuery.Parameters.Add("@PRAZO_DIVULGACAO", disciplina.PrazoDivulgacao);
                contextQuery.Parameters.Add("@PIM", disciplina.Pim);
                contextQuery.Parameters.Add("@COPIA_NOTA_SUBTURMA", disciplina.CopiaNotaSubturma);
                contextQuery.Parameters.Add("@NOME_FANTASIA", disciplina.NomeFantasia);
                contextQuery.Parameters.Add("@AVAL_COMPETENCIA", disciplina.AvalCompetencia);
                contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", DateTime.Now);
                contextQuery.Parameters.Add("@TEM_AVAL_DESCRITIVA", disciplina.TemAvalDescritiva);
                contextQuery.Parameters.Add("@COMPONENTE", disciplina.Componente);
                contextQuery.Parameters.Add("@AREA_CONHECIMENTO", disciplina.AreaConhecimento);
                contextQuery.Parameters.Add("@PERMITE_MANTER_HORARIO", disciplina.PermiteManterHorario);
                contextQuery.Parameters.Add("@MULTIPLA", disciplina.Multipla);
                contextQuery.Parameters.Add("@CAMPO_01", disciplina.Campo01);
                contextQuery.Parameters.Add("@CATEGORIA_ENTURMACAO", disciplina.CategoriaEnturmacao);
                contextQuery.Parameters.Add("@OBS_FORMULA_MF1", disciplina.ObsFormulaMf1);
                contextQuery.Parameters.Add("@OBS_FORMULA_MF2", disciplina.ObsFormulaMf2);
                contextQuery.Parameters.Add("@OBS_FORMULA_MF3", disciplina.ObsFormulaMf3);
                contextQuery.Parameters.Add("@ELETIVA", disciplina.Eletiva);
                contextQuery.Parameters.Add("@GRUPO", disciplina.Grupo);

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

        public void RemoveDisciplina(string disciplina)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                this.RemoveDisciplina(ctx, disciplina);
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

        private void RemoveDisciplina(DataContext ctx, string disciplina)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE  LY_DISCIPLINA
                        WHERE   DISCIPLINA = @DISCIPLINA ";

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);

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

        public static QueryTable ConsultarDepto(string faculdade)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = "SELECT DISTINCT depto, nome FROM ly_depto WHERE faculdade = ?";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, faculdade);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public static QueryTable ConsultarTipo()
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = "SELECT tipo, descricao FROM ly_tipo_disciplina";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public static bool ExisteGrupoNotaDisciplina(string grupo_nota, string conceito)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = "select 1 from ly_disciplina d inner join ly_conceito c on c.grupo = d.grupo_nota and d.NOTA_MAX = c.CONCEITO " +
                          "where GRUPO = ? and conceito = ? " +
                          "union all " +
                          "select 1 from ly_disciplina d inner join ly_conceito c on c.grupo = d.GRUPO_MEDIA and d.NOTA_MAX_MEDIA = c.CONCEITO " +
                          "where GRUPO = ? and conceito = ? ";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, grupo_nota, conceito, grupo_nota, conceito);
            }
            finally
            {
                connection.Close();
            }

            return qt.Rows.Count > 0;
        }

        public static QueryTable ListaDisciplinas()
        {
            TConnection connection = Config.CreateConnection();

            QueryTable tab = new QueryTable("select distinct disciplina, (disciplina +' - '+ nome) as nome from ly_disciplina order by disciplina, nome ");
            connection.Open();
            try
            {
                tab.Query(connection);
            }
            finally
            {
                connection.Close();
            }

            return tab;
        }

        public static QueryTable ConsultarDisciplinas(string aluno)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = " select '-1' DISCIPLINA, 'Todas' NOME " +
            "union select m.DISCIPLINA as disciplina, d.NOME as nome from LY_MATRICULA m  " +
            "inner join LY_DISCIPLINA d on (m.DISCIPLINA = d.disciplina) " +
            "where m.aluno = ? " +
            "and m.sit_matricula = 'Matriculado' " +
            "order by DISCIPLINA ";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, aluno);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public static string ConsultarTipoDisciplina(string disciplina)
        {
            string sql = "select campo_01 from ly_disciplina where DISCIPLINA = ? ";
            return ConsultarCampo(sql, disciplina);
        }

        public static string ConsultarTipoGrupo(string agrupamento)
        {
            string sql = "select tipo from ly_grupo_habilitacao where agrupamento = ? ";
            return ConsultarCampo(sql, agrupamento);
        }

        public bool ExisteCarenciaPor(string turma, string disciplina, int ano, int periodo, int bimestre)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            object obj = new Object();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT TOP 1
                                    1
                            FROM    LY_AULA_DOCENTE AD ( NOLOCK )
                            WHERE   DISCIPLINA = @DISCIPLINA -- CHAVE DA TURMA
                                    AND TURMA = @TURMA -- CHAVE DA TURMA
                                    AND ANO = @ANO -- CHAVE DA TURMA
                                    AND SEMESTRE = @PERIODO -- CHAVE DA TURMA
                                    AND NUM_FUNC IN ( 115451, 115460 ) -- Matrículas de Carência('00000000','99999999')
                                    AND ( DATA_FIM > ( SELECT   DT_INICIO
                                                       FROM     LY_SUBPERIODO_LETIVO ( NOLOCK )
                                                       WHERE    ANO = AD.ANO
                                                                AND PERIODO = AD.SEMESTRE
                                                                AND SUBPERIODO = @BIMESTRE
                                                     ) -- Data de Início do Bimestre
                                          AND DATA_INICIO < ( SELECT    DT_FIM
                                                              FROM      LY_SUBPERIODO_LETIVO ( NOLOCK )
                                                              WHERE     ANO = AD.ANO
                                                                        AND PERIODO = AD.SEMESTRE
                                                                        AND SUBPERIODO = @BIMESTRE
                                                            )-- Data do Fim do Bimestre              
                                          
                                        )  "
                };

                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@BIMESTRE", bimestre);

                obj = ctx.GetReturnValue(contextQuery);

                if (obj == null)
                {
                    return false;
                }

                return true;
            }

            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public bool PossuiOutroTipoDisciplinaGrupoPor(string disciplina, string tipo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                            FROM    LY_GRUPO_HABILITACAO_DISC gd
                                    INNER JOIN ly_disciplina d ON gd.disciplina = d.disciplina
                                    INNER JOIN LY_GRUPO_HABILITACAO g ON gd.AGRUPAMENTO = g.AGRUPAMENTO
                            WHERE   d.DISCIPLINA = @DISCIPLINA
                                    AND g.TIPO <> @TIPO ";

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@TIPO", tipo);

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
            finally
            {
                ctx.Dispose();
            }

            //            string sql = @"select 1 from LY_GRUPO_HABILITACAO_DISC gd
            //                            inner join ly_disciplina d
            //                            on gd.disciplina = d.disciplina
            //                            inner join LY_GRUPO_HABILITACAO g
            //                            on gd.AGRUPAMENTO = g.AGRUPAMENTO
            //                            where d.DISCIPLINA = ?
            //                            and g.TIPO <> ?";

            //            int retorno = ExecutarFuncao(sql, disciplina, agrupamento);

            //            if (retorno >= 1)
            //                return true;
            //            else
            //                return false;
        }

        /// <summary>
        /// Verifica se existe conflito de datas entre a Nova Disciplina Bloqueada de GLP
        /// e as Disciplinas Bloqueadas de GLP já cadastradas.
        /// </summary>
        /// <param name="grupoDisciplinaNovo">Grupo de Disciplinas Novo</param>
        /// <param name="dataInicioNova">Data de Início Nova</param>
        /// <param name="dataFimNova">Data de Fim Nova</param>
        /// <returns></returns>
        public static bool ExisteDisciplinaBloqueadaGLP(string grupoDisciplinaNovo, DateTime dataInicioNova, DateTime dataFimNova)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            QueryTable disciplinasBloqueadasGLP = ConsultarDisciplinasBloqueadasGLP(connection, grupoDisciplinaNovo);
            foreach (SimpleRow disciplinaBloqueadaGLP in disciplinasBloqueadasGLP.Rows)
            {
                DateTime dataInicio = Convert.ToDateTime(Convert.ToDateTime(disciplinaBloqueadaGLP["DT_INICIO"]));
                DateTime dataFim = Convert.ToDateTime(disciplinaBloqueadaGLP["DT_FIM"]);
                if ((dataInicio > dataInicioNova && dataInicio < dataFimNova) ||
                    (dataFim > dataInicioNova && dataFim < dataFimNova) ||
                    (dataInicio <= dataInicioNova && dataFim >= dataFimNova))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Consulta Disciplinas Bloqueadas de GLP a partir do grupo de disciplinas a que pertence.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="grupoDisciplinas">Grupo de Disciplinas</param>
        /// <returns></returns>
        public static QueryTable ConsultarDisciplinasBloqueadasGLP(TConnection connection, string grupoDisciplinas)
        {
            if (String.IsNullOrEmpty(grupoDisciplinas))
                return null;
            string sql = @"select DT_INICIO, DT_FIM from Ly_evento_geral
						where TIPO_FILTRO='DisciplinasBloqueadas'
						and VALOR_FILTRO=?";
            QueryTable qt = new QueryTable(sql);
            qt.Query(connection, grupoDisciplinas);
            return qt;
        }

        /// <summary>
        /// Verifica se existe conflito de datas entre a Nova Disciplina Não Excluída de GLP
        /// e as Disciplinas Não Excluídas de GLP já cadastradas.
        /// </summary>
        /// <param name="grupoDisciplinaNovo">Grupo de Disciplinas Novo</param>
        /// <param name="dataInicioNova">Data de Início Nova</param>
        /// <param name="dataFimNova">Data de Fim Nova</param>
        /// <returns></returns>
        public static bool ExisteDisciplinaNaoExcluidaGLP(string grupoDisciplinaNovo, DateTime dataInicioNova, DateTime dataFimNova)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            QueryTable disciplinasNaoExcluidasGLP = ConsultarDisciplinasNaoExcluidasGLP(connection, grupoDisciplinaNovo);
            foreach (SimpleRow disciplinaNaoExcluidaGLP in disciplinasNaoExcluidasGLP.Rows)
            {
                DateTime dataInicio = Convert.ToDateTime(Convert.ToDateTime(disciplinaNaoExcluidaGLP["DT_INICIO"]));
                DateTime dataFim = Convert.ToDateTime(disciplinaNaoExcluidaGLP["DT_FIM"]);
                if ((dataInicio > dataInicioNova && dataInicio < dataFimNova) ||
                    (dataFim > dataInicioNova && dataFim < dataFimNova) ||
                    (dataInicio <= dataInicioNova && dataFim >= dataFimNova))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Consulta Disciplinas Não Excluídas de GLP a partir do grupo de disciplinas a que pertence.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="grupoDisciplinas">Grupo de Disciplinas</param>
        /// <returns></returns>
        public static QueryTable ConsultarDisciplinasNaoExcluidasGLP(TConnection connection, string grupoDisciplinas)
        {
            if (string.IsNullOrEmpty(grupoDisciplinas))
            {
                return null;
            }

            string sql = @"select DT_INICIO, DT_FIM from Ly_evento_geral
						where TIPO_FILTRO='DisciplinasNaoExcluidas'
						and VALOR_FILTRO=?";
            QueryTable qt = new QueryTable(sql);
            qt.Query(connection, grupoDisciplinas);
            return qt;
        }

        public static QueryTable ConsultarDisciplinas(string turma, decimal ano, decimal semestre)
        {
            string sql = @"select t.disciplina, d.tipo from ly_turma t inner join LY_DISCIPLINA d on d.DISCIPLINA = t.DISCIPLINA
                          where t.TURMA = ? and t.ANO = ? and t.SEMESTRE = ? ";
            return Consultar(sql, turma, ano, semestre);
        }

        public static QueryTable ConsultarDisciplinaAtivas(string grade_id)
        {
            return Consultar(@"
                SELECT  gt.disciplina, d.nome_compl
                FROM    ly_grade_turma gt (NOLOCK) INNER JOIN
		                ly_disciplina d (NOLOCK) ON d.disciplina = gt.disciplina
                WHERE   EXISTS(
			                SELECT	TOP 1 1 
			                FROM	ly_turma t (NOLOCK)
			                WHERE	t.sit_turma = 'Aberta' AND
					                t.disciplina = gt.disciplina AND
					                t.turma = gt.turma AND
					                t.ano = gt.ano AND
					                t.semestre = gt.semestre					
		                ) AND gt.grade_id = ?
                ORDER BY d.nome_compl ASC", grade_id);
        }

        public static DataTable ListarDisciplina(string ano, string curso, string serie)
        {
            var dataTable = new DataTable();

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                //NOME_COMPL
                contextQuery.Command = @"SELECT DISTINCT NOME_COMPL,d.disciplina ,d.disciplina + ' - ' + NOME_COMPL as NOME_DISCIPLINA   
                    FROM LY_GRADE G
                    INNER JOIN LY_CURRICULO C ON G.CURRICULO = C.CURRICULO AND G.CURSO = C.CURSO AND G.TURNO =C.TURNO 
                    inner join LY_DISCIPLINA D on D.DISCIPLINA  = g.DISCIPLINA 
                    WHERE ANO_INI = @ano
                    AND G.CURSO = @curso
                    AND SERIE_IDEAL = @serie
                    order by NOME_COMPL";
                contextQuery.Parameters.Add("@ano", ano);
                contextQuery.Parameters.Add("@curso", curso);
                contextQuery.Parameters.Add("@serie", serie);

                dataTable = ctx.GetDataTable(contextQuery);
            }

            return dataTable;
        }

        public static DataTable ListarDisciplinaNotas(string grade, string turma, string ano, string periodo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dataTable = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT gt.grade_id                   AS grade_id, 
                                            gt.disciplina                 AS disciplina, 
                                            d.nome                        AS nome, 
                                            gt.disciplina + '  ' + d.nome AS nome_disc 
                            FROM   ly_grade_turma gt 
                                   INNER JOIN ly_turma tu 
                                           ON gt.turma = tu.turma 
                                              AND gt.ano = tu.ano 
                                              AND gt.semestre = tu.semestre 
                                              AND gt.disciplina = tu.disciplina 
                                   INNER JOIN ly_disciplina d 
                                           ON d.disciplina = tu.disciplina 
                            WHERE  tu.sit_turma IN ( 'Aberta', 'Finalizada' ) 
                                   AND tu.ano = @ano 
                                   AND tu.semestre = @periodo 
                                   AND gt.grade_id = @grade 
                                   AND gt.turma = @turma 
                            ORDER  BY d.nome ";

                contextQuery.Parameters.Add("@grade", grade);
                contextQuery.Parameters.Add("@turma", turma);
                contextQuery.Parameters.Add("@ano", ano);
                contextQuery.Parameters.Add("@periodo", periodo);

                dataTable = contexto.GetDataTable(contextQuery);
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

            return dataTable;
        }
               
        public static object ListarDisciplinasObrigatorias(string turma, int ano, int periodo)
        {
            return Consultar(
                new ContextQuery(
                    @"SELECT  t.FACULDADE AS 'UNIDADE_ENS', t.TURMA, t.ANO, t.SEMESTRE AS 'PERIODO', d.DISCIPLINA, d.NOME AS 'NOME_DISCIPLINA'
                    FROM    dbo.LY_TURMA t
                            INNER JOIN dbo.LY_GRADE g ON t.CURRICULO = g.CURRICULO
                                                         AND t.CURSO = g.CURSO
                                                         AND t.DISCIPLINA = g.DISCIPLINA
                                                         AND t.TURNO = g.TURNO
                            INNER JOIN dbo.LY_DISCIPLINA d ON g.DISCIPLINA = d.DISCIPLINA
                    WHERE   t.TURMA = @TURMA
                            AND t.ANO = @ANO
                            AND t.SEMESTRE = @PERIODO
                            AND g.OBRIGATORIA = 'S'
                            AND ISNULL(T.ELETIVA,'N') = 'N' ",
                    new ContextQueryParameter("@TURMA", turma),
                    new ContextQueryParameter("@ANO", ano),
                    new ContextQueryParameter("@PERIODO", periodo)));
        }

        public static DataTable ConsultarPorDisciplina(string disciplina)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dadosDisciplinas = null;

            try
            {
                contextQuery.Command = @" SELECT TEM_NOTA, 
                                                   TEM_FREQ 
                                            FROM   LY_DISCIPLINA 
                                            WHERE  DISCIPLINA = @DISCIPLINA ";

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);

                dadosDisciplinas = ctx.GetDataTable(contextQuery);
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

            return dadosDisciplinas;
        }

        public List<DadosDisciplinaParaFechamento> ObtemDisciplinasParaFechamentoPor(int gradeId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            List<DadosDisciplinaParaFechamento> disciplinas = new List<DadosDisciplinaParaFechamento>();
            SqlDataReader reader = null;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = (@" SELECT  D.DISCIPLINA ,
                            ISNULL(D.HORAS_AULA, 0) HORAS_AULA ,
                            ISNULL(D.HORAS_LAB, 0) HORAS_LAB ,
                            ISNULL(D.HORAS_ATIV, 0) HORAS_ATIV ,
                            ISNULL(D.HORAS_ESTAGIO, 0) HORAS_ESTAGIO ,
                            ISNULL(D.CREDITOS, 0) CREDITOS
                    FROM    LY_GRADE_TURMA GT WITH ( NOLOCK )
                            INNER JOIN LY_DISCIPLINA D WITH ( NOLOCK ) ON D.DISCIPLINA = GT.DISCIPLINA
                    WHERE   GRADE_ID = @GRADE_ID ")
                };

                contextQuery.Parameters.Add("@GRADE_ID", gradeId);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    DadosDisciplinaParaFechamento d = new DadosDisciplinaParaFechamento
                    {
                        Disciplina = Convert.ToString(reader["DISCIPLINA"]),
                        Creditos = Convert.ToDecimal(reader["HORAS_AULA"]),
                        HorasAtiv = Convert.ToDecimal(reader["HORAS_LAB"]),
                        HorasAula = Convert.ToDecimal(reader["HORAS_ATIV"]),
                        HorasEstagio = Convert.ToDecimal(reader["HORAS_ESTAGIO"]),
                        HorasLab = Convert.ToDecimal(reader["CREDITOS"])
                    };

                    disciplinas.Add(d);
                }

                return disciplinas;
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

        public DataTable ObtemDisciplinasGrade(decimal gradeId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable disciplinas = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                            D.DISCIPLINA ,
                            D.NOME ,
                            '' AS SIT_MATGRADE ,
                            T.GRADE_ID
                    FROM    LY_GRADE_TURMA T
                            INNER JOIN LY_DISCIPLINA D ON T.DISCIPLINA = D.DISCIPLINA
                    WHERE   T.GRADE_ID = @GRADE_ID ";

                contextQuery.Parameters.Add("@GRADE_ID", gradeId);

                disciplinas = ctx.GetDataTable(contextQuery);

                return disciplinas;
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
                if (ctx != null)
                    ctx.Dispose();
            }
        }

        public DadosDisciplinaParaFechamento ObtemDisciplinaParaFechamento(string disciplina)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            DadosDisciplinaParaFechamento dadosDisciplina = new DadosDisciplinaParaFechamento();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = (@" SELECT  D.DISCIPLINA ,
                                    ISNULL(D.HORAS_AULA, 0) HORAS_AULA ,
                                    ISNULL(D.HORAS_LAB, 0) HORAS_LAB ,
                                    ISNULL(D.HORAS_ATIV, 0) HORAS_ATIV ,
                                    ISNULL(D.HORAS_ESTAGIO, 0) HORAS_ESTAGIO ,
                                    ISNULL(D.CREDITOS, 0) CREDITOS
                            FROM    LY_DISCIPLINA D
                            WHERE   DISCIPLINA = @DISCIPLINA ")
                };

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosDisciplina.Disciplina = Convert.ToString(reader["DISCIPLINA"]);
                    dadosDisciplina.Creditos = Convert.ToDecimal(reader["HORAS_AULA"]);
                    dadosDisciplina.HorasAtiv = Convert.ToDecimal(reader["HORAS_LAB"]);
                    dadosDisciplina.HorasAula = Convert.ToDecimal(reader["HORAS_ATIV"]);
                    dadosDisciplina.HorasEstagio = Convert.ToDecimal(reader["HORAS_ESTAGIO"]);
                    dadosDisciplina.HorasLab = Convert.ToDecimal(reader["CREDITOS"]);
                }

                return dadosDisciplina;
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

        public static int ConsultarNotaMaximaPor(string disciplina)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(@"SELECT [NOTA_MAX]
                                                        FROM [LYCEUM].[dbo].[LY_DISCIPLINA]
                                                       WHERE DISCIPLINA = @DISCIPLINA");

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);

                return ctx.GetReturnValue<int?>(contextQuery) ?? 0;
            }
        }

        public List<RN.DTOs.DisciplinaTurmaTransferenciaAluno> ObtemDisciplinaTransferenciaAlunoPor(DataContext contexto, string turma, int ano, int periodo, string aluno)
        {
            List<RN.DTOs.DisciplinaTurmaTransferenciaAluno> listaDisciplinaTurmaTransferenciaAluno = (List<RN.DTOs.DisciplinaTurmaTransferenciaAluno>)contexto.TryToBindEntities<RN.DTOs.DisciplinaTurmaTransferenciaAluno>(
                new ContextQuery(
                    @"SELECT DISTINCT TU.DISCIPLINA,
                              D.AULAS_SEMANAIS as AULAS_DADAS,
                              D.TIPO,
                              D.TEM_NOTA as TEM_NOTA,
                              M.ALUNO
                      FROM    LY_TURMA TU
                              INNER JOIN LY_DISCIPLINA D ON TU.DISCIPLINA = D.DISCIPLINA
                              LEFT  JOIN LY_MATRICULA M ON M.ALUNO        = @ALUNO
                                                       AND TU.ANO         = M.ANO
                                                       AND TU.SEMESTRE    = M.SEMESTRE
                                                       AND TU.DISCIPLINA  = M.DISCIPLINA
                                                       AND TU.TURMA       = M.TURMA
                      WHERE   TU.TURMA = @TURMA
                              AND TU.ANO = @ANO
                              AND TU.SEMESTRE = @PERIODO",
                new ContextQueryParameter("@ALUNO", aluno),
                new ContextQueryParameter("@TURMA", turma),
                new ContextQueryParameter("@ANO", ano),
                new ContextQueryParameter("@PERIODO", periodo)));

            return listaDisciplinaTurmaTransferenciaAluno;
        }

        public DataTable ObtemDisciplinasTurmaPor(int ano, int semestre, string turma)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable disciplinas = null;

            try
            {
                contextQuery.Command = @"SELECT DISTINCT D.DISCIPLINA, 
                                                        D.NOME, 
                                                        '' AS sit_matgrade, 
                                                        GT.GRADE_ID, 
                                                        T.CURSO 
                                        FROM   LY_TURMA T (NOLOCK) 
                                               INNER JOIN LY_GRADE_TURMA GT (NOLOCK) 
                                                       ON GT.DISCIPLINA = T.DISCIPLINA 
                                                          AND GT.TURMA = T.TURMA 
                                                          AND GT.ANO = T.ANO 
                                                          AND GT.SEMESTRE = T.SEMESTRE 
                                               INNER JOIN LY_DISCIPLINA D (NOLOCK) 
                                                       ON T.DISCIPLINA = D.DISCIPLINA 
											   INNER JOIN LY_CURSO C ( NOLOCK ) ON C.CURSO = T.CURSO
											   INNER JOIN LY_SERIE S ON T.CURRICULO = S.CURRICULO
																				AND T.TURNO = S.TURNO
																				AND T.CURSO = S.CURSO  
																				AND T.SERIE = S.SERIE  
                                        WHERE  T.ANO = @ANO 
                                               AND T.SEMESTRE = @SEMESTRE 
                                               AND T.TURMA = @TURMA
											   AND (S.OFERTAELETIVA = 'N' OR (S.OFERTAELETIVA = 'S' AND ISNULL(T.ELETIVA, 'N') = 'N'))   ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@TURMA", turma);

                disciplinas = ctx.GetDataTable(contextQuery);
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return disciplinas;
        }

        public DataTable ObtemDisciplinaNotasHistoricoPor(string grade, string turma, string ano, string periodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable disciplinas = null;

            try
            {
                contextQuery.Command = @"SELECT DISTINCT GT.GRADE_ID                   AS grade_id,
                                                            GT.DISCIPLINA                 AS disciplina,
                                                            D.NOME                        AS nome,
                                                            GT.DISCIPLINA + '  ' + D.NOME AS nome_disc
                                            FROM   LY_GRADE_TURMA GT
                                                   INNER JOIN LY_TURMA TU
                                                           ON GT.TURMA = TU.TURMA
                                                              AND GT.ANO = TU.ANO
                                                              AND GT.SEMESTRE = TU.SEMESTRE
                                                              AND GT.DISCIPLINA = TU.DISCIPLINA
                                                   INNER JOIN LY_DISCIPLINA D
                                                           ON D.DISCIPLINA = TU.DISCIPLINA       
                                            WHERE  TU.SIT_TURMA IN ( 'Aberta', 'Finalizada' )
                                                   AND TU.ANO = @ANO
                                                   AND TU.SEMESTRE = @PERIODO
                                                   AND GT.GRADE_ID = @GRADE
                                                   AND GT.TURMA = @TURMA
                                            ORDER  BY D.NOME  ";


                contextQuery.Parameters.Add("@GRADE", grade);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                disciplinas = ctx.GetDataTable(contextQuery);
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return disciplinas;
        }

        public static QueryTable ListaDisciplinaMultipla(string turma, string ano, string semestre)
        {
            QueryTable qt = null;

            if (!string.IsNullOrEmpty(turma)
                && !string.IsNullOrEmpty(ano)
                && !string.IsNullOrEmpty(semestre))
            {
                TConnection connection = Config.CreateConnection();
                connection.Open();

                string sql = @"SELECT D1.DISCIPLINA + '|' + D.DISCIPLINA DISCIPLINA, D1.NOME + ' (' + D.NOME + ')' NOME 
                    FROM LY_TURMA TURMA
                    INNER JOIN LY_DISCIPLINA D   ON D.DISCIPLINA = TURMA.DISCIPLINA_MULTIPLA
                    INNER JOIN LY_DISCIPLINA D1  ON D1.DISCIPLINA = TURMA.DISCIPLINA
                    WHERE TURMA LIKE ?
                        AND TURMA.ANO = ? 
                        AND TURMA.SEMESTRE = ?
                        AND TURMA.DISCIPLINA_MULTIPLA IS NOT NULL 
                  ";
                try
                {
                    qt = new QueryTable(sql);

                    qt.Query(connection, turma, ano, semestre);
                }
                finally
                {
                    connection.Close();
                }
            }

            return qt;
        }

        public DataTable ListaDisciplinasTurmaComFrequenciaPor(int ano, int semestre, string turma)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable disciplinas = null;

            try
            {
                contextQuery.Command = @"SELECT DISTINCT T.DISCIPLINA, 
                                                        D.NOME AS NOME_DISC,
                                                        (T.DISCIPLINA + ' - ' + D.NOME) AS NOME                                                       
                                        FROM   LY_TURMA T (NOLOCK)                                           
                                                INNER JOIN LY_DISCIPLINA D (NOLOCK) 
                                                        ON ISNULL(t.DISCIPLINA_MULTIPLA, t.DISCIPLINA) = D.DISCIPLINA  
                                        WHERE  T.ANO = @ANO 
                                               AND T.SEMESTRE = @SEMESTRE 
                                               AND T.TURMA = @TURMA
											   AND ISNULL(D.TEM_FREQ,'N') = 'S'   ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@TURMA", turma);

                disciplinas = ctx.GetDataTable(contextQuery);
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return disciplinas;
        }
    }
}