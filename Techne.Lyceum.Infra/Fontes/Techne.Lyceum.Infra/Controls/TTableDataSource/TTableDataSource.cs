using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;
using System.Web.UI.Design.WebControls;
using System.Web.UI.WebControls;

namespace Techne.Controls
{
    [PersistChildren(false)]
    [ParseChildren(true)]
    [DefaultEvent("Selecting")]
    [DefaultProperty("TypeName")]
    [Designer("Techne.Controls.TTableDataSourceDesigner")]
    public class TTableDataSource : DataSourceControl
    {
        internal const string DefaultViewName = "DefaultView";

        private TTableDataView _view;

        private ICollection _viewNames;

        public TTableDataSource()
        {
        }

        public TTableDataSource(string typeName)
        {
            this.DataTableClassName = typeName;
        }

        [Category("Data")]
        public event TTableDataSourceSelectingEventHandler Selecting
        {
            add
            {
                this.GetView().Selecting += value;
            }

            remove
            {
                this.GetView().Selecting -= value;
            }
        }

        [DefaultValue("")]
        [Category("Data")]
        [Editor(typeof (TDataSourceClassEditor), typeof (System.Drawing.Design.UITypeEditor))]
        [Bindable(true)]
        public string DataTableClassName
        {
            get
            {
                return this.GetView().DataTableClassName;
            }

            set
            {
                this.GetView().DataTableClassName = value;
            }
        }

        [DefaultValue("")]
        [Category("Data")]
// [Editor(typeof(TDataSourceClassEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Bindable(true)]
        [Browsable(false)]
        public string SelectMethod
        {
            get
            {
                return this.GetView().SelectMethod;
            }

            set
            {
                this.GetView().SelectMethod = value;
            }
        }

        [Editor(typeof (ParameterCollectionEditor), typeof (UITypeEditor))]
        [MergableProperty(false)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DefaultValue((string)null)]
        [Category("Data")]
        [Browsable(false)]
        public ParameterCollection SelectMethodParameters
        {
            get
            {
                return this.GetView().SelectMethodParameters;
            }
        }

        [DefaultValue("")]
        [Category("Data - SQL")]
        public string SqlColumns
        {
            get
            {
                return this.GetView().SqlColumns;
            }

            set
            {
                this.GetView().SqlColumns = value;
            }
        }

        [DefaultValue("")]
        [Category("Data - SQL")]
        public string SqlOrder
        {
            get
            {
                return this.GetView().SqlOrder;
            }

            set
            {
                this.GetView().SqlOrder = value;
            }
        }

        [DefaultValue("")]
        [Category("Data - SQL")]
        public string SqlWhere
        {
            get
            {
                return this.GetView().SqlWhere;
            }

            set
            {
                this.GetView().SqlWhere = value;
            }
        }

        [Editor(typeof (ParameterCollectionEditor), typeof (UITypeEditor))]
        [MergableProperty(false)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DefaultValue((string)null)]
        [Category("Data - SQL")]
        public ParameterCollection SqlWhereParameters
        {
            get
            {
                return this.GetView().SqlWhereParameters;
            }
        }

        public IEnumerable Select()
        {
            return this.GetView().Select(DataSourceSelectArguments.Empty);
        }

        internal Type GetTableType()
        {
            return this.GetView().DataTableType;
        }

        // internal string CreateCacheKey(int startRowIndex, int maximumRows)
        // {
        // StringBuilder builder = this.CreateRawCacheKey();
        // builder.Append(':');
        // builder.Append(startRowIndex.ToString(CultureInfo.InvariantCulture));
        // builder.Append(':');
        // builder.Append(maximumRows.ToString(CultureInfo.InvariantCulture));
        // return builder.ToString();
        // }

        // internal string CreateMasterCacheKey()
        // {
        // return this.CreateRawCacheKey().ToString();
        // }

        // private StringBuilder CreateRawCacheKey()
        // {
        // StringBuilder builder = new StringBuilder("u", 0x400);
        // builder.Append(base.GetType().GetHashCode().ToString(CultureInfo.InvariantCulture));
        // builder.Append(":");
        // builder.Append(this.CacheDuration.ToString(CultureInfo.InvariantCulture));
        // builder.Append(':');
        // builder.Append(((int)this.CacheExpirationPolicy).ToString(CultureInfo.InvariantCulture));
        // builder.Append(":");
        // builder.Append(this.SqlCacheDependency);
        // builder.Append(":");
        // builder.Append(this.TypeName);
        // builder.Append(":");
        // builder.Append(this.SelectMethod);
        // if (this.SelectParameters.Count > 0)
        // {
        // builder.Append("?");
        // foreach (DictionaryEntry entry in this.SelectParameters.GetValues(this.Context, this))
        // {
        // builder.Append(entry.Key.ToString());
        // if ((entry.Value != null) && (entry.Value != DBNull.Value))
        // {
        // builder.Append("=");
        // builder.Append(entry.Value.ToString());
        // }
        // else if (entry.Value == DBNull.Value)
        // {
        // builder.Append("(dbnull)");
        // }
        // else
        // {
        // builder.Append("(null)");
        // }
        // builder.Append("&");
        // }
        // }
        // return builder;
        // }
        // internal void InvalidateCacheEntry()
        // {
        // string key = this.CreateMasterCacheKey();
        // this.Cache.Invalidate(key);
        // }
        // internal object LoadDataFromCache(int startRowIndex, int maximumRows)
        // {
        // string key = this.CreateCacheKey(startRowIndex, maximumRows);
        // return this.Cache.LoadDataFromCache(key);
        // }

