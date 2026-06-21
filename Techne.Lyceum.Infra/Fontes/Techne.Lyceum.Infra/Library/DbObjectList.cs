using System;
using System.Collections;

namespace Techne.Library
{
    internal class DbObjectList : CollectionBase
    {
        public int Add(DbObject o)
        {
            return this.List.Add(o);
        }

        public void AddRange(ICollection c)
        {
            foreach (DbObject o in c)
            {
                this.Add(o);
            }
        }

        public DbObject[] ToArray()
        {
            return (DbObject[])this.InnerList.ToArray(typeof (DbObject));
        }

        protected override void OnInsert(int index, object value)
        {
            if (!(value is IDbObject) && !DbObject.CanConvertToDbObject(value))
            {
                throw new ArgumentException("O tipo " + value.GetType().FullName + " não pode ser convertido para DbObject.");
            }

            base.OnInsert(index, value);
        }

        protected override void OnSet(int index, object oldValue, object newValue)
        {
            if (!(newValue is IDbObject) && !DbObject.CanConvertToDbObject(newValue))
            {
                throw new ArgumentException("O tipo " + newValue.GetType().FullName + " não pode ser convertido para DbObject.");
            }

            base.OnSet(index, oldValue, newValue);
        }
    }
}