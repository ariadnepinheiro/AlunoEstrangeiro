using System.Collections;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Techne.Controls
{
    internal class DataGridConverter : StringConverter
    {
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(
                (context == null || context.Container == null) ? new string[0] : this.GetControls(context.Container)
                );
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
                    if (component is DataGrid)
                    {
                        result.Add(((DataGrid)component).ID);
                    }
                }
            }

            return (string[])result.ToArray(typeof (string));
        }
    }
}