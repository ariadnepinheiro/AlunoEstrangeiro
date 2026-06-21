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
         NavUrl("~/PrestacaoContas/DocumentosNecessariosFornecedor.aspx"),
         ControlText("DocumentosNecessariosFornecedor"),
         Title("Documentos Necessários Fornecedor")
     ]
    public partial class DocumentosNecessariosFornecedor : TPage
    {
        public object Lista()
        {
            RN.PrestacaoContas.DocumentosNecessariosFornecedor rnDocumentosNecessariosFornecedor = new Techne.Lyceum.RN.PrestacaoContas.DocumentosNecessariosFornecedor();

            return rnDocumentosNecessariosFornecedor.Lista();

        }

        public void Insert(object DESCRICAO, object PERIODICIDADE, object TIPO, object ATIVO, object DATAINICIO, object DATAFIM) { }
        public void Update(object DESCRICAO, object PERIODICIDADE, object TIPO, object ATIVO, object DATAINICIO, object DATAFIM, object DOCUMENTOSNECESSARIOSFORNECEDORID) { }
        public void Delete(object DOCUMENTOSNECESSARIOSFORNECEDORID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdDocumentosNecessariosFornecedor, "Documento Necessário Fornecedor");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdDocumentosNecessariosFornecedor);
        }

        protected void grdDocumentosNecessariosFornecedor_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdDocumentosNecessariosFornecedor);
        }		


        protected void grdDocumentosNecessariosFornecedor_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdDocumentosNecessariosFornecedor.Settings.ShowFilterRow = false;
        }

        protected void grdDocumentosNecessariosFornecedor_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdDocumentosNecessariosFornecedor.Settings.ShowFilterRow = false;
        }

        protected void grdDocumentosNecessariosFornecedor_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.DocumentosNecessariosFornecedor documento = new Techne.Lyceum.RN.PrestacaoContas.Entidades.DocumentosNecessariosFornecedor();
            RN.PrestacaoContas.DocumentosNecessariosFornecedor rnDocumentosNecessariosFornecedor = new RN.PrestacaoContas.DocumentosNecessariosFornecedor();

            documento.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            documento.Periodicidade = e.NewValues["PERIODICIDADE"] != null ? Convert.ToInt32(e.NewValues["PERIODICIDADE"]) : -1;
            documento.Tipo = e.NewValues["TIPO"] != null ? Convert.ToString(e.NewValues["TIPO"]) : null;
            documento.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            documento.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            documento.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            documento.UsuarioId = User.Identity.Name;

            validacao = rnDocumentosNecessariosFornecedor.Valida(documento, true);

            if (validacao.Valido)
            {
                rnDocumentosNecessariosFornecedor.Insere(documento);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdDocumentosNecessariosFornecedor.DataBind();

        }

        protected void grdDocumentosNecessariosFornecedor_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.DocumentosNecessariosFornecedor documento = new Techne.Lyceum.RN.PrestacaoContas.Entidades.DocumentosNecessariosFornecedor();
            RN.PrestacaoContas.DocumentosNecessariosFornecedor rnDocumentosNecessariosFornecedor = new RN.PrestacaoContas.DocumentosNecessariosFornecedor();

            documento.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            documento.Periodicidade = e.NewValues["PERIODICIDADE"] != null ? Convert.ToInt32(e.NewValues["PERIODICIDADE"]) : -1;
            documento.Tipo = e.NewValues["TIPO"] != null ? Convert.ToString(e.NewValues["TIPO"]) : null;
            documento.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            documento.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            documento.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            documento.DocumentosNecessariosFornecedorId = Convert.ToInt32(e.Keys["DOCUMENTOSNECESSARIOSFORNECEDORID"]);
            documento.UsuarioId = User.Identity.Name;

            validacao = rnDocumentosNecessariosFornecedor.Valida(documento, true);

            if (validacao.Valido)
            {
                rnDocumentosNecessariosFornecedor.Atualiza(documento);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdDocumentosNecessariosFornecedor.DataBind();
        }

        protected void grdDocumentosNecessariosFornecedor_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.DocumentosNecessariosFornecedor rnDocumentosNecessariosFornecedor = new RN.PrestacaoContas.DocumentosNecessariosFornecedor();
            int documentoId = 0;

            documentoId = Convert.ToInt32(e.Keys["DOCUMENTOSNECESSARIOSFORNECEDORID"]);

            validacao = rnDocumentosNecessariosFornecedor.ValidaRemocao(documentoId);

            if (validacao.Valido)
            {
                rnDocumentosNecessariosFornecedor.Remove(documentoId);
                grdDocumentosNecessariosFornecedor.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

        protected void dtIni_Load(object sender, EventArgs e)
        {
            ASPxTextBox txtDataInicio = sender as ASPxTextBox;

            GridViewEditItemTemplateContainer cont = txtDataInicio.NamingContainer as GridViewEditItemTemplateContainer;
            if (string.IsNullOrEmpty(cont.Text.Replace("&nbsp;", string.Empty)))
            {
                txtDataInicio.Enabled = true;
            }
            else
            {
                txtDataInicio.Enabled = false;
            }
        }

        protected void dtFim_Load(object sender, EventArgs e)
        {
            ASPxTextBox txtDataFim = sender as ASPxTextBox;

            GridViewEditItemTemplateContainer cont = txtDataFim.NamingContainer as GridViewEditItemTemplateContainer;
            if (string.IsNullOrEmpty(cont.Text.Replace("&nbsp;", string.Empty)))
            {
                txtDataFim.Enabled = false;
            }
            else
            {
                txtDataFim.Enabled = true;
            }
        }
    }
}
