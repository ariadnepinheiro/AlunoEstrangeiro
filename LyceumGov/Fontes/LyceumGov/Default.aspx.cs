using System;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net
{
    [
         NavUrl("~/Default.aspx"),
         ControlText("PáginaInical"),
         Title("Página Inicial"),
     ]
    public partial class _default : TPage
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
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
        }
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
           if (!User.Identity.IsAuthenticated)
                Response.Redirect(Seguranca.Identificacao.GetUrl());
            else
            {
                //redireciona o usuário para o primeiro menu disponível
                SiteMapNode root = SiteMap.RootNode;
                if (root != null)
                {
                    foreach (SiteMapNode item in root.ChildNodes)
                    {
                        if (!string.IsNullOrEmpty(item.Url))
                        {
                            Response.Redirect(item.Url);
                            break;
                        }
                    }
                }

            }

        }
    }
}
