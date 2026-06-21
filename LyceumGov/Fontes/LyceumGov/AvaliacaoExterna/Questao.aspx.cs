using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using System.Data;
using DevExpress.Web.ASPxGridView;
using Techne.Web;
using Techne.Data;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.Data;

namespace Techne.Lyceum.Net.AvaliacaoExterna
{
    [NavUrl("~/AvaliacaoExterna/Questao.aspx")]
    [ControlText("Questões")]
    [Title("Questões")]
    public partial class Questao : TPage
    {
        ASPxTextBox txtNumero = null;
        ASPxComboBox ddlComponente = null;
        ASPxComboBox ddlHabilidade = null;
        public int habilidadeSelecionada = 0;

        public object ListaProva(int avaliacaoId)
        {
            RN.AvaliacaoExterna.Prova rnProva = new Techne.Lyceum.RN.AvaliacaoExterna.Prova();
            DataTable lista = rnProva.ListaAtivoPor(avaliacaoId);
            DataRow newRow = lista.NewRow();
            lista.Rows.InsertAt(newRow, 0);
            return lista;
        }

        public object ListaAnos()
        {
            RN.AvaliacaoExterna.Avaliacao rnAvaliacao = new Techne.Lyceum.RN.AvaliacaoExterna.Avaliacao();
            DataTable lista = rnAvaliacao.ListaAnos();
            DataRow newRow = lista.NewRow();
            lista.Rows.InsertAt(newRow, 0);
            return lista;
        }

        public object ListaAvaliacao(int ano)
        {
            RN.AvaliacaoExterna.Avaliacao rnAvaliacao = new Techne.Lyceum.RN.AvaliacaoExterna.Avaliacao();
            DataTable lista = rnAvaliacao.ListaAtivoPorAno(ano);
            DataRow newRow = lista.NewRow();
            lista.Rows.InsertAt(newRow, 0);
            return lista;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdQuestao, "Cadastro de Questões");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcessoGrid();
            plaGrid.Visible = ddlProva.SelectedIndex > 0;
            plaZero.Visible = !plaGrid.Visible;
        }

        protected void ControlaAcessoGrid()
        {
            ControlaAcesso(grdQuestao);

            if (!grdQuestao.IsEditing)
            {
                ControlaAcesso(grdQuestao, AcaoControle.excluir, "btnExcluir");
            }
        }

        public object ListaComponente()
        {
            RN.AvaliacaoExterna.Componente rnComponente = new Techne.Lyceum.RN.AvaliacaoExterna.Componente();
            DataTable lista = rnComponente.ListaAtivo();
            DataRow newRow = lista.NewRow();
            lista.Rows.InsertAt(newRow, 0);
            return lista;
        }

        public object ListaHabilidade()
        {
            RN.AvaliacaoExterna.Habilidade rnHabilidade = new Techne.Lyceum.RN.AvaliacaoExterna.Habilidade();
            DataTable lista = rnHabilidade.ListaAtivo();
            DataRow newRow = lista.NewRow();
            lista.Rows.InsertAt(newRow, 0);
            return lista;
        }

        public object Lista(int provaId)
        {
            if (provaId <= 0)
                return null;

            RN.AvaliacaoExterna.Questao rnQuestao = new Techne.Lyceum.RN.AvaliacaoExterna.Questao();
            return rnQuestao.ListaPorProva(provaId);
        }