        // internal int LoadTotalRowCountFromCache()
        // {
        // string key = this.CreateMasterCacheKey();
        // object obj2 = this.Cache.LoadDataFromCache(key);
        // if (obj2 is int)
        // {
        // return (int)obj2;
        // }
        // return -1;
        // }
        // internal SqlDataSourceCache Cache
        // {
        // get
        // {
        // if (this._cache == null)
        // {
        // this._cache = new SqlDataSourceCache();
        // }
        // return this._cache;
        // }
        // }

        // [WebCategory("Cache"), DefaultValue(0), TypeConverter(typeof(DataSourceCacheDurationConverter)), WebSysDescription("DataSourceCache_Duration")]
        // public virtual int CacheDuration
        // {
        // get
        // {
        // return this.Cache.Duration;
        // }
        // set
        // {
        // this.Cache.Duration = value;
        // }
        // }

        // [WebCategory("Cache"), DefaultValue(0), WebSysDescription("DataSourceCache_ExpirationPolicy")]
        // public virtual DataSourceCacheExpiry CacheExpirationPolicy
        // {
        // get
        // {
        // return this.Cache.ExpirationPolicy;
        // }
        // set
        // {
        // this.Cache.ExpirationPolicy = value;
        // }
        // }

        // [DefaultValue(""), WebSysDescription("DataSourceCache_KeyDependency"), WebCategory("Cache")]
        // public virtual string CacheKeyDependency
        // {
        // get
        // {
        // return this.Cache.KeyDependency;
        // }
        // set
        // {
        // this.Cache.KeyDependency = value;
        // }
        // }
        // [WebSysDescription("SqlDataSourceCache_SqlCacheDependency"), DefaultValue(""), WebCategory("Cache")]
        // public virtual string SqlCacheDependency
        // {
        // get
        // {
        // return this.Cache.SqlCacheDependency;
        // }
        // set
        // {
        // this.Cache.SqlCacheDependency = value;
        // }
        // }
        // internal void SaveTotalRowCountToCache(int totalRowCount)
        // {
        // string key = this.CreateMasterCacheKey();
        // this.Cache.SaveDataToCache(key, totalRowCount);
        // }
        // internal void SaveDataToCache(int startRowIndex, int maximumRows, object data)
        // {
        // string key = this.CreateCacheKey(startRowIndex, maximumRows);
        // string str2 = this.CreateMasterCacheKey();
        // if (this.Cache.LoadDataFromCache(str2) == null)
        // {
        // this.Cache.SaveDataToCache(str2, -1);
        // }
        // CacheDependency dependency = new CacheDependency(0, new string[0], new string[] { str2 });
        // this.Cache.SaveDataToCache(key, data, dependency);
        // }
        // [WebSysDescription("DataSourceCache_Enabled"), WebCategory("Cache"), DefaultValue(false)]
        // public virtual bool EnableCaching
        // {
        // get
        // {
        // return this.Cache.Enabled;
        // }
        // set
        // {
        // this.Cache.Enabled = value;
        // }
        // }
        protected override DataSourceView GetView(string viewName)
        {
            if (!string.IsNullOrEmpty(viewName) && !string.Equals(viewName, DefaultViewName, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException(this.ID + ": View " + viewName + " inválida.", "viewName");
            }

            return this.GetView();
        }

        protected override ICollection GetViewNames()
        {
            if (this._viewNames == null)
            {
                this._viewNames = new[] { DefaultViewName };
            }

            return this._viewNames;
        }

        protected override void LoadViewState(object savedState)
        {
            var pair = (Pair)savedState;
            if (savedState == null)
            {
                base.LoadViewState(null);
            }
            else
            {
                base.LoadViewState(pair.First);
                if (pair.Second != null)
                {
                    ((IStateManager)this.GetView()).LoadViewState(pair.Second);
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.Page != null)
            {
                this.Page.LoadComplete += this.LoadCompleteEventHandler;
            }
        }

        protected override object SaveViewState()
        {
            var pair = new Pair();
            pair.First = base.SaveViewState();
            if (this._view != null)
            {
                pair.Second = ((IStateManager)this._view).SaveViewState();
            }

            if ((pair.First == null) && (pair.Second == null))
            {
                return null;
            }

            return pair;
        }

        protected override void TrackViewState()
        {
            base.TrackViewState();
            if (this._view != null)
            {
                ((IStateManager)this._view).TrackViewState();
            }
        }

        private TTableDataView GetView()
        {
            if (this._view == null)
            {
                this._view = new TTableDataView(this, DefaultViewName, this.Context);
                if (base.IsTrackingViewState)
                {
                    ((IStateManager)this._view).TrackViewState();
                }
            }

            return this._view;
        }

        private void LoadCompleteEventHandler(object sender, EventArgs e)
        {
            this.SqlWhereParameters.UpdateValues(this.Context, this);
            this.SelectMethodParameters.UpdateValues(this.Context, this);
        }
    }
}