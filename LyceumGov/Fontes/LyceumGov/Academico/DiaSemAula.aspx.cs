using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Web.SessionState;
using System.Web.UI.HtmlControls;
using Techne.Web;
using Techne.Data;
using Techne.Lyceum.CR;
using System.Web.UI.MobileControls;
using Techne.Lyceum.RN;
using System.IO;
using DevExpress.Web.ASPxClasses;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxGridView;


namespace Techne.Lyceum.Net.Academico
{
    [
     NavUrl("~/Academico/DiaSemAula.aspx"),
      ControlText("DiaSemAula"),
      Title("Cadastro Reposição Aula"),
    ]
    public partial class DiaSemAula : TPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                if (!IsPostBack)
                {
                    LimparTela();
                    CarregaMotivo();
                    pnlReposicao.Visible = false;
                    btnCancel.Visible = false;
                    btnNovo.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(grdReposicao, AcaoControle.editar, "btnEditar");
            ControlaAcesso(grdReposicao, AcaoControle.excluir, "btnExcluir");
            ControlaAcesso(grdReposicao);
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdReposicao, "Dia Sem Aula");
        }


        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            LimparTela();
            dtDataSemAula.Enabled = true;
            pnlReposicao.Visible = true;
            btnCancel.Visible = true;
            btnNovo.Visible = false;
        }


        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            LimparTela();
            pnlReposicao.Visible = false;
            btnCancel.Visible = false;
            btnNovo.Visible = true;
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Turmas.DiaSemAula rnDiaSemAula = new Techne.Lyceum.RN.Turmas.DiaSemAula();
                RN.Turmas.Entidades.DiaSemAula diaSemAula = new Techne.Lyceum.RN.Turmas.Entidades.DiaSemAula();

                diaSemAula.DiaSemAulaId = !hdnIdDiaSemAula.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnIdDiaSemAula.Value) : -1;
                diaSemAula.Data = !dtDataSemAula.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataSemAula.Date : DateTime.MinValue;
                diaSemAula.MotivoDiaSemAulaId = !ddlMotivo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlMotivo.SelectedValue) : -1;
                diaSemAula.Justificativa = !txtJustificativa.Text.IsNullOrEmptyOrWhiteSpace() ? txtJustificativa.Text.Trim().ToUpper() : null;
                diaSemAula.ProcessoSei = !txtProcessoSEI.Text.IsNullOrEmptyOrWhiteSpace() ? txtProcessoSEI.Text.Trim() : null;
                diaSemAula.Censo = (tseUnidade.IsValidDBValue && !tseUnidade.DBValue.IsNull) ? tseUnidade.DBValue.ToString() : null;
                diaSemAula.DataReposicao = !dtReposicao.Text.IsNullOrEmptyOrWhiteSpace() ? dtReposicao.Date : DateTime.MinValue;
                diaSemAula.UsuarioId = User.Identity.Name;

                validacao = rnDiaSemAula.Valida(diaSemAula, diaSemAula.DiaSemAulaId == -1 ? true : false);

                if (validacao.Valido)
                {
                    if (diaSemAula.DiaSemAulaId == -1)
                    {
                        rnDiaSemAula.Insere(diaSemAula);
                    }
                    else
                    {
                        rnDiaSemAula.Atualiza(diaSemAula);
                    }
                    odsReposicao.DataBind();
                    grdReposicao.DataBind();
                    LimparTela();
                    pnlReposicao.Visible = false;
                    btnCancel.Visible = false;
                    btnNovo.Visible = true;


                    lblMensagem.Text = "Reposição de Aula " + (diaSemAula.DiaSemAulaId == -1 ? "cadastrada" : "atualizada") + " com sucesso.";
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

        protected void grdReposicao_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            try
            {

                if (e.ButtonID == "btnEditar")
                {
                    LimparTela();
                    hdnIdDiaSemAula.Value = Convert.ToString(grdReposicao.GetRowValues(e.VisibleIndex, "DIASEMAULAID"));
                    dtDataSemAula.Date = Convert.ToDateTime(grdReposicao.GetRowValues(e.VisibleIndex, "DATA"));
                    dtDataSemAula.Enabled = false;
                    txtProcessoSEI.Text = grdReposicao.GetRowValues(e.VisibleIndex, "PROCESSOSEI").ToString();
                    ddlMotivo.SelectedValue = Convert.ToString(grdReposicao.GetRowValues(e.VisibleIndex, "MOTIVODIASEMAULAID"));
                    txtJustificativa.Text = Convert.ToString(grdReposicao.GetRowValues(e.VisibleIndex, "JUSTIFICATIVA"));
                    dtReposicao.Date = Convert.ToDateTime(grdReposicao.GetRowValues(e.VisibleIndex, "DATAREPOSICAO"));
                    pnlReposicao.Visible = true;
                    btnNovo.Visible = false;
                    btnCancel.Visible = true;

                }

                if (e.ButtonID == "btnExcluir")
                {
                    hdnIdDiaSemAula.Value = Convert.ToString(grdReposicao.GetRowValues(e.VisibleIndex, "DIASEMAULAID"));
                    dtDataSemAula.Date = Convert.ToDateTime(grdReposicao.GetRowValues(e.VisibleIndex, "DATA"));

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
                RN.Turmas.DiaSemAula rnDiaSemAula = new Techne.Lyceum.RN.Turmas.DiaSemAula();

                int diaSemAulaId = 0;

                diaSemAulaId = Convert.ToInt32(hdnIdDiaSemAula.Value);
                string censo = (tseUnidade.IsValidDBValue && !tseUnidade.DBValue.IsNull) ? tseUnidade.DBValue.ToString() : null;
                DateTime data = !dtDataSemAula.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataSemAula.Date : DateTime.MinValue;

                validacao = rnDiaSemAula.ValidaRemocao(diaSemAulaId, censo, data);

                if (validacao.Valido)
                {
                    rnDiaSemAula.Remove(diaSemAulaId, censo, data);
                    grdReposicao.DataBind();
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    grdReposicao.CancelEdit();
                }

                this.pucConfirmar.ShowOnPageLoad = false;
            }

            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnNao_Click(object sender, EventArgs e)
        {
            this.pucConfirmar.ShowOnPageLoad = false;
            grdReposicao.CancelEdit();
        }

        private void LimparTela()
        {
            hdnIdDiaSemAula.Value = string.Empty;
            dtDataSemAula.Text = string.Empty;
            ddlMotivo.ClearSelection();
            txtProcessoSEI.Text = string.Empty;
            txtJustificativa.Text = string.Empty;
            dtReposicao.Text = string.Empty;
        }

        private void CarregaMotivo()
        {
            ListItem item = new ListItem("Selecione", string.Empty);
            RN.Turmas.MotivoDiaSemAula rnMotivoDiaSemAula = new Techne.Lyceum.RN.Turmas.MotivoDiaSemAula();


            ddlMotivo.Items.Clear();
            ddlMotivo.DataSource = rnMotivoDiaSemAula.ListaAtivoPor();
            ddlMotivo.DataBind();
            ddlMotivo.Items.Insert(0, item);
        }

        protected void tseUnidade_Changed(object sender, EventArgs e)
        {
            try
            {
                LimparTela();
                btnNovo.Visible = false;
                btnCancel.Visible = false;
                pnlReposicao.Visible = false;

                if (tseUnidade.IsValidDBValue && !tseUnidade.DBValue.IsNull)
                {
                    btnNovo.Visible = true;
                }
                else
                {
                    if (!tseUnidade.DBValue.IsNull)
                    {
                        lblMensagem.Text = "Unidade de Ensino não cadastrada.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public object Lista(object unidade)
        {
            RN.Turmas.DiaSemAula rnDiaSemAula = new Techne.Lyceum.RN.Turmas.DiaSemAula();

            string unidadeEnsino = unidade != null ? unidade.ToString() : string.Empty;

            if (!string.IsNullOrEmpty(unidadeEnsino))
                return rnDiaSemAula.ListaPor(unidade.ToString());


            return null;
        }

        //public void Update(object CENSO, object ATIVO, object DIASEMAULAID) { }
        //public void Delete(object DIASEMAULAID) { }


        protected void grdReposicao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdReposicao);
        }

        protected void grdReposicao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdReposicao.Settings.ShowFilterRow = false;
        }

        protected void grdReposicao_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdReposicao.Settings.ShowFilterRow = false;
        }

    }
}