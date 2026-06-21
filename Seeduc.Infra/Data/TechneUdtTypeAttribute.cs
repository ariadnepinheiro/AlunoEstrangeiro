namespace Seeduc.Infra.Data
{
    using System;
    using System.Data;

    internal class TechneUdtTypeAttribute : Attribute
    {
        public TechneUdtTypeAttribute(SqlDbType sqlDbType)
        {
            this.SqlDbType = sqlDbType;
        }

        internal SqlDbType SqlDbType { get; private set; }
    }
}