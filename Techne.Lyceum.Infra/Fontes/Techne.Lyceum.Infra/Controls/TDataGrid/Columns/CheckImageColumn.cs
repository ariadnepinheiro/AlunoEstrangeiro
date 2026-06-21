using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace Techne.Controls
{
    internal class CheckImageColumn : DataGridColumn
    {
        private string pvCheckedImageUrl = "~/images/TDataGridChecked.gif";

        private string pvCheckedValue = "S";

        private string pvUncheckedImageUrl = "~/images/TDataGridUnchecked.gif";

        private string pvUncheckedValue = "N";

        [Category("Images"), DefaultValue("~/images/TDataGridChecked.gif"), Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]
        public string CheckedImageUrl
        {
            get
            {
                return this.pvCheckedImageUrl;
            }

            set
            {
                this.pvCheckedImageUrl = value == null ? string.Empty : value.Trim();
            }
        }

        [DefaultValue("S")]
        public string CheckedValue
        {
            get
            {
                return this.pvCheckedValue;
            }

            set
            {
                this.pvCheckedValue = value == null ? string.Empty : value;
            }
        }

        public string DataField { get; set; }

        [Editor(typeof(ImageUrlEditor), typeof(UITypeEditor)), Category("Images"), DefaultValue("~/images/TDataGridUnchecked.gif")]
        public string UncheckedImageUrl
        {
            get
            {
                return this.pvUncheckedImageUrl;
            }

            set
            {
                this.pvUncheckedImageUrl = value == null ? string.Empty : value.Trim();
            }
        }

        [DefaultValue("N")]
        public string UncheckedValue
        {
            get
            {
                return this.pvUncheckedValue;
            }

            set
            {
                this.pvUncheckedValue = value == null ? string.Empty : value;
            }
        }

        public override void InitializeCell(TableCell cell, int columnIndex, ListItemType itemType)
        {
            base.InitializeCell(cell, columnIndex, itemType);
            Control ctrl = null;
            switch (itemType)
            {
                case ListItemType.AlternatingItem:
                    goto case ListItemType.Item;
                case ListItemType.SelectedItem:
                    goto case ListItemType.Item;
                case ListItemType.EditItem:
                    goto case ListItemType.Item;
                case ListItemType.Item:
                    if (this.DataField != null && this.DataField.Trim() != string.Empty)
                    {
                        ctrl = new Image();
                    }

                    break;
            }

            if (ctrl != null)
            {
                cell.Controls.Add(ctrl);
                ctrl.DataBinding += this.OnDataBinding;
            }
        }

        private void OnDataBinding(object sender, EventArgs e)
        {
            Control ctrl;
            DataGridItem gridItem;

            ctrl = (Control)sender;
            gridItem = (DataGridItem)ctrl.NamingContainer;

            object val;
            try
            {
                val = ((System.Data.DataRowView)gridItem.DataItem)[this.DataField];
            }
            catch (Exception)
            {
                if (!TControl.InDesignMode(ctrl))
                {
                    throw;
                }

                val = null;
            }

            var checkOn = (val as string) != null && ((string)val).ToUpper().Trim() == this.CheckedValue.ToUpper().Trim() ||
                          val is bool && (bool)val;
            var checkOff = (val as string) != null && ((string)val).ToUpper().Trim() == this.UncheckedValue.ToUpper().Trim() ||
                           val is bool && (bool)val;
            if (ctrl is Image)
            {
                if (checkOn)
                {
                    ((Image)ctrl).ImageUrl = this.CheckedImageUrl;
                }
                else if (checkOff)
                {
                    ((Image)ctrl).ImageUrl = this.UncheckedImageUrl;
                }
                else
                {
                    ctrl.Visible = false;
                }
            }
        }
    }
}