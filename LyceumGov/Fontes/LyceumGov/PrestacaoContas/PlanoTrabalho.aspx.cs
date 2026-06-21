using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using DevExpress.Web.ASPxEditors;
using System.Data;
using DevExpress.Web.ASPxGridView;
using Techne.Controls;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Certificacao;
using DevExpress.Web.ASPxTabControl;
using Techne.Lyceum.RN.DTOs;
using System.Text;
using Seeduc.Infra.Helpers;
using Techne.Lyceum.RN;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [NavUrl("~/Certificacao/PrestacaoContas.aspx"),
    ControlText("Projeto / Programa"),
    Title("Projeto / Programa")]
    public partial class PlanoTrabalho : TPage
    {
        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(grdDocumento, AcaoControle.excluir, "btnDeletar");
            ControlaAcesso(grdDocumento, AcaoControle.editar, "btnEditar");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;

            if (!IsPostBack)
            {
                CarregaTipoDeContratacao();
                CarregaTipoDeDespesa();
                CarregaSuperintendencia();
                CarregaFinalidade();
                Limpar();
            }
        }

        protected void grdDocumento_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Text = "<i>" + e.Row.Cells[1].Text + "</i>";
            }
        }

        protected void grdDocumento_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ((DevExpress.Web.ASPxGridView.ASPxGridView)sender).DataBind();
            ControlaAcesso(grdDocumento, AcaoControle.excluir, "btnDeletar");
            ControlaAcesso(grdDocumento, AcaoControle.editar, "btnEditar");
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdDocumento, string.Empty);
        }

        protected void tseProgramaTrabalho_Changed(object sender, Controls.ChangedEventArgs args)
        {
            if (Page.IsCallback)
            {
                return;
            }

            DataTable dataTable = null;
            RN.PrestacaoContas.PlanoTrabalho rnProtocoloPrestacao = new Techne.Lyceum.RN.PrestacaoContas.PlanoTrabalho();

            if (!tseProgramaTrabalho.DBValue.IsNull)
            {
                if (tseProgramaTrabalho.IsValidDBValue)
                {
                    dataTable = rnProtocoloPrestacao.ListaPorProgramaTrabalho(Convert.ToInt32(tseProgramaTrabalho.DBValue));
                }
                else
                {
                    dataTable = rnProtocoloPrestacao.ListaTodos();
                }
            }
            else
            {
                dataTable = rnProtocoloPrestacao.ListaTodos();
            }

            var sessao = SessaoUsuario.GetSessaoUsuario();
            grdDocumento.DataBind();
        }

        private void CarregaTipoDeContratacao()
        {
            RN.PrestacaoContas.TipoContratacao tipoContratacao = new RN.PrestacaoContas.TipoContratacao();
            cmbTipoDeContratacao.Items.Clear();
            cmbTipoDeContratacao.DataSource = tipoContratacao.ListaAtivo();
            cmbTipoDeContratacao.DataBind();
            cmbTipoDeContratacao.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        private void CarregaTipoDeDespesa()
        {
            RN.PrestacaoContas.TipoDespesa tipoDespesa = new RN.PrestacaoContas.TipoDespesa();
            cmbTipoDeDespesa.Items.Clear();
            cmbTipoDeDespesa.DataSource = tipoDespesa.ListaAtivo();
            cmbTipoDeDespesa.DataBind();
            cmbTipoDeDespesa.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        private void CarregaSuperintendencia()
        {
            RN.GestaoRede.Superintendencia superintendencia = new RN.GestaoRede.Superintendencia();
            cmbSuperintendencia.Items.Clear();
            cmbSuperintendencia.DataSource = superintendencia.ListaAtivo();
            cmbSuperintendencia.DataBind();
            cmbSuperintendencia.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        private void CarregaFinalidade()
        {
            RN.PrestacaoContas.Finalidade finalidade = new RN.PrestacaoContas.Finalidade();
            cmbFinalidade.Items.Clear();
            cmbFinalidade.DataSource = finalidade.ListaAtivo();
            cmbFinalidade.DataBind();
            cmbFinalidade.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        public void Limpar()
        {
            cmbTipoDeContratacao.ClearSelection();
            cmbTipoDeDespesa.ClearSelection();
            cmbSuperintendencia.ClearSelection();
            cmbPeriodicidade.ClearSelection();
            cmbFinalidade.ClearSelection();
            ddlPequenaDespesa.ClearSelection();
            txtDescricao.Text = string.Empty;
            lblIdentificador.Text = string.Empty;
            hdnplanotrabalhoid.Value = string.Empty;
            hdnprogramatrabalhoid.Value = string.Empty;
        }

        protected void grdDocumento_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            string identificador = grdDocumento.GetRowValues(e.VisibleIndex, "IDENTIFICADOR").ToString();
            string tipoContratacaoId = grdDocumento.GetRowValues(e.VisibleIndex, "TIPOCONTRATACAOID").ToString();
            string tipoDespesaId = grdDocumento.GetRowValues(e.VisibleIndex, "TIPODESPESAID").ToString();
            string finalidadeId = grdDocumento.GetRowValues(e.VisibleIndex, "FINALIDADEID").ToString();
            string SuperintendenciaId = grdDocumento.GetRowValues(e.VisibleIndex, "SUPERINTENDENCIAID").ToString();
            string Descricao = grdDocumento.GetRowValues(e.VisibleIndex, "DESCRICAO").ToString();
            string Periodicidade = grdDocumento.GetRowValues(e.VisibleIndex, "PERIODICIDADE").ToString();
            string pequenaDespesa = grdDocumento.GetRowValues(e.VisibleIndex, "PEQUENADESPESA").ToString();
            string planotrabalhoid = grdDocumento.GetRowValues(e.VisibleIndex, "PLANOTRABALHOID").ToString();
            string programatrabalhoid = grdDocumento.GetRowValues(e.VisibleIndex, "PROGRAMATRABALHOID").ToString();

            if (e.ButtonID == "btnEditar")
            {
                pnlDefinicao.Visible = true;
                Limpar();

                cmbTipoDeContratacao.SelectedValue = tipoContratacaoId;
                cmbTipoDeDespesa.SelectedValue = tipoDespesaId;
                cmbSuperintendencia.SelectedValue = SuperintendenciaId;
                cmbPeriodicidade.SelectedValue = Periodicidade;
                cmbFinalidade.SelectedValue = finalidadeId;
                ddlPequenaDespesa.SelectedValue = pequenaDespesa == "True" ? "1" : "0";
                txtDescricao.Text = Descricao;
                lblIdentificador.Text = identificador;
                hdnplanotrabalhoid.Value = planotrabalhoid;
                hdnprogramatrabalhoid.Value = programatrabalhoid;
            }
            if (e.ButtonID == "btnDeletar")
            {
                hdnplanotrabalhoid2.Value = planotrabalhoid;
                popup.ShowOnPageLoad = true;
            }
        }

        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                int planotrabalhoid = hdnplanotrabalhoid2.Value.IsNullOrEmptyOrWhiteSpace() ? -1 : Convert.ToInt32(hdnplanotrabalhoid2.Value);
                RN.PrestacaoContas.PlanoTrabalho rnPlanoTrabalho = new Techne.Lyceum.RN.PrestacaoContas.PlanoTrabalho();
                ValidacaoDados validacao = new ValidacaoDados();

                validacao = rnPlanoTrabalho.ValidaRemocao(planotrabalhoid);

                if (validacao.Valido)
                {
                    rnPlanoTrabalho.Remove(Convert.ToInt32(planotrabalhoid));
                    grdDocumento.DataBind();
                }
                else
                {
                    this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public object Listar(object PROGRAMATRABALHOID)
        {
            RN.PrestacaoContas.PlanoTrabalho rnProtocoloPrestacao = new Techne.Lyceum.RN.PrestacaoContas.PlanoTrabalho();

            DataTable dataTable = null;
            if (((Techne.Internal.Nullable)PROGRAMATRABALHOID).IsNull == false)
            {
                dataTable = rnProtocoloPrestacao.ListaPorProgramaTrabalho(Convert.ToInt32(PROGRAMATRABALHOID));
            }
            else
            {
                dataTable = rnProtocoloPrestacao.ListaTodos();
            }

            return dataTable;
        }

        public object ListaPorProgramaTrabalho(object PROGRAMATRABALHOID)
        {
            DataTable dataTable = null;
            RN.PrestacaoContas.PlanoTrabalho rnProtocoloPrestacao = new Techne.Lyceum.RN.PrestacaoContas.PlanoTrabalho();

            if (((Techne.Internal.Nullable)PROGRAMATRABALHOID).Type != null)
            {

                if (((Techne.Internal.Nullable)PROGRAMATRABALHOID).IsNull == false)
                {
                    dataTable = rnProtocoloPrestacao.ListaPorProgramaTrabalho(Convert.ToInt32(PROGRAMATRABALHOID));
                }
                else
                {
                    dataTable = rnProtocoloPrestacao.ListaTodos();
                }
            }

            return dataTable;

        }

        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            Limpar();
            pnlDefinicao.Visible = true;
            btnNovo.Visible = false;
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Limpar();
                pnlDefinicao.Visible = false;
                btnNovo.Visible = true;
                ControlaAcesso(btnNovo, AcaoControle.novo);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                RN.PrestacaoContas.PlanoTrabalho rnPlanoTrabalho = new Techne.Lyceum.RN.PrestacaoContas.PlanoTrabalho();
                bool cadastro = false;

                var planoTrabalho = new RN.PrestacaoContas.Entidades.PlanoTrabalho
                {
                    FinalidadeId = !cmbFinalidade.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(this.cmbFinalidade.SelectedValue) : -1,
                    SuperintendenciaId = !cmbSuperintendencia.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(this.cmbSuperintendencia.SelectedValue) : -1,
                    TipoDespesaId = !cmbTipoDeDespesa.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(this.cmbTipoDeDespesa.SelectedValue) : -1,
                    TipoContratacaoId = !cmbTipoDeContratacao.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(this.cmbTipoDeContratacao.SelectedValue) : -1,
                    PlanoTrabalhoId = !string.IsNullOrEmpty(hdnplanotrabalhoid.Value) ? Convert.ToInt32(hdnplanotrabalhoid.Value) : 0,
                    Descricao = !string.IsNullOrEmpty(txtDescricao.Text) ? txtDescricao.Text : "",
                    Periodicidade = !cmbPeriodicidade.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? this.cmbPeriodicidade.SelectedValue : "",
                    ProgramaTrabalhoId = !string.IsNullOrEmpty(hdnprogramatrabalhoid.Value) ? Convert.ToInt32(hdnprogramatrabalhoid.Value) : 0,
                    PequenaDespesa = !ddlPequenaDespesa.SelectedValue.IsNullOrEmptyOrWhiteSpace() && ddlPequenaDespesa.SelectedValue == "1" ? true : false,
                    UsuarioId = User.Identity.Name,
                };

                cadastro = planoTrabalho.PlanoTrabalhoId == 0;

                if (cadastro)
                {
                    planoTrabalho.ProgramaTrabalhoId = !this.tseProgramaTrabalho.DBValue.IsNull && tseProgramaTrabalho.IsValidDBValue ? Convert.ToInt32(tseProgramaTrabalho.DBValue) : 0;
                }

                var validacao = rnPlanoTrabalho.Valida(planoTrabalho, cadastro);

                if (validacao.Valido)
                {
                    if (cadastro)
                    {
                        rnPlanoTrabalho.Insere(planoTrabalho);
                    }
                    else
                    {
                        rnPlanoTrabalho.Atualiza(planoTrabalho);
                    }

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup",
                        "alert('Projeto / Programa " + (cadastro ? " inserido " : " atualizado ") + "com sucesso.');", true);

                    grdDocumento.DataBind();
                    Limpar();
                    pnlDefinicao.Visible = false;
                    btnNovo.Visible = true;
                    ControlaAcesso(btnNovo, AcaoControle.novo);
                }
                else
                {
                    this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}