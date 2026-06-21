namespace Techne
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Web.UI;
    using System.Xml;
    using Techne.Controls;
    using Techne.Library;
    using Techne.Web;

    public abstract class TPage : Page
    {
        public const string IDIOMADEFAULT = "pt-BR";

        /// <summary>
        ///   Se true, preenche no OnInit() as variáveis de contexto a partir dos valores dos parâmetros da página.
        /// </summary>
        protected bool compatibilityMode = true;

        private const string NumericScriptUrl = "~/Scripts/Numeric.js";

        private const string TPageScriptUrl = "~/Scripts/TPage.js";

        private readonly ArrayList numericFields = new ArrayList();

        private readonly string pvPageCaption = string.Empty;

        private NameObjectCollection allParameters;

        private string connection = string.Empty;

        private NameDbObjectCollection parameters;

        private string pvPageMessage = string.Empty;

        private string pvPageTitle = string.Empty;

        private bool switchPostback = true;

        private Type[] types = new Type[0];

        /// <summary>
        ///   Parâmetros passados à página.
        ///   A declaração desses parâmetros é feita via método estático GetUrl().
        /// </summary>
        [Browsable(false)]
        public NameObjectCollection AllParameters
        {
            get
            {
                return this.allParameters;
            }
        }

        [Browsable(false)]
        public Type AppContextClass { get; private set; }

        [Browsable(false)]
        public virtual string ApplicationName
        {
            get
            {
                var app = HttpContext.Current.ApplicationInstance as TechneHttpApplication;
                if (app == null)
                {
                    throw new InvalidOperationException("A aplicação não é derivada de " + typeof(TechneHttpApplication).FullName + ". " +
                                                        "Crie o override de TPage.ApplicationName.");
                }

                return app.ApplicationName;
            }
        }

        /// <summary>
        ///   Controles como TDropDownList e TSearch que possuem propriedade Connection
        ///   não informada utilizarão esta propriedade.
        /// </summary>
        [Category("Techne"), DefaultValue("")]
        public virtual string Connection
        {
            get
            {
                return this.connection;
            }

            set
            {
                this.connection = value == null ? string.Empty : value.Trim();
            }
        }

        public virtual string CssFilepath
        {
            get
            {
                return "~/" + this.ApplicationName + "Web.css";
            }
        }

        [Browsable(false)]
        public virtual string PageMessage
        {
            get
            {
                return this.pvPageMessage;
            }
        }

        [Category("Techne")]
        public virtual string PageTitle
        {
            get
            {
                return this.pvPageTitle;
            }

            set
            {
                this.pvPageTitle = value;
            }
        }

        /// <summary>
        ///   Parâmetros DbObject e similares passados à página.
        ///   A declaração desses parâmetros é feita via método estático GetUrl().
        ///   Para obter parâmetros não-DbObject, utilize a propriedade AllParameters.
        /// </summary>
        [Browsable(false)]
        public NameDbObjectCollection Parameters
        {
            get
            {
                return this.parameters;
            }
        }

        public TPermission Permission
        {
            get
            {
                var application = HttpContext.Current.ApplicationInstance as TechneHttpApplication;
                if (application == null)
                {
                    return null;
                }

                return application.GetPagePermission(this.Request);
            }
        }

        /// <summary>
        ///   Página chamadora.
        ///   O método utilizado para navegação para a página corrente deve possuir NavReturnAttribute setada para true,
        ///   caso contrário devolverá string.Empty (nunca devolve null).
        /// </summary>
        [Browsable(false)]
        public string ReturnUrl
        {
            get
            {
                var returnUrl = this.Request["returnUrl"];

                if (string.IsNullOrEmpty(returnUrl))
                {
                    return string.Empty;
                }

                return returnUrl;
            }
        }

        internal IList NumericFields
        {
            get
            {
                return this.numericFields;
            }
        }

        protected new bool DesignMode
        {
            get
            {
                return this.Site != null && this.Site.DesignMode;
            }
        }

        protected bool SwitchPostback
        {
            get
            {
                return this.switchPostback;
            }

            set
            {
                this.switchPostback = value;
            }
        }

        private bool IsHelpRequest
        {
            get
            {
                return string.Compare(this.Request.QueryString.ToString(), "help", true) == 0;
            }
        }

        public static List<Control> GetAllITControls(List<Control> controls, Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is ITControl)
                {
                    controls.Add(c);
                }

                if (c.HasControls())
                {
                    controls = GetAllITControls(controls, c);
                }
            }

            return controls;
        }

        /// <summary>
        ///   Devolve a propriedade PageCaption se foi informada.
        ///   Caso contrário, utiliza o atributo Caption fornecido na classe da página.
        /// </summary>
        public string GetPageCaption()
        {
            var pageCaption = this.pvPageCaption;

            // Se o título não foi informado em TPage, busca o atributo Title.
            if (string.IsNullOrEmpty(pageCaption))
            {
                pageCaption = GetPageCaption(HelpData.GetClassType(this), Thread.CurrentThread.CurrentCulture.Name);
            }

            return pageCaption;
        }

        /// <summary>
        ///  Devolve o atributo Image associado à página.
        ///  Se o atributo não tiver sido definido na página, devolve string vazia.
        /// </summary>
        public string GetPageImage()
        {
            return GetPageImage(HelpData.GetClassType(this));
        }

        /// <summary>
        ///   Devolve a propriedade PageTitle se foi informada.
        ///   Caso contrário, utiliza o atributo Title fornecido na classe da página.
        /// </summary>
        public string GetPageTitle()
        {
            var pageTitle = this.pvPageTitle;

            // Se o título não foi informado em TPage, busca o atributo Title.
            if (string.IsNullOrEmpty(pageTitle))
            {
                pageTitle = GetPageTitle(HelpData.GetClassType(this), Thread.CurrentThread.CurrentCulture.Name);
            }

            return pageTitle;
        }

        public Type GetParameterType(string parameterName)
        {
            if (parameterName == null)
            {
                throw new ArgumentNullException();
            }

            var index = this.allParameters.IndexOfKey(parameterName);
            if (index < 0)
            {
                throw new ArgumentException("O parâmetro '" + parameterName + "' não foi informado para a página.");
            }

            return this.types[index];
        }

        public virtual void HelpInit(HelpData help)
        {
            // A ser overriden por cada uma das páginas da aplicação.
        }

        public void Redirect(string url, string message)
        {
            if (HttpContext.Current.Session != null)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    this.Session["PageMessage"] = message;
                }
            }

            this.Response.Redirect(url);
        }

        public void Redirect(string url)
        {
            this.Redirect(url, null);
        }

        internal static string GetDefaultName(Type type)
        {
            var urlAtts = (NavUrlAttribute[])type.GetCustomAttributes(typeof(NavUrlAttribute), true);
            if (urlAtts.Length == 0)
            {
                return type.FullName;
            }

            var url = TUtil.TranslateRelativeUrl(urlAtts[0].Url);
            return Path.GetFileNameWithoutExtension(url);
        }

        /// <summary>
        ///   Determina o valor do atributo Caption na classe fornecida. Se Caption não for encontrado, busca
        ///   o valor do atributo Title. Se também não for encontrado, usa GetDefaultName().
        /// </summary>
        internal static string GetPageCaption(Type type, string idioma)
        {
            try
            {
                string text = null;

                try
                {
                    text = ControlTextAttribute.GetControlText(type, IDIOMADEFAULT);
                }
                catch
                {
                }

                if (text == null)
                {
                    try
                    {
                        text = TitleAttribute.GetPageTitle(type, IDIOMADEFAULT);
                    }
                    catch
                    {
                    }
                }

                if (text == null)
                {
                    return GetDefaultName(type);
                }

                return text;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///   Devolve o atributo Image associado à página.
        ///   Se o atributo não tiver sido definido na página, devolve string vazia.
        /// </summary>
        internal static string GetPageImage(Type type)
        {
            var url = ImageAttribute.GetUrl(type);
            return url ?? string.Empty;
        }

        /// <summary>
        ///   Determina o valor do atributo Title na classe fornecida. Se Title não for encontrado, busca
        ///   o valor do atributo Caption. Se também não for encontrado, usa GetDefaultName().
        /// </summary>
        internal static string GetPageTitle(Type type, string idioma)
        {
            if (type == null)
            {
                throw new ArgumentNullException();
            }

            try
            {
                string text = null;

                try
                {
                    text = TitleAttribute.GetPageTitle(type, IDIOMADEFAULT);
                }
                catch
                {
                }

                if (text == null)
                {
                    try
                    {
                        text = ControlTextAttribute.GetControlText(type, IDIOMADEFAULT);
                    }
                    catch
                    {
                    }
                }

                if (text == null)
                {
                    return GetDefaultName(type);
                }

                return text;
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static string GetPageUrl(Type type)
        {
            var urls = type.GetCustomAttributes(typeof(NavUrlAttribute), true);

            if (urls.Length > 1)
            {
                throw new InvalidOperationException("Foi encontrado mais de um atributo NavUrl.");
            }

            if (urls.Length == 1)
            {
                return ((NavUrlAttribute)urls[0]).Url;
            }

            // urls.Length == 0
            return string.Empty;
        }

        protected override object LoadPageStateFromPersistenceMedium()
        {
            var state = base.LoadPageStateFromPersistenceMedium();

            return state;
        }

        protected override void OnError(EventArgs e)
        {
            // Para contornar o bug do smartnavigation+customError
            string webconfig, defaultRedirect = null, mode;

            if (this.ErrorPage == null
                || this.ErrorPage.Trim() == string.Empty)
            {
                try
                {
                    webconfig = this.Server.MapPath(this.Request.ApplicationPath + "/web.config");
                    var xdoc = new XmlDocument();
                    var treader = new StreamReader(webconfig);
                    xdoc.Load(treader);
                    var n = xdoc.GetElementsByTagName("customErrors");
                    if (n.Count > 0)
                    {
                        mode = n[0].Attributes["mode"].Value;
                        if (mode == "On")
                        {
                            defaultRedirect = n[0].Attributes["defaultRedirect"].Value;
                            if (defaultRedirect.Substring(0, 1) != "/"
                                && defaultRedirect.Substring(0, 1) != "\\")
                            {
                                defaultRedirect = this.Request.ApplicationPath + "/" + defaultRedirect;
                            }
                        }
                    }

                    if ((this.Request.Url.AbsolutePath + "?" + this.Request.Url.Query).IndexOf(defaultRedirect.ToLower()) < 0)
                    {
                        this.ErrorPage = defaultRedirect;
                    }
                }
                catch
                {
                }
            }

            base.OnError(e);
        }

        protected override void OnInit(EventArgs args)
        {
            if (this.DesignMode)
            {
                base.OnInit(args);
                return;
            }

            // pega permissões da página e joga no thread
            TPermission.ThreadPermission = this.Permission;

            if (!this.Page.ClientScript.IsClientScriptBlockRegistered(typeof(TPage), "Help"))
            {
                this.Page.ClientScript.RegisterClientScriptBlock(typeof(TPage), "Help",
                    "<script language=\"javascript\" " +
                    "src=\"" + TUtil.TranslateRelativeUrl("~/scripts/Help.js", this) + "\">" +
                    "</script>");
            }

            if (this.IsHelpRequest)
            {
                var help = new HelpData(this);
                this.HelpInit(help);
                help.Render();
                this.Response.End();
            }
            else
            {
                // Monta a collection de parâmetros da página
                Navigation.GetPageParameters(this, out this.allParameters, out this.parameters, out this.types);

                base.OnInit(args);

                if (!this.IsPostBack)
                {
                    ResetValueRecursive(this);

                    if (HttpContext.Current.Session != null
                        && this.Session["PageMessage"] != null)
                    {
                        this.pvPageMessage = (string)this.Session["PageMessage"];
                        this.Session.Remove("PageMessage");
                    }
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (this.switchPostback)
            {
                if (!this.ClientScript.IsClientScriptBlockRegistered(typeof(TPage), "TPage"))
                {
                    this.Page.ClientScript.RegisterClientScriptBlock(typeof(TPage), "TPage",
                        "<script language=\"JavaScript\" " +
                        "src=\"" + TUtil.TranslateRelativeUrl(TPageScriptUrl, this) + "\">" +
                        "</script>\r\n" +
                        "<script language=\"JavaScript\">\r\n" +
                        "  switchPostBack();\r\n" +
                        "</script>\r\n"
                        );

                    // Esta função é chamada somente para garantir que __doPostBack() seja gerado.
                    this.ClientScript.GetPostBackEventReference(this, string.Empty);

                    // Adiciona campo escondido para guardar o último foco
                    this.ClientScript.RegisterHiddenField("__lastfocus", this.Request.Form["__lastfocus"]);
                }

                this.Page.ClientScript.RegisterOnSubmitStatement(typeof(TPage), "OnSubmitScript", "checkSubmit();");
            }

            this.InitPageTitle();
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            TPermission.ThreadPermission = null;
        }

        /// <summary>
        ///   Executa Response.Redirect para a página chamadora.
        ///   O método utilizado para navegação para a página corrente deve possuir NavReturnAttribute setada para true,
        ///   caso contrário nada será feito.
        /// </summary>
        protected void PageReturn()
        {
            var returnUrl = this.ReturnUrl;
            if (returnUrl.Length > 0)
            {
                this.Response.Redirect(returnUrl);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.Page.ClientScript.IsStartupScriptRegistered(typeof(TPage), "NumericFields"))
            {
                this.Page.ClientScript.RegisterStartupScript(typeof(TPage), "NumericFields", this.GetNumericFieldsInitializationScript());
            }

            base.Render(writer);
        }

        /// <summary>
        ///   Este método deve ser executado antes de TPage.OnInit() se existir classe derivada
        ///   de Techne.Web.AppContextBase na aplicação.
        /// </summary>
        /// <param name = "appContextClass">
        ///   typeof([classe]), onde [classe] é a classe derivada de Techne.Web.AppContextBase
        /// </param>
        protected void SetAppContextClass(Type appContextClass)
        {
            this.AppContextClass = appContextClass;
        }

        /// <summary>
        ///   Chama ResetValue() de todos os TControl's sob o controle informado
        ///   que não estejam sob managers.
        /// </summary>
        private static void ResetValueRecursive(Control rootControl)
        {
            var controls = new List<Control>();

            GetAllITControls(controls, rootControl);

            foreach (var control in controls)
            {
                var itControl = (ITControl)control;

                if (itControl != null
                    && itControl.Manager == null)
                {
                    itControl.ResetValue();
                }
            }
        }

        private string GetNumericFieldsInitializationScript()
        {
            var b = new StringBuilder();

            foreach (TTextBox txt in this.numericFields)
            {
                b.Append("numericPrepare('" + txt.ClientID + "');\n");
            }

            if (b.Length == 0)
            {
                return string.Empty;
            }

            return "<script language=javascript src='" + TUtil.TranslateRelativeUrl(NumericScriptUrl) + "'>" + "</script>\n" +
                   "<script language=javascript>\n" + b + "\n</script>";
        }

        private void InitPageTitle()
        {
            var pageTitle = this.GetPageTitle();
            var pageCaption = this.GetPageCaption();

            if (string.IsNullOrEmpty(pageTitle)
                || !(this.Controls[0] is LiteralControl))
            {
                return;
            }

            var lit = (LiteralControl)this.Controls[0];
            var s = lit.Text;
            var sl = s.ToLower();
            var ti = sl.IndexOf("<title>");
            var tf = sl.IndexOf("</title>");

            if (ti < tf
                && tf - ti < 100)
            {
                lit.Text = s.Substring(0, ti + 7) + pageTitle + s.Substring(tf);
            }
            else
            {
                var hi = sl.IndexOf("<head>");
                var hf = sl.IndexOf("</head>");
                if (hi < hf)
                {
                    lit.Text = s.Substring(0, hi + 6) + "\n<title>" + pageTitle + "</title>\n" + s.Substring(hi + 6);
                }
            }
        }
    }
}