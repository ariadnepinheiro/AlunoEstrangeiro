namespace Seeduc.Infra.Configuration
{
    using System.Configuration;

    internal class LogConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("enable", DefaultValue = "true", IsRequired = true)]
        public bool Enable
        {
            get
            {
                return (bool)this["enable"];
            }

            set
            {
                this["enable"] = value;
            }
        }
    }
}