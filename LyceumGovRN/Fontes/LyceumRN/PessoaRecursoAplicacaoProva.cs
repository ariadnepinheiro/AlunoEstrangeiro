using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using System;
using Techne.Lyceum.RN.Util;
using System.Collections.Generic;
using Techne.Data;

namespace Techne.Lyceum.RN
{
    public class PessoaRecursoAplicacaoProva
    {
        public static DataTable Listar(int idPessoa)
        {
            try
            {
                using (var contexto = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" SELECT PR.[PESSOAID],
                                            PR.[RECURSOAPLICACAOPROVAID],
	                                        PR.[DATAATUALIZACAO],
	                                        PR.[USUARIO], 
                                            R.[EXCLUSIVO]
                                       FROM [DBO].[PESSOA_RECURSOAPLICACAOPROVA] PR INNER JOIN [NecessidadeEspecial].[RECURSOAPLICACAOPROVA] R
                                         ON PR.RECURSOAPLICACAOPROVAID = R.RECURSOAPLICACAOPROVAID
                                      WHERE PR.PESSOAID = @PESSOAID 
                                         AND R.ATIVO = 1 "
                    };

                    contextQuery.Parameters.Add("@PESSOAID", idPessoa);

                    return contexto.GetDataTable(contextQuery);
                }
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
        }

        public static int RemoverPessoaRecursoAplicacaoProva(int idPessoa, DataContext contexto)
        {
            var contextQuery = new ContextQuery
            {
                Command = @" DELETE FROM PESSOA_RECURSOAPLICACAOPROVA WHERE PESSOAID = @PESSOAID "
            };

            contextQuery.Parameters.Add("@PESSOAID", idPessoa);

            return contexto.ApplyModifications(contextQuery);
        }

        public static void InserirPessoaRecursoAplicacaoProva(List<Entidades.PessoaRecursoAplicacaoProva> listPessoaRecursoAplicacaoProva, int idPessoa, DataContext contexto)
        {
            RemoverPessoaRecursoAplicacaoProva(idPessoa, contexto);

            var contextQuery = new ContextQuery
            {
                Command = @" INSERT INTO PESSOA_RECURSOAPLICACAOPROVA
                                     (PESSOAID, RECURSOAPLICACAOPROVAID, DATAATUALIZACAO, USUARIO)
                                 VALUES
                                     (@PESSOAID, @RECURSOAPLICACAOPROVAID, @DATAATUALIZACAO, @USUARIO) "
            };

            foreach (Entidades.PessoaRecursoAplicacaoProva pessoaRecursoAplicacaoProva in listPessoaRecursoAplicacaoProva)
            {
                contextQuery.Parameters.Clear();
                contextQuery.Parameters.Add("@PESSOAID", idPessoa);
                contextQuery.Parameters.Add("@RECURSOAPLICACAOPROVAID", pessoaRecursoAplicacaoProva.RecursoAplicacaoProvaId);
                contextQuery.Parameters.Add("@DATAATUALIZACAO", pessoaRecursoAplicacaoProva.DataAtualizacao);
                contextQuery.Parameters.Add("@USUARIO", pessoaRecursoAplicacaoProva.Usuario);

                contexto.ApplyModifications(contextQuery);
            }
        }

        public void RemovePessoaRecursoAplicacaoProvaDuplicadaPor(DataContext ctx, decimal pessoaId)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE FROM PESSOA_RECURSOAPLICACAOPROVA WHERE PESSOAID = @PESSOAID ";

                contextQuery.Parameters.Add("@PESSOAID", pessoaId);

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
