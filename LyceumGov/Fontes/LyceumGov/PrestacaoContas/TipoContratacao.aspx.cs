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
         NavUrl("~/PrestacaoContas/TipoContratacao.aspx"),
         ControlText("TipoContratacao"),
         Title("Tipo Transferência")
     ]
    public partial class TipoContratacao : TPage
    {
        public object Lista()
        {
            RN.PrestacaoContas.TipoContratacao rnTipoContratacao = new Techne.Lyceum.RN.PrestacaoContas.TipoContratacao();

            return rnTipoContratacao.Lista();

        }

        public void Insert(object DESCRICAO, object DATAINICIO,object DATAFIM) { }
        public void Update(object DESCRICAO, object DATAINICIO,object DATAFIM, object TIPOCONTRATACAOID) { }
        public void Delete(object TIPOCONTRATACAOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTipoContratacao, "Tipo Transferência");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTipoContratacao);
        }

        protected void grdTipoContratacao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdTipoContratacao);
        }		

        protected void grdTipoContratacao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTipoContratacao.Settings.ShowFilterRow = false;
        }

        protected void grdTipoContratacao_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {

            grdTipoContratacao.Settings.ShowFilterRow = false;
        }

        protected void grdTipoContratacao_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.TipoContratacao tipoContratacao = new Techne.Lyceum.RN.PrestacaoContas.Entidades.TipoContratacao();
            RN.PrestacaoContas.TipoContratacao rnTipoContratacao = new RN.PrestacaoContas.TipoContratacao();

            tipoContratacao.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipoContratacao.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            tipoContratacao.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            tipoContratacao.UsuarioId = User.Identity.Name;

            validacao = rnTipoContratacao.Valida(tipoContratacao, true);

            if (validacao.Valido)
            {
                rnTipoContratacao.Insere(tipoContratacao);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoContratacao.DataBind();

        }

        protected void grdTipoContratacao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.TipoContratacao tipoContratacao = new Techne.Lyceum.RN.PrestacaoContas.Entidades.TipoContratacao();
            RN.PrestacaoContas.TipoContratacao rnTipoContratacao = new RN.PrestacaoContas.TipoContratacao();

            tipoContratacao.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipoContratacao.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            tipoContratacao.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            tipoContratacao.TipoContratacaoId = Convert.ToInt32(e.Keys["TIPOCONTRATACAOID"]);
            tipoContratacao.UsuarioId = User.Identity.Name;

            validacao = rnTipoContratacao.Valida(tipoContratacao, true);

            if (validacao.Valido)
            {
                rnTipoContratacao.Atualiza(tipoContratacao);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoContratacao.DataBind();
        }

        protected void grdTipoContratacao_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.TipoContratacao rnTipoContratacao = new RN.PrestacaoContas.TipoContratacao();
            int tipoContratacaoId = 0;

            tipoContratacaoId = Convert.ToInt32(e.Keys["TIPOCONTRATACAOID"]);

            validacao = rnTipoContratacao.ValidaRemocao(tipoContratacaoId);

            if (validacao.Valido)
            {
                rnTipoContratacao.Remove(tipoContratacaoId);
                grdTipoContratacao.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

    }
}
