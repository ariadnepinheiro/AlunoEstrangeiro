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
using System.Web.UI.HtmlControls;

namespace Techne.Lyceum.Net.AvaliacaoExterna
{
    [NavUrl("~/AvaliacaoExterna/Avaliacao.aspx")]
    [ControlText("Avaliações")]
    [Title("Avaliações")]
    public partial class Avaliacao : TPage
    {
        public readonly RN.AvaliacaoExterna.Avaliacao rnAvaliacao;
        public readonly RN.AvaliacaoExterna.TipoAvaliacao rnTipoAvaliacao;

        public Avaliacao()
        {
            rnAvaliacao = new Techne.Lyceum.RN.AvaliacaoExterna.Avaliacao();
            rnTipoAvaliacao = new Techne.Lyceum.RN.AvaliacaoExterna.TipoAvaliacao();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdAvaliacao, "Avaliações");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdAvaliacao);
            AcessoGrid();
        }

        protected void AcessoGrid()
        {
            if (grdAvaliacao != null)
            {
                if (!Permission.AllowDelete && !Permission.AllowInsert && !Permission.AllowUpdate)
                {
                    grdAvaliacao.Columns[""].Visible = false;
                }               
            }
        }

        public object ListaTipoAvaliacao()
        {
            var listaAtivo = rnTipoAvaliacao.ListaAtivo();
            DataRow newRow = listaAtivo.NewRow();
            listaAtivo.Rows.InsertAt(newRow, 0);
            return listaAtivo;
        }

        public object Lista()
        {
            return rnAvaliacao.Lista();
        }
        public void Insert(object DESCRICAO, object ANO, object TIPOAVALIACAOID, object ATIVO) { }
        public void Update(object DESCRICAO, object ANO, object TIPOAVALIACAOID, object ATIVO, object AVALIACAOID) { }
        public void Delete(object AVALIACAOID) { }

        protected void grdAvaliacao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdAvaliacao);
            AcessoGrid();
        }        

        protected void grdAvaliacao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdAvaliacao.Settings.ShowFilterRow = false;
        }

        protected void grdAvaliacao_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdAvaliacao.Settings.ShowFilterRow = false;
        }

        protected void grdAvaliacao_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.AvaliacaoExterna.Entidades.Avaliacao avaliacao = new Techne.Lyceum.RN.AvaliacaoExterna.Entidades.Avaliacao();
            RN.AvaliacaoExterna.Avaliacao rnAvaliacao = new Techne.Lyceum.RN.AvaliacaoExterna.Avaliacao();

            avaliacao.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            avaliacao.Ano = string.IsNullOrEmpty(e.NewValues["ANO"].ToString()) ? -1 : Convert.ToInt32(e.NewValues["ANO"]);
            avaliacao.TipoAvaliacaoID = string.IsNullOrEmpty(e.NewValues["TIPOAVALIACAOID"].ToString()) ? -1 : Convert.ToInt32(e.NewValues["TIPOAVALIACAOID"]);
            avaliacao.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            avaliacao.UsuarioID = User.Identity.Name;

            validacao = rnAvaliacao.Valida(avaliacao, true);

            if (validacao.Valido)
            {
                rnAvaliacao.Insere(avaliacao);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdAvaliacao.DataBind();
        }

        protected void grdAvaliacao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.AvaliacaoExterna.Entidades.Avaliacao avaliacao = new Techne.Lyceum.RN.AvaliacaoExterna.Entidades.Avaliacao();
            RN.AvaliacaoExterna.Avaliacao rnAvaliacao = new Techne.Lyceum.RN.AvaliacaoExterna.Avaliacao();

            avaliacao.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            avaliacao.Ano = string.IsNullOrEmpty(e.NewValues["ANO"].ToString()) ? -1 : Convert.ToInt32(e.NewValues["ANO"]);
            avaliacao.TipoAvaliacaoID = string.IsNullOrEmpty(e.NewValues["TIPOAVALIACAOID"].ToString()) ? -1 : Convert.ToInt32(e.NewValues["TIPOAVALIACAOID"]);
            avaliacao.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            avaliacao.UsuarioID = User.Identity.Name;
            avaliacao.AvaliacaoId = Convert.ToInt32(e.Keys["AVALIACAOID"]);

            validacao = rnAvaliacao.Valida(avaliacao, false);

            if (validacao.Valido)
            {
                rnAvaliacao.Altera(avaliacao);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdAvaliacao.DataBind();
        }

        protected void grdAvaliacao_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.AvaliacaoExterna.Avaliacao rnEtapa = new Techne.Lyceum.RN.AvaliacaoExterna.Avaliacao();
            int avaliacaoId = 0;

            avaliacaoId = Convert.ToInt32(e.Keys["AVALIACAOID"]);

            validacao = rnEtapa.ValidaRemocao(avaliacaoId);

            if (validacao.Valido)
            {
                rnAvaliacao.Exclui(avaliacaoId);
                grdAvaliacao.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}