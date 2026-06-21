using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Techne.Controls.Design;

namespace Techne.Controls
{
    internal enum ImagePosition
    {
        BeforeText, 
        AfterText
    }

    [
        Designer(typeof (TLinkBaseDesigner)), 
    ]
    internal abstract class TLinkBase : TControl, INamingContainer
    {
        internal const string EmptyText_Def = "";

        internal const string TextSeparator = "&nbsp;";

        private const string FocusImageUrl_Def = "";

        private const ImagePosition ImagePosition_Def = ImagePosition.BeforeText;

        private const string ImageUrl_Def = "";

        private const int TextVerticalBias_Def = -3;

        private const string TextWithImageCssClass_Def = "TLinkButton";

        private const string Text_Def = "?";

        private const string ToolTip_Def = "?";

        private bool InternalControlInitialized;

        private string disableCause = string.Empty;

        private string emptyText;

        private string focusImageUrl;

        private string imageUrl;

        private HtmlImage img;

        private WebControl internalControl;

        private Label lbl;

        private string text;

        private string textWithImageCssClass;

        protected TLinkBase()
        {
            this.EmptyText = EmptyText_Def;
            this.FocusImageUrl = FocusImageUrl_Def;
            this.ImagePosition = ImagePosition_Def;
            this.ImageUrl = ImageUrl_Def;
            this.Text = Text_Def;
            this.TextVerticalBias = TextVerticalBias_Def;
            this.TextWithImageCssClass = TextWithImageCssClass_Def;
            this.ToolTip = ToolTip_Def;
        }

        [
            Category("Appearance"), 
            DefaultValue(ImageUrl_Def), 
            Description(
                "Url da imagem que será utilizado como link. Utilize \"~\" para indicar a raiz da aplicação. " +
                "A propriedade Text poderá ser utilizada simultaneamente. " +
                "Veja também: ImagePosition."
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
                this.imageUrl = value == null ? string.Empty : value.Trim();
            }
        }

        [
            Category("Techne"), 
            DefaultValue(Text_Def), 
            Description(
                "Texto que será utilizado como link. A propriedade ImageUrl poderá ser utilizada simultaneamente. " +
                "Veja também: ImagePosition."
                ), 
        ]
        public virtual string Text
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
            Browsable(false), 
            EditorBrowsable(EditorBrowsableState.Never), 
        ]
        public override bool Enabled
        {
            get
            {
                return this.DisableCause.Length == 0;
            }

            set
            {
                /* Nada faz */
            }
        }

        [
            DefaultValue(ToolTip_Def), 
        ]
        public override string ToolTip
        {
            get
            {
                return base.ToolTip;
            }

            set
            {
                base.ToolTip = value;
            }
        }

        [
            Browsable(false), 
        ]
        public override bool Visible
        {
            get
            {
                if (!base.Visible)
                {
                    // A propriedade Visible foi forçada para false
                    return false;
                }
                else
                {
                    return this.InternalControl != null &&
// Controle está em manager fora do modo View: desabilita.
                           (this.RecordContainer == null || this.RecordContainer.Mode == RecordManagerMode.View || this.ColumnName.Length > 0);
                }
            }

            set
            {
                base.Visible = value;
            }
        }

        /// <summary>
        ///   Motivo pelo qual o controle está desabilitado.
        ///   Em compilações Debug, o texto é utilizado como tooltip para auxiliar o desenvolvedor.
        /// </summary>
        [
            Browsable(false), 
        ]
        public string DisableCause
        {
            get
            {
                return this.disableCause;
            }
        }

        [
            Category("Appearance"), 
            DefaultValue(EmptyText_Def), 
            Description(
                "Texto a ser mostrado quando o valor associado ao controle é DBNull ou string " +
                "vazia e a propriedade ImageUrl não for informada. Nesta situação, se o valor " +
                "desta propriedade for uma string vazia, o link não terá como ser clicado."
                ), 
        ]
        public string EmptyText
        {
            get
            {
                return this.emptyText;
            }

            set
            {
                this.emptyText = value == null ? string.Empty : value;
            }
        }

        [
            Category("Appearance"), 
            DefaultValue(FocusImageUrl_Def), 
            Editor(typeof (ImageUrlEditor), typeof (UITypeEditor))
        ]
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

