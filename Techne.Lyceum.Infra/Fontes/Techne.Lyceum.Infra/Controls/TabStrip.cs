using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Techne.Controls.Design;

namespace Techne.Controls
{
    [
        DefaultProperty("Text"), 
        ToolboxData("<{0}:TabStrip runat=server></{0}:TabStrip>")
    ]
    internal class TabStrip : WebControl
    {
        private readonly TabCollection pvItems;

        private int pvDefaultSelectedTab = -1;

        private int pvSelectedIndex = -1;

        private HtmlTable pvTable;

        public TabStrip()
        {
            this.pvItems = new TabCollection(this);
        }

        [
            DefaultValue(-1), 
        ]
        public int DefaultSelectedTab
        {
            get
            {
                return this.pvDefaultSelectedTab;
            }

            set
            {
                this.pvDefaultSelectedTab = value < 0 || value >= this.Items.Count ? -1 : value;
            }
        }

        [
            Browsable(true), 
            PersistenceMode(PersistenceMode.InnerProperty), 
            DesignerSerializationVisibility(DesignerSerializationVisibility.Content), 
        ]
        public TabCollection Items
        {
            get
            {
                return this.pvItems;
            }
        }

        [
            DefaultValue(-1), 
        ]
        public int SelectedIndex
        {
            get
            {
                return this.pvSelectedIndex;
            }

            set
            {
                this.pvSelectedIndex = value < 0 || value >= this.Items.Count ? -1 : value;
            }
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();

            var selected = this.SelectedIndex == -1 ? this.DefaultSelectedTab : this.SelectedIndex;

            this.pvTable = new HtmlTable();
            this.pvTable.CellPadding = 0;
            this.pvTable.CellSpacing = 0;
            this.pvTable.Border = 0;
            if (!this.Width.IsEmpty)
            {
                this.pvTable.Width = this.Width.ToString();
            }

            if (!this.Height.IsEmpty)
            {
                this.pvTable.Height = this.Height.ToString();
            }

            var row = new HtmlTableRow();
            this.pvTable.Rows.Add(row);

            var script = string.Empty;
            for (var i = 0; i < this.Items.Count; i++)
            {
                var navigateUrl = TUtil.TranslateRelativeUrl(this.Items[i].NavigateUrl, this);

                if (TUtil.IsDesignMode(this) || TechneAuthorization.IsUrlAuthorized(navigateUrl))
                {
                    var cell = new HtmlTableCell("TD");
                    cell.Width = "1px";
                    row.Cells.Add(cell);

                    var img = new Image();
                    img.ID = this.ID + "_" + i;
                    if (i == selected && this.Items[i].SelectedImageUrl.Trim() != string.Empty)
                    {
                        img.ImageUrl = this.Items[i].SelectedImageUrl;
                    }
                    else
                    {
                        img.ImageUrl = this.Items[i].ImageUrl;
                    }

                    img.Attributes.Add("OnClick", "window.location.href = '" + navigateUrl + "'");
                    img.Style.Add("cursor", "pointer");
                    img.ToolTip = this.Items[i].ToolTip;
                    cell.Controls.Add(img);
                    if (this.Items[i].AccessKey.Length > 0 && navigateUrl.Length > 0)
                    {
                        script += "__ki(" + ((byte)this.Items[i].AccessKey.ToCharArray()[0]) + ",'" + navigateUrl + "');\n";
                    }
                }
            }

            var cell2 = new HtmlTableCell("TD");
            cell2.Width = "100%";
            row.Cells.Add(cell2);

            this.Controls.Add(this.pvTable);

            if (script.Length > 0)
            {
                this.Page.ClientScript.RegisterStartupScript(typeof (TabStrip), "__ki", "\n<script language='javascript'>\n" + script + "</script>\n");
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            // Define o tab selecionado de acordo com o diretório da página
            this.SelectedIndex = this.DefaultSelectedTab;

            var folder = TUtil.GetRelativeUrl(this.Page.Request.Url.AbsolutePath).ToLower();

            for (var i = 0; i < this.Items.Count; i++)
            {
                var currfolder = this.Items[i].Folder.ToLower().Trim();
                if (currfolder != string.Empty && folder.IndexOf(currfolder) > -1)
                {
                    this.SelectedIndex = i;
                    break;
                }
            }

            base.OnLoad(e);
        }

        protected override void Render(HtmlTextWriter output)
        {
            this.CreateChildControls();
            base.Render(output);
        }
    }

