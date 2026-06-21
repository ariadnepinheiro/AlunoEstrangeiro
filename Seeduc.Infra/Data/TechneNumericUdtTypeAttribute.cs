namespace Seeduc.Infra.Data
{
    using System.Data;

    internal class TechneNumericUdtTypeAttribute : TechneUdtTypeAttribute
    {
        public TechneNumericUdtTypeAttribute(SqlDbType sqlDbType, byte precision, byte scale)
            : base(sqlDbType)
        {
            this.Precision = precision;
            this.Scale = scale;
        }

        internal byte Precision { get; private set; }

        internal byte Scale { get; private set; }
    }
}