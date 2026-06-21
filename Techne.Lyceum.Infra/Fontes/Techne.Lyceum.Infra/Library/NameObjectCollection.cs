using System;
using System.Collections;
using System.Collections.Specialized;

namespace Techne.Library
{
    public class NameObjectCollection : NameObjectCollectionBase
    {
        private readonly bool caseSensitive;

        public NameObjectCollection(bool caseSensitiveKeys)
        {
            this.caseSensitive = caseSensitiveKeys;
        }

        /// <summary>
        ///   As chaves serão case sensitive.
        /// </summary>
        public NameObjectCollection() : this(true)
        {
        }

        public bool CaseSensitive
        {
            get
            {
                return this.caseSensitive;
            }
        }

        public object[] Values
        {
            get
            {
                return this.BaseGetAllValues();
            }
        }

        public object this[string key]
        {
            get
            {
                return BaseGet(this.caseSensitive ? key : key.ToUpper());
            }

            set
            {
                BaseSet(key, value);
            }
        }

        public object this[int index]
        {
            get
            {
                return BaseGet(index);
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

        public void Add(string key, object value)
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
                else if (value is DBNull)
                {
                    strValue = "<DBNull>";
                }
                else
                {
                    strValue = value.ToString();
                }

                items[i] = this.BaseGetKey(i) + pairSeparator + strValue;
            }

            return StrLib.EnumerableToStr(items, itemSeparator);
        }

        internal void Fill(string[] keys, object[] values)
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
    }
}