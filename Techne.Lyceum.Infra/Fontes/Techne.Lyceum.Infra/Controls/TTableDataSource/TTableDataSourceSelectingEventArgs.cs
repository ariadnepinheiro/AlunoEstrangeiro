using System.Collections.Specialized;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;

namespace Techne.Controls
{
    public delegate void TTableDataSourceSelectingEventHandler(object sender, TTableDataSourceSelectingEventArgs e);

    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal), AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class TTableDataSourceMethodEventArgs : CancelEventArgs
    {
        // Fields
        private readonly IOrderedDictionary _inputParameters;

        // Methods
        public TTableDataSourceMethodEventArgs(IOrderedDictionary inputParameters)
        {
            this._inputParameters = inputParameters;
        }

        // Properties
        public IOrderedDictionary InputParameters
        {
            get
            {
                return this._inputParameters;
            }
        }
    }

    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal), AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class TTableDataSourceSelectingEventArgs : TTableDataSourceMethodEventArgs
    {
        // Fields
        private readonly DataSourceSelectArguments _arguments;

        private bool _executingSelectCount;

        // Methods
        public TTableDataSourceSelectingEventArgs(IOrderedDictionary inputParameters, DataSourceSelectArguments arguments, bool executingSelectCount)
            : base(inputParameters)
        {
            this._arguments = arguments;
            this._executingSelectCount = executingSelectCount;
        }

        // Properties
        public DataSourceSelectArguments Arguments
        {
            get
            {
                return this._arguments;
            }
        }

        // public bool ExecutingSelectCount
        // {
        // get
        // {
        // return this._executingSelectCount;
        // }
        // }
    }
}