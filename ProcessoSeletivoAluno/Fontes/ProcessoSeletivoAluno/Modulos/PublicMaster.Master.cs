namespace Techne.Lyceum.Net.Modulos
{
    using System;
    using System.Configuration;
    using System.Web.UI;

    public partial class PublicMaster : System.Web.UI.MasterPage
    {
        public bool habilitaLoading = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            var versao = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var sufixo = ConfigurationManager.AppSettings["VersaoSufixo"] ?? string.Empty;

            lblVersao.Text = versao + sufixo;

            Techne.Web.TSearchBox.RegisterTSearchBoxScript(this.Page);

            if (this.habilitaLoading)
            {
                if (!Page.ClientScript.IsClientScriptBlockRegistered("managerMaster"))
                {
                    Page.ClientScript.RegisterStartupScript(typeof(MasterPage), "managerMaster",
                      "<script  type=\"text/javascript\" >" +
                            @"//add event handlers to the search UpdatePanel
                        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(startRequest);
                        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);

                        function startRequest(sender, e) {
                            pucLoading.Show();
                        }

                        function endRequest(sender, e) {
                            pucLoading.Hide();
                        }" +
                      "</script>\r\n"
                    );
                }
            }
        }
    }
}