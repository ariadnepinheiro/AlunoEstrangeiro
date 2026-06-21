using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Transporte
{

    [NavUrl("~/Transporte/SituacaoPagamento.aspx")]
    [ControlText("Situação Pagamento")]
    [Title("Situação Pagamento")]

    public partial class SituacaoPagamento : TPage
    {
        public object Lista()
        {
            RN.Transporte.SituacaoPagamento rnSituacaoPagamento = new Techne.Lyceum.RN.Transporte.SituacaoPagamento();

            return rnSituacaoPagamento.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object SITUACAOPAGAMENTOID) { }
        public void Delete(object SITUACAOPAGAMENTOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdSituacaoPagamento, "Situação de Pagamento");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdSituacaoPagamento);
        }

        protected void grdSituacaoPagamento_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdSituacaoPagamento.Settings.ShowFilterRow = false;
        }

        protected void grdSituacaoPagamento_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdSituacaoPagamento.Settings.ShowFilterRow = false;
        }

        protected void grdSituacaoPagamento_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Entidades.SituacaoPagamento situacaoPagamento = new Techne.Lyceum.RN.Transporte.Entidades.SituacaoPagamento();
            RN.Transporte.SituacaoPagamento rnSituacaoPagamento = new Techne.Lyceum.RN.Transporte.SituacaoPagamento();

            situacaoPagamento.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            situacaoPagamento.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            situacaoPagamento.UsuarioId = User.Identity.Name;

            validacao = rnSituacaoPagamento.Valida(situacaoPagamento, true);

            if (validacao.Valido)
            {
                rnSituacaoPagamento.Insere(situacaoPagamento);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdSituacaoPagamento.DataBind();

        }

        protected void grdSituacaoPagamento_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Entidades.SituacaoPagamento situacaoPagamento = new Techne.Lyceum.RN.Transporte.Entidades.SituacaoPagamento();
            RN.Transporte.SituacaoPagamento rnSituacaoPagamento = new Techne.Lyceum.RN.Transporte.SituacaoPagamento();

            situacaoPagamento.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            situacaoPagamento.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            situacaoPagamento.UsuarioId = User.Identity.Name;
            situacaoPagamento.SituacaoPagamentoId = Convert.ToInt32(e.Keys["SITUACAOPAGAMENTOID"]);


            validacao = rnSituacaoPagamento.Valida(situacaoPagamento, true);

            if (validacao.Valido)
            {
                rnSituacaoPagamento.Atualiza(situacaoPagamento);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdSituacaoPagamento.DataBind();
        }

        protected void grdSituacaoPagamento_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.SituacaoPagamento rnSituacaoPagamento = new Techne.Lyceum.RN.Transporte.SituacaoPagamento();
            int situacaoPagamentoId = 0;

            situacaoPagamentoId = Convert.ToInt32(e.Keys["SITUACAOPAGAMENTOID"]);

            validacao = rnSituacaoPagamento.ValidaRemocao(situacaoPagamentoId);

            if (validacao.Valido)
            {
                rnSituacaoPagamento.Remove(situacaoPagamentoId);
                grdSituacaoPagamento.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
