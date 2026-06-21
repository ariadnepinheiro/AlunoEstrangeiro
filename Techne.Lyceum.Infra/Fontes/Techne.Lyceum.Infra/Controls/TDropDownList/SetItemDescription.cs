using System;
using Techne.Data;
using Techne.Library;

namespace Techne.Controls
{
    internal delegate void SetItemDescriptionEventHandler(object sender, SetItemDescriptionEventArgs args);

    internal class SetItemDescriptionEventArgs : EventArgs
    {
        private readonly ListItemValues record;

        private readonly DbObject value;

        private string description;

        internal SetItemDescriptionEventArgs(TDataReader dataReader, string description, DbObject value)
        {
            this.record = new ListItemValues(dataReader);
            this.description = description;
            this.value = value;
        }

        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                this.description = value == null ? string.Empty : value;
            }
        }

        public ListItemValues Record
        {
            get
            {
                return this.record;
            }
        }

        public DbObject Value
        {
            get
            {
                return this.value;
            }
        }
    }

    internal class ListItemValues
    {
        readonly NameDbObjectCollection dict;

        internal ListItemValues(TDataReader dataReader)
        {
            this.dict = new NameDbObjectCollection(false);

            for (var i = 0; i < dataReader.FieldCount; i++)
            {
                this.dict.Add(dataReader.GetName(i), dataReader.GetValue(i));
            }
        }

        public DbObject this[string columnName]
        {
            get
            {
                return this.dict[columnName];
            }
        }
    }
}