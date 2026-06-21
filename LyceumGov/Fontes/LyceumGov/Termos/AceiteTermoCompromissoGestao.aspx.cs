using System;
using System.Web.Security;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.Net.Termos
{
    public partial class AceiteTermoCompromissoGestao : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            lblMensagem.Text = string.Empty;
            
            if (!IsPostBack)
            {
                var aceite = RN.AceiteTermoCompromissoGestao.RetornaMenorTermoSemAceite(User.Identity.Name.Trim());

                if (aceite != null)
                {
                    hdnAnoTermo.Value = aceite.Ano.ToString();
                    hdnIDTermo.Value = aceite.IdTermoGestao.ToString();

                    ifpanel.Attributes["src"] = "../Termos/" + aceite.Arquivo;
                }
                else
                {
                    Session["aceite_termo"] = "OK";
                    Response.Redirect("../Default.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }

            }
        }

        protected void chkAceite_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAceite.Checked)
            {
                btnSalvarAceite.Visible = true;
            }
            else
            {
                btnSalvarAceite.Visible = false;
            }
        }

        protected void btnSalvarAceite_Click(object sender, EventArgs e)
        {
            try
            {
                var TATC = new TceAceiteTermoCompromissoGestao
                {
                    
                    Ano = int.Parse(hdnAnoTermo.Value),
                    IdTermoGestao = int.Parse(hdnIDTermo.Value),
                    Ip = RN.Sistema.ObterIP(),
                    Matricula = this.User.Identity.Name.Trim()
                };

                var validacao = RN.AceiteTermoCompromissoGestao.Validar(TATC);

                if (validacao.Valido)
                {
                    RN.AceiteTermoCompromissoGestao.Inserir(TATC);

                    var aceite = RN.AceiteTermoCompromissoGestao.RetornaMenorTermoSemAceite(User.Identity.Name.Trim());

                    if (aceite != null)
                    {
                        chkAceite.Checked = false;
                        btnSalvarAceite.Visible = false;
                        Response.Redirect("AceiteTermoCompromissoGestao.aspx");
                    }
                    else
                    {
                        Session["aceite_termo"] = "OK";

                         var dtFim = System.Configuration.ConfigurationSettings.AppSettings["DataFimInformeLogin"];

                         if (DateTime.Now <= Convert.ToDateTime(dtFim.ToString()))
                             Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopup();", true);
                         else
                             Response.Redirect("../Default.aspx");
                    }
                   
                }
                else
                {
                    if (!string.IsNullOrEmpty(validacao.Mensagem))
                    {
                        this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
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
            Response.Redirect("../Default.aspx");
        }
    }
}
