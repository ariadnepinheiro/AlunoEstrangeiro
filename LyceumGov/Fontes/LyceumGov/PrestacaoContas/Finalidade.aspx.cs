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
         NavUrl("~/PrestacaoContas/Finalidade.aspx"),
         ControlText("Finalidade"),
         Title("Finalidade")
     ]
    public partial class Finalidade : TPage
    {
        public object Lista()
        {
            RN.PrestacaoContas.Finalidade rnFinalidade = new Techne.Lyceum.RN.PrestacaoContas.Finalidade();

            return rnFinalidade.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object FINALIDADEID) { }
        public void Delete(object FINALIDADEID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdFinalidade, "Finalidade");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdFinalidade);
        }

        protected void grdFinalidade_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdFinalidade);
        }		
        
        protected void grdFinalidade_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdFinalidade.Settings.ShowFilterRow = false;
        }

        protected void grdFinalidade_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdFinalidade.Settings.ShowFilterRow = false;
        }

        protected void grdFinalidade_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.Finalidade documento = new Techne.Lyceum.RN.PrestacaoContas.Entidades.Finalidade();
            RN.PrestacaoContas.Finalidade rnFinalidade = new RN.PrestacaoContas.Finalidade();

            documento.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            documento.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            documento.UsuarioId = User.Identity.Name;

            validacao = rnFinalidade.Valida(documento, true);

            if (validacao.Valido)
            {
                rnFinalidade.Insere(documento);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdFinalidade.DataBind();

        }

        protected void grdFinalidade_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.Finalidade documento = new Techne.Lyceum.RN.PrestacaoContas.Entidades.Finalidade();
            RN.PrestacaoContas.Finalidade rnFinalidade = new RN.PrestacaoContas.Finalidade();

            documento.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            documento.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            documento.FinalidadeId = Convert.ToInt32(e.Keys["FINALIDADEID"]);
            documento.UsuarioId = User.Identity.Name;

            validacao = rnFinalidade.Valida(documento, true);

            if (validacao.Valido)
            {
                rnFinalidade.Atualiza(documento);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdFinalidade.DataBind();
        }

        protected void grdFinalidade_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Finalidade rnFinalidade = new RN.PrestacaoContas.Finalidade();
            int finalidadeId = 0;

            finalidadeId = Convert.ToInt32(e.Keys["FINALIDADEID"]);

            validacao = rnFinalidade.ValidaRemocao(finalidadeId);

            if (validacao.Valido)
            {
                rnFinalidade.Remove(finalidadeId);
                grdFinalidade.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

      
       
    }
}
