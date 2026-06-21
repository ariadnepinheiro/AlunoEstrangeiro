using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN
{
    public class PessoaTranstornoAprendizagem
    {
        public void InsereLista(DataContext contexto, List<int> listaTranstornoAprendizagem, decimal pessoa, string usuarioResponsavel)
        {
            foreach (int transtornoAprendizagem in listaTranstornoAprendizagem)
            {
                //Insere cada escolha
                this.Insere(contexto, transtornoAprendizagem, pessoa, usuarioResponsavel);
            }
        }

        private void Insere(DataContext ctx, int transtornoAprendizagem, decimal pessoa, string usuarioResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO dbo.PESSOA_TRANSTORNOAPRENDIZAGEM
                                           (PESSOA
                                           ,TRANSTORNOAPRENDIZAGEMID
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
                                     VALUES
                                           (@PESSOA, 
                                           @TRANSTORNOAPRENDIZAGEMID, 
                                           @USUARIOID, 
                                           @DATACADASTRO, 
                                           @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@TRANSTORNOAPRENDIZAGEMID", SqlDbType.Int, transtornoAprendizagem);
            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, pessoa);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAAlTERACAO", SqlDbType.DateTime, DateTime.Now);

            ctx.ApplyModifications(contextQuery);
        }

        public void AtualizaLista(DataContext contexto, List<int> listaTranstornoAprendizagem, decimal pessoa, string usuarioResponsavel)
        {
            List<int> transtornoAprendizagemCadastrado = new List<int>();

            //Busca já cadastrados 
            transtornoAprendizagemCadastrado = this.ListaTranstornoAprendizagemPor(contexto, pessoa);

            //Verificar quais foram removidos
            foreach (int transtornoAprendizagem in transtornoAprendizagemCadastrado)
            {
                if (!listaTranstornoAprendizagem.Contains(transtornoAprendizagem))
                {
                    //Remove 
                    this.Remove(contexto, transtornoAprendizagem, pessoa);
                }
            }

            //Verifica quais foram adicionados
            foreach (int transtornoAprendizagem in listaTranstornoAprendizagem)
            {
                if (!transtornoAprendizagemCadastrado.Contains(transtornoAprendizagem))
                {
                    //Insere 
                    this.Insere(contexto, transtornoAprendizagem, pessoa, usuarioResponsavel);
                }
            }
        }

        public List<int> ListaTranstornoAprendizagemPor(decimal pessoa)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            List<int> lista = new List<int>();
            try
            {
                lista = this.ListaTranstornoAprendizagemPor(contexto, pessoa);

                return lista;
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

        private List<int> ListaTranstornoAprendizagemPor(DataContext ctx, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            List<int> retorno = new List<int>();
            try
            {
                contextQuery.Command = @" SELECT TRANSTORNOAPRENDIZAGEMID 
                                        FROM   PESSOA_TRANSTORNOAPRENDIZAGEM (NOLOCK) 
                                        WHERE  PESSOA = @PESSOA  ";

                contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, pessoa);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno.Add(Convert.ToInt32(reader["TRANSTORNOAPRENDIZAGEMID"]));
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return retorno;
        }

        private void Remove(DataContext ctx, int transtornoAprendizagem, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE FROM PESSOA_TRANSTORNOAPRENDIZAGEM 
                                        WHERE  PESSOA = @PESSOA 
                                            AND TRANSTORNOAPRENDIZAGEMID = @TRANSTORNOAPRENDIZAGEMID  ";

            contextQuery.Parameters.Add("@TRANSTORNOAPRENDIZAGEMID", SqlDbType.Int, transtornoAprendizagem);
            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, pessoa);

            ctx.ApplyModifications(contextQuery);
        }

        public void Remove(DataContext ctx, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE PESSOA_TRANSTORNOAPRENDIZAGEM 
                                        WHERE   PESSOA = @PESSOA ";

                contextQuery.Parameters.Add("@PESSOA", pessoa);

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

        public bool PossuiTranstornoAprendizagemPor(DataContext contexto, int transtornoAprendizagemId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                    FROM PESSOA_TRANSTORNOAPRENDIZAGEM
                                    WHERE TRANSTORNOAPRENDIZAGEMID = @TRANSTORNOAPRENDIZAGEMID ";

            contextQuery.Parameters.Add("@TRANSTORNOAPRENDIZAGEMID", transtornoAprendizagemId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }
    }
}
