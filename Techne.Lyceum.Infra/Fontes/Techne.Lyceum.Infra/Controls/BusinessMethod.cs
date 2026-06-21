using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.UI;
using Techne.Data;
using Techne.Web;

namespace Techne.Controls
{
    public class BusinessMethod
    {
        private const string ExecuteMessage_Def = "Função {0} executada com sucesso ({1} registro(s)).";

        private static IDictionary businessMethodDictionary;

        private IList changedTables;

        private string connection;

        private string executeMessage;

        private MethodInfo method;

        private string navigationMethod;

        private string navigationParameters;

        private Control parent;

        private string signature;

        private Type[] types;

        private string values;

        static BusinessMethod()
        {
            BusinessAssemblyAttribute.Reset += BusinessAssemblyAttribute_Reset;
        }

        // O construtor sem parâmetros deve ser PUBLIC.
        public BusinessMethod()
        {
            this.connection = string.Empty;
            this.navigationMethod = string.Empty;
            this.navigationParameters = string.Empty;
            this.signature = string.Empty;
            this.values = string.Empty;

            this.ExecuteMessage = ExecuteMessage_Def;
        }

        public BusinessMethod(Control parent) : this()
        {
            if (parent == null)
            {
                throw new ArgumentNullException();
            }

            this.parent = parent;
        }

        public static IDictionary BusinessMethodDictionary
        {
            get
            {
                if (businessMethodDictionary == null)
                {
                    try
                    {
                        var temp = new Hashtable();

                        foreach (var assembly in BusinessAssemblyAttribute.BusinessAssemblies)
                        {
                            foreach (var type in assembly.GetTypes())
                            {
                                // O método tem que ser público, estático...
                                foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
                                {
                                    var parameters = method.GetParameters();
                                    var firstParameter = parameters.Length == 0 ? null : parameters[0];

                                    // ... não pode ter parâmetros out, ref ou params...
                                    var hasOut = false;
                                    foreach (var parameter in parameters)
                                    {
                                        if (parameter.ParameterType.IsByRef || parameter.ParameterType.IsArray)
                                        {
                                            hasOut = true;
                                            break;
                                        }
                                    }

                                    // ... tem que devolver tipo RetVal ou derivado...
                                    if (!hasOut && typeof (RetVal).IsAssignableFrom(method.ReturnType) &&
// ... sendo o primeiro parâmetro uma TConnection ou derivado.
                                        firstParameter != null && typeof (TConnection).IsAssignableFrom(firstParameter.ParameterType))
                                    {
                                        temp.Add(GetMethodSignature(method), method);
                                    }
                                }
                            }
                        }

                        // Essa atribuição é feita somente no final para que acessos
                        // durante a construção não vejam a lista incompleta.
                        businessMethodDictionary = temp;
                    }
                    catch (Exception)
                    {
                        businessMethodDictionary = null;
                        throw;
                    }
                }

                return businessMethodDictionary;
            }
        }

        /// <summary>
        ///   Lista dos nomes das tabelas alteradas na última chamada ao método Call().
        /// </summary>
        [
            Browsable(false), 
        ]
        public IList ChangedTables
        {
            get
            {
                return this.changedTables;
            }
        }

        [
            DefaultValue(""), 
        ]
        public string Connection
        {
            get
            {
                return this.connection;
            }

            set
            {
                this.connection = value == null || value.Trim().Length == 0 ? string.Empty : value.Trim();
            }
        }

        [
            Category("Método"), 
            DefaultValue(ExecuteMessage_Def), 
            Description(
                "Mensagem dada ao usuário caso mais de um registro seja processado e " +
                "o método chamado não tenha especificado uma para execução bem sucedida. " +
                "Utilize {0} para indicar a substitução pelo atributo ControlText do método e " +
                "{1} para o número de registros processados."
                ), 
        ]
        public string ExecuteMessage
        {
            get
            {
                return this.executeMessage;
            }

            set
            {
                this.executeMessage = value == null ? string.Empty : value.Trim();
            }
        }

        [DefaultValue(""), TypeConverter(typeof (BusinessMethodConverter)), Category("M\x00e9todo")]
        public string ExecuteMethod
        {
            get
            {
                return this.signature;
            }

            set
            {
                this.signature = value == null ? string.Empty : value.Trim();
                this.method = null; // Reseta o cache de GetMethod()
            }
        }

        [
            Category("Método"), 
            DefaultValue(""), 
        ]
        public string ExecuteParameters
        {
            get
            {
                return this.values;
            }

            set
            {
                this.values = value == null ? string.Empty : value.Trim();
            }
        }

