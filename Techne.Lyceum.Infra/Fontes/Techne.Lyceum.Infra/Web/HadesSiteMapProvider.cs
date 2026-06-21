using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Web;
using Techne.Web.Hades;

namespace Techne.Web
{
    public class HadesSiteMapNode : SiteMapNode
    {
        public HadesSiteMapNode(SiteMapProvider provider, string key)
            : base(provider, key)
        {
        }

        public HadesSiteMapNode(SiteMapProvider provider, string key, string url)
            : base(provider, key, url)
        {
        }

        public HadesSiteMapNode(SiteMapProvider provider, string key, string url, string title)
            : base(provider, key, url, title)
        {
        }

        public HadesSiteMapNode(SiteMapProvider provider, string key, string url, string title, string description)
            : base(provider, key, url, title, description)
        {
        }

        public HadesSiteMapNode(SiteMapProvider provider, string key, string url, string title, string description, IList roles, NameValueCollection attributes, NameValueCollection explicitResourceKeys, string implicitResourceKey)
            : base(provider, key, url, title, description, roles, attributes, explicitResourceKeys, implicitResourceKey)
        {
        }

        public string ImageUrl
        {
            get
            {
                if (this.Attributes != null && this.Attributes["imageurl"] != null)
                {
                    return this.Attributes["imageurl"];
                }
                else
                {
                    return string.Empty;
                }
            }

            set
            {
                if (this.Attributes == null)
                {
                    this.Attributes = new NameValueCollection();
                }

                this.Attributes["imageurl"] = value;
            }
        }
    }

    public class HadesSiteMapProvider : StaticSiteMapProvider
    {
        private const string _errmsg2 = "Duplicate node ID";

        private const string _errmsg3 = "Missing parent ID";

        private const string _errmsg4 = "Invalid parent ID";

        private static readonly List<string> _cacheKeys = new List<string>();

        private readonly object _lock = new object();

        private readonly Dictionary<string, SiteMapNode> _nodes = new Dictionary<string, SiteMapNode>(16);

        private string _appName;

        private int _cacheDuration;

        private string _menuName = "Lyceum";

        private SiteMapNode _root;

        protected string CacheKey
        {
            get
            {
                return "HadesSiteMapProvider_" + this.Name + this._menuName + this._appName;
            }
        }

        protected bool IsExpired
        {
            get
            {
                if (HttpContext.Current.Cache[this.CacheKey] is string)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            set
            {
                if (value == false)
                {
                    HttpContext.Current.Cache.Remove(this.CacheKey);
                    if (this._cacheDuration > 0)
                    {
                        HttpContext.Current.Cache.Add(this.CacheKey, this.CacheKey, null, DateTime.Now.AddMinutes(this._cacheDuration), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                    }
                    else
                    {
                        HttpContext.Current.Cache.Add(this.CacheKey, this.CacheKey, null, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                    }

                    if (!_cacheKeys.Contains(this.CacheKey))
                    {
                        _cacheKeys.Add(this.CacheKey);
                    }
                }
                else
                {
                    HttpContext.Current.Cache.Remove(this.CacheKey);
                }
            }
        }

        public override SiteMapNode BuildSiteMap()
        {
            lock (this._lock)
            {
                if (this._root != null && !this.IsExpired)
                {
                    return this._root;
                }

                var mnu = Menu.GetMenu(this._menuName);

                if (mnu != null)
                {
                    // limpa árvore anterior
                    this.Clear();

// Cria Raiz
                    this._root = this.CreateSiteMapNodeFromMenu(mnu);
                    this.AddNode(this._root, null);

                    foreach (var mi in mnu.Items)
                    {
                        var node = this.CreateSiteMapNodeFromMenuItem(mi);
                        var cont = 10;
                        while (cont-- > 0)
                        {
                            try
                            {
                                this.AddNode(node, this.GetParentNode(mi.ParentID));
                                break;
                            }
                            catch
                            {
                                node.Url = node.Url + "#";
                            }
                        }
                    }

                    this.IsExpired = false;
                }

                return this._root;
            }
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            // checa se config existe
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            // atribui default provider name
            if (String.IsNullOrEmpty(name))
            {
                name = "HadesSiteMapProvider";
            }

            this._menuName = config["menuName"];
            this._appName = WebHelper.ApplicationName;

            if (!string.IsNullOrEmpty(config["cacheDuration"]))
            {
                try
                {
                    this._cacheDuration = Int32.Parse(config["cacheDuration"]);
                }
                catch
                {
                    this._cacheDuration = 0;
                }
            }
            else
            {
                this._cacheDuration = 2;
            }

            if (this._cacheDuration < 0)
            {
                this._cacheDuration = 2;
            }

            // atribui descrição
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", StringResource.GetString("HadesSiteMapProvider_Description"));
            }

            // chama Initialize da base
            base.Initialize(name, config);
        }

        public override bool IsAccessibleToUser(HttpContext context, SiteMapNode node)
        {
            var bAccessible = false;
            if (node.Roles == null)
            {
                bAccessible = true;
            }
            else
            {
                foreach (var o in node.Roles)
                {
                    var role = o as string;
                    if (context.User.IsInRole(role))
                    {
                        bAccessible = true;
                        break;
                    }
                }
            }

            return bAccessible;
        }

        internal static void ClearAllCaches()
        {
            if (HttpContext.Current == null)
            {
                return;
            }

            foreach (var key in _cacheKeys)
            {
                try
                {
                    HttpContext.Current.Cache.Remove(key);
                }
                catch
                {
                }
            }
        }

        protected override void Clear()
        {
            // limpa árvore anterior
            foreach (var pair in this._nodes)
            {
                try
                {
                    this.RemoveNode(pair.Value);
                }
                catch
                {
                }
            }

            this._nodes.Clear();
            base.Clear();
        }

        protected override SiteMapNode GetRootNodeCore()
        {
            lock (this._lock)
            {
                this.BuildSiteMap();
                return this._root;
            }
        }

        private SiteMapNode CreateSiteMapNodeFromMenu(HdMenu mi)
        {
            var id = mi.NomeMenu;

            if (this._nodes.ContainsKey(id))
            {
                throw new ProviderException(_errmsg2);
            }

            var node = new HadesSiteMapNode(this, id, null, mi.Descricao, mi.Descricao, null, null, null, null);
            this._nodes.Add(id, node);

            return node;
        }

        private SiteMapNode CreateSiteMapNodeFromMenuItem(HdMenuItem mi)
        {
            var id = mi.ID;

            if (this._nodes.ContainsKey(id))
            {
                throw new ProviderException(_errmsg2);
            }

            var url = mi.Url;
            if (!string.IsNullOrEmpty(url) && url[0] == '~' && this._appName != null && !string.IsNullOrEmpty(mi.SisUrl))
            {
                if (!mi.Sistema.Equals(this._appName, StringComparison.InvariantCultureIgnoreCase))
                {
                    url = mi.SisUrl + url.Substring(1);
                }
            }

            var node = new HadesSiteMapNode(this, id, url, mi.Texto, mi.Texto, mi.Roles, null, null, null);
            node.ImageUrl = mi.ImageUrl;
            this._nodes.Add(id, node);

            return node;
        }

        private SiteMapNode GetParentNode(string parentID)
        {
            if (string.IsNullOrEmpty(parentID))
            {
                return null;
            }

            if (!this._nodes.ContainsKey(parentID))
            {
                throw new ProviderException(_errmsg4);
            }

            return this._nodes[parentID];
        }
    }
}