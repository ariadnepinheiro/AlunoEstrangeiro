using System.ComponentModel;
using System.Data.Common;
using System.Threading;

namespace Techne.Data
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class TFactory
    {
        private static readonly object _sync = new object();

        private static DbProviderFactory _current;

        public static DbProviderFactory Instance
        {
            get
            {
                if (_current == null)
                {
                    Monitor.Enter(_sync);
                    try
                    {
                        _current = System.Data.OleDb.OleDbFactory.Instance;

// _current = Techne.Data.TDB.TDBFactory.Instance;
                    }
                    finally
                    {
                        Monitor.Exit(_sync);
                    }
                }

                return _current;
            }

            set
            {
                Monitor.Enter(_sync);
                try
                {
                    _current = value;
                }
                finally
                {
                    Monitor.Exit(_sync);
                }
            }
        }
    }
}