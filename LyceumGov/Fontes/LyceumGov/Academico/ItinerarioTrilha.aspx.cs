using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using Techne.Lyceum.RN.Entidades;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxTabControl;
using System.Web.UI.HtmlControls;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/ItinerarioTrilha.aspx"), ControlText("ItinerarioTrilha"), Title("Itinerário Formativo/Trilha de Aprendizagem")]

    public partial class ItinerarioTrilha : TPage
    {
        public object Lista()
        {
            RN.Pedagogico.ItinerarioFormativo rnItinerarioFormativo = new Techne.Lyceum.RN.Pedagogico.ItinerarioFormativo();

            return rnItinerarioFormativo.Lista();
        }

        public object ListaTrilha(object itinerario)
        {
            RN.Pedagogico.TrilhaAprendizagem rnTrilhaAprendizagem = new Techne.Lyceum.RN.Pedagogico.TrilhaAprendizagem();

            if (itinerario.ToString() == string.Empty)
            {
                return rnTrilhaAprendizagem.Lista();
            }
            else
            {
                return rnTrilhaAprendizagem.Lista(Convert.ToInt32(itinerario));
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                if (!this.IsPostBack)
                {
                    LimpaCamposItinerario();

                    Carregacategoria();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdItinerario, string.Empty);
            TituloGrid(grdTrilha, string.Empty);

        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdItinerario);
            ControlaAcesso(grdTrilha);
            ControlaAcesso(grdItinerario, AcaoControle.editar, "btnEditar");
            ControlaAcesso(grdItinerario, AcaoControle.excluir, "btnExcluir");
            ControlaAcesso(grdTrilha, AcaoControle.editar, "btnEditarTrilha");
            ControlaAcesso(grdTrilha, AcaoControle.excluir, "btnExcluirTrilha");
            if (Permission.AllowInsert)
            {
                ControlaAcesso(btnSalvarItinerario, AcaoControle.novo);
                ControlaAcesso(btnSalvarTrilha, AcaoControle.novo);
            }

            if (Permission.AllowUpdate)
            {
                ControlaAcesso(btnSalvarItinerario, AcaoControle.editar);
                ControlaAcesso(btnSalvarTrilha, AcaoControle.editar);
            }

            AcessoGrid();
        }

        private void Carregacategoria()
        {
            ListItem item = new ListItem("Selecione", string.Empty);
            RN.Turma rnTurma = new Turma();
            RN.Pedagogico.CategoriaItinerarioFormativo rnCategoriaItinerarioFormativo = new Techne.Lyceum.RN.Pedagogico.CategoriaItinerarioFormativo();


            ddlCategoria.Items.Clear();
            ddlCategoria.DataSource = rnCategoriaItinerarioFormativo.ListaCategoriaItinerarioFormativoAtivo();
            ddlCategoria.DataBind();
            ddlCategoria.Items.Insert(0, item);
        }


        protected void HabilitaPnlNovo(object sender, EventArgs e)
        {
            try
            {
                pnAbaItinerario.Visible = true;
                LimpaCamposItinerario();
                chkAtivo.Checked = true;                
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimpaCamposItinerario()
        {
            txtItinerario.Text = string.Empty;
            hdnIdItinerario.Value = string.Empty;
            chkAtivo.Checked = false;
            ddlCategoria.ClearSelection();
            txtObjetivoItinerario.Text = string.Empty;
            chkOfertaPesquisaItinerario.Checked = false;
        }

        protected void btnSalvarItinerario_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Pedagogico.ItinerarioFormativo rnItinerarioFormativo = new Techne.Lyceum.RN.Pedagogico.ItinerarioFormativo();
                RN.Pedagogico.Entidades.ItinerarioFormativo itinerarioFormativo = new Techne.Lyceum.RN.Pedagogico.Entidades.ItinerarioFormativo();

                itinerarioFormativo.CategoriaItinerarioFormativoId = !ddlCategoria.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlCategoria.SelectedValue) : -1;
                itinerarioFormativo.ItinerarioFormativoId = !hdnIdItinerario.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnIdItinerario.Value) : -1;
                itinerarioFormativo.Descricao = !txtItinerario.Text.IsNullOrEmptyOrWhiteSpace() ? txtItinerario.Text.Trim().ToUpper() : null;
                itinerarioFormativo.Objetivo = !txtObjetivoItinerario.Text.IsNullOrEmptyOrWhiteSpace() ? txtObjetivoItinerario.Text.Trim() : null;
                itinerarioFormativo.Ativo = chkAtivo.Checked;
                itinerarioFormativo.Oferta = chkOfertaPesquisaItinerario.Checked;
                itinerarioFormativo.UsuarioId = User.Identity.Name;

                validacao = rnItinerarioFormativo.Valida(itinerarioFormativo, itinerarioFormativo.ItinerarioFormativoId == -1 ? true : false);

                if (validacao.Valido)
                {
                    if (itinerarioFormativo.ItinerarioFormativoId == -1)
                    {
                        rnItinerarioFormativo.Insere(itinerarioFormativo);
                    }
                    else
                    {
                        rnItinerarioFormativo.Atualiza(itinerarioFormativo);
                    }
                    grdItinerario.DataBind();
                    LimpaCamposItinerario();
                    pnAbaItinerario.Visible = false;

                    lblMensagem.Text = "Itinerário " + (itinerarioFormativo.ItinerarioFormativoId == -1 ? "cadastrado" : "atualizado") + " com sucesso.";
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");

                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }



        protected void pcItinerarioTrilha_TabClick(object source, TabControlCancelEventArgs e)
        {
            this.lblMensagem.Text = string.Empty;
        }

        protected void grdItinerario_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdItinerario.Settings.ShowFilterRow = false;
        }

        protected void grdItinerario_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdItinerario.Settings.ShowFilterRow = false;
        }

        protected void grdItinerario_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdItinerario);
            ControlaAcesso(grdItinerario, AcaoControle.editar, "btnEditar");
            ControlaAcesso(grdItinerario, AcaoControle.excluir, "btnExcluir");
            AcessoGrid();
        }

        protected void grdItinerario_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            try
            {
               
                if (e.ButtonID == "btnEditar")
                {

                    LimpaCamposItinerario();

                    chkAtivo.Checked = Convert.ToBoolean(grdItinerario.GetRowValues(e.VisibleIndex, "ATIVO"));
                    txtItinerario.Text = grdItinerario.GetRowValues(e.VisibleIndex, "DESCRICAO").ToString();
                    hdnIdItinerario.Value = Convert.ToString(grdItinerario.GetRowValues(e.VisibleIndex, "ITINERARIOFORMATIVOID"));
                    ddlCategoria.SelectedValue = Convert.ToString(grdItinerario.GetRowValues(e.VisibleIndex, "CATEGORIAITINERARIOFORMATIVOID"));
                    txtObjetivoItinerario.Text = Convert.ToString(grdItinerario.GetRowValues(e.VisibleIndex, "OBJETIVO"));
                    chkOfertaPesquisaItinerario.Checked = Convert.ToBoolean(grdItinerario.GetRowValues(e.VisibleIndex, "OFERTA"));
                    pnAbaItinerario.Visible = true;

                }

                if (e.ButtonID == "btnExcluir")
                {
                    hdnIdItinerario.Value = Convert.ToString(grdItinerario.GetRowValues(e.VisibleIndex, "ITINERARIOFORMATIVOID"));

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopup();", true);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSim_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Pedagogico.ItinerarioFormativo rnItinerarioFormativo = new Techne.Lyceum.RN.Pedagogico.ItinerarioFormativo();

                int itinerarioId = 0;

                itinerarioId = Convert.ToInt32(hdnIdItinerario.Value);

                validacao = rnItinerarioFormativo.ValidaRemocao(itinerarioId);

                if (validacao.Valido)
                {
                    rnItinerarioFormativo.Remove(itinerarioId);
                    grdItinerario.DataBind();
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");                    
                    grdItinerario.CancelEdit();
                }

                this.pucConfirmarItinerario.ShowOnPageLoad = false;
            }

            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnNao_Click(object sender, EventArgs e)
        {
            this.pucConfirmarItinerario.ShowOnPageLoad = false;
            grdItinerario.CancelEdit();
        }


        public void Delete(object ITINERARIOFORMATIVOID) { }


        protected void grdItinerario_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Pedagogico.ItinerarioFormativo rnItinerarioFormativo = new Techne.Lyceum.RN.Pedagogico.ItinerarioFormativo();
            int itinerarioId = 0;

            itinerarioId = Convert.ToInt32(e.Keys["ITINERARIOFORMATIVOID"]);

            validacao = rnItinerarioFormativo.ValidaRemocao(itinerarioId);

            if (validacao.Valido)
            {
                rnItinerarioFormativo.Remove(itinerarioId);
                grdItinerario.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }


        protected void HabilitaPnlNovaTrilha(object sender, EventArgs e)
        {
            try
            {
                pnlAbaTrilha.Visible = true;
                LimpaCamposTrilha();
                chkTrilhaAtiva.Checked = true;
                
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimpaCamposTrilha()
        {
            txtTrilha.Text = string.Empty;
            hdnIdTrilha.Value = string.Empty;
            chkTrilhaAtiva.Checked = false;
            tseItinerario.ResetValue();
            grdTrilha.DataBind();
            ddlTipo.ClearSelection();
            ddlTipo.SelectedValue = string.Empty;
            txtObjetivoTrilha.Text = string.Empty;
            chkOfertaPesquisaTrilha.Checked = false;
        }

       protected void btnSalvarTrilha_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Pedagogico.TrilhaAprendizagem rnTrilhaAprendizagem = new Techne.Lyceum.RN.Pedagogico.TrilhaAprendizagem();
                RN.Pedagogico.Entidades.TrilhaAprendizagem trilha = new Techne.Lyceum.RN.Pedagogico.Entidades.TrilhaAprendizagem();

                trilha.TrilhaAprendizagemId = !hdnIdTrilha.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnIdTrilha.Value) : -1;
                trilha.ItinerarioFormativoId = (!this.tseItinerario.DBValue.IsNull && this.tseItinerario.IsValidDBValue) ? Convert.ToInt32(tseItinerario.DBValue) : -1;
                trilha.Descricao = !txtTrilha.Text.IsNullOrEmptyOrWhiteSpace() ? txtTrilha.Text.Trim().ToUpper() : null;
                trilha.Tipo = !ddlTipo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTipo.SelectedValue : null;
                trilha.Objetivo = !txtObjetivoTrilha.Text.IsNullOrEmptyOrWhiteSpace() ? txtObjetivoTrilha.Text.Trim() : null;
                trilha.Ativo = chkTrilhaAtiva.Checked;
                trilha.Oferta = chkOfertaPesquisaTrilha.Checked;
                trilha.UsuarioId = User.Identity.Name;

                validacao = rnTrilhaAprendizagem.Valida(trilha, trilha.TrilhaAprendizagemId == -1 ? true : false);

                if (validacao.Valido)
                {
                    if (trilha.TrilhaAprendizagemId == -1)
                    {
                        rnTrilhaAprendizagem.Insere(trilha);
                    }
                    else
                    {
                        rnTrilhaAprendizagem.Atualiza(trilha);
                    }
                    grdTrilha.DataBind();
                    LimpaCamposTrilha();
                    pnlAbaTrilha.Visible = false;

                    lblMensagem.Text = "Trilha " + (trilha.TrilhaAprendizagemId == -1 ? "cadastrada" : "atualizada") + " com sucesso.";
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");

                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdTrilha_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        { 
            grdTrilha.Settings.ShowFilterRow = false;
        }

        protected void grdTrilha_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdTrilha.Settings.ShowFilterRow = false;
        }

        protected void grdTrilha_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdTrilha);
            ControlaAcesso(grdTrilha, AcaoControle.editar, "btnEditarTrilha");
            ControlaAcesso(grdTrilha, AcaoControle.excluir, "btnExcluirTrilha");
            AcessoGrid();
        }

        protected void grdTrilha_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {


            if (e.ButtonID == "btnEditarTrilha")
            {
                try
                {
                    LimpaCamposTrilha();

                    chkTrilhaAtiva.Checked = Convert.ToBoolean(grdTrilha.GetRowValues(e.VisibleIndex, "ATIVO"));
                    txtTrilha.Text = Convert.ToString(grdTrilha.GetRowValues(e.VisibleIndex, "DESCRICAO"));
                    hdnIdTrilha.Value = Convert.ToString(grdTrilha.GetRowValues(e.VisibleIndex, "TRILHAAPRENDIZAGEMID"));
                    tseItinerario.Value = Convert.ToString(grdTrilha.GetRowValues(e.VisibleIndex, "ITINERARIOFORMATIVOID"));
                    ddlTipo.SelectedValue = Convert.ToString(grdTrilha.GetRowValues(e.VisibleIndex, "TIPO"));
                    txtObjetivoTrilha.Text = Convert.ToString(grdTrilha.GetRowValues(e.VisibleIndex, "OBJETIVO"));
                    chkOfertaPesquisaTrilha.Checked = Convert.ToBoolean(grdTrilha.GetRowValues(e.VisibleIndex, "OFERTA"));
                    pnlAbaTrilha.Visible = true;

                }
                catch (Exception ex)
                {
                    lblMensagem.Text = ex.Message;
                }
            }

            if (e.ButtonID == "btnExcluirTrilha")
            {
                hdnIdTrilha.Value = Convert.ToString(grdTrilha.GetRowValues(e.VisibleIndex, "TRILHAAPRENDIZAGEMID"));

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopupTrilha();", true);
            }
        }

        public void DeleteTrilha(object TRILHAAPRENDIZAGEMID) { }

        protected void grdTrilha_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Pedagogico.TrilhaAprendizagem rnTrilhaAprendizagem = new Techne.Lyceum.RN.Pedagogico.TrilhaAprendizagem();
            int trilhaId = 0;

            trilhaId = Convert.ToInt32(e.Keys["TRILHAAPRENDIZAGEMID"]);

            validacao = rnTrilhaAprendizagem.ValidaRemocao(trilhaId);

            if (validacao.Valido)
            {
                rnTrilhaAprendizagem.Remove(trilhaId);
                grdTrilha.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }


        protected void btnSimTrilha_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Pedagogico.TrilhaAprendizagem rnTrilhaAprendizagem = new Techne.Lyceum.RN.Pedagogico.TrilhaAprendizagem();
                int trilhaId = 0;

                trilhaId = Convert.ToInt32(hdnIdTrilha.Value);

                validacao = rnTrilhaAprendizagem.ValidaRemocao(trilhaId);

                if (validacao.Valido)
                {
                    rnTrilhaAprendizagem.Remove(trilhaId);
                    grdTrilha.DataBind();
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    grdTrilha.CancelEdit();
                }
                this.pucConfirmarTrilha.ShowOnPageLoad = false;
            }

            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnNaoTrilha_Click(object sender, EventArgs e)
        {
            this.pucConfirmarTrilha.ShowOnPageLoad = false;
            grdTrilha.CancelEdit();
        }

        protected void AcessoGrid()
        {
            if (grdItinerario != null)
            {
                HtmlInputImage img = (HtmlInputImage)grdItinerario.FindHeaderTemplateControl(grdItinerario.Columns[""], "btnNovoGridIti");
                HtmlInputImage imgT = (HtmlInputImage)grdTrilha.FindHeaderTemplateControl(grdTrilha.Columns[""], "btnNovoGridTrilha");
                

                if (img != null)
                {
                    img.Visible = Permission.AllowInsert;
                    imgT.Visible = Permission.AllowInsert;

                   
                }
            }
        }
    }
}
