using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Techne.Controls;
using Techne.Data;
using Techne.Web;

namespace Techne.Lyceum.Net.Matricula
{
    [NavUrl("~/Matricula/ControleRenovacao.aspx")
    , ControlText("ControleRenovacao")
    , Title("Controle de Renovação de Matrícula")]
    public partial class ControleRenovacao : TPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdProgressao, "Controle de Renovação de Matrícula.");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdProgressao);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;

            if (!IsPostBack)
            {
                CarregarDados();
            }
        }

        private void CarregarDados()
        {
            CarregarModalidade();
        }

        private void CarregarModalidade()
        {
            ddlModalidade.Items.Clear();
            ddlModalidade.DataSource = RN.Curso.ListarModalidadeSerie();
            ddlModalidade.DataBind();
            ListItem itemVazio = new ListItem("Selecione", "");
            ddlModalidade.Items.Insert(0, itemVazio);
        }       

        private void CarregarNivel()
        {
            ddlNivel.Items.Clear();
            ddlNivel.DataSource = RN.Curso.ListarTipoCurso();
            ddlNivel.DataBind();
            ListItem itemVazio = new ListItem("Selecione", "");
            ddlNivel.Items.Insert(0, itemVazio);
        }
       
        protected void ddlModalidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlNivel.ClearSelection();
            tseCurso.ResetValue();
            cmbSerie.Items.Clear();

            CarregarNivel();
        }

        protected void ddlNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlNivel.SelectedValue))
            {
                if (!string.IsNullOrEmpty(ddlNivel.SelectedValue))
                {
                    tseCurso.SqlWhere = "c.modalidade = '" + ddlModalidade.SelectedValue + "' AND c.tipo = '" + ddlNivel.SelectedValue + "'";
                }
                else
                {
                    tseCurso.SqlWhere = "c.tipo = '" + ddlNivel.SelectedValue + "'";
                }
            }
        }       

        protected void tseCurso_Changed(object sender, ChangedEventArgs args)
        {
            if (tseCurso.DBValue.IsNull || !tseCurso.IsValidDBValue)
            {
                cmbSerie.Items.Clear();
            }
            else
            {
                cmbSerie.Items.Clear();

                if (!tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue)
                {
                    CarregarSeries();
                }
            }

        }

        protected void cmbSerie_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((!string.IsNullOrEmpty(cmbSerie.SelectedItem.Value.ToString().Trim()) && cmbSerie.SelectedValue != "Selecione")
                && !string.IsNullOrEmpty(tseCurso.Value.ToString().Trim()))
            {
                CarregarControleRenovacao(cmbSerie.SelectedItem.Value.ToString().Trim(), tseCurso.Value.ToString().Trim());
            }
            else
            {
                grdProgressao.DataSource = null;
                grdProgressao.DataBind();
                lblMensagem.Text = "Todos os campos são obrigatórios!";
            }
        }

        private void CarregarControleRenovacao(
            string pSerie
            , string pCurso
            )
        {
            Techne.Lyceum.RN.ProgressaoCursoSerie service
                = new Techne.Lyceum.RN.ProgressaoCursoSerie();

            IList<Techne.Lyceum.RN.Entidades.ProgressaoCursoSerie> lista
                = new List<Techne.Lyceum.RN.Entidades.ProgressaoCursoSerie>();

            lista = service.RetornaProgressaoCursoSeriePor(
                    pSerie
                    , pCurso);

            if (lista.Count > 0)
            {
                grdProgressao.Visible = true;
                grdProgressao.DataSource = lista;
                grdProgressao.DataBind();
            }
            else
            {
                grdProgressao.DataSource = null;
                grdProgressao.Visible = false;
            }
        }

        private void CarregarSeries()
        {
            cmbSerie.Items.Clear();
            cmbSerie.DataSource = RN.Serie.ConsultarSeriesPorCursoDt(tseCurso.DBValue.ToString());
            cmbSerie.DataBind();
            ListItem itemVazio = new ListItem("Selecione", "");
            cmbSerie.Items.Insert(0, itemVazio);
        }        

        private void CarregarDropDownList(DropDownList drop, object data, string defaultValue)
        {
            drop.Items.Clear();
            drop.DataSource = data;
            drop.DataBind();
            ListItem itemVazio = new ListItem("Selecione", "");
            drop.Items.Insert(0, itemVazio);
        }

        public void Delete(object Progressaoid)
        {
        }

        protected void grdProgressao_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            int pId = -1;
            grdProgressao.Columns["IdProgressaoSerie"].Visible = true;

            Techne.Lyceum.RN.ProgressaoCursoSerie service
                = new Techne.Lyceum.RN.ProgressaoCursoSerie();

            //var validacao = new ValidacaoDados();

            var usuario = RN.Perfil.RetornaPerfilPor(this.User.Identity.Name);

            if (!e.Values["IdProgressaoSerie"].Equals("") || e.Values["IdProgressaoSerie"].Equals("-1"))
            {
                pId = Convert.ToInt32(e.Values["IdProgressaoSerie"]);
            }

            if (pId > 0)
            {
                service.Remove(pId);
            }

            grdProgressao.CancelEdit();
            e.Cancel = true;

            CarregarControleRenovacao(cmbSerie.SelectedItem.Value.ToString().Trim(), tseCurso.Value.ToString().Trim());
            grdProgressao.Columns["IdProgressaoSerie"].Visible = false;
        }

        protected void grdProgressao_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            CarregarControleRenovacao(cmbSerie.SelectedItem.Value.ToString().Trim(), tseCurso.Value.ToString().Trim());
            this.ControlaAcesso(this.grdProgressao);
        }

        protected void grdProgressao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdProgressao.Settings.ShowFilterRow = false;
        }

        protected void grdProgressao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            Techne.Lyceum.RN.ProgressaoCursoSerie rnControleRenovacao = new Techne.Lyceum.RN.ProgressaoCursoSerie();

            int id = Convert.ToInt32(e.Keys["IdProgressaoSerie"]);
            string usuarioId = User.Identity.Name;
            bool participaFase1 = (e.NewValues["ParticipaFase1"] == null || Convert.ToBoolean(e.NewValues["ParticipaFase1"]) == false) ? false : true;
            bool participaFase2 = (e.NewValues["ParticipaFase2"] == null || Convert.ToBoolean(e.NewValues["ParticipaFase2"]) == false) ? false : true;

            rnControleRenovacao.AlteraParticipaFase(id, participaFase1, participaFase2, usuarioId);

            grdProgressao.CancelEdit();
            e.Cancel = true;
        }
    }
}
