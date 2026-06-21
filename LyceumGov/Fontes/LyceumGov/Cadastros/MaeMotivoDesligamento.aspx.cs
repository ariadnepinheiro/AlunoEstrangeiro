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
    [NavUrl("~/Cadastros/MaeMotivoDesligamento.aspx")]
    [ControlText("Motivo Desligamento")]
    [Title("Motivo Desligamento")]

    public partial class MaeMotivoDesligamento : TPage
    {
        public object Lista()
        {
            RN.Cadastros.MaeMotivoDesligamento rnMaeMotivoDesligamento = new Techne.Lyceum.RN.Cadastros.MaeMotivoDesligamento();

            return rnMaeMotivoDesligamento.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object MAE_MOTIVODESLIGAMENTOID) { }
        public void Delete(object MAE_MOTIVODESLIGAMENTOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdMaeMotivoDesligamento, "Motivo Desligamento");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdMaeMotivoDesligamento);
        }

        protected void grdMaeMotivoDesligamento_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdMaeMotivoDesligamento);
        }

        protected void grdMaeMotivoDesligamento_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdMaeMotivoDesligamento.Settings.ShowFilterRow = false;
        }

        protected void grdMaeMotivoDesligamento_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdMaeMotivoDesligamento.Settings.ShowFilterRow = false;
        }

        protected void grdMaeMotivoDesligamento_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Cadastros.Entidades.MaeMotivoDesligamento motivo = new Techne.Lyceum.RN.Cadastros.Entidades.MaeMotivoDesligamento();
            RN.Cadastros.MaeMotivoDesligamento rnMaeMotivoDesligamento = new Techne.Lyceum.RN.Cadastros.MaeMotivoDesligamento();

            motivo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivo.UsuarioId = User.Identity.Name;

            validacao = rnMaeMotivoDesligamento.Valida(motivo, true);

            if (validacao.Valido)
            {
                rnMaeMotivoDesligamento.Insere(motivo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMaeMotivoDesligamento.DataBind();

        }

        protected void grdMaeMotivoDesligamento_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Cadastros.Entidades.MaeMotivoDesligamento motivo = new Techne.Lyceum.RN.Cadastros.Entidades.MaeMotivoDesligamento();
            RN.Cadastros.MaeMotivoDesligamento rnMaeMotivoDesligamento = new Techne.Lyceum.RN.Cadastros.MaeMotivoDesligamento();

            motivo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivo.UsuarioId = User.Identity.Name;
            motivo.MaeMotivoDesligamentoId = Convert.ToInt32(e.Keys["MAE_MOTIVODESLIGAMENTOID"]);


            validacao = rnMaeMotivoDesligamento.Valida(motivo, true);

            if (validacao.Valido)
            {
                rnMaeMotivoDesligamento.Atualiza(motivo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMaeMotivoDesligamento.DataBind();
        }

        protected void grdMaeMotivoDesligamento_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Cadastros.MaeMotivoDesligamento rnMaeMotivoDesligamento = new Techne.Lyceum.RN.Cadastros.MaeMotivoDesligamento();
            int MotivoRetornoInscricaoId = 0;

            MotivoRetornoInscricaoId = Convert.ToInt32(e.Keys["MAE_MOTIVODESLIGAMENTOID"]);

            validacao = rnMaeMotivoDesligamento.ValidaRemocao(MotivoRetornoInscricaoId);

            if (validacao.Valido)
            {
                rnMaeMotivoDesligamento.Remove(MotivoRetornoInscricaoId);
                grdMaeMotivoDesligamento.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

    }
}
