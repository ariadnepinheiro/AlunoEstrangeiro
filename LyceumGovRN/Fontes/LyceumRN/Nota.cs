namespace Techne.Lyceum.RN
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Seeduc.Infra.Data;
    using Techne.Data;
    using Techne.Library;
    using Techne.Lyceum.CR;
    using Techne.Lyceum.RN.Entidades;
    using System.Data;

    public class Nota : RNBase
    {
        public static RetValue Atualizar(Ly_nota.Row[] rows)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                //Remoção de todos as notas presentes no Array de Ly_nota.Row                
                RetValue retRemocao = Remover(connection, rows);
                if (retRemocao != null && !retRemocao.Ok)
                {
                    connection.Rollback();
                    return retRemocao;
                }

                //Inserção das notas cujo conceito foi informado (não vazio, não nulo)
                var notasInsercao = rows.Where(r => !String.IsNullOrEmpty(r.Conceito.Trim()));
                foreach (Ly_nota.Row row in notasInsercao)
                {
                    Ly_prova.Row provaRow = RN.ProvaTurma.ConsultarProva(row.Prova, row.Disciplina, row.Turma, row.Ano.Value, row.Semestre.Value);
                    if (String.IsNullOrEmpty(provaRow.Formula))
                    {
                        TCommand.ExecuteNonQuery(connection,
                            @"INSERT INTO Ly_nota(Aluno, Disciplina, Turma, Ano, Semestre, Prova, compareceu, conceito, data, ordem)
                            VALUES(?,?,?,?,?,?,?,?,?,?)", row.Aluno, row.Disciplina, row.Turma, row.Ano, row.Semestre,
                            row.Prova, row.Compareceu, row.Conceito.Replace(",", "."), row.Data, row.Ordem);
                        RetValue ret = VerificarErro(connection.GetErrors());
                        if (ret != null && !ret.Ok)
                        {
                            connection.Rollback();
                            return ret;
                        }
                    }
                }

                //Processamento das notas
                var notasProvas = from n in rows
                                  group n by new { n.Prova, n.Disciplina, n.Turma, n.Ano, n.Semestre } into g
                                  select g;
                foreach (var notasProva in notasProvas)
                {
                    String prova = notasProva.Key.Prova;
                    String disciplina = notasProva.Key.Disciplina;
                    String turma = notasProva.Key.Turma;
                    decimal? ano = notasProva.Key.Ano;
                    decimal? semestre = notasProva.Key.Semestre;

                    Ly_prova.Row provaRow = RN.ProvaTurma.ConsultarProva(prova, disciplina, turma, ano.Value, semestre.Value);
                    String formula = provaRow.Formula;

                    if (!String.IsNullOrEmpty(provaRow.Formula))
                    {
                        var notasAlunos = from n in notasProva select n;

                        foreach (var notaAluno in notasAlunos)
                        {
                            String aluno = notaAluno.Aluno;
                            String compareceu = notaAluno.Compareceu;
                            DateTime? data = notaAluno.Data;
                            decimal? ordem = notaAluno.Ordem;
                            String conceito;
                            RetValue ret = ProcessarNota(connection, formula, aluno, disciplina, turma, ano.Value, semestre.Value, out conceito);

                            if (ret == null || ret.Ok)
                            {
                                if (conceito.Length > 15)
                                    conceito = conceito.Substring(0, 15);

                                Ly_nota.Row.Insert(connection, aluno, disciplina, turma, ano.Value, semestre.Value, prova, notaAluno.Recuperacao_paralela, notaAluno.Sem_avaliacao, notaAluno.Justificativa, null, null, null,
                                    "compareceu, conceito, data, ordem",
                                    compareceu, conceito.Replace(",", "."), data, ordem);
                                RetValue retInsertProcessada = VerificarErro(connection.GetErrors());
                            }
                        }
                    }
                }
            }
            catch
            {
                connection.Rollback();
                return VerificarErro(connection.GetErrors());
            }
            finally
            {
                connection.Close();
            }

            return null;
        }

        public static RetValue ProcessarNota(TConnectionWritable connection, String formula, String aluno, String disciplina,
            String turma, decimal ano, decimal semestre, out String conceito)
        {
            try
            {
                return Formula.CalculaConceitoProvaAluno(connection, formula, disciplina, turma, (int?)ano, (int?)semestre, aluno, out conceito);
            }
            catch (Exception e)
            {
                conceito = null;
                return new RetValue(false, "", new ErrorList(e.Message));
            }
        }

        public static RetValue Remover(TConnectionWritable connection, Ly_nota.Row[] notas)
        {
            foreach (Ly_nota.Row nota in notas)
            {
                bool existia = Ly_nota.Row.Delete(connection, nota.Aluno, nota.Disciplina, nota.Turma, nota.Ano, nota.Semestre, nota.Prova);
                RetValue ret = VerificarErro(connection.GetErrors());
                if (existia)
                {
                    if (ret != null && !ret.Ok)
                        return ret;
                }
            }
            connection.ClearErrors();
            return null;
        }

        public static RetValue AtualizarNotas(Dictionary<String, String> alunosConceitos, String disciplina, String turma, decimal ano, decimal semestre, String prova)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                //Remoção dos conceitos referentes à disciplina/turma/ano/semestre/prova
                //dos alunos com situação de matrícula = 'Matriculado'
                RetValue retRemocao = RemoverNotasDeAlunosMatriculados(connection, disciplina, turma, ano, semestre, prova);
                if (retRemocao != null && !retRemocao.Ok)
                {
                    connection.Rollback();
                    return retRemocao;
                }

                //Recupera a prova
                Ly_prova.Row provaRow = RN.ProvaTurma.ConsultarProva(prova, disciplina, turma, ano, semestre);
                if (provaRow == null)
                    return new RetValue(false, "", new ErrorList("Instrumento não cadastrado: \"" + prova + "\""));
                else if (!String.IsNullOrEmpty(provaRow.Formula))
                    return new RetValue(false, "", new ErrorList("O Instrumento contém fórmula. Não é permitida a digitação manual das notas."));

                //Seleciona as notas cujo conceito foi informado
                var acsInsercao = alunosConceitos
                    .Select(ac => new { Aluno = ac.Key, Conceito = ac.Value })
                    .Where(ac => !String.IsNullOrEmpty(ac.Conceito));

                //Inserção das notas selecionadas (quando o conceito foi informado)
                Ly_nota dt = new Ly_nota();
                foreach (var acInsercao in acsInsercao)
                {
                    Ly_nota.Row nota = dt.NewRow();
                    nota.Aluno = acInsercao.Aluno;
                    nota.Ano = ano;
                    nota.Compareceu = "S";
                    nota.Conceito = acInsercao.Conceito == "SN" ? null : acInsercao.Conceito;
                    nota.Data = DateTime.Today;
                    nota.Disciplina = disciplina;
                    nota.Ordem = provaRow.Ordem;
                    nota.Prova = prova;
                    nota.Semestre = semestre;
                    nota.Turma = turma;

                    RetValue ret = InserirNotaDeAluno(connection, nota);
                    if (ret != null && !ret.Ok)
                    {
                        connection.Rollback();
                        return ret;
                    }
                }
                return null;
            }
            catch
            {
                connection.Rollback();
                return VerificarErro(connection.GetErrors());
            }
            finally
            {
                connection.Close();
            }
        }

        private static IDictionary<string, LyFalta> ListarFaltasSalvas(DataContext dataContext, int ano, int periodo, string turma, string disciplina, string frequencia)
        {
            var contextQuery = new ContextQuery
                               {
                                   Command = @"SELECT  ALUNO,
                                                       FALTAS
                                               FROM    LY_FALTA (NOLOCK)
                                               WHERE   FREQ = @FREQUENCIA
                                                       AND ANO = @ANO
                                                       AND PERIODO = @PERIODO
                                                       AND TURMA = @TURMA
                                                       AND DISCIPLINA = @DISCIPLINA"
                               };

            contextQuery.Parameters.Add("@FREQUENCIA", frequencia);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@DISCIPLINA", disciplina);

            var faltas = dataContext.TryToBindEntities<LyFalta>(contextQuery);

            if (faltas == null)
            {
                return new Dictionary<string, LyFalta>();
            }

            return faltas.ToDictionary(x => x.Aluno, x => x);
        }

        private static IDictionary<string, LyNota> ListarNotasSalvas(DataContext dataContext, int ano, int periodo, string turma, string disciplina, string prova)
        {
            var contextQuery = new ContextQuery
                               {
                                   Command = @"SELECT  ALUNO ,
                                                        CONCEITO ,
                                                        NOTAPROVA ,
                                                        NOTARECUPERACAO ,
                                                        RECUPERACAO_PARALELA ,
                                                        SEM_AVALIACAO ,
                                                        JUSTIFICATIVA ,
                                                        MOTIVOSEMNOTAID
                                               FROM    LY_NOTA (NOLOCK)
                                               WHERE   PROVA = @PROVA
                                                       AND ANO = @ANO
                                                       AND SEMESTRE = @PERIODO
                                                       AND TURMA = @TURMA
                                                       AND DISCIPLINA = @DISCIPLINA"
                               };

            contextQuery.Parameters.Add("@PROVA", prova);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@DISCIPLINA", disciplina);

            var notas = dataContext.TryToBindEntities<LyNota>(contextQuery);

            if (notas == null)
            {
                return new Dictionary<string, LyNota>();
            }

            return notas.ToDictionary(x => x.Aluno, x => x);
        }

        public void RemovePor(string aluno, string turma, string disciplina, int? ano, int? periodo, string prova, List<ContextQuery> listaContextQuery)
        {
            LyNota nota = new LyNota();
            nota.Aluno = aluno;
            nota.Turma = turma;
            nota.Disciplina = disciplina;
            nota.Ano = ano;
            nota.Semestre = periodo;
            nota.Prova = prova;

            listaContextQuery.Add(Remover(nota));
        }

        private static ContextQuery Remover(LyNota nota)
        {
            var contextQuery = new ContextQuery{
                Command = @"DELETE  LY_NOTA
                            WHERE   ALUNO = @ALUNO
                                    AND DISCIPLINA = @DISCIPLINA
                                    AND TURMA = @TURMA
                                    AND ANO = @ANO
                                    AND SEMESTRE = @SEMESTRE
                                    AND PROVA = @PROVA"
            };

            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, nota.Aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", TechneDbType.T_CODIGO, nota.Disciplina);
            contextQuery.Parameters.Add("@TURMA", nota.Turma);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, nota.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, nota.Semestre);
            contextQuery.Parameters.Add("@PROVA", TechneDbType.T_PROVA10, nota.Prova);

            return contextQuery;
        }

        public void RemovePorMatriculaParaFechamento(DataContext ctx, string aluno, string disciplina, string turma, decimal ano, decimal semestre)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" DELETE  LY_NOTA
                                          WHERE   ALUNO = @ALUNO
                                                  AND DISCIPLINA = @DISCIPLINA
                                                  AND TURMA = @TURMA
                                                  AND ANO = @ANO
                                                  AND SEMESTRE = @SEMESTRE ";

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);

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

        public void Inserir(LyNota nota, List<ContextQuery> listaContextQuery)
        {
            ContextQuery contextQuery = Inserir(nota);
            listaContextQuery.Add(contextQuery);
        }

        public void Inserir(DataContext contexto, LyNota nota)
        {
            contexto.ApplyModifications(Inserir(nota));
        }

        private ContextQuery Inserir(LyNota nota)
        {
            var contextQuery = new ContextQuery
            {
                Command = @" 

                        SELECT @NOTAATIVA = COUNT(*) 
                        FROM   LY_NOTA M 
                        WHERE  ALUNO = @ALUNO 
                                AND ANO = @ANO 
                                AND SEMESTRE = @SEMESTRE 
                                AND DISCIPLINA = @DISCIPLINA 
                                AND TURMA = @TURMA 
		                        AND PROVA = @PROVA

                        IF ( @NOTAATIVA = 0 ) 
                            BEGIN 
                                INSERT INTO LY_NOTA
                                     (
                                       ALUNO,
                                       DISCIPLINA,
                                       TURMA,
                                       ANO,
                                       SEMESTRE,
                                       PROVA,
                                       COMPARECEU,
                                       CONCEITO,
                                       DATA,
                                       ORDEM,
                                       FORMULARIO,
                                       RECUPERACAO_PARALELA,
                                       SEM_AVALIACAO,
                                       JUSTIFICATIVA,
                                       NOTAPROVA,
                                       NOTARECUPERACAO,
                                       MOTIVOSEMNOTAID
                            ) VALUES (
                                       @ALUNO,
                                       @DISCIPLINA,
                                       @TURMA,
                                       @ANO,
                                       @SEMESTRE,
                                       @PROVA,
                                       @COMPARECEU,
                                       @CONCEITO,
                                       @DATA,
                                       @ORDEM,
                                       @FORMULARIO,
                                       @RECUPERACAO_PARALELA,
                                       @SEM_AVALIACAO,
                                       @JUSTIFICATIVA,
                                       @NOTAPROVA,
                                       @NOTARECUPERACAO,
                                       @MOTIVOSEMNOTAID
                                )
	                        END 
                        ELSE 
                            BEGIN 
                               UPDATE  LY_NOTA
                                SET     COMPARECEU = @COMPARECEU,
                                        CONCEITO = @CONCEITO,
                                        DATA = @DATA,
                                        ORDEM = @ORDEM,
                                        FORMULARIO = @FORMULARIO,
                                        RECUPERACAO_PARALELA = @RECUPERACAO_PARALELA,
                                        SEM_AVALIACAO = @SEM_AVALIACAO,
                                        JUSTIFICATIVA = @JUSTIFICATIVA,
                                        NOTAPROVA = @NOTAPROVA,
                                        NOTARECUPERACAO = @NOTARECUPERACAO,
                                        MOTIVOSEMNOTAID = @MOTIVOSEMNOTAID
                                WHERE   ALUNO = @ALUNO
                                        AND DISCIPLINA = @DISCIPLINA
                                        AND TURMA = @TURMA
                                        AND ANO = @ANO
                                        AND SEMESTRE = @SEMESTRE
                                        AND PROVA = @PROVA
                            END "
            };
            
            contextQuery.Parameters.Add("@NOTAATIVA", SqlDbType.Int, 0);
            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, nota.Aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", TechneDbType.T_CODIGO, nota.Disciplina);
            contextQuery.Parameters.Add("@TURMA", nota.Turma);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, nota.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, nota.Semestre);
            contextQuery.Parameters.Add("@PROVA", TechneDbType.T_PROVA10, nota.Prova);
            contextQuery.Parameters.Add("@COMPARECEU", TechneDbType.T_SIMNAO, nota.Compareceu);
            contextQuery.Parameters.Add("@CONCEITO", TechneDbType.T_ALFASMALL, nota.Conceito);
            contextQuery.Parameters.Add("@DATA", TechneDbType.T_DATA, nota.Data);
            contextQuery.Parameters.Add("@ORDEM", TechneDbType.T_NUMERO_PEQUENO, nota.Ordem);
            contextQuery.Parameters.Add("@FORMULARIO", TechneDbType.T_NUMERO_GRANDE, 1);
            contextQuery.Parameters.Add("@RECUPERACAO_PARALELA", nota.RecuperacaoParalela);
            contextQuery.Parameters.Add("@SEM_AVALIACAO", nota.SemAvaliacao);
            contextQuery.Parameters.Add("@JUSTIFICATIVA", nota.Justificativa);
            contextQuery.Parameters.Add("@NOTAPROVA", nota.NotaProva);
            contextQuery.Parameters.Add("@NOTARECUPERACAO", nota.NotaRecuperacao);
            contextQuery.Parameters.Add("@MOTIVOSEMNOTAID", nota.MotivoSemNotaId);

            return contextQuery;
        }

        public void Atualizar(LyNota nota, List<ContextQuery> listaContextQuery)
        {
            ContextQuery contextQuery = Atualizar(nota);
            listaContextQuery.Add(contextQuery);
        }

        public void Atualizar(DataContext contexto, LyNota nota)
        {
            contexto.ApplyModifications(Atualizar(nota));
        }

        private ContextQuery Atualizar(LyNota nota)
        {
            var contextQuery = new ContextQuery
            {
                Command = @"UPDATE  LY_NOTA
                            SET     COMPARECEU = @COMPARECEU,
                                    CONCEITO = @CONCEITO,
                                    DATA = @DATA,
                                    ORDEM = @ORDEM,
                                    FORMULARIO = @FORMULARIO,
                                    RECUPERACAO_PARALELA = @RECUPERACAO_PARALELA,
                                    SEM_AVALIACAO = @SEM_AVALIACAO,
                                    JUSTIFICATIVA = @JUSTIFICATIVA,
                                    NOTAPROVA = @NOTAPROVA,
                                    NOTARECUPERACAO = @NOTARECUPERACAO,
                                    MOTIVOSEMNOTAID = @MOTIVOSEMNOTAID
                            WHERE   ALUNO = @ALUNO
                                    AND DISCIPLINA = @DISCIPLINA
                                    AND TURMA = @TURMA
                                    AND ANO = @ANO
                                    AND SEMESTRE = @SEMESTRE
                                    AND PROVA = @PROVA"
            };

            contextQuery.Parameters.Add("@COMPARECEU", TechneDbType.T_SIMNAO, nota.Compareceu);
            contextQuery.Parameters.Add("@CONCEITO", TechneDbType.T_ALFASMALL, nota.Conceito);
            contextQuery.Parameters.Add("@DATA", TechneDbType.T_DATA, nota.Data);
            contextQuery.Parameters.Add("@ORDEM", TechneDbType.T_NUMERO_PEQUENO, nota.Ordem);
            contextQuery.Parameters.Add("@FORMULARIO", TechneDbType.T_NUMERO_GRANDE, 1);
            contextQuery.Parameters.Add("@RECUPERACAO_PARALELA", nota.RecuperacaoParalela);
            contextQuery.Parameters.Add("@SEM_AVALIACAO", nota.SemAvaliacao);
            contextQuery.Parameters.Add("@JUSTIFICATIVA", nota.Justificativa);
            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, nota.Aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", TechneDbType.T_CODIGO, nota.Disciplina);
            contextQuery.Parameters.Add("@TURMA", nota.Turma);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, nota.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, nota.Semestre);
            contextQuery.Parameters.Add("@PROVA", TechneDbType.T_PROVA10, nota.Prova);
            contextQuery.Parameters.Add("@NOTAPROVA", nota.NotaProva);
            contextQuery.Parameters.Add("@NOTARECUPERACAO", nota.NotaRecuperacao);
            contextQuery.Parameters.Add("@MOTIVOSEMNOTAID", nota.MotivoSemNotaId);

            return contextQuery;
        }

        private static ContextQuery AtualizarLancamento(int ano, int periodo, string turma, string disciplina, string prova)
        {
            var contextQuery = new ContextQuery
                               {
                                   Command = @"UPDATE  PROVA
                                                       SET     PROVA.COMPLEMENTO = PROVA.LANCAMENTO_COMPLETO
                                                       FROM    (
                                                                SELECT P.COMPLEMENTO,
                                                                       (
                                                                        SELECT CASE WHEN EXISTS (  -- QUANDO EXISTE MATRÍCULA SEM NOTA, LANCAMENTO_COMPLETO = 'N'
                                                                                         SELECT TOP 1
                                                                                                   1
                                                                                         FROM      LY_MATRICULA M (NOLOCK)
                                                                                         WHERE     M.DISCIPLINA = P.DISCIPLINA
                                                                                                   AND M.TURMA = P.TURMA
                                                                                                   AND M.ANO = P.ANO
                                                                                                   AND M.SEMESTRE = P.SEMESTRE
                                                                                                   AND M.SIT_MATRICULA = 'Matriculado'
                                                                                                   AND NOT EXISTS ( SELECT TOP 1
                                                                                                                           1
                                                                                                                    FROM   LY_NOTA N (NOLOCK)
                                                                                                                    WHERE  N.DISCIPLINA = M.DISCIPLINA
                                                                                                                           AND N.TURMA = M.TURMA
                                                                                                                           AND N.ANO = M.ANO
                                                                                                                           AND N.SEMESTRE = M.SEMESTRE
                                                                                                                           AND N.PROVA = P.PROVA
                                                                                                                           AND N.ALUNO = M.ALUNO ) ) THEN 'N'
                                                                                    ELSE 'S'
                                                                               END
                                                                       ) LANCAMENTO_COMPLETO
                                                                FROM   LY_PROVA P (NOLOCK)
                                                                WHERE  P.DISCIPLINA = @DISCIPLINA
                                                                       AND P.TURMA = @TURMA
                                                                       AND P.ANO = @ANO
                                                                       AND P.SEMESTRE = @PERIODO
                                                                       AND P.PROVA = @PROVA
                                                               ) PROVA"
                               };

            contextQuery.Parameters.Add("@DISCIPLINA", TechneDbType.T_CODIGO, disciplina);
            contextQuery.Parameters.Add("@TURMA",turma);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
            contextQuery.Parameters.Add("@PERIODO", TechneDbType.T_SEMESTRE2, periodo);
            contextQuery.Parameters.Add("@PROVA", TechneDbType.T_PROVA10, prova);

            return contextQuery;
        }

        public static RetValue AtualizarNotas(ICollection<LyNota> notas, string usuario,
            ICollection<LyFalta> faltas, string aulasDadas, string aulasPrevistas,
            decimal subPeriodo, TceProtocoloNota protocolo, IList<ContextQuery> contextQueries,
            DataContext ctx)
        {
            //using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            //{
            if (notas == null || notas.Count == 0 || notas.Select(n => new { n.Prova, n.Turma, n.Ano, n.Semestre, n.Disciplina }).Distinct().Count() != 1)
            {
                return new RetValue(false, "Necessário enviar as notas para atualizar as notas!", null);
            }

            //if (faltas == null || faltas.Count == 0 || faltas.Select(n => new { n.Freq, n.Turma, n.Ano, n.Periodo, n.Disciplina }).Distinct().Count() != 1)
            //{
            //    return new RetValue(false, "Necessário enviar as faltas para atualizar as faltas!", null);
            //}

            //if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(aulasDadas) || string.IsNullOrEmpty(aulasPrevistas) || protocolo == null)
            //{
            //    return new RetValue(false, "Necessário enviar os dados restantes para atualizar as notas!", null);
            //}

            if (string.IsNullOrEmpty(usuario) || protocolo == null)
            {
                return new RetValue(false, "Necessário enviar os dados restantes para atualizar as notas!", null);
            }

            //try
            //{
            var primeiraNota = notas.First();
            var disciplina = primeiraNota.Disciplina;
            var turma = primeiraNota.Turma;
            var ano = Convert.ToInt32(primeiraNota.Ano);
            var periodo = Convert.ToInt32(primeiraNota.Semestre);
            var prova = primeiraNota.Prova;
            var notasSalvas = ListarNotasSalvas(ctx, ano, periodo, turma, disciplina, prova);

            //string frequencia = string.Empty;
            //LyFalta primeiraFalta = null;
            //IDictionary<string, LyFalta> faltasSalvas = null;

            //if (faltas.Count > 0) 
            //{
            //    primeiraFalta = faltas.First();
            //    frequencia = primeiraFalta.Freq;
            //    faltasSalvas = ListarFaltasSalvas(ctx, ano, periodo, turma, disciplina, frequencia);
            //}

            //var contextQueries = new List<ContextQuery> ();
            RN.DeclaracaoSemNota rnDeclaracaoSemNota = new RN.DeclaracaoSemNota();
            Entidades.DeclaracaoSemNota declaracaoSemNota = new Entidades.DeclaracaoSemNota();

            //ctx.BeginBulkModifications();

            //contextQueries.Add(Frequencia.AtualizarAulas(aulasDadas, aulasPrevistas, disciplina, turma, ano, periodo, frequencia));

            // Atualização das notas
            foreach (var nota in notas)
            {
                nota.Compareceu = "S";
                nota.Data = DateTime.Today;

                if (!string.IsNullOrEmpty(nota.Conceito))
                {
                    decimal conceito;

                    if (!decimal.TryParse(nota.Conceito, out conceito))
                    {
                        var nomeAluno = ConsultarCampo("SELECT nome_compl FROM ly_aluno (NOLOCK) WHERE aluno = ?", nota.Aluno);

                        throw new Exception("Existem valores de nota inválidos.<br/> - Aluno: " + nomeAluno + "<br/> - Nota: " + nota.Conceito);
                    }

                    nota.Conceito = conceito.ToString().Replace(".", ",");
                }

                if (notasSalvas.ContainsKey(nota.Aluno))
                {
                    var notaSalva = notasSalvas[nota.Aluno];

                    if (nota.SemAvaliacao == "N" && string.IsNullOrEmpty(nota.Conceito))
                    {
                        contextQueries.Add(LogNota.Inserir(nota, usuario, notaSalva.Conceito, notaSalva.RecuperacaoParalela, notaSalva.SemAvaliacao, notaSalva.Justificativa, 3));

                        contextQueries.Add(rnDeclaracaoSemNota.RemoveComLancamentoNotas(nota));

                        contextQueries.Add(Remover(nota));
                    }
                    else
                    {
                        if (notaSalva.Conceito != nota.Conceito
                            || notaSalva.RecuperacaoParalela != nota.RecuperacaoParalela
                            || notaSalva.SemAvaliacao != nota.SemAvaliacao
                            || notaSalva.Justificativa != nota.Justificativa
                            || notaSalva.NotaProva != nota.NotaProva
                            || notaSalva.NotaRecuperacao != nota.NotaRecuperacao
                            || notaSalva.MotivoSemNotaId != nota.MotivoSemNotaId)
                        {
                            contextQueries.Add(LogNota.Inserir(nota, usuario, notaSalva.Conceito, notaSalva.RecuperacaoParalela, notaSalva.SemAvaliacao, notaSalva.Justificativa, 2));

                            contextQueries.Add(new Nota().Atualizar(nota));

                            if ((notaSalva.SemAvaliacao != nota.SemAvaliacao) && (nota.SemAvaliacao == "N"))
                            {
                                contextQueries.Add(rnDeclaracaoSemNota.RemoveComLancamentoNotasDoUsuario(nota, usuario));
                            }

                            //Se o motiva para 
                            if (notaSalva.MotivoSemNotaId != nota.MotivoSemNotaId)
                            {
                                contextQueries.Add(rnDeclaracaoSemNota.RemoveComLancamentoNotasDoUsuario(nota, usuario));

                                //Caso o campo sem avaliação esteja marcado, atualiza tabela DECLARACAOSEMNOTA
                                if (nota.SemAvaliacao == "S")
                                {
                                    declaracaoSemNota.Matricula = usuario;

                                    if (nota.MotivoSemNotaId == 1) //1 - Afastamento Médico / Maternidade / Serviço Militar (Avaliação Residencial)
                                    {
                                        declaracaoSemNota.TipoDeclaracaoSemNotaId = 1;
                                    }

                                    if (nota.MotivoSemNotaId == 0) //0 - Aluno em Progressão Parcial (Dependência) não apresentou trabalho
                                    {
                                        declaracaoSemNota.TipoDeclaracaoSemNotaId = 0;
                                    }

                                    if (nota.MotivoSemNotaId == 2) //2 - Outros
                                    {
                                        declaracaoSemNota.TipoDeclaracaoSemNotaId = 2;
                                    }

                                    contextQueries.Add(rnDeclaracaoSemNota.InsereComLancamentoNotas(declaracaoSemNota, nota));
                                }
                            }
                        }
                    }
                }
                else
                {
                    contextQueries.Add(LogNota.Inserir(nota, usuario, string.Empty, string.Empty, string.Empty, string.Empty, 1));

                    contextQueries.Add(new Nota().Inserir(nota));

                    //Caso o campo sem avaliação esteja marcado, atualiza tabela DECLARACAOSEMNOTA
                    if (nota.SemAvaliacao == "S")
                    {
                        declaracaoSemNota.Matricula = usuario;

                        if (nota.MotivoSemNotaId == 1) //1 - Afastamento Médico / Maternidade / Serviço Militar (Avaliação Residencial)
                        {
                            declaracaoSemNota.TipoDeclaracaoSemNotaId = 1;
                        }

                        if (nota.MotivoSemNotaId == 0) //0 - Aluno em Progressão Parcial (Dependência) não apresentou trabalho
                        {
                            declaracaoSemNota.TipoDeclaracaoSemNotaId = 0;
                        }

                        if (nota.MotivoSemNotaId == 2) //2 - Outros
                        {
                            declaracaoSemNota.TipoDeclaracaoSemNotaId = 2;
                        }

                        contextQueries.Add(rnDeclaracaoSemNota.InsereComLancamentoNotas(declaracaoSemNota, nota));
                    }
                }
            }

            // Atualiza flag de lançamento completo do quadro
            contextQueries.Add(AtualizarLancamento(ano, periodo, turma, disciplina, prova));

            // Atualização das aulas dadas e previstas
            //contextQueries.Add(Frequencia.AtualizarAulas(aulasDadas, aulasPrevistas, disciplina, turma, ano, periodo, frequencia));

            //foreach (var falta in faltas)
            //{
            //    var numeroFaltas = falta.Faltas.HasValue ? Convert.ToInt32(falta.Faltas.Value) : -1;

            //    if (faltasSalvas.ContainsKey(falta.Aluno))
            //    {
            //        var faltaSalva = faltasSalvas[falta.Aluno];

            //        if (numeroFaltas == -1)
            //        {
            //            var nomeAluno = ConsultarCampo("SELECT nome_compl FROM ly_aluno (NOLOCK) WHERE aluno = ?", falta.Aluno);

            //            return new RetValue(false, string.Empty, new ErrorList("Não é permitido remoção das faltas do aluno " + nomeAluno + "."));
            //        }

            //        if (faltaSalva.Faltas != numeroFaltas)
            //        {
            //            contextQueries.Add(Falta.Atualizar(numeroFaltas, falta.Aluno, disciplina, turma, ano, periodo, frequencia));
            //        }
            //    }
            //    else
            //    {
            //        if (numeroFaltas != -1)
            //        {
            //            contextQueries.Add(Falta.Inserir(numeroFaltas, falta.Aluno, disciplina, turma, ano, periodo, frequencia));
            //        }
            //    }
            //}

            //// Executar todas as alterações
            //foreach (var contextQuery in contextQueries)
            //{
            //    ctx.ApplyModifications(contextQuery);
            //}

            //ctx.EndBulkModifications();

            //// Gerar Protocolo
            //ProtocoloNota.Inserir(ctx, protocolo);

            //// Verificação de notas lançadas
            //if (ExistePendenciaNotasBimestresAnteriores(ctx, primeiraNota.Disciplina, primeiraNota.Turma, primeiraNota.Ano, primeiraNota.Semestre, subPeriodo))
            //{
            //    return new RetValue(true, "Notas atualizadas com sucesso.", null);
            //}

            return null;
            //}
            //catch (Exception e)
            //{
            //    ctx.Abandon();

            //    return new RetValue(false, string.Empty, new ErrorList("Erro durante atualização de notas: " + e.Message));
            //}
            // }
        }

        public static RetValue AtualizarFrequencias(string usuario, ICollection<LyFalta> faltas, string aulasDadas,
            string aulasPrevistas, string disciplina, string turma, int ano, int periodo, decimal subPeriodo,
            TceProtocoloNota protocolo, IList<ContextQuery> contextQueries, DataContext ctx)
        {
            //using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            //{
            if (faltas == null || faltas.Count == 0 || faltas.Select(n => new { n.Freq, n.Turma, n.Ano, n.Periodo, n.Disciplina }).Distinct().Count() != 1)
            {
                return new RetValue(false, "Necessário enviar as faltas para atualizar as faltas!", null);
            }

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(aulasDadas) || string.IsNullOrEmpty(aulasPrevistas) || protocolo == null)
            {
                return new RetValue(false, "Necessário enviar os dados restantes para atualizar as notas!", null);
            }

            //try
            //{
            var primeiraFalta = faltas.First();
            var frequencia = primeiraFalta.Freq;
            var faltasSalvas = ListarFaltasSalvas(ctx, ano, periodo, turma, disciplina, frequencia);
            //var contextQueries = new List<ContextQuery>();
            RN.DeclaracaoSemNota rnDeclaracaoSemNota = new RN.DeclaracaoSemNota();
            Entidades.DeclaracaoSemNota declaracaoSemNota = new Entidades.DeclaracaoSemNota();

            //ctx.BeginBulkModifications();

            // Atualização das aulas dadas e previstas
            contextQueries.Add(Frequencia.AtualizarAulas(aulasDadas, aulasPrevistas, disciplina, turma, ano, periodo, frequencia));

            foreach (var falta in faltas)
            {
                var numeroFaltas = falta.Faltas.HasValue ? Convert.ToInt32(falta.Faltas.Value) : -1;

                if (faltasSalvas.ContainsKey(falta.Aluno))
                {
                    var faltaSalva = faltasSalvas[falta.Aluno];

                    if (numeroFaltas == -1)
                    {
                        var nomeAluno = ConsultarCampo("SELECT nome_compl FROM ly_aluno (NOLOCK) WHERE aluno = ?", falta.Aluno);

                        return new RetValue(false, string.Empty, new ErrorList("Não é permitido remoção das faltas do aluno " + nomeAluno + "."));
                    }

                    if (faltaSalva.Faltas != numeroFaltas)
                    {
                        contextQueries.Add(Falta.Atualizar(numeroFaltas, falta.Aluno, disciplina, turma, ano, periodo, frequencia));
                    }
                }
                else
                {
                    if (numeroFaltas != -1)
                    {
                        contextQueries.Add(Falta.Inserir(numeroFaltas, falta.Aluno, disciplina, turma, ano, periodo, frequencia));
                    }
                }
            }

            // Executar todas as alterações
            //foreach (var contextQuery in contextQueries)
            //{
            //    ctx.ApplyModifications(contextQuery);
            //}

            //ctx.EndBulkModifications();

            // Gerar Protocolo
            //ProtocoloNota.Inserir(ctx, protocolo);

            // Verificação de notas lançadas
            //if (ExistePendenciaNotasBimestresAnteriores(ctx, primeiraFalta.Disciplina, primeiraFalta.Turma, primeiraFalta.Ano, primeiraFalta.Periodo, subPeriodo))
            //{
            //    return new RetValue(true, "Notas atualizadas com sucesso.", null);
            //}

            return null;
            //}
            //catch (Exception e)
            //{
            //    ctx.Abandon();

            //    return new RetValue(false, string.Empty, new ErrorList("Erro durante atualização de notas: " + e.Message));
            //}
            //}
        }

        /// <summary>
        /// Atualiza as notas do histórico.
        /// </summary>
        /// <param name="notas">Notas a serem atualizadas.</param>
        /// <param name="faltas">Faltas a serem atualizadas.</param>
        /// <returns>Resultado da atualização.</returns>
        public static RetValue AtualizarNotas(Ly_nota_histmatr notas, Ly_falta_histmatr faltas)
        {
            //Valida se existe "SN" na coleção de notas            
            if (notas.Rows.Cast<Ly_nota_histmatr.Row>().Any(n => n.Conceito.Trim().ToUpper() == "SN"))
                return new RetValue(false, string.Empty, new ErrorList("Não é permitido lançar nota SN."));

            //Valida se existe faltas vazia
            if (faltas.Rows.Cast<Ly_falta_histmatr.Row>().Any(f => string.IsNullOrEmpty(Convert.ToString(f.Faltas))))
                return new RetValue(false, string.Empty, new ErrorList("Não é permitido lançar falta vazia."));


            ErrorList err = new ErrorList();

            //Atualiza as notas no histórico de notas. Insere, se não existe, atualiza, se já existe
            for (int iNota = 0; iNota < notas.Rows.Count; iNota++)
            {
                RetValue retNota = AtualizarNota(notas.Rows[iNota]);
                if (retNota != null && !retNota.Ok)
                    err.Add(String.Format("Erro ao lançar nota da prova {0} do aluno {1}.", notas.Rows[iNota].Nota_id, notas.Rows[iNota].Aluno), "ERRO");
            }

            //Atualiza as faltas no histórico de faltas. Insere, se não existe, atualiza, se já existe
            for (int iFalta = 0; iFalta < faltas.Rows.Count; iFalta++)
            {
                RetValue retFalta = AtualizarFalta(faltas.Rows[iFalta]);
                if (retFalta != null && !retFalta.Ok)
                    err.Add(String.Format("Erro ao lançar faltas {0} do aluno {1}.", faltas.Rows[iFalta].Freq_id, faltas.Rows[iFalta].Aluno, "ERRO"));
            }

            if (err.Count > 0)
                return new RetValue(false, "Alguns erros ocorreram durante o lançamento de notas e faltas da turma:", err);
            else
                return new RetValue(true, "Lançamento realizado com sucesso.", null);
        }

        public static RetValue AtualizarNota(Ly_nota_histmatr.Row nota)
        {
            return IAE(@"
                DECLARE @ALUNO VARCHAR(20) = ?,
                        @ORDEM INT = ?,
                        @ANO INT = ?,
                        @SEMESTRE INT = ?,
                        @DISCIPLINA VARCHAR(20) = ?,
                        @NOTA_ID VARCHAR(10) = ?,
                        @CONCEITO VARCHAR(15) = ?,
                        @DATA DATETIME = ?
                    
                UPDATE LY_NOTA_HISTMATR SET CONCEITO = @CONCEITO, DATA = @DATA WHERE ALUNO = @ALUNO AND ORDEM = @ORDEM AND ANO = @ANO AND SEMESTRE = @SEMESTRE AND DISCIPLINA = @DISCIPLINA AND NOTA_ID = @NOTA_ID
                IF @@ROWCOUNT = 0                
                    INSERT INTO LY_NOTA_HISTMATR(ALUNO,ORDEM,ANO,SEMESTRE,DISCIPLINA,NOTA_ID,CONCEITO,DATA,COMPARECEU)
                    VALUES(@ALUNO,@ORDEM,@ANO,@SEMESTRE,@DISCIPLINA,@NOTA_ID,@CONCEITO,@DATA,'S')",
                nota.Aluno, nota.Ordem, nota.Ano, nota.Semestre, nota.Disciplina, nota.Nota_id, nota.Conceito, nota.Data);
        }

        public static RetValue AtualizarFalta(Ly_falta_histmatr.Row falta)
        {
            return IAE(@"
                DECLARE @ALUNO VARCHAR(20) = ?,
                        @ORDEM INT = ?,
                        @ANO INT = ?,
                        @SEMESTRE INT = ?,
                        @DISCIPLINA VARCHAR(20) = ?,
                        @FREQ_ID VARCHAR(10) = ?,
                        @FALTAS DECIMAL = ?                        
                    
                UPDATE LY_FALTA_HISTMATR SET FALTAS = @FALTAS WHERE ALUNO = @ALUNO AND ORDEM = @ORDEM AND ANO = @ANO AND SEMESTRE = @SEMESTRE AND DISCIPLINA = @DISCIPLINA AND FREQ_ID = @FREQ_ID
                IF @@ROWCOUNT = 0                
                    INSERT INTO LY_FALTA_HISTMATR(ALUNO,ORDEM,ANO,SEMESTRE,DISCIPLINA,FREQ_ID,FALTAS)
                    VALUES(@ALUNO,@ORDEM,@ANO,@SEMESTRE,@DISCIPLINA,@FREQ_ID,@FALTAS)",
                falta.Aluno, falta.Ordem, falta.Ano, falta.Semestre, falta.Disciplina, falta.Freq_id, falta.Faltas);
        }

        public static RetValue ProcessarNotas(List<String> alunos, String disciplina, String turma, decimal ano, decimal semestre, String prova)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                //Remoção dos conceitos referentes à disciplina/turma/ano/semestre/prova
                //dos alunos com situação de matrícula = 'Matriculado'
                RetValue retRemocao = RemoverNotasDeAlunosMatriculados(connection, disciplina, turma, ano, semestre, prova);
                if (retRemocao != null && !retRemocao.Ok)
                {
                    connection.Rollback();
                    return retRemocao;
                }

                //Recupera a prova
                Ly_prova.Row provaRow = RN.ProvaTurma.ConsultarProva(prova, disciplina, turma, ano, semestre);
                if (provaRow == null)
                    return new RetValue(false, "", new ErrorList("Instrumento não cadastrado: \"" + prova + "\""));
                else if (String.IsNullOrEmpty(provaRow.Formula))
                    return new RetValue(false, "", new ErrorList("O Instrumento não contém fórmula. Não é possível processar as notas."));

                Ly_nota dt = new Ly_nota();
                foreach (String aluno in alunos)
                {
                    String conceito = null;
                    RetValue retProcess = Formula.CalculaConceitoProvaAluno(connection, provaRow.Formula, disciplina, turma, (int)ano, (int)semestre, aluno, out conceito);
                    if (retProcess != null && !retProcess.Ok)
                    {
                        connection.Rollback();
                        return retProcess;
                    }
                    if (!String.IsNullOrEmpty(conceito))
                    {
                        Ly_nota.Row nota = dt.NewRow();
                        nota.Aluno = aluno;
                        nota.Ano = ano;
                        nota.Compareceu = "S";
                        nota.Conceito = conceito.Replace(".", ",");
                        nota.Data = DateTime.Today;
                        nota.Disciplina = disciplina;
                        nota.Ordem = provaRow.Ordem;
                        nota.Prova = prova;
                        nota.Semestre = semestre;
                        nota.Turma = turma;
                        RetValue ret = InserirNotaDeAluno(connection, nota);
                        if (ret != null && !ret.Ok)
                        {
                            connection.Rollback();
                            return ret;
                        }
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                connection.Rollback();
                RetValue ret = VerificarErro(connection.GetErrors());
                if (ret != null && !ret.Ok)
                {
                    ErrorList err = ret.Errors;
                    err.Add(e.Message);
                    return new RetValue(false, "", err);
                }
                else
                    return ret;
            }
            finally
            {
                connection.Close();
            }
        }

        public static RetValue RemoverNotasDeAlunosMatriculados(TConnectionWritable connection, String disciplina, String turma, decimal ano, decimal semestre, String prova)
        {
            try
            {
                String sql =
                   @"DELETE n FROM ly_nota n WITH(NOLOCK)
                      WHERE n.disciplina = ? AND 
                      n.turma = ? AND 
                      n.ano = ? AND 
                      n.semestre = ? AND 
                      n.prova = ? AND
                     EXISTS (select 1 FROM ly_matricula mat WITH(NOLOCK)
                      WHERE mat.sit_matricula = 'Matriculado' AND
					        mat.disciplina = ? AND
							mat.turma = ? AND 
							mat.ano = ? AND 
							mat.semestre = ? AND
                            mat.aluno = n.aluno )";

                int count = TCommand.ExecuteNonQuery(connection, sql, disciplina, turma, ano, semestre, prova, disciplina, turma, ano, semestre);
                return VerificarErro(connection.GetErrors());
            }
            catch
            {
                return new RetValue(false, "", new ErrorList("Erro durante remoção de notas de alunos matriculados."));
            }
        }

        public static RetValue InserirNotaDeAluno(TConnectionWritable connection, Ly_nota.Row notaAluno)
        {
            try
            {
                if (notaAluno.Conceito != null)
                {
                    if (notaAluno.Conceito.Trim() != "")
                    {
                        TCommand.ExecuteNonQuery(connection,
                            @"  INSERT INTO ly_nota(aluno, disciplina, turma, ano, semestre, prova, compareceu, conceito, data, ordem)
                        VALUES(?,?,?,?,?,?,?,?,?,?)",
                                notaAluno.Aluno, notaAluno.Disciplina, notaAluno.Turma, notaAluno.Ano, notaAluno.Semestre, notaAluno.Prova,
                                notaAluno.Compareceu, notaAluno.Conceito, notaAluno.Data, notaAluno.Ordem);
                    }
                }
                else
                {
                    TCommand.ExecuteNonQuery(connection,
                        @"  INSERT INTO ly_nota(aluno, disciplina, turma, ano, semestre, prova, compareceu, data, ordem)
                        VALUES(?,?,?,?,?,?,?,?,?)",
                            notaAluno.Aluno, notaAluno.Disciplina, notaAluno.Turma, notaAluno.Ano, notaAluno.Semestre, notaAluno.Prova,
                            notaAluno.Compareceu, notaAluno.Data, notaAluno.Ordem);
                }
                return VerificarErro(connection.GetErrors());
            }
            catch
            {
                return new RetValue(false, "", new ErrorList("Erro durante inserção de nota \"" + notaAluno.Prova + "\" do aluno \"" + notaAluno.Aluno + "."));
            }
        }

        public static String VerificarConceito(String aluno, String prova, String disciplina, String turma,
            decimal ano, decimal semestre)
        {
            TConnection connection = Config.CreateConnection();
            String nota = String.Empty;

            try
            {
                connection.Open();

                QueryTable qtNota = new QueryTable(@"
                    SELECT *
                    FROM LY_NOTA
                    WHERE ALUNO = ?
                        AND PROVA = ?
                        AND DISCIPLINA = ?
                        AND TURMA = ?
                        AND ANO = ?
                        AND SEMESTRE = ?");
                qtNota.Query(connection, aluno, prova, disciplina, turma, ano, semestre);

                if (qtNota.Rows.Count > 0)
                {
                    nota = Convert.ToString(qtNota.Rows[0]["conceito"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return nota;
        }

        public static int RetornaBimestesLançados(string aluno, decimal ano, decimal semestre, string turma, string disciplina)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {

                var contextQuery = new ContextQuery(
                    @" SELECT  COUNT(PROVA) AS BIMESTRES
                        FROM    LY_NOTA n ( NOLOCK )
                        WHERE   n.ALUNO = @ALUNO
                                AND n.DISCIPLINA = @DISCIPLINA
                                AND n.TURMA = @TURMA
                                AND n.ANO = @ANO
                                AND n.SEMESTRE = @SEMESTRE
                                AND CONCEITO IS NOT NULL ");

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);

                return ctx.GetReturnValue<int>(contextQuery);
            }
        }

        public static decimal CalculaNotaFinal(string aluno, decimal ano, decimal semestre, string turma, string disciplina)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {

                var contextQuery = new ContextQuery(
                    @" SELECT  ISNULL(SUM(CONVERT(DECIMAL(10, 2), ( REPLACE(CONCEITO, ',', '.') ))),0) AS CONCEITO
                        FROM    LY_NOTA n ( NOLOCK )
                        WHERE   n.ALUNO = @ALUNO
                                AND n.DISCIPLINA = @DISCIPLINA
                                AND n.TURMA = @TURMA
                                AND n.ANO = @ANO
                                AND n.SEMESTRE = @SEMESTRE
                                AND (CONCEITO IS NOT NULL and CONCEITO <> '') ");

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);

                return ctx.GetReturnValue<decimal>(contextQuery);
            }
        }

        public static bool ExisteNotas(List<string> alunos, string turma, string ano, string periodo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Select 1 From ly_nota where turma = ? and ano = ? and semestre = ? ");
            sb.Append(" and aluno in(");

            for (int cont = 0; cont < alunos.Count; cont++)
            {
                if (cont != 0)
                    sb.Append(",");
                sb.Append("'" + alunos[cont] + "'");
            }
            sb.Append(") ");

            QueryTable qt = new QueryTable(sb.ToString());
            qt.Query(Config.CreateConnection(), turma, ano, periodo);

            return qt.Rows.Count > 0;
        }

        public static bool VerificaNotasLancadasTurmaPrimeiroBimestre(string disciplina, string turma, decimal ano, decimal semestre)
        {
            decimal subperiodo = 1;
            string prova = null;
            try
            {
                prova = RN.ProvaTurma.ConsultarProvas(disciplina, turma, ano, semestre, subperiodo)[0].Prova;
            }
            catch
            {
                return false;
            }
            if (String.IsNullOrEmpty(prova) ||
                String.IsNullOrEmpty(disciplina) ||
                String.IsNullOrEmpty(turma))
                return false;
            string sql = @"select isnull(conceito, 'SN') from LY_NOTA n 
                inner join LY_MATRICULA m on n.DISCIPLINA = m.DISCIPLINA 
	                and n.TURMA = m.TURMA 
	                and n.ANO = m.ANO 
	                and n.SEMESTRE = m. SEMESTRE
	                and m.SIT_MATRICULA = 'Matriculado'
                inner join LY_ALUNO a on m.ALUNO = a.ALUNO
                where n.PROVA = ? 
	                and m.DISCIPLINA = ?
	                AND m.TURMA = ?
	                AND m.ANO = ?
	                AND m.SEMESTRE = ? 
	                AND m.SIT_MATRICULA = 'Matriculado'";
            QueryTable qt = new QueryTable(sql);
            qt.Query(Config.CreateConnection(), prova, disciplina, turma, ano, semestre);
            return qt.Rows.Count > 0;
        }

        public static bool ExistePendenciaNotasBimestresAnteriores(DataContext dataContext, string disciplina, string turma, decimal? ano, decimal? semestre, decimal subperiodo)
        {
            var contextQuery = new ContextQuery(
                @"SELECT TOP 1
                        1
                FROM    ly_matricula m ( NOLOCK )
                        INNER JOIN ly_prova p ( NOLOCK ) ON p.DISCIPLINA = m.DISCIPLINA
                                                            AND p.TURMA = m.TURMA
                                                            AND p.ANO = m.ANO
                                                            AND p.SEMESTRE = m.SEMESTRE
                WHERE   NOT EXISTS ( SELECT TOP 1
                                            1
                                     FROM   LY_NOTA n ( NOLOCK )
                                     WHERE  n.ALUNO = m.ALUNO
                                            AND n.DISCIPLINA = m.DISCIPLINA
                                            AND n.TURMA = m.TURMA
                                            AND n.ANO = m.ANO
                                            AND n.SEMESTRE = m.SEMESTRE
                                            AND n.PROVA = p.PROVA )
                        AND m.ANO = @ANO
                        AND m.SEMESTRE = @PERIODO
                        AND m.DISCIPLINA = @DISCIPLINA
                        AND m.TURMA = @TURMA
                        AND m.SIT_MATRICULA = 'Matriculado'
                        AND p.SUBPERIODO < @SUPERIODO");

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", semestre);
            contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@SUPERIODO", subperiodo);

            var result = dataContext.GetReturnValue(contextQuery);

            return result != null;
        }

        public static bool ExistePendenciaFaltasBimestresAnteriores(DataContext dataContext, string disciplina, string turma, decimal? ano, decimal? semestre, decimal subperiodo)
        {
            var contextQuery = new ContextQuery(
                @"SELECT TOP 1
                        1
                FROM    ly_matricula m ( NOLOCK )
                        INNER JOIN ly_freq p ( NOLOCK ) ON p.DISCIPLINA = m.DISCIPLINA
                                                            AND p.TURMA = m.TURMA
                                                            AND p.ANO = m.ANO
                                                            AND p.PERIODO = m.SEMESTRE
                WHERE   NOT EXISTS ( SELECT TOP 1
                                            1
                                     FROM   LY_FALTA n ( NOLOCK )
                                     WHERE  n.ALUNO = m.ALUNO
                                            AND n.DISCIPLINA = m.DISCIPLINA
                                            AND n.TURMA = m.TURMA
                                            AND n.ANO = m.ANO
                                            AND n.PERIODO = m.SEMESTRE
                                            AND N.FREQ = P.FREQ )
                        AND m.ANO = @ANO
                        AND m.SEMESTRE = @PERIODO
                        AND m.DISCIPLINA = @DISCIPLINA
                        AND m.TURMA = @TURMA
                        AND m.SIT_MATRICULA = 'Matriculado'
                        AND p.SUBPERIODO < @SUPERIODO");

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", semestre);
            contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@SUPERIODO", subperiodo);

            var result = dataContext.GetReturnValue(contextQuery);

            return result != null;
        }

        internal static bool ExisteNotas(TConnectionWritable connection, string aluno, string disciplinaDestino, string turmaDestinoNota, decimal ano, decimal semestre, string prova)
        {
            QueryTable qt = Consultar(connection, "select 1 from ly_nota where aluno = ? and disciplina = ? and turma = ? and ano = ? and semestre = ? and prova = ?",
                aluno, disciplinaDestino, turmaDestinoNota, ano, semestre, prova);
            return qt.Rows.Count > 0;
        }

        public static bool ExisteNotaPendenteBimestresAnteriores(string disciplina, string turma, string ano, string semestre, string subperiodo)
        {
            var sql = @"select top 1 1 from ly_matricula m (NOLOCK) inner join 
                            ly_prova p (NOLOCK) on p.DISCIPLINA = m.DISCIPLINA and p.TURMA = m.TURMA
                            and p.ANO = m.ANO and p.SEMESTRE = m.SEMESTRE
                            where 
                            not exists(select TOP 1 1 from LY_NOTA n (NOLOCK) where n.ALUNO = m.ALUNO and n.DISCIPLINA = m.DISCIPLINA and n.TURMA = m.TURMA and n.ANO = m.ANO and n.SEMESTRE = m.SEMESTRE and n.PROVA = p.PROVA)
                            and m.ANO = ?
                            and m.SEMESTRE = ?
                            and m.DISCIPLINA = ?
                            and m.TURMA = ?
                            and m.SIT_MATRICULA = 'Matriculado'
                            and p.SUBPERIODO < ?";

            var retorno = ExecutarFuncao(sql, ano, semestre, disciplina, turma, subperiodo);

            return retorno == 1;
        }

        public static string ObterFrequencia(string ano, string semestre, string disciplina, string turma, decimal subperiodo)
        {
            return ConsultarCampo("select top 1 freq + '|' +  descricao + '|' + isnull(convert(varchar(9),convert(int,aulas_dadas)),'') + '|' + isnull(convert(varchar(9),convert(int,aulas_previstas)),'') from ly_freq (NOLOCK) where ano = ? and periodo = ? and disciplina = ? and turma = ? and subperiodo = ?", ano, semestre, disciplina, turma, subperiodo);
        }

        /// <summary>
        /// Obtém as notas dos alunos de uma turma.
        /// </summary>
        /// <param name="turma">Turma.</param>
        /// <param name="ano">Ano da turma.</param>
        /// <param name="semestre">Semestre da turma.</param>
        /// <returns>QueryTable contendo aluno, disciplina, conceito de Ly_nota.</returns>
        public static QueryTable ConsultarNotasTurma(string turma, string ano, string semestre)
        {
            return Consultar(
                @"  SELECT	aluno, disciplina, prova, conceito
                    FROM	ly_nota (NOLOCK)
                    WHERE	turma = ? AND ano = ? AND semestre = ?",
                    turma, ano, semestre);
        }

        #region Lançamento notas histórico

        public class DisciplinaNotaHistorico
        {

        }

        public class DisciplinaNotaHistoricoItem
        {

        }

        public class DisciplinaFaltaHistorico : List<DisciplinaFaltaHistoricoItem>
        {
            public string Disciplina { get; set; }
        }

        public class DisciplinaFaltaHistoricoItem
        {
            public string Aluno { get; set; }
            public string NomeAluno { get; set; }
            public int? Falta { get; set; }
        }



        #endregion

        public static void MigrarNotas(DataContext ctx, LyMatricula matricula, string turmaDestino)
        {
            //verificar se a disciplina tinha notas
            var contextQuery = new ContextQuery(
                        @" SELECT  COUNT(*) AS TOTAL
                            FROM    LY_NOTA N
									INNER JOIN LY_TURMA T ON N.ANO = T.ANO
															AND N.SEMESTRE = T.SEMESTRE
															AND N.TURMA = T.TURMA
															AND N.DISCIPLINA = T.DISCIPLINA
                            WHERE   N.ALUNO = @ALUNO
                                    AND N.DISCIPLINA = @DISCIPLINA
                                    AND N.ANO = @ANO
                                    AND N.SEMESTRE = @SEMESTRE
									AND ISNULL(T.ELETIVA, 'N') = 'N'
                                    AND EXISTS ( SELECT 1
                                                 FROM   LY_PROVA P
                                                 WHERE  N.DISCIPLINA = P.DISCIPLINA
                                                        AND ( P.TURMA = @TURMA )
                                                        AND N.ANO = P.ANO
                                                        AND N.SEMESTRE = P.SEMESTRE
                                                        AND N.PROVA = P.PROVA) ");

            contextQuery.Parameters.Add("@ALUNO", matricula.Aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", matricula.Disciplina);
            contextQuery.Parameters.Add("@TURMA", matricula.Turma);
            contextQuery.Parameters.Add("@ANO", matricula.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", matricula.Semestre);

            var notas = ctx.GetReturnValue<int>(contextQuery);

            if (notas > 0)
            {
                //se existir cadastra as novas para a turma nova
                ctx.ApplyModifications(
                new ContextQuery(
                @" INSERT  INTO dbo.LY_NOTA
                        ( ALUNO ,
                          DISCIPLINA ,
                          TURMA ,
                          ANO ,
                          SEMESTRE ,
                          PROVA ,
                          CONCEITO ,
                          ORDEM ,
                          FORMULARIO ,
                          DATA ,
                          COMPARECEU ,
                          RECUPERACAO_PARALELA ,
                          SEM_AVALIACAO ,
                          JUSTIFICATIVA ,
                          MOTIVOSEMNOTAID
                        )
                        ( SELECT    N.ALUNO ,
                                    N.DISCIPLINA ,
                                    @TURMADESTINO AS TURMA ,
                                    N.ANO ,
                                    N.SEMESTRE ,
                                    N.PROVA ,
                                    N.CONCEITO ,
                                    N.ORDEM ,
                                    N.FORMULARIO ,
                                    N.DATA ,
                                    N.COMPARECEU ,
                                    RECUPERACAO_PARALELA ,
                                    SEM_AVALIACAO ,
                                    JUSTIFICATIVA ,
                                    MOTIVOSEMNOTAID
                          FROM      LY_NOTA N
									INNER JOIN LY_TURMA T ON N.ANO = T.ANO
															AND N.SEMESTRE = T.SEMESTRE
															AND N.TURMA = T.TURMA
															AND N.DISCIPLINA = T.DISCIPLINA
                          WHERE     N.ALUNO = @ALUNO
                                    AND N.DISCIPLINA = @DISCIPLINA
                                    AND N.ANO = @ANO
                                    AND N.SEMESTRE = @SEMESTRE
                                    AND n.TURMA = @TURMA
									AND ISNULL(T.ELETIVA, 'N') = 'N'
                                    AND EXISTS ( SELECT 1
                                                 FROM   LY_PROVA P
                                                 WHERE  N.DISCIPLINA = P.DISCIPLINA
                                                        AND ( P.TURMA = @TURMADESTINO )
                                                        AND N.ANO = P.ANO
                                                        AND N.SEMESTRE = P.SEMESTRE
                                                        AND N.PROVA = P.PROVA ) 
									AND NOT EXISTS (SELECT 1
                                                 FROM   LY_NOTA N2
                                                 WHERE  N.DISCIPLINA = N2.DISCIPLINA
                                                        AND N2.TURMA =  @TURMADESTINO
                                                        AND N.ANO = N2.ANO
                                                        AND N.SEMESTRE = N2.SEMESTRE
                                                        AND N.PROVA = N2.PROVA 
                                                        AND N.ALUNO = N2.ALUNO)	)",
               new ContextQueryParameter("@ALUNO", matricula.Aluno),
               new ContextQueryParameter("@DISCIPLINA", matricula.Disciplina),
               new ContextQueryParameter("@TURMA", matricula.Turma),
               new ContextQueryParameter("@TURMADESTINO", turmaDestino),
               new ContextQueryParameter("@ANO", matricula.Ano),
               new ContextQueryParameter("@SEMESTRE", matricula.Semestre)));

                //Atualiza as declaraçoes sem nota para o aluno
                ctx.ApplyModifications(
                new ContextQuery(
                @" UPDATE  DN
                    SET     NOTAID = NOTADESTINO.NOTAID
                    FROM    DECLARACAOSEMNOTA DN
                            INNER JOIN LY_NOTA NOTAORIGEM ON DN.NOTAID = NOTAORIGEM.NOTAID
                            INNER JOIN LY_NOTA NOTADESTINO ON NOTAORIGEM.ALUNO = NOTADESTINO.ALUNO
                                                              AND NOTAORIGEM.DISCIPLINA = NOTADESTINO.DISCIPLINA
                                                              AND NOTAORIGEM.ANO = NOTADESTINO.ANO
                                                              AND NOTAORIGEM.SEMESTRE = NOTADESTINO.SEMESTRE
                                                              AND NOTADESTINO.TURMA = @TURMADESTINO
                    WHERE   NOTAORIGEM.ALUNO = @ALUNO
                            AND NOTAORIGEM.DISCIPLINA = @DISCIPLINA
                            AND NOTAORIGEM.ANO = @ANO
                            AND NOTAORIGEM.SEMESTRE = @SEMESTRE
                            AND NOTAORIGEM.TURMA = @TURMA ",
               new ContextQueryParameter("@ALUNO", matricula.Aluno),
               new ContextQueryParameter("@DISCIPLINA", matricula.Disciplina),
               new ContextQueryParameter("@TURMA", matricula.Turma),
               new ContextQueryParameter("@TURMADESTINO", turmaDestino),
               new ContextQueryParameter("@ANO", matricula.Ano),
               new ContextQueryParameter("@SEMESTRE", matricula.Semestre)));

                //deleta declaraçoes sem nota da turma antiga
                ctx.ApplyModifications(
                new ContextQuery(
                @" DELETE  DN
                  FROM    DECLARACAOSEMNOTA DN
                            INNER JOIN LY_NOTA NOTAORIGEM ON DN.NOTAID = NOTAORIGEM.NOTAID                          
                    WHERE   NOTAORIGEM.ALUNO = @ALUNO
                            AND NOTAORIGEM.DISCIPLINA = @DISCIPLINA
                            AND NOTAORIGEM.ANO = @ANO
                            AND NOTAORIGEM.SEMESTRE = @SEMESTRE
                            AND NOTAORIGEM.TURMA = @TURMA",

               new ContextQueryParameter("@ALUNO", matricula.Aluno),
               new ContextQueryParameter("@DISCIPLINA", matricula.Disciplina),
               new ContextQueryParameter("@TURMA", matricula.Turma),
               new ContextQueryParameter("@TURMADESTINO", turmaDestino),
               new ContextQueryParameter("@ANO", matricula.Ano),
               new ContextQueryParameter("@SEMESTRE", matricula.Semestre)));


                //deleta notas da turma antiga
                ctx.ApplyModifications(
                new ContextQuery(
                @" DELETE  LY_NOTA
                    WHERE   ALUNO = @ALUNO
                            AND DISCIPLINA = @DISCIPLINA
                            AND ANO = @ANO
                            AND SEMESTRE = @SEMESTRE
                            AND TURMA = @TURMA
                            --AND EXISTS ( SELECT 1
                            --             FROM   LY_PROVA P
                            --             WHERE  LY_NOTA.DISCIPLINA = P.DISCIPLINA
                            --                    AND ( P.TURMA = @TURMADESTINO )
                            --                    AND LY_NOTA.ANO = P.ANO
                            --                    AND LY_NOTA.SEMESTRE = P.SEMESTRE
                            --                    AND LY_NOTA.PROVA = P.PROVA ) ",

               new ContextQueryParameter("@ALUNO", matricula.Aluno),
               new ContextQueryParameter("@DISCIPLINA", matricula.Disciplina),
               new ContextQueryParameter("@TURMA", matricula.Turma),
               new ContextQueryParameter("@TURMADESTINO", turmaDestino),
               new ContextQueryParameter("@ANO", matricula.Ano),
               new ContextQueryParameter("@SEMESTRE", matricula.Semestre)));
            }
        }

        public static DataTable ListarFilipeta(string matricula)
        {
            if (string.IsNullOrEmpty(matricula))
            {
                return null;
            }

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT  PE.nome_compl,
                                        m.aluno,
                                        m.sit_matricula,
                                        ISNULL(( SELECT TOP 1
                                                        [DESCRICAO] + ' - ' + CONVERT(VARCHAR, [DT_INICIO], 103)
                                                        + ' a ' + CONVERT(VARCHAR, [DT_FIM], 103)
                                                 FROM   [LYCEUM].[dbo].[LY_ALUNO_LICENCA] (NOLOCK)
                                                 WHERE  [ALUNO] = m.aluno
                                                        AND [DISCIPLINA] = m.disciplina
                                                        AND [TURMA] = m.turma
                                                        AND [ANO] = m.ano
                                                        AND [SEMESTRE] = m.semestre
                                                        AND DT_FIM = ( SELECT   MAX(DT_FIM)
                                                                       FROM     [LY_ALUNO_LICENCA]
                                                                       WHERE    [ALUNO] = m.aluno
                                                                                AND [DISCIPLINA] = m.disciplina
                                                                                AND [TURMA] = m.turma
                                                                                AND [ANO] = m.ano
                                                                                AND [SEMESTRE] = m.semestre
                                                                     )
                                               ), '')
                                        + ISNULL(( SELECT TOP 1
                                                            'Remanejado para a turma ' + TURMA_DESTINO
                                                            + ' em ' + CONVERT(VARCHAR, DATA, 103)
                                                   FROM     dbo.LY_TURMA_TRANSF (NOLOCK)
                                                   WHERE    ALUNO = m.ALUNO
                                                            AND ANO = m.ANO
                                                            AND PERIODO = m.SEMESTRE
                                                            AND TURMA_ANT = m.TURMA
                                                 ), '')
                                        + ISNULL(( SELECT TOP 1
                                                            MOTIVO + ' em ' + CONVERT(VARCHAR, DT_ENCERRAMENTO, 103)
                                                   FROM     dbo.LY_H_CURSOS_CONCL (NOLOCK)
                                                   WHERE    ALUNO = m.ALUNO
                                                            AND DT_REABERTURA IS NULL
                                                 ), '') descricao_situacao,
                                        m.num_chamada,
                                        m.disciplina,
                                        m.turma,
                                        m.ano,
                                        m.semestre,
                                        CASE m.sit_matricula
                                          WHEN 'Matriculado' THEN n.conceito
                                          ELSE NULL
                                        END AS MÉDIA,
                                        CASE m.sit_matricula
                                          WHEN 'Matriculado' THEN CONVERT(INT, f.faltas)
                                          ELSE NULL
                                        END AS faltas,
                                        prova.nome AS nome_prova,
                                        prova.nota_max,
                                        prova.formula,
                                        CASE m.sit_matricula
                                          WHEN 'Matriculado' THEN n.recuperacao_paralela
                                          ELSE 'N'
                                        END AS recuperacao_paralela,
                                        CASE m.sit_matricula
                                          WHEN 'Matriculado' THEN n.sem_avaliacao
                                          ELSE 'N'
                                        END AS sem_avaliacao,
                                        CASE m.sit_matricula
                                          WHEN 'Matriculado' THEN n.justificativa
                                          ELSE ''
                                        END AS justificativa
                                FROM    ly_matricula m WITH ( NOLOCK )
                                        JOIN ly_aluno a WITH ( NOLOCK ) ON m.aluno = a.aluno
                                        INNER JOIN LY_PESSOA PE (NOLOCK)  ON PE.PESSOA = A.PESSOA
                                        LEFT JOIN ly_freq freq ( NOLOCK ) ON freq.disciplina = m.disciplina
                                                                             AND freq.turma = m.turma
                                                                             AND freq.ano = m.ano
                                                                             AND freq.periodo = m.semestre
                                                                             AND freq.subperiodo = 1
                                        LEFT JOIN LY_FALTA f ( NOLOCK ) ON f.ALUNO = m.ALUNO
                                                                           AND f.ANO = m.ANO
                                                                           AND f.DISCIPLINA = m.DISCIPLINA
                                                                           AND f.PERIODO = m.SEMESTRE
                                                                           AND f.TURMA = m.TURMA
                                                                           AND freq.freq = f.freq
                                        LEFT JOIN ly_prova prova ( NOLOCK ) ON prova.disciplina = m.disciplina
                                                                               AND prova.turma = m.turma
                                                                               AND prova.ano = m.ano
                                                                               AND prova.semestre = m.semestre
                                                                               AND prova.subperiodo = 1
                                        LEFT JOIN LY_NOTA n ( NOLOCK ) ON n.DISCIPLINA = prova.disciplina
                                                                          AND n.turma = prova.TURMA
                                                                          AND n.ANO = prova.ANO
                                                                          AND n.SEMESTRE = prova.SEMESTRE
                                                                          AND n.PROVA = prova.PROVA
                                                                          AND n.ALUNO = m.ALUNO
                                WHERE   m.disciplina = 'FIS_1_2'
                                        AND m.turma = '1002-180196'
                                        AND m.ano = 2012
                                        AND m.semestre = 0
                                ORDER BY m.num_chamada,
                                        PE.nome_compl"
                };

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static bool PossuiHistoricoNota(string aluno, string disciplina, int ano, int semestre, string prova, int ordem)
        {
            string sql = "select 1 from LY_NOTA_HISTMATR where ALUNO = ? AND ANO = ? AND SEMESTRE = ? AND DISCIPLINA = ? AND NOTA_ID = ? AND ORDEM = ? ";

            int retorno = ExecutarFuncao(sql, aluno, ano, semestre, disciplina, prova, ordem);

            if (retorno == 1)
                return true;
            else
                return false;
        }

        public bool PossuiNotaPor(string disciplina)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                FROM    LY_NOTA NOLOCK
                                WHERE   DISCIPLINA = @DISCIPLINA ";

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);

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
       
        public bool PossuiNotaNaoNulaPor(DataContext ctx, string aluno, decimal ano, decimal periodo, string turma)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM   LY_NOTA (NOLOCK) 
                                WHERE  ALUNO = @ALUNO 
                                       AND TURMA = @TURMA 
                                       AND ANO = @ANO 
                                       AND SEMESTRE = @SEMESTRE 
                                       AND CONCEITO IS NOT NULL  ";

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", periodo);
            contextQuery.Parameters.Add("@TURMA", turma);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void RemoveNotaNula(DataContext ctx, string aluno, decimal ano, decimal periodo, string turma)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE LY_NOTA 
                            WHERE ALUNO = @ALUNO
	                            AND TURMA = @TURMA
	                            AND ANO = @ANO
	                            AND SEMESTRE = @SEMESTRE
	                            AND CONCEITO IS NULL ";

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

        public bool PossuiNotaEmPeridosPossiveisPor(string aluno, int ano, string periodosPossiveis)
        {	
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;           

            try
            {
                contextQuery.Command = string.Format(@" SELECT COUNT(*)
                        FROM    DBO.LY_MATRICULA M
                                INNER JOIN DBO.LY_NOTA N ON M.ALUNO = N.ALUNO
                                                            AND M.DISCIPLINA = N.DISCIPLINA
                                                            AND M.TURMA = N.TURMA
                                                            AND M.ANO = N.ANO
                                                            AND M.SEMESTRE = N.SEMESTRE
                        WHERE   M.SIT_MATRICULA = @SIT_MATRICULA
                                AND M.ALUNO = @ALUNO
                                AND M.ANO = @ANO
                                AND M.SEMESTRE IN ( {0} ) ", periodosPossiveis);

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@SIT_MATRICULA", Matricula.Matriculado);

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
    
        internal List<RN.DTOs.NotaTurmaAtualTransferenciaAluno> ObtemNotaTurmaAtualTransferenciaAluno(DataContext contexto, string aluno, TceTransferenciaOrigem transferenciaOrigem, TceTransferenciaDestino transferenciaDestino)
        {
            DataTable dataTable = new DataTable();
            List<RN.DTOs.NotaTurmaAtualTransferenciaAluno> listaNotaTurmaAtualTransferenciaAluno = new List<RN.DTOs.NotaTurmaAtualTransferenciaAluno>();

            dataTable = contexto.GetDataTable(
                new ContextQuery(
                    @"SELECT  N.DISCIPLINA,
                              N.PROVA,
                              N.CONCEITO,
                              N.ORDEM,
                              N.FORMULARIO,
                              N.COMPARECEU,
                              N.DATA,
                              N.RECUPERACAO_PARALELA,
                              N.SEM_AVALIACAO,
                              N.JUSTIFICATIVA,
                              N.NOTAID,
                              N.NOTAPROVA,
                              N.NOTARECUPERACAO,
                              N.MOTIVOSEMNOTAID,
                              CASE WHEN ND.ALUNO IS NULL THEN 0 ELSE 1 END AS ATUALIZANOTA
                      FROM    LY_NOTA N
                              LEFT JOIN LY_NOTA ND ON ND.ALUNO      = N.ALUNO
                                                  AND ND.DISCIPLINA = N.DISCIPLINA
                                                  AND ND.TURMA      = @TURMADESTINO
                                                  AND ND.ANO        = @ANODESTINO
                                                  AND ND.SEMESTRE   = @PERIODODESTINO
                                                  AND ND.PROVA      = N.PROVA

                              INNER JOIN ( SELECT  DISTINCT G.DISCIPLINA
                                           FROM    LY_GRADE G 
                                           WHERE   G.CURSO           = @CURSODESTINO
                                                   AND G.TURNO       = @TURNODESTINO
                                                   AND G.CURRICULO   = @CURRICULODESTINO
                                                   AND G.SERIE_IDEAL = @SERIEDESTINO
                              ) G ON G.DISCIPLINA = N.DISCIPLINA
                              
                              INNER JOIN ( SELECT  DISTINCT P.DISCIPLINA,
                                                   P.PROVA
                                           FROM    LY_PROVA P
                                           WHERE   P.TURMA        = @TURMADESTINO
                                                   AND P.ANO      = @ANODESTINO
                                                   AND P.SEMESTRE = @PERIODODESTINO
                              ) P ON P.DISCIPLINA = N.DISCIPLINA
                                 AND P.PROVA      = N.PROVA

                      WHERE   N.ALUNO = @ALUNO
                              AND N.TURMA = @TURMA
                              AND N.ANO = @ANO
                              AND N.SEMESTRE = @PERIODO

                      ORDER BY DISCIPLINA,
                              PROVA",
                    new ContextQueryParameter("@ALUNO", aluno),
                    new ContextQueryParameter("@TURMA", transferenciaOrigem.Turma),
                    new ContextQueryParameter("@ANO", transferenciaOrigem.Ano),
                    new ContextQueryParameter("@PERIODO", transferenciaOrigem.Periodo),
                    new ContextQueryParameter("@CURSODESTINO", transferenciaDestino.Curso),
                    new ContextQueryParameter("@TURNODESTINO", transferenciaDestino.Turno),
                    new ContextQueryParameter("@CURRICULODESTINO", transferenciaDestino.Curriculo),
                    new ContextQueryParameter("@SERIEDESTINO", transferenciaDestino.Serie),
                    new ContextQueryParameter("@TURMADESTINO", transferenciaDestino.Turma),
                    new ContextQueryParameter("@ANODESTINO", transferenciaDestino.Ano),
                    new ContextQueryParameter("@PERIODODESTINO", transferenciaDestino.Periodo)));

            foreach (DataRow dataRow in dataTable.Rows)
            {
                RN.DTOs.NotaTurmaAtualTransferenciaAluno notaTurmaAtualTransferenciaAluno = new RN.DTOs.NotaTurmaAtualTransferenciaAluno();
                notaTurmaAtualTransferenciaAluno.Disciplina = dataRow["DISCIPLINA"].ToString();
                notaTurmaAtualTransferenciaAluno.Prova = dataRow["PROVA"].ToString();

                if (dataRow["CONCEITO"] != DBNull.Value)
                    notaTurmaAtualTransferenciaAluno.Conceito = dataRow["CONCEITO"].ToString();

                notaTurmaAtualTransferenciaAluno.Ordem = Convert.ToDecimal(dataRow["ORDEM"]);

                if (dataRow["FORMULARIO"] != DBNull.Value)
                    notaTurmaAtualTransferenciaAluno.Formulario = Convert.ToDecimal(dataRow["FORMULARIO"]);

                notaTurmaAtualTransferenciaAluno.Compareceu = dataRow["COMPARECEU"].ToString();
                notaTurmaAtualTransferenciaAluno.Data = Convert.ToDateTime(dataRow["DATA"]);

                if (dataRow["RECUPERACAO_PARALELA"] != DBNull.Value)
                    notaTurmaAtualTransferenciaAluno.RecuperacaoParalela = dataRow["RECUPERACAO_PARALELA"].ToString();

                if (dataRow["SEM_AVALIACAO"] != DBNull.Value)
                    notaTurmaAtualTransferenciaAluno.SemAvaliacao = dataRow["SEM_AVALIACAO"].ToString();

                if (dataRow["JUSTIFICATIVA"] != DBNull.Value)
                    notaTurmaAtualTransferenciaAluno.Justificativa = dataRow["JUSTIFICATIVA"].ToString();

                notaTurmaAtualTransferenciaAluno.NotaId = Convert.ToInt32(dataRow["NOTAID"]);

                if (dataRow["NOTAPROVA"] != DBNull.Value)
                    notaTurmaAtualTransferenciaAluno.NotaProva = Convert.ToDecimal(dataRow["NOTAPROVA"]);

                if (dataRow["NOTARECUPERACAO"] != DBNull.Value)
                    notaTurmaAtualTransferenciaAluno.NotaRecuperacao = Convert.ToDecimal(dataRow["NOTARECUPERACAO"]);

                if (dataRow["MOTIVOSEMNOTAID"] != DBNull.Value)
                    notaTurmaAtualTransferenciaAluno.MotivoSemNotaId = Convert.ToInt32(dataRow["MOTIVOSEMNOTAID"]);

                notaTurmaAtualTransferenciaAluno.AtualizaNota = Convert.ToBoolean(dataRow["ATUALIZANOTA"]);

                listaNotaTurmaAtualTransferenciaAluno.Add(notaTurmaAtualTransferenciaAluno);
            }

            return listaNotaTurmaAtualTransferenciaAluno;
        }

        public List<string> ChecaNotasNaoMigradas(DataContext ctx, string aluno, string turmaAtual, string turmaNova, string disciplina)
        {
            //Verifica se alguma nota não será migrada para turma destino por não existir prova gerada
            List<string> listaAvisos = new List<string>();
            DataTable notaSemProvaDestino = ObtemNotaSemProvaDestinoPor(ctx, aluno, turmaAtual, turmaNova, disciplina);

            foreach (DataRow dr in notaSemProvaDestino.Rows)
            {
                if (listaAvisos.Count == 0)
                {
                    listaAvisos.Add("Notas não migradas: Disciplina: " + Convert.ToString(dr["disciplina"]) + " Turma: " + Convert.ToString(dr["turma"]) + " Ano: " + Convert.ToString(dr["ano"]) + " Semestre: " + Convert.ToString(dr["semestre"]) + " - " + Convert.ToString(dr["prova"]) + " = " + Convert.ToString(dr["conceito"]));
                }
                else
                {
                    listaAvisos.Add(" Disciplina: " + Convert.ToString(dr["disciplina"]) + " Turma: " + Convert.ToString(dr["turma"]) + " Ano: " + Convert.ToString(dr["ano"]) + " Semestre: " + Convert.ToString(dr["semestre"]) + " - " + Convert.ToString(dr["prova"]) + " = " + Convert.ToString(dr["conceito"]));
                }
            }

            return listaAvisos;
        }

        public DataTable ObtemNotaSemProvaDestinoPor(DataContext ctx, string aluno, string turmaAtual, string turmaNova, string disciplina)
        {
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            contextQuery.Command = @" SELECT DISTINCT N.DISCIPLINA, 
                                        N.TURMA, 
                                        N.ANO, 
                                        N.SEMESTRE, 
                                        PROVA, 
                                        ISNULL(CONCEITO, 'SN') CONCEITO 
                        FROM   LY_NOTA N 
                        WHERE  N.ALUNO = @ALUNO 
                               AND N.TURMA = @TURMA_ATUAL 
                               AND N.DISCIPLINA = @DISCIPLINA 
                               AND NOT EXISTS (SELECT 1 
                                               FROM   LY_PROVA P2 
                                               WHERE  P2.DISCIPLINA = N.DISCIPLINA 
                                                      AND P2.ANO = N.ANO 
                                                      AND P2.SEMESTRE = N.SEMESTRE 
                                                      AND P2.PROVA = N.PROVA 
                                                      AND SUBSTRING(P2.TURMA, 1, LEN(@TURMA_NOVA)) = @TURMA_NOVA 
                                                      AND SUBSTRING(P2.TURMA, LEN( @TURMA_NOVA ) + 1, 
                                                          LEN(P2.TURMA)) = SUBSTRING(N.TURMA, LEN( @TURMA_NOVA ) + 1,LEN(N.TURMA)))  ";

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@TURMA_ATUAL", turmaAtual);
            contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
            contextQuery.Parameters.Add("@TURMA_NOVA", turmaNova);

            dt = ctx.GetDataTable(contextQuery);

            return dt;
        }
    }
}