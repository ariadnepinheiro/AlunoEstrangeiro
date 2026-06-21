using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.GestaoRede
{
    public class UnidadeFisicaTratamentoLixo
    {
        public ICollection<Entidades.UnidadeFisicaTratamentoLixo> ObtemPor(string unidadeFisica)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ICollection<Entidades.UnidadeFisicaTratamentoLixo> entidades = new List<Techne.Lyceum.RN.GestaoRede.Entidades.UnidadeFisicaTratamentoLixo>();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT * 
                                          FROM GESTAOREDE.UNIDADEFISICA_TRATAMENTOLIXO UT (NOLOCK)
                                          INNER JOIN GestaoRede.TratamentoLixo T ON UT.TRATAMENTOLIXOID=T.TRATAMENTOLIXOID
                                          WHERE UNIDADEFISICAID = @UNIDADEFISICAID ";

                contextQuery.Parameters.Add("@UNIDADEFISICAID", SqlDbType.VarChar, unidadeFisica);

                entidades = contexto.TryToBindEntities<Entidades.UnidadeFisicaTratamentoLixo>(contextQuery);

                return entidades;
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

        public void Insere(DataContext contexto, string unidadeFisica, int tratamentoLixoId, string usuarioResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO GestaoRede.UNIDADEFISICA_TRATAMENTOLIXO
                                               (TRATAMENTOLIXOID
                                               ,UNIDADEFISICAID
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                              (@TRATAMENTOLIXOID, 
                                               @UNIDADEFISICAID,
                                               @USUARIOID,
                                               @DATACADASTRO,
                                               @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@TRATAMENTOLIXOID", SqlDbType.Int, tratamentoLixoId);
            contextQuery.Parameters.Add("@UNIDADEFISICAID", SqlDbType.VarChar, unidadeFisica);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void RemovePorUnidade(DataContext contexto, string unidadeFisica)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE GestaoRede.UNIDADEFISICA_TRATAMENTOLIXO
                                      WHERE UNIDADEFISICAID = @UNIDADEFISICAID ";

            contextQuery.Parameters.Add("@UNIDADEFISICAID", SqlDbType.VarChar, unidadeFisica);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
