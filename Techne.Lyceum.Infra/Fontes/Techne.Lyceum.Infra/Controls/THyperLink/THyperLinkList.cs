using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.IO;
using System.Threading;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Forms.Design;
using Techne.Web;
using ControlDesigner = System.Web.UI.Design.ControlDesigner;

namespace Techne.Controls
{
    [
        Designer(typeof (THyperLinkListDesigner)), 
    ]
    internal class THyperLinkList : WebControl, INamingContainer
    {
        // INamingContainer é implementado para que os THyperLink's renderizados por este controle
        // possuam o nome deste controle no nome deles. Isso é útil nas mensagens das exceptions.
        private const string Align_Def = "";

        private const ListOrientation Orientation_Def = ListOrientation.Vertical;

        private const string TitleCssClass_Def = "PageTitle";

        private const string TitleHeight_Def = "";

        private readonly static bool Border;

        private readonly static string LinkSeparator = "&nbsp;&nbsp;";

        private readonly ArrayList internalLinks = new ArrayList();

        private readonly LinkListCollection links;

        private string align = Align_Def;

        private string hyperLinkCssClass;

        private string imageCssClass;

        private string imageUrl;

        private string title;

        private string titleCssClass;

        private string titleHeight;

        internal enum ListOrientation
        {
            Vertical, 
            Horizontal
        }

        public THyperLinkList()
        {
            this.EnableAuthorization = true;
            this.HyperLinkCssClass = string.Empty;
            this.ImageCssClass = string.Empty;
            this.ImageUrl = string.Empty;
            this.Orientation = Orientation_Def;
            this.Title = string.Empty;
            this.TitleCssClass = TitleCssClass_Def;
            this.TitleHeight = TitleHeight_Def;

            this.links = new LinkListCollection(this);
        }

        [
            Category("Techne"), 
            DefaultValue(""), 
            Description(
                "Válido somente em orientação vertical. " +
                "Ícone renderizado abaixo do título e ao lado esquerdo da lista de links. " +
                "Veja também ImageCssClass."
                ), 
            Editor(typeof (ImageUrlEditor), typeof (UITypeEditor)), 
        ]
        public virtual string ImageUrl
        {
            get
            {
                return this.imageUrl;
            }

            set
            {
                this.imageUrl = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(""), 
            Description(
                "Válido somente para orientação vertical. " +
                "Renderiza título no topo do controle. " +
                "Veja também TitleCssClass e TitleHeight."
                ), 
            PersistenceMode(PersistenceMode.Attribute), 
        ]
        public virtual string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                this.title = value == null ? string.Empty : value;
            }
        }

        [
            Category("Layout"), 
            DefaultValue(Align_Def), 
        ]
        public string Align
        {
            get
            {
                return this.align;
            }

            set
            {
                this.align = value == null ? string.Empty : value;
            }
        }

        [
            Category("Appearance"), 
            DefaultValue(""), 
        ]
        public string HyperLinkCssClass
        {
            get
            {
                return this.hyperLinkCssClass;
            }

            set
            {
                this.hyperLinkCssClass = value == null ? string.Empty : value;
            }
        }

