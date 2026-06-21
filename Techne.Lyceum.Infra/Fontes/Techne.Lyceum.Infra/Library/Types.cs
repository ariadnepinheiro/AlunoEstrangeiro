using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace Techne
{
    public enum DbType
    {
        Null, 
        VarChar, 
        Number, 
        Date, 
        Raw
    }
}

namespace Techne.Internal
{
    /// <summary>
    ///   Esta classe não deve ser referenciada.
    ///   Utilizar IDbObject quando isso for necessário.
    /// </summary>
    public abstract class Nullable : IDbObject
    {
        private readonly bool isNull;

        protected Nullable(bool isNull)
        {
            this.isNull = isNull;
        }

        public abstract DbType Type { get; }

        public bool IsNull
        {
            get
            {
                return this.isNull;
            }
        }

        public static bool operator ==(Nullable o1, Nullable o2)
        {
            if ((object)o1 == null)
            {
                return (object)o2 == null;
            }
            else
            {
                return o1.Equals(o2);
            }
        }

        public static bool operator !=(Nullable o1, Nullable o2)
        {
            return !(o1 == o2);
        }

        public abstract object ToObject();

        public abstract string ToString(string format);

        public abstract string ToString(string format, IFormatProvider provider);

        public abstract string ToString(IFormatProvider provider);

        public override bool Equals(object obj)
        {
            if (!(obj is IDbObject))
            {
                return this.ToObject().Equals(obj);
            }

            var obj2 = (IDbObject)obj;
            return (this.isNull && obj2.IsNull) || ((!this.isNull && !obj2.IsNull) && this.ToObject().Equals(obj2.ToObject()));
        }

        public override int GetHashCode()
        {
            return this.ToObject().GetHashCode();
        }
    }
}

namespace Techne
{
    [
        Serializable, 
    ]
    public sealed class DbObject : Techne.Internal.Nullable, ISerializable, IConvertible
    {
        private readonly IDbObject value;

        private DbObject() : base(true)
        {
        }

        private DbObject(IDbObject value) : base(false)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            this.value = value;
        }

        private DbObject(SerializationInfo info, StreamingContext context) : base(info.GetValue("value", typeof (object)) is DBNull)
        {
            var obj = info.GetValue("value", typeof (object));
            if (obj == null)
            {
                throw new ArgumentNullException();
            }

            this.value = ToDbObject(obj);
        }

        public override DbType Type
        {
            get
            {
                return this.IsNull ? DbType.Null : this.value.Type;
            }
        }

        private object DebugValue
        {
            get
            {
                return this.IsNull ? DBNull.Value : this.value.ToObject();
            }
        }

        /// <summary>
        ///   Indica se ToDbObject() devolverá um valor válido. null é válido.
        /// </summary>
        public static bool CanConvertToDbObject(object value)
        {
            if (value == null)
            {
                return true;
            }
            else if (value is DbObject)
            {
                return true;
            }
            else
            {
                return TechLib.GetImplicitConverter(value.GetType(), typeof (DbObject)) != null;
            }
        }

        public static IDbObject FromString(string str, DbType toType, IFormatProvider provider)
        {
            switch (toType)
            {
                case DbType.VarChar:
                    return (VarChar)str;
                case DbType.Number:
                    return (Number)Convert.ToDecimal(str, provider);
                case DbType.Date:
                    return (Date)Convert.ToDateTime(str, provider);
                default:
                    throw new NotImplementedException();
            }
        }

