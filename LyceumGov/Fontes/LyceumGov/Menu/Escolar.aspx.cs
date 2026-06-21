using System;
using System.Web.UI;

namespace Techne.Lyceum.Net.Menu
{
    [Techne.Title("Gestão Escolar")]
    public partial class Escolar : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Session["LeuInforme"] == null)
                    {
                        var dtFim = System.Configuration.ConfigurationSettings.AppSettings["DataFimInformeLogin"];

                        if (DateTime.Now <= Convert.ToDateTime(dtFim.ToString()))
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopup();", true);
                    }

                    var ExibeSAEB = Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["ExibeSAEB"]);

                    if (ExibeSAEB)
                    {
                        Popup pop = new Popup();
                        pop.Visible = false;

                        PopupA popA = new PopupA();
                        popA.Visible = true;
                    }
                    else
                    {
                        Popup pop = new Popup();
                        pop.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnConfirmarPopUp_OnClick(object sender, EventArgs e)
        {
            Session["LeuInforme"] = "OK";
            pcPesquisaClima.ShowOnPageLoad = false;
        }
    }
}
