using System.ComponentModel;

namespace Techne.Controls
{
    internal class HyperLinkColumn : TGridColumn
    {
        private const string Text_Def = "?";

        private const string ToolTip_Def = "?";

        public HyperLinkColumn()
        {
            this.ImageUrl = string.Empty;
            this.ImageUrlUnauthorized = string.Empty;
            this.NavigationMethod = string.Empty;
            this.NavigationParameters = string.Empty;
            this.EmptyText = TLinkBase.EmptyText_Def;
            this.ShowWhenUnauthorized = THyperLinkBase.ShowWhenUnauthorized_Def;
            this.Text = Text_Def;
            this.ToolTip = ToolTip_Def;
        }

        /// <summary>
        ///   Texto a ser mostrado quando o valor assumido pelo campo é string.Empty ou DBNull.
        /// </summary>
        [
            Category("Techne"), 
            DefaultValue(TLinkBase.EmptyText_Def), 
            Description("Texto a ser mostrado quando o valor assumido pelo campo é DBNull."), 
        ]
        public string EmptyText
        {
            get
            {
                return (string)this.ViewState["EmptyText"];
            }

            set
            {
                this.ViewState["EmptyText"] = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(""), 
            Description("Imagem utilizada como hyperlink para NavigateUrl. " +
                        "Tem prioridade sobre a propriedade Text."), 
        ]
        public string ImageUrl
        {
            get
            {
                return (string)this.ViewState["ImageUrl"];
            }

            set
            {
                this.ViewState["ImageUrl"] = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(""), 
            Description("Imagem a ser mostrada quando o usuário não tem permissão em NavigateUrl. " +
                        "Esta propriedade não é afetada pela propriedade ShowWhenAuthorized. " +
                        "Tem prioridade sobre a propriedade Text."), 
        ]
        public string ImageUrlUnauthorized
        {
            get
            {
                return (string)this.ViewState["ImageUrlUnauthorized"];
            }

            set
            {
                this.ViewState["ImageUrlUnauthorized"] = value == null ? string.Empty : value;
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
                return (string)this.ViewState["NavigationMethod"];
            }

            set
            {
                this.ViewState["NavigationMethod"] = value == null ? string.Empty : value;
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
                return (string)this.ViewState["NavigationParameters"];
            }

            set
            {
                this.ViewState["NavigationParameters"] = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(THyperLinkBase.ShowWhenUnauthorized_Def), 
            Description("Mostra o link desabilitado quando o usuário não possui permissão em NavigateUrl."), 
        ]
        public bool ShowWhenUnauthorized
        {
            get
            {
                return (bool)this.ViewState["ShowWhenUnauthorized"];
            }

            set
            {
                this.ViewState["ShowWhenUnauthorized"] = value;
            }
        }

        /// <summary>
        ///   Texto a ser mostrado no link.
        ///   Se a propriedade ColumnName for informada esta propriedade é ignorada.
        /// </summary>
        [
            Category("Techne"), 
            DefaultValue(Text_Def), 
            Description("Texto a ser mostrado no link. " +
                        "Informe \"?\" para que o caption da página destino seja utilizado. " +
                        "Se a propriedade ColumnName for informada esta propriedade será ignorada."), 
        ]
        public string Text
        {
            get
            {
                return (string)this.ViewState["Text"];
            }

            set
            {
                this.ViewState["Text"] = value == null ? string.Empty : value;
            }
        }

        [
            Category("Behavior"), 
            DefaultValue(ToolTip_Def), 
            Description("Informe \"?\" para que um texto default informando a página destino seja utilizado."), 
        ]
        public string ToolTip
        {
            get
            {
                return (string)this.ViewState["ToolTip"];
            }

            set
            {
                this.ViewState["ToolTip"] = value == null ? string.Empty : value;
            }
        }

        protected override void CopyProperties(ITControl target)
        {
            var hyp = target as THyperLink;
            if (hyp == null)
            {
                return;
            }

            base.CopyProperties(target);

            hyp.ImageUrl = this.ImageUrl;
            hyp.ImageUrlUnauthorized = this.ImageUrlUnauthorized;
            hyp.NavigationMethod = this.NavigationMethod;
            hyp.NavigationParameters = this.NavigationParameters;
            hyp.EmptyText = this.EmptyText;
            hyp.ShowWhenUnauthorized = this.ShowWhenUnauthorized;
            hyp.Text = this.Text;
            hyp.ToolTip = this.ToolTip;
        }

        protected override ITControl GetTControl(string id)
        {
            var hyp = new THyperLink();
            hyp.ID = id;
            this.CopyProperties(hyp);
            return hyp;
        }
    }
}