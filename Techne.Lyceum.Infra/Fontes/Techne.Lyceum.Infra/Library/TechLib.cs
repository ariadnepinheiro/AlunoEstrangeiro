using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Techne
{
    public delegate string ObjectToStringDelegate(object obj);

    public delegate string StringToStringDelegate(string s);

    public delegate object ObjectToObjectDelegate(object obj);

    internal delegate void MessageDelegate(string message, bool warning);

    public class TechLib
    {
        public static string[] ReservedAssemblies; // inicializado no construtor estático

        static TechLib()
        {
            // Inicializa ReservedAssemblies
            var reservedAssemblies = ConfigurationSettings.AppSettings["Techne.TechLib.ReservedAssemblies"];
            ReservedAssemblies = reservedAssemblies != null ? reservedAssemblies.Split(',') : new string[0];
            for (var i = 0; i < ReservedAssemblies.Length; i++)
            {
                ReservedAssemblies[i] = ReservedAssemblies[i].Trim();
            }
        }

        /// <summary>
        ///   Devolve o primeiro inteiro maior ou igual ao decimal informado.
        /// </summary>
        public static int Ceiling(decimal number)
        {
            if (number - decimal.Truncate(number) == 0)
            {
                return (int)number;
            }
            else
            {
                return (int)decimal.Truncate(number) + 1;
            }
        }

        /// <summary>
        ///   Cria uma cópia do array fornecido, em ordem inversa.
        ///   Utiliza o método Clone() e o método estático Reverse() de Array.
        /// </summary>
        public static Array CloneReverse(Array array)
        {
            if (array == null)
            {
                throw new ArgumentNullException();
            }

            var reverse = (Array)array.Clone();
            Array.Reverse(reverse);
            return reverse;
        }

        /// <summary>
        ///   Dada uma classe, verifica se existe uma customização para ela. Caso exista, instancia a dll
        ///   customizada (que deve ser derivada da classe fornecida). Caso não exista, a exception
        ///   FileNotFoundException ocorrerá.
        /// </summary>
        public static object CreateCustomizableClass(Type baseType)
        {
            return CreateCustomizableClass(baseType, true);
        }

        /// <summary>
        ///   Dada uma classe, verifica se existe uma customização para ela. Caso exista, instancia a dll
        ///   customizada (que deve ser derivada da classe fornecida). Caso não exista, o parâmetro
        ///   exceptionOnCustomNotFound definirá qual será o comportamento do método.
        /// </summary>
        /// <param name = "baseType">O tipo que deve ser instanciado, passível de customização.</param>
        /// <param name = "exceptionOnCustomNotFound">
        ///   Comportamento do método quando a dll que contém a customização não for encontrada:
        ///   se true, causa FileNotFoundException;
        ///   se false, devolve nova instância do tipo informado (não customizado).
        /// </param>
        /// <returns></returns>
        public static object CreateCustomizableClass(Type baseType, bool exceptionOnCustomNotFound)
        {
            var baseDll = baseType.Assembly.Location;
            string dllName, customDll, customType;
            {
                var currentPath = Path.GetDirectoryName(baseDll);
                var currentFile = Path.GetFileNameWithoutExtension(baseDll);
                dllName = currentFile + "_Custom.dll";
                customDll = Path.Combine(currentPath, dllName);
                customType = baseType.FullName + "Custom";
            }

            try
            {
                var custom = Assembly.LoadFile(customDll);
                var type = custom.GetType(customType);
                if (!baseType.IsAssignableFrom(type))
                {
                    throw new NotAssignableException(string.Format("A classe {0} encontrada na dll customizada {1} não é derivada da classe {2}.", customType, dllName, baseType.FullName));
                }

                var ctor = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
                return ctor.Invoke(new object[0]);
            }
            catch (NotAssignableException)
            {
                throw;
            }
            catch (FileNotFoundException)
            {
                if (exceptionOnCustomNotFound)
                {
                    throw new FileNotFoundException("A dll de customizações não foi encontrada.", customDll);
                }
            }
            catch
            {
            }

            var baseCtor = baseType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
            return baseCtor.Invoke(new object[0]);
        }

        public static object Deserialize(byte[] serialized)
        {
            var stream = new MemoryStream(serialized);
            IFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(stream);
        }

        /// <summary>
        ///   Cria um array com os valores do indexador de cada item do array dado.
        ///   Devolve object[0] se o array informado estiver vazio.
        /// </summary>
        /// <param name = "indexValue">Valor do indexador.</param>
        public static Array EnumerableItemIndexer(IEnumerable array, string indexValue)
        {
            return EnumerableItemIndexer(array, indexValue, true);
        }

        public static Array EnumerableItemIndexer(IEnumerable array, string indexValue, bool forceSameType)
        {
            return EnumerableItem(array, EnumerableGetIndexer, indexValue, forceSameType);
        }

        /// <summary>
        ///   Cria um array com os valores do método informado de cada item do array dado.
        ///   Devolve object[0] se o array informado estiver vazio.
        /// </summary>
        /// <param name = "methodName">Nome do método do item do array cujo valor deseja-se obter.</param>
        public static Array EnumerableItemMethod(IEnumerable array, string methodName)
        {
            return EnumerableItemMethod(array, methodName, true);
        }

        public static Array EnumerableItemMethod(IEnumerable array, string methodName, bool forceSameType)
        {
            return EnumerableItem(array, EnumerableGetMethod, methodName, forceSameType);
        }

        /// <summary>
        ///   Cria um array com os valores da propriedade informada de cada item do array dado.
        ///   Devolve object[0] se o array informado estiver vazio.
        /// </summary>
        /// <param name = "propertyName">Nome da propriedade do item do array cujo valor deseja-se obter.</param>
        public static Array EnumerableItemProperty(IEnumerable array, string propertyName)
        {
            return EnumerableItemProperty(array, propertyName, true);
        }

        public static Array EnumerableItemProperty(IEnumerable array, string propertyName, bool forceSameType)
        {
            return EnumerableItem(array, EnumerableGetProperty, propertyName, forceSameType);
        }

        public static Array EnumerableTransform(IEnumerable array, ObjectToObjectDelegate function)
        {
            return EnumerableTransform(array, function, typeof (object));
        }

        /// <summary>
        ///   Transforma cada elemento do enumerado fornecido, devolvendo um Array.
        /// </summary>
        /// <param name = "function">Função de transformação dos itens do array de origem</param>
        /// <param name = "type">Tipo dos elementos no array devolvido</param>
        public static Array EnumerableTransform(IEnumerable array, ObjectToObjectDelegate function, Type type)
        {
            var list = new ArrayList();
            foreach (var item in array)
            {
                list.Add(function(item));
            }

            return list.ToArray(type);
        }

        /// <summary>
        ///   Procura recursivamente a partir de um controle todos os controles de um tipo ou derivados desse tipo.
        /// </summary>
        /// <param name = "controlType">Tipo que será procurado. Os controles derivados desse tipo também farão parte do resultado.</param>
        /// <param name = "scopeControl">Controle a partir do qual a procura será feita (raiz da árvore de controles).</param>
        /// <param name = "inclusive">Indica se próprio controle passado em scopeControl será incluído na lista caso seu tipo satisfaça a condição.</param>
        /// <param name = "excludeSearchInTypes">Tipos dos controles dentro dos quais a procura não será feita.</param>
        public static Array FindControls(Type controlType, Control scopeControl, bool inclusive, Type[] excludeSearchInTypes)
        {
            var list = new ArrayList();
            searchControl(controlType, scopeControl, list, inclusive, excludeSearchInTypes);
            return list.ToArray(controlType);
        }

        /// <summary>
        ///   Procura recursivamente a partir de um controle todos os controles de um tipo ou derivados desse tipo.
        /// </summary>
        public static Array FindControls(Type controlType, Control controlScope)
        {
            return FindControls(controlType, controlScope, true, new Type[0]);
        }

        public static HtmlForm FindForm(Page page)
        {
            if (page == null)
            {
                return null;
            }

            var getForm = typeof (Page).GetProperty("Form", BindingFlags.NonPublic | BindingFlags.Instance);
            if (getForm == null)
            {
                // Se por acaso algum dia a propriedade Page.Form (que é internal) deixar de existir, vai pelo...
                // Método tradicional.
                return FindForm((Control)page);
            }
            else
            {
                // Via reflection.
                return getForm.GetValue(page, null) as HtmlForm;
            }
        }

        /// <summary>
        ///   Procura um tipo dentro de um assembly.
        ///   A busca é recursiva, ou seja, os assembly referenciados também estão no escopo da busca.
        ///   Os assemblies System* e Microsoft* não estão no escopo.
        /// </summary>
        public static Type FindType(string fullName, Assembly assembly)
        {
            var result = assembly.GetType(fullName);

            if (result == null)
            {
                foreach (var assemblyName in assembly.GetReferencedAssemblies())
                {
                    if (StrLib.StartsWith(ReservedAssemblies, assemblyName.Name, true) < 0)
                    {
                        result = FindType(fullName, Assembly.Load(assemblyName));
                        if (result != null)
                        {
                            break;
                        }
                    }
                }
            }

            return result;
        }

        public static MethodInfo GetImplicitConverter(Type fromType, Type toType)
        {
            foreach (MethodInfo m in toType.GetMember("op_Implicit", MemberTypes.Method, BindingFlags.Static | BindingFlags.Public))
            {
                if (m.ReturnType == toType)
                {
                    foreach (var p in m.GetParameters())
                    {
                        if (p.ParameterType == fromType)
                        {
                            return m;
                        }
                    }
                }
            }

            return null;
        }

        public static Type[] GetImplicitConvertersToType(Type t)
        {
            var l = new ArrayList();

            foreach (MethodInfo m in t.GetMember("op_Implicit", MemberTypes.Method, BindingFlags.Static | BindingFlags.Public))
            {
                if (m.ReturnType == t)
                {
                    var p = m.GetParameters();
                    if (p.Length == 1)
                    {
                        l.Add(p[0].ParameterType);
                    }
                }
            }

            return (Type[])l.ToArray(typeof (Type));
        }

        public static Type[] GetSubtypes(Type findType, Assembly assembly, bool recursive)
        {
            return GetSubtypes(findType, assembly, recursive, true);
        }

        /// <summary>
        ///   Obtém todos os tipos derivados de um determinado tipo dentro de um assembly.
        ///   Se a busca for recursiva, todos os assemblies referenciados também estarão no escopo.
        ///   A busca também incluirá o próprio tipo, se encontrado.
        /// </summary>
        public static Type[] GetSubtypes(Type findType, Assembly assembly, bool recursive, bool includeFindType)
        {
            var list = new ArrayList();
            pvGetSubtypes(findType, assembly, list, recursive, includeFindType);
            return (Type[])list.ToArray(typeof (Type));
        }

        /// <summary>
        ///   Busca um item dentro de um IList usando o IComparer fornecido.
        /// </summary>
        public static int IndexOf(object item, IList list, IComparer comparer)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (comparer.Compare(list[i], item) == 0)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        ///   Faz o mesmo papel de "is" em "obj is type".
        ///   Verifica se um objeto é do tipo informado ou de um tipo derivado dele.
        /// </summary>
        public static bool ObjectOfType(object obj, Type type)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            var objectType = obj.GetType();
            if ((objectType != type) && !objectType.IsSubclassOf(type))
            {
                return type.IsInstanceOfType(obj);
            }

            return true;
        }

        public static bool ObjectOfType(object obj, Type[] types)
        {
            if (types == null)
            {
                throw new ArgumentNullException("types");
            }

            foreach (var type in types)
            {
                if (ObjectOfType(obj, type))
                {
                    return true;
                }
            }

            return false;
        }

        public static byte[] Serialize(object obj)
        {
            var stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            return stream.ToArray();
        }

        private static object EnumerableGetIndexer(object item, string indexValue)
        {
            var m = item.GetType().GetProperty("Item", new[] { typeof (string) }).GetGetMethod();

            try
            {
                return m.Invoke(item, new object[] { indexValue });
            }
            catch (TargetInvocationException exc)
            {
                throw exc.InnerException;
            }
        }

        private static object EnumerableGetMethod(object item, string methodName)
        {
            var type = item.GetType();
            var method = type.GetMethod(methodName);
            return method.Invoke(item, new object[0]);
        }

        private static object EnumerableGetProperty(object item, string propertyName)
        {
            return item.GetType().GetProperty(propertyName).GetGetMethod().Invoke(item, new object[0]);
        }

        private static Array EnumerableItem(IEnumerable array, EnumerableGetValueFunction function, string aux, bool forceSameType)
        {
            var result = new ArrayList();
            var type = forceSameType ? null : typeof (object);

            foreach (var item in array)
            {
                var propertyValue = function(item, aux);

                if (forceSameType)
                {
                    if (type == null)
                    {
                        type = propertyValue == null ? null : propertyValue.GetType();
                    }
                    else if (propertyValue.GetType() != type)
                    {
                        throw new ArgumentException("As propriedades dos itens devem ser todas do mesmo tipo.");
                    }
                }

                result.Add(propertyValue);
            }

            if (type == null)
            {
                // Fornecido array vazio (nem entrou no loop)
                return new object[0];
            }
            else
            {
                return result.ToArray(type);
            }
        }

        private static HtmlForm FindForm(Control root)
        {
            if (root is HtmlForm)
            {
                return (HtmlForm)root;
            }
            else if (root != null)
            {
                foreach (Control child in root.Controls)
                {
                    var form = FindForm(child);
                    if (form != null)
                    {
                        return form;
                    }
                }
            }

            return null;
        }

        private static void pvGetSubtypes(Type findType, Assembly assembly, ArrayList list, bool recursive, bool includeFindType)
        {
            // Tipos definidos no assembly.
            foreach (var type in assembly.GetTypes())
            {
                if (type == findType && includeFindType || type.IsSubclassOf(findType))
                {
                    if (!list.Contains(type))
                    {
                        list.Add(type);
                    }
                }
            }

            if (recursive)
            {
                // Tipos definidos nos assemblies referenciados.
                foreach (var assemblyName in assembly.GetReferencedAssemblies())
                {
                    var refName = assemblyName.Name;

                    if (StrLib.StartsWith(ReservedAssemblies, refName, true) < 0)
                    {
                        pvGetSubtypes(findType, Assembly.Load(assemblyName), list, recursive, includeFindType);
                    }
                }
            }
        }

        /// <param name = "searchType">Tipo dos controles sendo procurados.</param>
        /// <param name = "scopeControl">Escopo de busca (raiz da árvore de controles).</param>
        /// <param name = "list">Lista contendo os controles já encontrados. Nela serão inseridos os novos controles encontrados.</param>
        /// <param name = "inclusive">Indica que o controle passado como parâmetro também será verificado e adicionado à lista se for o caso.</param>
        /// <param name = "excludeSearchInTypes">Indica os tipos dentro dos quais não ocorre busca.</param>
        private static void searchControl(Type searchType, 
                                          Control scopeControl, 
                                          ArrayList list, 
                                          bool inclusive, 
                                          Type[] excludeSearchInTypes)
        {
            if (scopeControl == null)
            {
                return;
            }

            scopeControl.GetType();
            var isType = ObjectOfType(scopeControl, searchType);

            if (isType && inclusive)
            {
                list.Add(scopeControl);
            }

            var isExcludedType = ObjectOfType(scopeControl, excludeSearchInTypes);

            if (!isExcludedType)
            {
                foreach (Control control in scopeControl.Controls)
                {
                    searchControl(searchType, control, list, true, excludeSearchInTypes);
                }
            }
        }

        private delegate object EnumerableGetValueFunction(object item, string aux);
    }

    public class NotAssignableException : Exception
    {
        public NotAssignableException()
        {
        }

        public NotAssignableException(string message) : base(message)
        {
        }
    }
}