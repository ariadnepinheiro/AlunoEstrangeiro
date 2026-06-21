using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using System.Data;
using DevExpress.Web.ASPxGridView;
using Techne.Web;
using Techne.Data;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxCallback;
using Seeduc.Infra.Helpers;
using Techne.Controls;

namespace Techne.Lyceum.Net.AvaliacaoExterna
{
    [NavUrl("~/AvaliacaoExterna/Etapa.aspx")]
    [ControlText("Etapas")]
    [Title("Etapas")]
    public partial class Etapa : TPage
    {
        ASPxComboBox cmbCurso = null;
        ASPxComboBox cmbSerie = null;

        public readonly RN.AvaliacaoExterna.Etapa rnEtapa;
        public readonly RN.AvaliacaoExterna.Avaliacao rnAvaliacao;
        public readonly RN.Curso rnCurso;
        public readonly RN.Serie rnSerie;
        RN.AvaliacaoExterna.Prova rnProva;

        public Etapa()
        {
            rnEtapa = new Techne.Lyceum.RN.AvaliacaoExterna.Etapa();
            rnAvaliacao = new Techne.Lyceum.RN.AvaliacaoExterna.Avaliacao();
            rnCurso = new Techne.Lyceum.RN.Curso();
            rnSerie = new Techne.Lyceum.RN.Serie();
            rnProva = new Techne.Lyceum.RN.AvaliacaoExterna.Prova();
        }       
              

        public object ListaAnos()
        {
            var lista = rnAvaliacao.ListaAnos();
            DataRow newRow = lista.NewRow();
            lista.Rows.InsertAt(newRow, 0);
            return lista;
        }

        public object ListaAvaliacao(int ano)
        {
            var lista = rnAvaliacao.ListaAtivoPorAno(ano);
            DataRow newRow = lista.NewRow();
            lista.Rows.InsertAt(newRow, 0);
            return lista;
        }

        public object ListaProva(int avaliacaoId)
        {
            DataTable lista = rnProva.ListaAtivoPor(avaliacaoId);
            DataRow newRow = lista.NewRow();
            lista.Rows.InsertAt(newRow, 0);
            return lista;
        }

        public object ListaCurso(int ano)
        {
            var consultar = rnCurso.ListaPorAno(ano);
            DataRow newRow = consultar.NewRow();
            consultar.Rows.InsertAt(newRow, 0);

            //removido curso 9999.99 do combo de cursos
            //porque a query que o retorna ficou muito
            //pesada, e certamente o usuário não o usará
            DataView dv = consultar.AsDataView();
            dv.RowFilter = "CURSO <> '9999.99'";
            consultar = dv.ToTable();

            return consultar;
        }

        public object ListaSerie(string curso)
        {
            var obtemSeriesPor = rnSerie.ObtemSeriesPor(curso);
            return obtemSeriesPor;
        }

        public object ListaSeries()
        {
            return rnEtapa.ListaSeries();
        }

        public DataTable Lista(int provaId)
        {
            return rnEtapa.ListaPorProva(provaId);
        }

