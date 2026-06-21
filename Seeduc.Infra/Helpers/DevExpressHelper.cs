namespace Seeduc.Infra.Helpers
{
    using System.Web.UI;
    using DevExpress.Web.ASPxGridView;

    public class DevExpressHelper
    {
        public static T GetControl<T>(ASPxGridView gridView, int rowIndex, string columnName, string controleName)
            where T : Control
        {
            var column = gridView.Columns[columnName] as GridViewDataColumn;

            if (column == null)
            {
                return null;
            }

            return gridView.FindRowCellTemplateControl(rowIndex, column, controleName) as T;
        }
    }
}