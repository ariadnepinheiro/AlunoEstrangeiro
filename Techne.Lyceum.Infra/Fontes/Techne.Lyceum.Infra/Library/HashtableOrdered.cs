using System;
using System.Collections;
using System.Collections.Specialized;

namespace Techne.Library
{
    internal class HashtableOrdered : IEnumerable
    {
        private readonly Hashtable pvHashTable;

        private readonly StringCollection pvStringCol;

        public HashtableOrdered()
        {
            this.pvHashTable = new Hashtable();
            this.pvStringCol = new StringCollection();
        }

        public int Count
        {
            get
            {
                return this.pvStringCol.Count;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return this.pvHashTable.IsSynchronized;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this.pvHashTable.SyncRoot;
            }
        }

        /// <summary>
        ///   VERIFICAR: Esta propriedade não retorna os elementos na ordem correta!!!
        /// </summary>
        public ICollection Values
        {
            get
            {
                return this.pvHashTable.Values;
            }
        }

        public object this[string key]
        {
            get
            {
                return this.pvHashTable[key];
            }
        }

        public object this[int position]
        {
            get
            {
                if (position >= this.pvStringCol.Count)
                {
                    throw new IndexOutOfRangeException("Tentativa de acesso a um elemento inexistente (posição " + position + " de " + this.pvStringCol.Count + " elementos).");
                }

                return this[this.pvStringCol[position]];
            }
        }

        public void Add(string key, object value)
        {
            this.pvHashTable.Add(key, value);
            this.pvStringCol.Add(key);
        }

        public bool Contains(string key)
        {
            return this.pvStringCol.Contains(key);
        }

        // ICollection
        public void CopyTo(Array array, int index)
        {
            this.pvHashTable.CopyTo(array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return new HashTableOrderedEnumerator(this);
        }

        public int IndexOf(string key)
        {
            return this.pvStringCol.IndexOf(key);
        }
    }

    internal class HashTableOrderedEnumerator : IEnumerator
    {
        private readonly HashtableOrdered pvSnapshot;

        int position;

        internal HashTableOrderedEnumerator(HashtableOrdered snapshot)
        {
            this.pvSnapshot = snapshot;
            this.Reset();
        }

        public object Current
        {
            get
            {
                if (this.position < 0 || this.position == this.pvSnapshot.Count)
                {
                    throw new InvalidOperationException();
                }

                return this.pvSnapshot[this.position];
            }
        }

        public bool MoveNext()
        {
            if (this.position < this.pvSnapshot.Count)
            {
                this.position++;
            }

            return this.position != this.pvSnapshot.Count;
        }

        public void Reset()
        {
            this.position = -1;
        }
    }
}