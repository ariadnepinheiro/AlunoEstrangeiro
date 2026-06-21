using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI;
using Techne.Controls;
using Techne.Library;

namespace Techne.Web
{
    /// <summary>
    ///   Classe que guarda informações para navegação a uma determinada página.
    ///   Obtém-se uma instância através do método estático GetNavigation().
    /// </summary>
    public class Navigation
    {
        public static readonly string[] ReservedParameterNames = new[] { "ReturnUrl", "manager" };

        private readonly ParameterCollection fixedParameters;

        private readonly string methodName;

        /// <summary>
        ///   Indica se o método de navegação é de uma classe derivada de NavegaBase.
        ///   Utilizado para decidir se mostra ou não o nome do método (métodos GetUrl() não são mostrados).
        /// </summary>
        private readonly bool navigationClass;

        private readonly string page;

        private readonly Type pageType;

        private readonly string[] paramNames;

        private readonly Type[] paramTypes;

        private readonly bool returnEnabled;

        private static IDictionary navigationMethods;

        static Navigation()
        {
            MainAssemblyAttribute.Reset += MainAssemblyAttribute_Reset;
        }

        private Navigation(MethodInfo method)
        {
            this.methodName = method.Name;
            this.pageType = method.DeclaringType;

            

            NavUrlAttribute attrUrl;
            {
                // Busca o atributo NavUrl no método.
                var attrsUrl = (NavUrlAttribute[])method.GetCustomAttributes(typeof (NavUrlAttribute), false);

                // Se não encontrou no método, procura na classe.
                if (attrsUrl.Length == 0)
                {
                    attrsUrl = (NavUrlAttribute[])method.DeclaringType.GetCustomAttributes(typeof (NavUrlAttribute), false);
                    if (attrsUrl.Length == 0)
                    {
                        throw new InvalidOperationException("Não existe atributo NavUrl na classe " + method.DeclaringType.FullName + ".");
                    }
                }

                attrUrl = attrsUrl[0];
            }

            

            this.page = attrUrl.Url;

            #region NavReturnAttribute attrReturn = ...;

            NavReturnAttribute attrReturn = null;
            {
                var attrsReturn = (NavReturnAttribute[])method.GetCustomAttributes(typeof (NavReturnAttribute), false);
                if (attrsReturn.Length == 1)
                {
                    attrReturn = attrsReturn[0];
                }
            }

            #endregion

            this.returnEnabled = attrReturn != null && attrReturn.Return;
            {
                #region this.paramNames = ...; this.paramTypes = ...;

                    var parameters = method.GetParameters();
                this.paramNames = new string[parameters.Length];
                this.paramTypes = new Type[parameters.Length];
                for (var i = 0; i < parameters.Length; i++)
                {
                    this.paramNames[i] = parameters[i].Name;
                    this.paramTypes[i] = parameters[i].ParameterType;
                }

                #endregion
            }

            #region this.fixedParameters = ...;

            this.fixedParameters = new ParameterCollection();
            {
                NavParameterAttribute[] attrsParameter;
                attrsParameter = (NavParameterAttribute[])method.GetCustomAttributes(typeof (NavParameterAttribute), false);
                foreach (var attrParameter in attrsParameter)
                {
                    this.fixedParameters.Add(attrParameter.Parameter, attrParameter.Value);
                }
            }

            #endregion

            this.navigationClass = string.Compare(method.Name, "GetUrl") == 0;
        }

        internal static event EventHandler NavigationMethodsReset;

