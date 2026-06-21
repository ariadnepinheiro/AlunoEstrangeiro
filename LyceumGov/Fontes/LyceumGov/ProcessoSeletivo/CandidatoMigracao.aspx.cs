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
using System.Web.UI.WebControls;


namespace Techne.Lyceum.Net.ProcessoSeletivo
{
    [NavUrl("~/ProcessoSeletivo/CandidatoMigracao.aspx"),
    ControlText("CandidatoMigracao"),
    Title("Candidato Migraçao - Inscrição"),]
    public partial class CandidatoMigracao : TPage
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
            Sucesso,
            Novo,
            Excluir,
            Alterar
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
            ControlaAcesso(grdDocumento);
            ControlaAcesso(grdDocumento, AcaoControle.editar, "btnDetalhes");

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                tseNaturalidade.Mode = ControlMode.View;

                if (!IsPostBack)
                {
                    //para a primeira vez que a página é carregada o tipo de operação será inicial
                    Page.Header.Controls.AddAt(0, new HtmlMeta { HttpEquiv = "X-UA-Compatible", Content = "IE=8" });
                    _tipoOperacao = TipoOperacao.Inicial;
                    ControlarTipoOperacao();
                    pcCandidatoDocente.ActiveTabIndex = 0;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdDocumento, "Documentos");
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }
        #endregion

        #region Eventos

        protected void tseConcursoBusca_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                tseCandidatoBusca.Mode = ControlMode.View;

                if (!tseConcursoBusca.DBValue.IsNull && tseConcursoBusca.IsValidDBValue)
                {
                    LimparTela();
                    LimparEndereco();
                    LimparEnderecoNaturalidade();
                    tseCandidatoBusca.ResetValue();
                    tseCandidatoBusca.Mode = ControlMode.Edit;

                    pcCandidatoDocente.TabPages[1].Enabled = false;
                    pcCandidatoDocente.TabPages[2].Enabled = false;
                    pcCandidatoDocente.TabPages[3].Enabled = false;
                    pcCandidatoDocente.ActiveTabIndex = 0;
                    pcCandidatoDocente.Visible = false;
                    txtStatusCandidato.Text = string.Empty;
                    pnlNumeroInscricao.Visible = false;
                    pnlDocenteTSearch.Visible = false;
                    grdDocumento.Columns[3].Visible = false;
                    pnlFuncaoDiretor.Visible = false;
                    pnlTempoGLP.Visible = false;
                    pnlAnosGLP.Visible = false;
                    

                    CarregaExperienciaTitulacao();

                    if (tseConcursoBusca["ano"].ToString() == "2024")
                    {
                        pnlFuncaoDiretor.Visible = true;
                    }

                    if (Convert.ToInt32(tseConcursoBusca["ano"]) < 2026)
                    {
                        pnlTempoGLP.Visible = true;
                    }
                    else
                    {
                        pnlAnosGLP.Visible = true;
                        CarregaAnosGLP();
                    }


                    pcCandidatoDocente.DataBind();
                }
                else
                {
                    lblMensagem.Text = "Favor informar um processo seletivo.";
                    pcCandidatoDocente.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaRegional()
        {
            ddlRegionaldesejada.Items.Clear();
            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem("Selecione", string.Empty);
            ddlRegionaldesejada.DataSource = RN.Regional.Listar();
            ddlRegionaldesejada.DataBind();
            ddlRegionaldesejada.Items.Insert(0, item);
        }


        protected void tseCandidatoBusca_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                txtCandidato.Text = string.Empty;
                hdnDocenteCandidatoId.Value = string.Empty;

                if (!tseCandidatoBusca.DBValue.IsNull && tseCandidatoBusca.IsValidDBValue)
                {
                    _tipoOperacao = TipoOperacao.Consultar;
                }
                else
                {
                    lblMensagem.Text = "Favor informar um processo seletivo e um candidato.";
                    _tipoOperacao = TipoOperacao.Inicial;
                }

                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseDocente_Changed(object sender, EventArgs args)
        {
            try
            {
                LimparTela();
                LimparEndereco();
                LimparEnderecoNaturalidade();

                if (tseDocente.IsValidDBValue && !tseDocente.DBValue.IsNull)
                {
                    var dataNascimento = Convert.ToDateTime(tseDocente["dt_nasc"]);
                    var idade = Utils.CalcularIdade(dataNascimento);

                    if (idade < 18 || idade > 70)
                    {
                        lblMensagem.Text = "ID Vínculo informado não está contemplado na MIGRAÇÃO CARGA HORÁRIA DOCENTE I";
                    }
                    else
                    {
                        this.CarregaDados(tseDocente.DBValue.ToString());

                        if (!txtNomeCompleto.Text.IsNullOrEmptyOrWhiteSpace())
                            HabilitaCampos();

                        if (!hdnDocenteCandidatoId.Value.IsNullOrEmptyOrWhiteSpace())
                        {
                            string concurso = tseConcursoBusca.DBValue.ToString();
                            lblMensagem.Text = string.Format("ID Vínculo informado já possui a inscrição {0}, favor consultar.", hdnDocenteCandidatoId.Value);

                            tseDocente.ResetValue();
                            LimparTela();
                            LimparEndereco();
                            LimparEnderecoNaturalidade();

                            _tipoOperacao = TipoOperacao.Inicial;
                            ControlarTipoOperacao();

                            tseConcursoBusca.DBValue = concurso;
                        }
                    }
                }
                else
                {
                    lblMensagem.Text = "Favor consultar um docente.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Inicial;
            ControlarTipoOperacao();
        }

        protected void btnProximoClick(object sender, EventArgs e)
        {
            pcCandidatoDocente.ActiveTabIndex = 1;

        }

        protected void CarregaDados(string idVinculo)
        {
            RN.RecursosHumanos.DocenteCandidato rnDocenteCandidato = new Techne.Lyceum.RN.RecursosHumanos.DocenteCandidato();
            RN.RecursosHumanos.DTO.DadosInscricaoMigracao dados = new DadosInscricaoMigracao();
            pcCandidatoDocente.Visible = true;
            pcCandidatoDocente.ActiveTabIndex = 0;
            pnlRegionalDesejadaSede.Visible = false;

            try
            {
                if (tseConcursoBusca.DBValue.IsNull || !tseConcursoBusca.IsValidDBValue || idVinculo.IsNullOrEmptyOrWhiteSpace())
                {
                    lblMensagem.Text = "Favor informar um processo seletivo e um candidato.";
                    pcCandidatoDocente.Visible = false;
                    return;
                }

                dados = rnDocenteCandidato.ObtemDadosInscricaoMigracaoPor(idVinculo, tseConcursoBusca.DBValue.ToString(), Convert.ToInt32(tseConcursoBusca["ANO"]));

                if (dados.NumFunc > 0)
                {
                    pcCandidatoDocente.Visible = true;
                    hdnNumFunc.Value = dados.NumFunc.ToString();

                    CarregarDadosDrop(ddlEst_Civil.ID);
                    CarregarDadosDrop(cmbRGUF.ID);
                    CarregarDadosDrop(cmbRGEmissor.ID);
                    CarregarDadosDrop(ddDlCprof_Uf.ID);

                    CarregarDadosDrop(ddlUFCNH.ID);

                    CarregarDadosDrop(ddlEleitor_Uf.ID);
                    CarregarDadosDrop(ddlCrUF.ID);
                    CarregaRegional();

                    dtDataNasc.MaxDate = DateTime.Now.Date;
                    dtDataExped.MaxDate = DateTime.Now.Date;

                    //dados de identificação
                    txtCandidato.Text = dados.NumeroInscricao;
                    hdnDocenteCandidatoId.Value = dados.DocenteCandidatoId == null || dados.DocenteCandidatoId == 0 ? string.Empty : Convert.ToString(dados.DocenteCandidatoId);

                    hdnDataConvocacao.Value = dados.DataConvocacao.ToShortDateString();
                    hdnSituacao.Value = dados.Situacao;
                    txtStatusCandidato.Text = dados.DescricaoSituacao;
                    txtPessoaHidden.Text = Convert.ToString(dados.Pessoa);

                    //dados Lotacao
                    hdnRegionalLotacao.Value = Convert.ToString(dados.RegionalId);
                    hdnSedeLotacao.Value = dados.Sede.ToUpper();

                    if (!hdnSedeLotacao.Value.IsNullOrEmptyOrWhiteSpace())
                    {
                        pnlRegionalDesejadaSede.Visible = true;
                        ddlRegionaldesejada.SelectedValue = Convert.ToString(dados.RegionalId);
                    }

                    txtLotacaoRegionalSede.Text = dados.RegionalDescricao.ToUpper();
                    hdnMunicipioLotacao.Value = dados.MunicipioCodigo;
                    txtLotacaoMunicipio.Text = dados.MunicipioDescricao.ToUpper();
                    hdnDiscIngresso.Value = dados.DisciplinaIngresso;
                    txtLotacaoDisciplinaIngresso.Text = dados.DisciplinaIngressoDescricao.ToUpper();

                    //dados do professor
                    txtNomeCompleto.Text = dados.Nome.ToUpper();
                    dtDataNasc.Value = dados.DataNascimento;
                    txtNomeMae.Text = dados.NomeMae.ToUpper();
                    txtNomePai.Text = dados.NomePai.ToUpper();

                    PreencherDadoCombo(ddlEst_Civil, Convert.ToString(dados.EstadoCivil));

                    txtDependentes.Text = Convert.ToString(dados.NumeroDependentes);

                    if (!dados.NaturalidadeId.IsNullOrEmptyOrWhiteSpace())
                    {
                        tseNaturalidade.DBValue = dados.NaturalidadeId;
                        if (tseNaturalidade.IsValidDBValue & !tseNaturalidade.DBValue.IsNull)
                            txtNaturalidadeUF.Text = dados.NaturalidadeUf;
                    }

                    //endereço
                    txtCep.Text = dados.Cep;
                    txtEndereco.Text = dados.Endereco;
                    txtEndNum.Text = dados.EnderecoNumero;
                    txtEndCompl.Text = dados.EnderecoComplemento;
                    txtBairro.Text = dados.EnderecoBairro;
                    txtMunicipio.Text = dados.EndMunicipioDescricao;
                    txtEstado.Value = dados.EnderecoUf;

                    //contato 
                    txtEmail.Text = dados.EmailPessoal;
                    txtEmailInstitucional.Text = dados.EmailInstitucional;
                    int resul;
                    if (int.TryParse(dados.Telefone, out resul))
                        txtFone.Text = string.Format("{0:(00)0000-0000}", resul);
                    else
                        txtFone.Text = dados.Telefone.ToString().AplicarMascaraTelefoneComDDD();
                    long resultado;
                    if (long.TryParse(dados.Celular.RetirarMascaraTelefone(), out resultado))
                    {
                        if (dados.Celular.Length == 10)
                        {
                            txtCelular.Text = string.Format("{0:(00)0000-0000}", resultado);
                        }
                        else
                        {
                            txtCelular.Text = string.Format("{0:(00)00000-0000}", resultado);
                        }
                    }

                    //documentos pessoais
                    txtRGNum.Text = dados.Rg;
                    PreencherDadoCombo(cmbRGEmissor, Convert.ToString(dados.RgOrgao));
                    dtDataExped.Value = Convert.ToDateTime(dados.RgDataExpedicao == null ? DateTime.MinValue : Convert.ToDateTime(dados.RgDataExpedicao));
                    PreencherDadoCombo(cmbRGUF, dados.RgUf);
                    txtCPF.Text = dados.Cpf;
                    txtPisPasep.Text = dados.PisPasep;
                    txtCrpof_Num.Text = dados.CarteiraTrabalho;
                    txtCprof_Serie.Text = dados.CarteiraTrabalhoSerie;
                    PreencherDadoCombo(ddDlCprof_Uf, Convert.ToString(dados.CarteiraTrabalhoUf));
                    txtDOC_Teleitor_Num.Text = dados.TituloEleitor;
                    txtDOC_Teleitor_Zona.Text = dados.TituloEleitorZona;
                    txtDOC_Teleitor_Secao.Text = dados.TituloEleitorSecao;
                    PreencherDadoCombo(ddlEleitor_Uf, dados.TituloEleitorUf);
                    txtNumCNH.Text = dados.Cnh;
                    ddlCategoriaCNH.SelectedValue = dados.CnhCategoria;
                    dtValidadeCNH.Value = dados.CnhValidade;

                    PreencherDadoCombo(ddlUFCNH, Convert.ToString(dados.CnhUf));
                    txtDMIL_Cr_Num.Text = dados.CertificadoReservista;
                    txtDMIL_Cr_Serie.Text = dados.CertificadoReservistaSerie;
                    PreencherDadoCombo(ddlCrUF, dados.CertificadoReservistaUf);

                    //outros
                    if (dados.Acumulacao != null)
                    {
                        rblAcumulacao.SelectedValue = Convert.ToBoolean(dados.Acumulacao) ? "Sim" : "Nao";
                    }

                    if (dados.FuncaoDiretor != null)
                    {
                        rblFuncaoDiretor.SelectedValue = Convert.ToBoolean(dados.FuncaoDiretor) ? "Sim" : "Nao";
                    }

                    if (dados.UtilizaRubrica != null)
                    {
                        rblRubrica.SelectedValue = Convert.ToBoolean(dados.UtilizaRubrica) ? "Sim" : "Nao";
                    }

                    if (dados.Situacao == "8")
                    {
                        pnlDataConvocacao.Visible = true;
                        dtConvocacao.Date = dados.DataMigracao;
                    }

                    txtGLP.Text = Convert.ToString(dados.QuantidadeAnosGlp);


                    foreach (var item in dados.AnosGLP)
                    {
                          chlAnosGLP.Items.FindByValue(item.ToString()).Selected = true;
                    }

                    if (!dados.Experiencia.IsNullOrEmptyOrWhiteSpace())
                    {
                        rblExperiencia.SelectedValue = dados.Experiencia;
                    }

                    if (!dados.Titulacao.IsNullOrEmptyOrWhiteSpace())
                    {
                        rblTitulacao.SelectedValue = dados.Titulacao;
                    }

                    chkMigracaoAnterior.Checked = !dados.ParticipouMigracaoAnterior;


                }
                else
                {
                    lblMensagem.Text = "Docente/Candidato não encontrado";
                    pcCandidatoDocente.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                RN.RecursosHumanos.DocenteCandidato rnDocenteCandidato = new Techne.Lyceum.RN.RecursosHumanos.DocenteCandidato();
                ValidacaoDados validacao = new ValidacaoDados();
                RN.RecursosHumanos.DTO.DadosInscricaoMigracao dados = new DadosInscricaoMigracao();
                List<int> anosGLP = new List<int>();

                dados.DocenteCandidatoId = !hdnDocenteCandidatoId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnDocenteCandidatoId.Value) : -1;
                dados.RegionalId = !hdnRegionalLotacao.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnRegionalLotacao.Value) : -1;
                dados.Sede = hdnSedeLotacao.Value;
                dados.MunicipioCodigo = !hdnMunicipioLotacao.Value.IsNullOrEmptyOrWhiteSpace() ? hdnMunicipioLotacao.Value : null;
                dados.DisciplinaIngresso = !hdnDiscIngresso.Value.IsNullOrEmptyOrWhiteSpace() ? hdnDiscIngresso.Value : null;
                dados.EmailPessoal = !txtEmail.Text.IsNullOrEmptyOrWhiteSpace() ? txtEmail.Text : null;
                dados.Experiencia = !rblExperiencia.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblExperiencia.SelectedValue : null;
                dados.Titulacao = !rblTitulacao.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblTitulacao.SelectedValue : null;
                dados.Acumulacao = !rblAcumulacao.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblAcumulacao.SelectedValue == "Nao" ? false : true) : (bool?)null;

                foreach (System.Web.UI.WebControls.ListItem item in chlAnosGLP.Items)
                {
                    if (item.Selected)
                    {
                        anosGLP.Add(Convert.ToInt32(item.Value));
                    }
                }

                decimal resultado;
                if (!txtGLP.Text.IsNullOrEmptyOrWhiteSpace() && decimal.TryParse(this.txtGLP.Text, out resultado))
                {
                    dados.QuantidadeAnosGlp = Convert.ToInt32(txtGLP.Text);
                }
                else
                {
                    if (chlAnosGLP.Items.Count > 0)
                    {
                        dados.QuantidadeAnosGlp = anosGLP.Count();
                    }
                    else
                    {
                        dados.QuantidadeAnosGlp = null;
                    }
                }

                if (!txtDependentes.Text.IsNullOrEmptyOrWhiteSpace() && decimal.TryParse(this.txtDependentes.Text, out resultado))
                {
                    dados.NumeroDependentes = Convert.ToInt32(txtDependentes.Text);
                }
                else
                {
                    dados.QuantidadeAnosGlp = null;
                }

                if (hdnSituacao.Value == "8")
                {
                    dados.DataMigracao = dtConvocacao.Date;
                }

                dados.FuncaoDiretor = !rblFuncaoDiretor.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblFuncaoDiretor.SelectedValue == "Nao" ? false : true) : (bool?)null;
                dados.UtilizaRubrica = !rblRubrica.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblRubrica.SelectedValue == "Nao" ? false : true) : (bool?)null;
                dados.UsuarioId = User.Identity.Name;
                dados.Pessoa = (tseDocente.IsValidDBValue && !tseDocente.DBValue.IsNull) ? Convert.ToInt32(tseDocente["pessoa"]) : (!txtPessoaHidden.Text.IsNullOrEmptyOrWhiteSpace()) ? Convert.ToInt32(txtPessoaHidden.Text) : -1;
                dados.Concurso = (tseConcursoBusca.IsValidDBValue && !tseConcursoBusca.DBValue.IsNull) ? tseConcursoBusca.DBValue.ToString() : null;
                dados.IdVinculo = (tseDocente.IsValidDBValue && !tseDocente.DBValue.IsNull) ? tseDocente.DBValue.ToString() : (tseCandidatoBusca.IsValidDBValue && !tseCandidatoBusca.DBValue.IsNull) ? tseCandidatoBusca.DBValue.ToString() : null;
                dados.NumFunc = (tseDocente.IsValidDBValue && !tseDocente.DBValue.IsNull) ? Convert.ToInt32(tseDocente["num_func"]) : (!hdnNumFunc.Value.IsNullOrEmptyOrWhiteSpace()) ? Convert.ToInt32(hdnNumFunc.Value) : -1;


                if (!hdnSedeLotacao.Value.IsNullOrEmptyOrWhiteSpace())
                {
                    dados.RegionalId = !ddlRegionaldesejada.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlRegionaldesejada.SelectedValue) : -1;
                }

                validacao = rnDocenteCandidato.Valida(dados, (hdnDocenteCandidatoId.Value.IsNullOrEmptyOrWhiteSpace() ? true : false));

                if (validacao.Valido)
                {
                    if (hdnDocenteCandidatoId.Value.IsNullOrEmptyOrWhiteSpace())
                    {
                        rnDocenteCandidato.Insere(dados);

                        tseCandidatoBusca.DBValue = tseDocente.DBValue;
                    }
                    else
                    {
                        rnDocenteCandidato.Atualiza(dados);
                    }

                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();

                    pcCandidatoDocente.ActiveTabIndex = 0;
                    pcCandidatoDocente.TabPages[3].Enabled = true;

                    lblMensagem.Text = "Ficha de Inscrição " + (hdnDocenteCandidatoId.Value.IsNullOrEmptyOrWhiteSpace() ? "cadastrada" : "atualizada") + " com sucesso." + (hdnDocenteCandidatoId.Value.IsNullOrEmptyOrWhiteSpace() ? "Favor verificar os documentos." : "");

                    txtCandidato.Text = dados.DocenteCandidatoId.Value.ToString();
                    hdnDocenteCandidatoId.Value = dados.DocenteCandidatoId.Value.ToString();

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

        ///// <summary>
        ///// Obter dados do Candidato Docente com a inclusão do campo etnia
        ///// </summary>
        ///// <param name="dadosCandidatoDocente"></param>
        //private void ObterDadosCandidatoDocente(RN.Entidades.LyCandidatoDocente dadosCandidatoDocente)
        //{
        //    string nome = txtNomeCompleto.Text.TrimEnd().EndsWith(".")
        //        ? txtNomeCompleto.Text.TrimEnd().Substring(0, txtNomeCompleto.Text.TrimEnd().Length - 1)
        //        : txtNomeCompleto.Text.TrimEnd();

        //    dadosCandidatoDocente.Nome = nome.Trim().ToUpper();
        //    txtNomeCompleto.Text = nome.Trim().ToUpper();

        //    dadosCandidatoDocente.Nome_mae = txtNomeMae.Text;
        //    dadosCandidatoDocente.Nome_pai = txtNomePai.Text;
        //    dadosCandidatoDocente.Cep = txtCep.Text;
        //    dadosCandidatoDocente.Endereco = txtEndereco.Text;
        //    dadosCandidatoDocente.End_num = txtEndNum.Text;
        //    dadosCandidatoDocente.End_compl = txtEndCompl.Text;
        //    dadosCandidatoDocente.Bairro = txtBairro.Text;
        //    dadosCandidatoDocente.Fone = txtFone.Text.RetirarMascaraTelefone();
        //    dadosCandidatoDocente.Celular = txtCelular.Text.RetirarMascaraTelefone();
        //    dadosCandidatoDocente.E_mail = txtEmail.Text;
        //    dadosCandidatoDocente.Pis_pasep = txtPisPasep.Text;
        //    dadosCandidatoDocente.Cprof_num = txtCrpof_Num.Text;
        //    dadosCandidatoDocente.Cprof_serie = txtCprof_Serie.Text;
        //    dadosCandidatoDocente.Rg_num = txtRGNum.Text;
        //    dadosCandidatoDocente.Cpf = txtCPF.Text.RetirarMascaraCPF();
        //    dadosCandidatoDocente.End_pais = "1";//--BRASIL 


        //    //if (dboCprof_DtExp.Value != null)
        //    //{
        //    //    dadosCandidatoDocente.Cprof_dtexp = (DateTime?)dboCprof_DtExp.Value;
        //    //}
        //    //else
        //    //{
        //    //    dadosCandidatoDocente.Cprof_dtexp = null;
        //    //}

        //    if (dtDataExped.Value != null)
        //    {
        //        dadosCandidatoDocente.Rg_dtexp = (DateTime)dtDataExped.Value;
        //    }
        //    else
        //    {
        //        dadosCandidatoDocente.Rg_dtexp = null;
        //    }

        //    if (dtDataNasc.Value != null)
        //    {
        //        dadosCandidatoDocente.Dt_nasc = (DateTime)dtDataNasc.Value;
        //    }
        //    else
        //    {
        //        dadosCandidatoDocente.Dt_nasc = null;
        //    }

        //    //if (dboCprof_DtExp.Value != null)
        //    //{
        //    //    dadosCandidatoDocente.Cprof_dtexp = (DateTime)dboCprof_DtExp.Value;
        //    //}
        //    //else
        //    //{
        //    //    dadosCandidatoDocente.Cprof_dtexp = null;
        //    //}

        //    if (ddDlCprof_Uf.SelectedValue != "")
        //    {
        //        dadosCandidatoDocente.Cprof_uf = ddDlCprof_Uf.SelectedValue.ToString();
        //    }


        //    //dadosCandidatoDocente.End_pais = ddlPaisNasc.SelectedValue;

        //    if (!tseNaturalidade.DBValue.IsNull) //if inserido por Rafaela Alves - 12/07/2013
        //        dadosCandidatoDocente.Municipio_nasc = tseNaturalidade.DBValue.ToString();

        //    if (ddlEst_Civil.SelectedValue != "")
        //    {
        //        dadosCandidatoDocente.Estado_civil = ddlEst_Civil.SelectedValue;
        //    }
        //    if (cmbRGUF.SelectedValue != "")
        //    {
        //        dadosCandidatoDocente.Rg_uf = cmbRGUF.SelectedValue.ToString();
        //    }

        //    //if (ddlRGTipoPessoa.SelectedValue != "")
        //    //{
        //    //    dadosCandidatoDocente.Rg_tipo = ddlRGTipoPessoa.SelectedValue;
        //    //}
        //    if (cmbRGEmissor.SelectedValue != "")
        //    {
        //        dadosCandidatoDocente.Rg_emissor = cmbRGEmissor.SelectedValue;
        //    }

        //    //if (ddlPaisNasc.SelectedValue != "")
        //    //{
        //    //    dadosCandidatoDocente.Pais_nasc = ddlPaisNasc.SelectedValue;
        //    //}

        //    //if (ddlNacionalidade.SelectedValue != "")
        //    //{
        //    //    dadosCandidatoDocente.Nacionalidade = ddlNacionalidade.SelectedValue;
        //    //}

        //    else
        //    {
        //        dadosCandidatoDocente.Candidato = txtCandidato.Text;
        //        dadosCandidatoDocente.Status = Convert.ToDecimal(txtStatusCandidato.Text);
        //    }
        //}

        private void LimparEnderecoNaturalidade()
        {
            txtNaturalidadeNasc.Text = string.Empty;
            txtNaturalidadeUF.Text = string.Empty;
            tseNaturalidade.ResetValue();
        }

        private void CarregaExperienciaTitulacao()
        {
            rblTitulacao.ClearSelection();
            rblTitulacao.Items.Clear();
            rblTitulacao.DataSource = RN.CandidatoDocente.ConsultarTitulacao(tseConcursoBusca.DBValue.ToString());
            rblTitulacao.DataTextField = "NOME";
            rblTitulacao.DataValueField = "TITULACAO";
            rblTitulacao.DataBind();

            rblExperiencia.ClearSelection();
            rblExperiencia.Items.Clear();
            rblExperiencia.DataSource = RN.CandidatoDocente.ConsultarExperiencia(tseConcursoBusca.DBValue.ToString());
            rblExperiencia.DataTextField = "NOME";
            rblExperiencia.DataValueField = "experiencia";
            rblExperiencia.DataBind();
        }

        private void CarregaAnosGLP()
        {
            chlAnosGLP.ClearSelection();

            for (int ano = DateTime.Now.Year; ano >= 2012; ano--)
            {
                chlAnosGLP.Items.Add(ano.ToString());
            }         
        }

        private void ControlarTipoOperacao()
        {
            DataTable dtDadosCandidatoDocente = new DataTable();
            RN.RecursosHumanos.DocenteCandidato rnDocenteCandidato = new Techne.Lyceum.RN.RecursosHumanos.DocenteCandidato();
            ValidacaoDados validacao = new ValidacaoDados();

            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);
                        pcCandidatoDocente.TabPages[1].Enabled = false;
                        pcCandidatoDocente.TabPages[2].Enabled = false;
                        pcCandidatoDocente.TabPages[3].Enabled = false;

                        pcCandidatoDocente.ActiveTabIndex = 0;
                        pcCandidatoDocente.Visible = false;
                        tseConcursoBusca.ResetValue();
                        tseCandidatoBusca.ResetValue();
                        tseConcursoBusca.Mode = ControlMode.Edit;
                        tseCandidatoBusca.Mode = ControlMode.View;

                        txtStatusCandidato.Text = string.Empty;
                        pnlNumeroInscricao.Visible = false;
                        pnlDocenteTSearch.Visible = false;
                        pnlDataConvocacao.Visible = false;
                        grdDocumento.Columns[3].Visible = false;
                        btnAlterarData.Visible = false;
                        CarregaRegional();
                        break;
                    }
                case TipoOperacao.Novo:
                    {
                        if (!tseConcursoBusca.DBValue.IsNull && tseConcursoBusca.IsValidDBValue)
                        {
                            var controles = new[] { btnCancel, btnSalvar };
                            ControlarVisibilidadeControle(controles);

                            pcCandidatoDocente.Visible = true;
                            pcCandidatoDocente.TabPages[1].Enabled = true;
                            pcCandidatoDocente.TabPages[2].Enabled = true;
                            pcCandidatoDocente.TabPages[3].Enabled = false;
                            pnlNumeroInscricao.Visible = false;
                            pnlDocenteTSearch.Visible = true;
                            btnAlterarData.Visible = false;

                            tseDocente.Enabled = true;
                            LimparTela();
                            LimparEndereco();
                            LimparEnderecoNaturalidade();


                            CarregarDadosDrop(ddlEst_Civil.ID);
                            CarregarDadosDrop(cmbRGUF.ID);
                            CarregarDadosDrop(cmbRGEmissor.ID);
                            CarregarDadosDrop(ddDlCprof_Uf.ID);
                            CarregarDadosDrop(ddlUFCNH.ID);

                            CarregarDadosDrop(ddlEleitor_Uf.ID);
                            CarregarDadosDrop(ddlCrUF.ID);
                            CarregaRegional();

                            CarregaExperienciaTitulacao();
                            pcCandidatoDocente.ActiveTabIndex = 0;

                            tseConcursoBusca.Mode = ControlMode.View;
                            tseCandidatoBusca.Mode = ControlMode.View;
                        }
                        else
                        {
                            lblMensagem.Text = "Para fazer uma inscrição é necessário selecionar um processo seletivo.";
                        }
                        break;
                    }
                case TipoOperacao.Excluir:
                    {
                        validacao = rnDocenteCandidato.ValidaRemocao(Convert.ToInt32(hdnDocenteCandidatoId.Value), User.Identity.Name);

                        if (validacao.Valido)
                        {
                            rnDocenteCandidato.Remove(Convert.ToInt32(hdnDocenteCandidatoId.Value), tseConcursoBusca.DBValue.ToString(), User.Identity.Name);

                            _tipoOperacao = TipoOperacao.Inicial;
                            ControlarTipoOperacao();
                            lblMensagem.Text = "Candidato removido com sucesso.";
                        }
                        else
                        {
                            lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        }

                        break;
                    }
                case TipoOperacao.Alterar:
                    {
                        if ((!tseConcursoBusca.DBValue.IsNull && tseConcursoBusca.IsValidDBValue) && (!tseCandidatoBusca.DBValue.IsNull && tseCandidatoBusca.IsValidDBValue))
                        {

                            var controles = new[] { btnCancel, btnSalvar };
                            ControlarVisibilidadeControle(controles);

                            HabilitaCampos();
                            pnlNumeroInscricao.Visible = true;
                            pnlDocenteTSearch.Visible = false;
                            tseConcursoBusca.Mode = ControlMode.View;
                            tseCandidatoBusca.Mode = ControlMode.View;
                            grdDocumento.Columns[3].Visible = true;
                            btnAlterarData.Visible = false;

                        }
                        else
                        {
                            lblMensagem.Text = "Para alterar uma inscrição é necessário selecionar um processo seletivo e um candidato.";
                        }

                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        pcCandidatoDocente.TabPages[1].Enabled = true;
                        pcCandidatoDocente.TabPages[2].Enabled = true;
                        pcCandidatoDocente.TabPages[3].Enabled = true;
                        pcCandidatoDocente.Visible = true;
                        pnlDocenteTSearch.Visible = false;

                        ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar };
                        ControlarVisibilidadeControle(controles);

                        DesabilitaCampos();
                        txtCandidato.Visible = true;
                        dtDataNasc.MaxDate = DateTime.Now.Date;
                        dtDataExped.MaxDate = DateTime.Now.Date;

                        tseConcursoBusca.Mode = ControlMode.Edit;
                        tseCandidatoBusca.Mode = ControlMode.Edit;

                        grdDocumento.Columns[3].Visible = false;
                        break;
                    }


                case TipoOperacao.Consultar:
                    {
                        pcCandidatoDocente.TabPages[1].Enabled = true;
                        pcCandidatoDocente.TabPages[2].Enabled = true;
                        pcCandidatoDocente.TabPages[3].Enabled = true;
                        pnlDataConvocacao.Visible = false;
                        txtCandidato.Visible = true;
                        btnAlterarData.Visible = false;

                        LimparTela();
                        LimparEndereco();
                        LimparEnderecoNaturalidade();

                        if (string.IsNullOrEmpty(tseConcursoBusca.DBValue.ToString()) || string.IsNullOrEmpty(tseCandidatoBusca.DBValue.ToString()))
                        {
                            lblMensagem.Text = "Favor informar um processo seletivo e um candidato.";
                            pcCandidatoDocente.Visible = false;
                            tseConcursoBusca.Mode = ControlMode.Edit;
                            tseCandidatoBusca.Mode = ControlMode.Edit;
                            pcCandidatoDocente.ActiveTabIndex = 0;

                            break;
                        }

                        string concurso = tseConcursoBusca.DBValue.ToString();
                        string candidatoIdVinculo = tseCandidatoBusca.DBValue.ToString();

                        CarregaExperienciaTitulacao();
                        this.CarregaDados(candidatoIdVinculo);
                        tseDocente.ResetValue();

                        DesabilitaCampos();

                        tseConcursoBusca.Mode = ControlMode.Edit;
                        tseCandidatoBusca.Mode = ControlMode.Edit;
                        pcCandidatoDocente.ActiveTabIndex = 0;
                        pnlNumeroInscricao.Visible = true;
                        pnlDocenteTSearch.Visible = false;
                        grdDocumento.Columns[3].Visible = false;


                        var controles = new[] { btnCancel, btnEditar, btnExcluir };

                        if (hdnSituacao.Value == "8")
                        {
                            controles = new[] { btnCancel, btnEditar, btnExcluir, btnEditarData };
                        }

                        ControlarVisibilidadeControle(controles);


                        break;
                    }
            }
        }

        protected void btnDetalhes_Command(object sender, CommandEventArgs e)
        {
            try
            {
                hdnDocenteCandidatoTipoArquivoId.Value = string.Empty;
                hdnDocenteCandidatoArquivoId.Value = string.Empty;

                string[] chave = e.CommandArgument.ToString().Split(new char[] { ',' });

                hdnDocenteCandidatoTipoArquivoId.Value = Convert.ToString(chave[1]);
                hdnDocenteCandidatoArquivoId.Value = Convert.ToString(chave[0]);

                pucConfirmarArquivo.ShowOnPageLoad = true;

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnImportar_Click(object sender, EventArgs e)
        {
            try
            {
                RN.RecursosHumanos.DocenteCandidatoArquivo rnDocenteCandidatoArquivo = new Techne.Lyceum.RN.RecursosHumanos.DocenteCandidatoArquivo();
                RN.RecursosHumanos.Entidades.DocenteCandidatoArquivo documento = new Techne.Lyceum.RN.RecursosHumanos.Entidades.DocenteCandidatoArquivo();

                ValidacaoDados validacao = new ValidacaoDados();
                string mensagem = string.Empty;
                Statuslbl.Text = string.Empty;

                byte[] imageBytes = new byte[FileUpload1.PostedFile.InputStream.Length + 1];
                FileUpload1.PostedFile.InputStream.Read(imageBytes, 0, imageBytes.Length);

                documento.DocenteCandidatoId = !hdnDocenteCandidatoId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnDocenteCandidatoId.Value) : -1;
                documento.TipoDocumentoId = !hdnDocenteCandidatoTipoArquivoId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnDocenteCandidatoTipoArquivoId.Value) : -1;
                documento.DocenteCandidatoArquivoId = !hdnDocenteCandidatoArquivoId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnDocenteCandidatoArquivoId.Value) : -1;
                documento.ChaveArquivo = Guid.NewGuid().ToString();
                documento.Arquivo = imageBytes;
                documento.NomeArquivo = FileUpload1.PostedFile.FileName;
                documento.TipoArquivo = FileUpload1.PostedFile.ContentType;
                documento.UsuarioId = User.Identity.Name;
                documento.DataCadastro = DateTime.Now;
                documento.DataAlteracao = DateTime.Now;

                validacao = rnDocenteCandidatoArquivo.Valida(documento, documento.DocenteCandidatoArquivoId == -1 ? true : false);
                if (validacao.Valido)
                {
                    if (documento.DocenteCandidatoArquivoId > 0)
                    {
                        rnDocenteCandidatoArquivo.Atualiza(documento);

                        mensagem = "Arquivo atualizado com sucesso.";
                    }
                    else
                    {
                        rnDocenteCandidatoArquivo.Insere(documento);

                        mensagem = "Arquivo atualizado com sucesso.";
                    }

                    Statuslbl.Text = mensagem;
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    Statuslbl.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }

                grdDocumento.DataBind();
                //repCarrossel.DataBind();

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

                    case "ddlEleitor_Uf":
                        dadosDrop = RN.Basico.ConsultarUF();
                        PreencheDropdownList(ddlEleitor_Uf, dadosDrop, "");
                        break;

                    case "ddlCrUF":
                        dadosDrop = RN.Basico.ConsultarUF();
                        PreencheDropdownList(ddlCrUF, dadosDrop, "");
                        break;
                    case "ddlUFCNH":
                        dadosDrop = RN.Basico.ConsultarUF();
                        PreencheDropdownList(ddlUFCNH, dadosDrop, "");
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

        public object ListaDocumento(object concurso, object candidato)
        {
            try
            {
                RN.RecursosHumanos.DocenteCandidatoArquivo rnDocenteCandidatoArquivo = new RN.RecursosHumanos.DocenteCandidatoArquivo();

                if (Convert.ToString(concurso).IsNullOrEmptyOrWhiteSpace())
                {
                    return null;
                }

                return rnDocenteCandidatoArquivo.ListaDocumentoPor(Convert.ToString(concurso), Convert.ToString(candidato));
            }
            catch
            {
                return null;
            }
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
            hdnNumFunc.Value = string.Empty;
            txtLotacaoDisciplinaIngresso.Text = string.Empty;
            hdnDiscIngresso.Value = string.Empty;
            txtLotacaoMunicipio.Text = string.Empty;
            hdnMunicipioLotacao.Value = string.Empty;
            txtLotacaoRegionalSede.Text = string.Empty;
            hdnRegionalLotacao.Value = string.Empty;
            hdnSedeLotacao.Value = string.Empty;
            txtCandidato.Text = string.Empty;
            hdnDocenteCandidatoId.Value = string.Empty;
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
            txtEmailInstitucional.Text = string.Empty;
            txtRGNum.Text = string.Empty;
            txtCPF.Text = string.Empty;
            txtPisPasep.Text = string.Empty;
            txtCrpof_Num.Text = string.Empty;
            txtCprof_Serie.Text = string.Empty;
            txtEstado.Value = string.Empty;
            txtNaturalidadeUF.Text = string.Empty;

            dtDataNasc.Text = string.Empty;
            dtDataExped.Text = string.Empty;

            ddlEst_Civil.Items.Clear();
            cmbRGUF.Items.Clear();
            cmbRGEmissor.Items.Clear();
            ddDlCprof_Uf.Items.Clear();
            ddlEleitor_Uf.Items.Clear();
            ddlCrUF.Items.Clear();
            tseNaturalidade.ResetValue();

            txtStatusCandidato.Text = string.Empty;

            txtDependentes.Text = string.Empty;
            txtNumCNH.Text = string.Empty;
            ddlCategoriaCNH.ClearSelection();
            ddlUFCNH.ClearSelection();
            dtValidadeCNH.Text = string.Empty;
            txtDOC_Teleitor_Num.Text = string.Empty;
            txtDOC_Teleitor_Zona.Text = string.Empty;
            txtDOC_Teleitor_Secao.Text = string.Empty;

            txtDMIL_Cr_Num.Text = string.Empty;
            txtDMIL_Cr_Serie.Text = string.Empty;

            rblAcumulacao.ClearSelection();
            txtGLP.Text = string.Empty;
            rblExperiencia.ClearSelection();
            rblTitulacao.ClearSelection();
            rblFuncaoDiretor.ClearSelection();
            rblRubrica.ClearSelection();
            hdnSituacao.Value = string.Empty;
            dtConvocacao.Text = string.Empty;
            hdnDataConvocacao.Value = string.Empty;

            chlAnosGLP.ClearSelection();
            ddlRegionaldesejada.ClearSelection();
            chkMigracaoAnterior.Checked = false;
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

            dtDataNasc.ReadOnly = true;
            dtDataExped.ReadOnly = true;
            ddlEst_Civil.Enabled = false;
            cmbRGUF.Enabled = false;
            cmbRGEmissor.Enabled = false;
            ddDlCprof_Uf.Enabled = false;
            ddlEleitor_Uf.Enabled = false;
            ddlCrUF.Enabled = false;

            tseNaturalidade.Mode = ControlMode.View;

            txtDependentes.Enabled = false;
            txtNumCNH.Enabled = false;
            ddlCategoriaCNH.Enabled = false;
            ddlUFCNH.Enabled = false;
            dtValidadeCNH.Enabled = false;

            txtDMIL_Cr_Num.Enabled = false;
            txtDMIL_Cr_Serie.Enabled = false;

            rblAcumulacao.Enabled = false;
            txtGLP.Enabled = false;
            rblExperiencia.Enabled = false;
            rblTitulacao.Enabled = false;
            rblFuncaoDiretor.Enabled = false;
            rblRubrica.Enabled = false;
            dtConvocacao.Enabled = false;

            chlAnosGLP.Enabled = false;
            ddlRegionaldesejada.Enabled = false;
            chkMigracaoAnterior.Enabled = false;
        }

        private void HabilitaCampos()
        {
            txtEmail.ReadOnly = false;
            txtEmail.Enabled = true;
            txtDependentes.Enabled = true;
            rblAcumulacao.Enabled = true;
            txtGLP.Enabled = true;
            rblExperiencia.Enabled = true;
            rblTitulacao.Enabled = true;
            rblFuncaoDiretor.Enabled = true;
            rblRubrica.Enabled = true;
            chlAnosGLP.Enabled = true;
            ddlRegionaldesejada.Enabled = true;
            chkMigracaoAnterior.Enabled = true;

        }

        private void LimparEndereco()
        {
            txtMunicipio.Text = string.Empty;
            txtEstado.Value = string.Empty;
            txtEndereco.Text = string.Empty;
            txtCep.Text = string.Empty;
            txtEndCompl.Text = string.Empty;
            txtEndNum.Text = string.Empty;
            txtBairro.Text = string.Empty;
            txtNaturalidadeUF.Text = string.Empty;
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
            ControlaAcesso(btnEditarData, AcaoControle.editar);

            if (!Permission.AllowUpdate || !Permission.AllowInsert)
            {
                grdDocumento.Columns[3].Visible = false;
            }
        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnEditar.Visible = false;
            btnNovo.Visible = false;
            btnSalvar.Visible = false;
            btnExcluir.Visible = false;
            btnEditarData.Visible = false;
        }

        #endregion


        //public static void MsgBox(string Mensagem, Page page)
        //{
        //    ScriptManager.RegisterStartupScript(page.Page, page.Page.GetType(), "popup", "Validamsg('" + Mensagem + "');", true);

        //    //var script = @"alert('" + Mensagem + @"');";
        //    //page.Page.ClientScript.RegisterStartupScript(page.Page.GetType(), "popup", "Validamsg('" + Mensagem + "');", true);
        //    //return;
        //}

        //protected void btnExportarPDF_Click(object sender, ImageClickEventArgs e)
        //{
        //    ExportaPdf pdf = new ExportaPdf();


        //    try
        //    {
        //        iTextSharp.text.Rectangle papel;
        //        papel = PageSize.A4;

        //        MemoryStream outputStream = new MemoryStream();

        //        //Monta e Define a Saida do PDF Unificado
        //        WebClient req = new WebClient();
        //        HttpResponse response = HttpContext.Current.Response;
        //        response.Clear();
        //        response.ClearContent();
        //        response.ClearHeaders();
        //        response.Buffer = true;
        //        response.ContentType = "application/pdf";
        //        response.AddHeader("Content-Disposition", "attachment;filename=\"Documento.pdf");
        //        //Cria documento
        //        RN.RecursosHumanos.DocenteCandidatoArquivo rnDocenteCandidatoArquivo = new RN.RecursosHumanos.DocenteCandidatoArquivo();

        //        List<int> ListaArquivos = new List<int>();
        //        List<byte[]> ListaDocumentos = new List<byte[]>();
        //        List<byte[]> ListaImagens = new List<byte[]>();
        //        byte[] juntapdf = null;

        //        //MONTA ARQUIVO DE IMAGEM EM UM UNICO PDF
        //        ListaArquivos = rnDocenteCandidatoArquivo.ObtemArquivoPorDocente(tseCandidatoBusca["candidato"].ToString(), tseConcursoBusca.DBValue.ToString());

        //        foreach (var listaArquivo in ListaArquivos)
        //        {
        //            RN.RecursosHumanos.Entidades.DocenteCandidatoArquivo listDocenteCandidatoArquivo = rnDocenteCandidatoArquivo.ObtemArquivoPor(listaArquivo);

        //            MemoryStream ms = new MemoryStream(listDocenteCandidatoArquivo.Arquivo as byte[]);
        //            //Adiciona ao pdf caso o pdf seja um jpeg
        //            if (listDocenteCandidatoArquivo.TipoArquivo.Equals("image/jpeg") || listDocenteCandidatoArquivo.TipoArquivo.Equals("image/png"))
        //                ListaImagens.Add(listDocenteCandidatoArquivo.Arquivo as byte[]);
        //            else ListaDocumentos.Add(listDocenteCandidatoArquivo.Arquivo as byte[]);

        //        }

        //        if (ListaDocumentos.Count != 0) //Caso Exista PDF 
        //        {
        //            juntapdf = MergePDFs(ListaDocumentos); //Junta o PDF
        //            //Insere as Imagens no PDF
        //            for (int k = 0; k < ListaImagens.Count; k++)
        //                juntapdf = MergeImagePdf(ListaImagens[k], juntapdf); //Adiciona as imagens no PDF
        //        }
        //        else
        //        { //Se somente existir imagem                          
        //            Document docPdf = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        //            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
        //            {

        //                PdfWriter writer = PdfWriter.GetInstance(docPdf, memoryStream);
        //                docPdf.Open();
        //                for (int k = 0; k < ListaImagens.Count; k++)
        //                {
        //                    Stream stream = new MemoryStream(ListaImagens[k]);
        //                    System.Drawing.Image imgOriginal = System.Drawing.Image.FromStream(stream);

        //                    iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(imgOriginal, System.Drawing.Imaging.ImageFormat.Jpeg);
        //                    pic.ScaleToFit(600f, 300f);
        //                    pic.Alignment = Element.ALIGN_JUSTIFIED;

        //                    docPdf.NewPage();
        //                    docPdf.Add(pic);

        //                }

        //                docPdf.Close();
        //                juntapdf = memoryStream.ToArray();
        //            }
        //        }

        //        response.BinaryWrite(juntapdf);

        //    }
        //    catch (Exception ex)
        //    {
        //        lblMensagem.Text = ex.Message;
        //    }
        //}

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

        protected void btnEditarData_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                pcCandidatoDocente.ActiveTabIndex = 2;
                DesabilitaCampos();

                if (hdnSituacao.Value == "8")
                {
                    pnlDataConvocacao.Visible = true;
                    dtConvocacao.Enabled = true;


                    var controles = new[] { btnCancel};
                  
                    ControlarVisibilidadeControle(controles);

                    btnAlterarData.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnAlterarData_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new Techne.Lyceum.RN.Util.ValidacaoDados();
                RN.RecursosHumanos.DocenteCandidato rnDocenteCandidato = new Techne.Lyceum.RN.RecursosHumanos.DocenteCandidato();

                if (!hdnDocenteCandidatoId.Value.IsNullOrEmptyOrWhiteSpace() && !dtConvocacao.Text.IsNullOrEmptyOrWhiteSpace())
                {
                    if (dtConvocacao.Date > DateTime.Now)
                    {
                        lblMensagem.Text = "A data da Migração não pode ser maior que a data atual.";
                    }

                    if (Convert.ToDateTime(hdnDataConvocacao.Value) > dtConvocacao.Date)
                    {
                        lblMensagem.Text = "A data da Migração não pode ser menor que a data Convocação.";
                    }

                    if (lblMensagem.Text.IsNullOrEmptyOrWhiteSpace())
                    {

                        rnDocenteCandidato.AtualizaData(Convert.ToInt32(hdnDocenteCandidatoId.Value), User.Identity.Name, dtConvocacao.Date);

                        lblMensagem.Text = "Data atualizada com sucesso.";
                        lblMensagem.Visible = true;
                        dtConvocacao.Enabled = false;

                        btnAlterarData.Visible = false;

                        var controles = new[] { btnNovo, btnEditar, btnExcluir, btnEditarData };

                        ControlarVisibilidadeControle(controles);
                    }


                   

                }
                else
                {
                    lblMensagem.Text = "Para efetuar a troca é necessário escolher uma data.";
                }
                lblMensagem.Visible = true;

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
            }
        }

    }
}