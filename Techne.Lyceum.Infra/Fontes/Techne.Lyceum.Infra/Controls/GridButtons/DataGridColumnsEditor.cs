using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DataGrid = System.Web.UI.WebControls.DataGrid;

namespace Techne.Controls.Design
{
    internal class DataGridColumnsEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, 
                                         IServiceProvider provider, 
                                         object value)
        {
            if (context == null || context.Instance == null || provider == null)
            {
                return value;
            }

            if (!(context.Instance is GridButtons) || ((GridButtons)context.Instance).Grid.Trim() == string.Empty)
            {
                return value;
            }

            var service = (IWindowsFormsEditorService)provider.GetService(typeof (IWindowsFormsEditorService));
            var control = (GridButtons)context.Instance;
            DataGrid dg;
            object o = control.Page.FindControl(control.Grid);
            if (o is System.Web.UI.WebControls.DataGrid)
            {
                dg = (System.Web.UI.WebControls.DataGrid)o;
            }
            else
            {
                return value;
            }

            var form = new DataColumnList();
            form.Grid = dg;
            form.Checked = (string)value;

            if (service.ShowDialog(form) == DialogResult.OK)
            {
                return form.Checked;
            }
            else
            {
                return value;
            }
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
    }
}