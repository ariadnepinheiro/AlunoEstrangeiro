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
         NavUrl("~/Patrimonio/MotivoBaixa.aspx"),
         ControlText("Motivo Baixa"),
         Title("Motivo Baixa")
     ]
    public partial class MotivoBaixa : TPage
    {
         public object Lista()
        {
            RN.Patrimonio.MotivoBaixa rnMotivoBaixa = new Techne.Lyceum.RN.Patrimonio.MotivoBaixa();

            return rnMotivoBaixa.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object MOTIVOBAIXAID) { }
        public void Delete(object MOTIVOBAIXAID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdMotivoBaixa, "Motivo Baixa");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdMotivoBaixa);
        }

        protected void grdMotivoBaixa_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdMotivoBaixa);
        }

        protected void grdMotivoBaixa_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdMotivoBaixa.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoBaixa_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdMotivoBaixa.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoBaixa_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Patrimonio.Entidades.MotivoBaixa motivobaixa = new Techne.Lyceum.RN.Patrimonio.Entidades.MotivoBaixa();
            RN.Patrimonio.MotivoBaixa rnMotivoBaixa = new Techne.Lyceum.RN.Patrimonio.MotivoBaixa();

            motivobaixa.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivobaixa.Ativo = e.NewValues["ATIVO"] == null ? false : true;
            motivobaixa.UsuarioId = User.Identity.Name;

            validacao = rnMotivoBaixa.Valida(motivobaixa, true);

            if (validacao.Valido)
            {
                rnMotivoBaixa.Insere(motivobaixa);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoBaixa.DataBind();

        }

        protected void grdMotivoBaixa_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Patrimonio.Entidades.MotivoBaixa motivobaixa = new Techne.Lyceum.RN.Patrimonio.Entidades.MotivoBaixa();
            RN.Patrimonio.MotivoBaixa rnMotivoBaixa = new Techne.Lyceum.RN.Patrimonio.MotivoBaixa();

            motivobaixa.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivobaixa.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivobaixa.MotivoBaixaId = Convert.ToInt32(e.Keys["MOTIVOBAIXAID"]);
            motivobaixa.UsuarioId = User.Identity.Name;

            validacao = rnMotivoBaixa.Valida(motivobaixa, true);

            if (validacao.Valido)
            {
                rnMotivoBaixa.Atualiza(motivobaixa);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoBaixa.DataBind();
        }

        protected void grdMotivoBaixa_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Patrimonio.MotivoBaixa rnMotivoBaixa = new Techne.Lyceum.RN.Patrimonio.MotivoBaixa();
            int motivoBaixaId = 0;

            motivoBaixaId = Convert.ToInt32(e.Keys["MOTIVOBAIXAID"]);

            validacao = rnMotivoBaixa.ValidaRemocao(motivoBaixaId);

            if (validacao.Valido)
            {
                rnMotivoBaixa.Remove(motivoBaixaId);
                grdMotivoBaixa.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
