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

    [NavUrl("~/Academico/SerieEntrada.aspx"), ControlText("SerieEntrada"), Title("Séries de Entrada")]
    public partial class SerieEntrada : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TituloGrid(grdSerieEntrada, "Anos de Escolaridade");

            var usuario = RN.Perfil.RetornaPerfilPor(this.User.Identity.Name);

            if (!IsPostBack)
            {
                ListaSerie();
            }
        }

        protected void ListaSerie()
        {
            cmbSerie.Items.Clear();
            if (tseEscolaridade.Value != null)
            {
                cmbSerie.DataSource = RN.TurnosVagas.SerieEntrada.ListaSeriePorCurso(tseEscolaridade.Value.ToString());
            }
            else
            {
                cmbSerie.DataSource = RN.TurnosVagas.SerieEntrada.ListaSerie();
            }
            cmbSerie.Items.Insert(0, "Selecione");
            cmbSerie.DataBind();
        }

        protected void tseEscolaridade_Changed(object sender, EventArgs args)
        {
            ListaSerie();
        }

        protected void tseNivel_Changed(object sender, EventArgs args)
        {
            MontaClausulaListaCurso();
        }

        protected void tseModalidade_Changed(object sender, EventArgs args)
        {
            MontaClausulaListaCurso();
        }

        protected void grdSerieEntrada_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdSerieEntrada);
        }

        protected void grdSerieEntrada_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdSerieEntrada.Settings.ShowFilterRow = false;
        }

        protected void grdSerieEntrada_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string cursoId = e.Keys["CURSO"].ToString();

            Techne.Lyceum.RN.TurnosVagas.SerieEntrada SerieEntrada = new Techne.Lyceum.RN.TurnosVagas.SerieEntrada();
            Techne.Lyceum.RN.TurnosVagas.Entidades.SerieEntrada EntSerieEntrada;

            if ((Techne.Lyceum.RN.TurnosVagas.SerieEntrada.VerificaSerieEntrada(cursoId, Convert.ToInt32(e.NewValues["SERIE"].ToString())))
                && Convert.ToInt32(e.NewValues["ENTRADA"]) == 0)
            {
                SerieEntrada.RemoveSerieEntrada(cursoId, Convert.ToInt32(e.NewValues["SERIE"].ToString()));
            }

            if ((!Techne.Lyceum.RN.TurnosVagas.SerieEntrada.VerificaSerieEntrada(cursoId, Convert.ToInt32(e.NewValues["SERIE"].ToString())))
                && Convert.ToInt32(e.NewValues["ENTRADA"]) == 1)
            {
                if (!Lyceum.RN.TurnosVagas.SerieEntrada.VerificaExistenciaSerieEntrada(cursoId))
                {
                    EntSerieEntrada = new Techne.Lyceum.RN.TurnosVagas.Entidades.SerieEntrada();

                    EntSerieEntrada.Entrada = Convert.ToInt32(e.NewValues["ENTRADA"]);
                    EntSerieEntrada.UsuarioId = this.User.Identity.Name;
                    EntSerieEntrada.DataCadastro = DateTime.Now;
                    EntSerieEntrada.CursoId = cursoId;
                    EntSerieEntrada.Serie = Convert.ToInt32(e.NewValues["SERIE"].ToString());

                    SerieEntrada.InsereSerieEntrada(EntSerieEntrada);

                    lblMensagem.Text = "Série de entrada gravada com sucesso!";
                }
                else
                {
                    throw new Exception("Já existe uma Série de Entrada para este Curso!");
                }
            }
        }

        protected void grdSerieEntrada_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdSerieEntrada.IsEditing)
            {
                if ((e.Column.FieldName) == "CURSO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "SERIE")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "DESCRICAO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "ENTRADA")
                    e.Editor.ReadOnly = false;
            }
        }

        public object ListaSerieEntrada(object CURSO, object SERIE)
        {
            if ((CURSO != null && CURSO.ToString() != "") && (SERIE != null && SERIE.ToString() != "Selecione"))
            {
                return RN.TurnosVagas.SerieEntrada.ListaSerieEntradaPorCursoSerie(Convert.ToString(CURSO), Convert.ToInt32(SERIE));
            }
            else if ((CURSO != null && CURSO.ToString() != "") && (SERIE == null || SERIE.ToString() == "Selecione"))
            {
                return RN.TurnosVagas.SerieEntrada.ListaSerieEntradaPorCurso(Convert.ToString(CURSO));
            };

            return null;
        }

        public void Update(object CURSO, object SERIE, object DESCRICAO, object ENTRADA)
        {
        }

        protected void odsSerieEntrada_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
        }

        protected void MontaClausulaListaCurso()
        {
            tseEscolaridade.ResetValue();
            tseEscolaridade.SqlWhere = "1=1";

            if (tseNivel.Value != null)
            {
                tseEscolaridade.SqlWhere += " AND TIPO = '" + tseNivel.Value + "'";
            }
            if (tseModalidade.Value != null)
            {
                tseEscolaridade.SqlWhere += " AND MODALIDADE = '" + tseModalidade.Value + "'";
            }
        }
    }
}

