using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxGridView;


namespace Techne.Lyceum.Net.Patrimonio
{
    [
     NavUrl("~/Patrimonio/EstadoConservacao.aspx"),
     ControlText("Estado Conservação"),
     Title("Estado Conservação")
 ]
    public partial class EstadoConservacao : TPage
    {
        public object Lista()
        {
            RN.Patrimonio.EstadoConservacao rnEstadoConservacao = new Techne.Lyceum.RN.Patrimonio.EstadoConservacao();

            return rnEstadoConservacao.Lista();
        }

        public void Insert(object CONCEITO,object PONTUACAO, object ATIVO) { }
        public void Update(object CONCEITO, object PONTUACAO, object ATIVO, object ESTADOCONSERVACAOID) { }
        public void Delete(object ESTADOCONSERVACAOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdEstadoConservacao, "Estado de Conservação");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdEstadoConservacao);
        }

        protected void grdEstadoConservacao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdEstadoConservacao);
        }

        protected void grdEstadoConservacao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdEstadoConservacao.Settings.ShowFilterRow = false;
        }

        protected void grdEstadoConservacao_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdEstadoConservacao.Settings.ShowFilterRow = false;
        }

        protected void grdEstadoConservacao_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Patrimonio.Entidades.EstadoConservacao EstadoConservacao = new Techne.Lyceum.RN.Patrimonio.Entidades.EstadoConservacao();
            RN.Patrimonio.EstadoConservacao rnEstadoConservacao = new Techne.Lyceum.RN.Patrimonio.EstadoConservacao();

            EstadoConservacao.Conceito = e.NewValues["CONCEITO"] != null ? e.NewValues["CONCEITO"].ToString().Trim().ToUpper() : null;
            EstadoConservacao.Pontuacao = e.NewValues["PONTUACAO"] != null ? Convert.ToInt32(e.NewValues["PONTUACAO"]) : -1;
            EstadoConservacao.Ativo = e.NewValues["ATIVO"] == null ? false : true;
            EstadoConservacao.UsuarioId = User.Identity.Name;

            validacao = rnEstadoConservacao.Valida(EstadoConservacao, true);

            if (validacao.Valido)
            {
                rnEstadoConservacao.Insere(EstadoConservacao);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdEstadoConservacao.DataBind();

        }

        protected void grdEstadoConservacao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Patrimonio.Entidades.EstadoConservacao EstadoConservacao = new Techne.Lyceum.RN.Patrimonio.Entidades.EstadoConservacao();
            RN.Patrimonio.EstadoConservacao rnEstadoConservacao = new Techne.Lyceum.RN.Patrimonio.EstadoConservacao();

            EstadoConservacao.Conceito = e.NewValues["CONCEITO"] != null ? e.NewValues["CONCEITO"].ToString().Trim().ToUpper() : null;
            EstadoConservacao.Pontuacao = e.NewValues["PONTUACAO"] != null ? Convert.ToInt32(e.NewValues["PONTUACAO"]) : -1;
            EstadoConservacao.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            EstadoConservacao.EstadoConservacaoId = Convert.ToInt32(e.Keys["ESTADOCONSERVACAOID"]);
            EstadoConservacao.UsuarioId = User.Identity.Name;

            validacao = rnEstadoConservacao.Valida(EstadoConservacao, true);

            if (validacao.Valido)
            {
                rnEstadoConservacao.Atualiza(EstadoConservacao);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdEstadoConservacao.DataBind();
        }

        protected void grdEstadoConservacao_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Patrimonio.EstadoConservacao rnEstadoConservacao = new Techne.Lyceum.RN.Patrimonio.EstadoConservacao();
            int EstadoConservacaoId = 0;

            EstadoConservacaoId = Convert.ToInt32(e.Keys["ESTADOCONSERVACAOID"]);

            validacao = rnEstadoConservacao.ValidaRemocao(EstadoConservacaoId);

            if (validacao.Valido)
            {
                rnEstadoConservacao.Remove(EstadoConservacaoId);
                grdEstadoConservacao.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
