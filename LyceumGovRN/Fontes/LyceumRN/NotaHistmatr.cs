using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Entidades;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN
{
    public class NotaHistmatr : RNBase
    {
        public void InserePorMatriculaParaFechamento(DataContext ctx, LyHistMatricula histMatricula)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" INSERT  INTO LY_NOTA_HISTMATR
                                        ( ALUNO ,
                                          ORDEM ,
                                          ANO ,
                                          SEMESTRE ,
                                          DISCIPLINA ,
                                          NOTA_ID ,
                                          CONCEITO ,
                                          DATA ,
                                          OBSERVACAO ,
                                          COMPARECEU ,
                                          NOTAPROVA ,
                                          NOTARECUPERACAO ,
                                          MOTIVOSEMNOTAID ,
                                          RECUPERACAOPARALELA ,
                                          SEMAVALIACAO ,
                                          TURMA
                                        )
                                SELECT  HM.ALUNO ,
                                        HM.ORDEM ,
                                        HM.ANO ,
                                        HM.SEMESTRE ,
                                        HM.DISCIPLINA ,
                                        N.PROVA ,
                                        N.CONCEITO ,
                                        N.DATA ,
                                        N.FORMULARIO ,
                                        N.COMPARECEU ,
                                        N.NOTAPROVA ,
                                        N.NOTARECUPERACAO ,
                                        N.MOTIVOSEMNOTAID ,
                                        N.RECUPERACAO_PARALELA ,
                                        N.SEM_AVALIACAO ,
                                        N.TURMA
                                FROM    LY_NOTA N ( NOLOCK )
                                        INNER JOIN LY_HISTMATRICULA HM ( NOLOCK ) ON N.ALUNO = HM.ALUNO
                                                                                     AND N.ANO = HM.ANO
                                                                                     AND N.SEMESTRE = HM.SEMESTRE
                                                                                     AND N.DISCIPLINA = HM.DISCIPLINA
                                                                                     AND N.TURMA = HM.TURMA
                                                                                     AND HM.ORDEM = @ORDEM
                                WHERE   N.ALUNO = @ALUNO
                                        AND N.DISCIPLINA = @DISCIPLINA
                                        AND N.TURMA = @TURMA
                                        AND N.ANO = @ANO
                                        AND N.SEMESTRE = @SEMESTRE                                       
                                        AND NOT EXISTS ( SELECT *
                                                         FROM   LY_NOTA_HISTMATR NT
                                                         WHERE  NT.ALUNO = N.ALUNO
                                                                AND NT.DISCIPLINA = N.DISCIPLINA
                                                                AND NT.ANO = N.ANO
                                                                AND NT.SEMESTRE = N.SEMESTRE
                                                                AND NT.NOTA_ID = N.PROVA
                                                                AND NT.ORDEM = HM.ORDEM ) ";

                contextQuery.Parameters.Add("@ALUNO", histMatricula.Aluno);
                contextQuery.Parameters.Add("@DISCIPLINA", histMatricula.Disciplina);
                contextQuery.Parameters.Add("@TURMA", histMatricula.Turma);
                contextQuery.Parameters.Add("@ANO", histMatricula.Ano);
                contextQuery.Parameters.Add("@SEMESTRE", histMatricula.Semestre);
                contextQuery.Parameters.Add("@ORDEM", histMatricula.Ordem);

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

        public void AtualizaNotas(ICollection<LyNotaHistmatr> notas, string usuario, decimal subPeriodo, IList<ContextQuery> contextQueries, DataContext ctx)
        {
            try
            {
                LyNotaHistmatr primeiraNota = notas.First();
                string disciplina = primeiraNota.Disciplina;
                string turma = primeiraNota.Turma;
                int ano = Convert.ToInt32(primeiraNota.Ano);
                int periodo = Convert.ToInt32(primeiraNota.Semestre);
                string prova = primeiraNota.NotaId;                
                
                IDictionary<string, LyNotaHistmatr> notasSalvas = this.ListaNotasSalvas(ctx, ano, periodo, turma, disciplina, prova);

                if (notas == null || notas.Count == 0 || notas.Select(n => new { n.NotaId, n.Turma, n.Ano, n.Semestre, n.Disciplina }).Distinct().Count() != 1)
                {
                    //return new RetValue(false, "Necessário enviar as notas para atualizar as notas!", null);
                    throw new Exception("ERRO_VALIDACAO:Necessário enviar as notas para atualizar as notas!");
                }

                // Atualização das notas
                foreach (LyNotaHistmatr nota in notas)
                {
                    nota.Compareceu = "S";
                    nota.Data = DateTime.Today;
                    int ordem = Convert.ToInt32(nota.Ordem);

                    if (!string.IsNullOrEmpty(nota.Conceito))
                    {
                        decimal conceito;

                        if (!decimal.TryParse(nota.Conceito, out conceito))
                        {
                            string nomeAluno = ConsultarCampo("SELECT nome_compl FROM ly_aluno (NOLOCK) WHERE aluno = ?", nota.Aluno);

                            throw new Exception("ERRO_VALIDACAO:Existem valores de nota inválidos.<br/> - Aluno: " + nomeAluno + "<br/> - Nota: " + nota.Conceito);
                        }

                        nota.Conceito = conceito.ToString().Replace(".", ",");
                    }

                    if (notasSalvas.ContainsKey(nota.Aluno))
                    {
                        var notaSalva = notasSalvas[nota.Aluno];

                        if (nota.SemAvaliacao == "N" && string.IsNullOrEmpty(nota.Conceito))
                        {
                            contextQueries.Add(this.Remove(nota));
                        }
                        else
                        {
                            if (notaSalva.Conceito != nota.Conceito
                                || notaSalva.RecuperacaoParalela != nota.RecuperacaoParalela
                                || notaSalva.SemAvaliacao != nota.SemAvaliacao
                                || notaSalva.NotaProva != nota.NotaProva
                                || notaSalva.NotaRecuperacao != nota.NotaRecuperacao
                                || notaSalva.MotivoSemNotaId != nota.MotivoSemNotaId)
                            {
                                contextQueries.Add(this.Atualiza(nota));
                            }
                        }
                    }
                    else
                    {
                        contextQueries.Add(this.Insere(nota));
                    }
                }

                //Como apenas podem ser lançadas notas para todos os alunos da turma, Atualiza flag de lançamento completo do quadro = 'S'
                contextQueries.Add(this.AtualizaLancamentoComoCompletoPor(ano, periodo, turma, disciplina, prova));
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();

                //Trata mensagens de erros de validacao no processo de enturmacao
                if (Convert.ToString(ex.Message).Contains("ERRO_VALIDACAO:"))
                {
                    mensagem = Convert.ToString(ex.Message).Replace("ERRO_VALIDACAO:", string.Empty);
                }
                else
                {
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
                }

                throw new Exception(mensagem);
            }
        }

        private IDictionary<string, LyNotaHistmatr> ListaNotasSalvas(DataContext dataContext, int ano, int periodo, string turma, string disciplina, string prova)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();
                ICollection<LyNotaHistmatr> notas = null;

                contextQuery.Command = @" SELECT  N.ALUNO ,
                                N.CONCEITO ,
                                REPLACE(N.NOTAPROVA, '.', ',') AS NOTAPROVA ,
                                REPLACE(N.NOTARECUPERACAO, '.', ',') AS NOTARECUPERACAO ,
                                N.RECUPERACAOPARALELA ,
                                N.SEMAVALIACAO ,
                                N.ORDEM ,
                                N.MOTIVOSEMNOTAID
                        FROM    DBO.LY_NOTA_HISTMATR N
                                INNER JOIN DBO.LY_HISTMATRICULA M ON N.ALUNO = M.ALUNO
                                                                     AND N.ORDEM = M.ORDEM
                                                                     AND N.ANO = M.ANO
                                                                     AND N.SEMESTRE = M.SEMESTRE
                                                                     AND N.DISCIPLINA = M.DISCIPLINA
                        WHERE   N.NOTA_ID = @PROVA
                                AND N.ANO = @ANO
                                AND N.SEMESTRE = @PERIODO
                                AND M.TURMA = @TURMA
                                AND N.DISCIPLINA = @DISCIPLINA ";

                contextQuery.Parameters.Add("@PROVA", prova);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);

                notas = dataContext.TryToBindEntities<LyNotaHistmatr>(contextQuery);

                if (notas == null)
                {
                    return new Dictionary<string, LyNotaHistmatr>();
                }

                return notas.ToDictionary(x => x.Aluno, x => x);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ContextQuery Remove(LyNotaHistmatr nota)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" DELETE  dbo.LY_NOTA_HISTMATR
                            WHERE   ALUNO = @ALUNO
                                    AND DISCIPLINA = @DISCIPLINA
                                    AND ORDEM = @ORDEM
                                    AND ANO = @ANO
                                    AND SEMESTRE = @SEMESTRE
                                    AND NOTA_ID = @PROVA ";

                contextQuery.Parameters.Add("@ALUNO", nota.Aluno);
                contextQuery.Parameters.Add("@DISCIPLINA", nota.Disciplina);
                contextQuery.Parameters.Add("@ORDEM", nota.Ordem);
                contextQuery.Parameters.Add("@ANO", nota.Ano);
                contextQuery.Parameters.Add("@SEMESTRE", nota.Semestre);
                contextQuery.Parameters.Add("@PROVA", nota.NotaId);

                return contextQuery;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ContextQuery Atualiza(LyNotaHistmatr nota)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" UPDATE  LY_NOTA_HISTMATR
                            SET     COMPARECEU = @COMPARECEU ,
                                    TURMA = @TURMA ,
                                    CONCEITO = @CONCEITO ,
                                    DATA = @DATA ,
                                    RECUPERACAOPARALELA = @RECUPERACAO_PARALELA ,
                                    SEMAVALIACAO = @SEM_AVALIACAO ,
                                    NOTAPROVA = @NOTAPROVA ,
                                    NOTARECUPERACAO = @NOTARECUPERACAO ,
                                    MOTIVOSEMNOTAID = @MOTIVOSEMNOTAID
                            WHERE   ALUNO = @ALUNO
                                    AND DISCIPLINA = @DISCIPLINA
                                    AND ORDEM = @ORDEM
                                    AND ANO = @ANO
                                    AND SEMESTRE = @SEMESTRE
                                    AND NOTA_ID = @PROVA ";

                contextQuery.Parameters.Add("@COMPARECEU", TechneDbType.T_SIMNAO, nota.Compareceu);
                contextQuery.Parameters.Add("@TURMA", nota.Turma);
                contextQuery.Parameters.Add("@CONCEITO", TechneDbType.T_ALFASMALL, nota.Conceito);
                contextQuery.Parameters.Add("@DATA", TechneDbType.T_DATA, nota.Data);
                contextQuery.Parameters.Add("@RECUPERACAO_PARALELA", nota.RecuperacaoParalela);
                contextQuery.Parameters.Add("@SEM_AVALIACAO", nota.SemAvaliacao);
                contextQuery.Parameters.Add("@NOTAPROVA", nota.NotaProva);
                contextQuery.Parameters.Add("@NOTARECUPERACAO", nota.NotaRecuperacao);
                contextQuery.Parameters.Add("@MOTIVOSEMNOTAID", nota.MotivoSemNotaId);
                contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, nota.Aluno);
                contextQuery.Parameters.Add("@DISCIPLINA", TechneDbType.T_CODIGO, nota.Disciplina);
                contextQuery.Parameters.Add("@ORDEM", nota.Ordem);
                contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, nota.Ano);
                contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, nota.Semestre);
                contextQuery.Parameters.Add("@PROVA", TechneDbType.T_PROVA10, nota.NotaId);

                return contextQuery;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ContextQuery Insere(LyNotaHistmatr nota)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" INSERT  INTO dbo.LY_NOTA_HISTMATR
                                ( ALUNO ,
                                  ORDEM ,
                                  ANO ,
                                  SEMESTRE ,
                                  DISCIPLINA ,
                                  NOTA_ID ,
                                  CONCEITO ,
                                  DATA ,
                                  OBSERVACAO ,
                                  COMPARECEU ,
                                  RECUPERACAOPARALELA ,
                                  SEMAVALIACAO ,
                                  TURMA ,
                                  NOTAPROVA ,
                                  NOTARECUPERACAO ,
                                  MOTIVOSEMNOTAID
                                )
                        VALUES  ( @ALUNO ,
                                  @ORDEM ,
                                  @ANO ,
                                  @SEMESTRE ,
                                  @DISCIPLINA ,
                                  @PROVA ,
                                  @CONCEITO ,
                                  @DATA ,
                                  @OBSERVACAO ,
                                  @COMPARECEU ,
                                  @RECUPERACAOPARALELA ,
                                  @SEMAVALIACAO ,
                                  @TURMA ,
                                  @NOTAPROVA ,
                                  @NOTARECUPERACAO ,
                                  @MOTIVOSEMNOTAID
                                ) ";

                contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, nota.Aluno);
                contextQuery.Parameters.Add("@ORDEM", nota.Ordem);
                contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, nota.Ano);
                contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, nota.Semestre);
                contextQuery.Parameters.Add("@DISCIPLINA", TechneDbType.T_CODIGO, nota.Disciplina);
                contextQuery.Parameters.Add("@PROVA", TechneDbType.T_PROVA10, nota.NotaId);
                contextQuery.Parameters.Add("@CONCEITO", TechneDbType.T_ALFASMALL, nota.Conceito);
                contextQuery.Parameters.Add("@DATA", TechneDbType.T_DATA, nota.Data);
                contextQuery.Parameters.Add("@OBSERVACAO", nota.Observacao);
                contextQuery.Parameters.Add("@COMPARECEU", TechneDbType.T_SIMNAO, nota.Compareceu);
                contextQuery.Parameters.Add("@RECUPERACAOPARALELA", nota.RecuperacaoParalela);
                contextQuery.Parameters.Add("@SEMAVALIACAO", nota.SemAvaliacao);
                contextQuery.Parameters.Add("@TURMA", nota.Turma);
                contextQuery.Parameters.Add("@NOTAPROVA", nota.NotaProva);
                contextQuery.Parameters.Add("@NOTARECUPERACAO", nota.NotaRecuperacao);
                contextQuery.Parameters.Add("@MOTIVOSEMNOTAID", nota.MotivoSemNotaId);

                return contextQuery;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ContextQuery AtualizaLancamentoComoCompletoPor(int ano, int periodo, string turma, string disciplina, string prova)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" UPDATE  LY_PROVA
                        SET     COMPLEMENTO = 'S'
                        WHERE   DISCIPLINA = @DISCIPLINA
                                AND TURMA = @TURMA
                                AND ANO = @ANO
                                AND SEMESTRE = @PERIODO
                                AND PROVA = @PROVA ";

                contextQuery.Parameters.Add("@DISCIPLINA", TechneDbType.T_CODIGO, disciplina);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
                contextQuery.Parameters.Add("@PERIODO", TechneDbType.T_SEMESTRE2, periodo);
                contextQuery.Parameters.Add("@PROVA", TechneDbType.T_PROVA10, prova);

                return contextQuery;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
