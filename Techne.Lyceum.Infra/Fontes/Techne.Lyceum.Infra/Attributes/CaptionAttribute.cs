using System;
using System.Reflection;

namespace Techne
{
    [
        AttributeUsage(AttributeTargets.Class |
                       AttributeTargets.Method), 
    ]
    public class ControlTextAttribute : TextAttribute
    {
        /// <summary>
        ///   Define o ControlText da página, que será utilizado nos textos dos THyperLink's
        ///   que apontam para ela.
        /// </summary>
        public ControlTextAttribute(string idioma, string controlText) : base(idioma, controlText)
        {
        }

        /// <summary>
        ///   Define o ControlText da página, que será utilizado nos textos dos THyperLink's
        ///   que apontam para ela.
        ///   Utiliza TPage.IDIOMADEFAULT como idioma.
        /// </summary>
        public ControlTextAttribute(string controlText) : base(controlText)
        {
        }

        public string ControlText
        {
            get
            {
                return base.Text;
            }
        }

        /// <summary>
        ///   Devolve o valor do atributo ControlText no idioma fornecido.
        ///   Devolve null se ControlText não foi definido na classe.
        /// </summary>
        public static string GetControlText(Type type, string idioma)
        {
            return TextAttribute.GetText(type, typeof (ControlTextAttribute), idioma);
        }

        /// <summary>
        ///   Devolve o valor do atributo ControlText no idioma fornecido.
        ///   Devolve null se ControlText não foi definido no método.
        /// </summary>
        public static string GetControlText(MethodInfo method, string idioma)
        {
            return TextAttribute.GetText(method, typeof (ControlTextAttribute), idioma);
        }
    }

    [Obsolete("Utilize Techne.ControlTextAttribute")]
    internal class CaptionAttribute : ControlTextAttribute
    {
        public CaptionAttribute(string idioma, string caption) : base(idioma, caption)
        {
        }

        public CaptionAttribute(string caption) : base(caption)
        {
        }

        public string Caption
        {
            get
            {
                return this.ControlText;
            }
        }
    }
}