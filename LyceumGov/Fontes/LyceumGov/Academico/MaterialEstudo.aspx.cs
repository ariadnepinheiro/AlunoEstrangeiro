using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using Techne.Web;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/MaterialEstudo.aspx"),
    ControlText("MaterialEstudo"),
    Title("Material de Estudo"),]
    public partial class MaterialEstudo : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public object Lista()
        {
            Techne.Lyceum.RN.LancamentoNotas.MaterialEstudo rnMaterialEstudo = new Techne.Lyceum.RN.LancamentoNotas.MaterialEstudo();

            return rnMaterialEstudo.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object MATERIALESTUDOID) { }
        public void Delete(object MATERIALESTUDOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdMaterialEstudo, "Material Estudo");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdMaterialEstudo);
        }

        protected void grdMaterialEstudo_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdMaterialEstudo.Settings.ShowFilterRow = false;
        }

        protected void grdMaterialEstudo_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdMaterialEstudo.Settings.ShowFilterRow = false;
        }

        protected void grdMaterialEstudo_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            Techne.Lyceum.RN.LancamentoNotas.Entidades.MaterialEstudo MaterialEstudoInscricao = new Techne.Lyceum.RN.LancamentoNotas.Entidades.MaterialEstudo();
            Techne.Lyceum.RN.LancamentoNotas.MaterialEstudo rnMaterialEstudo = new Techne.Lyceum.RN.LancamentoNotas.MaterialEstudo();

            MaterialEstudoInscricao.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            MaterialEstudoInscricao.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            MaterialEstudoInscricao.UsuarioId = User.Identity.Name;

            validacao = rnMaterialEstudo.Valida(MaterialEstudoInscricao, true);

            if (validacao.Valido)
            {
                rnMaterialEstudo.Insere(MaterialEstudoInscricao);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMaterialEstudo.DataBind();

        }

        protected void grdMaterialEstudo_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            Techne.Lyceum.RN.LancamentoNotas.Entidades.MaterialEstudo MaterialEstudoInscricao = new Techne.Lyceum.RN.LancamentoNotas.Entidades.MaterialEstudo();
            Techne.Lyceum.RN.LancamentoNotas.MaterialEstudo rnMaterialEstudo = new Techne.Lyceum.RN.LancamentoNotas.MaterialEstudo();

            MaterialEstudoInscricao.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            MaterialEstudoInscricao.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            MaterialEstudoInscricao.UsuarioId = User.Identity.Name;
            MaterialEstudoInscricao.MaterialEstudoId = Convert.ToInt32(e.Keys["MATERIALESTUDOID"]);


            validacao = rnMaterialEstudo.Valida(MaterialEstudoInscricao, true);

            if (validacao.Valido)
            {
                rnMaterialEstudo.Atualiza(MaterialEstudoInscricao);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMaterialEstudo.DataBind();
        }

        protected void grdMaterialEstudo_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            Techne.Lyceum.RN.LancamentoNotas.MaterialEstudo rnMaterialEstudo = new Techne.Lyceum.RN.LancamentoNotas.MaterialEstudo();
            int MAterialEstudoInscricaoId = 0;

            MAterialEstudoInscricaoId = Convert.ToInt32(e.Keys["MATERIALESTUDOID"]);

            validacao = rnMaterialEstudo.ValidaRemocao(MAterialEstudoInscricaoId);

            if (validacao.Valido)
            {
                rnMaterialEstudo.Remove(MAterialEstudoInscricaoId);
                grdMaterialEstudo.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
