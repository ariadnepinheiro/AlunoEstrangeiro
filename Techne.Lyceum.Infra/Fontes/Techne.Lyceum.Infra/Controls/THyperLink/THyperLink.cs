using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;

namespace Techne.Controls
{
    internal class THyperLink : THyperLinkBase
    {
        private const string ImageUrl_Def = "?";

        /// <summary>
        ///   Se null: ainda não inicializou navigation; se string.Empty: navigation já foi inicializado;
        ///   caso contrário, indica o motivo do link ter sido desabilitado.
        /// </summary>
        private bool navigInitialized;

        /// <summary>
        ///   Não acessar diretamente. Utilize a propriedade Navig.
        /// </summary>
        private Navigation navigation;

        private string navigationMethod;

        private string navigationParameters;

        public THyperLink()
        {
            this.ImageUrl = ImageUrl_Def;
            this.NavigationMethod = string.Empty;
            this.NavigationParameters = string.Empty;
        }

        public override bool Enabled
        {
            get
            {
                // O base é verificado antes de Navig porque
                // se base for false, Navig não será instanciado.
                return InDesignMode(this) ? true : base.Enabled && this.Navig != null;
            }

            set
            {
                /* Nada faz */
            }
        }

        [DefaultValue("?"), Description("Url da imagem que ser\x00e1 utilizado como link. Informe \"?\" para utilizar a url default da p\x00e1gina chamado (setado pelo atributo Image). Utilize \"~\" para indicar a raiz da aplica\x00e7\x00e3o. A propriedade Text poder\x00e1 ser utilizada simultaneamente. Veja tamb\x00e9m: ImagePosition.")]
        public override string ImageUrl
        {
            get
            {
                return base.ImageUrl;
            }

            set
            {
                base.ImageUrl = value;
            }
        }

        [
            Category("Techne"), 
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
                this.navigationMethod = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(""), 
            Description(
                "Valores a serem passados como parâmetros à página indicada pela propriedade NavigationMethod. " +
                "São permitidas referências a TControls (sintaxe #controle#) e campos (sintaxe $campo$). " +
                "Valores constantes devem ser informados no formato de cultura invariante. " +
                "DBNull deve ser informado como string vazia."
                ), 
        ]
        public string NavigationParameters
        {
            get
            {
                return this.navigationParameters;
            }

            set
            {
                this.navigationParameters = value == null ? string.Empty : value;
            }
        }

        protected override string BaseNavigateUrl
        {
            get
            {
                if (this.Navig != null)
                {
                    return this.Navig.GetUrl();
                }

                return string.Empty;
            }

            set
            {
                /* nada faz */
            }
        }

        protected override bool BaseReturnEnabled
        {
            get
            {
                return this.Navig == null ? false : this.Navig.ReturnEnabled;
            }

            set
            {
                /* nada faz */
            }
        }

        private Navigation Navig
        {
            get
            {
                if (!this.navigInitialized)
                {
                    var parameters = this.NavigationParameters.Trim().Length > 0 ? this.NavigationParameters.Split(',') : new string[0];
                    this.navigation = null;

                    if (this.Page == null)
                    {
                        this.SetDisableCause("O controle não está contido em uma página.");
                    }
                    else if (this.RecordContainer != null && this.RecordContainer.Mode != RecordManagerMode.View)
                    {
                        this.SetDisableCause("O manager " + ((Control)this.RecordContainer.Manager).ID + " está fora do modo View");
                    }
                    else if (this.NavigationMethod.Length == 0)
                    {
                        this.SetDisableCause("A propriedade NavigationMethod não foi informada");
                    }
                    else
                    {
                        Navigation tempNav;
                        try
                        {
                            tempNav = Navigation.Create(this.NavigationMethod);
                        }
                        catch (Exception exc)
                        {
                            this.SetDisableCause(exc.Message);
                            tempNav = null;
                        }

                        if (tempNav != null)
                        {
                            

                            var order = 0;
                            foreach (var parameter in parameters)
                            {
                                ValueReference parameterReference;
                                try
                                {
                                    parameterReference = ValueReference.Parse(parameter);
                                }
                                catch (Exception exc)
                                {
                                    throw new ApplicationException("Existe um problema no valor do parâmetro '" + tempNav.ParameterNames[order] + "' informado na propriedade NavigationParameters: " + exc.Message);
                                }

                                order++;

// Não valida referências que não são ControlReference
                                if (!(parameterReference is ControlReference))
                                {
                                    continue;
                                }

                                var ctl = ((ControlReference)parameterReference).GetControl(this.NamingContainer) as TControl;

// Não valida parâmetros que se referem a controles que não estão dentro de RecordContainers
                                if (ctl == null || ctl.RecordContainer == null)
                                {
                                    continue;
                                }

                                var mgr = ctl.RecordContainer as RecordManager;

                                // DESABILITA: Container do controle referenciado está em modo Edit ou AddNew
                                if (ctl.RecordContainer.Mode != RecordManagerMode.View)
                                {
                                    tempNav = null;
                                    this.SetDisableCause(
                                        "Um dos parâmetros é um controle (" + parameterReference.Reference + ") contido num " +
                                        "manager " + (mgr != null ? "(" + mgr.ID + ") " : string.Empty) +
                                        "fora do modo View"
                                        );
                                    break;
                                }

                                // DESABILITA: Container do controle referenciado está em modo View, mas não contém registro
                                if (ctl.RecordContainer.PrimaryKeyValues == null)
                                {
                                    tempNav = null;
                                    this.SetDisableCause(
                                        "Um dos parâmetros é um controle (" + parameterReference.Reference + ") contido num " +
                                        "manager " + (mgr != null ? "(" + mgr.ID + ") " : string.Empty) +
                                        "sem registro"
                                        );
                                    break;
                                }

                                // Se chegou aqui, o controle referenciado está num Container com registro e em modo View
                                // A validação deste parâmetro foi bem sucedida.
                            }
                        }

                        this.navigation = tempNav;
                    }

                    if (this.navigation != null)
                    {
                        // Inicializa BaseParameters.
                        for (var i = 0; i < this.navigation.ParameterTypes.Length && i < parameters.Length; i++)
                        {
                            this.BaseParameters.Add(
                                this.navigation.ParameterNames[i], this.navigation.ParameterTypes[i], 
                                parameters[i].Trim()
                                );
                        }

                        var fixedParameters = this.navigation.FixedParameters;
                        for (var i = 0; i < fixedParameters.Count; i++)
                        {
                            this.BaseParameters.Add(fixedParameters.GetKey(i), typeof (string), fixedParameters.GetValue(i));
                        }
                    }

                    this.navigInitialized = true;
                }

                return this.navigation;
            }
        }

