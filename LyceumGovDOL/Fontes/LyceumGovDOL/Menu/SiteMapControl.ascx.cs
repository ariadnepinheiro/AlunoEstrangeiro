namespace Techne.Lyceum.Net.Menu
{
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.ComponentModel;
    using System.Web.UI.HtmlControls;

    public partial class SiteMapControl : System.Web.UI.UserControl
    {
        private string _siteMapProvider = "";
        private string _startNode = "";
        private int _columns = 3;

        public string SiteMapProvider
        {
            get { return _siteMapProvider; }
            set { _siteMapProvider = value == null ? "" : value; }
        }

        public string StartNode
        {
            get { return _startNode; }
            set { _startNode = value == null ? "" : value; }
        }

        [DefaultValue(3)]
        public int Columns
        {
            get { return _columns; }
            set { _columns = value<1? 1: value; }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            System.Web.SiteMapProvider provider = null;
            if (_siteMapProvider.Trim().Length == 0)
                provider = SiteMap.Provider;
            else
                provider = SiteMap.Providers[SiteMapProvider];
           
            SiteMapNode topNode = null;
            if (provider != null)
            {
                if (!string.IsNullOrEmpty(StartNode))
                {
                    SiteMapNode root = provider.RootNode;
                    for (int i = 0; i < root.ChildNodes.Count; i++)
                    {
                        SiteMapNode item = root.ChildNodes[i];
                        if (item.Title != null && item.Title.Equals(StartNode, StringComparison.InvariantCulture))
                        {
                            topNode = item;
                            break;
                        }
                    }
                }
                topNode=provider.FindSiteMapNode(this.Context);
            }

            //conta links para distribuí-los mais ou menos igualmente entre as colunas
            int linkCount = 0;
            int avgLinkPerColumn = 1;
            if(topNode!=null)
            {
                foreach (SiteMapNode titleNode in topNode.ChildNodes)
                {
                    linkCount+=titleNode.ChildNodes.Count+7;
                }
            }
            avgLinkPerColumn = linkCount / Columns;

            //monta a tabela de links
            linkCount = 0;
            Table tab = new Table();
            tab.BorderStyle = BorderStyle.None;
            tab.CellPadding = 0;
            tab.CellSpacing = 0;
            TableRow row = new TableRow();
            tab.Rows.Add(row);
            TableCell cell = null;

            if (topNode != null)
            {
                foreach (SiteMapNode titleNode in topNode.ChildNodes)
                {
                    if (linkCount > avgLinkPerColumn || cell==null)
                    {
                        cell = new TableCell();
                        row.Cells.Add(cell);
                        cell.Width = Unit.Pixel(325);
                        cell.VerticalAlign = VerticalAlign.Top;
                        linkCount = 0;
                    }
                    cell.Controls.Add(LinkBlock(titleNode));
                    linkCount += (titleNode.ChildNodes.Count+7);
                }
            }

            this.Controls.Add(tab);

        }

        private Control LinkBlock(SiteMapNode titleNode)
        {
            //Monta um bloco de links neste formato:
            /*
                <div class="mnbox_int">
                <div class="mnbox_tit" style="background-image:url(../images/?????.png)">
                    <h4>Titulo</h4>
                </div>
                <ul>
                    <li><a href="../url">Link</a></li>
                    <li><a href="../url">Link</a></li>
                    <li><a href="../url">Link</a></li>
                    <li><a href="../url">Link</a></li>
                    <li><a href="../url">Link</a></li>
                    <li><a href="../url">Link</a></li>
                    <li><a href="../url">Link</a></li>
                </ul>
                </div>             
             */

            if (titleNode.ChildNodes.Count == 0)
                return new LiteralControl("");

            HtmlGenericControl divBlock = new HtmlGenericControl("div");
            divBlock.Attributes.Add("class","mnbox_int");

            //monta título
            HtmlGenericControl divTitle = new HtmlGenericControl("div");
            divTitle.Attributes.Add("class", "mnbox_tit");

            HtmlGenericControl h4 = new HtmlGenericControl("h4");
            h4.Controls.Add(new LiteralControl(titleNode.Title));
            divTitle.Controls.Add(h4);
            if (titleNode is Techne.Web.HadesSiteMapNode && !string.IsNullOrEmpty(((Techne.Web.HadesSiteMapNode)titleNode).ImageUrl))
                divTitle.Style.Add("background-image", "url(" + this.Page.ResolveClientUrl(((Techne.Web.HadesSiteMapNode)titleNode).ImageUrl) + ")");
            divBlock.Controls.Add(divTitle);

            //monta lista
            HtmlGenericControl ul = new HtmlGenericControl("ul");
            divBlock.Controls.Add(ul);
            foreach(SiteMapNode linkNode in titleNode.ChildNodes)
            {
                HtmlGenericControl li = new HtmlGenericControl("li");
                HyperLink a = new HyperLink();
                a.NavigateUrl = (string.IsNullOrEmpty(linkNode.Url)?"#":linkNode.Url);
                a.Text = linkNode.Title;
                li.Controls.Add(a);
                ul.Controls.Add(li);
            }

            return divBlock;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}