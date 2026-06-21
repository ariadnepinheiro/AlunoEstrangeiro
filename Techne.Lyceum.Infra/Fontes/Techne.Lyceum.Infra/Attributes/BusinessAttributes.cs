using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace Techne
{
    [
        AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false), 
        Description(
            "Indica que um assembly contém métodos de regras de negócio. " +
            "O designer busca os métodos de negócio somente nos assemblies com este atributo."
            ), 
    ]
    public class BusinessAssemblyAttribute : Attribute
    {
        private readonly string applicationName;

        private static Assembly[] businessAssemblies;

        static BusinessAssemblyAttribute()
        {
            MainAssemblyAttribute.Reset += MainAssemblyAttribute_Reset;
        }

        public BusinessAssemblyAttribute(string applicationName)
        {
            this.applicationName = applicationName;
        }

        internal static event EventHandler Reset;

        /// <summary>
        ///   Lista dos assemblies que contém o atributo BusinessAssemblyAttribute.
        ///   Esta lista é um subconjunto de MainAssemblyAttribute.ProjectAssemblies.
        /// </summary>
        public static Assembly[] BusinessAssemblies
        {
            get
            {
                if (businessAssemblies == null)
                {
                    try
                    {
                        var list = new ArrayList();
                        var assemblyList = new ArrayList();

                        foreach (var assembly in MainAssemblyAttribute.ProjectAssemblies)
                        {
                            if (assembly.IsDefined(typeof (BusinessAssemblyAttribute), false))
                            {
                                list.Add(assembly);
                                assemblyList.Add(assembly.GetName().Name);
                            }
                        }

                        businessAssemblies = (Assembly[])list.ToArray(typeof (Assembly));
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

                return businessAssemblies;
            }
        }

        public string ApplicationName
        {
            get
            {
                return this.applicationName;
            }
        }

        /// <summary>
        ///   Devolve a propriedade ApplicationName do atributo BusinessAssemblyAttribute do assembly informado.
        ///   Devolve null caso o atributo não exista no assembly.
        /// </summary>
        public static string GetApplicationName(Assembly assembly)
        {
            if (!assembly.IsDefined(typeof (BusinessAssemblyAttribute), false))
            {
                return null;
            }

            var attributes = assembly.GetCustomAttributes(typeof (BusinessAssemblyAttribute), false);
            if (attributes.Length > 1)
            {
                throw new InvalidOperationException(); // Não ocorrerá, pois AllowMultiple=false
            }

            var attribute = (BusinessAssemblyAttribute)attributes[0];
            return attribute.ApplicationName;
        }

        private static void MainAssemblyAttribute_Reset(object sender, EventArgs e)
        {
            businessAssemblies = null;
            if (Reset != null)
            {
                Reset(null, EventArgs.Empty);
            }
        }
    }

    [
        AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true), 
    ]
    public class ImageAttribute : Attribute
    {
        private readonly string url;

        public ImageAttribute(string url)
        {
            if (url == null || url.Trim().Length == 0)
            {
                throw new ArgumentException("A url é obrigatória.");
            }

            this.url = url;
        }

        public string Url
        {
            get
            {
                return this.url;
            }
        }

        /// <summary>
        ///   Devolve o atributo ImageAttribute associado ao método informado.
        ///   Devolve null se o atributo não for encontrado.
        /// </summary>
        internal static string GetUrl(MethodInfo method)
        {
            var attrs = (ImageAttribute[])method.GetCustomAttributes(typeof (ImageAttribute), true);
            return attrs.Length > 0 ? attrs[0].Url : null;
        }

        /// <summary>
        ///   Devolve o atributo ImageAttribute associado à página correspondente ao tipo informado.
        ///   Devolve null se o atributo não for encontrado.
        /// </summary>
        internal static string GetUrl(Type type)
        {
            var attrs = (ImageAttribute[])type.GetCustomAttributes(typeof (ImageAttribute), true);
            return attrs.Length > 0 ? attrs[0].Url : null;
        }
    }

    [
        AttributeUsage(AttributeTargets.Method, Inherited = true), 
    ]
    public class MethodDescriptionAttribute : TextAttribute
    {
        public MethodDescriptionAttribute(string idioma, string descricao) : base(idioma, descricao)
        {
        }

        public MethodDescriptionAttribute(string descricao) : base(descricao)
        {
        }

        public string Descricao
        {
            get
            {
                return base.Text;
            }
        }

        /// <summary>
        ///   Busca o texto associado ao atributo MethodDescription do método informado.
        ///   Devolve null se o atributo não existe.
        /// </summary>
        internal static string GetDescription(MethodInfo method, string idioma)
        {
            return TextAttribute.GetText(method, typeof (MethodDescriptionAttribute), idioma);
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = true), Description("Declara um texto a ser utilizado como tooltip nos controles associados ao m\x00e9todo de neg\x00f3cio no qual este atributo est\x00e1 declarado.")]
    public class ToolTipAttribute : TextAttribute
    {
        public ToolTipAttribute(string idioma, string texto) : base(idioma, texto)
        {
        }

        public ToolTipAttribute(string texto) : base(texto)
        {
        }

        public string Texto
        {
            get
            {
                return base.Text;
            }
        }

        /// <summary>
        ///   Busca o texto associado ao atributo ToolTip do método informado.
        ///   Devolve null se o atributo não existe.
        /// </summary>
        internal static string GetToolTip(MethodInfo method, string idioma)
        {
            return TextAttribute.GetText(method, typeof (ToolTipAttribute), idioma);
        }
    }
}