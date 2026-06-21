namespace Seeduc.Infra.Configuration
{
    using System.Configuration;

    internal class ContextQueryConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("bulk", IsRequired = true)]
        public BulkConfigElement Bulk
        {
            get
            {
                return (BulkConfigElement)this["bulk"];
            }

            set
            {
                this["bulk"] = value;
            }
        }

        [ConfigurationProperty("executionTimeout", DefaultValue = 30, IsRequired = true)]
        public int ExecutionTimeout
        {
            get
            {
                return (int)this["executionTimeout"];
            }

            set
            {
                this["executionTimeout"] = value;
            }
        }
    }
}