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
         NavUrl("~/PrestacaoContas/MotivoExigenciaEvento.aspx"),
         ControlText("MotivoExigenciaDespesa"),
         Title("Motivo Exigência Despesa")
     ]
    public partial class MotivoExigenciaEvento : TPage
    {
        public object Lista()
        {
            RN.PrestacaoContas.MotivoExigenciaEvento rnMotivoExigenciaEvento = new Techne.Lyceum.RN.PrestacaoContas.MotivoExigenciaEvento();

            return rnMotivoExigenciaEvento.Lista();

        }

        public void Insert(object DESCRICAO, object RESSARCIMENTO, object ATIVO) { }
        public void Update(object DESCRICAO, object RESSARCIMENTO, object ATIVO, object MOTIVOEXIGENCIAEVENTOID) { }
        public void Delete(object MOTIVOEXIGENCIAEVENTOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdMotivoExigenciaEvento, "Motivo Exigência Despesa");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdMotivoExigenciaEvento);
        }

        protected void grdMotivoExigenciaEvento_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdMotivoExigenciaEvento);
        }		
        
        protected void grdMotivoExigenciaEvento_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdMotivoExigenciaEvento.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoExigenciaEvento_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdMotivoExigenciaEvento.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoExigenciaEvento_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.MotivoExigenciaEvento motivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.MotivoExigenciaEvento();
            RN.PrestacaoContas.MotivoExigenciaEvento rnMotivoExigenciaEvento = new RN.PrestacaoContas.MotivoExigenciaEvento();

            motivo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivo.Ressarcimento = (e.NewValues["RESSARCIMENTO"] == null || Convert.ToBoolean(e.NewValues["RESSARCIMENTO"]) == false) ? false : true;
            motivo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivo.UsuarioId = User.Identity.Name;

            validacao = rnMotivoExigenciaEvento.Valida(motivo, true);

            if (validacao.Valido)
            {
                rnMotivoExigenciaEvento.Insere(motivo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoExigenciaEvento.DataBind();

        }

        protected void grdMotivoExigenciaEvento_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.MotivoExigenciaEvento motivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.MotivoExigenciaEvento();
            RN.PrestacaoContas.MotivoExigenciaEvento rnMotivoExigenciaEvento = new RN.PrestacaoContas.MotivoExigenciaEvento();

            motivo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivo.Ressarcimento = (e.NewValues["RESSARCIMENTO"] == null || Convert.ToBoolean(e.NewValues["RESSARCIMENTO"]) == false) ? false : true;
            motivo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivo.MotivoExigenciaEventoId = Convert.ToInt32(e.Keys["MOTIVOEXIGENCIAEVENTOID"]);
            motivo.UsuarioId = User.Identity.Name;

            validacao = rnMotivoExigenciaEvento.Valida(motivo, true);

            if (validacao.Valido)
            {
                rnMotivoExigenciaEvento.Atualiza(motivo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoExigenciaEvento.DataBind();
        }

        protected void grdMotivoExigenciaEvento_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.MotivoExigenciaEvento rnMotivoExigenciaEvento = new RN.PrestacaoContas.MotivoExigenciaEvento();
            int motivoId = 0;

            motivoId = Convert.ToInt32(e.Keys["MOTIVOEXIGENCIAEVENTOID"]);

            validacao = rnMotivoExigenciaEvento.ValidaRemocao(motivoId);

            if (validacao.Valido)
            {
                rnMotivoExigenciaEvento.Remove(motivoId);
                grdMotivoExigenciaEvento.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

      
       
    }
}
