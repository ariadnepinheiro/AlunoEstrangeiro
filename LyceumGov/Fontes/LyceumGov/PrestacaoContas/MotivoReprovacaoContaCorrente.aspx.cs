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
         NavUrl("~/PrestacaoContas/MotivoReprovacaoContaCorrente.aspx"),
         ControlText("MotivoReprovacaoContaCorrente"),
         Title("Motivo Reprovação Conta Corrente")
     ]
    public partial class MotivoReprovacaoContaCorrente : TPage
    {
        public object Lista()
        {
            RN.PrestacaoContas.MotivoReprovacaoContaCorrente rnMotivoReprovacaoContaCorrente = new Techne.Lyceum.RN.PrestacaoContas.MotivoReprovacaoContaCorrente();

            return rnMotivoReprovacaoContaCorrente.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object MOTIVOREPROVACAOCONTACORRENTEID) { }
        public void Delete(object MOTIVOREPROVACAOCONTACORRENTEID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdMotivoReprovacaoContaCorrente, "Motivo Reprovação Conta Corrente");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdMotivoReprovacaoContaCorrente);
        }

        protected void grdMotivoReprovacaoContaCorrente_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdMotivoReprovacaoContaCorrente);
        }		

        protected void grdMotivoReprovacaoContaCorrente_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdMotivoReprovacaoContaCorrente.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoReprovacaoContaCorrente_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdMotivoReprovacaoContaCorrente.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoReprovacaoContaCorrente_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.MotivoReprovacaoContaCorrente motivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.MotivoReprovacaoContaCorrente();
            RN.PrestacaoContas.MotivoReprovacaoContaCorrente rnMotivoReprovacaoContaCorrente = new RN.PrestacaoContas.MotivoReprovacaoContaCorrente();

            motivo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivo.UsuarioId = User.Identity.Name;

            validacao = rnMotivoReprovacaoContaCorrente.Valida(motivo, true);

            if (validacao.Valido)
            {
                rnMotivoReprovacaoContaCorrente.Insere(motivo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoReprovacaoContaCorrente.DataBind();

        }

        protected void grdMotivoReprovacaoContaCorrente_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.MotivoReprovacaoContaCorrente motivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.MotivoReprovacaoContaCorrente();
            RN.PrestacaoContas.MotivoReprovacaoContaCorrente rnMotivoReprovacaoContaCorrente = new RN.PrestacaoContas.MotivoReprovacaoContaCorrente();

            motivo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivo.MotivoReprovacaoContaCorrenteId = Convert.ToInt32(e.Keys["MOTIVOREPROVACAOCONTACORRENTEID"]);
            motivo.UsuarioId = User.Identity.Name;

            validacao = rnMotivoReprovacaoContaCorrente.Valida(motivo, true);

            if (validacao.Valido)
            {
                rnMotivoReprovacaoContaCorrente.Atualiza(motivo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoReprovacaoContaCorrente.DataBind();
        }

        protected void grdMotivoReprovacaoContaCorrente_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.MotivoReprovacaoContaCorrente rnMotivoReprovacaoContaCorrente = new RN.PrestacaoContas.MotivoReprovacaoContaCorrente();
            int motivoId = 0;

            motivoId = Convert.ToInt32(e.Keys["MOTIVOREPROVACAOCONTACORRENTEID"]);

            validacao = rnMotivoReprovacaoContaCorrente.ValidaRemocao(motivoId);

            if (validacao.Valido)
            {
                rnMotivoReprovacaoContaCorrente.Remove(motivoId);
                grdMotivoReprovacaoContaCorrente.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

       

    }
}
