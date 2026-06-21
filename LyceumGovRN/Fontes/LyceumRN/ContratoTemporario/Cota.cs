using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Seeduc.Infra.Data;
using Techne.Data;
using System.Data;

namespace Techne.Lyceum.RN.ContratoTemporario
{
    public class Cota : RNBase
    {
        public static QueryTable ListarCotas()
        {
            string sql = "SELECT COTAID,SIGLA FROM CONTRATOTEMPORARIO.COTA";
            return RNBase.Consultar(sql);
        }

        public static QueryTable ListarCotasPorDeficienteFisico()
        {
            string sql = "SELECT COTAID,SIGLA FROM CONTRATOTEMPORARIO.COTA where COTAID in(1,3,4) ";
            return RNBase.Consultar(sql);
        }

        public static QueryTable ListarCotasPorNecessidadeEtnia()
        {
            string sql = "SELECT * FROM CONTRATOTEMPORARIO.COTA WHERE COTAID IN (3,4) ";
            return RNBase.Consultar(sql);
        }

        public static QueryTable ListarCotasPorEtniaIndioOuNegra()
        {
            string sql = "SELECT * FROM CONTRATOTEMPORARIO.COTA where cotaid in (2,3,4) ";
            return RNBase.Consultar(sql);
        }

        public static QueryTable ListarCotasPorEtniaIndioOuNegraOuNenhuma()
        {
            string sql = "SELECT * FROM CONTRATOTEMPORARIO.COTA where cotaid in (1,2,3,4) ";
            return RNBase.Consultar(sql);
        }
        
        public string ObterDescricaoPor(int cotaID)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            string descricao = string.Empty;

            try
            {
                contextQuery.Command = "SELECT DESCRICAO FROM CONTRATOTEMPORARIO.COTA WHERE COTAID = @COTAID";

                contextQuery.Parameters.Add("@COTAID", cotaID);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    if (reader["DESCRICAO"] != DBNull.Value)
                    {
                        descricao = reader["DESCRICAO"].ToString();
                    }
                }
                return descricao;
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

                if (ctx != null)
                {
                    ctx.Dispose();
                }
            }
        }

    }
}
