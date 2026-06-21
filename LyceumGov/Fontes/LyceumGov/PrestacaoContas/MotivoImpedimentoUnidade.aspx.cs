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
         NavUrl("~/PrestacaoContas/MotivoImpedimentoUnidade.aspx"),
         ControlText("MotivoImpedimentoUnidade"),
         Title("Motivo Impedimento Unidade")
     ]
    public partial class MotivoImpedimentoUnidade : TPage
    {
        public object Lista()
        {
            RN.PrestacaoContas.MotivoImpedimento rnMotivoImpedimentoUnidade = new Techne.Lyceum.RN.PrestacaoContas.MotivoImpedimento();

            return rnMotivoImpedimentoUnidade.Lista();

        }

        public void Insert(object DESCRICAO, object DATAINICIO,object DATAFIM) { }
        public void Update(object DESCRICAO, object DATAINICIO,object DATAFIM, object MOTIVOIMPEDIMENTOID) { }
        public void Delete(object MOTIVOIMPEDIMENTOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdMotivoImpedimentoUnidade, "Motivo Impedimento Unidade");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdMotivoImpedimentoUnidade);
        }

        protected void grdMotivoImpedimentoUnidade_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdMotivoImpedimentoUnidade);
        }		

        protected void grdMotivoImpedimentoUnidade_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdMotivoImpedimentoUnidade.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoImpedimentoUnidade_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {

            grdMotivoImpedimentoUnidade.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoImpedimentoUnidade_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.MotivoImpedimento motivoImpedimento = new Techne.Lyceum.RN.PrestacaoContas.Entidades.MotivoImpedimento();
            RN.PrestacaoContas.MotivoImpedimento rnMotivoImpedimentoUnidade = new RN.PrestacaoContas.MotivoImpedimento();

            motivoImpedimento.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivoImpedimento.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            motivoImpedimento.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            motivoImpedimento.UsuarioId = User.Identity.Name;

            validacao = rnMotivoImpedimentoUnidade.Valida(motivoImpedimento, true);

            if (validacao.Valido)
            {
                rnMotivoImpedimentoUnidade.Insere(motivoImpedimento);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoImpedimentoUnidade.DataBind();

        }

        protected void grdMotivoImpedimentoUnidade_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.MotivoImpedimento motivoImpedimento = new Techne.Lyceum.RN.PrestacaoContas.Entidades.MotivoImpedimento();
            RN.PrestacaoContas.MotivoImpedimento rnMotivoImpedimentoUnidade = new RN.PrestacaoContas.MotivoImpedimento();

            motivoImpedimento.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivoImpedimento.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            motivoImpedimento.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            motivoImpedimento.MotivoImpedimentoId = Convert.ToInt32(e.Keys["MOTIVOIMPEDIMENTOID"]);
            motivoImpedimento.UsuarioId = User.Identity.Name;

            validacao = rnMotivoImpedimentoUnidade.Valida(motivoImpedimento, true);

            if (validacao.Valido)
            {
                rnMotivoImpedimentoUnidade.Atualiza(motivoImpedimento);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoImpedimentoUnidade.DataBind();
        }

        protected void grdMotivoImpedimentoUnidade_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.MotivoImpedimento rnMotivoImpedimentoUnidade = new RN.PrestacaoContas.MotivoImpedimento();
            int motivoImpedimentoId = 0;

            motivoImpedimentoId = Convert.ToInt32(e.Keys["MOTIVOIMPEDIMENTOID"]);

            validacao = rnMotivoImpedimentoUnidade.ValidaRemocao(motivoImpedimentoId);

            if (validacao.Valido)
            {
                rnMotivoImpedimentoUnidade.Remove(motivoImpedimentoId);
                grdMotivoImpedimentoUnidade.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

    }
}
