using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxGridView;


namespace Techne.Lyceum.Net.PrestacaoContas
{
    [
         NavUrl("~/PrestacaoContas/NaturezaDespesa.aspx"),
         ControlText("NaturezaDespesa"),
         Title("Natureza de Despesa")
     ]
    public partial class NaturezaDespesa : TPage
    {
        public object Lista()
        {
            RN.PrestacaoContas.NaturezaDespesa rnNaturezaDespesa = new Techne.Lyceum.RN.PrestacaoContas.NaturezaDespesa();

            return rnNaturezaDespesa.Lista();

        }

        public void Update(object DESCRICAO,object CODIGOSEFAZ, object ATIVO, object NATUREZADESPESAID) { }
        public void Delete(object NATUREZADESPESAID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdNaturezaDespesa, "Natureza de Despesa");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdNaturezaDespesa);
        }

        protected void grdNaturezaDespesa_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdNaturezaDespesa);
        }		

        protected void grdNaturezaDespesa_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdNaturezaDespesa.Settings.ShowFilterRow = false;
        }

        protected void grdNaturezaDespesa_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdNaturezaDespesa.Settings.ShowFilterRow = false;
        }

        protected void grdNaturezaDespesa_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.NaturezaDespesa naturezaDespesa = new Techne.Lyceum.RN.PrestacaoContas.Entidades.NaturezaDespesa();
            RN.PrestacaoContas.NaturezaDespesa rnNaturezaDespesa = new RN.PrestacaoContas.NaturezaDespesa();

            naturezaDespesa.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            naturezaDespesa.CodigoSefaz = e.NewValues["CODIGOSEFAZ"] != null ? e.NewValues["CODIGOSEFAZ"].ToString().Trim().ToUpper() : null;
            naturezaDespesa.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            naturezaDespesa.NaturezaDespesaId = Convert.ToInt32(e.Keys["NATUREZADESPESAID"]);
            naturezaDespesa.UsuarioId = User.Identity.Name;

            validacao = rnNaturezaDespesa.Valida(naturezaDespesa, true);

            if (validacao.Valido)
            {
                rnNaturezaDespesa.Atualiza(naturezaDespesa);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdNaturezaDespesa.DataBind();
        }

        protected void grdNaturezaDespesa_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.NaturezaDespesa rnNaturezaDespesa = new RN.PrestacaoContas.NaturezaDespesa();
            int naturezaDespesaId = 0;

            naturezaDespesaId = Convert.ToInt32(e.Keys["NATUREZADESPESAID"]);

            validacao = rnNaturezaDespesa.ValidaRemocao(naturezaDespesaId);

            if (validacao.Valido)
            {
                rnNaturezaDespesa.Remove(naturezaDespesaId);
                grdNaturezaDespesa.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
