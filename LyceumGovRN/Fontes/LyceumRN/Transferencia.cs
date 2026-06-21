using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using Seeduc.Infra.Data;
using Techne.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.DTOs;

namespace Techne.Lyceum.RN
{
    public class Transferencia : RNBase
    {
        public const string Aceita = "Aceita";
        public const string Recusada = "Recusada";
        public const string Pendente = "Pendente";
        public const string Todas = "Todas";

        public static QueryTable ConsultarAluno(string aluno)
        {
            string sql = "SELECT c.curso, t.turno, ue.NOME_COMP nomeuensino,a.unidade_fisica, a.unidade_ensino, uf.NOME_COMP nomeufisica, a.sit_aluno, c.NOME nomecurso, t.DESCRICAO nometurno, " +
                        "a.curriculo, a.serie, se.descricao descricaoSerie, a.ano_ingresso, a.sem_ingresso, (select top 1 gs.GRADE from ly_matgrade mg " +
                        "join ly_grade_turma gt " +
                        "on mg.grade_id = gt.grade_id " +
                        "join ly_grade_serie gs on gs.GRADE_ID = mg.GRADE_ID and gs.GRADE = gt.TURMA " +
                        "join ly_matricula m on m.ALUNO = mg.ALUNO and gt.disciplina = m.disciplina and gt.turma = m.turma and gt.ano = m.ano and gt.semestre = m.semestre " +
                        "join ly_disciplina d on m.disciplina = d.disciplina " +
                        "where mg.sit_matgrade = 'Matriculado' " +
                        "and mg.aluno = a.ALUNO) as turma,  " +
                        "(select distinct convert(varchar,m.ANO) + convert(varchar,m.SEMESTRE) from ly_matgrade mg " +
                        "join ly_grade_turma gt " +
                        "on mg.grade_id = gt.grade_id " +
                        "join ly_grade_serie gs on gs.GRADE_ID = mg.GRADE_ID and gs.GRADE = gt.TURMA " +
                        "join ly_matricula m on m.ALUNO = mg.ALUNO and gt.disciplina = m.disciplina and gt.turma = m.turma and gt.ano = m.ano and gt.semestre = m.semestre " +
                        "join ly_disciplina d on m.disciplina = d.disciplina " +
                        "where mg.sit_matgrade = 'Matriculado' " +
                        "and mg.aluno = a.ALUNO) anoperiodo  " +
                        "from LY_ALUNO a left join LY_UNIDADE_ENSINO ue on a.UNIDADE_ENSINO = ue.UNIDADE_ENS  " +
                        "left join LY_UNIDADE_FISICA uf on a.UNIDADE_FISICA = uf.UNIDADE_FIS  " +
                        "left join LY_CURSO c on c.CURSO = a.CURSO left join LY_TURNO t on t.TURNO = a.TURNO " +
                        "left join LY_SERIE se on se.CURSO = a.CURSO and se.TURNO = a.TURNO and se.CURRICULO = a.CURRICULO and se.SERIE = a.SERIE " +
                        "Where a.aluno = ?";

            return Consultar(sql, aluno);
        }

        public bool ExisteSolicitacaoPendentePor(string censo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" select COUNT(1)
                    FROM    dbo.TCE_TRANSFERENCIA t (NOLOCK)
                            INNER JOIN dbo.TCE_TRANSFERENCIA_DESTINO d (NOLOCK) ON t.ID_TRANSFERENCIA = d.ID_TRANSFERENCIA
                            INNER JOIN dbo.TCE_TRANSFERENCIA_ORIGEM o (NOLOCK) ON t.ID_TRANSFERENCIA = o.ID_TRANSFERENCIA
                    WHERE  o.CENSO = @CENSO
						AND STATUS = 'Pendente' ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

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

        public static DataTable ListarTransferenciasDeDestino(string censo, string status)
        {
            if (string.IsNullOrEmpty(censo))
            {
                return null;
            }

            var ativo = status != Todas;

            var contextQuery = new ContextQuery(
                string.Format(
                    @"SELECT t.ID_TRANSFERENCIA,
                           a.ALUNO,
                           Pa.NOME_COMPL AS 'NOME_ALUNO',
                           o.CENSO,
                           ue.NOME_COMP AS 'NOME_ESCOLA',
                           t.DT_CADASTRO,
                           t.MATRICULA_SOLICITANTE,
                           t.STATUS,
                           U.USUARIO + ' / ' + U.NOME AS 'SOLICITANTE',
                           CASE OBSERVACAO
                             WHEN 'Outros' THEN t.OBSERVACAO + ' - ' + t.JUSTIFICATIVA
                             ELSE t.OBSERVACAO
                           END OBSERVACAOJUST,
                           t.DT_ALTERACAO,
                           t.MOTIVO
                    FROM   dbo.TCE_TRANSFERENCIA t (NOLOCK)
                           INNER JOIN dbo.TCE_TRANSFERENCIA_DESTINO d (NOLOCK) ON t.ID_TRANSFERENCIA = d.ID_TRANSFERENCIA
                           INNER JOIN dbo.TCE_TRANSFERENCIA_ORIGEM o (NOLOCK) ON t.ID_TRANSFERENCIA = o.ID_TRANSFERENCIA
                           INNER JOIN LY_ALUNO a (NOLOCK) ON a.ALUNO = t.ALUNO
                           INNER JOIN LY_PESSOA PA (NOLOCK) ON PA.PESSOA = A.PESSOA
                           INNER JOIN dbo.LY_UNIDADE_ENSINO ue (NOLOCK) ON ue.UNIDADE_ENS = o.CENSO
                           LEFT JOIN HADES.DBO.HD_USUARIO U (NOLOCK) ON U.USUARIO = T.MATRICULA_SOLICITANTE
                    WHERE  d.CENSO = @CENSO{0}
                    ORDER BY t.DT_CADASTRO ASC",
                    ativo ? " AND t.[STATUS] = @STATUS" : string.Empty));

            contextQuery.Parameters.Add("@CENSO", censo);

            if (ativo)
            {
                contextQuery.Parameters.Add("@STATUS", status);
            }

            return Consultar(contextQuery);
        }

        public bool ExistePor(DataContext contexto, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @"  SELECT COUNT(*) 
                                FROM dbo.TCE_TRANSFERENCIA (NOLOCK)
                                WHERE ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", aluno);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public static DataTable ListarTransferenciasDeOrigem(string censo, string status)
        {
            if (string.IsNullOrEmpty(censo))
            {
                return null;
            }

            var ativo = status != Todas;

            var contextQuery = new ContextQuery(
                string.Format(
                    @"SELECT  t.ID_TRANSFERENCIA,
                            a.ALUNO,
                            Pa.NOME_COMPL AS 'NOME_ALUNO',
                            d.CENSO,
                            ue.NOME_COMP AS 'NOME_ESCOLA',
                            t.DT_CADASTRO,
                            t.MATRICULA_SOLICITANTE,
                            t.STATUS,
                            t.OBSERVACAO,
                            t.JUSTIFICATIVA,
                            U.USUARIO + ' / ' + U.NOME AS 'SOLICITANTE',
                            CASE OBSERVACAO
                              WHEN 'Outros' THEN t.OBSERVACAO + ' - ' + t.JUSTIFICATIVA
                              ELSE t.OBSERVACAO
                            END OBSERVACAOJUST,
                            t.DT_ALTERACAO,
                            t.MOTIVO
                    FROM    dbo.TCE_TRANSFERENCIA t (NOLOCK)
                            INNER JOIN dbo.TCE_TRANSFERENCIA_DESTINO d (NOLOCK) ON t.ID_TRANSFERENCIA = d.ID_TRANSFERENCIA
                            INNER JOIN dbo.TCE_TRANSFERENCIA_ORIGEM o (NOLOCK) ON t.ID_TRANSFERENCIA = o.ID_TRANSFERENCIA
                            INNER JOIN LY_ALUNO a (NOLOCK) ON a.ALUNO = t.ALUNO
                            INNER JOIN LY_PESSOA pa (NOLOCK) ON pa.pessoa = a.pessoa
                            INNER JOIN dbo.LY_UNIDADE_ENSINO ue (NOLOCK) ON ue.UNIDADE_ENS = d.CENSO
                            LEFT JOIN HADES.DBO.HD_USUARIO U (NOLOCK) ON U.USUARIO = T.MATRICULA_SOLICITANTE
                    WHERE   o.CENSO = @CENSO{0}
                    ORDER BY t.DT_CADASTRO ASC",
                    ativo ? " AND t.[STATUS] = @STATUS" : string.Empty));

            contextQuery.Parameters.Add("@CENSO", censo);

            if (ativo)
            {
                contextQuery.Parameters.Add("@STATUS", status);
            }

            return Consultar(contextQuery);
        }

        public static ICollection<ListItem> ListarStatusDeTransferencias()
        {
            return new List<ListItem>
                   {
                        new ListItem(Pendente, Pendente),
                        new ListItem(Todas, Todas),
                        new ListItem(Aceita, Aceita),
                        new ListItem(Recusada, Recusada)
                   };
        }

        private void CarregarOrigemDestino(TceTransferencia transferencia, out TceTransferenciaDestino transferenciaDestino, out TceTransferenciaOrigem transferenciaOrigem)
        {
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                CarregarOrigemDestino(contexto, transferencia, out transferenciaDestino, out transferenciaOrigem);
            }
            catch
            {
                if (contexto != null)
                    contexto.Abandon();

                throw;
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }
        }

        private void CarregarOrigemDestino(DataContext contexto, TceTransferencia transferencia, out TceTransferenciaDestino transferenciaDestino, out TceTransferenciaOrigem transferenciaOrigem)
        {
            transferenciaDestino = contexto.TryToBindEntity<TceTransferenciaDestino>(
                new ContextQuery(
                    @"SELECT *
                        FROM dbo.TCE_TRANSFERENCIA_DESTINO
                       WHERE ID_TRANSFERENCIA = @ID_TRANSFERENCIA",
                    new ContextQueryParameter("@ID_TRANSFERENCIA", transferencia.IdTransferencia)));

            transferenciaOrigem = contexto.TryToBindEntity<TceTransferenciaOrigem>(
                new ContextQuery(
                    @"SELECT *
                        FROM dbo.TCE_TRANSFERENCIA_ORIGEM
                       WHERE ID_TRANSFERENCIA = @ID_TRANSFERENCIA",
                    new ContextQueryParameter("@ID_TRANSFERENCIA", transferencia.IdTransferencia)));
        }

        #region Processamento/Execução de Transferências

        public void ProcessarTransferencias(ICollection<TceTransferencia> transferencias)
        {
            List<ContextQuery> contextQueries = new List<ContextQuery>();
            contextQueries = ProcessaTransferenciaLote(transferencias);
            ExecutaTransferencia(contextQueries);
        }

        private List<ContextQuery> ProcessaTransferenciaLote(ICollection<TceTransferencia> transferencias)
        {
            DataContext contexto = null;
            List<ContextQuery> contextQueries = new List<ContextQuery>();

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                foreach (var transferencia in transferencias)
                {
                    if (transferencia == null)
                    {
                        continue;
                    }

                    switch (transferencia.Status)
                    {
                        case Recusada:
                            Recusar(transferencia, contextQueries);
                            break;

                        case Aceita:
                            Aceitar(transferencia, contexto, contextQueries);
                            break;
                    }
                }
            }
            catch (Exception)
            {
                if (contexto != null)
                    contexto.Abandon();

                throw;
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }

            return contextQueries;
        }

