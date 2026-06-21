using System;
using System.Data;
using Seeduc.Infra.Data;
using System.Collections.Generic;

namespace Techne.Lyceum.RN.ProcessoSeletivoAluno
{
    public class RecursoAplicacaoProvaCandidato : RNBase
    {
        public static DataTable ListaRecursosNecessariosParaProva(int candidatoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            DataTable recursosNecessariosParaProva = null;

            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    contextQuery.Command = @"SELECT R.RECURSOAPLICACAOPROVAID
                                               FROM LYCEUM.PROCESSOSELETIVOALUNO.CANDIDATO C
                                              INNER JOIN LYCEUM.PROCESSOSELETIVOALUNO.RECURSOAPLICACAOPROVA_CANDIDATO R
                                                 ON C.CANDIDATOID = R.CANDIDATOID
                                              WHERE C.CANDIDATOID = @CANDIDATOID ";

                    contextQuery.Parameters.Add("@CANDIDATOID ", candidatoId);

                    recursosNecessariosParaProva = ctx.GetDataTable(contextQuery);
                }
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return recursosNecessariosParaProva;
        }


        public static void SalvaRecursoAplicacaoProva(List<RN.ProcessoSeletivoAluno.Entidades.RecursoAplicacaoProvaCandidato> listRecursoAplicacaoProvaCandidato, int candidatoId, DataContext ctx)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @"INSERT INTO LYCEUM.ProcessoSeletivoAluno.RECURSOAPLICACAOPROVA_CANDIDATO
                                                   (CANDIDATOID, RECURSOAPLICACAOPROVAID)
                                             VALUES
                                                   (@CANDIDATOID, @RECURSOAPLICACAOPROVAID)";

                foreach (RN.ProcessoSeletivoAluno.Entidades.RecursoAplicacaoProvaCandidato recursoAplicacaoProvaCandidato in listRecursoAplicacaoProvaCandidato)
                {
                    contextQuery.Parameters.Clear();
                    contextQuery.Parameters.Add("@CANDIDATOID", candidatoId);
                    contextQuery.Parameters.Add("@RECURSOAPLICACAOPROVAID", recursoAplicacaoProvaCandidato.RecursoAplicacaoProvaId);

                    ctx.ApplyModifications(contextQuery);
                }
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
        }

        public static void AlteraRecursoAplicacaoProva(List<RN.ProcessoSeletivoAluno.Entidades.RecursoAplicacaoProvaCandidato> listRecursoAplicacaoProvaCandidato, int candidatoId, DataContext ctx)
        {
            try
            {
                DeletaRecursoAplicacaoProva(candidatoId, ctx);
                SalvaRecursoAplicacaoProva(listRecursoAplicacaoProvaCandidato, candidatoId, ctx);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
        }

        private static void DeletaRecursoAplicacaoProva(int candidatoId, DataContext ctx)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @"DELETE FROM LYCEUM.ProcessoSeletivoAluno.RECURSOAPLICACAOPROVA_CANDIDATO
                                          WHERE CANDIDATOID = @CANDIDATOID";

                contextQuery.Parameters.Add("@CANDIDATOID", candidatoId);

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
