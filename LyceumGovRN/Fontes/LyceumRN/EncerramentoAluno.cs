using System;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN.Entidades;
using System.Linq;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.DTOs;
using System.Collections.Generic;

namespace Techne.Lyceum.RN
{
    public class EncerramentoAluno : RNBase
    {
        [Serializable]
        public class DadosExecucao
        {
            public bool Remove_PreMatricula { get; set; }

            public bool Remove_Disciplinas { get; set; }

            public string Situacao_Aluno { get; set; }

            public string Situacao_MatGrade { get; set; }

            public int Num_Chamada { get; set; }

            public bool Matricula { get; set; }

            public bool Cancela_Matricula { get; set; }

            public bool Busca_Carteirinha { get; set; }

            public bool Registra_EncMatGrade { get; set; }

            public bool Aluno_Formando { get; set; }

            public int Id_Grade { get; set; }

            public string Curso { get; set; }

            public int Serie { get; set; }

            public string Turno { get; set; }

            public string Curriculo { get; set; }

            public string Turma { get; set; }

            //Campos para permuta de aluno 
            public bool RealizaPermuta { get; set; }

            public bool ReservaVaga { get; set; }

            public string AlunoPermuta { get; set; }

            public bool EnsinoReligioso { get; set; }

            public bool LinguaEstrangeira { get; set; }

            public string Censo { get; set; }

            public string MotivoPermuta { get; set; }

            public string NecessidadeEspecialAlunoPermuta { get; set; }

            public string UsuarioResponsavel { get; set; }

            public DateTime DataNascimentoAlunoPermuta { get; set; }

            public bool PossuiEnturmacao { get; set; }

            public DadosTransferencia DadosTransferenciaPermuta { get; set; }

        }

        #region REABRIR

        //Atualiza Dados do aluno
        public static void AtualizaDadosReaberturaAluno(TConnectionWritable connection, LyAluno aluno)
        {
            string sql = @"UPDATE LY_ALUNO SET SIT_ALUNO = 'Ativo',                        
                        CURSO = ? ,
                        TURNO = ? ,
                        CURRICULO = ? ,
                        SERIE  = ?
                        WHERE ALUNO = ? ";
            IAE(connection, sql, aluno.Curso, aluno.Turno, aluno.Curriculo, aluno.Serie, aluno.Aluno);
        }

        //Atualiza a data de reabertura no histórico de conclusão do curso do aluno (LY_H_CURSOS_CONCL.DT_REABERTURA) com a data de hoje.
        //Se a tabela LY_H_CURSOS_CONCL possuir mais de um registro de encerramento sem a data de rabertura setada, deve-se reabrir somente o registro de maiot data de encerramento.

        public static void AtualizaDataReaberturaHistorico(TConnectionWritable connection, string aluno, string curso, string turno, string curriculo, DateTime data_reabr, string motivo, string anoReabertura, string periodoReabertura)
        {
            string sql = " UPDATE LY_H_CURSOS_CONCL SET DT_REABERTURA = ? , MOTIVOREABERTURA = ? ,DT_ULTALT = ? , OBSERVACAO = ? " +
                         " WHERE ALUNO = ? and DT_REABERTURA is null ";
            DateTime hoje = DateTime.Now;
            var obs = "Reabertura " + anoReabertura + "/" + periodoReabertura;

            IAE(connection, sql, data_reabr, motivo, hoje, obs, aluno);
        }

        public static void RetiraUsoGratuidade(TConnectionWritable connection, string aluno)
        {
            string sql = @" UPDATE  FP
                    SET     FL_FIELD_04 = 'N' ,
                            FL_FIELD_05 = NULL
                    FROM    dbo.LY_ALUNO A
                            INNER JOIN dbo.LY_FL_PESSOA FP ON A.PESSOA = FP.PESSOA
                    WHERE   ALUNO = ?
                            AND FL_FIELD_04 = 'S' ";

            IAE(connection, sql, aluno);
        }

        public static bool VerificaMatriculas(string aluno)
        {
            string sql = " select count(ALUNO) " +
                          "from LY_MATRICULA " +
                          "where ALUNO = ? ";
            int retorno = ExecutarFuncao(sql, aluno);

            if (retorno > 0)
                return true;
            else
                return false;
        }

        public static void AtivaMatriculas(TConnectionWritable connection, string aluno, DateTime data_reabr, int grade_id)
        {
            string sql = @"UPDATE LY_MATRICULA 
                    SET SIT_MATRICULA = 'Matriculado',
                    CONCOMITANTE = 'N',
                    DEPENDENCIA = 'N',
                    SERIE_REFERENCIA = NULL, 
                    DISCIPLINA_REFERENCIA = NULL, 
                    EDUC_ESPECIAL = 'N',
                    MAIS_EDUCACAO = 'N', 
                    DT_ULTALT = ? , 
                    DT_REABERTURA = ? 
                    WHERE ALUNO = ? 
                    AND ISNULL(DEPENDENCIA, 'N') = 'N'
					AND ISNULL(EDUC_ESPECIAL, 'N') = 'N'
					AND ISNULL(MAIS_EDUCACAO, 'N') = 'N'
					AND ISNULL(CONCOMITANTE, 'N') = 'N'
                    and DISCIPLINA + '*' + TURMA + '*' + convert(varchar,ANO) + '*' + convert(varchar,SEMESTRE) 
                    in (select  gt.DISCIPLINA + '*' + gt.TURMA + '*' + convert(varchar,gt.ANO) + '*' + convert(varchar,gt.SEMESTRE) 
                    from LY_GRADE_TURMA gt where gt.GRADE_ID = ?)";
            IAE(connection, sql, data_reabr, data_reabr, aluno, grade_id);
        }

        public static void VerificaCarteirinha(TConnectionWritable connection, string aluno, DateTime data_reabr)
        {
            string sql = "SELECT C.PESSOA as Pessoa, C.VIA_CARTEIRINHA as VIA_CARTEIRINHA " +
                        "FROM LY_CARTEIRINHA C JOIN LY_ALUNO A " +
                        "ON C.PESSOA = A.PESSOA " +
                        "WHERE C.ALUNO = ? " +
                        "ORDER BY VIA_CARTEIRINHA DESC";

            QueryTable qt = Consultar(connection, sql, aluno);

            if (qt != null)
            {
                if (qt.Rows.Count > 0)
                {
                    decimal pessoa = Convert.ToDecimal(qt.Rows[0]["Pessoa"]);
                    decimal via = Convert.ToDecimal(qt.Rows[0]["VIA_CARTEIRINHA"]);
                    if (pessoa != 0 && via != 0)
                    {
                        AtivaCarteirinha(connection, pessoa, via, aluno, data_reabr);
                    }
                }
            }
        }