        private void ExecutaTransferencia(List<ContextQuery> contextQueries)
        {
            DataContext contexto = null;

            try
            {
                //Abrir transação e efetuar todas os INSERT's e UPDATE's
                contexto = DataContextBuilder.FromLyceum.UsingLock();

                foreach (ContextQuery contextQuery in contextQueries)
                {
                    contexto.ApplyModifications(contextQuery);
                }
            }
            catch (Exception)
            {
                if (contexto != null)
                    contexto.Abandon();

                throw;
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }
        }

        private void Recusar(RN.Entidades.TceTransferencia transferencia, List<ContextQuery> listaContextQueries)
        {
            Atualiza(transferencia, Recusada, listaContextQueries);
        }

        private void Aceitar(RN.Entidades.TceTransferencia transferencia, DataContext contexto, List<ContextQuery> listaContextQueries)
        {
            TceTransferenciaDestino transferenciaDestino;
            TceTransferenciaOrigem transferenciaOrigem;

            RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new RN.ConfirmacaoMatricula();
            RN.Aluno rnAluno = new RN.Aluno();
            RN.HCurrAluno rnHCurrAluno = new RN.HCurrAluno();
            RN.Matricula rnMatricula = new RN.Matricula();
            RN.Matgrade rnMatGrade = new RN.Matgrade();
            RN.Disciplina rnDisciplina = new RN.Disciplina();
            RN.Nota rnNota = new RN.Nota();
            RN.Falta rnFalta = new RN.Falta();
            RN.FlPessoa rnFlPessoa = new RN.FlPessoa();
            RN.DeclaracaoSemNota rnDeclaracaoSemNota = new RN.DeclaracaoSemNota();
            Matriculas.DiasNaoLetivos rnDiasNaoLetivos = new Techne.Lyceum.RN.Matriculas.DiasNaoLetivos();
            Matriculas.VagaReservada rnVagaReservada = new Techne.Lyceum.RN.Matriculas.VagaReservada();
            DadosEnturmacaoAluno dadosEnturmacaoAluno = new DadosEnturmacaoAluno();
            RN.Entidades.LyHCurrAluno hCurrAluno;
            RN.Entidades.LyAluno lyAluno;
            RN.Entidades.TceConfirmacaoMatricula confirmacaoMatricula;
            List<RN.DTOs.DisciplinaTurmaTransferenciaAluno> listaDisciplinaTurmaTransferenciaAluno;
            RN.Entidades.LyMatricula lyMatricula;
            List<RN.DTOs.NotaTurmaAtualTransferenciaAluno> listaNotaTurmaAtualTransferenciaAluno = new List<RN.DTOs.NotaTurmaAtualTransferenciaAluno>();
            List<RN.DTOs.FrequenciaAlunoTurma> listaFrequenciaAlunoTurma = new List<RN.DTOs.FrequenciaAlunoTurma>();
            Matriculas.Entidades.VagaReservada vagaReservada = new Techne.Lyceum.RN.Matriculas.Entidades.VagaReservada();
            int controleVagaId = 0;
            string municipio = string.Empty;
            RN.Entidades.LyNota lyNota;
            RN.Entidades.LyFalta lyFalta;

            int idConfirmacaoMatricula = int.MinValue;
            string gradeId = string.Empty;
            string possiveisPeriodos = string.Empty;
            bool alunoAtivo;
            bool utilizaTransporte = false;

            //Carrega dados das Transferências - Origem e Destino
            CarregarOrigemDestino(contexto, transferencia, out transferenciaDestino, out transferenciaOrigem);

            gradeId = GradeSerie.ObterGradeId(
                contexto,
                transferenciaDestino.Ano,
                transferenciaDestino.Periodo,
                transferenciaDestino.Curso,
                transferenciaDestino.Curriculo,
                transferenciaDestino.Serie,
                transferenciaDestino.Turma);

            CarregaEntidadeLyHCurrAluno(transferencia, transferenciaOrigem, out hCurrAluno);
            CarregaEntidadeLyAluno(transferencia, transferenciaDestino, out lyAluno);

            //Verifica se existe uma confirmacao de matricula exatamente igual
            idConfirmacaoMatricula = rnConfirmacaoMatricula.ObtemIdConfirmacaoMatriculaPendenteOuAtivaPor(
                contexto,
                transferencia.Aluno,
                transferenciaDestino.Ano,
                transferenciaDestino.Periodo,
                transferenciaDestino.Censo,
                transferenciaDestino.Curso,
                transferenciaDestino.Serie,
                transferenciaDestino.Turno);

            //Atualiza ou insere confirmacao de matricula sempre com o tipoVaga = VN
            CarregaEntidadeConfirmacaoMatricula(transferencia,
                transferenciaDestino,
                idConfirmacaoMatricula,
                out confirmacaoMatricula,
                idConfirmacaoMatricula != int.MinValue);

            // Análise Matrícula para Inserção/Atualização das Disciplinas
            listaDisciplinaTurmaTransferenciaAluno = rnDisciplina.ObtemDisciplinaTransferenciaAlunoPor(
                contexto,
                transferenciaDestino.Turma,
                transferenciaDestino.Ano,
                transferenciaDestino.Periodo,
                transferencia.Aluno);

            // Busca a situação atual do aluno
            alunoAtivo = rnAluno.EhAlunoAtivoPor(transferencia.Aluno);

            //Verifica se o aluno esta ativo e possui ano e periodo de origem
            if (alunoAtivo
                && transferenciaOrigem.Ano != null && transferenciaOrigem.Ano > 0
                && transferenciaOrigem.Periodo != null && transferenciaOrigem.Periodo > 0)
            {
                //Busca dados da matricula atual do aluno
                dadosEnturmacaoAluno = rnMatricula.ObtemMatriculaPrincipalAtivaPor(contexto, transferencia.Aluno);

                //Verifica se o aluno tem dados da enturmacao
                if (dadosEnturmacaoAluno.IdControleVaga > 0)
                {
                    controleVagaId = dadosEnturmacaoAluno.IdControleVaga;
                    municipio = dadosEnturmacaoAluno.MunicipioEscola;
                }
                else
                {
                    //Caso não tenha busca dados da confirmação
                    vagaReservada.ControleVagaId = rnConfirmacaoMatricula.ObtemControleVagaIdConfirmadoPor(contexto, transferencia.Aluno, Convert.ToInt32(transferenciaOrigem.Ano), Convert.ToInt32(transferenciaOrigem.Periodo), out municipio);
                }

                //Monta Vaga reservada
                vagaReservada.Aluno = transferencia.Aluno;
                vagaReservada.DataInicio = DateTime.Now;
                vagaReservada.UsuarioId = transferencia.MatriculaAndamento;
                vagaReservada.ControleVagaId = controleVagaId;
                vagaReservada.DataFim = rnDiasNaoLetivos.RetornaDataFinalPor(contexto, DateTime.Now, 1, municipio);
            }

            // Se o aluno nao estiver ativo nao precisa buscar notas e faltas.
            if (alunoAtivo)
            {
                // Busca notas do aluno na turma atual
                listaNotaTurmaAtualTransferenciaAluno = rnNota.ObtemNotaTurmaAtualTransferenciaAluno(
                    contexto,
                    transferencia.Aluno,
                    transferenciaOrigem,
                    transferenciaDestino);

                // Busca faltas do aluno na turma atual menor ou igual a quantidade de aulas dadas da turma nova
                listaFrequenciaAlunoTurma = rnFalta.ObtemFrequenciaAlunoTurmaAtualTransferencia(
                    contexto,
                    transferencia.Aluno,
                    transferenciaOrigem,
                    transferenciaDestino);
            }
            else
            {
                // Retorna se aluno possui gratuidade no transporte
                utilizaTransporte = Aluno.ExisteUtilizaTransporte(contexto, transferencia.Aluno);
            }

            // --------------------------------------------------------------------------------------
            // INÍCIO DOS INSERTS E UPDATES
            // --------------------------------------------------------------------------------------

            //Verifica se existe vaga reservada
            if (vagaReservada.ControleVagaId > 0)
            {
                //Colocar a vaga com dados de origem da tabela de reserva 24 horas
                rnVagaReservada.Insere(vagaReservada, listaContextQueries);
            }

            // Insere o histórico de transferência do curso do aluno
            rnHCurrAluno.Insere(hCurrAluno, listaContextQueries);

            // Altera os dados acadêmicos do aluno
            rnAluno.AtualizaDadosTransferencia(lyAluno, listaContextQueries);

            if (idConfirmacaoMatricula != int.MinValue)
            {
                //Atualiza confirmacao de matricula sempre com o tipoVaga = VN
                rnConfirmacaoMatricula.AtualizaDadosTransferencia(confirmacaoMatricula, listaContextQueries);

                possiveisPeriodos = Utils.RecuperaPossiveisPeriodos(transferenciaDestino.Periodo);

                //Cancelar outras confirmaçoes possiveis para o aluno / ano
                rnConfirmacaoMatricula.CancelaOutrasConfirmacoesPossiveisPor(
                    transferencia.Aluno,
                    transferenciaDestino.Ano,
                    ConfirmacaoMatricula.Observacao.CanceladaPorTransferenciaDeAluno,
                    transferencia.MatriculaAndamento,
                    possiveisPeriodos,
                    idConfirmacaoMatricula,
                    listaContextQueries);
            }
            else
            {
                //Alterada para  "Não Confirmado" o status das confirmações existentes
                rnConfirmacaoMatricula.CancelaPossiveisConfirmacaoMatriculaPor(
                    transferencia.Aluno,
                    Convert.ToInt32(transferenciaDestino.Ano),
                    Convert.ToInt32(transferenciaDestino.Periodo),
                    transferencia.MatriculaAndamento,
                    ConfirmacaoMatricula.Observacao.CanceladaPorTransferenciaDeAluno,
                    listaContextQueries
                );

                // Insere confirmacao de matricula sempre com o tipoVaga = VN
                rnConfirmacaoMatricula.Inserir(confirmacaoMatricula, listaContextQueries);
            }

            // Atualiza a situação das matrículas do aluno para Cancelado
            rnMatricula.AlteraSituacaoMatriculaParaCanceladoPor(transferencia.Aluno, listaContextQueries);

            // Registra a transferência na matgrade
            rnMatGrade.AtualizaSituacaoParaCanceladoPor(transferencia.Aluno, listaContextQueries);

            // Insere a matgrade nova
            rnMatGrade.Insere(transferencia.Aluno, gradeId, listaContextQueries);

            // Atualiza ou Insere os Dados da Matrícula (Disciplinas) da Nova Turma do Aluno
            foreach (RN.DTOs.DisciplinaTurmaTransferenciaAluno disciplinaTurmaTransferenciaAluno in listaDisciplinaTurmaTransferenciaAluno)
            {
                //Verifica se a disciplina não é uma eletiva com enturmação separada
                if (!rnDisciplina.EhDisciplinaGradeEletivaPor(transferenciaDestino.Ano, transferenciaDestino.Periodo, transferenciaDestino.Turma, disciplinaTurmaTransferenciaAluno.Disciplina))
                {
                    CarregaEntidadeLyMatricula(transferencia, transferenciaDestino, disciplinaTurmaTransferenciaAluno.Disciplina, out lyMatricula);
                    rnMatricula.InsereOuAtualiza(lyMatricula, listaContextQueries);
                }
            }

            if (alunoAtivo)
            {
                foreach (RN.DTOs.NotaTurmaAtualTransferenciaAluno notaTurmaAtualTransferenciaAluno in listaNotaTurmaAtualTransferenciaAluno)
                {
                    //Verifica se a disciplina não é uma eletiva com enturmação separada
                    if (!rnDisciplina.EhDisciplinaGradeEletivaPor(transferenciaDestino.Ano, transferenciaDestino.Periodo, transferenciaDestino.Turma, notaTurmaAtualTransferenciaAluno.Disciplina))
                    {
                        // Insere ou atualiza a nota caso já exista
                        CarregaEntidadeLyNota(transferencia, transferenciaDestino, notaTurmaAtualTransferenciaAluno, out lyNota);

                        if (notaTurmaAtualTransferenciaAluno.AtualizaNota)
                        {
                            rnNota.Atualizar(lyNota, listaContextQueries);
                        }
                        else
                        {
                            rnNota.Inserir(lyNota, listaContextQueries);
                        }

                        rnDeclaracaoSemNota.TransfereDeclaracaoSemNotaParaNovoNotaId(
                            transferencia.Aluno,
                            notaTurmaAtualTransferenciaAluno.Disciplina,
                            notaTurmaAtualTransferenciaAluno.Prova,
                            transferenciaOrigem.Turma,
                            transferenciaDestino.Turma,
                            transferenciaOrigem.Ano,
                            transferenciaOrigem.Periodo,
                            listaContextQueries);
                    }

                    rnNota.RemovePor(
                        transferencia.Aluno,
                        transferenciaOrigem.Turma,
                        notaTurmaAtualTransferenciaAluno.Disciplina,
                        transferenciaOrigem.Ano,
                        transferenciaOrigem.Periodo,
                        notaTurmaAtualTransferenciaAluno.Prova,
                        listaContextQueries
                    );
                }

                // Insere ou atualiza a falta da turma atual na disciplina da nova turma
                foreach (RN.DTOs.FrequenciaAlunoTurma frequenciaAlunoTurma in listaFrequenciaAlunoTurma)
                {
                    //Verifica se a disciplina não é uma eletiva com enturmação separada
                    if (!rnDisciplina.EhDisciplinaGradeEletivaPor(transferenciaDestino.Ano, transferenciaDestino.Periodo, transferenciaDestino.Turma, frequenciaAlunoTurma.Disciplina))
                    {
                        // Insere ou atualiza a nota caso já exista
                        CarregaEntidadeLyFalta(transferencia, transferenciaDestino, frequenciaAlunoTurma, out lyFalta);

                        if (frequenciaAlunoTurma.AtualizaFalta)
                        {
                            rnFalta.AtualizaQuantidadeDeFaltas(lyFalta, listaContextQueries);
                        }
                        else
                        {
                            rnFalta.Insere(lyFalta, listaContextQueries);
                        }
                    }
                    rnFalta.RemovePor(transferencia.Aluno,
                        frequenciaAlunoTurma.Disciplina,
                        transferenciaOrigem.Turma,
                        transferenciaOrigem.Ano,
                        transferenciaOrigem.Periodo,
                        frequenciaAlunoTurma.Freq,
                        listaContextQueries
                    );
                }
            }
            else
            {
                // Se o aluno não estiver ativo, atualizar a Data de Reabertura
                rnHCurrAluno.AtualizaDataReaberturaPor(transferencia.Aluno, listaContextQueries);

                //Se aluno não estiver ativo verifica se possui marcação no campo gratuidade
                if (utilizaTransporte)
                {
                    //Caso exista marcação para aluno que não estiver ativo, são retirados os campos utiliza transporte publico e modais
                    rnFlPessoa.RetiraDadosTransportePublicoPor(transferencia.Aluno, listaContextQueries);
                }
            }

            Atualiza(transferencia, Aceita, listaContextQueries);
        }

