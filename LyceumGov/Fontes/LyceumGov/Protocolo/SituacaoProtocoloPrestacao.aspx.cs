using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Protocolo
{
    [
          NavUrl("~/Protocolo/SituacaoProtocoloPrestacao.aspx"),
          ControlText("Situação Protocolo"),
          Title("Situação Protocolo")
      ]
    public partial class SituacaoProtocoloPrestacao : TPage
    {
        public object ListaSituacaoProtocolo()
        {
            RN.Protocolo.SituacaoProtocolo rnSituacaoProtocolo = new Techne.Lyceum.RN.Protocolo.SituacaoProtocolo();

            return rnSituacaoProtocolo.ListaSituacaoProtocolo();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object SITUACAOPROTOCOLOID) { }
        public void Delete(object SITUACAOPROTOCOLOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdSituacaoProtocolo, "Situação Protocolo");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdSituacaoProtocolo);
        }

        protected void grdSituacaoProtocolo_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdSituacaoProtocolo);
        }
		
        protected void grdSituacaoProtocolo_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdSituacaoProtocolo.Settings.ShowFilterRow = false;
        }

        protected void grdSituacaoProtocolo_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdSituacaoProtocolo.Settings.ShowFilterRow = false;
        }

        protected void grdSituacaoProtocolo_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Protocolo.Entidades.SituacaoProtocolo situacaoProtocolo = new Techne.Lyceum.RN.Protocolo.Entidades.SituacaoProtocolo();
            RN.Protocolo.SituacaoProtocolo rnSituacaoProtocoloProtocolo = new RN.Protocolo.SituacaoProtocolo();

            situacaoProtocolo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            situacaoProtocolo.Ativo = e.NewValues["ATIVO"] == null ? false : true;
            situacaoProtocolo.UsuarioId = User.Identity.Name;

            validacao = rnSituacaoProtocoloProtocolo.Valida(situacaoProtocolo, true);

            if (validacao.Valido)
            {
                rnSituacaoProtocoloProtocolo.Insere(situacaoProtocolo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdSituacaoProtocolo.DataBind();

        }

        protected void grdSituacaoProtocolo_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Protocolo.Entidades.SituacaoProtocolo situacaoProtocolo = new Techne.Lyceum.RN.Protocolo.Entidades.SituacaoProtocolo();
            RN.Protocolo.SituacaoProtocolo rnSituacaoProtocolo = new RN.Protocolo.SituacaoProtocolo();

            situacaoProtocolo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            situacaoProtocolo.Ativo = e.NewValues["ATIVO"] == null ? false : true;
            situacaoProtocolo.SituacaoProtocoloId = Convert.ToInt32(e.Keys["SITUACAOPROTOCOLOID"]);
            situacaoProtocolo.UsuarioId = User.Identity.Name;

            validacao = rnSituacaoProtocolo.Valida(situacaoProtocolo, true);

            if (validacao.Valido)
            {
                rnSituacaoProtocolo.Atualiza(situacaoProtocolo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdSituacaoProtocolo.DataBind();
        }

        protected void grdSituacaoProtocolo_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Protocolo.SituacaoProtocolo rnSituacaoProtocolo = new RN.Protocolo.SituacaoProtocolo();
            int situacaoProtocoloId = 0;

            situacaoProtocoloId = Convert.ToInt32(e.Keys["SITUACAOPROTOCOLOID"]);

            validacao = rnSituacaoProtocolo.ValidaRemocao(situacaoProtocoloId);

            if (validacao.Valido)
            {
                rnSituacaoProtocolo.Remove(situacaoProtocoloId);
                grdSituacaoProtocolo.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }           
        }
    }
}
