namespace Seeduc.Infra.Data
{
    using System.Collections.Generic;
    using System.Data;

    public class ContextQueryParameters : List<ContextQueryParameter>
    {
        public void Add(string name, object value)
        {
            this.Add(new ContextQueryParameter(name, value));
        }

        public void Add(string name, SqlDbType sqlDbType, object value)
        {
            this.Add(new ContextQueryParameter(name, sqlDbType, value));
        }

        public void Add(string name, TechneDbType techneDbType, object value)
        {
            this.Add(new ContextQueryParameter(name, techneDbType, value));
        }

        public void Add(string name, SqlDbType sqlDbType, int size, object value)
        {
            this.Add(new ContextQueryParameter(name, sqlDbType, size, value));
        }

        public void Add(string name, SqlDbType sqlDbType, byte precision, byte scale, object value)
        {
            this.Add(new ContextQueryParameter(name, sqlDbType, precision, scale, value));
        }
    }
}