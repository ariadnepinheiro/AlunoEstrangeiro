using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.TurnosVagas
{
    public class ResultadoAnalise
    {
        public List<RN.TurnosVagas.Entidades.ResultadoAnalise> ListaResultadosAnaliseAtivos()
        {
            List<RN.TurnosVagas.Entidades.ResultadoAnalise> listaResultados = new List<RN.TurnosVagas.Entidades.ResultadoAnalise>();
            RN.TurnosVagas.Entidades.ResultadoAnalise resultado = new RN.TurnosVagas.Entidades.ResultadoAnalise();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  *
                        FROM    TurnosVagas.RESULTADOANALISE
                        WHERE   ATIVO = 1 ";               

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    resultado = new RN.TurnosVagas.Entidades.ResultadoAnalise();

                    resultado.ResultadoAnaliseId = Convert.ToInt32(reader["RESULTADOANALISEID"]);
                    resultado.Descricao = Convert.ToString(reader["DESCRICAO"]);
                    resultado.Ativo = Convert.ToBoolean(reader["ATIVO"]);
                    resultado.Matricula = Convert.ToString(reader["MATRICULA"]);
                    resultado.DataCadastro = Convert.ToDateTime(reader["DATACADASTRO"]);
                    if (reader["DATAALTERACAO"] != DBNull.Value)
                    {
                        resultado.DataAlteracao = Convert.ToDateTime(reader["DATAALTERACAO"]);
                    }

                    listaResultados.Add(resultado);
                }

                return listaResultados;
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }
    }
}
