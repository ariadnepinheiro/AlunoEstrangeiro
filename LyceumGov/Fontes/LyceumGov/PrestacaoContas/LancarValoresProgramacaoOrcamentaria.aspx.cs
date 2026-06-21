using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxTabControl;
using Techne.Lyceum.RN.Util;
using Techne.Controls;
using System.Data;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;
using Seeduc.Infra.Data;
using Seeduc.Infra.Helpers;
using Techne.Lyceum.RN.PrestacaoContas.DTOs;
using System.Data.SqlTypes;
using Techne.Web;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [
     NavUrl("~/PrestacaoContas/LancarValoresProgramacaoOrcamentaria.aspx"),
     ControlText("LancarValoresProgramacaoOrcamentaria"),
     Title("Lançamento Repasses das Parcelas PO")
    ]
    public partial class LancarValoresProgramacaoOrcamentaria : TPage
    {
        public object ListaItemPlanilhaOrcamentaria(object itemPlanilhaOrcamentariaId)
        {
            RN.PrestacaoContas.LancamentoRepasse rnLancamentoRepasse = new RN.PrestacaoContas.LancamentoRepasse();
            DataTable dados = new DataTable();

            if (itemPlanilhaOrcamentariaId == null)
            {
                return null;
            }

            if (((Techne.Internal.Nullable)itemPlanilhaOrcamentariaId).IsNull == false)
            {
                dados = rnLancamentoRepasse.ListaItemPlanilhaOrcamentaria(Convert.ToInt32(itemPlanilhaOrcamentariaId));
            }

            return dados;
        }

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

        protected bool VerificaCheck(object valor)
        {
            if (valor is DBNull)
            {
                return false;
            }
            if (valor is string)
            {
                return (string)valor == "S";
            }
            if (valor is bool)
            {
                return (bool)valor;
            }
            if (valor is int)
            {
                return (bool)valor;
            }

            return false;
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

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdItemPlanilhaOrcamentaria);
            ControlaAcesso(grdItemPlanilhaOrcamentaria, AcaoControle.novo, "btnNovo");
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdItemPlanilhaOrcamentaria, "");
        }

        protected void CarregaDados()
        {
            LimpaTela();
            RN.PrestacaoContas.LancamentoRepasse rnLancamentoRepasse = new RN.PrestacaoContas.LancamentoRepasse();
            DataTable dadosSomaValoresLancRepItemPlaOrc = new DataTable();

            try
            {
                int itemPlanilhaOrcamentariaId = Convert.ToInt32(tseItemPlanilha.DBValue);
                dadosSomaValoresLancRepItemPlaOrc = rnLancamentoRepasse.ListaSomaValorLancRepasseItemPlanilhaOrc(itemPlanilhaOrcamentariaId);

                lblTotalLancRepasse.Text = dadosSomaValoresLancRepItemPlaOrc.Rows[0]["SOMALANCREPASSE"].ToString();
                lblTotalItemPlaOrc.Text = dadosSomaValoresLancRepItemPlaOrc.Rows[0]["SOMAITEMPLAORC"].ToString();
                lblMesAnoReferenciaResult.Text = dadosSomaValoresLancRepItemPlaOrc.Rows[0]["MES"].ToString() + "/" + dadosSomaValoresLancRepItemPlaOrc.Rows[0]["ANO"].ToString();
                lblRegiaoFinanceiraResult.Text = dadosSomaValoresLancRepItemPlaOrc.Rows[0]["REGIAOFINANCEIRA"].ToString();
                lblFonteRecursosResult.Text = dadosSomaValoresLancRepItemPlaOrc.Rows[0]["CODIGOSEFAZ"].ToString() + "/" + dadosSomaValoresLancRepItemPlaOrc.Rows[0]["FONTE"].ToString();

                //Verifica se já tem numero do processo
                if (Convert.ToString(dadosSomaValoresLancRepItemPlaOrc.Rows[0]["NUMEROPROCESSOREPASSE"]).IsNullOrEmptyOrWhiteSpace())
                {
                    lblNumeroProcesso.Text = string.Empty;
                    lblNumeroProcesso.Visible = false;
                    lblNumeroProcessoTexto.Visible = false;
                }
                else
                {
                    lblNumeroProcesso.Text = Convert.ToString(dadosSomaValoresLancRepItemPlaOrc.Rows[0]["NUMEROPROCESSOREPASSE"]);
                    lblNumeroProcesso.Visible = true;
                    lblNumeroProcessoTexto.Visible = true;
                }

                odsItemPlanilhaOrcamentaria.Select();
                grdItemPlanilhaOrcamentaria.DataBind();
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
                        ImageButton[] controles = new ImageButton[] { };
                        ControlarVisibilidadeControle(null, null);
                        LimpaTela();
                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        ControlarVisibilidadeControle(null, null);
                        LimpaCamposRepasse();
                        grdItemPlanilhaOrcamentaria.DataBind();
                        CarregaDados();
                        pnlNovo.Visible = false;
                        pnlGrid.Visible = true;

                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        ControlarVisibilidadeControle(null, null);
                        LimpaTela();
                        CarregaDados();
                        pnlGrid.Visible = true;

                        break;
                    }
                case TipoOperacao.Alterar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles, null);
                        LimpaCamposRepasse();
                        pnlNovo.Visible = true;

                        break;
                    }
                case TipoOperacao.Novo:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles, null);
                        LimpaCamposRepasse();
                        pnlNovo.Visible = true;

                        //Verifica se já existe algum repasse
                        if (grdItemPlanilhaOrcamentaria.VisibleRowCount > 0)
                        {
                            //Busca numero do repasse para utilizar
                            txtNumProcessoRepasse.Text = lblNumeroProcesso.Text;
                        }

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
        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnSalvar.Visible = false;
        }

        private void LimpaTela()
        {
            pnlGrid.Visible = false;
            pnlNovo.Visible = false;
            lblMesAnoReferenciaResult.Text = string.Empty;
            lblRegiaoFinanceiraResult.Text = string.Empty;
            lblFonteRecursosResult.Text = string.Empty;
            lblNumeroProcesso.Text = string.Empty;
            lblTotalLancRepasse.Text = string.Empty;
            lblTotalItemPlaOrc.Text = string.Empty;
            LimpaCamposRepasse();
        }

        private void LimpaCamposRepasse()
        {
            txtNumProcessoRepasse.Text = string.Empty;
            tseUnidadeResponsavel.ResetValue();
            tseContaCorrente.ResetValue();
            txtValor.Text = string.Empty;
            hdnLancamentoRepasseId.Value = string.Empty;
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

        protected void btnSalva_Click(object sender, EventArgs e)
        {
            RN.PrestacaoContas.LancamentoRepasse rnLancamentoRepasse = new Techne.Lyceum.RN.PrestacaoContas.LancamentoRepasse();
            RN.PrestacaoContas.Entidades.LancamentoRepasse lancamentoRepasse = new Techne.Lyceum.RN.PrestacaoContas.Entidades.LancamentoRepasse();
            ValidacaoDados validacao = new ValidacaoDados();
            string mensagem = string.Empty;

            try
            {
                lancamentoRepasse.UsuarioId = User.Identity.Name;
                lancamentoRepasse.Censo = tseUnidadeResponsavel.DBValue != DBNull.Value ? Convert.ToString(tseUnidadeResponsavel.DBValue) : null;
                lancamentoRepasse.ContaCorrenteId = tseContaCorrente.DBValue != DBNull.Value ? Convert.ToInt32(tseContaCorrente.DBValue) : 0;
                lancamentoRepasse.Valor = !String.IsNullOrEmpty(txtValor.Text) ? Convert.ToDecimal(txtValor.Text) : 0;
                lancamentoRepasse.ItemPlanilhaOrcamentariaId = tseItemPlanilha.DBValue != DBNull.Value ? Convert.ToInt32(tseItemPlanilha.DBValue) : 0;
                lancamentoRepasse.LancamentoRepasseId = !String.IsNullOrEmpty(hdnLancamentoRepasseId.Value) ? Convert.ToInt32(hdnLancamentoRepasseId.Value) : 0;
                lancamentoRepasse.NumeroProcessoRepasse = !String.IsNullOrEmpty(txtNumProcessoRepasse.Text) ? Convert.ToString(txtNumProcessoRepasse.Text) : null;

                //Verifica se é cadastro
                bool cadastro = lancamentoRepasse.LancamentoRepasseId <= 0;

                validacao = rnLancamentoRepasse.Valida(lancamentoRepasse, cadastro);
                if (validacao.Valido)
                {
                    if (cadastro)
                    {
                        rnLancamentoRepasse.Insere(lancamentoRepasse);
                        mensagem = "Lancamento Repasse inserido com sucesso.";
                    }
                    else
                    {
                        rnLancamentoRepasse.Atualiza(lancamentoRepasse);
                        mensagem = "Lancamento Repasse atualizado com sucesso.";
                    }

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('" + mensagem + ".');", true);
                    lblMensagem.Text = mensagem;
                    odsItemPlanilhaOrcamentaria.Select();
                    grdItemPlanilhaOrcamentaria.DataBind();

                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();
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

        protected void tseAgencia_Changed(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseProgramacaoOrcamentaria_Changed(object sender, EventArgs args)
        {
            LimpaTela();
            tseItemPlanilha.ResetValue();
        }

        protected void tseItemPlanilha_Changed(object sender, EventArgs args)
        {
      
            LimpaTela();
        }

        protected void tseUnidadeResponsavel_Changed(object sender, EventArgs args)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            if (!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue)
            {
                tseContaCorrente.SqlWhere = " CENSO = " + tseUnidadeResponsavel.DBValue.ToString();
            }
        }

        protected void btnNovo_Command(object sender, CommandEventArgs e)
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

        protected void grdItemPlanilhaOrcamentaria_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdItemPlanilhaOrcamentaria);
            ControlaAcesso(grdItemPlanilhaOrcamentaria, AcaoControle.novo, "btnNovo");
        }

        protected void grdItemPlanilhaOrcamentaria_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            string contaCorrenteId = grdItemPlanilhaOrcamentaria.GetRowValues(e.VisibleIndex, "CONTACORRENTEID").ToString();
            string censo = grdItemPlanilhaOrcamentaria.GetRowValues(e.VisibleIndex, "CENSO").ToString();
            string contaCorrente = grdItemPlanilhaOrcamentaria.GetRowValues(e.VisibleIndex, "CONTA").ToString();
            string numeroProcesso = grdItemPlanilhaOrcamentaria.GetRowValues(e.VisibleIndex, "NUMEROPROCESSOREPASSE").ToString();
            string LancamentoRepasseId = grdItemPlanilhaOrcamentaria.GetRowValues(e.VisibleIndex, "LANCAMENTOREPASSEID").ToString();
            string valor = grdItemPlanilhaOrcamentaria.GetRowValues(e.VisibleIndex, "VALOR").ToString();

            if (e.ButtonID == "btnEditarCustom")
            {
                _tipoOperacao = TipoOperacao.Alterar;
                ControlarTipoOperacao();
                tseUnidadeResponsavel.DBValue = censo;
                tseUnidadeResponsavel_Changed(null, null);
                tseContaCorrente.DBValue = contaCorrenteId;
                txtValor.Text = string.Format("{0:N2}", valor);                 
                txtNumProcessoRepasse.Text = numeroProcesso;
                hdnLancamentoRepasseId.Value = LancamentoRepasseId;

            }
            if (e.ButtonID == "btnDeletar")
            {
                hdnLancamentoRepasseId.Value = LancamentoRepasseId;
                popup.ShowOnPageLoad = true;
            }
        }

        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.LancamentoRepasse rnLancamentoRepasse = new Techne.Lyceum.RN.PrestacaoContas.LancamentoRepasse();

            try
            {
                int lancamentoRepasseId = hdnLancamentoRepasseId.Value.IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToInt32(hdnLancamentoRepasseId.Value);

                validacao = rnLancamentoRepasse.ValidaRemocao(lancamentoRepasseId);
                if (validacao.Valido)
                {
                    rnLancamentoRepasse.Remove(lancamentoRepasseId);
                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();
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

        protected void grdItemPlanilhaOrcamentaria_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;

            if (e.CellType == GridViewTableCommandCellType.Filter)
            {
                e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                return;
            }

            string ws = Convert.ToString(grdItemPlanilhaOrcamentaria.GetRowValues(e.VisibleIndex, "WSREPASSESEFAZID"));
            string acao = Convert.ToString(grdItemPlanilhaOrcamentaria.GetRowValues(e.VisibleIndex, "ACAO"));

            if (e.ButtonID == "btnDeletar")
            {
                e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                if (ws != null && acao != "Aprovado")
                {
                    if (ws.ToString() == "" && Permission.AllowDelete)
                    {
                        e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.True;
                    }
                }
            }

            if (e.ButtonID == "btnEditarCustom")
            {
                e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                if (acao != "Aprovado")
                {
                    e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.True;
                }
            }
        }

        protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (!tseProgramacaoOrcamentaria.IsValidDBValue || tseProgramacaoOrcamentaria.DBValue.IsNull ||
                    !tseItemPlanilha.IsValidDBValue || tseItemPlanilha.DBValue.IsNull)
                {
                    lblMensagem.Text = "Para efetuar a busca é necessário selecionar todos os filtros.";
                    pnlGrid.Visible = false;
                }
                else
                {
                    _tipoOperacao = TipoOperacao.Consultar;
                    ControlarTipoOperacao();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
