namespace Techne.Lyceum.Net.Menu
{
    using System;

    [Title("Menu")]
    public partial class Config : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["LeuInforme"] == null)
                {
                    var dtFim = System.Configuration.ConfigurationSettings.AppSettings["DataFimInformeLogin"];


                    if (DateTime.Now <= Convert.ToDateTime(dtFim.ToString()))
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopup();", true);
                }
            }
        }
        protected void btnConfirmarPopUp_OnClick(object sender, EventArgs e)
        {
            Session["LeuInforme"] = "OK";
            pcPesquisaClima.ShowOnPageLoad = false;

        }
    }
}