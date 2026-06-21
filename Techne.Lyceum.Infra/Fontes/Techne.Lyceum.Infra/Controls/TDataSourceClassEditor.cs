using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Techne.Controls
{
    internal class TDataSourceClassEditor : UITypeEditor
    {
        IWindowsFormsEditorService wfeService;

        public override bool IsDropDownResizable
        {
            get
            {
                return false;
            }
        }

        public override object EditValue(ITypeDescriptorContext context, 
                                         IServiceProvider provider, 
                                         object value)
        {
            try
            {
                if (context == null || context.Instance == null || provider == null)
                {
                    return value;
                }

                var strValue = value == null ? string.Empty : ((string)value).Trim();
                this.wfeService = (IWindowsFormsEditorService)provider.GetService(typeof (IWindowsFormsEditorService));

                // Monta lista de TDataTables
                var discovery = (ITypeDiscoveryService)provider.GetService(typeof (ITypeDiscoveryService));
                var tableTypes = discovery.GetTypes(typeof (Techne.Data.TDataTable), false);

                var tableList = new List<string>();
                foreach (var t in tableTypes)
                {
                    tableList.Add(((Type)t).FullName);
                }

                if (!string.IsNullOrEmpty(value as string))
                {
                    if (!tableList.Contains((string)value, StringComparer.InvariantCultureIgnoreCase))
                    {
                        tableList.Add((string)value);
                    }
                }

                tableList.Sort(StringComparer.InvariantCultureIgnoreCase);

                // monta listbox
                var list = new ListBox();
                list.Items.AddRange(tableList.ToArray());
                var selIndex = list.Items.IndexOf(value);
                if (selIndex > -1)
                {
                    list.SelectedIndex = selIndex;
                }

                list.BorderStyle = System.Windows.Forms.BorderStyle.None;
                list.SelectedIndexChanged += this.list_SelectedIndexChanged;
                this.wfeService.DropDownControl(list);

                if (list.SelectedIndex < 0)
                {
                    return value;
                }
                else
                {
                    return list.SelectedItem;
                }
            }
            catch (Exception exc)
            {
                Trace.WriteLine("Ocorreu uma exception: " + exc.Message);
                Trace.WriteLine(exc.StackTrace);
                return value;
            }
            finally
            {
                Trace.WriteLine("TDataSourceEditor.EditValue() END");
            }
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.DropDown;
            }
            else
            {
                return base.GetEditStyle(context);
            }
        }

        private void list_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.wfeService != null)
            {
                this.wfeService.CloseDropDown();
            }
        }
    }
}