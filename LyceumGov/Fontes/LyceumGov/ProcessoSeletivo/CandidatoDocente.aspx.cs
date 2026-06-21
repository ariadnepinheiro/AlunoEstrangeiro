using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using Techne.Controls;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using Techne.Lyceum.RN.Entidades;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using Techne.Lyceum.RN.ContratoTemporario;
using System.Collections;
using System.Data.SqlClient;
using System.Linq;
using DevExpress.Utils;

namespace Techne.Lyceum.Net.ProcessoSeletivo
{
    [NavUrl("~/ProcessoSeletivo/CandidatoDocente.aspx"),
    ControlText("CandidatoDocente"),
    Title("Ficha de Inscrição"),]
    public partial class CandidatoDocente : TPage
    {
        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {

        }
        #endregion
        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        #region Propriedades e Enumeradores
        public enum TipoOperacao
        {
            Novo,
            Excluir,
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
                        return (TipoOperacao)ViewState["_tipoOperacao"];
                }

                return TipoOperacao.Inicial;
            }
            set { ViewState["_tipoOperacao"] = value; }
        }

        //private string AREA_INTEGRADA_DOCII = "039";
        #endregion

        #region Eventos da Página

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Page.Header.Controls.AddAt(0, new HtmlMeta { HttpEquiv = "X-UA-Compatible", Content = "IE=8" });
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdExperiencia);
            ControlaAcesso(grdProcessoSeletivo);
            ControlaAcesso(grdTitulacao);
            ControlaAcesso(grdDisciplinasHabilitacao);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["listFrom"] = listFrom.Items;
            Session["listTo"] = listTo.Items;

            if (!IsPostBack)
            {
                //para a primeira vez que a página é carregada o tipo de operação será inicial
                Page.Header.Controls.AddAt(0, new HtmlMeta { HttpEquiv = "X-UA-Compatible", Content = "IE=8" });
                _tipoOperacao = TipoOperacao.Inicial;
                ControlarTipoOperacao();
                pcCandidatoDocente.ActiveTabIndex = 0;
                hdnTitulacao.Value = "0";
                hdnExperiencia.Value = "0";
            }

            txtTotalPont.Text = SomarPontuacao();

            if (!tseConcursoBusca.DBValue.IsNull && !tseCandidatoBusca.DBValue.IsNull)
            {
                CarregarDadosContratoTemporario();
            }

            if (_tipoOperacao.Equals(TipoOperacao.Novo) || _tipoOperacao.Equals(TipoOperacao.Alterar))
            {
                tseConcursoBusca.Mode = ControlMode.View;
                tseCandidatoBusca.Mode = ControlMode.View;
            }
            else
            {
                tseConcursoBusca.Mode = ControlMode.Edit;
                tseCandidatoBusca.Mode = ControlMode.Edit;
            }

            lblMensagem.Text = string.Empty; //LIMPAR ERROS
            TituloGrid();

            if (RN.CandidatoDocente.PeriodoInscricaoEncerrado(tseConcurso.DBValue.ToString()))
            {
                grdTitulacao.Columns[0].Visible = false;
                grdExperiencia.Columns[0].Visible = false;
            }
            else
            {
                grdTitulacao.Columns[0].Visible = true;
                grdExperiencia.Columns[0].Visible = true;
            }

            IDictionary<string, string> pares = new Dictionary<string, string>();

            //if (Session["SScandidato"] == null)
            //{
            //    pares.Add("candidato", tseCandidatoBusca.DBValue.ToString());
            //    pares.Add("concurso", tseConcursoBusca.DBValue.ToString());
            //}
            //else
            //{

            //}
            pares.Add("candidato", tseCandidatoBusca.DBValue.ToString());
            pares.Add("concurso", tseConcursoBusca.DBValue.ToString());
            btnImprimir.Attributes.Add("onclick", @"javascript:window.open('../Relatorio/Relatorios.aspx?report=rsconcursodocente&grp=processoseletivo&" + TPage.CodificaQueryString(pares) + "','', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes'); return false;");
        }


        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdExperiencia, "Experiência");
            TituloGrid(grdTitulacao, "Titulação");
            TituloGrid(grdProcessoSeletivo, "Processo Seletivo");
            TituloGrid(grdDisciplinasHabilitacao, "Disciplinas Habilitação");

            if (!string.IsNullOrEmpty(QueryStringDecodificada["usuario"]))
            {
                tseConcurso.SqlWhere = string.Empty;
            }

            listFrom.Items.Assign(Session["listFrom"] as IAssignableCollection);
            listTo.Items.Assign(Session["listTo"] as IAssignableCollection);

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }
        #endregion

        #region Eventos

        protected void ddlRGTipoPessoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRGTipoPessoa.SelectedValue == "RG")
            {
                rfvEstadoRG.Enabled = true;
                lblRG_UF.Text = "Estado*: ";
                lblRG_UF.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                rfvDataExp.Enabled = true;
            }
            else
            {
                rfvEstadoRG.Enabled = false;
                lblRG_UF.Text = "Estado: ";
                lblRG_UF.Style.Add(HtmlTextWriterStyle.FontWeight, "normal");
                rfvDataExp.Enabled = false;
            }
        }

        protected void AtualizarDadosCota(object sender, EventArgs e)
        {
            QueryTable dadosDrop = null;

            if (cmbNecessidadeEspecial.SelectedItem.Text != "Não possui.")
            {
                if (ddlCorRaca.SelectedValue == "4" || ddlCorRaca.SelectedValue == "5")
                {
                    dadosDrop = RN.ContratoTemporario.Cota.ListarCotasPorEtniaIndioOuNegraOuNenhuma();
                    PreencheDropdownList(ddlCotas, dadosDrop, "");
                }
                else
                {
                    dadosDrop = RN.ContratoTemporario.Cota.ListarCotasPorDeficienteFisico();
                    PreencheDropdownList(ddlCotas, dadosDrop, "");
                }
            }
            else
            {
                if (ddlCorRaca.SelectedValue == "4" || ddlCorRaca.SelectedValue == "5")
                {
                    dadosDrop = RN.ContratoTemporario.Cota.ListarCotasPorEtniaIndioOuNegra();
                    PreencheDropdownList(ddlCotas, dadosDrop, "");
                }
                else
                {
                    dadosDrop = RN.ContratoTemporario.Cota.ListarCotasPorNecessidadeEtnia();
                    PreencheDropdownList(ddlCotas, dadosDrop, "");
                }
            }
        }

        protected void tseMunicipio_Changed(object sender, EventArgs args)
        {
            if (!tseMunicipio.DBValue.IsNull && tseMunicipio.IsValidDBValue)
            {
                txtEstado.Value = Convert.ToString(tseMunicipio["uf_sigla"]);
            }
            else
            {
                txtEstado.Value = string.Empty;
            }
        }

        protected void tseConcursoBusca_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            btnImprimir.Visible = false;
            btnValidarInscricao.Visible = false;
            lblMensagem.Text = "Favor informar um processo seletivo e um número de inscrição.";
            ImageButton[] controles = new ImageButton[] { btnNovo };
            ControlarVisibilidadeControle(controles);
            pcCandidatoDocente.Visible = false;
            tseCandidatoBusca.ResetValue();
        }

        protected void tseCandidatoBusca_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            if (!tseCandidatoBusca.DBValue.IsNull && tseCandidatoBusca.IsValidDBValue)
            {
                lblMensagem.Text = string.Empty;
                _tipoOperacao = TipoOperacao.Consultar;
            }
            else
            {
                lblMensagem.Text = "Favor informar um processo seletivo e um número de inscrição.";
                _tipoOperacao = TipoOperacao.Inicial;
            }

            ControlarTipoOperacao();
        }

        private void HabilitaDesabilitaAbaDisciplinaHabilitacoes()
        {
            bool PossuiStatusAguardandoOuAguardandoAvaliacaoCGP = false;

            if ((tseConcursoBusca.Value != null && tseCandidatoBusca.Value != null) || _tipoOperacao == TipoOperacao.Sucesso && txtStatusCandidato.Text == "23")
            {
                PossuiStatusAguardandoOuAguardandoAvaliacaoCGP = RN.CandidatoDocente.PossuiStatusAguardandoOuAguardandoAvaliacaoCGP(
                    tseConcurso.Value.ToString(), txtCandidato.Text
                );
            }

            grdDisciplinasHabilitacao.Settings.ShowFilterRow = PossuiStatusAguardandoOuAguardandoAvaliacaoCGP;

            if (PossuiStatusAguardandoOuAguardandoAvaliacaoCGP)
                grdDisciplinasHabilitacao.DataBind();
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Inicial;

            ControlarTipoOperacao();
        }

        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Novo;
            ControlarTipoOperacao();
        }

        protected void btnEditar_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Alterar;
            ControlarTipoOperacao();
        }

        protected void btnExcluir_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Excluir;
            ControlarTipoOperacao();
        }

        protected void btnProximoClick(object sender, EventArgs e)
        {
            if (grdTitulacao.VisibleRowCount == 0)
            {
                lblMensagem.Visible = true;
                MsgBox("É obrigatória a seleção de uma titulação", this.Page);
                pcCandidatoDocente.ActiveTabIndex = 1;
                return;
            }
        }

        protected void btnValidarInscricao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                RN.CandidatoDocente.ValidaFichaDeInscricao(tseConcurso.Value.ToString(), txtCandidato.Text);
                lblMensagem.Text = "Ficha de inscrição validada com sucesso.";
                btnValidarInscricao.Visible = false;
                grdDisciplinasHabilitacao.Enabled = false;
                //atualizar status do processo seletivo

                txtStatusCandidato.Text = "1";

                odsProcessoSeletivo.Select();
                odsProcessoSeletivo.DataBind();
                grdProcessoSeletivo.DataBind();
                ControlaAcesso(grdProcessoSeletivo);

                IDictionary<string, string> pares = new Dictionary<string, string>();
                pares.Add("candidato", Session["SScandidato"].ToString()); //dtCandidatoDocente.Candidato.ToString());
                pares.Add("concurso", Session["SSconcurso"].ToString());// dtCandidatoDocente.Concurso.ToString());
                btnImprimir.Attributes.Add("onclick", @"javascript:window.open('../Relatorio/Relatorios.aspx?report=rsconcursodocente&grp=processoseletivo&" + TPage.CodificaQueryString(pares) + "','', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes'); return false;");
                btnImprimir.Visible = true;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            RN.CandidatoDocente rnCandidatoDocente = new Techne.Lyceum.RN.CandidatoDocente();

            #region Validações Server

            #region Validações dos Campos Obrigatórios

            if (String.IsNullOrEmpty(cmbCargo.SelectedValue))
            {
                lblMensagem.Visible = true;
                lblMensagem.Text = "Função é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (String.IsNullOrEmpty(tseConcurso.DBValue.ToString()))
            {
                lblMensagem.Visible = true;
                lblMensagem.Text = "Processo Seletivo é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;

            }

            //Rafaela - 12/07/2013
            if (String.IsNullOrEmpty(tseMunicipioProc.DBValue.ToString()))
            {
                lblMensagem.Visible = true;
                lblMensagem.Text = "Municipio do Processo é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }

            if (string.IsNullOrEmpty(tseRegional.DBValue.ToString()))
            {
                lblMensagem.Visible = true;
                lblMensagem.Text = "Regional é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                tseRegional.Visible = true;
                return;
            }

            if (String.IsNullOrEmpty(txtNomeCompleto.Text))
            {
                lblMensagem.Visible = true;
                lblMensagem.Text = "Nome Completo é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (String.IsNullOrEmpty(dtDataNasc.Text))
            {
                lblMensagem.Visible = true;
                lblMensagem.Text = "Data de Nascimento é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (String.IsNullOrEmpty(rblSexo.SelectedValue))
            {
                lblMensagem.Visible = true;
                lblMensagem.Text = "Sexo é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (ddlCorRaca.SelectedIndex == 0)
            {
                lblMensagem.Visible = true;
                lblMensagem.Text = "Etnia é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (String.IsNullOrEmpty(txtNomeMae.Text))
            {
                lblMensagem.Visible = true;
                lblMensagem.Text = "Nome da Mãe é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            //if (String.IsNullOrEmpty(txtNomePai.Text))
            //{
            //    lblMensagem.Visible = true;
            //    lblMensagem.Text = "Nome do Pai é campo obrigatório.";
            //    pcCandidatoDocente.ActiveTabIndex = 0;
            //    return;
            //}
            if (ddlEst_Civil.SelectedIndex == 0)
            {
                lblMensagem.Visible = true;
                lblMensagem.Text = "Estado Civil é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (String.IsNullOrEmpty(cmbNecessidadeEspecial.SelectedItem.Text) || cmbNecessidadeEspecial.SelectedValue == "")
            {
                lblMensagem.Visible = true;
                lblMensagem.Text = "Necessidade Especial é campo obrigatório";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }

            if (String.IsNullOrEmpty(tseNaturalidade.DBValue.ToString()))
            {
                lblMensagem.Visible = true;
                lblMensagem.Text = "Naturalidade é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (ddlCotas.SelectedIndex == 0)
            {
                lblMensagem.Visible = true;
                lblMensagem.Text = lblMensagem.Text = "Cota é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (String.IsNullOrEmpty(txtCep.Text))
            {
                lblMensagem.Visible = true;
                lblMensagem.Text = "CEP é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (String.IsNullOrEmpty(tseMunicipio.DBValue.ToString()))
            {
                lblMensagem.Visible = true;
                lblMensagem.Text = "Município é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (String.IsNullOrEmpty(txtEstado.Value))
            {
                lblMensagem.Visible = true;
                lblMensagem.Text = "Estado é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (String.IsNullOrEmpty(txtEndereco.Text))
            {
                lblMensagem.Visible = true;
                lblMensagem.Text = "Endereço é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (String.IsNullOrEmpty(txtEndNum.Text))
            {
                lblMensagem.Visible = true;
                lblMensagem.Text = "Número é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (String.IsNullOrEmpty(txtBairro.Text))
            {
                lblMensagem.Visible = true;
                lblMensagem.Text = "Bairro é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (String.IsNullOrEmpty(txtCPF.Text))
            {
                lblMensagem.Visible = true;
                lblMensagem.Text = "CPF é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 3;
                return;
            }

            #endregion

            #region Validações de Campos Gerais
            if (!String.IsNullOrEmpty(dtDataNasc.Text))
            {
                if (dtDataNasc.Date > DateTime.Now)
                {
                    lblMensagem.Visible = true;
                    lblMensagem.Text = "A Data de Nascimento tem que ser menor que a data de hoje.";
                    pcCandidatoDocente.ActiveTabIndex = 0;
                    return;
                }
                else if (dtDataNasc.Date.AddYears(18) > DateTime.Now || dtDataNasc.Date.AddYears(80) < DateTime.Now)
                {
                    lblMensagem.Visible = true;
                    lblMensagem.Text = "Data de Nascimento: O candidato docente não pode ser menor de dezoito anos ou possuir mais de 80 anos.";
                    pcCandidatoDocente.ActiveTabIndex = 0;
                    return;
                }
            }
            //if (!String.IsNullOrEmpty(txtFone.Text))
            //{
            //    if (!RN.Validacao.ValidaTelefoneComDDD(txtFone.Text.RetirarMascaraTelefone()))
            //    {
            //        lblMensagem.Visible = true;
            //        lblMensagem.Text = "O Telefone é inválido.";
            //        pcCandidatoDocente.ActiveTabIndex = 0;
            //        return;
            //    }
            //}
            //if (!String.IsNullOrEmpty(txtCelular.Text))
            //{
            //    if (!RN.Validacao.ValidaCelularComDDD(txtCelular.Text.RetirarMascaraTelefone()))
            //    {
            //        lblMensagem.Visible = true;
            //        lblMensagem.Text = "O Celular é inválido.";
            //        pcCandidatoDocente.ActiveTabIndex = 0;
            //        return;
            //    }
            //}
            if (!String.IsNullOrEmpty(txtEmail.Text))
            {
                if (!RN.Validacao.ValidaEmail(txtEmail.Text))
                {
                    lblMensagem.Visible = true;
                    lblMensagem.Text = "O Email é inválido.";
                    pcCandidatoDocente.ActiveTabIndex = 0;
                    return;
                }
            }
            if (!String.IsNullOrEmpty(txtRGNum.Text))
            {
                if (txtRGNum.Text.Length < 5 || txtRGNum.Text.Length > 15)
                {
                    lblMensagem.Visible = true;
                    lblMensagem.Text = "Número do documento deve possuir entre 5 e 15 dígitos.";
                    return;
                }
            }
            if (!String.IsNullOrEmpty(txtPisPasep.Text))
            {
                if (!RN.Validacao.ValidaNumerosInteirosPositivos(txtPisPasep.Text))
                {
                    lblMensagem.Visible = true;
                    lblMensagem.Text = "PIS/PASEP só permite números inteiros e positivos.";

                    return;
                }
                if (!RN.Validacao.ValidouPISPASEP(txtPisPasep.Text))
                {
                    lblMensagem.Visible = true;
                    lblMensagem.Text = "PIS/PASEP é inválido.";

                    return;
                }
            }
            if (dtDataExped.Text.Trim() != string.Empty)
            {
                if (dtDataExped.Date > DateTime.Now.Date)
                {
                    lblMensagem.Visible = true;
                    lblMensagem.Text = "A Data de Expedição da identidade tem que ser menor que a data de hoje.";

                    return;
                }
            }
            if (dtDataExped.Text.Trim() != string.Empty)
            {
                if (dtDataExped.Date < dtDataNasc.Date)
                {
                    lblMensagem.Visible = true;
                    lblMensagem.Text = "A Data de Expedição da identidade tem que ser maior que a data de nascimento.";

                    return;
                }
            }
            if (!String.IsNullOrEmpty(txtCrpof_Num.Text.Trim()))
            {
                if (!RN.Validacao.ValidaNumerosInteirosPositivos(txtCrpof_Num.Text.RetirarCaracteres()))
                {
                    lblMensagem.Visible = true;
                    lblMensagem.Text = "O número da carteira profissional só permite números inteiros e positivos.";

                    return;
                }
                if (txtCrpof_Num.Text.Length < 5 || txtCrpof_Num.Text.Length > 15)
                {
                    lblMensagem.Visible = true;
                    lblMensagem.Text = "O número da carteira profissional deve possuir entre 5 e 15 dígitos.";

                    return;
                }
            }
            if (dboCprof_DtExp.Text.Trim() != string.Empty)
            {
                if (dboCprof_DtExp.Date > DateTime.Now.Date)
                {
                    lblMensagem.Visible = true;
                    lblMensagem.Text = "A Data de Expedição da carteira profissional tem que ser menor que a data de hoje.";

                    return;
                }
            }
            if (dboCprof_DtExp.Text.Trim() != string.Empty)
            {
                if (dboCprof_DtExp.Date < dtDataNasc.Date)
                {
                    lblMensagem.Visible = true;
                    lblMensagem.Text = "A Data de Expedição da carteira profissional tem que ser maior que a data de nascimento.";

                    return;
                }
            }

            //#region Validações dos Campos de Documento
            //bool documentoValido, iniciouMensagem, maisDeUmCampo;
            //documentoValido = true;
            //iniciouMensagem = maisDeUmCampo = false;
            //System.Text.StringBuilder mensagemDocumento = new System.Text.StringBuilder();
            //System.Text.StringBuilder camposDocumento = new System.Text.StringBuilder();
            //mensagemDocumento.Append("Documento:<br>Não é possível deixar de preencher um dos campos referentes ao tipo de documento.");
            //if ((ddlRGTipoPessoa.SelectedValue == string.Empty))
            //{
            //    lblMensagem.Visible = true;
            //    lblMensagem.Text = "Tipo de Documento é campo obrigatório.";
            //    pcCandidatoDocente.ActiveTabIndex = 0;
            //    return;
            //}
            //else
            //{
            //    if (ddlRGTipoPessoa.SelectedValue == "RG")
            //    {
            //        if (String.IsNullOrEmpty(txtRGNum.Text.Trim()))
            //        {
            //            documentoValido = false;
            //            iniciouMensagem = true;
            //            camposDocumento.Append("Número ");
            //        }
            //        if (cmbRGUF.SelectedValue == "")
            //        {
            //            documentoValido = false;
            //            if (iniciouMensagem)
            //            {
            //                maisDeUmCampo = true;
            //                camposDocumento.Append("- Estado ");
            //            }
            //            else
            //            {
            //                iniciouMensagem = true;
            //                camposDocumento.Append("Estado ");
            //            }
            //        }
            //        if (cmbRGEmissor.SelectedValue == "")
            //        {
            //            documentoValido = false;
            //            if (iniciouMensagem)
            //            {
            //                maisDeUmCampo = true;
            //                camposDocumento.Append("- Órgão Emissor ");
            //            }
            //            else
            //            {
            //                iniciouMensagem = true;
            //                camposDocumento.Append("Órgão Emissor ");
            //            }
            //        }
            //        if (dtDataExped.Text.Trim() == string.Empty)
            //        {
            //            documentoValido = false;
            //            if (iniciouMensagem)
            //            {
            //                maisDeUmCampo = true;
            //                camposDocumento.Append("- Data de Expedição ");
            //            }
            //            else
            //            {
            //                iniciouMensagem = true;
            //                camposDocumento.Append("Data de Expedição ");
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (String.IsNullOrEmpty(txtRGNum.Text.Trim()))
            //        {
            //            documentoValido = false;
            //            iniciouMensagem = true;
            //            camposDocumento.Append("Número ");
            //        }
            //        if (cmbRGEmissor.SelectedValue == "")
            //        {
            //            documentoValido = false;
            //            if (iniciouMensagem)
            //            {
            //                maisDeUmCampo = true;
            //                camposDocumento.Append("- Órgão Emissor ");
            //            }
            //            else
            //            {
            //                iniciouMensagem = true;
            //                camposDocumento.Append("Órgão Emissor ");
            //            }
            //        }
            //        if (dtDataExped.Text.Trim() == string.Empty)
            //        {
            //            documentoValido = false;
            //            if (iniciouMensagem)
            //            {
            //                maisDeUmCampo = true;
            //                camposDocumento.Append("- Data de Expedição ");
            //            }
            //            else
            //            {
            //                iniciouMensagem = true;
            //                camposDocumento.Append("Data de Expedição ");
            //            }
            //        }
            //    }
            //    if (!documentoValido)
            //    {
            //        if (maisDeUmCampo)
            //            mensagemDocumento.Append("<br>Campos Necessários: ");
            //        else
            //            mensagemDocumento.Append("<br>Campo Necessário: ");
            //        mensagemDocumento.Append(camposDocumento);
            //        lblMensagem.Visible = true;
            //        lblMensagem.Text = mensagemDocumento.ToString();
            //        pcCandidatoDocente.ActiveTabIndex = 0;
            //        return;
            //    }
            //}
            //#endregion

            #region Validações dos Campos de Documento Profissional


            //bool cateiraProfissionalValida;
            //cateiraProfissionalValida = true;
            //iniciouMensagem = maisDeUmCampo = false;
            //System.Text.StringBuilder mensagemCarteiraProfissional = new System.Text.StringBuilder();
            //System.Text.StringBuilder camposCarteiraProfissional = new System.Text.StringBuilder();
            //mensagemCarteiraProfissional.Append("Carteira Profissional:<br>Não é possível deixar de preencher um dos campos referentes à carteira profissional.");
            //if (String.IsNullOrEmpty(txtCrpof_Num.Text.Trim()) || String.IsNullOrEmpty(txtCprof_Serie.Text.Trim()) || dboCprof_DtExp.Text.Trim() == string.Empty || ddDlCprof_Uf.SelectedValue == string.Empty)
            //{
            //    if (String.IsNullOrEmpty(txtCrpof_Num.Text.Trim()))
            //    {
            //        cateiraProfissionalValida = false;
            //        iniciouMensagem = true;
            //        camposCarteiraProfissional.Append("Número ");
            //    }
            //    if (String.IsNullOrEmpty(txtCprof_Serie.Text.Trim()))
            //    {
            //        cateiraProfissionalValida = false;
            //        if (iniciouMensagem)
            //        {
            //            maisDeUmCampo = true;
            //            camposCarteiraProfissional.Append("- Série ");
            //        }
            //        else
            //        {
            //            iniciouMensagem = true;
            //            camposCarteiraProfissional.Append("Série ");
            //        }
            //    }
            //    if (dboCprof_DtExp.Text.Trim() == string.Empty)
            //    {
            //        cateiraProfissionalValida = false;
            //        if (iniciouMensagem)
            //        {
            //            maisDeUmCampo = true;
            //            camposCarteiraProfissional.Append("- Data de Expedição ");
            //        }
            //        else
            //        {
            //            iniciouMensagem = true;
            //            camposCarteiraProfissional.Append("Data de Expedição ");
            //        }
            //    }
            //    if (ddDlCprof_Uf.SelectedValue == string.Empty)
            //    {
            //        cateiraProfissionalValida = false;
            //        if (iniciouMensagem)
            //        {
            //            maisDeUmCampo = true;
            //            camposCarteiraProfissional.Append("- Estado ");
            //        }
            //        else
            //        {
            //            iniciouMensagem = true;
            //            camposCarteiraProfissional.Append("Estado ");
            //        }
            //    }
            //}
            //if (!cateiraProfissionalValida)
            //{
            //    if (maisDeUmCampo)
            //        mensagemCarteiraProfissional.Append("<br>Campos Necessários: ");
            //    else
            //        mensagemCarteiraProfissional.Append("<br>Campo Necessário: ");
            //    mensagemCarteiraProfissional.Append(camposCarteiraProfissional);
            //    lblMensagem.Visible = true;
            //    lblMensagem.Text = mensagemCarteiraProfissional.ToString();
            //    pcCandidatoDocente.ActiveTabIndex = 0;
            //    return;
            //}

            #endregion
            #endregion

            #region Validações Particulares

            if (txtNomeCompleto.Text.Trim() != string.Empty)
            {
                Regex regex = new Regex(@"\s{2,}");
                string NomeCompl = regex.Replace(txtNomeCompleto.Text.Trim().ToUpper(), " ");
                Match nome = Regex.Match(NomeCompl.TrimStart(), @"^[aA-zZà-úÀ-Ú]+((\s[aA-zZà-úÀ-Ú]+)+)?$");

                if (!nome.Success)
                {
                    lblMensagem.Text = "Nome inválido";
                    return;
                }
                else
                    txtNomeCompleto.Text = nome.Value;
            }

            if (txtNomeMae.Text.Trim() != string.Empty)
            {
                Regex regex = new Regex(@"\s{2,}");
                string NomeCompl = regex.Replace(txtNomeMae.Text.Trim().ToUpper(), " ");
                Match nome = Regex.Match(NomeCompl.TrimStart(), @"^[aA-zZà-úÀ-Ú]+((\s[aA-zZà-úÀ-Ú]+)+)?$");

                if (!nome.Success)
                {
                    lblMensagem.Text = "Nome da mãe não pode conter números";
                    return;
                }
                else
                    txtNomeMae.Text = nome.Value;
            }

            if (txtNomePai.Text.Trim() != string.Empty)
            {
                Regex regex = new Regex(@"\s{2,}");
                string NomeCompl = regex.Replace(txtNomePai.Text.Trim().ToUpper(), " ");
                Match nome = Regex.Match(NomeCompl.TrimStart(), @"^[aA-zZà-úÀ-Ú]+((\s[aA-zZà-úÀ-Ú]+)+)?$");

                if (!nome.Success)
                {
                    lblMensagem.Text = "Nome do pai não pode conter números";
                    return;
                }
                else
                    txtNomePai.Text = nome.Value;
            }


            if (_tipoOperacao.Equals(TipoOperacao.Novo) && !String.IsNullOrEmpty(tseConcurso.DBValue.ToString()))
            {
                if (RN.CandidatoDocente.PeriodoInscricaoEncerrado(tseConcurso.DBValue.ToString()))
                {
                    lblMensagem.Visible = true;
                    lblMensagem.Text = "Período de inscrição encerrado para o processo seletivo '" + tseConcurso.DBValue.ToString() + "'.";
                    pcCandidatoDocente.ActiveTabIndex = 0;
                    return;
                }
            }
            if (_tipoOperacao.Equals(TipoOperacao.Novo) &&
                   rnCandidatoDocente.PossuiCandidatoInscrito(txtCPF.Text.RetirarMascaraCPF(), tseConcurso.DBValue.ToString()))
            {
                lblMensagem.Visible = true;
                lblMensagem.Text = "Candidato já está cadastrado neste concurso.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (!String.IsNullOrEmpty(txtCPF.Text))
            {
                if (!RN.Validacao.ValidaCpf(txtCPF.Text.RetirarMascaraCPF()))
                {
                    lblMensagem.Visible = true;
                    lblMensagem.Text = "O CPF do candidato é inválido.";
                    pcCandidatoDocente.ActiveTabIndex = 0;
                    return;
                }
                if (RN.CandidatoDocente.CandidatoDocenteConcursado(txtCPF.Text.RetirarMascaraCPF()))
                {
                    lblMensagem.Visible = true;
                    lblMensagem.Text = "O candidato já consta como professor concursado.";
                    pcCandidatoDocente.ActiveTabIndex = 0;
                    return;
                }
                if (RN.CandidatoDocente.CandidatoFuncionarioConcursado(txtCPF.Text.RetirarMascaraCPF()))
                {
                    lblMensagem.Visible = true;
                    lblMensagem.Text = "O candidato é servidor concursado.";
                    pcCandidatoDocente.ActiveTabIndex = 0;
                    return;
                }
                if (Convert.ToString(tseConcurso["indigena"]) == "N")
                {
                    if (_tipoOperacao.Equals(TipoOperacao.Novo) &&
                        RN.CandidatoDocente.CandidatoConcursadoTemporario(txtCPF.Text.RetirarMascaraCPF())
                        )
                    {
                        lblMensagem.Visible = true;
                        lblMensagem.Text = "Candidato obteve contrato temporário com a SEEDUC nos últimos 12 meses. Inscrição não permitida.";
                        pcCandidatoDocente.ActiveTabIndex = 0;
                        return;
                    }
                }
                else
                {
                    if (_tipoOperacao.Equals(TipoOperacao.Novo) &&
                            rnCandidatoDocente.PodeInscricaoIndigena(txtCPF.Text.RetirarMascaraCPF())
                            )
                    {
                        lblMensagem.Visible = true;
                        lblMensagem.Text = "Candidato obteve contrato temporário com a SEEDUC nos últimos 30 dias. Inscrição não permitida.";
                        pcCandidatoDocente.ActiveTabIndex = 0;
                        return;
                    }
                }

                if (_tipoOperacao.Equals(TipoOperacao.Novo) &&
                    RN.CandidatoDocente.ExisteCPFConcurso(Convert.ToString(tseConcurso.DBValue), txtCPF.Text.RetirarMascaraCPF()))
                {
                    lblMensagem.Visible = true;
                    lblMensagem.Text = "O CPF do candidato já está cadastrado neste concurso.";
                    pcCandidatoDocente.ActiveTabIndex = 0;
                    return;
                }
                if (_tipoOperacao.Equals(TipoOperacao.Alterar) &&
                    RN.CandidatoDocente.ExisteCPFConcurso(Convert.ToString(tseConcurso.DBValue), txtCandidato.Text, txtCPF.Text.RetirarMascaraCPF()))
                {
                    lblMensagem.Visible = true;
                    lblMensagem.Text = "O CPF do candidato já está cadastrado neste concurso.";
                    pcCandidatoDocente.ActiveTabIndex = 0;
                    return;
                }

            }
            #endregion

            #endregion

            LyCandidatoDocente dtCandidatoDocente = new LyCandidatoDocente();

            ObterDadosCandidatoDocente(dtCandidatoDocente);
            RetValue retorno = null;

            if (_tipoOperacao.Equals(TipoOperacao.Novo))
            {
                int ret = new RN.CandidatoDocente().Insere(dtCandidatoDocente);

                if (ret == 0)
                {
                    retorno.Errors.Add("Erro ao Incluir o Cadastro");
                }
                else
                {
                    Session.Add("SSconcurso", dtCandidatoDocente.Concurso);
                    Session.Add("SScandidato", dtCandidatoDocente.Candidato);

                    Session["SSconcurso"] = dtCandidatoDocente.Concurso;
                    Session["SScandidato"] = dtCandidatoDocente.Candidato;


                    txtCandidato.Text = dtCandidatoDocente.Candidato;
                    tseConcursoBusca.Value = dtCandidatoDocente.Concurso;
                    tseCandidatoBusca.Value = dtCandidatoDocente.Candidato;

                    _tipoOperacao = TipoOperacao.Consultar;
                    ControlarTipoOperacao();
                    lblMensagem.Text = "Inscrição realizada com sucesso!";
                }

                IDictionary<string, string> pares = new Dictionary<string, string>();
                pares.Add("candidato", Session["SScandidato"].ToString()); //dtCandidatoDocente.Candidato.ToString());
                pares.Add("concurso", Session["SSconcurso"].ToString());// dtCandidatoDocente.Concurso.ToString());
                btnImprimir.Attributes.Add("onclick", @"javascript:window.open('../Relatorio/Relatorios.aspx?report=rsconcursodocente&grp=processoseletivo&" + TPage.CodificaQueryString(pares) + "','', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes'); return false;");
            }
            if (_tipoOperacao.Equals(TipoOperacao.Alterar))
            {
                int ret = new RN.CandidatoDocente().AlterarCandidatoDocente(dtCandidatoDocente, tseConcurso.DBValue.ToString(), txtCandidato.Text);

                DataTable dtDadosDocente = RN.Docentes.ConsultarDadosDocente(tseConcurso.DBValue.ToString(), txtCandidato.Text);

                if (dtDadosDocente.Rows.Count > 0)
                {
                    RN.Pessoa.AlterarDadosCarteiraProfissional(dtCandidatoDocente.Cprof_num, dtCandidatoDocente.Cprof_serie, dtCandidatoDocente.Cprof_uf, dtCandidatoDocente.Cprof_dtexp.Value, dtDadosDocente.Rows[0]["pessoa"].ToString());
                }

                if (ret == 0)
                {
                    retorno.Errors.Add("Erro ao Incluir o Cadastro");
                }
                else
                {
                    lblMensagem.Text = "Inscrição alterada com sucesso!<br />"; //Comentado por Rafaela Alves a pedido da Claudia helena //Informe suas titulações e experiências, pois estas informações não poderão ser atualizadas posteriormente.";
                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();
                }
                grdProcessoSeletivo.DataBind();
            }

            if (retorno != null)
            {
                if (!retorno.Ok)
                {
                    lblMensagem.Text = retorno.Errors.ToString();
                }
                else
                {
                    if (_tipoOperacao.Equals(TipoOperacao.Novo))
                        lblMensagem.Text =
                            "Inscrição efetuada com sucesso!<br />Informe suas titulações e experiências, pois estas informações não poderão ser atualizadas posteriormente.";
                    if (_tipoOperacao.Equals(TipoOperacao.Alterar))
                        lblMensagem.Text =
                            "Inscrição alterada com sucesso!<br />Informe suas titulações e experiências, pois estas informações não poderão ser atualizadas posteriormente.";
                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();

                    pcCandidatoDocente.ActiveTabIndex = 1;
                }
            }
        }

        protected void tseConcurso_Changed(object sender, EventArgs args)
        {
            RN.Concurso rnConcurso = new Concurso();

            if (!tseConcurso.DBValue.IsNull && (_tipoOperacao == TipoOperacao.Alterar || _tipoOperacao == TipoOperacao.Novo))
            {
                DataTable dt = new DataTable();
                tseCandidatoBusca.Mode = ControlMode.View;
                tseConcursoBusca.Mode = ControlMode.View;
                cmbCargo.Enabled = false;
                cmbCargo.Items.Clear();

                if (_tipoOperacao == TipoOperacao.Novo)
                {
                    PreencheCmbCargo(tseConcurso.Value.ToString());
                }
                else
                {
                    PreencheCmbCargo();
                }

                dt = rnConcurso.RetornaFuncaoConcuso(tseConcurso.Value.ToString());

                if (dt.Rows.Count > 0)
                {
                    foreach (ListItem item in cmbCargo.Items)
                    {
                        if (item.Value == dt.Rows[0]["FUNCAOID"].ToString())
                        {
                            cmbCargo.SelectedValue = dt.Rows[0]["FUNCAOID"].ToString();
                            break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(cmbCargo.SelectedValue) || cmbCargo.SelectedValue == "Selecione")
                {
                    lblMensagem.Text = "Função do Processo Seletivo não encontrada. Verifique.";
                    btnSalvar.Visible = false;
                    cmbCargo.ClearSelection();

                }
                tseMunicipioProc.Mode = ControlMode.Edit;
                tseRegional.Mode = ControlMode.View;
            }
            else if (_tipoOperacao == TipoOperacao.Alterar || _tipoOperacao == TipoOperacao.Novo)
            {
                tseCandidatoBusca.Mode = ControlMode.View;
                tseConcursoBusca.Mode = ControlMode.View;
                cmbCargo.Enabled = false;
                cmbCargo.Items.Clear();
                cmbCargo.SelectedIndex = -1;
                PreencheCmbCargo();
                cmbCargo.SelectedValue = rnConcurso.RetornaFuncaoConcuso(tseConcurso.Value.ToString()).Rows[0]["FUNCAOID"].ToString();
                tseRegional.Mode = ControlMode.View;
            }
        }
        protected void tseDiscIngresso_Changed(object sender, EventArgs args)
        {

        }
        protected void tseRegional_Changed(object sender, EventArgs args)
        {
            if (!tseRegional.DBValue.IsNull && (_tipoOperacao == TipoOperacao.Alterar || _tipoOperacao == TipoOperacao.Novo))
            {

                DataTable dt = new DataTable();
                tseCandidatoBusca.Mode = ControlMode.View;
                tseConcursoBusca.Mode = ControlMode.View;

            }
            else if (_tipoOperacao == TipoOperacao.Alterar || _tipoOperacao == TipoOperacao.Novo)
            {
                tseCandidatoBusca.Mode = ControlMode.View;
                tseConcursoBusca.Mode = ControlMode.View;
                cmbCargo.Enabled = false;
                cmbCargo.Items.Clear();
                cmbCargo.SelectedIndex = -1;
            }
        }

        protected void tseMunicipioProc_Changed(object sender, EventArgs args)
        {

            RN.ProcessoSeletivo rnProcessoSeletivo = new Techne.Lyceum.RN.ProcessoSeletivo();
            if (Page.IsCallback)
                return;
            if (!tseMunicipioProc.DBValue.IsNull && tseMunicipioProc.IsValidDBValue)
            {
                //dtnucleo = rnProcessoSeletivo.ListaCoordenadoriaPor(Convert.ToString(tseMunicipioProc.DBValue), Convert.ToString(tseConcurso.DBValue));

                //if (dtnucleo.Rows.Count > 1)
                //{
                //    tseCoordenadoria.Visible = true;
                //    lblCoordenadoria.Visible = true;
                //}
                //else
                //{
                //    tseCoordenadoria.Visible = false;
                //    lblCoordenadoria.Visible = false;

                //    hdnNucleo.Value = dtnucleo.Rows[0]["NUCLEO"].ToString();
                //}
            }
            else
            {
                tseMunicipioProc.ResetValue();
            }

        }

        protected void tseNaturalidade_Changed(object sender, EventArgs args)
        {
            if (tseNaturalidade.IsValidDBValue & !tseNaturalidade.DBValue.IsNull)
                txtNaturalidadeUF.Text = tseNaturalidade["uf_sigla"].ToString();
            else
                txtNaturalidadeUF.Text = string.Empty;
        }

        private void cmbTitulacaoPontuacao_OnCallback(object source, CallbackEventArgsBase e)
        {
            decimal pontuacao = RN.CandidatoDocente.ConsultarPontuacaoTitulacao(e.Parameter, tseConcursoBusca.DBValue.ToString());
            ListEditItem li = new ListEditItem(pontuacao.ToString(), pontuacao.ToString());
            (source as ASPxComboBox).Items.Clear();
            (source as ASPxComboBox).Items.Add(li);
            (source as ASPxComboBox).SelectedIndex = 0;
        }

        private void cmbExperienciaPontuacao_OnCallback(object source, CallbackEventArgsBase e)
        {
            decimal pontuacao = RN.CandidatoDocente.ConsultarPontuacaoExperiencia(e.Parameter, tseConcursoBusca.DBValue.ToString());
            ListEditItem li = new ListEditItem(pontuacao.ToString(), pontuacao.ToString());
            (source as ASPxComboBox).Items.Clear();
            (source as ASPxComboBox).Items.Add(li);
            (source as ASPxComboBox).SelectedIndex = 0;
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Obter dados do Candidato Docente com a inclusão do campo etnia
        /// </summary>
        /// <param name="dadosCandidatoDocente"></param>
        private void ObterDadosCandidatoDocente(RN.Entidades.LyCandidatoDocente dadosCandidatoDocente)
        {
            string nome = txtNomeCompleto.Text.TrimEnd().EndsWith(".")
                ? txtNomeCompleto.Text.TrimEnd().Substring(0, txtNomeCompleto.Text.TrimEnd().Length - 1)
                : txtNomeCompleto.Text.TrimEnd();

            dadosCandidatoDocente.Nome = nome.Trim().ToUpper();
            txtNomeCompleto.Text = nome.Trim().ToUpper();
            dadosCandidatoDocente.Sexo = rblSexo.Text;
            dadosCandidatoDocente.Nome_mae = txtNomeMae.Text;
            dadosCandidatoDocente.Nome_pai = txtNomePai.Text;
            dadosCandidatoDocente.Cep = txtCep.Text;
            dadosCandidatoDocente.Endereco = txtEndereco.Text;
            dadosCandidatoDocente.End_num = txtEndNum.Text;
            dadosCandidatoDocente.End_compl = txtEndCompl.Text;
            dadosCandidatoDocente.Bairro = txtBairro.Text;
            dadosCandidatoDocente.Fone = txtFone.Text.RetirarMascaraTelefone();
            dadosCandidatoDocente.Celular = txtCelular.Text.RetirarMascaraTelefone();
            dadosCandidatoDocente.E_mail = txtEmail.Text;
            dadosCandidatoDocente.Pis_pasep = txtPisPasep.Text;
            dadosCandidatoDocente.Cprof_num = txtCrpof_Num.Text;
            dadosCandidatoDocente.Cprof_serie = txtCprof_Serie.Text;
            dadosCandidatoDocente.Rg_num = txtRGNum.Text;
            dadosCandidatoDocente.Cpf = txtCPF.Text.RetirarMascaraCPF();
            dadosCandidatoDocente.End_pais = "1";//--BRASIL 

            if (!cmbNecessidadeEspecial.SelectedValue.IsNullOrEmptyOrWhiteSpace())
            {
                dadosCandidatoDocente.NecessidadeEspecialId = Convert.ToInt32(cmbNecessidadeEspecial.SelectedValue);
            }

            if (dboCprof_DtExp.Value != null)
            {
                dadosCandidatoDocente.Cprof_dtexp = (DateTime?)dboCprof_DtExp.Value;
            }
            else
            {
                dadosCandidatoDocente.Cprof_dtexp = null;
            }

            if (dtDataExped.Value != null)
            {
                dadosCandidatoDocente.Rg_dtexp = (DateTime)dtDataExped.Value;
            }
            else
            {
                dadosCandidatoDocente.Rg_dtexp = null;
            }

            if (dtDataNasc.Value != null)
            {
                dadosCandidatoDocente.Dt_nasc = (DateTime)dtDataNasc.Value;
            }
            else
            {
                dadosCandidatoDocente.Dt_nasc = null;
            }

            if (dboCprof_DtExp.Value != null)
            {
                dadosCandidatoDocente.Cprof_dtexp = (DateTime)dboCprof_DtExp.Value;
            }
            else
            {
                dadosCandidatoDocente.Cprof_dtexp = null;
            }

            if (ddDlCprof_Uf.SelectedValue != "")
            {
                dadosCandidatoDocente.Cprof_uf = ddDlCprof_Uf.SelectedValue.ToString();
            }

            if (tseMunicipio.DBValue.ToString() != "")
            {
                dadosCandidatoDocente.End_pais = ddlPaisNasc.SelectedValue;
            }
            if (!tseNaturalidade.DBValue.IsNull) //if inserido por Rafaela Alves - 12/07/2013
                dadosCandidatoDocente.Municipio_nasc = tseNaturalidade.DBValue.ToString();
            if (tseMunicipio.DBValue.ToString() != "")
            {
                dadosCandidatoDocente.End_municipio = tseMunicipio.DBValue.ToString();
            }
            if (ddlEst_Civil.SelectedValue != "")
            {
                dadosCandidatoDocente.Estado_civil = ddlEst_Civil.SelectedValue;
            }
            if (cmbRGUF.SelectedValue != "")
            {
                dadosCandidatoDocente.Rg_uf = cmbRGUF.SelectedValue.ToString();
            }

            if (ddlRGTipoPessoa.SelectedValue != "")
            {
                dadosCandidatoDocente.Rg_tipo = ddlRGTipoPessoa.SelectedValue;
            }
            if (cmbRGEmissor.SelectedValue != "")
            {
                dadosCandidatoDocente.Rg_emissor = cmbRGEmissor.SelectedValue;
            }
            if (ddlCorRaca.SelectedValue != "")
            {
                dadosCandidatoDocente.EtniaId = Convert.ToInt32(ddlCorRaca.SelectedValue);
            }
            if (ddlCotas.SelectedValue != "")
            {
                dadosCandidatoDocente.CotaIdInscricao = Convert.ToInt32(ddlCotas.SelectedValue);
            }
            if (ddlPaisNasc.SelectedValue != "")
            {
                dadosCandidatoDocente.Pais_nasc = ddlPaisNasc.SelectedValue;
            }
            if (ddlNacionalidade.SelectedValue != "")
            {
                dadosCandidatoDocente.Nacionalidade = ddlNacionalidade.SelectedValue;
            }
            if (!tseConcurso.DBValue.IsNull)
            {
                dadosCandidatoDocente.Concurso = tseConcurso.DBValue.ToString();
            }
            if (!tseMunicipioProc.DBValue.IsNull)
                dadosCandidatoDocente.Municipio_proc = tseMunicipioProc.DBValue.ToString();

            if (!tseRegional.DBValue.IsNull)
                dadosCandidatoDocente.RegionalId = Convert.ToInt32(tseRegional.DBValue.ToString());

            if (_tipoOperacao == TipoOperacao.Novo)
            {
                dadosCandidatoDocente.Candidato = RN.CandidatoDocente.GeraCandidato();
                dadosCandidatoDocente.Status = 1;
                txtStatusCandidato.Text = "1";
            }
            else
            {
                dadosCandidatoDocente.Candidato = txtCandidato.Text;
                dadosCandidatoDocente.Status = Convert.ToDecimal(txtStatusCandidato.Text);
            }
        }

        private void LimparEnderecoNaturalidade()
        {
            txtNaturalidadeNasc.Text = string.Empty;
            txtNaturalidadeUF.Text = string.Empty;
            tseNaturalidade.ResetValue();
        }

        private void TituloGrid()
        {
            string tituloGrade = grdProcessoSeletivo.SettingsText.Title;
            if (tituloGrade != string.Empty) grdProcessoSeletivo.SettingsText.Title = tituloGrade.Replace("|Tabela:|", "Processo Seletivo");
            string tituloTitulacao = grdTitulacao.SettingsText.Title;
            if (tituloTitulacao != string.Empty) grdTitulacao.SettingsText.Title = tituloTitulacao.Replace("|Tabela:|", "Titulações");
            string tituloExperiencia = grdExperiencia.SettingsText.Title;
            if (tituloExperiencia != string.Empty) grdExperiencia.SettingsText.Title = tituloExperiencia.Replace("|Tabela:|", "Experiências");
        }

        private void CarregarDadosContratoTemporario()
        {
            //QueryTable qt = RN.CandidatoDocente.ConsultarContratoTemporario(tseConcursoBusca.DBValue, tseCandidatoBusca.DBValue);

            //txtStatus.Text = string.Empty;
            //dtInicio.Text = string.Empty;
            //dtFim.Text = string.Empty;

            //if (qt.Rows.Count > 0)
            //{
            //    txtStatus.Text = qt.Rows[0]["status"].ToString();
            //    if (!string.IsNullOrEmpty(qt.Rows[0]["dt_inicio_contrato"].ToString()))
            //        dtInicio.Date = Convert.ToDateTime(qt.Rows[0]["dt_inicio_contrato"].ToString());
            //    if (!string.IsNullOrEmpty(qt.Rows[0]["dt_fim_contrato"].ToString()))
            //        dtFim.Date = Convert.ToDateTime(qt.Rows[0]["dt_fim_contrato"].ToString());
            //}
        }

        private void ControlarTipoOperacao()
        {
            DataTable dtDadosCandidatoDocente = new DataTable();
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        pcCandidatoDocente.TabPages[1].Enabled = false;
                        pcCandidatoDocente.TabPages[2].Enabled = false;
                        pcCandidatoDocente.TabPages[3].Enabled = false;
                        //pcCandidatoDocente.TabPages[4].Enabled = false;

                        btnImprimir.Visible = false;
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);
                        pcCandidatoDocente.ActiveTabIndex = 0;
                        pcCandidatoDocente.Visible = false;
                        tseConcursoBusca.ResetValue();
                        tseCandidatoBusca.ResetValue();
                        tseConcursoBusca.Mode = ControlMode.Edit;
                        tseCandidatoBusca.Mode = ControlMode.Edit;
                        
                        txtStatusCandidato.Text = string.Empty;
                        HabilitaBotalValidarInscricao();
                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        pcCandidatoDocente.TabPages[1].Enabled = true;
                        pcCandidatoDocente.TabPages[2].Enabled = true;
                        pcCandidatoDocente.TabPages[3].Enabled = true;
                        //pcCandidatoDocente.TabPages[4].Enabled = true;

                        
                        ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar, btnExcluir };
                        ControlarVisibilidadeControle(controles);
                        btnImprimir.Visible = true;
                        pcCandidatoDocente.Visible = true;

                        DesabilitaCampos();
                        txtCandidato.Visible = true;
                        dtDataNasc.MaxDate = DateTime.Now.Date;
                        dtDataExped.MaxDate = DateTime.Now.Date;
                        dboCprof_DtExp.MaxDate = DateTime.Now.Date;
                        lblMsgCandidato.Visible = false;
                        tseConcursoBusca.Mode = ControlMode.Edit;
                        tseCandidatoBusca.Mode = ControlMode.Edit;
                        HabilitaBotalValidarInscricao();
                        break;
                    }

                case TipoOperacao.Novo:
                    {
                        pcCandidatoDocente.TabPages[1].Enabled = false;
                        pcCandidatoDocente.TabPages[2].Enabled = false;
                        pcCandidatoDocente.TabPages[3].Enabled = false;
                        //pcCandidatoDocente.TabPages[4].Enabled = false;

                        btnImprimir.Visible = false;
                        btnValidarInscricao.Visible = false;
                        pcCandidatoDocente.Visible = true;
                        LimparTela();
                        HabilitaCampos();
                        tseCandidatoBusca.ResetValue();
                        tseConcursoBusca.ResetValue();
                        tseConcurso.SqlWhere =
                            "CONVERT(datetime,CONVERT(varchar(10),DT_FIM_INSCR,102),102) >= CONVERT(datetime,CONVERT(varchar(10),getdate(),102),102)";
                        txtCandidato.Visible = false;
                        dtDataNasc.MaxDate = DateTime.Now.Date;
                        dtDataExped.MaxDate = DateTime.Now.Date;
                        dboCprof_DtExp.MaxDate = DateTime.Now.Date;
                        lblMsgCandidato.Visible = true;
                        txtStatusCandidato.Text = string.Empty;

                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);

                        CarregarDadosDrop(ddlEst_Civil.ID);
                        CarregarDadosDrop(ddlPaisNasc.ID);
                        CarregarDadosDrop(ddlNacionalidade.ID);
                        CarregarDadosDrop(ddlRGTipoPessoa.ID);
                        CarregarDadosDrop(cmbRGUF.ID);
                        CarregarDadosDrop(cmbRGEmissor.ID);
                        CarregarDadosDrop(ddDlCprof_Uf.ID);
                        CarregarDadosDrop(ddlCorRaca.ID);
                        CarregarDadosDrop(ddlCotas.ID);
                        CarregaNecessidadeEspecial();

                        tseConcursoBusca.Mode = ControlMode.View;
                        tseCandidatoBusca.Mode = ControlMode.View;

                        cmbCargo.Enabled = false;
                        tseMunicipioProc.Mode = ControlMode.View;
                        tseRegional.Mode = ControlMode.View;
                        pcCandidatoDocente.ActiveTabIndex = 0;

                        break;
                    }
                case TipoOperacao.Excluir:
                    {
                        RN.CandidatoDocente rnCandidatoDocente = new Techne.Lyceum.RN.CandidatoDocente();
                        DataTable dtDados = new DataTable();
                        List<string> mensagens = new List<string>();

                        pcCandidatoDocente.TabPages[1].Enabled = false;
                        pcCandidatoDocente.TabPages[2].Enabled = false;
                        pcCandidatoDocente.TabPages[3].Enabled = false;
                        //pcCandidatoDocente.TabPages[4].Enabled = false;

                        Ly_candidato_docente dtCandidatoDocente = new Ly_candidato_docente();
                        Ly_candidato_docente.Row dadosCandidatoDocente = dtCandidatoDocente.NewRow();
                        dadosCandidatoDocente.Candidato = txtCandidato.Text;
                        dadosCandidatoDocente.Concurso = tseConcurso.DBValue.ToString();

                        RetValue retorno = null;

                        dtDados = rnCandidatoDocente.ObtemDadosCandidato(tseConcurso.DBValue.ToString(), txtCandidato.Text);

                        if (dtDados.Rows.Count > 0)
                        {
                            //mensagens.Add("Candidato não pode ser excluído. Motivos: ");
                            if (Convert.ToInt32(dtDados.Rows[0]["POSSUI_HABILITACAO"]) == 1)
                            {
                                mensagens.Add("Possui Habilitação(ões)");
                            }
                            if (Convert.ToInt32(dtDados.Rows[0]["POSSUI_EXPERIENCIA"]) == 1)
                            {
                                mensagens.Add("Possui Experiência(s)");
                            }
                            if (Convert.ToInt32(dtDados.Rows[0]["POSSUI_TITULACAO"]) == 1)
                            {
                                mensagens.Add("Possui Titulação(ões)");
                            }

                            if (mensagens.Count > 0)
                            {
                                lblMensagem.Text = "Candidato não pode ser excluído. Motivos: <br />" + mensagens.Aggregate((x, y) => x + Environment.NewLine + y).Replace(Environment.NewLine, "<br />");
                                break;

                            }

                        }

                        retorno = RN.CandidatoDocente.Excluir(dadosCandidatoDocente);

                        if (retorno != null)
                        {
                            if (!retorno.Ok)
                            {
                                _tipoOperacao = TipoOperacao.Consultar;
                                ControlarTipoOperacao();
                                lblMensagem.Text = retorno.Errors.ToString();
                            }
                            else
                            {
                                lblMensagem.Text = retorno.Message;
                                _tipoOperacao = TipoOperacao.Inicial;
                                ControlarTipoOperacao();
                            }
                        }
                        HabilitaBotalValidarInscricao();
                        break;
                    }
                case TipoOperacao.Alterar:
                    {
                        pcCandidatoDocente.TabPages[1].Enabled = true;
                        pcCandidatoDocente.TabPages[2].Enabled = true;
                        pcCandidatoDocente.TabPages[3].Enabled = true;
                        //pcCandidatoDocente.TabPages[4].Enabled = true;

                        btnImprimir.Visible = false;
                        HabilitaCampos();
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);
                        lblMensagem.Text = String.Empty;
                        txtCandidato.ReadOnly = true;
                        txtCandidato.Visible = true;
                        dtDataNasc.MaxDate = DateTime.Now.Date;
                        dtDataExped.MaxDate = DateTime.Now.Date;
                        dboCprof_DtExp.MaxDate = DateTime.Now.Date;
                        lblMsgCandidato.Visible = false;
                        tseConcurso.Mode = ControlMode.View;
                        tseConcurso.SqlWhere = string.Empty;
                        cmbCargo.Enabled = false;

                        tseRegional.Mode = ControlMode.View;

                        tseCandidatoBusca.Mode = ControlMode.View;
                        tseConcursoBusca.Mode = ControlMode.View;
                        pcCandidatoDocente.ActiveTabIndex = 0;
                        btnValidarInscricao.Visible = false;
                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        pcCandidatoDocente.TabPages[1].Enabled = true;
                        pcCandidatoDocente.TabPages[2].Enabled = true;
                        pcCandidatoDocente.TabPages[3].Enabled = true;
                        //pcCandidatoDocente.TabPages[4].Enabled = true;
                        btnValidarInscricao.Visible = false;
                        
                        txtCandidato.Visible = true;
                        lblMsgCandidato.Visible = false;
                        lblMensagem.Text = string.Empty;

                        LimparTela();
                        LimparEndereco();
                        LimparEnderecoNaturalidade();

                        Ly_candidato_docente dtCandidatoDocente = new Ly_candidato_docente();

                        Ly_candidato_docente.Row dadosCandidatoDocente = dtCandidatoDocente.NewRow();
                        dadosCandidatoDocente.Concurso = tseConcursoBusca.DBValue.ToString();
                        dadosCandidatoDocente.Candidato = tseCandidatoBusca.DBValue.ToString();

                        if (string.IsNullOrEmpty(tseConcursoBusca.DBValue.ToString()) || string.IsNullOrEmpty(tseCandidatoBusca.DBValue.ToString()))
                        {
                            lblMensagem.Text = "Favor informar um processo seletivo e um número de inscrição.";
                            pcCandidatoDocente.Visible = false;
                            ImageButton[] controles = new ImageButton[] { btnNovo };
                            ControlarVisibilidadeControle(controles);
                            tseConcursoBusca.Mode = ControlMode.Edit;
                            tseCandidatoBusca.Mode = ControlMode.Edit;
                            pcCandidatoDocente.ActiveTabIndex = 0;

                            break;
                        }

                        if (dadosCandidatoDocente == null)
                        {
                            lblMensagem.Text = "Candidato não encontrado";
                            pcCandidatoDocente.Visible = false;
                            ImageButton[] controles = new ImageButton[] { btnNovo };
                            ControlarVisibilidadeControle(controles);
                        }
                        else
                        {
                            ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar, btnExcluir };
                            ControlarVisibilidadeControle(controles);
                            pcCandidatoDocente.Visible = true;

                            CarregaNecessidadeEspecial();
                            CarregarDadosDrop(ddlEst_Civil.ID);
                            CarregarDadosDrop(ddlPaisNasc.ID);
                            CarregarDadosDrop(ddlNacionalidade.ID);
                            CarregarDadosDrop(ddlRGTipoPessoa.ID);
                            CarregarDadosDrop(cmbRGUF.ID);
                            CarregarDadosDrop(cmbRGEmissor.ID);
                            CarregarDadosDrop(ddDlCprof_Uf.ID);
                            CarregarDadosDrop(ddlCorRaca.ID);
                            CarregarDadosDrop(ddlCotas.ID);


                            dtDadosCandidatoDocente =
                                new RN.CandidatoDocente().ConsultarCandidatoDocente(
                                    dadosCandidatoDocente.Concurso, dadosCandidatoDocente.Candidato);

                            if (dtDadosCandidatoDocente.Rows.Count > 0)
                            {
                                CarregaDadosCandidatoDocenteNovo(dtDadosCandidatoDocente);

                            }

                            DesabilitaCampos();
                        }

                        tseConcursoBusca.Mode = ControlMode.Edit;
                        tseCandidatoBusca.Mode = ControlMode.Edit;
                        pcCandidatoDocente.ActiveTabIndex = 0;

                        IDictionary<string, string> pares = new Dictionary<string, string>();
                        pares.Add("candidato", tseCandidatoBusca.DBValue.ToString());
                        pares.Add("concurso", tseConcursoBusca.DBValue.ToString());
                        btnImprimir.Attributes.Add("onclick", @"javascript:window.open('../Relatorio/Relatorios.aspx?report=rsconcursodocente&grp=processoseletivo&" + TPage.CodificaQueryString(pares) + "','', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes'); return false;");
                        btnImprimir.Visible = true;

                        DateTime? dataValidacao = null;
                        dataValidacao = dtDadosCandidatoDocente.Rows[0]["DATAVALIDACAOINSCRICAO"] != DBNull.Value ? Convert.ToDateTime(dtDadosCandidatoDocente.Rows[0]["DATAVALIDACAOINSCRICAO"]) : (DateTime?)null;

                        HabilitaBotalValidarInscricao();

                        break;
                    }
            }

            HabilitaDesabilitaAbaDisciplinaHabilitacoes();
        }


        private void HabilitaBotalValidarInscricao()
        {
            btnValidarInscricao.Visible = false;

            if (txtStatusCandidato.Text == "23")
                btnValidarInscricao.Visible = true;
        }


        private void CarregaNecessidadeEspecial()
        {
            RN.NecessidadeEspecial.NecessidadeEspecial rnNecessidadeEspecial = new RN.NecessidadeEspecial.NecessidadeEspecial();
            ListItem itemVazio = new ListItem("Selecione", string.Empty);

            cmbNecessidadeEspecial.Items.Clear();
            cmbNecessidadeEspecial.DataSource = rnNecessidadeEspecial.ListaNecessidadeEspecialAtiva();
            cmbNecessidadeEspecial.DataBind();
            cmbNecessidadeEspecial.Items.Insert(0, itemVazio);
        }

        private void CarregaDadosCandidatoDocenteNovo(DataTable dadosCandidatoDocente)
        {

            RN.ProcessoSeletivo rnProcessoSeletivo = new Techne.Lyceum.RN.ProcessoSeletivo();

            txtNomeCompleto.Text = dadosCandidatoDocente.Rows[0]["NOME"].ToString();
            txtNomeMae.Text = dadosCandidatoDocente.Rows[0]["Nome_mae"].ToString();
            txtNomePai.Text = dadosCandidatoDocente.Rows[0]["Nome_pai"].ToString();
            txtCep.Text = dadosCandidatoDocente.Rows[0]["Cep"].ToString();
            txtMunicipio.Text = dadosCandidatoDocente.Rows[0]["Municipio_nasc"].ToString();
            txtEndereco.Text = dadosCandidatoDocente.Rows[0]["Endereco"].ToString();
            txtEndNum.Text = dadosCandidatoDocente.Rows[0]["End_num"].ToString();
            txtEndCompl.Text = dadosCandidatoDocente.Rows[0]["End_compl"].ToString();
            txtBairro.Text = dadosCandidatoDocente.Rows[0]["Bairro"].ToString();

            int resul;
            if (int.TryParse(dadosCandidatoDocente.Rows[0]["Fone"].ToString(), out resul))
                txtFone.Text = string.Format("{0:(00)0000-0000}", resul);
            else
                txtFone.Text = dadosCandidatoDocente.Rows[0]["Fone"].ToString().AplicarMascaraTelefoneComDDD();

            long resultado;
            if (long.TryParse(dadosCandidatoDocente.Rows[0]["Celular"].ToString(), out resultado))
            {
                if (dadosCandidatoDocente.Rows[0]["Celular"].ToString().Length == 10)
                {
                    txtCelular.Text = string.Format("{0:(00)0000-0000}", resultado);
                }
                else
                {
                    txtCelular.Text = string.Format("{0:(00)00000-0000}", resultado);
                }
            }



            txtEmail.Text = dadosCandidatoDocente.Rows[0]["E_mail"].ToString();
            txtRGNum.Text = dadosCandidatoDocente.Rows[0]["Rg_num"].ToString();
            txtCPF.Text = dadosCandidatoDocente.Rows[0]["Cpf"].ToString().AplicarMascaraCPF();
            txtPisPasep.Text = dadosCandidatoDocente.Rows[0]["Pis_pasep"].ToString();
            txtCrpof_Num.Text = dadosCandidatoDocente.Rows[0]["Cprof_num"].ToString();
            txtCprof_Serie.Text = dadosCandidatoDocente.Rows[0]["Cprof_serie"].ToString();
            txtStatusCandidato.Text = Convert.ToString(dadosCandidatoDocente.Rows[0]["Status"]);
            dtDataNasc.MaxDate = DateTime.Now.Date;
            dtDataExped.MaxDate = DateTime.Now.Date;
            dboCprof_DtExp.MaxDate = DateTime.Now.Date;
            rblSexo.Text = dadosCandidatoDocente.Rows[0]["Sexo"].ToString();

            dtDataNasc.Date = Convert.ToDateTime(dadosCandidatoDocente.Rows[0]["Dt_nasc"]);
            dtDataExped.Date = Convert.ToDateTime(dadosCandidatoDocente.Rows[0]["Rg_dtexp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dadosCandidatoDocente.Rows[0]["Rg_dtexp"]));
            dboCprof_DtExp.Date = dadosCandidatoDocente.Rows[0]["Cprof_dtexp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dadosCandidatoDocente.Rows[0]["Cprof_dtexp"]);

            PreencherDadoCombo(cmbNecessidadeEspecial, Convert.ToString(dadosCandidatoDocente.Rows[0]["necessidadeespecialid"] == DBNull.Value ? "" : dadosCandidatoDocente.Rows[0]["necessidadeespecialid"]));
            PreencherDadoCombo(ddlEst_Civil, Convert.ToString(dadosCandidatoDocente.Rows[0]["Estado_civil"]));
            PreencherDadoCombo(ddlPaisNasc, Convert.ToString(dadosCandidatoDocente.Rows[0]["Pais_nasc"]));
            PreencherDadoCombo(ddlNacionalidade, Convert.ToString(dadosCandidatoDocente.Rows[0]["Nacionalidade"]));
            PreencherDadoCombo(ddlRGTipoPessoa, Convert.ToString(dadosCandidatoDocente.Rows[0]["Rg_tipo"]));
            PreencherDadoCombo(cmbRGUF, Convert.ToString(dadosCandidatoDocente.Rows[0]["Rg_uf"]));
            PreencherDadoCombo(cmbRGEmissor, Convert.ToString(dadosCandidatoDocente.Rows[0]["Rg_emissor"]));
            PreencherDadoCombo(ddDlCprof_Uf, Convert.ToString(dadosCandidatoDocente.Rows[0]["Cprof_uf"]));
            PreencherDadoCombo(ddlCorRaca, Convert.ToString(dadosCandidatoDocente.Rows[0]["ETNIAID"]));
            PreencherDadoCombo(ddlCotas, Convert.ToString(dadosCandidatoDocente.Rows[0]["COTAIDINSCRICAO"]));

            if (!string.IsNullOrEmpty(dadosCandidatoDocente.Rows[0]["Concurso"].ToString()))
                tseConcurso.DBValue = dadosCandidatoDocente.Rows[0]["Concurso"].ToString();
            txtCandidato.Text = dadosCandidatoDocente.Rows[0]["Candidato"].ToString();

            Session["SSconcurso"] = dadosCandidatoDocente.Rows[0]["Concurso"].ToString();
            Session["SScandidato"] = dadosCandidatoDocente.Rows[0]["Candidato"].ToString();
            //Session["SScategoria"] = dadosCandidatoDocente.Rows[0]["Categoria"].ToString();

            //ControlarObrigatoriedadeDisciplinaIngresso(dadosCandidatoDocente.Rows[0]["Categoria"].ToString());
            cmbCargo.Items.Clear();

            //if (!String.IsNullOrEmpty(dadosCandidatoDocente.Rows[0]["Categoria"].ToString()))
            //{
            PreencheCmbCargo(dadosCandidatoDocente.Rows[0]["Concurso"].ToString());
            //}
            //else
            //    cmbCargo.SelectedIndex = -1;

            if (!string.IsNullOrEmpty(dadosCandidatoDocente.Rows[0]["End_municipio"].ToString()))
            {
                tseMunicipio.DBValue = dadosCandidatoDocente.Rows[0]["End_municipio"].ToString();
                if (tseMunicipio.IsValidDBValue & !tseMunicipio.DBValue.IsNull)
                    txtEstado.Value = tseMunicipio["uf_sigla"].ToString();
            }
            if (!string.IsNullOrEmpty(dadosCandidatoDocente.Rows[0]["Municipio_nasc"].ToString()))
            {
                tseNaturalidade.DBValue = dadosCandidatoDocente.Rows[0]["Municipio_nasc"].ToString();
                if (tseNaturalidade.IsValidDBValue & !tseNaturalidade.DBValue.IsNull)
                    txtNaturalidadeUF.Text = tseNaturalidade["uf_sigla"].ToString();

            }
            if (!string.IsNullOrEmpty(dadosCandidatoDocente.Rows[0]["Municipio_proc"].ToString())) //Rafaela Alves - 12/07/2013
            {
                tseMunicipioProc.DBValue = dadosCandidatoDocente.Rows[0]["Municipio_proc"].ToString();

                //dtnucleo = rnProcessoSeletivo.ListaCoordenadoriaPor(Convert.ToString(tseMunicipioProc.DBValue), Convert.ToString(tseConcurso.DBValue));

                //if (dtnucleo.Rows.Count == 0)
                //{
                //    lblMensagem.Text = "Erro. Não há município vinculado ao processo seletivo";
                //}
                //else if (dtnucleo.Rows.Count > 1)
                //{
                //    tseCoordenadoria.Visible = true;
                //    lblCoordenadoria.Visible = true;

                //    tseCoordenadoria.DBValue = dadosCandidatoDocente.Rows[0]["Nucleo"].ToString() == null ? "0" : dadosCandidatoDocente.Rows[0]["Nucleo"].ToString();
                //}
                //else
                //{
                //    hdnNucleo.Value = dtnucleo.Rows[0]["Nucleo"].ToString();
                //    tseCoordenadoria.Visible = false;
                //    lblCoordenadoria.Visible = false;
                //}
            }
            tseRegional.DBValue = dadosCandidatoDocente.Rows[0]["regionalid"].ToString() == null ? "0" : dadosCandidatoDocente.Rows[0]["regionalid"].ToString();

            txtTotalPont.Text = RN.CandidatoDocente.ConsultarPontuacao(dadosCandidatoDocente.Rows[0]["Candidato"].ToString(), dadosCandidatoDocente.Rows[0]["Concurso"].ToString()).ToString();
        }

        private object CarregarDadosDrop(string idDrop)
        {
            QueryTable dadosDrop = null;
            string param = string.Empty;

            try
            {
                switch (idDrop)
                {
                    case "ddlEst_Civil":
                        dadosDrop = RN.Basico.ConsultaItemTabelaValDescr("Estado civil");
                        PreencheDropdownList(ddlEst_Civil, dadosDrop, "");
                        break;

                    case "ddlPaisNasc":
                        dadosDrop = RN.Basico.ConsultarPais();
                        PreencheDropdownList(ddlPaisNasc, dadosDrop, "");
                        break;

                    case "ddlNacionalidade":
                        dadosDrop = RN.Basico.ConsultarNacionalidade();
                        PreencheDropdownList(ddlNacionalidade, dadosDrop, "");
                        break;

                    case "ddlRGTipoPessoa":
                        param = "TIPO DOC";
                        dadosDrop = RN.Basico.ConsultaItemTabelaValDescr(param);
                        PreencheDropdownList(ddlRGTipoPessoa, dadosDrop, "");
                        break;

                    case "cmbUF":
                        dadosDrop = RN.Basico.ConsultarUF();
                        break;

                    case "cmbRGUF":
                        dadosDrop = RN.Basico.ConsultarUF();
                        PreencheDropdownList(cmbRGUF, dadosDrop, "");
                        break;

                    case "cmbRGEmissor":
                        param = "ORGAO RG_CT";
                        dadosDrop = RN.Basico.ConsultaItemTabelaValDescr(param);
                        PreencheDropdownList(cmbRGEmissor, dadosDrop, "");
                        break;

                    case "ddDlCprof_Uf":
                        dadosDrop = RN.Basico.ConsultarUF();
                        PreencheDropdownList(ddDlCprof_Uf, dadosDrop, "");
                        break;

                    case "ddlCorRaca":
                        RN.Etnia rnEtnia = new Etnia();
                        dadosDrop = rnEtnia.ConsultarEtniaContratoTemporario();
                        PreencheDropdownList(ddlCorRaca, dadosDrop, "");
                        break;

                    case "ddlCotas":
                        dadosDrop = RN.ContratoTemporario.Cota.ListarCotas();
                        PreencheDropdownList(ddlCotas, dadosDrop, "<Selecione>");
                        break;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                dadosDrop = null;
            }

            return dadosDrop;
        }

        private void PreencheDropdownList(DropDownList lista, object dados, string valorPadrao)
        {
            lista.SelectedIndex = -1;
            lista.Items.Clear();
            lista.SelectedValue = null;
            lista.DataSource = dados;
            lista.DataBind();
            ListItem itemVazio = new ListItem("<Selecione>", "");
            lista.Items.Insert(0, itemVazio);

            if (_tipoOperacao.Equals(TipoOperacao.Novo))
            {
                if (lista == ddlNacionalidade)
                {
                    ListItem listItem = lista.Items.FindByText("BRASILEIRA");
                    if (listItem != null)
                    {
                        lista.ClearSelection();
                        listItem.Selected = true;
                    }
                }
            }

            if (valorPadrao != "")
            {
                ListItem itemPadrao = lista.Items.FindByText(valorPadrao);
                if (itemPadrao != null)
                {
                    lista.ClearSelection();
                    itemPadrao.Selected = true;
                }
                else
                {
                    //lista.Items.Insert(0, new ListItem(valorPadrao, string.Empty));
                    lista.Items.Add(new ListItem(valorPadrao, string.Empty));
                }
            }
        }

        private void LimparTela()
        {
            hdnNucleo.Value = string.Empty;
            txtNomeCompleto.Text = string.Empty;
            txtNomeMae.Text = string.Empty;
            txtNomePai.Text = string.Empty;
            txtCep.Text = string.Empty;
            txtMunicipio.Text = string.Empty;
            txtEndereco.Text = string.Empty;
            txtEndNum.Text = string.Empty;
            txtEndCompl.Text = string.Empty;
            txtBairro.Text = string.Empty;
            txtFone.Text = string.Empty;
            txtCelular.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtRGNum.Text = string.Empty;
            txtCPF.Text = string.Empty;
            txtPisPasep.Text = string.Empty;
            txtCrpof_Num.Text = string.Empty;
            txtCprof_Serie.Text = string.Empty;
            txtEstado.Value = string.Empty;
            txtNaturalidadeUF.Text = string.Empty;

            rblSexo.SelectedIndex = -1;

            dtDataNasc.Text = string.Empty;
            dtDataExped.Text = string.Empty;
            dboCprof_DtExp.Text = string.Empty;

            cmbNecessidadeEspecial.Items.Clear();
            ddlEst_Civil.Items.Clear();
            ddlPaisNasc.Items.Clear();
            ddlNacionalidade.Items.Clear();
            ddlRGTipoPessoa.Items.Clear();
            cmbRGUF.Items.Clear();
            cmbRGEmissor.Items.Clear();
            ddDlCprof_Uf.Items.Clear();

            tseConcurso.ResetValue();
            tseRegional.ResetValue();
            tseMunicipioProc.ResetValue();
            cmbCargo.Items.Clear();
            cmbCargo.SelectedIndex = -1;
            tseMunicipio.ResetValue();
            tseNaturalidade.ResetValue();
            btnValidarInscricao.Visible = false;
            grdTitulacao.CancelEdit();
            grdExperiencia.CancelEdit();
            grdDisciplinasHabilitacao.CancelEdit();
            txtStatusCandidato.Text = string.Empty;
        }

        private void DesabilitaCampos()
        {
            txtNomeCompleto.ReadOnly = true;
            txtNomeMae.ReadOnly = true;
            txtNomePai.ReadOnly = true;
            txtCep.ReadOnly = true;
            txtMunicipio.ReadOnly = true;
            txtEndereco.ReadOnly = true;
            txtEndNum.ReadOnly = true;
            txtEndCompl.ReadOnly = true;
            txtBairro.ReadOnly = true;
            txtFone.ReadOnly = true;
            txtCelular.ReadOnly = true;
            txtEmail.ReadOnly = true;
            txtRGNum.ReadOnly = true;
            txtCPF.ReadOnly = true;
            txtPisPasep.ReadOnly = true;
            txtCrpof_Num.ReadOnly = true;
            txtCprof_Serie.ReadOnly = true;

            rblSexo.Enabled = false;

            dtDataNasc.ReadOnly = true;
            dtDataExped.ReadOnly = true;
            dboCprof_DtExp.ReadOnly = true;

            cmbNecessidadeEspecial.Enabled = false;
            ddlEst_Civil.Enabled = false;
            ddlPaisNasc.Enabled = false;
            ddlNacionalidade.Enabled = false;

            ddlRGTipoPessoa.Enabled = false;
            cmbRGUF.Enabled = false;
            cmbRGEmissor.Enabled = false;
            ddDlCprof_Uf.Enabled = false;
            ddlCorRaca.Enabled = false;

            tseConcurso.Mode = ControlMode.View;
            tseConcurso.SqlWhere = string.Empty;
            cmbCargo.Enabled = false;
            tseRegional.Mode = ControlMode.View;
            tseMunicipio.Mode = ControlMode.View;
            tseNaturalidade.Mode = ControlMode.View;

            //Rafaela Alves - 12/07/2013
            tseMunicipioProc.Mode = ControlMode.View;

            tsCEP.ShowButton = false;

            ddlCotas.Enabled = false;
        }

        private void HabilitaCampos()
        {
            txtNomeCompleto.ReadOnly = false;
            txtNomeMae.ReadOnly = false;
            txtNomePai.ReadOnly = false;
            txtCep.ReadOnly = false;
            txtMunicipio.ReadOnly = false;
            txtEndereco.ReadOnly = false;
            txtEndNum.ReadOnly = false;
            txtEndCompl.ReadOnly = false;
            txtBairro.ReadOnly = false;
            txtFone.ReadOnly = false;
            txtCelular.ReadOnly = false;
            txtEmail.ReadOnly = false;
            txtRGNum.ReadOnly = false;
            txtCPF.ReadOnly = false;
            txtPisPasep.ReadOnly = false;
            txtCrpof_Num.ReadOnly = false;
            txtCprof_Serie.ReadOnly = false;

            rblSexo.Enabled = true;

            dtDataNasc.ReadOnly = false;
            dtDataExped.ReadOnly = false;
            dboCprof_DtExp.ReadOnly = false;

            cmbNecessidadeEspecial.Enabled = true;
            ddlEst_Civil.Enabled = true;
            ddlPaisNasc.Enabled = true;
            ddlNacionalidade.Enabled = true;

            ddlRGTipoPessoa.Enabled = true;
            cmbRGUF.Enabled = true;
            cmbRGEmissor.Enabled = true;
            ddDlCprof_Uf.Enabled = true;
            ddlCorRaca.Enabled = true;
            ddlCotas.Enabled = true;

            tseConcurso.Mode = ControlMode.Edit;
            cmbCargo.Enabled = true;
            tseRegional.Mode = ControlMode.Edit;
            tseMunicipio.Mode = ControlMode.Edit;
            tseNaturalidade.Mode = ControlMode.Edit;

            //Rafaela Alves - 12/07/2013
            tseMunicipioProc.Mode = ControlMode.Edit;

            tsCEP.ShowButton = true;

        }

        private void LimparEndereco()
        {
            txtMunicipio.Text = string.Empty;
            tseMunicipio.ResetValue();

            txtEstado.Value = string.Empty;
            txtEndereco.Text = string.Empty;
            txtCep.Text = string.Empty;
            txtEndCompl.Text = string.Empty;
            txtEndNum.Text = string.Empty;
            txtBairro.Text = string.Empty;
            txtNaturalidadeUF.Text = string.Empty;
        }

        /// <summary>
        /// Aplica estilos de obrigatoriedade para o campo Disciplina de Ingresso
        /// de acordo com a Função do candidato.
        /// </summary>
        /// <param name="categoria">Função do Candidato</param>
        private void ControlarObrigatoriedadeDisciplinaIngresso(string categoriaCandidato)
        {
            //
        }

        /// <summary>
        /// Preenche o campo Função da ficha de inscrição usando a notação definida pela SEEDUC.
        /// DEFINIÇÂO: Nomenclatura utilizada pelo usuário do sistema para Função de docente
        /// DOC I = Professor para atuar nos anos finais do ensino fundamental e/ou ensino médio
        /// DOC II = Professor para atuar nos anos iniciais do ensino fundamental
        /// </summary>
        /// <param name="funcaoCandidato">Função</param>

        private void PreencheCmbCargo()
        {
            RN.Funcao rnFuncao = new RN.Funcao();

            cmbCargo.Items.Clear();

            cmbCargo.DataSource = rnFuncao.RetornaFuncao();
            cmbCargo.DataBind();
            cmbCargo.Items.Insert(0, "Selecione");
            cmbCargo.DataValueField = "CODIGO";
            cmbCargo.DataTextField = "DESCRICAO";
        }

        private void PreencheCmbCargo(string concurso)
        {
            RN.Funcao rnFuncao = new RN.Funcao();

            cmbCargo.Items.Clear();

            cmbCargo.DataSource = rnFuncao.RetornaFuncao(concurso);
            cmbCargo.DataBind();
            cmbCargo.DataValueField = "CODIGO";
            cmbCargo.DataTextField = "DESCRICAO";
        }

        #endregion

        #region Métodos dos Botões

        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (ImageButton botao in botoes)
            {
                botao.Visible = true;
            }

            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(btnExcluir, AcaoControle.excluir);
        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnEditar.Visible = false;
            btnExcluir.Visible = false;
            btnNovo.Visible = false;
            btnSalvar.Visible = false;
            btnImprimir.Visible = false;
            btnValidarInscricao.Visible = false;
        }
        #endregion

        #region Grid Tituações e Experiências
        private string SomarPontuacao()
        {
            object somaTit = grdTitulacao.GetTotalSummaryValue(grdTitulacao.TotalSummary[0]);
            object somaExp = grdExperiencia.GetTotalSummaryValue(grdExperiencia.TotalSummary[0]);

            int somaTitu = 0;
            int somaExpe = 0;

           
                somaTitu = Convert.ToInt32(somaTit);
            
                somaExpe = Convert.ToInt32(somaExp);
            

            //if (Convert.ToInt32(somaTitu) 

            //if (somaTit != null && somaExp != null)
            //{
            //    int.TryParse(somaTit.ToString(), out somaTitu);
            //    int.TryParse(somaExp.ToString(), out somaExpe);
            //}

            return (somaTitu + somaExpe).ToString();
        }

        protected void grdExperiencia_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (!grdExperiencia.IsEditing || e.Column.FieldName != "pontuacao")
                return;
            ASPxComboBox combo = e.Editor as ASPxComboBox;
            combo.Callback += new CallbackEventHandlerBase(cmbExperienciaPontuacao_OnCallback);
        }

        protected void grdTitulacao_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (!grdTitulacao.IsEditing || e.Column.FieldName != "pontuacao")
                return;
            ASPxComboBox combo = e.Editor as ASPxComboBox;
            combo.Callback += new CallbackEventHandlerBase(cmbTitulacaoPontuacao_OnCallback);
        }

        protected void grdTitulacao_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues.Add("concurso", Session["SSconcurso"].ToString());
            e.NewValues.Add("candidato", Session["SScandidato"].ToString());
        }

        protected void grdTitulacao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            e.Keys.Add("concurso", Session["SSconcurso"].ToString());
            e.Keys.Add("candidato", Session["SScandidato"].ToString());
        }

        protected void grdTitulacao_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Keys.Add("concurso", Session["SSconcurso"].ToString());
            e.Keys.Add("candidato", Session["SScandidato"].ToString());
        }

        protected void grdExperiencia_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Keys.Add("concurso", Session["SSconcurso"].ToString());
            e.Keys.Add("candidato", Session["SScandidato"].ToString());
        }

        protected void grdExperiencia_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues.Add("concurso", Session["SSconcurso"].ToString());
            e.NewValues.Add("candidato", Session["SScandidato"].ToString());
        }

        protected void grdExperiencia_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            e.Keys.Add("concurso", Session["SSconcurso"].ToString());
            e.Keys.Add("candidato", Session["SScandidato"].ToString());
        }

        protected void grdExperiencia_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            txtTotalPont.Text = SomarPontuacao();
        }

        protected void grdTitulacao_CustomJSProperties(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpTotal"] = SomarPontuacao();
        }

        protected void grdExperiencia_CustomJSProperties(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpTotal"] = SomarPontuacao();
        }

        protected void grdExperiencia_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdExperiencia);
        }

        protected void grdTitulacao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdTitulacao);
        }
        #endregion

        #region Grid Processo Seletivo
        protected void grdProcessoSeletivo_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            odsProcessoSeletivo.Select();
            odsProcessoSeletivo.DataBind();
            grdProcessoSeletivo.DataBind();
        }

        protected void grdProcessoSeletivo_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdProcessoSeletivo);
        }
        #endregion

        public static void MsgBox(string Mensagem, Page page)
        {
            var script = @"alert('" + Mensagem + @"');";
            page.Page.ClientScript.RegisterStartupScript(page.Page.GetType(), "popup", script, true);
            return;
        }

        protected void grdDisciplinasHabilitacao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdDisciplinasHabilitacao);
        }

        protected void grdDisciplinasHabilitacao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdDisciplinasHabilitacao.Settings.ShowFilterRow = false;


        }

        protected void grdDisciplinasHabilitacao_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdDisciplinasHabilitacao.IsEditing)
            {
                if ((e.Column.FieldName) == "DESCRICAO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "HABILITADO")
                    e.Editor.ReadOnly = false;
            }
        }

        protected void grdDisciplinasHabilitacao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs eventArgs)
        {
            eventArgs.NewValues.Add("concurso", Session["SSconcurso"].ToString());
            eventArgs.NewValues.Add("candidato", Session["SScandidato"].ToString());
        }

        protected void grdDisciplinasHabilitacao_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs eventArgs)
        {
            int intStatus = RN.CandidatoDocente.RetornaStatusCandidato(Session["SSconcurso"].ToString(), Session["SScandidato"].ToString());

            //Status: 1- "Aguardando" / 23 - "Aguardando avaliação CGP"/ 26
            if (intStatus != (decimal)RN.ProcessoSeletivo.Status.AguardandoCGP && intStatus != (decimal)RN.ProcessoSeletivo.Status.Aguardando && intStatus != (decimal)RN.ProcessoSeletivo.Status.Inscrito)
            {
                if (eventArgs.ButtonType == ColumnCommandButtonType.Edit)
                {
                    eventArgs.Visible = false;
                }

                foreach (GridViewColumn b in grdDisciplinasHabilitacao.Columns)
                {
                    if (b is GridViewCommandColumn)
                    {
                        grdDisciplinasHabilitacao.FindHeaderTemplateControl(b, "btnInserirDisciplinas").Visible = false;
                        break;
                    }
                }
            }

        }

        protected void HabilitaPopUpInsercao(object sender, EventArgs e)
        {
            RN.ContratoTemporario.CandidatoDocente_GrupoHabilitacao rnCandidatoDocente_GrupoHabilitacao = new CandidatoDocente_GrupoHabilitacao();

            listTo.Items.Clear();
            listFrom.Items.Clear();
            listFrom.DataSource = rnCandidatoDocente_GrupoHabilitacao.ListaDisciplinasHabilitacaoPor(
                tseConcurso.Value.ToString(), tseMunicipioProc.Value.ToString(), tseRegional.Value.ToString(),
                txtCandidato.Text);
            listFrom.DataBind();

            ppcMensagem.ShowOnPageLoad = true;
        }

        protected void btnSalvarDisciplinas_Click(object sender, EventArgs e)
        {
            RN.ContratoTemporario.CandidatoDocente_GrupoHabilitacao rnCandidatoDocenteGrupoHabilitacao = new RN.ContratoTemporario.CandidatoDocente_GrupoHabilitacao();
            DataTable dtHabilitacao = new DataTable();
            RN.CandidatoDocente rnCandidatoDocente = new RN.CandidatoDocente();
            RN.ContratoTemporario.Entidades.CandidatoDocente_GrupoHabilitacao candidatoDocenteGrupoHabilitacao = new RN.ContratoTemporario.Entidades.CandidatoDocente_GrupoHabilitacao();
            int qtd = 0;

            if (listTo.Items.Count == 0)
            {
                MsgBox("É obrigatório escolher uma disciplina!", this);
                return;
            }

            try
            {
                dtHabilitacao = rnCandidatoDocente.ObtemHabilitacaoCandidatoPor(tseConcurso.Value.ToString(), txtCandidato.Text);

                for (int i = 0; i < listTo.Items.Count; i++)
                {
                    candidatoDocenteGrupoHabilitacao.Concurso = tseConcurso.Value.ToString();
                    candidatoDocenteGrupoHabilitacao.Candidato = txtCandidato.Text;
                    candidatoDocenteGrupoHabilitacao.Agrupamento = listTo.Items[i].Value.ToString();
                    candidatoDocenteGrupoHabilitacao.NomeDisciplina = listTo.Items[i].Text;

                    if (dtHabilitacao.Select("agrupamento ='" + listTo.Items[i].Value.ToString() + "'").Length == 0)
                    {
                        qtd = rnCandidatoDocenteGrupoHabilitacao.Insere(candidatoDocenteGrupoHabilitacao);
                    }
                }
            }
            catch (SqlException ex)
            {
                MsgBox(ex.Data["error"].ToString(), this);
            }

            ppcMensagem.ShowOnPageLoad = false;
            Session["listFrom"] = null;
            Session["listTo"] = null;

            grdDisciplinasHabilitacao.DataBind();

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "", "DesabilitarSubmitPopup();", true);
        }
    }
}