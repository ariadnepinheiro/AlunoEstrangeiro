using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Techne.Controls;
using Techne.Data;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/ProgressaoCursoSerie.aspx")
    , ControlText("ProgressaoCursoSerie")
    , Title("Progressão de Curso e Série")]
    public partial class ProgressaoCursoSerie : TPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdProgressao, "Progressão de Curso e Série.");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnSalvar, AcaoControle.novo);
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
            CarregarModalidadeProx();
        }

        private void CarregarModalidade()
        {
            ddlModalidade.Items.Clear();
            ddlModalidade.DataSource = RN.Curso.ListarModalidadeSerie();
            ddlModalidade.DataBind();
            ListItem itemVazio = new ListItem("Selecione", "");
            ddlModalidade.Items.Insert(0, itemVazio);
        }

        private void CarregarModalidadeProx()
        {
            ddlModalidadeProx.Items.Clear();
            ddlModalidadeProx.DataSource = RN.Curso.ListarModalidadeSerie();
            ddlModalidadeProx.DataBind();
            ListItem itemVazio = new ListItem("Selecione", "");
            ddlModalidadeProx.Items.Insert(0, itemVazio);
        }

        private void CarregarNivel()
        {
            ddlNivel.Items.Clear();
            ddlNivel.DataSource = RN.Curso.ListarTipoCurso();
            ddlNivel.DataBind();
            ListItem itemVazio = new ListItem("Selecione", "");
            ddlNivel.Items.Insert(0, itemVazio);
        }

        private void CarregarNivelProx()
        {
            ddlNivelProx.Items.Clear();
            ddlNivelProx.DataSource = RN.Curso.ListarTipoCurso();
            ddlNivelProx.DataBind();
            ListItem itemVazio = new ListItem("Selecione", "");
            ddlNivelProx.Items.Insert(0, itemVazio);
        }

        protected void ddlModalidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlNivel.ClearSelection();
            tseCurso.ResetValue();
            cmbSerie.Items.Clear();

            CarregarNivel();
        }

        protected void ddlModalidadeProx_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlNivelProx.ClearSelection();
            tseCursoProx.ResetValue();
            cmbSerieProx.Items.Clear();

            CarregarNivelProx();
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

        protected void ddlNivelProx_SelectedIndexChanged(object sender, EventArgs e)
        {
            tseCursoProx.ResetValue();
            cmbSerieProx.Items.Clear();

            if (!string.IsNullOrEmpty(ddlNivelProx.SelectedValue))
            {
                if (!string.IsNullOrEmpty(ddlNivelProx.SelectedValue))
                {
                    tseCursoProx.SqlWhere = "c.modalidade = '" + ddlModalidadeProx.SelectedValue + "' AND c.tipo = '" + ddlNivelProx.SelectedValue + "'";
                }
                else
                {
                    tseCursoProx.SqlWhere = "c.tipo = '" + ddlNivelProx.SelectedValue + "'";
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
                CarregarProgressaoCursoSerie(cmbSerie.SelectedItem.Value.ToString().Trim(), tseCurso.Value.ToString().Trim());
            }
            else
            {
                grdProgressao.DataSource = null;
                grdProgressao.DataBind();
                lblMensagem.Text = "Todos os campos são obrigatórios!";
            }
        }

        private void CarregarProgressaoCursoSerie(
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

        protected void tseCursoProx_Changed(object sender, ChangedEventArgs args)
        {
            if (tseCursoProx.DBValue.IsNull || !tseCursoProx.IsValidDBValue)
            {
                cmbSerieProx.Items.Clear();
            }
            else
            {
                cmbSerieProx.Items.Clear();

                if (!tseCursoProx.DBValue.IsNull && tseCursoProx.IsValidDBValue)
                {
                    CarregarSeriesProx();
                }
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

        private void CarregarSeriesProx()
        {
            QueryTable dadosDrop = null;

            dadosDrop = RN.Serie.ConsultarSeriesPorCurso(tseCursoProx.DBValue.ToString());
            CarregarDropDownList(cmbSerieProx, dadosDrop, "");
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

            Techne.Lyceum.RN.ProgressaoCursoSerie service = new Techne.Lyceum.RN.ProgressaoCursoSerie();

            //var validacao = new ValidacaoDados();
            //var usuario = RN.Perfil.RetornaPerfilPor(this.User.Identity.Name);

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

            CarregarProgressaoCursoSerie(cmbSerie.SelectedItem.Value.ToString().Trim(), tseCurso.Value.ToString().Trim());
            grdProgressao.Columns["IdProgressaoSerie"].Visible = false;
        }

        protected void grdProgressao_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            CarregarProgressaoCursoSerie(cmbSerie.SelectedItem.Value.ToString().Trim(), tseCurso.Value.ToString().Trim());
            this.ControlaAcesso(this.grdProgressao);
        }

        protected void grdProgressao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdProgressao.Settings.ShowFilterRow = false;
        }       

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            Techne.Lyceum.RN.ProgressaoCursoSerie service = new Techne.Lyceum.RN.ProgressaoCursoSerie();
            Techne.Lyceum.RN.Entidades.ProgressaoCursoSerie objProgressaoSerie;
            bool blnRetorno = false;

            if ((tseCurso.Value != null)
                && (cmbSerie.SelectedValue.ToString().Trim() != "Selecione" && !string.IsNullOrEmpty(cmbSerie.SelectedValue))
                && (!string.IsNullOrEmpty(ddlModalidade.SelectedValue))
                && (!string.IsNullOrEmpty(ddlNivel.SelectedValue))
                && (tseCursoProx.Value != null)
                && (!string.IsNullOrEmpty(cmbSerieProx.SelectedValue) || !cmbSerieProx.SelectedValue.ToString().Trim().Equals(""))
                && (!string.IsNullOrEmpty(ddlModalidadeProx.SelectedValue))
                && (!string.IsNullOrEmpty(ddlNivelProx.SelectedValue))

                )
            {
                objProgressaoSerie = new Techne.Lyceum.RN.Entidades.ProgressaoCursoSerie();
                objProgressaoSerie.Curso = tseCurso.Value.ToString().Trim();
                objProgressaoSerie.Serie = cmbSerie.SelectedValue.Trim();
                objProgressaoSerie.ProxCurso = tseCursoProx.Value.ToString().Trim();
                objProgressaoSerie.ProxSerie = cmbSerieProx.SelectedValue.ToString().Trim();
                objProgressaoSerie.ParticipaFase1 = false;
                objProgressaoSerie.ParticipaFase2 = false;
                objProgressaoSerie.Matricula = User.Identity.Name.ToString().Trim();

                blnRetorno = service.VerificaProgressao(objProgressaoSerie);

                if (!blnRetorno)
                {
                    if (Convert.ToInt32(objProgressaoSerie.Serie) <= Convert.ToInt32(objProgressaoSerie.ProxSerie) ||
                        objProgressaoSerie.Curso != objProgressaoSerie.ProxCurso)
                    {
                        service.Salva(objProgressaoSerie);
                        LimparProximo();
                    }
                    else
                    {
                        lblMensagem.Text = "A série não pode ser menor que a próxima!";
                    }
                }
                else
                {
                    lblMensagem.Text = "Já existe o registro!";
                }
                CarregarProgressaoCursoSerie(cmbSerie.SelectedItem.Value.ToString().Trim(), tseCurso.Value.ToString().Trim());
            }
            else
            {
                lblMensagem.Text = "Todos os campos são obrigatórios!";
            }
        }

        protected void LimparProximo()
        {
            ddlModalidadeProx.ClearSelection();
            ddlNivelProx.ClearSelection();
            tseCursoProx.ResetValue();
            cmbSerieProx.Items.Clear();
        }
    }
}