        public static void AtivaCarteirinha(TConnectionWritable connection, decimal pessoa, decimal via, string aluno, DateTime data_reabr)
        {
            string sql = "UPDATE LY_CARTEIRINHA " +
                         "SET SIT_CARTEIRINHA = 'Ativa', " +
                         "MOTIVO = 'Regularmente matriculado', " +
                         "DATA_ALT_SITUACAO = ? " +
                         "WHERE PESSOA = ? " +
                         "AND ALUNO = ? " +
                         "AND VIA_CARTEIRINHA = ? ";
            IAE(connection, sql, data_reabr, pessoa, aluno, via);
        }

        public static int ConsultaMatGradePor(TConnectionWritable connection, string aluno, string ano, string periodo, string curso, string turno, string serie, string curriculo, string unidadeEnsino)
        {
            string sql = @" SELECT TOP 1 MG.GRADE_ID
                            FROM LY_MATGRADE MG
                            INNER JOIN LY_GRADE_SERIE GS ON MG.GRADE_ID=GS.GRADE_ID
                            INNER JOIN LY_TURMA T ON GS.ANO = T.ANO
                                 AND GS.SEMESTRE = T.SEMESTRE
                                 AND GS.GRADE = T.TURMA
                            WHERE MG.ALUNO = ? 
                            AND GS.ANO = ?
                            AND GS.SEMESTRE = ? 
                            AND GS.CURSO = ?
                            AND GS.TURNO = ?
                            AND GS.CURRICULO = ?
                            AND GS.SERIE = ?
                            AND GS.FACULDADE = ?
                            AND T.SIT_TURMA = 'Aberta'
                            AND MG.SIT_MATGRADE IN ('TRANSF.EXTERNAMENTE','CANCELADO','JUBILADO','CONCLUIDO','DESISTENTE')
                            ORDER BY MG.DT_ULTALT DESC ";

            return ExecutarFuncao(sql, connection, aluno, ano, periodo, curso, turno, curriculo, serie, unidadeEnsino);
        }

        public static void InsereMatGrade(TConnectionWritable connection, string aluno, int grade, DateTime data_reabr)
        {
            string sql = " Insert into LY_MATGRADE values(?,?,0,'Matriculado',?)";
            IAE(connection, sql, aluno, grade, data_reabr);
        }

        private static DataTable ConsultaDados(int gradeId)
        {
            return Consultar(
                new ContextQuery(
                    @"SELECT  g.ano AS ano,
                            g.semestre AS semestre,
                            g.curso AS curso,
                            g.serie AS serie,
                            g.turno AS turno,
                            g.curriculo AS curriculo,
                            g.grade AS turma 
                    FROM    ly_matgrade r
                            JOIN ly_grade_serie g ON r.grade_id = g.grade_id
                    WHERE   r.sit_matgrade = 'Matriculado'
                            AND g.grade_id = @GRADE_ID
                    GROUP BY g.ano,
                            g.semestre,
                            g.curso,
                            g.serie,
                            g.turno,
                            g.curriculo,
                            g.grade",
                    new ContextQueryParameter("@GRADE_ID", gradeId)));
        }

        private static QueryTable ConsultaDados(TConnectionWritable connection, int grade)
        {
            string sql = " SELECT g.ano as ano, g.semestre as semestre, g.curso as curso, " +
             "g.serie as serie, g.turno as turno, g.curriculo as curriculo, " +
             "g.grade as turma  " +
             "FROM ly_matgrade r join ly_grade_serie g " +
             "on r.grade_id = g.grade_id " +
             "Where r.sit_matgrade = 'Matriculado' " +
             "and g.grade_id = ? " +
             "group by g.ano, g.semestre, g.curso,  " +
             "g.serie, g.turno, g.curriculo,  " +
             "g.grade";

            return Consultar(connection, sql, grade);
        }

        public static void GeraNumChamada(TConnectionWritable connection, string aluno, int grade_id)
        {
            var ObjetoExecucao = new DadosExecucao();
            var novonumero = 0;
            var qt = ConsultaDados(connection, grade_id);

            if (qt != null)
            {
                if (qt.Rows.Count == 1)
                {
                    int ano = Convert.ToInt32(qt.Rows[0]["ano"]);
                    int semestre = Convert.ToInt32(qt.Rows[0]["semestre"]);
                    //if (qt.Rows[0]["numeromax"].ToString() != "")
                    //    ObjetoExecucao.Num_Chamada = Convert.ToInt32(qt.Rows[0]["numeromax"]);
                    //else
                    //    ObjetoExecucao.Num_Chamada = 0;
                    ObjetoExecucao.Curso = qt.Rows[0]["curso"].ToString();
                    ObjetoExecucao.Serie = Convert.ToInt16(qt.Rows[0]["serie"]);
                    ObjetoExecucao.Turno = qt.Rows[0]["turno"].ToString();
                    ObjetoExecucao.Curriculo = qt.Rows[0]["curriculo"].ToString();
                    ObjetoExecucao.Turma = qt.Rows[0]["turma"].ToString();

                    //Numero de chamada apenas será atualizado por job
                    //if (!string.IsNullOrEmpty(ObjetoExecucao.Turma))
                    //{
                    //    novonumero = ObjetoExecucao.Num_Chamada + 1;

                    //    AtualizaNumChamadaMatGrade(connection, novonumero, aluno, ObjetoExecucao.Id_Grade);
                    //    AtualizaNumChamadaMatricula(connection, novonumero, aluno, ano, semestre);

                    //    AtualizaNumChamadaTurma(connection, novonumero, ObjetoExecucao.Serie, ObjetoExecucao.Curso, ObjetoExecucao.Turno, ObjetoExecucao.Curriculo, ObjetoExecucao.Turma, ano, semestre);
                    //    AtualizaNumChamadaGrade(connection, novonumero, ObjetoExecucao.Serie, ObjetoExecucao.Curso, ObjetoExecucao.Turno, ObjetoExecucao.Curriculo, ObjetoExecucao.Turma, ano, semestre);
                    //}
                }
            }
        }

        //private static void AtualizaNumChamadaMatGrade(TConnectionWritable connection, int numero, string aluno, int grade)
        //{
        //    string sql = " Update LY_MATGRADE " +
        //           "SET NUM_CHAMADA = ? " +
        //           "WHERE ALUNO = ? " +
        //           "and sit_matgrade = 'Matriculado' " +
        //           "and grade_id = ?";
        //    IAE(connection, sql, numero, aluno, grade);
        //}

        //private static void AtualizaNumChamadaMatricula(TConnectionWritable connection, int numero, string aluno, int ano, int semestre)
        //{
        //    string sql = " Update LY_MATRICULA " +
        //                   "SET NUM_CHAMADA = ? " +
        //                 "WHERE ALUNO = ? " +
        //                   "AND ANO = ? " + //de onde vem ano e semestre?
        //                   "AND SEMESTRE = ? " +
        //                   "AND Sit_Matricula = 'Matriculado'";
        //    IAE(connection, sql, numero, aluno, ano, semestre);
        //}

