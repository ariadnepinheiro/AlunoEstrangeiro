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
         NavUrl("~/PrestacaoContas/TipoProdutoServico.aspx"),
         ControlText("TipoProdutoServico"),
         Title("Tipo Produto Serviço")
     ]
    public partial class TipoProdutoServico : TPage
    {
        public object Lista()
        {
            RN.PrestacaoContas.TipoProdutoServico rnTipoProdutoServico = new Techne.Lyceum.RN.PrestacaoContas.TipoProdutoServico();

            return rnTipoProdutoServico.Lista();

        }

        public void Insert(object DESCRICAO,object ATIVO) { }
        public void Update(object DESCRICAO,object ATIVO, object TIPOPRODUTOSERVICOID) { }
        public void Delete(object TIPOPRODUTOSERVICOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTipoProdutoServico, "Tipo Produto Serviço");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTipoProdutoServico);
        }

        protected void grdTipoProdutoServico_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdTipoProdutoServico);
        }		


        protected void grdTipoProdutoServico_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTipoProdutoServico.Settings.ShowFilterRow = false;
        }

        protected void grdTipoProdutoServico_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdTipoProdutoServico.Settings.ShowFilterRow = false;
        }

        protected void grdTipoProdutoServico_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.TipoProdutoServico tipoProduto = new Techne.Lyceum.RN.PrestacaoContas.Entidades.TipoProdutoServico();
            RN.PrestacaoContas.TipoProdutoServico rnTipoProdutoServico = new RN.PrestacaoContas.TipoProdutoServico();

            tipoProduto.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipoProduto.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            tipoProduto.UsuarioId = User.Identity.Name;

            validacao = rnTipoProdutoServico.Valida(tipoProduto, true);

            if (validacao.Valido)
            {
                rnTipoProdutoServico.Insere(tipoProduto);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoProdutoServico.DataBind();

        }

        protected void grdTipoProdutoServico_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.TipoProdutoServico tipoProduto = new Techne.Lyceum.RN.PrestacaoContas.Entidades.TipoProdutoServico();
            RN.PrestacaoContas.TipoProdutoServico rnTipoProdutoServico = new RN.PrestacaoContas.TipoProdutoServico();

            tipoProduto.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipoProduto.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            tipoProduto.TipoProdutoServicoId = Convert.ToInt32(e.Keys["TIPOPRODUTOSERVICOID"]);
            tipoProduto.UsuarioId = User.Identity.Name;

            validacao = rnTipoProdutoServico.Valida(tipoProduto, true);

            if (validacao.Valido)
            {
                rnTipoProdutoServico.Atualiza(tipoProduto);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoProdutoServico.DataBind();
        }

        protected void grdTipoProdutoServico_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.TipoProdutoServico rnTipoProdutoServico = new RN.PrestacaoContas.TipoProdutoServico();
            int tipoProdutoId = 0;

            tipoProdutoId = Convert.ToInt32(e.Keys["TIPOPRODUTOSERVICOID"]);

            validacao = rnTipoProdutoServico.ValidaRemocao(tipoProdutoId);

            if (validacao.Valido)
            {
                rnTipoProdutoServico.Remove(tipoProdutoId);
                grdTipoProdutoServico.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }


    }
}