    internal class TabCollection : CollectionBase
    {
        private readonly Control pvParent;

        internal TabCollection(Control parent)
        {
            this.pvParent = parent;
        }

        public Tab this[int index]
        {
            get
            {
                return (Tab)this.List[index];
            }

            set
            {
                this.List[index] = value;
            }
        }

        public int Add(Tab tab)
        {
            return this.List.Add(tab);
        }

        public void Remove(Tab tab)
        {
            this.List.Remove(tab);
        }

        protected override void OnInsert(int index, object value)
        {
            var tab = value as Tab;
            if (tab != null)
            {
                tab.ParentControl = this.pvParent;
            }
        }

        protected override void OnSet(int index, object oldValue, object newValue)
        {
            if (newValue is Tab)
            {
                ((Tab)newValue).ParentControl = this.pvParent;
            }
        }
    }

    internal class Tab : IControlItem
    {
        private string pvAccessKey = string.Empty;

        private string pvFolder = string.Empty;

        private string pvHoverImageUrl = string.Empty;

        private string pvImageUrl = string.Empty;

        private string pvNavigateUrl = string.Empty;

        private string pvSelectedImageUrl = string.Empty;

        private string pvToolTip = string.Empty;

        [
            DefaultValue(""), 
        ]
        public string AccessKey
        {
            get
            {
                return this.pvAccessKey;
            }

            set
            {
                this.pvAccessKey = value == null ? string.Empty : value;
            }
        }

        [Editor(typeof (UrlEditor), typeof (UITypeEditor)), Browsable(true), PersistenceMode(PersistenceMode.Attribute), DefaultValue(""), Description("Folder associado ao tab. Quando a p\x00e1gina estiver dentro deste folder, o tab ser\x00e1 automaticamente selecionado."), Category("Action")]
        public string Folder
        {
            get
            {
                return this.pvFolder;
            }

            set
            {
                this.pvFolder = value == null ? string.Empty : value;
            }
        }

        [DefaultValue(""), Editor(typeof (TImageUrlEditor), typeof (UITypeEditor))]
        public string HoverImageUrl
        {
            get
            {
                return this.pvHoverImageUrl;
            }

            set
            {
                this.pvHoverImageUrl = value == null ? string.Empty : value;
            }
        }

        [
            DefaultValue(""), 
            Editor(typeof (TImageUrlEditor), typeof (UITypeEditor)), 
        ]
        public string ImageUrl
        {
            get
            {
                return this.pvImageUrl;
            }

            set
            {
                this.pvImageUrl = value == null ? string.Empty : value;
            }
        }

        [Editor(typeof (TUrlEditor), typeof (UITypeEditor)), DefaultValue("")]
        public string NavigateUrl
        {
            get
            {
                return this.pvNavigateUrl;
            }

            set
            {
                this.pvNavigateUrl = value == null ? string.Empty : value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public Control ParentControl { get; set; }

        [Editor(typeof (TImageUrlEditor), typeof (UITypeEditor)), DefaultValue("")]
        public string SelectedImageUrl
        {
            get
            {
                return this.pvSelectedImageUrl;
            }

            set
            {
                this.pvSelectedImageUrl = value == null ? string.Empty : value;
            }
        }

        [
            DefaultValue(""), 
        ]
        public string ToolTip
        {
            get
            {
                return this.pvToolTip;
            }

            set
            {
                this.pvToolTip = value == null ? string.Empty : value;
            }
        }
    }
}

namespace Techne.Controls.Design
{
    internal interface IControlItem
    {
        Control ParentControl { get; }
    }
}