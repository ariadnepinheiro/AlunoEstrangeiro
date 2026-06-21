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
         NavUrl("~/PrestacaoContas/ProdutoServicoGrupo.aspx"),
         ControlText("ProdutoServicoGrupo"),
         Title("Grupo Produto Serviço ")
     ]
    public partial class ProdutoServicoGrupo : TPage
    {
        public object Lista()
        {
            RN.PrestacaoContas.ProdutoServicoGrupo rnProdutoServicoGrupo = new Techne.Lyceum.RN.PrestacaoContas.ProdutoServicoGrupo();

            return rnProdutoServicoGrupo.Lista();

        }

        public void Insert(object DESCRICAO,object CODIGOCNAE, object ATIVO) { }
        public void Update(object DESCRICAO,object CODIGOCNAE, object ATIVO, object PRODUTOSERVICOGRUPOID) { }
        public void Delete(object PRODUTOSERVICOGRUPOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdProdutoServicoGrupo, "Grupo Produto Serviço");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdProdutoServicoGrupo);
        }

        protected void grdProdutoServicoGrupo_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdProdutoServicoGrupo);
        }		


        protected void grdProdutoServicoGrupo_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdProdutoServicoGrupo.Settings.ShowFilterRow = false;
        }

        protected void grdProdutoServicoGrupo_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdProdutoServicoGrupo.Settings.ShowFilterRow = false;
        }

        protected void grdProdutoServicoGrupo_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.ProdutoServicoGrupo grupo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ProdutoServicoGrupo();
            RN.PrestacaoContas.ProdutoServicoGrupo rnProdutoServicoGrupo = new RN.PrestacaoContas.ProdutoServicoGrupo();

            grupo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            grupo.CodigoCnae = e.NewValues["CODIGOCNAE"] != null ? e.NewValues["CODIGOCNAE"].ToString().Trim() : null;
            grupo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            grupo.UsuarioId = User.Identity.Name;

            validacao = rnProdutoServicoGrupo.Valida(grupo, true);

            if (validacao.Valido)
            {
                rnProdutoServicoGrupo.Insere(grupo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdProdutoServicoGrupo.DataBind();

        }

        protected void grdProdutoServicoGrupo_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.ProdutoServicoGrupo grupo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ProdutoServicoGrupo();
            RN.PrestacaoContas.ProdutoServicoGrupo rnProdutoServicoGrupo = new RN.PrestacaoContas.ProdutoServicoGrupo();

            grupo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            grupo.CodigoCnae = e.NewValues["CODIGOCNAE"] != null ? e.NewValues["CODIGOCNAE"].ToString().Trim() : null;
            grupo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            grupo.ProdutoServicoGrupoId = Convert.ToInt32(e.Keys["PRODUTOSERVICOGRUPOID"]);
            grupo.UsuarioId = User.Identity.Name;

            validacao = rnProdutoServicoGrupo.Valida(grupo, true);

            if (validacao.Valido)
            {
                rnProdutoServicoGrupo.Atualiza(grupo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdProdutoServicoGrupo.DataBind();
        }

        protected void grdProdutoServicoGrupo_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.ProdutoServicoGrupo rnProdutoServicoGrupo = new RN.PrestacaoContas.ProdutoServicoGrupo();
            int grupoId = 0;

            grupoId = Convert.ToInt32(e.Keys["PRODUTOSERVICOGRUPOID"]);

            validacao = rnProdutoServicoGrupo.ValidaRemocao(grupoId);

            if (validacao.Valido)
            {
                rnProdutoServicoGrupo.Remove(grupoId);
                grdProdutoServicoGrupo.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

   
    }
}
