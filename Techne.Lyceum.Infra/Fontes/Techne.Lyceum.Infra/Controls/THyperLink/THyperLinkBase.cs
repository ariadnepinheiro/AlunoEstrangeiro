using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace Techne.Controls
{
    internal abstract class THyperLinkBase : TLinkBase
    {
        public const bool ReturnEnabled_Def = false;

        public const bool ShowWhenUnauthorized_Def = true;

        private readonly TLinkParameterCollection parameters;

        private string baseNavigateUrl;

        private string imageUrlUnauthorized;

        private NavInfoClass navInfo;

        private bool returnEnabled;

        private string target;

        protected THyperLinkBase()
        {
            this.EnableAuthorization = true;
            this.ImageUrlUnauthorized = string.Empty;
            this.BaseNavigateUrl = string.Empty;
            this.parameters = new TLinkParameterCollection();
            this.BaseReturnEnabled = ReturnEnabled_Def;
            this.ShowWhenUnauthorized = ShowWhenUnauthorized_Def;
            this.Target = string.Empty;
        }

        public override bool Enabled
        {
            get
            {
                if (InDesignMode(this))
                {
                    return true;
                }

                if (!base.Enabled)
                {
                    return false;
                }

                if (this.NavInfo != null && !this.NavInfo.Authorized && this.NavInfo.NavigateUrl.Length > 0)
                {
                    this.SetDisableCause("Não autorizado");
                    return false;
                }

                return this.NavInfo != null && this.NavInfo.Enabled;
            }

            set
            {
                /* Nada faz */
            }
        }

        [DefaultValue(""), Editor(typeof (ImageUrlEditor), typeof (UITypeEditor)), Category("Techne"), Description("Imagem a ser mostrada quando o usu\x00e1rio n\x00e3o tem permiss\x00e3o em NavigateUrl. Esta propriedade n\x00e3o \x00e9 afetada pela propriedade ShowWhenAuthorized. Tem prioridade sobre a propriedade Text.")]
        public string ImageUrlUnauthorized
        {
            get
            {
                return this.imageUrlUnauthorized;
            }

            set
            {
                this.imageUrlUnauthorized = value == null ? string.Empty : value;
            }
        }

        [DefaultValue(true), Description("Mostra o link desabilitado quando o usu\x00e1rio n\x00e3o possui permiss\x00e3o em NavigateUrl."), Category("Techne")]
        public bool ShowWhenUnauthorized { get; set; }

        internal bool EnableAuthorization { get; set; }

        protected virtual string BaseNavigateUrl
        {
            get
            {
                return this.baseNavigateUrl;
            }

            set
            {
                this.baseNavigateUrl = value == null ? string.Empty : value;
            }
        }

        protected virtual bool BaseReturnEnabled
        {
            get
            {
                return this.returnEnabled;
            }

            set
            {
                this.returnEnabled = value;
            }
        }

        protected virtual string Target
        {
            get
            {
                return this.target;
            }

            set
            {
                this.target = value == null ? string.Empty : value;
            }
        }

        protected TLinkParameterCollection BaseParameters
        {
            get
            {
                return this.parameters;
            }
        }

        private NavInfoClass NavInfo
        {
            get
            {
                if (this.navInfo == null)
                {
                    try
                    {
                        this.navInfo = new NavInfoClass(this);
                        this.SetDisableCause(this.navInfo.DisableCause);
                    }
                    catch (Exception exc)
                    {
                        this.SetDisableCause(exc.Message);
                    }
                }

                return this.navInfo;
            }
        }

        internal override string GetImageUrl()
        {
            if ((!this.Enabled && (this.ImageUrlUnauthorized.Length != 0)) && !TControl.InDesignMode(this))
            {
                return this.ImageUrlUnauthorized;
            }

            return base.GetImageUrl();
        }

        protected override WebControl CreateInternalControl()
        {
            if (this.Enabled)
            {
                var hyp = new HyperLink();

                // Se o controle estiver dentro de uma grid,
                // seta NavigateUrl para que ele fique com cara de link.
                if (InDesignMode(this))
                {
                    hyp.NavigateUrl = "#";
                }

                return hyp;
            }

            if (this.ShowWhenUnauthorized)
            {
                return base.CreateInternalControl();
            }

            return null;
        }

        protected override string GetToolTip(bool convertQuestionToEmpty)
        {
            return base.GetToolTip(convertQuestionToEmpty);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!TControl.InDesignMode(this) && this.Enabled)
            {
                if (this.BaseReturnEnabled)
                {
                    this.Page.ClientScript.RegisterHiddenField("_myUrl", HttpUtility.UrlEncode(this.Page.Request.RawUrl));
                }

                var hyp = (HyperLink)this.InternalControl;

                // Copia os atributos setados em TControl até o momento (em TControl.OnPreRender(): DBValue, DataType)
                hyp.CopyBaseAttributes(this);

                hyp.NavigateUrl = this.NavInfo.NavigateUrl;
                hyp.ToolTip = this.GetToolTip(true);

                if (this.Target.Length > 0)
                {
                    hyp.Target = this.Target;
                }

                hyp.Attributes["OnClick"] = "substTLinkBaseHref('" + this.ClientID + "');";
                hyp.Attributes["Params"] = StrLib.EnumerableToStr(TechLib.EnumerableItemProperty(this.BaseParameters, "Name"), ",");
                hyp.Attributes["Values"] = StrLib.EnumerableToStr(this.NavInfo.Values, ",");
                hyp.Attributes["Return"] = this.BaseReturnEnabled.ToString();
            }
        }

        protected override void RenderControlViewMode(HtmlTextWriter writer)
        {
            if (this.InternalControl != null)
            {
                // InternalControl é null quando !Enabled e !ShowWhenUnauthorized,
                // de acordo com CreateInternalControl().
                base.RenderControlViewMode(writer);
            }
        }

        private class NavInfoClass
        {
            private readonly bool authorized;

            private readonly string disableCause;

            private readonly string navigateUrl;

            private readonly THyperLinkBase parent;

            private readonly Type[] types;

            private readonly string[] values;

            public NavInfoClass(THyperLinkBase parent)
            {
                if (parent == null)
                {
                    throw new ArgumentNullException();
                }

                this.parent = parent;

                this.navigateUrl = TUtil.TranslateRelativeUrl(parent.BaseNavigateUrl, parent);
                if (this.navigateUrl.Length > 0)
                {
                    this.authorized = InDesignMode(parent) || !parent.EnableAuthorization || TechneAuthorization.IsUrlAuthorized(this.navigateUrl);
                }
                else
                {
                    this.authorized = false;
                    this.disableCause = "A propriedade BaseNavigateUrl não foi informada";
                }

                // Transforma todos os $campo$ em #controle#. O script substTLinkBaseHref() só entende este formato.
                // Trata também @parametro@, substituindo pelo valor efetivo.
                var count = InDesignMode(parent) ? 0 : parent.BaseParameters.Count;
                this.values = new string[count];
                this.types = new Type[count];
                {
                    var disableCauses = new string[count];
                    for (var i = 0; i < count; i++)
                    {
                        this.GetTControlReference(i, out this.values[i], out this.types[i], out disableCauses[i]);
                    }

                    // Isso é feito no lugar de EnumerableToStr(disableCauses, "\r\n")
                    // porque as mensagens vazias não devem ser concatenadas.
                    var b = new StringBuilder();
                    foreach (var s in disableCauses)
                    {
                        if (s.Length > 0)
                        {
                            if (b.Length > 0)
                            {
                                b.Append("\r\n");
                            }

                            b.Append(s);
                        }
                    }

                    this.disableCause = b.ToString();
                }
            }

            public bool Authorized
            {
                get
                {
                    return this.authorized;
                }
            }

            public string DisableCause
            {
                get
                {
                    return this.disableCause;
                }
            }

            public bool Enabled
            {
                get
                {
                    return this.NavigateUrl.Length > 0 && this.Authorized && this.DisableCause.Length == 0;
                }
            }

            public string NavigateUrl
            {
                get
                {
                    return this.navigateUrl;
                }
            }

            public Type[] Types
            {
                get
                {
                    return this.types;
                }
            }

            public string[] Values
            {
                get
                {
                    return this.values;
                }
            }

            /// <summary>
            ///   Transforma $campo$ em #controle# e @parametro@ em valor.
            /// </summary>
            /// <param name = "error">
            ///   Quando não consegue resolver a referência, indica o motivo.
            ///   Devolve string vazia caso contrário.
            /// </param>
            private void GetTControlReference(int parameterIndex, out string newParameter, out Type type, out string error)
            {
                error = string.Empty;
                var parameter = this.parent.BaseParameters[parameterIndex].Value.Trim();

                if (parameter.Length >= 2)
                {
                    var first = parameter[0];
                    var last = parameter[parameter.Length - 1];

                    if (first == '#' && last == '#')
                    {
                        

                        if (parameter.Length < 3)
                        {
                            throw new InvalidOperationException("O nome do controle não foi informado.");
                        }

                        var controlName = parameter.Substring(1, parameter.Length - 2);

                        Control findControl = null;
                        var scope = this.parent.NamingContainer;
                        do
                        {
                            findControl = scope.FindControl(controlName);
                            if (findControl != null)
                            {
                                break;
                            }

                            scope = scope.NamingContainer;
                        }
                        while (scope != null);

                        if (findControl == null)
                        {
                            // Se o controle não foi encontrado, podemos estar em uma das situações:
                            // 1) o controle não foi renderizado (ele pode estar dentro de um RecordManager que não
                            // contém registro corrente, por exemplo);
                            // 2) o programador errou.
                            // Como não há possibilidade de distingüir as situações, o link será simplesmente desabilitado.
                            error = "Não foi possível resolver #" + controlName + "#";
                        }
                        else
                        {
                            if (!(findControl is ITControl))
                            {
                                throw new InvalidOperationException("O controle " + controlName + " informado na propriedade " + this.parent.UniqueID + ".NavigationParameters não é ITControl.");
                            }

                            newParameter = "#" + findControl.ClientID + "#";
                            type = DbObject.ToType(((ITControl)findControl).DataType);
                            return;
                        }

                        
                    }
                    else if (first == '$' && last == '$')
                    {
                        

                        var recordContainer = this.parent.RecordContainer;
                        if (recordContainer == null)
                        {
                            throw new InvalidOperationException("O controle não está em um IRecordContainer. A referência $[campo]$ em " + this.parent.UniqueID + ".NavigationParameters é inválida.");
                        }

                        if (parameter.Length < 3)
                        {
                            throw new InvalidOperationException("O nome do campo não foi informado.");
                        }

                        var fieldName = parameter.Substring(1, parameter.Length - 2);

                        ITControl fieldControl;
                        try
                        {
                            fieldControl = TControl.GetColumnControl((Control)recordContainer, fieldName);
                        }
                        catch
                        {
                            fieldControl = null;
                        }

                        if (fieldControl == null)
                        {
                            newParameter = StrLib.ToStr(recordContainer[fieldName]);

// Usar 'Manager' aqui é a mesma coisa do que usar 'TControl.GetManager(recordContainer)'?
                            var dbType = DbObject.ToDbType(this.parent.Manager.Table.Columns[fieldName].DataType);
                            type = DbObject.ToType(dbType);
                            return;
                        }

                        newParameter = "#" + ((Control)fieldControl).ClientID + "#";
                        type = DbObject.ToType(fieldControl.DataType);
                        return;

                        
                    }
                    else if (first == '@' && last == '@')
                    {
                        

                        var page = this.parent.Page as TPage;
                        if (page == null)
                        {
                            throw new InvalidOperationException("A página deve derivar de TPage para que valores no formato @parametro@ sejam aceitos pela propriedade " + this.parent.UniqueID + ".NavigationParameters.");
                        }

                        var pageParameter = parameter.Substring(1, parameter.Length - 2);
                        object pageParameterValue = page.Parameters[pageParameter];

                        if (pageParameterValue == null)
                        {
                            throw new InvalidOperationException("O parâmetro '" + pageParameter + "' não foi passado para a página.");
                        }

                        // Verifica se o parâmetro referenciado pode ser convertido para DbObject.
                        if (!(pageParameterValue is IDbObject))
                        {
                            throw new InvalidOperationException("Referência a parâmetro cujo tipo não é IDbObject.");
                        }

                        newParameter = StrLib.ToStr((DbObject)pageParameterValue);
                        type = page.GetParameterType(pageParameter);
                        return;

                        
                    }
                }

                newParameter = parameter;
                type = this.parent.BaseParameters[parameterIndex].DataType;
            }
        }
    }
}