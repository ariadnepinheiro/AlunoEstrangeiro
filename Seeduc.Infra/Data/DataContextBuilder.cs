namespace Seeduc.Infra.Data
{
    using Seeduc.Infra.Configuration;

    public class DataContextBuilder
    {
        private readonly ApplicationName applicationName;

        private DataContextBuilder(ApplicationName applicationName)
        {
            this.applicationName = applicationName;
        }

        public static DataContextBuilder FromHades
        {
            get
            {
                return new DataContextBuilder(ApplicationName.Hades);
            }
        }

        public static DataContextBuilder FromLyceum
        {
            get
            {
                return new DataContextBuilder(ApplicationName.Lyceum);
            }
        }

        public DataContext ToFastReadingOnly()
        {
            return new DataContext(this.applicationName, TransactionContext.WithoutLockAndReadOnly);
        }

        public DataContext UsingLock()
        {
            return new DataContext(this.applicationName, TransactionContext.WithLock);
        }

        public DataContext UsingNoLock()
        {
            return new DataContext(this.applicationName, TransactionContext.WithoutLock);
        }

        public DataContext UsingLockWithConnectionString(string connectionString)
        {
            return new DataContext(connectionString, this.applicationName, TransactionContext.WithLock);
        }

        public DataContext UsingNoLockWithConnectionString(string connectionString)
        {
            return new DataContext(connectionString, this.applicationName, TransactionContext.WithoutLock);
        }
    }
}