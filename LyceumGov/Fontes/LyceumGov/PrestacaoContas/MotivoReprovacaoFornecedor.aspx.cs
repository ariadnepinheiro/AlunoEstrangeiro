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
         NavUrl("~/PrestacaoContas/MotivoReprovacaoFornecedor.aspx"),
         ControlText("MotivoReprovacaoFornecedor"),
         Title("Motivo Reprovação Fornecedor")
     ]
    public partial class MotivoReprovacaoFornecedor : TPage
    {
        public object Lista()
        {
            RN.PrestacaoContas.MotivoReprovacaoFornecedor rnMotivoReprovacaoFornecedor = new Techne.Lyceum.RN.PrestacaoContas.MotivoReprovacaoFornecedor();

            return rnMotivoReprovacaoFornecedor.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object MOTIVOREPROVACAOFORNECEDORID) { }
        public void Delete(object MOTIVOREPROVACAOFORNECEDORID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdMotivoReprovacaoFornecedor, "Motivo Reprovação Fornecedor");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdMotivoReprovacaoFornecedor);
        }

        protected void grdMotivoReprovacaoFornecedor_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdMotivoReprovacaoFornecedor);
        }		

        protected void grdMotivoReprovacaoFornecedor_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdMotivoReprovacaoFornecedor.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoReprovacaoFornecedor_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdMotivoReprovacaoFornecedor.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoReprovacaoFornecedor_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.MotivoReprovacaoFornecedor motivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.MotivoReprovacaoFornecedor();
            RN.PrestacaoContas.MotivoReprovacaoFornecedor rnMotivoReprovacaoFornecedor = new RN.PrestacaoContas.MotivoReprovacaoFornecedor();

            motivo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivo.UsuarioId = User.Identity.Name;

            validacao = rnMotivoReprovacaoFornecedor.Valida(motivo, true);

            if (validacao.Valido)
            {
                rnMotivoReprovacaoFornecedor.Insere(motivo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoReprovacaoFornecedor.DataBind();

        }

        protected void grdMotivoReprovacaoFornecedor_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.MotivoReprovacaoFornecedor motivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.MotivoReprovacaoFornecedor();
            RN.PrestacaoContas.MotivoReprovacaoFornecedor rnMotivoReprovacaoFornecedor = new RN.PrestacaoContas.MotivoReprovacaoFornecedor();

            motivo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivo.MotivoReprovacaoFornecedorId = Convert.ToInt32(e.Keys["MOTIVOREPROVACAOFORNECEDORID"]);
            motivo.UsuarioId = User.Identity.Name;

            validacao = rnMotivoReprovacaoFornecedor.Valida(motivo, true);

            if (validacao.Valido)
            {
                rnMotivoReprovacaoFornecedor.Atualiza(motivo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoReprovacaoFornecedor.DataBind();
        }

        protected void grdMotivoReprovacaoFornecedor_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.MotivoReprovacaoFornecedor rnMotivoReprovacaoFornecedor = new RN.PrestacaoContas.MotivoReprovacaoFornecedor();
            int motivoId = 0;

            motivoId = Convert.ToInt32(e.Keys["MOTIVOREPROVACAOFORNECEDORID"]);

            validacao = rnMotivoReprovacaoFornecedor.ValidaRemocao(motivoId);

            if (validacao.Valido)
            {
                rnMotivoReprovacaoFornecedor.Remove(motivoId);
                grdMotivoReprovacaoFornecedor.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

       

    }
}
