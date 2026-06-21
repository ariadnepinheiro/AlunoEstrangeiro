using System;
using System.Web;
using DevExpress.Web.ASPxTabControl;
using Techne.Controls;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Entidades;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using System.Text;
using System.Data;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.DTOs.Agenda;
using DevExpress.Web.ASPxGridView;
using System.Collections.Generic;
using System.Linq;
using Techne.Data;
using Seeduc.Infra.Helpers;
using System.Web.UI.WebControls;
using System.Drawing;
using DevExpress.Web.ASPxEditors;

namespace Techne.Lyceum.Net.Patrimonio
{
    [NavUrl("~/Patrimonio/AcompanhamentoTransferenciaPatrimonio.aspx"), ControlText("Transferência de Bem"), Title("Transferência de Bem")]

    public partial class AcompanhamentoTransferenciaPatrimonio : TPage
    {
        public object ListaOrigem(object setor, object situacao)
        {
            RN.Setores rnSetor = new Setores();
            RN.Patrimonio.Transferencia rnTransferencia = new Techne.Lyceum.RN.Patrimonio.Transferencia();

            if (!string.IsNullOrEmpty(setor.ToString()) && !string.IsNullOrEmpty(situacao.ToString()))
            {
                string codigoSetor = rnSetor.ObtemSetorPor(setor.ToString());
                return rnTransferencia.ListaTransferenciaOrigemPor(codigoSetor, situacao.ToString());
            }
            return null;
        }

        public object ListaDestino(object setor, object situacao, object lote)
        {
            RN.Setores rnSetor = new Setores();
            RN.Patrimonio.Transferencia rnTransferencia = new Techne.Lyceum.RN.Patrimonio.Transferencia();
            int? transferenciaid = !string.IsNullOrEmpty(lote.ToString()) ? Convert.ToInt32(lote) : (int?)null;

            if (!string.IsNullOrEmpty(setor.ToString()) && !string.IsNullOrEmpty(situacao.ToString()))
            {
                string codigoSetor = rnSetor.ObtemSetorPor(setor.ToString());
                var dt = rnTransferencia.ListaTransferenciaDestinoPor(codigoSetor, situacao.ToString(), transferenciaid);
                return dt;
            }
            
            return null;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdPatrimonioSolicParaUE, string.Empty);
            TituloGrid(this.grdPatrimonioSolicPelaUE, string.Empty);
            ControlarVisibilidadeControle();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                RN.Patrimonio.Transferencia rnTransferencia = new Techne.Lyceum.RN.Patrimonio.Transferencia();
                lblMensagem.Text = string.Empty;
                lblMensagemLoteDisponível.Text = string.Empty;

                if (!ddlStatusSolicParaUE.SelectedValue.IsNullOrEmptyOrWhiteSpace() && ddlStatusSolicParaUE.SelectedValue == RN.Patrimonio.TransferenciaItem.Pendente)
                {
                    tseLote.Visible = true;
                    lblLote.Visible = true;
                    lblLoteDescricao.Visible = true;
                    lblDataTransferencia.Visible = true;
                    dtDataTransferencia.Visible = true;
                }
                else
                {
                    tseLote.ResetValue();
                    tseLote.Visible = false;
                    lblLote.Visible = false;
                    lblLoteDescricao.Visible = false;
                    lblDataTransferencia.Visible = false;
                    dtDataTransferencia.Visible = false;
                }

