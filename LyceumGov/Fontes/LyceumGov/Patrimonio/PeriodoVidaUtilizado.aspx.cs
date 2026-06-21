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
 NavUrl("~/Patrimonio/PeriodoVidaUtilizado.aspx"),
 ControlText("Período Vida Útil(Já utilizado)"),
 Title("Período Vida Útil(Já utilizado)")
]
    public partial class PeriodoVidaUtilizado : TPage
    {
        public object Lista()
        {
            RN.Patrimonio.PeriodoVidaUtilizado rnPeriodoVidaUtilizado = new Techne.Lyceum.RN.Patrimonio.PeriodoVidaUtilizado();

            return rnPeriodoVidaUtilizado.Lista();
        }

        public void Insert(object CONCEITO, object QUANTIDADEANOS, object PONTUACAO, object ATIVO) { }
        public void Update(object CONCEITO, object QUANTIDADEANOS, object PONTUACAO, object ATIVO, object PERIODOVIDAUTILIZADOID) { }
        public void Delete(object PeriodoVidaUtilizadoID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdPeriodoVidaUtilizado, "Período Vida Utilizado");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdPeriodoVidaUtilizado);
        }

        protected void grdPeriodoVidaUtilizado_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdPeriodoVidaUtilizado);
        }
		
        protected void grdPeriodoVidaUtilizado_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdPeriodoVidaUtilizado.Settings.ShowFilterRow = false;
        }

        protected void grdPeriodoVidaUtilizado_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdPeriodoVidaUtilizado.Settings.ShowFilterRow = false;
        }

        protected void grdPeriodoVidaUtilizado_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Patrimonio.Entidades.PeriodoVidaUtilizado periodoVidaUtilizado = new Techne.Lyceum.RN.Patrimonio.Entidades.PeriodoVidaUtilizado();
            RN.Patrimonio.PeriodoVidaUtilizado rnPeriodoVidaUtilizado = new Techne.Lyceum.RN.Patrimonio.PeriodoVidaUtilizado();

            periodoVidaUtilizado.Conceito = e.NewValues["CONCEITO"] != null ? e.NewValues["CONCEITO"].ToString().Trim().ToUpper() : null;
            periodoVidaUtilizado.QuantidadeAnos = e.NewValues["QUANTIDADEANOS"] != null ? Convert.ToInt32(e.NewValues["QUANTIDADEANOS"]) : -1;
            periodoVidaUtilizado.Pontuacao = e.NewValues["PONTUACAO"] != null ? Convert.ToInt32(e.NewValues["PONTUACAO"]) : -1;
            periodoVidaUtilizado.Ativo = e.NewValues["ATIVO"] == null ? false : true;
            periodoVidaUtilizado.UsuarioId = User.Identity.Name;

            validacao = rnPeriodoVidaUtilizado.Valida(periodoVidaUtilizado, true);

            if (validacao.Valido)
            {
                rnPeriodoVidaUtilizado.Insere(periodoVidaUtilizado);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdPeriodoVidaUtilizado.DataBind();

        }

        protected void grdPeriodoVidaUtilizado_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Patrimonio.Entidades.PeriodoVidaUtilizado periodoVidaUtilizado = new Techne.Lyceum.RN.Patrimonio.Entidades.PeriodoVidaUtilizado();
            RN.Patrimonio.PeriodoVidaUtilizado rnPeriodoVidaUtilizado = new Techne.Lyceum.RN.Patrimonio.PeriodoVidaUtilizado();

            periodoVidaUtilizado.Conceito = e.NewValues["CONCEITO"] != null ? e.NewValues["CONCEITO"].ToString().Trim().ToUpper() : null;
            periodoVidaUtilizado.QuantidadeAnos = e.NewValues["QUANTIDADEANOS"] != null ? Convert.ToInt32(e.NewValues["QUANTIDADEANOS"]) : -1;
            periodoVidaUtilizado.Pontuacao = e.NewValues["PONTUACAO"] != null ? Convert.ToInt32(e.NewValues["PONTUACAO"]) : -1;
            periodoVidaUtilizado.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            periodoVidaUtilizado.PeriodoVidaUtilizadoId = Convert.ToInt32(e.Keys["PERIODOVIDAUTILIZADOID"]);
            periodoVidaUtilizado.UsuarioId = User.Identity.Name;

            validacao = rnPeriodoVidaUtilizado.Valida(periodoVidaUtilizado, true);

            if (validacao.Valido)
            {
                rnPeriodoVidaUtilizado.Atualiza(periodoVidaUtilizado);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdPeriodoVidaUtilizado.DataBind();
        }

        protected void grdPeriodoVidaUtilizado_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Patrimonio.PeriodoVidaUtilizado rnPeriodoVidaUtilizado = new Techne.Lyceum.RN.Patrimonio.PeriodoVidaUtilizado();
            int PeriodoVidaUtilizadoId = 0;

            PeriodoVidaUtilizadoId = Convert.ToInt32(e.Keys["PERIODOVIDAUTILIZADOID"]);

            rnPeriodoVidaUtilizado.Remove(PeriodoVidaUtilizadoId);
            grdPeriodoVidaUtilizado.DataBind();

        }
    }
}
