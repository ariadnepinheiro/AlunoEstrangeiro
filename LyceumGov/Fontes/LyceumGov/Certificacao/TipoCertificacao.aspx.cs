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
    [NavUrl("~/Certificacao/TipoCertificacao.aspx")]
    [ControlText("TipoCertificacao")]
    [Title("Tipo de Certificação")]
    public partial class TipoCertificacao : TPage
    {
        public object Lista()
        {
            RN.Certificacao.TipoCertificacao rnTipoCertificacao = new Techne.Lyceum.RN.Certificacao.TipoCertificacao();

            return rnTipoCertificacao.Lista();
        }

        public void Insert(object DESCRICAO, object PERMITEPOLO, object PERMITECEJA, object PERMITETRANSPARENCIA, object ETAPAENSINO, object ATIVO) { }
        public void Update(object DESCRICAO, object PERMITEPOLO, object PERMITECEJA, object PERMITETRANSPARENCIA, object ETAPAENSINO, object ATIVO, object TIPOCERTIFICACAOID) { }
        public void Delete(object TIPOCERTIFICACAOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTipoCertificacao, "Tipo de Certificação");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTipoCertificacao);
        }

        protected void grdTipoCertificacao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTipoCertificacao.Settings.ShowFilterRow = false;
        }

        protected void grdTipoCertificacao_OnAfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdTipoCertificacao);
        }

        protected void grdTipoCertificacao_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdTipoCertificacao.Settings.ShowFilterRow = false;
        }

        protected void grdTipoCertificacao_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Certificacao.Entidades.TipoCertificacao tipoCertificacao = new Techne.Lyceum.RN.Certificacao.Entidades.TipoCertificacao();
            RN.Certificacao.TipoCertificacao rnTipoCertificacao = new RN.Certificacao.TipoCertificacao();

            tipoCertificacao.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipoCertificacao.PermitePolo = (e.NewValues["PERMITEPOLO"] == null || Convert.ToBoolean(e.NewValues["PERMITEPOLO"]) == false) ? false : true;
            tipoCertificacao.PermiteCeja = (e.NewValues["PERMITECEJA"] == null || Convert.ToBoolean(e.NewValues["PERMITECEJA"]) == false) ? false : true;
            tipoCertificacao.PermiteTransparencia = (e.NewValues["PERMITETRANSPARENCIA"] == null || Convert.ToBoolean(e.NewValues["PERMITETRANSPARENCIA"]) == false) ? false : true;
            tipoCertificacao.EtapaEnsino = e.NewValues["ETAPAENSINO"] != null ? e.NewValues["ETAPAENSINO"].ToString().Trim().ToUpper() : null;
            tipoCertificacao.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            tipoCertificacao.UsuarioId = User.Identity.Name;

            validacao = rnTipoCertificacao.Valida(tipoCertificacao, true);

            if (validacao.Valido)
            {
                rnTipoCertificacao.Insere(tipoCertificacao);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoCertificacao.DataBind();

        }

        protected void grdTipoCertificacao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Certificacao.Entidades.TipoCertificacao tipoCertificacao = new Techne.Lyceum.RN.Certificacao.Entidades.TipoCertificacao();
            RN.Certificacao.TipoCertificacao rnTipoCertificacao = new RN.Certificacao.TipoCertificacao();

            tipoCertificacao.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipoCertificacao.PermitePolo = (e.NewValues["PERMITEPOLO"] == null || Convert.ToBoolean(e.NewValues["PERMITEPOLO"]) == false) ? false : true;
            tipoCertificacao.PermiteCeja = (e.NewValues["PERMITECEJA"] == null || Convert.ToBoolean(e.NewValues["PERMITECEJA"]) == false) ? false : true;
            tipoCertificacao.PermiteTransparencia = (e.NewValues["PERMITETRANSPARENCIA"] == null || Convert.ToBoolean(e.NewValues["PERMITETRANSPARENCIA"]) == false) ? false : true;
            tipoCertificacao.EtapaEnsino = e.NewValues["ETAPAENSINO"] != null ? e.NewValues["ETAPAENSINO"].ToString().Trim().ToUpper() : null;
            tipoCertificacao.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            tipoCertificacao.TipoCertificacaoId = Convert.ToInt32(e.Keys["TIPOCERTIFICACAOID"]);
            tipoCertificacao.UsuarioId = User.Identity.Name;

            validacao = rnTipoCertificacao.Valida(tipoCertificacao, true);

            if (validacao.Valido)
            {
                rnTipoCertificacao.Atualiza(tipoCertificacao);
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
            RN.Certificacao.TipoCertificacao rnTipoCertificacao = new RN.Certificacao.TipoCertificacao();
            int tipoCertificacaoId = 0;

            tipoCertificacaoId = Convert.ToInt32(e.Keys["TIPOCERTIFICACAOID"]);

            validacao = rnTipoCertificacao.ValidaRemocao(tipoCertificacaoId);

            if (validacao.Valido)
            {
                rnTipoCertificacao.Remove(tipoCertificacaoId);
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
