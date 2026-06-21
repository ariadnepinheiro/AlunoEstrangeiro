using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Web;
using DevExpress.Web.ASPxEditors;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.ProcessoSeletivo
{
    [NavUrl("~/ProcessoSelectivo/ConcursoDocentes.aspx"),
    ControlText("Processo Seletivo de Docentes"),
    Title("Processos Seletivos"),]

    public partial class ConcursoDocentes : TPage
    {

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
            get { return (TipoOperacao)ViewState["_tipoOperacao"]; }
            set { ViewState["_tipoOperacao"] = value; }
        }

        private string vsMunicipio
        {
            get { return (string)ViewState["vsMunicipio"]; }
            set { ViewState["vsMunicipio"] = value; }
        }

        private string vsConcurso
        {
            get { return (string)Session["vsConcurso"]; }
            set { Session["vsConcurso"] = value; }
        }

        private string AREA_INTEGRADA_DOCII = "039";
        #endregion

        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }
        public object ListaTipoDocumento()
        {
            RN.RecursosHumanos.TipoDocumento rnTipoDocumento = new Techne.Lyceum.RN.RecursosHumanos.TipoDocumento();

            return rnTipoDocumento.ListaAtivoPor();
        }

        public object ListaTipoDocumentoConcurso(object concurso)
        {
            RN.RecursosHumanos.TipoDocumentoConcurso rnTipoDocumentoConcurso = new Techne.Lyceum.RN.RecursosHumanos.TipoDocumentoConcurso();

            if (concurso != null)
            {
                return rnTipoDocumentoConcurso.ListaPor(Convert.ToString(concurso));
            }
            return null;
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

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Page.Header.Controls.AddAt(0, new HtmlMeta { HttpEquiv = "X-UA-Compatible", Content = "IE=8" });
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnExcluir, AcaoControle.excluir);
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(grdHabilitacoes);
            ControlaAcesso(grdExperiencias);
            ControlaAcesso(grdTitulacoes);
            ControlaAcesso(grdCargos);
            ControlaAcesso(grdTipoDocumento);

            lblMensagem.Text = string.Empty;
            if (tseMunicipio2.Value != null)
            {
                vsMunicipio = tseMunicipio2.Value.ToString();
            }
            if (tseConcurso.Value != null)
            {
                vsConcurso = tseConcurso.Value.ToString();
            }

            if (!IsPostBack)
            {
                _tipoOperacao = TipoOperacao.Inicial;
                ControlarTipoOperacao();
                PreencheCmbCargo();              
            }

            dtDtInicio.MinDate = dtDtFim.MinDate = dtInscrIni.MinDate = dtInscrFim.MinDate = dtLiConsIni.MinDate =
                dtLiConsFim.MinDate = dtConvocIni.MinDate = dtConvocFim.MinDate = dtIngrIni.MinDate =
                dtIngrFim.MinDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            TituloGrid();
        }

        private void PreencheCmbCargo()
        {
            //cmbCargo.Items.Clear();
            //cmbCargo.DataSource = RN.Funcao.RetornaFuncao();
            //cmbCargo.DataBind();
            //cmbCargo.Items.Insert(0, new ListEditItem("<Selecione>"));
            //cmbCargo.ValueField = "CODIGO";
            //cmbCargo.TextField = "DESCRICAO";
            //cmbCargo.SelectedIndex = 0;
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTitulacoes);
            ControlaAcesso(grdExperiencias);
            ControlaAcesso(grdHabilitacoes);
            ControlaAcesso(grdCargos);
            ControlaAcesso(grdTipoDocumento);
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTipoDocumento, string.Empty);

        }

        private void TituloGrid()
        {
            string tituloGrade = grdHabilitacoes.SettingsText.Title;
            if (tituloGrade != string.Empty) grdHabilitacoes.SettingsText.Title = tituloGrade.Replace("|Tabela:|", "Habilitações");
            string tituloCargo = grdCargos.SettingsText.Title;
            if (tituloCargo != string.Empty) grdCargos.SettingsText.Title = tituloCargo.Replace("|Tabela:|", "Cargos");
            string tituloExperiencias = grdExperiencias.SettingsText.Title;
            if (tituloExperiencias != string.Empty) grdExperiencias.SettingsText.Title = tituloExperiencias.Replace("|Tabela:|", "Experiências");
            string tituloTitulacoes = grdTitulacoes.SettingsText.Title;
            if (tituloTitulacoes != string.Empty) grdTitulacoes.SettingsText.Title = tituloTitulacoes.Replace("|Tabela:|", "Titulações");
        }

        #region Eventos

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            RN.Concurso rnConcurso = new Techne.Lyceum.RN.Concurso();

            #region Validação de Campos

            int teste = 0;

            if (!tseCargo.IsValidDBValue)
            {
                lblMensagem.Text = "O campo Função é de preenchimento obrigatório.";
                return;
            }

            if (ddlTipo.SelectedValue.IsNullOrEmptyOrWhiteSpace())
            {
                lblMensagem.Text = "Campo Tipo é obrigatório";
                return;
            }

            if (!Int32.TryParse(txtDiasConvoc.Text, out teste))
            {
                lblMensagem.Text = "Dias para apresentação inválido.";
                return;
            }

            if (!Int32.TryParse(txtDigitos.Text, out teste))
            {
                lblMensagem.Text = "Dígitos para número de inscrição inválido.";
                return;
            }

            if (!string.IsNullOrEmpty(dtDtInicio.Text.Trim()) && !string.IsNullOrEmpty(dtDtFim.Text.Trim()))
            {
                if (dtDtInicio.Date > dtDtFim.Date)
                {
                    lblMensagem.Text = "A data de início deve ser menor que a data de fim.";
                    return;
                }
            }
            else
            {
                lblMensagem.Text = "Os campos data de início e de fim são obrigatórios.";
                return;
            }

            if (string.IsNullOrEmpty(dtInscrIni.Text.Trim()))
            {
                lblMensagem.Text = "Data Início Inscrição: Preenchimento obrigatório.";
                return;
            }
            else if (string.IsNullOrEmpty(dtInscrFim.Text.Trim()))
            {
                lblMensagem.Text = "Data Fim Inscrição: Preenchimento obrigatório.";
                return;
            }
            else if (dtInscrIni.Date > dtInscrFim.Date)
            {
                lblMensagem.Text = "A data de início da inscrição deve ser menor que a data de fim.";
                return;
            }

            if (!string.IsNullOrEmpty(dtInscrIni.Text.Trim()))
            {
                if (!(dtInscrIni.Date >= dtDtInicio.Date) || !(dtInscrFim.Date <= dtDtFim.Date))
                {
                    lblMensagem.Text = "Período de Inscrição não pode iniciar antes ou ultrapassar o Período do Processo Seletivo.";
                    return;
                }
            }

            if (string.IsNullOrEmpty(dtLiConsIni.Text.Trim()))
            {
                lblMensagem.Text = "Data Início Liberação Consulta: Preenchimento obrigatório.";
                return;
            }
            else if (string.IsNullOrEmpty(dtLiConsFim.Text.Trim()))
            {
                lblMensagem.Text = "Data Fim Liberação Consulta: Preenchimento obrigatório.";
                return;
            }
            else if (dtLiConsIni.Date > dtLiConsFim.Date)
            {
                lblMensagem.Text = "A data de início da liberação da consulta deve ser menor que a data de fim.";
                return;
            }

            if (string.IsNullOrEmpty(dtConvocIni.Text.Trim()))
            {
                lblMensagem.Text = "Data Início Convocação: Preenchimento obrigatório.";
                return;
            }
            else if (string.IsNullOrEmpty(dtConvocFim.Text.Trim()))
            {
                lblMensagem.Text = "Data Fim Convocação: Preenchimento obrigatório.";
                return;
            }
            else if (dtConvocIni.Date > dtConvocFim.Date)
            {
                lblMensagem.Text = "A data de início da convocação deve ser menor que a data de fim.";
                return;
            }

            if (!string.IsNullOrEmpty(dtConvocIni.Text.Trim()))
            {
                if (!(dtConvocIni.Date > dtInscrIni.Date))
                {
                    lblMensagem.Text = "Período de Convocação deve começar após o início do Período de Inscrição.";
                    return;
                }
            }

            if (string.IsNullOrEmpty(dtIngrIni.Text.Trim()))
            {
                lblMensagem.Text = "Data Início Ingresso: Preenchimento obrigatório.";
                return;
            }
            else if (string.IsNullOrEmpty(dtIngrFim.Text.Trim()))
            {
                lblMensagem.Text = "Data Fim Ingresso: Preenchimento obrigatório.";
                return;
            }
            else if (dtIngrIni.Date > dtIngrFim.Date)
            {
                lblMensagem.Text = "A data de início do ingresso deve ser menor que a data de fim.";
                return;
            }
            else if (!(dtIngrIni.Date > dtConvocIni.Date))
            {
                lblMensagem.Text = "Período de Ingresso deve começar após o início do Período de Convocação.";
                return;
            }
            else if (dtIngrFim.Date < dtConvocFim.Date)
            {
                lblMensagem.Text = "Período Fim de Ingresso não pode ser menor que o Período Fim de Convocação.";
                return;
            }

            if (Convert.ToInt32(txtDiasConvoc.Text) == 0)
            {
                lblMensagem.Text = "Dias para a Apresentação deve ser maior que zero.";
                return;
            }

            #endregion

            String ValidacaoNumerico = ValidarNumero();
            if (!String.IsNullOrEmpty(ValidacaoNumerico))
            {
                lblMensagem.Text = ValidacaoNumerico;
                return;
            }

            if (!String.IsNullOrEmpty(txtDigitos.Text))
            {
                if (Int32.Parse(txtDigitos.Text) < 1 || Int32.Parse(txtDigitos.Text) > 5)
                {
                    lblMensagem.Text = "Dígitos para número de inscrição permite apenas valores entre 1 e 5.";
                    return;
                }
            }

            RN.RetValue retorno = null;

            CR.Ly_concurso_docente dtConcurso = new Ly_concurso_docente();
            ObterDados(dtConcurso);

            if (_tipoOperacao.Equals(TipoOperacao.Novo))
            {
                retorno = RN.Concurso.Incluir(dtConcurso);

                if (retorno.Ok)
                {
                    rnConcurso.AlteraFuncaoConcurso(txtConcurso.Text, tseCargo.DBValue.ToString());
                }
            }
            else if (_tipoOperacao.Equals(TipoOperacao.Alterar))
            {
                string strFuncao = RN.Funcao.RetornaFuncaoPorConcurso(txtConcurso.Text);

                if (strFuncao == tseCargo.DBValue.ToString())
                {
                    retorno = RN.Concurso.Alterar(dtConcurso);
                }
                else
                {
                    int intQtdCategoria = new RN.Concurso().RetornarCategoriasPor(txtConcurso.Text);

                    if (intQtdCategoria > 0)
                    {
                        lblMensagem.Text = "Função: para alterar a função, é necessário primeiro excluir os cargos cadastrados para o processo seletivo.";
                        return;
                    }
                    else
                        retorno = RN.Concurso.Alterar(dtConcurso);
                }

                if (retorno.Ok)
                {
                    rnConcurso.AlteraFuncaoConcurso(txtConcurso.Text, tseCargo.DBValue.ToString());
                }
            }

            if (retorno != null)
            {
                if (!retorno.Ok)
                {
                    lblMensagem.Text = retorno.Errors.ToString();
                }
                else
                {
                    lblMensagem.Text = retorno.Message;
                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();
                    tseConcurso.DBValue = txtConcurso.Text;
                }
            }

        }


        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Novo;
            ControlarTipoOperacao();
        }

        protected void btnExcluir_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Excluir;
            ControlarTipoOperacao();
        }

        protected void btnEditar_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Alterar;
            ControlarTipoOperacao();
            Page_Load(sender, e);
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Inicial;
            ControlarTipoOperacao();
        }


        protected void tseConcurso_Changed(object sender, EventArgs e)
        {
            if (tseConcurso.IsValidDBValue && !tseConcurso.DBValue.IsNull)
            {
                vsConcurso = tseConcurso.DBValue.ToString();
                LimparTela();
                _tipoOperacao = TipoOperacao.Consultar;
                ControlarTipoOperacao();
                lblMensagem.Text = string.Empty;

                pcConcursoDocentes.Visible = true;

            }
            else if (!tseConcurso.DBValue.IsNull)
            {
                vsConcurso = string.Empty;
                lblMensagem.Text = "Processo seletivo não cadastrado.";
                pcConcursoDocentes.Visible = false;
            }
            else
            {
                vsConcurso = string.Empty;
                lblMensagem.Text = "Favor consultar um processo seletivo.";
                pcConcursoDocentes.Visible = false;
            }
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAno.SelectedValue.ToString() == "" || ddlAno.SelectedValue.ToString() == null)
            {
                ddlPeriodo.Items.Clear();
                CarregarDropDownList(ddlPeriodo, null, null);
            }
            else
            {
                CarregarDadosDrop(ddlPeriodo.ID);
            }
        }

        #endregion

        #region Métodos
        private void ControlarTipoOperacao()
        {
            RN.Concurso rnConcurso = new Techne.Lyceum.RN.Concurso();

            pcConcursoDocentes.ActiveTabIndex = 0;

            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);
                        pcConcursoDocentes.Visible = false;
                        tseConcurso.ResetValue();
                        LimparTela();
                        break;
                    }
                case TipoOperacao.Sucesso:
                    {

                        ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar, btnExcluir };
                        ControlarVisibilidadeControle(controles);
                        pcConcursoDocentes.Visible = true;
                        DesabilitaCampos();

                        tseConcurso.ResetValue();
                        pcConcursoDocentes.TabPages[1].Enabled = true;
                        pcConcursoDocentes.TabPages[2].Enabled = true;
                        pcConcursoDocentes.TabPages[3].Enabled = true;
                        pcConcursoDocentes.TabPages[4].Enabled = true;
                        break;
                    }
                case TipoOperacao.Novo:
                    {
                        pcConcursoDocentes.Visible = true;
                        LimparTela();
                        PreencheCmbCargo();
                        pcConcursoDocentes.Visible = true;
                        tseConcurso.ResetValue();
                        tdsExperiencias.Select();
                        //tdsHabilitacoes.Select();
                        tseConcurso.Enabled = false;
                        pcConcursoDocentes.TabPages[1].Enabled = false;
                        pcConcursoDocentes.TabPages[2].Enabled = false;
                        pcConcursoDocentes.TabPages[3].Enabled = false;
                        pcConcursoDocentes.TabPages[4].Enabled = true;

                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);

                        lblMensagem.Text = string.Empty;
                        txtConcurso.ReadOnly = false;
                        HabilitaCampos();

                        break;
                    }
                case TipoOperacao.Excluir:
                    {

                        Ly_concurso_docente dtConcurso = new Ly_concurso_docente();
                        Ly_concurso_docente.Row dadosUnidade = dtConcurso.NewRow();
                        string concurso = txtConcurso.Text;

                        if (!rnConcurso.ExisteDocumentoNecesssarioPor(concurso))
                        {

                            RN.RetValue retorno = null;
                            retorno = RN.Concurso.Excluir(concurso);

                            if (retorno != null)
                            {
                                if (!retorno.Ok)
                                {
                                    lblMensagem.Text = retorno.Errors.ToString();
                                }
                                else
                                {
                                    LimparTela();
                                    lblMensagem.Text = retorno.Message;
                                    _tipoOperacao = TipoOperacao.Inicial;
                                    ControlarTipoOperacao();
                                }
                            }
                        }
                        else
                        {
                            lblMensagem.Text = "Existe documento(s) vinculado(s) a este processo seletivo. Verifique.";
                        }
                        break;
                    }
                case TipoOperacao.Alterar:
                    {
                        tseConcurso.Enabled = false;

                        HabilitaCampos();

                        txtConcurso.ReadOnly = true;

                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);
                        break;
                    }

                case TipoOperacao.Consultar:
                    {
                        lblMensagem.Text = string.Empty;

                        tseConcurso.Enabled = true;

                        DataTable dt = new DataTable();
                        Ly_concurso_docente dtConcurso = new Ly_concurso_docente();
                        Ly_concurso_docente.Row dadosConcurso = dtConcurso.NewRow();
                        dadosConcurso.Concurso = tseConcurso.DBValue.ToString();

                        dt = rnConcurso.ConsultarConcurso(tseConcurso.Value.ToString());

                        if (dt.Rows.Count > 0)
                        {
                            ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar, btnExcluir };
                            ControlarVisibilidadeControle(controles);
                            PreencherDadosTela(dt);
                            pcConcursoDocentes.Visible = true;
                            pcConcursoDocentes.TabPages[1].Enabled = true;
                            pcConcursoDocentes.TabPages[2].Enabled = true;
                            pcConcursoDocentes.TabPages[3].Enabled = true;
                            pcConcursoDocentes.TabPages[4].Enabled = true;
                            DesabilitaCampos();
                        }
                        else
                        {
                            LimparTela();
                            pcConcursoDocentes.Visible = false;
                            pcConcursoDocentes.TabPages[2].Enabled = false;
                            pcConcursoDocentes.TabPages[3].Enabled = false;
                            pcConcursoDocentes.TabPages[4].Enabled = false;
                            ImageButton[] controles = new ImageButton[] { btnNovo };
                            ControlarVisibilidadeControle(controles);
                            lblMensagem.Text = "Processo seletivo não cadastrado.";
                        }
                        break;
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
            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnExcluir, AcaoControle.excluir);
            ControlaAcesso(btnNovo, AcaoControle.novo);
        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnEditar.Visible = false;
            btnExcluir.Visible = false;
            btnNovo.Visible = false;
            btnSalvar.Visible = false;
        }

        /// <summary>
        /// Limpa todas as textbox e combobox.
        /// </summary>
        protected void LimparTela()
        {
            txtConcurso.Text = string.Empty;
            ddlStatus.Items.Clear();
            ddlTipo.ClearSelection();
            ddlAno.Items.Clear();
            ddlPeriodo.Items.Clear();
            tseCargo.ResetValue();
            CarregarDadosDrop(ddlStatus.ID);
            CarregarDadosDrop(ddlAno.ID);
            CarregarDadosDrop(ddlPeriodo.ID);

            dtDtInicio.Text = string.Empty;
            dtDtFim.Text = string.Empty;
            dtConvocFim.Text = string.Empty;
            dtConvocIni.Text = string.Empty;
            dtIngrFim.Text = string.Empty;
            dtIngrIni.Text = string.Empty;
            dtInscrFim.Text = string.Empty;
            dtInscrIni.Text = string.Empty;
            dtLiConsFim.Text = string.Empty;
            dtLiConsIni.Text = string.Empty;
            dtPubliDO.Text = string.Empty;

            txtDiasConvoc.Text = string.Empty;
            txtDigitos.Text = string.Empty;
            txtNumResolu.Text = string.Empty;
            txtObservacao.Text = string.Empty;
            chkIndigena.Checked = false;
        }

        /// <summary>
        /// Habilita todos os campos para edição
        /// </summary>
        protected void HabilitaCampos()
        {
            txtConcurso.ReadOnly = false;
            ddlStatus.Enabled = true;
            ddlTipo.Enabled = true;
            ddlAno.Enabled = true;
            ddlPeriodo.Enabled = true;
            tseCargo.Enabled = true;

            dtDtInicio.Enabled = true;
            dtDtFim.Enabled = true;
            dtConvocFim.Enabled = true;
            dtConvocIni.Enabled = true;
            dtIngrFim.Enabled = true;
            dtIngrIni.Enabled = true;
            dtInscrFim.Enabled = true;
            dtInscrIni.Enabled = true;
            dtLiConsFim.Enabled = true;
            dtLiConsIni.Enabled = true;
            dtPubliDO.Enabled = true;

            txtDiasConvoc.ReadOnly = false;
            txtDigitos.ReadOnly = false;
            txtNumResolu.ReadOnly = false;
            txtObservacao.ReadOnly = false;
            chkIndigena.Enabled = true;
        }

        /// <summary>
        /// Desabilita todos os campos para edição.
        /// </summary>
        protected void DesabilitaCampos()
        {
            ddlStatus.Enabled = false;
            ddlTipo.Enabled = false;
            ddlAno.Enabled = false;
            ddlPeriodo.Enabled = false;
            tseCargo.Enabled = false;

            dtDtInicio.Enabled = false;
            dtDtFim.Enabled = false;
            dtConvocFim.Enabled = false;
            dtConvocIni.Enabled = false;
            dtIngrFim.Enabled = false;
            dtIngrIni.Enabled = false;
            dtInscrFim.Enabled = false;
            dtInscrIni.Enabled = false;
            dtLiConsFim.Enabled = false;
            dtLiConsIni.Enabled = false;
            dtPubliDO.Enabled = false;

            txtConcurso.ReadOnly = true;
            txtDiasConvoc.ReadOnly = true;
            txtDigitos.ReadOnly = true;
            txtNumResolu.ReadOnly = true;
            txtObservacao.ReadOnly = true;
            chkIndigena.Enabled = false;
        }

        /// <summary>
        /// Armazena uma nova linha com os dados da tela no datatable passado como parâmetro.
        /// DEFINIÇÃO: Não será mais realizado o critério de reprovação por Pontuação para o Processo Seletivo.
        /// Assim, para todos os concursos cadastrados no sistema, sua Pontuação Mínima será zero.
        /// </summary>
        /// <param name="dtDocente">DataTable do docente que será adicionado uma nova linha</param>
        private void ObterDados(Ly_concurso_docente dtConcurso)
        {
            Techne.Lyceum.CR.Ly_concurso_docente.Row dadosConcurso = dtConcurso.NewRow();

            dadosConcurso.Pontuacao_minima = 0M;

            //CAMPOS
            if (!string.IsNullOrEmpty(txtConcurso.Text))
                dadosConcurso.Concurso = txtConcurso.Text;
            vsConcurso = txtConcurso.Text;

            if (tseCargo.IsValidDBValue)
            {
                string[] strdescricao = tseCargo["descricao"].ToString().Split('-');
                dadosConcurso.Descricao = strdescricao[1].ToString();
                dadosConcurso.FuncaoID = tseCargo.DBValue.ToString();
            }

            //STATUS
            if (string.IsNullOrEmpty(ddlStatus.SelectedValue))
            {
                dadosConcurso.Status = null;
            }
            else
            {
                dadosConcurso.Status = Convert.ToString(ddlStatus.SelectedValue);
            }

            dadosConcurso.Tipo = !ddlTipo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTipo.SelectedValue : null;

            //ANO
            if (string.IsNullOrEmpty(ddlAno.SelectedValue))
            {
                dadosConcurso.Ano = null;
            }
            else
            {
                dadosConcurso.Ano = Convert.ToDecimal(ddlAno.SelectedValue);
            }
            //PERIODO
            if (string.IsNullOrEmpty(ddlPeriodo.SelectedValue))
            {
                dadosConcurso.Semestre = null;
            }
            else
            {
                dadosConcurso.Semestre = Convert.ToDecimal(ddlPeriodo.SelectedValue);
            }

            //DATAS
            if (!string.IsNullOrEmpty(dtDtInicio.Text))
                dadosConcurso.Dt_inicio = Convert.ToDateTime(dtDtInicio.Text);
            if (!string.IsNullOrEmpty(dtDtFim.Text))
                dadosConcurso.Dt_fim = Convert.ToDateTime(dtDtFim.Text);

            if (!string.IsNullOrEmpty(dtConvocFim.Text))
                dadosConcurso.Dt_fim_convocacao = Convert.ToDateTime(dtConvocFim.Text);
            if (!string.IsNullOrEmpty(dtConvocIni.Text))
                dadosConcurso.Dt_ini_convocacao = Convert.ToDateTime(dtConvocIni.Text);

            if (!string.IsNullOrEmpty(dtIngrFim.Text))
                dadosConcurso.Dt_fim_ingresso = Convert.ToDateTime(dtIngrFim.Text);
            if (!string.IsNullOrEmpty(dtIngrIni.Text))
                dadosConcurso.Dt_ini_ingresso = Convert.ToDateTime(dtIngrIni.Text);

            if (!string.IsNullOrEmpty(dtInscrFim.Text))
                dadosConcurso.Dt_fim_inscr = Convert.ToDateTime(dtInscrFim.Text);
            if (!string.IsNullOrEmpty(dtInscrIni.Text))
                dadosConcurso.Dt_ini_inscr = Convert.ToDateTime(dtInscrIni.Text);

            if (!string.IsNullOrEmpty(dtLiConsFim.Text))
                dadosConcurso.Dt_fim_consulta = Convert.ToDateTime(dtLiConsFim.Text);
            if (!string.IsNullOrEmpty(dtLiConsIni.Text))
                dadosConcurso.Dt_ini_consulta = Convert.ToDateTime(dtLiConsIni.Text);

            if (!string.IsNullOrEmpty(dtPubliDO.Text))
                dadosConcurso.Dt_publicacao_do = Convert.ToDateTime(dtPubliDO.Text);

            //CAMPOS
            if (!string.IsNullOrEmpty(txtDiasConvoc.Text))
                dadosConcurso.Qt_dias_convocacao = Convert.ToDecimal(txtDiasConvoc.Text);

            if (!string.IsNullOrEmpty(txtDigitos.Text))
                dadosConcurso.Nr_digitos_codigo = Convert.ToDecimal(txtDigitos.Text);

            if (!string.IsNullOrEmpty(txtNumResolu.Text))
                dadosConcurso.Nr_resolucao = txtNumResolu.Text;

            if (!string.IsNullOrEmpty(txtObservacao.Text))
                dadosConcurso.Observacao = txtObservacao.Text;

            dadosConcurso.Indigena = chkIndigena.Checked ? "S" : "N";

            dtConcurso.Rows.Add(dadosConcurso);
        }

        /// <summary>
        /// Preenche os dados na tela de acordo com a linha passada como parâmetro
        /// </summary>
        /// <param name="dadosDocente">Linha com os dados do docente</param>
        private void PreencherDadosTela(DataTable dadosConcurso)
        {
            DataTable dt = new DataTable();

            txtConcurso.Text = dadosConcurso.Rows[0]["CONCURSO"].ToString();
            //COMBOS
            PreencherDadoCombo(ddlStatus, dadosConcurso.Rows[0]["STATUS"].ToString());
            PreencherDadoCombo(ddlAno, dadosConcurso.Rows[0]["ANO"].ToString());
            CarregarDadosDrop(ddlPeriodo.ID);
            PreencherDadoCombo(ddlPeriodo, dadosConcurso.Rows[0]["SEMESTRE"].ToString());

            ddlTipo.SelectedValue = dadosConcurso.Rows[0]["TIPO"].ToString();


            //DATAS
            dtDtInicio.Value = Convert.ToDateTime(dadosConcurso.Rows[0]["DT_INICIO"].ToString());
            dtDtFim.Value = Convert.ToDateTime(dadosConcurso.Rows[0]["DT_FIM"].ToString());
            if (dadosConcurso.Rows[0]["DT_FIM_CONVOCACAO"] != DBNull.Value)
                dtConvocFim.Value = Convert.ToDateTime(dadosConcurso.Rows[0]["DT_FIM_CONVOCACAO"].ToString());
            if (dadosConcurso.Rows[0]["DT_INI_CONVOCACAO"] != DBNull.Value)
                dtConvocIni.Value = Convert.ToDateTime(dadosConcurso.Rows[0]["DT_INI_CONVOCACAO"].ToString());
            if (dadosConcurso.Rows[0]["DT_FIM_INGRESSO"] != DBNull.Value)
                dtIngrFim.Value = Convert.ToDateTime(dadosConcurso.Rows[0]["DT_FIM_INGRESSO"].ToString());
            if (dadosConcurso.Rows[0]["DT_INI_INGRESSO"] != DBNull.Value)
                dtIngrIni.Value = Convert.ToDateTime(dadosConcurso.Rows[0]["DT_INI_INGRESSO"].ToString());
            if (dadosConcurso.Rows[0]["DT_FIM_INSCR"] != DBNull.Value)
                dtInscrFim.Value = Convert.ToDateTime(dadosConcurso.Rows[0]["DT_FIM_INSCR"].ToString());
            if (dadosConcurso.Rows[0]["DT_INI_INSCR"] != DBNull.Value)
                dtInscrIni.Value = Convert.ToDateTime(dadosConcurso.Rows[0]["DT_INI_INSCR"].ToString());
            if (dadosConcurso.Rows[0]["DT_FIM_CONSULTA"] != DBNull.Value)
                dtLiConsFim.Value = Convert.ToDateTime(dadosConcurso.Rows[0]["DT_FIM_CONSULTA"].ToString());
            if (dadosConcurso.Rows[0]["DT_INI_CONSULTA"] != DBNull.Value)
                dtLiConsIni.Value = Convert.ToDateTime(dadosConcurso.Rows[0]["DT_INI_CONSULTA"].ToString());
            if (dadosConcurso.Rows[0]["DT_PUBLICACAO_DO"] != DBNull.Value)
                dtPubliDO.Value = Convert.ToDateTime(dadosConcurso.Rows[0]["DT_PUBLICACAO_DO"].ToString());

            //CAMPOS
            txtDiasConvoc.Text = dadosConcurso.Rows[0]["QT_DIAS_CONVOCACAO"].ToString();
            txtDigitos.Text = dadosConcurso.Rows[0]["NR_DIGITOS_CODIGO"].ToString();
            txtNumResolu.Text = dadosConcurso.Rows[0]["NR_RESOLUCAO"].ToString();
            txtObservacao.Text = dadosConcurso.Rows[0]["OBSERVACAO"].ToString();
            chkIndigena.Checked = dadosConcurso.Rows[0]["INDIGENA"].ToString() == "S" ? true : false;

            tseCargo.DBValue = dadosConcurso.Rows[0]["FUNCAOID"].ToString();

            if (!tseCargo.IsValidDBValue)
            {
                lblMensagem.Text = "Função do Processo Seletivo não encontrada. Verifique.";
                return;
            }
        }

        #region COMBO
        private void CarregarDropDownList(DropDownList drop, QueryTable data, string defaultValue)
        {
            drop.DataSource = data;
            drop.DataBind();

            if (drop.Items.Count == 0)
            {
                ListItem itemVazio = new ListItem("<Lista Vazia>", "");
                drop.Items.Add(itemVazio);
            }
            else
            {
                try
                {
                    if (!string.IsNullOrEmpty(defaultValue))
                        drop.SelectedValue = defaultValue;
                    else
                    {
                        ListItem itemNulo = new ListItem("<Nenhum>", "");
                        drop.Items.Add(itemNulo);
                        drop.SelectedValue = Convert.ToString(itemNulo);
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    drop.ClearSelection();
                }
            }
            if (_tipoOperacao.Equals(TipoOperacao.Novo))
            {
                drop.SelectedValue = "";
            }
        }

        private QueryTable CarregarDadosDrop(string idDrop)
        {
            QueryTable dadosDrop = null;

            try
            {
                switch (idDrop.ToUpper())
                {
                    case "DDLANO":
                        {
                            dadosDrop = RN.PeriodoLetivo.ConsultarAno();

                            CarregarDropDownList(ddlAno, dadosDrop, "");
                            break;
                        }
                    case "DDLPERIODO":
                        {
                            if (ddlAno.SelectedValue != "" && ddlAno.SelectedValue != null)
                            {
                                string ano = ddlAno.SelectedValue.ToString();
                                dadosDrop = RN.PeriodoLetivo.ConsultarPeriodo(ano);

                                CarregarDropDownList(ddlPeriodo, dadosDrop, "");
                            }

                            break;
                        }
                    case "DDLSTATUS":
                        {
                            dadosDrop = RN.Concurso.ConsultarStatus();

                            CarregarDropDownList(ddlStatus, dadosDrop, null);

                            break;
                        }
                }
            }
            catch
            {
                throw;
            }

            return dadosDrop;
        }



        #endregion //COMBOS
        #endregion //METODOS

        #region GRIDS

        //CARGOS
        protected void grdCargos_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdCargos.Settings.ShowFilterRow = false;
        }

        protected void grdCargos_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            RN.ContratoTemporario.ConcursoDocente_CategoriaDocente rnConcursoDocente_CategoriaDocente = new RN.ContratoTemporario.ConcursoDocente_CategoriaDocente();
            ValidacaoDados valida = new ValidacaoDados();
            string strCargaHoraria = string.Empty;

            TSearchBox tseCategoria = (TSearchBox)grdCargos.FindEditFormTemplateControl("tseCargos");

            try
            {
                if (!tseCategoria.DBValue.IsNull)
                {
                    strCargaHoraria = Convert.ToString(tseCategoria["cargaefetiva"]);
                }

                valida = rnConcursoDocente_CategoriaDocente.Valida(txtConcurso.Text, tseCategoria.DBValue.ToString(), strCargaHoraria);

                if (valida.Valido)
                {
                    int intRetorno = RN.ContratoTemporario.ConcursoDocente_CategoriaDocente.InserirCargosConcurso(txtConcurso.Text, tseCategoria.DBValue.ToString());
                    e.Cancel = true;
                    this.grdCargos.CancelEdit();
                }
                else
                {
                    throw new Exception(valida.Mensagem);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Data["error"].ToString());
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível incluir o cargo " + tseCategoria.DBValue + ", já existe um cargo cadastrado para o processo seletivo com o mesmo valor para a carga horária semanal efetiva.");
            }
        }

        protected void grdCargos_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdCargos);

            if (grdCargos.IsNewRowEditing)
            {
                TSearchBox tse = (TSearchBox)grdCargos.FindEditFormTemplateControl("tseCargos");
                if (tse != null)
                    tse.ReadOnly = false;
            }

            if (grdCargos.IsEditing)
            {
                TSearchBox tse = (TSearchBox)grdCargos.FindEditFormTemplateControl("tseCargos");
                if (tse != null)
                    tse.ReadOnly = false;
            }
        }

        protected void grdCargos_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            Techne.Lyceum.RN.ContratoTemporario.ConcursoDocente_CategoriaDocente rnConcursoDocente_CategoriaDocente = new Techne.Lyceum.RN.ContratoTemporario.ConcursoDocente_CategoriaDocente();
            string categoria = e.Values["CATEGORIA"].ToString().Trim();
            string concurso = txtConcurso.Text;

            rnConcursoDocente_CategoriaDocente.ExcluirCargosConcurso(concurso, categoria);

            e.Cancel = true;
            this.grdCargos.CancelEdit();
        }

        protected void grdCargos_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados valida = new ValidacaoDados();
            Techne.Lyceum.RN.ContratoTemporario.ConcursoDocente_CategoriaDocente rnConcursoDocente_CategoriaDocente = new Techne.Lyceum.RN.ContratoTemporario.ConcursoDocente_CategoriaDocente();
            TSearchBox tseCategoria = (TSearchBox)grdCargos.FindEditFormTemplateControl("tseCargos");

            string concurso = txtConcurso.Text;
            string categoria = tseCategoria.DBValue.ToString();
            string cargaHorariaEfetiva = string.Empty;

            if (!tseCategoria.DBValue.IsNull)
            {
                cargaHorariaEfetiva = Convert.ToString(tseCategoria["cargaefetiva"]);
            }

            valida = rnConcursoDocente_CategoriaDocente.Valida(concurso, categoria, cargaHorariaEfetiva);
            if (valida.Valido)
            {
                grdCargos.CancelEdit();
                e.Cancel = true;
            }
            else
            {
                throw new Exception(valida.Mensagem);
            }
        }

        //HABILITACOES
        protected void grdHabilitacoes_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string concurso = Convert.ToString(e.GetListSourceFieldValue("concurso"));
                string nucleo = Convert.ToString(e.GetListSourceFieldValue("NUCLEO"));
                string agrupamento = Convert.ToString(e.GetListSourceFieldValue("DISCIPLINA"));
                string municipio_proc = Convert.ToString(e.GetListSourceFieldValue("MUNICIPIO"));
                e.Value = concurso + "|" + nucleo + "|" + agrupamento + "|" + municipio_proc;
            }
        }

        protected void grdHabilitacoes_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Keys.Clear();
            e.Keys.Add("concurso", e.Values["concurso"]);
            e.Keys.Add("nucleo", e.Values["NUCLEO"]);
            e.Keys.Add("agrupamento", e.Values["DISCIPLINA"]);
            e.Keys.Add("municipio_proc", e.Values["MUNICIPIO"]);
        }

        protected void grdHabilitacoes_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            e.NewValues["concurso"] = txtConcurso.Text;
            string[] chaves = e.Keys["CompositeKey"].ToString().Split('|');
            e.Keys.Clear();
            e.Keys.Add("concurso", chaves[0]);
            e.Keys.Add("nucleo", chaves[1]);
            e.Keys.Add("agrupamento", chaves[3]);
        }

        protected void grdHabilitacoes_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdHabilitacoes.Settings.ShowFilterRow = false;
        }

        protected void grdHabilitacoes_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdHabilitacoes.Settings.ShowFilterRow = false;
        }

        protected void grdHabilitacoes_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["concurso"] = txtConcurso.Text;

            var cmbCargo = (ASPxComboBox)grdHabilitacoes.FindEditFormTemplateControl("cmbCargo");
            var tseDisciplina = (TSearchBox)grdHabilitacoes.FindEditFormTemplateControl("tseDisciplina");
            var tseMunicipio = (TSearchBox)grdHabilitacoes.FindEditFormTemplateControl("tseMunicipio");

            if (cmbCargo.SelectedItem.Value.Equals("DOC II") && tseDisciplina.DBValue.IsNull)
            {
                e.NewValues["agrupamento"] = AREA_INTEGRADA_DOCII;
            }
        }

        //EXPERIENCIAS
        protected void grdExperiencias_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string concurso = Convert.ToString(e.GetListSourceFieldValue("concurso"));
                string experiencia = Convert.ToString(e.GetListSourceFieldValue("experiencia"));
                e.Value = concurso + "|" + experiencia;
            }
        }

        protected void grdExperiencias_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Keys.Clear();
            e.Keys.Add("concurso", e.Values["concurso"]);
            e.Keys.Add("experiencia", e.Values["experiencia"]);
        }

        protected void grdExperiencias_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            e.NewValues["concurso"] = txtConcurso.Text;

            string[] chaves = e.Keys["CompositeKey"].ToString().Split('|');
            e.Keys.Clear();
            e.Keys.Add("concurso", chaves[0]);
            e.Keys.Add("experiencia", chaves[1]);
        }

        protected void grdExperiencias_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdExperiencias.Settings.ShowFilterRow = false;
        }

        protected void grdExperiencias_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdExperiencias.Settings.ShowFilterRow = false;
        }

        protected void grdExperiencias_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["concurso"] = txtConcurso.Text;
        }

        //TITULACOES
        protected void grdTitulacoes_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string concurso = Convert.ToString(e.GetListSourceFieldValue("concurso"));
                string titulacao = Convert.ToString(e.GetListSourceFieldValue("titulacao"));
                e.Value = concurso + "|" + titulacao;
            }
        }

        protected void grdTitulacoes_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Keys.Clear();
            e.Keys.Add("concurso", e.Values["concurso"]);
            e.Keys.Add("titulacao", e.Values["titulacao"]);
        }

        protected void grdTitulacoes_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            e.NewValues["concurso"] = txtConcurso.Text;

            string[] chaves = e.Keys["CompositeKey"].ToString().Split('|');
            e.Keys.Clear();
            e.Keys.Add("concurso", chaves[0]);
            e.Keys.Add("titulacao", chaves[1]);
        }

        protected void grdTitulacoes_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdTitulacoes.Settings.ShowFilterRow = false;
        }

        protected void grdTitulacoes_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTitulacoes.Settings.ShowFilterRow = false;
        }

        protected void grdTitulacoes_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["concurso"] = txtConcurso.Text;
        }

        #endregion

        protected void grdTitulacoes_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdTitulacoes);

            if (grdTitulacoes.IsNewRowEditing)
            {
                TSearchBox tse = (TSearchBox)grdTitulacoes.FindEditFormTemplateControl("tseTitulacao");
                if (tse != null)
                    tse.ReadOnly = false;
            }
            else if (grdTitulacoes.IsEditing)
            {
                TSearchBox tse = (TSearchBox)grdTitulacoes.FindEditFormTemplateControl("tseTitulacao");
                if (tse != null)
                    tse.ReadOnly = true;
            }
        }

        protected void grdExperiencias_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdExperiencias);

            if (grdExperiencias.IsNewRowEditing)
            {
                TSearchBox tse = (TSearchBox)grdExperiencias.FindEditFormTemplateControl("tseExperiencia");
                if (tse != null)
                    tse.ReadOnly = false;
            }
            else if (grdExperiencias.IsEditing)
            {
                TSearchBox tse = (TSearchBox)grdExperiencias.FindEditFormTemplateControl("tseExperiencia");
                if (tse != null)
                    tse.ReadOnly = true;
            }
        }

        protected void grdHabilitacoes_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdHabilitacoes);

            TSearchBox tseRegional = (TSearchBox)grdHabilitacoes.FindEditFormTemplateControl("tseRegional");
            TSearchBox tseMunicipio = (TSearchBox)grdHabilitacoes.FindEditFormTemplateControl("tseMunicipio");
            ASPxComboBox cmbCargo = (ASPxComboBox)grdHabilitacoes.FindEditFormTemplateControl("cmbCargo");
            TSearchBox tseDisciplina = (TSearchBox)grdHabilitacoes.FindEditFormTemplateControl("tseDisciplina");
            if (grdHabilitacoes.IsNewRowEditing)
            {
                if (tseDisciplina != null)
                    tseDisciplina.ReadOnly = false;
                if (cmbCargo.SelectedItem.Value != null || cmbCargo.SelectedItem.Value.Equals(string.Empty))
                    cmbCargo.Enabled = true;
                if (tseRegional != null)
                    tseRegional.ReadOnly = false;
                if (tseMunicipio != null)
                    tseMunicipio.ReadOnly = false;
            }
            else if (grdHabilitacoes.IsEditing)
            {
                if (tseDisciplina != null)
                    tseDisciplina.ReadOnly = true;
                if (cmbCargo != null)
                    cmbCargo.Enabled = false;
                if (tseRegional != null)
                    tseRegional.ReadOnly = true;
                if (tseMunicipio != null)
                    tseMunicipio.ReadOnly = true;
            }
        }

        protected void grdExperiencias_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (e.NewValues["pontuacao"] != null)
            {
                decimal teste;
                if (!decimal.TryParse(e.NewValues["pontuacao"].ToString(), out teste))
                {
                    lblMensagem.Text = "Pontuação inválida.";
                    return;
                }
            }
        }

        protected void grdTitulacoes_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (e.NewValues["pontuacao"] != null)
            {
                decimal teste;
                if (!decimal.TryParse(e.NewValues["pontuacao"].ToString(), out teste))
                {
                    lblMensagem.Text = "Pontuação inválida.";
                    return;
                }
            }
        }

        #region :: Validar Campos Numéricos ::
        private String ValidarNumero()
        {
            String retorno = String.Empty;

            // Digitos para número de inscrição
            if (!string.IsNullOrEmpty(txtDigitos.Text))
            {
                if (!RN.Validacao.ValidaNumerosInteirosPositivosOuVazios(txtDigitos.Text))
                    retorno = retorno + "Só é possível inserir número no campo Digitos para número de inscrição.<br />";
            }

            // Dias para a convocação
            if (!string.IsNullOrEmpty(txtDiasConvoc.Text))
            {
                if (!RN.Validacao.ValidaNumerosInteirosPositivosOuVazios(txtDiasConvoc.Text))
                    retorno = retorno + "Só é possível inserir número no campo Dias para a convocação.<br />";
            }

            return retorno;
        }
        #endregion

        protected void HabilitaPopUpInsercao(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddlAno.SelectedValue) > 2020)
            {
                tseRegional.ResetValue();
                tseMunicipio2.ResetValue();
                tseMunicipio2.Enabled = false;
                listTo.Items.Clear();
                listFrom.Items.Clear();
                vsMunicipio = string.Empty;
                ppcMensagem.ShowOnPageLoad = true;
            }
            else
            {
                MsgBox("Não é possível cadastrar habilitações para este processo seletivo!", this);

                return;
            }
        }

        protected void btnFrom_Click(object sender, EventArgs e)
        {
            ListEditItem item = new ListEditItem();

            for (int i = 0; i < listFrom.Items.Count; i++)
            {
                if (listFrom.Items[i].Selected == true)
                {
                    item = listFrom.Items[listFrom.SelectedIndex];
                    listTo.Items.Add(item);
                }

                listFrom.Items.RemoveAt(i);
            }
        }

        protected void btnGravar_Click(object sender, EventArgs e)
        {
            RN.Entidades.LyConcursoDocHabilitacao docHabilitacao = new Techne.Lyceum.RN.Entidades.LyConcursoDocHabilitacao();

            if (Convert.ToInt32(ddlAno.SelectedValue) > 2020)
            {
                if (tseRegional.Value == null)
                {
                    MsgBox("Campo Regional é obrigatório", this);
                    return;
                }

                if (vsMunicipio == null)
                {
                    MsgBox("Campo Município é obrigatório", this);
                    return;
                }
               

                if (listTo.Items.Count == 0)
                {
                    MsgBox("É obrigatório escolher uma disciplina!", this);
                    return;
                }

                for (int i = 0; i < listTo.Items.Count; i++)
                {

                    docHabilitacao.RegionalId = Convert.ToInt32(tseRegional.Value.ToString());
                    docHabilitacao.Municipio_proc = vsMunicipio;
                    docHabilitacao.Concurso = vsConcurso;
                    docHabilitacao.Agrupamento = listTo.Items[i].Value.ToString();

                    if (new RN.ConcursoDocHabilitacao().ValidaHabilitacoesExistentes(docHabilitacao).Rows.Count == 0)
                    {
                        new RN.ConcursoDocHabilitacao().Insere(docHabilitacao);
                        MsgBox("Salvo com sucesso!", this);
                    }
                }
                ppcMensagem.ShowOnPageLoad = false;
                tseRegional.ResetValue();

                grdHabilitacoes.DataBind();
            }
            else
            {
                MsgBox("Não é possível cadastrar habilitações para este processo seletivo!", this);
            }

        }

        protected void grdHabilitacoes_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            RN.Entidades.LyConcursoDocHabilitacao docHabilitacao = new Techne.Lyceum.RN.Entidades.LyConcursoDocHabilitacao();

            if (e.ButtonID == "btExcluir")
            {
                if (Convert.ToInt32(ddlAno.SelectedValue) > 2020)
                {
                    docHabilitacao.RegionalId = Convert.ToInt32(grdHabilitacoes.GetRowValues(e.VisibleIndex, "NUCLEO_ID"));
                    docHabilitacao.Municipio_proc = Convert.ToString(grdHabilitacoes.GetRowValues(e.VisibleIndex, "MUNICIPIO_ID"));
                    docHabilitacao.Concurso = txtConcurso.Text;
                    docHabilitacao.Agrupamento = Convert.ToString(grdHabilitacoes.GetRowValues(e.VisibleIndex, "DISCIPLINA_ID"));

                    DataTable dtHabilitacao = new DataTable();
                    dtHabilitacao = new RN.ProcessoSeletivo().RetornaExistenciaHabilitacaoPor(docHabilitacao.Concurso, docHabilitacao.Municipio_proc, docHabilitacao.Agrupamento, Convert.ToInt32(docHabilitacao.RegionalId));

                    if (dtHabilitacao.Rows.Count == 0)
                        new RN.ConcursoDocHabilitacao().Remove(docHabilitacao);
                    else
                    {
                        throw new Exception("Não é possível excluir, esta habilitação possui vínculos com candidatos!");
                    }
                }
                else
                {
                    throw new Exception("Não é possível excluir habilitações para este processo seletivo!");
                }
            }
            grdHabilitacoes.DataBind();

        }

        protected void tseRegional_Changed(object sender, EventArgs e)
        {
            if (tseRegional.DBValue.IsNull)
            {
                tseMunicipio2.DBValue = DBNull.Value;
                tseMunicipio2.Enabled = false;
                tseMunicipio2.Value = string.Empty;
            }
            else
            {
                tseMunicipio2.Enabled = true;
            }           
        }

        protected void tseMunicipio2_Changed(object sender, EventArgs e)
        {
            RN.Concurso rnConcurso = new Techne.Lyceum.RN.Concurso();
            RN.Entidades.LyConcursoDocHabilitacao docHabilitacao = new Techne.Lyceum.RN.Entidades.LyConcursoDocHabilitacao();

            if (!tseRegional.DBValue.IsNull && vsMunicipio != "")
            {
                listFrom.Items.Clear();
                listFrom.DataSource = rnConcurso.RetornaDisciplinasHabilitadasPor(txtConcurso.Text, tseRegional.DBValue.ToString(), vsMunicipio);
                listFrom.DataBind();

                tseMunicipio2.DBValue = vsMunicipio;
            }

            if (tseRegional.DBValue.IsNull && vsMunicipio != "")
            {
                tseMunicipio2.DBValue = DBNull.Value;
                tseMunicipio2.Enabled = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mensagem"></param>
        /// <param name="page"></param>
        public static void MsgBox(string Mensagem, Page page)
        {
            var script = @"alert('" + Mensagem + @"');";
            page.Page.ClientScript.RegisterStartupScript(page.Page.GetType(), "popup", script, true);
            return;
        }

        private void LimparCamposPopup()
        {
            tseRegional.ResetValue();
            tseMunicipio2.ResetValue();
            listTo.Items.Clear();
        }


        protected void grdTipoDocumento_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdTipoDocumento);
        }

        protected void grdTipoDocumento_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTipoDocumento.Settings.ShowFilterRow = false;
        }

        protected void grdTipoDocumento_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {

            if (grdTipoDocumento.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "TIPODOCUMENTOID")
                    e.Editor.Enabled = true;
            }
            else if (grdTipoDocumento.IsEditing)
            {
                if ((e.Column.FieldName == "TIPODOCUMENTOID"))
                    e.Editor.ReadOnly = true;
            }
        }

        protected void grdTipoDocumento_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ANEXO"] = true;
            grdTipoDocumento.Settings.ShowFilterRow = false;
        }

        public void InsertTipoDocumentoConcurso(object TIPODOCUMENTOID, object ANEXO) { }
        public void UpdateTipoDocumentoConcurso(object TIPODOCUMENTOID, object ANEXO, object TIPODOCUMENTOCONCURSOID) { }
        public void DeleteTipoDocumentoConcurso(object TIPODOCUMENTOCONCURSOID) { }

        protected void grdTipoDocumento_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RecursosHumanos.Entidades.TipoDocumentoConcurso tipoDocumentoConcurso = new Techne.Lyceum.RN.RecursosHumanos.Entidades.TipoDocumentoConcurso();
            RN.RecursosHumanos.TipoDocumentoConcurso rnTipoDocumentoConcurso = new RN.RecursosHumanos.TipoDocumentoConcurso();

            tipoDocumentoConcurso.Concurso = (tseConcurso.IsValidDBValue && !tseConcurso.DBValue.IsNull) ? tseConcurso.DBValue.ToString() : null;
            tipoDocumentoConcurso.TipoDocumentoId = e.NewValues["TIPODOCUMENTOID"] != null ? Convert.ToInt32(e.NewValues["TIPODOCUMENTOID"]) : -1;
            tipoDocumentoConcurso.Anexo = (e.NewValues["ANEXO"] == null || Convert.ToBoolean(e.NewValues["ANEXO"]) == false) ? false : true;
            tipoDocumentoConcurso.UsuarioId = User.Identity.Name;

            validacao = rnTipoDocumentoConcurso.Valida(tipoDocumentoConcurso, true);

            if (validacao.Valido)
            {
                rnTipoDocumentoConcurso.Insere(tipoDocumentoConcurso);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoDocumento.DataBind();

        }

        protected void grdTipoDocumento_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RecursosHumanos.Entidades.TipoDocumentoConcurso tipoDocumentoConcurso = new Techne.Lyceum.RN.RecursosHumanos.Entidades.TipoDocumentoConcurso();
            RN.RecursosHumanos.TipoDocumentoConcurso rnTipoDocumentoConcurso = new RN.RecursosHumanos.TipoDocumentoConcurso();

            tipoDocumentoConcurso.Anexo = (e.NewValues["ANEXO"] == null || Convert.ToBoolean(e.NewValues["ANEXO"]) == false) ? false : true;
            tipoDocumentoConcurso.TipoDocumentoId = Convert.ToInt32(e.NewValues["TIPODOCUMENTOID"]);
            tipoDocumentoConcurso.TipoDocumentoConcursoId = Convert.ToInt32(e.Keys["TIPODOCUMENTOCONCURSOID"]);
            tipoDocumentoConcurso.UsuarioId = User.Identity.Name;

            validacao = rnTipoDocumentoConcurso.Valida(tipoDocumentoConcurso, false);

            if (validacao.Valido)
            {
                rnTipoDocumentoConcurso.Atualiza(tipoDocumentoConcurso);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoDocumento.DataBind();
        }

        protected void grdTipoDocumento_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RecursosHumanos.TipoDocumentoConcurso rnTipoDocumentoConcurso = new RN.RecursosHumanos.TipoDocumentoConcurso();
            int tipoDocumentoConcursoId = 0;

            tipoDocumentoConcursoId = Convert.ToInt32(e.Keys["TIPODOCUMENTOCONCURSOID"]);

            validacao = rnTipoDocumentoConcurso.ValidaRemocao(tipoDocumentoConcursoId);

            if (validacao.Valido)
            {
                rnTipoDocumentoConcurso.Remove(tipoDocumentoConcursoId);
                grdTipoDocumento.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

    }
}
