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
         NavUrl("~/PrestacaoContas/MotivoReprovacaoRepasse.aspx"),
         ControlText("MotivoReprovacaoRepasse"),
         Title("Motivo Reprovação Repasse")
     ]
    public partial class MotivoReprovacaoRepasse : TPage
    {
        public object Lista()
        {
            RN.PrestacaoContas.MotivoReprovacaoLancamentoRepasse rnMotivoReprovacaoRepasse = new Techne.Lyceum.RN.PrestacaoContas.MotivoReprovacaoLancamentoRepasse();

            return rnMotivoReprovacaoRepasse.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object MOTIVOREPROVACAOLANCAMENTOREPASSEID) { }
        public void Delete(object MOTIVOREPROVACAOLANCAMENTOREPASSEID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdMotivoReprovacaoRepasse, "Motivo Reprovação Repasse");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdMotivoReprovacaoRepasse);
        }

        protected void grdMotivoReprovacaoRepasse_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdMotivoReprovacaoRepasse);
        }		
        
        protected void grdMotivoReprovacaoRepasse_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdMotivoReprovacaoRepasse.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoReprovacaoRepasse_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdMotivoReprovacaoRepasse.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoReprovacaoRepasse_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.MotivoReprovacaoLancamentoRepasse motivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.MotivoReprovacaoLancamentoRepasse();
            RN.PrestacaoContas.MotivoReprovacaoLancamentoRepasse rnMotivoReprovacaoRepasse = new RN.PrestacaoContas.MotivoReprovacaoLancamentoRepasse();

            motivo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivo.UsuarioId = User.Identity.Name;

            validacao = rnMotivoReprovacaoRepasse.Valida(motivo, true);

            if (validacao.Valido)
            {
                rnMotivoReprovacaoRepasse.Insere(motivo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoReprovacaoRepasse.DataBind();

        }

        protected void grdMotivoReprovacaoRepasse_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.MotivoReprovacaoLancamentoRepasse motivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.MotivoReprovacaoLancamentoRepasse();
            RN.PrestacaoContas.MotivoReprovacaoLancamentoRepasse rnMotivoReprovacaoRepasse = new RN.PrestacaoContas.MotivoReprovacaoLancamentoRepasse();

            motivo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivo.MotivoReprovacaoLancamentoRepasseId = Convert.ToInt32(e.Keys["MOTIVOREPROVACAOLANCAMENTOREPASSEID"]);
            motivo.UsuarioId = User.Identity.Name;

            validacao = rnMotivoReprovacaoRepasse.Valida(motivo, false);

            if (validacao.Valido)
            {
                rnMotivoReprovacaoRepasse.Atualiza(motivo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoReprovacaoRepasse.DataBind();
        }

        protected void grdMotivoReprovacaoRepasse_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.MotivoReprovacaoLancamentoRepasse rnMotivoReprovacaoRepasse = new RN.PrestacaoContas.MotivoReprovacaoLancamentoRepasse();
            int motivoId = 0;

            motivoId = Convert.ToInt32(e.Keys["MOTIVOREPROVACAOLANCAMENTOREPASSEID"]);

            validacao = rnMotivoReprovacaoRepasse.ValidaRemocao(motivoId);

            if (validacao.Valido)
            {
                rnMotivoReprovacaoRepasse.Remove(motivoId);
                grdMotivoReprovacaoRepasse.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

      
       
    }
}
