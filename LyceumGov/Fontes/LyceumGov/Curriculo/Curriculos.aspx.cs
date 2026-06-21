using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.CR;
using Techne.Data;
using DevExpress.Web.ASPxGridView;
using Techne.Lyceum.RN;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxClasses;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Curriculo
{
    [
     NavUrl("~/Curriculo/Curriculos.aspx"),
      ControlText("Curriculos"),
      Title("Matriz Curricular"),
    ]
    public partial class Curriculos : TPage
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
        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdGrade, "Componentes Curriculares");
            TituloGrid(grdSeries, "Anos de Escolaridade");
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                _tipoOperacao = TipoOperacao.Inicial;
                ControlarTipoOperacao();
                pcCurriculos.ActiveTabIndex = 0; //abrir sempre na primeira aba
            }

            lblMensagem.Text = string.Empty; //LIMPAR ERROS

            dtDt_Extinsao.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            dtDt_Homolog.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);


            ControlaVisibilidadeCurso();
            ControlaVisibilidadeCurriculo();
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ValidaMatrizCurricular();
            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnExcluir, AcaoControle.excluir);
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(grdSeries);
            ControlaAcesso(grdGrade);
        }


        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
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


        #region Definição

        private void ControlarTipoOperacao()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);

                        tseModalidade.ResetValue();
                        tseNivel.ResetValue();
                        tseCurriculo.ResetValue();
                        tseTurno.ResetValue();
                        tseCurso.ResetValue();

                        pcCurriculos.Visible = false;
                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar, btnExcluir };
                        ControlarVisibilidadeControle(controles);

                        pcCurriculos.TabPages[1].Enabled = true;
                        pcCurriculos.TabPages[2].Enabled = true;
                        pcCurriculos.Visible = true;
                        DesabilitaCampos();

                        //tdsCurriculosSeries.SqlWhere = "curso = '" + ddlCurso.DBValue.ToString() +
                        //                        "' AND curriculo = '" + txtCurriculo.Text + "'";

                        //tdsCurriculosSeries.Select();
                        //grdSeries.DataBind();

                        //tdsUnidadesFisicas.SqlWhere = "curso = '" + ddlCurso.DBValue.ToString() +
                        //        "' AND curriculo = '" + txtCurriculo.Text + "'";

                        //tdsUnidadesFisicas.Select();
                        //grdUnidadesFisicas.DataBind();

                        break;
                    }
                case TipoOperacao.Novo:
                    {

                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);

                        lblMensagem.Text = string.Empty;

                        pcCurriculos.TabPages[1].Enabled = false;
                        pcCurriculos.TabPages[2].Enabled = false;

                        tseModalidade.ResetValue();
                        tseNivel.ResetValue();
                        tseCurriculo.ResetValue();
                        tseTurno.ResetValue();
                        tseCurso.ResetValue();
                        grdGrade.DataBind();
                        grdSeries.DataBind();

                        tseModalidade.Enabled = false;
                        tseNivel.Enabled = false;
                        tseCurriculo.Enabled = false;
                        tseTurno.Enabled = false;
                        tseCurso.Enabled = false;

                        pcCurriculos.Visible = true;

                        this.ddlCurso.Enabled = true;
                        this.txtCurriculo.ReadOnly = false;

                        LimparTela();
                        HabilitaCampos();

                        CarregarDadosDrop(ddlCurso.ID);
                        CarregarDadosDrop(ddlTurno.ID);
                        CarregarDadosDrop(ddlAno_Ini.ID);
                        CarregarDadosDrop(ddlSem_Ini.ID);

                        break;
                    }
                case TipoOperacao.Excluir:
                    {
                        Ly_curriculo dtCurriculo = new Ly_curriculo();
                        Ly_curriculo.Row dadosCurrriculo = dtCurriculo.NewRow();
                        string curso = ddlCurso.DBValue.ToString();
                        string curriculo = txtCurriculo.Text;
                        string turno = tseTurno.DBValue.ToString();

                        RN.RetValue retorno = null;
                        retorno = RN.Curriculo.Excluir(curso, curriculo, turno);

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
                        break;
                    }
                case TipoOperacao.Alterar:
                    {
                        HabilitaCampos();
                        //não alterados:
                        this.ddlCurso.Enabled = false;
                        this.ddlTurno.Enabled = false;
                        this.txtCurriculo.ReadOnly = true;

                        tseModalidade.Enabled = false;
                        tseNivel.Enabled = false;
                        tseCurriculo.Enabled = false;
                        tseTurno.Enabled = false;
                        tseCurso.Enabled = false;

                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);

                        pcCurriculos.TabPages[1].Enabled = true;
                        pcCurriculos.TabPages[2].Enabled = true;

                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        lblMensagem.Text = string.Empty;

                        tseModalidade.Enabled = true;
                        tseNivel.Enabled = true;
                        tseCurriculo.Enabled = true;
                        tseTurno.Enabled = true;
                        tseCurso.Enabled = true;

                        Ly_curriculo dtCurriculo = new Ly_curriculo();
                        Ly_curriculo.Row dadosCurrriculo = dtCurriculo.NewRow();
                        string curso = tseCurso.DBValue.ToString();
                        string turno = tseTurno.DBValue.ToString();
                        string curriculo = tseCurriculo.DBValue.ToString();

                        dadosCurrriculo = RN.Curriculo.ConsultarPorCursoCurriculo(curso, turno, curriculo);

                        if (dadosCurrriculo != null)
                        {

                            ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar, btnExcluir };
                            ControlarVisibilidadeControle(controles);

                            pcCurriculos.Visible = true;
                            pcCurriculos.TabPages[1].Enabled = true;
                            pcCurriculos.TabPages[2].Enabled = true;

                            CarregarDadosDrop(ddlCurso.ID);
                            CarregarDadosDrop(ddlTurno.ID);
                            CarregarDadosDrop(ddlAno_Ini.ID);
                            CarregarDadosDrop(ddlSem_Ini.ID);

                            PreencherDadosTela(dadosCurrriculo);
                            DesabilitaCampos();
                        }
                        else
                        {
                            LimparTela();
                            ImageButton[] controles = new ImageButton[] { btnNovo };
                            ControlarVisibilidadeControle(controles);
                            pcCurriculos.Visible = false;
                            //lblMensagem.Text = "Currículo não cadastrado.";
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
        /// Armazena uma nova linha com os dados da tela no datatable passado como parâmetro
        /// </summary>
        /// <param name="dtDocente">DataTable do docente que será adicionado uma nova linha</param>
        private void ObterDados(Ly_curriculo dtCurriculo)
        {
            Techne.Lyceum.CR.Ly_curriculo.Row dadosCurriculo = dtCurriculo.NewRow();

            if (!string.IsNullOrEmpty(txtCurriculo.Text))
                dadosCurriculo.Curriculo = txtCurriculo.Text.Trim();

            if (ddlCurso.IsValidDBValue && !ddlCurso.DBValue.IsNull)
                dadosCurriculo.Curso = ddlCurso.DBValue.ToString();

            if (!string.IsNullOrEmpty(ddlAno_Ini.SelectedValue))
                dadosCurriculo.Ano_ini = Convert.ToInt16(ddlAno_Ini.SelectedValue);

            if (!string.IsNullOrEmpty(ddlSem_Ini.SelectedValue))
                dadosCurriculo.Sem_ini = Convert.ToInt16(ddlSem_Ini.SelectedValue);

            if (!string.IsNullOrEmpty(ddlTurno.SelectedValue))
                dadosCurriculo.Turno = ddlTurno.SelectedValue.ToString();

            if (!string.IsNullOrEmpty(dtDt_Extinsao.Text))
                dadosCurriculo.Dt_extincao = dtDt_Extinsao.Date;

            if (!string.IsNullOrEmpty(dtDt_Homolog.Text))
                dadosCurriculo.Dt_homolog = dtDt_Homolog.Date;

            if (chkEnsReligioso.Checked)
            {
                dadosCurriculo.Ensino_religioso = "S";
            }
            else
            {
                dadosCurriculo.Ensino_religioso = "N";
            }

            if (chkLinguaEstrangeira.Checked)
            {
                dadosCurriculo.Lingua_estrangeira = "S";
            }
            else
            {
                dadosCurriculo.Lingua_estrangeira = "N";
            }



            //FIXOS:

            dadosCurriculo.Regime = "Anual";
            dadosCurriculo.Aulas_previstas = 200;
            dadosCurriculo.Creditos = 200;
            dadosCurriculo.Prazo_ideal = 1;
            dadosCurriculo.Prazo_max = 1;
            dadosCurriculo.Credmin_matr = 1;
            dadosCurriculo.Tranc_max = 99;
            dadosCurriculo.Tranc_cons_max = 99;
            dadosCurriculo.Tranc_max_discip = 99;
            dadosCurriculo.Canc_max_discip = 99;
            dadosCurriculo.Atlz_max_discip = 99;
            dadosCurriculo.Ratear_mens = "N";
            dadosCurriculo.Credmax_matr = 9999;
            dadosCurriculo.Tranc_interv_data = "N";
            //dadosCurriculo.Turno = string.Empty;

            dtCurriculo.Rows.Add(dadosCurriculo);
        }

        /// <summary>
        /// Preenche os dados na tela de acordo com a linha passada como parâmetro
        /// </summary>
        /// <param name="dadosDocente">Linha com os dados do docente</param>
        private void PreencherDadosTela(Ly_curriculo.Row dadosCurriculo)
        {
            txtCurriculo.Text = Convert.ToString(dadosCurriculo.Curriculo);
            dtDt_Homolog.Date = Convert.ToDateTime(dadosCurriculo.Dt_homolog);
            dtDt_Extinsao.Date = Convert.ToDateTime(dadosCurriculo.Dt_extincao);
            ddlCurso.DBValue = dadosCurriculo.Curso;
            PreencherDadoCombo(ddlAno_Ini, Convert.ToString(dadosCurriculo.Ano_ini));
            PreencherDadoCombo(ddlSem_Ini, Convert.ToString(dadosCurriculo.Sem_ini));
            PreencherDadoCombo(ddlTurno, Convert.ToString(dadosCurriculo.Turno));
            chkLinguaEstrangeira.Checked = dadosCurriculo.Lingua_estrangeira == "S" ? true : false;
            chkEnsReligioso.Checked = dadosCurriculo.Ensino_religioso == "S" ? true : false;
            hdnEnsReligioso.Value = dadosCurriculo.Ensino_religioso;
            hdnLinguaEstrangeira.Value = dadosCurriculo.Lingua_estrangeira;
        }



        /// <summary>
        /// Limpa todas as textbox e combobox.
        /// </summary>
        protected void LimparTela()
        {
            ddlCurso.ResetValue();
            ddlAno_Ini.Items.Clear();
            ddlSem_Ini.Items.Clear();
            txtCurriculo.Text = string.Empty;
            dtDt_Homolog.Text = string.Empty;
            dtDt_Extinsao.Text = string.Empty;
            ddlTurno.Items.Clear();
            chkEnsReligioso.Checked = false;
            chkLinguaEstrangeira.Checked = false;
            hdnEnsReligioso.Value = string.Empty;
            hdnLinguaEstrangeira.Value = string.Empty;
        }

        /// <summary>
        /// Habilita todos os campos para edição
        /// </summary>
        protected void HabilitaCampos()
        {
            ddlCurso.Enabled = true;
            ddlAno_Ini.Enabled = true;
            ddlSem_Ini.Enabled = true;
            txtCurriculo.ReadOnly = false;
            dtDt_Homolog.Enabled = true;
            dtDt_Extinsao.Enabled = true;
            ddlTurno.Enabled = true;
            chkEnsReligioso.Enabled = true;
            chkLinguaEstrangeira.Enabled = true;
        }

        /// <summary>
        /// Desabilita todos os campos para edição.
        /// </summary>
        protected void DesabilitaCampos()
        {
            ddlCurso.Enabled = false;
            ddlAno_Ini.Enabled = false;
            ddlSem_Ini.Enabled = false;
            txtCurriculo.ReadOnly = true;
            dtDt_Homolog.Enabled = false;
            dtDt_Extinsao.Enabled = false;
            ddlTurno.Enabled = false;
            chkEnsReligioso.Enabled = false;
            chkLinguaEstrangeira.Enabled = false;
        }

        /// <summary>
        /// Carrega os dados do banco na dropdownlist
        /// </summary>
        /// <param name="drop">DropDownList que será carregado</param>
        /// <param name="data">Dados que serão preenchidos</param>
        /// <param name="defaultValue">Valor padrão</param>
        private void CarregarDropDownList(DropDownList drop, object data, string defaultValue)
        {

            drop.DataSource = data;
            drop.DataBind();
            ListItem itemVazio = new ListItem("<Nenhum>", "");
            drop.Items.Add(itemVazio);

            if (_tipoOperacao.Equals(TipoOperacao.Novo))
            {
                drop.SelectedValue = "";
            }
        }

        /// <summary>
        /// Faz a consulta ao banco para cada DropDownList e chama o método para carregá-los
        /// </summary>
        /// <param name="idDrop">ID da DropDownList</param>
        private QueryTable CarregarDadosDrop(string idDrop)
        {
            QueryTable dadosDrop = null;

            try
            {
                switch (idDrop)
                {
                    case "ddlAno_Ini":
                        {
                            dadosDrop = RN.PeriodoLetivo.ConsultarAno();

                            CarregarDropDownList(ddlAno_Ini, dadosDrop, "");
                            break;
                        }

                    case "ddlSem_Ini":
                        {
                            if (ddlAno_Ini.SelectedValue != "")
                            {
                                string ano = ddlAno_Ini.SelectedValue.ToString();
                                dadosDrop = RN.PeriodoLetivo.ConsultarPeriodo(ano);

                                CarregarDropDownList(ddlSem_Ini, dadosDrop, "");
                            }
                            break;
                        }
                    case "ddlTurno":
                        {
                            dadosDrop = RN.Turno.Consultar();
                            CarregarDropDownList(ddlTurno, dadosDrop, "");

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


        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            RN.Curriculo rnCurriculo = new Techne.Lyceum.RN.Curriculo();
            ValidacaoDados validacao = new ValidacaoDados();

            try
            {
                tseNivel.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
                tseModalidade.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
                tseCurriculo.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
                tseTurno.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
                tseCurso.RequiredFieldValidation = Techne.Controls.ValidationOption.False;

                RN.RetValue retorno = null;

                CR.Ly_curriculo dtCurriculo = new Techne.Lyceum.CR.Ly_curriculo();
                ObterDados(dtCurriculo);

                if (_tipoOperacao.Equals(TipoOperacao.Novo))
                {
                    validacao = rnCurriculo.Valida(dtCurriculo, hdnEnsReligioso.Value, hdnLinguaEstrangeira.Value, true);

                    if (validacao.Valido)
                    {
                        retorno = RN.Curriculo.Incluir(dtCurriculo);
                    }
                    else
                    {
                        lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");

                        return;
                    }

                }
                else if (_tipoOperacao.Equals(TipoOperacao.Alterar))
                {
                    validacao = rnCurriculo.Valida(dtCurriculo, hdnEnsReligioso.Value, hdnLinguaEstrangeira.Value, false);

                    if (validacao.Valido)
                    {
                        retorno = RN.Curriculo.Alterar(dtCurriculo);
                    }
                    else
                    {
                        lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");

                        return;
                    }
                }
                if (retorno != null)
                {
                    if (!retorno.Ok)
                    {
                        lblMensagem.Text = retorno.Errors.ToString().Replace("Currículo", "Matriz Curricular").Replace("cadastrado", "cadastrada");
                    }
                    else
                    {
                        lblMensagem.Text = retorno.Message;
                        _tipoOperacao = TipoOperacao.Sucesso;
                        ControlarTipoOperacao();
                        tseCurso.DBValue = dtCurriculo.Rows[0]["curso"].ToString();
                        tseTurno.DBValue = dtCurriculo.Rows[0]["turno"].ToString();
                        tseCurriculo.DBValue = dtCurriculo.Rows[0]["curriculo"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            tseNivel.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
            tseModalidade.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
            tseCurriculo.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
            tseTurno.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
            tseCurso.RequiredFieldValidation = Techne.Controls.ValidationOption.False;

            _tipoOperacao = TipoOperacao.Novo;
            ControlarTipoOperacao();
        }

        protected void btnExcluir_Click(object sender, ImageClickEventArgs e)
        {
            tseNivel.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
            tseModalidade.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
            tseCurriculo.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
            tseTurno.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
            tseCurso.RequiredFieldValidation = Techne.Controls.ValidationOption.False;

            _tipoOperacao = TipoOperacao.Excluir;
            ControlarTipoOperacao();
        }

        protected void btnEditar_Click(object sender, ImageClickEventArgs e)
        {
            tseNivel.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
            tseModalidade.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
            tseCurriculo.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
            tseTurno.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
            tseCurso.RequiredFieldValidation = Techne.Controls.ValidationOption.False;

            _tipoOperacao = TipoOperacao.Alterar;
            ControlarTipoOperacao();
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            tseNivel.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
            tseModalidade.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
            tseCurriculo.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
            tseTurno.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
            tseCurso.RequiredFieldValidation = Techne.Controls.ValidationOption.False;

            _tipoOperacao = TipoOperacao.Consultar;
            ControlarTipoOperacao();
        }

        #endregion

        #region Grid UnidadesFisicas

        protected void grdUnidadesFisicas_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "UnidadesFisicasKey")
            {
                string curso = Convert.ToString(e.GetListSourceFieldValue("curso"));
                string turno = Convert.ToString(e.GetListSourceFieldValue("turno"));
                string curriculo = Convert.ToString(e.GetListSourceFieldValue("curriculo"));
                string faculdade = Convert.ToString(e.GetListSourceFieldValue("faculdade"));
                string chave = Convert.ToString(e.GetListSourceFieldValue("chave"));
                e.Value = curso + "-" + turno + "-" + curriculo + "-" + faculdade + "-" + chave;
            }
        }

        protected void grdUnidadesFisicas_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Keys.Clear();
            e.Keys.Add("curso", e.Values["curso"]);
            e.Keys.Add("curriculo", e.Values["curriculo"]);
            e.Keys.Add("faculdade", e.Values["faculdade"]);
            e.Keys.Add("chave", e.Values["chave"]);
        }

        protected void grdUnidadesFisicas_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            //grdUnidadesFisicas.Settings.ShowFilterRow = false;
        }


        protected void grdSeries_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdSeries.Settings.ShowFilterRow = false;
        }

        protected void grdUnidadesFisicas_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["curso"] = tseCurso.DBValue.ToString();
            e.NewValues["curriculo"] = tseCurriculo.DBValue.ToString();
            //grdUnidadesFisicas.Settings.ShowFilterRow = false;
        }

        protected void grdUnidadesFisicas_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["chave"] = 1;

            if (!String.IsNullOrEmpty(ddlCurso.DBValue.ToString()))
                e.NewValues["curso"] = ddlCurso.DBValue.ToString();

            if (!String.IsNullOrEmpty(txtCurriculo.Text))
                e.NewValues["curriculo"] = txtCurriculo.Text;
        }


        protected void grdUnidadesFisicas_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            if ((ddlCurso.IsValidDBValue && !ddlCurso.DBValue.IsNull)
                && (!String.IsNullOrEmpty(txtCurriculo.Text.ToString())))
            {
                tdsUnidadesFisicas.SqlWhere = "curso = '" + RN.RNBase.MudarAspas(ddlCurso.DBValue.ToString()) +
                                                "' AND curriculo = '" + RN.RNBase.MudarAspas(txtCurriculo.Text) + "'";

                tdsUnidadesFisicas.Select();
                //grdUnidadesFisicas.DataBind();
            }
        }

        #endregion

        #region Grid Ano Escolar
        protected void grdSeries_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (e.NewValues["idade_minima"] != null)
            {
                int idade_minima = Convert.ToInt32(e.NewValues["idade_minima"]);
                if (idade_minima <= 0)
                    e.RowError = "Idade mínima deve ser maior que zero.";
                if (idade_minima > 150)
                    e.RowError = "Idade mínima deve ser menor que 150.";
            }

            if (e.NewValues["mes_aniv"] != null)
            {
                decimal mes = Convert.ToDecimal(e.NewValues["mes_aniv"]);
                if (mes > 12 || mes < 1)
                    e.RowError = "Mês de aniversário deve ser entre 1 e 12.";

                if (e.NewValues["dia_aniv"] != null)
                {
                    decimal dia = Convert.ToDecimal(e.NewValues["dia_aniv"]);
                    if (mes == 2)
                    {
                        if (dia > 29)
                            e.RowError = "Mês selecionado tem no máximo 29 dias.";
                    }
                    if (mes == 4 || mes == 6 || mes == 9 || mes == 11)
                    {
                        if (dia > 30)
                            e.RowError = "Mês selecionado tem no máximo 30 dias.";
                    }
                }
            }

            if (e.NewValues["dia_aniv"] != null)
            {
                decimal dia = Convert.ToDecimal(e.NewValues["dia_aniv"]);
                if (dia > 31 || dia < 1)
                    e.RowError = "Dia de aniversário deve ser entre 1 e 31.";
            }

            if (e.OldValues["complemento2"] != null && e.OldValues["complemento2"].ToString() != string.Empty)
            {
                int novo = 0;
                if (e.NewValues["complemento2"] != null && e.NewValues["complemento2"].ToString() != string.Empty)
                {
                    novo = Convert.ToInt32(e.NewValues["complemento2"]);
                }

                if (novo < Convert.ToInt32(e.OldValues["complemento2"]))
                {
                    if (RN.HorarioOperacional.ExisteHorarioOper(tseCurso.DBValue.ToString(), ddlTurno.SelectedValue, tseCurriculo.DBValue.ToString(), e.NewValues["serie"].ToString()))
                    {
                        e.RowError = "Não é possível alterar os tempos de aula, porque existe horário operacional cadastrado.";
                        return;
                    }
                }
            }
        }



        protected void grdSeries_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            string serie = e.NewValues["serie"].ToString();
            e.NewValues["serie"] = serie.Trim();

            string descricao = e.NewValues["descricao"].ToString();
            e.NewValues["descricao"] = descricao.Trim();


            if (ddlCurso.IsValidDBValue && !ddlCurso.DBValue.IsNull)
                e.NewValues["curso"] = ddlCurso.DBValue.ToString();

            if (!String.IsNullOrEmpty(ddlTurno.SelectedValue))
                e.NewValues["turno"] = ddlTurno.SelectedValue;

            if (!String.IsNullOrEmpty(txtCurriculo.Text))
                e.NewValues["curriculo"] = txtCurriculo.Text;
        }

        protected void grdSeries_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["curso"] = tseCurso.DBValue.ToString();
            e.NewValues["turno"] = ddlTurno.SelectedValue;
            e.NewValues["curriculo"] = tseCurriculo.DBValue.ToString();
            grdSeries.Settings.ShowFilterRow = false;
        }

        protected void grdSeries_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string curso = Convert.ToString(e.GetListSourceFieldValue("curso"));
                string turno = Convert.ToString(e.GetListSourceFieldValue("turno"));
                string curriculo = Convert.ToString(e.GetListSourceFieldValue("curriculo"));
                string serie = Convert.ToString(e.GetListSourceFieldValue("serie"));
                e.Value = curso + "|" + turno + "|" + curriculo + "|" + serie;
            }
        }

        protected void grdSeries_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string[] series_Chaves = e.Keys["CompositeKey"].ToString().Split('|');
            e.Keys.Clear();
            e.Keys.Add("curso", series_Chaves[0]);
            e.Keys.Add("turno", series_Chaves[1]);
            e.Keys.Add("curriculo", series_Chaves[2]);


            string serie = e.NewValues["serie"].ToString();
            series_Chaves[3] = serie.Trim();
            e.Keys.Add("serie", series_Chaves[3]);

            string descricao = e.NewValues["descricao"].ToString();
            e.NewValues["descricao"] = descricao.Trim();
        }

        protected void grdSeries_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            string[] series_Chaves = e.Keys["CompositeKey"].ToString().Split('|');
            e.Keys.Clear();
            e.Keys.Add("curso", series_Chaves[0]);
            e.Keys.Add("turno", series_Chaves[1]);
            e.Keys.Add("curriculo", series_Chaves[2]);
            e.Keys.Add("serie", series_Chaves[3]);
        }

        protected void grdSeries_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {

            if (grdSeries.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "curso")
                    e.Editor.Enabled = true;
                if ((e.Column.FieldName) == "curriculo")
                    e.Editor.Enabled = true;
            }
            else if (grdSeries.IsEditing)
            {
                if ((e.Column.FieldName) == "curso")
                    e.Editor.Enabled = false;
                if ((e.Column.FieldName) == "curriculo")
                    e.Editor.Enabled = false;
                if ((e.Column.FieldName == "serie"))
                    e.Editor.ReadOnly = true;
            }

            if (e.Column.FieldName == "serie_seguinte")
            {
                var combo = e.Editor as ASPxComboBox;

                if (combo == null)
                {
                    return;
                }

                combo.Callback += this.cmbCurso_OnCallback;
            }

            if (this.grdSeries.IsEditing
                && e.Column.FieldName == "serie_seguinte"
                && e.KeyValue != DBNull.Value
                && e.KeyValue != null)
            {
                var val = this.grdSeries.GetRowValuesByKeyValue(e.KeyValue, "curso");

                if (val == DBNull.Value)
                {
                    return;
                }

                var curso = (string)val;
                var combo = e.Editor as ASPxComboBox;

                if (combo == null)
                {
                    return;
                }

                this.CarregarSeries(combo, curso);
            }
        }

        protected void grdSeries_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdSeries);
        }

        private void CarregarSeries(ASPxComboBox cmbSerie, string curso)
        {
            if (string.IsNullOrEmpty(curso))
            {
                return;
            }

            cmbSerie.Items.Clear();
            cmbSerie.TextField = "SERIE";
            cmbSerie.ValueField = "SERIE";
            cmbSerie.DataSource = Serie.ListarSeries(curso);
            cmbSerie.DataBind();
        }

        private void cmbCurso_OnCallback(object source, CallbackEventArgsBase e)
        {
            this.CarregarSeries(source as ASPxComboBox, e.Parameter);
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
        #endregion


        protected void tseCurriculo_Changed(object sender, EventArgs e)
        {
            if (tseCurriculo.IsValidDBValue && !tseCurriculo.DBValue.IsNull)
                lblMensagem.Text = string.Empty;

            if ((tseCurriculo.IsValidDBValue && !tseCurriculo.DBValue.IsNull) && (tseCurso.IsValidDBValue && !tseCurso.DBValue.IsNull) && (tseNivel.IsValidDBValue && !tseNivel.DBValue.IsNull) && (tseModalidade.IsValidDBValue && !tseModalidade.DBValue.IsNull))
            {
                pcCurriculos.TabPages[1].Enabled = true;
                pcCurriculos.TabPages[2].Enabled = true;
                lblMensagem.Text = string.Empty;
                LimparTela();
                _tipoOperacao = TipoOperacao.Consultar;
                ControlarTipoOperacao();
            }
            else
            {
                string curso = tseCurso.DBValue.ToString();
                string curriculo = tseCurriculo.DBValue.ToString();
                string nivel = tseNivel.DBValue.ToString();
                string modalidade = tseModalidade.DBValue.ToString();

                if (!String.IsNullOrEmpty(curso) && !String.IsNullOrEmpty(curriculo) && !String.IsNullOrEmpty(modalidade) && !String.IsNullOrEmpty(nivel))
                {
                    lblMensagem.Text = "Matriz curricular não cadastrada.";
                    tseCurso.ResetValue();
                    tseCurriculo.ResetValue();

                    _tipoOperacao = TipoOperacao.Inicial;
                    ControlarTipoOperacao();
                }
                else if (!String.IsNullOrEmpty(curso) && !String.IsNullOrEmpty(curriculo) && (String.IsNullOrEmpty(modalidade) || String.IsNullOrEmpty(nivel)))
                {
                    lblMensagem.Text = "Campos obrigatórios não foram selecionados. Favor selecionar primeiramente Nível e Modalidade.";
                }

                pcCurriculos.TabPages[1].Enabled = false;
                pcCurriculos.TabPages[2].Enabled = false;
            }
        }

        protected void tseNivel_Changed(object sender, EventArgs e)
        {
            tseCurriculo.ResetValue();
            pcCurriculos.Visible = false;
            lblMensagem.Text = string.Empty;
        }

        private void ControlaVisibilidadeCurso()
        {
            if (!tseNivel.DBValue.IsNull && tseNivel.IsValidDBValue && !tseModalidade.DBValue.IsNull && tseModalidade.IsValidDBValue)
                tseCurso.Enabled = true;
            else
                tseCurso.Enabled = false;
        }

        protected void tseModalidade_Changed(object sender, EventArgs e)
        {
            tseCurriculo.ResetValue();
            pcCurriculos.Visible = false;
            lblMensagem.Text = string.Empty;
        }

        protected void tseCurso_Changed(object sender, EventArgs e)
        {
            tseCurriculo.ResetValue();
            pcCurriculos.Visible = false;
            lblMensagem.Text = string.Empty;
        }

        protected void tseTurno_Changed(object sender, EventArgs e)
        {
            tseCurriculo.ResetValue();
            pcCurriculos.Visible = false;
            lblMensagem.Text = string.Empty;
        }

        private void ControlaVisibilidadeCurriculo()
        {
            if (!tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue && !tseTurno.DBValue.IsNull && tseTurno.IsValidDBValue)
                tseCurriculo.Enabled = true;
            else
                tseCurriculo.Enabled = false;
        }

        protected void ddlAno_Ini_SelectedIndexChanged(object sender, EventArgs e)
        {
            tseNivel.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
            tseModalidade.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
            tseCurriculo.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
            tseTurno.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
            tseCurso.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
            CarregarDadosDrop(ddlSem_Ini.ID);
        }

        protected void ddlCurso_Changed(object sender, EventArgs e)
        {
            tseNivel.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
            tseModalidade.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
            tseCurriculo.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
            tseTurno.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
            tseCurso.RequiredFieldValidation = Techne.Controls.ValidationOption.False;
        }

        #region grdGrade / odsGrade

        protected void grdGrade_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (e.NewValues["disciplina"] == null)
            {
                e.RowError = "Selecione um Componente Curricular.";
                grdGrade.Settings.ShowFilterRow = false;
            }
            if (e.NewValues["serie_ideal"] == null)
            {
                e.RowError = "Selecione o Ano de Escolaridade.";
                grdGrade.Settings.ShowFilterRow = false;
            }
        }

        protected void grdGrade_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string curso = Convert.ToString(e.GetListSourceFieldValue("curso"));
                string turno = e.GetListSourceFieldValue("turno").ToString();
                string curriculo = Convert.ToString(e.GetListSourceFieldValue("curriculo"));
                string disciplina = Convert.ToString(e.GetListSourceFieldValue("disciplina"));

                e.Value = curso + "|" + turno + "|" + curriculo + "|" + disciplina;
            }
        }

        protected void grdGrade_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            grdGrade.Settings.ShowFilterRow = false;

            String disciplina = e.Values["disciplina"].ToString();
            decimal serie = Convert.ToDecimal(e.Values["serie_ideal"]);
            String curso = Convert.ToString(e.Values["curso"]);
            String turno = Convert.ToString(e.Values["turno"]);
            String curriculo = Convert.ToString(e.Values["curriculo"]);

            RetValue ret = RN.Turma.VerificaPodeAlterarGrade(curso, turno, curriculo, serie);
            if (ret != null && !ret.Ok)
            {
                e.Cancel = true;
                throw new ApplicationException(ret.Errors.ToString());
            }

            string[] chaves = e.Keys[0].ToString().Split('|');

            e.Keys.Clear();

            e.Keys.Add("curso", chaves[0]);
            e.Keys.Add("turno", chaves[1]);
            e.Keys.Add("curriculo", chaves[2]);
            e.Keys.Add("disciplina", chaves[3]);
        }

        protected void grdGrade_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["curso"] = tseCurso.DBValue.ToString();
            e.NewValues["turno"] = tseTurno.DBValue.ToString();
            e.NewValues["curriculo"] = tseCurriculo.DBValue.ToString();


            if (e.NewValues["permite_glp"] == null)
            {
                e.NewValues["permite_glp"] = "N";
            }
        }

        protected void grdGrade_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string[] chaves = e.Keys[0].ToString().Split('|');

            e.Keys.Clear();

            e.Keys.Add("curso", chaves[0]);
            e.Keys.Add("turno", chaves[1]);
            e.Keys.Add("curriculo", chaves[2]);
            e.Keys.Add("disciplina", e.NewValues["disciplina"]);
        }

        protected void grdGrade_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdGrade.Settings.ShowFilterRow = false;
            e.NewValues["obrigatoria"] = "S";
            e.NewValues["permite_glp"] = "S";
        }

        protected void grdGrade_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            //grdGrade.Settings.ShowFilterRow = false;

            //String disciplina = Convert.ToString(grdGrade.GetRowValuesByKeyValue(e.EditingKeyValue, "disciplina"));
            //decimal serie = Convert.ToDecimal(grdGrade.GetRowValuesByKeyValue(e.EditingKeyValue, "serie_ideal"));
            //String curso = Convert.ToString(grdGrade.GetRowValuesByKeyValue(e.EditingKeyValue, "curso"));
            //String turno = Convert.ToString(grdGrade.GetRowValuesByKeyValue(e.EditingKeyValue, "turno"));
            //String curriculo = Convert.ToString(grdGrade.GetRowValuesByKeyValue(e.EditingKeyValue, "curriculo"));

            //RetValue ret = RN.Turma.VerificaPodeAlterarGrade(curso, turno, curriculo, serie);
            //if (ret != null && !ret.Ok)
            //{
            //    e.Cancel = true;
            //    throw new ApplicationException(ret.Errors.ToString());
            //}
        }
        protected void grdGrade_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            var cboMacro = (ASPxComboBox)this.grdGrade.FindEditFormTemplateControl("cboMacro");

            if (grdGrade.IsNewRowEditing)
            {
                if (tseCurso.DBValue != "9999.92")
                {
                    cboMacro.Enabled = false;
                }

            }
            if (grdGrade.IsEditing)
            {
                if (tseCurso.DBValue != "9999.92")
                {
                    cboMacro.Enabled = false;
                }
            }
        }

        protected void grdGrade_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            ValidaMatrizCurricular();
        }

        protected void grdGrade_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdGrade);
        }

        protected void odsGrade_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            RN.RetValue retorno = null;
            CR.Ly_grade dtGrade = new Techne.Lyceum.CR.Ly_grade();
            Techne.Lyceum.CR.Ly_grade.Row dadosGrade = dtGrade.NewRow();
            RN.Curriculo rnCurriculo = new Techne.Lyceum.RN.Curriculo();
            ValidacaoDados validacao = new ValidacaoDados();

            int ano = Convert.ToInt32(ddlAno_Ini.SelectedValue);
            int semestre = Convert.ToInt32(ddlSem_Ini.SelectedValue);

            dadosGrade.Curso = e.InputParameters["curso"].ToString();
            dadosGrade.Turno = e.InputParameters["turno"].ToString();
            dadosGrade.Curriculo = e.InputParameters["curriculo"].ToString();
            dadosGrade.Disciplina = e.InputParameters["disciplina"].ToString();
            dadosGrade.Serie_ideal = Convert.ToDecimal(e.InputParameters["serie_ideal"]);

            if (e.InputParameters["obrigatoria"] != null)
            {
                dadosGrade.Obrigatoria = e.InputParameters["obrigatoria"].ToString();
            }
            else
            {
                dadosGrade.Obrigatoria = "N";
            }

            if (e.InputParameters["macro_nome"] != null)
            {
                int result;
                if (!Int32.TryParse(e.InputParameters["macro_nome"].ToString(), out result))
                {
                    dadosGrade.Macro = Convert.ToDecimal(RN.MacroCampos.GetID_Nome(e.InputParameters["macro_nome"].ToString()));
                }
                else
                {
                    dadosGrade.Macro = Convert.ToDecimal(e.InputParameters["macro_nome"]);
                }
            }

            if (e.InputParameters["permite_glp"] != null)
                dadosGrade.Permite_glp = e.InputParameters["permite_glp"].ToString();
            else
                dadosGrade.Permite_glp = "N";

            dtGrade.Rows.Add(dadosGrade);
            validacao = rnCurriculo.ValidaGrade(dtGrade.Rows[0], ano, semestre, false);

            if (validacao.Valido)
            {
                retorno = RN.Curriculo.AlterarGrade(dtGrade);

                if (retorno != null)
                {
                    if (!retorno.Ok)
                        throw new Exception(retorno.Errors.ToString());
                }
            }
            else
            {
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

        protected void odsGrade_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            RN.RetValue retorno = null;
            CR.Ly_grade dtGrade = new Techne.Lyceum.CR.Ly_grade();
            Techne.Lyceum.CR.Ly_grade.Row dadosGrade = dtGrade.NewRow();
            RN.Curriculo rnCurriculo = new Techne.Lyceum.RN.Curriculo();
            ValidacaoDados validacao = new ValidacaoDados();

            int ano = Convert.ToInt32(ddlAno_Ini.SelectedValue);
            int semestre = Convert.ToInt32(ddlSem_Ini.SelectedValue);

            dadosGrade.Curso = e.InputParameters["curso"].ToString();
            dadosGrade.Turno = e.InputParameters["turno"].ToString();
            dadosGrade.Curriculo = e.InputParameters["curriculo"].ToString();
            dadosGrade.Disciplina = e.InputParameters["disciplina"].ToString();

            if (e.InputParameters["serie_ideal"] != null)
                dadosGrade.Serie_ideal = Convert.ToDecimal(e.InputParameters["serie_ideal"]);

            if (e.InputParameters["macro_nome"] != null)
                dadosGrade.Macro = Convert.ToDecimal(e.InputParameters["macro_nome"]);

            if (e.InputParameters["obrigatoria"] != null)
                dadosGrade.Obrigatoria = e.InputParameters["obrigatoria"].ToString();
            else
                dadosGrade.Obrigatoria = "N";

            if (e.InputParameters["permite_glp"] != null)
                dadosGrade.Permite_glp = e.InputParameters["permite_glp"].ToString();
            else
                dadosGrade.Permite_glp = "N";

            dtGrade.Rows.Add(dadosGrade);
            validacao = rnCurriculo.ValidaGrade(dtGrade.Rows[0], ano, semestre, true);

            if (validacao.Valido)
            {
                retorno = RN.Curriculo.IncluirGrade(dtGrade);

                if (retorno != null)
                {
                    if (!retorno.Ok)
                    {
                        throw new Exception(retorno.Errors.ToString());
                    }
                }
            }
            else
            {
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

        protected void odsGrade_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            RN.RetValue retorno = null;
            RN.Curriculo rnCurriculo = new Techne.Lyceum.RN.Curriculo();
            ValidacaoDados validacao = new ValidacaoDados();
            CR.Ly_grade dtGrade = new Techne.Lyceum.CR.Ly_grade();

            Techne.Lyceum.CR.Ly_grade.Row dadosGrade = dtGrade.NewRow();

            dadosGrade.Curso = e.InputParameters["curso"].ToString();
            dadosGrade.Turno = e.InputParameters["turno"].ToString();
            dadosGrade.Curriculo = e.InputParameters["curriculo"].ToString();
            dadosGrade.Disciplina = e.InputParameters["disciplina"].ToString();
            dadosGrade.Serie_ideal = Convert.ToDecimal(e.InputParameters["serie_ideal"]);

            if (e.InputParameters["macro_nome"] != null)
                dadosGrade.Macro = Convert.ToDecimal(e.InputParameters["macro_nome"]);

            if (e.InputParameters["obrigatoria"] != null)
                dadosGrade.Obrigatoria = e.InputParameters["obrigatoria"].ToString();
            else
                dadosGrade.Obrigatoria = "N";

            if (e.InputParameters["permite_glp"] != null)
                dadosGrade.Permite_glp = e.InputParameters["permite_glp"].ToString();
            else
                dadosGrade.Permite_glp = "N";

            dtGrade.Rows.Add(dadosGrade);


            validacao = rnCurriculo.ValidaRemocao(dtGrade.Rows[0]);

            if (validacao.Valido)
            {

                retorno = RN.Curriculo.ExcluirGrade(dtGrade);

                if (retorno != null)
                {
                    if (!retorno.Ok)
                        throw new Exception("Não foi possível excluir a disciplina da matriz.\n" + retorno.Errors.ToString());
                }
            }
            else
            {
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

        #endregion

        private void ValidaMatrizCurricular()
        {
            bool validou = true;
            validou = tseCurso.IsValidDBValue && !tseCurso.DBValue.IsNull && tseCurriculo.IsValidDBValue && !tseCurriculo.DBValue.IsNull && tseTurno.IsValidDBValue && !tseTurno.DBValue.IsNull;
            if (validou)
            {
                string curso = tseCurso.DBValue.ToString();
                string turno = ddlTurno.SelectedValue;
                string curriculo = tseCurriculo.DBValue.ToString();

                validou = RN.Serie.ExisteSerie(curso, turno, curriculo);
            }

            if (!validou)
            {
                grdGrade.Columns[""].Visible = false;
            }
            else
            {
                grdGrade.Columns[""].Visible = true;
            }
        }

        public void Delete(string curso, string turno, string curriculo, string disciplina) { }

        public void Insert(string disciplina, decimal serie_ideal, object macro_nome, string obrigatoria, string permite_glp, string curso, string turno, string curriculo) { }

        public void Update(string disciplina, decimal serie_ideal, object macro_nome, string obrigatoria, string permite_glp, string curso, string turno, string curriculo) { }





        public object Listar(DbObject tseCurso, DbObject turno, DbObject tseCurriculo)
        {
            QueryTable qt = null;

            if (!tseCurso.IsNull && (!turno.IsNull) && (!tseCurriculo.IsNull))
            {
                qt = RN.Curriculo.ConsultarGrades(tseCurso.ToString(), turno.ToString(), tseCurriculo.ToString());
            }
            return qt;
        }

        public object ListarEscolaridades(DbObject tseCurso, DbObject tseTurno, DbObject tseCurriculo)
        {
            QueryTable qt = null;

            if (!tseCurso.IsNull && !tseTurno.IsNull && !tseCurriculo.IsNull)
            {
                qt = RN.Curriculo.ListarEscolaridades(tseCurso.ToString(), tseTurno.ToString(), tseCurriculo.ToString());
            }
            return qt;
        }

        public object ListarMacros()
        {
            return RN.MacroCampos.Listar();
        }

        protected void odsCurriculosSeries_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var se = new LySerie
                         {
                             Curso = e.InputParameters["curso"].ToString(),
                             Curriculo = e.InputParameters["curriculo"].ToString(),
                             Turno = e.InputParameters["turno"].ToString(),
                             Serie = Convert.ToDecimal(e.InputParameters["serie"]),
                             Descricao = e.InputParameters["descricao"].ToString(),
                             Complemento1 = Convert.ToString(e.InputParameters["complemento1"]),
                             Complemento2 = Convert.ToString(e.InputParameters["complemento2"]),
                             IdadeMinima = (e.InputParameters["idade_minima"] != null) ? Convert.ToDecimal(e.InputParameters["idade_minima"]) : (decimal?)null,
                             DiaAniv = (e.InputParameters["dia_aniv"] != null) ? Convert.ToInt32(e.InputParameters["dia_aniv"]) : (int?)null,
                             MesAniv = (e.InputParameters["mes_aniv"] != null) ? Convert.ToInt32(e.InputParameters["mes_aniv"]) : (int?)null,
                             DtExtincao = (e.InputParameters["dt_extincao"] != null) ? Convert.ToDateTime(e.InputParameters["dt_extincao"]) : (DateTime?)null,
                             CursoSeguinte = (e.InputParameters["curso_seguinte"] != null) ? e.InputParameters["curso_seguinte"].ToString() : null,
                             SerieSeguinte = (e.InputParameters["serie_seguinte"] != null) ? Convert.ToDecimal(e.InputParameters["serie_seguinte"]) : (decimal?)null,
                             AnoSerieConcluinte = (e.InputParameters["ano_serie_concluinte"] != null) ? e.InputParameters["ano_serie_concluinte"].ToString() : "N",
                             EmiteCertificacao = (e.InputParameters["emite_certificacao"] != null) ? e.InputParameters["emite_certificacao"].ToString() : "N",
                             OfertaEletiva = (e.InputParameters["ofertaeletiva"] != null) ? e.InputParameters["ofertaeletiva"].ToString() : "N",
                             Matricula = User.Identity.Name
                         };

            var validacao = Serie.ValidarInserir(se);

            if (validacao.Valido)
            {
                Serie.Inserir(se);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void odsCurriculosSeries_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var se = new LySerie
            {
                Curso = e.InputParameters["curso"].ToString(),
                Curriculo = e.InputParameters["curriculo"].ToString(),
                Turno = e.InputParameters["turno"].ToString(),
                Serie = Convert.ToDecimal(e.InputParameters["serie"]),
                Matricula = User.Identity.Name
            };

            var validacao = Serie.ValidarRemover(se);

            if (validacao.Valido)
            {
                Serie.Remover(se);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void odsCurriculosSeries_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var se = new LySerie
            {
                Curso = e.InputParameters["curso"].ToString(),
                Curriculo = e.InputParameters["curriculo"].ToString(),
                Turno = e.InputParameters["turno"].ToString(),
                Serie = Convert.ToDecimal(e.InputParameters["serie"]),
                Descricao = e.InputParameters["descricao"].ToString(),
                Complemento1 = Convert.ToString(e.InputParameters["complemento1"]),
                Complemento2 = Convert.ToString(e.InputParameters["complemento2"]),
                IdadeMinima = (e.InputParameters["idade_minima"] != null) ? Convert.ToDecimal(e.InputParameters["idade_minima"]) : (decimal?)null,
                DiaAniv = (e.InputParameters["dia_aniv"] != null) ? Convert.ToInt32(e.InputParameters["dia_aniv"]) : (int?)null,
                MesAniv = (e.InputParameters["mes_aniv"] != null) ? Convert.ToInt32(e.InputParameters["mes_aniv"]) : (int?)null,
                DtExtincao = (e.InputParameters["dt_extincao"] != null) ? Convert.ToDateTime(e.InputParameters["dt_extincao"]) : (DateTime?)null,
                CursoSeguinte = (e.InputParameters["curso_seguinte"] != null) ? e.InputParameters["curso_seguinte"].ToString() : null,
                SerieSeguinte = (e.InputParameters["serie_seguinte"] != null) ? Convert.ToDecimal(e.InputParameters["serie_seguinte"]) : (decimal?)null,
                AnoSerieConcluinte = (e.InputParameters["ano_serie_concluinte"] != null) ? e.InputParameters["ano_serie_concluinte"].ToString() : "N",
                EmiteCertificacao = (e.InputParameters["emite_certificacao"] != null) ? e.InputParameters["emite_certificacao"].ToString() : "N",
                OfertaEletiva = (e.InputParameters["ofertaeletiva"] != null) ? e.InputParameters["ofertaeletiva"].ToString() : "N",
                Matricula = User.Identity.Name
            };

            var validacao = Serie.ValidarAlterar(se);

            if (validacao.Valido)
            {
                Serie.Alterar(se);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }


    }
}