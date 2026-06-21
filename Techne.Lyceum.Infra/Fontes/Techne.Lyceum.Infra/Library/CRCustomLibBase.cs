using System;
using System.Configuration;
using System.Reflection;

namespace Techne
{
    public abstract class CRCustomLibBase
    {
        private const string CustomAssemblyPropertyName = "CustomAssembly";

        private const string CustomNamespacePropertyName = "CustomNamespace";

        private static string customNamespace;

        private static string CustomNamespace
        {
            get
            {
                if (customNamespace == null)
                {
                    customNamespace = ConfigurationSettings.AppSettings[CustomNamespacePropertyName];
                    if (customNamespace == null)
                    {
                        throw new InvalidOperationException("CustomDatasetNamespace não foi informado no arquivo de configuração.");
                    }
                }

                return customNamespace;
            }
        }

        protected static Assembly GetCustomAssembly(string applicationName)
        {
            var assemblyName = ConfigurationSettings.AppSettings[CustomAssemblyPropertyName + "(" + applicationName + ")"];
            if (assemblyName == null)
            {
                return null;
            }

            return Assembly.Load(assemblyName);
        }

        protected static ConstructorInfo GetCustomConstructor(string cronosDatasetName, string applicationName, 
                                                              Assembly cronosAssembly, Assembly customAssembly)
        {
            if (customAssembly == null)
            {
                return null;
            }

            var customType = customAssembly.GetType(CustomNamespace + "." + applicationName + "." + cronosDatasetName + "Custom");
            if (customType == null)
            {
                return null;
            }

            var typeCustomBase = cronosAssembly.GetType("Techne." + applicationName + ".CR." + cronosDatasetName + "+CustomBase");
            if (!customType.IsSubclassOf(typeCustomBase))
            {
                return null;
            }

            return customType.GetConstructor(Type.EmptyTypes);
        }

        protected static Assembly GetTCustomAssembly(string applicationName)
        {
            var businessAssemblies = BusinessAssemblyAttribute.BusinessAssemblies;
            if (businessAssemblies.Length == 0)
            {
                return null;
            }

            Assembly tcustomAssembly = null;

            foreach (var businessAssembly in businessAssemblies)
            {
                var attributes = businessAssembly.GetCustomAttributes(typeof (BusinessAssemblyAttribute), false);
                if (attributes.Length != 1)
                {
                    throw new InvalidOperationException("Mais de um atributo BusinessAssembly no assembly " + businessAssembly.GetName().FullName + ".");
                }

                if (((BusinessAssemblyAttribute)attributes[0]).ApplicationName == applicationName)
                {
                    if (tcustomAssembly != null)
                    {
                        throw new InvalidOperationException("Existe mais de um BusinessAssembly para a aplicação " + applicationName + ".");
                    }

                    tcustomAssembly = businessAssembly;
                }
            }

            return tcustomAssembly;
        }

        protected static ConstructorInfo GetTCustomConstructor(string cronosDatasetName, string applicationName, 
                                                               Assembly cronosAssembly, Assembly tcustomAssembly)
        {
            if (tcustomAssembly == null)
            {
                return null;
            }

            Type tcustomType = null;
            try
            {
                tcustomType = tcustomAssembly.GetType("Techne." + applicationName + ".RN.Custom." + cronosDatasetName + "Custom");
            }
            catch
            {
            }

            if (tcustomType == null)
            {
                return null;
            }

            Type typeCustomBase = null;
            try
            {
                typeCustomBase = cronosAssembly.GetType("Techne." + applicationName + ".CR." + cronosDatasetName + "+CustomBase");
            }
            catch
            {
            }

            if (!tcustomType.IsSubclassOf(typeCustomBase))
            {
                return null;
            }

            return tcustomType.GetConstructor(Type.EmptyTypes);
        }
    }
}