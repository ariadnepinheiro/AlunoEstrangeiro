using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.PrestacaoContas
{
            [
         NavUrl("~/PrestacaoContas/TipoTransporte.aspx"),
         ControlText("TipoTransporte"),
         Title("Tipo de Transporte")
     ]
    public partial class TipoTransporte : TPage
    {
        public object Lista()
        {
            RN.PrestacaoContas.TipoTransporte rnTipoTransporte = new Techne.Lyceum.RN.PrestacaoContas.TipoTransporte();

            return rnTipoTransporte.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object TIPOTRANSPORTEID) { }
        public void Delete(object TIPOTRANSPORTEID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTipoTransporte, "Tipo de Transporte");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTipoTransporte);
        }

        protected void grdTipoTransporte_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdTipoTransporte);
        }		
        
        protected void grdTipoTransporte_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTipoTransporte.Settings.ShowFilterRow = false;
        }

        protected void grdTipoTransporte_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdTipoTransporte.Settings.ShowFilterRow = false;
        }

        protected void grdTipoTransporte_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.TipoTransporte tipo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.TipoTransporte();
            RN.PrestacaoContas.TipoTransporte rnTipoTransporte = new RN.PrestacaoContas.TipoTransporte();

            tipo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            tipo.UsuarioId = User.Identity.Name;

            validacao = rnTipoTransporte.Valida(tipo, true);

            if (validacao.Valido)
            {
                rnTipoTransporte.Insere(tipo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoTransporte.DataBind();

        }

        protected void grdTipoTransporte_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.TipoTransporte tipo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.TipoTransporte();
            RN.PrestacaoContas.TipoTransporte rnTipoTransporte = new RN.PrestacaoContas.TipoTransporte();

            tipo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            tipo.TipoTransporteId = Convert.ToInt32(e.Keys["TIPOTRANSPORTEID"]);
            tipo.UsuarioId = User.Identity.Name;

            validacao = rnTipoTransporte.Valida(tipo, true);

            if (validacao.Valido)
            {
                rnTipoTransporte.Atualiza(tipo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoTransporte.DataBind();
        }

        protected void grdTipoTransporte_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.TipoTransporte rnTipoTransporte = new RN.PrestacaoContas.TipoTransporte();
            int tipoTransporteId = 0;

            tipoTransporteId = Convert.ToInt32(e.Keys["TIPOTRANSPORTEID"]);

            validacao = rnTipoTransporte.ValidaRemocao(tipoTransporteId);

            if (validacao.Valido)
            {
                rnTipoTransporte.Remove(tipoTransporteId);
                grdTipoTransporte.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

      
       
    }
}
