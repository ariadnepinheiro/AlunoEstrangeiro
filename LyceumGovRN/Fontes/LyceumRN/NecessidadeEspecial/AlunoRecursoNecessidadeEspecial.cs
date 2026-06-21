using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.NecessidadeEspecial
{
    public class AlunoRecursoNecessidadeEspecial
    {
        public void InsereLista(DataContext contexto, List<int> listaTipoRecurso, string aluno, string usuarioResponsavel)
        {
            foreach (int tipoRecurso in listaTipoRecurso)
            {
                //Insere cada escolha do aluno
                this.Insere(contexto, tipoRecurso, aluno, usuarioResponsavel);
            }
        }

        public void AtualizaLista(DataContext contexto, List<int> listaRecurso, string aluno, string usuarioResponsavel)
        {
            List<int> recursoCadastrado = new List<int>();

            //Busca recursos atuais do aluno
            recursoCadastrado = this.ListaRecursoAlunoPor(contexto, aluno);

            //Verificar quais recursos foram removidos
            foreach (int tipoRecurso in recursoCadastrado)
            {
                if (!listaRecurso.Contains(tipoRecurso))
                {
                    //Remove 
                    this.Remove(contexto, tipoRecurso, aluno);
                }
            }

            //Verifica quais recursos foram adicionados
            foreach (int tipoRecurso in listaRecurso)
            {
                if (!recursoCadastrado.Contains(tipoRecurso))
                {
                    //Insere 
                    this.Insere(contexto, tipoRecurso, aluno, usuarioResponsavel);
                }
            }
        }

        private List<int> ListaRecursoAlunoPor(DataContext ctx, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            List<int> retorno = new List<int>();
            try
            {
                contextQuery.Command = @" SELECT TIPORECURSONECESSIDADEESPECIALID 
                                        FROM   NECESSIDADEESPECIAL.ALUNORECURSONECESSIDADEESPECIAL (NOLOCK) 
                                        WHERE  ALUNOID = @ALUNOID  ";

                contextQuery.Parameters.Add("@ALUNOID", SqlDbType.VarChar, aluno);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno.Add(Convert.ToInt32(reader["TIPORECURSONECESSIDADEESPECIALID"]));
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

        public List<int> ListaRecursoAlunoPor(string aluno)
        {	
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            List<int> lista = new List<int>();
            try
            { 
                lista = this.ListaRecursoAlunoPor(contexto, aluno);

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

        public void Insere(DataContext ctx, int tipoRecursoNecessidadeEspecial, string aluno, string usuarioResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO NECESSIDADEESPECIAL.ALUNORECURSONECESSIDADEESPECIAL 
                                                    (TIPORECURSONECESSIDADEESPECIALID, 
                                                     ALUNOID, 
                                                     USUARIOID, 
                                                     DATACADASTRO, 
                                                     DATAALTERACAO) 
                                        VALUES      (@TIPORECURSONECESSIDADEESPECIALID, 
                                                     @ALUNOID, 
                                                     @USUARIOID, 
                                                     @DATACADASTRO, 
                                                     @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@TIPORECURSONECESSIDADEESPECIALID", SqlDbType.Int, tipoRecursoNecessidadeEspecial);
            contextQuery.Parameters.Add("@ALUNOID", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAAlTERACAO", SqlDbType.DateTime, DateTime.Now);

            ctx.ApplyModifications(contextQuery);
        }

        private void Remove(DataContext ctx, int tipoRecursoNecessidadeEspecial, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE FROM NECESSIDADEESPECIAL.ALUNORECURSONECESSIDADEESPECIAL 
                                        WHERE  ALUNOID = @ALUNOID 
                                            AND TIPORECURSONECESSIDADEESPECIALID = @TIPORECURSONECESSIDADEESPECIALID  ";

            contextQuery.Parameters.Add("@TIPORECURSONECESSIDADEESPECIALID", SqlDbType.Int, tipoRecursoNecessidadeEspecial);
            contextQuery.Parameters.Add("@ALUNOID", SqlDbType.VarChar, aluno);

            ctx.ApplyModifications(contextQuery);
        }

        public void Remove(DataContext ctx, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE FROM NECESSIDADEESPECIAL.ALUNORECURSONECESSIDADEESPECIAL 
                                        WHERE  ALUNOID = @ALUNOID   ";

            contextQuery.Parameters.Add("@ALUNOID", SqlDbType.VarChar, aluno);

            ctx.ApplyModifications(contextQuery);
        }
    }
}
