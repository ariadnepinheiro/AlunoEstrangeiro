using System;
using System.Collections;
using System.Collections.Specialized;

namespace Techne.Library
{
    public class NameDbObjectCollection : NameObjectCollectionBase
    {
        private readonly bool caseSensitive;

        public NameDbObjectCollection(bool caseSensitiveKeys)
        {
            this.caseSensitive = caseSensitiveKeys;
        }

        /// <summary>
        ///   As chaves serão case sensitive.
        /// </summary>
        public NameDbObjectCollection() : this(true)
        {
        }

        public bool CaseSensitive
        {
            get
            {
                return this.caseSensitive;
            }
        }

        public DbObject[] Values
        {
            get
            {
                return (DbObject[])this.BaseGetAllValues(typeof (DbObject));
            }
        }

        public DbObject this[string key]
        {
            get
            {
                return (DbObject)BaseGet(this.caseSensitive ? key : key.ToUpper());
            }

            set
            {
                BaseSet(key, value);
            }
        }

        public DbObject this[int index]
        {
            get
            {
                return (DbObject)BaseGet(index);
            }

            set
            {
                BaseSet(index, value);
            }
        }

        public override string ToString()
        {
            return this.ToString("; ", ": ");
        }

        public void Add(string key, DbObject value)
        {
            if (this.caseSensitive)
            {
                this.BaseAdd(key, value);
            }
            else
            {
                this.BaseAdd(key.ToUpper(), value);
            }
        }

        public bool Contains(string key)
        {
            return this.IndexOfKey(key) >= 0;
        }

        public void Fill(string[] keys, DbObject[] values)
        {
            if (keys == null)
            {
                throw new ArgumentNullException("keys");
            }

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            if (keys.Length != values.Length)
            {
                throw new ArgumentException("Os arrays informados são de tamanhos diferentes (keys: " + keys.Length + ", values: " + values.Length + ")");
            }

            this.BaseClear();

            if (this.caseSensitive)
            {
                for (var i = 0; i < keys.Length; i++)
                {
                    this.BaseAdd(keys[i], values[i]);
                }
            }
            else
            {
                for (var i = 0; i < keys.Length; i++)
                {
                    this.BaseAdd(keys[i].ToUpper(), values[i]);
                }
            }
        }

        public int IndexOfKey(string key)
        {
            return ((IList)this.BaseGetAllKeys()).IndexOf(this.caseSensitive ? key : key.ToUpper());
        }

        public string ToString(string itemSeparator, string pairSeparator)
        {
            var items = new string[this.Count];

            for (var i = 0; i < this.Count; i++)
            {
                var value = BaseGet(i);
                string strValue;
                if (value == null)
                {
                    strValue = "<null>";
                }
                else
                {
                    strValue = value.ToString();
                }

                items[i] = this.BaseGetKey(i) + pairSeparator + strValue;
            }

            return StrLib.EnumerableToStr(items, itemSeparator);
        }
    }
}