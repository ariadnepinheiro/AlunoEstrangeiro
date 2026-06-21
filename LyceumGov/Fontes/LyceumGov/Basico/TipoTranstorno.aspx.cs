using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Basico
{
    [
         NavUrl("~/Basico/TipoTranstorno.aspx"),
         ControlText("Motivo Baixa"),
         Title("Motivo Baixa")
     ]
    public partial class TipoTranstorno : TPage
    {
         public object Lista()
        {
            RN.RecursosHumanos.TranstornoAprendizagem rnTranstornoAprendizagem = new Techne.Lyceum.RN.RecursosHumanos.TranstornoAprendizagem();

            return rnTranstornoAprendizagem.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object TRANSTORNOAPRENDIZAGEMID) { }
        public void Delete(object TRANSTORNOAPRENDIZAGEMID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTipoTranstorno, "Tipo Transtorno Aprendizagem");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTipoTranstorno);
        }

        protected void grdTipoTranstorno_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdTipoTranstorno);
        }

        protected void grdTipoTranstorno_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTipoTranstorno.Settings.ShowFilterRow = false;
        }

        protected void grdTipoTranstorno_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdTipoTranstorno.Settings.ShowFilterRow = false;
        }

        protected void grdTipoTranstorno_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RecursosHumanos.Entidades.TranstornoAprendizagem transtornoAprendizagem = new Techne.Lyceum.RN.RecursosHumanos.Entidades.TranstornoAprendizagem();
            RN.RecursosHumanos.TranstornoAprendizagem rnTranstornoAprendizagem = new Techne.Lyceum.RN.RecursosHumanos.TranstornoAprendizagem();

            transtornoAprendizagem.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            transtornoAprendizagem.Ativo = e.NewValues["ATIVO"] == null ? false : true;
            transtornoAprendizagem.UsuarioId = User.Identity.Name;

            validacao = rnTranstornoAprendizagem.Valida(transtornoAprendizagem, true);

            if (validacao.Valido)
            {
                rnTranstornoAprendizagem.Insere(transtornoAprendizagem);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoTranstorno.DataBind();

        }

        protected void grdTipoTranstorno_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RecursosHumanos.Entidades.TranstornoAprendizagem transtornoAprendizagem = new Techne.Lyceum.RN.RecursosHumanos.Entidades.TranstornoAprendizagem();
            RN.RecursosHumanos.TranstornoAprendizagem rnTranstornoAprendizagem = new Techne.Lyceum.RN.RecursosHumanos.TranstornoAprendizagem();

            transtornoAprendizagem.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            transtornoAprendizagem.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            transtornoAprendizagem.TranstornoAprendizagemId = Convert.ToInt32(e.Keys["TRANSTORNOAPRENDIZAGEMID"]);
            transtornoAprendizagem.UsuarioId = User.Identity.Name;

            validacao = rnTranstornoAprendizagem.Valida(transtornoAprendizagem, true);

            if (validacao.Valido)
            {
                rnTranstornoAprendizagem.Atualiza(transtornoAprendizagem);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoTranstorno.DataBind();
        }

        protected void grdTipoTranstorno_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RecursosHumanos.TranstornoAprendizagem rnTranstornoAprendizagem = new Techne.Lyceum.RN.RecursosHumanos.TranstornoAprendizagem();
            int transtornoAprendizagemId = 0;

            transtornoAprendizagemId = Convert.ToInt32(e.Keys["TRANSTORNOAPRENDIZAGEMID"]);

            validacao = rnTranstornoAprendizagem.ValidaRemocao(transtornoAprendizagemId);

            if (validacao.Valido)
            {
                rnTranstornoAprendizagem.Remove(transtornoAprendizagemId);
                grdTipoTranstorno.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
