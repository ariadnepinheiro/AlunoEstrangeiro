using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Matriculas;
using Techne.Web;
using DevExpress.Web.ASPxGridView;


namespace Techne.Lyceum.Net.Cadastros
{
    [NavUrl("~/Cadastros/MaeMotivoNaoHabilitado.aspx")]
    [ControlText("Motivo Nao Habilitado ")]
    [Title("Motivo Não Habilitado")]

    public partial class MaeMotivoNaoHabilitado : TPage
    {
        public object Lista()
        {
            RN.Cadastros.MaeMotivoNaoHabilitado rnMaeMotivoNaoHabilitado = new Techne.Lyceum.RN.Cadastros.MaeMotivoNaoHabilitado();

            return rnMaeMotivoNaoHabilitado.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object MAE_MOTIVONAOHABILITADOID) { }
        public void Delete(object MAE_MOTIVONAOHABILITADOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdMaeMotivoNaoHabilitado, "Motivo Não Habilitado");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdMaeMotivoNaoHabilitado);
        }

        protected void grdMaeMotivoNaoHabilitado_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdMaeMotivoNaoHabilitado);
        }

        protected void grdMaeMotivoNaoHabilitado_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdMaeMotivoNaoHabilitado.Settings.ShowFilterRow = false;
        }

        protected void grdMaeMotivoNaoHabilitado_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdMaeMotivoNaoHabilitado.Settings.ShowFilterRow = false;
        }

        protected void grdMaeMotivoNaoHabilitado_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Cadastros.Entidades.MaeMotivoNaoHabilitado motivo = new Techne.Lyceum.RN.Cadastros.Entidades.MaeMotivoNaoHabilitado();
            RN.Cadastros.MaeMotivoNaoHabilitado rnMaeMotivoNaoHabilitado = new Techne.Lyceum.RN.Cadastros.MaeMotivoNaoHabilitado();

            motivo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivo.UsuarioId = User.Identity.Name;

            validacao = rnMaeMotivoNaoHabilitado.Valida(motivo, true);

            if (validacao.Valido)
            {
                rnMaeMotivoNaoHabilitado.Insere(motivo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMaeMotivoNaoHabilitado.DataBind();

        }

        protected void grdMaeMotivoNaoHabilitado_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Cadastros.Entidades.MaeMotivoNaoHabilitado motivo = new Techne.Lyceum.RN.Cadastros.Entidades.MaeMotivoNaoHabilitado();
            RN.Cadastros.MaeMotivoNaoHabilitado rnMaeMotivoNaoHabilitado = new Techne.Lyceum.RN.Cadastros.MaeMotivoNaoHabilitado();

            motivo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivo.UsuarioId = User.Identity.Name;
            motivo.MaeMotivoNaoHabilitadoId = Convert.ToInt32(e.Keys["MAE_MOTIVONAOHABILITADOID"]);


            validacao = rnMaeMotivoNaoHabilitado.Valida(motivo, true);

            if (validacao.Valido)
            {
                rnMaeMotivoNaoHabilitado.Atualiza(motivo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMaeMotivoNaoHabilitado.DataBind();
        }

        protected void grdMaeMotivoNaoHabilitado_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Cadastros.MaeMotivoNaoHabilitado rnMaeMotivoNaoHabilitado = new Techne.Lyceum.RN.Cadastros.MaeMotivoNaoHabilitado();
            int MotivoRetornoInscricaoId = 0;

            MotivoRetornoInscricaoId = Convert.ToInt32(e.Keys["MAE_MOTIVONAOHABILITADOID"]);

            validacao = rnMaeMotivoNaoHabilitado.ValidaRemocao(MotivoRetornoInscricaoId);

            if (validacao.Valido)
            {
                rnMaeMotivoNaoHabilitado.Remove(MotivoRetornoInscricaoId);
                grdMaeMotivoNaoHabilitado.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

    }
}
