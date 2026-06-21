using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Matriculas;
using Techne.Web;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.AvaliacaoExterna
{
    [NavUrl("~/AvaliacaoExterna/TipoAvaliacao.aspx")]
    [ControlText("Objetivos da Avaliação")]
    [Title("Objetivos da Avaliação")]
    public partial class TipoAvaliacao : TPage
    {
        public object Lista()
        {
            RN.AvaliacaoExterna.TipoAvaliacao rnTipoAvaliacao = new Techne.Lyceum.RN.AvaliacaoExterna.TipoAvaliacao();

            return rnTipoAvaliacao.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object TIPOAVALIACAOID) { }
        public void Delete(object TIPOAVALIACAOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTipoAvaliacao, "Objetivos da Avaliação");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTipoAvaliacao);
        }

        protected void grdTipoAvaliacao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdTipoAvaliacao);
        }

        protected void grdTipoAvaliacao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTipoAvaliacao.Settings.ShowFilterRow = false;
        }

        protected void grdTipoAvaliacao_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdTipoAvaliacao.Settings.ShowFilterRow = false;
        }

        protected void grdTipoAvaliacao_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.AvaliacaoExterna.Entidades.TipoAvaliacao tipoAvaliacao = new Techne.Lyceum.RN.AvaliacaoExterna.Entidades.TipoAvaliacao();
            RN.AvaliacaoExterna.TipoAvaliacao rnTipoAvaliacao = new Techne.Lyceum.RN.AvaliacaoExterna.TipoAvaliacao();

            tipoAvaliacao.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipoAvaliacao.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            tipoAvaliacao.UsuarioId = User.Identity.Name;

            validacao = rnTipoAvaliacao.Valida(tipoAvaliacao, true);

            if (validacao.Valido)
            {
                rnTipoAvaliacao.Insere(tipoAvaliacao);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoAvaliacao.DataBind();

        }

        protected void grdTipoAvaliacao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.AvaliacaoExterna.Entidades.TipoAvaliacao tipoAvaliacao = new Techne.Lyceum.RN.AvaliacaoExterna.Entidades.TipoAvaliacao();
            RN.AvaliacaoExterna.TipoAvaliacao rnTipoAvaliacao = new Techne.Lyceum.RN.AvaliacaoExterna.TipoAvaliacao();

            tipoAvaliacao.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipoAvaliacao.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;            
            tipoAvaliacao.UsuarioId = User.Identity.Name;
            tipoAvaliacao.TipoAvaliacaoId = Convert.ToInt32(e.Keys["TIPOAVALIACAOID"]);


            validacao = rnTipoAvaliacao.Valida(tipoAvaliacao, true);

            if (validacao.Valido)
            {
                rnTipoAvaliacao.Atualiza(tipoAvaliacao);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoAvaliacao.DataBind();
        }

        protected void grdTipoAvaliacao_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.AvaliacaoExterna.TipoAvaliacao rnTipoAvaliacao = new Techne.Lyceum.RN.AvaliacaoExterna.TipoAvaliacao();
            int tipoAvaliacaoId = 0;

            tipoAvaliacaoId = Convert.ToInt32(e.Keys["TIPOAVALIACAOID"]);

            validacao = rnTipoAvaliacao.ValidaRemocao(tipoAvaliacaoId);

            if (validacao.Valido)
            {
                rnTipoAvaliacao.Remove(tipoAvaliacaoId);
                grdTipoAvaliacao.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
