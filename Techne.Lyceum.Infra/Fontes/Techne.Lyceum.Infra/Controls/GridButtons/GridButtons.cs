using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Techne.Controls
{
    [Designer(typeof (Techne.Controls.Design.GridButtonsDesigner))]
    internal class GridButtons : WebControl, INamingContainer
    {
        public const string AccessKeyMultiPage_Def = "P";

        public const string AccessKeySinglePage_Def = "M";

        public const string MultiPageImageFocus_Def = "~/images/GridButtonsMultipleDocFocus.gif";

        public const string MultiPageImage_Def = "~/images/GridButtonsMultipleDoc.gif";

        public const string MultiPageToolTip_Def = "Desabilita paginação";

        public const string SinglePageImageFocus_Def = "~/images/GridButtonsSingleDocFocus.gif";

        public const string SinglePageImage_Def = "~/images/GridButtonsSingleDoc.gif";

        public const string SinglePageToolTip_Def = "Habilita paginação";

        private const string AccessKeyColumns_Def = "C";

        private const string AccessKeyDel_Def = "R";

        private const string AccessKeyNew_Def = "N";

        private const string ColumnsImageFocus_Def = "~/images/GridButtonsColumnsFocus.gif";

        private const string ColumnsImage_Def = "~/images/GridButtonsColumns.gif";

        private const string DelImageFocus_Def = "~/images/GridButtonsDeleteFocus.gif";

        private const string DelImage_Def = "~/images/GridButtonsDelete.gif";

        private const string NewImageFocus_Def = "~/images/GridButtonsNewFocus.gif";

        private const string NewImage_Def = "~/images/GridButtonsNew.gif";

        private readonly Label ColumnList = new Label();

        private readonly ImageLink ColumnsButton = new ImageLink();

        private readonly HyperLink ColumnsCancelButton = new HyperLink();

        private readonly LinkButton ColumnsOKButton = new LinkButton();

        private readonly ImageLink DeleteButton = new ImageLink();

        private readonly ImageLink NewButton = new ImageLink();

        private readonly ImageLink PagingButton = new ImageLink();

        private static string ColumnsToolTip_Def = "Seleciona colunas visíveis";

        private static string DelToolTip_Def = "Remove registros marcados";

        private static string NewToolTip_Def = "Adiciona um novo registro";

        private bool EnableColumnSelection = true;

        /// <summary>
        ///   Inicializado em OnLoad()
        /// </summary>
        private DataGrid grid;

        private Style pvColumnListStyle;

        public GridButtons()
        {
            this.Grid = string.Empty;
            this.pvColumnListStyle = new Style(this.ViewState);
            this.pvColumnListStyle.BorderStyle = BorderStyle.Outset;

            this.PagingButton.ID = "_Paging";
            this.DeleteButton.ID = "_Delete";
            this.NewButton.ID = "_New";
            this.ColumnsButton.ID = "_Columns";
            this.ColumnsOKButton.ID = "_ColumnsOK";
            this.ColumnsCancelButton.ID = "_ColumnsCancel";
            this.ColumnList.ID = "_ColumnList";

            this.ImageUrl_NewButton = NewImage_Def;
            this.ImageUrl_NewButtonFocus = NewImageFocus_Def;
            this.ToolTip_NewButton = NewToolTip_Def;
            this.ImageUrl_DelButton = DelImage_Def;
            this.ImageUrl_DelButtonFocus = DelImageFocus_Def;
            this.ToolTip_DelButton = DelToolTip_Def;
            this.ImageUrl_MultPage = MultiPageImage_Def;
            this.ImageUrl_MultPageFocus = MultiPageImageFocus_Def;
            this.ToolTip_MultPage = MultiPageToolTip_Def;
            this.ImageUrl_SinglePage = SinglePageImage_Def;
            this.ImageUrl_SinglePageFocus = SinglePageImageFocus_Def;
            this.ToolTip_SinglePage = SinglePageToolTip_Def;
            this.ImageUrl_ColumnsButton = ColumnsImage_Def;
            this.ImageUrl_ColumnsButtonFocus = ColumnsImageFocus_Def;
            this.ToolTip_ColumnsButton = ColumnsToolTip_Def;

            this.EnableColumnsButton = false;
            this.EnableNewButton = true;
            this.EnableDeleteButton = false;
            this.EnablePaging = true;
        }

        [
            DefaultValue(""), 
            Description("Estilo da lista de colunas"), 
            PersistenceMode(PersistenceMode.InnerProperty), 
            DesignerSerializationVisibility(DesignerSerializationVisibility.Content)
        ]
        public Style ColumnListStyle
        {
            get
            {
                return this.pvColumnListStyle;
            }

            set
            {
                this.pvColumnListStyle = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(false), 
        ]
        public bool EnableColumnsButton
        {
            get
            {
                return (bool)this.ViewState["EnableColumnsButton"];
            }

            set
            {
                this.ViewState["EnableColumnsButton"] = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(false), 
        ]
        public bool EnableDeleteButton
        {
            get
            {
                return (bool)this.ViewState["EnableDeleteButton"];
            }

            set
            {
                this.ViewState["EnableDeleteButton"] = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(true), 
        ]
        public bool EnableNewButton
        {
            get
            {
                return (bool)this.ViewState["EnableNewButton"];
            }

            set
            {
                this.ViewState["EnableNewButton"] = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(true), 
        ]
        public bool EnablePaging
        {
            get
            {
                return (bool)this.ViewState["EnablePaging"];
            }

            set
            {
                this.ViewState["EnablePaging"] = value;
            }
        }

        [
            DefaultValue(""), 
            Description("DataGrid que responderá aos eventos dos botões do GridButtons"), 
            TypeConverter(typeof (DataGridConverter))
        ]
        public string Grid
        {
            get
            {
                return (string)this.ViewState["Grid"];
            }

            set
            {
                this.ViewState["Grid"] = value == null ? string.Empty : value;
            }
        }

        [
            Category("Image"), 
            DefaultValue(ColumnsImage_Def), 
            Editor(typeof (System.Web.UI.Design.ImageUrlEditor), typeof (System.Drawing.Design.UITypeEditor)), 
        ]
        public string ImageUrl_ColumnsButton
        {
            get
            {
                return (string)this.ViewState["ColumnsImage"];
            }

            set
            {
                this.ViewState["ColumnsImage"] = value == null || value.Trim() == string.Empty ? ColumnsImage_Def : value;
            }
        }

        [
            Category("Image"), 
            DefaultValue(ColumnsImageFocus_Def), 
            Editor(typeof (System.Web.UI.Design.ImageUrlEditor), typeof (System.Drawing.Design.UITypeEditor)), 
        ]
        public string ImageUrl_ColumnsButtonFocus
        {
            get
            {
                return (string)this.ViewState["ColumnsImageFocus"];
            }

            set
            {
                this.ViewState["ColumnsImageFocus"] = value == null || value.Trim() == string.Empty ? ColumnsImageFocus_Def : value;
            }
        }

        [
            Category("Image"), 
            DefaultValue(DelImage_Def), 
            Editor(typeof (System.Web.UI.Design.ImageUrlEditor), typeof (System.Drawing.Design.UITypeEditor)), 
        ]
        public string ImageUrl_DelButton
        {
            get
            {
                return (string)this.ViewState["DelImage"];
            }

            set
            {
                this.ViewState["DelImage"] = value == null || value.Trim() == string.Empty ? DelImage_Def : value;
            }
        }

        [
            Category("Image"), 
            DefaultValue(DelImageFocus_Def), 
            Editor(typeof (System.Web.UI.Design.ImageUrlEditor), typeof (System.Drawing.Design.UITypeEditor)), 
        ]
        public string ImageUrl_DelButtonFocus
        {
            get
            {
                return (string)this.ViewState["DelImageFocus"];
            }

            set
            {
                this.ViewState["DelImageFocus"] = value == null || value.Trim() == string.Empty ? DelImageFocus_Def : value;
            }
        }

        [
            Category("Image"), 
            DefaultValue(MultiPageImage_Def), 
            Editor(typeof (System.Web.UI.Design.ImageUrlEditor), typeof (System.Drawing.Design.UITypeEditor)), 
        ]
        public string ImageUrl_MultPage
        {
            get
            {
                return (string)this.ViewState["MultiPageImage"];
            }

            set
            {
                this.ViewState["MultiPageImage"] = value == null || value.Trim() == string.Empty ? MultiPageImage_Def : value;
            }
        }

        [
            Category("Image"), 
            DefaultValue(MultiPageImageFocus_Def), 
            Editor(typeof (System.Web.UI.Design.ImageUrlEditor), typeof (System.Drawing.Design.UITypeEditor)), 
        ]
        public string ImageUrl_MultPageFocus
        {
            get
            {
                return (string)this.ViewState["MultiPageImageFocus"];
            }

            set
            {
                this.ViewState["MultiPageImageFocus"] = value == null || value.Trim() == string.Empty ? MultiPageImageFocus_Def : value;
            }
        }

        [
            Category("Image"), 
            DefaultValue(NewImage_Def), 
            Editor(typeof (System.Web.UI.Design.ImageUrlEditor), typeof (System.Drawing.Design.UITypeEditor)), 
        ]
        public string ImageUrl_NewButton
        {
            get
            {
                return (string)this.ViewState["NewImage"];
            }

            set
            {
                this.ViewState["NewImage"] = value == null || value.Trim() == string.Empty ? NewImage_Def : value;
            }
        }

        [
            Category("Image"), 
            DefaultValue(NewImageFocus_Def), 
            Editor(typeof (System.Web.UI.Design.ImageUrlEditor), typeof (System.Drawing.Design.UITypeEditor)), 
        ]
        public string ImageUrl_NewButtonFocus
        {
            get
            {
                return (string)this.ViewState["NewImageFocus"];
            }

            set
            {
                this.ViewState["NewImageFocus"] = value == null || value.Trim() == string.Empty ? NewImageFocus_Def : value;
            }
        }

        [
            Category("Image"), 
            DefaultValue(SinglePageImage_Def), 
            Editor(typeof (System.Web.UI.Design.ImageUrlEditor), typeof (System.Drawing.Design.UITypeEditor)), 
        ]
        public string ImageUrl_SinglePage
        {
            get
            {
                return (string)this.ViewState["SinglePageImage"];
            }

            set
            {
                this.ViewState["SinglePageImage"] = value == null || value.Trim() == string.Empty ? SinglePageImage_Def : value;
            }
        }

        [
            Category("Image"), 
            DefaultValue(SinglePageImageFocus_Def), 
            Editor(typeof (System.Web.UI.Design.ImageUrlEditor), typeof (System.Drawing.Design.UITypeEditor)), 
        ]
        public string ImageUrl_SinglePageFocus
        {
            get
            {
                return (string)this.ViewState["SinglePageImageFocus"];
            }

            set
            {
                this.ViewState["SinglePageImageFocus"] = value == null || value.Trim() == string.Empty ? SinglePageImageFocus_Def : value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(""), 
            Editor(typeof (Techne.Controls.Design.DataGridColumnsEditor), typeof (System.Drawing.Design.UITypeEditor))
        ]
        public string SelectableColumns
        {
            get
            {
                return (string)this.ViewState["SelectableColumns"];
            }

            set
            {
                this.ViewState["SelectableColumns"] = value == null ? string.Empty : value.Trim();
                this.InsertColumnList(this.ColumnList);
            }
        }

        [
            Category("Message"), 
            DefaultValue("Seleciona colunas visíveis"), 
        ]
        public string ToolTip_ColumnsButton
        {
            get
            {
                return (string)this.ViewState["ColumnsToolTip"];
            }

            set
            {
                this.ViewState["ColumnsToolTip"] = value == null || value.Trim() == string.Empty ? ColumnsToolTip_Def : value;
            }
        }

        [
            Category("Message"), 
            DefaultValue("Remove registros marcados"), 
        ]
        public string ToolTip_DelButton
        {
            get
            {
                return (string)this.ViewState["DelToolTip"];
            }

            set
            {
                this.ViewState["DelToolTip"] = value == null || value.Trim() == string.Empty ? DelToolTip_Def : value;
            }
        }

        [
            Category("Message"), 
            DefaultValue("Desabilita paginação"), 
        ]
        public string ToolTip_MultPage
        {
            get
            {
                return (string)this.ViewState["MultPageToolTip"];
            }

            set
            {
                this.ViewState["MultPageToolTip"] = value == null || value.Trim() == string.Empty ? MultiPageToolTip_Def : value;
            }
        }

        [
            Category("Message"), 
            DefaultValue("Adiciona um novo registro"), 
        ]
        public string ToolTip_NewButton
        {
            get
            {
                return (string)this.ViewState["NewToolTip"];
            }

            set
            {
                this.ViewState["NewToolTip"] = value == null || value.Trim() == string.Empty ? NewToolTip_Def : value;
            }
        }

        [
            Category("Message"), 
            DefaultValue("Habilita paginação"), 
        ]
        public string ToolTip_SinglePage
        {
            get
            {
                return (string)this.ViewState["SinglePageToolTip"];
            }

            set
            {
                this.ViewState["SinglePageToolTip"] = value == null || value.Trim() == string.Empty ? SinglePageToolTip_Def : value;
            }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            // Estes controles devem sempre ser inseridos porque eles dependem da quantidade de registros
            // existentes na grid, informação que neste momento pode não estar disponível se a grid
            // ainda não fez o get (EnableViewState = false). A propriedade Visible de cada um deles são
            // tratadas no OnPreRender().
            this.Controls.Add(this.NewButton);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.PagingButton);

            if (this.EnableColumnsButton && this.grid != null && this.grid.Items.Count > 0 && this.EnableColumnSelection)
            {
                this.ColumnsButton.ImageUrl = this.ImageUrl_ColumnsButton;
                this.ColumnsButton.ToolTip = this.ToolTip_ColumnsButton;
                this.ColumnsButton.AccessKey = AccessKeyColumns_Def;
                this.ColumnsButton.Attributes.Add("OnClick", "ShowColumnList('" + this.ID + "', true); return false;");
                this.ColumnsButton.FocusImageUrl = this.ImageUrl_ColumnsButtonFocus;
                this.Controls.Add(this.ColumnsButton);

                this.InsertColumnList(this.ColumnList);
                this.Controls.Add(this.ColumnList);
            }

            this.NewButton.Click += this.NewButton_Click;
            this.PagingButton.Click += this.PagingButton_Click;
            this.DeleteButton.Click += this.DeleteButton_Click;
            this.ColumnsOKButton.Click += this.ColumnsOKButton_Click;

            // Registra script
            string script;
            script = "<SCRIPT language=\"javascript\">\n" +
                     "<!--//\n" +
                     "  function ShowColumnList(id,visible)\n" +
                     "  {\n" +
                     "    \n" +
                     "  if(visible==true)\n" +
                     "    document.getElementById(id+\"_Columns\").style.visibility=\"visible\";\n" +
                     "  else\n" +
                     "    document.getElementById(id+\"_Columns\").style.visibility=\"hidden\";\n" +
                     "  return false;\n" +
                     "  }\n" +
                     "  //-->\n" +
                     "</SCRIPT>\n";
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered(typeof (GridButtons), "Techne.Controls.GridButtons"))
            {
                this.Page.ClientScript.RegisterClientScriptBlock(typeof (GridButtons), "Techne.Controls.GridButtons", script);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            var browser = "IE";
            var ver = 5;

            base.OnInit(e);

            if (this.Site == null || this.Site.DesignMode)
            {
                browser = "IE";
                ver = 5;
            }
            else
            {
                if (this.Page.Request.Browser.Browser.ToLower().IndexOf("netscape") > -1)
                {
                    browser = "NS";
                }
                else if (this.Page.Request.Browser.Browser.ToLower().IndexOf("mozilla") > -1)
                {
                    browser = "MZ";
                }
                else if (this.Page.Request.Browser.Browser.ToLower().IndexOf("opera") > -1)
                {
                    browser = "OP";
                }
                else if (this.Page.Request.Browser.Browser.ToLower().IndexOf("ie") > -1 || this.Page.Request.Browser.Browser.ToLower().IndexOf("explorer") > -1)
                {
                    browser = "IE";
                }
                else
                {
                    browser = "??";
                }

                ver = this.Page.Request.Browser.MajorVersion;
            }

            if (browser == "??" || (browser == "IE" && ver < 5) || (browser == "NS" && ver < 5) || (browser == "MZ" && ver < 1) || (browser == "OP" && ver < 6))
            {
                this.EnableColumnSelection = false;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            
            {
                if (this.Grid.Trim().Length == 0)
                {
                    throw new InvalidOperationException("A propriedade " + this.UniqueID + ".Grid não foi informada.");
                }

                var control = this.NamingContainer.FindControl(this.Grid);
                if (control == null)
                {
                    throw new InvalidOperationException("O controle informado na propriedade " + this.ID + ".Grid não foi encontrada na página");
                }

                if (!(control is DataGrid))
                {
                    throw new InvalidOperationException("O controle informado na propriedade " + this.ID + ".Grid não é do tipo DataGrid");
                }

                this.grid = (DataGrid)control;
            }

            
        }

        protected override void OnPreRender(EventArgs args)
        {
            var grid = this.NamingContainer.FindControl(this.Grid) as TDataGrid;

            this.NewButton.ImageUrl = this.ImageUrl_NewButton;
            this.NewButton.FocusImageUrl = this.ImageUrl_NewButtonFocus;
            this.NewButton.AccessKey = AccessKeyNew_Def;
            this.NewButton.ToolTip = this.ToolTip_NewButton;
            this.NewButton.Visible = grid != null && this.EnableNewButton;

            this.DeleteButton.ImageUrl = this.ImageUrl_DelButton;
            this.DeleteButton.FocusImageUrl = this.ImageUrl_DelButtonFocus;
            this.DeleteButton.AccessKey = AccessKeyDel_Def;
            this.DeleteButton.ToolTip = this.ToolTip_DelButton;

// DeleteButton.CommandName = "Remove";
            this.DeleteButton.Visible = grid != null && this.EnableDeleteButton && grid.Items.Count > 0;

            this.PagingButton.ToolTip = grid.AllowPaging ? this.ToolTip_MultPage : this.ToolTip_SinglePage;
            this.PagingButton.AccessKey = grid.AllowPaging ? AccessKeyMultiPage_Def : AccessKeySinglePage_Def;
            this.PagingButton.ImageUrl = grid.AllowPaging ? this.ImageUrl_SinglePage : this.ImageUrl_MultPage;
            this.PagingButton.FocusImageUrl = grid.AllowPaging ? this.ImageUrl_SinglePageFocus : this.ImageUrl_MultPageFocus;
            this.PagingButton.Visible = grid != null && this.EnablePaging &&
                                        (grid.AllowPaging && grid.PageCount > 1 || !grid.AllowPaging && grid.Items.Count >= grid.PageSize) &&
                                        ContainerManagerLib.AllContainersInMode(grid, RecordManagerMode.View);

            base.OnPreRender(args);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.InsertColumnList(this.ColumnList);
            base.Render(writer);
        }

        private void ColumnsOKButton_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < this.grid.Columns.Count; i++)
            {
                if (this.FindControl("Coluna" + i) is CheckBox)
                {
                    this.grid.Columns[i].Visible = ((CheckBox)this.FindControl("Coluna" + i)).Checked;
                }
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (this.grid is TDataGrid)
            {
                ((IContainerManager)this.grid).Delete();
            }
        }

        private void InsertColumnList(Label pnl)
        {
            Table tab;
            CheckBox chk;
            var cols = new ArrayList();
            string[] scols;
            string texto;
            int i, j, linhas, colunas = 3;

            if (!this.EnableColumnSelection)
            {
                return;
            }

            pnl.Controls.Clear();
            pnl.ID = "Columns";

// pnl.BorderStyle=ColumnListStyle.BorderStyle;
            // pnl.BackColor=ColumnListStyle.BackColor;
            // pnl.ForeColor=ColumnListStyle.ForeColor;
            // pnl.BorderColor=ColumnListStyle.BorderColor;
            // pnl.BorderWidth=ColumnListStyle.BorderWidth;
            pnl.CssClass = this.ColumnListStyle.CssClass;
            pnl.Font.CopyFrom(this.ColumnListStyle.Font);
            pnl.Style.Add("visibility", "hidden");
            pnl.Style.Add("position", "absolute");
            if (!this.ColumnListStyle.BackColor.IsEmpty)
            {
                pnl.Style.Add("background-color", this.ColumnListStyle.BackColor.ToString());
            }
            else
            {
                pnl.Style.Add("background-color", "white");
            }

            if (!this.ColumnListStyle.BorderWidth.IsEmpty)
            {
                pnl.Style.Add("border-width", this.ColumnListStyle.BorderWidth.ToString());
            }

            if (this.ColumnListStyle.BorderStyle != BorderStyle.NotSet)
            {
                pnl.Style.Add("border-style", this.ColumnListStyle.BorderStyle.ToString());
            }

            if (!this.ColumnListStyle.BorderColor.IsEmpty)
            {
                pnl.Style.Add("border-color", this.ColumnListStyle.BorderColor.ToString());
            }

            if (!this.ColumnListStyle.ForeColor.IsEmpty)
            {
                pnl.Style.Add("color", this.ColumnListStyle.ForeColor.ToString());
            }

            if (this.grid == null)
            {
                return;
            }

            // Verifica quais colunas aparecerão na lista
            if (this.SelectableColumns == null || this.SelectableColumns.Trim() == string.Empty)
            {
                for (i = 0; i < this.grid.Columns.Count; i++)
                {
                    cols.Add(i);
                }
            }
            else
            {
                scols = this.SelectableColumns.Split(',');
                for (i = 0; i < scols.Length; i++)
                {
                    try
                    {
                        j = int.Parse(scols[i]);
                        if (j < this.grid.Columns.Count)
                        {
                            cols.Add(j);
                        }
                    }
                    catch
                    {
                    }
                }
            }

// Monta tabela de colunas
            linhas = (cols.Count + 2) / colunas;
            tab = new Table();
            tab.Rows.Add(new TableRow());
            tab.Rows[0].Cells.Add(new TableCell());
            tab.Rows[0].Cells[0].ColumnSpan = 2 + 2 * colunas;
            tab.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            tab.Rows[0].Cells[0].Text = "Selecione as colunas a serem mostradas";
            tab.Rows.Add(new TableRow());
            tab.Rows[1].Cells.Add(new TableCell());
            tab.Rows[1].Cells[0].ColumnSpan = 2 + 2 * colunas;
            tab.Rows[1].Cells[0].Text = "&nbsp;";
            for (i = 0; i < linhas; i++)
            {
                tab.Rows.Add(new TableRow());
                for (j = 0; j < 2 * colunas + 2; j++)
                {
                    tab.Rows[i + 2].Cells.Add(new TableCell());
                }

                tab.Rows[i + 2].Cells[0].Width = Unit.Pixel(15);
                tab.Rows[i + 2].Cells[1 + 2 * colunas].Width = Unit.Pixel(15);
                for (j = 0; j < colunas; j++)
                {
                    if (cols.Count < 1 + i + j * linhas)
                    {
                        continue;
                    }

                    texto = this.grid.Columns[(int)cols[i + j * linhas]].HeaderText.Trim();
                    if (texto == string.Empty && this.grid.Columns[(int)cols[i + j * linhas]].FooterText.Trim() != string.Empty)
                    {
                        texto = this.grid.Columns[(int)cols[i + j * linhas]].FooterText.Trim();
                    }
                    else if (texto == string.Empty)
                    {
                        texto = "&lt;Coluna " + ((int)cols[i + j * linhas] + 1) + "&gt;";
                    }

                    tab.Rows[i + 2].Cells[2 * j + 1].Text = texto;
                    tab.Rows[i + 2].Cells[2 * j + 1].ControlStyle.Font.CopyFrom(this.ColumnListStyle.Font);
                    tab.Rows[i + 2].Cells[2 * j + 1].HorizontalAlign = HorizontalAlign.Right;
                    chk = new CheckBox();
                    chk.ID = "Coluna" + ((int)cols[i + j * linhas]);
                    tab.Rows[i + 2].Cells[2 * j + 2].Controls.Add(chk);
                    chk.Checked = this.grid.Columns[(int)cols[i + j * linhas]].Visible;
                }
            }

            i = tab.Rows.Add(new TableRow());
            tab.Rows[i].Cells.Add(new TableCell());
            tab.Rows[i].Cells[0].Text = "&nbsp;";
            tab.Rows[i].Cells[0].ColumnSpan = 2 + 2 * colunas;
            i = tab.Rows.Add(new TableRow());
            tab.Rows[i].Cells.Add(new TableCell());
            tab.Rows[i].Cells[0].ColumnSpan = 2 + 2 * colunas;
            tab.Rows[i].Cells[0].Controls.Add(this.ColumnsOKButton);
            tab.Rows[i].Cells[0].Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            tab.Rows[i].Cells[0].Controls.Add(this.ColumnsCancelButton);
            tab.Rows[i].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            this.ColumnsOKButton.Text = "OK";
            this.ColumnsOKButton.AccessKey = "O";
            this.ColumnsOKButton.ID = "OK";
            this.ColumnsCancelButton.Text = "Cancelar";
            this.ColumnsCancelButton.AccessKey = "C";
            this.ColumnsCancelButton.NavigateUrl = "#";
            this.ColumnsCancelButton.Attributes.Add("OnClick", "return ShowColumnList('" + this.ID + "',false); return false;");

            pnl.Controls.Add(tab);
            pnl.BackColor = System.Drawing.Color.White;
            return;
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            if (this.grid is TDataGrid)
            {
                ((TDataGrid)this.grid).AddNew();
            }
        }

        private void PagingButton_Click(object sender, EventArgs e)
        {
            if (this.grid is TDataGrid)
            {
                ((TDataGrid)this.grid).TogglePaging();
            }
        }
    }
}