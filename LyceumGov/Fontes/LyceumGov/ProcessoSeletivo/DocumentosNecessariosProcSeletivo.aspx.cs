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


namespace Techne.Lyceum.Net.ProcessoSeletivo
{
    [
         NavUrl("~/ProcessoSeletivo/DocumentosNecessariosProcSeletivo.aspx"),
         ControlText("DocumentosNecessariosProcSeletivo"),
         Title("Documentos Necessários ProcSeletivo")
     ]
    public partial class DocumentosNecessariosProcSeletivo : TPage
    {
        public object Lista()
        {
            RN.RecursosHumanos.TipoDocumento rnTipoDocumento = new Techne.Lyceum.RN.RecursosHumanos.TipoDocumento();

            return rnTipoDocumento.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object TIPODOCUMENTOID) { }
        public void Delete(object TIPODOCUMENTOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTipoDocumento, "Tipo Documento Necessário");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTipoDocumento);
        }

        protected void grdTipoDocumento_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdTipoDocumento);
        }

        protected void grdTipoDocumento_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTipoDocumento.Settings.ShowFilterRow = false;
        }

        protected void grdTipoDocumento_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdTipoDocumento.Settings.ShowFilterRow = false;
        }

        protected void grdTipoDocumento_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RecursosHumanos.Entidades.TipoDocumento tipo = new Techne.Lyceum.RN.RecursosHumanos.Entidades.TipoDocumento();
            RN.RecursosHumanos.TipoDocumento rnTipoDocumento = new RN.RecursosHumanos.TipoDocumento();

            tipo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            tipo.UsuarioId = User.Identity.Name;

            validacao = rnTipoDocumento.Valida(tipo, true);

            if (validacao.Valido)
            {
                rnTipoDocumento.Insere(tipo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoDocumento.DataBind();

        }

        protected void grdTipoDocumento_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RecursosHumanos.Entidades.TipoDocumento tipo = new Techne.Lyceum.RN.RecursosHumanos.Entidades.TipoDocumento();
            RN.RecursosHumanos.TipoDocumento rnTipoDocumento = new RN.RecursosHumanos.TipoDocumento();

            tipo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            tipo.TipoDocumentoId = Convert.ToInt32(e.Keys["TIPODOCUMENTOID"]);
            tipo.UsuarioId = User.Identity.Name;

            validacao = rnTipoDocumento.Valida(tipo, true);

            if (validacao.Valido)
            {
                rnTipoDocumento.Atualiza(tipo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoDocumento.DataBind();
        }

        protected void grdTipoDocumento_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RecursosHumanos.TipoDocumento rnTipoDocumento = new RN.RecursosHumanos.TipoDocumento();
            int tipoDocumentoId = 0;

            tipoDocumentoId = Convert.ToInt32(e.Keys["TIPODOCUMENTOID"]);

            validacao = rnTipoDocumento.ValidaRemocao(tipoDocumentoId);

            if (validacao.Valido)
            {
                rnTipoDocumento.Remove(tipoDocumentoId);
                grdTipoDocumento.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }



    }
}
