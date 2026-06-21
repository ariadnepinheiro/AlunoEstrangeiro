#define NOSTORE

using System;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Techne.Controls
{
    internal class DbImage : TControlEditable
    {
        private const double CacheSeconds = 60;

        private const string IgsBaseUrl = "cachedimage.techne.ashx?data={0}";

        private string storageKey = string.Empty;

        protected override string EntryValue
        {
            get
            {
                return null;
            }
        }

        protected override void LoadViewState(object savedState)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // Muda propriedade ENCTYPE do form. Exigido pelo controle html <INPUT TYPE="file">.
            var form = TechLib.FindForm(this.Page);
            if (form != null && form.Enctype.Length == 0)
            {
                form.Enctype = "multipart/form-data";
            }

            var cache = HttpContext.Current.Cache;
            this.storageKey = Guid.NewGuid().ToString();

// Guarda a imagem no cache.
            cache.Add(
                this.storageKey, 
                this.DBValue, 
                null, 
                Cache.NoAbsoluteExpiration, 
                TimeSpan.FromSeconds(CacheSeconds), 
                CacheItemPriority.High, 
                null
                );
        }

        protected override void RenderControlEditMode(HtmlTextWriter writer)
        {
            if (InDesignMode(this))
            {
                writer.Write("[" + this.ID + "]");
            }
            else
            {
                this.CreateImageControl().RenderControl(writer);

                if (this.ColumnName.Length > 0)
                {
                    var file = new HtmlInputFile();
                    file.ID = this.ID;
                    file.RenderControl(writer);
                }
            }
        }

        protected override void RenderControlViewMode(HtmlTextWriter writer)
        {
            this.CreateImageControl().RenderControl(writer);
        }

        protected override object SaveViewState()
        {
            return null;
        }

        private Image CreateImageControl()
        {
            var img = new Image();

            if (this.CssClass.Length > 0)
            {
                img.Attributes.Add("class", this.CssClass);
            }

            if (!this.Width.IsEmpty)
            {
                img.Width = this.Width;
            }

            if (!this.Height.IsEmpty)
            {
                img.Height = this.Height;
            }

            img.ImageUrl = string.Format(IgsBaseUrl, this.storageKey);

            return img;
        }
    }
}