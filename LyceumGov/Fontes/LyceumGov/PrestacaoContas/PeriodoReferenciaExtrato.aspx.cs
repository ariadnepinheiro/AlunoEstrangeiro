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
         NavUrl("~/PrestacaoContas/PeriodoReferenciaExtrato.aspx"),
         ControlText("PeriodoReferenciaExtrato"),
         Title("Período Referência Extrato")
     ]
    public partial class PeriodoReferenciaExtrato : TPage
    {
        public object Lista()
        {
            RN.PrestacaoContas.PeriodoReferenciaExtratoBancario rnPeriodoReferenciaExtrato = new Techne.Lyceum.RN.PrestacaoContas.PeriodoReferenciaExtratoBancario();

            return rnPeriodoReferenciaExtrato.Lista();
        }

        public void Insert(object DIAREFERENCIA, object DATAINICIO, object DATAFIM) { }
        public void Update(object DIAREFERENCIA, object DATAINICIO, object DATAFIM, object PERIODOREFERENCIAEXTRATOBANCARIOID) { }
        public void Delete(object PERIODOREFERENCIAEXTRATOBANCARIOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdPeriodoReferenciaExtrato, "Período Referência Extrato");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdPeriodoReferenciaExtrato);
        }

        protected void grdPeriodoReferenciaExtrato_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdPeriodoReferenciaExtrato);
        }		


        protected void grdPeriodoReferenciaExtrato_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdPeriodoReferenciaExtrato.Settings.ShowFilterRow = false;
        }

        protected void grdPeriodoReferenciaExtrato_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdPeriodoReferenciaExtrato.Settings.ShowFilterRow = false;
        }

        protected void grdPeriodoReferenciaExtrato_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.PeriodoReferenciaExtratoBancario periodo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.PeriodoReferenciaExtratoBancario();
            RN.PrestacaoContas.PeriodoReferenciaExtratoBancario rnPeriodoReferenciaExtrato = new RN.PrestacaoContas.PeriodoReferenciaExtratoBancario();

            
            periodo.DiaReferencia = e.NewValues["DIAREFERENCIA"] != null ? Convert.ToInt32(e.NewValues["DIAREFERENCIA"]) : -1;
            periodo.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            periodo.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            periodo.UsuarioId = User.Identity.Name;

            validacao = rnPeriodoReferenciaExtrato.Valida(periodo, true);

            if (validacao.Valido)
            {
                rnPeriodoReferenciaExtrato.Insere(periodo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdPeriodoReferenciaExtrato.DataBind();

        }

        protected void grdPeriodoReferenciaExtrato_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.PeriodoReferenciaExtratoBancario periodo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.PeriodoReferenciaExtratoBancario();
            RN.PrestacaoContas.PeriodoReferenciaExtratoBancario rnPeriodoReferenciaExtrato = new RN.PrestacaoContas.PeriodoReferenciaExtratoBancario();

            periodo.DiaReferencia = e.NewValues["DIAREFERENCIA"] != null ? Convert.ToInt32(e.NewValues["DIAREFERENCIA"]) : -1;
            periodo.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            periodo.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            periodo.PeriodoReferenciaExtratoBancarioId = Convert.ToInt32(e.Keys["PERIODOREFERENCIAEXTRATOBANCARIOID"]);
            periodo.UsuarioId = User.Identity.Name;

            validacao = rnPeriodoReferenciaExtrato.Valida(periodo, true);

            if (validacao.Valido)
            {
                rnPeriodoReferenciaExtrato.Atualiza(periodo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdPeriodoReferenciaExtrato.DataBind();
        }

        protected void grdPeriodoReferenciaExtrato_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.PeriodoReferenciaExtratoBancario rnPeriodoReferenciaExtrato = new RN.PrestacaoContas.PeriodoReferenciaExtratoBancario();
            int periodoId = 0;

            periodoId = Convert.ToInt32(e.Keys["PERIODOREFERENCIAEXTRATOBANCARIOID"]);

            validacao = rnPeriodoReferenciaExtrato.ValidaRemocao(periodoId);

            if (validacao.Valido)
            {
                rnPeriodoReferenciaExtrato.Remove(periodoId);
                grdPeriodoReferenciaExtrato.DataBind();
            }
            else
            {
                e.Cancel = true; 
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

        protected void grdPeriodoReferenciaExtrato_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            DateTime? dataFim = grdPeriodoReferenciaExtrato.GetRowValues(e.VisibleIndex, "DATAFIM") == DBNull.Value || grdPeriodoReferenciaExtrato.GetRowValues(e.VisibleIndex, "DATAFIM") == null ? (DateTime?)null : (DateTime)grdPeriodoReferenciaExtrato.GetRowValues(e.VisibleIndex, "DATAFIM");

            if (dataFim != null)
            {
                if (dataFim < DateTime.Now.Date)
                {
                    if (e.ButtonType == ColumnCommandButtonType.Delete || e.ButtonType == ColumnCommandButtonType.Edit)
                    {
                        e.Visible = false;
                    }
                }
            }
        }


    }
}
