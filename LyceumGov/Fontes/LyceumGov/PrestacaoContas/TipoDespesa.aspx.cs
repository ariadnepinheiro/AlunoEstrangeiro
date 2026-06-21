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
         NavUrl("~/PrestacaoContas/TipoDespesa.aspx"),
         ControlText("TipoDespesa"),
         Title("Tipo Despesa")
     ]
    public partial class TipoDespesa : TPage
    {
        public object Lista()
        {
            RN.PrestacaoContas.TipoDespesa rnTipoDespesa = new Techne.Lyceum.RN.PrestacaoContas.TipoDespesa();

            return rnTipoDespesa.Lista();

        }

        public void Insert(object DESCRICAO,object DATAINICIO,object DATAFIM) { }
        public void Update(object DESCRICAO,object DATAINICIO,object DATAFIM, object TIPODESPESAID) { }
        public void Delete(object TIPODESPESAID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTipoDespesa, "Tipo Despesa");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTipoDespesa);
        }

        protected void grdTipoDespesa_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdTipoDespesa);
        }		

        protected void grdTipoDespesa_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTipoDespesa.Settings.ShowFilterRow = false;
        }

        protected void grdTipoDespesa_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {

            grdTipoDespesa.Settings.ShowFilterRow = false;
        }

        protected void grdTipoDespesa_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.TipoDespesa tipoDespesa = new Techne.Lyceum.RN.PrestacaoContas.Entidades.TipoDespesa();
            RN.PrestacaoContas.TipoDespesa rnTipoDespesa = new RN.PrestacaoContas.TipoDespesa();

            tipoDespesa.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipoDespesa.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            tipoDespesa.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            tipoDespesa.UsuarioId = User.Identity.Name;

            validacao = rnTipoDespesa.Valida(tipoDespesa, true);

            if (validacao.Valido)
            {
                rnTipoDespesa.Insere(tipoDespesa);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoDespesa.DataBind();

        }

        protected void grdTipoDespesa_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.TipoDespesa tipoDespesa = new Techne.Lyceum.RN.PrestacaoContas.Entidades.TipoDespesa();
            RN.PrestacaoContas.TipoDespesa rnTipoDespesa = new RN.PrestacaoContas.TipoDespesa();

            tipoDespesa.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipoDespesa.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            tipoDespesa.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            tipoDespesa.TipoDespesaId = Convert.ToInt32(e.Keys["TIPODESPESAID"]);
            tipoDespesa.UsuarioId = User.Identity.Name;

            validacao = rnTipoDespesa.Valida(tipoDespesa, true);

            if (validacao.Valido)
            {
                rnTipoDespesa.Atualiza(tipoDespesa);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoDespesa.DataBind();
        }

        protected void grdTipoDespesa_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.TipoDespesa rnTipoDespesa = new RN.PrestacaoContas.TipoDespesa();
            int tipoDespesaid = 0;

            tipoDespesaid = Convert.ToInt32(e.Keys["TIPODESPESAID"]);

            validacao = rnTipoDespesa.ValidaRemocao(tipoDespesaid);

            if (validacao.Valido)
            {
                rnTipoDespesa.Remove(tipoDespesaid);
                grdTipoDespesa.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

    }
}
