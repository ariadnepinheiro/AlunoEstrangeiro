using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;


namespace Techne.Lyceum.Net.Interconectividade
{
    [
         NavUrl("~/Interconectividade/UnidadeVelocidade.aspx"),
         ControlText("UnidadeVelocidade"),
         Title("Unidade Velocidade")
     ]
    public partial class UnidadeVelocidade : TPage
    {
        public object Lista()
        {
            RN.FiscalizacaoLink.UnidadeVelocidade rnUnidadeVelocidade = new Techne.Lyceum.RN.FiscalizacaoLink.UnidadeVelocidade();

            return rnUnidadeVelocidade.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object UNIDADEVELOCIDADEID) { }
        public void Delete(object UNIDADEVELOCIDADEID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdUnidadeVelocidade, "Unidade Velocidade");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdUnidadeVelocidade);
        }

        protected void grdUnidadeVelocidade_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdUnidadeVelocidade.Settings.ShowFilterRow = false;
        }

        protected void grdUnidadeVelocidade_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdUnidadeVelocidade.Settings.ShowFilterRow = false;
        }

        protected void grdUnidadeVelocidade_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.FiscalizacaoLink.Entidades.UnidadeVelocidade unidadeVelocidade = new Techne.Lyceum.RN.FiscalizacaoLink.Entidades.UnidadeVelocidade();
            RN.FiscalizacaoLink.UnidadeVelocidade rnUnidadeVelocidade = new RN.FiscalizacaoLink.UnidadeVelocidade();

            unidadeVelocidade.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            unidadeVelocidade.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            unidadeVelocidade.UsuarioId = User.Identity.Name;

            validacao = rnUnidadeVelocidade.Valida(unidadeVelocidade, true);

            if (validacao.Valido)
            {
                rnUnidadeVelocidade.Insere(unidadeVelocidade);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdUnidadeVelocidade.DataBind();

        }

        protected void grdUnidadeVelocidade_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.FiscalizacaoLink.Entidades.UnidadeVelocidade unidadeVelocidade = new Techne.Lyceum.RN.FiscalizacaoLink.Entidades.UnidadeVelocidade();
            RN.FiscalizacaoLink.UnidadeVelocidade rnUnidadeVelocidade = new RN.FiscalizacaoLink.UnidadeVelocidade();

            unidadeVelocidade.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            unidadeVelocidade.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            unidadeVelocidade.UnidadeVelocidadeId = Convert.ToInt32(e.Keys["UNIDADEVELOCIDADEID"]);
            unidadeVelocidade.UsuarioId = User.Identity.Name;

            validacao = rnUnidadeVelocidade.Valida(unidadeVelocidade, true);

            if (validacao.Valido)
            {
                rnUnidadeVelocidade.Atualiza(unidadeVelocidade);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdUnidadeVelocidade.DataBind();
        }

        protected void grdUnidadeVelocidade_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.FiscalizacaoLink.UnidadeVelocidade rnUnidadeVelocidade = new RN.FiscalizacaoLink.UnidadeVelocidade();
            int unidadeVelocidadeId = 0;

            unidadeVelocidadeId = Convert.ToInt32(e.Keys["UNIDADEVELOCIDADEID"]);

            validacao = rnUnidadeVelocidade.ValidaRemocao(unidadeVelocidadeId);

            if (validacao.Valido)
            {
                rnUnidadeVelocidade.Remove(unidadeVelocidadeId);
                grdUnidadeVelocidade.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
