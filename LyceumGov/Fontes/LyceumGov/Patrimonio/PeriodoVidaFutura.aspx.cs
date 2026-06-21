using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Patrimonio
{
    [
 NavUrl("~/Patrimonio/PeriodoVidaFutura.aspx"),
 ControlText("Período Vida Futura"),
 Title("Período Vida Futura")
]
    public partial class PeriodoVidaFutura : TPage
    {
        public object Lista()
        {
            RN.Patrimonio.PeriodoVidaFutura rnPeriodoVidaFutura = new Techne.Lyceum.RN.Patrimonio.PeriodoVidaFutura();

            return rnPeriodoVidaFutura.Lista();
        }

        public void Insert(object CONCEITO, object QUANTIDADEANOS, object PONTUACAO, object ATIVO) { }
        public void Update(object CONCEITO, object QUANTIDADEANOS, object PONTUACAO, object ATIVO, object PERIODOVIDAFUTURAID) { }
        public void Delete(object PeriodoVidaFuturaID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdPeriodoVidaFutura, "Período Vida Futura");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdPeriodoVidaFutura);
        }

        protected void grdPeriodoVidaFutura_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdPeriodoVidaFutura);
        }
		
        protected void grdPeriodoVidaFutura_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdPeriodoVidaFutura.Settings.ShowFilterRow = false;
        }

        protected void grdPeriodoVidaFutura_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdPeriodoVidaFutura.Settings.ShowFilterRow = false;
        }

        protected void grdPeriodoVidaFutura_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Patrimonio.Entidades.PeriodoVidaFutura periodoVidaFutura = new Techne.Lyceum.RN.Patrimonio.Entidades.PeriodoVidaFutura();
            RN.Patrimonio.PeriodoVidaFutura rnPeriodoVidaFutura = new Techne.Lyceum.RN.Patrimonio.PeriodoVidaFutura();

            periodoVidaFutura.Conceito = e.NewValues["CONCEITO"] != null ? e.NewValues["CONCEITO"].ToString().Trim().ToUpper() : null;
            periodoVidaFutura.QuantidadeAnos = e.NewValues["QUANTIDADEANOS"] != null ? Convert.ToInt32(e.NewValues["QUANTIDADEANOS"]) : -1;
            periodoVidaFutura.Pontuacao = e.NewValues["PONTUACAO"] != null ? Convert.ToInt32(e.NewValues["PONTUACAO"]) : -1;
            periodoVidaFutura.Ativo = e.NewValues["ATIVO"] == null ? false : true;
            periodoVidaFutura.UsuarioId = User.Identity.Name;

            validacao = rnPeriodoVidaFutura.Valida(periodoVidaFutura, true);

            if (validacao.Valido)
            {
                rnPeriodoVidaFutura.Insere(periodoVidaFutura);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdPeriodoVidaFutura.DataBind();

        }

        protected void grdPeriodoVidaFutura_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Patrimonio.Entidades.PeriodoVidaFutura periodoVidaFutura = new Techne.Lyceum.RN.Patrimonio.Entidades.PeriodoVidaFutura();
            RN.Patrimonio.PeriodoVidaFutura rnPeriodoVidaFutura = new Techne.Lyceum.RN.Patrimonio.PeriodoVidaFutura();

            periodoVidaFutura.Conceito = e.NewValues["CONCEITO"] != null ? e.NewValues["CONCEITO"].ToString().Trim().ToUpper() : null;
            periodoVidaFutura.QuantidadeAnos = e.NewValues["QUANTIDADEANOS"] != null ? Convert.ToInt32(e.NewValues["QUANTIDADEANOS"]) : -1;
            periodoVidaFutura.Pontuacao = e.NewValues["PONTUACAO"] != null ? Convert.ToInt32(e.NewValues["PONTUACAO"]) : -1;
            periodoVidaFutura.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            periodoVidaFutura.PeriodoVidaFuturaId = Convert.ToInt32(e.Keys["PERIODOVIDAFUTURAID"]);
            periodoVidaFutura.UsuarioId = User.Identity.Name;

            validacao = rnPeriodoVidaFutura.Valida(periodoVidaFutura, true);

            if (validacao.Valido)
            {
                rnPeriodoVidaFutura.Atualiza(periodoVidaFutura);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdPeriodoVidaFutura.DataBind();
        }

        protected void grdPeriodoVidaFutura_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Patrimonio.PeriodoVidaFutura rnPeriodoVidaFutura = new Techne.Lyceum.RN.Patrimonio.PeriodoVidaFutura();
            int PeriodoVidaFuturaId = 0;

            PeriodoVidaFuturaId = Convert.ToInt32(e.Keys["PERIODOVIDAFUTURAID"]);

            rnPeriodoVidaFutura.Remove(PeriodoVidaFuturaId);
            grdPeriodoVidaFutura.DataBind();

        }
    }
}