        [
            Category("Navegação"), 
            DefaultValue(""), 
            TypeConverter(typeof (GetUrlConverter)), 
        ]
        public string NavigationMethod
        {
            get
            {
                return this.navigationMethod;
            }

            set
            {
                this.navigationMethod = value == null ? string.Empty : value.Trim();
            }
        }

        [
            Category("Navegação"), 
            DefaultValue(""), 
        ]
        public string NavigationParameters
        {
            get
            {
                return this.navigationParameters;
            }

            set
            {
                this.navigationParameters = value == null ? string.Empty : value.Trim();
            }
        }

        [
            Browsable(false), 
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), 
        ]
        public Control Parent
        {
            get
            {
                return this.parent;
            }
        }

        private MethodInfo Method
        {
            get
            {
                if (this.method == null)
                {
                    this.method = FindBusinessMethod(this.ExecuteMethod);
                }

                if (this.method == null)
                {
                    throw new InvalidOperationException("O método " + this.ExecuteMethod + " não foi encontrado nos BusinessAssemblies.");
                }

                return this.method;
            }
        }

        private Type[] Types
        {
            get
            {
                if (this.types == null)
                {
                    try
                    {
                        var methodParameters = this.Method.GetParameters();
                        this.types = new Type[methodParameters.Length];
                        for (var i = 0; i < methodParameters.Length; i++)
                        {
                            this.types[i] = methodParameters[i].ParameterType;
                        }
                    }
                    catch
                    {
                        this.types = null;
                        throw;
                    }
                }

                return this.types;
            }
        }

        public static MethodInfo FindBusinessMethod(string methodSignature)
        {
            return (MethodInfo)BusinessMethodDictionary[methodSignature];
        }

        public override string ToString()
        {
            return this.ExecuteMethod;
        }

        public RetVal Call(IRecordContainer container)
        {
            if (!(this.parent.Page is TPage))
            {
                throw new ArgumentException("O controle informado não está contido numa TPage.");
            }

            var permission = TControl.GetPermission(this.parent);
            if (permission == null || permission.ReadOnly)
            {
                return TPermission.DenialMessage;
            }

            

            var cn = TControl.CreateWritableConnection(this.Parent, this.Connection);
            cn.Open(true);
            try
            {
                

                var result = this.Call(cn, container, true);
                if (!result.Ok)
                {
                    cn.Rollback();
                }

                this.changedTables = cn.ChangedTables;
                return result;

                #region cn.Close();
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }

            #endregion
        }