        internal override string GetAutoText()
        {
            if (this.NavigationMethod.Length == 0)
            {
                throw new InvalidOperationException("A propriedade NavigationMethod não foi informada.");
            }

            return TPage.GetPageCaption(Navigation.GetType(this.NavigationMethod), Thread.CurrentThread.CurrentCulture.Name);
        }

        /// <summary>
        ///   Devolve ImageUrl, a não ser que seu valor seja "?".
        ///   Neste caso, busca o atributo Image da página destino.
        /// </summary>
        internal override string GetImageUrl()
        {
            var url = base.GetImageUrl();

            if (url != "?")
            {
                return url;
            }

            // Se ImageUrl == "?", utiliza o atributo Image associado à página destino.
            if (this.Navig == null)
            {
                return string.Empty;
            }

            return TPage.GetPageImage(this.Navig.PageType);
        }

        protected override string GetToolTip(bool convertQuestionToEmpty)
        {
            var toolTip = base.GetToolTip(false);

            if (toolTip == "?")
            {
                if (this.NavigationMethod.Length > 0)
                {
                    return "Navega para a p\x00e1gina " + TPage.GetPageTitle(Navigation.GetType(this.NavigationMethod), Thread.CurrentThread.CurrentCulture.Name);
                }

                if (convertQuestionToEmpty)
                {
                    toolTip = string.Empty;
                }
            }

            return toolTip;
        }
    }

    internal class GetUrlConverter : TypeConverter
    {
        private string[] navigationMethodsNames;

        public GetUrlConverter()
        {
            Navigation.NavigationMethodsReset += this.Navigation_NavigationMethodsReset;
        }

        public static Type GetTypeFromTypeDescriptorContext(ITypeDescriptorContext context)
        {
            // Type.GetType(IDesignerHost.RootComponentClassName) causa o load do assembly
            // que contém o tipo. Isso gera, em design time, a seguinte mensagem:
            // "You must rebuild your project for the changes to <class> to show up in any open designers."
            // IDesignerHost.Container: Microsoft.VisualStudio.Designer.Host.DesignerHost
            // IDesignerHost.RootComponent: System.Web.UI.Page
            // IDesignerHost.RootComponentClassName: <classe derivada de System.Web.UI.Page correspondente à página>
            if (context == null)
            {
                return null;
            }

            Type type = null;
            if (context.Container is IDesignerHost)
            {
                type = Type.GetType(((IDesignerHost)context.Container).RootComponentClassName);
            }
            else if (context.Instance is HyperLinkColumn)
            {
                var col = (HyperLinkColumn)context.Instance;
                var grd = col.GetOwner();
                if (grd != null)
                {
                    type = grd.Page.GetType();
                }
            }
            else if (context.Instance is LinkListItem)
            {
                return ((LinkListItem)context.Instance).ParentControl.Page.GetType();
            }

            if (!(context.Instance is BusinessMethod))
            {
                throw new NotImplementedException("N\x00e3o implementado em GetUrlConverter.GetTypeFromTypeDescriptorContext(): " + context.Instance.GetType().FullName + ".");
            }

            return ((BusinessMethod)context.Instance).Parent.Page.GetType();
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (this.navigationMethodsNames == null)
            {
                

                var list = new ArrayList();
                foreach (DictionaryEntry item in Navigation.NavigationMethods)
                {
                    list.Add(item.Key);
                }

                list.Sort();
                this.navigationMethodsNames = (string[])list.ToArray(typeof (string));

                
            }

            return new StandardValuesCollection(this.navigationMethodsNames);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        private void Navigation_NavigationMethodsReset(object sender, EventArgs e)
        {
            this.navigationMethodsNames = null;
        }
    }
}