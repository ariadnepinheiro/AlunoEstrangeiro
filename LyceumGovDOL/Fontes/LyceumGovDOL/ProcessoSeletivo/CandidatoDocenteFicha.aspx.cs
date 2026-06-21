using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using System.Data;
using Techne.Controls;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using Seeduc.Infra.Helpers;
using DevExpress.Web.Data;
using Techne.Lyceum.RN.ContratoTemporario;
using Techne.Lyceum.RN.Entidades;
using System.Linq;

namespace Techne.Lyceum.Net.ProcessoSeletivo
{
    [NavUrl("~/ProcessoSeletivo/CandidatoDocenteFicha.aspx"),
    ControlText("CandidatoDocenteFicha"),
    Title("Ficha de Inscrição"),]

    public partial class CandidatoDocenteFicha : TPage
    {
        RN.Entidades.LyCandidatoDocente ecandidatoDocente = new RN.Entidades.LyCandidatoDocente();

        public CandidatoDocenteFicha()
        {
            VerificarCompatibilidadeComIE = true;
            MensagemCompatibilidadeIE = @"Prezado candidato, a inscrição para a contratação temporária poderá ser efetivada 
                                        utilizando o seu navegador padrão. <br />Caso queira imprimir o comprovante de inscrição 
                                      posteriormente, você poderá retornar ao site da SEEDUC e, clicar em Imprimir o comprovante de inscrição.";
        }

        public RN.Entidades.LyCandidatoDocente EcandidatoDocente
        {
            get { return ecandidatoDocente; }
            set { ecandidatoDocente = value; }
        }


        //==================================================================================================================================//

        RN.Entidades.LyCandidatoDocExperiencias ecandidatoExperiencia = new Techne.Lyceum.RN.Entidades.LyCandidatoDocExperiencias();
        public RN.Entidades.LyCandidatoDocExperiencias EcandidatoExperiencia
        {
            get { return ecandidatoExperiencia; }
            set { ecandidatoExperiencia = value; }
        }

        //==================================================================================================================================//

        RN.Entidades.LyCandidatoDocTitulacoes ecandidatoTitulo = new Techne.Lyceum.RN.Entidades.LyCandidatoDocTitulacoes();
        public RN.Entidades.LyCandidatoDocTitulacoes EcandidatoTitulo
        {
            get { return ecandidatoTitulo; }
            set { ecandidatoTitulo = value; }
        }

        //==================================================================================================================================//

        RN.ContratoTemporario.Entidades.CandidatoDocente_GrupoHabilitacao ecandidatoDocenteGrupoHabilitacao = new RN.ContratoTemporario.Entidades.CandidatoDocente_GrupoHabilitacao();

        public RN.ContratoTemporario.Entidades.CandidatoDocente_GrupoHabilitacao EcandidatoDocenteGrupoHabilitacao
        {
            get { return ecandidatoDocenteGrupoHabilitacao; }
            set { ecandidatoDocenteGrupoHabilitacao = value; }
        }

        //==================================================================================================================================//

        public string SenhaCandidato
        {
            get
            {
                var senha = ViewState["SenhaCandidato"];

                if (senha != null)
                {
                    return senha.ToString();
                }

                return string.Empty;
            }

            set
            {
                ViewState["SenhaCandidato"] = value;
            }
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);

            Master.AtribuirMensagemCompatibilidadeIE(MensagemCompatibilidadeIE);
            Master.AtribuirModalOKClick(ModalOK_Click);
        }

        protected void ModalOK_Click(object sender, EventArgs e)
        {
            OcultarPopupModal();
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
            Sucesso,
            Finalizado
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

        private string AREA_INTEGRADA_DOCII = "039";
        #endregion

        #region Eventos da página
        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            Page.Header.Controls.AddAt(0, new HtmlMeta { HttpEquiv = "X-UA-Compatible", Content = "IE=8" });
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdExperiencia, "Experiência na SEEDUC (escolher apenas 1(uma) opção)");
            TituloGrid(grdExperienciaFora, "Experiência fora da SEEDUC (escolher apenas 1(uma) opção)");
            TituloGrid(grdTitulacao, "Titulação");
            TituloGrid(grdDisciplina, "Disciplinas");

            if (!string.IsNullOrEmpty(QueryStringDecodificada["usuario"]))
            {
                tseConcurso.SqlWhere = string.Empty;
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string textoJavaScript = string.Empty;

                if (!IsPostBack)
                {
                    CarregaDropEtnia();
                }
                else
                {
                    textoJavaScript = string.Concat("function AbrePopupImpressao(queryString){",
                        string.Concat("window.open('../Relatorio/Relatorios.aspx?report=rsconcursodocenteexterno&grp=processoseletivo&' + queryString,'','directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes');}"));

                    ClientScript.RegisterClientScriptBlock(this.GetType(), "AbrePopupImpressao", textoJavaScript, true);
                    btnImprimir.Attributes.Add("onclick", "AbrePopupImpressao");
                }

                if (!string.IsNullOrEmpty(QueryStringDecodificada["usuario"]))
                {
                    string candCon = CandidatoDocente.ConsultaCandidato(QueryStringDecodificada["usuario"]);
                    //tseCandidatoBusca.DBValue = candCon.Split('|')[0];
                    tseConcurso.DBValue = candCon.Split('|')[1];
                    if (!CandidatoDocente.ExisteConcursoConsulta(tseConcurso.DBValue.ToString()))
                    {
                        Response.Write("Não existe concurso ativo!");
                        Response.Redirect("~/Manutencao/ConcursoExpirou.aspx");
                    }
                    if (!IsPostBack)
                    {
                        pcCandidatoDocente.ActiveTabIndex = 0;
                    }

                    _tipoOperacao = TipoOperacao.Consultar;
                    ControlarTipoOperacao();
                }
                else
                {
                    if (!CandidatoDocente.ExisteConcursoAtivo())
                    {
                        Response.Write("Não existe concurso ativo!");
                        Response.Redirect("~/Manutencao/ConcursoNaoValido.aspx");
                    }

                    if (!IsPostBack)
                    {
                        hdnTitulacao.Value = "0";
                        hdnExperiencia.Value = "0";
                        //para a primeira vez que a página é carregada o tipo de operação será inicial
                        _tipoOperacao = TipoOperacao.Novo;
                        ControlarTipoOperacao();
                        pcCandidatoDocente.ActiveTabIndex = 0;
                    }
                }


                lblMensagem2.Text = lblMensagem.Text = string.Empty; //LIMPAR ERROS
                //TituloGrid();
                if (lblCoordMunic.Visible)
                    tseRegional.Visible = true;

                if (RN.CandidatoDocente.PeriodoInscricaoEncerrado(tseConcurso.DBValue.ToString()))
                {
                    grdTitulacao.Columns[0].Visible = false;
                    grdExperiencia.Columns[0].Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }
        #endregion

        #region Eventos
        protected void cmbPaises_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimparEndereco();
        }

