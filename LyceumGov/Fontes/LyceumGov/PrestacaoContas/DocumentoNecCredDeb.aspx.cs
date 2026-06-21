using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Matriculas;
using Techne.Web;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [NavUrl("~/PrestacaoContas/PrestacaoContas.aspx")]
    [ControlText("Documentos Créditos e Débitos")]
    [Title("Documentos Créditos e Débitos")]

    public partial class DocumentoNecCredDeb : TPage
    {
        public object Lista()
        {
            RN.PrestacaoContas.DocumentoNecCredDeb rnDocumentoNecCredDeb = new Techne.Lyceum.RN.PrestacaoContas.DocumentoNecCredDeb();

            return rnDocumentoNecCredDeb.Lista();
        }

        public void Insert(object DESCRICAO, object DATACADASTRO, object ATIVO) { }
        public void Update(object DESCRICAO, object DATACADASTRO, object ATIVO, object DOCUMENTOSNECESSARIOSOPERACOESID) { }
        public void Delete(object DOCUMENTOSNECESSARIOSOPERACOESID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTipoVeiculo, "Documentos Necessários para Operações Créditos e Débitos");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTipoVeiculo);
        }

        protected void grdTipoVeiculo_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTipoVeiculo.Settings.ShowFilterRow = false;
        }

        protected void grdTipoVeiculo_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdTipoVeiculo.Settings.ShowFilterRow = false;
        }

        protected void grdTipoVeiculo_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.DocumentoNecCredDeb documentoNecCredDeb = new Techne.Lyceum.RN.PrestacaoContas.Entidades.DocumentoNecCredDeb();
            RN.PrestacaoContas.DocumentoNecCredDeb rnDocumentoNecCredDeb = new Techne.Lyceum.RN.PrestacaoContas.DocumentoNecCredDeb();

            documentoNecCredDeb.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            documentoNecCredDeb.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            documentoNecCredDeb.UsuarioId = User.Identity.Name;

            validacao = rnDocumentoNecCredDeb.Valida(documentoNecCredDeb, true);

            if (validacao.Valido)
            {
                rnDocumentoNecCredDeb.Insere(documentoNecCredDeb);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoVeiculo.DataBind();

        }

        protected void grdTipoVeiculo_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.DocumentoNecCredDeb documentoNecCredDeb = new Techne.Lyceum.RN.PrestacaoContas.Entidades.DocumentoNecCredDeb();
            RN.PrestacaoContas.DocumentoNecCredDeb rnDocumentoNecCredDeb = new Techne.Lyceum.RN.PrestacaoContas.DocumentoNecCredDeb();

            documentoNecCredDeb.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            documentoNecCredDeb.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            documentoNecCredDeb.UsuarioId = User.Identity.Name;
            documentoNecCredDeb.DocumentosNecessariosOperacoesID = Convert.ToInt32(e.Keys["DOCUMENTOSNECESSARIOSOPERACOESID"]);


            validacao = rnDocumentoNecCredDeb.Valida(documentoNecCredDeb, true);

            if (validacao.Valido)
            {
                rnDocumentoNecCredDeb.Atualiza(documentoNecCredDeb);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoVeiculo.DataBind();
        }

        protected void grdTipoVeiculo_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.DocumentoNecCredDeb rnDocumentoNecCredDeb = new Techne.Lyceum.RN.PrestacaoContas.DocumentoNecCredDeb();
            int documentosNecessariosOperacoesID = 0;

            documentosNecessariosOperacoesID = Convert.ToInt32(e.Keys["DOCUMENTOSNECESSARIOSOPERACOESID"]);

            validacao = rnDocumentoNecCredDeb.ValidaRemocao(documentosNecessariosOperacoesID);

            if (validacao.Valido)
            {
                rnDocumentoNecCredDeb.Remove(documentosNecessariosOperacoesID);
                grdTipoVeiculo.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
