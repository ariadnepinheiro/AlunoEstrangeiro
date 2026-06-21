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
    [NavUrl("~/Transporte/TipoContratacao.aspx")]
    [ControlText("Tipo de Contratação")]
    [Title("Tipo de Contratação")]

    public partial class TipoContratacao : TPage
    {
        public object Lista()
        {
            RN.Transporte.TipoContratacao rnTipoContratacao = new Techne.Lyceum.RN.Transporte.TipoContratacao();

            return rnTipoContratacao.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object TIPOCONTRATACAOID) { }
        public void Delete(object TIPOCONTRATACAOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTipoContratacao, "Tipo de Contratação");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTipoContratacao);
        }

        protected void grdTipoContratacao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTipoContratacao.Settings.ShowFilterRow = false;
        }

        protected void grdTipoContratacao_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdTipoContratacao.Settings.ShowFilterRow = false;
        }

        protected void grdTipoContratacao_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Entidades.TipoContratacao tipoContratacao = new Techne.Lyceum.RN.Transporte.Entidades.TipoContratacao();
            RN.Transporte.TipoContratacao rnTipoContratacao = new Techne.Lyceum.RN.Transporte.TipoContratacao();

            tipoContratacao.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipoContratacao.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            tipoContratacao.UsuarioId = User.Identity.Name;

            validacao = rnTipoContratacao.Valida(tipoContratacao, true);

            if (validacao.Valido)
            {
                rnTipoContratacao.Insere(tipoContratacao);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoContratacao.DataBind();

        }

        protected void grdTipoContratacao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Entidades.TipoContratacao tipoContratacao = new Techne.Lyceum.RN.Transporte.Entidades.TipoContratacao();
            RN.Transporte.TipoContratacao rnTipoContratacao = new Techne.Lyceum.RN.Transporte.TipoContratacao();

            tipoContratacao.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipoContratacao.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            tipoContratacao.UsuarioId = User.Identity.Name;
            tipoContratacao.TipoContratacaoId = Convert.ToInt32(e.Keys["TIPOCONTRATACAOID"]);


            validacao = rnTipoContratacao.Valida(tipoContratacao, true);

            if (validacao.Valido)
            {
                rnTipoContratacao.Atualiza(tipoContratacao);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoContratacao.DataBind();
        }

        protected void grdTipoContratacao_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.TipoContratacao rnTipoContratacao = new Techne.Lyceum.RN.Transporte.TipoContratacao();
            int tipoContratacaoId = 0;

            tipoContratacaoId = Convert.ToInt32(e.Keys["TIPOCONTRATACAOID"]);

            validacao = rnTipoContratacao.ValidaRemocao(tipoContratacaoId);

            if (validacao.Valido)
            {
                rnTipoContratacao.Remove(tipoContratacaoId);
                grdTipoContratacao.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
