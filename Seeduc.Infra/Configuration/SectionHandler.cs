namespace Seeduc.Infra.Configuration
{
    using System.Configuration;

    internal class SectionHandler : ConfigurationSection
    {
        [ConfigurationProperty("contextQuery")]
        public ContextQueryConfigElement ContextQuery
        {
            get
            {
                return (ContextQueryConfigElement)this["contextQuery"];
            }

            set
            {
                this["contextQuery"] = value;
            }
        }

        [ConfigurationProperty("log")]
        public LogConfigElement Log
        {
            get
            {
                return (LogConfigElement)this["log"];
            }

            set
            {
                this["log"] = value;
            }
        }
    }
}