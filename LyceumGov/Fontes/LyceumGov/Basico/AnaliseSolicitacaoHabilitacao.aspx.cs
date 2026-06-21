using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
    using Techne.Lyceum.RN.DTOs;

    [
     NavUrl("~/Basico/AnaliseSolicitacaoHabilitacao.aspx"),
      ControlText("AnaliseSolicitacaoHabilitacao"),
      Title("Análise Solicitação de Habilitação"),
    ]

    public partial class AnaliseSolicitacaoHabilitacao : TPage
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

            TituloGrid(grdAnaliseSolHabilitacao, "Análise Solicitações de Habilitação");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    if (!RN.Usuarios.UsuarioPrivilegiado(HttpContext.Current.User.Identity.Name))
                    {
                        tseUnidade_Ensino.SqlWhere = tseUnidade_Ensino.SqlWhere.ToString() + " and uuf.USUARIO = '" + HttpContext.Current.User.Identity.Name + "'";
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
            ControlaAcesso(grdAnaliseSolHabilitacao);
        }

        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        protected void grdSolHabilitacao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdAnaliseSolHabilitacao);
        }

        public object Listar(object unidade_ens, object nucleo)
        {
            QueryTable qt = null;

            if (String.IsNullOrEmpty(unidade_ens.ToString()))
                qt = RN.SolicitacaoHabilitacaoDocente.ListarPorNucleo(nucleo.ToString(), "Aguardando");
            else
                qt = RN.SolicitacaoHabilitacaoDocente.ListarPorCenso(unidade_ens.ToString(), "Aguardando");
            return qt;
        }

        public object ListarPor(object unidade_ens, object id_regional)
        {
            QueryTable qt = null;

            if (String.IsNullOrEmpty(unidade_ens.ToString()))
                qt = RN.SolicitacaoHabilitacaoDocente.ListarPorNucleo(id_regional.ToString(), "Aguardando");
            else
                qt = RN.SolicitacaoHabilitacaoDocente.ListarPorCenso(unidade_ens.ToString(), "Aguardando");
            return qt;
        }

        public object ListaMotivoReprovacao()
        {

            RN.TabelaGeral rnTabelaGeral = new TabelaGeral();

            return rnTabelaGeral.Lista("MotivoReprovSolHab");
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
            grdAnaliseSolHabilitacao.Visible = true;
            grdAnaliseSolHabilitacao.DataBind();

            this.grdAnaliseSolHabilitacao.Selection.UnselectAll();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidade_Ensino_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
            if (Page.IsCallback)
                return;

            tseRegional.Value = tseUnidade_Ensino["id_regional"];
            tseRegional_Changed(sender, args);

            if (!tseUnidade_Ensino.DBValue.IsNull && tseUnidade_Ensino.IsValidDBValue)
            {
                tseRegional.Value = tseUnidade_Ensino["id_regional"];
                tseRegional_Changed(sender, args);
            }

            if (tseUnidade_Ensino.IsValidDBValue && !tseUnidade_Ensino.DBValue.IsNull)
            {
                grdAnaliseSolHabilitacao.Visible = true;
                lblMensagem.Text = string.Empty;
                pnGrid.Visible = true;
            }
            else if (!tseUnidade_Ensino.DBValue.IsNull)
            {
                grdAnaliseSolHabilitacao.Visible = false;
                lblMensagem.Text = "Unidade Administrativa não cadastrada.";
                pnGrid.Visible = false;
            }
            else
            {
                grdAnaliseSolHabilitacao.Visible = false;
                lblMensagem.Text = "Favor consultar uma unidade administrativa.";
                pnGrid.Visible = false;
            }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdAnaliseSolHabilitacao_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            if (Session["Mensagem"] == null)
                e.Properties["cpMessage"] = String.Empty;
            else
            {
                e.Properties["cpMessage"] = Session["Mensagem"].ToString();
                Session["Mensagem"] = null;
            }
        }

        protected void btnAprovar_Click(object sender, EventArgs e)
        {
            try
            {
            var selectedItems = this.grdAnaliseSolHabilitacao
                .GetSelectedFieldValues("ID_SOLICITACAO_HABILITACAO_DOCENTE")
                .Select(x => new StatusSolicitacaoHabilitacaoDocente
                {
                    IdSolicitacaoHabilitacaoDocente = int.Parse(x.ToString())
                })
                .ToList();

            //validar se a disciplina já esta habilitada para o docente
            foreach (var selectedItem in selectedItems)
            {
                var validacao = new ValidacaoDados();
                validacao = RN.SolicitacaoHabilitacaoDocente.ValidarAprovacao(selectedItem.IdSolicitacaoHabilitacaoDocente.ToString());

                if (!validacao.Valido)
                {
                    lblMensagem.Text = validacao.Mensagem + " a solicitação " + selectedItem.IdSolicitacaoHabilitacaoDocente + ".";
                    return;
                }

            }

            var retorno = SolicitacaoHabilitacaoDocente.Aprovar(selectedItems);

            if (retorno != null)
            {
                if (!retorno.Ok)
                    throw new Exception(retorno.Errors.ToString());
            }
            else
            {
                lblMensagem.Text = "Pedido(s) aprovado(s) com sucesso.";
                odsSolicitacaoHabilitacao.Select();
                odsSolicitacaoHabilitacao.DataBind();
                grdAnaliseSolHabilitacao.DataBind();
                this.grdAnaliseSolHabilitacao.Selection.UnselectAll();
            }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnReprovar_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedItems = this.grdAnaliseSolHabilitacao
                   .GetSelectedFieldValues("ID_SOLICITACAO_HABILITACAO_DOCENTE")
                   .Select(x => new StatusSolicitacaoHabilitacaoDocente
                   {
                       IdSolicitacaoHabilitacaoDocente = int.Parse(x.ToString()),
                   })
                   .ToList();

                foreach (var selectedItem in selectedItems)
                {
                    var control = this.grdAnaliseSolHabilitacao.FindRowCellTemplateControlByKey(
                                            selectedItem.IdSolicitacaoHabilitacaoDocente,
                                            (GridViewDataColumn)this.grdAnaliseSolHabilitacao.Columns["MOTIVO"],
                                            "cmbMotivoReprovacao");

                    var id = selectedItem.IdSolicitacaoHabilitacaoDocente.ToString();

                    if (control is DropDownList)
                    {
                        var dropDownList = (DropDownList)control;

                        if ((dropDownList.SelectedValue != "Selecione") && (!string.IsNullOrEmpty(dropDownList.SelectedValue)))
                            selectedItem.Motivo = dropDownList.SelectedValue;
                        else
                        {
                            lblMensagem.Text = "Informe o motivo da reprovação para a solicitação " + id + ".";
                            return;
                        }
                    }
                }

                var retorno = SolicitacaoHabilitacaoDocente.Reprovar(selectedItems);

                if (retorno != null)
                {
                    if (!retorno.Ok)
                        throw new Exception(retorno.Errors.ToString());
                }
                else
                {
                    lblMensagem.Text = "Pedido(s) reprovado(s) com sucesso.";
                    odsSolicitacaoHabilitacao.Select();
                    odsSolicitacaoHabilitacao.DataBind();
                    grdAnaliseSolHabilitacao.DataBind();
                    this.grdAnaliseSolHabilitacao.Selection.UnselectAll();
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}

