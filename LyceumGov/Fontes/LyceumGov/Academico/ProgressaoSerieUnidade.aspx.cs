using System;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.Data;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Web;
using Techne.Controls;
using System.Collections.Generic;
using System.Web.UI;
using System.Data;

namespace Techne.Lyceum.Net.Academico
{

    [NavUrl("~/Academico/ProgressaoSerieUnidade.aspx"), ControlText("ProgressaoSerieUnidade"), Title("Progressao de Série por Unidade")]
    public partial class ProgressaoSerieUnidade : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TituloGrid(grdProgressaoSerieUnidade, "Anos de Escolaridade");

            if (!IsPostBack)
            {
                ListaModalidade();
                ListaNivel();
                ListaSerie();
                ListaModalidadeProximoCursoSerie();
                ListaNivelProximoCursoSerie();
                ListaSerieProximoCursoSerie();
            }
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdProgressaoSerieUnidade);
            ControlaAcesso(btnSalvar);
        }

        protected void grdProgressaoSerieUnidade_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdProgressaoSerieUnidade);
            ControlaAcesso(btnSalvar);
        }

        protected void grdProgressaoSerieUnidade_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdProgressaoSerieUnidade.Settings.ShowFilterRow = false;
        }

        protected void grdProgressaoSerieUnidade_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdProgressaoSerieUnidade.IsEditing)
            {
                if ((e.Column.FieldName) == "COD_CURSO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "NOME_CURSO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "NOME_NIVEL")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "SERIE")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "PROX_COD_CURSO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "PROX_NOME_CURSO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "PROX_NOME_NIVEL")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "PROX_SERIE")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "DATACADASTRO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "USUARIOID")
                    e.Editor.ReadOnly = true;
            }
        }

        protected void grdProgressaoSerieUnidade_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string ProgressaoSerie_UnidadeEnsino_Id = e.Keys["PROGRESSAOSERIE_UNIDADEENSINO_ID"].ToString();

            Techne.Lyceum.RN.TurnosVagas.ProgressaoSerie_UnidadeEnsino ProgressaoSerie_UnidadeEnsino = new Techne.Lyceum.RN.TurnosVagas.ProgressaoSerie_UnidadeEnsino();
            Techne.Lyceum.RN.TurnosVagas.Entidades.ProgressaoSerie_UnidadeEnsino EntProgressaoSerie_UnidadeEnsino;

            EntProgressaoSerie_UnidadeEnsino = new Techne.Lyceum.RN.TurnosVagas.Entidades.ProgressaoSerie_UnidadeEnsino();

            EntProgressaoSerie_UnidadeEnsino.ProgressaoSerie_UnidadeEnsino_Id = Convert.ToInt32(ProgressaoSerie_UnidadeEnsino_Id);
            EntProgressaoSerie_UnidadeEnsino.Serie = Convert.ToInt32(e.NewValues["SERIE"].ToString().Substring(0,1));
            EntProgressaoSerie_UnidadeEnsino.Preferencial = Convert.ToInt32(e.NewValues["PREFERENCIAL"]);
            EntProgressaoSerie_UnidadeEnsino.CursoId = e.NewValues["COD_CURSO"].ToString();
            EntProgressaoSerie_UnidadeEnsino.UnidadeEnsinoId = tseUnidadeEnsino.Value.ToString();



            if ((Convert.ToInt32(e.NewValues["PREFERENCIAL"]) == 0)
                && (ProgressaoSerie_UnidadeEnsino.VerificaProgressaoSerie_UnidadeEnsinoPreferencial(EntProgressaoSerie_UnidadeEnsino)))
            {
                lblMensagem.Text = "";
                throw new Exception("Esta progressão não pode deixar de ser preferencial, pois deve existir pelo menos uma progressão preferencial para a unidade, curso e série!");
            }
            else if ((Convert.ToInt32(e.NewValues["PREFERENCIAL"]) == 1)
                && (ProgressaoSerie_UnidadeEnsino.ExisteOutraProgressaoSerie_UnidadeEnsino(EntProgressaoSerie_UnidadeEnsino)))
            {
                ProgressaoSerie_UnidadeEnsino.AlteraProgressaoSerie_UnidadeEnsinoPreferencial(EntProgressaoSerie_UnidadeEnsino);
                ProgressaoSerie_UnidadeEnsino.AlteraProgressaoSerie_UnidadeEnsinoPreferencial(Convert.ToInt32(ProgressaoSerie_UnidadeEnsino_Id));
            }

            grdProgressaoSerieUnidade.CancelEdit();
            e.Cancel = true;

            ListaProgressaoSerieUnidade(tseUnidadeEnsino.Value.ToString(), tseCurso.Value.ToString());
        }

        protected void grdProgressaoSerieUnidade_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            string ProgressaoSerie_UnidadeEnsino_Id = e.Keys["PROGRESSAOSERIE_UNIDADEENSINO_ID"].ToString();
            Techne.Lyceum.RN.TurnosVagas.ProgressaoSerie_UnidadeEnsino ProgressaoSerie_UnidadeEnsino = new Techne.Lyceum.RN.TurnosVagas.ProgressaoSerie_UnidadeEnsino();
            Techne.Lyceum.RN.TurnosVagas.Entidades.ProgressaoSerie_UnidadeEnsino EntProgressaoSerie_UnidadeEnsino;

            EntProgressaoSerie_UnidadeEnsino = new Techne.Lyceum.RN.TurnosVagas.Entidades.ProgressaoSerie_UnidadeEnsino();

            EntProgressaoSerie_UnidadeEnsino.ProgressaoSerie_UnidadeEnsino_Id = Convert.ToInt32(ProgressaoSerie_UnidadeEnsino_Id);
            EntProgressaoSerie_UnidadeEnsino.Serie = Convert.ToInt32(e.Values["SERIE"].ToString().Substring(0, 1));
            EntProgressaoSerie_UnidadeEnsino.CursoId = e.Values["COD_CURSO"].ToString();
            EntProgressaoSerie_UnidadeEnsino.ProximaSerie = Convert.ToInt32(e.Values["PROX_SERIE"].ToString().Substring(0, 1));
            EntProgressaoSerie_UnidadeEnsino.ProximoCursoId = e.Values["PROX_COD_CURSO"].ToString();
            EntProgressaoSerie_UnidadeEnsino.UnidadeEnsinoId = tseUnidadeEnsino.Value.ToString();

            if ((Convert.ToInt32(e.Values["PREFERENCIAL"]) == 1) 
                && (ProgressaoSerie_UnidadeEnsino.ExisteOutraProgressaoSerie_UnidadeEnsino(EntProgressaoSerie_UnidadeEnsino)))
            {
                lblMensagem.Text = "";
                throw new Exception("Não é possível excluir uma progressão preferencial, por favor incluir ou alterar uma outra progressão de mesma unidade, curso e série para preferencial!");
            }
            else
            {
                ProgressaoSerie_UnidadeEnsino.RemoveProgressaoSerie_UnidadeEnsino(Convert.ToInt32(ProgressaoSerie_UnidadeEnsino_Id));
                LimpaCampos();
            }
        }

        public void Delete(object PROGRESSAOSERIE_UNIDADEENSINO_ID)
        {
        }

        protected void odsProgressaoSerieUnidade_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
        }

        protected void ListaModalidade()
        {
            cmbModalidade.Items.Clear();
            cmbModalidade.DataSource = RN.Curso.ConsultarModalidadeCurso();
            cmbModalidade.Items.Insert(0, "Selecione");
            cmbModalidade.DataBind();
        }

        protected void ListaNivel()
        {
            cmbNivel.Items.Clear();
            cmbNivel.DataSource = RN.Curso.ConsultarTipoCurso();
            cmbNivel.Items.Insert(0, "Selecione");
            cmbNivel.DataBind();
        }

        protected void ListaSerie()
        {
            cmbSerie.Items.Clear();
            if (tseCurso.Value != null)
            {
                cmbSerie.DataSource = RN.TurnosVagas.SerieEntrada.ListaSeriePorCurso(tseCurso.Value.ToString());
            }
            else
            {
                cmbSerie.DataSource = RN.TurnosVagas.SerieEntrada.ListaSerie();
            }
            cmbSerie.Items.Insert(0, "Selecione");
            cmbSerie.DataBind();
        }
        protected void ListaModalidadeProximoCursoSerie()
        {
            cmbModalidadeProximoCursoSerie.Items.Clear();
            cmbModalidadeProximoCursoSerie.DataSource = RN.Curso.ConsultarModalidadeCurso();
            cmbModalidadeProximoCursoSerie.Items.Insert(0, "Selecione");
            cmbModalidadeProximoCursoSerie.DataBind();
        }

        protected void ListaNivelProximoCursoSerie()
        {
            cmbNivelProximoCursoSerie.Items.Clear();
            cmbNivelProximoCursoSerie.DataSource = RN.Curso.ConsultarTipoCurso();
            cmbNivelProximoCursoSerie.Items.Insert(0, "Selecione");
            cmbNivelProximoCursoSerie.DataBind();
        }

        protected void ListaSerieProximoCursoSerie()
        {
            cmbSerieProximoCursoSerie.Items.Clear();
            if (tseCursoProximoCursoSerie.Value != null)
            {
                cmbSerieProximoCursoSerie.DataSource = RN.TurnosVagas.SerieEntrada.ListaSeriePorCurso(tseCursoProximoCursoSerie.Value.ToString());
            }
            else
            {
                cmbSerieProximoCursoSerie.DataSource = RN.TurnosVagas.SerieEntrada.ListaSerie();
            }
            cmbSerieProximoCursoSerie.Items.Insert(0, "Selecione");
            cmbSerieProximoCursoSerie.DataBind();
        }

        public static DataTable ListaProgressaoSerieUnidade(object UnidadeEnsinoId, object CursoId)
        {
            if ((UnidadeEnsinoId != null && UnidadeEnsinoId.ToString() != "") && (CursoId != null && CursoId.ToString() != ""))
            {
                return RN.TurnosVagas.ProgressaoSerie_UnidadeEnsino.ListaProgressaoSerie_UnidadeEnsinoPorCurso(UnidadeEnsinoId.ToString(), CursoId.ToString());
            }

            return null;
        
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {

            if (ValidaCampos())
            {
                Techne.Lyceum.RN.TurnosVagas.ProgressaoSerie_UnidadeEnsino ProgressaoSerie_UnidadeEnsino = new Techne.Lyceum.RN.TurnosVagas.ProgressaoSerie_UnidadeEnsino();
                Techne.Lyceum.RN.TurnosVagas.Entidades.ProgressaoSerie_UnidadeEnsino EntProgressaoSerie_UnidadeEnsino;

                EntProgressaoSerie_UnidadeEnsino = new Techne.Lyceum.RN.TurnosVagas.Entidades.ProgressaoSerie_UnidadeEnsino();

                EntProgressaoSerie_UnidadeEnsino.Serie = Convert.ToInt32(cmbSerie.SelectedValue.ToString());
                EntProgressaoSerie_UnidadeEnsino.Preferencial = Convert.ToInt16(chkPreferencial.Checked);
                EntProgressaoSerie_UnidadeEnsino.UsuarioId = this.User.Identity.Name;
                EntProgressaoSerie_UnidadeEnsino.DataCadastro = DateTime.Now;
                EntProgressaoSerie_UnidadeEnsino.CursoId = tseCurso.Value.ToString();
                EntProgressaoSerie_UnidadeEnsino.ProximoCursoId = tseCursoProximoCursoSerie.Value.ToString();
                EntProgressaoSerie_UnidadeEnsino.ProximaSerie = Convert.ToInt32(cmbSerieProximoCursoSerie.SelectedValue.ToString());
                EntProgressaoSerie_UnidadeEnsino.UnidadeEnsinoId = tseUnidadeEnsino.Value.ToString();

                if (!ProgressaoSerie_UnidadeEnsino.VerificaProgressaoSerie_UnidadeEnsino(EntProgressaoSerie_UnidadeEnsino))
                {
                    if (!ProgressaoSerie_UnidadeEnsino.VerificaProgressaoSerie_UnidadeEnsinoPreferencial(EntProgressaoSerie_UnidadeEnsino))
                    {
                        EntProgressaoSerie_UnidadeEnsino.Preferencial = 1;
                    }
                    else
                    {
                        if ((chkPreferencial.Checked))
                        {
                            ProgressaoSerie_UnidadeEnsino.AlteraProgressaoSerie_UnidadeEnsinoPreferencial(EntProgressaoSerie_UnidadeEnsino);
                        }
                    }
                    ProgressaoSerie_UnidadeEnsino.InsereProgressaoSerie_UnidadeEnsino(EntProgressaoSerie_UnidadeEnsino);

                    LimpaCampos();
                    lblMensagem.Text += "Progressão gravada com sucesso!";
                }
                else
                {
                    lblMensagem.Text = "Já existe o registro!";
                }
            }
        }

        protected void tseUnidadeEnsino_Changed(object sender, EventArgs args)
        {
            MontaClausulaListaCurso();
            cmbModalidade.SelectedIndex = 0;
            cmbNivel.SelectedIndex = 0;
            cmbSerie.SelectedIndex = 0;
            LimpaCampos();
            lblMensagem.Text = "";
        }

        protected void cmbModalidade_SelectedIndexChanged(object sender, EventArgs args)
        {
            MontaClausulaListaCurso();
            lblMensagem.Text = "";
        }

        protected void cmbNivel_SelectedIndexChanged(object sender, EventArgs args)
        {
            MontaClausulaListaCurso();
            lblMensagem.Text = "";
        }

        protected void tseCurso_Changed(object sender, EventArgs args)
        {
            ListaSerie();
            lblMensagem.Text = "";
        }

        protected void cmbModalidadeProximoCursoSerie_SelectedIndexChanged(object sender, EventArgs args)
        {
            MontaClausulaListaCursoProximoCursoSerie();
            lblMensagem.Text = "";
        }

        protected void cmbNivelProximoCursoSerie_SelectedIndexChanged(object sender, EventArgs args)
        {
            MontaClausulaListaCursoProximoCursoSerie();
            lblMensagem.Text = "";
        }

        protected void tseCursoProximoCursoSerie_Changed(object sender, EventArgs args)
        {
            ListaSerieProximoCursoSerie();
            lblMensagem.Text = "";
        }

        protected void MontaClausulaListaCurso()
        {
            tseCurso.ResetValue();
            tseCurso.SqlWhere = "1=1";

            if (tseUnidadeEnsino.Value != null)
            {
                tseCurso.SqlWhere += " AND U.UNIDADE_ENS = '" + tseUnidadeEnsino.Value + "'";
            }
            if (cmbModalidade.SelectedValue != "Selecione")
            {
                tseCurso.SqlWhere += " AND M.MODALIDADE = '" + cmbModalidade.SelectedValue + "'";
            }
            if (cmbNivel.SelectedValue != "Selecione")
            {
                tseCurso.SqlWhere += " AND T.TIPO = '" + cmbNivel.SelectedValue + "'";
            }
        }

        public void MontaClausulaListaCursoProximoCursoSerie()
        {
            tseCursoProximoCursoSerie.ResetValue();
            tseCursoProximoCursoSerie.SqlWhere = "1=1";

            if (tseUnidadeEnsino.Value != null)
            {
                tseCursoProximoCursoSerie.SqlWhere += " AND U.UNIDADE_ENS = '" + tseUnidadeEnsino.Value + "'";
            }
            if (cmbModalidadeProximoCursoSerie.SelectedValue != "Selecione")
            {
                tseCursoProximoCursoSerie.SqlWhere += " AND M.MODALIDADE = '" + cmbModalidadeProximoCursoSerie.SelectedValue + "'";
            }
            if (cmbNivelProximoCursoSerie.SelectedValue != "Selecione")
            {
                tseCursoProximoCursoSerie.SqlWhere += " AND T.TIPO = '" + cmbNivelProximoCursoSerie.SelectedValue + "'";
            }
        }

        protected void LimpaCampos()
        {
            cmbModalidadeProximoCursoSerie.SelectedIndex = 0;
            cmbNivelProximoCursoSerie.SelectedIndex = 0;
            tseCursoProximoCursoSerie.ResetValue();
            cmbSerieProximoCursoSerie.SelectedIndex = 0;
            grdProgressaoSerieUnidade.DataBind();
            chkPreferencial.Checked = false;
            lblMensagem.Text = "";
        }

        protected Boolean ValidaCampos()
        {
            if (tseUnidadeEnsino.Value == null)
            {
                lblMensagem.Text = "O campo Unidade de Ensino é obrigatório.";
                return false;
            }
            if (cmbModalidade.SelectedIndex == 0)
            {
                lblMensagem.Text = "O campo Modalidade é obrigatório.";
                return false;
            }
            if (cmbNivel.SelectedIndex == 0)
            {
                lblMensagem.Text = "O campo Nível é obrigatório.";
                return false;
            }
            if (tseCurso.Value == null)
            {
                lblMensagem.Text = "O campo Curso é obrigatório.";
                return false;
            }
            if (cmbSerie.SelectedIndex == 0)
            {
                lblMensagem.Text = "O campo Série é obrigatório.";
                return false;
            }
            if (cmbModalidadeProximoCursoSerie.SelectedIndex == 0)
            {
                lblMensagem.Text = "O campo Modalidade do próximo Curso/Série é obrigatório.";
                return false;
            }
            if (cmbNivelProximoCursoSerie.SelectedIndex == 0)
            {
                lblMensagem.Text = "O campo Nível do próximo Curso/Série é obrigatório.";
                return false;
            }
            if (tseCursoProximoCursoSerie.Value == null)
            {
                lblMensagem.Text = "O campo Próximo Curso é obrigatório.";
                return false;
            }
            if (cmbSerieProximoCursoSerie.SelectedIndex == 0)
            {
                lblMensagem.Text = "O campo Série do próximo Curso/Série é obrigatório.";
                return false;
            }
            if ((tseCurso.Value.ToString() == tseCursoProximoCursoSerie.Value.ToString()) && (Convert.ToInt32(cmbSerie.SelectedValue) >= Convert.ToInt32(cmbSerieProximoCursoSerie.SelectedValue)))
            {
                lblMensagem.Text = "Não pode existir uma progressão de série para uma mesma série ou para uma série inferior.";
                return false;
            }

            return true;
        }
    }
}
