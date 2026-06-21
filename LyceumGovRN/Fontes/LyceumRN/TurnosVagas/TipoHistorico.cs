using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.TurnosVagas
{
    public class TipoHistorico : RNBase
    {
        public enum TiposHistorico
        {
            [StringValue("Diretor Unidade Ensino e/ou Diretoria Regional")]
            Diretor = 1,
            [StringValue("Secretaria Estadual de Educação")]
            Seeduc = 2
        }

        public List<RN.TurnosVagas.Entidades.TipoHistorico> ListaTiposHistoricoAtivos()
        {
            List<RN.TurnosVagas.Entidades.TipoHistorico> listaTipoHistorico = new List<RN.TurnosVagas.Entidades.TipoHistorico>();
            RN.TurnosVagas.Entidades.TipoHistorico tipoHistorico = new RN.TurnosVagas.Entidades.TipoHistorico();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  *
                                        FROM    TurnosVagas.TIPOHISTORICO
                                        WHERE   ATIVO = 1 ";               

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    tipoHistorico = new RN.TurnosVagas.Entidades.TipoHistorico();

                    tipoHistorico.TipoHistoricoId = Convert.ToInt32(reader["TIPOHISTORICOID"]);
                    tipoHistorico.Descricao = Convert.ToString(reader["DESCRICAO"]);
                    tipoHistorico.Ativo = Convert.ToBoolean(reader["ATIVO"]);
                    tipoHistorico.Matricula = Convert.ToString(reader["MATRICULA"]);
                    tipoHistorico.DataCadastro = Convert.ToDateTime(reader["DATACADASTRO"]);
                    if (reader["DATAALTERACAO"] != DBNull.Value)
                    {
                        tipoHistorico.DataAlteracao = Convert.ToDateTime(reader["DATAALTERACAO"]);
                    }

                    listaTipoHistorico.Add(tipoHistorico);
                }

                return listaTipoHistorico;
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
