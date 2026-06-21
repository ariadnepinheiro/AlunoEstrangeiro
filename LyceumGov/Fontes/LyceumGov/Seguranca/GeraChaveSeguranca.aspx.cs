using System;
using System.Reflection;
using System.Web.UI;
using Techne.Lyceum.RN;
using Techne.Web;

namespace Techne.Lyceum.Net.Seguranca
{
    public partial class GeraChaveSeguranca : TPage
    {
        protected System.Web.UI.WebControls.Label PageUser;
        protected System.Web.UI.WebControls.Label PageDate;

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion

        public static string GetUrl()
        {
            #region Código gerado Techne
            return Navigation.GetNavigation(MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Gera Captcha
            RN.Captcha rnCaptcha = new Techne.Lyceum.RN.Captcha();
            rnCaptcha.GeraCaptcha();
        }
    }
}
