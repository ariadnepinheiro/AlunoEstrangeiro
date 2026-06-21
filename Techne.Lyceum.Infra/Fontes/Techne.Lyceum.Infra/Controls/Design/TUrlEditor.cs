using System;
using System.ComponentModel;
using System.Web.UI.Design;
using System.Windows.Forms.Design;

namespace Techne.Controls.Design
{
    internal class TUrlEditor : UrlEditor
    {
        public override object EditValue(ITypeDescriptorContext context, 
                                         IServiceProvider provider, 
                                         object value)
        {
            if ((provider != null) && (provider.GetService(typeof (IWindowsFormsEditorService)) != null))
            {
                IComponent componente = null;
                if (context.Instance as IControlItem != null && ((IControlItem)context.Instance).ParentControl != null)
                {
                    componente = ((IControlItem)context.Instance).ParentControl;
                }
                else if (context.Instance as IComponent != null)
                {
                    componente = (IComponent)context.Instance;
                }
                else if (context.Instance as object[] != null)
                {
                    var compArray = (object[])context.Instance;
                    if (compArray[0] as IComponent != null)
                    {
                        componente = (IComponent)compArray[0];
                    }
                }

                var url = UrlBuilder.BuildUrl(componente, null, (string)value, this.Caption, this.Filter, this.Options);
                if (url != null)
                {
                    value = url;
                }
            }

            return value;
        }
    }
}