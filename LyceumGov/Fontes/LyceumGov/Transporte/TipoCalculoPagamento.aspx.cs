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
    [NavUrl("~/Transporte/TipoCalculoPagamento.aspx")]
    [ControlText("Tipo Cálculo Pagamento")]
    [Title("Tipo Cálculo Pagamento")]

    public partial class TipoCalculoPagamento : TPage
    {
        public object Lista()
        {
            RN.Transporte.TipoCalculoPagamento rnTipoCalculoPagamento = new Techne.Lyceum.RN.Transporte.TipoCalculoPagamento();

            return rnTipoCalculoPagamento.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object TIPOCALCULOPAGAMENTOID) { }
        public void Delete(object TIPOCALCULOPAGAMENTOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTipoCalculoPagamento, "Tipo Cálculo de Pagamento");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTipoCalculoPagamento);
        }

        protected void grdTipoCalculoPagamento_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTipoCalculoPagamento.Settings.ShowFilterRow = false;
        }

        protected void grdTipoCalculoPagamento_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdTipoCalculoPagamento.Settings.ShowFilterRow = false;
        }

        protected void grdTipoCalculoPagamento_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Entidades.TipoCalculoPagamento tipoCalculoPagamento = new Techne.Lyceum.RN.Transporte.Entidades.TipoCalculoPagamento();
            RN.Transporte.TipoCalculoPagamento rnTipoCalculoPagamento = new Techne.Lyceum.RN.Transporte.TipoCalculoPagamento();

            tipoCalculoPagamento.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipoCalculoPagamento.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            tipoCalculoPagamento.UsuarioId = User.Identity.Name;

            validacao = rnTipoCalculoPagamento.Valida(tipoCalculoPagamento, true);

            if (validacao.Valido)
            {
                rnTipoCalculoPagamento.Insere(tipoCalculoPagamento);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoCalculoPagamento.DataBind();

        }

        protected void grdTipoCalculoPagamento_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Entidades.TipoCalculoPagamento tipoCalculoPagamento = new Techne.Lyceum.RN.Transporte.Entidades.TipoCalculoPagamento();
            RN.Transporte.TipoCalculoPagamento rnTipoCalculoPagamento = new Techne.Lyceum.RN.Transporte.TipoCalculoPagamento();

            tipoCalculoPagamento.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipoCalculoPagamento.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            tipoCalculoPagamento.UsuarioId = User.Identity.Name;
            tipoCalculoPagamento.TipoCalculoPagamentoId = Convert.ToInt32(e.Keys["TIPOCALCULOPAGAMENTOID"]);


            validacao = rnTipoCalculoPagamento.Valida(tipoCalculoPagamento, true);

            if (validacao.Valido)
            {
                rnTipoCalculoPagamento.Atualiza(tipoCalculoPagamento);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoCalculoPagamento.DataBind();
        }

        protected void grdTipoCalculoPagamento_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.TipoCalculoPagamento rnTipoCalculoPagamento = new Techne.Lyceum.RN.Transporte.TipoCalculoPagamento();
            int tipoCalculoPagamentoId = 0;

            tipoCalculoPagamentoId = Convert.ToInt32(e.Keys["TIPOCALCULOPAGAMENTOID"]);

            validacao = rnTipoCalculoPagamento.ValidaRemocao(tipoCalculoPagamentoId);

            if (validacao.Valido)
            {
                rnTipoCalculoPagamento.Remove(tipoCalculoPagamentoId);
                grdTipoCalculoPagamento.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
