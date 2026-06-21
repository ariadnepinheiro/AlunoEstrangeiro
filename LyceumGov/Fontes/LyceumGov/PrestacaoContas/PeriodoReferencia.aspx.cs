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
          NavUrl("~/PrestacaoContas/PeriodoReferencia.aspx"),
          ControlText("Período Referência"),
          Title("Período Referência")
      ]
    public partial class PeriodoReferencia : TPage
    {
        public object Lista()
        {
            RN.PrestacaoContas.PeriodoReferencia rnPeriodoReferencia = new Techne.Lyceum.RN.PrestacaoContas.PeriodoReferencia();

            return rnPeriodoReferencia.Lista();

        }

        public void Insert(object ANO, object MESINICIAL, object MESFINAL, object REFERENCIA, object ANOANTERIOR, object MESANTERIOR, object DATALIMITEPRESTACAOCONTAS, object DATALIMITEANALISE) { }
        public void Update(object ANO, object MESINICIAL, object MESFINAL, object REFERENCIA, object ANOANTERIOR, object MESANTERIOR, object DATALIMITEPRESTACAOCONTAS, object DATALIMITEANALISE, object PERIODOREFERENCIAID) { }
        public void Delete(object PERIODOREFERENCIAID) { }
        public void Update(object REFERENCIA, object ANOANTERIOR, object MESANTERIOR, object DATALIMITEPRESTACAOCONTAS, object DATALIMITEANALISE, object PERIODOREFERENCIAID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdPeriodoReferencia, "Período Referência");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdPeriodoReferencia);
        }

        protected void grdPeriodoReferencia_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdPeriodoReferencia);
        }	

        protected void grdPeriodoReferencia_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdPeriodoReferencia.Settings.ShowFilterRow = false;
        }

        protected void grdPeriodoReferencia_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdPeriodoReferencia.Settings.ShowFilterRow = false;
        }

        protected void grdPeriodoReferencia_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdPeriodoReferencia.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "ANO")
                    e.Editor.Enabled = true;

                if ((e.Column.FieldName) == "MESINICIAL")
                    e.Editor.Enabled = true;
                
                if ((e.Column.FieldName) == "MESFINAL")
                    e.Editor.Enabled = true;
            }
            else if (grdPeriodoReferencia.IsEditing)
            {
                if ((e.Column.FieldName) == "ANO")
                    e.Editor.Enabled = false;

                if ((e.Column.FieldName) == "MESINICIAL")
                    e.Editor.Enabled = false;
                
                if ((e.Column.FieldName) == "MESFINAL")
                    e.Editor.Enabled = false;
            }
        }

        protected void grdPeriodoReferencia_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.PeriodoReferencia periodoReferencia = new Techne.Lyceum.RN.PrestacaoContas.Entidades.PeriodoReferencia();
            RN.PrestacaoContas.PeriodoReferencia rnPeriodoReferencia = new Techne.Lyceum.RN.PrestacaoContas.PeriodoReferencia();

            periodoReferencia.Ano = e.NewValues["ANO"] != null ? Convert.ToInt32(e.NewValues["ANO"]) : -1;
            periodoReferencia.MesInicial = e.NewValues["MESINICIAL"] != null ? Convert.ToInt32(e.NewValues["MESINICIAL"]) : -1;
            periodoReferencia.MesFinal = e.NewValues["MESFINAL"] != null ? Convert.ToInt32(e.NewValues["MESFINAL"]) : -1;
            periodoReferencia.Referencia = e.NewValues["REFERENCIA"] != null ? Convert.ToString(e.NewValues["REFERENCIA"]) : null;            
            periodoReferencia.AnoAnterior = e.NewValues["ANOANTERIOR"] != null ? Convert.ToInt32(e.NewValues["ANOANTERIOR"]) : -1;
            periodoReferencia.MesAnterior = e.NewValues["MESANTERIOR"] != null ? Convert.ToInt32(e.NewValues["MESANTERIOR"]) : -1;
            periodoReferencia.DataLimitePrestacaoContas = e.NewValues["DATALIMITEPRESTACAOCONTAS"] != null ? Convert.ToDateTime(e.NewValues["DATALIMITEPRESTACAOCONTAS"]) : DateTime.MinValue;
            periodoReferencia.DataLimiteAnalise = e.NewValues["DATALIMITEANALISE"] != null ? Convert.ToDateTime(e.NewValues["DATALIMITEANALISE"]) : DateTime.MinValue;
            periodoReferencia.DataLimiteDespesas = e.NewValues["DATALIMITEDESPESAS"] != null ? Convert.ToDateTime(e.NewValues["DATALIMITEDESPESAS"]) : DateTime.MinValue;
            periodoReferencia.UsuarioId = User.Identity.Name;

            validacao = rnPeriodoReferencia.Valida(periodoReferencia, true);

            if (validacao.Valido)
            {
                rnPeriodoReferencia.Insere(periodoReferencia);

                e.Cancel = true;
                grdPeriodoReferencia.CancelEdit();
                grdPeriodoReferencia.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdPeriodoReferencia.DataBind();

        }

        protected void grdPeriodoReferencia_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.PeriodoReferencia periodoReferencia = new Techne.Lyceum.RN.PrestacaoContas.Entidades.PeriodoReferencia();
            RN.PrestacaoContas.PeriodoReferencia rnPeriodoReferencia = new Techne.Lyceum.RN.PrestacaoContas.PeriodoReferencia();

            var ano = ((ASPxGridView)sender).GetRowValuesByKeyValue(e.Keys[0], "ANO");
            var mesInicial = ((ASPxGridView)sender).GetRowValuesByKeyValue(e.Keys[0], "MESINICIAL");
            var mesFinal = ((ASPxGridView)sender).GetRowValuesByKeyValue(e.Keys[0], "MESFINAL");

            periodoReferencia.Ano = Convert.ToInt32(ano);
            periodoReferencia.MesInicial = Convert.ToInt32(mesInicial);
            periodoReferencia.MesFinal = Convert.ToInt32(mesFinal);
            periodoReferencia.Referencia = e.NewValues["REFERENCIA"] != null ? Convert.ToString(e.NewValues["REFERENCIA"]) : null;
            periodoReferencia.AnoAnterior = e.NewValues["ANOANTERIOR"] != null ? Convert.ToInt32(e.NewValues["ANOANTERIOR"]) : -1;
            periodoReferencia.MesAnterior = e.NewValues["MESANTERIOR"] != null ? Convert.ToInt32(e.NewValues["MESANTERIOR"]) : -1;
            periodoReferencia.DataLimitePrestacaoContas = e.NewValues["DATALIMITEPRESTACAOCONTAS"] != null ? Convert.ToDateTime(e.NewValues["DATALIMITEPRESTACAOCONTAS"]) : DateTime.MinValue;
            periodoReferencia.DataLimiteAnalise = e.NewValues["DATALIMITEANALISE"] != null ? Convert.ToDateTime(e.NewValues["DATALIMITEANALISE"]) : DateTime.MinValue ;
            periodoReferencia.DataLimiteDespesas = e.NewValues["DATALIMITEDESPESAS"] != null ? Convert.ToDateTime(e.NewValues["DATALIMITEDESPESAS"]) : DateTime.MinValue;
            periodoReferencia.PeriodoReferenciaId = Convert.ToInt32(e.Keys["PERIODOREFERENCIAID"]);
            periodoReferencia.UsuarioId = User.Identity.Name;

            validacao = rnPeriodoReferencia.Valida(periodoReferencia, true);

            if (validacao.Valido)
            {
                rnPeriodoReferencia.Atualiza(periodoReferencia);

                e.Cancel = true;
                grdPeriodoReferencia.CancelEdit();
                grdPeriodoReferencia.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdPeriodoReferencia.DataBind();
        }

        protected void grdPeriodoReferencia_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.PeriodoReferencia rnPeriodoReferencia = new Techne.Lyceum.RN.PrestacaoContas.PeriodoReferencia();
            int periodoReferenciaId = 0;

            periodoReferenciaId = Convert.ToInt32(e.Keys["PERIODOREFERENCIAID"]);

            validacao = rnPeriodoReferencia.ValidaRemocao(periodoReferenciaId);

            if (validacao.Valido)
            {
                rnPeriodoReferencia.Remove(periodoReferenciaId);
                grdPeriodoReferencia.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
