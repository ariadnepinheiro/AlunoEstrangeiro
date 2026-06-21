using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Reflection;
using System.Windows.Forms;

namespace Techne.Controls
{
    internal partial class ConfigureTTableDataSource : Form
    {
        private readonly TTableDataSource _component;

        private TTableDataSourceDesigner _designer;

        private string _methodSignature = string.Empty;

        private string _selectMethod = string.Empty;

        private IServiceProvider _serviceProvider;

        private string _typeName = string.Empty;

        public ConfigureTTableDataSource()
        {
            this.InitializeComponent();
        }

        public ConfigureTTableDataSource(IServiceProvider provider, TTableDataSourceDesigner designer)
        {
            this.InitializeComponent();

            this._serviceProvider = provider;
            this._component = designer.Component as TTableDataSource;
            this._designer = designer;

            ////pega tipos do componente
            // if (_component != null)
            // {
            // MethodItem m = new MethodItem(_component.GetSelectMethodInfo());
            // this.TypeName = _component.TypeName;
            // this.SelectMethod = _component.SelectMethod;
            // this.SelectMethodSignature = m.Signature;
            // }

            // Preenche listas de tipos emétodos
            this.DiscoverTypes();
            this.DiscoverMethods();
        }

        internal string SelectMethod
        {
            get
            {
                return this._selectMethod;
            }

            set
            {
                this._selectMethod = value == null ? string.Empty : value;
            }
        }

        internal string SelectMethodSignature
        {
            get
            {
                return this._methodSignature;
            }

            set
            {
                this._methodSignature = value == null ? string.Empty : value;
            }
        }

        internal string TypeName
        {
            get
            {
                return this._typeName;
            }

            set
            {
                this._typeName = value == null ? string.Empty : value;
            }
        }

        private Type ServiceType
        {
            get
            {
                ITypeResolutionService typeService = null;
                if (this._component.Site != null)
                {
                    typeService = (ITypeResolutionService)this._component.Site.GetService(typeof (ITypeResolutionService));
                }

                if (typeService != null)
                {
                    var t = typeService.GetType(this._typeName, false, true);
                    return t;
                }
                else
                {
                    return null;
                }
            }
        }

        internal static string BuildSignature(MethodInfo method)
        {
            if (method == null)
            {
                return "()";
            }

            var pars = method.GetParameters();
            var listPars = new List<string>();
            foreach (var par in pars)
            {
                if (par.IsRetval)
                {
                    continue;
                }

                listPars.Add(par.ParameterType.Name + " " + par.Name);
            }

            return "(" + string.Join(", ", listPars.ToArray()) + ")";
        }

        internal static MethodInfo GetMethodInfo(Type type, string methodName, string methodSignature)
        {
            if (type == null || string.IsNullOrEmpty(methodName))
            {
                return null;
            }

            var methods = type.GetMethods();
            MethodInfo ret = null;
            foreach (var m in methods)
            {
                if (m.Name.Equals(methodName, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.IsNullOrEmpty(methodSignature))
                    {
                        ret = m;
                        break;
                    }
                    else
                    {
                        var signature = BuildSignature(m);
                        if (methodSignature.Equals(signature, StringComparison.InvariantCultureIgnoreCase))
                        {
                            ret = m;
                            break;
                        }
                    }
                }
            }

            return ret;
        }

