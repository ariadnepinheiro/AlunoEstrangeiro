namespace Techne.Lyceum.Web
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ToolboxData("<{0}:TMenu runat=\"server\"></{0}:TMenu>"), SupportsEventValidation]
    public class TMenu : WebControl
    {
        private readonly Style itemStyle = new Style();

        private readonly Style linkStyle = new Style();

        private readonly Style titleStyle = new Style();

        private Color hoverColor = Color.Empty;

        private string itemImageUrl = string.Empty;

        private string leftSeparatorImageUrl = string.Empty;

        private Color popupColor = Color.Empty;

        private int popupZIndex = 10000;

        private string rightSeparatorImageUrl = string.Empty;

        private string siteMapProvider = string.Empty;

        [DefaultValue(typeof(Color), null)]
        public Color HoverColor
        {
            get
            {
                return this.hoverColor;
            }

            set
            {
                this.hoverColor = value;
            }
        }

        [DefaultValue(""), Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor)), UrlProperty, Description("Imagem de fundo do item"), Bindable(true), Category("Appearance")]
        public string ItemImageUrl
        {
            get
            {
                return this.itemImageUrl;
            }

            set
            {
                this.itemImageUrl = value ?? string.Empty;
            }
        }

        [Category("Styles"), PersistenceMode(PersistenceMode.InnerProperty), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true)]
        public Style ItemStyle
        {
            get
            {
                return this.itemStyle;
            }
        }

        [DefaultValue(""), Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor)), UrlProperty, Description("Imagem do separador esquerdo"), Bindable(true), Category("Appearance")]
        public string LeftSeparatorImageUrl
        {
            get
            {
                return this.leftSeparatorImageUrl;
            }

            set
            {
                this.leftSeparatorImageUrl = value ?? string.Empty;
            }
        }

        [Category("Styles"), PersistenceMode(PersistenceMode.InnerProperty), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true)]
        public Style LinkStyle
        {
            get
            {
                return this.linkStyle;
            }
        }

        [DefaultValue(typeof(Color), null)]
        public Color PopupBackColor
        {
            get
            {
                return this.popupColor;
            }

            set
            {
                this.popupColor = value;
            }
        }

        [DefaultValue(10000)]
        public int PopupZIndex
        {
            get
            {
                return this.popupZIndex;
            }

            set
            {
                this.popupZIndex = value < 0 ? 0 : value;
            }
        }

        [DefaultValue(""), Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor)), UrlProperty, Description("Imagem do separador direito"), Bindable(true), Category("Appearance")]
        public string RightSeparatorImageUrl
        {
            get
            {
                return this.rightSeparatorImageUrl;
            }

            set
            {
                this.rightSeparatorImageUrl = value ?? string.Empty;
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

        [Category("Styles"), PersistenceMode(PersistenceMode.InnerProperty), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true)]
        public Style TitleStyle
        {
            get
            {
                return this.titleStyle;
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

            var itemCss = string.IsNullOrEmpty(this.ItemStyle.CssClass) ? string.Empty : " " + this.ItemStyle.CssClass;
            var menuHtml = new StringBuilder();

            if (provider != null)
            {
                var leftSeparatorSpan = string.Empty;
                var rightSeparatorSpan = string.Empty;

                if (this.leftSeparatorImageUrl
                    != string.Empty)
                {
                    leftSeparatorSpan = string.Format(
                        "<img src='{0}' height='{1}' align='absmiddle' border='0' />",
                        this.Page.ResolveUrl(this.leftSeparatorImageUrl),
                        this.Height);
                }

                if (this.rightSeparatorImageUrl
                    != string.Empty)
                {
                    rightSeparatorSpan = string.Format(
                        "<img src='{0}' height='{1}' align='absmiddle' border='0' />",
                        this.Page.ResolveUrl(this.rightSeparatorImageUrl),
                        this.Height);
                }

                var itemDivStyle = string.Format("float:left;height:{0};margin:0px 1px;", this.Height);

                if (this.ItemImageUrl
                    != string.Empty)
                {
                    itemDivStyle += string.Format("background:url({0});", this.Page.ResolveUrl(this.ItemImageUrl));
                }

                menuHtml.Append("<ul>");

                var root = provider.RootNode;

                for (var i = 0; i < root.ChildNodes.Count; i++)
                {
                    var item = root.ChildNodes[i];
                    var url = string.IsNullOrEmpty(item.Url) ? "#" : this.Page.ResolveUrl(item.Url);
                    var itemClass = i < root.ChildNodes.Count ? "sub" + itemCss : "right" + itemCss;

                    // procura pelo menos descendente com url não vazia
                    var hasChildUrl = false;

                    foreach (SiteMapNode subitem in item.ChildNodes)
                    {
                        if (subitem.ChildNodes.Count > 0)
                        {
                            hasChildUrl = true;
                            break;
                        }
                    }

                    if (!hasChildUrl)
                    {
                        if (url != "#")
                        {
                            menuHtml.Append("<li><div style='");
                            menuHtml.Append(itemDivStyle);
                            menuHtml.Append("'><a href='");
                            menuHtml.Append(url);
                            menuHtml.Append("'>");
                            menuHtml.Append(leftSeparatorSpan);
                            menuHtml.Append(item.Title);
                            menuHtml.Append(rightSeparatorSpan);
                            menuHtml.Append("</a></div></li>");
                        }
                    }
                    else
                    {
                        menuHtml.Append("<li><div style='");
                        menuHtml.Append(itemDivStyle);
                        menuHtml.Append("'><a class='");
                        menuHtml.Append(itemClass);
                        menuHtml.Append("' href='");
                        menuHtml.Append(url);
                        menuHtml.Append("'>");
                        menuHtml.Append(leftSeparatorSpan);
                        menuHtml.Append(item.Title);
                        menuHtml.Append(rightSeparatorSpan);
                        menuHtml.Append("</a></div>");

                        // submenu
                        if (!this.DesignMode)
                        {
                            menuHtml.Append("<div class='holder'>");
                            menuHtml.Append("<div class='leftSide'>");
                            menuHtml.Append("<div class='rightSide'>");

                            this.MontaTabelaSubMenu(menuHtml, item);

                            menuHtml.Append("</div>");
                            menuHtml.Append("</div>");
                            menuHtml.Append("</div>");
                        }
                    }
                }

                menuHtml.Append("</ul>");
            }

            this.Controls.Add(new LiteralControl(menuHtml.ToString()));
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this.RegisterMenuStyle();
        }

        private string FixUrl(string url)
        {
            if (!string.IsNullOrEmpty(url)
                && (url.Length >= 2))
            {
                while (url.EndsWith("#")
                       || url.EndsWith("%23"))
                {
                    if (url.EndsWith("#"))
                    {
                        url = url.Remove(url.Length - 1);
                    }
                    else if (url.EndsWith("%23"))
                    {
                        url = url.Remove(url.Length - 3);
                    }
                }

                return url;
            }

            return url;
        }

        private string GetCssFromStyle(Style style)
        {
            var sb = new StringBuilder(256);
            var c = style.ForeColor;

            if (!c.IsEmpty)
            {
                sb.Append("color:");
                sb.Append(ColorTranslator.ToHtml(c));
                sb.Append(";");
            }

            c = style.BackColor;

            if (!c.IsEmpty)
            {
                sb.Append("background-color:");
                sb.Append(ColorTranslator.ToHtml(c));
                sb.Append(";");
            }

            var fi = style.Font;
            var s = fi.Name;

            if (s.Length != 0)
            {
                sb.Append("font-family:'");
                sb.Append(s);
                sb.Append("';");
            }

            if (fi.Bold)
            {
                sb.Append("font-weight:bold;");
            }
            else
            {
                sb.Append("font-weight:normal;");
            }

            if (fi.Italic)
            {
                sb.Append("font-style:italic;");
            }

            s = string.Empty;

            if (fi.Underline)
            {
                s += "underline";
            }

            if (fi.Strikeout)
            {
                s += " line-through";
            }

            if (fi.Overline)
            {
                s += " overline";
            }

            if (s.Length != 0)
            {
                sb.Append("text-decoration:");
                sb.Append(s);
                sb.Append(';');
            }

            var fu = fi.Size;

            if (fu.IsEmpty == false)
            {
                sb.Append("font-size:");
                sb.Append(fu.ToString(System.Globalization.CultureInfo.InvariantCulture));
                sb.Append(';');
            }

            s = string.Empty;

            var u = style.BorderWidth;
            var bs = style.BorderStyle;

            if (u.IsEmpty == false)
            {
                s = u.ToString(System.Globalization.CultureInfo.InvariantCulture);

                if (bs == BorderStyle.NotSet)
                {
                    s += " solid";
                }
            }

            c = style.BorderColor;

            if (!c.IsEmpty)
            {
                s += " " + ColorTranslator.ToHtml(c);
            }

            if (bs != BorderStyle.NotSet)
            {
                s += " " + Enum.Format(typeof(BorderStyle), bs, "G");
            }

            if (s.Length != 0)
            {
                sb.Append("border:");
                sb.Append(s);
                sb.Append(';');
            }

            return sb.ToString();
        }

        private void MontaTabelaSubMenu(StringBuilder str, SiteMapNode menuItem)
        {
            // tenta arrumar os itens em 2 colunas. mais se não couber
            var colSubItems = new int[menuItem.ChildNodes.Count];
            var numSubItems = new int[menuItem.ChildNodes.Count];
            var total = 0;

            for (var i = 0; i < menuItem.ChildNodes.Count; i++)
            {
                numSubItems[i] = menuItem.ChildNodes[i].ChildNodes.Count + 1;
                total += numSubItems[i];
            }

            var avgColCount = total / 2;

            if (avgColCount > 22)
            {
                avgColCount = 22;
            }

            var currColCount = 0;
            var currCol = 0;

            for (var i = 0; i < numSubItems.Length; i++)
            {
                if (numSubItems[i] == 0)
                {
                    continue;
                }

                if (currColCount > 0
                    && currColCount + numSubItems[i] > avgColCount)
                {
                    currCol++;
                    currColCount = 0;
                }

                currColCount += numSubItems[i];
                colSubItems[i] = currCol;
            }

            // monta tabela
            var titleClass = string.IsNullOrEmpty(this.TitleStyle.CssClass) ? string.Empty : " class='" + this.TitleStyle.CssClass + "'";
            var linkClass = string.IsNullOrEmpty(this.LinkStyle.CssClass) ? string.Empty : " class='" + this.LinkStyle.CssClass + "'";

            str.Append("<table><tbody><tr>");
            str.Append("<td><dl>");

            currCol = 0;

            for (var i = 0; i < colSubItems.Length; i++)
            {
                if (numSubItems[i] == 0)
                {
                    continue;
                }

                if (colSubItems[i] > currCol)
                {
                    str.Append("</dl></td>");
                    str.Append("<td><dl>");

                    currCol++;
                }

                var groupNode = menuItem.ChildNodes[i];

                str.Append("<dt");
                str.Append(titleClass);
                str.Append(">");
                str.Append(groupNode.Title);
                str.Append("</dt>");

                foreach (SiteMapNode item in groupNode.ChildNodes)
                {
                    var urlFixed = this.FixUrl(item.Url);
                    var urlItem = string.IsNullOrEmpty(urlFixed) ? "#" : this.Page.ResolveClientUrl(urlFixed);

                    str.Append("<dd");
                    str.Append(linkClass);
                    str.Append("><a href='");
                    str.Append(urlItem);
                    str.Append("'>");
                    str.Append(item.Title);
                    str.Append("</a></dd>");
                }
            }

            str.Append("</dl></td>");
            str.Append("</tr></tbody></table>");
        }

        private void RegisterMenuStyle()
        {
            if (this.Page.IsCallback)
            {
                return;
            }

            var id = "#" + this.ClientID;
            var height = this.Height.ToString();
            var titleStyleCss = this.GetCssFromStyle(this.TitleStyle);
            var linkStyleCss = this.GetCssFromStyle(this.LinkStyle);
            var itemStyleCss = this.GetCssFromStyle(this.ItemStyle);

            var str = new StringBuilder();

            str.AppendLine("<style type=\"text/css\">");
            str.AppendLine(id + " {height:" + height + ";position:relative; margin: 0; float:left; padding-right:20px;}");
            str.AppendLine(id + " img {vertical-align:middle;}");
            str.AppendLine(id + " ul {padding:0; margin:0; list-style: none;}");
            str.AppendLine(id + " ul li {float:left;}");
            str.AppendLine(id + " ul li a, " + id + " ul li a:active, " + id + " ul li a:link, " + id + " ul li a:visited {padding:0px;text-decoration:none; height:" + height + ";" + itemStyleCss + "}");
            str.AppendLine(id + " ul li div.holder {position:absolute; left:-9999px; border:#CCC 1px solid; z-index:" + this.popupZIndex + ";}");
            str.AppendLine(id + " ul li div .leftSide {float:left;background: #FFF no-repeat left bottom; z-index:" + this.popupZIndex + ";}");
            str.AppendLine(id + " ul li div .rightSide {float:left; margin-left:10px; display:inline; padding:0 10px 0 0;  z-index:" + this.popupZIndex + ";}");
            str.AppendLine(id + " ul li:hover {position:relative;}");
            str.AppendLine(id + " ul li a:hover {background-position:right center; white-space:nowrap; position:relative;}");
            str.AppendLine(id + " ul li a:hover b {background-position:left center;}");
            str.AppendLine(id + " ul li a.sub:hover {background-position:right bottom; white-space:nowrap; position:relative;}");
            str.AppendLine(id + " ul li a.sub:hover b {background-position:left bottom;}");
            str.AppendLine(id + " ul li:hover > a {position:relative;}");
            str.AppendLine(id + " ul li:hover a.sub {background-position:right bottom; white-space:nowrap; position:relative;}");
            str.AppendLine(id + " ul li:hover a.sub > b {background-position:left bottom;}");
            str.AppendLine(id + " ul :hover div.holder {position:absolute; top:" + height + "; left:4px; margin: 0; padding: 0; z-index:" + this.popupZIndex + ";}");
            str.AppendLine(id + " ul li.right a:hover div.holder {left:auto; right:3px; top:" + height + ";z-index:" + this.popupZIndex + ";}");
            str.AppendLine(id + " ul li.right:hover div.holder {left:auto; right:4px; top:" + height + ";z-index:" + this.popupZIndex + ";}");
            str.AppendLine(id + " ul dl {width:auto; margin:5px 0 10px 0; padding:0 5px;list-style:none;}");
            str.AppendLine(id + " ul dl dt {padding:0 10px; margin:0; line-height:" + height + "; white-space:nowrap;" + titleStyleCss + "}");
            str.AppendLine(id + " ul dl dd {display:block; padding:0; margin:0;}");
            str.AppendLine(id + " ul dd a, " + id + " ul dd a:active, " + id + " ul dd a:link, " + id + " ul dd a:visited {background-image: none; display:block; height: " + height + "; line-height: " + height + "; text-align:left; margin: 0; padding:0 10px;white-space:nowrap; float:none;" + linkStyleCss + "}");
            str.AppendLine(id + " ul dd a:hover {color: #00F; text-decoration:underline;}");
            str.AppendLine(id + " ul table td {vertical-align:top;}");
            str.AppendLine("</style>");

            this.Page.Header.Controls.Add(new LiteralControl(str.ToString()));
        }
    }
}