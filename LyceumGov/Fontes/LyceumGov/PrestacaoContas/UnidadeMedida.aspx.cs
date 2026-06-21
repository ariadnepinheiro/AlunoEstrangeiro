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


namespace Techne.Lyceum.Net.PrestacaoContas
{
    [
         NavUrl("~/PrestacaoContas/UnidadeMedida.aspx"),
         ControlText("UnidadeMedida"),
         Title("Unidade Medida")
     ]
    public partial class UnidadeMedida : TPage
    {
        public object Lista()
        {
            RN.PrestacaoContas.UnidadeMedida rnUnidadeMedida = new Techne.Lyceum.RN.PrestacaoContas.UnidadeMedida();

            return rnUnidadeMedida.Lista();

        }

        public void Insert(object DESCRICAO,object SIGLA, object ATIVO) { }
        public void Update(object DESCRICAO,object SIGLA, object ATIVO, object UNIDADEMEDIDAID) { }
        public void Delete(object UNIDADEMEDIDAID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdUnidadeMedida, "Unidade Medida");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdUnidadeMedida);
        }

        protected void grdUnidadeMedida_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdUnidadeMedida);
        }		
        
        protected void grdUnidadeMedida_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdUnidadeMedida.Settings.ShowFilterRow = false;
        }

        protected void grdUnidadeMedida_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdUnidadeMedida.Settings.ShowFilterRow = false;
        }

        protected void grdUnidadeMedida_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.UnidadeMedida unidadeMedida = new Techne.Lyceum.RN.PrestacaoContas.Entidades.UnidadeMedida();
            RN.PrestacaoContas.UnidadeMedida rnUnidadeMedida = new RN.PrestacaoContas.UnidadeMedida();

            unidadeMedida.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            unidadeMedida.Sigla = e.NewValues["SIGLA"] != null ? e.NewValues["SIGLA"].ToString().Trim().ToUpper() : null;
            unidadeMedida.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            unidadeMedida.UsuarioId = User.Identity.Name;

            validacao = rnUnidadeMedida.Valida(unidadeMedida, true);

            if (validacao.Valido)
            {
                rnUnidadeMedida.Insere(unidadeMedida);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdUnidadeMedida.DataBind();

        }

        protected void grdUnidadeMedida_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.UnidadeMedida unidadeMedida = new Techne.Lyceum.RN.PrestacaoContas.Entidades.UnidadeMedida();
            RN.PrestacaoContas.UnidadeMedida rnUnidadeMedida = new RN.PrestacaoContas.UnidadeMedida();

            unidadeMedida.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            unidadeMedida.Sigla = e.NewValues["SIGLA"] != null ? e.NewValues["SIGLA"].ToString().Trim().ToUpper() : null;
            unidadeMedida.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            unidadeMedida.UnidadeMedidaId = Convert.ToInt32(e.Keys["UNIDADEMEDIDAID"]);
            unidadeMedida.UsuarioId = User.Identity.Name;

            validacao = rnUnidadeMedida.Valida(unidadeMedida, true);

            if (validacao.Valido)
            {
                rnUnidadeMedida.Atualiza(unidadeMedida);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdUnidadeMedida.DataBind();
        }

        protected void grdUnidadeMedida_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.UnidadeMedida rnUnidadeMedida = new RN.PrestacaoContas.UnidadeMedida();
            int unidadeMedidaId = 0;

            unidadeMedidaId = Convert.ToInt32(e.Keys["UNIDADEMEDIDAID"]);

            validacao = rnUnidadeMedida.ValidaRemocao(unidadeMedidaId);

            if (validacao.Valido)
            {
                rnUnidadeMedida.Remove(unidadeMedidaId);
                grdUnidadeMedida.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

      
       
    }
}