        #endregion

        private void Atualiza(TceTransferencia transferencia, string situacao, List<ContextQuery> listaContextQuery)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"UPDATE dbo.TCE_TRANSFERENCIA
                                        SET DT_ALTERACAO = GETDATE()
                                          , MATRICULA_ANDAMENTO = @MATRICULA
                                          , STATUS = @STATUS";

            if (situacao.Equals(Recusada))
            {
                contextQuery.Command += @", OBSERVACAO = @OBSERVACAO
                                          , JUSTIFICATIVA = @JUSTIFICATIVA";

                contextQuery.Parameters.Add("@OBSERVACAO", transferencia.Observacao);
                contextQuery.Parameters.Add("@JUSTIFICATIVA", string.IsNullOrEmpty(transferencia.Justificativa) ? null : transferencia.Justificativa);
            }

            contextQuery.Command += " WHERE ID_TRANSFERENCIA = @ID_TRANSFERENCIA";

            contextQuery.Parameters.Add("@MATRICULA", transferencia.MatriculaAndamento);
            contextQuery.Parameters.Add("@STATUS", situacao);
            contextQuery.Parameters.Add("@ID_TRANSFERENCIA", transferencia.IdTransferencia);

            listaContextQuery.Add(contextQuery);
        }

        #region Carregamento de Entidades

        private void CarregaEntidadeLyHCurrAluno(RN.Entidades.TceTransferencia transferencia, RN.Entidades.TceTransferenciaOrigem transferenciaOrigem, out RN.Entidades.LyHCurrAluno hCurrAluno)
        {
            hCurrAluno = new Techne.Lyceum.RN.Entidades.LyHCurrAluno();
            hCurrAluno.Curso = transferenciaOrigem.Curso;
            hCurrAluno.Turno = transferenciaOrigem.Turno;
            hCurrAluno.Curriculo = transferenciaOrigem.Curriculo;
            hCurrAluno.Aluno = transferencia.Aluno;
            hCurrAluno.Serie = transferenciaOrigem.Serie;
            hCurrAluno.Ano = Convert.ToInt32(transferenciaOrigem.Ano);
            hCurrAluno.Periodo = Convert.ToInt32(transferenciaOrigem.Periodo);
            hCurrAluno.Turma = transferenciaOrigem.Turma;
            hCurrAluno.UnidadeFisica = transferenciaOrigem.UnidadeFisica;
            hCurrAluno.Motivo = transferencia.Motivo;
            hCurrAluno.UnidadeEnsino = transferenciaOrigem.Censo;
        }

        private void CarregaEntidadeLyAluno(RN.Entidades.TceTransferencia transferencia, RN.Entidades.TceTransferenciaDestino transferenciaDestino, out RN.Entidades.LyAluno lyAluno)
        {
            lyAluno = new Techne.Lyceum.RN.Entidades.LyAluno();
            lyAluno.Curso = transferenciaDestino.Curso;
            lyAluno.Turno = transferenciaDestino.Turno;
            lyAluno.Curriculo = transferenciaDestino.Curriculo;
            lyAluno.Serie = transferenciaDestino.Serie;
            lyAluno.UnidadeEnsino = transferenciaDestino.Censo;
            lyAluno.UnidadeFisica = transferenciaDestino.UnidadeFisica;
            lyAluno.TipoEnsinoProfissionalizante = transferenciaDestino.TipoCurso;
            lyAluno.Aluno = transferencia.Aluno;
        }

        private void CarregaEntidadeConfirmacaoMatricula(RN.Entidades.TceTransferencia transferencia, RN.Entidades.TceTransferenciaDestino transferenciaDestino, int idConfirmacaoMatricula, out RN.Entidades.TceConfirmacaoMatricula confirmacaoMatricula, bool atualizar)
        {
            confirmacaoMatricula = new Techne.Lyceum.RN.Entidades.TceConfirmacaoMatricula();
            confirmacaoMatricula.Curso = transferenciaDestino.Curso;
            confirmacaoMatricula.Serie = transferenciaDestino.Serie;
            confirmacaoMatricula.Turno = transferenciaDestino.Turno;
            confirmacaoMatricula.Censo = transferenciaDestino.Censo;
            confirmacaoMatricula.Curriculo = transferenciaDestino.Curriculo;
            confirmacaoMatricula.Periodo = transferenciaDestino.Periodo;
            confirmacaoMatricula.Status = ConfirmacaoMatricula.Confirmado;
            confirmacaoMatricula.Matricula = transferencia.MatriculaAndamento;
            confirmacaoMatricula.IdConfirmacaoMatricula = idConfirmacaoMatricula;
            confirmacaoMatricula.EnsinoReligioso = transferenciaDestino.EnsinoReligioso;
            confirmacaoMatricula.LinguaEstrangeiraFacultativa = transferenciaDestino.LinguaEstrangeiraFacultativa;

            if (!atualizar)
            {
                confirmacaoMatricula.Aluno = transferencia.Aluno;
                confirmacaoMatricula.Ano = transferenciaDestino.Ano;
                confirmacaoMatricula.ProjetoAutonomia = false;
                confirmacaoMatricula.TipoVagaOcupada = "VN";
            }
        }

