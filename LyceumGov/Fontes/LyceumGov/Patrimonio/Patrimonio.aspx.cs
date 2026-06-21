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
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxClasses;
using System.Globalization;
using Techne.Lyceum.RN.Patrimonio;

namespace Techne.Lyceum.Net.Patrimonio
{
    [NavUrl("~/Patrimonio/Patrimonio.aspx"), ControlText("Patrimônio"), Title("Patrimônio")]

    public partial class Patrimonio : TPage
    {
        public enum TipoOperacao
        {
            Novo,
            Alterar,
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

        public object Lista(object bemId)
        {
            RN.Patrimonio.Reavaliacao rnReavaliacao = new Techne.Lyceum.RN.Patrimonio.Reavaliacao();

            if (bemId != null)
            {
                return rnReavaliacao.ListaPor(Convert.ToInt32(bemId.ToString()));
            }
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                lblMensagemReavaliacao.Text = string.Empty;
                lblMensagemBaixa.Text = string.Empty;

                if (!IsPostBack)
                {
                    _tipoOperacao = TipoOperacao.Inicial;
                    ControlarTipoOperacao();

                    if (Request.QueryString.Keys.Count > 0)
                    {
                        LimpaDados();
                        LimpaDadosReavaliacao();
                        LimpaDadosBaixa();
                        CarregaQueryString();
                    }
                }

