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
using Techne.Lyceum.RN.RecursosHumanos.DTO;
using Techne.Lyceum.RN.ContratoTemporario;
using System.Collections;
using System.Data.SqlClient;
using System.Linq;
using DevExpress.Utils;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Net;
using System.Web;
using System.Text;
using System.ComponentModel;
using System.Drawing;


namespace Techne.Lyceum.Net.ProcessoSeletivo
{
    [NavUrl("~/ProcessoSeletivo/CandidatoDocenteCH.aspx"),
    ControlText("CandidatoDocenteCH"),
    Title("Analise Inscrição"),]
    public partial class CandidatoDocenteCH : TPage
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
            TipoOperacao,
            Consultar,
            Confirmar,
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
            ControlaAcesso(grdTitulacao);
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
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Session["listFrom"] = listFrom.Items;
                Session["listTo"] = listTo.Items;
                lblMensagem.Text = string.Empty; //LIMPAR ERROS

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

                IDictionary<string, string> pares = new Dictionary<string, string>();

                pares.Add("candidato", tseCandidatoBusca.DBValue.ToString());
                pares.Add("concurso", tseConcursoBusca.DBValue.ToString());
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdExperiencia, "Experiência");
            TituloGrid(grdTitulacao, "Titulação");
            TituloGrid(grdDocumento, "Documentos");
            
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
            lblMensagem.Text = "Favor informar um processo seletivo e um número de inscrição.";
           pcCandidatoDocente.Visible = false;
            tseCandidatoBusca.ResetValue();
        }

        protected void tseCandidatoBusca_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                pnlFuncDiretor.Visible = false;

                if (!tseCandidatoBusca.DBValue.IsNull && tseCandidatoBusca.IsValidDBValue)
                {
                    // lblMensagem.Text = string.Empty;
                    _tipoOperacao = TipoOperacao.Consultar;
                }
                else
                {
                    lblMensagem.Text = "Favor informar um processo seletivo e um número de inscrição.";
                    _tipoOperacao = TipoOperacao.Inicial;
                }

                if (tseConcursoBusca["ano"].ToString() == "2024")
                {
                    pnlFuncDiretor.Visible = true;
                }

                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void HabilitaDesabilitaAbaDisciplinaHabilitacoes()
        {
            bool PossuiStatusAguardandoOuAguardandoAvaliacaoCGP = false;

      
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Inicial;

            ControlarTipoOperacao();
        }

        protected void btnProximoClick(object sender, EventArgs e)
        {
            if (grdTitulacao.VisibleRowCount == 0)
            {
               // lblMensagem.Visible = true;
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
                txtStatusCandidato.Text = "1";
                odsProcessoSeletivo.Select();
                odsProcessoSeletivo.DataBind();

                IDictionary<string, string> pares = new Dictionary<string, string>();
                pares.Add("candidato", Session["SScandidato"].ToString()); //dtCandidatoDocente.Candidato.ToString());
                pares.Add("concurso", Session["SSconcurso"].ToString());// dtCandidatoDocente.Concurso.ToString());
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
               // lblMensagem.Visible = true;
                lblMensagem.Text = "Função é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (String.IsNullOrEmpty(tseConcurso.DBValue.ToString()))
            {
               // lblMensagem.Visible = true;
                lblMensagem.Text = "Processo Seletivo é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;

            }

            if (String.IsNullOrEmpty(tseMunicipioProc.DBValue.ToString()))
            {
              //  lblMensagem.Visible = true;
                lblMensagem.Text = "Municipio do Processo é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }

            if (string.IsNullOrEmpty(tseRegional.DBValue.ToString()))
            {
              //  lblMensagem.Visible = true;
                lblMensagem.Text = "Regional é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                tseRegional.Visible = true;
                return;
            }

            if (String.IsNullOrEmpty(txtNomeCompleto.Text))
            {
               // lblMensagem.Visible = true;
                lblMensagem.Text = "Nome Completo é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (String.IsNullOrEmpty(dtDataNasc.Text))
            {
               // lblMensagem.Visible = true;
                lblMensagem.Text = "Data de Nascimento é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (String.IsNullOrEmpty(rblSexo.SelectedValue))
            {
              //  lblMensagem.Visible = true;
                lblMensagem.Text = "Sexo é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (ddlCorRaca.SelectedIndex == 0)
            {
               // lblMensagem.Visible = true;
                lblMensagem.Text = "Etnia é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (String.IsNullOrEmpty(txtNomeMae.Text))
            {
              //  lblMensagem.Visible = true;
                lblMensagem.Text = "Nome da Mãe é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (String.IsNullOrEmpty(txtNomePai.Text))
            {
              //  lblMensagem.Visible = true;
                lblMensagem.Text = "Nome do Pai é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (ddlEst_Civil.SelectedIndex == 0)
            {
               // lblMensagem.Visible = true;
                lblMensagem.Text = "Estado Civil é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (String.IsNullOrEmpty(cmbNecessidadeEspecial.SelectedItem.Text) || cmbNecessidadeEspecial.SelectedValue == "")
            {
              //  lblMensagem.Visible = true;
                lblMensagem.Text = "Necessidade Especial é campo obrigatório";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }

            if (String.IsNullOrEmpty(tseNaturalidade.DBValue.ToString()))
            {
             //   lblMensagem.Visible = true;
                lblMensagem.Text = "Naturalidade é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (ddlCotas.SelectedIndex == 0)
            {
              //  lblMensagem.Visible = true;
                lblMensagem.Text = lblMensagem.Text = "Cota é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (String.IsNullOrEmpty(txtCep.Text))
            {
              //  lblMensagem.Visible = true;
                lblMensagem.Text = "CEP é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (String.IsNullOrEmpty(tseMunicipio.DBValue.ToString()))
            {
              //  lblMensagem.Visible = true;
                lblMensagem.Text = "Município é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (String.IsNullOrEmpty(txtEstado.Value))
            {
             //   lblMensagem.Visible = true;
                lblMensagem.Text = "Estado é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (String.IsNullOrEmpty(txtEndereco.Text))
            {
              //  lblMensagem.Visible = true;
                lblMensagem.Text = "Endereço é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (String.IsNullOrEmpty(txtEndNum.Text))
            {
              //  lblMensagem.Visible = true;
                lblMensagem.Text = "Número é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (String.IsNullOrEmpty(txtBairro.Text))
            {
             //   lblMensagem.Visible = true;
                lblMensagem.Text = "Bairro é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            if (String.IsNullOrEmpty(txtCPF.Text))
            {
             //   lblMensagem.Visible = true;
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
                 //   lblMensagem.Visible = true;
                    lblMensagem.Text = "A Data de Nascimento tem que ser menor que a data de hoje.";
                    pcCandidatoDocente.ActiveTabIndex = 0;
                    return;
                }
                else if (dtDataNasc.Date.AddYears(18) > DateTime.Now || dtDataNasc.Date.AddYears(80) < DateTime.Now)
                {
                  //  lblMensagem.Visible = true;
                    lblMensagem.Text = "Data de Nascimento: O candidato docente não pode ser menor de dezoito anos ou possuir mais de 80 anos.";
                    pcCandidatoDocente.ActiveTabIndex = 0;
                    return;
                }
            }
            if (!String.IsNullOrEmpty(txtFone.Text))
            {
                if (!RN.Validacao.ValidaTelefoneComDDD(txtFone.Text.RetirarMascaraTelefone()))
                {
                  //  lblMensagem.Visible = true;
                    lblMensagem.Text = "O Telefone é inválido.";
                    pcCandidatoDocente.ActiveTabIndex = 0;
                    return;
                }
            }
            if (!String.IsNullOrEmpty(txtCelular.Text))
            {
                if (!RN.Validacao.ValidaCelularComDDD(txtCelular.Text.RetirarMascaraTelefone()))
                {
                 //   lblMensagem.Visible = true;
                    lblMensagem.Text = "O Celular é inválido.";
                    pcCandidatoDocente.ActiveTabIndex = 0;
                    return;
                }
            }
            if (!String.IsNullOrEmpty(txtEmail.Text))
            {
                if (!RN.Validacao.ValidaEmail(txtEmail.Text))
                {
                  //  lblMensagem.Visible = true;
                    lblMensagem.Text = "O Email é inválido.";
                    pcCandidatoDocente.ActiveTabIndex = 0;
                    return;
                }
            }
            if (!String.IsNullOrEmpty(txtRGNum.Text))
            {
                if (txtRGNum.Text.Length < 5 || txtRGNum.Text.Length > 15)
                {
                  //  lblMensagem.Visible = true;
                    lblMensagem.Text = "Número do documento deve possuir entre 5 e 15 dígitos.";
                    return;
                }
            }
            if (!String.IsNullOrEmpty(txtPisPasep.Text))
            {
                if (!RN.Validacao.ValidaNumerosInteirosPositivos(txtPisPasep.Text))
                {
                  //  lblMensagem.Visible = true;
                    lblMensagem.Text = "PIS/PASEP só permite números inteiros e positivos.";

                    return;
                }
                if (!RN.Validacao.ValidouPISPASEP(txtPisPasep.Text))
                {
                    //lblMensagem.Visible = true;
                    lblMensagem.Text = "PIS/PASEP é inválido.";

                    return;
                }
            }
            if (dtDataExped.Text.Trim() != string.Empty)
            {
                if (dtDataExped.Date > DateTime.Now.Date)
                {
                 //   lblMensagem.Visible = true;
                    lblMensagem.Text = "A Data de Expedição da identidade tem que ser menor que a data de hoje.";

                    return;
                }
            }
            if (dtDataExped.Text.Trim() != string.Empty)
            {
                if (dtDataExped.Date < dtDataNasc.Date)
                {
                   // lblMensagem.Visible = true;
                    lblMensagem.Text = "A Data de Expedição da identidade tem que ser maior que a data de nascimento.";

                    return;
                }
            }
            if (!String.IsNullOrEmpty(txtCrpof_Num.Text.Trim()))
            {
                if (!RN.Validacao.ValidaNumerosInteirosPositivos(txtCrpof_Num.Text.RetirarCaracteres()))
                {
                 //   lblMensagem.Visible = true;
                    lblMensagem.Text = "O número da carteira profissional só permite números inteiros e positivos.";

                    return;
                }
                if (txtCrpof_Num.Text.Length < 5 || txtCrpof_Num.Text.Length > 15)
                {
                 //   lblMensagem.Visible = true;
                    lblMensagem.Text = "O número da carteira profissional deve possuir entre 5 e 15 dígitos.";

                    return;
                }
            }
            if (dboCprof_DtExp.Text.Trim() != string.Empty)
            {
                if (dboCprof_DtExp.Date > DateTime.Now.Date)
                {
                   // lblMensagem.Visible = true;
                    lblMensagem.Text = "A Data de Expedição da carteira profissional tem que ser menor que a data de hoje.";

                    return;
                }
            }
            if (dboCprof_DtExp.Text.Trim() != string.Empty)
            {
                if (dboCprof_DtExp.Date < dtDataNasc.Date)
                {
                //    lblMensagem.Visible = true;
                    lblMensagem.Text = "A Data de Expedição da carteira profissional tem que ser maior que a data de nascimento.";

                    return;
                }
            }

            #region Validações dos Campos de Documento
            bool documentoValido, iniciouMensagem, maisDeUmCampo;
            documentoValido = true;
            iniciouMensagem = maisDeUmCampo = false;
            System.Text.StringBuilder mensagemDocumento = new System.Text.StringBuilder();
            System.Text.StringBuilder camposDocumento = new System.Text.StringBuilder();
            mensagemDocumento.Append("Documento:<br>Não é possível deixar de preencher um dos campos referentes ao tipo de documento.");
            if ((ddlRGTipoPessoa.SelectedValue == string.Empty))
            {
              //  lblMensagem.Visible = true;
                lblMensagem.Text = "Tipo de Documento é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return;
            }
            else
            {
                if (ddlRGTipoPessoa.SelectedValue == "RG")
                {
                    if (String.IsNullOrEmpty(txtRGNum.Text.Trim()))
                    {
                        documentoValido = false;
                        iniciouMensagem = true;
                        camposDocumento.Append("Número ");
                    }
                    if (cmbRGUF.SelectedValue == "")
                    {
                        documentoValido = false;
                        if (iniciouMensagem)
                        {
                            maisDeUmCampo = true;
                            camposDocumento.Append("- Estado ");
                        }
                        else
                        {
                            iniciouMensagem = true;
                            camposDocumento.Append("Estado ");
                        }
                    }
                    if (cmbRGEmissor.SelectedValue == "")
                    {
                        documentoValido = false;
                        if (iniciouMensagem)
                        {
                            maisDeUmCampo = true;
                            camposDocumento.Append("- Órgão Emissor ");
                        }
                        else
                        {
                            iniciouMensagem = true;
                            camposDocumento.Append("Órgão Emissor ");
                        }
                    }
                    if (dtDataExped.Text.Trim() == string.Empty)
                    {
                        documentoValido = false;
                        if (iniciouMensagem)
                        {
                            maisDeUmCampo = true;
                            camposDocumento.Append("- Data de Expedição ");
                        }
                        else
                        {
                            iniciouMensagem = true;
                            camposDocumento.Append("Data de Expedição ");
                        }
                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(txtRGNum.Text.Trim()))
                    {
                        documentoValido = false;
                        iniciouMensagem = true;
                        camposDocumento.Append("Número ");
                    }
                    if (cmbRGEmissor.SelectedValue == "")
                    {
                        documentoValido = false;
                        if (iniciouMensagem)
                        {
                            maisDeUmCampo = true;
                            camposDocumento.Append("- Órgão Emissor ");
                        }
                        else
                        {
                            iniciouMensagem = true;
                            camposDocumento.Append("Órgão Emissor ");
                        }
                    }
                    if (dtDataExped.Text.Trim() == string.Empty)
                    {
                        documentoValido = false;
                        if (iniciouMensagem)
                        {
                            maisDeUmCampo = true;
                            camposDocumento.Append("- Data de Expedição ");
                        }
                        else
                        {
                            iniciouMensagem = true;
                            camposDocumento.Append("Data de Expedição ");
                        }
                    }
                }
                if (!documentoValido)
                {
                    if (maisDeUmCampo)
                        mensagemDocumento.Append("<br>Campos Necessários: ");
                    else
                        mensagemDocumento.Append("<br>Campo Necessário: ");
                    mensagemDocumento.Append(camposDocumento);
                   // lblMensagem.Visible = true;
                    lblMensagem.Text = mensagemDocumento.ToString();
                    pcCandidatoDocente.ActiveTabIndex = 0;
                    return;
                }
            }
            #endregion

            #region Validações dos Campos de Documento Profissional


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

            if (!String.IsNullOrEmpty(txtCPF.Text))
            {
                if (!RN.Validacao.ValidaCpf(txtCPF.Text.RetirarMascaraCPF()))
                {
                  //  lblMensagem.Visible = true;
                    lblMensagem.Text = "O CPF do candidato é inválido.";
                    pcCandidatoDocente.ActiveTabIndex = 0;
                    return;
                }
                if (RN.CandidatoDocente.CandidatoDocenteConcursado(txtCPF.Text.RetirarMascaraCPF()))
                {
                  //  lblMensagem.Visible = true;
                    lblMensagem.Text = "O candidato já consta como professor concursado.";
                    pcCandidatoDocente.ActiveTabIndex = 0;
                    return;
                }
                if (RN.CandidatoDocente.CandidatoFuncionarioConcursado(txtCPF.Text.RetirarMascaraCPF()))
                {
                   // lblMensagem.Visible = true;
                    lblMensagem.Text = "O candidato é servidor concursado.";
                    pcCandidatoDocente.ActiveTabIndex = 0;
                    return;
                }     

               

            }
            #endregion

            #endregion

            LyCandidatoDocente dtCandidatoDocente = new LyCandidatoDocente();

            ObterDadosCandidatoDocente(dtCandidatoDocente);
            RetValue retorno = null;


            if (retorno != null)
            {
                if (!retorno.Ok)
                {
                    lblMensagem.Text = retorno.Errors.ToString();
                }
                else
                {
                     _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();

                    pcCandidatoDocente.ActiveTabIndex = 1;
                }
            }
        }

        protected void tseConcurso_Changed(object sender, EventArgs args)
        {
            RN.Concurso rnConcurso = new Concurso();

            if (!tseConcurso.DBValue.IsNull )
            {
                DataTable dt = new DataTable();
                tseCandidatoBusca.Mode = ControlMode.View;
                tseConcursoBusca.Mode = ControlMode.View;
                cmbCargo.Enabled = false;
                cmbCargo.Items.Clear();

                dt = rnConcurso.RetornaFuncaoConcuso(tseConcurso.Value.ToString());

                if (dt.Rows.Count > 0)
                {
                    foreach (System.Web.UI.WebControls.ListItem item in cmbCargo.Items)
                    {
                        if (item.Value == dt.Rows[0]["FUNCAOID"].ToString())
                        {
                            cmbCargo.SelectedValue = dt.Rows[0]["FUNCAOID"].ToString();
                            break;
                        }
                    }
                }

                pnlFuncDiretor.Visible = false;
               
                if (tseConcursoBusca["ano"].ToString() == "2024")
                {
                    pnlFuncDiretor.Visible = true;
                }

                if (string.IsNullOrEmpty(cmbCargo.SelectedValue) || cmbCargo.SelectedValue == "Selecione")
                {
                    lblMensagem.Text = "Função do Processo Seletivo não encontrada. Verifique.";
                    cmbCargo.ClearSelection();

                }
                tseMunicipioProc.Mode = ControlMode.Edit;
                tseRegional.Mode = ControlMode.View;
            }
            
        }
        protected void tseDiscIngresso_Changed(object sender, EventArgs args)
        {

        }
        protected void tseRegional_Changed(object sender, EventArgs args)
        {
            if (!tseRegional.DBValue.IsNull)
            {

                DataTable dt = new DataTable();
                tseCandidatoBusca.Mode = ControlMode.View;
                tseConcursoBusca.Mode = ControlMode.View;

            }
            
        }

        protected void tseMunicipioProc_Changed(object sender, EventArgs args)
        {

            RN.ProcessoSeletivo rnProcessoSeletivo = new Techne.Lyceum.RN.ProcessoSeletivo();
            if (Page.IsCallback)
                return;
            if (!tseMunicipioProc.DBValue.IsNull && tseMunicipioProc.IsValidDBValue)
            {

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
            decimal pontuacaoexp = RN.CandidatoDocente.ConsultarPontuacaoExperiencia(e.Parameter, tseConcursoBusca.DBValue.ToString());
            ListEditItem li = new ListEditItem(pontuacaoexp.ToString(), pontuacaoexp.ToString());
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
           string tituloTitulacao = grdTitulacao.SettingsText.Title;
            if (tituloTitulacao != string.Empty) grdTitulacao.SettingsText.Title = tituloTitulacao.Replace("|Tabela:|", "Titulações");
            string tituloExperiencia = grdExperiencia.SettingsText.Title;
            if (tituloExperiencia != string.Empty) grdExperiencia.SettingsText.Title = tituloExperiencia.Replace("|Tabela:|", "Experiências");
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
                        pcCandidatoDocente.TabPages[4].Enabled = false;
                        pcCandidatoDocente.ActiveTabIndex = 0;
                        pcCandidatoDocente.Visible = false;
                        tseConcursoBusca.ResetValue();
                        tseCandidatoBusca.ResetValue();
                        tseConcursoBusca.Mode = ControlMode.Edit;
                        tseCandidatoBusca.Mode = ControlMode.Edit;
                        
                        txtStatusCandidato.Text = string.Empty;
                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        pcCandidatoDocente.TabPages[1].Enabled = true;
                        pcCandidatoDocente.TabPages[2].Enabled = true;
                        pcCandidatoDocente.TabPages[3].Enabled = true;
                        pcCandidatoDocente.Visible = true;

                        DesabilitaCampos();
                        txtCandidato.Visible = true;
                        dtDataNasc.MaxDate = DateTime.Now.Date;
                        dtDataExped.MaxDate = DateTime.Now.Date;
                        dboCprof_DtExp.MaxDate = DateTime.Now.Date;
                       // lblMsgCandidato.Visible = false;
                        tseConcursoBusca.Mode = ControlMode.Edit;
                        tseCandidatoBusca.Mode = ControlMode.Edit;
                        break;
                    }

               
                case TipoOperacao.Confirmar:
                    {
                       
                      //  Response.Redirect("CandidatoDocenteCH.aspx");
                        
                        break;
                    }
               
                case TipoOperacao.Consultar:
                    {
                        pcCandidatoDocente.TabPages[1].Enabled = true;
                        pcCandidatoDocente.TabPages[2].Enabled = true;
                        pcCandidatoDocente.TabPages[3].Enabled = true;
                        pcCandidatoDocente.TabPages[4].Enabled = true;
                        
                        txtCandidato.Visible = true;
                       // lblMsgCandidato.Visible = false;
                       // lblMensagem.Text = string.Empty;

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
                            tseConcursoBusca.Mode = ControlMode.Edit;
                            tseCandidatoBusca.Mode = ControlMode.Edit;
                            pcCandidatoDocente.ActiveTabIndex = 0;

                            break;
                        }

                        if (dadosCandidatoDocente == null)
                        {
                            lblMensagem.Text = "Candidato não encontrado";
                            pcCandidatoDocente.Visible = false;
                        }
                        else
                        {                         
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

                            RN.RecursosHumanos.DocenteCandidato rnDocenteCandidato = new Techne.Lyceum.RN.RecursosHumanos.DocenteCandidato();

                            dtDadosCandidatoDocente = rnDocenteCandidato.ObtemPor( dadosCandidatoDocente.Candidato, dadosCandidatoDocente.Concurso);

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
              
                        break;
                    }
            }

            HabilitaDesabilitaAbaDisciplinaHabilitacoes();
        }



        private void CarregaNecessidadeEspecial()
        {
            RN.NecessidadeEspecial.NecessidadeEspecial rnNecessidadeEspecial = new RN.NecessidadeEspecial.NecessidadeEspecial();
            System.Web.UI.WebControls.ListItem itemVazio = new System.Web.UI.WebControls.ListItem("Selecione", string.Empty);

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
            if (long.TryParse(dadosCandidatoDocente.Rows[0]["Celular"].ToString().RetirarMascaraTelefone(), out resultado))
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
             txtStatusCandidato.Text = "";
            // txtStatusCandidato.Text = Convert.ToString(dadosCandidatoDocente.Rows[0]["Status"]);
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
            
               //PreencherDadoCombo(ddlCotas, Convert.ToString(dadosCandidatoDocente.Rows[0]["COTAIDINSCRICAO"]));

            if (!string.IsNullOrEmpty(dadosCandidatoDocente.Rows[0]["Concurso"].ToString()))
                tseConcurso.DBValue = dadosCandidatoDocente.Rows[0]["Concurso"].ToString();
            txtCandidato.Text = dadosCandidatoDocente.Rows[0]["Candidato"].ToString();

            Session["SSconcurso"] = dadosCandidatoDocente.Rows[0]["Concurso"].ToString();
            Session["SScandidato"] = dadosCandidatoDocente.Rows[0]["Candidato"].ToString();
            //Session["SScategoria"] = dadosCandidatoDocente.Rows[0]["Categoria"].ToString();

            //ControlarObrigatoriedadeDisciplinaIngresso(dadosCandidatoDocente.Rows[0]["Categoria"].ToString());
            cmbCargo.Items.Clear();


            PreencheCmbCargo(dadosCandidatoDocente.Rows[0]["Concurso"].ToString());

            if (!string.IsNullOrEmpty(dadosCandidatoDocente.Rows[0]["MUNICIPIOCODIGO"].ToString()))
            {
                tseMunicipio.DBValue = dadosCandidatoDocente.Rows[0]["MUNICIPIOCODIGO"].ToString();
                if (tseMunicipio.IsValidDBValue & !tseMunicipio.DBValue.IsNull)
                    txtEstado.Value = tseMunicipio["uf_sigla"].ToString();
            }

            if (!string.IsNullOrEmpty(dadosCandidatoDocente.Rows[0]["End_municipio"].ToString()))
            {
                tseMunicipioProc.DBValue = dadosCandidatoDocente.Rows[0]["End_municipio"].ToString();
                //if (tseMunicipioProc.IsValidDBValue & !tseMunicipio.DBValue.IsNull)
                //    txtEstado.Value = tseMunicipio["uf_sigla"].ToString();
            }

            

            if (!string.IsNullOrEmpty(dadosCandidatoDocente.Rows[0]["Municipio_nasc"].ToString()))
            {
                tseNaturalidade.DBValue = dadosCandidatoDocente.Rows[0]["Municipio_nasc"].ToString();
                if (tseNaturalidade.IsValidDBValue & !tseNaturalidade.DBValue.IsNull)
                    txtNaturalidadeUF.Text = tseNaturalidade["uf_sigla"].ToString();

            }


            //Lotacao

            txtLotacaoRegionalSede.Text = dadosCandidatoDocente.Rows[0]["REGIONALDESCRICAO"].ToString();
            txtLotacaoMunicipio.Text = dadosCandidatoDocente.Rows[0]["MUNICIPIODESCRICAO"].ToString();
            txtLotacaoDisciplinaIngresso.Text = dadosCandidatoDocente.Rows[0]["DISCIPLINAINGRESSODESCRICAO"].ToString();
            rblLotacao.Text = dadosCandidatoDocente.Rows[0]["ACUMULACAO"].ToString();
            TxtLotacaoGLP.Text = dadosCandidatoDocente.Rows[0]["QUANTIDADEANOSGLP"].ToString();

            //Processo Seletivo
            txtDataApresentacao.Text = dadosCandidatoDocente.Rows[0]["DTAPRESENTACAO"].ToString();
            txtHoraApresentacao.Text = dadosCandidatoDocente.Rows[0]["HORAAPRESENTACAO"].ToString();
            decimal pontuacaotitulacao = RN.RecursosHumanos.DocenteCandidatoTitulacao.ConsultarPontuacaoTitulacaoCandidato(tseCandidatoBusca.DBValue.ToString(), tseConcursoBusca.DBValue.ToString());
            decimal pontuacaoExperiencia = RN.RecursosHumanos.DocenteCandidatoExperiencia.ConsultarPontuacaoExperienciaCandidato(tseCandidatoBusca.DBValue.ToString(), tseConcursoBusca.DBValue.ToString());
            
            //O VALOR DO CAMPO ESTA EM UM ENNUM EM RN.ProcessoSeletivo.Status
            // ddlTipo.Value = dadosCandidatoDocente.Rows[0]["SITUACAO"].ToString();
             //Titulacao
             RdlFuncaoDiretor.Text = dadosCandidatoDocente.Rows[0]["FUNCAODIRETOR"].ToString();

             if (dadosCandidatoDocente.Rows[0]["FUNCAODIRETOR"].ToString() == "True")
                 txtPontuacao.Text = (pontuacaotitulacao + pontuacaoExperiencia + 1 + Convert.ToDecimal(Convert.ToInt32(TxtLotacaoGLP.Text) * 0.5)).ToString();
             else
                 txtPontuacao.Text = (pontuacaotitulacao + pontuacaoExperiencia + Convert.ToDecimal(Convert.ToInt32(TxtLotacaoGLP.Text) * 0.5)).ToString();

            tseRegional.DBValue = dadosCandidatoDocente.Rows[0]["regionalid"].ToString() == null ? "0" : dadosCandidatoDocente.Rows[0]["regionalid"].ToString();

            
        }
        protected void btnDetalhes_Command(object sender, CommandEventArgs e)
        {
            try
            {
                hdnDocenteCandidatoTipoArquivoId.Value = string.Empty;
                hdnDocenteCandidatoArquivoId.Value = string.Empty;

                string[] chave = e.CommandArgument.ToString().Split(new char[] { ',' });

                hdnDocenteCandidatoTipoArquivoId.Value = chave[1].ToString();
                hdnDocenteCandidatoArquivoId.Value = chave[0].ToString();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        protected void btnVisualizar_Command(object sender, CommandEventArgs e)
        {
            try
            {

                string embed = string.Empty;
                bimgArquivo.Visible = false;
                ltEmbed.Visible = false;
                DataTable dados = new DataTable();

                string[] chave = e.CommandArgument.ToString().Split(new char[] { ',' });

                if (!chave[1].ToString().IsNullOrEmptyOrWhiteSpace())
                {
                    pucVisualizarArquivo.ShowOnPageLoad = true;

                    if (chave[1].ToString() == "application/pdf")
                    {
                        embed = " <object data=\"{0}{1}\"";
                        embed += "type=\"application/pdf\" width=\"100%\" height=\"100%\">";
                        embed += "<iframe   src=\"{0}{1}\"  width=\"100%\"   height=\"100%\"";
                        embed += "style=\"border: none;\">    <p>Your browser does not support PDFs.";
                        embed += "<a href=\"{0}{1}\">Download the PDF</a>.</p>";
                        embed += "</iframe></object>";
                        ltEmbed.Text = string.Format(embed, ResolveUrl("~/PrestacaoContas/FileCS.ashx?Tabela=DocenteCandidatoArquivo&Id="), chave[0].ToString());
                        ltEmbed.Visible = true;
                        pucVisualizarArquivo.Width = Unit.Pixel(880);
                        pucVisualizarArquivo.Height = Unit.Pixel(580);
                    }
                    else
                    {
                        pucVisualizarArquivo.Width = Unit.Pixel(350);
                        pucVisualizarArquivo.Height = Unit.Pixel(350);
                        RN.RecursosHumanos.DocenteCandidatoArquivo rnDocenteCandidatoArquivo = new RN.RecursosHumanos.DocenteCandidatoArquivo();
                        bimgArquivo.ContentBytes = rnDocenteCandidatoArquivo.ObtemDocumentoPor(Convert.ToInt32(chave[0]));
                        bimgArquivo.Visible = true;
                    }
                }
                else
                {

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Não existe documento para visualização');", true);
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }
        protected void btnBaixar_Click(object sender, CommandEventArgs e)
        {
            try
            {
                string embed = string.Empty;
                bimgArquivo.Visible = false;
                ltEmbed.Visible = false;
                DataTable dados = new DataTable();

                string[] chave = e.CommandArgument.ToString().Split(new char[] { ',' });

                RN.RecursosHumanos.DocenteCandidatoArquivo rnDocenteCandidatoArquivo = new RN.RecursosHumanos.DocenteCandidatoArquivo();

                RN.RecursosHumanos.Entidades.DocenteCandidatoArquivo listDocenteCandidatoArquivo = rnDocenteCandidatoArquivo.ObtemArquivoPor(Convert.ToInt32(chave[0]));

                byte[] arquivo = rnDocenteCandidatoArquivo.ObtemDocumentoPor(Convert.ToInt32(chave[0]));
                string nomearquivo = listDocenteCandidatoArquivo.NomeArquivo.ToString();
                string tipoarquivo = listDocenteCandidatoArquivo.TipoArquivo.ToString();
                if (!chave[1].ToString().IsNullOrEmptyOrWhiteSpace())
                {
                    pucVisualizarArquivo.ShowOnPageLoad = true;

                    if (chave[1].ToString() == "application/pdf")
                    {
                        WebClient req = new WebClient();
                        HttpResponse response = HttpContext.Current.Response;
                        response.Clear();
                        response.ClearContent();
                        response.ClearHeaders();
                        response.Buffer = true;
                        response.ContentType = "application/pdf";
                        response.AddHeader("Content-Disposition", "attachment;filename=\"" + nomearquivo + "\"");

                        response.BinaryWrite(arquivo);
                        response.End();
                    }
                    else
                    {
                       WebClient req = new WebClient();
                        HttpResponse response = HttpContext.Current.Response;
                        response.Clear();
                        response.ClearContent();
                        response.ClearHeaders();
                        response.Buffer = true;
                        response.ContentType = "image/jpeg";
                        response.AddHeader("Content-Disposition", "attachment;filename=\"" + nomearquivo + "\"");

                        response.BinaryWrite(arquivo);
                        response.End();
                    }
                }
                else
                {

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Não existe documento para download');", true);
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
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
                        dadosDrop = rnEtnia.ConsultarEtnia();
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
            System.Web.UI.WebControls.ListItem itemVazio = new System.Web.UI.WebControls.ListItem("<Selecione>", "");
            lista.Items.Insert(0, itemVazio);

            
                if (lista == ddlNacionalidade)
                {
                    System.Web.UI.WebControls.ListItem listItem = lista.Items.FindByText("BRASILEIRA");
                    if (listItem != null)
                    {
                        lista.ClearSelection();
                        listItem.Selected = true;
                    }
                }
         
            if (valorPadrao != "")
            {
                System.Web.UI.WebControls.ListItem itemPadrao = lista.Items.FindByText(valorPadrao);
                if (itemPadrao != null)
                {
                    lista.ClearSelection();
                    itemPadrao.Selected = true;
                }
                else
                {
                    lista.Items.Add(new System.Web.UI.WebControls.ListItem(valorPadrao, string.Empty));
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
            grdTitulacao.CancelEdit();
            grdExperiencia.CancelEdit();
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

        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
        }
        #endregion

        #region Grid Tituações e Experiências
        private string SomarPontuacao()
        {
            object somaTit = grdTitulacao.GetTotalSummaryValue(grdTitulacao.TotalSummary[0]);
            object somaExp = grdExperiencia.GetTotalSummaryValue(grdExperiencia.TotalSummary[0]);

            int somaTitu = 0;
            int somaExpe = 0;

            if (somaTit != null && somaExp != null)
            {
                int.TryParse(somaTit.ToString(), out somaTitu);
                int.TryParse(somaExp.ToString(), out somaExpe);
            }

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
            if (!grdTitulacao.IsEditing || e.Column.FieldName != "pontuacaoexp")
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
        }
        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {

                RN.RecursosHumanos.DocenteCandidato rnDocenteCandidato = new Techne.Lyceum.RN.RecursosHumanos.DocenteCandidato();
                ValidacaoDados validacao = new ValidacaoDados();

                if (ddlTipo.Value != null)
                {
                    validacao = rnDocenteCandidato.ValidaAnalise(Convert.ToInt32(tseCandidatoBusca.DBValue.ToString()), Convert.ToInt32(ddlTipo.Value.ToString()), User.Identity.Name.ToString());

                    if (validacao.Valido)
                    {
                        rnDocenteCandidato.AnaliseCandidato(tseConcurso.Value.ToString(), tseCandidatoBusca.DBValue.ToString(), ddlTipo.Value.ToString(), User.Identity.Name);
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "msgBox('Processo Realizado com Sucesso');", true);
                                                                                               
                    }
                    else
                    {
                        MsgBox(validacao.Mensagem, this);
                    }
                }
                else
                {
                    MsgBox("A Situação do candidato não foi definida!", this);

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
           
        }
        protected void grdProcessoSeletivo_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {

        }
        #endregion

        public static void MsgBox(string Mensagem, Page page)
        {
            ScriptManager.RegisterStartupScript(page.Page, page.Page.GetType(), "popup", "Validamsg('" + Mensagem + "');", true);

            //var script = @"alert('" + Mensagem + @"');";
            //page.Page.ClientScript.RegisterStartupScript(page.Page.GetType(), "popup", "Validamsg('" + Mensagem + "');", true);
            //return;
        }


        protected void grdDisciplinasHabilitacao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs eventArgs)
        {
            eventArgs.NewValues.Add("concurso", Session["SSconcurso"].ToString());
            eventArgs.NewValues.Add("candidato", Session["SScandidato"].ToString());
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

        public DataTable ListaCandidatoExperiencia(object Concurso, object Candidato)
        {
            RN.CandidatoDocExperiencias rnCandidatoDocExperiencia = new RN.CandidatoDocExperiencias();
            return rnCandidatoDocExperiencia.ListaCandidatoExperiencia(Concurso.ToString(), Candidato.ToString());
        }

        public DataTable ListaCandidatoTitulacoes(object Concurso, object Candidato)
        {
            RN.CandidatoDocTitulacoes rnCandidatoDocTitulacoes = new RN.CandidatoDocTitulacoes();
            return rnCandidatoDocTitulacoes.ListaCandidatoTitulacoes(Concurso.ToString(), Candidato.ToString());
        }

        protected void btnExportarPDF_Click(object sender, ImageClickEventArgs e)
        {
            ExportaPdf pdf = new ExportaPdf();
       

            try 
            {
                 iTextSharp.text.Rectangle papel;
                 papel = PageSize.A4;
               
                 MemoryStream outputStream = new MemoryStream();

                 //Monta e Define a Saida do PDF Unificado
                 WebClient req = new WebClient();
                 HttpResponse response = HttpContext.Current.Response;
                 response.Clear();
                 response.ClearContent();
                 response.ClearHeaders();
                 response.Buffer = true;
                 response.ContentType = "application/pdf";
                 response.AddHeader("Content-Disposition", "attachment;filename=\"Documento.pdf");
                 //Cria documento
                 RN.RecursosHumanos.DocenteCandidatoArquivo rnDocenteCandidatoArquivo = new RN.RecursosHumanos.DocenteCandidatoArquivo();

                 List<int> ListaArquivos      = new List<int>();
                 List<byte[]> ListaDocumentos = new List<byte[]>();
                 List<byte[]> ListaImagens    = new List<byte[]>();
                 byte[] juntapdf = null;

                 //MONTA ARQUIVO DE IMAGEM EM UM UNICO PDF
                 ListaArquivos = rnDocenteCandidatoArquivo.ObtemArquivoPorDocente(tseCandidatoBusca.DBValue.ToString(), tseConcursoBusca.DBValue.ToString());

                 foreach (var listaArquivo in ListaArquivos)
                     {
                         RN.RecursosHumanos.Entidades.DocenteCandidatoArquivo listDocenteCandidatoArquivo = rnDocenteCandidatoArquivo.ObtemArquivoPor(listaArquivo);

                         MemoryStream ms = new MemoryStream(listDocenteCandidatoArquivo.Arquivo as byte[]);
                         //Adiciona ao pdf caso o pdf seja um jpeg
                         if (listDocenteCandidatoArquivo.TipoArquivo.Equals("image/jpeg") || listDocenteCandidatoArquivo.TipoArquivo.Equals("image/png"))
                                     ListaImagens.Add(listDocenteCandidatoArquivo.Arquivo as byte[]);  
                         else        ListaDocumentos.Add(listDocenteCandidatoArquivo.Arquivo as byte[]);                               
                             
                     }      

                 if (ListaDocumentos.Count != 0) //Caso Exista PDF 
                   {
                        juntapdf = MergePDFs(ListaDocumentos); //Junta o PDF
                        //Insere as Imagens no PDF
                        for (int k = 0; k < ListaImagens.Count; k++)
                             juntapdf = MergeImagePdf(ListaImagens[k], juntapdf); //Adiciona as imagens no PDF
                    }
                        else { //Se somente existir imagem                          
                            Document docPdf = new Document(PageSize.A4, 10f, 10f, 10f, 0f);                            
                            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                            {
                            
                              PdfWriter writer = PdfWriter.GetInstance(docPdf, memoryStream);
                              docPdf.Open();
                              for (int k = 0; k < ListaImagens.Count; k++)
                                {
                                        Stream stream = new MemoryStream(ListaImagens[k]);
                                        System.Drawing.Image imgOriginal = System.Drawing.Image.FromStream(stream);
                                    
                                        iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(imgOriginal, System.Drawing.Imaging.ImageFormat.Jpeg);
                                        pic.ScaleToFit(600f, 300f);
                                        pic.Alignment = Element.ALIGN_JUSTIFIED;

                                        docPdf.NewPage();
                                        docPdf.Add(pic);
                                      
                                }

                              docPdf.Close();
                              juntapdf = memoryStream.ToArray();  
                            }
                   }

                 response.BinaryWrite(juntapdf);

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }            
        }
    
    public static byte[] MergeImagePdf(byte[] imgFiles, byte[] file)
        {
            using (var ms = new MemoryStream())
            {
                var pdf = new PdfReader(file);
                var doc = new Document(pdf.GetPageSizeWithRotation(1));
                using (var writer = PdfWriter.GetInstance(doc, ms))
                {
                    doc.Open();

                    for (int page = 0; page < pdf.NumberOfPages; page++)
                    {
                        doc.SetPageSize(pdf.GetPageSizeWithRotation(page + 1));
                        doc.NewPage();
                        var pg = writer.GetImportedPage(pdf, page + 1);
                        int rotation = pdf.GetPageRotation(page + 1);
                        if (rotation == 90 || rotation == 270)
                            writer.DirectContent.AddTemplate(
                                pg, 0, -1f, 1f, 0, 0, pdf.GetPageSizeWithRotation(page).Height);
                        else
                            writer.DirectContent.AddTemplate(pg, 1f, 0, 0, 1f, 0, 0);
                    }

                        doc.NewPage();
                        iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(imgFiles);
                        pdfImage.Alignment = Element.ALIGN_CENTER;
                        pdfImage.ScaleToFit(doc.PageSize.Width - 10, doc.PageSize.Height - 10);
                        doc.Add(pdfImage);

                    doc.Close();
                    writer.Close();
                }
                ms.Flush();
                return ms.GetBuffer();
            }
        }

    public static byte[] MergePDFs(List<byte[]> pdfFiles)
    {  
        if (pdfFiles.Count > 1)
        {
            PdfReader finalPdf;
            Document pdfContainer;
            PdfWriter pdfCopy;
            MemoryStream msFinalPdf = new MemoryStream();

            finalPdf = new PdfReader(pdfFiles[0]);
            pdfContainer = new Document();
            pdfCopy = new PdfSmartCopy(pdfContainer, msFinalPdf);

            pdfContainer.Open();
            //Adicionar PDF
            for (int k = 0; k < pdfFiles.Count; k++)
            {
                finalPdf = new PdfReader(pdfFiles[k]);
                for (int i = 1; i < finalPdf.NumberOfPages + 1; i++)
                {
                    ((PdfSmartCopy)pdfCopy).AddPage(pdfCopy.GetImportedPage(finalPdf, i));
                }

            }
             
            pdfCopy.FreeReader(finalPdf);

            finalPdf.Close();
            pdfCopy.Close();
            pdfContainer.Close();

            return msFinalPdf.ToArray();
        }
        else if (pdfFiles.Count == 1)
        {
            return pdfFiles[0];
        }
        return null;
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


            ScriptManager.RegisterClientScriptBlock(this, GetType(), "", "DesabilitarSubmitPopup();", true);
        }
    }
}