        public void Insert(object PROVA, object NUMERO, object COMPONENTEID, object INDICEDIFICULDADE, object QUANTIDADEALTERNATIVAS, object ALTERNATIVACORRETA) { }
        public void Update(object PROVA, object NUMERO, object COMPONENTEID, object INDICEDIFICULDADE, object QUANTIDADEALTERNATIVAS, object ALTERNATIVACORRETA, object QUESTAOID) { }
        public void Delete(object QUESTAOID) { }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlAvaliacao.SelectedIndex = -1;
            ddlProva.SelectedIndex = -1;
            grdQuestao.CancelEdit();
            grdQuestao.DataBind();
        }

        protected void ddlAvaliacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlProva.SelectedIndex = -1;
            grdQuestao.CancelEdit();
            grdQuestao.DataBind();
        }

        protected void ddlProva_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdQuestao.CancelEdit();
            grdQuestao.DataBind();
        }

        protected void grdQuestao_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (grdQuestao.IsNewRowEditing)
            {
                if (e.Column.FieldName == "PROVA")
                {
                    e.Editor.Value = ddlProva.SelectedItem.Text;
                }
            }                     

            if (e.Column.FieldName == "COMPONENTEID")
                ddlComponente = e.Editor as ASPxComboBox;

            if (e.Column.FieldName == "HABILIDADEID")
                ddlHabilidade = e.Editor as ASPxComboBox;

            if (e.Column.FieldName == "NUMERO")
                e.Editor.Visible = !grdQuestao.IsNewRowEditing;

            grdQuestao.Settings.ShowFilterRow = !grdQuestao.IsEditing;
        }

        protected void grdQuestao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdQuestao);

            if (!grdQuestao.IsEditing)
            {
                ControlaAcesso(grdQuestao, AcaoControle.excluir, "btnExcluir");
            }
        }

        protected void grdQuestao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdQuestao.Settings.ShowFilterRow = false;
        }

        protected void grdQuestao_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdQuestao.Settings.ShowFilterRow = false;
        }
       
        protected void grdQuestao_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            habilidadeSelecionada = 0;

            try
            {
                RN.AvaliacaoExterna.Avaliacao rnAvaliacao = new Techne.Lyceum.RN.AvaliacaoExterna.Avaliacao();
                RN.AvaliacaoExterna.Questao rnQuestao = new Techne.Lyceum.RN.AvaliacaoExterna.Questao();
                RN.AvaliacaoExterna.Habilidade rnHabilidade = new Techne.Lyceum.RN.AvaliacaoExterna.Habilidade();
                ddlHabilidade = grdQuestao.FindEditRowCellTemplateControl((GridViewDataColumn)grdQuestao.Columns["HABILIDADEID"], "ddlHabilidade") as ASPxComboBox;

                ValidacaoDados validacao = new ValidacaoDados();
                RN.AvaliacaoExterna.Entidades.Questao questao = new Techne.Lyceum.RN.AvaliacaoExterna.Entidades.Questao();

                if (ddlHabilidade.SelectedItem == null && ddlHabilidade.Value == null)
                {
                    questao.HabilidadeId = -1;
                }
                else
                {
                    //!!!! G A M B I A R R A !!!! quando o gridview está sem registros, o ddlHabilidade.SelectedItem retorna nulo na hora da inserção. Forcei a barra para pegar o HABILIDADEID pelo ddlHabilidade.Value.
                    if (ddlHabilidade.SelectedItem == null || ddlHabilidade.SelectedItem.Value == null)
                        questao.HabilidadeId = rnHabilidade.ObtemIdPeloCodigo((ddlHabilidade.Value as string).Split(new char[] { '-' })[0].Trim());
                    else
                        questao.HabilidadeId = Convert.ToInt32(ddlHabilidade.SelectedItem.Value);
                }

                questao.ProvaId = ddlProva.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToInt32(ddlProva.SelectedValue);
                questao.IndiceDificuldade = Convert.ToString(e.NewValues["INDICEDIFICULDADE"]).IsNullOrEmptyOrWhiteSpace() ? -1 : Convert.ToDecimal(e.NewValues["INDICEDIFICULDADE"]);
                questao.QuantidadeAlternativas = Convert.ToString(e.NewValues["QUANTIDADEALTERNATIVAS"]).IsNullOrEmptyOrWhiteSpace() ? -1 : Convert.ToInt32(e.NewValues["QUANTIDADEALTERNATIVAS"]);
                questao.AlternativaCorreta = Convert.ToString(e.NewValues["ALTERNATIVACORRETA"]).IsNullOrEmptyOrWhiteSpace() ? -1 : Convert.ToInt32(e.NewValues["ALTERNATIVACORRETA"]);
                questao.UsuarioID = User.Identity.Name;

                validacao = rnQuestao.Valida(questao, true);

                if (validacao.Valido)
                {
                    rnQuestao.Insere(questao);
                }
                else
                {
                    habilidadeSelecionada = questao.HabilidadeId; //item para selecionar no grid caso
                    e.Cancel = true;
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }

                grdQuestao.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdQuestao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            habilidadeSelecionada = 0;

            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.AvaliacaoExterna.Entidades.Questao questao = new Techne.Lyceum.RN.AvaliacaoExterna.Entidades.Questao();
                RN.AvaliacaoExterna.Questao rnQuestao = new Techne.Lyceum.RN.AvaliacaoExterna.Questao();
                RN.AvaliacaoExterna.Habilidade rnHabilidade = new Techne.Lyceum.RN.AvaliacaoExterna.Habilidade();

                if (ddlHabilidade.SelectedItem == null && ddlHabilidade.Value == null)
                {
                    questao.HabilidadeId = -1;
                }
                else
                {
                    //!!!! G A M B I A R R A !!!! quando o gridview está sem registros, o ddlHabilidade.SelectedItem retorna nulo na hora da inserção. Forcei a barra para pegar o HABILIDADEID pelo ddlHabilidade.Value.
                    if (ddlHabilidade.SelectedItem == null || ddlHabilidade.SelectedItem.Value == null)
                        questao.HabilidadeId = rnHabilidade.ObtemIdPeloCodigo((ddlHabilidade.Value as string).Split(new char[] { '-' })[0].Trim());
                    else
                        questao.HabilidadeId = Convert.ToInt32(ddlHabilidade.SelectedItem.Value);
                }

                questao.QuestaoId = Convert.ToInt32(e.Keys["QUESTAOID"]);
                questao.ProvaId = ddlProva.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToInt32(ddlProva.SelectedValue);
                questao.Numero = Convert.ToString(e.NewValues["NUMERO"]).IsNullOrEmptyOrWhiteSpace() ? -1 : Convert.ToInt32(e.NewValues["NUMERO"]);
                questao.IndiceDificuldade = Convert.ToString(e.NewValues["INDICEDIFICULDADE"]).IsNullOrEmptyOrWhiteSpace() ? -1 : Convert.ToDecimal(e.NewValues["INDICEDIFICULDADE"]);
                questao.QuantidadeAlternativas = Convert.ToString(e.NewValues["QUANTIDADEALTERNATIVAS"]).IsNullOrEmptyOrWhiteSpace() ? -1 : Convert.ToInt32(e.NewValues["QUANTIDADEALTERNATIVAS"]);
                questao.AlternativaCorreta = Convert.ToString(e.NewValues["ALTERNATIVACORRETA"]).IsNullOrEmptyOrWhiteSpace() ? -1 : Convert.ToInt32(e.NewValues["ALTERNATIVACORRETA"]);
                questao.UsuarioID = User.Identity.Name;

                validacao = rnQuestao.Valida(questao, false);

                if (validacao.Valido)
                {
                    rnQuestao.Atualiza(questao);
                }
                else
                {
                    habilidadeSelecionada = questao.HabilidadeId; //item para selecionar no grid caso
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem);

                }

                grdQuestao.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnNao_Click(object sender, EventArgs e)
        {
            this.pucConfirmarItinerario.ShowOnPageLoad = false;
            grdQuestao.CancelEdit();
        }

        protected void btnSim_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.AvaliacaoExterna.Questao rnQuestao = new Techne.Lyceum.RN.AvaliacaoExterna.Questao();
                int questaoId = 0;

                questaoId = Convert.ToInt32(hdnIdQuestao.Value);

                validacao = rnQuestao.ValidaRemocao(questaoId);

                if (validacao.Valido)
                {
                    rnQuestao.Remove(questaoId);
                    grdQuestao.DataBind();
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    grdQuestao.CancelEdit();
                }

                this.pucConfirmarItinerario.ShowOnPageLoad = false;
            }

            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void grdQuestao_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            RN.AvaliacaoExterna.Resposta rnResposta = new Techne.Lyceum.RN.AvaliacaoExterna.Resposta();

            try
            {
                if (e.ButtonID == "btnExcluir")
                {
                    hdnIdQuestao.Value = Convert.ToString(grdQuestao.GetRowValues(e.VisibleIndex, "QUESTAOID"));

                    //Busca quantidade de respostas
                    int respostas = rnResposta.ObtemQuantidadePor(Convert.ToInt32(hdnIdQuestao.Value));

                    if (respostas > 0)
                    {
                        lblRespostas.Text = string.Format("{0} respostas serão excluídas.", respostas);
                    }
                    else
                    {
                        lblRespostas.Text = string.Empty;
                    }

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopup();", true);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlHabilidade_Load(object sender, EventArgs e)
        {
            RN.AvaliacaoExterna.Habilidade rnHabilidade = new Techne.Lyceum.RN.AvaliacaoExterna.Habilidade();
            ddlHabilidade = sender as ASPxComboBox;

            if (ddlComponente.SelectedItem == null || ddlComponente.SelectedItem.Value == null)
                return;

            if (ddlComponente.Value == null)
                return;

            int componenteId;
            int.TryParse(ddlComponente.SelectedItem.Value.ToString(), out componenteId);

            ddlHabilidade.Items.Clear();
            ddlHabilidade.DataSource = rnHabilidade.ListaPorComponente(componenteId);
            ddlHabilidade.DataBind();

            if (grdQuestao.IsEditing)
            {
                try
                {
                    if (grdQuestao.IsNewRowEditing && habilidadeSelecionada > 0)
                    {
                        ddlHabilidade.Items.FindByValue(habilidadeSelecionada.ToString()).Selected = true;
                    }
                    else
                    {
                        int habilidadeId = Convert.ToInt32(grdQuestao.GetDataRow(grdQuestao.EditingRowVisibleIndex)["HABILIDADEID"]);
                        ddlHabilidade.Items.FindByValue(habilidadeId.ToString()).Selected = true;
                    }
                }                    
                catch
                {
                }
            }
        }

        protected void txtNumero_Load(object sender, EventArgs e)
        {
            txtNumero = sender as ASPxTextBox;

            if (grdQuestao.IsNewRowEditing)
            {
                txtNumero.Style["visibility"] = "hidden";
                txtNumero.Text = "1";
            }
            else
            {
                txtNumero.Style["visibility"] = "visible";
            }
        }
    }
}