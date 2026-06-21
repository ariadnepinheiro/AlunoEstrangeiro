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
         NavUrl("~/GestaoRede/Subsecretaria.aspx"),
         ControlText("Subsecretaria"),
         Title("Subsecretaria")
     ]

    public partial class Subsecretaria : TPage
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
            RN.GestaoRede.Subsecretaria rnSubsecretaria = new Techne.Lyceum.RN.GestaoRede.Subsecretaria();

            return rnSubsecretaria.ListaSubsecretaria();

        }


        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdSubsecretaria, string.Empty);
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

            ControlaAcesso(grdSubsecretaria);
            ControlaAcesso(grdSubsecretaria, AcaoControle.editar, "btnEditar");
            ControlaAcesso(grdSubsecretaria, AcaoControle.excluir, "btnDeletar");
            
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
            hdnSubsecretariaId.Value = string.Empty;
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

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Inicial;

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
            RN.GestaoRede.Entidades.Subsecretaria subsecretaria = new Techne.Lyceum.RN.GestaoRede.Entidades.Subsecretaria();
            RN.GestaoRede.Subsecretaria rnSubsecretaria = new RN.GestaoRede.Subsecretaria();
            try
            {
                subsecretaria.Descricao = !txtDescricao.Text.IsNullOrEmptyOrWhiteSpace() ? txtDescricao.Text.Trim() : null;
                subsecretaria.Setor = (tseSetor.IsValidDBValue && !tseSetor.DBValue.IsNull) ? tseSetor["setor"].ToString() : null;
                subsecretaria.Ativo = chkAtivo.Checked ? true : false;
                subsecretaria.UsuarioId = User.Identity.Name;
                subsecretaria.SubsecretariaId = hdnSubsecretariaId.Value.IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToInt32(hdnSubsecretariaId.Value);

                validacao = rnSubsecretaria.Valida(subsecretaria, true);

                if (validacao.Valido)
                {
                    if (subsecretaria.SubsecretariaId == 0)
                    {
                        rnSubsecretaria.Insere(subsecretaria);
                        lblMensagem.Text = "Subsecretaria incluída com sucesso.";
                    }
                    else
                    {
                        rnSubsecretaria.Atualiza(subsecretaria);
                        lblMensagem.Text = "Subsecretaria atualizada com sucesso.";
                    }
                    
                    grdSubsecretaria.DataBind();

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
                grdSubsecretaria.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            RN.GestaoRede.Subsecretaria rnSubsecretaria = new Techne.Lyceum.RN.GestaoRede.Subsecretaria();
            ValidacaoDados validacao = new ValidacaoDados();

            try
            {
                int subsecretariaId = Convert.ToInt32(hdnSubsecretariaId.Value);

                validacao = rnSubsecretaria.ValidaRemocao(subsecretariaId);

                if (validacao.Valido)
                {
                    rnSubsecretaria.Remove(subsecretariaId);
                    LimparCampos();
                    grdSubsecretaria.DataBind();
                    lblMensagem.Text = "Subsecretaria excluída com sucesso.";
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

        protected void grdSubsecretaria_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdSubsecretaria);
        }

        protected void grdSubsecretaria_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdSubsecretaria.Settings.ShowFilterRow = false;
        }

        protected void grdSubsecretaria_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdSubsecretaria.Settings.ShowFilterRow = false;
        }

        protected void grdSubsecretaria_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            btnCancelarAtualizacao.Visible = false;

            string descricao = grdSubsecretaria.GetRowValues(e.VisibleIndex, "DESCRICAO").ToString();
            string setor = grdSubsecretaria.GetRowValues(e.VisibleIndex, "UA_ATUAL").ToString();
            string ativo = grdSubsecretaria.GetRowValues(e.VisibleIndex, "ATIVO").ToString();

            string subsecretariaId = grdSubsecretaria.GetRowValues(e.VisibleIndex, "SUBSECRETARIAID").ToString();

            if (e.ButtonID == "btnEditar")
            {
                LimparCampos();
                hdnSubsecretariaId.Value = subsecretariaId;
                txtDescricao.Text = descricao;
                tseSetor.DBValue = setor;
                chkAtivo.Checked = Convert.ToBoolean(ativo) ;
                        
                btnCancelarAtualizacao.Visible = btnSalvar.Visible = true;
                pnDados.Visible = true;

                ControlaAcesso(btnSalvar, AcaoControle.editar);
            }
            if (e.ButtonID == "btnDeletar")
            {
                hdnSubsecretariaId.Value = subsecretariaId;
                popup.ShowOnPageLoad = true;
            }
        }





    }
}
