using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace Techne.Data
{
    public class TParameter
    {
        private static readonly DateTime minDateTime;

        private readonly DbParameter par;

        static TParameter()
        {
            minDateTime = new DateTime(1753, 1, 1);
        }

        public TParameter()
        {
            this.par = TFactory.Instance.CreateParameter();
        }

        public TParameter(string parameterName, System.Data.DbType dbType) : this()
        {
            this.par.ParameterName = parameterName;
            this.par.DbType = dbType;
        }

        public TParameter(string parameterName, DbObject parameterValue) : this()
        {
            if (parameterValue == null)
            {
                throw new ArgumentNullException();
            }

            GetDateTimeError(parameterValue, true);
            this.par.ParameterName = parameterName;
            this.par.Value = parameterValue.ToObject();
        }

        public TParameter(string parameterName, System.Data.DbType dbType, int size, ParameterDirection direction, bool isNullable, 
                          byte precision, byte scale, string srcColumn, DataRowVersion srcVersion, DbObject value) : this()
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            GetDateTimeError(value, true);

            this.par.ParameterName = parameterName;
            this.par.DbType = dbType;
            this.par.Size = size;
            this.par.Direction = direction;
            this.par.IsNullable = isNullable;
            if (this.par is IDbDataParameter)
            {
                ((IDbDataParameter)this.par).Precision = precision;
                ((IDbDataParameter)this.par).Scale = scale;
            }

            this.par.SourceColumn = srcColumn;
            this.par.SourceVersion = srcVersion;
            this.par.Value = value.ToObject();
        }

        public TParameter(string parameterName, OleDbType dbType, int size, ParameterDirection direction, bool isNullable, 
                          byte precision, byte scale, string srcColumn, DataRowVersion srcVersion, DbObject value) : this()
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            GetDateTimeError(value, true);

            switch (dbType)
            {
                case System.Data.OleDb.OleDbType.VarChar:
                    this.par.DbType = System.Data.DbType.String;
                    break;
                case System.Data.OleDb.OleDbType.Numeric:
                    this.par.DbType = System.Data.DbType.Decimal;
                    break;
                case System.Data.OleDb.OleDbType.Date:
                    this.par.DbType = System.Data.DbType.DateTime;
                    break;
                case System.Data.OleDb.OleDbType.Integer:
                    this.par.DbType = System.Data.DbType.Int32;
                    break;
                case System.Data.OleDb.OleDbType.VarBinary:
                    this.par.DbType = System.Data.DbType.Binary;
                    break;
                default:
                    this.par.DbType = System.Data.DbType.String;
                    break;
            }

            this.par.ParameterName = parameterName;
            this.par.Size = size;
            this.par.Direction = direction;
            this.par.IsNullable = isNullable;
            if (this.par is IDbDataParameter)
            {
                ((IDbDataParameter)this.par).Precision = precision;
                ((IDbDataParameter)this.par).Scale = scale;
            }

            this.par.SourceColumn = srcColumn;
            this.par.SourceVersion = srcVersion;
            this.par.Value = value.ToObject();
        }

        internal TParameter(DbParameter parameter) : this()
        {
            this.par = parameter;
        }

        public System.Data.DbType DbType
        {
            get
            {
                return this.par.DbType;
            }

            set
            {
                this.par.DbType = value;
            }
        }

        public ParameterDirection Direction
        {
            get
            {
                return this.par.Direction;
            }

            set
            {
                this.par.Direction = value;
            }
        }

        public string ParameterName
        {
            get
            {
                return this.par.ParameterName;
            }

            set
            {
                this.par.ParameterName = value;
            }
        }

        public string SourceColumn
        {
            get
            {
                return this.par.SourceColumn;
            }

            set
            {
                this.par.SourceColumn = value;
            }
        }

        public DataRowVersion SourceVersion
        {
            get
            {
                return this.par.SourceVersion;
            }

            set
            {
                this.par.SourceVersion = value;
            }
        }

        public DbObject Value
        {
            get
            {
                return DbObject.ToDbObject(this.par.Value);
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                GetDateTimeError(value, true);
                this.par.Value = value.ToObject();
            }
        }

        /// <summary>
        ///   Verifica se o valor é DateTime e se a data é superior a 1/1/1753 (limitação do SQL Server).
        ///   Devolve mensagem de erro caso seja ou string.Empty caso contrário.
        /// </summary>
        /// <param name = "throwException">Causa ArgumentException ao invés de devolver mensagem de erro.</param>
        public static string GetDateTimeError(object value, bool throwException)
        {
            const string msg = "O ano não deve ser inferior a 1753.";

            if (value is DateTime && (DateTime)value < minDateTime)
            {
                if (throwException)
                {
                    throw new ArgumentException(msg);
                }
                else
                {
                    return msg;
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///   Transforma um array de valores em array de TParameter's, onde cada parâmetro possui o valor
        ///   informado no array de origem.
        ///   A ordem do array de origem é mantida.
        ///   Todos os TParameter's serão do tipo ParameterDirection.Input.
        /// </summary>
        public static TParameter[] ToArray(params DbObject[] values)
        {
            var parameters = new TParameter[values.Length];

            for (var i = 0; i < values.Length; i++)
            {
// object typedValue;

// try {

// typedValue = NullLib.IsNull(values[i]) ? DBNull.Value : values[i];

// }

// catch(NotImplementedException) {

// throw new NotImplementedException("TParameter.ToArray(): Existe um valor cujo tipo não é suportado (" + values[i].GetType().FullName + ")");

// }
                parameters[i] = new TParameter(string.Format("p_{0:00}", i), values[i]);
            }

            return parameters;
        }

        internal static TParameter ConvertFromDb(DbParameter parameter)
        {
            return new TParameter(parameter);
        }

        internal static DbParameter ConvertToDb(TParameter parameter)
        {
            return parameter.par;
        }
    }

    public class TParameterCollection : IEnumerable
    {
        private readonly DbParameterCollection coll;

        private TParameterCollection(DbParameterCollection parameterCollection)
        {
            this.coll = parameterCollection;
        }

        public int Count
        {
            get
            {
                return this.coll.Count;
            }
        }

        public TParameter this[string parameterName]
        {
            get
            {
                return TParameter.ConvertFromDb(this.coll[parameterName]);
            }

            set
            {
                this.coll[parameterName] = TParameter.ConvertToDb(value);
            }
        }

        public TParameter this[int index]
        {
            get
            {
                return TParameter.ConvertFromDb(this.coll[index]);
            }

            set
            {
                this.coll[index] = TParameter.ConvertToDb(value);
            }
        }

        public TParameter Add(TParameter parameter)
        {
            var dbpar = TParameter.ConvertToDb(parameter);
            this.coll.Add(dbpar);
            return TParameter.ConvertFromDb(dbpar);
        }

        public TParameter Add(string parameterName, DbObject value)
        {
            var par = new TParameter(parameterName, value);
            this.coll.Add(TParameter.ConvertToDb(par));
            return par;
        }

        public void AddValues(params DbObject[] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
                this.Add("p_" + this.coll.Count, values[i]);
            }
        }

        public void Clear()
        {
            this.coll.Clear();
        }

        public int IndexOf(string parameterName)
        {
            for (var i = 0; i < this.coll.Count; i++)
            {
                if (this.coll[i].ParameterName == parameterName)
                {
                    return i;
                }
            }

            return -1;
        }

        internal static TParameterCollection ConvertFromDb(DbParameterCollection parameterCollection)
        {
            return new TParameterCollection(parameterCollection);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new TParameterCollectionEnumerator(this);
        }
    }

    public class TParameterCollectionEnumerator : IEnumerator
    {
        private readonly TParameterCollection collection;

        int pos;

        public TParameterCollectionEnumerator(TParameterCollection collection)
        {
            this.collection = collection;
            this.Reset();
        }

        public object Current
        {
            get
            {
                if (this.pos < 0 || this.pos == this.collection.Count)
                {
                    throw new InvalidOperationException();
                }

                return this.collection[this.pos];
            }
        }

        public bool MoveNext()
        {
            if (this.pos < this.collection.Count)
            {
                this.pos++;
            }

            return this.pos != this.collection.Count;
        }

        public void Reset()
        {
            this.pos = -1;
        }
    }
}