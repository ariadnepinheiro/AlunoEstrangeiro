using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using DevExpress.Web.ASPxTabControl;
using Techne.Lyceum.RN.Util;
using Techne.Controls;
using System.Data;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [NavUrl("~/PrestacaoContas/ProdutoServico.aspx"), ControlText("Produtos e Serviços"), Title("Produtos e Serviços")]
    public partial class ProdutoServico : TPage
    {
        public enum TipoOperacao
        {
            Novo,
            Alterar,
            Consultar,
            Inicial,
            Sucesso,
            Cancelar
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

        public object ListaRestricao(object produtoServicoId)
        {
            RN.PrestacaoContas.ProdutoServicoValorMaximo rnProdutoServicoValorMaximo = new Techne.Lyceum.RN.PrestacaoContas.ProdutoServicoValorMaximo();

            if (produtoServicoId != null)
            {
                if (!string.IsNullOrEmpty(produtoServicoId.ToString()))
                {
                    return rnProdutoServicoValorMaximo.ListaPor(Convert.ToInt32(produtoServicoId.ToString()));
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
            TituloGrid(grdRestricao, "");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdRestricao);
            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnNovo, AcaoControle.novo);
            
        }

        private void LimpaDadosInfoGerais()
        {
            txtProduto.Text = string.Empty;
            txtDetalhes.Text = string.Empty;
            txtNcm.Text = string.Empty;
            txtCodigoFGV.Text = string.Empty;
            tseGrupo.ResetValue();
            ddlTipo.ClearSelection();
            ddlUnidadeMedida.ClearSelection();
            ddlFinalidade.ClearSelection();
            chkPequenaDespesa.Checked = false;
            chkOrcamento.Checked = false;
            chkInventariavel.Checked = false;
            chkNcmNaoEspecifico.Checked = false;
        }

        private void LimpaDadosRestricao()
        {
            ddlRegiaoFgv.ClearSelection();
            txtValorMaximo.Text = string.Empty;
            dtDataInicio.Text = string.Empty;
            dtDataFim.Text = string.Empty;
        }

        private void ControlarTSearchs()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        tseGrupoFiltro.Enabled = true;
                        tseGrupoFiltro.Mode = ControlMode.Edit;
                        tseProdutoServico.Enabled = true;
                        tseProdutoServico.Mode = ControlMode.Edit;
                        btnBuscar.Visible = true;
                        break;
                    }

                case TipoOperacao.Sucesso:
                    {
                        tseGrupoFiltro.Enabled = true;
                        tseGrupoFiltro.Mode = ControlMode.Edit;
                        tseProdutoServico.Enabled = true;
                        tseProdutoServico.Mode = ControlMode.Edit;
                        tseGrupo.Mode = ControlMode.View;
                        btnBuscar.Visible = true;

                        break;
                    }
                case TipoOperacao.Cancelar:
                    {
                        tseGrupoFiltro.Enabled = true;
                        tseGrupoFiltro.Mode = ControlMode.Edit;
                        tseProdutoServico.Enabled = true;
                        tseProdutoServico.Mode = ControlMode.Edit;
                        tseGrupo.Enabled = false;
                        tseGrupo.Mode = ControlMode.View;
                        btnBuscar.Visible = true;

                        break;
                    }
                case TipoOperacao.Novo:
                    {
                        tseGrupoFiltro.Enabled = false;
                        tseGrupoFiltro.Mode = ControlMode.View;
                        tseProdutoServico.Enabled = false;
                        tseProdutoServico.Mode = ControlMode.View;
                        tseGrupo.Enabled = true;
                        tseGrupo.Mode = ControlMode.Edit;
                        btnBuscar.Visible = false;

                        break;
                    }

                case TipoOperacao.Alterar:
                    {
                        tseGrupoFiltro.Enabled = false;
                        tseGrupoFiltro.Mode = ControlMode.View;
                        tseProdutoServico.Enabled = false;
                        tseProdutoServico.Mode = ControlMode.View;
                        tseGrupo.Mode = ControlMode.Edit;
                        btnBuscar.Visible = false;

                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        tseGrupoFiltro.Mode = ControlMode.Edit;
                        tseProdutoServico.Mode = ControlMode.Edit;
                        tseGrupo.Mode = ControlMode.View;
                        btnBuscar.Visible = true;

                        break;
                    }
            }
        }

        private void ControlarVisibilidadeControle(ImageButton[] imgBotoes, Button[] botoes)
        {
            RetiraVisibilidadeBotao();

            if (imgBotoes != null)
            {
                foreach (ImageButton botao in imgBotoes)
                {
                    botao.Visible = true;
                }
            }

            if (botoes != null)
            {
                foreach (Button botao in botoes)
                {
                    botao.Visible = true;
                }
            }

            ControlaAcesso(btnSalvar, AcaoControle.editar);
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(btnSalvarRestricao, AcaoControle.editar);
        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnEditar.Visible = false;
            btnNovo.Visible = false;
            btnSalvar.Visible = false;
            btnSalvarRestricao.Visible = false;
        }

        private void ControlarTipoOperacao()
        {
            RN.PrestacaoContas.ProdutoServico rnProdutoServico = new Techne.Lyceum.RN.PrestacaoContas.ProdutoServico();
            RN.PrestacaoContas.Entidades.ProdutoServico produtoServico = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ProdutoServico();

            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles, null);
                        ControlarTSearchs();
                        LimpaDadosInfoGerais();
                        LimpaDadosRestricao();
                        tseGrupoFiltro.ResetValue();
                        tseProdutoServico.ResetValue();
                        pcProdutoServico.Visible = false;
                        CarregaTipo();
                        CarregaUnidadeMedida();
                        CarregaFinalidade();
                        CarregaRegiaoFgv();
                        pcProdutoServico.ActiveTabIndex = 0;

                        break;
                    }
                case TipoOperacao.Cancelar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles, null);
                        ControlarTSearchs();
                        LimpaDadosInfoGerais();
                        LimpaDadosRestricao();
                        pcProdutoServico.Visible = false;
                        btnBuscar.Visible = true;
                        break;
                    }
                case TipoOperacao.Novo:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles, null);
                        ControlarTSearchs();

                        tseProdutoServico.ResetValue();
                        tseGrupoFiltro.ResetValue();
                        LimpaDadosInfoGerais();
                        LimpaDadosRestricao();
                        pcProdutoServico.Visible = true;
                        HabilitaDesabilitaCamposAbaInformacoesGerais(true);
                        pcProdutoServico.TabPages[1].Enabled = false;

                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { };
                        controles = new[] { btnNovo, btnEditar };

                        ControlarVisibilidadeControle(controles, null);
                        ControlarTSearchs();

                        HabilitaDesabilitaCamposAbaInformacoesGerais(false);
                        HabilitaDesabilitaCamposAbaRestricao(false);
                        pcProdutoServico.TabPages[1].Enabled = true;
                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        RN.PrestacaoContas.ProdutoServicoGrupo rnProdutoServicoGrupo = new Techne.Lyceum.RN.PrestacaoContas.ProdutoServicoGrupo();
                        ImageButton[] controles = new ImageButton[] { };
                        Button[] controlesButton = new Button[] { };

                        pcProdutoServico.Visible = true;
                        LimpaDadosInfoGerais();
                        LimpaDadosRestricao();
                        HabilitaDesabilitaCamposAbaRestricao(false);
                        HabilitaDesabilitaCamposAbaInformacoesGerais(false);
                        pcProdutoServico.TabPages[1].Enabled = true;

                        produtoServico = rnProdutoServico.ObtemPor(Convert.ToInt32(tseProdutoServico.DBValue));

                        if (produtoServico.ProdutoServicoId > 0)
                        {
                            tseGrupo.DBValue = produtoServico.ProdutoServicoGrupoId > 0 ? rnProdutoServicoGrupo.ObtemCodigoCnaePor(produtoServico.ProdutoServicoGrupoId) : string.Empty;
                            tseGrupo_Changed(null, null);
                            txtProduto.Text = !produtoServico.Nome.IsNullOrEmptyOrWhiteSpace() ? produtoServico.Nome.Trim().ToUpper() : string.Empty;
                            txtDetalhes.Text = !produtoServico.Detalhe.IsNullOrEmptyOrWhiteSpace() ? produtoServico.Detalhe.Trim().ToUpper() : string.Empty;
                            txtNcm.Text = !produtoServico.Ncm.IsNullOrEmptyOrWhiteSpace() ? produtoServico.Ncm.Trim().ToUpper() : string.Empty;
                            txtCodigoFGV.Text = produtoServico.CodigoFgv != null ? produtoServico.CodigoFgv.ToString() : string.Empty;
                            ddlTipo.SelectedValue = produtoServico.TipoProdutoServicoId > 0 ? produtoServico.TipoProdutoServicoId.ToString() : string.Empty;
                            ddlFinalidade.SelectedValue = produtoServico.FinalidadeId > 0 ? produtoServico.FinalidadeId.ToString() : string.Empty;
                            chkPequenaDespesa.Checked = produtoServico.PequenasDespesas ? true : false;
                            chkOrcamento.Checked = produtoServico.NecessitaOrcamento;
                            chkInventariavel.Checked = produtoServico.Inventariavel ? true : false;
                            chkNcmNaoEspecifico.Checked = produtoServico.FlagNcm ? true : false;

                            if (produtoServico.UnidadeMedidaId > 0)
                            {
                                //Verifica se existe a unidade de medida no combo
                                if (ddlUnidadeMedida.Items.FindByValue(produtoServico.UnidadeMedidaId.ToString()) != null)
                                {
                                    ddlUnidadeMedida.SelectedValue = produtoServico.UnidadeMedidaId.ToString();
                                }
                            }

                            controles = new[] { btnNovo, btnEditar };

                        }


                        ControlarVisibilidadeControle(controles, controlesButton);
                        ControlarTSearchs();

                        grdRestricao.Columns[0].Visible = false;

                        break;

                    }
                case TipoOperacao.Alterar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        Button[] controlesButton = new[] { btnSalvarRestricao };

                        ControlarVisibilidadeControle(controles, controlesButton);
                        ControlarTSearchs();
                        HabilitaDesabilitaCamposAbaInformacoesGerais(true);
                        HabilitaDesabilitaCamposAbaRestricao(true);
                        pcProdutoServico.TabPages[1].Enabled = true;
                        grdRestricao.Columns[0].Visible = true;
                        break;
                    }
            }
        }

        private void HabilitaDesabilitaCamposAbaInformacoesGerais(bool habilita)
        {
            txtProduto.Enabled = habilita;
            txtDetalhes.Enabled = habilita;
            txtCodigoFGV.Enabled = habilita;
            txtNcm.Enabled = habilita;
            ddlTipo.Enabled = habilita;
            ddlUnidadeMedida.Enabled = habilita;
            ddlFinalidade.Enabled = habilita;
            chkPequenaDespesa.Enabled = habilita;
            chkOrcamento.Enabled = habilita;
            chkInventariavel.Enabled = habilita;
            chkNcmNaoEspecifico.Enabled = habilita;
            tseGrupo.Mode = habilita ? ControlMode.Edit : ControlMode.View;
        }

        private void HabilitaDesabilitaCamposAbaRestricao(bool habilita)
        {
            txtValorMaximo.Enabled = habilita;
            ddlRegiaoFgv.Enabled = habilita;
            dtDataInicio.Enabled = habilita;
            dtDataFim.Enabled = habilita;
        }

        private void CarregaUnidadeMedida()
        {
            RN.PrestacaoContas.UnidadeMedida rnUnidadeMedida = new Techne.Lyceum.RN.PrestacaoContas.UnidadeMedida();
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlUnidadeMedida.Items.Clear();
            ddlUnidadeMedida.DataSource = rnUnidadeMedida.ListaAtivo();
            ddlUnidadeMedida.DataBind();
            ddlUnidadeMedida.Items.Insert(0, item);
        }

        private void CarregaFinalidade()
        {
            RN.PrestacaoContas.Finalidade rnFinalidade = new Techne.Lyceum.RN.PrestacaoContas.Finalidade();
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlFinalidade.Items.Clear();
            ddlFinalidade.DataSource = rnFinalidade.ListaAtivo();
            ddlFinalidade.DataBind();
            ddlFinalidade.Items.Insert(0, item);
        }

        private void CarregaTipo()
        {
            RN.PrestacaoContas.TipoProdutoServico rnTipoProdutoServico = new Techne.Lyceum.RN.PrestacaoContas.TipoProdutoServico();

            ListItem item = new ListItem("Selecione", string.Empty);

            ddlTipo.Items.Clear();
            ddlTipo.DataSource = rnTipoProdutoServico.ListaAtivo();
            ddlTipo.DataBind();
            ddlTipo.Items.Insert(0, item);

        }

        private void CarregaRegiaoFgv()
        {
            RN.PrestacaoContas.RegiaoFgv rnRegiaoFgv = new Techne.Lyceum.RN.PrestacaoContas.RegiaoFgv();
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlRegiaoFgv.Items.Clear();
            ddlRegiaoFgv.DataSource = rnRegiaoFgv.ListaAtivo();
            ddlRegiaoFgv.DataBind();
            ddlRegiaoFgv.Items.Insert(0, item);
        }

        protected void btnBuscar_Click(object sender, EventArgs args)
        {
            List<string> mensagens = new List<string>();

            try
            {
                if ((tseGrupoFiltro.Value == null) || (string.IsNullOrEmpty(tseGrupoFiltro.Value.ToString())))
                {
                    mensagens.Add("O campo Grupo de Produto / Serviço é obrigatório.");
                }

                if ((tseProdutoServico.Value == null) || (string.IsNullOrEmpty(tseProdutoServico.Value.ToString())))
                {
                    mensagens.Add("O campo Produto / Serviço é obrigatório.");
                }

                if (mensagens.Count > 0)
                {
                    lblMensagem.Text = mensagens.Aggregate((x, y) => x + Environment.NewLine + y).Replace(Environment.NewLine, "<br />");

                }
                else
                {
                    this._tipoOperacao = TipoOperacao.Consultar;
                    tseGrupoFiltro.DBValue = tseProdutoServico["codigocnae"];
                    tseGrupoFiltro.DataBind();
                    lblMensagem.Text = string.Empty;
                    ControlarTipoOperacao();
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
                _tipoOperacao = TipoOperacao.Cancelar;
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
                _tipoOperacao = TipoOperacao.Novo;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnEditar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Alterar;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            RN.PrestacaoContas.ProdutoServico rnProdutoServicor = new Techne.Lyceum.RN.PrestacaoContas.ProdutoServico();
            RN.PrestacaoContas.Entidades.ProdutoServico produtoServico = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ProdutoServico();
            ValidacaoDados validacao = new ValidacaoDados();
            string mensagem = string.Empty;

            try
            {
                produtoServico.ProdutoServicoId = !this.tseProdutoServico.DBValue.IsNull && tseProdutoServico.IsValidDBValue ? Convert.ToInt32(tseProdutoServico.DBValue) : -1;
                produtoServico.ProdutoServicoGrupoId = !this.tseGrupo.DBValue.IsNull && tseGrupo.IsValidDBValue ? Convert.ToInt32(tseGrupo["produtoservicogrupoid"]) : -1;
                produtoServico.Nome = !txtProduto.Text.IsNullOrEmptyOrWhiteSpace() ? txtProduto.Text.Trim().ToUpper() : null;
                produtoServico.Detalhe = !txtDetalhes.Text.IsNullOrEmptyOrWhiteSpace() ? txtDetalhes.Text.Trim().ToUpper() : null;
                produtoServico.Ncm = !txtNcm.Text.IsNullOrEmptyOrWhiteSpace() ? txtNcm.Text.Trim().ToUpper() : null;
                produtoServico.TipoProdutoServicoId = !ddlTipo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlTipo.SelectedValue) : -1;
                produtoServico.UnidadeMedidaId = !ddlUnidadeMedida.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlUnidadeMedida.SelectedValue) : -1;
                produtoServico.FinalidadeId = !ddlFinalidade.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlFinalidade.SelectedValue) : -1;
                produtoServico.PequenasDespesas = chkPequenaDespesa.Checked;
                produtoServico.NecessitaOrcamento = chkOrcamento.Checked;
                produtoServico.Inventariavel = chkInventariavel.Checked;
                produtoServico.FlagNcm = chkNcmNaoEspecifico.Checked;
                produtoServico.CodigoFgv = !txtCodigoFGV.Text.IsNullOrEmptyOrWhiteSpace() ? (int?)Convert.ToUInt32(txtCodigoFGV.Text.Trim()) : (int?)null;
                produtoServico.Ativo = true;
                produtoServico.UsuarioId = User.Identity.Name;

                validacao = rnProdutoServicor.Valida(produtoServico, (produtoServico.ProdutoServicoId == -1 ? true : false));

                if (validacao.Valido)
                {
                    if (_tipoOperacao == TipoOperacao.Novo)
                    {
                        rnProdutoServicor.Insere(produtoServico);
                        tseGrupoFiltro.ResetValue();
                        tseGrupoFiltro.DBValue = tseGrupo.DBValue;
                        tseProdutoServico.ResetValue();
                        tseProdutoServico.DBValue = produtoServico.ProdutoServicoId;

                        mensagem = "Item inserido com sucesso.";
                    }
                    else
                    {
                        rnProdutoServicor.Atualiza(produtoServico);
                        mensagem = "Item atualizado com sucesso.";
                    }

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('" + mensagem + ".');", true);
                    lblMensagem.Text = mensagem;

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

        protected void pcProdutoServico_TabClick(object source, TabControlCancelEventArgs e)
        {
            lblMensagem.Text = string.Empty;
        }

        protected void btnSalvarRestricao_Click(object sender, EventArgs e)
        {
            RN.PrestacaoContas.Entidades.ProdutoServicoValorMaximo produtoServicoValorMaximo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ProdutoServicoValorMaximo();
            RN.PrestacaoContas.ProdutoServicoValorMaximo rnProdutoServicoValorMaximo = new Techne.Lyceum.RN.PrestacaoContas.ProdutoServicoValorMaximo();
            ValidacaoDados validacao = new ValidacaoDados();
            try
            {
                produtoServicoValorMaximo.ProdutoServicoId = !tseProdutoServico.DBValue.IsNull && tseProdutoServico.IsValidDBValue ? Convert.ToInt32(tseProdutoServico.DBValue) : -1;
                produtoServicoValorMaximo.RegiaoFgvId = !ddlRegiaoFgv.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlRegiaoFgv.SelectedValue) : -1;
                produtoServicoValorMaximo.ValorMaximo = !txtValorMaximo.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(txtValorMaximo.Text.Trim()) : -1;
                produtoServicoValorMaximo.DataInicio = !dtDataInicio.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataInicio.Date : DateTime.MinValue;
                produtoServicoValorMaximo.DataFim = !dtDataFim.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataFim.Date : (DateTime?)null;
                produtoServicoValorMaximo.UsuarioId = User.Identity.Name;

                validacao = rnProdutoServicoValorMaximo.Valida(produtoServicoValorMaximo, true);

                if (validacao.Valido)
                {
                    rnProdutoServicoValorMaximo.Insere(produtoServicoValorMaximo);
                    grdRestricao.DataBind();
                    LimpaDadosRestricao();
                    lblMensagem.Text = "Restrição de Valores salvo com sucesso.";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Restrição de Valores salvo com sucesso.');", true);

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

        public void Update(object REGIAOFGV, object VALORMAXIMO, object DATAINICIO, object DATAFIM, object PRODUTOSERVICOVALORMAXIMOID) { }

        public void Delete(object PRODUTOSERVICOVALORMAXIMOID) { }

        protected void grdRestricao_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdRestricao);
        }

        protected void grdRestricao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdRestricao.Settings.ShowFilterRow = false;
        }

        protected void grdRestricao_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdRestricao.Settings.ShowFilterRow = false;
        }

        protected void grdRestricao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.ProdutoServicoValorMaximo produtoServicoValorMaximo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ProdutoServicoValorMaximo();
            RN.PrestacaoContas.ProdutoServicoValorMaximo rnProdutoServicoValorMaximo = new Techne.Lyceum.RN.PrestacaoContas.ProdutoServicoValorMaximo();

            produtoServicoValorMaximo.ProdutoServicoValorMaximoId = Convert.ToInt32(e.Keys["PRODUTOSERVICOVALORMAXIMOID"]);
            produtoServicoValorMaximo.ProdutoServicoId = !tseProdutoServico.DBValue.IsNull && tseProdutoServico.IsValidDBValue ? Convert.ToInt32(tseProdutoServico.DBValue) : -1;
            produtoServicoValorMaximo.ValorMaximo = e.NewValues["VALORMAXIMO"] != null ? Convert.ToDecimal(e.NewValues["VALORMAXIMO"].ToString()) : -1;
            produtoServicoValorMaximo.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            produtoServicoValorMaximo.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            produtoServicoValorMaximo.UsuarioId = User.Identity.Name;

            validacao = rnProdutoServicoValorMaximo.Valida(produtoServicoValorMaximo, false);

            if (validacao.Valido)
            {
                rnProdutoServicoValorMaximo.Atualiza(produtoServicoValorMaximo);
                grdRestricao.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

        }

        protected void grdRestricao_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.ProdutoServicoValorMaximo rnProdutoServicoValorMaximo = new Techne.Lyceum.RN.PrestacaoContas.ProdutoServicoValorMaximo();
            int produtoServicoValorMaximoId = 0;
            int produtoServicoId = 0;

            produtoServicoValorMaximoId = Convert.ToInt32(e.Keys["PRODUTOSERVICOVALORMAXIMOID"]);
            produtoServicoId = !tseProdutoServico.DBValue.IsNull && tseProdutoServico.IsValidDBValue ? Convert.ToInt32(tseProdutoServico.DBValue) : -1;

            validacao = rnProdutoServicoValorMaximo.ValidaRemocao(produtoServicoValorMaximoId, produtoServicoId, User.Identity.Name);

            if (validacao.Valido)
            {
                rnProdutoServicoValorMaximo.Remove(produtoServicoValorMaximoId, produtoServicoId, User.Identity.Name);
                grdRestricao.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

        protected void tseProdutoServico_Changed(object sender, EventArgs args)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (Page.IsCallback)
                {
                    return;
                }

                if (!tseProdutoServico.DBValue.IsNull)
                {
                    if (!tseProdutoServico.IsValidDBValue)
                    {
                        lblMensagem.Text = "Item não ativo ou não cadastrado (favor verificar).";
                    }
                    else
                    {
                        this._tipoOperacao = TipoOperacao.Consultar;
                        tseGrupoFiltro.DBValue = tseProdutoServico["codigocnae"];
                        tseGrupoFiltro.DataBind();

                        ControlarTipoOperacao();
                    }
                }
                else
                {
                    lblMensagem.Text = "Fornecedor não ativo ou não cadastrado (favor verificar).";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseGrupo_Changed(object sender, EventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }
                lblMensagem.Text = string.Empty;

                if (!tseGrupo.DBValue.IsNull)
                {
                    if (!tseGrupo.IsValidDBValue)
                    {
                        lblMensagem.Text = "Grupo não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Grupo não cadastrado (favor verificar).";
                }
                ControlarTSearchs();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseGrupoFiltro_Changed(object sender, EventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                lblMensagem.Text = string.Empty;

                if (!tseGrupoFiltro.DBValue.IsNull)
                {
                    if (!tseGrupoFiltro.IsValidDBValue)
                    {
                        lblMensagem.Text = "Grupo não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Grupo não cadastrado (favor verificar).";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
