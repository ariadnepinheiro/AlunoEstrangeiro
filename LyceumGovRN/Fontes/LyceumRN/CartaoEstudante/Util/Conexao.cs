using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.CartaoEstudante.Util
{
    public class Conexao
    {
        public SqlConnection sqlConnection;
        public SqlConnection openConn()
        {
            try
            {
                sqlConnection = new SqlConnection(System.Text.RegularExpressions.Regex.Replace(Techne.Data.ConnectionList.GetConnectionString("Lyceum"), "Provider=SQLOLEDB.1;", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase));
                
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }
            }
            catch (Exception ex)
            {
                Log.Adicionar(ex.ToString().Replace("\n", ". "));
                throw ex;
            }

            return sqlConnection;
        }

        public void closeConn()
        {
            try
            {
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log.Adicionar(ex.ToString().Replace("\n", ". "));
                throw ex;
            }
        }
    }
}
