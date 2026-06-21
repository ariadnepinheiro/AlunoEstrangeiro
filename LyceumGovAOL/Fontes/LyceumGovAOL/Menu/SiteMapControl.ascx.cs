namespace Techne.Lyceum.Net.Menu
{
    using System;
    using System.ComponentModel;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Techne.Web;

    public partial class SiteMapControl : UserControl
    {
        private int columns = 3;

        private string siteMapProvider = string.Empty;

        private string startNode = string.Empty;

        [DefaultValue(3)]
        public int Columns
        {
            get
            {
                return this.columns;
            }

            set
            {
                this.columns = value < 1 ? 1 : value;
            }
        }

        public string SiteMapProvider
        {
            get
            {
                return this.siteMapProvider;
            }

            set
            {
                this.siteMapProvider = value ?? string.Empty;
            }
        }

        public string StartNode
        {
            get
            {
                return this.startNode;
            }

            set
            {
                this.startNode = value ?? string.Empty;
            }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            SiteMapProvider provider;

            if (this.siteMapProvider.Trim().Length == 0)
            {
                provider = SiteMap.Provider;
            }
            else
            {
                provider = SiteMap.Providers[this.SiteMapProvider];
            }

            SiteMapNode topNode = null;

            if (provider != null)
            {
                topNode = provider.FindSiteMapNode(this.Context);
            }

            // conta links para distribuí-los mais ou menos igualmente entre as colunas
            var linkCount = 0;

            if (topNode != null)
            {
                foreach (SiteMapNode titleNode in topNode.ChildNodes)
                {
                    linkCount += titleNode.ChildNodes.Count + 7;
                }
            }

            var avgLinkPerColumn = linkCount / this.Columns;

            // monta a tabela de links
            linkCount = 0;

            var tab = new Table
                      {
                          BorderStyle = BorderStyle.None,
                          CellPadding = 0,
                          CellSpacing = 0,
                          CssClass = "mx-5"
                      };

            var row = new TableRow();

            tab.Rows.Add(row);

            TableCell cell = null;

            if (topNode != null)
            {
                foreach (SiteMapNode titleNode in topNode.ChildNodes)
                {
                    if (linkCount > avgLinkPerColumn || cell == null)
                    {
                        cell = new TableCell();
                        row.Cells.Add(cell);
                        cell.Width = Unit.Pixel(325);
                        cell.VerticalAlign = VerticalAlign.Top;
                        linkCount = 0;
                    }

                    cell.Controls.Add(this.LinkBlock(titleNode));
                    linkCount += titleNode.ChildNodes.Count + 7;
                }
            }

            this.Controls.Add(tab);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private Control LinkBlock(SiteMapNode titleNode)
        {
            if (titleNode.ChildNodes.Count == 0)
            {
                return new LiteralControl(string.Empty);
            }

            var divBlock = new HtmlGenericControl("div");

            divBlock.Attributes.Add("class", "mnbox_int");

            // monta título
            var divTitle = new HtmlGenericControl("div");

            divTitle.Attributes.Add("class", "mnbox_tit");

            var h4 = new HtmlGenericControl("h4");

            h4.Controls.Add(new LiteralControl(titleNode.Title));
            divTitle.Controls.Add(h4);

            if (titleNode is HadesSiteMapNode 
                && !string.IsNullOrEmpty(((HadesSiteMapNode)titleNode).ImageUrl))
            {
                divTitle.Style.Add("background-image", "url(" + this.Page.ResolveClientUrl(((HadesSiteMapNode)titleNode).ImageUrl) + ")");
            }

            divBlock.Controls.Add(divTitle);

            // monta lista
            var ul = new HtmlGenericControl("ul");

            divBlock.Controls.Add(ul);

            foreach (SiteMapNode linkNode in titleNode.ChildNodes)
            {
                var li = new HtmlGenericControl("li");
                var a = new HyperLink
                        {
                            NavigateUrl = string.IsNullOrEmpty(linkNode.Url) ? "#" : linkNode.Url,
                            Text = linkNode.Title
                        };

                li.Controls.Add(a);
                ul.Controls.Add(li);
            }

            return divBlock;
        }
    }
}