        //private static void AtualizaNumChamadaTurma(TConnectionWritable connection, int numero, int serie, string curso, string turno, string curriculo, string turma, int ano, int semestre)
        //{
        //    string sql = " Update LY_TURMA " +
        //                   "SET ULT_NUM_CHAMADA = ? " +
        //                 "WHERE serie = ? " +
        //                   "AND CURSO = ? " +
        //                   "AND TURNO = ? " +
        //                   "AND CURRICULO = ? " +
        //                   "AND TURMA = ? " +
        //                   "AND ANO = ? " +
        //                   "AND SEMESTRE = ?";
        //    IAE(connection, sql, numero, serie, curso, turno, curriculo, turma, ano, semestre);
        //}

        //private static void AtualizaNumChamadaGrade(TConnectionWritable connection, int numero, int serie, string curso, string turno, string curriculo, string grade, int ano, int semestre)
        //{
        //    string sql = "  Update LY_GRADE_SERIE " +
        //                   "SET ULT_NUM_CHAMADA = ? " +
        //                 "WHERE serie = ? " +
        //                   "AND CURSO = ? " +
        //                   "AND TURNO = ? " +
        //                   "AND CURRICULO = ? " +
        //                   "AND GRADE = ? " +
        //                   "AND ANO = ? " +
        //                   "AND SEMESTRE = ?";
        //    IAE(connection, sql, numero, serie, curso, turno, curriculo, grade, ano, semestre);
        //}

        #endregion

        #region ENCERRAR
        public static QueryTable ConsultarCausaEncerr()
        {
            string sql = "select CAUSA_ENCERR, DESCRICAO from ly_causa_encerr";
            return Consultar(sql, "");
        }

        public static QueryTable ConsultarMotivoEncerr(string ativo)
        {
            string sql = "select MOTIVOSAIDA, DESCRICAO from ly_motivosaida WHERE IND_ATIVO_REABERTURA=? ";
            return Consultar(sql, ativo);
        }

        public static QueryTable ConsultarEncerramentos(string aluno)
        {
            string sql = "select a.ALUNO as aluno, a.CURSO as curso, c.NOME as nome_curso, a.TURNO as turno, t.DESCRICAO as nome_turno, a.CURRICULO as curriculo, " +
                         "a.SERIE as serie, a.ANO_INGRESSO as ano_ingresso, a.SEM_INGRESSO as periodo_ingresso, hc.DT_ENCERRAMENTO as dt_encerramento, hc.DT_REABERTURA as dt_reabertura, " +
                         "hc.MOTIVO as motivo, ms.DESCRICAO as nome_motivo, hc.OUTRA_FACULDADE as instituicao, i.NOME_COMP as nome_instituicao, " +
                         "hc.DT_COLACAO as dt_colacao, hc.DT_DIPLOMA as dt_diploma, hc.ANO_ENCERRAMENTO as ano_encerramento, " +
                         "hc.SEM_ENCERRAMENTO as periodo_encerramento, hc.CAUSA_ENCERR as causa, ce.DESCRICAO as nome_causa, hc.motivoreabertura " +
                         "from LY_ALUNO a inner join LY_CURSO c on a.CURSO = c.CURSO " +
                         "inner join LY_TURNO t on a.TURNO = t.TURNO " +
                         "inner join LY_H_CURSOS_CONCL hc on a.ALUNO = hc.ALUNO " +
                         "inner join LY_MOTIVOSAIDA ms on hc.MOTIVO = ms.MOTIVOSAIDA " +
                         "left join ly_instituicao i on hc.OUTRA_FACULDADE = i.OUTRA_FACULDADE " +
                         "left join LY_CAUSA_ENCERR ce on hc.CAUSA_ENCERR = ce.CAUSA_ENCERR " +
                         "where a.ALUNO = ? ORDER BY (hc.DT_ENCERRAMENTO) DESC";

            return Consultar(sql, aluno);
        }

        public static bool HabilitaEncerrar(string aluno)
        {

            string sql = "select 1 from LY_H_CURSOS_CONCL where DT_ENCERRAMENTO = (select MAX(DT_ENCERRAMENTO) from LY_H_CURSOS_CONCL WHERE ALUNO = ?) AND " +
                         "DT_REABERTURA is not null AND ALUNO = ? ";

            int retorno = ExecutarFuncao(sql, aluno, aluno);

            string sql_vazio = "select 1 from LY_H_CURSOS_CONCL WHERE ALUNO = ?";

            int retorno_vazio = ExecutarFuncao(sql_vazio, aluno);

            if (retorno == 1 || retorno_vazio != 1)
                return true;
            else
                return false;
        }

        public static bool HabilitaReabrir(string aluno)
        {

            string sql = "select 1 from LY_H_CURSOS_CONCL where DT_ENCERRAMENTO = (select MAX(DT_ENCERRAMENTO) from LY_H_CURSOS_CONCL WHERE ALUNO = ?) AND " +
                         "DT_REABERTURA is null AND ALUNO = ? AND MOTIVO <> 'OBITO'";

            int retorno = ExecutarFuncao(sql, aluno, aluno);

            if (retorno == 1)
                return true;
            else
                return false;
        }

        public static bool VerificaMatriculaAberta(string aluno)
        {
            string sql = "select 1 from LY_MATRICULA WHERE ALUNO = ? and sit_matricula in ('Aprovado', 'Rep Freq', 'Rep Nota')";

            int retorno = ExecutarFuncao(sql, aluno);

            if (retorno >= 1)
                return true;
            else
                return false;
        }

        public static bool VerificaMatricula(string aluno)
        {

            string sql = "select 1 from LY_MATRICULA WHERE ALUNO = ? and sit_matricula in ('Matriculado', 'Trancado')";

            int retorno = ExecutarFuncao(sql, aluno);

            if (retorno >= 1)
                return true;
            else
                return false;
        }

        public static QueryTable ConsultarMatriculaDisciplinas(string aluno)
        {
            string sql = "select m.disciplina as disciplina, dis.NOME as nome_disciplina, m.turma as turma, " +
                         "m.ano, m.semestre, m.sit_matricula from LY_MATRICULA m " +
                         "INNER JOIN LY_DISCIPLINA dis ON dis.DISCIPLINA = m.DISCIPLINA where ALUNO = ? " +
                         "AND sit_matricula in ('Matriculado', 'Trancado') ORDER BY DISCIPLINA";
            return Consultar(sql, aluno);
        }

        public static QueryTable AlunoFormandoSerie(string curso, string turno, string curriculo)
        {
            DbObject[] parametros = new DbObject[] { curso, turno, curriculo };

            string sql = "select MAX(SERIE_IDEAL) AS ULTIMA_SERIE from LY_GRADE " +
                         "WHERE CURSO = ? AND TURNO = ? AND CURRICULO = ? ";

            return Consultar(sql, parametros);
        }

        public static QueryTable DisciplinasUltimaSerie(string curso, string turno, string curriculo, string serie)
        {
            DbObject[] parametros = new DbObject[] { curso, turno, curriculo, serie };

            string sql = "select DISCIPLINA from LY_GRADE " +
                         "WHERE CURSO = ? AND TURNO = ? AND CURRICULO = ? " +
                         "AND SERIE_IDEAL = ? ";

            return Consultar(sql, parametros);
        }