        [
            Category("Appearance"), 
            DefaultValue("Estilo do ícone informado na propriedade ImageUrl."), 
        ]
        public string ImageCssClass
        {
            get
            {
                return this.imageCssClass;
            }

            set
            {
                this.imageCssClass = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne"), 
            DesignerSerializationVisibility(DesignerSerializationVisibility.Content), 
            PersistenceMode(PersistenceMode.InnerProperty), 
        ]
        public LinkListCollection Links
        {
            get
            {
                return this.links;
            }
        }

        [Category("Techne"), DefaultValue(Orientation_Def),]
        public ListOrientation Orientation { get; set; }

        [
            Category("Appearance"), 
            Description("Estilo do título no topo do controle. Veja também Title."), 
            DefaultValue(TitleCssClass_Def), 
        ]
        public string TitleCssClass
        {
            get
            {
                return this.titleCssClass;
            }

            set
            {
                this.titleCssClass = value == null ? string.Empty : value;
            }
        }

        [
            Category("Layout"), 
            Description("Altura da área reservada ao título no topo do controle. Ignorado se a propriedade Title for vazia. Veja também Title."), 
            DefaultValue(TitleHeight_Def), 
        ]
        public string TitleHeight
        {
            get
            {
                return this.titleHeight;
            }

            set
            {
                this.titleHeight = value == null ? string.Empty : value;
            }
        }

        protected internal bool EnableAuthorization { get; set; }

        internal void CopyProperties(THyperLinkList target)
        {
            target.Align = this.Align;
            target.CssClass = this.CssClass;
            target.Height = this.Height;
            target.HyperLinkCssClass = this.HyperLinkCssClass;
            target.ImageCssClass = this.ImageCssClass;
            target.ImageUrl = this.ImageUrl;
            target.Orientation = this.Orientation;
            target.Title = this.Title;
            target.TitleCssClass = this.TitleCssClass;
            target.TitleHeight = this.TitleHeight;
            target.Width = this.Width;
        }

        internal Control CreateControl(bool designMode)
        {
            if (this.Orientation == ListOrientation.Vertical)
            {
                return this.CreateControlVertical(designMode);
            }
            else
            {
                return this.CreateControlHorizontal(designMode);
            }
        }

        protected override void CreateChildControls()
        {
            var designMode = TUtil.IsDesignMode(this);

            // Marca links visíveis
            var visiblelinks = 0;
            if (!designMode)
            {
                visiblelinks = this.Links.Count;
                foreach (LinkListItem link in this.Links)
                {
                    // TODO Esta verificação não é precisa. O próprio THyperLink faz isso melhor.
                    if (link.NavigationMethod.Length == 0 && link.NavigateUrl.Length == 0 ||
                        link.NavigateUrl.Length > 0 && !link.NavigateUrlAuthorized)
                    {
                        visiblelinks--;
                    }
                }
            }

            this.Controls.Clear();

            if (visiblelinks > 0 || designMode)
            {
                this.Controls.Add(this.CreateControl(designMode));
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            var render = false;
            foreach (WebControl ctl in this.internalLinks)
            {
                if (ctl.Visible)
                {
                    render = true;
                }
            }

            if (render)
            {
                this.RenderChildren(writer);
            }
        }

        private Control CreateControlHorizontal(bool designMode)
        {
            var span = new WebControl(HtmlTextWriterTag.Span);
            span.CssClass = this.CssClass;
            span.Width = this.Width;
            span.Height = this.Height;

            var count = 0;
            foreach (LinkListItem link in this.Links)
            {
                var ctl = link.CreateControl(designMode, count++);
                if (ctl == null)
                {
                    continue;
                }

                this.internalLinks.Add(ctl);

                if (span.Controls.Count > 0)
                {
                    span.Controls.Add(new LiteralControl(LinkSeparator));
                }

                span.Controls.Add(ctl);
            }

            return span;
        }

        private HtmlTable CreateControlVertical(bool designMode)
        {
            const int imageWidth = 44;
            const int defaultWidth = 220;
            var hasTitle = this.Title.Length > 0;
            var hasImage = this.ImageUrl.Length > 0;

            var table = new HtmlTable();
            table.Align = this.Align;
            table.Border = Border ? 1 : 0;
            if (Border)
            {
                table.BorderColor = "Red";
            }

            table.CellPadding = Border ? 5 : 1;
            table.CellSpacing = Border ? 3 : 0;
            if (this.CssClass.Length > 0)
            {
                table.Attributes.Add("class", this.CssClass);
            }

            if (!this.Width.IsEmpty)
            {
                table.Width = this.Width.ToString();
            }
            else
            {
                table.Width = defaultWidth.ToString();
            }

            if (!this.Height.IsEmpty)
            {
                table.Height = this.Height.ToString();
            }

            // Título
            if (hasTitle)
            {
                var rowTitle = new HtmlTableRow();
                table.Rows.Add(rowTitle);
                if (this.TitleHeight.Length > 0)
                {
                    rowTitle.Height = this.TitleHeight;
                }

                var cell = new HtmlTableCell();
                cell.InnerText = this.Title;
                if (this.TitleCssClass.Length > 0)
                {
                    cell.Attributes.Add("class", this.TitleCssClass);
                }

                if (hasImage)
                {
                    cell.ColSpan = 2;
                }

                rowTitle.Cells.Add(cell);
            }

            var row = new HtmlTableRow();
            table.Rows.Add(row);

            // Imagem
            if (hasImage)
            {
                var cellImage = new HtmlTableCell();
                cellImage.Width = imageWidth + "px";
                cellImage.VAlign = "top";
                if (this.ImageCssClass.Length > 0)
                {
                    cellImage.Attributes.Add("class", this.ImageCssClass);
                }

                var image = new Image();
                image.ImageUrl = designMode ? TUtil.TranslateRelativeUrl(this.ImageUrl, this) : this.ImageUrl;
                cellImage.Controls.Add(image);

                row.Cells.Add(cellImage);
            }
            {
// Links
                var cellLinks = new HtmlTableCell();
                cellLinks.VAlign = "top";
                cellLinks.Align = "left";

                if (this.Width.IsEmpty)
                {
                    cellLinks.Width = (defaultWidth - (hasImage ? imageWidth : 0)).ToString();
                }
                else if (this.Width.Type == UnitType.Pixel)
                {
                    cellLinks.Width = (this.Width.Value - (hasImage ? imageWidth : 0)).ToString();
                }
                else
                {
                    cellLinks.Width = "100%";
                }

                cellLinks.Controls.Add(this.CreateLinksTable(designMode));

                row.Cells.Add(cellLinks);
            }

            return table;
        }

        private HtmlControl CreateLinksTable(bool designMode)
        {
            var tabLinks = new HtmlTable();

            tabLinks.Border = Border ? 1 : 0;
            if (Border)
            {
                tabLinks.BorderColor = "Blue";
            }

            tabLinks.CellPadding = Border ? 5 : 0;
            tabLinks.CellSpacing = Border ? 3 : 0;
            tabLinks.Width = "100%";
            tabLinks.Height = "100%";

            var count = 0;
            foreach (LinkListItem link in this.Links)
            {
                var ctl = link.CreateControl(designMode, count++);
                if (ctl == null)
                {
                    continue;
                }

                this.internalLinks.Add(ctl);

                var row = new HtmlTableRow();
                tabLinks.Rows.Add(row);

                // cell
                var cell = new HtmlTableCell();
                row.Cells.Add(cell);
                if (this.HyperLinkCssClass.Length > 0)
                {
                    cell.Attributes.Add("class", this.HyperLinkCssClass);
                }

                cell.Controls.Add(ctl);
            }

            return tabLinks;
        }
    }

    internal class LinkListItem
    {
        private const bool ShowWhenUnauthorized_Def = false;

        private const string Text_Def = "?";

        private const string ToolTip_Def = "?";

        private string navigateUrl = string.Empty;

        private string navigationMethod = string.Empty;

        private string navigationParameters = string.Empty;

        private THyperLinkList parent;

        private bool showWhenUnauthorized;

        private string text = Text_Def;

        private string toolTip = ToolTip_Def;

        [Editor(typeof (LinkListUrlEditor), typeof (UITypeEditor)), Category("Techne"), DefaultValue("")]
        public string NavigateUrl
        {
            get
            {
                return this.navigateUrl;
            }

            set
            {
                this.navigateUrl = value == null ? string.Empty : value;
            }
        }

        [
            Browsable(false), 
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), 
        ]
        public bool NavigateUrlAuthorized
        {
            get
            {
                if ((this.parent == null) || TControl.InDesignMode(this.parent))
                {
                    return false;
                }

                if (this.parent.EnableAuthorization)
                {
                    return TechneAuthorization.IsUrlAuthorized(this.navigateUrl);
                }

                return true;
            }
        }

        [TypeConverter(typeof (GetUrlConverter)), DefaultValue(""), Category("Techne")]
        public string NavigationMethod
        {
            get
            {
                return this.navigationMethod;
            }

            set
            {
                this.navigationMethod = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(""), 
            Description(
                "Valores a serem passados como parâmetros à página indicada pela propriedade NavigationMethod. " +
                "São permitidas referências a TControls (sintaxe #controle#) e campos (sintaxe $campo$). " +
                "Valores constantes devem ser informados no formato de cultura invariante. " +
                "DBNull deve ser informado como string vazia."
                ), 
        ]
        public string NavigationParameters
        {
            get
            {
                return this.navigationParameters;
            }

            set
            {
                this.navigationParameters = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(ShowWhenUnauthorized_Def), 
        ]
        public bool ShowWhenUnauthorized
        {
            get
            {
                return this.showWhenUnauthorized;
            }

            set
            {
                this.showWhenUnauthorized = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(Text_Def), 
        ]
        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                this.text = value == null ? string.Empty : value.Trim();
            }
        }

        [
            Category("Techne"), 
            DefaultValue(ToolTip_Def), 
        ]
        public string ToolTip
        {
            get
            {
                return this.toolTip;
            }

            set
            {
                this.toolTip = value == null ? string.Empty : value;
            }
        }

        internal THyperLinkList ParentControl
        {
            get
            {
                return this.parent;
            }

            set
            {
                this.parent = value;
            }
        }

        public override string ToString()
        {
            return this.GetText(true);
        }

        internal Control CreateControl(bool designMode, int pos)
        {
            var designModeId = this.ParentControl.ID + "[" + pos + "]";

            if (designMode)
            {
                var ctl = new HyperLink();
                var text = this.GetText(false);
                if (text.Length == 0 || text == "?")
                {
                    text = "[" + designModeId + "]";
                }

                ctl.Text = text;
                ctl.NavigateUrl = "#"; // Só para o link aparecer
                return ctl;
            }

                // Usa NavigateUrl ao invés de NavigationMethod.
            else if (this.navigationMethod.Length == 0 && this.NavigateUrl.Length > 0)
            {
                var authorized = this.NavigateUrlAuthorized;

                if (!this.showWhenUnauthorized && !authorized)
                {
                    return null;
                }

                var lnk = new THyperLinkInternal();
                lnk.Text = this.text;
                lnk.CssClass = TControl.ReadOnlyCssClass_Def;
                lnk.EnableAuthorization = this.parent.EnableAuthorization;
                lnk.ShowWhenUnauthorized = this.showWhenUnauthorized;
                lnk.ToolTip = this.ToolTip;
                if (this.text != null && this.text.Trim().Length > 0)
                {
                    lnk.AccessKey = this.text.Trim().Substring(0, 1).ToUpper();
                }

                if (authorized)
                {
                    lnk.NavigateUrl = this.navigateUrl;
                }

                return lnk;
            }
            else
            {
                var hyp = new THyperLink();
                hyp.ID = "hyp" + pos;
                hyp.Text = this.text;
                hyp.NavigationMethod = this.navigationMethod;
                hyp.NavigationParameters = this.navigationParameters;
                hyp.EnableAuthorization = this.parent.EnableAuthorization;
                hyp.ShowWhenUnauthorized = this.showWhenUnauthorized;
                hyp.ImageUrl = string.Empty;
                hyp.ToolTip = this.ToolTip;
                return hyp;
            }
        }

        internal string GetText(bool convertQuestionToEmpty)
        {
            var text = this.Text;

            if (text == "?" && this.navigationMethod.Length > 0)
            {
                try
                {
                    text = TPage.GetPageCaption(Navigation.GetType(this.NavigationMethod), Thread.CurrentThread.CurrentCulture.Name);
                }
                catch
                {
                    // Vai devolver text.
                }
            }

            if (text == "?" && convertQuestionToEmpty)
            {
                return string.Empty;
            }
            else
            {
                return text;
            }
        }
    }

    internal class LinkListCollection : CollectionBase
    {
        readonly THyperLinkList pvParentControl;

        internal LinkListCollection(THyperLinkList parent)
        {
            this.pvParentControl = parent;
        }

        public LinkListItem this[int index]
        {
            get
            {
                return (LinkListItem)this.List[index];
            }

            set
            {
                this.List[index] = value;
            }
        }

        public int Add(LinkListItem item)
        {
            return this.List.Add(item);
        }

        public void Remove(LinkListItem item)
        {
            this.List.Remove(item);
        }

        protected override void OnInsertComplete(int index, object value)
        {
            ((LinkListItem)value).ParentControl = this.pvParentControl;
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            ((LinkListItem)newValue).ParentControl = this.pvParentControl;
        }
    }

    internal class LinkListUrlEditor : UrlEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            string url;
            string caption;
            string filter;
            IComponent componente;
            object[] compArray;

            if ((provider != null) && (provider.GetService(typeof (IWindowsFormsEditorService)) != null))
            {
                url = (String)value;
                caption = this.Caption;
                filter = this.Filter;
                componente = null;
                if (context.Instance as LinkListItem != null && ((LinkListItem)context.Instance).ParentControl != null)
                {
                    componente = ((LinkListItem)context.Instance).ParentControl;
                }
                else if (context.Instance as IComponent != null)
                {
                    componente = (IComponent)context.Instance;
                }
                else if (context.Instance as object[] != null)
                {
                    compArray = (object[])context.Instance;
                    if (compArray[0] as IComponent != null)
                    {
                        componente = (IComponent)compArray[0];
                    }
                }

                url = UrlBuilder.BuildUrl(componente, null, url, caption, filter, this.Options);
                if (url != null)
                {
                    value = url;
                }
            }

            return value;
        }
    }

    internal class THyperLinkListDesigner : ControlDesigner
    {
        public override string GetDesignTimeHtml()
        {
            Trace.WriteLine("THyperLinkListDesigner.GetDesignTimeHtml() BEGIN");
            try
            {
                var control = (THyperLinkList)this.Component;

                if (control.Title.Length == 0 && control.Links.Count == 0 && control.ImageUrl.Length == 0)
                {
                    return this.GetEmptyDesignTimeHtml();
                }

                return this.Render(control);
            }
            catch (Exception exc)
            {
                Trace.WriteLine("THyperLinkListDesigner.GetDesignTimeHtml(): " + exc.Message);
                Trace.WriteLine(exc.StackTrace);
                throw;
            }
            finally
            {
                Trace.WriteLine("THyperLinkListDesigner.GetDesignTimeHtml() END");
            }
        }

        protected override string GetEmptyDesignTimeHtml()
        {
            var control = (THyperLinkList)this.Component;

            var sample = new THyperLinkList();
            sample.Title = "[" + control.ID + "]";
            control.CopyProperties(sample);

            for (var i = 1; i <= 5; i++)
            {
                var item = new LinkListItem();
                item.Text = string.Format("Sample link {0}", i);
                sample.Links.Add(item);
            }

            return this.Render(sample);
        }

        private string Render(THyperLinkList control)
        {
            var sw = new StringWriter();
            var writer = new HtmlTextWriter(sw);
            control.CreateControl(true).RenderControl(writer);
            return sw.ToString();
        }
    }
}