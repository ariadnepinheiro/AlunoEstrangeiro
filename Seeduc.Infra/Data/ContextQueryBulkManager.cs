namespace Seeduc.Infra.Data
{
    using System.Collections.Generic;
    using Seeduc.Infra.Configuration;
    using Seeduc.Infra.Helpers;

    internal class ContextQueryBulkManager
    {
        private readonly Queue<ContextQuery> queue;

        private readonly int? size;

        private int contextQueryParameters;

        public ContextQueryBulkManager()
        {
            var configurationManager = ConfigurationManager.Instance;

            this.Enable = configurationManager.Section.ContextQuery.Bulk.Enable;

            if (this.Enable)
            {
                this.size = configurationManager.Section.ContextQuery.Bulk.Size;

                this.queue = new Queue<ContextQuery>(this.size.Value);
            }
        }

        public bool Enable { get; private set; }

        public bool IsEmpty
        {
            get
            {
                if (!this.size.HasValue)
                {
                    return true;
                }

                return this.queue.Count == 0;
            }
        }

        public bool IsFull
        {
            get
            {
                if (!this.size.HasValue)
                {
                    return false;
                }

                // SQL Server has a limitation of 2100 parameters per command
                return this.queue.Count >= this.size.Value
                       || this.contextQueryParameters > 2100;
            }
        }

        public void Enqueue(ContextQuery contextQuery)
        {
            if (this.IsFull)
            {
                return;
            }

            this.contextQueryParameters += contextQuery.Parameters.Count;

            this.queue.Enqueue(contextQuery);
        }

        public ContextQuery Process()
        {
            if (this.queue.Count == 0)
            {
                return new ContextQuery();
            }

            this.contextQueryParameters = 0;

            return ContextQueryHelper.Merge(this.queue);
        }
    }
}