        private void CarregaEntidadeLyMatricula(RN.Entidades.TceTransferencia transferencia, RN.Entidades.TceTransferenciaDestino transferenciaDestino, string disciplina, out RN.Entidades.LyMatricula lyMatricula)
        {
            lyMatricula = new RN.Entidades.LyMatricula();
            lyMatricula.Aluno = transferencia.Aluno;
            lyMatricula.Disciplina = disciplina;
            lyMatricula.Turma = transferenciaDestino.Turma;
            lyMatricula.Ano = transferenciaDestino.Ano;
            lyMatricula.Semestre = transferenciaDestino.Periodo;
            lyMatricula.SitMatricula = Matricula.Matriculado;
            lyMatricula.CobrancaSep = "N";
            lyMatricula.Concomitante = "N";
            lyMatricula.Dependencia = "N";
            lyMatricula.EducEspecial = "N";
            lyMatricula.MaisEducacao = "N";
            lyMatricula.Matricula = transferencia.MatriculaAndamento;
        }

        private void CarregaEntidadeLyNota(RN.Entidades.TceTransferencia transferencia, RN.Entidades.TceTransferenciaDestino transferenciaDestino, RN.DTOs.NotaTurmaAtualTransferenciaAluno notaTurmaAtualTransferenciaAluno, out RN.Entidades.LyNota lyNota)
        {
            lyNota = new Techne.Lyceum.RN.Entidades.LyNota();
            lyNota.Aluno = transferencia.Aluno;
            lyNota.Disciplina = notaTurmaAtualTransferenciaAluno.Disciplina;
            lyNota.Turma = transferenciaDestino.Turma;
            lyNota.Ano = transferenciaDestino.Ano;
            lyNota.Semestre = transferenciaDestino.Periodo;
            lyNota.Prova = notaTurmaAtualTransferenciaAluno.Prova;
            lyNota.Conceito = notaTurmaAtualTransferenciaAluno.Conceito;
            lyNota.Ordem = notaTurmaAtualTransferenciaAluno.Ordem;
            lyNota.Formulario = notaTurmaAtualTransferenciaAluno.Formulario;
            lyNota.Compareceu = notaTurmaAtualTransferenciaAluno.Compareceu;
            lyNota.Data = DateTime.Today;
            lyNota.RecuperacaoParalela = notaTurmaAtualTransferenciaAluno.RecuperacaoParalela;
            lyNota.SemAvaliacao = notaTurmaAtualTransferenciaAluno.SemAvaliacao;
            lyNota.Justificativa = notaTurmaAtualTransferenciaAluno.Justificativa;
            lyNota.NotaId = notaTurmaAtualTransferenciaAluno.NotaId;
            lyNota.NotaProva = notaTurmaAtualTransferenciaAluno.NotaProva;
            lyNota.NotaRecuperacao = notaTurmaAtualTransferenciaAluno.NotaRecuperacao;
            lyNota.MotivoSemNotaId = notaTurmaAtualTransferenciaAluno.MotivoSemNotaId;
        }

        private void CarregaEntidadeLyFalta(RN.Entidades.TceTransferencia transferencia, RN.Entidades.TceTransferenciaDestino transferenciaDestino, RN.DTOs.FrequenciaAlunoTurma frequenciaAlunoTurma, out RN.Entidades.LyFalta lyFalta)
        {
            lyFalta = new Techne.Lyceum.RN.Entidades.LyFalta();
            lyFalta.Aluno = transferencia.Aluno;
            lyFalta.Ano = transferenciaDestino.Ano;
            lyFalta.Disciplina = frequenciaAlunoTurma.Disciplina;
            lyFalta.Faltas = frequenciaAlunoTurma.Faltas;
            lyFalta.FaltasCompensadas = frequenciaAlunoTurma.FaltasCompensadas;
            lyFalta.Periodo = transferenciaDestino.Periodo;
            lyFalta.Turma = transferenciaDestino.Turma;
            lyFalta.Freq = frequenciaAlunoTurma.Freq;
        }

        #endregion

        private static string BuscarTurmaDisciplina(DataContext contexto, string curso, string turno, string curriculo, decimal? serie, decimal? ano, decimal? periodo, string disciplina, string nivel, string turma)
        {
            var dataTable = contexto.GetDataTable(
                new ContextQuery(
                    @"SELECT  TURMA
                      FROM    LY_TURMA
                      WHERE   CURSO = @CURSO
                              AND TURNO = @TURNO
                              AND CURRICULO = @CURRICULO
                              AND SERIE = @SERIE
                              AND ANO = @ANO
                              AND SEMESTRE = @PERIODO
                              AND DISCIPLINA = @DISCIPLINA
                              AND NIVEL = @NIVEL
                              AND SUBSTRING(TURMA, 1, @FOO) = @TURMA",
                    new ContextQueryParameter("@CURSO", curso),
                    new ContextQueryParameter("@TURNO", turno),
                    new ContextQueryParameter("@CURRICULO", curriculo),
                    new ContextQueryParameter("@SERIE", serie),
                    new ContextQueryParameter("@ANO", ano),
                    new ContextQueryParameter("@PERIODO", periodo),
                    new ContextQueryParameter("@DISCIPLINA", disciplina),
                    new ContextQueryParameter("@NIVEL", nivel),
                    new ContextQueryParameter("@TURMA", turma)));

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows[0]["TURMA"].ToString();
            }

