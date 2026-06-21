using System.Collections;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Techne.Controls
{
    /// <summary>
    ///   Obtém lista de todos os Label's.
    /// </summary>
    internal class LabelConverter : StringConverter
    {
        /// <summary>
        ///   Dado um container, obtém uma lista de todos os Label's contido nele.
        /// </summary>
        public static string[] GetLabels(IContainer container)
        {
            var result = new ArrayList();

            result.Add(string.Empty);
            if (container != null)
            {
                foreach (IComponent c in container.Components)
                {
                    if (c is Label)
                    {
                        result.Add(((Label)c).ID);
                    }
                }
            }

            if (result.Count == 0)
            {
                return new string[0];
            }
            else
            {
                return (string[])result.ToArray(typeof (string));
            }
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (context != null && context.Container != null)
            {
                return new StandardValuesCollection(GetLabels(context.Container));
            }

            return null;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext desccontext)
        {
            return true;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext desccontext)
        {
            return true;
        }
    }
}