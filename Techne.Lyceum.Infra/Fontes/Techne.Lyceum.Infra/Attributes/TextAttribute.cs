using System;
using System.Reflection;
using Techne.Controls;

namespace Techne
{
    public abstract class TextAttribute : Attribute
    {
        private readonly string idioma;

        private readonly string text;

        protected TextAttribute(string idioma, string text)
        {
            if (idioma == null || text == null)
            {
                throw new ArgumentNullException();
            }

            if (idioma.Length == 0 || text.Length == 0)
            {
                throw new ArgumentException("Os valores não devem ser vazios.");
            }

            this.idioma = idioma;
            this.text = text;
        }

        protected TextAttribute(string text) : this(TPage.IDIOMADEFAULT, text)
        {
        }

        public string Idioma
        {
            get
            {
                return this.idioma;
            }
        }

        protected string Text
        {
            get
            {
                return this.text;
            }
        }

        protected static string GetText(object attributeTarget, Type attributeType, string idioma)
        {
            object[] atts;

            // Método
            if (typeof (MethodInfo).IsAssignableFrom(attributeTarget.GetType()))
            {
                try
                {
                    atts = ((MethodInfo)attributeTarget).GetCustomAttributes(attributeType, true);
                }
                catch (Exception exc)
                {
                    throw new Exception("Problema com o atributo " + attributeType.Name + " no método " + BusinessMethod.GetMethodSignature((MethodInfo)attributeTarget) + ": " + exc.Message);
                }
            }

                // Página
            else if (typeof (Type).IsAssignableFrom(attributeTarget.GetType()))
            {
                try
                {
                    atts = ((Type)attributeTarget).GetCustomAttributes(attributeType, true);
                }
                catch (Exception exc)
                {
                    throw new Exception("Problema com o atributo " + attributeType.Name + " na página " + ((Type)attributeTarget).FullName + ": " + exc.Message);
                }
            }
            else
            {
                throw new NotImplementedException("Tipo não implementado em TextAttribute.GetText(): " + attributeTarget.GetType().FullName + ".");
            }

            // Procura o idioma correto dentre os atributos obtidos do tipo solicitado.
            foreach (TextAttribute att in atts)
            {
                if (att.Idioma == idioma)
                {
                    return att.Text;
                }
            }

            return null;
        }
    }
}