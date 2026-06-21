using System.Collections;
using System.ComponentModel;
using System.Web.UI;

namespace Techne.Controls
{
    /// <summary>
    ///   Obtém lista de todos os RecordManager's.
    /// </summary>
    internal class RecordManagerConverter : StringConverter
    {
        /// <summary>
        ///   Dado um container, obtém uma lista de todos os RecordManager's contido nele.
        /// </summary>
        public static string[] GetManagers(IContainer container)
        {
            var result = new ArrayList();

            foreach (IComponent c in container.Components)
            {
                if (c is IContainerManager)
                {
                    result.Add(((Control)c).ID);
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
                return new StandardValuesCollection(GetManagers(context.Container));
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