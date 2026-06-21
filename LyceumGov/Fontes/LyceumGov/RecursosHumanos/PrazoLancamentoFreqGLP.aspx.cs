using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.RecursosHumanos
{
    [
    NavUrl("~/RecursosHumanos/PrazoLancamentoFreqGLP.aspx"),
     ControlText("PrazoLancamentoFreqGLP"),
     Title("Prazo Lançamento Excepcional Frequência GLP"),
   ]

    public partial class PrazoLancamentoFreqGLP : TPage
    {
        public object Lista()
        {
            RN.RecursosHumanos.PeriodoLancamentoFreqGLP rnPeriodoLancamento = new Techne.Lyceum.RN.RecursosHumanos.PeriodoLancamentoFreqGLP();

            return rnPeriodoLancamento.Lista();

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdPrazo, "Prazo Lançamento Excepcional Frequência GLP");
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdPrazo);
        }

        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
        }
        #endregion

        protected void grdPrazo_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdPrazo);
        }


        public void Insert(object ANO, object MES, object DATAINICIO, object DATAFIM) { }
        public void Update(object ANO, object MES, object DATAINICIO, object DATAFIM, object PeriodoLancamentoFreqGLPID) { }
        public void Delete(object PeriodoLancamentoFreqGLPID) { }

        protected void grdPrazo_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdPrazo.Settings.ShowFilterRow = false;
        }

        protected void grdPrazo_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdPrazo.Settings.ShowFilterRow = false;
        }

        protected void grdPrazo_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RecursosHumanos.Entidades.PeriodoLancamentoFreqGLP periodoLancamento = new Techne.Lyceum.RN.RecursosHumanos.Entidades.PeriodoLancamentoFreqGLP();
            RN.RecursosHumanos.PeriodoLancamentoFreqGLP rnPeriodoLancamento = new Techne.Lyceum.RN.RecursosHumanos.PeriodoLancamentoFreqGLP();

            periodoLancamento.Ano = e.NewValues["ANO"] != null ? Convert.ToInt32(e.NewValues["ANO"]) : -1;
            periodoLancamento.Mes = e.NewValues["MES"] != null ? Convert.ToInt32(e.NewValues["MES"]) : -1;
            periodoLancamento.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            periodoLancamento.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            periodoLancamento.UsuarioId = User.Identity.Name;

            validacao = rnPeriodoLancamento.Valida(periodoLancamento, true);

            if (validacao.Valido)
            {
                rnPeriodoLancamento.Insere(periodoLancamento);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdPrazo.DataBind();

        }

        protected void grdPrazo_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RecursosHumanos.Entidades.PeriodoLancamentoFreqGLP periodoLancamento = new Techne.Lyceum.RN.RecursosHumanos.Entidades.PeriodoLancamentoFreqGLP();
            RN.RecursosHumanos.PeriodoLancamentoFreqGLP rnPeriodoLancamento = new Techne.Lyceum.RN.RecursosHumanos.PeriodoLancamentoFreqGLP();

            periodoLancamento.Ano = e.NewValues["ANO"] != null ? Convert.ToInt32(e.NewValues["ANO"]) : -1;
            periodoLancamento.Mes = e.NewValues["MES"] != null ? Convert.ToInt32(e.NewValues["MES"]) : -1;
            periodoLancamento.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            periodoLancamento.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            periodoLancamento.UsuarioId = User.Identity.Name;
            periodoLancamento.PeriodoLancamentoFreqGLPId = Convert.ToInt32(e.Keys["PERIODOLANCAMENTOFREQGLPID"]);

            validacao = rnPeriodoLancamento.Valida(periodoLancamento, true);

            if (validacao.Valido)
            {
                rnPeriodoLancamento.Atualiza(periodoLancamento);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdPrazo.DataBind();
        }

        protected void grdPrazo_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RecursosHumanos.PeriodoLancamentoFreqGLP rnPeriodoLancamento = new Techne.Lyceum.RN.RecursosHumanos.PeriodoLancamentoFreqGLP();
            int PeriodoLancamentoFreqGLPId = 0;

            PeriodoLancamentoFreqGLPId = Convert.ToInt32(e.Keys["PERIODOLANCAMENTOFREQGLPID"]);


            rnPeriodoLancamento.Remove(PeriodoLancamentoFreqGLPId);
            grdPrazo.DataBind();

        }

        protected void grdPrazo_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdPrazo.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "PERIODOLANCAMENTOFREQGLPID")
                {
                    e.Editor.Enabled = true;
                }
            }
            else if (grdPrazo.IsEditing)
            {
                if ((e.Column.FieldName) == "PERIODOLANCAMENTOFREQGLPID")
                {
                    e.Editor.Enabled = false;
                }
                if ((e.Column.FieldName) == "ANO")
                {
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "MES")
                {
                    e.Editor.ReadOnly = true;
                }
            }
        }    
    }
}
