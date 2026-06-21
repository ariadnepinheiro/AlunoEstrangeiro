using System.ComponentModel;
using System.Web.UI;

namespace Techne.Controls
{
    /// <summary>
    ///   O THyperLinkInternal é um controle utilizado somente por THyperLinkList.
    /// </summary>
    [
        ToolboxItem(false)
    ]
    internal class THyperLinkInternal : THyperLinkBase
    {
        /// <summary>
        ///   Página aspx para a qual a navegação deverá ser feita.
        ///   Ao indicar a raiz, deve-se utilizar o caractere '~'.
        /// </summary>
        [Description("P\x00e1gina aspx para a qual a navega\x00e7\x00e3o dever\x00e1 ser feita. Ao indicar a raiz, deve-se utilizar o caractere '~'."), Category("Techne"), DefaultValue("")]
        public string NavigateUrl
        {
            get
            {
                return this.BaseNavigateUrl;
            }

            set
            {
                this.BaseNavigateUrl = value;
            }
        }

        /// <summary>
        ///   Lista de parâmetros a serem passados à página sendo chamada.
        /// </summary>
        [Description("Lista de par\x00e2metros a serem passados \x00e0 p\x00e1gina sendo chamada."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Techne"), PersistenceMode(PersistenceMode.InnerProperty)]
        public TLinkParameterCollection Parameters
        {
            get
            {
                return this.BaseParameters;
            }
        }

        /// <summary>
        ///   Habilita a navegação inversa, ou seja, da página chamada para a chamadora
        ///   através do método TPage.PageReturn(). Da mesma forma, o botão \"Voltar\" de
        ///   EditButtons e GridButtons só serão habilitados se essa propriedade for setada para True.
        /// </summary>
        [
            Category("Techne"), 
            DefaultValue(ReturnEnabled_Def), 
            Description("Habilita a navegação inversa, ou seja, da página chamada para a chamadora " +
                        "através do método TPage.PageReturn(). Da mesma forma, o botão \"Voltar\" de " +
                        "EditButtons e GridButtons só serão habilitados se essa propriedade for setada para True."), 
        ]
        public bool ReturnEnabled
        {
            get
            {
                return this.BaseReturnEnabled;
            }

            set
            {
                this.BaseReturnEnabled = value;
            }
        }
    }
}