        [Category("Appearance"), DefaultValue(ImagePosition_Def), Description("Indica qual é a posição da imagem em relação ao texto."),]
        public ImagePosition ImagePosition { get; set; }

        [Category("Appearance"), DefaultValue(TextVerticalBias_Def), Description("Posicionamento do texto em relação à imagem em pixels."),]
        public int TextVerticalBias { get; set; }

        [
            Category("Appearance"), 
            DefaultValue(TextWithImageCssClass_Def), 
            Description("Estilo alternativo para o texto quando a imagem é renderizada juntamente."), 
        ]
        public string TextWithImageCssClass
        {
            get
            {
                return this.textWithImageCssClass;
            }

            set
            {
                this.textWithImageCssClass = value == null ? string.Empty : value.Trim();
            }
        }

        protected WebControl InternalControl
        {
            get
            {
                if (!this.InternalControlInitialized)
                {
                    this.internalControl = this.CreateInternalControl();
                    this.InternalControlInitialized = true;
                }

                return this.internalControl;
            }
        }

        public override sealed void RenderBeginTag(HtmlTextWriter writer)
        {
            // Acerta a visibilidade dos controles internos.
            this.SetChildrenVisibility();

            // Se nenhum dos controles internos for visível, não deve renderizar o próprio controle (InternalControl).
            if (this.InternalControl != null && (this.img.Visible || this.lbl.Visible))
            {
                if (!InDesignMode(this))
                {
                    try
                    {
                        // Seta AccessKey de acordo com o texto
                        var text = this.GetText();
                        if (text.Length > 0)
                        {
                            this.InternalControl.AccessKey = text.Substring(0, 1).ToUpper();
                        }
                    }
                    catch (Exception)
                    {
                        // Se deu exception em GetText() simplesmente deixa de setar AccessKey.
                    }
                }

                this.InternalControl.RenderBeginTag(writer);
            }
        }

        public override sealed void RenderEndTag(HtmlTextWriter writer)
        {
            if (this.InternalControl != null && (this.img.Visible || this.lbl.Visible))
            {
                this.InternalControl.RenderEndTag(writer);
            }
        }

        // É internal para seja possível ser chamado pelo designer e pelo help dinâmico.
        internal virtual string GetAutoText()
        {
            return string.Empty;
        }

        // É internal para seja possível ser chamado pelo designer e pelo help dinâmico.
        internal virtual string GetImageUrl()
        {
            return this.ImageUrl;
        }

        protected virtual WebControl CreateInternalControl()
        {
            return new WebControl(HtmlTextWriterTag.Span);
        }

        protected virtual string GetToolTip(bool convertQuestionToEmpty)
        {
            if (this.DisableCause.Length > 0)
            {
                return string.Empty;
            }

            if (this.RecordContainer == null || this.RecordContainer.Mode == RecordManagerMode.View)
            {
                if (this.ToolTip != "?" || !convertQuestionToEmpty)
                {
                    return this.ToolTip;
                }
            }

            return string.Empty;
        }

        protected override void CreateChildControls()
        {
            this.img = new HtmlImage();
            this.Controls.Add(this.img);
            this.lbl = new Label();
            this.Controls.Add(this.lbl);
        }

        protected override void OnPreRender(EventArgs e)
        {
            try
            {
                base.OnPreRender(e);

                if (this.InternalControl != null && !TControl.InDesignMode(this))
                {
                    this.CopyProperties(this.InternalControl);
                    this.InternalControl.ID = this.ClientID;
                    try
                    {
                        this.InternalControl.ToolTip = this.GetToolTip(true);
                    }
                    catch
                    {
                    }

                    if (this.ImageUrl.Length > 0 && this.FocusImageUrl.Length > 0)
                    {
                        this.InternalControl.Attributes.Add("OnFocus", "setImage(this, '" + TUtil.TranslateRelativeUrl(this.FocusImageUrl) + "');");
                        this.InternalControl.Attributes.Add("OnBlur", "setImage(this, '" + TUtil.TranslateRelativeUrl(this.ImageUrl) + "');");
                    }

                    RegisterTControlScript(this.Page);
                }
            }
            catch (Exception exc)
            {
                throw new Exception("Erro no " + this.ID + ".OnPreRender().", exc);
            }
        }

