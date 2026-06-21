using System;
using System.Collections;

namespace Techne.Controls
{
    internal delegate void SetSqlClausesEventHandler(object sender, SetSqlClausesEventArgs args);

    internal class SetSqlClausesEventArgs
    {
        public string Order;

        public string Where;

        private readonly ArrayList pvValues;

        public SetSqlClausesEventArgs(string where, DbObject[] whereValues, string order)
        {
            this.Where = where.Trim().Length == 0 ? string.Empty : "(" + where + ")";
            this.pvValues = new ArrayList(whereValues);
            this.Order = order;
        }

        public IList WhereValues
        {
            get
            {
                return this.pvValues;
            }
        }

        public void Add(string where)
        {
            this.Add(where, null);
        }

        public void Add(string where, params DbObject[] values)
        {
            if (values != null && where.IndexOf('?') < 0)
            {
                throw new ArgumentException("Se <values> for especificado (<values> != null), então <where> deve conter '?'.");
            }
            else if (values == null && where.IndexOf('?') >= 0)
            {
                throw new ArgumentException("Se <values> não for especificado (<values> != null), então <where> não deve conter '?'.");
            }

            this.Where += (this.Where.Trim().Length == 0 ? string.Empty : " AND ") + where;

            if (values != null)
            {
                foreach (var value in values)
                {
                    this.WhereValues.Add(value);
                }
            }
        }
    }
}