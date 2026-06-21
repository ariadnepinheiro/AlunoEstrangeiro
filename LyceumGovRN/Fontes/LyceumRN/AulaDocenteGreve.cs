namespace Techne.Lyceum.RN
{
    using System;
    using System.Data;
    using Seeduc.Infra.Data;
    using Techne.Lyceum.RN.Entidades;

    public class AulaDocenteGreve
    {
        public static DataTable Listar(string turma, int ano, int semestre, string unidade_ens, string datainicio, string datafim)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT CODIGO,
                                   AD.ANO,
                                   AD.AULA,
                                   AD.DATA_INICIO,
                                   ( CONVERT(CHAR(5), O.HORAINI_AULA, 108) ) + ' - ' + (
                                   CONVERT(CHAR(5), O.HORAFIM_AULA, 108) )                    AS 'HORARIO',
                                   AD.DIA_SEMANA,
                                   CASE AD.DIA_SEMANA
                                     WHEN 2 THEN 'Segunda'
                                     WHEN 3 THEN 'Terça'
                                     WHEN 4 THEN 'Quarta'
                                     WHEN 5 THEN 'Quinta'
                                     WHEN 6 THEN 'Sexta'
                                     WHEN 7 THEN 'Sábado'
                                   END                                                        DIA,
                                   AD.DISCIPLINA,
                                   ISNULL((SELECT DI.NOME + ' (' + DM.NOME + ')'
                                           FROM   LY_TURMA T (NOLOCK)
                                                  INNER JOIN LY_DISCIPLINA DM (NOLOCK)
                                                          ON T.DISCIPLINA_MULTIPLA = DM.DISCIPLINA
                                           WHERE  T.TURMA = AD.TURMA
                                                  AND T.ANO = AD.ANO
                                                  AND T.SEMESTRE = AD.SEMESTRE
                                                  AND T.DISCIPLINA = DI.DISCIPLINA), DI.NOME) AS
                                   'NOME_DISCIPLINA',
                                   AD.FACULDADE,
                                   AD.NUM_FUNC,
                                   PE.NOME_COMPL,
                                   D.MATRICULA,
                                   ISNULL((CONVERT(VARCHAR,PE.IDFUNCIONAL) + '/' + CONVERT(VARCHAR,D.VINCULO)),D.MATRICULA) IDVINCULO,
                                   AD.SEMESTRE,
                                   AD.TURMA,
                                   AD.TURNO,
                                   ADG.DATA_INICIO_SUBSTITUICAO,
                                   ADG.DATA_FIM_SUBSTITUICAO,
                                   ADG.MATRICULA_SUBSTITUTO,
                                   PE2.NOME_COMPL                                              AS
                                   'NOME_SUBSTITUTO',
                                   D2.MATRICULA                                               AS
                                   'MATRICULA_SUBSTITUTO',
                                   D2.NUM_FUNC AS NUMFUNC2,
                                   ISNULL((CONVERT(VARCHAR,PE2.IDFUNCIONAL) + '/' + CONVERT(VARCHAR,D2.VINCULO)),D2.MATRICULA) IDVINCULO2,
                                   ADG.MATRICULA,
                                   ADG.DATA_CADASTRO,
                                   ISNULL(ADT.TIPO_AULA, 'NORMAL')                            AS 'TIPO',
                                   DI.CAMPO_01                                                AS
                                   'TIPO_DISCIPLINA'
                            FROM   DBO.LY_AULA_DOCENTE AD WITH (NOLOCK)
                                   LEFT JOIN DBO.LY_AULA_DOCENTE_TIPO ADT WITH (NOLOCK)
                                          ON ADT.ANO = AD.ANO
                                             AND ADT.AULA = AD.AULA
                                             AND ADT.DATA_INICIO = AD.DATA_INICIO
                                             AND ADT.DIA_SEMANA = AD.DIA_SEMANA
                                             AND ADT.DISCIPLINA = AD.DISCIPLINA
                                             AND ADT.FACULDADE = AD.FACULDADE
                                             AND ADT.NUM_FUNC = AD.NUM_FUNC
                                             AND ADT.SEMESTRE = AD.SEMESTRE
                                             AND ADT.TURMA = AD.TURMA
                                             AND ADT.TURNO = AD.TURNO
                                             AND ADT.TIPO_AULA = 'GLP'
                                   INNER JOIN DBO.LY_DOCENTE D WITH (NOLOCK)
                                           ON AD.NUM_FUNC = D.NUM_FUNC
	                               INNER JOIN DBO.LY_PESSOA PE WITH (NOLOCK)
                                           ON PE.PESSOA = D.PESSOA
                                   INNER JOIN LY_DISCIPLINA DI (NOLOCK)
                                           ON DI.DISCIPLINA = AD.DISCIPLINA
                                   INNER JOIN LY_HOR_OPER O (NOLOCK)
                                           ON AD.AULA = O.AULA
                                              AND AD.DIA_SEMANA = O.DIA_SEMANA
                                              AND AD.FACULDADE = O.FACULDADE
                                              AND AD.TURNO = O.TURNO
                                   LEFT JOIN DBO.TCE_AULA_DOCENTE_GREVE ADG WITH (NOLOCK)
                                          ON AD.ANO = ADG.ANO
                                             AND AD.AULA = ADG.AULA
                                             AND AD.DATA_INICIO = ADG.DATA_INICIO
                                             AND AD.DIA_SEMANA = ADG.DIA_SEMANA
                                             AND AD.DISCIPLINA = ADG.DISCIPLINA
                                             AND AD.FACULDADE = ADG.FACULDADE
                                             AND AD.NUM_FUNC = ADG.NUM_FUNC
                                             AND AD.SEMESTRE = ADG.SEMESTRE
                                             AND AD.TURMA = ADG.TURMA
                                             AND AD.TURNO = ADG.TURNO
                                             AND ADG.ATIVO = 1
                                   LEFT JOIN LY_DOCENTE D2
                                          ON ADG.MATRICULA_SUBSTITUTO = D2.MATRICULA
	                               LEFT JOIN LY_PESSOA PE2 WITH (NOLOCK)
                                           ON PE2.PESSOA = D2.PESSOA
                            WHERE  AD.ANO = @ANO
                                   AND AD.SEMESTRE = @SEMESTRE
                                   AND AD.TURMA = @TURMA
                                   AND AD.FACULDADE = @FACULDADE
                                   AND EXISTS (SELECT 1
                                               FROM   LY_AULA_DOCENTE A
                                                      INNER JOIN LY_LICENCA_DOCENTE LD
                                                              ON AD.NUM_FUNC = LD.NUM_FUNC
                                               WHERE  MOTIVO IN ( '61', '61A' )
                                                      AND ANO = @ANO
                                                      AND SEMESTRE = @SEMESTRE
                                                      AND FACULDADE = @FACULDADE
                                                      AND AD.TURMA = A.TURMA
                                                      AND AD.ANO = A.ANO
                                                      AND AD.SEMESTRE = A.SEMESTRE
                                                      AND AD.TURNO = A.TURNO
                                                      AND AD.FACULDADE = A.FACULDADE)
                                                      --AND AD.DATA_FIM >= Getdate()
                                                      --AND CONVERT(VARCHAR, DTINI, 103) >= @DATAINICIO
                                                      --AND CONVERT(VARCHAR, DTINI, 103) <= @DATAFIM)    "
                };

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@FACULDADE", unidade_ens);
                contextQuery.Parameters.Add("@datainicio", datainicio);
                contextQuery.Parameters.Add("@datafim", datafim);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static int Inserir(TceAulaDocenteGreve aulaDocenteGreve)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {

                contextQuery.Command = @" INSERT INTO dbo.TCE_AULA_DOCENTE_GREVE
                            (
                             NUM_FUNC,
                             TURNO,
                             FACULDADE,
                             DIA_SEMANA,
                             AULA,
                             DISCIPLINA,
                             TURMA,
                             ANO,
                             SEMESTRE,
                             DATA_INICIO,
                             TIPO,
                             MATRICULA_SUBSTITUTO,
                             DATA_INICIO_SUBSTITUICAO,
                             DATA_FIM_SUBSTITUICAO,
                             ATIVO,
                             MATRICULA,
                             VALOR_GLP
                                                           
                            )
                    VALUES  (
                             @NUM_FUNC,
                             @TURNO,
                             @FACULDADE,
                             @DIA_SEMANA,
                             @AULA,
                             @DISCIPLINA,
                             @TURMA,
                             @ANO,
                             @SEMESTRE,
                             @DATA_INICIO,
                             @TIPO,
                             @MATRICULA_SUBSTITUTO,
                             @DATA_INICIO_SUBSTITUICAO,
                             @DATA_FIM_SUBSTITUICAO,
                             1,
                             @MATRICULA,
                             @VALOR_GLP
                               )"
                    ;

                contextQuery.Parameters.Add("@CODIGO", aulaDocenteGreve.Codigo);
                contextQuery.Parameters.Add("@NUM_FUNC", aulaDocenteGreve.NumFunc);
                contextQuery.Parameters.Add("@TURNO", aulaDocenteGreve.Turno);
                contextQuery.Parameters.Add("@FACULDADE", aulaDocenteGreve.Faculdade);
                contextQuery.Parameters.Add("@DIA_SEMANA", aulaDocenteGreve.DiaSemana);
                contextQuery.Parameters.Add("@AULA", aulaDocenteGreve.Aula);
                contextQuery.Parameters.Add("@DISCIPLINA", aulaDocenteGreve.Disciplina);
                contextQuery.Parameters.Add("@TURMA", aulaDocenteGreve.Turma);
                contextQuery.Parameters.Add("@ANO", aulaDocenteGreve.Ano);
                contextQuery.Parameters.Add("@SEMESTRE", aulaDocenteGreve.Semestre);
                contextQuery.Parameters.Add("@DATA_INICIO", aulaDocenteGreve.DataInicio);
                contextQuery.Parameters.Add("@TIPO", aulaDocenteGreve.Tipo);
                contextQuery.Parameters.Add("@MATRICULA_SUBSTITUTO", aulaDocenteGreve.MatriculaSubstituto);
                contextQuery.Parameters.Add("@DATA_INICIO_SUBSTITUICAO", aulaDocenteGreve.DataInicioSubstituicao);
                contextQuery.Parameters.Add("@DATA_FIM_SUBSTITUICAO", aulaDocenteGreve.DataFimSubstituicao);
                contextQuery.Parameters.Add("@MATRICULA", aulaDocenteGreve.Matricula);
                contextQuery.Parameters.Add("@VALOR_GLP", aulaDocenteGreve.ValorGLP);

                return ctx.ApplyModifications(contextQuery);

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

        public static int Alterar(TceAulaDocenteGreve aulaDocenteGreve)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            try
            {
                contextQuery.Command = @" UPDATE  TCE_AULA_DOCENTE_GREVE
                        SET     DATA_INICIO_SUBSTITUICAO = @DATA_INICIO_SUBSTITUICAO,
                                DATA_FIM_SUBSTITUICAO = @DATA_FIM_SUBSTITUICAO,
                                MATRICULA_SUBSTITUTO = @MATRICULA_SUBSTITUTO,
                                VALOR_GLP = @VALOR_GLP,
                                MATRICULA = @MATRICULA
                        WHERE   CODIGO = @CODIGO"
                ;

                contextQuery.Parameters.Add("@CODIGO", aulaDocenteGreve.Codigo);
                contextQuery.Parameters.Add("@MATRICULA_SUBSTITUTO", aulaDocenteGreve.MatriculaSubstituto);
                contextQuery.Parameters.Add("@DATA_INICIO_SUBSTITUICAO", aulaDocenteGreve.DataInicioSubstituicao);
                contextQuery.Parameters.Add("@DATA_FIM_SUBSTITUICAO", aulaDocenteGreve.DataFimSubstituicao);
                contextQuery.Parameters.Add("@VALOR_GLP", aulaDocenteGreve.ValorGLP);
                contextQuery.Parameters.Add("@MATRICULA", aulaDocenteGreve.Matricula);

                return ctx.ApplyModifications(contextQuery);

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

        public static int Remover(int codigoAulaDocenteGreve, string usuario)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"UPDATE TCE_AULA_DOCENTE_GREVE
                        SET     ATIVO = 0,
                                MATRICULA = @MATRICULA
                        WHERE   CODIGO = @CODIGO"
                    };
                    contextQuery.Parameters.Add("@CODIGO", codigoAulaDocenteGreve);
                    contextQuery.Parameters.Add("@MATRICULA", usuario);
                    return ctx.ApplyModifications(contextQuery);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void Salvar(TceAulaDocenteGreve aulaDocenteGreve)
        {
            if (aulaDocenteGreve.Codigo != 0)
                Alterar(aulaDocenteGreve);
            else
                Inserir(aulaDocenteGreve);
        }

    }
}