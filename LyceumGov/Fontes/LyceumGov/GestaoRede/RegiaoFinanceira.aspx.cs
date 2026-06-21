
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.GestaoRede
{
    [
         NavUrl("~/GestaoRede/RegiaoFinanceira.aspx"),
         ControlText("Região Geográfica / Financeira"),
         Title("Região Geográfica / Financeira")
     ]
    public partial class RegiaoFinanceira : TPage
    {
        public object Lista()
        {
            RN.GestaoRede.RegiaoFinanceira rnRegiaoFinanceira = new Techne.Lyceum.RN.GestaoRede.RegiaoFinanceira();

            return rnRegiaoFinanceira.Lista();

        }

        public void Insert(object DESCRICAO, object CODIGOCG) { }

        public void Update(object DESCRICAO, object CODIGOCG, object REGIAOFINANCEIRAID) { }

        public void Delete(object REGIAOFINANCEIRAID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdRegiaoFinanceira, "Região Geográfica / Financeira");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdRegiaoFinanceira);
        }

        protected void grdRegiaoFinanceira_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdRegiaoFinanceira.Settings.ShowFilterRow = false;
        }

        protected void grdRegiaoFinanceira_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdRegiaoFinanceira.Settings.ShowFilterRow = false;
        }

        protected void grdRegiaoFinanceira_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.GestaoRede.Entidades.RegiaoFinanceira regiaoFinanceira = new Techne.Lyceum.RN.GestaoRede.Entidades.RegiaoFinanceira();
            RN.GestaoRede.RegiaoFinanceira rnRegiaoFinanceira = new Techne.Lyceum.RN.GestaoRede.RegiaoFinanceira();

            regiaoFinanceira.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            regiaoFinanceira.CodigoCg = e.NewValues["CODIGOCG"] != null ? e.NewValues["CODIGOCG"].ToString().Trim().ToUpper() : null;
            regiaoFinanceira.UsuarioId = User.Identity.Name;

            validacao = rnRegiaoFinanceira.Valida(regiaoFinanceira, true);

            if (validacao.Valido)
            {
                rnRegiaoFinanceira.Insere(regiaoFinanceira);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdRegiaoFinanceira.DataBind();

        }

        protected void grdRegiaoFinanceira_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.GestaoRede.Entidades.RegiaoFinanceira regiaoFinanceira = new Techne.Lyceum.RN.GestaoRede.Entidades.RegiaoFinanceira();
            RN.GestaoRede.RegiaoFinanceira rnRegiaoFinanceira = new Techne.Lyceum.RN.GestaoRede.RegiaoFinanceira();

            regiaoFinanceira.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            regiaoFinanceira.CodigoCg = e.NewValues["CODIGOCG"] != null ? e.NewValues["CODIGOCG"].ToString().Trim().ToUpper() : null;
            regiaoFinanceira.RegiaoFinanceiraId = Convert.ToInt32(e.Keys["REGIAOFINANCEIRAID"]);
            regiaoFinanceira.UsuarioId = User.Identity.Name;

            validacao = rnRegiaoFinanceira.Valida(regiaoFinanceira, true);

            if (validacao.Valido)
            {
                rnRegiaoFinanceira.Atualiza(regiaoFinanceira);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdRegiaoFinanceira.DataBind();
        }

        protected void grdRegiaoFinanceira_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.GestaoRede.RegiaoFinanceira rnRegiaoFinanceira = new Techne.Lyceum.RN.GestaoRede.RegiaoFinanceira();
            int regiaoFinanceiraId = 0;

            regiaoFinanceiraId = Convert.ToInt32(e.Keys["REGIAOFINANCEIRAID"]);

            validacao = rnRegiaoFinanceira.ValidaRemocao(regiaoFinanceiraId);

            if (validacao.Valido)
            {
                rnRegiaoFinanceira.Remove(regiaoFinanceiraId);
                grdRegiaoFinanceira.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}

