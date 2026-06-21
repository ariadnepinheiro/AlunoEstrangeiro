using System;
using System.ComponentModel;

namespace Techne.Controls
{
    internal class TLinkMethod : TLinkButtonBase
    {
        internal const string ExecuteMessage_Def = "Função {0} executada com sucesso.";

        internal const string ImageUrl_Def = "?";

        internal const string SuccessMsgCssClass_Def = "MsgWarning";

        private const ControlMessageType ControlMessageType_Def = ControlMessageType.Label;

        readonly BusinessMethod method;

        /// <summary>
        ///   Instanciado por CallMethod(). Não setar diretamente.
        ///   Quando diferente de null, indica o resultado do método executado.
        /// </summary>
        RetVal methodResult;

        private TLinkParameterCollection parameters;

        public TLinkMethod()
        {
            this.parameters = new TLinkParameterCollection();
            this.method = new BusinessMethod(this);

            this.ControlMessageType = ControlMessageType_Def;
            this.ExecuteMethod = string.Empty;
            this.ExecuteParameters = string.Empty;
            this.ImageUrl = ImageUrl_Def;
            this.NavigationMethod = string.Empty;
            this.NavigationParameters = string.Empty;
            this.SuccessMsg = ExecuteMessage_Def;
            this.SuccessMsgCssClass = SuccessMsgCssClass_Def;
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
            DefaultValue(ImageUrl_Def), 
            Description(
                "Url da imagem que será utilizado como link. " +
                "Informe \"?\" para utilizar a url default do método chamado (setado pelo atributo Image). " +
                "Utilize \"~\" para indicar a raiz da aplicação. " +
                "A propriedade Text poderá ser utilizada simultaneamente. " +
                "Veja também: ImagePosition."
                ), 
        ]
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

        [TypeConverter(typeof (BusinessMethodConverter)), Description("M\x00e9todo a ser executado quando o controle \x00e9 clicado. Os m\x00e9todos permitidos s\x00e3o aqueles declarados em projetos com o atributo BusinessAssembly (arquivo AssemblyInfo.cs), p\x00fablicos, est\x00e1ticos, e que devolvem valores derivados de Techne.RetVal."), DefaultValue(""), Category("Techne")]
        public string ExecuteMethod
        {
            get
            {
                return this.method.ExecuteMethod;
            }

            set
            {
                this.method.ExecuteMethod = value == null ? string.Empty : value;
                this.ViewState["ExecuteMethod"] = this.method.ExecuteMethod;
            }
        }

        [Description("Valores a serem passados como par\x00e2metros ao m\x00e9todo informado na propriedade ExecuteMethod. S\x00e3o permitidas refer\x00eancias a TControls (#controle#), campos ($campo$) e par\x00e2metros da p\x00e1gina (@par\x00e2metro@). Valores constantes devem ser informados no formato de cultura invariante. DBNull deve ser informado como string vazia."), DefaultValue(""), Category("Techne")]
        public string ExecuteParameters
        {
            get
            {
                return this.method.ExecuteParameters;
            }

            set
            {
                this.method.ExecuteParameters = value == null ? string.Empty : value;
                this.ViewState["ExecuteParameters"] = this.method.ExecuteParameters;
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
                return this.method.NavigationMethod;
            }

            set
            {
                this.method.NavigationMethod = value == null ? string.Empty : value;
                this.ViewState["NavigationMethod"] = this.method.NavigationMethod;
            }
        }

        [Category("Techne"), Description("Valores a serem passados como par\x00e2metros \x00e0 p\x00e1gina indicada pela propriedade NavigationMethod. S\x00e3o permitidas refer\x00eancias a TControls (#controle#), campos ($campo$) e par\x00e2metros da p\x00e1gina (@par\x00e2metro@). Valores constantes devem ser informados no formato de cultura invariante. DBNull deve ser informado como string vazia."), DefaultValue("")]
        public string NavigationParameters
        {
            get
            {
                return this.method.NavigationParameters;
            }

            set
            {
                this.method.NavigationParameters = value == null ? string.Empty : value;
                this.ViewState["NavigationParameters"] = this.method.NavigationParameters;
            }
        }

        [Description("Mensagem dada ao usu\x00e1rio caso o m\x00e9todo chamado n\x00e3o tenha especificado uma para execu\x00e7\x00e3o bem sucedida. Utilize {0} para indicar a substitu\x00e7\x00e3o pelo atributo ControlText do m\x00e9todo."), DefaultValue("Fun\x00e7\x00e3o {0} executada com sucesso."), Category("Appearance")]
        public string SuccessMsg
        {
            get
            {
                return this.method.ExecuteMessage;
            }

            set
            {
                this.method.ExecuteMessage = value;
            }
        }

        [Category("Appearance"), DefaultValue("MsgWarning")]
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

        internal override string GetAutoText()
        {
            return this.method.GetControlText();
        }

        /// <summary>
        ///   Devolve ImageUrl, a não ser que seu valor seja "?".
        ///   Neste caso, busca o atributo Image do método.
        /// </summary>
        internal override string GetImageUrl()
        {
            var url = base.GetImageUrl();

            if (url != "?")
            {
                return url;
            }
            else
            {
                // Se ImageUrl == "?", utiliza o atributo Image associado ao método.
                return this.method.GetImageUrl();
            }
        }

        protected override string GetMessageCssClass()
        {
            if ((this.methodResult != null) && this.methodResult.Ok)
            {
                return this.SuccessMsgCssClass;
            }

            return base.GetMessageCssClass();
        }

        protected override string GetToolTip(bool convertQuestionToEmpty)
        {
            if (this.Enabled && this.ToolTip == "?")
            {
                return this.method.GetToolTip();
            }
            else
            {
                return base.GetToolTip(convertQuestionToEmpty);
            }
        }

        protected override void OnClick(EventArgs args)
        {
            try
            {
                this.methodResult = this.method.Call(this.RecordContainer); // Inicializa methodResult
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException("Erro no controle " + this.ID + ".", exc);
            }

            // Se o método não devolver uma mensagem de sucesso, cria uma mensagem default.
            var methodMsg = this.methodResult.Message;
            if (this.methodResult.Ok)
            {
                if (methodMsg.Length == 0)
                {
                    methodMsg = this.method.GetExecuteMessage(1);
                }

                ContainerManagerLib.Refresh(this.method, this.Page);
            }

            this.Msg = methodMsg;
        }
    }
}