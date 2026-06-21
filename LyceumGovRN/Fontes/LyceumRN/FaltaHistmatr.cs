using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Entidades;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN
{
    public class FaltaHistmatr : RNBase
    {
        public void InserePorMatriculaParaFechamento(DataContext ctx, LyHistMatricula histMatricula)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" INSERT  INTO LY_FALTA_HISTMATR
                        ( ALUNO ,
                          ORDEM ,
                          ANO ,
                          SEMESTRE ,
                          DISCIPLINA ,
                          FREQ_ID ,
                          FALTAS,
                          TURMA
                        )
                        SELECT  HM.ALUNO ,
                                HM.ORDEM ,
                                FA.ANO ,
                                FA.PERIODO ,
                                FA.DISCIPLINA ,
                                FA.FREQ ,
                                FA.FALTAS - ISNULL(FA.FALTAS_COMPENSADAS, 0),
                                FA.TURMA
                        FROM    LY_FALTA FA ( NOLOCK )
                                INNER JOIN LY_FREQ FR ( NOLOCK ) ON FA.ANO = FR.ANO
                                                                    AND FA.DISCIPLINA = FR.DISCIPLINA
                                                                    AND FA.PERIODO = FR.PERIODO
                                                                    AND FA.TURMA = FR.TURMA
                                                                    AND FA.FREQ = FR.FREQ
                                INNER JOIN LY_HISTMATRICULA HM ON FA.ALUNO = HM.ALUNO
                                                                  AND FA.ANO = HM.ANO
                                                                  AND FA.PERIODO = HM.SEMESTRE
                                                                  AND FA.DISCIPLINA = HM.DISCIPLINA
                                                                  AND FA.TURMA = HM.TURMA
                        WHERE   FA.ALUNO = @ALUNO
                                AND FA.DISCIPLINA = @DISCIPLINA
                                AND FA.TURMA = @TURMA
                                AND FA.ANO = @ANO
                                AND FA.PERIODO = @PERIODO
                                AND NOT EXISTS ( SELECT *
                                                 FROM   LY_FALTA_HISTMATR HFA
                                                 WHERE  HFA.ALUNO = FA.ALUNO
                                                        AND HFA.ANO = FA.ANO
                                                        AND HFA.SEMESTRE = FA.PERIODO
                                                        AND HFA.DISCIPLINA = FA.DISCIPLINA
                                                        AND HFA.FREQ_ID = FA.FREQ
                                                        AND HFA.ORDEM = HM.ORDEM ) ";

                contextQuery.Parameters.Add("@ALUNO", histMatricula.Aluno);
                contextQuery.Parameters.Add("@DISCIPLINA", histMatricula.Disciplina);
                contextQuery.Parameters.Add("@TURMA", histMatricula.Turma);
                contextQuery.Parameters.Add("@ANO", histMatricula.Ano);
                contextQuery.Parameters.Add("@PERIODO", histMatricula.Semestre);

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

        public void AtualizaFrequencias(string usuario, ICollection<LyFaltaHistmatr> faltas, string aulasDadas, string aulasPrevistas, decimal subPeriodo, IList<ContextQuery> contextQueries, DataContext ctx)
        {
            try
            {
                LyFaltaHistmatr primeiraFalta = faltas.First();
                string frequencia = primeiraFalta.FreqId;
                string disciplina = primeiraFalta.Disciplina;
                string turma = primeiraFalta.Turma;
                int ano = Convert.ToInt32(primeiraFalta.Ano);
                int periodo = Convert.ToInt32(primeiraFalta.Semestre);
                IDictionary<string, LyFaltaHistmatr> faltasSalvas = this.ListaFaltasSalvas(ctx, ano, periodo, turma, disciplina, frequencia);

                if (faltas == null || faltas.Count == 0 || faltas.Select(n => new { n.FreqId, n.Turma, n.Ano, n.Semestre, n.Disciplina }).Distinct().Count() != 1)
                {
                    //return new RetValue(false, "Necessário enviar as faltas para atualizar as faltas!", null);
                    throw new Exception("ERRO_VALIDACAO:Necessário enviar as faltas para atualizar as faltas!");
                }

                if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(aulasDadas) || string.IsNullOrEmpty(aulasPrevistas))
                {
                    //return new RetValue(false, "Necessário enviar os dados restantes para atualizar as notas!", null);
                    throw new Exception("ERRO_VALIDACAO:Necessário enviar os dados restantes para atualizar as notas!");
                }


                // Atualização das aulas dadas e previstas
                contextQueries.Add(Frequencia.AtualizarAulas(aulasDadas, aulasPrevistas, disciplina, turma, ano, periodo, frequencia));

                foreach (LyFaltaHistmatr falta in faltas)
                {
                    int numeroFaltas = falta.Faltas.HasValue ? Convert.ToInt32(falta.Faltas.Value) : -1;
                    int ordem = Convert.ToInt32(falta.Ordem);

                    if (faltasSalvas.ContainsKey(falta.Aluno))
                    {
                        var faltaSalva = faltasSalvas[falta.Aluno];

                        if (numeroFaltas == -1)
                        {
                            var nomeAluno = ConsultarCampo("SELECT nome_compl FROM ly_aluno (NOLOCK) WHERE aluno = ?", falta.Aluno);

                            //return new RetValue(false, string.Empty, new ErrorList("Não é permitido remoção das faltas do aluno " + nomeAluno + "."));
                            throw new Exception("ERRO_VALIDACAO:Não é permitido remoção das faltas do aluno " + nomeAluno + ".");
                        }

                        if (faltaSalva.Faltas != numeroFaltas)
                        {
                            contextQueries.Add(this.Atualiza(numeroFaltas, falta.Aluno, disciplina, turma, ordem, ano, periodo, frequencia));
                        }
                    }
                    else
                    {
                        if (numeroFaltas != -1)
                        {
                            contextQueries.Add(this.Insere(numeroFaltas, falta.Aluno, disciplina, turma, ordem, ano, periodo, frequencia));
                        }
                    }
                }
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

        private IDictionary<string, LyFaltaHistmatr> ListaFaltasSalvas(DataContext dataContext, int ano, int semestre, string turma, string disciplina, string frequencia)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();
                ICollection<LyFaltaHistmatr> faltas = null;

                contextQuery.Command = @" SELECT  F.ALUNO ,
                                        F.FALTAS
                                FROM    DBO.LY_FALTA_HISTMATR F
                                        INNER JOIN DBO.LY_HISTMATRICULA M ON F.ALUNO = M.ALUNO
                                                                             AND F.ORDEM = M.ORDEM
                                                                             AND F.ANO = M.ANO
                                                                             AND F.SEMESTRE = M.SEMESTRE
                                                                             AND F.DISCIPLINA = M.DISCIPLINA
                                WHERE   F.FREQ_ID = @FREQUENCIA
                                        AND F.ANO = @ANO
                                        AND F.SEMESTRE = @SEMESTRE
                                        AND M.TURMA = @TURMA
                                        AND F.DISCIPLINA = @DISCIPLINA ";

                contextQuery.Parameters.Add("@FREQUENCIA", frequencia);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);

                faltas = dataContext.TryToBindEntities<LyFaltaHistmatr>(contextQuery);

                if (faltas == null)
                {
                    return new Dictionary<string, LyFaltaHistmatr>();
                }

                return faltas.ToDictionary(x => x.Aluno, x => x);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ContextQuery Atualiza(int faltas, string aluno, string disciplina, string turma, int ordem, int ano, int periodo, string frequencia)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @"UPDATE  LY_FALTA_HISTMATR
                            SET     FALTAS = @FALTAS ,
                                    TURMA = @TURMA
                            WHERE   ALUNO = @ALUNO
                                    AND DISCIPLINA = @DISCIPLINA
                                    AND ORDEM = @ORDEM
                                    AND ANO = ANO
                                    AND SEMESTRE = @PERIODO
                                    AND FREQ_ID = @FREQ ";

                contextQuery.Parameters.Add("@FALTAS", TechneDbType.T_DECIMAL_MEDIO_PRECISO, faltas);
                contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
                contextQuery.Parameters.Add("@DISCIPLINA", TechneDbType.T_CODIGO, disciplina);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
                contextQuery.Parameters.Add("@PERIODO", TechneDbType.T_SEMESTRE2, periodo);
                contextQuery.Parameters.Add("@FREQ", TechneDbType.T_FALTA, frequencia);
                contextQuery.Parameters.Add("@ORDEM", ordem);

                return contextQuery;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ContextQuery Insere(int faltas, string aluno, string disciplina, string turma, int ordem, int ano, int periodo, string frequencia)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" INSERT  INTO dbo.LY_FALTA_HISTMATR
                                    ( ALUNO ,
                                      ORDEM ,
                                      ANO ,
                                      SEMESTRE ,
                                      DISCIPLINA ,
                                      FREQ_ID ,
                                      FALTAS ,
                                      TURMA
                                    )
                            VALUES  ( @ALUNO ,
                                      @ORDEM ,
                                      @ANO ,
                                      @SEMESTRE ,
                                      @DISCIPLINA ,
                                      @FREQ_ID ,
                                      @FALTAS ,
                                      @TURMA
                                    ) ";

                contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
                contextQuery.Parameters.Add("@ORDEM", ordem);
                contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
                contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, periodo);
                contextQuery.Parameters.Add("@DISCIPLINA", TechneDbType.T_CODIGO, disciplina);
                contextQuery.Parameters.Add("@FREQ_ID", TechneDbType.T_FALTA, frequencia);
                contextQuery.Parameters.Add("@FALTAS", TechneDbType.T_DECIMAL_MEDIO_PRECISO, faltas);
                contextQuery.Parameters.Add("@TURMA", turma);

                return contextQuery;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
