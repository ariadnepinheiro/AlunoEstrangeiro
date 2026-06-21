namespace Seeduc.Infra.Data
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class ContextQueryParameter : ICloneable
    {
        public ContextQueryParameter(string name, object value)
        {
            this.Name = name;
            this.Value = value ?? DBNull.Value;
        }

        public ContextQueryParameter(string name, SqlDbType sqlDbType, object value)
            : this(name, value)
        {
            this.SqlDbType = sqlDbType;
        }

        public ContextQueryParameter(string name, TechneDbType techneDbType, object value)
            : this(name, value)
        {
            var attributes = typeof(TechneDbType).GetField(Enum.GetName(typeof(TechneDbType), techneDbType)).GetCustomAttributes(true);

            if (attributes.Length == 0)
            {
                throw new SeeducException(string.Format("There is no custom converter for the TechneDbType \"{0}\" value!", Enum.GetName(typeof(TechneDbType), techneDbType)));
            }

            for (var i = 0; i < attributes.Length; i++)
            {
                var attribute = attributes[i];

                if (attribute is TechneTextUdtTypeAttribute)
                {
                    var techneTextUdtType = (TechneTextUdtTypeAttribute)attribute;

                    this.SqlDbType = techneTextUdtType.SqlDbType;
                    this.Size = techneTextUdtType.Size;

                    return;
                }

                if (attribute is TechneNumericUdtTypeAttribute)
                {
                    var techneNumericUdtType = (TechneNumericUdtTypeAttribute)attribute;

                    this.SqlDbType = techneNumericUdtType.SqlDbType;
                    this.Precision = techneNumericUdtType.Precision;
                    this.Scale = techneNumericUdtType.Scale;

                    return;
                }

                if (attribute is TechneUdtTypeAttribute)
                {
                    var techneNumericUdtType = (TechneUdtTypeAttribute)attribute;

                    this.SqlDbType = techneNumericUdtType.SqlDbType;
                }
            }
        }

        public ContextQueryParameter(string name, SqlDbType sqlDbType, int size, object value)
            : this(name, sqlDbType, value)
        {
            this.Size = size;
        }

        public ContextQueryParameter(string name, SqlDbType sqlDbType, byte precision, byte scale, object value)
            : this(name, sqlDbType, value)
        {
            this.Precision = precision;
            this.Scale = scale;
        }

        public string Name { get; internal set; }

        public byte? Precision { get; internal set; }

        public byte? Scale { get; internal set; }

        public int Size { get; internal set; }

        public SqlDbType? SqlDbType { get; internal set; }

        public object Value { get; internal set; }

        public object Clone()
        {
            var contextQueryParameter = new ContextQueryParameter(this.Name, this.Value);

            if (this.SqlDbType.HasValue)
            {
                contextQueryParameter.SqlDbType = this.SqlDbType;
            }

            if (this.Precision.HasValue)
            {
                contextQueryParameter.Precision = this.Precision;
            }

            if (this.Scale.HasValue)
            {
                contextQueryParameter.Scale = this.Scale;
            }

            return contextQueryParameter;
        }

        internal SqlParameter ToSqlParameter()
        {
            var sqlParameter = new SqlParameter(this.Name, this.Value);

            if (this.SqlDbType.HasValue)
            {
                sqlParameter.SqlDbType = this.SqlDbType.Value;
            }

            if (this.Precision.HasValue)
            {
                sqlParameter.Precision = this.Precision.Value;
            }

            if (this.Scale.HasValue)
            {
                sqlParameter.Scale = this.Scale.Value;
            }

            return sqlParameter;
        }
    }
}