                dtDataAquisicao.MaxDate = new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12));
                dtDataIncorporacao.MinDate = new DateTime(DateTime.Now.Year - 2, 1, 1);
                dtDataIncorporacao.MaxDate = new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12));
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdReavaliacao, string.Empty);
            AcessoGrid();
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            AcessoGrid();
        }

        private void LimpaDados()
        {
            hdnMoedaId.Value = string.Empty;
            rblOperacao.ClearSelection();
            tseClassificacao.ResetValue();
            txtDescricao.Text = string.Empty;
            ddlEstadoConservacao.ClearSelection();
            ddlPeriodoUtilizacaoFutura.ClearSelection();
            ddlPeriodoVidaUtil.ClearSelection();
            dtDataAquisicao.Text = string.Empty;
            dtDataIncorporacao.Text = string.Empty;
            txtValor.Text = string.Empty;
            txtMoeda.Text = string.Empty;
            txtDocumentoHabil.Text = string.Empty;
            txtHistoricoOperacao.Text = string.Empty;
            chkEfetuarBaixa.Checked = false;
            txtQtdeReplicacao.Text = string.Empty;
            LimpaDadosBaixa();
        }

        private void RetiraVisibilidadeControles()
        {
            lblEstadoConservacao.Visible = false;
            ddlEstadoConservacao.Visible = false;
            lblPeriodoUtilizacaoFutura.Visible = false;
            ddlPeriodoUtilizacaoFutura.Visible = false;
            lblDataAquisicao.Visible = false;
            dtDataAquisicao.Visible = false;
        }

        private void DesabilitaCampos()
        {
            rblOperacao.Enabled = false;
            tseClassificacao.Mode = ControlMode.View;
            tseClassificacao.ReadOnly = true;
            tseClassificacao.Enabled = false;
            txtDescricao.Enabled = false;
            ddlEstadoConservacao.Enabled = false;
            ddlPeriodoUtilizacaoFutura.Enabled = false;
            ddlPeriodoVidaUtil.Enabled = false;
            dtDataAquisicao.Enabled = false;
            dtDataIncorporacao.Enabled = false;
            txtValor.Enabled = false;
            txtMoeda.Enabled = false;
            txtDocumentoHabil.Enabled = false;
            txtHistoricoOperacao.Enabled = false;

        }

        private void CarregaQueryString()
        {
            RN.Setores rnSetor = new Setores();
            byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
            string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

            string[] listaDados = decodedText.Split('&');

            hdnbemId.Value = string.Empty;
            lblNomeUA.Text = string.Empty;
            lblUA.Text = string.Empty;
            hdnSetor.Value = string.Empty;

            foreach (string dados in listaDados)
            {
                if (dados.IndexOf("Setor") >= 0)
                {
                    lblUA.Text = dados.Substring(dados.LastIndexOf('=') + 1);
                    //Setor da unidade selecionada
                    hdnSetor.Value = rnSetor.ObtemSetorPor(lblUA.Text.Trim());
                }
                else if (dados.IndexOf("NomeUnidade") >= 0)
                {
                    lblNomeUA.Text = dados.Substring(dados.LastIndexOf('=') + 1);
                }
                else if (dados.IndexOf("BemId") >= 0)
                {
                    hdnbemId.Value = dados.Substring(dados.LastIndexOf('=') + 1);
                }
                else if (dados.IndexOf("tipoOperacao") >= 0)
                {
                    string tipoOperacao = dados.Substring(dados.LastIndexOf('=') + 1);

                    if (tipoOperacao == "NOVO")
                    {
                        _tipoOperacao = TipoOperacao.Novo;
                    }
                    else if (tipoOperacao == "ALTERAR")
                    {
                        _tipoOperacao = TipoOperacao.Alterar;
                    }
                    else if (tipoOperacao == "CONSULTAR")
                    {
                        _tipoOperacao = TipoOperacao.Consultar;

                    }
                }
            }
            ControlarTipoOperacao();
        }

        private void CarregaOperacao()
        {
            RN.Patrimonio.Operacao rnOperacao = new Techne.Lyceum.RN.Patrimonio.Operacao();

            rblOperacao.Items.Clear();
            rblOperacao.DataSource = rnOperacao.ListaOperacaoAtiva();
            rblOperacao.DataTextField = "DESCRICAO";
            rblOperacao.DataValueField = "OPERACAOID";
            rblOperacao.DataBind();
        }

        private void CarregaEstadoConservacao(DropDownList controle)
        {
            RN.Patrimonio.EstadoConservacao rnEstadoConservacao = new Techne.Lyceum.RN.Patrimonio.EstadoConservacao();
            ListItem item = new ListItem("Selecione", string.Empty);

            controle.Items.Clear();
            controle.DataSource = rnEstadoConservacao.ListaEstadoConservacaoAtivo();
            controle.DataBind();
            controle.Items.Insert(0, item);
        }

        private void CarregaPeriodoVidaUtilizado()
        {
            RN.Patrimonio.PeriodoVidaUtilizado rnPeriodoVidaUtilizado = new Techne.Lyceum.RN.Patrimonio.PeriodoVidaUtilizado();
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlPeriodoVidaUtil.Items.Clear();
            ddlPeriodoVidaUtil.DataSource = rnPeriodoVidaUtilizado.ListaPeriodoVidaUtilizadoAtivo();
            ddlPeriodoVidaUtil.DataBind();
            ddlPeriodoVidaUtil.Items.Insert(0, item);
        }

        private void CarregaPeriodoUtilizacaoFutura(DropDownList controle)
        {
            RN.Patrimonio.PeriodoVidaFutura rnPeriodoVidaFutura = new Techne.Lyceum.RN.Patrimonio.PeriodoVidaFutura();
            ListItem item = new ListItem("Selecione", string.Empty);

            controle.Items.Clear();
            controle.DataSource = rnPeriodoVidaFutura.ListaPeriodoVidaFuturaAtivo();
            controle.DataBind();
            controle.Items.Insert(0, item);
        }

        private void CarregaMotivoBaixa()
        {
            RN.Patrimonio.MotivoBaixa rnMotivoBaixa = new Techne.Lyceum.RN.Patrimonio.MotivoBaixa();
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlMotivoBaixa.Items.Clear();
            ddlMotivoBaixa.DataSource = rnMotivoBaixa.ListaMotivoBaixaAtivo();
            ddlMotivoBaixa.DataBind();
            ddlMotivoBaixa.Items.Insert(0, item);
        }

        private void ControlarTipoOperacao()
        {
            RN.Patrimonio.PeriodoLancamento rnPeriodoLancamento = new Techne.Lyceum.RN.Patrimonio.PeriodoLancamento();
            RN.Perfil rnPerfil = new Perfil();

            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { btnSalvar, btnCancel };
                        ControlarVisibilidadeControle(controles);
                        pnlBaixa.Visible = false;
                        pnlReavaliacao.Visible = false;
                        CarregaOperacao();
                        CarregaEstadoConservacao(ddlEstadoConservacao);
                        CarregaPeriodoUtilizacaoFutura(ddlPeriodoUtilizacaoFutura);
                        CarregaPeriodoVidaUtilizado();

                        break;
                    }

                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel };
                        ControlarVisibilidadeControle(controles);

                        LimpaDadosReavaliacao();
                        pnlDadosReavaliacao.Visible = false;

                        if (!hdnbemId.Value.IsNullOrEmptyOrWhiteSpace())
                        {
                            PreencheDadosBem(Convert.ToInt32(hdnbemId.Value));
                            if (chkEfetuarBaixa.Checked)
                            {
                                pnlBaixa.Visible = true;
                                DesabilitaCamposBaixa();
                                chkEfetuarBaixa.Text = "Baixa Efetuada";
                                chkEfetuarBaixa.Enabled = false;
                            }
                        }
                        else
                        {
                            lblMensagem.Text = "Bem não identificado.";
                        }
                        DesabilitaCampos();
                        break;
                    }

                case TipoOperacao.Novo:
                    {
                        RN.Patrimonio.Moeda rnMoeda = new Techne.Lyceum.RN.Patrimonio.Moeda();
                        RN.Patrimonio.Entidades.Moeda moeda = new Techne.Lyceum.RN.Patrimonio.Entidades.Moeda();
                        var controles = new[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);
                        LimpaDados();
                        LimpaDadosBaixa();
                        LimpaDadosReavaliacao();
                        chkEfetuarBaixa.Visible = false;
                        pnlBaixa.Visible = false;
                        pnlReavaliacao.Visible = false;
                        lblNumero.Visible = false;
                        txtNumero.Visible = false;
                        lblQtdeReplicacao.Visible = true;
                        txtQtdeReplicacao.Visible = true;

                        if (rblOperacao.Items.FindByValue(((int)RN.Patrimonio.Operacao.EnumOperacao.Aquisicao).ToString()) != null)
                        {
                            rblOperacao.SelectedValue = ((int)RN.Patrimonio.Operacao.EnumOperacao.Aquisicao).ToString();
                            rblOperacao_SelectedIndexChanged(null, null);
                        }

                        moeda = rnMoeda.ObtemMoedaVigentePor(DateTime.Now);

                        if (moeda.MoedaId > 0)
                        {
                            txtMoeda.Text = moeda.Sigla + " - " + moeda.Descricao;
                            hdnMoedaId.Value = moeda.MoedaId.ToString();
                        }

                        break;
                    }


                case TipoOperacao.Consultar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel };

                        LimpaDados();
                        LimpaDadosBaixa();
                        LimpaDadosReavaliacao();
                        pnlBaixa.Visible = false;
                        pnlReavaliacao.Visible = true;
                        chkEfetuarBaixa.Enabled = false;
                        lblNumero.Visible = true;
                        txtNumero.Visible = true;
                        lblQtdeReplicacao.Visible = false;
                        txtQtdeReplicacao.Visible = false;

                        if (!hdnbemId.Value.IsNullOrEmptyOrWhiteSpace())
                        {
                            PreencheDadosBem(Convert.ToInt32(hdnbemId.Value));
                            if (chkEfetuarBaixa.Checked)
                            {
                                pnlBaixa.Visible = true;
                                DesabilitaCamposBaixa();
                                chkEfetuarBaixa.Text = "Baixa Efetuada";
                            }
                        }
                        else
                        {
                            lblMensagem.Text = "Bem não identificado.";
                        }
                        DesabilitaCampos();
                        ControlarVisibilidadeControle(controles);
                        break;
                    }
                case TipoOperacao.Alterar:
                    {
                        TransferenciaItem rnTransferenciaItem = new TransferenciaItem();
                        ImageButton[] controles;

                        LimpaDados();
                        LimpaDadosBaixa();
                        LimpaDadosReavaliacao();
                        pnlBaixa.Visible = false;
                        pnlReavaliacao.Visible = true;
                        lblNumero.Visible = true;
                        txtNumero.Visible = true;
                        lblQtdeReplicacao.Visible = false;
                        txtQtdeReplicacao.Visible = false;
                        btnSalvarReavaliacao.Visible = false;
                        btnEfetuarBaixa.Visible = false;

                        if (!hdnbemId.Value.IsNullOrEmptyOrWhiteSpace())
                        {
                            PreencheDadosBem(Convert.ToInt32(hdnbemId.Value));
                            if (chkEfetuarBaixa.Checked)
                            {
                                pnlBaixa.Visible = true;
                                DesabilitaCamposBaixa();
                                chkEfetuarBaixa.Text = "Baixa Efetuada";
                            }
                        }
                        else
                        {
                            lblMensagem.Text = "Bem não identificado.";
                        }

                        RN.Patrimonio.Movimentacao rnMovimentacao = new Movimentacao();
                        string setorAtualBem = rnMovimentacao.ObtemSetorVigentePor(Convert.ToInt32(hdnbemId.Value), DateTime.Now);
                        if (hdnSetor.Value != setorAtualBem)
                        {
                            DesabilitaCampos();
                            controles = new[] { btnCancel };
                            lblMensagem.Text = "Este não pode ser alterado pois não pertence a esta unidade.";
                            chkEfetuarBaixa.Enabled = false;
                            grdReavaliacao.Columns[0].Visible = false;
                        }
                        else
                        {
                            if (!rnPerfil.PossuiPerfilLiberacaoPatrimonioFinalizadoPor(User.Identity.Name) && !rnPeriodoLancamento.PossuiPeriodoLancamentoAbertoPor(dtDataIncorporacao.Date.Year, DateTime.Now.Date))
                            {
                                DesabilitaCampos();
                                controles = new[] { btnCancel };
                                lblMensagem.Text = "Este bem não pode ser alterado pois o período de lançamento não permite.";
                            }

                            DataTable transferencias = rnTransferenciaItem.ListaTransferenciaPendenteAbertaPor(Convert.ToInt32(hdnbemId.Value));
                            int pendente = 0;

                            if (transferencias.Rows.Count > 0)
                            {
                                DesabilitaCampos();
                                controles = new[] { btnCancel };
                                lblMensagem.Text = "Este bem não pode ser alterado pois possui transferência pendente ou aceita.";

                                foreach (DataRow item in transferencias.Rows)
                                {
                                    if (Convert.ToString(item["SITUACAO"]) == "Pendente")
                                    {
                                        pendente = Convert.ToInt32(item["QUANTIDADE"]);
                                    }
                                }

                                if (pendente > 0)
                                {
                                    chkEfetuarBaixa.Enabled = false;
                                    grdReavaliacao.Columns[0].Visible = false;
                                }
                                else
                                {
                                    btnSalvarReavaliacao.Visible = true;
                                    btnEfetuarBaixa.Visible = true;
                                }
                            }
                            else
                            {
                                controles = new[] { btnCancel, btnSalvar };
                                btnSalvarReavaliacao.Visible = true;
                                btnEfetuarBaixa.Visible = true;
                            }
                        }

                        ControlarVisibilidadeControle(controles);
                        break;
                    }
            }
        }

        protected void AcessoGrid()
        {
            if (grdReavaliacao != null)
            {
                HtmlInputImage img = (HtmlInputImage)grdReavaliacao.FindHeaderTemplateControl(grdReavaliacao.Columns[""], "btnNovoGrid");

                if (img != null)
                {
                    img.Visible = true;
                    img.Style.Add("visibility", "visible");

                    if (_tipoOperacao == TipoOperacao.Consultar || chkEfetuarBaixa.Checked)
                    {
                        img.Visible = false;
                        img.Style.Add("visibility", "hidden");
                    }
                }
            }

        }

        private void PreencheDadosBem(int bemId)
        {

            RN.DTOs.DadosBemPatrimonial dados = new Techne.Lyceum.RN.DTOs.DadosBemPatrimonial();
            RN.Patrimonio.Bem rnBem = new Techne.Lyceum.RN.Patrimonio.Bem();
            RN.Patrimonio.Moeda rnMoeda = new Techne.Lyceum.RN.Patrimonio.Moeda();
            RN.Patrimonio.Classificacao rnClassificacao = new Techne.Lyceum.RN.Patrimonio.Classificacao();
            RN.Patrimonio.Entidades.Moeda moeda = new Techne.Lyceum.RN.Patrimonio.Entidades.Moeda();

            dados = rnBem.ObtemDadosBemPatrimonialPor(Convert.ToInt32(hdnbemId.Value));

            if (dados.BemId > 0)
            {
                rblOperacao.SelectedValue = dados.OperacaoId.ToString();
                rblOperacao_SelectedIndexChanged(null, null);
                txtNumero.Text = dados.Numero.ToString().PadLeft(6, '0');
                tseClassificacao.DBValue = rnClassificacao.RetornaContaPor(dados.ClassificacaoId);
                txtDescricao.Text = !dados.Descricao.IsNullOrEmptyOrWhiteSpace() ? dados.Descricao : string.Empty;
                ddlEstadoConservacao.SelectedValue = dados.EstadoconservacaoId.ToString();

                if (dados.OperacaoId == (int)RN.Patrimonio.Operacao.EnumOperacao.Aquisicao || dados.OperacaoId == (int)RN.Patrimonio.Operacao.EnumOperacao.DoacaoItemNovo)
                {
                    if (ddlPeriodoVidaUtil.Items.FindByValue(dados.VidaFutura.ToString()) == null)
                    {
                        if (dados.VidaFutura > 10)
                        {
                            ddlPeriodoVidaUtil.SelectedValue = "10";
                        }
                    }
                    else
                    {
                        ddlPeriodoVidaUtil.SelectedValue = dados.VidaFutura.ToString();
                    }
                }
                else
                {
                    ddlPeriodoVidaUtil.SelectedValue = dados.VidaUtilizada.ToString();
                    ddlPeriodoUtilizacaoFutura.SelectedValue = dados.VidaFutura.ToString();
                }

                dtDataAquisicao.Date = dados.DataAquisicao;
                dtDataIncorporacao.Date = dados.DataIncorporacao;
                txtValor.Text = dados.ValorAtualizado.ToString("c");

                if (dados.MoedaId > 0)
                {
                    moeda = rnMoeda.ObtemDadosMoedaPor(dados.MoedaId);
                    txtMoeda.Text = moeda.Sigla + " - " + moeda.Descricao;
                }

                txtDocumentoHabil.Text = !dados.DocumentoHabil.IsNullOrEmptyOrWhiteSpace() ? dados.DocumentoHabil.Trim() : string.Empty;
                txtHistoricoOperacao.Text = !dados.Historico.IsNullOrEmptyOrWhiteSpace() ? dados.Historico.Trim() : string.Empty;

                if (dados.Baixa)
                {
                    chkEfetuarBaixa.Visible = true;
                    chkEfetuarBaixa.Checked = true;
                    chkEfetuarBaixa_CheckedChanged(null, null);
                    pnlBaixa.Visible = true;
                    ddlMotivoBaixa.SelectedValue = dados.MotivoBaixaId.ToString();
                    ddlMotivoBaixa_SelectedIndexChanged(null, null);
                    dtDataBaixa.Date = dados.DataBaixa.Value;
                    string[] inicial = dados.ProcessoBaixa.Split('-');
                    ddlProcessoPrefixo.SelectedValue = inicial[0].Length == 1 ? inicial[0] + "-03/" : inicial[0] + "-";
                    txtProcesso.Text = dados.ProcessoBaixa.Substring(ddlProcessoPrefixo.SelectedValue.Length, (dados.ProcessoBaixa.Length - (ddlProcessoPrefixo.SelectedValue.Length)));
                    txtBoletimOcorrencia.Text = dados.BoletimOcorrencia != null ? dados.BoletimOcorrencia : string.Empty;
                    txtCNPJ.Text = !dados.CnpjInstituicaoDestino.IsNullOrEmptyOrWhiteSpace() ? dados.CnpjInstituicaoDestino.AplicarMascaraCNPJ() : string.Empty;
                    txtPrefeituraInstituicao.Text = !dados.InstituicaoDestino.IsNullOrEmptyOrWhiteSpace() ? dados.InstituicaoDestino.Trim() : string.Empty;
                    txtObservacao.Text = !dados.JustificativaBaixa.IsNullOrEmptyOrWhiteSpace() ? dados.JustificativaBaixa.Trim() : string.Empty;
                }
            }

        }

        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (ImageButton botao in botoes)
            {
                botao.Visible = true;
            }
            ControlaAcesso(btnSalvar, AcaoControle.editar);
            ControlaAcesso(btnSalvarReavaliacao, AcaoControle.editar);
            ControlaAcesso(btnEfetuarBaixa, AcaoControle.editar);
        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnSalvar.Visible = false;
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string queryString = "Setor=" + lblUA.Text;
                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                Response.Redirect("ListarPatrimonio.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
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
                RN.Patrimonio.Bem rnBem = new Techne.Lyceum.RN.Patrimonio.Bem();
                RN.DTOs.DadosCadastroBemPatrimonial dadosBem = new DadosCadastroBemPatrimonial();
                RN.DTOs.DadosBaixaBemPatrimonial dadosBaixa = new DadosBaixaBemPatrimonial();
                string mensagem = string.Empty;

                dadosBem.BemId = !hdnbemId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnbemId.Value) : 0;
                dadosBem.Setor = hdnSetor.Value;
                dadosBem.OperacaoId = !rblOperacao.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(rblOperacao.SelectedValue) : -1;
                dadosBem.ClassificacaoId = !tseClassificacao.DBValue.IsNull && tseClassificacao.IsValidDBValue ? Convert.ToInt32(tseClassificacao["CLASSIFICACAOID"].ToString()) : -1;
                dadosBem.Descricao = !txtDescricao.Text.IsNullOrEmptyOrWhiteSpace() ? txtDescricao.Text.Trim() : null;
                dadosBem.DataAquisicao = !dtDataAquisicao.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataAquisicao.Date : DateTime.MinValue;
                dadosBem.DocumentoHabil = !txtDocumentoHabil.Text.IsNullOrEmptyOrWhiteSpace() ? txtDocumentoHabil.Text.Trim() : null;
                dadosBem.Historico = !txtHistoricoOperacao.Text.IsNullOrEmptyOrWhiteSpace() ? txtHistoricoOperacao.Text.Trim() : null;
                dadosBem.UsuarioId = User.Identity.Name;
                dadosBem.DataIncorporacao = !dtDataIncorporacao.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataIncorporacao.Date : DateTime.MinValue;
                dadosBem.EstadoconservacaoId = !ddlEstadoConservacao.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlEstadoConservacao.SelectedValue) : -1;

                if (_tipoOperacao == TipoOperacao.Novo)
                {
                    dadosBem.MoedaId = !hdnMoedaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnMoedaId.Value) : -1;
                    dadosBem.Quantidade = (!txtQtdeReplicacao.Text.IsNullOrEmptyOrWhiteSpace()) ? Convert.ToInt32(txtQtdeReplicacao.Text.Trim()) : -1;
                }
                else
                {
                    dadosBem.Numero = txtNumero.Text;
                    dadosBem.Quantidade = 1;
                }

                if (dadosBem.OperacaoId == (int)RN.Patrimonio.Operacao.EnumOperacao.Aquisicao || dadosBem.OperacaoId == (int)RN.Patrimonio.Operacao.EnumOperacao.DoacaoItemNovo)
                {
                    dadosBem.VidaUtil = !ddlPeriodoVidaUtil.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlPeriodoVidaUtil.SelectedValue) : -1;
                    dadosBem.VidaUtilizada = null;
                    dadosBem.VidaFutura = null;
                    dadosBem.Valor = !txtValor.Text.IsNullOrEmptyOrWhiteSpace() ? txtValor.Text.RetiraMascaraMonetaria() : -1;
                    dadosBem.ValorMercado = null;
                }
                else
                {
                    dadosBem.VidaUtil = null;
                    dadosBem.VidaUtilizada = !ddlPeriodoVidaUtil.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlPeriodoVidaUtil.SelectedValue) : -1;
                    dadosBem.VidaFutura = !ddlPeriodoUtilizacaoFutura.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlPeriodoUtilizacaoFutura.SelectedValue) : -1;
                    dadosBem.ValorMercado = !txtValor.Text.IsNullOrEmptyOrWhiteSpace() ? txtValor.Text.RetiraMascaraMonetaria() : -1;
                    dadosBem.Valor = 0;
                }

                validacao = rnBem.Valida(dadosBem, dadosBem.BemId == 0 ? true : false);

                if (validacao.Valido)
                {
                    if (dadosBem.BemId == 0)
                    {
                        rnBem.Insere(dadosBem);
                        hdnbemId.Value = dadosBem.BemId.ToString();

                        if (Convert.ToInt32(txtQtdeReplicacao.Text) == 1)
                        {
                            mensagem = "Operação realizada com sucesso. O Número de Idenficação desse Bem é " + dadosBem.Numero + ".";
                        }
                        else
                        {
                            mensagem = "Operação realizada com sucesso. Foram criados " + txtQtdeReplicacao.Text + " bens. Com número de inventário de  " + dadosBem.NumeroInicial + " até " + dadosBem.Numero + ".";
                        }
                    }
                    else
                    {
                        rnBem.Altera(dadosBem);
                        mensagem = "Bem atualizado com sucesso.";
                    }

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('" + mensagem + "');", true);
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

        protected void tseClassificacao_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                RN.Patrimonio.Classificacao rnClassificacao = new Techne.Lyceum.RN.Patrimonio.Classificacao();
                int vidaUtil = 0;

                if (!this.tseClassificacao.DBValue.IsNull)
                {
                    if (this.tseClassificacao.IsValidDBValue)
                    {
                        if (rblOperacao.SelectedValue == ((int)RN.Patrimonio.Operacao.EnumOperacao.Aquisicao).ToString() || rblOperacao.SelectedValue == ((int)RN.Patrimonio.Operacao.EnumOperacao.DoacaoItemNovo).ToString())
                        {
                            vidaUtil = rnClassificacao.RetornaVidaUtilVigentePor(Convert.ToInt32(tseClassificacao["CLASSIFICACAOID"].ToString()));

                            if (ddlPeriodoVidaUtil.Items.FindByValue(vidaUtil.ToString()) == null)
                            {
                                if (vidaUtil > 10)
                                {
                                    ddlPeriodoVidaUtil.SelectedValue = "10";
                                }
                            }
                            else
                            {
                                ddlPeriodoVidaUtil.SelectedValue = vidaUtil.ToString();
                            }
                        }
                    }
                    else
                    {
                        this.lblMensagem.Text = "Classificação não cadastrada.";
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar uma classificacao.";
                }
                updatePanel3.Update();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void rblOperacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pnlHistorico.Visible = false;
                txtHistoricoOperacao.Text = string.Empty;
                ddlPeriodoVidaUtil.Enabled = true;
                tseClassificacao.ResetValue();
                RetiraVisibilidadeControles();

                if (!rblOperacao.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    if (rblOperacao.SelectedValue == ((int)RN.Patrimonio.Operacao.EnumOperacao.DoacaoItemUsado).ToString() || rblOperacao.SelectedValue == ((int)RN.Patrimonio.Operacao.EnumOperacao.IncorporacaoSemRegistroAnterior).ToString())
                    {
                        lblVidaUtil.Text = "Período Vida Utilizado:*";
                        lblValor.Text = "Valor de Mercado:*";
                        pnlHistorico.Visible = true;
                        lblEstadoConservacao.Visible = true;
                        ddlEstadoConservacao.Visible = true;
                        lblPeriodoUtilizacaoFutura.Visible = true;
                        ddlPeriodoUtilizacaoFutura.Visible = true;
                    }

                    if (rblOperacao.SelectedValue == ((int)RN.Patrimonio.Operacao.EnumOperacao.Aquisicao).ToString() || rblOperacao.SelectedValue == ((int)RN.Patrimonio.Operacao.EnumOperacao.DoacaoItemNovo).ToString())
                    {
                        lblVidaUtil.Text = "Período de Vida Útil:*";
                        lblValor.Text = "Valor Unitário:*";
                        CarregaPeriodoUtilizacaoFutura(ddlPeriodoVidaUtil);
                        ddlPeriodoVidaUtil.Enabled = false;
                        lblDataAquisicao.Visible = true;
                        dtDataAquisicao.Visible = true;
                    }

                    if (_tipoOperacao != TipoOperacao.Novo)
                    {
                        lblValor.Text = "Valor Atualizado:*";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void chkEfetuarBaixa_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LimpaDadosBaixa();
                LimpaDadosReavaliacao();
                pnlDadosReavaliacao.Visible = false;
                pnlBaixa.Visible = false;
                lblBoletim.Visible = false;
                txtBoletimOcorrencia.Visible = false;
                lblCNPJ.Visible = false;
                txtCNPJ.Visible = false;
                lblPrefeitura.Visible = false;
                txtPrefeituraInstituicao.Visible = false;

                if (chkEfetuarBaixa.Checked)
                {
                    CarregaMotivoBaixa();
                    pnlBaixa.Visible = true;
                    btnEfetuarBaixa.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimpaDadosBaixa()
        {
            txtBoletimOcorrencia.Text = string.Empty;
            txtProcesso.Text = string.Empty;
            txtCNPJ.Text = string.Empty;
            txtPrefeituraInstituicao.Text = string.Empty;
            txtObservacao.Text = string.Empty;
            ddlProcessoPrefixo.ClearSelection();
            dtDataBaixa.Text = string.Empty;
        }

        private void DesabilitaCamposBaixa()
        {
            ddlMotivoBaixa.Enabled = false;
            txtProcesso.Enabled = false;
            txtBoletimOcorrencia.Enabled = false;
            txtCNPJ.Enabled = false;
            txtPrefeituraInstituicao.Enabled = false;
            txtObservacao.Enabled = false;
            btnEfetuarBaixa.Visible = false;
            ddlProcessoPrefixo.Enabled = false;
            dtDataBaixa.Enabled = false;

        }

        protected void ddlMotivoBaixa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtProcesso.Text = string.Empty;
                lblBoletim.Visible = false;
                txtBoletimOcorrencia.Visible = false;
                lblCNPJ.Visible = false;
                txtCNPJ.Visible = false;
                lblPrefeitura.Visible = false;
                txtPrefeituraInstituicao.Visible = false;
                txtBoletimOcorrencia.Text = string.Empty;
                txtCNPJ.Text = string.Empty;
                txtPrefeituraInstituicao.Text = string.Empty;
                txtObservacao.Text = string.Empty;

                if (!ddlMotivoBaixa.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    if (ddlMotivoBaixa.SelectedValue == "2")
                    {
                        lblBoletim.Visible = true;
                        txtBoletimOcorrencia.Visible = true;
                    }
                    if (ddlMotivoBaixa.SelectedValue == "3")
                    {
                        lblCNPJ.Visible = true;
                        txtCNPJ.Visible = true;
                        lblPrefeitura.Visible = true;
                        txtPrefeituraInstituicao.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnEfetuarBaixa_Click(object sender, EventArgs e)
        {
            try
            {

                ValidacaoDados validacao = new ValidacaoDados();
                RN.Patrimonio.Bem rnBem = new Techne.Lyceum.RN.Patrimonio.Bem();
                RN.DTOs.DadosBaixaBemPatrimonial dadosBaixa = new DadosBaixaBemPatrimonial();
                string mensagem = string.Empty;

                dadosBaixa.Baixa = chkEfetuarBaixa.Checked;
                dadosBaixa.BemId = !hdnbemId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnbemId.Value) : -1;

                if (dadosBaixa.Baixa)
                {
                    dadosBaixa.MotivoBaixaId = !ddlMotivoBaixa.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlMotivoBaixa.SelectedValue) : -1;
                    dadosBaixa.ProcessoBaixa = !txtProcesso.Text.IsNullOrEmptyOrWhiteSpace() ? txtProcesso.Text.Trim() : null;
                    dadosBaixa.BoletimOcorrencia = !txtBoletimOcorrencia.Text.IsNullOrEmptyOrWhiteSpace() ? txtBoletimOcorrencia.Text.Trim() : null;
                    dadosBaixa.CnpjInstituicaoDestino = !txtCNPJ.Text.IsNullOrEmptyOrWhiteSpace() ? txtCNPJ.Text.Trim().RetirarMascaraCNPJ() : null;
                    dadosBaixa.InstituicaoDestino = !txtPrefeituraInstituicao.Text.IsNullOrEmptyOrWhiteSpace() ? txtPrefeituraInstituicao.Text.Trim() : null;
                    dadosBaixa.JustificativaBaixa = !txtObservacao.Text.IsNullOrEmptyOrWhiteSpace() ? txtObservacao.Text.Trim() : null;
                    dadosBaixa.DataBaixa = !dtDataBaixa.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataBaixa.Date : DateTime.MinValue;
                    dadosBaixa.UsuarioId = User.Identity.Name;
                    dadosBaixa.PrefixoProcesso = !ddlProcessoPrefixo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlProcessoPrefixo.SelectedValue : null;
                }

                validacao = rnBem.ValidaBaixa(dadosBaixa, dtDataAquisicao.Date, dtDataIncorporacao.Date, hdnSetor.Value);

                if (validacao.Valido)
                {
                    rnBem.Baixa(dadosBaixa);


                    if (dadosBaixa.MotivoBaixaId == (int)RN.Patrimonio.MotivoBaixa.EnumMotivoBaixa.Subtraido)
                    {
                        mensagem = "Baixa realizada com sucesso. <br />Será aberta uma sindicância para averiguação.";
                    }
                    else
                    {
                        mensagem = "Baixa realizada com sucesso.";
                    }

                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", string.Format("alert('{0}');", mensagem.Replace("<br />", "\\n")), true);


                    lblMensagem.Text = mensagem.Replace(Environment.NewLine, "<br />");
                    lblMensagemBaixa.Text = lblMensagem.Text;

                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    lblMensagemBaixa.Text = lblMensagem.Text;
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagemBaixa.Text = lblMensagem.Text;
            }
            updatePanel3.Update();
        }

        private void LimpaDadosReavaliacao()
        {
            rblBemInservivel.ClearSelection();
            ddlEstadoConservacaoNaoInservivel.ClearSelection();
            ddlVidaUtilNaoInservivel.ClearSelection();
            txtValorNaoInservivel.Text = string.Empty;
            ddlPrefixoProcessoReavaliacao.ClearSelection();
        }

        protected void rblBemInservivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlEstadoConservacaoNaoInservivel.ClearSelection();
                ddlVidaUtilNaoInservivel.ClearSelection();
                txtValorNaoInservivel.Text = string.Empty;
                pnlBemNaoInservivel.Visible = false;
                pnlBemInservivel.Visible = false;
                ddlPrefixoProcessoReavaliacao.ClearSelection();

                if (rblBemInservivel.SelectedValue == "0")
                {
                    pnlBemNaoInservivel.Visible = true;
                    CarregaEstadoConservacao(ddlEstadoConservacaoNaoInservivel);
                    CarregaPeriodoUtilizacaoFutura(ddlVidaUtilNaoInservivel);
                }
                else
                {
                    pnlBemInservivel.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagemReavaliacao.Text = lblMensagem.Text;
            }
        }

        protected void btnSalvarReavaliacao_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.DTOs.DadosBemReavaliacao dadosReavaliacao = new DadosBemReavaliacao();
                RN.Patrimonio.Reavaliacao rnReavaliacao = new Techne.Lyceum.RN.Patrimonio.Reavaliacao();
                RN.Patrimonio.Moeda rnMoeda = new Techne.Lyceum.RN.Patrimonio.Moeda();

                dadosReavaliacao.BemId = !hdnbemId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnbemId.Value) : -1;
                dadosReavaliacao.MoedaId = rnMoeda.ObtemMoedaVigentePor(DateTime.Now).MoedaId;
                dadosReavaliacao.VidaAdicional = !ddlVidaUtilNaoInservivel.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlVidaUtilNaoInservivel.SelectedValue) : -1;
                dadosReavaliacao.ValorMercado = !txtValorNaoInservivel.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(txtValorNaoInservivel.Text.Trim()) : -1;
                dadosReavaliacao.EstadoconservacaoId = !ddlEstadoConservacaoNaoInservivel.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlEstadoConservacaoNaoInservivel.SelectedValue) : -1;
                dadosReavaliacao.Inservivel = !rblBemInservivel.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblBemInservivel.SelectedValue == "1" ? true : false) : (bool?)null;

                if (!txtProcessoReavaliacao.Text.IsNullOrEmptyOrWhiteSpace() && !ddlPrefixoProcessoReavaliacao.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    dadosReavaliacao.Processo = ddlPrefixoProcessoReavaliacao.SelectedValue + txtProcessoReavaliacao.Text.Trim();
                }
                else
                {
                    dadosReavaliacao.Processo = null;
                }
                dadosReavaliacao.DataReavaliacao = DateTime.Now;
                dadosReavaliacao.UsuarioId = User.Identity.Name;
                dadosReavaliacao.UltimoValorAtualizado = !txtValor.Text.IsNullOrEmptyOrWhiteSpace() ? decimal.Parse(txtValor.Text, NumberStyles.Currency) : -1;
                dadosReavaliacao.ClassificacaoId = !tseClassificacao.DBValue.IsNull && tseClassificacao.IsValidDBValue ? Convert.ToInt32(tseClassificacao["CLASSIFICACAOID"].ToString()) : -1;

                if (rblOperacao.SelectedValue == ((int)RN.Patrimonio.Operacao.EnumOperacao.Aquisicao).ToString() || rblOperacao.SelectedValue == ((int)RN.Patrimonio.Operacao.EnumOperacao.DoacaoItemNovo).ToString())
                {
                    dadosReavaliacao.DataAquisicao = !dtDataAquisicao.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataAquisicao.Date : DateTime.MinValue;
                }
                else
                {
                    dadosReavaliacao.DataAquisicao = !dtDataIncorporacao.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataIncorporacao.Date : DateTime.MinValue;
                }

                validacao = rnReavaliacao.ValidaCadastro(dadosReavaliacao, hdnSetor.Value);

                if (validacao.Valido)
                {
                    rnReavaliacao.Insere(dadosReavaliacao);

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Operação realizada com sucesso.');", true);
                    lblMensagem.Text = "Operação realizada com sucesso.";
                    lblMensagemReavaliacao.Text = lblMensagem.Text;

                    LimpaDadosReavaliacao();
                    pnlDadosReavaliacao.Visible = false;
                    odsReavaliacao.DataBind();
                    grdReavaliacao.DataBind();

                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();

                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    lblMensagemReavaliacao.Text = lblMensagem.Text;
                    return;
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagemReavaliacao.Text = lblMensagem.Text;
            }
            updatePanel3.Update();
        }

        protected void HabilitaPnlReavaliacao(object sender, EventArgs e)
        {
            LimpaDadosReavaliacao();
            pnlDadosReavaliacao.Visible = true;
        }

        protected void txtValorNaoInservivel_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            RN.Util.Utils.Moeda(ref txt);
        }
    }
}
