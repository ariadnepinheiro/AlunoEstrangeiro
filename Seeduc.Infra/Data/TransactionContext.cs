namespace Seeduc.Infra.Data
{
    public enum TransactionContext
    {
        WithLock, 

        WithoutLock, 

        WithoutLockAndReadOnly
    }
}