        public static IDictionary NavigationMethods
        {
            get
            {
                if (navigationMethods == null)
                {
                    try
                    {
                        navigationMethods = new Hashtable();
                        foreach (var type in MainAssemblyAttribute.MainAssembly.GetTypes())
                        {
                            if (typeof (TPage).IsAssignableFrom(type))
                            {
                                foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
                                {
                                    if (VerifyNavigationMethod(method).Length == 0)
                                    {
                                        navigationMethods.Add(GetNavigationMethodSignature(method), method);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        navigationMethods = null;
                    }
                }

                return navigationMethods;
            }
        }

        /// <summary>
        ///   Parâmetros adicionais (constantes) passados à página destino.
        /// </summary>
        public ParameterCollection FixedParameters
        {
            get
            {
                return this.fixedParameters;
            }
        }

        public string MethodName
        {
            get
            {
                return this.methodName;
            }
        }

        /// <summary>
        ///   Nome e path da página aspx para a qual a navegação será feita.
        /// </summary>
        public string Page
        {
            get
            {
                return this.page;
            }
        }

        /// <summary>
        ///   Devolve a classe da página para a qual a navegação será feita.
        /// </summary>
        public Type PageType
        {
            get
            {
                return this.pageType;
            }
        }

        /// <summary>
        ///   Array de strings contendo o nome dos parâmetros recebidos pela página aspx
        ///   especificada pela propriedade Page.
        /// </summary>
        public string[] ParameterNames
        {
            get
            {
                return this.paramNames;
            }
        }

        /// <summary>
        ///   Array de Type's contendo os tipos dos parâmetros recebidos pela página aspx
        ///   especificada pela propriedade Page.
        /// </summary>
        public Type[] ParameterTypes
        {
            get
            {
                return this.paramTypes;
            }
        }

        /// <summary>
        ///   Indica se a navegação permitirá volta à página chamadora.
        /// </summary>
        public bool ReturnEnabled
        {
            get
            {
                return this.returnEnabled;
            }
        }

        /// <summary>
        ///   Dada uma string no formato [class]([type1] [name2], [type2] [name2],...
        ///   devolve o método correspondente.
        ///   Nunca devolve null.
        /// </summary>
        public static Navigation Create(string navigationString)
        {
            if (navigationString == null || navigationString.Trim().Length == 0)
            {
                return null;
            }

            string[] parameterNames;
            {
                var p = navigationString.IndexOf('(');
                if (p >= 0)
                {
                    var q = navigationString.IndexOf(')', p);
                    if (q < 0)
                    {
                        throw new ArgumentException("Não foi encontrado ')'");
                    }

                    parameterNames = StrLib.Split(navigationString.Substring(p + 1, q - p - 1), ',');
                }
                else
                {
                    parameterNames = new string[0];
                }
            }

            var type = GetType(navigationString);
            var methodInfo = Navigation.GetMethod(type, parameterNames);
            return new Navigation(methodInfo);
        }

        /// <summary>
        ///   Dado um método, devolve uma instância de Navigation.
        ///   Nunca devolve null.
        /// </summary>
        public static Navigation GetNavigation(MethodBase method)
        {
            if (!(method is MethodInfo))
            {
                throw new ArgumentException("O argumento deve ser do tipo " + typeof (MethodInfo).FullName, "method");
            }

            try
            {
                return new Navigation((MethodInfo)method);
            }
            catch (Exception exc)
            {
                throw new Exception("Não foi possível obter informações de navegação para o método informado", exc);
            }
        }

        public override string ToString()
        {
            var list = new string[this.paramNames.Length];

            for (var i = 0; i < this.paramNames.Length; i++)
            {
                list[i] = this.paramNames[i];
            }

            return this.pageType.FullName +
                   (this.navigationClass ? string.Empty : "." + this.MethodName) +
                   "(" + StrLib.EnumerableToStr(list, ", ") + ")";
        }

        public string GetUrl()
        {
            return TUtil.TranslateRelativeUrl(this.Page);
        }

        /// <summary>
        ///   Url para navegação à página especificada pela propriedade Page, passando os parâmetros
        ///   indicados pelo array fornecido. A quantidade de parâmetros no array fornecido deve coincidir
        ///   com a quantidade informada pelas propriedades ParameterNames e ParameterTypes.
        /// </summary>
        public string GetUrl(object[] parameterValues)
        {
            if (parameterValues.Length != this.paramNames.Length)
            {
                throw new ArgumentException("O número de valores difere do número de parâmetros");
            }

            var parameters = new string[this.paramNames.Length];
            for (var i = 0; i < this.paramNames.Length; i++)
            {
                string s;
                if (parameterValues[i] == null)
                {
                    s = TDropDownListBase.SelectAllValue;
                }
                else if (parameterValues[i] is DBNull)
                {
                    s = string.Empty;
                }

                    // Este tratamento específico para IDbObject é provisório.
                    // O correto seria que Convert.ToString() funcionasse, mas o parâmetro
                    // InvariantCulture está sendo ignorado. Será necessário implementar
                    // IConvertible no tipos derivados de IDbObject?
                else if (parameterValues[i] is IDbObject)
                {
                    var dbo = (IDbObject)parameterValues[i];
                    if (dbo.IsNull)
                    {
                        s = string.Empty;
                    }
                    else
                    {
                        s = HttpUtility.UrlEncode(dbo.ToString(CultureInfo.InvariantCulture));
                    }
                }
                else
                {
                    s = HttpUtility.UrlEncode(Convert.ToString(parameterValues[i], CultureInfo.InvariantCulture));
                }

                parameters[i] = this.paramNames[i] + "=" + s;
            }

            // Adiciona parâmetros extras, não presentes no método
            var listParameters = new ArrayList(parameters);

            // fixedParameters
            for (var i = 0; i < this.fixedParameters.Count; i++)
            {
                listParameters.Add(this.fixedParameters.GetKey(i) + "=" + HttpUtility.UrlEncode(this.fixedParameters.GetValue(i)));
            }

            // returnUrl
            if (this.returnEnabled)
            {
                listParameters.Add("returnUrl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl));
            }

            return this.GetUrl() + (parameters.Length == 0 ? string.Empty : "?" + StrLib.EnumerableToStr(listParameters, "&"));
        }

        internal static object ConvertToType(string str, Type type)
        {
            if (str == null)
            {
                throw new ArgumentNullException();
            }

            if (type.IsEnum)
            {
                return Enum.Parse(type, str);
            }
            else if (str.Length == 0)
            {
                if (typeof (IDbObject).IsAssignableFrom(type))
                {
                    return (DbObject)DBNull.Value;
                }
                else
                {
                    return DBNull.Value;
                }
            }
            else if (typeof (IDbObject).IsAssignableFrom(type))
            {
                return DbObject.FromString(str, DbObject.FromType(type), CultureInfo.InvariantCulture);
            }
            else
            {
                return Convert.ChangeType(str, type, CultureInfo.InvariantCulture);
            }
        }

        internal static string FromBase64String(string b64Method)
        {
            var reader = new BinaryReader(new MemoryStream(Convert.FromBase64String(b64Method)));
            var method = reader.ReadString();
            reader.Close();

            return method;
        }

        /// <summary>
        ///   Dada uma página, devolve collection do tipo NameObjectCollection que contém
        ///   os parâmetros e valores (já tipados) passados a ela.
        /// </summary>
        internal static void GetPageParameters(Page page, 
                                               out NameObjectCollection allParameters, 
                                               out NameDbObjectCollection parameters, 
                                               out Type[] types)
        {
            // O C# permite criar dois parâmetros iguais só com os cases diferentes.
            // Entretanto, estamos criando as collections como case insensitive para facilitar o programador.
            // Se ele criar dois parâmetros iguais e cases diferentes ocorrerá erro. Mas esta é uma situação
            // em que talvez seja melhor mudar os nomes dos parâmetros...
            allParameters = new NameObjectCollection(false);
            parameters = new NameDbObjectCollection(false);
            var listType = new ArrayList();

            

            object[] parameterValues = null;
            foreach (var method in page.GetType().GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
            {
                if (method.Name == "GetUrl" && method.ReturnType == typeof (string))
                {
                    var methodParams = method.GetParameters();

                    // Conta o número de parâmetros na query string, ignorando os parâmetros reservados (ReservedParameterNames).
                    var queryStrParamCount = page.Request.QueryString.Count;
                    foreach (var reservedParam in ReservedParameterNames)
                    {
                        if (StrLib.IndexOfInsensitive(reservedParam, page.Request.QueryString.AllKeys) >= 0)
                        {
                            queryStrParamCount--;
                        }
                    }

                    // Quantidade de parâmetros do overload = quantidade de parâmetros na query string
                    if (methodParams.Length == queryStrParamCount)
                    {
                        var failed = false;
                        parameterValues = new object[methodParams.Length];

                        // Percorre todos os parâmetros do overload para ver se eles existem na query string
                        for (var i = 0; i < methodParams.Length; i++)
                        {
                            // Verifica se o parâmetro existe na query string
                            if (Array.IndexOf(page.Request.QueryString.AllKeys, methodParams[i].Name) < 0)
                            {
                                failed = true;
                                parameterValues = null;
                                break;
                            }

                            // Aproveita para pegar o valor do parâmetro na query string (já tipado)
                            try
                            {
                                var strValue = page.Request.QueryString[methodParams[i].Name];
                                if (strValue == TDropDownListBase.SelectAllValue)
                                {
                                    // Trata caso específico do valor "TODOS" em TDropDownList.
                                    parameterValues[i] = null;
                                }
                                else
                                {
                                    parameterValues[i] = ConvertToType(strValue, methodParams[i].ParameterType);
                                }
                            }
                            catch (FormatException)
                            {
                                throw new FormatException("Não foi possível converter o valor do parâmetro " + methodParams[i].Name + " (" + page.Request.QueryString[methodParams[i].Name] + ") para o tipo " + methodParams[i].ParameterType.FullName);
                            }
                        }

                        // Todos os parâmetros foram encontrados na query string
                        if (!failed)
                        {
                            for (var i = 0; i < methodParams.Length; i++)
                            {
                                allParameters.Add(methodParams[i].Name, parameterValues[i]);
                                if (parameterValues[i] == null)
                                {
                                    // null é o caso específico do TDropDownList
                                    parameters.Add(methodParams[i].Name, null);
                                }
                                else if (parameterValues[i] is IDbObject)
                                {
                                    parameters.Add(methodParams[i].Name, DbObject.ToDbObject(((IDbObject)parameterValues[i]).ToObject()));
                                }
                                else if (parameterValues[i] is DBNull)
                                {
                                    parameters.Add(methodParams[i].Name, DBNull.Value);
                                }

                                listType.Add(methodParams[i].ParameterType);
                            }

                            break;
                        }
                    }
                }
            }

            

            types = (Type[])listType.ToArray(typeof (Type));
        }

        internal static Type GetType(string navigationString)
        {
            var typeName = GetTypeName(navigationString);
            var assembly = MainAssemblyAttribute.MainAssembly;
            var type = assembly.GetType(typeName);
            if (type == null)
            {
                throw new ArgumentException("O tipo " + typeName + " não foi encontrado no assembly " + assembly.GetName().Name + ".");
            }

            return type;
        }

        internal string ToBase64String()
        {
            var stream = new MemoryStream();
            var bwriter = new BinaryWriter(stream);

            bwriter.Write(this.ToString());

            var str = HttpUtility.UrlEncode(Convert.ToBase64String(stream.ToArray()));

            bwriter.Close();
            stream.Close();

            return str;
        }

        /// <summary>
        ///   Dada uma lista de parâmetros (nomes), obtém o overload de GetUrl() que a contém.
        ///   Nunca devolve null.
        /// </summary>
        private static MethodInfo GetMethod(Type typePage, string[] parameterNames)
        {
            return GetMethod(typePage, "GetUrl", typeof (string), parameterNames);
        }

        /// <summary>
        ///   Dada o nome do método e uma lista de parâmetros (nomes), obtém MethodInfo correspondente.
        ///   Nunca devolve null.
        /// </summary>
        private static MethodInfo GetMethod(Type typePage, string methodName, Type returnType, string[] parameterNames)
        {
            var exceptionMessage = "Foi encontrado mais de um método de nome '" + methodName + "' na classe " + typePage.FullName + ".";
            MethodInfo result = null;

            foreach (var method in typePage.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
            {
                if (method.Name.CompareTo(methodName) == 0 && method.ReturnType == returnType)
                {
                    if (parameterNames == null)
                    {
                        if (result != null)
                        {
                            throw new InvalidOperationException(exceptionMessage);
                        }

                        result = method;
                    }
                    else
                    {
                        var parameters = method.GetParameters();

                        // Quantidade de parâmetros do overload = quantidade de parâmetros informados
                        if (parameters.Length == parameterNames.Length)
                        {
                            var failed = false;

                            // Percorre todos os parâmetros do overload para ver se eles existem na query string
                            for (var i = 0; i < parameters.Length; i++)
                            {
                                // Verifica se o parâmetro existe na query string
                                if (Array.IndexOf(parameterNames, parameters[i].Name) < 0)
                                {
                                    failed = true;
                                    break;
                                }
                            }

                            // Todos os parâmetros foram encontrados na query string
                            if (!failed)
                            {
                                if (result != null)
                                {
                                    throw new InvalidOperationException(exceptionMessage);
                                }

                                result = method;
                            }
                        }
                    }
                }
            }

            if (result == null)
            {
                throw new InvalidOperationException("O método " + methodName + "(" + StrLib.EnumerableToStr(parameterNames) + ") não foi encontrado na classe " + typePage.FullName + ".");
            }

            return result;
        }

        /// <summary>
        ///   Obtém todos os métodos públicos estáticos de nome "GetUrl" que devolvam string.
        ///   O tipo deve conter o atributo NavUrlAttribute.
        /// </summary>
        private static MethodInfo[] GetMethods(Type typePage)
        {
            var list = new ArrayList();

            var urlInClass = typePage.GetCustomAttributes(typeof (NavUrlAttribute), false).Length >= 1;

            if (typePage.IsSubclassOf(typeof (Page)))
            {
                foreach (var methodInfo in typePage.GetMethods(BindingFlags.Public | BindingFlags.Static))
                {
                    if (methodInfo.Name == "GetUrl" && methodInfo.ReturnType == typeof (string))
                    {
                        if (urlInClass || methodInfo.GetCustomAttributes(typeof (NavUrlAttribute), false).Length >= 1)
                        {
                            list.Add(methodInfo);
                        }
                    }
                }
            }

            return (MethodInfo[])list.ToArray(typeof (MethodInfo));
        }

        private static string GetNavigationMethodSignature(MethodInfo method)
        {
            var message = VerifyNavigationMethod(method);
            if (message.Length > 0)
            {
                throw new ArgumentException(message);
            }

            return method.DeclaringType.FullName + "(" + StrLib.EnumerableToStr(TechLib.EnumerableItemProperty(method.GetParameters(), "Name"), ", ") + ")";
        }

        private static string GetTypeName(string navigationString)
        {
            if (navigationString == null || navigationString.Trim().Length == 0)
            {
                throw new ArgumentException();
            }

            var p = navigationString.IndexOf('(');
            return p < 0 ? navigationString : navigationString.Substring(0, p);
        }

        private static void MainAssemblyAttribute_Reset(object sender, EventArgs e)
        {
            navigationMethods = null;
            if (NavigationMethodsReset != null)
            {
                NavigationMethodsReset(null, EventArgs.Empty);
            }
        }

        /// <summary>
        ///   Devolve string vazia se o método informado é de navegação
        ///   (GetUrl() público, estático, que devolve string, declarado em classe derivada de TPage).
        ///   Caso não seja, devolve mensagem informando a causa.
        /// </summary>
        /// <param name = "method"></param>
        private static string VerifyNavigationMethod(MethodInfo method)
        {
            if (method.Name != "GetUrl")
            {
                return "O nome do método não é GetUrl.";
            }

            if (!method.IsStatic)
            {
                return "O método não é estático.";
            }

            if (!method.IsPublic)
            {
                return "O método não é público.";
            }

            if (method.ReturnType != typeof (string))
            {
                return "O método não devolve string.";
            }

            if (!typeof (TPage).IsAssignableFrom(method.DeclaringType))
            {
                return "O método não está declarado em classe derivada de TPage.";
            }

            return string.Empty;
        }

        public class ParameterCollection : ReadOnlyCollectionBase
        {
            readonly ArrayList keys = new ArrayList();

            // Modifier 'internal' não permite que esta classe seja instanciada fora do assembly
            internal ParameterCollection()
            {
            }

            public string[] Keys
            {
                get
                {
                    return (string[])this.keys.Clone();
                }
            }

            public string this[string key]
            {
                get
                {
                    var index = this.keys.IndexOf(key);
                    if (index < 0)
                    {
                        throw new ArgumentException("Elmento não encontrado");
                    }

                    return (string)this.InnerList[index];
                }
            }

            public string GetKey(int index)
            {
                return (string)this.keys[index];
            }

            public string GetValue(int index)
            {
                return (string)this.InnerList[index];
            }

            internal void Add(string key, string value)
            {
                this.keys.Add(key);
                this.InnerList.Add(value);
            }
        }
    }
}