using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using Techne.Data;

namespace Techne.Web
{
    internal class HadesUtil
    {
        private static string _hadesConnectionString;

        private static string HadesConnectionString
        {
            get
            {
                if (_hadesConnectionString == null)
                {
                    try
                    {
                        _hadesConnectionString = Techne.Data.ConnectionList.GetConnectionString("Hades");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(StringResource.GetString("HadesConnectionStringNotDefined"), ex);
                    }
                }

                return _hadesConnectionString;
            }
        }

        public static DataTable ExecuteQuery(string connStr, string sql, object[] pars)
        {
            DataTable dt = null;
            var conn = new TConnection(connStr);
            conn.Open();
            try
            {
                var qt = new QueryTable(sql);
                qt.Query(conn, DbObject.ToDbObjectArray(pars));
                dt = qt;
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }

        public static DataTable ExecuteQuery(string sql, object[] pars)
        {
            DataTable dt = null;
            var connStr = HadesConnectionString;
            var conn = new TConnection(connStr);
            conn.Open();
            try
            {
                var qt = new QueryTable(sql);
                qt.Query(conn, DbObject.ToDbObjectArray(pars));
                dt = qt;
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }

        public static object GetContextObject(string itemname)
        {
            if (itemname != null &&
                System.Web.HttpContext.Current != null &&
                System.Web.HttpContext.Current.Items[itemname] != null)
            {
                return System.Web.HttpContext.Current.Items[itemname];
            }
            else
            {
                return null;
            }
        }

        public static string GetProviderConnectionString(NameValueCollection config, ref string connect)
        {
            string connStr = null;

// Pega a conexão como o banco de dados
            connect = config["connectionStringName"];

            // se nenhuma conexão foi definida, tenta "Hades" no techne\database
            if (String.IsNullOrEmpty(connect))
            {
                try
                {
                    connStr = Techne.Data.ConnectionList.GetConnectionString("Hades");
                }
                catch
                {
                    connStr = null;
                }
            }

            config.Remove("connectionStringName");

            // Busca na lista de conexões da Techne
            if (connStr == null)
            {
                try
                {
                    connStr = Techne.Data.ConnectionList.GetConnectionString(connect);
                }
                catch
                {
                }

                if (connStr != null && connStr.Trim() == string.Empty)
                {
                    connStr = null;
                }
            }

            return connStr;
        }

        public static void SetContextObject(string itemname, object value)
        {
            if (itemname != null && HttpContext.Current != null)
            {
                System.Web.HttpContext.Current.Items[itemname] = value;
            }
        }
    }
}