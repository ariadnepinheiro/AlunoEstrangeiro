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
using DevExpress.Web.ASPxTabControl;
using Techne.Controls;

namespace Techne.Lyceum.Net.Transporte
{
    [NavUrl("~/Transporte/Bloqueio.aspx")]
    [ControlText("Bloqueio")]
    [Title("Bloqueio")]

    public partial class Bloqueio : TPage
    {
        public enum TipoOperacao
        {
            Novo,
            Cancelar,
            Consultar,
            Inicial,
            Sucesso
        }

        private TipoOperacao _tipoOperacao
        {
            get
            {
                if (ViewState["_tipoOperacao"] != null)
                {
                    if (ViewState["_tipoOperacao"] is TipoOperacao)
                    {
                        return (TipoOperacao)ViewState["_tipoOperacao"];
                    }
                }

                return TipoOperacao.Inicial;
            }

            set
            {
                ViewState["_tipoOperacao"] = value;
            }
        }

        private void ControlarTipoOperacao()
        {
            switch (this._tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { };
                        ControlarVisibilidadeControle(controles);
                        ddlTipoBloqueio.SelectedIndex = 0;
                        ddlTipoBloqueio.SelectedValue = string.Empty;                  
                        pnlPrestador.Visible = false;
                        pnlVeiculo.Visible = false;
                        pnlCondutor.Visible = false;
                        pnlGrid.Visible = false;
                        this.ddlMotivoBloqueio.Items.Clear();
                        pnlDadosBloqueio.Visible = false;
                        pnlCondutor.Visible = false;
                        pnlVeiculo.Visible = false;
                        pnlPrestador.Visible = false;
                        pnlGrid.Visible = false;
                        tseCondutor.ResetValue();
                        tseVeiculo.ResetValue();
                        break;

                    }
                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);

