using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Seeduc.Infra.Helpers;
using Seeduc.Infra.Validation;
using Techne.Controls;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.CartaoEstudante.Service;
using Techne.Lyceum.RN.DTOs.Agenda;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using Image = System.Drawing.Image;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/Alunos.aspx"), ControlText("Alunos"), Title("Alunos")]
    public partial class Alunos : TPage
    {
        public Alunos()
        {
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
            CadastrarAlunoCompartilhadas,
            CadastrarAlunoCompartilhadasBusca,
            NovaMatricula
        }

        private bool _mudouCurso
        {
            get { return (bool)ViewState["_mudouCurso"]; }
            set { ViewState["_mudouCurso"] = value; }
        }

        private bool _alterouFoto
        {
            get
            {
                if (ViewState["_alterouFoto"] != null)
                {
                    if (ViewState["_alterouFoto"] is bool)
                    {
                        return (bool)ViewState["_alterouFoto"];
                    }
                }

                return false;
            }

            set
            {
                ViewState["_alterouFoto"] = value;
            }
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

        private String AlunoTransf;

        public String alunoTransf
        {
            get { return AlunoTransf; }
            set { AlunoTransf = value; }
        }

        #endregion

        public static string GetUrl()
        {
            return Navigation.GetNavigation(System.Reflection.MethodBase.GetCurrentMethod()).GetUrl(new object[] { });
        }

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

        public object ListaCuidador(object aluno)
        {
            RN.NecessidadeEspecial.CuidadorAluno rnCuidadorAluno = new Techne.Lyceum.RN.NecessidadeEspecial.CuidadorAluno();

            if (!string.IsNullOrEmpty(aluno.ToString()))
            {
                return rnCuidadorAluno.ListaPor(aluno.ToString());
            }

            return null;
        }

        public object ListaLedor(object aluno)
        {
            RN.NecessidadeEspecial.LedorAluno rnLedorAluno = new Techne.Lyceum.RN.NecessidadeEspecial.LedorAluno();

            if (!string.IsNullOrEmpty(aluno.ToString()))
            {
                return rnLedorAluno.ListaPor(aluno.ToString());
            }

            return null;
        }

        public object ListaInterprete(object aluno)
        {
            RN.NecessidadeEspecial.InterpreteTurma rnInterpreteTurma = new Techne.Lyceum.RN.NecessidadeEspecial.InterpreteTurma();

            if (!string.IsNullOrEmpty(aluno.ToString()))
            {
                return rnInterpreteTurma.ListaPor(aluno.ToString());
            }

            return null;
        }

        public object ListaSalaRecurso(object aluno)
        {
            RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();

            if (!string.IsNullOrEmpty(aluno.ToString()))
            {
                return rnMatricula.ListaEnturmacaoEducacaoEspecialPor(Convert.ToString(aluno), "9999.91");
            }

            return null;
        }

        public object ListaPAPEE(object aluno)
        {
            RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();

            if (!string.IsNullOrEmpty(aluno.ToString()))
            {
                return rnMatricula.ListaEnturmacaoEducacaoEspecialPor(Convert.ToString(aluno), "9999.04");
            }

            return null;
        }

        public object ListaAEDH(object aluno)
        {
            RN.RecursosHumanos.AtendimentoOutroEspaco rnAtendimentoOutroEspaco = new Techne.Lyceum.RN.RecursosHumanos.AtendimentoOutroEspaco();

            if (!string.IsNullOrEmpty(aluno.ToString()))
            {
                return rnAtendimentoOutroEspaco.ListaPor(Convert.ToString(aluno));
            }

            return null;
        }

        #region Eventos Página

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdProgramas, "Programas Sociais");
            TituloGrid(grdDocumentos, "Documentos Entregues");
            TituloGrid(grdConfirmacao, "Histórico de Confirmação de Matrícula");
            TituloGrid(grdRenovacaoMatricula, "Renovação de Matrícula");
            TituloGrid(grdBusca, "Pesquisa Aluno");
            TituloGrid(grdBuscaIrmaos, "Resultado Pesquisa Irmãos");
            TituloGrid(grdBuscaIrmaosCadastrados, "Irmãos");
            TituloGrid(grdLedor, "");
            TituloGrid(grdCuidador, "");
            TituloGrid(grdInterprete, "");
            TituloGrid(grdSalaRecurso, "");
            TituloGrid(grdPAPEE, "");
            TituloGrid(grdAEDH, "Atendimentos");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                RN.Perfil rnPerfil = new Perfil();
                RN.PeriodoLetivo rnPeriodoLetivo = new Techne.Lyceum.RN.PeriodoLetivo();

                lblMensagem.Text = string.Empty; //LIMPAR ERROS  
                lblMensagemAtendimentoEspecializado.Text = string.Empty;
                lblErroConfirmacao.Text = string.Empty;
                lblConfirmarMatricula.Text = string.Empty;
                lblMensagemIrmaos.Text = string.Empty;

                this.ValidarCampos();

                if (!IsPostBack)
                {
                    CarregaPaisNascimento();
                    CarregaNacionalidade();
                    CarregaEtnia();
                    CarregaEstadoCivil();
                    CarregaNecessidadeEspecial();
                    CarregaOutroEnsino();
                    CarregaCredo();
                    CarregaTipoSanguineo();
                    CarregaRGTipoPessoa();
                    CarregaRGEmissor();
                    CarregaTipoIngresso();
                    CarregaUfRg();
                    CarregaUfCertNasc();
                    CarregaUFCartorio();
                    CarregaModalidade();
                    CarregaNivel();
                    CarregarList();
                    CarregaUFNaturalidade();
                    CarregaRedeEnsinoOrigem();
                    CarregaPeriodoIngresso();
                    CarregaAnoIngresso();
                    CarregaTurno();
                    CarregaSerie();
                    CarregaRecursoAplicacaoProva();
                    CarregaTipoRecursoNecessidadeEspecial();
                    CarregaTipoTranstorno();

                    // abrir sempre na primeira aba
                    tab.ActiveTabIndex = 0;

                    // para a primeira vez que a página é carregada o tipo de operação será inicial
                    this._tipoOperacao = TipoOperacao.Inicial;

                    Page.Title = Page.Title + " - " + GetPageTitle();

                    if (Request.QueryString.Keys.Count > 0)
                    {
                        if (Request.QueryString["ChaveInscricaoCompartilhadas"] != null)
                        {
                            this.CarregaDadosInscricaoCompartilhadas();
                        }
                        else
                        {
                            this.CarregarDadosAluno();
                        }
                    }
                    if (rnPerfil.PossuiPerfilExclusaoAEDHPor(User.Identity.Name) || RN.Usuarios.UsuarioPrivilegiado(HttpContext.Current.User.Identity.Name))
                    {
                        hdnPerfilAEDH.Value = "S";
                    }

                    this.ControlarTipoOperacao();


                    dtDataExped.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    dboDOC_CertNasc_DtEmissao.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                    if (Request.InputStream.Length > 0)
                    {
                        using (StreamReader reader = new StreamReader(Request.InputStream))
                        {
                            RN.Util.Imagem rnImagem = new RN.Util.Imagem();
                            string hexString = Server.UrlEncode(reader.ReadToEnd());
                            byte[] foto = rnImagem.ConvertHexToBytes(hexString);
                            Session["imageBytes"] = rnImagem.RedimencionaImagemPor(foto, 240, 240);
                        }
                    }
                }

                this.ObterImagem();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblErroConfirmacao.Text = this.lblMensagem.Text;
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            this.ControlarEnderecoPais();
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(btnExcluir, AcaoControle.excluir);
            ControlaAcesso(grdBusca);
            ControlaAcesso(grdDocumentos);
            ControlaAcesso(grdProgramas);
            ControlaAcesso(grdConfirmacao);
            ControlaAcesso(grdRenovacaoMatricula);
            ControlaAcesso(grdAEDH);


            bool nascidoFora = !string.IsNullOrEmpty(txtPaisNasc.Text) && txtPaisNasc.Text.Trim().ToUpper() != "BRASIL";

            bool modoEdicao = (_tipoOperacao == TipoOperacao.Novo
                            || _tipoOperacao == TipoOperacao.Alterar
                            || _tipoOperacao == TipoOperacao.NovaMatricula
                            || _tipoOperacao == TipoOperacao.CadastrarAlunoCompartilhadas);

            // País: em edição esconde para nato (preenchido automaticamente),
            //       em consulta mostra SEMPRE
            if (modoEdicao)
            {
                lblPaisNasc.Visible = nascidoFora;
                txtPaisNasc.Visible = nascidoFora;
            }
            else
            {
                // modo consulta — mostra país sempre que tiver valor
                lblPaisNasc.Visible = !string.IsNullOrEmpty(txtPaisNasc.Text);
                txtPaisNasc.Visible = !string.IsNullOrEmpty(txtPaisNasc.Text);
            }

            lblNaturalidadeUF.Visible = true;
            txtUFNascimento.Visible = true;

            if (modoEdicao)
            {
                tseNaturalidade.Visible = false;
                tseNaturalidadeEstrangeira.Visible = false;

                if (!string.IsNullOrEmpty(ddlNacionalidade.SelectedValue))
                {
                    tseNaturalidade.Visible = !nascidoFora;
                    tseNaturalidadeEstrangeira.Visible = nascidoFora;
                }
            }
            else
            {
                tseNaturalidade.Visible = !nascidoFora;
                tseNaturalidadeEstrangeira.Visible = nascidoFora;
            }
        }

        #endregion

        #region Eventos Botões

        protected void btnConfirmarFoto_Click(object sender, ImageClickEventArgs e)
        {
            if (flFoto.PostedFile == null || flFoto.PostedFile.FileName == "")
            {
                lblMensagem.Text = "Não há imagem a ser salva.";
            }
            else
            {

                byte[] FotoPessoa = new byte[flFoto.PostedFile.InputStream.Length];

                flFoto.PostedFile.InputStream.Read(FotoPessoa, 0, FotoPessoa.Length);
                string msg = RN.FotoPessoa.ValidaFoto(FotoPessoa);
                if (msg != string.Empty)
                {
                    lblMensagem.Text = msg;
                    return;
                }

                bimgFotoPessoa.ContentBytes = FotoPessoa;
                _alterouFoto = true;
            }

            ControlarTSearchs();
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ViewState["confirmaalteracao"] = null;
                _tipoOperacao = TipoOperacao.Inicial;

                ControlarTipoOperacao();
                ControlarTSearchs();
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
                _tipoOperacao = TipoOperacao.Inicial;
                ControlarTipoOperacao();
                ControlarTSearchs();
                txtNomeMaeAlunoNovo.Text = string.Empty;
                txtNomeAlunoNovo.Text = string.Empty;
                dtDataNascAlunoNovo.Text = string.Empty;
                chkNaoDeclarMaeNovo.Checked = false;
                txtNomeMaeAlunoNovo.ReadOnly = false;

                pnlBuscaAlunoNovo.Visible = false;
                pnlNovaMatricula.Visible = true;
                ControlaVisibilidadeMensagemBloqueio();
                this.CarregaAnoMatricula();
                this.CarregaPeriodoMatricula();
                CarregaModalidade();
                CarregaNivel();

                CarregaUnidadeEnsinoMatricula();
                CarregarFiltroCursoMatricula();

                btnNovo.Visible = false;
                tseAluno.Enabled = false;
                btnNovoBusca.Visible = false;
                btnCancel.Visible = true;

                pntabEspecializado.Enabled = false;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                lblMensagemAlunoNovo.Text = string.Empty;
                btnNovoBusca.Visible = false;

                if (string.IsNullOrEmpty(txtNomeAlunoNovo.Text) && string.IsNullOrEmpty(txtNomeMaeAlunoNovo.Text) && string.IsNullOrEmpty(dtDataNascAlunoNovo.Text))
                {
                    lblMensagemAlunoNovo.Text = "Para efetuar a busca é necessário preencher pelo menos dois campos.";
                    return;
                }
                else
                {
                    int cont = 0;
                    Regex regex = new Regex(@"\s{2,}");

                    string aluno = tseAluno.DBValue.ToString();
                    string NomeCompl = regex.Replace(txtNomeAlunoNovo.Text.Trim().ToUpper(), " ");
                    string NomeMae = regex.Replace(txtNomeMaeAlunoNovo.Text.Trim().ToUpper(), " ");

                    if (!string.IsNullOrEmpty(NomeCompl)) cont = cont + 1;
                    if (!string.IsNullOrEmpty(NomeMae)) cont = cont + 1;
                    if (!string.IsNullOrEmpty(dtDataNascAlunoNovo.Text)) cont = cont + 1;

                    if (cont < 2)
                    {
                        lblMensagemAlunoNovo.Text = "Para fazer a busca será necessário preencher pelo menos dois campos.";
                        return;
                    }

                    var validacao = Aluno.ValidarBuscaNovoAluno(aluno, NomeCompl, NomeMae, Convert.ToDateTime(dtDataNascAlunoNovo.Value));

                    if (validacao.Valido)
                    {
                        txtNomeAlunoNovo.Text = NomeCompl;
                        txtNomeMaeAlunoNovo.Text = NomeMae;
                        btnNovoBusca.Visible = true;
                    }
                    else
                    {
                        lblMensagemAlunoNovo.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnNovoBusca_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (Request.QueryString.Keys.Count > 0)
                {
                    if (Request.QueryString["ChaveInscricaoCompartilhadas"] != null)
                    {
                        _tipoOperacao = TipoOperacao.CadastrarAlunoCompartilhadas;
                    }
                    else
                    {
                        _tipoOperacao = TipoOperacao.NovaMatricula;
                    }
                }

                ControlarTipoOperacao();
                ControlarTSearchs();

                txtNomeCompl.Text = txtNomeAlunoNovo.Text;
                txtNomeMae.Text = txtNomeMaeAlunoNovo.Text;
                dtDataNasc.Text = dtDataNascAlunoNovo.Text;

                if (chkNaoDeclarMaeNovo.Checked)
                {
                    chkNaoDeclarMae.Enabled = false;
                    chkNaoDeclarMae.Checked = true;
                    txtNomeMae.Text = chkNaoDeclarMae.Text.ToUpper();
                    txtNomeMae.ReadOnly = true;
                    txtTelefoneMae.Enabled = false;
                    txtTelefoneMae.Text = string.Empty;
                    txtCPFMae.Text = string.Empty;
                    txtCPFMae.Enabled = false;
                    chkFalecidaMae.Checked = false;
                    chkFalecidaMae.Enabled = false;
                    chkDeclaroAusenciaMae.Visible = true;
                    DesabilitaResponsavelLegal("H", "Mãe");
                }
                else
                {
                    if (!string.IsNullOrEmpty(txtNomeMaeAlunoNovo.Text))
                    {
                        chkNaoDeclarMae.Checked = false;
                        chkNaoDeclarMae.Enabled = false;
                    }
                    else
                    {
                        chkNaoDeclarMae.Enabled = true;
                    }
                }

                txtNomeCompl.Enabled = txtNomeCompl.Text.IsNullOrEmptyOrWhiteSpace();
                txtNomeMae.Enabled = txtNomeMae.Text.IsNullOrEmptyOrWhiteSpace();
                dtDataNasc.Enabled = dtDataNasc.Text.IsNullOrEmptyOrWhiteSpace();

                pnlBuscaAlunoNovo.Visible = false;
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
                RN.Aluno rnAluno = new Aluno();

                if ((!tseAluno.DBValue.IsNull) && (tseAluno.IsValidDBValue))
                {
                    if (rnAluno.EhAlunoSemNecessidadeEspecialPor(txtAluno.Text))
                    {
                        pntabEspecializado.Enabled = false;
                    }
                    else
                    {
                        pntabEspecializado.Enabled = true;
                        tab.Tabs[5].Enabled = true;
                    }

                    //verifica se o aluno está encerrado e não permite
                    if (txtSituacao.Text.Trim().ToUpper() != "ATIVO")
                    {
                        lblMensagem.Text = "Não é possível alterar dados de um aluno que não está ativo.";
                        return;
                    }

                    _tipoOperacao = TipoOperacao.Alterar;
                    ControlarTipoOperacao();
                    ControlarTSearchs();
                }
                else
                {
                    this._tipoOperacao = TipoOperacao.Inicial;
                    lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnExcluir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this._tipoOperacao = TipoOperacao.Excluir;

                this.ControlarTipoOperacao();
                this.ControlarTSearchs();
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
                this.Salvar();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnProximo_Click(object sender, EventArgs e)
        {
            try
            {
                pntabDadosPessoais.Visible = false;
                pntabDadosEscolares.Visible = true;
                pntabTransporteEscolar.Visible = false;
                pntabDocumentos.Visible = false;
                pntabProgramas.Visible = false;
                pntabEspecializado.Visible = false;
                pntabIrmaos.Visible = false;
                pntabAEDH.Visible = false;
                tab.ActiveTabIndex = 1;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnAnterior_Click(object sender, EventArgs e)
        {
            try
            {
                pntabDadosPessoais.Visible = true;
                pnlLoginCartao.Visible = false;
                pntabDadosEscolares.Visible = false;
                pntabTransporteEscolar.Visible = false;
                pntabDocumentos.Visible = false;
                pntabProgramas.Visible = false;
                pntabEspecializado.Visible = false;
                pntabIrmaos.Visible = false;
                pntabAEDH.Visible = false;
                tab.ActiveTabIndex = 0;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnProximoTransp_Click(object sender, EventArgs e)
        {
            try
            {
                pntabDadosEscolares.Visible = false;
                pntabDadosPessoais.Visible = false;
                pntabTransporteEscolar.Visible = false;
                pntabDocumentos.Visible = true;
                pntabProgramas.Visible = false;
                pntabIrmaos.Visible = false;
                pntabEspecializado.Visible = false;
                pntabAEDH.Visible = false;
                tab.ActiveTabIndex = 3;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnAnteriorTransp_Click(object sender, EventArgs e)
        {
            try
            {
                pntabDadosPessoais.Visible = false;
                pnlLoginCartao.Visible = false;
                pntabDadosEscolares.Visible = true;
                pntabTransporteEscolar.Visible = false;
                pntabDocumentos.Visible = false;
                pntabProgramas.Visible = false;
                pntabEspecializado.Visible = false;
                pntabIrmaos.Visible = false;
                pntabAEDH.Visible = false;
                tab.ActiveTabIndex = 1;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnProximo2_Click(object sender, EventArgs e)
        {
            try
            {
                // Oculta todas as abas
                pntabDadosEscolares.Visible = false;
                pntabDadosPessoais.Visible = false;
                pntabTransporteEscolar.Visible = true;
                pntabDocumentos.Visible = false;
                pntabProgramas.Visible = false;
                pntabIrmaos.Visible = false;
                pntabEspecializado.Visible = false;
                pntabAEDH.Visible = false;

                // Ativa a aba correta
                tab.ActiveTabIndex = 2; // ✅ Transporte Escolar
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnAnterior2_Click(object sender, EventArgs e)
        {
            try
            {
                pntabDadosPessoais.Visible = false;
                pntabDadosEscolares.Visible = false;
                pntabTransporteEscolar.Visible = true;
                pntabDocumentos.Visible = false;
                pntabProgramas.Visible = false;
                pntabIrmaos.Visible = false;
                pntabAEDH.Visible = false;
                tab.ActiveTabIndex = 2;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnProximo3_Click(object sender, EventArgs e)
        {
            try
            {
                pntabDadosPessoais.Visible = false;
                pntabDadosEscolares.Visible = false;
                pntabTransporteEscolar.Visible = false;
                pntabDocumentos.Visible = false;

                pntabProgramas.Visible = true;
                pntabEspecializado.Visible = false;
                pntabIrmaos.Visible = false;
                pntabAEDH.Visible = false;
                tab.ActiveTabIndex = 4;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnAnterior3_Click(object sender, EventArgs e)
        {
            try
            {
                pntabDadosPessoais.Visible = false;
                pntabDadosEscolares.Visible = false;
                pntabTransporteEscolar.Visible = false;
                pntabDocumentos.Visible = true;
                pntabProgramas.Visible = false;
                pntabIrmaos.Visible = false;
                pntabEspecializado.Visible = false;
                pntabAEDH.Visible = false;
                tab.ActiveTabIndex = 3;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnProximo4_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbNecessidadeEspecial.SelectedValue == "30")
                {
                    pntabEspecializado.Visible = false;
                    pntabDadosPessoais.Visible = false;
                    pntabDadosEscolares.Visible = false;
                    pntabTransporteEscolar.Visible = false;
                    pntabDocumentos.Visible = false;
                    pntabProgramas.Visible = !chkPossuiIrmao.Checked;
                    //pntabAEDH.Visible = chkEscolarOutroEspaco.Checked;
                    pntabIrmaos.Visible = chkPossuiIrmao.Checked;
                    btnProximo4.Visible = chkPossuiIrmao.Checked;
                    chkDeclaroNecessidadeEspecial.Checked = false;
                    chkDeclaroNecessidadeEspecial.Text = "Declaro estar de posse do Laudo Médico ou Parecer Pedagógico";
                    chkDeclaroNecessidadeEspecial.Visible = false;

                    if (pntabIrmaos.Visible)
                        tab.ActiveTabIndex = 6;
                    else
                        tab.ActiveTabIndex = 4;

                    // Navegação para aba AEDH controlada por btnProximo6_Click
                }
                else
                {
                    pntabEspecializado.Visible = true;
                    pntabDadosPessoais.Visible = false;
                    pntabDadosEscolares.Visible = false;
                    pntabDocumentos.Visible = false;
                    pntabTransporteEscolar.Visible = false;
                    pntabProgramas.Visible = false;
                    pntabIrmaos.Visible = chkPossuiIrmao.Checked;
                    //pntabAEDH.Visible = chkEscolarOutroEspaco.Checked;
                    chkDeclaroNecessidadeEspecial.Visible = true;
                    tab.ActiveTabIndex = 5;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnAnterior4_Click(object sender, EventArgs e)
        {
            try
            {
                pntabEspecializado.Visible = false;
                pntabDadosPessoais.Visible = false;
                pntabDadosEscolares.Visible = false;
                pntabTransporteEscolar.Visible = false;
                pntabDocumentos.Visible = true;
                pntabProgramas.Visible = false;
                pntabIrmaos.Visible = false;
                pntabAEDH.Visible = false;
                tab.ActiveTabIndex = 3;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnProximo5_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkPossuiIrmao.Checked)
                {
                    pntabEspecializado.Visible = false;
                    pntabDadosPessoais.Visible = false;
                    pntabDadosEscolares.Visible = false;
                    pntabTransporteEscolar.Visible = false;
                    pntabDocumentos.Visible = false;
                    pntabProgramas.Visible = false;
                    pntabIrmaos.Visible = true;
                    pntabAEDH.Visible = false;

                    tab.ActiveTabIndex = 6;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnAnterior5_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbNecessidadeEspecial.SelectedValue == "30")
                {
                    chkDeclaroNecessidadeEspecial.Checked = false;
                    chkDeclaroNecessidadeEspecial.Visible = false;
                    chkDeclaroNecessidadeEspecial.Text = "Declaro estar de posse do Laudo Médico ou Parecer Pedagógico";
                    pntabEspecializado.Visible = false;
                    pntabDadosPessoais.Visible = false;
                    pntabDadosEscolares.Visible = false;
                    pntabTransporteEscolar.Visible = false;
                    pntabDocumentos.Visible = false;
                    pntabProgramas.Visible = true;
                    pntabIrmaos.Visible = false;
                    pntabAEDH.Visible = false;
                    tab.ActiveTabIndex = 4;
                }
                else
                {
                    pntabEspecializado.Visible = true;
                    pntabDadosPessoais.Visible = false;
                    pntabDadosEscolares.Visible = false;
                    pntabTransporteEscolar.Visible = false;
                    pntabDocumentos.Visible = false;
                    pntabProgramas.Visible = false;
                    pntabIrmaos.Visible = false;
                    pntabAEDH.Visible = false;
                    chkDeclaroNecessidadeEspecial.Visible = true;
                    tab.ActiveTabIndex = 5;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        protected void btnProximo6_Click(object sender, EventArgs e)
        {
            try
            {
                pntabEspecializado.Visible = false;
                pntabDadosPessoais.Visible = false;
                pntabDadosEscolares.Visible = false;
                pntabDocumentos.Visible = false;
                pntabTransporteEscolar.Visible = false;
                pntabProgramas.Visible = false;
                pntabIrmaos.Visible = false;
                pntabAEDH.Visible = true;
                tab.ActiveTabIndex = 7;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnAnterior6_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkPossuiIrmao.Checked)
                {
                    pntabEspecializado.Visible = false;
                    pntabDadosPessoais.Visible = false;
                    pntabDadosEscolares.Visible = false;
                    pntabTransporteEscolar.Visible = false;
                    pntabDocumentos.Visible = false;
                    pntabProgramas.Visible = false;
                    pntabIrmaos.Visible = true;
                    pntabAEDH.Visible = false;

                    tab.ActiveTabIndex = 6;
                }
                else
                {
                    pntabEspecializado.Visible = true;
                    pntabDadosPessoais.Visible = false;
                    pntabDadosEscolares.Visible = false;
                    pntabTransporteEscolar.Visible = false;
                    pntabDocumentos.Visible = false;
                    pntabProgramas.Visible = false;
                    pntabIrmaos.Visible = false;
                    pntabAEDH.Visible = false;
                    tab.ActiveTabIndex = 5;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }



        protected void btnConfirmarTransf_Click(object sender, EventArgs e)
        {
            try
            {
                string queryString = MontarQueryString(hdnAlunoTransf.Value);

                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                Response.Redirect("SolicitacaoTransferenciaAluno.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        #endregion

        #region Eventos Tsearch

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                if (!tseAluno.DBValue.IsNull)
                {
                    if (tseAluno.IsValidDBValue)
                    {
                        this._tipoOperacao = TipoOperacao.Consultar;

                        lblMensagem.Text = string.Empty;
                        lblErroConfirmacao.Text = string.Empty;

                    }
                    else
                    {
                        this._tipoOperacao = TipoOperacao.Inicial;

                        lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                    _tipoOperacao = TipoOperacao.Inicial;
                }

                ControlarTipoOperacao();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseCurso_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                    return;

                if (_tipoOperacao == TipoOperacao.Alterar)
                    _mudouCurso = true;


                if (tseCurso.DBValue.IsNull || !tseCurso.IsValidDBValue)
                {
                    cmbSerie.Items.Clear();
                    cmbTurno.Items.Clear();
                }
                else
                {
                    CarregaTurno();
                    cmbCurriculo.DataBind();
                    cmbCurriculo.ClearSelection();
                    cmbSerie.Items.Clear();
                    if (!string.IsNullOrEmpty(cmbTurno.SelectedValue) && !tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue)
                    {
                        if (!string.IsNullOrEmpty(cmbCurriculo.SelectedValue))
                        {
                            CarregaSerie();
                        }
                    }
                    ControlarTSearchs();


                    string tipo_curso = RN.Curso.ConsultarTipoProfCurso(Convert.ToString(tseCurso["curso"]));

                    if (tipo_curso == "Concomitante/Subsequente")
                    {
                        lblTipoCurso.Visible = true;
                        ddlTipoCurso.Visible = true;
                    }
                    else
                    {
                        lblTipoCurso.Visible = false;
                        ddlTipoCurso.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseNaturalidade_Changed(object sender, EventArgs args)
        {
            try
            {
                if (tseNaturalidade.IsValidDBValue
                    && !tseNaturalidade.DBValue.IsNull)
                    txtUFNascimento.Text = tseNaturalidade["uf_sigla"].ToString();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeFis_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseInstituicao_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseUnidadeEns_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseUnidadeEnsinoMatricula_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseMunicipio_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseAluno_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseNaturalidade_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseNaturalidadeEstrangeira_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tsePessoa_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseCurso_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseCursoIngresso_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseUnidadeEns_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                tseCurso.ResetValue();
                CarregarFiltroCurso();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeEnsConfMatricula_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseUnidadeEnsinoMatricula_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                lblMensagemBloqueio.Text = string.Empty;
                ddlNivelMatricula.ClearSelection();
                ddlModalidadeMatricula.ClearSelection();
                tseCursoMatricula.ResetValue();
                btnProsseguirMatricula.Visible = false;

                if (!tseUnidadeEnsinoMatricula.DBValue.IsNull)
                {
                    if (tseUnidadeEnsinoMatricula.IsValidDBValue)
                    {
                        if (tseUnidadeEnsinoMatricula["situacao"] == "ESTADUAL")
                        {
                            CarregarFiltroCursoMatricula();
                            btnProsseguirMatricula.Visible = true;
                            hdnUnidade.Value = tseUnidadeEnsinoMatricula.DBValue.ToString();
                        }
                        else
                        {
                            lblMensagemBloqueio.Text = "Unidade escolhida na inclusão não pertence a rede de ensino da SEEDUC.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        #endregion

        #region Eventos Combo

        // Mantido para compatibilidade — ddlPaisNasc substituído por txtPaisNasc
        protected void ddlPaisNasc_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LimparEnderecoNaturalidade();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void cmbPaises_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LimparEndereco();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlRGTipoPessoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtComplIdentidade.ReadOnly = true;
                txtComplIdentidade.Text = string.Empty;

                if (ddlRGTipoPessoa.SelectedValue == "RG")
                {
                    lblRG_Num.Text = "Número*: ";
                    lblRG_Num.Font.Bold = true;

                    lblRG_UF.Text = "Estado*: ";
                    lblRG_UF.Font.Bold = true;

                    lblRG_Emissor.Text = "Órgão Emissor*: ";
                    lblRG_Emissor.Font.Bold = true;

                    lblRG_Data.Text = "Data de Expedição*: ";
                    lblRG_Data.Font.Bold = true;

                    if (ddlNacionalidade.SelectedItem.Text == "BRASILEIRA")
                    {
                        txtComplIdentidade.ReadOnly = false;
                    }

                }
                else if (ddlRGTipoPessoa.SelectedValue == string.Empty)
                {
                    lblRG_Num.Text = "Número: ";
                    lblRG_Num.Font.Bold = false;

                    lblRG_UF.Text = "Estado: ";
                    lblRG_UF.Font.Bold = false;

                    lblRG_Emissor.Text = "Órgão Emissor: ";
                    lblRG_Emissor.Font.Bold = false;

                    lblRG_Data.Text = "Data de Expedição: ";
                    lblRG_Data.Font.Bold = false;
                }
                else if (ddlRGTipoPessoa.SelectedValue == "PASSAPORTE")
                {
                    lblRG_Num.Text = "Número: ";
                    lblRG_Num.Font.Bold = false;

                    lblRG_UF.Text = "Estado: ";
                    lblRG_UF.Font.Bold = false;

                    lblRG_Emissor.Text = "Órgão Emissor: ";
                    lblRG_Emissor.Font.Bold = false;
                    cmbRGEmissor.Enabled = true;

                    lblRG_Data.Text = "Data de Expedição: ";
                    lblRG_Data.Font.Bold = false;

                    CarregaRGEmissor();
                    cmbRGEmissor.SelectedValue = string.Empty;
                }
                else
                {
                    lblRG_Num.Text = "Número*: ";
                    lblRG_Num.Font.Bold = true;

                    lblRG_UF.Text = "Estado: ";
                    lblRG_UF.Font.Bold = false;

                    lblRG_Emissor.Text = "Órgão Emissor*: ";
                    lblRG_Emissor.Font.Bold = true;

                    lblRG_Data.Text = "Data de Expedição: ";
                    lblRG_Data.Font.Bold = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlModalidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tseCurso.ResetValue();
                CarregarFiltroCurso();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tseCurso.ResetValue();
                CarregarFiltroCurso();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void cmbSemIngresso_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbSerie.Items.Clear();
                if (!cmbSemIngresso.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    if (!cmbTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && (!tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue) && !cmbCurriculo.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                    {
                        CarregaSerie();
                    }
                    ControlarTSearchs();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void cmbAnoIngresso_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbSerie.Items.Clear();
                if (!cmbAnoIngresso.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    cmbSerie.Items.Clear();
                    if (!cmbTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && (!tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue) && !cmbCurriculo.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                    {
                        CarregaSerie();
                    }
                    ControlarTSearchs();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlUFNaturalidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!txtUFNascimento.Text.IsNullOrEmptyOrWhiteSpace())
                {
                    tseNaturalidade.ResetValue();
                    tseNaturalidade.SqlWhere = " UF_SIGLA = '" + txtUFNascimento.Text + "'";
                }
                tseNaturalidade.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlRedeEnsinoOrigem_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtTempoAfastamento.Text = string.Empty;
                lblTempoAfastamento.Visible = false;
                txtTempoAfastamento.Visible = false;

                if (!ddlRedeEnsinoOrigem.SelectedValue.IsNullOrEmptyOrWhiteSpace() && ddlRedeEnsinoOrigem.SelectedValue == "Afastado")
                {
                    lblTempoAfastamento.Visible = true;
                    txtTempoAfastamento.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlGratuidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlPoderPublicoTransp.ClearSelection();
                ddlPoderPublicoTransp.Enabled = false;
                chkModais.ClearSelection();
                chkModais.Enabled = false;

                if (!ddlGratuidade.SelectedValue.IsNullOrEmptyOrWhiteSpace() && (ddlGratuidade.SelectedValue == "S"))
                {
                    ddlPoderPublicoTransp.SelectedValue = "Estadual";
                    ddlPoderPublicoTransp.Enabled = true;
                    chkModais.Enabled = true;
                }

                if (!ddlGratuidade.SelectedValue.IsNullOrEmptyOrWhiteSpace() && (ddlGratuidade.SelectedValue == "N"))
                {

                    chkRodoviario.ClearSelection();
                    chkRodoviario.Visible = false;
                    lblRodoviario.Visible = false;

                    chkAquaviario.ClearSelection();
                    chkAquaviario.Visible = false;
                    lblAquaviario.Visible = false;

                    chkOnibus.ClearSelection();
                    chkOnibus.Visible = false;
                    lblOnibus.Visible = false;

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkModais_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //chkRodoviario.ClearSelection();
                //chkRodoviario.Visible = false;
                //lblRodoviario.Visible = false;

                //chkAquaviario.ClearSelection();
                //chkAquaviario.Visible = false;
                //lblAquaviario.Visible = false;

                //chkOnibus.ClearSelection();
                //chkOnibus.Visible = false;
                //lblOnibus.Visible = false;

                foreach (ListItem item in chkModais.Items)
                {
                    if (item.Selected && item.Text == "TRANSPORTE RURAL")
                    {
                        //chkModais.ClearSelection();
                        //item.Selected = true;

                        chkRodoviario.ClearSelection();
                        chkRodoviario.Visible = true;
                        lblRodoviario.Visible = true;

                        chkAquaviario.ClearSelection();
                        chkAquaviario.Visible = true;
                        lblAquaviario.Visible = true;

                        return;
                    }
                    else if (!item.Selected && item.Text == "TRANSPORTE RURAL")
                    {
                        chkRodoviario.ClearSelection();
                        chkRodoviario.Visible = false;
                        lblRodoviario.Visible = false;

                        chkAquaviario.ClearSelection();
                        chkAquaviario.Visible = false;
                        lblAquaviario.Visible = false;
                    }

                    //if (item.Selected && item.Text == "CARRO COM ACESSIBILIDADE (CADEIRANTE)")
                    //{
                    //    //chkModais.ClearSelection();
                    //    //item.Selected = true;
                    //}

                    if (item.Selected && item.Text == "ÔNIBUS")
                    {
                        //chkModais.ClearSelection();
                        //item.Selected = true;

                        chkOnibus.Visible = true;
                        lblOnibus.Visible = true;
                    }
                    else if (!item.Selected && item.Text == "ÔNIBUS")
                    {
                        chkOnibus.ClearSelection();
                        chkOnibus.Visible = false;
                        lblOnibus.Visible = false;
                    }

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkAquaviario_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem item in chkAquaviario.Items)
                {
                    if (item.Selected && item.Text == "Não utiliza transporte Aquaviário")
                    {
                        chkAquaviario.ClearSelection();
                        item.Selected = true;
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkNaoDeclarMae_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtNomeMae.ReadOnly = true;
                txtCPFMae.Enabled = true;
                txtTelefoneMae.Enabled = true;
                txtNomeMae.Text = string.Empty;
                chkFalecidaMae.Enabled = true;
                chkDeclaroAusenciaMae.Visible = false;

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), string.Format("CheckedChangedMae('{0}','{1}')", this.chkNaoDeclarMae.ClientID, this.chkDeclaroAusenciaMae.ClientID), true);

                if (chkNaoDeclarMae.Checked)
                {
                    txtNomeMae.Text = chkNaoDeclarMae.Text.ToUpper();
                    txtNomeMae.ReadOnly = true;
                    txtCPFMae.Enabled = false;
                    txtTelefoneMae.Enabled = false;
                    txtCPFMae.Text = string.Empty;
                    txtTelefoneMae.Text = string.Empty;
                    chkFalecidaMae.Checked = false;
                    chkFalecidaMae.Enabled = false;
                    chkDeclaroAusenciaMae.Visible = true;
                    DesabilitaResponsavelLegal("H", "Mãe");
                }
                else
                {
                    DesabilitaResponsavelLegal("D", "Mãe");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkNaoDeclarMaeNovo_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtNomeMaeAlunoNovo.ReadOnly = false;
                txtNomeMaeAlunoNovo.Text = string.Empty;

                if (chkNaoDeclarMaeNovo.Checked)
                {
                    txtNomeMaeAlunoNovo.Text = chkNaoDeclarMaeNovo.Text.ToUpper();
                    txtNomeMaeAlunoNovo.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkNaoDeclarPai_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtNomePai.ReadOnly = false;
                txtCPFPai.Enabled = true;
                txtTelefonePai.Enabled = true;
                txtNomePai.Text = string.Empty;
                chkFalecidoPai.Enabled = true;
                chkDeclaroAusenciaPai.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), string.Format("CheckedChangedPai('{0}','{1}')", this.chkNaoDeclarPai.ClientID, this.chkDeclaroAusenciaPai.ClientID), true);

                if (chkNaoDeclarPai.Checked)
                {
                    txtNomePai.Text = chkNaoDeclarPai.Text.ToUpper();
                    txtCPFPai.Text = string.Empty;
                    txtTelefonePai.Text = string.Empty;
                    txtNomePai.ReadOnly = true;
                    txtCPFPai.Enabled = false;
                    txtTelefonePai.Enabled = false;
                    chkFalecidoPai.Checked = false;
                    chkFalecidoPai.Enabled = false;
                    chkDeclaroAusenciaPai.Visible = true;
                    DesabilitaResponsavelLegal("H", "Pai");
                }
                else
                {
                    DesabilitaResponsavelLegal("D", "Pai");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkFalecidaMae_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkFalecidaMae.Checked)
                {
                    DesabilitaResponsavelLegal("H", "Mãe");
                }
                else
                {
                    DesabilitaResponsavelLegal("D", "Mãe");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkFalecidoPai_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkFalecidoPai.Checked)
                {
                    DesabilitaResponsavelLegal("H", "Pai");
                }
                else
                {
                    DesabilitaResponsavelLegal("D", "Pai");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkPossuiIrmao_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkPossuiIrmao.Checked && (_tipoOperacao == TipoOperacao.Novo || _tipoOperacao == TipoOperacao.NovaMatricula))
                {
                    tab.Tabs[6].Enabled = false;
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Para localizar o irmão do aluno matriculado na rede, realize a busca através da aba \"Irmão\",  após gravar esse cadastro. ');", true);
                }
                else if (chkPossuiIrmao.Checked && (_tipoOperacao != TipoOperacao.Novo && _tipoOperacao != TipoOperacao.NovaMatricula))
                {
                    tab.Tabs[6].Enabled = true;
                    pntabIrmaos.Visible = true;
                    pnIrmaos.Enabled = true;
                    pntabDadosPessoais.Visible = false;
                    tab.ActiveTabIndex = 6;
                }
                else if (!chkPossuiIrmao.Checked && grdBuscaIrmaosCadastrados.VisibleRowCount > 0)
                {
                    tab.Tabs[6].Enabled = true;
                    chkPossuiIrmao.Checked = true;
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Esta opção não pode ser desmarcada devido possuir irmãos cadastrados. Verifique a aba \"Irmão\".');", true);
                }
                else
                {
                    tab.Tabs[6].Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void rblPossuiTranstorno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pnlTipoTranstorno.Visible = false;

                if (rblPossuiTranstorno.SelectedValue == "S")
                {
                    pnlTipoTranstorno.Visible = true;
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void rblResponsavel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblNomeResponsavel.Visible = false;
                txtNomeResponsavel.Visible = false;
                lblCPFResponsavel.Visible = false;
                txtCPFResponsavel.Visible = false;
                lblTelefoneResponsavel.Visible = false;
                txtTelefoneResp.Visible = false;
                var verifica = false;

                foreach (ListItem item in rblResponsavel.Items)
                {
                    if (item.Selected)
                    {
                        if (item.Text == "Outros")
                        {
                            verifica = true;
                            lblNomeResponsavel.Visible = true;
                            txtNomeResponsavel.Visible = true;
                            lblCPFResponsavel.Visible = true;
                            txtCPFResponsavel.Visible = true;
                            lblTelefoneResponsavel.Visible = true;
                            txtTelefoneResp.Visible = true;
                        }
                        if (item.Text == "Próprio Aluno")
                        {
                            if (dtDataNasc.Text.Trim() != string.Empty)
                            {
                                var idade = Utils.CalcularIdade(dtDataNasc.Date);

                                if (idade < 18)
                                {
                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup",
                                                                            "alert('Para o aluno ser responsável é necessário ter mais que 18 anos.');",
                                                                            true);
                                    item.Selected = false;
                                }
                            }
                            else
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup",
                                                                        "alert('Favor preencher o campo Data de Nascimento.');",
                                                                        true);
                                item.Selected = false;
                            }
                        }
                    }
                }

                if (!verifica)
                {
                    txtNomeResponsavel.Text = string.Empty;
                    txtCPFResponsavel.Text = string.Empty;
                    txtTelefoneResp.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void cmbTurno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbSerie.Items.Clear();
                if (!cmbTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    cmbCurriculo.DataBind();
                    cmbCurriculo.Items.Insert(0, new ListItem("Selecione", string.Empty));

                    if (!string.IsNullOrEmpty(cmbTurno.SelectedValue) && (!tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue) && !cmbCurriculo.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                    {
                        CarregaSerie();

                        if (Request.QueryString.Keys.Count > 0)
                        {
                            if (Request.QueryString["ChaveInscricaoCompartilhadas"] != null)
                            {
                                cmbSerie.SelectedIndex = cmbSerie.Items.IndexOf(cmbSerie.Items[0]);
                                cmbSerie.Enabled = false;
                            }
                        }
                    }
                    ControlarTSearchs();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void cmbCurriculo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string serie = string.Empty;

                if (!string.IsNullOrEmpty(cmbCurriculo.SelectedValue))
                {
                    CarregaSerie();
                    if (Request.QueryString.Keys.Count > 0)
                    {
                        if (Request.QueryString["ChaveInscricaoCompartilhadas"] != null)
                        {

                            byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["ChaveInscricaoCompartilhadas"]);
                            string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

                            string[] inscricaoCompartilhadas = decodedText.Split('&');

                            foreach (string dados in inscricaoCompartilhadas)
                            {
                                if (dados.IndexOf("serie") >= 0)
                                {
                                    serie = dados.Substring(dados.LastIndexOf('=') + 1);
                                }
                            }

                            if (cmbSerie.Items.FindByValue(serie) != null)
                            {
                                cmbSerie.SelectedValue = serie;
                                cmbSerie.Enabled = false;
                            }

                        }
                    }
                }

                ControlarTSearchs();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlUFCartorio_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlMunicipioCartorio.Items.Clear();
                ddlCartorio.Items.Clear();

                if (!string.IsNullOrEmpty(ddlUFCartorio.SelectedValue))
                {
                    this.CarregaMunicipioCartorio();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlMunicipioCartorio_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlCartorio.Items.Clear();

                if (!string.IsNullOrEmpty(ddlMunicipioCartorio.SelectedValue))
                {
                    CarregaCartorio();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlTipoCertidao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtNumMatriculaCertidao.Text = string.Empty;
                txtDOC_CertNasc_Folha.Text = string.Empty;
                txtDOC_CertNasc_Livro.Text = string.Empty;
                txtDOC_CertNasc_Numero.Text = string.Empty;
                ddlCertidaoCivil.ClearSelection();
                ddlUFCartorio.ClearSelection();
                ddlMunicipioCartorio.Items.Clear();
                ddlCartorio.Items.Clear();
                dboDOC_CertNasc_DtEmissao.Text = string.Empty;
                ddDOC_CertNasc_Uf.ClearSelection();
                pnlTipoCertidaoCivil.Visible = false;
                chkDeclaroCertidaoCivil.Checked = false;
                txtMotivoCertidaoCivil.Text = string.Empty;
                lblCertCivil.Visible = true;
                ddlCertidaoCivil.Visible = true;

                if (!string.IsNullOrEmpty(ddlTipoCertidao.SelectedValue))
                {
                    if (ddlTipoCertidao.SelectedValue == "Nenhum")
                    {
                        pnlTipoCertidaoCivil.Visible = true;
                        chkDeclaroCertidaoCivil.Checked = true;
                        chkDeclaroCertidaoCivil.Visible = true;
                        txtMotivoCertidaoCivil.Visible = true;
                        pnlAntigo.Visible = false;
                        pnlNovo.Visible = false;
                        lblCertCivil.Visible = false;
                        ddlCertidaoCivil.Visible = false;
                    }
                    else
                    {
                        ddlCertidaoCivil.Enabled = true;
                        pnlAntigo.Visible = false;
                        pnlNovo.Visible = false;
                    }
                }
                else
                {
                    ddlCertidaoCivil.Enabled = false;
                    pnlAntigo.Visible = false;
                    pnlNovo.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }
        protected void ddlEtnia_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlPovoIndigena.Visible = false;
                lblPovo.Visible = false;
                ddlPovoIndigena.ClearSelection();

                if (ddlEtnia.SelectedValue == "Índigena")
                {
                    CarregaPovoIndigena();
                    ddlPovoIndigena.Visible = true;
                    lblPovo.Visible = true;
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaPovoIndigena()
        {

            RN.RecursosHumanos.PovoIndigena rnPovoIndigena = new RN.RecursosHumanos.PovoIndigena();
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlPovoIndigena.Items.Clear();
            ddlPovoIndigena.DataSource = rnPovoIndigena.ListaAtivoPor();
            ddlPovoIndigena.DataBind();
            ddlPovoIndigena.Items.Insert(0, item);

        }

        protected void ddlCertidaoCivil_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtNumMatriculaCertidao.Text = string.Empty;
                txtDOC_CertNasc_Folha.Text = string.Empty;
                txtDOC_CertNasc_Livro.Text = string.Empty;
                txtDOC_CertNasc_Numero.Text = string.Empty;
                ddlUFCartorio.ClearSelection();
                ddlMunicipioCartorio.Items.Clear();
                ddlCartorio.Items.Clear();
                dboDOC_CertNasc_DtEmissao.Text = string.Empty;
                ddDOC_CertNasc_Uf.ClearSelection();

                if (ddlCertidaoCivil.SelectedValue == "Modelo Novo")
                {
                    pnlNovo.Visible = true;
                    pnlAntigo.Visible = false;
                }
                else if (ddlCertidaoCivil.SelectedValue == "Modelo Antigo")
                {
                    pnlNovo.Visible = false;
                    pnlAntigo.Visible = true;
                }
                else
                {
                    pnlNovo.Visible = false;
                    pnlAntigo.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlAnoMatricula_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.NovaMatricula;

                ddlPeriodoMatricula.ClearSelection();
                CarregaUnidadeEnsinoMatricula();
                ddlNivelMatricula.ClearSelection();
                ddlModalidadeMatricula.ClearSelection();
                CarregarFiltroCursoMatricula();
                ControlarTSearchs();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlPeriodoMatricula_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.NovaMatricula;

                CarregaUnidadeEnsinoMatricula();
                ddlNivelMatricula.ClearSelection();
                ddlModalidadeMatricula.ClearSelection();
                CarregarFiltroCursoMatricula();
                ControlarTSearchs();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlModalidadeMatricula_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.NovaMatricula;

                tseCursoMatricula.ResetValue();
                CarregarFiltroCursoMatricula();
                ControlarTSearchs();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlNivelMatricula_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.NovaMatricula;

                ddlModalidadeMatricula.ClearSelection();
                tseCursoMatricula.ResetValue();
                CarregarFiltroCursoMatricula();
                ControlarTSearchs();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseCursoMatricula_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                    return;

                if (tseCursoMatricula.DBValue.IsNull || !tseCursoMatricula.IsValidDBValue)
                {
                    cmbSerieMatricula.Items.Clear();
                    cmbTurnoMatricula.Items.Clear();
                }
                else
                {
                    CarregaTurnoMatricula();
                    cmbCurriculoMatricula.DataBind();
                    cmbCurriculoMatricula.ClearSelection();
                    cmbSerieMatricula.Items.Clear();
                    if (!string.IsNullOrEmpty(cmbTurnoMatricula.SelectedValue) && !tseCursoMatricula.DBValue.IsNull && tseCursoMatricula.IsValidDBValue)
                    {
                        if (!string.IsNullOrEmpty(cmbCurriculoMatricula.SelectedValue))
                        {
                            CarregaSerieMatricula();
                        }
                    }
                    ControlarTSearchs();

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void cmbTurnoMatricula_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbSerieMatricula.Items.Clear();
                if (!cmbTurnoMatricula.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    cmbCurriculoMatricula.DataBind();
                    cmbCurriculoMatricula.Items.Insert(0, new ListItem("Selecione", string.Empty));

                    if (!string.IsNullOrEmpty(cmbTurnoMatricula.SelectedValue) && (!tseCursoMatricula.DBValue.IsNull && tseCursoMatricula.IsValidDBValue) && !cmbCurriculoMatricula.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                    {
                        CarregaSerieMatricula();
                    }
                    ControlarTSearchs();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void cmbCurriculoMatricula_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(cmbCurriculoMatricula.SelectedValue))
                {
                    CarregaSerieMatricula();

                }

                ControlarTSearchs();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaTurnoMatricula()
        {
            RN.Turno rnTurno = new Turno();
            RN.Curriculo rnCurriculo = new Techne.Lyceum.RN.Curriculo();

            ListItem item = new ListItem("Selecione", string.Empty);
            cmbTurnoMatricula.Items.Clear();

            if ((_tipoOperacao.Equals(TipoOperacao.NovaMatricula)) && (tseCursoMatricula.IsValidDBValue && !tseCursoMatricula.DBValue.IsNull))
            {
                cmbTurnoMatricula.DataSource = rnCurriculo.ObtemListaTurnoPor(tseCursoMatricula.DBValue.ToString());

            }
            else
            {
                cmbTurnoMatricula.DataSource = rnTurno.ListaTurnosPor();
            }
            cmbTurnoMatricula.DataBind();
            cmbTurnoMatricula.Items.Insert(0, item);
        }

        private void CarregaSerieMatricula()
        {
            RN.Serie rnSerie = new Serie();

            bool bloquearSeriesIniciaisAluno = Convert.ToBoolean(ConfigurationManager.AppSettings["BloquearSeriesIniciaisTransfAluno"] ?? "false");

            ListItem item = new ListItem("Selecione", string.Empty);
            cmbSerieMatricula.Items.Clear();

            if ((tseCursoMatricula.IsValidDBValue && !tseCursoMatricula.DBValue.IsNull) && !cmbTurnoMatricula.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !cmbCurriculoMatricula.SelectedValue.IsNullOrEmptyOrWhiteSpace())
            {
                cmbSerieMatricula.DataSource = rnSerie.ObtemSeriesAlunoNovoPor(tseCursoMatricula.DBValue.ToString(), cmbTurnoMatricula.SelectedValue.ToString(), cmbCurriculoMatricula.SelectedValue, ((_tipoOperacao.Equals(TipoOperacao.Novo) || _tipoOperacao.Equals(TipoOperacao.NovaMatricula)) ? bloquearSeriesIniciaisAluno : false));
            }
            cmbSerieMatricula.DataBind();
            cmbSerieMatricula.Items.Insert(0, item);
        }

        #endregion

        protected void tab_onchange(object source, DevExpress.Web.ASPxTabControl.TabControlCancelEventArgs e)
        {
            if (e.Tab.Name == "tabIrmaos")
            {
                pntabIrmaos.Visible = true;
                ImageClickEventArgs ev = null;
                txtMae.ReadOnly = false;
                txtPai.ReadOnly = false;
                hdnDataNascimento.Value = dtDataNasc.Value.ToString();
                btnBuscarIrmaos_Click(source, ev);
            }
        }

        protected void tab_TabClick(object source, DevExpress.Web.ASPxTabControl.TabControlCancelEventArgs e)
        {
            try
            {
                pntabDadosEscolares.Visible = false;
                pntabDadosPessoais.Visible = false;
                pntabTransporteEscolar.Visible = false;
                pntabDocumentos.Visible = false;
                pntabProgramas.Visible = false;
                pntabEspecializado.Visible = false;
                pntabIrmaos.Visible = false;
                pntabAEDH.Visible = false;

                if (e.Tab.Name == "tabDadosPessoais")
                {
                    pntabDadosPessoais.Visible = true;

                    if (_tipoOperacao == TipoOperacao.Consultar)
                        HabilitaLoginOperadora();

                    if (grdBuscaIrmaosCadastrados.VisibleRowCount == 0)
                        chkPossuiIrmao.Checked = false;
                }
                else if (e.Tab.Name == "tabDadosEscolares")
                {
                    pntabDadosEscolares.Visible = true;
                }
                else if (e.Tab.Name == "tabTransporteEscolar")
                {
                    pntabTransporteEscolar.Visible = true;
                }
                else if (e.Tab.Name == "tabDocumentos")
                {
                    pntabDocumentos.Visible = true;
                }
                else if (e.Tab.Name == "tabProgramas")
                {
                    pntabProgramas.Visible = true;
                    if (cmbNecessidadeEspecial.SelectedValue == "30" && chkPossuiIrmao.Checked == false)//"Não possui."
                        btnProximo4.Visible = false;
                }
                else if (e.Tab.Name == "tabEspecializado")
                {
                    pntabEspecializado.Visible = true;
                    CarregaDadosAvaliacaoCuidador();
                    CarregaDadosAvaliacaoInterprete();
                    CarregaDadosAvaliacaoLedor();
                    CarregaDadosAvaliacaoPAPEE();
                    CarregaDadosAvaliacaoSalaRecurso();
                    odsCuidador.Select();
                    odsCuidador.DataBind();
                    grdCuidador.DataBind();
                    odsLedor.Select();
                    odsLedor.DataBind();
                    grdLedor.DataBind();
                    odsInterprete.Select();
                    odsInterprete.DataBind();
                    grdInterprete.DataBind();

                    odsSalaRecurso.Select();
                    odsSalaRecurso.DataBind();
                    grdSalaRecurso.DataBind();

                    odsPAPEE.Select();
                    odsPAPEE.DataBind();
                    grdPAPEE.DataBind();


                    if (!pnlCuidador.Visible && !pnlLedor.Visible && !pnlInterprete.Visible && !pnlSalaRecurso.Visible && !pnlPAPEE.Visible)
                    {
                        lblMensagemAtendimentoEspecializado.Text = "A necessidade de recurso deste aluno ainda não foi avaliada pelo NAPES.";
                    }
                }
                else if (e.Tab.Name == "tabIrmaos")
                {
                    pntabIrmaos.Visible = true;
                    ImageClickEventArgs ev = null;
                    txtMae.ReadOnly = false;
                    txtPai.ReadOnly = false;
                    btnBuscarIrmaos_Click(source, ev);
                }
                else if (e.Tab.Name == "tabAEDH")
                {
                    pntabAEDH.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimpaDadosAvaliacaoCuidador()
        {
            lblNecessitaCuidador.Text = string.Empty;
            lblTipoAvaliacaoCuidador.Text = string.Empty;
            lblVigenciaCuidador.Text = string.Empty;
            txtJustificativaCuidador.Text = string.Empty;
        }

        private void LimpaDadosAvaliacaoSalaRecurso()
        {
            lblNecessitaSalaRecurso.Text = string.Empty;
            lblTipoAvaliacaoSalaRecurso.Text = string.Empty;
            lblVigenciaSalaRecurso.Text = string.Empty;
            lblUnidadeSalaRecurso.Text = string.Empty; ;
            txtJustificativaSalaRecurso.Text = string.Empty;
        }

        private void LimpaDadosAvaliacaoPAPEE()
        {
            lblNecessitaPAPEE.Text = string.Empty;
            lblTipoAvaliacaoPAPEE.Text = string.Empty;
            lblVigenciaPAPEE.Text = string.Empty;
            txtJustificativaPAPEE.Text = string.Empty;
        }

        private void CarregaDadosAvaliacaoCuidador()
        {
            RN.NecessidadeEspecial.AvaliacaoNapes rnAvaliacaoNapes = new Techne.Lyceum.RN.NecessidadeEspecial.AvaliacaoNapes();
            RN.NecessidadeEspecial.Entidades.AvaliacaoNapes avaliacaoNapes = new Techne.Lyceum.RN.NecessidadeEspecial.Entidades.AvaliacaoNapes();
            LimpaDadosAvaliacaoCuidador();
            pnlCuidador.Visible = false;

            avaliacaoNapes = rnAvaliacaoNapes.ObtemPor(tseAluno.DBValue.ToString(), (int)Techne.Lyceum.RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.Cuidador);

            if (avaliacaoNapes.AvaliacaoNapesId > 0)
            {
                pnlCuidador.Visible = true;
                lblNecessitaCuidador.Text = Convert.ToBoolean(avaliacaoNapes.NecessitaRecurso) ? "Sim" : "Não";
                txtJustificativaCuidador.Text = avaliacaoNapes.Justificativa;

                if (!Convert.ToBoolean(avaliacaoNapes.NecessitaRecurso))
                {
                    lblAvalTipoCuidador.Visible = false;
                    lblAvalVigenciaCuidador.Visible = false;
                    lblTipoAvaliacaoCuidador.Visible = false;
                    lblVigenciaCuidador.Visible = false;
                }
                else
                {
                    lblAvalTipoCuidador.Visible = true;
                    lblTipoAvaliacaoCuidador.Visible = true;
                    lblTipoAvaliacaoCuidador.Text = Convert.ToBoolean(avaliacaoNapes.Transitorio) ? "Transitório" : "Permanente";
                    if (Convert.ToBoolean(avaliacaoNapes.Transitorio))
                    {
                        lblAvalVigenciaCuidador.Visible = true;
                        lblVigenciaCuidador.Visible = true;
                        lblVigenciaCuidador.Text = avaliacaoNapes.DataInicio.Value.ToString("dd/MM/yyyy") + " à " + avaliacaoNapes.DataFim.Value.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        lblAvalVigenciaCuidador.Visible = false;
                        lblVigenciaCuidador.Visible = false;
                    }
                }
            }
        }

        private void CarregaDadosAvaliacaoPAPEE()
        {
            RN.NecessidadeEspecial.AvaliacaoNapes rnAvaliacaoNapes = new Techne.Lyceum.RN.NecessidadeEspecial.AvaliacaoNapes();
            RN.NecessidadeEspecial.Entidades.AvaliacaoNapes avaliacaoNapes = new Techne.Lyceum.RN.NecessidadeEspecial.Entidades.AvaliacaoNapes();
            LimpaDadosAvaliacaoPAPEE();
            pnlPAPEE.Visible = false;

            avaliacaoNapes = rnAvaliacaoNapes.ObtemPor(tseAluno.DBValue.ToString(), (int)Techne.Lyceum.RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.PAPEE);

            if (avaliacaoNapes.AvaliacaoNapesId > 0)
            {
                pnlPAPEE.Visible = true;
                lblNecessitaPAPEE.Text = Convert.ToBoolean(avaliacaoNapes.NecessitaRecurso) ? "Sim" : "Não";
                txtJustificativaPAPEE.Text = avaliacaoNapes.Justificativa;


                if (!Convert.ToBoolean(avaliacaoNapes.NecessitaRecurso))
                {
                    lblAvalTipoPAPEE.Visible = false;
                    lblAvalVigenciaPAPEE.Visible = false;
                    lblTipoAvaliacaoPAPEE.Visible = false;
                    lblVigenciaPAPEE.Visible = false;
                }
                else
                {
                    lblAvalTipoPAPEE.Visible = true;
                    lblTipoAvaliacaoPAPEE.Visible = true;
                    lblTipoAvaliacaoPAPEE.Text = Convert.ToBoolean(avaliacaoNapes.Transitorio) ? "Transitório" : "Permanente";
                    if (Convert.ToBoolean(avaliacaoNapes.Transitorio))
                    {
                        lblAvalVigenciaPAPEE.Visible = true;
                        lblVigenciaPAPEE.Visible = true;
                        lblVigenciaPAPEE.Text = avaliacaoNapes.DataInicio.Value.ToString("dd/MM/yyyy") + " à " + avaliacaoNapes.DataFim.Value.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        lblAvalVigenciaPAPEE.Visible = false;
                        lblVigenciaPAPEE.Visible = false;
                    }
                }
            }
        }

        private void CarregaDadosAvaliacaoSalaRecurso()
        {
            RN.NecessidadeEspecial.AvaliacaoNapes rnAvaliacaoNapes = new Techne.Lyceum.RN.NecessidadeEspecial.AvaliacaoNapes();
            RN.NecessidadeEspecial.Entidades.AvaliacaoNapes avaliacaoNapes = new Techne.Lyceum.RN.NecessidadeEspecial.Entidades.AvaliacaoNapes();
            LimpaDadosAvaliacaoSalaRecurso();
            pnlSalaRecurso.Visible = false;

            avaliacaoNapes = rnAvaliacaoNapes.ObtemPor(tseAluno.DBValue.ToString(), (int)Techne.Lyceum.RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.SalaRecurso);

            if (avaliacaoNapes.AvaliacaoNapesId > 0)
            {
                pnlSalaRecurso.Visible = true;
                lblNecessitaSalaRecurso.Text = Convert.ToBoolean(avaliacaoNapes.NecessitaRecurso) ? "Sim" : "Não";
                txtJustificativaSalaRecurso.Text = avaliacaoNapes.Justificativa;

                if (!Convert.ToBoolean(avaliacaoNapes.NecessitaRecurso))
                {
                    lblAvalTipoSalaRecurso.Visible = false;
                    lblAvalVigenciaSalaRecurso.Visible = false;
                    lblTipoAvaliacaoSalaRecurso.Visible = false;
                    lblVigenciaSalaRecurso.Visible = false;
                    lblAvalUnidadeSalaRecurso.Visible = false;
                }
                else
                {
                    lblAvalTipoSalaRecurso.Visible = true;
                    lblTipoAvaliacaoSalaRecurso.Visible = true;

                    lblTipoAvaliacaoSalaRecurso.Text = Convert.ToBoolean(avaliacaoNapes.Transitorio) ? "Transitório" : "Permanente";
                    if (Convert.ToBoolean(avaliacaoNapes.Transitorio))
                    {
                        lblAvalVigenciaSalaRecurso.Visible = true;
                        lblVigenciaSalaRecurso.Visible = true;
                        lblVigenciaSalaRecurso.Text = avaliacaoNapes.DataInicio.Value.ToString("dd/MM/yyyy") + " à " + avaliacaoNapes.DataFim.Value.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        lblAvalVigenciaSalaRecurso.Visible = false;
                        lblVigenciaSalaRecurso.Visible = false;
                    }
                }
            }
        }

        private void LimpaDadosAvaliacaoLedor()
        {
            lblNecessitaLedor.Text = string.Empty;
            lblTipoAvaliacaoLedor.Text = string.Empty;
            lblVigenciaLedor.Text = string.Empty;
            txtJustificativaLedor.Text = string.Empty;
        }

        private void CarregaDadosAvaliacaoLedor()
        {
            RN.NecessidadeEspecial.AvaliacaoNapes rnAvaliacaoNapes = new Techne.Lyceum.RN.NecessidadeEspecial.AvaliacaoNapes();
            RN.NecessidadeEspecial.Entidades.AvaliacaoNapes avaliacaoNapes = new Techne.Lyceum.RN.NecessidadeEspecial.Entidades.AvaliacaoNapes();
            LimpaDadosAvaliacaoLedor();
            pnlLedor.Visible = false;

            avaliacaoNapes = rnAvaliacaoNapes.ObtemPor(tseAluno.DBValue.ToString(), (int)Techne.Lyceum.RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.Ledor);

            if (avaliacaoNapes.AvaliacaoNapesId > 0)
            {
                pnlLedor.Visible = true;
                lblNecessitaLedor.Text = Convert.ToBoolean(avaliacaoNapes.NecessitaRecurso) ? "Sim" : "Não";
                txtJustificativaLedor.Text = avaliacaoNapes.Justificativa;

                if (!Convert.ToBoolean(avaliacaoNapes.NecessitaRecurso))
                {
                    lblAvalTipoLedor.Visible = false;
                    lblAvalVigenciaLedor.Visible = false;
                    lblTipoAvaliacaoLedor.Visible = false;
                    lblVigenciaLedor.Visible = false;
                }
                else
                {
                    lblAvalTipoLedor.Visible = true;
                    lblTipoAvaliacaoLedor.Visible = true;
                    lblTipoAvaliacaoLedor.Text = Convert.ToBoolean(avaliacaoNapes.Transitorio) ? "Transitório" : "Permanente";
                    if (Convert.ToBoolean(avaliacaoNapes.Transitorio))
                    {
                        lblAvalVigenciaLedor.Visible = true;
                        lblVigenciaLedor.Visible = true;
                        lblVigenciaLedor.Text = avaliacaoNapes.DataInicio.Value.ToString("dd/MM/yyyy") + " à " + avaliacaoNapes.DataFim.Value.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        lblAvalVigenciaLedor.Visible = false;
                        lblVigenciaLedor.Visible = false;
                    }
                }
            }
        }

        private void LimpaDadosAvaliacaoInterprete()
        {
            lblNecessitaInterprete.Text = string.Empty;
            lblTipoAvaliacaoInterprete.Text = string.Empty;
            lblVigenciaInterprete.Text = string.Empty;
            txtJustificativaInterprete.Text = string.Empty;
        }

        private void CarregaDadosAvaliacaoInterprete()
        {
            RN.NecessidadeEspecial.AvaliacaoNapes rnAvaliacaoNapes = new Techne.Lyceum.RN.NecessidadeEspecial.AvaliacaoNapes();
            RN.NecessidadeEspecial.Entidades.AvaliacaoNapes avaliacaoNapes = new Techne.Lyceum.RN.NecessidadeEspecial.Entidades.AvaliacaoNapes();
            LimpaDadosAvaliacaoInterprete();
            pnlInterprete.Visible = false;

            avaliacaoNapes = rnAvaliacaoNapes.ObtemPor(tseAluno.DBValue.ToString(), (int)Techne.Lyceum.RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.Interprete);

            if (avaliacaoNapes.AvaliacaoNapesId > 0)
            {
                pnlInterprete.Visible = true;
                lblNecessitaInterprete.Text = Convert.ToBoolean(avaliacaoNapes.NecessitaRecurso) ? "Sim" : "Não";
                txtJustificativaInterprete.Text = avaliacaoNapes.Justificativa;

                if (!Convert.ToBoolean(avaliacaoNapes.NecessitaRecurso))
                {
                    lblAvalTipoInterprete.Visible = false;
                    lblAvalVigenciaInterprete.Visible = false;
                    lblTipoAvaliacaoInterprete.Visible = false;
                    lblVigenciaInterprete.Visible = false;
                }
                else
                {
                    lblAvalTipoInterprete.Visible = true;
                    lblTipoAvaliacaoInterprete.Visible = true;
                    lblTipoAvaliacaoInterprete.Text = Convert.ToBoolean(avaliacaoNapes.Transitorio) ? "Transitório" : "Permanente";
                    if (Convert.ToBoolean(avaliacaoNapes.Transitorio))
                    {
                        lblAvalVigenciaInterprete.Visible = true;
                        lblVigenciaInterprete.Visible = true;
                        lblVigenciaInterprete.Text = avaliacaoNapes.DataInicio.Value.ToString("dd/MM/yyyy") + " à " + avaliacaoNapes.DataFim.Value.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        lblAvalVigenciaInterprete.Visible = false;
                        lblVigenciaInterprete.Visible = false;
                    }
                }
            }
        }

        private void ControlarEnderecoPais()
        {
            // Controle do botão de CEP (inalterado)
            if (_tipoOperacao == TipoOperacao.Novo
                || _tipoOperacao == TipoOperacao.NovaMatricula
                || _tipoOperacao == TipoOperacao.Alterar
                || _tipoOperacao == TipoOperacao.CadastrarAlunoCompartilhadas)
            {
                tsCEP.ShowButton = true;
            }
            else
            {
                tsCEP.ShowButton = false;
            }

            txtCep.MaxLength = 8;
            txtEstado.Attributes.Add("readonly", "readonly");

            bool ehEstrangeiro = !string.IsNullOrEmpty(txtPaisNasc.Text)
                                 && txtPaisNasc.Text.ToUpper() != "BRASIL";

            if (ehEstrangeiro)
            {
                // Oculta o TSearch de municípios brasileiros
                tseNaturalidade.Visible = false;

                // Exibe o TSearch de naturalidade estrangeira
                tseNaturalidadeEstrangeira.Visible = true;

                // Libera edição da UF (campo livre, sem vínculo com IBGE)
                if (_tipoOperacao == TipoOperacao.Novo
                    || _tipoOperacao == TipoOperacao.NovaMatricula
                    || _tipoOperacao == TipoOperacao.Alterar
                    || _tipoOperacao == TipoOperacao.CadastrarAlunoCompartilhadas)
                {
                    txtUFNascimento.Attributes.Remove("readonly");
                }
            }
            else
            {
                // Exibe o TSearch de municípios brasileiros
                tseNaturalidade.Visible = true;

                // Oculta o TSearch estrangeiro
                tseNaturalidadeEstrangeira.Visible = false;
            }
        }

        private void LimparEnderecoNaturalidade()
        {
            txtUFNascimento.Text = string.Empty;
            tseNaturalidade.ResetValue();
            tseNaturalidadeEstrangeira.ResetValue();
        }

        private void LimparEndereco()
        {
            txtMunicipio.Text = string.Empty;
            hdnCodMunicipio.Value = string.Empty;
            txtEstado.Value = string.Empty;
            txtEndereco.Text = string.Empty;
            txtCep.Text = string.Empty;
            txtEndCompl.Text = string.Empty;
            txtEndNum.Text = string.Empty;
            txtBairro.Text = string.Empty;
            chkAreaAssentamento.Checked = false;
            chkTerraIndigena.Checked = false;
            chkQuilombos.Checked = false;
            chkAreaTradicional.Checked = false;
            chkNaoSeAplica.Checked = false;
            txtLatitude.Text = string.Empty;
            txtLongitude.Text = string.Empty;
        }

        private void ControlarTipoOperacao()
        {
            switch (this._tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);

                        tab.Visible = false;
                        pntabDadosEscolares.Visible = false;
                        pntabDadosPessoais.Visible = false;
                        pntabTransporteEscolar.Visible = false;
                        pntabDocumentos.Visible = false;
                        pntabProgramas.Visible = false;
                        pntabEspecializado.Visible = false;
                        pntabIrmaos.Visible = false;
                        pntabAEDH.Visible = false;
                        btnSalvarDoc.Visible = false;
                        btnSalvarTransporte.Visible = false;
                        pnlBuscaAlunoNovo.Visible = false;
                        pnlNovaMatricula.Visible = false;
                        pnAEDH.Visible = false;

                        CarregaUnidadeEnsino();

                        tseAluno.ResetValue();
                        txtAluno.Visible = true;
                        lblMatricula.Visible = false;
                        lbltxtPessoa.Visible = false;
                        txtPessoa.Visible = true;
                        pnCurso.GroupingText = "Escolaridade";
                        txtPessoa.Visible = false;
                        lblPessoa.Visible = false;
                        if (txtSituacao.Text == "Ativo")
                        {
                            txtCausaEncerramento.Visible = false;
                            txtMotivo.Visible = false;
                            lblCausaEncerramento.Visible = false;
                            lblMotivo.Visible = false;
                        }

                        grdRenovacaoMatricula.DataSource = null;
                        grdRenovacaoMatricula.DataBind();

                        grdConfirmacao.DataSource = null;
                        grdConfirmacao.DataBind();

                        if (!string.IsNullOrEmpty(ddlGratuidade.SelectedValue) && (ddlGratuidade.SelectedValue == "N"))
                        {
                            ddlPoderPublicoTransp.ClearSelection();
                            ddlPoderPublicoTransp.Enabled = false;
                            chkModais.ClearSelection();
                            chkModais.Enabled = false;
                        }

                        if (grdBuscaIrmaosCadastrados.VisibleRowCount > 0)
                            tab.Tabs[6].Enabled = true;
                        else
                            tab.Tabs[6].Enabled = false;

                        ddlPovoIndigena.ClearSelection();
                        ddlPovoIndigena.Visible = false;
                        lblPovo.Visible = false;
                        pnlTipoTranstorno.Visible = false;
                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar, btnExcluir };
                        ControlarVisibilidadeControle(controles);
                        tab.Visible = true;
                        DesabilitaCampos();
                        txtAluno.Visible = true;
                        lblMatricula.Visible = false;
                        lbltxtPessoa.Visible = false;
                        txtPessoa.Visible = true;
                        btnProximo2.Visible = true;
                        btnSalvarDoc.Visible = false;
                        btnSalvarTransporte.Visible = false;
                        pnlBuscaAlunoNovo.Visible = false;
                        pnlNovaMatricula.Visible = false;

                        tab.Tabs[2].Enabled = true;
                        tab.Tabs[3].Enabled = true;
                        pnIrmaos.Enabled = false;

                        txtCausaEncerramento.Visible = true;
                        txtMotivo.Visible = true;
                        lblCausaEncerramento.Visible = true;
                        lblMotivo.Visible = true;
                        txtPessoa.Visible = false;
                        lblPessoa.Visible = false;
                        if (txtSituacao.Text == "Ativo")
                        {
                            txtCausaEncerramento.Visible = false;
                            txtMotivo.Visible = false;
                            lblCausaEncerramento.Visible = false;
                            lblMotivo.Visible = false;
                        }

                        string aluno = string.IsNullOrEmpty(tseAluno.DBValue.ToString()) ? txtAluno.Text : tseAluno.DBValue.ToString();

                        CarregaDadosDeclaracao(aluno);

                        if (!string.IsNullOrEmpty(ddlGratuidade.SelectedValue) && (ddlGratuidade.SelectedValue == "N"))
                        {
                            ddlPoderPublicoTransp.ClearSelection();
                            ddlPoderPublicoTransp.Enabled = false;
                            chkModais.ClearSelection();
                            chkModais.Enabled = false;
                        }

                        if (ConfirmacaoMatricula.PossuiConfirmacaoEmAberto(aluno))
                        {
                            btnConfRenovMatricula.Visible = true;
                        }
                        else
                        {
                            btnConfRenovMatricula.Visible = false;
                        }
                        this._mudouCurso = false;
                        ViewState["confirmaalteracao"] = null;

                        hdnDataNascimento.Value = dtDataNasc.Value.ToString();
                        tab.Tabs[6].Enabled = grdBuscaIrmaosCadastrados.VisibleRowCount > 0;


                        if (ddlOutroEnsino.SelectedValue != "3" || grdAEDH.VisibleRowCount > 0)
                        {
                            tab.Tabs[7].Enabled = true;
                        }

                        lblObsLoginCartao.Visible = false;

                        var dadosFieldPessoa = FlPessoa.Carregar(Convert.ToDecimal(txtPessoa.Text));

                        if (dadosFieldPessoa != null)
                        {
                            CarregaDadosFieldPessoa(dadosFieldPessoa);

                            if (dadosFieldPessoa.FlField03 != "3")
                            {
                                tab.Tabs[7].Enabled = true;
                            }
                        }
                        else
                        {
                            ddlLocalZona.SelectedValue = "";
                            ddlGratuidade.SelectedValue = "";
                            ddlTipoCertidao.SelectedValue = "";
                            ddlOutroEnsino.SelectedValue = "";
                        }
                        break;
                    }
                case TipoOperacao.Novo:
                    {
                        btnProximo2.Visible = false;
                        tab.Visible = true;
                        tab.ActiveTabIndex = 0;

                        pntabDadosPessoais.Visible = true;
                        pntabDadosEscolares.Visible = false;
                        pntabTransporteEscolar.Visible = false;
                        pntabDocumentos.Visible = false;
                        pntabProgramas.Visible = false;
                        pntabEspecializado.Visible = false;
                        pnlLoginCartao.Visible = false;
                        pntabIrmaos.Visible = false;
                        pntabAEDH.Visible = false;
                        btnSalvarDoc.Visible = true;
                        btnSalvarTransporte.Visible = true;
                        btnEditar.Visible = false;
                        pntabEspecializado.Visible = false;

                        tab.Tabs[0].Enabled = true;
                        tab.Tabs[1].Enabled = true;
                        tab.Tabs[2].Enabled = false;
                        tab.Tabs[3].Enabled = false;
                        tab.Tabs[4].Enabled = false;
                        tab.Tabs[5].Enabled = false;
                        tab.Tabs[6].Enabled = false;
                        tab.Tabs[7].Enabled = false;

                        pnImprimirMatricula.Visible = false;
                        LimparTela();
                        LimpaDadosAvaliacaoCuidador();
                        LimpaDadosAvaliacaoInterprete();
                        LimpaDadosAvaliacaoLedor();
                        LimpaDadosAvaliacaoSalaRecurso();
                        LimpaDadosAvaliacaoPAPEE();
                        LimpaCamposAEDH();
                        HabilitaCampos();
                        tseAluno.ResetValue();
                        CarregaUnidadeEnsino();

                        tseUnidadeEns.ResetValue();
                        txtAluno.Visible = false;
                        lblMatricula.Visible = true;
                        txtPessoa.Visible = false;

                        ImageButton[] controles2 = new ImageButton[] { btnCancel, btnSalvar, btnFotografar };
                        ControlarVisibilidadeControle(controles2);
                        this.txtSituacao.Text = "Ativo";
                        txtSituacao.ReadOnly = true;

                        txtCausaEncerramento.Visible = false;
                        txtMotivo.Visible = false;
                        lblCausaEncerramento.Visible = false;
                        lblMotivo.Visible = false;

                        txtPessoa.Text = Convert.ToString(0);
                        cmbAnoIngresso.Enabled = true;
                        cmbSemIngresso.Enabled = true;
                        tseUnidadeEns.Mode = ControlMode.Edit;
                        ddlNivel.Enabled = true;
                        ddlModalidade.Enabled = true;
                        tseCurso.Mode = ControlMode.Edit;
                        cmbTurno.Enabled = true;
                        cmbCurriculo.Enabled = true;
                        cmbSerie.Enabled = true;
                        ddlRedeEnsinoOrigem.Enabled = true;

                        txtPaisNasc.Text = string.Empty;
                        PadronizarDropDownList(ddlNacionalidade, "");
                        ddlPoderPublicoTransp.Enabled = false;
                        txtUFNascimento.Enabled = true;
                        chkModais.Enabled = false;
                        ExibeOcultaInfoEmailRiocard(false);
                        chkNaoSeAplica.Checked = true;

                        ddlPovoIndigena.ClearSelection();
                        ddlPovoIndigena.Visible = false;
                        lblPovo.Visible = false;
                        pnlTipoTranstorno.Visible = false;

                        break;
                    }
                case TipoOperacao.Excluir:
                    {
                        ValidacaoDados validacao = new ValidacaoDados();
                        RN.Aluno rnAluno = new Aluno();

                        if (!txtAluno.Text.IsNullOrEmptyOrWhiteSpace() && !txtPessoa.Text.IsNullOrEmptyOrWhiteSpace() && (!tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue))
                        {
                            validacao = rnAluno.ValidaRemocao(txtAluno.Text.Trim(), Convert.ToDecimal(txtPessoa.Text), Convert.ToString(tseCurso.DBValue));

                            if (validacao.Valido)
                            {
                                rnAluno.Remove(txtAluno.Text.Trim(), Convert.ToDecimal(txtPessoa.Text));
                                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup",
                                                                               "alert('Aluno excluído com sucesso.');", true);

                                _tipoOperacao = TipoOperacao.Inicial;
                                ControlarTipoOperacao();

                            }
                            else
                            {
                                lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                            }
                        }
                        break;
                    }
                case TipoOperacao.Alterar:
                    {
                        if ((!tseAluno.DBValue.IsNull) && (tseAluno.IsValidDBValue))
                        {
                            RN.Aluno rnAluno = new Aluno();
                            string censoAluno = Aluno.ConsultarCenso(tseAluno.Text);
                            RN.EventoGeral rnEventoGeral = new EventoGeral();
                            HabilitaCampos();
                            chkNaoDeclarMae.Enabled = false;
                            btnEditar.Visible = false;
                            btnSalvarDoc.Visible = true;
                            btnSalvarTransporte.Visible = true;

                            txtPessoa.Visible = false;
                            //verifica se o curso está em peródo de matrícula e bloqueia campos:
                            if (rnEventoGeral.ExisteBloqueioCadastroAlunoAtivoPor(Convert.ToString(tseCurso.DBValue)))
                            {
                                ViewState["AgendaCurso"] = "true";
                                dtDataNasc.Enabled = false;
                                txtNomeCompl.ReadOnly = true;
                                cmbNecessidadeEspecial.Enabled = true;
                                tseCurso.Mode = ControlMode.View;
                                cmbTurno.Enabled = false;
                                cmbCurriculo.Enabled = false;
                                cmbSerie.Enabled = false;
                                ddlModalidade.Enabled = false;
                                ddlNivel.Enabled = false;
                                tseUnidadeEns.Mode = ControlMode.View;
                                if (!tseUnidadeEns.IsValidDBValue)
                                {
                                    //Se não estiver carrega a lista com a escola do aluno
                                    CarregaUnidadeEnsinoPor(censoAluno);
                                }
                            }
                            else
                            {
                                ViewState["AgendaCurso"] = null;
                            }

                            if (RN.Matricula.ExisteMatriculaAtiva(Convert.ToString(tseAluno.Value)))
                            {
                                ViewState["ExisteMatricula"] = "true";

                                pnCurso.GroupingText = "Escolaridade (Aluno Matriculado)";
                                tseUnidadeEns.Mode = ControlMode.View;
                                ddlNivel.Enabled = false;
                                ddlModalidade.Enabled = false;
                                tseCurso.Mode = ControlMode.View;
                                cmbTurno.Enabled = false;
                                cmbCurriculo.Enabled = false;
                                cmbAnoIngresso.Enabled = false;
                                cmbSemIngresso.Enabled = false;
                                cmbSerie.Enabled = false;
                            }
                            else
                            {
                                ViewState["ExisteMatricula"] = null;
                            }

                            ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar, btnFotografar };
                            ControlarVisibilidadeControle(controles);
                            lblMensagem.Text = String.Empty;
                            lblErroConfirmacao.Text = string.Empty;
                            txtAluno.ReadOnly = true;
                            lbltxtPessoa.Visible = false;
                            txtPessoa.Visible = true;
                            txtSituacao.ReadOnly = true;
                            txtCausaEncerramento.Visible = true;
                            lblCausaEncerramento.Visible = true;
                            txtMotivo.Visible = true;
                            lblMotivo.Visible = true;

                            txtPessoa.Visible = false;
                            lblPessoa.Visible = false;

                            if (txtSituacao.Text == "Ativo")
                            {
                                txtCausaEncerramento.Visible = false;
                                txtMotivo.Visible = false;
                                lblCausaEncerramento.Visible = false;
                                lblMotivo.Visible = false;
                            }

                            if (!string.IsNullOrEmpty(ddlGratuidade.SelectedValue) && (ddlGratuidade.SelectedValue == "N"))
                            {
                                ddlPoderPublicoTransp.ClearSelection();
                                ddlPoderPublicoTransp.Enabled = false;
                                chkModais.ClearSelection();
                                chkModais.Enabled = false;
                            }

                            if (ConfirmacaoMatricula.PossuiConfirmacaoEmAberto(tseAluno.Text))
                            {
                                btnConfRenovMatricula.Visible = true;
                            }
                            else
                            {
                                btnConfRenovMatricula.Visible = false;
                            }

                            if (grdBuscaIrmaosCadastrados.VisibleRowCount > 0)
                            {
                                pnIrmaos.Enabled = true;
                            }
                            else
                            {
                                pnIrmaos.Enabled = false;
                                tab.Tabs[6].Enabled = false;
                            }

                            grdAEDH.Columns[0].Visible = true;

                            if (grdAEDH.VisibleRowCount > 0 || ddlOutroEnsino.SelectedValue != "3")
                            {
                                pntabAEDH.Enabled = true;
                                tab.Tabs[7].Enabled = true;
                            }
                            else
                            {
                                pntabAEDH.Enabled = false;
                                tab.Tabs[7].Enabled = false;
                            }

                            ExibeOcultaInfoEmailRiocard(false);
                            HabilitaLoginOperadora();

                            if (rnAluno.ParticipouMatriculaFacilPor(tseAluno.Text) && !ddlRedeEnsinoOrigem.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                            {
                                ddlRedeEnsinoOrigem.Enabled = false;
                            }
                            else
                            {
                                ddlRedeEnsinoOrigem.Enabled = true;
                            }
                            btnEncontraNoMapa.Visible = true;

                            //O CAMPO SÓ PODERA SER ALTERADDO CASO NAO TENHA INFORMAÇÃO(OU SEJA, ESTEJA VAZIO)
                            if (!txtEmailGoogle.Text.IsNullOrEmptyOrWhiteSpace())
                            {
                                txtEmailGoogle.Enabled = false;
                            }
                            break;
                        }
                        else
                        {
                            this._tipoOperacao = TipoOperacao.Inicial;
                            lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";

                            break;
                        }
                    }
                case TipoOperacao.Consultar:
                    {
                        RN.RecursosHumanos.GoogleEducation rnGoogleEducation = new Techne.Lyceum.RN.RecursosHumanos.GoogleEducation();
                        RN.RecursosHumanos.Entidades.GoogleEducation googleEducation = new Techne.Lyceum.RN.RecursosHumanos.Entidades.GoogleEducation();
                        btnEncontraNoMapa.Visible = false;
                        btnProximo2.Visible = true;
                        tseAluno.Enabled = true;
                        tab.ActiveTabIndex = 0;
                        pnCurso.GroupingText = "Escolaridade";
                        txtAluno.Visible = true;
                        lbltxtPessoa.Visible = false;
                        txtPessoa.Visible = true;
                        lblMatricula.Visible = false;
                        lblMensagem.Text = string.Empty;
                        lblErroConfirmacao.Text = string.Empty;
                        pnlBuscaAlunoNovo.Visible = false;
                        pnlNovaMatricula.Visible = false;

                        LimparTela();
                        LimparEndereco();
                        LimparEnderecoNaturalidade();
                        LimpaDadosAvaliacaoCuidador();
                        LimpaDadosAvaliacaoInterprete();
                        LimpaDadosAvaliacaoLedor();
                        LimpaDadosAvaliacaoSalaRecurso();
                        LimpaDadosAvaliacaoPAPEE();
                        LimpaCamposAEDH();

                        grdAEDH.Columns[0].Visible = false;

                        var dadosAluno = Aluno.Carregar(tseAluno.DBValue.ToString());

                        if (!IsPostBack)
                        {
                            if (Request.QueryString.Keys.Count > 0)
                            {
                                string aluno = string.Empty;
                                if (!string.IsNullOrEmpty(Request.QueryString["Chave"]))
                                {
                                    byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                                    string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                                    aluno = decodedText.Substring(decodedText.LastIndexOf('=') + 1);
                                }
                                else if (!string.IsNullOrEmpty(Request.QueryString["Aluno"]))
                                {
                                    aluno = Request.QueryString["Aluno"];
                                }
                                else if (!String.IsNullOrEmpty(Request.QueryString["ChaveConfirmacao"]))
                                {
                                    byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["ChaveConfirmacao"]);
                                    string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                                    aluno = decodedText.Substring(decodedText.LastIndexOf('=') + 1);
                                }

                                dadosAluno.Aluno = aluno;
                            }
                        }

                        if (dadosAluno.Aluno == null)
                        {
                            lblMensagem.Text = "Aluno não encontrado.";

                            //controle de abas
                            tab.Visible = false;
                            pntabDadosEscolares.Visible = false;
                            pntabDadosPessoais.Visible = false;
                            pntabTransporteEscolar.Visible = false;
                            pntabDocumentos.Visible = false;
                            pntabProgramas.Visible = false;
                            pntabEspecializado.Visible = false;
                            pntabIrmaos.Visible = false;
                            pntabAEDH.Visible = false;
                            pntabEspecializado.Visible = false;


                            ImageButton[] controles = new ImageButton[] { btnNovo };
                            ControlarVisibilidadeControle(controles);
                        }
                        else
                        {
                            txtPessoa.Text = Convert.ToString(dadosAluno.Pessoa);
                            ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar, btnExcluir };
                            ControlarVisibilidadeControle(controles);

                            //controle de abas
                            tab.Visible = true;

                            pntabDadosPessoais.Visible = true;
                            pntabDadosEscolares.Visible = false;
                            pntabTransporteEscolar.Visible = false;
                            pntabDocumentos.Visible = false;
                            pntabProgramas.Visible = false;
                            pntabEspecializado.Visible = false;
                            pntabIrmaos.Visible = false;
                            pntabAEDH.Visible = false;
                            pnAEDH.Visible = false;

                            tab.Tabs[0].Enabled = true;
                            tab.Tabs[1].Enabled = true;
                            tab.Tabs[2].Enabled = true;
                            tab.Tabs[3].Enabled = true;
                            tab.Tabs[4].Enabled = true;
                            tab.Tabs[5].Enabled = true;
                            tab.Tabs[6].Enabled = true;
                            tab.Tabs[7].Enabled = true;

                            CarregaDadosAluno(dadosAluno);

                            var dadosPessoa = Pessoa.Carregar(Convert.ToInt32(dadosAluno.Pessoa));

                            if (dadosPessoa != null)
                            {
                                CarregaDadosPessoa(dadosPessoa);
                                var dadosFotoPessoa = FotoPessoa.Carregar(Convert.ToInt32(dadosAluno.Pessoa));
                                CarregaDadosFotoPessoa(dadosFotoPessoa);
                            }
                            else
                            {
                                lblMensagem.Text = "Pessoa não cadastrada para este aluno.";
                                return;
                            }

                            if (cmbNecessidadeEspecial.SelectedValue == "30")//"Não possui."
                            {
                                chkDeclaroNecessidadeEspecial.Checked = false;
                                chkDeclaroNecessidadeEspecial.Visible = false;
                                chkDeclaroNecessidadeEspecial.Text = "Declaro estar de posse do Laudo Médico ou Parecer Pedagógico";
                                pntabEspecializado.Enabled = false;
                                tab.Tabs[5].Enabled = false;
                            }
                            else
                            {
                                chkDeclaroNecessidadeEspecial.Visible = true;
                                pntabEspecializado.Enabled = true;
                                tab.Tabs[5].Enabled = true;

                                if (!string.IsNullOrEmpty(cmbNecessidadeEspecial.SelectedValue))
                                {
                                    DataTable dadosRecursosAplicacaoProva = RN.PessoaRecursoAplicacaoProva.Listar(Convert.ToInt32(dadosAluno.Pessoa));
                                    CarregaDadosRecursoAplicacaoProva(dadosRecursosAplicacaoProva);
                                    CarregaDadosAlunoTipoRecursoNecessidadeEspecial();
                                }
                            }

                            // ── Carga da naturalidade ───────────────────────────

                            tseNaturalidadeEstrangeira.Enabled = true;

                            if (!dadosPessoa.Municipio_nasc.IsNullOrEmptyOrWhiteSpace())
                            {
                                // Tenta primeiro na tse do Brasil
                                tseNaturalidade.DBValue = dadosPessoa.Municipio_nasc;

                                if (tseNaturalidade.IsValidDBValue && !tseNaturalidade.DBValue.IsNull)
                                {
                                    // Encontrou → nascido no Brasil
                                    txtUFNascimento.Text = tseNaturalidade["uf_sigla"].ToString();
                                }
                                else
                                {
                                    // Nascido fora do Brasil
                                    tseNaturalidade.ResetValue();
                                    tseNaturalidadeEstrangeira.DBValue = dadosPessoa.Municipio_nasc;

                                    if (tseNaturalidadeEstrangeira.IsValidDBValue && !tseNaturalidadeEstrangeira.DBValue.IsNull)
                                    {
                                        txtUFNascimento.Text = tseNaturalidadeEstrangeira["ESTADO"].ToString();
                                        txtPaisNasc.Text = tseNaturalidadeEstrangeira["PAIS"].ToString();
                                    }
                                }
                            }

                            CarregaDadosTipoTransporto(Convert.ToDecimal(dadosAluno.Pessoa));
                            CarregaDadosDeclaracao(dadosAluno.Aluno);

                            var dadosFieldPessoa = FlPessoa.Carregar(Convert.ToDecimal(dadosAluno.Pessoa));

                            if (dadosFieldPessoa != null)
                            {
                                CarregaDadosFieldPessoa(dadosFieldPessoa);

                                if (dadosFieldPessoa.FlField03 != "3")
                                {
                                    tab.Tabs[7].Enabled = true;
                                }
                            }
                            else
                            {
                                ddlLocalZona.SelectedValue = "";
                                ddlGratuidade.SelectedValue = "";
                                ddlTipoCertidao.SelectedValue = "";
                                ddlOutroEnsino.SelectedValue = "";
                            }

                            CarregaBuscaIrmaos(dadosPessoa);

                            googleEducation = rnGoogleEducation.ObtemPor(dadosAluno.Aluno);

                            if (!googleEducation.Email.IsNullOrEmptyOrWhiteSpace())
                            {
                                txtEmailGoogle.Text = googleEducation.Email;
                            }

                            if (txtSituacao.Text == "Ativo")
                            {
                                txtCausaEncerramento.Visible = false;
                                txtMotivo.Visible = false;
                                lblCausaEncerramento.Visible = false;
                                lblMotivo.Visible = false;
                            }
                            else
                            {
                                txtCausaEncerramento.Visible = true;
                                lblCausaEncerramento.Visible = true;
                                txtMotivo.Visible = true;
                                lblMotivo.Visible = true;
                            }

                            DesabilitaCampos();

                            if (!string.IsNullOrEmpty(ddlGratuidade.SelectedValue) && (ddlGratuidade.SelectedValue == "N"))
                            {
                                ddlPoderPublicoTransp.ClearSelection();
                                ddlPoderPublicoTransp.Enabled = false;
                                chkModais.ClearSelection();
                                chkModais.Enabled = false;
                            }

                            pnImprimirMatricula.Visible = true;
                            btnConfRenovMatricula.Visible = (ConfirmacaoMatricula.PossuiConfirmacaoEmAberto(dadosAluno.Aluno) && (RN.PadroesDeAcessos.VerificaPodeCadAlt(Convert.ToString(HttpContext.Current.User.Identity.Name), "Techne.Lyceum.Net.Academico.Alunos", "LyceumNet") || RN.PadroesDeAcessos.VerificaPrivilegio(Convert.ToString(HttpContext.Current.User.Identity.Name))));

                            CarregarGridIrmaosCadasAntes(Convert.ToDecimal(txtPessoa.Text));
                            ExibeOcultaInfoEmailRiocard(true);
                            VerificaNotaEmailDuplicado();
                            HabilitaLoginOperadora();
                            CarregarGridConfirmacaoRenovacaoMatricula();
                            CarregarGridDocumentos();
                            CarregarGridProgramas();
                        }

                        txtPessoa.Visible = false;
                        tab.Tabs[6].Enabled = chkPossuiIrmao.Checked = (grdBuscaIrmaosCadastrados.VisibleRowCount > 0);

                        if (grdAEDH.VisibleRowCount > 0)
                        {
                            pntabAEDH.Enabled = true;
                            tab.Tabs[7].Enabled = true;
                        }

                        break;
                    }

                case TipoOperacao.CadastrarAlunoCompartilhadasBusca:
                    {
                        int ano = 0;
                        int periodo = 0;
                        string unidadeDestino = string.Empty;
                        string unidadeOrigem = string.Empty;
                        string curso = string.Empty;
                        string serie = string.Empty;
                        string modalidade = string.Empty;
                        string segmento = string.Empty;
                        string nomeCurso = string.Empty;
                        hdnCompartilhada.Value = "S";

                        this._mudouCurso = false;

                        byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["ChaveInscricaoCompartilhadas"]);
                        string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

                        string[] inscricaoCompartilhadas = decodedText.Split('&');

                        foreach (string dados in inscricaoCompartilhadas)
                        {
                            if (dados.IndexOf("ano") >= 0)
                            {
                                ano = Convert.ToInt32(dados.Substring(dados.LastIndexOf('=') + 1));
                            }
                            else if (dados.IndexOf("periodo") >= 0)
                            {
                                periodo = Convert.ToInt32(dados.Substring(dados.LastIndexOf('=') + 1));
                            }
                            else if (dados.IndexOf("unidadeDestino") >= 0)
                            {
                                unidadeDestino = dados.Substring(dados.LastIndexOf('=') + 1);
                            }
                            else if (dados.IndexOf("unidadeOrigem") >= 0)
                            {
                                unidadeOrigem = dados.Substring(dados.LastIndexOf('=') + 1);
                            }
                            else if (dados.IndexOf("curso") >= 0)
                            {
                                curso = dados.Substring(dados.LastIndexOf('=') + 1);
                            }
                            else if (dados.IndexOf("serie") >= 0)
                            {
                                serie = dados.Substring(dados.LastIndexOf('=') + 1);
                            }
                        }

                        if (ano > 0 && periodo >= 0 && !unidadeDestino.IsNullOrEmptyOrWhiteSpace() && !unidadeOrigem.IsNullOrEmptyOrWhiteSpace() && !curso.IsNullOrEmptyOrWhiteSpace() && !serie.IsNullOrEmptyOrWhiteSpace())
                        {
                            pnlBuscaAlunoNovo.Visible = true;

                            modalidade = RN.Curso.ConsultaModalidade(curso);

                            btnProximo2.Visible = false;

                            tab.Visible = true;
                            tab.ActiveTabIndex = 0;

                            lblPessoa.Visible = false;
                            pntabDadosPessoais.Visible = false;
                            pnlLoginCartao.Visible = false;
                            pntabDadosEscolares.Visible = false;
                            pntabTransporteEscolar.Visible = false;
                            pntabDocumentos.Visible = false;
                            pntabProgramas.Visible = false;
                            pntabIrmaos.Visible = false;
                            pntabAEDH.Visible = false;
                            btnSalvarDoc.Visible = false;
                            btnSalvarTransporte.Visible = false;
                            pntabEspecializado.Visible = false;
                            txtAluno.Visible = false;
                            lblMatricula.Visible = false;
                            txtPessoa.Visible = false;

                            ImageButton[] controles2 = new ImageButton[] { btnCancel, btnSalvar, btnFotografar };
                            ControlarVisibilidadeControle(controles2);
                            this.txtSituacao.Text = "Ativo";

                            txtCausaEncerramento.Visible = false;
                            txtMotivo.Visible = false;
                            lblCausaEncerramento.Visible = false;
                            lblMotivo.Visible = false;
                            txtPessoa.Text = Convert.ToString(0);
                        }
                        else
                        {
                            _tipoOperacao = TipoOperacao.Inicial;

                            ControlarTipoOperacao();
                            ControlarTSearchs();

                            lblMensagem.Text = "Dados não encontrado da Inscrição de Compartilhadas. Favor verifique.";
                        }
                        break;
                    }
                case TipoOperacao.CadastrarAlunoCompartilhadas:
                    {
                        int ano = 0;
                        int periodo = 0;
                        string unidadeDestino = string.Empty;
                        string unidadeOrigem = string.Empty;
                        string curso = string.Empty;
                        string serie = string.Empty;
                        string modalidade = string.Empty;
                        string segmento = string.Empty;
                        string nomeCurso = string.Empty;

                        this._mudouCurso = false;

                        byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["ChaveInscricaoCompartilhadas"]);
                        string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

                        string[] inscricaoCompartilhadas = decodedText.Split('&');

                        foreach (string dados in inscricaoCompartilhadas)
                        {
                            if (dados.IndexOf("ano") >= 0)
                            {
                                ano = Convert.ToInt32(dados.Substring(dados.LastIndexOf('=') + 1));
                            }
                            else if (dados.IndexOf("periodo") >= 0)
                            {
                                periodo = Convert.ToInt32(dados.Substring(dados.LastIndexOf('=') + 1));
                            }
                            else if (dados.IndexOf("unidadeDestino") >= 0)
                            {
                                unidadeDestino = dados.Substring(dados.LastIndexOf('=') + 1);
                            }
                            else if (dados.IndexOf("unidadeOrigem") >= 0)
                            {
                                unidadeOrigem = dados.Substring(dados.LastIndexOf('=') + 1);
                            }
                            else if (dados.IndexOf("curso") >= 0)
                            {
                                curso = dados.Substring(dados.LastIndexOf('=') + 1);
                            }
                            else if (dados.IndexOf("serie") >= 0)
                            {
                                serie = dados.Substring(dados.LastIndexOf('=') + 1);
                            }
                        }

                        if (ano > 0 && periodo >= 0 && !unidadeDestino.IsNullOrEmptyOrWhiteSpace() && !unidadeOrigem.IsNullOrEmptyOrWhiteSpace() && !curso.IsNullOrEmptyOrWhiteSpace() && !serie.IsNullOrEmptyOrWhiteSpace())
                        {


                            modalidade = RN.Curso.ConsultaModalidade(curso);

                            btnProximo2.Visible = false;

                            tab.Visible = true;
                            tab.ActiveTabIndex = 0;

                            lblPessoa.Visible = false;
                            pntabDadosPessoais.Visible = true;
                            pnlLoginCartao.Visible = false;
                            pntabDadosEscolares.Visible = false;
                            pntabTransporteEscolar.Visible = false;
                            pntabDocumentos.Visible = false;
                            pntabProgramas.Visible = false;
                            pntabIrmaos.Visible = false;
                            pntabAEDH.Visible = false;
                            btnSalvarDoc.Visible = true;
                            btnSalvarTransporte.Visible = true;
                            pntabEspecializado.Visible = false;

                            tab.Tabs[0].Enabled = true;
                            tab.Tabs[1].Enabled = true;
                            tab.Tabs[2].Enabled = false;
                            tab.Tabs[3].Enabled = false;
                            tab.Tabs[4].Enabled = false;
                            tab.Tabs[5].Enabled = false;
                            tab.Tabs[6].Enabled = false;
                            tab.Tabs[7].Enabled = false;

                            pnImprimirMatricula.Visible = false;
                            LimparTela();
                            LimpaDadosAvaliacaoCuidador();
                            LimpaDadosAvaliacaoInterprete();
                            LimpaDadosAvaliacaoLedor();
                            LimpaDadosAvaliacaoSalaRecurso();
                            LimpaDadosAvaliacaoPAPEE();
                            HabilitaCampos();
                            tseAluno.ResetValue();
                            txtAluno.Visible = false;
                            lblMatricula.Visible = true;
                            txtPessoa.Visible = false;

                            ImageButton[] controles2 = new ImageButton[] { btnCancel, btnSalvar, btnFotografar };
                            ControlarVisibilidadeControle(controles2);
                            this.txtSituacao.Text = "Ativo";
                            txtSituacao.ReadOnly = true;

                            txtCausaEncerramento.Visible = false;
                            txtMotivo.Visible = false;
                            lblCausaEncerramento.Visible = false;
                            lblMotivo.Visible = false;
                            txtPessoa.Text = Convert.ToString(0);

                            cmbTurno.Enabled = true;
                            cmbCurriculo.Enabled = true;
                            txtPaisNasc.Text = string.Empty;
                            PadronizarDropDownList(ddlNacionalidade, "");
                            cmbAnoIngresso.SelectedIndex = cmbAnoIngresso.Items.IndexOf(cmbAnoIngresso.Items.FindByValue(Convert.ToString(ano)));
                            cmbAnoIngresso.Enabled = false;

                            cmbSemIngresso.SelectedIndex = cmbSemIngresso.Items.IndexOf(cmbSemIngresso.Items.FindByValue(Convert.ToString(periodo)));
                            cmbSemIngresso.Enabled = false;

                            CarregaUnidadeEnsinoPor(unidadeDestino);

                            tseUnidadeEns.Enabled = false;
                            ddlNivel.SelectedIndex = ddlNivel.Items.IndexOf(ddlNivel.Items.FindByValue(Convert.ToString("3")));
                            ddlNivel.Enabled = false;
                            ddlModalidade.SelectedIndex = ddlModalidade.Items.IndexOf(ddlModalidade.Items.FindByValue(Convert.ToString(modalidade)));
                            ddlModalidade.Enabled = false;
                            ddlRedeEnsinoOrigem.SelectedIndex = ddlRedeEnsinoOrigem.Items.IndexOf(ddlRedeEnsinoOrigem.Items.FindByValue(Convert.ToString("Municipal")));
                            ddlRedeEnsinoOrigem.Enabled = false;

                            cmbSerie.Enabled = false;
                            cmbSerie.SelectedValue = serie;
                            tseCurso.DBValue = curso;
                            tseCurso.Enabled = false;
                            ddlPoderPublicoTransp.Enabled = false;
                            txtUFNascimento.Enabled = true;
                            chkModais.Enabled = false;

                            txtNomeMae.ReadOnly = false;
                            txtNomeCompl.ReadOnly = false;
                            dtDataNasc.Enabled = true;
                        }
                        else
                        {
                            _tipoOperacao = TipoOperacao.Inicial;

                            ControlarTipoOperacao();
                            ControlarTSearchs();

                            lblMensagem.Text = "Dados não encontrado da Inscrição de Compartilhadas. Favor verifique.";
                        }
                        break;
                    }
                case TipoOperacao.NovaMatricula:
                    {
                        btnProximo2.Visible = false;
                        _mudouCurso = false;
                        tab.Visible = true;
                        tab.ActiveTabIndex = 0;

                        pntabDadosPessoais.Visible = true;
                        pnlLoginCartao.Visible = false;
                        pntabDadosEscolares.Visible = false;
                        pntabTransporteEscolar.Visible = false;
                        pntabDocumentos.Visible = false;
                        pntabProgramas.Visible = false;
                        pntabIrmaos.Visible = false;
                        pntabAEDH.Visible = false;
                        btnSalvarDoc.Visible = true;
                        btnSalvarTransporte.Visible = true;
                        btnEditar.Visible = false;
                        pntabEspecializado.Visible = false;

                        tab.Tabs[0].Enabled = true;
                        tab.Tabs[1].Enabled = true;
                        tab.Tabs[2].Enabled = false;
                        tab.Tabs[3].Enabled = false;
                        tab.Tabs[4].Enabled = false;
                        tab.Tabs[5].Enabled = false;
                        tab.Tabs[6].Enabled = false;
                        tab.Tabs[7].Enabled = false;

                        pnImprimirMatricula.Visible = false;
                        LimparTela();
                        LimpaDadosAvaliacaoCuidador();
                        LimpaDadosAvaliacaoInterprete();
                        LimpaDadosAvaliacaoLedor();
                        LimpaDadosAvaliacaoSalaRecurso();
                        LimpaDadosAvaliacaoPAPEE();
                        HabilitaCampos();
                        tseAluno.ResetValue();
                        CarregaUnidadeEnsino();
                        tseUnidadeEns.ResetValue();
                        txtAluno.Visible = false;
                        lblMatricula.Visible = true;
                        txtPessoa.Visible = false;

                        ImageButton[] controles2 = new ImageButton[] { btnCancel, btnSalvar, btnFotografar };
                        ControlarVisibilidadeControle(controles2);
                        this.txtSituacao.Text = "Ativo";
                        txtSituacao.ReadOnly = true;

                        txtCausaEncerramento.Visible = false;
                        txtMotivo.Visible = false;
                        lblCausaEncerramento.Visible = false;
                        lblMotivo.Visible = false;

                        txtPessoa.Text = Convert.ToString(0);

                        ddlRedeEnsinoOrigem.Enabled = true;
                        txtPaisNasc.Text = string.Empty;
                        PadronizarDropDownList(ddlNacionalidade, "");

                        chkModais.Enabled = false;
                        ddlPoderPublicoTransp.Enabled = false;
                        txtUFNascimento.Enabled = true;
                        tseUnidadeEns.Mode = ControlMode.View;
                        tseCurso.Mode = ControlMode.View;

                        tseUnidadeEns.Enabled = false;
                        tseCurso.Enabled = false;

                        cmbAnoIngresso.SelectedValue = ddlAnoMatricula.SelectedValue;
                        cmbSemIngresso.SelectedValue = ddlPeriodoMatricula.SelectedValue;
                        ddlNivel.SelectedValue = ddlNivelMatricula.SelectedValue;
                        ddlModalidade.SelectedValue = ddlModalidadeMatricula.SelectedValue;
                        CarregaUnidadeEnsinoPor(tseUnidadeEnsinoMatricula.DBValue.ToString());
                        tseUnidadeEns.DBValue = tseUnidadeEnsinoMatricula.DBValue;
                        CarregarFiltroCurso();
                        tseCurso.DBValue = tseCursoMatricula.DBValue;
                        tseCurso_Changed(null, null);
                        cmbTurno.SelectedValue = cmbTurnoMatricula.SelectedValue;
                        cmbTurno_SelectedIndexChanged(null, null);
                        cmbCurriculo.SelectedValue = cmbCurriculoMatricula.SelectedValue;
                        cmbCurriculo_SelectedIndexChanged(null, null);
                        cmbSerie.SelectedValue = cmbSerieMatricula.SelectedValue;

                        cmbAnoIngresso.Enabled = false;
                        cmbSemIngresso.Enabled = false;
                        ddlNivel.Enabled = false;
                        ddlModalidade.Enabled = false;
                        cmbTurno.Enabled = false;
                        cmbCurriculo.Enabled = false;
                        cmbSerie.Enabled = false;
                        ExibeOcultaInfoEmailRiocard(false);
                        chkNaoSeAplica.Checked = true;
                        ddlPovoIndigena.Visible = false;
                        lblPovo.Visible = false;
                        ddlPovoIndigena.ClearSelection();
                        break;
                    }
            }
        }

        private void HabilitaLoginOperadora()
        {
            Techne.Lyceum.RN.CartaoEstudante.DTO.DadosAlunoOperadoraDTO dadosAlunoOperadora = LoginOperadoraAlunoService.Instancia.ObtemLoginOperadoraPor(tseAluno.DBValue.ToString());
            pnlLoginCartao.Visible = false;
            if (dadosAlunoOperadora != null)
            {
                txtLoginCartao.Text = dadosAlunoOperadora.LoginOperadora;
                txtOperadora.Text = dadosAlunoOperadora.NomeOperadora;
                txtCodigoAlunoOperadora.Text = dadosAlunoOperadora.AlunoOperadoraId.ToString();
                txtDataAtualizacaoLoginOperadora.Text = dadosAlunoOperadora.DataAtualizacaoLogin.ToString();
                pnlLoginCartao.Visible = true;
            }

            lblObsLoginCartao.Visible = pnlLoginCartao.Visible && (_tipoOperacao == TipoOperacao.Alterar);
            rowDtAtualizacaoEmail.Visible = ((_tipoOperacao == TipoOperacao.Consultar) && (dadosAlunoOperadora != null));

        }

        private void ControlaVisibilidadeMensagemBloqueio()
        {
            lblMensagemBloqueio.Text = string.Empty;
            RN.Agenda.Evento rnEvento = new Techne.Lyceum.RN.Agenda.Evento();
            List<RN.Agenda.Entidades.Evento> eventos = new List<RN.Agenda.Entidades.Evento>();
            int idEventoBloqueioaMatricula = Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.BloqueioCadastroMatricula);
            RN.Perfil rnPerfil = new Perfil();

            //Verifica se existem eventos de bloqueio abertos
            eventos = rnEvento.ListaEventoAbertoPor(idEventoBloqueioaMatricula, DateTime.Today);

            if (eventos.Count != 0 && !rnPerfil.PossuiPerfilMatriculaTransferenciaPeriodoBloqueioPor(User.Identity.Name))
            {
                lblMensagemBloqueio.Text = "Há um ou mais bloqueios de matrícula vigentes, é necessário informar o ano, período, unidade e curso para prosseguir.";
            }
        }

        private void ControlarObrigatoriedadeDocumentos(string tipoDocumento)
        {
            if (tipoDocumento == "RG")
            {
                lblRG_Num.Text = "Número*: ";
                lblRG_Num.Font.Bold = true;

                lblRG_UF.Text = "Estado*: ";
                lblRG_UF.Font.Bold = true;

                lblRG_Emissor.Text = "Órgão Emissor*: ";
                lblRG_Emissor.Font.Bold = true;

                lblRG_Data.Text = "Data de Expedição*: ";
                lblRG_Data.Font.Bold = true;
            }
            else if (tipoDocumento == string.Empty)
            {
                lblRG_Num.Text = "Número: ";
                lblRG_Num.Font.Bold = false;

                lblRG_UF.Text = "Estado: ";
                lblRG_UF.Font.Bold = false;

                lblRG_Emissor.Text = "Órgão Emissor: ";
                lblRG_Emissor.Font.Bold = false;

                lblRG_Data.Text = "Data de Expedição: ";
                lblRG_Data.Font.Bold = false;
            }
            else
            {
                lblRG_Num.Text = "Número*: ";
                lblRG_Num.Font.Bold = true;

                lblRG_UF.Text = "Estado: ";
                lblRG_UF.Font.Bold = false;

                lblRG_Emissor.Text = "Órgão Emissor*: ";
                lblRG_Emissor.Font.Bold = true;

                lblRG_Data.Text = "Data de Expedição: ";
                lblRG_Data.Font.Bold = false;
            }
        }

        private void Salvar()
        {
            try
            {
                RN.Entidades.DeclaracaoAusencia dtDeclaracao = new RN.Entidades.DeclaracaoAusencia();
                List<RN.Entidades.PessoaRecursoAplicacaoProva> listPessoaRecursoAplicacaoProva = new List<RN.Entidades.PessoaRecursoAplicacaoProva>();
                List<int> listTipoRecursoAluno = new List<int>();
                List<int> listTipoTranstorno = new List<int>();
                var alterarFoto = this.AlteraFoto();
                var declaracoesAusencia = new List<RN.Entidades.DeclaracaoAusencia>();
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Aluno rnAluno = new Aluno();

                string tipo_resp = string.Empty;
                string unidadeCompartilhada = string.Empty;
                string unidadeOrigem = string.Empty;
                string unidadeDestino = string.Empty;
                byte[] decodedBytes = null;
                string decodedText = string.Empty;
                string[] inscricaoCompartilhadas = null;
                string mensagem = string.Empty;
                string naturalidade = string.Empty;
                string municipioEstrangeiro = string.Empty;
                RN.RecursosHumanos.Entidades.GoogleEducation googleEducation = new Techne.Lyceum.RN.RecursosHumanos.Entidades.GoogleEducation();

                var aluno = new LyAluno
                {
                    Aluno = !txtAluno.Text.IsNullOrEmptyOrWhiteSpace() ? txtAluno.Text.Trim() : null,
                    SitAluno = !txtSituacao.Text.IsNullOrEmptyOrWhiteSpace() ? txtSituacao.Text.Trim() : null,
                    Pessoa = !txtPessoa.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(txtPessoa.Text.Trim()) : -1,
                    Numinscricao = !txtNumInscricao.Text.IsNullOrEmptyOrWhiteSpace() ? txtNumInscricao.Text.Trim() : null,
                    UnidadeEnsino = (!tseUnidadeEns.DBValue.IsNull && tseUnidadeEns.IsValidDBValue) ? Convert.ToString(tseUnidadeEns["unidade_ens"]) : null,
                    UnidadeFisica = (!tseUnidadeEns.DBValue.IsNull && tseUnidadeEns.IsValidDBValue) ? Convert.ToString(tseUnidadeEns["unidade_ens"]) : null,
                    Turno = !cmbTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? cmbTurno.SelectedValue : null,
                    Curso = (!tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue) ? Convert.ToString(tseCurso["curso"]) : null,
                    Serie = !cmbSerie.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(cmbSerie.Text.Trim()) : -1,
                    AnoIngresso = !cmbAnoIngresso.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(cmbAnoIngresso.Text.Trim()) : -1,
                    SemIngresso = !cmbSemIngresso.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(cmbSemIngresso.Text.Trim()) : -1,
                    Curriculo = !cmbCurriculo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? cmbCurriculo.SelectedValue : null,
                    TipoIngresso = !ddlTipoIngresso.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTipoIngresso.SelectedValue : null,
                    EMailInterno = !txtEmail.Text.IsNullOrEmptyOrWhiteSpace() ? txtEmail.Text.Trim() : null,
                    TipoEnsinoProfissionalizante = !ddlTipoCurso.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTipoCurso.SelectedValue : null,
                    RedeEnsinoOrigem = !ddlRedeEnsinoOrigem.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlRedeEnsinoOrigem.SelectedValue : null,
                    TempoAfastamentoRede = !txtTempoAfastamento.Text.IsNullOrEmptyOrWhiteSpace() ? int.Parse(txtTempoAfastamento.Text) : (int?)null,
                    EMailAnterior = !hddTxtEmail.Value.IsNullOrEmptyOrWhiteSpace() ? hddTxtEmail.Value.Trim() : null,
                    EMailConfirmacao = !txtEmailConfirmacao.Text.IsNullOrEmptyOrWhiteSpace() ? txtEmailConfirmacao.Text.Trim() : null,
                    DeclaroNecessidadeEspecial = chkDeclaroNecessidadeEspecial.Checked,
                    DeclaroAusenciaMae = chkDeclaroAusenciaMae.Checked,
                    DeclaroAusenciaPai = chkDeclaroAusenciaPai.Checked,
                    DeclaroCertidaoCivil = chkDeclaroCertidaoCivil.Checked,
                    NenhumRecursoAplicacaoProva = chkNenhumRecursoAplicacaoProva.Checked,
                    MunicipioEscola = (!tseUnidadeEns.DBValue.IsNull && tseUnidadeEns.IsValidDBValue) ? tseUnidadeEns["municipio"].ToString() : null,
                    DtCadastro = DateTime.Now,
                    Usuario = User.Identity.Name,
                };


                if (!hddTxtEmail.Value.Equals(txtEmail.Text))
                {
                    aluno.DataAtualizacaoEmailInterno = DateTime.Now;
                }
                else
                {
                    aluno.DataAtualizacaoEmailInterno = string.IsNullOrEmpty(hddDataAlteracaoEmail.Value) ? default(DateTime?) : DateTime.Parse(hddDataAlteracaoEmail.Value);
                }

                foreach (ListItem item in rblResponsavel.Items)
                {
                    if (item.Selected && !string.IsNullOrEmpty(item.Value))
                    {
                        tipo_resp += item.Value;
                        tipo_resp += ";";
                    }
                }

                if (!txtPaisNasc.Text.IsNullOrEmptyOrWhiteSpace())
                {
                    if (txtPaisNasc.Text.ToUpper() == "BRASIL")
                    {
                        naturalidade = (!tseNaturalidade.DBValue.IsNull && tseNaturalidade.IsValidDBValue) ? Convert.ToString(tseNaturalidade.DBValue) : null;
                    }
                    else
                    {
                        // obtém o municipio estrangeiro
                        SimpleRow sr = Endereco.ObterCodigoMunicipioEstrangeiroHades(tseNaturalidadeEstrangeira.DBValue.ToString());

                        //verifica se a função retornou algum valor para a simplerow
                        if (sr != null)
                        {
                            //preenche os dados obtidos de municipio estrangeiro
                            if (!sr["municipio_estrangeiro"].IsNull)
                            {
                                municipioEstrangeiro = Convert.ToString(sr["municipio_estrangeiro"]);
                            }
                        }

                        naturalidade = !municipioEstrangeiro.IsNullOrEmptyOrWhiteSpace() ? municipioEstrangeiro : null;
                    }
                }

                var pessoa = new LyPessoa
                {
                    Pessoa = !txtPessoa.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(txtPessoa.Text) : -1,
                    Nome_compl = !txtNomeCompl.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeCompl.Text.Trim().ToUpper() : null,
                    Dt_nasc = !dtDataNasc.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataNasc.Date : (DateTime?)null,
                    Sexo = !rblSexo.Text.IsNullOrEmptyOrWhiteSpace() ? rblSexo.Text : null,
                    Est_civil = !ddlEst_Civil.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlEst_Civil.SelectedValue : null,
                    NecessidadeEspecialId = !cmbNecessidadeEspecial.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(cmbNecessidadeEspecial.SelectedValue) : (int?)null,
                    Tipo_Sanguineo = !ddlTipoSanguineo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTipoSanguineo.SelectedValue : null,
                    Etnia = !ddlEtnia.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlEtnia.SelectedValue : null,
                    Credo = !ddlCredo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlCredo.SelectedValue : null,
                    QtFilhos = !txtFilhos.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(txtFilhos.Text.Trim()) : (decimal?)null,
                    PreNomeSocial = !txtNomeSocial.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeSocial.Text.Trim().ToUpper() : null,
                    Nacionalidade = !ddlNacionalidade.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlNacionalidade.SelectedValue : null,
                    Pais_nasc = !txtPaisNasc.Text.IsNullOrEmptyOrWhiteSpace() ? txtPaisNasc.Text : null,
                    Endereco = !txtEndereco.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndereco.Text.Trim() : null,
                    End_num = !txtEndNum.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndNum.Text.Trim() : null,
                    End_compl = !txtEndCompl.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndCompl.Text.Trim() : null,
                    Cep = !txtCep.Text.RetirarCaracteres().IsNullOrEmptyOrWhiteSpace() ? txtCep.Text.RetirarCaracteres() : null,
                    Bairro = !txtBairro.Text.IsNullOrEmptyOrWhiteSpace() ? txtBairro.Text.Trim() : null,
                    End_municipio = !hdnCodMunicipio.Value.IsNullOrEmptyOrWhiteSpace() ? hdnCodMunicipio.Value : null,
                    Fone = txtFone.Text.Trim(),
                    Celular = txtCelular.Text.Trim(),
                    E_mail = !txtEmail.Text.IsNullOrEmptyOrWhiteSpace() ? txtEmail.Text.Trim() : null,
                    Municipio_nasc = naturalidade,
                    Rg_tipo = !ddlRGTipoPessoa.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlRGTipoPessoa.SelectedValue : null,
                    Rg_num = !txtRGNum.Text.IsNullOrEmptyOrWhiteSpace() ? txtRGNum.Text.Trim() : null,
                    Rg_uf = !cmbRGUF.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? cmbRGUF.SelectedValue : null,
                    Rg_emissor = !cmbRGEmissor.Text.IsNullOrEmptyOrWhiteSpace() ? cmbRGEmissor.Text.Trim() : null,
                    Rg_dtexp = !dtDataExped.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataExped.Date : (DateTime?)null,
                    Id_censo = !txtIDCenso.Text.IsNullOrEmptyOrWhiteSpace() ? txtIDCenso.Text.Trim() : null,
                    Cpf = !txtCPF.Text.RetirarMascaraCPF().IsNullOrEmptyOrWhiteSpace() ? txtCPF.Text.RetirarMascaraCPF().Trim() : null,
                    CertNascNum = !txtDOC_CertNasc_Numero.Text.IsNullOrEmptyOrWhiteSpace() ? txtDOC_CertNasc_Numero.Text.Trim() : null,
                    CertNascFolha = !txtDOC_CertNasc_Folha.Text.IsNullOrEmptyOrWhiteSpace() ? txtDOC_CertNasc_Folha.Text.Trim() : null,
                    CertNascLivro = !txtDOC_CertNasc_Livro.Text.IsNullOrEmptyOrWhiteSpace() ? txtDOC_CertNasc_Livro.Text.Trim() : null,
                    CertNascCartorioExped = !ddlCartorio.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlCartorio.SelectedItem.Text : null,
                    IdCartorio = !ddlCartorio.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? int.Parse(ddlCartorio.SelectedValue) : (int?)null,
                    CertNumeroMatricula = !txtNumMatriculaCertidao.Text.IsNullOrEmptyOrWhiteSpace() ? txtNumMatriculaCertidao.Text.Trim() : null,
                    CertNascEmissao = !dboDOC_CertNasc_DtEmissao.Text.IsNullOrEmptyOrWhiteSpace() ? dboDOC_CertNasc_DtEmissao.Date : (DateTime?)null,
                    CertNascCartorioUf = !ddDOC_CertNasc_Uf.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddDOC_CertNasc_Uf.SelectedValue : null,
                    MaeFalecida = chkFalecidaMae.Checked ? "S" : "N",
                    PaiFalecido = chkFalecidoPai.Checked ? "S" : "N",
                    MaeCpf = !txtCPFMae.Text.IsNullOrEmptyOrWhiteSpace() ? txtCPFMae.Text.Trim() : null,
                    PaiCpf = !txtCPFPai.Text.IsNullOrEmptyOrWhiteSpace() ? txtCPFPai.Text.Trim() : null,
                    MaeTelefone = !txtTelefoneMae.Text.IsNullOrEmptyOrWhiteSpace() ? txtTelefoneMae.Text.Trim() : null,
                    PaiTelefone = !txtTelefonePai.Text.IsNullOrEmptyOrWhiteSpace() ? txtTelefonePai.Text.Trim() : null,
                    Responsavel = tipo_resp,
                    RespNomeCompl = !txtNomeResponsavel.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeResponsavel.Text.Trim().ToUpper() : null,
                    RespCpf = !txtCPFResponsavel.Text.IsNullOrEmptyOrWhiteSpace() ? txtCPFResponsavel.Text.Trim() : null,
                    RespFone = !txtTelefoneResp.Text.IsNullOrEmptyOrWhiteSpace() ? txtTelefoneResp.Text.Trim() : null,
                    NomeMae = !txtNomeMae.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeMae.Text.Trim().ToUpper() : null,
                    NomePai = !txtNomePai.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomePai.Text.Trim().ToUpper() : null,
                    AreaAssentamento = chkAreaAssentamento.Checked ? "S" : "N",
                    TerraIndigena = chkTerraIndigena.Checked ? "S" : "N",
                    AreaQuilombos = chkQuilombos.Checked ? "S" : "N",
                    AreaTradicional = chkAreaTradicional.Checked ? "S" : "N",
                    Latitude = !txtLatitude.Text.IsNullOrEmptyOrWhiteSpace() ? txtLatitude.Text.Trim() : null,
                    Longitude = !txtLongitude.Text.IsNullOrEmptyOrWhiteSpace() ? txtLongitude.Text.Trim() : null,
                    UsuarioId = User.Identity.Name
                };

                long resultado;

                if (long.TryParse(txtCelular.Text.Trim().RetirarMascaraTelefone(), out resultado))
                {
                    if (txtCelular.Text.Trim().RetirarMascaraTelefone().Length == 10)
                    {
                        pessoa.Celular = string.Format("{0:(00)0000-0000}", resultado);
                    }
                    if (txtCelular.Text.Trim().RetirarMascaraTelefone().Length == 11)
                    {
                        pessoa.Celular = string.Format("{0:(00)00000-0000}", resultado);
                    }
                    txtCelular.Text = pessoa.Celular;
                }
                else
                {
                    pessoa.Celular = null;
                }

                if (chkNaoDeclarMae.Checked)
                {
                    dtDeclaracao = new RN.Entidades.DeclaracaoAusencia();

                    dtDeclaracao.AlunoId = !txtAluno.Text.IsNullOrEmptyOrWhiteSpace() ? txtAluno.Text.Trim() : null;
                    dtDeclaracao.Matricula = User.Identity.Name;
                    dtDeclaracao.TipoDeclaracaoAusenciaId = Convert.ToInt32(RN.DeclaracaoAusencia.TipoDeclaracaoAusenciaId.DeclaracaoAusenciaMae);
                    declaracoesAusencia.Add(dtDeclaracao);
                }
                if (chkNaoDeclarPai.Checked)
                {
                    dtDeclaracao = new RN.Entidades.DeclaracaoAusencia();

                    dtDeclaracao.AlunoId = !txtAluno.Text.IsNullOrEmptyOrWhiteSpace() ? txtAluno.Text.Trim() : null;
                    dtDeclaracao.Matricula = User.Identity.Name;
                    dtDeclaracao.TipoDeclaracaoAusenciaId = Convert.ToInt32(RN.DeclaracaoAusencia.TipoDeclaracaoAusenciaId.DeclaracaoAusenciaPai);
                    declaracoesAusencia.Add(dtDeclaracao);
                }
                if (ddlTipoCertidao.SelectedValue == "Nenhum")
                {
                    dtDeclaracao = new RN.Entidades.DeclaracaoAusencia();

                    dtDeclaracao.AlunoId = !txtAluno.Text.IsNullOrEmptyOrWhiteSpace() ? txtAluno.Text.Trim() : null;
                    dtDeclaracao.Matricula = User.Identity.Name;
                    dtDeclaracao.TipoDeclaracaoAusenciaId = Convert.ToInt32(RN.DeclaracaoAusencia.TipoDeclaracaoAusenciaId.DeclaracaoCertidaoCivil);
                    dtDeclaracao.Motivo = txtMotivoCertidaoCivil.Text.TrimEnd().ToUpper();
                    declaracoesAusencia.Add(dtDeclaracao);
                }
                if (cmbNecessidadeEspecial.SelectedValue != "30")//"Não possui."
                {
                    dtDeclaracao = new RN.Entidades.DeclaracaoAusencia();

                    dtDeclaracao.AlunoId = !txtAluno.Text.IsNullOrEmptyOrWhiteSpace() ? txtAluno.Text.Trim() : null;
                    dtDeclaracao.Matricula = User.Identity.Name;
                    dtDeclaracao.TipoDeclaracaoAusenciaId = Convert.ToInt32(RN.DeclaracaoAusencia.TipoDeclaracaoAusenciaId.DeclaracaoNecessidadeEspecial);
                    declaracoesAusencia.Add(dtDeclaracao);
                }


                var flPessoa = new LyFlPessoa
                {
                    Pessoa = !txtPessoa.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(this.txtPessoa.Text) : -1,
                    FlField01 = !ddlLocalZona.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlLocalZona.SelectedValue : null,
                    FlField02 = !ddlTipoCertidao.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTipoCertidao.SelectedValue : null,
                    FlField03 = !ddlOutroEnsino.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlOutroEnsino.SelectedValue : null,
                    FlField07 = !txtComplIdentidade.Text.IsNullOrEmptyOrWhiteSpace() ? txtComplIdentidade.Text : null,
                    FlField08 = !txtNIS.Text.IsNullOrEmptyOrWhiteSpace() ? txtNIS.Text.Trim() : null,
                    FlField09 = !ddlCertidaoCivil.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlCertidaoCivil.SelectedValue : null,
                    FlField10 = !ddlPoderPublicoTransp.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlPoderPublicoTransp.SelectedValue : null,
                    FlField21 = !ddlPovoIndigena.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlPovoIndigena.SelectedValue : null,
                    FlField22 = !rblPossuiTranstorno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblPossuiTranstorno.SelectedValue : null,
                    FlField23 = !rblDescFamilia.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblDescFamilia.SelectedValue : null
                };


                var fotoPessoa = new LyFotoPessoa();
                var msg = this.ObterDadosFotoPessoa(fotoPessoa);

                if (msg != string.Empty)
                {
                    lblMensagem.Text = msg;
                    return;
                }

                DateTime dataAtualizacao = DateTime.Now;

                if (!cmbNecessidadeEspecial.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    if (cmbNecessidadeEspecial.SelectedValue != "30") //NAO POSSUI
                    {
                        foreach (ListItem item in chkRecursoNecessidadeEspecial.Items)
                        {
                            if (item.Selected)
                            {
                                listTipoRecursoAluno.Add(Convert.ToInt32(item.Value));
                            }
                        }

                        foreach (ListItem item in rblRecursoAplicaProvaExclusivo.Items)
                        {
                            if (item.Selected)
                            {
                                AdicionaItemRecursoAplicacaoProva(listPessoaRecursoAplicacaoProva, dataAtualizacao, item.Value);
                            }
                        }

                        foreach (ListItem item in chkRecursoAplicacaoProva.Items)
                        {
                            if (item.Selected)
                            {
                                AdicionaItemRecursoAplicacaoProva(listPessoaRecursoAplicacaoProva, dataAtualizacao, item.Value);
                            }
                        }
                    }
                }

                //Verifica se é compartilhada
                bool compartilhada = Request.QueryString["ChaveInscricaoCompartilhadas"] != null;
                if (compartilhada)
                {
                    decodedBytes = Convert.FromBase64String(Request.QueryString["ChaveInscricaoCompartilhadas"]);
                    decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                    inscricaoCompartilhadas = decodedText.Split('&');

                    foreach (string dados in inscricaoCompartilhadas)
                    {
                        if (dados.IndexOf("unidadeDestino") >= 0)
                        {
                            unidadeDestino = dados.Substring(dados.LastIndexOf('=') + 1);
                        }
                        else if (dados.IndexOf("unidadeOrigem") >= 0)
                        {
                            unidadeOrigem = dados.Substring(dados.LastIndexOf('=') + 1);
                        }
                    }
                }

                if (!txtEmailGoogle.Text.IsNullOrEmptyOrWhiteSpace())
                {
                    googleEducation.Email = txtEmailGoogle.Text.Trim();
                }

                if (rblPossuiTranstorno.SelectedValue == "S")
                {
                    foreach (ListItem item in chkTipoTranstorno.Items)
                    {
                        if (item.Selected)
                        {
                            listTipoTranstorno.Add(Convert.ToInt32(item.Value));
                        }
                    }
                }

                validacao = rnAluno.Valida(aluno, pessoa, flPessoa, fotoPessoa, declaracoesAusencia, listPessoaRecursoAplicacaoProva, listTipoRecursoAluno, (txtAluno.Text.IsNullOrEmptyOrWhiteSpace()) ? true : false, _mudouCurso, compartilhada, unidadeDestino, unidadeOrigem, (chkNaoSeAplica.Checked ? "S" : "N"), googleEducation, listTipoTranstorno);

                if (validacao.Valido)
                {
                    if (this._tipoOperacao.Equals(TipoOperacao.Novo) || _tipoOperacao.Equals(TipoOperacao.NovaMatricula) || this._tipoOperacao.Equals(TipoOperacao.CadastrarAlunoCompartilhadas))
                    {
                        rnAluno.Insere(aluno, pessoa, flPessoa, fotoPessoa, declaracoesAusencia, listPessoaRecursoAplicacaoProva, listTipoRecursoAluno, compartilhada, unidadeDestino, unidadeOrigem, listTipoTranstorno);

                        txtAluno.Text = aluno.Aluno;
                        txtPessoa.Text = aluno.Pessoa.ToString();
                        mensagem = "Aluno inserido com sucesso. Favor realizar a confirmação/renovação do aluno na aba \"Dados Escolares\". Somente após esta confirmação será possível realizar a Enturmação do Aluno.";
                        pnImprimirMatricula.Visible = true;
                    }
                    else if (this._tipoOperacao.Equals(TipoOperacao.Alterar))
                    {
                        rnAluno.Atualiza(aluno, pessoa, flPessoa, fotoPessoa, declaracoesAusencia, listPessoaRecursoAplicacaoProva, listTipoRecursoAluno, googleEducation, compartilhada, unidadeDestino, unidadeOrigem, listTipoTranstorno);
                        mensagem = "Aluno alterado com sucesso. Favor verificar se a confirmação/renovação do aluno foi realizada na aba \"Dados Escolares\".";
                    }

                    txtNomeCompl.Text = Convert.ToString(pessoa.Nome_compl);
                    txtNomeMae.Text = Convert.ToString(pessoa.NomeMae);
                    txtNomePai.Text = Convert.ToString(pessoa.NomePai);
                    txtNomeSocial.Text = Convert.ToString(pessoa.PreNomeSocial);

                    hddTxtEmail.Value = aluno.EMailInterno;
                    hddDataAlteracaoEmail.Value = aluno.DataAtualizacaoEmailInterno.HasValue ? aluno.DataAtualizacaoEmailInterno.Value.ToString() : string.Empty;
                    txtDataAtualizacaoEmail.Text = hddDataAlteracaoEmail.Value;
                    txtDataAtualizacaoEmailTransp.Text = hddDataAlteracaoEmail.Value;
                    this.MaintainScrollPositionOnPostBack = false;

                    // Exibe a foto
                    var dadosFotoPessoa = FotoPessoa.Carregar(Convert.ToInt32(pessoa.Pessoa));
                    VerificaNotaEmailDuplicado();
                    this.CarregaDadosFotoPessoa(dadosFotoPessoa);
                    this.CarregarGridConfirmacaoRenovacaoMatricula();
                    tab.Visible = true;
                    pntabDadosPessoais.Visible = false;
                    pntabDadosEscolares.Visible = true;
                    pntabTransporteEscolar.Visible = false;
                    pntabDocumentos.Visible = false;
                    pntabProgramas.Visible = false;
                    pntabIrmaos.Visible = false;
                    pntabAEDH.Visible = false;
                    pntabEspecializado.Visible = false;
                    pntabIrmaos.Enabled = true;

                    txtMae.Text = txtNomeMae.Text;
                    txtPai.Text = txtNomePai.Text;
                    tab.ActiveTabIndex = 1;

                    if (cmbNecessidadeEspecial.SelectedValue == "30")//"Não possui."
                    {
                        chkDeclaroNecessidadeEspecial.Checked = false;
                        chkDeclaroNecessidadeEspecial.Visible = false;
                        chkDeclaroNecessidadeEspecial.Text = "Declaro estar de posse do Laudo Médico ou Parecer Pedagógico";
                        pntabEspecializado.Enabled = false;
                    }
                    else
                    {
                        chkDeclaroNecessidadeEspecial.Visible = true;
                        pntabEspecializado.Enabled = true;
                        tab.Tabs[5].Enabled = true;
                    }

                    this._tipoOperacao = TipoOperacao.Sucesso;
                    this.ControlarTipoOperacao();
                    this.ControlarTSearchs();

                    var script = @"alert('" + mensagem + @"');";

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
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

        private void VerificaNotaEmailDuplicado()
        {
            lblNotaEmailDuplicado.Text = string.Empty;
            lblNotaEmailDuplicado.Visible = false;
            Aluno aluno = new Aluno();
            string alunoComMesmoEmail = aluno.ObtemAlunoComDuplicidadeDeEmailPor(tseAluno.DBValue.ToString(), txtEmail.Text);

            if (!string.IsNullOrEmpty(alunoComMesmoEmail))
            {
                lblNotaEmailDuplicado.Text = "Nota: Este e-mail já está cadastrado para a matrícula " + alunoComMesmoEmail;
                lblNotaEmailDuplicado.Visible = true;
            }
        }

        /// <summary>
        /// Alterna a exibição dos campos "Data da última atualização do e-mail" e "Confirmação de e-mail", de acordo com o valor passado.
        /// </summary>
        /// <param name="exibe"></param>
        /// <remarks>Deve ser true para operação de consulta na tela e false, para inclusão ou alteração</remarks>
        /// <example>Exemplo de uso</example>
        /// <code>switch tipoOperacao { 
        ///     case TipoOperacao.Consultar: ExibeOcultaInfoEmailRiocard(true);
        /// } </code>
        private void ExibeOcultaInfoEmailRiocard(bool exibe)
        {
            rowDtAtualizacaoEmail.Visible = ((exibe) && (_tipoOperacao == TipoOperacao.Consultar));
            rowConfirmaEmail.Visible = !exibe;
        }

        private void AdicionaItemRecursoAplicacaoProva(List<RN.Entidades.PessoaRecursoAplicacaoProva> listPessoaRecursoAplicacaoProva, DateTime dataAtualizacao, string strIdRecursoAplicacaoProva)
        {
            RN.Entidades.PessoaRecursoAplicacaoProva entidade = new RN.Entidades.PessoaRecursoAplicacaoProva();
            entidade.DataAtualizacao = dataAtualizacao;
            entidade.PessoaId = Convert.ToInt32(txtPessoa.Text);
            entidade.RecursoAplicacaoProvaId = Convert.ToInt32(strIdRecursoAplicacaoProva);
            entidade.Usuario = User.Identity.Name;

            listPessoaRecursoAplicacaoProva.Add(entidade);
        }

        #region Métodos Visibilidade dos Botões
        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (var botao in botoes)
            {
                botao.Visible = true;
            }

            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(btnExcluir, AcaoControle.excluir);

            if (btnEditar.Visible)
            {
                btnConfRenovMatricula.Visible = true;
            }
        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnEditar.Visible = false;
            btnExcluir.Visible = false;
            btnNovo.Visible = false;
            btnSalvar.Visible = false;
            //btnConfirmarFoto.Visible = false;
            btnFotografar.Visible = false;
        }
        #endregion

        #region Métodos da FOTO

        private void CarregaDadosFotoPessoa(LyFotoPessoa dadosFotoPessoa)
        {
            if (dadosFotoPessoa == null || dadosFotoPessoa.Foto == null)
            {
                bimgFotoPessoa.EmptyImage.Url = "~/Images/semfoto.jpg";
                bimgFotoPessoa.EmptyImage.AlternateText = "Sem foto";
                bimgFotoPessoa.ContentBytes = null;
            }
            else
            {
                try
                {
                    using (MemoryStream m = new MemoryStream(dadosFotoPessoa.Foto))
                    {
                        //image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(dadosFotoPessoa.Foto);
                        //Convert base64String to Image
                        Image.FromStream(new MemoryStream(Convert.FromBase64String(base64String)));
                    }

                    // Tenta carregar array de bytes em objeto Image. 
                    // Em caso de exceção, a foto está em formato inválido
                    Image.FromStream(new MemoryStream(dadosFotoPessoa.Foto));
                    bimgFotoPessoa.ContentBytes = dadosFotoPessoa.Foto;
                }
                catch (Exception e)
                {
                    bimgFotoPessoa.EmptyImage.Url = "~/Images/fotoinvalida.jpg";
                    bimgFotoPessoa.EmptyImage.AlternateText = "Foto inválida";
                    bimgFotoPessoa.ContentBytes = null;
                }
            }
        }

        private string AlteraFoto()
        {
            if (flFoto.PostedFile == null
                || flFoto.PostedFile.FileName == string.Empty)
            {

                if (_tipoOperacao == TipoOperacao.Alterar
                    && _alterouFoto)
                {
                    if (FotoPessoa.ExisteFoto(Convert.ToDecimal(txtPessoa.Text))
                        && bimgFotoPessoa.ContentBytes == null)
                    {
                        return "Remove";
                    }
                }

                return "Nenhuma";
            }

            return "Salva";
        }

        private string ObterDadosFotoPessoa(LyFotoPessoa dadosFotoPessoa)
        {
            if (flFoto.PostedFile == null
                || flFoto.PostedFile.FileName == string.Empty)
            {
                if (bimgFotoPessoa.ContentBytes != null)
                {
                    //Verifica tamanho da foto. As fotos precisam ser quadradas, com no máximo 400px de largura.
                    RN.Util.Imagem rnImagem = new RN.Util.Imagem();
                    var ms = new MemoryStream(bimgFotoPessoa.ContentBytes);
                    var imagem = System.Drawing.Image.FromStream(ms);
                    int width = imagem.Width;
                    int height = imagem.Height;
                    int tamanhoByte = System.Buffer.ByteLength(bimgFotoPessoa.ContentBytes);

                    if (width != height || width > 400) //Verifica dimensoes imagem
                    {
                        bimgFotoPessoa.ContentBytes = rnImagem.RedimencionaImagemPor(bimgFotoPessoa.ContentBytes, 240, 240);
                    }

                    if (tamanhoByte < 8192 || tamanhoByte > 32768) //Verifica tamanho arquivo
                    {
                        bimgFotoPessoa.ContentBytes = rnImagem.ComprimiImagemPor(bimgFotoPessoa.ContentBytes, 8192, 32000);
                    }

                    var msg = FotoPessoa.ValidaFoto(bimgFotoPessoa.ContentBytes);
                    if (msg == string.Empty)
                    {
                        dadosFotoPessoa.Foto = bimgFotoPessoa.ContentBytes;
                    }
                    else
                    {
                        return msg;
                    }
                }
                else
                {
                    byte[] imagemVazia = { 0 };
                    dadosFotoPessoa.Foto = null;
                }
            }
            else
            {
                var fotoPessoa = new byte[flFoto.PostedFile.InputStream.Length];

                flFoto.PostedFile.InputStream.Read(fotoPessoa, 0, fotoPessoa.Length);

                var msg = FotoPessoa.ValidaFoto(fotoPessoa);

                if (msg == string.Empty)
                {
                    dadosFotoPessoa.Foto = fotoPessoa;
                }
                else
                {
                    return msg;
                }
            }

            dadosFotoPessoa.Pessoa = Convert.ToDecimal(txtPessoa.Text);

            return string.Empty;
        }
        #endregion

        #region Métodos de Obter e Carregar Dados do Aluno e da Pessoa

        private void CarregaDadosAluno(LyAluno dadosAluno)
        {
            _mudouCurso = false;

            ViewState["confirmaalteracao"] = null;
            CarregaUnidadeEnsino();

            txtAluno.Text = dadosAluno.Aluno;

            if (dadosAluno.SitAluno == "Ativo" && dadosAluno.Suspenso)
            {
                txtSituacao.Text = "Matrícula em Suspensão";

            }
            else
            {
                txtSituacao.Text = dadosAluno.SitAluno;
            }

            txtNumInscricao.Text = dadosAluno.Numinscricao;
            hddDataAlteracaoEmail.Value = dadosAluno.DataAtualizacaoEmailInterno.HasValue ? dadosAluno.DataAtualizacaoEmailInterno.Value.ToString() : string.Empty;
            txtDataAtualizacaoEmail.Text = hddDataAlteracaoEmail.Value;
            txtDataAtualizacaoEmailTransp.Text = hddDataAlteracaoEmail.Value;

            var qt = RN.EncerramentoAluno.ConsultarCausaEncerramento(dadosAluno.Aluno);

            if (qt.Rows.Count > 0)
            {
                txtCausaEncerramento.Text = Convert.ToString(qt.Rows[0]["causa_encerramento"]);
            }
            else
            {
                txtCausaEncerramento.Text = string.Empty;
            }

            var qt2 = RN.EncerramentoAluno.ConsultarMotivoEncerramento(dadosAluno.Aluno);

            if (qt2.Rows.Count > 0)
            {
                txtMotivo.Text = Convert.ToString(qt2.Rows[0]["MOTIVOSAIDA"]);
            }
            else
            {
                txtMotivo.Text = string.Empty;
            }

            if (!string.IsNullOrEmpty(dadosAluno.UnidadeEnsino))
            {
                tseUnidadeEns.DBValue = dadosAluno.UnidadeEnsino;

                //Verifica se a escola do aluno esta na lista de escola disponiveis para o usuario logado
                if (!tseUnidadeEns.IsValidDBValue)
                {
                    //Se não estiver carrega a lista com a escola do aluno
                    CarregaUnidadeEnsinoPor(dadosAluno.UnidadeEnsino);
                }
            }
            else
            {
                tseUnidadeEns.ResetValue();
            }

            cmbTurno.SelectedValue = dadosAluno.Turno;

            ddlRedeEnsinoOrigem.SelectedValue = !dadosAluno.RedeEnsinoOrigem.IsNullOrEmptyOrWhiteSpace() ? System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dadosAluno.RedeEnsinoOrigem.ToLower()) : string.Empty;

            if (ddlRedeEnsinoOrigem.SelectedValue == "Afastado")
            {
                txtTempoAfastamento.Text = dadosAluno.TempoAfastamentoRede.ToString();
                txtTempoAfastamento.Visible = true;
                lblTempoAfastamento.Visible = true;
            }
            else
            {
                txtTempoAfastamento.Visible = false;
                lblTempoAfastamento.Visible = false;
            }

            if (!string.IsNullOrEmpty(dadosAluno.DtCadastro.ToString()))
            {
                txtDataCadastro.Text = dadosAluno.DtCadastro.ToString();
            }

            if (!string.IsNullOrEmpty(dadosAluno.Curso))
            {
                tseCurso.SqlWhere = null;
                tseCurso.DataBind();
                tseCurso.DBValue = dadosAluno.Curso;
                PreencherDadoCombo(ddlModalidade, Curso.ConsultaModalidade(dadosAluno.Curso));
                PreencherDadoCombo(ddlNivel, Curso.ConsultaNivel(dadosAluno.Curso));

                if (!string.IsNullOrEmpty(dadosAluno.TipoEnsinoProfissionalizante))
                {
                    ddlTipoCurso.SelectedValue = dadosAluno.TipoEnsinoProfissionalizante;
                    ddlTipoCurso.Visible = true;
                    lblTipoCurso.Visible = true;
                }
                else
                {
                    string tipo_curso = Curso.ConsultarTipoProfCurso(Convert.ToString(dadosAluno.Curso));

                    if (string.IsNullOrEmpty(tipo_curso)
                        || tipo_curso == "Especial")
                    {
                        lblTipoCurso.Visible = false;
                        ddlTipoCurso.Visible = false;
                    }
                    else
                    {
                        ddlTipoCurso.Visible = true;
                        lblTipoCurso.Visible = true;
                    }
                }
            }
            else
            {
                tseCurso.ResetValue();
            }

            CarregaDadosDeclaracao(dadosAluno.Aluno);
            cmbCurriculo.DataBind();
            PreencherDadoCombo(cmbCurriculo, Convert.ToString(dadosAluno.Curriculo));
            CarregaSerie();
            PreencherDadoCombo(cmbSerie, Convert.ToString(dadosAluno.Serie));
            PreencherDadoCombo(cmbAnoIngresso, Convert.ToString(dadosAluno.AnoIngresso));
            PreencherDadoCombo(cmbSemIngresso, Convert.ToString(dadosAluno.SemIngresso));
            PreencherDadoCombo(ddlTipoIngresso, dadosAluno.TipoIngresso);

            // ── Carga de Pessoa (Ly_Pessoa) para naturalidade e nacionalidade ──────
            try
            {
                RN.Pessoa rnPessoa = new Pessoa();
                RN.DTOs.DadosAlunoCertificacao dadosPessoa = new Techne.Lyceum.RN.DTOs.DadosAlunoCertificacao();

                dadosPessoa = rnPessoa.ObtemDadosCertificacaoPor(Convert.ToDecimal(dadosAluno.Pessoa));

                if (dadosPessoa != null)
                {
                    // Carga de Nacionalidade
                    ddlNacionalidade.SelectedValue = !dadosPessoa.Nacionalidade.IsNullOrEmptyOrWhiteSpace()
                        ? dadosPessoa.Nacionalidade
                        : string.Empty;

                    // Carga de naturalidade (Brasil ou exterior)
                    tseNaturalidadeEstrangeira.Enabled = true;

                    if (!dadosPessoa.MunicipioNascimento.IsNullOrEmptyOrWhiteSpace())
                    {
                        tseNaturalidade.DBValue = dadosPessoa.MunicipioNascimento;

                        if (tseNaturalidade.IsValidDBValue && !tseNaturalidade.DBValue.IsNull)
                        {
                            // Nascido no Brasil
                            txtUFNascimento.Text = tseNaturalidade["uf_sigla"].ToString();
                        }
                        else
                        {
                            // Nascido fora do Brasil
                            tseNaturalidade.ResetValue();

                            tseNaturalidadeEstrangeira.DBValue = dadosPessoa.MunicipioNascimento;
                            if (tseNaturalidadeEstrangeira.IsValidDBValue && !tseNaturalidadeEstrangeira.DBValue.IsNull)
                            {
                                txtUFNascimento.Text = tseNaturalidadeEstrangeira["ESTADO"].ToString();
                                txtPaisNasc.Text = tseNaturalidadeEstrangeira["PAIS"].ToString();
                            }
                        }
                    }
                    else
                    {
                        tseNaturalidade.ResetValue();
                        tseNaturalidadeEstrangeira.ResetValue();
                        txtUFNascimento.Text = string.Empty;
                        txtPaisNasc.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = "Erro ao carregar naturalidade: " + ex.Message;
            }

        }

        private void CarregaUnidadeEnsinoPor(string unidadeEnsino)
        {
            //Altera listagem da tseUnidadeEns
            string table = string.Empty;
            Techne.Library.Sql.Structure.SqlSelectColumns coluna = new Techne.Library.Sql.Structure.SqlSelectColumns();
            Techne.Library.Sql.Structure.SqlSelect sqlSelect;

            //Esta view lista todos as escolas estaduais sem considerar permissoes usuario
            table = " VW_UNIDADE_ENSINO_ESTADUAL ";

            coluna.Add("unidade_ens");
            coluna.Add("nome_comp");
            coluna.Add("setor");
            coluna.Add("cgc");
            coluna.Add("situacao");
            coluna.Add("municipio");
            coluna.Add("ua_atual");
            coluna.Add("ua_antiga");

            sqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);

            tseUnidadeEns.SqlSelect = sqlSelect;
            tseUnidadeEns.SqlWhere = string.Format(" unidade_ens = '{0}' ", unidadeEnsino);
            tseUnidadeEns.DataBind();
            tseUnidadeEns.DBValue = unidadeEnsino;
        }

        private void CarregaUnidadeEnsino()
        {
            //Cria listagem da tseUnidadeEns
            string table = string.Empty;
            Techne.Library.Sql.Structure.SqlSelectColumns coluna = new Techne.Library.Sql.Structure.SqlSelectColumns();
            Techne.Library.Sql.Structure.SqlSelect sqlSelect;

            //Esta view lista todos as escolas considerando as permissoes usuario
            table = " VW_UNIDADE_ENSINO_SITUACAO ";

            coluna.Add("unidade_ens");
            coluna.Add("nome_comp");
            coluna.Add("setor");
            coluna.Add("cgc");
            coluna.Add("situacao");
            coluna.Add("municipio");
            coluna.Add("ua_atual");
            coluna.Add("ua_antiga");

            sqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);

            tseUnidadeEns.SqlSelect = sqlSelect;
            tseUnidadeEns.SqlWhere = string.Empty;
            tseUnidadeEns.DataBind();
        }

        private void CarregaUnidadeEnsinoMatricula()
        {
            tseUnidadeEnsinoMatricula.ResetValue();
            //Cria listagem da tseUnidadeEnsinoMatricula
            string table = string.Empty;
            Techne.Library.Sql.Structure.SqlSelectColumns coluna = new Techne.Library.Sql.Structure.SqlSelectColumns();
            Techne.Library.Sql.Structure.SqlSelect sqlSelect;

            //Esta view lista todos as escolas considerando as permissoes usuario
            table = " VW_UNIDADE_ENSINO_SITUACAO ";

            coluna.Add("unidade_ens");
            coluna.Add("nome_comp");
            coluna.Add("setor");
            coluna.Add("cgc");
            coluna.Add("situacao");
            coluna.Add("municipio");
            coluna.Add("ua_atual");
            coluna.Add("ua_antiga");

            sqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);

            tseUnidadeEnsinoMatricula.SqlSelect = sqlSelect;
            tseUnidadeEnsinoMatricula.SqlWhere = string.Empty;
            tseUnidadeEnsinoMatricula.DataBind();
        }

        private void CarregaDadosPessoa(LyPessoa dadosPessoa)
        {
            int idade = 0;
            string dataLimite = "31/01/" + (DateTime.Now.Year + 1);

            txtPessoa.Text = dadosPessoa.Pessoa > 0 ? Convert.ToString(dadosPessoa.Pessoa) : string.Empty;
            txtNomeCompl.Text = !dadosPessoa.Nome_compl.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.Nome_compl : string.Empty;

            if (dadosPessoa.Dt_nasc.HasValue)
            {
                dtDataNasc.Date = dadosPessoa.Dt_nasc.Value;
                idade = Utils.CalcularIdadePorData(dtDataNasc.Date, Convert.ToDateTime(dataLimite));
                hdnIdade.Text = idade.ToString();
            }

            txtNomeSocial.Text = !dadosPessoa.PreNomeSocial.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.PreNomeSocial : string.Empty;

            if (!string.IsNullOrEmpty(dadosPessoa.Sexo))
            {
                if (rblSexo.Items.FindByValue(dadosPessoa.Sexo) != null)
                {
                    rblSexo.Text = dadosPessoa.Sexo;
                }
            }

            txtEndereco.Text = !dadosPessoa.Endereco.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.Endereco : string.Empty;
            txtEndNum.Text = !dadosPessoa.End_num.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.End_num : string.Empty;
            txtEndCompl.Text = !dadosPessoa.End_compl.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.End_compl : string.Empty;
            txtCep.Text = !dadosPessoa.Cep.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.Cep : string.Empty;
            txtBairro.Text = !dadosPessoa.Bairro.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.Bairro : string.Empty;
            txtLatitude.Text = !dadosPessoa.Latitude.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.Latitude : string.Empty;
            txtLongitude.Text = !dadosPessoa.Longitude.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.Longitude : string.Empty;
            txtEmail.Text = !dadosPessoa.E_mail.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.E_mail : string.Empty;
            txtEmailTransp.Text = !dadosPessoa.E_mail.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.E_mail : string.Empty;
            hddTxtEmail.Value = !dadosPessoa.E_mail.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.E_mail : string.Empty;
            txtRGNum.Text = !dadosPessoa.Rg_num.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.Rg_num : string.Empty;
            txtFilhos.Text = dadosPessoa.QtFilhos.HasValue ? Convert.ToString(dadosPessoa.QtFilhos) : string.Empty;
            txtDOC_CertNasc_Numero.Text = !dadosPessoa.CertNascNum.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.CertNascNum : string.Empty;
            txtDOC_CertNasc_Folha.Text = !dadosPessoa.CertNascFolha.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.CertNascFolha : string.Empty;
            txtDOC_CertNasc_Livro.Text = !dadosPessoa.CertNascLivro.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.CertNascLivro : string.Empty;
            txtNumMatriculaCertidao.Text = !dadosPessoa.CertNumeroMatricula.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.CertNumeroMatricula : string.Empty;

            if (dadosPessoa.CertNascEmissao.HasValue)
            {
                dboDOC_CertNasc_DtEmissao.Date = dadosPessoa.CertNascEmissao.Value;
            }

            PreencherDadoCombo(ddlUFCartorio, Convert.ToString(dadosPessoa.CodigoUf));
            if (!string.IsNullOrEmpty(dadosPessoa.CodigoUf))
            {
                this.CarregaMunicipioCartorio();
                PreencherDadoCombo(ddlMunicipioCartorio, Convert.ToString(dadosPessoa.CodigoMunicipio));
            }

            if (dadosPessoa.IdCartorio != null && dadosPessoa.IdCartorio != 0)
            {
                CarregaCartorio();
                PreencherDadoCombo(ddlCartorio, Convert.ToString(dadosPessoa.IdCartorio));
            }

            txtIDCenso.Text = !dadosPessoa.Id_censo.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.Id_censo : string.Empty;

            if (!string.IsNullOrEmpty(dadosPessoa.Pais_nasc))
            {
                //string descricaoPais = RN.Endereco.ObterPais(dadosPessoa.Pais_nasc);
                string descricaoPais = txtPaisNasc.Text;

                if (descricaoPais.ToUpper() != "BRASIL")
                {
                    // Aluno nascido fora do Brasil
                    txtPaisNasc.Text = !string.IsNullOrEmpty(dadosPessoa.Pais_nasc)
                    ? RN.Endereco.ObterPaisEstrangeiro(dadosPessoa.Pais_nasc)
                    : string.Empty;

                    tseNaturalidade.ResetValue();

                    if (!string.IsNullOrEmpty(dadosPessoa.Municipio_nasc))
                    {
                        SimpleRow sr = Endereco.ObterMunicipioEstrangeiroHades(dadosPessoa.Municipio_nasc);

                        if (sr != null && !sr["nome"].IsNull)
                        {
                            tseNaturalidadeEstrangeira.DBValue = dadosPessoa.Municipio_nasc;

                            if (tseNaturalidadeEstrangeira.IsValidDBValue && !tseNaturalidadeEstrangeira.DBValue.IsNull)
                            {
                                txtUFNascimento.Text = tseNaturalidadeEstrangeira["ESTADO"].ToString();
                                txtPaisNasc.Text = tseNaturalidadeEstrangeira["PAIS"].ToString();
                            }
                        }
                        else
                        {
                            // Município estrangeiro não encontrado na tabela — habilita para o usuário corrigir
                            tseNaturalidadeEstrangeira.Enabled = true;
                        }
                    }
                    else
                    {
                        // Aluno brasileiro
                        tseNaturalidadeEstrangeira.ResetValue();

                        if (!string.IsNullOrEmpty(dadosPessoa.Municipio_nasc))
                        {
                            tseNaturalidade.DBValue = dadosPessoa.Municipio_nasc;

                            if (tseNaturalidade.IsValidDBValue && !tseNaturalidade.DBValue.IsNull)
                            {
                                txtUFNascimento.Text = tseNaturalidade["uf_sigla"].ToString();
                            }
                            else
                            {
                                tseNaturalidade.ResetValue();
                            }
                        }
                    }
                }

                
            }

            // verifica se existe valor para municipio
            if (!string.IsNullOrEmpty(dadosPessoa.End_municipio))
            {
                // preenche os dados nos controles da tela
                hdnCodMunicipio.Value = dadosPessoa.End_municipio;
                txtMunicipio.Text = dadosPessoa.NomeMunicipio;

                // obtém a UF de acordo com o codigo do municipío
                txtEstado.Value = Endereco.ObterUFMunicipio(dadosPessoa.End_municipio);
            }
            else
            {
                hdnCodMunicipio.Value = string.Empty;
                txtMunicipio.Text = string.Empty;
                txtEstado.Value = string.Empty;
            }

            txtPaisNasc.Text = !string.IsNullOrEmpty(dadosPessoa.Pais_nasc) ? RN.Endereco.ObterPaisEstrangeiro(dadosPessoa.Pais_nasc) : string.Empty;
            //PreencherDadoCombo(ddlNacionalidade, Convert.ToString(dadosPessoa.Nacionalidade));
            //PreencherDadoCombo(ddlEst_Civil, Convert.ToString(dadosPessoa.Est_civil));

            if (!string.IsNullOrEmpty(dadosPessoa.Est_civil))
            {
                if (ddlEst_Civil.Items.FindByValue(dadosPessoa.Est_civil) != null)
                {
                    ddlEst_Civil.SelectedValue = dadosPessoa.Est_civil;
                }
            }

            if (!string.IsNullOrEmpty(dadosPessoa.Nacionalidade))
            {
                if (ddlNacionalidade.Items.FindByValue(dadosPessoa.Nacionalidade) != null)
                {
                    ddlNacionalidade.SelectedValue = dadosPessoa.Nacionalidade;
                }
            }


            PreencherDadoCombo(cmbNecessidadeEspecial, Convert.ToString(dadosPessoa.NecessidadeEspecialId));
            VerificaNecessidadeEspecial();

            PreencherDadoCombo(ddlTipoSanguineo, Convert.ToString(dadosPessoa.Tipo_Sanguineo));
            PreencherDadoCombo(ddlEtnia, Convert.ToString(dadosPessoa.Etnia));
            ddlEtnia_SelectedIndexChanged(null, null);

            PreencherDadoCombo(ddlCredo, Convert.ToString(dadosPessoa.Credo));
            PreencherDadoCombo(cmbRGUF, Convert.ToString(dadosPessoa.Rg_uf));
            PreencherDadoCombo(cmbRGEmissor, Convert.ToString(dadosPessoa.Rg_emissor));
            PreencherDadoCombo(ddlRGTipoPessoa, Convert.ToString(dadosPessoa.Rg_tipo));
            PreencherDadoCombo(ddDOC_CertNasc_Uf, Convert.ToString(dadosPessoa.CertNascCartorioUf));

            this.ControlarObrigatoriedadeDocumentos(ddlRGTipoPessoa.Text);

            long result;

            if (long.TryParse(dadosPessoa.Cpf, out result))
            {
                if (result != 0)
                {
                    txtCPF.Text = string.Format(@"{0:000\.000\.000-00}", result);
                }
                else
                {
                    txtCPF.Text = string.Empty;
                }
            }
            else
            {
                txtCPF.Text = dadosPessoa.Cpf;
            }

            long resultadoFixoCelular;
            var fixoCelular = dadosPessoa.Fone.RetirarMascaraTelefone();

            if (long.TryParse(fixoCelular, out resultadoFixoCelular))
            {
                if (fixoCelular.Length == 10)
                {
                    txtFone.Text = string.Format("{0:(00)0000-0000}", resultadoFixoCelular);
                }
                else if (fixoCelular.Length == 11)
                {
                    txtFone.Text = string.Format("{0:(00)00000-0000}", resultadoFixoCelular);
                }
                else
                {
                    txtFone.Text = resultadoFixoCelular.ToString();

                }
            }
            long resultado;
            var celular = dadosPessoa.Celular.RetirarMascaraTelefone();
            if (long.TryParse(celular, out resultado))
            {
                if (celular.Length == 10)
                {
                    txtCelular.Text = string.Format("{0:(00)0000-0000}", resultado);
                }
                else if (celular.Length == 11)
                {
                    txtCelular.Text = string.Format("{0:(00)00000-0000}", resultado);
                }
                else
                {
                    txtCelular.Text = resultado.ToString();
                }
            }


            if (dadosPessoa.Rg_dtexp.HasValue)
                dtDataExped.Date = dadosPessoa.Rg_dtexp.Value;

            txtNomeMae.Text = dadosPessoa.NomeMae;
            txtNomePai.Text = dadosPessoa.NomePai;


            if (!string.IsNullOrEmpty(dadosPessoa.Responsavel))
            {
                string[] tipo_resp = dadosPessoa.Responsavel.Split(';');
                foreach (String str in tipo_resp)
                {
                    if (!string.IsNullOrEmpty(str) && (rblResponsavel.Items.FindByValue(str) != null))
                    {
                        rblResponsavel.Items.FindByValue(str).Selected = true;
                    }
                }
            }

            if (!string.IsNullOrEmpty(dadosPessoa.RespNomeCompl))
            {
                txtTelefoneResp.Visible = true;
                txtCPFResponsavel.Visible = true;
                txtNomeResponsavel.Visible = true;
                lblTelefoneResponsavel.Visible = true;
                lblCPFResponsavel.Visible = true;
                lblNomeResponsavel.Visible = true;

                txtNomeResponsavel.Text = dadosPessoa.RespNomeCompl;
                if (Int64.TryParse(dadosPessoa.RespCpf, out result))
                {
                    if (result != 0)
                        txtCPFResponsavel.Text = string.Format(@"{0:000\.000\.000-00}", result);
                    else
                        txtCPFResponsavel.Text = "";
                }
                else
                    txtCPFResponsavel.Text = dadosPessoa.RespCpf;

                if (Int64.TryParse(dadosPessoa.RespFone, out result))
                    txtTelefoneResp.Text = string.Format("{0:(00)0000-0000}", result);
                else
                    txtTelefoneResp.Text = dadosPessoa.RespFone;

            }

            if (Int64.TryParse(dadosPessoa.MaeCpf, out result))
            {
                if (result != 0)
                    txtCPFMae.Text = string.Format(@"{0:000\.000\.000-00}", result);
                else
                    txtCPFMae.Text = "";
            }
            else
                txtCPFMae.Text = dadosPessoa.MaeCpf;

            if (Int64.TryParse(dadosPessoa.PaiCpf, out result))
            {
                if (result != 0)
                    txtCPFPai.Text = string.Format(@"{0:000\.000\.000-00}", result);
                else
                    txtCPFPai.Text = "";
            }
            else
                txtCPFPai.Text = dadosPessoa.PaiCpf;

            if (Int64.TryParse(dadosPessoa.MaeTelefone, out result))
                txtTelefoneMae.Text = string.Format("{0:(00)0000-0000}", result);
            else
                txtTelefoneMae.Text = dadosPessoa.MaeTelefone;

            if (Int64.TryParse(dadosPessoa.PaiTelefone, out result))
                txtTelefonePai.Text = string.Format("{0:(00)0000-0000}", result);
            else
                txtTelefonePai.Text = dadosPessoa.PaiTelefone;

            chkFalecidaMae.Checked = dadosPessoa.MaeFalecida == "S";

            chkNaoDeclarMae.Checked = dadosPessoa.NomeMae == chkNaoDeclarMae.Text.ToUpper();


            if (chkFalecidaMae.Checked || chkNaoDeclarMae.Checked)
                DesabilitaResponsavelLegal("H", "Mãe");

            if (chkNaoDeclarMae.Checked)
            {
                txtCPFMae.Text = string.Empty;
                txtTelefoneMae.Text = string.Empty;
                chkFalecidaMae.Checked = false;
                txtNomeMae.ReadOnly = true;
                txtCPFMae.Enabled = false;
                txtTelefoneMae.Enabled = false;
                chkFalecidaMae.Enabled = false;
            }
            chkNaoDeclarPai.Checked = dadosPessoa.NomePai == chkNaoDeclarPai.Text.ToUpper();


            if (chkFalecidoPai.Checked || chkNaoDeclarPai.Checked)
                DesabilitaResponsavelLegal("H", "Pai");

            if (chkNaoDeclarPai.Checked)
            {
                txtCPFPai.Text = string.Empty;
                txtTelefonePai.Text = string.Empty;
                txtNomePai.ReadOnly = true;
                txtCPFPai.Enabled = false;
                txtTelefonePai.Enabled = false;
                chkFalecidoPai.Checked = false;
            }

            chkAreaAssentamento.Checked = !dadosPessoa.AreaAssentamento.IsNullOrEmptyOrWhiteSpace() ? (dadosPessoa.AreaAssentamento == "S" ? true : false) : false;
            chkTerraIndigena.Checked = !dadosPessoa.TerraIndigena.IsNullOrEmptyOrWhiteSpace() ? (dadosPessoa.TerraIndigena == "S" ? true : false) : false;
            chkQuilombos.Checked = !dadosPessoa.AreaQuilombos.IsNullOrEmptyOrWhiteSpace() ? (dadosPessoa.AreaQuilombos == "S" ? true : false) : false;
            chkAreaTradicional.Checked = !dadosPessoa.AreaTradicional.IsNullOrEmptyOrWhiteSpace() ? (dadosPessoa.AreaTradicional == "S" ? true : false) : false;
            chkNaoSeAplica.Checked = (!chkAreaAssentamento.Checked && !chkTerraIndigena.Checked && !chkQuilombos.Checked && !chkAreaTradicional.Checked) ? true : false;


            string latStr = !dadosPessoa.Latitude.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.Latitude : "null";
            string lngStr = !dadosPessoa.Longitude.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.Longitude : "null";

            ScriptManager.RegisterStartupScript(
                this.Page,
                this.GetType(),
                "InicializarMapa_" + Guid.NewGuid().ToString(),
                string.Format("InicializarMapa({0}, {1});", latStr, lngStr),
                true
            );
        }

        private void CarregaDadosRecursoAplicacaoProva(DataTable dadosRecursosAplicacaoProva)
        {
            foreach (DataRow linha in dadosRecursosAplicacaoProva.Rows)
            {
                if (Convert.ToInt32(linha["EXCLUSIVO"]) == 1)
                {
                    rblRecursoAplicaProvaExclusivo.Items.FindByValue(linha["RECURSOAPLICACAOPROVAID"].ToString()).Selected = true;
                }
                else
                {
                    chkRecursoAplicacaoProva.Items.FindByValue(linha["RECURSOAPLICACAOPROVAID"].ToString()).Selected = true;
                }
            }

            if (rblRecursoAplicaProvaExclusivo.SelectedIndex == -1 && chkRecursoAplicacaoProva.SelectedIndex == -1)
            {
                chkNenhumRecursoAplicacaoProva.Checked = true;
            }
        }

        private void CarregaDadosDeclaracao(string aluno)
        {
            DataTable dadosDeclaracao = new DataTable();
            dadosDeclaracao = RN.DeclaracaoAusencia.Carregar(aluno);
            string textoDeclaroNecEspecial = string.Empty;
            chkDeclaroNecessidadeEspecial.Text = "Declaro estar de posse do Laudo Médico ou Parecer Pedagógico";

            if (dadosDeclaracao.Rows.Count > 0)
            {
                textoDeclaroNecEspecial = string.Empty;
                foreach (DataRow linha in dadosDeclaracao.Rows)
                {
                    if (Convert.ToInt32(linha["TIPODECLARACAOAUSENCIAID"]) == Convert.ToInt32(RN.DeclaracaoAusencia.TipoDeclaracaoAusenciaId.DeclaracaoAusenciaMae))
                    {
                        chkDeclaroAusenciaMae.Checked = true;
                        DateTime? datahora = Convert.ToDateTime(linha["DATACADASTRO"]);
                        chkDeclaroAusenciaMae.Text = chkDeclaroAusenciaMae.Text + " em " + datahora.Value.ToString("dd/MM/yyyy") + " às " + datahora.Value.ToString("HH:mm") + " por " + linha["NOME_USUARIO"];
                        chkDeclaroAusenciaMae.Visible = true;
                    }
                    if (Convert.ToInt32(linha["TIPODECLARACAOAUSENCIAID"]) == Convert.ToInt32(RN.DeclaracaoAusencia.TipoDeclaracaoAusenciaId.DeclaracaoAusenciaPai))
                    {
                        chkDeclaroAusenciaPai.Checked = true;
                        DateTime? datahora = Convert.ToDateTime(linha["DATACADASTRO"]);
                        chkDeclaroAusenciaPai.Text = chkDeclaroAusenciaPai.Text + " em " + datahora.Value.ToString("dd/MM/yyyy") + " às " + datahora.Value.ToString("HH:mm") + " por " + linha["NOME_USUARIO"];
                        chkDeclaroAusenciaPai.Visible = true;
                    }
                    if (Convert.ToInt32(linha["TIPODECLARACAOAUSENCIAID"]) == Convert.ToInt32(RN.DeclaracaoAusencia.TipoDeclaracaoAusenciaId.DeclaracaoCertidaoCivil))
                    {
                        chkDeclaroCertidaoCivil.Checked = true;
                        chkDeclaroCertidaoCivil.Visible = true;
                        DateTime? datahora = Convert.ToDateTime(linha["DATACADASTRO"]);
                        chkDeclaroCertidaoCivil.Text = chkDeclaroCertidaoCivil.Text + " em " + datahora.Value.ToString("dd/MM/yyyy") + " às " + datahora.Value.ToString("HH:mm") + " por " + linha["NOME_USUARIO"];
                        txtMotivoCertidaoCivil.Text = linha["MOTIVO"].ToString();
                        txtMotivoCertidaoCivil.Visible = true;
                        pnlTipoCertidaoCivil.Visible = true;
                    }
                    if (Convert.ToInt32(linha["TIPODECLARACAOAUSENCIAID"]) == Convert.ToInt32(RN.DeclaracaoAusencia.TipoDeclaracaoAusenciaId.DeclaracaoNecessidadeEspecial))
                    {
                        chkDeclaroNecessidadeEspecial.Checked = true;
                        chkDeclaroNecessidadeEspecial.Visible = true;
                        DateTime? datahora = Convert.ToDateTime(linha["DATACADASTRO"]);
                        textoDeclaroNecEspecial = " em " + datahora.Value.ToString("dd/MM/yyyy") + " às " + datahora.Value.ToString("HH:mm") + " por " + linha["NOME_USUARIO"];
                    }
                }

                chkDeclaroNecessidadeEspecial.Text = chkDeclaroNecessidadeEspecial.Text + textoDeclaroNecEspecial;
            }
        }

        private void CarregaDadosFieldPessoa(LyFlPessoa dadosFieldPessoa)
        {
            ddlLocalZona.ClearSelection();
            ddlTipoCertidao.ClearSelection();
            ddlOutroEnsino.ClearSelection();
            ddlGratuidade.ClearSelection();
            ddlPoderPublicoTransp.ClearSelection();
            ddlCertidaoCivil.ClearSelection();
            rblDescFamilia.ClearSelection();
            txtComplIdentidade.ReadOnly = true;
            txtComplIdentidade.Text = string.Empty;
            txtNIS.Text = string.Empty;
            pnlNovo.Visible = false;
            pnlAntigo.Visible = false;
            chkOnibus.ClearSelection();
            chkOnibus.Visible = false;
            lblOnibus.Visible = false;
            chkRodoviario.ClearSelection();
            chkRodoviario.Visible = false;
            lblRodoviario.Visible = false;
            chkAquaviario.ClearSelection();
            chkAquaviario.Visible = false;
            lblAquaviario.Visible = false;

            if (!dadosFieldPessoa.FlField01.IsNullOrEmptyOrWhiteSpace())
            {
                PreencherDadoCombo(ddlLocalZona, dadosFieldPessoa.FlField01);
            }
            if (!string.IsNullOrEmpty(dadosFieldPessoa.FlField02))
            {
                PreencherDadoCombo(ddlTipoCertidao, dadosFieldPessoa.FlField02);

                if (dadosFieldPessoa.FlField02 == "Nenhum")
                {
                    pnlTipoCertidaoCivil.Visible = true;
                }
            }

            if (!string.IsNullOrEmpty(dadosFieldPessoa.FlField03))
            {
                PreencherDadoCombo(ddlOutroEnsino, dadosFieldPessoa.FlField03);
            }

            if (!string.IsNullOrEmpty(dadosFieldPessoa.FlField04))
            {
                PreencherDadoCombo(ddlGratuidade, dadosFieldPessoa.FlField04);
            }

            if (!string.IsNullOrEmpty(dadosFieldPessoa.FlField05))
            {
                string[] modais = dadosFieldPessoa.FlField05.Split(';');
                foreach (String str in modais)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        chkModais.Items.FindByValue(str).Selected = true;
                    }
                }

                //if (string.Compare(dadosFieldPessoa.FlField05, "5", true) == 1)
                if (modais != null && modais.Contains("5"))
                {

                    chkRodoviario.ClearSelection();
                    chkRodoviario.Visible = true;
                    lblRodoviario.Visible = true;

                    chkAquaviario.ClearSelection();
                    chkAquaviario.Visible = true;
                    lblAquaviario.Visible = true;

                    if (!string.IsNullOrEmpty(dadosFieldPessoa.FlField11))
                    {
                        string[] rodov = dadosFieldPessoa.FlField11.Split(';');
                        foreach (String str in rodov)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                chkRodoviario.Items.FindByValue(str).Selected = true;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(dadosFieldPessoa.FlField12))
                    {
                        string[] aquav = dadosFieldPessoa.FlField12.Split(';');
                        foreach (String str in aquav)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                chkAquaviario.Items.FindByValue(str).Selected = true;
                            }
                        }
                    }
                }

                //if (string.Compare(dadosFieldPessoa.FlField05, "2", true) == 1)
                if (modais != null && modais.Contains("2"))
                {
                    chkOnibus.ClearSelection();
                    chkOnibus.Visible = true;
                    lblOnibus.Visible = true;

                    if (!string.IsNullOrEmpty(dadosFieldPessoa.FlField20))
                    {
                        string[] onibus = dadosFieldPessoa.FlField20.Split(';');
                        foreach (String str in onibus)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                chkOnibus.Items.FindByValue(str).Selected = true;
                            }
                        }
                    }
                }
            }

            if (ddlNacionalidade.SelectedItem.Text == "BRASILEIRA")
            {
                if (!string.IsNullOrEmpty(ddlRGTipoPessoa.SelectedValue))
                {
                    if (ddlRGTipoPessoa.SelectedValue == "RG")
                    {
                        if (!string.IsNullOrEmpty(dadosFieldPessoa.FlField07))
                        {
                            txtComplIdentidade.Text = dadosFieldPessoa.FlField07;
                            txtComplIdentidade.ReadOnly = false;
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(dadosFieldPessoa.FlField08))
            {
                txtNIS.Text = dadosFieldPessoa.FlField08;
            }

            if (!string.IsNullOrEmpty(dadosFieldPessoa.FlField09))
            {
                PreencherDadoCombo(ddlCertidaoCivil, dadosFieldPessoa.FlField09);

                if (ddlCertidaoCivil.SelectedValue == "Modelo Novo")
                {
                    pnlNovo.Visible = true;
                    pnlAntigo.Visible = false;
                }
                else if (ddlCertidaoCivil.SelectedValue == "Modelo Antigo")
                {
                    pnlNovo.Visible = false;
                    pnlAntigo.Visible = true;
                }
            }

            if (!string.IsNullOrEmpty(dadosFieldPessoa.FlField10))
            {
                PreencherDadoCombo(ddlPoderPublicoTransp, dadosFieldPessoa.FlField10);
            }

            if (ddlEtnia.SelectedValue == "Índigena")
            {
                ddlEtnia_SelectedIndexChanged(null, null);

                if (!string.IsNullOrEmpty(dadosFieldPessoa.FlField21))
                {
                    ddlPovoIndigena.SelectedValue = dadosFieldPessoa.FlField21;
                }
            }

            if (!string.IsNullOrEmpty(dadosFieldPessoa.FlField22))
            {
                rblPossuiTranstorno.SelectedValue = dadosFieldPessoa.FlField22;

                if (dadosFieldPessoa.FlField22 == "S")
                {
                    CarregaDadosTipoTransporto(dadosFieldPessoa.Pessoa);
                    pnlTipoTranstorno.Visible = true;
                }
            }

            if (!string.IsNullOrEmpty(dadosFieldPessoa.FlField23))
            {
                rblDescFamilia.SelectedValue = dadosFieldPessoa.FlField23;
            }
        }

        private void CarregarDadosAluno()
        {
            try
            {
                string aluno = string.Empty;

                if (!String.IsNullOrEmpty(Request.QueryString["Chave"]))
                {
                    byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                    string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                    aluno = decodedText.Substring(decodedText.LastIndexOf('=') + 1);
                }
                else if (!String.IsNullOrEmpty(Request.QueryString["Aluno"]))
                {
                    aluno = Request.QueryString["Aluno"];
                }
                else if (!String.IsNullOrEmpty(Request.QueryString["ChaveConfirmacao"]))
                {
                    byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["ChaveConfirmacao"]);
                    string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                    aluno = decodedText.Substring(decodedText.LastIndexOf('=') + 1);
                }

                tseAluno.DBValue = aluno;
                txtAluno.Text = aluno;
                _tipoOperacao = TipoOperacao.Consultar;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ValidarCampos()
        {
            txtNIS.Attributes.Add("onkeyPress", "return onlyNumbers(event, this);");
            txtNIS.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
            txtNIS.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");
        }

        private void CarregarGridDocumentos()
        {
            var aluno = string.IsNullOrEmpty(tseAluno.DBValue.ToString()) ? txtAluno.Text : tseAluno.DBValue.ToString();

            if (!string.IsNullOrEmpty(aluno))
            {
                grdDocumentos.DataSource = RN.DocumentoAluno.Listar(aluno, Convert.ToDecimal(cmbAnoIngresso.SelectedValue), Convert.ToDecimal(cmbSemIngresso.SelectedValue));
                grdDocumentos.DataBind();
            }
        }

        /// <summary>
        /// Método para carregar os grids de Renovação e Histórico de Confirmação de Matrícula
        /// </summary>
        private void CarregarGridConfirmacaoRenovacaoMatricula()
        {
            try
            {
                var aluno = string.IsNullOrEmpty(tseAluno.DBValue.ToString()) ? txtAluno.Text : tseAluno.DBValue.ToString();
                if (!string.IsNullOrEmpty(aluno))
                {
                    CarregaHistoricoConfirmacaoMatricula(aluno);
                    CarregaRenovacaoMatricula(aluno);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        /// <summary>
        /// Carrega a grid com os Históricos de Confirmação de Matrícula do Aluno
        /// </summary>
        /// <param name="anulo">Matrícula do Aluno</param>
        private void CarregaHistoricoConfirmacaoMatricula(string aluno)
        {
            grdConfirmacao.DataSource = RN.ConfirmacaoMatricula.Listar(User.Identity.Name, aluno);
            grdConfirmacao.DataBind();

            ddlConfirmacaoMatricula.DataBind();

            btnConfRenovMatricula.Visible = RN.ConfirmacaoMatricula.PossuiConfirmacaoEmAberto(aluno);

        }

        /// <summary>
        /// Carrega a grid com as Renovações de Matrícula do Aluno
        /// </summary>
        /// <param name="anulo">Matrícula do Aluno</param>
        private void CarregaRenovacaoMatricula(string aluno)
        {
            grdRenovacaoMatricula.DataSource = RN.RenovacaoMatricula.Renovacao.ListaRenovacaoPor(aluno);
            grdRenovacaoMatricula.DataBind();
        }

        private void CarregarGridProgramas()
        {
            var aluno = string.IsNullOrEmpty(tseAluno.DBValue.ToString()) ? txtAluno.Text : tseAluno.DBValue.ToString();

            if (string.IsNullOrEmpty(aluno))
            {
                lblMensagem.Text = "Para visualizar os programas sociais/especiais é necessário selecionar um aluno.";
                return;
            }
            else
            {
                grdProgramas.DataSource = RN.ProgramaSocial.Listar(aluno);
                grdProgramas.DataBind();
            }

        }

        protected bool VerificarCheck(object valor)
        {
            if (valor is DBNull)
            {
                return false;
            }

            if (valor is string)
            {
                return (string)valor == "1";
            }

            if (valor is bool)
            {
                return (bool)valor;
            }
            return false;
        }

        protected void grdDocumentos_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (!this.grdDocumentos.Visible
                || this.grdDocumentos.VisibleRowCount == 0
                )
            {
                return;
            }

            var colData = this.grdDocumentos.Columns["DT_ENTREGA"] as GridViewDataColumn;
            var colEntrega = (GridViewDataColumn)this.grdDocumentos.Columns["ENTREGA"];


            var txtDataEntrega = (TextBox)this.grdDocumentos.FindRowCellTemplateControl(e.VisibleIndex, colData, "txtDataEntrega");
            var chkEntrega = (CheckBox)this.grdDocumentos.FindRowCellTemplateControl(e.VisibleIndex, colEntrega, "chkEntrega");

            chkEntrega.InputAttributes.Add("entrega", txtDataEntrega.ClientID);

        }

        protected void btnSalvarDoc_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtAluno.Text))
                {
                    lblMensagem.Text = "Para incluir um documento é necessário selecionar um Aluno.";
                    return;
                }

                var entregas = new List<TceDocumentoAluno>();

                for (var rowIndex = 0; rowIndex < this.grdDocumentos.VisibleRowCount; rowIndex++)
                {

                    var chkEntrega = DevExpressHelper.GetControl<CheckBox>(this.grdDocumentos, rowIndex, "ENTREGA",
                                                                              "chkEntrega");

                    if (chkEntrega.Checked)
                    {
                        var id = (int)this.grdDocumentos.GetRowValues(rowIndex, "ID_DOCUMENTO");
                        var txtDataEntrega = DevExpressHelper.GetControl<TextBox>(grdDocumentos, rowIndex, "DT_ENTREGA",
                                                                                  "txtDataEntrega");

                        if (string.IsNullOrEmpty(txtDataEntrega.Text))
                        {
                            lblMensagem.Text = "O campo Data da Entrega é de preenchimento obrigatório para os documentos entregues.";
                            return;
                        }
                        DateTime data;
                        if (!DateTime.TryParse(txtDataEntrega.Text, out data))
                        {
                            lblMensagem.Text = "Data da Entrega inválida.";
                            return;
                        }
                        if (Convert.ToDateTime(txtDataEntrega.Text) > DateTime.Now)
                        {
                            lblMensagem.Text = "A Data da Entrega não pode ser maior que a data de hoje.";
                            return;
                        }
                        if (Convert.ToDateTime(txtDataEntrega.Text) < dtDataNasc.Date)
                        {
                            lblMensagem.Text = "A Data da Entrega não pode ser menor que a Data de Nascimento.";
                            return;
                        }
                        var entrega = new TceDocumentoAluno
                        {
                            IdDocumento = id,
                            Aluno = txtAluno.Text.Trim(),
                            DtEntrega = Convert.ToDateTime(txtDataEntrega.Text),
                            Matricula = User.Identity.Name
                        };

                        entregas.Add(entrega);

                    }
                }

                DocumentoAluno.SalvarDocumentos(entregas, txtAluno.Text);

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Documentos incluído com sucesso.');", true);

                CarregarGridDocumentos();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdConfirmacao_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            var colEnsinoReligioso = (GridViewDataColumn)this.grdConfirmacao.Columns["ENSINO_RELIGIOSO"];
            var colLinguaEstr = (GridViewDataColumn)this.grdConfirmacao.Columns["LINGUA_ESTRANGEIRA_FACULTATIVA"];
            var colProjAut = (GridViewDataColumn)this.grdConfirmacao.Columns["PROJETO_AUTONOMIA"];
            var modalidade = (string)grdConfirmacao.GetRowValues(e.VisibleIndex, "MODALIDADE");
            var curso = (string)grdConfirmacao.GetRowValues(e.VisibleIndex, "CURSO");
            var serie = grdConfirmacao.GetRowValues(e.VisibleIndex, "SERIE") != null ? grdConfirmacao.GetRowValues(e.VisibleIndex, "SERIE").ToString() : string.Empty;
            var podeEnsReligioso = (string)grdConfirmacao.GetRowValues(e.VisibleIndex, "PODE_ENSINO_RELIGIOSO");
            var podeLinguaEstran = (string)grdConfirmacao.GetRowValues(e.VisibleIndex, "PODE_LINGUA_ESTRANGEIRA");
            var cadastrou = (string)grdConfirmacao.GetRowValues(e.VisibleIndex, "CADASTROU");

            var chkEnsinoReligioso = (CheckBox)this.grdConfirmacao.FindRowCellTemplateControl(e.VisibleIndex, colEnsinoReligioso, "chkEnsinoReligioso");
            var chkLinguaEstrangeira = (CheckBox)this.grdConfirmacao.FindRowCellTemplateControl(e.VisibleIndex, colLinguaEstr, "chkLinguaEstrangeira");
            var chkProjetoAutonomia = (CheckBox)this.grdConfirmacao.FindRowCellTemplateControl(e.VisibleIndex, colProjAut, "chkProjetoAutonomia");

            var rbConfirmado = DevExpressHelper.GetControl<RadioButton>(this.grdConfirmacao, e.VisibleIndex, "STATUS", "rbConfirmado");
            var rbNaoConfirmado = DevExpressHelper.GetControl<RadioButton>(this.grdConfirmacao, e.VisibleIndex, "STATUS", "rbNaoConfirmado");
            var status = grdConfirmacao.GetRowValues(e.VisibleIndex, "STATUS");

            if (rbConfirmado == null
               || rbNaoConfirmado == null)
            {
                return;
            }

            if (status.ToString() == ConfirmacaoMatricula.Confirmado)
            {
                rbConfirmado.Checked = true;
                rbNaoConfirmado.Checked = false;
            }

            if (status.ToString() == ConfirmacaoMatricula.NaoConfirmado)
            {
                rbConfirmado.Checked = false;
                rbNaoConfirmado.Checked = true;
            }

            if (podeLinguaEstran == "S")
            {
                chkLinguaEstrangeira.Enabled = true;
            }
            else
            {
                chkLinguaEstrangeira.Enabled = false;
            }

            if (podeEnsReligioso == "S")
            {
                chkEnsinoReligioso.Enabled = true;
            }
            else
            {
                chkEnsinoReligioso.Enabled = false;
            }

            var proj = false;
            var idade = Utils.CalcularIdade(dtDataNasc.Date);

            if ((idade >= 17 && idade <= 20)
                && ((serie == "1") || (serie == "2"))
                && (curso.Contains("MÉDIO"))
                && ((modalidade == "ED2") || (modalidade == "RE1")))
            {
                proj = true;
            }

            if ((idade >= 15 && idade <= 18)
                && ((serie == "6") || (serie == "7"))
                && (curso.Contains("FUNDAMENTAL"))
                && ((modalidade == "ED2") || (modalidade == "RE1")))
            {
                proj = true;
            }

            chkProjetoAutonomia.Enabled = proj;

            if (cadastrou == "N")
            {
                rbConfirmado.Enabled = true;
                rbNaoConfirmado.Enabled = true;
            }
            else
            {
                rbConfirmado.Enabled = false;
                rbNaoConfirmado.Enabled = false;
                chkProjetoAutonomia.Enabled = false;
                chkEnsinoReligioso.Enabled = false;
                chkLinguaEstrangeira.Enabled = false;
            }
        }

        protected void grdRenovacaoMatricula_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            var aluno = string.IsNullOrEmpty(tseAluno.DBValue.ToString()) ? txtAluno.Text : tseAluno.DBValue.ToString();
            if (!string.IsNullOrEmpty(aluno))
            {
                CarregaRenovacaoMatricula(aluno);
            }
        }

        protected void btnConfRenovMatricula_Click(object sender, EventArgs e)
        {
            /* variavel nova que sera utilizada no update da tabela TCE_CONFIRMACAO_MATRICULA */

            var dtConfirmacao = new TceConfirmacaoMatricula();
            RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();

            try
            {
                if (string.IsNullOrEmpty(txtAluno.Text))
                {
                    lblMensagem.Text = "Para incluir um documento é necessário selecionar um Aluno.";
                    return;
                }

                /*  inicio da nova rotina que checa se mais de uma opcao foi confirmada pelo usuario */

                int contConf = 0;

                for (var rowIndex = 0; rowIndex < this.grdConfirmacao.VisibleRowCount; rowIndex++)
                {
                    var rbConfirmado = DevExpressHelper.GetControl<RadioButton>(this.grdConfirmacao, rowIndex, "STATUS", "rbConfirmado");

                    if (rbConfirmado != null && rbConfirmado.Checked && rbConfirmado.Enabled)
                    {
                        contConf += 1;
                    }
                }

                if (contConf >= 2)
                {
                    var script = @"alert('Erro. Não é permitido confirmar mais de uma confirmação. P)or favor, selecione apenas uma.');";

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);

                    return;
                }

                /*  fim da nova rotina que checa se mais de uma opcao foi confirmada pelo usuario */

                var confs = new List<TceConfirmacaoMatricula>();

                for (var rowIndex = 0; rowIndex < this.grdConfirmacao.VisibleRowCount; rowIndex++)
                {
                    var chkEnsinoReligioso = DevExpressHelper.GetControl<CheckBox>(this.grdConfirmacao, rowIndex, "ENSINO_RELIGIOSO", "chkEnsinoReligioso");
                    var chkLinguaEstrangeira = DevExpressHelper.GetControl<CheckBox>(this.grdConfirmacao, rowIndex, "LINGUA_ESTRANGEIRA_FACULTATIVA", "chkLinguaEstrangeira");
                    var chkProjetoAutonomia = DevExpressHelper.GetControl<CheckBox>(this.grdConfirmacao, rowIndex, "PROJETO_AUTONOMIA", "chkProjetoAutonomia");
                    var rbConfirmado = DevExpressHelper.GetControl<RadioButton>(this.grdConfirmacao, rowIndex, "STATUS", "rbConfirmado");
                    var rbNaoConfirmado = DevExpressHelper.GetControl<RadioButton>(this.grdConfirmacao, rowIndex, "STATUS", "rbNaoConfirmado");
                    var id = (int)this.grdConfirmacao.GetRowValues(rowIndex, "ID_CONFIRMACAO_MATRICULA");
                    var cadastrou = (string)grdConfirmacao.GetRowValues(rowIndex, "CADASTROU");
                    var censo = (string)grdConfirmacao.GetRowValues(rowIndex, "CENSO");
                    var curso = (string)grdConfirmacao.GetRowValues(rowIndex, "COD_CURSO");
                    var serie = (decimal)grdConfirmacao.GetRowValues(rowIndex, "SERIE");
                    var turno = (string)grdConfirmacao.GetRowValues(rowIndex, "TURNO");
                    var curriculo = (string)grdConfirmacao.GetRowValues(rowIndex, "CURRICULO");
                    var ano = (decimal)grdConfirmacao.GetRowValues(rowIndex, "ANO");
                    var periodo = (decimal)grdConfirmacao.GetRowValues(rowIndex, "PERIODO");
                    var tipoVagaOcupada = (string)grdConfirmacao.GetRowValues(rowIndex, "TIPOVAGAOCUPADA");

                    if (cadastrou == "N")
                    {
                        if ((rbConfirmado.Checked || rbNaoConfirmado.Checked) && rbConfirmado.Enabled)
                        {
                            var conf = new TceConfirmacaoMatricula
                            {
                                IdConfirmacaoMatricula = id,
                                Aluno = string.IsNullOrEmpty(tseAluno.DBValue.ToString()) ? txtAluno.Text : tseAluno.DBValue.ToString(),
                                EnsinoReligioso = chkEnsinoReligioso.Checked,
                                LinguaEstrangeiraFacultativa = chkLinguaEstrangeira.Checked,
                                ProjetoAutonomia = chkProjetoAutonomia.Checked,
                                Status = rbConfirmado.Checked ? ConfirmacaoMatricula.Confirmado : ConfirmacaoMatricula.NaoConfirmado,
                                Censo = censo,
                                Turno = turno,
                                Curso = curso,
                                Serie = Convert.ToDecimal(serie),
                                Matricula = User.Identity.Name,
                                Ano = ano,
                                Periodo = periodo,
                                Curriculo = curriculo,
                                TipoVagaOcupada = tipoVagaOcupada
                            };

                            /* carrega os dados da confirmacao */

                            if (rbConfirmado.Checked && rbConfirmado.Enabled)
                                dtConfirmacao = conf;

                            /* carrega os dados da confirmacao */

                            var validacao = rnConfirmacaoMatricula.ValidaConfirmacaoTelaAluno(conf);

                            if (!validacao.Valido)
                            {
                                if (!string.IsNullOrEmpty(validacao.Mensagem))
                                {
                                    this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                                }

                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "atualizarGrid", "atualizarGrid();", true);

                                return;
                            }

                            confs.Add(conf);
                        }
                    }
                }


                if (confs.Count > 0)
                {
                    var msg = string.Empty;
                    var confirmado = false;
                    ConfirmacaoMatricula.Alterar(confs);

                    foreach (var confirmacaoMatricula in confs)
                    {
                        if (confirmado) break;

                        var dtAluno = new LyAluno
                        {
                            Aluno = txtAluno.Text.Trim(),
                            UnidadeEnsino = tseUnidadeEns.DBValue.ToString(),
                            Curso = tseCurso.DBValue.ToString(),
                            Serie = Convert.ToDecimal(cmbSerie.SelectedValue),
                            Turno = cmbTurno.SelectedValue
                        };

                        if (confirmacaoMatricula.Status == ConfirmacaoMatricula.Confirmado)
                        {
                            var validacao = Turma.ValidarEnturmacao(confirmacaoMatricula, dtAluno);
                            confirmado = true;
                            if (validacao.Valido)
                            {
                                string turma;
                                string mensagem;

                                if (Turma.EnturmarAluno(confirmacaoMatricula, out turma, out mensagem))
                                {
                                    msg = "O(A) aluno(a) " + txtNomeCompl.Text.ToUpper() +
                                          " foi enturmado(a) com sucesso na turma " + turma +
                                          ". Você poderá realizar permuta de alunos entre turmas caso seja necessária.";
                                }
                                else
                                {
                                    msg = "Não foi possível realizar a enturmação. Motivo:" + mensagem;
                                }

                                break;
                            }
                            else
                            {
                                msg = validacao.Mensagem;
                            }

                        }
                    }

                    /* inicio nova rotina de update UPDATE TCE_CONFIRMACAO_MATRICULA */

                    if (contConf == 1)
                    {
                        rnConfirmacaoMatricula.AtualizarStatusNaoConfirmado(dtConfirmacao);
                    }

                    /* final nova rotina de update UPDATE TCE_CONFIRMACAO_MATRICULA */

                    var script = @"alert('Confirmação/Renovação atualizada com sucesso. ' + '" + msg + @"');";

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);

                    CarregarGridConfirmacaoRenovacaoMatricula();

                    ddlConfirmacaoMatricula.Enabled = true;

                    this._tipoOperacao = TipoOperacao.Sucesso;

                    ControlarTipoOperacao();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Não existe Confirmação/Renovação a ser atualizado.');", true);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnImprimirConfirmacao_Click(object sender, EventArgs e)
        {
            RN.Entidades.LogImpressaoFichaMatricula dtImprimirLog = new Techne.Lyceum.RN.Entidades.LogImpressaoFichaMatricula();
            RN.Aluno rnAluno = new Aluno();
            ValidacaoDados validacao = new ValidacaoDados();

            try
            {
                if (ddlConfirmacaoMatricula.Value != null)
                {
                    validacao = rnAluno.ValidaImpressaoFicha(txtAluno.Text.Trim(), User.Identity.Name);

                    if (validacao.Valido)
                    {
                        //Gravar log de impressao
                        dtImprimirLog = new RN.Entidades.LogImpressaoFichaMatricula
                        {
                            AlunoId = txtAluno.Text.Trim(),
                            ConfirmacaoMatriculaId = Convert.ToInt32(ddlConfirmacaoMatricula.Value.ToString()),
                            Matricula = User.Identity.Name
                        };

                        RN.LogImpressaoFichaMatricula.Inserir(dtImprimirLog);

                        //Abrir Ficha
                        byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(ddlConfirmacaoMatricula.Value.ToString());

                        Response.Write("<script type=text/javascript>");
                        Response.Write("pagina = " + @"'FichaConfirmacao.aspx?Chave=" + Convert.ToBase64String(bytesToEncode) + "'" + ";");
                        Response.Write("abriu = false;");
                        Response.Write("function abrir() {newWindow = window.open(pagina, 'nova', 'status=no, scrollbars=yes, resizable=yes, width=850, height=800'); if (newWindow) {abriu = true;   return false;}}");
                        Response.Write("abrir();");
                        Response.Write("</script>");
                    }
                    else
                    {
                        lblMensagem.Text = lblErroConfirmacao.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblErroConfirmacao.Text = lblMensagem.Text;
            }
        }

        protected void btnImprimirRenovacao_Click(object sender, EventArgs e)
        {
            RN.RenovacaoMatricula.Entidades.LogImpressaoFichaRenovacao dtImprimirLog = new Techne.Lyceum.RN.RenovacaoMatricula.Entidades.LogImpressaoFichaRenovacao();
            RN.RenovacaoMatricula.LogImpressaoFichaRenovacao rnLogImpressaoFichaRenovacao = new Techne.Lyceum.RN.RenovacaoMatricula.LogImpressaoFichaRenovacao();
            RN.Aluno rnAluno = new Aluno();
            ValidacaoDados validacao = new ValidacaoDados();

            try
            {
                if (ddlRenovacaoMatricula.Value != null)
                {
                    validacao = rnAluno.ValidaImpressaoFicha(txtAluno.Text.Trim(), User.Identity.Name);

                    if (validacao.Valido)
                    {
                        //Gravar log de impressao
                        dtImprimirLog = new RN.RenovacaoMatricula.Entidades.LogImpressaoFichaRenovacao
                        {
                            AlunoId = txtAluno.Text.Trim(),
                            RenovacaoId = Convert.ToInt32(ddlRenovacaoMatricula.Value.ToString()),
                            Matricula = User.Identity.Name
                        };

                        rnLogImpressaoFichaRenovacao.Insere(dtImprimirLog);

                        //Abrir Ficha
                        byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(ddlRenovacaoMatricula.Value.ToString());

                        Response.Write("<script type=text/javascript>");
                        Response.Write("pagina = " + @"'FichaRenovacao.aspx?Chave=" + Convert.ToBase64String(bytesToEncode) + "'" + ";");
                        Response.Write("abriu = false;");
                        Response.Write("function abrir() {newWindow = window.open(pagina, 'nova', 'status=no, scrollbars=yes, resizable=yes, width=850, height=800'); if (newWindow) {abriu = true;   return false;}}");
                        Response.Write("abrir();");
                        Response.Write("</script>");
                    }
                    else
                    {
                        lblErroConfirmacao.Text = lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblErroConfirmacao.Text = lblMensagem.Text;
            }
        }

        private void ObterImagem()
        {
            //obtem os dados do pixel recebidos do Flash
            string pixelData = imagem.Value;

            if (string.IsNullOrEmpty(pixelData))
                return;

            var bytesImagem = System.Convert.FromBase64String(pixelData);
            bimgFotoPessoa.Value = bytesImagem;
            _alterouFoto = true;
            imagem.Value = string.Empty;

        }

        #endregion

        #region Métodos de Visibilidade dos Campos
        protected void HabilitaCampos()
        {
            tseNaturalidade.Mode = ControlMode.Edit;
            tseUnidadeEns.Mode = ControlMode.View;
            tseCurso.Mode = ControlMode.View;

            tseNaturalidade.Mode = ControlMode.Edit;
            tseNaturalidadeEstrangeira.Mode = ControlMode.Edit;
            ddlNacionalidade.Enabled = true;
            tseNaturalidadeEstrangeira.Enabled = false;
            txtSituacao.ReadOnly = false;
            txtNomeCompl.ReadOnly = true;
            ddlNacionalidade.Enabled = true;
            txtFone.ReadOnly = false;
            txtCelular.ReadOnly = false;
            txtRGNum.ReadOnly = false;
            txtCPF.ReadOnly = false;
            txtDOC_CertNasc_Numero.ReadOnly = false;
            txtDOC_CertNasc_Folha.ReadOnly = false;
            txtDOC_CertNasc_Livro.ReadOnly = false;
            ddlCartorio.Enabled = true;

            txtCep.ReadOnly = false;
            txtEndCompl.ReadOnly = false;
            txtEndNum.ReadOnly = false;
            txtEndereco.ReadOnly = false;
            txtBairro.ReadOnly = false;
            txtEstado.Attributes.Add("readonly", "readonly");
            txtNomeSocial.ReadOnly = false;
            txtFilhos.ReadOnly = false;
            txtLatitude.Enabled = true;
            txtLongitude.Enabled = true;
            pnlLocalizacao.Enabled = true;

            txtNomeCompl.ReadOnly = true;
            txtMunicipio.ReadOnly = false;
            flFoto.Enabled = true;

            ddlTipoCurso.Enabled = true;
            dtDataNasc.Enabled = false;
            rblSexo.Enabled = true;
            rblDescFamilia.Enabled = true;
            ddlEst_Civil.Enabled = true;
            cmbNecessidadeEspecial.Enabled = true;
            pnRecursos.Enabled = false;
            pnlAplicacaoProva.Enabled = true;
            ddlEtnia.Enabled = true;
            ddlCredo.Enabled = true;
            ddlTipoSanguineo.Enabled = true;

            ddlRGTipoPessoa.Enabled = true;
            cmbRGUF.Enabled = true;
            cmbRGEmissor.Enabled = true;
            dtDataExped.Enabled = true;
            ddlTipoIngresso.Enabled = true;
            dboDOC_CertNasc_DtEmissao.Enabled = true;
            ddDOC_CertNasc_Uf.Enabled = true;
            tsCEP.ShowButton = true;

            txtSituacao.ReadOnly = false;
            txtCausaEncerramento.ReadOnly = true;
            txtMotivo.ReadOnly = true;

            ddlLocalZona.Enabled = true;
            chkAreaAssentamento.Enabled = true;
            chkTerraIndigena.Enabled = true;
            chkQuilombos.Enabled = true;
            chkAreaTradicional.Enabled = true;
            chkNaoSeAplica.Enabled = true;

            ddlTipoCertidao.Enabled = true;

            ddlOutroEnsino.Enabled = true;
            ddlPoderPublicoTransp.Enabled = true;
            ddlGratuidade.Enabled = true;
            chkModais.Enabled = true;
            txtTempoAfastamento.ReadOnly = false;
            txtComplIdentidade.ReadOnly = false;
            txtNumMatriculaCertidao.ReadOnly = false;
            ddlCertidaoCivil.Enabled = true;
            ddlUFCartorio.Enabled = true;
            ddlMunicipioCartorio.Enabled = true;

            chkNaoDeclarMae.Enabled = true;
            chkNaoDeclarPai.Enabled = true;
            rblResponsavel.Enabled = true;
            chkPossuiIrmao.Enabled = true;

            if (!chkNaoDeclarMae.Checked)
            {
                txtTelefoneMae.ReadOnly = false;
                txtCPFMae.ReadOnly = false;
                chkFalecidaMae.Enabled = true;
                txtTelefoneMae.Enabled = true;
                txtCPFMae.Enabled = true;
                chkDeclaroAusenciaMae.Visible = false;
            }
            else
            {
                txtNomeMae.ReadOnly = true;
                txtTelefoneMae.Enabled = false;
                txtCPFMae.Enabled = false;
                chkFalecidaMae.Enabled = false;
                chkDeclaroAusenciaMae.Visible = true;
            }
            if (!chkNaoDeclarPai.Checked)
            {
                txtNomePai.ReadOnly = false;
                txtCPFPai.ReadOnly = false;
                txtTelefonePai.ReadOnly = false;
                chkFalecidoPai.Enabled = true;
                txtTelefonePai.Enabled = true;
                txtCPFPai.Enabled = true;
                chkDeclaroAusenciaPai.Visible = false;
            }
            else
            {
                txtNomePai.ReadOnly = true;
                txtTelefonePai.Enabled = false;
                txtCPFPai.Enabled = false;
                chkFalecidoPai.Enabled = false;
                chkDeclaroAusenciaPai.Visible = true;
            }

            txtNomeResponsavel.ReadOnly = false;
            txtCPFResponsavel.ReadOnly = false;
            txtTelefoneResp.ReadOnly = false;

            if (chkNaoDeclarMae.Checked)
            {
                txtNomeMae.ReadOnly = true;
            }
            if (chkNaoDeclarPai.Checked)
            {
                txtNomePai.ReadOnly = true;
            }

            chkAquaviario.Enabled = true;
            chkRodoviario.Enabled = true;
            chkOnibus.Enabled = true;
            chkDeclaroAusenciaMae.Enabled = true;
            chkDeclaroAusenciaPai.Enabled = true;
            chkDeclaroNecessidadeEspecial.Enabled = true;
            chkDeclaroCertidaoCivil.Enabled = true;

            txtMotivoCertidaoCivil.ReadOnly = false;
            txtMae.ReadOnly = false;
            txtPai.ReadOnly = false;
            txtEmail.ReadOnly = false;
            txtEmailGoogle.Enabled = true;
            ddlPovoIndigena.Enabled = true;
            chkTipoTranstorno.Enabled = true;
            rblPossuiTranstorno.Enabled = true;
        }

        protected void DesabilitaResponsavelLegal(string operacao, string filiacao)
        {
            foreach (ListItem item in rblResponsavel.Items)
            {
                if (item.Text == filiacao)
                {
                    if (operacao == "H")
                    {
                        item.Selected = false;
                        item.Enabled = false;
                        return;
                    }
                    if (operacao == "D")
                    {
                        item.Selected = false;
                        item.Enabled = true;
                        return;
                    }
                }
            }
        }

        protected void DesabilitaCampos()
        {
            tseNaturalidade.Mode = ControlMode.View;
            tseUnidadeEns.Mode = ControlMode.View;
            tseCurso.Mode = ControlMode.View;

            flFoto.Enabled = false;

            tseNaturalidade.Mode = ControlMode.View;
            tseNaturalidadeEstrangeira.Mode = ControlMode.View;
            ddlNacionalidade.Enabled = false;
            txtUFNascimento.Enabled = true;
            txtPaisNasc.Enabled = true;
            tseNaturalidadeEstrangeira.Enabled = false;
            txtSituacao.ReadOnly = true;
            txtNomeCompl.ReadOnly = true;
            txtAluno.ReadOnly = true;
            txtFone.ReadOnly = true;
            txtCelular.ReadOnly = true;
            txtEmail.ReadOnly = true;
            txtRGNum.ReadOnly = true;
            txtCep.ReadOnly = true;
            txtEndCompl.ReadOnly = true;
            txtEndNum.ReadOnly = true;
            txtEndereco.ReadOnly = true;
            txtBairro.ReadOnly = true;
            txtMunicipio.ReadOnly = true;
            txtEstado.Attributes.Add("readonly", "readonly");
            txtLatitude.Enabled = false;
            txtLongitude.Enabled = false;
            pnlLocalizacao.Enabled = false;
            txtCPF.ReadOnly = true;
            txtDOC_CertNasc_Numero.ReadOnly = true;
            txtDOC_CertNasc_Folha.ReadOnly = true;
            txtDOC_CertNasc_Livro.ReadOnly = true;
            ddlCartorio.Enabled = false;
            txtNomeSocial.ReadOnly = true;
            txtFilhos.ReadOnly = true;
            ddlNacionalidade.Enabled = false;
            ddlNivel.Enabled = false;
            ddlModalidade.Enabled = false;
            cmbTurno.Enabled = false;
            cmbCurriculo.Enabled = false;
            cmbSerie.Enabled = false;
            cmbAnoIngresso.Enabled = false;
            cmbSemIngresso.Enabled = false;
            ddlTipoCurso.Enabled = false;
            dtDataNasc.Enabled = false;
            rblSexo.Enabled = false;
            rblDescFamilia.Enabled = false;
            ddlEst_Civil.Enabled = false;
            cmbNecessidadeEspecial.Enabled = false;
            pnRecursos.Enabled = false;
            pnlAplicacaoProva.Enabled = false;
            ddlTipoSanguineo.Enabled = false;
            ddlEtnia.Enabled = false;
            ddlCredo.Enabled = false;
            ddlRGTipoPessoa.Enabled = false;
            cmbRGUF.Enabled = false;
            cmbRGEmissor.Enabled = false;
            dtDataExped.Enabled = false;
            tsCEP.ShowButton = false;
            ddlTipoIngresso.Enabled = false;
            dboDOC_CertNasc_DtEmissao.Enabled = false;
            ddDOC_CertNasc_Uf.Enabled = false;
            txtSituacao.ReadOnly = true;
            txtCausaEncerramento.ReadOnly = true;
            txtMotivo.ReadOnly = true;
            ddlLocalZona.Enabled = false;
            chkAreaAssentamento.Enabled = false;
            chkTerraIndigena.Enabled = false;
            chkQuilombos.Enabled = false;
            chkAreaTradicional.Enabled = false;
            chkNaoSeAplica.Enabled = false;
            ddlTipoCertidao.Enabled = false;
            ddlOutroEnsino.Enabled = false;
            ddlGratuidade.Enabled = false;
            ddlPoderPublicoTransp.Enabled = false;
            chkModais.Enabled = false;
            ddlRedeEnsinoOrigem.Enabled = false;
            txtTempoAfastamento.ReadOnly = true;
            txtComplIdentidade.ReadOnly = true;

            txtNumMatriculaCertidao.ReadOnly = true;
            ddlCertidaoCivil.Enabled = false;
            ddlUFCartorio.Enabled = false;
            ddlMunicipioCartorio.Enabled = false;

            txtNomeMae.ReadOnly = true;
            txtNomePai.ReadOnly = true;
            txtNomeResponsavel.ReadOnly = true;
            txtCPFMae.ReadOnly = true;
            txtCPFPai.ReadOnly = true;
            txtCPFResponsavel.ReadOnly = true;
            txtTelefoneMae.ReadOnly = true;
            txtTelefonePai.ReadOnly = true;
            txtTelefoneResp.ReadOnly = true;
            txtMotivoCertidaoCivil.ReadOnly = true;
            txtMae.ReadOnly = true;
            txtPai.ReadOnly = true;
            chkPossuiIrmao.Enabled = false;

            chkFalecidaMae.Enabled = false;
            chkFalecidoPai.Enabled = false;
            chkNaoDeclarMae.Enabled = false;
            chkNaoDeclarPai.Enabled = false;
            rblResponsavel.Enabled = false;

            chkAquaviario.Enabled = false;
            chkRodoviario.Enabled = false;
            chkOnibus.Enabled = false;
            chkDeclaroAusenciaMae.Enabled = false;
            chkDeclaroAusenciaPai.Enabled = false;
            chkDeclaroNecessidadeEspecial.Enabled = false;
            chkDeclaroCertidaoCivil.Enabled = false;

            txtEmailGoogle.Enabled = false;
            ddlPovoIndigena.Enabled = false;
            rblPossuiTranstorno.Enabled = false;
            chkTipoTranstorno.Enabled = false;
        }

        private void LimparTela()
        {
            hdnCompartilhada.Value = string.Empty;
            hdnFoto.Value = "N";
            hddDataAlteracaoEmail.Value = string.Empty;
            txtDataAtualizacaoEmailTransp.Text = string.Empty;
            lblConfirmarMatricula.Text = string.Empty;
            tseNaturalidade.ResetValue();
            txtIDCenso.Text = string.Empty;
            txtAluno.Text = string.Empty;
            txtSituacao.Text = string.Empty;
            txtPessoa.Text = string.Empty;
            txtNomeCompl.Text = string.Empty;
            txtNumInscricao.Text = string.Empty;
            txtNomeSocial.Text = string.Empty;
            txtFilhos.Text = string.Empty;
            tseUnidadeEns.ResetValue();
            cmbTurno.ClearSelection();
            cmbCurriculo.Items.Clear();
            tseCurso.ResetValue();
            cmbSerie.Items.Clear();
            ddlNivel.SelectedValue = string.Empty;
            ddlModalidade.SelectedValue = string.Empty;
            cmbAnoIngresso.SelectedValue = string.Empty;
            cmbSemIngresso.SelectedValue = string.Empty;
            lblMensagem.Text = string.Empty;
            lblErroConfirmacao.Text = string.Empty;
            dtDataNasc.Text = string.Empty;
            rblSexo.SelectedIndex = -1;
            rblDescFamilia.SelectedIndex = -1;
            cmbNecessidadeEspecial.SelectedValue = string.Empty;
            rblRecursoAplicaProvaExclusivo.SelectedIndex = -1;
            chkRecursoAplicacaoProva.SelectedIndex = -1;
            chkNenhumRecursoAplicacaoProva.Checked = false;
            pnRecursos.Visible = false;
            pnlAplicacaoProva.Visible = false;
            ddlEtnia.SelectedValue = string.Empty;
            ddlCredo.SelectedValue = string.Empty;
            chkModais.SelectedValue = null;
            chkAquaviario.ClearSelection();
            chkRodoviario.ClearSelection();
            chkOnibus.ClearSelection();
            lblRodoviario.Visible = false;
            lblAquaviario.Visible = false;
            lblOnibus.Visible = false;
            chkRodoviario.Visible = false;
            chkAquaviario.Visible = false;
            chkOnibus.Visible = false;
            ddlTipoSanguineo.SelectedValue = string.Empty;
            txtEndereco.Text = string.Empty;
            txtEndNum.Text = string.Empty;
            txtEndCompl.Text = string.Empty;
            txtCep.Text = string.Empty;
            txtBairro.Text = string.Empty;
            txtMunicipio.Text = string.Empty;
            txtLatitude.Text = string.Empty;
            txtLongitude.Text = string.Empty;
            txtFone.Text = string.Empty;
            txtCelular.Text = string.Empty;
            hddTxtEmail.Value = string.Empty;
            txtEmail.Text = string.Empty;
            txtEmailTransp.Text = string.Empty;
            txtEmailConfirmacao.Text = string.Empty;
            txtEmailGoogle.Text = string.Empty;
            ddlRGTipoPessoa.SelectedValue = string.Empty;
            txtRGNum.Text = string.Empty;
            bimgFotoPessoa.ContentBytes = null;
            cmbRGEmissor.SelectedValue = string.Empty;
            dtDataExped.Text = string.Empty;
            ddlTipoIngresso.SelectedValue = string.Empty;
            txtDOC_CertNasc_Numero.Text = string.Empty;
            txtDOC_CertNasc_Folha.Text = string.Empty;
            txtDOC_CertNasc_Livro.Text = string.Empty;
            ddlCartorio.Items.Clear();
            dboDOC_CertNasc_DtEmissao.Text = string.Empty;
            ddDOC_CertNasc_Uf.SelectedValue = string.Empty;
            ddlNacionalidade.SelectedValue = string.Empty;
            ddlEst_Civil.SelectedValue = string.Empty;
            txtMunicipio.Text = string.Empty;
            hdnCodMunicipio.Value = string.Empty;
            txtPessoa.Text = string.Empty;
            txtEstado.Value = string.Empty;
            txtCPF.Text = string.Empty;
            ddlUFCartorio.ClearSelection();
            ddlMunicipioCartorio.Items.Clear();
            ddlCartorio.Items.Clear();
            txtNumMatriculaCertidao.Text = string.Empty;
            txtComplIdentidade.Text = string.Empty;
            txtNIS.Text = string.Empty;
            ddlCertidaoCivil.ClearSelection();
            pnlNovo.Visible = false;
            pnlAntigo.Visible = false;
            pnlTipoCertidaoCivil.Visible = false;
            ddlRedeEnsinoOrigem.ClearSelection();
            txtTempoAfastamento.Text = string.Empty;
            txtDataCadastro.Text = string.Empty;
            txtUFNascimento.Text = string.Empty;
            txtPaisNasc.Text = string.Empty;
            tseNaturalidadeEstrangeira.Enabled = false;
            tseNaturalidade.ResetValue();
            tseNaturalidadeEstrangeira.ResetValue();

            txtNomePai.Text = string.Empty;
            txtNomeMae.Text = string.Empty;
            chkNaoDeclarMae.Checked = false;
            chkNaoDeclarPai.Checked = false;
            chkFalecidaMae.Checked = false;
            chkFalecidoPai.Checked = false;
            chkDeclaroAusenciaMae.Checked = false;
            chkDeclaroAusenciaPai.Checked = false;
            chkDeclaroCertidaoCivil.Checked = false;
            chkDeclaroNecessidadeEspecial.Checked = false;
            chkDeclaroNecessidadeEspecial.Visible = false;
            chkDeclaroNecessidadeEspecial.Text = "Declaro estar de posse do Laudo Médico ou Parecer Pedagógico";
            chkDeclaroAusenciaMae.Visible = false;
            chkDeclaroAusenciaPai.Visible = false;
            chkDeclaroCertidaoCivil.Visible = false;
            txtCPFPai.Text = string.Empty;
            txtCPFMae.Text = string.Empty;
            txtTelefoneMae.Text = string.Empty;
            txtTelefonePai.Text = string.Empty;
            rblResponsavel.ClearSelection();
            txtNomeResponsavel.Text = string.Empty;
            txtCPFResponsavel.Text = string.Empty;
            txtTelefoneResp.Text = string.Empty;
            txtNomeResponsavel.Visible = false;
            lblNomeResponsavel.Visible = false;
            txtCPFResponsavel.Visible = false;
            lblCPFResponsavel.Visible = false;
            txtTelefoneResp.Visible = false;
            lblTelefoneResponsavel.Visible = false;
            txtMotivoCertidaoCivil.Text = string.Empty;
            txtMotivoCertidaoCivil.Visible = false;
            chkPossuiIrmao.Checked = false;

            txtPai.Text = string.Empty;
            txtMae.Text = string.Empty;
            hdnDataNascimento.Value = string.Empty;
            txtNomeIrmao.Text = string.Empty;

            ddlConfirmacaoMatricula.Items.Clear();
            grdBuscaIrmaosCadastrados.DataSource = null;
            grdBuscaIrmaosCadastrados.DataBind();
            ddlGratuidade.SelectedValue = string.Empty;
            chkRecursoNecessidadeEspecial.ClearSelection();

            ddlPovoIndigena.ClearSelection();
            rblPossuiTranstorno.ClearSelection();
            chkTipoTranstorno.ClearSelection();

        }

        #endregion

        #region Métodos de Carregar DropDownLists
        private void CarregarFiltroCurso()
        {
            if (!tseUnidadeEns.DBValue.IsNull && tseUnidadeEns.IsValidDBValue && ddlNivel.SelectedValue != "" && ddlModalidade.SelectedValue != "")
                tseCurso.SqlWhere = "pc.curso is null and UNIDADE_ENS = '" + RN.RNBase.MudarAspas(tseUnidadeEns.DBValue.ToString()) + "' and modalidade = '" + RN.RNBase.MudarAspas(ddlModalidade.SelectedValue.ToString()) + "' and tipo = '" + RN.RNBase.MudarAspas(ddlNivel.SelectedValue.ToString()) + "'";
            else if (!tseUnidadeEns.DBValue.IsNull && tseUnidadeEns.IsValidDBValue && ddlNivel.SelectedValue == "" && ddlModalidade.SelectedValue != "")
                tseCurso.SqlWhere = "pc.curso is null and UNIDADE_ENS = '" + RN.RNBase.MudarAspas(tseUnidadeEns.DBValue.ToString()) + "' and modalidade = '" + RN.RNBase.MudarAspas(ddlModalidade.SelectedValue.ToString()) + "'";
            else if (!tseUnidadeEns.DBValue.IsNull && tseUnidadeEns.IsValidDBValue && ddlNivel.SelectedValue != "" && ddlModalidade.SelectedValue == "")
                tseCurso.SqlWhere = "pc.curso is null and UNIDADE_ENS = '" + RN.RNBase.MudarAspas(tseUnidadeEns.DBValue.ToString()) + "' and tipo = '" + RN.RNBase.MudarAspas(ddlNivel.SelectedValue.ToString()) + "'";
            else if (!tseUnidadeEns.DBValue.IsNull && tseUnidadeEns.IsValidDBValue && ddlNivel.SelectedValue == "" && ddlModalidade.SelectedValue == "")
                tseCurso.SqlWhere = "pc.curso is null and UNIDADE_ENS = '" + RN.RNBase.MudarAspas(tseUnidadeEns.DBValue.ToString()) + "'";

            tseCurso.DataBind();
        }

        private void CarregarFiltroCursoMatricula()
        {
            tseCursoMatricula.ResetValue();
            if (!tseUnidadeEnsinoMatricula.DBValue.IsNull && tseUnidadeEnsinoMatricula.IsValidDBValue && ddlNivelMatricula.SelectedValue != "" && ddlModalidadeMatricula.SelectedValue != "")
                tseCursoMatricula.SqlWhere = "pc.curso is null and UNIDADE_ENS = '" + RN.RNBase.MudarAspas(tseUnidadeEnsinoMatricula.DBValue.ToString()) + "' and modalidade = '" + RN.RNBase.MudarAspas(ddlModalidadeMatricula.SelectedValue.ToString()) + "' and tipo = '" + RN.RNBase.MudarAspas(ddlNivelMatricula.SelectedValue.ToString()) + "'";
            else if (!tseUnidadeEnsinoMatricula.DBValue.IsNull && tseUnidadeEnsinoMatricula.IsValidDBValue && ddlNivelMatricula.SelectedValue == "" && ddlModalidadeMatricula.SelectedValue != "")
                tseCursoMatricula.SqlWhere = "pc.curso is null and UNIDADE_ENS = '" + RN.RNBase.MudarAspas(tseUnidadeEnsinoMatricula.DBValue.ToString()) + "' and modalidade = '" + RN.RNBase.MudarAspas(ddlModalidadeMatricula.SelectedValue.ToString()) + "'";
            else if (!tseUnidadeEnsinoMatricula.DBValue.IsNull && tseUnidadeEnsinoMatricula.IsValidDBValue && ddlNivelMatricula.SelectedValue != "" && ddlModalidadeMatricula.SelectedValue == "")
                tseCursoMatricula.SqlWhere = "pc.curso is null and UNIDADE_ENS = '" + RN.RNBase.MudarAspas(tseUnidadeEnsinoMatricula.DBValue.ToString()) + "' and tipo = '" + RN.RNBase.MudarAspas(ddlNivelMatricula.SelectedValue.ToString()) + "'";
            else if (!tseUnidadeEnsinoMatricula.DBValue.IsNull && tseUnidadeEnsinoMatricula.IsValidDBValue && ddlNivelMatricula.SelectedValue == "" && ddlModalidadeMatricula.SelectedValue == "")
                tseCursoMatricula.SqlWhere = "pc.curso is null and UNIDADE_ENS = '" + RN.RNBase.MudarAspas(tseUnidadeEnsinoMatricula.DBValue.ToString()) + "'";

            tseCursoMatricula.DataBind();
        }

        private void PadronizarDropDownList(DropDownList drop, string defaultValue)
        {
            if (_tipoOperacao.Equals(TipoOperacao.Novo) || _tipoOperacao.Equals(TipoOperacao.NovaMatricula))
            {
                drop.SelectedValue = "";
                if (false) // ddlPaisNasc substituído por txtPaisNasc
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

            if (!string.IsNullOrEmpty(defaultValue))
            {
                if (drop.Items.FindByValue(defaultValue) != null)
                {
                    drop.SelectedValue = defaultValue;
                }
            }
        }

        private void CarregarList()
        {
            Object objModal = RN.Util.Cache.CarregaItemTabelaGeralPor(RN.Util.Cache.TransporteModal);

            if (objModal != null)
            {
                chkModais.Items.Clear();
                chkModais.DataSource = objModal;
                chkModais.DataBind();
            }
        }

        private void CarregaTipoRecursoNecessidadeEspecial()
        {
            RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial rnTipoRecursoNecessidadeEspecial = new Techne.Lyceum.RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial();

            chkRecursoNecessidadeEspecial.Items.Clear();
            chkRecursoNecessidadeEspecial.DataSource = rnTipoRecursoNecessidadeEspecial.ListaTipoRecursoNecessidadeEspecialAtivo();
            chkRecursoNecessidadeEspecial.DataTextField = "DESCRICAO";
            chkRecursoNecessidadeEspecial.DataValueField = "TIPORECURSONECESSIDADEESPECIALID";
            chkRecursoNecessidadeEspecial.DataBind();
        }

        private void CarregaTipoTranstorno()
        {
            RN.RecursosHumanos.TranstornoAprendizagem rnTranstornoAprendizagem = new RN.RecursosHumanos.TranstornoAprendizagem();

            chkTipoTranstorno.Items.Clear();
            chkTipoTranstorno.DataSource = rnTranstornoAprendizagem.ListaAtivo();
            chkTipoTranstorno.DataTextField = "DESCRICAO";
            chkTipoTranstorno.DataValueField = "TRANSTORNOAPRENDIZAGEMID";
            chkTipoTranstorno.DataBind();
        }

        private void CarregaRecursoAplicacaoProva()
        {
            DataTable dtRecursoAplicacaoProva = new DataTable();
            dtRecursoAplicacaoProva = RN.NecessidadeEspecial.RecursoAplicacaoProva.Listar();

            chkRecursoAplicacaoProva.Items.Clear();
            chkRecursoAplicacaoProva.DataSource = dtRecursoAplicacaoProva.Select("EXCLUSIVO = 0 and ATIVO = 1").CopyToDataTable();
            chkRecursoAplicacaoProva.DataTextField = "NOME";
            chkRecursoAplicacaoProva.DataValueField = "RECURSOAPLICACAOPROVAID";
            chkRecursoAplicacaoProva.DataBind();

            rblRecursoAplicaProvaExclusivo.Items.Clear();
            rblRecursoAplicaProvaExclusivo.DataSource = dtRecursoAplicacaoProva.Select("EXCLUSIVO = 1").CopyToDataTable();
            rblRecursoAplicaProvaExclusivo.DataTextField = "NOME";
            rblRecursoAplicaProvaExclusivo.DataValueField = "RECURSOAPLICACAOPROVAID";
            rblRecursoAplicaProvaExclusivo.DataBind();

            chkRecursoAplicacaoProva.Attributes.Add("onclick", "retiraSelecaoCheckNenhumRecursoAplicacaoProva('" + chkNenhumRecursoAplicacaoProva.ClientID + "', '" + chkRecursoAplicacaoProva.ClientID + "', '" + rblRecursoAplicaProvaExclusivo.ClientID + "')");
            rblRecursoAplicaProvaExclusivo.Attributes.Add("onclick", "retiraSelecaoCheckNenhumRecursoAplicacaoProva('" + chkNenhumRecursoAplicacaoProva.ClientID + "', '" + chkRecursoAplicacaoProva.ClientID + "', '" + rblRecursoAplicaProvaExclusivo.ClientID + "')");
            chkNenhumRecursoAplicacaoProva.Attributes.Add("onclick", "retiraSelecaoCheckRecursoAplicacaoProva('" + chkNenhumRecursoAplicacaoProva.ClientID + "', '" + chkRecursoAplicacaoProva.ClientID + "', '" + rblRecursoAplicaProvaExclusivo.ClientID + "')");
        }

        private void CarregaDadosAlunoTipoRecursoNecessidadeEspecial()
        {
            RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial rnTipoRecursoNecessidadeEspecial = new Techne.Lyceum.RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial();
            RN.NecessidadeEspecial.AlunoRecursoNecessidadeEspecial rnAlunoRecursoNecessidadeEspecial = new Techne.Lyceum.RN.NecessidadeEspecial.AlunoRecursoNecessidadeEspecial();
            RN.NecessidadeEspecial.AvaliacaoNapes rnAvaliacaoNapes = new Techne.Lyceum.RN.NecessidadeEspecial.AvaliacaoNapes();
            RN.DTOs.DadosAvaliacaoNapes dadosAvaliacaoNapes = new Techne.Lyceum.RN.DTOs.DadosAvaliacaoNapes();

            string textoAvaliacao = string.Empty;
            List<int> tipoRecursoAluno = new List<int>();
            CarregaTipoRecursoNecessidadeEspecial();

            DataTable dtTipoRecurso = new DataTable();
            dtTipoRecurso = rnTipoRecursoNecessidadeEspecial.ListaTipoRecursoNecessidadeEspecialAtivo();

            if (!tseAluno.DBValue.IsNull)
            {
                if (tseAluno.IsValidDBValue)
                {
                    //tipoRecursoAluno = rnAlunoRecursoNecessidadeEspecial.ListaRecursoAlunoPor(tseAluno.DBValue.ToString());

                    //foreach (var tipoRecurso in tipoRecursoAluno)
                    //{
                    //    chkRecursoNecessidadeEspecial.Items.FindByValue(tipoRecurso.ToString()).Selected = true;
                    //}

                    foreach (DataRow tipos in dtTipoRecurso.Rows)
                    {
                        textoAvaliacao = string.Empty;
                        dadosAvaliacaoNapes = rnAvaliacaoNapes.ObtemDadosAvaliacaoPor(tseAluno.DBValue.ToString(), Convert.ToInt32(tipos["TIPORECURSONECESSIDADEESPECIALID"].ToString()));

                        if (dadosAvaliacaoNapes.AvaliacaoNapesId > 0)
                        {
                            textoAvaliacao = " <b> Avaliado: </b>" + (Convert.ToBoolean(dadosAvaliacaoNapes.NecessitaRecurso) ? "Sim" : "Não") + " Por: " + dadosAvaliacaoNapes.UsuarioId + " - " + dadosAvaliacaoNapes.NomeUsuario + (Convert.ToBoolean(dadosAvaliacaoNapes.Transitorio) ? " Vigência: " + String.Format("{0:dd/MM/yyyy}", dadosAvaliacaoNapes.DataInicio) + " a " + String.Format("{0:dd/MM/yyyy}", dadosAvaliacaoNapes.DataFim) : string.Empty);
                        }
                        else
                        {
                            textoAvaliacao = string.Empty;
                        }

                        chkRecursoNecessidadeEspecial.Items.FindByValue(tipos["TIPORECURSONECESSIDADEESPECIALID"].ToString()).Text += textoAvaliacao;

                        if (Convert.ToBoolean(dadosAvaliacaoNapes.NecessitaRecurso))
                        {
                            chkRecursoNecessidadeEspecial.Items.FindByValue(tipos["TIPORECURSONECESSIDADEESPECIALID"].ToString()).Selected = true;
                        }
                    }
                }
            }
        }

        private void CarregaDadosTipoTransporto(decimal pessoa)
        {
            RN.PessoaTranstornoAprendizagem rnPessoaTranstornoAprendizagem = new Techne.Lyceum.RN.PessoaTranstornoAprendizagem();

            List<int> listaTranstorno = new List<int>();

            listaTranstorno = rnPessoaTranstornoAprendizagem.ListaTranstornoAprendizagemPor(pessoa);

            foreach (var linha in listaTranstorno)
            {

                chkTipoTranstorno.Items.FindByValue(linha.ToString()).Selected = true;

            }
        }

        #endregion

        private void ControlarTSearchs()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        tseAluno.Enabled = true;
                        pntabEspecializado.Visible = false;

                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        tseAluno.Enabled = true;
                        tseNaturalidade.Mode = ControlMode.View;
                        tseUnidadeEns.Mode = ControlMode.View;
                        tseCurso.Mode = ControlMode.View;
                        break;
                    }

                case TipoOperacao.Novo:
                    {
                        tseAluno.Enabled = false;
                        tseCurso.Enabled = true;
                        tseNaturalidade.Enabled = true;
                        tseUnidadeEns.Enabled = true;

                        break;
                    }
                case TipoOperacao.Excluir:
                    {
                        tseAluno.Enabled = false;
                        break;
                    }
                case TipoOperacao.Alterar:
                    {
                        tseAluno.Enabled = false;
                        tseNaturalidade.Enabled = true;
                        tseUnidadeEns.Mode = ControlMode.View;
                        tseCurso.Mode = ControlMode.View;

                        if (ViewState["AgendaCurso"] != null)
                        {
                            tseCurso.Mode = ControlMode.View;
                            tseUnidadeEns.Mode = ControlMode.View;
                        }

                        if (ViewState["ExisteMatricula"] != null)
                        {
                            tseUnidadeEns.Mode = ControlMode.View;
                            tseCurso.Mode = ControlMode.View;
                        }

                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        tseNaturalidade.Mode = ControlMode.View;
                        tseUnidadeEns.Mode = ControlMode.View;
                        tseCurso.Mode = ControlMode.View;

                        break;
                    }
                case TipoOperacao.CadastrarAlunoCompartilhadas:
                    {
                        tseAluno.Enabled = false;
                        tseCurso.Enabled = false;
                        tseNaturalidade.Enabled = true;
                        tseUnidadeEns.Enabled = false;

                        break;
                    }
                case TipoOperacao.NovaMatricula:
                    {
                        tseAluno.Enabled = false;
                        tseUnidadeEns.Mode = ControlMode.View;
                        tseCurso.Mode = ControlMode.View;

                        break;
                    }
            }
        }

        protected void cmbNecessidadeEspecial_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RN.Aluno rnAluno = new Aluno();
                if (this._tipoOperacao.Equals(TipoOperacao.Alterar))
                {
                    chkDeclaroNecessidadeEspecial.Visible = false;
                    chkDeclaroNecessidadeEspecial.Checked = false;
                    chkDeclaroNecessidadeEspecial.Text = "Declaro estar de posse do Laudo Médico ou Parecer Pedagógico";


                    if (rnAluno.EhAlunoSemNecessidadeEspecialPor(txtAluno.Text))
                    {
                        pntabEspecializado.Enabled = false;
                    }
                    else
                    {
                        pntabEspecializado.Enabled = true;
                        tab.Tabs[5].Enabled = true;
                    }
                }
                //Verifica se a nova opção é de necessidade especial
                if (cmbNecessidadeEspecial.SelectedValue != "30") //"Não possui."
                {
                    //Habilita e declaração de laudo de deficiencia
                    chkDeclaroNecessidadeEspecial.Visible = true;
                    chkDeclaroNecessidadeEspecial.Checked = true;

                    //Confirma com usuario a declaração
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), string.Format("CheckedNecessidadeEspecial('{0}')", this.chkDeclaroNecessidadeEspecial.ClientID), true);
                }

                VerificaNecessidadeEspecial();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void VerificaNecessidadeEspecial()
        {
            pnRecursos.Visible = !(cmbNecessidadeEspecial.SelectedValue == "30" || string.IsNullOrEmpty(cmbNecessidadeEspecial.SelectedValue));
            pnlAplicacaoProva.Visible = !(cmbNecessidadeEspecial.SelectedValue == "30" || string.IsNullOrEmpty(cmbNecessidadeEspecial.SelectedValue));
            pntabEspecializado.Enabled = !((cmbNecessidadeEspecial.SelectedValue == "30") || (cmbTurno.SelectedValue == "INTEGRAL") || (cmbTurno.SelectedValue == "AMPLIADO") || string.IsNullOrEmpty(cmbNecessidadeEspecial.SelectedValue));
        }

        private int GetSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = grdBusca.PageIndex * grdBusca.SettingsPager.PageSize;
            for (int i = 0; i < grdBusca.VisibleRowCount; i++)
            {
                if (grdBusca.Selection.IsRowSelected(startIndexOnPage + i))
                    return startIndexOnPage + i;
            }

            return -1;
        }

        protected void grdBusca_SelectionChanged(object sender, EventArgs e)
        {
            string situacaoAluno = string.Empty;
            string unidadeEnsinoAluno = string.Empty;
            hdnAlunoTransf.Value = string.Empty;

            try
            {
                int curPageSelection = GetSelectedRowOnTheCurrentPage();

                if (curPageSelection >= 0)
                {
                    Techne.Lyceum.RN.Aluno.DadosAluno aluno = new Techne.Lyceum.RN.Aluno.DadosAluno();

                    aluno.Aluno = Convert.ToString(grdBusca.GetRowValues(curPageSelection, "ALUNO"));
                    aluno.UnidadeResponsavel = Convert.ToString(grdBusca.GetRowValues(curPageSelection, "UNIDADE_ENSINO"));

                    string queryString = MontarQueryString(aluno.Aluno);

                    byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                    DataTable dtUsuarios = Usuarios.ListarUsuariosPorUE(User.Identity.Name);
                    string unidadeEnsinoUsuario = dtUsuarios.Rows[0]["UNIDADE_ENS"].ToString();
                    DataTable dt = Aluno.ListarAlunoNovo(aluno.Aluno, txtNomeAlunoNovo.Text.Trim(), txtNomeMaeAlunoNovo.Text.Trim(), Convert.ToDateTime(grdBusca.GetRowValues(curPageSelection, "DT_NASC")));

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[curPageSelection]["SIT_ALUNO"] != null)
                        {
                            situacaoAluno = dt.Rows[curPageSelection]["SIT_ALUNO"].ToString();
                        }
                        unidadeEnsinoAluno = dt.Rows[curPageSelection]["UNIDADE_ENSINO"].ToString();
                    }

                    if (hdnCompartilhada.Value == "S")
                    {
                        unidadeEnsinoUsuario = hdnUnidade.Value;
                    }

                    if (unidadeEnsinoUsuario.Equals(unidadeEnsinoAluno))
                    {
                        if (situacaoAluno.Equals("Ativo"))
                        {
                            Response.Redirect("Alunos.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
                        }
                        else
                        {
                            Response.Redirect("ListarEncerramentoAluno.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
                        }
                    }
                    else
                    {
                        lblMensagemPopup.Text = "O aluno está cadastrado em outra escola. Deseja solicitar sua transferência?";

                        hdnAlunoTransf.Value = aluno.Aluno;

                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopupConfirmarTransf();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagemAlunoNovo.Text = ex.Message;
            }
        }

        private string MontarQueryString(string aluno)
        {
            string queryString = string.Empty;
            int ano = 0;
            int periodo = 0;
            string unidadeDestino = string.Empty;
            string curso = string.Empty;
            string serie = string.Empty;
            hdnUnidade.Value = string.Empty;

            if (!hdnCompartilhada.Value.IsNullOrEmptyOrWhiteSpace())
            {

                byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["ChaveInscricaoCompartilhadas"]);
                string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

                string[] inscricaoCompartilhadas = decodedText.Split('&');

                foreach (string dados in inscricaoCompartilhadas)
                {
                    if (dados.IndexOf("ano") >= 0)
                    {
                        ano = Convert.ToInt32(dados.Substring(dados.LastIndexOf('=') + 1));
                    }
                    else if (dados.IndexOf("periodo") >= 0)
                    {
                        periodo = Convert.ToInt32(dados.Substring(dados.LastIndexOf('=') + 1));
                    }
                    else if (dados.IndexOf("unidadeDestino") >= 0)
                    {
                        unidadeDestino = dados.Substring(dados.LastIndexOf('=') + 1);
                        hdnUnidade.Value = unidadeDestino;
                    }
                    else if (dados.IndexOf("curso") >= 0)
                    {
                        curso = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("serie") >= 0)
                    {
                        serie = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("aluno") >= 0)
                    {
                        aluno = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                }
            }

            if (aluno != null)
            {
                queryString += "aluno=" + aluno;
                queryString += "&comp=" + hdnCompartilhada.Value;
                queryString += "&ano=" + ano;
                queryString += "&periodo=" + periodo;
                queryString += "&destino=" + unidadeDestino;
                queryString += "&curso=" + curso;
                queryString += "&serie=" + serie;

            }
            return queryString;
        }

        protected void btnBuscarIrmaos_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                var mensagens = new List<string>();
                lblMensagemIrmaos.Text = string.Empty;

                var pessoa = Aluno.RetornaPessoa(txtAluno.Text);

                if (string.IsNullOrEmpty(txtNomeIrmao.Text.Trim()))
                {
                    mensagens.Add("É necessário preencher o Nome do irmão para realizar a busca.");
                }
                else
                {
                    if (string.IsNullOrEmpty(txtPai.Text) && string.IsNullOrEmpty(txtMae.Text))
                    {
                        mensagens.Add("Para efetuar a busca é necessário preencher o Nome da Mãe ou Nome do Pai.");
                    }
                    else
                    {
                        int cont = 0;
                        Regex regex = new Regex(@"\s{2,}");
                        string NomeMae = regex.Replace(txtMae.Text.Trim().ToUpper(), " ");
                        string NomePai = regex.Replace(txtPai.Text.Trim().ToUpper(), " ");
                        string NomeIrmão = regex.Replace(txtNomeIrmao.Text.Trim().ToUpper(), " ");
                        var contemRepeticao = RN.Validacao.ContemRepeticao(txtMae.Text, 3);
                        var contemRepeticaoPai = RN.Validacao.ContemRepeticao(txtPai.Text, 3);
                        var contemRepeticaoIrmao = RN.Validacao.ContemRepeticao(txtNomeIrmao.Text, 3);

                        if (!string.IsNullOrEmpty(NomeMae)) cont = cont + 1;
                        if (!string.IsNullOrEmpty(NomePai)) cont = cont + 1;
                        if (!string.IsNullOrEmpty(NomeIrmão)) cont = cont + 1;

                        if (cont < 2)
                            mensagens.Add("Para fazer a busca será necessário a preencher pelo menos dois campos.");

                        if (contemRepeticao)
                            mensagens.Add("O campo NOME DA MÃE possui inconsistência por repetição excessiva de letras. Favor corrigir a informação.");

                        if (contemRepeticaoPai)
                            mensagens.Add("O campo NOME DA PAI possui inconsistência por repetição excessiva de letras. Favor corrigir a informação.");

                        if (contemRepeticaoIrmao)
                            mensagens.Add("O campo NOME DO IRMÃO possui inconsistência por repetição excessiva de letras. Favor corrigir a informação.");

                        if (mensagens.Count == 0)
                        {
                            CarregarGridIrmaosCadasAntes(pessoa);
                            CarregarGridPesquisaIrmaos();
                        }
                    }
                }

                if (mensagens.Count > 0)
                {
                    lblMensagemIrmaos.Text = mensagens.Aggregate((x, y) => x + Environment.NewLine + y).Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                lblMensagemIrmaos.Text = ex.Message;
            }
        }

        protected void btnSalvarIrmaos_Click(object sender, EventArgs e)
        {
            this.SalvarIrmaos();
        }

        protected void grdBuscaIrmaos_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            CarregarGridPesquisaIrmaos();
        }

        protected void grdBuscaIrmaosCadastrados_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
            string irmaos_id = Convert.ToString(e.Keys["ALUNO"]);

            var parente = Aluno.RetornaPessoa(irmaos_id);
            var pessoaAluno = Aluno.RetornaPessoa(txtAluno.Text);

            RN.RelacaoPessoa.Excluir(parente);
            RN.RelacaoPessoa.Excluir(pessoaAluno);
            var dtAluno = new LyAluno();
            dtAluno.Aluno = (string)tseAluno.DBValue;
            var pessoa = Aluno.RetornaPessoa(dtAluno.Aluno);
            CarregarGridIrmaosCadasAntes(pessoa);

            if (grdBuscaIrmaosCadastrados.DataSource != null)
                chkPossuiIrmao.Checked = false;
        }

        private void SalvarIrmaos()
        {
            try
            {
                var nomeInvalido = TextValidator.HasNumbers(txtMae.Text);
                var contemRepeticao = RN.Validacao.ContemRepeticao(txtMae.Text, 3);
                var dtAluno = new LyAluno();
                var matricsel = string.Empty;
                var matriculas = new List<LyAluno>();
                dtAluno.Aluno = (string)tseAluno.DBValue;
                var pessoa = Aluno.RetornaPessoa(txtAluno.Text);

                bool ehChecado = false;

                if (nomeInvalido)
                {
                    lblMensagem.Text = "O campo NOME DA MÃE possui inconsistência por não representar um nome válido. Favor corrigir a informação.";

                    return;
                }

                if (contemRepeticao)
                {
                    lblMensagem.Text = "O campo NOME DA MÃE possui inconsistência por repetição excessiva de letras. Favor corrigir a informação.";

                    return;
                }

                for (var i = 0; i < this.grdBuscaIrmaos.VisibleRowCount; i++)
                {
                    var chkIrmaos = DevExpressHelper.GetControl<CheckBox>(this.grdBuscaIrmaos, i, "IRMAOS", "chkIrmaos");

                    if (chkIrmaos != null)
                    {
                        if (chkIrmaos.Checked)
                            ehChecado = true;
                    }
                }

                if (ehChecado == false)
                {
                    lblMensagem.Text = "É necessário selecionar pelo menos uma pessoa antes de salvar.";
                    return;
                }

                var row = 0;
                for (var rowIndex = 0; rowIndex < this.grdBuscaIrmaos.VisibleRowCount; rowIndex++)
                {
                    var chkIrmaos = DevExpressHelper.GetControl<CheckBox>(this.grdBuscaIrmaos, rowIndex, "IRMAOS",
                                                         "chkIrmaos");
                    var matricSel = (string)grdBuscaIrmaos.GetRowValues(rowIndex, "ALUNO");

                    if (chkIrmaos != null)
                    {
                        if (chkIrmaos.Checked)
                        {
                            chkIrmaos.Enabled = false;
                            chkIrmaos.Checked = false;
                            row = rowIndex;
                            var pessoaSelecionada = 0;
                            var relacaoPessoa = new RN.Entidades.RelacaoPessoa();
                            var relacaoPessoaIrmao = new RN.Entidades.RelacaoPessoa();
                            var dataNascimento = grdBuscaIrmaos.GetRowValues(rowIndex, "DT_NASC");
                            var nomePai = grdBuscaIrmaos.GetRowValues(rowIndex, "NOME_PAI");
                            string nomeMae = grdBuscaIrmaos.GetRowValues(rowIndex, "NOME_MAE").ToString();

                            pessoaSelecionada = Aluno.RetornaPessoa(matricSel.ToString());
                            relacaoPessoa.PessoaId = pessoa;
                            relacaoPessoa.ParenteId = pessoaSelecionada;
                            relacaoPessoaIrmao.PessoaId = pessoaSelecionada;
                            relacaoPessoaIrmao.ParenteId = pessoa;

                            if (!string.IsNullOrEmpty(dtDataNasc.Text))
                            {
                                int comparaDatas = DateTime.Compare(Convert.ToDateTime(dataNascimento), dtDataNasc.Date);
                                //Irmão gêmeo
                                if (comparaDatas == 0 && Convert.ToString(txtNomeMae.Text) != "NÃO DECLARADA" &&
                                    Convert.ToString(txtNomeMae.Text) != "NÃO DECLARADO" &&
                                    txtNomeMae.Text.Equals(nomeMae)
                                    && txtNomePai.Text.Equals(nomePai))
                                {
                                    relacaoPessoa.ParentescoId = 2;
                                    relacaoPessoaIrmao.ParentescoId = 2;
                                }
                                //Irmão
                                else
                                {
                                    relacaoPessoa.ParentescoId = 1;
                                    relacaoPessoaIrmao.ParentescoId = 1;
                                }
                                try
                                {
                                    RN.RelacaoPessoa.Inserir(relacaoPessoa);
                                    bool temIrmao = RN.RelacaoPessoa.ConsultaIrmao(relacaoPessoaIrmao);

                                    if (temIrmao == false)
                                    {
                                        RN.RelacaoPessoa.Inserir(relacaoPessoaIrmao);
                                    }

                                    var matricula = new LyAluno();
                                    {

                                        matricula.Aluno = (string)grdBuscaIrmaos.GetRowValues(rowIndex, "ALUNO");
                                    };

                                    matriculas.Add(matricula);


                                }
                                catch (Exception ex)
                                {
                                    {
                                        throw ex;
                                    }

                                }
                            }
                        }
                    }
                }

                CarregarGridPesquisaIrmaos();
                CarregarGridIrmaosCadasAntes(pessoa);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message + "SalvarIrmaos";
            }
        }

        private void CarregaBuscaIrmaos(LyPessoa dadosPessoa)
        {
            txtMae.Text = dadosPessoa.NomeMae;
            txtPai.Text = dadosPessoa.NomePai;
            hdnDataNascimento.Value = dadosPessoa.Dt_nasc.ToString();
            grdBuscaIrmaos.DataSource = null;
            grdBuscaIrmaos.DataBind();
        }

        private void CarregarGridIrmaosCadasAntes(decimal Parentesco)
        {
            try
            {
                DataTable dtIrmaos = new DataTable();
                var pessoa = Aluno.RetornaPessoa(txtAluno.Text);

                dtIrmaos = RN.Aluno.ConsultarFiliacaoAluno(pessoa);
                var mae = dtIrmaos.Rows[0]["NOME_MAE"].ToString();
                var pai = dtIrmaos.Rows[0]["NOME_PAI"].ToString();
                if (!string.IsNullOrEmpty(mae) || !string.IsNullOrEmpty(pai))
                {
                    if (Parentesco != 0 && Parentesco == pessoa)
                        grdBuscaIrmaosCadastrados.DataSource = RN.Aluno.ListarIrmaosPessoa(mae, pai, pessoa, Parentesco);
                    else
                        grdBuscaIrmaosCadastrados.DataSource = RN.Aluno.ListarIrmaosPessoaParente(mae, pai, pessoa, Parentesco);

                    grdBuscaIrmaosCadastrados.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message + "CarregarGridIrmaosCadasAntes";

            }
        }

        private void CarregarGridPesquisaIrmaos()
        {
            try
            {
                Regex regex = new Regex(@"\s{2,}");
                string NomeMae = regex.Replace(txtMae.Text.Trim().ToUpper(), " ");
                string NomePai = regex.Replace(txtPai.Text.Trim().ToUpper(), " ");
                string NomeIrmao = regex.Replace(txtNomeIrmao.Text.Trim().ToUpper(), " ");
                DateTime DataNascimento = Convert.ToDateTime(hdnDataNascimento.Value);
                var aluno = string.IsNullOrEmpty(tseAluno.DBValue.ToString()) ? txtAluno.Text : tseAluno.DBValue.ToString();
                bool maenaodeclar = chkNaoDeclarMae.Checked;
                bool painaodeclar = chkNaoDeclarPai.Checked;
                var pessoa = Aluno.RetornaPessoa(aluno);

                if (!string.IsNullOrEmpty(NomeMae) || !string.IsNullOrEmpty(NomePai) || !string.IsNullOrEmpty(dtDataNasc.Text)
                    && !string.IsNullOrEmpty(aluno) || !maenaodeclar || !painaodeclar)
                {
                    grdBuscaIrmaos.DataSource = RN.Aluno.ListarIrmaos(NomeMae, NomePai, NomeIrmao, DataNascimento, aluno, maenaodeclar, painaodeclar, pessoa);
                    grdBuscaIrmaos.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message + "CarregarGridPesquisaIrmaos";
            }
        }

        private void CarregaDadosInscricaoCompartilhadas()
        {
            _tipoOperacao = TipoOperacao.CadastrarAlunoCompartilhadasBusca;
            pntabDadosEscolares.Visible = true;

            ControlarTipoOperacao();
        }

        protected void btnProsseguirMatricula_Click(object sender, EventArgs e)
        {
            try
            {
                RN.Agenda.Agenda rnAgenda = new Techne.Lyceum.RN.Agenda.Agenda();
                int idEventoBloqueioaMatricula = Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.BloqueioCadastroMatricula);
                DadosParticipacao bloqueio = new DadosParticipacao();
                RN.ControleVaga rnControleVaga = new ControleVaga();
                RN.Perfil rnPerfil = new Perfil();
                int vagasLiberadas = 0;
                int vagasUtilizadas = 0;

                if ((!tseUnidadeEnsinoMatricula.DBValue.IsNull && tseUnidadeEnsinoMatricula.IsValidDBValue) && !ddlAnoMatricula.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlPeriodoMatricula.SelectedValue.IsNullOrEmptyOrWhiteSpace() && (!tseCursoMatricula.DBValue.IsNull && tseCursoMatricula.IsValidDBValue) && !cmbTurnoMatricula.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !cmbCurriculoMatricula.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !cmbSerieMatricula.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    if (rnControleVaga.PartipaMatriculaFacilPor(tseUnidadeEnsinoMatricula.DBValue.ToString(), int.Parse(ddlAnoMatricula.SelectedValue), int.Parse(ddlPeriodoMatricula.SelectedValue), tseCursoMatricula.DBValue.ToString(), Convert.ToInt32(cmbSerieMatricula.SelectedValue), cmbTurnoMatricula.SelectedValue))
                    {
                        lblMensagem.Text = "Nova Matrícula não permitida. Esta escola/curso/série/turno está participando do Matrícula Fácil.";
                        pnlBuscaAlunoNovo.Visible = false;
                        return;
                    }

                    //Verificar se tem vaga no curso / serie / turno / ano / semestre
                    vagasLiberadas = rnControleVaga.ObtemVagasLiberadasTotalPor(
                        tseUnidadeEnsinoMatricula.DBValue.ToString(),
                        Convert.ToInt32(ddlAnoMatricula.SelectedValue),
                        Convert.ToInt32(ddlPeriodoMatricula.SelectedValue),
                        Convert.ToInt32(cmbSerieMatricula.SelectedValue),
                        tseCursoMatricula.DBValue.ToString(),
                        cmbTurnoMatricula.SelectedValue);

                    vagasUtilizadas = rnControleVaga.ObtemVagasUtilizadasTotalPor(
                        tseUnidadeEnsinoMatricula.DBValue.ToString(),
                        Convert.ToInt32(ddlAnoMatricula.SelectedValue),
                        Convert.ToInt32(ddlPeriodoMatricula.SelectedValue),
                        Convert.ToInt32(cmbSerieMatricula.SelectedValue),
                        tseCursoMatricula.DBValue.ToString(),
                        cmbTurnoMatricula.SelectedValue);

                    if (vagasLiberadas <= vagasUtilizadas)
                    {
                        lblMensagem.Text = "Não será possível cadastrar o aluno , pois não existem vagas disponíveis para o curso/série e turno!";
                        pnlBuscaAlunoNovo.Visible = false;
                        return;
                    }

                    bloqueio = rnAgenda.VerificaEventoPor(idEventoBloqueioaMatricula, int.Parse(ddlAnoMatricula.SelectedValue), int.Parse(ddlPeriodoMatricula.SelectedValue), tseUnidadeEnsinoMatricula.DBValue.ToString(), tseCursoMatricula.DBValue.ToString());

                    if (((bloqueio.ParticipaCurso && bloqueio.ParticipaUnidade) || bloqueio.ParticipaTotal) && !rnPerfil.PossuiPerfilMatriculaTransferenciaPeriodoBloqueioPor(User.Identity.Name))
                    {
                        lblMensagem.Text = string.Format("Novas matrículas estão bloqueadas no período de {0} á {1} para este Período, Unidade e/ou Curso de acordo com a Agenda da SEEDUC.", bloqueio.DataInicio.ToString("dd/MM/yyyy"), bloqueio.DataFim.ToString("dd/MM/yyyy"));
                        pnlBuscaAlunoNovo.Visible = false;
                    }
                    else
                    {
                        pnlBuscaAlunoNovo.Visible = true;
                    }

                    pnlNovaMatricula.Visible = false;

                }
                else
                {
                    lblMensagem.Text = "Para prosseguir é necessário escolher um ano/período/unidade escolar/curso/turno/currículo/série.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaPaisNascimento()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            // txtPaisNasc é preenchido diretamente via RN.Endereco.ObterPais
            // Pré-selecionar Brasil como padrão
            txtPaisNasc.Text = "BRASIL";
        }

        private void CarregaEstadoCivil()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlEst_Civil.Items.Clear();
            ddlEst_Civil.DataSource = RN.Util.Cache.CarregaItemTabelaGeralPor(RN.Util.Cache.EstadoCivil);
            ddlEst_Civil.DataBind();
            ddlEst_Civil.Items.Insert(0, item);
        }

        private void CarregaNacionalidade()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlNacionalidade.Items.Clear();
            ddlNacionalidade.DataSource = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.Nacionalidade, RN.Basico.QueryListaNacionalidades);
            ddlNacionalidade.DataBind();
            ddlNacionalidade.Items.Insert(0, item);

            item = ddlNacionalidade.Items.FindByText("BRASILEIRA");
            if (item != null)
            {
                ddlNacionalidade.ClearSelection();
                item.Selected = true;
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

        private void CarregaOutroEnsino()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlOutroEnsino.Items.Clear();
            ddlOutroEnsino.DataSource = RN.Util.Cache.CarregaItemTabelaGeralPor(RN.Util.Cache.OutroEnsino);
            ddlOutroEnsino.DataBind();
            ddlOutroEnsino.Items.Insert(0, item);
        }

        private void CarregaEtnia()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlEtnia.Items.Clear();
            ddlEtnia.DataSource = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.Etnia, RN.Basico.QueryListaEtniaAtiva);
            ddlEtnia.DataBind();
            ddlEtnia.Items.Insert(0, item);
        }

        private void CarregaCredo()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlCredo.Items.Clear();
            ddlCredo.DataSource = RN.Util.Cache.CarregaItemTabelaGeralPor(RN.Util.Cache.Credo);
            ddlCredo.DataBind();
            ddlCredo.Items.Insert(0, item);
        }

        private void CarregaTipoSanguineo()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlTipoSanguineo.Items.Clear();
            ddlTipoSanguineo.DataSource = RN.Util.Cache.CarregaItemTabelaGeralPor(RN.Util.Cache.TipoSanguineo);
            ddlTipoSanguineo.DataBind();
            ddlTipoSanguineo.Items.Insert(0, item);
        }

        private void CarregaRGTipoPessoa()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlRGTipoPessoa.Items.Clear();
            ddlRGTipoPessoa.DataSource = RN.Util.Cache.CarregaItemTabelaGeralPor(RN.Util.Cache.RgTipoPessoa);
            ddlRGTipoPessoa.DataBind();
            ddlRGTipoPessoa.Items.Insert(0, item);
        }

        private void CarregaRGEmissor()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            cmbRGEmissor.Items.Clear();
            cmbRGEmissor.DataSource = RN.Util.Cache.CarregaItemTabelaGeralPor(RN.Util.Cache.RgEmissor);
            cmbRGEmissor.DataBind();
            cmbRGEmissor.Items.Insert(0, item);
        }

        private void CarregaUfRg()
        {
            ListItem item = new ListItem("Selecione", string.Empty);
            Object dtUf;

            dtUf = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.Uf, RN.Basico.QueryListaUF);

            cmbRGUF.Items.Clear();
            cmbRGUF.DataSource = dtUf;
            cmbRGUF.DataBind();
            cmbRGUF.Items.Insert(0, item);
        }

        private void CarregaUfCertNasc()
        {
            ListItem item = new ListItem("Selecione", string.Empty);
            Object dtUf;

            dtUf = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.Uf, RN.Basico.QueryListaUF);

            ddDOC_CertNasc_Uf.Items.Clear();
            ddDOC_CertNasc_Uf.DataSource = dtUf;
            ddDOC_CertNasc_Uf.DataBind();
            ddDOC_CertNasc_Uf.Items.Insert(0, item);
        }

        private void CarregaUFNaturalidade()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            // txtUFNascimento é preenchido automaticamente via tseNaturalidade_Changed
        }

        private void CarregaTipoIngresso()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlTipoIngresso.Items.Clear();
            ddlTipoIngresso.DataSource = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.TipoIngresso, RN.Basico.QueryListaTipoIngresso);
            ddlTipoIngresso.DataBind();
            ddlTipoIngresso.Items.Insert(0, item);
        }

        private void CarregaCartorio()
        {
            if (!string.IsNullOrEmpty(ddlMunicipioCartorio.SelectedValue))
            {
                RN.Basico rnBasico = new Techne.Lyceum.RN.Basico();
                ListItem item = new ListItem("Selecione", string.Empty);

                ddlCartorio.Items.Clear();
                ddlCartorio.DataSource = rnBasico.ObtemListaCartorioPor(ddlUFCartorio.SelectedValue, ddlMunicipioCartorio.SelectedValue.ToString());
                ddlCartorio.DataBind();
                ddlCartorio.Items.Insert(0, item);
            }
        }

        private void CarregaMunicipioCartorio()
        {
            if (!string.IsNullOrEmpty(ddlUFCartorio.SelectedValue))
            {
                RN.Basico rnBasico = new Techne.Lyceum.RN.Basico();
                ListItem item = new ListItem("Selecione", string.Empty);

                ddlMunicipioCartorio.Items.Clear();
                ddlMunicipioCartorio.DataSource = rnBasico.ObtemListaMunicipioCartorioPor(ddlUFCartorio.SelectedValue);
                ddlMunicipioCartorio.DataBind();
                ddlMunicipioCartorio.Items.Insert(0, item);
            }
        }

        private void CarregaAnoIngresso()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            cmbAnoIngresso.Items.Clear();
            cmbAnoIngresso.DataSource = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.ProximosAnosLetivos, RN.PeriodoLetivo.QueryListaProximosAnos);
            cmbAnoIngresso.DataBind();
            cmbAnoIngresso.Items.Insert(0, item);
        }

        private void CarregaAnoMatricula()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlAnoMatricula.Items.Clear();
            ddlAnoMatricula.DataSource = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.ProximosAnosLetivos, RN.PeriodoLetivo.QueryListaProximosAnos);
            ddlAnoMatricula.DataBind();
            ddlAnoMatricula.Items.Insert(0, item);
        }

        private void CarregaPeriodoIngresso()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            cmbSemIngresso.Items.Clear();
            cmbSemIngresso.DataSource = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.PeriodoLetivo, RN.PeriodoLetivo.QueryListaPeriodos);
            cmbSemIngresso.DataBind();
            cmbSemIngresso.Items.Insert(0, item);
        }

        private void CarregaPeriodoMatricula()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlPeriodoMatricula.Items.Clear();
            ddlPeriodoMatricula.DataSource = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.PeriodoLetivo, RN.PeriodoLetivo.QueryListaPeriodos);
            ddlPeriodoMatricula.DataBind();
            ddlPeriodoMatricula.Items.Insert(0, item);
        }

        private void CarregaUFCartorio()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlUFCartorio.Items.Clear();
            ddlUFCartorio.DataSource = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.UfCartorio, RN.Basico.QueryListaUFCartorio);
            ddlUFCartorio.DataBind();
            ddlUFCartorio.Items.Insert(0, item);
        }

        private void CarregaRedeEnsinoOrigem()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlRedeEnsinoOrigem.Items.Clear();
            ddlRedeEnsinoOrigem.DataSource = RN.Util.Cache.CarregaItemTabelaGeralPor(RN.Util.Cache.RedeEnsinoOrigem);
            ddlRedeEnsinoOrigem.DataBind();
            ddlRedeEnsinoOrigem.Items.Insert(0, item);
        }

        private void CarregaModalidade()
        {
            ListItem item = new ListItem("Selecione", string.Empty);
            Object objModalidade = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.Modalidade, RN.Curso.QueryListaModalidadeCurso);

            ddlModalidade.Items.Clear();
            ddlModalidade.DataSource = objModalidade;
            ddlModalidade.DataBind();
            ddlModalidade.Items.Insert(0, item);

            ddlModalidadeMatricula.Items.Clear();
            ddlModalidadeMatricula.DataSource = objModalidade;
            ddlModalidadeMatricula.DataBind();
            ddlModalidadeMatricula.Items.Insert(0, item);
        }

        private void CarregaNivel()
        {
            ListItem item = new ListItem("Selecione", string.Empty);
            Object objNivel = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.Nivel, RN.Curso.QueryListaTipoCurso);

            ddlNivel.Items.Clear();
            ddlNivel.DataSource = objNivel;
            ddlNivel.DataBind();
            ddlNivel.Items.Insert(0, item);

            ddlNivelMatricula.Items.Clear();
            ddlNivelMatricula.DataSource = objNivel;
            ddlNivelMatricula.DataBind();
            ddlNivelMatricula.Items.Insert(0, item);
        }

        private void CarregaTurno()
        {
            RN.Turno rnTurno = new Turno();
            RN.Curriculo rnCurriculo = new Techne.Lyceum.RN.Curriculo();

            ListItem item = new ListItem("Selecione", string.Empty);
            cmbTurno.Items.Clear();

            if ((_tipoOperacao.Equals(TipoOperacao.Novo) || _tipoOperacao.Equals(TipoOperacao.NovaMatricula)) && (tseCurso.IsValidDBValue && !tseCurso.DBValue.IsNull))
            {
                cmbTurno.DataSource = rnCurriculo.ObtemListaTurnoPor(tseCurso.DBValue.ToString());

            }
            else
            {
                cmbTurno.DataSource = rnTurno.ListaTurnosPor();
            }
            cmbTurno.DataBind();
            cmbTurno.Items.Insert(0, item);
        }

        private void CarregaSerie()
        {
            RN.Serie rnSerie = new Serie();

            bool bloquearSeriesIniciaisAluno = Convert.ToBoolean(ConfigurationManager.AppSettings["BloquearSeriesIniciaisTransfAluno"] ?? "false");

            ListItem item = new ListItem("Selecione", string.Empty);
            cmbSerie.Items.Clear();

            if ((tseCurso.IsValidDBValue && !tseCurso.DBValue.IsNull) && !cmbTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !cmbCurriculo.SelectedValue.IsNullOrEmptyOrWhiteSpace())
            {
                cmbSerie.DataSource = rnSerie.ObtemSeriesAlunoNovoPor(tseCurso.DBValue.ToString(), cmbTurno.SelectedValue.ToString(), cmbCurriculo.SelectedValue, ((_tipoOperacao.Equals(TipoOperacao.Novo) || _tipoOperacao.Equals(TipoOperacao.NovaMatricula)) ? bloquearSeriesIniciaisAluno : false));
            }
            cmbSerie.DataBind();
            cmbSerie.Items.Insert(0, item);
        }

        protected void hplLinkNomeSocial_Click(object sender, EventArgs e)
        {
            try
            {
                string FilePath = HttpContext.Current.Server.MapPath(@"~\Arquivos\SaibaMaisNomeSocial.pdf");
                WebClient client = new WebClient();
                Byte[] buffer = client.DownloadData(FilePath);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", buffer.Length.ToString());
                Response.BinaryWrite(buffer);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void hplLinkNecEspecial_Click(object sender, EventArgs e)
        {
            try
            {
                string FilePath = HttpContext.Current.Server.MapPath(@"~\Arquivos\SaibaMaisNecEspecial.pdf");
                WebClient client = new WebClient();
                Byte[] buffer = client.DownloadData(FilePath);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", buffer.Length.ToString());
                Response.BinaryWrite(buffer);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnConfirmaFoto_Click(object sender, EventArgs e)
        {
            RN.Util.Imagem rnImagem = new RN.Util.Imagem();

            try
            {
                if (hdnFotoUrl.Value != null)
                {

                    var base64Data = Regex.Match(hdnFotoUrl.Value, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                    if (!base64Data.IsNullOrEmptyOrWhiteSpace())
                    {
                        byte[] binData = Convert.FromBase64String(base64Data);
                        byte[] foto = rnImagem.RedimencionaImagemPor(binData, 400, 400);
                        System.Drawing.Image.FromStream(new MemoryStream(foto));
                        bimgFotoPessoa.ContentBytes = foto;
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Problema na captura da foto. Favor verificar.');", true);
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Antes de confirmar capture a foto.');", true);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public byte[] EncodeToBase64(string texto)
        {
            try
            {
                byte[] textoAsBytes = Encoding.ASCII.GetBytes(texto);
                // string resultado = System.Convert.ToBase64String(textoAsBytes);
                // return resultado;
                return textoAsBytes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void btnUsarFoto_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["imageBytes"] != null)
                {
                    byte[] fotoPessoa = (byte[])Session["imageBytes"];

                    System.Drawing.Image.FromStream(new MemoryStream(fotoPessoa));
                    //bimgFoto.ContentBytes = fotoPessoa;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        [WebMethod(EnableSession = true)]

        public static string GetCapturedImage()
        {
            return string.Empty;
        }

        protected void chkNaoSeAplica_CheckedChanged(object sender, EventArgs e)
        {
            ValidaLocalizacaoDiferenciada();
        }

        private void ValidaLocalizacaoDiferenciada()
        {
            if (chkNaoSeAplica.Checked)
            {
                chkQuilombos.Checked = !chkNaoSeAplica.Checked;
                chkAreaTradicional.Checked = !chkNaoSeAplica.Checked;
                chkAreaAssentamento.Checked = !chkNaoSeAplica.Checked;
                chkTerraIndigena.Checked = !chkNaoSeAplica.Checked;

                chkQuilombos.Enabled = !chkNaoSeAplica.Checked;
                chkAreaTradicional.Enabled = !chkNaoSeAplica.Checked;
                chkAreaAssentamento.Enabled = !chkNaoSeAplica.Checked;
                chkTerraIndigena.Enabled = !chkNaoSeAplica.Checked;
            }
            else
            {
                HabilitaLocalizacaoDiferenciada();
            }
        }

        private void HabilitaLocalizacaoDiferenciada()
        {
            if (!chkNaoSeAplica.Checked)
            {
                Util.Utils.HabilitaDesabilitaControlesWeb(
                    new WebControl[] {
                        chkQuilombos, chkTerraIndigena, chkAreaAssentamento, chkAreaTradicional
                    }, true
                );
            }

            chkNaoSeAplica.Enabled = true;
        }

        protected void btnSalvarTransporte_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Aluno rnAluno = new Aluno();

                string modais = string.Empty;
                bool transporte = false;
                bool onibus = false;
                int cont_transp = 0;
                string transp_rodov = string.Empty;
                string transp_aquav = string.Empty;
                string transp_onibus = string.Empty;

                foreach (ListItem item in chkModais.Items)
                {
                    if (item.Selected && item.Value != string.Empty)
                    {
                        modais += item.Value;
                        modais += ";";

                        if (item.Value == "5") //TRANSPORTE RURAL
                        {
                            transporte = true;
                        }
                    }

                    if (item.Selected && item.Value != string.Empty)
                    {
                        modais += item.Value;
                        modais += ";";

                        if (item.Value == "2") //OPERADORA ÔNIBUS
                        {
                            onibus = true;
                        }
                    }
                }

                if (transporte)
                {
                    foreach (ListItem item in chkRodoviario.Items)
                    {
                        if (item.Selected)
                        {
                            cont_transp++;
                            transp_rodov += item.Value;
                            transp_rodov += ";";
                        }
                    }

                    foreach (ListItem item in chkAquaviario.Items)
                    {
                        if (item.Selected)
                        {
                            cont_transp++;
                            transp_aquav += item.Value;
                            transp_aquav += ";";
                        }
                    }
                }

                if (onibus)
                {
                    foreach (ListItem item in chkOnibus.Items)
                    {
                        if (item.Selected)
                        {
                            cont_transp++;
                            transp_onibus += item.Value;
                            transp_onibus += ";";
                        }
                    }
                }

                var flPessoa = new LyFlPessoa
                {
                    Pessoa = !txtPessoa.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(this.txtPessoa.Text) : -1,
                    FlField04 = !ddlGratuidade.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlGratuidade.SelectedValue : null,
                    FlField05 = !modais.IsNullOrEmptyOrWhiteSpace() ? modais : null,
                    FlField10 = !ddlPoderPublicoTransp.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlPoderPublicoTransp.SelectedValue : null,
                    FlField11 = !transp_rodov.IsNullOrEmptyOrWhiteSpace() ? transp_rodov : null,
                    FlField12 = !transp_aquav.IsNullOrEmptyOrWhiteSpace() ? transp_aquav : null,
                    FlField20 = !transp_onibus.IsNullOrEmptyOrWhiteSpace() ? transp_onibus : null,
                };

                validacao = rnAluno.ValidaTransporte(!cmbNecessidadeEspecial.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(cmbNecessidadeEspecial.SelectedValue) : -1, txtEmailTransp.Text, flPessoa);

                if (validacao.Valido)
                {
                    rnAluno.SalvaTransporte(flPessoa);
                    lblMensagem.Text = "Dados do Transporte Escolar salvo com sucesso.";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Dados do Transporte Escolar salvo com sucesso.');", true);
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

        private void LimpaCamposAEDH()
        {
            hdnIdAEDH.Value = string.Empty;
            hdnCensoAEDH.Value = string.Empty;
            hdnDataInicio.Value = string.Empty;
            hdnDataFim.Value = string.Empty;
            lblAnoAEDH.Text = string.Empty;
            lblPeriodoAEDH.Text = string.Empty;
            lblTurmaAEDH.Text = string.Empty;
            dtInicioAEDH.Text = string.Empty;
            dtFimAEDH.Text = string.Empty;
            rblLaudoEntregue.ClearSelection();
            rblReqAtendEntregue.ClearSelection();
            rblPlEspecial.ClearSelection();
            txtNumSEI.Text = string.Empty;
            rblProrrogacao.ClearSelection();
            rblTpAtendimento.ClearSelection();
            txtDescricaoAEDH.Text = string.Empty;
            hdnDataMatricula.Value = string.Empty;
        }

        protected void HabilitaPnlNovoAEDH(object sender, EventArgs e)
        {
            try
            {
                RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();
                RN.DTOs.DadosEnturmacaoAluno dados = new Techne.Lyceum.RN.DTOs.DadosEnturmacaoAluno();
                RN.PeriodoLetivo rnPeriodoLetivo = new PeriodoLetivo();

                pnAEDH.Visible = true;
                LimpaCamposAEDH();

                dados = rnMatricula.ObtemMatriculaPrincipalAtivaPor(tseAluno.DBValue.ToString());

                if (!dados.Turma.IsNullOrEmptyOrWhiteSpace())
                {
                    lblAnoAEDH.Text = dados.Ano.ToString();
                    lblPeriodoAEDH.Text = dados.Periodo.ToString();
                    lblTurmaAEDH.Text = dados.Turma;
                    hdnCensoAEDH.Value = dados.Censo;
                    hdnDataMatricula.Value = dados.DataMatricula.ToShortDateString();


                    var datas = rnPeriodoLetivo.ObtemDataInicioFimAulaPor(Convert.ToInt32(lblAnoAEDH.Text), Convert.ToInt32(lblPeriodoAEDH.Text));

                    hdnDataInicio.Value = datas[0];
                    hdnDataFim.Value = datas[1];

                    dtInicioAEDH.MinDate = Convert.ToDateTime(hdnDataInicio.Value);
                    dtInicioAEDH.MaxDate = Convert.ToDateTime(hdnDataFim.Value);

                    dtFimAEDH.MinDate = Convert.ToDateTime(hdnDataInicio.Value);
                    dtFimAEDH.MaxDate = Convert.ToDateTime(hdnDataFim.Value);

                    if (ddlOutroEnsino.SelectedValue != "3")
                    {
                        if (ddlOutroEnsino.SelectedValue == "1")
                        {
                            rblTpAtendimento.SelectedValue = "Hospitalar";
                        }
                        else if (ddlOutroEnsino.SelectedValue == "2")
                        {
                            rblTpAtendimento.SelectedValue = "Domiciliar";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnCancelarAEDH_Click(object sender, EventArgs e)
        {
            try
            {
                LimpaCamposAEDH();
                pnAEDH.Visible = false;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSalvarAEDH_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.RecursosHumanos.AtendimentoOutroEspaco rnAtendimentoOutroEspaco = new Techne.Lyceum.RN.RecursosHumanos.AtendimentoOutroEspaco();
                RN.RecursosHumanos.Entidades.AtendimentoOutroEspaco atendimento = new Techne.Lyceum.RN.RecursosHumanos.Entidades.AtendimentoOutroEspaco();

                atendimento.AtendimentoOutroEspacoId = !hdnIdAEDH.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnIdAEDH.Value) : 0;
                atendimento.Ano = !lblAnoAEDH.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(lblAnoAEDH.Text) : -1;
                atendimento.Periodo = (!lblAnoAEDH.Text.IsNullOrEmptyOrWhiteSpace()) ? Convert.ToInt32(lblPeriodoAEDH.Text) : -1;
                atendimento.Turma = !lblTurmaAEDH.Text.IsNullOrEmptyOrWhiteSpace() ? lblTurmaAEDH.Text : null;
                atendimento.Aluno = (!this.tseAluno.DBValue.IsNull && this.tseAluno.IsValidDBValue) ? Convert.ToString(tseAluno.DBValue) : null;
                atendimento.Censo = !hdnCensoAEDH.Value.IsNullOrEmptyOrWhiteSpace() ? hdnCensoAEDH.Value : null;
                atendimento.DataInicio = !dtInicioAEDH.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(dtInicioAEDH.Text) : DateTime.MinValue;
                atendimento.DataFim = !dtFimAEDH.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(dtFimAEDH.Text) : DateTime.MinValue;
                atendimento.Descricao = !txtDescricaoAEDH.Text.IsNullOrEmptyOrWhiteSpace() ? txtDescricaoAEDH.Text.Trim() : null;
                atendimento.Laudo = !rblLaudoEntregue.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblLaudoEntregue.SelectedValue == "1" ? true : false) : (bool?)null;
                atendimento.NumeroSei = !txtNumSEI.Text.IsNullOrEmptyOrWhiteSpace() ? txtNumSEI.Text.Trim() : null;
                atendimento.PlanoEspecial = !rblPlEspecial.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblPlEspecial.SelectedValue == "1" ? true : false) : (bool?)null;
                atendimento.Prorrogacao = !rblProrrogacao.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblProrrogacao.SelectedValue == "1" ? true : false) : (bool?)null;
                atendimento.Requerimento = !rblReqAtendEntregue.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblReqAtendEntregue.SelectedValue == "1" ? true : false) : (bool?)null;
                atendimento.Tipo = !rblTpAtendimento.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblTpAtendimento.SelectedValue : null;
                atendimento.UsuarioId = User.Identity.Name;

                validacao = rnAtendimentoOutroEspaco.Valida(atendimento, atendimento.AtendimentoOutroEspacoId == 0 ? true : false, Convert.ToDateTime(hdnDataMatricula.Value));

                if (validacao.Valido)
                {
                    if (atendimento.AtendimentoOutroEspacoId == 0)
                    {
                        rnAtendimentoOutroEspaco.Insere(atendimento);
                        lblMensagem.Text = "Escolarização em Outros Espaços cadastrada com sucesso.";
                    }
                    else
                    {
                        rnAtendimentoOutroEspaco.Atualiza(atendimento);
                        lblMensagem.Text = "Escolarização em Outros Espaços atualizado com sucesso.";
                    }

                    grdAEDH.DataBind();
                    LimpaCamposAEDH();
                    pnAEDH.Visible = false;

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

        public void Delete(object ATENDIMENTOOUTROESPACOID)
        {
        }

        protected void grdAEDH_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RecursosHumanos.AtendimentoOutroEspaco rnAtendimentoOutroEspaco = new Techne.Lyceum.RN.RecursosHumanos.AtendimentoOutroEspaco();
            int Id = 0;

            Id = Convert.ToInt32(e.Keys["ATENDIMENTOOUTROESPACOID"]);

            validacao = rnAtendimentoOutroEspaco.ValidaRemocao(Id, User.Identity.Name);

            if (validacao.Valido)
            {
                rnAtendimentoOutroEspaco.Remove(Id);
                grdAEDH.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

        protected void grdAEDH_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdAEDH.Settings.ShowFilterRow = false;
        }

        protected void grdAEDH_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdAEDH);
            AcessoGrid();
        }

        protected void AcessoGrid()
        {
            if (grdAEDH != null)
            {
                HtmlInputImage img = (HtmlInputImage)grdAEDH.FindHeaderTemplateControl(grdAEDH.Columns[""], "btnNovoGridAEDH");

                if (img != null)
                {
                    img.Visible = Permission.AllowInsert;
                }
            }
        }

        protected void grdAEDH_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            if (e.ButtonID == "btnEditarAEDH")
            {
                try
                {

                    RN.PeriodoLetivo rnPeriodoLetivo = new PeriodoLetivo();
                    RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();

                    LimpaCamposAEDH();
                    hdnIdAEDH.Value = Convert.ToString(grdAEDH.GetRowValues(e.VisibleIndex, "ATENDIMENTOOUTROESPACOID"));
                    hdnCensoAEDH.Value = Convert.ToString(grdAEDH.GetRowValues(e.VisibleIndex, "CENSO"));

                    lblAnoAEDH.Text = Convert.ToString(grdAEDH.GetRowValues(e.VisibleIndex, "ANO"));
                    lblPeriodoAEDH.Text = Convert.ToString(grdAEDH.GetRowValues(e.VisibleIndex, "PERIODO"));
                    lblTurmaAEDH.Text = Convert.ToString(grdAEDH.GetRowValues(e.VisibleIndex, "TURMA"));
                    dtInicioAEDH.Date = Convert.ToDateTime(grdAEDH.GetRowValues(e.VisibleIndex, "DATAINICIO"));
                    dtFimAEDH.Date = Convert.ToDateTime(grdAEDH.GetRowValues(e.VisibleIndex, "DATAFIM"));
                    rblTpAtendimento.SelectedValue = Convert.ToString(grdAEDH.GetRowValues(e.VisibleIndex, "TIPO"));
                    rblLaudoEntregue.SelectedValue = Convert.ToBoolean(grdAEDH.GetRowValues(e.VisibleIndex, "LAUDO")) == true ? "1" : "0";
                    rblReqAtendEntregue.SelectedValue = Convert.ToBoolean(grdAEDH.GetRowValues(e.VisibleIndex, "REQUERIMENTO")) == true ? "1" : "0";
                    rblPlEspecial.SelectedValue = Convert.ToBoolean(grdAEDH.GetRowValues(e.VisibleIndex, "PLANOESPECIAL")) == true ? "1" : "0"; ;
                    txtNumSEI.Text = Convert.ToString(grdAEDH.GetRowValues(e.VisibleIndex, "NUMEROSEI"));
                    rblProrrogacao.SelectedValue = Convert.ToBoolean(grdAEDH.GetRowValues(e.VisibleIndex, "PRORROGACAO")) == true ? "1" : "0"; ;
                    txtDescricaoAEDH.Text = Convert.ToString(grdAEDH.GetRowValues(e.VisibleIndex, "DESCRICAO"));
                    hdnDataMatricula.Value = rnMatricula.ObtemDataMatriculaEnturmacaoPor(Convert.ToInt32(lblAnoAEDH.Text), Convert.ToInt32(lblPeriodoAEDH.Text), lblTurmaAEDH.Text, tseAluno.DBValue.ToString()).ToShortDateString();

                    var datas = rnPeriodoLetivo.ObtemDataInicioFimAulaPor(Convert.ToInt32(lblAnoAEDH.Text), Convert.ToInt32(lblPeriodoAEDH.Text));

                    hdnDataInicio.Value = datas[0];
                    hdnDataFim.Value = datas[1];

                    dtInicioAEDH.MinDate = Convert.ToDateTime(hdnDataInicio.Value);
                    dtInicioAEDH.MaxDate = Convert.ToDateTime(hdnDataFim.Value);

                    dtFimAEDH.MinDate = Convert.ToDateTime(hdnDataInicio.Value);
                    dtFimAEDH.MaxDate = Convert.ToDateTime(hdnDataFim.Value);

                    pnAEDH.Visible = true;

                }
                catch (Exception ex)
                {
                    lblMensagem.Text = ex.Message;
                }
            }

            if (e.ButtonID == "btnExcluirAEDH")
            {

                hdnIdAEDH.Value = Convert.ToString(grdAEDH.GetRowValues(e.VisibleIndex, "ATENDIMENTOOUTROESPACOID"));

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopupAEDH();", true);

            }
        }

        protected void btnSimAEDH_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.RecursosHumanos.AtendimentoOutroEspaco rnAtendimentoOutroEspaco = new Techne.Lyceum.RN.RecursosHumanos.AtendimentoOutroEspaco();
                int Id = 0;

                Id = Convert.ToInt32(hdnIdAEDH.Value);

                validacao = rnAtendimentoOutroEspaco.ValidaRemocao(Id, User.Identity.Name);

                if (validacao.Valido)
                {
                    rnAtendimentoOutroEspaco.Remove(Id);
                    grdAEDH.DataBind();
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    grdAEDH.CancelEdit();
                }
                this.pucConfirmarAEDH.ShowOnPageLoad = false;
            }

            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnNaoAEDH_Click(object sender, EventArgs e)
        {
            this.pucConfirmarAEDH.ShowOnPageLoad = false;
            grdAEDH.CancelEdit();
        }

        protected void grdAEDH_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;

            if (e.CellType == GridViewTableCommandCellType.Filter)
            {
                e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                return;
            }

            if (e.ButtonID == "btnExcluirAEDH")
            {
                //Verifica se ainda não foi aprovado
                if (hdnPerfilAEDH.Value == "S")
                {
                    e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.True;
                }
            }
        }

        protected void grdAEDH_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            RN.Perfil rnPerfi = new Perfil();

            if (e.ButtonType == ColumnCommandButtonType.Delete)
            {
                if (rnPerfi.PossuiPerfilExclusaoAEDHPor(User.Identity.Name))
                {
                    e.Visible = true;
                }
                else
                {
                    e.Visible = false;
                }
            }
        }

        protected void ddlOutroEnsino_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlOutroEnsino.SelectedValue != "3")
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Para alunos que recebem escolarização em outros espaços diferentes da escola, preencha a aba \"AEDH- ESCOLARIZAÇÃO EM OUTROS ESPAÇOS\".');", true);
            }
        }

        private void AlternarControlesNaturalidade(bool nascidoForaDoBrasil)
        {
            tseNaturalidade.Visible = !nascidoForaDoBrasil;
            tseNaturalidadeEstrangeira.Visible = nascidoForaDoBrasil;

            lblPaisNasc.Visible = nascidoForaDoBrasil;
            txtPaisNasc.Visible = nascidoForaDoBrasil;

            if (!nascidoForaDoBrasil)
            {
                // Limpa dados estrangeiros ao voltar para "Não"
                tseNaturalidadeEstrangeira.ResetValue();
                txtPaisNasc.Text = string.Empty;
                txtUFNascimento.Text = string.Empty;
            }
        }

        protected void ddlNacionalidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                bool eBrasileiro = ddlNacionalidade.SelectedItem.Text == "BRASILEIRA";

                if (ddlNacionalidade.SelectedItem != null)
                {
                    if (!eBrasileiro)
                    {
                        // Estrangeiro: força "nascido fora do Brasil"
                        AlternarControlesNaturalidade(true);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseNaturalidadeEstrangeira_Changed(object sender, EventArgs args)
        {
            try
            {
                if (tseNaturalidadeEstrangeira.IsValidDBValue
                    && !tseNaturalidadeEstrangeira.DBValue.IsNull)
                {
                    txtUFNascimento.Text = tseNaturalidadeEstrangeira["ESTADO"].ToString();
                    txtPaisNasc.Text = tseNaturalidadeEstrangeira["PAIS"].ToString();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}