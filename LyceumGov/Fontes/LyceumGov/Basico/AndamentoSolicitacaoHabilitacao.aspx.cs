using System;
using System.Web;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Web;
using Techne.Controls;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Basico
{
    [
     NavUrl("~/Basico/AndamentoSolicitacaoHabilitacao.aspx"),
      ControlText("AndamentoSolicitacaoHabilitacao"),
      Title("Andamento da Solicitação de Habilitação"),
    ]

    public partial class AndamentoSolicitacaoHabilitacao : TPage
    {
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

        protected void Page_Init(object sender, EventArgs e)
        {

            TituloGrid(grdSolHabilitacao, "Solicitações de Habilitação");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    if (!RN.Usuarios.UsuarioPrivilegiado(HttpContext.Current.User.Identity.Name))
                    {
                        tseUnidadeAdministrativa.SqlWhere = tseUnidadeAdministrativa.SqlWhere.ToString() + " and uuf.USUARIO = '" + HttpContext.Current.User.Identity.Name + "'";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdSolHabilitacao);
        }

        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        protected void grdSolHabilitacao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdSolHabilitacao);
        }

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                    return;

                if (tseRegional.IsValidDBValue && !tseRegional.DBValue.IsNull)
                {
                    lblMensagem.Text = string.Empty;
                    grdSolHabilitacao.Visible = true;
                    pnGrid.Visible = true;
                }
                else if (!tseRegional.DBValue.IsNull)
                {
                    lblMensagem.Text = "Regional não cadastrada.";
                    return;
                }
                else
                {
                    lblMensagem.Text = "Favor consultar uma Regional.";
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;
                if (tseMunicipio.IsValidDBValue && !tseMunicipio.DBValue.IsNull)
                {
                    lblMensagem.Text = string.Empty;
                    grdSolHabilitacao.Visible = true;
                    pnGrid.Visible = true;

                    tseUnidadeAdministrativa.ResetValue();
                    tseUnidadeAdministrativa.DataBind();
                }
                else if (!tseMunicipio.DBValue.IsNull)
                {
                    lblMensagem.Text = "Municipio não cadastrado.";
                    return;
                }
                else
                {
                    lblMensagem.Text = "Favor consultar um Municipio.";
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        public object Listar(object id_regional, object municipio, object setor, object status)
        {
            QueryTable qt = null;

            var status_sel = String.IsNullOrEmpty((string)status) ? null : (string)status;

            if (string.IsNullOrEmpty(setor.ToString()) && string.IsNullOrEmpty(municipio.ToString()))
            {
                qt = RN.SolicitacaoHabilitacaoDocente.ListarPorNucleo(id_regional.ToString(), status_sel);
            }
            else
            {
                if (string.IsNullOrEmpty(setor.ToString()))
                {
                    qt = RN.SolicitacaoHabilitacaoDocente.ListarPorMunicipio(municipio.ToString(), status_sel);
                }
                else
                {
                    qt = RN.SolicitacaoHabilitacaoDocente.ListarPorUA(setor.ToString(), status_sel);
                }
            }
            return qt;
        }

        protected void tseUnidadeAdministrativa_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;

                if (tseUnidadeAdministrativa.IsValidDBValue && !tseUnidadeAdministrativa.DBValue.IsNull)
                {
                    grdSolHabilitacao.Visible = true;
                    lblMensagem.Text = string.Empty;
                    pnGrid.Visible = true;

                    tseRegional.Value = tseUnidadeAdministrativa["id_regional"].ToString();
                    tseMunicipio.Value = tseUnidadeAdministrativa["municipio"];
                }
                else if (!tseUnidadeAdministrativa.DBValue.IsNull)
                {
                    grdSolHabilitacao.Visible = false;

                    lblMensagem.Text = "Unidade Administrativa não cadastrada.";
                    pnGrid.Visible = false;
                }
                else
                {
                    grdSolHabilitacao.Visible = false;
                    lblMensagem.Text = "Favor consultar uma unidade administrativa.";
                    pnGrid.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void grdSolHabilitacao_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            if (Session["Mensagem"] == null)
                e.Properties["cpMessage"] = String.Empty;
            else
            {
                e.Properties["cpMessage"] = Session["Mensagem"].ToString();
                Session["Mensagem"] = null;
            }
        }


        protected void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(cmbStatus.SelectedValue))
                {
                    odsSolicitacaoHabilitacao.Select();
                    odsSolicitacaoHabilitacao.DataBind();
                    grdSolHabilitacao.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

    }
}

