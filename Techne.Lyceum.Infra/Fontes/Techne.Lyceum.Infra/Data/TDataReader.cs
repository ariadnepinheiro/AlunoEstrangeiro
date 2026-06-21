using System.Data.Common;

namespace Techne.Data
{
    public class TDataReader
    {
        private readonly DbDataReader dr;

        private TDataReader(DbDataReader reader)
        {
            this.dr = reader;
        }

        public int FieldCount
        {
            get
            {
                return this.dr.FieldCount;
            }
        }

        public bool IsClosed
        {
            get
            {
                return this.dr.IsClosed;
            }
        }

        public DbObject this[string name]
        {
            get
            {
                return DbObject.ToDbObject(this.dr[name]);
            }
        }

        public void Close()
        {
            this.dr.Close();
        }

        public DbType GetFieldType(int index)
        {
            return DbObject.ToDbType(this.dr.GetFieldType(index));
        }

        public string GetName(int index)
        {
            return this.dr.GetName(index);
        }

        public DbObject GetValue(int ordinal)
        {
            return DbObject.ToDbObject(this.dr.GetValue(ordinal));
        }

        public bool Read()
        {
            return this.dr.Read();
        }

        internal static TDataReader ConvertFromDb(DbDataReader reader)
        {
            return new TDataReader(reader);
        }
    }
}