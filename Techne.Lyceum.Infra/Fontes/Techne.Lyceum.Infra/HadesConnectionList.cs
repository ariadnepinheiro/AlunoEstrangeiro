using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Text;
using Techne.Data;

namespace Techne
{
    public class HadesConnectionList : ConnectionList
    {
        /// <summary>
        ///   Obtém a connection string com user id e password.
        /// </summary>
        public static string GetConnectionString(string applicationName, string webConfigPath)
        {
            var loginConnstr = GetConnectionStringFromWebConfig("Login", webConfigPath);

            var tabDyncls = new QueryTable(
                "SELECT sis, clsid_main, clsid_base, clsid_db, clsid_log " +
                "FROM hd_dyncls " +
                "WHERE sis = ? OR sis = ?"
                );

            var cn = new TConnection(loginConnstr + ";USER ID=login;PASSWORD=login");
            cn.Open();
            try
            {
                tabDyncls.Query(cn, "Hera", applicationName);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }

            string hera;
            {
                SimpleRow rowHera;
                {
                    var rowsHera = tabDyncls.Select("sis = 'Hera'");
                    rowHera = rowsHera.Length == 1 ? rowsHera[0] : null;
                }

                var heraMain = rowHera != null ? rowHera["clsid_main"].ToString() : string.Empty;
                var heraBase = rowHera != null ? rowHera["clsid_base"].ToString() : string.Empty;

                hera = Decrypt(new[] { heraMain, heraBase }, "p3r53f0n3");
            }

            SimpleRow row;
            {
                var rows = tabDyncls.Select("sis = '" + applicationName + "'");
                row = rows.Length == 1 ? rows[0] : null;
            }

            string user;
            if (!row["clsid_main"].IsNull && !row["clsid_base"].IsNull)
            {
                user = Decrypt(new[] { (string)row["clsid_main"], (string)row["clsid_base"] }, hera);
            }
            else
            {
                user = null;
            }

            string password;
            if (!row["clsid_db"].IsNull && !row["clsid_log"].IsNull)
            {
                password = Decrypt(new[] { (string)row["clsid_db"], (string)row["clsid_log"] }, Reverse(hera));
            }
            else
            {
                password = null;
            }

            var connStr = GetConnectionStringFromWebConfig(applicationName, webConfigPath);
            var dicConnStr = StrLib.StrToDictionary(connStr, ';', "=", true);
            if (!dicConnStr.Contains("user id"))
            {
                dicConnStr.Add("user id", user);
            }

            if (!dicConnStr.Contains("password"))
            {
                dicConnStr.Add("password", password);
            }

            return StrLib.DictionaryToStr(dicConnStr, ';', "=");
        }

        internal static TConnectionWritable CreateWritableConnectionWithoutPermission(string application)
        {
            return ConnectionList.CreateWritableConnection(new HadesPermission(string.Empty, string.Empty, true, true, true, true), application);
        }

        protected override ConnectionDictionary InstanceLoad()
        {
            // Pega lista de conexões do config
            var cDatabase = (ConnectionDictionary)System.Configuration.ConfigurationManager.GetSection("techne/database");
            if (cDatabase == null)
            {
                throw new ApplicationException("Está faltando a seção <Database> de conexões com banco de dados no arquivo de configuração");
            }

// Checa se o usuário Login existe
            if (cDatabase["Login"] == null)
            {
                throw new ApplicationException("Está faltando a ConnectionString Login na seção <database> do arquivo de configuração.");
            }

            // Checa se dá para conectar com o usuário Login
            var conn = new TConnection(cDatabase["Login"].ConnectionString + ";PASSWORD=login;USER ID=login");
            try
            {
                conn.Open();
            }
            catch (Exception exc)
            {
                throw new ApplicationException("A ConnectionString de Login definida na seção <database> do arquivo de configuração não é válida.", exc);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            var tabDyncls = new QueryTable(
                "SELECT sis, clsid_main, clsid_base, clsid_db, clsid_log " +
                "FROM hd_dyncls"
                );
            tabDyncls.Query(conn);

            string hera;
            {
                SimpleRow rowHera;
                {
                    var rowsHera = tabDyncls.Select("sis = 'Hera'");
                    rowHera = rowsHera.Length == 1 ? rowsHera[0] : null;
                }

                var heraMain = rowHera != null ? rowHera["clsid_main"].ToString() : string.Empty;
                var heraBase = rowHera != null ? rowHera["clsid_base"].ToString() : string.Empty;

                hera = Decrypt(new[] { heraMain, heraBase }, "p3r53f0n3");
            }

            foreach (var rowDyncls in tabDyncls.Select())
            {
                if (((VarChar)rowDyncls["sis"]).Equals("Hera", true))
                {
                    continue;
                }

                var sis = (string)rowDyncls["sis"];

                string user;
                if (!rowDyncls["clsid_main"].IsNull && !rowDyncls["clsid_base"].IsNull)
                {
                    user = Decrypt(new[] { (string)rowDyncls["clsid_main"], (string)rowDyncls["clsid_base"] }, hera);
                }
                else
                {
                    user = null;
                }

                string password;
                if (!rowDyncls["clsid_db"].IsNull && !rowDyncls["clsid_log"].IsNull)
                {
                    password = Decrypt(new[] { (string)rowDyncls["clsid_db"], (string)rowDyncls["clsid_log"] }, Reverse(hera));
                }
                else
                {
                    password = null;
                }

                if (user != null && password != null && cDatabase[sis] != null)
                {
                    cDatabase[sis].ConnectionString += ";" +
                                                       "USER ID=" + user + ";" +
                                                       "PASSWORD=" + password;
                }
            }

            return cDatabase;
        }

        private static string Decrypt(string[] textos, string chave)
        {
            var enc = Encoding.GetEncoding(1252);
            var bchave = enc.GetBytes(chave == string.Empty ? " " : chave);

            var texto = textos[0] + textos[1];
            texto = texto.Replace("-", string.Empty).Replace("{", string.Empty).Replace("}", string.Empty);

            var btexto = new byte[(texto.Length / 2)];
            for (var i = 0; i < btexto.Length; i++)
            {
                btexto[i] = (byte)int.Parse(texto.Substring(2 * i, 2), NumberStyles.HexNumber);
            }

            var bresult = new byte[(texto.Length / 2)];

            var j = 0;
            byte woffset = 0;

            for (var i = 0; i < btexto.Length; i++)
            {
                var cod = (byte)((btexto[i] - woffset - bchave[j]) & 255);
                bresult[i] = cod;
                woffset = (byte)((woffset + cod * 3 + bchave[j]) & 255);
                j++;
                if (j >= chave.Length)
                {
                    j = 0;
                }
            }

            return enc.GetString(bresult).Trim();
        }

        /// <summary>
        ///   Este procedimento inverte a string passada, EXCETO o último caractere, que é REMOVIDO.
        /// </summary>
        private static string Reverse(string s)
        {
            if (s == null)
            {
                return null;
            }

            var r = new char[s.Length - 1];
            for (int i = 0, j = s.Length - 1; i < s.Length - 1; i++, j--)
            {
                r[i] = s[j];
            }

            return new string(r);
        }
    }
}