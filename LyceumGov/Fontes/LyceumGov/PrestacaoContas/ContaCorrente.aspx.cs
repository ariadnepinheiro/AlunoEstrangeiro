using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Controls;
using DevExpress.Web.ASPxGridView;
using Techne.Lyceum.RN.Util;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Web;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [NavUrl("~/PrestacaoContas/ContaCorrente.aspx"), ControlText("Conta Corrente"), Title("Conta Corrente")]
    public partial class ContaCorrente : TPage
    {
        public enum TipoOperacao
        {
            Novo,
            Alterar,
            Consultar,
            Inicial,
            Sucesso,
            Excluir,
            Desativar
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

        public void Delete(object CONTACORRENTEID) { }

        public object Lista(object regionalId, object censo, object filtro)
        {
            RN.PrestacaoContas.ContaCorrente rnContaCorrente = new Techne.Lyceum.RN.PrestacaoContas.ContaCorrente();

            if (filtro != null && (regionalId != null || censo != null))
            {

                if (filtro == "R" && !string.IsNullOrEmpty(regionalId.ToString()))
                {
                    return rnContaCorrente.ListaPorRegional(Convert.ToInt32(regionalId.ToString()));
                }

                if (filtro == "U" && !string.IsNullOrEmpty(censo.ToString()))
                {
                    return rnContaCorrente.ListaPorUnidadeEnsino(censo.ToString());
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
            TituloGrid(grdContaCorrente, "");
            grdContaCorrente.SettingsText.CommandEdit = "Confirma a desativação/reativação da conta?";
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            //ControlaAcesso(grdContaCorrente);
        }

        private void ControlarVisibilidadeControle(ImageButton[] imgBotoes)
        {
            RetiraVisibilidadeBotao();

            if (imgBotoes != null)
            {
                foreach (ImageButton botao in imgBotoes)
                {
                    botao.Visible = true;
                }
            }
            ControlaAcesso(btnSalvar, AcaoControle.editar);
            ControlaAcesso(btnNovo, AcaoControle.novo);

        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnNovo.Visible = false;
            btnSalvar.Visible = false;

        }
        protected void rblTipoFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tseUnidadeResponsavel.ResetValue();
                tseRegional.ResetValue();
                //grdContaCorrente.Visible = false;
                pnlFiltro.Visible = true;

                if (rblTipoFiltro.SelectedValue == "R")
                {
                    pnlUnidade.Visible = false;
                }
                else
                {
                    pnlUnidade.Visible = true;
                }
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
                _tipoOperacao = TipoOperacao.Consultar;
                ControlarTipoOperacao();
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
                if (!rblTipoFiltro.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    if (rblTipoFiltro.SelectedValue == "R")
                    {
                        if (!this.tseRegional.DBValue.IsNull && this.tseRegional.IsValidDBValue)
                        {
                            hdnContaCorrente.Value = string.Empty;
                            _tipoOperacao = TipoOperacao.Novo;
                            ControlarTipoOperacao();
                        }
                        else
                        {
                            lblMensagem.Text = "Para uma Nova Conta Corrente é necessário escolher uma Regional.";
                            return;
                        }
                    }
                    else
                    {
                        if (!this.tseUnidadeResponsavel.DBValue.IsNull && this.tseUnidadeResponsavel.IsValidDBValue)
                        {
                            _tipoOperacao = TipoOperacao.Novo;
                            ControlarTipoOperacao();
                        }
                        else
                        {
                            lblMensagem.Text = "Para uma Nova Conta Corrente é necessário escolher a Unidade de Ensino.";
                            return;
                        }
                    }
                }
                else
                {
                    lblMensagem.Text = "Para uma Nova Conta Corrente é necessário a escolha de um filtro.";
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ControlarTipoOperacao()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);
                        LimpaDados();
                        tseUnidadeResponsavel.ResetValue();
                        tseRegional.ResetValue();
                        pnlContaCorrente.Visible = false;
                        grdContaCorrente.Visible = false;
                        ControlarTSearchs();
                        break;
                    }
                case TipoOperacao.Novo:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);
                        LimpaDados();
                        tseRegional.Enabled = false;
                        tseUnidadeResponsavel.Enabled = false;
                        pnlContaCorrente.Visible = true;
                        ControlarTSearchs();
                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { };
                        controles = new[] { btnNovo };
                        pnlContaCorrente.Visible = false;
                        ControlarVisibilidadeControle(controles);
                        ControlarTSearchs();
                        break;
                    }

                case TipoOperacao.Alterar:
                    {
                        pnlContaCorrente.Visible = true;
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);
                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        if (rblTipoFiltro.SelectedValue == "R" || (rblTipoFiltro.SelectedValue == "U" && (tseUnidadeResponsavel.IsValidDBValue && !this.tseUnidadeResponsavel.DBValue.IsNull)))
                        {
                            grdContaCorrente.Visible = true;
                        }

                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);
                        pnlContaCorrente.Visible = false;
                        LimpaDados();
                        ControlarTSearchs();
                        break;
                    }

            }
        }

        protected void tseBanco_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseAgencia_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseAgencia_Changed(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                if (!this.tseAgencia.DBValue.IsNull)
                {
                    if (!this.tseAgencia.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Agência não cadastrada.";
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Agência não cadastrada.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseBanco_Changed(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                if (!this.tseBanco.DBValue.IsNull)
                {
                    if (this.tseBanco.IsValidDBValue)
                    {
                        tseAgencia.Mode = ControlMode.Edit;
                        tseAgencia.SqlWhere = "banco= '" + tseBanco.DBValue.ToString() + "'";
                        tseAgencia.DataBind();
                    }
                    else
                    {
                        this.lblMensagem.Text = "Banco não cadastrado.";
                    }
                }
                else
                {
                    tseBanco.ResetValue();
                    tseAgencia.Mode = ControlMode.Edit;
                    tseAgencia.ResetValue();
                    tseAgencia.SqlWhere = string.Empty;
                    tseAgencia.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeResponsavel_Changed(object sender, ChangedEventArgs args)
        {
            if (Page.IsCallback)
            {
                return;
            }
            try
            {
                grdContaCorrente.Visible = false;

                if (!this.tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (!this.tseUnidadeResponsavel.IsValidDBValue)
                    {
                        this._tipoOperacao = TipoOperacao.Inicial;
                        this.lblMensagem.Text = "Unidade Escolar não cadastrada.";
                    }
                    else
                    {
                        var sessao = RN.SessaoUsuario.GetSessaoUsuario();

                        if (!this.tseUnidadeResponsavel["id_regional"].IsNull)
                        {
                            this.tseRegional.Value = this.tseUnidadeResponsavel["id_regional"];
                        }

                        if (sessao != null)
                        {
                            sessao.Escola = Convert.ToString(this.tseUnidadeResponsavel.DBValue);
                            sessao.Regional = Convert.ToString(this.tseRegional.DBValue);
                        }

                        this._tipoOperacao = TipoOperacao.Consultar;

                    }
                }
                else
                {
                    this._tipoOperacao = TipoOperacao.Inicial;
                    this.lblMensagem.Text = "Favor consultar uma Unidade Escolar.";
                }

                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseRegional_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }
                //grdContaCorrente.Visible = false;
                lblMensagem.Text = string.Empty;

                if (!tseRegional.DBValue.IsNull)
                {
                    if (!tseRegional.IsValidDBValue)
                    {
                        this._tipoOperacao = TipoOperacao.Inicial;
                        lblMensagem.Text = "Regional não cadastrada (favor verificar).";
                    }
                    else
                    {
                        this._tipoOperacao = TipoOperacao.Consultar;
                    }
                }
                else
                {
                    this._tipoOperacao = TipoOperacao.Inicial;
                    lblMensagem.Text = "Regional não cadastrada (favor verificar).";
                }

                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimpaDados()
        {
            hdnContaCorrente.Value = string.Empty;
            txtConta.Text = string.Empty;
            tseBanco.ResetValue();
            tseAgencia.ResetValue();
            txtConta.Text = string.Empty;
            dtDataInicio.Text = string.Empty;
            dtDataFim.Text = string.Empty;
        }

        private void ControlarTSearchs()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        tseUnidadeResponsavel.Enabled = true;
                        tseUnidadeResponsavel.Mode = ControlMode.Edit;
                        tseRegional.Enabled = true;
                        tseRegional.Mode = ControlMode.Edit;
                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        tseUnidadeResponsavel.Enabled = true;
                        tseUnidadeResponsavel.Mode = ControlMode.Edit;
                        tseRegional.Enabled = true;
                        tseRegional.Mode = ControlMode.Edit;
                        tseBanco.Mode = ControlMode.Edit;
                        tseAgencia.Mode = ControlMode.Edit;
                        break;
                    }

                case TipoOperacao.Novo:
                    {
                        tseUnidadeResponsavel.Enabled = false;
                        tseUnidadeResponsavel.Mode = ControlMode.Edit;
                        tseRegional.Enabled = false;
                        tseRegional.Mode = ControlMode.Edit;
                        tseBanco.Enabled = true;
                        tseAgencia.Enabled = true;
                        tseAgencia.Mode = ControlMode.Edit;
                        tseBanco.Mode = ControlMode.Edit;
                        break;
                    }

                case TipoOperacao.Alterar:
                    {
                        tseUnidadeResponsavel.Enabled = false;
                        tseUnidadeResponsavel.Mode = ControlMode.Edit;
                        tseRegional.Enabled = false;
                        tseRegional.Mode = ControlMode.Edit;
                        tseBanco.Mode = ControlMode.Edit;
                        tseAgencia.Mode = ControlMode.Edit;

                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        tseBanco.Mode = ControlMode.Edit;
                        tseAgencia.Mode = ControlMode.Edit;

                        break;
                    }
            }
        }
        protected void tseRegional_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseUnidadeResponsavel_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        private int GetSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = grdContaCorrente.PageIndex * grdContaCorrente.SettingsPager.PageSize;
            for (int i = 0; i < grdContaCorrente.VisibleRowCount; i++)
            {
                if (grdContaCorrente.Selection.IsRowSelected(startIndexOnPage + i))
                {
                    return startIndexOnPage + i;
                }
            }
            return -1;
        }
        protected void grdContaCorrente_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdContaCorrente.Settings.ShowFilterRow = false;
        }

        protected void grdContaCorrente_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdContaCorrente.Settings.ShowFilterRow = false;
        }

        protected void grdContaCorrente_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdContaCorrente);
        }


        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            RN.PrestacaoContas.ContaCorrente rnContaCorrente = new Techne.Lyceum.RN.PrestacaoContas.ContaCorrente();
            RN.PrestacaoContas.Entidades.ContaCorrente contaCorrente = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ContaCorrente();
            ValidacaoDados validacao = new ValidacaoDados();
            string mensagem = string.Empty;
            try
            {
                contaCorrente.ContaCorrenteId = !hdnContaCorrente.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnContaCorrente.Value) : -1;
                if (rblTipoFiltro.SelectedValue == "R")
                {
                    contaCorrente.RegionalId = !this.tseRegional.DBValue.IsNull && tseRegional.IsValidDBValue ? Convert.ToInt32(tseRegional.DBValue.ToString()) : (int?)null;
                    contaCorrente.Censo = null;
                }
                else
                {
                    contaCorrente.Censo = !this.tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue ? tseUnidadeResponsavel.DBValue.ToString() : null;
                    contaCorrente.RegionalId = null;
                }

                contaCorrente.Agencia = !this.tseAgencia.DBValue.IsNull && tseAgencia.IsValidDBValue ? tseAgencia.DBValue.ToString() : null;
                contaCorrente.Banco = !this.tseBanco.DBValue.IsNull && tseBanco.IsValidDBValue ? tseBanco.DBValue.ToString() : null;
                contaCorrente.Conta = !txtConta.Text.IsNullOrEmptyOrWhiteSpace() ? txtConta.Text.Trim().ToUpper() : null;
                contaCorrente.UsuarioId = User.Identity.Name;
                contaCorrente.DataInicio = !dtDataInicio.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataInicio.Date : DateTime.MinValue;
                contaCorrente.DataFim = !dtDataFim.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataFim.Date : (DateTime?)null;
                contaCorrente.DataAlteracao = DateTime.Now;
              
                validacao = rnContaCorrente.Valida(contaCorrente, (contaCorrente.ContaCorrenteId == -1 ? true : false));

                if (validacao.Valido)
                {
                    if (contaCorrente.ContaCorrenteId == -1)
                    {
                        rnContaCorrente.Insere(contaCorrente);
                        mensagem = "Conta Corrente inserida com sucesso.";
                    }
                    else
                    {
                        rnContaCorrente.Atualiza(contaCorrente);
                        mensagem = "Conta Corrente atualizado com sucesso.";
                    }

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('" + mensagem + ".');", true);
                    lblMensagem.Text = mensagem;
                    LimpaDados();
                    grdContaCorrente.DataBind();

                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdContaCorrente_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            string contaCorrenteId = grdContaCorrente.GetRowValues(e.VisibleIndex, "CONTACORRENTEID").ToString();
            string banco = grdContaCorrente.GetRowValues(e.VisibleIndex, "BANCO").ToString();
            string agencia = grdContaCorrente.GetRowValues(e.VisibleIndex, "AGENCIA").ToString();
            string conta = grdContaCorrente.GetRowValues(e.VisibleIndex, "CONTA").ToString();
            string dataInicio = grdContaCorrente.GetRowValues(e.VisibleIndex, "DATAINICIO").ToString();
            string dataFim = grdContaCorrente.GetRowValues(e.VisibleIndex, "DATAFIM").ToString();

            if (e.ButtonID == "btnEditarCustom")
            {
                _tipoOperacao = TipoOperacao.Alterar;
                ControlarTipoOperacao();
                hdnContaCorrente.Value = contaCorrenteId;
                tseBanco.DBValue = banco;
                tseAgencia.DBValue = agencia;
                txtConta.Text = conta;

                if (!dataInicio.IsNullOrEmptyOrWhiteSpace())
                {
                    dtDataInicio.Value = Convert.ToDateTime(dataInicio);
                }
                if (!dataFim.IsNullOrEmptyOrWhiteSpace())
                {
                    dtDataFim.Value = Convert.ToDateTime(dataFim);
                }

            }
            if (e.ButtonID == "btnDeletar")
            {
                hdnContaCorrente.Value = contaCorrenteId;
                popup.ShowOnPageLoad = true;
            }
            grdContaCorrente.DataBind();
        }

        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();

                string contaCorrenteId = hdnContaCorrente.Value;
                RN.PrestacaoContas.ContaCorrente rnContaCorrente = new Techne.Lyceum.RN.PrestacaoContas.ContaCorrente();

                validacao = rnContaCorrente.ValidaRemocao(Convert.ToInt32(contaCorrenteId), (!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue) ? tseUnidadeResponsavel.DBValue.ToString() : null, (!tseRegional.DBValue.IsNull && tseRegional.IsValidDBValue) ? Convert.ToInt32(tseRegional.DBValue) : (int?)null);

                if (validacao.Valido)
                {
                    rnContaCorrente.Remove(Convert.ToInt32(contaCorrenteId));
                    grdContaCorrente.DataBind();
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }


            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdContaCorrente_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;

            if (e.CellType == GridViewTableCommandCellType.Filter)
            {
                e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                return;
            }
            if (e.ButtonID == "btnDeletar")
            {
                e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;

                if (Permission.AllowDelete)
                {
                    e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.True;
                }

            }

            if (e.ButtonID == "btnEditarCustom")
            {
                e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;

                if (Permission.AllowUpdate)
                {
                    e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.True;
                }

            }
        }


    }
}
