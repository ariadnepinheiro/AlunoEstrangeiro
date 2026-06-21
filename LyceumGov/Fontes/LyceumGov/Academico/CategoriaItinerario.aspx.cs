using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;

using Techne.Web;
using DevExpress.Web.ASPxGridView;


namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/CategoriaItinerario.aspx")]
    [ControlText("CategoriaItinerario")]
    [Title("Categoria Itinerário")]

    public partial class CategoriaItinerario : TPage
    {
        public object Lista()
        {
            RN.Pedagogico.CategoriaItinerarioFormativo rnCategoriaItinerarioFormativo = new Techne.Lyceum.RN.Pedagogico.CategoriaItinerarioFormativo();

            return rnCategoriaItinerarioFormativo.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object CATEGORIAITINERARIOFORMATIVOID) { }
        public void Delete(object CATEGORIAITINERARIOFORMATIVOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdCategoria, "Categoria Itinerário");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdCategoria);
        }

        protected void grdCategoria_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdCategoria);
        }

        protected void grdCategoria_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdCategoria.Settings.ShowFilterRow = false;
        }

        protected void grdCategoria_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdCategoria.Settings.ShowFilterRow = false;
        }

        protected void grdCategoria_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Pedagogico.Entidades.CategoriaItinerarioFormativo categoria = new Techne.Lyceum.RN.Pedagogico.Entidades.CategoriaItinerarioFormativo();
            RN.Pedagogico.CategoriaItinerarioFormativo rnCategoriaItinerarioFormativo = new Techne.Lyceum.RN.Pedagogico.CategoriaItinerarioFormativo();

            categoria.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            categoria.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            categoria.UsuarioId = User.Identity.Name;

            validacao = rnCategoriaItinerarioFormativo.Valida(categoria, true);

            if (validacao.Valido)
            {
                rnCategoriaItinerarioFormativo.Insere(categoria);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdCategoria.DataBind();

        }

        protected void grdCategoria_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Pedagogico.Entidades.CategoriaItinerarioFormativo categoria = new Techne.Lyceum.RN.Pedagogico.Entidades.CategoriaItinerarioFormativo();
            RN.Pedagogico.CategoriaItinerarioFormativo rnCategoriaItinerarioFormativo = new Techne.Lyceum.RN.Pedagogico.CategoriaItinerarioFormativo();

            categoria.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            categoria.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            categoria.UsuarioId = User.Identity.Name;
            categoria.CategoriaItinerarioFormativoId = Convert.ToInt32(e.Keys["CATEGORIAITINERARIOFORMATIVOID"]);


            validacao = rnCategoriaItinerarioFormativo.Valida(categoria, true);

            if (validacao.Valido)
            {
                rnCategoriaItinerarioFormativo.Atualiza(categoria);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdCategoria.DataBind();
        }

        protected void grdCategoria_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Pedagogico.CategoriaItinerarioFormativo rnCategoriaItinerarioFormativo = new Techne.Lyceum.RN.Pedagogico.CategoriaItinerarioFormativo();

            int categoriaId = 0;

            categoriaId = Convert.ToInt32(e.Keys["CATEGORIAITINERARIOFORMATIVOID"]);

            validacao = rnCategoriaItinerarioFormativo.ValidaRemocao(categoriaId);

            if (validacao.Valido)
            {
                rnCategoriaItinerarioFormativo.Remove(categoriaId);
                grdCategoria.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