                if (!IsPostBack)
                {
                    dtDataTransferencia.Text = string.Empty;
                    ddlStatusSolicPelaUE.Items.Clear();
                    ddlStatusSolicPelaUE.DataSource = rnTransferencia.ListaSituacao();
                    ddlStatusSolicPelaUE.DataBind();

                    ddlStatusSolicParaUE.Items.Clear();
                    ddlStatusSolicParaUE.DataSource = rnTransferencia.ListaSituacao();
                    ddlStatusSolicParaUE.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlStatusSolicParaUE_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tseLote.ResetValue();
                tseLote.Visible = false;
                lblLote.Visible = false;
                lblLoteDescricao.Visible = false;
                lblDataTransferencia.Visible = false;
                dtDataTransferencia.Visible = false;
                dtDataTransferencia.Text = string.Empty;

                if (!ddlStatusSolicParaUE.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    if (ddlStatusSolicParaUE.SelectedValue == RN.Patrimonio.TransferenciaItem.Pendente)
                    {
                        tseLote.Visible = true;
                        lblLote.Visible = true;
                        lblLoteDescricao.Visible = true;
                        lblDataTransferencia.Visible = true;
                        dtDataTransferencia.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ControlarVisibilidadeControle()
        {
            ControlaAcesso(btnSalvar, AcaoControle.novo);
            ControlaAcesso(btnRecusarTodas, AcaoControle.novo);
            ControlaAcesso(btnAceitarTodas, AcaoControle.novo);
        }

        protected void pcTransferencia_TabClick(object source, TabControlCancelEventArgs e)
        {
            if (e.Tab.Index == 0)
            {
                this.Server.Transfer("SolicitacaoTransferenciaPatrimonio.aspx");
            }

            if (e.Tab.Index == 1)
            {
                this.Server.Transfer("AcompanhamentoTransferenciaPatrimonio.aspx");
            }

            if (e.Tab.Index == 2)
            {
                this.Server.Transfer("HistoricoTransferenciaPatrimonio.aspx");
            }
        }
        protected void tseUA_Changed(object sender, EventArgs args)
        {
            try
            {
                RN.Patrimonio.TransferenciaItem rnTransferenciaItem = new Techne.Lyceum.RN.Patrimonio.TransferenciaItem();
                int qtde = 0;
                pnlLote.Visible = false;
                ddlStatusSolicParaUE.ClearSelection();
                ddlStatusSolicPelaUE.ClearSelection();
                lblMensagemLoteDisponível.Text = string.Empty;
                dtDataTransferencia.Text = string.Empty;

                if (!this.tseUA.DBValue.IsNull)
                {
                    if (this.tseUA.IsValidDBValue)
                    {
                        pnlLote.Visible = true;

                        string setor = tseUA["setor"].ToString();
                        qtde = rnTransferenciaItem.ObtemQuantidadeSolicitacaoPendentePor(setor);
                        lblMensagemLoteDisponível.Text = "Existe(m) " + qtde.ToString() + " itens(s) pendente(s) de transfêrencia.";

                        //Carrega lotes da ua selecionada                        
                        tseLote.ResetValue();
                        tseLote.SqlWhere = " dataandamento is nulL and setordestino = '" + setor + "'";
                        tseLote.DataBind();
                    }
                    else
                    {
                        this.lblMensagem.Text = "Unidade Administrativa não cadastrada.";
                    }

                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar uma unidade administrativa.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void tseLote_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                dtDataTransferencia.Text = string.Empty;

                if (!this.tseLote.DBValue.IsNull)
                {
                    if (!this.tseLote.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Lote não cadastrado.";
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar um lote de transferência.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void grdPatrimonioSolicParaUE_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (!this.grdPatrimonioSolicParaUE.Visible || this.grdPatrimonioSolicParaUE.VisibleRowCount == 0)
            {
                return;
            }

            var situacao = (string)grdPatrimonioSolicParaUE.GetRowValues(e.VisibleIndex, "SITUACAO");

            var txtJustificativa = DevExpressHelper.GetControl<TextBox>(this.grdPatrimonioSolicParaUE, e.VisibleIndex, "JUSTIFICATIVA", "txtJustificativa");

            if (string.IsNullOrEmpty(situacao))
            {
                return;
            }

            var rbAceitar = DevExpressHelper.GetControl<RadioButton>(this.grdPatrimonioSolicParaUE, e.VisibleIndex, "ANDAMENTO", "rbAceitar");
            var rbRecusar = DevExpressHelper.GetControl<RadioButton>(this.grdPatrimonioSolicParaUE, e.VisibleIndex, "ANDAMENTO", "rbRecusar");

            if (rbAceitar == null
                || rbRecusar == null)
            {
                return;
            }

            rbAceitar.Enabled = false;
            rbRecusar.Enabled = false;
            txtJustificativa.Enabled = false;
            txtJustificativa.BackColor = Color.Gainsboro;
            txtJustificativa.TabIndex = -1;

            if (situacao == Transferencia.Pendente)
            {
                rbAceitar.Enabled = true;
                rbRecusar.Enabled = true;
            }

            if (situacao == Transferencia.Aceita)
            {
                rbAceitar.Checked = true;
                rbRecusar.Checked = false;
            }

            if (situacao == Transferencia.Recusada)
            {
                rbAceitar.Checked = false;
                rbRecusar.Checked = true;
            }

            rbAceitar.InputAttributes.Add("txtJustificativa", txtJustificativa.ClientID);
            rbRecusar.InputAttributes.Add("txtJustificativa", txtJustificativa.ClientID);
        }

        protected void grdPatrimonioSolicParaUE_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "VALORCOMSIGLA")
            {
                var sigla = e.GetListSourceFieldValue("SIGLA");
                var valor = e.GetListSourceFieldValue("VALOR");

                e.Value = sigla + " " + valor;
            }
        }

        protected void grdPatrimonioSolicPelaUE_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "VALORCOMSIGLA")
            {
                var sigla = e.GetListSourceFieldValue("SIGLA");
                var valor = e.GetListSourceFieldValue("VALOR");

                e.Value = sigla + " " + valor;
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                for (var rowIndex = 0; rowIndex < this.grdPatrimonioSolicParaUE.VisibleRowCount; rowIndex++)
                {
                    var idItem = (int)this.grdPatrimonioSolicParaUE.GetRowValues(rowIndex, "TRANSFERENCIAITEMID");
                    var id = (int)this.grdPatrimonioSolicParaUE.GetRowValues(rowIndex, "TRANSFERENCIAID");
                    var situacao = (string)this.grdPatrimonioSolicParaUE.GetRowValues(rowIndex, "SITUACAO");
                    var bem = (string)this.grdPatrimonioSolicParaUE.GetRowValues(rowIndex, "BEM");
                    var numeroBem = (string)this.grdPatrimonioSolicParaUE.GetRowValues(rowIndex, "NUMERO");
                    var rbAceitar = DevExpressHelper.GetControl<RadioButton>(this.grdPatrimonioSolicParaUE, rowIndex, "ANDAMENTO", "rbAceitar");
                    var rbRecusar = DevExpressHelper.GetControl<RadioButton>(this.grdPatrimonioSolicParaUE, rowIndex, "ANDAMENTO", "rbRecusar");

                    if (id > 0
                        && situacao.Equals("Pendente"))
                    {
                        if (rbRecusar.Checked)
                        {
                            var bemDescricao = numeroBem + " - " + bem;
                            blRecusados.Items.Add(bemDescricao);
                        }

                        if (rbAceitar.Checked)
                        {
                            var bemDescricao = numeroBem + " - " + bem;
                            blAceitos.Items.Add(bemDescricao);
                        }
                    }
                }


                this.trAceitos.Visible = false;
                this.trRecusados.Visible = false;
                this.btnConfirmar.Visible = false;

                if (blAceitos.Items.Count == 0
                    && blRecusados.Items.Count == 0)
                {
                    this.lblMensagemPopup.Text = "Favor selecionar pelo menos um bem para aceitar ou recusar a transferência!";

                    this.btnCancelar.Text = "Fechar";

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopup();", true);

                    return;
                }

                this.btnConfirmar.Visible = true;
                this.btnCancelar.Text = "Cancelar";

                this.lblMensagemPopup.Text = string.Format(
                    "As transferências para a unidade {0} - {1} serão processadas conforme abaixo:",
                    tseUA.DBValue.ToString(), tseUA["nome"].ToString());

                if (blAceitos.Items.Count > 0)
                {
                    this.trAceitos.Visible = true;
                }

                if (blRecusados.Items.Count > 0)
                {
                    this.trRecusados.Visible = true;
                }

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopup();", true);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            RN.Patrimonio.TransferenciaItem rnTransferenciaItem = new Techne.Lyceum.RN.Patrimonio.TransferenciaItem();
            RN.Patrimonio.Entidades.TransferenciaItem transferencia = new Techne.Lyceum.RN.Patrimonio.Entidades.TransferenciaItem();
            ValidacaoDados validacao = new ValidacaoDados();
            List<RN.Patrimonio.Entidades.TransferenciaItem> transferencias = new List<RN.Patrimonio.Entidades.TransferenciaItem>();
            this.pucConfirmar.ShowOnPageLoad = false;

            try
            {
                for (var rowIndex = 0; rowIndex < this.grdPatrimonioSolicParaUE.VisibleRowCount; rowIndex++)
                {
                    var iditem = grdPatrimonioSolicParaUE.GetRowValues(rowIndex, "TRANSFERENCIAITEMID").ToString();
                    var id = grdPatrimonioSolicParaUE.GetRowValues(rowIndex, "TRANSFERENCIAID").ToString();
                    var situacao = grdPatrimonioSolicParaUE.GetRowValues(rowIndex, "SITUACAO").ToString();
                    var bemid = grdPatrimonioSolicParaUE.GetRowValues(rowIndex, "BEMID");
                    var valor = grdPatrimonioSolicParaUE.GetRowValues(rowIndex, "VALOR");
                    var numero = grdPatrimonioSolicParaUE.GetRowValues(rowIndex, "NUMERO");
                    var moedaId = grdPatrimonioSolicParaUE.GetRowValues(rowIndex, "MOEDAID");
                    var txtJustificativa = DevExpressHelper.GetControl<TextBox>(this.grdPatrimonioSolicParaUE, rowIndex, "JUSTIFICATIVA", "txtJustificativa");
                    var rbAceitar = DevExpressHelper.GetControl<RadioButton>(this.grdPatrimonioSolicParaUE, rowIndex, "ANDAMENTO", "rbAceitar");
                    var rbRecusar = DevExpressHelper.GetControl<RadioButton>(this.grdPatrimonioSolicParaUE, rowIndex, "ANDAMENTO", "rbRecusar");

                    if (id != null
                        && situacao.Equals("Pendente"))
                    {
                        if (rbRecusar != null)
                        {
                            if (rbRecusar.Checked)
                            {
                                transferencia = new Techne.Lyceum.RN.Patrimonio.Entidades.TransferenciaItem();

                                transferencia.TransferenciaItemId = iditem != null ? Convert.ToInt32(iditem) : -1;
                                transferencia.TransferenciaId = id != null ? Convert.ToInt32(id) : -1;
                                transferencia.Situacao = Transferencia.Recusada;
                                transferencia.Justificativa = string.IsNullOrEmpty(txtJustificativa.Text) ? null : txtJustificativa.Text;
                                transferencia.Valor = valor != null ? Convert.ToDecimal(valor) : -1;
                                transferencia.BemId = bemid != null ? Convert.ToInt32(bemid) : -1;
                                transferencia.NumeroBemOrigem = numero != null ? Convert.ToInt32(numero) : -1;
                                transferencia.MoedaId = moedaId != null ? Convert.ToInt32(moedaId) : (int?)null;

                                transferencias.Add(transferencia);
                            }
                        }

                        if (rbAceitar != null)
                        {
                            if (rbAceitar.Checked)
                            {
                                transferencia = new Techne.Lyceum.RN.Patrimonio.Entidades.TransferenciaItem();

                                transferencia.TransferenciaItemId = iditem != null ? Convert.ToInt32(iditem) : -1;
                                transferencia.TransferenciaId = id != null ? Convert.ToInt32(id) : -1;
                                transferencia.Situacao = Transferencia.Aceita;
                                transferencia.Valor = valor != null ? Convert.ToDecimal(valor) : -1;
                                transferencia.BemId = bemid != null ? Convert.ToInt32(bemid) : -1;
                                transferencia.NumeroBemOrigem = numero != null ? Convert.ToInt32(numero) : -1;
                                transferencia.MoedaId = moedaId != null ? Convert.ToInt32(moedaId) : (int?)null;

                                transferencias.Add(transferencia);
                            }
                        }
                    }
                }

                if (transferencias.Count > 0)
                {
                    string setor = tseUA["setor"].ToString();
                    validacao = rnTransferenciaItem.ValidaAndamento(transferencias, setor, User.Identity.Name, (dtDataTransferencia.Date != null ? dtDataTransferencia.Date : DateTime.MinValue));

                    if (!validacao.Valido)
                    {
                        if (!string.IsNullOrEmpty(validacao.Mensagem))
                        {
                            this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        }

                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "atualizarGrid", "atualizarGrid();", true);
                        return;
                    }

                    rnTransferenciaItem.Transfere(transferencias, setor, User.Identity.Name, dtDataTransferencia.Date);

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Transferências Aceitas/Recusadas com sucesso.');", true);
                    lblMensagem.Text = "Transferências Aceitas/Recusadas com sucesso.";
                }
                else
                {
                    lblMensagem.Text = "Não existem transferências pendentes para a escola.";
                }

                CarregaDadosGrid();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaDadosGrid()
        {
            odsAcompanhamentoOrigem.Select();
            odsAcompanhamentoOrigem.DataBind();
            grdPatrimonioSolicPelaUE.DataBind();

            odsAcompanhamentoDestino.Select();
            odsAcompanhamentoDestino.DataBind();
            grdPatrimonioSolicParaUE.DataBind();
        }

        protected void btnAceitarTodas_Click(object sender, EventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Patrimonio.TransferenciaItem rnTransferenciaItem = new Techne.Lyceum.RN.Patrimonio.TransferenciaItem();
            RN.Patrimonio.Entidades.TransferenciaItem transferencia = new Techne.Lyceum.RN.Patrimonio.Entidades.TransferenciaItem();
            List<RN.Patrimonio.Entidades.TransferenciaItem> transferencias = new List<RN.Patrimonio.Entidades.TransferenciaItem>();

            try
            {
                for (var rowIndex = 0; rowIndex < this.grdPatrimonioSolicParaUE.VisibleRowCount; rowIndex++)
                {
                    var iditem = grdPatrimonioSolicParaUE.GetRowValues(rowIndex, "TRANSFERENCIAITEMID");
                    var id = grdPatrimonioSolicParaUE.GetRowValues(rowIndex, "TRANSFERENCIAID");
                    var bemid = grdPatrimonioSolicParaUE.GetRowValues(rowIndex, "BEMID");
                    var valor = grdPatrimonioSolicParaUE.GetRowValues(rowIndex, "VALOR");
                    var numero = grdPatrimonioSolicParaUE.GetRowValues(rowIndex, "NUMERO");
                    var moedaId = grdPatrimonioSolicParaUE.GetRowValues(rowIndex, "MOEDAID");

                    transferencia = new Techne.Lyceum.RN.Patrimonio.Entidades.TransferenciaItem();


                    transferencia.TransferenciaItemId = iditem != null ? Convert.ToInt32(iditem) : -1;
                    transferencia.TransferenciaId = id != null ? Convert.ToInt32(id) : -1;
                    transferencia.Situacao = Transferencia.Aceita;
                    transferencia.Valor = valor != null ? Convert.ToDecimal(valor) : -1;
                    transferencia.BemId = bemid != null ? Convert.ToInt32(bemid) : -1;
                    transferencia.NumeroBemOrigem = numero != null ? Convert.ToInt32(numero) : -1;
                    transferencia.MoedaId = moedaId != null ? Convert.ToInt32(moedaId) : (int?)null;

                    transferencias.Add(transferencia);

                }

                if (transferencias.Count > 0)
                {
                    string setor = tseUA["setor"].ToString();
                    validacao = rnTransferenciaItem.ValidaAndamento(transferencias, setor, User.Identity.Name, (dtDataTransferencia.Date != null ? dtDataTransferencia.Date : DateTime.MinValue));

                    if (!validacao.Valido)
                    {
                        if (!string.IsNullOrEmpty(validacao.Mensagem))
                        {
                            this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        }

                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "atualizarGrid", "atualizarGrid();", true);
                        return;
                    }

                    rnTransferenciaItem.Transfere(transferencias, setor, User.Identity.Name, dtDataTransferencia.Date);

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Transferências Aceitas com sucesso.');", true);
                    this.lblMensagem.Text = "Transferências Aceitas com sucesso.";
                }
                else
                {
                    lblMensagemPopup.Text = "Não existem transferências a serem aceitas para a escola.";
                    lblMensagem.Text = "Não existem transferências a serem aceitas para a escola.";

                    btnCancelar.Text = "Fechar";

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopup();", true);

                }
                CarregaDadosGrid();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnRecusarTodas_Click(object sender, EventArgs e)
        {
            try
            {
                txtJustificativa.Text = string.Empty;

                if (this.grdPatrimonioSolicParaUE.VisibleRowCount > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopupRecusados();", true);
                }
                else
                {
                    lblMensagemPopup.Text = "Não existem transferências a serem recusadas para a escola.";
                    lblMensagem.Text = "Não existem transferências a serem recusadas para a escola.";
                    btnCancelar.Text = "Fechar";

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopup();", true);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnConfirmarRecusados_Click(object sender, EventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Patrimonio.TransferenciaItem rnTransferenciaItem = new Techne.Lyceum.RN.Patrimonio.TransferenciaItem();
            RN.Patrimonio.Entidades.TransferenciaItem transferencia = new Techne.Lyceum.RN.Patrimonio.Entidades.TransferenciaItem();
            List<RN.Patrimonio.Entidades.TransferenciaItem> transferencias = new List<RN.Patrimonio.Entidades.TransferenciaItem>();
            this.pucConfirmarRecusados.ShowOnPageLoad = false;

            try
            {
                for (var rowIndex = 0; rowIndex < this.grdPatrimonioSolicParaUE.VisibleRowCount; rowIndex++)
                {
                    var iditem = grdPatrimonioSolicParaUE.GetRowValues(rowIndex, "TRANSFERENCIAITEMID");
                    var id = grdPatrimonioSolicParaUE.GetRowValues(rowIndex, "TRANSFERENCIAID");
                    var bemid = grdPatrimonioSolicParaUE.GetRowValues(rowIndex, "BEMID");
                    var valor = grdPatrimonioSolicParaUE.GetRowValues(rowIndex, "VALOR");
                    var numero = grdPatrimonioSolicParaUE.GetRowValues(rowIndex, "NUMERO");
                    var moedaId = grdPatrimonioSolicParaUE.GetRowValues(rowIndex, "MOEDAID");

                    transferencia = new Techne.Lyceum.RN.Patrimonio.Entidades.TransferenciaItem();

                    transferencia.TransferenciaItemId = iditem != null ? Convert.ToInt32(iditem) : -1;
                    transferencia.TransferenciaId = id != null ? Convert.ToInt32(id) : -1;
                    transferencia.Situacao = Transferencia.Recusada;
                    transferencia.Justificativa = txtJustificativa.Text.Trim();
                    transferencia.Valor = valor != null ? Convert.ToDecimal(valor) : -1;
                    transferencia.BemId = bemid != null ? Convert.ToInt32(bemid) : -1;
                    transferencia.NumeroBemOrigem = numero != null ? Convert.ToInt32(numero) : -1;
                    transferencia.MoedaId = moedaId != null ? Convert.ToInt32(moedaId) : (int?)null;

                    transferencias.Add(transferencia);

                }

                if (transferencias.Count > 0)
                {
                    string setor = tseUA["setor"].ToString();
                    validacao = rnTransferenciaItem.ValidaAndamento(transferencias, setor, User.Identity.Name, (dtDataTransferencia.Date != null ? dtDataTransferencia.Date : DateTime.MinValue));

                    if (!validacao.Valido)
                    {
                        if (!string.IsNullOrEmpty(validacao.Mensagem))
                        {
                            this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        }

                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "atualizarGrid", "atualizarGrid();", true);
                        return;
                    }

                    rnTransferenciaItem.Transfere(transferencias, setor, User.Identity.Name, dtDataTransferencia.Date);

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Transferências Recusadas com sucesso.');", true);
                    this.lblMensagem.Text = "Transferências Recusadas com sucesso.";
                }
                else
                {
                    lblMensagemPopup.Text = "Não existem transferências a serem recusadas para a escola.";
                    lblMensagem.Text = "Não existem transferências a serem recusadas para a escola.";
                    btnCancelar.Text = "Fechar";

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopup();", true);
                }
                CarregaDadosGrid();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void dtDataTransferencia_PreRender(object sender, EventArgs e)
        {
            var self = sender as ASPxDateEdit;
            var currentYear = DateTime.Now.Year;

            dtDataTransferencia.MinDate = new DateTime(currentYear, 1, 1);
            dtDataTransferencia.MaxDate = new DateTime(currentYear, 12, 31);
        }
    }
}
