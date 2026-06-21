using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN
{
    public class CapacidaDeAlunoTurma
    {
        public static DataTable Listar(decimal ano, decimal periodo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" SELECT  CONVERT(VARCHAR(4), CT.ANO) + '/' + CONVERT(VARCHAR(1), CT.PERIODO) AS ANOPERIODO ,
                                        CT.CURSOID ,
                                        CUR.NOME AS DESCRICAOCURSO ,
                                        TC.DESCRICAO AS DESCRICAOTIPO ,
                                        CAPACIDADEMINIMA ,
                                        CAPACIDADEMAXIMA ,                                        
                                        CONVERT(VARCHAR(10), ct.DATAALTERACAO, 103) AS DATAALTERACAO ,
                                        CAPACIDADEALUNOTURMAID
                                FROM    DBO.CAPACIDADEALUNOTURMA CT
                                        INNER JOIN DBO.LY_CURSO CUR ON CT.CURSOID = CUR.CURSO
                                        INNER JOIN DBO.LY_TIPO_CURSO TC ON CUR.TIPO = TC.TIPO
                                WHERE   CT.ANO = @ANO
                                        AND CT.PERIODO = @PERIODO
                                ORDER BY CUR.NOME ,
                                        CT.ANO ,
                                        CT.PERIODO "
                    };

                    contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
                    contextQuery.Parameters.Add("@PERIODO", TechneDbType.T_SEMESTRE2, periodo);

                    return ctx.GetDataTable(contextQuery);
                }
                catch (Exception ex)
                {
                    ctx.Abandon();
                    var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                       Environment.NewLine, Convert.ToString(ex.Message));
                    throw new Exception(mensagem);
                }
            }
        }

        public static DataTable ListarAnoPeriodoReplicacao(int ano, int periodo)
        {
            var anoperiodo = string.Format("{0}/{1}",
                Convert.ToString(ano), Convert.ToString(periodo));

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" SELECT  DISTINCT
                                CONVERT(VARCHAR(4), CT.ANO) + '/' + CONVERT(VARCHAR(1), CT.PERIODO) AS ANOPERIODO ,
                                CT.ANO ,
                                CT.PERIODO
                        FROM    DBO.CAPACIDADEALUNOTURMA CT
                        WHERE   (CONVERT(VARCHAR(4), CT.ANO) + '/' + CONVERT(VARCHAR(1), CT.PERIODO)) <> @ANOPERIODO
                        ORDER BY CT.ANO ,
                                CT.PERIODO "
                    };
                    contextQuery.Parameters.Add("@ANOPERIODO", anoperiodo);

                    return ctx.GetDataTable(contextQuery);
                }
                catch (Exception ex)
                {
                    ctx.Abandon();
                    var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                       Environment.NewLine, Convert.ToString(ex.Message));
                    throw new Exception(mensagem);
                }
            }
        }

        public static ValidacaoDados Validar(Entidades.CapacidaDeAlunoTurma capacidaDeAlunoTurma)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (capacidaDeAlunoTurma == null)
            {
                return validacaoDados;
            }

            if (capacidaDeAlunoTurma.Ano <= 0)
            {
                mensagens.Add("Campos Ano é obrigatório.");
            }

            if (capacidaDeAlunoTurma.Periodo < 0)
            {
                mensagens.Add("Campos Periodoo é obrigatório.");
            }

            if (string.IsNullOrEmpty(capacidaDeAlunoTurma.CursoId))
            {
                mensagens.Add("Campos Curso é obrigatório.");
            }

            if (capacidaDeAlunoTurma.CapacidadeMinima <= 0)
            {
                mensagens.Add("Campos Capacidade Mínima não pode estar zerado.");
            }

            if (capacidaDeAlunoTurma.CapacidadeMaxima <= 0)
            {
                mensagens.Add("Campos Capacidade Máxima não pode estar zerado.");
            }

            if (capacidaDeAlunoTurma.CapacidadeMinima > capacidaDeAlunoTurma.CapacidadeMaxima)
            {
                mensagens.Add("Capacidade Máxima não podem ser menor do que Mínima.");
            }

            if (string.IsNullOrEmpty(capacidaDeAlunoTurma.Matricula)
                || (!string.IsNullOrEmpty(capacidaDeAlunoTurma.Matricula)
                    && capacidaDeAlunoTurma.Matricula.Length > 12))
            {
                mensagens.Add("O campo Matricula é obrigatório com o máximo de 12 caracteres!");
            }

            if (mensagens.Count == 0)
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    var contextQuery = new ContextQuery
                    {
                        //Verifica já existe o ano / periodo / curso cadastrados
                        Command =
                            @" SELECT  1
                        FROM    DBO.CAPACIDADEALUNOTURMA
                        WHERE   ANO = @ANO
                                AND PERIODO = @PERIODO
                                AND CURSOID = @CURSO
                                AND CAPACIDADEALUNOTURMAID <> @CAPACIDADEALUNOTURMAID "
                    };

                    contextQuery.Parameters.Add("@ANO", capacidaDeAlunoTurma.Ano);
                    contextQuery.Parameters.Add("@PERIODO", capacidaDeAlunoTurma.Periodo);
                    contextQuery.Parameters.Add("@CURSO", capacidaDeAlunoTurma.CursoId);
                    contextQuery.Parameters.Add("@CAPACIDADEALUNOTURMAID", capacidaDeAlunoTurma.CapacidaDeAlunoTurmaId);

                    var obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        mensagens.Add("Curso já cadastrado anteriormente para este ano / periodo.");
                    }
                }

                //Verifica ser é alteração
                if (capacidaDeAlunoTurma.CapacidaDeAlunoTurmaId > 0)
                {
                    RN.CapacidaDeAlunoTurma rnCapacidaDeAlunoTurma = new CapacidaDeAlunoTurma();

                    //Verifica a capacidade maxima
                    if (rnCapacidaDeAlunoTurma.ExisteTurmaSuperiorPor(capacidaDeAlunoTurma.Ano, capacidaDeAlunoTurma.Periodo, capacidaDeAlunoTurma.CursoId, capacidaDeAlunoTurma.CapacidadeMaxima))
                    {
                        mensagens.Add("Não será possivel alterar, pois existem turmas com capacidade superior.");
                    }

                    //Verifica a capacidade minima
                    if (rnCapacidaDeAlunoTurma.ExisteTurmaInferiorPor(capacidaDeAlunoTurma.Ano, capacidaDeAlunoTurma.Periodo, capacidaDeAlunoTurma.CursoId, capacidaDeAlunoTurma.CapacidadeMinima))
                    {
                        mensagens.Add("Não será possivel alterar, pois existem turmas com capacidade inferior.");
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

        private bool ExisteTurmaSuperiorPor(decimal ano, decimal periodo, string curso, int capacidade)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                            FROM   LY_TURMA T (NOLOCK) 
                                                   INNER JOIN LY_UNIDADE_ENSINO E (NOLOCK) 
                                                           ON T.FACULDADE = E.UNIDADE_ENS 
                                                   INNER JOIN LY_DEPENDENCIA DE (NOLOCK) 
                                                           ON E.UNIDADE_ENS = DE.FACULDADE 
                                                              AND T.DEPENDENCIA = DE.DEPENDENCIA 
                                            WHERE  T.ANO = @ANO 
                                                   AND T.SEMESTRE = @SEMESTRE 
                                                   AND T.CURSO = @CURSO 
                                                   AND T.NUM_ALUNOS > @CAPACIDADE	                                               
	                                               AND NOT EXISTS ( SELECT TOP 1 
					                                              1 
			                                               FROM   CAPACIDADEALUNOTURMAMUNICIPIO CM 
			                                               WHERE  CM.MUNICIPIOID = E.MUNICIPIO
                                                                  AND TIPO = 1 ) ";

                contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
                contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, periodo);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@CAPACIDADE", TechneDbType.T_NUMERO, capacidade);

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

        private bool ExisteTurmaInferiorPor(decimal ano, decimal periodo, string curso, int capacidade)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                            FROM   LY_TURMA T (NOLOCK) 
                                                   INNER JOIN LY_UNIDADE_ENSINO E (NOLOCK) 
                                                           ON T.FACULDADE = E.UNIDADE_ENS 
                                                   INNER JOIN LY_DEPENDENCIA DE (NOLOCK) 
                                                           ON E.UNIDADE_ENS = DE.FACULDADE 
                                                              AND T.DEPENDENCIA = DE.DEPENDENCIA 
                                            WHERE  T.ANO = @ANO 
                                                   AND T.SEMESTRE = @SEMESTRE 
                                                   AND T.CURSO = @CURSO 
                                                   AND T.NUM_ALUNOS < @CAPACIDADE
	                                               AND NOT EXISTS ( SELECT TOP 1 
					                                              1 
			                                               FROM   CAPACIDADEALUNOTURMAMUNICIPIO CM 
			                                               WHERE  CM.MUNICIPIOID = E.MUNICIPIO
                                                                  AND TIPO = 0 ) ";

                contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
                contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, periodo);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@CAPACIDADE", TechneDbType.T_NUMERO, capacidade);

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

        public static void Replicar(DadosReplicacaoCapacidadeTurma dadosReplicacaoCapacidadeTurma)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" INSERT INTO dbo.CAPACIDADEALUNOTURMA
                                        ( CURSOID ,
                                          ANO ,
                                          PERIODO ,
                                          CAPACIDADEMINIMA ,
                                          CAPACIDADEMAXIMA ,
                                          MATRICULA ,
                                          DATAALTERACAO
                                        )
                                SELECT  CI.CURSOID ,
                                        @ANO ,
                                        @PERIODO ,
                                        CI.CAPACIDADEMINIMA ,
                                        CI.CAPACIDADEMAXIMA ,
                                        @MATRICULA ,
                                        GETDATE()
                                FROM    DBO.CAPACIDADEALUNOTURMA CI
                                WHERE   ANO = @ANOREPLICACAO
                                        AND PERIODO = @PERIODOREPLICACAO
                                        AND NOT EXISTS ( SELECT 1
                                                         FROM   CAPACIDADEALUNOTURMA CF
                                                         WHERE  CF.CURSOID = CI.CURSOID
                                                                AND ANO = @ANO
                                                                AND PERIODO = @PERIODO ) "
                    };

                    contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, dadosReplicacaoCapacidadeTurma.Ano);
                    contextQuery.Parameters.Add("@PERIODO", TechneDbType.T_SEMESTRE2, dadosReplicacaoCapacidadeTurma.Periodo);
                    contextQuery.Parameters.Add("@MATRICULA", dadosReplicacaoCapacidadeTurma.Matricula);
                    contextQuery.Parameters.Add("@ANOREPLICACAO", TechneDbType.T_ANO, dadosReplicacaoCapacidadeTurma.AnoReplicacao);
                    contextQuery.Parameters.Add("@PERIODOREPLICACAO", TechneDbType.T_SEMESTRE2, dadosReplicacaoCapacidadeTurma.PeriodoReplicacao);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception ex)
                {
                    ctx.Abandon();
                    var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine, Convert.ToString(ex.Message));
                    throw new Exception(mensagem);
                }
            }
        }

        public static void Inserir(Entidades.CapacidaDeAlunoTurma capacidaDeAlunoTurma)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" INSERT  INTO dbo.CAPACIDADEALUNOTURMA
                                        ( CURSOID ,
                                          ANO ,
                                          PERIODO ,
                                          CAPACIDADEMINIMA ,
                                          CAPACIDADEMAXIMA ,
                                          MATRICULA ,
                                          DATAALTERACAO
                                        )
                                VALUES  ( @CURSOID ,
                                          @ANO ,
                                          @PERIODO ,
                                          @CAPACIDADEMINIMA ,
                                          @CAPACIDADEMAXIMA ,
                                          @MATRICULA ,
                                          GETDATE()
                                        ) "
                    };

                    contextQuery.Parameters.Add("@CURSOID", capacidaDeAlunoTurma.CursoId);
                    contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, capacidaDeAlunoTurma.Ano);
                    contextQuery.Parameters.Add("@PERIODO", TechneDbType.T_SEMESTRE2, capacidaDeAlunoTurma.Periodo);
                    contextQuery.Parameters.Add("@CAPACIDADEMINIMA", capacidaDeAlunoTurma.CapacidadeMinima);
                    contextQuery.Parameters.Add("@CAPACIDADEMAXIMA", capacidaDeAlunoTurma.CapacidadeMaxima);
                    contextQuery.Parameters.Add("@MATRICULA", capacidaDeAlunoTurma.Matricula);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception ex)
                {
                    ctx.Abandon();
                    var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                       Environment.NewLine, Convert.ToString(ex.Message));
                    throw new Exception(mensagem);
                }
            }
        }

        public static void Alterar(Entidades.CapacidaDeAlunoTurma capacidaDeAlunoTurma)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" UPDATE  DBO.CAPACIDADEALUNOTURMA
                                    SET     CAPACIDADEMINIMA = @CAPACIDADEMINIMA ,
                                            CAPACIDADEMAXIMA = @CAPACIDADEMAXIMA ,
                                            DATAALTERACAO = GETDATE() ,
                                            MATRICULA = @MATRICULA
                                    WHERE   CAPACIDADEALUNOTURMAID = @CAPACIDADEALUNOTURMAID "
                    };

                    contextQuery.Parameters.Add("@CAPACIDADEALUNOTURMAID", capacidaDeAlunoTurma.CapacidaDeAlunoTurmaId);
                    contextQuery.Parameters.Add("@CAPACIDADEMINIMA", capacidaDeAlunoTurma.CapacidadeMinima);
                    contextQuery.Parameters.Add("@CAPACIDADEMAXIMA", capacidaDeAlunoTurma.CapacidadeMaxima);
                    contextQuery.Parameters.Add("@MATRICULA", capacidaDeAlunoTurma.Matricula);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception ex)
                {
                    ctx.Abandon();
                    var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                       Environment.NewLine, Convert.ToString(ex.Message));
                    throw new Exception(mensagem);
                }
            }
        }

        public static void Remover(int id)
        {
            if (id < 1)
            {
                return;
            }

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"DELETE  dbo.CAPACIDADEALUNOTURMA
                                    WHERE   CAPACIDADEALUNOTURMAID = @ID"
                    };

                    contextQuery.Parameters.Add("@ID", id);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception ex)
                {
                    ctx.Abandon();
                    var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                       Environment.NewLine, Convert.ToString(ex.Message));
                    throw new Exception(mensagem);
                }
            }
        }

        public static Entidades.CapacidaDeAlunoTurma Carregar(decimal ano, decimal periodo, string curso)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                try
                {
                    var contextQuery =
                        new ContextQuery(
                            @" SELECT  *
                            FROM    DBO.CAPACIDADEALUNOTURMA
                            WHERE   ANO = @ANO
                                    AND PERIODO = @PERIODO
                                    AND CURSOID = @CURSOID ");

                    contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
                    contextQuery.Parameters.Add("@PERIODO", TechneDbType.T_SEMESTRE2, periodo);
                    contextQuery.Parameters.Add("@CURSOID", curso);

                    return ctx.TryToBindEntity<Entidades.CapacidaDeAlunoTurma>(contextQuery);
                }
                catch (Exception ex)
                {
                    ctx.Abandon();
                    var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                       Environment.NewLine, Convert.ToString(ex.Message));
                    throw new Exception(mensagem);
                }
            }
        }
    }
}