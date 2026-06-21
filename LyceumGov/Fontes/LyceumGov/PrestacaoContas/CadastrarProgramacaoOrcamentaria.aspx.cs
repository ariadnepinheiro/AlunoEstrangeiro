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
using System.Globalization;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [
     NavUrl("~/PrestacaoContas/CadastrarProgramacaoOrcamentaria.aspx"),
     ControlText("CadastrarProgramacaoOrcamentaria"),
     Title("Cadastrar Programação Orçamentária")
    ]
    public partial class CadastrarProgramacaoOrcamentaria : TPage
    {
        public object ListaFonteRecurso()
        {
            RN.PrestacaoContas.FonteRecurso rnFonteRecurso = new RN.PrestacaoContas.FonteRecurso();
            var dados = rnFonteRecurso.ListaAtivo();
            return dados;
        }

        public object ListaItemPlanilhaOrcamentaria(object planilhaOrcamentariaId)
        {
            RN.PrestacaoContas.ItemPlanilhaOrcamentaria rnItemProgramacaoOrcamentaria = new RN.PrestacaoContas.ItemPlanilhaOrcamentaria();

            if (planilhaOrcamentariaId.ToString() == "")
            {
                return null;
            }

            var dados = rnItemProgramacaoOrcamentaria.ListaItemPlanilhaOrcamentaria(planilhaOrcamentariaId.ToString());
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
            Desativar,
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

        protected void dpdAnoRef_SelectedIndexChanged(object sender, EventArgs e)
        {
            // dpdNumProcesso.Items.Clear();
            pnProgamacaoOrcamentaria.Visible = false;

            if (!dpdAnoRef.SelectedValue.IsNullOrEmptyOrWhiteSpace())
            {
                //CarregaNumeroProcesso(Convert.ToInt32(dpdAnoRef.SelectedValue));
                tseProcesso.ResetValue();
                tseProcesso.SqlWhere = " ano = '" + dpdAnoRef.SelectedValue + "'";
            }
            tseProcesso.DataBind();

        }

        private void CarregaAno()
        {
            RN.PeriodoLetivo rnPeriodoLetivo = new RN.PeriodoLetivo();
            dpdAnoRef.Items.Clear();
            dpdAnoRef.DataSource = rnPeriodoLetivo.ListaAnos(false);
            dpdAnoRef.DataBind();
            dpdAnoRef.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        private void CarregaNumeroProcesso(int ano)
        {
            RN.PrestacaoContas.PlanilhaOrcamentaria rnPlanilhaOrcamentaria = new RN.PrestacaoContas.PlanilhaOrcamentaria();
            tseProcesso.ResetValue();
            //dpdNumProcesso.Items.Clear();
            // dpdNumProcesso.DataSource = rnPlanilhaOrcamentaria.CarregaNumProcesso(ano);
            tseProcesso.SqlWhere = " ano = '" + dpdAnoRef.SelectedValue + "'";
            tseProcesso.DataBind();
            //dpdNumProcesso.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        private void CarregaAnoCadastro()
        {
            RN.PeriodoLetivo rnPeriodoLetivo = new RN.PeriodoLetivo();
            ddlAnoCadastro.Items.Clear();
            ddlAnoCadastro.DataSource = rnPeriodoLetivo.ListaAnos(false);
            ddlAnoCadastro.DataBind();
            ddlAnoCadastro.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }



        private void CarregaRegiaoFinanceira()
        {
            RN.GestaoRede.RegiaoFinanceira rnRegiaoFinanceira = new RN.GestaoRede.RegiaoFinanceira();
            ddlRegiaoFinanceira.Items.Clear();
            ddlRegiaoFinanceira.DataSource = rnRegiaoFinanceira.Lista();
            ddlRegiaoFinanceira.DataBind();
            ddlRegiaoFinanceira.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RN.PrestacaoContas.ItemPlanilhaOrcamentaria rnItemProgramacaoOrcamentaria = new RN.PrestacaoContas.ItemPlanilhaOrcamentaria();

            try
            {
                lblMensagem.Text = string.Empty;

                if (pnProgamacaoOrcamentaria.Visible && (PlanilhaOrcamentariaId.Text != "0" && PlanilhaOrcamentariaId.Text != ""))
                {
                    //Bsuca valor dos itens
                    lblTotalParcelas.Text = rnItemProgramacaoOrcamentaria.ListaSomaValorItemPlanilhaOrcamentaria(Convert.ToInt32(PlanilhaOrcamentariaId.Text)).ToString("c", CultureInfo.CurrentCulture);
                }

                if (!IsPostBack)
                {
                    CarregaAno();
                    CarregaRegiaoFinanceira();
                    CarregaAnoCadastro();

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
            TituloGrid(grdItemPlanilhaOrcamentaria, "");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdItemPlanilhaOrcamentaria);
        }

        protected void HabilitaCampos(bool habilita)
        {
            txtNumProcesso.Enabled = habilita;
            txtDescricaoComplDespesa.Enabled = habilita;
            ddlRegiaoFinanceira.Enabled = habilita;
            ddlAnoCadastro.Enabled = habilita;

            if (habilita)
            {
                tsePlanoTrabalho.Mode = ControlMode.Edit;
                tseProgramaTrabalho.Mode = ControlMode.Edit;
                tseNaturezaDespeza.Mode = ControlMode.Edit;
            }
            else
            {
                tsePlanoTrabalho.Mode = ControlMode.View;
                tseProgramaTrabalho.Mode = ControlMode.View;
                tseNaturezaDespeza.Mode = ControlMode.View;
            }
        }

        private void ControlarTipoOperacao()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { };
                        controles = new[] { btnNovo };
                        ControlarVisibilidadeControle(controles, null);
                        tseProcesso.DBValue = "";
                        PlanilhaOrcamentariaId.Text = "0";
                        tseProcesso.Enabled = true;
                        dpdAnoRef.Enabled = true;
                        LimpaDados();
                        HabilitaCampos(false);
                        pnProgamacaoOrcamentaria.Visible = false;

                        pcPlanilhaOrcamentaria.ActiveTabIndex = 0;
                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { };
                        controles = new[] { btnNovo };
                        ControlarVisibilidadeControle(controles, null);
                        tseProcesso.Enabled = true;
                        // dpdNumProcesso.Enabled = true;
                        dpdAnoRef.Enabled = true;
                        HabilitaCampos(false);
                        pnProgamacaoOrcamentaria.Visible = true;
                        pcPlanilhaOrcamentaria.TabPages.FindByName("tabParcela").ClientEnabled = true;
                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        RN.PrestacaoContas.MandatoAae rnMandatoAEE = new Techne.Lyceum.RN.PrestacaoContas.MandatoAae();
                        RN.PrestacaoContas.Entidades.MandatoAae fornecedor = new Techne.Lyceum.RN.PrestacaoContas.Entidades.MandatoAae();

                        ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar };
                        ControlarVisibilidadeControle(controles, null);
                        pcPlanilhaOrcamentaria.Visible = true;
                        pcPlanilhaOrcamentaria.ActiveTabIndex = 0;
                        tseProcesso.Enabled = true;
                        dpdAnoRef.Enabled = true;
                        LimpaDados();
                        Carrega(tseProcesso.DBValue.ToString());
                        pcPlanilhaOrcamentaria.TabPages.FindByName("tabParcela").ClientEnabled = true;
                        HabilitaCampos(false);
                        pnProgamacaoOrcamentaria.Visible = true;
                        break;
                    }
                case TipoOperacao.Alterar:
                    {

                        if (tseProcesso.DBValue.ToString() == "")
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Número do Processo não preenchido.');", true);
                            lblMensagem.Text = "Número do Processo não preenchido";
                            tseProcesso.Focus();
                            return;
                        }
                        else
                        {
                            ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                            ControlarVisibilidadeControle(controles, null);
                            tseProcesso.Enabled = false;
                            dpdAnoRef.Enabled = false;
                            HabilitaCampos(true);

                            RN.PrestacaoContas.AnalisePlanilhaOrcamentaria rnAnalisePlanilhaOrcamentaria = new Techne.Lyceum.RN.PrestacaoContas.AnalisePlanilhaOrcamentaria();
                            if (rnAnalisePlanilhaOrcamentaria.EhAprovadaPor(Convert.ToInt32(tseProcesso["PLANILHAORCAMENTARIAID"])))
                            {
                                ddlAnoCadastro.Enabled = false;
                            }

                            pnProgamacaoOrcamentaria.Visible = true;
                            pcPlanilhaOrcamentaria.TabPages.FindByName("tabParcela").ClientEnabled = true;
                        }
                        break;
                    }
                case TipoOperacao.Novo:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles, null);
                        // dpdNumProcesso.ClearSelection();
                        // dpdAnoRef.ClearSelection();
                        PlanilhaOrcamentariaId.Text = "0";
                        tseProcesso.Enabled = false;
                        // dpdNumProcesso.Enabled = false;
                        dpdAnoRef.Enabled = false;
                        pcPlanilhaOrcamentaria.TabPages.FindByName("tabParcela").ClientEnabled = false;
                        LimpaDados();
                        HabilitaCampos(true);
                        pnProgamacaoOrcamentaria.Visible = true;
                        break;
                    }
                case TipoOperacao.Cancelar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar };
                        ControlarVisibilidadeControle(controles, null);
                        tseProcesso.Enabled = true;
                        //dpdNumProcesso.Enabled = true;
                        dpdAnoRef.Enabled = true;
                        pcPlanilhaOrcamentaria.TabPages.FindByName("tabParcela").ClientEnabled = true;
                        LimpaDados();
                        HabilitaCampos(false);
                        pnProgamacaoOrcamentaria.Visible = false;
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

            ControlaAcesso(btnEditar, Techne.Lyceum.Net.TPage.AcaoControle.editar);
            ControlaAcesso(btnNovo, Techne.Lyceum.Net.TPage.AcaoControle.novo);
        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnEditar.Visible = false;
            btnNovo.Visible = false;
            btnSalvar.Visible = false;
        }

        private void LimpaDados()
        {
            lblMotivo.Visible = false;
            lblMotivoRetorno.Visible = false;
            lblStatusRetorno.Text = string.Empty;
            lblMotivoRetorno.Text = string.Empty;
            txtNumProcesso.Text = string.Empty;
            PlanilhaOrcamentariaId.Text = "0";
            txtDescricaoComplDespesa.Text = string.Empty;
            tseProgramaTrabalho.ResetValue();
            tsePlanoTrabalho.ResetValue();
            tseNaturezaDespeza.ResetValue();
            ddlRegiaoFinanceira.ClearSelection();
            ddlAnoCadastro.ClearSelection();
            lblTotalParcelas.Text = string.Empty;
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
            //if (lblStatusRetorno.Text == "")
            // {
            try
            {
                _tipoOperacao = TipoOperacao.Alterar;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

            //}
            // else {
            //      var mensagem = "Programação Orçamentária fechada.";
            //      Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('" + mensagem + ".');", true);
            //  }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            RN.PrestacaoContas.PlanilhaOrcamentaria rnPlanilhaOrcamentaria = new Techne.Lyceum.RN.PrestacaoContas.PlanilhaOrcamentaria();
            RN.PrestacaoContas.Entidades.PlanilhaOrcamentaria planilhaOrcamentaria = new Techne.Lyceum.RN.PrestacaoContas.Entidades.PlanilhaOrcamentaria();
            ValidacaoDados validacao = new ValidacaoDados();

            string mensagem = string.Empty;
            try
            {
                planilhaOrcamentaria.Processo = !String.IsNullOrEmpty(txtNumProcesso.Text) ? txtNumProcesso.Text : null;
                planilhaOrcamentaria.Descricao = !String.IsNullOrEmpty(txtDescricaoComplDespesa.Text) ? txtDescricaoComplDespesa.Text : null;
                planilhaOrcamentaria.PlanoTrabalhoId = tsePlanoTrabalho.DBValue != null && tsePlanoTrabalho.DBValue != DBNull.Value ? Convert.ToInt32(tsePlanoTrabalho.DBValue) : 0;
                planilhaOrcamentaria.NaturezaDespesaId = tseNaturezaDespeza.DBValue != null && tseNaturezaDespeza.DBValue != DBNull.Value ? Convert.ToInt32(tseNaturezaDespeza.DBValue) : 0;
                planilhaOrcamentaria.RegiaoFinanceiraId = !ddlRegiaoFinanceira.SelectedValue.IsNullOrEmptyOrWhiteSpace() && Convert.ToInt32(ddlRegiaoFinanceira.SelectedValue) != 0 ? Convert.ToInt32(ddlRegiaoFinanceira.SelectedValue) : 0;
                planilhaOrcamentaria.UsuarioId = User.Identity.Name;
                planilhaOrcamentaria.Ano = !ddlAnoCadastro.SelectedValue.IsNullOrEmptyOrWhiteSpace() && Convert.ToInt32(ddlAnoCadastro.SelectedValue) != 0 ? Convert.ToInt32(ddlAnoCadastro.SelectedValue) : 0;
                if (txtNumProcesso.Text != "")
                {
                    planilhaOrcamentaria.PlanilhaOrcamentariaId = rnPlanilhaOrcamentaria.PesquisaIDPlanilha(txtNumProcesso.Text);
                    PlanilhaOrcamentariaId.Text = planilhaOrcamentaria.PlanilhaOrcamentariaId.ToString();
                }
                else
                    planilhaOrcamentaria.PlanilhaOrcamentariaId = 0;
                planilhaOrcamentaria.ProgramaTrabalhoId = tseProgramaTrabalho.DBValue != null && tseProgramaTrabalho.DBValue != DBNull.Value ? Convert.ToInt32(tseProgramaTrabalho.DBValue) : 0;


                if (_tipoOperacao == TipoOperacao.Novo)
                {
                    validacao = rnPlanilhaOrcamentaria.Valida(planilhaOrcamentaria, true);

                    if (validacao.Valido)
                    {
                        rnPlanilhaOrcamentaria.Insere(planilhaOrcamentaria);
                        dpdAnoRef.SelectedValue = planilhaOrcamentaria.Ano.ToString();
                        CarregaNumeroProcesso(Convert.ToInt32(dpdAnoRef.SelectedValue));
                        tseProcesso.DBValue = planilhaOrcamentaria.Processo.ToString();

                        PlanilhaOrcamentariaId.Text = rnPlanilhaOrcamentaria.PesquisaIDPlanilha(tseProcesso.DBValue.ToString()).ToString();
                        mensagem = "Programação Orçamentária inserida com sucesso.";

                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('" + mensagem + ".');", true);
                        lblMensagem.Text = mensagem;

                        _tipoOperacao = TipoOperacao.Consultar;
                        ControlarTipoOperacao();
                    }
                    else
                    {
                        lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        return;
                    }
                }
                else
                {
                    validacao = rnPlanilhaOrcamentaria.ValidaEdicao(planilhaOrcamentaria, false);

                    if (validacao.Valido)
                    {
                        rnPlanilhaOrcamentaria.Atualiza(planilhaOrcamentaria);
                        mensagem = "Programação Orçamentária atualizada com sucesso.";
                    }
                    else
                    {
                        lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        return;
                    }

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('" + mensagem + ".');", true);
                    lblMensagem.Text = mensagem;

                    _tipoOperacao = TipoOperacao.Consultar;
                    ControlarTipoOperacao();
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void pcPlanilhaOrcamentaria_TabClick(object source, TabControlCancelEventArgs e)
        {
            lblMensagem.Text = string.Empty;

            if (e.Tab.Name == "tabGeral")
            {
                _tipoOperacao = TipoOperacao.Consultar;
                ControlarTipoOperacao();
                grdItemPlanilhaOrcamentaria.DataBind();
            }
        }

        public void Insert(object REFERENCIA, object FONTERECURSOID, object VALOR, object DESCRICAORETORNOREFERENCIA) { }
        public void Update(object REFERENCIA, object FONTERECURSOID, object VALOR, object DESCRICAORETORNOREFERENCIA, object ITEMPLANILHAORCAMENTARIAID) { }
        public void Delete(object ITEMPLANILHAORCAMENTARIAID) { }

        protected void grdItemPlanilhaOrcamentaria_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.ItemPlanilhaOrcamentaria itemPlanilhaOrcamentaria = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ItemPlanilhaOrcamentaria();
            RN.PrestacaoContas.ItemPlanilhaOrcamentaria rnItemPlanilhaOrcamentaria = new Techne.Lyceum.RN.PrestacaoContas.ItemPlanilhaOrcamentaria();

            itemPlanilhaOrcamentaria.ItemPlanilhaOrcamentariaId = Convert.ToInt32(e.NewValues["ITEMPLANILHAORCAMENTARIAID"]);
            itemPlanilhaOrcamentaria.PlanilhaOrcamentariaId = Convert.ToInt32(PlanilhaOrcamentariaId.Text);
            itemPlanilhaOrcamentaria.Referencia = Convert.ToInt32(e.NewValues["REFERENCIA"]);
            itemPlanilhaOrcamentaria.FonteRecursoId = Convert.ToInt32(e.NewValues["FONTERECURSOID"]);
            itemPlanilhaOrcamentaria.Valor = Convert.ToDecimal(e.NewValues["VALOR"]);
            itemPlanilhaOrcamentaria.RetornoReferencia = "E";
            itemPlanilhaOrcamentaria.UsuarioId = User.Identity.Name;

            validacao = rnItemPlanilhaOrcamentaria.Valida(itemPlanilhaOrcamentaria, false);

            if (validacao.Valido)
            {
                rnItemPlanilhaOrcamentaria.Insere(itemPlanilhaOrcamentaria);
                lblTotalParcelas.Text = rnItemPlanilhaOrcamentaria.ListaSomaValorItemPlanilhaOrcamentaria(Convert.ToInt32(PlanilhaOrcamentariaId.Text)).ToString("c", CultureInfo.CurrentCulture);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdItemPlanilhaOrcamentaria.DataBind();
        }

        protected void grdItemPlanilhaOrcamentaria_OnRowDeleted(object sender, DevExpress.Web.Data.ASPxDataDeletedEventArgs e)
        {
            RN.PrestacaoContas.ItemPlanilhaOrcamentaria rnItemPlanilhaOrcamentaria = new Techne.Lyceum.RN.PrestacaoContas.ItemPlanilhaOrcamentaria();
            lblTotalParcelas.Text = rnItemPlanilhaOrcamentaria.ListaSomaValorItemPlanilhaOrcamentaria(Convert.ToInt32(PlanilhaOrcamentariaId.Text)).ToString("c", CultureInfo.CurrentCulture);

            if (!string.IsNullOrEmpty(lblTotalParcelas.Text) && lblTotalParcelas.Text != "0")
            {
                ((ASPxGridView)sender).JSProperties["cpAtualizar"] = lblTotalParcelas.Text;
            }
            else
            {
                ((ASPxGridView)sender).JSProperties["cpAtualizar"] = string.Empty;
            }
        }

        protected void grdItemPlanilhaOrcamentaria_OnRowInserted(object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e)
        {
            RN.PrestacaoContas.ItemPlanilhaOrcamentaria rnItemPlanilhaOrcamentaria = new Techne.Lyceum.RN.PrestacaoContas.ItemPlanilhaOrcamentaria();
            lblTotalParcelas.Text = rnItemPlanilhaOrcamentaria.ListaSomaValorItemPlanilhaOrcamentaria(Convert.ToInt32(PlanilhaOrcamentariaId.Text)).ToString("c", CultureInfo.CurrentCulture);

            if (!string.IsNullOrEmpty(lblTotalParcelas.Text) && lblTotalParcelas.Text != "0")
            {
                ((ASPxGridView)sender).JSProperties["cpAtualizar"] = lblTotalParcelas.Text;
            }
            else
            {
                ((ASPxGridView)sender).JSProperties["cpAtualizar"] = string.Empty;
            }
        }


        protected void grdItemPlanilhaOrcamentaria_OnRowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
        {
            RN.PrestacaoContas.ItemPlanilhaOrcamentaria rnItemPlanilhaOrcamentaria = new Techne.Lyceum.RN.PrestacaoContas.ItemPlanilhaOrcamentaria();
            lblTotalParcelas.Text = rnItemPlanilhaOrcamentaria.ListaSomaValorItemPlanilhaOrcamentaria(Convert.ToInt32(PlanilhaOrcamentariaId.Text)).ToString("c", CultureInfo.CurrentCulture);

            if (!string.IsNullOrEmpty(lblTotalParcelas.Text) && lblTotalParcelas.Text != "0")
            {
                ((ASPxGridView)sender).JSProperties["cpAtualizar"] = lblTotalParcelas.Text;
            }
            else
            {
                ((ASPxGridView)sender).JSProperties["cpAtualizar"] = string.Empty;
            }
        }
        protected void grdItemPlanilhaOrcamentaria_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.ItemPlanilhaOrcamentaria itemPlanilhaOrcamentaria = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ItemPlanilhaOrcamentaria();
            RN.PrestacaoContas.ItemPlanilhaOrcamentaria rnItemPlanilhaOrcamentaria = new Techne.Lyceum.RN.PrestacaoContas.ItemPlanilhaOrcamentaria();

            itemPlanilhaOrcamentaria.ItemPlanilhaOrcamentariaId = Convert.ToInt32(e.Keys["ITEMPLANILHAORCAMENTARIAID"]);
            itemPlanilhaOrcamentaria.PlanilhaOrcamentariaId = Convert.ToInt32(PlanilhaOrcamentariaId.Text);
            itemPlanilhaOrcamentaria.Referencia = Convert.ToInt32(e.NewValues["REFERENCIA"]);
            itemPlanilhaOrcamentaria.FonteRecursoId = Convert.ToInt32(e.NewValues["FONTERECURSOID"]);
            itemPlanilhaOrcamentaria.Valor = Convert.ToDecimal(e.NewValues["VALOR"]);
            itemPlanilhaOrcamentaria.UsuarioId = User.Identity.Name;

            validacao = rnItemPlanilhaOrcamentaria.Valida(itemPlanilhaOrcamentaria, false);

            if (validacao.Valido)
            {
                rnItemPlanilhaOrcamentaria.Atualiza(itemPlanilhaOrcamentaria);
                lblTotalParcelas.Text = rnItemPlanilhaOrcamentaria.ListaSomaValorItemPlanilhaOrcamentaria(Convert.ToInt32(PlanilhaOrcamentariaId.Text)).ToString("c", CultureInfo.CurrentCulture);
                if (!string.IsNullOrEmpty(lblTotalParcelas.Text) && lblTotalParcelas.Text != "")
                {
                    ((ASPxGridView)sender).JSProperties["cpAtualizar"] = lblTotalParcelas.Text;
                }
                else
                {
                    ((ASPxGridView)sender).JSProperties["cpAtualizar"] = string.Empty;
                }
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

        protected void grdItemPlanilhaOrcamentaria_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            var status = (string)grdItemPlanilhaOrcamentaria.GetRowValues(e.VisibleIndex, "RETORNOREFERENCIA");

            if (!string.IsNullOrEmpty(status)
                && status == "F")
            {
                if (e.ButtonType == ColumnCommandButtonType.Delete)
                {
                    e.Visible = false;
                }

                if (e.ButtonType == ColumnCommandButtonType.Edit)
                {
                    e.Visible = false;
                }
            }
        }

        protected void grdItemPlanilhaOrcamentaria_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.ItemPlanilhaOrcamentaria rnItemPlanilhaOrcamentaria = new RN.PrestacaoContas.ItemPlanilhaOrcamentaria();
            int itemPlanilhaOrcamentariaId = Convert.ToInt32(e.Keys["ITEMPLANILHAORCAMENTARIAID"]);
            int planilhaOrcamentariaId = Convert.ToInt32(PlanilhaOrcamentariaId.Text);

            validacao = rnItemPlanilhaOrcamentaria.ValidaRemocao(itemPlanilhaOrcamentariaId, planilhaOrcamentariaId);

            if (validacao.Valido)
            {
                rnItemPlanilhaOrcamentaria.Remove(itemPlanilhaOrcamentariaId, planilhaOrcamentariaId);
                grdItemPlanilhaOrcamentaria.DataBind();
                lblTotalParcelas.Text = rnItemPlanilhaOrcamentaria.ListaSomaValorItemPlanilhaOrcamentaria(Convert.ToInt32(PlanilhaOrcamentariaId.Text)).ToString("c", CultureInfo.CurrentCulture);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            pnProgamacaoOrcamentaria.Visible = false;
            LimpaDados();

            try
            {
                if (dpdAnoRef.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    lblMensagem.Text = "O campo obrigatório Ano Referência não foi preenchido ";
                    return;
                }

                if (tseProcesso.DBValue.IsNull)
                {
                    lblMensagem.Text = "O campo obrigatório Número Processo não foi preenchido ";
                    return;
                }

                _tipoOperacao = TipoOperacao.Consultar;
                ControlarTipoOperacao();

                grdItemPlanilhaOrcamentaria.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Carrega(String planilhaOrcamentariaId)
        {
            RN.PrestacaoContas.PlanilhaOrcamentaria rnPlanilhaOrcamentaria = new Techne.Lyceum.RN.PrestacaoContas.PlanilhaOrcamentaria();
            RN.PrestacaoContas.AnalisePlanilhaOrcamentaria rnAnalisePlanilhaOrcamentaria = new RN.PrestacaoContas.AnalisePlanilhaOrcamentaria();
            RN.PrestacaoContas.ItemPlanilhaOrcamentaria rnItemProgramacaoOrcamentaria = new RN.PrestacaoContas.ItemPlanilhaOrcamentaria();
            RN.PrestacaoContas.MotivoReprovacaoPlanilhaOrcamentaria rnMotivoReprovacaoPlanilhaOrcamentaria = new Techne.Lyceum.RN.PrestacaoContas.MotivoReprovacaoPlanilhaOrcamentaria();
            DataTable dt = new DataTable();

            try
            {
                dt = rnPlanilhaOrcamentaria.ListaPlanilhaOrcamentariaPor(planilhaOrcamentariaId);

                txtNumProcesso.Text = dt.Rows[0]["PROCESSO"].ToString();
                txtDescricaoComplDespesa.Text = dt.Rows[0]["DESCRICAO"].ToString();
                tseProgramaTrabalho.DBValue = dt.Rows[0]["PROGRAMATRABALHOID"].ToString();
                tsePlanoTrabalho.DBValue = dt.Rows[0]["PLANOTRABALHOID"].ToString();
                tseNaturezaDespeza.DBValue = dt.Rows[0]["NATUREZADESPESAID"].ToString();
                ddlRegiaoFinanceira.SelectedValue = dt.Rows[0]["REGIAOFINANCEIRAID"].ToString();
                ddlAnoCadastro.SelectedValue = dt.Rows[0]["ANO"].ToString();
                PlanilhaOrcamentariaId.Text = dt.Rows[0]["PLANILHAORCAMENTARIAID"].ToString();
                //Bsuca valor dos itens
                lblTotalParcelas.Text = rnItemProgramacaoOrcamentaria.ListaSomaValorItemPlanilhaOrcamentaria(Convert.ToInt32(PlanilhaOrcamentariaId.Text)).ToString("c", CultureInfo.CurrentCulture);
                //Busca Ultima analise
                RN.PrestacaoContas.Entidades.AnalisePlanilhaOrcamentaria analise = new Techne.Lyceum.RN.PrestacaoContas.Entidades.AnalisePlanilhaOrcamentaria();
                analise = rnAnalisePlanilhaOrcamentaria.ObtemPor(Convert.ToInt32(PlanilhaOrcamentariaId.Text));

                if (analise.AnalisePlanilhaOrcamentariaId > 0)
                {
                    lblStatus.Visible = true;
                    lblStatusRetorno.Visible = true;
                    lblStatusRetorno.Text = string.Empty;

                    if (analise.Aprovada)
                    {
                        lblStatusRetorno.Text = "Aprovado";
                        lblMotivo.Visible = false;
                        lblMotivoRetorno.Visible = false;
                        lblMotivoRetorno.Text = string.Empty;
                    }
                    else
                    {
                        lblStatusRetorno.Text = "Reprovado";
                        lblMotivo.Visible = true;
                        lblMotivoRetorno.Visible = true;
                        lblMotivoRetorno.Text = rnMotivoReprovacaoPlanilhaOrcamentaria.ObtemDescricaoPor(Convert.ToInt32(analise.MotivoReprovacaoPlanilhaOrcamentariaId));
                    }
                }
                else
                {
                    lblStatus.Visible = false;
                    lblStatusRetorno.Visible = false;
                    lblStatusRetorno.Text = string.Empty;
                    lblMotivo.Visible = false;
                    lblMotivoRetorno.Visible = false;
                    lblMotivoRetorno.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