                        grdCondutor.DataBind();
                        grdVeiculo.DataBind();
                        grdPrestador.DataBind();
                        pnlDadosBloqueio.Visible = false;
                        pnlCondutor.Visible = false;
                        pnlVeiculo.Visible = false;
                        pnlPrestador.Visible = false;
                        ddlMotivoBloqueio.ClearSelection();
                        txtObservacao.Text = string.Empty;
                        dtInicio.Text = string.Empty;
                        tseCondutor.ResetValue();
                        tseVeiculo.ResetValue();
                        tsePrestador.ResetValue();
                        VisibilidadeTipoBloqueio();
                        break;
                    }
                case TipoOperacao.Novo:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnIncluir };
                        ControlarVisibilidadeControle(controles);

                        pnlDadosBloqueio.Visible = true;
                        if (!string.IsNullOrEmpty(ddlTipoBloqueio.SelectedValue))
                        {
                            if (ddlTipoBloqueio.SelectedValue.Equals(Convert.ToInt32(RN.Transporte.MotivoBloqueio.TipoMotivoBloqueio.Condutor).ToString()))
                            {
                                pnlCondutor.Visible = true;
                            }
                            if (ddlTipoBloqueio.SelectedValue.Equals(Convert.ToInt32(RN.Transporte.MotivoBloqueio.TipoMotivoBloqueio.Veiculo).ToString()))
                            {
                                pnlVeiculo.Visible = true;
                            }

                            if (ddlTipoBloqueio.SelectedValue.Equals(Convert.ToInt32(RN.Transporte.MotivoBloqueio.TipoMotivoBloqueio.Prestador).ToString()))
                            {
                                pnlPrestador.Visible = true;
                            }
                        }

                        ddlMotivoBloqueio.ClearSelection();
                        txtObservacao.Text = string.Empty;
                        dtInicio.Text = string.Empty;
                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);
                        ddlMotivoBloqueio.ClearSelection();
                        txtObservacao.Text = string.Empty;
                        dtInicio.Text = string.Empty;
                        pnlDadosBloqueio.Visible = false;
                        pnlCondutor.Visible = false;
                        pnlVeiculo.Visible = false;
                        pnlPrestador.Visible = false;
                        pnlGrid.Visible = true;
                        VisibilidadeTipoBloqueio();
                        break;
                    }
                case TipoOperacao.Cancelar:
                    {
                        ImageButton[] controles;

                        if (!string.IsNullOrEmpty(ddlTipoBloqueio.SelectedValue))
                        {
                            controles = new ImageButton[] { btnNovo };
                        }
                        else
                        { 
                            controles = new ImageButton[] { };
                        }



                        ControlarVisibilidadeControle(controles);
                        pnlDadosBloqueio.Visible = false;
                        pnlCondutor.Visible = false;
                        pnlVeiculo.Visible = false;
                        pnlPrestador.Visible = false;
                        break;
                    }
            }
        }

        public object ListarCondutor(object tipo)
        {           
            RN.Transporte.CondutorBloqueio rnCondutorBloqueio = new Techne.Lyceum.RN.Transporte.CondutorBloqueio();
           
            if (!string.IsNullOrEmpty(tipo.ToString()))
            {
                if (tipo.Equals(Convert.ToInt32(RN.Transporte.MotivoBloqueio.TipoMotivoBloqueio.Condutor).ToString()))
                {
                    return rnCondutorBloqueio.Lista();
                }      
            }
            return null;
        }

        public object ListarVeiculo(object tipo)
        {
            RN.Transporte.VeiculoBloqueio rnVeiculoBloqueio = new Techne.Lyceum.RN.Transporte.VeiculoBloqueio();
            
            if (!string.IsNullOrEmpty(tipo.ToString()))
            {
                if (tipo.Equals(Convert.ToInt32(RN.Transporte.MotivoBloqueio.TipoMotivoBloqueio.Veiculo).ToString()))
                {
                    return rnVeiculoBloqueio.Lista();
                }
            }

            return null;
        }

        public object ListarPrestador(object tipo)
        {
            RN.Transporte.VeiculoBloqueio rnVeiculoBloqueio = new Techne.Lyceum.RN.Transporte.VeiculoBloqueio();
            RN.Transporte.PrestadorBloqueio rnPrestadorBloqueio = new Techne.Lyceum.RN.Transporte.PrestadorBloqueio();


            if (!string.IsNullOrEmpty(tipo.ToString()))
            {
                if (tipo.Equals(Convert.ToInt32(RN.Transporte.MotivoBloqueio.TipoMotivoBloqueio.Prestador).ToString()))
                {
                    return rnPrestadorBloqueio.Lista();
                }
            }

            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    _tipoOperacao = TipoOperacao.Inicial;
                    ControlarTipoOperacao();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdPrestador, "Prestador");
            TituloGrid(grdCondutor, "Condutor");
            TituloGrid(grdVeiculo, "Veículo");

        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnIncluir, AcaoControle.novo);
            ControlaAcesso(grdPrestador);
            ControlaAcesso(grdCondutor);
            ControlaAcesso(grdVeiculo);
        }

        protected void ddlTipoBloqueio_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RN.Transporte.MotivoBloqueio rnMotivoBloqueio = new Techne.Lyceum.RN.Transporte.MotivoBloqueio();               
                _tipoOperacao = TipoOperacao.Inicial;   

                if (!string.IsNullOrEmpty(ddlTipoBloqueio.SelectedValue))
                {
                    _tipoOperacao = TipoOperacao.Consultar;

                    ddlMotivoBloqueio.ClearSelection();
                    ddlMotivoBloqueio.Items.Clear();
                    ddlMotivoBloqueio.DataSource = rnMotivoBloqueio.ListaAtivoPor(Convert.ToInt32(ddlTipoBloqueio.SelectedValue));                    
                    ddlMotivoBloqueio.Items.Insert(0, new ListItem("Selecione", string.Empty));
                    ddlMotivoBloqueio.DataBind();
                } 
                
                ControlarTipoOperacao();
                VisibilidadeTipoBloqueio();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }   

        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Novo;
                ControlarTipoOperacao();
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
                _tipoOperacao = TipoOperacao.Cancelar;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (var botao in botoes)
            {
                botao.Visible = true;
            }
        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnNovo.Visible = false;
            btnIncluir.Visible = false;
        }

        protected void btnIncluir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Transporte.VeiculoBloqueio rnVeiculoBloqueio = new Techne.Lyceum.RN.Transporte.VeiculoBloqueio();
                RN.Transporte.CondutorBloqueio rnCondutorBloqueio = new Techne.Lyceum.RN.Transporte.CondutorBloqueio();
                RN.Transporte.PrestadorBloqueio rnPrestadorBloqueio= new Techne.Lyceum.RN.Transporte.PrestadorBloqueio();
                RN.Transporte.Entidades.CondutorBloqueio condutorBloqueio = new Techne.Lyceum.RN.Transporte.Entidades.CondutorBloqueio();
                RN.Transporte.Entidades.VeiculoBloqueio veiculoBloqueio = new Techne.Lyceum.RN.Transporte.Entidades.VeiculoBloqueio();
                RN.Transporte.Entidades.PrestadorBloqueio prestadorBloqueio = new Techne.Lyceum.RN.Transporte.Entidades.PrestadorBloqueio();
                string mensagem = string.Empty;

                if (!string.IsNullOrEmpty(ddlTipoBloqueio.SelectedValue))
                {
                    if (ddlTipoBloqueio.SelectedValue.Equals(Convert.ToInt32(RN.Transporte.MotivoBloqueio.TipoMotivoBloqueio.Condutor).ToString()))
                    {
                        condutorBloqueio.CondutorBloqueioId = 0;
                        condutorBloqueio.CondutorId = (this.tseCondutor.IsValidDBValue && !this.tseCondutor.DBValue.IsNull) ? Convert.ToInt32(tseCondutor["condutorid"]) : -1;
                        condutorBloqueio.MotivoBloqueioId = !string.IsNullOrEmpty(ddlMotivoBloqueio.SelectedValue) ? Convert.ToInt32(ddlMotivoBloqueio.SelectedValue) : -1;
                        condutorBloqueio.Observacao = !string.IsNullOrEmpty(txtObservacao.Text.Trim()) ? txtObservacao.Text.Trim() : null;
                        condutorBloqueio.DataBloqueio = !string.IsNullOrEmpty(dtInicio.Text) ? dtInicio.Date : DateTime.MinValue;
                        condutorBloqueio.UsuarioBloqueioId = User.Identity.Name;

                        validacao = rnCondutorBloqueio.ValidaBloqueio(condutorBloqueio, true);

                    }

                    if (ddlTipoBloqueio.SelectedValue.Equals(Convert.ToInt32(RN.Transporte.MotivoBloqueio.TipoMotivoBloqueio.Veiculo).ToString()))
                    {
                        veiculoBloqueio.VeiculoBloqueioId = 0;
                        veiculoBloqueio.VeiculoId = (this.tseVeiculo.IsValidDBValue && !this.tseVeiculo.DBValue.IsNull) ? Convert.ToInt32(tseVeiculo["veiculoid"]) : -1;
                        veiculoBloqueio.MotivoBloqueioId = !string.IsNullOrEmpty(ddlMotivoBloqueio.SelectedValue) ? Convert.ToInt32(ddlMotivoBloqueio.SelectedValue) : -1;
                        veiculoBloqueio.Observacao = !string.IsNullOrEmpty(txtObservacao.Text.Trim()) ? txtObservacao.Text.Trim() : null;
                        veiculoBloqueio.DataBloqueio = !string.IsNullOrEmpty(dtInicio.Text) ? dtInicio.Date : DateTime.MinValue;
                        veiculoBloqueio.UsuarioBloqueioId = User.Identity.Name;

                        validacao = rnVeiculoBloqueio.ValidaBloqueio(veiculoBloqueio, true);

                    }

                    if (ddlTipoBloqueio.SelectedValue.Equals(Convert.ToInt32(RN.Transporte.MotivoBloqueio.TipoMotivoBloqueio.Prestador).ToString()))
                    {
                        prestadorBloqueio.PrestadorBloqueioId = 0;
                        prestadorBloqueio.PrestadorId = (this.tsePrestador.IsValidDBValue && !this.tsePrestador.DBValue.IsNull) ? Convert.ToInt32(tsePrestador["prestadorid"]) : -1;
                        prestadorBloqueio.MotivoBloqueioId = !string.IsNullOrEmpty(ddlMotivoBloqueio.SelectedValue) ? Convert.ToInt32(ddlMotivoBloqueio.SelectedValue) : -1;
                        prestadorBloqueio.Observacao = !string.IsNullOrEmpty(txtObservacao.Text.Trim()) ? txtObservacao.Text.Trim() : null;
                        prestadorBloqueio.DataBloqueio = !string.IsNullOrEmpty(dtInicio.Text) ? dtInicio.Date : DateTime.MinValue;
                        prestadorBloqueio.UsuarioBloqueioId = User.Identity.Name;

                        validacao = rnPrestadorBloqueio.ValidaBloqueio(prestadorBloqueio, true);

                    }

                    if (validacao.Valido)
                    {
                        if (ddlTipoBloqueio.SelectedValue.Equals(Convert.ToInt32(RN.Transporte.MotivoBloqueio.TipoMotivoBloqueio.Condutor).ToString()))
                        {
                            rnCondutorBloqueio.Bloqueia(condutorBloqueio);
                        }
                        else if (ddlTipoBloqueio.SelectedValue.Equals(Convert.ToInt32(RN.Transporte.MotivoBloqueio.TipoMotivoBloqueio.Veiculo).ToString()))
                        {
                            rnVeiculoBloqueio.Bloqueia(veiculoBloqueio);
                        }
                        else if (ddlTipoBloqueio.SelectedValue.Equals(Convert.ToInt32(RN.Transporte.MotivoBloqueio.TipoMotivoBloqueio.Prestador).ToString()))
                        {
                            rnPrestadorBloqueio.Bloqueia(prestadorBloqueio);
                        }

                        mensagem = ddlTipoBloqueio.SelectedItem.Text + " bloqueado com sucesso.";

                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup",
                          "alert('" + mensagem + @"');", true);

                        lblMensagem.Text = string.Empty;

                        _tipoOperacao = TipoOperacao.Sucesso;
                        ControlarTipoOperacao();
                    }
                    else
                    {
                        this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdCondutor_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdCondutor.Settings.ShowFilterRow = false;
        }

        protected void grdCondutor_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdCondutor.Settings.ShowFilterRow = false;
        }

        protected void grdCondutor_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdCondutor.IsEditing)
            {

                if ((e.Column.FieldName) == "CPF")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.ClientEnabled = false;
                }
                else if ((e.Column.FieldName) == "NOME")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.ClientEnabled = false;
                }
                else if ((e.Column.FieldName) == "MOTIVOBLOQUEIO")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.ClientEnabled = false;
                }              
                else if ((e.Column.FieldName) == "DATABLOQUEIO")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.ClientEnabled = false;
                }

            }
        }

        protected void grdCondutor_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "CPF" && e.Value != null)
            {
                e.DisplayText = string.Format(@"{0:000\.000\.000\-00}", e.Value);
            }
        }

        protected void grdCondutor_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Entidades.CondutorBloqueio condutorBloqueio = new Techne.Lyceum.RN.Transporte.Entidades.CondutorBloqueio();
            RN.Transporte.CondutorBloqueio rnCondutorBloqueio = new Techne.Lyceum.RN.Transporte.CondutorBloqueio();

            condutorBloqueio.Observacao = e.NewValues["OBSERVACAO"] != null ? e.NewValues["OBSERVACAO"].ToString() : null;
            condutorBloqueio.DataDesbloqueio = e.NewValues["DATADESBLOQUEIO"] != null ? Convert.ToDateTime(e.NewValues["DATADESBLOQUEIO"]) : DateTime.MinValue;
            condutorBloqueio.DataBloqueio = e.NewValues["DATABLOQUEIO"] != null  ? Convert.ToDateTime(e.NewValues["DATABLOQUEIO"]) : DateTime.MinValue;
            condutorBloqueio.UsuarioDesbloqueioId = User.Identity.Name;
            condutorBloqueio.CondutorBloqueioId = Convert.ToInt32(e.Keys["CONDUTORBLOQUEIOID"]);

            validacao = rnCondutorBloqueio.ValidaDesbloqueio(condutorBloqueio.CondutorBloqueioId, condutorBloqueio.DataDesbloqueio.Value, condutorBloqueio.UsuarioDesbloqueioId,condutorBloqueio.DataBloqueio);

            if (validacao.Valido)
            {
                rnCondutorBloqueio.Desbloqueia(condutorBloqueio.CondutorBloqueioId, condutorBloqueio.DataDesbloqueio.Value, condutorBloqueio.UsuarioDesbloqueioId,condutorBloqueio.Observacao);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdCondutor.DataBind();
        }

        protected void grdCondutor_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Entidades.CondutorBloqueio condutorBloqueio = new Techne.Lyceum.RN.Transporte.Entidades.CondutorBloqueio();
            RN.Transporte.CondutorBloqueio rnCondutorBloqueio = new Techne.Lyceum.RN.Transporte.CondutorBloqueio();

            int condutorBloqueioId = 0;

            condutorBloqueioId = Convert.ToInt32(e.Keys["CONDUTORBLOQUEIOID"]);

            validacao = rnCondutorBloqueio.ValidaRemocao(condutorBloqueioId);

            if (validacao.Valido)
            {
                rnCondutorBloqueio.Remove(condutorBloqueioId);
                grdCondutor.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

        protected void grdCondutor_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            var dataDesbloqueio = grdCondutor.GetRowValues(e.VisibleIndex, "DATADESBLOQUEIO").ToString(); 

            if (!string.IsNullOrEmpty(dataDesbloqueio))
            {
                if (Convert.ToDateTime(dataDesbloqueio) < DateTime.Now.Date)
                {
                    if (e.ButtonType == ColumnCommandButtonType.Edit)
                    {
                        e.Visible = false;
                    }
                }
            }
        }

        public void Update(object CPF, object NOME, object MOTIVOBLOQUEIO, object OBSERVACAO, object DATABLOQUEIO, object DATADESBLOQUEIO, object CONDUTORBLOQUEIOID)
        {}

        public void Delete(object CONDUTORBLOQUEIOID)
        { }

        protected void grdVeiculo_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdVeiculo.Settings.ShowFilterRow = false;
        }

        protected void grdVeiculo_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdVeiculo.Settings.ShowFilterRow = false;
        }

        protected void grdVeiculo_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdVeiculo.IsEditing)
            {

                if ((e.Column.FieldName) == "PLACA")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.ClientEnabled = false;
                }
                else if ((e.Column.FieldName) == "NOME")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.ClientEnabled = false;
                }
                else if ((e.Column.FieldName) == "TIPO")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.ClientEnabled = false;
                }
                else if ((e.Column.FieldName) == "MOTIVOBLOQUEIO")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.ClientEnabled = false;
                }
                else if ((e.Column.FieldName) == "DATABLOQUEIO")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.ClientEnabled = false;
                }

            }
        }

        protected void grdVeiculo_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            var dataDesbloqueio = grdVeiculo.GetRowValues(e.VisibleIndex, "DATADESBLOQUEIO").ToString();

            if (!string.IsNullOrEmpty(dataDesbloqueio))
            {
                if (Convert.ToDateTime(dataDesbloqueio) < DateTime.Now.Date)
                {
                    if (e.ButtonType == ColumnCommandButtonType.Edit)
                    {
                        e.Visible = false;
                    }
                }
            }
        }

        protected void grdVeiculo_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Entidades.VeiculoBloqueio veiculoBloqueio = new Techne.Lyceum.RN.Transporte.Entidades.VeiculoBloqueio();
            RN.Transporte.VeiculoBloqueio rnVeiculoBloqueio = new Techne.Lyceum.RN.Transporte.VeiculoBloqueio();

            veiculoBloqueio.Observacao = e.NewValues["OBSERVACAO"] != null ? e.NewValues["OBSERVACAO"].ToString() : null;
            veiculoBloqueio.DataDesbloqueio = e.NewValues["DATADESBLOQUEIO"] != null ? Convert.ToDateTime(e.NewValues["DATADESBLOQUEIO"]) : DateTime.MinValue;
            veiculoBloqueio.DataBloqueio = e.NewValues["DATABLOQUEIO"] != null ? Convert.ToDateTime(e.NewValues["DATABLOQUEIO"]) : DateTime.MinValue;
            veiculoBloqueio.UsuarioDesbloqueioId = User.Identity.Name;
            veiculoBloqueio.VeiculoBloqueioId = Convert.ToInt32(e.Keys["VEICULOBLOQUEIOID"]);

            validacao = rnVeiculoBloqueio.ValidaDesbloqueio(veiculoBloqueio.VeiculoBloqueioId, veiculoBloqueio.DataDesbloqueio.Value, veiculoBloqueio.UsuarioDesbloqueioId, veiculoBloqueio.DataBloqueio);

            if (validacao.Valido)
            {
                rnVeiculoBloqueio.Desbloqueia(veiculoBloqueio.VeiculoBloqueioId, veiculoBloqueio.DataDesbloqueio.Value, veiculoBloqueio.UsuarioDesbloqueioId, veiculoBloqueio.Observacao);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdVeiculo.DataBind();
        }

        protected void grdVeiculo_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Entidades.VeiculoBloqueio veiculoBloqueio = new Techne.Lyceum.RN.Transporte.Entidades.VeiculoBloqueio();
            RN.Transporte.VeiculoBloqueio rnVeiculoBloqueio = new Techne.Lyceum.RN.Transporte.VeiculoBloqueio();

            int veiculoBloqueioId = 0;

            veiculoBloqueioId = Convert.ToInt32(e.Keys["VEICULOBLOQUEIOID"]);

            validacao = rnVeiculoBloqueio.ValidaRemocao(veiculoBloqueioId);

            if (validacao.Valido)
            {
                rnVeiculoBloqueio.Remove(veiculoBloqueioId);
                grdVeiculo.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

        public void UpdateVeiculo(object NOME, object PLACA, object TIPO, object MOTIVOBLOQUEIO, object OBSERVACAO, object DATABLOQUEIO, object DATADESBLOQUEIO, object VEICULOBLOQUEIOID)
        { }

        public void DeleteVeiculo(object VEICULOBLOQUEIOID)
        { }

        protected void grdPrestador_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdPrestador.Settings.ShowFilterRow = false;
        }

        protected void grdPrestador_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdPrestador.Settings.ShowFilterRow = false;
        }

        protected void grdPrestador_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdPrestador.IsEditing)
            {

                if ((e.Column.FieldName) == "CNPJCPF")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.ClientEnabled = false;
                }
                else if ((e.Column.FieldName) == "NOME")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.ClientEnabled = false;
                }
                else if ((e.Column.FieldName) == "MOTIVOBLOQUEIO")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.ClientEnabled = false;
                }
                else if ((e.Column.FieldName) == "DATABLOQUEIO")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.ClientEnabled = false;
                }

            }
        }

        protected void grdPrestador_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "CNPJCPF" && e.Value != null)
            {
                if (e.Value.ToString().Length == 11)
                {
                    e.DisplayText = string.Format(@"{0:000\.000\.000\-00}", e.Value);
                }
                else
                {
                    e.DisplayText = string.Format(@"{0:00\.000\.000\.0000\-00}", e.Value);
                }
            }
        }

        protected void grdPrestador_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            var dataDesbloqueio = grdPrestador.GetRowValues(e.VisibleIndex, "DATADESBLOQUEIO").ToString();

            if (!string.IsNullOrEmpty(dataDesbloqueio))
            {
                if (Convert.ToDateTime(dataDesbloqueio) < DateTime.Now.Date)
                {
                    if (e.ButtonType == ColumnCommandButtonType.Edit)
                    {
                        e.Visible = false;
                    }
                }
            }
        }

        protected void grdPrestador_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Entidades.PrestadorBloqueio prestadorBloqueio = new Techne.Lyceum.RN.Transporte.Entidades.PrestadorBloqueio();
            RN.Transporte.PrestadorBloqueio rnPrestadorBloqueio = new Techne.Lyceum.RN.Transporte.PrestadorBloqueio();

            prestadorBloqueio.Observacao = e.NewValues["OBSERVACAO"] != null ? e.NewValues["OBSERVACAO"].ToString() : null;
            prestadorBloqueio.DataDesbloqueio = e.NewValues["DATADESBLOQUEIO"] != null ? Convert.ToDateTime(e.NewValues["DATADESBLOQUEIO"]) : DateTime.MinValue;
            prestadorBloqueio.DataBloqueio = e.NewValues["DATABLOQUEIO"] != null ? Convert.ToDateTime(e.NewValues["DATABLOQUEIO"]) : DateTime.MinValue;
            prestadorBloqueio.UsuarioDesbloqueioId = User.Identity.Name;
            prestadorBloqueio.PrestadorBloqueioId = Convert.ToInt32(e.Keys["PRESTADORBLOQUEIOID"]);

            validacao = rnPrestadorBloqueio.ValidaDesbloqueio(prestadorBloqueio.PrestadorBloqueioId, prestadorBloqueio.DataDesbloqueio.Value, prestadorBloqueio.UsuarioDesbloqueioId, prestadorBloqueio.DataBloqueio);

            if (validacao.Valido)
            {
                rnPrestadorBloqueio.Desbloqueia(prestadorBloqueio.PrestadorBloqueioId, prestadorBloqueio.DataDesbloqueio.Value, prestadorBloqueio.UsuarioDesbloqueioId, prestadorBloqueio.Observacao);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdPrestador.DataBind();
        }

        protected void grdPrestador_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Entidades.PrestadorBloqueio prestadorBloqueio = new Techne.Lyceum.RN.Transporte.Entidades.PrestadorBloqueio();
            RN.Transporte.PrestadorBloqueio rnPrestadorBloqueio = new Techne.Lyceum.RN.Transporte.PrestadorBloqueio();

            int prestadorBloqueioId = 0;

            prestadorBloqueioId = Convert.ToInt32(e.Keys["PRESTADORBLOQUEIOID"]);

            validacao = rnPrestadorBloqueio.ValidaRemocao(prestadorBloqueioId);

            if (validacao.Valido)
            {
                rnPrestadorBloqueio.Remove(prestadorBloqueioId);
                grdPrestador.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

        public void UpdatePrestador(object CNPJCPF, object NOME, object MOTIVOBLOQUEIO, object OBSERVACAO, object DATABLOQUEIO, object DATADESBLOQUEIO, object PRESTADORBLOQUEIOID)
        { }

        public void DeletePrestador(object PRESTADORBLOQUEIOID)
        { }

        private void VisibilidadeTipoBloqueio()
        {
            pnlTipoBloqueio.Visible = false;
            pnlCondutor.Visible = false;
            pnlVeiculo.Visible = false;
            pnlPrestador.Visible = false;
            pnlGrdCondutor.Visible = false;
            pnlGrdVeiculo.Visible = false;
            pnlGrdPrestador.Visible = false;
            pnlGrid.Visible = false;

            if (!string.IsNullOrEmpty(ddlTipoBloqueio.SelectedValue))
            {
                if (ddlTipoBloqueio.SelectedValue.Equals(Convert.ToInt32(RN.Transporte.MotivoBloqueio.TipoMotivoBloqueio.Condutor).ToString()))
                {
                    pnlGrdCondutor.Visible = true;
                }
                if (ddlTipoBloqueio.SelectedValue.Equals(Convert.ToInt32(RN.Transporte.MotivoBloqueio.TipoMotivoBloqueio.Veiculo).ToString()))
                {
                    pnlGrdVeiculo.Visible = true;
                }

                if (ddlTipoBloqueio.SelectedValue.Equals(Convert.ToInt32(RN.Transporte.MotivoBloqueio.TipoMotivoBloqueio.Prestador).ToString()))
                {
                    pnlGrdPrestador.Visible = true;
                }

                pnlTipoBloqueio.Visible = true;
                pnlGrid.Visible = true;
            }
        }

        protected void tsePrestador_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (!this.tsePrestador.DBValue.IsNull)
                {
                    if (!this.tsePrestador.IsValidDBValue)
                    {                      
                        this.lblMensagem.Text = "Prestador não cadastrado.";
                    }                   
                }
                else
                {  
                    this.lblMensagem.Text = "Favor consultar um prestador.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseCondutor_Changed(object sender, ChangedEventArgs args)
        {
            try
            {          

                if (!this.tseCondutor.DBValue.IsNull)
                {
                    if (!this.tseCondutor.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Condutor não cadastrado.";
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar um condutor.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseVeiculo_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (!this.tseVeiculo.DBValue.IsNull)
                {
                    if (!this.tseVeiculo.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Veículo não cadastrado.";
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar um veículo.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

    }
}
