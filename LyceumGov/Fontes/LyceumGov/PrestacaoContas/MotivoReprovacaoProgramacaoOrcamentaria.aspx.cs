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
         NavUrl("~/PrestacaoContas/MotivoReprovacaoProgramacaoOrcamentaria.aspx"),
         ControlText("MotivoReprovacaoProgramacaoOrcamentaria"),
         Title("Motivo Reprovação Programação Orçamentária")
     ]
    public partial class MotivoReprovacaoProgramacaoOrcamentaria : TPage
    {
        public object Lista()
        {
            RN.PrestacaoContas.MotivoReprovacaoPlanilhaOrcamentaria rnMotivoReprovacaoProgramacaoOrcamentaria = new Techne.Lyceum.RN.PrestacaoContas.MotivoReprovacaoPlanilhaOrcamentaria();

            return rnMotivoReprovacaoProgramacaoOrcamentaria.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object MOTIVOREPROVACAOPLANILHAORCAMENTARIAID) { }
        public void Delete(object MOTIVOREPROVACAOPLANILHAORCAMENTARIAID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdMotivoReprovacaoProgramacaoOrcamentaria, "Motivo Reprovação Programação Orçamentária");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdMotivoReprovacaoProgramacaoOrcamentaria);
        }

        protected void grdMotivoReprovacaoProgramacaoOrcamentaria_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdMotivoReprovacaoProgramacaoOrcamentaria);
        }		

        protected void grdMotivoReprovacaoProgramacaoOrcamentaria_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdMotivoReprovacaoProgramacaoOrcamentaria.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoReprovacaoProgramacaoOrcamentaria_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdMotivoReprovacaoProgramacaoOrcamentaria.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoReprovacaoProgramacaoOrcamentaria_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.MotivoReprovacaoPlanilhaOrcamentaria motivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.MotivoReprovacaoPlanilhaOrcamentaria();
            RN.PrestacaoContas.MotivoReprovacaoPlanilhaOrcamentaria rnMotivoReprovacaoProgramacaoOrcamentaria = new RN.PrestacaoContas.MotivoReprovacaoPlanilhaOrcamentaria();

            motivo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivo.UsuarioId = User.Identity.Name;

            validacao = rnMotivoReprovacaoProgramacaoOrcamentaria.Valida(motivo, true);

            if (validacao.Valido)
            {
                rnMotivoReprovacaoProgramacaoOrcamentaria.Insere(motivo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoReprovacaoProgramacaoOrcamentaria.DataBind();

        }

        protected void grdMotivoReprovacaoProgramacaoOrcamentaria_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.MotivoReprovacaoPlanilhaOrcamentaria motivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.MotivoReprovacaoPlanilhaOrcamentaria();
            RN.PrestacaoContas.MotivoReprovacaoPlanilhaOrcamentaria rnMotivoReprovacaoProgramacaoOrcamentaria = new RN.PrestacaoContas.MotivoReprovacaoPlanilhaOrcamentaria();

            motivo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivo.MotivoReprovacaoPlanilhaOrcamentariaId = Convert.ToInt32(e.Keys["MOTIVOREPROVACAOPLANILHAORCAMENTARIAID"]);
            motivo.UsuarioId = User.Identity.Name;

            validacao = rnMotivoReprovacaoProgramacaoOrcamentaria.Valida(motivo, true);

            if (validacao.Valido)
            {
                rnMotivoReprovacaoProgramacaoOrcamentaria.Atualiza(motivo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoReprovacaoProgramacaoOrcamentaria.DataBind();
        }

        protected void grdMotivoReprovacaoProgramacaoOrcamentaria_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.MotivoReprovacaoPlanilhaOrcamentaria rnMotivoReprovacaoProgramacaoOrcamentaria = new RN.PrestacaoContas.MotivoReprovacaoPlanilhaOrcamentaria();
            int motivoId = 0;

            motivoId = Convert.ToInt32(e.Keys["MOTIVOREPROVACAOPLANILHAORCAMENTARIAID"]);

            validacao = rnMotivoReprovacaoProgramacaoOrcamentaria.ValidaRemocao(motivoId);

            if (validacao.Valido)
            {
                rnMotivoReprovacaoProgramacaoOrcamentaria.Remove(motivoId);
                grdMotivoReprovacaoProgramacaoOrcamentaria.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

       

    }
}
