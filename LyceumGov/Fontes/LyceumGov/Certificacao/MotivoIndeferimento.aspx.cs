using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxGridView;


namespace Techne.Lyceum.Net.Certificacao
{
    [
         NavUrl("~/Certificacao/MotivoIndeferimento.aspx"),
         ControlText("MotivoIndeferimento"),
         Title("Motivo Indeferimento")
     ]
    public partial class MotivoIndeferimento : TPage
    {
        public object Lista()
        {
            RN.Certificacao.MotivoIndeferido rnMotivoIndeferimento = new Techne.Lyceum.RN.Certificacao.MotivoIndeferido();

            return rnMotivoIndeferimento.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object MOTIVOINDEFERIDOID) { }
        public void Delete(object MOTIVOINDEFERIDOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdMotivoIndeferimento, "Motivo Indeferimento");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdMotivoIndeferimento);
        }

        protected void grdMotivoIndeferimento_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdMotivoIndeferimento.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoIndeferimento_OnAfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdMotivoIndeferimento);
        }

        protected void grdMotivoIndeferimento_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdMotivoIndeferimento.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoIndeferimento_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Certificacao.Entidades.MotivoIndeferido motivoIndeferimento = new Techne.Lyceum.RN.Certificacao.Entidades.MotivoIndeferido();
            RN.Certificacao.MotivoIndeferido rnMotivoIndeferimento = new RN.Certificacao.MotivoIndeferido();

            motivoIndeferimento.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivoIndeferimento.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivoIndeferimento.UsuarioId = User.Identity.Name;

            validacao = rnMotivoIndeferimento.Valida(motivoIndeferimento, true);

            if (validacao.Valido)
            {
                rnMotivoIndeferimento.Insere(motivoIndeferimento);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoIndeferimento.DataBind();

        }

        protected void grdMotivoIndeferimento_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Certificacao.Entidades.MotivoIndeferido motivoIndeferimento = new Techne.Lyceum.RN.Certificacao.Entidades.MotivoIndeferido();
            RN.Certificacao.MotivoIndeferido rnMotivoIndeferimento = new RN.Certificacao.MotivoIndeferido();

            motivoIndeferimento.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivoIndeferimento.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivoIndeferimento.MotivoIndeferidoId = Convert.ToInt32(e.Keys["MOTIVOINDEFERIDOID"]);
            motivoIndeferimento.UsuarioId = User.Identity.Name;

            validacao = rnMotivoIndeferimento.Valida(motivoIndeferimento, true);

            if (validacao.Valido)
            {
                rnMotivoIndeferimento.Atualiza(motivoIndeferimento);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoIndeferimento.DataBind();
        }

        protected void grdMotivoIndeferimento_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Certificacao.MotivoIndeferido rnMotivoIndeferimento = new RN.Certificacao.MotivoIndeferido();
            int motivoIndeferimentoId = 0;

            motivoIndeferimentoId = Convert.ToInt32(e.Keys["MOTIVOINDEFERIDOID"]);

            validacao = rnMotivoIndeferimento.ValidaRemocao(motivoIndeferimentoId);

            if (validacao.Valido)
            {
                rnMotivoIndeferimento.Remove(motivoIndeferimentoId);
                grdMotivoIndeferimento.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
