using System;
using System.Collections;
using System.Data;
using Techne.Data;

namespace Techne.Web.Data
{
    internal class TSearchQueryParameter
    {
        private object _value;

        public TSearchQueryParameter()
        {
        }

        public TSearchQueryParameter(string name, Type type, string caption)
        {
            this.Name = name;
            this.Type = type;
            this.Caption = caption;
            this.MinSize = 0;
            this.MaxSize = 0;
            this.MinWordSize = 0;
        }

        public TSearchQueryParameter(string name, Type type, string caption, int maxSize)
        {
            this.Name = name;
            this.Type = type;
            this.Caption = caption;
            this.MaxSize = maxSize;
            this.MinSize = 0;
            this.MinWordSize = 0;
        }

        internal TSearchQueryParameter(string name, Type type, string caption, int maxSize, int minSize, int minWordSize)
        {
            this.Name = name;
            this.Type = type;
            this.Caption = caption;
            this.MaxSize = maxSize;
            this.MinSize = minSize;
            this.MinWordSize = minWordSize;
        }

        public string Caption { get; set; }

        public int MaxSize { get; set; }

        public int MinSize { get; set; }

        public string Name { get; set; }

        public Type Type { get; set; }

        public object Value
        {
            get
            {
                return this._value;
            }

            set
            {
                if (value == null || value.GetType() == this.Type)
                {
                    this._value = value;
                }
                else
                {
                    if (this.Type == typeof (decimal?) || this.Type == typeof (decimal))
                    {
                        this._value = Convert.ToDecimal(value);
                    }
                    else if (this.Type == typeof (string))
                    {
                        this._value = value.ToString();
                    }
                    else
                    {
                        this._value = value;
                    }
                }
            }
        }

        internal int MinWordSize { get; set; }
    }

    internal class TSearchQueryParameterCollection : CollectionBase
    {
        public TSearchQueryParameter this[int index]
        {
            get
            {
                return this.InnerList[index] as TSearchQueryParameter;
            }

            set
            {
                this.InnerList[index] = value;
            }
        }

        public TSearchQueryParameter this[string name]
        {
            get
            {
                for (var i = 0; i < this.InnerList.Count; i++)
                {
                    if (this[i].Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return this[i];
                    }
                }

                return null;
            }
        }

        public void Add(TSearchQueryParameter value)
        {
            base.InnerList.Add(value);
        }

        public void Add(string name, Type type, string caption)
        {
            base.InnerList.Add(new TSearchQueryParameter(name, type, caption));
        }

        public void Add(string name, Type type, string caption, int maxSize)
        {
            base.InnerList.Add(new TSearchQueryParameter(name, type, caption, maxSize));
        }

        public void Remove(TSearchQueryParameter value)
        {
            base.InnerList.Remove(value);
        }

        internal void Add(string name, Type type, string caption, int maxSize, int minSize, int minWordSize)
        {
            base.InnerList.Add(new TSearchQueryParameter(name, type, caption, maxSize, minSize, minWordSize));
        }
    }

    internal class TSearchQueryOrder
    {
        public TSearchQueryOrder()
            : this(string.Empty, true)
        {
        }

        public TSearchQueryOrder(string field, bool ascending)
        {
            this.Field = field;
            this.Ascending = ascending;
        }

        public bool Ascending { get; set; }

        public string Field { get; set; }
    }

    internal class TSearchQueryOrderCollection : CollectionBase
    {
        public TSearchQueryOrder this[int index]
        {
            get
            {
                return this.InnerList[index] as TSearchQueryOrder;
            }

            set
            {
                this.InnerList[index] = value;
            }
        }

        public void Add(TSearchQueryOrder value)
        {
            base.InnerList.Add(value);
        }

        public void Add(string field, bool ascending)
        {
            base.InnerList.Add(new TSearchQueryOrder(field, ascending));
        }

        public void Remove(TSearchQueryOrder value)
        {
            base.InnerList.Remove(value);
        }
    }

    internal abstract class TSearchQuery
    {
        private readonly TSearchQueryOrderCollection _order = new TSearchQueryOrderCollection();

        private readonly TSearchQueryParameterCollection _parameters = new TSearchQueryParameterCollection();

        private bool _isPrepared;

        public TSearchQuery()
        {
            this.PrepareQuery();
        }

        public TConnection Connection { get; set; }

        public TSearchQueryOrderCollection Order
        {
            get
            {
                return this._order;
            }
        }

        public TSearchQueryParameterCollection Parameters
        {
            get
            {
                return this._parameters;
            }
        }

        public string SortExpression
        {
            // get;
            set
            {
                this._order.Clear();
                if (value != null && value.Trim().Length > 0)
                {
                    foreach (var sort in  value.Split(','))
                    {
                        var word = sort.Split(' ');
                        if (word.Length > 1 && (word[1].ToLower() == "asc" || word[1].ToLower() == "desc"))
                        {
                            if (this.Parameters[word[0]] != null)
                            {
                                var ord = new TSearchQueryOrder();
                                ord.Field = word[0];
                                ord.Ascending = word[1].ToLower() == "asc";
                                this._order.Add(ord);
                            }
                        }
                    }
                }
            }
        }

        public DataTable Query(int startRow, int maxRows)
        {
            this.PrepareQuery();
            return this.DoQuery(startRow, maxRows);
        }

        public int QueryCount()
        {
            this.PrepareQuery();
            return this.DoQueryCount();
        }

        protected abstract void DoPrepareQuery();

        protected abstract DataTable DoQuery(int startRow, int maxRows);

        protected abstract int DoQueryCount();

        private void PrepareQuery()
        {
            if (!this._isPrepared)
            {
                this.DoPrepareQuery();
                this._isPrepared = true;
            }
        }
    }
}