        public static DbType FromType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException();
            }

            if (!typeof (IDbObject).IsAssignableFrom(type))
            {
                throw new ArgumentException("O tipo " + type.FullName + " não é derivado de IDbObject.");
            }

            if (type == typeof (VarChar))
            {
                return DbType.VarChar;
            }
            else if (type == typeof (Number))
            {
                return DbType.Number;
            }
            else if (type == typeof (Date))
            {
                return DbType.Date;
            }
            else if (type == typeof (Raw))
            {
                return DbType.Raw;
            }
            else
            {
                throw new NotImplementedException("Alterar DBObject.FromType() para implementar " + type.FullName + ".");
            }
        }

        /// <summary>
        ///   Se fornecido null, devolverá null.
        /// </summary>
        public static DbObject ToDbObject(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return DBNull.Value;
            }

            if (value is DbObject)
            {
                return (DbObject)value;
            }

            var valueType = value.GetType();
            if (value.Equals(typeof (Techne.VarChar)))
            {
                return (VarChar)value;
            }
            else if (value.Equals(typeof (Techne.Number)))
            {
                return (Techne.Number)value;
            }
            else if (value.Equals(typeof (Techne.Date)))
            {
                return (Techne.Date)value;
            }

            var m = TechLib.GetImplicitConverter(value.GetType(), typeof (DbObject));
            if (m == null)
            {
                throw new ArgumentException("DbObject.ToDbObject(): Tipo não suportado (" + value.GetType().FullName + ").");
            }

            return (DbObject)m.Invoke(null, new[] { value });
        }

        public static DbObject[] ToDbObjectArray(object[] values)
        {
            if (values == null)
            {
                return null;
            }

            var result = new DbObject[values.Length];
            for (var i = 0; i < values.Length; i++)
            {
                result[i] = ToDbObject(values[i]);
            }

            return result;
        }

        public static DbType ToDbType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException();
            }

            if (typeof (IDbObject).IsAssignableFrom(type))
            {
                throw new ArgumentException("O tipo " + type.FullName + " não é do framework.");
            }

            if (type == typeof (string))
            {
                return DbType.VarChar;
            }
            else if (type == typeof (decimal) || type == typeof (int))
            {
                return DbType.Number;
            }
            else if (type == typeof (DateTime))
            {
                return DbType.Date;
            }
            else if (type == typeof (byte[]))
            {
                return DbType.Raw;
            }
            else
            {
                throw new NotImplementedException("Alterar DbObject.ToDbType() para implementar " + type.FullName + ".");
            }
        }

        /// <summary>
        ///   Cria um array onde cada elemento é ToObject() do elemento correspondente no array fornecido.
        /// </summary>
        public static object[] ToObjectArray(DbObject[] values)
        {
            if (values == null)
            {
                return null;
            }

            var result = new object[values.Length];
            for (var i = 0; i < values.Length; i++)
            {
                result[i] = values[i].ToObject();
            }

            return result;
        }

        public static Type ToType(DbType type)
        {
            if (type == DbType.VarChar)
            {
                return typeof (VarChar);
            }
            else if (type == DbType.Number)
            {
                return typeof (Number);
            }
            else if (type == DbType.Date)
            {
                return typeof (Date);
            }
            else if (type == DbType.Raw)
            {
                return typeof (Raw);
            }
            else if (type == DbType.Null)
            {
                throw new ArgumentException();
            }
            else
            {
                throw new NotImplementedException("Alterar DBObject.ToType() para implementar DBObject." + type + ".");
            }
        }

        public static bool operator ==(DbObject o1, DbObject o2)
        {
            return o1 == (Techne.Internal.Nullable)o2;
        }

        // Conversões implícitas de tipos do framework para o tipo.

        // Cast do tipo para outros tipos Techne.
        public static explicit operator VarChar(DbObject t)
        {
            if (t == null)
            {
                return null;
            }
            else if (t.IsNull)
            {
                return DBNull.Value;
            }
            else if (t.value is DbObject)
            {
                return (VarChar)(DbObject)t.value;
            }
            else if (t.value is VarChar)
            {
                return (VarChar)t.value;
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        public static explicit operator Number(DbObject t)
        {
            if (t == null)
            {
                return null;
            }
            else if (t.IsNull)
            {
                return DBNull.Value;
            }
            else if (t.value is DbObject)
            {
                return (Number)(DbObject)t.value;
            }
            else if (t.value is Number)
            {
                return (Number)t.value;
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        public static explicit operator Date(DbObject t)
        {
            if (t == null)
            {
                return null;
            }
            else if (t.IsNull)
            {
                return DBNull.Value;
            }
            else if (t.value is DbObject)
            {
                return (Date)(DbObject)t.value;
            }
            else if (t.value is Date)
            {
                return (Date)t.value;
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        public static explicit operator Raw(DbObject t)
        {
            if (t == null)
            {
                return null;
            }
            else if (t.IsNull)
            {
                return DBNull.Value;
            }
            else if (t.value is DbObject)
            {
                return (Raw)(DbObject)t.value;
            }
            else if (t.value is Raw)
            {
                return (Raw)t.value;
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        // Cast do tipo para tipos do framework.
        public static explicit operator string(DbObject t)
        {
            return (VarChar)t;
        }

        public static explicit operator decimal(DbObject t)
        {
            return (decimal)(Number)t;
        }

        public static explicit operator double(DbObject t)
        {
            return (double)(Number)t;
        }

        public static explicit operator int(DbObject t)
        {
            return (int)(Number)t;
        }

        public static explicit operator DateTime(DbObject t)
        {
            return (DateTime)(Date)t;
        }

        public static explicit operator byte[](DbObject t)
        {
            return (byte[])(Raw)t;
        }

        public static explicit operator decimal?(DbObject t)
        {
            return (Number)t;
        }

        public static explicit operator double?(DbObject t)
        {
            return (Number)t;
        }

        public static explicit operator int?(DbObject t)
        {
            return (int?)(Number)t;
        }

        public static explicit operator DateTime?(DbObject t)
        {
            return (Date)t;
        }

        public static implicit operator DbObject(DBNull n)
        {
            if (n == null)
            {
                throw new ArgumentNullException();
            }

            return new DbObject();
        }

        public static implicit operator DbObject(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }

            return new DbObject((VarChar)s);
        }

        public static implicit operator DbObject(decimal m)
        {
            return new DbObject((Number)m);
        }

        public static implicit operator DbObject(double d)
        {
            return new DbObject((Number)d);
        }

        public static implicit operator DbObject(int i)
        {
            return new DbObject((Number)i);
        }

        public static implicit operator DbObject(short s)
        {
            // System.Int16
            return new DbObject((Number)s);
        }

        public static implicit operator DbObject(DateTime d)
        {
            return new DbObject((Date)d);
        }

        public static implicit operator DbObject(byte[] b)
        {
            return new DbObject((Raw)b);
        }

        public static implicit operator DbObject(decimal? m)
        {
            return new DbObject((Number)m);
        }

        public static implicit operator DbObject(double? d)
        {
            return new DbObject((Number)d);
        }

        public static implicit operator DbObject(int? i)
        {
            return new DbObject((Number)i);
        }

        public static implicit operator DbObject(short? s)
        {
            // System.Int16
            return new DbObject((Number)s);
        }

        public static implicit operator DbObject(DateTime? d)
        {
            return new DbObject((Date)d);
        }

        public static bool operator !=(DbObject o1, DbObject o2)
        {
            return !(o1 == o2);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        ///   Nunca devolve null.
        /// </summary>
        public override object ToObject()
        {
            return this.IsNull ? DBNull.Value : this.value.ToObject();
        }

        public override string ToString()
        {
            return this.IsNull ? string.Empty : this.value.ToString();
        }

        public override string ToString(string format)
        {
            return this.IsNull ? string.Empty : this.value.ToString(format);
        }

        public override string ToString(string format, IFormatProvider provider)
        {
            return this.IsNull ? string.Empty : this.value.ToString(format, provider);
        }

        public override string ToString(IFormatProvider provider)
        {
            return this.IsNull ? string.Empty : this.value.ToString(provider);
        }

        public TypeCode GetTypeCode()
        {
            if (this.value == null || this.IsNull)
            {
                return TypeCode.DBNull;
            }

            switch (this.value.Type)
            {
                case Techne.DbType.VarChar:
                    return TypeCode.String;
                case Techne.DbType.Date:
                    return TypeCode.DateTime;
                case Techne.DbType.Number:
                    return TypeCode.Decimal;
                case Techne.DbType.Null:
                    return TypeCode.DBNull;
                case Techne.DbType.Raw:
                default:
                    return TypeCode.Object;
            }
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(this.ToObject());
        }

        public byte ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(this.ToObject());
        }

        public char ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(this.ToObject());
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            return Convert.ToDateTime(this.ToObject());
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(this.ToObject());
        }

        public double ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(this.ToObject());
        }

        public short ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(this.ToObject());
        }

        public int ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(this.ToObject());
        }

        public long ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(this.ToObject());
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(this.ToObject());
        }

        public float ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(this.ToObject());
        }

        /// <summary>
        ///   Converte o valor (DbObject) para o tipo específico (VarChar, Number, Date...).
        ///   Necessário na passagem de parâmetros ao chamar métodos via Invoke() (reflection)
        ///   (situação na qual não é possível fazer o cast dos parâmetros).
        ///   Quando o valor é nulo, converte para o tipo fornecido.
        /// </summary>
        public IDbObject ToSpecific(DbType typeWhenNull)
        {
            if (typeWhenNull == DbType.Null)
            {
                throw new ArgumentException();
            }

            object value = this.Type == DbType.Null ? DBNull.Value : this;
            var type = this.Type != DbType.Null ? this.Type : typeWhenNull;

            switch (type)
            {
                case DbType.VarChar:
                    return value.GetType() == typeof (DbObject) ? (VarChar)(DbObject)value : (VarChar)value;
                case DbType.Number:
                    return value.GetType() == typeof (DbObject) ? (Number)(DbObject)value : (Number)value;
                case DbType.Date:
                    return value.GetType() == typeof (DbObject) ? (Date)(DbObject)value : (Date)value;
                case DbType.Raw:
                    return value.GetType() == typeof (DbObject) ? (Raw)(DbObject)value : (Raw)value;
                case DbType.Null:
                    throw new ApplicationException();
                default:
                    throw new NotImplementedException("DbObject.ToSpecific() não implementa o tipo " + this.Type + ".");
            }
        }

        public object ToSpecific(Type typeWhenNull)
        {
            if (this.Type != DbType.Null)
            {
                return this.ToSpecific(this.Type);
            }
            else if (typeWhenNull.IsSubclassOf(typeof (ISpecific)))
            {
                return this.ToSpecific(FromType(typeWhenNull));
            }
            else if (typeWhenNull == typeof (object) || typeWhenNull == typeof (DBNull))
            {
                return DBNull.Value;
            }
            else
            {
                throw new ArgumentException("O valor nulo não pode ser representado pelo tipo fornecido (" + typeWhenNull.FullName + ").");
            }
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            if (conversionType == typeof (bool))
            {
                return this.ToBoolean(provider);
            }
            else if (conversionType == typeof (string))
            {
                return ToString(provider);
            }
            else if (conversionType == typeof (byte))
            {
                return this.ToByte(provider);
            }
            else if (conversionType == typeof (char))
            {
                return this.ToChar(provider);
            }
            else if (conversionType == typeof (DateTime))
            {
                return this.ToDateTime(provider);
            }
            else if (conversionType == typeof (decimal))
            {
                return this.ToDecimal(provider);
            }
            else if (conversionType == typeof (double))
            {
                return this.ToDouble(provider);
            }
            else if (conversionType == typeof (Int16))
            {
                return this.ToInt16(provider);
            }
            else if (conversionType == typeof (Int32))
            {
                return this.ToInt32(provider);
            }
            else if (conversionType == typeof (Int64))
            {
                return this.ToInt64(provider);
            }
            else if (conversionType == typeof (sbyte))
            {
                return this.ToSByte(provider);
            }
            else if (conversionType == typeof (Single))
            {
                return this.ToSingle(provider);
            }
            else if (conversionType == typeof (UInt16))
            {
                return this.ToUInt16(provider);
            }
            else if (conversionType == typeof (UInt32))
            {
                return this.ToUInt32(provider);
            }
            else if (conversionType == typeof (UInt64))
            {
                return this.ToUInt64(provider);
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(this.ToObject());
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(this.ToObject());
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(this.ToObject());
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("value", this.ToObject());
        }
    }

    public sealed class VarChar : Techne.Internal.Nullable, ISpecific
    {
        private readonly string value;

        private VarChar() : base(true)
        {
        }

        private VarChar(string value) : base(false)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            this.value = value;
        }

        public override DbType Type
        {
            get
            {
                return this.IsNull ? DbType.Null : DbType.VarChar;
            }
        }

        public int Length
        {
            get
            {
                return this.IsNull ? 0 : this.value.Length;
            }
        }

        private object DebugValue
        {
            get
            {
                return this.IsNull ? DBNull.Value : (object)this.value;
            }
        }

        public static bool operator ==(VarChar v1, VarChar v2)
        {
            return v1 == (Techne.Internal.Nullable)v2;
        }

        // Conversão implícita do tipo para DbObject (simula herança).
        public static implicit operator DbObject(VarChar v)
        {
            if (v == null)
            {
                return null;
            }
            else if (v.IsNull)
            {
                return DBNull.Value;
            }
            else
            {
                return v.value;
            }
        }

        // Conversões implícitas de tipos do framework para o tipo.
        public static implicit operator VarChar(DBNull n)
        {
            if (n == null)
            {
                throw new ArgumentNullException();
            }

            return new VarChar();
        }

        public static implicit operator VarChar(string s)
        {
            if (s == null)
            {
                var v = new VarChar();
                v = DBNull.Value;
                return v;
            }
            else
            {
                return new VarChar(s);
            }
        }

        // Cast do tipo para tipos do framework.
        public static implicit operator string(VarChar v)
        {
            if ((object)v == null || v.IsNull)
            {
                return null; // O cast para object é para não chamar o == de VarChar.
            }
            else
            {
                return v.value;
            }
        }

        public static bool operator !=(VarChar v1, VarChar v2)
        {
            return !(v1 == v2);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override object ToObject()
        {
            return this.IsNull ? DBNull.Value : (object)this.value;
        }

        public override string ToString()
        {
            if (this.IsNull)
            {
                return string.Empty;
            }
            else
            {
                return this.value;
            }
        }

        public override string ToString(string format)
        {
            return this.ToString();
        }

        public override string ToString(string format, IFormatProvider provider)
        {
            return this.ToString();
        }

        public override string ToString(IFormatProvider provider)
        {
            return this.ToString();
        }

        public bool Equals(VarChar v, bool ignoreCase)
        {
            if (v == null)
            {
                throw new ArgumentNullException();
            }

            if (!this.IsNull && !v.IsNull && ignoreCase)
            {
                return string.Compare(this.value, v, true) == 0;
            }
            else
            {
                return this.Equals(v);
            }
        }

        public bool Equals(string v)
        {
            return v.Equals(this.value);
        }

        public bool Equals(string v, StringComparison comparisonType)
        {
            return v.Equals(this.value, comparisonType);
        }

        public TypeCode GetTypeCode()
        {
            if (this.value != null)
            {
                return this.value.GetTypeCode();
            }
            else
            {
                return TypeCode.String;
            }
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToBoolean(this.value, provider);
        }

        public byte ToByte(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToByte(this.value, provider);
        }

        public char ToChar(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToChar(this.value, provider);
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToDateTime(this.value, provider);
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToDecimal(this.value, provider);
        }

        public double ToDouble(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToDouble(this.value, provider);
        }

        public short ToInt16(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToInt16(this.value, provider);
        }

        public int ToInt32(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToInt32(this.value, provider);
        }

        public long ToInt64(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToInt64(this.value, provider);
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToSByte(this.value, provider);
        }

        public float ToSingle(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToSingle(this.value, provider);
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return ((IConvertible)this.value).ToType(conversionType, provider);
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToUInt16(this.value, provider);
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToUInt32(this.value, provider);
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToUInt64(this.value, provider);
        }
    }

    public sealed class Number : Techne.Internal.Nullable, ISpecific, IConvertible
    {
        private decimal value;

        private Number() : base(true)
        {
        }

        private Number(decimal value) : base(false)
        {
            this.value = (decimal)DataLib.FixOleDbValue(value);
        }

        public override DbType Type
        {
            get
            {
                return this.IsNull ? DbType.Null : DbType.Number;
            }
        }

        private object DebugValue
        {
            get
            {
                return this.IsNull ? DBNull.Value : (object)this.value;
            }
        }

        public static decimal operator +(Number n1, Number n2)
        {
            if ((object)n2 == null)
            {
                throw new ArgumentNullException();
            }

            if (n1.IsNull || n2.IsNull)
            {
                throw new InvalidNullOperationException();
            }

            return n1.value + n2.value;
        }

        public static Number operator --(Number n)
        {
            if (n.IsNull)
            {
                throw new InvalidNullOperationException();
            }

            n.value--;
            return n;
        }

        public static decimal operator /(Number n1, Number n2)
        {
            if ((object)n2 == null)
            {
                throw new ArgumentNullException();
            }

            if (n1.IsNull || n2.IsNull)
            {
                throw new InvalidNullOperationException();
            }

            return n1.value / n2.value;
        }

        public static bool operator ==(Number n1, Number n2)
        {
            return n1 == (Techne.Internal.Nullable)n2;
        }

        public static explicit operator decimal(Number n)
        {
            if ((object)n == null)
            {
                throw new InvalidCastException(); // O cast para object é para não chamar o == de Number.
            }

            if (n.IsNull)
            {
                throw new InvalidCastException();
            }

            return n.value;
        }

        public static explicit operator double(Number n)
        {
            if ((object)n == null)
            {
                throw new InvalidCastException(); // O cast para object é para não chamar o == de Number.
            }

            if (n.IsNull)
            {
                throw new InvalidCastException();
            }

            return (double)n.value;
        }

        public static explicit operator int(Number n)
        {
            if ((object)n == null)
            {
                throw new InvalidCastException(); // O cast para object é para não chamar o == de Number.
            }

            if (n.IsNull)
            {
                throw new InvalidCastException();
            }

            return (int)n.value;
        }

        public static explicit operator int?(Number n)
        {
            if ((object)n == null || n.IsNull)
            {
                return null;
            }
            else
            {
                var i = Convert.ToInt32(n.value);
                return i;
            }
        }

        public static explicit operator short?(Number n)
        {
            if ((object)n == null || n.IsNull)
            {
                return null;
            }
            else
            {
                int s = Convert.ToInt16(n.value);
                return (short?)s;
            }
        }

        public static bool operator >(Number n1, Number n2)
        {
            if (n1.IsNull || n2.IsNull)
            {
                return false;
            }

            return (decimal)n1 > (decimal)n2;
        }

        public static bool operator >=(Number n1, Number n2)
        {
            return n1 == n2 || n1 > n2;
        }

        // Conversão implícita do tipo para DbObject (simula herança).
        public static implicit operator DbObject(Number n)
        {
            if (n == null)
            {
                return null;
            }
            else if (n.IsNull)
            {
                return DBNull.Value;
            }
            else
            {
                return n.value;
            }
        }

        // Conversões implícitas de tipos do framework para o tipo.
        public static implicit operator Number(DBNull n)
        {
            return new Number();
        }

        public static implicit operator Number(decimal m)
        {
            return new Number(m);
        }

        public static implicit operator Number(double d)
        {
            return new Number((decimal)d);
        }

        public static implicit operator Number(int i)
        {
            return new Number(i);
        }

        public static implicit operator Number(short s)
        {
            // System.Int16
            return new Number(s);
        }

        public static implicit operator Number(decimal? d)
        {
            if (d.HasValue)
            {
                return new Number(d.Value);
            }
            else
            {
                var n = new Number();
                n = DBNull.Value;
                return n;
            }
        }

        public static implicit operator Number(double? d)
        {
            if (d.HasValue)
            {
                return new Number((decimal)d.Value);
            }
            else
            {
                var n = new Number();
                n = DBNull.Value;
                return n;
            }
        }

        public static implicit operator Number(int? d)
        {
            if (d.HasValue)
            {
                return new Number(d.Value);
            }
            else
            {
                var n = new Number();
                n = DBNull.Value;
                return n;
            }
        }

        public static implicit operator Number(short? d)
        {
            if (d.HasValue)
            {
                return new Number(d.Value);
            }
            else
            {
                var n = new Number();
                n = DBNull.Value;
                return n;
            }
        }

        // Cast do tipo para tipos do framework.
        public static implicit operator decimal?(Number n)
        {
            if ((object)n == null || n.IsNull)
            {
                return null;
            }
            else
            {
                return n.value;
            }
        }

        public static implicit operator double?(Number n)
        {
            if ((object)n == null || n.IsNull)
            {
                return null;
            }
            else
            {
                var d = Convert.ToDouble(n.value);
                return d;
            }
        }

        public static Number operator ++(Number n)
        {
            if (n.IsNull)
            {
                throw new InvalidNullOperationException();
            }

            n.value++;
            return n;
        }

        public static bool operator !=(Number n1, Number n2)
        {
            return !(n1 == n2);
        }

        public static bool operator <(Number n1, Number n2)
        {
            if (n1.IsNull || n2.IsNull)
            {
                return false;
            }

            return (decimal)n1 < (decimal)n2;
        }

        public static bool operator <=(Number n1, Number n2)
        {
            return n1 == n2 || n1 < n2;
        }

        public static decimal operator *(Number n1, Number n2)
        {
            if ((object)n2 == null)
            {
                throw new ArgumentNullException();
            }

            if (n1.IsNull || n2.IsNull)
            {
                throw new InvalidNullOperationException();
            }

            return n1.value * n2.value;
        }

        public static decimal operator -(Number n1, Number n2)
        {
            if ((object)n2 == null)
            {
                throw new ArgumentNullException();
            }

            if (n1.IsNull || n2.IsNull)
            {
                throw new InvalidNullOperationException();
            }

            return n1.value - n2.value;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override object ToObject()
        {
            return this.IsNull ? DBNull.Value : (object)this.value;
        }

        public override string ToString()
        {
            return this.IsNull ? string.Empty : this.value.ToString();
        }

        public override string ToString(string format, IFormatProvider provider)
        {
            return this.IsNull ? string.Empty : this.value.ToString(format, provider);
        }

        public override string ToString(string format)
        {
            return this.IsNull ? string.Empty : this.value.ToString(format);
        }

        public override string ToString(IFormatProvider provider)
        {
            return this.IsNull ? string.Empty : this.value.ToString(provider);
        }

        public TypeCode GetTypeCode()
        {
            return this.value.GetTypeCode();
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToBoolean(this.value, provider);
        }

        public byte ToByte(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToByte(this.value, provider);
        }

        public char ToChar(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToChar(this.value, provider);
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToDateTime(this.value, provider);
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToDecimal(this.value, provider);
        }

        public double ToDouble(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToDouble(this.value, provider);
        }

        public short ToInt16(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToInt16(this.value, provider);
        }

        public int ToInt32(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToInt32(this.value, provider);
        }

        public long ToInt64(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToInt64(this.value, provider);
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToSByte(this.value, provider);
        }

        public float ToSingle(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToSingle(this.value, provider);
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return ((IConvertible)this.value).ToType(conversionType, provider);
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToUInt16(this.value, provider);
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToUInt32(this.value, provider);
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToUInt64(this.value, provider);
        }
    }

    public sealed class Date : Techne.Internal.Nullable, ISpecific, IConvertible
    {
        private DateTime value;

        private Date() : base(true)
        {
        }

        private Date(DateTime value) : base(false)
        {
            this.value = value;
        }

        public override DbType Type
        {
            get
            {
                return this.IsNull ? DbType.Null : DbType.Date;
            }
        }

        private object DebugValue
        {
            get
            {
                return this.IsNull ? DBNull.Value : (object)this.value;
            }
        }

        public static bool operator ==(Date d1, Date d2)
        {
            return d1 == (Techne.Internal.Nullable)d2;
        }

        public static explicit operator DateTime(Date d)
        {
            if (d.IsNull)
            {
                throw new InvalidCastException();
            }

            return d.value;
        }

        public static bool operator >(Date d1, Date d2)
        {
            if (d1.IsNull || d2.IsNull)
            {
                return false;
            }

            return d1.value > d2.value;
        }

        public static bool operator >=(Date d1, Date d2)
        {
            return d1 == d2 || d1 > d2;
        }

        // Conversão implícita do tipo para DbObject (simula herança).
        public static implicit operator DbObject(Date d)
        {
            if (d == null)
            {
                return null;
            }
            else if (d.IsNull)
            {
                return DBNull.Value;
            }
            else
            {
                return d.value;
            }
        }

        // Conversões implícitas de tipos do framework para o tipo.
        public static implicit operator Date(DBNull n)
        {
            return new Date();
        }

        public static implicit operator Date(DateTime d)
        {
            return new Date(d);
        }

        // Cast do tipo para tipos do framework.
        public static implicit operator DateTime?(Date d)
        {
            if ((object)d == null || d.IsNull)
            {
                return null;
            }
            else
            {
                return d.value;
            }
        }

        public static bool operator !=(Date d1, Date d2)
        {
            return !(d1 == d2);
        }

        public static bool operator <(Date d1, Date d2)
        {
            if (d1.IsNull || d2.IsNull)
            {
                return false;
            }

            return d1.value < d2.value;
        }

        public static bool operator <=(Date d1, Date d2)
        {
            return d1 == d2 || d1 < d2;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override object ToObject()
        {
            return this.IsNull ? DBNull.Value : (object)this.value;
        }

        public override string ToString()
        {
            return this.IsNull ? string.Empty : this.value.ToString();
        }

        public override string ToString(string format, IFormatProvider provider)
        {
            return this.IsNull ? string.Empty : this.value.ToString(format, provider);
        }

        public override string ToString(string format)
        {
            return this.IsNull ? string.Empty : this.value.ToString(format);
        }

        public override string ToString(IFormatProvider provider)
        {
            return this.IsNull ? string.Empty : this.value.ToString(provider);
        }

        public TypeCode GetTypeCode()
        {
            return this.value.GetTypeCode();
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToBoolean(this.value, provider);
        }

        public byte ToByte(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToByte(this.value, provider);
        }

        public char ToChar(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToChar(this.value, provider);
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToDateTime(this.value, provider);
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToDecimal(this.value, provider);
        }

        public double ToDouble(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToDouble(this.value, provider);
        }

        public short ToInt16(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToInt16(this.value, provider);
        }

        public int ToInt32(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToInt32(this.value, provider);
        }

        public long ToInt64(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToInt64(this.value, provider);
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToSByte(this.value, provider);
        }

        public float ToSingle(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToSingle(this.value, provider);
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return ((IConvertible)this.value).ToType(conversionType, provider);
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToUInt16(this.value, provider);
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToUInt32(this.value, provider);
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            if (this.IsNull)
            {
                throw new InvalidCastException();
            }

            return Convert.ToUInt64(this.value, provider);
        }
    }

    public sealed class Raw : Techne.Internal.Nullable, ISpecific
    {
        private readonly byte[] value;

        private Raw() : base(true)
        {
        }

        private Raw(byte[] value) : base(false)
        {
            this.value = value;
        }

        public override DbType Type
        {
            get
            {
                if (!base.IsNull)
                {
                    return DbType.Raw;
                }

                return DbType.Null;
            }
        }

        private object DebugValue
        {
            get
            {
                if (!base.IsNull)
                {
                    return this.value;
                }

                return DBNull.Value;
            }
        }

        public static explicit operator byte[](Raw r)
        {
            if (r.IsNull)
            {
                throw new InvalidCastException();
            }

            return r.value;
        }

        // Conversão implícita do tipo para DbObject (simula herança).
        public static implicit operator DbObject(Raw r)
        {
            if (r == null)
            {
                return null;
            }
            else if (r.IsNull)
            {
                return DBNull.Value;
            }
            else
            {
                return r.value;
            }
        }

        // Conversões implícitas de tipos do framework para o tipo.
        public static implicit operator Raw(DBNull n)
        {
            return new Raw();
        }

        public static implicit operator Raw(byte[] b)
        {
            return new Raw(b);
        }

        // Cast do tipo para tipos do framework.
        public override object ToObject()
        {
            if (!base.IsNull)
            {
                return this.value;
            }

            return DBNull.Value;
        }

        public override string ToString(string format)
        {
            return this.ToString();
        }

        public override string ToString(string format, IFormatProvider provider)
        {
            return this.ToString();
        }

        public override string ToString(IFormatProvider provider)
        {
            return this.ToString();
        }

        public override string ToString()
        {
            if (!this.IsNull)
            {
                return "{ " + StrLib.EnumerableToStr(this.value, ", ") + " }";
            }
            else
            {
                return string.Empty;
            }
        }
    }

    public class InvalidNullOperationException : ApplicationException
    {
        public InvalidNullOperationException() : base("Operação inválida com null.")
        {
        }
    }

    public interface IDbObject
    {
        bool IsNull { get; }

        DbType Type { get; }

        object ToObject();

        string ToString(string format);

        string ToString(string format, IFormatProvider provider);

        string ToString(IFormatProvider provider);
    }

    public interface ISpecific
    {
    }
}