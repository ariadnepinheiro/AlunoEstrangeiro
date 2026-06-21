using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN
{
    public class HCurrAluno
    {
        public void Insere(RN.Entidades.LyHCurrAluno hCurrAluno, List<ContextQuery> listaContextQuery)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"INSERT INTO dbo.LY_H_CURR_ALUNO
                                            (
                                              CURSO,
                                              TURNO,
                                              CURRICULO,
                                              ALUNO,
                                              DT_TRANS,
                                              SERIEH,
                                              ANO,
                                              PERIODO,
                                              TURMA_PREF,
                                              UNIDADE_FISICA,
                                              MOTIVO,
                                              UNIDADE_ENSINO
                                            )
                                     VALUES (
                                              @CURSO,
                                              @TURNO,
                                              @CURRICULO,
                                              @ALUNO,
                                              GETDATE(),
                                              @SERIEH,
                                              @ANO,
                                              @PERIODO,
                                              @TURMA_PREF,
                                              @UNIDADE_FISICA,
                                              @MOTIVO,
                                              @UNIDADE_ENSINO
                                            )";

            contextQuery.Parameters.Add("@CURSO", hCurrAluno.Curso);
            contextQuery.Parameters.Add("@TURNO", hCurrAluno.Turno);
            contextQuery.Parameters.Add("@CURRICULO", hCurrAluno.Curriculo);
            contextQuery.Parameters.Add("@ALUNO", hCurrAluno.Aluno);
            contextQuery.Parameters.Add("@SERIEH", hCurrAluno.Serie);
            contextQuery.Parameters.Add("@ANO", hCurrAluno.Ano);
            contextQuery.Parameters.Add("@PERIODO", hCurrAluno.Periodo);
            contextQuery.Parameters.Add("@TURMA_PREF", hCurrAluno.Turma);
            contextQuery.Parameters.Add("@UNIDADE_FISICA", hCurrAluno.UnidadeFisica);
            contextQuery.Parameters.Add("@MOTIVO", hCurrAluno.Motivo);
            contextQuery.Parameters.Add("@UNIDADE_ENSINO", hCurrAluno.UnidadeEnsino);

            listaContextQuery.Add(contextQuery);
        }

        internal void AtualizaDataReaberturaPor(string aluno, List<ContextQuery> listaContextQuery)
        {
            ContextQuery contextQuery = new ContextQuery(
                    @"UPDATE  LY_H_CURSOS_CONCL
                      SET     DT_REABERTURA = GETDATE()
                      WHERE   ALUNO = @ALUNO
                              AND DT_REABERTURA IS NULL",
                new ContextQueryParameter("@ALUNO", aluno));

            listaContextQuery.Add(contextQuery);
        }
    }
}
