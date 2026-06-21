using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.RN
{
    public class DeclaracaoSemNota
    {
        public static DataTable ListaMotivoSemNotaId()
        {
            DataTable listaMotivoSemNotaId = new DataTable();

            listaMotivoSemNotaId.Columns.Add("DESCR");
            listaMotivoSemNotaId.Columns.Add("ITEM");

            listaMotivoSemNotaId.Rows.Add("<Selecione>", "Selecione");
            listaMotivoSemNotaId.Rows.Add("Aluno em Progressão Parcial (Dependência) não apresentou trabalho", "0");
            listaMotivoSemNotaId.Rows.Add("Afastamento Médico / Maternidade / Serviço Militar", "1");
            listaMotivoSemNotaId.Rows.Add("Outros", "2");

            return listaMotivoSemNotaId;
        }

        public ContextQuery InsereComLancamentoNotas(Entidades.DeclaracaoSemNota declaracaoSemNota, LyNota nota)
        {
            var contextQuery = new ContextQuery
            {
                Command = @" INSERT  INTO dbo.DECLARACAOSEMNOTA
                            ( NOTAID ,
                              TIPODECLARACAOSEMNOTAID ,
                              MATRICULA ,
                              DATACADASTRO
                            )
                            ( SELECT    NOTAID ,
                                        @TIPODECLARACAOSEMNOTAID ,
                                        @MATRICULA ,
                                        GETDATE()
                              FROM      dbo.LY_NOTA (NOLOCK)
                              WHERE     ALUNO = @ALUNO
                                        AND DISCIPLINA = @DISCIPLINA
                                        AND TURMA = @TURMA
                                        AND ANO = @ANO
                                        AND SEMESTRE = @SEMESTRE
                                        AND PROVA = @PROVA
                            ) "
            };

            contextQuery.Parameters.Add("@TIPODECLARACAOSEMNOTAID", declaracaoSemNota.TipoDeclaracaoSemNotaId);
            contextQuery.Parameters.Add("@MATRICULA", declaracaoSemNota.Matricula);
            contextQuery.Parameters.Add("@ALUNO", nota.Aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", nota.Disciplina);
            contextQuery.Parameters.Add("@TURMA", nota.Turma);
            contextQuery.Parameters.Add("@ANO", nota.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", nota.Semestre);
            contextQuery.Parameters.Add("@PROVA", nota.Prova);

            return contextQuery;
        }

        public ContextQuery RemoveComLancamentoNotas(LyNota nota)
        {
            var contextQuery = new ContextQuery
            {
                Command = @" DELETE  dbo.DECLARACAOSEMNOTA
                            WHERE   NOTAID = ( SELECT TOP 1
                                                        NOTAID
                                               FROM     dbo.LY_NOTA (NOLOCK)
                                               WHERE    ALUNO = @ALUNO
                                                        AND DISCIPLINA = @DISCIPLINA
                                                        AND TURMA = @TURMA
                                                        AND ANO = @ANO
                                                        AND SEMESTRE = @SEMESTRE
                                                        AND PROVA = @PROVA ) "
            };

            contextQuery.Parameters.Add("@ALUNO", nota.Aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", nota.Disciplina);
            contextQuery.Parameters.Add("@TURMA", nota.Turma);
            contextQuery.Parameters.Add("@ANO", nota.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", nota.Semestre);
            contextQuery.Parameters.Add("@PROVA", nota.Prova);

            return contextQuery;
        }

        public void Remove(DataContext ctx, string aluno, string disciplina, string turma, decimal ano, decimal semestre)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" DELETE  dbo.DECLARACAOSEMNOTA
                        WHERE   NOTAID IN ( SELECT DISTINCT
                                                    NOTAID
                                            FROM    dbo.LY_NOTA (NOLOCK)
                                            WHERE   ALUNO = @ALUNO
                                                    AND DISCIPLINA = @DISCIPLINA
                                                    AND TURMA = @TURMA
                                                    AND ANO = @ANO
                                                    AND SEMESTRE = @SEMESTRE ) ";

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


        public ContextQuery RemoveComLancamentoNotasDoUsuario(LyNota nota, string matricula)
        {
            var contextQuery = new ContextQuery
            {
                Command = @" DELETE  dbo.DECLARACAOSEMNOTA
                            WHERE   NOTAID = ( SELECT TOP 1
                                                        NOTAID
                                               FROM     dbo.LY_NOTA (NOLOCK)
                                               WHERE    ALUNO = @ALUNO
                                                        AND DISCIPLINA = @DISCIPLINA
                                                        AND TURMA = @TURMA
                                                        AND ANO = @ANO
                                                        AND SEMESTRE = @SEMESTRE
                                                        AND PROVA = @PROVA )
                            AND MATRICULA = @MATRICULA "
            };

            contextQuery.Parameters.Add("@ALUNO", nota.Aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", nota.Disciplina);
            contextQuery.Parameters.Add("@TURMA", nota.Turma);
            contextQuery.Parameters.Add("@ANO", nota.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", nota.Semestre);
            contextQuery.Parameters.Add("@PROVA", nota.Prova);
            contextQuery.Parameters.Add("@MATRICULA", matricula);

            return contextQuery;
        }

        internal void TransfereDeclaracaoSemNotaParaNovoNotaId(string aluno, string disciplina, string prova, string turmaAnterior, string turmaNova, int? ano, int? periodo, List<ContextQuery> listaContextQueries)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  WITH CTE
                                         AS 
                                          ( SELECT D.NOTAID,
                                                   NEW.NOTAID AS _NOTAIDDESTINO
                                            FROM   DECLARACAOSEMNOTA D
                                                   JOIN LY_NOTA OLD ON D.NOTAID = OLD.NOTAID
                                                   JOIN LY_NOTA NEW ON NEW.ALUNO = @ALUNO
                                                                   AND NEW.DISCIPLINA = @DISCIPLINA
                                                                   AND NEW.TURMA = @TURMA
                                                                   AND NEW.ANO = @ANO
                                                                   AND NEW.SEMESTRE = @SEMESTRE
                                                                   AND NEW.PROVA = @PROVA
                                            WHERE  OLD.ALUNO = @ALUNO
                                                   AND OLD.DISCIPLINA = @DISCIPLINA
                                                   AND OLD.TURMA = @TURMAOLD
                                                   AND OLD.ANO = @ANO
                                                   AND OLD.SEMESTRE = @SEMESTRE
                                                   AND OLD.PROVA = @PROVA
                                          )
                                     UPDATE CTE 
                                     SET    CTE.NOTAID = _NOTAIDDESTINO";
            
            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
            contextQuery.Parameters.Add("@TURMAOLD", turmaAnterior);
            contextQuery.Parameters.Add("@TURMA", turmaNova);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", periodo);
            contextQuery.Parameters.Add("@PROVA", prova);

            listaContextQueries.Add(contextQuery);
        }

        public void Remove(DataContext ctx, string aluno, string turma, decimal ano, decimal semestre)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" DELETE  dbo.DECLARACAOSEMNOTA
                        WHERE   NOTAID IN ( SELECT DISTINCT
                                                    NOTAID
                                            FROM    dbo.LY_NOTA (NOLOCK)
                                            WHERE   ALUNO = @ALUNO
                                                    AND TURMA = @TURMA
                                                    AND ANO = @ANO
                                                    AND SEMESTRE = @SEMESTRE ) ";

                contextQuery.Parameters.Add("@ALUNO", aluno);
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
    }
}
