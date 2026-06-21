using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Certificacao
{
    [NavUrl("~/Certificacao/TipoCertificacaoUnidadeCertificadora.aspx")]
    [ControlText("TipoCertificacaoUnidadeCertificadora")]
    [Title("Associação entre Tipo e Unidade")]
    public partial class TipoCertificacaoUnidadeCertificadora : TPage
    {
        RN.Certificacao.TipoCertificacaoUnidadeCertificadora rnTcuc = new Techne.Lyceum.RN.Certificacao.TipoCertificacaoUnidadeCertificadora();
        RN.Certificacao.TipoCertificacao rnTipoCertificacao = new Techne.Lyceum.RN.Certificacao.TipoCertificacao();
        RN.Certificacao.UnidadeCertificadora rnUnidadeCertificadora = new Techne.Lyceum.RN.Certificacao.UnidadeCertificadora();

        public object ListaTipoCertificacao() 
        {
            return rnTipoCertificacao.ListaAtivoPor();
        }

        public object ListaUnidadeCertificadora()
        {
            return rnUnidadeCertificadora.Lista();
        }

        public object Lista()
        {
            return rnTcuc.Lista();
        }
        public void Insert(object TIPOCERTIFICACAOID, object UNIDADECERTIFICADORAID) { }
        public void Delete(object TIPOCERTIFICACAOID, object UNIDADECERTIFICADORAID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTipoCertificacao, "Associação entre Tipo e Unidade");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTipoCertificacao);
        }

        protected void grdTipoCertificacao_OnAfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdTipoCertificacao);
        }

        protected void grdTipoCertificacao_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdTipoCertificacao.Settings.ShowFilterRow = false;
        }

        protected void grdTipoCertificacao_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Certificacao.Entidades.TipoCertificacaoUnidadeCertificadora tcuc = new Techne.Lyceum.RN.Certificacao.Entidades.TipoCertificacaoUnidadeCertificadora();

            tcuc.TipoCertificacaoId = e.NewValues["TIPOCERTIFICACAOID"] != null ? Convert.ToInt32(e.NewValues["TIPOCERTIFICACAOID"]) : 0;
            tcuc.UnidadeCertificadoraId = e.NewValues["UNIDADECERTIFICADORAID"] != null ? Convert.ToInt32(e.NewValues["UNIDADECERTIFICADORAID"]) : 0;
            tcuc.UsuarioId = User.Identity.Name;

            validacao = rnTcuc.Valida(tcuc);

            if (validacao.Valido)
            {
                rnTcuc.Insere(tcuc);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoCertificacao.DataBind();

        }

        protected void grdTipoCertificacao_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            
            int tipoCertificacaoId = Convert.ToInt32(e.Keys["TIPOCERTIFICACAOID"] ?? "0");
            int unidadeCertificadoraId = Convert.ToInt32(e.Keys["UNIDADECERTIFICADORAID"] ?? "0");

            validacao = rnTcuc.ValidaRemocao(tipoCertificacaoId, unidadeCertificadoraId);

            if (validacao.Valido)
            {
                rnTcuc.Remove(tipoCertificacaoId, unidadeCertificadoraId);
                grdTipoCertificacao.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
