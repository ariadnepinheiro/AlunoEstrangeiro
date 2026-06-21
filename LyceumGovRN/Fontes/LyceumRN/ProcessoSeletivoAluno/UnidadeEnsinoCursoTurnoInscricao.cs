using System;
using System.Data;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.ProcessoSeletivoAluno
{
    public class UnidadeEnsinoCursoTurnoInscricao : RNBase
    {
        public static DataTable ListaUnidadeEnsinoCursoInscricao(Int64 NumeroInscricao)
        {
            ContextQuery contextQuery = new ContextQuery();
            DataTable unidadeEnsinoCursoInscricao = null;

            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    contextQuery.Command = @"SELECT UCT.UNIDADEENSINOID,
                                                    UCT.CURSOID,
                                                    UCT.TURNOID
                                               FROM LYCEUM.PROCESSOSELETIVOALUNO.INSCRICAO I
                                              INNER JOIN LYCEUM.PROCESSOSELETIVOALUNO.UNIDADEENSINO_CURSO_TURNO_INSCRICAO UCT
                                                 ON UCT.INSCRICAOID = I.INSCRICAOID
                                              WHERE I.NUMEROINSCRICAO = @NUMEROINSCRICAO";

                    contextQuery.Parameters.Add("@NUMEROINSCRICAO", NumeroInscricao);

                    unidadeEnsinoCursoInscricao = ctx.GetDataTable(contextQuery);
                }
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return unidadeEnsinoCursoInscricao;
        }

        public static void SalvaUnidadeEnsinoCursoTurnoInscricao(RN.ProcessoSeletivoAluno.Entidades.UnidadeEnsinoCursoTurnoInscricao unidadeEnsinoCursoTurnoInscricao, int inscricaoId, DataContext ctx)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @"INSERT INTO LYCEUM.ProcessoSeletivoAluno.UNIDADEENSINO_CURSO_TURNO_INSCRICAO
                                               (UNIDADEENSINOID
                                               ,INSCRICAOID
                                               ,CURSOID
                                               ,TURNOID
                                               ,DATAALTERACAO
                                               ,IP
                                               ,DATACADASTRO)
                                         VALUES
                                               (@UNIDADEENSINOID
                                               ,@INSCRICAOID
                                               ,@CURSOID
                                               ,@TURNOID
                                               ,@DATAALTERACAO
                                               ,@IP
                                               ,@DATACADASTRO)";

                contextQuery.Parameters.Add("@UNIDADEENSINOID", unidadeEnsinoCursoTurnoInscricao.UnidadeEnsinoId);
                contextQuery.Parameters.Add("@INSCRICAOID", inscricaoId);
                contextQuery.Parameters.Add("@CURSOID", unidadeEnsinoCursoTurnoInscricao.CursoId);
                contextQuery.Parameters.Add("@TURNOID", unidadeEnsinoCursoTurnoInscricao.TurnoId);
                contextQuery.Parameters.Add("@DATAALTERACAO", unidadeEnsinoCursoTurnoInscricao.DataAlteracao);
                contextQuery.Parameters.Add("@IP", unidadeEnsinoCursoTurnoInscricao.IP);
                contextQuery.Parameters.Add("@DATACADASTRO", unidadeEnsinoCursoTurnoInscricao.DataCadastro);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
        }

        public static void AlteraUnidadeEnsinoCursoTurnoInscricao(RN.ProcessoSeletivoAluno.Entidades.UnidadeEnsinoCursoTurnoInscricao unidadeEnsinoCursoTurnoInscricao, int inscricaoId, DataContext ctx)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" UPDATE LYCEUM.ProcessoSeletivoAluno.UNIDADEENSINO_CURSO_TURNO_INSCRICAO
                                            SET UNIDADEENSINOID = @UNIDADEENSINOID
                                               ,CURSOID = @CURSOID
                                               ,TURNOID = @TURNOID
                                               ,DATAALTERACAO = @DATAALTERACAO
                                               ,IP = @IP
                                               ,DATACADASTRO = @DATACADASTRO
                                                WHERE INSCRICAOID = @INSCRICAOID";

                contextQuery.Parameters.Add("@UNIDADEENSINOID", unidadeEnsinoCursoTurnoInscricao.UnidadeEnsinoId);
                contextQuery.Parameters.Add("@INSCRICAOID", inscricaoId);
                contextQuery.Parameters.Add("@CURSOID", unidadeEnsinoCursoTurnoInscricao.CursoId);
                contextQuery.Parameters.Add("@TURNOID", unidadeEnsinoCursoTurnoInscricao.TurnoId);
                contextQuery.Parameters.Add("@DATAALTERACAO", unidadeEnsinoCursoTurnoInscricao.DataAlteracao);
                contextQuery.Parameters.Add("@IP", unidadeEnsinoCursoTurnoInscricao.IP);
                contextQuery.Parameters.Add("@DATACADASTRO", unidadeEnsinoCursoTurnoInscricao.DataCadastro);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
        }

    }
}
