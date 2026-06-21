using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;


namespace Techne.Lyceum.Net.PrestacaoContas
{
    [NavUrl("~/PrestacaoContas/UnidadeEnsinoImpedida.aspx")]
    [ControlText("UnidadeEnsinoImpedida")]
    [Title("Unidade de Ensino Impedida")]
    public partial class UnidadeEnsinoImpedida : TPage
    {
        public object Lista(object unidade)
        {
            RN.PrestacaoContas.UnidadeEnsinoImpedida rnUnidadeEnsinoImpedida = new Techne.Lyceum.RN.PrestacaoContas.UnidadeEnsinoImpedida();

            return rnUnidadeEnsinoImpedida.ListaPor(Convert.ToString(unidade));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    CarregaMotivoImpedimento();
                    LimpaCampos();
                    pnlNovoImpedimento.Visible = false;
                    ImageButton[] controles = new ImageButton[] { btnNovo };
                    ControlarVisibilidadeControle(controles);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdUnidadeEnsinoImpedida);
            ControlaAcesso(btnNovo, AcaoControle.novo);
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdUnidadeEnsinoImpedida, string.Empty);
        }

        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (var botao in botoes)
            {
                botao.Visible = true;
            }

            ControlaAcesso(btnNovo, AcaoControle.novo);
        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnNovo.Visible = false;
            btnSalvar.Visible = false;          
        }

        protected void tseUnidadeResponsavel_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (!this.tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (!this.tseUnidadeResponsavel.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Unidade não cadastrada.";
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar uma unidade.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeImpedida_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (!this.tseUnidadeImpedida.DBValue.IsNull)
                {
                    if (!this.tseUnidadeImpedida.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Unidade não cadastrada.";
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar uma unidade.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaMotivoImpedimento()
        {
            RN.PrestacaoContas.MotivoImpedimento rnMotivoImpedimento = new Techne.Lyceum.RN.PrestacaoContas.MotivoImpedimento();
            ddlMotivoImpedimento.Items.Clear();
            ddlMotivoImpedimento.DataSource = rnMotivoImpedimento.ListaAtivoPor();
            ddlMotivoImpedimento.DataBind();
            ddlMotivoImpedimento.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        private void LimpaCampos()
        {
            tseUnidadeImpedida.ResetValue();
            ddlMotivoImpedimento.ClearSelection();
            dtDataInicio.Text = string.Empty;
            dtDataFim.Text = string.Empty;
        }

        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                LimpaCampos();
                pnlNovoImpedimento.Visible = true;
                ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                ControlarVisibilidadeControle(controles);

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                LimpaCampos();
                pnlNovoImpedimento.Visible = false;

                ImageButton[] controles = new ImageButton[] { btnNovo };
                ControlarVisibilidadeControle(controles);
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
                ValidacaoDados validacao = new ValidacaoDados();
                RN.PrestacaoContas.UnidadeEnsinoImpedida rnUnidadeEnsinoImpedida = new Techne.Lyceum.RN.PrestacaoContas.UnidadeEnsinoImpedida();
                RN.PrestacaoContas.Entidades.UnidadeEnsinoImpedida unidade = new Techne.Lyceum.RN.PrestacaoContas.Entidades.UnidadeEnsinoImpedida();

                unidade.Censo = (tseUnidadeImpedida.IsValidDBValue && !tseUnidadeImpedida.DBValue.IsNull) ? tseUnidadeImpedida.DBValue.ToString() : null;
                unidade.DataInicio = !dtDataInicio.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataInicio.Date : DateTime.MinValue;
                unidade.DataFim = !dtDataFim.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataFim.Date : (DateTime?)null;
                unidade.MotivoImpedimentoId = !ddlMotivoImpedimento.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlMotivoImpedimento.SelectedValue) : -1;
                unidade.UsuarioId = User.Identity.Name;

                validacao = rnUnidadeEnsinoImpedida.Valida(unidade, true);

                if (validacao.Valido)
                {
                    rnUnidadeEnsinoImpedida.Insere(unidade);
                    LimpaCampos();
                    pnlNovoImpedimento.Visible = false;
                    grdUnidadeEnsinoImpedida.DataBind();
                    ImageButton[] controles = new ImageButton[] { btnNovo };
                    ControlarVisibilidadeControle(controles);
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

        protected void rblTipoFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pnlFiltros.Visible = false;
                tseUnidadeResponsavel.ResetValue();

                if (rblTipoFiltro.SelectedValue == "U")
                {
                    pnlFiltros.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void grdUnidadeEnsinoImpedida_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdUnidadeEnsinoImpedida);
        }

        protected void grdUnidadeEnsinoImpedida_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdUnidadeEnsinoImpedida.Settings.ShowFilterRow = false;
        }

        protected void grdUnidadeEnsinoImpedida_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdUnidadeEnsinoImpedida.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "DATAINICIO")
                    e.Editor.Enabled = true;

                if ((e.Column.FieldName) == "REGIONAL")
                    e.Editor.Enabled = true;
                if ((e.Column.FieldName) == "ESCOLA")
                    e.Editor.Enabled = true;
                if ((e.Column.FieldName) == "CENSO")
                    e.Editor.Enabled = true;
                if ((e.Column.FieldName) == "MOTIVO")
                    e.Editor.Enabled = true;

               
            }
            else if (grdUnidadeEnsinoImpedida.IsEditing)
            {
                if ((e.Column.FieldName) == "DATAINICIO")
                    e.Editor.Enabled = false;

               
            }
        }


        protected void grdUnidadeEnsinoImpedida_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.UnidadeEnsinoImpedida rnUnidadeEnsinoImpedida = new Techne.Lyceum.RN.PrestacaoContas.UnidadeEnsinoImpedida();
            RN.PrestacaoContas.Entidades.UnidadeEnsinoImpedida unidade = new Techne.Lyceum.RN.PrestacaoContas.Entidades.UnidadeEnsinoImpedida();

            var motivo = ((ASPxGridView)sender).GetRowValuesByKeyValue(e.Keys[0], "MOTIVOIMPEDIMENTOID");
            var dataInicio = ((ASPxGridView)sender).GetRowValuesByKeyValue(e.Keys[0], "DATAINICIO");
            
            unidade.Censo = e.NewValues["CENSO"] != null ? e.NewValues["CENSO"].ToString() : null;
            unidade.DataInicio = Convert.ToDateTime(dataInicio) ;
            unidade.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            unidade.MotivoImpedimentoId = Convert.ToInt32(motivo);
            unidade.UnidadeEnsinoImpedidaId = Convert.ToInt32(e.Keys["UNIDADEENSINOIMPEDIDAID"]);
            unidade.UsuarioId = User.Identity.Name;

            validacao = rnUnidadeEnsinoImpedida.Valida(unidade, false);

            if (validacao.Valido)
            {
                rnUnidadeEnsinoImpedida.Atualiza(unidade);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdUnidadeEnsinoImpedida.DataBind();
        }

        protected void grdUnidadeEnsinoImpedida_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.UnidadeEnsinoImpedida rnUnidadeEnsinoImpedida = new Techne.Lyceum.RN.PrestacaoContas.UnidadeEnsinoImpedida();
            int unidadeImpedidaId = 0;

            unidadeImpedidaId = Convert.ToInt32(e.Keys["UNIDADEENSINOIMPEDIDAID"]);

            validacao = rnUnidadeEnsinoImpedida.ValidaRemocao(unidadeImpedidaId);

            if (validacao.Valido)
            {
                rnUnidadeEnsinoImpedida.Remove(unidadeImpedidaId);
                grdUnidadeEnsinoImpedida.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
        
        public void Update(object REGIONAL,object CENSO,object ESCOLA,object MOTIVO,object DATAFIM, object UNIDADEENSINOIMPEDIDAID) { }

        public void Delete(object UNIDADEENSINOIMPEDIDAID) { }
    }
}
