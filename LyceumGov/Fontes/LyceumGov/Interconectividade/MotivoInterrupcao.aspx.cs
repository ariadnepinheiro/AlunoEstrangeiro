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
         NavUrl("~/Interconectividade/MotivoInterrupcao.aspx"),
         ControlText("MotivoInterrupcao"),
         Title("Motivo Interrupção")
     ]
    public partial class MotivoInterrupcao : TPage
    {
        public object Lista()
        {
            RN.FiscalizacaoLink.MotivoInterrupcao rnMotivoInterrupcao = new Techne.Lyceum.RN.FiscalizacaoLink.MotivoInterrupcao();

            return rnMotivoInterrupcao.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object MOTIVOINTERRUPCAOID) { }
        public void Delete(object MOTIVOINTERRUPCAOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdMotivoInterrupcao, "Motivo Interrupção");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdMotivoInterrupcao);
        }

        protected void grdMotivoInterrupcao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdMotivoInterrupcao.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoInterrupcao_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdMotivoInterrupcao.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoInterrupcao_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.FiscalizacaoLink.Entidades.MotivoInterrupcao motivoInterrupcao = new Techne.Lyceum.RN.FiscalizacaoLink.Entidades.MotivoInterrupcao();
            RN.FiscalizacaoLink.MotivoInterrupcao rnMotivoInterrupcao = new RN.FiscalizacaoLink.MotivoInterrupcao();

            motivoInterrupcao.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivoInterrupcao.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivoInterrupcao.UsuarioId = User.Identity.Name;

            validacao = rnMotivoInterrupcao.Valida(motivoInterrupcao, true);

            if (validacao.Valido)
            {
                rnMotivoInterrupcao.Insere(motivoInterrupcao);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoInterrupcao.DataBind();

        }

        protected void grdMotivoInterrupcao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.FiscalizacaoLink.Entidades.MotivoInterrupcao motivoInterrupcao = new Techne.Lyceum.RN.FiscalizacaoLink.Entidades.MotivoInterrupcao();
            RN.FiscalizacaoLink.MotivoInterrupcao rnMotivoInterrupcao = new RN.FiscalizacaoLink.MotivoInterrupcao();

            motivoInterrupcao.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivoInterrupcao.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivoInterrupcao.MotivoInterrupcaoId = Convert.ToInt32(e.Keys["MOTIVOINTERRUPCAOID"]);
            motivoInterrupcao.UsuarioId = User.Identity.Name;

            validacao = rnMotivoInterrupcao.Valida(motivoInterrupcao, true);

            if (validacao.Valido)
            {
                rnMotivoInterrupcao.Atualiza(motivoInterrupcao);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoInterrupcao.DataBind();
        }

        protected void grdMotivoInterrupcao_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.FiscalizacaoLink.MotivoInterrupcao rnMotivoInterrupcao = new RN.FiscalizacaoLink.MotivoInterrupcao();
            int motivoInterrupcaoId = 0;

            motivoInterrupcaoId = Convert.ToInt32(e.Keys["MOTIVOINTERRUPCAOID"]);

            validacao = rnMotivoInterrupcao.ValidaRemocao(motivoInterrupcaoId);

            if (validacao.Valido)
            {
                rnMotivoInterrupcao.Remove(motivoInterrupcaoId);
                grdMotivoInterrupcao.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
