using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;
using System.Web.UI.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Techne.Data;

namespace Techne.Controls
{
    public class TTableDataSourceDesigner : DataSourceDesigner
    {
        private static readonly string[] _views = { TTableDataSource.DefaultViewName };

        private TTableDataViewDesigner _view;

        // private bool _inWizard;

        public override bool CanConfigure
        {
            get
            {
                return false;
// return this.TypeServiceAvailable; 
            }
        }

        public override bool CanRefreshSchema
        {
            get
            {
                if (!String.IsNullOrEmpty(this.DataTableClassName))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public string DataTableClassName
        {
            get
            {
                if (this.DataSourceComponent != null)
                {
                    return this.DataSourceComponent.DataTableClassName;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.DataSourceComponent != null)
                {
                    if (String.Compare(this.DataSourceComponent.DataTableClassName, value, false) != 0)
                    {
                        this.DataSourceComponent.DataTableClassName = value;

                        // notify to the associated designers that this component has changed
                        if (this.CanRefreshSchema)
                        {
                            this.RefreshSchema(true);
                        }
                        else
                        {
                            this.OnDataSourceChanged(EventArgs.Empty);
                        }

                        this.UpdateDesignTimeHtml();
                    }
                }
            }
        }

        public string SqlWhere
        {
            get
            {
                if (this.DataSourceComponent != null)
                {
                    return this.DataSourceComponent.SqlWhere;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.DataSourceComponent != null)
                {
                    if (String.Compare(this.DataSourceComponent.SqlWhere, value, false) != 0)
                    {
                        this.DataSourceComponent.SqlWhere = value;

                        // notify to the associated designers that this component has changed
                        if (this.CanRefreshSchema)
                        {
                            this.RefreshSchema(true);
                        }
                        else
                        {
                            this.OnDataSourceChanged(EventArgs.Empty);
                        }

                        this.UpdateDesignTimeHtml();
                    }
                }
            }
        }

        internal TTableDataSource DataSourceComponent
        {
            get
            {
                return this.Component as TTableDataSource;
            }
        }

        internal IDataSourceViewSchema DataSourceSchema
        {
            get
            {
                var schema = this.DesignerState["DataSourceSchema"] as IDataSourceViewSchema;
                return schema;
            }

            set
            {
                this.DesignerState["DataSourceSchema"] = value;
            }
        }

        protected TTableDataViewDesigner View
        {
            get
            {
                if (this._view == null)
                {
                    this._view = new TTableDataViewDesigner(this, _views[0]);
                }

                return this._view;
            }
        }

        private bool TypeServiceAvailable
        {
            get
            {
                IServiceProvider site = base.Component.Site;
                if (site == null)
                {
                    return false;
                }

                var service = (ITypeResolutionService)site.GetService(typeof (ITypeResolutionService));
                var service2 = (ITypeDiscoveryService)site.GetService(typeof (ITypeDiscoveryService));
                if (service == null)
                {
                    return service2 != null;
                }

                return true;
            }
        }

        public override void Configure()
        {
            // _inWizard = true;

            // generate a transaction to undo changes
            InvokeTransactedChange(this.Component, this.ConfigureDataSourceCallback, null, "ConfigureDataSource");

            // _inWizard = false;
        }

        public override DesignerDataSourceView GetView(string viewName)
        {
            if ((viewName == null) || ((viewName.Length != 0) && (String.Compare(viewName, TTableDataSource.DefaultViewName, StringComparison.OrdinalIgnoreCase) != 0)))
            {
                throw new ArgumentException("An invalid view was requested", "viewName");
            }

            return this.View;
        }

        public override string[] GetViewNames()
        {
            return _views;
        }

        public override void RefreshSchema(bool preferSilent)
        {
            // if (1 + 1 == 2)
            // {
            // base.RefreshSchema(preferSilent);
            // return;
            // }
            // saves the old cursor
            var oldCursor = Cursor.Current;

            try
            {
                // ignore data source events while refreshing the schema
                this.SuppressDataSourceEvents();

                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    var type = this.DataSourceComponent.GetTableType();

                    if (type == null)
                    {
                        return;
                    }

                    this.RefreshSchemaInternal(type, preferSilent);
                }
                finally
                {
                    // restores the cursor
                    Cursor.Current = oldCursor;
                }
            }
            finally
            {
                // resume data source events
                this.ResumeDataSourceEvents();
            }
        }

        internal static Type GetType(IServiceProvider serviceProvider, string typeName)
        {
            // try to get a reference to the resolution service
            var resolution = (ITypeResolutionService)serviceProvider.GetService(typeof (ITypeResolutionService));
            if (resolution == null)
            {
                return null;
            }

            // try to get the type
            return resolution.GetType(typeName, false, true);
        }

        internal TDataTable CreateTableInstance()
        {
            // gets the Type used in the DataSourceControl
            var type = GetType(this.Component.Site, this.DataTableClassName);

            // if we can't find the type, return
            TDataTable dt = null;
            if (type != null)
            {
                try
                {
                    dt = type.Assembly.CreateInstance(type.FullName) as Techne.Data.TDataTable;
                }
                catch
                {
                }
            }

            return dt;
        }

        internal void RefreshSchemaInternal(Type tableType, bool preferSilent)
        {
            // if all parameters are filled
            if (tableType != null)
            {
                try
                {
                    // esquema antigo
                    var oldSchema = this.DataSourceSchema;

// esquema novo
                    var sch = new TypeSchema(tableType);
                    var typeSchemas = sch.GetViews();
                    if (typeSchemas == null)
                    {
                        sch = new TypeSchema(tableType);
                    }

// se não conseguiu pegar o esquema novo, limpa esquema e sai
                    if ((typeSchemas == null) || (typeSchemas.Length == 0))
                    {
                        this.DataSourceSchema = null;
                        return;
                    }

                    var newSchema = typeSchemas[0];

                    // se o esquema mudou, atualiza e dispara evento de mudança
                    if (!DataSourceDesigner.ViewSchemasEquivalent(oldSchema, newSchema))
                    {
                        this.DataSourceSchema = newSchema;

                        this.OnSchemaRefreshed(EventArgs.Empty);
                    }
                }
                catch (Exception e)
                {
                    if (!preferSilent)
                    {
                        this.ShowError(this.DataSourceComponent.Site, "Esquema da tabela" + tableType.FullName + " não pode ser encontrado. " + e.Message);
                    }
                }
            }
        }

        protected virtual bool ConfigureDataSourceCallback(object context)
        {
            try
            {
                this.SuppressDataSourceEvents();

                IServiceProvider provider = this.Component.Site;
                if (provider == null)
                {
                    return false;
                }

                // get the service needed to show a form
                var UIService = (IUIService)provider.GetService(typeof (IUIService));
                if (UIService == null)
                {
                    return false;
                }

                // shows the form
                var configureForm = new ConfigureTTableDataSource(provider, this);
                if (UIService.ShowDialog(configureForm) == DialogResult.OK)
                {
                    this.OnDataSourceChanged(EventArgs.Empty);
                    return true;
                }
            }
            finally
            {
                this.ResumeDataSourceEvents();
            }

            return false;
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);

            // filters the SqlWhere property
            var selectMethodProp = (PropertyDescriptor)properties["SqlWhere"];
            properties["SqlWhere"] = TypeDescriptor.CreateProperty(this.GetType(), selectMethodProp, new Attribute[0]);

            // filters the DataTableClassName property
            var serviceTypeProp = (PropertyDescriptor)properties["DataTableClassName"];
            properties["DataTableClassName"] = TypeDescriptor.CreateProperty(this.GetType(), serviceTypeProp, new Attribute[0]);
        }

        private bool IsMatchingMethod(MethodInfo method, string selectMethod)
        {
            // we only check the name of the method
            return String.Compare(method.Name, selectMethod, true) == 0;
        }

        private void ShowError(ISite serviceProvider, string message)
        {
            // gets a reference to the UI service and shows the error message box
            var UIService = (IUIService)serviceProvider.GetService(typeof (IUIService));
            if (UIService != null)
            {
                UIService.ShowError(message);
            }
        }
    }
}