        protected override void RenderControlViewMode(HtmlTextWriter writer)
        {
            if (this.Visible)
            {
                this.EnsureChildControls();

                this.RenderBeginTag(writer);
                this.RenderContents(writer);
                this.RenderEndTag(writer);
            }
        }

        /// <summary>
        ///   Setando um valor diferente de string vazia, fará com que o controle seja desabilitado.
        ///   Em compilações Debug, o texto informado é utilizado como tooltip para auxiliar o desenvolvedor.
        /// </summary>
        protected void SetDisableCause(string disableCause)
        {
            if (this.DisableCause.Length == 0)
            {
                this.disableCause = disableCause;
            }
        }

        private string GetText()
        {
            var designMode = InDesignMode(this); // Passará por aqui em design mode se o controle estiver dentro de uma grid.

            string text;
            if (this.ColumnName.Length == 0 || this.RecordContainer == null || designMode)
            {
                if (this.Text == "?")
                {
                    text = this.GetAutoText();
                }
                else
                {
                    text = this.Text;
                }

                // Se chegou aqui é porque o desenvolvedor não informou ColumnName.
                // Ou Text foi setado para vazio ou não foi possível obter o texto automático
                // (isso teoricamente não acontece). Assim, deve-se dar exception caso o valor
                // efetivo de ImageUrl também seja vazio.
                if (text.Length == 0 && this.GetImageUrl().Length == 0)
                {
                    throw new InvalidOperationException(
                        "O controle deve renderizar imagem e/ou texto. Verifique se a propriedade " +
                        "Text contém um valor não-vazio, ou informe ColumnName ou ImageUrl."
                        );
                }
            }
            else
            {
                text = this.ToString(this.DBValue);
                if (text.Length == 0 && !this.img.Visible)
                {
                    text = this.EmptyText;
                }
            }

            return text;
        }

        private void SetChildrenVisibility()
        {
            if (this.Page == null)
            {
                throw new InvalidOperationException();
            }

            var designMode = InDesignMode(this); // Passará por aqui em design mode se o controle estiver dentro de uma grid.

            // Inicializa controle interno de imagem.
            var imageUrl = this.GetImageUrl();
            if (imageUrl.Length > 0)
            {
                this.img.Border = 0;
                this.img.Src = TUtil.TranslateRelativeUrl(imageUrl, this);
                if (!designMode)
                {
                    var toolTip = this.GetToolTip(true);
                    if (toolTip.Length > 0)
                    {
                        this.img.Attributes["title"] = toolTip;
                    }
                }
            }
            else
            {
                this.img.Visible = false;
            }

            // Inicializa controle interno de texto.
            string text;
            if (designMode && this.ColumnName.Length > 0)
            {
                text = "<" + this.ColumnName + ">";
            }
            else
            {
                try
                {
                    text = this.GetText();
                }
                catch (Exception exc)
                {
                    // Deixa TControlDesigner tratar.
                    // Estamos em design-mode, então não deve-se criar nova exception passando
                    // exc como innerException, sob a pena de perdermos a mensagem.
                    if (designMode)
                    {
                        throw;
                    }

                    // Aqui podemos dar uma mensagem genérica porque estamos passando exc como
                    // innerException e o stack trace será mostrado por estarmos em runtime.
                    if (!this.img.Visible)
                    {
                        throw new Exception("Ocorreu um problema ao obter o texto do link.", exc);
                    }

                    // Como a imagem será renderizada (com a devida mensagem no seu tooltip), deixa sem texto mesmo.
                    text = string.Empty;
                }
            }

            if (text.Length > 0)
            {
                this.lbl.ToolTip = string.Empty; // Não precisa de ToolTip porque o container (LinkButton) já tem.

                Label sep = null;
                Control txt;
                if (this.img.Visible)
                {
                    if (TextSeparator.Length > 0)
                    {
                        sep = new Label();
                        sep.Text = TextSeparator;
                        sep.Style.Add("text-decoration", "none");
                    }

                    txt = new Label();
                    ((Label)txt).Text = HttpUtility.HtmlEncode(text);
                    ((Label)txt).CssClass = this.TextWithImageCssClass;
                    if (this.img.Visible)
                    {
                        ((Label)txt).Style["position"] = "relative";
                        ((Label)txt).Style["top"] = this.TextVerticalBias.ToString(CultureInfo.InvariantCulture) + "px";
                    }
                }
                else
                {
                    txt = new LiteralControl(HttpUtility.HtmlEncode(text));
                }

                this.lbl.Controls.Clear();
                if (sep != null && this.ImagePosition == ImagePosition.BeforeText)
                {
                    this.lbl.Controls.Add(sep);
                }

                this.lbl.Controls.Add(txt);
                if (sep != null && this.ImagePosition == ImagePosition.AfterText)
                {
                    this.lbl.Controls.Add(sep);
                }
            }
            else
            {
                this.lbl.Visible = false;
            }

            // Acerta a ordem dos controles internos de acordo com ImagePosition.
            if (this.Controls[0] is HtmlImage && this.ImagePosition == ImagePosition.AfterText ||
                this.Controls[0] is Label && this.ImagePosition == ImagePosition.BeforeText)
            {
                // Swap imagem <-> label
                var temp = this.Controls[0];
                this.Controls.RemoveAt(0);
                this.Controls.Add(temp);
            }
        }
    }
}

