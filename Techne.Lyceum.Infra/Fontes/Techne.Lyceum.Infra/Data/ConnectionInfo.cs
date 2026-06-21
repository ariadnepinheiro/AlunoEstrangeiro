using System.Collections;

namespace Techne.Data
{
    internal class ConnectionInfo
    {
        public ConnectionInfo()
        {
            this.Name = string.Empty;
            this.ConnectionString = string.Empty;
            this.IsDefault = false;
        }

        public string ConnectionString { get; set; }

        public bool IsDefault { get; set; }

        public string Name { get; set; }
    }

    public class ConnectionDictionary : DictionaryBase
    {
        internal ConnectionInfo this[string key]
        {
            get
            {
                return (ConnectionInfo)this.Dictionary[key == null ? string.Empty : key.ToLower()];
            }

            set
            {
                this.Dictionary[key == null ? string.Empty : key.ToLower()] = value;
            }
        }

        internal void Add(string key, ConnectionInfo conn)
        {
            this.Dictionary.Add(key == null ? string.Empty : key.ToLower(), conn);
        }

        internal void Remove(string key)
        {
            this.Dictionary.Remove(key == null ? string.Empty : key.ToLower());
        }
    }
}