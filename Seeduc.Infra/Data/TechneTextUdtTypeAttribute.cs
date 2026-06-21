namespace Seeduc.Infra.Data
{
    using System.Data;

    internal class TechneTextUdtTypeAttribute : TechneUdtTypeAttribute
    {
        public TechneTextUdtTypeAttribute(SqlDbType sqlDbType, int size)
            : base(sqlDbType)
        {
            this.Size = size;
        }

        internal int Size { get; private set; }
    }
}