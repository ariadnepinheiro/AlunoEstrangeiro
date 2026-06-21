using System;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Data;

namespace Techne
{
    internal class HistoryLib
    {
        public const string TabHist = TConnection.CronosPrefix + "Hist";

        public const string TabHistDet = TConnection.CronosPrefix + "HistCampo";

        public const string TabHistTrans = TConnection.CronosPrefix + "HistTrans";

        private readonly TConnection cn;

        private readonly DataGrid grd;

        private readonly TDataRow row;

        private string[] fields;

        private HistoryLib(TConnection connection, 
                           TDataRow row, 
                           DataGrid dataGrid)
        {
            this.cn = connection;
            this.grd = dataGrid;
            this.row = row;
        }

        public static void FillGrid(DataGrid dataGrid, TPage page)
        {
            var b64dt = page.Request["dt"];
            var b64pk = page.Request["pk"];

            if (b64dt == null || b64pk == null)
            {
                return;
            }

            var dataTableType = TechLib.FindType(StrLib.Base64StringToString(b64dt), page.GetType().Assembly);
            var dt = (TDataTable)dataTableType.GetConstructor(Type.EmptyTypes).Invoke(new object[0]);
            if (dt != null)
            {
                var cn = dt.CreateConnection();
                cn.Open();
                try
                {
                    var row = DataLib2.GetRecord(
                        cn, dt, 
                        DbObject.ToDbObjectArray(StrLib.FromBase64String(b64pk)), 
                        true
                        );
                    var history = new HistoryLib(cn, row, dataGrid);
                    history.Fill();
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                    }
                }
            }
        }

        /// <summary>
        ///   Monta a query string para passagem de parâmetros para a página que apresentará o histórico.
        /// </summary>
        public static string GetHistoryQueryString(TDataRow row)
        {
            return GetHistoryQueryString(row.Table, row.PrimaryKeyValues);
        }

        public static void RegisterHistoryScript(Page page)
        {
            if (page.ClientScript.IsClientScriptBlockRegistered(typeof (HistoryLib), "HistoryScript"))
            {
                return;
            }

            var script =
                "<SCRIPT LANGUAGE='javascript'>" +
                "function openHistory(src) { " +
                "window.open(src, 'wndHistory', 'menubar=no, location=no, resizable=yes'); " +
                "}" +
                "</SCRIPT>";

            page.ClientScript.RegisterClientScriptBlock(typeof (HistoryLib), "HistoryScript", script);
        }

        private static string GetHistoryQueryString(TDataTable dataTable, DbObject[] pkValues)
        {
            if (dataTable == null || pkValues == null)
            {
                return string.Empty;
            }

            return "dt=" + StrLib.StringToBase64String(dataTable.GetType().FullName) + "&" +
                   "pk=" + StrLib.ToBase64String(DbObject.ToObjectArray(pkValues));
        }

        private void CreateColumns()
        {
            this.grd.AutoGenerateColumns = false;
            this.grd.Columns.Clear();

            BoundColumn column;

            column = new BoundColumn();
            column.DataField = "Stamp";
            column.HeaderText = "Data";
            column.DataFormatString = "{0:G}";
            column.ItemStyle.Width = Unit.Pixel(120);
            this.grd.Columns.Add(column);

            column = new BoundColumn();
            column.DataField = "Usuario";
            column.HeaderText = "Usuário";
            column.ItemStyle.Width = Unit.Pixel(120);
            this.grd.Columns.Add(column);

            column = new BoundColumn();
            column.DataField = "Status";
            column.HeaderText = "Operação";
            column.ItemStyle.Width = Unit.Pixel(100);
            this.grd.Columns.Add(column);

            var table = this.row.Table;

            foreach (var field in this.fields)
            {
                var templateCol = new TemplateColumn();

                var caption = string.Empty;
                if (table != null)
                {
                    caption = ((TDataColumn)table.Columns[field]).GetName();
                }

                if (caption.Length == 0)
                {
                    caption = field;
                }

                templateCol.HeaderText = StrLib.ToProper(caption);
                this.grd.Columns.Add(templateCol);
            }
        }

        private void Fill()
        {
            this.grd.ItemDataBound += this.ItemDataBound;

            this.fields = this.row.GetHistoryFields(this.cn);
            this.CreateColumns();

            var tab = this.Query();
            this.grd.DataSource = tab;
            this.grd.DataBind();
        }

        private void ItemDataBound(object sender, DataGridItemEventArgs args)
        {
            var dataRowView = args.Item.DataItem as DataRowView;
            var rowHist = dataRowView == null ? null : dataRowView.Row;
            if (rowHist == null)
            {
                return;
            }

            if ((string)rowHist["Status"] == "ATUAL")
            {
                for (var i = 0; i < this.fields.Length; i++)
                {
                    var field = this.fields[i];
                    var text = Convert.ToString(this.row[field], CultureInfo.InvariantCulture);
                    args.Item.Cells[i + 3].Text = text;
                }
            }
            else
            {
                var histId = rowHist["Id"];
                if (histId is DBNull)
                {
                    return;
                }

                for (var i = 0; i < this.fields.Length; i++)
                {
                    var field = this.fields[i];

                    var valAnt = string.Empty;
                    var descAnt = string.Empty;
                    var valPos = string.Empty;
                    var descPos = string.Empty;
                    var text = string.Empty;

                    if (TDataRow.GetHistoryValues(this.cn, (decimal)histId, field, out valAnt, out descAnt, out valPos, out descPos))
                    {
                        var ant = valAnt + (descAnt.Length == 0 ? string.Empty : " - " + descAnt);
                        var pos = valPos + (descPos.Length == 0 ? string.Empty : " - " + descPos);
                        text = "<TABLE>" +
                               "<TR><TD><B>Para:</B></TD><TD>" + HttpUtility.HtmlEncode(pos) + "</TD></TR>" +
                               "<TR><TD><B>De:</B></TD><TD>" + HttpUtility.HtmlEncode(ant) + "</TD></TR>" +
                               "</TABLE>";
                    }

                    args.Item.Cells[i + 3].Text = text;
                }
            }
        }

        private DataTable Query()
        {
            var tab = this.row.GetHistory(this.cn);

            // Adiciona no histórico a situação corrente
            var objStatus = this.row["Hist Status"];
            if (objStatus.IsNull || objStatus != "R")
            {
                var currentRow = tab.NewRow();
                currentRow["Stamp"] = DateTime.Now;
                currentRow["Status"] = "ATUAL";
                tab.Rows.Add(currentRow);
            }

            tab.DefaultView.Sort = "Stamp DESC";

            return tab;
        }
    }
}