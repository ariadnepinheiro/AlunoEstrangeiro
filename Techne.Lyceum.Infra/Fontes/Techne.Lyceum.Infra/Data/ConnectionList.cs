using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Xml;

namespace Techne.Data
{
    public class ConnectionList
    {
        private static ConnectionList connectionList;

        private ConnectionDictionary connections = new ConnectionDictionary();

        private bool loaded;

        [
            EditorBrowsable(EditorBrowsableState.Never),
        ]
        public static ConnectionList Current
        {
            get
            {
                return connectionList;
            }

            set
            {
                connectionList = value;
            }
        }

        /// <summary>
        ///   Cria uma conexão a partir de um item da lista de connections strings do arquivo de configuração (web.config).
        /// </summary>
        /// <param name = "application">Um elemento da lista de connection strings declarada na seção configuration/techne/database do web.config.</param>
        public static TConnection CreateConnection(string application)
        {
            return CreateConnection(application, string.Empty);
        }

        /// <summary>
        ///   Cria uma conexão a partir de um item da lista de connections strings do arquivo de configuração (web.config).
        /// </summary>
        /// <param name = "application">Um elemento da lista de connection strings declarada na seção configuration/techne/database do web.config.</param>
        public static TConnection CreateConnection(string application, string transacao)
        {
            var connStr = ConnectionList.GetConnectionString(application);
            if (connStr == null)
            {
                throw new ArgumentException("O item '" + application + "' não foi encontrado na lista de connection strings.");
            }

            return new TConnection(connStr, transacao);
        }

        /// <summary>
        ///   Cria uma conexão a partir de um item da lista de connections strings do arquivo de configuração (web.config).
        /// </summary>
        /// <param name = "application">Um elemento da lista de connection strings declarada na seção configuration/techne/database do web.config.</param>
        public static TConnectionWritable CreateWritableConnection(TPermission permission, string application)
        {
            return CreateWritableConnection(permission, application, string.Empty);
        }

        /// <summary>
        ///   Cria uma conexão a partir de um item da lista de connections strings do arquivo de configuração (web.config).
        /// </summary>
        /// <param name = "application">Um elemento da lista de connection strings declarada na seção configuration/techne/database do web.config.</param>
        public static TConnectionWritable CreateWritableConnection(TPermission permission, string application, string transacao)
        {
            var connStr = ConnectionList.GetConnectionString(application);
            if (connStr == null)
            {
                throw new ArgumentException("O item '" + application + "' não foi encontrado na lista de connection strings.");
            }

            return new TConnectionWritable(permission, connStr, transacao);
        }

        /// <summary>
        ///   Cria uma conexão a partir de um item da lista de connections strings do arquivo de configuração (web.config).
        /// </summary>
        /// <param name = "application">Um elemento da lista de connection strings declarada na seção configuration/techne/database do web.config.</param>
        public static TConnectionWritable CreateWritableConnection(string application, string transacao)
        {
            var connStr = ConnectionList.GetConnectionString(application);
            if (connStr == null)
            {
                throw new ArgumentException("O item '" + application + "' não foi encontrado na lista de connection strings.");
            }

            if (TPermission.ThreadPermission != null)
            {
                return new TConnectionWritable(connStr, transacao);
            }
            else
            {
                return TConnectionWritable.CreateWithoutPermission(connStr);
            }
        }

        /// <summary>
        ///   Cria uma conexão a partir de um item da lista de connections strings do arquivo de configuração (web.config).
        /// </summary>
        /// <param name = "application">Um elemento da lista de connection strings declarada na seção configuration/techne/database do web.config.</param>
        public static TConnectionWritable CreateWritableConnection(string application)
        {
            return CreateWritableConnection(application, string.Empty);
        }

