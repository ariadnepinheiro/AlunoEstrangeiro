using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Techne.Controls
{
    internal class ColumnNameConverter : StringConverter
    {
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            IContainerManager manager = null;

            if (context != null)
            {
                if (!(context.Instance is TGridColumn))
                {
                    if (!(context.Instance is Control))
                    {
                        throw new NotSupportedException();
                    }

                    manager = TControl.GetManager((Control)context.Instance);
                }
                else
                {
                    var owner = ((TGridColumn)context.Instance).GetOwner();
                    if (owner is TDataGrid)
                    {
                        manager = (IContainerManager)owner;
                    }
                }
            }

            if (manager == null)
            {
                return new StandardValuesCollection(new string[0]);
            }
            else
            {
                return new StandardValuesCollection(this.GetTableColumns(manager));
            }
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public object[] GetTableColumns(IContainerManager manager)
        {
            var result = new ArrayList();

            if (manager != null)
            {
                var table = manager.GetDesignTimeDataTable();
                if (table != null)
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        result.Add(column.ColumnName);
                    }
                }
            }

            return result.ToArray();
        }
    }
}