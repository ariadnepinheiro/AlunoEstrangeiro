using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace Techne.Controls
{
    internal class MarkColumn : DataGridColumn
    {
        public const string ErrorImageUrl_Def = "~/images/TDataGridError.gif";

        public const string HistoryImageUrl_Def = "~/images/History.gif";

        public const bool ShowCheckBox_Def = true;

        public const bool ShowHistoryIcon_Def = true;

        public const bool ShowMessageIcons_Def = true;

        public const string WarningImageUrl_Def = "~/images/AlertaMens.gif";

        public MarkColumn()
        {
            this.ErrorImageUrl = ErrorImageUrl_Def;
            this.HistoryImageUrl = HistoryImageUrl_Def;
            this.ShowCheckBox = ShowCheckBox_Def;
            this.ShowHistoryIcon = ShowHistoryIcon_Def;
            this.ShowMessageIcons = ShowMessageIcons_Def;
            this.WarningImageUrl = WarningImageUrl_Def;
        }

        [Editor(typeof (ImageUrlEditor), typeof (UITypeEditor)), DefaultValue("~/images/TDataGridError.gif"), Category("Images")]
        public string ErrorImageUrl
        {
            get
            {
                return (string)this.ViewState["ErrorImageUrl"];
            }

            set
            {
                this.ViewState["ErrorImageUrl"] = value == null ? ErrorImageUrl_Def : value.Trim();
            }
        }

        [Category("Images"), Editor(typeof (ImageUrlEditor), typeof (UITypeEditor)), DefaultValue("~/images/History.gif")]
        public string HistoryImageUrl
        {
            get
            {
                return (string)this.ViewState["HistoryImageUrl"];
            }

            set
            {
                this.ViewState["HistoryImageUrl"] = value == null ? HistoryImageUrl_Def : value.Trim();
            }
        }

        [DefaultValue(true), Category("Techne")]
        public bool ShowCheckBox
        {
            get
            {
                return (bool)this.ViewState["ShowCheckBox"];
            }

            set
            {
                this.ViewState["ShowCheckBox"] = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(ShowHistoryIcon_Def), 
        ]
        public bool ShowHistoryIcon
        {
            get
            {
                return (bool)this.ViewState["ShowHistoryIcon"];
            }

            set
            {
                this.ViewState["ShowHistoryIcon"] = value;
            }
        }

        [Category("Techne"), DefaultValue(true)]
        public bool ShowMessageIcons
        {
            get
            {
                return (bool)this.ViewState["ShowMessageIcons"];
            }

            set
            {
                this.ViewState["ShowMessageIcons"] = value;
            }
        }

        [Editor(typeof (ImageUrlEditor), typeof (UITypeEditor)), DefaultValue("~/images/AlertaMens.gif"), Category("Images")]
        public string WarningImageUrl
        {
            get
            {
                return (string)this.ViewState["WarningImageUrl"];
            }

            set
            {
                this.ViewState["WarningImageUrl"] = value == null ? WarningImageUrl_Def : value.Trim();
            }
        }

        public static void InitializeCell(TableCell cell, int columnIndex, ListItemType itemType, 
                                          string historyImageUrl, string errorImageUrl, string warningImageUrl, 
                                          bool showCheckBox, bool showHistoryIcon, bool showMessageIcons)
        {
            if (TGridItem.ItemTypeIsData(itemType))
            {
                cell.Controls.Clear();

                var markCell = cell as MarkCell;
                if (markCell != null)
                {
                    markCell.ID = "mark" + columnIndex;

                    if (showCheckBox)
                    {
                        markCell.AddMarkControl();
                    }

                    if (showHistoryIcon)
                    {
                        markCell.AddHistoryControl(historyImageUrl);
                    }

                    if (showMessageIcons)
                    {
                        markCell.AddErrorControl(errorImageUrl);
                        markCell.AddWarningControl(warningImageUrl);
                    }
                }
            }
        }

        public static void SetHistoryUrl(DataGrid grid, DataGridItem gridItem, string url)
        {
            url = url == null ? string.Empty : url.Trim();

            var columns = grid.Columns;
            var cells = gridItem.Cells;
            for (var i = 0; i < columns.Count; i++)
            {
                if (columns[i] is MarkColumn && ((MarkColumn)columns[i]).ShowHistoryIcon)
                {
                    ((MarkCell)cells[i]).HistoryUrl = url;
                }
            }
        }

        /// <summary>
        ///   Seta mensagem na coluna MarkColumn do TGridItem informado.
        ///   Se a grid não contiver MarkColumn's, devolve false;
        /// </summary>
        public static bool SetMessage(TGridItem gridItem, string message, bool isError)
        {
            if (message == null)
            {
                throw new ArgumentNullException();
            }

            var success = false;
            foreach (TableCell cell in gridItem.Cells)
            {
                if (cell is MarkCell)
                {
                    var img = (isError ? ((MarkCell)cell).ErrorControl : ((MarkCell)cell).WarningControl) as HyperLink;
                    if (img == null)
                    {
                        continue;
                    }

                    img.Attributes.Add("tooltip", message);
                    img.Attributes.Add("OnFocus", "TC_tooltip('" + img.ClientID + "',true)");
                    img.Attributes.Add("OnBlur", "TC_tooltip('" + img.ClientID + "',false)");
                    img.Attributes.Add("OnMouseOver", "TC_tooltip('" + img.ClientID + "',true)");
                    img.Attributes.Add("OnMouseOut", "TC_tooltip('" + img.ClientID + "',false)");
                    img.Visible = message.Length > 0;

                    success = true;
                }
            }

            return success;
        }

        public override void InitializeCell(TableCell cell, int columnIndex, ListItemType itemType)
        {
            InitializeCell(cell, columnIndex, itemType, 
                           this.HistoryImageUrl, this.ErrorImageUrl, this.WarningImageUrl, 
                           this.ShowCheckBox, this.ShowHistoryIcon, this.ShowMessageIcons);
        }

        /// <summary>
        ///   Determina o TableCell que contém direta ou indiretamente o TControl informado.
        /// </summary>
        private static TableCell FindCellContainer(Control control)
        {
            Control container;
            for (container = control.Parent;
                 container != null && !(container is TableCell);
                 container = container.Parent)
            {
                ;
            }

            return (TableCell)container;
        }
    }

    internal class MarkCell : TTableCell, INamingContainer
    {
        public bool EnableMark
        {
            get
            {
                var chk = this.MarkControl;
                return chk != null && chk.Enabled;
            }

            set
            {
                var chk = this.MarkControl;
                if (chk != null)
                {
                    chk.Enabled = value;
                }
            }
        }

        public WebControl ErrorControl
        {
            get
            {
                return this.FindControl("Error") as WebControl;
            }
        }

        public string HistoryUrl
        {
            get
            {
                var img = this.HistoryControl;
                if (img == null)
                {
                    return string.Empty;
                }

                return img.ImageUrl;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                var img = this.HistoryControl;
                if (img == null)
                {
                    img = this.AddHistoryControl(string.Empty);
                }

                img.Visible = value.Trim().Length > 0;
                if (img.Visible)
                {
                    img.Attributes.Add("onclick", "openHistory('" + value.Trim() + "');");
                    img.Attributes.Add("onmouseover", "style.cursor = 'hand';");
                    img.Attributes.Add("onmouseout", "style.cursor = 'auto';");
                }
            }
        }

        public bool Marked
        {
            get
            {
                var chk = this.MarkControl;
                return chk != null && chk.Checked;
            }

            set
            {
                var chk = this.MarkControl;
                if (chk != null)
                {
                    chk.Checked = value;
                }
            }
        }

        public WebControl WarningControl
        {
            get
            {
                return this.FindControl("Warn") as WebControl;
            }
        }

        private Image HistoryControl
        {
            get
            {
                return this.FindControl("Hist") as Image;
            }
        }

        private CheckBox MarkControl
        {
            get
            {
                return this.FindControl("chk") as CheckBox;
            }
        }

        public void AddErrorControl(string imageUrl)
        {
            if (this.ErrorControl != null)
            {
                return;
            }

            var img = new HyperLink();
            img.ID = "Error";
            img.ImageUrl = imageUrl;
            img.NavigateUrl = "#";
            img.Visible = false;
            img.EnableViewState = false;

            this.Controls.Add(img);
        }

        public Image AddHistoryControl(string imageUrl)
        {
            var img = this.HistoryControl;

            if (img == null)
            {
                img = new Image();
                img.ID = "Hist";
                img.ToolTip = "Histórico de alterações";
                img.Visible = false;

                this.Controls.Add(img);
            }

            img.ImageUrl = imageUrl;
            return img;
        }

        public void AddMarkControl()
        {
            if (this.MarkControl != null)
            {
                return;
            }

            var chk = new CheckBox();
            chk.ID = "chk";

            this.Controls.Add(chk);
        }

        public void AddWarningControl(string imageUrl)
        {
            if (this.WarningControl != null)
            {
                return;
            }

            var img = new HyperLink();
            img.ID = "Warn";
            img.ImageUrl = imageUrl;
            img.Visible = false;
            img.EnableViewState = false;

            this.Controls.Add(img);
        }

        public int GetWidth()
        {
            var width = 0;

            if (this.EnableMark)
            {
                width += 25;
            }

            var ctlMessage = this.ErrorControl;
            if (ctlMessage == null)
            {
                ctlMessage = this.WarningControl;
            }

            if (ctlMessage != null && ctlMessage.Visible)
            {
                width += 15;
            }

            var imgHistory = this.HistoryControl;
            if (imgHistory != null && imgHistory.Visible)
            {
                var container = TControl.GetRecordContainer(this);
                if (container.Mode == RecordManagerMode.View)
                {
                    width += 15;
                }
            }

            return width;
        }
    }
}