namespace Seeduc.Infra.Configuration
{
    using System.Configuration;

    internal class BulkConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("enable", DefaultValue = true, IsRequired = true)]
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

        [ConfigurationProperty("size", DefaultValue = 10, IsRequired = true)]
        public int Size
        {
            get
            {
                return (int)this["size"];
            }

            set
            {
                this["size"] = value;
            }
        }
    }
}