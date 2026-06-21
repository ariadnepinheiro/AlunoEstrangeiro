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
    [NavUrl("~/AvaliacaoExterna/Prova.aspx")]
    [ControlText("Provas")]
    [Title("Provas")]
    public partial class Prova : TPage
    {
        public object ListaAvaliacao(int ano)
        {
            if (ano <= 0)
                return null;

            RN.AvaliacaoExterna.Avaliacao rnAvaliacao = new Techne.Lyceum.RN.AvaliacaoExterna.Avaliacao();
            var lista = rnAvaliacao.ListaAtivoPorAno(ano);
            DataRow newRow = lista.NewRow();
            lista.Rows.InsertAt(newRow, 0);
            return lista;
        }

        public DataTable Lista(int avaliacaoId)
        {
            if (avaliacaoId <= 0)
                return null;

            RN.AvaliacaoExterna.Prova rnProva = new Techne.Lyceum.RN.AvaliacaoExterna.Prova();
            return rnProva.ListaProvaAvaliacaoPor(avaliacaoId);
        }

        public void Insert(object AVALIACAO, object DESCRICAO, object QUANTIDADEQUESTOES, object ATIVO) { }
        public void Update(object AVALIACAO, object DESCRICAO, object QUANTIDADEQUESTOES, object ATIVO, object PROVAID) { }
        public void Delete(object PROVAID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdProva, "Provas");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaAno();
            }
        }

        private void CarregaAno()
        {
            RN.AvaliacaoExterna.Avaliacao rnAvaliacao = new Techne.Lyceum.RN.AvaliacaoExterna.Avaliacao();
            ListItem item = new ListItem(string.Empty, string.Empty);
            ddlAno.Items.Clear();
            ddlAno.DataSource = rnAvaliacao.ListaAnos();
            ddlAno.DataBind();
            ddlAno.Items.Insert(0, item);
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdProva);
            plaGrid.Visible = ddlAvaliacao.SelectedIndex > 0;
            plaZero.Visible = !plaGrid.Visible;
        }       

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as DropDownList;
            ddlAvaliacao.SelectedIndex = -1;
        }      

        protected void grdProva_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdProva);
        }

        protected void grdProva_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (grdProva.IsNewRowEditing)
            {
                if (e.Column.FieldName == "AVALIACAO")
                {
                    e.Editor.Value = ddlAvaliacao.SelectedItem.Text;
                }              
            }           
        }        

        protected void grdProva_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdProva.Settings.ShowFilterRow = false;
        }

        protected void grdProva_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdProva.Settings.ShowFilterRow = false;
        }

        protected void grdProva_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.AvaliacaoExterna.Entidades.Prova prova = new Techne.Lyceum.RN.AvaliacaoExterna.Entidades.Prova();
            RN.AvaliacaoExterna.Prova rnProva = new Techne.Lyceum.RN.AvaliacaoExterna.Prova();

            prova.AvaliacaoId = Convert.ToInt32(ddlAvaliacao.SelectedValue);
            prova.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            prova.QuantidadeQuestoes = e.NewValues["QUANTIDADEQUESTOES"] != null ? Convert.ToInt32(e.NewValues["QUANTIDADEQUESTOES"]) : 0;
            prova.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            prova.DataCadastro = DateTime.Now.Date;
            prova.UsuarioId = User.Identity.Name;

            validacao = rnProva.Valida(prova, true);

            if (validacao.Valido)
            {
                rnProva.Insere(prova);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdProva.DataBind();
        }

        protected void grdProva_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.AvaliacaoExterna.Entidades.Prova prova = new Techne.Lyceum.RN.AvaliacaoExterna.Entidades.Prova();
            RN.AvaliacaoExterna.Prova rnProva = new Techne.Lyceum.RN.AvaliacaoExterna.Prova();

            prova.ProvaId = Convert.ToInt32(e.Keys["PROVAID"]);
            prova.AvaliacaoId = Convert.ToInt32(ddlAvaliacao.SelectedValue);
            prova.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            prova.QuantidadeQuestoes = e.NewValues["QUANTIDADEQUESTOES"] != null ? Convert.ToInt32(e.NewValues["QUANTIDADEQUESTOES"]) : 0;
            prova.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            prova.UsuarioId = User.Identity.Name;

            validacao = rnProva.Valida(prova, false);

            if (validacao.Valido)
            {
                rnProva.Atualiza(prova);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdProva.DataBind();
        }

        protected void grdProva_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.AvaliacaoExterna.Prova rnProva = new Techne.Lyceum.RN.AvaliacaoExterna.Prova();
            int PROVAID = e.Keys["PROVAID"] == null ? 0 : Convert.ToInt32(e.Keys["PROVAID"]);
            validacao = rnProva.ValidaRemocao(PROVAID);

            if (validacao.Valido)
            {
                rnProva.Remove(PROVAID);
                grdProva.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem);
            }
        }
    }
}