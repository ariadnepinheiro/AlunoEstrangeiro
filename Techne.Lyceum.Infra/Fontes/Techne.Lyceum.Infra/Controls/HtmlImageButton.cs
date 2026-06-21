using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Techne.Controls
{
    /// <summary>
    ///   Controle HTML de imagem com link.
    /// </summary>
    internal class HtmlImageButton : HtmlControl
    {
        private string pvAccessKey = string.Empty;

        private string pvAlt = string.Empty;

        private string pvFocusSrc = string.Empty;

        private string pvHRef = string.Empty;

        private string pvSrc = string.Empty;

        public string AccessKey
        {
            get
            {
                return this.pvAccessKey;
            }

            set
            {
                this.pvAccessKey = value == null ? string.Empty : value.Trim();
            }
        }

        public string Alt
        {
            get
            {
                return this.pvAlt;
            }

            set
            {
                this.pvAlt = value == null ? string.Empty : value.Trim();
            }
        }

        public string FocusSrc
        {
            get
            {
                return this.pvFocusSrc;
            }

            set
            {
                this.pvFocusSrc = value == null ? string.Empty : value.Trim();
            }
        }

        public string HRef
        {
            get
            {
                return this.pvHRef;
            }

            set
            {
                this.pvHRef = value == null ? string.Empty : value.Trim();
            }
        }

        public string Src
        {
            get
            {
                return this.pvSrc;
            }

            set
            {
                this.pvSrc = value == null ? string.Empty : value.Trim();
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            var a = new HtmlAnchor();
            var i = new HtmlImage();

            a.HRef = this.pvHRef;
            if (this.AccessKey.Length > 0)
            {
                a.Attributes.Add("accessKey", this.AccessKey);
            }

            if (this.FocusSrc.Trim().Length > 0)
            {
                a.Attributes.Add("onFocus", "setImage(this,'" + this.pvFocusSrc + "')");
                a.Attributes.Add("onBlur", "setImage(this,'" + this.pvSrc + "')");
            }

            i.Src = this.pvSrc;
            i.Alt = this.pvAlt;
            i.Border = 0;

            a.Controls.Add(i);

            a.RenderControl(writer);

            a = null;
            i = null;
        }
    }
}