        private void DiscoverMethods()
        {
            var serviceType = this.ServiceType;
            if (serviceType == null)
            {
                this.ddlMethods.Items.Clear();
                return;
            }

            try
            {
                this.ddlMethods.BeginUpdate();
                this.ddlMethods.Items.Clear();

                var methods = this.GetQueryMethods(serviceType, false);
                var bHasCurrentMethod = false;

                foreach (var m in methods)
                {
                    var methodItem = new MethodItem(m);
                    this.ddlMethods.Items.Add(methodItem);
                    if (!bHasCurrentMethod &&
                        methodItem.Name.Equals(this.SelectMethod, StringComparison.InvariantCultureIgnoreCase) &&
                        methodItem.Signature.Equals(this.SelectMethodSignature, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bHasCurrentMethod = true;
                    }
                }

                if (!bHasCurrentMethod)
                {
                    var m = GetMethodInfo(serviceType, this.SelectMethod, this.SelectMethodSignature);
                    if (m != null)
                    {
                        var methodItem = new MethodItem(m);
                        this.ddlMethods.Items.Add(methodItem);
                    }
                }
            }
            finally
            {
                this.ddlMethods.EndUpdate();
            }

            this.ddlMethods.Sorted = true;

// seleciona o tipo corrente
            for (var i = 0; i < this.ddlMethods.Items.Count; i++)
            {
                var mi = this.ddlMethods.Items[i] as MethodItem;
                if (mi != null && mi.Name.Equals(this.SelectMethod, StringComparison.InvariantCultureIgnoreCase) &&
                    mi.Signature.Equals(this.SelectMethodSignature, StringComparison.InvariantCultureIgnoreCase))
                {
                    this.ddlMethods.SelectedIndex = i;
                    break;
                }
            }
        }

        private void DiscoverTypes()
        {
            var businessOnly = false;

            // try to get a reference to the type discovery service
            ITypeDiscoveryService discovery = null;
            if (this._component != null && this._component.Site != null)
            {
                discovery = (ITypeDiscoveryService)this._component.Site.GetService(typeof (ITypeDiscoveryService));
            }

            // if the type discovery service is available
            if (discovery != null)
            {
                // salva cursor
                var previousCursor = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;

                businessOnly = this.chkBusinessOnly.Checked;
                try
                {
                    this.ddlTypes.BeginUpdate();
                    this.ddlTypes.Items.Clear();

                    // busca todos os tipos
                    var types = discovery.GetTypes(typeof (object), true);

                    var bHasCurrentType = false;

// adiciona os tipos ao combobox
                    if (types != null)
                    {
                        var asmTechne = this.GetType().Assembly;
                        foreach (Type type in types)
                        {
                            // ignora a Techne.dll
                            if (type.Assembly.FullName == asmTechne.FullName)
                            {
                                continue;
                            }

// ignora classes genéricas, abstratas ou interfaces
                            if (type.IsInterface || type.IsAbstract || type.IsGenericType || type.IsSpecialName)
                            {
                                continue;
                            }

// ignora classes fora do RN, quando pedido
                            if (businessOnly)
                            {
                                var attrib = type.Assembly.GetCustomAttributes(typeof (Techne.BusinessAssemblyAttribute), false);
                                if (attrib == null || attrib.Length == 0)
                                {
                                    continue;
                                }
                            }

// busca somente classes com métodos que retornem TDataTable ou QueryTable
                            var methods = this.GetQueryMethods(type, true);
                            if (methods.Length == 0)
                            {
                                continue;
                            }

                            var typeItem = new TypeItem(type);
                            var nIndex = this.ddlTypes.Items.Add(typeItem);

                            // marca tipo corrente no combo
                            if (!bHasCurrentType && typeItem.Name.Equals(this.TypeName, StringComparison.InvariantCultureIgnoreCase))
                            {
                                bHasCurrentType = true;
                            }
                        }
                    }

// se o tipo corrente não está no combobox, adicione-o
                    if (!bHasCurrentType && this.TypeName.Length > 0)
                    {
                        var typeItem = new TypeItem(this.TypeName);
                        var nIndex = this.ddlTypes.Items.Add(typeItem);
                        this.ddlTypes.SelectedIndex = nIndex;
                    }
                }
                finally
                {
                    Cursor.Current = previousCursor;
                    this.ddlTypes.EndUpdate();
                }

                this.ddlTypes.Sorted = true;

// seleciona o tipo corrente
                for (var i = 0; i < this.ddlTypes.Items.Count; i++)
                {
                    var ti = this.ddlTypes.Items[i] as TypeItem;
                    if (ti != null && ti.Name.Equals(this.TypeName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        this.ddlTypes.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private MethodInfo[] GetQueryMethods(Type type, bool firstOnly)
        {
            MethodInfo[] methods = null;

            methods = type.GetMethods();
            var queries = new List<MethodInfo>();
            foreach (var m in methods)
            {
                if (m.IsGenericMethod || m.IsConstructor || m.IsAbstract || m.IsSpecialName)
                {
                    continue;
                }

                var retType = m.ReturnType;
                if (retType.IsSubclassOf(typeof (System.Data.DataTable)))
                {
                    queries.Add(m);
                    if (firstOnly && queries.Count > 0)
                    {
                        break;
                    }
                }
            }

            return queries.ToArray();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ////se alguma propriedade mudou, salva as mudanças
            // if (String.Compare(TypeName, _component.TypeName, false) != 0 ||
            // String.Compare(SelectMethod, _component.SelectMethod, false) != 0)
            // {
            // PropertyDescriptor prop=TypeDescriptor.GetProperties(_component)["TypeName"];
            // prop.SetValue(_component, TypeName);

            // prop = TypeDescriptor.GetProperties(_component)["SelectMethod"];
            // prop.SetValue(_component, SelectMethod);

            // if (this.ServiceType != null && _designer.CanRefreshSchema)
            // _designer.RefreshSchema(true);
            // }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkBusinessOnly_CheckedChanged(object sender, EventArgs e)
        {
            this.DiscoverTypes();
            this.DiscoverMethods();
        }

        private void ddlMethods_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mi = this.ddlMethods.SelectedItem as MethodItem;
            if (mi != null)
            {
                this._selectMethod = mi.Name;
                this._methodSignature = mi.Signature;
            }
            else
            {
                this._selectMethod = string.Empty;
                this._methodSignature = string.Empty;
            }
        }

        private void ddlTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ti = this.ddlTypes.SelectedItem as TypeItem;
            if (ti != null)
            {
                this._typeName = ti.Name;
            }

            this.DiscoverMethods();
        }

        internal class MethodItem
        {
            private readonly string _methodName = string.Empty;

            private readonly string _methodSignature = string.Empty;

            private readonly string _returnType = string.Empty;

            public MethodItem(MethodInfo methodInfo)
            {
                if (methodInfo == null)
                {
                    this._methodName = string.Empty;
                    this._methodSignature = string.Empty;
                    this._returnType = string.Empty;
                }
                else
                {
                    this._methodName = methodInfo.Name;
                    this._methodSignature = BuildSignature(methodInfo);
                    this._returnType = methodInfo.ReturnType == null ? "void" : methodInfo.ReturnType.Name;
                }
            }

            public string FullName
            {
                get
                {
                    return this.Name + this.Signature + " returns " + this.ReturnType;
                }
            }

            public string Name
            {
                get
                {
                    return this._methodName;
                }
            }

            public string ReturnType
            {
                get
                {
                    return this._returnType;
                }
            }

            public string Signature
            {
                get
                {
                    return this._methodSignature;
                }
            }

            // public MethodItem(string methodName,string methodSignature,string returnType)
            // {
            // _methodName = methodName;
            // _methodSignature = methodSignature;
            // _returnType = returnType;
            // }
            public override string ToString()
            {
                return this.FullName;
            }
        }

        internal class TypeItem
        {
            private readonly string _typeName;

            // public Type Type
            // {
            // get 
            // { 
            // return _type; 
            // }
            // }
            public TypeItem(Type type)
            {
                if (type == null)
                {
                    this._typeName = string.Empty;
                }
                else
                {
                    this._typeName = type.FullName;
                }
            }

            public TypeItem(string typeName)
            {
                if (typeName == null)
                {
                    this._typeName = string.Empty;
                }
                else
                {
                    this._typeName = typeName;
                }
            }

            public string Name
            {
                get
                {
                    return this._typeName;
                }
            }

            public override string ToString()
            {
                return this.Name;
            }
        }
    }
}