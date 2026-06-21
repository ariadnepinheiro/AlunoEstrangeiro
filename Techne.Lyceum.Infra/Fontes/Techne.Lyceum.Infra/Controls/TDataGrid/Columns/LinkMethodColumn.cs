using System.ComponentModel;

namespace Techne.Controls
{
    internal class LinkMethodColumn : TGridColumn
    {
        private const ControlMessageType ControlMessageType_Def = ControlMessageType.Icon;

        public LinkMethodColumn()
        {
            this.ControlMessageType = ControlMessageType_Def;
            this.ExecuteMethod = string.Empty;
            this.ExecuteParameters = string.Empty;
            this.ImageUrl = TLinkMethod.ImageUrl_Def;
            this.NavigationMethod = string.Empty;
            this.NavigationParameters = string.Empty;
            this.SuccessMsg = TLinkMethod.ExecuteMessage_Def;
            this.SuccessMsgCssClass = TLinkMethod.SuccessMsgCssClass_Def;
        }

        [
            DefaultValue(ControlMessageType_Def), 
        ]
        public override ControlMessageType ControlMessageType
        {
            get
            {
                return base.ControlMessageType;
            }

            set
            {
                base.ControlMessageType = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(""), 
            Description(
                "Método a ser executado quando o controle é clicado. " +
                "Os métodos permitidos são aqueles declarados em projetos com o atributo BusinessAssembly (arquivo AssemblyInfo.cs), " +
                "públicos, estáticos, e que devolvem valores derivados de Techne.RetVal."
                ), 
            TypeConverter(typeof (BusinessMethodConverter)), 
        ]
        public string ExecuteMethod
        {
            get
            {
                return (string)this.ViewState["ExecuteMethod"];
            }

            set
            {
                this.ViewState["ExecuteMethod"] = value == null ? string.Empty : value.Trim();
            }
        }

        [
            Category("Techne"), 
            DefaultValue(""), 
            Description(
                "Valores a serem passados como parâmetros ao método informado na propriedade ExecuteMethod. " +
                "São permitidas referências a TControls (#controle#), campos ($campo$) e parâmetros da página (@parâmetro@). " +
                "Valores constantes devem ser informados no formato de cultura invariante. " +
                "DBNull deve ser informado como string vazia."
                ), 
        ]
        public string ExecuteParameters
        {
            get
            {
                return (string)this.ViewState["ExecuteParameters"];
            }

            set
            {
                this.ViewState["ExecuteParameters"] = value == null ? string.Empty : value.Trim();
            }
        }

        [
            DefaultValue(TLinkMethod.ImageUrl_Def), 
            Description(
                "Url da imagem que será utilizado como link. " +
                "Informe \"?\" para utilizar a url default do método chamado (setado pelo atributo Image). " +
                "Utilize \"~\" para indicar a raiz da aplicação. " +
                "A propriedade Text poderá ser utilizada simultaneamente. " +
                "Veja também: ImagePosition."
                ), 
        ]
        public string ImageUrl
        {
            get
            {
                return (string)this.ViewState["ImageUrl"];
            }

            set
            {
                this.ViewState["ImageUrl"] = value == null ? string.Empty : value.Trim();
            }
        }

        [
            Category("Techne"), 
            DefaultValue(""), 
            Description("Página para onde o redirecionamento será feito quando o método for executado com sucesso."), 
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
                this.ViewState["NavigationMethod"] = value == null ? string.Empty : value.Trim();
            }
        }

        [
            Category("Techne"), 
            DefaultValue(""), 
            Description(
                "Valores a serem passados como parâmetros à página indicada pela propriedade NavigationMethod. " +
                "São permitidas referências a TControls (#controle#), campos ($campo$) e parâmetros da página (@parâmetro@). " +
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
                this.ViewState["NavigationParameters"] = value == null ? string.Empty : value.Trim();
            }
        }

        [
            Category("Appearance"), 
            DefaultValue(TLinkMethod.ExecuteMessage_Def), 
            Description(
                "Mensagem dada ao usuário caso o método chamado não tenha especificado uma para execução bem sucedida. " +
                "Utilize {0} para indicar a substitução pelo atributo ControlText do método."
                ), 
        ]
        public string SuccessMsg
        {
            get
            {
                return (string)this.ViewState["SuccessMsg"];
            }

            set
            {
                this.ViewState["SuccessMsg"] = value == null ? string.Empty : value;
            }
        }

        [
            Category("Appearance"), 
            DefaultValue(TLinkMethod.SuccessMsgCssClass_Def), 
        ]
        public string SuccessMsgCssClass
        {
            get
            {
                return (string)this.ViewState["SuccessMsgCssClass"];
            }

            set
            {
                this.ViewState["SuccessMsgCssClass"] = value == null ? string.Empty : value.Trim();
            }
        }

        protected override void CopyProperties(ITControl target)
        {
            var lnk = target as TLinkMethod;
            if (lnk == null)
            {
                return;
            }

            base.CopyProperties(target);

            lnk.ExecuteMethod = this.ExecuteMethod;
            lnk.ExecuteParameters = this.ExecuteParameters;
            lnk.ImageUrl = this.ImageUrl;
            lnk.NavigationMethod = this.NavigationMethod;
            lnk.NavigationParameters = this.NavigationParameters;
            lnk.SuccessMsg = this.SuccessMsg;
            lnk.SuccessMsgCssClass = this.SuccessMsgCssClass;
        }

        protected override ITControl GetTControl(string id)
        {
            var lnk = new TLinkMethod();
            lnk.ID = id;
            this.CopyProperties(lnk);
            return lnk;
        }
    }
}