using System;
using System.Web;
using System.Web.UI;
using Techne.Lyceum.RN;
using Techne.Web;


namespace Techne.Lyceum.Net.Biblioteca
{
    [NavUrl("~/Biblioteca/Inicial.aspx"),
    ControlText("Inicial"),
    Title("Inicial"),]

    public partial class Inicial : TPage
    {
        #region Código gerado Techne
        public static string GetUrl()
        {
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {

        }
        #endregion
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }

    }
}
