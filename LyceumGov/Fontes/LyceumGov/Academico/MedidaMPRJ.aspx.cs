using System;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/MedidaMPRJ.aspx")]
    [ControlText("Medidas Adotadas pelo MPRJ")]
    [Title("Medidas Adotadas pelo MPRJ")]

    public partial class MedidaMPRJ : TPage
    {
        public object Lista()
        {
            RN.Turmas.MedidaMPRJ rnMedidaMPRJ = new Techne.Lyceum.RN.Turmas.MedidaMPRJ();

            return rnMedidaMPRJ.Lista();
        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object MEDIDAMPRJID) { }
        public void Delete(object MEDIDAMPRJID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdMedidaMPRJ, "Cadastro de Medidas Adotadas pelo MPRJ");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdMedidaMPRJ);
        }

        protected void grdMedidaMPRJ_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdMedidaMPRJ);
        }

        protected void grdMedidaMPRJ_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdMedidaMPRJ.Settings.ShowFilterRow = false;
        }

        protected void grdMedidaMPRJ_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdMedidaMPRJ.Settings.ShowFilterRow = false;
        }

        protected void grdMedidaMPRJ_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Turmas.Entidades.MedidaMPRJ medida = new Techne.Lyceum.RN.Turmas.Entidades.MedidaMPRJ();
            RN.Turmas.MedidaMPRJ rnMedidaMPRJ = new Techne.Lyceum.RN.Turmas.MedidaMPRJ();

            medida.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            medida.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            medida.UsuarioId = User.Identity.Name;

            validacao = rnMedidaMPRJ.Valida(medida, true);

            if (validacao.Valido)
            {
                rnMedidaMPRJ.Insere(medida);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMedidaMPRJ.DataBind();

        }

        protected void grdMedidaMPRJ_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Turmas.Entidades.MedidaMPRJ medida = new Techne.Lyceum.RN.Turmas.Entidades.MedidaMPRJ();
            RN.Turmas.MedidaMPRJ rnMedidaMPRJ = new Techne.Lyceum.RN.Turmas.MedidaMPRJ();

            medida.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            medida.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            medida.UsuarioId = User.Identity.Name;
            medida.MedidaMPRJId = Convert.ToInt32(e.Keys["MEDIDAMPRJID"]);

            validacao = rnMedidaMPRJ.Valida(medida, true);

            if (validacao.Valido)
            {
                rnMedidaMPRJ.Atualiza(medida);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMedidaMPRJ.DataBind();
        }

        protected void grdMedidaMPRJ_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Turmas.MedidaMPRJ rnMedidaMPRJ = new Techne.Lyceum.RN.Turmas.MedidaMPRJ();
            int Id = 0;

            Id = Convert.ToInt32(e.Keys["MEDIDAMPRJID"]);

            validacao = rnMedidaMPRJ.ValidaRemocao(Id);

            if (validacao.Valido)
            {
                rnMedidaMPRJ.Remove(Id);
                grdMedidaMPRJ.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

    }
}