            return string.Empty;
        }

        public static string ValidaMatricula(DataContext dataContext, string disciplina, string turma, decimal? ano, decimal? periodo)
        {
            var alunosNaTurma = dataContext.GetDataTable(
                new ContextQuery(
                    @"SELECT  num_alunos,
                            sit_turma
                    FROM    LY_TURMA
                    WHERE   DISCIPLINA = @DISCIPLINA
                            AND TURMA = @TURMA
                            AND ANO = @ANO
                            AND SEMESTRE = @PERIODO",
                    new ContextQueryParameter("@DISCIPLINA", disciplina),
                    new ContextQueryParameter("@TURMA", turma),
                    new ContextQueryParameter("@ANO", ano),
                    new ContextQueryParameter("@PERIODO", periodo)));

            var totalAlunos = dataContext.GetReturnValue<int>(
                new ContextQuery(
                    @"SELECT  COUNT(*) AS qtMatriculados
                    FROM    LY_MATRICULA
                    WHERE   DISCIPLINA = @DISCIPLINA
                            AND TURMA = @TURMA
                            AND ANO = @ANO
                            AND SEMESTRE = @PERIODO
                            AND SIT_MATRICULA IN ( 'Matriculado', 'Aprovado', 'Rep Freq', 'Rep Nota' )",
                    new ContextQueryParameter("@DISCIPLINA", disciplina),
                    new ContextQueryParameter("@TURMA", turma),
                    new ContextQueryParameter("@ANO", ano),
                    new ContextQueryParameter("@PERIODO", periodo)));

            if (alunosNaTurma.Rows.Count > 0)
            {
                if (alunosNaTurma.Rows[0]["sit_turma"].ToString() != "Aberta")
                {
                    return "Turma não está aberta.";
                }

                if (alunosNaTurma.Rows[0]["num_alunos"].ToString() != string.Empty)
                {
                    if (totalAlunos < Convert.ToInt32(alunosNaTurma.Rows[0]["num_alunos"]))
                    {
                        return string.Empty;
                    }

                    return "Turma já está lotada.";
                }
            }

            return string.Empty;
        }

        public static ValidacaoDados ValidarRecusa(TceTransferencia transferencia)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados();

            if (transferencia == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(transferencia.Status)
                || transferencia.Status != Recusada)
            {
                mensagens.Add("O STATUS RECUSADO é obrigatório!");
            }

            if (string.IsNullOrEmpty(transferencia.Observacao))
            {
                mensagens.Add("O campo OBSERVAÇÃO é obrigatório!");
            }

            var justificativa = transferencia.Justificativa == null ? string.Empty : transferencia.Justificativa.Trim();

            if (transferencia.Observacao == "Outros"
                && string.IsNullOrEmpty(justificativa))
            {
                mensagens.Add("O campo JUSTIFICATIVA é obrigatório para Observação: OUTROS!");
            }

            if (!string.IsNullOrEmpty(justificativa))
            {
                if (justificativa.Length < 10)
                {
                    mensagens.Add("O campo JUSTIFICATIVA deve ter mais 10 caracteres.");
                }

                var regex = new Regex(@"(\w)\1\1+");

                if (regex.IsMatch(justificativa))
                {
                    mensagens.Add("O campo justificativa não deve ter mais de 2 letras consecutivas repetidas.");
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

        public ValidacaoDados ValidarAceite(TceTransferencia transferencia)
        {
            var validacaoDados = new ValidacaoDados();
            RN.Aluno rnAluno = new Aluno();

            if (transferencia == null
                || transferencia.IdTransferencia == 0)
            {
                validacaoDados.Mensagem = "É necessário selecionar uma transferência!";
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(transferencia.Aluno))
            {
                validacaoDados.Mensagem = "É necessário selecionar um aluno!";
                return validacaoDados;
            }

            var mensagens = new List<string>();
            TceTransferenciaDestino transferenciaDestino;
            TceTransferenciaOrigem transferenciaOrigem;

            CarregarOrigemDestino(transferencia, out transferenciaDestino, out transferenciaOrigem);

            if (transferenciaDestino.IdTransferenciaDestino == 0
                || transferenciaOrigem.IdTransferenciaOrigem == 0)
            {
                validacaoDados.Mensagem = "Falha ao carregar as informações de origem e destino da transferência!";
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(transferenciaDestino.Censo))
            {
                validacaoDados.Mensagem = "Informe a unidade de ensino para onde o aluno será transferido.";
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(transferenciaDestino.UnidadeFisica))
            {
                validacaoDados.Mensagem = "Informe a unidade física para onde o aluno será transferido.";
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(transferenciaDestino.Curso))
            {
                validacaoDados.Mensagem = "Informe o curso para onde o aluno será transferido.";
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(transferenciaDestino.Turno))
            {
                validacaoDados.Mensagem = "Informe o turno para onde o aluno será transferido.";
                return validacaoDados;
            }

            if (transferenciaDestino.Serie == 0)
            {
                validacaoDados.Mensagem = "Informe a série para onde o aluno será transferido.";
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(transferenciaDestino.Turma))
            {
                validacaoDados.Mensagem = "Informe a turma para onde o aluno será transferido.";
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(transferencia.Motivo))
            {
                validacaoDados.Mensagem = "Informe o motivo da transferência.";
                return validacaoDados;
            }

            if (transferenciaOrigem.Censo == transferenciaDestino.Censo
                && transferenciaOrigem.UnidadeFisica == transferenciaDestino.UnidadeFisica)
            {
                validacaoDados.Mensagem = "Transferência cancelada. Os dados do destino são os mesmos dos atuais.";
                return validacaoDados;
            }

            if (!string.IsNullOrEmpty(transferenciaDestino.Curso))
            {
                var tipoCurso = Curso.ConsultarTipoProfCurso(transferenciaDestino.Curso);

                if (tipoCurso == "Especial")
                {
                    if (rnAluno.EhAlunoSemNecessidadeEspecialPor(transferencia.Aluno))
                    {
                        mensagens.Add("Para escolher um curso de educação especial, informar o tipo de necessidade especial do aluno(a) na aba 'Dados Pessoais'.");
                    }
                }
                else if (tipoCurso == "Concomitante/Subsequente")
                {
                    if (string.IsNullOrEmpty(transferenciaDestino.TipoCurso))
                    {
                        mensagens.Add("Para escolher um curso Concomitante/Subsequente, deverá escolher o Tipo de Ensino Profissionalizante.");
                    }
                }
            }

            if (mensagens.Count == 0)
            {
                string gradeId = ConsultarCampo(
                    new ContextQuery(
                        @"SELECT TOP 1
                            grade_id
                    FROM    LY_GRADE_SERIE
                    WHERE   ANO = @ANO
                            AND SEMESTRE = @PERIODO
                            AND CURSO = @CURSO
                            AND CURRICULO = @CURRICULO
                            AND SERIE = @SERIE
                            AND GRADE = @GRADE",
                        new ContextQueryParameter("@ANO", transferenciaDestino.Ano),
                        new ContextQueryParameter("@PERIODO", transferenciaDestino.Periodo),
                        new ContextQueryParameter("@CURSO", transferenciaDestino.Curso),
                        new ContextQueryParameter("@CURRICULO", transferenciaDestino.Curriculo),
                        new ContextQueryParameter("@SERIE", transferenciaDestino.Serie),
                        new ContextQueryParameter("@GRADE", transferenciaDestino.Turma)));

                if (string.IsNullOrEmpty(gradeId))
                {
                    mensagens.Add("Não foi possível obter o código interno da Turma a ser transferida.");
                }

                //Verifica se turma de destino está aberta
                string situacaoTurma = ConsultarCampo(
                   new ContextQuery(
                       @" SELECT TOP 1
                                        SIT_TURMA
                                FROM    dbo.LY_TURMA
                                WHERE   ANO = @ANO
                                        AND SEMESTRE = @PERIODO
                                        AND TURMA = @TURMA ",
                       new ContextQueryParameter("@ANO", transferenciaDestino.Ano),
                       new ContextQueryParameter("@PERIODO", transferenciaDestino.Periodo),
                       new ContextQueryParameter("@TURMA", transferenciaDestino.Turma)));

                if (string.IsNullOrEmpty(situacaoTurma) || situacaoTurma.ToUpper() != "ABERTA")
                {
                    mensagens.Add(string.Format("Transferência cancelada. A turma de destino do aluno {0} não está aberta.", transferencia.Aluno));
                }
            }

            //Verificar se turma continua Ativa
            if (RN.SituacaoFinalAluno.ExisteSituacaoFinalPor(transferencia.Aluno, transferenciaDestino.Ano, transferenciaDestino.Periodo, transferenciaDestino.Turma))
            {
                mensagens.Add("Transferência cancelada. O aluno já possui situação final para este ano/periodo/turma destino.");
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

        public static ValidacaoDados ValidarSolicitacao(TceTransferencia tceTransferencia, TceTransferenciaDestino tceTransferenciaDestino, TceTransferenciaOrigem tceTransferenciaOrigem, bool compartilhada)
        {
            RN.UsuarioUnidadeFis rnUsuarioUnidadeFis = new UsuarioUnidadeFis();
            RN.Compartilhada rnCompartilhada = new Compartilhada();
            RN.ControleVaga rnControleVaga = new ControleVaga();
            TceConfirmacaoMatricula confirmacao = new TceConfirmacaoMatricula();
            RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            RN.Aluno rnAluno = new RN.Aluno();
            RN.Aluno.DadosAluno aluno = new RN.Aluno.DadosAluno();
            RN.Matriculas.PessoaAluno rnPessoaAluno = new Techne.Lyceum.RN.Matriculas.PessoaAluno();
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
                                 {
                                     Valido = false
                                 };

            if (tceTransferencia == null)
            {
                return validacaoDados;
            }

            if (tceTransferenciaDestino == null)
            {
                return validacaoDados;
            }

            if (tceTransferenciaOrigem == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(tceTransferencia.Aluno))
            {
                mensagens.Add("O campo ALUNO é obrigatório!");
            }

            if (string.IsNullOrEmpty(tceTransferenciaOrigem.Censo))
            {
                mensagens.Add("O campo ESCOLA DE ORIGEM é obrigatório!");
            }

            if (string.IsNullOrEmpty(tceTransferenciaDestino.Censo))
            {
                mensagens.Add("O campo ESCOLA DE DESTINO é obrigatório!");
            }

            if (!string.IsNullOrEmpty(tceTransferenciaDestino.Censo)
                && !string.IsNullOrEmpty(tceTransferenciaOrigem.Censo)
                && tceTransferenciaOrigem.Censo == tceTransferenciaDestino.Censo)
            {
                mensagens.Add("As escolas de ORIGEM e DESTINO devem ser diferentes!");
            }

            if (tceTransferenciaDestino.Ano <= 0)
            {
                mensagens.Add("O campo ANO LETIVO é obrigatório");
            }

            if (string.IsNullOrEmpty(tceTransferenciaDestino.Turno))
            {
                mensagens.Add("O campo TURNO é obrigatório!");
            }

            if (tceTransferenciaDestino.Serie <= 0)
            {
                mensagens.Add("O campo SÉRIE é obrigatório");
            }

            if (string.IsNullOrEmpty(tceTransferenciaDestino.Turma))
            {
                mensagens.Add("O campo TURMA é obrigatório!");
            }

            if (string.IsNullOrEmpty(tceTransferencia.Motivo))
            {
                mensagens.Add("O campo MOTIVO é obrigatório!");
            }

            if (string.IsNullOrEmpty(tceTransferenciaDestino.Curso))
            {
                mensagens.Add("O campo CURSO é obrigatório!");
            }

            aluno = rnAluno.ObtemDadosAluno(tceTransferencia.Aluno);

            if (aluno.UnidadeResponsavel != tceTransferenciaOrigem.Censo)
            {
                mensagens.Add("A ESCOLA DE ORIGEM não é mais a ESCOLA ATUAL do aluno.");
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);

                return validacaoDados;
            }

            // Início das validações de negócio
            if (ExistePendenciaTranf(tceTransferencia.Aluno))
            {
                mensagens.Add("Já existe uma solicitação de transferência pendente para este aluno.");
            }

            if (!string.IsNullOrEmpty(tceTransferenciaDestino.Curso))
            {
                var tipoCurso = Curso.ConsultarTipoProfCurso(tceTransferenciaDestino.Curso);

                if (tipoCurso == "Especial")
                {
                    if (rnAluno.EhAlunoSemNecessidadeEspecialPor(tceTransferencia.Aluno))
                    {
                        mensagens.Add("Para escolher um curso de educação especial, informar o tipo de necessidade especial do aluno(a) na aba 'Dados Pessoais'.");
                    }
                }
                else if (tipoCurso == "Concomitante/Subsequente")
                {
                    if (string.IsNullOrEmpty(tceTransferenciaDestino.TipoCurso))
                    {
                        mensagens.Add("Para escolher um curso Concomitante/Subsequente, deverá escolher o Tipo de Ensino Profissionalizante.");
                    }
                }
            }

            if (mensagens.Count == 0)
            {
                //Verifica a data de nascimento do aluno
                if (aluno.DataNascimento == null || aluno.DataNascimento == DateTime.MinValue)
                {
                    mensagens.Add("Aluno sem data de nascimento para validação.");
                }
                else
                {
                    //Verifica se é transferencia de aluno de compartilhada
                    if (compartilhada)
                    {
                        //Verifica se existe compartilhada
                        if (!rnCompartilhada.PossuiUnidadeCompartilhadaDestino(tceTransferenciaDestino.Censo))
                        {
                            mensagens.Add("Não foi encontrado registro de unidade compartilhada para a unidade do aluno");
                        }
                    }
                    else if (rnControleVaga.PartipaMatriculaFacilPor(tceTransferenciaDestino.UnidadeFisica, tceTransferenciaDestino.Ano, tceTransferenciaDestino.Periodo, tceTransferenciaDestino.Curso, tceTransferenciaDestino.Serie, tceTransferenciaDestino.Turno))
                    {
                        mensagens.Add("Não será possível realizar a transferência pois a unidade/curso/série/turno está participando do matrícula fácil.");
                    }

                    //Verifica se o aluno esta fechado
                    if (aluno.SitAluno != "Ativo")
                    {
                        //Verifica matricula correta da pessoa
                        string matricula = rnPessoaAluno.ObtemOutraPessoaAlunoPor(aluno.Aluno);

                        if (!matricula.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add("Não será possível realizar a transferência pois está não é a matricula mais atual deste aluno. Solicite a transferência da matrícula de número " + matricula + ".");
                        }

                        //Verifica se existe outro aluno ativo com mesmo nome / mae / data Nascimento
                        if (rnAluno.PossuiOutroAlunoAtivoPor(aluno.Nome_compl, aluno.NomeMae, Convert.ToDateTime(aluno.DataNascimento), tceTransferencia.Aluno))
                        {
                            mensagens.Add("Não será possível realizar a transferência do aluno cancelado, pois já existe outro aluno ativo com mesmo nome/data de nascimento/nome da mãe.");
                        }

                        if (!aluno.Cpf.IsNullOrEmptyOrWhiteSpace())
                        {
                            //Verifica se existe outro aluno ativo com mesmo cpf
                            if (rnAluno.PossuiOutroCPFAtivoPor(aluno.Cpf, tceTransferencia.Aluno))
                            {
                                mensagens.Add("Não será possível realizar a transferência do aluno cancelado, pois já existe outro aluno ativo com mesmo CPF");
                            }
                        }
                    }
                }

                //Verifica se o usuario possue permissao para a escola solicitada
                if (!rnUsuarioUnidadeFis.PossuiPermissaoPor(tceTransferencia.MatriculaSolicitante, tceTransferenciaDestino.UnidadeFisica))
                {
                    mensagens.Add("O usuário não possui permissão para solicitar transferencia para esta unidade.");
                }

                //Para transferencia a vaga será sempre do tipo vaga nova (VN)
                //Caso não exista validar vagas disponíveis para a serie ou modalidade ou turno pretendido

                int vagasLiberadas = 0;
                int vagasUtilizadas = 0;

                vagasLiberadas = rnControleVaga.ObtemVagasLiberadasTotalPor(
                    tceTransferenciaDestino.Censo,
                    tceTransferenciaDestino.Ano,
                    tceTransferenciaDestino.Periodo,
                    tceTransferenciaDestino.Serie,
                    tceTransferenciaDestino.Curso,
                    tceTransferenciaDestino.Turno);

                vagasUtilizadas = rnControleVaga.ObtemVagasUtilizadasTotalPor(
                   tceTransferenciaDestino.Censo,
                   tceTransferenciaDestino.Ano,
                   tceTransferenciaDestino.Periodo,
                   tceTransferenciaDestino.Serie,
                   tceTransferenciaDestino.Curso,
                   tceTransferenciaDestino.Turno);

                if (vagasLiberadas <= vagasUtilizadas)
                {
                    mensagens.Add("Não será possível realizar a solicitação de transferência, pois não existem vagas disponíveis para a serie ou modalidade ou turno pretendidos!");
                }

                var vagasTurma = Turma.RetornaVagasPrincipal(
                   tceTransferenciaDestino.Censo,
                   tceTransferenciaDestino.Ano,
                   tceTransferenciaDestino.Periodo,
                   tceTransferenciaDestino.Serie,
                   tceTransferenciaDestino.Curso,
                   tceTransferenciaDestino.Turno,
                   tceTransferenciaDestino.Turma);

                if (vagasTurma <= 0)
                {
                    mensagens.Add("Não será possível realizar a solicitação de transferência, pois a turma informada já atingiu o quantitativo de alunos permitidos. Por favor, verificar disponibilidade em outra turma!");
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

        public void InsereFechamentoMatricula(DataContext ctx, TceTransferencia tceTransferencia, TceTransferenciaDestino tceTransferenciaDestino, TceTransferenciaOrigem tceTransferenciaOrigem)
        {
            try
            {
                int id = 0;

                id = InserirTransferencia(ctx, tceTransferencia);

                tceTransferenciaDestino.IdTransferencia = id;
                InserirTransferenciaDestinoComCurriculo(ctx, tceTransferenciaDestino);

                tceTransferenciaOrigem.IdTransferencia = id;
                InserirTransferenciaOrigem(ctx, tceTransferenciaOrigem);
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

        public void GeraTransferenciaEnturmaAlunoPreMatricula(DataContext contexto, string aluno, string usuarioResponsavel, int opcaoInscricaoId, string turma, string curriculo, bool ensinoReligioso, bool linguaEstrangeiraFacultativa)
        {
            TceTransferencia transferencia = new TceTransferencia();
            TceTransferenciaOrigem transferenciaOrigem = new TceTransferenciaOrigem();
            TceTransferenciaDestino transferenciaDestino = new TceTransferenciaDestino();
            RN.ControleVaga rnControleVaga = new ControleVaga();
            TceControleVaga controleVaga = new TceControleVaga();
            RN.Matricula rnMatricula = new Matricula();
            RN.Aluno.DadosAluno dadosAluno = new Aluno.DadosAluno();
            RN.Aluno rnAluno = new Aluno();

            //TRANSFERENCIA
            transferencia.Aluno = aluno;
            transferencia.Motivo = "Outros";
            transferencia.MatriculaSolicitante = usuarioResponsavel;
            transferencia.Status = Transferencia.Aceita;
            transferencia.MatriculaAndamento = usuarioResponsavel;
            transferencia.Observacao = null;

            //Busca dados de destino pelo controleVaga
            controleVaga = rnControleVaga.ObtemControleVagaOpcaoPor(contexto, opcaoInscricaoId);

            //Destino
            transferenciaDestino.Turno = controleVaga.Turno;
            transferenciaDestino.Turma = turma;
            transferenciaDestino.TipoCurso = null;
            transferenciaDestino.Ano = controleVaga.Ano;
            transferenciaDestino.Periodo = controleVaga.Periodo;
            transferenciaDestino.Serie = controleVaga.Serie;
            transferenciaDestino.Curso = controleVaga.Curso;
            transferenciaDestino.Censo = controleVaga.Censo;
            transferenciaDestino.UnidadeFisica = controleVaga.Censo;
            transferenciaDestino.Curriculo = curriculo;
            transferenciaDestino.EnsinoReligioso = ensinoReligioso;
            transferenciaDestino.LinguaEstrangeiraFacultativa = linguaEstrangeiraFacultativa;

            //Busca Dados de origem pelo aluno
            dadosAluno = rnAluno.ObtemDadosAluno(contexto, aluno);

            //Origem
            transferenciaOrigem.Censo = dadosAluno.UnidadeResponsavel;
            transferenciaOrigem.UnidadeFisica = dadosAluno.UnidadeResponsavel;
            transferenciaOrigem.Turno = dadosAluno.Turno;
            transferenciaOrigem.Curriculo = dadosAluno.Curriculo;
            transferenciaOrigem.Turma = null;
            transferenciaOrigem.Ano = null;
            transferenciaOrigem.Periodo = null;
            transferenciaOrigem.Serie = Convert.ToInt32(dadosAluno.Serie);
            transferenciaOrigem.Curso = dadosAluno.Curso;

            //Insere transferencia
            int id = 0;
            id = InsereTransferenciaFinalizada(contexto, transferencia);

            transferenciaDestino.IdTransferencia = id;
            InserirTransferenciaDestinoComCurriculo(contexto, transferenciaDestino);

            transferenciaOrigem.IdTransferencia = id;
            InserirTransferenciaOrigem(contexto, transferenciaOrigem);

            //Enturma o aluno
            rnMatricula.EnturmaAluno(contexto, aluno, controleVaga.Ano, controleVaga.Periodo, turma, controleVaga.Curso, curriculo, controleVaga.Serie, usuarioResponsavel);

            //Atualiza escola aluno
            rnAluno.AtualizaEscola(contexto, aluno, transferenciaDestino.Censo);
        }

        public void GeraTransferenciaAluno(DataContext contexto, string aluno, string usuarioResponsavel, int ano, int periodo, string censo, string curso, string turno, int serie, string turma, string curriculo)
        {
            TceTransferencia transferencia = new TceTransferencia();
            TceTransferenciaOrigem transferenciaOrigem = new TceTransferenciaOrigem();
            TceTransferenciaDestino transferenciaDestino = new TceTransferenciaDestino();
            RN.Matricula rnMatricula = new Matricula();
            RN.Aluno.DadosAluno dadosAluno = new Aluno.DadosAluno();
            RN.Aluno rnAluno = new Aluno();

            //TRANSFERENCIA
            transferencia.Aluno = aluno;
            transferencia.Motivo = "Outros";
            transferencia.MatriculaSolicitante = usuarioResponsavel;
            transferencia.Status = Transferencia.Aceita;
            transferencia.MatriculaAndamento = usuarioResponsavel;
            transferencia.Observacao = null;

            //Destino
            transferenciaDestino.Turno = turno;
            transferenciaDestino.Turma = turma;
            transferenciaDestino.TipoCurso = null;
            transferenciaDestino.Ano = ano;
            transferenciaDestino.Periodo = periodo;
            transferenciaDestino.Serie = serie;
            transferenciaDestino.Curso = curso;
            transferenciaDestino.Censo = censo;
            transferenciaDestino.UnidadeFisica = censo;
            transferenciaDestino.Curriculo = curriculo;
            transferenciaDestino.EnsinoReligioso = false;
            transferenciaDestino.LinguaEstrangeiraFacultativa = false;

            //Busca Dados de origem pelo aluno
            dadosAluno = rnAluno.ObtemDadosAluno(contexto, aluno);

            //Origem
            transferenciaOrigem.Censo = dadosAluno.UnidadeResponsavel;
            transferenciaOrigem.UnidadeFisica = dadosAluno.UnidadeResponsavel;
            transferenciaOrigem.Turno = dadosAluno.Turno;
            transferenciaOrigem.Curriculo = dadosAluno.Curriculo;
            transferenciaOrigem.Turma = null;
            transferenciaOrigem.Ano = null;
            transferenciaOrigem.Periodo = null;
            transferenciaOrigem.Serie = Convert.ToInt32(dadosAluno.Serie);
            transferenciaOrigem.Curso = dadosAluno.Curso;

            //Insere transferencia
            int id = 0;
            id = InsereTransferenciaFinalizada(contexto, transferencia);

            transferenciaDestino.IdTransferencia = id;
            InserirTransferenciaDestinoComCurriculo(contexto, transferenciaDestino);

            transferenciaOrigem.IdTransferencia = id;
            InserirTransferenciaOrigem(contexto, transferenciaOrigem);
        }

        public void GeraTransferenciaEnturmaEncaminhamentoEspecial(DataContext contexto, DTOs.DadosEncaminhamentoEspecial dados)
        {
            TceTransferencia transferencia = new TceTransferencia();
            TceTransferenciaOrigem transferenciaOrigem = new TceTransferenciaOrigem();
            TceTransferenciaDestino transferenciaDestino = new TceTransferenciaDestino();
            RN.Matricula rnMatricula = new Matricula();
            RN.Aluno.DadosAluno dadosAluno = new Aluno.DadosAluno();
            RN.Aluno rnAluno = new Aluno();

            //TRANSFERENCIA
            transferencia.Aluno = dados.PessoaAluno.Aluno;
            transferencia.Motivo = "Outros";
            transferencia.MatriculaSolicitante = dados.UsuarioResponsavel;
            transferencia.Status = Transferencia.Aceita;
            transferencia.MatriculaAndamento = dados.UsuarioResponsavel;
            transferencia.Observacao = null;

            //Destino
            transferenciaDestino.Turno = dados.Turno;
            transferenciaDestino.Turma = dados.Turma;
            transferenciaDestino.TipoCurso = null;
            transferenciaDestino.Ano = dados.Ano;
            transferenciaDestino.Periodo = dados.Periodo;
            transferenciaDestino.Serie = dados.Serie;
            transferenciaDestino.Curso = dados.Curso;
            transferenciaDestino.Censo = dados.Censo;
            transferenciaDestino.UnidadeFisica = dados.Censo;
            transferenciaDestino.Curriculo = dados.Curriculo;
            transferenciaDestino.EnsinoReligioso = false;
            transferenciaDestino.LinguaEstrangeiraFacultativa = false;

            //Busca Dados de origem pelo aluno
            dadosAluno = rnAluno.ObtemDadosAluno(contexto, dados.PessoaAluno.Aluno);

            //Origem
            transferenciaOrigem.Censo = dadosAluno.UnidadeResponsavel;
            transferenciaOrigem.UnidadeFisica = dadosAluno.UnidadeResponsavel;
            transferenciaOrigem.Turno = dadosAluno.Turno;
            transferenciaOrigem.Curriculo = dadosAluno.Curriculo;
            transferenciaOrigem.Turma = null;
            transferenciaOrigem.Ano = null;
            transferenciaOrigem.Periodo = null;
            transferenciaOrigem.Serie = Convert.ToInt32(dadosAluno.Serie);
            transferenciaOrigem.Curso = dadosAluno.Curso;

            //Insere transferencia
            int id = 0;
            id = InsereTransferenciaFinalizada(contexto, transferencia);

            transferenciaDestino.IdTransferencia = id;
            InserirTransferenciaDestinoComCurriculo(contexto, transferenciaDestino);

            transferenciaOrigem.IdTransferencia = id;
            InserirTransferenciaOrigem(contexto, transferenciaOrigem);

            //Enturma o aluno
            rnMatricula.EnturmaAluno(contexto, dados.PessoaAluno.Aluno, dados.Ano, dados.Periodo, dados.Turma, dados.Curso, dados.Curriculo, dados.Serie, dados.UsuarioResponsavel);
        }

        public static void Inserir(TceTransferencia tceTransferencia, TceTransferenciaDestino tceTransferenciaDestino, TceTransferenciaOrigem tceTransferenciaOrigem, string statusAluno)
        {
            using (var context = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var id = InserirTransferencia(context, tceTransferencia);

                    tceTransferenciaDestino.IdTransferencia = id;
                    InserirTransferenciaDestino(context, tceTransferenciaDestino);

                    tceTransferenciaOrigem.IdTransferencia = id;
                    CarregaDadosOrigem(tceTransferenciaOrigem, tceTransferencia.Aluno, statusAluno == "Ativo");

                    InserirTransferenciaOrigem(context, tceTransferenciaOrigem);
                }
                catch (Exception)
                {
                    context.Abandon();

                    throw;
                }
            }
        }

        private int InsereTransferenciaFinalizada(DataContext context, TceTransferencia tceTransferencia)
        {
            DateTime dataAtual = DateTime.Now;

            var contextQuery = new ContextQuery(
                @" INSERT  INTO TCE_TRANSFERENCIA ( ALUNO, STATUS, MATRICULA_SOLICITANTE, MOTIVO, OBSERVACAO, MATRICULA_ANDAMENTO, DT_CADASTRO, DT_ALTERACAO )
                    VALUES  ( @ALUNO, @STATUS, @MATRICULA_SOLICITANTE, @MOTIVO, @OBSERVACAO, @MATRICULA_ANDAMENTO, @DATA, @DATA ) ");

            contextQuery.Parameters.Add("@ALUNO", tceTransferencia.Aluno);
            contextQuery.Parameters.Add("@STATUS", tceTransferencia.Status);
            contextQuery.Parameters.Add("@MATRICULA_SOLICITANTE", tceTransferencia.MatriculaSolicitante);
            contextQuery.Parameters.Add("@MOTIVO", tceTransferencia.Motivo);
            contextQuery.Parameters.Add("@OBSERVACAO", tceTransferencia.Observacao);
            contextQuery.Parameters.Add("@MATRICULA_ANDAMENTO", tceTransferencia.MatriculaAndamento);
            contextQuery.Parameters.Add("@DATA", dataAtual);

            context.ApplyModifications(contextQuery);

            contextQuery = new ContextQuery(
              @" SELECT  ID_TRANSFERENCIA
                    FROM    dbo.TCE_TRANSFERENCIA
                    WHERE   STATUS = @STATUS
                            AND ALUNO = @ALUNO
                            AND MATRICULA_SOLICITANTE = @MATRICULA_SOLICITANTE
                            AND MATRICULA_ANDAMENTO = @MATRICULA_ANDAMENTO 
                            AND DT_CADASTRO = @DATA
                            AND DT_ALTERACAO = @DATA ");

            contextQuery.Parameters.Add("@ALUNO", tceTransferencia.Aluno);
            contextQuery.Parameters.Add("@MATRICULA_SOLICITANTE", tceTransferencia.MatriculaSolicitante);
            contextQuery.Parameters.Add("@STATUS", tceTransferencia.Status);
            contextQuery.Parameters.Add("@MATRICULA_ANDAMENTO", tceTransferencia.MatriculaAndamento);
            contextQuery.Parameters.Add("@DATA", dataAtual);

            var id = Convert.ToInt32(context.GetReturnValue(contextQuery));

            return id;
        }

        private static int InserirTransferencia(DataContext context, TceTransferencia tceTransferencia)
        {
            DateTime dataAtual = DateTime.Now;

            var contextQuery = new ContextQuery(
                @"INSERT  INTO TCE_TRANSFERENCIA ( ALUNO, [STATUS], MATRICULA_SOLICITANTE, MOTIVO, DT_CADASTRO )
                    VALUES  ( @ALUNO, 'Pendente', @MATRICULA_SOLICITANTE, @MOTIVO, @DATA )");

            contextQuery.Parameters.Add("@ALUNO", tceTransferencia.Aluno);
            contextQuery.Parameters.Add("@MATRICULA_SOLICITANTE", tceTransferencia.MatriculaSolicitante);
            contextQuery.Parameters.Add("@MOTIVO", tceTransferencia.Motivo);
            contextQuery.Parameters.Add("@DATA", dataAtual);

            context.ApplyModifications(contextQuery);

            contextQuery = new ContextQuery(
              @" SELECT  ID_TRANSFERENCIA
                    FROM    dbo.TCE_TRANSFERENCIA
                    WHERE   STATUS = @STATUS
                            AND ALUNO = @ALUNO
                            AND MATRICULA_SOLICITANTE = @MATRICULA_SOLICITANTE 
                            AND DT_CADASTRO = @DATA ");

            contextQuery.Parameters.Add("@ALUNO", tceTransferencia.Aluno);
            contextQuery.Parameters.Add("@MATRICULA_SOLICITANTE", tceTransferencia.MatriculaSolicitante);
            contextQuery.Parameters.Add("@STATUS", Pendente);
            contextQuery.Parameters.Add("@DATA", dataAtual);

            var id = Convert.ToInt32(context.GetReturnValue(contextQuery));

            return id;
        }

        private static void InserirTransferenciaDestino(DataContext context, TceTransferenciaDestino tceTransferenciaDestino)
        {
            var ativo = !string.IsNullOrEmpty(tceTransferenciaDestino.TipoCurso);

            var contextQuery = new ContextQuery(
                string.Format(
                    @" DECLARE @CURRICULO VARCHAR(20)

                    SELECT TOP 1 @CURRICULO = CURRICULO FROM dbo.LY_TURMA
                    WHERE turma = @TURMA
                    AND ano = @ANO
                    AND SEMESTRE = @PERIODO

                    INSERT INTO TCE_TRANSFERENCIA_DESTINO ( ID_TRANSFERENCIA, CENSO, ANO, PERIODO,
                                                             CURSO, SERIE, TURNO, TURMA, UNIDADE_FISICA, CURRICULO, ENSINO_RELIGIOSO, LINGUA_ESTRANGEIRA_FACULTATIVA {0})
                                        VALUES  ( @ID_TRANSFERENCIA, @CENSO, @ANO, @PERIODO, @CURSO, @SERIE, @TURNO,
                                              @TURMA, @UNIDADE_FISICA, @CURRICULO, @ENSINO_RELIGIOSO, @LINGUA_ESTRANGEIRA_FACULTATIVA{1})",
                    ativo ? ", TIPO_CURSO" : string.Empty,
                    ativo ? ", @TIPO_CURSO" : string.Empty));

            contextQuery.Parameters.Add("@ID_TRANSFERENCIA", tceTransferenciaDestino.IdTransferencia);
            contextQuery.Parameters.Add("@CENSO", tceTransferenciaDestino.Censo);
            contextQuery.Parameters.Add("@ANO", tceTransferenciaDestino.Ano);
            contextQuery.Parameters.Add("@PERIODO", tceTransferenciaDestino.Periodo);
            contextQuery.Parameters.Add("@CURSO", tceTransferenciaDestino.Curso);
            contextQuery.Parameters.Add("@SERIE", tceTransferenciaDestino.Serie);
            contextQuery.Parameters.Add("@TURNO", tceTransferenciaDestino.Turno);
            contextQuery.Parameters.Add("@TURMA", tceTransferenciaDestino.Turma);
            contextQuery.Parameters.Add("@UNIDADE_FISICA", tceTransferenciaDestino.UnidadeFisica);
            contextQuery.Parameters.Add("@ENSINO_RELIGIOSO", tceTransferenciaDestino.EnsinoReligioso);
            contextQuery.Parameters.Add("@LINGUA_ESTRANGEIRA_FACULTATIVA", tceTransferenciaDestino.LinguaEstrangeiraFacultativa);

            if (ativo)
            {
                contextQuery.Parameters.Add("@TIPO_CURSO", tceTransferenciaDestino.TipoCurso);
            }

            context.ApplyModifications(contextQuery);
        }

        private static void InserirTransferenciaDestinoComCurriculo(DataContext context, TceTransferenciaDestino tceTransferenciaDestino)
        {
            var ativo = !string.IsNullOrEmpty(tceTransferenciaDestino.TipoCurso);

            var contextQuery = new ContextQuery(
                string.Format(
                    @" INSERT INTO TCE_TRANSFERENCIA_DESTINO ( ID_TRANSFERENCIA, CENSO, ANO, PERIODO,
                                                             CURSO, SERIE, TURNO, TURMA, UNIDADE_FISICA, CURRICULO, ENSINO_RELIGIOSO, LINGUA_ESTRANGEIRA_FACULTATIVA {0})
                                        VALUES  ( @ID_TRANSFERENCIA, @CENSO, @ANO, @PERIODO, @CURSO, @SERIE, @TURNO,
                                              @TURMA, @UNIDADE_FISICA, @CURRICULO, @ENSINO_RELIGIOSO, @LINGUA_ESTRANGEIRA_FACULTATIVA{1})",
                    ativo ? ", TIPO_CURSO" : string.Empty,
                    ativo ? ", @TIPO_CURSO" : string.Empty));

            contextQuery.Parameters.Add("@ID_TRANSFERENCIA", tceTransferenciaDestino.IdTransferencia);
            contextQuery.Parameters.Add("@CENSO", tceTransferenciaDestino.Censo);
            contextQuery.Parameters.Add("@ANO", tceTransferenciaDestino.Ano);
            contextQuery.Parameters.Add("@PERIODO", tceTransferenciaDestino.Periodo);
            contextQuery.Parameters.Add("@CURSO", tceTransferenciaDestino.Curso);
            contextQuery.Parameters.Add("@CURRICULO", tceTransferenciaDestino.Curriculo);
            contextQuery.Parameters.Add("@SERIE", tceTransferenciaDestino.Serie);
            contextQuery.Parameters.Add("@TURNO", tceTransferenciaDestino.Turno);
            contextQuery.Parameters.Add("@TURMA", tceTransferenciaDestino.Turma);
            contextQuery.Parameters.Add("@UNIDADE_FISICA", tceTransferenciaDestino.UnidadeFisica);
            contextQuery.Parameters.Add("@ENSINO_RELIGIOSO", tceTransferenciaDestino.EnsinoReligioso);
            contextQuery.Parameters.Add("@LINGUA_ESTRANGEIRA_FACULTATIVA", tceTransferenciaDestino.LinguaEstrangeiraFacultativa);

            if (ativo)
            {
                contextQuery.Parameters.Add("@TIPO_CURSO", tceTransferenciaDestino.TipoCurso);
            }

            context.ApplyModifications(contextQuery);
        }

        private static void CarregaDadosOrigem(TceTransferenciaOrigem tceTransferenciaOrigem, string aluno, bool ativo)
        {
            if (ativo)
            {
                CarregaDadosOrigemAtivo(tceTransferenciaOrigem, aluno);
            }
            else
            {
                CarregaDadosOrigemInativo(tceTransferenciaOrigem, aluno);
            }
        }

        private static void CarregaDadosOrigemInativo(TceTransferenciaOrigem tceTransferenciaOrigem, string aluno)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT a.CURSO, a.SERIE, a.TURNO, a.UNIDADE_ENSINO, u.UNIDADE_FIS, curriculo
                    FROM  dbo.LY_ALUNO a
                    inner join LY_UNIDADES_ASSOCIADAS u
                        on a.UNIDADE_ENSINO = u.UNIDADE_ENS
                    WHERE   a.aluno = @ALUNO
                        AND a.UNIDADE_ENSINO = @CENSO ");

                contextQuery.Parameters.Add("@CENSO", tceTransferenciaOrigem.Censo);
                contextQuery.Parameters.Add("@ALUNO", aluno);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    if (reader.Read())
                    {
                        tceTransferenciaOrigem.Ano = null;
                        tceTransferenciaOrigem.Periodo = null;
                        tceTransferenciaOrigem.Curso = Convert.ToString(reader["CURSO"]);
                        tceTransferenciaOrigem.Serie = Convert.ToInt32(reader["SERIE"]);
                        tceTransferenciaOrigem.Turno = Convert.ToString(reader["TURNO"]);
                        tceTransferenciaOrigem.Turma = null;
                        tceTransferenciaOrigem.UnidadeFisica = Convert.ToString(reader["UNIDADE_FIS"]);
                        tceTransferenciaOrigem.Curriculo = Convert.ToString(reader["curriculo"]);
                    }
                }
            }
        }

        private static void CarregaDadosOrigemAtivo(TceTransferenciaOrigem tceTransferenciaOrigem, string aluno)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT DISTINCT TOP 1
                            mat.ANO,
                            mat.SEMESTRE,
                            al.CURSO,
                            al.SERIE,
                            al.TURNO,
                            t.TURMA,
                            a.UNIDADE_FIS,
                            al.CURRICULO
                    FROM    dbo.LY_ALUNO al
                            LEFT JOIN ly_matricula mat ON al.ALUNO = mat.ALUNO
                                                          AND mat.SIT_MATRICULA <> 'Cancelado'
                            LEFT JOIN LY_TURMA t ON mat.DISCIPLINA = t.DISCIPLINA
                                                    AND mat.TURMA = t.TURMA
                                                    AND mat.ANO = t.ANO
                                                    AND mat.SEMESTRE = t.SEMESTRE
                            INNER JOIN dbo.LY_UNIDADES_ASSOCIADAS a ON al.UNIDADE_ENSINO = a.UNIDADE_ENS
                    WHERE   al.aluno = @ALUNO
                            AND al.UNIDADE_ENSINO = @CENSO
                    ORDER BY mat.ANO DESC");

                contextQuery.Parameters.Add("@CENSO", tceTransferenciaOrigem.Censo);
                contextQuery.Parameters.Add("@ALUNO", aluno);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    if (reader.Read())
                    {
                        tceTransferenciaOrigem.Ano = reader["ANO"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["ANO"]);
                        tceTransferenciaOrigem.Periodo = reader["SEMESTRE"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["SEMESTRE"]);
                        tceTransferenciaOrigem.Curso = Convert.ToString(reader["CURSO"]);
                        tceTransferenciaOrigem.Serie = Convert.ToInt32(reader["SERIE"]);
                        tceTransferenciaOrigem.Turno = Convert.ToString(reader["TURNO"]);
                        tceTransferenciaOrigem.Turma = reader["TURMA"] == DBNull.Value ? string.Empty : Convert.ToString(reader["TURMA"]);
                        tceTransferenciaOrigem.UnidadeFisica = Convert.ToString(reader["UNIDADE_FIS"]);
                        tceTransferenciaOrigem.Curriculo = Convert.ToString(reader["CURRICULO"]);
                    }
                }
            }
        }

        private static void InserirTransferenciaOrigem(DataContext context, TceTransferenciaOrigem tceTransferenciaOrigem)
        {
            var ativo = tceTransferenciaOrigem.Ano.HasValue
                        && tceTransferenciaOrigem.Periodo.HasValue
                        && !string.IsNullOrEmpty(tceTransferenciaOrigem.Turma);

            var contextQuery = new ContextQuery(
                string.Format(
                    @"INSERT  INTO TCE_TRANSFERENCIA_ORIGEM ( ID_TRANSFERENCIA, CENSO, CURSO, SERIE,CURRICULO,UNIDADE_FISICA, TURNO{0})
                    VALUES  ( @ID_TRANSFERENCIA, @CENSO, @CURSO, @SERIE,@CURRICULO,@UNIDADE_FISICA, @TURNO{1})",
                    ativo ? ", ANO, PERIODO, TURMA" : string.Empty,
                    ativo ? ", @ANO, @PERIODO, @TURMA" : string.Empty));

            contextQuery.Parameters.Add("@ID_TRANSFERENCIA", tceTransferenciaOrigem.IdTransferencia);
            contextQuery.Parameters.Add("@CENSO", tceTransferenciaOrigem.Censo);
            contextQuery.Parameters.Add("@CURSO", tceTransferenciaOrigem.Curso);
            contextQuery.Parameters.Add("@SERIE", tceTransferenciaOrigem.Serie);
            contextQuery.Parameters.Add("@TURNO", tceTransferenciaOrigem.Turno);
            contextQuery.Parameters.Add("@CURRICULO", tceTransferenciaOrigem.Curriculo);
            contextQuery.Parameters.Add("@UNIDADE_FISICA", tceTransferenciaOrigem.UnidadeFisica);

            if (ativo)
            {
                contextQuery.Parameters.Add("@ANO", tceTransferenciaOrigem.Ano);
                contextQuery.Parameters.Add("@PERIODO", tceTransferenciaOrigem.Periodo);
                contextQuery.Parameters.Add("@TURMA", tceTransferenciaOrigem.Turma);
            }

            context.ApplyModifications(contextQuery);
        }

        public static bool ExistePendenciaTranf(string aluno)
        {
            var sql = @"SELECT  count(*)
                    FROM    dbo.TCE_TRANSFERENCIA
                    WHERE   STATUS = ?
                            AND ALUNO = ?";

            var retorno = ExecutarFuncao(sql, Pendente, aluno);

            return retorno != 0;
        }

        public static ValidacaoDados ValidarRemover(int idTransferencia)
        {
            var validacaoDados = new ValidacaoDados
                                 {
                                     Valido = false
                                 };

            if (idTransferencia == 0)
            {
                return validacaoDados;
            }

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT  1
                    FROM    dbo.TCE_TRANSFERENCIA
                    WHERE   STATUS = @STATUS
                            AND ID_TRANSFERENCIA = @ID_TRANSFERENCIA");

                contextQuery.Parameters.Add("@ID_TRANSFERENCIA", idTransferencia);
                contextQuery.Parameters.Add("@STATUS", Pendente);

                var obj = ctx.GetReturnValue(contextQuery);

                if (obj == null)
                {
                    validacaoDados.Mensagem = "Não é permitido realizar a exclusão de uma solicitação que ja foi aceita ou recusada pela Unidade de Origem.";
                }
            }

            if (string.IsNullOrEmpty(validacaoDados.Mensagem))
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public static void Remover(int idTransferencia)
        {
            if (idTransferencia < 1)
            {
                return;
            }

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery(
                        @"DELETE FROM TCE_TRANSFERENCIA_DESTINO WHERE ID_TRANSFERENCIA = @ID_TRANSFERENCIA
                        DELETE FROM TCE_TRANSFERENCIA_ORIGEM WHERE ID_TRANSFERENCIA = @ID_TRANSFERENCIA                                            
                        DELETE FROM TCE_TRANSFERENCIA WHERE ID_TRANSFERENCIA = @ID_TRANSFERENCIA");

                    contextQuery.Parameters.Add("@ID_TRANSFERENCIA", idTransferencia);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }
    }
}