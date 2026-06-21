using System;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net
{
    [
         NavUrl("~/Redir.aspx"),
         ControlText("PáginaInical"),
         Title("Página Inicial"),
     ]

    public partial class _redir : TPage
    {
        #region Web Form Designer generated code
        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        override protected void OnInit(EventArgs e)
        {
        }

        private void InitializeComponent()
        {
        }
        #endregion


        protected void Page_Load(object sender, System.EventArgs e)
        {

        }
    }
}
