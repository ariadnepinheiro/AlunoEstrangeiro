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
         NavUrl("~/Patrimonio/Operacao.aspx"),
         ControlText("Operação"),
         Title("Operação")
     ]

    public partial class Operacao : TPage
    {        
        public object Lista()
        {
            RN.Patrimonio.Operacao rnOperacao = new Techne.Lyceum.RN.Patrimonio.Operacao();

            return rnOperacao.Lista();
        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object OperacaoID) { }
        public void Delete(object OperacaoID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdOperacao, "Operação");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdOperacao);
        }

        protected void grdOperacao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdOperacao);
        }
		

        protected void grdOperacao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdOperacao.Settings.ShowFilterRow = false;
        }

        protected void grdOperacao_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdOperacao.Settings.ShowFilterRow = false;
        }

        protected void grdOperacao_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Patrimonio.Entidades.Operacao Operacao = new Techne.Lyceum.RN.Patrimonio.Entidades.Operacao();
            RN.Patrimonio.Operacao rnOperacao = new Techne.Lyceum.RN.Patrimonio.Operacao();

            Operacao.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            Operacao.Ativo = e.NewValues["ATIVO"] == null ? false : true;
            Operacao.UsuarioId = User.Identity.Name;

            validacao = rnOperacao.Valida(Operacao, true);

            if (validacao.Valido)
            {
                rnOperacao.Insere(Operacao);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdOperacao.DataBind();

        }

        protected void grdOperacao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Patrimonio.Entidades.Operacao Operacao = new Techne.Lyceum.RN.Patrimonio.Entidades.Operacao();
            RN.Patrimonio.Operacao rnOperacao = new Techne.Lyceum.RN.Patrimonio.Operacao();

            Operacao.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            Operacao.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            Operacao.OperacaoId = Convert.ToInt32(e.Keys["OPERACAOID"]);
            Operacao.UsuarioId = User.Identity.Name;

            validacao = rnOperacao.Valida(Operacao, true);

            if (validacao.Valido)
            {
                rnOperacao.Atualiza(Operacao);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdOperacao.DataBind();
        }

        protected void grdOperacao_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Patrimonio.Operacao rnOperacao = new Techne.Lyceum.RN.Patrimonio.Operacao();
            int OperacaoId = 0;

            OperacaoId = Convert.ToInt32(e.Keys["OPERACAOID"]);

            validacao = rnOperacao.ValidaRemocao(OperacaoId);

            if (validacao.Valido)
            {
                rnOperacao.Remove(OperacaoId);
                grdOperacao.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
