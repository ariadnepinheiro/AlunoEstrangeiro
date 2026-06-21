using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN
{
    public class AlunoConcomitante : RNBase
    {
        public const string Liberado = "Liberado";

        public const string Cancelado = "Cancelado";

        public static DataTable Listar( string censo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @" SELECT DISTINCT
                                ( SELECT TOP 1
                                            ID_ALUNO_CONCOMITANTE
                                  FROM      TCE_ALUNO_CONCOMITANTE AC2
                                  WHERE     AC.ALUNO = AC2.ALUNO
                                            AND AC.ANO = AC2.ANO
                                            AND AC.PERIODO = AC2.PERIODO
                                  ORDER BY  AC2.DT_CADASTRO DESC
                                ) AS ID_ALUNO_CONCOMITANTE,
                                AC.CENSO,
                                AC.ALUNO,
                                PE.NOME_COMPL AS 'NOME_ALUNO',
                                AC.ANO,
                                AC.PERIODO,
                                ( SELECT TOP 1
                                            STATUS
                                  FROM      TCE_ALUNO_CONCOMITANTE AC2
                                  WHERE     AC.ALUNO = AC2.ALUNO
                                            AND AC.ANO = AC2.ANO
                                            AND AC.PERIODO = AC2.PERIODO
                                  ORDER BY  AC2.DT_CADASTRO DESC
                                ) AS 'STATUS',
                                CASE WHEN ( SELECT TOP 1
                                                    STATUS
                                            FROM    TCE_ALUNO_CONCOMITANTE AC2
                                            WHERE   AC.ALUNO = AC2.ALUNO
                                                    AND AC.ANO = AC2.ANO
                                                    AND AC.PERIODO = AC2.PERIODO
                                            ORDER BY AC2.DT_CADASTRO DESC
                                          ) = 'Cancelado' THEN '1'
                                     ELSE '0'
                                END 'CANCELADO',
                                CASE WHEN M.TURMA IS NOT NULL THEN 'true'
                                     ELSE 'false'
                                END 'ENTURMADO'
                        FROM    TCE_ALUNO_CONCOMITANTE AC
                                INNER JOIN LY_ALUNO A ON AC.ALUNO = A.ALUNO
                                INNER JOIN LY_PESSOA PE (NOLOCK)  ON PE.PESSOA = A.PESSOA
                                INNER JOIN DBO.LY_MATRICULA M2 ON AC.ALUNO = M2.ALUNO
                                                                  AND AC.ANO = M2.ANO                                                                  
                                                                  AND M2.SIT_MATRICULA = 'Matriculado'
                                                                  AND ( M2.CONCOMITANTE <> 'S'
                                                                        OR m2.CONCOMITANTE IS NULL
                                                                      )
                                LEFT JOIN DBO.LY_MATRICULA M ON AC.ALUNO = M.ALUNO
                                                                AND AC.ANO = M.ANO
                                                                AND AC.PERIODO = M.SEMESTRE
                                                                AND M.SIT_MATRICULA = 'Matriculado'
                                                                AND M.CONCOMITANTE = 'S'
                            WHERE AC.CENSO = @CENSO
                            ORDER BY ID_ALUNO_CONCOMITANTE"
                };

                contextQuery.Parameters.Add("@CENSO", censo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static TceAlunoConcomitante Carregar(string aluno, int ano, int periodo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery =
                    new ContextQuery(
                        @" SELECT TOP 1
                                    *
                            FROM    TCE_ALUNO_CONCOMITANTE
                            WHERE   ALUNO = @ALUNO
                                    AND ANO = @ANO
                                    AND PERIODO = @PERIODO
                            ORDER BY DT_CADASTRO DESC ");
                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                return ctx.TryToBindEntity<TceAlunoConcomitante>(contextQuery);
            }
        }

        public static TceAlunoConcomitante Carregar(string aluno, int ano)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery =
                    new ContextQuery(
                        @" SELECT TOP 1
                                    *
                            FROM    TCE_ALUNO_CONCOMITANTE
                            WHERE   ALUNO = @ALUNO
                                    AND ANO = @ANO
                            ORDER BY DT_CADASTRO DESC ");
                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);

                return ctx.TryToBindEntity<TceAlunoConcomitante>(contextQuery);
            }
        }

        public static void Salvar(TceAlunoConcomitante alunoConcomitante)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    if (alunoConcomitante.Status == Cancelado)
                    {
                        var ctxQuery = new ContextQuery(
                        @" SELECT TOP 1
                                    CENSO
                            FROM    DBO.TCE_ALUNO_CONCOMITANTE
                            WHERE   ALUNO = @ALUNO
                                    AND ANO = @ANO
                                    AND PERIODO = @PERIODO
                            ORDER BY DT_CADASTRO DESC ");

                        ctxQuery.Parameters.Add("@ALUNO", alunoConcomitante.Aluno);
                        ctxQuery.Parameters.Add("@ANO", alunoConcomitante.Ano);
                        ctxQuery.Parameters.Add("@PERIODO", alunoConcomitante.Periodo);

                        alunoConcomitante.Censo = ctx.GetReturnValue<string>(ctxQuery);

                        if (string.IsNullOrEmpty(alunoConcomitante.Censo))
                        {
                            throw new Exception("Cancelamento não efetuado. Unidade de Ensino do Aluno não foi encontrada.");
                        }
                    }

                   

                    var contextQuery = new ContextQuery
                    {
                        Command = @" INSERT  INTO dbo.TCE_ALUNO_CONCOMITANTE ( ALUNO, CENSO, ANO, PERIODO, STATUS,
                                          MATRICULA )
                                     VALUES  ( @ALUNO, @CENSO, @ANO, @PERIODO, @STATUS, @MATRICULA ) "
                    };

                    contextQuery.Parameters.Add("@ALUNO", alunoConcomitante.Aluno);
                    contextQuery.Parameters.Add("@CENSO", alunoConcomitante.Censo);
                    contextQuery.Parameters.Add("@ANO", alunoConcomitante.Ano);
                    contextQuery.Parameters.Add("@PERIODO", alunoConcomitante.Periodo);
                    contextQuery.Parameters.Add("@STATUS", alunoConcomitante.Status);
                    contextQuery.Parameters.Add("@MATRICULA", alunoConcomitante.Matricula);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static ValidacaoDados Validar(TceAlunoConcomitante alunoConcomitante)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (alunoConcomitante == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(alunoConcomitante.Aluno))
            {
                mensagens.Add("O campo ALUNO é obrigatório!");
            }

            if (string.IsNullOrEmpty(alunoConcomitante.Censo) && (alunoConcomitante.Status == Liberado))
            {
                mensagens.Add("O campo CENSO é obrigatório!");
            }

            if (string.IsNullOrEmpty(alunoConcomitante.Status))
            {
                mensagens.Add("O campo STATUS é obrigatório!");
            }

            if (alunoConcomitante.Ano <= 0)
            {
                mensagens.Add("O campo ANO é obrigatório!");
            }

            if (alunoConcomitante.Periodo < 0)
            {
                mensagens.Add("O campo PERIODO é obrigatório!");
            }

            if (string.IsNullOrEmpty(alunoConcomitante.Matricula))
            {
                mensagens.Add("O campo MATRÍCULA é obrigatório!");
            }

            if (mensagens.Count == 0)
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    var contextQuery = new ContextQuery(
                        @" SELECT TOP 1
                                    STATUS
                            FROM    DBO.TCE_ALUNO_CONCOMITANTE
                            WHERE   ANO = @ANO
                                    AND PERIODO = @PERIODO
                                    AND ALUNO = @ALUNO
                            ORDER BY DT_CADASTRO DESC ");

                    contextQuery.Parameters.Add("@ANO", alunoConcomitante.Ano);
                    contextQuery.Parameters.Add("@PERIODO", alunoConcomitante.Periodo);
                    contextQuery.Parameters.Add("@ALUNO", alunoConcomitante.Aluno);

                    var ultimoStatus = ctx.GetReturnValue<string>(contextQuery);

                    if (ultimoStatus == alunoConcomitante.Status)
                    {
                        mensagens.Add("O aluno escolhido já se encontra " + alunoConcomitante.Status + " neste ano / periodo.");
                    }

                    //Busca municipio do aluno
                    var municipioAluno = Aluno.RetornaMunicipio(alunoConcomitante.Aluno);

                    //Busca municipio da escola
                    var municipioEscola = UnidadeEnsino.RetornaMunicipio(alunoConcomitante.Censo);

                    if (!string.IsNullOrEmpty(municipioEscola) && municipioAluno != municipioEscola)
                    {
                        var limitrofe = MunicipioLimitrofe.Listar(municipioEscola);
                        var achou = false;

                        foreach (DataRow item in limitrofe.Rows)
                        {
                            if (item["CODIGO_MUNICIPIO_LIMITROFE"].ToString() == municipioAluno)
                            {
                                achou = true;
                                break;
                            }
                        }

                        if (!achou)
                        {
                           mensagens.Add("O Município/UF informado no endereço do aluno nao condiz com o Município da escola ou de entorno. Favor verificar se o endereço do aluno está correto.");
                        }
                    }

                    //Verificar se o aluno já possui uma enturmação ativa para concomitante
                    contextQuery = new ContextQuery(
                        @" SELECT  COUNT(*)
                                FROM    DBO.LY_MATRICULA
                                WHERE   ALUNO = @ALUNO
                                        AND ANO = @ANO
                                        AND SEMESTRE = @SEMESTRE
                                        AND CONCOMITANTE = 'S'
                                        AND SIT_MATRICULA = 'Matriculado' ");

                    contextQuery.Parameters.Add("@ALUNO", alunoConcomitante.Aluno);
                    contextQuery.Parameters.Add("@ANO", alunoConcomitante.Ano);
                    contextQuery.Parameters.Add("@SEMESTRE", alunoConcomitante.Periodo);

                    var matriculas = ctx.GetReturnValue<int>(contextQuery);

                    if (matriculas > 0)
                    {
                        mensagens.Add("Este aluno já possui uma enturmaçao ativa no ensino profissional concomitante.");
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
    }
}
