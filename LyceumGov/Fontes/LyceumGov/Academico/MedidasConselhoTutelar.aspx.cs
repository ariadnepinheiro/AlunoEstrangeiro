using System;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/MedidasConselhoTutelar.aspx")]
    [ControlText("Medidas Adotadas pelo Conselho Tutelar")]
    [Title("Medidas Adotadas pelo Conselho Tutelar")]

    public partial class MedidasConselhoTutelar : TPage
    {
        public object Lista()
        {
            RN.Turmas.MedidaConselhoTutelar rnMedidasConselhoTutelar = new Techne.Lyceum.RN.Turmas.MedidaConselhoTutelar();

            return rnMedidasConselhoTutelar.Lista();
        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object MEDIDACONSELHOTUTELARID) { }
        public void Delete(object MEDIDACONSELHOTUTELARID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdMedidasConselhoTutelar, "Cadastro de Medidas Adotadas pelo Conselho Tutelar");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdMedidasConselhoTutelar);
        }

        protected void grdMedidasConselhoTutelar_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdMedidasConselhoTutelar);
        }

        protected void grdMedidasConselhoTutelar_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdMedidasConselhoTutelar.Settings.ShowFilterRow = false;
        }

        protected void grdMedidasConselhoTutelar_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdMedidasConselhoTutelar.Settings.ShowFilterRow = false;
        }

        protected void grdMedidasConselhoTutelar_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Turmas.Entidades.MedidaConselhoTutelar medida = new Techne.Lyceum.RN.Turmas.Entidades.MedidaConselhoTutelar();
            RN.Turmas.MedidaConselhoTutelar rnMedidasConselhoTutelar = new Techne.Lyceum.RN.Turmas.MedidaConselhoTutelar();

            medida.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            medida.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            medida.UsuarioId = User.Identity.Name;

            validacao = rnMedidasConselhoTutelar.Valida(medida, true);

            if (validacao.Valido)
            {
                rnMedidasConselhoTutelar.Insere(medida);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMedidasConselhoTutelar.DataBind();

        }

        protected void grdMedidasConselhoTutelar_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Turmas.Entidades.MedidaConselhoTutelar medida = new Techne.Lyceum.RN.Turmas.Entidades.MedidaConselhoTutelar();
            RN.Turmas.MedidaConselhoTutelar rnMedidasConselhoTutelar = new Techne.Lyceum.RN.Turmas.MedidaConselhoTutelar();

            medida.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            medida.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            medida.UsuarioId = User.Identity.Name;
            medida.MedidasConselhoTutelarId = Convert.ToInt32(e.Keys["MEDIDACONSELHOTUTELARID"]);

            validacao = rnMedidasConselhoTutelar.Valida(medida, true);

            if (validacao.Valido)
            {
                rnMedidasConselhoTutelar.Atualiza(medida);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMedidasConselhoTutelar.DataBind();
        }

        protected void grdMedidasConselhoTutelar_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Turmas.MedidaConselhoTutelar rnMedidasConselhoTutelar = new Techne.Lyceum.RN.Turmas.MedidaConselhoTutelar();
            int Id = 0;

            Id = Convert.ToInt32(e.Keys["MEDIDACONSELHOTUTELARID"]);

            validacao = rnMedidasConselhoTutelar.ValidaRemocao(Id);

            if (validacao.Valido)
            {
                rnMedidasConselhoTutelar.Remove(Id);
                grdMedidasConselhoTutelar.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

    }
}