        public static QueryTable SituacaoDisciplinasHistorico(string aluno, string serie, string disciplina)
        {
            DbObject[] parametros = new DbObject[] { aluno, serie, disciplina };

            string sql = "select SITUACAO_HIST from ly_histmatricula " +
                         "where aluno = ? and SERIE = ? and disciplina = ?";
            return Consultar(sql, parametros);
        }

        public static bool VerificaCurriculoSubSeq(string curso, string turno, string curriculo)
        {
            DbObject[] parametros = new DbObject[] { curso, turno, curriculo };
            string sql = "Select 1 from ly_curriculo c join ly_curriculo_seq s " +
                         "on  c.curso = s.curso and c.turno = s.turno and c.curriculo = s.curriculo " +
                         "where c.curso = ? and c.turno = ? and c.curriculo = ?";

            int retorno = ExecutarFuncao(sql, parametros);

            if (retorno >= 1)
                return true;
            else
                return false;
        }

        public void EncerrarAluno(Ly_h_cursos_concl dtEncerramento, DadosExecucao ObjetoExecucao, out string aviso)
        {
            Matriculas.VagaReservada rnVagaReservada = new Techne.Lyceum.RN.Matriculas.VagaReservada();
            Matriculas.Entidades.VagaReservada vagaReservada = new Techne.Lyceum.RN.Matriculas.Entidades.VagaReservada();
            Matriculas.DiasNaoLetivos rnDiasNaoLetivos = new Techne.Lyceum.RN.Matriculas.DiasNaoLetivos();
            Matriculas.OpcaoInscricao rnOpcaoInscricao = new Techne.Lyceum.RN.Matriculas.OpcaoInscricao();
            Matriculas.InscricaoAluno rnInscricaoAluno = new Techne.Lyceum.RN.Matriculas.InscricaoAluno();
            Matriculas.PessoaAluno rnPessoaAluno = new Techne.Lyceum.RN.Matriculas.PessoaAluno();
            ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            TransferenciaTurma rnTransferenciaTurma = new TransferenciaTurma();
            Carteirinha rnCarteirinha = new Carteirinha();
            RenovacaoMatricula.Renovacao rnRenovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Renovacao();
            HCursosConcl rnHCursosConcl = new HCursosConcl();
            RN.DTOs.DadosEmail dadosEmail = new DadosEmail();
            Turno rnTurno = new Turno();
            Aluno rnAluno = new Aluno();
            Matricula rnMatricula = new Matricula();
            Matgrade rnMatgrade = new Matgrade();
            DataContext contexto = null;
            DadosEnturmacaoAluno dadosEnturmacaoAluno = new DadosEnturmacaoAluno();
            DadosConfirmacaoCandidato dadosCandidato = new DadosConfirmacaoCandidato();
            DateTime prazoFinal = DateTime.Now.Date;
            int controleVagaId = 0;
            string municipio = string.Empty;
            aviso = string.Empty;
            bool emailEnviado = false;
            Matriculas.Entidades.ConvocacaoSemEmail convocacaoSemEmail = new Techne.Lyceum.RN.Matriculas.Entidades.ConvocacaoSemEmail();
            Matriculas.ConvocacaoSemEmail rnConvocacaoSemEmail = new Techne.Lyceum.RN.Matriculas.ConvocacaoSemEmail();
            RN.Matriculas.ContatoOpcaoInscricaoHist rnContatoOpcaoInscricaoHist = new RN.Matriculas.ContatoOpcaoInscricaoHist();
            RN.Matriculas.ContatoOpcaoInscricao rnContatoOpcaoInscricao = new RN.Matriculas.ContatoOpcaoInscricao();
            RN.Matriculas.OpcaoInscricaoHist rnOpcaoInscricaoHist = new RN.Matriculas.OpcaoInscricaoHist();
            RN.Turmas.HistoricoSuspensao rnHistoricoSuspensao = new Techne.Lyceum.RN.Turmas.HistoricoSuspensao();

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingLock();

                DateTime dt = Convert.ToDateTime(dtEncerramento.Rows[0].Dt_encerramento);

                //Busca dados da matricula atual do aluno
                dadosEnturmacaoAluno = rnMatricula.ObtemMatriculaPrincipalAtivaPor(contexto, dtEncerramento.Rows[0].Aluno);

                //Verifica se o aluno tem dados da enturmacao
                if (dadosEnturmacaoAluno.IdControleVaga > 0)
                {
                    controleVagaId = dadosEnturmacaoAluno.IdControleVaga;
                    municipio = dadosEnturmacaoAluno.MunicipioEscola;
                }
                else
                {
                    //Caso não tenha busca dados da confirmação
                    controleVagaId = rnConfirmacaoMatricula.ObtemControleVagaIdConfirmadoPor(contexto, dtEncerramento.Rows[0].Aluno, Convert.ToInt32(dtEncerramento.Rows[0].Ano_encerramento), Convert.ToInt32(dtEncerramento.Rows[0].Sem_encerramento), out municipio);
                }

                if (ObjetoExecucao.Cancela_Matricula)
                {
                    //Cancela o aluno
                    rnMatricula.CancelaMatriculaPor(contexto, dtEncerramento.Rows[0].Aluno, ObjetoExecucao.UsuarioResponsavel);
                }
                if (ObjetoExecucao.Busca_Carteirinha)
                {
                    //Busca dados de carteirinha ativa
                    DataTable qt = rnCarteirinha.RetornaCarteirinhaAtivaPor(contexto, dtEncerramento.Rows[0].Aluno);

                    if (qt.Rows.Count > 0)
                    {
                        decimal pessoa = Convert.ToDecimal(qt.Rows[0]["PESSOA"]);
                        string via_carteirinha = Convert.ToString(qt.Rows[0]["VIA_CARTEIRINHA"]);

                        //Bloqueia carteirinha
                        rnCarteirinha.BloqueiaCarteirinha(contexto, dtEncerramento.Rows[0].Motivo, pessoa, dtEncerramento.Rows[0].Aluno, via_carteirinha, dt);
                    }
                }

                if (ObjetoExecucao.Registra_EncMatGrade)
                {
                    //Cancela matgrade
                    rnMatgrade.CancelaMatgradePor(contexto, dtEncerramento.Rows[0].Aluno);
                }

                //Atualiza situação aluno aluno
                rnAluno.AtualizaSituacao(contexto, ObjetoExecucao.Situacao_Aluno, dtEncerramento.Rows[0].Aluno);

                //Verifica se existe registro na tabela de aluno correto
                if (rnPessoaAluno.PossuiPessoaAlunoPor(contexto, dtEncerramento.Rows[0].Aluno))
                {
                    //Atualiza dados de encerramento
                    rnPessoaAluno.Atualiza(contexto, dtEncerramento.Rows[0].Aluno, Convert.ToInt32(dtEncerramento.Rows[0].Ano_encerramento), Convert.ToInt32(dtEncerramento.Rows[0].Sem_encerramento), ObjetoExecucao.UsuarioResponsavel);
                }

                //Insere registro de encerramento
                rnHCursosConcl.Insere(contexto, dtEncerramento);

                if (dtEncerramento.Rows[0].Motivo != "CONCLUSAO")
                {
                    //Cancela confirmações de matricula
                    rnConfirmacaoMatricula.EncerraConfirmacoesMatricula(contexto, dtEncerramento.Rows[0].Aluno, Convert.ToInt32(dtEncerramento.Rows[0].Ano_encerramento), Convert.ToInt32(dtEncerramento.Rows[0].Sem_encerramento));

                    //Cancela renovaçoes de matricula
                    rnRenovacao.EncerraRenocacoesMatricula(contexto, dtEncerramento.Rows[0].Aluno);
                }

                //Atualiza Historico de Suspensao
                rnHistoricoSuspensao.EncerraSuspensaoMatricula(contexto, dtEncerramento.Rows[0].Aluno, ObjetoExecucao.UsuarioResponsavel);


                bool confirmado = false;

                int motivoRejeicaoInscricaoId = 29;//ALUNO ENCERRADO E RETIRADO DA FILA

                int opcaoIdFila = rnOpcaoInscricao.ObtemOpcaoIdPor(contexto, dtEncerramento.Rows[0].Aluno);

                //CASO O ALUNO ESTEJA NA FILA DE ALGUMA UNIDADE, REMOVE
                if (opcaoIdFila > 0)
                {
                    //Insere opção no historico
                    rnOpcaoInscricaoHist.Insere(contexto, opcaoIdFila, confirmado, motivoRejeicaoInscricaoId);

                    //Leva todos os contatos da opção para historio
                    rnContatoOpcaoInscricaoHist.Insere(contexto, opcaoIdFila);

                    //Remover todos os contatos da opção 
                    rnContatoOpcaoInscricao.Remove(contexto, opcaoIdFila);

                    //Remover opção 
                    rnOpcaoInscricao.Remove(contexto, opcaoIdFila);
                }

                //Verifica se vai realiza a reserva da vaga
                if (ObjetoExecucao.ReservaVaga)
                {
                    //Monta Vaga reservada  
                    vagaReservada.Aluno = dtEncerramento.Rows[0].Aluno;
                    vagaReservada.DataInicio = DateTime.Now;
                    vagaReservada.UsuarioId = ObjetoExecucao.UsuarioResponsavel;
                    vagaReservada.ControleVagaId = controleVagaId;
                    vagaReservada.DataFim = rnDiasNaoLetivos.RetornaDataFinalPor(contexto, DateTime.Now, 1, municipio);

                    //Verifica se existe vaga reservada
                    if (vagaReservada.ControleVagaId > 0)
                    {
                        //Colocar a vaga com dados de origem da tabela de reserva 24 horas
                        rnVagaReservada.Insere(contexto, vagaReservada);
                    }
                }
                else
                {
                    //Verfica se vai realizar a permuta
                    if (ObjetoExecucao.RealizaPermuta)
                    {
                        //Caso seja faz transferencia do aluno da permuta
                        rnTransferenciaTurma.TransfereTurmaPrincipal(contexto, ObjetoExecucao.DadosTransferenciaPermuta);
                    }
                    else
                    {
                        if (ObjetoExecucao.PossuiEnturmacao)
                        {
                            //Busca dados do proximo da fila
                            dadosCandidato = rnOpcaoInscricao.ObtemDadosConfirmacaoProximoFilaPor(contexto, Convert.ToInt32(dtEncerramento.Rows[0].Ano_encerramento), Convert.ToInt32(dtEncerramento.Rows[0].Sem_encerramento), ObjetoExecucao.Censo, ObjetoExecucao.Curso, ObjetoExecucao.Serie, ObjetoExecucao.Turno);

                            //Verifica se exite aluno na fila
                            if (dadosCandidato.OpcaoInscricaoId > 0)
                            {
                                //Convoca proximo aluno da fila

                                //Atualiza dados da convocacao
                                rnOpcaoInscricao.AtualizaConvocacao(contexto, dadosCandidato.OpcaoInscricaoId, DateTime.Now, ObjetoExecucao.UsuarioResponsavel, out prazoFinal, dadosCandidato.Municipio);

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
                                           ", dadosCandidato.Nome, dadosCandidato.Serie, dadosCandidato.Segmento, dadosCandidato.Escola, DateTime.Now.ToString("dd/MM/yyyy"), prazoFinal.ToString("dd/MM/yyyy"), rnMatricula.RetornaTextoEmailConvocacaoPor(Convert.ToInt32(dtEncerramento.Rows[0].Ano_encerramento)));

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
                                    convocacaoSemEmail.UsuarioResponsavel = ObjetoExecucao.UsuarioResponsavel;
                                    convocacaoSemEmail.DataAviso = DateTime.Now;

                                    rnConvocacaoSemEmail.Insere(contexto, convocacaoSemEmail);
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

        #endregion

        #region Aluno

        public static QueryTable ConsultarCausaEncerramento(string aluno)
        {
            string sql = @"select ce.DESCRICAO as causa_encerramento from LY_ALUNO a INNER JOIN LY_H_CURSOS_CONCL cc ON a.ALUNO = cc.ALUNO
                        INNER JOIN  LY_CAUSA_ENCERR ce ON cc.CAUSA_ENCERR = ce.CAUSA_ENCERR WHERE a.ALUNO = ? and DT_REABERTURA is null ORDER BY cc.DT_ENCERRAMENTO DESC";
            return Consultar(sql, aluno);
        }

        public static QueryTable ConsultarMotivoEncerramento(string aluno)
        {
            string sql = @"select motivo, ms.descricao as MOTIVOSAIDA
                            from LY_H_CURSOS_CONCL cc
                            inner join LY_ALUNO a ON a.ALUNO = cc.ALUNO 
                            inner join LY_MOTIVOSAIDA ms on cc.MOTIVO = ms.MOTIVOSAIDA 
                            WHERE a.ALUNO = ? and DT_REABERTURA is null 
                            ORDER BY cc.DT_ENCERRAMENTO DESC";
            return Consultar(sql, aluno);
        }

        #endregion

        public static DateTime UltimaDataEncerramento(string aluno)
        {
            string sql = @"select top 1 hc.dt_encerramento
                            from LY_ALUNO a 
                            inner join LY_H_CURSOS_CONCL hc on a.ALUNO = hc.ALUNO 
                            inner join LY_MOTIVOSAIDA ms on hc.MOTIVO = ms.MOTIVOSAIDA 
                            left join ly_instituicao i on hc.OUTRA_FACULDADE = i.OUTRA_FACULDADE 
                            left join LY_CAUSA_ENCERR ce on hc.CAUSA_ENCERR = ce.CAUSA_ENCERR 
                            where a.ALUNO = ? ORDER BY (hc.DT_ENCERRAMENTO) DESC";

            QueryTable qt = Consultar(sql, aluno);
            if (qt.Rows.Count > 0)
                return Convert.ToDateTime(qt.Rows[0]["dt_encerramento"]);
            else
                return DateTime.MinValue;
        }

        public static bool ValidaDataEncerramento(string aluno, DateTime dateTime)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            Matgrade rnMatgrade = new Matgrade();
            bool ehMaior = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*)
                                        FROM   LY_ALUNO A (NOLOCK)
                                               INNER JOIN LY_H_CURSOS_CONCL HC (NOLOCK)
                                                       ON A.ALUNO = HC.ALUNO 
                                        WHERE  A.ALUNO = @ALUNO
                                               AND DT_REABERTURA > @DATA  ";

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@DATA", dateTime);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    ehMaior = true;
                }

                return ehMaior;
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

        public static bool PossuiMatGrade(string aluno, int matgrade)
        {
            string sql = @"select 1 from LY_MATGRADE where ALUNO=? AND GRADE_ID = ? AND SIT_MATGRADE = 'Matriculado'";
            int qtd = ExecutarFuncao(sql, aluno, matgrade);
            if (qtd == 1)
            {
                return true;
            }
            return false;
        }

        public static bool PossuiEncerramentoPorDuplicidade(string aluno)
        {
            string sql = @"select 1 FROM dbo.LY_H_CURSOS_CONCL where ALUNO=? AND MOTIVO IN ('DUPLIC_SIS','DUPLICIDADE') AND DT_REABERTURA IS NULL";
            int qtd = ExecutarFuncao(sql, aluno);
            if (qtd == 1)
            {
                return true;
            }
            return false;
        }

        public bool PossuiEncerramentoParaReabertura(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                possui = this.PossuiEncerramentoParaReabertura(ctx, aluno);
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

        public bool PossuiEncerramentoParaReabertura(DataContext ctx, string aluno)
        {
            bool possui = false;

            ContextQuery contextQuery = new ContextQuery
                 {
                     Command = @" SELECT  COUNT(*)
                                FROM    LY_H_CURSOS_CONCL
                                WHERE   ALUNO = @ALUNO                                        
                                        AND DT_REABERTURA IS NULL "
                 };

            contextQuery.Parameters.Add("@ALUNO", aluno);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public Techne.Lyceum.RN.Util.ValidacaoDados ValidaEncerramento(LyHCursosConcl lyHCursosConcl, DadosExecucao ObjetoExecucao)
        {
            RN.Aluno rnAluno = new Aluno();
            System.Collections.Generic.List<string> mensagens = new System.Collections.Generic.List<string>();
            DataContext contexto = null;
            RN.PeriodoLetivo rnPeriodoLetivo = new PeriodoLetivo();
            DadosTransferencia dadosTransferenciaPermuta = new DadosTransferencia();
            DadosEnturmacaoAluno dadosEnturmacaoAlunoPermuta = new DadosEnturmacaoAluno();
            DadosEnturmacaoAluno dadosEnturmacaoAlunoEncerramento = new DadosEnturmacaoAluno();
            RenovacaoMatricula.Renovacao rnRenovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Renovacao();
            ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            RN.SituacaoFinalAluno rnSituacaoFinalAluno = new SituacaoFinalAluno();
            Agenda.Entidades.ParametroBloqueioEncerramento parametroBloqueioEncerramento = new RN.Agenda.Entidades.ParametroBloqueioEncerramento();
            Agenda.ParametroBloqueioEncerramento rnParametroBloqueioEncerramento = new Techne.Lyceum.RN.Agenda.ParametroBloqueioEncerramento();
            RN.Agenda.Evento rnEvento = new Techne.Lyceum.RN.Agenda.Evento();
            List<RN.Agenda.Entidades.Evento> eventos = new List<RN.Agenda.Entidades.Evento>();
            int idEventoBloqueioaEncerramento = Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.BloqueioEncerramentoAluno);
            Matricula rnMatricula = new Matricula();
            Matgrade rnMatgrade = new Matgrade();
            GradeTurma rnGradeTurma = new GradeTurma();
            List<string> listaAvisosPermuta = new List<string>();
            TransferenciaTurma rnTransferenciaTurma = new TransferenciaTurma();
            Techne.Lyceum.RN.Util.ValidacaoDados validacaoDadosPermuta = new ValidacaoDados();
            Techne.Lyceum.RN.Util.ValidacaoDados validacaoDados = new Techne.Lyceum.RN.Util.ValidacaoDados
            {
                Valido = false
            };

            if (lyHCursosConcl == null)
            {
                return validacaoDados;
            }
            if (lyHCursosConcl.AnoEncerramento == -1)
            {
                mensagens.Add("Campo ANO DE ENCERRAMENTO é obrigatório.");
            }
            if (lyHCursosConcl.SemEncerramento == -1)
            {
                mensagens.Add("Campo PERIODO DE ENCERRAMENTO é obrigatório.");
            }
            if (string.IsNullOrEmpty(lyHCursosConcl.Aluno))
            {
                mensagens.Add("Campo ALUNO é obrigatório.");
            }
            if (string.IsNullOrEmpty(lyHCursosConcl.Curso))
            {
                mensagens.Add("Campo CURSO é obrigatório.");
            }
            if (string.IsNullOrEmpty(lyHCursosConcl.Curriculo))
            {
                mensagens.Add("Campo CURRICULO é obrigatório.");
            }
            if (string.IsNullOrEmpty(lyHCursosConcl.Turno))
            {
                mensagens.Add("Campo TURNO é obrigatório.");
            }
            if (string.IsNullOrEmpty(lyHCursosConcl.Motivo))
            {
                mensagens.Add("Campo MOTIVO é obrigatório.");
            }

            if (string.IsNullOrEmpty(lyHCursosConcl.CausaEncerr))
            {
                mensagens.Add("Campo CAUSA é obrigatório.");
            }
            if (string.IsNullOrEmpty(ObjetoExecucao.UsuarioResponsavel))
            {
                mensagens.Add("Campo USUARIO RESPONSAVEL é obrigatório.");
            }
            if (ObjetoExecucao.Censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE ENSINO é obrigatório.");
            }
            if (ObjetoExecucao.Turno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TURNO é obrigatório.");
            }
            if (ObjetoExecucao.Curso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CURSO é obrigatório.");
            }
            if (ObjetoExecucao.Serie <= 0)
            {
                mensagens.Add("Campo SERIE é obrigatório.");
            }

            //Verifica se vai realiza a reserva da vaga
            if (ObjetoExecucao.ReservaVaga)
            {
                //Verifica se será realizada a permuta
                if (ObjetoExecucao.RealizaPermuta)
                {
                    mensagens.Add("Não é possivel colocar aluno na vaga em caso de reserva de vaga.");
                }
            }
            else
            {
                //Verifica se será realizada a permuta
                if (ObjetoExecucao.RealizaPermuta)
                {
                    if (ObjetoExecucao.AlunoPermuta.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Caso seja selecionada a opção de colocar aluno na vaga liberada é obrigatório informar o ALUNO.");
                    }
                }
            }

            if (mensagens.Count == 0)
            {
                //Verifica se a matricula que está sendo encerrada é a correta do aluno existinado outra ativa
                string matricula = rnAluno.ObtemAlunoMaisPessoaAtivaPor(lyHCursosConcl.Pessoa, lyHCursosConcl.Aluno);

                if (!matricula.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Não será possível realizar o encerramento, pois esta matricula consta como correta do aluno, com isso a matricula de número " + matricula + " deve ser encerrada antes.");
                }

                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se o aluno está sendo encerrado com o motivo de conclusão e se o ano do encerramento ainda está vigente
                    if (lyHCursosConcl.Motivo.ToUpper() == "CONCLUSAO" &&
                        rnPeriodoLetivo.EhAnoPeriodoAulaAtivoPor(contexto, Convert.ToInt32(lyHCursosConcl.AnoEncerramento), Convert.ToInt32(lyHCursosConcl.SemEncerramento), DateTime.Now))
                    {
                        mensagens.Add("O aluno não pode ser encerrado com motivo de conclusão em um ANO / PERIODO que ainda esteja vigente.");
                    }

                    //Verifica se existem eventos de bloqueio abertos
                    eventos = rnEvento.ListaEventoAbertoPor(idEventoBloqueioaEncerramento, DateTime.Today);
                    if (eventos.Count != 0)
                    {
                        foreach (RN.Agenda.Entidades.Evento evento in eventos)
                        {
                            //Busca parametros do evento
                            parametroBloqueioEncerramento = rnParametroBloqueioEncerramento.ObtemPor(contexto, evento.AgendaId);

                            //Verifica se o bloqueio é sem validações especificas
                            if (parametroBloqueioEncerramento.BloqueioTotal)
                            {
                                mensagens.Add("Não é possivel encerrar este aluno pois há um ou mais bloqueios de encerramentos vigentes para seus dados.");
                            }
                            else
                            {
                                //Verifica se eh necessario validação de existencia de renovação de matricula
                                if (parametroBloqueioEncerramento.ValidaRenovacao)
                                {
                                    //Verifica se aluno possui renovação (Ativa ou com confrimação) para o ano e periodo
                                    if (rnRenovacao.PossuiRenovacaoAtivaConfirmadaPor(contexto, lyHCursosConcl.Aluno, Convert.ToInt32(lyHCursosConcl.AnoEncerramento), Convert.ToInt32(lyHCursosConcl.SemEncerramento)))
                                    {
                                        mensagens.Add("Não é possivel encerrar este aluno pois ele possui renovação para ano / periodo informado.");
                                    }
                                }

                                //Verifica se eh necessario validação de existencia de confirmação vinda do matricula facil
                                if (parametroBloqueioEncerramento.ValidaMatriculaFacil)
                                {
                                    if (rnConfirmacaoMatricula.PossuiConfirmacaoMatriculaFacilPendenteConfirmadaPor(contexto, lyHCursosConcl.Aluno, Convert.ToInt32(lyHCursosConcl.AnoEncerramento), Convert.ToInt32(lyHCursosConcl.SemEncerramento)))
                                    {
                                        mensagens.Add("Não é possivel encerrar este aluno pois ele possui Confirmação do Matricula Fácil para ano / periodo informado.");
                                    }

                                }
                            }
                        }
                    }

                    var contextQuery = new ContextQuery(
                        @"SELECT  1
                    FROM    dbo.[LY_H_CURSOS_CONCL]
                    WHERE   ALUNO = @ALUNO
                            AND DT_REABERTURA IS NULL");

                    contextQuery.Parameters.Add("@ALUNO", lyHCursosConcl.Aluno);

                    var obj = contexto.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        mensagens.Add("Já existe um encerramento cadastrado para este aluno.");
                    }

                    if (ObjetoExecucao.Registra_EncMatGrade)
                    {
                        if (rnMatgrade.RetornaMatGradePrincipalAtivaPor(contexto, lyHCursosConcl.Aluno) > 1)
                        {
                            mensagens.Add("Aluno possui mais de uma grade, não pode ser encerrado.");
                        }
                    }

                    //Busca dados da matricula atual do aluno encerrado
                    dadosEnturmacaoAlunoEncerramento = rnMatricula.ObtemMatriculaPrincipalAtivaPor(contexto, lyHCursosConcl.Aluno);
                    //Verifica se o aluno do encerramento esta enturmado
                    if (!dadosEnturmacaoAlunoEncerramento.Aluno.IsNullOrEmptyOrWhiteSpace())
                    {
                        ObjetoExecucao.PossuiEnturmacao = true;
                    }
                    else
                    {
                        ObjetoExecucao.PossuiEnturmacao = false;
                    }
                    //Chamado 47993 - Solicitação de retirada da regra
                    //Verifica se o aluno não foi enturmado e reprovado em 2023 0 e 1 na 2ª e 3ª série, EJA III e EJA IV
                    //if (dadosEnturmacaoAlunoEncerramento.Ano != 2024 //Não enturmado em 2024
                    //    && rnRenovacao.PossuiRenovacaoAtivaConfirmadaPor(contexto, lyHCursosConcl.Aluno, 2024, 0) //Verifica se possui renovação para 2024 ou seja de serie concluinte
                    //    && (rnSituacaoFinalAluno.EhReprovadoPor(contexto, lyHCursosConcl.Aluno, 2023, 0, "0002.31", 3) //verifica se é reprovado na 3ª série do ensino medio
                    //            || rnSituacaoFinalAluno.EhReprovadoItinerarioFormativoTrihaPor(contexto, lyHCursosConcl.Aluno, 2023, 0, 2, "RE1", 3) //Verifica se é reprovado em cursos do itinerario regular serie 2
                    //            || rnSituacaoFinalAluno.EhReprovadoItinerarioFormativoTrihaPor(contexto, lyHCursosConcl.Aluno, 2023, 0, 3, "ED2", 3) //Verifica se é reprovado em cursos do itinerario eja serie 3
                    //            || rnSituacaoFinalAluno.EhReprovadoItinerarioFormativoTrihaPor(contexto, lyHCursosConcl.Aluno, 2023, 0, 4, "ED2", 3)) //Verifica se é reprovado em cursos do itinerario eja serie 4
                    //        )
                    //{
                    //    mensagens.Add("Aluno(s) com renovação retido(s) em 2023 na 2ª / 3ª Série do Regular ou EJA III / IV ficarão em pendência de enturmação para futura escolha do curso/série de 2024, realize a enturmação antes do encerramento.");
                    //}

                    //Verifica se será realizada a permuta
                    if (ObjetoExecucao.RealizaPermuta)
                    {
                        //Busca dados da matricula atual do aluno da permuta
                        dadosEnturmacaoAlunoPermuta = rnMatricula.ObtemMatriculaPrincipalAtivaPor(contexto, ObjetoExecucao.AlunoPermuta);

                        //Verifica se o aluno do encerramento esta enturmado
                        if (!ObjetoExecucao.PossuiEnturmacao)
                        {
                            mensagens.Add("Apenas podem ser colocados alunos na vaga liberada de um aluno que esteja enturmado.");
                        }

                        //Verifica se o aluno da permuta esta enturmado
                        if (dadosEnturmacaoAlunoPermuta.Aluno.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add("Apenas alunos ENTURMADOS podem ser colocados na vaga liberada.");
                        }

                        //Verifica se o aluno eh da mesma escola
                        if (dadosEnturmacaoAlunoPermuta.Censo != ObjetoExecucao.Censo)
                        {
                            mensagens.Add("Apenas alunos da mesma UNIDADE DE ENSINO podem ser colocados na vaga liberada.");
                        }

                        //Verifica se eh no mesmo ano                        
                        if (dadosEnturmacaoAlunoPermuta.Ano != Convert.ToInt32(lyHCursosConcl.AnoEncerramento))
                        {
                            mensagens.Add("O aluno deve estar ENTURMADO no mesmo ANO do Encerramento.");
                        }

                        if (mensagens.Count == 0)
                        {
                            //Monta DadosTransferencia
                            dadosTransferenciaPermuta.Aluno = ObjetoExecucao.AlunoPermuta;
                            dadosTransferenciaPermuta.Ano = Convert.ToString(lyHCursosConcl.AnoEncerramento);
                            dadosTransferenciaPermuta.SituacaoAluno = "Ativo";
                            dadosTransferenciaPermuta.UnidadeEnsino = ObjetoExecucao.Censo;
                            dadosTransferenciaPermuta.UnidadeFisica = ObjetoExecucao.Censo;
                            dadosTransferenciaPermuta.MotivoTransferencia = ObjetoExecucao.MotivoPermuta;
                            dadosTransferenciaPermuta.EnsinoReligioso = Convert.ToString(ObjetoExecucao.EnsinoReligioso);
                            dadosTransferenciaPermuta.LinguaEstrangeira = Convert.ToString(ObjetoExecucao.LinguaEstrangeira);
                            dadosTransferenciaPermuta.UsuarioResponsavel = ObjetoExecucao.UsuarioResponsavel;
                            dadosTransferenciaPermuta.NecessidadeEspecial = ObjetoExecucao.NecessidadeEspecialAlunoPermuta;
                            dadosTransferenciaPermuta.DataNascimento = ObjetoExecucao.DataNascimentoAlunoPermuta;

                            //Busca turno da turma concomitante do aluno da permuta
                            dadosTransferenciaPermuta.TurnoAtualTurmaConcomitante = rnMatricula.ObtemTurnoMatriculaConcomitantePor(contexto, Convert.ToDecimal(dadosTransferenciaPermuta.Ano), Convert.ToDecimal(dadosEnturmacaoAlunoPermuta.Periodo), ObjetoExecucao.AlunoPermuta);
                            if (dadosTransferenciaPermuta.TurnoAtualTurmaConcomitante.IsNullOrEmptyOrWhiteSpace())
                            {
                                dadosTransferenciaPermuta.PossuiTurmaConcomitante = false;
                            }
                            else
                            {
                                dadosTransferenciaPermuta.PossuiTurmaConcomitante = true;
                            }

                            //Dados da origem:
                            dadosTransferenciaPermuta.SemestreAtual = Convert.ToString(dadosEnturmacaoAlunoPermuta.Periodo);
                            dadosTransferenciaPermuta.TurmaAtual = dadosEnturmacaoAlunoPermuta.Turma;
                            dadosTransferenciaPermuta.NumChamadaAtual = null;
                            dadosTransferenciaPermuta.GradeIdAtual = dadosEnturmacaoAlunoPermuta.GradeId;
                            dadosTransferenciaPermuta.TurnoAtual = dadosEnturmacaoAlunoPermuta.Turno;
                            dadosTransferenciaPermuta.CursoAtual = dadosEnturmacaoAlunoPermuta.Curso;
                            dadosTransferenciaPermuta.SerieAtual = Convert.ToString(dadosEnturmacaoAlunoPermuta.Serie);

                            //Dados da matricula destino:
                            dadosTransferenciaPermuta.SemestreDestino = Convert.ToString(dadosEnturmacaoAlunoEncerramento.Periodo);
                            dadosTransferenciaPermuta.TurnoDestino = dadosEnturmacaoAlunoEncerramento.Turno;
                            dadosTransferenciaPermuta.CursoDestino = dadosEnturmacaoAlunoEncerramento.Curso;
                            dadosTransferenciaPermuta.TipoCursoDestino = dadosEnturmacaoAlunoEncerramento.TipoCurso;
                            dadosTransferenciaPermuta.CurriculoDestino = dadosEnturmacaoAlunoEncerramento.Curriculo;
                            dadosTransferenciaPermuta.SerieDestino = Convert.ToString(dadosEnturmacaoAlunoEncerramento.Serie);
                            dadosTransferenciaPermuta.TurmaDestino = dadosEnturmacaoAlunoEncerramento.Turma;
                            dadosTransferenciaPermuta.GradeIdDestino = dadosEnturmacaoAlunoEncerramento.GradeId;
                            dadosTransferenciaPermuta.TipoEnsProfissionalizanteDestino = dadosEnturmacaoAlunoEncerramento.TipoEnsinoProfissionalizante;
                            dadosTransferenciaPermuta.ListaDisciplinasTurmaDestino = rnGradeTurma.ListaDisciplinaMatriculaRegular(dadosEnturmacaoAlunoEncerramento.Ano, dadosEnturmacaoAlunoEncerramento.Periodo, dadosEnturmacaoAlunoEncerramento.Turma, dadosEnturmacaoAlunoEncerramento.GradeId);

                            //Verificar todas as validaçoes de transferencia
                            validacaoDadosPermuta = rnTransferenciaTurma.ValidaTransferenciaTurmaPrincipal(dadosTransferenciaPermuta, out listaAvisosPermuta, true);

                            if (validacaoDadosPermuta.Valido)
                            {
                                //Verifica se possui avisos
                                if (listaAvisosPermuta.Count > 0)
                                {
                                    string avisos = listaAvisosPermuta.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
                                    mensagens.Add(string.Format("O Aluno escolhido não pode utilizar a vaga liberada no encerramento pelo(s) seguinte(s) motivo(s): {0}", avisos));
                                }
                                else
                                {
                                    //Atualiza com dados encontados
                                    ObjetoExecucao.DadosTransferenciaPermuta = dadosTransferenciaPermuta;
                                }
                            }
                            else
                            {
                                mensagens.Add(string.Format("O Aluno escolhido não pode utilizar a vaga liberada no encerramento pelo(s) seguinte(s) motivo(s): {0}", validacaoDadosPermuta.Mensagem));
                            }
                        }
                    }
                }
                catch
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw;
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

    }//fim classe
}//fim namespace
