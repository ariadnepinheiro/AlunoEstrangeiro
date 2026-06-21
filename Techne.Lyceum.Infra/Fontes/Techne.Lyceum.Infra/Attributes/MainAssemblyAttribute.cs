using System;
using System.Collections;
using System.Reflection;

namespace Techne
{
    /// <summary>
    ///   Atributo que indica o projeto principal (web ou win).
    ///   Só é permitido um por aplicação.
    /// </summary>
    public class MainAssemblyAttribute : Attribute
    {
        private static Assembly mainAssembly;

        private static Assembly[] projectAssemblies;

        static MainAssemblyAttribute()
        {
            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;
        }

        internal static event EventHandler Reset;

        /// <summary>
        ///   Assembly do projeto principal (web ou win).
        ///   Deverá existir um, e somente um, projeto na aplicação com o atributo MainAssemblyAttribute,
        ///   caso contrário ocorrerá uma exception.
        /// </summary>
        public static Assembly MainAssembly
        {
            get
            {
                if (mainAssembly == null)
                {
                    try
                    {
                        // Assume-se que o assembly do projeto principal (web ou win) sempre estará em AppDomain.CurrentDomain.
                        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            if (assembly.IsDefined(typeof (MainAssemblyAttribute), false))
                            {
                                if (mainAssembly != null)
                                {
                                    var mainAssemblyName = mainAssembly.GetName();
                                    var assemblyName = assembly.GetName();

                                    if (string.Compare(mainAssemblyName.Name, assemblyName.Name, true) != 0)
                                    {
                                        throw new ApplicationException(
                                            "Existe pelo menos dois assemblies com o atributo " + typeof (MainAssemblyAttribute).FullName + ": " +
                                            mainAssemblyName.Name + " e " + assemblyName.Name + "."
                                            );
                                    }

                                    if (assemblyName.Version.CompareTo(mainAssemblyName.Version) <= 0)
                                    {
                                        continue;
                                    }
                                }

                                mainAssembly = assembly;
                            }
                        }

                        if (mainAssembly == null)
                        {
                            throw new ApplicationException("Não foi encontrado nenhum assembly com o atributo " + typeof (MainAssemblyAttribute).FullName + ".");
                        }
                    }
                    catch (Exception exc)
                    {
                        throw new Exception(exc.Message, exc);
                    }
                }

                return mainAssembly;
            }
        }

        /// <summary>
        ///   Lista dos assemblies que compõe a aplicação.
        ///   A lista é montada a partir do MainAssembly, incluindo recursivamente os assemblies referenciados.
        ///   Exclui os assemblies iniciados por strings listadas em reservedAssemblies (mscorlib, jvs, System e Microsoft).
        /// </summary>
        public static Assembly[] ProjectAssemblies
        {
            get
            {
                if (projectAssemblies == null)
                {
                    try
                    {
                        var list = new ArrayList();
                        list.Add(MainAssembly);
                        AddReferencedAssembliesRecursive(MainAssembly, list);
                        projectAssemblies = (Assembly[])list.ToArray(typeof (Assembly));

                        var nameList = new ArrayList();
                        foreach (var assembly in projectAssemblies)
                        {
                            nameList.Add(assembly.GetName().Name);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

                return projectAssemblies;
            }
        }

        /// <summary>
        ///   Utilizado por ProjectAssemblies.
        /// </summary>
        private static void AddReferencedAssembliesRecursive(Assembly assembly, IList assemblyList)
        {
            foreach (var referencedName in assembly.GetReferencedAssemblies())
            {
                if (StrLib.StartsWith(TechLib.ReservedAssemblies, referencedName.Name, true) < 0)
                {
                    Assembly referenced = null;
                    try
                    {
                        referenced = Assembly.Load(referencedName);
                    }
                    catch
                    {
                        referenced = null;
                    }

                    if (referenced != null && !assemblyList.Contains(referenced))
                    {
                        assemblyList.Add(referenced);
                        AddReferencedAssembliesRecursive(referenced, assemblyList);
                    }
                }
            }
        }

        private static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            ResetCacheIfNewer(args.LoadedAssembly);
        }

        /// <summary>
        ///   Verifica se o assembly informado é um MainAssembly mais novo que o do cache.
        ///   Reseta o cache se for.
        /// </summary>
        private static void ResetCacheIfNewer(Assembly loadedAssembly)
        {
            try
            {
                if (loadedAssembly == null)
                {
                    return;
                }

                // loadedAssembly não tem atributo MainAssembly
                if (!loadedAssembly.IsDefined(typeof (MainAssemblyAttribute), false))
                {
                    return;
                }

                // Cache está vazio. Não precisa resetá-lo.
                if (mainAssembly == null)
                {
                    return;
                }

                var loadedAssemblyVersion = loadedAssembly.GetName().Version;
                var mainAssemblyVersion = mainAssembly.GetName().Version;

                // loadedAssembly é igual ou mais antigo que o cache.
                if (loadedAssemblyVersion.CompareTo(mainAssemblyVersion) <= 0)
                {
                    return;
                }

                // --- RESETA O CACHE ---
                mainAssembly = null;
                projectAssemblies = null;
                if (Reset != null)
                {
                    Reset(null, EventArgs.Empty);
                }
            }
            finally
            {
            }
        }
    }
}