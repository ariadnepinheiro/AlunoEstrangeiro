using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace Techne.Controls
{
    /// <summary>
    ///   Controle WEB que é simples controle HTML (HtmlImageButton) quando a propriedade Url é informada
    ///   e controle WEB (LinkButton) caso contrário (neste caso o evento Click deve ser tratado).
    /// </summary>
    [Designer(typeof (ImageLinkDesigner)), ToolboxItem(false)]
    internal class ImageLink : WebControl, INamingContainer
    {
        private readonly LinkButton ibt;

        private string commandArgument;

        private string commandName;

        private string focusImageUrl;

        private string imageUrl;

        private string url;

        public ImageLink()
        {
            this.BlockMultipleSubmit = true;
            this.CommandArgument = string.Empty;
            this.CommandName = string.Empty;
            this.FocusImageUrl = string.Empty;
            this.ImageUrl = string.Empty;
            this.Url = string.Empty;

            this.ibt = new LinkButton();
        }

        public event EventHandler Click;

        [DefaultValue(true), Category("Behavior"), Description("Bloqueia m\x00faltiplos submits. S\x00f3 \x00e9 v\x00e1lido se a Url for vazia")]
        public bool BlockMultipleSubmit { get; set; }

        [Category("Behavior"), DefaultValue("")]
        public string CommandArgument
        {
            get
            {
                return this.commandArgument;
            }

            set
            {
                this.commandArgument = value == null ? string.Empty : value;
            }
        }

        [DefaultValue(""), Category("Behavior")]
        public string CommandName
        {
            get
            {
                return this.commandName;
            }

            set
            {
                this.commandName = value == null ? string.Empty : value;
            }
        }

        [Editor(typeof (ImageUrlEditor), typeof (UITypeEditor)), Category("Appearance"), DefaultValue("")]
        public string FocusImageUrl
        {
            get
            {
                return this.focusImageUrl;
            }

            set
            {
                this.focusImageUrl = value == null ? string.Empty : value;
            }
        }

        [Editor(typeof (ImageUrlEditor), typeof (UITypeEditor)), Category("Appearance"), DefaultValue("")]
        public string ImageUrl
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

        [DefaultValue(""), Description("Url para a qual um link html ser\x00e1 criado. Caso n\x00e3o seja informado, o evento Click ser\x00e1 disparado se a imagem for clicada. Esta propriedade nunca retorna null.")]
        public string Url
        {
            get
            {
                return this.url;
            }

            set
            {
                this.url = value == null ? string.Empty : value;
            }
        }

        private bool IsLink
        {
            get
            {
                return this.Url != string.Empty;
            }
        }

        protected override void LoadViewState(object savedState)
        {
        }

        protected override void OnInit(EventArgs args)
        {
            base.OnInit(args);

            this.ibt.ID = this.GetType().Name;
            this.ibt.Click += this.ibt_Click;
            this.ibt.CommandName = this.CommandName;
            this.ibt.CommandArgument = this.CommandArgument;
            this.Controls.Add(this.ibt);
        }

        protected override void OnPreRender(EventArgs args)
        {
            base.OnPreRender(args);

            if (!TControl.InDesignMode(this))
            {
                TControl.RegisterTControlScript(this);
            }

            this.ibt.ToolTip = this.ToolTip;
            var img = new Image();
            img.ImageUrl = this.ImageUrl;
            this.ibt.Controls.Add(img);
            this.ibt.AccessKey = this.AccessKey;
            if (this.FocusImageUrl.Trim().Length > 0)
            {
                this.ibt.Attributes.Add("OnFocus", "setImage(this, '" + TUtil.TranslateRelativeUrl(this.FocusImageUrl) + "')");
                this.ibt.Attributes.Add("OnBlur", "setImage(this, '" + TUtil.TranslateRelativeUrl(this.ImageUrl) + "')");
            }

            if (this.BlockMultipleSubmit == false)
            {
                this.ibt.Attributes.Add("bypassCheck", "true");
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.IsLink)
            {
                var lnk = new HtmlImageButton();
                lnk.ID = this.ID;
                lnk.HRef = TUtil.TranslateRelativeUrl(this.Url, lnk);
                lnk.Src = TUtil.TranslateRelativeUrl(this.ImageUrl, lnk);
                lnk.FocusSrc = TUtil.TranslateRelativeUrl(this.FocusImageUrl, lnk);
                lnk.Alt = this.ToolTip;
                lnk.AccessKey = this.AccessKey;
                lnk.RenderControl(writer);
            }
            else
            {
                base.Render(writer);
            }
        }

        protected override object SaveViewState()
        {
            return null;
        }

        private void ibt_Click(object sender, EventArgs e)
        {
            if (this.Click != null)
            {
                this.Click(this, EventArgs.Empty);
            }
        }
    }

    internal class ImageLinkDesigner : ControlDesigner
    {
        public override string GetDesignTimeHtml()
        {
            var sw = new StringWriter();
            var writer = new HtmlTextWriter(sw);

            var control = (ImageLink)this.Component;

            var image = new Image();
            image.ImageUrl = TUtil.TranslateRelativeUrl(control.ImageUrl, control);
            image.RenderControl(writer);

            return sw.ToString();
        }
    }
}