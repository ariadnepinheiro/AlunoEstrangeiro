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

namespace Techne.Lyceum.Net.AvaliacaoExterna
{
    [NavUrl("~/AvaliacaoExterna/Habilidade.aspx")]
    [ControlText("Habilidades do Componente")]
    [Title("Habilidades do Componente")]
    public partial class Habilidade : TPage
    {
        public readonly RN.AvaliacaoExterna.Habilidade rnHabilidade;
        public readonly RN.AvaliacaoExterna.Componente rnComponente;

        public Habilidade()
        {
            rnHabilidade = new Techne.Lyceum.RN.AvaliacaoExterna.Habilidade();
            rnComponente = new Techne.Lyceum.RN.AvaliacaoExterna.Componente();
        }

        public int ComponenteId
        {
            get
            {
                int componenteId;
                string key = this.QueryStringDecodificada["componenteId"];
                int.TryParse(key ?? "0", out componenteId);
                return componenteId;
            }
        }

        public object ListaComponente()
        {
            var lista = rnComponente.ListaAtivo();
            DataRow newRow = lista.NewRow();
            lista.Rows.InsertAt(newRow, 0);
            return lista;
        }

        public object Lista(int componenteId)
        {
            return rnHabilidade.ListaPorComponente(componenteId);
        }
        public void Insert(object CODIGO, object DESCRICAO) { }
        public void Update(object CODIGO, object DESCRICAO, object HABILIDADEID) { }
        public void Delete(object HABILIDADEID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdHabilidade, "Habilidades do Componente");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcessoGrid();
            plaGrid.Visible = ddlComponente.SelectedIndex > 0;
            plaZero.Visible = !plaGrid.Visible;
        }

        protected void ControlaAcessoGrid()
        {
            if (grdHabilidade != null)
            {
                if (!Permission.AllowDelete && !Permission.AllowInsert && !Permission.AllowUpdate)
                {
                    grdHabilidade.Columns[""].Visible = false;
                }                
            }

            ControlaAcesso(grdHabilidade);
        }

        protected void ddlComponente_DataBound(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            if (ComponenteId > 0)
            {
                ddlComponente.SelectedValue = ComponenteId.ToString();
            }
        }

        protected void grdHabilidade_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdHabilidade);
        }

        protected void grdHabilidade_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (this.grdHabilidade.IsNewRowEditing)
            {
                if (e.Column.FieldName == "HABILIDADEID")
                {
                    e.Editor.ReadOnly = true;
                }
                if (e.Column.FieldName == "CURSOID")
                {
                    e.Editor.ReadOnly = false;
                }
                if (e.Column.FieldName == "SERIE")
                {
                    e.Editor.ReadOnly = false;
                }
            }
            else if (this.grdHabilidade.IsEditing)
            {
                if (e.Column.FieldName == "HABILIDADEID")
                {
                    e.Editor.Enabled = false;
                }
                if (e.Column.FieldName == "CURSOID")
                {
                    e.Editor.ReadOnly = true;
                }
                if (e.Column.FieldName == "SERIE")
                {
                    e.Editor.ReadOnly = true;
                }
            }
        }

        protected void grdHabilidade_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdHabilidade.Settings.ShowFilterRow = false;
        }

        protected void grdHabilidade_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdHabilidade.Settings.ShowFilterRow = false;
        }

        protected void grdHabilidade_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.AvaliacaoExterna.Entidades.Habilidade habilidade = new Techne.Lyceum.RN.AvaliacaoExterna.Entidades.Habilidade();
            RN.AvaliacaoExterna.Habilidade rnHabilidade = new Techne.Lyceum.RN.AvaliacaoExterna.Habilidade();

            habilidade.ComponenteId = ddlComponente.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToInt32(ddlComponente.SelectedValue);
            habilidade.Codigo = e.NewValues["CODIGO"] != null ? e.NewValues["CODIGO"].ToString().Trim().ToUpper() : null;
            habilidade.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            habilidade.Ativo = true;
            habilidade.UsuarioID = User.Identity.Name;

            validacao = rnHabilidade.Valida(habilidade, true);

            if (validacao.Valido)
            {
                rnHabilidade.Insere(habilidade);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdHabilidade.DataBind();

        }

        protected void grdHabilidade_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.AvaliacaoExterna.Entidades.Habilidade habilidade = new Techne.Lyceum.RN.AvaliacaoExterna.Entidades.Habilidade();
            RN.AvaliacaoExterna.Habilidade rnHabilidade = new Techne.Lyceum.RN.AvaliacaoExterna.Habilidade();

            habilidade.ComponenteId = ddlComponente.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToInt32(ddlComponente.SelectedValue);
            habilidade.Codigo = e.NewValues["CODIGO"] != null ? e.NewValues["CODIGO"].ToString().Trim().ToUpper() : null;
            habilidade.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            habilidade.Ativo = true;

            habilidade.UsuarioID = User.Identity.Name;
            habilidade.HabilidadeId = Convert.ToString(e.Keys["HABILIDADEID"]).IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToInt32(e.Keys["HABILIDADEID"]);

            validacao = rnHabilidade.Valida(habilidade, true);

            if (validacao.Valido)
            {
                rnHabilidade.Atualiza(habilidade);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdHabilidade.DataBind();
        }

        protected void grdHabilidade_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.AvaliacaoExterna.Habilidade rnHabilidade = new Techne.Lyceum.RN.AvaliacaoExterna.Habilidade();
            int habilidadeId = 0;

            habilidadeId = Convert.ToInt32(e.Keys["HABILIDADEID"]);

            validacao = rnHabilidade.ValidaRemocao(habilidadeId);

            if (validacao.Valido)
            {
                rnHabilidade.Remove(habilidadeId);
                grdHabilidade.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}