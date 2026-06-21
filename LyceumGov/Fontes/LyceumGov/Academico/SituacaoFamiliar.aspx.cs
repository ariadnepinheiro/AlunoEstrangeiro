using System;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/SituacaoFamiliar.aspx")]
    [ControlText("Situação Familiar")]
    [Title("Situação Familiar")]

    public partial class SituacaoFamiliar : TPage
    {
        public object Lista()
        {
            RN.Turmas.SituacaoFamiliar rnSituacaoFamiliar = new Techne.Lyceum.RN.Turmas.SituacaoFamiliar();

            return rnSituacaoFamiliar.Lista();
        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object SITUACAOFAMILIARID) { }
        public void Delete(object SITUACAOFAMILIARID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdSituacaoFamiliar, "Situação Familiar/Necessidades Verificadas");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdSituacaoFamiliar);
        }

        protected void grdSituacaoFamiliar_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdSituacaoFamiliar);
        }

        protected void grdSituacaoFamiliar_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdSituacaoFamiliar.Settings.ShowFilterRow = false;
        }

        protected void grdSituacaoFamiliar_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdSituacaoFamiliar.Settings.ShowFilterRow = false;
        }

        protected void grdSituacaoFamiliar_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Turmas.Entidades.SituacaoFamiliar medida = new Techne.Lyceum.RN.Turmas.Entidades.SituacaoFamiliar();
            RN.Turmas.SituacaoFamiliar rnSituacaoFamiliar = new Techne.Lyceum.RN.Turmas.SituacaoFamiliar();

            medida.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            medida.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            medida.UsuarioId = User.Identity.Name;

            validacao = rnSituacaoFamiliar.Valida(medida, true);

            if (validacao.Valido)
            {
                rnSituacaoFamiliar.Insere(medida);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdSituacaoFamiliar.DataBind();

        }

        protected void grdSituacaoFamiliar_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Turmas.Entidades.SituacaoFamiliar medida = new Techne.Lyceum.RN.Turmas.Entidades.SituacaoFamiliar();
            RN.Turmas.SituacaoFamiliar rnSituacaoFamiliar = new Techne.Lyceum.RN.Turmas.SituacaoFamiliar();

            medida.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            medida.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            medida.UsuarioId = User.Identity.Name;
            medida.SituacaoFamiliarId = Convert.ToInt32(e.Keys["SITUACAOFAMILIARID"]);

            validacao = rnSituacaoFamiliar.Valida(medida, true);

            if (validacao.Valido)
            {
                rnSituacaoFamiliar.Atualiza(medida);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdSituacaoFamiliar.DataBind();
        }

        protected void grdSituacaoFamiliar_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Turmas.SituacaoFamiliar rnSituacaoFamiliar = new Techne.Lyceum.RN.Turmas.SituacaoFamiliar();
            int Id = 0;

            Id = Convert.ToInt32(e.Keys["SITUACAOFAMILIARID"]);

            validacao = rnSituacaoFamiliar.ValidaRemocao(Id);

            if (validacao.Valido)
            {
                rnSituacaoFamiliar.Remove(Id);
                grdSituacaoFamiliar.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

    }
}
