using System.Collections;
using System.ComponentModel;
using System.Web.UI;

namespace Techne.Controls
{
    internal class TControlConverter : StringConverter
    {
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var controls = new string[0];

            if (context != null && context.Container != null)
            {
                controls = this.GetControls(context.Container);
            }

            return new StandardValuesCollection(controls);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        private string[] GetControls(IContainer container)
        {
            var result = new ArrayList();

            if (container != null && container.Components != null)
            {
                foreach (IComponent component in container.Components)
                {
                    if (component is ITControl)
                    {
                        result.Add(((Control)component).ID);
                    }
                }
            }

            return (string[])result.ToArray(typeof (string));
        }
    }
}