        public RetVal Call(TConnectionWritable cn, IRecordContainer container, bool navigate)
        {
            // Obtém os valores dos PARÂMETROS
            object[] parameters;
            try
            {
                var references = ValueReference.ParseList(this.ExecuteParameters);
                var start = container != null ? (Control)container : this.Parent;

                // Verifica se algum controle referenciado tem valor inválido
                var controlReferenceError = this.ControlReferenceError(references, start);
                if (controlReferenceError.Length > 0)
                {
                    return "Foi informado algum valor inválido";
                }

                var tempParameters = ValueReference.GetValues(references, start, this.Types);
                parameters = new object[tempParameters.Length + 1];
                tempParameters.CopyTo(parameters, 1);
            }
            catch (Exception exc)
            {
                throw new ApplicationException("Ocorreu um problema na obtenção dos valores dos parâmetros do método a ser executado.", exc);
            }

            parameters[0] = cn;

            

            for (var i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] == null)
                {
                    var vref = ValueReference.ParseList(this.ExecuteParameters)[i];
                    var ctlref = vref as ControlReference;
                    var ctl = ctlref != null ? ctlref.GetControl(this.Parent) as ITControl : null;
                    if (ctl == null)
                    {
                        throw new ApplicationException("A referência " + vref + " possui um valor inválido como parâmetro de chamada ao método (null).");
                    }

                    return "O campo " + ctl.GetCaption(Thread.CurrentThread.CurrentCulture.Name) + " possui um valor inválido.";
                }
            }

            if (parameters.Length != this.Types.Length)
            {
                if (parameters.Length > 0)
                {
                    throw new ApplicationException("O número de parâmetros informados é diferente do esperado.");
                }
                else
                {
                    throw new ApplicationException("Os parâmetros não foram informados.");
                }
            }

            

            // EXECUTA o método
            RetVal methodResult;
            try
            {
                methodResult = this.Method.Invoke(null, parameters) as RetVal;
            }
            catch (ArgumentException exc)
            {
                // Verifica se o erro é tipo inválido do parâmetro.
                for (var i = 0; i < this.Types.Length; i++)
                {
                    if (!this.Types[i].IsAssignableFrom(parameters[i].GetType()))
                    {
                        throw new ArgumentException("O tipo do parâmetro " + (i + 1) + " deve ser " + this.Types[i].FullName + " (método " + this.ExecuteMethod + ").");
                    }
                }

                throw new ArgumentException(string.Empty, exc);
            }

            if (methodResult == null)
            {
                throw new ApplicationException("O método devolveu null.");
            }

            // NAVEGA se não deu erro
            if (navigate && methodResult.Ok && this.NavigationMethod.Length > 0)
            {
                try
                {
                    this.Navigate(methodResult.Message);
                }
                catch (Exception exc)
                {
                    throw new ApplicationException("Ocorreu um problema no momento da navegação, após executar o método com sucesso.", exc);
                }
            }

            return methodResult;
        }

        /// <summary>
        ///   Devolve o atributo ControlText associado ao método,
        ///   ou o nome do método se o atributo não foi informado.
        ///   Nunca devolve null ou string vazia.
        /// </summary>
        public string GetControlText()
        {
            var caption = ControlTextAttribute.GetControlText(this.Method, TPage.IDIOMADEFAULT);
            if (caption == null)
            {
                return this.Method.Name;
            }

            return caption;
        }

        public string GetDescription()
        {
            var description = MethodDescriptionAttribute.GetDescription(this.Method, TPage.IDIOMADEFAULT);
            if (description != null)
            {
                return description;
            }

            return string.Empty;
        }

        public string GetExecuteMessage(int countSuccessful)
        {
            return string.Format(this.ExecuteMessage, this.GetControlText(), countSuccessful);
        }

        public string GetImageUrl()
        {
            var url = ImageAttribute.GetUrl(this.Method);
            if (url == null)
            {
                return string.Empty;
            }

            return url;
        }

        public string GetToolTip()
        {
            var tooltip = ToolTipAttribute.GetToolTip(this.Method, TPage.IDIOMADEFAULT);

// Se não for null, tooltip nunca será string vazia.
            return tooltip != null ? tooltip : string.Empty;
        }

        /// <summary>
        ///   Obtém uma string no formato Namespace.Classe.Metodo(parametro1, parametro2...), ou seja,
        ///   includeParameters=true.
        ///   Veja que o primeiro parâmetro (parametro0), a TConnection, não é devolvido!
        /// </summary>
        internal static string GetMethodSignature(MethodInfo method)
        {
            return GetMethodSignature(method, true, typeof (TConnection));
        }

        /// <summary>
        ///   Obtém uma string no formato Namespace.Classe.Metodo(parametro1, parametro2...).
        ///   A lista de parâmetros pode ser incluída ou não.
        ///   Veja que o primeiro parâmetro (parametro0), a TConnection, não é devolvido!
        /// </summary>
        internal static string GetMethodSignature(MethodInfo method, bool includeParameters)
        {
            return GetMethodSignature(method, includeParameters, typeof (TConnection));
        }

        /// <summary>
        ///   Obtém uma string no formato Namespace.Classe.Metodo(parametro1, parametro2...).
        ///   A lista de parâmetros pode ser incluída ou não.
        ///   Veja que o primeiro parâmetro (parametro0) não é devolvido!
        /// </summary>
        /// <param name = "includeParameters">
        ///   Inclui lista com o nome dos parâmetros.
        /// </param>
        internal static string GetMethodSignature(MethodInfo method, bool includeParameters, Type firstParameterType)
        {
            // Monta um array sem o primeiro parâmetro (que é TConnection)
            ParameterInfo[] parameters;
            {
                var methodParameters = method.GetParameters();
                var count = methodParameters.Length;
                if (count == 0 || !firstParameterType.IsAssignableFrom(methodParameters[0].ParameterType))
                {
                    throw new ArgumentException("O método não possui o primeiro parâmetro do tipo " + firstParameterType.FullName + ".");
                }

                parameters = new ParameterInfo[count - 1];
                Array.Copy(methodParameters, 1, parameters, 0, count - 1);
            }

            return method.ReflectedType.FullName + "." + method.Name + "(" +
                   (includeParameters ? StrLib.EnumerableToStr(TechLib.EnumerableItemProperty(parameters, "Name", true), ", ") : string.Empty) +
                   ")";
        }

        internal RetVal Call(IRecordContainer[] containers)
        {
            if (!(this.parent.Page is TPage))
            {
                throw new ArgumentException("O controle informado não está contido numa TPage.");
            }

            var permission = TControl.GetPermission(this.parent);
            if (permission.ReadOnly)
            {
                return TPermission.DenialMessage;
            }

            var countSuccessful = 0;
            bool allDone;

            

            var cn = TControl.CreateWritableConnection(this.Parent, this.Connection);
            cn.Open(true);
            try
            {
                

                foreach (var container in containers)
                {
                    var result = this.Call(cn, container, false);

                    if (result.Ok)
                    {
                        countSuccessful++;
                    }
                    else
                    {
                        // Só adiciona a mensagem ao container se deu erro.
                        ((IRecordContainerInternal)container).AddMessage(result.Message, true);
                    }
                }

                allDone = countSuccessful == containers.Length;

                if (!allDone)
                {
                    cn.Rollback();
                }

                this.changedTables = cn.ChangedTables;

                #region cn.Close();
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }

            #endregion

            if (allDone)
            {
                var message = this.GetExecuteMessage(countSuccessful);

                // Navega se nenhuma das chamadas falhou.
                if (this.NavigationMethod.Length > 0)
                {
                    this.Navigate(message);
                }

                return RetVal.Success(message);
            }

            if (containers.Length == 1)
            {
                return "Ocorreram erros.";
            }
            else
            {
                return "Ocorreram erros com pelo menos um dos registros.";
            }
        }

        internal void SetParent(Control parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException();
            }

            this.parent = parent;
        }

        private static void BusinessAssemblyAttribute_Reset(object sender, EventArgs e)
        {
            businessMethodDictionary = null;
        }

        /// <summary>
        ///   Dentre as referências informadas, devolve a mensagem de
        ///   erro do primeiro controle referenciado com erro.
        /// </summary>
        /// <returns>
        ///   Erro do primeiro controle referenciado com erro, ou string vazia
        ///   se controle referenciado nesta situação não existir.
        /// </returns>
        private string ControlReferenceError(ValueReference[] references, Control start)
        {
            var error = string.Empty;

            foreach (var reference in references)
            {
                var controlReference = reference as ControlReference;
                var control = controlReference != null ? controlReference.GetControl(start) : null;
                var controlEditable = control != null ? control as TControlEditable : null;
                error = controlEditable != null ? controlEditable.GetValueError() : string.Empty;
                if (error.Length > 0)
                {
                    controlEditable.Msg = error;
                    break;
                }
            }

            return error;
        }

        private void Navigate(string message)
        {
            if (this.NavigationMethod.Length == 0)
            {
                return;
            }

            Navigation navigation;
            try
            {
                navigation = Navigation.Create(this.NavigationMethod);
            }
            catch
            {
                return;
            }

            var parameters = ValueReference.GetValues(this.NavigationParameters, this.Parent, navigation.ParameterTypes);

            var page = this.Parent != null ? this.Parent.Page as TPage : null;

            if (page != null)
            {
                page.Redirect(navigation.GetUrl(parameters), message);
            }
            else
            {
                HttpContext.Current.Response.Redirect(navigation.GetUrl(parameters));
            }
        }
    }

    public class BusinessMethodConverter : TypeConverter
    {
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var list = new ArrayList();
            try
            {
                foreach (DictionaryEntry item in BusinessMethod.BusinessMethodDictionary)
                {
                    list.Add(BusinessMethod.GetMethodSignature((MethodInfo)item.Value));
                }
            }
            catch
            {
            }

            list.Sort();

            return new StandardValuesCollection(list);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }

    public class BusinessMethodCollection : CollectionBase
    {
        private readonly Control parent;

        internal BusinessMethodCollection(Control parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException();
            }

            this.parent = parent;
        }

        internal Control Parent
        {
            get
            {
                return this.parent;
            }
        }

        public BusinessMethod this[int index]
        {
            get
            {
                return (BusinessMethod)this.List[index];
            }

            set
            {
                this.List[index] = value;
            }
        }

        public int Add(BusinessMethod method)
        {
            return this.List.Add(method);
        }

        public void Remove(BusinessMethod method)
        {
            this.List.Remove(method);
        }

        protected override void OnInsert(int index, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            ((BusinessMethod)value).SetParent(this.Parent);
            base.OnInsert(index, value);
        }

        protected override void OnRemove(int index, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            ((BusinessMethod)value).SetParent(null);
            base.OnRemove(index, value);
        }

        protected override void OnSet(int index, object oldValue, object newValue)
        {
            if (newValue == null)
            {
                throw new ArgumentNullException();
            }

            ((BusinessMethod)oldValue).SetParent(null);
            ((BusinessMethod)newValue).SetParent(this.Parent);
            base.OnSet(index, oldValue, newValue);
        }
    }
}