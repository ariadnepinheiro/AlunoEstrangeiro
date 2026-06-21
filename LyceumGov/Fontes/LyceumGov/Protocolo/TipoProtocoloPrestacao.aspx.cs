using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Protocolo
{
    [
      NavUrl("~/Protocolo/TipoProtocoloPrestacao.aspx"),
      ControlText("Tipo de Programa"),
      Title("Tipo de Programa")
  ]
    public partial class TipoProtocoloPrestacao : TPage
    {
        public object ListaTipoProtocolo()
        {
            RN.Protocolo.TipoProtocolo rnTipoProtocolo = new Techne.Lyceum.RN.Protocolo.TipoProtocolo();

            return rnTipoProtocolo.ListaTipoProtocolo();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object TIPOPROTOCOLOID) { }
        public void Delete(object TIPOPROTOCOLOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTipoProtocolo, "Tipo Protocolo");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTipoProtocolo);
        }

        protected void grdTipoProtocolo_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdTipoProtocolo);
        }		

        protected void grdTipoProtocolo_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTipoProtocolo.Settings.ShowFilterRow = false;
        }

        protected void grdTipoProtocolo_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdTipoProtocolo.Settings.ShowFilterRow = false;
        }

        protected void grdTipoProtocolo_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Protocolo.Entidades.TipoProtocolo TipoProtocolo = new Techne.Lyceum.RN.Protocolo.Entidades.TipoProtocolo();
            RN.Protocolo.TipoProtocolo rnTipoProtocolo = new RN.Protocolo.TipoProtocolo();

            TipoProtocolo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            TipoProtocolo.Ativo = e.NewValues["ATIVO"] == null ? false : true;
            TipoProtocolo.UsuarioId = User.Identity.Name;

            validacao = rnTipoProtocolo.Valida(TipoProtocolo, true);

            if (validacao.Valido)
            {
                rnTipoProtocolo.Insere(TipoProtocolo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoProtocolo.DataBind();

        }

        protected void grdTipoProtocolo_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Protocolo.Entidades.TipoProtocolo tipoProtocolo = new Techne.Lyceum.RN.Protocolo.Entidades.TipoProtocolo();
            RN.Protocolo.TipoProtocolo rnTipoProtocolo = new RN.Protocolo.TipoProtocolo();

            tipoProtocolo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipoProtocolo.Ativo = e.NewValues["ATIVO"] == null ? false : true;
            tipoProtocolo.TipoProtocoloId = Convert.ToInt32(e.Keys["TIPOPROTOCOLOID"]);
            tipoProtocolo.UsuarioId = User.Identity.Name;

            validacao = rnTipoProtocolo.Valida(tipoProtocolo, true);

            if (validacao.Valido)
            {
                rnTipoProtocolo.Atualiza(tipoProtocolo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoProtocolo.DataBind();
        }

        protected void grdTipoProtocolo_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Protocolo.TipoProtocolo rnTipoProtocolo = new RN.Protocolo.TipoProtocolo();
            int tipoProtocoloId = 0;

            tipoProtocoloId = Convert.ToInt32(e.Keys["TIPOPROTOCOLOID"]);

            validacao = rnTipoProtocolo.ValidaRemocao(tipoProtocoloId);

            if (validacao.Valido)
            {
                rnTipoProtocolo.Remove(tipoProtocoloId);
                grdTipoProtocolo.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
