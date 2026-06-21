using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Basico
{
    [
      NavUrl("~/Basico/TipoUsuarioExterno.aspx"),
      ControlText("Tipo Usuario Externo"),
      Title("Tipo Usuario Externo")
  ]
    public partial class TipoUsuarioExterno : TPage
    {
        public object ListaTipoUsuarioExterno()
        {
            RN.RecursosHumanos.TipoUsuarioExterno rnTipoUsuarioExterno = new Techne.Lyceum.RN.RecursosHumanos.TipoUsuarioExterno();

            return rnTipoUsuarioExterno.ListaTipoUsuarioExterno();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object TIPOUSUARIOEXTERNOID) { }
        public void Delete(object TIPOUSUARIOEXTERNOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTipoUsuarioExterno, "Tipo Basico");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTipoUsuarioExterno);
        }

        protected void grdTipoUsuarioExterno_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdTipoUsuarioExterno);
        }		

        protected void grdTipoUsuarioExterno_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTipoUsuarioExterno.Settings.ShowFilterRow = false;
        }

        protected void grdTipoUsuarioExterno_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdTipoUsuarioExterno.Settings.ShowFilterRow = false;
        }

        protected void grdTipoUsuarioExterno_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RecursosHumanos.Entidades.TipoUsuarioExterno TipoUsuarioExterno = new Techne.Lyceum.RN.RecursosHumanos.Entidades.TipoUsuarioExterno();
            RN.RecursosHumanos.TipoUsuarioExterno rnTipoUsuarioExterno = new RN.RecursosHumanos.TipoUsuarioExterno();

            TipoUsuarioExterno.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            TipoUsuarioExterno.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            TipoUsuarioExterno.UsuarioId = User.Identity.Name;

            validacao = rnTipoUsuarioExterno.Valida(TipoUsuarioExterno, true);

            if (validacao.Valido)
            {
                rnTipoUsuarioExterno.Insere(TipoUsuarioExterno);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoUsuarioExterno.DataBind();

        }

        protected void grdTipoUsuarioExterno_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RecursosHumanos.Entidades.TipoUsuarioExterno tipoUsuarioExterno = new Techne.Lyceum.RN.RecursosHumanos.Entidades.TipoUsuarioExterno();
            RN.RecursosHumanos.TipoUsuarioExterno rnTipoUsuarioExterno = new RN.RecursosHumanos.TipoUsuarioExterno();

            tipoUsuarioExterno.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipoUsuarioExterno.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            tipoUsuarioExterno.TipoUsuarioExternoId = Convert.ToInt32(e.Keys["TIPOUSUARIOEXTERNOID"]);
            tipoUsuarioExterno.UsuarioId = User.Identity.Name;

            validacao = rnTipoUsuarioExterno.Valida(tipoUsuarioExterno, true);

            if (validacao.Valido)
            {
                rnTipoUsuarioExterno.Atualiza(tipoUsuarioExterno);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoUsuarioExterno.DataBind();
        }

        protected void grdTipoUsuarioExterno_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RecursosHumanos.TipoUsuarioExterno rnTipoUsuarioExterno = new RN.RecursosHumanos.TipoUsuarioExterno();
            int tipoUsuarioExternoId = 0;

            tipoUsuarioExternoId = Convert.ToInt32(e.Keys["TIPOUSUARIOEXTERNOID"]);

            validacao = rnTipoUsuarioExterno.ValidaRemocao(tipoUsuarioExternoId);

            if (validacao.Valido)
            {
                rnTipoUsuarioExterno.Remove(tipoUsuarioExternoId);
                grdTipoUsuarioExterno.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
