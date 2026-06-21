namespace Seeduc.Infra.Configuration
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using log4net.Config;
    using Techne.Data;

    internal class ConfigurationManager
    {
        private readonly IDictionary<ApplicationName, string> connectionStrings = new Dictionary<ApplicationName, string>();

        private readonly SectionHandler sectionHandler = (SectionHandler)System.Configuration.ConfigurationManager.GetSection("seeducInfra");

        private ConfigurationManager()
        {
        }

        public static ConfigurationManager Instance
        {
            get
            {
                return Nested.ConfigurationManagerInstance;
            }
        }

        public SectionHandler Section
        {
            get
            {
                return this.sectionHandler;
            }
        }

        public string GetConnectionString(ApplicationName applicationName)
        {            
            lock (this.connectionStrings)
            {
                if (!this.connectionStrings.ContainsKey(applicationName))
                {
                    var connectionString = ConnectionList.GetConnectionString(applicationName.ToString());

                    this.connectionStrings[applicationName] = Regex.Replace(connectionString, "Provider=SQLOLEDB.1;", string.Empty, RegexOptions.IgnoreCase);
                }

                return this.connectionStrings[applicationName];
            }
        }

        private class Nested
        {
            internal static readonly ConfigurationManager ConfigurationManagerInstance = new ConfigurationManager();

            static Nested()
            {
                // Load log4net configuration
                XmlConfigurator.Configure();
            }
        }
    }
}