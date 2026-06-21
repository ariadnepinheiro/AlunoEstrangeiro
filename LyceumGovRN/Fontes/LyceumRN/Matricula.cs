using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Entidades;
using System;
using System.Data;
using System.Text;
using Seeduc.Infra.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN
{
    using Data;
    using Library;
    using CR;
    using Techne.Lyceum.RN.DTOs.Agenda;

    public class Matricula : RNBase
    {
        public const string Matriculado = "Matriculado";
        public const string Cancelado = "Cancelado";

        /// <summary>
        /// Consulta dados da matrícula do aluno caso exista, senão dados do aluno.
        /// </summary>
        /// <param name="aluno">matrícula do aluno</param>
        /// <returns>unidade, curso, turno, unidade de ensino, unidade física, coordenadoria do aluno </returns>
        public static QueryTable ConsultaSerieTurma(string aluno)
        {
            string sql =
                @"SELECT top 1 a.aluno, a.unidade_ensino, c.nome AS nomecurso, c.curso, t.descricao AS nometurno, t.turno, gs.curriculo, a.serie, s.descricao AS nomeserie, 
                           ue.nome_comp AS nomefaculdade, n.descricao AS nomenucleo, ue.nucleo, a.unidade_fisica, uf.nome_comp AS nomeUnidadeFisica 
                           FROM Ly_Matgrade mg 
                           JOIN Ly_Grade_turma gt ON mg.grade_id = gt.grade_id 
                           JOIN Ly_Grade_serie gs ON gs.grade_id = mg.grade_id AND gs.grade = gt.turma AND gs.grade = gt.turma 
                           JOIN Ly_Matricula m ON m.aluno = mg.aluno AND gt.disciplina = m.disciplina AND m.turma = gt.turma AND m.ano = gt.ano AND m.semestre = gt.semestre 
                           JOIN Ly_Aluno a ON mg.aluno = a.aluno 
                           INNER JOIN Ly_Curso c ON gs.curso = c.curso 
                           LEFT JOIN Ly_unidade_fisica uf ON a.unidade_fisica = uf.unidade_fis 
                           INNER JOIN Ly_unidade_ensino ue ON a.unidade_ensino = ue.unidade_ens 
                           INNER JOIN Ly_turno t ON gs.turno = t.turno 
                           INNER JOIN Ly_serie s ON gs.curso = s.curso AND gs.curriculo = s.curriculo AND t.turno = s.turno AND a.serie = s.serie 
                           LEFT JOIN Ly_nucleo n ON n.nucleo = ue.nucleo 
                           WHERE mg.sit_matgrade = 'Matriculado' 
                           AND mg.aluno = ? ";

            var qt = Consultar(sql, aluno);

            if (!(qt.Rows.Count > 0))
            {
                sql =
                    @"SELECT a.aluno, a.unidade_ensino, c.nome as nomecurso, c.curso, t.descricao as nometurno, t.turno, a.curriculo, a.serie, s.descricao as nomeserie, 
                               ue.nome_comp as nomefaculdade, n.descricao as nomenucleo, ue.nucleo, a.unidade_fisica, uf.nome_comp as nomeUnidadeFisica 
                        FROM Ly_Aluno a 
                        INNER JOIN Ly_Curso c ON a.curso = c.curso 
                        LEFT JOIN Ly_unidade_fisica uf ON a.unidade_fisica = uf.unidade_fis 
                        INNER JOIN Ly_unidade_ensino ue ON a.unidade_ensino = ue.unidade_ens 
                        INNER JOIN Ly_turno t ON a.turno = t.turno 
                        INNER JOIN Ly_serie s ON c.curso = s.curso AND a.curriculo = s.curriculo AND t.turno = s.turno AND a.serie = s.serie 
                        LEFT JOIN Ly_nucleo n ON n.nucleo = ue.nucleo 
                        WHERE a.aluno = ?";

                qt = Consultar(sql, aluno);
            }

            return qt;
        }

        /// <summary>
        /// Consulta matrículas com status "Matriculado" do aluno
        /// </summary>
        /// <param name="aluno">aluno</param>
        /// <returns>ano, período e turma do aluno</returns>
        public static QueryTable ConsultaMatricula(string aluno)
        {
            string sql = @"Select top 1 gt.ano, gt.semestre as periodo, gt.turma from ly_matgrade mg " +
                         "join ly_grade_turma gt on mg.grade_id = gt.grade_id " +
                         "join ly_grade_serie gs on gs.GRADE_ID = mg.GRADE_ID and gs.GRADE = gt.TURMA and gs.GRADE = gt.TURMA " +
                         "join ly_matricula m on m.ALUNO = mg.ALUNO and gt.disciplina = m.disciplina and m.turma = gt.turma and m.ANO = gt.ANO and m.SEMESTRE = gt.SEMESTRE " +
                         "where mg.sit_matgrade = 'Matriculado' " +
                         "and mg.aluno = ?";
            return Consultar(sql, aluno);
        }     

        /// <summary>
        /// Consulta matrículas com status "Matriculado" do aluno.
        /// </summary>
        /// <param name="aluno">aluno</param>
        /// <returns>ano, período e turma do aluno</returns>
        public static QueryTable ConsultaMatriculaGrade(string aluno)
        {
            string sql =
                "select pl.ano, convert(varchar,pl.periodo) + ' ' + isnull(pl.ID_REDUZIDA,'') as id_reduzida, pl.periodo, gt.turma from ly_matgrade mg " +
                "join ly_grade_turma gt on mg.grade_id = gt.grade_id " +
                "join ly_grade_serie gs on gs.GRADE_ID = mg.GRADE_ID and gs.GRADE = gt.TURMA and gs.GRADE = gt.TURMA " +
                "join ly_matricula m on m.ALUNO = mg.ALUNO and gt.disciplina = m.disciplina and m.turma = gt.turma and m.ANO = gt.ANO and m.SEMESTRE = gt.SEMESTRE " +
                "join Ly_periodo_letivo pl on pl.ANO = gs.ANO and pl.PERIODO = gs.SEMESTRE " +
                "where mg.sit_matgrade = 'Matriculado' " +
                "and mg.aluno = ?";
            return Consultar(sql, aluno);
        }

        /// <summary>
        /// Verifica se existem matrículas com status "Matriculado" para o aluno.
        /// </summary>
        /// <param name="aluno">matrícula do aluno</param>
        /// <returns>true se existe, false se não existe</returns>
        public static bool ExisteMatriculaAtiva(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            if (string.IsNullOrEmpty(aluno))
            {
                return false;
            }

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                            FROM    LY_MATRICULA (NOLOCK)
                            WHERE   SIT_MATRICULA = @SIT_MATRICULA
                                    AND ALUNO = @ALUNO ";

                contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
                contextQuery.Parameters.Add("@ALUNO", aluno);

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

        /// <summary>
        /// Consulta turmas.
        /// </summary>
        /// <param name="ano">ano</param>
        /// <param name="periodo">período</param>
        /// <param name="turno">turno</param>
        /// <param name="curso">curso</param>
        /// <param name="curriculo">currículo</param>
        /// <param name="serie">série</param>
        /// <returns>turmas</returns>
        public static QueryTable ConsultarTurma(string ano, string periodo, string turno, string curso, string curriculo,
                                                string serie)
        {
            string sql =
                "select distinct turma from ly_grade_serie s inner join ly_grade_turma t on s.grade_id = t.grade_id and t.turma = s.grade " +
                "WHERE s.ano = ? " +
                "AND s.semestre = ? " +
                "AND s.turno = ? " +
                "AND s.curso = ? " +
                "AND s.curriculo = ? " +
                "AND s.serie = ? ";
            return Consultar(sql, ano, periodo, turno, curso, curriculo, serie);
        }

        /// <summary>
        /// Consulta turmas.
        /// </summary>
        /// <param name="ano">ano</param>
        /// <param name="periodo">período</param>
        /// <param name="unidadeEns">unidade de ensino</param>
        /// <returns>turmas</returns>
        public static QueryTable ConsultarTurma(string ano, string periodo, string unidadeEns)
        {
            string sql =
                "select distinct turma from ly_grade_serie s inner join ly_grade_turma t on s.grade_id = t.grade_id and t.turma = s.grade " +
                "WHERE s.ano = ? " +
                "AND s.semestre = ? " +
                "AND s.UNIDADE_RESPONSAVEL = ?";
            return Consultar(sql, ano, periodo, unidadeEns);
        }

        /// <summary>
        /// Consulta disciplinas de uma grade.
        /// </summary>
        /// <param name="ano">ano</param>
        /// <param name="periodo">período</param>
        /// <param name="turma">turma</param>
        /// <returns>disciplinas e descrições</returns>
        public static QueryTable ConsultaDisciplinaGrade(string ano, string periodo, string turma)
        {
            if (!string.IsNullOrEmpty(ano) && !string.IsNullOrEmpty(periodo) && !string.IsNullOrEmpty(turma))
            {
                string sql =
                    @"
                    SELECT 
	                    DISTINCT d.disciplina, d.nome, '' as sit_matgrade, gt.grade_id , t.CURSO
                    FROM 
                        ly_turma t (NOLOCK) INNER JOIN 
                        ly_grade_turma gt (NOLOCK) ON 
                            gt.disciplina = t.disciplina AND
                            gt.turma = t.turma AND 
                            gt.ano = t.ano AND 
                            gt.semestre = t.semestre INNER JOIN 
                        ly_disciplina d (NOLOCK) ON 
                            t.disciplina = d.disciplina 
                    WHERE t.ano = ? AND t.semestre = ? AND t.turma = ?";
                return Consultar(sql, ano, periodo, turma);
            }
            return null;
        }

        public static QueryTable ConsultaDadosTurmaEducacaoEspecial(string ano, string periodo, string turma, List<Atendimento> disciplinasAtendimento)
        {
            if (!string.IsNullOrEmpty(ano) && !string.IsNullOrEmpty(periodo) && !string.IsNullOrEmpty(turma))
            {
                string disciplinasTurma = string.Empty;

                foreach (Atendimento disciplina in disciplinasAtendimento)
                {
                    if (!string.IsNullOrEmpty(disciplinasTurma))
                    {
                        disciplinasTurma = string.Format("{0}, ", disciplinasTurma);
                    }

                    disciplinasTurma = string.Format("{0}'{1}'", disciplinasTurma, disciplina.Disciplina);
                }

                string sql =
                    @"
                    SELECT 
	                    DISTINCT d.disciplina, d.nome, '' as sit_matgrade, gt.grade_id , t.CURSO
                    FROM 
                        ly_turma t (NOLOCK) INNER JOIN 
                        ly_grade_turma gt (NOLOCK) ON 
                            gt.disciplina = t.disciplina AND
                            gt.turma = t.turma AND 
                            gt.ano = t.ano AND 
                            gt.semestre = t.semestre INNER JOIN 
                        ly_disciplina d (NOLOCK) ON 
                            t.disciplina = d.disciplina 
                    WHERE t.ano = ? AND t.semestre = ? AND t.turma = ? and t.disciplina in (" + disciplinasTurma + ") ";
                return Consultar(sql, ano, periodo, turma);
            }
            return null;
        }

        public static QueryTable ConsultaDisciplinaGradePorTurmasDiferenteDe(string aluno, string ano, string periodo, string turmaExcessao)
        {
            if (!string.IsNullOrEmpty(ano) && !string.IsNullOrEmpty(periodo) && !string.IsNullOrEmpty(turmaExcessao))
            {
                string sql =
                    @"
                     SELECT 
	                DISTINCT
                    d.disciplina ,
                    d.nome ,
                    '' AS sit_matgrade ,
                    gt.grade_id ,
                    t.CURSO,
                    t.TURMA
             FROM   ly_turma t ( NOLOCK )
                    INNER JOIN ly_grade_turma gt ( NOLOCK ) ON gt.disciplina = t.disciplina
                                                               AND gt.turma = t.turma
                                                               AND gt.ano = t.ano
                                                               AND gt.semestre = t.semestre
                    INNER JOIN ly_disciplina d ( NOLOCK ) ON t.disciplina = d.disciplina
             WHERE  t.ano = ?
                    AND t.semestre = ?
                    AND t.turma IN ( SELECT DISTINCT
                                            TURMA
                                     FROM   DBO.LY_MATRICULA
                                     WHERE  ALUNO = ?
                                            AND SIT_MATRICULA = 'MATRICULADO'
                                            AND ANO = ?
                                            AND SEMESTRE = ?
                                            AND ISNULL(DEPENDENCIA, 'N') = 'N'
                                            AND TURMA <> ? ) ";
                return Consultar(sql, ano, periodo, aluno, ano, periodo, turmaExcessao);
            }
            return null;
        }

        public static QueryTable ConsultaDisciplinaGradePorTurmas(string aluno, string ano, string periodo)
        {
            if (!string.IsNullOrEmpty(ano) && !string.IsNullOrEmpty(periodo))
            {
                string sql =
                    @" SELECT DISTINCT
                    d.disciplina ,
                    d.nome ,
                    '' AS sit_matgrade ,
                    gt.grade_id ,
                    t.CURSO,
                    t.TURMA
             FROM   ly_turma t ( NOLOCK )
                    INNER JOIN ly_grade_turma gt ( NOLOCK ) ON gt.disciplina = t.disciplina
                                                               AND gt.turma = t.turma
                                                               AND gt.ano = t.ano
                                                               AND gt.semestre = t.semestre
                    INNER JOIN ly_disciplina d ( NOLOCK ) ON t.disciplina = d.disciplina
             WHERE  t.ano = ?
                    AND t.semestre = ?
                    AND t.turma IN ( SELECT DISTINCT
                                            TURMA
                                     FROM   DBO.LY_MATRICULA
                                     WHERE  ALUNO = ?
                                            AND SIT_MATRICULA = 'MATRICULADO'
                                            AND ANO = ?
                                            AND SEMESTRE = ?
                                            AND ISNULL(DEPENDENCIA, 'N') = 'N')
                                            AND ISNULL(MAIS_EDUCACAO, 'N') NOT IN ('L','O') ";
                return Consultar(sql, ano, periodo, aluno, ano, periodo);
            }
            return null;
        }

        /// <summary>
        /// Consulta disciplinas do aluno.
        /// </summary>
        /// <param name="ano">ano</param>
        /// <param name="periodo">período</param>
        /// <param name="turma">turma</param>
        /// <param name="turno">turno</param>
        /// <param name="curso">curso</param>
        /// <param name="curriculo">currículo</param>
        /// <param name="aluno">aluno</param>
        /// <param name="serie">série</param>
        /// <returns>disciplinas</returns>
        public static QueryTable ConsultaDisciplinaGrade(string ano, string periodo, string turma, string turno,
                                                         string curso, string curriculo, string aluno, string serie)
        {
            if (!string.IsNullOrEmpty(ano) && !string.IsNullOrEmpty(periodo) && !string.IsNullOrEmpty(turma) &&
                !string.IsNullOrEmpty(turno) && !string.IsNullOrEmpty(curriculo) && !string.IsNullOrEmpty(serie) &&
                !string.IsNullOrEmpty(aluno))
            {
                string sql =
                    "SELECT distinct d.disciplina, d.nome, m.sit_matgrade, t.grade_id, m.dt_ultalt from ly_grade_serie s inner join ly_grade_turma t on s.grade_id = t.grade_id and t.turma = s.grade " +
                    "inner join ly_disciplina d on t.disciplina = d.disciplina join ly_matgrade m on s.grade_id = m.grade_id " +
                    "inner join ly_matricula ma on ma.ALUNO = m.ALUNO and t.disciplina = ma.disciplina and ma.turma = t.turma and ma.ANO = t.ANO and ma.SEMESTRE = t.SEMESTRE " +
                    "WHERE s.ano = ? " +
                    "AND s.semestre = ? " +
                    "AND t.turma = ? " +
                    "AND s.turno = ? " +
                    "AND s.curso = ? " +
                    "AND s.curriculo = ? " +
                    "AND m.aluno = ? " +
                    "AND m.sit_matgrade = 'Matriculado' " +
                    "AND s.serie = ? ";
                return Consultar(sql, ano, periodo, turma, turno, curso, curriculo, aluno, serie);
            }
            return null;
        }

        public static QueryTable ConsultaMatriculasRegulares(string aluno, decimal ano, decimal semestre)
        {
            string sql =
                "select distinct m.disciplina, (m.disciplina + ' - ' + d.nome) nome, m.turma, m.ano, m.semestre, m.sit_matricula " +
                "from ly_matgrade mg " +
                "join ly_grade_turma gt " +
                "on mg.grade_id = gt.grade_id " +
                "join ly_grade_serie gs on gs.GRADE_ID = mg.GRADE_ID and gs.GRADE = gt.TURMA " +
                "join ly_matricula m on m.ALUNO = mg.ALUNO and gt.disciplina = m.disciplina and gt.turma = m.turma and gt.ano = m.ano and gt.semestre = m.semestre " +
                "join ly_disciplina d on m.disciplina = d.disciplina " +
                "where mg.sit_matgrade = 'Matriculado' " +
                "and mg.aluno = ? " +
                "and gt.ano = ? " +
                "and gt.semestre = ? ";
            return Consultar(sql, aluno, ano, semestre);
        }

        private static bool ExisteMatricula(string aluno, string disciplina, string turma, decimal ano, decimal periodo)
        {
            var retorno = ExecutarFuncao<int>(
                new ContextQuery(
                    @"SELECT COUNT(*)
                    FROM    ly_matricula
                    WHERE   ALUNO = @ALUNO
                            AND TURMA = @TURMA
                            AND ANO = @ANO
                            AND SEMESTRE = @PERIODO
                            AND DISCIPLINA = @DISCIPLINA",
                    new ContextQueryParameter("@ALUNO", aluno),
                    new ContextQueryParameter("@DISCIPLINA", disciplina),
                    new ContextQueryParameter("@TURMA", turma),
                    new ContextQueryParameter("@ANO", ano),
                    new ContextQueryParameter("@PERIODO", periodo)));

            return retorno > 0;
        }

        public void IncluiMatriculaFechamento(DataContext ctx, LyMatricula matricula, LyMatGrade matGrade, int tipoAprovacao, string matriculaResponsavel, string periodoOrigem)
        {
            LyTurma lyTurma = new LyTurma();
            int vagasLiberadas = 0;
            int vagasUtilizadas = 0;
            DataTable conf = null;
            DataTable confReprovacao = null;
            RN.ControleVaga rnControleVaga = new ControleVaga();
            ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            Disciplina rnDisciplina = new Disciplina();
            DataTable listaDisciplinas = new DataTable();
            Matgrade rnMatgrade = new Matgrade();
            TceConfirmacaoMatricula confirmacaoMatricula = new TceConfirmacaoMatricula();
            Aluno rnAluno = new Aluno();
            RN.Aluno.DadosAluno dadosAluno = new Aluno.DadosAluno();

            try
            {
                dadosAluno = rnAluno.ObtemDadosAluno(matricula.Aluno);

                //aluno = Aluno.Carregar(matricula.Aluno);

                if (dadosAluno.SitAluno.ToUpper() != "ATIVO")
                {
                    throw new Exception(String.Format("ERRO_VALIDACAO:O aluno: {0} - {1} não pode ser enturmado pois não se encontra ativo.", dadosAluno.Aluno, dadosAluno.Nome_compl));
                }
                //CARREGAR DADOS DE ACORDO COM A TURMA:
                lyTurma = RN.Turma.Carregar(Convert.ToInt32(matricula.Ano), Convert.ToInt32(matricula.Semestre), matricula.Turma);

                //se tipo aprovação é igual a reprovado (1)
                if (tipoAprovacao == 1)
                {
                    confReprovacao = ConfirmacaoMatricula.VerificaConfirmacaoMatriculaAluno(matricula.Aluno,
                                                                           Convert.ToInt32(matricula.Ano),
                                                                           Convert.ToInt32(matricula.Semestre),
                                                                           Convert.ToInt32(lyTurma.Serie),
                                                                           lyTurma.Turno,
                                                                           lyTurma.Curso,
                                                                           lyTurma.Faculdade);
                    if (confReprovacao.Rows.Count == 0)
                    {
                        //Verificar se tem vaga no curso / serie / turno / ano / semestre
                        vagasLiberadas = rnControleVaga.ObtemVagasLiberadasTotalPor(ctx,
                            lyTurma.Faculdade,
                            Convert.ToInt32(lyTurma.Ano),
                            Convert.ToInt32(lyTurma.Semestre),
                            Convert.ToInt32(lyTurma.Serie),
                            lyTurma.Curso,
                            lyTurma.Turno);

                        vagasUtilizadas = rnControleVaga.ObtemVagasUtilizadasTotalPor(ctx,
                            lyTurma.Faculdade,
                            Convert.ToInt32(lyTurma.Ano),
                            Convert.ToInt32(lyTurma.Semestre),
                            Convert.ToInt32(lyTurma.Serie),
                            lyTurma.Curso,
                            lyTurma.Turno);

                        if (vagasLiberadas <= vagasUtilizadas)
                        {
                            throw new Exception("ERRO_VALIDACAO:Não existem vagas de continuidade disponíveis para a serie ou modalidade ou turno pretendidos.");
                        }

                        //VERIFICA SE EXISTE CONFIRMAÇÃO DE MATRICULA PARA O ALUNO ANO E OS POSSIVEIS PERIODOS
                        conf = rnConfirmacaoMatricula.ListaPossiveisConfirmacaoMatriculaPor(matricula.Aluno, Convert.ToInt32(matricula.Ano), Convert.ToInt32(periodoOrigem));

                        if (conf.Rows.Count > 0)
                        {
                            //Verifica se já existe confirmação (pendente ou confirmada) com os mesmos dados
                            confirmacaoMatricula = rnConfirmacaoMatricula.ObtemConfirmacaoPendenteOuAtivaPor(matricula.Aluno,
                                Convert.ToInt32(matricula.Ano),
                                Convert.ToInt32(matricula.Semestre),
                                Convert.ToInt32(lyTurma.Serie),
                                lyTurma.Turno,
                                lyTurma.Curso,
                                lyTurma.Faculdade
                            );

                            //Alterada para  "Não Confirmado" o status das confirmações existentes
                            rnConfirmacaoMatricula.CancelaPorReprovacaoPossiveisConfirmacaoMatriculaPor(ctx,
                                matricula.Aluno,
                                Convert.ToInt32(matricula.Ano),
                                Convert.ToInt32(matricula.Semestre),
                                matriculaResponsavel,
                                Convert.ToInt32(periodoOrigem));

                            if (confirmacaoMatricula.IdConfirmacaoMatricula == 0)
                            {
                                //Cria nova confirmação com dados da turma escolhida caso não exista uma com mesmos dados
                                TceConfirmacaoMatricula confirmacao = new TceConfirmacaoMatricula
                                {
                                    Censo = lyTurma.Faculdade,
                                    Ano = Convert.ToInt32(matricula.Ano),
                                    Periodo = Convert.ToInt32(matricula.Semestre),
                                    Aluno = matricula.Aluno,
                                    Curso = lyTurma.Curso,
                                    Turno = lyTurma.Turno,
                                    Curriculo = lyTurma.Curriculo,
                                    Serie = Convert.ToDecimal(lyTurma.Serie),
                                    Matricula = matriculaResponsavel,
                                    TipoVagaOcupada = "VC",
                                    DtSugerida = DateTime.Now,
                                    EnsinoReligioso = Convert.ToBoolean(conf.Rows[0]["ENSINO_RELIGIOSO"].ToString()),
                                    LinguaEstrangeiraFacultativa = Convert.ToBoolean(conf.Rows[0]["LINGUA_ESTRANGEIRA_FACULTATIVA"].ToString()),
                                    ProjetoAutonomia = false, //Como regra criar false
                                    DtCadastro = DateTime.Now,
                                    DtAlteracao = DateTime.Now,
                                    Observacao = "GERADA POR REPROVAÇÃO DO ALUNO",
                                    Status = RN.ConfirmacaoMatricula.Confirmado
                                };

                                //criar confirmação
                                rnConfirmacaoMatricula.InsereConfirmacaoMatricula(ctx, confirmacao);
                            }
                            else
                            {
                                //Caso já exita dar update
                                confirmacaoMatricula.Curriculo = lyTurma.Curriculo;
                                confirmacaoMatricula.Matricula = matriculaResponsavel;
                                confirmacaoMatricula.TipoVagaOcupada = "VC";
                                confirmacaoMatricula.ProjetoAutonomia = false; //Como regra criar false
                                confirmacaoMatricula.DtAlteracao = DateTime.Now;
                                confirmacaoMatricula.Observacao = "ATUALIZADA POR REPROVAÇÃO DO ALUNO";
                                confirmacaoMatricula.Status = RN.ConfirmacaoMatricula.Confirmado;

                                rnConfirmacaoMatricula.AlteraDados(ctx, confirmacaoMatricula);
                            }
                        }
                        else
                        {
                            throw new Exception(String.Format("ERRO_VALIDACAO:Enturmação cancelada. Não existe confirmação de matrícula para o aluno {0} - {1} no ano/período selecionado.", matricula.Aluno, dadosAluno.Nome_compl));
                        }
                    }
                }
                else //senão tipo aprovação é igual a aprovado (2)
                {
                    //VERIFICA E ATUALIZA A CONFIRMAÇÃO DE MATRICULA COM OS DADOS DA TURMA NOVA
                    conf = ConfirmacaoMatricula.VerificaConfirmacaoMatriculaAluno(matricula.Aluno,
                                                                                 Convert.ToInt32(matricula.Ano),
                                                                                 Convert.ToInt32(matricula.Semestre),
                                                                                 Convert.ToInt32(lyTurma.Serie),
                                                                                 lyTurma.Turno,
                                                                                 lyTurma.Curso,
                                                                                 lyTurma.Faculdade);
                    if (conf.Rows.Count == 0)
                    {
                        throw new Exception(String.Format("ERRO_VALIDACAO:Enturmação cancelada. Não existe confirmação de matrícula para o aluno {0} - {1} no ano / período / curso / série e turno selecionado.", matricula.Aluno, dadosAluno.Nome_compl));
                    }
                    else
                    {
                        if (lyTurma.Curriculo != conf.Rows[0]["CURRICULO"].ToString())
                        {
                            TceConfirmacaoMatricula confirmacao = new TceConfirmacaoMatricula
                            {
                                Censo = lyTurma.Faculdade,
                                Ano = Convert.ToInt32(matricula.Ano),
                                Periodo = Convert.ToInt32(matricula.Semestre),
                                Aluno = matricula.Aluno,
                                Curso = lyTurma.Curso,
                                Turno = lyTurma.Turno,
                                Curriculo = lyTurma.Curriculo,
                                Serie = Convert.ToDecimal(lyTurma.Serie),
                                Matricula = matriculaResponsavel,
                                DtAlteracao = DateTime.Now
                            };

                            rnConfirmacaoMatricula.AtualizaCurriculoConfirmacaoMatriculaPor(ctx, confirmacao);
                        }
                    }
                }

                //Lista Disciplinas na turma
                listaDisciplinas = rnDisciplina.ObtemDisciplinasGrade(matGrade.GradeId);

                foreach (DataRow dr in listaDisciplinas.Rows)
                {
                    matricula.Disciplina = dr["disciplina"].ToString();

                    //Verifica se a disciplina não é uma eletiva com enturmação separada
                    if (!rnDisciplina.EhDisciplinaGradeEletivaPor(matricula.Ano, matricula.Semestre, matricula.Turma, matricula.Disciplina))
                    {
                        if (this.PossuiMatricula(matricula.Aluno, matricula.Disciplina, matricula.Turma, matricula.Ano, matricula.Semestre))
                        {
                            this.Atualiza(ctx, matricula);
                        }
                        else
                        {
                            this.Insere(ctx, matricula);
                        }
                    }
                }

                if (!rnMatgrade.PossuiMatGrade(matGrade))
                {
                    rnMatgrade.InsereParaFechamento(ctx, matGrade);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ContextQuery Insere(LyMatricula lyMatricula)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT  INTO dbo.LY_MATRICULA
                                                  ( ALUNO ,
                                                    DISCIPLINA ,
                                                    TURMA ,
                                                    ANO ,
                                                    SEMESTRE ,
                                                    SIT_MATRICULA ,
                                                    DT_ULTALT ,
                                                    COBRANCA_SEP ,
                                                    DT_INSERCAO ,
                                                    DT_MATRICULA ,
                                                    DEPENDENCIA ,
                                                    MATRICULA ,
                                                    DT_CADASTRO ,
                                                    CONCOMITANTE ,
                                                    EDUC_ESPECIAL ,
                                                    MAIS_EDUCACAO
                                                  )
                                          VALUES  ( @ALUNO ,
                                                    @DISCIPLINA ,
                                                    @TURMA ,
                                                    @ANO ,
                                                    @SEMESTRE ,
                                                    @SIT_MATRICULA ,
                                                    GETDATE() ,
                                                    @COBRANCA_SEP ,
                                                    GETDATE() ,
                                                    GETDATE() ,
                                                    @DEPENDENCIA ,
                                                    @MATRICULA ,
                                                    GETDATE() ,
                                                    @CONCOMITANTE ,
                                                    @EDUC_ESPECIAL ,
                                                    @MAIS_EDUCACAO
                                                  ) ";

            contextQuery.Parameters.Add("@ALUNO", lyMatricula.Aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", lyMatricula.Disciplina);
            contextQuery.Parameters.Add("@TURMA", lyMatricula.Turma);
            contextQuery.Parameters.Add("@ANO", lyMatricula.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", lyMatricula.Semestre);
            contextQuery.Parameters.Add("@SIT_MATRICULA", lyMatricula.SitMatricula);
            contextQuery.Parameters.Add("@COBRANCA_SEP", lyMatricula.CobrancaSep);
            contextQuery.Parameters.Add("@DEPENDENCIA", lyMatricula.Dependencia);
            contextQuery.Parameters.Add("@MATRICULA", lyMatricula.Matricula);
            contextQuery.Parameters.Add("@CONCOMITANTE", lyMatricula.Concomitante);
            contextQuery.Parameters.Add("@EDUC_ESPECIAL", lyMatricula.EducEspecial);
            contextQuery.Parameters.Add("@MAIS_EDUCACAO", lyMatricula.MaisEducacao);

            return contextQuery;
        }

        public void InsereOuAtualiza(LyMatricula lyMatricula, List<ContextQuery> listaContextQuery)
        {
            ContextQuery contextQuery = new ContextQuery(
                    @"DECLARE @MATRICULAATIVA INT 

                    SELECT @MATRICULAATIVA = COUNT(*) 
                    FROM   LY_MATRICULA M 
                    WHERE  ALUNO = @ALUNO 
                           AND ANO = @ANO 
                           AND SEMESTRE = @SEMESTRE 
                           AND DISCIPLINA = @DISCIPLINA 
                           AND TURMA = @TURMA 

                    IF ( @MATRICULAATIVA = 0 ) 
                      BEGIN 
                          INSERT INTO DBO.LY_MATRICULA 
                                      (ALUNO, 
                                       DISCIPLINA, 
                                       TURMA, 
                                       ANO, 
                                       SEMESTRE, 
                                       SIT_MATRICULA, 
                                       DT_ULTALT, 
                                       COBRANCA_SEP, 
                                       DT_INSERCAO, 
                                       DT_MATRICULA, 
                                       DEPENDENCIA, 
                                       MATRICULA, 
                                       DT_CADASTRO, 
                                       CONCOMITANTE, 
                                       EDUC_ESPECIAL, 
                                       MAIS_EDUCACAO) 
                          VALUES      ( @ALUNO, 
                                        @DISCIPLINA, 
                                        @TURMA, 
                                        @ANO, 
                                        @SEMESTRE, 
                                        @SIT_MATRICULA, 
                                        GETDATE(), 
                                        @COBRANCA_SEP, 
                                        GETDATE(), 
                                        GETDATE(), 
                                        @DEPENDENCIA, 
                                        @MATRICULA, 
                                        GETDATE(), 
                                        @CONCOMITANTE, 
                                        @EDUC_ESPECIAL, 
                                        @MAIS_EDUCACAO ) 
                      END 
                    ELSE 
                      BEGIN 
                          UPDATE LY_MATRICULA 
                          SET    SIT_MATRICULA = 'Matriculado', 
                                 CONCOMITANTE = 'N',
                                 DEPENDENCIA = 'N',
                                 SERIE_REFERENCIA = NULL, 
                                 DISCIPLINA_REFERENCIA = NULL, 
                                 EDUC_ESPECIAL = 'N',
                                 MAIS_EDUCACAO = 'N',
                                 DT_ULTALT = GETDATE(), 
                                 DT_MATRICULA = GETDATE() 
                          WHERE  ALUNO = @ALUNO 
                                 AND DISCIPLINA = @DISCIPLINA 
                                 AND TURMA = @TURMA 
                                 AND ANO = @ANO 
                                 AND SEMESTRE = @SEMESTRE 
                      END");

            contextQuery.Parameters.Add("@ALUNO", lyMatricula.Aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", lyMatricula.Disciplina);
            contextQuery.Parameters.Add("@TURMA", lyMatricula.Turma);
            contextQuery.Parameters.Add("@ANO", lyMatricula.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", lyMatricula.Semestre);
            contextQuery.Parameters.Add("@SIT_MATRICULA", lyMatricula.SitMatricula);
            contextQuery.Parameters.Add("@COBRANCA_SEP", lyMatricula.CobrancaSep);
            contextQuery.Parameters.Add("@DEPENDENCIA", lyMatricula.Dependencia);
            contextQuery.Parameters.Add("@MATRICULA", lyMatricula.Matricula);
            contextQuery.Parameters.Add("@CONCOMITANTE", lyMatricula.Concomitante);
            contextQuery.Parameters.Add("@EDUC_ESPECIAL", lyMatricula.EducEspecial);
            contextQuery.Parameters.Add("@MAIS_EDUCACAO", lyMatricula.MaisEducacao);

            listaContextQuery.Add(contextQuery);
        }

        public void Insere(DataContext contexto, LyMatricula lyMatricula)
        {
            try
            {
                ContextQuery contextQuery = Insere(lyMatricula);
                contexto.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;

                if (contexto != null)
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

        private ContextQuery Atualiza(LyMatricula lyMatricula)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE  LY_MATRICULA
                                      SET     SIT_MATRICULA = 'Matriculado' ,
                                              DT_ULTALT = GETDATE() ,
                                              CONCOMITANTE = 'N',
                                              DEPENDENCIA = 'N',
                                              SERIE_REFERENCIA = NULL, 
                                              DISCIPLINA_REFERENCIA = NULL, 
                                              EDUC_ESPECIAL = 'N',
                                              MAIS_EDUCACAO = 'N',
                                              DT_MATRICULA = GETDATE()
                                      WHERE   ALUNO = @ALUNO
                                              AND DISCIPLINA = @DISCIPLINA
                                              AND TURMA = @TURMA
                                              AND ANO = @ANO
                                              AND SEMESTRE = @SEMESTRE ";

            contextQuery.Parameters.Add("@ALUNO", lyMatricula.Aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", lyMatricula.Disciplina);
            contextQuery.Parameters.Add("@TURMA", lyMatricula.Turma);
            contextQuery.Parameters.Add("@ANO", lyMatricula.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", lyMatricula.Semestre);

            return contextQuery;
        }

        public void Atualiza(DataContext contexto, LyMatricula lyMatricula)
        {
            try
            {
                ContextQuery contextQuery = Atualiza(lyMatricula);
                contexto.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;

                if (contexto != null)
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

        private static bool ValidaMatriculaAluno(Ly_matricula dtMatricula, TConnectionWritable connection)
        {
            QueryTable qtAluno = new QueryTable("select ano_ingresso, sem_ingresso from ly_aluno where aluno = ?");
            qtAluno.Query(connection, dtMatricula.Rows[0].Aluno);

            if (qtAluno.Rows.Count > 0 && qtAluno.Rows[0]["ano_ingresso"].ToString() != "" &&
                qtAluno.Rows[0]["sem_ingresso"].ToString() != "")
            {
                if (dtMatricula.Rows[0].Ano == Convert.ToDecimal(qtAluno.Rows[0]["ano_ingresso"]) &&
                    dtMatricula.Rows[0].Semestre == Convert.ToDecimal(qtAluno.Rows[0]["sem_ingresso"]))
                {
                    QueryTable qtContrato =
                        new QueryTable(
                            "select 1 from ly_contrato_aluno where ALUNO = ? and ANO = ? and PERIODO = ? and CONTRATO_ACEITO = 'S'");
                    qtContrato.Query(connection, dtMatricula.Rows[0].Aluno, dtMatricula.Rows[0].Ano,
                                     dtMatricula.Rows[0].Semestre);
                    return qtContrato.Rows.Count > 0;
                }
            }

            return true;
        }

        public static QueryTable ConsultarTurma(string ano, string semestre, DbObject curso, string turno, string serie)
        {
            if (turno == null)
            {
                turno = string.Empty;
            }
            if (serie == null)
            {
                serie = "0";
            }
            string sql =
                "select distinct turma from ly_grade_serie s inner join ly_grade_turma t on s.grade_id = t.grade_id and t.turma = s.grade " +
                "WHERE s.ano = ? " +
                "AND s.semestre = ? " +
                "AND s.turno = ? " +
                "AND s.curso = ? " +
                "AND s.serie = ? ";
            return Consultar(sql, Convert.ToDecimal(ano), Convert.ToDecimal(semestre), turno, curso,
                             Convert.ToDecimal(serie));
        }

        public static QueryTable ListarTurma(string ano, string semestre, DbObject curso, string turno, string serie,
                                             string unidadeEns)
        {
            if (turno == null)
            {
                turno = string.Empty;
            }
            if (serie == null)
            {
                serie = "0";
            }
            string sql =
                "select '' turma,' <Selecione>' descricao union all select distinct turma + '|' + convert(varchar,s.grade_id) turma, turma descricao  from ly_grade_serie s inner join ly_grade_turma t on s.grade_id = t.grade_id and t.turma = s.grade " +
                "WHERE s.ano = ? " +
                "AND s.semestre = ? " +
                "AND s.turno = ? " +
                "AND s.curso = ? " +
                "AND s.serie = ? " +
                "AND s.unidade_responsavel = ? ";
            return Consultar(sql, Convert.ToDecimal(ano), Convert.ToDecimal(semestre), turno, curso,
                             Convert.ToDecimal(serie), unidadeEns);
        }

        public static QueryTable ListarTurma(string ano, string semestre, DbObject curso, string turno, string curriculo,
                                             string serie, string unidadeEns)
        {
            if (turno == null)
            {
                turno = string.Empty;
            }
            if (serie == null)
            {
                serie = "0";
            }
            string sql =
                "select '' turma,' <Selecione>' descricao union all select distinct turma + '|' + convert(varchar,s.grade_id) turma, turma descricao  from ly_grade_serie s inner join ly_grade_turma t on s.grade_id = t.grade_id and t.turma = s.grade " +
                "WHERE s.ano = ? " +
                "AND s.semestre = ? " +
                "AND s.turno = ? " +
                "AND s.curso = ? " +
                "AND s.curriculo = ? " +
                "AND s.serie = ? " +
                "AND s.unidade_responsavel = ? ";
            return Consultar(sql, Convert.ToDecimal(ano), Convert.ToDecimal(semestre), turno, curso, curriculo,
                             Convert.ToDecimal(serie), unidadeEns);
        }

        /// <summary>
        /// Obtém as turmas abertas.
        /// </summary>
        /// <param name="ano">Ano.</param>
        /// <param name="semestre">Semestre.</param>
        /// <param name="curso">Curso.</param>
        /// <param name="turno">Turno.</param>
        /// <param name="curriculo">Currículo.</param>
        /// <param name="serie">Série.</param>
        /// <param name="unidadeEns">Unidade de Ensino.</param>
        /// <returns>QueryTable contendo as turmas.</returns>
        public static QueryTable ListarTurmasAbertas(string ano, string semestre, DbObject curso, string turno,
                                                     string curriculo, string serie, string unidadeEns)
        {
            if (turno == null)
            {
                turno = string.Empty;
            }
            if (string.IsNullOrEmpty(serie))
            {
                serie = "0";
            }
            string sql =
                @"
                SELECT '' turma,' <Selecione>' descricao 
                
                UNION ALL 
        
                SELECT DISTINCT 
                    t.turma + '|' + CONVERT(VARCHAR,s.grade_id) turma, 
                    t.turma descricao  
                FROM    ly_grade_serie s INNER JOIN 
                        ly_grade_turma t ON 
                            s.grade_id = t.grade_id AND 
                            t.turma = s.grade INNER JOIN
                        ly_turma tu ON
                            tu.disciplina = t.disciplina AND
                            tu.turma = t.turma AND
                            tu.ano = t.ano AND
                            tu.semestre = t.semestre                
                WHERE tu.sit_turma = 'Aberta'
                    AND s.ano = ? 
                    AND s.semestre = ? 
                    AND s.turno = ? 
                    AND s.curso = ? 
                    AND s.curriculo = ? 
                    AND s.serie = ? 
                    AND s.unidade_responsavel = ? ";
            return Consultar(sql, Convert.ToDecimal(ano), Convert.ToDecimal(semestre), turno, curso, curriculo,
                             Convert.ToDecimal(serie), unidadeEns);
        }

        public static RetValue Incluir(Ly_matricula dtMatricula)
        {
            RetValue retorno = null;

            TConnectionWritable connection = Config.CreateWritableConnection();
            if (dtMatricula != null)
            {
                if (dtMatricula.Rows != null)
                {
                    //Ly_matricula.Row.Insert(connection, dtMatricula.Rows[0].Aluno, dtMatricula.Rows[0].Disciplina, dtMatricula.Rows[0].Turma, dtMatricula.Rows[0].Ano, dtMatricula.Rows[0].Semestre, "Matriculado", dtMatricula.Rows[0].Dt_ultalt, "N", MontarParametros(dtMatricula).Colunas, MontarParametros(dtMatricula).ValorColuna);
                    dtMatricula.Put(connection);

                    retorno = VerificarErro(dtMatricula);

                    if (retorno != null && !retorno.Ok)
                    {
                        //connection.Rollback();
                        return retorno;
                    }
                    //else //verifica se a disciplina é de estágio e inclui registro em ly_estagio e em ly_estagio_empresa
                    //{
                    //    try
                    //    {
                    //        if (RN.Estagio.VerificaDisciplinaEstagio(dtMatricula.Rows[0].Disciplina))
                    //        {
                    //            Techne.Lyceum.CR.Ly_estagio.Row dadosEstagio = new Techne.Lyceum.CR.Ly_estagio().NewRow();
                    //            Techne.Lyceum.CR.Ly_estagio_empresa.Row dadosEmpresa = new Techne.Lyceum.CR.Ly_estagio_empresa().NewRow();

                    //            dadosEstagio.Ano = dtMatricula.Rows[0].Ano;
                    //            dadosEstagio.Semestre = dtMatricula.Rows[0].Semestre;
                    //            dadosEstagio.Aluno = dtMatricula.Rows[0].Aluno;
                    //            dadosEstagio.Turma = dtMatricula.Rows[0].Turma;
                    //            dadosEstagio.Dtini = DateTime.Today;

                    //            dadosEmpresa.Ano = dtMatricula.Rows[0].Ano;
                    //            dadosEmpresa.Semestre = dtMatricula.Rows[0].Semestre;
                    //            dadosEmpresa.Aluno = dtMatricula.Rows[0].Aluno;
                    //            dadosEmpresa.Turma = dtMatricula.Rows[0].Turma;

                    //            dadosEmpresa.Disciplina = dtMatricula.Rows[0].Disciplina;
                    //            dadosEstagio.Disciplina = dtMatricula.Rows[0].Disciplina;

                    //            retorno = RN.Estagio.IncluirEstagio(dadosEstagio, dadosEmpresa);

                    //            if (retorno != null && !retorno.Ok)
                    //            {
                    //                return retorno;
                    //            }
                    //        }
                    //    }
                    //    catch (Exception e)
                    //    {
                    //        connection.Rollback();
                    //        return new RetValue(false, "", new ErrorList(e.Message));
                    //    }
                    //}
                    return new RetValue(true, "Registro incluído com sucesso.", null);
                }
            }

            return retorno;
        }

        #region Exclusão de Matrículas e Matrículas com Grades com verificação das Notas/Faltas/Licença/Ocorrências/PresencaSemCartão cadastradas

        public static RetValue Excluir(Ly_matricula dtMatricula)
        {
            RetValue retorno = null;
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);
            DbObject[] parametros = null;
            try
            {
                if (dtMatricula != null)
                {
                    if (dtMatricula.Rows != null)
                    {
                        for (int i = 0; i < dtMatricula.Rows.Count; i++)
                        {
                            parametros = new DbObject[]
                                             {
                                                 dtMatricula.Rows[i].Aluno, dtMatricula.Rows[i].Disciplina,
                                                 dtMatricula.Rows[i].Turma, dtMatricula.Rows[i].Ano,
                                                 dtMatricula.Rows[i].Semestre
                                             };
                            retorno = MayDelete(connection, parametros);
                            if (retorno != null)
                                return retorno;
                        }

                        for (int i = 0; i < dtMatricula.Rows.Count; i++)
                        {
                            //apaga notas nulas
                            if (ConsultaNotasNulas(connection, parametros))
                            {
                                string sql = "delete ly_nota where aluno = '" + dtMatricula.Rows[i].Aluno
                                             + "' and disciplina = '" + dtMatricula.Rows[i].Disciplina
                                             + "' and ano = " + dtMatricula.Rows[i].Ano
                                             + " and semestre = " + dtMatricula.Rows[i].Semestre
                                             + " and conceito is null";
                                TCommand cmd = new TCommand(sql, connection);
                                cmd.ExecuteScalar();
                                retorno = VerificarErro(connection.GetErrors());
                                if (retorno != null && !retorno.Ok)
                                {
                                    connection.Rollback();
                                    return retorno;
                                }
                            }

                            //apaga faltas zeradas
                            if (ConsultaFaltasNulas(connection, parametros))
                            {
                                string sql = "delete ly_falta where aluno = '" + dtMatricula.Rows[i].Aluno
                                             + "' and disciplina = '" + dtMatricula.Rows[i].Disciplina
                                             + "' and ano = " + dtMatricula.Rows[i].Ano
                                             + " and periodo = " + dtMatricula.Rows[i].Semestre
                                             + " and faltas = 0";
                                TCommand cmd = new TCommand(sql, connection);
                                cmd.ExecuteScalar();
                                retorno = VerificarErro(connection.GetErrors());
                                if (retorno != null && !retorno.Ok)
                                {
                                    connection.Rollback();
                                    return retorno;
                                }
                            }

                            //insere na tabela de backup
                            string sqlBKP =
                                @"	INSERT INTO LY_MATRICULA_BKP (ALUNO, DISCIPLINA, TURMA, ANO, SEMESTRE, SIT_MATRICULA,
	                        DT_ULTALT, DT_INSERCAO, DT_MATRICULA, NUM_CHAMADA, STAMP_ATUALIZACAO, COMENTARIO, DT_EXCLUSAO) 
	                        SELECT ALUNO, DISCIPLINA, TURMA, ANO, SEMESTRE, SIT_MATRICULA,
	                        DT_ULTALT, DT_INSERCAO, DT_MATRICULA, NUM_CHAMADA, STAMP_ATUALIZACAO, 'Exclusão tela Matrícula por Disciplina', GETDATE() 
	                        FROM LY_MATRICULA WHERE ALUNO = ? 
	                        AND DISCIPLINA = ?
	                        AND TURMA = ?
	                        AND ANO = ?
	                        AND SEMESTRE = ? ";
                            TCommand.ExecuteNonQuery(connection, sqlBKP, dtMatricula.Rows[i].Aluno,
                                                     dtMatricula.Rows[i].Disciplina, dtMatricula.Rows[i].Turma,
                                                     dtMatricula.Rows[i].Ano, dtMatricula.Rows[i].Semestre);
                            retorno = VerificarErro(connection.GetErrors());
                            if (retorno != null && !retorno.Ok)
                            {
                                return retorno;
                            }

                            Ly_matricula.Row.Delete(connection, dtMatricula.Rows[i].Aluno,
                                                    dtMatricula.Rows[i].Disciplina, dtMatricula.Rows[i].Turma,
                                                    dtMatricula.Rows[i].Ano, dtMatricula.Rows[i].Semestre);
                        }

                        retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null)
                            return new RetValue(false, "", null);

                        return new RetValue(true, "Registro excluído com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        public static RetValue ExcluirReclassificacao(Ly_matricula dtMatricula, Ly_matgrade dtMatGrade)
        {
            RetValue retorno = null;

            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);
            DbObject[] parametros = null;

            try
            {
                if (dtMatricula != null)
                {
                    if (dtMatricula.Rows != null)
                    {
                        for (int i = 0; i < dtMatricula.Rows.Count; i++)
                        {
                            parametros = new DbObject[]
                                             {
                                                 dtMatricula.Rows[i].Aluno, dtMatricula.Rows[i].Disciplina,
                                                 dtMatricula.Rows[i].Turma, dtMatricula.Rows[i].Ano,
                                                 dtMatricula.Rows[i].Semestre
                                             };
                            retorno = MayDelete(connection, parametros);
                            if (retorno != null)
                            {
                                return retorno;
                            }
                        }

                        for (int i = 0; i < dtMatricula.Rows.Count; i++)
                        {
                            parametros = new DbObject[]
                                             {
                                                 dtMatricula.Rows[i].Aluno, dtMatricula.Rows[i].Disciplina,
                                                 dtMatricula.Rows[i].Turma, dtMatricula.Rows[i].Ano,
                                                 dtMatricula.Rows[i].Semestre
                                             };

                            //apaga notas nulas
                            if (ConsultaNotasNulas(connection, parametros))
                            {
                                string sql = "delete ly_nota where aluno = '" + dtMatricula.Rows[i].Aluno
                                             + "' and disciplina = '" + dtMatricula.Rows[i].Disciplina
                                             + "' and ano = " + dtMatricula.Rows[i].Ano
                                             + " and semestre = " + dtMatricula.Rows[i].Semestre
                                             + " and conceito is null";
                                TCommand cmd = new TCommand(sql, connection);
                                cmd.ExecuteScalar();
                                retorno = VerificarErro(connection.GetErrors());
                                if (retorno != null && !retorno.Ok)
                                {
                                    connection.Rollback();
                                    return retorno;
                                }
                            }

                            //apaga faltas zeradas
                            if (ConsultaFaltasNulas(connection, parametros))
                            {
                                string sql = "delete ly_falta where aluno = '" + dtMatricula.Rows[i].Aluno
                                             + "' and disciplina = '" + dtMatricula.Rows[i].Disciplina
                                             + "' and ano = " + dtMatricula.Rows[i].Ano
                                             + " and periodo = " + dtMatricula.Rows[i].Semestre
                                             + " and faltas = 0";
                                TCommand cmd = new TCommand(sql, connection);
                                cmd.ExecuteScalar();
                                retorno = VerificarErro(connection.GetErrors());
                                if (retorno != null && !retorno.Ok)
                                {
                                    connection.Rollback();
                                    return retorno;
                                }
                            }

                            //insere na tabela de backup
                            string sqlBKP =
                                @"	INSERT INTO LY_MATRICULA_BKP (ALUNO, DISCIPLINA, TURMA, ANO, SEMESTRE, SIT_MATRICULA,
	                        DT_ULTALT, DT_INSERCAO, DT_MATRICULA, NUM_CHAMADA, STAMP_ATUALIZACAO, COMENTARIO, DT_EXCLUSAO) 
	                        SELECT ALUNO, DISCIPLINA, TURMA, ANO, SEMESTRE, SIT_MATRICULA,
	                        DT_ULTALT, DT_INSERCAO, DT_MATRICULA, NUM_CHAMADA, STAMP_ATUALIZACAO, 'Exclusão tela Reclassificação', GETDATE() 
	                        FROM LY_MATRICULA WHERE ALUNO = ? 
	                        AND DISCIPLINA = ?
	                        AND TURMA = ?
	                        AND ANO = ?
	                        AND SEMESTRE = ? ";
                            TCommand.ExecuteNonQuery(connection, sqlBKP, dtMatricula.Rows[i].Aluno,
                                                     dtMatricula.Rows[i].Disciplina, dtMatricula.Rows[i].Turma,
                                                     dtMatricula.Rows[i].Ano, dtMatricula.Rows[i].Semestre);
                            retorno = VerificarErro(connection.GetErrors());
                            if (retorno != null && !retorno.Ok)
                            {
                                return retorno;
                            }


                            Ly_matricula.Row.Delete(connection, dtMatricula.Rows[i].Aluno,
                                                    dtMatricula.Rows[i].Disciplina, dtMatricula.Rows[i].Turma,
                                                    dtMatricula.Rows[i].Ano, dtMatricula.Rows[i].Semestre);
                        }

                        retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null)
                            return retorno;

                        if (dtMatGrade.Rows.Count > 0)
                            Ly_matgrade.Row.Delete(connection, dtMatGrade.Rows[0].Aluno, dtMatGrade.Rows[0].Grade_id,
                                                   dtMatGrade.Rows[0].Dt_ultalt);

                        retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null)
                            return new RetValue(false, "", null);

                        return new RetValue(true, "Registro excluído com sucesso.", null);
                    }
                }
            }
            catch (Exception e)
            {
                connection.Rollback();
                return new RetValue(false, null, new ErrorList(e.Message));
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        public static RetValue MayDelete(TConnection connection, DbObject[] parametros)
        {
            RetValue retorno = null;
            retorno = ConsultaProvas(connection, parametros);
            if (retorno != null)
                return retorno;
            retorno = ConsultaFaltas(connection, parametros);
            if (retorno != null)
                return retorno;
            retorno = ConsultaLicenca(connection, parametros);
            if (retorno != null)
                return retorno;
            retorno = ConsultaOcorrencia(connection, parametros);
            if (retorno != null)
                return retorno;
            retorno = ConsultaPresencaSemCartao(connection, parametros);
            if (retorno != null)
                return retorno;
            return retorno;
        }

        public static RetValue ConsultaProvas(TConnection connection, DbObject[] parametros)
        {

            string sql = "SELECT PROVA FROM LY_NOTA WHERE " +
                         "ALUNO = ? AND DISCIPLINA = ? AND " +
                         "TURMA = ?  AND ANO = ? AND SEMESTRE = ? and CONCEITO is not null";
            RetValue retorno = null;
            QueryTable qt = null;
            qt = new QueryTable(sql);
            qt.Query(connection, parametros);
            if (qt.Rows.Count > 0)
                retorno = new RetValue(false, "",
                                       new ErrorList("Existe nota de avaliação cadastrada para disciplina " +
                                                     parametros.GetValue(1) + " ."));
            return retorno;
        }

        public static bool ConsultaNotasNulas(TConnection connection, DbObject[] parametros)
        {
            string sql = "SELECT 1 FROM LY_NOTA WHERE " +
                         "ALUNO = ? AND DISCIPLINA = ? AND " +
                         "TURMA = ?  AND ANO = ? AND SEMESTRE = ? and CONCEITO is null";
            int retorno = ExecutarFuncao(sql, parametros);

            if (retorno == 1)
            {
                return true;
            }

            return false;
        }

        public static bool ConsultaFaltasNulas(TConnection connection, DbObject[] parametros)
        {
            string sql = "SELECT 1 FROM LY_FALTA WHERE " +
                         "ALUNO = ? AND DISCIPLINA = ? AND " +
                         "TURMA = ?  AND ANO = ? AND PERIODO = ? and FALTAS = 0";
            int retorno = ExecutarFuncao(sql, parametros);
            if (retorno == 1)
            {
                return true;
            }

            return false;
        }

        public static RetValue ConsultaLicenca(TConnection connection, DbObject[] parametros)
        {
            string sql = "SELECT ID_ALUNO_LICENCA FROM ly_aluno_licenca WHERE " +
                         "ALUNO = ? AND DISCIPLINA = ? AND " +
                         "TURMA = ?  AND ANO = ? AND SEMESTRE = ?";
            RetValue retorno = null;
            QueryTable qt = null;
            qt = new QueryTable(sql);
            qt.Query(connection, parametros);
            if (qt.Rows.Count > 0)
                retorno = new RetValue(false, "",
                                       new ErrorList("Existe falta justificada cadastrada para esta matrícula."));
            return retorno;
        }

        public static RetValue ConsultaPresencaSemCartao(TConnection connection, DbObject[] parametros)
        {
            string sql = "SELECT ID_PRESENCA_SEM_CARTAO FROM ly_presenca_sem_cartao WHERE " +
                         "ALUNO = ? AND DISCIPLINA = ? AND " +
                         "TURMA = ?  AND ANO = ? AND SEMESTRE = ?";
            RetValue retorno = null;
            QueryTable qt = null;
            qt = new QueryTable(sql);
            qt.Query(connection, parametros);
            if (qt.Rows.Count > 0)
            {
                retorno = new RetValue(false, "",
                                       new ErrorList("Existe presença sem cartão cadastrada para esta matrícula."));
            }
            return retorno;
        }

        public static RetValue ConsultaOcorrencia(TConnection connection, DbObject[] parametros)
        {
            string sql = "SELECT * FROM ly_ocorrencia WHERE " +
                         "ALUNO = ? AND DISCIPLINA = ? AND " +
                         "TURMA = ?  AND ANO = ? AND PERIODO = ?";
            RetValue retorno = null;
            QueryTable qt = null;
            qt = new QueryTable(sql);
            qt.Query(connection, parametros);
            if (qt.Rows.Count > 0)
            {
                retorno = new RetValue(false, "", new ErrorList("Existe ocorrência cadastrada para esta matrícula."));
            }
            return retorno;
        }

        public static RetValue ConsultaFaltas(TConnection connection, DbObject[] parametros)
        {
            string sql = "SELECT Faltas FROM LY_FALTA WHERE " +
                         "ALUNO = ? AND DISCIPLINA = ? AND " +
                         "TURMA = ? AND ANO = ? AND PERIODO = ? and FALTAS <> 0 ";
            RetValue retorno = null;
            QueryTable qt = null;
            qt = new QueryTable(sql);
            qt.Query(connection, parametros);
            if (qt.Rows.Count > 0)
                retorno = new RetValue(false, "",
                                       new ErrorList("Existem faltas cadastradas para disciplina " +
                                                     parametros.GetValue(1) + " ."));
            return retorno;
        }

        #endregion

        /// <summary>
        /// Consulta informações das matrículas da turma para fechamento de período.
        /// </summary>
        /// <param name="conn">Conexão.</param>
        /// <param name="turma">Turma.</param>
        /// <param name="ano">Ano da turma.</param>
        /// <param name="semestre">Semestre da turma.</param>
        /// <returns>QueryTable contendo informações das matrículas.</returns>
        public static QueryTable ConsultarDadosFechamentoMatriculas(TConnection conn, string turma, string ano,
                                                                    string semestre)
        {
            return Consultar(conn,
                             @"SELECT	m.aluno,
                            m.disciplina,
                            m.sit_matricula,        
		                    m.sit_detalhe,
		                    m.obs,
		                    m.num_chamada,
		                    m.dt_ultalt,
		                    m.dt_matricula,
		                    m.tipo_aprovacao,
		                    a.serie
                    FROM	ly_matricula m (NOLOCK) INNER JOIN
		                    ly_aluno a (NOLOCK) ON a.aluno = m.aluno INNER JOIN
		                    ly_turma t (NOLOCK) ON	m.disciplina = t.disciplina AND
								                    m.turma = t.turma AND
								                    m.ano = t.ano AND
								                    m.semestre = t.semestre
                    WHERE	t.turma = ? AND t.ano = ? AND t.semestre = ?",
                             turma, ano, semestre);
        }

        public static QueryTable ConsultaMatriculaPorDisciplina(string aluno, decimal ano, decimal semestre)
        {
            string sql =
                "Select m.disciplina, (m.disciplina + ' - ' + d.nome) disciplinashow, m.turma, m.ano, m.semestre, m.sit_matricula " +
                "from ly_matricula m inner join ly_disciplina d on m.disciplina = d.disciplina " +
                "where aluno = ? " +
                "and ano = ? " +
                "and semestre = ? " +
                "and not exists (select 1 from ly_aluno a join ly_matgrade mg on a.aluno = mg.aluno " +
                "join ly_grade_turma gt on  mg.grade_id = gt.Grade_id and gt.disciplina = m.disciplina and gt.turma = m.turma " +
                "and gt.ano = m.ano and gt.semestre = m.semestre where a.aluno = m.aluno)";
            return Consultar(sql, aluno, ano, semestre);
        }

        public DataTable ListaTurmaPor(int ano, int periodo, string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable turmas = null;

            try
            {
                contextQuery.Command = @" SELECT  DISTINCT
                                TURMA
                        FROM    DBO.LY_MATRICULA
                        WHERE   ALUNO = @ALUNO
                                AND SIT_MATRICULA = @SIT_MATRICULA
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE ";

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);
                contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);

                turmas = ctx.GetDataTable(contextQuery);
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

            return turmas;
        }

        public static QueryTable ListarTurma(string aluno, decimal ano, decimal semestre, string disciplina)
        {
            string sql =
                "Select top 30 TURMA from LY_TURMA t where t.ANO = ? and t.SEMESTRE = ? and not exists(Select top 1 1 From LY_MATGRADE mg " +
                "inner join LY_GRADE_TURMA gt on gt.GRADE_ID = mg.GRADE_ID inner join LY_MATRICULA m on m.ANO = gt.ANO " +
                "and m.SEMESTRE = gt.SEMESTRE and m.DISCIPLINA = gt.DISCIPLINA and m.TURMA = gt.TURMA where mg.ALUNO = ? " +
                "and gt.ANO = ? and gt.SEMESTRE = ? AND gt.TURMA = t.TURMA) and t.DISCIPLINA = ?";
            return Consultar(sql, ano, semestre, aluno, ano, semestre, disciplina);
        }

        public static RetValue Alterar(Ly_matricula dtMatricula)
        {
            RetValue retorno = null;

            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                if (dtMatricula != null)
                {
                    if (dtMatricula.Rows != null)
                    {
                        ColunasTable colunas = MontarParametros(dtMatricula.Columns, dtMatricula.Rows[0]);

                        Ly_matricula.Row.Update(connection, dtMatricula.Rows[0].Aluno, dtMatricula.Rows[0].Disciplina,
                                                dtMatricula.Rows[0].Turma, dtMatricula.Rows[0].Ano,
                                                dtMatricula.Rows[0].Semestre, "sit_matricula,dt_ultalt,dt_matricula",
                                                dtMatricula.Rows[0].Sit_matricula, dtMatricula.Rows[0].Dt_ultalt,
                                                dtMatricula.Rows[0].Dt_matricula);

                        retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null && !retorno.Ok)
                        {
                            connection.Rollback();
                            return retorno;
                        }

                        return new RetValue(true, "Registro incluído com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }

            return retorno;
        }

        public void CancelaMatriculaPor(DataContext ctx, string aluno, string turma, decimal ano, decimal periodo, string usuarioResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE LY_MATRICULA 
                        SET    SIT_MATRICULA = @SIT_MATRICULA_CANCELADA, 
                               DT_ULTALT = @DT_ULTALT,
                               MATRICULA = @MATRICULA 
                        WHERE  ALUNO = @ALUNO 
                               AND TURMA = @TURMA 
                               AND ANO = @ANO
                               AND SEMESTRE = @SEMESTRE
                               AND SIT_MATRICULA = @SIT_MATRICULA  ";

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);
                contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
                contextQuery.Parameters.Add("@SIT_MATRICULA_CANCELADA", Cancelado);
                contextQuery.Parameters.Add("@DT_ULTALT", DateTime.Now);
                contextQuery.Parameters.Add("@MATRICULA", usuarioResponsavel);

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

        /// <summary>
        /// Atualiza a situação da matrícula do aluno (Ly_matricula.Sit_matricula)
        /// </summary>
        /// <param name="connW">Conexão.</param>
        /// <param name="aluno">Código do aluno.</param>
        /// <param name="disciplina">Código da disciplina da turma do aluno.</param>
        /// <param name="turma">Turma.</param>
        /// <param name="ano">Ano da turma do aluno.</param>
        /// <param name="semestre">Semestre da turma do aluno.</param>
        /// <param name="novaSituacao">Nova situação da matrícula do aluno.</param>
        /// <returns>Erro, se existir.</returns>
        public static RetValue AtualizarSituacaoMatricula(TConnectionWritable connW, string aluno, string disciplina,
                                                          string turma, string ano, string semestre, string novaSituacao)
        {
            return IAE(connW,
                       @"  UPDATE ly_matricula 
                    SET sit_matricula = ? 
                    WHERE aluno = ? AND disciplina = ? AND turma = ? AND ano = ? AND semestre = ?",
                       novaSituacao, aluno, disciplina, turma, ano, semestre);
        }

        /// <summary>
        /// Atualiza a situação da matrícula na grade do aluno (Ly_matgrade.sit_matgrade).
        /// </summary>
        /// <param name="connW">Conexão.</param>
        /// <param name="gradeId">Grade ID da turma do aluno.</param>
        /// <param name="aluno">Código do aluno.</param>
        /// <param name="novaSituacao">Nova situação da matrícula da grade do aluno.</param>
        /// <returns>Erro, se existir.</returns>
        public static RetValue AtualizarSituacaoMatGrade(TConnectionWritable connW, string gradeId, string aluno,
                                                         string novaSituacao)
        {
            return IAE(connW,
                       @"  UPDATE  ly_matgrade
                    SET     sit_matgrade = ?, dt_ultalt = GETDATE()
                    WHERE   aluno = ? AND grade_id = ?",
                       novaSituacao, aluno, gradeId);
        }

        public static QueryTable ConsultarMatricula(string aluno, string disciplina)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            QueryTable qt = null;
            string sql = "Select ano, semestre, turma from Ly_matricula WHERE aluno = ?  and disciplina = ? and sit_matricula = 'Matriculado'";
            try
            {
                qt = new QueryTable(sql);
                DbObject[] parametros = new DbObject[] { aluno, disciplina };
                qt.Query(connection, parametros);
            }
            finally
            {
                connection.Close();
            }
            return qt;
        }

        public static bool ConsultaMatriculaPorAno(string matricula, decimal ano, string turma)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT 1
                        FROM    ly_matgrade mg
                                JOIN ly_grade_turma gt ON mg.grade_id = gt.grade_id
                                JOIN ly_grade_serie gs ON gs.GRADE_ID = mg.GRADE_ID
                                                          AND gs.GRADE = gt.TURMA
                                                          AND gs.GRADE = gt.TURMA
                                JOIN ly_matricula m ON m.ALUNO = mg.ALUNO
                                                       AND gt.disciplina = m.disciplina
                                                       AND m.turma = gt.turma
                                                       AND m.ANO = gt.ANO
                                                       AND m.SEMESTRE = gt.SEMESTRE
                        WHERE   m.SIT_MATRICULA = @SIT_MATRICULA
                                AND m.turma = @turma
                                AND mg.aluno = @matricula
                                AND m.ANO = @ano");

                contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
                contextQuery.Parameters.Add("@matricula", matricula);
                contextQuery.Parameters.Add("@ano", ano);
                contextQuery.Parameters.Add("@turma", turma);

                var obj = ctx.GetReturnValue(contextQuery);
                if (obj == null)
                {
                    return false;
                }
                return true;
            }
        }

        public static QueryTable ConsultarMatriculas(String disciplina, String turma, decimal ano, decimal periodo,
                                                     decimal? subperiodo)
        {
            if (String.IsNullOrEmpty(disciplina) ||
                String.IsNullOrEmpty(turma))
            {
                return null;
            }

            Ly_prova.Row[] provas = RN.ProvaTurma.ConsultarProvas(disciplina, turma, ano, periodo, subperiodo);

            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql =
                    @"select 
	                        PE.nome_compl, 
	                        m.aluno, 
	                        m.sit_matricula, 
	                        m.num_chamada, 
	                        m.disciplina, 
	                        m.turma, 
	                        m.ano, 
	                        m.semestre ";

                foreach (Ly_prova.Row p in provas)
                {
                    String pStr = p.Prova;
                    sql +=
                        @",
	                        (select isnull(conceito, 'SN') from LY_NOTA n where
		                    n.ALUNO = m.ALUNO and
		                    n.DISCIPLINA = m.DISCIPLINA and
		                    n.TURMA = m.TURMA and
		                    n.ANO = m.ANO and
		                    n.SEMESTRE = m.SEMESTRE and
		                    n.PROVA = '" +
                        RN.RNBase.MudarAspas(pStr) + "') as '" + RN.RNBase.MudarAspas(pStr) + "' ";
                }
                sql +=
                    @"from ly_matricula m join ly_aluno a
	                        on m.aluno = a.aluno
                        INNER JOIN LY_PESSOA PE (NOLOCK)  ON PE.PESSOA = A.PESSOA
                        where m.disciplina = ?
                          and m.turma = ?
                          and m.ano = ?
                          and m.semestre = ?";

                //union

                sql +=
                    @" UNION select 
	                        PE.nome_compl, 
	                        m.aluno, 
	                        m.SITUACAO_HIST, 
	                        m.num_chamada, 
	                        m.disciplina, 
	                        m.turma, 
	                        m.ano, 
	                        m.semestre ";

                foreach (Ly_prova.Row p in provas)
                {
                    String pStr = p.Prova;
                    sql +=
                        @",
	                        (select isnull(conceito, 'SN') from LY_NOTA_HISTMATR n where
		                    n.ALUNO = m.ALUNO and
		                    n.DISCIPLINA = m.DISCIPLINA and
		                    --n.TURMA = m.TURMA and
		                    n.ANO = m.ANO and
		                    n.SEMESTRE = m.SEMESTRE and
		                    n.NOTA_ID = '" +
                        RN.RNBase.MudarAspas(pStr) + "') as '" + RN.RNBase.MudarAspas(pStr) + "' ";
                }
                sql +=
                    @"from LY_HISTMATRICULA m join ly_aluno a
	                        on m.aluno = a.aluno
                        INNER JOIN LY_PESSOA PE (NOLOCK)  ON PE.PESSOA = A.PESSOA
                        where m.disciplina = ?
                          and m.turma = ?
                          and m.ano = ?
                          and m.semestre = ?";

                QueryTable qt = new QueryTable(sql);
                qt.Query(connection, disciplina, turma, ano, periodo, disciplina, turma, ano, periodo);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable ConsultarMatriculasDOL(String disciplina, String turma, decimal ano, decimal periodo,
                                                        decimal? subperiodo)
        {
            if (String.IsNullOrEmpty(disciplina) ||
                String.IsNullOrEmpty(turma))
            {
                return null;
            }

            Ly_prova.Row[] provas = RN.ProvaTurma.ConsultarProvas(disciplina, turma, ano, periodo, subperiodo);

            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql =
                    @"select 
	                        PE.nome_compl, 
	                        m.aluno, 
	                        m.sit_matricula, 
	                        m.num_chamada, 
	                        m.disciplina, 
	                        m.turma, 
	                        m.ano, 
	                        m.semestre ";

                foreach (Ly_prova.Row p in provas)
                {
                    String pStr = p.Prova;
                    sql +=
                        @",
	                        (select isnull(conceito, 'SN') from LY_NOTA n WITH(NOLOCK) where
		                    n.ALUNO = m.ALUNO and
		                    n.DISCIPLINA = m.DISCIPLINA and
		                    n.TURMA = m.TURMA and
		                    n.ANO = m.ANO and
		                    n.SEMESTRE = m.SEMESTRE and
		                    n.PROVA = '" +
                        RN.RNBase.MudarAspas(pStr) + "') as '" + RN.RNBase.MudarAspas(pStr) + "' ";
                }

                sql +=
                    @"from ly_matricula m  WITH(NOLOCK) join ly_aluno a WITH(NOLOCK)
	                        on m.aluno = a.aluno
                        INNER JOIN LY_PESSOA PE (NOLOCK)  ON PE.PESSOA = A.PESSOA
                        where m.disciplina = ?
                          and m.turma = ?
                          and m.ano = ?
                          and m.semestre = ?
                          and m.sit_matricula = 'Matriculado'
                        order by m.num_chamada, PE.nome_compl";

                //union

                QueryTable qt = new QueryTable(sql);
                qt.Query(connection, disciplina, turma, ano, periodo);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static DataTable ConsultarMatriculasDOL_Media(String disciplina, String turma, decimal ano, decimal periodo, decimal? subperiodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
//                contextQuery.Command = @"SELECT DISTINCT PE.nome_compl ,
//                            m.aluno ,
//                            m.sit_matricula ,
//                            ISNULL(( SELECT TOP 1
//                                            [DESCRICAO] + ' - ' + CONVERT(VARCHAR, [DT_INICIO], 103)
//                                            + ' a ' + CONVERT(VARCHAR, [DT_FIM], 103)
//                                     FROM   [LYCEUM].[dbo].[LY_ALUNO_LICENCA] WITH ( NOLOCK )
//                                     WHERE  [ALUNO] = m.aluno
//                                            AND [DISCIPLINA] = m.disciplina
//                                            AND [TURMA] = m.turma
//                                            AND [ANO] = m.ano
//                                            AND [SEMESTRE] = m.semestre
//                                            AND DT_FIM = ( SELECT   MAX(DT_FIM)
//                                                           FROM     [LY_ALUNO_LICENCA] WITH ( NOLOCK )
//                                                           WHERE    [ALUNO] = m.aluno
//                                                                    AND [DISCIPLINA] = m.disciplina
//                                                                    AND [TURMA] = m.turma
//                                                                    AND [ANO] = m.ano
//                                                                    AND [SEMESTRE] = m.semestre
//                                                         )
//                                   ), '')
//                            + ISNULL(( SELECT TOP 1
//                                                'Remanejado para a turma ' + TURMA_DESTINO
//                                                + ' em ' + CONVERT(VARCHAR, DATA, 103)
//                                       FROM     dbo.LY_TURMA_TRANSF WITH ( NOLOCK )
//                                       WHERE    ALUNO = m.ALUNO
//                                                AND ANO = m.ANO
//                                                AND PERIODO = m.SEMESTRE
//                                                AND TURMA_ANT = m.TURMA
//                                     ), '')
//                            + ISNULL(( SELECT TOP 1
//                                                LY_MOTIVOSAIDA.DESCRICAO + ' em '
//                                                + CONVERT(VARCHAR, DT_ENCERRAMENTO, 103)
//                                       FROM     dbo.LY_H_CURSOS_CONCL WITH ( NOLOCK )
//                                                INNER JOIN dbo.LY_MOTIVOSAIDA WITH ( NOLOCK ) ON LY_H_CURSOS_CONCL.MOTIVO = LY_MOTIVOSAIDA.MOTIVOSAIDA
//                                       WHERE    ALUNO = m.ALUNO
//                                                AND DT_REABERTURA IS NULL
//                                     ), '')
//                            + ISNULL(( SELECT TOP 1
//                                                CASE WHEN ( LY_H_CURR_ALUNO.TURMA_PREF = m.turma
//                                                            AND m.sit_matricula = 'Matriculado'
//                                                          )
//                                                     THEN ' Transferido para essa unidade da rede estadual em '
//                                                          + CONVERT(VARCHAR, DT_TRANS, 103)
//                                                     WHEN LY_H_CURR_ALUNO.TURMA_PREF <> m.turma
//                                                     THEN ' Transferido para outra unidade da rede estadual em '
//                                                          + CONVERT(VARCHAR, DT_TRANS, 103)
//                                                END
//                                       FROM     dbo.LY_H_CURR_ALUNO WITH ( NOLOCK )
//                                                INNER JOIN dbo.LY_UNIDADE_ENSINO WITH ( NOLOCK ) ON LY_H_CURR_ALUNO.UNIDADE_ENSINO = LY_UNIDADE_ENSINO.UNIDADE_ENS
//                                       WHERE    ALUNO = m.ALUNO
//                                                AND ANO = m.ANO
//                                                AND PERIODO = m.SEMESTRE
//                                     ), '') descricao_situacao ,
//                            m.num_chamada ,
//                            ISNULL (m.num_chamada,99999) As Ordem,
//                            dt_matricula,
//                            m.disciplina ,
//                            m.turma ,
//                            m.ano ,
//                            m.semestre ,
//                            CASE WHEN ( m.sit_matricula = 'Matriculado'
//                                        AND N.NOTAPROVA IS NULL AND N.CONCEITO IS NOT NULL AND RTRIM(N.CONCEITO) <> ''
//                                      ) THEN REPLACE(CAST(REPLACE(n.conceito, ',','.') AS DECIMAL(5,2)),'.',',')
//                                 WHEN ( m.sit_matricula = 'Matriculado'
//                                        AND n.NotaProva IS NOT NULL
//                                      ) THEN REPLACE(CAST(n.NotaProva AS DECIMAL(5,2)),'.',',')
//                                 ELSE NULL
//                            END AS MÉDIA ,
//                            CASE m.sit_matricula
//                              WHEN 'Matriculado' THEN REPLACE(CAST(n.NotaRecuperacao AS DECIMAL(5,2)),'.',',') 
//                              ELSE NULL
//                            END AS NotaRecuperacao ,      
//                            '' AS NotaFinal ,
//                            CASE
//                              WHEN  m.sit_matricula = 'Matriculado' and f.FALTAS is not null THEN CONVERT(INT, f.faltas)
//							  when m.SIT_MATRICULA = 'Matriculado' and f.FALTAS is null then (	select count(*) 
//																								from Turma.FREQUENCIADIARIA fd
//																								inner join Turma.FREQUENCIADIARIA_ALUNOFALTA fda on fd.FREQUENCIADIARIAID=fda.FREQUENCIADIARIAID
//																								inner join LY_SUBPERIODO_LETIVO SL on sl.ANO = fd.ANO and sl.PERIODO = fd.SEMESTRE and sl.SUBPERIODO =  @SUBPERIODO
//																								where  m.DISCIPLINA = fd.disciplina
//																									AND m.turma = fd.TURMA
//																									AND m.ANO = fd.ANO
//																									AND m.SEMESTRE = fd.SEMESTRE                                                                 
//																									AND m.ALUNO = fda.ALUNO                                                                 
//																									AND FDA.ATIVO = 1					                                            
//																									and fd.DATAFREQUENCIA between sl.DT_INICIO and sl.DT_FIM)
//                              ELSE NULL
//                            END AS faltas  ,
//                            prova.nome AS nome_prova ,
//                            prova.nota_max ,
//                            prova.formula ,
//                            CASE m.sit_matricula
//                              WHEN 'Matriculado' THEN n.recuperacao_paralela
//                              ELSE 'N'
//                            END AS recuperacao_paralela,
//                            CASE 
//                              WHEN m.sit_matricula = 'Matriculado' THEN n.sem_avaliacao
//                              ELSE 'N'
//                            END AS sem_avaliacao ,
//                            CASE WHEN m.sit_matricula = 'Matriculado'
//                                      AND MOTIVOSEMNOTAID IS NOT NULL
//                                 THEN CONVERT(VARCHAR(1), n.MOTIVOSEMNOTAID)
//                                 ELSE ''
//                            END AS justificativa
//                    FROM    ly_matricula m WITH ( NOLOCK )
//                            JOIN ly_aluno a WITH ( NOLOCK ) ON m.aluno = a.aluno
//                            INNER JOIN LY_PESSOA PE (NOLOCK)  ON PE.PESSOA = A.PESSOA
//                            LEFT JOIN ly_freq freq WITH ( NOLOCK ) ON freq.disciplina = m.disciplina
//                                                                     AND freq.turma = m.turma
//                                                                     AND freq.ano = m.ano
//                                                                     AND freq.periodo = m.semestre
//                                                                     AND freq.subperiodo = @SUBPERIODO
//                            LEFT JOIN LY_FALTA f WITH ( NOLOCK ) ON f.ALUNO = m.ALUNO
//                                                                   AND f.ANO = m.ANO
//                                                                   AND f.DISCIPLINA = m.DISCIPLINA
//                                                                   AND f.PERIODO = m.SEMESTRE
//                                                                   AND f.TURMA = m.TURMA
//                                                                   AND freq.freq = f.freq
//                            LEFT JOIN ly_prova prova WITH ( NOLOCK ) ON prova.disciplina = m.disciplina
//                                                                       AND prova.turma = m.turma
//                                                                       AND prova.ano = m.ano
//                                                                       AND prova.semestre = m.semestre
//                                                                       AND prova.subperiodo = @SUBPERIODO
//                            LEFT JOIN LY_NOTA n WITH ( NOLOCK ) ON n.DISCIPLINA = prova.disciplina
//                                                                  AND n.turma = prova.TURMA
//                                                                  AND n.ANO = prova.ANO
//                                                                  AND n.SEMESTRE = prova.SEMESTRE
//                                                                  AND n.PROVA = prova.PROVA
//                                                                  AND n.ALUNO = m.ALUNO
//                    WHERE   m.disciplina = @DISCIPLINA
//                            AND m.turma = @TURMA
//                            AND m.ano = @ANO
//                            AND m.semestre = @PERIODO
//                    ORDER BY Ordem, dt_matricula,
//                            PE.nome_compl";

                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"SP_LANCAMENTONOTAS";

                contextQuery.Parameters.Add("@SUBPERIODO", subperiodo);
                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);


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

        public DataTable ListaMatriculasConsolidadoBimestralPor(int ano, int periodo, string turma, string disciplina)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable lista = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"SP_CONSOLIDADOBIMESTRAL";
                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

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

        public static QueryTable ConsultarMatriculasLancamentoNotasHistorico(String disciplina, String turma, string ano,
                                                                             string periodo)
        {
            Frequencia.InfoFreq[] freqs = Frequencia.ConsultarFreqsHistorico(turma, Convert.ToDecimal(ano),
                                                                             Convert.ToDecimal(periodo), disciplina);
            ProvaTurma.InfoProva[] provas = ProvaTurma.ConsultarProvasHistorico(turma, Convert.ToDecimal(ano),
                                                                                Convert.ToDecimal(periodo), disciplina);

            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                StringBuilder sqlSelect = new StringBuilder();
                StringBuilder sqlFrom = new StringBuilder();
                StringBuilder sqlWhere = new StringBuilder();

                sqlSelect.AppendLine(
                    @"
                    SELECT 
                        HM.ALUNO,
                        PE.NOME_COMPL NOME_ALUNO,                        
                        HM.NUM_CHAMADA,
                        HM.ORDEM,
                        HM.SITUACAO_HIST");

                sqlFrom.AppendLine(
                    @"
                    FROM
                        LY_HISTMATRICULA HM (NOLOCK) INNER JOIN
                        LY_ALUNO A (NOLOCK) ON HM.ALUNO = A.ALUNO
                        INNER JOIN LY_PESSOA PE (NOLOCK)  ON PE.PESSOA = A.PESSOA");

                sqlWhere.AppendLine(
                    @"
                    WHERE
                        HM.DISCIPLINA = ? AND
                        HM.TURMA = ? AND
                        HM.ANO = ? AND
                        HM.SEMESTRE = ?
                    ORDER BY PE.NOME_COMPL");

                if (provas != null && provas.Length > 0)
                {
                    for (int iProva = 0; iProva < provas.Length; iProva++)
                    {
                        ProvaTurma.InfoProva prova = provas[iProva];
                        string alias = "N" + (iProva + 1);

                        sqlSelect.AppendLine(
                            String.Format(
                                ", (CASE WHEN {0}.ALUNO IS NULL THEN NULL WHEN {0}.CONCEITO IS NULL THEN 'SN' ELSE {0}.CONCEITO END) {1}",
                                alias, prova.Prova));
                        sqlFrom.AppendLine(
                            String.Format(
                                @"LEFT JOIN LY_NOTA_HISTMATR {0} (NOLOCK) ON 
                        {0}.ALUNO = HM.ALUNO AND {0}.DISCIPLINA = HM.DISCIPLINA AND {0}.ANO = HM.ANO AND {0}.SEMESTRE = HM.SEMESTRE AND {0}.NOTA_ID = '{1}'",
                                alias, prova.Prova));
                    }
                }

                if (freqs != null && freqs.Length > 0)
                {
                    for (int iFreq = 0; iFreq < freqs.Length; iFreq++)
                    {
                        Frequencia.InfoFreq freq = freqs[iFreq];
                        string alias = "F" + (iFreq + 1);
                        sqlSelect.AppendLine(
                            String.Format(
                                ", (CASE WHEN {0}.FALTAS IS NULL THEN 0 ELSE CONVERT(INT,{0}.FALTAS) END) {1}", alias,
                                freq.Freq));
                        sqlFrom.AppendLine(
                            string.Format(
                                @"LEFT JOIN LY_FALTA_HISTMATR {0} (NOLOCK) ON
                        {0}.ALUNO = HM.ALUNO AND {0}.DISCIPLINA = HM.DISCIPLINA AND {0}.ANO = HM.ANO AND {0}.SEMESTRE = HM.SEMESTRE AND {0}.FREQ_ID = '{1}'",
                                alias, freq.Freq));
                    }
                }

                var qt = new QueryTable(sqlSelect.ToString() + sqlFrom.ToString() + sqlWhere.ToString());
                qt.Query(connection, disciplina, turma, ano, periodo);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable ConsultarAnoSemestre(string aluno)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            QueryTable qt = null;
            if (aluno != null)
            {
                string sql = "Select distinct ano, semestre from Ly_matricula " +
                             "WHERE aluno = ? and sit_matricula = 'Matriculado'";

                try
                {
                    qt = new QueryTable(sql);
                    DbObject[] parametros = new DbObject[] { aluno };
                    qt.Query(connection, parametros);
                }
                finally
                {
                    connection.Close();
                }
            }
            return qt;
        }

        public static bool ExisteDadoMatricula(string aluno)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @"  SELECT    1
                                  FROM      Ly_matricula
                                  WHERE     aluno = @aluno
                                            AND sit_matricula = @sit_matricula
                                            AND ANO=YEAR(GETDATE())    "
                };
                contextQuery.Parameters.Add("@sit_matricula", Matriculado);
                contextQuery.Parameters.Add("@aluno", aluno);

                object obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    return true;
                }
            }

            return false;
        }

        public static QueryTable ConsultarTurma(string aluno)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            QueryTable qt = null;
            if (aluno != null)
            {
                string sql = "Select distinct turma from Ly_matricula " +
                             "WHERE aluno = ? and sit_matricula = 'Matriculado'";

                try
                {
                    qt = new QueryTable(sql);
                    DbObject[] parametros = new DbObject[] { aluno };
                    qt.Query(connection, parametros);
                }
                finally
                {
                    connection.Close();
                }
            }
            return qt;
        }

        public static QueryTable CarregarTurmasAluno(string aluno, object curso, string turno, string curriculo,
                                                     string serie)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();

            try
            {
                connection.Open(true);
                string alunoParametro = string.Empty;

                if (aluno != null)
                {
                    alunoParametro = Convert.ToString(aluno);

                    StringBuilder strQuery = new StringBuilder();

                    strQuery.Append(
                        "select distinct m.turma, m.disciplina, d.nome, m.disciplina + '|' + m.disciplina + ' ' + d.nome + '|' + ");
                    strQuery.Append(
                        "convert(varchar,m.ano) + '|' + convert(varchar,m.semestre) + '|' + m.sit_matricula as codigo, ");
                    strQuery.Append("m.ano,m.semestre, m.sit_matricula ");
                    strQuery.Append(
                        "from LY_MATRICULA m join LY_DISCIPLINA d on m.DISCIPLINA = d.DISCIPLINA join LY_TURMA t ");
                    strQuery.Append("on t.TURMA = m.TURMA and t.ANO = m.ANO and t.SEMESTRE = m.SEMESTRE ");
                    strQuery.Append("where m.aluno = ? ");

                    if (curso != null)
                    {
                        strQuery.Append(" and t.curso = '" + RN.RNBase.MudarAspas(curso.ToString()) + "'");
                    }
                    if (!string.IsNullOrEmpty(turno))
                    {
                        strQuery.Append(" and t.turno = '" + RN.RNBase.MudarAspas(turno) + "'");
                    }
                    if (!string.IsNullOrEmpty(curriculo))
                    {
                        strQuery.Append(" and t.curriculo = '" + RN.RNBase.MudarAspas(curriculo) + "'");
                    }
                    if (!string.IsNullOrEmpty(serie))
                    {
                        strQuery.Append(" and t.serie = " + Convert.ToString(serie));
                    }

                    return Consultar(connection, strQuery.ToString(), alunoParametro);
                }
            }
            finally
            {
                connection.Close();
            }
            return null;
        }

        public static QueryTable ConsultaMatriculaPorDisciplinaProgParcial(string turma, decimal ano, decimal semestre)
        {
            // m.disciplina, (m.disciplina + ' - ' + d.nome) disciplinashow, inner join ly_disciplina d on m.disciplina = d.disciplina
            string sql =
                @"Select DISTINCT a.ALUNO as ALUNO1, PE.NOME_COMPL as NOME_COMPL1, Null as NUM_CHAMADA1,
                                    m.SIT_MATRICULA as SIT_MATRICULA, '' as NOVATURMA ,
                                   
                                    m.turma, m.ano, m.semestre
                                    from ly_matricula m inner join LY_ALUNO a on a.ALUNO=m.ALUNO
                                    INNER JOIN LY_PESSOA PE (NOLOCK)  ON PE.PESSOA = A.PESSOA
                                    where m.turma = ? 
                                    and ano = ?
                                    and semestre = ? ORDER BY PE.NOME_COMPL ";
            return Consultar(sql, turma, ano, semestre);
        }

        public static QueryTable ConsultarProvasTurma(String aluno)
        {
            TConnection connection = Config.CreateConnection();
            QueryTable qt = null;

            try
            {
                connection.Open();

                qt =
                    new QueryTable(
                        @"
                    SELECT pv.DISCIPLINA,	
	                    d.NOME_COMPL AS NOME_DISCIPLINA,
	                    (pv.DISCIPLINA + ' - ' + d.NOME_COMPL) AS DISCIPLINA_DESCR,
	                    pv.NOME AS NOME_PROVA,
	                    pv.PROVA,
	                    pv.DT_PROVA AS DATA_PROVA, 
	                    t.TURMA,
	                    pv.SUBDISCIPLINA, 
	                    d.AVAL_COMPETENCIA,
	                    MONTH(pv.DT_PROVA) AS MES,
	                    CASE MONTH(pv.DT_PROVA) 
		                    WHEN 1 THEN 'Janeiro' 
		                    WHEN 2 THEN 'Fevereiro' 
		                    WHEN 3 THEN 'Março' 
		                    WHEN 4 THEN 'Abril' 
		                    WHEN 5 THEN 'Maio' 
		                    WHEN 6 THEN 'Junho' 
		                    WHEN 7 THEN 'Julho' 
		                    WHEN 8 THEN 'Agosto' 
		                    WHEN 9 THEN 'Setembro' 
		                    WHEN 10 THEN 'Outubro' 
		                    WHEN 11 THEN 'Novembro' 
		                    WHEN 12 THEN 'Dezembro' 
	                    END AS MES_DESCRICAO
                    FROM LY_PROVA pv
                    INNER JOIN LY_MATRICULA m ON pv.DISCIPLINA = m.DISCIPLINA 
                        AND pv.TURMA = m.TURMA 
                        AND pv.ANO = m.ANO 
                        AND pv.SEMESTRE = m.SEMESTRE
                    INNER JOIN LY_DISCIPLINA d ON m.DISCIPLINA = d.DISCIPLINA 
                    INNER JOIN LY_TURMA t ON m.DISCIPLINA = t.DISCIPLINA 
                        AND m.TURMA = t.TURMA 
                        AND m.ANO = t.ANO 
                        AND m.SEMESTRE = t.SEMESTRE
                    WHERE pv.DT_PROVA IS NOT NULL
	                    AND pv.FORMULA IS NULL
	                    AND m.SIT_MATRICULA <> 'Cancelado'
	                    AND m.ALUNO = ?
                    ORDER BY pv.DT_PROVA ");
                qt.Query(connection, aluno);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        //        /// <summary>
        //        /// Enturma o aluno matriculando-o nas disciplinas (Ly_matricula).
        //        /// </summary>
        //        /// <param name="connW">Conexão.</param>
        //        /// <param name="aluno">Matrícula do aluno.</param>
        //        /// <param name="gradeId">Grade ID da turma em que o aluno será matriculado.</param>
        //        /// <returns>Erro, caso ocorra.</returns>
        //        public static RetValue EnturmarAlunoMatricula(TConnectionWritable connW, string aluno, string gradeId)
        //        {
        //            return IAE(connW,
        //                    @" DECLARE @aluno T_CODIGO,
        //                            @grade_id T_NUMERO_GRANDE
        //
        //                    SET @aluno = ?
        //                    SET @grade_id = ?
        //
        //                    INSERT INTO LY_MATRICULA(aluno,disciplina,turma,ano,semestre,sit_matricula,dt_ultalt,cobranca_sep,dt_matricula,dt_insercao,stamp_atualizacao)
        //                    SELECT @aluno,disciplina,turma,ano,semestre,'Matriculado',GETDATE(),'N',GETDATE(),GETDATE(),GETDATE()
        //                    FROM ly_grade_turma (NOLOCK)
        //                    WHERE grade_id = @grade_id",
        //                       aluno, gradeId);
        //        }

        /// <summary>
        /// Enturma o aluno matriculando-o na grade (Ly_matgrade).
        /// </summary>
        /// <param name="connW">Conexão.</param>
        /// <param name="aluno">Matrícula do aluno.</param>
        /// <param name="grade_id">Grade ID da turma em que o aluno será matriculado.</param>
        /// <returns>Erro, caso ocorra.</returns>
        public static RetValue EnturmarAlunoMatGrade(TConnectionWritable connW, string aluno, string grade_id)
        {
            Ly_matgrade.Row.Insert(connW, aluno, Convert.ToDecimal(grade_id), Matriculado, DateTime.Now);
            return VerificarErro(connW.GetErrors());
        }

        //internal static void InserirOuAtualizar(DataContext ctx, TceConfirmacaoMatricula confirmacaoMatricula, string turma, string disciplina)
        //{
        //    RN.Disciplina rnDisciplina = new Disciplina();

        //    //Verifica se a disciplina não é uma eletiva com enturmação separada
        //        if (ExisteMatricula(confirmacaoMatricula.Aluno, disciplina, turma, confirmacaoMatricula.Ano,
        //                            confirmacaoMatricula.Periodo))
        //        {
        //            Alterar(
        //                ctx,
        //                new LyMatricula
        //                {
        //                    Aluno = confirmacaoMatricula.Aluno,
        //                    Disciplina = disciplina,
        //                    Turma = turma,
        //                    Ano = confirmacaoMatricula.Ano,
        //                    Semestre = confirmacaoMatricula.Periodo,
        //                    Matricula = confirmacaoMatricula.Matricula
        //                });
        //        }
        //        else
        //        {
        //            Inserir(
        //                ctx,
        //                new LyMatricula
        //                {
        //                    Aluno = confirmacaoMatricula.Aluno,
        //                    Disciplina = disciplina,
        //                    Turma = turma,
        //                    Ano = confirmacaoMatricula.Ano,
        //                    Semestre = confirmacaoMatricula.Periodo,
        //                    Matricula = confirmacaoMatricula.Matricula
        //                });
        //        }
        //}

        private static void Inserir(DataContext ctx, LyMatricula matricula)
        {
            ctx.ApplyModifications(
                new ContextQuery(
                    @"INSERT  INTO dbo.LY_MATRICULA
                            (
                              ALUNO,
                              DISCIPLINA,
                              TURMA,
                              ANO,
                              SEMESTRE,
                              SIT_MATRICULA,
                              DT_ULTALT,
                              COBRANCA_SEP,
                              DT_INSERCAO,
                              DT_MATRICULA,
                              MATRICULA
                            )
                    VALUES  (
                              @ALUNO,
                              @DISCIPLINA,
                              @TURMA,
                              @ANO,
                              @SEMESTRE,
                              @SIT_MATRICULA,
                              GETDATE(),
                              'N',
                              GETDATE(),
                              GETDATE(),
                              @MATRICULA
                            )",
                    new ContextQueryParameter("@ALUNO", matricula.Aluno),
                    new ContextQueryParameter("@DISCIPLINA", matricula.Disciplina),
                    new ContextQueryParameter("@TURMA", matricula.Turma),
                    new ContextQueryParameter("@ANO", matricula.Ano),
                    new ContextQueryParameter("@SIT_MATRICULA", Matriculado),
                    new ContextQueryParameter("@MATRICULA", matricula.Matricula),
                    new ContextQueryParameter("@SEMESTRE", matricula.Semestre)));
        }

        private static void Alterar(DataContext ctx, LyMatricula matricula)
        {
            ctx.ApplyModifications(
                new ContextQuery(
                    @"UPDATE  LY_MATRICULA
                    SET     SIT_MATRICULA = @SIT_MATRICULA,
                            DT_ULTALT = GETDATE(),
                            DT_MATRICULA = GETDATE(),
                            CONCOMITANTE = 'N',
                            DEPENDENCIA = 'N',
                            SERIE_REFERENCIA = NULL, 
                            DISCIPLINA_REFERENCIA = NULL, 
                            EDUC_ESPECIAL = 'N',
                            MAIS_EDUCACAO = 'N',
                            MATRICULA = @MATRICULA
                    WHERE   ALUNO = @ALUNO
                            AND DISCIPLINA = @DISCIPLINA
                            AND TURMA = @TURMA
                            AND ANO = @ANO
                            AND SEMESTRE = @SEMESTRE",
                    new ContextQueryParameter("@ALUNO", matricula.Aluno),
                    new ContextQueryParameter("@SIT_MATRICULA", Matriculado),
                    new ContextQueryParameter("@DISCIPLINA", matricula.Disciplina),
                    new ContextQueryParameter("@TURMA", matricula.Turma),
                    new ContextQueryParameter("@ANO", matricula.Ano),
                    new ContextQueryParameter("@MATRICULA", matricula.Matricula),
                    new ContextQueryParameter("@SEMESTRE", matricula.Semestre)));
        }

        public void AlteraSituacaoMatriculaParaCanceladoPor(string aluno, List<ContextQuery> listaContextQueries)
        {
            ContextQuery contextQuery = new ContextQuery(
                    @"UPDATE  LY_MATRICULA
                         SET  SIT_MATRICULA = 'Cancelado',
                              DT_ULTALT = GETDATE()
                      WHERE   ALUNO = @ALUNO",
                new ContextQueryParameter("@ALUNO", aluno));

            listaContextQueries.Add(contextQuery);
        }

        public void AtualizaSuspensao(DataContext contexto, int ano, int historicoSuspensaoId, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_MATRICULA 
                                        SET    SIT_MATRICULA = 'Cancelado', 
                                                DT_ULTALT = getdate(),
                                                MATRICULA = @USUARIOID
                                        from LY_MATRICULA M
	                                        INNER JOIN Turma.HISTORICOSUSPENSAO h ON h.ALUNO = M.ALUNO
                                        WHERE M.ANO = @ANO
	                                        AND SIT_MATRICULA = 'Matriculado'
	                                        and HISTORICOSUSPENSAOID = @HISTORICOSUSPENSAOID ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@HISTORICOSUSPENSAOID", SqlDbType.Int, historicoSuspensaoId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);

            contexto.ApplyModifications(contextQuery);
        }

        public static DataTable ListarProgressaoParcial(LyMatricula matricula)
        {
            var contextQuery = new ContextQuery(
                @" SELECT  m.DISCIPLINA_REFERENCIA, m.DISCIPLINA, d.NOME_COMPL AS NOME_DISCIPLINA, m.SIT_MATRICULA AS 'SITUACAO'
                            FROM    DBO.LY_MATRICULA m
                                    INNER JOIN dbo.LY_DISCIPLINA d ON m.DISCIPLINA_REFERENCIA = d.DISCIPLINA
                            WHERE   DEPENDENCIA = 'S'                            
                            AND ANO = @ANO
                            AND SEMESTRE = @SEMESTRE
                            AND ALUNO = @ALUNO
                            AND TURMA = @TURMA");

            contextQuery.Parameters.Add("@ANO", matricula.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", matricula.Semestre);
            contextQuery.Parameters.Add("@ALUNO", matricula.Aluno);
            contextQuery.Parameters.Add("@TURMA", matricula.Turma);

            return Consultar(contextQuery);
        }

        public static DataTable ListarProgressaoParcial(string aluno)
        {
            var contextQuery = new ContextQuery(
                @" SELECT  M.ALUNO, M.ANO, M.TURMA, M.SEMESTRE, T.TURNO, TU.DESCRICAO AS DSC_TURNO,
		                M.DISCIPLINA, M.DISCIPLINA_REFERENCIA, M.SERIE_REFERENCIA,
                        ( D1.DISCIPLINA + ' - ' + D1.NOME_COMPL ) AS DSC_DISCIPLINA,
                        ( D2.DISCIPLINA + ' | ' + D2.NOME_COMPL ) AS DSC_DISCIPLINA_REFERENCIA,
                         '' NOVATURMA
                FROM    DBO.LY_MATRICULA M
                        INNER JOIN DBO.LY_TURMA T ON T.TURMA = M.TURMA
                                                     AND T.ANO = M.ANO
                                                     AND T.SEMESTRE = M.SEMESTRE
                                                     AND T.DISCIPLINA = M.DISCIPLINA
                        INNER JOIN DBO.LY_DISCIPLINA D1 ON M.DISCIPLINA = D1.DISCIPLINA
                        LEFT JOIN DBO.LY_DISCIPLINA D2 ON M.DISCIPLINA_REFERENCIA = D2.DISCIPLINA
                        INNER JOIN DBO.LY_TURNO TU ON TU.TURNO = T.TURNO
                WHERE   M.DEPENDENCIA = 'S'                        
                        AND M.ALUNO = @ALUNO
                        AND M.SIT_MATRICULA = @SIT_MATRICULA ");
            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);

            return Consultar(contextQuery);
        }

        public static ValidacaoDados ValidarProgressaoParcial(LyMatricula progressao)
        {
            var mensagens = new List<string>();
            LyTurma turmaDependencia = new LyTurma();
            Turma rnTurma = new Turma();
            RN.HistMatricula rnHistMatricula = new HistMatricula();
            RN.ProgressaoSerie rnProgressaoSerie = new RN.ProgressaoSerie();
            RN.FechamentoMatricula rnFechamentoMatricula = new FechamentoMatricula();
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
                if (!PossuiMatriculaRegularAtiva(progressao.Ano, progressao.Aluno))
                {
                    mensagens.Add("Não foi encontrada matricula principal para este aluno.");
                }

                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    //verifica se a aluno tem matricula ativa em outro ano / periodo
                    var contextQuery = new ContextQuery(
                        @" SELECT  COUNT(*)
                        FROM    DBO.LY_MATRICULA M
                        WHERE   M.SIT_MATRICULA = @SIT_MATRICULA        
                                AND M.ALUNO = @ALUNO
                                AND (M.ANO <> @ANO
                                or M.SEMESTRE <> @SEMESTRE) ");

                    contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
                    contextQuery.Parameters.Add("@ANO", progressao.Ano);
                    contextQuery.Parameters.Add("@SEMESTRE", progressao.Semestre);
                    contextQuery.Parameters.Add("@ALUNO", progressao.Aluno);

                    var matriculas = ctx.GetReturnValue<int>(contextQuery);

                    if (matriculas > 0)
                    {
                        mensagens.Add("Já existe matricula ATIVA para este aluno em outro ano / periodo!");
                    }

                    //verifica se a aluno já está matriculado naquele ano / semestre / turma /disciplina
                    contextQuery = new ContextQuery(
                        @" SELECT  COUNT(*)
                                FROM    DBO.LY_MATRICULA M
                                WHERE   M.SIT_MATRICULA = @SIT_MATRICULA
                                        AND M.ANO = @ANO
                                        AND M.SEMESTRE = @SEMESTRE
                                        AND M.ALUNO = @ALUNO
                                        AND M.DISCIPLINA = @DISCIPLINA
                                        AND M.TURMA = @TURMA ");

                    contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
                    contextQuery.Parameters.Add("@ANO", progressao.Ano);
                    contextQuery.Parameters.Add("@SEMESTRE", progressao.Semestre);
                    contextQuery.Parameters.Add("@ALUNO", progressao.Aluno);
                    contextQuery.Parameters.Add("@DISCIPLINA", progressao.Disciplina);
                    contextQuery.Parameters.Add("@TURMA", progressao.Turma);

                    int matriculaExistente = ctx.GetReturnValue<int>(contextQuery);

                    if (matriculaExistente > 0)
                    {
                        mensagens.Add("Já existe matricula para este aluno nesta turma / disciplina!");
                    }

                    //verifica se a disciplina de referencia já existe para aquele aluno
                    contextQuery = new ContextQuery(
                        @" SELECT  COUNT(*)
                        FROM    DBO.LY_MATRICULA M
                        WHERE   M.SIT_MATRICULA = @SIT_MATRICULA
                                AND M.ANO = @ANO
                                AND M.SEMESTRE = @SEMESTRE
                                AND M.ALUNO = @ALUNO
                                AND M.DISCIPLINA_REFERENCIA = @DISCIPLINA_REFERENCIA
                                ");

                    contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
                    contextQuery.Parameters.Add("@ANO", progressao.Ano);
                    contextQuery.Parameters.Add("@SEMESTRE", progressao.Semestre);
                    contextQuery.Parameters.Add("@ALUNO", progressao.Aluno);
                    contextQuery.Parameters.Add("@DISCIPLINA_REFERENCIA", progressao.DisciplinaReferencia);

                    var disciplinasRef = ctx.GetReturnValue<int>(contextQuery);

                    if (disciplinasRef > 0)
                    {
                        mensagens.Add(
                            "Já existe cadastrada esta mesma DISCIPLINA DE REFERÊNCIA para este ano / periodo!");
                    }

                    //verifica se o aluno já foi aprovado na disciplina de referencia
                    contextQuery = new ContextQuery(
                        @" SELECT COUNT(*)
                            FROM    DBO.LY_HISTMATRICULA H (NOLOCK)
                            WHERE   H.SITUACAO_HIST = @SITUACAO_HIST
		                            AND H.DEPENDENCIA = 'S'
                                    AND H.ALUNO = @ALUNO
                                    AND H.DISCIPLINA_REFERENCIA = @DISCIPLINA_REFERENCIA ");

                    contextQuery.Parameters.Add("@SITUACAO_HIST", "Aprovado");
                    contextQuery.Parameters.Add("@ALUNO", progressao.Aluno);
                    contextQuery.Parameters.Add("@DISCIPLINA_REFERENCIA", progressao.DisciplinaReferencia);

                    var disciplinasRefAprovada = ctx.GetReturnValue<int>(contextQuery);

                    if (disciplinasRefAprovada > 0)
                    {
                        mensagens.Add("O aluno já foi aprovado nesta DISCIPLINA DE REFERÊNCIA!");
                    }
                    //verifica se a disciplina existe na turma
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
                    contextQuery.Parameters.Add("@TURMA", progressao.Turma);
                    contextQuery.Parameters.Add("@DISCIPLINA", progressao.Disciplina);

                    var disciplinas = ctx.GetReturnValue<int>(contextQuery);

                    if (disciplinas <= 0)
                    {
                        mensagens.Add("A TURMA DESTINO não possui a disciplina para este ano / semestre");
                    }

                    //Verifica se a disciplina possui opção de lançamento de notas
                    contextQuery = new ContextQuery(
                        @" SELECT COUNT(*) FROM DBO.LY_DISCIPLINA                      
                                   WHERE TEM_NOTA = 'S'
										 AND DISCIPLINA = @DISCIPLINA ");

                    contextQuery.Parameters.Add("@DISCIPLINA", progressao.Disciplina);

                    var possuiNota = ctx.GetReturnValue<int>(contextQuery);

                    if (possuiNota <= 0)
                    {
                        mensagens.Add("A DISCIPLINA escolhida não possui opção de lançamento de notas.");
                    }

                    //verificar a serie, curso e turma atual do aluno
                    contextQuery = new ContextQuery(
                        @" SELECT TOP 1
                                ISNULL(SERIE, 0) AS SERIE, M.TURMA, t.CURSO
                        FROM    DBO.LY_MATRICULA M
                                INNER JOIN DBO.LY_TURMA T ON M.DISCIPLINA = T.DISCIPLINA
                                                             AND M.ANO = T.ANO
                                                             AND M.TURMA = T.TURMA
                                                             AND M.SEMESTRE = T.SEMESTRE
                        WHERE   M.SIT_MATRICULA = @SIT_MATRICULA
                                AND M.ANO = @ANO
                                AND M.ALUNO = @ALUNO 
                                AND ( M.DEPENDENCIA IS NULL
                                      OR M.DEPENDENCIA = 'N'
                                    )
                                AND ( M.CONCOMITANTE IS NULL
                                      OR M.CONCOMITANTE = 'N'
                                    )
                                AND ( M.EDUC_ESPECIAL IS NULL
                                      OR M.EDUC_ESPECIAL = 'N'
                                    )
                                AND ( M.MAIS_EDUCACAO IS NULL
                                      OR M.MAIS_EDUCACAO = 'N'
                                    )
                                AND T.OPTATIVAREFORCO = 'N'
                                AND ISNULL(T.ELETIVA,'N') = 'N'
                        ORDER BY SERIE DESC ");

                    contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
                    contextQuery.Parameters.Add("@ANO", progressao.Ano);
                    contextQuery.Parameters.Add("@SEMESTRE", progressao.Semestre);
                    contextQuery.Parameters.Add("@ALUNO", progressao.Aluno);

                    var serie = 0;
                    var turma = string.Empty;
                    var curso = string.Empty;
                    using (var reader = ctx.GetDataReader(contextQuery))
                    {
                        while (reader.Read())
                        {
                            turma = Convert.ToString(reader["TURMA"]);
                            serie = Convert.ToInt32(reader["SERIE"]);
                            curso = Convert.ToString(reader["CURSO"]);
                        }
                    }

                    if (serie <= 0)
                    {
                        //se o aluno não estiver com matricula ativa, verifica a serie na tabela de ly_aluno
                        contextQuery = new ContextQuery(
                            @" SELECT  ISNULL(MAX(SERIE), 0) AS SERIE, CURSO
                            FROM    LY_ALUNO A
                            WHERE   A.ANO_INGRESSO = @ANO
                                    AND A.SEM_INGRESSO = @SEMESTRE
                                    AND A.ALUNO = @ALUNO
                                    GROUP BY CURSO ");

                        contextQuery.Parameters.Add("@ANO", progressao.Ano);
                        contextQuery.Parameters.Add("@SEMESTRE", progressao.Semestre);
                        contextQuery.Parameters.Add("@ALUNO", progressao.Aluno);

                        serie = 0;
                        curso = string.Empty;

                        using (var reader = ctx.GetDataReader(contextQuery))
                        {
                            while (reader.Read())
                            {
                                serie = Convert.ToInt32(reader["SERIE"]);
                                curso = Convert.ToString(reader["CURSO"]);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(turma) && progressao.Turma == turma)
                    {
                        mensagens.Add("A TURMA da progressão não pode ser a mesma turma do aluno.");
                    }
                    else
                    {
                        //Verifica a mesma turma / disciplina já foi finalizada anteriormente
                        if (rnHistMatricula.EhMatriculaHistoricoAtivaPor(progressao.Aluno, Convert.ToInt32(progressao.Ano), Convert.ToInt32(progressao.Semestre), progressao.Turma, progressao.Disciplina))
                        {
                            mensagens.Add("Já existe matricula ativa no histórico para este aluno / ano / periodo / turma / disciplina.");
                        }
                    }

                    //carrega dados da turma de dependencia do aluno
                    turmaDependencia = rnTurma.CarregaTurmaDependenciaPor(progressao.Aluno, Convert.ToInt32(progressao.SerieReferencia), progressao.DisciplinaReferencia);

                    if (curso == turmaDependencia.Curso)
                    {
                        //Caso o aluno continue no msm curso verificar se serie referencia é menor
                        if (progressao.SerieReferencia > 0 && progressao.SerieReferencia > serie)
                        {
                            mensagens.Add("A SÉRIE REFERÊNCIA deve ser menor que a serie atual do aluno!");
                        }
                    }
                    else
                    {
                        //Verifica se o curso da turma escolhida é algum curso que progride do curso da dependencia, 
                        //em caso positivo a serie será considerada maior
                        if (!rnProgressaoSerie.EhCursoProximoCurso(turmaDependencia.Curso, curso))
                        {
                            mensagens.Add("Não existe Progressão de Série cadastrada do CURSO/SÉRIE REFERÊNCIA para o curso/série atual do aluno!");
                        }
                    }

                    //Busca quantidade de progressões que o aluno está cumprindo
                    int qtdeDepAtuais = ObtemQuantidadeProgressaoParcialPor(progressao.Aluno, progressao.Ano, progressao.Semestre);

                    //Por padrão quantidade máxima é 2
                    int qtdemaxDep = 2; 
                    
                    if (DateTime.Now.Date <= Convert.ToDateTime("2026-12-23"))
                    {
                        //Chamado - ID 82368
                        //Serão considerados, excepcionalmente nos próximos três anos, como aptos à progressão parcial, os estudantes do Ensino Médio que apresentem retenções nos seguintes limites:
                        //I -1ª e 2ª séries: até 6 (seis) retenções em componentes curriculares distintos;
                        //II -3ª série: até 3 (três) retenções em componentes curriculares distintos."
                        //Solicitamos que inicialmente seja registrado o seguinte período de vigência do ajuste em tela:
                        //Data de início: 07/04/2026
                        //Data final: 23/12/2026 (conforme término letivo no Calendário Escolar 2026).

                        //Busca quantidade máxima de dependencia para o curso / serie
                        qtdemaxDep = rnFechamentoMatricula.ObtemDependenciasPermitidasPor(turmaDependencia.Curso, Convert.ToInt32(turmaDependencia.Serie));

                        if (qtdemaxDep == 0)
                        {
                            qtdemaxDep = 2;
                        }
                    }

                    if (qtdeDepAtuais >= qtdemaxDep)
                    {
                        mensagens.Add("Já existem " + qtdemaxDep.ToString() + " progressões cadastradas. Favor verificar!");
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

        public static void InserirProgressaoParcial(LyMatricula matricula)
        {
            using (var context = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
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
                                AND SIT_MATRICULA = @SIT_MATRICULA ");

                    contextQuery.Parameters.Add("@ALUNO", matricula.Aluno);
                    contextQuery.Parameters.Add("@DISCIPLINA", matricula.Disciplina);
                    contextQuery.Parameters.Add("@TURMA", matricula.Turma);
                    contextQuery.Parameters.Add("@ANO", matricula.Ano);
                    contextQuery.Parameters.Add("@SEMESTRE", matricula.Semestre);
                    contextQuery.Parameters.Add("@SIT_MATRICULA", Cancelado);

                    var idCancelada = context.GetReturnValue<int>(contextQuery);

                    //se existir atualiza senao insere
                    if (idCancelada > 0)
                    {
                        Matricula.AtualizarProgressaoParcial(context, matricula);
                    }
                    else
                    {
                        Matricula.InserirProgressaoParcial(context, matricula);
                    }

                    //carregar dados da turma
                    var lyTurma = RN.Turma.Carregar(Convert.ToInt32(matricula.Ano),
                                                    Convert.ToInt32(matricula.Semestre),
                                                    matricula.Turma);

                    //obter grade_id
                    var gradeId = GradeSerie.ObterGradeId(
                        context,
                        matricula.Ano,
                        matricula.Semestre,
                        lyTurma.Curso,
                        lyTurma.Curriculo,
                        lyTurma.Serie,
                        matricula.Turma);

                    //Obter dados e atualizar matgrade
                    context.ApplyModifications(
                        new ContextQuery(
                            string.Format(
                                @"DECLARE @aluno T_CODIGO,
                                                        @grade_id T_NUMERO_GRANDE,
                                                        @sit_matgrade T_SIT_MATGRADE	
                                                                                    		
                                                    SET @aluno = '{0}'
                                                    SET @grade_id = {1}
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
                                                                ) ",
                                matricula.Aluno,
                                gradeId)));
                }
                catch (Exception)
                {
                    context.Abandon();
                    throw;
                }
            }
        }

        public static void InserirProgressaoParcial(DataContext ctx, LyMatricula matricula)
        {
            ctx.ApplyModifications(
                new ContextQuery(
                    @" INSERT  INTO dbo.LY_MATRICULA ( ALUNO, DISCIPLINA, TURMA, ANO, SEMESTRE,
                                                        SIT_MATRICULA, DT_ULTALT, COBRANCA_SEP,
                                                        DT_INSERCAO, DT_MATRICULA, DEPENDENCIA, 
                                                        SERIE_REFERENCIA, DISCIPLINA_REFERENCIA,
                                                        MATRICULA, DT_CADASTRO )
                        VALUES  ( @ALUNO, @DISCIPLINA, @TURMA, @ANO, @SEMESTRE, @SIT_MATRICULA,
                                  GETDATE(), 'N', GETDATE(), GETDATE(), 'S',
                                  @SERIE_REFERENCIA, @DISCIPLINA_REFERENCIA, @MATRICULA, GETDATE() ) ",
                    new ContextQueryParameter("@ALUNO", matricula.Aluno),
                    new ContextQueryParameter("@DISCIPLINA", matricula.Disciplina),
                    new ContextQueryParameter("@TURMA", matricula.Turma),
                    new ContextQueryParameter("@ANO", matricula.Ano),
                    new ContextQueryParameter("@SEMESTRE", matricula.Semestre),
                    new ContextQueryParameter("@SIT_MATRICULA", Matriculado),
                    new ContextQueryParameter("@SERIE_REFERENCIA", matricula.SerieReferencia),
                    new ContextQueryParameter("@DISCIPLINA_REFERENCIA", matricula.DisciplinaReferencia),
                    new ContextQueryParameter("@MATRICULA", matricula.Matricula)));
        }

        public static void AtualizarProgressaoParcial(DataContext ctx, LyMatricula matricula)
        {
            ctx.ApplyModifications(
                new ContextQuery(
                    @" UPDATE  LY_MATRICULA
                        SET     SIT_MATRICULA = @SIT_MATRICULA, 
		                        STAMP_ATUALIZACAO = GETDATE(),
                                MATRICULA = @MATRICULA, 
		                        CONCOMITANTE = 'N',
                                DEPENDENCIA = 'S',
                                EDUC_ESPECIAL = 'N',
                                MAIS_EDUCACAO = 'N',
                                SERIE_REFERENCIA = @SERIE_REFERENCIA,
                                DISCIPLINA_REFERENCIA = @DISCIPLINA_REFERENCIA
                        WHERE   ALUNO = @ALUNO
                                AND DISCIPLINA = @DISCIPLINA
                                AND TURMA = @TURMA
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE ",
                    new ContextQueryParameter("@SIT_MATRICULA", Matriculado),
                    new ContextQueryParameter("@MATRICULA", matricula.Matricula),
                    new ContextQueryParameter("@SERIE_REFERENCIA", matricula.SerieReferencia),
                    new ContextQueryParameter("@DISCIPLINA_REFERENCIA", matricula.DisciplinaReferencia),
                    new ContextQueryParameter("@ALUNO", matricula.Aluno),
                    new ContextQueryParameter("@DISCIPLINA", matricula.Disciplina),
                    new ContextQueryParameter("@TURMA", matricula.Turma),
                    new ContextQueryParameter("@ANO", matricula.Ano),
                    new ContextQueryParameter("@SEMESTRE", matricula.Semestre)));
        }

        public static void RemoverProgressaoParcial(LyMatricula matricula)
        {
            using (var context = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    Matricula.RemoverProgressaoParcial(context, matricula);

                    //carregar dados da turma
                    var lyTurma = RN.Turma.Carregar(Convert.ToInt32(matricula.Ano),
                                                    Convert.ToInt32(matricula.Semestre),
                                                    matricula.Turma);
                    //obter grade_id
                    var gradeId = GradeSerie.ObterGradeId(
                        context,
                        matricula.Ano,
                        matricula.Semestre,
                        lyTurma.Curso,
                        lyTurma.Curriculo,
                        lyTurma.Serie,
                        matricula.Turma);

                    RN.Matgrade.Remover(context, matricula.Aluno, gradeId);
                }
                catch (Exception)
                {
                    context.Abandon();
                    throw;
                }
            }
        }

        public static void RemoverProgressaoParcial(DataContext context, LyMatricula matricula)
        {
            var contextQuery = new ContextQuery(
                @" UPDATE  LY_MATRICULA
                SET     SIT_MATRICULA = @SIT_MATRICULA, 
		                STAMP_ATUALIZACAO = GETDATE(),
                        DT_ULTALT = GETDATE(),
                        MATRICULA = @MATRICULA                        
                WHERE   ALUNO = @ALUNO
                        AND DISCIPLINA = @DISCIPLINA
                        AND TURMA = @TURMA
                        AND ANO = @ANO
                        AND SEMESTRE = @SEMESTRE
                        AND DEPENDENCIA = 'S' ");

            contextQuery.Parameters.Add("@SIT_MATRICULA", Cancelado);
            contextQuery.Parameters.Add("@MATRICULA", matricula.Matricula);
            contextQuery.Parameters.Add("@ALUNO", matricula.Aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", matricula.Disciplina);
            contextQuery.Parameters.Add("@TURMA", matricula.Turma);
            contextQuery.Parameters.Add("@ANO", matricula.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", matricula.Semestre);

            context.ApplyModifications(contextQuery);
        }

        public static DadosEnsProfConcomitante CarregarEnsProfConcomitante(string aluno, int ano, int periodo, string usuario)
        {
            var dados = new DadosEnsProfConcomitante
            {
                ExibirLiberacao = false,
                ExibirEnturmacao = false
            };

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                //Verifica se a aluno pode ser liberado para concomitante
                var contextQuery = new ContextQuery
                {
                    Command =
                        @" SELECT TOP 1 1
                                FROM    ly_matricula m
                                        INNER JOIN ly_turma t ON m.TURMA = t.TURMA
                                                                 AND m.ano = t.ANO
                                                                 AND m.SEMESTRE = t.SEMESTRE
                                                                 AND m.DISCIPLINA = t.DISCIPLINA
                                        INNER JOIN dbo.LY_CURSO c ON t.CURSO = c.CURSO
                                        INNER JOIN USUARIO u ON u.USUARIO = @USUARIO
                                                                AND ( EXISTS ( SELECT TOP 1
                                                                                        UNIDADE_FIS
                                                                               FROM     LY_USUARIO_UNIDADE_FIS usuuni
                                                                                        WITH ( NOLOCK )
                                                                               WHERE    usuuni.UNIDADE_FIS = t.FACULDADE
                                                                                        AND usuuni.USUARIO = u.USUARIO
                                                                                        AND u.PRIVIL <> 'S' )
                                                                      OR ( U.PRIVIL = 'S' )
                                                                    )
                                WHERE   m.sit_matricula = @SIT_MATRICULA
                                        AND c.TIPO = '3'
                                        AND c.MODALIDADE = 'RE1'
                                        AND m.aluno = @ALUNO
                                        AND ( M.DEPENDENCIA IS NULL
                                              OR M.DEPENDENCIA = 'N'
                                            )
                                        AND ( M.CONCOMITANTE IS NULL
                                              OR M.CONCOMITANTE = 'N'
                                            )
                                        AND ( M.EDUC_ESPECIAL IS NULL
                                              OR M.EDUC_ESPECIAL = 'N'
                                            )
                                        AND ( M.MAIS_EDUCACAO IS NULL
                                              OR M.MAIS_EDUCACAO = 'N'
                                            )
                                        AND t.OPTATIVAREFORCO = 'N'
                                        AND ISNULL(T.ELETIVA,'N') = 'N'
                                        AND M.ANO = @ANO
                                        AND M.SEMESTRE = @PERIODO "
                };

                contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
                contextQuery.Parameters.Add("@USUARIO", usuario);
                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    if (reader.Read())
                    {
                        dados.ExibirLiberacao = true;
                    }
                }

                //Carrega dados das liberações anteriores
                var dadosLiberacao = AlunoConcomitante.Carregar(aluno, ano, periodo);
                dados.Status = dadosLiberacao.Status;
                dados.Censo = dadosLiberacao.Censo;

                if (dadosLiberacao.IdAlunoConcomitante > 0 && dadosLiberacao.Status == AlunoConcomitante.Liberado)
                {
                    //Verifica dados da enturmação do aluno
                    contextQuery = new ContextQuery
                    {
                        Command =
                            @" SELECT  DISTINCT TOP 1
                                            AC.ALUNO ,
                                            AC.CENSO ,
                                            UE.NOME_COMP ,
                                            AC.ANO ,
                                            AC.PERIODO ,
                                            M.CURSO ,
                                            M.SERIE ,
                                            M.TURNO ,
                                            TU.DESCRICAO ,
                                            M.TURMA ,
                                            AC.DT_CADASTRO ,
                                            AC.STATUS
                                    FROM    DBO.TCE_ALUNO_CONCOMITANTE AC
                                            INNER JOIN DBO.LY_UNIDADE_ENSINO UE ON AC.CENSO = UE.UNIDADE_ENS
                                            LEFT JOIN ( SELECT  M.ALUNO ,
                                                                M.ANO ,
                                                                M.SEMESTRE ,
                                                                T.CURSO ,
                                                                T.SERIE ,
                                                                T.TURNO ,
                                                                T.TURMA ,
                                                                T.FACULDADE
                                                        FROM    DBO.LY_MATRICULA M
                                                                INNER  JOIN DBO.LY_TURMA T ON M.TURMA = T.TURMA
                                                                                              AND M.ANO = T.ANO
                                                                                              AND M.SEMESTRE = T.SEMESTRE
                                                        WHERE   M.SIT_MATRICULA = @SIT_MATRICULA
                                                                AND M.CONCOMITANTE = 'S'
                                                      ) AS M ON AC.ALUNO = M.ALUNO
                                                                AND AC.ANO = M.ANO
                                                                AND AC.CENSO = M.FACULDADE
                                            LEFT JOIN DBO.LY_TURNO TU ON M.TURNO = TU.TURNO
                                    WHERE   AC.ID_ALUNO_CONCOMITANTE = @ID_ALUNO_CONCOMITANTE "
                    };
                    contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
                    contextQuery.Parameters.Add("@ID_ALUNO_CONCOMITANTE", dadosLiberacao.IdAlunoConcomitante);

                    using (var reader = ctx.GetDataReader(contextQuery))
                    {
                        if (reader.Read())
                        {
                            dados.ExibirEnturmacao = true;
                            dados.Ano = Convert.ToInt32(reader["ANO"]);
                            dados.Periodo = Convert.ToInt32(reader["PERIODO"]);
                            dados.NomeUnidadeEnsino = Convert.ToString(reader["NOME_COMP"]);
                            dados.Curso = Convert.ToString(reader["CURSO"]);
                            dados.Turno = Convert.ToString(reader["TURNO"]);
                            dados.Turma = Convert.ToString(reader["TURMA"]);
                            dados.NomeTurno = Convert.ToString(reader["DESCRICAO"]);
                            dados.Enturmado = !string.IsNullOrEmpty(dados.Turma);
                            if (reader["SERIE"] != DBNull.Value)
                            {
                                dados.Serie = Convert.ToInt32(reader["SERIE"]);
                            }
                        }
                    }
                }
            }

            return dados;
        }

        public static LyMatricula CarregarMatriculaRegular(string aluno, int ano, int periodo)
        {
            var matricula = new LyMatricula();

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                //Verifica dados da enturmação concomitante do aluno
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT  DISTINCT
                                    M.ALUNO ,
                                    M.ANO ,
                                    M.SEMESTRE ,
                                    M.TURMA
                            FROM    DBO.LY_MATRICULA M
                                    INNER JOIN LY_TURMA T ON M.DISCIPLINA = T.DISCIPLINA
                                                             AND M.TURMA = T.TURMA
                                                             AND M.ANO = T.ANO
                                                             AND M.SEMESTRE = T.SEMESTRE
                            WHERE   M.ANO = @ANO
                                    AND M.SEMESTRE = @SEMESTRE
                                    AND ( M.DEPENDENCIA IS NULL
                                          OR M.DEPENDENCIA = 'N'
                                        )
                                    AND ( M.CONCOMITANTE IS NULL
                                          OR M.CONCOMITANTE = 'N'
                                        )
                                    AND ( M.EDUC_ESPECIAL IS NULL
                                          OR M.EDUC_ESPECIAL = 'N'
                                        )
                                    AND ( M.MAIS_EDUCACAO IS NULL
                                          OR M.MAIS_EDUCACAO = 'N'
                                        )
                                    AND t.OPTATIVAREFORCO = 'N'
                                    AND ISNULL(T.ELETIVA,'N') = 'N'
                                    AND M.ALUNO = @ALUNO
                                    AND M.SIT_MATRICULA = @SIT_MATRICULA "
                };
                contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    while (reader.Read())
                    {
                        matricula.Aluno = Convert.ToString(reader["ALUNO"]);
                        matricula.Ano = Convert.ToInt32(reader["ANO"]);
                        matricula.Semestre = Convert.ToInt32(reader["SEMESTRE"]);
                        matricula.Turma = Convert.ToString(reader["TURMA"]);
                    }
                }
            }

            return matricula;
        }

        public static LyMatricula CarregarMatriculaAtiva(string aluno)
        {
            var matricula = new LyMatricula();

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                //Verifica dados da enturmação concomitante do aluno
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT  DISTINCT
                                    M.ALUNO ,
                                    M.ANO ,
                                    M.SEMESTRE ,
                                    M.TURMA
                            FROM    DBO.LY_MATRICULA M
                                    INNER JOIN LY_TURMA T ON M.DISCIPLINA = T.DISCIPLINA
                                                             AND M.TURMA = T.TURMA
                                                             AND M.ANO = T.ANO
                                                             AND M.SEMESTRE = T.SEMESTRE
                            WHERE   ( M.DEPENDENCIA IS NULL
                                          OR M.DEPENDENCIA = 'N'
                                        )
                                    AND ( M.CONCOMITANTE IS NULL
                                          OR M.CONCOMITANTE = 'N'
                                        )
                                    AND ( M.EDUC_ESPECIAL IS NULL
                                          OR M.EDUC_ESPECIAL = 'N'
                                        )
                                    AND ( M.MAIS_EDUCACAO IS NULL
                                          OR M.MAIS_EDUCACAO = 'N'
                                        )
                                    AND t.OPTATIVAREFORCO = 'N'
                                    AND ISNULL(T.ELETIVA,'N') = 'N'
                                    AND M.ALUNO = @ALUNO
                                    AND M.SIT_MATRICULA = @SIT_MATRICULA "
                };
                contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
                contextQuery.Parameters.Add("@ALUNO", aluno);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    while (reader.Read())
                    {
                        matricula.Aluno = Convert.ToString(reader["ALUNO"]);
                        matricula.Ano = Convert.ToInt32(reader["ANO"]);
                        matricula.Semestre = Convert.ToInt32(reader["SEMESTRE"]);
                        matricula.Turma = Convert.ToString(reader["TURMA"]);
                    }
                }
            }

            return matricula;
        }

        /// <summary>
        /// /// Verifica se a Matrícula é Regular.
        /// </summary>
        /// <param name="aluno"></param>
        /// <param name="ano"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public bool EhMatriculaRegular(string aluno, int ano, int periodo)
        {
            bool ehMatriculaRegular;

            ContextQuery contextQuery = new ContextQuery(
                     @" SELECT  COUNT(*)
                        FROM    DBO.LY_MATRICULA M
                                INNER JOIN LY_TURMA T ON M.DISCIPLINA = T.DISCIPLINA
                                                         AND M.TURMA = T.TURMA
                                                         AND M.ANO = T.ANO
                                                         AND M.SEMESTRE = T.SEMESTRE
                        WHERE   M.ANO = @ANO
                                AND M.SEMESTRE = @SEMESTRE
                                AND ( M.DEPENDENCIA IS NULL
                                      OR M.DEPENDENCIA = 'N'
                                    )
                                AND ( M.CONCOMITANTE IS NULL
                                      OR M.CONCOMITANTE = 'N'
                                    )
                                AND ( M.EDUC_ESPECIAL IS NULL
                                      OR M.EDUC_ESPECIAL = 'N'
                                    )
                                AND ( M.MAIS_EDUCACAO IS NULL
                                      OR M.MAIS_EDUCACAO = 'N'
                                    )
                                AND T.OPTATIVAREFORCO = 'N'
                                AND ISNULL(T.ELETIVA,'N') = 'N'
                                AND M.ALUNO = @ALUNO
                                AND M.SIT_MATRICULA = @SIT_MATRICULA ");

            contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", periodo);

            ehMatriculaRegular = (ExecutarFuncao<int>(contextQuery) > 0);

            return ehMatriculaRegular;
        }

        public bool EhMatriculaConcomitante(string aluno, int ano, int periodo, string turma)
        {
            bool ehMatriculaConcomitante;

            ContextQuery contextQuery = new ContextQuery(
                     @" SELECT  COUNT(*)
                        FROM    DBO.LY_MATRICULA M
                        WHERE   M.ANO = @ANO
                                AND M.SEMESTRE = @SEMESTRE
                                AND ISNULL(M.CONCOMITANTE,'N') = 'S'
                                AND M.ALUNO = @ALUNO
                                AND M.SIT_MATRICULA = @SIT_MATRICULA
                                AND M.TURMA = @TURMA ");

            contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", periodo);
            contextQuery.Parameters.Add("@TURMA", turma);

            ehMatriculaConcomitante = (ExecutarFuncao<int>(contextQuery) > 0);

            return ehMatriculaConcomitante;
        }

        public static LyMatricula CarregarMatriculaConcomitante(string aluno, int ano, int periodo)
        {
            var matricula = new LyMatricula();

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                //Verifica dados da enturmação concomitante do aluno
                var contextQuery = new ContextQuery
                {
                    Command =
                        @" SELECT  DISTINCT
                                    M.ALUNO, M.ANO, M.SEMESTRE, M.TURMA
                            FROM    DBO.LY_MATRICULA M
                            WHERE   M.ANO = @ANO
                                    AND M.SEMESTRE = @SEMESTRE
                                    AND M.CONCOMITANTE = 'S'
                                    AND M.ALUNO = @ALUNO
                                    AND M.SIT_MATRICULA = @SIT_MATRICULA "
                };
                contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    while (reader.Read())
                    {
                        matricula.Aluno = Convert.ToString(reader["ALUNO"]);
                        matricula.Ano = Convert.ToInt32(reader["ANO"]);
                        matricula.Semestre = Convert.ToInt32(reader["SEMESTRE"]);
                        matricula.Turma = Convert.ToString(reader["TURMA"]);
                    }
                }
            }

            return matricula;
        }

        public static ValidacaoDados ValidarEnsProfConcomitante(LyMatricula concomitante, LyMatricula turmaRegular, string turno, string turnoPrincipal)
        {
            var mensagens = new List<string>();
            RN.Aluno rnAluno = new Aluno();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (concomitante == null)
            {
                return validacaoDados;
            }

            concomitante.Concomitante = "S";

            if (string.IsNullOrEmpty(concomitante.Aluno))
            {
                mensagens.Add("O campo ALUNO é obrigatório!");
            }

            if (concomitante.Ano <= 0)
            {
                mensagens.Add("O campo ANO é obrigatório!");
            }

            if (concomitante.Semestre < 0)
            {
                mensagens.Add("O campo PERIODO é obrigatório!");
            }

            if (string.IsNullOrEmpty(concomitante.Turma))
            {
                mensagens.Add("O campo TURMA é obrigatório!");
            }

            if (string.IsNullOrEmpty(concomitante.Matricula))
            {
                mensagens.Add("O campo MATRICULA é obrigatório!");
            }

            if (mensagens.Count == 0)
            {
                if (!PossuiMatriculaRegularAtiva(concomitante.Ano, concomitante.Aluno))
                {
                    mensagens.Add("Não foi encontrada matricula principal para este aluno.");
                }

                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    //verifica se a aluno já tem matricula concomitante ativa
                    var contextQuery = new ContextQuery(
                        @" SELECT  COUNT(*)
                        FROM    DBO.LY_MATRICULA M
                        WHERE   M.SIT_MATRICULA = @SIT_MATRICULA        
                                AND M.ALUNO = @ALUNO                                
                                AND CONCOMITANTE = 'S' ");

                    contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
                    contextQuery.Parameters.Add("@ALUNO", concomitante.Aluno);

                    var matriculas = ctx.GetReturnValue<int>(contextQuery);

                    if (matriculas > 0)
                    {
                        mensagens.Add(
                            "Já existe matricula de ensino profissional concomitante ATIVA para este aluno em outro ano / periodo!");
                    }

                    //Verificar se o aluno possui turma em ensino medio (tipo = 3, modalidade = 'RE1') ativa no ano / semestre.
                    contextQuery = new ContextQuery(
                        @" SELECT  COUNT(*)
                        FROM    DBO.LY_MATRICULA M
                                INNER JOIN LY_TURMA T ON M.TURMA = T.TURMA
                                                         AND M.ANO = T.ANO
                                                         AND M.SEMESTRE = T.SEMESTRE
                                                         AND M.DISCIPLINA = T.DISCIPLINA
                                INNER JOIN LY_CURSO C ON T.CURSO = C.CURSO
                        WHERE   M.SIT_MATRICULA = @SIT_MATRICULA
                                AND M.ALUNO = @ALUNO
                                AND M.ANO = @ANO
                                AND M.SEMESTRE = @SEMESRE
                                AND ( M.DEPENDENCIA IS NULL
                                      OR M.DEPENDENCIA = 'N'
                                    )
                                AND ( M.CONCOMITANTE IS NULL
                                      OR M.CONCOMITANTE = 'N'
                                    )
                                AND ( M.EDUC_ESPECIAL IS NULL
                                      OR M.EDUC_ESPECIAL = 'N'
                                    )
                                AND ( M.MAIS_EDUCACAO IS NULL
                                      OR M.MAIS_EDUCACAO = 'N'
                                    )
                                AND C.TIPO = 3
                                AND C.MODALIDADE = 'RE1' ");

                    contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
                    contextQuery.Parameters.Add("@ALUNO", turmaRegular.Aluno);
                    contextQuery.Parameters.Add("@ANO", turmaRegular.Ano);
                    contextQuery.Parameters.Add("@SEMESRE", turmaRegular.Semestre);

                    matriculas = ctx.GetReturnValue<int>(contextQuery);

                    if (matriculas <= 0)
                    {
                        mensagens.Add("Este aluno não está matriculado no Ensino Médio Regular!");
                    }

                    //Verifica se a turma tem vaga
                    var vagas = RN.Turma.RetornaVagas(Convert.ToInt32(concomitante.Ano),
                                                      Convert.ToInt32(concomitante.Semestre), concomitante.Turma);
                    if (vagas <= 0)
                    {
                        mensagens.Add("A capacidade da turma desejada nao comporta mais alunos.");
                    }

                    //verifica se já existe historico de matricula para aquele aluno / turma / ano / periodo
                    contextQuery = new ContextQuery(
                        @" SELECT  COUNT(1)
                        FROM    LY_HISTMATRICULA
                        WHERE   ALUNO = @ALUNO
                                AND TURMA = @TURMA
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE ");

                    contextQuery.Parameters.Add("@ALUNO", concomitante.Aluno);
                    contextQuery.Parameters.Add("@TURMA", concomitante.Turma);
                    contextQuery.Parameters.Add("@ANO", concomitante.Ano);
                    contextQuery.Parameters.Add("@SEMESTRE", concomitante.Semestre);

                    var historicos = ctx.GetReturnValue<int>(contextQuery);

                    if (historicos > 0)
                    {
                        mensagens.Add("Aluno já possui histórico de matrícula em disciplina, turma, ano e semestre!");
                    }

                    if (!RN.Turno.VerificarContraTurno(turno, turnoPrincipal))
                    {
                        mensagens.Add("A Turma Profissional Concomitante deve estar em contraturno com a turma regular.");
                    }

                    //obter dados da turma concomitante
                    var turmaConcomitante = RN.Turma.Carregar(Convert.ToInt32(concomitante.Ano),
                                                    Convert.ToInt32(concomitante.Semestre),
                                                    concomitante.Turma);

                    //Consultar o Tipo de Curso
                    string tipoCurso = RN.Curso.ConsultarTipoProfCurso(turmaConcomitante.Curso);

                    //Se o Tipo de Curso for especial verificar se o aluno possui necessidades especial
                    if (tipoCurso == "Especial")
                    {
                        if (rnAluno.EhAlunoSemNecessidadeEspecialPor(concomitante.Aluno))
                        {
                            mensagens.Add("Para escolher um curso de educação especial, informar o tipo de necessidade especial do aluno(a) na aba 'Dados Pessoais'.");
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

        public static ValidacaoDados ValidarRemoverEnsProfConcomitante(LyMatricula concomitante)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (concomitante == null)
            {
                return validacaoDados;
            }

            concomitante.Concomitante = "S";

            if (string.IsNullOrEmpty(concomitante.Aluno))
            {
                mensagens.Add("O campo ALUNO é obrigatório!");
            }

            if (concomitante.Ano <= 0)
            {
                mensagens.Add("O campo ANO é obrigatório!");
            }

            if (concomitante.Semestre < 0)
            {
                mensagens.Add("O campo PERIODO é obrigatório!");
            }

            if (string.IsNullOrEmpty(concomitante.Turma))
            {
                mensagens.Add("O campo TURMA é obrigatório!");
            }

            if (string.IsNullOrEmpty(concomitante.Matricula))
            {
                mensagens.Add("O campo MATRICULA é obrigatório!");
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

        public static void InserirEnsProfConcomitante(LyMatricula concomitante)
        {
            using (var context = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    //Consultar Disciplinas da turma
                    var disciplinas = Matricula.ConsultaDisciplinaGrade(Convert.ToString(concomitante.Ano), Convert.ToString(concomitante.Semestre), concomitante.Turma);

                    foreach (DataRow disciplinaRow in disciplinas.Rows)
                    {
                        concomitante.Disciplina = disciplinaRow["disciplina"].ToString();

                        //verificar se ja existe um deles cancelado na ly_matricula
                        var contextQuery = new ContextQuery(
                            @" SELECT  COUNT(*)
                                FROM    LY_MATRICULA
                                WHERE   ALUNO = @ALUNO
                                        AND DISCIPLINA = @DISCIPLINA
                                        AND TURMA = @TURMA
                                        AND ANO = @ANO
                                        AND SEMESTRE = @SEMESTRE
                                        AND SIT_MATRICULA = @SIT_MATRICULA ");

                        contextQuery.Parameters.Add("@ALUNO", concomitante.Aluno);
                        contextQuery.Parameters.Add("@DISCIPLINA", concomitante.Disciplina);
                        contextQuery.Parameters.Add("@TURMA", concomitante.Turma);
                        contextQuery.Parameters.Add("@ANO", concomitante.Ano);
                        contextQuery.Parameters.Add("@SEMESTRE", concomitante.Semestre);
                        contextQuery.Parameters.Add("@SIT_MATRICULA", Cancelado);

                        var idCancelada = context.GetReturnValue<int>(contextQuery);

                        //se existir atualiza senao insere
                        if (idCancelada > 0)
                        {
                            Matricula.AtualizarEnsProfConcomitante(context, concomitante);
                        }
                        else
                        {
                            Matricula.InserirEnsProfConcomitante(context, concomitante);
                        }
                    }

                    //carregar dados da turma
                    var lyTurma = RN.Turma.Carregar(Convert.ToInt32(concomitante.Ano),
                                                    Convert.ToInt32(concomitante.Semestre),
                                                    concomitante.Turma);

                    //obter grade_id
                    var gradeId = GradeSerie.ObterGradeId(
                        context,
                        concomitante.Ano,
                        concomitante.Semestre,
                        lyTurma.Curso,
                        lyTurma.Curriculo,
                        lyTurma.Serie,
                        concomitante.Turma);

                    //Obter dados e atualizar matgrade
                    context.ApplyModifications(
                        new ContextQuery(
                            string.Format(
                                @"DECLARE @aluno T_CODIGO,
                                    @grade_id T_NUMERO_GRANDE,
                                    @sit_matgrade T_SIT_MATGRADE	
                                                                		
                                SET @aluno = '{0}'
                                SET @grade_id = {1}
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
                               ",
                                concomitante.Aluno,
                                gradeId)));

                    //INCLUI A INFORMAÇÃO DE TIPO ENS.PROFISSIONALIZANTE = Concomitante PARA O ALUNO
                    context.ApplyModifications(
                        new ContextQuery(
                            @" UPDATE  LY_ALUNO
                                SET     TIPO_ENSINO_PROFISSIONALIZANTE = 'Concomitante'
                                WHERE   ALUNO = @ALUNO ",
                            new ContextQueryParameter("@ALUNO", concomitante.Aluno)));
                    var liberacao = AlunoConcomitante.Carregar(concomitante.Aluno, Convert.ToInt32(concomitante.Ano));

                    if (!string.IsNullOrEmpty(liberacao.Status))
                    {
                        context.ApplyModifications(
                       new ContextQuery(
                           @" UPDATE  DBO.TCE_ALUNO_CONCOMITANTE
                                SET     PERIODO = @PERIODO
                                WHERE   ID_ALUNO_CONCOMITANTE = @ID ",
                           new ContextQueryParameter("@PERIODO", concomitante.Semestre),
                            new ContextQueryParameter("@ID", liberacao.IdAlunoConcomitante)));
                    }
                }
                catch (Exception)
                {
                    context.Abandon();
                    throw;
                }
            }
        }

        public static void InserirEnsProfConcomitante(DataContext ctx, LyMatricula concomitante)
        {
            ctx.ApplyModifications(
                new ContextQuery(
                    @" INSERT  INTO dbo.LY_MATRICULA ( ALUNO, DISCIPLINA, TURMA, ANO, SEMESTRE,
                                                        SIT_MATRICULA, DT_ULTALT, COBRANCA_SEP,
                                                        DT_INSERCAO, DT_MATRICULA, CONCOMITANTE,
                                                        MATRICULA, DT_CADASTRO )
                        VALUES  ( @ALUNO, @DISCIPLINA, @TURMA, @ANO, @SEMESTRE, 
			                        @SIT_MATRICULA, GETDATE(), 'N', 
			                        GETDATE(), GETDATE(), 'S', 
			                        @MATRICULA, GETDATE() ) ",
                    new ContextQueryParameter("@ALUNO", concomitante.Aluno),
                    new ContextQueryParameter("@DISCIPLINA", concomitante.Disciplina),
                    new ContextQueryParameter("@TURMA", concomitante.Turma),
                    new ContextQueryParameter("@ANO", concomitante.Ano),
                    new ContextQueryParameter("@SEMESTRE", concomitante.Semestre),
                    new ContextQueryParameter("@SIT_MATRICULA", Matriculado),
                    new ContextQueryParameter("@MATRICULA", concomitante.Matricula)));
        }

        public static void AtualizarEnsProfConcomitante(DataContext ctx, LyMatricula concomitante)
        {
            ctx.ApplyModifications(
                new ContextQuery(
                    @" UPDATE  LY_MATRICULA
                        SET     SIT_MATRICULA = @SIT_MATRICULA, 
		                        STAMP_ATUALIZACAO = GETDATE(),
                                MATRICULA = @MATRICULA,
                                CONCOMITANTE = 'S',
                                DEPENDENCIA = 'N',
                                SERIE_REFERENCIA = NULL, 
                                DISCIPLINA_REFERENCIA = NULL, 
                                EDUC_ESPECIAL = 'N',
                                MAIS_EDUCACAO = 'N'
                        WHERE   ALUNO = @ALUNO
                                AND DISCIPLINA = @DISCIPLINA
                                AND TURMA = @TURMA
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE ",
                    new ContextQueryParameter("@SIT_MATRICULA", Matriculado),
                    new ContextQueryParameter("@MATRICULA", concomitante.Matricula),
                    new ContextQueryParameter("@ALUNO", concomitante.Aluno),
                    new ContextQueryParameter("@DISCIPLINA", concomitante.Disciplina),
                    new ContextQueryParameter("@TURMA", concomitante.Turma),
                    new ContextQueryParameter("@ANO", concomitante.Ano),
                    new ContextQueryParameter("@SEMESTRE", concomitante.Semestre)));
        }

        public static string AnalisarChoqueHorario(LyMatricula novaMatricula)
        {
            var erro = string.Empty;
            RN.Matricula rnMatricula = new Matricula();

            ////carregar disciplinas matriculadas do aluno
            //var disciplinasDeOutrasTurmas = Matricula.ConsultaDisciplinaGradePorTurmas(Convert.ToString(novaMatricula.Aluno), Convert.ToString(novaMatricula.Ano), Convert.ToString(novaMatricula.Semestre));

            var disciplinasDeOutrasTurmas = rnMatricula.ObtemDisciplinaGradePor(Convert.ToInt32(novaMatricula.Ano), Convert.ToInt32(novaMatricula.Semestre), novaMatricula.Aluno);

            //carregar disciplinas da turma concomitante destino
            var disciplinasTurmaDestino = Matricula.ConsultaDisciplinaGrade(Convert.ToString(novaMatricula.Ano), Convert.ToString(novaMatricula.Semestre), novaMatricula.Turma);

            //cria lista de horarios
            var listaHorario = new List<TransferenciaTurma.Horarios>();

            //Adiciona na lista de horarios os horarios das disciplina regulares
            foreach (DataRow disciplinaRow in disciplinasDeOutrasTurmas.Rows)
            {
                List<TransferenciaTurma.Horarios> listaHorarioAux = TransferenciaTurma.CarregarHorarios(disciplinaRow["disciplina"].ToString(), disciplinaRow["TURMA"].ToString(), Convert.ToString(novaMatricula.Ano), Convert.ToString(novaMatricula.Semestre));
                if (listaHorarioAux != null && listaHorarioAux.Count > 0)
                {
                    listaHorario.AddRange(listaHorarioAux);
                }
            }

            //Adiciona na lista de horarios os horarios das disciplina da turma destino
            foreach (DataRow disciplinaRow in disciplinasTurmaDestino.Rows)
            {
                novaMatricula.Disciplina = disciplinaRow["disciplina"].ToString();
                List<TransferenciaTurma.Horarios> listaHorarioAux = TransferenciaTurma.CarregarHorarios(novaMatricula.Disciplina, novaMatricula.Turma, Convert.ToString(novaMatricula.Ano), Convert.ToString(novaMatricula.Semestre));
                if (listaHorarioAux != null && listaHorarioAux.Count > 0)
                {
                    listaHorario.AddRange(listaHorarioAux);
                }
            }

            var valorRetorno = TransferenciaTurma.VerificarChoqueHorario(novaMatricula.Aluno, listaHorario);

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

        public static string AnalisarChoqueHorarioEducacaoEspecial(LyMatricula novaMatricula, List<Atendimento> disciplinasAtendimento)
        {
            var erro = string.Empty;

            ////carregar disciplinas matriculadas do aluno
            var disciplinasDeOutrasTurmas = Matricula.ConsultaDisciplinaGradePorTurmas(Convert.ToString(novaMatricula.Aluno), Convert.ToString(novaMatricula.Ano), Convert.ToString(novaMatricula.Semestre));

            //carregar disciplinas da turma educacao especial
            var disciplinasTurmaDestino = Matricula.ConsultaDadosTurmaEducacaoEspecial(Convert.ToString(novaMatricula.Ano), Convert.ToString(novaMatricula.Semestre), novaMatricula.Turma, disciplinasAtendimento);

            //cria lista de horarios
            var listaHorario = new List<TransferenciaTurma.Horarios>();

            //Adiciona na lista de horarios os horarios das disciplina regulares
            foreach (DataRow disciplinaRow in disciplinasDeOutrasTurmas.Rows)
            {
                List<TransferenciaTurma.Horarios> listaHorarioAux = TransferenciaTurma.CarregarHorarios(disciplinaRow["disciplina"].ToString(), disciplinaRow["TURMA"].ToString(), Convert.ToString(novaMatricula.Ano), Convert.ToString(novaMatricula.Semestre));
                if (listaHorarioAux != null && listaHorarioAux.Count > 0)
                {
                    listaHorario.AddRange(listaHorarioAux);
                }
            }

            //Adiciona na lista de horarios os horarios das disciplina da turma destino
            foreach (DataRow disciplinaRow in disciplinasTurmaDestino.Rows)
            {
                novaMatricula.Disciplina = disciplinaRow["disciplina"].ToString();
                List<TransferenciaTurma.Horarios> listaHorarioAux = TransferenciaTurma.CarregarHorarios(novaMatricula.Disciplina, novaMatricula.Turma, Convert.ToString(novaMatricula.Ano), Convert.ToString(novaMatricula.Semestre));
                if (listaHorarioAux != null && listaHorarioAux.Count > 0)
                {
                    listaHorario.AddRange(listaHorarioAux);
                }
            }

            var valorRetorno = TransferenciaTurma.VerificarChoqueHorario(novaMatricula.Aluno, listaHorario);

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

        public static void RemoverEnsProfConcomitante(LyMatricula matricula)
        {
            using (var context = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    Matricula.RemoverEnsProfConcomitante(context, matricula);

                    //carregar dados da turma
                    var lyTurma = RN.Turma.Carregar(Convert.ToInt32(matricula.Ano),
                                                    Convert.ToInt32(matricula.Semestre),
                                                    matricula.Turma);
                    //obter grade_id
                    var gradeId = GradeSerie.ObterGradeId(
                        context,
                        matricula.Ano,
                        matricula.Semestre,
                        lyTurma.Curso,
                        lyTurma.Curriculo,
                        lyTurma.Serie,
                        matricula.Turma);

                    RN.Matgrade.Remover(context, matricula.Aluno, gradeId);
                }
                catch (Exception)
                {
                    context.Abandon();
                    throw;
                }
            }
        }

        public static void RemoverEnsProfConcomitante(DataContext context, LyMatricula matricula)
        {
            var contextQuery = new ContextQuery(
                @" UPDATE  LY_MATRICULA
                    SET     SIT_MATRICULA = @SIT_MATRICULA, 
		                    STAMP_ATUALIZACAO = GETDATE(),
                            DT_ULTALT = GETDATE(), 
                            MATRICULA = @MATRICULA
                    WHERE   ALUNO = @ALUNO        
                            AND TURMA = @TURMA
                            AND ANO = @ANO
                            AND SEMESTRE = @SEMESTRE
                            AND CONCOMITANTE = 'S' ");

            contextQuery.Parameters.Add("@SIT_MATRICULA", Cancelado);
            contextQuery.Parameters.Add("@MATRICULA", matricula.Matricula);
            contextQuery.Parameters.Add("@ALUNO", matricula.Aluno);
            contextQuery.Parameters.Add("@TURMA", matricula.Turma);
            contextQuery.Parameters.Add("@ANO", matricula.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", matricula.Semestre);

            context.ApplyModifications(contextQuery);
        }

        public static DadosMaisEducacao CarregarDadosMaisEducacao(string aluno, int ano, int periodo, string usuario)
        {
            var dados = new DadosMaisEducacao
            {
                ExibirMaisEducacao = false,
                Enturmado = false,
                Aluno = aluno,
                Ano = ano,
                Periodo = periodo
            };

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery
                {
                    //Ensino Fundamental/Médio: tipo 1, 2 e 3
                    //Modalidades: EDUCAÇÃO INDÍGENA, INTEGRADO REGULAR E NORMAL
                    Command =
                        @" SELECT  COUNT(*)
                            FROM    LY_MATRICULA M
                                    INNER JOIN LY_TURMA T ON M.TURMA = T.TURMA
                                                             AND M.ANO = T.ANO
                                                             AND M.SEMESTRE = T.SEMESTRE
                                                             AND M.DISCIPLINA = T.DISCIPLINA
                                    INNER JOIN DBO.LY_CURSO C ON T.CURSO = C.CURSO
                                    INNER JOIN USUARIO U ON U.USUARIO = @USUARIO
                                                            AND ( EXISTS ( SELECT TOP 1
                                                                                    UNIDADE_FIS
                                                                           FROM     LY_USUARIO_UNIDADE_FIS usuuni
                                                                                    WITH ( NOLOCK )
                                                                           WHERE    usuuni.UNIDADE_FIS = t.FACULDADE
                                                                                    AND usuuni.USUARIO = u.USUARIO
                                                                                    AND u.PRIVIL <> 'S' )
                                                                  OR ( U.PRIVIL = 'S' )
                                                                )
                            WHERE   M.SIT_MATRICULA = @SIT_MATRICULA
                                    AND C.TIPO IN ( 1, 2, 3 )  
                                    AND C.MODALIDADE  in ('RE1','NO9', 'ED3', 'IN8')                                    
                                    AND M.ALUNO = @ALUNO
                                    AND M.ANO = @ANO
                                    AND M.SEMESTRE = @PERIODO
                                    AND ( M.DEPENDENCIA IS NULL
                                          OR M.DEPENDENCIA = 'N'
                                        )
                                    AND ( M.CONCOMITANTE IS NULL
                                          OR M.CONCOMITANTE = 'N'
                                        )
                                    AND ( M.EDUC_ESPECIAL IS NULL
                                          OR M.EDUC_ESPECIAL = 'N'
                                        )
                                    AND ( M.MAIS_EDUCACAO IS NULL
                                          OR M.MAIS_EDUCACAO = 'N'
                                        ) "
                };

                contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
                contextQuery.Parameters.Add("@USUARIO", usuario);
                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                var matriculas = ctx.GetReturnValue<int>(contextQuery);

                if (matriculas > 0)
                {
                    dados.ExibirMaisEducacao = true;
                }
                if (dados.ExibirMaisEducacao)
                {
                    contextQuery = new ContextQuery
                    {
                        Command =
                            @" SELECT DISTINCT
                                        M.ALUNO, E.UNIDADE_ENS AS CENSO, E.NOME_COMP AS NOME_ESCOLA, M.ANO,
                                        M.SEMESTRE, T.CURSO, C.NOME AS NOME_CURSO, T.SERIE, T.TURNO, TU.DESCRICAO AS NOME_TURNO,
                                        M.TURMA
                                FROM    LY_MATRICULA M
                                        INNER JOIN LY_TURMA T ON M.TURMA = T.TURMA
                                                                 AND M.ANO = T.ANO
                                                                 AND M.SEMESTRE = T.SEMESTRE
                                                                 AND M.DISCIPLINA = T.DISCIPLINA
                                        INNER JOIN DBO.LY_UNIDADE_ENSINO E ON T.FACULDADE = E.UNIDADE_ENS
                                        LEFT JOIN LY_CURSO C ON T.CURSO = C.CURSO
                                        INNER JOIN DBO.LY_TURNO TU ON T.TURNO = TU.TURNO
                                WHERE   M.ALUNO = @ALUNO
                                        AND M.SIT_MATRICULA = @SIT_MATRICULA
                                        AND M.MAIS_EDUCACAO = 'S'
                                        AND M.ANO = @ANO
                                        AND M.SEMESTRE = @SEMESTRE "
                    };

                    contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
                    contextQuery.Parameters.Add("@ALUNO", aluno);
                    contextQuery.Parameters.Add("@ANO", ano);
                    contextQuery.Parameters.Add("@SEMESTRE", periodo);

                    using (var reader = ctx.GetDataReader(contextQuery))
                    {
                        if (reader.Read())
                        {
                            dados.Enturmado = true;
                            dados.Censo = Convert.ToString(reader["CENSO"]);
                            dados.NomeUnidadeEnsino = Convert.ToString(reader["NOME_ESCOLA"]);
                            dados.Curso = Convert.ToString(reader["CURSO"]);
                            dados.NomeCurso = Convert.ToString(reader["NOME_CURSO"]);
                            dados.Turno = Convert.ToString(reader["TURNO"]);
                            dados.NomeTurno = Convert.ToString(reader["NOME_TURNO"]);
                            dados.Turma = Convert.ToString(reader["TURMA"]);

                            if (reader["ANO"] != DBNull.Value)
                            {
                                dados.Ano = Convert.ToInt32(reader["ANO"]);
                            }
                            if (reader["SEMESTRE"] != DBNull.Value)
                            {
                                dados.Periodo = Convert.ToInt32(reader["SEMESTRE"]);
                            }
                            if (reader["SERIE"] != DBNull.Value)
                            {
                                dados.Serie = Convert.ToInt32(reader["SERIE"]);
                            }
                        }
                    }
                }
            }

            return dados;
        }

        public static ValidacaoDados ValidarMaisEducacao(LyMatricula maisEducacao, LyMatricula matriculaRegular)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (matriculaRegular == null || maisEducacao == null)
            {
                return validacaoDados;
            }

            maisEducacao.MaisEducacao = "S";
            maisEducacao.SitMatricula = "Matriculado";

            if (string.IsNullOrEmpty(maisEducacao.Aluno) || string.IsNullOrEmpty(matriculaRegular.Aluno))
            {
                mensagens.Add("O campo ALUNO é obrigatório!");
            }

            if (string.IsNullOrEmpty(maisEducacao.Turma) || string.IsNullOrEmpty(matriculaRegular.Turma))
            {
                mensagens.Add("O campo Turma Mais Educacao é obrigatório!");
            }

            if (maisEducacao.Turma == matriculaRegular.Turma)
            {
                mensagens.Add("A turma de destino não pode ser a mesma turma de origem!");
            }

            if (maisEducacao.Ano <= 0 || matriculaRegular.Ano <= 0)
            {
                mensagens.Add("O campo Ano é obrigatório!");
            }

            if (maisEducacao.Semestre < 0 || matriculaRegular.Semestre < 0)
            {
                mensagens.Add("O campo Período é obrigatório!");
            }

            //Valida se periodo do aluno é 0 (periodo do mais educação)
            if (matriculaRegular.Semestre != 0)
            {
                mensagens.Add("O Período da turma principal do aluno deve ser igual a 0!");
            }

            if (string.IsNullOrEmpty(maisEducacao.Matricula) || string.IsNullOrEmpty(matriculaRegular.Matricula))
            {
                mensagens.Add("O campo MATRICULA é obrigatório!");
            }

            if (mensagens.Count == 0)
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    if (!PossuiMatriculaRegularAtiva(maisEducacao.Ano, maisEducacao.Aluno))
                    {
                        mensagens.Add("Não foi encontrada matricula principal para este aluno.");
                    }

                    //verifica se a aluno já tem matricula outra matricula Mais Educacao ou Educação especial ativa
                    var contextQuery = new ContextQuery(
                        @" SELECT  COUNT(*)
                            FROM    DBO.LY_MATRICULA M
                            WHERE   M.SIT_MATRICULA = @SIT_MATRICULA
                                    AND M.ALUNO = @ALUNO
                                    AND ( MAIS_EDUCACAO = 'S'
                                          OR EDUC_ESPECIAL = 'S'
                                        ) ");

                    contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
                    contextQuery.Parameters.Add("@ALUNO", maisEducacao.Aluno);

                    var matriculas = ctx.GetReturnValue<int>(contextQuery);

                    if (matriculas > 0)
                    {
                        mensagens.Add(
                            "Já existe matricula de Mais Educação e/ou Educação Especial ATIVA para este aluno em outro ano / periodo!");
                    }

                    //Verificar se o aluno possui turma em ensino fundamental/Medio (tipo = 1, 2 e 3) ativa no ano / semestre.
                    contextQuery = new ContextQuery(
                        @" SELECT  COUNT(*)
                        FROM    DBO.LY_MATRICULA M
                                INNER JOIN LY_TURMA T ON M.TURMA = T.TURMA
                                                         AND M.ANO = T.ANO
                                                         AND M.SEMESTRE = T.SEMESTRE
                                                         AND M.DISCIPLINA = T.DISCIPLINA
                                INNER JOIN LY_CURSO C ON T.CURSO = C.CURSO
                        WHERE   M.SIT_MATRICULA = @SIT_MATRICULA
                                AND M.ALUNO = @ALUNO
                                AND M.ANO = @ANO
                                AND M.SEMESTRE = @SEMESRE
                                AND ( M.DEPENDENCIA IS NULL
                                      OR M.DEPENDENCIA = 'N'
                                    )
                                AND ( M.CONCOMITANTE IS NULL
                                      OR M.CONCOMITANTE = 'N'
                                    )
                                AND ( M.EDUC_ESPECIAL IS NULL
                                      OR M.EDUC_ESPECIAL = 'N'
                                    )
                                AND ( M.MAIS_EDUCACAO IS NULL
                                      OR M.MAIS_EDUCACAO = 'N'
                                    )
                                AND C.TIPO in (1, 2, 3)
                                 ");

                    contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
                    contextQuery.Parameters.Add("@ALUNO", matriculaRegular.Aluno);
                    contextQuery.Parameters.Add("@ANO", matriculaRegular.Ano);
                    contextQuery.Parameters.Add("@SEMESRE", matriculaRegular.Semestre);

                    matriculas = ctx.GetReturnValue<int>(contextQuery);

                    if (matriculas <= 0)
                    {
                        mensagens.Add("Este aluno não está matriculado no Ensino Fundamental Regular!");
                    }

                    //verifica se já existe historico de matricula para aquele aluno / turma / ano / periodo
                    contextQuery = new ContextQuery(
                        @" SELECT  COUNT(1)
                        FROM    LY_HISTMATRICULA
                        WHERE   ALUNO = @ALUNO
                                AND TURMA = @TURMA
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE ");

                    contextQuery.Parameters.Add("@ALUNO", maisEducacao.Aluno);
                    contextQuery.Parameters.Add("@TURMA", maisEducacao.Turma);
                    contextQuery.Parameters.Add("@ANO", maisEducacao.Ano);
                    contextQuery.Parameters.Add("@SEMESTRE", maisEducacao.Semestre);

                    var historicos = ctx.GetReturnValue<int>(contextQuery);

                    if (historicos > 0)
                    {
                        mensagens.Add("Aluno já possui histórico de matrícula nesta turma, ano e semestre!");
                    }

                    //Verifica se a turma maisEducacao tem vaga
                    var vagas = RN.Turma.RetornaVagas(Convert.ToInt32(maisEducacao.Ano),
                                                      Convert.ToInt32(maisEducacao.Semestre), maisEducacao.Turma);
                    if (vagas <= 0)
                    {
                        mensagens.Add("A capacidade da turma desejada nao comporta mais alunos.");
                    }

                    //Analisa Choque de Horários
                    var retorno = Matricula.AnalisarChoqueHorario(maisEducacao);
                    if (!string.IsNullOrEmpty(retorno))
                    {
                        mensagens.Add("Enturmação cancelada na verificação do choque de horário.");
                    }

                    if (maisEducacao.Turma == matriculaRegular.Turma)
                    {
                        mensagens.Add("A TURMA não pode ser igual a turma regular do aluno.");
                    }

                    //Busca dados da turma maisEducacao
                    var turmaMaisEducacao = RN.Turma.Carregar(Convert.ToInt32(maisEducacao.Ano),
                                                              Convert.ToInt32(maisEducacao.Semestre), maisEducacao.Turma);
                    //Busca dados da turma Regular
                    var turmaRegular = RN.Turma.Carregar(Convert.ToInt32(matriculaRegular.Ano),
                                                         Convert.ToInt32(matriculaRegular.Semestre),
                                                         matriculaRegular.Turma);

                    //Retirado temporariamento a pedido da demanda 6887
                    ////Verificar se a turma Mais Educacao é contra-turno da Turma Regular
                    //if (!Turno.VerificarContraTurno(turmaMaisEducacao.Turno, turmaRegular.Turno))
                    //{
                    //    mensagens.Add("A turma Mais Educação deve estar em contraturno com a turma regular.");
                    //}
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

        public static ValidacaoDados ValidarRemoverMaisEducacao(LyMatricula maisEducacao)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (maisEducacao == null)
            {
                return validacaoDados;
            }

            maisEducacao.MaisEducacao = "S";

            if (string.IsNullOrEmpty(maisEducacao.Aluno))
            {
                mensagens.Add("O campo ALUNO é obrigatório!");
            }

            if (maisEducacao.Ano <= 0)
            {
                mensagens.Add("O campo ANO é obrigatório!");
            }

            if (maisEducacao.Semestre < 0)
            {
                mensagens.Add("O campo PERIODO é obrigatório!");
            }

            if (string.IsNullOrEmpty(maisEducacao.Turma))
            {
                mensagens.Add("O campo TURMA é obrigatório!");
            }

            if (string.IsNullOrEmpty(maisEducacao.Matricula))
            {
                mensagens.Add("O campo MATRICULA é obrigatório!");
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

        public static void InserirMaisEducacao(LyMatricula maisEducacao)
        {
            using (var context = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    //Consultar Disciplinas da turma
                    var disciplinas = Matricula.ConsultaDisciplinaGrade(Convert.ToString(maisEducacao.Ano), Convert.ToString(maisEducacao.Semestre), maisEducacao.Turma);

                    foreach (DataRow disciplinaRow in disciplinas.Rows)
                    {
                        maisEducacao.Disciplina = disciplinaRow["disciplina"].ToString();

                        //verificar se ja existe um deles cancelado na ly_matricula
                        var contextQuery = new ContextQuery(
                            @" SELECT  COUNT(*)
                                                    FROM    LY_MATRICULA
                                                    WHERE   ALUNO = @ALUNO
                                                            AND DISCIPLINA = @DISCIPLINA
                                                            AND TURMA = @TURMA
                                                            AND ANO = @ANO
                                                            AND SEMESTRE = @SEMESTRE
                                                            AND SIT_MATRICULA <> @SIT_MATRICULA ");

                        contextQuery.Parameters.Add("@ALUNO", maisEducacao.Aluno);
                        contextQuery.Parameters.Add("@DISCIPLINA", maisEducacao.Disciplina);
                        contextQuery.Parameters.Add("@TURMA", maisEducacao.Turma);
                        contextQuery.Parameters.Add("@ANO", maisEducacao.Ano);
                        contextQuery.Parameters.Add("@SEMESTRE", maisEducacao.Semestre);
                        contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);

                        var idCancelada = context.GetReturnValue<int>(contextQuery);

                        //se existir atualiza senao insere
                        if (idCancelada > 0)
                        {
                            Matricula.AtualizarMaisEducacao(context, maisEducacao);
                        }
                        else
                        {
                            Matricula.InserirMaisEducacao(context, maisEducacao);
                        }
                    }

                    //carregar dados da turma
                    var lyTurma = RN.Turma.Carregar(Convert.ToInt32(maisEducacao.Ano),
                                                    Convert.ToInt32(maisEducacao.Semestre),
                                                    maisEducacao.Turma);

                    //obter grade_id
                    var gradeId = GradeSerie.ObterGradeId(
                        context,
                        maisEducacao.Ano,
                        maisEducacao.Semestre,
                        lyTurma.Curso,
                        lyTurma.Curriculo,
                        lyTurma.Serie,
                        maisEducacao.Turma);

                    //Obter dados e atualizar matgrade
                    context.ApplyModifications(
                        new ContextQuery(
                            string.Format(
                                @"DECLARE @aluno T_CODIGO,
                                                        @grade_id T_NUMERO_GRANDE,
                                                        @sit_matgrade T_SIT_MATGRADE	
                                                                                    		
                                                    SET @aluno = '{0}'
                                                    SET @grade_id = {1}
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
                                                                ) ",
                                maisEducacao.Aluno,
                                gradeId)));
                }
                catch (Exception)
                {
                    context.Abandon();
                    throw;
                }
            }
        }

        public static void InserirMaisEducacao(DataContext ctx, LyMatricula maisEducacao)
        {
            ctx.ApplyModifications(
                new ContextQuery(
                    @" INSERT  INTO dbo.LY_MATRICULA ( ALUNO, DISCIPLINA, TURMA, ANO, SEMESTRE,
                                                        SIT_MATRICULA, DT_ULTALT, COBRANCA_SEP,
                                                        DT_INSERCAO, DT_MATRICULA, MAIS_EDUCACAO,
                                                        MATRICULA, DT_CADASTRO )
                        VALUES  ( @ALUNO, @DISCIPLINA, @TURMA, @ANO, @SEMESTRE, 
			                        @SIT_MATRICULA, GETDATE(), 'N', 
			                        GETDATE(), GETDATE(), 'S', 
			                        @MATRICULA, GETDATE() ) ",
                    new ContextQueryParameter("@ALUNO", maisEducacao.Aluno),
                    new ContextQueryParameter("@DISCIPLINA", maisEducacao.Disciplina),
                    new ContextQueryParameter("@TURMA", maisEducacao.Turma),
                    new ContextQueryParameter("@ANO", maisEducacao.Ano),
                    new ContextQueryParameter("@SEMESTRE", maisEducacao.Semestre),
                    new ContextQueryParameter("@SIT_MATRICULA", Matriculado),
                    new ContextQueryParameter("@MATRICULA", maisEducacao.Matricula)));
        }

        public static void AtualizarMaisEducacao(DataContext ctx, LyMatricula maisEducacao)
        {
            ctx.ApplyModifications(
                new ContextQuery(
                    @" UPDATE  LY_MATRICULA
                        SET     SIT_MATRICULA = @SIT_MATRICULA, 
		                        STAMP_ATUALIZACAO = GETDATE(),
                                MATRICULA = @MATRICULA, 
                                CONCOMITANTE = 'N',
                                DEPENDENCIA = 'N',
                                SERIE_REFERENCIA = NULL, 
                                DISCIPLINA_REFERENCIA = NULL, 
                                EDUC_ESPECIAL = 'N',
                                MAIS_EDUCACAO = 'S'                                
                        WHERE   ALUNO = @ALUNO
                                AND DISCIPLINA = @DISCIPLINA
                                AND TURMA = @TURMA
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE ",
                    new ContextQueryParameter("@SIT_MATRICULA", Matriculado),
                    new ContextQueryParameter("@MATRICULA", maisEducacao.Matricula),
                    new ContextQueryParameter("@ALUNO", maisEducacao.Aluno),
                    new ContextQueryParameter("@DISCIPLINA", maisEducacao.Disciplina),
                    new ContextQueryParameter("@TURMA", maisEducacao.Turma),
                    new ContextQueryParameter("@ANO", maisEducacao.Ano),
                    new ContextQueryParameter("@SEMESTRE", maisEducacao.Semestre)));
        }

        public static void RemoverMaisEducacao(LyMatricula matricula)
        {
            using (var context = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    Matricula.RemoverMaisEducacao(context, matricula);

                    //carregar dados da turma
                    var lyTurma = RN.Turma.Carregar(Convert.ToInt32(matricula.Ano),
                                                    Convert.ToInt32(matricula.Semestre),
                                                    matricula.Turma);
                    //obter grade_id
                    var gradeId = GradeSerie.ObterGradeId(
                        context,
                        matricula.Ano,
                        matricula.Semestre,
                        lyTurma.Curso,
                        lyTurma.Curriculo,
                        lyTurma.Serie,
                        matricula.Turma);

                    RN.Matgrade.Remover(context, matricula.Aluno, gradeId);
                }
                catch (Exception)
                {
                    context.Abandon();
                    throw;
                }
            }
        }

        public static void RemoverMaisEducacao(DataContext context, LyMatricula matricula)
        {
            var contextQuery = new ContextQuery(
                @" UPDATE  LY_MATRICULA
                    SET     SIT_MATRICULA = @SIT_MATRICULA, 
		                    STAMP_ATUALIZACAO = GETDATE(),
                            DT_ULTALT = GETDATE(), 
                            MATRICULA = @MATRICULA
                    WHERE   ALUNO = @ALUNO        
                            AND TURMA = @TURMA
                            AND ANO = @ANO
                            AND SEMESTRE = @SEMESTRE
                            AND MAIS_EDUCACAO = 'S' ");

            contextQuery.Parameters.Add("@SIT_MATRICULA", Cancelado);
            contextQuery.Parameters.Add("@MATRICULA", matricula.Matricula);
            contextQuery.Parameters.Add("@ALUNO", matricula.Aluno);
            contextQuery.Parameters.Add("@TURMA", matricula.Turma);
            contextQuery.Parameters.Add("@ANO", matricula.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", matricula.Semestre);

            context.ApplyModifications(contextQuery);
        }

        public DataTable ListaEnturmacaoEducacaoEspecialPor(string aluno, string curso)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT M.ALUNO+';'+ convert(varchar, M.ANO)+';'+  convert(varchar, M.SEMESTRE) +';'+ M.TURMA AS CHAVE,
			                                        TR.REGIONAL,lue.NOME_COMP AS ESCOLA,M.ALUNO, 
			                                        M.ANO, 
			                                        M.SEMESTRE, 
			                                        M.TURMA,  
                                                    TN.DESCRICAO TURNO,
			                                        P.NOME_COMPL,
			                                        P.CPF, 
			                                        M.SIT_MATRICULA
                                        FROM LY_MATRICULA M
	                                        INNER JOIN LY_TURMA T ON M.TURMA = T.TURMA
						                                        AND M.ANO = T.ANO
						                                        AND M.SEMESTRE = T.SEMESTRE
	                                        LEFT JOIN DBO.LY_AULA_DOCENTE A ON  A.TURMA = M.TURMA
											                                        AND A.DISCIPLINA = M.DISCIPLINA
                                                                                    AND A.ANO = m.ANO
                                                                                    AND A.SEMESTRE = m.SEMESTRE
											                                        AND A.NUM_FUNC NOT IN (SELECT D2.NUM_FUNC
														                                        FROM LY_DOCENTE D2
														                                        WHERE D2.MATRICULA IN ('00000000','11111111','22222222','44444444','66666666','88888888','55555551','55555555','99999999'))
                                                                                    AND A.DATA_INICIO <> DATA_FIM
                                                                                    AND ( A.DATA_FIM IS NULL
			                                                                              OR A.DATA_FIM > GETDATE()
			                                                                            )

	                                        LEFT JOIN LY_DOCENTE D ON A.NUM_FUNC = D.NUM_FUNC	
	                                        LEFT JOIN LY_PESSOA P ON D.PESSOA = P.PESSOA
                                            JOIN	LY_UNIDADE_ENSINO LUE (NOLOCK) ON LUE.UNIDADE_ENS = T.FACULDADE
                                            JOIN	TCE_REGIONAL TR (NOLOCK) ON TR.ID_REGIONAL = LUE.ID_REGIONAL
                                            JOIN	MUNICIPIO MU (NOLOCK) ON MU.CODIGO = LUE.MUNICIPIO
                                            JOIN    LY_TURNO TN ON TN.TURNO = T.TURNO
                                        WHERE CURSO = @CURSO
	                                        AND ISNULL(EDUC_ESPECIAL, 'N') = 'S'
	                                        AND ALUNO = @ALUNO
                                        ORDER BY SIT_MATRICULA DESC ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);

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

        public static DadosEducacaoEspecial CarregarDadosEducacaoEspecial(string aluno, int ano, int periodo, string usuario)
        {
            var dados = new DadosEducacaoEspecial
            {
                ExibirEducacaoEspecial = false,
                Enturmado = false,
                Aluno = aluno,
                Ano = ano,
                Periodo = periodo
            };

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery
                {
                    //Verifica se o aluno possui aceite para educacao especial, de acordo com permissao do usuario
                    Command =
                        @" SELECT TOP 1
                                    EE.ACEITE, EE.CENSO, E.NOME_COMP AS NOME_ESCOLA, U.USUARIO
                            FROM    DBO.TCE_ALUNO_EDUC_ESPECIAL EE
                                    LEFT JOIN DBO.LY_UNIDADE_ENSINO E ON EE.CENSO = E.UNIDADE_ENS
                                    LEFT JOIN USUARIO U ON U.USUARIO = @USUARIO
                                                           AND ( EXISTS ( SELECT TOP 1
                                                                                    USUUNI.UNIDADE_FIS
                                                                          FROM      LY_USUARIO_UNIDADE_FIS USUUNI
                                                                                    WITH ( NOLOCK )
                                                                                    INNER JOIN DBO.LY_UNIDADES_ASSOCIADAS UA ON USUUNI.UNIDADE_FIS = UA.UNIDADE_FIS
                                                                          WHERE     UA.UNIDADE_ENS = EE.CENSO
                                                                                    AND USUUNI.USUARIO = U.USUARIO
                                                                                    AND U.PRIVIL <> 'S' )
                                                                 OR ( U.PRIVIL = 'S' )
                                                               )
                            WHERE   EE.ALUNO = @ALUNO
                                    AND EE.ANO = @ANO
                                    AND EE.PERIODO = @PERIODO        
                            ORDER BY EE.DT_CADASTRO DESC "
                };

                contextQuery.Parameters.Add("@USUARIO", usuario);
                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    if (reader.Read())
                    {
                        dados.ExibirEducacaoEspecial = true;
                        dados.ExibirEnturmacao = reader["USUARIO"] != DBNull.Value;
                        dados.Aceite = Convert.ToBoolean(reader["ACEITE"]);
                        dados.Censo = Convert.ToString(reader["CENSO"]);
                        dados.NomeUnidadeEnsino = Convert.ToString(reader["NOME_ESCOLA"]);
                    }
                }

                if (dados.Aceite)
                {
                    contextQuery = new ContextQuery
                    {
                        //Verifica se o aluno tem enturmação ativa
                        Command =
                            @" SELECT  DISTINCT M.TURMA, T.CURSO, C.NOME AS NOME_CURSO, M.DISCIPLINA,
                                T.SERIE, T.TURNO, TU.DESCRICAO AS NOME_TURNO
                                FROM    DBO.LY_MATRICULA M
                                        INNER JOIN LY_TURMA T ON M.TURMA = T.TURMA
                                                                 AND M.ANO = T.ANO
                                                                 AND M.SEMESTRE = T.SEMESTRE
                                                                 AND M.DISCIPLINA = T.DISCIPLINA
                                        INNER JOIN LY_CURSO C ON C.CURSO = T.CURSO
                                        INNER JOIN LY_TURNO TU ON T.TURNO = TU.TURNO
                                WHERE   M.ALUNO = @ALUNO
                                        AND M.ANO = @ANO
                                        AND M.SEMESTRE = @SEMESTRE
                                        AND M.SIT_MATRICULA = @SIT_MATRICULA
                                        AND M.EDUC_ESPECIAL = 'S' "
                    };

                    contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
                    contextQuery.Parameters.Add("@ALUNO", aluno);
                    contextQuery.Parameters.Add("@ANO", ano);
                    contextQuery.Parameters.Add("@SEMESTRE", periodo);

                    using (var reader = ctx.GetDataReader(contextQuery))
                    {
                        if (reader.Read())
                        {
                            dados.Enturmado = true;
                            dados.Curso = Convert.ToString(reader["CURSO"]);
                            dados.NomeCurso = Convert.ToString(reader["NOME_CURSO"]);
                            dados.Turno = Convert.ToString(reader["TURNO"]);
                            dados.NomeTurno = Convert.ToString(reader["NOME_TURNO"]);
                            dados.Turma = Convert.ToString(reader["TURMA"]);

                            if (reader["SERIE"] != DBNull.Value)
                            {
                                dados.Serie = Convert.ToInt32(reader["SERIE"]);
                            }
                        }
                    }
                    if (dados.Enturmado)
                    {
                        dados.Atendimentos = new List<Atendimento>();

                        contextQuery = new ContextQuery
                        {
                            //Verifica os atendimentos (disciplinas) do aluno
                            Command =
                                @" SELECT  DISTINCT DISCIPLINA
                                FROM    DBO.LY_MATRICULA
                                WHERE   ALUNO = @ALUNO
                                        AND ANO = @ANO
                                        AND SEMESTRE = @SEMESTRE
                                        AND TURMA = @TURMA
                                        AND SIT_MATRICULA = @SIT_MATRICULA
                                        AND EDUC_ESPECIAL = 'S'  "
                        };

                        contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
                        contextQuery.Parameters.Add("@ALUNO", aluno);
                        contextQuery.Parameters.Add("@ANO", ano);
                        contextQuery.Parameters.Add("@Turma", dados.Turma);
                        contextQuery.Parameters.Add("@SEMESTRE", periodo);

                        using (var reader = ctx.GetDataReader(contextQuery))
                        {
                            while (reader.Read())
                            {
                                var disciplina = Convert.ToString(reader["DISCIPLINA"]);
                                var horario = Turma.RetornaHorarioAtendimento(dados.Ano, dados.Periodo, dados.Curso,
                                                                              disciplina, dados.Turma);

                                var atendimento = new Atendimento
                                {
                                    Disciplina = disciplina,
                                    Horario = horario
                                };

                                dados.Atendimentos.Add(atendimento);
                            }
                        }
                    }
                }
            }

            return dados;
        }

        public static DadosEducacaoEspecial CarregarDadosSalaRecurso(string aluno, int ano, int periodo, string usuario)
        {
            var dados = new DadosEducacaoEspecial
            {
                ExibirEducacaoEspecial = false,
                Enturmado = false,
                Aluno = aluno,
                Ano = ano,
                Periodo = periodo
            };

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery
                {
                    //Verifica se o aluno possui aceite para educacao especial, de acordo com permissao do usuario
                    Command =
                        @"SELECT 1 as ACEITE, @USUARIO AS USUARIO
                          FROM   [LYCEUM].[NecessidadeEspecial].[AVALIACAONAPES] N                                
		                  where N.TIPORECURSONECESSIDADEESPECIALID =4 
                            AND ALUNOID = @ALUNO 
                            AND NECESSITARECURSO = 1 "
                };

                contextQuery.Parameters.Add("@USUARIO", usuario);
                contextQuery.Parameters.Add("@ALUNO", aluno);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    if (reader.Read())
                    {
                        dados.ExibirEducacaoEspecial = true;
                        dados.ExibirEnturmacao = reader["USUARIO"] != DBNull.Value;
                        dados.Aceite = Convert.ToBoolean(reader["ACEITE"]);
                    }
                }

                if (dados.Aceite)
                {
                    contextQuery = new ContextQuery
                    {
                        //Verifica se o aluno tem enturmação ativa
                        Command =
                            @" SELECT  DISTINCT M.TURMA, T.CURSO, C.NOME AS NOME_CURSO, M.DISCIPLINA,
                                T.SERIE, T.TURNO, TU.DESCRICAO AS NOME_TURNO
                                FROM    DBO.LY_MATRICULA M
                                        INNER JOIN LY_TURMA T ON M.TURMA = T.TURMA
                                                                 AND M.ANO = T.ANO
                                                                 AND M.SEMESTRE = T.SEMESTRE
                                                                 AND M.DISCIPLINA = T.DISCIPLINA
                                        INNER JOIN LY_CURSO C ON C.CURSO = T.CURSO
                                        INNER JOIN LY_TURNO TU ON T.TURNO = TU.TURNO
                                WHERE   M.ALUNO = @ALUNO
                                        AND M.ANO = @ANO
                                        AND M.SEMESTRE = @SEMESTRE
                                        AND M.SIT_MATRICULA = @SIT_MATRICULA
                                        AND M.EDUC_ESPECIAL = 'S' 
                                        AND T.CURSO = '9999.91' "
                    };

                    contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
                    contextQuery.Parameters.Add("@ALUNO", aluno);
                    contextQuery.Parameters.Add("@ANO", ano);
                    contextQuery.Parameters.Add("@SEMESTRE", periodo);

                    using (var reader = ctx.GetDataReader(contextQuery))
                    {
                        if (reader.Read())
                        {
                            dados.Enturmado = true;
                            dados.Curso = Convert.ToString(reader["CURSO"]);
                            dados.NomeCurso = Convert.ToString(reader["NOME_CURSO"]);
                            dados.Turno = Convert.ToString(reader["TURNO"]);
                            dados.NomeTurno = Convert.ToString(reader["NOME_TURNO"]);
                            dados.Turma = Convert.ToString(reader["TURMA"]);

                            if (reader["SERIE"] != DBNull.Value)
                            {
                                dados.Serie = Convert.ToInt32(reader["SERIE"]);
                            }
                        }
                    }
                    if (dados.Enturmado)
                    {
                        dados.Atendimentos = new List<Atendimento>();

                        contextQuery = new ContextQuery
                        {
                            //Verifica os atendimentos (disciplinas) do aluno
                            Command =
                                @" SELECT  DISTINCT DISCIPLINA
                                FROM    DBO.LY_MATRICULA
                                WHERE   ALUNO = @ALUNO
                                        AND ANO = @ANO
                                        AND SEMESTRE = @SEMESTRE
                                        AND TURMA = @TURMA
                                        AND SIT_MATRICULA = @SIT_MATRICULA
                                        AND EDUC_ESPECIAL = 'S'  "
                        };

                        contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
                        contextQuery.Parameters.Add("@ALUNO", aluno);
                        contextQuery.Parameters.Add("@ANO", ano);
                        contextQuery.Parameters.Add("@Turma", dados.Turma);
                        contextQuery.Parameters.Add("@SEMESTRE", periodo);

                        using (var reader = ctx.GetDataReader(contextQuery))
                        {
                            while (reader.Read())
                            {
                                var disciplina = Convert.ToString(reader["DISCIPLINA"]);
                                var horario = Turma.RetornaHorarioAtendimento(dados.Ano, dados.Periodo, dados.Curso,
                                                                              disciplina, dados.Turma);

                                var atendimento = new Atendimento
                                {
                                    Disciplina = disciplina,
                                    Horario = horario
                                };

                                dados.Atendimentos.Add(atendimento);
                            }
                        }
                    }
                }
            }

            return dados;
        }

        public static ValidacaoDados ValidarEducacaoEspecial(LyMatricula educacaoEspecial, LyMatricula matriculaRegular, List<Atendimento> atendimentos, string curso)
        {
            RN.Matricula rnMatricula = new Matricula();
            RN.AulaDocente rnAulaDocente = new AulaDocente();
            RN.NecessidadeEspecial.AvaliacaoNapes rnAvaliacaoNapes = new Techne.Lyceum.RN.NecessidadeEspecial.AvaliacaoNapes();
            int tipoRecursoNecessidadeEspecialId = 0;
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (educacaoEspecial == null || matriculaRegular == null)
            {
                return validacaoDados;
            }

            educacaoEspecial.EducEspecial = "S";
            educacaoEspecial.SitMatricula = "Matriculado";

            if (string.IsNullOrEmpty(educacaoEspecial.Aluno) || string.IsNullOrEmpty(matriculaRegular.Aluno))
            {
                mensagens.Add("O campo ALUNO é obrigatório!");
            }

            if (string.IsNullOrEmpty(educacaoEspecial.Turma) || string.IsNullOrEmpty(matriculaRegular.Turma))
            {
                mensagens.Add("O campo Turma Educacao Especial é obrigatório!");
            }

            if (educacaoEspecial.Turma == matriculaRegular.Turma)
            {
                mensagens.Add("A turma de destino não pode ser a mesma turma de origem!");
            }

            if (educacaoEspecial.Ano <= 0 || matriculaRegular.Ano <= 0)
            {
                mensagens.Add("O campo Ano é obrigatório!");
            }

            if (educacaoEspecial.Semestre < 0 || matriculaRegular.Semestre < 0)
            {
                mensagens.Add("O campo Período é obrigatório!");
            }

            if (string.IsNullOrEmpty(educacaoEspecial.Matricula) || string.IsNullOrEmpty(matriculaRegular.Matricula))
            {
                mensagens.Add("O campo MATRICULA é obrigatório!");
            }

            if (mensagens.Count == 0)
            {
                if (!PossuiMatriculaRegularAtiva(educacaoEspecial.Ano, educacaoEspecial.Aluno))
                {
                    mensagens.Add("Não foi encontrada matricula principal para este aluno.");
                }

                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    //Busca dados da turma Destino
                    var turmaEducacaoEspecial = RN.Turma.Carregar(Convert.ToInt32(educacaoEspecial.Ano),
                                                         Convert.ToInt32(educacaoEspecial.Semestre), educacaoEspecial.Turma);
                    //Busca dados da turma Regular
                    var turmaRegular = RN.Turma.Carregar(Convert.ToInt32(matriculaRegular.Ano),
                                                         Convert.ToInt32(matriculaRegular.Semestre), matriculaRegular.Turma);

                    //Verifica a mesma turma / disciplina já foi finalizada anteriormente
                    if (rnMatricula.PossuiMatriculaEducacaoEspecialPor(ctx, educacaoEspecial.Aluno, curso, turmaRegular.Turno))
                    {
                        mensagens.Add("Já existe matricula Educação Especial ATIVA para este aluno.");
                    }

                    //Sala de Recursos APENAS no contra turno do aluno
                    if (curso == "9999.91")
                    {
                        //Verificar se a turma Educacao Especial é contra-turno da Turma Regular
                        if (!Turno.VerificarContraTurno(turmaEducacaoEspecial.Turno, turmaRegular.Turno))
                        {
                            mensagens.Add("A turma Educacao Especial deve estar em contraturno com a turma regular.");
                        }

                        string professor = rnAulaDocente.RetornaPrimeiroDocentesEmAulaPor(ctx, educacaoEspecial.Turma, educacaoEspecial.Ano, educacaoEspecial.Semestre);

                        if (professor.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add("Não há professor alocado para esta turma.");
                        }

                        tipoRecursoNecessidadeEspecialId = 4; //4 -Sala de Recursos
                    }
                    else if (curso == "9999.04")
                    {

                        if (turmaRegular.Turno != "I")
                        {
                            //Atendimento Especializado no mesmo horário q a matrícula ativa dele
                            if (turmaEducacaoEspecial.Turno != turmaRegular.Turno)
                            {
                                mensagens.Add("O atendimento de Educacao Especial deve estar no mesmo turno que a turma regular.");
                            }
                        }

                        //Verifica se o atendimento já tem 5 alunos
                        var matriculados = rnMatricula.ObtemQtdeAlunoMatriculadoEdEspecialPor(Convert.ToInt32(educacaoEspecial.Ano), Convert.ToInt32(educacaoEspecial.Semestre), educacaoEspecial.Turma);
                        if (matriculados >= 5)
                        {
                            mensagens.Add("Turma com capacidade máxima permitida. Favor enturmar na próxima turma disponível.");
                        }

                        tipoRecursoNecessidadeEspecialId = 5; //5 - Professor Articulador Pedagógico Educação Especial

                    }

                    //Verifica se a avaliação do napes esta vigente
                    if (!rnAvaliacaoNapes.PossuiAvaliacaoPositivaVigentePor(ctx, educacaoEspecial.Aluno, tipoRecursoNecessidadeEspecialId))
                    {
                        mensagens.Add("Este aluno não possui avaliação positiva vigente.");
                    }

                    //verifica se já existe historico de matricula para aquele aluno / turma / ano / periodo
                    var contextQuery = new ContextQuery(
                        @" SELECT  COUNT(1)
                                    FROM    LY_HISTMATRICULA
                                    WHERE   ALUNO = @ALUNO
                                            AND TURMA = @TURMA
                                            AND ANO = @ANO
                                            AND SEMESTRE = @SEMESTRE ");

                    contextQuery.Parameters.Add("@ALUNO", educacaoEspecial.Aluno);
                    contextQuery.Parameters.Add("@TURMA", educacaoEspecial.Turma);
                    contextQuery.Parameters.Add("@ANO", educacaoEspecial.Ano);
                    contextQuery.Parameters.Add("@SEMESTRE", educacaoEspecial.Semestre);

                    var historicos = ctx.GetReturnValue<int>(contextQuery);

                    if (historicos > 0)
                    {
                        mensagens.Add("Aluno já possui histórico de matrícula em disciplina, turma, ano e semestre!");
                    }
                }

                //Verifica se a turma Destino tem vaga
                var vagas = RN.Turma.RetornaVagas(Convert.ToInt32(educacaoEspecial.Ano), Convert.ToInt32(educacaoEspecial.Semestre), educacaoEspecial.Turma);
                if (vagas <= 0)
                {
                    mensagens.Add("A capacidade da turma desejada não comporta mais alunos.");
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

        public static ValidacaoDados ValidarRemoverEducacaoEspecial(LyMatricula educacaoEspecial)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (educacaoEspecial == null)
            {
                return validacaoDados;
            }

            educacaoEspecial.EducEspecial = "S";

            if (string.IsNullOrEmpty(educacaoEspecial.Aluno))
            {
                mensagens.Add("O campo ALUNO é obrigatório!");
            }

            if (educacaoEspecial.Ano <= 0)
            {
                mensagens.Add("O campo ANO é obrigatório!");
            }

            if (educacaoEspecial.Semestre < 0)
            {
                mensagens.Add("O campo PERIODO é obrigatório!");
            }

            if (string.IsNullOrEmpty(educacaoEspecial.Turma))
            {
                mensagens.Add("O campo TURMA é obrigatório!");
            }

            if (string.IsNullOrEmpty(educacaoEspecial.Matricula))
            {
                mensagens.Add("O campo MATRICULA é obrigatório!");
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

        public static void InserirEducacaoEspecial(LyMatricula educacaoEspecial, List<Atendimento> atendimentos)
        {
            using (var context = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    foreach (var atendimento in atendimentos)
                    {
                        educacaoEspecial.Disciplina = atendimento.Disciplina;

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

                        contextQuery.Parameters.Add("@ALUNO", educacaoEspecial.Aluno);
                        contextQuery.Parameters.Add("@DISCIPLINA", educacaoEspecial.Disciplina);
                        contextQuery.Parameters.Add("@TURMA", educacaoEspecial.Turma);
                        contextQuery.Parameters.Add("@ANO", educacaoEspecial.Ano);
                        contextQuery.Parameters.Add("@SEMESTRE", educacaoEspecial.Semestre);

                        var canceladas = context.GetReturnValue<int>(contextQuery);

                        //se existir atualiza senao insere
                        if (canceladas > 0)
                        {
                            Matricula.AtualizarEducacaoEspecial(context, educacaoEspecial);
                        }
                        else
                        {
                            Matricula.InserirEducacaoEspecial(context, educacaoEspecial);
                        }

                        //carregar dados da turma destino
                        var lyTurma = RN.Turma.Carregar(Convert.ToInt32(educacaoEspecial.Ano),
                                                        Convert.ToInt32(educacaoEspecial.Semestre),
                                                        educacaoEspecial.Turma);
                        //obter gradeid destino
                        var gradeId = GradeSerie.ObterGradeId(
                            context,
                            educacaoEspecial.Ano,
                            educacaoEspecial.Semestre,
                            lyTurma.Curso,
                            lyTurma.Curriculo,
                            lyTurma.Serie,
                            educacaoEspecial.Turma);

                        //Obter dados e atualizar matgrade
                        context.ApplyModifications(
                            new ContextQuery(
                                string.Format(
                                    @"DECLARE @aluno T_CODIGO,
                                    @grade_id T_NUMERO_GRANDE,                                    
                                    @sit_matgrade T_SIT_MATGRADE	
                                                                		
                                SET @aluno = '{0}'
                                SET @grade_id = {1}                                
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
                               ",
                                    educacaoEspecial.Aluno,
                                    gradeId)));
                    }
                }
                catch (Exception)
                {
                    context.Abandon();
                    throw;
                }
            }
        }

        public static void InserirEducacaoEspecial(DataContext ctx, LyMatricula educacaoEspecial)
        {
            ctx.ApplyModifications(
                new ContextQuery(
                    @" INSERT  INTO dbo.LY_MATRICULA ( ALUNO, DISCIPLINA, TURMA, ANO, SEMESTRE,
                                                        SIT_MATRICULA, DT_ULTALT, COBRANCA_SEP,
                                                        DT_INSERCAO, DT_MATRICULA, EDUC_ESPECIAL,
                                                        MATRICULA, DT_CADASTRO )
                        VALUES  ( @ALUNO, @DISCIPLINA, @TURMA, @ANO, @SEMESTRE, 
			                        @SIT_MATRICULA, GETDATE(), 'N', 
			                        GETDATE(), GETDATE(), 'S', 
			                        @MATRICULA, GETDATE() ) ",
                    new ContextQueryParameter("@ALUNO", educacaoEspecial.Aluno),
                    new ContextQueryParameter("@DISCIPLINA", educacaoEspecial.Disciplina),
                    new ContextQueryParameter("@TURMA", educacaoEspecial.Turma),
                    new ContextQueryParameter("@ANO", educacaoEspecial.Ano),
                    new ContextQueryParameter("@SEMESTRE", educacaoEspecial.Semestre),
                    new ContextQueryParameter("@SIT_MATRICULA", Matriculado),
                    new ContextQueryParameter("@MATRICULA", educacaoEspecial.Matricula)));
        }

        public static void AtualizarEducacaoEspecial(DataContext ctx, LyMatricula educacaoEspecial)
        {
            ctx.ApplyModifications(
                new ContextQuery(
                    @" UPDATE  LY_MATRICULA
                        SET     SIT_MATRICULA = @SIT_MATRICULA, 
		                        STAMP_ATUALIZACAO = GETDATE(),
                                MATRICULA = @MATRICULA, 
                                CONCOMITANTE = 'N',
                                DEPENDENCIA = 'N',
                                SERIE_REFERENCIA = NULL, 
                                DISCIPLINA_REFERENCIA = NULL, 
                                EDUC_ESPECIAL = 'S',
                                MAIS_EDUCACAO = 'N'                                
                        WHERE   ALUNO = @ALUNO
                                AND DISCIPLINA = @DISCIPLINA
                                AND TURMA = @TURMA
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE ",
                    new ContextQueryParameter("@SIT_MATRICULA", Matriculado),
                    new ContextQueryParameter("@MATRICULA", educacaoEspecial.Matricula),
                    new ContextQueryParameter("@ALUNO", educacaoEspecial.Aluno),
                    new ContextQueryParameter("@DISCIPLINA", educacaoEspecial.Disciplina),
                    new ContextQueryParameter("@TURMA", educacaoEspecial.Turma),
                    new ContextQueryParameter("@ANO", educacaoEspecial.Ano),
                    new ContextQueryParameter("@SEMESTRE", educacaoEspecial.Semestre)));
        }

        public static void RemoverEducacaoEspecial(LyMatricula matricula)
        {
            using (var context = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    RemoverEducacaoEspecial(context, matricula);

                    //carregar dados da turma
                    var lyTurma = Turma.Carregar(Convert.ToInt32(matricula.Ano),
                                                    Convert.ToInt32(matricula.Semestre),
                                                    matricula.Turma);
                    //obter grade_id
                    var gradeId = GradeSerie.ObterGradeId(
                        context,
                        matricula.Ano,
                        matricula.Semestre,
                        lyTurma.Curso,
                        lyTurma.Curriculo,
                        lyTurma.Serie,
                        matricula.Turma);

                    Matgrade.Remover(context, matricula.Aluno, gradeId);
                }
                catch (Exception)
                {
                    context.Abandon();
                    throw;
                }
            }
        }

        public static void RemoverEducacaoEspecial(DataContext context, LyMatricula matricula)
        {
            var contextQuery = new ContextQuery(
                @" UPDATE  LY_MATRICULA
                    SET     SIT_MATRICULA = @SIT_MATRICULA, 
		                    STAMP_ATUALIZACAO = GETDATE(),
                            DT_ULTALT = GETDATE(), 
                            MATRICULA = @MATRICULA
                    WHERE   ALUNO = @ALUNO        
                            AND TURMA = @TURMA
                            AND ANO = @ANO
                            AND SEMESTRE = @SEMESTRE
                            AND EDUC_ESPECIAL = 'S' ");

            contextQuery.Parameters.Add("@SIT_MATRICULA", Cancelado);
            contextQuery.Parameters.Add("@MATRICULA", matricula.Matricula);
            contextQuery.Parameters.Add("@ALUNO", matricula.Aluno);
            contextQuery.Parameters.Add("@TURMA", matricula.Turma);
            contextQuery.Parameters.Add("@ANO", matricula.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", matricula.Semestre);

            context.ApplyModifications(contextQuery);
        }

        public static int RetornaUltimoPeriodoConcomitante(string aluno, int ano)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                var periodo = -1;
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT  DISTINCT
                                    SEMESTRE
                            FROM    DBO.LY_MATRICULA
                            WHERE   ALUNO = @ALUNO
                                    AND ANO = @ANO
                                    AND CONCOMITANTE = 'S' "
                };
                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    if (reader.Read())
                    {
                        periodo = Convert.ToInt32(reader["SEMESTRE"]);
                    }
                }
                return periodo;
            }
        }

        //        public static void RecuperarMatricula(string aluno, string gradeId)
        //        {
        //            var contextQuery = new ContextQuery(
        //                @" INSERT  INTO DBO.LY_MATRICULA
        //                                    ( ALUNO ,
        //                                      DISCIPLINA ,
        //                                      TURMA ,
        //                                      ANO ,
        //                                      SEMESTRE ,
        //                                      SIT_MATRICULA ,
        //                                      DT_ULTALT ,
        //                                      COBRANCA_SEP ,
        //                                      DT_INSERCAO ,
        //                                      DT_MATRICULA                            
        //                                    )
        //                                    ( SELECT DISTINCT
        //                                            m.ALUNO ,
        //                                            m.DISCIPLINA ,
        //                                            m.TURMA ,
        //                                            m.ANO ,
        //                                            m.SEMESTRE ,
        //                                            m.SIT_MATRICULA ,
        //                                            m.DT_ULTALT ,
        //                                            'N' ,
        //                                            DT_INSERCAO ,
        //                                            DT_MATRICULA
        //                                    FROM    LY_MATRICULA_BKP m
        //                                            INNER JOIN dbo.LY_GRADE_SERIE gs ON gs.GRADE = m.TURMA
        //                                                                                AND gs.ANO = m.ANO
        //                                                                                AND gs.SEMESTRE = m.SEMESTRE
        //                                            INNER JOIN dbo.LY_MATGRADE g ON gs.GRADE_ID = g.GRADE_ID
        //                                    WHERE   m.ALUNO = @aluno                                            
        //                                            AND m.SIT_MATRICULA = 'Matriculado'
        //                                            AND m.DT_ULTALT = ( SELECT  MAX(DT_ULTALT)
        //                                                                FROM    LY_MATRICULA_BKP
        //                                                                WHERE   ALUNO = m.ALUNO
        //                                                              )
        //                                            AND g.SIT_MATGRADE = 'Matriculado'
        //                                            AND g.GRADE_ID = @gradeId ) ");

        //            contextQuery.Parameters.Add("@aluno", aluno);
        //            contextQuery.Parameters.Add("@gradeId", gradeId);

        //            ExecutarAlteracao(contextQuery);
        //        }

        public DataTable ListaMatriculaAtivaOptativaReforcoPor(string aluno)
        {
            DataTable matriculasOptativaReforco = null;
            ContextQuery contextQuery = new ContextQuery();

            if (!string.IsNullOrEmpty(aluno))
            {
                try
                {
                    contextQuery.Command = @" SELECT M.ALUNO, M.ANO ,
                                M.SEMESTRE ,
                                M.TURMA ,
                                D.NOME , 
                                CONVERT(VARCHAR(10), M.DT_MATRICULA, 103) as DT_MATRICULA , 
                                '' NOVATURMA
                        FROM    DBO.LY_MATRICULA M ( NOLOCK )
                                INNER JOIN DBO.LY_TURMA T ( NOLOCK ) ON T.TURMA = M.TURMA
                                                                        AND T.ANO = M.ANO
                                                                        AND T.SEMESTRE = M.SEMESTRE
                                                                        AND T.DISCIPLINA = M.DISCIPLINA
                                INNER JOIN DBO.LY_DISCIPLINA D ( NOLOCK ) ON M.DISCIPLINA = D.DISCIPLINA 
                        WHERE   M.SIT_MATRICULA = 'MATRICULADO'
                                AND NOT ( T.OPTATIVAREFORCO = 'N' )
                                AND ALUNO = @ALUNO ";

                    contextQuery.Parameters.Add("@ALUNO", aluno);

                    matriculasOptativaReforco = Consultar(contextQuery);
                }
                catch (Exception exception)
                {
                    string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine, Convert.ToString(exception.Message));
                    throw new Exception(mensagem);
                }
            }

            return matriculasOptativaReforco;
        }
        public DataTable ListaMatriculaAtivaOptativaReforcoPorProjetoFoco(string aluno)
        {
            DataTable matriculasOptativaReforco = null;
            ContextQuery contextQuery = new ContextQuery();

            if (!string.IsNullOrEmpty(aluno))
            {
                try
                {
                    contextQuery.Command = @" SELECT M.ALUNO, M.ANO ,
                                M.SEMESTRE ,
                                M.TURMA ,
                                D.NOME , 
                                T.TURNO,
                                CONVERT(VARCHAR(10), M.DT_MATRICULA, 103) as DT_MATRICULA , 
                                '' NOVATURMA, iif(M.MAIS_EDUCACAO ='P', 'PRESENCIAL','VIRTUAL') AS MAISEDUCACAO
                        FROM    DBO.LY_MATRICULA M ( NOLOCK )
                                INNER JOIN DBO.LY_TURMA T ( NOLOCK ) ON T.TURMA = M.TURMA
                                                                        AND T.ANO = M.ANO
                                                                        AND T.SEMESTRE = M.SEMESTRE
                                                                        AND T.DISCIPLINA = M.DISCIPLINA
                                INNER JOIN DBO.LY_DISCIPLINA D ( NOLOCK ) ON M.DISCIPLINA = D.DISCIPLINA 
                        WHERE   M.SIT_MATRICULA = 'MATRICULADO'
                                AND ISNULL(M.MAIS_EDUCACAO, 'N') IN ('P', 'V')
                                AND ALUNO = @ALUNO ";

                    contextQuery.Parameters.Add("@ALUNO", aluno);

                    matriculasOptativaReforco = Consultar(contextQuery);
                }
                catch (Exception exception)
                {
                    string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine, Convert.ToString(exception.Message));
                    throw new Exception(mensagem);
                }
            }

            return matriculasOptativaReforco;
        }


        public ValidacaoDados ValidaMatriculaOptativaReforcoProjetoFoco(LyMatricula matriculaOptativaReforco)
        {
            List<string> mensagens = new List<string>();
            string retorno = string.Empty;
            string tipo = string.Empty;
            Turma rnTurma = new Turma();
            AulaDocente rnAulaDocente = new AulaDocente();

            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (matriculaOptativaReforco != null)
            {
                matriculaOptativaReforco.SitMatricula = Matriculado;

                if (string.IsNullOrEmpty(matriculaOptativaReforco.Aluno))
                {
                    mensagens.Add("O aluno deve ser informado.");
                }

                if (matriculaOptativaReforco.Ano <= 0)
                {
                    mensagens.Add("O campo Ano é obrigatório!");
                }

                if (matriculaOptativaReforco.Semestre < 0)
                {
                    mensagens.Add("O campo Período é obrigatório!");
                }

                if (matriculaOptativaReforco.Turma.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("O campo Turma é obrigatório!");
                }

                if (matriculaOptativaReforco.Matricula.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("A matrícula do responsável é obrigatória!");
                }

                if (mensagens.Count == 0)
                {
                    if (!PossuiMatriculaRegularAtiva(matriculaOptativaReforco.Ano, matriculaOptativaReforco.Aluno))
                    {
                        mensagens.Add("Não foi encontrada matricula principal para este aluno.");
                    }

                    matriculaOptativaReforco.Disciplina = rnTurma.ObtemDisciplinaOptativaProjetoFoco(matriculaOptativaReforco.Ano, matriculaOptativaReforco.Semestre, matriculaOptativaReforco.Turma);

                    if (string.IsNullOrEmpty(matriculaOptativaReforco.Disciplina))
                    {
                        mensagens.Add("Não foi encontrada disciplina para esta turma.");
                    }

                    retorno = Matricula.AnalisarChoqueHorario(matriculaOptativaReforco);

                    if (!string.IsNullOrEmpty(retorno))
                    {
                        mensagens.Add("Encontrado choque de horário para esta aluno/turma.");
                    }

                    using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                    {
                        //verifica se a aluno já tem matricula outra matricula ativa para a turma
                        var contextQuery = new ContextQuery(
                            @" SELECT  COUNT(*)
                            FROM    DBO.LY_MATRICULA M
                            WHERE   M.SIT_MATRICULA = @SIT_MATRICULA
                                    AND M.ALUNO = @ALUNO
                                    AND DISCIPLINA = @DISCIPLINA 
                                    AND ANO = @ANO
                                    AND SEMESTRE = @SEMESTRE
                                    AND TURMA = @TURMA");

                        contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
                        contextQuery.Parameters.Add("@ALUNO", matriculaOptativaReforco.Aluno);
                        contextQuery.Parameters.Add("@DISCIPLINA", matriculaOptativaReforco.Disciplina);
                        contextQuery.Parameters.Add("@ANO", matriculaOptativaReforco.Ano);
                        contextQuery.Parameters.Add("@SEMESTRE", matriculaOptativaReforco.Semestre);
                        contextQuery.Parameters.Add("@TURMA", matriculaOptativaReforco.Turma);

                        var matriculas = ctx.GetReturnValue<int>(contextQuery);

                        if (matriculas > 0)
                        {
                            mensagens.Add("Já existe matricula ATIVA para este aluno!");
                        }


                        string disciplina = string.Empty;

                        var contextQuery2 = new ContextQuery(
                            @" SELECT  COUNT(*)
                            FROM    DBO.LY_MATRICULA M
                            WHERE   M.SIT_MATRICULA = @SIT_MATRICULA
                                    AND M.ALUNO = @ALUNO                                    
                                    AND ANO = @ANO
                                    AND SEMESTRE = @SEMESTRE
                                    ");

                        if (matriculaOptativaReforco.Disciplina == "FOCO_MAT_4_PRESENC" || matriculaOptativaReforco.Disciplina == "FOCO_MAT_6_REMOTO")
                        {
                            contextQuery2.Command += @" AND DISCIPLINA  in ('FOCO_MAT_6_REMOTO', 'FOCO_MAT_4_PRESENC') ";
                        }

                        if (matriculaOptativaReforco.Disciplina == "FOCO_PORT_4_PRESENC" || matriculaOptativaReforco.Disciplina == "FOCO_PORT_6_REMOTO")
                        {
                            contextQuery2.Command += @" AND DISCIPLINA  in ('FOCO_PORT_4_PRESENC', 'FOCO_PORT_6_REMOTO') ";
                        }

                        contextQuery2.Parameters.Add("@SIT_MATRICULA", Matriculado);
                        contextQuery2.Parameters.Add("@ALUNO", matriculaOptativaReforco.Aluno);
                        contextQuery2.Parameters.Add("@ANO", matriculaOptativaReforco.Ano);
                        contextQuery2.Parameters.Add("@SEMESTRE", matriculaOptativaReforco.Semestre);


                        var matriculas2 = ctx.GetReturnValue<int>(contextQuery2);

                        if (matriculas2 > 0)
                        {
                            mensagens.Add("Já existe matricula ATIVA nesta disciplina de reforço!");
                        }


                        //Se for turma de reforço valida se existe professor alocado
                        if (!rnAulaDocente.ExisteDocentesRealEmAulaAtivaPor(ctx, matriculaOptativaReforco.Turma, matriculaOptativaReforco.Ano, matriculaOptativaReforco.Semestre, matriculaOptativaReforco.Disciplina))
                        {
                            mensagens.Add(string.Format("A turma de reforço {0} não possui professor alocado, para realizar a enturmação primeiro realize a alocação do professor.", matriculaOptativaReforco.Turma));
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
            }

            return validacaoDados;
        }
        public ValidacaoDados ValidaMatriculaOptativaReforco(LyMatricula matriculaOptativaReforco)
        {
            List<string> mensagens = new List<string>();
            string retorno = string.Empty;
            string tipo = string.Empty;
            Turma rnTurma = new Turma();
            AulaDocente rnAulaDocente = new AulaDocente();

            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (matriculaOptativaReforco != null)
            {
                matriculaOptativaReforco.SitMatricula = Matriculado;

                if (string.IsNullOrEmpty(matriculaOptativaReforco.Aluno))
                {
                    mensagens.Add("O aluno deve ser informado.");
                }

                if (matriculaOptativaReforco.Ano <= 0)
                {
                    mensagens.Add("O campo Ano é obrigatório!");
                }

                if (matriculaOptativaReforco.Semestre < 0)
                {
                    mensagens.Add("O campo Período é obrigatório!");
                }

                if (string.IsNullOrEmpty(matriculaOptativaReforco.Turma))
                {
                    mensagens.Add("O campo Turma Optativa / Reforço é obrigatório!");
                }

                if (string.IsNullOrEmpty(matriculaOptativaReforco.Matricula))
                {
                    mensagens.Add("A matrícula do responsável é obrigatória!");
                }

                if (mensagens.Count == 0)
                {
                    if (!PossuiMatriculaRegularAtiva(matriculaOptativaReforco.Ano, matriculaOptativaReforco.Aluno))
                    {
                        mensagens.Add("Não foi encontrada matricula principal para este aluno.");
                    }

                    matriculaOptativaReforco.Disciplina = rnTurma.ObtemDisciplinaOptativa(matriculaOptativaReforco.Ano, matriculaOptativaReforco.Semestre, matriculaOptativaReforco.Turma);

                    if (string.IsNullOrEmpty(matriculaOptativaReforco.Disciplina))
                    {
                        mensagens.Add("Não foi encontrada disciplina para esta turma.");
                    }

                    retorno = Matricula.AnalisarChoqueHorario(matriculaOptativaReforco);

                    if (!string.IsNullOrEmpty(retorno))
                    {
                        mensagens.Add("Encontrado choque de horário para esta aluno/turma. " + retorno);
                    }

                    using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                    {
                        //verifica se a aluno já tem matricula outra matricula ativa para a turma
                        var contextQuery = new ContextQuery(
                            @" SELECT  COUNT(*)
                            FROM    DBO.LY_MATRICULA M
                            WHERE   M.SIT_MATRICULA = @SIT_MATRICULA
                                    AND M.ALUNO = @ALUNO
                                    AND DISCIPLINA = @DISCIPLINA 
                                    AND ANO = @ANO
                                    AND SEMESTRE = @SEMESTRE
                                    AND TURMA = @TURMA");

                        contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
                        contextQuery.Parameters.Add("@ALUNO", matriculaOptativaReforco.Aluno);
                        contextQuery.Parameters.Add("@DISCIPLINA", matriculaOptativaReforco.Disciplina);
                        contextQuery.Parameters.Add("@ANO", matriculaOptativaReforco.Ano);
                        contextQuery.Parameters.Add("@SEMESTRE", matriculaOptativaReforco.Semestre);
                        contextQuery.Parameters.Add("@TURMA", matriculaOptativaReforco.Turma);

                        var matriculas = ctx.GetReturnValue<int>(contextQuery);

                        if (matriculas > 0)
                        {
                            mensagens.Add("Já existe matricula Optativa / Reforço ATIVA para este aluno!");
                        }
                        //Busca o tipo da turma optativa reforço                 
                        tipo = rnTurma.ObtemTipoOptativaReforcoPor(matriculaOptativaReforco.Turma, matriculaOptativaReforco.Ano, matriculaOptativaReforco.Semestre);

                        //Verifica se é uma turma de reforço
                        if (tipo == "S")
                        {
                            //Se for turma de reforço valida se existe professor alocado 
                            if (!rnAulaDocente.ExisteDocentesEmAulaPor(matriculaOptativaReforco.Turma, matriculaOptativaReforco.Ano, matriculaOptativaReforco.Semestre))
                            {
                                mensagens.Add(string.Format("A turma de reforço {0} não possui professor alocado, para realizar a enturmação primeiro realize a alocação do professor.", matriculaOptativaReforco.Turma));
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
            }

            return validacaoDados;
        }

        public bool EhMatriculaCancelada(LyMatricula matricula)
        {
            bool existeMatriculaCancelada;

            ContextQuery contextQuery = new ContextQuery(
                     @"SELECT  COUNT(*)
                        FROM    LY_MATRICULA
                        WHERE   ALUNO = @ALUNO
                                AND DISCIPLINA = @DISCIPLINA
                                AND TURMA = @TURMA
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE
                                AND SIT_MATRICULA = @SIT_MATRICULA ");

            contextQuery.Parameters.Add("@ALUNO", matricula.Aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", matricula.Disciplina);
            contextQuery.Parameters.Add("@TURMA", matricula.Turma);
            contextQuery.Parameters.Add("@ANO", matricula.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", matricula.Semestre);
            contextQuery.Parameters.Add("@SIT_MATRICULA", Cancelado);

            existeMatriculaCancelada = (ExecutarFuncao<int>(contextQuery) > 0);

            return existeMatriculaCancelada;
        }

        public bool PossuiAlgumaDisciplinaCanceladaPorTurma(LyMatricula matricula)
        {
            bool existe;

            ContextQuery contextQuery = new ContextQuery(
                     @" SELECT COUNT(*) 
                            FROM   LY_MATRICULA M 
                                   INNER JOIN LY_TURMA T 
                                           ON M.ANO = T.ANO 
                                              AND M.SEMESTRE = T.SEMESTRE 
                                              AND M.TURMA = T.TURMA 
                            WHERE  T.SIT_TURMA = 'Aberta' 
                                   AND M.ALUNO = @ALUNO 
                                   AND M.TURMA = @TURMA 
                                   AND M.ANO = @ANO 
                                   AND M.SEMESTRE = @SEMESTRE 
                                   AND SIT_MATRICULA = @SIT_MATRICULA  ");

            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, matricula.Aluno);
            contextQuery.Parameters.Add("@TURMA", matricula.Turma);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, matricula.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, matricula.Semestre);
            contextQuery.Parameters.Add("@SIT_MATRICULA", TechneDbType.T_SIT_MATRICULA, Cancelado);

            existe = (ExecutarFuncao<int>(contextQuery) > 0);

            return existe;
        }

        public bool EhMatriculaAtiva(string aluno, int ano, int periodo)
        {
            DataContext contexto = null;
            bool retorno = false;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                retorno = EhMatriculaAtiva(contexto, aluno, ano, periodo);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                {
                    contexto.Dispose();
                }
            }
            return retorno;
        }

        private bool EhMatriculaAtiva(DataContext ctx, string aluno, int ano, int periodo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                        FROM    LY_MATRICULA
                        WHERE   ALUNO = @ALUNO                               
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE
                                AND SIT_MATRICULA = @SIT_MATRICULA ";

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);
                contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);


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
        }

        public void SalvaMatriculaOptativaReforco(LyMatricula matriculaOptativaReforco)
        {
            LyTurma dadosTurma = new LyTurma();
            Matgrade matgradeRN = new Matgrade();
            string gradeId = string.Empty;

            using (var dataContext = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    if (this.EhMatriculaCancelada(matriculaOptativaReforco))
                    {
                        this.AtualizaMatriculaOptativaReforco(dataContext, matriculaOptativaReforco);
                    }
                    else
                    {
                        this.InsereMatriculaOptativaReforco(dataContext, matriculaOptativaReforco);
                    }

                    dadosTurma = RN.Turma.Carregar(Convert.ToInt32(matriculaOptativaReforco.Ano),
                                                    Convert.ToInt32(matriculaOptativaReforco.Semestre),
                                                    matriculaOptativaReforco.Turma);

                    gradeId = GradeSerie.ObterGradeId(
                        dataContext,
                        matriculaOptativaReforco.Ano,
                        matriculaOptativaReforco.Semestre,
                        dadosTurma.Curso,
                        dadosTurma.Curriculo,
                        dadosTurma.Serie,
                        matriculaOptativaReforco.Turma);

                    matgradeRN.Insere(matriculaOptativaReforco.Aluno, gradeId, dataContext);
                }
                catch (Exception exception)
                {
                    string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine, Convert.ToString(exception.Message));
                    throw new Exception(mensagem);
                }
            }
        }
        public void SalvaMatriculaOptativaReforcoProjetoFoco(LyMatricula matriculaOptativaReforco)
        {
            LyTurma dadosTurma = new LyTurma();
            Matgrade matgradeRN = new Matgrade();
            string gradeId = string.Empty;

            using (var dataContext = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    if (this.EhMatriculaCancelada(matriculaOptativaReforco))
                    {
                        this.AtualizaMatriculaOptativaReforco(dataContext, matriculaOptativaReforco);
                    }
                    else
                    {
                        this.InsereMatriculaReforcoProjetoFoco(dataContext, matriculaOptativaReforco);
                    }

                    dadosTurma = RN.Turma.Carregar(Convert.ToInt32(matriculaOptativaReforco.Ano),
                                                    Convert.ToInt32(matriculaOptativaReforco.Semestre),
                                                    matriculaOptativaReforco.Turma);

                    gradeId = GradeSerie.ObterGradeId(
                        dataContext,
                        matriculaOptativaReforco.Ano,
                        matriculaOptativaReforco.Semestre,
                        dadosTurma.Curso,
                        dadosTurma.Curriculo,
                        dadosTurma.Serie,
                        matriculaOptativaReforco.Turma);
                    if (gradeId != null)
                        matgradeRN.Insere(matriculaOptativaReforco.Aluno, gradeId, dataContext);

                }
                catch (Exception exception)
                {
                    string mensagem = string.Format("Não foi encontrado a grade curricular para essa turma.{0}Erro gerado: {1}",
                            Environment.NewLine, Convert.ToString(exception.Message));
                    throw new Exception(mensagem);
                }
            }
        }
        public void InsereMatriculaOptativaReforco(DataContext dataContext, LyMatricula matriculaOptativaReforco)
        {
            var contextQuery = new ContextQuery(
                @" INSERT  INTO dbo.LY_MATRICULA
                        ( ALUNO ,
                          DISCIPLINA ,
                          TURMA ,
                          ANO ,
                          SEMESTRE ,
                          SIT_MATRICULA ,
                          DT_ULTALT ,
                          COBRANCA_SEP ,
                          DT_INSERCAO ,
                          DT_MATRICULA ,
                          MATRICULA ,
                          DT_CADASTRO 
                        )
                VALUES  ( @ALUNO ,
                          @DISCIPLINA ,
                          @TURMA ,
                          @ANO ,
                          @SEMESTRE ,
                          @SIT_MATRICULA ,
                          GETDATE() ,
                          'N' ,
                          GETDATE() ,
                          GETDATE() ,
                          @MATRICULA ,
                          GETDATE()
                        ) ");

            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, matriculaOptativaReforco.Aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", TechneDbType.T_CODIGO, matriculaOptativaReforco.Disciplina);
            contextQuery.Parameters.Add("@TURMA", matriculaOptativaReforco.Turma);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, matriculaOptativaReforco.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, matriculaOptativaReforco.Semestre);
            contextQuery.Parameters.Add("@SIT_MATRICULA", TechneDbType.T_SIT_MATRICULA, Matriculado);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, matriculaOptativaReforco.Matricula);

            dataContext.ApplyModifications(contextQuery);
        }

        public void InsereMatriculaReforcoProjetoFoco(DataContext dataContext, LyMatricula matriculaOptativaReforco)
        {
            var contextQuery = new ContextQuery(
                @" INSERT  INTO dbo.LY_MATRICULA
                        ( ALUNO ,
                          DISCIPLINA ,
                          TURMA ,
                          ANO ,
                          SEMESTRE ,
                          SIT_MATRICULA ,
                          DT_ULTALT ,
                          COBRANCA_SEP ,
                          DT_INSERCAO ,
                          DT_MATRICULA ,
                          MATRICULA ,
                          DT_CADASTRO ,
                          MAIS_EDUCACAO
                        )
                VALUES  ( @ALUNO ,
                          @DISCIPLINA ,
                          @TURMA ,
                          @ANO ,
                          @SEMESTRE ,
                          @SIT_MATRICULA ,
                          GETDATE() ,
                          'N' ,
                          GETDATE() ,
                          GETDATE() ,
                          @MATRICULA ,
                          GETDATE(),
                          @MAIS_EDUCACAO
                        ) ");
            string maiseduca = "";
            if (matriculaOptativaReforco.MaisEducacao == "9999.01" || matriculaOptativaReforco.MaisEducacao == "9999.03") maiseduca = "P"; else maiseduca = "V";
            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, matriculaOptativaReforco.Aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", TechneDbType.T_CODIGO, matriculaOptativaReforco.Disciplina);
            contextQuery.Parameters.Add("@TURMA", matriculaOptativaReforco.Turma);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, matriculaOptativaReforco.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, matriculaOptativaReforco.Semestre);
            contextQuery.Parameters.Add("@SIT_MATRICULA", TechneDbType.T_SIT_MATRICULA, Matriculado);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, matriculaOptativaReforco.Matricula);
            contextQuery.Parameters.Add("@MAIS_EDUCACAO", SqlDbType.VarChar, maiseduca);

            dataContext.ApplyModifications(contextQuery);
        }

        public void AtualizaMatriculaOptativaReforco(DataContext dataContext, LyMatricula matriculaOptativaReforco)
        {
            var contextQuery = new ContextQuery(
                @" UPDATE  LY_MATRICULA
                    SET     SIT_MATRICULA = @SIT_MATRICULA ,
                            STAMP_ATUALIZACAO = GETDATE() ,
                            MATRICULA = @MATRICULA ,
                            CONCOMITANTE = 'N' ,
                            DEPENDENCIA = 'N' ,
                            SERIE_REFERENCIA = NULL, 
                            DISCIPLINA_REFERENCIA = NULL, 
                            EDUC_ESPECIAL = 'N' 
                    WHERE   ALUNO = @ALUNO
                            AND DISCIPLINA = @DISCIPLINA
                            AND TURMA = @TURMA
                            AND ANO = @ANO
                            AND SEMESTRE = @SEMESTRE  ");

            contextQuery.Parameters.Add("@SIT_MATRICULA", TechneDbType.T_SIT_MATRICULA, Matriculado);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, matriculaOptativaReforco.Matricula);
            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, matriculaOptativaReforco.Aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", TechneDbType.T_CODIGO, matriculaOptativaReforco.Disciplina);
            contextQuery.Parameters.Add("@TURMA", matriculaOptativaReforco.Turma);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, matriculaOptativaReforco.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, matriculaOptativaReforco.Semestre);

            dataContext.ApplyModifications(contextQuery);
        }

        public void RemoveMatriculaOptativaReforco(LyMatricula matriculaOptativaReforco)
        {
            LyTurma turmaOptativaReforco = new LyTurma();
            string gradeId = string.Empty;

            using (var context = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    this.RemoveMatriculaOptativaReforco(context, matriculaOptativaReforco);

                    //carregar dados da turma
                    turmaOptativaReforco = RN.Turma.Carregar(Convert.ToInt32(matriculaOptativaReforco.Ano),
                                                    Convert.ToInt32(matriculaOptativaReforco.Semestre),
                                                    matriculaOptativaReforco.Turma);
                    //obter grade_id
                    gradeId = GradeSerie.ObterGradeId(
                        context,
                        matriculaOptativaReforco.Ano,
                        matriculaOptativaReforco.Semestre,
                        turmaOptativaReforco.Curso,
                        turmaOptativaReforco.Curriculo,
                        turmaOptativaReforco.Serie,
                        matriculaOptativaReforco.Turma);

                    RN.Matgrade.Remover(context, matriculaOptativaReforco.Aluno, gradeId);
                }
                catch (Exception exception)
                {
                    string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine, Convert.ToString(exception.Message));
                    throw new Exception(mensagem);
                }
            }
        }

        public void RemoveMatriculaOptativaReforco(DataContext dataContext, LyMatricula matriculaOptativaReforco)
        {
            var contextQuery = new ContextQuery(
                @" UPDATE  LY_MATRICULA
                    SET     SIT_MATRICULA = @SIT_MATRICULA, 
		                    STAMP_ATUALIZACAO = GETDATE(),
                            DT_ULTALT = GETDATE(), 
                            MATRICULA = @MATRICULA
                    WHERE   ALUNO = @ALUNO        
                            AND TURMA = @TURMA
                            AND ANO = @ANO
                            AND SEMESTRE = @SEMESTRE ");

            contextQuery.Parameters.Add("@SIT_MATRICULA", Cancelado);
            contextQuery.Parameters.Add("@MATRICULA", matriculaOptativaReforco.Matricula);
            contextQuery.Parameters.Add("@ALUNO", matriculaOptativaReforco.Aluno);
            contextQuery.Parameters.Add("@TURMA", matriculaOptativaReforco.Turma);
            contextQuery.Parameters.Add("@ANO", matriculaOptativaReforco.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", matriculaOptativaReforco.Semestre);

            dataContext.ApplyModifications(contextQuery);
        }

        bool PossuiMatriculaOptativaReforco(DataContext ctx, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @"  SELECT COUNT(*)
                                FROM    DBO.LY_MATRICULA M ( NOLOCK )
                                        INNER JOIN DBO.LY_TURMA T ( NOLOCK ) ON T.TURMA = M.TURMA
                                                                                AND T.ANO = M.ANO
                                                                                AND T.SEMESTRE = M.SEMESTRE
                                                                                AND T.DISCIPLINA = M.DISCIPLINA
                                WHERE   M.SIT_MATRICULA = 'Matriculado'
                                        AND NOT ( T.OPTATIVAREFORCO = 'N' )
                                        AND ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", aluno);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        bool PossuiMatriculaEletiva(DataContext ctx, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @"  SELECT COUNT(*)
                                FROM    DBO.LY_MATRICULA M ( NOLOCK )
                                        INNER JOIN DBO.LY_TURMA T ( NOLOCK ) ON T.TURMA = M.TURMA
                                                                                AND T.ANO = M.ANO
                                                                                AND T.SEMESTRE = M.SEMESTRE
                                                                                AND T.DISCIPLINA = M.DISCIPLINA
                                WHERE   M.SIT_MATRICULA = 'Matriculado'
                                        AND T.ELETIVA = 'S' 
                                        AND ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", aluno);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiMatriculaEletiva(DataContext ctx, string censo, int ano, int periodo, string curso, int serie)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1)
                                    FROM    DBO.LY_MATRICULA M ( NOLOCK )
                                            INNER JOIN DBO.LY_TURMA T ( NOLOCK ) ON T.TURMA = M.TURMA
                                                                                    AND T.ANO = M.ANO
                                                                                    AND T.SEMESTRE = M.SEMESTRE
                                                                                    AND T.DISCIPLINA = M.DISCIPLINA
		                                    INNER JOIN LY_TURMA TR ON T.TURMAREFERENCIA = TR.TURMA 
									                                    AND T.ANO = TR.ANO 
									                                    AND T.SEMESTRE = TR.SEMESTRE
                                    WHERE   M.SIT_MATRICULA = 'Matriculado'
                                            AND ISNULL(T.ELETIVA,'N') = 'S' 
                                            AND T.ANO = @ANO
		                                    AND T.SEMESTRE = @SEMESTRE
		                                    AND TR.CURSO = @CURSO
		                                    AND TR.SERIE = @SERIE
		                                    AND T.FACULDADE = @CENSO ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, periodo);
            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
            contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public int ObtemQuantidadeMatriculadosPor(DataContext contexto, string censo, int ano, int periodo, int serie, string curso, string turno)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT COUNT(DISTINCT ALUNO) AS MATRICULADOS
								FROM LY_MATRICULA M
									INNER JOIN LY_TURMA T ON M.ANO = T.ANO
													AND M.SEMESTRE = T.SEMESTRE
													AND M.TURMA = T.TURMA
													AND M.DISCIPLINA = T.DISCIPLINA
								WHERE M.SIT_MATRICULA = 'Matriculado'
                                    AND M.ANO = @ANO
									AND M.SEMESTRE = @PERIODO
                                    AND T.FACULDADE = @CENSO
									and T.CURSO = @CURSO
									AND T.SERIE = @SERIE
									AND T.TURNO = @TURNO 
                                    AND ( M.DEPENDENCIA IS NULL
                                          OR M.DEPENDENCIA = 'N'
                                        )
                                    AND ( M.CONCOMITANTE IS NULL
                                          OR M.CONCOMITANTE = 'N'
                                        )
                                    AND ( M.EDUC_ESPECIAL IS NULL
                                          OR M.EDUC_ESPECIAL = 'N'
                                        )
                                    AND ( M.MAIS_EDUCACAO IS NULL
                                          OR M.MAIS_EDUCACAO = 'N'
                                        )
                                    AND T.OPTATIVAREFORCO = 'N'
                                    AND ISNULL(T.ELETIVA,'N') = 'N'";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
                contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
                contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, turno);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["MATRICULADOS"]);
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

        public bool PossuiMatriculaEletivaAtiva(int ano, int periodo, string turma)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();

            try
            {
                return this.PossuiMatriculaEletivaAtiva(ctx, ano, periodo, turma);
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

        private bool PossuiMatriculaEletivaAtiva(DataContext ctx, int ano, int periodo, string turma)
        {
            bool possui = false;
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT COUNT(*)
                                FROM    DBO.LY_MATRICULA M ( NOLOCK )
                                        INNER JOIN DBO.LY_TURMA T ( NOLOCK ) ON T.TURMA = M.TURMA
                                                                                AND T.ANO = M.ANO
                                                                                AND T.SEMESTRE = M.SEMESTRE
                                                                                AND T.DISCIPLINA = M.DISCIPLINA
                                WHERE   M.SIT_MATRICULA = @SIT_MATRICULA
                                        AND T.ELETIVA = 'S' 
                                        AND M.ANO = @ANO
                                        AND M.SEMESTRE = @SEMESTRE
                                        AND M.TURMA = @TURMA ";

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", periodo);
            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public bool PossuiMatriculaEletivaAtiva(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();

            try
            {
                return this.PossuiMatriculaEletivaAtiva(ctx, aluno);
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

        private bool PossuiMatriculaEletivaAtiva(DataContext ctx, string aluno)
        {
            bool possui = false;
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT COUNT(*)
                                FROM    DBO.LY_MATRICULA M ( NOLOCK )
                                        INNER JOIN DBO.LY_TURMA T ( NOLOCK ) ON T.TURMA = M.TURMA
                                                                                AND T.ANO = M.ANO
                                                                                AND T.SEMESTRE = M.SEMESTRE
                                                                                AND T.DISCIPLINA = M.DISCIPLINA
                                WHERE   M.SIT_MATRICULA = @SIT_MATRICULA
                                        AND T.ELETIVA = 'S' 
                                        AND ALUNO = @ALUNO  ";

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public bool PossuiMatriculaOptativaEnsinoReligioso(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                possui = this.PossuiMatriculaOptativaEnsinoReligioso(ctx, aluno);
                return possui;
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public bool PossuiMatriculaOptativaEnsinoReligioso(DataContext ctx, string aluno)
        {
            bool possui = false;

            ContextQuery contextQuery = new ContextQuery
            {
                Command = @" SELECT COUNT(*)
                                FROM    DBO.LY_MATRICULA M ( NOLOCK )
                                        INNER JOIN DBO.LY_TURMA T ( NOLOCK ) ON T.TURMA = M.TURMA
                                                                                AND T.ANO = M.ANO
                                                                                AND T.SEMESTRE = M.SEMESTRE
                                                                                AND T.DISCIPLINA = M.DISCIPLINA
                                WHERE   M.SIT_MATRICULA = 'Matriculado'
                                        AND T.OPTATIVAREFORCO = 'R'
                                        AND ALUNO = @ALUNO"
            };

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public bool PossuiMatriculaOptativaLinguaEstrangeira(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                possui = this.PossuiMatriculaOptativaLinguaEstrangeira(ctx, aluno);
                return possui;
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public bool PossuiMatriculaOptativaLinguaEstrangeira(DataContext ctx, string aluno)
        {
            bool possui = false;

            ContextQuery contextQuery = new ContextQuery
            {
                Command = @" SELECT COUNT(*)
                                FROM    DBO.LY_MATRICULA M ( NOLOCK )
                                        INNER JOIN DBO.LY_TURMA T ( NOLOCK ) ON T.TURMA = M.TURMA
                                                                                AND T.ANO = M.ANO
                                                                                AND T.SEMESTRE = M.SEMESTRE
                                                                                AND T.DISCIPLINA = M.DISCIPLINA
                                WHERE   M.SIT_MATRICULA = 'Matriculado'
                                        AND T.OPTATIVAREFORCO = 'L'
                                        AND ALUNO = @ALUNO "
            };

            contextQuery.Parameters.Add("@ALUNO", aluno);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiMatriculaDependencia(DataContext ctx, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @"  SELECT COUNT(*)
                                FROM    DBO.LY_MATRICULA M ( NOLOCK )
                                WHERE   M.SIT_MATRICULA = 'Matriculado'
                                        AND ISNULL(DEPENDENCIA, 'N') = 'S'
                                        AND ALUNO = @ALUNO  ";

            contextQuery.Parameters.Add("@ALUNO", aluno);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }
            return existe;
        }

        private bool PossuiMatriculaMaisEducacao(DataContext ctx, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*)
                                FROM    DBO.LY_MATRICULA M ( NOLOCK )
                                WHERE   M.SIT_MATRICULA = 'Matriculado'
                                        AND ISNULL(MAIS_EDUCACAO, 'N') = 'S'
                                        AND ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", aluno);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiMatriculaReforcoFoco(DataContext ctx, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*)
                                FROM    DBO.LY_MATRICULA M ( NOLOCK )
                                WHERE   M.SIT_MATRICULA = 'Matriculado'
                                        AND ISNULL(MAIS_EDUCACAO, 'N') in ('P', 'V')
                                        AND ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", aluno);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiMatriculaEducacaoEspecial(DataContext ctx, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*)
                                FROM    DBO.LY_MATRICULA M ( NOLOCK )
                                WHERE   M.SIT_MATRICULA = 'Matriculado'
                                        AND ISNULL(EDUC_ESPECIAL, 'N') = 'S'
                                        AND ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", aluno);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiMatriculaLetramento(DataContext ctx, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*)
                                FROM    DBO.LY_MATRICULA M ( NOLOCK )
                                WHERE   M.SIT_MATRICULA = 'Matriculado'
                                        AND ISNULL(MAIS_EDUCACAO, 'N') = 'L'
                                        AND ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", aluno);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiMatriculaEnsinoProfissionalConcomitante(DataContext ctx, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*)
                                FROM    DBO.LY_MATRICULA M ( NOLOCK )
                                WHERE   M.SIT_MATRICULA = 'Matriculado'
                                        AND ISNULL(CONCOMITANTE, 'N') = 'S'
                                        AND ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", aluno);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable ListaMatriculasEletivasPor(string turmaReferencia, int ano, int periodo)
        {
            return ListaMatriculasEletivasPor(null, turmaReferencia, ano, periodo);
        }

        public DataTable ListaMatriculasEletivasPorAluno(string aluno, int ano, int periodo)
        {
            return ListaMatriculasEletivasPor(aluno, null, ano, periodo);
        }

        private DataTable ListaMatriculasEletivasPor(string aluno, string turmaReferencia, int ano, int periodo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            StringBuilder sql = new StringBuilder();
            try
            {
                sql.Append(@"  SELECT DISTINCT A.ALUNO, 
	                                        P.NOME_COMPL, 
	                                        EL1.TURMA AS TURMAELETIVA1, 
	                                        EL1.NOMEDISCIPLINA + ' - ' + EL1.TURMA as TURMADISCIPLINAELETIVA1,
	                                        EL2.TURMA AS TURMAELETIVA2, 
	                                        EL2.NOMEDISCIPLINA + ' - ' + EL2.TURMA as TURMADISCIPLINAELETIVA2,
	                                        EL3.TURMA AS TURMAELETIVA3,
	                                        EL3.NOMEDISCIPLINA + ' - ' + EL3.TURMA as TURMADISCIPLINAELETIVA3
                                        FROM LY_MATRICULA M (NOLOCK)
	                                        INNER JOIN LY_ALUNO A (NOLOCK) ON M.ALUNO = A.ALUNO
	                                        INNER JOIN LY_PESSOA P (NOLOCK) ON A.PESSOA = P.PESSOA
	                                        INNER JOIN DBO.LY_TURMA T (NOLOCK) ON M.DISCIPLINA = T.DISCIPLINA
									                                        AND M.TURMA = T.TURMA
									                                        AND M.ANO = T.ANO
									                                        AND M.SEMESTRE = T.SEMESTRE
	                                        LEFT JOIN (SELECT DISTINCT M2.ALUNO, T2.TURMA, T2.DISCIPLINA, T2.DISCIPLINA_MULTIPLA, D2.NOME AS NOMEDISCIPLINA, T2.TURMAREFERENCIA, T2.ANO, T2.SEMESTRE, D2.GRUPO
					                                         FROM LY_TURMA T2 (NOLOCK)
						                                        INNER JOIN LY_DISCIPLINA D2 (NOLOCK) ON ISNULL(T2.DISCIPLINA_MULTIPLA, T2.DISCIPLINA) = D2.DISCIPLINA
						                                        INNER JOIN LY_MATRICULA M2 (NOLOCK) ON M2.DISCIPLINA = T2.DISCIPLINA
									                                        AND M2.TURMA = T2.TURMA
									                                        AND M2.ANO = T2.ANO
									                                        AND M2.SEMESTRE = T2.SEMESTRE
						                                        WHERE T2.ELETIVA = 'S'
							                                         AND D2.GRUPO = 1
							                                         and SIT_MATRICULA = 'Matriculado' ) EL1 
								                                        ON EL1.ALUNO = M.ALUNO AND EL1.ANO = M.ANO
	                                        LEFT JOIN (SELECT DISTINCT M2.ALUNO, T2.TURMA, T2.DISCIPLINA, T2.DISCIPLINA_MULTIPLA,  D2.NOME AS NOMEDISCIPLINA, T2.TURMAREFERENCIA, T2.ANO, T2.SEMESTRE, D2.GRUPO
					                                         FROM LY_TURMA T2 (NOLOCK)
						                                        INNER JOIN LY_DISCIPLINA D2 (NOLOCK) ON ISNULL(T2.DISCIPLINA_MULTIPLA, T2.DISCIPLINA) = D2.DISCIPLINA
						                                        INNER JOIN LY_MATRICULA M2 (NOLOCK) ON M2.DISCIPLINA = T2.DISCIPLINA
									                                        AND M2.TURMA = T2.TURMA
									                                        AND M2.ANO = T2.ANO
									                                        AND M2.SEMESTRE = T2.SEMESTRE
						                                        WHERE T2.ELETIVA = 'S'
							                                         AND D2.GRUPO = 2
							                                         and SIT_MATRICULA = 'Matriculado' ) EL2 
								                                        ON EL2.ALUNO = M.ALUNO AND EL2.ANO = M.ANO
	                                        LEFT JOIN (SELECT DISTINCT M2.ALUNO, T2.TURMA, T2.DISCIPLINA, T2.DISCIPLINA_MULTIPLA,  D2.NOME AS NOMEDISCIPLINA, T2.TURMAREFERENCIA, T2.ANO, T2.SEMESTRE, D2.GRUPO
					                                         FROM LY_TURMA T2 (NOLOCK)
						                                        INNER JOIN LY_DISCIPLINA D2 (NOLOCK) ON ISNULL(T2.DISCIPLINA_MULTIPLA, T2.DISCIPLINA) = D2.DISCIPLINA
						                                        INNER JOIN LY_MATRICULA M2 (NOLOCK) ON M2.DISCIPLINA = T2.DISCIPLINA
									                                        AND M2.TURMA = T2.TURMA
									                                        AND M2.ANO = T2.ANO
									                                        AND M2.SEMESTRE = T2.SEMESTRE
						                                        WHERE T2.ELETIVA = 'S'
							                                         AND D2.GRUPO = 3
							                                         and SIT_MATRICULA = 'Matriculado' ) EL3 
								                                        ON EL3.ALUNO = M.ALUNO AND EL3.ANO = M.ANO
                                        WHERE M.ANO = @ANO
	                                        AND M.SEMESTRE = @PERIODO
	                                        AND SIT_MATRICULA = 'Matriculado'
                                            AND ISNULL(T.ELETIVA, 'N') = 'N'
                                            AND ISNULL(M.DEPENDENCIA, 'N') = 'N'
                                            AND ISNULL(M.MAIS_EDUCACAO, 'N') = 'N' 
                                            AND ISNULL(M.EDUC_ESPECIAL, 'N') = 'N' 
                                            AND ISNULL(M.CONCOMITANTE, 'N') = 'N' 
                                            ");

                if (aluno.IsNullOrEmptyOrWhiteSpace())
                {
                    sql.Append(@"AND M.TURMA = @TURMAREFERENCIA
	                                        ");
                    contextQuery.Parameters.Add("@TURMAREFERENCIA", SqlDbType.VarChar, turmaReferencia);
                }
                else
                {
                    sql.Append(@"AND A.ALUNO = @ALUNO
	                                        ");
                    contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
                }

                sql.Append(@"ORDER BY P.NOME_COMPL ");

                contextQuery.Command = sql.ToString();

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);

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

        public DataTable ListaMatriculaEletivaAtivaPor(string aluno)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT M.ALUNO,
		                                    M.ANO, 
		                                    M.SEMESTRE, 
		                                    T.TURNO, 
		                                    TU.DESCRICAO AS DESCRICAOTURNO, 
		                                    T.TURMA, 
		                                    D.NOME AS DISCIPLINA, 
											D.GRUPO, 
		                                    T.DT_INICIO
                                    FROM LY_MATRICULA M (NOLOCK)
	                                    INNER JOIN DBO.LY_TURMA T (NOLOCK) ON M.DISCIPLINA = T.DISCIPLINA
									                                    AND M.TURMA = T.TURMA
									                                    AND M.ANO = T.ANO
									                                    AND M.SEMESTRE = T.SEMESTRE
	                                    INNER JOIN LY_TURNO TU (NOLOCK) ON T.TURNO = TU.TURNO
	                                    INNER JOIN LY_DISCIPLINA D ON ISNULL(T.DISCIPLINA_MULTIPLA, T.DISCIPLINA) = D.DISCIPLINA
                                    WHERE T.ELETIVA = 'S'
	                                      AND M.SIT_MATRICULA = @SIT_MATRICULA
	                                      AND M.ALUNO = @ALUNO ";

                contextQuery.Parameters.Add("@SIT_MATRICULA", SqlDbType.VarChar, Matriculado);
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


        public DataTable ListaDadosEscolaresPor(string aluno)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT T.TURMA, 
		                                            CASE
			                                            WHEN ISNULL(M.DEPENDENCIA, 'N') = 'S' THEN 'PROGRESSÃO PARCIAL'
			                                            WHEN ISNULL(M.EDUC_ESPECIAL, 'N') = 'S' THEN 'EDUCAÇÃO ESPECIAL'
			                                            WHEN ISNULL(M.CONCOMITANTE, 'N') = 'S' THEN 'EDUCAÇÃO PROFISSIONAL CONCOMITANTE'
			                                            WHEN ISNULL(M.MAIS_EDUCACAO, 'N') = 'S' THEN 'MAIS EDUCAÇÃO'
                                                        WHEN ISNULL(M.MAIS_EDUCACAO, 'N') in ('P','V') THEN 'FOCO'
                                                        WHEN ISNULL(M.MAIS_EDUCACAO, 'N') = 'L' THEN 'LETRAMENTO'
                                                        WHEN ISNULL(M.MAIS_EDUCACAO, 'N') = 'O' THEN 'NOA'
			                                            WHEN ISNULL(T.OPTATIVAREFORCO, 'N') <> 'N' THEN 'OPTATIVA'
			                                            WHEN ISNULL(T.ELETIVA, 'N') = 'S' THEN 'ELETIVA'
			                                            ELSE 'PRINCIPAL'
		                                            END TIPO,
		                                            C.NOME AS CURSO,
		                                            TU.DESCRICAO AS TURNO,
		                                            M.ALUNO
                                            FROM LY_MATRICULA M
	                                            INNER JOIN LY_TURMA T ON M.ANO = T.ANO 
							                                            AND M.SEMESTRE = T.SEMESTRE 
							                                            AND T.TURMA = M.TURMA
							                                             AND T.DISCIPLINA = M.DISCIPLINA
	                                            INNER JOIN LY_CURSO C ON T.CURSO = C.CURSO
	                                            INNER JOIN LY_TURNO TU ON T.TURNO = TU.TURNO
                                            WHERE SIT_MATRICULA = 'Matriculado'
	                                            AND ISNULL(M.MAIS_EDUCACAO, 'N') NOT IN ('P','V')
	                                            AND ALUNO = @ALUNO";

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

        public LyMatricula ObtemMatriculaPor(string aluno, int ano, int periodo, string turma, string disciplina)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            LyMatricula matricula = new LyMatricula();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT  *
                        FROM    LY_MATRICULA WITH ( NOLOCK )
                        WHERE   DISCIPLINA = @DISCIPLINA
                                AND TURMA = @TURMA
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE
                                AND ALUNO = @ALUNO "
                };

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);
                contextQuery.Parameters.Add("@ALUNO", aluno);

                matricula = ctx.TryToBindEntity<LyMatricula>(contextQuery);

                return matricula;
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

        public string ObtemTurmaPrincipalCanceladaPor(DataContext contexto, string aluno, int ano, int periodo, string curso, string turno, string curriculo, int serie, string unidadeEnsino)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT TOP 1 M.TURMA
                                    FROM LY_MATRICULA M
                                        INNER JOIN LY_TURMA T ON M.ANO = T.ANO
                                            AND M.SEMESTRE = T.SEMESTRE
                                            AND M.TURMA = T.TURMA
                                    WHERE M.ALUNO = @ALUNO 
		                                    AND T.ANO = @ANO
		                                    AND T.SEMESTRE = @SEMESTRE 
		                                    AND T.CURSO = @CURSO
		                                    AND T.TURNO = @TURNO
		                                    AND T.CURRICULO =@CURRICULO
		                                    AND T.SERIE = @SERIE
		                                    AND T.FACULDADE = @FACULDADE
		                                    AND T.SIT_TURMA = 'ABERTA'
		                                    AND M.SIT_MATRICULA <> @SIT_MATRICULA
                                            AND T.OPTATIVAREFORCO = 'N'
                                            AND ISNULL(T.ELETIVA,'N') = 'N'
											AND ISNULL(M.DEPENDENCIA, 'N') = 'N'
											AND ISNULL(M.EDUC_ESPECIAL, 'N') = 'N'
											AND ISNULL(M.MAIS_EDUCACAO, 'N') = 'N'
											AND ISNULL(M.CONCOMITANTE, 'N') = 'N'
                                    ORDER BY M.STAMP_ATUALIZACAO DESC  ";

            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE, periodo);
            contextQuery.Parameters.Add("@CURSO", TechneDbType.T_CODIGO, curso);
            contextQuery.Parameters.Add("@TURNO", TechneDbType.T_CODIGO, turno);
            contextQuery.Parameters.Add("@CURRICULO", TechneDbType.T_CODIGO, curriculo);
            contextQuery.Parameters.Add("@SERIE", TechneDbType.T_NUMERO_PEQUENO, serie);
            contextQuery.Parameters.Add("@FACULDADE", TechneDbType.T_CODIGO, unidadeEnsino);
            contextQuery.Parameters.Add("@SIT_MATRICULA", TechneDbType.T_CODIGO, Matriculado);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public void ReativaMatriculaPrincipal(DataContext contexto, string aluno, DateTime dataReabertura, int ano, int periodo, string turma)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_MATRICULA 
                        SET    SIT_MATRICULA = @SIT_MATRICULA, 
                               CONCOMITANTE = 'N', 
                               DEPENDENCIA = 'N', 
                               SERIE_REFERENCIA = NULL, 
                               DISCIPLINA_REFERENCIA = NULL, 
                               EDUC_ESPECIAL = 'N', 
                               MAIS_EDUCACAO = 'N', 
                               DT_ULTALT = @DT_ULTALT, 
                               DT_REABERTURA = @DT_REABERTURA 
                        WHERE  ALUNO = @ALUNO 
                               AND ANO = @ANO 
                               AND SEMESTRE = @SEMESTRE 
                               AND TURMA = @TURMA  ";

            contextQuery.Parameters.Add("@SIT_MATRICULA", TechneDbType.T_CODIGO, Matriculado);
            contextQuery.Parameters.Add("@DT_ULTALT", TechneDbType.T_DATA, dataReabertura);
            contextQuery.Parameters.Add("@DT_REABERTURA", TechneDbType.T_DATA, dataReabertura);
            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, periodo);
            contextQuery.Parameters.Add("@TURMA", turma);

            contexto.ApplyModifications(contextQuery);
        }

        public DadosEnturmacaoAluno ObtemMatriculaPrincipalAtivaPor(string aluno)
        {
            DadosEnturmacaoAluno enturmacao = new DadosEnturmacaoAluno();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();

            try
            {
                enturmacao = this.ObtemMatriculaPrincipalAtivaPor(contexto, aluno);
                return enturmacao;
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

        public DadosEnturmacaoAluno ObtemMatriculaPrincipalAtivaPor(DataContext contexto, string aluno)
        {
            DadosEnturmacaoAluno enturmacao = new DadosEnturmacaoAluno();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DISTINCT 
                            M.ALUNO ,
                            M.ANO ,
                            M.SEMESTRE ,
                            M.TURMA ,
                            T.CURSO ,
                            T.TURNO ,
                            T.SERIE ,
                            T.CURRICULO ,
                            T.FACULDADE ,
		                    T.DT_FIM,
							CV.ID_CONTROLE_VAGA,
							UE.MUNICIPIO,
							A.TIPO_ENSINO_PROFISSIONALIZANTE,
							C.TIPO_CURSO,
                            C.TIPO,
							GT.GRADE_ID,
							M.DT_MATRICULA
                    FROM    LY_MATRICULA M (NOLOCK)
							INNER JOIN LY_ALUNO A (NOLOCK) ON M.ALUNO = A.ALUNO
                            INNER JOIN DBO.LY_TURMA T (NOLOCK) ON M.DISCIPLINA = T.DISCIPLINA
                                                         AND M.TURMA = T.TURMA
                                                         AND M.ANO = T.ANO
                                                         AND M.SEMESTRE = T.SEMESTRE
							INNER JOIN LY_GRADE_TURMA GT (NOLOCK) 
                                                       ON GT.DISCIPLINA = T.DISCIPLINA 
                                                          AND GT.TURMA = T.TURMA 
                                                          AND GT.ANO = T.ANO 
                                                          AND GT.SEMESTRE = T.SEMESTRE 
							INNER JOIN LY_CURSO C (NOLOCK) ON C.CURSO = T.CURSO
						    INNER JOIN TCE_CONTROLE_VAGA CV (NOLOCK) ON CV.ANO = M.ANO
							                              AND CV.PERIODO = M.SEMESTRE
														  AND CV.CURSO = T.CURSO
														  AND CV.SERIE = T.SERIE
														  AND CV.CENSO = T.FACULDADE
														  AND CV.TURNO = T.TURNO
							INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK) ON CV.CENSO = UE.UNIDADE_ENS
                    WHERE   M.ALUNO = @ALUNO
                            AND M.SIT_MATRICULA = @SIT_MATRICULA
                            AND T.OPTATIVAREFORCO = 'N'
                            AND ISNULL(T.ELETIVA,'N') = 'N'
                            AND ISNULL(M.DEPENDENCIA, 'N') = 'N'
                            AND ISNULL(M.EDUC_ESPECIAL, 'N') = 'N'
                            AND ISNULL(M.MAIS_EDUCACAO, 'N') = 'N'
                            AND ISNULL(M.CONCOMITANTE, 'N') = 'N'  ";

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    enturmacao.Aluno = Convert.ToString(reader["ALUNO"]);
                    enturmacao.Ano = Convert.ToInt32(reader["ANO"]);
                    enturmacao.Periodo = Convert.ToInt32(reader["SEMESTRE"]);
                    enturmacao.Turma = Convert.ToString(reader["TURMA"]);
                    enturmacao.Curso = Convert.ToString(reader["CURSO"]);
                    enturmacao.Serie = Convert.ToInt32(reader["SERIE"]);
                    enturmacao.Turno = Convert.ToString(reader["TURNO"]);
                    enturmacao.Censo = Convert.ToString(reader["FACULDADE"]);
                    enturmacao.Curriculo = Convert.ToString(reader["CURRICULO"]);
                    enturmacao.DataFimTurma = Convert.ToDateTime(reader["DT_FIM"]);
                    enturmacao.IdControleVaga = Convert.ToInt32(reader["ID_CONTROLE_VAGA"]);
                    enturmacao.MunicipioEscola = Convert.ToString(reader["MUNICIPIO"]);
                    enturmacao.TipoEnsinoProfissionalizante = Convert.ToString(reader["TIPO_ENSINO_PROFISSIONALIZANTE"]);
                    enturmacao.TipoCurso = Convert.ToString(reader["TIPO_CURSO"]);
                    enturmacao.Tipo = Convert.ToString(reader["TIPO"]);
                    enturmacao.GradeId = Convert.ToString(reader["GRADE_ID"]);
                    enturmacao.DataMatricula = Convert.ToDateTime(reader["DT_MATRICULA"]);
                }

                return enturmacao;
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

        public void RemoveParaFechamento(DataContext ctx, LyMatricula matricula)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" DELETE LY_MATRICULA
                    WHERE   ALUNO = @ALUNO
                            AND DISCIPLINA = @DISCIPLINA
                            AND TURMA = @TURMA
                            AND ANO = @ANO
                            AND SEMESTRE = @SEMESTRE ";

                contextQuery.Parameters.Add("@DISCIPLINA", matricula.Disciplina);
                contextQuery.Parameters.Add("@TURMA", matricula.Turma);
                contextQuery.Parameters.Add("@ANO", matricula.Ano);
                contextQuery.Parameters.Add("@SEMESTRE", matricula.Semestre);
                contextQuery.Parameters.Add("@ALUNO", matricula.Aluno);

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


        public bool PossuiMatriculaAtivaNaTurma(string turma, decimal ano, decimal semestre)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT  COUNT(*)
                        FROM    LY_MATRICULA
                        WHERE   TURMA = @TURMA
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE
                                AND SIT_MATRICULA <> @SIT_MATRICULA "
                };

                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@SIT_MATRICULA", Cancelado);

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
        }

        public string ObtemMatriculaEletivaPor(DataContext ctx, string aluno, int ano, int semestre, int grupo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT  DISTINCT TURMA
                            FROM    LY_MATRICULA M
	                            INNER JOIN LY_DISCIPLINA D ON M.DISCIPLINA = D.DISCIPLINA
                            WHERE   ALUNO = @ALUNO
                                    AND ANO = @ANO
                                    AND SEMESTRE = @SEMESTRE
                                    and SIT_MATRICULA = @SIT_MATRICULA
		                            AND D.GRUPO = @GRUPO ";

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
                contextQuery.Parameters.Add("@GRUPO", grupo);

                resultado = contexto.GetReturnValue<string>(contextQuery);

                return resultado;
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

        public bool PossuiMatriculaAtivaPossiveisPeriodosPor(string aluno, decimal ano, decimal periodo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            bool retorno = false;

            try
            {
                retorno = this.PossuiMatriculaAtivaPossiveisPeriodosPor(contexto, aluno, ano, periodo);
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

        public bool PossuiMatriculaAtivaPeriodoPor(string aluno, int ano)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();

            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                FROM LY_MATRICULA (NOLOCK)
                                WHERE ANO = @ANO
									and SIT_MATRICULA = @SIT_MATRICULA
									AND ALUNO = @ALUNO ";

                contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
                contextQuery.Parameters.Add("@SIT_MATRICULA", TechneDbType.T_SIT_MATRICULA, Matriculado);
                contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);

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

        public bool PossuiMatriculaAtivaPeriodoPor(DataContext contexto, string aluno, int ano, int periodo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM LY_MATRICULA (NOLOCK)
                                WHERE ANO = @ANO
                                    AND SEMESTRE = @PERIODO
									and SIT_MATRICULA = @SIT_MATRICULA
									AND ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
            contextQuery.Parameters.Add("@SIT_MATRICULA", TechneDbType.T_SIT_MATRICULA, Matriculado);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public bool PossuiMatriculaAtivaPeriodoPor(DataContext contexto, List<string> alunos, int ano, int periodo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;
            string matriculas = alunos.Aggregate((x, y) => x + ", " + y);

            contextQuery.Command = string.Format(@" SELECT COUNT(*) 
                                FROM LY_MATRICULA (NOLOCK)
                                WHERE ANO = @ANO
                                    AND SEMESTRE = @PERIODO
									and SIT_MATRICULA = @SIT_MATRICULA
									AND ALUNO IN ({0}) ", matriculas);

            contextQuery.Parameters.Add("@SIT_MATRICULA", TechneDbType.T_SIT_MATRICULA, Matriculado);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public bool PossuiMatriculaAtivaAnoPor(DataContext contexto, List<string> alunos, int ano)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            string matriculas = "'" + alunos.Aggregate((a, b) => a + "', '" + b) + "'";

            contextQuery.Command = string.Format(@" SELECT COUNT(*) 
                                FROM LY_MATRICULA (NOLOCK)
                                WHERE ANO = @ANO
									and SIT_MATRICULA = @SIT_MATRICULA
									AND ALUNO IN ({0}) ", matriculas);

            contextQuery.Parameters.Add("@SIT_MATRICULA", TechneDbType.T_SIT_MATRICULA, Matriculado);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public bool PossuiMatriculaAtivaPossiveisPeriodosPor(DataContext contexto, string aluno, decimal ano, decimal periodo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;
            string possiveisPeriodos = Utils.RecuperaPossiveisPeriodosCompleto(Convert.ToInt32(periodo));

            contextQuery.Command = string.Format(@" SELECT COUNT(*) 
                                FROM LY_MATRICULA (NOLOCK)
                                WHERE ANO = @ANO
                                    AND SEMESTRE IN ({0})
									and SIT_MATRICULA = @SIT_MATRICULA
									AND ALUNO = @ALUNO ", possiveisPeriodos);

            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
            contextQuery.Parameters.Add("@SIT_MATRICULA", TechneDbType.T_SIT_MATRICULA, Matriculado);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public static bool PossuiMatriculaRegularAtiva(decimal ano, string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                        FROM    LY_MATRICULA m
                                INNER JOIN dbo.LY_TURMA t ON m.DISCIPLINA = t.DISCIPLINA
                                                             AND m.TURMA = t.TURMA
                                                             AND m.ANO = t.ANO
                                                             AND m.SEMESTRE = t.SEMESTRE
                        WHERE   m.ANO = @ANO
                                AND M.ALUNO = @ALUNO
                                AND m.SIT_MATRICULA = @SIT_MATRICULA
                                AND t.OPTATIVAREFORCO = 'N'
                                AND ISNULL(T.ELETIVA,'N') = 'N'
                                AND ISNULL(m.DEPENDENCIA, 'N') = 'N'
                                AND ISNULL(m.EDUC_ESPECIAL, 'N') = 'N'
                                AND ISNULL(m.MAIS_EDUCACAO, 'N') = 'N'
                                AND ISNULL(m.CONCOMITANTE, 'N') = 'N' ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);

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
        }

        public bool PossuiMatriculaEspecialAtivaPor(DataContext ctx, int ano, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT  COUNT(*)
                            FROM    LY_MATRICULA M ( NOLOCK )
                                    INNER JOIN DBO.LY_TURMA T ( NOLOCK ) ON M.DISCIPLINA = T.DISCIPLINA
                                                                            AND M.TURMA = T.TURMA
                                                                            AND M.ANO = T.ANO
                                                                            AND M.SEMESTRE = T.SEMESTRE
                            WHERE   M.ANO = @ANO
                                    AND M.ALUNO = @ALUNO
                                    AND M.SIT_MATRICULA = @SIT_MATRICULA
                                    AND ( T.OPTATIVAREFORCO <> 'N'
                                      OR ISNULL(M.DEPENDENCIA, 'N') = 'S'
                                      OR ISNULL(M.EDUC_ESPECIAL, 'N') = 'S'
                                      OR ISNULL(M.MAIS_EDUCACAO, 'N') = 'S'
                                      OR ISNULL(M.CONCOMITANTE, 'N') = 'S'
                                    ) ";

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiMatriculaAtivaNaTurmaPorAluno(DataContext ctx, string aluno, string turma, decimal ano, decimal semestre)
        {
            bool possui = false;

            ContextQuery contextQuery = new ContextQuery
            {
                Command = @" SELECT  COUNT(*)
                                FROM    LY_MATRICULA
                                WHERE   TURMA = @TURMA
                                        AND ANO = @ANO
                                        AND SEMESTRE = @SEMESTRE
                                        AND ALUNO = @ALUNO
                                        AND SIT_MATRICULA = @SIT_MATRICULA "
            };

            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, semestre);
            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
            contextQuery.Parameters.Add("@SIT_MATRICULA", TechneDbType.T_SIT_MATRICULA, Matriculado);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public bool PossuiMatriculaPrincipalAtivaPor(DataContext ctx, string aluno, int ano, int semestre, string censo, string curso, string turno, int serie)
        {
            bool possui = false;

            ContextQuery contextQuery = new ContextQuery
            {
                Command = @" SELECT COUNT(*) 
                             FROM   LY_MATRICULA M (NOLOCK) 
                                   INNER JOIN LY_TURMA T (NOLOCK) 
                                           ON M.ANO = T.ANO 
                                              AND M.SEMESTRE = T.SEMESTRE 
                                              AND M.TURMA = T.TURMA 
                                              AND M.DISCIPLINA = T.DISCIPLINA 
                             WHERE  ALUNO = @ALUNO 
                                    AND SIT_MATRICULA = @SIT_MATRICULA 
                                    AND M.ANO = @ANO 
                                    AND M.SEMESTRE = @SEMESTRE 
                                    AND FACULDADE = @CENSO 
                                    AND CURSO = @CURSO 
                                    AND TURNO = @TURNO 
                                    AND SERIE = @SERIE
                                    AND ( M.DEPENDENCIA IS NULL
                                          OR M.DEPENDENCIA = 'N'
                                        )
                                    AND ( M.CONCOMITANTE IS NULL
                                          OR M.CONCOMITANTE = 'N'
                                        )
                                    AND ( M.EDUC_ESPECIAL IS NULL
                                          OR M.EDUC_ESPECIAL = 'N'
                                        )
                                    AND ( M.MAIS_EDUCACAO IS NULL
                                          OR M.MAIS_EDUCACAO = 'N'
                                        )
                                    AND T.OPTATIVAREFORCO = 'N'
                                    AND ISNULL(T.ELETIVA,'N') = 'N' "
            };

            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
            contextQuery.Parameters.Add("@SIT_MATRICULA", TechneDbType.T_SIT_MATRICULA, Matriculado);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, semestre);
            contextQuery.Parameters.Add("@CENSO", TechneDbType.T_CODIGO, censo);
            contextQuery.Parameters.Add("@CURSO", TechneDbType.T_CODIGO, curso);
            contextQuery.Parameters.Add("@TURNO", TechneDbType.T_CODIGO, turno);
            contextQuery.Parameters.Add("@SERIE", TechneDbType.T_NUMERO_PEQUENO, serie);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public string ObtemTurmaMatriculaPrincipalAtivaPor(DataContext ctx, List<string> alunos, int ano, int semestre, string censo, string curso, string turno, int serie)
        {
            string matriculas = alunos.Aggregate((x, y) => x + ", " + y);
            string turma;

            ContextQuery contextQuery = new ContextQuery
            {
                Command = string.Format(@" SELECT TOP 1 M.TURMA 
                             FROM   LY_MATRICULA M (NOLOCK) 
                                   INNER JOIN LY_TURMA T (NOLOCK) 
                                           ON M.ANO = T.ANO 
                                              AND M.SEMESTRE = T.SEMESTRE 
                                              AND M.TURMA = T.TURMA 
                                              AND M.DISCIPLINA = T.DISCIPLINA 
                             WHERE  ALUNO IN ({0})
                                    AND SIT_MATRICULA = @SIT_MATRICULA 
                                    AND M.ANO = @ANO 
                                    AND M.SEMESTRE = @SEMESTRE 
                                    AND FACULDADE = @CENSO 
                                    AND CURSO = @CURSO 
                                    AND TURNO = @TURNO 
                                    AND SERIE = @SERIE
                                    AND ( M.DEPENDENCIA IS NULL
                                          OR M.DEPENDENCIA = 'N'
                                        )
                                    AND ( M.CONCOMITANTE IS NULL
                                          OR M.CONCOMITANTE = 'N'
                                        )
                                    AND ( M.EDUC_ESPECIAL IS NULL
                                          OR M.EDUC_ESPECIAL = 'N'
                                        )
                                    AND ( M.MAIS_EDUCACAO IS NULL
                                          OR M.MAIS_EDUCACAO = 'N'
                                        )
                                    AND T.OPTATIVAREFORCO = 'N'
                                    AND ISNULL(T.ELETIVA,'N') = 'N' ", matriculas)
            };

            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, matriculas);
            contextQuery.Parameters.Add("@SIT_MATRICULA", TechneDbType.T_SIT_MATRICULA, Matriculado);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, semestre);
            contextQuery.Parameters.Add("@CENSO", TechneDbType.T_CODIGO, censo);
            contextQuery.Parameters.Add("@CURSO", TechneDbType.T_CODIGO, curso);
            contextQuery.Parameters.Add("@TURNO", TechneDbType.T_CODIGO, turno);
            contextQuery.Parameters.Add("@SERIE", TechneDbType.T_NUMERO_PEQUENO, serie);

            turma = ctx.GetReturnValue<string>(contextQuery);

            return turma;
        }

        public bool PossuiMatriculaPor(DataContext contexto, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @"  SELECT  COUNT(*)
                        FROM    LY_MATRICULA (NOLOCK)
                        WHERE   ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", aluno);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public bool PossuiMatricula(string aluno, string disciplina, string turma, decimal ano, decimal semestre)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT  COUNT(*)
                        FROM    LY_MATRICULA
                        WHERE   ALUNO = @ALUNO
                                AND DISCIPLINA = @DISCIPLINA
                                AND TURMA = @TURMA
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE "
                };

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);

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
        }

        public bool PossuiMatriculaCanceladaPor(string aluno, string turma, decimal ano, decimal semestre)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT  COUNT(*)
                        FROM    LY_MATRICULA
                        WHERE   ALUNO = @ALUNO
                                AND TURMA = @TURMA
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE 
                                AND SIT_MATRICULA <> @SIT_MATRICULA "
                };

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);

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
        }

        public bool PossuiMatriculaEletivaCanceladaPor(string aluno, string turma, decimal ano, decimal semestre, int grupo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT  COUNT(*)
                        FROM    LY_MATRICULA M
                                INNER JOIN DBO.LY_TURMA T ON M.DISCIPLINA = T.DISCIPLINA
									                                        AND M.TURMA = T.TURMA
									                                        AND M.ANO = T.ANO
									                                        AND M.SEMESTRE = T.SEMESTRE
                                 INNER JOIN LY_DISCIPLINA D ON ISNULL(T.DISCIPLINA_MULTIPLA, T.DISCIPLINA) = D.DISCIPLINA
                        WHERE   M.ALUNO = @ALUNO
                                AND M.TURMA = @TURMA
                                AND M.ANO = @ANO
                                AND M.SEMESTRE = @SEMESTRE 
                                AND M.SIT_MATRICULA <> @SIT_MATRICULA 
                                AND D.GRUPO = @GRUPO
                                AND ISNULL(T.ELETIVA, 'N') = 'S' "
                };

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@GRUPO", grupo);
                contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);

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
        }

        public bool EhMatriculaRegularConcluinte(string aluno, int ano, int periodo, string turma)
        {
            bool ehMatriculaRegularConcluinte;

            ContextQuery contextQuery = new ContextQuery(
                     @" SELECT  COUNT(*)
                        FROM    DBO.LY_MATRICULA M
                                INNER JOIN LY_TURMA T ON M.DISCIPLINA = T.DISCIPLINA
                                                         AND M.TURMA = T.TURMA
                                                         AND M.ANO = T.ANO
                                                         AND M.SEMESTRE = T.SEMESTRE
                                INNER JOIN DBO.LY_SERIE S ON T.CURRICULO = S.CURRICULO
                                                             AND T.CURSO = S.CURSO
                                                             AND T.SERIE = S.SERIE
                                                             AND T.TURNO = S.TURNO
                                INNER JOIN LY_CURSO C ON C.CURSO = T.CURSO
                        WHERE   M.ANO = @ANO
                                AND M.SEMESTRE = @SEMESTRE
                                AND ( M.DEPENDENCIA IS NULL
                                      OR M.DEPENDENCIA = 'N'
                                    )
                                AND ( M.CONCOMITANTE IS NULL
                                      OR M.CONCOMITANTE = 'N'
                                    )
                                AND ( M.EDUC_ESPECIAL IS NULL
                                      OR M.EDUC_ESPECIAL = 'N'
                                    )
                                AND ( M.MAIS_EDUCACAO IS NULL
                                      OR M.MAIS_EDUCACAO = 'N'
                                    )
                                AND T.OPTATIVAREFORCO = 'N'
                                AND ISNULL(T.ELETIVA,'N') = 'N'
                                AND M.ALUNO = @ALUNO
                                AND M.TURMA = @TURMA
                                AND M.SIT_MATRICULA = @SIT_MATRICULA 
                                AND ((ANO_SERIE_CONCLUINTE = 'S' AND TIPO IN ( 3, 4 ) OR (T.SERIE = 9) OR (T.CURSO = '0092.30' AND T.SERIE = 4 )))");

            contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", periodo);
            contextQuery.Parameters.Add("@TURMA", turma);

            ehMatriculaRegularConcluinte = (ExecutarFuncao<int>(contextQuery) > 0);

            return ehMatriculaRegularConcluinte;
        }

        public DataTable ListaTurmasParaLiberacaoConfirmacaoPor(string aluno, int ano, int periodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable turmas = null;
            string periodosPossiveis = string.Empty;

            if (periodo == 0)
            {
                periodosPossiveis = "0, 1, 2";
            }
            else if (periodo == 1)
            {
                periodosPossiveis = "0, 1";
            }
            else
            {
                periodosPossiveis = "0, 2";
            }

            try
            {
                contextQuery.Command = string.Format(@" SELECT  DISTINCT
                        t.FACULDADE ,
                        t.FACULDADE + ' - ' + E.NOME_COMP AS UNIDADE ,
                        t.CURSO ,
                        MD.DESCRICAO + ' / ' + TC.DESCRICAO + ' / ' + C.NOME AS MOD_SEG_CURSO ,
                        t.SERIE ,
                        t.TURNO ,
                        t.TURMA ,
                        CASE WHEN ISNULL(m.DEPENDENCIA, 'N') = 'S' THEN 'Dependência'
                             WHEN ISNULL(m.MAIS_EDUCACAO, 'N') = 'S' THEN 'Mais Educação'
                             WHEN ISNULL(m.EDUC_ESPECIAL, 'N') = 'S' THEN 'Educação Especial'
                             WHEN ISNULL(m.CONCOMITANTE, 'N') = 'S'
                             THEN 'Ensino Profissional Concomitante'
                             WHEN t.OPTATIVAREFORCO <> 'N' THEN 'Optativa / Reforço'
                             ELSE 'Principal'
                        END TIPO_MATRICULA
                FROM    dbo.LY_MATRICULA m
                        INNER JOIN dbo.LY_TURMA t ON m.DISCIPLINA = t.DISCIPLINA
                                                     AND m.TURMA = t.TURMA
                                                     AND m.ANO = t.ANO
                                                     AND m.SEMESTRE = t.SEMESTRE
                        INNER JOIN dbo.LY_UNIDADE_ENSINO E ON t.FACULDADE = E.UNIDADE_ENS
                        INNER JOIN dbo.LY_CURSO C ON t.CURSO = C.CURSO
                        INNER JOIN dbo.LY_MODALIDADE_CURSO MD ON C.MODALIDADE = MD.MODALIDADE
                        INNER JOIN dbo.LY_TIPO_CURSO TC ON C.TIPO = TC.TIPO
                WHERE   ALUNO = @ALUNO
                        AND T.ANO = @ANO
                        AND T.SEMESTRE IN ( {0} )
                        AND SIT_MATRICULA = @SIT_MATRICULA ", periodosPossiveis);

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);

                turmas = ctx.GetDataTable(contextQuery);
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

            return turmas;
        }

        public void LiberaMatriculasEmPeridosPossiveisPor(DataContext ctx, DadosLiberacaoConfirmacao liberacao, string periodosPossiveis)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = string.Format(@"  UPDATE LY_MATRICULA
                                 SET    SIT_MATRICULA = @SIT_CANCELADO ,
                                        STAMP_ATUALIZACAO = GETDATE() ,
                                        DT_ULTALT = GETDATE() ,
                                        MATRICULA = @MATRICULA
                                 WHERE  SIT_MATRICULA = @SIT_MATRICULADO
                                        AND ALUNO = @ALUNO
                                        AND ANO = @ANO
                                        AND SEMESTRE IN ( {0} ) ", periodosPossiveis);

                contextQuery.Parameters.Add("@SIT_CANCELADO", Cancelado);
                contextQuery.Parameters.Add("@MATRICULA", liberacao.MatriculaResponsavel);
                contextQuery.Parameters.Add("@SIT_MATRICULADO", Matriculado);
                contextQuery.Parameters.Add("@ALUNO", liberacao.Aluno);
                contextQuery.Parameters.Add("@ANO", liberacao.Ano);

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

        public bool EhMatriculaProgressaoParcial(DataContext ctx, string aluno, int ano, int periodo, string turma)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT  COUNT(*)
                                FROM    LY_MATRICULA
                                WHERE   ALUNO = @ALUNO                               
                                        AND ANO = @ANO
                                        AND SEMESTRE = @SEMESTRE
                                        AND SIT_MATRICULA = @SIT_MATRICULA 
                                        AND TURMA = @TURMA
                                        AND DEPENDENCIA = 'S' ";

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", periodo);
            contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
            contextQuery.Parameters.Add("@TURMA", turma);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public static int ObtemQuantidadeProgressaoParcialPor(string aluno, decimal ano, decimal periodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;

            contextQuery.Command = @" SELECT  COUNT(*) as QUANTIDADE
                                FROM    LY_MATRICULA
                                WHERE   ALUNO = @ALUNO                               
                                        AND ANO = @ANO
                                        AND SEMESTRE = @SEMESTRE
                                        AND SIT_MATRICULA = @SIT_MATRICULA 
                                        AND DEPENDENCIA = 'S' ";

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", periodo);
            contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);

            reader = ctx.GetDataReader(contextQuery);

            while (reader.Read())
            {
                retorno = Convert.ToInt32(reader["QUANTIDADE"]);
            }

            if (reader != null)
            {
                reader.Close();
            }

            return retorno;
        }

        public ValidacaoDados ValidaMatriculaPrincipal(LyMatricula matricula, LyTurma turma, string tipoCurso, string tipoEnsinoProfissionalizante, string necessidadeEspecial)
        {
            List<string> mensagens = new List<string>();
            RN.Pessoa rnPessoa = new Pessoa();
            RN.Turma rnTurma = new Turma();
            DataContext contexto = null;
            ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            RN.ControleVaga rnControleVaga = new ControleVaga();
            RN.HistMatricula rnHistMatricula = new HistMatricula();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (matricula == null || turma == null)
            {
                return validacaoDados;
            }

            if (matricula.Aluno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo TURMA é obrigatório!");
            }

            if (matricula.Ano <= 0 || turma.Ano <= 0)
            {
                mensagens.Add("O campo ANO é obrigatório!");
            }

            if (matricula.Semestre < 0 || turma.Semestre < 0)
            {
                mensagens.Add("O campo PERIODO é obrigatório!");
            }

            if (matricula.Turma.IsNullOrEmptyOrWhiteSpace() || turma.Turma.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo TURMA é obrigatório!");
            }

            if (matricula.Matricula.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo USUARIO RESPONSAVEL é obrigatório!");
            }

            if (turma.Faculdade.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo UNIDADE ENSINO DA TURMA é obrigatório!");
            }

            if (turma.Curso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo CURSO DA TURMA é obrigatório!");
            }

            if (turma.Turno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo TURNO DA TURMA é obrigatório!");
            }

            if (turma.Serie <= 0)
            {
                mensagens.Add("O campo SERIE DA TURMA é obrigatório!");
            }

            if (tipoCurso == "Especial")
            {
                if (necessidadeEspecial == "Não possui.")
                {
                    mensagens.Add("Para escolher um curso de educação especial, informar o tipo de necessidade especial do aluno(a) na aba 'Dados Pessoais'.");
                }
            }
            else if (tipoCurso == "Concomitante/Subsequente")
            {
                if (string.IsNullOrEmpty(tipoEnsinoProfissionalizante))
                {
                    mensagens.Add("Para escolher um curso Concomitante/Subsequente, deverá escolher o Tipo de Ensino Profissionalizante.");
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verificar se já existe matricula ativa
                    if (EhMatriculaAtiva(contexto, matricula.Aluno, Convert.ToInt32(turma.Ano), Convert.ToInt32(turma.Semestre)))
                    {
                        mensagens.Add("Já existe matricula ativa para o aluno / ano / periodo.");
                        validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
                        return validacaoDados;
                    }

                    //Verificar se aluno já possui situação final para a turma/ano/periodo
                    if (RN.SituacaoFinalAluno.ExisteSituacaoFinalPor(contexto, matricula.Aluno, turma.Ano, turma.Semestre, turma.Turma))
                    {
                        mensagens.Add("Este aluno já possui situação final para este ano/periodo/turma.");
                        validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
                        return validacaoDados;
                    }

                    //Verificar se turma continua Ativa                    
                    if (!rnTurma.EhTurmaAbertaPor(contexto, Convert.ToInt32(turma.Ano), Convert.ToInt32(turma.Semestre), turma.Turma))
                    {
                        mensagens.Add("Não foram localizados os dados desta turma com situação ativa.");
                        validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
                        return validacaoDados;
                    }

                    //Verifica se Aluno possui confirmação confirmada com os dados dados turma escolhida
                    if (!rnConfirmacaoMatricula.PossuiConfirmacaoMatriculaConfirmadaPor(contexto, matricula.Aluno, turma.Curso, turma.Serie, turma.Turno, Convert.ToInt32(turma.Ano), Convert.ToInt32(turma.Semestre), turma.Faculdade))
                    {
                        mensagens.Add("Não existe confirmação de matrícula para o aluno no ano, período, escola, curso, série e turno.");
                        validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
                        return validacaoDados;
                    }

                    //Verifica se é a mesma turma de progressao parcial
                    if (this.EhMatriculaProgressaoParcial(contexto, matricula.Aluno, Convert.ToInt32(turma.Ano), Convert.ToInt32(turma.Semestre), turma.Turma))
                    {
                        mensagens.Add("A TURMA não pode ser igual a turma de progressão parcial do aluno.");
                    }
                    else
                    {
                        //Verifica a mesma turma já foi finalizada anteriormente
                        if (rnHistMatricula.EhMatriculaHistoricoAtivaPor(contexto, matricula.Aluno, Convert.ToInt32(turma.Ano), Convert.ToInt32(turma.Semestre), turma.Turma))
                        {
                            mensagens.Add("Já existe matricula ativa no histórico para este aluno / ano / periodo / turma.");
                        }
                    }

                    //Verifica se a turma tem vaga
                    var vagas = rnTurma.ObtemVagasPrincipalLiberadasTurmaPor(contexto, Convert.ToInt32(turma.Ano), Convert.ToInt32(turma.Semestre), turma.Turma);
                    if (vagas <= 0)
                    {
                        mensagens.Add("A capacidade da turma desejada não comporta mais alunos.");
                    }
                }
                catch (Exception ex)
                {
                    string mensagem = string.Empty;
                    contexto.Abandon();
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine,
                            Convert.ToString(ex.Message));
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

        public ValidacaoDados ValidaMatriculaEletiva(List<DTOs.DadosEnturmacaoEletiva> dadosEnturmacaoEletiva, string turmaBusca, int anoBusca, int periodoBusca, bool porAluno, bool enturmar)
        {
            List<string> mensagens = new List<string>();
            RN.Turma rnTurma = new Turma();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosEnturmacaoEletiva == null)
            {
                return validacaoDados;
            }

            if (!enturmar)
            {
                mensagens.Add("Não foram identificadas novas enturmações!");
            }

            if (dadosEnturmacaoEletiva.Count == 0)
            {
                mensagens.Add("Não foram encontrados alunos na turma!");
            }

            if (anoBusca <= 0)
            {
                mensagens.Add("O campo ANO DA BUSCA é obrigatório!");
            }

            if (periodoBusca < 0)
            {
                mensagens.Add("O campo PERIODO DA BUSCA é obrigatório!");
            }

            if (!porAluno)
            {
                if (turmaBusca.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("O campo TURMA DA BUSCA é obrigatório!");
                }
            }

            foreach (DadosEnturmacaoEletiva enturmacaoEletiva in dadosEnturmacaoEletiva)
            {
                if (enturmacaoEletiva.Aluno.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("O campo TURMA é obrigatório!");
                }

                if (enturmacaoEletiva.Ano <= 0)
                {
                    mensagens.Add("O campo ANO é obrigatório!");
                }

                if (enturmacaoEletiva.Periodo < 0)
                {
                    mensagens.Add("O campo PERIODO é obrigatório!");
                }
                /*
                 *  //RETIRADA A REGRA POR FALTA DE DEFINIÇÃO DA AREA RESPONSAVEL
                if (enturmacaoEletiva.TurmaEletiva1.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add(enturmacaoEletiva.Nome + " - O campo TURMA DA ELETIVA 1 é obrigatório!");
                }

                if (enturmacaoEletiva.TurmaEletiva2.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add(enturmacaoEletiva.Nome + " - O campo TURMA DA ELETIVA 2 é obrigatório!");
                }

                if (enturmacaoEletiva.TurmaEletiva3.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add(enturmacaoEletiva.Nome + " - O campo TURMA DA ELETIVA 3 é obrigatório!");
                }
                */
                if (enturmacaoEletiva.UsuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("O campo USUARIO RESPONSAVEL é obrigatório!");
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (porAluno)
                    {
                        if (dadosEnturmacaoEletiva.Count > 1)
                        {
                            mensagens.Add("Para enturmação por aluno apenas pode ser informado 1 aluno de cada vez.");
                        }
                    }
                    else
                    {
                        //Busca quantidade de alunos na turma referncia
                        int qtdeAlunos = this.RetornaQuantidadeMatriculadosPor(contexto, turmaBusca, anoBusca, periodoBusca);

                        //Verifica se todos os alunos matriculados na turma referencia estão na lista               
                        if (qtdeAlunos != dadosEnturmacaoEletiva.Count)
                        {
                            mensagens.Add("Ainda existem alunos a serem matriculados.");
                        }
                    }

                    //percorre alunos
                    foreach (DadosEnturmacaoEletiva enturmacaoEletiva in dadosEnturmacaoEletiva)
                    {
                        enturmacaoEletiva.PossuiEletiva1 = false;
                        enturmacaoEletiva.PossuiEletiva2 = false;
                        enturmacaoEletiva.PossuiEletiva3 = false;

                        //Verificar se já existe outra matricula ativa na eletiva 1
                        string turmaEletiva1 = this.ObtemMatriculaEletivaPor(contexto, enturmacaoEletiva.Aluno, enturmacaoEletiva.Ano, enturmacaoEletiva.Periodo, 1);
                        if (!turmaEletiva1.IsNullOrEmptyOrWhiteSpace())
                        {
                            if (turmaEletiva1 == enturmacaoEletiva.TurmaEletiva1)
                            {
                                enturmacaoEletiva.PossuiEletiva1 = true;
                            }
                            else
                            {
                                mensagens.Add(string.Format("Já existe outra eletiva 1 ativa para o aluno {0} / ano / periodo.", enturmacaoEletiva.Aluno));
                            }
                        }

                        //Verificar se já existe outra matricula ativa na eletiva 2
                        string turmaEletiva2 = this.ObtemMatriculaEletivaPor(contexto, enturmacaoEletiva.Aluno, enturmacaoEletiva.Ano, enturmacaoEletiva.Periodo, 2);
                        if (!turmaEletiva2.IsNullOrEmptyOrWhiteSpace())
                        {
                            if (turmaEletiva2 == enturmacaoEletiva.TurmaEletiva2)
                            {
                                enturmacaoEletiva.PossuiEletiva2 = true;
                            }
                            else
                            {
                                mensagens.Add(string.Format("Já existe outra eletiva 2 ativa para o aluno {0} / ano / periodo.", enturmacaoEletiva.Aluno));
                            }
                        }

                        //Verificar se já existe outramatricula ativa na eletiva 3
                        string turmaEletiva3 = this.ObtemMatriculaEletivaPor(contexto, enturmacaoEletiva.Aluno, enturmacaoEletiva.Ano, enturmacaoEletiva.Periodo, 3);
                        if (!turmaEletiva3.IsNullOrEmptyOrWhiteSpace())
                        {
                            if (turmaEletiva3 == enturmacaoEletiva.TurmaEletiva3)
                            {
                                enturmacaoEletiva.PossuiEletiva3 = true;
                            }
                            else
                            {
                                mensagens.Add(string.Format("Já existe outra eletiva 3 ativa para o aluno {0} / ano / periodo.", enturmacaoEletiva.Aluno));
                            }
                        }
                    }

                    //percorre turmas eletivas 1
                    foreach (string turmaEletiva in dadosEnturmacaoEletiva.Select(x => x.TurmaEletiva1).Distinct())
                    {
                        if (turmaEletiva != null)
                        {
                            int ano = dadosEnturmacaoEletiva.Where(x => x.TurmaEletiva1 == turmaEletiva).Select(x => x.Ano).First();
                            int periodo = dadosEnturmacaoEletiva.Where(x => x.TurmaEletiva1 == turmaEletiva).Select(x => x.Periodo).First();
                            int qtdeAlunos = dadosEnturmacaoEletiva.Where(x => x.TurmaEletiva1 == turmaEletiva && !x.PossuiEletiva1).Count();

                            //Verificar se a turma da eletiva 1 continua Ativa                    
                            if (!rnTurma.EhTurmaAbertaPor(contexto, ano, periodo, turmaEletiva))
                            {
                                mensagens.Add(string.Format("Não foram localizados os dados da eletiva 1, turma {0}, com situação ativa.", turmaEletiva));
                            }

                            //Verifica se a turma eletiva 1 pertence é eletiva
                            if (!rnTurma.EhEletivaAbertaPor(contexto, ano, periodo, turmaEletiva))
                            {
                                mensagens.Add(string.Format("A eletiva 1, turma {0}, não está configurada como eletiva e aberta.", turmaEletiva));
                            }
                            else if (qtdeAlunos > 0)
                            {
                                //Verifica se a turma eletiva 1 tem vaga
                                var vagasEletiva1 = rnTurma.ObtemVagasEletivaLiberadasTurmaPor(contexto, ano, periodo, turmaEletiva, 1);
                                if (vagasEletiva1 < qtdeAlunos)
                                {
                                    mensagens.Add(string.Format("A capacidade da eletiva 1, turma {0}, desejada não comporta os {1} alunos alocados para ela.", turmaEletiva, qtdeAlunos));
                                }
                            }
                        }
                    }

                    //percorre turmas eletivas 2
                    foreach (string turmaEletiva in dadosEnturmacaoEletiva.Select(x => x.TurmaEletiva2).Distinct())
                    {
                        if (turmaEletiva != null)
                        {
                            int ano = dadosEnturmacaoEletiva.Where(x => x.TurmaEletiva2 == turmaEletiva).Select(x => x.Ano).First();
                            int periodo = dadosEnturmacaoEletiva.Where(x => x.TurmaEletiva2 == turmaEletiva).Select(x => x.Periodo).First();
                            int qtdeAlunos = dadosEnturmacaoEletiva.Where(x => x.TurmaEletiva2 == turmaEletiva && !x.PossuiEletiva2).Count();

                            //Verificar se a turma da eletiva 2 continua Ativa                    
                            if (!rnTurma.EhTurmaAbertaPor(contexto, ano, periodo, turmaEletiva))
                            {
                                mensagens.Add(string.Format("Não foram localizados os dados da eletiva 2, turma {0}, com situação ativa.", turmaEletiva));
                            }

                            //Verifica se a turma eletiva 2 pertence é eletiva
                            if (!rnTurma.EhEletivaAbertaPor(contexto, ano, periodo, turmaEletiva))
                            {
                                mensagens.Add(string.Format("A eletiva 2, turma {0}, não está configurada como eletiva e aberta.", turmaEletiva));
                            }
                            else if (qtdeAlunos > 0)
                            {
                                //Verifica se a turma eletiva 2 tem vaga
                                var vagasEletiva2 = rnTurma.ObtemVagasEletivaLiberadasTurmaPor(contexto, ano, periodo, turmaEletiva, 2);
                                if (vagasEletiva2 < qtdeAlunos)
                                {
                                    mensagens.Add(string.Format("A capacidade da eletiva 2, turma {0}, desejada não comporta os {1} alunos alocados para ela.", turmaEletiva, qtdeAlunos));
                                }
                            }
                        }
                    }

                    //percorre turmas eletivas 3
                    foreach (string turmaEletiva in dadosEnturmacaoEletiva.Select(x => x.TurmaEletiva3).Distinct())
                    {
                        if (turmaEletiva != null)
                        {
                            int ano = dadosEnturmacaoEletiva.Where(x => x.TurmaEletiva3 == turmaEletiva).Select(x => x.Ano).First();
                            int periodo = dadosEnturmacaoEletiva.Where(x => x.TurmaEletiva3 == turmaEletiva).Select(x => x.Periodo).First();
                            int qtdeAlunos = dadosEnturmacaoEletiva.Where(x => x.TurmaEletiva3 == turmaEletiva && !x.PossuiEletiva3).Count();

                            //Verificar se a turma da eletiva 3 continua Ativa                    
                            if (!rnTurma.EhTurmaAbertaPor(contexto, ano, periodo, turmaEletiva))
                            {
                                mensagens.Add(string.Format("Não foram localizados os dados da eletiva 3, turma {0}, com situação ativa.", turmaEletiva));
                            }

                            //Verifica se a turma eletiva 3 pertence é eletiva
                            if (!rnTurma.EhEletivaAbertaPor(contexto, ano, periodo, turmaEletiva))
                            {
                                mensagens.Add(string.Format("A eletiva 3, turma {0}, não está configurada como eletiva e aberta.", turmaEletiva));
                            }
                            else if (qtdeAlunos > 0)
                            {
                                //Verifica se a turma eletiva 3 tem vaga
                                var vagasEletiva3 = rnTurma.ObtemVagasEletivaLiberadasTurmaPor(contexto, ano, periodo, turmaEletiva, 3);
                                if (vagasEletiva3 < qtdeAlunos)
                                {
                                    mensagens.Add(string.Format("A capacidade da eletiva 3, turma {0}, desejada não comporta os {1} alunos alocados para ela.", turmaEletiva, qtdeAlunos));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string mensagem = string.Empty;
                    contexto.Abandon();
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine,
                            Convert.ToString(ex.Message));
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

        private int RetornaQuantidadeMatriculadosPor(DataContext contexto, string turma, int ano, int periodo)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT COUNT(DISTINCT ALUNO) AS QTDE
                        FROM    LY_MATRICULA M
                        INNER JOIN LY_TURMA T ON M.DISCIPLINA = T.DISCIPLINA
                                                             AND M.TURMA = T.TURMA
                                                             AND M.ANO = T.ANO
                                                             AND M.SEMESTRE = T.SEMESTRE
                        WHERE   M.TURMA = @TURMA
                                AND M.ANO = @ANO
                                AND M.SEMESTRE = @SEMESTRE 
                                AND SIT_MATRICULA = @SIT_MATRICULA 
                                AND ISNULL(M.DEPENDENCIA, 'N') = 'N'
                                AND ISNULL(MAIS_EDUCACAO, 'N') = 'N' 
                                AND ISNULL(EDUC_ESPECIAL, 'N') = 'N' 
                                AND ISNULL(CONCOMITANTE, 'N') = 'N' 
                                AND ISNULL(T.ELETIVA,'N') = 'N'";

                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, periodo);
                contextQuery.Parameters.Add("@SIT_MATRICULA", SqlDbType.VarChar, Matriculado);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["QTDE"]);
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

        public void SalvaMatriculaEletiva(List<DTOs.DadosEnturmacaoEletiva> dadosEnturmacaoEletiva)
        {
            RN.Matgrade rnMatgrade = new Matgrade();
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingLock();

                //percorre alunos
                foreach (DadosEnturmacaoEletiva enturmacaoEletiva in dadosEnturmacaoEletiva)
                {
                    //Verifica se o aluno precisa ser matriculado na eletiva 1
                    if (!enturmacaoEletiva.PossuiEletiva1)
                    {
                        if (enturmacaoEletiva.TurmaEletiva1 != null)
                        {
                            //Verifica se o aluno já possui matricula cancelada na turma eletiva 1
                            if (this.PossuiMatriculaEletivaCanceladaPor(enturmacaoEletiva.Aluno, enturmacaoEletiva.TurmaEletiva1, enturmacaoEletiva.Ano, enturmacaoEletiva.Periodo, 1))
                            {
                                //Atualiza
                                this.AtivaMatriculaEletiva(contexto, enturmacaoEletiva.Aluno, enturmacaoEletiva.Ano, enturmacaoEletiva.Periodo, enturmacaoEletiva.TurmaEletiva1, 1);
                            }
                            else
                            {
                                //Insere Eletiva 1
                                this.InsereMatriculaEletiva(contexto, enturmacaoEletiva.Aluno, enturmacaoEletiva.Ano, enturmacaoEletiva.Periodo, enturmacaoEletiva.TurmaEletiva1, enturmacaoEletiva.UsuarioResponsavel, 1);
                            }

                            //Verifica se não possui matgrade ativa
                            if (!rnMatgrade.PossuiMatgradeAtivaPor(enturmacaoEletiva.Aluno, enturmacaoEletiva.TurmaEletiva1, enturmacaoEletiva.Ano, enturmacaoEletiva.Periodo))
                            {
                                //Verifica se o aluno já possui matgrade cancelada na turma eletiva 1
                                if (rnMatgrade.PossuiMatgradeCanceladaPor(enturmacaoEletiva.Aluno, enturmacaoEletiva.TurmaEletiva1, enturmacaoEletiva.Ano, enturmacaoEletiva.Periodo))
                                {
                                    //Atualiza
                                    rnMatgrade.AtivaMatgrade(contexto, enturmacaoEletiva.Aluno, enturmacaoEletiva.Ano, enturmacaoEletiva.Periodo, enturmacaoEletiva.TurmaEletiva1);
                                }
                                else
                                {
                                    //Insere Eletiva 1
                                    rnMatgrade.InsereMatgrade(contexto, enturmacaoEletiva.Aluno, enturmacaoEletiva.Ano, enturmacaoEletiva.Periodo, enturmacaoEletiva.TurmaEletiva1);
                                }
                            }
                        }
                    }

                    //Verifica se o aluno precisa ser matriculado na eletiva 2
                    if (!enturmacaoEletiva.PossuiEletiva2)
                    {
                        if (enturmacaoEletiva.TurmaEletiva2 != null)
                        {
                            //Verifica se o aluno já possui matricula cancelada na turma eletiva 2
                            if (this.PossuiMatriculaEletivaCanceladaPor(enturmacaoEletiva.Aluno, enturmacaoEletiva.TurmaEletiva2, enturmacaoEletiva.Ano, enturmacaoEletiva.Periodo, 2))
                            {
                                //Atualiza
                                this.AtivaMatriculaEletiva(contexto, enturmacaoEletiva.Aluno, enturmacaoEletiva.Ano, enturmacaoEletiva.Periodo, enturmacaoEletiva.TurmaEletiva2, 2);
                            }
                            else
                            {
                                //Insere Eletiva 2
                                this.InsereMatriculaEletiva(contexto, enturmacaoEletiva.Aluno, enturmacaoEletiva.Ano, enturmacaoEletiva.Periodo, enturmacaoEletiva.TurmaEletiva2, enturmacaoEletiva.UsuarioResponsavel, 2);
                            }

                            //Verifica se não possui matgrade ativa
                            if (!rnMatgrade.PossuiMatgradeAtivaPor(enturmacaoEletiva.Aluno, enturmacaoEletiva.TurmaEletiva2, enturmacaoEletiva.Ano, enturmacaoEletiva.Periodo))
                            {
                                //Verifica se o aluno já possui matgrade cancelada na turma eletiva 2
                                if (rnMatgrade.PossuiMatgradeCanceladaPor(enturmacaoEletiva.Aluno, enturmacaoEletiva.TurmaEletiva2, enturmacaoEletiva.Ano, enturmacaoEletiva.Periodo))
                                {
                                    //Atualiza
                                    rnMatgrade.AtivaMatgrade(contexto, enturmacaoEletiva.Aluno, enturmacaoEletiva.Ano, enturmacaoEletiva.Periodo, enturmacaoEletiva.TurmaEletiva2);
                                }
                                else
                                {
                                    //Insere Eletiva 2
                                    rnMatgrade.InsereMatgrade(contexto, enturmacaoEletiva.Aluno, enturmacaoEletiva.Ano, enturmacaoEletiva.Periodo, enturmacaoEletiva.TurmaEletiva2);
                                }
                            }
                        }
                    }

                    //Verifica se o aluno precisa ser matriculado na eletiva 3
                    if (!enturmacaoEletiva.PossuiEletiva3)
                    {
                        if (enturmacaoEletiva.TurmaEletiva3 != null)
                        {
                            //Verifica se o aluno já possui matricula cancelada na turma eletiva 3
                            if (this.PossuiMatriculaEletivaCanceladaPor(enturmacaoEletiva.Aluno, enturmacaoEletiva.TurmaEletiva3, enturmacaoEletiva.Ano, enturmacaoEletiva.Periodo, 3))
                            {
                                //Atualiza
                                this.AtivaMatriculaEletiva(contexto, enturmacaoEletiva.Aluno, enturmacaoEletiva.Ano, enturmacaoEletiva.Periodo, enturmacaoEletiva.TurmaEletiva3, 3);
                            }
                            else
                            {
                                //Insere Eletiva 3
                                this.InsereMatriculaEletiva(contexto, enturmacaoEletiva.Aluno, enturmacaoEletiva.Ano, enturmacaoEletiva.Periodo, enturmacaoEletiva.TurmaEletiva3, enturmacaoEletiva.UsuarioResponsavel, 3);
                            }

                            //Verifica se não possui matgrade ativa
                            if (!rnMatgrade.PossuiMatgradeAtivaPor(enturmacaoEletiva.Aluno, enturmacaoEletiva.TurmaEletiva3, enturmacaoEletiva.Ano, enturmacaoEletiva.Periodo))
                            {
                                //Verifica se o aluno já possui matgrade cancelada na turma eletiva 3
                                if (rnMatgrade.PossuiMatgradeCanceladaPor(enturmacaoEletiva.Aluno, enturmacaoEletiva.TurmaEletiva3, enturmacaoEletiva.Ano, enturmacaoEletiva.Periodo))
                                {
                                    //Atualiza
                                    rnMatgrade.AtivaMatgrade(contexto, enturmacaoEletiva.Aluno, enturmacaoEletiva.Ano, enturmacaoEletiva.Periodo, enturmacaoEletiva.TurmaEletiva3);
                                }
                                else
                                {
                                    //Insere Eletiva 3
                                    rnMatgrade.InsereMatgrade(contexto, enturmacaoEletiva.Aluno, enturmacaoEletiva.Ano, enturmacaoEletiva.Periodo, enturmacaoEletiva.TurmaEletiva3);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
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

        public void AtivaMatriculaEletiva(DataContext contexto, string aluno, int ano, int periodo, string turma, int grupo)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE  LY_MATRICULA
                                      SET     SIT_MATRICULA = 'Matriculado' ,
                                              DT_ULTALT = GETDATE() ,
                                              CONCOMITANTE = 'N',
                                              DEPENDENCIA = 'N',
                                              SERIE_REFERENCIA = NULL, 
                                              DISCIPLINA_REFERENCIA = NULL, 
                                              EDUC_ESPECIAL = 'N',
                                              MAIS_EDUCACAO = 'N',
                                              DT_MATRICULA = GETDATE()
                                      FROM LY_MATRICULA M
                                      INNER JOIN DBO.LY_TURMA T ON M.DISCIPLINA = T.DISCIPLINA
									                                        AND M.TURMA = T.TURMA
									                                        AND M.ANO = T.ANO
									                                        AND M.SEMESTRE = T.SEMESTRE
                                      INNER JOIN LY_DISCIPLINA D ON ISNULL(T.DISCIPLINA_MULTIPLA, T.DISCIPLINA) = D.DISCIPLINA  
                                      WHERE   ALUNO = @ALUNO
                                              AND M.TURMA = @TURMA
                                              AND M.ANO = @ANO
                                              AND M.SEMESTRE = @SEMESTRE 
                                              AND D.GRUPO = @GRUPO
                                              AND ISNULL(T.ELETIVA, 'N') = 'S' ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, periodo);
            contextQuery.Parameters.Add("@GRUPO", SqlDbType.Int, grupo);

            contexto.ApplyModifications(contextQuery);
        }

        public void InsereMatriculaEletiva(DataContext contexto, string aluno, int ano, int periodo, string turma, string usuarioResponsavel, int grupo)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT  INTO dbo.LY_MATRICULA
                                    (
                                        ALUNO,
                                        DISCIPLINA,
                                        TURMA,
                                        ANO,
                                        SEMESTRE,
                                        SIT_MATRICULA,
                                        DT_ULTALT,
                                        COBRANCA_SEP,
                                        DT_INSERCAO,
                                        DT_MATRICULA,
                                        MATRICULA
                                    )
                                SELECT DISTINCT 
                                        @ALUNO,
                                        t.DISCIPLINA,
                                        t.TURMA,
                                        t.ANO,
                                        t.SEMESTRE,
                                        @SIT_MATRICULA,
                                        GETDATE(),
                                        'N',
                                        GETDATE(),
                                        GETDATE(),
                                        @MATRICULA
                            FROM LY_TURMA T
                            INNER JOIN LY_DISCIPLINA d ON ISNULL(t.DISCIPLINA_MULTIPLA, t.DISCIPLINA) = d.DISCIPLINA
                            WHERE T.TURMA = @TURMA
                                AND T.ANO = @ANO
                                AND T.SEMESTRE = @SEMESTRE 
                                AND D.GRUPO = @GRUPO
								AND ISNULL(T.ELETIVA, 'N') = 'S' ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, periodo);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@SIT_MATRICULA", SqlDbType.VarChar, Matriculado);
            contextQuery.Parameters.Add("@GRUPO", SqlDbType.Int, grupo);

            contexto.ApplyModifications(contextQuery);
        }

        public void InsereMatriculaPrincipal(LyMatricula matricula, string tipoEnsinoProfissionalizante, decimal gradeId)
        {
            RN.Matgrade rnMatgrade = new Matgrade();
            RN.GradeTurma rnGradeTurma = new GradeTurma();
            RN.Turma rnTurma = new Turma();
            ICollection<LyTurma> turma = new List<LyTurma>();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            bool erroEnturmacao = false;

            try
            {
                //Insere matricula
                this.InsereMatricula(ctx, matricula);

                if (PossuiMatriculaAtivaNaTurmaPorAluno(ctx, matricula.Aluno, matricula.Turma, matricula.Ano, matricula.Semestre))
                {
                    //Insere matgrade e Cancela demais
                    rnMatgrade.InsereMatgradePrincipal(ctx, matricula.Aluno, gradeId);

                    //Atualizar dados de aluno e Confirmação de Matricula
                    this.AtualizaDadosAlunoConfirmacaoMatriculaPrincipal(ctx, matricula.Turma, matricula.Ano, matricula.Semestre, matricula.Aluno, tipoEnsinoProfissionalizante);
                }
                else
                {
                    erroEnturmacao = true;
                    turma = rnTurma.ObtemDisciplinasTurmaPor(ctx, Convert.ToInt32(matricula.Ano), Convert.ToInt32(matricula.Semestre), matricula.Turma);
                    throw new Exception(string.Format("Aluno não matriculado. Turma com situação {0} e com {1} disciplina(s). Entre em contato com o Administrador do Sistema repassando esta mensagem(Nova Matrícula) ou tente novamente mais tarde.", (turma.Select(x => x.Disciplina).Count() > 0 ? turma.Select(x => x.SitTurma).First() : "Não Encontrada"), turma.Select(x => x.Disciplina).Count().ToString()));
                }
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.") && !erroEnturmacao)
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else if (erroEnturmacao)
                {
                    mensagem = ex.Message;
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

        public void InsereMatricula(DataContext ctx, LyMatricula matricula)
        {
            //Verifica se Aluno já possaui a matricula de alguma disciplina como cancelada
            if (this.PossuiAlgumaDisciplinaCanceladaPorTurma(matricula))
            {
                //SE possui insere as disciplinas que não possuir e atualiza as que possuir
                this.InsereAtualizaDisciplinasAtivasPorTurma(ctx, matricula);
            }
            else
            {
                //Insere todas as disciplinas da turma
                this.InsereDisciplinasAtivasPorTurma(ctx, matricula);
            }
        }

        public void InsereDisciplinasAtivasPorTurma(DataContext ctx, LyMatricula matricula)
        {
            ContextQuery contextQuery = new ContextQuery();
            contextQuery.Command = @" INSERT INTO DBO.LY_MATRICULA 
                                    (ALUNO, 
                                     DISCIPLINA, 
                                     TURMA, 
                                     ANO, 
                                     SEMESTRE, 
                                     SIT_MATRICULA, 
                                     DT_ULTALT, 
                                     COBRANCA_SEP, 
                                     DT_INSERCAO, 
                                     DT_MATRICULA, 
                                     MATRICULA) 
                        SELECT DISTINCT @ALUNO, 
                               T.DISCIPLINA, 
                               T.TURMA, 
                               T.ANO, 
                               T.SEMESTRE, 
                               @SIT_MATRICULA, 
                               GETDATE(), 
                               'N', 
                               GETDATE(), 
                               GETDATE(), 
                               @MATRICULA 
                        FROM   LY_TURMA T 
							   INNER JOIN LY_CURSO C ( NOLOCK ) ON C.CURSO = T.CURSO
							   INNER JOIN LY_SERIE S ON T.CURRICULO = S.CURRICULO
														AND T.TURNO = S.TURNO
														AND T.CURSO = S.CURSO  
														AND T.SERIE = S.SERIE
                        WHERE  T.SIT_TURMA = 'Aberta' 
                               AND t.TURMA = @TURMA 
                               AND t.ANO = @ANO 
                               AND t.SEMESTRE = @SEMESTRE 
							   and (S.OFERTAELETIVA = 'N' or (S.OFERTAELETIVA = 'S' AND ISNULL(t.ELETIVA, 'N') = 'N'))  ";

            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, matricula.Aluno);
            contextQuery.Parameters.Add("@TURMA", matricula.Turma);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, matricula.Ano);
            contextQuery.Parameters.Add("@SIT_MATRICULA", TechneDbType.T_SIT_MATRICULA, Matriculado);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, matricula.Matricula);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, matricula.Semestre);

            ctx.ApplyModifications(contextQuery);
        }

        public void InsereAtualizaDisciplinasAtivasPorTurma(DataContext ctx, LyMatricula matricula)
        {
            ContextQuery contextQuery = new ContextQuery();
            contextQuery.Command = @" --Atualiza disciplinas que já existem canceladas 
                    UPDATE M 
                    SET    SIT_MATRICULA = @SIT_MATRICULA, 
                           DT_ULTALT = Getdate(), 
                           DT_MATRICULA = Getdate(), 
                           CONCOMITANTE = 'N',
                           DEPENDENCIA = 'N',
                           SERIE_REFERENCIA = NULL, 
                           DISCIPLINA_REFERENCIA = NULL, 
                           EDUC_ESPECIAL = 'N',
                           MAIS_EDUCACAO = 'N',
                           MATRICULA = @MATRICULA 
                    FROM   ly_matricula m 
                           INNER JOIN ly_turma t 
                                   ON M.ano = T.ano 
                                      AND M.semestre = T.semestre 
                                      AND M.turma = T.turma 
                                      AND m.disciplina = t.disciplina 
							INNER JOIN LY_CURSO C ( NOLOCK ) ON C.CURSO = T.CURSO
							INNER JOIN LY_SERIE S ON T.CURRICULO = S.CURRICULO
														AND T.TURNO = S.TURNO
														AND T.CURSO = S.CURSO  
														AND T.SERIE = S.SERIE     
                    WHERE  m.aluno = @ALUNO 
                           AND m.turma = @TURMA 
                           AND m.ano = @ANO 
                           AND m.semestre = @SEMESTRE 
                           AND T.sit_turma = 'Aberta' 
						   and (S.OFERTAELETIVA = 'N' or (S.OFERTAELETIVA = 'S' AND ISNULL(t.ELETIVA, 'N') = 'N'))

                    --Insere Disciplinas que ainda nao existem 
                    INSERT INTO dbo.ly_matricula 
                                (aluno, 
                                 disciplina, 
                                 turma, 
                                 ano, 
                                 semestre, 
                                 sit_matricula, 
                                 dt_ultalt, 
                                 cobranca_sep, 
                                 dt_insercao, 
                                 dt_matricula, 
                                 matricula) 
                    SELECT @ALUNO, 
                           T.disciplina, 
                           T.turma, 
                           T.ano, 
                           T.semestre, 
                           @SIT_MATRICULA, 
                           Getdate(), 
                           'N', 
                           Getdate(), 
                           Getdate(), 
                           @MATRICULA 
                    FROM   ly_turma T 
							INNER JOIN LY_CURSO C ( NOLOCK ) ON C.CURSO = T.CURSO
							INNER JOIN LY_SERIE S ON T.CURRICULO = S.CURRICULO
														AND T.TURNO = S.TURNO
														AND T.CURSO = S.CURSO  
														AND T.SERIE = S.SERIE
                           LEFT JOIN ly_matricula m 
                                  ON M.ano = T.ano 
                                     AND M.semestre = T.semestre 
                                     AND M.turma = T.turma 
                                     AND m.disciplina = t.disciplina 
                                     AND M.ALUNO=@ALUNO
                    WHERE  m.aluno IS NULL 
                           AND T.sit_turma = 'Aberta' 
                           AND T.turma = @TURMA 
                           AND T.ano = @ANO 
                           AND T.semestre = @SEMESTRE
						   and (S.OFERTAELETIVA = 'N' or (S.OFERTAELETIVA = 'S' AND ISNULL(t.ELETIVA, 'N') = 'N'))  ";

            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, matricula.Aluno);
            contextQuery.Parameters.Add("@TURMA", matricula.Turma);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, matricula.Ano);
            contextQuery.Parameters.Add("@SIT_MATRICULA", TechneDbType.T_SIT_MATRICULA, Matriculado);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, matricula.Matricula);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, matricula.Semestre);

            ctx.ApplyModifications(contextQuery);
        }

        public void AtualizaDadosAlunoConfirmacaoMatriculaPrincipal(DataContext ctx, string turma, decimal ano, decimal periodo, string aluno, string tipoEnsinoProfissionalizante)
        {
            ContextQuery contextQuery = new ContextQuery();
            contextQuery.Command = @"  DECLARE @CURSO VARCHAR(100) ,
                                        @TURNO VARCHAR(100) ,
                                        @CURRICULO VARCHAR(100) ,
                                        @SERIE NUMERIC(10, 0)                                      

                                   SELECT TOP 1
                                            @CURSO = CURSO, 
                                            @TURNO = TURNO, 
                                            @CURRICULO = CURRICULO, 
                                            @SERIE = SERIE                                            
                                    FROM    LY_TURMA
                                    WHERE   TURMA = @TURMA
                                            AND ANO = @ANO
                                            AND SEMESTRE = @SEMESTRE         
                                            
                                    UPDATE  LY_ALUNO
                                    SET     CURSO = @CURSO, 
		                                    TURNO = @TURNO, 
		                                    CURRICULO = @CURRICULO, 
		                                    SERIE = @SERIE	,
		                                    STAMP_ATUALIZACAO = GETDATE(),
                                            TIPO_ENSINO_PROFISSIONALIZANTE = @TIPO_ENSINO_PROFISSIONALIZANTE	
                                    WHERE   ALUNO = @ALUNO

                                    --UPDATE  TCE_CONFIRMACAO_MATRICULA --Retirar pois dados da confirmação precisam estar iguais na validação
                                    --SET     CURSO = @CURSO, 
		                            --        SERIE = @SERIE,	
		                            --        TURNO = @TURNO, 
		                            --        CURRICULO = @CURRICULO, 
		                            --        DT_ALTERACAO = GETDATE()	
                                    --WHERE   ALUNO = @ALUNO
                                    --        AND ANO = @ANO
                                    --        AND PERIODO = @SEMESTRE
                                    --        AND STATUS = @STATUS  ";

            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", periodo);
            contextQuery.Parameters.Add("@STATUS", RN.ConfirmacaoMatricula.Confirmado);
            contextQuery.Parameters.Add("@TIPO_ENSINO_PROFISSIONALIZANTE", tipoEnsinoProfissionalizante);

            ctx.ApplyModifications(contextQuery);
        }

        public DataTable ObtemDisciplinasAlunoMatriculadoPor(string aluno, int ano, int semestre, string turma)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable confirmacoes = null;

            try
            {
                contextQuery.Command = @" SELECT M.DISCIPLINA AS disciplina, 
                                                   DIS.NOME     AS nome, 
                                                   M.TURMA      AS turma, 
                                                   M.ANO, 
                                                   M.SEMESTRE, 
                                                   M.SIT_MATRICULA 
                                            FROM   LY_MATRICULA M 
                                                   INNER JOIN LY_DISCIPLINA DIS 
                                                           ON DIS.DISCIPLINA = M.DISCIPLINA 
												    INNER JOIN ly_turma t 
														   ON M.ano = T.ano 
															  AND M.semestre = T.semestre 
															  AND M.turma = T.turma 
															  AND m.disciplina = t.disciplina 
													INNER JOIN LY_CURSO C ( NOLOCK ) ON C.CURSO = T.CURSO
													INNER JOIN LY_SERIE S ON T.CURRICULO = S.CURRICULO
																				AND T.TURNO = S.TURNO
																				AND T.CURSO = S.CURSO  
																				AND T.SERIE = S.SERIE  
                                            WHERE  ALUNO = @ALUNO 
                                                   AND SIT_MATRICULA IN ( 'Matriculado' ) 
                                                   AND M.ANO = @ANO
                                                   AND M.SEMESTRE = @SEMESTRE 
                                                   AND m.TURMA = @TURMA 
												   AND (S.OFERTAELETIVA = 'N' OR (S.OFERTAELETIVA = 'S' AND ISNULL(T.ELETIVA, 'N') = 'N'))
                                            ORDER  BY DISCIPLINA  ";

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@TURMA", turma);


                confirmacoes = ctx.GetDataTable(contextQuery);
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

            return confirmacoes;
        }

        public ValidacaoDados ValidaRemocaoEletivas(string aluno, string usuarioResponsavel)
        {
            DataContext contexto = null;
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (aluno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O ALUNO não foi encontrado.");
            }

            if (usuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O USUARIO RESPONSAVEL não foi encontrado.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (!this.PossuiMatriculaEletivaAtiva(contexto, aluno))
                    {
                        mensagens.Add("Não há eletivas para serem excluídas.");
                    }
                }
                catch (Exception ex)
                {
                    string mensagem = string.Empty;
                    contexto.Abandon();
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine,
                            Convert.ToString(ex.Message));
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

        public void RemoveEletivas(string aluno, string usuarioResponsavel)
        {
            RN.Matgrade rnMatgrade = new Matgrade();
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingLock();

                //Cancela matricula
                this.CancelaMatriculaEletivaPor(contexto, aluno, usuarioResponsavel);

                //Cancela matgrade
                rnMatgrade.CancelaMatgradeEletivaPor(contexto, aluno);
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

        public string RetornaTurnoMatriculaCursoPor(DataContext ctx, decimal ano, decimal semestre, string aluno, string curso)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            string turno = string.Empty;

            contextQuery.Command = @" SELECT DISTINCT TURNO 
                        FROM   DBO.LY_MATRICULA M (NOLOCK) 
                               INNER JOIN LY_TURMA T (NOLOCK) 
                                       ON M.ANO = T.ANO 
                                          AND M.SEMESTRE = T.SEMESTRE 
                                          AND M.TURMA = T.TURMA 
                                          AND M.DISCIPLINA = T.DISCIPLINA 
                        WHERE  M.ANO = @ANO 
                               AND M.SEMESTRE = @SEMESTRE 
                               AND T.CURSO = @CURSO
                               AND SIT_MATRICULA = 'Matriculado' 
                               AND M.ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", semestre);
            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@CURSO", curso);

            reader = ctx.GetDataReader(contextQuery);

            while (reader.Read())
            {
                turno = Convert.ToString(reader["TURNO"]);
            }

            if (reader != null)
            {
                reader.Close();
            }

            return turno;
        }

        public ValidacaoDados ValidaRemocaoMatriculaPrincipal(string aluno, int ano, int periodo, string turma)
        {
            DataContext contexto = null;
            List<string> mensagens = new List<string>();
            RN.Matricula rnMatricula = new RN.Matricula();
            RN.Nota rnNota = new Nota();
            RN.Falta rnFalta = new Falta();
            RN.Ocorrencia rnOcorrencia = new Ocorrencia();
            RN.AlunoLicenca rnAlunoLicenca = new AlunoLicenca();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (aluno.IsNullOrEmptyOrWhiteSpace() || ano <= 0 || periodo < 0)
            {
                return validacaoDados;
            }

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                if (!EhMatriculaAtiva(contexto, aluno, ano, periodo))
                {
                    mensagens.Add("Não há matrícula para ser excluída.");
                    validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
                    return validacaoDados;
                }

                if (rnMatricula.PossuiMatriculaOptativaReforco(contexto, aluno))
                {
                    mensagens.Add("Aluno não pode ser excluído devido estar matriculado em uma Turma Optativa/Reforço.");
                }

                if (rnMatricula.PossuiMatriculaDependencia(contexto, aluno))
                {
                    mensagens.Add("Aluno não pode ser excluído devido estar matriculado em uma Dependência.");
                }

                if (rnMatricula.PossuiMatriculaEducacaoEspecial(contexto, aluno))
                {
                    mensagens.Add("Aluno não pode ser excluído devido estar matriculado em uma Turma de Educação Especial.");
                }

                if (rnMatricula.PossuiMatriculaEnsinoProfissionalConcomitante(contexto, aluno))
                {
                    mensagens.Add("Aluno não pode ser excluído devido estar matriculado em uma Turma Ensino Profissional Concomitante.");
                }

                if (rnMatricula.PossuiMatriculaMaisEducacao(contexto, aluno))
                {
                    mensagens.Add("Aluno não pode ser excluído devido estar matriculado em uma Turma de Mais Educação.");
                }

                if (rnMatricula.PossuiMatriculaEletiva(contexto, aluno))
                {
                    mensagens.Add("Aluno não pode ser excluído devido estar matriculado em uma Turma Eletiva.");
                }

                if (rnMatricula.PossuiMatriculaReforcoFoco(contexto, aluno))
                {
                    mensagens.Add("Aluno não pode ser excluído devido estar matriculado em uma Turma de Reforço - Projeto FOCO.");
                }

                if (mensagens.Count == 0)
                {
                    if (rnNota.PossuiNotaNaoNulaPor(contexto, aluno, ano, periodo, turma))
                    {
                        mensagens.Add("Existe notas cadastrada para a turma.");
                    }

                    if (rnFalta.PossuiFaltaNaoZeradaPor(contexto, aluno, ano, periodo, turma))
                    {
                        mensagens.Add("Existem faltas cadastradas para a turma.");
                    }

                    if (rnAlunoLicenca.PossuiLicencaPor(contexto, aluno, ano, periodo, turma))
                    {
                        mensagens.Add("Existem faltas justificadas cadastradas para a turma.");
                    }

                    if (rnOcorrencia.PossuiOcorrenciaPor(contexto, aluno, ano, periodo, turma))
                    {
                        mensagens.Add("Existe ocorrência cadastrada para esta matrícula.");
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

            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                {
                    contexto.Dispose();
                }
            }

            return validacaoDados;
        }

        public void RemoveMatricula(string aluno, int ano, int periodo, string turma)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            RN.Nota rnNota = new Nota();
            RN.Falta rnFalta = new Falta();
            RN.MatriculaBkp rnMatriculaBkp = new MatriculaBkp();
            RN.Matgrade rnMatgrade = new Matgrade();
            RN.DeclaracaoSemNota rnDeclaracaoSemNota = new DeclaracaoSemNota();
            string motivoBkp = string.Empty;

            try
            {
                //apaga as declaraçoes sem nota
                rnDeclaracaoSemNota.Remove(ctx, aluno, turma, ano, periodo);

                //apaga notas nulas
                rnNota.RemoveNotaNula(ctx, aluno, ano, periodo, turma);

                //apaga faltas zeradas
                rnFalta.RemoveFaltaZerada(ctx, aluno, ano, periodo, turma);

                //insere na tabela de backup
                motivoBkp = "Exclusão tela Matrícula";
                rnMatriculaBkp.InserePorTurma(ctx, aluno, ano, periodo, turma, motivoBkp);

                //apaga matricula
                this.RemoveMatriculaPorTurma(ctx, aluno, ano, periodo, turma);

                //apaga matgrade
                rnMatgrade.RemovePorTurma(ctx, aluno, ano, periodo, turma);
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

        public void RemoveMatriculaPorTurma(DataContext ctx, string aluno, decimal ano, decimal periodo, string turma)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE  LY_MATRICULA 
                                    WHERE ALUNO = @ALUNO
	                                    AND TURMA = @TURMA
	                                    AND ANO = @ANO
	                                    AND SEMESTRE = @SEMESTRE ";

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);
                contextQuery.Parameters.Add("@TURMA", turma);

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
        public DataTable ListaDisciplinaGradePorTurmasDiferenteDe(DataContext ctx, string aluno, decimal ano, decimal periodo, string turmaExcessao)
        {
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            contextQuery.Command = @"SELECT DISTINCT 
                                                    D.DISCIPLINA, 
                                                    D.NOME, 
                                                    '' AS SIT_MATGRADE, 
                                                    GT.GRADE_ID, 
                                                    T.CURSO, 
                                                    T.TURMA 
                                    FROM   LY_TURMA T ( NOLOCK ) 
                                           INNER JOIN LY_GRADE_TURMA GT ( NOLOCK ) 
                                                   ON GT.DISCIPLINA = T.DISCIPLINA 
                                                      AND GT.TURMA = T.TURMA 
                                                      AND GT.ANO = T.ANO 
                                                      AND GT.SEMESTRE = T.SEMESTRE 
                                           INNER JOIN LY_DISCIPLINA D ( NOLOCK ) 
                                                   ON T.DISCIPLINA = D.DISCIPLINA 
                                    WHERE  T.ANO = @ANO 
                                           AND T.SEMESTRE = @SEMESTRE
                                           AND T.TURMA IN (SELECT DISTINCT TURMA 
                                                           FROM   DBO.LY_MATRICULA 
                                                           WHERE  ALUNO = @ALUNO 
                                                                  AND SIT_MATRICULA = 'MATRICULADO' 
                                                                  AND ANO = @ANO 
                                                                  AND SEMESTRE = @SEMESTRE 
                                                                  AND ISNULL(DEPENDENCIA, 'N') = 'N' 
                                                                  AND ISNULL(EDUC_ESPECIAL, 'N') = 'N' 
                                                                  AND TURMA <> @TURMA)  ";

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", periodo);
            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@TURMA", turmaExcessao);

            dt = ctx.GetDataTable(contextQuery);

            return dt;
        }

        public DataTable ListaDisciplinaGradePor(DataContext ctx, decimal ano, decimal periodo, string turma)
        {
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            contextQuery.Command = @"SELECT DISTINCT 
                                                    D.DISCIPLINA, 
                                                    D.NOME, 
                                                    '' AS SIT_MATGRADE, 
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
                                    WHERE  T.ANO = @ANO 
                                           AND T.SEMESTRE = @SEMESTRE 
                                           AND T.TURMA = @TURMA";

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", periodo);
            contextQuery.Parameters.Add("@TURMA", turma);

            dt = ctx.GetDataTable(contextQuery);
            return dt;
        }

        public string ObtemTurmaMatriculaConcomitantePor(DataContext ctx, decimal ano, decimal semestre, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            string turma = string.Empty;

            contextQuery.Command = @" SELECT  DISTINCT TURMA
                        FROM    DBO.LY_MATRICULA M       
                                WHERE M.ANO = @ANO
                                AND M.SEMESTRE = @SEMESTRE
                                AND M.CONCOMITANTE = 'S'
                                AND SIT_MATRICULA = 'Matriculado'
                                AND M.ALUNO = @ALUNO";

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", semestre);
            contextQuery.Parameters.Add("@ALUNO", aluno);

            reader = ctx.GetDataReader(contextQuery);

            while (reader.Read())
            {
                turma = Convert.ToString(reader["TURMA"]);
            }

            if (reader != null)
            {
                reader.Close();
            }

            return turma;
        }

        public string ObtemTurnoMatriculaConcomitantePor(DataContext ctx, decimal ano, decimal semestre, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            string turno = string.Empty;

            contextQuery.Command = @" SELECT DISTINCT TURNO 
                        FROM   DBO.LY_MATRICULA M (NOLOCK) 
                               INNER JOIN LY_TURMA T (NOLOCK) 
                                       ON M.ANO = T.ANO 
                                          AND M.SEMESTRE = T.SEMESTRE 
                                          AND M.TURMA = T.TURMA 
                                          AND M.DISCIPLINA = T.DISCIPLINA 
                        WHERE  M.ANO = @ANO 
                               AND M.SEMESTRE = @SEMESTRE 
                               AND M.CONCOMITANTE = 'S' 
                               AND SIT_MATRICULA = 'Matriculado' 
                               AND M.ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", semestre);
            contextQuery.Parameters.Add("@ALUNO", aluno);

            reader = ctx.GetDataReader(contextQuery);

            while (reader.Read())
            {
                turno = Convert.ToString(reader["TURNO"]);
            }

            if (reader != null)
            {
                reader.Close();
            }

            return turno;
        }


        public string ObtemTurmaPrincipalPor(DataContext ctx, string aluno, int ano, int periodo)
        {
            ContextQuery contextQuery = new ContextQuery();
            string turma = string.Empty;

            contextQuery.Command = @" SELECT  DISTINCT                                    
                                    M.TURMA
                            FROM    DBO.LY_MATRICULA M
                                    INNER JOIN LY_TURMA T ON M.DISCIPLINA = T.DISCIPLINA
                                                             AND M.TURMA = T.TURMA
                                                             AND M.ANO = T.ANO
                                                             AND M.SEMESTRE = T.SEMESTRE
                            WHERE   M.ANO = @ANO
                                    AND M.SEMESTRE = @SEMESTRE
                                    AND ( M.DEPENDENCIA IS NULL
                                          OR M.DEPENDENCIA = 'N'
                                        )
                                    AND ( M.CONCOMITANTE IS NULL
                                          OR M.CONCOMITANTE = 'N'
                                        )
                                    AND ( M.EDUC_ESPECIAL IS NULL
                                          OR M.EDUC_ESPECIAL = 'N'
                                        )
                                    AND ( M.MAIS_EDUCACAO IS NULL
                                          OR M.MAIS_EDUCACAO = 'N'
                                        )
                                    AND t.OPTATIVAREFORCO = 'N'
                                    AND ISNULL(T.ELETIVA,'N') = 'N'
                                    AND M.ALUNO = @ALUNO
                                    AND M.SIT_MATRICULA = @SIT_MATRICULA ";

            contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", periodo);

            turma = ctx.GetReturnValue<string>(contextQuery);

            return turma;
        }

        public void InsereOuAtualizaMatriculaOptativaReforco(DataContext dataContext, LyMatricula matriculaOptativaReforco)
        {
            ContextQuery contextQuery = new ContextQuery(
                    @"DECLARE @MATRICULAATIVA INT 

                        SELECT @MATRICULAATIVA = COUNT(*)
                        FROM    LY_MATRICULA
                        WHERE   ALUNO = @ALUNO
                                AND DISCIPLINA = @DISCIPLINA
                                AND TURMA = @TURMA
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE

                    IF ( @MATRICULAATIVA = 0 ) 
                      BEGIN 
                        INSERT  INTO dbo.LY_MATRICULA
                                                ( ALUNO ,
                                                  DISCIPLINA ,
                                                  TURMA ,
                                                  ANO ,
                                                  SEMESTRE ,
                                                  SIT_MATRICULA ,
                                                  DT_ULTALT ,
                                                  COBRANCA_SEP ,
                                                  DT_INSERCAO ,
                                                  DT_MATRICULA ,
                                                  MATRICULA ,
                                                  DT_CADASTRO 
                                                )
                                        VALUES  ( @ALUNO ,
                                                  @DISCIPLINA ,
                                                  @TURMA ,
                                                  @ANO ,
                                                  @SEMESTRE ,
                                                  @SIT_MATRICULA ,
                                                  GETDATE() ,
                                                  'N' ,
                                                  GETDATE() ,
                                                  GETDATE() ,
                                                  @MATRICULA ,
                                                  GETDATE()
                                                )
                      END 
                    ELSE 
                      BEGIN 
                                UPDATE  LY_MATRICULA
                            SET     SIT_MATRICULA = @SIT_MATRICULA ,
                                    STAMP_ATUALIZACAO = GETDATE() ,
                                    MATRICULA = @MATRICULA ,
                                    CONCOMITANTE = 'N' ,
                                    DEPENDENCIA = 'N' ,
                                    SERIE_REFERENCIA = NULL, 
                                    DISCIPLINA_REFERENCIA = NULL, 
                                    EDUC_ESPECIAL = 'N' ,
                                    MAIS_EDUCACAO = 'N'
                            WHERE   ALUNO = @ALUNO
                                    AND DISCIPLINA = @DISCIPLINA
                                    AND TURMA = @TURMA
                                    AND ANO = @ANO
                                    AND SEMESTRE = @SEMESTRE
                      END");


            contextQuery.Parameters.Add("@SIT_MATRICULA", TechneDbType.T_SIT_MATRICULA, Matriculado);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, matriculaOptativaReforco.Matricula);
            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, matriculaOptativaReforco.Aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", TechneDbType.T_CODIGO, matriculaOptativaReforco.Disciplina);
            contextQuery.Parameters.Add("@TURMA", matriculaOptativaReforco.Turma);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, matriculaOptativaReforco.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, matriculaOptativaReforco.Semestre);

            dataContext.ApplyModifications(contextQuery);
        }

        public void InsereouAtualizaMatriculaPrincipal(DataContext dataContext, TceConfirmacaoMatricula confirmacaoMatricula, string turma, string disciplina)
        {
            ContextQuery contextQuery = new ContextQuery(
                    @"DECLARE @MATRICULAATIVA INT 

                        SELECT @MATRICULAATIVA = COUNT(*)
                        FROM    LY_MATRICULA
                        WHERE   ALUNO = @ALUNO
                                AND DISCIPLINA = @DISCIPLINA
                                AND TURMA = @TURMA
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE

                    IF ( @MATRICULAATIVA = 0 ) 
                      BEGIN 
                        INSERT  INTO dbo.LY_MATRICULA
                            (
                              ALUNO,
                              DISCIPLINA,
                              TURMA,
                              ANO,
                              SEMESTRE,
                              SIT_MATRICULA,
                              DT_ULTALT,
                              COBRANCA_SEP,
                              DT_INSERCAO,
                              DT_MATRICULA,
                              MATRICULA
                            )
                    VALUES  (
                              @ALUNO,
                              @DISCIPLINA,
                              @TURMA,
                              @ANO,
                              @SEMESTRE,
                              @SIT_MATRICULA,
                              GETDATE(),
                              'N',
                              GETDATE(),
                              GETDATE(),
                              @MATRICULA
                            )
                      END 
                    ELSE 
                      BEGIN 
                               UPDATE  LY_MATRICULA
                                SET     SIT_MATRICULA = @SIT_MATRICULA,
                                        DT_ULTALT = GETDATE(),
                                        DT_MATRICULA = GETDATE(),
                                        CONCOMITANTE = 'N',
                                        DEPENDENCIA = 'N',
                                        SERIE_REFERENCIA = NULL, 
                                        DISCIPLINA_REFERENCIA = NULL, 
                                        EDUC_ESPECIAL = 'N',
                                        MAIS_EDUCACAO = 'N',
                                        MATRICULA = @MATRICULA
                                WHERE   ALUNO = @ALUNO
                                        AND DISCIPLINA = @DISCIPLINA
                                        AND TURMA = @TURMA
                                        AND ANO = @ANO
                                        AND SEMESTRE = @SEMESTRE
                      END");


            contextQuery.Parameters.Add("@SIT_MATRICULA", TechneDbType.T_SIT_MATRICULA, Matriculado);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, confirmacaoMatricula.Matricula);
            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, confirmacaoMatricula.Aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", TechneDbType.T_CODIGO, disciplina);
            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, confirmacaoMatricula.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, confirmacaoMatricula.Periodo);

            dataContext.ApplyModifications(contextQuery);
        }

        public void InsereOuAtualizaMatriculaPrincipal(DataContext dataContext, string turma, string disciplina, string usuarioResponsavel, string aluno, int ano, int periodo)
        {
            ContextQuery contextQuery = new ContextQuery(
                    @"DECLARE @MATRICULAATIVA INT 

                        SELECT @MATRICULAATIVA = COUNT(*)
                        FROM    LY_MATRICULA
                        WHERE   ALUNO = @ALUNO
                                AND DISCIPLINA = @DISCIPLINA
                                AND TURMA = @TURMA
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE

                    IF ( @MATRICULAATIVA = 0 ) 
                      BEGIN 
                        INSERT  INTO dbo.LY_MATRICULA
                            (
                              ALUNO,
                              DISCIPLINA,
                              TURMA,
                              ANO,
                              SEMESTRE,
                              SIT_MATRICULA,
                              DT_ULTALT,
                              COBRANCA_SEP,
                              DT_INSERCAO,
                              DT_MATRICULA,
                              MATRICULA
                            )
                    VALUES  (
                              @ALUNO,
                              @DISCIPLINA,
                              @TURMA,
                              @ANO,
                              @SEMESTRE,
                              @SIT_MATRICULA,
                              GETDATE(),
                              'N',
                              GETDATE(),
                              GETDATE(),
                              @MATRICULA
                            )
                      END 
                    ELSE 
                      BEGIN 
                               UPDATE  LY_MATRICULA
                                SET     SIT_MATRICULA = @SIT_MATRICULA,
                                        DT_ULTALT = GETDATE(),
                                        DT_MATRICULA = GETDATE(),
                                        CONCOMITANTE = 'N',
                                        DEPENDENCIA = 'N',
                                        SERIE_REFERENCIA = NULL, 
                                        DISCIPLINA_REFERENCIA = NULL, 
                                        EDUC_ESPECIAL = 'N',
                                        MAIS_EDUCACAO = 'N',
                                        MATRICULA = @MATRICULA
                                WHERE   ALUNO = @ALUNO
                                        AND DISCIPLINA = @DISCIPLINA
                                        AND TURMA = @TURMA
                                        AND ANO = @ANO
                                        AND SEMESTRE = @SEMESTRE
                      END");


            contextQuery.Parameters.Add("@SIT_MATRICULA", TechneDbType.T_SIT_MATRICULA, Matriculado);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", TechneDbType.T_CODIGO, disciplina);
            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, periodo);

            dataContext.ApplyModifications(contextQuery);
        }

        public void EnturmaAluno(DataContext ctx, string aluno, int ano, int periodo, string turma, string curso, string curriculo, int serie, string usuarioResponsavel, DTOs.DadosEnturmacaoAluno dadosEnturmacaoAluno)
        {
            RN.Falta rnFalta = new Falta();
            RN.Matgrade rnMatgrade = new Matgrade();
            string mensagem = string.Empty;
            RN.Matricula rnMatricula = new Matricula();
            RN.Turma rnTurma = new Turma();
            RN.Disciplina rnDisciplina = new Disciplina();

            var disciplinas = Matricula.ConsultaDisciplinaGrade(Convert.ToString(ano), Convert.ToString(periodo), turma);

            if (disciplinas == null)
            {
                mensagem = "Não existe turma para efetuar a enturmação. Favor verificar.";
                throw new Exception(mensagem);
            }
            if (disciplinas.Rows.Count == 0)
            {
                var disciplinasTurma = rnTurma.ObtemDisciplinasTurmaPor(ano, periodo, turma);
                mensagem = string.Format("Turma com situação {0} e {1} disciplina(s). Entre em contato com o Administrador do Sistema repassando esta mensagem(Confirmação de Matricula) ou tente novamente mais tarde.", disciplinasTurma.Count() == 0 ? "NÃO ENCONTRADA" : disciplinasTurma.Select(x => x.SitTurma).First(), disciplinasTurma.Count().ToString());
                throw new Exception(mensagem);
            }

            foreach (DataRow disciplinaRow in disciplinas.Rows)
            {
                var disciplina = disciplinaRow["disciplina"].ToString();

                if (HistMatricula.ExisteHistorico(aluno, disciplina, turma, ano, periodo))
                {
                    mensagem = string.Format(
                        "Já existe histórico do aluno {0} para a disciplina {1} da turma {2}",
                        aluno,
                        disciplina,
                        turma);
                    throw new Exception(mensagem);
                }

                //Verifica se a disciplina não é uma eletiva com enturmação separada
                if (!rnDisciplina.EhDisciplinaGradeEletivaPor(ano, periodo, turma, disciplina))
                {
                    rnMatricula.InsereOuAtualizaMatriculaPrincipal(ctx, turma, disciplina, usuarioResponsavel, aluno, ano, periodo);
                }

                //Verifica se o aluno já estava enturmado
                if (!dadosEnturmacaoAluno.Turma.IsNullOrEmptyOrWhiteSpace() //Ter turma atual
                    && dadosEnturmacaoAluno.Ano == ano //Ser o mesmo ano
                    && (dadosEnturmacaoAluno.Periodo == periodo || dadosEnturmacaoAluno.Periodo == 0 || periodo == 0)) //Não pode levar notas do 1 p 2 ou 2 p/ 1
                {
                    //Monta dados de origem para migração de notas e faltas
                    LyMatricula matriculaOrigem = new LyMatricula();
                    matriculaOrigem.Ano = dadosEnturmacaoAluno.Ano;
                    matriculaOrigem.Semestre = dadosEnturmacaoAluno.Periodo;
                    matriculaOrigem.Turma = dadosEnturmacaoAluno.Turma;
                    matriculaOrigem.Aluno = aluno;
                    matriculaOrigem.Matricula = usuarioResponsavel;
                    matriculaOrigem.Disciplina = disciplina;

                    //Verifica se o aluno possui, em qualquer bimestre, numero de faltas maior que de aulas dadas da turma de destino
                    bool possuiInconsistenciaFalta = rnFalta.PossuiInconsistenciaFaltaAulasDadasPor(ctx, matriculaOrigem, turma);

                    //Apenas migrar notas e faltas caso nao exista inconsistencia nas faltas e auldas dadas do destino
                    if (!possuiInconsistenciaFalta)
                    {
                        Nota.MigrarNotas(ctx, matriculaOrigem, turma);
                        Falta.MigrarFaltas(ctx, matriculaOrigem, turma);
                    }
                }
            }

            if (!rnMatricula.PossuiMatriculaAtivaNaTurmaPorAluno(ctx, aluno, turma, ano, periodo))
            {
                var disciplinasTurma = rnTurma.ObtemDisciplinasTurmaPor(ano, periodo, turma);

                turma = string.Empty;
                mensagem = string.Format("Turma com situação {0} e {1} disciplina(s). Entre em contato com o Administrador do Sistema repassando esta mensagem(Confirmação de Matricula) ou tente novamente mais tarde.", disciplinasTurma.Count() == 0 ? "NÃO ENCONTRADA" : disciplinasTurma.Select(x => x.SitTurma).First(), disciplinasTurma.Count().ToString());
                throw new Exception(mensagem);
            }

            var gradeId = GradeSerie.ObterGradeId(
                ctx,
                ano,
                periodo,
                curso,
                curriculo,
                serie,
                turma);

            if (gradeId.IsNullOrEmptyOrWhiteSpace())
            {
                turma = string.Empty;
                mensagem = string.Format("Grade série não encontrada para Turma {0} selecionada.", turma);
                throw new Exception(mensagem);
            }
            else
            {
                rnMatgrade.InsereOuAtualizaMatGrade(ctx, aluno, gradeId);
            }
        }

        public void EnturmaAluno(DataContext ctx, string aluno, int ano, int periodo, string turma, string curso, string curriculo, int serie, string usuarioResponsavel)
        {
            RN.Matgrade rnMatgrade = new Matgrade();
            string mensagem = string.Empty;
            RN.Matricula rnMatricula = new Matricula();
            RN.Turma rnTurma = new Turma();
            RN.Disciplina rnDisciplina = new Disciplina();

            var disciplinas = Matricula.ConsultaDisciplinaGrade(Convert.ToString(ano), Convert.ToString(periodo), turma);

            if (disciplinas == null)
            {
                mensagem = "Não existe turma para efetuar a enturmação. Favor verificar.";
                throw new Exception(mensagem);
            }
            if (disciplinas.Rows.Count == 0)
            {
                var disciplinasTurma = rnTurma.ObtemDisciplinasTurmaPor(ano, periodo, turma);
                mensagem = string.Format("Turma com situação {0} e {1} disciplina(s). Entre em contato com o Administrador do Sistema repassando esta mensagem(Confirmação de Matricula) ou tente novamente mais tarde.", disciplinasTurma.Count() == 0 ? "NÃO ENCONTRADA" : disciplinasTurma.Select(x => x.SitTurma).First(), disciplinasTurma.Count().ToString());
                throw new Exception(mensagem);
            }

            foreach (DataRow disciplinaRow in disciplinas.Rows)
            {
                var disciplina = disciplinaRow["disciplina"].ToString();

                if (HistMatricula.ExisteHistorico(aluno, disciplina, turma, ano, periodo))
                {
                    mensagem = string.Format(
                        "Já existe histórico do aluno {0} para a disciplina {1} da turma {2}",
                        aluno,
                        disciplina,
                        turma);
                    throw new Exception(mensagem);
                }

                //Verifica se a disciplina não é uma eletiva com enturmação separada
                if (!rnDisciplina.EhDisciplinaGradeEletivaPor(ano, periodo, turma, disciplina))
                {
                    rnMatricula.InsereOuAtualizaMatriculaPrincipal(ctx, turma, disciplina, usuarioResponsavel, aluno, ano, periodo);
                }
            }

            if (!rnMatricula.PossuiMatriculaAtivaNaTurmaPorAluno(ctx, aluno, turma, ano, periodo))
            {
                var disciplinasTurma = rnTurma.ObtemDisciplinasTurmaPor(ano, periodo, turma);

                turma = string.Empty;
                mensagem = string.Format("Turma com situação {0} e {1} disciplina(s). Entre em contato com o Administrador do Sistema repassando esta mensagem(Confirmação de Matricula) ou tente novamente mais tarde.", disciplinasTurma.Count() == 0 ? "NÃO ENCONTRADA" : disciplinasTurma.Select(x => x.SitTurma).First(), disciplinasTurma.Count().ToString());
                throw new Exception(mensagem);
            }

            var gradeId = GradeSerie.ObterGradeId(
                ctx,
                ano,
                periodo,
                curso,
                curriculo,
                serie,
                turma);

            if (gradeId.IsNullOrEmptyOrWhiteSpace())
            {
                turma = string.Empty;
                mensagem = string.Format("Grade série não encontrada para Turma {0} selecionada.", turma);
                throw new Exception(mensagem);
            }
            else
            {
                rnMatgrade.InsereOuAtualizaMatGrade(ctx, aluno, gradeId);
            }
        }

        public void CancelaMatriculaPor(DataContext ctx, string aluno, string usuarioResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE LY_MATRICULA 
                        SET    SIT_MATRICULA = @SIT_MATRICULA_CANCELADA, 
                               DT_ULTALT = @DT_ULTALT,
                               MATRICULA = @MATRICULA
                        WHERE  ALUNO = @ALUNO
                               AND SIT_MATRICULA <> @SIT_MATRICULA_CANCELADA ";

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@SIT_MATRICULA_CANCELADA", Cancelado);
                contextQuery.Parameters.Add("@DT_ULTALT", DateTime.Now);
                contextQuery.Parameters.Add("@MATRICULA", usuarioResponsavel);

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

        public void CancelaMatriculaEletivaPor(DataContext ctx, string aluno, string usuarioResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE M 
                                          SET   SIT_MATRICULA = @SIT_MATRICULA_CANCELADA, 
                                                DT_ULTALT = @DT_ULTALT,
                                                MATRICULA = @MATRICULA
			                        FROM LY_MATRICULA M
			                                INNER JOIN DBO.LY_TURMA T ( NOLOCK ) ON T.TURMA = M.TURMA
                                                                            AND T.ANO = M.ANO
                                                                            AND T.SEMESTRE = M.SEMESTRE
                                                                            AND T.DISCIPLINA = M.DISCIPLINA
                                    WHERE  ALUNO = @ALUNO
                                           AND SIT_MATRICULA <> @SIT_MATRICULA_CANCELADA
                                           AND T.ELETIVA = 'S' ";

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@SIT_MATRICULA_CANCELADA", Cancelado);
                contextQuery.Parameters.Add("@DT_ULTALT", DateTime.Now);
                contextQuery.Parameters.Add("@MATRICULA", usuarioResponsavel);

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

        public int ObtemPeriodoMatriculaConcomitantePor(decimal ano, string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int periodo = -1;

            contextQuery.Command = @" SELECT  DISTINCT SEMESTRE
                        FROM    DBO.LY_MATRICULA M       
                                WHERE M.ANO = @ANO                                
                                AND M.CONCOMITANTE = 'S'
                                AND SIT_MATRICULA = 'Matriculado'
                                AND M.ALUNO = @ALUNO";

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@ALUNO", aluno);

            reader = ctx.GetDataReader(contextQuery);

            while (reader.Read())
            {
                periodo = Convert.ToInt32(reader["SEMESTRE"]);
            }

            if (reader != null)
            {
                reader.Close();
            }

            return periodo;
        }

        private DataTable ObtemDisciplinaGradePor(int ano, int periodo, string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable disciplinas = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
		                                            D.DISCIPLINA ,
		                                            D.NOME ,
		                                            GT.GRADE_ID ,
		                                            T.CURSO,
		                                            T.TURMA  
                                            FROM   DBO.LY_MATRICULA M
                                            INNER JOIN LY_TURMA T ON T.ANO=M.ANO 
						                                            AND T.SEMESTRE=M.SEMESTRE 
						                                            AND T.TURMA=M.TURMA 
						                                            AND T.DISCIPLINA=M.DISCIPLINA
                                            INNER JOIN LY_GRADE_TURMA GT ( NOLOCK ) ON GT.DISCIPLINA = T.DISCIPLINA
                                                                                    AND GT.TURMA = T.TURMA
                                                                                    AND GT.ANO = T.ANO
                                                                                    AND GT.SEMESTRE = T.SEMESTRE
                                            INNER JOIN LY_DISCIPLINA D ( NOLOCK ) ON T.DISCIPLINA = D.DISCIPLINA
                        WHERE   M.ALUNO = @ALUNO
                                AND M.SIT_MATRICULA = @SIT_MATRICULA
                                AND M.ANO = @ANO
                                AND M.SEMESTRE = @SEMESTRE 
                                AND ISNULL(M.DEPENDENCIA, 'N') = 'N'
                                AND ISNULL(M.MAIS_EDUCACAO, 'N') NOT IN ('L','O') ";

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);
                contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);

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

            return disciplinas;
        }


        public DataTable ListaMatriculasElegiveisPor(string turmaReferencia, int ano, int periodo, int mes)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable lista = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"Turma.SP_FREQUENCIAALUNO";
                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turmaReferencia);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
                contextQuery.Parameters.Add("@MES", SqlDbType.Int, mes);

                lista = contexto.GetDataTable(contextQuery);
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

            return lista;
        }

        public DataTable ListaMatriculasElegiveisHistoricoPor(string turmaReferencia, int ano, int periodo, int mes)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable lista = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"Turma.SP_FREQUENCIAALUNOHISTORICO";
                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turmaReferencia);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
                contextQuery.Parameters.Add("@MES", SqlDbType.Int, mes);

                lista = contexto.GetDataTable(contextQuery);
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

            return lista;
        }

        public DataTable ListaMatriculaEducacaoEspecialPor(string aluno, int ano, int semestre, string curso)
        {
            DataTable matriculas = null;
            ContextQuery contextQuery = new ContextQuery();

            if (!string.IsNullOrEmpty(aluno))
            {
                try
                {
                    contextQuery.Command = @" SELECT DISTINCT M.ALUNO,TR.REGIONAL,lue.NOME_COMP AS ESCOLA, M.ANO ,
                                M.SEMESTRE ,
                                TN.DESCRICAO TURNO,
                                M.TURMA ,                             
                                CONVERT(VARCHAR(10), M.DT_MATRICULA, 103) as DT_MATRICULA ,                               
                                M.SIT_MATRICULA
                        FROM    DBO.LY_MATRICULA M ( NOLOCK )
                                INNER JOIN DBO.LY_TURMA T ( NOLOCK ) ON T.TURMA = M.TURMA
                                                                        AND T.ANO = M.ANO
                                                                        AND T.SEMESTRE = M.SEMESTRE
                                                                        AND T.DISCIPLINA = M.DISCIPLINA
                                INNER JOIN DBO.LY_DISCIPLINA D ( NOLOCK ) ON M.DISCIPLINA = D.DISCIPLINA 
                                JOIN	LY_UNIDADE_ENSINO LUE (NOLOCK) ON LUE.UNIDADE_ENS = T.FACULDADE
                                JOIN	TCE_REGIONAL TR (NOLOCK) ON TR.ID_REGIONAL = LUE.ID_REGIONAL
                                JOIN	MUNICIPIO MU (NOLOCK) ON MU.CODIGO = LUE.MUNICIPIO
                                JOIN    LY_TURNO TN ON TN.TURNO = T.TURNO
                        WHERE  NOT ( M.EDUC_ESPECIAL = 'N' )
                                AND ALUNO = @ALUNO 
                                AND M.ANO = @ANO
                                AND M.SEMESTRE = @SEMESTRE
                                AND CURSO = @CURSO";

                    contextQuery.Parameters.Add("@ALUNO", aluno);
                    contextQuery.Parameters.Add("@ANO", ano);
                    contextQuery.Parameters.Add("@SEMESTRE", semestre);
                    contextQuery.Parameters.Add("@CURSO", curso);

                    matriculas = Consultar(contextQuery);
                }
                catch (Exception exception)
                {
                    string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine, Convert.ToString(exception.Message));
                    throw new Exception(mensagem);
                }
            }

            return matriculas;
        }

        private int ObtemQtdeAlunoMatriculadoEdEspecialPor(int ano, int semestre, string turma)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            int total = 0;
            SqlDataReader reader = null;

            try
            {
                contextQuery.Command = @" SELECT COUNT(DISTINCT ALUNO) as TOTAL
                                            FROM   LY_MATRICULA M                                                  
												  
                                            WHERE  SIT_MATRICULA ='Matriculado' 
                                                   AND M.ANO = @ANO
                                                   AND M.SEMESTRE = @SEMESTRE 
                                                   AND m.TURMA = @TURMA 
												   AND EDUC_ESPECIAL = 'S'
                                              ";


                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@TURMA", turma);


                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    total = Convert.ToInt32(reader["TOTAL"]);
                }
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
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return total;
        }

        public static void InserirSalaRecurso(LyMatricula salaRecurso)
        {
            using (var context = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    //Consultar Disciplinas da turma
                    var disciplinas = Matricula.ConsultaDisciplinaGrade(Convert.ToString(salaRecurso.Ano), Convert.ToString(salaRecurso.Semestre), salaRecurso.Turma);

                    foreach (DataRow disciplinaRow in disciplinas.Rows)
                    {
                        salaRecurso.Disciplina = disciplinaRow["disciplina"].ToString();

                        //verificar se ja existe um deles cancelado na ly_matricula
                        var contextQuery = new ContextQuery(
                            @" SELECT  COUNT(*)
                                                    FROM    LY_MATRICULA
                                                    WHERE   ALUNO = @ALUNO
                                                            AND DISCIPLINA = @DISCIPLINA
                                                            AND TURMA = @TURMA
                                                            AND ANO = @ANO
                                                            AND SEMESTRE = @SEMESTRE
                                                            AND SIT_MATRICULA <> @SIT_MATRICULA ");

                        contextQuery.Parameters.Add("@ALUNO", salaRecurso.Aluno);
                        contextQuery.Parameters.Add("@DISCIPLINA", salaRecurso.Disciplina);
                        contextQuery.Parameters.Add("@TURMA", salaRecurso.Turma);
                        contextQuery.Parameters.Add("@ANO", salaRecurso.Ano);
                        contextQuery.Parameters.Add("@SEMESTRE", salaRecurso.Semestre);
                        contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);

                        var idCancelada = context.GetReturnValue<int>(contextQuery);

                        //se existir atualiza senao insere
                        if (idCancelada > 0)
                        {
                            Matricula.AtualizarEducacaoEspecial(context, salaRecurso);
                        }
                        else
                        {
                            Matricula.InserirEducacaoEspecial(context, salaRecurso);
                        }
                    }

                    //carregar dados da turma
                    var lyTurma = RN.Turma.Carregar(Convert.ToInt32(salaRecurso.Ano),
                                                    Convert.ToInt32(salaRecurso.Semestre),
                                                    salaRecurso.Turma);

                    //obter grade_id
                    var gradeId = GradeSerie.ObterGradeId(
                        context,
                        salaRecurso.Ano,
                        salaRecurso.Semestre,
                        lyTurma.Curso,
                        lyTurma.Curriculo,
                        lyTurma.Serie,
                        salaRecurso.Turma);

                    //Obter dados e atualizar matgrade
                    context.ApplyModifications(
                        new ContextQuery(
                            string.Format(
                                @"DECLARE @aluno T_CODIGO,
                                                        @grade_id T_NUMERO_GRANDE,
                                                        @sit_matgrade T_SIT_MATGRADE	
                                                                                    		
                                                    SET @aluno = '{0}'
                                                    SET @grade_id = {1}
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
                                                                ) ",
                                salaRecurso.Aluno,
                                gradeId)));
                }
                catch (Exception)
                {
                    context.Abandon();
                    throw;
                }
            }
        }

        public static void RemoverSalaRecurso(LyMatricula matricula)
        {
            using (var context = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    Matricula.RemoverSalaRecurso(context, matricula);

                    //carregar dados da turma
                    var lyTurma = RN.Turma.Carregar(Convert.ToInt32(matricula.Ano),
                                                    Convert.ToInt32(matricula.Semestre),
                                                    matricula.Turma);
                    //obter grade_id
                    var gradeId = GradeSerie.ObterGradeId(
                        context,
                        matricula.Ano,
                        matricula.Semestre,
                        lyTurma.Curso,
                        lyTurma.Curriculo,
                        lyTurma.Serie,
                        matricula.Turma);

                    RN.Matgrade.Remover(context, matricula.Aluno, gradeId);
                }
                catch (Exception)
                {
                    context.Abandon();
                    throw;
                }
            }
        }

        public static void RemoverSalaRecurso(DataContext context, LyMatricula matricula)
        {
            var contextQuery = new ContextQuery(
                @" UPDATE  LY_MATRICULA
                SET     SIT_MATRICULA = @SIT_MATRICULA, 
		                STAMP_ATUALIZACAO = GETDATE(),
                        DT_ULTALT = GETDATE(),
                        MATRICULA = @MATRICULA                        
                WHERE   ALUNO = @ALUNO                      
                        AND TURMA = @TURMA
                        AND ANO = @ANO
                        AND SEMESTRE = @SEMESTRE
                        AND SIT_MATRICULA = 'Matriculado' 
                        AND EDUC_ESPECIAL = 'S' ");

            contextQuery.Parameters.Add("@SIT_MATRICULA", Cancelado);
            contextQuery.Parameters.Add("@MATRICULA", matricula.Matricula);
            contextQuery.Parameters.Add("@ALUNO", matricula.Aluno);
            contextQuery.Parameters.Add("@TURMA", matricula.Turma);
            contextQuery.Parameters.Add("@ANO", matricula.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", matricula.Semestre);

            context.ApplyModifications(contextQuery);
        }


        public string RetornaTextoEmailConvocacaoPor(int ano)
        {
           
            string retorno = string.Empty;

            retorno = string.Format(@"<br />No momento da confirmação deverão ser apresentados os seguintes documentos:
                                        <br />
                                        <br />1 - Certidão de Nascimento ou Casamento; 
                                        <br />2 - Carteira de Identidade ou documento que a substitua e CPF, se possuir.  A Original será devolvida no ato;
                                        <br />3 - Histórico Escolar ou Declaração da última unidade escolar em que estudou, constando a série para a qual o(a) aluno(a) está habilitado(a), ficando o original na escola;
                                        <br />4 - Carteira de identidade e CPF do responsável legal, no caso de menor de 18 anos, original e cópia;
                                        <br />5 - Laudo comprobatório de deficiências declaradas (se for o caso), na forma prevista no § 3º, art. 19, da Resolução SEEDUC Nº 6375/2025;
                                        <br />6 - Comprovante de residência com o mesmo endereço informado no ato da inscrição na 1ª Fase da Matrícula Fácil 2026;
                                        <br />7 - Documento que comprove o vínculo familiar, no caso do candidato ter declarado possuir irmão matriculado ou pleiteante à vaga na Rede Estadual da SEEDUC, para o ano letivo desejado.                                           
                                        <br />
                                        <br />Importante:
                                        <br />Na falta de alguma documentação listada acima, solicite no momento da Confirmação de Matrícula o TERMO DE RESPONSABILIDADE (ANEXO II da RESOLUÇÃO SEEDUC Nº **** de ** de ******** de 2025).
                                        <br />
                                        <br />Para maiores informações, acesse www.matriculafacil.rj.gov.br.
                                        <br />
                                        <br />Seeduc", ano);

            return retorno;
        }


        public bool PossuiMatriculaEducacaoEspecialPor(DataContext ctx, string aluno, string curso, string turno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;
            int total = 0;

            contextQuery.Command = @" SELECT  COUNT(*)
                            FROM    DBO.LY_MATRICULA M
                            INNER JOIN DBO.LY_TURMA T ( NOLOCK ) ON T.TURMA = M.TURMA
                                                                        AND T.ANO = M.ANO
                                                                        AND T.SEMESTRE = M.SEMESTRE
                                                                        AND T.DISCIPLINA = M.DISCIPLINA
                                INNER JOIN DBO.LY_DISCIPLINA D ( NOLOCK ) ON M.DISCIPLINA = D.DISCIPLINA 
                            WHERE   M.SIT_MATRICULA = @SIT_MATRICULA
                                    AND M.ALUNO = @ALUNO
                                    AND T.CURSO = @CURSO
                                    AND EDUC_ESPECIAL = 'S' ";

            if (turno == "I")
            {
                contextQuery.Command += " AND T.TURNO IN ('M','T')";
            }

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@CURSO", curso);
            contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);

            total = ctx.GetReturnValue<int>(contextQuery);

            if (turno == "I")
            {
                if (total == 2)
                {
                    existe = true;
                }
            }
            else
            {
                if (total > 0)
                {
                    existe = true;
                }
            }         

            return existe;
        }

        public DateTime ObtemDataMatriculaEnturmacaoPor(int ano,int periodo, string turma, string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            DateTime data = new DateTime();

            contextQuery.Command = @" SELECT  DISTINCT MAX(M.DT_MATRICULA) DT_MATRICULA
                        FROM    DBO.LY_MATRICULA M       
                            INNER JOIN DBO.LY_TURMA T ( NOLOCK ) ON T.TURMA = M.TURMA
                                                                        AND T.ANO = M.ANO
                                                                        AND T.SEMESTRE = M.SEMESTRE
                                                                        AND T.DISCIPLINA = M.DISCIPLINA
                                WHERE M.ANO = @ANO     
                                    AND ALUNO = @ALUNO                       
                                    AND M.SEMESTRE = @SEMESTRE
                                    AND ( M.DEPENDENCIA IS NULL
                                          OR M.DEPENDENCIA = 'N'
                                        )
                                    AND ( M.CONCOMITANTE IS NULL
                                          OR M.CONCOMITANTE = 'N'
                                        )
                                    AND ( M.EDUC_ESPECIAL IS NULL
                                          OR M.EDUC_ESPECIAL = 'N'
                                        )
                                    AND ( M.MAIS_EDUCACAO IS NULL
                                          OR M.MAIS_EDUCACAO = 'N'
                                        )
                                    AND t.OPTATIVAREFORCO = 'N'
                                    AND ISNULL(T.ELETIVA,'N') = 'N'
                                
                               ";

            if (!turma.IsNullOrEmptyOrWhiteSpace() && turma != "Todas")
            {
                contextQuery.Command += " AND M.TURMA = @TURMA";
                contextQuery.Parameters.Add("@TURMA", turma);
            }

            contextQuery.Parameters.Add("@ALUNO", aluno);
           
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", periodo);

            reader = ctx.GetDataReader(contextQuery);

            while (reader.Read())
            {
                data = Convert.ToDateTime(reader["DT_MATRICULA"]);
            }

            if (reader != null)
            {
                reader.Close();
            }

            return data;
        }


        public void RemoveMatriculaLetramentoNOA(LyMatricula matricula)
        {
            LyTurma turma = new LyTurma();
            string gradeId = string.Empty;

            using (var context = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    this.RemoveMatriculaLetramentoNOA(context, matricula);

                    //carregar dados da turma
                    turma = RN.Turma.Carregar(Convert.ToInt32(matricula.Ano),
                                                    Convert.ToInt32(matricula.Semestre),
                                                    matricula.Turma);
                    //obter grade_id
                    gradeId = GradeSerie.ObterGradeId(
                        context,
                        matricula.Ano,
                        matricula.Semestre,
                        turma.Curso,
                        turma.Curriculo,
                        turma.Serie,
                        matricula.Turma);

                    RN.Matgrade.Remover(context, matricula.Aluno, gradeId);
                }
                catch (Exception exception)
                {
                    string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine, Convert.ToString(exception.Message));
                    throw new Exception(mensagem);
                }
            }
        }

        public void RemoveMatriculaLetramentoNOA(DataContext dataContext, LyMatricula matricula)
        {
            var contextQuery = new ContextQuery(
                @" UPDATE  LY_MATRICULA
                    SET     SIT_MATRICULA = @SIT_MATRICULA, 
		                    STAMP_ATUALIZACAO = GETDATE(),
                            DT_ULTALT = GETDATE(), 
                            MATRICULA = @MATRICULA
                    WHERE   ALUNO = @ALUNO        
                            AND TURMA = @TURMA
                            AND ANO = @ANO
                            AND SEMESTRE = @SEMESTRE ");

            contextQuery.Parameters.Add("@SIT_MATRICULA", Cancelado);
            contextQuery.Parameters.Add("@MATRICULA", matricula.Matricula);
            contextQuery.Parameters.Add("@ALUNO", matricula.Aluno);
            contextQuery.Parameters.Add("@TURMA", matricula.Turma);
            contextQuery.Parameters.Add("@ANO", matricula.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", matricula.Semestre);

            dataContext.ApplyModifications(contextQuery);
        }

        public DataTable ListaMatriculaAtivaLetramento(string aluno)
        {
            DataTable matriculasOptativaReforco = null;
            ContextQuery contextQuery = new ContextQuery();

            if (!string.IsNullOrEmpty(aluno))
            {
                try
                {
                    contextQuery.Command = @" SELECT DISTINCT M.ALUNO, M.ANO ,
                                M.SEMESTRE ,
                                M.TURMA ,                             
                                TU.DESCRICAO AS TURNO,
                                CONVERT(VARCHAR(10), M.DT_MATRICULA, 103) as DT_MATRICULA 
                        FROM    DBO.LY_MATRICULA M ( NOLOCK )
                                INNER JOIN DBO.LY_TURMA T ( NOLOCK ) ON T.TURMA = M.TURMA
                                                                        AND T.ANO = M.ANO
                                                                        AND T.SEMESTRE = M.SEMESTRE
                                                                        AND T.DISCIPLINA = M.DISCIPLINA
                                INNER JOIN LY_TURNO TU ON T.TURNO = TU.TURNO
                        WHERE   M.SIT_MATRICULA = 'MATRICULADO'
                                AND ISNULL(M.MAIS_EDUCACAO, 'N') = 'L'
                                AND ALUNO = @ALUNO ";

                    contextQuery.Parameters.Add("@ALUNO", aluno);

                    matriculasOptativaReforco = Consultar(contextQuery);
                }
                catch (Exception exception)
                {
                    string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine, Convert.ToString(exception.Message));
                    throw new Exception(mensagem);
                }
            }

            return matriculasOptativaReforco;
        }

        public ValidacaoDados ValidaMatriculaLetramento(LyMatricula matriculaOptativaReforco)
        {
            List<string> mensagens = new List<string>();
            string retorno = string.Empty;
            string tipo = string.Empty;
            Turma rnTurma = new Turma();
            AulaDocente rnAulaDocente = new AulaDocente();

            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (matriculaOptativaReforco != null)
            {
                matriculaOptativaReforco.SitMatricula = Matriculado;

                if (string.IsNullOrEmpty(matriculaOptativaReforco.Aluno))
                {
                    mensagens.Add("O aluno deve ser informado.");
                }

                if (matriculaOptativaReforco.Ano <= 0)
                {
                    mensagens.Add("O campo Ano é obrigatório!");
                }

                if (matriculaOptativaReforco.Semestre < 0)
                {
                    mensagens.Add("O campo Período é obrigatório!");
                }

                if (matriculaOptativaReforco.Turma.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("O campo Turma é obrigatório!");
                }

                if (matriculaOptativaReforco.Matricula.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("A matrícula do responsável é obrigatória!");
                }

                if (mensagens.Count == 0)
                {
                    if (!PossuiMatriculaRegularAtiva(matriculaOptativaReforco.Ano, matriculaOptativaReforco.Aluno))
                    {
                        mensagens.Add("Não foi encontrada matricula principal para este aluno.");
                    }                

                    //retorno = Matricula.AnalisarChoqueHorario(matriculaOptativaReforco);

                    //if (!string.IsNullOrEmpty(retorno))
                    //{
                    //    mensagens.Add("Encontrado choque de horário para esta aluno/turma.");
                    //}

                    using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                    {
                        //verifica se a aluno já tem matricula outra matricula ativa para a turma
                        var contextQuery = new ContextQuery(
                            @" SELECT  COUNT(*)
                            FROM    DBO.LY_MATRICULA M
                            WHERE   M.SIT_MATRICULA = @SIT_MATRICULA
                                    AND M.ALUNO = @ALUNO
                                    AND DISCIPLINA = @DISCIPLINA 
                                    AND ANO = @ANO
                                    AND SEMESTRE = @SEMESTRE
                                    AND TURMA = @TURMA");

                        contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
                        contextQuery.Parameters.Add("@ALUNO", matriculaOptativaReforco.Aluno);
                        contextQuery.Parameters.Add("@DISCIPLINA", matriculaOptativaReforco.Disciplina);
                        contextQuery.Parameters.Add("@ANO", matriculaOptativaReforco.Ano);
                        contextQuery.Parameters.Add("@SEMESTRE", matriculaOptativaReforco.Semestre);
                        contextQuery.Parameters.Add("@TURMA", matriculaOptativaReforco.Turma);

                        var matriculas = ctx.GetReturnValue<int>(contextQuery);

                        if (matriculas > 0)
                        {
                            mensagens.Add("Já existe enturmação ATIVA para este aluno nesta turma!");
                        }

                        //Verifica se o aluno já possui enturmação no letramento
                        if (this.PossuiMatriculaLetramento(ctx, matriculaOptativaReforco.Aluno))
                        { 
                            mensagens.Add("Já existe enturmação em Letramento ATIVA para este aluno!"); 
                        }
                        
                        //Se for turma de reforço valida se existe professor alocado
                        //if (!rnAulaDocente.ExisteDocentesRealEmAulaAtivaPor(ctx, matriculaOptativaReforco.Turma, matriculaOptativaReforco.Ano, matriculaOptativaReforco.Semestre, matriculaOptativaReforco.Disciplina))
                        //{
                        //    mensagens.Add(string.Format("A turma de reforço {0} não possui professor alocado, para realizar a enturmação primeiro realize a alocação do professor.", matriculaOptativaReforco.Turma));
                        //}
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
            }

            return validacaoDados;
        }

        public void SalvaMatriculaLetramentoNOA(LyMatricula matricula)
        {
            LyTurma dadosTurma = new LyTurma();
            Matgrade matgradeRN = new Matgrade();
            string gradeId = string.Empty;

            using (var dataContext = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    if (this.EhMatriculaCancelada(matricula))
                    {
                        this.AtualizaMatriculaLetramento(dataContext, matricula);
                    }
                    else
                    {
                        this.InsereMatriculaLetramentoNOA(dataContext, matricula);
                    }

                    dadosTurma = RN.Turma.Carregar(Convert.ToInt32(matricula.Ano),
                                                    Convert.ToInt32(matricula.Semestre),
                                                    matricula.Turma);

                    gradeId = GradeSerie.ObterGradeId(
                        dataContext,
                        matricula.Ano,
                        matricula.Semestre,
                        dadosTurma.Curso,
                        dadosTurma.Curriculo,
                        dadosTurma.Serie,
                        matricula.Turma);
                    if (gradeId != null)
                        matgradeRN.Insere(matricula.Aluno, gradeId, dataContext);

                }
                catch (Exception exception)
                {
                    string mensagem = string.Format("Não foi encontrado a grade curricular para essa turma.{0}Erro gerado: {1}",
                            Environment.NewLine, Convert.ToString(exception.Message));
                    throw new Exception(mensagem);
                }
            }
        }

        public void AtualizaMatriculaLetramento(DataContext dataContext, LyMatricula matricula)
        {
            var contextQuery = new ContextQuery(
                @" UPDATE  LY_MATRICULA
                    SET     SIT_MATRICULA = @SIT_MATRICULA ,
                            STAMP_ATUALIZACAO = GETDATE() ,
                            MATRICULA = @MATRICULA ,
                            CONCOMITANTE = 'N' ,
                            DEPENDENCIA = 'N' ,
                            SERIE_REFERENCIA = NULL, 
                            DISCIPLINA_REFERENCIA = NULL, 
                            EDUC_ESPECIAL = 'N' 
                    WHERE   ALUNO = @ALUNO                           
                            AND TURMA = @TURMA
                            AND ANO = @ANO
                            AND SEMESTRE = @SEMESTRE  ");

            contextQuery.Parameters.Add("@SIT_MATRICULA", TechneDbType.T_SIT_MATRICULA, Matriculado);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, matricula.Matricula);
            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, matricula.Aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", TechneDbType.T_CODIGO, matricula.Disciplina);
            contextQuery.Parameters.Add("@TURMA", matricula.Turma);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, matricula.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, matricula.Semestre);

            dataContext.ApplyModifications(contextQuery);
        }


        public void InsereMatriculaLetramentoNOA(DataContext ctx, LyMatricula matricula)
        {
            //Verifica se Aluno já possaui a matricula de alguma disciplina como cancelada
            if (this.PossuiAlgumaDisciplinaCanceladaPorTurma(matricula))
            {
                //SE possui insere as disciplinas que não possuir e atualiza as que possuir
                this.InsereAtualizaDisciplinasAtivasPorTurmaLetramentoNOA(ctx, matricula);
            }
            else
            {
                //Insere todas as disciplinas da turma
                this.InsereDisciplinasAtivasPorTurmaLetramentoNOA(ctx, matricula);
            }
        }

        public void InsereAtualizaDisciplinasAtivasPorTurmaLetramentoNOA(DataContext ctx, LyMatricula matricula)
        {
            ContextQuery contextQuery = new ContextQuery();
            contextQuery.Command = @" --Atualiza disciplinas que já existem canceladas 
                    UPDATE M 
                    SET    SIT_MATRICULA = @SIT_MATRICULA, 
                           DT_ULTALT = Getdate(), 
                           DT_MATRICULA = Getdate(), 
                           CONCOMITANTE = 'N',
                           DEPENDENCIA = 'N',
                           SERIE_REFERENCIA = NULL, 
                           DISCIPLINA_REFERENCIA = NULL, 
                           EDUC_ESPECIAL = 'N',
                           MAIS_EDUCACAO = @MAIS_EDUCACAO,
                           MATRICULA = @MATRICULA 
                    FROM   ly_matricula m 
                           INNER JOIN ly_turma t 
                                   ON M.ano = T.ano 
                                      AND M.semestre = T.semestre 
                                      AND M.turma = T.turma 
                                      AND m.disciplina = t.disciplina 
							INNER JOIN LY_CURSO C ( NOLOCK ) ON C.CURSO = T.CURSO
							INNER JOIN LY_SERIE S ON T.CURRICULO = S.CURRICULO
														AND T.TURNO = S.TURNO
														AND T.CURSO = S.CURSO  
														AND T.SERIE = S.SERIE     
                    WHERE  m.aluno = @ALUNO 
                           AND m.turma = @TURMA 
                           AND m.ano = @ANO 
                           AND m.semestre = @SEMESTRE 
                           AND T.sit_turma = 'Aberta' 
						   and (S.OFERTAELETIVA = 'N' or (S.OFERTAELETIVA = 'S' AND ISNULL(t.ELETIVA, 'N') = 'N'))

                    --Insere Disciplinas que ainda nao existem 
                    INSERT INTO dbo.ly_matricula 
                                (aluno, 
                                 disciplina, 
                                 turma, 
                                 ano, 
                                 semestre, 
                                 sit_matricula,
                                 MAIS_EDUCACAO,
                                 dt_ultalt, 
                                 cobranca_sep, 
                                 dt_insercao, 
                                 dt_matricula, 
                                 matricula) 
                    SELECT @ALUNO, 
                           T.disciplina, 
                           T.turma, 
                           T.ano, 
                           T.semestre, 
                           @SIT_MATRICULA,
                           @MAIS_EDUCACAO  ,
                           Getdate(), 
                           'N', 
                           Getdate(), 
                           Getdate(), 
                           @MATRICULA 
                    FROM   ly_turma T 
							INNER JOIN LY_CURSO C ( NOLOCK ) ON C.CURSO = T.CURSO
							INNER JOIN LY_SERIE S ON T.CURRICULO = S.CURRICULO
														AND T.TURNO = S.TURNO
														AND T.CURSO = S.CURSO  
														AND T.SERIE = S.SERIE
                           LEFT JOIN ly_matricula m 
                                  ON M.ano = T.ano 
                                     AND M.semestre = T.semestre 
                                     AND M.turma = T.turma 
                                     AND m.disciplina = t.disciplina 
                                     AND M.ALUNO=@ALUNO
                    WHERE  m.aluno IS NULL 
                           AND T.sit_turma = 'Aberta' 
                           AND T.turma = @TURMA 
                           AND T.ano = @ANO 
                           AND T.semestre = @SEMESTRE
						   and (S.OFERTAELETIVA = 'N' or (S.OFERTAELETIVA = 'S' AND ISNULL(t.ELETIVA, 'N') = 'N'))  ";

            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, matricula.Aluno);
            contextQuery.Parameters.Add("@TURMA", matricula.Turma);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, matricula.Ano);
            contextQuery.Parameters.Add("@SIT_MATRICULA", TechneDbType.T_SIT_MATRICULA, Matriculado);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, matricula.Matricula);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, matricula.Semestre);
            contextQuery.Parameters.Add("@MAIS_EDUCACAO", SqlDbType.VarChar, matricula.MaisEducacao);


            ctx.ApplyModifications(contextQuery);
        }

        public void InsereDisciplinasAtivasPorTurmaLetramentoNOA(DataContext ctx, LyMatricula matricula)
        {
            ContextQuery contextQuery = new ContextQuery();
            contextQuery.Command = @" INSERT INTO DBO.LY_MATRICULA 
                                    (ALUNO, 
                                     DISCIPLINA, 
                                     TURMA, 
                                     ANO, 
                                     SEMESTRE, 
                                     SIT_MATRICULA, 
                                     MAIS_EDUCACAO,
                                     DT_ULTALT, 
                                     COBRANCA_SEP, 
                                     DT_INSERCAO, 
                                     DT_MATRICULA, 
                                     MATRICULA) 
                        SELECT DISTINCT @ALUNO, 
                               T.DISCIPLINA, 
                               T.TURMA, 
                               T.ANO, 
                               T.SEMESTRE, 
                               @SIT_MATRICULA, 
                               @MAIS_EDUCACAO,
                               GETDATE(), 
                               'N', 
                               GETDATE(), 
                               GETDATE(), 
                               @MATRICULA 
                        FROM   LY_TURMA T 
							   INNER JOIN LY_CURSO C ( NOLOCK ) ON C.CURSO = T.CURSO
							   INNER JOIN LY_SERIE S ON T.CURRICULO = S.CURRICULO
														AND T.TURNO = S.TURNO
														AND T.CURSO = S.CURSO  
														AND T.SERIE = S.SERIE
                        WHERE  T.SIT_TURMA = 'Aberta' 
                               AND t.TURMA = @TURMA 
                               AND t.ANO = @ANO 
                               AND t.SEMESTRE = @SEMESTRE 
							   and (S.OFERTAELETIVA = 'N' or (S.OFERTAELETIVA = 'S' AND ISNULL(t.ELETIVA, 'N') = 'N'))  ";

            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, matricula.Aluno);
            contextQuery.Parameters.Add("@TURMA", matricula.Turma);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, matricula.Ano);
            contextQuery.Parameters.Add("@SIT_MATRICULA", TechneDbType.T_SIT_MATRICULA, Matriculado);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, matricula.Matricula);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, matricula.Semestre);
            contextQuery.Parameters.Add("@MAIS_EDUCACAO", SqlDbType.VarChar, matricula.MaisEducacao);

            ctx.ApplyModifications(contextQuery);
        }

        public DataTable ListaMatriculaSemLetramentoPor(string aluno)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT T.TURMA, 
		                                            CASE
			                                            WHEN ISNULL(M.DEPENDENCIA, 'N') = 'S' THEN 'PROGRESSÃO PARCIAL'
			                                            WHEN ISNULL(M.EDUC_ESPECIAL, 'N') = 'S' THEN 'EDUCAÇÃO ESPECIAL'
			                                            WHEN ISNULL(M.CONCOMITANTE, 'N') = 'S' THEN 'EDUCAÇÃO PROFISSIONAL CONCOMITANTE'
			                                            WHEN ISNULL(M.MAIS_EDUCACAO, 'N') = 'S' THEN 'MAIS EDUCAÇÃO'
                                                        WHEN ISNULL(M.MAIS_EDUCACAO, 'N') in ('P','V') THEN 'FOCO'
                                                        WHEN ISNULL(M.MAIS_EDUCACAO, 'N') = 'L' THEN 'LETRAMENTO'
                                                        WHEN ISNULL(M.MAIS_EDUCACAO, 'N') = 'O' THEN 'NOA'
			                                            WHEN ISNULL(T.OPTATIVAREFORCO, 'N') <> 'N' THEN 'OPTATIVA'
			                                            WHEN ISNULL(T.ELETIVA, 'N') = 'S' THEN 'ELETIVA'
			                                            ELSE 'PRINCIPAL'
		                                            END TIPO,
		                                            C.NOME AS CURSO,
		                                            TU.DESCRICAO AS TURNO,
		                                            M.ALUNO
                                            FROM LY_MATRICULA M
	                                            INNER JOIN LY_TURMA T ON M.ANO = T.ANO 
							                                            AND M.SEMESTRE = T.SEMESTRE 
							                                            AND T.TURMA = M.TURMA
							                                             AND T.DISCIPLINA = M.DISCIPLINA
	                                            INNER JOIN LY_CURSO C ON T.CURSO = C.CURSO
	                                            INNER JOIN LY_TURNO TU ON T.TURNO = TU.TURNO
                                            WHERE SIT_MATRICULA = 'Matriculado'
	                                            AND ISNULL(M.MAIS_EDUCACAO, 'N') <> 'L'
	                                            AND ALUNO = @ALUNO";

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


        public DataTable ListaMatriculaAtivaNOA(string aluno)
        {
            DataTable matriculasOptativaReforco = null;
            ContextQuery contextQuery = new ContextQuery();

            if (!string.IsNullOrEmpty(aluno))
            {
                try
                {
                    contextQuery.Command = @" SELECT DISTINCT M.ALUNO, M.ANO ,
                                M.SEMESTRE ,
                                M.TURMA ,                             
                                TU.DESCRICAO AS TURNO,
                                CONVERT(VARCHAR(10), M.DT_MATRICULA, 103) as DT_MATRICULA 
                        FROM    DBO.LY_MATRICULA M ( NOLOCK )
                                INNER JOIN DBO.LY_TURMA T ( NOLOCK ) ON T.TURMA = M.TURMA
                                                                        AND T.ANO = M.ANO
                                                                        AND T.SEMESTRE = M.SEMESTRE
                                                                        AND T.DISCIPLINA = M.DISCIPLINA
                                INNER JOIN LY_TURNO TU ON T.TURNO = TU.TURNO
                        WHERE   M.SIT_MATRICULA = 'MATRICULADO'
                                AND ISNULL(M.MAIS_EDUCACAO, 'N') = 'O' --NOA
                                AND ALUNO = @ALUNO ";

                    contextQuery.Parameters.Add("@ALUNO", aluno);

                    matriculasOptativaReforco = Consultar(contextQuery);
                }
                catch (Exception exception)
                {
                    string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine, Convert.ToString(exception.Message));
                    throw new Exception(mensagem);
                }
            }

            return matriculasOptativaReforco;
        }

        private bool PossuiMatriculaNOA(DataContext ctx, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*)
                                FROM    DBO.LY_MATRICULA M ( NOLOCK )
                                WHERE   M.SIT_MATRICULA = 'Matriculado'
                                        AND ISNULL(MAIS_EDUCACAO, 'N') = 'O'
                                        AND ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", aluno);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public ValidacaoDados ValidaMatriculaNOA(LyMatricula matricula)
        {
            List<string> mensagens = new List<string>();
            string retorno = string.Empty;
            string tipo = string.Empty;
            Turma rnTurma = new Turma();
            AulaDocente rnAulaDocente = new AulaDocente();

            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (matricula != null)
            {
                matricula.SitMatricula = Matriculado;

                if (string.IsNullOrEmpty(matricula.Aluno))
                {
                    mensagens.Add("O aluno deve ser informado.");
                }

                if (matricula.Ano <= 0)
                {
                    mensagens.Add("O campo Ano é obrigatório!");
                }

                if (matricula.Semestre < 0)
                {
                    mensagens.Add("O campo Período é obrigatório!");
                }

                if (matricula.Turma.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("O campo Turma é obrigatório!");
                }

                if (matricula.Matricula.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("A matrícula do responsável é obrigatória!");
                }

                if (mensagens.Count == 0)
                {
                    if (!PossuiMatriculaRegularAtiva(matricula.Ano, matricula.Aluno))
                    {
                        mensagens.Add("Não foi encontrada matricula principal para este aluno.");
                    }
                  

                    using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                    {
                        //verifica se a aluno já tem matricula outra matricula ativa para a turma
                        var contextQuery = new ContextQuery(
                            @" SELECT  COUNT(*)
                            FROM    DBO.LY_MATRICULA M
                            WHERE   M.SIT_MATRICULA = @SIT_MATRICULA
                                    AND M.ALUNO = @ALUNO
                                    AND DISCIPLINA = @DISCIPLINA 
                                    AND ANO = @ANO
                                    AND SEMESTRE = @SEMESTRE
                                    AND TURMA = @TURMA");

                        contextQuery.Parameters.Add("@SIT_MATRICULA", Matriculado);
                        contextQuery.Parameters.Add("@ALUNO", matricula.Aluno);
                        contextQuery.Parameters.Add("@DISCIPLINA", matricula.Disciplina);
                        contextQuery.Parameters.Add("@ANO", matricula.Ano);
                        contextQuery.Parameters.Add("@SEMESTRE", matricula.Semestre);
                        contextQuery.Parameters.Add("@TURMA", matricula.Turma);

                        var matriculas = ctx.GetReturnValue<int>(contextQuery);

                        if (matriculas > 0)
                        {
                            mensagens.Add("Já existe enturmação ATIVA para este aluno nesta turma!");
                        }

                        //Verifica se o aluno já possui enturmação no letramento
                        if (this.PossuiMatriculaNOA(ctx, matricula.Aluno))
                        {
                            mensagens.Add("Já existe enturmação no Reforço NOA ATIVA para este aluno!");
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
            }

            return validacaoDados;
        }

        public DataTable ListaMatriculaSemNOAPor(string aluno)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT T.TURMA, 
		                                            CASE
			                                            WHEN ISNULL(M.DEPENDENCIA, 'N') = 'S' THEN 'PROGRESSÃO PARCIAL'
			                                            WHEN ISNULL(M.EDUC_ESPECIAL, 'N') = 'S' THEN 'EDUCAÇÃO ESPECIAL'
			                                            WHEN ISNULL(M.CONCOMITANTE, 'N') = 'S' THEN 'EDUCAÇÃO PROFISSIONAL CONCOMITANTE'
			                                            WHEN ISNULL(M.MAIS_EDUCACAO, 'N') = 'S' THEN 'MAIS EDUCAÇÃO'
                                                        WHEN ISNULL(M.MAIS_EDUCACAO, 'N') in ('P','V') THEN 'FOCO'
                                                        WHEN ISNULL(M.MAIS_EDUCACAO, 'N') = 'L' THEN 'LETRAMENTO'
                                                        WHEN ISNULL(M.MAIS_EDUCACAO, 'N') = 'O' THEN 'NOA'
			                                            WHEN ISNULL(T.OPTATIVAREFORCO, 'N') <> 'N' THEN 'OPTATIVA'
			                                            WHEN ISNULL(T.ELETIVA, 'N') = 'S' THEN 'ELETIVA'
			                                            ELSE 'PRINCIPAL'
		                                            END TIPO,
		                                            C.NOME AS CURSO,
		                                            TU.DESCRICAO AS TURNO,
		                                            M.ALUNO
                                            FROM LY_MATRICULA M
	                                            INNER JOIN LY_TURMA T ON M.ANO = T.ANO 
							                                            AND M.SEMESTRE = T.SEMESTRE 
							                                            AND T.TURMA = M.TURMA
							                                             AND T.DISCIPLINA = M.DISCIPLINA
	                                            INNER JOIN LY_CURSO C ON T.CURSO = C.CURSO
	                                            INNER JOIN LY_TURNO TU ON T.TURNO = TU.TURNO
                                            WHERE SIT_MATRICULA = 'Matriculado'
	                                            AND ISNULL(M.MAIS_EDUCACAO, 'N') <> 'O'
	                                            AND ALUNO = @ALUNO";

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


    }
}