using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;


namespace Techne.Lyceum.Net.Interconectividade
{
    [
         NavUrl("~/Interconectividade/TipoLink.aspx"),
         ControlText("TipoLink"),
         Title("Tipo Link")
     ]
    public partial class TipoLink : TPage
    {
        public object Lista()
        {
            RN.FiscalizacaoLink.TipoLink rnTipoLink = new Techne.Lyceum.RN.FiscalizacaoLink.TipoLink();

            return rnTipoLink.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object TIPOLINKID) { }
        public void Delete(object TIPOLINKID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTipoLink, "Tipo Link");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTipoLink);
        }

        protected void grdTipoLink_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTipoLink.Settings.ShowFilterRow = false;
        }

        protected void grdTipoLink_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdTipoLink.Settings.ShowFilterRow = false;
        }

        protected void grdTipoLink_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.FiscalizacaoLink.Entidades.TipoLink tipoLink = new Techne.Lyceum.RN.FiscalizacaoLink.Entidades.TipoLink();
            RN.FiscalizacaoLink.TipoLink rnTipoLink = new RN.FiscalizacaoLink.TipoLink();

            tipoLink.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipoLink.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            tipoLink.UsuarioId = User.Identity.Name;

            validacao = rnTipoLink.Valida(tipoLink, true);

            if (validacao.Valido)
            {
                rnTipoLink.Insere(tipoLink);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoLink.DataBind();

        }

        protected void grdTipoLink_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.FiscalizacaoLink.Entidades.TipoLink tipoLink = new Techne.Lyceum.RN.FiscalizacaoLink.Entidades.TipoLink();
            RN.FiscalizacaoLink.TipoLink rnTipoLink = new RN.FiscalizacaoLink.TipoLink();

            tipoLink.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipoLink.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            tipoLink.TipoLinkId = Convert.ToInt32(e.Keys["TIPOLINKID"]);
            tipoLink.UsuarioId = User.Identity.Name;

            validacao = rnTipoLink.Valida(tipoLink, true);

            if (validacao.Valido)
            {
                rnTipoLink.Atualiza(tipoLink);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoLink.DataBind();
        }

        protected void grdTipoLink_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.FiscalizacaoLink.TipoLink rnTipoLink = new RN.FiscalizacaoLink.TipoLink();
            int tipoLinkId = 0;

            tipoLinkId = Convert.ToInt32(e.Keys["TIPOLINKID"]);

            validacao = rnTipoLink.ValidaRemocao(tipoLinkId);

            if (validacao.Valido)
            {
                rnTipoLink.Remove(tipoLinkId);
                grdTipoLink.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