namespace Techne.Controls.Design
{
    internal class TLinkBaseDesigner : TControlDesigner
    {
        /// <summary>
        ///   Cria um controle visualmente semelhante ao TLinkBase informado.
        ///   Por não possuir funcionalidade incorporada, o controle é utilizado em design-time e help dinâmico.
        /// </summary>
        /// <param name = "helpMode">
        ///   Indica se o controle será utilizado em help dinâmico. Informar false se ele for utilizado em design.
        /// </param>
        public static void RenderHollowControl(TLinkBase control, HtmlTextWriter writer, bool helpMode, string text, string imageUrl)
        {
            var c = new WebControl(HtmlTextWriterTag.Span);
            control.CopyProperties(c);
            c.Enabled = true;
            c.ToolTip = string.Empty;

            HtmlImage img = null;
            if (imageUrl.Length > 0)
            {
                img = new HtmlImage();
                img.Src = TUtil.TranslateRelativeUrl(imageUrl, control);
            }

            HtmlContainerControl a = null;
            if (text.Length > 0)
            {
                a = helpMode ? new HtmlGenericControl() : (HtmlContainerControl)new HtmlAnchor();
                if (a is HtmlAnchor)
                {
                    ((HtmlAnchor)a).HRef = "#";
                }

                a.Attributes.Add("class", control.ReadOnlyCssClass);

                if (img == null)
                {
                    a.InnerHtml = HttpUtility.HtmlEncode(text);
                }
                else
                {
                    Label sep = null;
                    if (TLinkBase.TextSeparator.Length > 0)
                    {
                        sep = new Label();
                        sep.Text = TLinkBase.TextSeparator;
                        sep.Style.Add("text-decoration", "none");
                    }

                    var txt = new Label();
                    txt.Text = HttpUtility.HtmlEncode(text);
                    txt.CssClass = control.TextWithImageCssClass;
                    txt.Style["position"] = "relative";
                    txt.Style["top"] = control.TextVerticalBias.ToString(CultureInfo.InvariantCulture) + "px";

                    if (sep != null && control.ImagePosition == ImagePosition.BeforeText)
                    {
                        a.Controls.Add(sep);
                    }

                    a.Controls.Add(txt);
                    if (sep != null && control.ImagePosition == ImagePosition.AfterText)
                    {
                        a.Controls.Add(sep);
                    }
                }
            }

            if (img != null && control.ImagePosition == ImagePosition.BeforeText)
            {
                c.Controls.Add(img);
            }

            if (a != null)
            {
                c.Controls.Add(a);
            }

            if (img != null && control.ImagePosition == ImagePosition.AfterText)
            {
                c.Controls.Add(img);
            }

            if (img != null || a != null)
            {
                c.RenderControl(writer);
            }
        }

        protected override void RenderControlEditMode(HtmlTextWriter writer)
        {
            var control = (TLinkBase)this.Component;

            var text = control.ColumnName.Length > 0
                           ? "<" + control.ColumnName + ">"
                           : (control.Text == "?" ? control.GetAutoText() : control.Text);

            RenderHollowControl(control, writer, false, text, control.GetImageUrl());
        }

        protected override void RenderControlViewMode(HtmlTextWriter writer)
        {
            this.RenderControlEditMode(writer);
        }
    }
}