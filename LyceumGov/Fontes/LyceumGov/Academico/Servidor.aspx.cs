using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using System.Data;
using System.Web.UI;
using DevExpress.Web.ASPxEditors;
using Techne.Controls;
using Techne.Lyceum.CR;
using Techne.Web;
using Techne.Lyceum.RN.Entidades;
using DevExpress.Web.ASPxClasses;
using System.Collections.Generic;
using DevExpress.Web.ASPxTabControl;
using System.Configuration;


namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/Servidor.aspx"),
    ControlText("Servidor"),
    Title("Servidores/Funcionários"),]

    public partial class Servidor : TPage
    {
        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }
        private string ID_FORMACAO_PESSOAL = null;

        protected int minCargaHorariaCursoCapacitacao
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["MinCargaHorariaCursoCapacitacao"]);
            }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                lblMensagemFormacao.Text = string.Empty;
                lblMensValidacao.Text = string.Empty;

                if (!IsPostBack)
                {
                    pcPessoa.ActiveTabIndex = 0;

                    //para a primeira vez que a página é carregada o tipo de operação será inicial
                    _tipoOperacao = TipoOperacao.Inicial;

                    ControlarTipoOperacao();

                    if (!IsPostBack)
                    {
                        CarregaAreaCurso(ddlAreaCurso);
                        CarregaAreaCurso(ddlAreaCursoPosGraduacao);
                        CarregaUFCartorio();

                        PreecherComboTabGeral(ddlTipoInstituicao, RN.Util.Cache.TipoInstituicao);
                        PreecherComboTabGeral(ddlSituacaoCurso, RN.Util.Cache.SituacaoCursoFormacao);
                        PreecherComboTabGeral(ddlFormComplementPedag, RN.Util.Cache.FormacaoComplementar);
                        PreecherComboTabGeral(ddlTipoInstituicaoPosGraduacao, RN.Util.Cache.TipoInstituicao);
                        PreecherComboTabGeral(ddlSituacaoCursoPosGraduacao, RN.Util.Cache.SituacaoCursoFormacao);
                        PreecherComboTabGeral(ddlFormComplementPedagPosGraduacao, RN.Util.Cache.FormacaoComplementar);

                        PreecherComboTabGeralFiltro(ddlEscolaridade, "EscolaridadeFormacao", "", "Pós-", "Superior");
                        PreecherComboTabGeralFiltro(ddlEscolaridadePosGraduacao, "EscolaridadeFormacao", "Pós-", "-1", "Superior");
                    }
                }

                ControlarTSearchs();
                //tamanho máximo das datas
                DateTime dtAtual = DateTime.Now;

                dtDataNasc.MaxDate = dtAtual.Date.AddDays(-1);
                dboCprof_DtExp.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                dboDOC_CertNasc_DtEmissao.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                dboDOC_Rg_Dtexp.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, (DateTime.Now.Day));
                dboDMIL_Alist_DtExp.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, (DateTime.Now.Day));
                dboDMIL_Cr_DtExp.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, (DateTime.Now.Day));
                dboDOC_Teleitor_DtExp.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, (DateTime.Now.Day));

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        private void CarregaAreaCurso(DropDownList controle)
        {
            RN.AreaFormacaoPessoal rnAreaFormacaoPessoal = new AreaFormacaoPessoal();
            ListItem item = new ListItem("Selecione", string.Empty);

            controle.Items.Clear();
            controle.DataSource = rnAreaFormacaoPessoal.ObtemListaAreas();
            controle.DataBind();
            controle.Items.Insert(0, item);
        }

        private void CarregaNecessidadeEspecial()
        {
            RN.NecessidadeEspecial.NecessidadeEspecial rnNecessidadeEspecial = new RN.NecessidadeEspecial.NecessidadeEspecial();
            ListItem itemVazio = new ListItem("Selecione", string.Empty);

            ddlNecessidadeEspecial.Items.Clear();
            ddlNecessidadeEspecial.DataSource = rnNecessidadeEspecial.ListaNecessidadeEspecialAtiva();
            ddlNecessidadeEspecial.DataBind();
            ddlNecessidadeEspecial.Items.Insert(0, itemVazio);
        }

        private void CarregaUFCartorio()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlUFCartorio.Items.Clear();
            ddlUFCartorio.DataSource = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.UfCartorio, RN.Basico.QueryListaUFCartorio);
            ddlUFCartorio.DataBind();
            ddlUFCartorio.Items.Insert(0, item);
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

        private void CarregaEtnia()
        {
            RN.Etnia rnEtnia = new Etnia();
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlEtnia.Items.Clear();
            ddlEtnia.DataSource = rnEtnia.ListaEtniaAtiva();
            ddlEtnia.DataBind();
            ddlEtnia.Items.Insert(0, item);
        }

        private void CarregaCargo()
        {
            RN.CategoriaDocente rnCategoriaDocente = new CategoriaDocente();
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlCargo.Items.Clear();
            ddlCargo.DataSource = rnCategoriaDocente.ListaCategoriaFuncionario();
            ddlCargo.DataBind();
            ddlCargo.Items.Insert(0, item);
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdDadosIngresso, "Dados de Ingresso");
            TituloGrid(grdFormacaoPessoal, "Formação Pessoal");
            TituloGrid(grdCapacitacao, "Capacitação Profissional");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdDadosIngresso);
        }

        protected void grdFormacaoPessoal_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdFormacaoPessoal.Settings.ShowFilterRow = false;

        }
        protected void grdFormacaoPessoal_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdFormacaoPessoal.Settings.ShowFilterRow = false;
        }

        protected void ddlTipoInstituicao_SelectedIndexChanged(object sender, EventArgs e)
        {
            tseInstituicao.ResetValue();

            if (ddlTipoInstituicao.SelectedValue != "Selecione")
            {
                tseInstituicao.DataBind();
            }
        }

        protected void tseInstituicao_Changed(object sender, EventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                if (!tseInstituicao.DBValue.IsNull)
                {
                    if (tseInstituicao.IsValidDBValue)
                    {
                        lblMensagem.Text = string.Empty;
                    }
                    else
                    {
                        lblMensagem.Text = "Instituição não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Instituição não cadastrado (favor verificar).";

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdCapacitacao_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdCapacitacao.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "PESSOAID")
                {
                    e.Editor.Enabled = true;
                    e.Editor.ReadOnly = true;
                }

                if ((e.Column.FieldName) == "CURSOCAPACITACAOID")
                    e.Editor.Enabled = false;

            }
            else if (grdCapacitacao.IsEditing)
            {
                if ((e.Column.FieldName) == "PESSOAID")
                    e.Editor.Enabled = false;

                if ((e.Column.FieldName) == "CURSOCAPACITACAOID")
                    e.Editor.Enabled = false;
            }

        }

        private decimal CalcularOrdem(string pessoa)
        {
            decimal ordem = 0;

            QueryTable dadosCapacitacao = null;

            dadosCapacitacao = RN.Capacitacao.ConsultarOrdem(pessoa);

            string dados = dadosCapacitacao.Rows[0].ToString();
            char[] parametros = new char[] { ':' };
            string[] dadosOrdem = dados.Split(parametros, 2, StringSplitOptions.None);
            if (dadosOrdem[1].ToString() != " ")
                ordem = Convert.ToDecimal(dadosOrdem[1]);
            else
                ordem = 0;

            ordem = ordem + 1;

            return ordem;
        }

        protected void grdCapacitacao_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["PESSOAID"] = txtPessoa.Text.ToString();
            decimal ordem = CalcularOrdem(txtPessoa.Text.ToString());
            e.NewValues["CURSOCAPACITACAOID"] = ordem.ToString();
            grdCapacitacao.Settings.ShowFilterRow = false;
        }

        protected void grdCapacitacao_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string pessoa = Convert.ToString(e.GetListSourceFieldValue("PESSOAID"));
                string ordem = Convert.ToString(e.GetListSourceFieldValue("CURSOCAPACITACAOID"));
                e.Value = pessoa + "-" + ordem;
            }
        }

        protected void grdCapacitacao_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Keys.Clear();
            e.Keys.Add("PESSOAID", e.Values["PESSOAID"]);
            e.Keys.Add("CURSOCAPACITACAOID", e.Values["CURSOCAPACITACAOID"]);
            e.Keys.Add("TIPOCURSOCAPACITACAOID", Convert.ToInt32(e.Values["TIPOCURSOCAPACITACAOID"]));
        }

        protected void grdCapacitacao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string[] chaves = e.Keys["CompositeKey"].ToString().Split('-');
            e.Keys.Clear();
            e.Keys.Add("PESSOAID", chaves[0]);
            e.Keys.Add("CURSOCAPACITACAOID", chaves[1]);
        }

        protected void grdCapacitacao_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["PESSOAID"] = txtPessoa.Text.ToString();
        }

        protected void grdCapacitacao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdCapacitacao.Settings.ShowFilterRow = false;
        }

        protected void grdCapacitacao_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (e.NewValues["DATACONCLUSAO"] != null)
            {
                DateTime dataconcl = Convert.ToDateTime(e.NewValues["DATACONCLUSAO"]);
                DateTime hoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                if (dataconcl > hoje)
                    e.RowError = "Data de conclusão não pode ser maior que a data atual.";

                DateTime milnov = new DateTime(1900, 1, 1);

                if (dataconcl < milnov)
                    e.RowError = "Data de conclusão não pode ser menor que 1900.";
            }

            if (e.NewValues["CARGAHORARIA"] != null)
            {

                int ch = int.Parse(e.NewValues["CARGAHORARIA"].ToString());

                if (ch < 4)
                    e.RowError = "Não é permitido cadastrar cursos/capacitações com carga horária inferior a 4 horas.";
            }

            if (e.NewValues["NOMECURSO"] != null)
            {

                if (e.NewValues["NOMECURSO"].ToString().Length > 100)
                    e.RowError = "Não é permitido cadastrar cursos/capacitações com mais de 100 caracteres.";
            }
        }

        protected void grdCapacitacao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdCapacitacao);
        }

        protected void grdFormacaoPessoal_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdFormacaoPessoal.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "PESSOA")
                    e.Editor.Enabled = true;
                if ((e.Column.FieldName) == "ID_INSTITUICAO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "AREA")
                    e.Editor.Enabled = false;
                if ((e.Column.FieldName) == "NOME_COMP")
                    e.Editor.Enabled = false;

            }
            else if (grdFormacaoPessoal.IsEditing)
            {
                if ((e.Column.FieldName) == "PESSOA")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "ID_INSTITUICAO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "AREA")
                    e.Editor.Enabled = false;
                if ((e.Column.FieldName) == "NOME_COMP")
                    e.Editor.Enabled = false;

                if ((e.Column.FieldName) == "AREA_CURSO")
                {
                    ASPxComboBox cmbcurso = (e.Editor as ASPxComboBox);

                    cmbcurso.Items.Clear();
                    cmbcurso.DataSource = RN.CursoFormacaoPessoal.ListarCursoArea();
                    cmbcurso.TextField = "AREA_CURSO";
                    cmbcurso.ValueField = "CODIGO";
                    cmbcurso.DataBind();

                    var item = cmbcurso.Items.FindByText((string)e.Value);

                    if (item != null)
                    {
                        item.Selected = true;
                    }
                }

                if ((e.Column.FieldName) == "ESCOLARIDADE")
                {
                    ASPxComboBox cmbEscolaridade = (e.Editor as ASPxComboBox);
                }

                if ((e.Column.FieldName) == "SITUACAO_CURSO")
                {
                    ASPxComboBox cmbSituacaoCurso = (e.Editor as ASPxComboBox);
                }
                if ((e.Column.FieldName) == "FORMACAO_COMPLEMENTACAO_PEDAGOGICA")
                {
                    ASPxComboBox cmbFormacao = (e.Editor as ASPxComboBox);
                }
            }
        }

        protected void grdFormacaoPessoal_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            if (e.ButtonID == "Editar")
            {
                string escolaridade = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "ESCOLARIDADE"));
                string area = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "AREA"));
                ID_FORMACAO_PESSOAL = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "ID_FORMACAO_PESSOAL"));
                ViewState["idFormacaoPessoa"] = ID_FORMACAO_PESSOAL;
                if (escolaridade.Contains("Superior") || escolaridade.Contains("Ensino Médio"))
                {
                    ddlEscolaridade.Text = string.Empty;
                    if (!string.IsNullOrEmpty(escolaridade))
                    {
                        if (ddlEscolaridade.Items.FindByText(escolaridade) != null)
                        {
                            ddlEscolaridade.Text = escolaridade;
                        }
                    }

                    ddlEscolaridade_SelectedIndexChanged(sender, e);
                    DataTable dtDisciplinasAdicionais = RN.FormacaoPessoal.ListarDisciplinaAdicional(ID_FORMACAO_PESSOAL);

                    ddlSituacaoCurso.SelectedValue = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "SITUACAO_CURSO"));
                    ddlAreaCurso.SelectedValue = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "CODIGOAREA"));
                    ddlAreaCurso_SelectedIndexChanged(sender, e);
                    ddlCurso.SelectedValue = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "CODIGOCURSO"));
                    ddlCurso_SelectedIndexChanged(sender, e);
                    ddlFormComplementPedag.SelectedValue = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "FORMACAO_COMPLEMENTACAO_PEDAGOGICA"));
                    txtAnoInicio.Text = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "ANO_INICIO"));
                    txtAnoConclusao.Text = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "ANO_CONCLUSAO"));
                    if (Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "TIPOINSTITUICAO")) != DBNull.Value.ToString())
                    {
                        ddlTipoInstituicao.SelectedValue = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "TIPOINSTITUICAO"));
                    }
                    tseInstituicao.DataBind();
                    tseInstituicao.Text = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "ID_INSTITUICAO"));
                    string docComprobatorios = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "DOC_COMPROBATORIO"));
                    if (docComprobatorios.Equals("Sim"))
                    {
                        ckDocComprob.Checked = true;
                    }
                    else
                    {
                        ckDocComprob.Checked = false;
                    }
                    btnSalvarFormacao.Text = "Salvar Formação-Pessoal";
                }
                else if (escolaridade.Contains("Pós-Graduação"))
                {
                    ddlEscolaridadePosGraduacao.Text = string.Empty;
                    if (!string.IsNullOrEmpty(escolaridade))
                    {
                        if (ddlEscolaridadePosGraduacao.Items.FindByText(escolaridade) != null)
                        {
                            ddlEscolaridadePosGraduacao.Text = escolaridade;
                        }
                    }
                    ddlEscolaridadePosGraduacao_SelectedIndexChanged(sender, e);
                    ddlSituacaoCursoPosGraduacao.SelectedValue = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "SITUACAO_CURSO"));
                    ddlAreaCursoPosGraduacao.SelectedValue = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "CODIGOAREA"));
                    ddlAreaCursoPosGraduacao_SelectedIndexChanged(sender, e);
                    ddlCursoPosGraduacao.SelectedValue = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "CODIGOCURSO"));
                    ddlCursoPosGraduacao_SelectedIndexChanged(sender, e);
                    ddlFormComplementPedagPosGraduacao.SelectedValue = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "FORMACAO_COMPLEMENTACAO_PEDAGOGICA"));
                    txtAnoInicioPosGraduacao.Text = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "ANO_INICIO"));
                    txtAnoConclusaoPosGraduacao.Text = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "ANO_CONCLUSAO"));
                    if (Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "TIPOINSTITUICAO")) != DBNull.Value.ToString())
                    {
                        ddlTipoInstituicaoPosGraduacao.SelectedValue = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "TIPOINSTITUICAO"));
                    }
                    tseInstituicaoPosGraduacao.DataBind();
                    tseInstituicaoPosGraduacao.Text = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "ID_INSTITUICAO"));
                    string docComprobatoriosPosGrad = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "DOC_COMPROBATORIO"));
                    if (docComprobatoriosPosGrad.Equals("Sim"))
                    {
                        ckDocComprobPosGraduacao.Checked = true;
                    }
                    else
                    {
                        ckDocComprobPosGraduacao.Checked = false;
                    }
                    btnSalvarFormacaoPosGraduacao.Text = "Salvar Formação-Pessoal Pós-Graduação";
                }
            }
        }

        protected void grdFormacaoPessoal_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdFormacaoPessoal);
        }

        public void PreecherComboTabGeral(DropDownList combo, string tabela)
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            combo.Items.Clear();
            combo.DataSource = RN.Util.Cache.CarregaItemTabelaGeralPor(tabela);
            combo.DataBind();
            combo.Items.Insert(0, item);
        }

        public void PreecherComboTabGeralFiltro(DropDownList combo, string tabela, string filtro, string exceto, string excluso)
        {
            combo.Items.Clear();
            combo.DataSource = RN.TabelaGeral.ConsultaItemTabelaValDescrFiltro(tabela, filtro, exceto, excluso);
            combo.DataBind();
            combo.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        protected void grdFormacaoPessoal_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(e.NewValues["ESCOLARIDADE"])))
            {
                e.RowError = "Favor informar a Escolaridade.";
            }
            if (string.IsNullOrEmpty(Convert.ToString(e.NewValues["SITUACAO_CURSO"])))
            {
                e.RowError = "Favor informar a Situação do Curso.";
            }
            if (string.IsNullOrEmpty(Convert.ToString(e.NewValues["AREA_CURSO"])))
            {
                e.RowError = "Favor informar o Curso.";
            }
            if (string.IsNullOrEmpty(Convert.ToString(e.NewValues["FORMACAO_COMPLEMENTACAO_PEDAGOGICA"])))
            {
                e.RowError = "Favor informar a Formação/Complementação Pedagógica.";
            }
            if (string.IsNullOrEmpty(Convert.ToString(e.NewValues["ANO_INICIO"])))
            {
                e.RowError = "Favor informar o Ano de Início.";
            }

            if ((Convert.ToString(e.NewValues["SITUACAO_CURSO"]) == "Concluído") && e.NewValues["ANO_CONCLUSAO"] == null)
            {
                e.RowError = "O campo 'Ano de Conclusão' deve ser preenchido.";
            }

            if (!string.IsNullOrEmpty(e.NewValues["ANO_INICIO"].ToString()) && e.NewValues["ANO_CONCLUSAO"] != null)
            {
                if (int.Parse(e.NewValues["ANO_INICIO"].ToString()) > int.Parse(e.NewValues["ANO_CONCLUSAO"].ToString()))
                {
                    e.RowError = "Ano de Início não pode ser superior ao Ano de Conclusão ";
                }
            }

            if (!string.IsNullOrEmpty(e.NewValues["ANO_INICIO"].ToString()))
            {
                if (int.Parse(e.NewValues["ANO_INICIO"].ToString()) < 1930)
                {
                    e.RowError = " O campo 'Ano de Início' deve ser maior que 1930.";
                }
                if (int.Parse(e.NewValues["ANO_INICIO"].ToString()) == 0)
                {
                    e.RowError = "O campo 'Ano de Início' não pode ser igual a zero(0).";
                }
                if (e.NewValues["ANO_INICIO"].ToString().Length != 4)
                {
                    e.RowError = "O campo 'Ano de Início' deve ter 4 dígitos.";
                }
                if (int.Parse(e.NewValues["ANO_INICIO"].ToString()) > DateTime.Now.Year)
                {
                    e.RowError = "O campo 'Ano de Início' não pode ser maior que ano vigente.";
                }
            }

            if (e.NewValues["ANO_CONCLUSAO"] != null)
            {
                if (int.Parse(e.NewValues["ANO_CONCLUSAO"].ToString()) < 1930)
                {
                    e.RowError = " O campo 'Ano de Conclusão' deve ser maior que 1930.";
                }
                if (int.Parse(e.NewValues["ANO_CONCLUSAO"].ToString()) == 0)
                {
                    e.RowError = "O campo 'Ano de Conclusão' não pode ser igual a zero(0).";
                }
                if (e.NewValues["ANO_CONCLUSAO"].ToString().Length != 4)
                {
                    e.RowError = "O campo 'Ano de Conclusão' deve ter 4 dígitos.";
                }
            }
        }

        protected void grdFormacaoPessoal_AutoFilterCellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "DOC_COMPROBATORIO")
            {
                DevExpress.Web.ASPxEditors.ASPxComboBox combo = e.Editor as DevExpress.Web.ASPxEditors.ASPxComboBox;
                combo.Items.Clear();
                DevExpress.Web.ASPxGridView.GridViewDataCheckColumn check = e.Column as DevExpress.Web.ASPxGridView.GridViewDataCheckColumn;
                combo.ValueType = check.PropertiesCheckEdit.ValueType;
                combo.Items.Add(string.Empty, null);
                combo.Items.Add("Marcado", check.PropertiesCheckEdit.ValueChecked);
                combo.Items.Add("Desmarcado", check.PropertiesCheckEdit.ValueUnchecked);
            }

        }

        protected void odsArea_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var validacao = new ValidacaoDados();

            var id = e.InputParameters["ID_FORMACAO_PESSOAL"].ToString();


            lblMensagem.Text = "";
            lblMensValidacao.Text = "";

            var TFP = RN.FormacaoPessoal.Carregar(int.Parse(id));

            var cEscolaridade = TFP.Escolaridade.ToString();
            var cSituacaoCurso = TFP.SituacaoCurso.ToString();
            bool valido = true;

            Techne.Data.QueryTable qtGraduacaoConcluida = RN.FormacaoPessoal.ConsultarGraduacaoConcluida(int.Parse(txtPessoa.Text.ToString()), int.Parse(id));
            Techne.Data.QueryTable qtGraduacaoAndamento = RN.FormacaoPessoal.ConsultarGraduacaoAndamento(int.Parse(txtPessoa.Text.ToString()), int.Parse(id));
            Techne.Data.QueryTable qtPosGraduacao = RN.FormacaoPessoal.ConsultarPosGraduacao(int.Parse(txtPessoa.Text.ToString()), int.Parse(id));

            if (cEscolaridade.Trim().Substring(0, 8) == "Superior" && cSituacaoCurso.Trim() == "Concluído")
            {
                if (qtGraduacaoConcluida.Rows.Count == 1 && qtGraduacaoAndamento.Rows.Count >= 1)
                {
                    lblMensValidacao.Text = @"Não se pode excluir uma graduação completa se tiver uma graduação em andamento cadastrada !";

                    valido = false;
                }

                if (qtGraduacaoConcluida.Rows.Count == 0 && qtPosGraduacao.Rows.Count >= 1)
                {
                    lblMensValidacao.Text = @"Não se pode excluir uma graduação se tiver uma Pós-Graduação cadastrada !";
                    valido = false;
                }

            }

            if (valido)
            {
                RN.FormacaoPessoal.Remover(int.Parse(id));
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "window.opener.ExecutarPostBack();", true);
            }
        }

        protected void odsArea_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var validacao = new ValidacaoDados();

            var TFP = new TceFormacaoPessoal
            {
                Pessoa = int.Parse(e.InputParameters["PESSOA"].ToString()),
                IdFormacaoPessoal = int.Parse(e.InputParameters["ID_FORMACAO_PESSOAL"].ToString()),
                Escolaridade = e.InputParameters["ESCOLARIDADE"].ToString(),
                SituacaoCurso = e.InputParameters["SITUACAO_CURSO"].ToString(),
                IdCursoFormacaoPessoal = int.Parse(e.InputParameters["AREA_CURSO"].ToString().Split('-')[1]),
                FormacaoComplementacaoPedagogica = e.InputParameters["FORMACAO_COMPLEMENTACAO_PEDAGOGICA"].ToString(),
                AnoInicio = int.Parse(e.InputParameters["ANO_INICIO"].ToString()),
                AnoConclusao = e.InputParameters["ANO_CONCLUSAO"] != null ? int.Parse(e.InputParameters["ANO_CONCLUSAO"].ToString()) : 0,
                IdInstituicao = e.InputParameters["ID_INSTITUICAO"].ToString(),
                Doc_comprobatorio = e.InputParameters["DOC_COMPROBATORIO"].ToString(),
                Matricula = User.Identity.Name
            };

            validacao = RN.FormacaoPessoal.Validar(TFP);

            if (!validacao.Valido)
            {
                throw new Exception(validacao.Mensagem.ToString());
            }
            else
            {
                if (RN.FormacaoPessoal.Alterar(TFP) > 0)
                {

                }
            }

        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            ControlarEnderecoPais();
        }

        #region Propriedades e Enumeradores
        public enum TipoOperacao
        {
            Novo,
            Excluir,
            Alterar,
            Consultar,
            ConsultarPessoa,
            Inicial,
            Sucesso
        }

        private TipoOperacao _tipoOperacao
        {
            get { return (TipoOperacao)ViewState["_tipoOperacao"]; }
            set { ViewState["_tipoOperacao"] = value; }
        }
        #endregion

        protected void ddlPaisNasc_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimparEnderecoNascimento();
        }

        protected void tseMunicipioEleitor_Changed(object sender, EventArgs e)
        {
            if (!tseMunicipioEleitor.DBValue.IsNull && tseMunicipioEleitor.IsValidDBValue)
            {
                txtEstadoEleitor.Value = tseMunicipioEleitor["uf_sigla"].ToString();
            }
        }

        protected void tseMunicipioResid_Changed(object sender, EventArgs e)
        {
            if (!tseMunicipioResid.DBValue.IsNull && tseMunicipioResid.IsValidDBValue)
            {
                txtEstado.Value = tseMunicipioResid["uf_sigla"].ToString();
            }
        }

        protected void tseMunicipioNaturalidade_Changed(object sender, EventArgs e)
        {
            if (!tseMunicipioNaturalidade.DBValue.IsNull && tseMunicipioNaturalidade.IsValidDBValue)
            {
                txtEstadoNaturalidade.Value = tseMunicipioNaturalidade["uf_sigla"].ToString();
            }
        }

        private void ControlarEnderecoPais()
        {
            if (ddlPaisResid.SelectedValue != "")
            {
                if (ddlPaisResid.SelectedItem.Text.ToUpper() != "BRASIL")
                {
                    tsCEP.ShowButton = false;

                    txtCEPResid.Enabled = true;
                    txtCEPResid.MaxLength = 9;

                    txtMunicipio.Visible = true;
                    txtMunicipio.Enabled = true;
                    tseMunicipioResid.Visible = false;

                    if (_tipoOperacao == TipoOperacao.Novo || _tipoOperacao == TipoOperacao.Alterar)
                        txtEstado.Attributes.Remove("readonly");
                }
                else
                {
                    if (_tipoOperacao == TipoOperacao.Novo || _tipoOperacao == TipoOperacao.Alterar || _tipoOperacao == TipoOperacao.ConsultarPessoa)
                    {
                        tsCEP.ShowButton = true;
                    }
                    else
                    {
                        tsCEP.ShowButton = false;
                    }
                    txtCEPResid.Enabled = true;
                    txtCEPResid.MaxLength = 8;

                    txtMunicipio.Visible = false;
                    txtMunicipio.Enabled = false;

                    tseMunicipioResid.Visible = true;

                    txtEstado.Attributes.Add("readonly", "readonly");
                }
            }

            if (ddlPaisNasc.SelectedItem != null)
            {
                if (ddlPaisNasc.SelectedItem.Text.ToUpper() != "BRASIL")
                {
                    txtMunicipioNaturalidade.Visible = true;
                    tseMunicipioNaturalidade.Visible = false;
                    if (_tipoOperacao == TipoOperacao.Novo || _tipoOperacao == TipoOperacao.Alterar)
                        txtEstadoNaturalidade.Attributes.Remove("readonly");
                }
                else
                {
                    txtMunicipioNaturalidade.Visible = false;
                    tseMunicipioNaturalidade.Visible = true;

                    txtEstadoNaturalidade.Attributes.Add("readonly", "readonly");
                }
            }
        }

        private void LimparEnderecoNascimento()
        {
            tseMunicipioNaturalidade.ResetValue();
            txtMunicipioNaturalidade.Text = string.Empty;
            txtEstadoNaturalidade.Value = string.Empty;
        }

        private void LimparEndereco()
        {
            txtMunicipio.Text = string.Empty;
            tseMunicipioResid.ResetValue();
            txtEstado.Value = string.Empty;
            txtEnderecoResid.Text = string.Empty;
            txtCEPResid.Text = string.Empty;
            txtComplementoResid.Text = string.Empty;
            txtEndNumResid.Text = string.Empty;
            txtBairroResid.Text = string.Empty;
            chkAreaAssentamento.Checked = false;
            chkTerraIndigena.Checked = false;
            chkQuilombos.Checked = false;
            chkNaoSeAplica.Checked = false;
            rblLocalizacaoUF.SelectedIndex = -1;
        }

        private void LimparFiliacao()
        {
            txtNomePai.Text = string.Empty;
            txtNomeMae.Text = string.Empty;
            chkMaeNaoDeclarada.Checked = false;
            chkPaiNaoDeclarado.Checked = false;
        }


        public object ListaIdVinculo(object pessoa)
        {

            if (pessoa == null)
                return null;

            RN.VinculoLy rnVinculo = new VinculoLy();
            return rnVinculo.ListaIdVinculo(pessoa.ToString());


        }



        public object ListarPessoa(string pessoa)
        {
            if (!String.IsNullOrEmpty(pessoa))
                return RN.FormacaoPessoal.ListarPessoa(pessoa);


            return null;
        }

        public object ListarPessoaDisciplinaAdic(string formacaopessoalid)
        {
            if (!String.IsNullOrEmpty(formacaopessoalid))
                return RN.FormacaoPessoal.ListarDisciplinaAdicional(formacaopessoalid);


            return null;
        }

        public void Delete(object FORMACAOPESSOALID, object ESTUDOADICIONALID)
        {
            TceFormacaoEstudoAdicional tcd = new TceFormacaoEstudoAdicional();


            tcd.FormacaoPessoalID = int.Parse(FORMACAOPESSOALID.ToString());
            tcd.EstudoAdicionalID = int.Parse(ESTUDOADICIONALID.ToString());

            if (RN.FormacaoPessoal.DeletarFormacaoPessoalAdicional(tcd) > 0)
            {

            }
        }

        private void ControlarTSearchs()
        {
            if (ddlPaisResid.SelectedValue != "")
            {
                if (ddlPaisResid.SelectedItem.Text.ToUpper() == "BRASIL")
                {
                    txtMunicipio.Visible = false;
                    tseMunicipioResid.Visible = true;
                }
                else
                {
                    txtMunicipio.Visible = true;
                    tseMunicipioResid.Visible = false;
                }
            }
            else
            {
                txtMunicipio.Visible = true;
                tseMunicipioResid.Visible = false;
            }

            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        tseMunicipioEleitor.Enabled = true;
                        tseMunicipioNaturalidade.Enabled = true;
                        tseMunicipioResid.Enabled = true;
                        tseServidor.Enabled = true;
                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        tseMunicipioEleitor.Enabled = true;
                        tseMunicipioNaturalidade.Enabled = true;
                        tseMunicipioResid.Enabled = true;
                        tseServidor.Enabled = true;
                        break;
                    }

                case TipoOperacao.Novo:
                    {
                        tseMunicipioEleitor.Enabled = true;
                        tseMunicipioNaturalidade.Enabled = true;
                        tseMunicipioResid.Enabled = true;
                        tseServidor.Enabled = false;
                        break;
                    }
                case TipoOperacao.Excluir:
                    {
                        tseMunicipioEleitor.Enabled = false;
                        tseMunicipioNaturalidade.Enabled = false;
                        tseMunicipioResid.Enabled = false;
                        tseServidor.Enabled = true;
                        break;
                    }
                case TipoOperacao.Alterar:
                    {
                        tseMunicipioEleitor.Enabled = true;
                        tseMunicipioNaturalidade.Enabled = true;
                        tseMunicipioResid.Enabled = true;
                        tseServidor.Enabled = false;
                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        tseMunicipioEleitor.Mode = ControlMode.View;
                        tseMunicipioNaturalidade.Mode = ControlMode.View;
                        tseMunicipioResid.Mode = ControlMode.View;
                        tseServidor.Enabled = true;
                        break;
                    }
            }
        }

        private void ControlarTipoOperacao()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);
                        pcPessoa.Visible = false;
                        tseServidor.ResetValue();
                        tseServidor.Enabled = true;
                        lbltxtPessoa.Visible = false;
                        txtPessoa.Visible = true;
                        pcPessoa.ActiveTabIndex = 0;
                        ddlPovoIndigena.Visible = false;
                        lblPovo.Visible = false;
                        ddlPovoIndigena.ClearSelection();
                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar, btnExcluir };
                        ControlarVisibilidadeControle(controles);
                        pnlPessoa.Visible = false;
                        pcPessoa.Visible = true;
                        pcPessoa.TabPages[5].Enabled = true;
                        pcPessoa.TabPages[6].Enabled = true;
                        pnlDadosIngresso.Visible = false;
                        LimparDadosIngresso();
                        DesabilitaCampos();
                        tseServidor.Enabled = true;
                        lbltxtPessoa.Visible = false;
                        txtPessoa.Visible = true;
                        break;
                    }

                case TipoOperacao.Novo:
                    {
                        pnlPessoa.Visible = true;
                        pcPessoa.Visible = true;
                        pcPessoa.ActiveTabIndex = 0;
                        pcPessoa.TabPages[5].Enabled = false;
                        pcPessoa.TabPages[6].Enabled = false;
                        LimparTela();
                        HabilitaCampos();
                        tsePessoa.ResetValue();
                        tseServidor.ResetValue();
                        tseServidor.Enabled = false;
                        lbltxtPessoa.Visible = true;
                        txtPessoa.Visible = false;
                        pnlDadosIngresso.Visible = true;
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);

                        CarregarDadosDrop(ddlPaisNasc.ID);
                        CarregarDadosDrop(ddlNacionalidade.ID);
                        CarregarDadosDrop(ddlPaisResid.ID);
                        CarregarDadosDrop(ddlRG_Tipo.ID);
                        CarregarDadosDrop(ddDOC_Rg_Emissor.ID);
                        CarregarDadosDrop(ddlEstadoCivil.ID);
                        CarregarDadosDrop(ddDOC_Rg_Uf.ID);
                        CarregarDadosDrop(ddDlCprof_Uf.ID);
                        CarregarDadosDrop(ddDOC_CertNasc_Uf.ID);
                        CarregaEtnia();
                        CarregaCargo();
                        CarregaNecessidadeEspecial();
                        ddlPovoIndigena.Visible = false;
                        lblPovo.Visible = false;
                        ddlPovoIndigena.ClearSelection();
                        break;
                    }
                case TipoOperacao.Excluir:
                    {
                        pnlPessoa.Visible = false;
                        ValidacaoDados validacao = new ValidacaoDados();
                        RN.VinculoLy rnVinculo = new VinculoLy();

                        validacao = rnVinculo.ValidaRemocaoServidor(Convert.ToDecimal(txtPessoa.Text));

                        if (validacao.Valido)
                        {
                            rnVinculo.RemoveServidor(Convert.ToDecimal(txtPessoa.Text));
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup",
                                                                           "alert('Servidor excluído com sucesso.');", true);

                            _tipoOperacao = TipoOperacao.Inicial;
                            ControlarTipoOperacao();

                        }
                        else
                        {
                            lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        }

                        break;
                    }
                case TipoOperacao.Alterar:
                    {
                        pnlPessoa.Visible = false;
                        pcPessoa.TabPages[5].Enabled = true;
                        pcPessoa.TabPages[6].Enabled = true;
                        pcPessoa.ActiveTabIndex = 0;
                        lbltxtPessoa.Visible = false;
                        txtPessoa.Visible = true;
                        pnlDadosIngresso.Visible = false;
                        HabilitaCampos();
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);
                        lblMensagem.Text = String.Empty;
                        tseServidor.Enabled = false;
                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        LimparEndereco();
                        LimparFiliacao();
                        LimparEnderecoNascimento();
                        LimparTela();
                        pnlPessoa.Visible = false;
                        pcPessoa.ActiveTabIndex = 0;
                        pcPessoa.TabPages[5].Enabled = true;
                        pcPessoa.TabPages[6].Enabled = true;
                        lbltxtPessoa.Visible = false;
                        txtPessoa.Visible = true;
                        tseServidor.Enabled = true;
                        pnlDadosIngresso.Visible = false;
                        lblMensagem.Text = string.Empty;

                        var dadosPessoa = Pessoa.Carregar(Convert.ToInt32(tseServidor["pessoa"].ToString()));

                        if (dadosPessoa == null)
                        {
                            lblMensagem.Text = "Pessoa não cadastrada.";
                            pcPessoa.Visible = false;
                            ImageButton[] controles = new ImageButton[] { btnNovo };
                            ControlarVisibilidadeControle(controles);
                        }
                        else
                        {
                            txtPessoa.Text = Convert.ToString(dadosPessoa.Pessoa);
                            txtPessoaHid.Text = txtPessoa.Text;
                            ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar, btnExcluir };
                            ControlarVisibilidadeControle(controles);
                            pcPessoa.Visible = true;
                            CarregarDadosDrop(ddlPaisNasc.ID);
                            CarregarDadosDrop(ddlNacionalidade.ID);
                            CarregarDadosDrop(ddlPaisResid.ID);
                            CarregarDadosDrop(ddlRG_Tipo.ID);
                            CarregarDadosDrop(ddDOC_Rg_Emissor.ID);
                            CarregarDadosDrop(ddlEstadoCivil.ID);
                            CarregarDadosDrop(ddDOC_Rg_Uf.ID);
                            CarregarDadosDrop(ddDlCprof_Uf.ID);
                            CarregarDadosDrop(ddDOC_CertNasc_Uf.ID);
                            CarregaEtnia();
                            CarregaCargo();
                            CarregaNecessidadeEspecial();
                            CarregaDadosPessoa(dadosPessoa);

                            if (!tseServidor.DBValue.IsNull)
                            {
                                txtIdFuncional.Text = Convert.ToString(tseServidor["idfuncional"]);
                                txtIdFuncionalAtualizacao.Text = Convert.ToString(tseServidor["idfuncional"]);
                                txtMatricula.Text = Convert.ToString(tseServidor["matricula"]);
                            }
                            else
                            {
                                txtIdFuncional.Text = string.Empty;
                                txtIdFuncionalAtualizacao.Text = string.Empty;
                                txtMatricula.Text = string.Empty;
                            }

                            RN.RecursosHumanos.Entidades.GoogleEducation googleEducation = new Techne.Lyceum.RN.RecursosHumanos.Entidades.GoogleEducation();
                            RN.RecursosHumanos.GoogleEducation rnGoogleEducation = new Techne.Lyceum.RN.RecursosHumanos.GoogleEducation();

                            googleEducation = rnGoogleEducation.ObtemPor(Convert.ToDecimal(dadosPessoa.Pessoa));

                            if (!googleEducation.Email.IsNullOrEmptyOrWhiteSpace())
                            {
                                txtEmailGoogle.Text = googleEducation.Email.Trim();
                            }

                            DesabilitaCampos();
                        }
                        break;
                    }
                case TipoOperacao.ConsultarPessoa:
                    {
                        LimparEndereco();
                        LimparFiliacao();
                        LimparEnderecoNascimento();

                        LimparTela();
                        CarregarDadosDrop(ddlPaisNasc.ID);
                        CarregarDadosDrop(ddlNacionalidade.ID);
                        CarregarDadosDrop(ddlPaisResid.ID);
                        CarregarDadosDrop(ddlRG_Tipo.ID);
                        CarregarDadosDrop(ddDOC_Rg_Emissor.ID);
                        CarregarDadosDrop(ddlEstadoCivil.ID);
                        CarregarDadosDrop(ddDOC_Rg_Uf.ID);
                        CarregarDadosDrop(ddDlCprof_Uf.ID);
                        CarregarDadosDrop(ddDOC_CertNasc_Uf.ID);
                        CarregarDadosDrop(ddlEtnia.ID);
                        CarregaNecessidadeEspecial();

                        lbltxtPessoa.Visible = false;
                        txtPessoa.Visible = false;

                        var dadosPessoa = Pessoa.Carregar(Convert.ToInt32(tsePessoa.DBValue.ToString()));

                        CarregaDadosPessoa(dadosPessoa);
                        if (!string.IsNullOrEmpty(txtPessoa.Text))
                        {
                            pcPessoa.TabPages[5].Enabled = true;
                            pcPessoa.TabPages[6].Enabled = true;
                        }
                        txtPessoa.Visible = true;

                        RN.RecursosHumanos.Entidades.GoogleEducation googleEducation = new Techne.Lyceum.RN.RecursosHumanos.Entidades.GoogleEducation();
                        RN.RecursosHumanos.GoogleEducation rnGoogleEducation = new Techne.Lyceum.RN.RecursosHumanos.GoogleEducation();

                        googleEducation = rnGoogleEducation.ObtemPor(Convert.ToDecimal(dadosPessoa.Pessoa));

                        if (!googleEducation.Email.IsNullOrEmptyOrWhiteSpace())
                        {
                            txtEmailGoogle.Text = googleEducation.Email.Trim();
                        }
                        break;
                    }
            }

        }
        private bool ValidarPreenchimento(int tipo)
        {
            string _message = "Não é possível deixar de preencher os seguintes campos.<br>Campos Necessários: ";
            bool Valido = true;

            if (!tseServidor.IsValidDBValue || tseServidor.DBValue.IsNull)
            {
                _message += "- Pessoa ";
                Valido = false;
            }

            if ((tipo == 1 ? ddlEscolaridade.SelectedValue : ddlEscolaridadePosGraduacao.SelectedValue) == "Selecione")
            {
                _message += "- Escolaridade ";
                Valido = false;
            }
            if ((tipo == 1 ? ddlSituacaoCurso.SelectedValue : ddlSituacaoCursoPosGraduacao.SelectedValue) == "Selecione")
            {
                _message += "- Situação do Curso ";
                Valido = false;
            }
            if ((tipo == 1 ? ddlAreaCurso.SelectedValue : ddlAreaCursoPosGraduacao.SelectedValue) == "Selecione")
            {
                _message += "- Área do Curso ";
                Valido = false;
            }
            if ((tipo == 1 ? ddlCurso.SelectedValue : ddlCursoPosGraduacao.SelectedValue) == "Selecione")
            {
                _message += "- Curso ";
                Valido = false;
            }
            if ((tipo == 1 ? ddlFormComplementPedag.SelectedValue : ddlFormComplementPedagPosGraduacao.SelectedValue) == "Selecione")
            {
                _message += "- Formação/Complementação Pedagógica ";
                Valido = false;
            }
            if (string.IsNullOrEmpty(tipo == 1 ? txtAnoInicio.Text.Trim() : txtAnoInicioPosGraduacao.Text.Trim()))
            {
                _message += "- Ano de Início ";
                Valido = false;
            }
            if ((tipo == 1 ? ddlTipoInstituicao.SelectedValue : ddlTipoInstituicaoPosGraduacao.SelectedValue) == "Selecione")
            {
                _message += "- Tipo de Instituição ";
                Valido = false;
            }
            if (!(tipo == 1 ? tseInstituicao.IsValidDBValue : tseInstituicaoPosGraduacao.IsValidDBValue) || (tipo == 1 ? tseInstituicao.DBValue.IsNull : tseInstituicaoPosGraduacao.DBValue.IsNull))
            {
                _message += "- Nome da Instituição ";
                Valido = false;
            }

            if (!Valido)
            {
                lblMensagem.Text = _message;
            }

            return Valido;
        }

        protected void ddlEscolaridade_SelectedIndexChanged(object sender, EventArgs e)
        {
            string nomeEscolaridade = ddlEscolaridade.SelectedValue.ToString().Trim();

            if (nomeEscolaridade == "Ensino Médio Normal/Magistério" ||
               nomeEscolaridade == "Ensino Médio Normal/Magistério Específico Indígena" ||
               nomeEscolaridade == "Ensino Médio Normal/Magistério - Estudos Adicionais")
            {
                ddlSituacaoCurso.Text = "Concluído";
                ddlSituacaoCurso.Enabled = false;

            }
            else
            {
                ddlSituacaoCurso.Enabled = true;
            }

            if (nomeEscolaridade == "Superior Licenciatura")
            {
                ddlFormComplementPedag.Text = "Não";
                ddlFormComplementPedag.Enabled = false;
            }

            int posadicional = nomeEscolaridade.ToUpper().IndexOf("ADICIONAIS");

        }

        protected void btnSalvarFormacao_Click(object sender, EventArgs e)
        {
            blocoGravacaoFormacao(1); //Graduação

        }
        private bool VerificarCampos(int tipo)
        {
            string _message = "Verificações:<br> ";
            bool Valido = true;

            if (((tipo == 1 ? ddlSituacaoCurso.SelectedValue : ddlSituacaoCursoPosGraduacao.SelectedValue) == "Concluído") && (string.IsNullOrEmpty((tipo == 1 ? txtAnoConclusao.Text.Trim() : txtAnoConclusaoPosGraduacao.Text.Trim()))))
            {
                _message += "- O campo 'Ano de Conclusão' deve ser preenchido.";
                Valido = false;
            }
            if (!string.IsNullOrEmpty((tipo == 1 ? txtAnoInicio.Text.Trim() : txtAnoInicioPosGraduacao.Text.Trim())))
            {
                if (int.Parse((tipo == 1 ? txtAnoInicio.Text.Trim() : txtAnoInicioPosGraduacao.Text.Trim())) < 1930)
                {
                    _message += "- O campo 'Ano de Início' deve ser maior que 1930.";
                    Valido = false;
                }
                if (int.Parse((tipo == 1 ? txtAnoInicio.Text.Trim() : txtAnoInicioPosGraduacao.Text.Trim())) == 0)
                {
                    _message += "- O campo 'Ano de Início' não pode ser igual a zero(0).";
                    Valido = false;
                }
                if ((tipo == 1 ? txtAnoInicio.Text.Length : txtAnoInicioPosGraduacao.Text.Length) != 4)
                {
                    _message += "- O campo 'Ano de Início' deve ter 4 dígitos.";
                    Valido = false;
                }
                if (int.Parse((tipo == 1 ? txtAnoInicio.Text.Trim() : txtAnoInicioPosGraduacao.Text.Trim())) > DateTime.Now.Year)
                {
                    _message += "- O campo 'Ano de Início' não pode ser maior que ano vigente.";
                    Valido = false;
                }

            }
            if (!string.IsNullOrEmpty((tipo == 1 ? txtAnoConclusao.Text.Trim() : txtAnoConclusaoPosGraduacao.Text.Trim())))
            {
                if (int.Parse((tipo == 1 ? txtAnoConclusao.Text.Trim() : txtAnoConclusaoPosGraduacao.Text.Trim())) < 1930)
                {
                    _message += "- O campo 'Ano de Conclusão' deve ser maior que 1930.";
                    Valido = false;
                }
                if (int.Parse((tipo == 1 ? txtAnoConclusao.Text.Trim() : txtAnoConclusaoPosGraduacao.Text.Trim())) == 0)
                {
                    _message += "- O campo 'Ano de Conclusão' não pode ser igual a zero(0).";
                    Valido = false;
                }
                if ((tipo == 1 ? txtAnoConclusao.Text.Length : txtAnoConclusaoPosGraduacao.Text.Length) != 4)
                {
                    _message += "- O campo 'Ano de Conclusão' deve ter 4 dígitos.";
                    Valido = false;
                }
            }

            if (!string.IsNullOrEmpty(tipo == 1 ? txtAnoInicio.Text.Trim() : txtAnoInicioPosGraduacao.Text.Trim()) && !string.IsNullOrEmpty(tipo == 1 ? txtAnoConclusao.Text.Trim() : txtAnoConclusaoPosGraduacao.Text.Trim()))
            {
                if (int.Parse(tipo == 1 ? txtAnoInicio.Text.Trim() : txtAnoInicioPosGraduacao.Text.Trim()) > int.Parse(tipo == 1 ? txtAnoConclusao.Text.Trim() : txtAnoConclusaoPosGraduacao.Text.Trim()))
                {
                    _message += "- Ano de Início não pode ser superior ao Ano de Conclusão ";
                    Valido = false;
                }
            }

            if (!Valido)
            {
                lblMensagem.Text = _message;
            }

            return Valido;
        }

        private void LimparCampos()
        {
            ddlEscolaridade.ClearSelection();
            ddlSituacaoCurso.ClearSelection();
            ddlAreaCurso.ClearSelection();
            ddlCurso.Items.Clear();
            ddlFormComplementPedag.ClearSelection();
            txtAnoConclusao.Text = string.Empty;
            txtAnoInicio.Text = string.Empty;
            ddlTipoInstituicao.ClearSelection();
            tseInstituicao.ResetValue();
            ckDocComprob.Checked = false;
        }

        public void Delete(object ID_FORMACAO_PESSOAL)
        {

        }
        private void CarregaDadosPessoa(LyPessoa dadosPessoa)
        {
            RN.FlPessoa rnFlPessoa = new FlPessoa();

            if (!string.IsNullOrEmpty(dadosPessoa.Pais_nasc))
            {
                string descricaoPais = RN.Endereco.ObterPais(dadosPessoa.Pais_nasc);

                //verifica se valor não é Brasil
                if (descricaoPais.ToUpper() != "BRASIL")
                {
                    //obtém o municipio estrangeiro
                    SimpleRow sr = RN.Endereco.ObterMunicipioEstrangeiro(dadosPessoa.Municipio_nasc);

                    //verifica se a função retornou algum valor para a simplerow
                    if (sr != null)
                    {
                        //preenche os dados obtidos de municipio estrangeiro
                        if (!sr["nome"].IsNull)
                            txtMunicipioNaturalidade.Text = Convert.ToString(sr["nome"]);

                        if (!sr["nome_estado"].IsNull)
                            txtEstadoNaturalidade.Value = Convert.ToString(sr["nome_estado"]);
                    }
                }
                else //se for Brasil
                {
                    //verifica se existe valor para municipio
                    if (!string.IsNullOrEmpty(dadosPessoa.Municipio_nasc))
                    {
                        //preenche os dados nos controles da tela
                        tseMunicipioNaturalidade.DBValue = dadosPessoa.Municipio_nasc;
                        //obtém a UF de acordo com o codigo do municipío
                        txtEstadoNaturalidade.Value = RN.Endereco.ObterUFMunicipio(dadosPessoa.Municipio_nasc);

                        if (!tseMunicipioNaturalidade.IsValidDBValue)
                        {
                            tseMunicipioNaturalidade.DBValue = string.Empty;
                        }
                    }
                    else
                    {
                        tseMunicipioNaturalidade.ResetValue();
                        txtEstadoNaturalidade.Value = string.Empty;
                    }
                }
            }

            txtNomeMae.Text = dadosPessoa.NomeMae;
            txtNomePai.Text = dadosPessoa.NomePai;

            chkMaeNaoDeclarada.Checked = dadosPessoa.NomeMae == chkMaeNaoDeclarada.Text.ToUpper();
            chkPaiNaoDeclarado.Checked = dadosPessoa.NomePai == chkPaiNaoDeclarado.Text.ToUpper();

            if (chkMaeNaoDeclarada.Checked)
            {
                txtNomeMae.ReadOnly = true;
            }

            if (chkPaiNaoDeclarado.Checked)
            {
                txtNomePai.ReadOnly = true;
            }

            //verifica se retornou valor para pais
            if (!string.IsNullOrEmpty(dadosPessoa.End_pais))
            {
                string descricaoPais = RN.Endereco.ObterPais(dadosPessoa.End_pais);

                //verifica se valor não é Brasil
                if (descricaoPais.ToUpper() != "BRASIL" && dadosPessoa.End_pais.ToUpper() != "BRASIL")
                {
                    //obtém o municipio estrangeiro
                    SimpleRow sr = RN.Endereco.ObterMunicipioEstrangeiro(dadosPessoa.End_municipio);

                    //verifica se a função retornou algum valor para a simplerow
                    if (sr != null)
                    {
                        //preenche os dados obtidos de municipio estrangeiro
                        if (!sr["nome"].IsNull)
                            txtMunicipio.Text = Convert.ToString(sr["nome"]);

                        if (!sr["nome_estado"].IsNull)
                            txtEstado.Value = Convert.ToString(sr["nome_estado"]);
                    }
                }
                else //se for Brasil
                {
                    //verifica se existe valor para municipio
                    if (!string.IsNullOrEmpty(dadosPessoa.End_municipio))
                    {
                        //preenche os dados nos controles da tela
                        tseMunicipioResid.DBValue = dadosPessoa.End_municipio;
                        //obtém a UF de acordo com o codigo do municipío
                        txtEstado.Value = RN.Endereco.ObterUFMunicipio(dadosPessoa.End_municipio);

                        if (!tseMunicipioResid.IsValidDBValue)
                        {
                            tseMunicipioResid.DBValue = string.Empty;
                        }
                    }
                    else
                    {
                        tseMunicipioResid.ResetValue();
                        txtEstado.Value = string.Empty;
                    }
                }
            }

            if (!string.IsNullOrEmpty(dadosPessoa.Teleitor_mun))
            {
                tseMunicipioEleitor.DBValue = dadosPessoa.Teleitor_mun;
                txtEstadoEleitor.Value = tseMunicipioEleitor["uf_sigla"].ToString();
            }

            txtPessoa.Text = dadosPessoa.Pessoa.ToString();
            txtPessoaHid.Text = txtPessoa.Text;
            if (dadosPessoa.IdFuncional != null && dadosPessoa.IdFuncional == 0)
            {
                chkNaoPossuiIdFuncional.Checked = true;
                txtIdFuncional.Text = string.Empty;
                txtIdFuncionalAtualizacao.Text = string.Empty;
            }
            else
            {
                chkNaoPossuiIdFuncional.Checked = false;
                txtIdFuncional.Text = dadosPessoa.IdFuncional > 0 ? dadosPessoa.IdFuncional.ToString() : string.Empty;
                txtIdFuncionalAtualizacao.Text = dadosPessoa.IdFuncional > 0 ? dadosPessoa.IdFuncional.ToString() : string.Empty;
            }

            txtNomeSocial.Text = dadosPessoa.Nome_social;
            txtNomeCompl.Text = dadosPessoa.Nome_compl;
            txtCEPResid.Text = dadosPessoa.Cep;
            txtEnderecoResid.Text = dadosPessoa.Endereco;
            txtEndNumResid.Text = dadosPessoa.End_num;
            txtComplementoResid.Text = dadosPessoa.End_compl;
            txtBairroResid.Text = dadosPessoa.Bairro;

            Int64 result;
            if (Int64.TryParse(dadosPessoa.Fone, out result))
                txtTelefone.Text = string.Format("{0:(00)0000-0000}", result);
            else
                txtTelefone.Text = dadosPessoa.Fone;


            long resultado;
            if (long.TryParse(dadosPessoa.Celular, out resultado))
            {
                if (dadosPessoa.Celular.Length == 10)
                {
                    txtCelular.Text = string.Format("{0:(00)0000-0000}", resultado);
                }
                else
                {
                    txtCelular.Text = string.Format("{0:(00)00000-0000}", resultado);
                }
            }


            txtEmailInterno.Text = !dadosPessoa.E_mail_interno.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.E_mail_interno : string.Empty;
            txtEmailExterno.Text = !dadosPessoa.E_mail.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.E_mail : string.Empty;

            txtDOC_Rg_Num.Text = dadosPessoa.Rg_num;

            if (Int64.TryParse(dadosPessoa.Cpf, out result))
            {
                if (result != 0)
                    txtCPF.Text = string.Format(@"{0:000\.000\.000-00}", result);
                else
                    txtCPF.Text = string.Empty; ;
            }
            else
            {
                txtCPF.Text = dadosPessoa.Cpf;
            }

            if (txtCPF.Text.IsNullOrEmptyOrWhiteSpace())
            {
                txtCPF.Enabled = true;
            }
            else
            {
                txtCPF.Enabled = false;
            }

            txtPassaporte.Text = dadosPessoa.Passaporte;
            txtCrpof_Num.Text = dadosPessoa.Cprof_num;
            txtCprof_Serie.Text = dadosPessoa.Cprof_serie;
            txtDOC_Teleitor_Num.Text = dadosPessoa.Teleitor_num;
            txtDOC_Teleitor_Zona.Text = dadosPessoa.Teleitor_zona;
            txtDOC_Teleitor_Secao.Text = dadosPessoa.Teleitor_secao;
            txtDOC_CertNasc_Numero.Text = dadosPessoa.CertNascNum;
            txtDOC_CertNasc_Folha.Text = dadosPessoa.CertNascFolha;
            txtDOC_CertNasc_Livro.Text = dadosPessoa.CertNascLivro;
            txtNumMatriculaCertidao.Text = !dadosPessoa.CertNumeroMatricula.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.CertNumeroMatricula : string.Empty;

            if (dadosPessoa.CertNascEmissao.HasValue)
            {
                dboDOC_CertNasc_DtEmissao.Date = dadosPessoa.CertNascEmissao.Value;
            }

            // VERIFICAR
            PreencherDadoCombo(ddlUFCartorio, Convert.ToString(dadosPessoa.CodigoUf));

            if (!string.IsNullOrEmpty(dadosPessoa.CodigoUf))
            {
                PreencherDadoCombo(ddlMunicipioCartorio, Convert.ToString(dadosPessoa.CodigoMunicipio));
            }

            if (dadosPessoa.IdCartorio != null && dadosPessoa.IdCartorio != 0)
            {
                CarregaCartorio();
                PreencherDadoCombo(ddlCartorio, Convert.ToString(dadosPessoa.IdCartorio));
            }
            PreencherDadoCombo(ddlNecessidadeEspecial, Convert.ToString(dadosPessoa.NecessidadeEspecialId));


            if (!string.IsNullOrEmpty(dadosPessoa.Est_civil))
            {
                if (ddlEstadoCivil.Items.FindByValue(dadosPessoa.Est_civil) != null)
                {
                    ddlEstadoCivil.SelectedValue = dadosPessoa.Est_civil;
                }
                else
                {
                    ddlEstadoCivil.SelectedValue = string.Empty;
                }
            }

            if (!string.IsNullOrEmpty(dadosPessoa.Nacionalidade))
            {
                if (ddlNacionalidade.Items.FindByValue(dadosPessoa.Nacionalidade) != null)
                {
                    ddlNacionalidade.SelectedValue = dadosPessoa.Nacionalidade;
                }
            }

            txtDMIL_Alist_Num.Text = dadosPessoa.Alist_num;
            txtDMIL_Alist_RM.Text = dadosPessoa.Alist_rm;
            txtDMIL_Alist_Serie.Text = dadosPessoa.Alist_serie;
            txtDMIL_Alist_CSM.Text = dadosPessoa.Alist_csm;
            txtDMIL_Cr_Num.Text = dadosPessoa.Cr_num;
            txtDMIL_Cr_RM.Text = dadosPessoa.Cr_rm;
            txtDMIL_Cr_Serie.Text = dadosPessoa.Cr_serie;
            txtDMIL_Cr_CSM.Text = dadosPessoa.Cr_csm;
            txtDMIL_Cr_CAT.Text = dadosPessoa.Cr_cat;

            if (dadosPessoa.Dt_nasc.HasValue)
                dtDataNasc.Date = dadosPessoa.Dt_nasc.Value;
            if (dadosPessoa.Rg_dtexp.HasValue)
                dboDOC_Rg_Dtexp.Date = dadosPessoa.Rg_dtexp.Value;
            if (dadosPessoa.Cprof_dtexp.HasValue)
                dboCprof_DtExp.Date = dadosPessoa.Cprof_dtexp.Value;
            if (dadosPessoa.Teleitor_dtexp.HasValue)
                dboDOC_Teleitor_DtExp.Date = dadosPessoa.Teleitor_dtexp.Value;

            if (dadosPessoa.CertNascEmissao.HasValue)
                dboDOC_CertNasc_DtEmissao.Date = dadosPessoa.CertNascEmissao.Value;

            if (dadosPessoa.Alist_dtexp.HasValue)
                dboDMIL_Alist_DtExp.Date = dadosPessoa.Alist_dtexp.Value;
            if (dadosPessoa.Cr_dtexp.HasValue)
                dboDMIL_Cr_DtExp.Date = dadosPessoa.Cr_dtexp.Value;


            if (!string.IsNullOrEmpty(dadosPessoa.Sexo))
            {
                if (rblSexo.Items.FindByValue(dadosPessoa.Sexo) != null)
                {
                    rblSexo.Text = dadosPessoa.Sexo;
                }
            }

            PreencherDadoCombo(ddlPaisNasc, dadosPessoa.Pais_nasc);
            PreencherDadoCombo(ddlPaisResid, dadosPessoa.End_pais);
            PreencherDadoCombo(ddlRG_Tipo, dadosPessoa.Rg_tipo);
            PreencherDadoCombo(ddDOC_Rg_Emissor, dadosPessoa.Rg_emissor);
            PreencherDadoCombo(ddDOC_Rg_Uf, dadosPessoa.Rg_uf);
            PreencherDadoCombo(ddDlCprof_Uf, dadosPessoa.Cprof_uf);
            PreencherDadoCombo(ddlEtnia, dadosPessoa.Etnia);

            chkAreaAssentamento.Checked = !dadosPessoa.AreaAssentamento.IsNullOrEmptyOrWhiteSpace() ? (dadosPessoa.AreaAssentamento == "S" ? true : false) : false;
            chkTerraIndigena.Checked = !dadosPessoa.TerraIndigena.IsNullOrEmptyOrWhiteSpace() ? (dadosPessoa.TerraIndigena == "S" ? true : false) : false;
            chkQuilombos.Checked = !dadosPessoa.AreaQuilombos.IsNullOrEmptyOrWhiteSpace() ? (dadosPessoa.AreaQuilombos == "S" ? true : false) : false;
            chkNaoSeAplica.Checked = (!chkAreaAssentamento.Checked && !chkTerraIndigena.Checked && !chkQuilombos.Checked) ? true : false;

            //Busca Zona Residencial
            string zonaResidencial = rnFlPessoa.ObtemZonaResidencialPor(dadosPessoa.Pessoa);
            if (!zonaResidencial.IsNullOrEmptyOrWhiteSpace())
            {
                rblLocalizacaoUF.SelectedValue = zonaResidencial;
            }

            ddlPovoIndigena.Visible = false;
            lblPovo.Visible = false;
            ddlPovoIndigena.ClearSelection();
            if (ddlEtnia.SelectedValue == "Índigena")
            {
                CarregaPovoIndigena();                
                ddlPovoIndigena.Visible = true;
                lblPovo.Visible = true;

                string povoIndigena = rnFlPessoa.ObtemPovoIndigenaPor(dadosPessoa.Pessoa);
                if (!povoIndigena.IsNullOrEmptyOrWhiteSpace())
                {
                    ddlPovoIndigena.SelectedValue = povoIndigena;
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
        }

        private void CarregarDropDownList(DropDownList drop, object data, string defaultValue)
        {
            drop.SelectedIndex = -1;
            drop.Items.Clear();
            drop.SelectedValue = null;
            drop.DataSource = data;
            drop.DataBind();
            ListItem itemVazio = new ListItem("Selecione", "");
            drop.Items.Insert(0, itemVazio);

            if (_tipoOperacao.Equals(TipoOperacao.Novo))
            {
                drop.SelectedValue = "";
                if (drop == ddlPaisResid || drop == ddlPaisNasc)
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

        private object CarregarDadosDrop(string idDrop)
        {
            QueryTable dadosDrop = null;

            try
            {
                switch (idDrop)
                {
                    case "ddlNacionalidade":
                        {
                            dadosDrop = RN.Basico.ConsultarNacionalidade();
                            CarregarDropDownList(ddlNacionalidade, dadosDrop, "");
                            break;
                        }
                    case "ddlPaisNasc":
                        {
                            dadosDrop = RN.Basico.ConsultarPais();
                            CarregarDropDownList(ddlPaisNasc, dadosDrop, "");
                            break;
                        }
                    case "ddlPaisResid":
                        {
                            dadosDrop = RN.Basico.ConsultarPais();
                            CarregarDropDownList(ddlPaisResid, dadosDrop, "");
                            break;
                        }
                    case "ddlRG_Tipo":
                        {
                            string param = "TIPO DOC";
                            dadosDrop = RN.Basico.ConsultaItemTabelaValDescr(param);
                            CarregarDropDownList(ddlRG_Tipo, dadosDrop, "");
                            break;
                        }
                    case "ddDOC_Rg_Emissor":
                        {
                            dadosDrop = RN.Basico.ConsultaItemTabelaValDescr("Orgao RG");
                            CarregarDropDownList(ddDOC_Rg_Emissor, dadosDrop, "");
                            break;
                        }
                    case "ddlEstadoCivil":
                        {
                            dadosDrop = RN.Basico.ConsultaItemTabelaValDescr("Estado civil");
                            CarregarDropDownList(ddlEstadoCivil, dadosDrop, "");
                            break;
                        }
                    case "ddDOC_Rg_Uf":
                        {
                            dadosDrop = RN.Basico.ConsultarUF();
                            CarregarDropDownList(ddDOC_Rg_Uf, dadosDrop, "");
                            break;
                        }
                    case "ddDlCprof_Uf":
                        {
                            dadosDrop = RN.Basico.ConsultarUF();
                            CarregarDropDownList(ddDlCprof_Uf, dadosDrop, "");
                            break;
                        }
                    case "ddDOC_CertNasc_Uf":
                        {
                            dadosDrop = RN.Basico.ConsultarUF();
                            CarregarDropDownList(ddDOC_CertNasc_Uf, dadosDrop, "");
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

        protected void HabilitaCampos()
        {
            tseMunicipioNaturalidade.Mode = ControlMode.Edit;
            tseMunicipioEleitor.Mode = ControlMode.Edit;
            tseMunicipioResid.Mode = ControlMode.Edit;

            txtMunicipioNaturalidade.Enabled = true;
            txtEstadoNaturalidade.Attributes.Add("readonly", "readonly");

            txtMunicipio.Enabled = true;
            txtEstado.Attributes.Add("readonly", "readonly");

            txtNomeSocial.Enabled = true;
            txtNomeCompl.Enabled = true;
            txtNomeMae.ReadOnly = false;
            txtNomePai.ReadOnly = false;
            chkPaiNaoDeclarado.Enabled = true;
            chkMaeNaoDeclarada.Enabled = true;
            txtPISPASEP.Enabled = true;
            txtCEPResid.ReadOnly = false;
            txtEnderecoResid.Enabled = true;
            txtEndNumResid.Enabled = true;
            txtComplementoResid.Enabled = true;
            txtBairroResid.Enabled = true;
            rblLocalizacaoUF.Enabled = true;
            txtTelefone.Enabled = true;
            txtCelular.Enabled = true;
            txtEmailInterno.Enabled = true;
            txtEmailExterno.Enabled = true;
            txtEmailGoogle.Enabled = true;
            txtDOC_Rg_Num.Enabled = true;
            txtCPF.Enabled = true;
            txtPassaporte.Enabled = true;
            txtCrpof_Num.Enabled = true;
            txtCprof_Serie.Enabled = true;
            txtDOC_Teleitor_Num.Enabled = true;
            txtDOC_Teleitor_Zona.Enabled = true;
            txtDOC_Teleitor_Secao.Enabled = true;

            txtDOC_CertNasc_Numero.Enabled = true;
            txtDOC_CertNasc_Folha.Enabled = true;
            txtDOC_CertNasc_Livro.Enabled = true;
            ddlCartorio.Enabled = true;
            ddlTipoCertidao.Enabled = true;
            txtNumMatriculaCertidao.Enabled = true;
            ddlCertidaoCivil.Enabled = true;
            ddlUFCartorio.Enabled = true;
            ddlMunicipioCartorio.Enabled = true;
            dboDOC_CertNasc_DtEmissao.Enabled = true;
            ddDOC_CertNasc_Uf.Enabled = true;


            txtDMIL_Alist_Num.Enabled = true;
            txtDMIL_Alist_RM.Enabled = true;
            txtDMIL_Alist_Serie.Enabled = true;
            txtDMIL_Alist_CSM.Enabled = true;
            txtDMIL_Cr_Num.Enabled = true;
            txtDMIL_Cr_RM.Enabled = true;
            txtDMIL_Cr_Serie.Enabled = true;
            txtDMIL_Cr_CSM.Enabled = true;
            txtDMIL_Cr_CAT.Enabled = true;

            dtDataNasc.Enabled = true;
            dboDOC_Rg_Dtexp.Enabled = true;
            dboCprof_DtExp.Enabled = true;
            dboDOC_Teleitor_DtExp.Enabled = true;
            dboDMIL_Cr_DtExp.Enabled = true;
            dboDMIL_Alist_DtExp.Enabled = true;

            rblSexo.Enabled = true;
            ddlEstadoCivil.Enabled = true;
            ddlPaisNasc.Enabled = true;
            ddlNacionalidade.Enabled = true;
            ddlPaisResid.Enabled = true;
            ddlRG_Tipo.Enabled = true;
            ddDOC_Rg_Emissor.Enabled = true;
            ddDOC_Rg_Uf.Enabled = true;
            ddDlCprof_Uf.Enabled = true;
            ddDOC_CertNasc_Uf.Enabled = true;
            ddlEtnia.Enabled = true;
            ddlPovoIndigena.Enabled = true;
            ddlNecessidadeEspecial.Enabled = true;
            btnSalvarFormacao.Enabled = true;
            btnSalvarFormacaoPosGraduacao.Enabled = true;

            txtIdFuncional.Enabled = true;
            txtVinculo.Enabled = true;
            txtCHCargo.Enabled = true;
            dteDtNomeacao.Enabled = true;
            ddlCargo.Enabled = true;
            chkAreaAssentamento.Enabled = true;
            chkTerraIndigena.Enabled = true;
            chkQuilombos.Enabled = true;
            chkNaoSeAplica.Enabled = true;
        }

        protected void DesabilitaCampos()
        {
            tseMunicipioNaturalidade.Mode = ControlMode.View;
            tseMunicipioEleitor.Mode = ControlMode.View;
            tseMunicipioResid.Mode = ControlMode.View;

            txtMunicipioNaturalidade.Enabled = false;
            txtEstadoNaturalidade.Attributes.Add("readonly", "readonly");

            txtMunicipio.Enabled = false;
            txtEstado.Attributes.Add("readonly", "readonly");
            txtPISPASEP.Enabled = false;
            txtNomeCompl.Enabled = false;
            txtNomeSocial.Enabled = false;
            txtNomeMae.ReadOnly = true;
            txtNomePai.ReadOnly = true;
            chkPaiNaoDeclarado.Enabled = false;
            chkMaeNaoDeclarada.Enabled = false;
            txtCEPResid.Enabled = false;
            txtEnderecoResid.Enabled = false;
            txtEndNumResid.Enabled = false;
            txtComplementoResid.Enabled = false;
            txtBairroResid.Enabled = false;
            rblLocalizacaoUF.Enabled = false;
            txtTelefone.Enabled = false;
            txtCelular.Enabled = false;
            txtEmailInterno.Enabled = false;
            txtEmailExterno.Enabled = false;
            txtEmailGoogle.Enabled = false;
            txtDOC_Rg_Num.Enabled = false;
            txtCPF.Enabled = false;
            txtPassaporte.Enabled = false;
            txtCrpof_Num.Enabled = false;
            txtCprof_Serie.Enabled = false;
            txtDOC_Teleitor_Num.Enabled = false;
            txtDOC_Teleitor_Zona.Enabled = false;
            txtDOC_Teleitor_Secao.Enabled = false;
            dboDOC_Teleitor_DtExp.Enabled = false;

            txtDOC_CertNasc_Numero.Enabled = false;
            txtDOC_CertNasc_Folha.Enabled = false;
            txtDOC_CertNasc_Livro.Enabled = false;
            ddlCartorio.Enabled = false;
            ddlTipoCertidao.Enabled = false;
            txtNumMatriculaCertidao.Enabled = false;
            ddlCertidaoCivil.Enabled = false;
            ddlUFCartorio.Enabled = false;
            ddlMunicipioCartorio.Enabled = false;
            dboDOC_CertNasc_DtEmissao.Enabled = false;
            ddDOC_CertNasc_Uf.Enabled = false;

            txtDMIL_Alist_Num.Enabled = false;
            txtDMIL_Alist_RM.Enabled = false;
            txtDMIL_Alist_Serie.Enabled = false;
            txtDMIL_Alist_CSM.Enabled = false;
            dboDMIL_Alist_DtExp.Enabled = false;
            txtDMIL_Cr_Num.Enabled = false;
            txtDMIL_Cr_RM.Enabled = false;
            txtDMIL_Cr_Serie.Enabled = false;
            txtDMIL_Cr_CSM.Enabled = false;
            txtDMIL_Cr_CAT.Enabled = false;
            dboDMIL_Cr_DtExp.Enabled = false;

            dtDataNasc.Enabled = false;
            dboDOC_Rg_Dtexp.Enabled = false;
            dboCprof_DtExp.Enabled = false;

            rblSexo.Enabled = false;
            ddlEstadoCivil.Enabled = false;
            ddlPaisNasc.Enabled = false;
            ddlNacionalidade.Enabled = false;
            ddlPaisResid.Enabled = false;
            ddlRG_Tipo.Enabled = false;
            ddDOC_Rg_Emissor.Enabled = false;
            ddDOC_Rg_Uf.Enabled = false;
            ddDlCprof_Uf.Enabled = false;
            ddlEtnia.Enabled = false;
            ddlPovoIndigena.Enabled = false;
            ddlNecessidadeEspecial.Enabled = false;

            txtIdFuncional.Enabled = false;
            txtVinculo.Enabled = false;
            txtCHCargo.Enabled = false;
            dteDtNomeacao.Enabled = false;
            ddlCargo.Enabled = false;

            chkAreaAssentamento.Enabled = false;
            chkTerraIndigena.Enabled = false;
            chkQuilombos.Enabled = false;
            chkNaoSeAplica.Enabled = false;
        }

        protected void chkMaeNaoDeclarada_CheckedChanged(object sender, EventArgs e)
        {
            txtNomeMae.ReadOnly = false;
            txtNomeMae.Text = string.Empty;
            if (chkMaeNaoDeclarada.Checked)
            {
                txtNomeMae.Text = chkMaeNaoDeclarada.Text.ToUpper();
                txtNomeMae.ReadOnly = true;
            }
        }

        protected void chkPaiNaoDeclarado_CheckedChanged(object sender, EventArgs e)
        {
            txtNomePai.ReadOnly = false;
            txtNomePai.Text = string.Empty;
            if (chkPaiNaoDeclarado.Checked)
            {
                txtNomePai.Text = chkPaiNaoDeclarado.Text.ToUpper();
                txtNomePai.ReadOnly = true;
            }
        }

        private void LimparDadosIngresso()
        {
            txtMatricula.Text = string.Empty;
            txtIdFuncional.Text = string.Empty;
            txtVinculo.Text = string.Empty;
            txtCHCargo.Text = string.Empty;
            dteDtNomeacao.Text = string.Empty;
            ddlCargo.ClearSelection();
        }

        private void LimparTela()
        {
            LimparFiliacao();
            tseMunicipioNaturalidade.ResetValue();
            tseMunicipioEleitor.ResetValue();
            tseMunicipioResid.ResetValue();

            txtEstado.Value = string.Empty;
            txtEstadoEleitor.Value = string.Empty;
            txtEstadoNaturalidade.Value = string.Empty;

            txtPessoa.Text = string.Empty;
            txtPessoaHid.Text = txtPessoa.Text;
            txtNomeCompl.Text = string.Empty;
            txtNomeSocial.Text = string.Empty;
            txtCEPResid.Text = string.Empty;
            txtEnderecoResid.Text = string.Empty;
            txtEndNumResid.Text = string.Empty;
            txtComplementoResid.Text = string.Empty;
            txtBairroResid.Text = string.Empty;
            rblLocalizacaoUF.SelectedIndex = -1;
            txtTelefone.Text = string.Empty;
            txtCelular.Text = string.Empty;
            txtEmailInterno.Text = string.Empty;
            txtEmailExterno.Text = string.Empty;
            txtEmailGoogle.Text = string.Empty;
            ddlRG_Tipo.SelectedValue = string.Empty;
            ddDOC_Rg_Emissor.SelectedValue = string.Empty;
            txtDOC_Rg_Num.Text = string.Empty;
            ddDOC_Rg_Uf.ClearSelection();
            txtCPF.Text = string.Empty;
            txtPassaporte.Text = string.Empty;
            txtCrpof_Num.Text = string.Empty;
            txtCprof_Serie.Text = string.Empty;
            txtDOC_Teleitor_Num.Text = string.Empty;
            txtDOC_Teleitor_Zona.Text = string.Empty;
            txtDOC_Teleitor_Secao.Text = string.Empty;
            txtDOC_CertNasc_Numero.Text = string.Empty;
            txtDOC_CertNasc_Folha.Text = string.Empty;
            txtDOC_CertNasc_Livro.Text = string.Empty;

            txtDMIL_Alist_Num.Text = string.Empty;
            txtDMIL_Alist_RM.Text = string.Empty;
            txtDMIL_Alist_Serie.Text = string.Empty;
            txtDMIL_Alist_CSM.Text = string.Empty;
            txtDMIL_Cr_Num.Text = string.Empty;
            txtDMIL_Cr_RM.Text = string.Empty;
            txtDMIL_Cr_Serie.Text = string.Empty;
            txtDMIL_Cr_CSM.Text = string.Empty;
            txtDMIL_Cr_CAT.Text = string.Empty;

            dtDataNasc.Text = string.Empty;
            dboDOC_Rg_Dtexp.Text = string.Empty;
            dboCprof_DtExp.Text = string.Empty;
            dboDOC_Teleitor_DtExp.Text = string.Empty;
            dboDMIL_Cr_DtExp.Text = string.Empty;
            dboDMIL_Alist_DtExp.Text = string.Empty;

            rblSexo.ClearSelection();
            ddlEstadoCivil.SelectedValue = "";
            ddlPaisNasc.Items.Clear();
            ddlNacionalidade.ClearSelection();
            ddlPaisResid.Items.Clear();
            ddlRG_Tipo.ClearSelection();
            ddDOC_Rg_Emissor.ClearSelection();
            ddDlCprof_Uf.ClearSelection();
            ddDOC_CertNasc_Uf.ClearSelection();
            ddlEtnia.ClearSelection(); 
            ddlPovoIndigena.Visible = false;
            lblPovo.Visible = false;
            ddlPovoIndigena.ClearSelection();
            ddlNecessidadeEspecial.ClearSelection();
            txtPISPASEP.Text = string.Empty;

            txtMatricula.Text = string.Empty;
            txtIdFuncional.Text = string.Empty;
            txtVinculo.Text = string.Empty;
            txtCHCargo.Text = string.Empty;
            dteDtNomeacao.Text = string.Empty;
            ddlCargo.ClearSelection();
            chkAreaAssentamento.Checked = false;
            chkTerraIndigena.Checked = false;
            chkQuilombos.Checked = false;
            chkNaoSeAplica.Checked = false;
        }

        protected void ddlPaisResid_Changed(object sender, EventArgs e)
        {
            LimparEndereco();
        }

        protected void ddlPaisNasc_Changed(object sender, EventArgs e)
        {
            LimparEndereco();
        }

        protected void tseServidor_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseServidor_Changed(object sender, EventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                    return;

                if (!tseServidor.DBValue.IsNull)
                {
                    if (tseServidor.IsValidDBValue)
                    {
                        _tipoOperacao = TipoOperacao.Consultar;
                        lblMensagem.Text = string.Empty;

                    }
                    else
                    {
                        lblMensagem.Text = "Pessoa não cadastrada.";
                        _tipoOperacao = TipoOperacao.Inicial;
                    }
                }
                else
                {
                    lblMensagem.Text = "Pessoa não cadastrada.";
                    _tipoOperacao = TipoOperacao.Inicial;
                }

                ControlarTipoOperacao();


                if (string.IsNullOrEmpty(txtPessoa.Text.Trim()))
                {
                    txtPessoa.Text = "0";
                }


                odsFormacaoPessoal.Select();
                odsFormacaoPessoal.DataBind();
                grdFormacaoPessoal.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tsePessoa_Changed(object sender, EventArgs args)
        {
            try
            {
                if (!tsePessoa.DBValue.IsNull)
                {
                    if (tsePessoa.IsValidDBValue)
                    {
                        _tipoOperacao = TipoOperacao.ConsultarPessoa;
                        ControlarTipoOperacao();
                    }
                    else
                        lblMensagem.Text = "Pessoa não cadastrada.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }
        protected void tsePessoa_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void ddlRGTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlRG_Tipo.SelectedValue == "RG")
                {
                    NumeroRG.Text = "Número*: ";
                    NumeroRG.Font.Bold = true;

                    lblRGUF.Text = "Estado*: ";
                    lblRGUF.Font.Bold = true;

                    lblRGEmissor.Text = "Órgão Emissor*: ";
                    lblRGEmissor.Font.Bold = true;

                    lblDataExp.Text = "Data de Expedição*: ";
                    lblDataExp.Font.Bold = true;
                }
                else if (ddlRG_Tipo.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    NumeroRG.Text = "Número: ";
                    NumeroRG.Font.Bold = false;

                    lblRGUF.Text = "Estado: ";
                    lblRGUF.Font.Bold = false;

                    lblRGEmissor.Text = "Órgão Emissor: ";
                    lblRGEmissor.Font.Bold = false;

                    lblDataExp.Text = "Data de Expedição: ";
                    lblDataExp.Font.Bold = false;
                }
                else
                {

                    NumeroRG.Text = "Número*: ";
                    NumeroRG.Font.Bold = true;

                    lblRGUF.Text = "Estado: ";
                    lblRGUF.Font.Bold = false;

                    lblRGEmissor.Text = "Órgão Emissor*: ";
                    lblRGEmissor.Font.Bold = false;

                    lblDataExp.Text = "Data de Expedição: ";
                    lblDataExp.Font.Bold = false;
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
                tsePessoa.ResetValue();
                _tipoOperacao = TipoOperacao.Inicial;
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
                ControlarTSearchs();
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
                _tipoOperacao = TipoOperacao.Excluir;
                ControlarTipoOperacao();
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
                RN.VinculoLy rnVinculo = new VinculoLy();
                string mensagem = string.Empty;
                string naturalidade = string.Empty;
                string municipioEstrangeiro = string.Empty;
                string zonaResidencial = null;
                RN.RecursosHumanos.Entidades.GoogleEducation googleEducation = new Techne.Lyceum.RN.RecursosHumanos.Entidades.GoogleEducation();

                if (!ddlPaisNasc.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    if (ddlPaisNasc.SelectedItem.Text.ToUpper() == "BRASIL")
                    {
                        naturalidade = (!tseMunicipioNaturalidade.DBValue.IsNull && tseMunicipioNaturalidade.IsValidDBValue) ? Convert.ToString(tseMunicipioNaturalidade.DBValue) : null;
                    }
                    else
                    {
                        // obtém o municipio estrangeiro
                        SimpleRow sr = Endereco.ObterCodigoMunicipioEstrangeiro(txtMunicipioNaturalidade.Text.Trim());

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
                    Pessoa = !txtPessoa.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(txtPessoa.Text) : 0,
                    IdFuncional = (!txtIdFuncional.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtIdFuncional.Text) : (int?)null),
                    Nome_compl = !txtNomeCompl.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeCompl.Text.Trim().ToUpper() : null,
                    Nome_social = !txtNomeSocial.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeSocial.Text.Trim().ToUpper() : null,
                    Dt_nasc = !dtDataNasc.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataNasc.Date : (DateTime?)null,
                    NomeMae = txtNomeMae.Text.TrimEnd(),
                    NomePai = txtNomePai.Text.TrimEnd(),
                    Sexo = !rblSexo.Text.IsNullOrEmptyOrWhiteSpace() ? rblSexo.Text : null,
                    Est_civil = !ddlEstadoCivil.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlEstadoCivil.SelectedValue : null,
                    NecessidadeEspecialId = !ddlNecessidadeEspecial.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlNecessidadeEspecial.SelectedValue) : (int?)null,
                    Etnia = !ddlEtnia.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlEtnia.SelectedValue : null,
                    Nacionalidade = !ddlNacionalidade.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlNacionalidade.SelectedValue : null,
                    Pais_nasc = !ddlPaisNasc.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlPaisNasc.SelectedValue : null,
                    Endereco = !txtEnderecoResid.Text.IsNullOrEmptyOrWhiteSpace() ? txtEnderecoResid.Text.Trim() : null,
                    End_num = !txtEndNumResid.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndNumResid.Text.Trim() : null,
                    End_compl = !txtComplementoResid.Text.IsNullOrEmptyOrWhiteSpace() ? txtComplementoResid.Text.Trim() : null,
                    Cep = !txtCEPResid.Text.RetirarCaracteres().IsNullOrEmptyOrWhiteSpace() ? txtCEPResid.Text.RetirarCaracteres() : null,
                    Bairro = !txtBairroResid.Text.IsNullOrEmptyOrWhiteSpace() ? txtBairroResid.Text.Trim() : null,
                    End_municipio = txtMunicipio.Visible == true ? txtMunicipio.Text : (tseMunicipioResid.Visible == true ? tseMunicipioResid.DBValue.ToString() : null),
                    End_pais = !ddlPaisResid.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlPaisResid.SelectedValue : null,
                    Fone = txtTelefone.Text.Trim(),
                    Celular = txtCelular.Text.Trim(),
                    E_mail = !txtEmailExterno.Text.IsNullOrEmptyOrWhiteSpace() ? txtEmailExterno.Text.Trim() : null,
                    E_mail_interno = !txtEmailInterno.Text.IsNullOrEmptyOrWhiteSpace() ? txtEmailInterno.Text.Trim() : null,
                    Rg_tipo = !ddlRG_Tipo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlRG_Tipo.SelectedValue : null,
                    Rg_num = !txtDOC_Rg_Num.Text.IsNullOrEmptyOrWhiteSpace() ? txtDOC_Rg_Num.Text.Trim() : null,
                    Rg_uf = !ddDOC_Rg_Uf.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddDOC_Rg_Uf.SelectedValue : null,
                    Rg_emissor = !ddDOC_Rg_Emissor.Text.IsNullOrEmptyOrWhiteSpace() ? ddDOC_Rg_Emissor.Text.Trim() : null,
                    Rg_dtexp = !dboDOC_Rg_Dtexp.Text.IsNullOrEmptyOrWhiteSpace() ? dboDOC_Rg_Dtexp.Date : (DateTime?)null,
                    Cpf = !txtCPF.Text.RetirarMascaraCPF().IsNullOrEmptyOrWhiteSpace() ? txtCPF.Text.RetirarMascaraCPF().Trim() : null,
                    Passaporte = !txtPassaporte.Text.RetirarMascaraCPF().IsNullOrEmptyOrWhiteSpace() ? txtPassaporte.Text.Trim() : null,
                    CertNascNum = !txtDOC_CertNasc_Numero.Text.IsNullOrEmptyOrWhiteSpace() ? txtDOC_CertNasc_Numero.Text.Trim() : null,
                    CertNascFolha = !txtDOC_CertNasc_Folha.Text.IsNullOrEmptyOrWhiteSpace() ? txtDOC_CertNasc_Folha.Text.Trim() : null,
                    CertNascLivro = !txtDOC_CertNasc_Livro.Text.IsNullOrEmptyOrWhiteSpace() ? txtDOC_CertNasc_Livro.Text.Trim() : null,
                    CertNascCartorioExped = !ddlCartorio.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlCartorio.SelectedItem.Text : null,
                    IdCartorio = !ddlCartorio.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? int.Parse(ddlCartorio.SelectedValue) : (int?)null,
                    CertNumeroMatricula = !txtNumMatriculaCertidao.Text.IsNullOrEmptyOrWhiteSpace() ? txtNumMatriculaCertidao.Text.Trim() : null,
                    CertNascEmissao = !dboDOC_CertNasc_DtEmissao.Text.IsNullOrEmptyOrWhiteSpace() ? dboDOC_CertNasc_DtEmissao.Date : (DateTime?)null,
                    CertNascCartorioUf = !ddDOC_CertNasc_Uf.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddDOC_CertNasc_Uf.SelectedValue : null,
                    Cprof_num = !txtCrpof_Num.Text.IsNullOrEmptyOrWhiteSpace() ? txtCrpof_Num.Text : null,
                    Cprof_serie = !txtCprof_Serie.Text.IsNullOrEmptyOrWhiteSpace() ? txtCprof_Serie.Text : null,
                    Cprof_dtexp = !dboCprof_DtExp.Text.IsNullOrEmptyOrWhiteSpace() ? dboCprof_DtExp.Date : (DateTime?)null,
                    Cprof_uf = !ddDlCprof_Uf.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddDlCprof_Uf.SelectedValue : null,
                    Teleitor_num = !txtDOC_Teleitor_Num.Text.IsNullOrEmptyOrWhiteSpace() ? txtDOC_Teleitor_Num.Text : null,
                    Teleitor_zona = !txtDOC_Teleitor_Zona.Text.IsNullOrEmptyOrWhiteSpace() ? txtDOC_Teleitor_Zona.Text : null,
                    Teleitor_secao = !txtDOC_Teleitor_Secao.Text.IsNullOrEmptyOrWhiteSpace() ? txtDOC_Teleitor_Secao.Text : null,
                    Teleitor_dtexp = !dboDOC_Teleitor_DtExp.Text.IsNullOrEmptyOrWhiteSpace() ? dboDOC_Teleitor_DtExp.Date : (DateTime?)null,
                    Teleitor_mun = (!tseMunicipioEleitor.DBValue.IsNull && tseMunicipioEleitor.IsValidDBValue) ? tseMunicipioEleitor.DBValue.ToString() : null,
                    Alist_num = !txtDMIL_Alist_Num.Text.IsNullOrEmptyOrWhiteSpace() ? txtDMIL_Alist_Num.Text : null,
                    Alist_rm = !txtDMIL_Alist_RM.Text.IsNullOrEmptyOrWhiteSpace() ? txtDMIL_Alist_RM.Text : null,
                    Alist_serie = !txtDMIL_Alist_Serie.Text.IsNullOrEmptyOrWhiteSpace() ? txtDMIL_Alist_Serie.Text : null,
                    Alist_csm = !txtDMIL_Alist_CSM.Text.IsNullOrEmptyOrWhiteSpace() ? txtDMIL_Alist_CSM.Text : null,
                    Alist_dtexp = !dboDMIL_Alist_DtExp.Text.IsNullOrEmptyOrWhiteSpace() ? dboDMIL_Alist_DtExp.Date : (DateTime?)null,
                    Cr_dtexp = !dboDMIL_Cr_DtExp.Text.IsNullOrEmptyOrWhiteSpace() ? dboDMIL_Cr_DtExp.Date : (DateTime?)null,
                    Cr_num = !txtDMIL_Cr_Num.Text.IsNullOrEmptyOrWhiteSpace() ? txtDMIL_Cr_Num.Text : null,
                    Cr_rm = !txtDMIL_Cr_RM.Text.IsNullOrEmptyOrWhiteSpace() ? txtDMIL_Cr_RM.Text : null,
                    Cr_serie = !txtDMIL_Cr_Serie.Text.IsNullOrEmptyOrWhiteSpace() ? txtDMIL_Cr_Serie.Text : null,
                    Cr_csm = !txtDMIL_Cr_CSM.Text.IsNullOrEmptyOrWhiteSpace() ? txtDMIL_Cr_CSM.Text : null,
                    Cr_cat = !txtDMIL_Cr_CAT.Text.IsNullOrEmptyOrWhiteSpace() ? txtDMIL_Cr_CAT.Text : null,
                    Pispasep = !txtPISPASEP.Text.IsNullOrEmptyOrWhiteSpace() ? txtPISPASEP.Text : null,
                    UsuarioId = User.Identity.Name,
                    Municipio_nasc = naturalidade,
                    AreaAssentamento = chkAreaAssentamento.Checked ? "S" : "N",
                    TerraIndigena = chkTerraIndigena.Checked ? "S" : "N",
                    AreaQuilombos = chkQuilombos.Checked ? "S" : "N"

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

                var vinculo = new LyVinculo
                {
                    Matricula = !txtMatricula.Text.IsNullOrEmptyOrWhiteSpace() ? txtMatricula.Text : null,
                    Vinculo = !txtVinculo.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtVinculo.Text) : (int?)null,
                    DataNomeacao = !dteDtNomeacao.Text.IsNullOrEmptyOrWhiteSpace() ? dteDtNomeacao.Date : DateTime.MinValue,
                    ChCategoria = !txtCHCargo.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(txtCHCargo.Text) : (Decimal?)null,
                    Categoria = !ddlCargo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlCargo.SelectedValue : null,
                    UsuarioId = User.Identity.Name
                };

                zonaResidencial = !rblLocalizacaoUF.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblLocalizacaoUF.SelectedValue : null;

                googleEducation.Pessoa = pessoa.Pessoa;
                googleEducation.Email = !txtEmailGoogle.Text.IsNullOrEmptyOrWhiteSpace() ? txtEmailGoogle.Text.Trim() : null;
                googleEducation.UsuarioId = User.Identity.Name;

                validacao = rnVinculo.ValidaServidor(pessoa, vinculo, _tipoOperacao.Equals(TipoOperacao.Alterar) ? false : true, (chkNaoSeAplica.Checked ? "S" : "N"), zonaResidencial, googleEducation,ddlPovoIndigena.SelectedValue);

                if (validacao.Valido)
                {
                    if (_tipoOperacao.Equals(TipoOperacao.Novo) || _tipoOperacao.Equals(TipoOperacao.ConsultarPessoa))
                    {
                        rnVinculo.InsereServidor(pessoa, vinculo, zonaResidencial,ddlPovoIndigena.SelectedValue);
                        txtPessoa.Text = Convert.ToString(pessoa.Pessoa);
                        pnlGridDadosIngresso.Visible = true;
                        lblMensagemIdFuncional.Text = string.Empty;
                        txtPessoaHid.Text = txtPessoa.Text;
                        tseServidor.DBValue = vinculo.Matricula;

                        mensagem = "Servidor inserido com sucesso.";

                    }
                    else if (_tipoOperacao.Equals(TipoOperacao.Alterar))
                    {
                        rnVinculo.AtualizaServidor(pessoa, zonaResidencial, googleEducation,ddlPovoIndigena.SelectedValue);
                        mensagem = "Servidor atualizado com sucesso.";
                        pcPessoa.ActiveTabIndex = 0;
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

        protected void ddlUFCartorio_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlMunicipioCartorio.ClearSelection();
                ddlCartorio.ClearSelection();

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
                ddlCartorio.ClearSelection();

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
                ddlMunicipioCartorio.ClearSelection();
                ddlCartorio.ClearSelection();
                dboDOC_CertNasc_DtEmissao.Text = string.Empty;
                ddDOC_CertNasc_Uf.ClearSelection();
                lblCertCivil.Visible = true;
                ddlCertidaoCivil.Visible = true;

                if (!string.IsNullOrEmpty(ddlTipoCertidao.SelectedValue))
                {
                    if (ddlTipoCertidao.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                    {
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

        protected void ddlCertidaoCivil_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtNumMatriculaCertidao.Text = string.Empty;
                txtDOC_CertNasc_Folha.Text = string.Empty;
                txtDOC_CertNasc_Livro.Text = string.Empty;
                txtDOC_CertNasc_Numero.Text = string.Empty;
                ddlUFCartorio.ClearSelection();
                ddlCartorio.ClearSelection();
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

        protected void ddlAreaCurso_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlAreaCurso.SelectedValue != "Selecione")
                {
                    ddlCurso.Items.Clear();
                    ddlCurso.DataSource = RN.CursoFormacaoPessoal.Listar(int.Parse(ddlAreaCurso.SelectedValue));
                    ddlCurso.Items.Insert(0, "Selecione");
                    ddlCurso.DataBind();
                    ddlCurso.Enabled = true;
                }
                else
                {
                    ddlCurso.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdDadosIngresso_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (grdDadosIngresso.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "data_nomeacao" || (e.Column.FieldName) == "data_desativacao")
                {
                    (e.Editor as ASPxDateEdit).MaxDate = DateTime.Now;
                }
                if ((e.Column.FieldName) == "matricula")
                {
                    e.Editor.ReadOnly = true;
                }

                if ((e.Column.FieldName) == "idfuncional")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.Value = txtIdFuncional.Text;
                }
            }
            else if (grdDadosIngresso.IsEditing)
            {
                if ((e.Column.FieldName) == "data_nomeacao" || (e.Column.FieldName) == "data_desativacao")
                {
                    (e.Editor as ASPxDateEdit).MaxDate = DateTime.Now;
                }
                if ((e.Column.FieldName) == "matricula" || (e.Column.FieldName) == "idfuncional")
                {
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "VINCULO")
                {
                    if (Convert.ToString(e.Editor.Value).IsNullOrEmptyOrWhiteSpace())
                    {
                        e.Editor.ReadOnly = false;
                    }
                    else
                    {
                        e.Editor.ReadOnly = true;
                    }
                }
            }
        }
        protected void grdDadosIngresso_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdDadosIngresso);
        }       

        protected void grdDadosIngresso_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.VinculoLy rnVinculo = new VinculoLy();
            LyVinculo vinculo = new LyVinculo();

            int? idFuncional = e.NewValues["idfuncional"] != null ? Convert.ToInt32(e.NewValues["idfuncional"]) : (int?)null;

            vinculo.Pessoa = !txtPessoa.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(txtPessoa.Text) : 0;
            vinculo.Vinculo = e.NewValues["VINCULO"] != null ? Convert.ToInt32(e.NewValues["VINCULO"]) : (int?)null;
            vinculo.DataNomeacao = e.NewValues["data_nomeacao"] != null ? Convert.ToDateTime(e.NewValues["data_nomeacao"]) : DateTime.MinValue;
            vinculo.DataDesativacao = e.NewValues["data_desativacao"] != null ? Convert.ToDateTime(e.NewValues["data_desativacao"]) : (DateTime?)null;
            vinculo.ChCategoria = e.NewValues["ch_categoria"] != null ? Convert.ToDecimal(e.NewValues["ch_categoria"]) : (Decimal?)null;
            vinculo.Categoria = e.NewValues["categoria"] != null ? Convert.ToString(e.NewValues["categoria"]) : null;
            vinculo.UsuarioId = User.Identity.Name;

            validacao = rnVinculo.Valida(vinculo, idFuncional, true);

            if (validacao.Valido)
            {
                rnVinculo.Insere(vinculo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdDadosIngresso.DataBind();

        }
        protected void grdDadosIngresso_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.VinculoLy rnVinculo = new VinculoLy();
            LyVinculo vinculo = new LyVinculo();

            int? idFuncional = e.NewValues["idfuncional"] != null ? Convert.ToInt32(e.NewValues["idfuncional"]) : (int?)null;
            vinculo.Pessoa = !txtPessoa.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(txtPessoa.Text) : 0;
            vinculo.Matricula = e.NewValues["matricula"] != null ? Convert.ToString(e.NewValues["matricula"]) : null;
            vinculo.Vinculo = e.NewValues["VINCULO"] != null ? Convert.ToInt32(e.NewValues["VINCULO"]) : (int?)null;
            vinculo.Ordem = e.NewValues["ordem"] != null ? Convert.ToDecimal(e.NewValues["ordem"]) : 0;
            vinculo.DataNomeacao = e.NewValues["data_nomeacao"] != null ? Convert.ToDateTime(e.NewValues["data_nomeacao"]) : DateTime.MinValue;
            vinculo.DataDesativacao = e.NewValues["data_desativacao"] != null ? Convert.ToDateTime(e.NewValues["data_desativacao"]) : (DateTime?)null;
            vinculo.ChCategoria = e.NewValues["ch_categoria"] != null ? Convert.ToDecimal(e.NewValues["ch_categoria"]) : (Decimal?)null;
            vinculo.Categoria = e.NewValues["categoria"] != null ? Convert.ToString(e.NewValues["categoria"]) : null;
            vinculo.UsuarioId = User.Identity.Name;

            validacao = rnVinculo.Valida(vinculo, idFuncional, false);

            if (validacao.Valido)
            {
                rnVinculo.Atualiza(vinculo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdDadosIngresso.DataBind();
        }

        protected void grdDadosIngresso_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.VinculoLy rnVinculo = new VinculoLy();

            decimal pessoa = !txtPessoa.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(txtPessoa.Text) : 0;
            int ordem = e.Values["ordem"] != null ? Convert.ToInt32(e.Values["ordem"]) : 0;
            string matricula = e.Values["matricula"] != null ? Convert.ToString(e.Values["matricula"]) : null;

            validacao = rnVinculo.ValidaRemocao(pessoa, ordem, matricula);

            if (validacao.Valido)
            {
                rnVinculo.Remove(pessoa, ordem);
                grdDadosIngresso.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

        public void InsertVinculo(object idfuncional, object VINCULO, object matricula, object data_nomeacao, object ordem, object data_desativacao, object categoria, object ch_categoria) { }
        public void UpdateVinculo(object idfuncional, object VINCULO, object matricula, object data_nomeacao, object ordem, object data_desativacao, object categoria, object ch_categoria, object idvinculo) { }
        public void DeleteVinculo(object idvinculo,object ordem) { }

        protected void grdFormacaoPessoal_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string pessoa = Convert.ToString(e.GetListSourceFieldValue("pessoa"));
                string chave = Convert.ToString(e.GetListSourceFieldValue("chave"));
                e.Value = pessoa + "|" + chave;
            }
        }

        protected void grdFormacaoPessoal_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Keys.Clear();
            e.Keys.Add("pessoa", e.Values["pessoa"]);
            e.Keys.Add("chave", e.Values["chave"]);
        }

        protected void grdFormacaoPessoal_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["chave"] = RN.Pessoa.GeraOrdem(txtPessoa.Text.ToString());

            e.NewValues["pessoa"] = txtPessoa.Text.ToString();

            if (e.NewValues["entregou_doc"] == null)
            {
                e.NewValues["entregou_doc"] = "N";
            }
        }
        protected void grdFormacaoPessoal_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string[] chaves = e.Keys["CompositeKey"].ToString().Split('|');
            e.Keys.Clear();
            e.Keys.Add("pessoa", chaves[0]);
            e.Keys.Add("chave", chaves[1]);

        }

        public object ListarServidorCursoCapacitacao(string idPessoa)
        {
            if (!String.IsNullOrEmpty(idPessoa))
                return RN.Capacitacao.Listar(Convert.ToInt32(idPessoa));

            return null;
        }

        public object ListarAreaConhecimento()
        {
            return RN.AreaConhecimento.Listar();
        }

        public object ListarTipoCurso()
        {
            return RN.TipoCursoCapacitacao.Listar();
        }

        protected void ddlAreaCursoPosGraduacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlAreaCursoPosGraduacao.SelectedValue != "Selecione")
                {
                    ddlCursoPosGraduacao.Items.Clear();
                    ddlCursoPosGraduacao.DataSource = RN.CursoFormacaoPessoal.Listar(int.Parse(ddlAreaCursoPosGraduacao.SelectedValue));
                    ddlCursoPosGraduacao.Items.Insert(0, "Selecione");
                    ddlCursoPosGraduacao.DataBind();
                    ddlCursoPosGraduacao.Enabled = true;
                }
                else
                {
                    ddlCursoPosGraduacao.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlCurso_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCurso.SelectedItem.Text.Trim().ToUpper().IndexOf("LICENCIATURA") > 0)
                {
                    ddlFormComplementPedag.Text = "Não";
                    ddlFormComplementPedag.Enabled = false;
                }
                else
                {
                    ddlFormComplementPedag.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlCursoPosGraduacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCursoPosGraduacao.SelectedItem.Text.Trim().ToUpper().IndexOf("LICENCIATURA") > 0)
                {
                    ddlFormComplementPedagPosGraduacao.Text = "Não";
                    ddlFormComplementPedagPosGraduacao.Enabled = false;
                }
                else
                {

                    ddlFormComplementPedagPosGraduacao.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void ddlTipoInstituicaoPosGraduacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tseInstituicaoPosGraduacao.ResetValue();

                if (ddlTipoInstituicaoPosGraduacao.SelectedValue != "Selecione")
                {
                    tseInstituicaoPosGraduacao.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void ddlEscolaridadePosGraduacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string nomeEscolaridade = ddlEscolaridadePosGraduacao.SelectedValue.ToString().Trim();

                if (nomeEscolaridade == "Ensino Médio Normal/Magistério" ||
                   nomeEscolaridade == "Ensino Médio Normal/Magistério Específico Indígena" ||
                   nomeEscolaridade == "Ensino Médio Normal/Magistério - Estudos Adicionais")
                {
                    ddlSituacaoCursoPosGraduacao.Text = "Concluído";
                    ddlSituacaoCursoPosGraduacao.Enabled = false;

                }
                else
                {
                    ddlSituacaoCursoPosGraduacao.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSalvarFormacaoPosGraduacao_Click(object sender, EventArgs e)
        {
            try
            {
                blocoGravacaoFormacao(2); //Pós-Graduação
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void blocoGravacaoFormacao(int x)
        {
            // x == 1 Graduação
            // x == 2 Pós-Graduação

            lblMensagem.Text = "";
            lblMensValidacao.Text = "";

            if (ValidarPreenchimento(x)) //Revisado
            {
                if (VerificarCampos(x))
                {
                    TceFormacaoPessoal TFP = new TceFormacaoPessoal();
                    var validacao = new ValidacaoDados();

                    TFP.Pessoa = int.Parse(txtPessoa.Text.ToString());
                    TFP.Escolaridade = x == 1 ? ddlEscolaridade.SelectedValue : ddlEscolaridadePosGraduacao.SelectedValue;
                    TFP.SituacaoCurso = x == 1 ? ddlSituacaoCurso.SelectedValue : ddlSituacaoCursoPosGraduacao.SelectedValue;
                    TFP.IdCursoFormacaoPessoal = int.Parse(x == 1 ? ddlCurso.SelectedValue : ddlCursoPosGraduacao.SelectedValue);
                    TFP.FormacaoComplementacaoPedagogica = x == 1 ? ddlFormComplementPedag.SelectedValue : ddlFormComplementPedagPosGraduacao.SelectedValue;
                    TFP.AnoInicio = int.Parse(x == 1 ? txtAnoInicio.Text.Trim() : txtAnoInicioPosGraduacao.Text.Trim());

                    if (!string.IsNullOrEmpty(x == 1 ? txtAnoConclusao.Text.Trim() : txtAnoConclusaoPosGraduacao.Text.Trim()))
                    {
                        TFP.AnoConclusao = int.Parse(x == 1 ? txtAnoConclusao.Text.Trim() : txtAnoConclusaoPosGraduacao.Text.Trim());
                    }
                    TFP.IdInstituicao = x == 1 ? tseInstituicao.DBValue.ToString() : tseInstituicaoPosGraduacao.DBValue.ToString();
                    TFP.Matricula = User.Identity.Name.ToString();
                    TFP.Doc_comprobatorio = (x == 1 ? ckDocComprob.Checked : ckDocComprobPosGraduacao.Checked) == true ? "Sim" : "Não";


                    validacao = RN.FormacaoPessoal.Validar(TFP);

                    if (validacao.Valido == true)
                    {
                        validacao = RN.FormacaoPessoal.ValidarPreRequisito(TFP);
                    }

                    if (validacao.Valido)
                    {

                        if (RN.FormacaoPessoal.Inserir(TFP) > 0)
                        {
                            LimparCampos();

                            odsFormacaoPessoal.Select();
                            odsFormacaoPessoal.DataBind();
                            grdFormacaoPessoal.DataBind();
                        }
                        else
                        {
                            lblMensValidacao.Text = validacao.Mensagem;
                        }
                    }
                    else
                    {
                        lblMensValidacao.Text = validacao.Mensagem;
                    }
                }

                lblMensagemFormacao.Text = lblMensagem.Text = lblMensValidacao.Text;
            }
        }

        #region Eventos odsServidorCursoCapacitacao

        public static void DeleteCursoCapacitacao(decimal PESSOAID, decimal CURSOCAPACITACAOID, int TIPOCURSOCAPACITACAOID)
        {

        }

        protected void odsServidorCursoCapacitacao_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            string idCursoCapacitacao = e.InputParameters["CURSOCAPACITACAOID"].ToString();
            string idPessoa = e.InputParameters["PESSOAID"].ToString();

            //Data: 25/04/2013 - Alterado por: Lucas Collina - Solicitado por: Wagner Medeiros
            //A validação por esse tipo de curso (EspecificoEduEspecial) já existia. 
            //Com a criação do campo 'TIPOCURSOCAPACITACAOID' na tabela 'LY_CAPACITACAO', 
            //foi alterado para tratar pelo ID deste tipo de curso == '6'
            if (e.InputParameters["TIPOCURSOCAPACITACAOID"].ToString() == "6")
            {
                if (RN.GrupoHabilitacao.PossuiGrupoCapacitacaoEdEspecial(idPessoa))
                {
                    throw new ApplicationException("Esta capacitação não pode ser excluída devido ter grupo de habilitação relacionado.");
                }
            }

            var validacao = new ValidacaoDados();
            int retornoExclusaoCapacitacao = 0;

            retornoExclusaoCapacitacao = RN.Capacitacao.RemoverCursoCapacitacao(int.Parse(idCursoCapacitacao), int.Parse(idPessoa));

            if (retornoExclusaoCapacitacao > 0)
            {
                AtualizaGridServidorCursoCapacitacao();
                ClientScript.RegisterClientScriptBlock(GetType(), "sas", "<script> alert('Curso de Capacitação excluído com sucesso.');</script>");
            }
        }

        public static void AlteraCursoCapacitacao(string NOMECURSO, int AREACONHECIMENTOID, int TIPOCURSOCAPACITACAOID, string NOMEINSTITUICAO, decimal CARGAHORARIA, DateTime DATACONCLUSAO, decimal PESSOAID, decimal CURSOCAPACITACAOID)
        {

        }

        protected void odsServidorCursoCapacitacao_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var validacao = new ValidacaoDados();
            int retornoAtualizacaoCapacitacao = 0;

            var entidadeLyCapacitacao = new RN.Entidades.LyCapacitacao
            {
                AreaConhecimentoId = Convert.ToInt32(e.InputParameters["AREACONHECIMENTOID"]),
                Capacitacao = e.InputParameters["NOMECURSO"].ToString(),
                CargaHoraria = Convert.ToInt32(e.InputParameters["CARGAHORARIA"]),
                DataConclusao = Convert.ToDateTime(e.InputParameters["DATACONCLUSAO"]),
                NomeInstituicao = e.InputParameters["NOMEINSTITUICAO"].ToString(),
                Ordem = Convert.ToInt32(e.InputParameters["CURSOCAPACITACAOID"]),
                Pessoa = Convert.ToInt32(e.InputParameters["PESSOAID"]),
                TipoCursoCapacitacaoId = Convert.ToInt32(e.InputParameters["TIPOCURSOCAPACITACAOID"])
            };

            validacao = RN.Capacitacao.Validar(entidadeLyCapacitacao);

            if (validacao.Valido)
            {
                retornoAtualizacaoCapacitacao = RN.Capacitacao.AlterarCurso(entidadeLyCapacitacao);
            }
            else
            {
                throw new Exception(validacao.Mensagem.ToString());
            }

            if (retornoAtualizacaoCapacitacao > 0)
            {
                AtualizaGridServidorCursoCapacitacao();
                ClientScript.RegisterClientScriptBlock(GetType(), "sas", "<script> alert('Curso de Capacitação alterado com sucesso.');</script>");
            }
        }

        public static void InsereCursoCapacitacao(string NOMECURSO, int AREACONHECIMENTOID, int TIPOCURSOCAPACITACAOID, string NOMEINSTITUICAO, decimal CARGAHORARIA, DateTime DATACONCLUSAO, int PESSOAID)
        {

        }

        protected void odsServidorCursoCapacitacao_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var validacao = new ValidacaoDados();
            int retornoInserirCurso = 0;

            int idPessoa = Convert.ToInt32(txtPessoa.Text);

            var entidadeLyCapacitacao = new RN.Entidades.LyCapacitacao
            {
                AreaConhecimentoId = Convert.ToInt32(e.InputParameters["AREACONHECIMENTOID"]),
                Capacitacao = e.InputParameters["NOMECURSO"].ToString(),
                CargaHoraria = Convert.ToInt32(e.InputParameters["CARGAHORARIA"]),
                DataConclusao = Convert.ToDateTime(e.InputParameters["DATACONCLUSAO"]),
                NomeInstituicao = e.InputParameters["NOMEINSTITUICAO"].ToString(),
                Pessoa = Convert.ToInt32(e.InputParameters["PESSOAID"]),
                TipoCursoCapacitacaoId = Convert.ToInt32(e.InputParameters["TIPOCURSOCAPACITACAOID"])
            };

            validacao = RN.Capacitacao.Validar(entidadeLyCapacitacao);

            if (validacao.Valido)
            {
                retornoInserirCurso = RN.Capacitacao.InserirCurso(entidadeLyCapacitacao);
            }

            if (retornoInserirCurso > 0)
            {
                AtualizaGridServidorCursoCapacitacao();
                ClientScript.RegisterClientScriptBlock(GetType(), "sas", "<script> alert('Curso de Capacitação incluído com sucesso.');</script>");
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        private void AtualizaGridServidorCursoCapacitacao()
        {
            odsServidorCursoCapacitacao.Select();
            odsServidorCursoCapacitacao.DataBind();
            grdCapacitacao.DataBind();
        }

        #endregion


        protected void btnSalvarIdFuncional_Click(object sender, EventArgs e)
        {
            try
            {
                RN.Pessoa rnPessoa = new Pessoa();
                ValidacaoDados validacao = new ValidacaoDados();

                decimal pessoa = !txtPessoa.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(txtPessoa.Text) : -1;
                int? idFuncional = !txtIdFuncionalAtualizacao.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtIdFuncionalAtualizacao.Text.Trim()) : (int?)null;
                bool possuiId = !chkNaoPossuiIdFuncional.Checked;

                validacao = rnPessoa.ValidaAlteracaoIdFuncional(pessoa, idFuncional, possuiId, User.Identity.Name);

                if (validacao.Valido)
                {
                    rnPessoa.AlteraIdFuncional(pessoa, User.Identity.Name, (!possuiId ? 0 : idFuncional));

                    lblMensagem.Text = "ID Funcional alterado com sucesso.";
                    txtIdFuncional.Text = txtIdFuncionalAtualizacao.Text;
                    pnlIdFuncional.Visible = false;

                    if (!txtIdFuncionalAtualizacao.Text.IsNullOrEmptyOrWhiteSpace() || chkNaoPossuiIdFuncional.Checked)
                    {
                        pnlGridDadosIngresso.Visible = true;
                        lblMensagemIdFuncional.Text = string.Empty;
                    }

                    var script = @"alert('" + lblMensagem.Text + @"');";

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

        protected void pcPessoa_TabClick(object source, DevExpress.Web.ASPxTabControl.TabControlCancelEventArgs e)
        {
            try
            {
                pnlIdFuncional.Visible = false;
                RN.Pessoa rnPessoa = new Pessoa();
                lblMensagemIdFuncional.Text = string.Empty;

                //Verifica se esta indo para ABA Dados de ingerrros
                if (Convert.ToInt16(e.Tab.Index) == 4)
                {
                    //Verifica se esta´editando um servidor
                    if (!txtPessoa.Text.IsNullOrEmptyOrWhiteSpace())
                    {
                        pnlGridDadosIngresso.Visible = false;

                        int? idFuncional = rnPessoa.ObtemIdFuncionalPor(Convert.ToDecimal(txtPessoa.Text));

                        if (idFuncional >= 0)
                        {
                            pnlGridDadosIngresso.Visible = true;

                            if (idFuncional == 0)
                            {                                
                                chkNaoPossuiIdFuncional.Checked = true;
                                txtIdFuncionalAtualizacao.Text = string.Empty;
                            }
                            else
                            {
                                txtIdFuncionalAtualizacao.Text = Convert.ToString(idFuncional);
                            }
                        }
                        else
                        {
                            pnlIdFuncional.Visible = true;
                            lblMensagemIdFuncional.Text = "Para visualizar os Dados de Ingresso é necessário salvar o ID Funcional do Servidor.";
                            lblMensagemIdFuncional.Visible = true;
                        }
                    }
                    else
                    {
                        //Em caso de cadastro não exibe opção para Não possui id funcional
                        pnlIdFuncional.Visible = false;
                        lblMensagemIdFuncional.Visible = false;
                        pnlGridDadosIngresso.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkNaoPossuiIdFuncional_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtIdFuncionalAtualizacao.Enabled = true;
                if (chkNaoPossuiIdFuncional.Checked)
                {
                    txtIdFuncionalAtualizacao.Enabled = false;
                    txtIdFuncionalAtualizacao.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void txtIdFuncionalAtualizacao_TextChanged(object sender, EventArgs e)
        {
            if (!txtIdFuncionalAtualizacao.Text.IsNullOrEmptyOrWhiteSpace())
            {
                chkNaoPossuiIdFuncional.Checked = false;
            }
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
                chkAreaAssentamento.Checked = !chkNaoSeAplica.Checked;
                chkTerraIndigena.Checked = !chkNaoSeAplica.Checked;

                chkQuilombos.Enabled = !chkNaoSeAplica.Checked;
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
                        chkQuilombos, chkTerraIndigena, chkAreaAssentamento
                    }, true
                );
            }

            chkNaoSeAplica.Enabled = true;
        }
    }
}