        protected void ddlRGTipoPessoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*if (ddlRGTipoPessoa.SelectedValue == "RG")
            {
                rfvEstadoRG.Enabled = true;
                lblRG_UF.Text = "Estado*: ";
                lblRG_UF.Font.Bold = true;
            }
            else
            {
                rfvEstadoRG.Enabled = false;
                lblRG_UF.Text = "Estado: ";
                lblRG_UF.Font.Bold = false;
            }*/
        }

        protected void tseMunicipio_Changed(object sender, EventArgs args)
        {
            try
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
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true; lblMensagem2.Visible = true;

            }
        }
        protected void tseMunicipioProc_Changed(object sender, EventArgs args)
        {
            try
            {
                //DataTable dtnucleo = new DataTable();

                if (!tseConcurso.DBValue.IsNull && !tseMunicipioProc.DBValue.IsNull)
                {
                    PnlDadosPessoais.Enabled = true;
                    pnlContatos.Enabled = true;
                    pnlDocumentos.Enabled = true;
                    pnlEndereco.Enabled = true;
                }

                if (Page.IsCallback)
                    return;
                if (!tseMunicipioProc.DBValue.IsNull && tseMunicipioProc.IsValidDBValue)
                {                    
                    //dtnucleo = new Techne.Lyceum.RN.ProcessoSeletivo().ListaCoordenadoriaPor(tseMunicipioProc.DBValue.ToString(), tseConcurso.DBValue.ToString());

                    //tseRegional.Visible = true;
                    //lblCoordMunic.Visible = true;

                    //if (dtnucleo.Rows.Count <= 1)
                    //{
                    //    hdnNucleo.Value = dtnucleo.Rows[0]["NUCLEO"].ToString();
                    //}
                }
                else
                {
                    tseMunicipioProc.ResetValue();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true; lblMensagem2.Visible = true;

            }
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

        protected void btnProximoDadosPessoais_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarAba1())
                {
                    return;
                }

                pcCandidatoDocente.TabPages[0].Enabled = false;
                pcCandidatoDocente.TabPages[1].Enabled = true;
                pcCandidatoDocente.ActiveTabIndex = 1;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true; lblMensagem2.Visible = true;

            }
        }

        protected void btnAnteriorTelaDadosPessoais_Click(object sender, EventArgs e)
        {
            try
            {
                pcCandidatoDocente.TabPages[1].Enabled = true;
                pcCandidatoDocente.TabPages[2].Enabled = false;
                pcCandidatoDocente.ActiveTabIndex = 1;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true; lblMensagem2.Visible = true;

            }
        }

        protected void btnProximoTitulacao_Click(object sender, EventArgs e)
        {
            try
            {
                if (_tipoOperacao != TipoOperacao.Consultar)
                {
                    if (this.SalvarOuAtualizar())
                    {
                        pcCandidatoDocente.TabPages[2].Enabled = false;
                        pcCandidatoDocente.TabPages[3].Enabled = true;
                        pcCandidatoDocente.ActiveTabIndex = 3;
                        ControlarTipoOperacao();
                    }
                }
                else
                {
                    pcCandidatoDocente.ActiveTabIndex = 3;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true; lblMensagem2.Visible = true;

            }
        }

        protected void btnAnteriorDocumentos_Click(object sender, EventArgs e)
        {
            try
            {
                pcCandidatoDocente.TabPages[1].Enabled = false;
                pcCandidatoDocente.ActiveTabIndex = 0;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true; lblMensagem2.Visible = true;

            }
        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (SalvarOuAtualizar() == true)
                {
                    var candidato = EcandidatoDocente.Candidato;
                    var concurso = EcandidatoDocente.Concurso;

                    var retorno = CandidatoDocente.Finalizar(concurso, candidato);

                    if (retorno != null
                        && !retorno.Ok)
                    {
                        lblMensagem2.Text = lblMensagem.Text = retorno.Errors.ToString();
                        return;
                    }
                    else
                    {
                        lblMensagem2.Text = lblMensagem.Text = "Inscrição efetuada com sucesso!";
                        lblMensagem.Visible = true; lblMensagem2.Visible = true;
                        _tipoOperacao = TipoOperacao.Finalizado;
                        ControlarTipoOperacao();
                        ProtegeTodosCampos(this);

                        Imprimir();
                    }
                }
                else
                {
                    //Em caso de qualquer erro limpar o codigo digitado
                    txtChave.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true; lblMensagem2.Visible = true;

            }
        }

        private bool ValidarAba1()
        {
            string erro = string.Empty;
            Validacao validacao = null;
            var nucleo = string.Empty;

            if (String.IsNullOrEmpty(tseConcurso.DBValue.ToString()))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Processo Seletivo é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return false;
            }
            if (String.IsNullOrEmpty(cmbCargo.SelectedItem.Value))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Função é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return false;
            }
            if (tseConcurso.DBValue.ToString() == "24º Processo" || tseConcurso.DBValue.ToString() == "22º Processo")
            {
                if (String.IsNullOrEmpty(tseRegional.DBValue.ToString()) && lblCoordMunic.Visible)
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "Coordenadoria é campo obrigatório.";
                    pcCandidatoDocente.ActiveTabIndex = 0;
                    return false;
                }
            }
            if (String.IsNullOrEmpty(tseMunicipioProc.DBValue.ToString()))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Municipio do Processo é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return false;
            }

            if (tseRegional.Visible && String.IsNullOrEmpty(tseRegional.DBValue.ToString()))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Regional é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 0;
                return false;
            }

            if (lblCoordMunic.Visible)
                nucleo = tseRegional.DBValue.ToString();
            else
                nucleo = hdnNucleo.Value;

            #region Validações Gerais
            validacao = new Validacao();
            erro = validacao.ValidaNomeProprio("Nome Completo", txtNomeCompleto.Text);

            if (!string.IsNullOrEmpty(erro))
            {
                validacao = null;

                lblMensagem.Visible = true;
                lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = erro;
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            else
            {
                txtNomeCompleto.Text = RetiraDuplosEspacosEmBrancoPorRegEx(txtNomeCompleto.Text);
            }

            erro = validacao.ValidaNomeProprio("Nome da Mãe", txtNomeMae.Text);

            if (!string.IsNullOrEmpty(erro))
            {
                validacao = null;

                lblMensagem.Visible = true;
                lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = erro;
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            else
            {
                txtNomeMae.Text = RetiraDuplosEspacosEmBrancoPorRegEx(txtNomeMae.Text);
            }

            erro = validacao.ValidaNomeProprio("Nome do Pai", txtNomePai.Text);

            /*if (!string.IsNullOrEmpty(erro))
            {
                validacao = null;

                lblMensagem.Visible = true;
                lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = erro;
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            else
            {
                txtNomePai.Text = RetiraDuplosEspacosEmBrancoPorRegEx(txtNomePai.Text);
            }*/

            if (validacao != null)
                validacao = null;

            if (String.IsNullOrEmpty(txtEmail.Text))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "E-mail é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }

            if (!string.IsNullOrEmpty(txtEmail.Text))
            {
                if (!Validacao.Email(txtEmail.Text))
                {
                    lblMensagem2.Text = lblMensagem.Text = "O campo E-mail está em um formato incorreto!";
                    return false;
                }
            }

            /*if (String.IsNullOrEmpty(txtFone.Text))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Telefone é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }*/
            if (String.IsNullOrEmpty(txtCelular.Text))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Celular é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }

            if (String.IsNullOrEmpty(dtDataNasc.Text))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Data de Nascimento é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            if (String.IsNullOrEmpty(rblSexo.SelectedValue))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Sexo é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            if (cmbNecessidadeEspecial.SelectedValue == "")
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = "Necessidade Especial é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            if (String.IsNullOrEmpty(txtNomeMae.Text))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Nome da Mãe é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            /*if (String.IsNullOrEmpty(txtNomePai.Text))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Nome do Pai é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }*/
            if (ddlEst_Civil.SelectedIndex == 0)
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Estado Civil é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            if (String.IsNullOrEmpty(tseNaturalidade.DBValue.ToString()))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Naturalidade é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            else
            {
                if (!RN.Validacao.IsNumeric(tseNaturalidade.DBValue.ToString()))
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "Naturalidade deve ter somente números positivos.";
                    pcCandidatoDocente.ActiveTabIndex = 1;
                    return false;

                }
                if (!RN.Endereco.VerificarMunicipio(tseNaturalidade.DBValue.ToString()))
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "Naturalidade inválida.";
                    pcCandidatoDocente.ActiveTabIndex = 1;
                    return false;
                }
            }
            if (ddlPaisNasc.SelectedIndex == 0)
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "País é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            if (ddlNacionalidade.SelectedIndex == 0)
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Nacionalidade é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            if (String.IsNullOrEmpty(txtCep.Text))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "CEP é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            else
            {
                if (txtCep.Text.Trim().Length != 8)
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "CEP deve ter digítos.";
                    pcCandidatoDocente.ActiveTabIndex = 1;
                    return false;
                }

                if (!RN.Validacao.IsNumeric(txtCep.Text.Trim()))
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "CEP deve ter somente números positivos.";
                    pcCandidatoDocente.ActiveTabIndex = 1;
                    return false;

                }
            }
            if (String.IsNullOrEmpty(tseMunicipio.DBValue.ToString()))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Município é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            else
            {
                if (!RN.Validacao.IsNumeric(tseMunicipio.DBValue.ToString()))
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "Município deve ter somente números positivos.";
                    pcCandidatoDocente.ActiveTabIndex = 1;
                    return false;

                }
                if (!RN.Endereco.VerificarMunicipio(tseMunicipio.DBValue.ToString()))
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "Município de nascimento inválido.";
                    pcCandidatoDocente.ActiveTabIndex = 1;
                    return false;
                }
            }
            if (String.IsNullOrEmpty(txtEstado.Value))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Estado é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            if (String.IsNullOrEmpty(txtEndereco.Text))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Endereço é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            if (String.IsNullOrEmpty(txtEndNum.Text))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Número é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            if (String.IsNullOrEmpty(txtBairro.Text))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Bairro é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            else
            {

                bool bairroOk = RN.Validacao.ValidaBairro(txtBairro.Text);
                if (!bairroOk)
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "Bairro possui caracteres inválido.";
                    pcCandidatoDocente.ActiveTabIndex = 1;
                    return false;
                }

            }
            if (String.IsNullOrEmpty(txtCPF.Text))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "CPF é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 2;
                return false;
            }
            #endregion

            #region Validações de Campos Gerais

            if (!String.IsNullOrEmpty(dtDataNasc.Text))
            {
                if (dtDataNasc.Date > DateTime.Now)
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "A Data de Nascimento tem que ser menor que a data de hoje.";
                    pcCandidatoDocente.ActiveTabIndex = 1;
                    return false;
                }
                else if (dtDataNasc.Date.AddYears(18) > DateTime.Now || dtDataNasc.Date.AddYears(80) < DateTime.Now)
                {
                    lblMensagem.Visible = true;
                    lblMensagem.Text = "Data de Nascimento: O candidato docente não pode ser menor de dezoito anos ou possuir mais de 80 anos.";
                    pcCandidatoDocente.ActiveTabIndex = 1;
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(txtFone.Text))
            {
                if (!RN.Validacao.ValidaTelefoneComDDD(txtFone.Text.RetirarMascaraTelefone()))
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "O Telefone é inválido.";
                    pcCandidatoDocente.ActiveTabIndex = 1;
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(txtCelular.Text))
            {
                if (!RN.Validacao.ValidaCelularComDDD(txtCelular.Text.RetirarMascaraTelefone()))
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "O Celular é inválido.";
                    pcCandidatoDocente.ActiveTabIndex = 1;
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(txtEmail.Text))
            {
                if (!RN.Validacao.ValidaEmail(txtEmail.Text))
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "O Email é inválido.";
                    pcCandidatoDocente.ActiveTabIndex = 1;
                    return false;
                }
                if (!ValidarEmail(txtEmail.Text))
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "O Email é inválido.";
                    pcCandidatoDocente.ActiveTabIndex = 1;
                    return false;
                }

            }
            if (!String.IsNullOrEmpty(txtRGNum.Text))
            {
                if (ddlRGTipoPessoa.Text == "RG")
                {
                    if (!RN.Validacao.ValidaNumerosInteirosPositivos(txtRGNum.Text))
                    {
                        lblMensagem.Visible = true; lblMensagem2.Visible = true;
                        lblMensagem2.Text = lblMensagem.Text = "O campo Número(documento) só permite números inteiros e positivos.";
                        pcCandidatoDocente.ActiveTabIndex = 2;
                        return false;
                    }
                }
                if (txtRGNum.Text.Length < 5 || txtRGNum.Text.Length > 15)
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "Número do documento deve possuir entre 5 e 15 dígitos.";
                    pcCandidatoDocente.ActiveTabIndex = 2;
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(txtPisPasep.Text))
            {
                if (!RN.Validacao.ValidaNumerosInteirosPositivos(txtPisPasep.Text))
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "PIS/PASEP só permite números inteiros e positivos.";
                    pcCandidatoDocente.ActiveTabIndex = 2;
                    return false;
                }
            }
            if (dtDataExped.Text.Trim() != string.Empty)
            {
                if (dtDataExped.Date > DateTime.Now.Date)
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "A Data de Expedição da identidade tem que ser menor que a data de hoje.";
                    pcCandidatoDocente.ActiveTabIndex = 2;
                    return false;
                }
            }
            if (dtDataExped.Text.Trim() != string.Empty)
            {
                if (dtDataExped.Date < dtDataNasc.Date)
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "A Data de Expedição da identidade tem que ser maior que a data de nascimento.";
                    pcCandidatoDocente.ActiveTabIndex = 2;
                    return false;
                }
            }

            #region Validações dos Campos de Documento

            /*bool documentoValido, iniciouMensagem, maisDeUmCampo;
            documentoValido = true;
            iniciouMensagem = maisDeUmCampo = false;
            System.Text.StringBuilder mensagemDocumento = new System.Text.StringBuilder();
            System.Text.StringBuilder camposDocumento = new System.Text.StringBuilder();
            mensagemDocumento.Append("Documento:<br>Não é possível deixar de preencher um dos campos referentes ao tipo de documento.");
            if ((ddlRGTipoPessoa.SelectedIndex == 0))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Tipo de Documento é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 2;
                return false;
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
                    if (cmbRGUF.SelectedIndex == 0)
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
                    if (cmbRGEmissor.SelectedIndex == 0)
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
                    if (cmbRGEmissor.SelectedIndex == 0)
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
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = mensagemDocumento.ToString();
                    pcCandidatoDocente.ActiveTabIndex = 2;
                    return false;
                }
            }
             */
            #endregion

            #endregion
        

            return true;
        }

        private string RetiraDuplosEspacosEmBrancoPorRegEx(string nome)
        {
            Regex regex = new Regex(@"\s{2,}");
            return regex.Replace(nome.Trim().ToUpper(), " ");
        }

        private bool ValidarAbaDisciplinas()
        {
            if (grdDisciplina.VisibleRowCount > 0)
            {
                List<object> fieldValues = grdDisciplina.GetSelectedFieldValues(new string[] { "agrupamento", "descricao" });

                if (fieldValues.Count == 0)
                {
                    lblMensagem2.Text = lblMensagem.Text = "Ficha de inscrição não foi salva, favor escolher uma ou mais disciplinas de habilitação.";
                    pcCandidatoDocente.ActiveTabIndex = 2;
                    return false;
                }
            }
            else
            {
                lblMensagem2.Text = lblMensagem.Text = "Não existem disciplinas, favor pesquisar novamente.";
                pcCandidatoDocente.ActiveTabIndex = 2;
                return false;
            }

            return true;
        }

        private bool ValidarAbaTitulacaoExperiencia()
        {
            List<string> mensagens = new List<string>();
            string captchaGerado = string.Empty;
            List<object> fieldValues = grdTitulacao.GetSelectedFieldValues(new string[] { "descricao", "pontuacao" });

            if (fieldValues.Count == 0)
            {
                mensagens.Add("A titulação é obrigatória.");
            }

            if (string.IsNullOrEmpty(txtChave.Text))
            {

                mensagens.Add("O CÓDIGO DA IMAGEM é de preenchimento obrigatório.");
                txtChave.Text = string.Empty;
            }
            else
            {
                if (HttpContext.Current.Response.Cookies["CaptchaValue"] != null)
                {
                    captchaGerado = HttpContext.Current.Request.Cookies["CaptchaValue"].Value;
                }

                // Valida Captcha
                if (this.txtChave.Text != captchaGerado)
                {
                    txtChave.Text = string.Empty;
                    mensagens.Add("Código digitado incorreto. Digite-o novamente.");
                }
            }

            List<object> fieldExperiencia = grdExperiencia.GetSelectedFieldValues(new string[] { "experiencia", "pontuacao" });

            if (fieldExperiencia.Count > 1)
            {
                mensagens.Add("Somente pode escolher 1 opção para Experiência na SEEDUC.");
            }

            List<object> fieldExperienciaFora = grdExperienciaFora.GetSelectedFieldValues(new string[] { "experiencia", "pontuacao" });

            if (fieldExperienciaFora.Count > 1)
            {
                mensagens.Add("Somente pode escolher 1 opção para Experiência fora da SEEDUC.");
            }


            if (mensagens.Count > 0)
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = mensagens.Aggregate((x, y) => x + Environment.NewLine + y).Replace(Environment.NewLine, "<br />");
                pcCandidatoDocente.ActiveTabIndex = 3;
                return false;
            }

            return true;
        }

        protected void ddlPaisNasc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPaisNasc.SelectedItem.Text != "BRASIL")
            {
                //tseNaturalidade.Visible = false;
                //txtNaturalidadeNasc.Visible = true;
                //txtNaturalidadeUF.ReadOnly = false;
                //txtNaturalidadeUF.Text = string.Empty;
            }
            else
            {
                //tseNaturalidade.Visible = true;
                //txtNaturalidadeNasc.Visible = false;
                //txtNaturalidadeUF.ReadOnly = true;
            }
        }

        #endregion

        private Boolean SalvarOuAtualizar()
        {
            DataTable dtHabilitacao = new DataTable();
            RN.CandidatoDocente rnCandidatoDocente = new RN.CandidatoDocente();
            RN.ContratoTemporario.CandidatoDocente_GrupoHabilitacao rnCandidatoDocente_GrupoHabilitacao = new Techne.Lyceum.RN.ContratoTemporario.CandidatoDocente_GrupoHabilitacao();

            try
            {
                if (ValidarTela() == true)
                {
                    if (ValidarAbaTitulacaoExperiencia() == false)
                    {
                        return false;
                    }

                    ObterDadosCandidatoDocente(EcandidatoDocente);
                    Session.Add("SSconcurso", EcandidatoDocente.Concurso);
                    Session.Add("SScandidato", EcandidatoDocente.Candidato);
                    EcandidatoDocente.Status = 1;
                    RetValue retorno = null;

                    int ret = new RN.CandidatoDocente().Insere(EcandidatoDocente);
                    if (ret == 0)
                    {
                        retorno.Errors.Add("Erro ao Incluir o Cadastro");
                    }

                    List<object> fieldValues = grdDisciplina.GetSelectedFieldValues(new string[] { "agrupamento", "descricao" });

                    if (fieldValues.Count > 0)
                    {
                        dtHabilitacao = rnCandidatoDocente.ObtemHabilitacaoCandidatoPor(Session["SSconcurso"].ToString(), Session["SScandidato"].ToString());

                        foreach (object[] item in fieldValues)
                        {
                            EcandidatoDocenteGrupoHabilitacao.Concurso = Session["SSconcurso"].ToString();
                            EcandidatoDocenteGrupoHabilitacao.Candidato = Session["SScandidato"].ToString();
                            EcandidatoDocenteGrupoHabilitacao.Agrupamento = item[0].ToString();

                            if (dtHabilitacao.Select("agrupamento ='" + item[0].ToString() + "'").Length == 0)
                            {
                                ret = rnCandidatoDocente_GrupoHabilitacao.Insere(EcandidatoDocenteGrupoHabilitacao);
                                if (ret == 0)
                                {
                                    retorno.Errors.Add("Erro ao Incluir Disciplina");
                                }
                            }
                        }
                    }

                    List<object> fieldTitulacao = grdTitulacao.GetSelectedFieldValues(new string[] { "titulacao", "descricao" });

                    foreach (object[] item in fieldTitulacao)
                    {
                        LyCandidatoDocTitulacoes dadosTitulacao = new LyCandidatoDocTitulacoes();

                        dadosTitulacao.Concurso = EcandidatoDocenteGrupoHabilitacao.Concurso;
                        dadosTitulacao.Candidato = EcandidatoDocenteGrupoHabilitacao.Candidato;
                        dadosTitulacao.Titulacao = item[0].ToString();

                        ret = new RN.CandidatoDocTitulacoes().Insere(dadosTitulacao);
                        if (ret == 0)
                        {
                            retorno.Errors.Add("Erro ao Incluir Titulação");
                        }
                    }

                    List<object> fieldExperiencia = grdExperiencia.GetSelectedFieldValues(new string[] { "experiencia", "pontuacao" });


                    foreach (object[] item in fieldExperiencia)
                    {
                        LyCandidatoDocExperiencias dadosExperiencia = new LyCandidatoDocExperiencias();

                        dadosExperiencia.Concurso = EcandidatoDocenteGrupoHabilitacao.Concurso;
                        dadosExperiencia.Candidato = EcandidatoDocenteGrupoHabilitacao.Candidato;
                        dadosExperiencia.Experiencia = item[0].ToString();

                        ret = new RN.CandidatoDocExperiencias().Insere(dadosExperiencia);
                        if (ret == 0)
                        {
                            retorno.Errors.Add("Erro ao Incluir Experiência na Seeduc.");
                        }
                    }


                    List<object> fieldExperienciaFora = grdExperienciaFora.GetSelectedFieldValues(new string[] { "experiencia", "pontuacao" });


                    foreach (object[] item in fieldExperienciaFora)
                    {
                        LyCandidatoDocExperiencias dadosExperiencia = new LyCandidatoDocExperiencias();

                        dadosExperiencia.Concurso = EcandidatoDocenteGrupoHabilitacao.Concurso;
                        dadosExperiencia.Candidato = EcandidatoDocenteGrupoHabilitacao.Candidato;
                        dadosExperiencia.Experiencia = item[0].ToString();

                        ret = new RN.CandidatoDocExperiencias().Insere(dadosExperiencia);
                        if (ret == 0)
                        {
                            retorno.Errors.Add("Erro ao Incluir Experiência fora da Seeduc.");
                        }
                    }


                    if (retorno != null)
                    {
                        if (!retorno.Ok)
                        {
                            lblMensagem2.Text = lblMensagem2.Text = lblMensagem.Text = retorno.Errors.ToString();
                            return false;
                        }
                        else
                        {
                            ControlarTipoOperacao();

                        }
                    }
                    else
                    {
                        pcCandidatoDocente.ActiveTabIndex = 0;
                    }

                    _tipoOperacao = TipoOperacao.Sucesso;

                    tseConcurso.Mode = ControlMode.View;
                    tseMunicipioProc.Mode = ControlMode.View;
                    grdTitulacao.DataBind();
                    grdExperiencia.DataBind();
                }
                else
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                lblMensagem2.Text = lblMensagem2.Text = lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                return false;
            }
        }

        private Boolean ValidarTela()
        {
            RN.CandidatoDocente rnCandidatoDocente = new CandidatoDocente();

            #region Validações Server

            #region Validações dos Campos Obrigatórios
            if (!ValidarAba1())
            {
                pcCandidatoDocente.ActiveTabIndex = 0;
                return false;
            }

            if (String.IsNullOrEmpty(txtNomeCompleto.Text))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Nome Completo é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            if (String.IsNullOrEmpty(txtEmail.Text))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "E-mail é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }

            if (txtNomeCompleto.Text.Trim() != string.Empty)
            {
                Regex regex = new Regex(@"\s{2,}");
                string NomeCompl = regex.Replace(txtNomeCompleto.Text.Trim().ToUpper(), " ");
                Match nome = Regex.Match(NomeCompl.TrimStart(), @"^[aA-zZà-úÀ-Ú]+((\s[aA-zZà-úÀ-Ú]+)+)?$");

                if (!nome.Success)
                {
                    lblMensagem.Text = "Nome inválido";
                    return false;
                }
                else
                {
                    txtNomeCompleto.Text = nome.Value;
                }
            }

            if (!string.IsNullOrEmpty(txtEmail.Text))
            {
                if (!Validacao.Email(txtEmail.Text))
                {
                    lblMensagem2.Text = lblMensagem.Text = "O campo E-mail está em um formato incorreto!";
                    return false;
                }
            }

            /*if (String.IsNullOrEmpty(txtFone.Text))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Telefone é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }*/
            if (String.IsNullOrEmpty(txtCelular.Text))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Celular é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            if (String.IsNullOrEmpty(dtDataNasc.Text))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Data de Nascimento é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            if (String.IsNullOrEmpty(rblSexo.SelectedValue))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Sexo é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            if (cmbNecessidadeEspecial.SelectedValue == "")
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = "Necessidade Especial é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            if (String.IsNullOrEmpty(txtNomeMae.Text))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Nome da Mãe é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            /*if (String.IsNullOrEmpty(txtNomePai.Text))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Nome do Pai é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }*/
            if (ddlEst_Civil.SelectedIndex == 0)
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Estado Civil é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            if (String.IsNullOrEmpty(tseNaturalidade.DBValue.ToString()))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Naturalidade é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            else
            {
                if (!RN.Validacao.IsNumeric(tseNaturalidade.DBValue.ToString()))
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "Naturalidade deve ter somente números positivos.";
                    pcCandidatoDocente.ActiveTabIndex = 1;
                    return false;

                }
                if (!RN.Endereco.VerificarMunicipio(tseNaturalidade.DBValue.ToString()))
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "Naturalidade inválida.";
                    pcCandidatoDocente.ActiveTabIndex = 1;
                    return false;
                }
            }
            if (ddlPaisNasc.SelectedIndex == 0)
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "País é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            if (ddlNacionalidade.SelectedIndex == 0)
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Nacionalidade é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            if (ddlCotas.SelectedIndex == 0)
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Cota é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            if (String.IsNullOrEmpty(txtCep.Text))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "CEP é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            else
            {
                if (txtCep.Text.Trim().Length != 8)
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "CEP deve ter digítos.";
                    pcCandidatoDocente.ActiveTabIndex = 1;
                    return false;
                }

                if (!RN.Validacao.IsNumeric(txtCep.Text.Trim()))
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "CEP deve ter somente números positivos.";
                    pcCandidatoDocente.ActiveTabIndex = 1;
                    return false;

                }
            }
            if (String.IsNullOrEmpty(tseMunicipio.DBValue.ToString()))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Município é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            else
            {
                if (!RN.Validacao.IsNumeric(tseMunicipio.DBValue.ToString()))
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "Município deve ter somente números positivos.";
                    pcCandidatoDocente.ActiveTabIndex = 1;
                    return false;

                }
                if (!RN.Endereco.VerificarMunicipio(tseMunicipio.DBValue.ToString()))
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "Município de nascimento inválido.";
                    pcCandidatoDocente.ActiveTabIndex = 1;
                    return false;
                }
            }
            if (String.IsNullOrEmpty(txtEstado.Value))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Estado é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            if (String.IsNullOrEmpty(txtEndereco.Text))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Endereço é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            if (String.IsNullOrEmpty(txtEndNum.Text))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Número é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            if (String.IsNullOrEmpty(txtBairro.Text))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Bairro é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 1;
                return false;
            }
            else
            {

                bool bairroOk = RN.Validacao.ValidaBairro(txtBairro.Text);
                if (!bairroOk)
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "Bairro possui caracteres inválido.";
                    pcCandidatoDocente.ActiveTabIndex = 1;
                    return false;
                }

            }
            if (String.IsNullOrEmpty(txtCPF.Text))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "CPF é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 2;
                return false;
            }
            #endregion

            #region Validações de Campos Gerais
            if (!String.IsNullOrEmpty(dtDataNasc.Text))
            {
                if (dtDataNasc.Date > DateTime.Now)
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "A Data de Nascimento tem que ser menor que a data de hoje.";
                    pcCandidatoDocente.ActiveTabIndex = 1;
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(txtFone.Text))
            {
                if (!RN.Validacao.ValidaTelefoneComDDD(txtFone.Text.RetirarMascaraTelefone()))
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "O Telefone é inválido.";
                    pcCandidatoDocente.ActiveTabIndex = 1;
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(txtCelular.Text))
            {
                if (!RN.Validacao.ValidaCelularComDDD(txtCelular.Text.RetirarMascaraTelefone()))
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "O Celular é inválido.";
                    pcCandidatoDocente.ActiveTabIndex = 1;
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(txtEmail.Text))
            {
                if (!RN.Validacao.ValidaEmail(txtEmail.Text))
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "O Email é inválido.";
                    pcCandidatoDocente.ActiveTabIndex = 1;
                    return false;
                }
                if (!ValidarEmail(txtEmail.Text))
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "O Email é inválido.";
                    pcCandidatoDocente.ActiveTabIndex = 1;
                    return false;
                }

            }
            if (!String.IsNullOrEmpty(txtRGNum.Text))
            {
                //if (ddlRGTipoPessoa.Text == "RG")
                //{
                //    if (!RN.Validacao.ValidaNumerosInteirosPositivos(txtRGNum.Text))
                //    {
                //        lblMensagem.Visible = true; lblMensagem2.Visible = true;
                //        lblMensagem2.Text = lblMensagem.Text = "O campo Número(documento) só permite números inteiros e positivos.";
                //        pcCandidatoDocente.ActiveTabIndex = 2;
                //        return false;
                //    }
                //}
                if (txtRGNum.Text.Length < 5 || txtRGNum.Text.Length > 15)
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "Número do documento deve possuir entre 5 e 15 dígitos.";
                    pcCandidatoDocente.ActiveTabIndex = 2;
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(txtPisPasep.Text))
            {
                if (!RN.Validacao.ValidaNumerosInteirosPositivos(txtPisPasep.Text))
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "PIS/PASEP só permite números inteiros e positivos.";
                    pcCandidatoDocente.ActiveTabIndex = 2;
                    return false;
                }
            }
            if (dtDataExped.Text.Trim() != string.Empty)
            {
                if (dtDataExped.Date > DateTime.Now.Date)
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "A Data de Expedição da identidade tem que ser menor que a data de hoje.";
                    pcCandidatoDocente.ActiveTabIndex = 2;
                    return false;
                }
            }
            if (dtDataExped.Text.Trim() != string.Empty)
            {
                if (dtDataExped.Date < dtDataNasc.Date)
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "A Data de Expedição da identidade tem que ser maior que a data de nascimento.";
                    pcCandidatoDocente.ActiveTabIndex = 2;
                    return false;
                }
            }

            #region Validações dos Campos de Documento
            /*bool documentoValido, iniciouMensagem, maisDeUmCampo;
            documentoValido = true;
            iniciouMensagem = maisDeUmCampo = false;
            System.Text.StringBuilder mensagemDocumento = new System.Text.StringBuilder();
            System.Text.StringBuilder camposDocumento = new System.Text.StringBuilder();
            mensagemDocumento.Append("Documento:<br>Não é possível deixar de preencher um dos campos referentes ao tipo de documento.");
            if ((ddlRGTipoPessoa.SelectedIndex == 0))
            {
                lblMensagem.Visible = true; lblMensagem2.Visible = true;
                lblMensagem2.Text = lblMensagem.Text = "Tipo de Documento é campo obrigatório.";
                pcCandidatoDocente.ActiveTabIndex = 2;
                return false;
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
                    if (cmbRGUF.SelectedIndex == 0)
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
                    if (cmbRGEmissor.SelectedIndex == 0)
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
                    if (cmbRGEmissor.SelectedIndex == 0)
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
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = mensagemDocumento.ToString();
                    pcCandidatoDocente.ActiveTabIndex = 2;
                    return false;
                }
            }*/
            #endregion
            #endregion
            #endregion

            #region Validações Particulares
            if (_tipoOperacao.Equals(TipoOperacao.Novo) && !String.IsNullOrEmpty(tseConcurso.DBValue.ToString()))
            {
                if (RN.CandidatoDocente.PeriodoInscricaoEncerrado(tseConcurso.DBValue.ToString()))
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "Período de inscrição encerrado para o processo seletivo '" + tseConcurso.DBValue.ToString() + "'.";
                    pcCandidatoDocente.ActiveTabIndex = 0;
                    return false;
                }
            }

            if (!String.IsNullOrEmpty(txtCPF.Text))
            {
                if (!RN.Validacao.ValidaCpf(txtCPF.Text.RetirarMascaraCPF()))
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "O CPF do candidato é inválido.";
                    return false;
                }
                if (RN.CandidatoDocente.CandidatoDocenteConcursado(txtCPF.Text.RetirarMascaraCPF()))
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "O candidato já consta como professor concursado.";
                    return false;
                }
                if (RN.CandidatoDocente.CandidatoFuncionarioConcursado(txtCPF.Text.RetirarMascaraCPF()))
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "O candidato é servidor concursado.";
                    return false;
                }
                if (Convert.ToString(tseConcurso["indigena"]) == "N")
                {
                    if (RN.CandidatoDocente.CandidatoConcursadoTemporario(txtCPF.Text.RetirarMascaraCPF()))
                    {
                        lblMensagem.Visible = true; lblMensagem2.Visible = true;
                        lblMensagem2.Text = lblMensagem.Text = "Candidato obteve contrato temporário com a SEEDUC nos últimos 12 meses. Inscrição não permitida.";
                        return false;
                    }
                }
                else
                {
                    if (rnCandidatoDocente.PodeInscricaoIndigena(txtCPF.Text.RetirarMascaraCPF()))
                    {
                        lblMensagem.Visible = true; lblMensagem2.Visible = true;
                        lblMensagem2.Text = lblMensagem.Text = "Candidato obteve contrato temporário com a SEEDUC nos últimos 30 dias. Inscrição não permitida.";
                        return false;
                    }
                }

                if (_tipoOperacao.Equals(TipoOperacao.Novo) &&
                    RN.CandidatoDocente.ExisteCPFConcurso(Convert.ToString(tseConcurso.DBValue), txtCPF.Text.RetirarMascaraCPF()))
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "O CPF do candidato já está cadastrado neste concurso.";
                    return false;
                }
                if (_tipoOperacao.Equals(TipoOperacao.Novo) &&
                    RN.CandidatoDocente.ExisteCPFConcurso(Convert.ToString(tseConcurso.DBValue), txtCandidato.Text, txtCPF.Text.RetirarMascaraCPF()))
                {
                    lblMensagem.Visible = true; lblMensagem2.Visible = true;
                    lblMensagem2.Text = lblMensagem.Text = "O CPF do candidato já está cadastrado neste concurso.";
                    return false;
                }
            }
            #endregion

            if (pcCandidatoDocente.ActiveTabIndex == 2 || pcCandidatoDocente.ActiveTabIndex == 3)
            {
                if (ValidarAbaDisciplinas() == false)
                {
                    return false;
                }
            }

            return true;
        }

        protected void tseConcurso_Changed(object sender, EventArgs args)
        {
            try
            {
                RN.Concurso rnConcurso = new Concurso();
                DataTable dt = new DataTable();

                if (!tseConcurso.DBValue.IsNull && _tipoOperacao == TipoOperacao.Novo)
                {
                    cmbCargo.Enabled = false;
                    cmbCargo.Items.Clear();
                    PreencheCmbCargo(tseConcurso.DBValue.ToString());
                    tseRegional.Mode = ControlMode.View;
                    odsTitulacao.Select();

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
                }
                else if (_tipoOperacao == TipoOperacao.Novo)
                {
                    cmbCargo.Enabled = false;
                    cmbCargo.Items.Clear();
                    cmbCargo.SelectedIndex = -1;
                    tseRegional.Mode = ControlMode.View;
                    odsTitulacao.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true; lblMensagem2.Visible = true;

            }
        }

        protected void tseRegional_Changed(object sender, EventArgs args)
        {
            try
            {
                DataTable dt = new DataTable();
                RN.Concurso rnConcurso = new Concurso();

                if ((!tseRegional.DBValue.IsNull || !tseRegional.DBValue.IsNull) && _tipoOperacao == TipoOperacao.Novo)
                {
                    if (lblCoordMunic.Visible)
                        tseRegional.Visible = true;

                    //var nucleo = tseRegional.DBValue.ToString() != "" ? tseRegional.DBValue.ToString() : string.Empty;

                    dt = rnConcurso.RetornaFuncaoConcuso(tseConcurso.Value.ToString());

                    if (dt.Rows.Count > 0)
                    {
                        cmbCargo.SelectedValue = dt.Rows[0]["FUNCAOID"].ToString();
                        //hdnNucleo.Value = nucleo;
                    }

                    if (string.IsNullOrEmpty(cmbCargo.SelectedValue) || cmbCargo.SelectedValue == "Selecione")
                    {
                        lblMensagem.Text = "Função do Processo Seletivo não encontrada. Verifique.";
                        cmbCargo.Enabled = false;
                        cmbCargo.ClearSelection();
                        return;
                    }
                }
                else if (_tipoOperacao == TipoOperacao.Novo)
                {
                    cmbCargo.Enabled = false;
                    cmbCargo.Items.Clear();
                    cmbCargo.SelectedIndex = -1;
                }
                tseRegional.Visible = true;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true; lblMensagem2.Visible = true;

            }
        }

        protected void cmbCargo_Changed(object sender, EventArgs args)
        {
            try
            {
                if (!cmbCargo.SelectedItem.Value.Equals(string.Empty) && _tipoOperacao == TipoOperacao.Novo)
                {
                    tseRegional.Mode = ControlMode.Edit;

                    //var nucleo = RN.Coordenadoria.ObterCoordenadoriaMunic(Convert.ToString(tseMunicipioProc.DBValue));
                }
                else if (_tipoOperacao == TipoOperacao.Novo)
                {
                    tseRegional.Mode = ControlMode.View;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true; lblMensagem2.Visible = true;

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
            try
            {
                decimal pontuacao = CandidatoDocente.ConsultarPontuacaoTitulacao(e.Parameter, tseConcurso.DBValue.ToString());
                ListEditItem li = new ListEditItem(pontuacao.ToString(), pontuacao.ToString());
                (source as ASPxComboBox).Items.Clear();
                (source as ASPxComboBox).Items.Add(li);
                (source as ASPxComboBox).SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true; lblMensagem2.Visible = true;

            }
        }

        private void cmbExperienciaPontuacao_OnCallback(object source, CallbackEventArgsBase e)
        {
            try
            {
                decimal pontuacao = CandidatoDocente.ConsultarPontuacaoExperiencia(e.Parameter, tseConcurso.DBValue.ToString());
                ListEditItem li = new ListEditItem(pontuacao.ToString(), pontuacao.ToString());
                (source as ASPxComboBox).Items.Clear();
                (source as ASPxComboBox).Items.Add(li);
                (source as ASPxComboBox).SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true; lblMensagem2.Visible = true;

            }
        }

        #region Métodos

        private void ObterDadosCandidatoDocente(RN.Entidades.LyCandidatoDocente dadosCandidatoDocente)
        {

            string nome = txtNomeCompleto.Text.TrimEnd().EndsWith(".") ? txtNomeCompleto.Text.TrimEnd().Substring(0, txtNomeCompleto.Text.TrimEnd().Length - 1) : txtNomeCompleto.Text.TrimEnd();

            dadosCandidatoDocente.Nome = nome.Trim().ToUpper();
            dadosCandidatoDocente.Dt_nasc = dtDataNasc.Date;
            dadosCandidatoDocente.Sexo = rblSexo.Text;
            dadosCandidatoDocente.Nome_mae = txtNomeMae.Text;
            dadosCandidatoDocente.Nome_pai = txtNomePai.Text;
            dadosCandidatoDocente.Estado_civil = ddlEst_Civil.SelectedValue;
            dadosCandidatoDocente.CotaIdInscricao = Convert.ToInt32(ddlCotas.SelectedValue);
            dadosCandidatoDocente.Cep = txtCep.Text;
            dadosCandidatoDocente.End_municipio = tseMunicipio.DBValue.ToString();
            dadosCandidatoDocente.Endereco = txtEndereco.Text;
            dadosCandidatoDocente.End_num = txtEndNum.Text;
            dadosCandidatoDocente.End_compl = txtEndCompl.Text;
            dadosCandidatoDocente.End_pais = "1";//--BRASIL
            dadosCandidatoDocente.Bairro = txtBairro.Text;
            dadosCandidatoDocente.Fone = txtFone.Text.RetirarMascaraTelefone();
            dadosCandidatoDocente.Celular = txtCelular.Text.RetirarMascaraTelefone();
            dadosCandidatoDocente.E_mail = txtEmail.Text;
            dadosCandidatoDocente.Pis_pasep = txtPisPasep.Text;
            dadosCandidatoDocente.EtniaId = Convert.ToInt32(ddlEtnia.SelectedItem.Value);
            dadosCandidatoDocente.Rg_tipo = ddlRGTipoPessoa.SelectedValue;
            dadosCandidatoDocente.Rg_num = txtRGNum.Text;
            dadosCandidatoDocente.Rg_uf = cmbRGUF.SelectedValue;
            dadosCandidatoDocente.Rg_emissor = cmbRGEmissor.SelectedValue;
            dadosCandidatoDocente.Cpf = txtCPF.Text.RetirarMascaraCPF();
            dadosCandidatoDocente.Status = 26;
            dadosCandidatoDocente.NecessidadeEspecialId = !cmbNecessidadeEspecial.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(cmbNecessidadeEspecial.SelectedValue) : (int?)null;

            if (dtDataExped.Value != null)
            {
                dadosCandidatoDocente.Rg_dtexp = (DateTime)dtDataExped.Value;
            }
            else
            {
                dadosCandidatoDocente.Rg_dtexp = null;
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
            {
                dadosCandidatoDocente.Municipio_proc = tseMunicipioProc.DBValue.ToString();
            }

            dadosCandidatoDocente.RegionalId = Convert.ToInt32(tseRegional.DBValue.ToString());

            if (!tseNaturalidade.DBValue.IsNull)
            {
                dadosCandidatoDocente.Municipio_nasc = tseNaturalidade.DBValue.ToString();
            }
            if (_tipoOperacao == TipoOperacao.Novo)
            {
                dadosCandidatoDocente.Candidato = RN.CandidatoDocente.GeraCandidato();
                txtStatusCandidato.Text = "1";
                lblMsgCandidato.Text = dadosCandidatoDocente.Candidato;
                txtCandidato.Text = dadosCandidatoDocente.Candidato;
            }
            else
            {
                dadosCandidatoDocente.Candidato = txtCandidato.Text;
                dadosCandidatoDocente.Status = Convert.ToDecimal(txtStatusCandidato.Text);
            }

        }

        private void LimparEnderecoNaturalidade()
        {
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
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        btnImprimir.Visible = true;

                        pcCandidatoDocente.ActiveTabIndex = 0;
                        pcCandidatoDocente.Visible = false;
                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        pcCandidatoDocente.Visible = true;
                        txtCandidato.Visible = true;
                        dtDataNasc.MaxDate = DateTime.Now.Date;
                        dtDataExped.MaxDate = DateTime.Now.Date;
                        lblMsgCandidato.Visible = false;

                        break;
                    }

                case TipoOperacao.Novo:
                    {
                        btnImprimir.Visible = false;
                        pcCandidatoDocente.Visible = true;
                        LimparTela();
                        HabilitaCampos();
                        txtCandidato.Visible = false;
                        dtDataNasc.MaxDate = DateTime.Now.Date;
                        dtDataExped.MaxDate = DateTime.Now.Date;
                        lblMsgCandidato.Visible = true;
                        CarregaNecessidadeEspecial();
                        CarregarDadosDrop(ddlEst_Civil.ID);
                        CarregarDadosDrop(ddlPaisNasc.ID);
                        CarregarDadosDrop(ddlNacionalidade.ID);
                        CarregarDadosDrop(ddlRGTipoPessoa.ID);
                        CarregarDadosDrop(cmbRGUF.ID);
                        CarregarDadosDrop(cmbRGEmissor.ID);
                        CarregarDadosDrop(ddlCotas.ID);

                        cmbCargo.Enabled = false;
                        tseRegional.Mode = ControlMode.Edit;
                        pcCandidatoDocente.ActiveTabIndex = 0;

                        DesabilitarCampos();

                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        btnImprimir.Visible = true;
                        txtCandidato.Visible = true;
                        lblMsgCandidato.Visible = false;
                        lblMensagem.Text = string.Empty;

                        btnFinalizar.Visible = false;

                        LimparTela();
                        LimparEndereco();
                        LimparEnderecoNaturalidade();

                        Ly_candidato_docente dtCandidatoDocente = new Ly_candidato_docente();

                        Ly_candidato_docente.Row dadosCandidatoDocente = dtCandidatoDocente.NewRow();
                        dadosCandidatoDocente.Concurso = tseConcurso.DBValue.ToString();
                        dadosCandidatoDocente = RN.CandidatoDocente.Consultar(dadosCandidatoDocente);

                        if (dadosCandidatoDocente == null)
                        {
                            lblMensagem.Text = "Candidato não encontrado";
                            pcCandidatoDocente.Visible = false;
                        }
                        else
                        {
                            ImageButton[] controles = new ImageButton[] { };
                            ControlarVisibilidadeControle(controles);
                            pcCandidatoDocente.Visible = true;

                            CarregaNecessidadeEspecial();
                            CarregarDadosDrop(ddlEst_Civil.ID);
                            CarregarDadosDrop(ddlPaisNasc.ID);
                            CarregarDadosDrop(ddlNacionalidade.ID);

                            CarregarDadosDrop(ddlRGTipoPessoa.ID);
                            CarregarDadosDrop(cmbRGUF.ID);
                            CarregarDadosDrop(cmbRGEmissor.ID);

                            CarregaDadosCandidatoDocente(dadosCandidatoDocente);
                            DesabilitaCampos();

                        }

                        pcCandidatoDocente.ActiveTabIndex = 0;

                        grdTitulacao.Enabled = false;
                        grdExperiencia.Enabled = false;

                        break;

                    }
                case TipoOperacao.Finalizado:
                    {
                        btnImprimir.Visible = true;

                        tseConcurso.Enabled = false;
                        tseMunicipio.Enabled = false;
                        cmbCargo.Enabled = false;
                        pnlContatos.Enabled = false;
                        PnlDadosPessoais.Enabled = false;
                        pnlDocumentos.Enabled = false;
                        pnlEndereco.Enabled = false;
                        btnFinalizar.Enabled = false;
                        break;
                    }
            }
        }

        private void CarregaDadosCandidatoDocente(Ly_candidato_docente.Row dadosCandidatoDocente)
        {
            //DataTable dtnucleo = new DataTable();

            txtNomeCompleto.Text = dadosCandidatoDocente.Nome;
            txtNomeMae.Text = dadosCandidatoDocente.Nome_mae;
            txtNomePai.Text = dadosCandidatoDocente.Nome_pai;
            txtCep.Text = dadosCandidatoDocente.Cep;
            txtMunicipio.Text = dadosCandidatoDocente.Municipio_nasc;
            txtEndereco.Text = dadosCandidatoDocente.Endereco;
            txtEndNum.Text = dadosCandidatoDocente.End_num;
            txtEndCompl.Text = dadosCandidatoDocente.End_compl;
            txtBairro.Text = dadosCandidatoDocente.Bairro;
            txtFone.Text = dadosCandidatoDocente.Fone.AplicarMascaraTelefoneComDDD();
            txtCelular.Text = dadosCandidatoDocente.Celular.AplicarMascaraTelefoneComDDD();
            txtEmail.Text = dadosCandidatoDocente.E_mail;
            txtRGNum.Text = dadosCandidatoDocente.Rg_num;
            txtCPF.Text = dadosCandidatoDocente.Cpf.AplicarMascaraCPF();
            txtPisPasep.Text = dadosCandidatoDocente.Pis_pasep;
            txtStatusCandidato.Text = Convert.ToString(dadosCandidatoDocente.Status);
            dtDataNasc.MaxDate = DateTime.Now.Date;
            dtDataExped.MaxDate = DateTime.Now.Date;
            rblSexo.Text = dadosCandidatoDocente.Sexo;

            if (dadosCandidatoDocente.Dt_nasc.HasValue)
                dtDataNasc.Date = dadosCandidatoDocente.Dt_nasc.Value;
            if (dadosCandidatoDocente.Rg_dtexp.HasValue)
                dtDataExped.Date = dadosCandidatoDocente.Rg_dtexp.Value;
            if (dadosCandidatoDocente.Cprof_dtexp.HasValue)

                PreencherDadoCombo(cmbNecessidadeEspecial, Convert.ToString(dadosCandidatoDocente.NecessidadeEspecialId));
            PreencherDadoCombo(ddlEst_Civil, Convert.ToString(dadosCandidatoDocente.Estado_civil));
            PreencherDadoCombo(ddlPaisNasc, Convert.ToString(dadosCandidatoDocente.Pais_nasc));
            PreencherDadoCombo(ddlNacionalidade, Convert.ToString(dadosCandidatoDocente.Nacionalidade));
            PreencherDadoCombo(ddlRGTipoPessoa, Convert.ToString(dadosCandidatoDocente.Rg_tipo));
            PreencherDadoCombo(cmbRGUF, Convert.ToString(dadosCandidatoDocente.Rg_uf));
            PreencherDadoCombo(cmbRGEmissor, Convert.ToString(dadosCandidatoDocente.Rg_emissor));

            if (!string.IsNullOrEmpty(dadosCandidatoDocente.Concurso))
                tseConcurso.DBValue = dadosCandidatoDocente.Concurso;
            txtCandidato.Text = dadosCandidatoDocente.Candidato;
            tseRegional.DBValue = dadosCandidatoDocente.RegionalId;
            cmbCargo.Items.Clear();

            if (!string.IsNullOrEmpty(dadosCandidatoDocente.End_municipio))
            {
                tseMunicipio.DBValue = dadosCandidatoDocente.End_municipio;
                if (tseMunicipio.IsValidDBValue & !tseMunicipio.DBValue.IsNull)
                    txtEstado.Value = tseMunicipio["uf_sigla"].ToString();
            }
            if (!string.IsNullOrEmpty(dadosCandidatoDocente.Municipio_nasc))
            {
                tseNaturalidade.DBValue = dadosCandidatoDocente.Municipio_nasc;
                if (tseNaturalidade.IsValidDBValue & !tseNaturalidade.DBValue.IsNull)
                    txtNaturalidadeUF.Text = tseMunicipio["uf_sigla"].ToString();
            }
            if (!string.IsNullOrEmpty(dadosCandidatoDocente.Municipio_proc))
            {
                tseMunicipioProc.DBValue = dadosCandidatoDocente.Municipio_proc;
                tseRegional.DBValue = dadosCandidatoDocente.RegionalId.ToString();
                tseRegional.Visible = true;
                lblCoordMunic.Visible = true;

                //dtnucleo = new RN.ProcessoSeletivo().ListaCoordenadoriaPor(Convert.ToString(tseMunicipioProc.DBValue), Convert.ToString(tseConcurso.DBValue));

                //if (dtnucleo.Rows.Count > 1)
                //{
                //    tseRegional.Visible = true;
                //    lblCoordMunic.Visible = true;

                //    tseRegional.DBValue = dadosCandidatoDocente.Nucleo == null ? "0" : dadosCandidatoDocente.Nucleo;
                //}
                //else
                //{
                //    hdnNucleo.Value = dtnucleo.Rows[0]["Nucleo"].ToString();
                //    tseRegional.Visible = false;
                //    lblCoordMunic.Visible = false;

                //}
            }
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

        private object CarregarDadosDrop(string idDrop)
        {
            QueryTable dadosDrop = null;

            try
            {
                switch (idDrop)
                {
                    case "ddlEst_Civil":
                        {
                            dadosDrop = RN.Basico.ConsultaItemTabelaValDescr("Estado civil");
                            CarregarDropDownList(ddlEst_Civil, dadosDrop, "");
                            break;
                        }

                    case "ddlPaisNasc":
                        {
                            dadosDrop = RN.Basico.ConsultarPais();
                            CarregarDropDownList(ddlPaisNasc, dadosDrop, "");
                            break;
                        }
                    case "ddlNacionalidade":
                        {
                            dadosDrop = RN.Basico.ConsultarNacionalidade();
                            CarregarDropDownList(ddlNacionalidade, dadosDrop, "");
                            break;
                        }
                    case "ddlRGTipoPessoa":
                        {
                            string param = "TIPO DOC";
                            dadosDrop = RN.Basico.ConsultaItemTabelaValDescr(param);
                            CarregarDropDownList(ddlRGTipoPessoa, dadosDrop, "");
                            break;
                        }
                    case "cmbRGUF":
                        {
                            dadosDrop = RN.Basico.ConsultarUF();
                            CarregarDropDownList(cmbRGUF, dadosDrop, "");
                            break;
                        }
                    case "cmbRGEmissor":
                        {
                            string param = "ORGAO RG_CT";
                            dadosDrop = RN.Basico.ConsultaItemTabelaValDescr(param);
                            CarregarDropDownList(cmbRGEmissor, dadosDrop, "");
                            break;
                        }
                    case "ddlCotas":
                        {
                            dadosDrop = RN.ContratoTemporario.Cota.ListarCotasPorNecessidadeEtnia();
                            CarregarDropDownList(ddlCotas, dadosDrop, "");
                            ddlCotas.SelectedIndex = 3;
                            break;
                        }
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

        private void CarregarDropDownList(DropDownList drop, object data, string defaultValue)
        {
            drop.SelectedIndex = -1;
            drop.Items.Clear();
            drop.SelectedValue = null;
            drop.DataSource = data;
            drop.DataBind();
            ListItem itemVazio = new ListItem("<Selecione>", "");
            drop.Items.Insert(0, itemVazio);

            if (_tipoOperacao.Equals(TipoOperacao.Novo))
            {
                drop.SelectedValue = "";
                if (drop == ddlPaisNasc)
                {
                    ListItem listItem = drop.Items.FindByText("BRASIL");
                    if (listItem != null)
                    {
                        drop.ClearSelection();
                        listItem.Selected = true;
                    }
                }

                if (drop == ddlNacionalidade)
                {
                    ListItem listItem = drop.Items.FindByText("BRASILEIRA");
                    if (listItem != null)
                    {
                        drop.ClearSelection();
                        listItem.Selected = true;
                    }
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
            txtEstado.Value = string.Empty;
            rblSexo.SelectedIndex = -1;
            dtDataNasc.Text = string.Empty;
            dtDataExped.Text = string.Empty;
            cmbNecessidadeEspecial.Items.Clear();
            ddlEst_Civil.Items.Clear();
            ddlPaisNasc.Items.Clear();
            ddlNacionalidade.Items.Clear();
            ddlRGTipoPessoa.Items.Clear();
            cmbRGUF.Items.Clear();
            cmbRGEmissor.Items.Clear();

            tseConcurso.ResetValue();
            tseRegional.ResetValue();
            cmbCargo.Items.Clear();
            cmbCargo.SelectedIndex = -1;
            tseMunicipio.ResetValue();
            tseNaturalidade.ResetValue();
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
            rblSexo.Enabled = false;
            dtDataNasc.ReadOnly = true;
            dtDataExped.ReadOnly = true;

            cmbNecessidadeEspecial.Enabled = false;
            ddlEst_Civil.Enabled = false;
            ddlPaisNasc.Enabled = false;
            ddlNacionalidade.Enabled = false;
            ddlRGTipoPessoa.Enabled = false;
            cmbRGUF.Enabled = false;
            cmbRGEmissor.Enabled = false;

            tseConcurso.Mode = ControlMode.View;
            cmbCargo.Enabled = false;
            tseRegional.Mode = ControlMode.View;
            tseMunicipio.Mode = ControlMode.View;
            tseNaturalidade.Mode = ControlMode.View;

            tsCEP.ShowButton = false;

            if (lblCoordMunic.Visible)
                tseRegional.Visible = true;
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
            rblSexo.Enabled = true;
            dtDataNasc.ReadOnly = false;
            dtDataExped.ReadOnly = false;
            cmbNecessidadeEspecial.Enabled = true;
            ddlEst_Civil.Enabled = true;
            ddlPaisNasc.Enabled = true;
            ddlNacionalidade.Enabled = true;
            ddlRGTipoPessoa.Enabled = true;
            cmbRGUF.Enabled = true;
            cmbRGEmissor.Enabled = true;
            tseConcurso.Mode = ControlMode.Edit;
            cmbCargo.Enabled = true;
            tseRegional.Mode = ControlMode.Edit;
            tseMunicipio.Mode = ControlMode.Edit;
            tseNaturalidade.Mode = ControlMode.Edit;

            tsCEP.ShowButton = true;

            if (lblCoordMunic.Visible)
                tseRegional.Visible = true;
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
        }
        /// <summary>
        /// Preenche o campo Função da ficha de inscrição usando a notação definida pela SEEDUC.
        /// DEFINIÇÂO: Nomenclatura utilizada pelo usuário do sistema para Função de docente
        /// DOC I = Professor para atuar nos anos finais do ensino fundamental e/ou ensino médio
        /// DOC II = Professor para atuar nos anos iniciais do ensino fundamental
        /// </summary>
        /// <param name="funcaoCandidato">Função</param>
        private void PreencherFuncao()
        {
            RN.Funcao rnFuncao = new RN.Funcao();

            cmbCargo.DataSource = rnFuncao.RetornaFuncao();
            cmbCargo.DataBind();
            cmbCargo.DataValueField = "CODIGO";
            cmbCargo.DataTextField = "DESCRICAO";
        }

        #endregion

        #region Métodos dos Botões

        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            foreach (ImageButton botao in botoes)
            {
                botao.Visible = true;
            }
        }

        #endregion

        #region Grid Tituações e Experiências

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
            EcandidatoDocente.Concurso = Session["SSconcurso"].ToString();
            EcandidatoDocente.Candidato = Session["SScandidato"].ToString();
            e.NewValues.Add("concurso", EcandidatoDocente.Concurso);
            e.NewValues.Add("candidato", EcandidatoDocente.Candidato);
        }

        protected void grdTitulacao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            EcandidatoDocente.Concurso = Session["SSconcurso"].ToString();
            EcandidatoDocente.Candidato = Session["SScandidato"].ToString();
            e.Keys.Add("concurso", EcandidatoDocente.Concurso);
            e.Keys.Add("candidato", EcandidatoDocente.Candidato);
        }

        protected void grdTitulacao_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            EcandidatoDocente.Concurso = Session["SSconcurso"].ToString();
            EcandidatoDocente.Candidato = Session["SScandidato"].ToString();
            e.Keys.Add("concurso", EcandidatoDocente.Concurso);
            e.Keys.Add("candidato", EcandidatoDocente.Candidato); ;
        }

        protected void grdExperiencia_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            EcandidatoDocente.Concurso = Session["SSconcurso"].ToString();
            EcandidatoDocente.Candidato = Session["SScandidato"].ToString();
            e.Keys.Add("concurso", EcandidatoDocente.Concurso);
            e.Keys.Add("candidato", EcandidatoDocente.Candidato); ;
        }

        protected void grdExperiencia_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            EcandidatoDocente.Concurso = Session["SSconcurso"].ToString();
            EcandidatoDocente.Candidato = Session["SScandidato"].ToString();
            e.NewValues.Add("concurso", EcandidatoDocente.Concurso);
            e.NewValues.Add("candidato", EcandidatoDocente.Candidato);
        }

        protected void grdExperiencia_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            EcandidatoDocente.Concurso = Session["SSconcurso"].ToString();
            EcandidatoDocente.Candidato = Session["SScandidato"].ToString();
            e.Keys.Add("concurso", EcandidatoDocente.Concurso);
            e.Keys.Add("candidato", EcandidatoDocente.Candidato);
        }

        #endregion

        public static bool ValidarEmail(string email)
        {
            bool validEmail = false;
            int indexArr = email.IndexOf('@');
            if (indexArr > -1)
            {
                int indexDot = email.IndexOf('.', indexArr);
                if (indexDot > -1 && email.Length - 1 > indexDot)
                {
                    validEmail = true;
                }
            }
            return validEmail;
        }

        protected void btnSalvarCandidato_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarTela() == true)
                {
                    pcCandidatoDocente.ActiveTabIndex = 1;
                }
                else
                {
                    pcCandidatoDocente.ActiveTabIndex = 0;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true; lblMensagem2.Visible = true;

            }
        }

        protected void btnAvancarDisciplina_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarAbaDisciplinas() == true)
                {
                    pcCandidatoDocente.ActiveTabIndex = 2;
                    ccTitulacaoExperiencia.Enabled = true;
                }
                else
                    pcCandidatoDocente.ActiveTabIndex = 1;

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true; lblMensagem2.Visible = true;

            }
        }

        /// <summary>
        /// Método que desabilita todos os campos do tipo textBox , RichTextBox , MaskedTextBox,
        /// DropDownList, CheckBox, RadioButton contidos dentro de um controle
        /// </summary>
        /// <param name="parent"></param>
        public static void ProtegeTodosCampos(Control parent)
        {
            foreach (Control ctrControl in parent.Controls)
            {
                if (object.ReferenceEquals(ctrControl.GetType(), typeof(TextBox)))
                {
                    ((TextBox)ctrControl).Enabled = false;
                }
                else if (object.ReferenceEquals(ctrControl.GetType(), typeof(DropDownList)))
                {
                    ((DropDownList)ctrControl).Enabled = false;
                }
                else if (object.ReferenceEquals(ctrControl.GetType(), typeof(CheckBox)))
                {
                    ((CheckBox)ctrControl).Enabled = false;
                }
                else if (object.ReferenceEquals(ctrControl.GetType(), typeof(RadioButton)))
                {
                    ((RadioButton)ctrControl).Enabled = false;
                }
                else if (object.ReferenceEquals(ctrControl.GetType(), typeof(ASPxComboBox)))
                {
                    ((ASPxComboBox)ctrControl).Enabled = false;
                }
                else if (object.ReferenceEquals(ctrControl.GetType(), typeof(ASPxDateEdit)))
                {
                    ((ASPxDateEdit)ctrControl).Enabled = false;
                }
                else if (object.ReferenceEquals(ctrControl.GetType(), typeof(ASPxGridView)))
                {
                    ((ASPxGridView)ctrControl).Enabled = false;
                }
                if (ctrControl.Controls.Count > 0)
                {
                    ProtegeTodosCampos(ctrControl);
                }
            }
        }

        private void CarregaDropEtnia()
        {
            ddlEtnia.DataSource = new RN.Etnia().ListaEtniaAtivaContratoTemporario();
            ddlEtnia.DataBind();
            ddlEtnia.DataTextField = "NOME";
            ddlEtnia.DataValueField = "ETNIAID";
        }

        private void PreencheCmbCargo(string concurso)
        {
            RN.Funcao rnFuncao = new RN.Funcao();

            cmbCargo.DataSource = rnFuncao.RetornaFuncao(concurso);
            cmbCargo.DataBind();
            cmbCargo.DataValueField = "CODIGO";
            cmbCargo.DataTextField = "DESCRICAO";
        }

        #region Métodos Privados

        private void DesabilitarCampos()
        {
            tseMunicipioProc.Enabled = false;
            //tseDiscIngresso.Enabled = false;
            PnlDadosPessoais.Enabled = false;
            pnlContatos.Enabled = false;
            pnlDocumentos.Enabled = false;
            pnlEndereco.Enabled = false;
        }

        private void ValidarCotasPorNecessidadeEspecialERaca()
        {
            QueryTable dadosDrop = null;

            if (cmbNecessidadeEspecial.SelectedItem.Text != "Não possui.")
            {
                if (ddlEtnia.SelectedIndex == 3 || ddlEtnia.SelectedIndex == 4 || ddlEtnia.SelectedIndex == 5)
                {
                    dadosDrop = RN.ContratoTemporario.Cota.ListarCotas();
                    CarregarDropDownList(ddlCotas, dadosDrop, "");
                }
                else
                {
                    dadosDrop = RN.ContratoTemporario.Cota.ListarCotasPorDeficienteFisico();
                    CarregarDropDownList(ddlCotas, dadosDrop, "");
                }
            }
            else
            {
                if (ddlEtnia.SelectedIndex == 3 || ddlEtnia.SelectedIndex == 4 || ddlEtnia.SelectedIndex == 5)
                {
                    dadosDrop = RN.ContratoTemporario.Cota.ListarCotasPorEtniaIndioOuNegra();
                    CarregarDropDownList(ddlCotas, dadosDrop, "");
                }
                else
                {
                    dadosDrop = RN.ContratoTemporario.Cota.ListarCotasPorNecessidadeEtnia();
                    CarregarDropDownList(ddlCotas, dadosDrop, "");
                }
            }
        }
        #endregion

        #region Eventos

        protected void Imprimir_Click(object sender, EventArgs e)
        {
            Imprimir();
        }

        private void Imprimir()
        {
            IDictionary<string, string> pares = new Dictionary<string, string>();
            string textoJavaScript = string.Empty;

            if (Session["SScandidato"] != null && Session["SSconcurso"] != null)
            {
                pares.Add("candidato", Session["SScandidato"].ToString());
                pares.Add("concurso", Session["SSconcurso"].ToString());

                textoJavaScript = string.Concat("<script>AbrePopupImpressao('",
                    string.Concat(TPage.CodificaQueryString(pares), "');</script>"));

                Page.ClientScript.RegisterStartupScript(this.GetType(), string.Empty, textoJavaScript);
            }
        }

        protected void cmbNecessidadeEspecial_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidarCotasPorNecessidadeEspecialERaca();
        }

        protected void ddlEtnia_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidarCotasPorNecessidadeEspecialERaca();
        }

        #endregion

    }
}
