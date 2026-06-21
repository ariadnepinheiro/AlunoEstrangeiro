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
         NavUrl("~/PrestacaoContas/TipoExigenciaExtrato.aspx"),
         ControlText("TipoExigenciaExtrato"),
         Title("Tipo Exigência Extrato")
     ]
    public partial class TipoExigenciaExtrato : TPage
    {
        public object Lista()
        {
            RN.PrestacaoContas.TipoExigenciaExtrato rnTipoExigenciaExtrato = new Techne.Lyceum.RN.PrestacaoContas.TipoExigenciaExtrato();

            return rnTipoExigenciaExtrato.Lista();

        }


        public void Delete(object TIPOEXIGENCIAEXTRATOID) { }
        public void Insert(object DESCRICAO, object DATAINICIO, object DATAFIM) { }
        public void Update(object DESCRICAO, object DATAINICIO,object DATAFIM, object TIPOEXIGENCIAEXTRATOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTipoExigenciaExtrato, "Tipo Exigência Extrato");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTipoExigenciaExtrato);
        }

        protected void grdTipoExigenciaExtrato_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdTipoExigenciaExtrato);
        }		

        protected void grdTipoExigenciaExtrato_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTipoExigenciaExtrato.Settings.ShowFilterRow = false;
        }

        protected void grdTipoExigenciaExtrato_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdTipoExigenciaExtrato.Settings.ShowFilterRow = false;
        }

        protected void grdTipoExigenciaExtrato_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.TipoExigenciaExtrato tipo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.TipoExigenciaExtrato();
            RN.PrestacaoContas.TipoExigenciaExtrato rnTipoExigenciaExtrato = new RN.PrestacaoContas.TipoExigenciaExtrato();

            tipo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipo.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            tipo.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            tipo.TipoExigenciaExtratoId = Convert.ToInt32(e.Keys["TIPOEXIGENCIAEXTRATOID"]);
            tipo.UsuarioId = User.Identity.Name;

            validacao = rnTipoExigenciaExtrato.Valida(tipo, false);

            if (validacao.Valido)
            {
                rnTipoExigenciaExtrato.Atualiza(tipo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoExigenciaExtrato.DataBind();
        }

        protected void grdTipoExigenciaExtrato_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.TipoExigenciaExtrato tipo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.TipoExigenciaExtrato();
            RN.PrestacaoContas.TipoExigenciaExtrato rnTipoExigenciaExtrato = new RN.PrestacaoContas.TipoExigenciaExtrato();

            tipo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipo.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            tipo.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            tipo.UsuarioId = User.Identity.Name;

            validacao = rnTipoExigenciaExtrato.Valida(tipo, true);

            if (validacao.Valido)
            {
                rnTipoExigenciaExtrato.Insere(tipo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoExigenciaExtrato.DataBind();

        }

        protected void grdTipoExigenciaExtrato_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.TipoExigenciaExtrato rnTipoExigenciaExtrato = new RN.PrestacaoContas.TipoExigenciaExtrato();
            int tipoId = 0;

            tipoId = Convert.ToInt32(e.Keys["TIPOEXIGENCIAEXTRATOID"]);

            validacao = rnTipoExigenciaExtrato.ValidaRemocao(tipoId);

            if (validacao.Valido)
            {
                rnTipoExigenciaExtrato.Remove(tipoId);
                grdTipoExigenciaExtrato.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }


    }
}
