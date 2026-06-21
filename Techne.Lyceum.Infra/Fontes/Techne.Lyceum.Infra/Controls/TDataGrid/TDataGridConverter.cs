using System.Collections;
using System.ComponentModel;

namespace Techne.Controls
{
    internal class TDataGridConverter : StringConverter
    {
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(
                context.Container == null ? new string[0] : this.GetControls(context.Container)
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

            foreach (IComponent component in container.Components)
            {
                if (component is TDataGrid)
                {
                    result.Add(((TDataGrid)component).ID);
                }
            }

            return (string[])result.ToArray(typeof (string));
        }
    }
}