        public void Insert(object PROVA, object CURSO, object INICIOREALIZACAO, object FIMREALIZACAO, object INICIOTRANSCRICAO, object FIMTRANSCRICAO, object ATIVO) { }
        public void Update(object PROVA, object CURSO, object INICIOREALIZACAO, object FIMREALIZACAO, object INICIOTRANSCRICAO, object FIMTRANSCRICAO, object ATIVO, object ETAPAID) { }
        public void Delete(object ETAPAID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdEtapa, "Etapas");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdEtapa);
            plaGrid.Visible = ddlProva.SelectedIndex > 0;
            plaZero.Visible = !plaGrid.Visible;
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlAvaliacao.SelectedIndex = -1;
            ddlProva.SelectedIndex = -1;
        }

        protected void ddlAvaliacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlProva.SelectedIndex = -1;
        }         

        protected void grdEtapa_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;

            if (e.CellType == GridViewTableCommandCellType.Filter)
            {
                e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                return;
            }            
        }       

        protected void grdEtapa_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdEtapa);
        }

        protected void grdEtapa_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (grdEtapa.IsNewRowEditing)
            {
                if (e.Column.FieldName == "PROVA")
                {
                    e.Editor.Value = ddlProva.SelectedItem.Text;
                }
            }

            if (grdEtapa.IsEditing)
            {
                if (e.Column.FieldName == "CURSO")
                {
                    cmbCurso = e.Editor as ASPxComboBox;
                    if (!grdEtapa.IsNewRowEditing)
                    {
                        e.Editor.ReadOnly = true;
                        cmbCurso.ReadOnly = true;
                    }
                }


                if (e.Column.FieldName == "SERIE")
                {
                    cmbSerie = e.Editor as ASPxComboBox;
                    if (!grdEtapa.IsNewRowEditing)
                    {
                        e.Editor.ReadOnly = true;
                        cmbSerie.ReadOnly = true;
                    }                  
                }
            }
        }

        protected void cmbSerie_Load(object sender, EventArgs e)
        {
            ASPxComboBox cmbSerie = sender as ASPxComboBox;

            if (cmbCurso.Value == null)
                return;

            string curso = cmbCurso.Value.ToString();

            cmbSerie.Items.Clear();
            cmbSerie.DataSource = ListaSerie(curso);
            cmbSerie.DataBind();

            if (grdEtapa.IsEditing)
            {
                try
                {
                    int serie = Convert.ToInt32(grdEtapa.GetDataRow(grdEtapa.EditingRowVisibleIndex)["SERIE"]);
                    cmbSerie.Items.FindByValue(serie.ToString()).Selected = true;
                    if (!grdEtapa.IsNewRowEditing)
                    {
                        cmbSerie.ReadOnly = true;
                    }
                }
                catch
                {
                }
            }
        }

        protected void grdEtapa_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdEtapa.Settings.ShowFilterRow = false;
        }

        protected void grdEtapa_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdEtapa.Settings.ShowFilterRow = false;
        }

        protected void grdEtapa_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            cmbSerie = grdEtapa.FindEditRowCellTemplateControl((GridViewDataColumn)grdEtapa.Columns["SERIE"], "cmbSerie") as ASPxComboBox;

            ValidacaoDados validacao = new ValidacaoDados();
            RN.AvaliacaoExterna.Entidades.Etapa etapa = new Techne.Lyceum.RN.AvaliacaoExterna.Entidades.Etapa();
            RN.AvaliacaoExterna.Etapa rnEtapa = new Techne.Lyceum.RN.AvaliacaoExterna.Etapa();

            etapa.ProvaId = Convert.ToInt32(ddlProva.SelectedValue);
            etapa.Curso = e.NewValues["CURSO"] != null ? e.NewValues["CURSO"].ToString().Trim().ToUpper() : null;
            etapa.Serie = cmbSerie == null || cmbSerie.SelectedIndex == -1 ? -1 : Convert.ToInt32(cmbSerie.SelectedItem.Value);
            etapa.InicioRealizacao = !string.IsNullOrEmpty((e.NewValues["INICIOREALIZACAO"] ?? "").ToString()) ? Convert.ToDateTime(e.NewValues["INICIOREALIZACAO"]) : DateTime.MinValue;
            etapa.FimRealizacao = !string.IsNullOrEmpty((e.NewValues["FIMREALIZACAO"] ?? "").ToString()) ? Convert.ToDateTime(e.NewValues["FIMREALIZACAO"]) : DateTime.MinValue;
            etapa.InicioTranscricao = !string.IsNullOrEmpty((e.NewValues["INICIOTRANSCRICAO"] ?? "").ToString()) ? Convert.ToDateTime(e.NewValues["INICIOTRANSCRICAO"]) : DateTime.MinValue;
            etapa.FimTranscricao = !string.IsNullOrEmpty((e.NewValues["FIMTRANSCRICAO"] ?? "").ToString()) ? Convert.ToDateTime(e.NewValues["FIMTRANSCRICAO"]) : DateTime.MinValue;
            etapa.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;

            etapa.UsuarioID = User.Identity.Name;

            validacao = rnEtapa.Valida(etapa, true);

            if (validacao.Valido)
            {
                rnEtapa.Insere(etapa);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdEtapa.DataBind();
        }

        protected void grdEtapa_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            cmbSerie = grdEtapa.FindEditRowCellTemplateControl((GridViewDataColumn)grdEtapa.Columns["SERIE"], "cmbSerie") as ASPxComboBox;
            int serieOldValue = Convert.ToInt32(grdEtapa.GetDataRow(grdEtapa.EditingRowVisibleIndex)["SERIE"]);
            int serieNewValue = cmbSerie == null || cmbSerie.SelectedIndex == -1 ? -1 : Convert.ToInt32(cmbSerie.SelectedItem.Value);

            ValidacaoDados validacao = new ValidacaoDados();
            RN.AvaliacaoExterna.Entidades.Etapa etapa = new Techne.Lyceum.RN.AvaliacaoExterna.Entidades.Etapa();
            RN.AvaliacaoExterna.Etapa rnEtapa = new Techne.Lyceum.RN.AvaliacaoExterna.Etapa();

            etapa.ProvaId = Convert.ToInt32(ddlProva.SelectedValue);
            etapa.Curso = e.NewValues["CURSO"] != null ? e.NewValues["CURSO"].ToString().Trim().ToUpper() : null;
            etapa.Serie = serieNewValue == -1 ? serieOldValue : serieNewValue;
            etapa.InicioRealizacao = !string.IsNullOrEmpty((e.NewValues["INICIOREALIZACAO"] ?? "").ToString()) ? Convert.ToDateTime(e.NewValues["INICIOREALIZACAO"]) : DateTime.MinValue;
            etapa.FimRealizacao = !string.IsNullOrEmpty((e.NewValues["FIMREALIZACAO"] ?? "").ToString()) ? Convert.ToDateTime(e.NewValues["FIMREALIZACAO"]) : DateTime.MinValue;
            etapa.InicioTranscricao = string.IsNullOrEmpty(e.NewValues["INICIOTRANSCRICAO"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(e.NewValues["INICIOTRANSCRICAO"]);
            etapa.FimTranscricao = string.IsNullOrEmpty(e.NewValues["FIMTRANSCRICAO"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(e.NewValues["FIMTRANSCRICAO"]);
            etapa.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;

            etapa.UsuarioID = User.Identity.Name;
            etapa.EtapaId = Convert.ToInt32(e.Keys["ETAPAID"]);

            validacao = rnEtapa.Valida(etapa, false);

            if (validacao.Valido)
            {
                rnEtapa.Atualiza(etapa);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdEtapa.DataBind();
        }

        protected void grdEtapa_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.AvaliacaoExterna.Etapa rnEtapa = new Techne.Lyceum.RN.AvaliacaoExterna.Etapa();
            int etapaId = 0;

            etapaId = e.Keys["ETAPAID"] == null ? 0 : Convert.ToInt32(e.Keys["ETAPAID"]);

            validacao = rnEtapa.ValidaRemocao(etapaId);

            if (validacao.Valido)
            {
                rnEtapa.Remove(etapaId);
                grdEtapa.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem);
            }
        }
    }
}