        /// <summary>
        ///   Retorna uma ConnectString da lista
        /// </summary>
        public static string GetConnectionString(string key)
        {
            string keyComplete = string.Format("ConnectString{0}", key);
            string cache = GetKeyFromCache(keyComplete);

            if (string.IsNullOrEmpty(cache))
            {
                if (key == null || key.Trim().Length == 0)
                {
                    throw new ArgumentException("O indexador da connect string não foi informado.", "key");
                }

                lock (connectionList)
                {
                    if (connectionList == null)
                    {
                        connectionList = new ConnectionList();
                    }

                    if (!connectionList.loaded)
                    {
                        Load();
                    }

                    if (connectionList.connections[key] == null)
                    {
                        connectionList.loaded = false;
                        throw new ArgumentException(
                            "A connect string indexada por '" + key + "' não foi encontrada. Verifique a seção configuration/techne/database " +
                            "do arquivo web.config e se o sistema foi cadastrado via HadesKernelLoginConfig.",
                            "key"
                            );
                    }

                    cache = connectionList.connections[key].ConnectionString;
                }

                SetCacheWithKey(keyComplete, cache);
            }

            return cache;
        }

        private static void SetCacheWithKey(string key, Object cache)
        {
            try
            {
                TimeSpan timeSpan = new TimeSpan(24, 0, 0);
                System.Web.HttpRuntime.Cache.Add(key, cache, null, System.Web.Caching.Cache.NoAbsoluteExpiration,
                    timeSpan,
                    System.Web.Caching.CacheItemPriority.Default,
                    null);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetKeyFromCache(string key)
        {
            try
            {
                string stringConnection = Convert.ToString(System.Web.HttpRuntime.Cache[key]);
                return stringConnection;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///   Preenche a lista de conexões com os valores contidos na seção &lt;Database%gt; do arquivo .config da aplicação
        /// </summary>
        public static void Load()
        {
            connectionList.connections = connectionList.InstanceLoad();
            connectionList.loaded = true;
        }

        internal static TConnectionWritable CreateWritableConnectionWithoutPermission(string application)
        {
            var connStr = ConnectionList.GetConnectionString(application);
            if (connStr == null)
            {
                throw new ArgumentException("O item '" + application + "' não foi encontrado na lista de connection strings.");
            }

            return TConnectionWritable.CreateWithoutPermission(connStr);
        }

        /// <summary>
        ///   Abre o arquivo web.config no path informado e obtém a connection string da aplicação solicitada
        ///   (xpath: /configuration/techne/database/add[@key = applicationName]).
        /// </summary>
        protected static string GetConnectionStringFromWebConfig(string applicationName, string webConfigPath)
        {
            var filepath = Path.Combine(webConfigPath, "web.config");
            var xpath = "/configuration/techne/database/add[@key = \"" + applicationName + "\"]";

            var doc = new XmlDocument();
            doc.Load(filepath);

            var node = doc.SelectSingleNode(xpath);
            var attrib = node != null ? node.Attributes["value"] : null;

            if (attrib == null)
            {
                throw new ApplicationException("Connection string da aplicação '" + applicationName + "' não encontrada no arquivo '" + filepath + "'.");
            }

            return attrib.Value;
        }

        protected virtual ConnectionDictionary InstanceLoad()
        {
            var connections = System.Configuration.ConfigurationManager.GetSection("techne/database") as ConnectionDictionary;

            if (connections == null)
            {
                connections = new ConnectionDictionary();
            }

            foreach (DictionaryEntry connection in connections)
            {
                var connectionString = ((ConnectionInfo)connection.Value).ConnectionString;
                var dic = StrLib.StrToDictionary(connectionString, ';', "=", true);
                if ((!dic.Contains("user id") && !dic.Contains("password")) && (!dic.Contains("integrated security") && (DbLib.GetRdbms(connectionString) == Rdbms.SQLServer)))
                {
                    dic.Add("integrated security", "sspi");
                    connections[(string)connection.Key].ConnectionString = StrLib.DictionaryToStr(dic, ';', "=");
                }
            }

            return connections;
        }
    }
}