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
    [
         NavUrl("~/Certificacao/GrupoUnidadeCertificadora.aspx"),
         ControlText("GrupoUnidadeCertificadora"),
         Title("Grupo Unidade Certificadora")
     ]

    public partial class GrupoUnidadeCertificadora : TPage
    {
        public object Lista()
        {
            RN.Certificacao.GrupoUnidadeCertificadora rnGrupoUnidadeCertificadora = new Techne.Lyceum.RN.Certificacao.GrupoUnidadeCertificadora();

            return rnGrupoUnidadeCertificadora.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object GRUPOUNIDADECERTIFICADORAID) { }
        public void Delete(object GRUPOUNIDADECERTIFICADORAID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdGrupoUnidadeCertificadora, "Grupo de Unidade Certificadora");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdGrupoUnidadeCertificadora);
        }

        protected void grdGrupoUnidadeCertificadora_OnAfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdGrupoUnidadeCertificadora);
        }

        protected void grdGrupoUnidadeCertificadora_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdGrupoUnidadeCertificadora.Settings.ShowFilterRow = false;
        }

        protected void grdGrupoUnidadeCertificadora_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdGrupoUnidadeCertificadora.Settings.ShowFilterRow = false;
        }

        protected void grdGrupoUnidadeCertificadora_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Certificacao.Entidades.GrupoUnidadeCertificadora grupo = new Techne.Lyceum.RN.Certificacao.Entidades.GrupoUnidadeCertificadora();
            RN.Certificacao.GrupoUnidadeCertificadora rnGrupoUnidadeCertificadora = new RN.Certificacao.GrupoUnidadeCertificadora();

            grupo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            grupo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            grupo.UsuarioId = User.Identity.Name;

            validacao = rnGrupoUnidadeCertificadora.Valida(grupo, true);

            if (validacao.Valido)
            {
                rnGrupoUnidadeCertificadora.Insere(grupo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdGrupoUnidadeCertificadora.DataBind();

        }

        protected void grdGrupoUnidadeCertificadora_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Certificacao.Entidades.GrupoUnidadeCertificadora grupo = new Techne.Lyceum.RN.Certificacao.Entidades.GrupoUnidadeCertificadora();
            RN.Certificacao.GrupoUnidadeCertificadora rnGrupoUnidadeCertificadora = new RN.Certificacao.GrupoUnidadeCertificadora();

            grupo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            grupo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            grupo.GrupoUnidadeCertificadoraId = Convert.ToInt32(e.Keys["GRUPOUNIDADECERTIFICADORAID"]);
            grupo.UsuarioId = User.Identity.Name;

            validacao = rnGrupoUnidadeCertificadora.Valida(grupo, true);

            if (validacao.Valido)
            {
                rnGrupoUnidadeCertificadora.Atualiza(grupo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdGrupoUnidadeCertificadora.DataBind();
        }

        protected void grdGrupoUnidadeCertificadora_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Certificacao.GrupoUnidadeCertificadora rnGrupoUnidadeCertificadora = new RN.Certificacao.GrupoUnidadeCertificadora();
            int grupoId = 0;

            grupoId = Convert.ToInt32(e.Keys["GRUPOUNIDADECERTIFICADORAID"]);

            validacao = rnGrupoUnidadeCertificadora.ValidaRemocao(grupoId);

            if (validacao.Valido)
            {
                rnGrupoUnidadeCertificadora.Remove(grupoId);
                grdGrupoUnidadeCertificadora.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
