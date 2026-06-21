namespace Techne.Lyceum.Net.Academico
{
    using System;
    using System.Reflection;
    using System.Web.UI.WebControls;
    using DevExpress.Web.ASPxGridView;
    using DevExpress.Web.ASPxTabControl;
    using DevExpress.Web.Data;
    using Techne.Controls;
    using Techne.Lyceum.RN;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Lyceum.RN.Util;
    using Techne.Web;

    [NavUrl("~/Academico/CompetenciasHabilidadesEscolaridade.aspx"), ControlText("CompetenciasHabilidadesEscolaridade"), Title("Competencias/Habilidades")]
    public partial class CompetenciasHabilidadesEscolaridade : TPage
    {
        public static string GetUrl()
        {
            return Navigation.GetNavigation(MethodBase.GetCurrentMethod()).GetUrl(new object[]
                                                                                  {
                                                                                  });
        }

        public void Delete(object ID_COMPETENCIA_HABILIDADE_GRUPO)
        {
        }

        public object Listar(object DISCIPLINA, object ano, object SUBPERIODO, object serie, object curso, object tipo)
        {
            if (DISCIPLINA.ToString() != "Selecione" && ano.ToString() != "Selecione" && SUBPERIODO.ToString() != "Selecione" && serie.ToString() != "Selecione" && tipo.ToString() != "Selecione")
            {
                return CompetenciaHabilidadeGrupo.Listar(DISCIPLINA.ToString(), int.Parse(ano.ToString().Substring(0, 4)), int.Parse(SUBPERIODO.ToString()), int.Parse(serie.ToString()), int.Parse(ano.ToString().Substring(7, 1)), curso.ToString(), tipo.ToString());
            }

            return null;
        }

        public object ListarCompetencia(object ID_COMPETENCIA_HABILIDADE_GRUPO)
        {
            if (ID_COMPETENCIA_HABILIDADE_GRUPO != null)
            {
                if (ID_COMPETENCIA_HABILIDADE_GRUPO.ToString() != "Selecione")
                {
                    return CompetenciaHabilidadeItem.Listar(int.Parse(ID_COMPETENCIA_HABILIDADE_GRUPO.ToString()));
                }
            }

            return null;
        }

        public void Update(object CURSO, object MODALIDADE, object TIPO_CURSO, object ANO, object PERIODO, object SUBPERIODO, object SERIE, object DISCIPLINA, object NOME_DISCIPLINA, object GRUPO, object ORDEM, object DT_CADASTRO, object ID_COMPETENCIA_HABILIDADE_GRUPO)
        {
        }

        public void Update(object GRUPO, object ORDEM, object ID_COMPETENCIA_HABILIDADE_GRUPO)
        {
        }

        public void odsCompetencia_Delete(object ID_COMPETENCIA_HABILIDADE_ITEM)
        {
        }

        public void odsCompetencia_Update(object ID_COMPETENCIA_HABILIDADE_ITEM, object TIPO_CURRICULO, object ID_COMPETENCIA_HABILIDADE_GRUPO, object GRUPO, object ORDEM, object COMPETENCIA_HABILIDADE, object DT_CADASTRO)
        {
        }

        public void odsCompetencia_Update(object ID_COMPETENCIA_HABILIDADE_GRUPO, object GRUPO, object ORDEM, object COMPETENCIA_HABILIDADE, object DT_CADASTRO, object ID_COMPETENCIA_HABILIDADE_ITEM)
        {
        }

        public void odsCompetencia_Update(object GRUPO, object ORDEM, object COMPETENCIA_HABILIDADE, object ID_COMPETENCIA_HABILIDADE_ITEM)
        {
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdGrupo, "Grupo Competências/Habilidades");
            TituloGrid(this.grdCompetenciaHab, "Competências/Habilidades");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.lblMensagem.Text = string.Empty;

            this.ValidarCampos();
        }

        protected void btnSalvarComp_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ValidarCompetencia())
                {
                    var TCHI = new TceCompetenciaHabilidadeItem();
                    var validacao = new ValidacaoDados();

                    TCHI.IdCompetenciaHabilidadeGrupo = int.Parse(this.ddlGrupo.SelectedValue);
                    TCHI.CompetenciaHabilidade = this.txtNomeCompetencia.Text.Trim();
                    TCHI.Ordem = int.Parse(this.txtOrdemComp.Text.Trim());
                    TCHI.Matricula = this.User.Identity.Name;

                    validacao = CompetenciaHabilidadeItem.Validar(TCHI);

                    if (validacao.Valido)
                    {
                        if (CompetenciaHabilidadeItem.Inserir(TCHI) > 0)
                        {
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Competência/Habilidade incluída com sucesso.');", true);
                            this.txtNomeCompetencia.Text = string.Empty;
                            this.txtOrdemComp.Text = string.Empty;

                            this.odsCompetencia.Select();
                            this.odsCompetencia.DataBind();
                            this.grdCompetenciaHab.DataBind();
                        }
                    }
                    else
                    {
                        this.lblMensagem.Text = validacao.Mensagem;
                    }
                }
            }
            catch (Exception ex)
            {
                this.lblMensagem.Text = "ERRO:" + ex.Message;
            }
        }

        protected void btnSalvarGrupo_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ValidarGrupo())
                {
                    var TCHG = new TceCompetenciaHabilidadeGrupo();
                    var validacao = new ValidacaoDados();

                    TCHG.Curso = this.tseCurso.DBValue.ToString();
                    TCHG.TipoCurso = this.tseCurso["tipo"].ToString();
                    TCHG.Tipo = this.ddlTipo.SelectedValue;
                    TCHG.Modalidade = this.tseCurso["cod_modalidade"].ToString();
                    TCHG.Ano = Convert.ToInt32(this.ddlAno.SelectedValue.Substring(0, 4));
                    TCHG.Periodo = Convert.ToInt32(this.ddlAno.SelectedValue.Substring(7, 1));
                    TCHG.Subperiodo = Convert.ToInt32(this.ddlBimestre.SelectedValue);
                    TCHG.Serie = Convert.ToInt32(this.ddlSerie.SelectedValue);
                    TCHG.Disciplina = this.ddlDisciplinaGrupo.SelectedValue;
                    TCHG.Grupo = this.txtNomeGrupo.Text.Trim();
                    TCHG.Ordem = Convert.ToInt32(this.txtOrdemGrupo.Text.Trim());
                    TCHG.Matricula = this.User.Identity.Name;

                    validacao = CompetenciaHabilidadeGrupo.Validar(TCHG, true);

                    if (validacao.Valido)
                    {
                        if (CompetenciaHabilidadeGrupo.Inserir(TCHG) > 0)
                        {
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Grupo incluído com sucesso.');", true);
                            this.txtNomeGrupo.Text = string.Empty;
                            this.txtOrdemGrupo.Text = string.Empty;

                            this.ddlGrupo.Items.Clear();
                            this.ddlGrupo.DataSource = CompetenciaHabilidadeGrupo.Listar(this.ddlDisciplinaGrupo.SelectedValue, int.Parse(this.ddlAno.SelectedValue.Substring(0, 4)), int.Parse(this.ddlBimestre.SelectedValue), int.Parse(this.ddlSerie.SelectedValue), int.Parse(this.ddlAno.SelectedValue.Substring(7, 1)), this.tseCurso.DBValue.ToString(), ddlTipo.SelectedValue);

                            this.ddlGrupo.Items.Insert(0, "Selecione");
                            this.ddlGrupo.DataBind();

                            this.odsGrupo.Select();
                            this.odsGrupo.DataBind();
                            this.grdGrupo.DataBind();
                        }
                    }
                    else
                    {
                        this.lblMensagem.Text = validacao.Mensagem;
                    }
                }
            }
            catch (Exception ex)
            {
                this.lblMensagem.Text = "ERRO:" + ex.Message;
            }
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlSerie.Items.Clear();
            this.ddlTipo.ClearSelection();
            this.ddlBimestre.Items.Clear();
            this.ddlDisciplinaGrupo.Items.Clear();

            this.txtNomeGrupo.Text = string.Empty;
            this.txtOrdemGrupo.Text = string.Empty;

            this.ddlGrupo.Items.Clear();
            this.txtNomeCompetencia.Text = string.Empty;
            this.txtOrdemComp.Text = string.Empty;
            this.pnAbas.Visible = false;

            if (this.ddlAno.SelectedValue != "Selecione")
            {
                this.ddlSerie.DataSource = Serie.ListarSeries(this.tseCurso.DBValue.ToString());
                this.ddlSerie.Items.Insert(0, "Selecione");
                this.ddlSerie.DataBind();

                this.ddlBimestre.DataSource = SubperiodoLetivo.ListarBimestres(this.ddlAno.SelectedValue.Substring(0, 4), this.ddlAno.SelectedValue.Substring(7, 1));
                this.ddlBimestre.Items.Insert(0, "Selecione");
                this.ddlBimestre.DataBind();
            }
        }

        protected void ddlBimestre_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtNomeGrupo.Text = string.Empty;
            this.txtOrdemGrupo.Text = string.Empty;
            this.ddlDisciplinaGrupo.ClearSelection();
            this.ddlGrupo.Items.Clear();
            this.txtNomeCompetencia.Text = string.Empty;
            this.txtOrdemComp.Text = string.Empty;
            this.pnAbas.Visible = false;
        }

        protected void ddlDisciplinaGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtNomeGrupo.Text = string.Empty;
            this.txtOrdemGrupo.Text = string.Empty;

            this.ddlGrupo.Items.Clear();
            this.txtNomeCompetencia.Text = string.Empty;
            this.txtOrdemComp.Text = string.Empty;
            this.pnAbas.Visible = false;

            if (this.ddlDisciplinaGrupo.SelectedValue != "Selecione" && ddlAno.SelectedValue != "Selecione" && ddlBimestre.SelectedValue != "Selecione" && ddlTipo.SelectedValue != "Selecione")
            {
                this.pnAbas.Visible = true;

                var dt = CompetenciaHabilidadeGrupo.Listar(this.ddlDisciplinaGrupo.SelectedValue, int.Parse(this.ddlAno.SelectedValue.Substring(0, 4)), int.Parse(this.ddlBimestre.SelectedValue), int.Parse(this.ddlSerie.SelectedValue), int.Parse(this.ddlAno.SelectedValue.Substring(7, 1)), this.tseCurso.DBValue.ToString(), ddlTipo.SelectedValue);

                this.ddlGrupo.Items.Clear();
                this.ddlGrupo.DataSource = dt;
                this.ddlGrupo.Items.Insert(0, "Selecione");
                this.ddlGrupo.DataBind();
            }
            else
            {
                lblMensagem.Text = "Favor preencher os campos obrigatórios.";

            }
        }

        protected void ddlSerie_SelectedIndexChanged(object sender, EventArgs e)
        {

            this.ddlTipo.ClearSelection();
            this.ddlDisciplinaGrupo.Items.Clear();
            this.txtNomeGrupo.Text = string.Empty;
            this.txtOrdemGrupo.Text = string.Empty;
            this.ddlGrupo.Items.Clear();
            this.txtNomeCompetencia.Text = string.Empty;
            this.txtOrdemComp.Text = string.Empty;
            this.pnAbas.Visible = false;

            if (this.ddlSerie.SelectedValue != "Selecione")
            {
                this.ddlDisciplinaGrupo.DataSource = Disciplina.ListarDisciplina(this.ddlAno.SelectedValue.Substring(0, 4), this.tseCurso.DBValue.ToString(), this.ddlSerie.SelectedValue);
                this.ddlDisciplinaGrupo.Items.Insert(0, "Selecione");
                this.ddlDisciplinaGrupo.DataBind();
            }
        }

        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlDisciplinaGrupo.Items.Clear();
            this.txtNomeGrupo.Text = string.Empty;
            this.txtOrdemGrupo.Text = string.Empty;
            this.ddlGrupo.Items.Clear();
            this.txtNomeCompetencia.Text = string.Empty;
            this.txtOrdemComp.Text = string.Empty;
            this.pnAbas.Visible = false;

            if (this.ddlDisciplinaGrupo.SelectedValue != "Selecione" && ddlAno.SelectedValue != "Selecione" && ddlBimestre.SelectedValue != "Selecione" && ddlTipo.SelectedValue != "Selecione")
            {
                this.pnAbas.Visible = true;

                var dt = CompetenciaHabilidadeGrupo.Listar(this.ddlDisciplinaGrupo.SelectedValue, int.Parse(this.ddlAno.SelectedValue.Substring(0, 4)), int.Parse(this.ddlBimestre.SelectedValue), int.Parse(this.ddlSerie.SelectedValue), int.Parse(this.ddlAno.SelectedValue.Substring(7, 1)), this.tseCurso.DBValue.ToString(), ddlTipo.SelectedValue);

                this.ddlGrupo.Items.Clear();
                this.ddlGrupo.DataSource = dt;
                this.ddlGrupo.Items.Insert(0, "Selecione");
                this.ddlGrupo.DataBind();
            }
        }

        protected void grdCompetenciaHab_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            this.ControlaAcesso(this.grdCompetenciaHab);
        }

        protected void grdCompetenciaHab_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (this.grdCompetenciaHab.IsNewRowEditing)
            {
                if (e.Column.FieldName == "ID_COMPETENCIA_HABILIDADE_ITEM")
                {
                    e.Editor.Enabled = true;
                }

                if (e.Column.FieldName == "ID_COMPETENCIA_HABILIDADE_GRUPO")
                {
                    e.Editor.ReadOnly = true;
                }

                if (e.Column.FieldName == "DT_CADASTRO")
                {
                    e.Editor.ReadOnly = true;
                }

                if (e.Column.FieldName == "GRUPO")
                {
                    e.Editor.ReadOnly = true;
                }

                if (e.Column.FieldName == "ID_GRUPO")
                {
                    e.Editor.Enabled = true;
                }
            }
            else if (this.grdCompetenciaHab.IsEditing)
            {
                if (e.Column.FieldName == "ID_GRUPO")
                {
                    e.Editor.Enabled = false;
                }

                if (e.Column.FieldName == "ID_COMPETENCIA_HABILIDADE_ITEM")
                {
                    e.Editor.Enabled = false;
                }

                if (e.Column.FieldName == "ID_COMPETENCIA_HABILIDADE_GRUPO")
                {
                    e.Editor.ReadOnly = true;
                }

                if (e.Column.FieldName == "DT_CADASTRO")
                {
                    e.Editor.ReadOnly = true;
                }

                if (e.Column.FieldName == "GRUPO")
                {
                    e.Editor.ReadOnly = true;
                }
            }
        }

        protected void grdCompetenciaHab_InitNewRow(object sender, ASPxDataInitNewRowEventArgs e)
        {
            this.grdCompetenciaHab.Settings.ShowFilterRow = false;
        }

        protected void grdCompetenciaHab_RowValidating(object sender, ASPxDataValidationEventArgs e)
        {
            this.lblMensagem.Text = string.Empty;

            if (e.NewValues["GRUPO"] == null)
            {
                e.RowError = "Favor informar o Grupo.";
                return;
            }

            if (e.NewValues["ORDEM"] == null)
            {
                e.RowError = "Favor informar a Ordem.";
                return;
            }

            var grd = (ASPxGridView)sender;
            var TCHI = CompetenciaHabilidadeItem.Carregar(int.Parse(e.Keys[0].ToString()));

            if (grd != null && grd.IsNewRowEditing)
            {
                if (!CompetenciaHabilidadeItem.Validar(TCHI).Valido)
                {
                    e.RowError = "Competência/Habilidade já existente.";
                }
            }
        }

        protected void grdCompetenciaHab_StartRowEditing(object sender, ASPxStartRowEditingEventArgs e)
        {
            this.grdCompetenciaHab.Settings.ShowFilterRow = false;
        }

        protected void grdGrupo_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            this.ControlaAcesso(this.grdGrupo);
        }

        protected void grdGrupo_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (this.grdGrupo.IsNewRowEditing)
            {
                if (e.Column.FieldName == "ID_COMPETENCIA_HABILIDADE_GRUPO")
                {
                    e.Editor.Enabled = true;
                }

                if (e.Column.FieldName == "DISCIPLINA")
                {
                    e.Editor.ReadOnly = true;
                }

                if (e.Column.FieldName == "CURSO")
                {
                    e.Editor.ReadOnly = true;
                }

                if (e.Column.FieldName == "MODALIDADE")
                {
                    e.Editor.ReadOnly = true;
                }

                if (e.Column.FieldName == "TIPO_CURSO")
                {
                    e.Editor.ReadOnly = true;
                }

                if (e.Column.FieldName == "ANO")
                {
                    e.Editor.ReadOnly = true;
                }

                if (e.Column.FieldName == "SERIE")
                {
                    e.Editor.ReadOnly = true;
                }

                if (e.Column.FieldName == "TIPO_CURRICULO")
                {
                    e.Editor.ReadOnly = true;
                }

                if (e.Column.FieldName == "PERIODO")
                {
                    e.Editor.ReadOnly = true;
                }

                if (e.Column.FieldName == "SUBPERIODO")
                {
                    e.Editor.ReadOnly = true;
                }

                if (e.Column.FieldName == "DT_CADASTRO")
                {
                    e.Editor.ReadOnly = true;
                }

                if (e.Column.FieldName == "NOME_DISCIPLINA")
                {
                    e.Editor.ReadOnly = true;
                }
            }
            else if (this.grdGrupo.IsEditing)
            {
                if (e.Column.FieldName == "ID_COMPETENCIA_HABILIDADE_GRUPO")
                {
                    e.Editor.Enabled = false;
                }

                if (e.Column.FieldName == "DISCIPLINA")
                {
                    e.Editor.ReadOnly = true;
                }

                if (e.Column.FieldName == "CURSO")
                {
                    e.Editor.ReadOnly = true;
                }

                if (e.Column.FieldName == "MODALIDADE")
                {
                    e.Editor.ReadOnly = true;
                }

                if (e.Column.FieldName == "TIPO_CURSO")
                {
                    e.Editor.ReadOnly = true;
                }

                if (e.Column.FieldName == "ANO")
                {
                    e.Editor.ReadOnly = true;
                }

                if (e.Column.FieldName == "SERIE")
                {
                    e.Editor.ReadOnly = true;
                }

                if (e.Column.FieldName == "PERIODO")
                {
                    e.Editor.ReadOnly = true;
                }

                if (e.Column.FieldName == "SUBPERIODO")
                {
                    e.Editor.ReadOnly = true;
                }

                if (e.Column.FieldName == "DT_CADASTRO")
                {
                    e.Editor.ReadOnly = true;
                }

                if (e.Column.FieldName == "NOME_DISCIPLINA")
                {
                    e.Editor.ReadOnly = true;
                }
            }
        }

        protected void grdGrupo_InitNewRow(object sender, ASPxDataInitNewRowEventArgs e)
        {
            this.grdGrupo.Settings.ShowFilterRow = false;
        }

        protected void grdGrupo_RowValidating(object sender, ASPxDataValidationEventArgs e)
        {
            this.lblMensagem.Text = string.Empty;

            if (e.NewValues["GRUPO"] == null)
            {
                e.RowError = "Favor informar o Grupo.";
                return;
            }

            if (e.NewValues["ORDEM"] == null)
            {
                e.RowError = "Favor informar a Ordem.";
                return;
            }           
        }

        protected void grdGrupo_StartRowEditing(object sender, ASPxStartRowEditingEventArgs e)
        {
            this.grdGrupo.Settings.ShowFilterRow = false;
        }

        protected void odsCompetencia_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var validacao = new ValidacaoDados();
            var id = e.InputParameters["ID_COMPETENCIA_HABILIDADE_ITEM"].ToString();
            var TCHI = CompetenciaHabilidadeItem.Carregar(int.Parse(id));

            validacao = CompetenciaHabilidadeItem.ValidarExclusao(int.Parse(id));

            if (!validacao.Valido)
            {
                throw new Exception(validacao.Mensagem);
            }

            CompetenciaHabilidadeItem.Remover(int.Parse(id));
        }

        protected void odsCompetencia_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var validacao = new ValidacaoDados();
            var TCHI = new TceCompetenciaHabilidadeItem
                       {
                           IdCompetenciaHabilidadeItem = int.Parse(e.InputParameters["ID_COMPETENCIA_HABILIDADE_ITEM"].ToString()),
                           IdCompetenciaHabilidadeGrupo = Convert.ToInt32(grdCompetenciaHab.GetRowValues(grdCompetenciaHab.EditingRowVisibleIndex, "ID_COMPETENCIA_HABILIDADE_GRUPO")),
                           CompetenciaHabilidade = e.InputParameters["COMPETENCIA_HABILIDADE"] != null ? e.InputParameters["COMPETENCIA_HABILIDADE"].ToString() : null,
                           Ordem = e.InputParameters["ORDEM"] != null ? int.Parse(e.InputParameters["ORDEM"].ToString()) : -1,
                           Matricula = this.User.Identity.Name
                       };

            validacao = CompetenciaHabilidadeItem.Validar(TCHI);

            if (!validacao.Valido)
            {
                throw new Exception(validacao.Mensagem);
            }

            CompetenciaHabilidadeItem.Alterar(TCHI);
        }

        protected void odsGrupo_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var validacao = new ValidacaoDados();
            var id = e.InputParameters["ID_COMPETENCIA_HABILIDADE_GRUPO"].ToString();
            var TCHG = CompetenciaHabilidadeGrupo.Carregar(int.Parse(id));

            validacao = CompetenciaHabilidadeGrupo.ValidarExclusao(TCHG);

            if (!validacao.Valido)
            {
                throw new Exception(validacao.Mensagem);
            }
            else
            {
                if (CompetenciaHabilidadeGrupo.Remover(int.Parse(id)) > 0)
                {
                }
            }
        }

        protected void odsGrupo_Update(object sender, ObjectDataSourceStatusEventArgs e)
        {
            this.ddlGrupo.Items.Clear();
            this.ddlGrupo.DataSource = CompetenciaHabilidadeGrupo.Listar(this.ddlDisciplinaGrupo.SelectedValue, int.Parse(this.ddlAno.SelectedValue.Substring(0, 4)), int.Parse(this.ddlBimestre.SelectedValue), int.Parse(this.ddlSerie.SelectedValue), int.Parse(this.ddlAno.SelectedValue.Substring(7, 1)), this.tseCurso.DBValue.ToString(), ddlTipo.SelectedValue);

            this.ddlGrupo.Items.Insert(0, "Selecione");
            this.ddlGrupo.DataBind();
        }

        protected void odsGrupo_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var validacao = new ValidacaoDados();
            var TCHG = new TceCompetenciaHabilidadeGrupo();

            TCHG.IdCompetenciaHabilidadeGrupo = int.Parse(e.InputParameters["ID_COMPETENCIA_HABILIDADE_GRUPO"].ToString());
            TCHG.Curso = this.tseCurso.DBValue.ToString();
            TCHG.TipoCurso = this.tseCurso["tipo"].ToString();
            TCHG.Tipo = this.ddlTipo.SelectedValue;
            TCHG.Modalidade = this.tseCurso["cod_modalidade"].ToString();
            TCHG.Ano = Convert.ToInt32(this.ddlAno.SelectedValue.Substring(0, 4));
            TCHG.Periodo = Convert.ToInt32(this.ddlAno.SelectedValue.Substring(7, 1));
            TCHG.Subperiodo = Convert.ToInt32(this.ddlBimestre.SelectedValue);
            TCHG.Serie = Convert.ToInt32(this.ddlSerie.SelectedValue);
            TCHG.Disciplina = this.ddlDisciplinaGrupo.SelectedValue;
            TCHG.Grupo = e.InputParameters["GRUPO"].ToString();
            TCHG.Ordem = Convert.ToInt32(e.InputParameters["ORDEM"]);
            TCHG.Matricula = this.User.Identity.Name;

            validacao = CompetenciaHabilidadeGrupo.Validar(TCHG, false);

            if (!validacao.Valido)
            {
                throw new Exception(validacao.Mensagem);
            }
            else
            {
                if (CompetenciaHabilidadeGrupo.Alterar(TCHG) > 0)
                {
                }
            }
        }

        protected void pcCompHabil_TabClick(object source, TabControlCancelEventArgs e)
        {
            this.lblMensagem.Text = string.Empty;

            this.ddlGrupo.Items.Clear();
            this.ddlGrupo.DataSource = CompetenciaHabilidadeGrupo.Listar(this.ddlDisciplinaGrupo.SelectedValue, int.Parse(this.ddlAno.SelectedValue.Substring(0, 4)), int.Parse(this.ddlBimestre.SelectedValue), int.Parse(this.ddlSerie.SelectedValue), int.Parse(this.ddlAno.SelectedValue.Substring(7, 1)), this.tseCurso.DBValue.ToString(), ddlTipo.SelectedValue);
            this.ddlGrupo.Items.Insert(0, "Selecione");
            this.ddlGrupo.DataBind();
        }

        protected void tseCurso_Changed(object sender, ChangedEventArgs args)
        {
            if (this.Page.IsCallback)
            {
                return;
            }

            this.pnGeral.Visible = false;
            this.ddlAno.Items.Clear();
            this.ddlSerie.Items.Clear();
            this.ddlTipo.ClearSelection();
            this.ddlBimestre.Items.Clear();
            this.ddlDisciplinaGrupo.Items.Clear();

            this.txtNomeGrupo.Text = string.Empty;
            this.txtOrdemGrupo.Text = string.Empty;

            this.ddlGrupo.Items.Clear();
            this.txtNomeCompetencia.Text = string.Empty;
            this.txtOrdemComp.Text = string.Empty;
            this.pnAbas.Visible = false;

            this.pcCompHabil.Visible = false;

            if (!this.tseCurso.DBValue.IsNull)
            {
                this.pnGeral.Visible = true;
                this.pcCompHabil.Visible = true;

                this.ddlAno.DataSource = SubperiodoLetivo.ListarAnosPeriodo();
                this.ddlAno.Items.Insert(0, "Selecione");
                this.ddlAno.DataBind();
            }
        }

        private void InitializeComponent()
        {
        }

        private void Page_PreRenderComplete(object sender, EventArgs e)
        {
            this.ControlaAcesso(this.grdCompetenciaHab);
            this.ControlaAcesso(this.grdGrupo);
            ControlaAcesso(btnSalvarGrupo, AcaoControle.novo);
            ControlaAcesso(btnSalvarComp, AcaoControle.novo);
        }

        private void ValidarCampos()
        {

            txtOrdemComp.Attributes.Add("onkeyPress", "return isNumberKey(event);");
            //this.txtOrdemComp.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
            //this.txtOrdemComp.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");

            txtOrdemGrupo.Attributes.Add("onkeyPress", "return isNumberKey(event);");
            //this.txtOrdemGrupo.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
            //this.txtOrdemGrupo.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");

            //this.txtNomeCompetencia.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
            //this.txtNomeCompetencia.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");

            //this.txtNomeGrupo.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
            //this.txtNomeGrupo.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");
        }

        private bool ValidarCompetencia()
        {
            if (!this.ValidarDadosGerais())
            {
                return false;
            }

            if (this.ddlGrupo.SelectedValue == "Selecione")
            {
                this.lblMensagem.Text = "Favor selecionar o Grupo.";
                this.ddlGrupo.Focus();
                return false;
            }

            if (!this.txtOrdemComp.Text.IsNullOrEmptyOrWhiteSpace())
            {
                if (Convert.ToInt32(txtOrdemComp.Text) <= 0 )
                {
                    this.lblMensagem.Text = "O campo Ordem não pode ser igual a zero(0).";
                    this.txtOrdemGrupo.Focus();
                    return false;
                }
            }
            else
            {
                this.lblMensagem.Text = "Favor digitar a Ordem da Competencia/Habilidade.";
                this.txtOrdemComp.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(this.txtNomeCompetencia.Text.Trim()))
            {
                this.lblMensagem.Text = "Favor digitar o Nome da Competencia/Habilidade.";
                this.txtNomeCompetencia.Focus();
                return false;
            }

            return true;
        }

        private bool ValidarDadosGerais()
        {
            // é assim que verifica preenchimento do tseCurso?
            if (this.tseCurso.DBValue.IsNull)
            {
                this.lblMensagem.Text = "Favor selecionar a Escolaridade.";
                return false;
            }

            if (this.ddlAno.SelectedValue == "Selecione")
            {
                this.lblMensagem.Text = "Favor selecionar o ano.";
                this.ddlAno.Focus();
                return false;
            }

            if (this.ddlSerie.SelectedValue == "Selecione")
            {
                this.lblMensagem.Text = "Favor selecionar a série.";
                this.ddlSerie.Focus();
                return false;
            }

            if (this.ddlTipo.SelectedValue == "Selecione")
            {
                this.lblMensagem.Text = "Favor selecionar o tipo do curriculo.";
                this.ddlTipo.Focus();
                return false;
            }

            if (this.ddlBimestre.SelectedValue == "Selecione")
            {
                this.lblMensagem.Text = "Favor selecionar o Bimestre.";
                this.ddlBimestre.Focus();
                return false;
            }

            if (this.ddlDisciplinaGrupo.SelectedValue == "Selecione")
            {
                this.lblMensagem.Text = "Favor selecionar a Disciplina/Grupo.";
                this.ddlDisciplinaGrupo.Focus();
                return false;
            }

            return true;
        }

        private bool ValidarGrupo()
        {
            if (!this.ValidarDadosGerais())
            {
                return false;
            }


            if (txtOrdemGrupo.Text.IsNullOrEmptyOrWhiteSpace())
            {
                this.lblMensagem.Text = "Favor digitar a Ordem do Grupo.";
                this.txtOrdemGrupo.Focus();
                return false;
            }
            else
            {

                if (Convert.ToInt32(txtOrdemGrupo.Text) <= 0)
                {
                    this.lblMensagem.Text = "O campo Ordem não pode ser igual a zero(0).";
                    this.txtOrdemGrupo.Focus();
                    return false;
                }
            }

            if (string.IsNullOrEmpty(this.txtNomeGrupo.Text.Trim()))
            {
                this.lblMensagem.Text = "Favor digitar o Nome do Grupo.";
                this.txtNomeGrupo.Focus();
                return false;
            }

            return true;
        }
    }
}