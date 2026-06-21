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


namespace Techne.Lyceum.Net.GestaoRede
{
    [
         NavUrl("~/GestaoRede/Superintendencia.aspx"),
         ControlText("Superintendencia"),
         Title("Superintendência")
     ]

    public partial class Superintendencia : TPage
    {
        public enum TipoOperacao
        {
            Novo,
            Excluir,
            Alterar,
            Consultar,
            Inicial
        }

        private TipoOperacao _tipoOperacao
        {
            get
            {
                if (ViewState["_tipoOperacao"] != null)
                {
                    if (ViewState["_tipoOperacao"] is TipoOperacao)
                    {
                        return (TipoOperacao)ViewState["_tipoOperacao"];
                    }
                }

                return TipoOperacao.Inicial;
            }

            set
            {
                ViewState["_tipoOperacao"] = value;
            }
        }

        public object Lista()
        {
            RN.GestaoRede.Superintendencia rnSuperintendencia = new Techne.Lyceum.RN.GestaoRede.Superintendencia();

            return rnSuperintendencia.ListaSuperintendencia();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdSuperintendencia, string.Empty);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty; 

                if (!IsPostBack)
                {

                    this._tipoOperacao = TipoOperacao.Inicial;
                    this.ControlarTipoOperacao();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;

            }
        }


        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdSuperintendencia);
            ControlaAcesso(grdSuperintendencia, AcaoControle.editar, "btnEditar");
            ControlaAcesso(grdSuperintendencia, AcaoControle.excluir, "btnDeletar");

            ControlaAcesso(btnExcluir, AcaoControle.excluir);
            ControlaAcesso(btnNovo, AcaoControle.novo);
        }

        private void ControlarTipoOperacao()
        {
            switch (this._tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {                        
                        LimparCampos();
                        pnDados.Visible = false;
                        break;
                    }
                case TipoOperacao.Novo:
                    {                       
                        LimparCampos();
                        pnDados.Visible = true;
                        btnSalvar.Visible = true;
                        ControlaAcesso(btnSalvar, AcaoControle.novo);
                        break;
                    }
            }
        }

        private void LimparCampos()
        {
            hdnSuperintendenciaId.Value = string.Empty;
            tseSubsecretaria.ResetValue();
            tseSetor.ResetValue();
            txtDescricao.Text = string.Empty;
            chkAtivo.Checked = false;
        }


        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Novo;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }        

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.GestaoRede.Entidades.Superintendencia superintendencia = new Techne.Lyceum.RN.GestaoRede.Entidades.Superintendencia();
            RN.GestaoRede.Superintendencia rnSuperintendencia = new RN.GestaoRede.Superintendencia();
            try
            {
                superintendencia.Descricao = !txtDescricao.Text.IsNullOrEmptyOrWhiteSpace() ? txtDescricao.Text.Trim() : null;
                superintendencia.Setor = (tseSetor.IsValidDBValue && !tseSetor.DBValue.IsNull) ? tseSetor["setor"].ToString() : null;
                superintendencia.SubsecretariaId = (tseSubsecretaria.IsValidDBValue && !tseSubsecretaria.DBValue.IsNull) ? Convert.ToInt32(tseSubsecretaria.DBValue) : -1;
                superintendencia.Ativo = chkAtivo.Checked ? true : false;
                superintendencia.UsuarioId = User.Identity.Name;
                superintendencia.SuperintendenciaId = hdnSuperintendenciaId.Value.IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToInt32(hdnSuperintendenciaId.Value);

                bool cadastro = superintendencia.SuperintendenciaId == 0;
                validacao = rnSuperintendencia.Valida(superintendencia, cadastro);

                if (validacao.Valido)
                {
                    if (cadastro)
                    {
                        rnSuperintendencia.Insere(superintendencia);
                        lblMensagem.Text = "Superintendência incluída com sucesso.";
                    }
                    else
                    {
                        rnSuperintendencia.Atualiza(superintendencia);
                        lblMensagem.Text = "Superintendência atualizada com sucesso.";
                    }
                    
                    grdSuperintendencia.DataBind();

                    this._tipoOperacao = TipoOperacao.Inicial;

                    this.ControlarTipoOperacao();
                  
                }
                else
                {
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnCancelarAtualizacao_Click(object sender, EventArgs e)
        {
            try
            {
                LimparCampos();              
                grdSuperintendencia.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            RN.GestaoRede.Superintendencia rnSuperintendencia = new Techne.Lyceum.RN.GestaoRede.Superintendencia();
            ValidacaoDados validacao = new ValidacaoDados();

            try
            {
                int superintendenciaId = Convert.ToInt32(hdnSuperintendenciaId.Value);

                validacao = rnSuperintendencia.ValidaRemocao(superintendenciaId);

                if (validacao.Valido)
                {
                    rnSuperintendencia.Remove(superintendenciaId);
                    LimparCampos();
                    grdSuperintendencia.DataBind();
                    lblMensagem.Text = "Superintendência excluída com sucesso.";
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdSuperintendencia_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdSuperintendencia);
        }

        protected void grdSuperintendencia_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdSuperintendencia.Settings.ShowFilterRow = false;
        }

        protected void grdSuperintendencia_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdSuperintendencia.Settings.ShowFilterRow = false;
        }

        protected void grdSuperintendencia_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            btnCancelarAtualizacao.Visible = false;

            string descricao = grdSuperintendencia.GetRowValues(e.VisibleIndex, "DESCRICAO").ToString();
            string setor = grdSuperintendencia.GetRowValues(e.VisibleIndex, "UA_ATUAL").ToString();
            string ativo = grdSuperintendencia.GetRowValues(e.VisibleIndex, "ATIVO").ToString();
            string superintendenciaId = grdSuperintendencia.GetRowValues(e.VisibleIndex, "SUPERINTENDENCIAID").ToString();
            string subsecretariaId = grdSuperintendencia.GetRowValues(e.VisibleIndex, "SUBSECRETARIAID").ToString();

            if (e.ButtonID == "btnEditar")
            {
                LimparCampos();
                hdnSuperintendenciaId.Value = superintendenciaId;
                txtDescricao.Text = descricao;
                tseSetor.DBValue = setor;
                tseSubsecretaria.DBValue = subsecretariaId;
                chkAtivo.Checked = Convert.ToBoolean(ativo) ;

                btnCancelarAtualizacao.Visible = btnSalvar.Visible = true;
                pnDados.Visible = true;

                ControlaAcesso(btnSalvar, AcaoControle.editar);
            }
            if (e.ButtonID == "btnDeletar")
            {
                hdnSuperintendenciaId.Value = superintendenciaId;
                popup.ShowOnPageLoad = true;
            }
        }





    }
}
