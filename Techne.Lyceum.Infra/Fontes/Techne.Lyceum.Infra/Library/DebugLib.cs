using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Web.UI;

namespace Techne
{
// DateTime _start = DebugLib.Start(System.Reflection.MethodBase.GetCurrentMethod());

// try {

// // --- Método original --- //

// }

// finally {

// DebugLib.End(System.Reflection.MethodBase.GetCurrentMethod(), _start);

// }

    internal class DebugLib
    {
        public static void End(MethodBase debugMethod, DateTime start)
        {
            var debugElapsed = DateTime.Now - start;
        }

        public static void PrintControlTree(Page page)
        {
            PrintControlTree(page, 0);
        }

        /// <param name = "pageState">Objeto devolvido por LoadPageStateFromPersistenceMedium().</param>
        public static void PrintPageViewState(object pageState)
        {
            var state = pageState as Triplet;
            if (state == null)
            {
                throw new ArgumentException();
            }

            var typeHashCode = (string)state.First;
            var controlsRequiringPostBack = (ArrayList)state.Third;

            DebugLib.PrintStateTriplet((Triplet)state.Second, 0);
        }

        public static DateTime Start(MethodBase debugMethod)
        {
            return DateTime.Now;
        }

        private static void PrintControlTree(Control control, int order)
        {
            var controls = control.Controls;
            for (var i = 0; i < controls.Count; i++)
            {
                PrintControlTree(controls[i], i);
            }
        }

        private static string PrintObject(object obj)
        {
            var b = new StringBuilder();

            if (obj == null)
            {
                b.Append("null");
            }
            else if (obj is object[])
            {
                b.Append("object[](");
                var first = true;
                foreach (var item in (object[])obj)
                {
                    if (!first)
                    {
                        b.Append(", ");
                    }
                    else
                    {
                        first = false;
                    }

                    b.Append(PrintObject(item));
                }

                b.Append(")");
            }
            else if (obj is ArrayList)
            {
                b.Append("ArrayList(");
                var first = true;
                foreach (var item in (ArrayList)obj)
                {
                    if (!first)
                    {
                        b.Append(", ");
                    }
                    else
                    {
                        first = false;
                    }

                    b.Append(PrintObject(item));
                }

                b.Append(")");
            }
            else if (obj is Pair)
            {
                b.Append("Pair(");
                b.Append(PrintObject(((Pair)obj).First));
                b.Append(", ");
                b.Append(PrintObject(((Pair)obj).Second));
                b.Append(")");
            }
            else if (obj is Triplet)
            {
                b.Append("Triplet(");
                b.Append(PrintObject(((Triplet)obj).First));
                b.Append(", ");
                b.Append(PrintObject(((Triplet)obj).Second));
                b.Append(", ");
                b.Append(PrintObject(((Triplet)obj).Third));
                b.Append(")");
            }
            else if (obj is string)
            {
                b.Append("\"" + (string)obj + "\"");
            }
            else if (obj is bool || obj is decimal || obj is int || obj is DateTime || obj.GetType().IsEnum)
            {
                b.Append(Convert.ToString(obj, CultureInfo.InvariantCulture));
            }
            else
            {
                b.Append(obj.GetType().FullName);
            }

            return b.ToString();
        }

        private static void PrintStateTriplet(Triplet t, int childOrder)
        {
            var second = (ArrayList)t.Second;
            if (t.Third != null)
            {
                var i = 0;
                foreach (Triplet child in (ArrayList)t.Third)
                {
                    PrintStateTriplet(child, second != null && i < second.Count ? (int)second[i++] : -1);
                }
            }
        }
    }
}