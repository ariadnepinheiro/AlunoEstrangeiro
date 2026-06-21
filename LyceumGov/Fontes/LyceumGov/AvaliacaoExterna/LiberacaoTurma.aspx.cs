using System;
using System.Data;
using System.Web;
using DevExpress.Web.ASPxGridView;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using DevExpress.Web.Data;
using DevExpress.Web.ASPxEditors;

namespace Techne.Lyceum.Net.AvaliacaoExterna
{
    [NavUrl("~/AvaliacaoExterna/LiberacaoTurma.aspx")]
    [ControlText("Liberação de Período de Transcrição")]
    [Title("Liberação de Período de Transcrição")]
    public partial class LiberacaoTurma : TPage
    {
        public readonly RN.AvaliacaoExterna.ReaberturaTurma rnReaberturaTurma;

        public LiberacaoTurma()
        {
            rnReaberturaTurma = new Techne.Lyceum.RN.AvaliacaoExterna.ReaberturaTurma();
        }

        public DataTable Lista(object avaliacaoId, object periodo, object regional, object municipio, object unidadeEnsino)
        {
            if (avaliacaoId == null || periodo == null)
                return null;

            return rnReaberturaTurma.ConsultarSolicitacoes(Convert.ToInt32(avaliacaoId), Convert.ToInt32(periodo), Convert.ToString(regional), Convert.ToString(municipio), Convert.ToString(unidadeEnsino), User.Identity.Name);
        }
        public void Update(object DATAFECHAMENTO, object REABERTURATURMAID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdLiberacaoTurma, "Liberação de Período de Transcrição");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdLiberacaoTurma);
            ControlaAcesso(grdLiberacaoTurma, AcaoControle.editar, "btnReprovar");
        }

        protected void tseAvaliacao_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                ddlPeriodo.ClearSelection();
                tseRegional.ResetValue();
                tseMunicipio.ResetValue();
                tseUnidadeResponsavel.ResetValue();
                grdLiberacaoTurma.DataBind();
                pnlOperacoes.Visible = false;
                dtfechamento.Text = string.Empty;

                if (!ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace() && (tseAvaliacao.IsValidDBValue && !tseAvaliacao.DBValue.IsNull))
                {
                    pnlOperacoes.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                tseMunicipio.ResetValue();
                tseUnidadeResponsavel.ResetValue();
                grdLiberacaoTurma.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            tseUnidadeResponsavel.ResetValue();
        }

        protected void tseUnidadeResponsavel_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (tseUnidadeResponsavel.IsValidDBValue && !tseUnidadeResponsavel["unidade_ens"].IsNull)
                {
                    tseRegional.Value = tseUnidadeResponsavel["id_regional"];
                    tseMunicipio.Value = tseUnidadeResponsavel["municipio"];
                }

                grdLiberacaoTurma.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                tseRegional.ResetValue();
                tseMunicipio.ResetValue();
                tseUnidadeResponsavel.ResetValue();
                grdLiberacaoTurma.DataBind();
                pnlOperacoes.Visible = false;
                dtfechamento.Text = string.Empty;

                if (!ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace() && (tseAvaliacao.IsValidDBValue && !tseAvaliacao.DBValue.IsNull))
                {
                    pnlOperacoes.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdLiberacaoTurma_HtmlCommandCellPrepared(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableCommandCellEventArgs e)
        {
            if (grdLiberacaoTurma.IsEditing && e.VisibleIndex == grdLiberacaoTurma.EditingRowVisibleIndex)
            {
                if (e.Cell.Controls.Count == 3)
                {
                    e.Cell.Controls[2].Visible = false;
                }
            }
        }

        protected void grdLiberacaoTurma_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;

            if (e.CellType == GridViewTableCommandCellType.Filter)
            {
                e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                return;
            }

            string situacao = Convert.ToString(grdLiberacaoTurma.GetRowValues(e.VisibleIndex, "STATUS"));

            if (e.ButtonID == "btnReprovar")
            {
                if (situacao != "PENDENTE")
                {
                    e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                }
            }
        }

        protected void grdLiberacaoTurma_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            string situacao = Convert.ToString(grdLiberacaoTurma.GetRowValues(e.VisibleIndex, "STATUS"));

            if (situacao != "PENDENTE")
            {
                if (e.ButtonType == ColumnCommandButtonType.Edit)
                {
                    e.Visible = false;
                }
            }
        }

        protected void grdLiberacaoTurma_StartRowEditing(object sender, EventArgs e)
        {
            grdLiberacaoTurma.Settings.ShowFilterRow = false;

            //Coloca todas as calunos invisiveis
            for (int i = 2; i < grdLiberacaoTurma.Columns.Count; i++)
                grdLiberacaoTurma.Columns[i].Visible = false;

            //Volta visibilidade da data de fechamento
            grdLiberacaoTurma.Columns["DATAFECHAMENTO"].Visible = true;
        }

        protected void grdLiberacaoTurma_CancelRowEditing(object sender, EventArgs e)
        {
            //Volta visibilidade das colunas
            for (int i = 2; i < grdLiberacaoTurma.Columns.Count; i++)
                grdLiberacaoTurma.Columns[i].Visible = true;
        }

        protected void grdLiberacaoTurma_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            DataRow dataRow = grdLiberacaoTurma.GetDataRow(grdLiberacaoTurma.EditingRowVisibleIndex);
            int reaberturaTurmaId = Convert.ToString(dataRow["REABERTURATURMAID"]).IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToInt32(dataRow["REABERTURATURMAID"]);
            string aprovadorId = User.Identity.Name;
            DateTime dataFechamento = Convert.ToString(e.NewValues["DATAFECHAMENTO"]).IsNullOrEmptyOrWhiteSpace() ? DateTime.MinValue : Convert.ToDateTime(e.NewValues["DATAFECHAMENTO"]);
            string usuarioId = User.Identity.Name;

            validacao = rnReaberturaTurma.ValidaLiberacao(reaberturaTurmaId, aprovadorId, dataFechamento, usuarioId);

            if (validacao.Valido)
            {
                rnReaberturaTurma.Aprova(reaberturaTurmaId, aprovadorId, dataFechamento, usuarioId);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdLiberacaoTurma_CancelRowEditing(sender, e);
            grdLiberacaoTurma.DataBind();
        }

        protected void grdLiberacaoTurma_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            switch (e.ButtonID)
            {
                case "btnReprovar":
                    DataRow dataRow = grdLiberacaoTurma.GetDataRow(e.VisibleIndex);

                    ValidacaoDados validacao = new ValidacaoDados();

                    int reaberturaTurmaId = Convert.ToInt32(dataRow["REABERTURATURMAID"]);
                    string aprovadorId = User.Identity.Name;
                    string usuarioId = User.Identity.Name;

                    rnReaberturaTurma.Reprova(reaberturaTurmaId, aprovadorId, usuarioId);

                    grdLiberacaoTurma_CancelRowEditing(sender, e);
                    grdLiberacaoTurma.DataBind();
                    break;
            }
        }

        protected void rblTipoOperacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pnlData.Visible = false;
                dtfechamento.Text = string.Empty;

                if (rblTipoOperacao.SelectedValue == "A")
                {
                    pnlData.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void btExecutar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();

                if (rblTipoOperacao.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    lblMensagem.Text = "É obrigatório escolher uma das opções - Aprovar ou Reprovar.";
                    return;
                }
                else
                {
                    if (rblTipoOperacao.SelectedValue == "A" && dtfechamento.Text.IsNullOrEmptyOrWhiteSpace())
                    {
                        lblMensagem.Text = "Para a opção de Aprovar é necessário informar a Nova Data de Fechamento.";
                        return;
                    }
                }

                var ids = new System.Collections.Generic.List<int>();

                for (var rowIndex = 0; rowIndex < this.grdLiberacaoTurma.VisibleRowCount; rowIndex++)
                {
                    var id = (int)this.grdLiberacaoTurma.GetRowValues(rowIndex, "REABERTURATURMAID");

                    var status = (string)this.grdLiberacaoTurma.GetRowValues(rowIndex, "STATUS");

                    if (status == "PENDENTE")
                    {
                        ids.Add(id);
                    }
                }
                if (ids.Count > 0)
                {
                    if (rblTipoOperacao.SelectedValue == "A")
                    {
                        validacao = rnReaberturaTurma.ValidaAprovaTodos(ids, dtfechamento.Date, User.Identity.Name);

                        if (validacao.Valido)
                        {

                            rnReaberturaTurma.AprovaTodas(ids, dtfechamento.Date, User.Identity.Name);
                            lblMensagem.Text = "Liberações aprovadas com sucesso.";
                        }
                        else
                        {
                            lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        }
                    }
                    else
                    {
                        rnReaberturaTurma.ReprovaTodos(ids, User.Identity.Name);
                        lblMensagem.Text = "Liberações reprovadas com sucesso.";
                    }
                }
                else
                {
                    lblMensagem.Text = "Não existe pedido para liberação.";
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


    }
}