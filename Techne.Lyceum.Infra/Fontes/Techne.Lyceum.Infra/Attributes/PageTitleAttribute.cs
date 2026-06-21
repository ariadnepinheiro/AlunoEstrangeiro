using System;

namespace Techne
{
    [
        AttributeUsage(AttributeTargets.Class), 
    ]
    public class TitleAttribute : TextAttribute
    {
        /// <summary>
        ///   Define o title da página, que será utilizado como PageTitle.
        /// </summary>
        public TitleAttribute(string idioma, string titulo) : base(idioma, titulo)
        {
        }

        /// <summary>
        ///   Define o title da página, que será utilizado como PageTitle.
        ///   Utiliza TPage.IDIOMADEFAULT como idioma.
        /// </summary>
        public TitleAttribute(string titulo) : base(titulo)
        {
        }

        public string Titulo
        {
            get
            {
                return base.Text;
            }
        }

        /// <summary>
        ///   Devolve o valor do atributo Title no idioma fornecido.
        ///   Devolve null se Title não foi definido na classe.
        /// </summary>
        public static string GetPageTitle(Type type, string idioma)
        {
            return TextAttribute.GetText(type, typeof (TitleAttribute), idioma);
        }
    }
}